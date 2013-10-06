using UnityEngine;
using System.Collections;

public class CutsceneUtil : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void printText(string text) {
		Camera.main.GetComponent<CameraControl>().pauseAndDrawTextMessage(text);
	}
	
	void switchScene(string sceneName) {
		Application.LoadLevel(sceneName);
	}
}
