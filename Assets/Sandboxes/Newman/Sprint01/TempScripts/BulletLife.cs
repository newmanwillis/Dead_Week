using UnityEngine;
using System.Collections;

public class BulletLife : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnTriggerEnter(Collider other){
		if(other.tag != "Player" && other.tag != "Attack" && other.tag != "ZombieDetectionRange" && other.tag != "ZombieAttack"){
			Destroy(gameObject);	
		}
	}
}
