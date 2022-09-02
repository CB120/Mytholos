using UnityEngine;

public class BeamAttack : MonoBehaviour
{
    public GameObject Beam;
    public GameObject BeamOrigin;
    private Transform BeamTransform;
    public Vector3 BeamOriginPosition;
    private GameObject NewBeam;
    private float BeamOfset;

    private void Start()
    {
        BeamOfset = 0.5f;
        BeamOriginPosition = BeamOrigin.transform.position;
        BeamTransform = BeamOrigin.transform;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            NewBeam = Instantiate(Beam, BeamOriginPosition + new Vector3(BeamOfset, 0), Quaternion.identity);
            NewBeam.transform.parent = BeamOrigin.transform;
        }
    }
}
