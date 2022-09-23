using UnityEngine;
using Myths;

public class FlurryAbility : Ability
{
    private float DurationTimer;

    Transform Collider;

    [SerializeField] private float FlurryDuration;

    public override void Start()
    {
        base.Start();
        Collider = gameObject.transform.GetChild(0);
        
        if (Collider)
        {
            Collider.gameObject.SetActive(true);
        }
    }

    public override void Update()
    {
        DurationTimer += Time.deltaTime;

        if (DurationTimer > FlurryDuration)
        {
            Destroy(gameObject);
        }
    }

    public override void Trigger(Myth myth)
    {
        Attack(myth, ability.damage); //Called In The Parent Ability
        Debug.LogWarning($"Beam Collided With Object: {myth.gameObject.name}");
        base.Trigger(myth);
    }
}
