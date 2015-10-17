using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IPointerClickHandler {

	private Stack<Item> items;

	public Stack<Item> Items {
		get {return items;}
	}

	public Text stackText;
	public Sprite slotEmpty;
	public Sprite slotHighlight;

    public float slotWidth;
    private float slotHeight;
    private Vector2 slotPosition;
  


	public Item CurrentItem{

		get{
    
      if(items.Count>0) {
      return items.Peek();
      } else {
      return null;
      }

    }
    
	}

	public bool isEmpty{
		get { return (items.Count == 0);}
	}

	public bool isAvailable{
		get{ return CurrentItem.itemMaxStackSize > items.Count;}
	}

	void Start () {
		items = new Stack<Item> ();
		RectTransform slotRect = GetComponent<RectTransform> ();
		RectTransform txtRect = stackText.GetComponent<RectTransform> ();

		int txtScaleFactor = (int)(slotRect.sizeDelta.x * 0.60);
		stackText.resizeTextMaxSize = txtScaleFactor;
		stackText.resizeTextMinSize = txtScaleFactor;

		txtRect.SetSizeWithCurrentAnchors (RectTransform.Axis.Horizontal, slotRect.sizeDelta.x);
		txtRect.SetSizeWithCurrentAnchors (RectTransform.Axis.Vertical, slotRect.sizeDelta.y);

        RectTransform rectTrans = this.gameObject.GetComponent<RectTransform>();
        slotPosition = rectTrans.position;
        slotHeight = rectTrans.rect.height;
        slotWidth = rectTrans.rect.width;
        slotPosition.x += slotWidth-20;
        slotPosition.y -= (slotHeight / 2);


    }

	
	public void AddItem(Item item){
		items.Push (item);

		if (items.Count > 1) {
			stackText.text = items.Count.ToString();
		}

		ChangeSprite (item.itemSprite, item.itemSpriteHighlighted);
	}

	public void AddItems(Stack<Item> items){
		this.items = new Stack<Item> (items);
		stackText.text = items.Count > 1 ? items.Count.ToString() : string.Empty;
		ChangeSprite (CurrentItem.itemSprite, CurrentItem.itemSpriteHighlighted);
	}

	private void ChangeSprite(Sprite normal, Sprite highlight){  
		GetComponent<Image> ().sprite = normal;
		SpriteState st = new SpriteState ();
		st.highlightedSprite = highlight;
		st.pressedSprite = normal;

		GetComponent<Button> ().spriteState = st;
	}


	public void UseItem(){  
		if (!isEmpty) {

			//items.Pop().Use(this);
            items.Peek().Use(this);

			stackText.text = items.Count > 1 ? items.Count.ToString() : string.Empty;

			if(isEmpty){
				ChangeSprite(slotEmpty,slotHighlight);
				Inventory.EmptySlot++;
			}
		}
	}

    public void DropItem(bool returnToWorld)
    {
        if (!isEmpty)
        {
                 
            items.Pop().Use(this);
             
            stackText.text = items.Count > 1 ? items.Count.ToString() : string.Empty;

            if (isEmpty)
            {
                ChangeSprite(slotEmpty, slotHighlight);
                Inventory.EmptySlot++;
            } 
            
            if(returnToWorld) {
            // return to world
            Debug.Log("return item to world");
            }
            
        }
    }
  
    public void ClearSlot(){
		items.Clear ();
		ChangeSprite (slotEmpty, slotHighlight);
		stackText.text = string.Empty;
	  }

	#region IPointerClickHandler implementation

	public void OnPointerClick (PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Right) {
            UseItem();
        }
	}

	#endregion
}
