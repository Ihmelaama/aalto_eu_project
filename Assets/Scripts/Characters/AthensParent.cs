using UnityEngine;
using System.Collections;

public class AthensParent : NPC {

//------------------------------------------------------
// VARIABLES
  
  // settings ---
  
    public GameObject otherParent;

    private float minDistanceApart=1f;
    
  // holders ---
  
    private Player player;

//------------------------------------------------------
// EVENTS

	void Start() {
  
    base.Start();
    
    minDistanceApart=(otherParent.transform.position-transform.position).magnitude;
    
    minDistanceFromDestination=1.75f;
    stopDistanceFromDestination=1.25f;    
	
    player=GameObject.Find("Player").GetComponent<Player>();
  
	}
  
//------------
	
	void Update() {

    Vector3 dif=otherParent.transform.position-transform.position;

    if(dif.magnitude>minDistanceApart+1f) {
    moveTowards(otherParent.transform.position);
    }
    
    //Debug.Log(player.lifeValues[0]);

	}
  
}
