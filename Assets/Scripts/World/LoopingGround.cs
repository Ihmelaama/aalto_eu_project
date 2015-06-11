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
  
    private const float margin=1f;
    
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
    
      Transform newTile=unplacedTiles[0];
      newTile.gameObject.SetActive(true);
      Collider newTileCollider=newTile.Find("WalkableArea").GetComponent<Collider>();
    
    // get closest corner in current collider ---

      Collider currentTileCollider=t.gameObject.GetComponent<Collider>();      
      Vector3 currentCenter=currentTileCollider.bounds.center;
      
      Vector3 closestCorner=getClosestCorner(
      t.gameObject.transform, 
      currentTileCollider.bounds, 
      player.transform.position
      );
      
    // get position for new tile ---
    
      Vector3 newTilePosition=Vector3.zero;
      string newTileName=null;
      Vector3 pos=Vector3.zero;
      Vector3 dif;
      float f;
      
      pos=screenPosToWorldPos(screenPoints[pointNum], transform.position.y);
      pos.x=getBestTileX(pos, closestCorner, currentTileCollider, newTileCollider);
      pos.z=getBestTileZ(pos, closestCorner, currentTileCollider, newTileCollider);
      newTilePosition=pos; 
      
      // debug
      switch(pointNum) {
      
        case TOP:
        newTileName="tile top "+debugNum;  
        break;   

        case TOPRIGHT: 
        newTileName="tile top right "+debugNum;  
        break; 

        case RIGHT: 
        newTileName="tile right "+debugNum;  
        break; 
        
        case BOTTOMRIGHT: 
        newTileName="tile bottom right "+debugNum;  
        break;

        case BOTTOM: 
        newTileName="tile bottom "+debugNum;
        break;  

        case BOTTOMLEFT: 
        newTileName="tile bottom left "+debugNum;
        break;    
        
        case LEFT: 
        newTileName="tile left "+debugNum;
        break;   
        
        case TOPLEFT: 
        newTileName="tile top left "+debugNum;
        break;        
      
      }             

    // position new tile ---
    
      if(newTileName!=null) {
      
        newTile.gameObject.SetActive(true);
        newTilePosition.y=newTile.parent.position.y;
        newTile.position=newTilePosition;
        
        unplacedTiles.Remove(newTile);
        placedTiles.Add(newTile);
        
        newTile.name=newTileName;
        debugNum++;
        
      } else {
      newTile.gameObject.SetActive(false);
      } 

    }

    /*
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
      //topLeftCorner.x+=currentTileCollider.bounds.extents.x;
      //topLeftCorner.z-=currentTileCollider.bounds.extents.z;
      
      topLeftCorner.x-=currentTileCollider.bounds.extents.x;
      topLeftCorner.z+=currentTileCollider.bounds.extents.z;

      //Vector3 playerOffset=topLeftCorner-player.transform.position;
      Vector3 playerOffset=player.transform.position-topLeftCorner;
      //playerOffset.z=Mathf.Abs(playerOffset.z);

      float difX=playerOffset.x/newTileCollider.bounds.size.x;
      float ratioX=Mathf.Floor(difX);

      float difZ=playerOffset.z/newTileCollider.bounds.size.z;
      float ratioZ=Mathf.Floor(difZ);
      
      //currentCorner.x=topLeftCorner.x-ratioX*newTileCollider.bounds.size.x;
      //currentCorner.z=topLeftCorner.z+ratioZ*newTileCollider.bounds.size.z;

      currentCorner.x=topLeftCorner.x+ratioX*newTileCollider.bounds.size.x;
      currentCorner.z=topLeftCorner.z+ratioZ*newTileCollider.bounds.size.z;
    
    // get new tile position ---

      Vector3 newTilePos=currentCenter;  
      bool b=false;

      switch(pointNum) {
          
        case TOP: 
        
          newTile.name="tile top "+debugNum;  
          newTilePos.x=currentCorner.x+newTileCollider.bounds.extents.x;
          newTilePos.z=currentCorner.z+newTileCollider.bounds.extents.z;
          b=true; 
        
        break;    
        
        case TOPRIGHT: 
        
          newTile.name="tile top right "+debugNum;    
          newTilePos.x=currentCorner.x+newTileCollider.bounds.extents.x*3;
          newTilePos.z=currentCorner.z+newTileCollider.bounds.extents.z;
          b=true; 
          
        break; 

        case RIGHT: 

          newTile.name="tile right "+debugNum;    
          newTilePos.x=currentCorner.x+newTileCollider.bounds.extents.x*3;
          newTilePos.z=currentCorner.z-newTileCollider.bounds.extents.z;          
          b=true; 
          
        break; 
        
        case BOTTOMRIGHT: 

          newTile.name="tile bottom right "+debugNum;   
          newTilePos.x=currentCorner.x+newTileCollider.bounds.extents.x*3;
          newTilePos.z=currentCorner.z-newTileCollider.bounds.extents.z;        
          b=true; 
          
        break;   
        
        case BOTTOM: 
        
          newTile.name="tile bottom "+debugNum;    
          newTilePos.x=currentCorner.x+newTileCollider.bounds.extents.x;
          newTilePos.z=currentCorner.z-newTileCollider.bounds.extents.z;          
          b=true; 
          
        break;  
        
        case BOTTOMLEFT: 

          newTile.name="tile bottom left "+debugNum;   
          newTilePos.x=currentCorner.x-newTileCollider.bounds.extents.x;
          newTilePos.z=currentCorner.z-newTileCollider.bounds.extents.z;          
          b=true; 
          
        break;    
        
        case LEFT: 

          newTile.name="tile bottom left "+debugNum;   
          newTilePos.x=currentCorner.x-newTileCollider.bounds.extents.x;
          newTilePos.z=currentCorner.z-newTileCollider.bounds.extents.z;          
          b=true; 
          
        break;   
        
        case TOPLEFT: 

          newTile.name="tile top left "+debugNum;    
          newTilePos.x=currentCorner.x-newTileCollider.bounds.extents.x;
          newTilePos.z=currentCorner.z+newTileCollider.bounds.extents.z;
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
    */

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
  
