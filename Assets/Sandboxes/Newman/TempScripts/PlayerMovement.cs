using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {
	
	public float Speed = 0.5f;
	public Transform swordAttack;

	float attackAngle = 0f;	
	private bool isAttacking = false;
	//Transform swordAttacking;
	
	// Use this for initialization
	void Start () {
	
	}
	
	void Update () {
		if(Input.GetKeyDown(KeyCode.Space) && !isAttacking){
			isAttacking = true;
			if(swordAttack){

				Transform swordAttacking = (Transform)Instantiate(swordAttack, transform.position, Quaternion.identity);
				swordAttacking.Rotate(0,0,attackAngle);
				swordAttacking.parent = transform;
				StartCoroutine( FinishAttackAnimation(swordAttacking));
				
				
			}
		}
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		Vector3 curPos = transform.position;
		if(Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.D)){
			curPos.y += Speed;
			curPos.x += Speed;
			attackAngle = 315;
		}
		else if(Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.A)){
			curPos.y += Speed;
			curPos.x -= Speed;
			attackAngle = 45;
		}
		else if(Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.D)){
			curPos.y -= Speed;
			curPos.x += Speed;
			attackAngle = 225;
		}	
		else if(Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.A)){
			curPos.y -= Speed;
			curPos.x -= Speed;
			attackAngle = 135;
		}			
		else if(Input.GetKey(KeyCode.W)){
			curPos.y += Speed;
			attackAngle = 0;
		}		
		else if(Input.GetKey(KeyCode.S)){
			curPos.y -= Speed;
			attackAngle = 180;
		}
		else if(Input.GetKey(KeyCode.D)){
			curPos.x += Speed;
			attackAngle = 270;
		}		
		else if(Input.GetKey(KeyCode.A)){
			curPos.x -= Speed;
			attackAngle = 90;			
		}	
	
		transform.position = curPos;
	}

	IEnumerator FinishAttackAnimation(Transform sword){
		yield return new WaitForSeconds(0.7f);
		//swordAttacking = null;
		Destroy(sword.gameObject);
		isAttacking = false;
	}
	
	
}
