using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DialogueManager : MonoBehaviour {

//----------------------------------------------------
// VARIABLES

  // settings ---
  
    public GameObject talkButton;
    public GameObject talkMenu;
  
  // holders ---
  
    Transform UICanvas;
    Transform NPCHolder;
  
  // state ---

    private bool userIsLookingToTalk=false;
    private bool userIsTalking=false;
    
//----------------------------------------------------
// START

  void Start() {
  
    NPCHolder=GameObject.Find("NPCHolder").transform;  
    UICanvas=GameObject.Find("UI/Canvas").transform;

  }
  
//----------------------------------------------------
// EVENTS

	void Update() {
	
    if(userIsLookingToTalk) {
    }
  
  }  

//----------------------------------------------------
// PUBLIC SETTERS

  public void initDialogue() {

  // show available talk targets
  
    if(!userIsLookingToTalk) {
  
      userIsLookingToTalk=true;
      showCharactersWithDialogue();
  
    } else {
    }
  
  }

//------------  
  
  public void chooseTalkTarget(Transform talkTarget) {
  
    Debug.Log("helou!");
    userIsTalking=true;
  
  }
  
//----------------------------------------------------
// PRIVATE SETTERSS

  private void showCharactersWithDialogue() {

    if(talkButton!=null) {
    
      GameObject button=Instantiate(talkButton);
      button.transform.SetParent(UICanvas, false);

      RectTransform rect=button.GetComponent<RectTransform>() as RectTransform;
      rect.anchoredPosition=new Vector2(100f, 100f);
      
      Button b=button.GetComponent<Button>() as Button;
      b.onClick.AddListener(() => chooseTalkTarget(null));

    }

  }

  
}
