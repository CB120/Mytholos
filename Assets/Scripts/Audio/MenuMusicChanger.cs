using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuMusicChanger : MonoBehaviour
{
    public E_MenuState newState = E_MenuState.MainMenu;

    void Start()
    {
        MenuMusicController.ChangeMenuState(newState);
        Destroy(gameObject);
    }
}
