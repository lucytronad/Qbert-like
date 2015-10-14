using UnityEngine;
using System.Collections;
using System;

public class GameController : MonoBehaviour {

	private static GameController instance;
    public static GameController Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<GameController>();
                DontDestroyOnLoad(instance.gameObject);
            }
            return instance;
        }
    }

	public string m_levelName;

    private UIManager m_UIManager;

    private GameLogic m_gameLogic;
    public GameLogic M_GameLogic
    {
        get { return m_gameLogic; }
        set { m_gameLogic = value; }
    }

    private GameObject m_player;
    private GameObject m_targetCube;
    
    private bool m_gameEnded;

	void Awake()
	{
		if(instance == null)
		{
			instance = this;
			DontDestroyOnLoad(this);
		}
		else
		{
			if(this != instance)
			{
				Destroy(this.gameObject);
			}
		}
	}


	void Start ()
    {
        m_UIManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>();
        if(Application.loadedLevelName == "MainGame")
        {
            StartLevel();
        }
	}

    void OnLevelWasLoaded()
    {
        //TODO : check singleton pattern : OnLevelWasLoaded is called before destruction of GameController unwanted instance
        if (this == instance)
        {
            StartLevel();
        }
    }
	

	void Update ()
    {
        if (!m_gameEnded && Application.loadedLevelName == "MainGame")
        {
            //Checking if level is complete
            if (m_gameLogic.IsLevelComplete())
            {
                LevelWin();
            }

            //Checking if player is still alive
            if (m_gameLogic.M_PlayerLogic.M_IsDead)
            {
                LevelLose();
            }

            //Checking user input : player jump (left click)
            if (Input.GetButton("Jump"))
            {
                RaycastHit hitInfo;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hitInfo, 100.0f))
                {
                    //If click on a level block
                    if (hitInfo.collider != null && hitInfo.transform.gameObject.tag == "LevelBlock")
                    {
                        m_targetCube = hitInfo.transform.gameObject;
                    }
                }
                //If target acquired order jump
                if (m_targetCube != null && m_gameLogic.M_PlayerLogic.M_CubeLogic.M_Neighbors.Contains(m_targetCube.GetComponent<CubeController>().M_CubeLogic))
                {
                    m_player.GetComponent<PlayerController>().JumpAction(m_targetCube);
                }
            }
        }
	}

	public void StartLevel()
    {
        m_gameEnded = false;
        //Creating game logic
        m_gameLogic = new GameLogic(m_levelName);
        //Creating game graphic
        BuildLevel();
    }

    public void BuildLevel()
    {
        //Build the block
        foreach (CubeLogic cube in m_gameLogic.M_CubeLogics)
        {
            GameObject go = Instantiate(Resources.Load("level_block"), cube.M_InitialPosition, Quaternion.identity) as GameObject;
            go.GetComponent<CubeController>().Initialize(cube);
        }

        //Adding the player
        Collider[] colliders = Physics.OverlapSphere(m_gameLogic.M_PlayerLogic.M_CubeLogic.M_InitialPosition, 1f);
        if (colliders.Length == 1)
        {
            if (colliders[0].gameObject.tag == "LevelBlock")
            {
                GameObject currentPlayerCube = colliders[0].gameObject;
                m_player = Instantiate(Resources.Load("player"), m_gameLogic.M_PlayerLogic.M_InitialPosition, Quaternion.identity) as GameObject;
                m_player.GetComponent<PlayerController>().Initialize(m_gameLogic.M_PlayerLogic, currentPlayerCube);
            }
        }

        //Initialize camera position and rotation
        Camera.main.transform.position = m_gameLogic.M_CameraInitialPosition;
        Camera.main.transform.rotation = Quaternion.Euler(m_gameLogic.M_CameraInitialRotation);
    }

    //Method called when detection of level's end
	public void LevelWin()
	{
        Debug.Log("WIIIIIIIIIIIIIIIIIIN!");
        LevelEnd();
        
	}

    public void LevelLose()
    {
        Debug.Log("LOOOOOOOOOOOOOOOOOSE!");
        LevelEnd();
    }

    public void LevelEnd()
    {
        m_gameEnded = true;
        m_gameLogic.M_SpawnerLogics.Clear();
        m_UIManager.DisplayRestartScreen();
    }
}
