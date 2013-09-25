using UnityEngine;
using System.Collections;

public class TestEnemy : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	}
	
	void OnTriggerEnter(Collider other){		
		if(other.tag == "PlayerAttack"){
			Debug.Log("Hit by PlayerAttack");
			//print ("Hiit by player");		
		}
	}
	
	void OnCollisionEnter(Collision collision){
		
		
		if(collision.gameObject.tag == "Player"){
			Debug.Log("Hit by Player");
			//print ("Hiit by player");		
		}
	}	
}
