using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {
	
	public int health = 100;
	public float Speed = 0.5f;
	public Transform swordAttack;
	public Transform bullet;
	                                                                                            
	
	float attackAngle = 0f;	
	private bool isAttacking = false;
	tk2dSprite curSprite;
	
	GroundControl ground;
	
	Vector3 facingAngle = Vector3.up;
	//Transform swordAttacking;
	
	// Use this for initialization
	void Start () {
		ground = GameObject.Find("Ground").GetComponent<GroundControl>();
	
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
			
			
			StartCoroutine( FinishBulletAnimation(shootingBullet));

		}
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		Vector3 curPos = transform.position;
		if(Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.D)){
			curPos.y += Speed;
			curPos.x += Speed;
			attackAngle = 315;
			facingAngle = new Vector3(1, 1, 0);
		}
		else if(Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.A)){
			curPos.y += Speed;
			curPos.x -= Speed;
			attackAngle = 45;
			facingAngle = new Vector3(-1, 1, 0);
		}
		else if(Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.D)){
			curPos.y -= Speed;
			curPos.x += Speed;
			attackAngle = 225;
			facingAngle = new Vector3(1, -1, 0);
		}	
		else if(Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.A)){
			curPos.y -= Speed;
			curPos.x -= Speed;
			attackAngle = 135;
			facingAngle = new Vector3(-1, -1, 0);
		}			
		else if(Input.GetKey(KeyCode.W)){
			curPos.y += Speed;
			attackAngle = 0;
			facingAngle = Vector3.up;
		}		
		else if(Input.GetKey(KeyCode.S)){
			curPos.y -= Speed;
			attackAngle = 180;
			facingAngle = Vector3.down;
		}
		else if(Input.GetKey(KeyCode.D)){
			curPos.x += Speed;
			attackAngle = 270;
			facingAngle = Vector3.right;
		}		
		else if(Input.GetKey(KeyCode.A)){
			curPos.x -= Speed;
			attackAngle = 90;		
			facingAngle = Vector3.left;
		}	
	
		if (ground.pointPathable(curPos)) {
			transform.position = curPos;
		}
	}
	
	void OnTriggerEnter(Collider other){
		if(other.tag == "Zombie"){
			//Vector3 zombieDirection = other.transform.position - transform.position;
			health -= 20;
		}	
	}
	
	
	IEnumerator FinishAttackAnimation(Transform sword){
		yield return new WaitForSeconds(0.5f);
		//swordAttacking = null;
		Destroy(sword.gameObject);
		isAttacking = false;
	}
	
	IEnumerator FinishBulletAnimation (Transform bullet){
		yield return new WaitForSeconds(2f);
		Destroy(bullet.gameObject);
	}	
	
	
}
