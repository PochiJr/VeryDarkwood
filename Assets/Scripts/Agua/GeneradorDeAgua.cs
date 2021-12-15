using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneradorDeAgua : MonoBehaviour
{
    public float size = 1;
    public int gridSize = 16;

    private MeshFilter filter;


    // Start is called before the first frame update
    void Start()
    {
        filter = GetComponent<MeshFilter>();
        filter.mesh = GenerateMesh();
    }

    private Mesh GenerateMesh()
    {
        Mesh m = new Mesh();

        var vertices = new List<Vector3>();
        var normals = new List<Vector3>();
        var uvs = new List<Vector2>();

        for (int x = 0; x <= gridSize; x++)
        {
            for (int y = 0; y <= gridSize; y++)
            {
                vertices.Add(new Vector3(-size * 0.5f + size * (x / (float)gridSize), 0, -size * 0.5f + size * (y / (float)gridSize)));
                normals.Add(Vector3.up);
                uvs.Add(new Vector2(x/(float)gridSize, y/(float)gridSize));
            }
        }

        var triangles = new List<int>();
        var verCount = gridSize + 1;

        for(int i = 0; i < verCount * verCount - verCount; i++)
        {
            if ((i + 1) % verCount == 0)
                continue;
            triangles.AddRange(new List<int>()
            {
                i+1+verCount, i+verCount, i, // Primer triangulo
                i, i+1, i+verCount+1         // Segundo triangulo
            });
        }

        m.SetVertices(vertices);
        m.SetNormals(normals);
        m.SetUVs(0, uvs);
        m.SetTriangles(triangles, 0);

        return m;
    }
}