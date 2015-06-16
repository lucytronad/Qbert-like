using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public float upForce = 0.5f;
	public GameObject jumpDownPath;
	public GameObject jumpUpPath;

	private Animator anim;
	private bool isJumping = false;
	private bool isJumpingDown = false;
	private bool isJumpingUp = false;
	private Vector3 invalidJump = new Vector3(100.0f,100.0f,100.0f);
	private Vector3 jumpPoint = Vector3.zero;
	private GameObject targetBlock;
	private GameObject currentBlock;
	private float blockWidth = 0.0f;
	
	void Start () {
		anim = GetComponent<Animator>();

		RaycastHit hitInfo;
		
		if(Physics.Raycast(transform.position, -transform.up, out hitInfo, 10.0f))
		{
			if(hitInfo.collider != null)
			{
				currentBlock = hitInfo.transform.gameObject;
				if(blockWidth == 0.0f)
					blockWidth = currentBlock.transform.localScale.x;
			}
		}

	}

	void Update() {
		Debug.DrawRay(transform.position,-transform.up);
		if(Input.GetButton("Jump") && anim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
		{

			RaycastHit hitInfo;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if(Physics.Raycast(ray, out hitInfo, 100.0f))
			{
				if(hitInfo.collider != null)
				{
					targetBlock = hitInfo.transform.gameObject;
					jumpPoint = hitInfo.transform.GetChild(0).position;
					CheckJumpPoint();
					if(!isJumping && jumpPoint != invalidJump) {
						FaceJumpPoint();
						if(targetBlock.transform.position.y>currentBlock.transform.position.y)
						{
							Debug.Log("Jumping Up!");
							InitJumpUp();
						}
						else
						{
							Debug.Log("Jumping Down!");
							InitJumpDown();
						}
					}
				}
			}
		}
	}

	void FixedUpdate ()
	{
		if(isJumpingDown)
		{
			JumpDown();
		}
		if(isJumpingUp)
		{
			JumpUp();
		}
	}

	private void InitJumpUp() {
		isJumping = true;
		isJumpingUp = true;
		anim.SetBool("IsJumpingUp",true);
	}
	
	private void JumpUp() {
		if(anim.GetFloat("JumpingUp")>0f)
		{
			Vector3 spawnPosition = currentBlock.transform.GetChild(0).position;
			Vector3 forwardDirection = targetBlock.transform.position - currentBlock.transform.position;
			forwardDirection.y = 0;
			Quaternion spawnRotation = Quaternion.LookRotation(-forwardDirection,currentBlock.transform.up);
			
			GameObject jumpPathClone = Instantiate(jumpUpPath,spawnPosition,spawnRotation) as GameObject;
			Transform[] transforms = jumpPathClone.GetComponentsInChildren<Transform>();
			Destroy (jumpPathClone);
			Hashtable ht = new Hashtable();
			ht.Add("path",transforms);
			ht.Add ("time",1.0f);
			iTween.MoveTo(gameObject,ht);
			
			currentBlock = targetBlock;
			isJumpingUp = false;
			isJumping = false;
			anim.SetBool("IsJumpingUp",false);
		}
	}

	private void InitJumpDown() {
		isJumping = true;
		isJumpingDown = true;
		anim.SetBool("IsJumpingDown",true);
	}

	private void JumpDown() {
		if(anim.GetFloat("JumpingDown")>0f)
		{
			Vector3 spawnPosition = currentBlock.transform.GetChild(0).position;
			Vector3 forwardDirection = targetBlock.transform.position - currentBlock.transform.position;
			forwardDirection.y = 0;
			Quaternion spawnRotation = Quaternion.LookRotation(forwardDirection,currentBlock.transform.up);

			GameObject jumpPathClone = Instantiate(jumpDownPath,spawnPosition,spawnRotation) as GameObject;
			Transform[] transforms = jumpPathClone.GetComponentsInChildren<Transform>();
			Destroy (jumpPathClone);
			Hashtable ht = new Hashtable();
			ht.Add("path",transforms);
			ht.Add ("time",1.0f);
			iTween.MoveTo(gameObject,ht);

			currentBlock = targetBlock;
			isJumpingDown = false;
			isJumping = false;
			anim.SetBool("IsJumpingDown",false);
		}
	}

	private void Jump(string jump, bool jumpUp) {
		if(anim.GetFloat(jump)>0f)
		{
			Vector3 spawnPosition = currentBlock.transform.GetChild(0).position;
			Vector3 forwardDirection = targetBlock.transform.position - currentBlock.transform.position;
			forwardDirection.y = 0;
			Quaternion spawnRotation = Quaternion.LookRotation(-forwardDirection,currentBlock.transform.up);

			GameObject jumpPath = jumpUp ? jumpUpPath : jumpDownPath;
			GameObject jumpPathClone = Instantiate(jumpPath,spawnPosition,spawnRotation) as GameObject;

			Transform[] transforms = jumpPathClone.GetComponentsInChildren<Transform>();
			Destroy (jumpPathClone);
			Hashtable ht = new Hashtable();
			ht.Add("path",transforms);
			ht.Add ("time",1.0f);
			iTween.MoveTo(gameObject,ht);
			
			currentBlock = targetBlock;
			isJumpingUp = false;
			isJumping = false;
			anim.SetBool("Is"+jump,false);
		}
	}

	
	private void CheckJumpPoint() {
		if(currentBlock.transform.position.y == targetBlock.transform.position.y 
		   || (targetBlock.transform.position-currentBlock.transform.position).magnitude > Mathf.Sqrt((blockWidth*blockWidth)*2))
		{
			string error = "Invalid Jump!\n";
			error+="currentBlock.transform.position.y: " + currentBlock.transform.position.y+"\n";
			error+="targetBlockPosition.y: " + targetBlock.transform.position.y+"\n";
			jumpPoint = invalidJump;
			Debug.Log(error);
		}
	}

	private void FaceJumpPoint() {
		Vector3 lookAtDirection = jumpPoint;
		lookAtDirection.y = transform.position.y;
		transform.LookAt(lookAtDirection);
	}

	/*private void JumpDown() {

		if(anim.GetFloat("Jump")>0f && !hasLeaveGround)
		{
			jumpTranslation = transform.position;
			jumpTranslation.z+=7.0f;
			rb.AddForce(transform.up * upForce,ForceMode.Impulse);
			hasLeaveGround = true;
		}
		if(hasLeaveGround)
		{
			transform.Translate(jumpTranslation*Time.deltaTime);
			if(transform.position.z >= jumpTranslation.z)
			{
				SetCurrentBlock();
				isJumpingDown=false;
				hasLeaveGround = false;
				anim.SetBool("IsJumpingDown",false);

			}
		}
	}*/
	
}
