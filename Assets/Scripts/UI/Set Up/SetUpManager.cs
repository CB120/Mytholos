using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class SetUpManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI[] playerTexts;
    PlayerInputManager inputManager;
    int playerCount;

    void Start()
    {
        inputManager = FindObjectOfType<PlayerInputManager>();
    }

    void Update()
    {
        if (playerCount < inputManager.playerCount)
        {
            playerTexts[playerCount].text = "Player " + (playerCount + 1) + " has joined!";
            playerTexts[playerCount].GetComponent<RectTransform>().anchoredPosition = Vector2.zero; // Amend unwanted 0.5 offset/graphical issue that arises with centred text
            playerCount++;

            if (playerCount >= 2)
            {
                // Progress to party builder
                Invoke("LoadNextScene", 1.0f);
            }
        }
    }

    void LoadNextScene()
    {
        if (transitionAnimator)
        {
            transitionAnimator.SetInteger("Direction", playTransitionBackwards ? -1 : 1);
            transitionAnimator.SetTrigger("Fade");
            StartCoroutine(LoadScene(0.3f));
        }
        else
        {
            SceneManager.LoadScene(nameOfSceneToLoad);
        }
    }

    [SerializeField] string nameOfSceneToLoad;
    [SerializeField] bool playTransitionBackwards;
    [SerializeField] Animator transitionAnimator;

    IEnumerator LoadScene(float timeToWait)
    {
        yield return new WaitForSeconds(timeToWait);
        SceneManager.LoadScene(nameOfSceneToLoad);
    }

}
