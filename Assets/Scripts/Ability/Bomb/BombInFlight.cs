using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Myths;
using FMODUnity;

public class BombInFlight : MonoBehaviour
{
    // Start is called before the first frame update
    public BombAbility bomb;
    [Tooltip("How high the arc should be, in units")]
    public float arcHeight = 1;
    public float destroySpeed = 1;
    public Collider bombCollider;
    public MeshRenderer mesh;

    public StudioEventEmitter bombFuseSFX; //SFX stuff added by Ethan
    bool explosionElementalSFXPlayed = false;

    // Update is called once per frame
    void Update()
    {
        if (!bomb.hasReachedPosition)
        {
            float progressFromStart = (Vector3.Distance(bomb.nextBasePos, bomb.startPos));
            float progressToEndPos = -(Vector3.Distance(bomb.nextBasePos, bomb.targetPos));
            float dist = Vector3.Distance(bomb.startPos, bomb.targetPos);
            float baseY = Mathf.Lerp(bomb.startPos.y, bomb.targetPos.y, progressFromStart / dist);
            float arc = (arcHeight * progressFromStart * progressToEndPos) / (-0.25f * dist * dist);
            transform.rotation = LookAt2D(bomb.nextBasePos - transform.position);
            transform.position = new Vector3(transform.position.x, baseY + arc, transform.position.z);
        }
        else
        {
            transform.localScale = Vector3.Lerp(
                transform.localScale,
                new Vector3(bomb.areaOfEffect, bomb.areaOfEffect, bomb.areaOfEffect),
                bomb.expandSpeed * Time.deltaTime
            );
        }
    }

    static Quaternion LookAt2D(Vector2 forward)
    {
        return Quaternion.Euler(0, 0, Mathf.Atan2(forward.y, forward.x) * Mathf.Rad2Deg);
    }

    private void OnTriggerEnter(Collider other)
    {
        bomb.hasReachedPosition = true;
        bomb.abilityPS.gameObject.SetActive(true);
        mesh.enabled = false;
        //Invoke("ResetScale", (bomb.expandSpeed * 0.75f));
        Myth attackedMyth = other.gameObject.GetComponent<Myth>();
        if (attackedMyth)
        {
            bomb.Trigger(attackedMyth);
        }
        Destroy(bomb.gameObject, destroySpeed);

        bombFuseSFX.enabled = false; //SFX stuff added by Ethan

        if (!explosionElementalSFXPlayed)
        {
            bomb.PlayElementalSFX();
            explosionElementalSFXPlayed = true;
        }
    }

    //private void ResetScale()
    //{
    //    bombCollider.enabled = false;
    //    bomb.areaOfEffect = 0f;
    //}
}
