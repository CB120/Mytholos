using System;
using Participants;
using UnityEngine;
using UnityEngine.Events;

namespace TemporaryWinScreen
{
    // TODO: Move to Scripts
    public class PauseController : MonoBehaviour
    {
        [SerializeField] private PlayerParticipantRuntimeSet playerParticipantRuntimeSet;
        
        [NonSerialized] public UnityEvent paused = new();
        [NonSerialized] public UnityEvent resumed = new();

        public PlayerParticipant PausingPlayer { get; private set; }
        
        private void OnEnable()
        {
            playerParticipantRuntimeSet.ListenToAll(participant => participant.pauseRequested, OnPauseRequested);
            playerParticipantRuntimeSet.ListenToAll(participant => participant.resumeRequested, OnResumeRequested);
        }

        private void OnDisable()
        {
            playerParticipantRuntimeSet.UnlistenToAll(participant => participant.pauseRequested, OnPauseRequested);
            playerParticipantRuntimeSet.UnlistenToAll(participant => participant.resumeRequested, OnResumeRequested);
        }

        private void OnPauseRequested(PlayerParticipant playerParticipant)
        {
            PausingPlayer = playerParticipant;
            paused.Invoke();
        }

        private void OnResumeRequested(PlayerParticipant playerParticipant)
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