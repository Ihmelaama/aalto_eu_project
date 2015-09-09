using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Constants {

//--------------------------------------
// GENERAL 

  public const int TALK=1;
  public const int USER_GIVES_ITEM=2;
  public const int USER_GETS_ITEM=3;

//--------------------------------------
// LIFE VALUES

  public static List<string> lifeValueNames=new List<string> {
  "health",
  "happiness",
  "wisdom"
  };
  
  public static List<float> defaultLifeValues=new List<float> {
  0.5f,
  0.5f,
  0.5f
  };  
   
}
