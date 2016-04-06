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
    
    public bool debugRun=false;
  
    [HideInInspector]
    public float jumpForce=10f;
    
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
    private float runFrameRate=8f;
    private float jumpFrameRate=6f;
    
    private float throwForceX=6f;
    private float throwForceY=6f;
    
  // holders ---
  
    public static Character instance;
  
    private Collider2D characterCollider;
    private Collider2D groundCollider;
    private Collider2D triggerCollider;
    
    [HideInInspector]
    public Rigidbody2D body;
    
    private List<Sprite> idleFrames;
    private List<Sprite> runFrames;
    private List<Sprite> jumpFrames;
    
    private GameObject graphics;
    private SpriteRenderer spriteRenderer;
    
    private Collider2D[] allColliders;
    
    private ScoreCounter goodCounter;
    private ScoreCounter badCounter;
    
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
    
    private bool isDead=false;

//---------------------------------------------------------
// EVENTS

	void Awake() {
  
    instance=this;

  //---

    MonoBehaviour[] children=GetComponentsInChildren<MonoBehaviour>();
    for(int i=0; i<children.Length; i++) {
    children[i].tag=tag;
    }
    
  //---
  
    characterCollider=transform.Find("CharacterCollider").GetComponent<Collider2D>();
    body=GetComponent<Rigidbody2D>();
    
    groundCollider=transform.Find("GroundCollider").GetComponent<Collider2D>();
    triggerCollider=transform.Find("TriggerCollider").GetComponent<Collider2D>();

    graphics=transform.Find("Graphics").gameObject;
    spriteRenderer=graphics.GetComponent<SpriteRenderer>();

  //---

    getAnimationSprites();
    if(idleFrames!=null) spriteRenderer.sprite=idleFrames[0];

    StartCoroutine(setAnimationFrame(1f/idleFrameRate, 0));

  //---
  
    allColliders=gameObject.GetComponentsInChildren<Collider2D>();
    
  //---
  
    if(GameObject.Find("GoodScore")!=null) goodCounter=GameObject.Find("GoodScore").gameObject.GetComponent<ScoreCounter>();
	  if(GameObject.Find("BadScore")!=null) badCounter=GameObject.Find("BadScore").gameObject.GetComponent<ScoreCounter>();
    
	}
  
