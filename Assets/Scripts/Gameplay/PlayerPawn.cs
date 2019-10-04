using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerPawn : Pawn
{
    private Rigidbody rb;
    private Vector3 moveDir=Vector3.zero;
    private Vector3 backbarPosition = Vector3.zero;
    public Rigidbody backBar;
    public float topMargin=20;
    public float minMargin = 2;
    public float sideMargin = 7f;
    public static event FNotify_1Params<float> OnMoveWorld;
    
    protected override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody>();

    }

    void MovefrontBar()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        moveDir.x = h;
        moveDir.z = v;
        Vector3 targetPos = rb.position + Time.fixedDeltaTime * moveSpeed * moveDir;

        if (targetPos.z > topMargin)
        {
            OnMoveWorld?.Invoke(targetPos.z- topMargin);
            targetPos.z = topMargin;

        }

        targetPos.z = Mathf.Clamp(targetPos.z, minMargin, topMargin);
        targetPos.x = Mathf.Clamp(targetPos.x, -sideMargin, sideMargin);

        rb.position = targetPos;
        backbarPosition = backbarPosition;
        backbarPosition.x = rb.position.x;


        backBar.position = backbarPosition;


    }

    private void OnCollisionEnter(Collision other)
    {
        other.transform.GetComponent<Ball>()?.PushTowards((other.transform.position - transform.position).normalized, moveSpeed + 1);

    }

    void OnTriggerEnter(Collider other)
    {   

         
        other.GetComponent<Ball>()?.PushTowards((other.transform.position-transform.position).normalized,moveSpeed+1);
    }
    
    // Update is called once per frame
    void Update()
    {
        MovefrontBar();
        
    }
}
