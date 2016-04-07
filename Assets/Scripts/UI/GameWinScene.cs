using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameWinScene : MonoBehaviour {

//-----------------------------------------------------------------
// VARIABLES


//-----------------------------------------------------------------
// EVENTS

	void Start() {
  
    if(GameState.score!=0 && GameState.time!=null) {
    GameObject.Find("MainCanvas/Image/Text").GetComponent<Text>().text="You win at life!\nYour score: "+GameState.score+"\nYour time: "+GameState.time;
    }
	
	}

//------------

	void Update() {
	
	}
  
}
