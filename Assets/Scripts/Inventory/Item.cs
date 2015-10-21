using UnityEngine;
using System.Collections;

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
      Destroy(this.gameObject);
  }

//------------

  public void Use(Slot slot){
      ActionMenu.instance.ShowMenu(slot);
      /*switch (itemType) {
	    case ItemType.Food:Debug.Log("food");break;
	    case ItemType.Quest:Debug.Log("quest");break;
	    case ItemType.Test:Debug.Log("test");break;
      case ItemType.Drop: ActionMenu.instance.ShowMenu(slot); break;
	    default:break;
      */
	}                
  
//------------
    
  public bool craftWith(int ID) {
  
    if(Helpful.ArrayContainsInt(craftableWith, ID)) {
    
      return true;
    
    }
  
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
