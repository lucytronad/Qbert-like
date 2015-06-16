using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour {

	private GameController gameController;
	private int blockCount;
	public int BlockCount{
		get
		{
			return blockCount;
		}
		set
		{
			blockCount = value;
		}
	}
	
	void Awake()
	{
		gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
		Instantiate(Resources.Load("level"+gameController.level, typeof(GameObject)),Vector3.zero,Quaternion.identity);

		Vector3 playerInitialPosition = (GameObject.FindGameObjectWithTag("PlayerInitialPosition")).GetComponent<Transform>().position;
		Instantiate(Resources.Load("character_dragon", typeof(GameObject)),playerInitialPosition,Quaternion.identity);

		Vector3 cameraInitialPosition = (GameObject.FindGameObjectWithTag("CameraInitialPosition")).GetComponent<Transform>().position;
		Camera.main.transform.position = cameraInitialPosition;

		blockCount = GameObject.FindGameObjectsWithTag("LevelBlock").Length;
	}

	void Update()
	{
		if(blockCount <= 0)
		{
			gameController.LevelWin();
		}
	}

}
