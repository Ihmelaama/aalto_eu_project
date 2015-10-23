using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Character : MonoBehaviour {

//---------------------------------------------------
// VARIABLES

  // public settings ---
  
    public Sprite characterSprite=null;  
    public string characterSpriteName=null;

    public float minWalkSpeed=0.05f;
    public float maxWalkSpeed=0.1f;
    
    public float[] defaultLifeValues;
    
    public bool movable=true;
    
  // private settings ---
  
    private string characterIdString=null;
  
    private float minDistanceFromDestination=0.75f;
    private float stopDistanceFromDestination=0.25f;

    [System.NonSerialized]
    public float laziness=0.02f;   
         
    [System.NonSerialized]
    public float walkFriction=0.9f;
    
    [System.NonSerialized]
    public float slowDownRatio=0.8f;
    
    [System.NonSerialized]
    public float idleSpeed=0.005f;
    
    [System.NonSerialized]
    public float minDetermination=0.2f;    
    
    private float lifeValueLerpThreshold=0.05f;
    private float lifeValueLerpSpeed=0.1f;

  // holders ---
  
    private GameObject graphics;
    private Animator graphicsAnimator;
    private SpriteRenderer graphicsSpriteRenderer;
    private Dialogue dialogue;

  // state ---

    [System.NonSerialized]
    public Vector3 walkVector=Vector3.zero;

    [System.NonSerialized]
    public List<Vector3> destinations=new List<Vector3>();
    
    [System.NonSerialized]
    public bool allowNewDestinations=true;
                              
    [System.NonSerialized]
    public float determination=0f;
    
    [System.NonSerialized]
    public List<float> lifeValues=new List<float>();
    
    private List<float> lerpLifeValues=new List<float>();
    
//---------------------------------------------------
// START

  protected virtual void Awake() {

  // set initial life values for character ---

    string name;
    float val;
    
    for(int i=0; i<Constants.lifeValueNames.Count; i++) {

      if(defaultLifeValues==null || defaultLifeValues.Length<=i) {
      val=Constants.defaultLifeValues[i];
      } else {
      val=defaultLifeValues[i];
      }
      
      lifeValues.Add(val);
      lerpLifeValues.Add(val);

    }

  }

//------------

	
	protected virtual void Start() {
  
    getElements();
    setCharacterSprite();
    getCharacterId();
    
    randomizeAnimationPosition();
    
  }
  
//------------
  
  private void getElements() {
 
    graphics=transform.Find("Graphics").gameObject; 
    graphicsAnimator=graphics.GetComponent<Animator>();
    if(graphics!=null) graphicsSpriteRenderer=graphics.GetComponent<SpriteRenderer>();
    
  }
  
//---------------------------------------------------
// PUBLIC SETTERS

  protected virtual void randomizeAnimationPosition() {
  StartCoroutine(uglyAnimationPositionHack(0.1f, graphicsAnimator.speed));
  }
  
  IEnumerator uglyAnimationPositionHack(float delay, float originalSpeed) {
  graphicsAnimator.speed=UnityEngine.Random.Range(0f, 2000f);  
  yield return new WaitForSeconds(delay);
  graphicsAnimator.speed=originalSpeed; 
  }  

//------------

  public void moveTowards(Vector3 d) {
  
    if(movable && allowNewDestinations) {
    
      Vector3 dif=d-transform.position;
      d=transform.position+dif.normalized*(minDistanceFromDestination*2);
      addDestination(d, true);

    }
 
  }

//------------

  public void addDestination(Vector3 d, bool overrideOthers) {
    
    if(allowNewDestinations) {

    // add new destination if path is walkable ---
      
      GameObject obstacle=getObstacleOnPath(transform.position, d);

      if(obstacle==null) {
    
        if(overrideOthers) destinations.Clear();
        destinations.Add(d);
         
    // if path not walkable try to find a detour
    
      } else {

        // if destination is inside the obstacle      
        if(isPointInsideCollider(d, obstacle)) {

          Transform node=getClosestNavigationNode(d, obstacle);
          addDestination(node.position, overrideOthers);
        
        // if destination is on the other side of obstacle
        } else {
        
          List<Transform> nodes=getAllWalkableNavigationNodes(obstacle);
          List<Vector3> path=getShortestPathAroundObstacle(obstacle, nodes, d);
         
          if(path!=null) {
         
            if(overrideOthers) {
            
              destinations=path;
              destinations.Add(d);
  
            } else {
            destinations.InsertRange(destinations.Count-2, path);
            }
          
          }
          
        }

      }    
       
    }
  
  }

//------------

  public void clearDestination() {
  
    walkVector=Vector3.zero;
    destinations.Clear();
  
  }
    
//------------

  public void changeLifeValue(int valueNum, float change) {
  
    if(Mathf.Abs(change)<lifeValueLerpThreshold) {
    
      lifeValues[valueNum]+=change;
      lerpLifeValues[valueNum]=lifeValues[valueNum];
    
    } else {
    
      lerpLifeValues[valueNum]=lifeValues[valueNum]+change;

    }
  
  }  
  
//------------

  public void sayHello() {
  SoundManager.PlayCharacterHello(characterIdString);
  }

  public void sayHello(float delay) {
  SoundManager.PlayCharacterHello(characterIdString, delay);
  }
  
  public void sayYes() {
  SoundManager.PlayCharacterYes(characterIdString);
  }
  
  public void sayNo() {
  SoundManager.PlayCharacterNo(characterIdString);
  }
  
//---------------------------------------------------
// PUBLIC GETTERS  
  
  public bool isVisibleToPlayer() {
  return false;
  }

//---------------------------------------------------
// EVENTS  
  
	protected virtual void FixedUpdate() {
  
    moveToDestination();
    handleAnimations();
    handleLifeValues();
      
	}

//------------

  protected virtual void OnTriggerEnter(Collider c) {

    if(c.gameObject.tag=="NotWalkableArea") {
    }
    
  }
  
//------------

  protected virtual void OnTriggerStay(Collider c) {

    if(c.gameObject.tag=="NotWalkableArea") {
    }
  
  }   
  
//------------  

  protected virtual void OnTriggerExit(Collider c) {
  
    if(c.gameObject.tag=="NotWalkableArea") {
    }
  
  }  
  
//------------

  public virtual void finalDestinationReached() {
  destinations.Clear();
  allowNewDestinations=true;  
  }
  
//------------

  public virtual void lifeValueFull(int lifeValueNum) {
  }
  
//------------  
  
  public virtual void lifeValueEmpty(int lifeValueNum) {
  }
  
//---------------------------------------------------
// PRIVATE SETTERS

  private void setCharacterSprite() {

    Sprite s;
  
  // if characterSprite is defined in editor use that
  
    if(characterSprite!=null) {
    s=characterSprite as Sprite;

  // if only name is defined load that (a Texture2D by that name should be somewhere in a Resources folder)
   
    } else if(characterSpriteName.Length>0) {
    s=Resources.Load<Sprite>(characterSpriteName) as Sprite;

  // else get a random character texture from RandomTextures class
    
    } else {
    s=RandomTextures.getRandomCharacterSprite();
    }
    
  //---
  
    if(s!=null) {
    graphicsSpriteRenderer.sprite=s;
    }

  }
  
//------------

  private void getCharacterId() {
  
    if(graphicsSpriteRenderer.sprite!=null) {
    
      String s=graphicsSpriteRenderer.sprite.name;
      characterIdString=s;
    
    }
  
  }
  
//------------

  private void moveToDestination() {

  // if final destination not reached ---

    if(destinations!=null && destinations.Count>0 && determination>minDetermination) {
    
      Vector3 current=transform.position;
      
      Vector3 destination=destinations[0];
      destination.y=transform.position.y;
      
      Vector3 dif=destination-current;
      
    // get walk vector ---

      walkVector+=dif.normalized*laziness;            
      walkVector*=walkFriction;
      
      if(dif.magnitude<minDistanceFromDestination) {
      walkVector*=slowDownRatio;
      }
      
      if(dif.magnitude<stopDistanceFromDestination) {
      walkVector*=dif.magnitude/stopDistanceFromDestination;
      destinations.RemoveAt(0);      
      }
      
      if(walkVector.magnitude>maxWalkSpeed) {
      walkVector=walkVector.normalized*maxWalkSpeed;
      }      

    // set position ---
      
      walkVector*=determination;
      current+=walkVector;
      transform.position=current;
    
  // final destination reached ---
      
    } else {
    
      walkVector*=slowDownRatio;
      if(walkVector.magnitude<minWalkSpeed) {
      walkVector=Vector3.zero;
      }
    
      finalDestinationReached();
    
    }
  
  }  
  
//------------

  private void handleAnimations() {

    if(walkVector.magnitude>0f) {
    graphicsAnimator.SetFloat("speed", walkVector.magnitude);

    } else if(destinations.Count==0) {
    graphicsAnimator.SetFloat("speed", -1f);
    }
  
  }
  
//------------

  private void handleLifeValues() {
  
    for(int i=0; i<lifeValues.Count; i++) {
    
    // lerp to new values if needed ---
    
      if(lifeValues[i]!=lerpLifeValues[i]) {
      lifeValues[i]=Mathf.Lerp(lifeValues[i], lerpLifeValues[i], lifeValueLerpSpeed);
      
      } else {
      lerpLifeValues[i]=lifeValues[i];
      }
      
    // limit values ---
    
      if(lifeValues[i]>1f) lifeValues[i]=1f;
      if(lifeValues[i]<0f) lifeValues[i]=0f;

    // full of life ---

      if(lifeValues[i]==1f) {
      lifeValueFull(i);
      }
   
    // out of life ---
    
      if(lifeValues[i]==0f) {
      lifeValueEmpty(i);
      }
   
    }

  }

//---------------------------------------------------
// PRIVATE GETTERS  

//--------------------------------------------------------------------------
// get all walkable navigation nodes

  private List<Transform> getAllWalkableNavigationNodes(GameObject area) {
  
    int i;
  
  // get navigation nodes ---
  
    Transform[] kids=area.transform.parent.GetComponentsInChildren<Transform>();
    List<Transform> navigationNodes=new List<Transform>(); 
    
    Transform node;
    
    for(i=0; i<kids.Length; i++) {
    
      if(kids[i].name=="navigationNode") {
      
        node=kids[i];
        if(isPathWalkable(transform.position, node.position)) {
        navigationNodes.Add(node);
        }
      
      }
    
    }       
  
  return navigationNodes;
  }
  
//--------------------------------------------------------------------------
// get the navigation node that is closest to pos1 and pos2

  private Transform getClosestNavigationNode(Vector3 pos, GameObject area) {    
  return getClosestNavigationNode(pos, area, null, null);
  }
  
  private Transform getClosestNavigationNode(Vector3 pos, GameObject area, List<Transform> availableNodes, List<Transform> notTheseNodes) {
  
    int i;
  
  // get navigation nodes ---
  
    if(availableNodes==null) {

      availableNodes=new List<Transform>();
      Transform[] kids=area.transform.parent.GetComponentsInChildren<Transform>();
  
      for(i=0; i<kids.Length; i++) {
      if(kids[i].name=="navigationNode") availableNodes.Add(kids[i]);
      }  
    
    }
    
  // get closest node ---
  
    Transform closestNode=null;
    Vector3 dif=Vector3.zero;
    float dist=Mathf.Infinity;

    for(i=0; i<availableNodes.Count; i++) {
    
      if(notTheseNodes==null || !notTheseNodes.Contains(availableNodes[i])) {
    
        dif=availableNodes[i].position-pos;

        if(dif.magnitude<dist) {
        closestNode=availableNodes[i];
        dist=dif.magnitude;
        }
      
      }

    }
  
  // done ---
  
    return closestNode;
  
  }
  
//--------------------------------------------------------------------------
// test if point is inside collider

  private bool isPointInsideCollider(Vector3 point, GameObject area) {
  
    Collider c=area.GetComponent<Collider>();
    RaycastHit h;

    point.y+=10f;
    Ray ray=new Ray(point, Vector3.down);
    
    if(c.Raycast(ray, out h, Mathf.Infinity)) {
    return true;
    }

  return false;
  }

//--------------------------------------------------------------------------
// test if path intersects not walkable areas 

  private bool isPathWalkable(Vector3 A, Vector3 B) {
  
      RaycastHit hitInfo;
      bool hit=Physics.Linecast(A, B, out hitInfo);
      
      if(!hit || hitInfo.transform.gameObject.tag!="NotWalkableArea") {
      return true;
      }  

  return false;
  }
  
//--------------------------------------------------------------------------
// test if path intersects with obstacles and returns it as a gameobject

  private GameObject getObstacleOnPath(Vector3 A, Vector3 B) {
  
      RaycastHit hitInfo;
      bool hit=Physics.Linecast(A, B, out hitInfo);
      
      if(hit && hitInfo.transform.gameObject.tag=="NotWalkableArea") {
      return hitInfo.transform.gameObject;
      }  

  return null;
  }

//--------------------------------------------------------------------------
// get shortest path
  
  private List<Vector3> getShortestPathAroundObstacle(GameObject obstacle, List<Transform> startNodes, Vector3 targetDestination) {
    
    List<Vector3> currentPos=new List<Vector3>();  
    List<List<Vector3>> paths=new List<List<Vector3>>();
    List<int> possiblePaths=new List<int>();
    List<int> failedPaths=new List<int>();    
    List<List<Transform>> usedNodes=new List<List<Transform>>();    
    
    int i, j, num;
    float d;
    List<Vector3> v;
    
  // get navigation nodes ---
  
    Transform[] kids=obstacle.transform.parent.GetComponentsInChildren<Transform>();
    List<Transform> navigationNodes=new List<Transform>();

    for(i=0; i<kids.Length; i++) {
    if(kids[i].name=="navigationNode") navigationNodes.Add(kids[i]);
    }

  // prepare things ---
      
    for(i=0; i<startNodes.Count; i++) {
    
      paths.Add(new List<Vector3>());
      currentPos.Add(startNodes[i].position);
      usedNodes.Add(new List<Transform>());

    }

  // get possible paths ---

    Transform node;
    bool b=true;
    i=0;
    
    while(b) {
    
    // get next nodes ---
    
      for(i=0; i<startNodes.Count; i++) {
      
        if(!possiblePaths.Contains(i) && !failedPaths.Contains(i)) {
      
          node=getClosestNavigationNode(currentPos[i], obstacle, navigationNodes, usedNodes[i]);
          usedNodes[i].Add(node);
  
        // add node to path if reachable ---
          
          if(isPathWalkable(currentPos[i], node.position)) {
          
            paths[i].Add(node.position);
            currentPos[i]=node.position;
          
            // if target destination reachable, path is ready
            if(isPathWalkable(currentPos[i], targetDestination)) {
            possiblePaths.Add(i);
            }
  
          }
        
        // check if available nodes left ---
          
          if(usedNodes[i].Count>=navigationNodes.Count) {
          failedPaths.Add(i);
          }
        
        }
        
      }
      
    // stop the loop if all paths are checked ---
    
      if(possiblePaths.Count+failedPaths.Count>=startNodes.Count) {
      b=false;
      }
  
    // stop the loop if taking way too long ---

      i++;
      if(i>1000) {
      b=false;
      }

    }


  // get shortest path ---
  
    List<float> distances=new List<float>();
  
    for(i=0; i<possiblePaths.Count; i++) {
    
      num=possiblePaths[i];
      v=paths[num];

      distances.Add(Vector3.Distance(transform.position, v[0]));

      for(j=1; j<=v.Count; j++) {
    
        if(j<v.Count-1) {
        distances[i]+=Vector3.Distance(v[j-1], v[j]);
        
        } else {
        distances[i]+=Vector3.Distance(v[j-1], targetDestination);
        }

      }
    
    }
    
    d=Mathf.Infinity;
    num=-1;
    
    for(i=0; i<distances.Count; i++) {
    
      if(distances[i]<d) {
      num=possiblePaths[i];
      d=distances[i];
      }
    
    }
    
    if(num>-1) {
    return paths[num];
    }

  return null;
  }

}
