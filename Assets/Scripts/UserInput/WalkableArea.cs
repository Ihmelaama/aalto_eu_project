using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class WalkableArea : MonoBehaviour {

//---------------------------------------------------
// VARIABLES

  private List<Character> movableCharacters=new List<Character>();

//---------------------------------------------------
// START

	void Start() {
  
    Character c=GameObject.Find("Player").GetComponent<Character>();
    movableCharacters.Add(c);

	}

//---------------------------------------------------
// EVENTS
	
	void Update() {

    if(GestureManager.isTouched && EventSystem.current.currentSelectedGameObject==null) {
    
      Vector3 v=GestureManager.testTouch3D(transform);
      
      if(v!=Vector3.zero) {
      
        foreach(Character c in movableCharacters) {
        c.moveTowards(v);
        }
      
      }    
    
    }
  
  //---  
  
    if(GestureManager.wasTouched && EventSystem.current.currentSelectedGameObject==null) {
  
      Vector3 v=GestureManager.testTouch3D(transform);
      
      if(v!=Vector3.zero) {
      
        foreach(Character c in movableCharacters) {
        c.addDestination(v, true);
        }
      
      }
    
    }

	}
  
}
