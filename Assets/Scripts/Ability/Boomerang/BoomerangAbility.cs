using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Myths;
using Elements;
using FMODUnity;

public class BoomerangAbility : Ability
{
    private Transform BoomerangTransform;
    private Vector3 Direction;

    private bool Returning;

    [Header("Boomerang Ability Fields")]
    [SerializeField] private float speed;
    [SerializeField] private float acceleration;

    private float currentPoint;
    private float endPoint;

    StudioEventEmitter boomerangSFXemitter; //SFX, inserted by Ethan

    [SerializeField] private TrailRenderer Trail1;
    [SerializeField] private TrailRenderer Trail2;
    [SerializeField] private TrailRenderer Trail3;

    [SerializeField] private Renderer Renderer;

    public override void Start()
    {
        base.Start();
        Direction = owningMyth.transform.forward;
        BoomerangTransform = this.gameObject.transform;
        transform.parent = null;

        boomerangSFXemitter = GetComponent<StudioEventEmitter>(); //SFX, inserted by Ethan

        Gradient Trailgrad = new Gradient();
        
        GradientColorKey StartColor = new GradientColorKey(ability.element.boomerangStartColor, 0);

        Trailgrad.SetKeys(new GradientColorKey[] { StartColor }, new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(1.0f, 1.0f) });

        Renderer.material = ability.element.boomerangMaterial;
        Trail1.colorGradient = Trailgrad;
        Trail2.colorGradient = Trailgrad;
        Trail3.colorGradient = Trailgrad;
    }

    // Update is called once per frame
    public override void Update()
    {
        if (!owningMyth)
            Destroy(this.gameObject);

        if (speed <= 0)
        {
            Returning = true;
            endPoint = Vector3.Distance(BoomerangTransform.position, owningMyth.transform.position);
        }


        if (!Returning)
        {
            speed -= acceleration * Time.deltaTime;
            BoomerangTransform.position += Direction * speed * Time.deltaTime;
        }
        
        if (Returning)
        {
            speed += acceleration * Time.deltaTime;
            Direction = ( owningMyth.transform.position - BoomerangTransform.position).normalized;
            BoomerangTransform.position += Direction * speed * Time.deltaTime;
            
            currentPoint = Vector3.Distance(BoomerangTransform.position, owningMyth.transform.position);

            this.gameObject.transform.localScale = new Vector3 (1*currentPoint/endPoint, 1, 1*currentPoint/endPoint);

            if (Vector3.Distance(BoomerangTransform.position, owningMyth.transform.position) < .1f)
                Destroy(this.gameObject);
        }

        boomerangSFXemitter.SetParameter("Boomerang Speed", speed); //SFX, inserted by Ethan
    }

    public override void Trigger(Myth myth)
    {
        Attack(myth, ability.damage); //Called In The Parent Ability
        Debug.LogWarning($"Shot Collided With Object: {myth.gameObject.name}");
        base.Trigger(myth);
    }

    public override void TerrainInteraction()
    {
        if (!Returning)
        {
            speed = 0;
        }
        else Destroy(this.gameObject);
    }
}