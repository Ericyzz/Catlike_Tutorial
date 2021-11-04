using UnityEngine;

/// <summary>
/// 六边形的基础数据（顶点、内外半径）
/// </summary>
public static class HexMetrics
{
    /// <summary>
    /// 外切圆半径(中心到顶点)
    /// </summary>
    public const float outerRadius = 10f;
    /// <summary>
    /// 内切圆半径(中心到边)
    /// </summary>
    public const float innerRadius = outerRadius * 0.866025404f;

    /// <summary>
    /// 顶点坐标
    /// </summary>
    public static Vector3[] corners = {
        new Vector3(0f, 0f, outerRadius),
        new Vector3(innerRadius, 0f, 0.5f * outerRadius),
        new Vector3(innerRadius, 0f, -0.5f * outerRadius),
        new Vector3(0f, 0f, -outerRadius),
        new Vector3(-innerRadius, 0f, -0.5f * outerRadius),
        new Vector3(-innerRadius, 0f, 0.5f * outerRadius),
        new Vector3(0f, 0f, outerRadius)//最后绘制的三角面会回到第一个点
    };

    /// <summary>
    /// 绘制三角面的首个顶点
    /// </summary>
    /// <param name="direction"></param>
    /// <returns></returns>
    public static Vector3 GetFirstCorner(HexDirection direction)
    {
        return corners[(int)direction];
    }

    /// <summary>
    /// 绘制三角面的第二个顶点
    /// </summary>
    /// <param name="direction"></param>
    /// <returns></returns>
    public static Vector3 GetSecondCorner(HexDirection direction)
    {
        return corners[(int)direction + 1];
    }
}
