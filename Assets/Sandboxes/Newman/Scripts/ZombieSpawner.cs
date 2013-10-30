using UnityEngine;
using System.Collections;

public class ZombieSpawner : MonoBehaviour {

	// public....direction up/down
	public float spawnTimer = 5f;
	private float spawnDelay;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		// 1. Check there's nothing inside
		// 2. Put timer between each zombie spawn
		// 3. make zombies move desired amount/direction into hallway
		
		
		
	}
	
	void OnTriggerEnter(Collider other){
		
	}
	
	void OnTriggerStay(Collider other){
			
	}
	
	void OnTriggerExit(Collider other){
		
	}
	
}
