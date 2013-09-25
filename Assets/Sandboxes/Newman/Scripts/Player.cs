using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	
	public float movementSpeed = 0.1f;
	public int maxHealth = 100;
	public int maxPhoneEnergy = 100;
	public int maxStamina = 100;
	
	public enum PlayerState {PlayerInput, SwordAttack, PhoneAttack, TakingDamage, Dead, Cutscene, Menu};
	//public enum FacingDirection {Up = 0, UpLeft = 45, Left = 90, DownLeft = 135, Down = 180, RightDown = 215, Right = 270, UpRight = 315};
	public enum FacingDirection {Up = 0, Left = 90, Down = 180, Right = 270};
	
	private PlayerState curState;
	private FacingDirection curDirection;
	private tk2dSpriteAnimator curAnim;
	int curHealth;
	int curPhoneEnergy;
	int curStamina;
	
	Transform phoneBullet;
	Transform phoneBeam;	
	
	// private float attackAngle = 0;		// change to enum?
	// private Vector3 facingAngle = Vector3.up;		// REMOVE?

	// Use this for initialization
	void Start () {
		
		curState = PlayerState.PlayerInput;
		curDirection = FacingDirection.Down;
		curAnim = transform.FindChild("PlayerSprite").GetComponent<tk2dSpriteAnimator>();
		curHealth = maxHealth;
		curPhoneEnergy = maxPhoneEnergy;
		curStamina = maxStamina;
	}
	
	// Update is called once per frame
	void Update () {
		
		switch( curState)
		{
			case PlayerState.PlayerInput:
			AttackInput();  // Check for player attacks
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
	
	void AttackInput(){
		
		// change to enum/switch 
		
		// Sword Attack
		if(Input.GetKey(KeyCode.A)){	/*		
			curState = PlayerState.SwordAttack;
			print ("curState in attack: " + curState); 
			switch(curDirection)
			{
				case FacingDirection.Up:
				curAnim.Play("PlayerAttackBackLeft");				
				break;
				case FacingDirection.Left:
				curAnim.Play("PlayerAttackFrontLeft");
				break;
				case FacingDirection.Down:
				curAnim.Play("PlayerAttackFrontLeft");				
				break;
				case FacingDirection.Right:
				curAnim.Play("PlayerAttackFrontRight");				
				break;				
				
			}
			curAnim.Resume();
			StartCoroutine(waitForAnimationtoEnd());*/
		}
		// Phone Bullet/Beam
		else if(Input.GetKey(KeyCode.S)){ // change this
			// play sprite
		}
		//Phone Stun
		else if(Input.GetKey(KeyCode.D)){
			
		}	
	}
	
	IEnumerator waitForAnimationtoEnd(){
		
		while(curAnim.Playing){
			yield return null;
		}
		curState = PlayerState.PlayerInput;
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
}
