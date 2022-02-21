using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    #region Singleton
    public static GameController instance;
    private void Awake()
    {
        instance = this;
    }
    #endregion

    public bool end { get; set; }

    [Header("Spawn Point:"), SerializeField]
    Transform spawnPoint;

    [Header("Cubes:"), SerializeField]
    Cube[] cubes;   //2, 4, 8, 16, 32

    [Header("Fruit:"), SerializeField]
    GameObject fruit;
    [Space]
    GameObject outline;

    [Header("Outline Cubes:"), SerializeField]
    Field[] outlineCubes;    //The ones that make out the fruit oultine :D
    

    public void SpawnCube(int cubeID, Transform target, Transform spawnPos = null)
    {
        if (spawnPos == null)
        {
            Cube c = Instantiate(cubes[cubeID], spawnPoint.position, cubes[cubeID].transform.rotation);
            c.SetTarget(target);
        }
        else
            Instantiate(cubes[cubeID], spawnPos.position, cubes[cubeID].transform.rotation);
    }

    public void CheckOutline()
    {
        bool isComplete = true;
        for (int i = 0; i < outlineCubes.Length; i++)
        {
            if (!outlineCubes[i].isComplete)
            {
                isComplete = false;
                break;
            }
        }
        if (isComplete) GameEnded();
    }

    public void GameEnded()
    {
        end = true;
        outline.SetActive(false);
        fruit.SetActive(true);
    }
}
