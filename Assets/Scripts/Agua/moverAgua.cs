using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moverAgua : MonoBehaviour
{
    public float fuerza = 3;
    public float escala = 1;
    public float tEscala = 1;

    private float offsetX;
    private float offsetY;
    private MeshFilter mf;

    // Start is called before the first frame update
    void Start()
    {
        mf = GetComponent<MeshFilter>();
        calcularRuido()
;    }

    // Update is called once per frame
    void Update()
    {
        calcularRuido();
        offsetX += Time.deltaTime * tEscala;
        if (offsetY <= 0.1) offsetY += Time.deltaTime * tEscala;
        if (offsetY >= fuerza) offsetY -= Time.deltaTime * tEscala;
    }

    void calcularRuido()
    {
        Vector3[] vertices = mf.mesh.vertices;

        for (int i = 0; i< vertices.Length; i++)
        {
            vertices[i].y = calcularAltura(vertices[i].x, vertices[i].z) * fuerza;
        }
        mf.mesh.vertices = vertices;
        mf.mesh.RecalculateNormals();
    }

    float calcularAltura(float x, float y)
    {
        float xCord = x * escala + offsetX;
        float yCord = y * escala + offsetY;

        return Mathf.PerlinNoise(xCord, yCord);
    }
}
