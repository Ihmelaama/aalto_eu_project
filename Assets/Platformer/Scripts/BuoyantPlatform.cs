using UnityEngine;
using System.Collections;

namespace Platformer {
public class BuoyantPlatform : Platform {

  private float incrMass=0.1f;
  private bool isFloating=false;
  private float mass;
  
  public override void Start() {
  
    base.Start();
    mass=body.mass;
  
  }

  public override void OnCollisionStay2D(Collision2D collision) {
  
    string tag="";
    
    if(collision.collider.transform.parent!=null) {
    tag=collision.collider.transform.parent.tag;
    }
     
    if(isFloating && tag=="Player") {
    mass+=incrMass;
    body.mass=mass;
    }
  
  }  
  
  void OnTriggerStay2D(Collider2D other) {

    if(other.tag=="Liquid") {
    isFloating=true;
    }
  
  }

  void OnTriggerExit2D(Collider2D other) {
  
    if(other.tag=="Liquid") {
    isFloating=false;
    }
    
  }
   
}
}
