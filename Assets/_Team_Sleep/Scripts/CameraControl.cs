using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {
	GameObject player;
	public Texture sprite;
	
	// Use this for initialization
	void Start () {
		camera.orthographic = true;
		camera.orthographicSize = Screen.height/2;
		
		player = GameObject.Find("Player");
	}
	
	// Update is called once per frame
	void Update () {
	}
	void OnGUI() {
		
		float xPos= player.transform.position.x + Screen.width/2 - sprite.width/2;
		float yPos= -player.transform.position.y + Screen.height/2 - sprite.height/2;
		Debug.Log("here"+sprite.height+" "+xPos);
		Rect r = new Rect(xPos, yPos, sprite.width, sprite.height);
		Debug.Log("here"+sprite.height+" "+xPos);
		GUI.DrawTexture(r, sprite, ScaleMode.ScaleToFit, true, 10.0F);
	}
}
