using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetConveyor : MonoBehaviour
{

    public Transform startTrans;
    public Transform endTrans;
    // Start is called before the first frame update
    void Start()
    {
        InstantiateConveyorLine(startTrans.position, endTrans.position);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void InstantiateConveyorLine(Vector3 startPos, Vector3 endPos)
    {
        //GameObject obj = Instantiate(conveyorPrefab);
        transform.position = startPos + ((endPos - startPos) / 2);
        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, (startPos - endPos).magnitude);
        Vector3 temp = new Vector3(endPos.x, endPos.y, endPos.z);
        temp.y = startPos.y;
        float angle = Vector3.SignedAngle((temp - startPos), (endPos - startPos), -Vector3.right);
        transform.Rotate(Vector3.right, -angle);
    }
}
