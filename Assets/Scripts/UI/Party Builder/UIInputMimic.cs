using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIInputMimic : MonoBehaviour
{
    enum InputToMimic { FaceButtonNorth, FaceButtonEast, FaceButtonSouth, FaceButtonWest }
    [SerializeField] InputToMimic inputToMimic;
    [SerializeField] int playerNumber;
    [SerializeField] Sprite[] offOnSprites; // up, down
    Image image;

    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
        image.sprite = offOnSprites[0];

        foreach(PlayerParticipant participant in FindObjectsOfType<PlayerParticipant>())
        {
            if (participant.partyIndex == playerNumber)
            {
                switch (inputToMimic)
                {
                    case InputToMimic.FaceButtonNorth:
                        participant.FaceButtonNorth.AddListener(UpdateSprite);
                        break;
                    case InputToMimic.FaceButtonEast:
                        participant.FaceButtonEast.AddListener(UpdateSprite);
                        break;
                    case InputToMimic.FaceButtonSouth:
                        participant.FaceButtonSouth.AddListener(UpdateSprite);
                        break;
                    case InputToMimic.FaceButtonWest:
                        participant.FaceButtonWest.AddListener(UpdateSprite);
                        break;
                }
            }
        }
    }

    void UpdateSprite(bool isPressed)
    {
        image.sprite = offOnSprites[isPressed ? 1 : 0];
    }
}
