using UnityEngine;

/// <summary>
/// 地块
/// </summary>
public class HexCell : MonoBehaviour
{
    /// <summary>
    /// 地块位置信息
    /// </summary>
    public HexCoordinates coordinates;

    /// <summary>
    /// 颜色信息
    /// </summary>
    public Color color;

    /// <summary>
    /// 相邻地块们
    /// </summary>
    [SerializeField]
    public HexCell[] neighbors;

    /// <summary>
    /// 添加相邻地块(同时添加反方向邻居)
    /// </summary>
    public void SetNeighbor(HexDirection direction, HexCell cell)
    {
        neighbors[(int)direction] = cell;
        cell.neighbors[(int)direction.Opposite()] = this;
    }

    /// <summary>
    /// 获取相邻地块
    /// </summary>
    public HexCell GetNeighbor(HexDirection direction)
    {
        return neighbors[(int)direction];
    }
}

public enum HexDirection
{
    NE, E, SE, SW, W, NW
}

public static class HexDirectionExtensions
{
    /// <summary>
    /// 计算相反方向
    /// </summary>
    /// <param name="direction"></param>
    /// <returns></returns>
    public static HexDirection Opposite(this HexDirection direction)
    {
        return (int)direction < 3 ? (direction + 3) : (direction - 3);
    }

    /// <summary>
    /// 前一个方向
    /// </summary>
    /// <param name="direction"></param>
    /// <returns></returns>
    public static HexDirection Previous(this HexDirection direction)
    {
        return direction == HexDirection.NE ? HexDirection.NW : (direction - 1);
    }

    /// <summary>
    /// 后一个方向
    /// </summary>
    /// <param name="direction"></param>
    /// <returns></returns>
    public static HexDirection Next(this HexDirection direction)
    {
        return direction == HexDirection.NW ? HexDirection.NE : (direction + 1);
    }
}
