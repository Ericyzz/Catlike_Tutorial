using UnityEngine;
namespace yzz
{
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
        /// 格子的纯色区域的半径占比
        /// </summary>
        public const float solidFactor = 0.75f;
        /// <summary>
        /// 格子的混合边界颜色的半径占比
        /// </summary>
        public const float blendFactor = 1f - solidFactor;

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
        /// 绘制三角面的首个顶点向量
        /// </summary>
        /// <param name="direction"></param>
        /// <returns></returns>
        public static Vector3 GetFirstCorner(HexDirection direction)
        {
            return corners[(int)direction];
        }

        /// <summary>
        /// 绘制三角面的第二个顶点向量
        /// </summary>
        /// <param name="direction"></param>
        /// <returns></returns>
        public static Vector3 GetSecondCorner(HexDirection direction)
        {
            return corners[(int)direction + 1];
        }

        /// <summary>
        /// 获取三角面的第一个顶点向量(纯色区域)
        /// </summary>
        /// <param name="direction"></param>
        /// <returns></returns>
        public static Vector3 GetFirstSolidCorner(HexDirection direction)
        {
            return corners[(int)direction] * solidFactor;
        }

        /// <summary>
        /// 获取三角面的第二个顶点向量(纯色区域)
        /// </summary>
        /// <param name="direction"></param>
        /// <returns></returns>
        public static Vector3 GetSecondSolidCorner(HexDirection direction)
        {
            return corners[(int)direction + 1] * solidFactor;
        }

        public static Vector3 GetBridge(HexDirection direction)
        {
            return (corners[(int)direction] + corners[(int)direction + 1]) * blendFactor;
        }
    }
}

