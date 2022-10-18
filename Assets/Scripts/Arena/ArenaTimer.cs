using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaTimer : MonoBehaviour
{
    /*[HideInInspector]*/ public float currentTime;
    private bool timerStopped = false;

    void Start()
    {
        currentTime = 0;
    }
    void Update()
    {
        if (currentTime < 90)
        {
            currentTime += Time.deltaTime;
        } else if (!timerStopped)
        {
            StartShrinking();
        }
    }

    void StartShrinking()
    {
        timerStopped = true;
        Debug.Log("Game Timer Done! Time to shrink");
    }
}
