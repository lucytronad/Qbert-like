using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyController : QCharacterController {

    private const float initialTimer = 0.0f;
    private const float timeBetweenJumps = 1.5f;
    private float m_timer;
    
    protected override void Start()
    {
        base.Start();
        m_timer = initialTimer;

    }

    void Update()
    {
        m_timer += Time.fixedDeltaTime;
    }

    protected override void FixedUpdate()
    {
        m_timer += Time.fixedDeltaTime;
        base.FixedUpdate();
        if(!m_isJumping && m_timer >= timeBetweenJumps && m_characterLogic.M_CubeLogic != null)
        {
            m_timer = 0.0f;
            //Randomly choose a lower neighbor to jump to
            System.Random rnd = new System.Random();
            List<CubeLogic> lowerNeighbor = m_characterLogic.M_CubeLogic.FindLowerNeighbors();
            if(lowerNeighbor.Count > 0)
            {
                Collider[] colliders = Physics.OverlapSphere(lowerNeighbor[rnd.Next(0,lowerNeighbor.Count)].M_InitialPosition, 1f);
                if (colliders.Length == 1)
                {
                    if (colliders[0].gameObject.tag == "LevelBlock")
                    {
                        GameObject targetCube = colliders[0].gameObject;
                        JumpAction(targetCube);
                    }
                }
            }
            else
            {
                //Suicide jump
                SuicideJump();
            }
            
        }
	}

    private void SuicideJump()
    {
        m_jumpType = "JumpingDown";
        m_jumpPathType = 1;
        m_animator.SetBool("Is" + m_jumpType, true);
        m_isJumping = true;
        m_characterLogic.Jump(null);
    }
}
