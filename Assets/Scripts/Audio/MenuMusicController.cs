using UnityEngine;
using FMODUnity;

public enum E_MenuState
{
    MainMenu = 0, //values correspond with Parameter values in FMOD
    Options = 1,
    Encyclopaedia = 2,
    MythSelect = 3,
    ArenaSelect = 4
}

public class MenuMusicController : MonoBehaviour
{
    // Properties
    [Tooltip("If True, enables Don't Destroy On Load for this GameObject")]
    public bool persistent = true;


    // Variables
    [SerializeField] private E_MenuState currentMenuState = E_MenuState.MainMenu;


    // References
    StudioEventEmitter menuMusicEmitter;


    // Engine-called
    private void Awake()
    {
        if (persistent) DontDestroyOnLoad(gameObject);

        menuMusicEmitter = GetComponent<StudioEventEmitter>();
    }

    void Start()
    {
        ChangeMenuState(currentMenuState); //allows you to change the default state in the Inspector
    }


    // Methods
        // Public
    public void ChangeMenuState(E_MenuState newState)
    {
        currentMenuState = newState;
        menuMusicEmitter.SetParameter("Menu Music", (int)currentMenuState);
    }
}
