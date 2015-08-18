using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DebugStuff : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
  
  public void DebugButton() {
  
    GameObject text=GameObject.Find("UI/Canvas/DebugText");
    Text t=text.GetComponent<Text>();
    t.text+="\n done pushed button";
  
  }
  
}
