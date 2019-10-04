using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private Rigidbody rb;

    private float moveSpeed;
    private Vector3 moveDirection;
    public LayerMask bounceableLayerMask;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void PushTowards(Vector3 Direction, float Impulse)
    {
        moveDirection = Direction;
        moveSpeed = Impulse;
        rb.velocity = Direction * moveSpeed;

    }

    private void OnCollisionEnter(Collision other)
    {
        IDamageable iDamageable = other.transform.GetComponent<IDamageable>();

        if (iDamageable != null)
        {

 
            if (!iDamageable.ApplyDamage(1, transform))
            {

            }




        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Border"))
        {
            Vector3 normal = other.transform.position.x < transform.position.x ? Vector3.right : Vector3.left;
            ChangeDirection(Vector3.Reflect(rb.velocity,normal));
        }

        if (other.CompareTag("BorderTop"))
        {
             
            ChangeDirection(Vector3.Reflect(rb.velocity, Vector3.back));
        }

        Debug.Log(other.tag);

        if (other.CompareTag("BackBar"))
        {

            ChangeDirection(Vector3.Reflect(rb.velocity, Vector3.forward));
            return;
            ;
        }


        if ( other.GetComponent<IDamageable>()!=null) 
        {
           

            Vector3 dir = (other.transform.position - transform.position);
            RaycastHit hit;
            Debug.DrawRay(transform.position,dir.normalized,Color.magenta,100,false);

           // RaycastHit hits[] =Physics.RaycastAll(transform.position,rb.velocity.normalized,)
            if (Physics.Raycast(transform.position, rb.velocity.normalized, out hit,11,bounceableLayerMask.value ))
            { 
               Vector3 refVector3= Vector3.Reflect(rb.velocity, hit.normal);
                 Debug.DrawRay(transform.position, refVector3, Color.yellow,1000,false);
                ChangeDirection(refVector3);
            }
            if (!other.GetComponent<IDamageable>().ApplyDamage(1, transform))
            {

            }




        }
    }

    public void ChangeDirection(Vector3 Direction)
    {
        rb.velocity = Direction.normalized * rb.velocity.magnitude;
    }
    void MoveBall()
    {

    }
    // Update is called once per frame
    void Update()
    {
        MoveBall();
    }
}
