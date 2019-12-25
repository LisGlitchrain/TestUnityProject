using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipUp : MonoBehaviour
{
    delegate bool Action();
    Action action = null;
    public GameObject obj;
    public float angularSpeed = 50f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        action?.Invoke();
    }

    public void Flip()
    {
        action = Flipping;
        obj.GetComponent<Rigidbody>().useGravity = false;
    }

    bool Flipping()
    {
        Plane plane = new Plane(Vector3.up, obj.transform.up, Vector3.zero);
        float angle = Vector3.SignedAngle(Vector3.up, obj.transform.up, plane.normal);
        if(Mathf.Abs(angle) > 5)
        {
            obj.transform.Rotate(plane.normal, angularSpeed * Mathf.Sign(angle) * Time.deltaTime);
            return false;
        }
        else
        {
            obj.GetComponent<Rigidbody>().useGravity = true;
            action = null;
            return true;
        }
    }

}
