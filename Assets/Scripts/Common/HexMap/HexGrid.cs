using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 地图
/// </summary>
public class HexGrid : MonoBehaviour
{
    /// <summary>
    /// 地图宽度
    /// </summary>
    public int width = 6;
    /// <summary>
    /// 地图高度
    /// </summary>
    public int height = 6;
    /// <summary>
    /// 地块预制体
    /// </summary>
    public HexCell cellPrefab;
    /// <summary>
    /// 存储所有地块
    /// </summary>
    HexCell[] cells;
    /// <summary>
    /// 地块的网格
    /// </summary>
    HexMesh hexMesh;

    public Text cellLabelPrefab;
    Canvas gridCanvas;

    public Color defaultColor = Color.white;

    void Awake()
    {
        gridCanvas = GetComponentInChildren<Canvas>();
        hexMesh = GetComponentInChildren<HexMesh>();

        cells = new HexCell[height * width];

        for (int z = 0, i = 0; z < height; z++)
        {
            for (int x = 0; x < width; x++)
            {
                CreateCell(x, z, i++);
            }
        }
    }

    void Start()
    {
        hexMesh.Triangulate(cells);
    }

    /// <summary>
    /// 创建网格
    /// (六边形位置相邻,中心点之间的x轴距离为两倍内半径，y轴距离为1.5倍外半径，每行偏移1个内半径长度)
    /// </summary>
    /// <param name="x">列</param>
    /// <param name="z">行</param>
    /// <param name="i">索引</param>
    void CreateCell(int x, int z, int i)
    {
        Vector3 position;
        position.x = (x + z * 0.5f - z / 2) * (HexMetrics.innerRadius * 2f);
        position.y = 0f;
        position.z = z * (HexMetrics.outerRadius * 1.5f);

        HexCell cell = cells[i] = Instantiate<HexCell>(cellPrefab);
        cell.transform.SetParent(transform, false);
        cell.transform.localPosition = position;
        cell.coordinates = HexCoordinates.FromOffsetCoordinates(x, z);
        cell.color = defaultColor;

        #region 连接邻居
        //除了第一个都有左邻(把所有格子左右都连接了)
        if (x > 0)
        {
            cell.SetNeighbor(HexDirection.W, cells[i - 1]); //左边就是索引前一个
        }
        //除了第一行
        if (z > 0)
        {
            if ((z & 1) == 0) //偶数行
            {
                cell.SetNeighbor(HexDirection.SE, cells[i - width]);
                if (x > 0)
                {
                    cell.SetNeighbor(HexDirection.SW, cells[i - width - 1]);
                }
            }
            else //奇数行
            {
                cell.SetNeighbor(HexDirection.SW, cells[i - width]);
                if (x < width - 1)
                {
                    cell.SetNeighbor(HexDirection.SE, cells[i - width + 1]);
                }
            }
        }
        #endregion

        Text label = Instantiate<Text>(cellLabelPrefab);
        label.rectTransform.SetParent(gridCanvas.transform, false);
        label.rectTransform.anchoredPosition =
            new Vector2(position.x, position.z);
        label.text = cell.coordinates.ToStringOnSeparateLines();
    }

    /// <summary>
    /// 着色格子
    /// </summary>
    /// <param name="position"></param>
    public void ColorCell(Vector3 position, Color color)
    {
        position = transform.InverseTransformPoint(position);
        HexCoordinates coordinates = HexCoordinates.FromPosition(position);
        int index = coordinates.X + coordinates.Z * width + coordinates.Z / 2;
        HexCell cell = cells[index];
        cell.color = color;
        hexMesh.Triangulate(cells);
    }
}
