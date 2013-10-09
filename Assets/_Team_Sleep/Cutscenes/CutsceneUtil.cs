using UnityEngine;
using System.Collections;

public class CutsceneUtil : MonoBehaviour {
	public Texture[] speakerFaces;
	
	public string speakerName;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	//void printText(string text) {
	//	printText(text, 0, false);
	//}
	
	void printText(AnimationEvent aniEvent) {
		printText(aniEvent.stringParameter, aniEvent.intParameter, false);
	}
	
	void printText(string text, int faceNum, bool faceToTheLeft) {
		if (speakerName != null && speakerName != "") {
			text = speakerName + ":  " + text;
		}
		if (faceNum == -1) {
			Camera.main.GetComponent<CameraControl>().pauseAndDrawTextMessage(text);
		} else {
			Camera.main.GetComponent<CameraControl>().pauseAndDrawTextMessage(text, speakerFaces[faceNum], faceToTheLeft);
		}
	}
	
	void switchScene(string sceneName) {
		Application.LoadLevel(sceneName);
	}
}
