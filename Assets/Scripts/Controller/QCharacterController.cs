using UnityEngine;
using System.Collections;

public class QCharacterController : MonoBehaviour {

    public GameObject m_jumpDownPath;
    public GameObject m_jumpUpPath;
    public GameObject m_jumpForwardPath;

    protected Animator m_animator;

    protected bool m_isJumping = false;
    protected bool m_hasJumped = false;
    protected string m_jumpType;
    protected int m_jumpPathType;

    protected GameObject m_targetCube;
    protected GameObject m_currentCube;

    protected CharacterLogic m_characterLogic;
    public CharacterLogic M_CharacterLogic
    {
        get { return m_characterLogic; }
    }

	protected virtual void Start () {
        m_animator = GetComponent<Animator>();
	}

    protected virtual void FixedUpdate()
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

    //Instant rotation to look in the direction of the jump
    protected void FaceJumpPoint()
    {
        Vector3 lookAtDirection = m_targetCube.transform.GetChild(0).position;
        lookAtDirection.y = transform.position.y;
        transform.LookAt(lookAtDirection);
    }

    //Initialize the jump according to its type
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
        if (m_animator.GetFloat(jump) > 0 && !m_hasJumped)
        {
            Vector3 spawnPosition = m_currentCube.transform.GetChild(0).position;
            Vector3 forwardDirection = transform.forward;// m_targetCube.transform.position - m_currentCube.transform.position;
            forwardDirection.y = 0;
            Quaternion spawnRotation = Quaternion.LookRotation(forwardDirection, m_currentCube.transform.up);

            GameObject jumpPath = jumpType == 0 ? m_jumpUpPath : jumpType == 1 ? m_jumpDownPath : m_jumpForwardPath;
            GameObject jumpPathClone = Instantiate(jumpPath, spawnPosition, spawnRotation) as GameObject;

            Transform[] transforms = jumpPathClone.GetComponentsInChildren<Transform>();
            Destroy(jumpPathClone);
            Hashtable ht = new Hashtable();
            ht.Add("path", transforms);
            ht.Add("time", 1.0f);
            ht.Add("onComplete", "EndJump");
            ht.Add("onCompleteParams", jump);
            iTween.MoveTo(gameObject, ht);
            m_hasJumped = true;
            m_currentCube = m_targetCube;
            m_animator.SetBool("Is" + jump, false);
        }
    }

    public void EndJump(string jump)
    {
        m_hasJumped = false;
        m_isJumping = false;     
    }

    public virtual void Initialize(CharacterLogic characterLogic, GameObject currentCube)
    {
        m_characterLogic = characterLogic;
        m_currentCube = currentCube;
    }
}
