using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;

namespace BCW.Graph
{
    #region 结构Color32
    /// <summary>
    /// 封装的颜色结构
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    internal struct Color32
    {
        /// <summary>
        /// 颜色中的B,位于最低位
        /// </summary>
        [FieldOffset(0)]
        internal byte Blue;

        /// <summary>
        /// 颜色中的G,位于第二位
        /// </summary>
        [FieldOffset(1)]
        internal byte Green;

        /// <summary>
        /// 颜色中的R,位于第三位
        /// </summary>
        [FieldOffset(2)]
        internal byte Red;

        /// <summary>
        /// 颜色中的A,位于第四位
        /// </summary>
        [FieldOffset(3)]
        internal byte Alpha;

        /// <summary>
        /// 颜色的整形值
        /// </summary>
        [FieldOffset(0)]
        internal int ARGB;

        /// <summary>
        /// 颜色
        /// </summary>
        internal Color Color
        {
            get
            {
                return Color.FromArgb(ARGB);
            }
        }

        internal Color32(int c)
        {
            Alpha =0 ;
            Red = 0;
            Green = 0;
            Blue = 0;
            ARGB =c;           
        }
        internal Color32(byte a, byte r, byte g, byte b)
        {
            ARGB = 0;
            Alpha = a;
            Red = r;
            Green = g;
            Blue = b;
        }
    }
    #endregion
}
