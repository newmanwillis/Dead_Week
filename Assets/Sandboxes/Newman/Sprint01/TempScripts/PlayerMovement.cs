﻿using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {
	public int phoneMaxCharge;
	public int PhoneCharge {get; private set;}
	
	public float Speed = 0.1f;
	public Transform swordAttack;
	public Transform bullet;
	public Transform lazerBeam;
	public Transform stunBullet;
	
	public float lazerBeamChargeTime;  // time it takes the lazer to charge
	bool chargingLazer = false;
	float lazerChargedAtTime;  // time at which the lazer will be charged
	Transform currentlyFiringLazer = null;
	                                                                                            
	
	float attackAngle = 0f;	
	private bool isAttacking = false;
	tk2dSprite curSprite;
	tk2dSpriteAnimator curAnim;
	
	Vector3 facingAngle = Vector3.up;
	//Transform swordAttacking;
	
	void Awake () {
		curAnim = GetComponent<tk2dSpriteAnimator>();
		//curSprite = GetComponent<tk2dSprite>();	
	}
	
	// Use this for initialization
	void Start () {
		PhoneCharge = phoneMaxCharge;
	}
	
	void Update () {
		if(Input.GetKeyDown(KeyCode.Space) && !isAttacking && !chargingLazer){
			isAttacking = true;
			if(swordAttack){  // melee

				Transform swordAttacking = (Transform)Instantiate(swordAttack, transform.position, Quaternion.identity);
				swordAttacking.Rotate(0,0,attackAngle);
				swordAttacking.parent = transform;
				StartCoroutine( FinishAttackAnimation(swordAttacking));
			}
		}
		
		// handle lazers:
		if(Input.GetKeyDown(KeyCode.R)){
			chargingLazer = true;
			lazerChargedAtTime = Time.time + lazerBeamChargeTime;
		}
		if (Input.GetKeyUp(KeyCode.R)) {
			chargingLazer = false;
			if (Time.time < lazerChargedAtTime) {
				fireBullet(bullet);
			} else {
				stopBeamLazer();
			}
		}
		if (chargingLazer) {
			if (Time.time >= lazerChargedAtTime) {
				fireBeamLazer();
				chargingLazer = false;
			}
		}
		if (currentlyFiringLazer != null) {
			int beamCost = currentlyFiringLazer.GetComponentInChildren<PlayerProjectile>().energyCost;
			if (PhoneCharge < beamCost) {
				stopBeamLazer();
			} else {
				PhoneCharge -= beamCost;
			}
		}
		
		if (Input.GetKeyDown(KeyCode.F)) {
			fireBullet(stunBullet);
		}
	}
	
	void fireBullet(Transform bulletTypeToFire) {
		int cost = bulletTypeToFire.GetComponent<PlayerProjectile>().energyCost;
		if (PhoneCharge < cost) {
			// TODO: replace this with a sound effect
			Debug.Log("Not enough battery");
		} else {
			Transform shootingBullet = (Transform)Instantiate(bulletTypeToFire, transform.position, Quaternion.identity);
			shootingBullet.rigidbody.AddForce(facingAngle * 8000);
			PhoneCharge -= cost;
		}
	}
	
	void fireBeamLazer() {
		currentlyFiringLazer = (Transform)Instantiate(lazerBeam, transform.position, Quaternion.identity);
		currentlyFiringLazer.Rotate(0, 0, attackAngle + 90);
		currentlyFiringLazer.parent = transform;
	}
	
	void stopBeamLazer() {
		if (currentlyFiringLazer != null) {
			Destroy(currentlyFiringLazer.gameObject);
			currentlyFiringLazer = null;
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
			curAnim.Play("walkingBackward");			
		}		
		else if(Input.GetKey(KeyCode.S)){
			//curPos.y -= Speed;
			attackAngle = 180;
			facingAngle = Vector3.down;
			curAnim.Resume();
			curAnim.Play("walkingForward");			
		}
		else if(Input.GetKey(KeyCode.D)){
			//curPos.x += Speed;
			attackAngle = 270;
			facingAngle = Vector3.right;
			curAnim.Resume();
			curAnim.Play("walkingRight");			
		}		
		else if(Input.GetKey(KeyCode.A)){
			//curPos.x -= Speed;
			attackAngle = 90;		
			facingAngle = Vector3.left;
			curAnim.Resume();
			curAnim.Play("walkingLeft");
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
	
	void OnTriggerStay(Collider other) {
		if (other.tag == "ChargingStation") {
			PhoneCharge = Mathf.Min(PhoneCharge + 1, phoneMaxCharge);
		}
	}
	
	IEnumerator FinishAttackAnimation(Transform sword){
		yield return new WaitForSeconds(0.1f);
		//swordAttacking = null;
		Destroy(sword.gameObject);
		isAttacking = false;
	}	
}
