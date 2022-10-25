using System;
using Myths;
using UnityEngine;
using UnityEngine.Events;

public class WinState : MonoBehaviour
{
    private int team1Remaining = 3;
    private int team2Remaining = 3;
    [SerializeField] private MythRuntimeSet mythRuntimeSet;

    [NonSerialized] public UnityEvent<int> gameEnded = new();

    // TODO: Duplicate code. See EpicEddieCam.
    private void OnEnable()
    {
        mythRuntimeSet.itemAdded.AddListener(OnMythAdded);
        mythRuntimeSet.itemRemoved.AddListener(OnMythRemoved);
        
        mythRuntimeSet.items.ForEach(OnMythAdded);
    }

    private void OnDisable()
    {
        mythRuntimeSet.itemAdded.RemoveListener(OnMythAdded);
        mythRuntimeSet.itemRemoved.RemoveListener(OnMythRemoved);
        
        mythRuntimeSet.items.ForEach(OnMythRemoved);
    }

    private void OnMythAdded(Myth myth)
    {
        myth.died.AddListener(OnMythDied);
    }

    private void OnMythRemoved(Myth myth)
    {
        myth.died.RemoveListener(OnMythDied);
    }

    private void OnMythDied(Myth myth)
    {
        DecreaseScore(myth.partyIndex);
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
        gameEnded.Invoke(0);
    }
}
