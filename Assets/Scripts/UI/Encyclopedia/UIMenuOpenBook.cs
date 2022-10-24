using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIMenuOpenBook : UIMenuNodeGraph
{
    public TextMeshProUGUI selectedBookName;
    [SerializeField] TextMeshProUGUI Tab1;
    [SerializeField] TextMeshProUGUI Tab2;
    [SerializeField] TextMeshProUGUI Tab3;
    [SerializeField] TextMeshProUGUI Tab4;

   public void GetBookName(string name)
    {
        selectedBookName.text = name;
    }

}
