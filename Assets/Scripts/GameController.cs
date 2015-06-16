using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

	private static GameController instance;
	public string level;

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

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnLevelButtonClick(Text buttonText)
	{
		level = buttonText.text;
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

	public void LevelWin()
	{
		Debug.Log ("WIIIIIIIIIIIIIIIIIIN!");
	}
}
