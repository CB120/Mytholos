using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Serialization;
[CreateAssetMenu(fileName = "Untitled", menuName = "ScriptableObjects/UIBook", order = 1)]

public class SO_Book : ScriptableObject
{
    [Header("Book Name")]
    public string bookName;
    
    [Header("Tab Names")]
    public string[] tabs;
    public bool hasTabs;

    [Header("Plz dont use more than 2 of each field... plz ~ Christian")]
    public TabData[] tabData; 

    
}
