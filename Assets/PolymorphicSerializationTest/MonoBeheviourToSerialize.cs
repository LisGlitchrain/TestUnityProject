using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class MonoBeheviourToSerialize : MonoBehaviour
{
    [SerializeReference] ISomeInterface someInterface;
    [SerializeReference] List<ISomeInterface> listOfInterface;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        if (someInterface == null)
        {
            someInterface = new MonoBehImplementingInterface();
            Debug.Log("Created object implemented interface!");
        }
        else
        {
            Debug.Log("Read object implemented interface!");
        }

        if (listOfInterface == null)
        {
            listOfInterface = new List<ISomeInterface>()
            {
                new MonoBehImplementingInterface(),
                new MonoBehImplementingInterface()
            };
            listOfInterface[0].SomeValue = 1;
            listOfInterface[1].SomeValue = 2;
            Debug.Log("Created list with objects implemented interface!");
        }
        else
        {
            Debug.Log("Read list with objects implemented interface!");
        }

    }
}
