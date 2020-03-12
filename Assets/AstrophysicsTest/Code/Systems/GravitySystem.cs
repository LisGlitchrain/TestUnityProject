using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Jobs;
using Unity.Burst;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Collections;

public class GravitySystem : ComponentSystem
{
    public static float Gscaled = 6.674e-4f; //6.674e-11f;


    //[BurstCompile]
    //struct ProcessGracityEmitters : IJobForEach<GravityEmitter, Translation>
    //{
    //    public void Execute(ref GravityEmitter gravityEmitter, ref Translation translation)
    //    {
    //        foreach(var receiver in receiversQuery.ToEntityArray(Allocator.TempJob))
    //        {
    //            var bodyReceiver = GalaxySpawnerBridge.entityManager.GetComponentObject<NewtonBody>(receiver, typeof(NewtonBody));
    //            var translationReceiver = GalaxySpawnerBridge.entityManager.GetComponentObject<Translation>(receiver, typeof(Translation));

    //            var direction = (translation.Value - translationReceiver.Value);
    //            var distance = Float3Helper.GetMagnitudeFloat3(direction);
    //            if (distance < gravityEmitter.radius) return;
    //            var accelMagnitude = Gscaled * gravityEmitter.mass / Mathf.Pow(distance, 2);
    //            var accel = Float3Helper.NormalizeFloat3(direction) * accelMagnitude;
    //            bodyReceiver.accel += accel;
    //        }
    //    }
    //}



    //protected override void OnUpdate(JobHandle inputDeps)
    //{
    //    var job = new ProcessGracityEmitters();
    //    return job.Schedule(this, inputDeps);
    //}



    //---------------------------------------------------------TEST



    protected override void OnUpdate()
    {
        ProcessAllEmitters();
    }


    public void ProcessAllEmitters()
    {
        Entities.WithAll<GravityEmitter>().ForEach((ref GravityEmitter gravEmitter, ref Translation translationEmitter) =>
            {
                ProcessAllReceivers(gravEmitter, translationEmitter);
            }
        );
    }

    public void ProcessAllReceivers(GravityEmitter gravityEmitter, Translation translationEmitter)
    {
        Entities.WithAll<GravityReceiver>().ForEach((ref NewtonBody bodyReceiver, ref Translation translationReceiver) =>
            {
                var direction = (translationEmitter.Value - translationReceiver.Value);
                var distance = Float3Helper.GetMagnitudeFloat3(direction);
                if (distance < gravityEmitter.radius) return;
                var accelMagnitude = Gscaled * gravityEmitter.mass / Mathf.Pow(distance, 2);
                var accel = Float3Helper.NormalizeFloat3(direction) * accelMagnitude;
                bodyReceiver.accel += accel;
            }
        );
    }
}
