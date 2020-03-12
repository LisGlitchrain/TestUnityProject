using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomTest : MonoBehaviour
{
    public int min = -6;
    public int max = 17;

    public void RandomRange()
    {
        print($"{Random.Range(min, max) * 0.5f}");
    }
}
