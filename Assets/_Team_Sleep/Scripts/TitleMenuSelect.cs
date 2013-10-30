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

	void Update () {
		
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
			else if (Input.GetKeyDown(KeyCode.Return)) {
				
				switch( (TitleButtons)curButton ){
					case TitleButtons.NewGame:
					 	Application.LoadLevel("IntroCutScene");
						break;
					case TitleButtons.Continue:
						NoSaveDataPopUp.SetActive(true);
						subMenu = true;
						break;
					case TitleButtons.Quit:
						Application.Quit();
						break;
				}
			}
		}
		else{
			if (Input.GetKeyDown(KeyCode.Return)) {
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
