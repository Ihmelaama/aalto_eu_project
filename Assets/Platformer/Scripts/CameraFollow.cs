using UnityEngine;
using System.Collections;

namespace Platformer {
public class CameraFollow : MonoBehaviour {

//----------------------------------------------------------
// VARIABLES

    private enum FollowMode {
    NONE,
    CENTER,
    ANTICIPATE
    }

  // settings ---
  
    private float lerpSpeedX=0.075f;
    private float lerpSpeedY=0.1f;
    private float lerpHurrySpeed=0.003f;

    private float followRadiusX=3f;
    private float followRadiusY=2f;
    
    private float anticipationRadiusX=7f;
    private float anticipationRadiusY=3f;

  // holders ---

    public Transform target;
    private Rigidbody2D targetBody;
    private Character targetCharacter;

  // state ---
  
    private float lerpHurry=0f;
    private FollowMode mode=FollowMode.CENTER;
    private Vector3 futureCameraPos;
    
//----------------------------------------------------------
// EVENTS

	void Start() {
  
    if(target!=null) {
    
      targetBody=target.GetComponent<Rigidbody2D>();
      targetCharacter=target.GetComponent<Character>();
      centerToTarget(true);
      
      futureCameraPos=transform.position;

    }
	
	}

//------------
	
  void FixedUpdate() {
  
    if(target!=null) {
  
      Vector3 dif=target.position-futureCameraPos;
      float targetVelocityX=targetBody.velocity.x;
      float targetVelocityY=targetBody.velocity.y;
    
    // anticipate horizontal movement ---
    
      if(Mathf.Abs(targetBody.velocity.x)>0.1f) {
      
        if(mode!=FollowMode.ANTICIPATE) {
        lerpHurry=0f;
        }
        
        if(
        Mathf.Abs(dif.x)>followRadiusX && 
        dif.x/Mathf.Abs(dif.x)==targetBody.velocity.x/Mathf.Abs(targetBody.velocity.x)
        ) {
        mode=FollowMode.ANTICIPATE;
        }
        
        if(mode==FollowMode.ANTICIPATE) {
        anticipatePosition(0);
        }
  
    // stop following ---
      
      } else {
  
        if(mode!=FollowMode.NONE) {
        
          mode=FollowMode.NONE;
          lerpHurry=0f; 
          futureCameraPos=transform.position;
          
        }
  
      } 
      
    // follow vertical movement ---
  
      if(Mathf.Abs(dif.y)>followRadiusY) {
        
        float d=Mathf.Abs(Mathf.Abs(dif.y)-followRadiusY);
        d/=anticipationRadiusY;
        futureCameraPos.y=Mathf.Lerp(futureCameraPos.y, target.position.y, d*lerpSpeedY);
  
      }
      
      if(targetVelocityY<-8f) {
      
        float y=(targetVelocityY+8f)/12f;
        futureCameraPos.y+=y;
      
      }
      
    // set position ---
    
      transform.position=Vector3.Lerp(transform.position, futureCameraPos, 0.1f);
    
    }
    
  }
  
//----------------------------------------------------------
// PRIVATE SETTERS

  private void lerpToPosition(Vector3 target) {
  lerpToPosition(target, 1f);
  }

  private void lerpToPosition(Vector3 target, float hurry) {
  
    target.z=transform.position.z;
    futureCameraPos=Vector3.Lerp(futureCameraPos, target, lerpSpeedX*hurry);
  
  }
  
//------------

  private Vector3 anticipatePosition(int axis) {
  
    Vector3 pos=futureCameraPos;
    Vector3 center=centerToTarget(false);

    if(axis==0) {
    float moveDirX= Mathf.Abs(targetBody.velocity.x)<0.01f ? 0f : targetBody.velocity.x/Mathf.Abs(targetBody.velocity.x) ;
    pos.x=center.x+moveDirX*anticipationRadiusX;
    }
    
    if(axis==1) {
    float moveDirY= Mathf.Abs(targetBody.velocity.y)<0.01f ? 0f : targetBody.velocity.y/Mathf.Abs(targetBody.velocity.y) ;
    pos.y=center.y+moveDirY*anticipationRadiusY;
    }
  
    lerpHurry=Mathf.Lerp(lerpHurry, 1f, lerpHurrySpeed);
    lerpToPosition(pos, lerpHurry);  

  return pos;
  }

//----------------------------------------------------------
// PRIVATE GETTERS

  private Vector3 centerToTarget(bool move) {

    Vector3 pos=target.transform.position;
    pos.z=transform.position.z;
    
    if(move) transform.position=pos;

  return pos;
  }

}
}