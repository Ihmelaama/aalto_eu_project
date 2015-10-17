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
// PUBLIC SETTERSS

  public void gotoGameWin() {
  StartCoroutine(gotoGameWin_());
  }
  
  IEnumerator gotoGameWin_() {
  
    yield return new WaitForSeconds(3f);
    Application.LoadLevel("GameWin");
  
  }

}
