using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MissionMenu : MonoBehaviour {

    public Button missionButton;
    public Image missionBG;
    bool missionVisible = true;


    //easy references to the mission information
    public Text mission_1_name;
    public Text mission_1_desc;

    public Text mission_2_name;
    public Text mission_2_desc;

    public Text mission_3_name;
    public Text mission_3_desc;



    void Start()
    {
        missionBG.rectTransform.sizeDelta = new Vector2(missionBG.rectTransform.rect.width, Screen.height/1.5f);
        missionBG.rectTransform.localPosition = new Vector3(0, -missionBG.rectTransform.rect.height + 20);
    }


    public void showMission()
    {
        if (missionVisible)
        {
            missionBG.gameObject.SetActive(true);
            missionButton.gameObject.transform.localPosition += new Vector3(0,missionBG.rectTransform.localPosition.y - 15);
            missionVisible = false;
        }
        else
        {
            missionBG.gameObject.SetActive(false);
            missionButton.gameObject.transform.localPosition = new Vector3(0, 0);
            missionVisible = true;
        }
    }

    public void ChangeMissionName(string newName, int missionNumber)
    {
        switch (missionNumber)
        {
            case 1: mission_1_name.text = newName; break;
            case 2: mission_2_name.text = newName; break;
            case 3: mission_3_name.text = newName; break;
            default: Debug.Log("Check the mission number!"); break;
        }
    }

    public void ChangeMissionDesc(string newDesc, int missionNumber)
    {
        switch (missionNumber)
        {
            case 1: mission_1_desc.text = newDesc; break;
            case 2: mission_2_desc.text = newDesc; break;
            case 3: mission_3_desc.text = newDesc; break;
            default: Debug.Log("Check the mission number!"); break;
        }
    }
}
