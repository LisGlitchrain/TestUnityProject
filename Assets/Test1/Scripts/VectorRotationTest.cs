using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VectorRotationTest : MonoBehaviour
{
    public Transform setPoint;
    public bool drawRayObj;
    public bool drawRaySetPoint;
    public bool drawRaySum;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (drawRayObj) Debug.DrawLine(Vector3.zero, setPoint.position - transform.rotation * setPoint.localPosition, Color.green);
        if (drawRaySetPoint) Debug.DrawLine(transform.position, setPoint.localPosition, Color.yellow);
        if (drawRaySum) Debug.DrawLine(Vector3.zero, transform.rotation * setPoint.localPosition + transform.position, Color.red);
    }
}
