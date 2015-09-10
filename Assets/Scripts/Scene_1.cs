using UnityEngine;
using System.Collections;

public class Scene_1 : MonoBehaviour {

	private GameObject playerCharacter;
	// Use this for initialization
	void Start () {
		playerCharacter = GameController.getPlayerCharacter ();
		Instantiate(playerCharacter, new Vector3(1,1,1), playerCharacter.transform.rotation);

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
