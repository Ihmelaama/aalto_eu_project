using UnityEngine;
using System.Xml;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;      

public class MissionManager : MonoBehaviour {

    public List<Mission> publicMissionList;
    public static List<Mission> missions;

    public List<GameObject> publicMissionUIElements;
    public static List<GameObject> missionUIElements;
    

    public enum ActionType { use, give, find};


    void Start()
    {
    
        missions = new List<Mission>();
        foreach(Mission mis in publicMissionList)
        {
            missions.Add(mis);
        }

        missionUIElements = new List<GameObject>();
        int i = missions.Count -1;
        foreach(GameObject go in publicMissionUIElements)
        {
            missionUIElements.Add(go);
            go.GetComponent<Text>().text = missions[i].name;
            GameObject goChild = go.transform.GetChild(0).gameObject;
            goChild.GetComponent<Text>().text = missions[i].missionDescription;
            i--;
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
        }
        MakeUiChanges(currentMission);
    }

    public static bool checkIfMission(Item item, ActionType action) {
    
        foreach(Mission mis in missions)
        {
            if(mis.itemNeeded.itemID == item.itemID)
            {
                if (mis.actionNeeded == action)
                {
                    changeMissionStatus(mis.name);
                    return true;
                }
            }
        }

    return false;    
    }
    

    public static void MakeUiChanges(Mission mission)
    {
        Text target = null;
        foreach (GameObject go in missionUIElements)
        {
            if (go.GetComponent<Text>().text.Equals(mission.name))
            {
                target = go.transform.GetChild(0).gameObject.GetComponent<Text>();
            }
        }
        target.text = mission.missionDescription + "\n Found " +  (mission.targetAmount - mission.amountItemsNeeded) + " of " + mission.targetAmount;
    }

}


