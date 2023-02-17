using System;
using Myths;
using UnityEngine;
using UnityEngine.Events;

public class WinState : MonoBehaviour
{
    private int team1Remaining = 3;
    private int team2Remaining = 3;
    [SerializeField] private MythRuntimeSet mythRuntimeSet;

    [Header("Debug Only")]
    [SerializeField] private int winningPlayerIndex;

    [NonSerialized] public UnityEvent<int> gameEnded = new();

    private void OnEnable()
    {
        mythRuntimeSet.ListenToAll(myth => myth.died, OnMythDied);
    }

    private void OnDisable()
    {
        mythRuntimeSet.UnlistenToAll(myth => myth.died, OnMythDied);
    }

    private void OnMythDied(Myth myth)
    {
        DecreaseScore(myth.PartyIndex);
    }

    private void DecreaseScore(int partyIndex)
    {
        if (partyIndex == 0)
        {
            team1Remaining--;
            if (team1Remaining == 0)
            {
                gameEnded.Invoke(1);
            }
        }
        else if (partyIndex == 1)
        {
            team2Remaining--;
            if (team2Remaining == 0)
            {
                gameEnded.Invoke(0);
            }
        }
    }

    [ContextMenu("EndGame")]
    private void EndGame()
    {
        gameEnded.Invoke(winningPlayerIndex);
    }
}
