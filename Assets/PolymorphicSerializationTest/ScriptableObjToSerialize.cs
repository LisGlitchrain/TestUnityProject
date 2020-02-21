using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "New ScriptableWithINterfaceToSerialize", menuName = "Scriptables/Serialized interface within scriptable", order = 51)]
class ScriptableObjToSerialize: ScriptableObject
{
    [SerializeReference]
    public ISomeInterface someInterface;
    [SerializeReference] 
    List<ISomeInterface> listOfInterface;
    public float valval;
}


