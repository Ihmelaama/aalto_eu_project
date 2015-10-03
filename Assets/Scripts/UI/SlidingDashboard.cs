using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SlidingDashboard : MonoBehaviour {

//------------------------------------------------------
// VARIABLES

  // holders ---

    public int hideDirection=-1;

    private Button toggleButton;
    private RectTransform rect;
    private Vector2 anchoredPosition;
    
  // state ---
  
    public bool visible=false;
    private bool isAnimating=false;
    
    private float position=0f;

//------------------------------------------------------
// EVENTS

  void Awake() {
  
    toggleButton=transform.Find("ToggleButton").gameObject.GetComponent<Button>();
    toggleButton.onClick.AddListener(toggleDashboard);
    
    rect=(RectTransform) transform;
    anchoredPosition=rect.anchoredPosition;
    
    if(!visible) {
    Hide(false);
    }

  }
  
//------------  
  
  void Update() { 
  
    anchoredPosition.x=hideDirection*(rect.sizeDelta.x/2f)*(1f-position); 
    rect.anchoredPosition=anchoredPosition;

  }
  
//------------------------------------------------------
// PUBLIC SETTERS  

  public void toggleDashboard() {
  toggleDashboard(!visible, true);
  }
  
  public void toggleDashboard(bool b) {
  toggleDashboard(b, true);
  }  

  public void toggleDashboard(bool b, bool animate) {
  
    if(b!=visible) {
    
      if(b) {
      Show(animate);
      } else {
      Hide(animate);
      }
 
    }
  
  }
  
//------------  
  
  public void Show() {
  Show(true);
  }
  
  public void Show(bool animate) {
  
    //if(!animate) {
    
      position=1f;
      visible=true;
    
    //} else {
    
      //DOTween.To(position
    
    //}
  
    /*
    if(!animate) {
  
      anchoredPosition.x=0f;
      rect.anchoredPosition=anchoredPosition;
      visible=true;
      
    } else {
    }
    */
  
  //---
  
    Fabric.EventManager.Instance.PostEvent("UI/Button");    
        
  }
  
//------------  

  public void Hide() {
  Hide(true);
  }
  
  public void Hide(bool animate) {
  
    position=0f;
    visible=false;
  
    /*
    if(!animate) {
  
      anchoredPosition.x=hideDirection*rect.sizeDelta.x/2f;
      rect.anchoredPosition=anchoredPosition;  
      visible=false;
    
    } else {
    }
    */
    
  //---    
    
    Fabric.EventManager.Instance.PostEvent("UI/Button");    

  }

}
