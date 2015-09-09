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

//------------------------------------------------------
// EVENTS

  void Awake() {
  
    toggleButton=transform.Find("ToggleButton").gameObject.GetComponent<Button>();
    toggleButton.onClick.AddListener(toggleDashboard);
    
    rect=(RectTransform) transform;
    anchoredPosition=rect.anchoredPosition;
    
    if(!visible) {
    Hide();

    } else {
    Show();
    }

  }
  
//------------------------------------------------------
// PUBLIC SETTERS  

  public void toggleDashboard() {
  toggleDashboard(!visible);
  }

  public void toggleDashboard(bool b) {
  
    if(b!=visible) {
    
      if(b) {
      Show(false);
      } else {
      Hide(false);
      }
 
    }
  
  }
  
//------------  
  
  public void Show() {
  Show(true);
  }
  
  public void Show(bool animate) {
  
    anchoredPosition.x=0f;
    rect.anchoredPosition=anchoredPosition;
    visible=true;
        
  }
  
//------------  

  public void Hide() {
  Hide(true);
  }
  
  public void Hide(bool animate) {
  
    anchoredPosition.x=hideDirection*rect.sizeDelta.x/2f;
    rect.anchoredPosition=anchoredPosition;  
    visible=false;

  }

}
