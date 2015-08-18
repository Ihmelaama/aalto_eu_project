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
  base.Start();
  }
  
//---------------------------------------------------
// EVENTS

  protected override void Update() {
  base.Update();  
  determination=1f;
  }  

}
