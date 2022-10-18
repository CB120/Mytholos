using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepSFXController : MonoBehaviour
{
    // Properties
    [Header("Properties")]
    public float timeToDestroySFX = 0.2f;
    public bool playFrontStepOnLeft = true;


    // Variables


    // References
    [Header("In-Prefab References")]
    public Transform frontLeftLeg;
    public Transform frontRightLeg;
    public Transform backLeftLeg;
    public Transform backRightLeg;

    [Header("SFX Prefabs")]
    public GameObject frontStepSFXPrefab;
    public GameObject backStepSFXPrefab;


    // Animation-Event Called
    public void FrontStep()
    {
        Transform target = frontRightLeg;
        if (playFrontStepOnLeft) target = frontLeftLeg;
        PlayFrontStep(target);
    }

    public void FrontLeftStep()
    {
        PlayBackStep(frontLeftLeg); // yes, I know it says PlayBackStep(), this is because the function called determines the sound played, and FrontStep is louder, only for the Run animation
    }

    public void FrontRightStep()
    {
        PlayBackStep(frontRightLeg); // so basically yes, this isn't a mistake :)
    }

    public void BackLeftStep()
    {
        PlayBackStep(backLeftLeg);
    }

    public void BackRightStep()
    {
        PlayBackStep(backRightLeg);
    }


    // Methods
        // Private
    void PlayFrontStep(Transform targetTransform)
    {
        GameObject sfx = Instantiate(frontStepSFXPrefab, targetTransform);
        Destroy(sfx, timeToDestroySFX);
    }

    void PlayBackStep(Transform targetTransform)
    {
        GameObject sfx = Instantiate(backStepSFXPrefab, targetTransform);
        Destroy(sfx, timeToDestroySFX);
    }
}
