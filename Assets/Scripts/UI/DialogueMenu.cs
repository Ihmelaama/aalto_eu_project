using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DialogueMenu : MonoBehaviour {

//----------------------------------------------------
// VARIABLES

  // settings ---

    public string dialogueFile=null;
    public Dialogue dialogue=null;

  // holders ---
  
    private GameObject playerBubble;
    private GameObject NPCBubble;
    private GameObject selector;
  
    private Text playerBubbleText;
    private Text NPCBubbleText;
    private Text selectorText;
    
    private DialogueManager dialogueManager;
    
  // state ---
  
    private Dialogue.DialogueItem currentDialogueItem;
    private int currentSelectorPos=0;
  
//----------------------------------------------------
// EVENTS

	void Start() {
  
    playerBubble=transform.Find("DialogueHistory/Player").gameObject;
    playerBubbleText=playerBubble.transform.Find("Text").GetComponent<Text>();
    playerBubble.SetActive(false);
    
    NPCBubble=transform.Find("DialogueHistory/NPC").gameObject;
    NPCBubbleText=NPCBubble.transform.Find("Text").GetComponent<Text>();
    
    selector=transform.Find("Selector").gameObject;
    selectorText=selector.transform.Find("Text").GetComponent<Text>();
    
  //---
  
    dialogueManager=GameObject.Find("Scripts").GetComponent<DialogueManager>();
    
  //---

    if(dialogue==null && dialogueFile!=null && dialogueFile.Length>0) {
     
      dialogue=new Dialogue(dialogueFile);
      setDialogue(dialogue);

    }
    
  //---
  
    gameObject.SetActive(false);
    
	}
	
//------------

	void Update() {
	
	}
  
//----------------------------------------------------
// PUBLIC SETTERS

  public void setDialogue(Dialogue d) {
  
    dialogue=d;

    Dialogue.DialogueItem dialogueItem=d.getDefaultDialogue();
    currentDialogueItem=dialogueItem;
    
    NPCBubbleText.text=getRandomDialogueLine(dialogueItem);

    playerBubbleText.text="";    
    playerBubble.SetActive(false);

    selectorText.text=dialogueItem.replies[0];  

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
  
    string str;
    
    str=currentDialogueItem.replies[currentSelectorPos];
    playerBubble.SetActive(true); 
    playerBubbleText.text=str;  
  
    // if conversation has somewhere to go
    if(currentDialogueItem.gotoItems.Count>currentSelectorPos) {
    
      str=currentDialogueItem.gotoItems[currentSelectorPos];
      currentDialogueItem=dialogue.getDialogueByName(str);
      NPCBubbleText.text=getRandomDialogueLine();
    
      currentSelectorPos=0;
      selectorText.text=currentDialogueItem.replies[currentSelectorPos];
      
    // if not quit talking
    } else {
    
      currentSelectorPos=0;
      dialogueManager.toggleDialogue();
    
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
