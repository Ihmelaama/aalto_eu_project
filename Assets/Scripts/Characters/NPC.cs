using UnityEngine;
using System.Collections;

public class NPC : Character {

//---------------------------------------------------
// VARIABLES

  // public settings ---

    public float followPlayer=0f;
    public float avoidPlayer=0f;

    public float reactionRadius=10f;
    
    public string dialogueFile=null;
    
    public bool willAcceptAnyItem=true;
    public int needsItemId=-1;
    public int[] willDeclineItems;

  // private settings ---

    private float minReactionRadius=1.5f;
    private float maxReactionRadius=10f;

  // holders ---
  
    [HideInInspector]
    public GameObject player;
    
    [HideInInspector]
    public Player playerScript;

    [HideInInspector]
    public Dialogue dialogue;
    
    private DialogueManager dialogueManager;

//---------------------------------------------------
// START

  protected override void Awake() {
  
    base.Awake();
    
  // get stuff ---

    player=GameObject.Find("Player");
    playerScript=player.GetComponent<Player>();
    
    if(dialogueFile!=null && dialogueFile.Length!=0) {
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
  
//------------
  
  protected override void Start() {
  
    base.Start();

    if(dialogueFile==null || dialogueFile.Length==0) {
    getCharacterDialogue();
    }    
  
  }

//---------------------------------------------------
// EVENTS

  protected override void FixedUpdate() {

    base.FixedUpdate();  

  // react to player ---

    reactToPlayer();

  // test user touch ---
  
    testUserTouch();
    
  }

//---------------------------------------------------
// PUBLIC GETTERS
  
  public Dialogue.DialogueItem getDialogue() {
  Dialogue.DialogueItem d=dialogue.getDefaultDialogue();
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

  public bool receiveItem(Item item) {
  
    Dialogue.DialogueItem d;
  
  // NPC accepts item ---
  
    if(
    (willAcceptAnyItem || needsItemId==item.itemID) && 
    (willDeclineItems.Length==0 || !Helpful.ArrayContainsInt(willDeclineItems, item.itemID))
    ) {
    
      handleAthensItemGiven(item);
     
      int wasMission=MissionManager.checkIfMission(item, MissionManager.ActionType.give);
      
      // give mission success feedback      
      if(wasMission!=0) {
      
        if(wasMission==1) {
        d=dialogue.getDialogueByName("accept_mission_progress");
        } else {
        d=dialogue.getDialogueByName("accept_mission_done");
        }
        
        dialogueManager.setNewDialogue(this, d);
      
      // give other feedback
      } else {
      
        d=dialogue.getDialogueByName("accept_item");
        dialogueManager.setNewDialogue(this, d);
      
      }
       
      if(needsItemId==item.itemID) needsItemId=-1;
      return true;

  // NPC declines item ---
  
    } else {
    
      d=dialogue.getDialogueByName("decline_item");    
      dialogueManager.setNewDialogue(this, d);
    
    }

  return false;
  }  
  
//------------

  public void setCharacterDialogue(string d) {
  dialogueFile=d;
  dialogue=new Dialogue(d);
  }
  
//---------------------------------------------------
// PRIVATE SETTERS

  private void getCharacterDialogue() {

    dialogueFile="Dialogue/"+GameState.currentWorldName+"/"+characterIdString+"_dialogue";
    dialogue=new Dialogue(dialogueFile);

  }
  
//------------

  private void reactToPlayer() {
  
    if(player!=null) {
    
      Vector3 dif=player.transform.position-transform.position;

      if(dif.magnitude<reactionRadius) {
      
        float sum=1f;
        
        float follow=followPlayer/sum;
        float avoid=avoidPlayer/sum;
        float distanceRatio=1f;
        float strength=0f;
            
      // follow ---
              
        if(follow>avoid) {

          if(dif.magnitude<minReactionRadius*1.2f) {
          distanceRatio=0f;
          }
       
          if(dif.magnitude<minReactionRadius) {   
          
            strength=1f;
            avoidThis(player.transform, strength);
            
          } else {
          
            strength=follow*distanceRatio;
            if(playerScript.walkVector.magnitude==0f) strength=0f;          
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

  private void testUserTouch() {
  
    if(dialogueManager!=null && GestureManager.wasTouched && !GestureManager.wasDragged && GestureManager.testTouch3D(transform)!=Vector3.zero) {
    dialogueManager.showDialogueMenu(transform);
    }

  }
  
//---------------------------------------------------
// DISGUSTING SHORTCUTS

  private void handleAthensItemGiven(Item item) {
  
    if(item.itemID==1) {
    playerScript.changeLifeValue(0, AthensSettings.goodItemGivenValue);
    
    } else if(item.itemID==2) {
    playerScript.changeLifeValue(0, AthensSettings.badItemGivenValue);
    }
  
  }

}
