using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	
	public float movementSpeed = 0.8f;
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

	public Transform phoneBullet;
	public Transform phoneLazerBeam;
	public Transform phoneStunBullet;
	
	public bool hasSword;
	public bool hasPhoneBullet;
	public bool hasPhoneLazer;
	public bool hasPhoneStun;
	
	private Transform flashLight;
	private bool flashLightOn = false;

	// Use this for initialization
	void Start () {
		
		playerSprite = transform.FindChild("PlayerSprite");
		flashLight = transform.FindChild("FlashLight");
		
		curState = PlayerState.PlayerInput;
		curDirection = FacingDirection.Down;
		flashLight.transform.Rotate(35, 0, 0);
		flashLight.GetComponent<Light>().intensity = 0;
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
		
	}
	
	void FixedUpdate () {
		
		switch( curState)
		{
			case PlayerState.PlayerInput:
				MovementInput();  // Check for player movement
			break;
		}		
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
			flashLight.transform.localRotation = Quaternion.identity;
			flashLight.transform.Rotate(-35, 0, 0);
			curAnim.Resume();
			curAnim.Play("walkingBackward");			
		}		
		else if(Input.GetKey(KeyCode.DownArrow)){
			curDirection = FacingDirection.Down;
			flashLight.transform.localRotation = Quaternion.identity;
			flashLight.transform.Rotate(35, 0, 0);
			curAnim.Resume();
			curAnim.Play("walkingForward");			
		}
		else if(Input.GetKey(KeyCode.RightArrow)){
			curDirection = FacingDirection.Right;
			flashLight.transform.localRotation = Quaternion.identity;
			flashLight.transform.Rotate(0, 35, 0);
			curAnim.Resume();
			curAnim.Play("walkingRight");			
		}		
		else if(Input.GetKey(KeyCode.LeftArrow)){
			curDirection = FacingDirection.Left;
			flashLight.transform.localRotation = Quaternion.identity;
			flashLight.transform.Rotate(0, -35, 0);
			curAnim.Resume();
			curAnim.Play("walkingLeft");
		}	
				
		// Stops walking animation when nothing is happening.
		if(!(Input.GetKey(KeyCode.UpArrow)) && !(Input.GetKey(KeyCode.LeftArrow)) && !(Input.GetKey(KeyCode.DownArrow)) && !(Input.GetKey(KeyCode.RightArrow))){
			curAnim.StopAndResetFrame();			
		}
	}
	
	// Called every tick that the player is firing or charging the lazer
	void handleLazer() {
		if (Input.GetKeyUp(KeyCode.S)) {
			curState = PlayerState.PlayerInput;
			if (Time.time < lazerChargedAtTime) {
				if (hasPhoneBullet) {
					fireBullet(phoneBullet);
				}
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
			shootingBullet.Rotate(0, 0, (int)curDirection - 270);
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
		if(Input.GetKeyDown(KeyCode.S)){			// Phone bullet
			if (hasPhoneLazer) {
				curState = PlayerState.chargingLazer;
				lazerChargedAtTime = Time.time + lazerBeamChargeTime;
			} else if (hasPhoneBullet) {
				fireBullet(phoneBullet);
			}
		} else if (Input.GetKeyDown(KeyCode.D) && hasPhoneStun) {	// Phone stun
			fireBullet(phoneStunBullet);
		} else if(Input.GetKey(KeyCode.A) && hasSword){			// Sword Attack
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
		} else if (Input.GetKeyDown(KeyCode.F)) {    // flashlight
			if (flashLightOn) {
				flashLightOn = false;
				flashLight.GetComponent<Light>().intensity = 0;
			} else {
				flashLightOn = true;
				flashLight.GetComponent<Light>().intensity = 6;
			}
		}
	}
	
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
		} else if (other.tag == "SwordPickup") {
			hasSword = true;
			Destroy(other.gameObject);
		} else if (other.tag == "PhoneBulletPickup") {
			hasPhoneBullet = true;
			Destroy(other.gameObject);
		} else if (other.tag == "PhoneLazerPickup") {
			hasPhoneLazer = true;
			Destroy(other.gameObject);
		} else if (other.tag == "PhoneStunPickup") {
			hasPhoneStun = true;
			Destroy(other.gameObject);
		} else if (other.tag == "TextMessage") {
			Camera.main.GetComponent<CameraControl>().pauseAndDrawTextMessage(other.GetComponent<TextMessage>().message);
			Destroy(other.gameObject);
		}
	}
}
