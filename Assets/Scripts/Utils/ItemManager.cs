using UnityEngine;
using System.Collections;

public class ItemManager : MonoBehaviour {

//----------------------------------------------------
// VARIABLES

  // holders ---
  
    private DialogueManager dialogueManager;
    
  // state ---
  
    public static int currentItemId=-1;
    public static int currentItemType=-1;

//----------------------------------------------------
// START

	void Start() {
  
    dialogueManager=gameObject.GetComponent<DialogueManager>();
	
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
                       
  public void chooseWhoToGiveItem(int itemType) {
  
    dialogueManager.toggleDialogue(Constants.USER_GIVES_ITEM);

  }  

}
