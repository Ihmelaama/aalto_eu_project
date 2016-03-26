using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Platformer {
public class PlatformerCharacterSelectionButton : MonoBehaviour {

//-----------------------------------------------------------------
// VARIABLES

  // settings ---
  
    public string characterId;
    public string characterName;
    
  // holders ---
  
    private Image image;
    private Button button;

//-----------------------------------------------------------------
// EVENTS

	void Start() {

    if(characterId!=null) {
    
      Texture2D spriteSheet=Resources.Load<Texture2D>("Platformer/Characters/Animated/"+characterId);
      if(spriteSheet==null) Resources.Load<Texture2D>("Platformer/Characters/Still/"+characterId);
      
      float w=spriteSheet.width;
      float h=spriteSheet.height;

      image=GetComponent<Image>();
      image.sprite=Sprite.Create(spriteSheet, new Rect(0, h-h/4f, w/4f, h/4f), new Vector2(0.5f, 0.5f));
  
    } else {
    
      Destroy(gameObject);
      return;
    
    }
    
  //---
  
    button=GetComponent<Button>();
    
    button.onClick.AddListener(setAsPlayerCharacter);
  
	}
	
//------------

	void Update() {
	
	}
  
//-----------------------------------------------------------------
// PUBLIC SETTERS  

	public void setAsPlayerCharacter(){

		GameState.playerCharacterId=characterId;
    CharSelectScript.instance.loadLevel();
    
    button.onClick.RemoveListener(setAsPlayerCharacter);

	}  
  
}
}
