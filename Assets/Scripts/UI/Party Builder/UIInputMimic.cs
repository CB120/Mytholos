using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.DualShock;
using UnityEngine.InputSystem.XInput;
using UnityEngine.InputSystem.Switch;

public class UIInputMimic : MonoBehaviour
{
    enum InputToMimic { FaceButtonNorth, FaceButtonEast, FaceButtonSouth, FaceButtonWest }
    [SerializeField] InputToMimic inputToMimic;
    [SerializeField] int playerNumber;
    [SerializeField] Sprite[] buttonsXbox; // Up/Down for each of: Xbox, PlayStation, Nintendo, Keyboard
    [SerializeField] Sprite[] buttonsPlayStation;
    [SerializeField] Sprite[] buttonsNintendo;
    [SerializeField] Sprite[] buttonsKeyboard;

    int controllerType; // We assume Xbox by default
    Image image;

    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
        //image.sprite = offOnSprites[0];

        foreach(PlayerParticipant participant in FindObjectsOfType<PlayerParticipant>())
        {
            if (participant.partyIndex == playerNumber)
            {
                PlayerInput playerInput = participant.GetComponent<PlayerInput>();
                if (playerInput)
                {
                    if (playerInput.devices[0] is DualShockGamepad)
                        controllerType = 1;
                    else if (playerInput.devices[0] is SwitchProControllerHID)
                        controllerType = 2;
                    else if (playerInput.devices[0] is XInputController)
                        controllerType = 0;
                    else
                        controllerType = 3;
                }

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

        UpdateSprite(false);
    }

    void UpdateSprite(bool isPressed)
    {
        switch (controllerType)
        {
            case 0:
                image.sprite = buttonsXbox[isPressed ? 1 : 0];
                break;
            case 1:
                image.sprite = buttonsPlayStation[isPressed ? 1 : 0];
                break;
            case 2:
                image.sprite = buttonsNintendo[isPressed ? 1 : 0];
                break;
            case 3:
                image.sprite = buttonsKeyboard[isPressed ? 1 : 0];
                break;
            default:
                image.sprite = buttonsXbox[isPressed ? 1 : 0];
                break;
        }
    }
}
