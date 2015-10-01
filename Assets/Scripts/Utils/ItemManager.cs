using UnityEngine;
using System.Collections;

public class ItemManager : MonoBehaviour {

//----------------------------------------------------
// VARIABLES

  // holders ---

    private Inventory inventory;
    private SlidingDashboard inventorySlider;

  // state ---
  
    public static int currentItemId=-1;
    public static int currentItemType=-1;

//----------------------------------------------------
// START

	void Start() {

    inventory=GameObject.Find("Inv/Inventory").GetComponent<Inventory>();
    inventorySlider=GameObject.Find("Inv").GetComponent<SlidingDashboard>();
	
	}
  
//----------------------------------------------------
// EVENTS  

	void Update() {
	
	}
  
//----------------------------------------------------
// PUBLIC SETTERS

  public void dropItem(int itemType) {
  }
  
//---------  
  
  public void useItem(int itemType) {
  }
  
//---------  
                       
  public void giveItemToNPC(int itemType) {
  
    if(DialogueManager.currentTalkTarget) {
    
      NPC npc=DialogueManager.currentTalkTarget.gameObject.GetComponent<NPC>();
      if(npc!=null) npc.receiveItem(itemType);
    
    }

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
    
    }

  }   

}
