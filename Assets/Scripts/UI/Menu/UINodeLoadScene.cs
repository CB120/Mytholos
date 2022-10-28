using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class UINodeLoadScene : UIMenuNode
{
    [SerializeField] string nameOfSceneToLoad;
    [SerializeField] bool playTransitionBackwards;
    [SerializeField] bool destroyAllParticipantsOnSceneLoad;
    [SerializeField] bool updateAllParticipantActionMaps;
    [SerializeField] string nameOfActionMap;
    [SerializeField] Animator transitionAnimator;

    override public void OnAction(Action action, int playerNumber)
    {
        switch (action)
        {
            case Action.Submit:
                // Ethan: Yes, I know this could now be a Switch in itself
                if (nameOfSceneToLoad == "ControllerSetup") UISFXManager.PlaySound("Game Start");
                if (nameOfSceneToLoad == "Encyclopedia") UISFXManager.PlaySound("To Encyclopaedia");
                if (nameOfSceneToLoad == "PartyBuilder") UISFXManager.PlaySound("Back to PartyBuilder");
                if (nameOfSceneToLoad == "Main Menu") UISFXManager.PlaySound("Back Scene");
                if (nameOfSceneToLoad == "ArenaEddie") UISFXManager.PlaySound("Match Start");

                if (transitionAnimator)
                {
                    if (!destroyAllParticipantsOnSceneLoad && updateAllParticipantActionMaps)
                    {
                        foreach (PlayerParticipant participant in FindObjectsOfType<PlayerParticipant>())
                        {
                            participant.PlayerInput.SwitchCurrentActionMap(nameOfActionMap);
                        }
                    }

                    transitionAnimator.SetInteger("Direction", playTransitionBackwards ? -1 : 1);
                    transitionAnimator.SetTrigger("Fade");
                    StartCoroutine(LoadScene(0.35f));
                }
                else
                {
                    SceneManager.LoadScene(nameOfSceneToLoad);
                }
                break;
            default:
                break;
        }
    }

    IEnumerator LoadScene(float timeToWait)
    {
        foreach (PlayerParticipant participant in FindObjectsOfType<PlayerParticipant>())
            participant.DisablePlayerInput();

        yield return new WaitForSeconds(timeToWait);
        
        if (destroyAllParticipantsOnSceneLoad)
        {
            foreach (PlayerParticipant participant in FindObjectsOfType<PlayerParticipant>())
                participant.DestroyParticipant();
        }

        SceneManager.LoadScene(nameOfSceneToLoad);

        foreach (PlayerParticipant participant in FindObjectsOfType<PlayerParticipant>())
            participant.EnablePlayerInput();
    }
}