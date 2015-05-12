using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LoopingGround : MonoBehaviour {

//---------------------------------------------------
// VARIABLES

  // debug ---
  
    private int debugNum=1;

  // static ---

    private const int TOP=0;
    private const int TOPRIGHT=1;
    private const int RIGHT=2;
    private const int BOTTOMRIGHT=3;
    private const int BOTTOM=4;
    private const int BOTTOMLEFT=5;
    private const int LEFT=6;
    private const int TOPLEFT=7;  
    
  // definitions ---
  
    private const float margin=0.5f;
    
    private float tileVisibilityDistance=20f;
    
  // holders ---

    private GameObject player;
    private Vector3[] screenPoints=new Vector3[8];

    private List<Transform> placedTiles=new List<Transform>();
    private List<Transform> unplacedTiles=new List<Transform>();

//---------------------------------------------------
// START

	void Start() {
  
    Vector3 v;
  
  // get player ---
  
    player=GameObject.Find("Player");
    
  // get tiles ---
  
    Transform[] t=gameObject.GetComponentsInChildren<Transform>();
    for(int i=0; i<t.Length; i++) {
    
      if(t[i].name=="tile") {
      unplacedTiles.Add(t[i]);
      t[i].gameObject.SetActive(false);
      }
    
    }
    
  // get screen edges ---
    
    // top
    v=new Vector3(Screen.width/2f, Screen.height+margin, 0f);
    screenPoints[TOP]=v;
    
    // top right
    v=new Vector3(Screen.width+margin, Screen.height+margin, 0f);
    screenPoints[TOPRIGHT]=v;    
    
    // right
    v=new Vector3(Screen.width+margin, Screen.height/2f, 0f);
    screenPoints[RIGHT]=v;     

    // bottom right
    v=new Vector3(Screen.width+margin, -margin, 0f);
    screenPoints[BOTTOMRIGHT]=v; 
    
    // bottom
    v=new Vector3(Screen.width/2f, -margin, 0f);
    screenPoints[BOTTOM]=v;     
    
    // bottom left
    v=new Vector3(-margin, -margin, 0f);
    screenPoints[BOTTOMLEFT]=v;       
    
    // left
    v=new Vector3(-margin, Screen.height/2f, 0f);
    screenPoints[LEFT]=v;       

    // top left
    v=new Vector3(-margin, Screen.height+margin, 0f);
    screenPoints[TOPLEFT]=v;       

	}
  
//---------------------------------------------------
// EVENTS  
	
	void Update() {
  
    Vector3 point=Vector3.zero;
    int pointNum=-1;
    int i;
    List<int> points=new List<int>();
    
  // check if world needs new tiles ---
  
    for(i=0; i<screenPoints.Length; i++) {
    
      if(raycastScreenPoint(screenPoints[i])==null) {
      points.Add(i);
      }
    
    }
    
  // position new tiles if needed ---
    
    for(i=0; i<points.Count; i++) {
    positionNewTiles(points[i]);
    }
    
  // remove old tiles ---
  
    handlePlacedTiles();
    
	}
  
//---------------------------------------------------
// PRIVATE GETTERS

  private Transform raycastScreenPoint(Vector3 point) {
  return raycastScreenPoint(point, -1);
  }

  private Transform raycastScreenPoint(Vector3 point, int layerToTest) {

    RaycastHit hit;
    Ray ray=Camera.main.ScreenPointToRay(point);

    if(layerToTest>-1) {

      LayerMask layerMask=(1 << layerToTest);
      if(Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask)) {
      return hit.transform;
      } 
    
    } else {

      if(Physics.Raycast(ray, out hit, Mathf.Infinity)) {
      return hit.transform;
      } 

    }

  return null;
  }
  
//------------

  private Transform raycastWorldPoint(Vector3 point) {
  
    RaycastHit hit;
    Ray ray=new Ray(point+Vector3.up*1f, Vector3.down); 

    LayerMask layerMask=(1 << 11);    
    if(Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask)) {
    return hit.transform;
    }
  
  return null;
  }
  
