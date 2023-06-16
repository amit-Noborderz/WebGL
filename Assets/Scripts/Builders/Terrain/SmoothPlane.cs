using System.Threading;
using UnityEngine;

public class SmoothPlane : MonoBehaviour
{
   /*  Mesh mesh;
    // Start is called before the first frame update
    void Start()
    {
        SmoothUpMesh().Forget();
    }

    private async UniTask SmoothUpMesh()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        Vector3[] vertices = mesh.vertices;

        for (int i = 0; i < vertices.Length; i++)
        {
            int count = 0;
            float average = 0f;
            for (int j = 0; j < vertices.Length; j++)
            {
                if (Vector3.Distance(vertices[i], vertices[j]) < 1f)
                {
                    count++;
                    average += vertices[j].y;
                }
            }
            average /= count;
            vertices[i].y = average;
        }

        mesh.vertices = vertices;
        mesh.RecalculateNormals();
    } */


    // The mesh to be smoothed
    public Mesh mesh;

    // The number of smoothing iterations to perform
    public int iterations = 10;

    // The amount of smoothing to apply in each iteration
    public float smoothingAmount = 0.5f;

    // The number of threads to use for smoothing
    public int numThreads = 4;

    private Vector3[] vertices;
    private Vector3[] smoothedVertices;
    private Thread[] threads;

    public float meshVerticesLength;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Z))
        {
            SmoothTerrain();
        }
    }

    private void SmoothTerrain()
    {

        mesh = GetComponent<MeshFilter>().mesh;
        meshVerticesLength = mesh.vertices.Length;
        // Get the mesh vertices
        vertices = mesh.vertices;

        // Create an array to hold the smoothed vertices
        smoothedVertices = new Vector3[vertices.Length];

        // Create an array to hold the threads
        threads = new Thread[numThreads];

        // Start the threads
        for (int i = 0; i < numThreads; i++)
        {
            threads[i] = new Thread(new ParameterizedThreadStart(SmoothVertices));
            threads[i].Start(i);
        }

        // Wait for the threads to finish
        for (int i = 0; i < numThreads; i++)
        {
            threads[i].Join();
        }

        // Update the mesh with the smoothed vertices
        mesh.vertices = smoothedVertices;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
    }

    private void SmoothVertices(object data)
    {
        int threadIndex = (int)data;
        int startIndex = threadIndex * (vertices.Length / numThreads);
        int endIndex = startIndex + (vertices.Length / numThreads);

        for (int i = startIndex; i < endIndex; i++)
        {
            Vector3 smoothedVertex = vertices[i];

            for (int j = 0; j < meshVerticesLength; j++)
            {
                if (i != j && vertices[i] == vertices[j])
                {
                    smoothedVertex += (vertices[j] - smoothedVertex) * smoothingAmount;
                }
            }

            smoothedVertices[i] = smoothedVertex;
        }
    }
}
