using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemManager : MonoBehaviour {

//----------------------------------------------------
// VARIABLES

  // holders ---
  
    public static ItemManager instance;

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
  
    instance=this;

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
    
      SoundManager.playGameSound(SoundManager.GameSound.GiveItem);    
    
      NPC npc=DialogueManager.currentTalkTarget.gameObject.GetComponent<NPC>();

      // check if npc wants item
      if(npc!=null) {
      b=npc.receiveItem(item);      
      }
      
      if(b) {
      npc.sayYes();
      } else {
      npc.sayNo();
      }
      
    }

  return b;
  } 
  
//---------  

  public static void foundItem(Item item) {
  
  // pick up ---
  
      item.PickUp();
      
  // check if mission ---
      
      MissionManager.checkIfMission(item, MissionManager.ActionType.find);
      
  // play sound ---

      SoundManager.playGameSound(SoundManager.GameSound.PickUpItem);

  }  
  
//--------- 

  public void toggleInventory(bool b) {
  toggleInventory(b, 0);
  }

  public void toggleInventory(bool b, int mode) {
  
    if(b && inventory.getItemCount()>0) {
                                               
      if(mode!=0) inventory.setMode(mode);    
      inventoryActionMenu.ShowActions();
      inventorySlider.toggleDashboard(true);          
    
    } else {
    
      inventorySlider.toggleDashboard(false);
      inventoryActionMenu.CloseMenu();
    
    }

  }   
  
}
