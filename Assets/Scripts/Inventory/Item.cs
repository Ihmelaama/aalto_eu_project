using UnityEngine;
using System.Collections;

[System.Serializable]
public class Item : MonoBehaviour{

	public string itemName;
	public int itemID;
	public string itemDesc;
	public Texture2D itemIcon;
	public Sprite itemSprite;
	public Sprite itemSpriteHighlighted;
	public int itemValue;
	public ItemType itemType;
	public int itemMaxStackSize;

	public enum ItemType{Food,Quest,Test}

    void OnMouseDown()
    {
        //walk to the object first?
        PickUp();
    }

    public void PickUp()
    {
        Debug.Log("Picked up " + itemName);
        Inventory.instance.AddItem(this);
    }


    public void Use(){
		switch (itemType) {
		case ItemType.Food:Debug.Log("food");break;
		case ItemType.Quest:Debug.Log("quest");break;
		case ItemType.Test:Debug.Log("test");break;
		default:break;
		}
	}
	

}
