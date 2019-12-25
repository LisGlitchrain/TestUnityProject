using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceInFrontOfMainCamera : MonoBehaviour
{
    public float distance = 1f;
    public Vector2 offset;
    Camera camera;
    public bool freezeX;
    public bool freezeY;
    // Start is called before the first frame update
    void Start()
    {
        camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        //transform.position = camera.transform.position + camera.transform.forward * distance + camera.transform.right * offset.x + camera.transform.up * offset.y;


# region OPTION 1 
        //yaw => pitch => roll

        //Quaternion rot = camera.transform.rotation; //variable just for shorten code lines
        //transform.localRotation = Quaternion.AngleAxis(rot.eulerAngles.y, Vector3.up); //set yaw

        //Quaternion temp = Quaternion.AngleAxis(rot.eulerAngles.x, Vector3.right); //get pitch rotation
        //transform.localRotation = temp * transform.localRotation; //set pitch

        //temp = Quaternion.AngleAxis(-rot.eulerAngles.z, Vector3.forward); //get roll rotation
        //transform.localRotation = temp * transform.localRotation;//set roll
#endregion

#region OPTION 2
        //pitch => yaw => roll

        //Quaternion rot = camera.transform.rotation; //variable just for shorten code lines
        //transform.localRotation = Quaternion.AngleAxis(rot.eulerAngles.x, Vector3.right); //set pitch

        //Quaternion temp = Quaternion.AngleAxis(rot.eulerAngles.y, transform.up); //get yaw rotation AROUND SELF UP-AXIS
        //transform.localRotation = temp * transform.localRotation; //set pitch

        //temp = Quaternion.AngleAxis(-rot.eulerAngles.z, Vector3.forward); //get roll rotation
        //transform.localRotation = temp * transform.localRotation;//set roll
#endregion

#region OPTION 3
        //roll => pitch => yaw


        Quaternion rot = camera.transform.rotation; //variable just for shorten code lines

        transform.localRotation = Quaternion.AngleAxis(-rot.eulerAngles.z, Vector3.forward); //set roll

        Quaternion temp = Quaternion.AngleAxis(rot.eulerAngles.x, transform.right);
        transform.localRotation = temp * transform.localRotation;//set pitch

        temp = Quaternion.AngleAxis(rot.eulerAngles.y, transform.up); //get yaw rotation AROUND SELF UP-AXIS
        transform.localRotation = temp * transform.localRotation; //set yaw

         //get roll rotation
#endregion

    }
}
