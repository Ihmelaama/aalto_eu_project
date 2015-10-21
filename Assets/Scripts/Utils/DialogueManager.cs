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
    
    private GameObject player;
    private Player playerScript;
    private LazyFollow playerCameraFollow;
    
    private Transform NPCHolder;

    private List<GameObject> talkButtons=new List<GameObject>();
    private DialogueMenu dialogueMenu;
    private SlidingDashboard dashboard;
    
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
    
    player=GameObject.Find("PlayerHolder/Player");
    playerScript=player.GetComponent<Player>();
    playerCameraFollow=Camera.main.GetComponent<LazyFollow>();
     
    NPCHolder=GameObject.Find("NPCHolder").transform;  
    
    dialogueMenu=GameObject.Find("UI/Canvas/DialogueMenu").GetComponent<DialogueMenu>();
    dialogueMenu.gameObject.SetActive(false);
    
    itemManager=gameObject.GetComponent<ItemManager>();
        
    dashboard=GameObject.Find("UI/Canvas/Dashboard").GetComponent<SlidingDashboard>();
                                 
  }
  
//----------------------------------------------------
// EVENTS

	void Update() {
  }  

//----------------------------------------------------
// PUBLIC SETTERS

  public void showDialogueMenu(Transform talkTarget) {
  
    if(dialogueMenu!=null && UICanvas!=null && currentTalkTarget==null) {
    
      currentTalkTarget=talkTarget;
    
      NPC NPCScript=talkTarget.gameObject.GetComponent<NPC>();

      dialogueMenu.gameObject.SetActive(true);
      dialogueMenu.setDialogue(NPCScript);
      moveCameraToTarget(talkTarget.gameObject);

      WorldState.allowUserInput=false;     

      itemManager.toggleInventory(true, 2);
      dashboard.toggleDashboard(true);

      NPCScript.sayHello();

      WorldState.playerIsTalking=true;  

    }  
  
  }
  
//------------  

  public void hideDialogueMenu() {
  
    if(dialogueMenu!=null) {
    dialogueMenu.gameObject.SetActive(false);
    itemManager.toggleInventory(false);
    dashboard.toggleDashboard(false);
    }
    
    WorldState.allowUserInput=true;
    playerCameraFollow.enabled=true;       
    currentTalkTarget=null;    
    
    moveCameraToTarget(player);
    itemManager.toggleInventory(false, 1);
    
    WorldState.playerIsTalking=false;
  
  }  
  
//------------  

  public void setNewDialogue(NPC npc, Dialogue.DialogueItem d) {
  dialogueMenu.setDialogue(npc, d);  
  moveCameraToTarget(npc.gameObject);
  }  
  
//------------

  public void moveCameraToTarget(GameObject g) {
  
    Transform head=g.transform.Find("HeadMarker");
    playerCameraFollow.followTarget= head==null ? g : head.gameObject ;
    
  
  }

//----------------------------------------------------
// PRIVATE SETTERS

}
