using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Clock : MonoBehaviour {

//-----------------------------------------------------------------
// VARIABLES

  public static Clock instance;
  
  public int defaultTime=120;

  // available time in seconds
  private int time=120;
  
  private Text text;
  
  private bool isRunning=false;
  
//-----------------------------------------------------------------
// EVENTS

	void Start() {
  
    instance=this;
  
    text=GetComponent<Text>();

    writeText();
    isRunning=true;
    StartCoroutine(timer(1f));
	
	}
  
//------------
	
	void Update() {
	
	}

//-----------------------------------------------------------------
// PUBLIC SETTERS

  public void stopTime() {
  isRunning=false;
  }
  
  public void startTime() {
  isRunning=true;
  }
  
//---------

  // set time in seconds
  public void setTime(int t) {
  time=t;
  }

//-----------------------------------------------------------------
// PRIVATE SETTERS

  IEnumerator timer(float wait) {
  
    yield return new WaitForSeconds(wait);
    
    if(isRunning) {
    time--;
    writeText();
    }

    if(time<=0) {
    
      if(SceneManager.instance!=null) SceneManager.instance.gotoGameLose();
    
    } else {
    StartCoroutine(timer(1f));
    }

  }
  
//-------------

  private void writeText() {
  
    float min=time/60f;
    float sec=(min-Mathf.Floor(min))*60f;
    
    string minStr=Mathf.Floor(min).ToString();
    string secStr=Mathf.Floor(sec).ToString();
    
    if(minStr.Length<2) minStr="0"+minStr;
    if(secStr.Length<2) secStr="0"+secStr;
    
    text.text=minStr+":"+secStr;
  
  }
  
}
