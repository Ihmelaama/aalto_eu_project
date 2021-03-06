using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Platformer {
public class ScoreCounter : MonoBehaviour {

//--------------------------------------------------------
// VARIABLES

    public static ScoreCounter instance;
    
  // holders ---
  
    private Text scoreText;
  
  // state ---  
  
    private int score=0;

//--------------------------------------------------------
// EVENTS

	void Start() {

    instance=this;
    
    scoreText=GetComponent<Text>();
    scoreText.text=score.ToString();
	
	}
  
//------------
  
  public void changeScore(int change, int sign) {
  
    score+=change;
    string s=sign>0 ? "+" : "-" ;
    scoreText.text=s+""+score.ToString();
    
    GameState.score+=change*sign;
  
  }
  
}
}
