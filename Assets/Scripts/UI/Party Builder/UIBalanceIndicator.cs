using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class UIBalanceIndicator : MonoBehaviour
{
    [SerializeField] private UIPartyTeam friendlyTeam;
    [SerializeField] private UIPartyTeam opposingTeam;

    [SerializeField] private Image[] dots = new Image[3];
    [SerializeField] private Image backgroundColour;

    [SerializeField] private Color[] balanceColours = new Color[3];

    private List<SO_Element> lastFriendlyElements;
    private List<SO_Element> lastOpposingElements;

    private void Start()
    {
        friendlyTeam.uiUpdated += OnUIUpdatedFriendly_Callback;
        opposingTeam.uiUpdated += OnUIUpdatedOpposing_Callback;
    }


    public void OnUIUpdatedFriendly_Callback(List<SO_Element> selectedMyths)
    {
        lastFriendlyElements = selectedMyths;
        if (lastFriendlyElements != null)
            CompareElementalMatchups();
    }
    public void OnUIUpdatedOpposing_Callback(List<SO_Element> selectedMyths)
    {
        lastOpposingElements = selectedMyths;
        if (lastOpposingElements != null)
            CompareElementalMatchups();
    }

    private void CompareElementalMatchups()
    {
        if (lastFriendlyElements == null) return;
        if (lastOpposingElements == null) return;

        int balance = 0;

        for (int x = 0; x < lastFriendlyElements.Count; x++)
        {
            for (int y = 0; y < lastOpposingElements.Count; y++)
            {
                if (lastFriendlyElements[x].strongAgainst.Contains(lastOpposingElements[y]))
                {
                    balance++;
                }
                if (lastFriendlyElements[x].weakAgainst.Contains(lastOpposingElements[y]))
                {
                    balance--;
                }
            }
        }
        Color dotColour = Color.white;
        dotColour.a = 1f;
        foreach (Image image in dots)
            image.color = dotColour;
        switch (balance)
        {
            case > 1:
        backgroundColour.color = balanceColours[2];
                break;
            case -1:
            case 0:
            case 1:
                dotColour.a = 0.1f;
                dots[2].color = dotColour;
                backgroundColour.color = balanceColours[1];
                break;
            case < -1:
                dotColour.a = 0.1f;
                dots[1].color = dotColour;
                dots[2].color = dotColour;
                backgroundColour.color = balanceColours[0];
                break;
        }

    }



}
