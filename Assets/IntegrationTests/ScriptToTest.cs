﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ScriptToTest : MonoBehaviour
{
    public float speed; 

    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.forward * Time.deltaTime * speed;
    }
}
