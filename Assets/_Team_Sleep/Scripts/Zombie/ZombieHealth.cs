using UnityEngine;
using System.Collections;

public class ZombieHealth : MonoBehaviour {
	
	public int health = 100;

	public bool IsStunned { get; private set; }
	public float LastHitTime { get; private set; }
	float stunEnd;
	
	public bool isDead = false;
	private bool hitMultipleTimes = false;

	private tk2dSpriteAnimator curAnim;		
	private ZombieSM _state;
	private CharacterController CC;
	//private ZombieFollowPlayer myZombieFollowPlayer;
	
	// Use this for initialization
	void Start () {
		LastHitTime = 0;
		curAnim = GetComponent<tk2dSpriteAnimator>();
		_state = GetComponent<ZombieSM>();
		CC = GetComponent<CharacterController>();
		// myZombieFollowPlayer = GetComponentInChildren<ZombieFollowPlayer>();
	}
	
	public void TakeDamage(int damage){		// perhaps change parameters to get enum of what attack killed it to determine death animation
		LastHitTime = Time.time;
		health -= damage;
		if(health <= 0 && !isDead){
			isDead = true;
			_state.curState = ZombieSM.ZombieState.Die;
			// Set to false, so their processes wont interfere with the death animation
			
			// Find better solution than turning them off
			transform.FindChild("ZombieAttackRange").gameObject.SetActive(false);
			// transform.FindChild("ZombieDetectionRange").gameObject.SetActive(false);
			CC.enabled = false;
			ChooseDeathAnimation();
			StartCoroutine( waitForAnimationToEnd());
		}
		if(isDead){		// In case the player keeps attacking the zombie even though it has already died
			// Don't process anything
		}
		else if(_state.curState == ZombieSM.ZombieState.TakingDamage){
			hitMultipleTimes = true;
		}
		else{  	// taking damage
			//print ("HIT");
			//if(_state.curState != ZombieSM.ZombieState.TakingDamage)
			_state.curState = ZombieSM.ZombieState.TakingDamage;
			//else
			//hitMultipleTimes = true;
			StartCoroutine( PauseWhenHit(0.4f));
		}
	}
	
	// Update is called once per frame
	/*void Update () {
		if (IsStunned) {
			if (Time.time >= stunEnd) {
				IsStunned = false;
			}
		}
		
		if(health <= 0  && !isDead){
			isDead = true;
			curAnim.Play("deathDown");
			// curAnim.Play(getCorrectDeathAnimation());
		 	StartCoroutine( waitForAnimationToEnd());
			//StartCoroutine(RemoveZombie(3.0f));
			//Destroy(transform.parent.gameObject);	
		}
	}*/
	/*
	string getCorrectDeathAnimation() {
		switch (myZombieFollowPlayer.curDirection) {
		case Player.FacingDirection.Down:
			return "deathDown";
		case Player.FacingDirection.Left:
			return "deathLeft";
		case Player.FacingDirection.Right:
			return "deathRight";
		case Player.FacingDirection.Up:
			return "deathUp";
		}
		return null;
	}*/
	
	public void Stun(float duration){
		// _state.curState = ZombieSM.ZombieState.Stunned;
		_state.curState = ZombieSM.ZombieState.TakingDamage;
	 	StartCoroutine(PauseWhenHit(duration));	
	}
	
	public void stunFor(float duration) {
		IsStunned = true;
		if (duration > stunEnd - Time.time) {
			stunEnd = Time.time + duration;
		}
	}
	
	void Knockback(){
		float xOffset = 1f;
		float yOffset = 1f;
		Vector3 playerPos = GameObject.FindGameObjectWithTag("Player").transform.position;
		Vector3 newZombiePos = transform.position;
		//if(playerPos.x > transform.position){a
				
		//}
		
	}
	
	IEnumerator waitForAnimationToEnd(){
		
		while(curAnim.Playing){
			//print("in while loop");
			yield return null;
		}
		curAnim.Stop();
		StartCoroutine(RemoveZombie(0.5f));
	}
	
	IEnumerator RemoveZombie(float deathTime){
		yield return new WaitForSeconds(deathTime);
		//Destroy(transform.parent.gameObject);	
		Destroy(gameObject);
	}
	
	/*
	IEnumerator PauseWhenHit(float pauseTime){
		curAnim.StopAndResetFrame();
		yield return new WaitForSeconds(pauseTime);
		if(_state.curState == ZombieSM.ZombieState.Die){
			yield break;	
		}
		_state.SetStateToChase();	
	}*/
	
	IEnumerator PauseWhenHit(float rawPauseTime){
		float pauseTime = Time.time + rawPauseTime;
		curAnim.StopAndResetFrame();
		//gotHit = false;
		while(Time.time < pauseTime){
			if(hitMultipleTimes == true){ // || _state.curState == ZombieSM.ZombieState.Die){
				hitMultipleTimes = false;
				StartCoroutine(PauseWhenHit(rawPauseTime));
				yield break;
			}
			if(_state.curState == ZombieSM.ZombieState.Die){
				yield break;	
			}
			yield return null;	
		}
		hitMultipleTimes = false;
		_state.SetStateToChase();	
	}
	
	public void ChooseDeathAnimation(){
		string clipName = curAnim.CurrentClip.name;
		if(clipName.Contains("Down") || clipName.Contains("Forward"))
			curAnim.Play("deathDown");	
		else if(clipName.Contains("Up") || clipName.Contains("Backward"))
			curAnim.Play("deathUp");
		else if(clipName.Contains("Left"))
			curAnim.Play("deathLeft");
		else
			curAnim.Play("deathRight");
	}
}

