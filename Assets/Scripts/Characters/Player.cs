using UnityEngine;
using System.Collections;

public class Player : Character {

//---------------------------------------------------
// VARIABLES

  public static Player instance;

//---------------------------------------------------
// START

  protected override void Awake() {
  base.Awake();
  instance=this;
  }

  protected override void Start() {
  
    characterSprite=GameState.playerCharacterSprite;
    base.Start();

  }
  
  protected override void randomizeAnimationPosition() {
  // do nothing
  }  
  
//---------------------------------------------------
// EVENTS

  protected override void FixedUpdate() {
  base.FixedUpdate();  
  determination=1f;
  }  
  
//------------

  protected override void OnTriggerEnter(Collider c) {

    base.OnTriggerEnter(c);

    if(c.gameObject.tag=="Item") {
    
      Item item=c.gameObject.GetComponent<Item>();
      ItemManager.foundItem(item);

      // debug ---
      //changeLifeValue(0, 0.3f);
      //changeLifeValue(1, -0.3f);

    }

  }
  
//------------

  public override void lifeValueFull(int lifeValueNum) {

    //Debug.Log("Full of "+Constants.lifeValueNames[lifeValueNum]+".");

  }
  
//------------  
  
  public override void lifeValueEmpty(int lifeValueNum) {
  
    //Debug.Log(Constants.lifeValueNames[lifeValueNum]+" is gone.");
    
  }  

}
