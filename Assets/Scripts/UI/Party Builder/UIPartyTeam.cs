using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Myths;

public class UIPartyTeam : MonoBehaviour
{
    public List<MythData> mythDatas; // Remembers configs for each possible selectable myth, so that if you change your myth selection, the abilities you chose are not forgotten
    public RawImage[] mythTeamRenderImages = new RawImage[3];
    public int[] selectedMythIndices = { -1, -1, -1 };

    public void UpdateUI(int teamMemberIndex, int mythSelectionIndex, Texture teamRender)
    {
        selectedMythIndices[teamMemberIndex] = mythSelectionIndex;
        mythTeamRenderImages[teamMemberIndex].texture = teamRender;
        mythTeamRenderImages[teamMemberIndex].enabled = true;
    }
}
