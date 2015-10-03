using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DialogueMenu : MonoBehaviour {

//----------------------------------------------------
// VARIABLES

  // holders ---
  
    private GameObject playerBubble;
    private GameObject NPCBubble;
    private GameObject selector;
    private GameObject nextButton;
  
    //private Text playerBubbleText;
    private Text NPCBubbleText;
    private Text selectorText;
    
    private DialogueManager dialogueManager;
    
  // state ---
  
    private Dialogue currentDialogue;
    private Dialogue.DialogueItem currentDialogueItem;
    private int currentSelectorPos=0;
  
//----------------------------------------------------
// EVENTS

	void Start() {
  
    //playerBubble=transform.Find("DialogueHistory/Player").gameObject;
    //playerBubbleText=playerBubble.transform.Find("Text").GetComponent<Text>();
    //playerBubble.SetActive(false);
    
    NPCBubble=transform.Find("DialogueHistory/NPC").gameObject;
    NPCBubbleText=NPCBubble.transform.Find("Text").GetComponent<Text>();
    
    selector=transform.Find("Selector").gameObject;
    selectorText=selector.transform.Find("Text").GetComponent<Text>();
    
    nextButton=transform.Find("Next").gameObject;
    
  //---
  
    dialogueManager=GameObject.Find("Scripts").GetComponent<DialogueManager>();

	}
	
//------------

	void Update() {
	
	}
  
//----------------------------------------------------
// PUBLIC SETTERS

  public void setDialogue(NPC npc) {

    Dialogue.DialogueItem dialogueItem=npc.getDialogue();
    setDialogue(npc, dialogueItem);
      
  }

  public void setDialogue(NPC npc, Dialogue.DialogueItem dialogueItem) {

    currentDialogue=npc.dialogue;
    currentDialogueItem=dialogueItem;
    NPCBubbleText.text=getRandomDialogueLine(dialogueItem);

    //playerBubbleText.text="";    
    //playerBubble.SetActive(false);

    selectorText.text=dialogueItem.replies[0];  
    
  // show/hide parts of UI that are needed/not ---
    
    if(dialogueItem.replies.Count==1) {
    nextButton.SetActive(false);
    } else {
    nextButton.SetActive(true);
    }

  }

//---------

  public void browseThingsToSay(int direction) {
  
    currentSelectorPos+=direction;
    
    if(currentSelectorPos>=currentDialogueItem.replies.Count) {
    currentSelectorPos=0;
    }
    
    if(currentSelectorPos<0) {
    currentSelectorPos=currentDialogueItem.replies.Count-1;
    }
    
    selectorText.text=currentDialogueItem.replies[currentSelectorPos];
  
  }  
  
//---------

  public void sayThing() {
    
    string str=currentDialogueItem.replies[currentSelectorPos];
    //playerBubble.SetActive(true); 
    //playerBubbleText.text=str;

    // if conversation has somewhere to go
    if(currentDialogueItem.gotoItems.Count>currentSelectorPos) {

      str=currentDialogueItem.gotoItems[currentSelectorPos];
      currentDialogueItem=currentDialogue.getDialogueByName(str);
      NPCBubbleText.text=getRandomDialogueLine();

      currentSelectorPos=0;
      selectorText.text=currentDialogueItem.replies[currentSelectorPos];

    // else quit talking
    } else {

      currentSelectorPos=0;
      dialogueManager.hideDialogueMenu();

    }
    
  }
  
//----------------------------------------------------
// HELPERS

  private string getRandomDialogueLine() {
  return getRandomDialogueLine(currentDialogueItem);
  }

  private string getRandomDialogueLine(Dialogue.DialogueItem item) {
  return item.lines[Random.Range(0, item.lines.Count)];
  }  

}
