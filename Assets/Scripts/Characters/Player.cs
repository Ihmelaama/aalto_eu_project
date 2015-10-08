using UnityEngine;
using System.Collections;

public class Player : Character {

//---------------------------------------------------
// VARIABLES

//---------------------------------------------------
// START

  protected override void Awake() {
  base.Awake();
  }

  protected override void Start() {
  
    characterSprite=GameState.playerCharacterSprite;
    base.Start();
  
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
    c.gameObject.GetComponent<Item>().PickUp();

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
