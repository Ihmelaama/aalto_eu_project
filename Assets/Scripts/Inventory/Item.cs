using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Item : MonoBehaviour{

  //---

  	public string itemName;
  	public int itemID;
  	public string itemDesc;
  	public Texture2D itemIcon;
  	public Sprite itemSprite;
  	public Sprite itemSpriteHighlighted;
  	public int itemValue;
  	public ItemType itemType;
  	public int itemMaxStackSize;
    
    public bool canBeGiven=true;
    public bool canBeUsed=true;
    
  	public enum ItemType{Food,Quest,Test, Drop}

  //---
  
    private GameObject graphics;
    private SpriteRenderer graphicsSpriteRenderer;  
  
//--------------------------------------------------------
// EVENTS

  void Awake() {

    graphics=transform.Find("Graphics").gameObject; 
    if(graphics!=null) graphicsSpriteRenderer=graphics.GetComponent<SpriteRenderer>();    
  
    setItemSprite();
    
  }

//--------------------------------------------------------
// PUBLIC SETTERS

  public void PickUp()
  {
        
      if(Inventory.instance.getItemCount()<5) {
      Inventory.instance.AddItem(this);
      Destroy(this.gameObject);
      
      } else {
      Player.instance.sayNo();
      }
      
  }

//------------

  public void Use(Slot slot){

      if(canBeGiven || canBeUsed) {
      ActionMenu.instance.ShowMenu(slot);
      }

	}                
  
//--------------------------------------------------------
// PRIVATE SETTERS 

  private void setItemSprite() {

    if(graphicsSpriteRenderer!=null && itemSprite!=null) {
    graphicsSpriteRenderer.sprite=itemSprite;
    }  
  
  }  
    
}
