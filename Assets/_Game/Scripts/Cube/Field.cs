using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour
{
    [Header("Field ID")]
    public int id;
    public bool isComplete { get; private set; }
    public void LockField()
    {
        isComplete = true;
        GetComponent<BoxCollider>().enabled = false;
    }
}
