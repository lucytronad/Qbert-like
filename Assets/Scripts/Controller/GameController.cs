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
        m_gameLogic = new GameLogic(m_levelName);
        BuildLevel();
	}
	

	void Update ()
    {
	    if(m_gameLogic.IsLevelComplete())
        {
            LevelWin();
        }

        if(Input.GetButton("Jump"))
        {
            RaycastHit hitInfo;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hitInfo, 100.0f))
            {
                if (hitInfo.collider != null && hitInfo.transform.gameObject.tag == "LevelBlock")
                {
                    m_targetCube = hitInfo.transform.gameObject;
                }
            }
            if (m_gameLogic.M_PlayerLogic.M_CubeLogic.M_Neighbors.Contains(m_targetCube.GetComponent<CubeController>().M_CubeLogic))
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

        GameObject currentPlayerCube = new GameObject();
        Collider[] colliders = Physics.OverlapSphere(m_gameLogic.M_PlayerLogic.M_CubeLogic.M_InitialPosition, 1f);
        if (colliders.Length == 1)
        {
            if (colliders[0].gameObject.tag == "LevelBlock")
            {
                currentPlayerCube = colliders[0].gameObject;
            }
        }

        m_player = Instantiate(Resources.Load("character_dragon"), m_gameLogic.M_PlayerLogic.M_InitialPosition, Quaternion.identity) as GameObject;
        m_player.GetComponent<PlayerController>().Initialize(m_gameLogic.M_PlayerLogic, currentPlayerCube);

        Camera.main.transform.position = m_gameLogic.M_CameraInitialPosition;
        Camera.main.transform.rotation = Quaternion.Euler(m_gameLogic.M_CameraInitialRotation);
    }

	public void LevelWin()
	{
        Debug.Log("WIIIIIIIIIIIIIIIIIIN!");
	}
}
