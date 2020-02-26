using System;
using System.Collections.Generic;
using UnityEngine;


public class MonoGravReceiver : MonoBehaviour
{
    public AstroRigidbody AstroRigidbody { get; private set; }
    private void OnEnable()
    {
        FindObjectOfType<GravitationManager>().GravReceivers.Add(this);
        AstroRigidbody = GetComponent<AstroRigidbody>();
    }
    private void OnDisable()
    {
        FindObjectOfType<GravitationManager>().GravReceivers.Remove(this);
    }
}

