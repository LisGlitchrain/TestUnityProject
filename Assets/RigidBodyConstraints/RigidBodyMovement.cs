using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
public class RigidBodyMovement : MonoBehaviour
{

    public float forceMultiplier;
    public float speed;
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.velocity = Vector3.forward.normalized * speed + new Vector3(0, rb.velocity.y,0);
        //rb.AddForce(Vector3.forward * forceMultiplier * Time.deltaTime, ForceMode.VelocityChange);
        //rb.MovePosition(transform.position + Vector3.forward * Time.deltaTime * speed);
        
    }
}
