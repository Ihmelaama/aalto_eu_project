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
    public bool SliderOn;
	public int slots;
	public int rows;
	public float slotPaddingLeft, slotPaddingTop;
	public float slotSize;
	public GameObject slotPrefab;
	public Canvas canvas;

	public GameObject iconPrefab;
	private static GameObject hoverObject;
	private float hoverYOffset;

	public EventSystem eventSystem;

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
        Debug.Log(slotSize);
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
					if(tmp.CurrentItem.itemType == item.itemType && tmp.isAvailable){
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

    bool inventoryVisible = false;
    public void HideInventory()
    {
        if (inventoryVisible)
        {
            inventoryRect.anchoredPosition = new Vector2(0f, 0f);
            inventoryVisible = false;
        }
        else
        {
            float percentage = Screen.width / (slotSize+slotPaddingLeft*2);
            inventoryRect.anchoredPosition = new Vector2(-Screen.width / (percentage), 0f);
            inventoryVisible = true;
        }
        /*foreach(GameObject slot in allSlots)
        {
            slot.SetActive(!slot.activeSelf);
        }*/
    }

}