//------------
	
	void Update() {
   
    if(isPlayer && WorldState.allowUserInput) {
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
    
  //---
  
    if(debugRun) {
    animationState=AnimationState.RUN;
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

  public void moveToPosition(Vector2 pos) {
  
    transform.position=pos;
  
  }
  
//------------

  public void setScale() {
  
    float w=(float) spriteSheet.width;
    float h=(float) spriteSheet.height;
    
    graphics.transform.localScale*=w/h/1.1f;  
  
  }

//------------

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
      
      SoundManager.instance.playVesalaJumpSound();

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

  public void stopMovement() {
  
    moveLeft(false);
    moveRight(false);
  
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
    
      if(item.value>0) {
      
        if(goodCounter!=null) goodCounter.changeScore(Mathf.Abs(item.value), 1);      
      
      } else if(item.value<0) {
      
        if(badCounter!=null) badCounter.changeScore(Mathf.Abs(item.value), -1); 

      }
    
    }
  
  }  
  
//------------

  public void Die() {
  
    if(!isDead) {
  
      isPlayer=false;
      stopMovement();
      
      if(LevelManager.instance!=null) {
      LevelManager.instance.showUI(false);
   
      } else {
      Debug.Log("dead");
      }
      
      SoundManager.instance.playVesalaLevelEndSound(false);
      
      isDead=true;
      
    }
  
  
  }
  
//---------------------------------------------------------
// PRIVATE SETTERS

  private void getAnimationSprites() {
  
    if(spriteSheet==null && characterId!=null) {
    
      if(isPlayer && GameState.playerCharacterId!=null) {
      characterId=GameState.playerCharacterId;
      }
    
      spriteSheet=Resources.Load<Texture2D>("Platformer/Characters/Still/"+characterId);
      if(spriteSheet!=null) {
      
        getAnimationSprites_1();
        setScale();
        isAnimated=false;
        return;
      
      }
    
      spriteSheet=Resources.Load<Texture2D>("Platformer/Characters/Animated/4_frames/"+characterId);
      if(spriteSheet!=null) {
      
        getAnimationSprites_4();
        setScale();
        isAnimated=true;
        return;
      
      }
      
      spriteSheet=Resources.Load<Texture2D>("Platformer/Characters/Animated/6_frames/"+characterId);
      if(spriteSheet!=null) {
      
        getAnimationSprites_6();
        setScale();
        isAnimated=true;
        return;
      
      }      
      
      if(spriteSheet==null) {
      isAnimated=false;
      }
      
    }  

  }

//------------  

  private void getAnimationSprites_1() {

    int fullW=spriteSheet.width;
    int fullH=spriteSheet.height;    

    idleFrames=new List<Sprite>();
    idleFrames[0]=Sprite.Create(spriteSheet, new Rect(0f, 0f, fullW, fullH), new Vector2(0.5f, 0f));

  }
  
//------------

  private void getAnimationSprites_4() {
  
    int fullW=spriteSheet.width;
    int fullH=spriteSheet.height;
    
    float w=fullW/4;
    float h=fullH/2f;    
         
    Sprite s;
  
  //---
  
    idleFrames=new List<Sprite>();
    s=Sprite.Create(spriteSheet, new Rect(0f, h, w, h), new Vector2(0.5f, 0f));
    idleFrames.Add(s);

    jumpFrames=new List<Sprite>();
    s=Sprite.Create(spriteSheet, new Rect(w, h, w, h), new Vector2(0.5f, 0f));
    jumpFrames.Add(s);

    runFrames=new List<Sprite>();
    for(int i=0; i<4; i++) {
  
      s=Sprite.Create(spriteSheet, new Rect(i*w, 0f, w, h), new Vector2(0.5f, 0f));
      runFrames.Add(s);
    
    }    
    
  }

//------------
  
  private void getAnimationSprites_6() {
  
    int fullW=spriteSheet.width;
    int fullH=spriteSheet.height;
    
    float w=fullW/6;
    float h=fullH/2f;    
         
    Sprite s;
  
  //---
  
    idleFrames=new List<Sprite>();
    s=Sprite.Create(spriteSheet, new Rect(0f, h, w, h), new Vector2(0.5f, 0f));
    idleFrames.Add(s);

    jumpFrames=new List<Sprite>();
    
    s=Sprite.Create(spriteSheet, new Rect(w, h, w, h), new Vector2(0.5f, 0f));
    jumpFrames.Add(s);
    
    s=Sprite.Create(spriteSheet, new Rect(w*2f, h, w, h), new Vector2(0.5f, 0f));
    jumpFrames.Add(s);

    runFrames=new List<Sprite>();
    for(int i=0; i<6; i++) {
  
      s=Sprite.Create(spriteSheet, new Rect(i*w, 0f, w, h), new Vector2(0.5f, 0f));
      runFrames.Add(s);
    
    }      
  
  }  
  
//------------  

  private IEnumerator setAnimationFrame(float wait, int frameNum=0) {
  
    yield return new WaitForSeconds(wait);

    switch(animationState) {
    
      case AnimationState.IDLE:

        if(idleFrames!=null) {

          frameNum= idleFrames.Count> 1 ? Random.Range(0, idleFrames.Count) : 0 ;
          spriteRenderer.sprite=idleFrames[frameNum];
          StartCoroutine(setAnimationFrame(1f/idleFrameRate));
          
        }
        
      break;
      
      case AnimationState.RUN:
      
        if(runFrames!=null) {
        
          spriteRenderer.sprite=runFrames[frameNum];
        
          frameNum++;
          if(frameNum>=runFrames.Count) frameNum=0;   
          StartCoroutine(setAnimationFrame(1f/runFrameRate, frameNum));      
          
          if(frameNum==0) {
          SoundManager.instance.playVesalaFootStepSound();
          }          

        }

      break;
      
      case AnimationState.JUMP:
      
        if(jumpFrames!=null) {
        
          if(jumpFrames.Count>1) {
            
            if(body.velocity.y>0f) spriteRenderer.sprite=jumpFrames[0];
            if(body.velocity.y<=0f) spriteRenderer.sprite=jumpFrames[1];        
          
          } else {
          spriteRenderer.sprite=jumpFrames[0];
          }
        
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
    
    Platform p=g.GetComponent<Platform>();
    if(p!=null) {

      if(p.isDeadly) {
      Die();
      }
      
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
  
    if(collision.collider.tag=="NotGround") {
    return false;
    }
  
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

    if(collider.tag=="NotGround") {
    return false;
    }  
         
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
