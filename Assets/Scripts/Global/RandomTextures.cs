using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class RandomTextures : MonoBehaviour {

//---------------------------------------------------
// VARIABLES

  public static List<Sprite> characterTextures=new List<Sprite>();
  public static List<string> reservedCharacterTextures=new List<string>();

//---------------------------------------------------
// START

	void Awake() {
  
    reservedCharacterTextures.Add("blue_star");
	
    loadCharacterSprites();
  
  }
  
//---------------------------------------------------
// PUBLIC GETTERS

  public static Sprite getRandomCharacterSprite() {
  
    if(characterTextures!=null && characterTextures.Count>0) {
    
      int rand=(int) Mathf.Floor(Random.Range(0f, characterTextures.Count));
      return characterTextures[rand];

    }

  return null;
  } 
  
//---------------------------------------------------
// PRIVATE SETTERS  
  
  private void loadCharacterSprites() {
  
    characterTextures=new List<Sprite>();
  
    DirectoryInfo dir=new DirectoryInfo("Assets/Textures/Characters/Dynamic/Resources");
    FileInfo[] files=dir.GetFiles("*.png");
    
    foreach(FileInfo f in files) {
    
      string n=f.Name.Split('.')[0];
      Sprite s=Resources.Load<Sprite>(n) as Sprite;
      
      if(!reservedCharacterTextures.Contains(n)) {
      characterTextures.Add(s);
      }

    }
  
  }

}
