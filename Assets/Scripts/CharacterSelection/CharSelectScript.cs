using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


public class CharSelectScript : MonoBehaviour {

	//publics
	public GameObject[] buttons;
    public GameObject[] buttons2;
    public GameObject[] buttons3;

    public Canvas Canvas;
	public Canvas Canvas2;
	public Canvas Canvas3;


	//private
	private GameObject button;



	// Use this for initialization
	void Start () {
        Debug.Log("called");
		createButtons();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	

	void createButtons(){
		//create a button
		for(int x = buttons.Length-1; x>=0; x--){
			button = Instantiate (buttons[x], new Vector3 (0, 0), Quaternion.identity) as GameObject;
			button.transform.SetParent(Canvas.transform);
		}

		for(int x = buttons2.Length-1; x>=0; x--){
			button = Instantiate (buttons2[x], new Vector3 (0, 0), Quaternion.identity) as GameObject;
			button.transform.SetParent(Canvas2.transform);
		}

		for(int x = buttons3.Length-1; x>=0; x--){
			button = Instantiate (buttons3[x], new Vector3 (0, 0), Quaternion.identity) as GameObject;
			button.transform.SetParent(Canvas3.transform);
		}

	}
	
}
