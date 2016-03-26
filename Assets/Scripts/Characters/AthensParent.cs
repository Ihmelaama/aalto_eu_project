using UnityEngine;
using System.Collections;

public class AthensParent : NPC {

//------------------------------------------------------
// VARIABLES
  
  // settings ---
  
    public GameObject otherParent;
    public Sprite[] altSprites;

    private float minDistanceApart=1f;
    
//------------------------------------------------------
// EVENTS

	protected override void Start() {
  
    base.Start();
    
    minDistanceApart=(otherParent.transform.position-transform.position).magnitude;
    
    minDistanceFromDestination=1.75f;
    stopDistanceFromDestination=1.25f;    
    
    if(altSprites.Length>0) {
    int rand=Random.Range(0, altSprites.Length);
    characterSprite=altSprites[rand];
    setCharacterSprite();
    }

	}
  
//------------
	
	void Update() {

    Vector3 dif=otherParent.transform.position-transform.position;

    if(dif.magnitude>minDistanceApart+1f) {
    moveTowards(otherParent.transform.position);
    }
    
    if(playerScript.lifeValues[0]>0.9f) {
    
      if(dialogueFile!="Dialogue/Athens/HappyParent") {
      setCharacterDialogue("Dialogue/HappyParent");
      }
    
      avoidPlayer=0f;
      followPlayer=1f;
      
      dif=transform.position-player.transform.position;
      if(dif.magnitude>minDistanceApart+1f) {
      moveTowards(player.transform.position);
      }
    
    }
    
	}
  
}
