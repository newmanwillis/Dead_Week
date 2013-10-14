using UnityEngine;
using System.Collections;

public class ZombieAttackSlash : MonoBehaviour {
	
	public Transform LeftAttack;
	public Transform RightAttack;
	
	private Transform Zombie;
	private Transform Attack;
	private tk2dSpriteAnimator AttackAnim;
	private ZombieSM _state;
	private tk2dSpriteAnimator curAnim;
	
	
	public float _attackDelay = 1f;
	private bool _readyToAttack = false;
	private float _timeSinceLastAttack = 0;
	
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
		// print ("in Attack Collider");
		if(other.tag == "Player"  &&  _state.curState == ZombieSM.ZombieState.Chase && _timeSinceLastAttack < Time.time){
			
			_state.curState = ZombieSM.ZombieState.Attack;					// switch to Attack State
			
			Transform Player = other.transform;
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
			
			// call coroutine to puase before attack
			// check in ontriggerstay when its sone if play is in range
			
			
			
			
			AttackAnim.Play();
			_timeSinceLastAttack = Time.time + _attackDelay;				// Delays attack
			StartCoroutine(RemoveAttackAnimation());						// Remove attack animation when finished
			//print ("between 2 coroutines");
			StartCoroutine(MovementPause(1f));								// FIX, stop when out of detection range, but in chase mode.
		}
		
	}
	
	//IEnumerator PauseBeforeAttack(){
			
	//}
	
	
	IEnumerator RemoveAttackAnimation(){
		//print ("in removeattackanimation");
		while(AttackAnim.Playing){
			//print ("in removeattackanimation LOOP");
			yield return null;	
			
		}
		Destroy(Attack.gameObject);
		//_state.curState = ZombieSM.ZombieState.Chase;
		
	}
	
	IEnumerator MovementPause(float standTime){
		//print ("in movement pause");
		yield return new WaitForSeconds(standTime);
		//print ("goign back to chase");
		// curAnim.StopAndResetFrame();
		_state.SetStateToChase();
		//_state.curState = ZombieSM.ZombieState.Chase;
		
	}
	
	
	
}
