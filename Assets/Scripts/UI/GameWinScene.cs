using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameWinScene : MonoBehaviour {

//-----------------------------------------------------------------
// VARIABLES


//-----------------------------------------------------------------
// EVENTS

	void Start() {
  
    if(GameState.score!=0) {
    GameObject.Find("MainCanvas/Image/Text").GetComponent<Text>().text="You win at life!\nYour score: "+GameState.score;
    }
	
	}

//------------

	void Update() {
	
	}
  
}
