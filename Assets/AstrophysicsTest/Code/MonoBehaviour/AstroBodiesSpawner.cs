using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstroBodiesSpawner : MonoBehaviour
{
    public GameObject[] prefabs;
    public int solarSystemsToSpawn;
    public float boundX;
    public float boundY;
    public float boundZ;
    System.Random rand;
    public Transform parentToSpawned;
    public bool onStart;
    GravitationManager gravitationManager;
    public int maxPlanetCount;
    public float solarSystemMaxRadius;

    // Start is called before the first frame update
    void Start()
    {
        gravitationManager = FindObjectOfType<GravitationManager>();
        if(onStart)
        {
            SpawnMainMethod();
        }
    }

    public void SpawnMainMethod()
    {
        rand = new System.Random();
        for (var i = 0; i < solarSystemsToSpawn; i++)
            SpawnSolarSystem();
        print($"Physics enabled");
        FindObjectOfType<GravitationManager>().physicsEnabled = true;
    }

    public void SpawnSolarSystem()
    {
        var point = new Vector3(Random.Range(-boundX, boundX), Random.Range(-boundY, boundY), Random.Range(-boundZ, boundZ));
        var sun = SpawnSingle(point,0, parentToSpawned, StartVelocityType.Predefined);

        for(var i = 0; i < maxPlanetCount; i++)
        {
            var distance = Random.Range(0.5f, solarSystemMaxRadius);
            point = new Vector3(Random.Range(-solarSystemMaxRadius, solarSystemMaxRadius), Random.Range(-solarSystemMaxRadius, solarSystemMaxRadius) / 10, Random.Range(-solarSystemMaxRadius, solarSystemMaxRadius));
            point = point.normalized * distance;
            var indexToSpawn = rand.Next(1, prefabs.Length);
            SpawnSatellite(point, indexToSpawn, sun.GetComponent<AstroRigidbody>(), StartVelocityType.Orbital);
        }
    }

    public void SpawnRandom()
    {
        for(var i = 0; i < solarSystemsToSpawn; i++)
        {
            var index = rand.Next(0, prefabs.Length);
            var point = new Vector3(Random.Range(-boundX, boundX), Random.Range(-boundY, boundY), Random.Range(-boundZ, boundZ));
            SpawnSingle(point, index, parentToSpawned);
        }
    }

    public GameObject SpawnSingle(Vector3 pointToSpawn, int prefabIndex, Transform parentToSpawned, StartVelocityType type = StartVelocityType.Random)
    {
        var body = Instantiate(prefabs[prefabIndex], parentToSpawned);
        body.transform.position = pointToSpawn;
        if (type == StartVelocityType.Random)
            body.GetComponent<StartVelocity>().ApplyVelocityRandom();
        else if (type == StartVelocityType.None)
            body.GetComponent<AstroRigidbody>().FreezePos = true;
        print($"SinglePlanetSpawned!");
        return body;
    }

    public GameObject SpawnSatellite(Vector3 pointToSpawn, int prefabIndex, AstroRigidbody mainBody, StartVelocityType type = StartVelocityType.Random)
    {
        var body = Instantiate(prefabs[prefabIndex], mainBody.gameObject.transform);
        body.transform.position = mainBody.transform.position + pointToSpawn * mainBody.transform.localScale.x;
        if (type == StartVelocityType.Random)
            body.GetComponent<StartVelocity>().ApplyVelocityRandom();
        else if (type == StartVelocityType.Orbital)
        {
            var orbitalSpeed = CalculateOrbitalSpeed(mainBody, body.GetComponent<AstroRigidbody>());
            Debug.DrawLine(body.transform.position, body.transform.position + Vector3.Cross(Vector3.up, body.transform.position - mainBody.Position).normalized, Color.green);
            Debug.DrawLine(Vector3.zero, Vector3.Cross(Vector3.up, body.transform.position - mainBody.Position).normalized, Color.green);
            var plane = new Plane(body.transform.position - mainBody.Position, body.transform.position);
            Debug.DrawLine(body.transform.position, body.transform.position + plane.normal, Color.magenta);
            var orbitalForward = Vector3.Cross(Vector3.up, body.transform.position - mainBody.Position).normalized;
            var velocity = orbitalForward.normalized * orbitalSpeed;
            print($"Spawn velocity: {velocity.magnitude}");
            body.GetComponent<StartVelocity>().ApplyVelocity(velocity);
        }
        print($"SatelliteSpawned!");
        return body;
    }

    public float CalculateOrbitalSpeed(AstroRigidbody mainBody, AstroRigidbody satellite)
    {
        var speed = Mathf.Sqrt( mainBody.Mass * GravitationManager.Gscaled / ((mainBody.Position - satellite.Position).magnitude) );
        return speed;
    }
}
