using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuMusicResetter : MonoBehaviour
{
    // Properties
    [Header("Properties")]
    [Tooltip("s seconds | How long before the Music fades to Encyclopaedia when on the Main Menu?")]
    public float timeBeforeFade = 60f;

    void Start()
    {
        //In every other Menu Scene the relevant controller updates the MenuMusicController's state. It defaults to MainMenu, but if the Player(s)
        //return to the Main Menu, there's no controller to reset it, so this does!
        MenuMusicController.ChangeMenuState(E_MenuState.MainMenu);
        Invoke("FadeToEncyclopaedia", timeBeforeFade);
    }

    void FadeToEncyclopaedia()
    {
        MenuMusicController.ChangeMenuState(E_MenuState.Encyclopaedia);
        Destroy(gameObject);
    }
}
