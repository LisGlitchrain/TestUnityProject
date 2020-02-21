using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MonoBehImplementingInterface : ISomeInterface
{
    [SerializeField]
    public float someValue;
    public float SomeValue { get => someValue; set => someValue = value; }

    public void DoStuff(int num)
    {
        throw new System.NotImplementedException();        
    }

    public void DoStuff2(int str)
    {
        throw new System.NotImplementedException();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
