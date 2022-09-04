using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
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
                text.text = "Team 2 was Defeated";
            }
        }
        else if (partyIndex == 1)
        {
            team2Remaining--;
            if (team2Remaining == 0)
            {
                obj.SetActive(true);
                text.text = "Team 2 was Defeated";
            }
        }
    }

    public void RestartGame()
    {
        obj.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
