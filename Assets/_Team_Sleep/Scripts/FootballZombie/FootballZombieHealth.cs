using UnityEngine;
using System.Collections;

public class FootballZombieHealth : MonoBehaviour {
	public int health;
	public int maxHealth;
	public float lastSwordHitTime = 0;
	public float vulnerableTime;
	
	private tk2dSpriteAnimator anim;
	
	public bool isVulnerable = false;
	private float becomeInvulnerableTime = 0;
	private GameObject barrier;	

	// Use this for initialization
	void Start () {
		health = maxHealth;
		anim = GetComponent<tk2dSpriteAnimator>();
		barrier = gameObject.transform.FindChild("Barrier").gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.time > becomeInvulnerableTime) {
			isVulnerable = false;
			barrier.SetActive(true);
		}
	}
	
	public void takeDamage(int damage) {
		if (isVulnerable) {
			health -= damage;
			if (health <= 0) {
				// start death animation
				StartCoroutine(waitForAnimationAndDie());
			}
		}
	}
	
	public void becomeVulnerable() {
		isVulnerable = true;
		becomeInvulnerableTime = Time.time + vulnerableTime;
		barrier.SetActive(false);
	}
	
	IEnumerator waitForAnimationAndDie() {
		while (anim.Playing) {
			yield return null;
		}
		Destroy(gameObject);
	}
}
