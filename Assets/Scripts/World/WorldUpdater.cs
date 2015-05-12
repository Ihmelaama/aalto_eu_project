using UnityEngine;
using System.Collections;

public class WorldUpdater : MonoBehaviour {

//---------------------------------------------------
// VARIABLES

  // utils ---

    private GestureManager gestureManager;

//---------------------------------------------------
// START

	void Awake() {
	
    gestureManager=new GestureManager();

	}
  
//---------------------------------------------------
// EVENTS
	
	void Update() {
  
  // update utils ---
  
    gestureManager.Update();
    
	}
  
//---------------------------------------------------
// PRIVATE SETTERS

  private void updatePlayerNavigation() {
  }  

}
