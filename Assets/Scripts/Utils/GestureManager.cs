using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class GestureManager {

//----------------------------------------------
// COMMENTS & NOTES

// works best with ortographic camera

//----------------------------------------------
// VARIABLES

  // settings ---

    private int historySize=10;
    private float minDragDistance=0.1f;

  // holders ---
  
    public Camera camera;

  // state ---

    public static bool isTouched=false;
    public static bool isDragged=false;
    
    public static bool wasTouched=false;
    public static bool wasDragged=false;

  //---
    
    public static Vector2 firstTouch=Vector2.zero;
    public static Vector2 currentTouch=Vector2.zero;
    public static Vector2 lastTouch=Vector2.zero;

    public static Vector2 dragVector=Vector2.zero;
    public static Vector2 dragVelocity=Vector2.zero;
    
  //---  

    public static Vector2 firstTouchWorld=Vector2.zero;
    public static Vector2 currentTouchWorld=Vector2.zero;
    public static Vector2 lastTouchWorld=Vector2.zero;

    public static Vector2 dragVectorWorld=Vector2.zero;
    public static Vector2 dragVelocityWorld=Vector2.zero;
    
  //---
  
    public static List<Vector2> touchHistory;
    public static List<Vector2> dragHistory;
    
  //---
  
    private float differenceX, differenceY;
    
  //---
  
    [HideInInspector]
    public static bool isCreated=false;

//----------------------------------------------
// CONSTRUCTOR

  public GestureManager() {
  
    touchHistory=new List<Vector2>();
    touchHistory.Capacity=historySize;
    
    dragHistory=new List<Vector2>();
    dragHistory.Capacity=historySize;
    
    isCreated=true;

  }
  
//----------------------------------------------
// PUBLIC SETTERS  

  public void setCamera(Camera cam) {
  camera=cam;
  }
  
//------------
  
  public static void clearTouch() {

    lastTouch=currentTouch;
    firstTouch=Vector2.zero;      
    currentTouch=Vector2.zero;
    touchHistory.Clear();
    wasTouched=false;
  
    dragVector=Vector2.zero;    
    dragVelocity=Vector2.zero;      
    dragHistory.Clear();
    wasDragged=false;

  }
  
//----------------------------------------------
// PUBLIC GETTERS   

  public static Vector3 testTouch3D(Transform target) {
  return GestureManager.testTouch3D(target, null, null);
  }
  
  public static Vector3 testTouch3D(Transform target, string layer) {
  return GestureManager.testTouch3D(target, null, layer);
  }  
  
  public static Vector3 testTouch3D(Transform target, Camera c, string layer) {
  
    if(c==null) {
    c=Camera.main;
    }
      
    Vector2 t= currentTouch == Vector2.zero ? lastTouch : currentTouch ;
    Ray ray=c.ScreenPointToRay(t);
    RaycastHit hit;
    
    int layerMask=Physics.DefaultRaycastLayers;

    if(layer!=null) {
    int layerIndex=LayerMask.NameToLayer("User Input");
    layerMask=(1 << layerIndex);
    }

    if(Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask)) {

      if(hit.collider.transform==target) return hit.point;

    }     

  return Vector3.zero;  
  }
  
