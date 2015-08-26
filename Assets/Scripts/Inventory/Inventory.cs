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

		hoverYOffset = slotSize*0.01f;

        float screenWidth = Screen.width;
         slots = (int)(screenWidth / slotSize+slotPaddingLeft);

		inventoryWidth = (slots / rows) * (slotSize + slotPaddingLeft) + slotPaddingLeft;
		inventoryHeight = rows * (slotSize + slotPaddingTop) + slotPaddingTop;
		inventoryRect.SetSizeWithCurrentAnchors (RectTransform.Axis.Horizontal, inventoryWidth);
		inventoryRect.SetSizeWithCurrentAnchors (RectTransform.Axis.Vertical, inventoryHeight);

		int columns = (slots / rows);

		for (int y = 0; y<rows; y++) {
			for (int x = 0; x<columns; x++) {
				GameObject newSlot = (GameObject)Instantiate(slotPrefab);
				RectTransform slotRect = newSlot.GetComponent<RectTransform>();
				newSlot.name = "Slot";
				newSlot.transform.SetParent(this.transform.parent);
				slotRect.localPosition = inventoryRect.localPosition + new Vector3(slotPaddingLeft*(x+1)+(slotSize*x), 
				         -slotPaddingTop * (y+1)-(slotSize*y));
				slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, slotSize);
				slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, slotSize);
				allSlots.Add(newSlot);
			}
		}
	}

	void TestPrint(){
		for (int i = 0; i<inventory.Count; i++) {
			print(inventory[i].itemName);
		}
	}

	void Update () {

		if (Input.GetMouseButtonUp (0)) {
			if(!eventSystem.IsPointerOverGameObject(-1) && from != null){
				from.GetComponent<Image>().color = Color.grey;
				from.ClearSlot();
				Destroy(GameObject.Find("Hover"));
				to = null;
				from = null;
				hoverObject = null;
			}
		}

		if (hoverObject != null) {
			Vector2 position;
			RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform,Input.mousePosition,canvas.worldCamera,out position);
			position.Set (position.x,position.y-hoverYOffset);
			hoverObject.transform.position = canvas.transform.TransformPoint(position);
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


    public void HideInventory()
    {
        foreach(GameObject slot in allSlots)
        {
            slot.SetActive(!slot.activeSelf);
        }
    }

	/*public void MoveItem(GameObject clicked){
		if (from == null) {
			if (!clicked.GetComponent<Slot> ().isEmpty) {
				from = clicked.GetComponent<Slot> ();
				from.GetComponent<Image> ().color = Color.grey;

				hoverObject = (GameObject)Instantiate(iconPrefab);
				hoverObject.GetComponent<Image>().sprite = clicked.GetComponent<Image>().sprite;
				hoverObject.name = "Hover";

				RectTransform hoverTransform = hoverObject.GetComponent<RectTransform>();
				RectTransform clickedTransform = clicked.GetComponent<RectTransform>();

				hoverTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal,clickedTransform.sizeDelta.x);
				hoverTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical,clickedTransform.sizeDelta.y);

				hoverObject.transform.SetParent(GameObject.Find("Canvas").transform,true);
				hoverObject.transform.localScale = from.gameObject.transform.localScale;
			}
		} else if (to == null) {
			to = clicked.GetComponent<Slot>();
			Destroy(GameObject.Find("Hover"));
		}
		if (to != null && from != null) {
			Stack<Item> tmpTo = new Stack<Item>(to.Items);
			to.AddItems(from.Items);

			if(tmpTo.Count == 0){
				from.ClearSlot();
			}
			else{
				from.AddItems(tmpTo);
			}

			from.GetComponent<Image>().color = Color.gray;
			to = null;
			from = null;
			hoverObject = null;
		}
	}*/

}
