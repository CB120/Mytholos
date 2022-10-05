using UnityEngine;
using FMODUnity;
using UnityEngine.InputSystem.LowLevel;

public enum E_MenuState
{
    MainMenu = 0, //values correspond with Parameter values in FMOD
    Options = 1,
    Encyclopaedia = 2,
    MythSelect = 3,
    ArenaSelect = 4
}

[System.Serializable]
public class MenuMusicTrack : MusicLayer
{
    [Tooltip("Menu State")]
    public E_MenuState state;
}

public class MenuMusicController : MonoBehaviour
{
    // Properties
    [Header("Properties")]
    [Tooltip("If True, enables Don't Destroy On Load for this GameObject")]
    public bool persistent = true;
    [Tooltip("% volume per second | Larger values = faster crossfades")]
    public float fadeInRate = 40f;
    [Tooltip("% volume per second | Larger values = faster crossfades")]
    public float fadeOutRate = 20f;

    [Space(30)]
    [Tooltip("All Menu Music Tracks")]
    public MenuMusicTrack[] musicTracks;

    // Variables
    [Space(30)]
    [Header("Debug Readouts")]
    [SerializeField] private E_MenuState currentMenuState = E_MenuState.MainMenu;


    // References
    static MenuMusicController Singleton;

    StudioEventEmitter menuMusicEmitter;


    // Engine-called
    private void Awake()
    {
        if (Singleton != null && Singleton != this)
        {
            Destroy(gameObject);
            return;
        }

        Singleton = this;
        if (persistent) DontDestroyOnLoad(gameObject);

        menuMusicEmitter = GetComponent<StudioEventEmitter>();
    }

    void Start()
    {
        ChangeMenuState(currentMenuState); //allows you to change the default state in the Inspector
    }

    private void Update()
    {
        UpdateFades();
        UpdateTrackVolumes();
    }


    // Methods
        // Public
            // Static
    public static void ChangeMenuState(E_MenuState newState)
    {
        if (Singleton != null)
        {
            Singleton.SetMenuState(newState);
        } else
        {
            Debug.LogWarning("Singleton == null | Couldn't find a MenuMusicController!");
        }
    }

            // Non-Static
    public void SetMenuState(E_MenuState state)
    {
        GetTrackWithState(currentMenuState).targetVolume = 0f;
        GetTrackWithState(state).targetVolume = 100f;
        currentMenuState = state;
    }


        // Private
    void UpdateFades() //applies the lerp between the current volume and target volume
    {
        foreach (MenuMusicTrack m in musicTracks)
        {
            if (m.manualVolumeOverride) continue; //GUARD to allow manualVolumeOverride to disable the automated fading

            //Adding/subtracting a constant per second = linear transition.
            //Adjust the volume curve in FMOD, don't try and change the lerp curve
            if (m.volume > m.targetVolume) m.volume -= fadeOutRate * Time.deltaTime;
            if (m.volume < m.targetVolume) m.volume += fadeInRate * Time.deltaTime;
            if (Mathf.Abs(m.volume - m.targetVolume) <= 0.1f) m.volume = m.targetVolume;

            //If we end up with targetVolumes that aren't 0 or 100, add an 'if volume delta <= 0.1, volume = target' here

            m.volume = Mathf.Clamp(m.volume, 0f, 100f);
        }
    }

    void UpdateTrackVolumes() //applies the current volumes to FMOD
    {
        foreach (MenuMusicTrack m in musicTracks)
        {
            menuMusicEmitter.SetParameter(m.name + " Volume", m.volume); //Syntax in FMOD is currently '<Layer> Volume', e.g 'Main Menu Volume'
        }
    }


    // Functions
    MenuMusicTrack GetTrackWithState(E_MenuState state)
    {
        foreach (MenuMusicTrack m in musicTracks)
        {
            if (m.state == state) return m;
        }

        Debug.LogWarning("Could not find a Menu Music Track with E_MenuState " + state + ". Returning null, errors expected.");
        return null;
    }
}
