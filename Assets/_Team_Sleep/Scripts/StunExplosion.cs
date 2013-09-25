using UnityEngine;
using System.Collections;

public class StunExplosion : MonoBehaviour {
	public float explosionDuration;
	public float stunDuration;
	
	float startTime;

	// Use this for initialization
	void Start () {
		startTime = Time.time;
		transform.position += new Vector3(0, 0, 1);  // make this show up behind zombies
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.time > startTime + explosionDuration) {
			Destroy(gameObject);
		}
	}
	
	void OnTriggerStay(Collider other) {
		if (other.tag == "Zombie") {
			other.GetComponent<ZombieHealth>().stunFor(stunDuration);
		}
	}
}
