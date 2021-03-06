﻿using UnityEngine;
using System.Collections;

public class ZombieHealth : MonoBehaviour {
		
	public Transform[] Drops;
	public AudioClip[] DeathSounds;
	public AudioClip[] HitSounds;
	public float DropChance = 0.13f;
	public float hitPauseTime = 0.5f;
	
	public bool CountDeath = false;
	
	public enum HitTypes {sword, burstLaser, stun, laser}
	private HitTypes lastHitType;

	public int[] Healths = {100};
	private int health = 100;

	// public bool IsStunned { get; private set; }
	public float LastHitTime { get; private set; }
	// float stunEnd;
	
	private enum direction {up, down, left, right};
	public bool isDead = false;
	
	private bool hitMultipleTimes = false;
	public bool isStunned = false;
	
	private tk2dSpriteAnimator curAnim;
	private tk2dSprite sprite;
	private ZombieSM _state;
	private CharacterController CC;
	
	// Use this for initialization
	void Start () {
		LastHitTime = 0;
		curAnim = GetComponent<tk2dSpriteAnimator>();
		sprite = GetComponent<tk2dSprite>();
		_state = GetComponent<ZombieSM>();
		CC = GetComponent<CharacterController>();

		int healthIndex = Random.Range(0, Healths.Length);
		health = Healths[healthIndex];
	}
	
	public void TakeDamage(int damage, HitTypes source, bool generateStamina = true){		// perhaps change parameters to get enum of what attack killed it to determine death animation
		LastHitTime = Time.time;
		
		health -= damage;
		if(health <= 0 && !isDead){
			isDead = true;
			lastHitType = source;
			_state.curState = ZombieSM.ZombieState.Die;
			
			// transform.FindChild("ZombieAttackRange").gameObject.SetActive(false);  // Find better solution than turning them off
			// transform.FindChild("ZombieDetectionRange").gameObject.SetActive(false);
			
			if (generateStamina) {
				DropItem();
				//GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().curStamina += 20;
			}
			
			if(CountDeath)	// removes from Zombie Spawner counter
				transform.parent.GetComponent<ZombieSpawner2>().ZombieCount--;

			// Just for Boss Zombie
			if(damage != 1007)
				StartCoroutine( TurnOffCharacterController(0.4f) );
			else
				CC.enabled = false;
			
			direction facing = FindDirection();
			ChooseDeathAnimation(facing);
			PlayDeathSound(source, generateStamina);
			//if (lastHitType == HitTypes.burstLaser) {
				// GetComponents<AudioSource>()[0].Play(); // disintigrate
			//}
			StartCoroutine( waitForAnimationToEnd());
		}
		else if(isDead){		// In case the player keeps attacking the zombie even though it has already died
			// Don't process anything

			// Just for Boss
			if(damage == 1007)
				CC.enabled = false;
		}
		else if(isStunned){  // If Zombie is stunned
			curAnim.Play ();
			StartCoroutine( PauseWhenHit(hitPauseTime));
		}
		else if(_state.curState == ZombieSM.ZombieState.TakingDamage){
			hitMultipleTimes = true;
		}
		else{  	// taking damage
			_state.curState = ZombieSM.ZombieState.TakingDamage;
			PlayRandomHitSound();
			StartCoroutine( PauseWhenHit(hitPauseTime));
		}
	}

	
	public void DropItem(){
		float randValue = Random.value;	
		if(randValue < DropChance){
			int randNum = Random.Range(0, Drops.Length);
			Vector3 dropPos = transform.position;
			dropPos.z = 0.01f;
			Instantiate(Drops[randNum], dropPos, Quaternion.identity);
		}
		
	}
	
	public void Stun(float stunDuration){
		StartCoroutine(StunTime(stunDuration));
	}
	
