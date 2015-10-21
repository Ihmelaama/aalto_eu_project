using UnityEngine;
using System.Collections;

public class Helpful : MonoBehaviour {

  public static bool ArrayContainsInt(int[] arr, int item) {

    for(int i=0; i<arr.Length; i++) {
    if(arr[i]==item) return true;
    }
  
  return false;
  }
  
//------------  
  
  public static string getRandomString(string[] strings, string notThis) {

    int rand=Random.Range(0, strings.Length);
    while(strings[rand]==notThis) rand=Random.Range(0, strings.Length);
    return strings[rand];

  }

}
