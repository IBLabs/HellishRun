using System.Collections;
using System.Collections.Generic;
using PathCreation;
using UnityEngine;

public class TubeSegment
{
    public Vector3 Position { get; set; }
    public Vector3 Direction { get; set; }
    public float Radius { get; set; }

    public TubeSegment(Vector3 position, Vector3 direction, float radius)
    {
        Position = position;
        Direction = direction;
        Radius = radius;
    }
}

public class TubeMeshGenerator
{
    private List<TubeSegment> tubeSegments;
    private int resolution; // the number of vertices in a circle

    public TubeMeshGenerator(List<TubeSegment> tubeSegments, int resolution)
    {
        this.tubeSegments = tubeSegments;
        this.resolution = resolution;
    }
    
    public Mesh GenerateMesh()
    {
        Mesh mesh = new Mesh();

        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();
        List<Vector2> uvs = new List<Vector2>();

        for (int i = 0; i < tubeSegments.Count; i++)
        {
            TubeSegment segment = tubeSegments[i];

            // Generate the vertices for the circle at this segment
            for (int j = 0; j < resolution; j++)
            {
                float angle = j * 360f / resolution;
                
                Vector3 ortho;
                if (segment.Direction != Vector3.up && segment.Direction != Vector3.down)
                {
                    ortho = Vector3.Cross(segment.Direction, Vector3.up).normalized;
                }
                else
                {
                    ortho = Vector3.Cross(segment.Direction, Vector3.right).normalized;
                }
                
                Quaternion orthoRotate = Quaternion.AngleAxis(angle, segment.Direction);

                Vector3 something = orthoRotate * ortho;
                
                Vector3 vertex = segment.Position + something * segment.Radius;
                vertices.Add(vertex);

                float uvX = (float)j / resolution;
                float uvY = (float)i / tubeSegments.Count;
                Vector2 uvCoords = new Vector2(uvX, uvY);
                uvs.Add(uvCoords);
            }

            // Generate the triangles that connect this segment to the next one
            if (i < tubeSegments.Count - 1)
            {
                for (int j = 0; j < resolution; j++)
                {
                    int start = i * resolution;
                    triangles.Add(start + j);
                    triangles.Add(start + (j + 1) % resolution);
                    triangles.Add(start + j + resolution);

                    triangles.Add(start + (j + 1) % resolution);
                    triangles.Add(start + ((j + 1) % resolution) + resolution);
                    triangles.Add(start + j + resolution);
                }
            }
        }

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.uv = uvs.ToArray();
        mesh.RecalculateNormals();

        return mesh;
    }

}

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class TubeController : MonoBehaviour
{
    public float radius;

    // Start is called before the first frame update
    void Start()
    {
        PathCreator pathCreator = GetComponent<PathCreator>();
        VertexPath path = pathCreator.path;

        List<TubeSegment> segments = new List<TubeSegment>();

        for (int i = 0; i < path.NumPoints; i++)
        {
            Vector3 pointPos = path.GetPoint(i);
            Vector3 pointNormal = path.GetTangent(i);

            TubeSegment newSeg = new TubeSegment(pointPos, pointNormal, radius);
            
            segments.Add(newSeg);
        }

        TubeMeshGenerator generator = new TubeMeshGenerator(segments, 24);

        MeshFilter meshFilter = GetComponent<MeshFilter>();

        Mesh tubeMesh = generator.GenerateMesh();
        meshFilter.mesh = tubeMesh;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
