using UnityEngine;
using System.Collections;

namespace Platformer {
public class AnimatedPlatform : Platform {

  public bool isTriggeredByPlayer=false;
  
  private Animation animation;
  
  private bool isTriggered=false;

	void Start() {
  
    animation=GetComponent<Animation>();
	
	}
	
	void Update() {
	
	}
  
  public override void OnCollisionStay2D(Collision2D collision) {

    if(!isTriggered && collision.collider.transform.parent!=null) {
    
      string tag=collision.collider.transform.parent.tag;
      Debug.Log("tag: "+tag);

      if(tag=="Player" && !isTriggered) {
  
        GameObject g=collision.collider.gameObject;
        if(g.tag=="Untagged") g=collision.collider.transform.parent.gameObject;
        g.transform.SetParent(transform);
        
        if(animation!=null) {
        animation["Helicopter"].speed=0.2f;
        animation.Play();
        }
        
        isTriggered=true;
        
      }    
 
    }
  

  }  
  
//------------

  void OnCollisionExit2D(Collision2D collision) {
  
    GameObject g=collision.collider.gameObject;
    if(g.tag=="Untagged") g=collision.collider.transform.parent.gameObject;
    g.transform.SetParent(null);   

  }  
  
}
}
