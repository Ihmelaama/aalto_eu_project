using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class DialogueManager : MonoBehaviour {

//----------------------------------------------------
// VARIABLES

  // settings ---
  
    public GameObject talkButton;
    public GameObject dialogueOption;
  
  // holders ---
  
    private Camera currentCamera;
    private Transform UICanvas;
    private Transform NPCHolder;
    private List<Transform> NPCs;
    private List<GameObject> talkButtons=new List<GameObject>();
    private DialogueMenu dialogueMenu;
    private ItemManager itemManager;

  // state ---

    private int dialogueType=0;
    
    public static Transform currentTalkTarget=null;

//----------------------------------------------------
// START

  void Awake() {
  
  }

  void Start() {

    currentCamera=GameObject.Find("PlayerCamera").GetComponent<Camera>();
    UICanvas=GameObject.Find("UI/Canvas").transform;
    NPCHolder=GameObject.Find("NPCHolder").transform;  
    
    dialogueMenu=GameObject.Find("UI/Canvas/DialogueMenu").GetComponent<DialogueMenu>();
    dialogueMenu.gameObject.SetActive(true);
    
    itemManager=gameObject.GetComponent<ItemManager>();
    
  }
  
//----------------------------------------------------
// EVENTS

	void Update() {
  }  

//----------------------------------------------------
// PUBLIC SETTERS

  public void showDialogueMenu(Transform talkTarget, int type) {
  
    if(dialogueMenu!=null && UICanvas!=null) {
    
      DialogueManager.currentTalkTarget=talkTarget;
    
      NPC NPCScript=talkTarget.gameObject.GetComponent<NPC>();

      dialogueMenu.gameObject.SetActive(true);
      dialogueMenu.setDialogue(NPCScript, dialogueType);

      WorldState.allowUserInput=false;     

      itemManager.toggleInventory(true, 2);

    }  
  
  }
  
//------------  

  public void hideDialogueMenu() {
  
    if(dialogueMenu!=null) {
    dialogueMenu.gameObject.SetActive(false);
    itemManager.toggleInventory(false);
    }
    
    WorldState.allowUserInput=true;   
    currentTalkTarget=null;
  
  }  

//----------------------------------------------------
// PRIVATE SETTERS

}
