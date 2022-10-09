using UnityEngine;
using Myths;
using Elements;

public class FlurryAbility : Ability
{
    private Element element { get => ability.element.element; }

    private Color color;

    private float DurationTimer;

    private float performTimer;

    Transform Collider;

    [SerializeField] private ParticleSystem PS1;
    [SerializeField] private ParticleSystem PS2;

    #region Colours
    private GradientColorKey WoodStart = new GradientColorKey(new Color(0f, 0.3f, 0f), 0);
    private GradientColorKey WoodEnd = new GradientColorKey(new Color(0f, 0.8f, 0f), 1);
    private GradientColorKey EarthStart = new GradientColorKey(new Color(0.53f, 0.22f, 0.13f), 0);
    private GradientColorKey EarthEnd = new GradientColorKey(new Color(1.0f, 0.29f, 0.01f), 1);

    private GradientColorKey StartColor;
    private GradientColorKey EndColor;
    #endregion

    public override void Start()
    {
        var FlurryColor = PS1.colorOverLifetime;

        Gradient grad = new Gradient();
        GetColor();
        grad.SetKeys(new GradientColorKey[] { StartColor, EndColor }, new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(0.0f, 1.0f) });

        FlurryColor.color = grad;

        Collider = gameObject.transform.GetChild(0);
    }

    public override void Update()
    {
        performTimer += Time.deltaTime;

        if (performTimer > ability.chargeTime)
        {
            DurationTimer += Time.deltaTime;
            if (Collider) 
            Collider.gameObject.SetActive(true);
            
        }
       
        if (DurationTimer > ability.performTime - ability.chargeTime)
        {
            Destroy(gameObject);
        }
    }
    public override void TriggerStay(Myth myth)
    {
        Trigger(myth);
    }
    public override void Trigger(Myth myth)
    {
        Attack(myth, ability.damage); //Called In The Parent Ability
        Debug.LogWarning($"Beam Collided With Object: {myth.gameObject.name}");
        base.Trigger(myth);
    }

    private void GetColor()
    {
        switch (element)
        {
            case Element.Wood:
                StartColor = WoodStart;
                EndColor = WoodEnd;
                break;
            case Element.Metal:
                break;
            case Element.Earth:
                StartColor = EarthStart;
                EndColor = EarthEnd;
                break;
        }
    }
}
