using UnityEngine;
using System.Collections;

namespace Platformer {
public class LevelWinTrigger : MonoBehaviour {

//---------------------------------------------------
// VARIABLES

  public int nextLevel=-1;

//---------------------------------------------------
// EVENTS

  void OnTriggerEnter2D(Collider2D collider) {  
  
    if(collider.transform.parent!=null) {
  
      GameObject g=collider.transform.parent.gameObject;
      if(g.tag=="Player") {
  
        WorldState.allowUserInput=false;
        
        Character character=g.GetComponent<Character>();
        character.stopMovement();
        
        LevelManager.instance.showUI();

      }
    
    }

  } 

}
}
