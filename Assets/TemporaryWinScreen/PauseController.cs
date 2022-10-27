using System;
using UnityEngine;
using UnityEngine.Events;

namespace TemporaryWinScreen
{
    public class PauseController : MonoBehaviour
    {
        [SerializeField] private PlayerParticipantRuntimeSet playerParticipantRuntimeSet;
        
        [NonSerialized] public UnityEvent paused = new();
        [NonSerialized] public UnityEvent resumed = new();

        public PlayerParticipant PausingPlayer { get; private set; }
        
        // TODO: Duplicate code. See WinState and EpicEddieCam.
        private void OnEnable()
        {
            playerParticipantRuntimeSet.itemAdded.AddListener(OnPlayerParticipantAdded);
            playerParticipantRuntimeSet.itemRemoved.AddListener(OnPlayerParticipantRemoved);
            
            playerParticipantRuntimeSet.items.ForEach(OnPlayerParticipantAdded);
        }

        private void OnDisable()
        {
            playerParticipantRuntimeSet.itemAdded.RemoveListener(OnPlayerParticipantAdded);
            playerParticipantRuntimeSet.itemRemoved.RemoveListener(OnPlayerParticipantRemoved);
            
            playerParticipantRuntimeSet.items.ForEach(OnPlayerParticipantRemoved);
        }

        private void OnPlayerParticipantAdded(PlayerParticipant playerParticipant)
        {
            playerParticipant.pauseRequested.AddListener(OnPauseRequested);
            playerParticipant.resumeRequested.AddListener(OnResumeRequested);
        }

        private void OnPlayerParticipantRemoved(PlayerParticipant playerParticipant)
        {
            playerParticipant.pauseRequested.RemoveListener(OnPauseRequested);
            playerParticipant.resumeRequested.RemoveListener(OnResumeRequested);
        }

        public void OnPauseRequested(PlayerParticipant playerParticipant)
        {
            PausingPlayer = playerParticipant;
            paused.Invoke();
        }

        public void OnResumeRequested(PlayerParticipant playerParticipant)
        {
            if (playerParticipant == PausingPlayer)
                resumed.Invoke();
        }

        public void Resume()
        {
            resumed.Invoke();
        }
    }
}