using UnityEngine;
using System.Collections;

public class CharScript : MonoBehaviour {

	public Sprite character;
	public string charName;
  public bool tapped=false;

	public string getCharName(){
		return charName;
	}

	public void setAsPlayerCharacter(){

    if(!tapped) {
		GameState.playerCharacterSprite=this.character;
    CharSelectScript.instance.loadLevel();
    tapped=true;
    }

	}

}
