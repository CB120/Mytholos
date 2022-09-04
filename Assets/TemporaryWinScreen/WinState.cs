using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.SceneManagement;
public class WinState : MonoBehaviour
{
    /*TO BE REMOVED AFTER MONDAY*/
    public int team1Remaining = 2, team2Remaining = 2;
    public GameObject obj;
    public TextMeshProUGUI text; 
    public void DecreaseScore(int partyIndex)
    {
        if (partyIndex == 0)
        {
            team1Remaining--;
            if (team1Remaining == 0)
            {
                obj.SetActive(true);

                StartCoroutine(AnyButtonCoroutine());
                text.text = "Team 2 was Defeated";
            }
        }
        else if (partyIndex == 1)
        {
            team2Remaining--;
            if (team2Remaining == 0)
            {
                obj.SetActive(true);
                
                StartCoroutine(AnyButtonCoroutine());
                text.text = "Team 2 was Defeated";
            }
        }
    }

    private IEnumerator AnyButtonCoroutine()
    {
        yield return new WaitForSeconds(1);
        
        InputSystem.onEvent += OnAnyButtonPressed;
    }

    public void RestartGame()
    {
        obj.SetActive(false);
        
        InputSystem.onEvent -= OnAnyButtonPressed;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    // TODO: Copied from Round Robbin. Not optimal at all.
    private void OnAnyButtonPressed(InputEventPtr eventPtr, InputDevice device)
    {
        if (!eventPtr.IsA<StateEvent>() && !eventPtr.IsA<DeltaStateEvent>()) return;

        // Copied from InputUser, works somehow.
        foreach (var control in eventPtr.EnumerateChangedControls(device: device, magnitudeThreshold: 0.0001f))
        {
            if (control == null || control.synthetic || control.noisy) continue;

            RestartGame();
            
            break;
        }
    }
}
