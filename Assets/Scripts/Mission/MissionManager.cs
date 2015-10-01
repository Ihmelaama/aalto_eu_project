using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MissionManager : MonoBehaviour {

    public List<Mission> publicMissionList;
    public static List<Mission> missions;
    

    void Start()
    {
        missions = new List<Mission>();
        foreach(Mission mis in publicMissionList)
        {
            missions.Add(mis);
        }
    }

    public static void changeMissionStatus(string MissionName)
    {
        Mission currentMission = null;
        foreach (Mission mis in missions)
        {
            if (mis.name.Equals(MissionName)){
                currentMission = mis;
            }
        }
        currentMission.amountItemsNeeded--;
        if (currentMission.missionDone)
        {
            bool doneHere = false;
            foreach (Mission mis in missions)
            {
                doneHere = mis.missionDone;
            }
            if (doneHere)
            {
                //we are so done here
                Debug.Log("we are so done here");
            }
            //make UI changes
        }
    }

    public static void checkIfMission(Item item)
    {
        foreach(Mission mis in missions)
        {
            if(mis.itemNeeded.itemID == item.itemID)
            {
                changeMissionStatus(mis.name);
            }
        }
    }

}


