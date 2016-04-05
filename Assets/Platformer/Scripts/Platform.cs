using UnityEngine;
using System.Collections;

namespace Platformer {

[ExecuteInEditMode]
public class Platform : MonoBehaviour {

//---------------------------------------------------------
// VARIABLES

  // settings ---
  
    public float scale=1f;
    public float width=1f;
    public float height=1f;
    public float depth=0f;
    
    public bool playerCanMoveThrough=true;

    public bool isDeadly=false;
    public bool isElevator=false;
    public bool isSuperBouncy=false;
    
    private float _scale=1f;
    private float _width=1f;
    private float _height=1f;
    private float _depth=0f;
    
  // holders ---
  
    private BoxCollider2D collider;
  
    private SpriteRenderer topLeft;
    private SpriteRenderer top;
    private SpriteRenderer topRight;
    
    private SpriteRenderer left;
    private SpriteRenderer middle;
    private SpriteRenderer right;     
    
    private SpriteRenderer bottomLeft;
    private SpriteRenderer bottom;
    private SpriteRenderer bottomRight; 
    
    [HideInInspector]
    public Rigidbody2D body;       
       
//---------------------------------------------------------
// EVENTS

	public virtual void Start() {
  
    collider=GetComponent<BoxCollider2D>();
    handleSize();
    
    body=GetComponent<Rigidbody2D>();
	
	}
  
//-----------
	
	public virtual void Update() {
  
    handleSize();
    handleDepth();
	
	}
  
//------------

  public virtual void OnCollisionEnter2D(Collision2D collision) {
  }

//------------

  public virtual void OnCollisionStay2D(Collision2D collision) {
  }

//---------------------------------------------------------
// PRIVATE SETTERS

  private void handleSize() {
  
    if(Mathf.Abs(scale)!=_scale || Mathf.Abs(width)!=_width || Mathf.Abs(height)!=_height) {
    
      _scale=Mathf.Abs(scale);
      _width=Mathf.Abs(width);
      _height=Mathf.Abs(height);    
      
      float w, h;
      w=_width/_scale;
      h=_height/_scale;

    // scale ---
    
      transform.localScale=_scale*Vector3.one;
  
      if(collider==null) collider=GetComponent<BoxCollider2D>();
      if(collider!=null) collider.size=new Vector2(w*2f+1f, h*2f+1f);
      
    // corners ---

      if(topLeft==null) topLeft=transform.Find("topLeft").GetComponent<SpriteRenderer>();
      if(topLeft!=null) {
      
        topLeft.transform.localPosition=new Vector3(-w, h, 0f);  
        topLeft.transform.localScale=new Vector3(0.5f, 0.5f, 1f);
      
      }

      if(topRight==null) topRight=transform.Find("topRight").GetComponent<SpriteRenderer>();
      if(topRight!=null) {
      
        topRight.transform.localPosition=new Vector3(w, h, 0f); 
        topRight.transform.localScale=new Vector3(0.5f, 0.5f, 1f); 
      
      }
          
      if(bottomLeft==null) bottomLeft=transform.Find("bottomLeft").GetComponent<SpriteRenderer>();
      if(bottomLeft!=null) {
      
        bottomLeft.transform.localPosition=new Vector3(-w, -h, 0f); 
        bottomLeft.transform.localScale=new Vector3(0.5f, 0.5f, 1f); 
      
      }
      
      if(bottomRight==null) bottomRight=transform.Find("bottomRight").GetComponent<SpriteRenderer>();
      if(bottomRight!=null) {
      
        bottomRight.transform.localPosition=new Vector3(w, -h, 0f);  
        bottomRight.transform.localScale=new Vector3(0.5f, 0.5f, 1f);
      
      }
      
    // sides ---
    
      if(top==null) top=transform.Find("top").GetComponent<SpriteRenderer>();
      if(top!=null) {
      
        top.transform.localPosition=new Vector3(0f, h, 0f);
        top.transform.localScale=new Vector3(w, 0.5f, 1f);
        
      }
      
      if(right==null) right=transform.Find("right").GetComponent<SpriteRenderer>();
      if(right!=null) {
      
        right.transform.localPosition=new Vector3(w, 0f, 0f);  
        right.transform.localScale=new Vector3(0.5f, h, 1f);
      
      }   
      
      if(bottom==null) bottom=transform.Find("bottom").GetComponent<SpriteRenderer>();
      if(bottom!=null) {
      
        bottom.transform.localPosition=new Vector3(0f, -h, 0f);  
        bottom.transform.localScale=new Vector3(w, 0.5f, 1f);
      
      }       
      
      if(left==null) left=transform.Find("left").GetComponent<SpriteRenderer>();
      if(left!=null) {
      
        left.transform.localPosition=new Vector3(-w, 0f, 0f);  
        left.transform.localScale=new Vector3(0.5f, h, 1f);
      
      }   
      
    // middle ---               

      if(middle==null) middle=transform.Find("middle").GetComponent<SpriteRenderer>();
      if(middle!=null) {
      
        middle.transform.localScale=new Vector3(w-0.45f, h-0.45f, 1f);
      
      }

    }
  
  }
  
//------------

  private void handleDepth() {
  
    if(depth!=_depth) {
    
      _depth=depth;
      
      Vector3 pos=transform.localPosition;
      pos.z=depth;
      transform.localPosition=pos;
    
    }
  
  }

}
}
