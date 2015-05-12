using UnityEngine;
using System.Collections;

public class LookAtCamera : MonoBehaviour {

//---------------------------------------------------
// VARIABLES

  public Camera targetCamera;

//---------------------------------------------------
// START

	void Start() {
	if(targetCamera==null) targetCamera=Camera.main;
  }
  
//---------------------------------------------------
// EVENTS  

	void Update() {
  
    if(targetCamera!=null) {
    
      Quaternion q;
      Vector3 v;
    
      if(targetCamera.orthographic) {
      
        q=targetCamera.transform.rotation;
        v=q.eulerAngles;

      } else {
      
        q=Quaternion.LookRotation(transform.position-targetCamera.transform.position);
        v=q.eulerAngles;

      }
      
      //v.x-=90f;
      transform.rotation=Quaternion.Euler(v);    

    }
	
	}
  
}
