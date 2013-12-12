using UnityEngine;
using System.Collections;

public class TitleMenuSelect : MonoBehaviour {
	
	public enum TitleButtons {NewGame = 0, Continue, Quit}
	private int curButton = 0;
	private bool subMenu = false;
	
	private tk2dSprite NewGameButton;
	private tk2dSprite ContinueButton;
	private tk2dSprite QuitGameButton;
	private GameObject NoSaveDataPopUp;
	
	void Start () {
		NewGameButton = transform.FindChild("NewGameButton").GetComponent<tk2dSprite>();
		ContinueButton = transform.FindChild("ContinueButton").GetComponent<tk2dSprite>();
		QuitGameButton = transform.FindChild("QuitGameButton").GetComponent<tk2dSprite>();
		NoSaveDataPopUp = transform.FindChild("NoSaveDataFound").gameObject;
		
		NewGameButton.color = Color.red;
		
	}

	string latestLevel() {
		if (PlayerPrefs.GetString("Level4.1") == "Unlocked") {
			return "Level4.1";
		}
		if (PlayerPrefs.GetString("Level3.1") == "Unlocked") {
			return "Level3.1";
		}
		if (PlayerPrefs.GetString("Level2-Generator") == "Unlocked") {
			return "Level2-Generator";
		}
		if (PlayerPrefs.GetString("Level1.1") == "Unlocked") {
			return "Level1.1";
		}
		return null;
	}

	void Update () {
		Debug.Log ("LOGG");
		//Debug.Log("Level1.1: " + (PlayerPrefs.GetString("Level1.1") == null? "null" : "notnull"));
		Debug.Log("Level1.1: " + PlayerPrefs.GetString("Level1.1"));
		Debug.Log("Level2-Generator: " + PlayerPrefs.GetString("Level2-Generator"));
		Debug.Log("Level3.1: " + PlayerPrefs.GetString("Level3.1"));
		Debug.Log("Level4.1: " + PlayerPrefs.GetString("Level4.1"));

		if (Input.GetKeyDown(KeyCode.A)) {
			PlayerPrefs.DeleteAll();
			PlayerPrefs.Save();
		}

		RaycastHit hitInfo;
		bool mouseOnButton = false;
		if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo)) {
			mouseOnButton = true;
			MakeButtonWhite();
			switch (hitInfo.transform.name) {
			case "ContinueButton":
				curButton = 1;
				break;
			case "NewGameButton":
				curButton = 0;
				break;
			case "QuitGameButton":
				curButton = 2;
				break;
			}
			CheckCurrentButton();
			MakeButtonRed();
		}

		//if (Input.GetKeyDown(KeyCode.A)) {
		//}
		
		if(!subMenu){
			if (Input.GetKeyDown(KeyCode.UpArrow)) {
				MakeButtonWhite();
				curButton--;
				CheckCurrentButton();
				MakeButtonRed();
			}
			else if (Input.GetKeyDown(KeyCode.DownArrow)) {
				MakeButtonWhite();			
				curButton++;
				CheckCurrentButton();
				MakeButtonRed();			
			}
			else if (Input.GetKeyDown(KeyCode.Return) || (mouseOnButton && Input.GetMouseButtonDown(0))) {
				
				switch( (TitleButtons)curButton ){
					case TitleButtons.NewGame:
					 	Application.LoadLevel("IntroCutScene");
						break;
					case TitleButtons.Continue:
						string levelName = latestLevel();
						if (levelName == null) {
							NoSaveDataPopUp.SetActive(true);
							subMenu = true;
						} else {
							Application.LoadLevel(levelName);
						}
						break;
					case TitleButtons.Quit:
						Application.Quit();
						break;
				}
			}
		}
		else{
			if (Input.GetKeyDown(KeyCode.Return) || Input.GetMouseButtonDown(0)) {
				NoSaveDataPopUp.SetActive(false);
				subMenu = false;
			}
		}
	}
	
	void CheckCurrentButton(){
		if(curButton < 0){
			curButton = 2;
		}
		else if(curButton > 2){
			curButton = 0;	
		}
	}
	
	void MakeButtonWhite(){
		switch( (TitleButtons)curButton ){
			case TitleButtons.NewGame:				
				NewGameButton.color = Color.white;
				break;
			case TitleButtons.Continue:				
				ContinueButton.color = Color.white;
				break;
			case TitleButtons.Quit:				
				QuitGameButton.color = Color.white;
				break;			
		}
	}
	
	void MakeButtonRed(){
		switch( (TitleButtons)curButton ){
			case TitleButtons.NewGame:				
				NewGameButton.color = Color.red;
				break;
			case TitleButtons.Continue:				
				ContinueButton.color = Color.red;
				break;
			case TitleButtons.Quit:				
				QuitGameButton.color = Color.red;
				break;			
		}		
	}
}
