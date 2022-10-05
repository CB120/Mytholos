using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuMusicResetter : MonoBehaviour
{
    void Start()
    {
        //In every other Menu Scene the relevant controller updates the MenuMusicController's state. It defaults to MainMenu, but if the Player(s)
        //return to the Main Menu, there's no controller to reset it, so this does!
        MenuMusicController.ChangeMenuState(E_MenuState.MainMenu); 
        Destroy(gameObject);
    }
}
