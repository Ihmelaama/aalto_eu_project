using UnityEngine;
using System.Collections;

public class Fabric3dSound : MonoBehaviour {

	public string eventName;
	public GameObject parentedGameObject;


	
	// Update is called once per frame
	void Start () {

		Fabric.EventManager.Instance.PostEvent (eventName, parentedGameObject);
	
	}
}
