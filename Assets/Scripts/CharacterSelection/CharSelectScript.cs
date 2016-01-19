using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


public class CharSelectScript : MonoBehaviour {

  public static int selectedLevelNum=0;
  
  public static CharSelectScript instance;

	//publics
	public GameObject[] buttons;
    public GameObject[] buttons2;
    public GameObject[] buttons3;

    public GameObject Canvas;
	public GameObject Canvas2;
	public GameObject Canvas3;


	//private
	private GameObject button;
  
  private GameObject loadingMessage;



	// Use this for initialization
	void Start () {
  
		createButtons();
    
    instance=this;
    
    loadingMessage=GameObject.Find("MainCanvas/LoadingMessage");
    loadingMessage.SetActive(false);
    
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	

	void createButtons(){
		//create a button
		for(int x = buttons.Length-1; x>=0; x--){
			button = Instantiate (buttons[x], new Vector3 (0, 0), Quaternion.identity) as GameObject;
			button.transform.SetParent(Canvas.transform, false);
		}

		for(int x = buttons2.Length-1; x>=0; x--){
			button = Instantiate (buttons2[x], new Vector3 (0, 0), Quaternion.identity) as GameObject;
			button.transform.SetParent(Canvas2.transform, false);
		}

		for(int x = buttons3.Length-1; x>=0; x--){
			button = Instantiate (buttons3[x], new Vector3 (0, 0), Quaternion.identity) as GameObject;
			button.transform.SetParent(Canvas3.transform, false);
		}

	}
  
//---------------------------------------------------------------
// PUBLIC SETTERS

  public void selectLevel(int num) {
  
    CharSelectScript.selectedLevelNum=num;
    GameState.currentWorld=num;
  
    switch(num) {
    
      case 1:
      GameState.currentWorldName="Prague";
      break;

      case 2:
      GameState.currentWorldName="Athens";
      break;
    
    }

  }
  
//------------
  
  public void loadLevel() {
  loadingMessage.SetActive(true);  
	Application.LoadLevel(CharSelectScript.selectedLevelNum);  
  }
  

}