	IEnumerator StunTime(float stunDuration){
		float stunTime = Time.time + stunDuration;
		_state.curState = ZombieSM.ZombieState.TakingDamage;
		
		isStunned = true;
		curAnim.Stop();
		Color origColor = sprite.color;
		float colorMod = 10f;
		Color stunColor = new Color(origColor.r/colorMod, origColor.g/colorMod, origColor.b/colorMod, origColor.a);
		sprite.color = stunColor;
		while(Time.time < stunTime){
			if(_state.curState == ZombieSM.ZombieState.Die){
				sprite.color = origColor;
				isStunned = false;
				curAnim.Play();					
				yield break; 
			}			
			yield return null;
		}
		isStunned = false;
		sprite.color = origColor;
		curAnim.Play();	
		_state.SetStateToChase();
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
	
	IEnumerator PauseWhenHit(float rawPauseTime){
		float pauseTime = Time.time + rawPauseTime;
		direction facing = FindDirection();
		ChooseGotHitAnimation(facing);
		
		Color origColor = sprite.color;
		float colorMod = 3.5f;
		Color hitColor = new Color(origColor.r, origColor.g/colorMod, origColor.b/colorMod, origColor.a);
		float startTime = Time.time;
		
		while(Time.time < pauseTime){
			if(hitMultipleTimes == true){ // || _state.curState == ZombieSM.ZombieState.Die){
				hitMultipleTimes = false;
				sprite.color = origColor;
				StartCoroutine(PauseWhenHit(rawPauseTime));
				yield break;
			}
			if(_state.curState == ZombieSM.ZombieState.Die){
				sprite.color = origColor;
				yield break;	
			}
			// Make zombie flash red when hit
			float curTime = Time.time - startTime;
			if(curTime < 0.05f){// || (curTime > 0.06f && curTime < 0.09f)){// || (curTime < 0.2f && curTime > 0.25)){
				sprite.color = hitColor;
			}
			else{
				sprite.color = origColor;	
			}
			
			yield return null;	
		}
		sprite.color = origColor;
		hitMultipleTimes = false;
		curAnim.Play();
		if(!isStunned){
			_state.SetStateToChase();	
		}
		else{
			curAnim.StopAndResetFrame();	
		}
	}

	// waits for a short period before turning off character controller to evoid bugs	
	IEnumerator TurnOffCharacterController(float waitTime){
		yield return new WaitForSeconds(waitTime);
		CC.enabled = false;
		
		// Moves sprite back so it doesn't overlap enemies running over it
		Vector3 newPos = transform.position;
		newPos.z = 0f;
		transform.position = newPos;
	}
	
	private direction FindDirection(){
		//int facing;
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
	
	private void ChooseGotHitAnimation(direction facing){
		switch(facing){
			case direction.up:
				curAnim.Play("GotHitUp");
				break;
			case direction.down:
				curAnim.Play("GotHitDown");
				break;
			case direction.left:
				curAnim.Play("GotHitLeft");
				break;
			case direction.right:
				curAnim.Play("GotHitRight");			
				break;
		}
	}
	
	
	private void ChooseDeathAnimation(direction facing){
		switch(facing){
			case direction.up:
				if (lastHitType == HitTypes.burstLaser) {
					curAnim.Play("laserDeathUp");
				} else {
					curAnim.Play("deathUp");
				}
				break;
			case direction.down:
				if (lastHitType == HitTypes.burstLaser) {
					curAnim.Play("laserDeathDown");
				} else {
					curAnim.Play("deathDown");
				}
				break;
			case direction.left:
				if (lastHitType == HitTypes.burstLaser) {
					curAnim.Play("laserDeathLeft");
				} else {
					curAnim.Play("deathLeft");
				}
				break;
			case direction.right:
				if (lastHitType == HitTypes.burstLaser) {
					curAnim.Play("laserDeathRight");
				} else {
					curAnim.Play("deathRight");
				}
				break;
		}		
		
	/*	
		string clipName = curAnim.CurrentClip.name;
		if(clipName.Contains("Down") || clipName.Contains("Forward"))
			curAnim.Play("deathDown");	
		else if(clipName.Contains("Up") || clipName.Contains("Backward"))
			curAnim.Play("deathUp");
		else if(clipName.Contains("Left"))
			curAnim.Play("deathLeft");
		else
			curAnim.Play("deathRight");
	*/
	}

	private void PlayDeathSound(HitTypes source, bool stam){
		if(source == HitTypes.sword){
			//if(Random.value < 0.4f){
				audio.clip = DeathSounds[0];
				audio.Play();
			//}
		}
		else if(source == HitTypes.burstLaser && stam){
			audio.clip = DeathSounds[1];
			audio.Play();
		}
	}

	private void PlayRandomHitSound(){
		if(Random.value < 0.35f){
			int rand = Random.Range(0, HitSounds.Length);
			audio.clip = HitSounds[rand];
			audio.Play();
		}
	}
}

