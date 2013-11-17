using UnityEngine;
using System.Collections;

public class RepeatedlySpawn : MonoBehaviour {
	public float spawnInterval;
	private float nextSpawnTime;
	public Transform transformToSpawn;
	
	void Start() {
	}
	
	void FixedUpdate() {
		if (Time.time > nextSpawnTime) {
			nextSpawnTime += spawnInterval;
			Instantiate(transformToSpawn, transform.position, Quaternion.identity);
		}
	}
}
