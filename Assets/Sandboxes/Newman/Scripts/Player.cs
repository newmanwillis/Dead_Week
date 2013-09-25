using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	
	public float movementSpeed = 0.1f;
	public int maxHealth = 100;
	public int maxPhoneCharge = 100;
	public int maxStamina = 100;
	
	public enum PlayerState {PlayerInput, SwordAttack, PhoneAttack, TakingDamage, Dead, Cutscene, Menu, chargingLazer, firingLazer};
	//public enum FacingDirection {Up = 0, UpLeft = 45, Left = 90, DownLeft = 135, Down = 180, RightDown = 215, Right = 270, UpRight = 315};
	public enum FacingDirection {Up = 0, Left = 90, Down = 180, Right = 270};
	public static Vector3 directionToVector(FacingDirection direction) {
		switch(direction) {
		case FacingDirection.Up:
			return new Vector3(0, 1, 0);
		case FacingDirection.Left:
			return new Vector3(-1, 0, 0);
		case FacingDirection.Down:
			return new Vector3(0, -1, 0);
		case FacingDirection.Right:
			return new Vector3(1, 0, 0);
		}
		return new Vector3(0, 0, 0);
	}
		
	private PlayerState curState;
	private FacingDirection curDirection;
	private tk2dSpriteAnimator curAnim;
	private Transform playerSprite;
	public int curPhoneCharge {get; private set;}
	public int curHealth {get; set;}
	int curStamina;
	
	public float lazerBeamChargeTime;  // time it takes the lazer to charge
	bool chargingLazer = false;
	float lazerChargedAtTime;  // time at which the lazer will be charged
	Transform currentlyFiringLazer = null;
	
	public Transform swordAttack;
	public Transform phoneBullet;
	public Transform phoneLazerBeam;
	public Transform phoneStunBullet;
	
	// private float attackAngle = 0;		// change to enum?
	// private Vector3 facingAngle = Vector3.up;		// REMOVE?

	// Use this for initialization
	void Start () {
		
		playerSprite = transform.FindChild("PlayerSprite");
		
		curState = PlayerState.PlayerInput;
		curDirection = FacingDirection.Down;
		curAnim = transform.FindChild("PlayerSprite").GetComponent<tk2dSpriteAnimator>();
		curHealth = maxHealth;
		curPhoneCharge = maxPhoneCharge;
		curStamina = maxStamina;
	}
	
	// Update is called once per frame
	void Update () {
		
		switch( curState)
		{
			case PlayerState.PlayerInput:
			AttackInput();  // Check for player attacks
			break;
			
			case PlayerState.firingLazer:  // deliberate fallthrough
			case PlayerState.chargingLazer:
				handleLazer();
			break;
		}
		
		//if(curState == PlayerStates.PlayerInput){
		//	AttackInput();
		//}
		
	}
	
	void FixedUpdate () {
		
		switch( curState)
		{
			case PlayerState.PlayerInput:
				MovementInput();  // Check for player movement
			break;
		}		
		
		//if(curState == PlayerState.PlayerInput) // change
		//	MovementInput();
		
	}
	
	void MovementInput(){
		
		// Moves the player
		float transH = Input.GetAxisRaw("Horizontal") * movementSpeed ;
		float transV = Input.GetAxisRaw("Vertical") * movementSpeed;
		Vector3 moveAmount = new Vector3(transH, transV, 0);
		GetComponent<CharacterController>().Move(moveAmount);
		
		// Switches animations & decides projectile attack angles
		/*
		if(Input.GetKey(KeyCode.UpArrow) && Input.GetKey(KeyCode.RightArrow)){
			attackAngle = 315;
			facingAngle = new Vector3(1, 1, 0);
		}
		else if(Input.GetKey(KeyCode.UpArrow) && Input.GetKey(KeyCode.LeftArrow)){
			attackAngle = 45;
			facingAngle = new Vector3(-1, 1, 0);
		}
		else if(Input.GetKey(KeyCode.DownArrow) && Input.GetKey(KeyCode.RightArrow)){
			attackAngle = 225;
			facingAngle = new Vector3(1, -1, 0);
		}	
		else if(Input.GetKey(KeyCode.DownArrow) && Input.GetKey(KeyCode.LeftArrow)){
			attackAngle = 135;
			facingAngle = new Vector3(-1, -1, 0);
		}			
		else */
		
		// change to enum/switch statements
		
		if(Input.GetKey(KeyCode.UpArrow)){
			curDirection = FacingDirection.Up;
			// attackAngle = 0;
			// facingAngle = Vector3.up;
			curAnim.Resume();
			curAnim.Play("walkingBackward");			
		}		
		else if(Input.GetKey(KeyCode.DownArrow)){
			curDirection = FacingDirection.Down;
			// attackAngle = 180;
			// facingAngle = Vector3.down;
			curAnim.Resume();
					curAnim.Play("walkingForward");			
		}
		else if(Input.GetKey(KeyCode.RightArrow)){
			curDirection = FacingDirection.Right;
			// attackAngle = 270;
			// facingAngle = Vector3.right;
			curAnim.Resume();
			curAnim.Play("walkingRight");			
		}		
		else if(Input.GetKey(KeyCode.LeftArrow)){
			curDirection = FacingDirection.Left;
			// attackAngle = 90;		
			// facingAngle = Vector3.left;
			curAnim.Resume();
			curAnim.Play("walkingLeft");
		}	
				
		// Pauses walking animation when nothing is happening.
		if(!(Input.GetKey(KeyCode.UpArrow)) && !(Input.GetKey(KeyCode.LeftArrow)) && !(Input.GetKey(KeyCode.DownArrow)) && !(Input.GetKey(KeyCode.RightArrow))){
			curAnim.StopAndResetFrame();			
		}
	}
	
	// called every tick that the player is firing or charging the lazer
	void handleLazer() {
		if (Input.GetKeyUp(KeyCode.R)) {
			curState = PlayerState.PlayerInput;
			if (Time.time < lazerChargedAtTime) {
				fireBullet(phoneBullet);
			} else {
				stopBeamLazer();
			}
		}
		if (curState == PlayerState.chargingLazer) {
			if (Time.time >= lazerChargedAtTime) {
				fireBeamLazer();
				curState = PlayerState.firingLazer;
			}
		}
		if (curState == PlayerState.firingLazer) {
			int beamCost = currentlyFiringLazer.GetComponentInChildren<PlayerProjectile>().energyCost;
			if (curPhoneCharge < beamCost) {
				curState = PlayerState.PlayerInput;
				stopBeamLazer();
			} else {
				curPhoneCharge -= beamCost;
			}
		}
	}
	
	void fireBullet(Transform bulletTypeToFire) {
		int cost = bulletTypeToFire.GetComponent<PlayerProjectile>().energyCost;
		if (curPhoneCharge < cost) {
			// TODO: replace this with a sound effect
			Debug.Log("Not enough battery");
		} else {
			Transform shootingBullet = (Transform)Instantiate(bulletTypeToFire, transform.position, Quaternion.identity);
			shootingBullet.rigidbody.AddForce(directionToVector(curDirection) * 8000);
			curPhoneCharge -= cost;
		}
	}
	
	void fireBeamLazer() {
		currentlyFiringLazer = (Transform)Instantiate(phoneLazerBeam, transform.position, Quaternion.identity);
		currentlyFiringLazer.Rotate(0, 0, (int)curDirection + 90);
		currentlyFiringLazer.parent = transform;
	}
	
	void stopBeamLazer() {
		if (currentlyFiringLazer != null) {
			Destroy(currentlyFiringLazer.gameObject);
			currentlyFiringLazer = null;
		}
	}
	
	void AttackInput(){
		
		// change to enum/switch 
		if(Input.GetKeyDown(KeyCode.R)){
			curState = PlayerState.chargingLazer;
			lazerChargedAtTime = Time.time + lazerBeamChargeTime;
		} else if (Input.GetKeyDown(KeyCode.F)) {
			fireBullet(phoneStunBullet);
		} else if(Input.GetKey(KeyCode.A)){
			/*curState = PlayerState.SwordAttack;
			if(swordAttack){  // melee

				Transform swordAttacking = (Transform)Instantiate(swordAttack, transform.position, Quaternion.identity);
				swordAttacking.Rotate(0,0,(int)curDirection);
				swordAttacking.parent = transform;
				StartCoroutine( FinishAttackAnimation(swordAttacking));
			}*/
			
					
			curState = PlayerState.SwordAttack;
			switch(curDirection)
			{
				case FacingDirection.Up:
				curAnim.Play("swordLeft");				
				break;
				case FacingDirection.Left:
				curAnim.Play("swordLeft");
				break;
				case FacingDirection.Down:
				curAnim.Play("swordRight");				
				break;
				case FacingDirection.Right:
				curAnim.Play("swordRight");				
				break;				
				
			}
			curAnim.Resume();
			StartCoroutine(waitForAnimationtoEnd());
		}
		// Phone Bullet/Beam
		else if(Input.GetKey(KeyCode.S)){ // change this
			// play sprite
		}
		//Phone Stun
		else if(Input.GetKey(KeyCode.D)){
			
		}	
	}
	/*
	IEnumerator FinishAttackAnimation(Transform sword){
		yield return new WaitForSeconds(0.1f);
		//swordAttacking = null;
		Destroy(sword.gameObject);
		curState = PlayerState.PlayerInput;
	}*/
	
	IEnumerator waitForAnimationtoEnd(){
		
		while(curAnim.Playing){
			yield return null;
		}
		curState = PlayerState.PlayerInput;
		Destroy(playerSprite.collider);
		// curAnim.Pause();
		switch(curDirection)
		{		
			case FacingDirection.Up:
			curAnim.Play("walkingLeft");				
			break;
			case FacingDirection.Left:
			curAnim.Play("walkingLeft");
			break;
			case FacingDirection.Down:
			curAnim.Play("walkingForward");				
			break;
			case FacingDirection.Right:
			curAnim.Play("walkingBackward");				
			break;	
		}	
	}
	
	void OnTriggerStay(Collider other) {
		if (other.tag == "ChargingStation") {
			curPhoneCharge = Mathf.Min(curPhoneCharge + 1, maxPhoneCharge);
		}
	}
}
