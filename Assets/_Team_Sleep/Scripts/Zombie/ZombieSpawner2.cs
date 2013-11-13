using UnityEngine;
using System.Collections;

public class ZombieSpawner2 : MonoBehaviour {
	
	public Transform Zombie;
	public float SpawnDelay = 2.3f;
	
	public int ZombieCount = 0;	
	
	private int ColliderCount = 0;
	private int ZombieMax = 22;
	private int NumSpawners = 3;	
	private Vector3[] SpawnPositions;
	
	// Use this for initialization
	void Start () {
		SetSpawnPositions();
		
		StartCoroutine(SpawnZombies(1));		
	}
	
	
	void OnTriggerEnter(Collider other){
		if(!other.isTrigger)
			ColliderCount++;
	}
	
	void OnTriggerExit(Collider other){
		if(!other.isTrigger)
			ColliderCount--;
	}
	
	void SetSpawnPositions(){
		SpawnPositions = new Vector3[NumSpawners];
		float dist = 20;
		Vector3 pos = transform.position;
		
		pos.x -= dist;
		SpawnPositions[0] = pos;
		pos.x += dist;
		SpawnPositions[1] = pos;
		pos.x += dist;
		SpawnPositions[2] = pos;
	}
	
	IEnumerator SpawnZombies(float spawnTimer){
		
		yield return new WaitForSeconds(spawnTimer);
		if(ColliderCount == 1 && ZombieCount < ZombieMax){
			for(int i = 0; i < SpawnPositions.Length; i++){
				Transform ZombieCopy = ((Transform)Instantiate(Zombie, SpawnPositions[i], Quaternion.identity));	
				ZombieCopy.GetComponent<ZombieHealth>().CountDeath = true;
				ZombieCopy.parent = transform;
				ZombieCount++;
				// print ("SPAWN: " + ZombieCount);				
			}
			
		}
		StartCoroutine(SpawnZombies(SpawnDelay));
	}
	
	public void CountReduction(){
		
	}
	
}
