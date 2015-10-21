using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Inventory : MonoBehaviour {

	//public List<Item> inventory = new List<Item>();
	public List<Item> inventory = new List<Item>();
    public static Inventory instance;
   


	public RectTransform inventoryRect;
	private float inventoryWidth, inventoryHeight;

    public Button inventoryButton;
    //public bool SliderOn;
	public int slots;
	public int rows;
	public float slotPaddingLeft, slotPaddingTop;
	public float slotSize;
	public GameObject slotPrefab;
	public Canvas canvas;

	public GameObject iconPrefab;

	public EventSystem eventSystem;
  
  // 1=use, 2=give ---
  [HideInInspector]
  public int inventoryMode=1;

	private static Slot from, to;


	private static int emptySlot;

	public static int EmptySlot{
		get{return emptySlot;}
		set{emptySlot = value;}
	}

	private List<GameObject> allSlots = new List<GameObject>();

	void Start () {
        instance = this;
		CreateLayout ();
	}

	private void CreateLayout(){

		emptySlot = slots;

        slotSize = (Screen.height-slotPaddingTop*2) / slots;
        rows = slots;
        //inventoryRect.anchoredPosition = new Vector3(-slotSize, 0f); For som reason moves too far in on tablet

		int columns = (slots / rows);
        GameObject invParent = GameObject.Find("Inventory");
		for (int y = 0; y<rows; y++) {
			for (int x = 0; x<columns; x++) {
				GameObject newSlot = (GameObject)Instantiate(slotPrefab);
				RectTransform slotRect = newSlot.GetComponent<RectTransform>();
				newSlot.name = "Slot";
                slotRect.sizeDelta = new Vector2(slotSize, slotSize);
				newSlot.transform.SetParent(invParent.transform);
                slotRect.position = inventoryRect.position + new Vector3(0f, (-y*slotSize)-slotPaddingTop);
				allSlots.Add(newSlot);
			}
		}
	}

    


	void Update () {
		if (Input.GetMouseButtonUp (0)) {
			if(!eventSystem.IsPointerOverGameObject(-1) && from != null){
				from.GetComponent<Image>().color = Color.grey;
				from.ClearSlot();
				
			}
		}
	}

	private bool PlaceEmpty(Item item){
		if (emptySlot > 0) {
			foreach(GameObject slot in allSlots){
				Slot tmp = slot.GetComponent<Slot>();
				if(tmp.isEmpty){
					tmp.AddItem (item);
					emptySlot--;
					return true;
				}
			}
		}
		return false;
	}

	public bool AddItem(Item item){

		if (item.itemMaxStackSize <= 1) {
			PlaceEmpty (item);
			return true;
		}
		else {
			foreach(GameObject slot in allSlots){
				Slot tmp = slot.GetComponent<Slot>();
				if(!tmp.isEmpty){
					//if(tmp.CurrentItem.itemType == item.itemType && tmp.isAvailable){
          //if(tmp.CurrentItem.itemID == item.itemID && tmp.isAvailable){
          if(tmp.CurrentItem.itemName == item.itemName && tmp.isAvailable){
						tmp.AddItem(item);
						return true;
					}
				}
			}
		}
		if (emptySlot > 0) {
			PlaceEmpty(item);
		}
		return false;
	}
  
  // set inventory to use mode (1) or give mode (2)
  public void setMode(int mode) {
  
    if(inventoryMode!=mode) {
    inventoryMode=mode;
    }

  }

  public int getItemCount() {
  
    int num=0;
    
    foreach(GameObject slot in allSlots){
    Slot tmp=slot.GetComponent<Slot>();
    if(!tmp.isEmpty) num++;
    }
  
  return num;
  }
  
  public List<Item> getItems() {
  
    List<Item> items=new List<Item>();
    
    foreach(GameObject slot in allSlots){
    Slot tmp=slot.GetComponent<Slot>();
    if(!tmp.isEmpty) items.Add(tmp.CurrentItem);
    }
  
  return items;
  }  

}
