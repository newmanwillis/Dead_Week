using UnityEngine;
using System.Collections;

public class ZombieAttackSlash : MonoBehaviour {

	public Transform LeftAttack;
	public Transform RightAttack;
	public AudioClip[] AttackSounds;

	private Transform Zombie;
	private Transform Attack;
	private tk2dSpriteAnimator AttackAnim;
	private tk2dSpriteAnimator curAnim;
	private ZombieSM _state;
	private ZombieHealth zombieHealth;
	private enum direction {up, down, left, right};
	
	private float _attackDelay = 1f;
	private bool _readyToAttack = false;
	private float _timeSinceLastAttack = 0;

	private Vector3 playerAttackedPosition;
	private Transform Player;

	void Start () {
		Zombie = transform.parent;
		_state = Zombie.GetComponent<ZombieSM>();
		curAnim = Zombie.GetComponent<tk2dSpriteAnimator>();
		zombieHealth = Zombie.GetComponent<ZombieHealth>();
	}
	
	void OnTriggerStay(Collider other){
		// print ("in Attack Collider");
		if(other.tag == "Player"  &&  _state.curState == ZombieSM.ZombieState.Chase && _timeSinceLastAttack < Time.time){

			_state.curState = ZombieSM.ZombieState.Attack;					// switch to Attack State

			//StartCoroutine(AttackPause());
			// Call coroutine


			//Transform Player = other.transform;
			Player = other.transform;

			StartCoroutine(BeginAttack());
			/*
			Vector3 posDifference = Player.position - Zombie.position;
			if(posDifference.x < 0){	// Left Attack 
				Attack = (Transform)Instantiate(LeftAttack, Player.position, Quaternion.identity);
			}
			else{						// RightAttack
				Attack = (Transform)Instantiate(RightAttack, Player.position, Quaternion.identity);
			}
			Attack.transform.position += new Vector3(0, 0, -1);
			Attack.parent = transform;
			AttackAnim = Attack.GetComponent<tk2dSpriteAnimator>();
			
			// call coroutine to pause before attack
			// check in ontriggerstay when its sone if play is in range
			
			PlayRandomAttackSound();
			AttackAnim.Play();
			_timeSinceLastAttack = Time.time + _attackDelay;				// Delays attack
			StartCoroutine(RemoveAttackAnimation());						// Remove attack animation when finished
			//print ("between 2 coroutines");
			StartCoroutine(MovementPause(1f));								// FIX, stop when out of detection range, but in chase mode.
			*/
		}
		
	}

	IEnumerator BeginAttack(){
		direction facing = FindDirection();
		ChooseAttackAnimation(facing);

		Vector3 posDifference = Player.position - Zombie.position;

		yield return new WaitForSeconds(0.2f);
		if(_state.curState != ZombieSM.ZombieState.Attack){
			yield break;
		}

		if(posDifference.x < 0){	// Left Attack 
			Attack = (Transform)Instantiate(LeftAttack, Player.position, Quaternion.identity);
		}
		else{						// RightAttack
			Attack = (Transform)Instantiate(RightAttack, Player.position, Quaternion.identity);
		}
		Attack.transform.position += new Vector3(0, 0, -1);
		Attack.parent = transform;
		AttackAnim = Attack.GetComponent<tk2dSpriteAnimator>();
		
		// call coroutine to pause before attack
		// check in ontriggerstay when its sone if play is in range
		
		PlayRandomAttackSound();
		AttackAnim.Play();
		_timeSinceLastAttack = Time.time + _attackDelay;				// Delays attack
		StartCoroutine(RemoveAttackAnimation());						// Remove attack animation when finished
		//print ("between 2 coroutines");
		StartCoroutine(MovementPause(1f));								// FIX, stop when out of detection range, but in chase mode.
		
	}
	
	
	IEnumerator RemoveAttackAnimation(){
		//print ("in removeattackanimation");
		while(AttackAnim.Playing){
			if( _state.curState == ZombieSM.ZombieState.Die){
				Destroy(Attack.gameObject);				
				yield break;	
			}	
			//print ("in removeattackanimation LOOP");
			yield return null;	
		}
		Destroy(Attack.gameObject);
		//_state.curState = ZombieSM.ZombieState.Chase;	
	}

	/*IEnumerator AttackPause(){



		print ("first");
		//
		curAnim.Stop();
		yield return new WaitForSeconds(0.2f);

		direction facing = FindDirection();
		ChooseAttackAnimation(facing);
		print ("second");



		yield return new WaitForSeconds(0.4f);





		print ("thrid");
	}*/


	
	IEnumerator MovementPause(float standTime){
		float totalTime = Time.time + standTime;
		while(Time.time < totalTime){
			if(_state.curState == ZombieSM.ZombieState.Die){
				yield break;	
			}
			yield return null;
		}
		if(!zombieHealth.isStunned)
			_state.SetStateToChase();		
		
		/*
		//print ("in movement pause");
		yield return new WaitForSeconds(standTime);
		
		
		
		
		if(_state.curState == ZombieSM.ZombieState.Die){
			yield break;	
		}
		
		// curAnim.StopAndResetFrame();
		if(!zombieHealth.isStunned)
			_state.SetStateToChase();
		//_state.curState = ZombieSM.ZombieState.Chase;
		*/
	}

	private void PlayRandomAttackSound(){
		int rand = Random.Range(0, AttackSounds.Length);
		transform.parent.audio.clip = AttackSounds[rand];
		transform.parent.audio.Play();
	}

	private direction FindDirection(){
		string clipName = curAnim.CurrentClip.name;
		if(clipName.Contains("Down") || clipName.Contains("Forward"))
			return direction.down;
		else if(clipName.Contains("Up") || clipName.Contains("Backward"))
			return direction.up;		
		else if(clipName.Contains("Left"))
			return direction.left;
		else
			return direction.right;			
	}

	private void ChooseAttackAnimation(direction facing){
		switch(facing){
		case direction.up:
			curAnim.Play("attackUp");
			break;
		case direction.down:
			curAnim.Play("attackDown");
			break;
		case direction.left:
			curAnim.Play("attackLeft");
			break;
		case direction.right:
			curAnim.Play("attackRight");
			break;
		}	
	}

}
