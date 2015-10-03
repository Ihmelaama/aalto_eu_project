using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ActionMenu : MonoBehaviour {

    public GameObject actMenu;
    public Button dropButton;
    public Button useButton;
    public Button giveButton;
    public static Slot currentSlot = null;

    public Text useText;

    public static ActionMenu instance;
    
    private Inventory inventory;
    private ItemManager itemManager;


	void Start () {
        instance = this;
        actMenu.gameObject.SetActive(false);
        inventory=transform.parent.gameObject.GetComponent<Inventory>();
        itemManager=GameObject.Find("Scripts").GetComponent<ItemManager>();
	}
	
    public void DropMenu()
    {
        Debug.Log("Drop it like it's hot");
        currentSlot.DropItem();
        actMenu.gameObject.SetActive(false);
    }
    
    public void UseMenu()
    {
        Debug.Log("Use it!");
        MissionManager.checkIfMission(currentSlot.CurrentItem, MissionManager.ActionType.use);
    }    
    
    public void GiveMenu()
    {

        bool b=itemManager.giveItemToNPC(currentSlot.CurrentItem);
        
        if(b) {
        currentSlot.DropItem();
        actMenu.gameObject.SetActive(false);
        }
         
    }    

    public void CloseMenu()
    {
        actMenu.gameObject.SetActive(false);
    }

    public void ShowMenu(Slot parent)
    {

        currentSlot = parent;
        transform.SetParent(parent.transform);
        actMenu.gameObject.SetActive(true);
        actMenu.transform.SetAsLastSibling();
        actMenu.GetComponent<RectTransform>().anchoredPosition = new Vector3(-parent.slotWidth-20f,0f,0f);

        dropButton.gameObject.SetActive(false);
        useButton.gameObject.SetActive(false);
        giveButton.gameObject.SetActive(false);

        if(inventory.inventoryMode==1) {
        
          dropButton.gameObject.SetActive(true);
          useButton.gameObject.SetActive(true);

        } else if(inventory.inventoryMode==2) {
        
          giveButton.gameObject.SetActive(true);
                  
        }

    }
	
}
