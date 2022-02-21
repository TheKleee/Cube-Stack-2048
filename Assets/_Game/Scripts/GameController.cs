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


    public List<Cube> craftCube = new List<Cube>();
    Cube mainCube;
    //bool newBall;

    #region Create Balls:
    public void CheckCollisions(Cube b)
    {
        if (craftCube.Count < 2)
            craftCube.Add(b);
        if (craftCube.Count == 2)
            BallCrafter();
    }

    void BallCrafter()
    {
        //for (int i = 0; i < craftBalls.Count; i++)
        //{
        //    newBall = true;
        //    //Store the data of the last ball
        //}
        ////Create a new ball instead of the stored two
        //if (newBall)
            mainCube = craftCube[0].nextCube;
        int id = 0;
        switch (mainCube.id) 
        {
            case 2:
                id = 0;
                break;

            case 4:
                id = 1;
                break;

            case 8:
                id = 2;
                break;

            case 16:
                id = 3;
                break;

            case 32:
                id = 4;
                break;
        }
        SpawnCube(id, null, craftCube[0].transform.parent);

        //craftCube.Remove(mainCube);

        for (int i = 0; i < craftCube.Count; i++)
            Destroy(craftCube[i].gameObject);
        craftCube.Clear();
    }
    #endregion

    public void SpawnCube(int cubeID, Transform target, Transform spawnPos = null)
    {
        if (spawnPos == null)
        {
            Cube c = Instantiate(cubes[cubeID], spawnPoint.position, cubes[cubeID].transform.rotation);
            c.transform.SetParent(target);
            c.SetTarget(target);
        }
        else
        {
            Cube c = Instantiate(cubes[cubeID], spawnPos.position, cubes[cubeID].transform.rotation);
            c.transform.SetParent(spawnPos);
        }
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
