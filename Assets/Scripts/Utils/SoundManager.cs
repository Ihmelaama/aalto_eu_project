using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour {

  public void playUISound(int type) {
  
    switch(type) {
    
      default:
      Fabric.EventManager.Instance.PostEvent("UI/Button");
      break;
    
    }
        
  }

}
