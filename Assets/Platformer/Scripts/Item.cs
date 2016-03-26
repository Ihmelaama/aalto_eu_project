using UnityEngine;
using System.Collections;

namespace Platformer {
public class Item : MonoBehaviour {

//---------------------------------------------------------
// VARIABLES

  // settings ---

    public int id=-1;
    public int value=0;
    
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
        if(Inventory.instance!=null) Inventory.instance.addItem(id, sprite, gameObject, character);
        gameObject.SetActive(false);
      
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
  
}
}
