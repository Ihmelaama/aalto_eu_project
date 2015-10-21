using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class DialogueMenu : MonoBehaviour {

//----------------------------------------------------
// VARIABLES

  // holders ---
  
    private GameObject playerBubble;
    private GameObject NPCBubble;
    private GameObject selector;
    private GameObject nextButton;
    private GameObject prevButton;
  
    private Text NPCBubbleText;
    private Text selectorText;
    
    private DialogueManager dialogueManager;
    
  // state ---
  
    private Dialogue currentDialogue;
    private Dialogue.DialogueItem currentDialogueItem;
    private int currentSelectorPos=0;
  
//----------------------------------------------------
// EVENTS

	void Awake() {

    NPCBubble=transform.Find("DialogueHistory/NPC").gameObject;
    NPCBubbleText=NPCBubble.transform.Find("Text").GetComponent<Text>();

    selector=transform.Find("Selector").gameObject;
    selectorText=selector.transform.Find("Text").GetComponent<Text>();
    
    nextButton=transform.Find("Next").gameObject;
    prevButton=transform.Find("Previous").gameObject;
    
  //---
  
    dialogueManager=GameObject.Find("Scripts").GetComponent<DialogueManager>();

	}
	
//------------

	void Update() {
  
    // hide dialogue if user touched somewhere else than interactable dialogue objects
    if(GestureManager.wasTouched) {
    dialogueManager.hideDialogueMenu();
    GestureManager.clearTouch();
    }
	
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

    selectorText.text=dialogueItem.replies[0];  
    
  // show/hide parts of UI that are needed/not ---
    
    if(dialogueItem.replies.Count>1) {
    toggleSelectorButtons(true);
    
    } else {
    toggleSelectorButtons(false);
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
    
    SoundManager.instance.playUISound(1);    

  }  
  
//---------

  public void sayThing() {
    
    string str=currentDialogueItem.replies[currentSelectorPos];      

    // if conversation has somewhere to go
    if(currentDialogueItem.gotoItems.Count>currentSelectorPos) {

      str=currentDialogueItem.gotoItems[currentSelectorPos];
      currentDialogueItem=currentDialogue.getDialogueByName(str);
      NPCBubbleText.text=getRandomDialogueLine();

      currentSelectorPos=0;
      List<string> replies=currentDialogueItem.replies;
      
      selectorText.text=replies[currentSelectorPos];
      
      if(replies.Count>1) {
      toggleSelectorButtons(true);
      
      } else {
      toggleSelectorButtons(false);
      }

    // else quit talking
    } else {

      currentSelectorPos=0;
      dialogueManager.hideDialogueMenu();

    }
    
    SoundManager.instance.playUISound(1);
    
  }
  
//----------------------------------------------------
// PRIVATE SETTERS

  private void toggleSelectorButtons(bool b) {
  
    nextButton.SetActive(b);
    prevButton.SetActive(b);
  
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