//---------------------------------------------------
// HELPFUL

  private Vector3 getClosestCorner(Transform parentTransform, Bounds bounds, Transform target) {
  Vector3 pos=target.TransformPoint(target.position);
  return getClosestCorner(parentTransform, bounds, pos);
  }

  private Vector3 getClosestCorner(Transform parentTransform, Bounds bounds, Vector3 targetPosition) {
  
    Vector3 center=bounds.center;
    //Vector3 targetPos=targetPosition-center;
    Vector3 targetPos=targetPosition;  

    Vector3 closestCorner=Vector3.zero;
    Vector3 corner;
    float shortestMag;
    float mag;
    
  //---

  // bottom left ---
  
    corner=bounds.min;
    shortestMag=(corner-targetPos).magnitude;
    closestCorner=corner;

  // top right ---

    corner=bounds.max;
    mag=(corner-targetPos).magnitude;
    
    if(mag<shortestMag) {
    closestCorner=corner;
    shortestMag=mag;
    }
    
  // bottom right ---
  
    corner=bounds.min;
    corner.x=bounds.max.x;
    mag=(corner-targetPos).magnitude;
    
    if(mag<shortestMag) {
    closestCorner=corner;
    shortestMag=mag;
    }    
    
  // top left ---  
  
    corner=bounds.max;
    corner.x=bounds.min.x;
    mag=(corner-targetPos).magnitude;
    
    if(mag<shortestMag) {
    closestCorner=corner;
    shortestMag=mag;
    }

  //---

    return closestCorner;
  
  }  
  
//------------

  private Vector3 screenPosToWorldPos(Vector3 screenPos, float worldY) {
  
    Camera camera=Camera.main;
  
    Vector3 pos1=Vector3.zero;
    pos1.x=screenPos.x/Screen.width;
    pos1.y=screenPos.y/Screen.height;
    pos1.z=0f;
    pos1=camera.ViewportToWorldPoint(pos1);
    
    worldY=worldY-pos1.y;

    Vector3 pos2=Vector3.zero;
    pos2.x=screenPos.x/Screen.width;
    pos2.y=screenPos.y/Screen.height;
    pos2.z=5f;
    pos2=camera.ViewportToWorldPoint(pos2);
      
    Vector3 dif=pos2-pos1;
    dif*=worldY/dif.y;
    
    pos1+=dif;
    
    Vector3 debug=new Vector3(0f, 1f, 0f);
    //Debug.DrawLine(pos1, pos1+debug, Color.white);
    
  return pos1;
  }
  
//------------

  private float getBestTileX(Vector3 newTilePosition, Vector3 closestCorner, Collider currentTileCollider, Collider newTileCollider) {
  
    float dif=newTilePosition.x-closestCorner.x;
    float dir=dif/Mathf.Abs(dif);
    
    float offset=Mathf.Floor(Mathf.Abs(dif/newTileCollider.bounds.size.x));
    offset=offset*newTileCollider.bounds.size.x;
    offset*=dir;
    
    offset+=dir*newTileCollider.bounds.extents.x;
  
  return closestCorner.x+offset;
  }
  
//------------

  private float getBestTileZ(Vector3 newTilePosition, Vector3 closestCorner, Collider currentTileCollider, Collider newTileCollider) {
  
    float dif=newTilePosition.z-closestCorner.z;
    float dir=dif/Mathf.Abs(dif);
    
    float offset=Mathf.Floor(Mathf.Abs(dif/newTileCollider.bounds.size.z));
    offset=offset*newTileCollider.bounds.size.z;
    offset*=dir;
    
    offset+=dir*newTileCollider.bounds.extents.z;
  
  return closestCorner.z+offset;
  }

}
