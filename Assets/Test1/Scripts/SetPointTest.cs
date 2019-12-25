using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetPointTest : MonoBehaviour
{
    public Transform setPoint;
    public Transform setPointWorld;
    float rotationDiff;
    public float rotationSmoother = 600f;
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
        transform.position = Vector3.Lerp(setPoint.position,
                                        setPointWorld.position,
                                        0.05f) - transform.rotation * setPoint.localPosition; // * 1.05f //processor.transform.rotation *
        rotationDiff = (transform.rotation.eulerAngles - setPointWorld.transform.rotation.eulerAngles).magnitude;
       transform.rotation = Quaternion.Lerp(transform.rotation,
                                                    setPointWorld.rotation,
                                                    rotationDiff / rotationSmoother);
        print($"Magnitude {(transform.position + transform.rotation * setPoint.localPosition - setPointWorld.position).magnitude}");
        if ((setPoint.position - setPointWorld.position).magnitude < 0.1f)
        {
            Debug.Log("DONE!");
            transform.position = setPointWorld.position - transform.rotation * (setPoint.localPosition);
            transform.rotation = setPointWorld.rotation;
        }

        if (drawRayObj) Debug.DrawLine(Vector3.zero, setPoint.position - transform.rotation * setPoint.localPosition, Color.green);
        if (drawRaySetPoint) Debug.DrawLine(transform.position, setPoint.localPosition, Color.yellow);
        if (drawRaySum) Debug.DrawLine(Vector3.zero, transform.rotation * setPoint.localPosition + transform.position, Color.red);
    }
}
