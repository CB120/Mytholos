using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Serialization;
[CreateAssetMenu(fileName = "Untitled", menuName = "ScriptableObjects/UIBook", order = 1)]

public class SO_Book : ScriptableObject
{
    public string bookName;
    public string[] tabs;

    [Header("Plz dont use more than 2 images or 2 Descriptions... plz ~ Christian")]
    public TabData[] tabData; 

    [Header("Tab1")]
    public string tab1Name;
    public string tab1PageTitle;

    [Header("Tab2")]
    public string tab2Name;
    public string tab2PageTitle;

    [Header("Tab3")]
    public string tab3Name;
    public string tab3PageTitle;

    [Header("Tab4")]
    public string tab4Name;
    public string tab4PageTitle;
}
