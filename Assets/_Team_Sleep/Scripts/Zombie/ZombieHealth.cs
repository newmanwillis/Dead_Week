using UnityEngine;
using System.Collections;

public class ZombieHealth : MonoBehaviour {
	
	public int health = 100;
	
	private tk2dSpriteAnimator curAnim;	
	public bool IsStunned { get; private set; }
	float stunEnd;
	
	public bool isDead = false;
	
	// Use this for initialization
	void Start () {
		curAnim = GetComponent<tk2dSpriteAnimator>();
		
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
		 	StartCoroutine( waitForAnimationToEnd());
			//StartCoroutine(RemoveZombie(3.0f));
			//Destroy(transform.parent.gameObject);	
		}
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
	
	void OnTriggerEnter(Collider other){
		if(other.tag == "PlayerAttack"){
			health -= 50;
		}
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
		Destroy(transform.parent.gameObject);	
	}
	
}

