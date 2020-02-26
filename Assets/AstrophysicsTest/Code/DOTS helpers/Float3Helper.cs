using UnityEngine;
using Unity.Mathematics;

public static class Float3Helper
{
    public static float GetMagnitudeFloat3(float3 v) =>
    Mathf.Sqrt(v.x * v.x + v.y * v.y + v.z * v.z);

    public static float3 NormalizeFloat3(float3 v) =>
        v / GetMagnitudeFloat3(v);

    public static Vector3 ToVector3(float3 v) =>
        new Vector3(v.x, v.y, v.z );

    public static float3 FromVector3(Vector3 v) =>
        new float3(v.x, v.y, v.z);
}