//----------------------------------------------
// EVENTS 

	public void Update() {

  // get touch position ---
        
#if UNITY_EDITOR || UNITY_WEBPLAYER  

    if(Input.GetMouseButton(0)) {
    
      isTouched=true;
      
      if(firstTouch==Vector2.zero) {
      
        firstTouch.x=Input.mousePosition.x;
        firstTouch.y=Input.mousePosition.y;

        lastTouch=firstTouch;

      }
      
      currentTouch.x=Input.mousePosition.x;
      currentTouch.y=Input.mousePosition.y;

      if(firstTouch!=Vector2.zero && touchHistory.Count>1) {
      
        differenceX=touchHistory[touchHistory.Count-1].x-touchHistory[touchHistory.Count-2].x;
        differenceY=touchHistory[touchHistory.Count-1].y-touchHistory[touchHistory.Count-2].y;
        
        if(Mathf.Abs(differenceX)>minDragDistance || Mathf.Abs(differenceY)>minDragDistance) {
        isDragged=true;
        }
        
        dragVelocity=calculateDragVelocity();
        dragVector=touchHistory[touchHistory.Count-1]-touchHistory[touchHistory.Count-2];
        
      }                          
    
    } else {

      if(wasTouched) {

        lastTouch=currentTouch;
        firstTouch=Vector2.zero;      
        currentTouch=Vector2.zero;
        touchHistory.Clear();

        wasTouched=false;
      
      }
      
      if(wasDragged) {
      
        dragVector=Vector2.zero;    
        dragVelocity=Vector2.zero;      
        dragHistory.Clear();
        wasDragged=false;
      
      }

      if(isTouched) {
      isTouched=false;
      wasTouched=true;
      }
      
      if(isDragged) {
      isDragged=false;
      wasDragged=true;
      }

    }
    
# else

    if(Input.touches.Length>0) {
    
      Vector2 touchPos=Input.touches[0].position;
    
      isTouched=true;
      
      if(firstTouch==Vector2.zero) {
      
        firstTouch.x=touchPos.x;
        firstTouch.y=touchPos.y;
        
        lastTouch=firstTouch;

      }
      
      currentTouch.x=touchPos.x;
      currentTouch.y=touchPos.y;

      if(firstTouch!=Vector2.zero && touchHistory.Count>1) {
   
        differenceX=touchHistory[touchHistory.Count-1].x-touchHistory[touchHistory.Count-2].x;
        differenceY=touchHistory[touchHistory.Count-1].y-touchHistory[touchHistory.Count-2].y;
        
        if(Mathf.Abs(differenceX)>minDragDistance || Mathf.Abs(differenceY)>minDragDistance) {
        isDragged=true;
        }
        
        dragVelocity=calculateDragVelocity();
        dragVector=touchHistory[touchHistory.Count-1]-touchHistory[touchHistory.Count-2];
        
      }                          
    
    } else {

      if(wasTouched) {

        lastTouch=currentTouch;
        firstTouch=Vector2.zero;      
        currentTouch=Vector2.zero;
        touchHistory.Clear();
        wasTouched=false;
      
      }
      
      if(wasDragged) {
      
        dragVector=Vector2.zero;    
        dragVelocity=Vector2.zero;      
        dragHistory.Clear();
        wasDragged=false;
      
      }

      if(isTouched) {
      isTouched=false;
      wasTouched=true;
      }
      
      if(isDragged) {
      isDragged=false;
      wasDragged=true;
      }

    }

# endif   

  // save touch position history ---   
  
    if(isTouched) {
    
      if(touchHistory.Count<historySize) {
      touchHistory.Add(currentTouch);
  
      } else {
      touchHistory.RemoveAt(0);
      touchHistory.Add(currentTouch);
      }
    
    }
    
  // save drag velocity history ---
 
    if(isDragged) {
 
      if(dragHistory.Count<historySize) {
      dragHistory.Add(dragVelocity);
      
      } else {
      dragHistory.RemoveAt(0);      
      dragHistory.Add(dragVelocity);
      }
    
    }
    
  // convert positions to world coordinates ---
  
    Camera cam= camera==null ? Camera.main : camera ;
  
    if(firstTouch!=Vector2.zero) {
    firstTouchWorld=getWorldPoint(cam, firstTouch);
    } else {
    firstTouchWorld=Vector2.zero;
    }
    
    if(currentTouch!=Vector2.zero) {
    currentTouchWorld=getWorldPoint(cam, currentTouch);
    } else {
    currentTouchWorld=Vector2.zero;
    }
    
    if(lastTouch!=Vector2.zero) {
    lastTouchWorld=getWorldPoint(cam, lastTouch);
    } else {
    lastTouchWorld=Vector2.zero;
    }
    
    if(dragVector!=Vector2.zero) {
    
      dragVectorWorld=cam.ScreenToWorldPoint(
      new Vector2(
      dragVector.x+Screen.width/2,
      dragVector.y+Screen.height/2
      ));
    
    } else {
    dragVectorWorld=Vector2.zero;
    }
    
    if(dragVelocity!=Vector2.zero) {
    
      dragVelocityWorld=cam.ScreenToWorldPoint(
      new Vector2(
      dragVelocity.x+Screen.width/2,
      dragVelocity.y+Screen.height/2
      ));    

    } else {
    dragVelocityWorld=Vector2.zero;
    }
    
  // stop propagation if UI system interacted with ---
  
    if(EventSystem.current!=null && EventSystem.current.currentSelectedGameObject!=null) {
    clearTouch();
    }      

  }
  
//----------------------------------------------
// HELPERS 

	private Vector2 calculateDragVelocity() {

    int iter=0;
    float x=0f;
    float y=0f;
    
    for(int i=1; i<touchHistory.Count; i++) {
      
      x+=touchHistory[i].x-touchHistory[i-1].x;
      y+=touchHistory[i].y-touchHistory[i-1].y;    
      iter++;
      
    }
    
    x/=iter;
    y/=iter;

  return new Vector2(x, y);
  }  
  
//------------
  
  private Vector3 getWorldPoint(Camera cam, Vector3 p) {
  
    if(cam.orthographic) {
    p=cam.ScreenToWorldPoint(p);
    
    } else {
    p.z=cam.nearClipPlane;
    p=cam.ScreenToWorldPoint(p);
    }
  
  return p;
  }
  
}
