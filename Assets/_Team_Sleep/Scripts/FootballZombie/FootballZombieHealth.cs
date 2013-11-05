using UnityEngine;
using System.Collections;

public class FootballZombieHealth : MonoBehaviour {
	public int health;
	public int maxHealth;
	public float lastSwordHitTime = 0;
	
	private tk2dSpriteAnimator anim;

	// Use this for initialization
	void Start () {
		health = maxHealth;
		anim = GetComponent<tk2dSpriteAnimator>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void takeDamage(int damage) {
		health -= damage;
		if (health <= 0) {
			// start death animation
			StartCoroutine(waitForAnimationAndDie());
		}
	}
	
	IEnumerator waitForAnimationAndDie() {
		while (anim.Playing) {
			yield return null;
		}
		Destroy(gameObject);
	}
}
