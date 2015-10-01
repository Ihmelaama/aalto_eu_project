using UnityEngine;
using System.Collections;

public class NPC : Character {

//---------------------------------------------------
// VARIABLES

  // public settings ---

    public float followPlayer=0f;
    public float avoidPlayer=0f;

    public float followOtherNPC=0f;
    public float avoidOtherNPC=0f;

    public float reactionRadius=10f;
    
    public string dialogueFile=null;
    
    public int needsItemId=-1;
    public int needsItemType=-1;

  // private settings ---

    private float minReactionRadius=1.5f;
    private float maxReactionRadius=10f;

  // holders ---
  
    private GameObject player;
    
    [HideInInspector]
    public Dialogue dialogue;
    
    private DialogueManager dialogueManager;

//---------------------------------------------------
// START

  protected override void Awake() {
  
    base.Awake();
    
  // get stuff ---

    player=GameObject.Find("Player");

    if(dialogueFile!=null) {
    dialogue=new Dialogue(dialogueFile);
    }
    
    dialogueManager=GameObject.Find("Scripts").GetComponent<DialogueManager>();

  // set stuff ---

    if(reactionRadius<minReactionRadius) {
    reactionRadius=minReactionRadius;

    } else if(reactionRadius>maxReactionRadius) {
    reactionRadius=maxReactionRadius;
    }

  }

//---------------------------------------------------
// EVENTS

  protected override void FixedUpdate() {
  
    base.FixedUpdate();  
  
  // react to player ---
  
    reactToPlayer();
    
  // react to other characters ---
  
    reactToOtherCharacters();
    
  // test user touch ---
  
    testUserTouch();
    
  }

//---------------------------------------------------
// PUBLIC GETTERS
  
  public Dialogue.DialogueItem getDialogue(int type) {
  
    Dialogue.DialogueItem d=dialogue.getDefaultDialogue();
  
    switch(type) {
    
      case Constants.USER_GIVES_ITEM:

        if(
        (needsItemId>-1 && ItemManager.currentItemId==needsItemId) ||
        (needsItemType>-1 && ItemManager.currentItemType==needsItemType)
        ) {
        
          d=dialogue.getDialogueByName("item_accept");
        
        } else {
        d=dialogue.getDialogueByName("item_decline");
        }

      break;
    
    }
  
  return d;
  }
  
//---------------------------------------------------
// PUBLIC SETTERS

  public void followThis(Transform target, float strength) {
  
    determination=strength;
      
    Vector3 dif=target.position-transform.position; 
    dif=dif.normalized;    
    
    Vector3 newPos=transform.position+dif;
    moveTowards(newPos);

  }
  
//------------
  
  public void avoidThis(Transform target, float strength) {
  
    determination=strength;
  
    Vector3 dif=target.position-transform.position;
    dif=dif.normalized;

    Vector3 newPos=transform.position-dif;
    moveTowards(newPos);

  }  
  
//------------

  public void receiveItem(int itemType) {
  Debug.Log("I was given: "+itemType);
  }  
  
//---------------------------------------------------
// PRIVATE SETTERS

  private void reactToPlayer() {
  
    if(player!=null) {
    
      Vector3 dif=player.transform.position-transform.position;

      if(dif.magnitude<reactionRadius) {
      
        //float sum=followPlayer+avoidPlayer;
        float sum=1f;
        
        float follow=followPlayer/sum;
        float avoid=avoidPlayer/sum;
        float distanceRatio=1f;
            
      // follow ---
              
        if(follow>avoid) {

          if(dif.magnitude<minReactionRadius*1.2f) {
          distanceRatio=0f;
          }
       
          if(dif.magnitude<minReactionRadius) {   
          avoidThis(player.transform, 1f);
          
          } else {
          followThis(player.transform, follow*distanceRatio);
          }
        
      // avoid ---
        
        } else {
        
          if(dif.magnitude<minReactionRadius) {
          distanceRatio+=1f-dif.magnitude/minReactionRadius;
          }
        
          avoidThis(player.transform, avoid*distanceRatio);
        
        }
      
      }
    
    }

  }

//------------

  private void reactToOtherCharacters() {
  }
  
//------------

  private void testUserTouch() {

    if(dialogueManager!=null && GestureManager.wasTouched && GestureManager.testTouch3D(transform)!=Vector3.zero) {
    dialogueManager.showDialogueMenu(transform, 2);
    }

  }

}
