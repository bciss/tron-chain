using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TrailRendererWith2DCollider : MonoBehaviour
{

    //************
    //
    // Fields
    //  
    //************
    // public Tailer snake;                           //the snake this script is attached on to get the speed and more if affinity
    public Material trailMaterial;                  //the material of the trail.  Changing this during runtime will have no effect.
    public float lifeTime = 1.0f;                   //the amount of time in seconds that the trail lasts
    public float changeTime = 0.5f;                 //time point when the trail begins changing its width (if widthStart != widthEnd)
    public float widthStart = 1.0f;                 //the starting width of the trail
    public float widthEnd = 1.0f;                   //the ending width of the trail
    public float length = 200;
    public float vertexDistanceMin = 1f;         //the minimum distance between the center positions
    public Vector3 renderDirection = new Vector3(0, 0, -1); //the direction that the mesh of the trail will be rendered towards
    public bool colliderIsTrigger = true;           //determines if the collider is a trigger.  Changing this during runtime will have no effect.
    public bool colliderEnabled = true;             //determines if the collider is enabled.  Changing this during runtime will have no effect.
    public bool pausing = false;                     //determines if the trail is pausing, i.e. neither creating nor destroying vertices

    private Transform trans;                        //transform of the object this script is attached to                    
    private Mesh mesh;
    private new PolygonCollider2D collider;
    private Collider2D trailCollider;
    private Material colliderRendererMaterial;
    private Renderer trailRenderer;

    private LinkedList<Vector3> centerPositions;    //the previous positions of the object this script is attached to
    private LinkedList<Vertex> leftVertices;        //the left vertices derived from the center positions
    private LinkedList<Vertex> rightVertices;       //the right vertices derived from the center positions

    public void ChangeTrailMaterial(Material material)
    {
        trailMaterial = material;
        colliderRendererMaterial = material;
    }

    public void ChangeColliderTrigger(bool isTrigger)
    {
        colliderIsTrigger = isTrigger;
        collider.isTrigger = isTrigger;
    }

    public void ChangeColliderEnabled(bool enabled)
    {
        colliderEnabled = enabled;
        collider.enabled = enabled;
    }

    private void Awake()
    {
        GameObject trail = new GameObject("Trail", new[] { typeof(MeshRenderer), typeof(MeshFilter), typeof(PolygonCollider2D) });
        mesh = trail.GetComponent<MeshFilter>().mesh = new Mesh();
        trail.GetComponent<Renderer>().material = trailMaterial;

        collider = trail.GetComponent<PolygonCollider2D>();
        colliderRendererMaterial = collider.GetComponent<Renderer>().material;
        collider.isTrigger = colliderIsTrigger;
        collider.SetPath(0, null);

        trans = base.transform;

        centerPositions = new LinkedList<Vector3>();
        centerPositions.AddFirst(trans.position);

        leftVertices = new LinkedList<Vertex>();
        rightVertices = new LinkedList<Vertex>();
    }

    private void Update()
    {
        if (!pausing)
        {
            TryAddVertices();
                SetMesh();
        }
    }

    private void TryAddVertices()
    {

        if ((centerPositions.First.Value - trans.position).sqrMagnitude > vertexDistanceMin)
        {
                float x = transform.position.x;
                float y = transform.position.y;

                Vector3 nextVertex;
                    nextVertex = new Vector3(x, y, 0);
                    //points.append((x, y));

                    ////// end freestylt 4h du mat

                    //calculate the normalized direction from the 1) most recent position of vertex creation to the 2) current position
                    Vector3 dirToCurrentPos = (nextVertex - centerPositions.First.Value).normalized;

                    //calculate the positions of the left and right vertices --> they are perpendicular to 'dirToCurrentPos' and 'renderDirection'
                    Vector3 cross = Vector3.Cross(renderDirection, dirToCurrentPos);
                    Vector3 leftPos = nextVertex + (cross * -widthStart * 0.5f);
                    Vector3 rightPos = nextVertex + (cross * widthStart * 0.5f);

                    //create two new vertices at the calculated positions
                    leftVertices.AddFirst(new Vertex(leftPos, nextVertex, (leftPos - nextVertex).normalized, cross, -widthStart));
                    rightVertices.AddFirst(new Vertex(rightPos, nextVertex, (rightPos - nextVertex).normalized, cross, widthStart));

                    //add the current position as the most recent center position
                    centerPositions.AddFirst(nextVertex);
        }
    }

    private void SetMesh()
    {
        if (centerPositions.Count < 2)
        {
            return;
        }

        Vector3[] vertices = new Vector3[centerPositions.Count * 2];
        Vector2[] uvs = new Vector2[centerPositions.Count * 2];
        int[] triangles = new int[(centerPositions.Count - 1) * 6];
        Vector2[] colliderPath = new Vector2[(centerPositions.Count - 1) * 2];

        LinkedListNode<Vertex> leftVertNode = leftVertices.First;
        LinkedListNode<Vertex> rightVertNode = rightVertices.First;

        float timeDelta = leftVertices.Last.Value.TimeAlive - leftVertices.First.Value.TimeAlive;

        for (int i = 0; i < leftVertices.Count; ++i)
        {
            Vertex leftVert = leftVertNode.Value;
            Vertex rightVert = rightVertNode.Value;

            int vertIndex = i * 2;
            vertices[vertIndex] = leftVert.Position;
            vertices[vertIndex + 1] = rightVert.Position;

            colliderPath[i] = leftVert.Position;
            colliderPath[colliderPath.Length - (i + 1)] = rightVert.Position;

            float uvValue = leftVert.TimeAlive / timeDelta;
            uvs[vertIndex] = new Vector2(uvValue, 0);
            uvs[vertIndex + 1] = new Vector2(uvValue, 1);

            if (i > 0)
            {
                int triIndex = (i - 1) * 6;
                triangles[triIndex] = vertIndex - 2;
                triangles[triIndex + 1] = vertIndex - 1;
                triangles[triIndex + 2] = vertIndex + 1;
                triangles[triIndex + 3] = vertIndex - 2;
                triangles[triIndex + 4] = vertIndex + 1;
                triangles[triIndex + 5] = vertIndex;
            }

            //increment the left and right vertex nodes
            leftVertNode = leftVertNode.Next;
            rightVertNode = rightVertNode.Next;
        }

        mesh.Clear();
        mesh.vertices = vertices;
        mesh.uv = uvs;
        mesh.triangles = triangles;

        if (colliderEnabled)
        {
            collider.SetPath(0, colliderPath);
        }
    }

    private class Vertex
    {
        private Vector3 centerPosition; //the center position in the trail that this vertex was derived from
        private Vector3 derivedDirection; //the direction from the 1) center position to the 2) position of this vertex
        private float creationTime;
        private Vector3 cross;
        private float widthStart;

        public Vector3 Position { get; set; }
        public float TimeAlive { get { return Time.time - creationTime; } }

        public void AdjustWidth(float width)
        {
            Position = centerPosition + (derivedDirection * width);
        }

        public void UpdateVertexPostion(Vector3 position, Vector3 centerPosition, Vector3 derivedDirection, Vector3 cross, float widthStart)
        {
            this.Position = position;
            this.centerPosition = centerPosition;
            this.derivedDirection = derivedDirection;
            this.cross = cross;
            this.widthStart = widthStart;
            creationTime = Time.time;
        }

        public Vertex(Vector3 position, Vector3 centerPosition, Vector3 derivedDirection, Vector3 cross, float widthStart)
        {
            this.Position = position;
            this.centerPosition = centerPosition;
            this.derivedDirection = derivedDirection;
            this.cross = cross;
            this.widthStart = widthStart;
            creationTime = Time.time;
        }

    }
}