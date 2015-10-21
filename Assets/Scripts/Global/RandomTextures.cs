using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class RandomTextures : MonoBehaviour {

//---------------------------------------------------
// VARIABLES

  public const int CHARACTER=1;
  public const int NATURE=2;
  public const int BUILDING=3;

  public static List<Sprite> characterTextures=new List<Sprite>();
  public static List<string> reservedCharacterTextures=new List<string>();

  public static List<Sprite> natureTextures=new List<Sprite>();
  public static List<Sprite> buildingTextures=new List<Sprite>();

//---------------------------------------------------
// START

	void Awake() {
  
    //reservedCharacterTextures.Add("human_base_marimekko");
	
  	if(GameState.playerCharacterSprite!=null) {
    
      // current selected player ---
      reservedCharacterTextures.Add(GameState.playerCharacterSprite.texture.name);
  	
      // mission specific
      
        // Prague: "Sweet friendship, fat friendship"
        reservedCharacterTextures.Add("FatGuy");
    
    }

    loadSprites(CHARACTER);
    loadSprites(NATURE);
    loadSprites(BUILDING);
  
  }
  
//---------------------------------------------------
// PUBLIC GETTERS

  public static Sprite getRandomCharacterSprite() {
  return getRandomSprite(CHARACTER);
  } 
  
//------------
  
  public static Sprite getRandomSprite(int type) {
  
    List<Sprite> textures=null;
    
    switch(type) {
    
      case CHARACTER:
      textures=characterTextures;
      break;
      
      case NATURE:
      textures=natureTextures;
      break;
      
      case BUILDING:
      textures=buildingTextures;
      break;
    
    }
    
  //---

    if(textures!=null && textures.Count>0) {
    
      int rand=(int) Mathf.Floor(Random.Range(0f, textures.Count));
      return textures[rand];

    }

  return null;
  }   
  
//---------------------------------------------------
// PRIVATE SETTERS  

  private void loadSprites(int type) {
  
    List<Sprite> textures=new List<Sprite>();
    string path=null;
  
  //---  

    switch(type) {
    
      case CHARACTER:
      path="Assets/Textures/Characters/Resources";
      break;
      
      case NATURE:
      path="Assets/Textures/Environment/Nature/Resources";
      break;
      
      case BUILDING:
      path="Assets/Textures/Environment/Buildings/Resources";
      break;
    
    }
  
  //---  
      
    if(path!=null) {  
      
      DirectoryInfo dir=new DirectoryInfo(path);
      FileInfo[] files=dir.GetFiles("*.png");
      
      foreach(FileInfo f in files) {
      
        string n=f.Name.Split('.')[0];
        Sprite s=Resources.Load<Sprite>(n) as Sprite;
        
        if(!reservedCharacterTextures.Contains(n)) {
        textures.Add(s);
        }
  
      }
    
    }
    
  //---    

    switch(type) {
    
      case CHARACTER:
      characterTextures=textures;
      break;
      
      case NATURE:
      natureTextures=textures;
      break;
      
      case BUILDING:
      buildingTextures=textures;
      break;
    
    }

  }
  
}
