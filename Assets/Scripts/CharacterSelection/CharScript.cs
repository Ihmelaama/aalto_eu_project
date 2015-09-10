using UnityEngine;
using System.Collections;

public class CharScript : MonoBehaviour {

	public Sprite character;
	public string charName;

	public string getCharName(){
		return charName;
	}

	public void setAsPlayerCharacter(){
		//make the global player character this.character
	}

}
