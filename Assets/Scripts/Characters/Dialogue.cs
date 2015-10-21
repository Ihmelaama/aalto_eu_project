using UnityEngine;
using System.Xml;
using System.Collections;
using System.Collections.Generic;

public class Dialogue {

//---------------------------------------------------
// VARIABLES

  // general dialogue files ---
  
    string[] randomDialogueFiles={
    "Dialogue/general_dialogue_01",
    "Dialogue/general_dialogue_02"
    };

  // settings ---
  
    public string dialogueFile=null;
    
  // holders ---

    private List<DialogueItem> defaultDialogueItems=new List<DialogueItem>();  
    private Dictionary<string, DialogueItem> otherDialogueItems=new Dictionary<string, DialogueItem>();
    
//---------------------------------------------------
// START

	public Dialogue(string dialogueFile) {
  
    if(dialogueFile==null || dialogueFile.Length==0) {
    dialogueFile=Helpful.getRandomString(randomDialogueFiles, null);
    }

    if(dialogueFile!=null) {
    
    // load xml ---

      TextAsset textAsset=(TextAsset) Resources.Load(dialogueFile);
      XmlDocument xml=new XmlDocument();
      xml.LoadXml(textAsset.text);    

    // prepare stuff ---
    
      string name, text;
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
// PUBLIC SETTERS  

//---------------------------------------------------
// HELPER CLASSES

  public struct DialogueItem {
  
    public string name;
    public List<string> lines;
    public List<string> replies;
    public List<string> gotoItems;
  
  }
  
}
