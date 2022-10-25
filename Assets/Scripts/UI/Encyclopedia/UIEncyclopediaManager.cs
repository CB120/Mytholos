using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIEncyclopediaManager : MonoBehaviour
{
    [Header("Canvas References")]
    // Canvas Game Object References (For turning on and off stages)
    [SerializeField] GameObject openBookCanvas;
    [SerializeField] GameObject libraryCanvas;

    [Header("Menu Graph References")]
    // Menu Graph references
    PlayerParticipant player;
    [SerializeField] UIMenuNodeGraph LibraryGraph;
    [SerializeField] UIMenuNodeGraph openBookGraph;

    public UINodeBook playerCurrentBook;
    public SO_Book playerCurrentBookSO;
    public UINodeTab playerCurrentTab;


    private void Start()
    {
        player = FindObjectOfType<PlayerParticipant>();
        if(player != null)
        {
            player.currentMenuGraph = LibraryGraph;
        }
        if(openBookGraph == null)
        {
            openBookGraph = openBookCanvas.GetComponent<UIMenuOpenBook>();
        }
    }
    public void SetLibraryActive(bool active)
    {

       libraryCanvas.SetActive(active);
        if(active == true)
        {
            player.currentMenuGraph = LibraryGraph;
        }
    }

    public void SetBookCanvas(bool active)
    {
        openBookCanvas.SetActive(active);
        if (active == true)
        {
            player.currentMenuGraph = openBookGraph;
        }
    }

    public void ParseBookInformation(UINodeBook selectedBook, SO_Book bookSO)
    {
        playerCurrentBook = selectedBook;
        playerCurrentBookSO = bookSO;
        Debug.Log(bookSO + " " + selectedBook);

        // Bro why the fuck couldnt i get the function from the reference i already have? lol
        openBookCanvas.GetComponent<UIMenuOpenBook>().GetCurrentBook(bookSO);
        
    }
}
