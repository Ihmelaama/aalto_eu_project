using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EventManager : MonoBehaviour {

//----------------------------------------------------------
// VARIABLES

  public static EventManager instance;
  private Dictionary<string, List<Component>> listeners=new Dictionary<string, List<Component>>();
 
//----------------------------------------------------------
// EVENTS
  
  void Awake() {
  
    if(EventManager.instance==null) {
    instance=this;
    }
    
  }
  
//----------------------------------------------------------
// PUBLIC SETTERS  
  
  public void addListener(Component listener, string eventName) {
  
    if(!listeners.ContainsKey(eventName)) {
    listeners.Add(eventName, new List<Component>());
    }
    
    listeners[eventName].Add(listener);
  
  }
  
//------------
  
  public void postEvent(Component sender, string eventName) {
  
    if(listeners.ContainsKey(eventName)) {
    
      foreach(Component listener in listeners[eventName]) {
      listener.SendMessage(eventName, sender, SendMessageOptions.DontRequireReceiver);
      }
    
    }
  
  }
  

}
