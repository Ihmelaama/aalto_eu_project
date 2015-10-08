using UnityEngine;
using System.Collections;

public class CharScript : MonoBehaviour {

	public Sprite character;
	public string charName;

	public string getCharName(){
		return charName;
	}

	public void setAsPlayerCharacter(){

		GameState.playerCharacterSprite=this.character;
		Application.LoadLevel(1);

	}

}
