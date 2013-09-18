using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {
	
	//public int health = 100;
	public float Speed = 0.1f;
	public Transform swordAttack;
	public Transform bullet;
	                                                                                            
	
	float attackAngle = 0f;	
	private bool isAttacking = false;
	tk2dSprite curSprite;
	tk2dSpriteAnimator curAnim;
	
	GroundControl ground;
	
	Vector3 facingAngle = Vector3.up;
	//Transform swordAttacking;
	
	void Awake () {
		curAnim = GetComponent<tk2dSpriteAnimator>();
		//curSprite = GetComponent<tk2dSprite>();	
	}
	
	// Use this for initialization
	void Start () {
		//ground = GameObject.Find("Ground").GetComponent<GroundControl>();
	
	}
	
	void Update () {
		if(Input.GetKeyDown(KeyCode.Space) && !isAttacking){
			isAttacking = true;
			if(swordAttack){  // melee

				Transform swordAttacking = (Transform)Instantiate(swordAttack, transform.position, Quaternion.identity);
				swordAttacking.Rotate(0,0,attackAngle);
				swordAttacking.parent = transform;
				StartCoroutine( FinishAttackAnimation(swordAttacking));
			}
		}
		if(Input.GetKeyDown(KeyCode.R)){  // shoot
			Transform shootingBullet = (Transform)Instantiate(bullet, transform.position, Quaternion.identity);
			shootingBullet.rigidbody.AddForce(facingAngle * 8000);
			
			
			//StartCoroutine( FinishBulletAnimation(shootingBullet));

		}
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		
		
		//Vector3 curPos = transform.position;
		if(Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.D)){
			//curPos.y += Speed;
			//curPos.x += Speed;
			attackAngle = 315;
			facingAngle = new Vector3(1, 1, 0);
			
			//curSprite.SetSprite("Zombie");
			//Debug.Log("tk2d sprite: " + GetComponent<tk2dSprite>().name);
		}
		else if(Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.A)){
			//curPos.y += Speed;
			//curPos.x -= Speed;
			attackAngle = 45;
			facingAngle = new Vector3(-1, 1, 0);
		}
		else if(Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.D)){
			//curPos.y -= Speed;
			//curPos.x += Speed;
			attackAngle = 225;
			facingAngle = new Vector3(1, -1, 0);
		}	
		else if(Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.A)){
			//curPos.y -= Speed;
			//curPos.x -= Speed;
			attackAngle = 135;
			facingAngle = new Vector3(-1, -1, 0);
		}			
		else if(Input.GetKey(KeyCode.W)){
			//curPos.y += Speed;
			attackAngle = 0;
			facingAngle = Vector3.up;
			curAnim.Resume();
			curAnim.Play("PlayerWalkingBack");			
		}		
		else if(Input.GetKey(KeyCode.S)){
			//curPos.y -= Speed;
			attackAngle = 180;
			facingAngle = Vector3.down;
			curAnim.Resume();
			curAnim.Play("PlayerWalkingFront");			
		}
		else if(Input.GetKey(KeyCode.D)){
			//curPos.x += Speed;
			attackAngle = 270;
			facingAngle = Vector3.right;
			curAnim.Resume();
			curAnim.Play("PlayerWalkingRight");			
		}		
		else if(Input.GetKey(KeyCode.A)){
			//curPos.x -= Speed;
			attackAngle = 90;		
			facingAngle = Vector3.left;
			curAnim.Resume();
			curAnim.Play("PlayerWalkingLeft");
				//= curAnim.Library.GetClipByName("PlayerWalkingLeft");
		}	
		
		if(!(Input.GetKey(KeyCode.W)) && !(Input.GetKey(KeyCode.A)) && !(Input.GetKey(KeyCode.S)) && !(Input.GetKey(KeyCode.D))){
			curAnim.Pause();
			
		}
	
		//if (ground.pointPathable(curPos)) {
		//transform.position = curPos;
		//}
		
		float transH = Input.GetAxisRaw("Horizontal") * Speed ;
		//Debug.Log("GetAxis Horizontal: " + Input.GetAxisRaw("Horizontal"));
		float transV = Input.GetAxisRaw("Vertical") * Speed;
		//Debug.Log("GetAxis Vertical: " + Input.GetAxisRaw("Vertical"));
		Vector3 moveAmount = new Vector3(transH, transV, 0);
		//Debug.Log("moveAmount: " + moveAmount);
		GetComponent<CharacterController>().Move(moveAmount);
	}
	
	void OnTriggerEnter(Collider other){
		if(other.tag == "ZombieAttack"){
			//Vector3 zombieDirection = other.transform.position - transform.position;
			Debug.Log("bite");
			gameObject.GetComponent<PlayerHealth>().health -= 1;
		}	
	}
	
	
	IEnumerator FinishAttackAnimation(Transform sword){
		yield return new WaitForSeconds(0.2f);
		//swordAttacking = null;
		Destroy(sword.gameObject);
		isAttacking = false;
	}
	
	IEnumerator FinishBulletAnimation (Transform bullet){
		yield return new WaitForSeconds(2f);
		Destroy(bullet.gameObject);
	}	
	
	
}
