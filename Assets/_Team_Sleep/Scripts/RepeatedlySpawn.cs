using UnityEngine;
using System.Collections;

public class RepeatedlySpawn : MonoBehaviour {
	public float spawnInterval = 4;
	private float nextSpawnTime;
	public Transform transformToSpawn;

	public string pathName;

	public bool spawning = true;
	
	void Start() {
		nextSpawnTime = 0;
		if (transformToSpawn == null) {
			Debug.LogError("Not assigned");
			transformToSpawn = ((GameObject) Resources.Load("WireLight")).transform;
		}
	}
	
	void FixedUpdate() {
		if (spawning && nextSpawnTime < Time.time) {
			Debug.Log ("name: " + gameObject.name + " nextSpawnTime: " + nextSpawnTime + " time: " + Time.time + " interval: " + spawnInterval);
			nextSpawnTime = Time.time + spawnInterval;
			GameObject obj = ((Transform)Instantiate(transformToSpawn, transform.position, Quaternion.identity)).gameObject;
			obj.GetComponent<FollowLinearPath>().setPath(pathName);
		}
	}
}
