using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ActionMenu : MonoBehaviour {

    public GameObject actMenu;
    public Button dropButton;
    public Button useButton;
    public Button giveButton;
    public static Slot currentSlot = null;

    public Text useText;

    public static ActionMenu instance;


	void Start () {
        instance = this;
        actMenu.gameObject.SetActive(false);
	}
	
    public void DropMenu()
    {
        Debug.Log("Drop it like it's hot");
        currentSlot.DropItem();
        actMenu.gameObject.SetActive(false);
    }

    public void CloseMenu()
    {
        actMenu.gameObject.SetActive(false);
    }

    public void ShowMenu(Slot parent)
    {
        currentSlot = parent;
        transform.SetParent(parent.transform);
        actMenu.gameObject.SetActive(true);
        actMenu.transform.SetAsLastSibling();
        actMenu.GetComponent<RectTransform>().anchoredPosition = new Vector3(-120f,0f,0f);
       

    }
	
}
