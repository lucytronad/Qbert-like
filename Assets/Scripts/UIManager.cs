using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIManager : MonoBehaviour {

    private static UIManager instance;
    public static UIManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<UIManager>();
                DontDestroyOnLoad(instance.gameObject);
            }
            return instance;
        }
    }

    private GameController m_gameController;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            if (this != instance)
            {
                Destroy(this.gameObject);
            }
        }
    }

	void Start () 
    {
        m_gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnLevelButtonClick(Text buttonText)
    {
        m_gameController.m_levelName = "LV"+ buttonText.text;
        Application.LoadLevel("MainGame");
        //m_gameController.StartLevel();
    }

    public void DisplayRestartScreen()
    {
        GameObject blackPanel = Instantiate(Resources.Load("BlackPanel")) as GameObject;
        blackPanel.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform);
        blackPanel.GetComponent<Image>().rectTransform.anchoredPosition = new Vector2(0.0f, 0.0f);
        blackPanel.GetComponent<Image>().rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        blackPanel.GetComponent<Image>().rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        blackPanel.GetComponent<Image>().rectTransform.pivot = new Vector2(0.5f, 0.5f);
        GameObject restartButton = Instantiate(Resources.Load("RestartButton")) as GameObject;
        restartButton.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform);
        restartButton.GetComponent<Image>().rectTransform.anchoredPosition = new Vector2(0.0f, 0.0f);
        restartButton.GetComponent<Image>().rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        restartButton.GetComponent<Image>().rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        restartButton.GetComponent<Image>().rectTransform.pivot = new Vector2(0.5f, 0.5f);
        restartButton.GetComponent<Button>().onClick.AddListener(RestartButtonEvent);
    }

    public void RestartButtonEvent()
    {
        Application.LoadLevel(Application.loadedLevel);
    }
}
