using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class RandomTextures : MonoBehaviour {

//---------------------------------------------------
// VARIABLES

  public const int CHARACTER=1;
  public const int SECONDARY_CHARACTER=2;
  public const int NATURE=3;
  public const int BUILDING=4;

  public static List<Sprite> characterTextures=new List<Sprite>();
  public static List<Sprite> secondaryCharacterTextures=new List<Sprite>();
  public static List<Sprite> usedCharacterTextures=new List<Sprite>();
  public static List<string> reservedCharacterTextures=new List<string>();

  public static List<Sprite> natureTextures=new List<Sprite>();
  public static List<Sprite> buildingTextures=new List<Sprite>();

//---------------------------------------------------
// START

	void Awake() {
  
  	if(GameState.playerCharacterSprite!=null) {
    
      // current selected player ---
      reservedCharacterTextures.Add(GameState.playerCharacterSprite.texture.name);
  	
      // mission specific ---
      
        // Prague: "Sweet friendship, fat friendship"
        reservedCharacterTextures.Add("FatGuy");
    
    }

    loadSprites(CHARACTER);
    loadSprites(SECONDARY_CHARACTER);
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
        if(usedCharacterTextures.Count==characterTextures.Count) textures=secondaryCharacterTextures;

      break;
      
      case SECONDARY_CHARACTER:
      textures=secondaryCharacterTextures;
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
      Sprite s=textures[rand];
      int i=0;
      
      while(usedCharacterTextures.Contains(s)) {
      rand=(int) Mathf.Floor(Random.Range(0f, textures.Count));
      s=textures[rand];
      if(i>1000) break;
      i++;
      }
      
      if(type==CHARACTER) usedCharacterTextures.Add(textures[rand]);
      
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
      path="Assets/Textures/"+GameState.currentWorldName+"/Characters/Resources";
      break;
      
      case SECONDARY_CHARACTER:
      path="Assets/Textures/"+GameState.currentWorldName+"/SecondaryCharacters/Resources";
      break;      
      
      case NATURE:
      path="Assets/Textures/"+GameState.currentWorldName+"/Environment/Nature/Resources/";
      break;
      
      case BUILDING:
      path="Assets/Textures/"+GameState.currentWorldName+"/Environment/Buildings/Resources/";
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
      
      case SECONDARY_CHARACTER:
      secondaryCharacterTextures=textures;
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
