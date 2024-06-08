using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(ParticleSystemUser))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
public class ClothMeshGenerator : MonoBehaviour {

    private Mesh mesh;

    public void GenerateMesh(List<Vector3> positions, int n) {
        GetComponent<MeshFilter>().mesh = mesh = new Mesh();
        mesh.name = "Procedural Cloth Mesh";

        mesh.vertices = positions.ToArray();
        CreateTriangles(n);
        mesh.RecalculateNormals();
    }

    private void CreateTriangles(int n) {
        int[] triangles = new int[(n - 1) * (n - 1) * 2 * 3];

        int trisInd = 0;
        for (int i = 0; i < n - 1; i++) {
            for (int j = 0; j < n - 1; j++) {
                triangles[trisInd] = (i * n + j);
                triangles[trisInd + 1] = (i * n + j + 1);
                triangles[trisInd + 2] = ((i + 1) * n + j);

                triangles[trisInd + 3] = (i * n + j + 1);
                triangles[trisInd + 4] = ((i + 1) * n + j + 1);
                triangles[trisInd + 5] = ((i + 1) * n + j);

                trisInd += 6;
            }
        }

        mesh.triangles = triangles;
    }
}
