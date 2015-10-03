using UnityEngine;
using System.Collections;

public class LazyFollow : MonoBehaviour {

//---------------------------------------------------
// VARIABLES

  // settings ---

    public GameObject followTarget;
  
    public bool allowAxisX=true;
    public bool allowAxisY=true;
    public bool allowAxisZ=true;
    
    public bool needsUserInput=true;

    private float laziness=0.05f;
    
  // state ---
  
    private Vector3 initialOffset=Vector3.zero;
    
    [HideInInspector]
    public bool enabled=true;

//---------------------------------------------------
// START

	void Start() {
  
    if(followTarget!=null) {
    initialOffset=followTarget.transform.position-transform.position;
    }
	
	}
  
//---------------------------------------------------
// EVENTS  
	
	void Update() {
  
    if(enabled && followTarget!=null) {
    
      Vector3 dif=followTarget.transform.position-transform.position-initialOffset;
      Vector3 move=Vector3.zero;
    
      if(allowAxisX) {
      move.x=dif.x;
      }

      if(allowAxisY) {
      move.y=dif.y;
      }
      
      if(allowAxisZ) {
      move.z=dif.z;
      }
      
      transform.position+=move*laziness;
      
    }

	}

}
                           