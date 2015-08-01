using UnityEngine;
using System.Collections;

public class CubeController : MonoBehaviour {

    private GameController m_gameController;

    private CubeLogic m_cubeLogic;
    public CubeLogic M_CubeLogic
    {
        get { return m_cubeLogic; }
        set { m_cubeLogic = value; }
    }

    private Renderer m_renderer;

	public void Awake()
	{
        m_gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        m_renderer = GetComponent<Renderer>();
        m_renderer.sharedMaterial = new Material(m_gameController.M_GameLogic.M_CubeMaterial[0]);
        m_renderer.enabled = true;
	}

    public void Initialize(CubeLogic cubeLogic)
    {
        this.m_cubeLogic = cubeLogic;
    }

	void OnTriggerEnter(Collider other)
	{
		if(other.tag == "Player")
		{
            m_renderer.sharedMaterial = m_cubeLogic.M_Material;
		}
	}
}
