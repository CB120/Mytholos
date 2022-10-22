using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.InputSystem;

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

                UISFXManager.PlaySound("Game Start");

                if (transitionAnimator)
                {
                    foreach (PlayerParticipant participant in FindObjectsOfType<PlayerParticipant>())
                    {
                        if (destroyAllParticipantsOnSceneLoad)
                            participant.DestroyParticipant();
                        else
                        {
                            if (updateAllParticipantActionMaps)
                            {
                                PlayerInput input = participant.GetComponent<PlayerInput>();
                                if (input != null)
                                {
                                    string oldActionMap = input.currentActionMap.name;
                                    input.actions.FindActionMap(oldActionMap).Disable();
                                    input.actions.FindActionMap(nameOfActionMap).Enable();
                                }
                                else
                                    Debug.LogWarning("Failed to update a player participant's action map");
                            }
                            participant.DisablePlayerInput(0.5f);
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
        yield return new WaitForSeconds(timeToWait);
        SceneManager.LoadScene(nameOfSceneToLoad);
    }
}