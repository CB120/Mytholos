using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class SetUpManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI[] playerTexts;
    [SerializeField] UIMenuNodeGraph returnToPreviousSceneGraph;
    PlayerInputManager inputManager;
    int playerCount;

    void Start()
    {
        inputManager = FindObjectOfType<PlayerInputManager>();
        MenuMusicController.ChangeMenuState(E_MenuState.Options);
    }

    void Update()
    {
        if (playerCount < inputManager.playerCount)
        {
            playerTexts[playerCount].text = "Player " + (playerCount + 1) + " has joined!";
            playerTexts[playerCount].GetComponent<RectTransform>().anchoredPosition = Vector2.zero; // Amend unwanted 0.5 offset/graphical issue that arises with centred text
            playerCount++;

            // Attach that playerparticipant to the menu that lets them cancel to return to main menu
            foreach (PlayerParticipant participant in FindObjectsOfType<PlayerParticipant>())
            {
                participant.currentMenuGraph = returnToPreviousSceneGraph;
                participant.GetComponent<PlayerInput>().SwitchCurrentActionMap("UI");
            }

                if (playerCount >= 2)
            {
                foreach (PlayerParticipant participant in FindObjectsOfType<PlayerParticipant>())
                    participant.DisablePlayerInput(1.0f);

                // Progress to party builder
                Invoke("LoadNextScene", 0.5f);
            }
        }
    }

    void LoadNextScene()
    {
        if (transitionAnimator)
        {
            foreach (PlayerParticipant participant in FindObjectsOfType<PlayerParticipant>())
                participant.DisablePlayerInput(0.5f);

            transitionAnimator.SetInteger("Direction", 1);
            transitionAnimator.SetTrigger("Fade");
            StartCoroutine(LoadScene(0.35f));
        }
        else
        {
            SceneManager.LoadScene(nameOfSceneToLoad);
        }
    }

    [SerializeField] string nameOfSceneToLoad;
    [SerializeField] Animator transitionAnimator;

    IEnumerator LoadScene(float timeToWait)
    {
        yield return new WaitForSeconds(timeToWait);
        SceneManager.LoadScene(nameOfSceneToLoad);
    }
}
