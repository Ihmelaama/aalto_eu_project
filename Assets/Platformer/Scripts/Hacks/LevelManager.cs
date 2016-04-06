using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Platformer {
public class LevelManager : MonoBehaviour {

//---------------------------------------------------
// VARIABLES

  public static LevelManager instance;
  public GameObject[] levels;
  public GameObject levelWinUI=null;
  
  private GameObject currentLevel=null;
  private List<int> usedLevels=new List<int>();
  
  public int defaultLevel=-1;
  
//---------------------------------------------------
// EVENTS  

	void Start() {
  
    instance=this;
    
    for(int i=0; i<levels.Length; i++) {
    levels[i].SetActive(false);
    }
    
    activateLevel(defaultLevel);
	
	}
  
//---------------------------------------------------
// PUBLIC SETTERS  

  public void showUI() {
  
    if(levelWinUI!=null) {
    levelWinUI.SetActive(true);
    }
    
    if(Clock.instance!=null) Clock.instance.stopTime();
  
  }
  
//------------
  
  public void activateLevel(int num) {
  
    int levelNum=0;
    
    if(num>-1) {
    levelNum=num;
    
    } else {
    
      if(usedLevels.Count==levels.Length) {
      Application.LoadLevel("GameWin");
      }
    
      levelNum=Random.Range(0, levels.Length);
    
      while(usedLevels.Contains(levelNum)) {
      levelNum=Random.Range(0, levels.Length);
      }

      usedLevels.Add(levelNum);

    }
   
    if(currentLevel!=null) currentLevel.SetActive(false);
    levels[levelNum].SetActive(true);
    currentLevel=levels[levelNum];
    
    Transform spawnPoint=currentLevel.transform.Find("PlayerSpawn");
    
    Character character=GameObject.Find("Player").GetComponent<Character>();
    character.moveToPosition(spawnPoint.position);

    if(CameraFollow.instance!=null) CameraFollow.instance.setToTarget();
    
    if(levelWinUI!=null) levelWinUI.SetActive(false);   
    
    if(Clock.instance!=null) {
    Clock.instance.setTime(Clock.instance.defaultTime);    
    Clock.instance.startTime();
    }
    
    WorldState.allowUserInput=true; 

  }

}
}
