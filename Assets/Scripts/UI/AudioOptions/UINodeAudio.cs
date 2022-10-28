using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UINodeAudio : UIMenuNode
{
    [SerializeField] Slider volume;
    [SerializeField] float increments;

    public override void OnAudioFuckYa(Direction direction, int playerNumber)
    {
        base.OnAudioFuckYa(direction, playerNumber);
        switch (direction)
        {
            case Direction.Left:
                volume.value -= increments;
                Debug.Log("no way this works");
                break;
            case Direction.Right:
                volume.value += increments;
                break;
            default:
                break;
        }
    }
}
