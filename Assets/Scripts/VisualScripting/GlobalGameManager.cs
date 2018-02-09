using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class GlobalGameManager : ScriptableObject
{
    public void printInfo(string value)
    {
        Debug.Log(value);
    }
}
