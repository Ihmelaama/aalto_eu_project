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

  // state ---

    private int state=0;
    private int dialogueType=0;

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
    
  // get NPCs with dialogue ---
  
    getCharactersWithDialogue();
    
  }
  
//----------------------------------------------------
// EVENTS

	void Update() {
	
    if(state==1) {
    updateTalkButtons();
    }
  
  }  

//----------------------------------------------------
// PUBLIC SETTERS

  public void toggleDialogue() {
  toggleDialogue(Constants.TALK);
  }

  public void toggleDialogue(int type) {

  // reset talk state if different type of dialogue ---
  
    if(dialogueType!=0 && dialogueType!=type) {
    
      hideDialogueMenu();    
      hideCharactersWithDialogue();    
      state=0;  
      dialogueType=0;
      
    }
      
  // else show available talk targets ---        

    dialogueType=type;

  // show available talk targets
  
    if(state==0) {
  
      state=1;
      showCharactersWithDialogue();

    } else {
    
      if(state==1) {
      
        hideCharactersWithDialogue();
        state=0;
        dialogueType=0;
      
      } else {
      
        hideDialogueMenu();
        showCharactersWithDialogue();
        WorldState.allowUserInput=true; 
        state=1;
      
      }
      
    }

  }

//------------  
  
  public void showDialogueMenu(Transform talkTarget) {
  
    if(state!=2 && dialogueMenu!=null && UICanvas!=null) {
    
      NPC NPCScript=talkTarget.gameObject.GetComponent<NPC>();

      dialogueMenu.gameObject.SetActive(true);
      dialogueMenu.setDialogue(NPCScript, dialogueType);

      state=2;
      WorldState.allowUserInput=false;     
      hideCharactersWithDialogue();

    }  
  
  }
  
//------------  

  public void hideDialogueMenu() {
  
    if(dialogueMenu!=null) {
    dialogueMenu.gameObject.SetActive(false);
    }
  
  }  

//----------------------------------------------------
// PRIVATE SETTERS

  private void getCharactersWithDialogue() {
  
    NPCs=new List<Transform>();
    Transform[] t=NPCHolder.GetComponentsInChildren<Transform>();

    for(int i=0; i<t.Length; i++) {
    
      if(t[i].GetComponent<NPC>()!=null) {
      
        if(t[i].GetComponent<NPC>().dialogue!=null) {
        NPCs.Add(t[i]);
        }
      
      }
      
    }
  
  }
  
//------------

  private void showCharactersWithDialogue() {
  
    if(talkButton!=null) {

      for(int i=0; i<NPCs.Count; i++) {
      createTalkButton(NPCs[i]);
      }

    }

  }
  
//------------

  private void hideCharactersWithDialogue() {
  
    for(int i=0; i<talkButtons.Count; i++) {
    Destroy(talkButtons[i]);
    }
    
    talkButtons.Clear();  

  }
  
//------------

  private void createTalkButton(Transform target) {
  
    if(talkButton!=null && UICanvas!=null) {
  
      GameObject button=Instantiate(talkButton);
      button.transform.SetParent(UICanvas, false);
  
      Vector2 pos=currentCamera.WorldToScreenPoint(target.position);
  
      RectTransform rect=button.GetComponent<RectTransform>() as RectTransform;
      rect.anchoredPosition=pos;
      
      Button b=button.GetComponent<Button>() as Button;
      b.onClick.AddListener(() => showDialogueMenu(target));  
      
      setButtonType(button, dialogueType);
  
      talkButtons.Add(button);
    
    }
  
  }
  
  //--- 
  
  private void setButtonType(GameObject button, int type) {

    Text buttonText=button.transform.Find("Text").GetComponent<Text>();

    switch(type) {
          
      case Constants.USER_GIVES_ITEM:
      buttonText.text="Tap to give";
      break;
    
      default:
      case Constants.TALK:
      buttonText.text="Tap to talk";
      break;    
    
    }

  }
  
//------------

  private void updateTalkButtons() {
  
    for(int i=0; i<NPCs.Count; i++) {
    
      Vector2 pos=currentCamera.WorldToScreenPoint(NPCs[i].position);  
      talkButtons[i].transform.position=pos;  
    
    }

  }

}
