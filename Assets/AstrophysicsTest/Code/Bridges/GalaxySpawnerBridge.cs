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
    public Material[] unitMaterials;
    public static EntityManager entityManager;
    public float centerScaleToSpawn;
    public float starScaleToSpawn;
    public float nebulaScaleToSpawn;
    List<Entity> entities = new List<Entity>();
    public int satellitesToSpawn;
    public Vector2 satelliteSpawnRange;
    public float galaxyCenterRadius = 0.1f;
    public float gScaled = 6.674e-4f;
    System.Random rand;
    public Vector2Int centerMatIndexies; 
    public Vector2Int starMatIndexies; 
    public Vector2Int nebulasrMatIndexies;
    public int armsCount;
    // Start is called before the first frame update
    void Start()
    {
        GravitySystem.Gscaled = gScaled;
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

        var entity = entityManager.CreateEntity(movingSunArchetype);
        entityManager.AddComponentData<Translation>(entity,
                new Translation
                {
                    Value = new float3(-1, 15, -2)
                }
            );
        entityManager.AddSharedComponentData(entity,
                new RenderMesh
                {
                    mesh = unitMesh,
                    material = unitMaterials[0]
                }
            );
        entityManager.AddComponentData<Scale>(entity,
                new Scale
                {
                    Value = centerScaleToSpawn
                }
            );
        entityManager.AddComponentData<GravityEmitter>(entity,
                new GravityEmitter
                {
                    mass = 10000,
                    radius = galaxyCenterRadius
                }
            );
        entityManager.AddComponentData<NewtonBody>(entity,
        new NewtonBody
        {
            accel = new float3(0),
            velocity = new float3(0, -0.5f, 0),
            force = new float3(0),
            mass = 100,
        }
        );
        entities.Add(entity);

        //----------------1 STAR

        entity = entityManager.CreateEntity(sunArchetype);
        entityManager.AddComponentData<Translation>(entity,
                new Translation
                {
                    Value = new float3(0, 0, 0),
                }
            );
        entityManager.AddSharedComponentData(entity,
                new RenderMesh
                {
                    mesh = unitMesh,
                    material = unitMaterials[0]
                }
            );
        entityManager.AddComponentData<Scale>(entity,
                new Scale
                {
                    Value = centerScaleToSpawn
                }
            );
        entityManager.AddComponentData<GravityEmitter>(entity,
                new GravityEmitter
                {
                    mass = 10000,
                    radius = galaxyCenterRadius
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

        var pointsToSpawn = GetGalaxyPointsToSpawn(armsCount, 0, new float3(0,0,0), 20f, 5f, 0.1f, 0f);
        for(var  i = 0; i < pointsToSpawn.Count; i++)
        {
            if( i % 3 == 0)
            {
                var distance = UnityEngine.Random.Range(satelliteSpawnRange.x, satelliteSpawnRange.y);
                var pointToSpawn = Float3Helper.NormalizeFloat3(new float3(UnityEngine.Random.Range(-satelliteSpawnRange.y, satelliteSpawnRange.y),
                                                UnityEngine.Random.Range(-satelliteSpawnRange.y, satelliteSpawnRange.y) / 10,
                                                UnityEngine.Random.Range(-satelliteSpawnRange.y, satelliteSpawnRange.y))) * distance;
                var satellite = SpawnSatellite(pointsToSpawn[i], entity, planetArchetype, starScaleToSpawn *((float) rand.NextDouble()) * 2 + starScaleToSpawn, 
                    rand.Next(starMatIndexies.x, starMatIndexies.y + 1));
                entities.Add(satellite);
            }
            else
            {
                var distance = UnityEngine.Random.Range(satelliteSpawnRange.x, satelliteSpawnRange.y);
                var pointToSpawn = Float3Helper.NormalizeFloat3(new float3(UnityEngine.Random.Range(-satelliteSpawnRange.y, satelliteSpawnRange.y),
                                                UnityEngine.Random.Range(-satelliteSpawnRange.y, satelliteSpawnRange.y) / 10,
                                                UnityEngine.Random.Range(-satelliteSpawnRange.y, satelliteSpawnRange.y))) * distance;
                var satellite = SpawnSatellite(pointsToSpawn[i], entity, planetArchetype, nebulaScaleToSpawn, rand.Next(nebulasrMatIndexies.x, nebulasrMatIndexies.y + 1));
                entities.Add(satellite);
            }

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
        //print($"orbitalSpeed:{orbitalSpeed}");
        var plane = new Plane(satelliteTranslation.Value - emitterTranslation.Value, satelliteTranslation.Value);
        Debug.DrawLine(satelliteTranslation.Value, satelliteTranslation.Value + Float3Helper.FromVector3(plane.normal), Color.magenta);
        var orbitalForward = Vector3.Cross(Vector3.up, satelliteTranslation.Value - emitterTranslation.Value).normalized;
        var velocity = orbitalForward.normalized * orbitalSpeed;
        Debug.DrawLine(satelliteTranslation.Value, satelliteTranslation.Value + Float3Helper.FromVector3(velocity), Color.green);

        //print($"Spawn velocity: {velocity.magnitude}");
        return velocity;
    }

    public Entity SpawnSatellite(float3 pointToSpawn, Entity mainBody, EntityArchetype archetype, float scale, int matIndex)
    {

        var satellite = entityManager.CreateEntity(archetype);
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
                    material = unitMaterials[matIndex]
                }
            );
        entityManager.AddComponentData<Scale>(satellite,
                new Scale
                {
                    Value = scale
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

    //private void Update()
    //{
    //    foreach(var emitter in gravEmitters)
    //    {
    //        var gravityEmitter = entityManager.GetComponentObject<GravityEmitter>(emitter);
    //        var translationEmitter = entityManager.GetComponentObject<Translation>(emitter);
    //        foreach (var receivers in gravReceivers)
    //        {
    //            var bodyReceiver = entityManager.GetComponentObject<NewtonBody>(receivers);
    //            var translationReceiver = entityManager.GetComponentObject<Translation>(receivers);



    //            var direction = (translationEmitter.Value - translationReceiver.Value);
    //            var distance = Float3Helper.GetMagnitudeFloat3(direction);
    //            if (distance < gravityEmitter.radius) return;
    //            var accelMagnitude = GravitySystem.Gscaled * gravityEmitter.mass / Mathf.Pow(distance, 2);
    //            var accel = Float3Helper.NormalizeFloat3(direction) * accelMagnitude;
    //            bodyReceiver.accel += accel;

    //        }
    //    }
    //}

    List<float3> GetGalaxyPointsToSpawn(int armsCount, int pointsCount, float3 center, float galaxySize, float flatFactor, float armThickness, float angleFactor)
    {
        var pointToSpawn = new List<float3>();
        var ai = galaxyCenterRadius; //initial radius
        var af = galaxySize; //final radius
        var phi = Linspace(0, 2 * Mathf.PI, 220); //angle in radians
        var b = (af - ai) / (2 * Mathf.PI * armsCount); //spiralGrowFactor
        //360 stars on arm now NEED TO FIX IT!!11
        for (var arm = 0; arm < armsCount; arm++ )
        {
            for (var i = 0; i < phi.Count; i++)
            {
                var x = (ai + b * phi[i]) * Mathf.Cos(phi[i] + (Mathf.PI * 2 / armsCount) * arm);
                var z = (ai + b * phi[i]) * Mathf.Sin(phi[i] + (Mathf.PI * 2 / armsCount) * arm);
                var currentVector = new float3(x, 0, z);
                var distance = Float3Helper.GetMagnitudeFloat3(center - currentVector);
                var randomizingVector = new float3(UnityEngine.Random.Range(-distance * armThickness, distance * armThickness),
                                                     UnityEngine.Random.Range(-distance * armThickness, distance * armThickness)/ flatFactor,
                                                     UnityEngine.Random.Range(-distance * armThickness, distance * armThickness)
                                                     );
                currentVector += randomizingVector;
                pointToSpawn.Add(currentVector);
                //Need to fix randomizing
            }
        }

        
        return pointToSpawn;
    }


    List<float> Linspace(float start, float stop, int count)
    {
        var step = (stop - start) / count;
        var linspace = new List<float>();
        for (var i = 0; i < count; i++)
            linspace.Add(start + step * i);
        return linspace;
    }

    float GetNormalRandom()
    {
        var u1 = 1.0f - (float)rand.NextDouble(); //uniform(0,1] random doubles
        var u2 = 1.0f - (float)rand.NextDouble();
        var randStdNormal = Mathf.Sqrt(-2.0f * Mathf.Log(u1, 2.7f)) *
                     Mathf.Sin(2.0f * Mathf.PI * u2); //random normal(0,1)
        return randStdNormal;
    }
}
