using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	
	public int swordDamage;
	public float movementSpeed = 0.8f;
	public float maxHealth = 100;
	public float maxPhoneCharge = 100;
	public float maxStamina = 200;
	public float sprintSpeedup = 1.6f;
	public float knockedBackSpeed = 3f;
	public bool invulnerable {get; set;}
	
	private bool hasCheckpoint = false;
	private Vector3 currentCheckpoint;
	public Transform checkpointNotification;
	
	public enum PlayerState {PlayerInput, SwordAttack, PhoneAttack, TakingDamage, Dead, Cutscene, Menu, chargingLazer, firingLazer, KnockedBack};
	public enum PhonePower {Bullet, Beam, Stun};
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
	
	private AudioSource swordAudioChannel;  
	private AudioSource footStepsAudio;
	private AudioSource takingDamageAudio;
	private AudioSource itemPickupAudio;
	private AudioSource noBatteryAudio;
	private AudioSource batteryChargingAudio;
	private AudioSource[] aSources;
	private PlayerState curState;
	private PhonePower curPower;
	private FacingDirection curDirection;
	private tk2dSpriteAnimator curAnim;
	private Transform playerSprite;
	public float curPhoneCharge {get; private set;}
	public float curHealth {get; set;}
	public float __curStaminaActual;
	public float curStamina {
		get {
			return __curStaminaActual;
		}
		set {
			__curStaminaActual = value > maxStamina ? maxStamina : value;
		}
	}
	private bool isSprinting = false;
	
	public bool footStepsAudioPlaying = false;
	public float lazerBeamChargeTime;  // time it takes the lazer to charge
	bool chargingLazer = false;
	float lazerChargedAtTime;  // time at which the lazer will be charged
	Transform currentlyFiringLazer = null;

	private Vector3 knockBackDirection;

	private Transform phoneBullet;
	private Transform phoneLazerBeam;
	private Transform phoneStunBullet;
	private Transform phoneStunExplosion;
	private Transform phoneStunLight;
	
	public bool hasSword;
	public bool hasPhoneBullet;
	public bool hasPhoneLazer;
	public bool hasPhoneStun;
	
	private FlashlightControl flashLight;
	
	private float swordAttackStartTime;
	private PlayerAttackHitbox currentAttackHitbox;
	private PlayerAttackHitbox leftSideHitbox;
	private PlayerAttackHitbox rightSideHitbox;
	private PlayerAttackHitbox upHitbox;
	private PlayerAttackHitbox downHitbox;
	
	public bool startInCutscene;
	public Texture initialInfoCard;

	// Use this for initialization
	void Start () {
		if (initialInfoCard != null) {
			Camera.main.GetComponent<CameraControl>().pauseAndDrawInfoCard(initialInfoCard);
		}
		
		if (startInCutscene) {
			enterCutscene();
		} else {
			
			// Loads Prefabs directly from Resources Folder
			phoneBullet = ((GameObject) Resources.Load("Phone Attacks/Bullet")).transform;
			phoneLazerBeam = ((GameObject) Resources.Load("Phone Attacks/LazerBeam")).transform;
			phoneStunBullet = ((GameObject) Resources.Load("Phone Attacks/StunBullet")).transform;
			phoneStunExplosion = ((GameObject) Resources.Load("Phone Attacks/StunExplosion")).transform;
			phoneStunLight = ((GameObject) Resources.Load("Phone Attacks/StunLight")).transform;
		
			playerSprite = transform.FindChild("PlayerSprite");
			flashLight = GetComponent<FlashlightControl>();
			
			// Player Audio sources - location in array dependent upon order of player components
			// first audio source listed is source[0]
			aSources = GetComponents<AudioSource>();
			footStepsAudio = aSources[0];
			swordAudioChannel = aSources[1];
			takingDamageAudio = aSources[2];
			itemPickupAudio = aSources[3];
			noBatteryAudio = aSources[4];
			batteryChargingAudio = aSources[5];
			swordAudioChannel.loop = false;
			takingDamageAudio.loop = false;
			itemPickupAudio.loop = false;
			
		
			curState = PlayerState.PlayerInput;
			curDirection = FacingDirection.Down;
			curAnim = transform.FindChild("PlayerSprite").GetComponent<tk2dSpriteAnimator>();
			curHealth = maxHealth;
			curPhoneCharge = maxPhoneCharge;
			curStamina = 0;
		
			rightSideHitbox = GameObject.Find("RightSideAttackHitbox").GetComponent<PlayerAttackHitbox>();
			leftSideHitbox = GameObject.Find("LeftSideAttackHitbox").GetComponent<PlayerAttackHitbox>();
			upHitbox = GameObject.Find("UpAttackHitbox").GetComponent<PlayerAttackHitbox>();
			downHitbox = GameObject.Find("DownAttackHitbox").GetComponent<PlayerAttackHitbox>();
		
			invulnerable = false;
			
		}
	}
	
	// Update is called once per frame
	void Update () {
		switch( curState)
		{
			case PlayerState.PlayerInput:
			AttackInput();  // Check for player attacks
			break;
			
		    case PlayerState.SwordAttack:
			currentAttackHitbox.attack(swordDamage, swordAttackStartTime);
			//swordAttack(currentAttackHitbox, swordAttackStartTime);
			break;
			
			case PlayerState.firingLazer:  // deliberate fallthrough
			case PlayerState.chargingLazer:
				handleLazer();
			break;
		}
		
	}
	
	void FixedUpdate () {
		// Keep player at 0 in Z-zone
		if(transform.position.z != -0.01){
			if(curState != PlayerState.Dead){
				Vector3 newPos = transform.position;
				newPos.z = 0;
				transform.position = newPos;
				playerSprite.GetComponent<Rigidbody>().velocity = Vector3.zero;
			}
		}		
		
		/*if (curHealth <= 0) {
			curState = PlayerState.Dead;
		}*/
		switch( curState)
		{
			case PlayerState.PlayerInput:
				MovementInput();  // Check for player movement
			break;
			case PlayerState.KnockedBack:
				if (curAnim.Playing) {
					GetComponent<CharacterController>().Move(knockBackDirection * knockedBackSpeed);
				} else {
					curState = PlayerState.PlayerInput;
				}
			break;
			/*
			case PlayerState.Dead:
				if (!curAnim.Playing) {
					if (hasCheckpoint) {
						transform.position = currentCheckpoint;
						curHealth = maxHealth;
						curPhoneCharge = maxPhoneCharge;
						curStamina = 0;
						curState = PlayerState.PlayerInput;
						invulnerable = false;
						GotHit(0); // make the sprite flash
					} else {
						Application.LoadLevel(Application.loadedLevel);
					}
				}
			break;*/
		}
		if (isSprinting) {
			curStamina -= 1;
		} else {
			curStamina += 0.5f;
		}
	}

	string directionToString(FacingDirection dir) {
		switch (dir) {
		case FacingDirection.Down:
			return "Down";
		case FacingDirection.Left:
			return "Left";
		case FacingDirection.Right:
			return "Right";
		case FacingDirection.Up:
			return "Up";
		}
		return null;
	}
	
	void MovementInput(){
					
		// Moves the player
		float transH = Input.GetAxisRaw("Horizontal") * movementSpeed ;
		float transV = Input.GetAxisRaw("Vertical") * movementSpeed;
		Vector3 moveAmount = new Vector3(transH, transV, 0);
		if (isSprinting) {
			moveAmount *= sprintSpeedup;
		}
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

		bool isMoving = true;
		if(Input.GetKey(KeyCode.UpArrow)){
			curDirection = FacingDirection.Up;
		}		
		else if(Input.GetKey(KeyCode.DownArrow)){
			curDirection = FacingDirection.Down;
		}
		else if(Input.GetKey(KeyCode.RightArrow)){
			curDirection = FacingDirection.Right;
		}		
		else if(Input.GetKey(KeyCode.LeftArrow)){
			curDirection = FacingDirection.Left;
		} else {
			isMoving = false;
		}

		flashLight.face(curDirection);

		if (isMoving) {
			string animName = "";
			animName += (isSprinting && hasSword) ? "run" : "walking";
			animName += directionToString(curDirection);
			animName += hasSword ? "" : "NoSword";
			curAnim.Play(animName);
		}
				
		// Stops walking animation when nothing is happening.
		if(!isMoving){
			curAnim.Play("walking" + directionToString(curDirection) + (hasSword? "" : "NoSword"));
			curAnim.StopAndResetFrame();	
			footStepsAudio.Stop();
			footStepsAudioPlaying = false;
		}
		else{
			if(!footStepsAudioPlaying){
				footStepsAudioPlaying = true;
				footStepsAudio.Play();
			}
		}
	}
	
	// Called every tick that the player is firing or charging the lazer
	void handleLazer() {
		if (Input.GetKeyUp(KeyCode.S)) {
			curState = PlayerState.PlayerInput;
			if (Time.time < lazerChargedAtTime) {
				if (hasPhoneBullet) {
					waitForPhoneAnimationAndFire(PhonePower.Bullet, true);
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
				noBatteryAudio.Play();
				stopBeamLazer();
			} else {
				curPhoneCharge -= beamCost;
			}
		}
	}
	
	void fireBullet(Transform bulletTypeToFire, bool dropInsteadOfFire = false) {
		PlayerProjectile proj = bulletTypeToFire.GetComponent<PlayerProjectile>();
		int cost;
		if (proj != null) {
			cost = proj.energyCost;
		} else {
			cost = bulletTypeToFire.GetComponent<StunExplosion>().energyCost;
		}
		if (curPhoneCharge < cost) {
			noBatteryAudio.Play();
			Debug.Log("Not enough battery");
		} else {
			Transform shootingBullet = (Transform)Instantiate(bulletTypeToFire, transform.position, Quaternion.identity);
			shootingBullet.Rotate(0, 0, (int)curDirection - 270);
			if (!dropInsteadOfFire) {
				shootingBullet.rigidbody.AddForce(directionToVector(curDirection) * 8000);
			}
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
	
	void waitForPhoneAnimationAndFire(PhonePower power, bool alreadyAnimated = false) {
		curState = PlayerState.PhoneAttack;
		curPower = power;
		if (!alreadyAnimated) {
			PhoneAttackAnimation();
		}
		StartCoroutine(waitForPhoneAnimationtoEnd());
	}
	
	void AttackInput(){
		if (Input.GetKeyDown(KeyCode.LeftShift)) {
			isSprinting = true;
		} 
		if (Input.GetKeyUp(KeyCode.LeftShift) || (curStamina < 1)) {
			isSprinting = false;
		}

		//TODO remove this
		if (Input.GetKeyDown(KeyCode.G)) {
			knockBack(new Vector3(1, 0, 0));
		}
		
		// change to enum/switch 
		if(Input.GetKeyDown(KeyCode.S)){			// Phone bullet
			if (hasPhoneLazer) {
				PhoneAttackAnimation();
				curState = PlayerState.chargingLazer;
				lazerChargedAtTime = Time.time + lazerBeamChargeTime;
			} else if (hasPhoneBullet) {
				PhoneAttackAnimation();
				waitForPhoneAnimationAndFire(PhonePower.Bullet);
			}
		} else if (Input.GetKeyDown(KeyCode.D) && hasPhoneStun) {	// Phone stun
			waitForPhoneAnimationAndFire(PhonePower.Stun);
		} else if(Input.GetKeyDown(KeyCode.A) && hasSword){			// Sword Attack
			curState = PlayerState.SwordAttack;
			swordAttackStartTime = Time.time;
			switch(curDirection)
			{
				case FacingDirection.Up:
				currentAttackHitbox = upHitbox;
				curAnim.Play("swordUp");				
				break;
				case FacingDirection.Left:
				currentAttackHitbox = leftSideHitbox;
				curAnim.Play("swordLeft");
				break;
				case FacingDirection.Down:
				currentAttackHitbox = downHitbox;
				curAnim.Play("swordDown");				
				break;
				case FacingDirection.Right:
				currentAttackHitbox = rightSideHitbox;
				curAnim.Play("swordRight");				
				break;					
			}
			currentAttackHitbox.attack(swordDamage, swordAttackStartTime);
			//swordAttack(currentAttackHitbox, swordAttackStartTime);
			swordAudioChannel.Play ();
			curAnim.Resume();
			StartCoroutine(waitForAnimationtoEnd());
		} else if (Input.GetKeyDown(KeyCode.F)) {    // flashlight
			flashLight.toggleOnOff();
		}
	}
	
	IEnumerator waitForAnimationtoEnd(){
		
		while(curAnim.Playing){
			yield return null;
		}
		curState = PlayerState.PlayerInput;
		Destroy(playerSprite.collider);
		// curAnim.Pause();
		curAnim.Play("walking" + directionToString(curDirection) + (hasSword? "" : "NoSword"));
	}
	
	IEnumerator waitForPhoneAnimationtoEnd(){
		
		while(curAnim.Playing){
			yield return null;
		}
		curState = PlayerState.PlayerInput;
		
		switch(curPower) {
		case PhonePower.Stun:
			//fireBullet(phoneStunBullet);
			fireBullet(phoneStunExplosion, true);

			// Creates a Light when shooting the stun
			Vector3 stunLightPos = transform.position;
			stunLightPos.z -= 100;
			Instantiate(phoneStunLight, stunLightPos, Quaternion.identity);

			break;
		case PhonePower.Bullet:
			fireBullet(phoneBullet);
			break;
		}
		//Destroy(playerSprite.collider);
		// curAnim.Pause();
		curAnim.Play("walking" + directionToString(curDirection) + (hasSword? "" : "NoSword"));
	}
	
	void PhoneAttackAnimation(){
		switch(curDirection)
		{		
			case FacingDirection.Up:
			//curAnim.Play("phoneBack (1 Frame)");
			curAnim.Play("phoneUp");
			break;
			case FacingDirection.Left:
			//curAnim.Play("phoneLeft (1 Frame)");
			curAnim.Play("phoneLeft");
			break;
			case FacingDirection.Down:
			//curAnim.Play("phoneFront (1 Frame)");
			curAnim.Play("phoneDown");
			break;
			case FacingDirection.Right:
			//curAnim.Play("phoneRight (1 Frame)");
			curAnim.Play("phoneRight");
			break;	
		}				
	}
	
	void OnTriggerStay(Collider other) {
		if (other.tag == "ChargingStation") {
			curPhoneCharge = Mathf.Min(curPhoneCharge + 1, maxPhoneCharge);
		} else if (other.tag == "SwordPickup") {
			hasSword = true;
			Destroy(other.gameObject);
			itemPickupAudio.Play();
		} else if (other.tag == "PhoneBulletPickup") {
			hasPhoneBullet = true;
			Destroy(other.gameObject);
			itemPickupAudio.Play();
		} else if (other.tag == "PhoneLazerPickup") {
			hasPhoneLazer = true;
			Destroy(other.gameObject);
			itemPickupAudio.Play();
		} else if (other.tag == "PhoneStunPickup") {
			hasPhoneStun = true;
			Destroy(other.gameObject);
			itemPickupAudio.Play();
		} else if (other.tag == "TextMessage") {
			TextMessage message = other.GetComponent<TextMessage>();
			if (message.message != null && message.message != "") {
				Camera.main.GetComponent<CameraControl>().pauseAndDrawTextMessage(message.message);
			} else {
				Camera.main.GetComponent<CameraControl>().pauseAndDrawInfoCard(message.infocard);
			}
			Destroy(other.gameObject);
		} else if (other.tag == "Pickup") {
			Pickup pickup = other.GetComponent<Pickup>();
			curHealth = Mathf.Min(curHealth + pickup.healthRestored, maxHealth);
			curPhoneCharge = Mathf.Min(curPhoneCharge + pickup.energyRestored, maxPhoneCharge);
			if (pickup.onPickupAnimation != null) {
				Instantiate(pickup.onPickupAnimation, pickup.gameObject.transform.position + new Vector3(0, 0, -1), Quaternion.identity);
			}
			Destroy(other.gameObject);
			itemPickupAudio.Play();
		}
	}
	
	public void setCheckpoint(Vector3 checkpointPos) {
		if ((checkpointPos - currentCheckpoint).magnitude > 10) {
			hasCheckpoint = true;
			currentCheckpoint = checkpointPos;
			//Camera.main.GetComponent<CameraControl>().gotCheckpoint();
			Instantiate(checkpointNotification, transform.position, Quaternion.identity);
		}
	}
	
	void OnTriggerEnter(Collider other) {
		if (other.tag == "Checkpoint") {
			setCheckpoint(other.gameObject.transform.position);
		}
		if (other.tag == "ChargingStation") {
			batteryChargingAudio.Play();
		}
	}
	
	public void unlockUpgrade(string upgradeName) {
		if (upgradeName == "SwordPickup") {
			hasSword = true;
		} else if (upgradeName == "PhoneBulletPickup") {
			hasPhoneBullet = true;
		} else if (upgradeName == "PhoneLazerPickup") {
			hasPhoneLazer = true;
		} else if (upgradeName == "PhoneStunPickup") {
			hasPhoneStun = true;
		} else {
			Debug.LogError("Unknown unlockable: " + upgradeName);
		}
	}
	
	public void enterCutscene() {
		curState = PlayerState.Cutscene;
	}
	
	public void exitCutscene() {
		curState = PlayerState.PlayerInput;
	}
	
	public void GotHit(float damage){
		if(!invulnerable){
			invulnerable = true;
			curHealth -= damage;
			if (curHealth > 0) {
				float timeInvulnerable = 1.5f;	// the function "Invulnerable currently assumes this being 1.5f
				takingDamageAudio.Play();
				StartCoroutine(Invulnerable(Time.time + timeInvulnerable, Time.time));
			} else {
				curAnim.Play("die");
				curState = PlayerState.Dead;
				//
				GetComponent<GameOver>().StartGameOverSequence();
			}
		}	
	}

	public void knockBack(Vector3 cardinalDirection) {
		curState = PlayerState.KnockedBack;
		knockBackDirection = cardinalDirection;
		if (cardinalDirection.x > 0.1) {
			curAnim.Play("leftKnockback");
		} else if (cardinalDirection.x < -0.1) {
			curAnim.Play("rightKnockback");
		} else {
			curAnim.Play("upKnockback");
		}
		// play animation based on dir
	}
	
	public IEnumerator Invulnerable(float timeInvulnerable, float startTime){
		Transform playerSprite = transform.FindChild("PlayerSprite");
		while(Time.time < timeInvulnerable){
			float curTime = Time.time - startTime;
			
			if( (curTime < 0.1) || (curTime > 0.3f && curTime < 0.4f) || (curTime > 0.7f && curTime < 0.8f) || (curTime > 1.05f && curTime < 1.15f) || (curTime > 1.4f && curTime < 1.5f)){
				playerSprite.renderer.enabled = false;
			}
			else
				playerSprite.renderer.enabled = true;
				
			yield return null;
		}
		playerSprite.renderer.enabled = true;
		invulnerable = false;
	}	
	
	public void ReloadPlayerAfterDeath(){
		if (hasCheckpoint) {
			transform.position = currentCheckpoint;
			curHealth = maxHealth;
			curPhoneCharge = maxPhoneCharge;
			curStamina = 0;
			curState = PlayerState.PlayerInput;
			invulnerable = false;
			GotHit(0); // make the sprite flash
		} else {
			Application.LoadLevel(Application.loadedLevel);
		}
	}
	
}
