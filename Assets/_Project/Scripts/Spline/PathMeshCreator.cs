using PathCreation;
using PathCreation.Examples;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class PathMeshCreator : PathSceneTool
{
    //Components
    [Header("Components")]
    [SerializeField] private GameObject meshHolder;

    //Variables
    [Header("Path Settings")]
    [SerializeField] private float pathWidth = .4f;
    [SerializeField][Range(0, .5f)] private float thickness = .15f;
    [SerializeField] private bool flattenSurface;

    [Header("Material Settings")]
    [SerializeField] private Material pathMaterial;
    [SerializeField] private Material undersideMaterial;

    [Space(10)]

    [SerializeField] private float textureTiling = 1;
    [SerializeField] private float textureTilingMultiplier = 1;

    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;
    private Mesh mesh;

    //Getters
    public Material PathMaterial => pathMaterial;

    //Setters
    public void SetMaterial(Material newMaterial, float newTextureTilingMultiplier)
    {
#if UNITY_EDITOR
        UnityEditor.Undo.RecordObject(this, "Path Texture Update");
#endif

        pathMaterial = newMaterial;
        textureTilingMultiplier = newTextureTilingMultiplier;
    }

    protected override void PathUpdated()
    {
        if (pathCreator != null)
        {
            UpdateTextureTiling();
            AssignMeshComponents();
            AssignMaterials(pathMaterial);
            CreatePathMesh();
        }
    }

    private void CreatePathMesh()
    {
        Vector3[] verts = new Vector3[path.NumPoints * 8];
        Vector2[] uvs = new Vector2[verts.Length];
        Vector3[] normals = new Vector3[verts.Length];

        int numTris = 2 * (path.NumPoints - 1) + ((path.isClosedLoop) ? 2 : 0);
        int[] pathTriangles = new int[numTris * 3];
        int[] underPathTriangles = new int[numTris * 3];
        int[] sideOfPathTriangles = new int[numTris * 2 * 3];

        int vertIndex = 0;
        int triIndex = 0;

        // Vertices for the top of the path are layed out:
        // 0  1
        // 8  9
        // and so on... So the triangle map 0,8,1 for example, defines a triangle from top left to bottom left to bottom right.
        int[] triangleMap = { 0, 8, 1, 1, 8, 9 };
        int[] sidesTriangleMap = { 4, 6, 14, 12, 4, 14, 5, 15, 7, 13, 15, 5 };

        bool usePathNormals = !(path.space == PathSpace.xyz && flattenSurface);

        for (int i = 0; i < path.NumPoints; i++)
        {
            Vector3 localUp = (usePathNormals) ? Vector3.Cross(path.GetTangent(i), path.GetNormal(i)) : path.up;
            Vector3 localRight = (usePathNormals) ? path.GetNormal(i) : Vector3.Cross(localUp, path.GetTangent(i));

            // Find position to left and right of current path vertex
            Vector3 vertSideA = path.GetPoint(i) - localRight * Mathf.Abs(pathWidth);
            Vector3 vertSideB = path.GetPoint(i) + localRight * Mathf.Abs(pathWidth);

            // Add top of path vertices
            verts[vertIndex + 0] = vertSideA;
            verts[vertIndex + 1] = vertSideB;
            // Add bottom of path vertices
            verts[vertIndex + 2] = vertSideA - localUp * thickness;
            verts[vertIndex + 3] = vertSideB - localUp * thickness;

            // Duplicate vertices to get flat shading for sides of path
            verts[vertIndex + 4] = verts[vertIndex + 0];
            verts[vertIndex + 5] = verts[vertIndex + 1];
            verts[vertIndex + 6] = verts[vertIndex + 2];
            verts[vertIndex + 7] = verts[vertIndex + 3];

            // Set uv on y axis to path time (0 at start of path, up to 1 at end of path)
            uvs[vertIndex + 0] = new Vector2(0, path.times[i]);
            uvs[vertIndex + 1] = new Vector2(1, path.times[i]);

            // Top of path normals
            normals[vertIndex + 0] = localUp;
            normals[vertIndex + 1] = localUp;
            // Bottom of path normals
            normals[vertIndex + 2] = -localUp;
            normals[vertIndex + 3] = -localUp;
            // Sides of path normals
            normals[vertIndex + 4] = -localRight;
            normals[vertIndex + 5] = localRight;
            normals[vertIndex + 6] = -localRight;
            normals[vertIndex + 7] = localRight;

            // Set triangle indices
            if (i < path.NumPoints - 1 || path.isClosedLoop)
            {
                for (int j = 0; j < triangleMap.Length; j++)
                {
                    pathTriangles[triIndex + j] = (vertIndex + triangleMap[j]) % verts.Length;
                    // reverse triangle map for under path so that triangles wind the other way and are visible from underneath
                    underPathTriangles[triIndex + j] = (vertIndex + triangleMap[triangleMap.Length - 1 - j] + 2) % verts.Length;
                }
                for (int j = 0; j < sidesTriangleMap.Length; j++)
                {
                    sideOfPathTriangles[triIndex * 2 + j] = (vertIndex + sidesTriangleMap[j]) % verts.Length;
                }

            }

            vertIndex += 8;
            triIndex += 6;
        }

        mesh.Clear();
        mesh.vertices = verts;
        mesh.uv = uvs;
        mesh.normals = normals;
        mesh.subMeshCount = 3;
        mesh.SetTriangles(pathTriangles, 0);
        mesh.SetTriangles(underPathTriangles, 1);
        mesh.SetTriangles(sideOfPathTriangles, 2);
        mesh.RecalculateBounds();
    }

    // Add MeshRenderer and MeshFilter components to this gameobject if not already attached
    private void AssignMeshComponents()
    {

        if (meshHolder == null)
        {
            MeshRenderer meshRenderer = GetComponentInChildren<MeshRenderer>();

            if (meshRenderer != null)
            {
                meshHolder = meshRenderer.gameObject;
            }
            else
            {
                meshHolder = new GameObject("PathMeshHolder");
                meshHolder.transform.SetParent(transform);
            }
        }

        meshHolder.transform.rotation = Quaternion.identity;
        meshHolder.transform.position = Vector3.zero;
        meshHolder.transform.localScale = Vector3.one;

        // Ensure mesh renderer and filter components are assigned
        if (!meshHolder.gameObject.GetComponent<MeshFilter>())
        {
            meshHolder.gameObject.AddComponent<MeshFilter>();
        }

        if (!meshHolder.GetComponent<MeshRenderer>())
        {
            meshHolder.gameObject.AddComponent<MeshRenderer>();
        }

        meshRenderer = meshHolder.GetComponent<MeshRenderer>();
        meshFilter = meshHolder.GetComponent<MeshFilter>();

        if (mesh == null)
        {
            mesh = new Mesh();
        }

        meshFilter.sharedMesh = mesh;
    }

    private void AssignMaterials(Material _pathMaterial)
    {
        if (_pathMaterial != null && undersideMaterial != null)
        {
            Material[] _material = new Material[] { _pathMaterial, undersideMaterial, undersideMaterial };
            Vector3 _vector = new Vector3(1, textureTiling);

            if(Application.isPlaying == false)
            {
                meshRenderer.sharedMaterials = _material;
                meshRenderer.sharedMaterials[0].mainTextureScale = _vector;
            }
            else
            {
                meshRenderer.materials = _material;
                meshRenderer.materials[0].mainTextureScale = _vector;
            }
        }
    }

    /// <summary>
    /// Assign materials at runtime so that each path can have a different texture size.
    /// </summary>
    public void AssignMaterialsRuntime()
    {
        if(pathMaterial != null)
        {
            Material pathMaterialRuntime = Instantiate(pathMaterial);

            AssignMaterials(pathMaterialRuntime);
        }
    }

    private void UpdateTextureTiling()
    {
        textureTiling = path.length * textureTilingMultiplier;
    }
}
