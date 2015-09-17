using UnityEngine;
using System.Collections;

public class TextureSetter : MonoBehaviour {

//------------------------------------------------------------
// VARIABLES

  // settings ---
  
    public Renderer targetRenderer;
    public string textureType;
    
  // holders ---
  
    private Sprite sprite;

//------------------------------------------------------------
// EVENTS

  void Start() {
  
    if(targetRenderer==null) {
    
      Renderer[] renderers=transform.GetComponentsInChildren<Renderer>();
      if(renderers.Length>0) targetRenderer=renderers[0];
    
    }
    
  //---
  
    if(targetRenderer!=null) {

      switch(textureType) {
      
        case "character":
        sprite=RandomTextures.getRandomSprite(RandomTextures.CHARACTER);
        break;
        
        case "nature":
        sprite=RandomTextures.getRandomSprite(RandomTextures.NATURE);
        break;
        
        case "building":
        sprite=RandomTextures.getRandomSprite(RandomTextures.BUILDING);
        break;
      
      }

      if(sprite!=null) {
      targetRenderer.materials[0].mainTexture=sprite.texture;
      }

    }
  
  }

//------------------------------------------------------------
// PUBLIC SETTERS

  public void setTexture() {
  }

}
