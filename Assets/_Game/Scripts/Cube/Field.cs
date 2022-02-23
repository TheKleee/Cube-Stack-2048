using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour
{
    [Header("Field ID")]
    public int id;
    [Space, SerializeField]
    Vector3 cubeLocalRotation;
    public bool isComplete { get; private set; }
    public void LockField()
    {
        isComplete = true;
        GetComponent<BoxCollider>().enabled = false;
    }

    private void OnTriggerEnter(Collider cube)
    {
        if (cube.GetComponent<Cube>() != null)
        {
            var c = cube.transform.GetComponent<Cube>();
            c.die = false;
            RotateCube(c.transform);
            if (c.id == id && !c.isComplete)
            {
                c.isComplete = true;
                c.transform.GetChild(0).gameObject.SetActive(false);
                LockField();
            }
        }
    }

    void RotateCube(Transform cube)
    {
        cube.localEulerAngles = cubeLocalRotation;
    }
}
