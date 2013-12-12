using UnityEngine;
using System.Collections;

public class TitleMenuSelect : MonoBehaviour {
	private string[] levelNames = { "Level1.1", "Level2-Generator", "Level3.1", "Level4.1" };
	private string[] levelSelectImageNames = { "levelselecttunnels", "levelselectacademia1", "levelselectacademia2", "levelselectstadium" };
	private string[] mainMenuImageNames = { "titlenewgame", "titlelevelselect", "titlequitgame" };
	private string[] mainButtonNames = { "NewGameButton", "LevelSelectButton", "QuitButton" };
	private string[] levelSelectButtonNames = { "L1Button", "L2Button", "L3Button", "L4Button" };
	private int curButton = 0;
	private bool subMenu = false;

	private tk2dSprite sprite;
	//private tk2dSprite NewGameButton;
	//private tk2dSprite ContinueButton;
	//private tk2dSprite QuitGameButton;
	//private GameObject NoSaveDataPopUp;
	
	void Start () {
		//NewGameButton = transform.FindChild("NewGameButton").GetComponent<tk2dSprite>();
		//ContinueButton = transform.FindChild("ContinueButton").GetComponent<tk2dSprite>();
		//QuitGameButton = transform.FindChild("QuitGameButton").GetComponent<tk2dSprite>();
		//NoSaveDataPopUp = transform.FindChild("NoSaveDataFound").gameObject;
		
		//NewGameButton.color = Color.red;

		sprite = GetComponent<tk2dSprite>();
		
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
		updateSprite();
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
			//MakeButtonWhite();
			switch (hitInfo.transform.name) {
			case "NewGameButton":
				curButton = 0;
				break;
			case "LevelSelectButton":
				curButton = 1;
				break;
			case "QuitButton":
				curButton = 2;
				break;
			}
			for (int i = 0; i < 4; i++) {
				if (hitInfo.transform.name == levelSelectButtonNames[i]) {
					curButton = i;
				}
			}
			//CheckCurrentButton();
			//MakeButtonRed();
		}

		//if (Input.GetKeyDown(KeyCode.A)) {
		//}
		if (Input.GetKeyDown(KeyCode.UpArrow)) {
			curButton--;
		}
		else if (Input.GetKeyDown(KeyCode.DownArrow)) {
			curButton++;
		}

		fixIndex();

		if(!subMenu){
			if (Input.GetKeyDown(KeyCode.Return) || (mouseOnButton && Input.GetMouseButtonDown(0))) {
				
				switch( curButton ){
					case 0:
					 	Application.LoadLevel("IntroCutScene");
						break;
					case 1:
						setSubMenu(true);
						break;
					case 2:
						Application.Quit();
						break;
				}
			}
		}
		else{
			if (Input.GetKeyDown(KeyCode.Backspace)) {
				//NoSaveDataPopUp.SetActive(false);
				setSubMenu(false);
			}
			if (Input.GetKeyDown(KeyCode.Return) || (mouseOnButton && Input.GetMouseButtonDown(0))) {
				Application.LoadLevel(levelNames[curButton]);
			}
		}
	}

	void setSubMenu(bool newVal) {
		curButton = 0;
		subMenu = newVal;
		if (subMenu) {
			GameObject.Find(mainButtonNames[0]).SetActive(false);
			GameObject.Find(mainButtonNames[1]).SetActive(false);
			GameObject.Find(mainButtonNames[2]).SetActive(false);
			GameObject.Find(levelSelectButtonNames[0]).SetActive(true);
			GameObject.Find(levelSelectButtonNames[1]).SetActive(true);
			GameObject.Find(levelSelectButtonNames[2]).SetActive(true);
			GameObject.Find(levelSelectButtonNames[3]).SetActive(true);
		} else {
			GameObject.Find(mainButtonNames[0]).SetActive(true);
			GameObject.Find(mainButtonNames[1]).SetActive(true);
			GameObject.Find(mainButtonNames[2]).SetActive(true);
			GameObject.Find(levelSelectButtonNames[0]).SetActive(false);
			GameObject.Find(levelSelectButtonNames[1]).SetActive(false);
			GameObject.Find(levelSelectButtonNames[2]).SetActive(false);
			GameObject.Find(levelSelectButtonNames[3]).SetActive(false);
		}
	}

	void fixIndex() {
		if (subMenu) {
			if (curButton < 0) {
				curButton = 3;
			} else if (curButton > 3) {
				curButton = 0;
			}
		} else {
			if (curButton < 0) {
				curButton = 2;
			} else if (curButton > 2) {
				curButton = 0;
			}
		}
	}

	void updateSprite() {
		if (subMenu) {
			sprite.SetSprite(levelSelectImageNames[curButton]);
		} else {
			sprite.SetSprite(mainMenuImageNames[curButton]);
		}
	}
	
	/*void CheckCurrentButton(){
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
	}*/
}
