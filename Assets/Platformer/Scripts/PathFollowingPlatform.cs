using UnityEngine;
using System.Collections;

namespace Platformer {
public class PathFollowingPlatform : Platform {

  public Vector2[] positions;
  public float animationSpeed=1f;
  public bool triggeredByPlayer=false;
  
  private Vector2 originalPosition;
  private float originalZ=0f;
  
  private Vector2 node1;
  private Vector2 node2;
  private Vector2 movementRange;
  private float positionBetweenNodes=0f;
  private int currentDestinationNum=0;
  private int currentDirection=1;

  private Vector2 velocity=Vector2.zero;
  private Vector2 prevPos=Vector2.zero;
  
  private bool isTriggered=true;
  
  private Coroutine untriggerCoroutine=null;
  
//----------------------------------------------
	
  public override void Start() {
	
    originalPosition=((Vector2) transform.position)+node1;
    originalZ=transform.position.z;
    base.Start();
    
    if(triggeredByPlayer) {
    isTriggered=false;
    }
    
    setNextDestination();

	}

//----------------------------------------------
  
  public override void Update() {
    
    base.Update();
    
    if(Application.isPlaying && isTriggered) {    
    
      positionBetweenNodes+=animationSpeed;
      Vector2 pos=originalPosition+node1+movementRange*positionBetweenNodes;
      
      transform.position=pos;
      
      if(positionBetweenNodes>1f) {
      setNextDestination();
      }
      
      velocity=pos-prevPos;
      prevPos=pos;      
      
    }

  }

//------------

  public override void OnCollisionStay2D(Collision2D collision) {
  
    if(collision.collider.transform.parent!=null) {
    
      string tag=collision.collider.transform.parent.tag;

      if(triggeredByPlayer && tag=="Player") {
      
        isTriggered=true;
        
        if(untriggerCoroutine!=null) {
        StopCoroutine(untriggerCoroutine);
        untriggerCoroutine=null;
        }
        
      }
  
      if(tag!="Platform" && isTriggered) {
  
        GameObject g=collision.collider.gameObject;
        if(g.tag=="Untagged") g=collision.collider.transform.parent.gameObject;
        g.transform.SetParent(transform);

      }    
 
    }

  }
  
//------------

  void OnCollisionExit2D(Collision2D collision) {
  
    if(collision.collider.transform.parent!=null) {
    
      string tag=collision.collider.transform.parent.tag;

      if(triggeredByPlayer && tag=="Player") {
      
        isTriggered=false;
        
        if(untriggerCoroutine!=null) {
        StopCoroutine(untriggerCoroutine);
        untriggerCoroutine=null;
        }      

        untriggerCoroutine=StartCoroutine(untrigger());
        
      }
  
      if(tag!="Platform" && isTriggered) {
  
        GameObject g=collision.collider.gameObject;
        if(g.tag=="Untagged") g=collision.collider.transform.parent.gameObject;
        g.transform.SetParent(null);

      }    
 
    }  
  
  }  
  
  IEnumerator untrigger() {
  
    yield return new WaitForSeconds(1f);
    isTriggered=false;
  
  }
  
//------------

  private void setNextDestination() {
  
    positionBetweenNodes=0f;
    
    if(currentDestinationNum==0) {
    
      node1=Vector2.zero;
      node2=positions[0];
      currentDirection=1;
 
    } else {
     
      node1=positions[currentDestinationNum-currentDirection];
      node2=positions[currentDestinationNum];  
      
      if(currentDestinationNum>=positions.Length-1) {
      currentDirection*=-1;
      }  
    
    }

    currentDestinationNum+=currentDirection;
    movementRange=node2-node1;     

  }  

}
}
