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
    bool isActive;
    [Header("Main Data:"), SerializeField]
    CubeType cType;
    
    [HideInInspector] public int id;

    private void Start()
    {
        switch (cType)
        {
            case CubeType.c2:
                id = 2;
                break;

            case CubeType.c4:
                id = 4;
                break;

            case CubeType.c8:
                id = 8;
                break;

            case CubeType.c16:
                id = 16;
                break;

            case CubeType.c32:
                id = 32;
                break;
        }
    }

    public void SetTarget(Transform target)
    {
        LeanTween.move(gameObject, target.position, 0.25f).setEaseOutBack();
        Timing.RunCoroutine(_ActiveDelay().CancelWith(gameObject));
    }

    IEnumerator<float> _ActiveDelay()
    {
        yield return Timing.WaitForSeconds(.25f);
        isActive = false;
    }

    private void OnCollisionEnter(Collision cube)
    {
        if (!isComplete)
        {
            if (cube.transform.CompareTag("Field") && cube.collider.isTrigger)
            {
                var f = cube.transform.GetComponent<Field>();
                if (f.id == id)
                {
                    f.LockField();
                    return;
                }
            }

            if(cube.transform.GetComponent<Cube>() != null && isActive)
            {
                var c = cube.transform.GetComponent<Cube>();
                if (c.id == id)
                {
                    //combine;
                    int cubeID = 0;
                    switch (cType)
                    {
                        default:
                            break;

                        case CubeType.c4:
                            cubeID = 1;
                            break;

                        case CubeType.c8:
                            cubeID = 2;
                            break;

                        case CubeType.c16:
                            cubeID = 3;
                            break;

                        case CubeType.c32:
                            cubeID = 4;
                            break;
                    }
                    GameController.instance.SpawnCube(cubeID, null, transform.parent);
                    Destroy(c.gameObject);
                    Destroy(gameObject);
                }
            }
        }
    }
}
