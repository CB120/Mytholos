using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace TemporaryWinScreen
{
    public class ResultsUI : MonoBehaviour
    {
        //public GameObject obj;
        //public TextMeshProUGUI text;

        [Header("Scene References")]
        [SerializeField] private WinState winState;
        [SerializeField] CanvasGroup gameplayUI;
        [SerializeField] CanvasGroup resultsUI;
        [SerializeField] Image playerWinImage;
        [SerializeField] UIMenuNodeGraph resultsMenu;

        [Header("Asset References")]
        [SerializeField] Sprite[] playerWinSprites;

        private void OnEnable()
        {
            winState.gameEnded.AddListener(OnGameEnded);
        }
        
        private void OnDisable()
        {
            winState.gameEnded.RemoveListener(OnGameEnded);
        }

        private void OnGameEnded(int winningPlayerIndex)
        {
            //obj.SetActive(true);
            //StartCoroutine(AnyButtonCoroutine());
            
            resultsUI.gameObject.SetActive(true);

            // Swap player input controls schemes, disable their inputs momentarily for duration of transition
            foreach (PlayerParticipant participant in FindObjectsOfType<PlayerParticipant>())
            {
                // Update the winning player's current UI graph to the results menu
                //if (participant.partyIndex == winningPlayerIndex)
                //    participant.currentMenuGraph = resultsMenu;

                //participant.DisablePlayerInput(0.35f);
                //PlayerInput input = participant.GetComponent<PlayerInput>();      // Temporarily commented out
                //input.actions.FindActionMap("Player").Disable();
                //input.actions.FindActionMap("UI").Enable();

                participant.DisablePlayerInput(2.0f);
            }

            StartCoroutine(UhhReloadTheScene());

            // Begin transition to fade out gameplay UI and fade in results UI
            StartCoroutine(FadeGameOutFadeResultsIn(0.35f));

            // Do something with the camera
            FindObjectOfType<EpicEddieCam>().FocusOnSingleMyth(winningPlayerIndex);

            // Set winning text image based on winning player index
            playerWinImage.sprite = playerWinSprites[winningPlayerIndex];
        }

        IEnumerator UhhReloadTheScene()
        {
            yield return new WaitForSeconds(2.0f);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        IEnumerator FadeGameOutFadeResultsIn(float duration)
        {
            float timer = 0;
            Vector2 originalSize = playerWinImage.rectTransform.sizeDelta;
            float cycles = Mathf.PI * 5.0f / (duration + 0.5f);

            while (timer < duration)
            {
                resultsUI.alpha = timer / duration;
                gameplayUI.alpha = 1 - timer / duration;

                playerWinImage.rectTransform.sizeDelta = originalSize * Mathf.Lerp((0.8f + 0.4f * Mathf.Sin(cycles * timer)), 1, timer / (duration + 0.5f));

                timer += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }

            resultsUI.alpha = 1.0f;
            gameplayUI.alpha = 0.0f;

            while (timer < duration + 0.5f)
            {
                playerWinImage.rectTransform.sizeDelta = originalSize * Mathf.Lerp((0.8f + 0.4f * Mathf.Sin(cycles * timer)), 1, timer / (duration + 0.5f));

                timer += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }

            playerWinImage.rectTransform.sizeDelta = originalSize;
        }

        //private IEnumerator AnyButtonCoroutine()
        //{
        //    yield return new WaitForSeconds(1);

        //    InputSystem.onEvent += OnAnyButtonPressed;
        //}

        //public void RestartGame()
        //{
        //    obj.SetActive(false);

        //    InputSystem.onEvent -= OnAnyButtonPressed;
        //    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        //}

        //// TODO: Copied from Round Robbin. Not optimal at all.
        //private void OnAnyButtonPressed(InputEventPtr eventPtr, InputDevice device)
        //{
        //    if (!eventPtr.IsA<StateEvent>() && !eventPtr.IsA<DeltaStateEvent>()) return;

        //    // Copied from InputUser, works somehow.
        //    foreach (var control in eventPtr.EnumerateChangedControls(device: device, magnitudeThreshold: 0.0001f))
        //    {
        //        if (control == null || control.synthetic || control.noisy) continue;

        //        RestartGame();

        //        break;
        //    }
        //}
    }
}