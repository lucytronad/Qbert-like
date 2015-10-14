using UnityEngine;
using System.Collections;

public class EnemyRemover : MonoBehaviour {

    private GameController m_gameController;

	// Use this for initialization
	void Start () {
        m_gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Enemy")
        {
            EnemyController enemyController = other.gameObject.GetComponent<EnemyController>();
            m_gameController.M_GameLogic.M_EnemyLogics.Remove((EnemyLogic)enemyController.M_CharacterLogic);
            Destroy(other.gameObject);
        }
    }
}
