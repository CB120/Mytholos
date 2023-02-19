using System.Collections;
using Participants;
using UnityEngine;
using UnityEngine.UI;

namespace TemporaryWinScreen
{
    public class ResultsUI : MonoBehaviour
    {
        [Header("Scene References")]
        [SerializeField] private WinState winState;
        [SerializeField] private PauseController pauseController;
        [SerializeField] CanvasGroup gameplayUI;
        [SerializeField] CanvasGroup resultsUI;
        [SerializeField] Image playerWinImage;
        [SerializeField] UIMenuNodeGraph resultsMenu;
        [SerializeField] private UIMenuNodeGraph pauseMenu;

        [Header("Asset References")]
        [SerializeField] private PlayerParticipantRuntimeSet playerParticipantRuntimeSet;
        [SerializeField] Sprite[] playerWinSprites;
        [SerializeField] private Sprite pausedTextSprite;

        private void OnEnable()
        {
            winState.gameEnded.AddListener(OnGameEnded);
            
            pauseController.paused.AddListener(OnPaused);
            pauseController.resumed.AddListener(OnResumed);
        }
        
        private void OnDisable()
        {
            winState.gameEnded.RemoveListener(OnGameEnded);
            
            pauseController.paused.RemoveListener(OnPaused);
            pauseController.resumed.RemoveListener(OnResumed);
        }

        private void OnPaused()
        {
            StartCoroutine(OnGamePausedRoutine(pauseController.PausingPlayer.partyIndex));
        }

        private void OnResumed()
        {
            StartCoroutine(OnGameResumedRoutine(pauseController.PausingPlayer.partyIndex));
        }

        private void OnGameEnded(int winningPlayerIndex)
        {
            StartCoroutine(OnGameEndedRoutine(winningPlayerIndex));
        }

        private IEnumerator OnGamePausedRoutine(int pausingPlayerIndex)
        {
            playerWinImage.sprite = pausedTextSprite;
            
            yield return StartCoroutine(DisableInputAndDisplay(pausingPlayerIndex, pauseMenu));
        }

        private IEnumerator OnGameResumedRoutine(int pausingPlayerIndex)
        {
            yield return StartCoroutine(DisableInputAndHide(pausingPlayerIndex, pauseMenu));
        }
        
        private IEnumerator OnGameEndedRoutine(int winningPlayerIndex)
        {
            FindObjectOfType<EpicEddieCam>().FocusOnSingleMyth(winningPlayerIndex);

            // Set winning text image based on winning player index
            playerWinImage.sprite = playerWinSprites[winningPlayerIndex];

            yield return StartCoroutine(DisableInputAndDisplay(winningPlayerIndex, resultsMenu));
        }

        private IEnumerator DisableInputAndDisplay(int controllingPlayerIndex, UIMenuNodeGraph nodeGraph)
        {
            resultsUI.gameObject.SetActive(true);

            nodeGraph.gameObject.SetActive(true);

            // Swap player input controls schemes, disable their inputs momentarily for duration of transition
            foreach (PlayerParticipant participant in playerParticipantRuntimeSet.items)
            {
                // Update the winning player's current UI graph to the results menu
                if (participant.partyIndex == controllingPlayerIndex)
                    participant.currentMenuGraph = nodeGraph;

                participant.DisablePlayerInput();
            }

            nodeGraph.playerCursors[1 - controllingPlayerIndex].gameObject.SetActive(false);

            // Begin transition to fade out gameplay UI and fade in results UI
            yield return StartCoroutine(FadeGameOutFadeResultsIn(0.35f));
            
            playerParticipantRuntimeSet.items.ForEach(playerParticipant =>
            {
                playerParticipant.EnablePlayerInput();

                playerParticipant.PlayerInput.SwitchCurrentActionMap("UI");
            });
        }

        private IEnumerator DisableInputAndHide(int controllingPlayerIndex, UIMenuNodeGraph nodeGraph)
        {
            // Swap player input controls schemes, disable their inputs momentarily for duration of transition
            foreach (PlayerParticipant participant in playerParticipantRuntimeSet.items)
            {
                // Update the winning player's current UI graph to the results menu
                // if (participant.partyIndex == controllingPlayerIndex)
                //     participant.currentMenuGraph = resultsMenu;

                participant.DisablePlayerInput();
            }

            nodeGraph.playerCursors[1 - controllingPlayerIndex].gameObject.SetActive(true);

            // Begin transition to fade out gameplay UI and fade in results UI
            yield return StartCoroutine(FadeResultsOutFadeGameIn(0.15f));
            
            playerParticipantRuntimeSet.items.ForEach(playerParticipant =>
            {
                playerParticipant.EnablePlayerInput();

                playerParticipant.PlayerInput.currentActionMap = playerParticipant.PlayerInput.actions.FindActionMap("Player");
            });
            
            resultsUI.gameObject.SetActive(false);
            
            nodeGraph.gameObject.SetActive(false);
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
                yield return null;
            }

            resultsUI.alpha = 1.0f;
            gameplayUI.alpha = 0.0f;

            while (timer < duration + 0.5f)
            {
                playerWinImage.rectTransform.sizeDelta = originalSize * Mathf.Lerp((0.8f + 0.4f * Mathf.Sin(cycles * timer)), 1, timer / (duration + 0.5f));

                timer += Time.deltaTime;
                yield return null;
            }

            playerWinImage.rectTransform.sizeDelta = originalSize;
        }

        // TODO:
        private IEnumerator FadeResultsOutFadeGameIn(float duration)
        {
            float timer = 0;
            Vector2 originalSize = playerWinImage.rectTransform.sizeDelta;

            while (timer < duration)
            {
                gameplayUI.alpha = timer / duration;
                resultsUI.alpha = 1 - timer / duration;

                playerWinImage.rectTransform.sizeDelta = Mathf.Cos(timer / duration * Mathf.PI * 0.5f) * originalSize;

                timer += Time.deltaTime;
                yield return null;
            }

            resultsUI.alpha = 0.0f;
            gameplayUI.alpha = 1.0f;
            playerWinImage.rectTransform.sizeDelta = originalSize;
        }
    }
}