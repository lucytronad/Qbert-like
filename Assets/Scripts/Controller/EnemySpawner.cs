using UnityEngine;
using System.Collections;
using System;

public class EnemySpawner : MonoBehaviour {

    private const float timeBetweenSpawn = 5.0f;
    private const float initialTimer = 0.0f;
    private float m_timer;
    private GameController m_gameController;
    private GameObject m_enemy;

	void Start () {
        m_gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        m_timer = initialTimer;
	}

    void Update()
    {
        m_timer += Time.fixedDeltaTime;
    }

	void FixedUpdate () {
        
	    if(m_gameController.M_GameLogic.M_SpawnerLogics.Count > 0 && m_gameController.M_GameLogic.M_EnemyLogics.Count < m_gameController.M_GameLogic.M_MaximumEnemiesNumber && m_timer >= timeBetweenSpawn)
        {
            m_timer = 0.0f;
            //Randomly choose a spawner
            System.Random rnd = new System.Random();
            Vector3 spawnPosition = m_gameController.M_GameLogic.M_SpawnerLogics[rnd.Next(0,m_gameController.M_GameLogic.M_SpawnerLogics.Count)].M_Position;

            //Adding an enemy
            EnemyLogic enemyLogic = new EnemyLogic(spawnPosition, m_gameController.M_GameLogic.FindCubeByPosition(spawnPosition));
            m_gameController.M_GameLogic.M_EnemyLogics.Add(enemyLogic);
            Collider[] colliders = Physics.OverlapSphere(spawnPosition, 1f);
            if (colliders.Length == 1)
            {
                if (colliders[0].gameObject.tag == "LevelBlock")
                {
                    GameObject currentEnemyCube = colliders[0].gameObject;
                    m_enemy = Instantiate(Resources.Load("enemy"), spawnPosition, Quaternion.identity) as GameObject;
                    m_enemy.GetComponent<EnemyController>().Initialize(enemyLogic, currentEnemyCube);
                }
            }
        }
	}
}
