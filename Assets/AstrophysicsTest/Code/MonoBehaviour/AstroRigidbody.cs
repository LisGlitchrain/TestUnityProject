using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstroRigidbody : MonoBehaviour
{
    [SerializeField] float mass = 1f;
    [SerializeField] Vector3 position = new Vector3(0,0,0);
    [SerializeField] Vector3 velocity = new Vector3(0, 0, 0);
    [SerializeField] Vector3 currentAccel = new Vector3(0, 0, 0);

    public Vector3 Position { get => GetPosition(); set { position = value; } }
    public Vector3 Velocity { get => velocity; set { velocity = value; } }
    public float Mass { get => mass; set { mass = value; } }
    public Vector3 CurrentAccel { get => currentAccel; set { currentAccel = value; } }

    public bool FreezePos;

    Vector3 GetPosition()
    {
        position = transform.position;
        return position;
    }

    private void OnEnable()
    {
        position = transform.position;
    }
    private void Start()
    {
        position = transform.position;
    }
    private void FixedUpdate()
    {
        if (FreezePos) return;
        position = transform.position;
        velocity += currentAccel * Time.fixedDeltaTime;
        position += Velocity * Time.fixedDeltaTime;    
        transform.position = position;
        currentAccel = Vector3.zero;

        Debug.DrawLine(position, position + velocity, Color.red);
    }

    public void AddForce(Vector3 force)
    {
        if (mass == 0) return;
        currentAccel += force / mass;
    }

}
