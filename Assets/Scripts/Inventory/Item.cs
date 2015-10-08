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

	public enum ItemType{Food,Quest,Test, Drop}

    public void PickUp()
    {
        Inventory.instance.AddItem(this);
        Destroy(this.gameObject);
    }


    public void Use(Slot slot){
        ActionMenu.instance.ShowMenu(slot);
        /*switch (itemType) {
		    case ItemType.Food:Debug.Log("food");break;
		    case ItemType.Quest:Debug.Log("quest");break;
		    case ItemType.Test:Debug.Log("test");break;
            case ItemType.Drop: ActionMenu.instance.ShowMenu(slot); break;
		default:break;
		}*/
	}
	

}
