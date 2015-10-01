using UnityEngine;
using System.Collections;

public class Mission : MonoBehaviour {

    
        public string name;
        public int amountItemsNeeded;
        public Item itemNeeded;

        public bool missionDone { get { return (amountItemsNeeded <= 0); } }


}
