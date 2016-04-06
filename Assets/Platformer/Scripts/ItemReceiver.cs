using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Platformer {
public class ItemReceiver : MonoBehaviour {

  public enum Behaviour {
  NONE,
  ADVANCE_SPRITE,
  MAKE_PHYSICAL
  }

//---------------------------------------------------------------------
// VARIABLES

  // settings ---

    public int acceptsItemId=-1;
    public int maxAmount=-1;

    public bool destroyItem=false;    
    public Behaviour behaviour;
    
    public Sprite[] sprites;
    
  // holders ---
  
    private SpriteRenderer spriteRenderer=null;
    
  // state ---
  
    private int itemCounter=0;
    private int currentSprite=0;

//---------------------------------------------------------------------
// EVENTS

	void Start() {
  
    if(transform.Find("Sprite")!=null) {
    spriteRenderer=transform.Find("Sprite").GetComponent<SpriteRenderer>();
    }

	}
  
//---------

	void Update() {
	
	}
  
//---------

  void OnCollisionEnter2D(Collision2D c) {
  checkItemCollision(c.collider);
  }
  
//--------- 

  void OnTriggerEnter2D(Collider2D c) {
  checkItemCollision(c);
  }
  
//---------------------------------------------------------------------
// PUBLIC SETTERS  

//---------------------------------------------------------------------
// PRIVATE SETTERS

  private void checkItemCollision(Collider2D c) {

    Item item=c.gameObject.GetComponent<Item>();       

    if(item!=null && item.id==acceptsItemId) {

      if(maxAmount<0 || itemCounter<maxAmount) {

        if(itemCounter>=maxAmount) {
        
        itemsFull();
        } else {        
        receivedItem(item);
        itemCounter++;        
        }
        
        if(destroyItem) item.Destroy();        
      
      }
    
    }
    
  }
  
//------------

  private void receivedItem(Item item) {
  
    switch(behaviour) {
    
      case Behaviour.ADVANCE_SPRITE:
      advanceSprite();
      break;
      
      case Behaviour.MAKE_PHYSICAL:
      makePhysical(item);
      break;
    
    }
  
  }
  
//------------

  private void itemsFull() {
  Debug.Log("has all items!");
  }
  
//------------

  private void advanceSprite() {        
  
    currentSprite++;
    
    if(spriteRenderer!=null && currentSprite<sprites.Length) {
    spriteRenderer.sprite=sprites[currentSprite];
    }
  
  }  
  
//------------

  private void makePhysical(Item item) {
  
    Rigidbody2D[] bodies=GetComponentsInChildren<Rigidbody2D>();
    
    for(int i=0; i<bodies.Length; i++) {
    
      bodies[i].isKinematic=false;
    
    }
  
  }  
  
}
}
