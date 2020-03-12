using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Jobs;
using Unity.Burst;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Collections;
public class NewtonBodySystem : JobComponentSystem
{

    [BurstCompile]
    struct ProcessNewtonBodies : IJobForEach<NewtonBody, Translation>
    {
        public float deltaTime;
        public void Execute(ref NewtonBody body, ref Translation translation)
        {
            body.velocity += body.accel * deltaTime;
            translation.Value += body.velocity * deltaTime;
            body.accel = new float3(0);
            body.force = new float3(0);
        }
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var job = new ProcessNewtonBodies()
            {
                deltaTime = Time.DeltaTime
            };
        return job.Schedule(this, inputDeps);
    }

    //public void Execute(ref NewtonBody body, ref Translation translation)
    //{
    //    body.velocity += body.accel * Time.deltaTime;
    //    translation.Value += body.velocity * Time.deltaTime;
    //    body.accel = new float3(0);
    //    body.force = new float3(0);
    //}


    //protected override void OnUpdate()
    //{
    //    Entities.WithAll<NewtonBody>().ForEach((ref NewtonBody body, ref Translation translation) =>
    //         {
    //             body.velocity += body.accel * Time.DeltaTime;
    //             translation.Value += body.velocity * Time.DeltaTime;
    //             body.accel = new float3(0);
    //             body.force = new float3(0);
    //         }
    //    );
    //}

}
