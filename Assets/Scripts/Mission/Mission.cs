using UnityEngine;
using System.Collections;

public class Mission : MonoBehaviour {

    
    public string name;
    public string missionDescription;
    public int amountItemsNeeded;
    [HideInInspector]
    public int targetAmount;
    public Item itemNeeded;

    void Start()
    {
        targetAmount = amountItemsNeeded;
    }

    public bool missionDone { get { return (amountItemsNeeded <= 0); } }


}
