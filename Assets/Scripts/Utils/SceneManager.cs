using UnityEngine;
using System.Collections;

public class SceneManager : MonoBehaviour {

//-------------------------------------------------------
// VARIABLES

  public static SceneManager instance;
  
//-------------------------------------------------------
// EVENTS
  
  void Awake() {
  
    instance=this;
  
  }
  
//-------------------------------------------------------
// PUBLIC SETTERS

  public void gotoGameWin() {
  StartCoroutine(gotoGameWin_());
  }
  
  IEnumerator gotoGameWin_() {
  
    yield return new WaitForSeconds(4f);
    Application.LoadLevel("GameWin");
  
  }
  
//------------

  public void gotoIntro() {
  Reset();
  SoundManager.playUISound(SoundManager.UISound.Button6);
  Application.LoadLevel("Intro");  
  }
  
//------------

  public void Reset() {
  
  // reset player ---
  
    if(GameObject.Find("Player")!=null) {
    Player playerScript=GameObject.Find("Player").GetComponent<Player>();
    playerScript.clearDestination();
    }
    
  // reset NPCs ---

    if(GameObject.Find("NPCHolder")!=null) {

      NPC[] NPCs=GameObject.Find("NPCHolder").GetComponentsInChildren<NPC>();
      
      for(int i=0; i<NPCs.Length; i++) {
      NPCs[i].clearDestination();
      }
    
    }
    
  //---
  
    GestureManager.clearTouch();

  //---
  
      WorldState.allowUserInput=false;

  }

}
