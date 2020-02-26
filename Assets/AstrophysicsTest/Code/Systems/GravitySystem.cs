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
    public const float Gscaled = 6.674e-4f; //6.674e-11f;

    //[BurstCompile]
    //struct ProcessGracityEmitters : IJobForEach<GravityEmitter, Translation>
    //{
    //    public void Execute(ref GravityEmitter gravityEmitter, ref Translation translation)
    //    {
    //        var job = new ProcessGravityReceivers()
    //        {
    //            currentEmitter = gravityEmitter,
    //            currentEmitterTranslation = translation
    //        };
    //    }

    //}


    //[RequireComponentTag(typeof(GravityReceiver))]
    //[BurstCompile]
    //struct ProcessGravityReceivers : IJobForEach<NewtonBody, Translation>
    //{
    //    public GravityEmitter currentEmitter;
    //    public Translation currentEmitterTranslation;
    //    public void Execute(ref NewtonBody body, ref Translation translation)
    //    {

    //        var direction = (currentEmitterTranslation.Value - translation.Value);
    //        var distance = GetMagnitudeFloat3(direction);
    //        if (distance == 0) return;
    //        var forceMagnitude = Gscaled * (currentEmitter.mass * body.mass) / Mathf.Pow(distance, 2);
    //        var force = NormalizeFloat3(direction) * forceMagnitude;

    //        //Debug.Log($"force add:{GetMagnitudeFloat3(force)}");
    //        body.force += force;
    //    }
    //}


    //protected override JobHandle OnUpdate(JobHandle inputDeps)
    //{
    //    var job = new ProcessGracityEmitters();
    //    return job.Schedule(this, inputDeps);
    //}



    protected override void OnUpdate()
    {
        ProcessAllEmitters();
    }

    public void ProcessAllEmitters()
    {
        Entities.WithAll<GravityEmitter>().ForEach<GravityEmitter, Translation>((ref GravityEmitter gravEmitter, ref Translation translationEmitter) =>
            {
                ProcessAllReceivers(gravEmitter, translationEmitter);
            }
        );
    }
    public void ProcessAllReceivers(GravityEmitter gravityEmitter, Translation translationEmitter)
    {
        Entities.WithAll<GravityReceiver>().ForEach<NewtonBody, Translation>((ref NewtonBody bodyReceiver, ref Translation translationReceiver) =>
            {
                var direction = (translationEmitter.Value - translationReceiver.Value);
                var distance = Float3Helper.GetMagnitudeFloat3(direction);
                if (distance < gravityEmitter.radius) return;
                var accelMagnitude = Gscaled * gravityEmitter.mass / Mathf.Pow(distance, 2);
                var accel = Float3Helper.NormalizeFloat3(direction) * accelMagnitude;

                //Debug.Log($"force add:{GetMagnitudeFloat3(force)}");
                bodyReceiver.accel += accel;
            }
        );
    }
}
