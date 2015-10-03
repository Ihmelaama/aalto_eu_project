using UnityEngine;
using System.Collections;
using DG.Tweening;

public class WorldUpdater : MonoBehaviour {

//---------------------------------------------------
// VARIABLES

  // utils ---

    private GestureManager gestureManager;
    
  // state ---
  
    public static float wind=0;

//---------------------------------------------------
// START

	void Awake() {
	
    DOTween.Init();
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
