using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour {

  public static SoundManager instance;

  void Awake() {
  instance=this;
  }
  
//------------

/*
UI/Button Kaikki muut
UI/Button1 dialogi
UI/Button2 Reppu auki
UI/Button3 Reppu kiinni
UI/Button4 Missio auki
UI/Button5 Missio kiinni
*/

  public void playUISound(int type) {
  
    switch(type) {
    
      case 1:
      Fabric.EventManager.Instance.PostEvent("UI/Button1");      
      break;
      
      case 2:
      Fabric.EventManager.Instance.PostEvent("UI/Button2");      
      break;     
      
      case 3:
      Fabric.EventManager.Instance.PostEvent("UI/Button3");      
      break;     
      
      case 4:
      Fabric.EventManager.Instance.PostEvent("UI/Button4");      
      break;                

      case 5:
      Fabric.EventManager.Instance.PostEvent("UI/Button5");      
      break;
    
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
