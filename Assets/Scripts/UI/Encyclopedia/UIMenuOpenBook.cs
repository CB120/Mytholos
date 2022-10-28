using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIMenuOpenBook : UIMenuNodeGraph
{
    public SO_Book currentOpenBook;
    public TextMeshProUGUI selectedBookName;
    [SerializeField] TextMeshProUGUI[] tabNames;
    //UIMenuNode removedNode;
    [Header("Menu Graph References")]
    [SerializeField] GameObject Arrows;
    [SerializeField] TextMeshProUGUI leftPageNumber;
    [SerializeField] TextMeshProUGUI rightPageNumber;

    public void GetCurrentBook(SO_Book book)
    {
        currentOpenBook = book;
    }

    private void OnEnable()
    {
        if(nodes[3].adjacent[2] == nodes[4].gameObject){
            nodes[0].adjacent[3] = nodes[3].gameObject;
            nodes[3].adjacent[2] = nodes[0].gameObject;
        }
        if (currentOpenBook.hasTabs == false)
        {
            Arrows.SetActive(true);
            foreach (UINodeTab tab in nodes)
            {
                tab.gameObject.GetComponent<Image>().enabled = false;
                for (int i = 0; i < tab.transform.childCount; i++)
                {
                    Image childImage = tab.transform.GetChild(i).GetComponent<Image>();
                    if (childImage)
                        childImage.enabled = false;
                }
            }
            playerCursors[0].GetComponent<Image>().enabled = false; playerCursors[0].transform.GetChild(0).GetComponent<Image>().enabled = false;
            leftPageNumber.text = playerCurrentNode[0].GetComponent<UINodeTab>().leftPageNumber;
            rightPageNumber.text = playerCurrentNode[0].GetComponent<UINodeTab>().rightPageNumber;
            if(currentOpenBook.bookName == "Book Of Abilities")
            {
                Debug.Log("Detected");
                nodes[3].adjacent[2] = nodes[4].gameObject;
                nodes[0].adjacent[3] = nodes[4].gameObject;
            }
        } else {
            for (int i = 0; i < currentOpenBook.tabs.Length; i++)
            {
                nodes[i].gameObject.SetActive(true);
                nodes[i].gameObject.GetComponent<Image>().enabled = true;
                for (int k = 0; k < nodes[i].transform.childCount; k++)
                {
                    Image childImage = nodes[i].transform.GetChild(k).GetComponent<Image>();
                    if (childImage)
                        childImage.enabled = true;
                }
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
            }
            leftPageNumber.text = " ";
            rightPageNumber.text = " ";
            Arrows.SetActive(false);
            playerCursors[0].GetComponent<Image>().enabled = true; playerCursors[0].transform.GetChild(0).GetComponent<Image>().enabled = true;
        }
        selectedBookName.text = currentOpenBook.bookName;
        //Debug.Log(selectedBookName.text);
        for (int t = 0; t < currentOpenBook.tabData.Length; t++)
        {
            for(int d = 0; d < currentOpenBook.tabData[t].Descriptions.Length; d++)
            {
                nodes[t].GetComponent<UINodeTab>().descriptions[d].text = currentOpenBook.tabData[t].Descriptions[d];
            }

            if (currentOpenBook.tabData[t].Images.Length > 0)
            {
                for (int i = 0; i < currentOpenBook.tabData[t].Images.Length; i++)
                {
                    nodes[t].GetComponent<UINodeTab>().images[i].sprite = currentOpenBook.tabData[t].Images[i];
                }
            }
            else
            {
                for (int i = 0; i < currentOpenBook.tabData[t].Images.Length; i++)
                {
                    nodes[t].GetComponent<UINodeTab>().images[i].sprite = null;
                }
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

    public override void Navigate(UIMenuNode node, int playerNumber, UIMenuNode.Direction direction, bool isPlayerInput)
    {
        base.Navigate(node, playerNumber, direction, isPlayerInput);
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
                    if (currentOpenBook.hasTabs == false)
                    {
                        leftPageNumber.text = playerCurrentNode[0].GetComponent<UINodeTab>().leftPageNumber;
                        rightPageNumber.text = playerCurrentNode[0].GetComponent<UINodeTab>().rightPageNumber;
                    }
                }
            }
        }

    }
}
