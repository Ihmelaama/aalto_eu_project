using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Clock : MonoBehaviour {

//-----------------------------------------------------------------
// VARIABLES

  public int time=180;
  
  private Text text;
  
//-----------------------------------------------------------------
// EVENTS

	void Start() {
  
    text=GetComponent<Text>();

    writeText();
    StartCoroutine(timer(1f));
	
	}
  
//------------
	
	void Update() {
	
	}

//-----------------------------------------------------------------
// PRIVATE SETTERS

  IEnumerator timer(float wait) {
  
    yield return new WaitForSeconds(wait);
    
    time--;
    writeText();

    if(time<=0) {
    
      if(SceneManager.instance!=null) SceneManager.instance.gotoGameWin();
    
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
    
    text.text=minStr+":"+secStr;
  
  }
  
}
