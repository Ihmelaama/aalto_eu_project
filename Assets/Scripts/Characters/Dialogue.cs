using UnityEngine;
using System.Xml;
using System.Collections;
using System.Collections.Generic;

public class Dialogue {

//---------------------------------------------------
// VARIABLES

  // settings ---
  
    public string dialogueFile=null;
    
  // holders ---
    
    private List<DialogueItem> defaultDialogueItems=new List<DialogueItem>();  
    private Dictionary<string, DialogueItem> otherDialogueItems=new Dictionary<string, DialogueItem>();
    
//---------------------------------------------------
// START

	public Dialogue(string dialogueFile) {
  
    if(dialogueFile!=null) {
    
    // load xml ---
    
      XmlDocument xml=new XmlDocument();
      xml.Load(Application.dataPath+"/Dialogue/XML/"+dialogueFile+".xml");
    
    // prepare stuff ---
    
      string name, text, gotoItemName;
      XmlAttributeCollection attributes;
      XmlAttribute attribute;
      DialogueItem dialogueItem;
      
    // get all dialogue items ---
    
      XmlNodeList itemList=xml.GetElementsByTagName("item");
      foreach(XmlNode item in itemList) {
      
        attributes=item.Attributes;
        attribute=attributes["name"];
        
        dialogueItem=new DialogueItem();
        dialogueItem.lines=new List<string>();
        dialogueItem.replies=new List<string>();
        dialogueItem.gotoItems=new List<string>();
        
      // add dialogue item to default list or other list ---

        if(attribute==null) {
        
          defaultDialogueItems.Add(dialogueItem); 
         
        } else {
        
          dialogueItem.name=attribute.InnerText;
          otherDialogueItems.Add(attribute.InnerText, dialogueItem);
        
        }      
        
      // parse item's content ---
      
        XmlNodeList nodes=item.ChildNodes;
        foreach(XmlNode node in nodes) {
        
          name=node.Name;   
          
          text=node.InnerText;      
          text=text.Replace("\\n", "\n"); 
          
          if(name=="line") {
          
            dialogueItem.lines.Add(text);

          } else if(name=="reply") {
          
            dialogueItem.replies.Add(text);
            
            attributes=node.Attributes;
            attribute=attributes["goto-item"];
            if(attribute!=null) dialogueItem.gotoItems.Add(attribute.InnerText);
          
          }

        }      
      
      }

    }

  }
  
//---------------------------------------------------
// PUBLIC GETTERS

  public DialogueItem getDefaultDialogue() {
  return defaultDialogueItems[Random.Range(0, defaultDialogueItems.Count)];
  }
 
//------------  
  
  public DialogueItem getDialogueByName(string name) {
  return otherDialogueItems[name];
  }  

//---------------------------------------------------
// HELPER CLASSES

  public struct DialogueItem {
  
    public string name;
    public List<string> lines;
    public List<string> replies;
    public List<string> gotoItems;
  
  }
  
}
