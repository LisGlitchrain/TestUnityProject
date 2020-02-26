using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

public struct NewtonBody : IComponentData
{
    public float3 accel;
    public float3 velocity;
    public float mass;
    public float3 force;
}
