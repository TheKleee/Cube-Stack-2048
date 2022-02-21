using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;

public enum CubeType
{
    c2,
    c4,
    c8,
    c16,
    c32
}
public class Cube : MonoBehaviour
{

    public bool isComplete { get; set; }
    [Header("Main Data:"), SerializeField]
    CubeType cType;
    [Space]
    public Cube nextCube;
    [HideInInspector] public int id;

    private void Awake()
    {
        switch (cType)
        {
            case CubeType.c2:
                id = 2;
                nextCube.id = 4;
                break;

            case CubeType.c4:
                id = 4;
                nextCube.id = 8;
                break;

            case CubeType.c8:
                id = 8;
                nextCube.id = 16;
                break;

            case CubeType.c16:
                id = 16;
                nextCube.id = 32;
                break;

            case CubeType.c32:
                id = 32;
                nextCube.id = 32;
                break;
        }
    }

    public void SetTarget(Transform target)
    {
        LeanTween.move(gameObject, target.position, 0.25f)/*.setEaseOutBounce()*/;
    }

    private void OnCollisionEnter(Collision cube)
    {
        if (!isComplete)
        {
            if(cube.transform.GetComponent<Cube>() != null)
            {
                var c = cube.transform.GetComponent<Cube>();
                if (c.id == id)
                {
                    //combine;
                    //isComplete = true;
                    GameController.instance.CheckCollisions(this);
                }
            }
        }
    }
}
