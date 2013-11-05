using UnityEngine;
using System.Collections;

public class RandomizeColor : MonoBehaviour {
	
	public float time = 5f;
	
	private float trueTime;
	private float timeMod;
	private tk2dSprite Sprite;
	private Color[] Colors;
	
	void Start () {
		Sprite = GetComponent<tk2dSprite>();
		
		timeMod = time;
		trueTime = 1f/timeMod;
		//Color orange = new Color(255, 80, 0, 255);
		Colors = new Color[] {Color.cyan, Color.green, Color.magenta, Color.yellow, Color.blue, Color.white, Color.grey, Color.red, Color.black};
		StartCoroutine(ChangeColor(trueTime));
	}
	
	/*void FixedUpdate () {
		Sprite.color = ChooseColor();
	}*/
	
	IEnumerator ChangeColor(float colorTime){
		Color startColor = Sprite.color;
		Color endColor = ChooseColor();
		float fullTime = Time.time + colorTime;
		while(Time.time < fullTime){
			float timeLeft = (colorTime - (fullTime - Time.time)) * timeMod;
			Sprite.color = Color.Lerp(startColor, endColor, timeLeft);
			yield return null;
		}
		StartCoroutine(ChangeColor(trueTime));
	}
	
	public Color ChooseColor(){
		Color randColor = Sprite.color;
		while(randColor.Equals(Sprite.color)){
			int rand = Random.Range(0, Colors.Length);
			randColor = Colors[rand];
		}
		return randColor;
	}
}