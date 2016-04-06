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

    string e="";
    
    // temporary (yeah right) hack for Athens sounds
    if(GameState.currentWorld==2) {
    e="A/Char/"+characterId;
    } else {
    e="Char/Hello/"+characterId;
    }
    
    Debug.Log(e);

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
  
//------------

  public void playVesalaIntroMusic() {
  }
  
//------------  

  public void playVesalaMusic() {

    Fabric.EventManager.Instance.PostEvent("V/Music/Main Stop");
    Fabric.EventManager.Instance.PostEvent("V/Music/Main");
  
  }  
  
//------------

  public void playVesalaPickupSound(bool positive) {
  
    if(positive) {
    
      if(WorldState.randomSoundsOn) {
      Fabric.EventManager.Instance.PostEvent("V/Sfx/PickUpPos Random");
      
      } else {
      Debug.Log("pos");      
      Fabric.EventManager.Instance.PostEvent("V/Sfx/PickUpPos");
      }
    
    } else {
  
      if(WorldState.randomSoundsOn) {
      Fabric.EventManager.Instance.PostEvent("V/Sfx/PickUpNeg Random");
      
      } else {
      Debug.Log("neg");
      Fabric.EventManager.Instance.PostEvent("V/Sfx/PickUpNeg");
      }    
  
    }

  }  
  
//------------
  
  public void playVesalaPowerUpSound() {
  
    if(WorldState.randomSoundsOn) {
    Fabric.EventManager.Instance.PostEvent("V/Sfx/PowerUp Random");
    
    } else {
    Fabric.EventManager.Instance.PostEvent("V/Sfx/PowerUp");
    }
  
  }
  
//------------

  public void playVesalaUISound() {
  
    if(WorldState.randomSoundsOn) {
    Fabric.EventManager.Instance.PostEvent("V/Sfx/UI Random");
    
    } else {
    Fabric.EventManager.Instance.PostEvent("V/Sfx/UI");
    }
    
  }
  
//------------

  public void playVesalaJumpSound() {
  
    if(WorldState.randomSoundsOn) {  
    Fabric.EventManager.Instance.PostEvent("V/Sfx/Jump Random");
    
    } else {
    Fabric.EventManager.Instance.PostEvent("V/Sfx/Jump");
    }

  }  
  
//------------

  public void playVesalaFootStepSound() {
  
    if(WorldState.randomSoundsOn) {  
    Fabric.EventManager.Instance.PostEvent("V/Sfx/FS Random");
    
    } else {
    Fabric.EventManager.Instance.PostEvent("V/Sfx/FS");
    }

  }    
  
//------------

  public void playVesalaLevelEndSound(bool win) {
  
    if(win) {  
    Fabric.EventManager.Instance.PostEvent("V/Sfx/Winning Sound");
    
    } else {
    Fabric.EventManager.Instance.PostEvent("V/Sfx/Game Over");
    }  
  
  }

}
