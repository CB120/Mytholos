using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIMenuOpenBook : UIMenuNodeGraph
{
    public SO_Book currentOpenBook;
    public TextMeshProUGUI selectedBookName;
    [SerializeField] TextMeshProUGUI[] tabNames;
    //UIMenuNode removedNode;

    public void GetCurrentBook(SO_Book book)
    {
        currentOpenBook = book;
    }

    private void OnEnable()
    {
        for (int i = 0; i < currentOpenBook.tabs.Length; i++)
        {
            nodes[i].gameObject.SetActive(true);
            tabNames[i].text = currentOpenBook.tabs[i];
            if (currentOpenBook.tabs.Length < 4)
            {
                for (int x = currentOpenBook.tabs.Length; x < tabNames.Length; x++)
                {
                    //nodes[x].gameObject.SetActive(false);
                    //removedNode = nodes[x];
                    //nodes.RemoveAt(x);
                }
            }
            selectedBookName.text = currentOpenBook.bookName;
            Debug.Log(selectedBookName.text);
        }

        for (int t = 0; t < currentOpenBook.tabData.Length; t++)
        {
            for(int d = 0; d < currentOpenBook.tabData[t].Descriptions.Length; d++)
            {
                nodes[t].GetComponent<UINodeTab>().descriptions[d].text = currentOpenBook.tabData[t].Descriptions[d];
            }

            for (int i = 0; i < currentOpenBook.tabData[t].Images.Length; i++)
            {
                nodes[t].GetComponent<UINodeTab>().images[i].sprite = currentOpenBook.tabData[t].Images[i];
            }

            for (int pt = 0; pt < currentOpenBook.tabData[t].PageNames.Length; pt++)
            {
                if(currentOpenBook.tabData[t].PageNames[pt] != null)
                nodes[t].GetComponent<UINodeTab>().pageTitles[pt].text = currentOpenBook.tabData[t].PageNames[pt];
            }
        }
    }


    private void OnDisable()
    {
        for (int i = 0; i < currentOpenBook.tabs.Length; i++)
        {
            tabNames[i].text = " ";
        }
            //TODO : 
            //if (nodes.Count < 4 && removedNode != null)
            //{
            //nodes.Add(removedNode);
            //}
        }

    public override void Navigate(UIMenuNode node, int playerNumber, UIMenuNode.Direction direction)
    {
        base.Navigate(node, playerNumber, direction);
        for (int i = 0; i < nodes.Count; i++)
        {
            if (nodes[i].GetComponent<UINodeTab>().informationSector.gameObject != null)
            {
                if (nodes[i] != playerCurrentNode[0])
                {

                    nodes[i].GetComponent<UINodeTab>().informationSector.gameObject.SetActive(false);
                }
                else
                {
                    playerCurrentNode[0].GetComponent<UINodeTab>().informationSector.gameObject.SetActive(true);
                }
            }
        }

    }
}
