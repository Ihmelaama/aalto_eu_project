using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;

public class SlidingDashboard : MonoBehaviour {

//------------------------------------------------------
// VARIABLES

  // settings ---
  
    public SoundManager.UISound soundIn=0;
    public SoundManager.UISound soundOut=0;
  
    private float slideDuration=0.5f;

  // holders ---

    public int hideDirection=-1;

    private Button toggleButton;
    private RectTransform rect;
    private Vector2 anchoredPosition;
    
  // state ---
  
    [HideInInspector]
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
  
    if(WorldState.allowUserInput) {
    toggleDashboard(!visible, true);
    }
  
  }
  
  public void toggleDashboard(bool b) {
  
    if(WorldState.allowUserInput) {
    toggleDashboard(b, true);
    }
  
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
  
    visible=true;
  
    if(!animate) {
    position=1f;
    
    } else {
    SoundManager.playUISound(soundIn);
    Tween t=DOTween.To(()=>position, x=>position=x, 1f, slideDuration);
    t.SetEase(Ease.OutQuint);
    }
  
  }
  
//------------  

  public void Hide() {
  Hide(true);
  }
  
  public void Hide(bool animate) {
  
    visible=false;
  
    if(!animate) {
    position=0f;

    } else {
    SoundManager.playUISound(soundOut);     
    Tween t=DOTween.To(()=>position, x=>position=x, 0f, slideDuration);   
    t.SetEase(Ease.OutQuint);
    }

  }

}
