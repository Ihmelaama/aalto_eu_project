using UnityEngine;
using System.Collections;

namespace Platformer {
public class MovingPlatform : Platform {

  public Vector2 position1;
  public Vector2 position2;
  public float animationSpeed=1f;
  public bool triggeredByPlayer=false;
  
  private Vector2 originalPosition;
  private float originalZ=0f;
  private Vector2 movementRange;
  private float position=0f;
  private float deg=-90f;
  
  private Vector2 velocity=Vector2.zero;
  private Vector2 prevPos=Vector2.zero;
  
  private bool isTriggered=true;
  
  private Coroutine untriggerCoroutine=null;
  
//----------------------------------------------
	
  public override void Start() {
	
    originalPosition=((Vector2) transform.position)+position1;
    movementRange=position2-position1;
    originalZ=transform.position.z;
    base.Start();
    
    if(triggeredByPlayer) {
    isTriggered=false;
    }

	}

//----------------------------------------------
  
  public override void Update() {
  
    base.Update();
    
    Vector2 pos=originalPosition+movementRange*position;
    
  //---
    
    if(Application.isPlaying && isTriggered) {
    deg+=animationSpeed;
    }

    if(Application.isPlaying && !isTriggered && deg!=-90f) {
    
      if(deg>90f) deg=90f-(deg-90f);
    
      deg-=animationSpeed;    
      if(deg<-90f) deg=-90f;

    }

    if(deg>360f) deg-=360f;

  //---
  
    if(Application.isPlaying) {
  
      position=Mathf.Sin(Mathf.Deg2Rad*deg)+1f;
      position/=2f;     
        
      Vector3 p=new Vector3();
      p.x=pos.x;
      p.y=pos.y;
      p.z=originalZ;
      transform.position=p;
    
      velocity=pos-prevPos;
      prevPos=pos;
    
    }
          
  }

//------------

  public override void OnCollisionStay2D(Collision2D collision) {
  
    if(collision.collider.transform.parent!=null) {
    
      Transform t=collision.collider.transform;
      t.parent.SetParent(transform);

      string tag=t.parent.tag;
                
      if(triggeredByPlayer && tag=="Player") {
      
        isTriggered=true;
        
        if(untriggerCoroutine!=null) {
        StopCoroutine(untriggerCoroutine);
        untriggerCoroutine=null;
        }

      }
      
      /*
      if(tag!="Platform" && isTriggered) {

        Rigidbody2D b=collision.collider.gameObject.GetComponent<Rigidbody2D>();
        if(b==null) b=collision.collider.transform.parent.gameObject.GetComponent<Rigidbody2D>();   

        Vector2 v=velocity;
        v.x*=1000f;
 
        if(v.y>0f) {
        v.y*=400f;
        } else {
        v.y*=1000f;
        }

        if(b!=null) b.AddForce(v);

      }
      */
 
    }

  }
  
//------------

  void OnCollisionExit2D(Collision2D collision) {
  
      string tag=collision.collider.transform.parent.tag;
      
      Transform t=collision.collider.transform;
      t.parent.SetParent(null);      
      
      Rigidbody2D rb=t.parent.GetComponent<Rigidbody2D>();
      
      /*
      if(rb!=null) {
      
        Vector2 v=velocity;
        v.x*=10f;
        v.y=rb.velocity.y;
        rb.velocity=v;
      
      }
      */
      
      if(triggeredByPlayer && tag=="Player") {
      
        if(untriggerCoroutine!=null) {
        StopCoroutine(untriggerCoroutine);
        untriggerCoroutine=null;
        }      

        untriggerCoroutine=StartCoroutine(untrigger());
        
      }

  }  
  
  IEnumerator untrigger() {
  
    yield return new WaitForSeconds(1f);
    isTriggered=false;
  
  }

}
}
