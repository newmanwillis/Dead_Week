using UnityEngine;
using System.Collections;

public class CutsceneUtil : MonoBehaviour {
	public string speakerName;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void printText(string text) {
		if (speakerName != null && speakerName != "") {
			Camera.main.GetComponent<CameraControl>().pauseAndDrawTextMessage(speakerName + ":  " + text);
		} else {
			Camera.main.GetComponent<CameraControl>().pauseAndDrawTextMessage(text);
		}
	}
	
	void switchScene(string sceneName) {
		Application.LoadLevel(sceneName);
	}
}
