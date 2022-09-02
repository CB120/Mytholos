using UnityEngine;
using UnityEngine.InputSystem;
public class BeamAttack : MonoBehaviour
{
    public GameObject Beam;
    public GameObject BeamOrigin;
    private Transform BeamTransform;
    public Vector3 BeamOriginPosition;
    private GameObject NewBeam;
    private float BeamOfset;

    private bool BeamExists;

    public void UseAbilityWest(InputAction.CallbackContext context)
    {
        //Debug.Log(context);
        if (!BeamExists)
        {
            NewBeam = Instantiate(Beam, BeamOriginPosition, Quaternion.identity);
            NewBeam.transform.parent = BeamOrigin.transform;
            BeamExists = true;
        }
        
    }
    

    private void Start()
    {
        BeamOfset = 0.5f;
        BeamOriginPosition = BeamOrigin.transform.position;
        BeamTransform = BeamOrigin.transform;
    }
}