using UnityEngine;
using System.Collections;

public class ZombieHealth : MonoBehaviour {
	
	public int health = 100;
	
	public bool IsStunned { get; private set; }
	float stunEnd;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (IsStunned) {
			if (Time.time >= stunEnd) {
				IsStunned = false;
			}
		}
		
		if(health <= 0){
			Destroy(transform.parent.gameObject);	
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

}
