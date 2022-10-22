using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIEncyclopediaManager : MonoBehaviour
{
    [SerializeField] GameObject bookCanvas;
    [SerializeField] GameObject libraryCanvas;

    public void SetLibraryActive(bool active)
    {
       libraryCanvas.SetActive(active);
    }

    public void SetBookCanvas(bool active)
    {
        bookCanvas.SetActive(active);
    }
}
