using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

	private static GameController instance;
	public string m_levelName;

    private GameLogic m_gameLogic;
    public GameLogic M_GameLogic
    {
        get { return m_gameLogic; }
        set { m_gameLogic = value; }
    }

    private GameObject m_player;
    private GameObject m_targetCube;
    private GameObject m_enemy;

	public static GameController Instance
	{
		get{
			if(instance == null)
			{
				instance = GameObject.FindObjectOfType<GameController>();
				DontDestroyOnLoad(instance.gameObject);
			}
			return instance;
		}
	}

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
        //Creating game logic
        m_gameLogic = new GameLogic(m_levelName);
        //Creating game graphic
        BuildLevel();
	}
	

	void Update ()
    {
        //Checking if level is complete
	    if(m_gameLogic.IsLevelComplete())
        {
            LevelWin();
        }

        //Checking user input : player jump (left click)
        if(Input.GetButton("Jump"))
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
            if (m_targetCube !=null && m_gameLogic.M_PlayerLogic.M_CubeLogic.M_Neighbors.Contains(m_targetCube.GetComponent<CubeController>().M_CubeLogic))
            {
                m_player.GetComponent<PlayerController>().JumpAction(m_targetCube);
            }
        }
	}

	public void OnLevelButtonClick(Text buttonText)
	{
		m_levelName = buttonText.text;
		Application.LoadLevel(1);
		/*catch(FormatException e)
		{
			Debug.LogError("Level string is not a sequence of digit!");
		}
		catch(OverflowException e)
		{
			Debug.LogError("The number cannot fit in an Int32.");
		}*/
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

        //Adding an enemy
        /*colliders = Physics.OverlapSphere(m_gameLogic.M_EnemyLogics[0].M_CubeLogic.M_InitialPosition, 1f);
        if (colliders.Length == 1)
        {
            if (colliders[0].gameObject.tag == "LevelBlock")
            {
                GameObject currentEnemyCube = colliders[0].gameObject;
                m_enemy = Instantiate(Resources.Load("enemy"), m_gameLogic.M_EnemyLogics[0].M_InitialPosition, Quaternion.identity) as GameObject;
                m_enemy.GetComponent<EnemyController>().Initialize(m_gameLogic.M_EnemyLogics[0], currentEnemyCube);
            }
        }*/
        
        //Initialize camera position and rotation
        Camera.main.transform.position = m_gameLogic.M_CameraInitialPosition;
        Camera.main.transform.rotation = Quaternion.Euler(m_gameLogic.M_CameraInitialRotation);
    }

    //Method called when detection of level's end
	public void LevelWin()
	{
        Debug.Log("WIIIIIIIIIIIIIIIIIIN!");
	}
}
