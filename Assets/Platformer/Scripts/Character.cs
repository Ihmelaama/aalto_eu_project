using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Platformer {
public class Character : MonoBehaviour {

//---------------------------------------------------------
// VARIABLES

    private enum AnimationState {
    IDLE,
    RUN,
    JUMP
    }

  // settings ---
  
    public string characterId=null;
    public bool isPlayer=false;
    public Texture2D spriteSheet;
  
    private float jumpForce=8f;
    private float directionalJumpForce=1f;
    private float directionalJumpThreshold=0.2f;
    
    private float horizontalVelocity=0.2f;
    private float horizontalFriction=0.85f;
    private float brakeFriction=0.6f;
    private float maxHorizontalSpeed=4f;
    
    private float airDriftVelocity=0.06f;  
    private float airBrakeFriction=0.7f;
    
    private float moveDurationLimit=1f;
    
    private float idleFrameRate=6f;
    private float runFrameRate=6f;
    private float jumpFrameRate=6f;
    
    private float throwForceX=3f;
    private float throwForceY=3f;
    
  // holders ---
  
    public static Character instance;
  
    private Collider2D characterCollider;
    private Collider2D groundCollider;
    private Collider2D triggerCollider;
    
    [HideInInspector]
    public Rigidbody2D body;
    
    private Sprite stillFrame;
    private List<Sprite> idleFrames;
    private List<Sprite> runFrames;
    private List<Sprite> jumpFrames;
    
    private GameObject graphics;
    private SpriteRenderer spriteRenderer;
    
    private Collider2D[] allColliders;
    
  // state ---
  
    [HideInInspector]
    public bool onGround=false;
    
    private GameObject currentGround=null;    
    private List<Collider2D> ignoredColliders=new List<Collider2D>();    
    
    [HideInInspector]
    public int faceDirection=1;
  
    private bool movingRight=false;
    private float movedRightDuration=0f;
    
    private bool movingLeft=false;
    private float movedLeftDuration=0f;
    
    private float horizontalSpeed=0f;
    
    private AnimationState animationState=AnimationState.IDLE;
    
    private bool isAnimated=false;

//---------------------------------------------------------
// EVENTS

  void Awake() {
  
    MonoBehaviour[] children=GetComponentsInChildren<MonoBehaviour>();
    for(int i=0; i<children.Length; i++) {
    children[i].tag=tag;
    }

  }
  
//------------

	void Start() {
  
    instance=this;
    
  //---
  
    characterCollider=transform.Find("CharacterCollider").GetComponent<Collider2D>();
    body=GetComponent<Rigidbody2D>();
    
    groundCollider=transform.Find("GroundCollider").GetComponent<Collider2D>();
    triggerCollider=transform.Find("TriggerCollider").GetComponent<Collider2D>();

    graphics=transform.Find("Graphics").gameObject;
    spriteRenderer=graphics.GetComponent<SpriteRenderer>();

  //---

    getAnimationSprites();
    if(isAnimated && idleFrames!=null) spriteRenderer.sprite=idleFrames[0];
    if(!isAnimated && stillFrame!=null) spriteRenderer.sprite=stillFrame;

    StartCoroutine(setAnimationFrame(1f/idleFrameRate, 0));
    
  //---
  
    allColliders=gameObject.GetComponentsInChildren<Collider2D>();
	
	}
  
//------------
	
	void Update() {
  
    if(isPlayer) {
    getKeyboardInput();
    getGameControllerInput();
    }
    
    handleHorizontalMovementOnGround();
    handleHorizontalMovementInAir();
    
  //---
    
    Vector2 v=body.velocity;
    v.x=horizontalSpeed;
    body.velocity=v;  
    
  //---
  
    if(!onGround && animationState!=AnimationState.JUMP) {
    animationState=AnimationState.JUMP;
    }

	}
  
//-----------

  void OnCollisionEnter2D(Collision2D c) {
  
    if(touchedGround(c)) {
    onGround=true;
    setCurrentGround(c.collider.gameObject);
    }        

  }
  
//-----------    
  
  void OnCollisionStay2D(Collision2D c) {
  
    if(touchedGround(c)) {
    onGround=true;
    //setCurrentGround(c.collider.gameObject);
    } 

  } 
  
//-----------  

  void OnTriggerEnter2D(Collider2D c) {
  
    if(!ignoredColliders.Contains(c) && touchedGround(c)) {
    onGround=true;
    setCurrentGround(c.gameObject);
    }

  } 

//----------- 

  void OnTriggerStay2D(Collider2D c) {

    if(!ignoredColliders.Contains(c) && touchedGround(c)) {
    onGround=true;
    setCurrentGround(c.gameObject);
    }  

  }

//-----------  

  void OnTriggerExit2D(Collider2D c) {

    unignoreCollider(c);
    onGround=false;

  }   

//---------------------------------------------------------
// PUBLIC SETTERS

  public void Drop() {
  
    if(onGround && currentGround!=null && currentGround.tag=="Platform" && currentGround.GetComponent<Platform>().playerCanMoveThrough) {
    
      Collider2D groundCollider=currentGround.GetComponent<Collider2D>();
      ignoreCollider(groundCollider);
      currentGround=null;    
      onGround=false;
    
    }
    
    if(!onGround) {
    Debug.Log("air drop?");
    }
  
  }
  
//------------

  public void Jump() {
  
    if(onGround) {
    
      float moveDir=0f;
      
      if(movingLeft) {
      
        moveDir=movedLeftDuration/moveDurationLimit;
        if(moveDir>1f) moveDir=1f;
        moveDir=1f-moveDir;
        moveDir*=-1f;

      } else if(movingRight) {
      
        moveDir=movedRightDuration/moveDurationLimit;
        if(moveDir>1f) moveDir=1f;
        moveDir=1f-moveDir;        
        
      }

      //horizontalSpeed*=0.65f;
      horizontalSpeed+=moveDir*directionalJumpForce;
    
      body.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
      onGround=false;    
      currentGround=null;
      unignoreCollider();

    }
    
  }
  
//------------
  
  public void moveLeft(bool b) {
  movingLeft=b;
  }  
  
//------------
  
  public void moveRight(bool b) {
  movingRight=b;
  }   
  
//------------

  public void throwItem(GameObject g) {
  
    Vector3 v;

    g.SetActive(true);
    g.GetComponent<Item>().setIgnorePlayer(true, gameObject);
    
    v=characterCollider.bounds.center;
    g.transform.position=v;
    
    v=new Vector3(throwForceX, throwForceY, 0f);
    v.x*=faceDirection;
    g.GetComponent<Rigidbody2D>().velocity=v;

  }   
  
//------------

  public void collectedItem(Item item) {
  
    if(isPlayer && ScoreCounter.instance!=null) {
    ScoreCounter.instance.changeScore(item.value);
    }
  
  }  
  
//---------------------------------------------------------
// PRIVATE SETTERS

  private void getAnimationSprites() {
  
    bool animated=false;

    if(spriteSheet==null && characterId!=null) {
    
      if(isPlayer && GameState.playerCharacterId!=null) {
      characterId=GameState.playerCharacterId;
      }
    
      spriteSheet=Resources.Load<Texture2D>("Platformer/Characters/Animated/"+characterId);
      if(spriteSheet!=null) animated=true;
      
      if(spriteSheet==null) {
      Resources.Load<Texture2D>("Platformer/Characters/Still/"+characterId);
      animated=false;
      }
      
    }
  
    if(spriteSheet!=null) {

      int fullW=spriteSheet.width;
      int fullH=spriteSheet.height;    

    // animated sprite ---

      if(animated) {
      
        int w=fullW/4;
        int h=w;
        Sprite sprite;
        
        idleFrames=new List<Sprite>();
        for(int i=0; i<4; i++) {
    
          sprite=Sprite.Create(spriteSheet, new Rect(i*w, fullH-h, w, h), new Vector2(0.5f, 0f));
          idleFrames.Add(sprite);
        
        }
        
        runFrames=new List<Sprite>();
        for(int i=0; i<4; i++) {
    
          sprite=Sprite.Create(spriteSheet, new Rect(i*w, fullH-h*2, w, h), new Vector2(0.5f, 0f));
          runFrames.Add(sprite);
        
        }    
        
        jumpFrames=new List<Sprite>();
        for(int i=0; i<4; i++) {
    
          sprite=Sprite.Create(spriteSheet, new Rect(i*w, fullH-h*3, w, h), new Vector2(0.5f, 0f));
          jumpFrames.Add(sprite);
        
        }      
        
    // still sprite ---
            
      } else {
      }
      
    //---
      
      graphics.transform.localScale=Vector3.one*(fullW/150f);
      
    }

  }
  
//------------  

  private IEnumerator setAnimationFrame(float wait, int frameNum) {
  
    yield return new WaitForSeconds(wait);

    switch(animationState) {
    
      case AnimationState.IDLE:

        if(idleFrames!=null) {

          spriteRenderer.sprite=idleFrames[frameNum];
          
          frameNum=Random.Range(0, 4);
          StartCoroutine(setAnimationFrame(1f/idleFrameRate, frameNum));
          
        }
        
      break;
      
      case AnimationState.RUN:
      
        if(runFrames!=null) {
      
          spriteRenderer.sprite=runFrames[frameNum];
        
          frameNum++;
          if(frameNum>3) frameNum=0;      
          StartCoroutine(setAnimationFrame(1f/runFrameRate, frameNum));      

        }

      break;
      
      case AnimationState.JUMP:
      
        if(jumpFrames!=null) {
        
          if(body.velocity.y>2f) spriteRenderer.sprite=jumpFrames[0];
          if(body.velocity.y>0f && body.velocity.y<=2f) spriteRenderer.sprite=jumpFrames[1];
          if(body.velocity.y<=0f && body.velocity.y>=-2f) spriteRenderer.sprite=jumpFrames[2];
          if(body.velocity.y<-2f) spriteRenderer.sprite=jumpFrames[3];
          
          StartCoroutine(setAnimationFrame(1f/jumpFrameRate, 0)); 
          
        }
      
      break;    
    
    }    
  
  }
  
//------------

  private void setFaceDirection(int dir) {
  
    Vector3 v=graphics.transform.localScale;
    v.x=dir*Mathf.Abs(v.x);
    graphics.transform.localScale=v;  
    
    faceDirection=dir;  

  }

//------------

  private void getKeyboardInput() {
  
    if(Input.GetKeyDown(KeyCode.LeftArrow)) {
    moveLeft(true);
    }
    
    if(Input.GetKeyUp(KeyCode.LeftArrow)) {
    moveLeft(false);
    }
    
  //---    
    
    if(Input.GetKeyDown(KeyCode.RightArrow)) {
    moveRight(true);
    }
    
    if(Input.GetKeyUp(KeyCode.RightArrow)) {
    moveRight(false);
    }    
    
  //---
  
    if(Input.GetKeyUp(KeyCode.DownArrow)) {
    Drop();
    }      

    if(Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow)) {
    Jump();
    }   
  
  }
  
//------------
  
  private void getGameControllerInput() {
  }

//------------
  
  private void handleHorizontalMovementOnGround() {
  
    if(onGround) {
    
    // update speed ---
    
      float s=horizontalSpeed;
  
      if(movingLeft && !movingRight) {
      s-=horizontalVelocity;

      } else if(!movingLeft && movingRight) {
      s+=horizontalVelocity;
      
      } else {
      s*=horizontalFriction;
      }  
  
      if(Mathf.Abs(s)>maxHorizontalSpeed) {
      s=maxHorizontalSpeed*s/Mathf.Abs(s);
      }
      
      if(movingLeft && s>0f) {
      s*=brakeFriction;
      }
      
      if(movingRight && s<0f) {
      s*=brakeFriction;
      }      

      horizontalSpeed=s;

    // update time ---
      
      if(movingLeft) {
      movedLeftDuration+=Time.deltaTime;
      } else {
      movedLeftDuration=0f;
      }
      
      if(movingRight) {
      movedRightDuration+=Time.deltaTime;
      } else {
      movedRightDuration=0f;
      }      
      
    // update animation state ---
    
      if(movingLeft || movingRight) {
      animationState=AnimationState.RUN;
      
      } else {
      animationState=AnimationState.IDLE;      
      }
      
      if(movingLeft && !movingRight) {
      setFaceDirection(-1);
      
      } else if(!movingLeft && movingRight) {
      setFaceDirection(1);
      }

    }
  
  }
  
//------------

  private void handleHorizontalMovementInAir() {
  
    if(!onGround) {
    
      if(movingLeft) {
      if(horizontalSpeed>0f) horizontalSpeed*=airBrakeFriction;
      horizontalSpeed-=airDriftVelocity;
      setFaceDirection(-1);
      }
      
      if(movingRight) {
      if(horizontalSpeed<0f) horizontalSpeed*=airBrakeFriction;
      horizontalSpeed+=airDriftVelocity;
      setFaceDirection(1);
      }  

    }
  
  }  
  
//------------

  private void setCurrentGround(GameObject g) {
  
    if(g.tag=="Platform") {
    currentGround=g; 
    }
  
  }  
  
//------------

  private void ignoreCollider(Collider2D c) {
  
    if(c.gameObject==currentGround && !ignoredColliders.Contains(c)) {
    unignoreCollider(c);
    ignoredColliders.Add(c);
    Physics2D.IgnoreCollision(groundCollider, c, true);
    }

  }
  
//------------  

  private void unignoreCollider() {
  
    for(int i=0; i<ignoredColliders.Count; i++) {
    unignoreCollider(ignoredColliders[i]);
    }
  
  }

  private void unignoreCollider(Collider2D c) {
  
    if(ignoredColliders.Contains(c)) {
    Physics2D.IgnoreCollision(groundCollider, c, false);
    ignoredColliders.Remove(c);
    }

  }  
  
//---------------------------------------------------------
// HELPFUL

  private bool touchedGround(Collision2D collision) {

    Vector2 colliderMin=groundCollider.bounds.center;
    colliderMin.y-=groundCollider.bounds.extents.y-0.1f;

    foreach(ContactPoint2D c in collision.contacts) {

      if(c.point.y>colliderMin.y || body.velocity.y>1f) {
      return false;
      }
    
    }    
    
  return true;
  }
  
//------------
  
  private bool touchedGround(Collider2D collider) {
         
    Bounds b=collider.bounds;
    Vector2 center=(Vector2) collider.bounds.center;
    Vector2 pos=collider.transform.InverseTransformPoint(transform.position);
    
    Vector2 pos1=pos;
    pos1+=new Vector2(-0.5f, -0.2f);
    
    Vector2 pos2=pos;
    pos2+=new Vector2(0.5f, -0.2f);  
    
    Vector2 pos3=pos;
    pos3+=new Vector2(-0.5f, 0.2f);
    
    Vector2 pos4=pos;
    pos4+=new Vector2(0.5f, 0.2f);      
    
    pos1=collider.transform.TransformPoint(pos1);  
    pos2=collider.transform.TransformPoint(pos2);  
    pos3=collider.transform.TransformPoint(pos3);  
    pos4=collider.transform.TransformPoint(pos4);     
    
    bool contains1=collider.OverlapPoint(pos1);
    bool contains2=collider.OverlapPoint(pos2);
    bool contains3=collider.OverlapPoint(pos3);
    bool contains4=collider.OverlapPoint(pos4);    
    
    //Debug.Log(collider.name+" - "+contains1+" "+contains2+" - "+contains3+" "+contains4);
    //Debug.DrawLine(pos1, pos2, Color.red);
    //Debug.DrawLine(pos3, pos4, Color.red);
    
    if(contains1==true && contains2==true && contains3==false && contains4==false) {
    return true;
    }

  return false;
  }
                                            
}
}