using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepSFXController : MonoBehaviour
{
    // Properties
    [Header("Properties")]
    public float timeToDestroySFX = 0.2f;


    // Variables


    // References
    [Header("SFX Prefabs")]
    public GameObject frontStepSFXPrefab;
    public GameObject backStepSFXPrefab;


    // Animation-Event Called
    public void PlayFrontStep(Transform targetTransform)
    {
        GameObject sfx = Instantiate(frontStepSFXPrefab, targetTransform);
        Destroy(sfx, timeToDestroySFX);
    }

    public void PlayBackStep(Transform targetTransform)
    {
        GameObject sfx = Instantiate(backStepSFXPrefab, targetTransform);
        Destroy(sfx, timeToDestroySFX);
    }
}
