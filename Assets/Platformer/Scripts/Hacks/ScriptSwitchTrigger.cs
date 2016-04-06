using UnityEngine;
using System.Collections;

namespace Platformer {
public class ScriptSwitchTrigger : MonoBehaviour {

//---------------------------------------------------
// VARIABLES

//---------------------------------------------------
// EVENTS

	void Start() {
	
	}
  
//------------

	void Update() {
	
	}
  
//------------

  void OnTriggerEnter2D(Collider2D c) {  
  if(c.transform.parent!=null) toggleScripts(c.transform.parent.gameObject, false);
  } 
  
//------------

  void OnTriggerExit2D(Collider2D c) {
  if(c.transform.parent!=null) toggleScripts(c.transform.parent.gameObject, true);
  }   
  
//---------------------------------------------------
// PRIVATE SETTERS  
  
  private void toggleScripts(GameObject g, bool b) {
  
    if(g!=null) {
    
      Character characterScript=g.GetComponent<Character>();
      if(characterScript!=null) characterScript.enabled=b;
    
    }

  }
  
}
}
