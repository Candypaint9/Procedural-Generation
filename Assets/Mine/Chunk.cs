using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    Mesh mesh;

    Vector3[] vertices;
    int[] triangles;

    int Size = 64;
    int renderDist = 7;

    public GameObject Parent;

    int seed = 01983;
    float xOffset;
    float yOffset;

    GameObject Player;
    Vector3 playerPos;


    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");

        Random.InitState(seed);
        xOffset = Random.value * 10;
        yOffset = Random.value * 10;

        Parent.transform.position -= new Vector3(Size / 2, 0, Size / 2);


        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        if (true)
        {
            mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        }

        CreateShape();
        UpdateMesh();
        GetComponent<MeshCollider>().sharedMesh = mesh;
    }

    void Update()
    {
        playerPos = Player.transform.position;
        float dist = Vector3.Distance(playerPos, transform.parent.position);

        if (dist >= renderDist * Size + Size)
        {
            GetComponent<Renderer>().enabled = false;
        }
        else if (dist <= renderDist * Size + Size) 
        {
            GetComponent<Renderer>().enabled = true;
        }
    }

    void CreateShape()
    {
        vertices = new Vector3[(Size + 1) * (Size + 1)];

        for (int i = 0, z = 0; z <= Size; z++)
        {
            for (int x = 0; x <= Size; x++)
            {
                int X = Mathf.RoundToInt(transform.parent.position.x) + x;
                int Z = Mathf.RoundToInt(transform.parent.position.z) + z;

                float y = Mathf.PerlinNoise((X + xOffset) * 0.001f, (Z + yOffset) * 0.001f) * 100;
                y += Mathf.PerlinNoise((X + xOffset) * 0.01f, (Z + yOffset) * 0.01f) * 15;
                y += Mathf.PerlinNoise((X + xOffset) * 0.02f, (Z + yOffset) * 0.02f) * 5;


                vertices[i] = new Vector3(x, y, z);

                i++;
            }
        }


        triangles = new int[Size * Size * 6];

        int vertNum = 0;
        int triangleIndex = 0;

        for (int z = 0; z < Size; z++)
        {
            for (int x = 0; x < Size; x++)
            {
                triangles[triangleIndex + 0] = vertNum + 0;
                triangles[triangleIndex + 1] = vertNum + Size + 1;
                triangles[triangleIndex + 2] = vertNum + 1;
                triangles[triangleIndex + 3] = vertNum + 1;
                triangles[triangleIndex + 4] = vertNum + Size + 1;
                triangles[triangleIndex + 5] = vertNum + Size + 2;

                triangleIndex += 6;
                vertNum++;
            }
            vertNum++;
        }
    }

    void UpdateMesh()
    {
        mesh.Clear();

        mesh.vertices = vertices;
        mesh.triangles = triangles;

        mesh.RecalculateNormals();
    }
}
