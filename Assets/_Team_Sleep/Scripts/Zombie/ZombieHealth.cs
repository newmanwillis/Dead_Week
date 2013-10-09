using UnityEngine;
using System.Collections;

public class ZombieHealth : MonoBehaviour {
	
	public int health = 100;
	
	private tk2dSpriteAnimator curAnim;	
	public bool IsStunned { get; private set; }
	float stunEnd;
	
	public bool isDead = false;
	
	private ZombieFollowPlayer myZombieFollowPlayer;
	
	// Use this for initialization
	void Start () {
		curAnim = GetComponent<tk2dSpriteAnimator>();
		// myZombieFollowPlayer = GetComponentInChildren<ZombieFollowPlayer>();
	}
	
	// Update is called once per frame
	void Update () {
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
	}
	/*f
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
	
}

