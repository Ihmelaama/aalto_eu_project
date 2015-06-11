using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tree : MonoBehaviour {

//---------------------------------------------------
// VARIABLES

  // holders ---

    private List<Transform> bones;
    
  // state ---
  
    private float angle=0f;
    private float angularSpeed=0;

//---------------------------------------------------
// EVENTS

	void Start() {
 
    bones=getBones(transform);
    angularSpeed=Random.Range(1, 10f);

	}
	
//------------

	void Update() {
  
    angle+=angularSpeed;

    if(angle>360f) {
    angle=0f;
    }
    
    Vector3 rot=Vector3.zero;
    for(int i=0; i<bones.Count; i++) {
    
      rot=bones[i].rotation.eulerAngles;
      rot.z=Mathf.Sin(angle*Mathf.Deg2Rad)*((i+1)*5f)-90f;
      bones[i].rotation=Quaternion.Euler(rot);

    }    
	
	}
  
//---------------------------------------------------
// PRIVATE GETTERS

  private List<Transform> getBones(Transform tree) {

    List<Transform> bones=new List<Transform>();
    Transform[] kids=tree.GetComponentsInChildren<Transform>();

    for(int i=0; i<kids.Length; i++) {
    
      if(kids[i].name.IndexOf("Bone")>-1) {
      bones.Add(kids[i]);
      }

    }

  return bones;
  }  
  
}
