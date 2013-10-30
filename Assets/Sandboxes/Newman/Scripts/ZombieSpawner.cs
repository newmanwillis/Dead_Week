using UnityEngine;
using System.Collections;

public class ZombieSpawner : MonoBehaviour {

	// public....direction up/down
	// public float StartSpawnDelay = 2
	public float SpawnTimer = 5f;
	private float SpawnDelay;
	
	private int ColliderCount = 0;
	
	// Use this for initialization
	void Start () {
		StartCoroutine(SpawnZombie());
	}
	
	// Update is called once per frame
	void Update () {
		// 1. Check there's nothing inside
		// 2. Put timer between each zombie spawn
		// 3. make zombies move desired amount/direction into hallway
		
		
		
	}
	
	void OnTriggerEnter(Collider other){
		ColliderCount++;
	}
	
	void OnTriggerStay(Collider other){
			
	}
	
	void OnTriggerExit(Collider other){
		ColliderCount--;
	}
	
	IEnumerator SpawnZombie(){
		
		if(ColliderCount == 0){
			print ("A");
			yield return new WaitForSeconds(SpawnTimer);	
			print ("B");
		}
		
	}
	
}
