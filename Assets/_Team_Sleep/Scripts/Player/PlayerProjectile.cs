using UnityEngine;
using System.Collections;

public class PlayerProjectile : MonoBehaviour {
	public int damage;
	public int energyCost;
	public bool disappearOnCollide;
	public bool dealDamageOnce;
	public Transform spawnOther;
	bool dealtDamage = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnTriggerStay(Collider other) {
		print ("TAG: " + other.tag);
		if (other.tag == "Zombie" && !(dealDamageOnce && dealtDamage)) {
			//print ("in first");
			other.GetComponent<ZombieHealth>().health -= damage;
			dealtDamage = true;
		}
		if (other.tag == "Zombie" || other.tag == "Wall") {
			//print ("in 2nd");
		//if(other.tag != "Player" && other.tag != "Attack" && other.tag != "ZombieDetectionRange" && other.tag != "ZombieAttack"){
			if (disappearOnCollide) {
				if (spawnOther != null) {
					Instantiate(spawnOther, transform.position, Quaternion.identity);
				}
				Destroy(gameObject);
			}	
		}
	}
}
