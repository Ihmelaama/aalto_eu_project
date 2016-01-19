using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour {

  public enum UISound {
  Button,
  Button1,
  Button2,
  Button3,
  Button4,
  Button5,
  Button6,
  Button7,
  ButtonError
  };
  
  public enum GameSound {
  PickUpItem,
  GiveItem,
  Laugh
  }

  public static SoundManager instance;

//------------

  void Awake() {
  instance=this;
  }
  
//------------

  public static void playUISound(UISound type) {
  
    switch(type) {
    
      case UISound.Button1:
      Fabric.EventManager.Instance.PostEvent("UI/Button1");      
      break;
      
      case UISound.Button2:
      Fabric.EventManager.Instance.PostEvent("UI/Button2");      
      break;     
      
      case UISound.Button3:
      Fabric.EventManager.Instance.PostEvent("UI/Button3");      
      break;     
      
      case UISound.Button4:
      Fabric.EventManager.Instance.PostEvent("UI/Button4");      
      break;                

      case UISound.Button5:
      Fabric.EventManager.Instance.PostEvent("UI/Button5");      
      break;
      
      case UISound.Button6:
      Fabric.EventManager.Instance.PostEvent("UI/Button6");      
      break;    
      
      case UISound.Button7:
      Fabric.EventManager.Instance.PostEvent("UI/Button7");      
      break;              
      
      case UISound.ButtonError:
      Fabric.EventManager.Instance.PostEvent("UI/Error");      
      break;
      
      default:
      SoundManager.instance.playUIDefaultSound();
      break;  

    }  
          
  }

  public void playUIDefaultSound() {
  Fabric.EventManager.Instance.PostEvent("UI/Button");    
  } 
  
  public void playUIErrorSound() {
  Fabric.EventManager.Instance.PostEvent("UI/Error");    
  }       
  
//------------

  public static void playGameSound(GameSound type) {  
  
    switch(type) {
    
      case GameSound.PickUpItem:
      Fabric.EventManager.Instance.PostEvent("Game/Item");   
      break;
      
      case GameSound.GiveItem:
      Fabric.EventManager.Instance.PostEvent("Game/Give");   
      break;     
      
      case GameSound.Laugh:
      Fabric.EventManager.Instance.PostEvent("Game/Laugh");   
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

  public void PlayMusic_() {
  
    if(GameState.currentWorld==1) {
    Fabric.EventManager.Instance.PostEvent("Music/Demo");
    }
  
  }

  public static void PlayMusic() {

    if(GameState.currentWorld==1) {
    Fabric.EventManager.Instance.PostEvent("Music/Demo");
    }
  
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
