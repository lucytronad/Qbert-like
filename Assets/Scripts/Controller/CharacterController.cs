using UnityEngine;
using System.Collections;

public class CharacterController : MonoBehaviour {

    public GameObject m_jumpDownPath;
    public GameObject m_jumpUpPath;
    public GameObject m_jumpForwardPath;

    protected Animator m_animator;

    protected bool m_isJumping = false;
    protected string m_jumpType;
    protected int m_jumpPathType;

    protected GameObject m_targetCube;
    protected GameObject m_currentCube;

    protected CharacterLogic m_characterLogic;

	protected void Start () {
        m_animator = GetComponent<Animator>();
	}

    protected void FixedUpdate()
    {
        if (m_isJumping)
        {
            Jump(m_jumpType, m_jumpPathType);
        }
    }

    public void JumpAction(GameObject targetCube)
    {
        m_targetCube = targetCube;
        if (!m_isJumping)
        {
            FaceJumpPoint();
            InitJump();
        }
    }

    protected void FaceJumpPoint()
    {
        Vector3 lookAtDirection = m_targetCube.transform.GetChild(0).position;
        lookAtDirection.y = transform.position.y;
        transform.LookAt(lookAtDirection);
    }

    protected void InitJump()
    {
        if (m_targetCube.transform.position.y > m_currentCube.transform.position.y)
        {
            m_jumpType = "JumpingUp";
            m_jumpPathType = 0;
        }
        else if (m_targetCube.transform.position.y < m_currentCube.transform.position.y)
        {
            m_jumpType = "JumpingDown";
            m_jumpPathType = 1;
        }
        else
        {
            m_jumpType = "JumpingForward";
            m_jumpPathType = 2;
        }
        m_animator.SetBool("Is" + m_jumpType, true);
        m_isJumping = true;
        m_characterLogic.Jump(m_targetCube.GetComponent<CubeController>().M_CubeLogic);
    }

    protected void Jump(string jump, int jumpType)
    {
        if (m_animator.GetFloat(jump) > 0f)
        {
            Vector3 spawnPosition = m_currentCube.transform.GetChild(0).position;
            Vector3 forwardDirection = m_targetCube.transform.position - m_currentCube.transform.position;
            forwardDirection.y = 0;
            Quaternion spawnRotation = Quaternion.LookRotation(forwardDirection, m_currentCube.transform.up);

            GameObject jumpPath = jumpType == 0 ? m_jumpUpPath : jumpType == 1 ? m_jumpDownPath : m_jumpForwardPath;
            GameObject jumpPathClone = Instantiate(jumpPath, spawnPosition, spawnRotation) as GameObject;

            Transform[] transforms = jumpPathClone.GetComponentsInChildren<Transform>();
            Destroy(jumpPathClone);
            Hashtable ht = new Hashtable();
            ht.Add("path", transforms);
            ht.Add("time", 1.0f);
            iTween.MoveTo(gameObject, ht);

            m_currentCube = m_targetCube;
            m_isJumping = false;
            m_animator.SetBool("Is" + jump, false);
        }
    }
}
