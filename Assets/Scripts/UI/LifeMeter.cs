using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class LifeMeter : MonoBehaviour {

//------------------------------------------------------
// VARIABLES

  // settings ---
  
    public Character targetCharacter;
    public int lifeValueNum;
  
    public Color fullColor;
    public Color emptyColor;
    
    public Sprite symbol;
    
    private float maxWidth;
    private float maxHeight;
    
  // holders ---
  
    private Image symbolImage;

    private Transform backgroundBar;
    private RectTransform backgroundBarRect;
    private Image backgroundBarImage;
  
    private Transform valueBar;
    private RectTransform valueBarRect;
    private Image valueBarImage;
    
  // state ---
  
    public float currentValue=0.5f;
  
    private Color currentColor;
    
//------------------------------------------------------
// EVENTS

	void Start() {
  
  // symbol ---
  
    symbolImage=transform.Find("Symbol").gameObject.GetComponent<Image>();
    if(symbol!=null) symbolImage.sprite=symbol;

  // get background bar ---
  
    backgroundBar=transform.Find("BackgroundBar");
    backgroundBarRect=(RectTransform) backgroundBar.transform;
    backgroundBarImage=backgroundBar.gameObject.GetComponent<Image>();

  // get value bar ---    

    valueBar=transform.Find("ValueBar");
    valueBarRect=(RectTransform) valueBar.transform;
    valueBarImage=valueBar.gameObject.GetComponent<Image>();
    
  // set stuff ---

    RectTransform r=(RectTransform) backgroundBar.transform;
    maxWidth=r.sizeDelta.x;
    maxHeight=r.sizeDelta.y;

    if(targetCharacter!=null) {
    currentValue=targetCharacter.lifeValues[lifeValueNum];
    }

    setValue(currentValue);
    setColor(currentValue);
    
	}

//------------
	
	void Update() {
	
    currentValue=targetCharacter.lifeValues[lifeValueNum];
    setValue(currentValue);
    setColor(currentValue);
  
	}
  
//------------------------------------------------------
// PUBLIC SETTERS

  public void setValue(float f) {
  
    Vector2 v=new Vector2(maxWidth*f, maxHeight);
    valueBarRect.sizeDelta=v;
  
  }
  
//------------  
  
  public void setColor(float f) {
  
    float col;

    col=emptyColor.r+(fullColor.r-emptyColor.r)*f;
    currentColor.r=col;

    col=emptyColor.g+(fullColor.g-emptyColor.g)*f;
    currentColor.g=col;
    
    col=emptyColor.b+(fullColor.b-emptyColor.b)*f;
    currentColor.b=col;
    
    col=emptyColor.a+(fullColor.a-emptyColor.a)*f;
    currentColor.a=col;

    valueBarImage.color=currentColor;

  }

}
