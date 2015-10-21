using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour {

  public static SoundManager instance;

  void Awake() {
  instance=this;
  }
  
//------------

  public void playUISound(int type) {
  
    switch(type) {
    
      default:
      Fabric.EventManager.Instance.PostEvent("UI/Button");
      break;
    
    }
        
  }
  
//------------
  
  public static void PlayCharacterHello(string characterId) {
  SoundManager.PlayCharacterHello(characterId, 0f);   
  }
  
  public static void PlayCharacterHello(string characterId, float delay) {         

    string e="Char/Hello/"+characterId;

    if(delay==0f) {
    Fabric.EventManager.Instance.PostEvent(e);  

    } else {
    SoundManager.instance.PostEventWithDelay(e, delay);
    }

  }
  
//------------
  
  public static void PlayCharacterYes(string characterId) {         

    Fabric.EventManager.Instance.PostEvent("Char/Yes/"+characterId);  

  }
  
//------------
  
  public static void PlayCharacterNo(string characterId) {         

    Fabric.EventManager.Instance.PostEvent("Char/No/"+characterId);  

  }
  
//------------  
  
  public void PostEventWithDelay(string eventName, float delay) {
  StartCoroutine(PostEventWithDelay_(eventName, delay));
  }
  
  IEnumerator PostEventWithDelay_(string eventName, float delay) {
  
    yield return new WaitForSeconds(delay);
    Fabric.EventManager.Instance.PostEvent(eventName);  
  
  }    

}
