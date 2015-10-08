using UnityEngine;
using System.Collections;

public class Mission {

    
    public string name;
    public string missionDescription;
    public int amountItemsNeeded;
    [HideInInspector]
    public int targetAmount;
    public int itemNeeded;
    public MissionManager.ActionType actionNeeded;

    public bool missionDone { get { return (amountItemsNeeded <= 0); } }


}
