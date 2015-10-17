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
    
        publicMissionList=new List<Mission>();
        getMissionsFromXML();
    
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
        
        if(!currentMission.missionDone) {
        VisualFeedbackManager.instance.MissionProgressed();

        } else {
        VisualFeedbackManager.instance.MissionDone();        
        }
        
        MakeUiChanges(currentMission);
        
        checkIfAllMissionsComplete();
        
    }

    public static bool checkIfMission(Item item, ActionType action) {
    
        foreach(Mission mis in missions)
        {
            if(mis.itemNeeded == item.itemID)
            {
                if (mis.actionNeeded == action)
                {
                
                    if(!mis.missionDone) 
                    {
                    
                      changeMissionStatus(mis.name);
                      return true;
                    
                    }

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

        if(target!=null) {
        target.text = mission.missionDescription + "\n Found " +  (mission.targetAmount - mission.amountItemsNeeded) + " of " + mission.targetAmount;
        }
        
    }
    
    public static void checkIfAllMissionsComplete() {
    
        bool b=true;
    
        foreach(Mission mis in missions)
        {
        
          if(!mis.missionDone) {
          b=false;
          break;
          }
        
        }    
        
        if(b) {
        SceneManager.instance.gotoGameWin();
        }

    }
  
//---------------------------------------------------
// PRIVATE GETTERS    
    
  private void getMissionsFromXML() 
  {
  
  // load xml ---

    TextAsset textAsset=(TextAsset) Resources.Load("Missions/missions");
    XmlDocument xml=new XmlDocument();
    xml.LoadXml(textAsset.text);       
    
  // prepare stuff ---
  
    string name, text;
    XmlAttributeCollection attributes;
    XmlAttribute attribute;
    
  // get missions ---
  
    XmlNodeList items=xml.GetElementsByTagName("world");
    XmlNodeList missionsXML=null;
    
    foreach(XmlNode item in items) { 
    
      attributes=item.Attributes;
      attribute=attributes["number"];
      
      if(attribute!=null && int.Parse(attribute.InnerText)==GameState.currentWorld) {
      missionsXML=item.ChildNodes;
      break;
      }
               
    }      
    
  // parse missions ---
  
    if(missionsXML!=null) {

      foreach(XmlNode mission in missionsXML) {
      parseMissionFromXMLNode(mission); 
      }
          
    }

  }
  
//------------
  
  private void parseMissionFromXMLNode(XmlNode missionNode) 
  {
  
    Mission mission=new Mission();
    
    XmlNodeList items=missionNode.ChildNodes;
    foreach(XmlNode item in items) {

      switch(item.Name) {
      
        case "name":
        mission.name=item.InnerText;
        break;
        
        case "description":
        mission.missionDescription=item.InnerText;
        break;   

        case "action":
        mission.actionNeeded=(ActionType) int.Parse(item.InnerText);
        break; 
        
        case "amount":
        mission.amountItemsNeeded=int.Parse(item.InnerText);
        mission.targetAmount=mission.amountItemsNeeded;
        break;       
        
        case "itemID":
        mission.itemNeeded=int.Parse(item.InnerText);
        break;                 
      
      }
    
    }    

    publicMissionList.Add(mission);

  }

}


