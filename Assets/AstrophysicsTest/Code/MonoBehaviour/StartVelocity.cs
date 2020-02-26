using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StartVelocityType
{
    None,
    Predefined,
    Random,
    Orbital,
}

[RequireComponent(typeof(AstroRigidbody))]
public class StartVelocity : MonoBehaviour
{
    public Vector3 startVelocity;
    public float max;
    public bool onStart;
    public StartVelocityType type;
    // Start is called before the first frame update
    void Start()
    {
        if(type == StartVelocityType.Predefined)
            GetComponent<AstroRigidbody>().Velocity = startVelocity;
    }

    public void ApplyVelocityRandom()
    {
        if (type != StartVelocityType.Random) return;
        var randomVelocity = new Vector3(Random.Range(-max, max), Random.Range(-max, max), Random.Range(-max, max));
        GetComponent<AstroRigidbody>().Velocity = randomVelocity;
    }

    public void ApplyVelocity(Vector3 velocity)
    {
        GetComponent<AstroRigidbody>().Velocity = velocity;
    }
}


