using UnityEngine;
using System.Collections;

public class Level1StartCutscene : MonoBehaviour {

	//public Texture OneSemesterLater;
	//public Texture BlackBox;
	public AudioClip CellPhoneRing;
	public Texture MysteryTalkSprite;
	public Texture HeroTalkSprite;
	public Texture InfoCard;

	//private float StartTime;
	private Transform Black;
	private Transform Text;
	private tk2dSprite TextSprite;
	private Transform Player;

	// Use this for initialization
	void Start () {
		Black = transform.FindChild("Black");
		Text = transform.FindChild("Text");
		TextSprite = Text.GetComponent<tk2dSprite>();
		Player = GameObject.Find("Player").transform;

		//Player.GetComponent<"Player">();
		Player.GetComponent<Player>().enterCutscene();
		Camera.main.GetComponent<CameraControl>().isCutscene = true;

		Black.gameObject.SetActive(true);

		StartCoroutine(FadeIn());

		//StartTime = Time.time;
		//ScreenCenter = new Rect(0, 0, Screen.width, Screen.height);
	}


	IEnumerator FadeIn(){
		//Player.GetComponent<Player>().enterCutscene();

		yield return new WaitForSeconds(1f);

		float lerpTime = 2f;
		float fullTime = Time.time + lerpTime;
		Color colorVector = TextSprite.color;

		while(Time.time < fullTime){
			float curTime = fullTime - Time.time;
			colorVector.a = Mathf.Lerp(0, 1, 1 - (curTime/lerpTime));
			TextSprite.color = colorVector;
			yield return null;
		}
		Black.gameObject.SetActive(false);
		StartCoroutine(FadeOut());
	}

	IEnumerator FadeOut(){
		yield return new WaitForSeconds(1f);
		
		float lerpTime = 2f;
		float fullTime = Time.time + lerpTime;
		Color colorVector = TextSprite.color;
		
		while(Time.time < fullTime){
			float curTime = fullTime - Time.time;
			colorVector.a = Mathf.Lerp(1, 0, 1 - (curTime/lerpTime));
			TextSprite.color = colorVector;
			yield return null;
		}

		Camera.main.GetComponent<CameraControl>().isCutscene = false;

		StartCoroutine(ReturnToPlayerControl());

	}

	IEnumerator ReturnToPlayerControl(){

		yield return new WaitForSeconds(1.5f);
		audio.clip = CellPhoneRing;
		audio.Play();

		yield return new WaitForSeconds(1f);
		string message = "Anonymous:  Hello? Is anyone there? I need help. I'm on the 2nd floor. I'm tra...AAHHHH";
		Camera.main.GetComponent<CameraControl>().pauseAndDrawTextMessage(message, MysteryTalkSprite, true);

		yield return new WaitForSeconds(0.5f);
		message = "Hero:  Hello? HELLO?....Dang it";
		Camera.main.GetComponent<CameraControl>().pauseAndDrawTextMessage(message, HeroTalkSprite, true);

		yield return new WaitForSeconds(1f);
		Camera.main.GetComponent<CameraControl>().pauseAndDrawInfoCard(InfoCard);
		Player.GetComponent<Player>().exitCutscene();
	}

	/*
	void FixedUpdate(){
		print ("Time: " + Time.time);
	}

	void OnGUI(){
		GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), BlackBox);

		if(Time.time < StartTime + 3){

		}
	}*/
}
