using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(AstroRigidbody))]
public class Attractor : MonoBehaviour
{
    public AstroRigidbody AstroRigidbody { get; private set; }
    public float radius = 0.5f;
    private void OnEnable()
    {
        FindObjectOfType<GravitationManager>().Attractors.Add(this);
        AstroRigidbody = GetComponent<AstroRigidbody>();
    }
    private void OnDisable()
    {
        FindObjectOfType<GravitationManager>().Attractors.Remove(this);
    }
}
