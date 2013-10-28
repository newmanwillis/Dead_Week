using UnityEngine;
using System.Collections;

public class ZombieAttack : MonoBehaviour {
	public float biteDelay;
	public int attackDamage;
	bool canAttack = true;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnTriggerStay(Collider other){
		if(other.tag == "Player" && canAttack) {
			ZombieHealth myHealth = gameObject.transform.parent.GetComponent<ZombieHealth>();
			if (!myHealth.isStunned && !myHealth.isDead) {
				other.GetComponent<Player>().curHealth -= attackDamage;
				canAttack = false;
				StartCoroutine(BiteCooldown());
			}
		}	
	}
	
	IEnumerator BiteCooldown(){
		yield return new WaitForSeconds(biteDelay);
		canAttack = true;
	}
}
