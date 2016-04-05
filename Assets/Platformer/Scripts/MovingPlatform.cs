using UnityEngine;
using System.Collections;

namespace Platformer {
public class MovingPlatform : Platform {

  public Vector2 position1;
  public Vector2 position2;
  public float animationSpeed=1f;
  
  private Vector2 originalPosition;
  private Vector2 movementRange;
  private float position=0f;
  private float deg=0f;
  
  private Vector2 velocity=Vector2.zero;
  private Vector2 prevPos=Vector2.zero;
  
//----------------------------------------------
	
  public override void Start() {
	
    originalPosition=((Vector2) transform.position)+position1;
    movementRange=position2-position1;
    base.Start();

	}

//----------------------------------------------
  
  public override void Update() {

    base.Update();
    
    if(Application.isPlaying) {
    
    //---
      
      position=Mathf.Sin(Mathf.Deg2Rad*deg)+1f;
      position/=2f;
      
      deg+=animationSpeed;
      
    //---

      Vector2 pos=originalPosition+movementRange*position;
      velocity=pos-prevPos;
      prevPos=pos;

      transform.position=pos;
      
    }

  }
  
//------------

  public override void OnCollisionStay2D(Collision2D collision) {
  
    if(collision.collider.transform.parent!=null) {
    
      string tag=collision.collider.transform.parent.tag;

      if(tag!="Platform") {
            
        Rigidbody2D b=collision.collider.transform.parent.gameObject.GetComponent<Rigidbody2D>();   
        if(b!=null) b.AddForce(velocity*1000f);

      }

    }

  }

}
}
