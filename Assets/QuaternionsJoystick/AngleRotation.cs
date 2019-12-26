using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngleRotation : MonoBehaviour
{
    public Vector3 rotation;
    public GameObject joyBase;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        joyBase.transform.localRotation = Quaternion.identity;
        joyBase.transform.Rotate(rotation.x, rotation.y, 0);
        joyBase.transform.Rotate(Vector3.forward, rotation.z); //transform.position + joyBase.transform.forward //LOCAL ROTATION!
        Debug.DrawLine(joyBase.transform.position, joyBase.transform.position + joyBase.transform.forward, Color.magenta);
        Debug.DrawLine(joyBase.transform.position, joyBase.transform.position + Quaternion.Inverse(joyBase.transform.rotation) * joyBase.transform.forward, Color.green);
    }
}
