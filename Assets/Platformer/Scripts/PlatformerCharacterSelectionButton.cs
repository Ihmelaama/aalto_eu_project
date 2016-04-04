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
    
      Texture2D spriteSheet=Resources.Load<Texture2D>("Platformer/Characters/Animated/4_frames/"+characterId);
      int type=1;
      
      if(spriteSheet==null) {
      spriteSheet=Resources.Load<Texture2D>("Platformer/Characters/Animated/6_frames/"+characterId);
      type=2;
      }
      
      if(spriteSheet==null) {
      spriteSheet=Resources.Load<Texture2D>("Platformer/Characters/Still/"+characterId);
      type=3;
      }
      
      float w=spriteSheet.width;
      float h=spriteSheet.height;
      
      float x=0f;
      float y=h/2f;
      
      switch(type) {
      
        case 1:
        w/=4f;
        h/=2f;
        break;
        
        case 2:
        w/=6f;
        h/=2f;
        break;
        
        case 3:
        y=0f;
        break;
        
      }

      image=GetComponent<Image>();
      image.sprite=Sprite.Create(spriteSheet, new Rect(x, y, w, h), new Vector2(0.5f, 0.5f));

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
