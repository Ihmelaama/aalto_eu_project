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
    private LazyFollow playerCameraFollow;
    private SlidingDashboard dashboard;

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
    dialogueMenu.gameObject.SetActive(false);
    
    itemManager=gameObject.GetComponent<ItemManager>();
    
    playerCameraFollow=Camera.main.GetComponent<LazyFollow>();
    
    dashboard=GameObject.Find("UI/Canvas/Dashboard").GetComponent<SlidingDashboard>();
                                 
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
      dialogueMenu.setDialogue(NPCScript);
      moveCameraToTarget(talkTarget);

      WorldState.allowUserInput=false;     

      itemManager.toggleInventory(true, 2);
      //dashboard.toggleDashboard(true);

      // construct event string for fabric: "Chars/Hello/"+GameState.currentPlayerCharacterName or something
      Debug.Log("say hello!");
      Fabric.EventManager.Instance.PostEvent("Chars/Hello/Name");    

    }  
  
  }
  
//------------  

  public void hideDialogueMenu() {
  
    if(dialogueMenu!=null) {
    dialogueMenu.gameObject.SetActive(false);
    itemManager.toggleInventory(false);
    }
    
    //dashboard.toggleDashboard(false);
    
    WorldState.allowUserInput=true;
    playerCameraFollow.enabled=true;       
    currentTalkTarget=null;    
  
  }  
  
//------------  

  public void setNewDialogue(NPC npc, Dialogue.DialogueItem d) {
  dialogueMenu.setDialogue(npc, d);  
  moveCameraToTarget(npc.transform);
  }  
  
//------------

  public void moveCameraToTarget(Transform t) {
  playerCameraFollow.enabled=false;
  }

//----------------------------------------------------
// PRIVATE SETTERS

}
