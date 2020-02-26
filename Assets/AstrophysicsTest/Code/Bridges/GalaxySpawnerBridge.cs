using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Rendering;
using Unity.Mathematics;
public class GalaxySpawnerBridge : MonoBehaviour
{
    public Mesh unitMesh;
    public Material unitMaterial;
    EntityManager entityManager;
    public float scaleToSpawn;
    List<Entity> entities = new List<Entity>();
    public int satellitesToSpawn;
    public Vector2 satelliteSpawnRange;
    System.Random rand;
    // Start is called before the first frame update
    void Start()
    {
        rand = new System.Random();
        entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        var sunArchetype = entityManager.CreateArchetype(
            typeof(Translation),
            typeof(Rotation),
            typeof(RenderMesh),
            typeof(RenderBounds),
            typeof(LocalToWorld),
            typeof(Scale),
            typeof(GravityEmitter)
            );


        var movingSunArchetype = entityManager.CreateArchetype(
            typeof(Translation),
            typeof(Rotation),
            typeof(RenderMesh),
            typeof(RenderBounds),
            typeof(LocalToWorld),
            typeof(Scale),
            typeof(GravityEmitter),
            typeof(NewtonBody)
            );

        //-------------------------------2 STAR

        //var entity = entityManager.CreateEntity(movingSunArchetype);
        //entityManager.AddComponentData<Translation>(entity,
        //        new Translation
        //        {
        //            Value = new float3(-1, 15, -2)
        //        }
        //    );
        //entityManager.AddSharedComponentData(entity,
        //        new RenderMesh
        //        {
        //            mesh = unitMesh,
        //            material = unitMaterial
        //        }
        //    );
        //entityManager.AddComponentData<Scale>(entity,
        //        new Scale
        //        {
        //            Value = scaleToSpawn
        //        }
        //    );
        //entityManager.AddComponentData<GravityEmitter>(entity,
        //        new GravityEmitter
        //        {
        //            mass = 10000,
        //            radius = 1
        //        }
        //    );
        //entityManager.AddComponentData<NewtonBody>(entity,
        //new NewtonBody
        //    {
        //        accel = new float3(0),
        //        velocity = new float3(0,-0.5f,0),
        //        force = new float3(0),
        //        mass = 100,
        //    }
        //);
        //entities.Add(entity);

        //----------------1 STAR

        var entity = entityManager.CreateEntity(sunArchetype);
        entityManager.AddComponentData<Translation>(entity,
                new Translation
                {
                    Value = new float3(0, 0, 0)
                }
            );
        entityManager.AddSharedComponentData(entity,
                new RenderMesh
                {
                    mesh = unitMesh,
                    material = unitMaterial
                }
            );
        entityManager.AddComponentData<Scale>(entity,
                new Scale
                {
                    Value = scaleToSpawn
                }
            );
        entityManager.AddComponentData<GravityEmitter>(entity,
                new GravityEmitter
                {
                    mass = 10000,
                    radius = 1
                }
            ); 
        entities.Add(entity);




        //---------------------------------------------------------------------

        var planetArchetype = entityManager.CreateArchetype(
            typeof(Translation),
            typeof(Rotation),
            typeof(RenderMesh),
            typeof(RenderBounds),
            typeof(LocalToWorld),
            typeof(Scale),
            typeof(NewtonBody),
            typeof(GravityReceiver)
            );
        for(var  i = 0; i < satellitesToSpawn; i++)
        {
            var distance = UnityEngine.Random.Range(satelliteSpawnRange.x, satelliteSpawnRange.y);
            var pointToSpawn = Float3Helper.NormalizeFloat3( new float3(UnityEngine.Random.Range(-satelliteSpawnRange.y, satelliteSpawnRange.y),
                                            UnityEngine.Random.Range(-satelliteSpawnRange.y, satelliteSpawnRange.y)/10, 
                                            UnityEngine.Random.Range(-satelliteSpawnRange.y, satelliteSpawnRange.y))) * distance;           
            entities.Add(SpawnSatellite(pointToSpawn, entity , planetArchetype));
        }
    }

    float CalculateOrbitalSpeed(Entity mainBody, Entity satellite)
    {
        var gravEmitter = entityManager.GetComponentData<GravityEmitter>(mainBody);
        var emitterTranslation = entityManager.GetComponentData<Translation>(mainBody);
        var satelliteTranslation = entityManager.GetComponentData<Translation>(satellite);
        var speed = Mathf.Sqrt(
                gravEmitter.mass * GravitySystem.Gscaled / Float3Helper.GetMagnitudeFloat3(emitterTranslation.Value - satelliteTranslation.Value)
            );
        return speed;
    }

    float3 GetOrbitalSpeed(Entity mainBody, Entity satellite)
    {
        var emitterTranslation = entityManager.GetComponentData<Translation>(mainBody);
        var satelliteTranslation = entityManager.GetComponentData<Translation>(satellite);

        var orbitalSpeed = CalculateOrbitalSpeed(mainBody, satellite);
        print($"orbitalSpeed:{orbitalSpeed}");
        var plane = new Plane(satelliteTranslation.Value - emitterTranslation.Value, satelliteTranslation.Value);
        Debug.DrawLine(satelliteTranslation.Value, satelliteTranslation.Value + Float3Helper.FromVector3(plane.normal), Color.magenta);
        var orbitalForward = Vector3.Cross(Vector3.up, satelliteTranslation.Value - emitterTranslation.Value).normalized;
        var velocity = orbitalForward.normalized * orbitalSpeed;
        Debug.DrawLine(satelliteTranslation.Value, satelliteTranslation.Value + Float3Helper.FromVector3(velocity), Color.green);

        print($"Spawn velocity: {velocity.magnitude}");
        return velocity;
    }

    public Entity SpawnSatellite(float3 pointToSpawn, Entity mainBody, EntityArchetype planetArchetype)
    {

        var satellite = entityManager.CreateEntity(planetArchetype);
        entityManager.AddComponentData<Translation>(satellite,
                new Translation
                {
                    Value = pointToSpawn
                }
            );
        entityManager.AddSharedComponentData(satellite,
                new RenderMesh
                {
                    mesh = unitMesh,
                    material = unitMaterial
                }
            );
        entityManager.AddComponentData<Scale>(satellite,
                new Scale
                {
                    Value = scaleToSpawn
                }
            );
        entityManager.AddComponentData<NewtonBody>(satellite,
                new NewtonBody
                {
                    mass = 100,
                    force = new float3(0, 0, 0),
                    velocity = GetOrbitalSpeed(mainBody, satellite),
                    accel = new float3(0, 0, 0)
                }
            );
        return satellite;
    }
}
