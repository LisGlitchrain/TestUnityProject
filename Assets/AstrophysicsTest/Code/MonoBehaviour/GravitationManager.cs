using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravitationManager : MonoBehaviour
{
    public const float Gscaled = 6.674e-4f; //6.674e-11f;
    public List<Attractor> Attractors = new List<Attractor>();
    public List<MonoGravReceiver> GravReceivers = new List<MonoGravReceiver>();

    public float TimeScale = 1f;
    //public float distanceScale = 1f;

    public bool physicsEnabled = false;

    public void Update()
    {
        Time.timeScale = TimeScale;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(physicsEnabled)
        {
            for (var i = 0; i < Attractors.Count; i++)
            {
                for (var j = 0; j < GravReceivers.Count; j++)
                {
                    AttractReceiver(Attractors[i], GravReceivers[j]);
                }
            }
        }
    }


    void AttractBoth(Attractor attractor1, Attractor arrtactor2)
    {
        var rigidBody2 = arrtactor2.AstroRigidbody;
        var rigidBody1 = attractor1.AstroRigidbody;

        var direction = (rigidBody1.Position - rigidBody2.Position);
        var distance = direction.magnitude;
        if (distance == 0) return;
        var forceMagnitude = Gscaled * (rigidBody1.Mass * rigidBody2.Mass) / Mathf.Pow(distance, 2);
        var force = direction.normalized * forceMagnitude;
        rigidBody2.AddForce(force);
        rigidBody1.AddForce(-force);
    }

    void AttractReceiver(Attractor attractor, MonoGravReceiver gravReceiver)
    {
        var rigidBody2 = gravReceiver.AstroRigidbody;
        var rigidBody1 = attractor.AstroRigidbody;

        var direction = (rigidBody1.Position - rigidBody2.Position);
        var distance = direction.magnitude;
        if (distance == 0) return;
        if (distance < attractor.radius) return;
        var forceMagnitude = Gscaled * (rigidBody1.Mass * rigidBody2.Mass) / Mathf.Pow(distance, 2);
        var force = direction.normalized * forceMagnitude;

        rigidBody2.AddForce(force);
    }
}
