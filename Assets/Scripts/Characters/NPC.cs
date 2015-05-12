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

  // private settings ---

    private float minReactionRadius=1.5f;
    private float maxReactionRadius=10f;

  // holders ---
  
    private GameObject player;

//---------------------------------------------------
// START

  public override void Start_() {

  // get stuff ---

    player=GameObject.Find("Player");

  // set stuff ---

    if(reactionRadius<minReactionRadius) {
    reactionRadius=minReactionRadius;

    } else if(reactionRadius>maxReactionRadius) {
    reactionRadius=maxReactionRadius;
    }

  }

//---------------------------------------------------
// EVENTS

  public override void Update_() {
  
  // react to player ---
  
    reactToPlayer();
    
  // react to other characters ---
  
    reactToOtherCharacters();
    
  // update ---
  
    base.Update_();

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
    

}
