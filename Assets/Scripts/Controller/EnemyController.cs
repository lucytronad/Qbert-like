using UnityEngine;
using System.Collections;

public class EnemyController : CharacterController {

    private const float timeBetweenJumps = 1.5f;
    private float m_timer;
    
    private void Start()
    {
        base.Start();
        m_timer = 0.0f;

    }

    private void Update()
    {
        m_timer += Time.fixedDeltaTime;
    }

	private void FixedUpdate ()
    {
        
        base.FixedUpdate();
        if(!m_isJumping && m_timer >= timeBetweenJumps)
        {
            m_timer = 0.0f;
            Collider[] colliders = Physics.OverlapSphere(m_characterLogic.M_CubeLogic.M_Neighbors[0].M_InitialPosition, 1f);
            if (colliders.Length == 1)
            {
                if (colliders[0].gameObject.tag == "LevelBlock")
                {
                    GameObject targetCube = colliders[0].gameObject;
                    Debug.LogWarning(targetCube.transform.position);
                    JumpAction(targetCube);
                }
            }
        }
	}

    public void Initialize(EnemyLogic enemyLogic, GameObject currentCube)
    {
        m_characterLogic = enemyLogic;
        m_currentCube = currentCube;
    }
}
