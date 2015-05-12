using UnityEngine;
using System.Collections;

public class DiscoverableArea : MonoBehaviour {

//---------------------------------------------------
// VARIABLES  

  // settings ---
  
    public float discoveryRadius=5f;

  // holders ---
  
    private Transform walkableArea;
    private Collider walkableAreaCollider;
    private GameObject player;  

  // state ---
  
    public bool isDiscovered=false;

//---------------------------------------------------
// START

	void Start() {
      
  // get stuff ---
  
    Collider[] colliders=GetComponentsInChildren<Collider>();

    for(int i=0; i<colliders.Length; i++) {
    
      if(colliders[i].gameObject.name=="WalkableArea") {
      walkableArea=colliders[i].gameObject.transform;
      walkableAreaCollider=colliders[i];
      break;
      }
    
    }
    
    player=GameObject.Find("Player");    

  // prepare for business ---
  
    if(!isDiscovered) {
    toggleActive(false);
    }    
  
	}
	
//---------------------------------------------------
// EVENTS

	void Update() {
  
    handleDiscovery();

	}

//------------

  private void handleDiscovery() {
  
    if(!isDiscovered && player!=null) {
    
      Vector3 p1=walkableAreaCollider.bounds.center;
      Vector3 p2=player.transform.position;
      float distance=Vector3.Distance(p1, p2);

      if(distance<discoveryRadius) {
  
      // set active ---        
      
        isDiscovered=true;
        toggleActive(true);

      // get direction of discovered area ---

        Vector3 dif=p1-p2;
        dif=dif.normalized;
      
        float d=Mathf.Abs(dif.x)-Mathf.Abs(dif.z);
        d=Mathf.Abs(d);

        if(d>0.75f) {

          dif.x=Mathf.Round(dif.x);
          dif.z=Mathf.Round(dif.z);

        } else {

          dif.x=dif.x/Mathf.Abs(dif.x);
          dif.z=dif.z/Mathf.Abs(dif.z);

        }
        
      // position next to current ground ---
      
        Transform currentGround=raycastWorldPoint(player.transform.position);
        positionNextTo(currentGround, dif, 0);

      }

    }
  
  }

//--------------------------------------------------
// PRIVATE SETTERS

  private void toggleActive(bool b) {
  
    foreach(Transform child in transform) {
    child.gameObject.SetActive(b);
    }
  
  }
  
//------------

  private void positionNextTo(Transform area, Vector3 dir, int num) {
  
    Collider collider=area.GetComponent<Collider>();
    
    if(collider!=null) {
    
      Vector3 pos=collider.bounds.center;
      pos.x+=dir.x*collider.bounds.extents.x;
      pos.z+=dir.z*collider.bounds.extents.z;

      pos.x+=dir.x*walkableAreaCollider.bounds.extents.x;
      pos.z+=dir.z*walkableAreaCollider.bounds.extents.z;      

      pos+=transform.position-walkableAreaCollider.bounds.center;
      transform.position=pos;

    }

  }
  
//--------------------------------------------------
// PRIVATE GETTERS

  private Transform raycastWorldPoint(Vector3 point) {
    
    RaycastHit hit;
    Ray ray=new Ray(point+Vector3.up*1f, Vector3.down); 

    LayerMask layerMask=(1 << 11);    
    if(Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask)) {
    return hit.transform;
    }
  
  return null;
  }
  

}
