using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class WaterManager : MonoBehaviour
{
    private MeshFilter meshFilter;

    void Start()
    {
        meshFilter = GetComponent<MeshFilter>();
    }

    void Update()
    {
        Vector3[] verticies = meshFilter.mesh.vertices;
        for (int i = 0; i < verticies.Length; i++)
        {
            verticies[i].y = WaveManager.instance.GetWaveHeight(transform.position.x + (verticies[i].x * transform.localScale.x));
        }

        meshFilter.mesh.vertices = verticies;
        meshFilter.mesh.RecalculateNormals();
    }
}
