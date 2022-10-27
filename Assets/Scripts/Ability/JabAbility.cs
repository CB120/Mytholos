using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Myths
{
    public class JabAbility : Ability
    {
        [Header("Jab Ability Fields")]
        [SerializeField] private float speed = 0.5f;
        [SerializeField] private float jabDistance = 0.5f;
        [SerializeField] private Collider jabCollider;
        [SerializeField] ParticleSystemRenderer pr;
        public override void Start()
        {
            pr.material.SetColor("_Toon_Ramp_Tinting", ability.element.color);
            base.Start();
            Destroy(this.gameObject, ability.timeToDestroy);
        }
        public override void Update()
        {
            gameObject.transform.localScale = Vector3.Lerp(gameObject.transform.localScale, new Vector3(1, 1, jabDistance), speed * Time.deltaTime);
        }

        // Update is called once per frame
        public override void Trigger(Myth myth)
        {
            Attack(myth, ability.damage);
            base.Trigger(myth);

            if (myth.partyIndex != owningMyth.partyIndex)
            jabCollider.enabled = false;
        }
    }
}
