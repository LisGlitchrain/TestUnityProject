using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputeShaderManager : MonoBehaviour
{
    public ComputeShader computeShader;
    public ComputeBuffer computeBuffer;
    public Vector3 position;
    public Vector3 position2;
    public Vector3 velocity;
    public Vector3 acceleration;
    public float mass;
    public NewtonBody newtonBody;
    public NewtonBody[] newtonBodies;
    public GameObject gravityCenter;
    public float gravityCenterMass = 100;
    public GameObject galaxy;
    Renderer renderer;
    public Shader shader;
    public Material material;
    public Bounds bounds;

    int kernel;
    public struct float3
    {
        public float x, y, z;
    }

    public struct NewtonBody
    {
        public float3 position;
        public float3 acceleration;
        public float3 velocity;
        public float mass;
    };

    // Start is called before the first frame update
    void Start()
    {
        newtonBodies = new NewtonBody[2];

        newtonBodies[0].position = new float3
        {
            x = position.x,
            y = position.y,
            z = position.z
        };
        newtonBodies[0].velocity = new float3
        {
            x = velocity.x,
            y = velocity.y,
            z = velocity.z
        };
        newtonBodies[0].acceleration = new float3
        {
            x = acceleration.x,
            y = acceleration.y,
            z = acceleration.z
        };
        newtonBodies[0].mass = 100;

        newtonBodies[1].position = new float3
        {
            x = position2.x,
            y = position2.y,
            z = position2.z
        };
        newtonBodies[1].velocity = new float3
        {
            x = -1,
            y = 0,
            z = 0
        };
        newtonBodies[1].acceleration = new float3
        {
            x = acceleration.x,
            y = acceleration.y,
            z = acceleration.z
        };
        newtonBodies[1].mass = 100;
        bounds = new Bounds(galaxy.transform.position, new Vector3(50, 50, 50));

        unsafe
        {
            computeBuffer = new ComputeBuffer(newtonBodies.Length, sizeof(NewtonBody));
        }

        kernel = computeShader.FindKernel("CSMain");
        //kernel = 0;
        computeBuffer.SetData(newtonBodies);

        computeShader.SetFloat(Shader.PropertyToID("gravityCenterX"), gravityCenter.transform.position.x);
        computeShader.SetFloat(Shader.PropertyToID("gravityCenterY"), gravityCenter.transform.position.y);
        computeShader.SetFloat(Shader.PropertyToID("gravityCenterZ"), gravityCenter.transform.position.z);
        computeShader.SetFloat(Shader.PropertyToID("gravityCenterMass"), gravityCenterMass);

        computeShader.SetFloat(Shader.PropertyToID("gConstant"), 0.00664f);
        computeShader.SetBuffer(kernel, Shader.PropertyToID("Result"), computeBuffer);

        shader = Shader.Find("Stars/Unlit4");
        galaxy.GetComponent<Renderer>().material.shader = shader;
        //Shader.SetGlobalBuffer("NewtonBodiesGlobal", computeBuffer);
        renderer = galaxy.GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        computeShader.SetFloat(Shader.PropertyToID("deltaTime"), Time.deltaTime);

        computeShader.Dispatch(kernel, newtonBodies.Length / 2, 1, 1);
        Graphics.DrawProceduralIndirect(material, bounds, MeshTopology.Points, computeBuffer);

        computeBuffer.GetData(newtonBodies);
        position = new Vector3(newtonBodies[0].position.x, newtonBodies[0].position.y, newtonBodies[0].position.z);
        position2 = new Vector3(newtonBodies[1].position.x, newtonBodies[1].position.y, newtonBodies[1].position.z);


    }

}
