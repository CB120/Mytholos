using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomerangMovement : MonoBehaviour
{
    private GameObject ReturnPoint;
    private Vector3 Direction;

    private bool Returning;

    private Transform Boomerang;

    [SerializeField] private float acceleration;
    [SerializeField] private float speed;

    // Start is called before the first frame update
    void Start()
    {
        Boomerang = this.gameObject.transform;
    }

    // Update is called once per frame
    void Update()
    {
        //if (!Returning)
        //{
            //speed -= acceleration * Time.deltaTime;
            this.gameObject.transform.position += Direction * speed * Time.deltaTime;
        //}
        //else
        //{
         //   speed += acceleration * Time.deltaTime;
           // Boomerang.position = Vector3.MoveTowards(Boomerang.position, ReturnPoint.transform.position, speed);
        //}

        //if (speed <= 0 && !Returning)
        //{
          //  Returning = true;
        //}

        /*if (Boomerang.position == ReturnPoint.transform.position)
        {
            Destroy(ReturnPoint);
            Destroy(this.gameObject);
        }*/
    }

    public void setReturnPoint(GameObject ReturnPoint)
    {
        ReturnPoint = this.ReturnPoint;
    }
    
    public void setDirection(Vector3 Direction)
    {
        Direction = this.Direction;
    }

    public void nullParent()
    {
        transform.parent = null;
    }
}
