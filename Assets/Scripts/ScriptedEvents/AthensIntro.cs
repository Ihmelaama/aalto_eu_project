using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AthensIntro : MonoBehaviour {

//----------------------------------------------------------
// VARIABLES

  private Text text;
  private int phase=1;

//----------------------------------------------------------
// EVENTS

	void Start() {
  
    WorldState.allowUserInput=false;
  
    text=transform.Find("Text").GetComponent<Text>();

    text.text="You wake up to a sound of your parents murmuring in the living room. They sound worried.";
    // play murmur sound here

	}
  
//------------

	void Update() {
  
    if(GestureManager.wasTouched) {
    nextPhase();
    }
  
	}
  
//----------------------------------------------------------
// PRIVATE SETTERS

  private void nextPhase() {
  
    phase++;
    
    switch(phase) {
    
      case 2:
      
        text.text="Something is wrong.";
        // play crying sound
      
      break;
      
      case 3:
      
        text.text="Very wrong.";
        // play door slam
      
      break;
      
      case 4:
      
        text.text=""+
        "In the living room you find a note from your parents.";
      
      break;    
      
      case 5:
      
        text.fontSize=40;
      
        text.text=""+
        "We have decided to go away.\n"+
        "Your bad behaviour has become too much for us to handle.\n"+
        "\n"+
        "We still love you but we can not go on like this.\n"+
        "Come find us when you're ready to change your ways.\n"+
        "\n"+
        "Goodbye.";

      break;        
      
      case 6:
      Destroy(gameObject);
      WorldState.allowUserInput=true;
      break;
    
    }

  }
  
}
