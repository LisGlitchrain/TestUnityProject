using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISomeInterface
{
    //float someValue; - Interface can declare properties but not variables
    float SomeValue { get; set; }
    //void DoStuff(int num);
    //void DoStuff2(int str);
}
