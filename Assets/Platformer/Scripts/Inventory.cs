using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace Platformer {
public class Inventory : MonoBehaviour {

//---------------------------------------------------------
// VARIABLES

  public static Inventory instance;
  public GameObject itemButtonTemplate;
  
  private List<InventoryItem> items=new List<InventoryItem>();

//---------------------------------------------------------
// EVENTS

	void Start() {
  
    instance=this;
	
	}
  
//------------

	void Update() {
	
	}
  
//---------------------------------------------------------
// PUBLIC SETTERS

  public void addItem(int itemId, Sprite sprite, GameObject worldObject, Character owner) {
  
    GameObject g=Instantiate(itemButtonTemplate);
    g.transform.parent=transform;
    g.transform.localScale=Vector3.one;    
    g.SetActive(true);
    
    Button b=g.GetComponent<Button>();
    b.onClick.AddListener(() => {
    useItem(g);
    });
    
    Image image=g.transform.Find("Image").GetComponent<Image>();
    image.sprite=sprite;
    
    InventoryItem item=new InventoryItem();
    item.gameObject=g;
    item.button=b;
    item.worldObject=worldObject;
    item.owner=owner;
    items.Add(item);

  }  
  
//------------
  
  public void useItem(GameObject g) {
  
    InventoryItem item=null;
    int iter=-1;
    for(int i=0; i<items.Count; i++) {
    
      if(items[i].gameObject==g) {
      item=items[i];
      iter=i;
      break;
      }
    
    }

    if(item!=null) {

      item.owner.throwItem(item.worldObject);    

      Destroy(g);
      if(iter>=0) items.RemoveAt(iter);    
  
    }
  
  }
  
}
}

//---------------------------------------------------------
// HELPER CLASSES

  namespace Platformer {
  public class InventoryItem {
  
    public GameObject gameObject=null;
    public Button button=null;
    public GameObject worldObject=null;
    public Character owner=null;
  
  }  
  }
  

