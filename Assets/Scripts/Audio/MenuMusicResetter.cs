using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuMusicResetter : MonoBehaviour
{
    void Start()
    {
        MenuMusicController.ChangeMenuState(E_MenuState.MainMenu);
        Destroy(gameObject);
    }
}
