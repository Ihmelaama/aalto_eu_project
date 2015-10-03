using UnityEngine;
using System.Collections;

public class SoundTestScript : MonoBehaviour {

	public string T;
	public string Y;
	public string U;
	public string Mouse;
	public Fabric.AudioComponent sound;


	void Update () {
		//For testing Fabric sound levels quickly

		if (Input.GetKeyDown (KeyCode.T)) {
			Fabric.EventManager.Instance.PostEvent(T);
		}
		if (Input.GetKeyDown (KeyCode.Y)) {
			Fabric.EventManager.Instance.PostEvent(Y);
		}
		if (Input.GetKeyDown (KeyCode.U)) {
			Fabric.EventManager.Instance.PostEvent(U);
		}
		if (Input.GetMouseButtonDown (0)) {
		
			sound.Stop();
	}
}
}
