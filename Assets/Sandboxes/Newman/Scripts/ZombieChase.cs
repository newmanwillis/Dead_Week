// This script requires objects to be labeled with correct tags and layers
// Tags: Player, Wall
// Layers: Default, Wall, Player

// problems with constant raycasting if ontriggerstay & problem with not finding player sometimes with ontriggerenter
// rotate detection range collider too? - would require parent object to it.
using UnityEngine;
using System.Collections;

public class ZombieChase : MonoBehaviour {
	
	float _speed = 24f;
	
	float _lookForPlayerTimer = 0;	
	float _lookForPause = 0.5f;
	// float _chasePlayerTimer = 0;
	// float _chasePause = 0.5f;
	float _stopChaseTimer = 0;
	float _stopChaseDelay = 2.0f; 
	
	float _xOffset = 3f;
	float _yOffset = 7f;
	

	private bool _foundPlayer = false;
	private bool _outsideDetectionRange = true;
	
	Transform Player;
	Transform Zombie;	
	private ZombieSM _state;
	private tk2dSpriteAnimator curAnim;
	
	// Use this for initialization
	void Start () {
		Zombie = transform.parent;
		_state = Zombie.GetComponent<ZombieSM>();
		curAnim = Zombie.GetComponent<tk2dSpriteAnimator>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnTriggerStay(Collider other){
		
	if(!_foundPlayer){	
		if(other.tag == "Player" && !_foundPlayer && _lookForPlayerTimer < Time.time){		// Keeps checking for player every 0.5 seconds if in detection range
																							// but behind a wall. This is so it doesn't raycast every update.
			int layerMask = ~(1 << 0);
			//layerMask = ~layerMask;
			
			RaycastHit hit;
			if(Physics.Raycast(Zombie.position, other.transform.position - Zombie.position, out hit, 40, layerMask)){
				
				if(hit.transform.tag == "Wall"){
					_lookForPlayerTimer = Time.time + _lookForPause;					
				}
				else if(hit.transform.tag == "Player"){
					_foundPlayer = true;
					_outsideDetectionRange = false;
					Player = other.transform;
					_state.curState = ZombieSM.ZombieState.Chase;
					CalculateChase();					
				}			
				/*
				if(hit.transform.gameObject.layer == LayerMask.NameToLayer( "Wall" )){
					print("hit wall layer");
				}
				if(hit.transform.gameObject.layer == LayerMask.NameToLayer( "Player" )){
					print("hit player layer");
				}	*/		
			}
		}

		}
	}
	
	
	void CalculateChase(){
		
	if(_outsideDetectionRange && _stopChaseTimer < Time.time){
		_state.curState = ZombieSM.ZombieState.Wander;
		_foundPlayer = false;
		curAnim.Stop();
	}
	else{
		Vector3 difference = Player.position - Zombie.position;
		
		float furtherAxis;
		if(Mathf.Abs(difference.x) <= _xOffset && Mathf.Abs(difference.y) <= _yOffset){
			difference.x = 0;
			difference.y = 0;
			furtherAxis = 1;
		}
		else if(Mathf.Abs(difference.x) <= _xOffset){
			difference.x = 0;
			furtherAxis = Mathf.Abs(difference.y);
		}
		else if(Mathf.Abs(difference.y) <= _yOffset){
			difference.y = 0;
			furtherAxis = Mathf.Abs(difference.x);
		}		
		else{
			furtherAxis = Mathf.Max(Mathf.Abs(difference.x), Mathf.Abs(difference.y));
		}
		Vector3 direction = difference / furtherAxis;
		//ChooseDirectionAnimation(direction.x, direction.y);
		
		if(Mathf.Abs (direction.x) > Mathf.Abs(direction.y)){
			if(direction.x < 0)
				curAnim.Play("walkingLeft");
			else
				curAnim.Play("walkingRight");
			
			if(Mathf.Abs(direction.y) < 0.35f)
				direction.y = 0;
		}
		else if(Mathf.Abs (direction.x) < Mathf.Abs(direction.y)){
			if(direction.y < 0)
				curAnim.Play("walkingForward");
			else
				curAnim.Play("walkingBackward");
			
			if(Mathf.Abs(direction.x) < 0.35f)
				direction.x = 0;			
		}
		else{  
			//  they are equal	
		}		
		
		
		Vector3 move = direction * _speed;
		float chaseTime = Time.time + Random.Range(0.4f, 1.1f);
		StartCoroutine(Chase(move, chaseTime));
	}	
		
		// randomize lesser variable 
	}
	
	IEnumerator Chase(Vector3 move, float chaseTime){
		
		while(Time.time < chaseTime){ 
			Zombie.GetComponent<CharacterController>().Move(move * Time.deltaTime);
			yield return null;
		}
		CalculateChase();
	}
	
	/*
	void ChooseDirectionAnimation(float x, float y){
		if(Mathf.Abs (x) > Mathf.Abs(y)){
			if(x < 0)
				curAnim.Play("walkingLeft");
			else
				curAnim.Play("walkingRight");			
		}
		else if(Mathf.Abs (x) < Mathf.Abs(y)){
			if(y < 0)
				curAnim.Play("walkingForward");
			else
				curAnim.Play("walkingBackward");				
		}
		else{  
			//  they are equal	
		}
	}*/
	
	void OnTriggerExit(Collider other){
		// Stop following player after some point
		if(other.tag == "Player"){
			_stopChaseTimer = Time.time + _stopChaseDelay;	
			_outsideDetectionRange = true;
		}
		
	}
}
