using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuaternionRotation : MonoBehaviour
{
    public float angularSpeed;

    // Start is called before the first frame update
    void Start()
    {
        //print($"Rot yaw -90 {Quaternion.AngleAxis(-90, Vector3.up)}");
        //print($"Rot yaw  90 {Quaternion.AngleAxis(90, Vector3.up)}");
        //print($"Rot pit -90 {Quaternion.AngleAxis(-90, Vector3.right)}");
        //print($"Rot pit  90 {Quaternion.AngleAxis(90, Vector3.right)}");
        //print($"Rot rol  90 {Quaternion.AngleAxis(90, Vector3.forward)}");
        //print($"Rot rol -90 {Quaternion.AngleAxis(-90, Vector3.forward)}");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Q)) RotateYawLeft();
        if (Input.GetKey(KeyCode.W)) RotatePitchDown();
        if (Input.GetKey(KeyCode.S)) RotatePitchUp();
        if (Input.GetKey(KeyCode.E)) RotateYawRight();
        if (Input.GetKey(KeyCode.A)) RotateRollClockwise();
        if (Input.GetKey(KeyCode.D)) RotateRollCounterClockwise();
    }

    void RotateYawLeft()
    {

        Quaternion nextRotation = Quaternion.AngleAxis( - angularSpeed * Time.deltaTime, transform.up);

        nextRotation = nextRotation * transform.rotation;
        transform.rotation = nextRotation;
    }

    void RotateYawRight()
    {

        Quaternion nextRotation = Quaternion.AngleAxis(angularSpeed * Time.deltaTime, transform.up);

        nextRotation = nextRotation * transform.rotation;
        transform.rotation = nextRotation;
    }

    void RotatePitchUp()
    {

        Quaternion nextRotation = Quaternion.AngleAxis(- angularSpeed * Time.deltaTime, transform.right);

        nextRotation = nextRotation * transform.rotation;
        transform.rotation = nextRotation;
    }

    void RotatePitchDown()
    {

        Quaternion nextRotation = Quaternion.AngleAxis( angularSpeed * Time.deltaTime, transform.right);

        nextRotation = nextRotation * transform.rotation;
        transform.rotation =  nextRotation;
    }

    void RotateRollClockwise()
    {

        Quaternion nextRotation = Quaternion.AngleAxis( angularSpeed * Time.deltaTime, transform.forward);

        nextRotation = nextRotation * transform.rotation;
        transform.rotation = nextRotation;
    }

    void RotateRollCounterClockwise()
    {

        Quaternion nextRotation = Quaternion.AngleAxis( - angularSpeed * Time.deltaTime, transform.forward);

        nextRotation = nextRotation * transform.rotation;
        transform.rotation = nextRotation;
    }
}