//---------------------------------------------------
// PRIVATE SETTERS  
  
  private void positionNewTiles(int pointNum) {

    Transform t=raycastWorldPoint(player.transform.position);      

    if(t!=null && unplacedTiles.Count>0) {

    // prepare stuff ---

      Collider currentTileCollider=t.gameObject.GetComponent<Collider>();      
      Vector3 currentCenter=currentTileCollider.bounds.center;
      Vector3 currentCorner=currentCenter;
      
      Transform newTile=unplacedTiles[0];
      newTile.gameObject.SetActive(true);
      Collider newTileCollider=newTile.Find("WalkableArea").GetComponent<Collider>();

      Vector3 topLeftCorner=currentCenter;
      topLeftCorner.x+=currentTileCollider.bounds.extents.x;
      topLeftCorner.z-=currentTileCollider.bounds.extents.z;
      
      Vector3 playerOffset=topLeftCorner-player.transform.position;
      playerOffset.z=Mathf.Abs(playerOffset.z);

      float difX=playerOffset.x/newTileCollider.bounds.size.x;
      float ratioX=Mathf.Floor(difX);

      float difZ=playerOffset.z/newTileCollider.bounds.size.z;
      float ratioZ=Mathf.Floor(difZ);
      
      currentCorner.x=topLeftCorner.x-ratioX*newTileCollider.bounds.size.x;
      currentCorner.z=topLeftCorner.z+ratioZ*newTileCollider.bounds.size.z;
      
    // get new tile position ---

      Vector3 newTilePos=currentCenter;  
      bool b=false;

      switch(pointNum) {
          
        case TOP: 
        
          newTile.name="tile top "+debugNum;  
          newTilePos.x=currentCorner.x-newTileCollider.bounds.extents.x;
          newTilePos.z=currentCorner.z-newTileCollider.bounds.extents.z;
          b=true; 
        
        break;    
        
        case TOPRIGHT: 
        
          newTile.name="tile top right "+debugNum;    
          newTilePos.x=currentCorner.x-newTileCollider.bounds.extents.x*3;
          newTilePos.z=currentCorner.z-newTileCollider.bounds.extents.z;
          b=true; 
          
        break; 

        case RIGHT: 

          newTile.name="tile right "+debugNum;    
          newTilePos.x=currentCorner.x-newTileCollider.bounds.extents.x*3;
          newTilePos.z=currentCorner.z+newTileCollider.bounds.extents.z;          
          b=true; 
          
        break; 
        
        case BOTTOMRIGHT: 

          newTile.name="tile bottom right "+debugNum;   
          newTilePos.x=currentCorner.x-newTileCollider.bounds.extents.x*3;
          newTilePos.z=currentCorner.z+newTileCollider.bounds.extents.z*3;          
          b=true; 
          
        break;   
        
        case BOTTOM: 

          newTile.name="tile bottom "+debugNum;    
          newTilePos.x=currentCorner.x-newTileCollider.bounds.extents.x;
          newTilePos.z=currentCorner.z+newTileCollider.bounds.extents.z*3;          
          b=true; 
          
        break;  
        
        case BOTTOMLEFT: 

          newTile.name="tile bottom left "+debugNum;   
          newTilePos.x=currentCorner.x+newTileCollider.bounds.extents.x;
          newTilePos.z=currentCorner.z+newTileCollider.bounds.extents.z*3;          
          b=true; 
          
        break;    
        
        case LEFT: 

          newTile.name="tile bottom left "+debugNum;   
          newTilePos.x=currentCorner.x+newTileCollider.bounds.extents.x;
          newTilePos.z=currentCorner.z+newTileCollider.bounds.extents.z;          
          b=true; 
          
        break;   
        
        case TOPLEFT: 

          newTile.name="tile top left "+debugNum;    
          newTilePos.x=currentCorner.x+newTileCollider.bounds.extents.x;
          newTilePos.z=currentCorner.z-newTileCollider.bounds.extents.z;
          b=true; 
          
        break;                                               

      }
      
    // set new tile position ---      

      if(b) {
      
        newTile.gameObject.SetActive(true);
        newTilePos.y=newTile.parent.position.y;
        newTile.position=newTilePos;
        
        unplacedTiles.Remove(newTile);
        placedTiles.Add(newTile);
        
        debugNum++;
        
      } else {
      newTile.gameObject.SetActive(false);
      }

    }

  }
  
//------------

  private void handlePlacedTiles() {

    int i, j;
    bool b;

    Transform tile;
    Transform tileWalkableArea;
    
    for(i=0; i<placedTiles.Count; i++) {
    
      tile=placedTiles[i];
      tileWalkableArea=tile.Find("WalkableArea");

      b=false;
      for(j=0; j<screenPoints.Length; j++) {
      
        if(raycastScreenPoint(screenPoints[j])==tileWalkableArea) {
        b=true;
        break;
        }
      
      } 
      
      if(!b) {
      tile.gameObject.SetActive(false);
      tile.name="tile";
      unplacedTiles.Add(tile); 
      placedTiles.Remove(tile);      
      }     

    }

  }
  
}
