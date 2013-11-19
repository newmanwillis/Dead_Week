using UnityEngine;
using System.Collections;

public class Destructible : MonoBehaviour {
	private tk2dSpriteAnimator animator;
	
	private bool destroyed = false;
	
	private AudioSource destructionSound;
	private AudioSource disintigrationSound;
	
	private int numHits = 0;
	public int maxHits;
	
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
		AudioSource[] audio = GetComponents<AudioSource>();
		if (audio != null && audio.Length > 0) {
			destructionSound = audio[0];//GetComponent<AudioSource>();
			disintigrationSound = audio.Length > 1 ? audio[1] : null;
		}
		if (destructionSound != null) {
			destructionSound.loop = false;
		}
		calculateCutoffs();
	}
	
	public void smash() {
		if (numHits < maxHits && (!animator.Playing)) {
			string clipname = "smashed" + (++numHits);
			if (animator.GetClipByName(clipname) != null) {
				animator.Play(clipname);
			} else {
				Destroy(GetComponent<tk2dSprite>());
			}
			if (destructionSound != null) {
				destructionSound.Play ();
			}
			if (numHits == maxHits) {
				StartCoroutine(waitForAnimationAndDie());
			}
		}
	}
	
	public void disintigrate() {
		if (numHits < maxHits && (!animator.Playing)) {
			string clipname = "disintigrated" + (++numHits);
			if (animator.GetClipByName(clipname) == null) {
				--numHits;
				smash();
			} else {
				wasDisintigrated = true;
				animator.Play(clipname);
				if (disintigrationSound != null) {
					disintigrationSound.Play ();
				}
				if (numHits == maxHits) {
					StartCoroutine(waitForAnimationAndDie());
				}
			}
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
		Destroy(transform.FindChild("ColliderForBullet").gameObject);
		if (!wasDisintigrated && animator.GetClipByName("smashed_flashing") != null) {
			animator.Play("smashed_flashing");
		}
		
		Generator gen = GetComponent<Generator>();
		if (gen != null) {
			gen.shutdown();
		}
	}
}
