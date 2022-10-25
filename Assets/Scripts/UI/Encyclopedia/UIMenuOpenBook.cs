using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIMenuOpenBook : UIMenuNodeGraph
{
    public SO_Book currentOpenBook;
    public TextMeshProUGUI selectedBookName;
    [SerializeField] TextMeshProUGUI[] TabNames;
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
            TabNames[i].text = currentOpenBook.tabs[i];
            if (currentOpenBook.tabs.Length < 4)
            {
                for (int x = currentOpenBook.tabs.Length; x < TabNames.Length; x++)
                {
                    //nodes[x].gameObject.SetActive(false);
                    //removedNode = nodes[x];
                    //nodes.RemoveAt(x);
                }
            }
            selectedBookName.text = currentOpenBook.bookName;
        }

        /* for each tab (Tab Data), 
             for each description 

             for each image
         */

        for (int i = 0; i < currentOpenBook.tabData.Length; i++)
        {
                for(int x = 0; x < currentOpenBook.tabData[i].Descriptions.Length; x++)
                {
                    nodes[i].GetComponent<UINodeTab>().descriptions[x].text = currentOpenBook.tabData[i].Descriptions[x];
                }

                for (int y = 0; y < currentOpenBook.tabData[i].Images.Length; y++)
                {
                    if (currentOpenBook.tabData[i].Images[y] != null)
                    {
                    Debug.Log("This is getting called");
                        nodes[i].GetComponent<UINodeTab>().images[y].sprite = currentOpenBook.tabData[i].Images[y];
                    }
                }
        }
    }


    private void OnDisable()
    {
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
