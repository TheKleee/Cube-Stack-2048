using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Indicators : MonoBehaviour
{
    [Header("Spanw ID:")]
    public int spawnID;

    public int cubeID;

    private void Start()
    {
        cubeID = (int)Mathf.Pow(2, spawnID + 1);
    }
}
