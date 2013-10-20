using UnityEngine;
using System.Collections;

public class Destructible : MonoBehaviour {
	private tk2dSpriteAnimator animator;
	
	private bool destroyed = false;
	
	private AudioSource destructionSound;
	
	public int[] dropChances;
	public Transform[] drops;
	
	private float[] dropCutoffs;
	/*public int healthDropChance;
	public Transform healthPickup;
	public int energyDropChance;
	public Transform energyPickup;*/
	
	private bool wasDisintigrated = false;
	void Start () {
		animator = GetComponent<tk2dSpriteAnimator>();
		destructionSound = GetComponent<AudioSource>();
		destructionSound.loop = false;
		calculateCutoffs();
	}
	
	public void smash() {
		if (!destroyed) {
			if (animator.GetClipByName("smashed") != null) {
				animator.Play("smashed");
			} else {
				Destroy(GetComponent<tk2dSprite>());
			}
			destroyed = true;
			destructionSound.Play ();
			StartCoroutine(waitForAnimationAndDie());
		}
	}
	
	public void disintigrate() {
		if (animator.GetClipByName("Disintigrated") == null) {
			smash();
		} else if (!destroyed) {
			wasDisintigrated = true;
			animator.Play("Disintigrated");
			destroyed = true;
			StartCoroutine(waitForAnimationAndDie());
		}
	}
	
	void calculateCutoffs() {
		dropCutoffs = new float[drops.Length];
		float totalChance = 0;
		foreach (int chance in dropChances) {
			totalChance += chance;
		}
		
		float lowerBound = 0;
		for (int i = 0; i < drops.Length; i++) {
			dropCutoffs[i] = lowerBound + dropChances[i]/totalChance;
			lowerBound = dropCutoffs[i];
		}
	}
	
	void spawnPickups() {
		float roll = (float)Random.Range(0, 100) / 100;
		int index = 0;
		while (roll > dropCutoffs[index]) {
			index++;
		}
		
		if (drops[index] != null) {
			Instantiate(drops[index], transform.position, Quaternion.identity);
		} // else, drop nothing
	}
	
	IEnumerator waitForAnimationAndDie() {
		while (animator.Playing) {
			yield return null;
		}
		
		spawnPickups();
		Destroy(GetComponent<BoxCollider>());
		if (!wasDisintigrated && animator.GetClipByName("smashed_flashing") != null) {
			animator.Play("smashed_flashing");
		}
	}
}
