using UnityEngine;
using System.Collections;

public class BlockValidation : MonoBehaviour {

	public LevelManager levelManager;
	public Material validationMaterial;

	private Material currentMaterial;
	private Renderer rend;

	public void Awake()
	{
		levelManager = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>();
	}

	// Use this for initialization
	void Start () {
		rend = GetComponent<Renderer>();
		rend.enabled = true;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other)
	{
		if(other.tag == "Player")
		{
			rend.sharedMaterial = validationMaterial;
			levelManager.BlockCount--;
		}
	}
}
