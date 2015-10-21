using UnityEngine;
using System.Collections;

public class VisualFeedbackManager : MonoBehaviour {

//---------------------------------------------
// SETTINGS

  public static VisualFeedbackManager instance;

  public ParticleSystem missionProgressedParticlesPrefab;
  public ParticleSystem missionDoneParticlesPrefab;
  
  private GameObject player;

//---------------------------------------------
// PUBLIC SETTERS  

  void Awake() {
  
    instance=this;
  
    player=GameObject.Find("Player");
  
  }
  
  
//---------------------------------------------
// PUBLIC SETTERS  

  public void MissionProgressed() {

    if(missionProgressedParticlesPrefab!=null) {
    ParticleSystem p=Instantiate(missionProgressedParticlesPrefab) as ParticleSystem;
    parentToPlayer(p.transform);
    }

  }
  
//------------
  
  public void MissionDone() {

    if(missionDoneParticlesPrefab!=null) {
    ParticleSystem p=Instantiate(missionDoneParticlesPrefab) as ParticleSystem;
    parentToPlayer(p.transform);
    }

  }
  
//---------------------------------------------
// HELPERS

  private void parentToPlayer(Transform targetTransform) {
  
    Vector3 pos=player.transform.position;

    Vector3 dif=pos-Camera.main.transform.position;
    dif.Normalize();
    
    pos-=dif;
    pos.y+=1.5f;

    targetTransform.position=pos;
    
  }
  
}
