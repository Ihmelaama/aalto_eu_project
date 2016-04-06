using UnityEngine;
using System.Collections;

namespace Platformer {
public class Item : MonoBehaviour {

//---------------------------------------------------------
// VARIABLES

    public enum PowerUpType {
    NONE,
    BOOST_JUMP
    }

  // settings ---

    public int id=-1;
    public int value=0;
    public PowerUpType powerUp=PowerUpType.NONE;
    public float powerUpValue=0f;
    
  // holders ---
  
    private Sprite sprite;
    private Collider2D collider;
    
  // state ---
  
    [HideInInspector]
    public bool ignorePlayer=false;

//---------------------------------------------------------
// EVENTS

	void Start() {
  
    sprite=GetComponent<SpriteRenderer>().sprite;
    collider=GetComponent<Collider2D>();
	
	}
  
//------------
	
	void Update() {
	
	}
  
//------------

  void OnCollisionEnter2D(Collision2D c) {
  
    if(!ignorePlayer && c.collider.tag=="Player") {
       
      Character character=c.collider.GetComponent<Character>();
      if(character==null) character=c.collider.transform.parent.GetComponent<Character>();
      
      if(character!=null && character.isPlayer) {
        
        character.collectedItem(this);
        
        if(Inventory.instance!=null && id!=-1) {
        Inventory.instance.addItem(id, sprite, gameObject, character);
        transform.parent=GameObject.Find("SharedItemHolder").transform;
        }
        
        gameObject.SetActive(false);
        
        if(powerUp!=PowerUpType.NONE) {
        applyPowerUp(character);
        }
        
      // sound ---
      
        if(value<0) {
        SoundManager.instance.playVesalaPickupSound(false);  
        
        } else if(id==-1 || powerUp==PowerUpType.NONE) {
        SoundManager.instance.playVesalaPickupSound(true);  
        
        } else {
        SoundManager.instance.playVesalaPowerUpSound();  
        }
      
      }
      
    }
  
  }  
  
//---------------------------------------------------------
// PUBLIC SETTERS

  public void setIgnorePlayer(bool b, GameObject player) {
  
    ignorePlayer=b;
    
    Collider2D[] playerColliders=player.GetComponentsInChildren<Collider2D>();
    for(int i=0; i<playerColliders.Length; i++) {
    Physics2D.IgnoreCollision(collider, playerColliders[i], b);
    }
    
    if(b) StartCoroutine(unignorePlayer_(1f, player));

  }  
  
//------------
  
  private IEnumerator unignorePlayer_(float wait, GameObject player) {
  
    yield return new WaitForSeconds(wait);
    setIgnorePlayer(false, player);
  
  }
  
//------------

  public void Destroy() {
  Destroy(gameObject);
  }  
  
//---------------------------------------------------------
// PRIVATE SETTERS  

  private void applyPowerUp(Character c) {
  
    switch(powerUp) {
    
      case PowerUpType.BOOST_JUMP:
      c.jumpForce+=powerUpValue;
      break;
    
    }
  
  }
  
}
}
