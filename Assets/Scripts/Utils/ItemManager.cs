using UnityEngine;
using System.Collections;

public class ItemManager : MonoBehaviour {

//----------------------------------------------------
// VARIABLES

  // holders ---

    private Inventory inventory;
    private SlidingDashboard inventorySlider;
    private ActionMenu inventoryActionMenu;
        
    private DialogueManager dialogueManager;

  // state ---
  
    public static int currentItemId=-1;
    public static int currentItemType=-1;

//----------------------------------------------------
// START

	void Start() {

    inventory=GameObject.Find("Inv/Inventory").GetComponent<Inventory>();
    inventorySlider=GameObject.Find("Inv").GetComponent<SlidingDashboard>();
    inventoryActionMenu=GameObject.Find("Inv/Inventory/ActionMenu").GetComponent<ActionMenu>();
    
    dialogueManager=GameObject.Find("Scripts").GetComponent<DialogueManager>();
	
	}
  
//----------------------------------------------------
// EVENTS  

	void Update() {
	
	}
  
//----------------------------------------------------
// PUBLIC SETTERS
                
  public bool giveItemToNPC(Item item) {
  
    bool b=false;
    if(DialogueManager.currentTalkTarget) {
    
      NPC npc=DialogueManager.currentTalkTarget.gameObject.GetComponent<NPC>();

      // check if npc wants item
      if(npc!=null) {
      b=npc.receiveItem(item);      
      }
      
      if(b) {
      }
      
    }

  return b;
  } 
  
//---------  

  public static bool foundItem(Item item) {
  
      item.PickUp();
      
      bool b=MissionManager.checkIfMission(item, MissionManager.ActionType.find);
    
      if(b) {
      // mission progressed! Give visual feedback
      }

  return b;
  }  
  
//--------- 

  public void toggleInventory(bool b) {
  toggleInventory(b, 0);
  }

  public void toggleInventory(bool b, int mode) {
  
    if(b && inventory.getItemCount()>0) {
    
      if(mode!=0) inventory.setMode(mode);    
      inventorySlider.toggleDashboard(true);          
    
    } else {
    
      inventorySlider.toggleDashboard(false);
      inventoryActionMenu.CloseMenu();
    
    }

  }   

}
