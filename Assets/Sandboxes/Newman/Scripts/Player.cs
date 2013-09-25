using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	
	public float movementSpeed = 0.1f;
	public int health = 100;
	public int phoneEnergy = 100;
	public int stamina = 100;
	
	public enum PlayerState {PlayerInput, SwordAttack, PhoneAttack, Dead, Cutscene};
	//public enum FacingDirection {Up = 0, UpLeft = 45, Left = 90, DownLeft = 135, Down = 180, RightDown = 215, Right = 270, UpRight = 315};
	public enum FacingDirection {Up = 0, Left = 90, Down = 180, Right = 270};
	
	private PlayerState curState;
	private FacingDirection curDirection;
	// private int curState;
	private tk2dSpriteAnimator curAnim;
	
	private float attackAngle = 0;		// change to enum?
	private Vector3 facingAngle = Vector3.up;		// REMOVE?

	// Use this for initialization
	void Start () {
		
		curState = PlayerState.PlayerInput;
		curDirection = FacingDirection.Down;
		//curState = (int) PlayerStates.PlayerInput;
		curAnim = transform.FindChild("PlayerSprite").GetComponent<tk2dSpriteAnimator>();
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
		
		if(curState == PlayerState.PlayerInput) // change
			MovementInput();
		
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
		else */if(Input.GetKey(KeyCode.UpArrow)){
			curDirection = FacingDirection.Up;
			attackAngle = 0;
			facingAngle = Vector3.up;
			curAnim.Resume();
			curAnim.Play("PlayerWalkingBack");			
		}		
		else if(Input.GetKey(KeyCode.DownArrow)){
			curDirection = FacingDirection.Down;
			attackAngle = 180;
			facingAngle = Vector3.down;
			curAnim.Resume();
					curAnim.Play("PlayerWalkingFront");			
		}
		else if(Input.GetKey(KeyCode.RightArrow)){
			curDirection = FacingDirection.Right;
			attackAngle = 270;
			facingAngle = Vector3.right;
			curAnim.Resume();
			curAnim.Play("PlayerWalkingRight");			
		}		
		else if(Input.GetKey(KeyCode.LeftArrow)){
			curDirection = FacingDirection.Left;
			attackAngle = 90;		
			facingAngle = Vector3.left;
			curAnim.Resume();
			curAnim.Play("PlayerWalkingLeft");
		}	
				
		// Pauses walking animation when nothing is happening.
		if(!(Input.GetKey(KeyCode.UpArrow)) && !(Input.GetKey(KeyCode.LeftArrow)) && !(Input.GetKey(KeyCode.DownArrow)) && !(Input.GetKey(KeyCode.RightArrow))){
			//curAnim.Stop();
			curAnim.StopAndResetFrame();
			// CHANGE to reset the animation
			// CHANGE so this doesnt stop when animating attack, etc...
			// curAnim.Pause();			
		}
	}
	
	void AttackInput(){
		
		// Sword Attack
		if(Input.GetKey(KeyCode.A)){			
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
	
	IEnumerator waitForAnimationtoEnd(){
		
		while(curAnim.Playing){
			yield return null;
		}
		curState = PlayerState.PlayerInput;
		curAnim.Pause();
		switch(curDirection)
		{		
			case FacingDirection.Up:
			curAnim.Play("PlayerWalkingLeft");				
			break;
			case FacingDirection.Left:
			curAnim.Play("PlayerWalkingLeft");
			break;
			case FacingDirection.Down:
			curAnim.Play("PlayerWalkingFront");				
			break;
			case FacingDirection.Right:
			curAnim.Play("PlayerWalkingRight");				
			break;	
		}	
	}
}
