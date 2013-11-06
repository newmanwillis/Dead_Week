using UnityEngine;
using System.Collections;

public class ZombieHole : MonoBehaviour {
	
	public float FadeTime = 3f;
	
	tk2dSprite Sprite;
	Color StartColor; 
	
	// Use this for initialization
	void Start () {
		Sprite = GetComponent<tk2dSprite>();
		StartColor = GetComponent<tk2dSprite>().color;

		StartCoroutine(FadeHole(FadeTime));
	}
	
	IEnumerator FadeHole(float fadeTime){
		float fullFadeTime = Time.time + FadeTime;
		Color endColor = Sprite.color;
		endColor.a = 0;	
		
		while(Time.time < fullFadeTime){
			float timeSpent = fullFadeTime - Time.time;
			Sprite.color = Color.Lerp(StartColor, endColor, 1 - (timeSpent/fadeTime));
			yield return null;
		}
		Destroy(gameObject);		
	}
}
