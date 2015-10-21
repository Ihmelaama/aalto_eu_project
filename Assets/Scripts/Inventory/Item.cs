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
    
    public int[] craftableWith;
    public int craftResult;
  
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
      Inventory.instance.AddItem(this);
      
      bool b=checkIfCraftable();
      
      Destroy(this.gameObject);
  }

//------------

  public void Use(Slot slot){

      if(canBeGiven || canBeUsed) {
      ActionMenu.instance.ShowMenu(slot);
      
      } else if(craftableWith.Length>0) {
      Craft();
      }

	}                
  
//------------

  public bool checkIfCraftable() {
  
    List<Item> items=Inventory.instance.getItems();
    int count=0;

    foreach(Item item in items) {
    
      if(Helpful.ArrayContainsInt(craftableWith, item.itemID)) {
      count++;
      }
    
    }
    
    if(count>=craftableWith.Length) {
    Debug.Log("craft result: "+craftResult);
    }
  
  return false;
  }
    
  public bool Craft() {
  
    /*
    if(Helpful.ArrayContainsInt(craftableWith, ID)) {
    
      return true;
    
    }
    */
    
    Debug.Log("so how the fuck do we craft this item?");
  
  return false;
  }
  
//--------------------------------------------------------
// PRIVATE SETTERS 

  private void setItemSprite() {

    if(graphicsSpriteRenderer!=null && itemSprite!=null) {
    graphicsSpriteRenderer.sprite=itemSprite;
    }  
  
  }  
    
}
