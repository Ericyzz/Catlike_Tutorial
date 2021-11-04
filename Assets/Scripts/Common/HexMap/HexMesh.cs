using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 六边形的网格信息
/// </summary>
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class HexMesh : MonoBehaviour
{
    /// <summary>
    /// 网格
    /// </summary>
    Mesh hexMesh;
    /// <summary>
    /// 网格物理属性
    /// </summary>
    MeshCollider meshCollider;
    /// <summary>
    /// 存放所有顶点
    /// </summary>
    List<Vector3> vertices;
    /// <summary>
    /// 存放所有三角面
    /// </summary>
    List<int> triangles;
    /// <summary>
    /// 存放所有三角面的颜色
    /// </summary>
    List<Color> colors;

    void Awake()
    {
        GetComponent<MeshFilter>().mesh = hexMesh = new Mesh();
        meshCollider = gameObject.AddComponent<MeshCollider>();
        hexMesh.name = "Hex Mesh";
        vertices = new List<Vector3>();
        triangles = new List<int>();
        colors = new List<Color>();
    }

    /// <summary>
    /// 绘制地块
    /// </summary>
    /// <param name="cells"></param>
    public void Triangulate(HexCell[] cells)
    {
        hexMesh.Clear();
        vertices.Clear();
        triangles.Clear();
        colors.Clear();
        for (int i = 0; i < cells.Length; i++)
        {
            Triangulate(cells[i]);
        }
        hexMesh.vertices = vertices.ToArray();
        hexMesh.triangles = triangles.ToArray();
        hexMesh.colors = colors.ToArray();
        hexMesh.RecalculateNormals();
        meshCollider.sharedMesh = hexMesh;
    }

    /// <summary>
    /// 绘制六边形(六个三角面)
    /// </summary>
    /// <param name="cell"></param>
    void Triangulate(HexCell cell)
    {
        for (HexDirection d = HexDirection.NE;d <= HexDirection.NW;d++)
        {
            Triangulate(d, cell);
        }
    }

    void Triangulate(HexDirection direction, HexCell cell)
    {
        Vector3 center = cell.transform.localPosition;
        AddTriangle(
            center,
            center + HexMetrics.GetFirstCorner(direction),
            center + HexMetrics.GetSecondCorner(direction)
        );
        //一个顶点由(自身格+邻居+上或下个邻居)混合而成
        HexCell preNeighbor = cell.GetNeighbor(direction.Previous()) ?? cell;
        HexCell neighbor = cell.GetNeighbor(direction) ?? cell; //防止边界没有邻居
        HexCell nextNeighbor = cell.GetNeighbor(direction.Next()) ?? cell;

        AddTriangleColor(
            cell.color,
            (cell.color + neighbor.color + preNeighbor.color) / 3,
            (cell.color + neighbor.color + nextNeighbor.color) / 3);
    }

    /// <summary>
    /// 添加一个三角面(索引为添加前列表长度)
    /// </summary>
    /// <param name="v1"></param>
    /// <param name="v2"></param>
    /// <param name="v3"></param>
    void AddTriangle(Vector3 v1, Vector3 v2, Vector3 v3)
    {
        int vertexIndex = vertices.Count;
        vertices.Add(v1);
        vertices.Add(v2);
        vertices.Add(v3);
        triangles.Add(vertexIndex);
        triangles.Add(vertexIndex + 1);
        triangles.Add(vertexIndex + 2);
    }

    /// <summary>
    /// 添加混合颜色
    /// </summary>
    /// <param name="color"></param>
    void AddTriangleColor(Color c1,Color c2,Color c3)
    {
        colors.Add(c1);
        colors.Add(c2);
        colors.Add(c3);
    }
}