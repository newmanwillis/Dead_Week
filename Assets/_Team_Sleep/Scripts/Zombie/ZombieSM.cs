using UnityEngine;
using System.Collections;

public class ZombieSM : MonoBehaviour {
	
	public enum ZombieState {Wander, Chase, Attack, TakingDamage, Stunned, Die};
	public ZombieState curState;
	
	private tk2dSprite sprite;
	private tk2dSpriteAnimator curAnim;
	
	// scripts on Zombie
	private ZombieWander _wander;
	private ZombieChase _chase;
	
	// Use this for initialization
	void Start () {
		sprite = GetComponent<tk2dSprite>();
		curAnim = GetComponent<tk2dSpriteAnimator>();
		
		//Debug.Log("in Zombie Start");		
		_wander = transform.GetComponent<ZombieWander>();
		_chase = transform.FindChild("ZombieDetectionRange").GetComponent<ZombieChase>();
		
		curState = ZombieState.Wander;
		// Do the wander change in here, then add a "changeStateToWander" Function
	}
	
	// Update is called once per frame
	void Update () {
		//print("in Zombie Update");
		switch(curState){
			case ZombieState.Wander:	// Change this so it straight calls the same method being used, dont have to check this every time.
				if(!_wander._isWandering)
					_wander.StartWanderProcess();
				break;  // Do nothing, let ZombieWander script continue
			case ZombieState.Chase:
				break;
			case ZombieState.Attack:
				break;			
			// case ZombieState.TakingDamage:
			
		}
		
	}
	
	public void SetStateToChase(){
		if(curState != ZombieSM.ZombieState.Die && curState != ZombieSM.ZombieState.Chase){
			curState = ZombieState.Chase;
			_chase.PreCalculateChase();
		}
	}
	
	public void SetStateToStun(float stunDuration){
		curState = ZombieState.Stunned;
		StartCoroutine(Stun(stunDuration));
	}
	
	IEnumerator Stun(float stunDuration){
		float stunTime = Time.time + stunDuration;
		
		curAnim.Stop();
		Color origColor = sprite.color;
		float colorMod = 3f;
		Color stunColor = new Color(origColor.r/colorMod, origColor.g/colorMod, origColor.b/colorMod, origColor.a);
		sprite.color = stunColor;
		while(Time.time < stunTime){
			if(curState == ZombieState.Die){
				sprite.color = origColor;
				curAnim.Play();					
				yield break; 
			}
			
			print ("IS PLAYING?: " + curAnim.Playing);
			
			yield return null;
		}

		sprite.color = origColor;
		curAnim.Play();	
		//if(!curAnim.Playing){
		//	SetStateToChase();
		//}
		//else{
		//while(curAnim.Playing){
			// wait for gethit animation to end
		//	print ("STUCK IN LOOP");
		//}
		if(curState == ZombieState.Stunned){
			SetStateToChase();
		}
	}
	
}
