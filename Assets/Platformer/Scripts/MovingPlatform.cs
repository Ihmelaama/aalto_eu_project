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
      transform.position=pos;
      
    }

  }
	
}
}
