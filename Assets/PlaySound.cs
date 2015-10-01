using UnityEngine;
using System.Collections;

public class PlaySound : MonoBehaviour {


	// Update is called once per frame
	void Playsound (string Eventname) {

		Fabric.EventManager.Instance.PostEvent (Eventname);
	
	}
}
