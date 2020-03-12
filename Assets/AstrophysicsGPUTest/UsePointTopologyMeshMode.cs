using UnityEngine;

//[ExecuteInEditMode]
public class UsePointTopologyMeshMode : MonoBehaviour
{

    [ContextMenu("ConvertToPointTopology")]
    void ConvertToPointTopology()
    {
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        print($"Name: {gameObject.name} mesh: {mesh.name} vertices: {mesh.vertices.Length}");
        int[] indices = new int[mesh.vertices.Length];
        for (int i = 0; i < indices.Length; i++)
        {
            indices[i] = i;
        }
        mesh.SetIndices(indices, MeshTopology.Points, 0);
    }

    // Use this for initialization
    void Start()
    {
        ConvertToPointTopology();
    }

    private void OnEnable()
    {
        ConvertToPointTopology();
    }
}