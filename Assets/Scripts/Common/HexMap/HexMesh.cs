using UnityEngine;
using System.Collections.Generic;

namespace yzz
{
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
            for (HexDirection d = HexDirection.NE; d <= HexDirection.NW; d++)
            {
                Triangulate(d, cell);
            }
        }

        void Triangulate(HexDirection direction, HexCell cell)
        {
            Vector3 center = cell.transform.localPosition;
            Vector3 v1 = center + HexMetrics.GetFirstSolidCorner(direction);
            Vector3 v2 = center + HexMetrics.GetSecondSolidCorner(direction);
            AddTriangle(center, v1, v2);
            AddTriangleColor(cell.color);
            if (direction <= HexDirection.SE)
            {
                TriangulateConnection(direction, cell, v1, v2);
            }
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
        void AddTriangleColor(Color c1)
        {
            colors.Add(c1);
            colors.Add(c1);
            colors.Add(c1);
        }

        /// <summary>
        /// 添加混合颜色
        /// </summary>
        /// <param name="color"></param>
        void AddTriangleColor(Color c1,Color c2, Color c3)
        {
            colors.Add(c1);
            colors.Add(c2);
            colors.Add(c3);
        }

        /// <summary>
        /// 混合颜色的梯形顶点
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <param name="v3"></param>
        /// <param name="v4"></param>
        void AddQuad(Vector3 v1, Vector3 v2, Vector3 v3, Vector3 v4)
        {
            int vertexIndex = vertices.Count;
            vertices.Add(v1);
            vertices.Add(v2);
            vertices.Add(v3);
            vertices.Add(v4);
            triangles.Add(vertexIndex);
            triangles.Add(vertexIndex + 2);
            triangles.Add(vertexIndex + 1);
            triangles.Add(vertexIndex + 1);
            triangles.Add(vertexIndex + 2);
            triangles.Add(vertexIndex + 3);
        }

        /// <summary>
        /// 梯形颜色
        /// </summary>
        /// <param name="c1"></param>
        /// <param name="c2"></param>
        /// <param name="c3"></param>
        /// <param name="c4"></param>
        void AddQuadColor(Color c1, Color c2)
        {
            colors.Add(c1);
            colors.Add(c1);
            colors.Add(c2);
            colors.Add(c2);
        }

        void TriangulateConnection(HexDirection direction, HexCell cell, Vector3 v1, Vector3 v2)
        {
            HexCell neighbor = cell.GetNeighbor(direction);
            if (neighbor == null)
            {
                return;
            }

            Vector3 bridge = HexMetrics.GetBridge(direction);
            Vector3 v3 = v1 + bridge;
            Vector3 v4 = v2 + bridge;

            AddQuad(v1, v2, v3, v4);
            AddQuadColor(cell.color, neighbor.color);

            HexCell nextNeighbor = cell.GetNeighbor(direction.Next());
            if (direction <= HexDirection.E && nextNeighbor != null)
            {
                AddTriangle(v2, v4, v2 + HexMetrics.GetBridge(direction.Next()));
                AddTriangleColor(cell.color, neighbor.color, nextNeighbor.color);
            }
        }
    }
}
