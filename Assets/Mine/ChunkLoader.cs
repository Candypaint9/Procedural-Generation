using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkLoader : MonoBehaviour
{
    List<Vector3> spawnedChunks = new List<Vector3>();
    List<Vector3> availableChunks = new List<Vector3>();
    List<Vector3> pos = new List<Vector3>();


    Vector3 playerPos;

    public GameObject ChunkObject;

    int renderDist = 7;
    int Size = 64;

    GameObject tempObject;


    void Start()
    {
        spawnedChunks.Add(new Vector3(0, 0, 0));
    }

    void Update()
    {
        getAvailableChunks();
    }



    void getAvailableChunks()
    {
        playerPos = transform.parent.position;

        int xCentre = System.Math.DivRem(Mathf.RoundToInt(playerPos.x), Size, out int remainder) * Size;
        int zCentre = System.Math.DivRem(Mathf.RoundToInt(playerPos.z), Size, out int remainder2) * Size;

        for (int i = 1; i <= renderDist; i++)
        {
            int offset = i * Size;

            // variables for corners of chunk around the previous layers
            Vector3 rightTop = new Vector3(xCentre + offset, 0, zCentre + offset);
            Vector3 rightBottom = new Vector3(xCentre + offset, 0, zCentre - offset);
            Vector3 leftTop = new Vector3(xCentre - offset, 0, zCentre + offset);
            Vector3 leftBottom = new Vector3(xCentre - offset, 0, zCentre - offset);

            pos.AddRange(new List<Vector3>
            {
                // getting corners first
                rightTop,   
                rightBottom,
                leftTop,
                leftBottom
            });

            for (int j = 1; j <= (1 + (2 * (i - 1))); j++)
            {
                int offset2 = j * Size;

                // using corners to get sides by going downwards
                pos.AddRange(new List<Vector3>
                {
                    rightTop - new Vector3(0, 0, offset2),            //downwards
                    leftTop - new Vector3(0, 0, offset2),           //downwards
                    leftTop + new Vector3(offset2, 0, 0),          //sideways
                    leftBottom + new Vector3(offset2, 0, 0),          //sideways
                });
            }
        }

        addToAvailableChunks(pos);
    }


    void addToAvailableChunks(List<Vector3> posList)
    {
        foreach (Vector3 position in posList)
        {
            if (!spawnedChunks.Contains(position))
            {
                availableChunks.Add(position);
            }
        }

        pos.Clear();

        SpawnChunk();
    }


    void SpawnChunk()
    {
        foreach (Vector3 position in availableChunks)
        {
            Instantiate(ChunkObject, position, Quaternion.identity);
            spawnedChunks.Add(position);
        }
        availableChunks.Clear();
    }
}
