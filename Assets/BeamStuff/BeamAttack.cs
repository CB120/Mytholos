using UnityEngine;
using UnityEngine.InputSystem;
public class BeamAttack : MonoBehaviour
{
    public BeamSO BeamSettings;

    public GameObject BeamPrefab;

    public GameObject BeamOrigin;
    private Transform BeamTransform;
    public Vector3 BeamOriginPosition;

    private GameObject Beam;

    private void Start()
    {
        BeamOriginPosition = BeamOrigin.transform.position;
        BeamTransform = BeamOrigin.transform;
    }

    public void UseAbilityWest(InputAction.CallbackContext context)
    {
        //Debug.Log(context);
        if (!Beam)
        {
            Beam = Instantiate(BeamPrefab, BeamOriginPosition, Quaternion.identity);
            Beam.transform.parent = BeamOrigin.transform.parent;
        }
        
    }

    public void DealDamage(Character character)
    {
        character.TakeDamage(BeamSettings.damage);
    }

    public void DestroyBeam()
    {
        Destroy(this);
    }

    public void BeamTimeout()
    {

    }
    

}