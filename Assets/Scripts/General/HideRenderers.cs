using UnityEngine;
using System.Collections;

public class HideRenderers : MonoBehaviour {

  private Renderer[] renderers;

	void Awake() {
  
    renderers=gameObject.GetComponentsInChildren<Renderer>();
    
    for(int i=0; i<renderers.Length; i++) {
    renderers[i].enabled=false;
    }
	
	}
  
}
