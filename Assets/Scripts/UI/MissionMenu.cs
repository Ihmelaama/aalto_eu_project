using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MissionMenu : MonoBehaviour {

    public Button missionButton;
    public Image missionBG;
    bool missionVisible = true;


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
}
