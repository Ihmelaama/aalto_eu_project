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
  
    if(WorldState.allowUserInput && EventSystem.current.currentSelectedGameObject==null) {

      if(GestureManager.isTouched) {
      
        Vector3 v=GestureManager.testTouch3D(transform, new string[] {"User Input", "NPC"});

        if(v!=Vector3.zero) {
        
          foreach(Character c in movableCharacters) {
          c.moveTowards(v);
          }
        
        }    
      
      }
    
    //---  
    
      if(GestureManager.wasTouched) {
    
        Vector3 v=GestureManager.testTouch3D(transform, "User Input");
        
        if(v!=Vector3.zero) {
        
          foreach(Character c in movableCharacters) {
          c.addDestination(v, true);
          }
        
        }
      
      }
    
    }

	}
  
}
