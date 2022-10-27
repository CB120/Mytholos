using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[System.Serializable]
public class TabData
{
    // 0 will be left, 1 will be right
    [TextArea]
    public string[] Descriptions;
    public Sprite[] Images; 
    public string[] PageNames;
}
