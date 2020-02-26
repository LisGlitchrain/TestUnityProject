using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Jobs;
using Unity.Burst;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Collections;
public class NewtonBodySystem : ComponentSystem
{
    protected override void OnUpdate()
    {
        Entities.ForEach<NewtonBody, Translation>((ref NewtonBody body, ref Translation translation) =>
             {
                 body.velocity += body.accel * Time.DeltaTime;
                 translation.Value += body.velocity * Time.DeltaTime;
                 body.accel.x = 0;
                 body.accel.y = 0;
                 body.accel.z = 0;
                 body.force.x = 0;
                 body.force.y = 0;
                 body.force.z = 0;
             }
        );
    }
}
