using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {


	public static GameObject PlayerCharacter;

	public static GameObject getPlayerCharacter(){
			return PlayerCharacter;
	}

	public static void setPlayerCharacter(GameObject player){
		PlayerCharacter = player;
		Debug.Log("Player character is now "+PlayerCharacter.GetComponent<CharScript>().getCharName());
	}



}
