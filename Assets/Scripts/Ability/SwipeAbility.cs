using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeAbility : Ability
{
    [SerializeField] private float lerpSpeed = 0.5f;
    [SerializeField] private float rotationAngle = 180f;

    private void Start()
    {
        
    }

    public override void Update()
    {
        gameObject.transform.rotation = Quaternion.Lerp(gameObject.transform.rotation, new Quaternion(0f, rotationAngle, 0f, 0f), lerpSpeed * Time.deltaTime)
;        base.Update();
    }
}
