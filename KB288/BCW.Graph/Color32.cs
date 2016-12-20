using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;

namespace BCW.Graph
{
    #region �ṹColor32
    /// <summary>
    /// ��װ����ɫ�ṹ
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    internal struct Color32
    {
        /// <summary>
        /// ��ɫ�е�B,λ�����λ
        /// </summary>
        [FieldOffset(0)]
        internal byte Blue;

        /// <summary>
        /// ��ɫ�е�G,λ�ڵڶ�λ
        /// </summary>
        [FieldOffset(1)]
        internal byte Green;

        /// <summary>
        /// ��ɫ�е�R,λ�ڵ���λ
        /// </summary>
        [FieldOffset(2)]
        internal byte Red;

        /// <summary>
        /// ��ɫ�е�A,λ�ڵ���λ
        /// </summary>
        [FieldOffset(3)]
        internal byte Alpha;

        /// <summary>
        /// ��ɫ������ֵ
        /// </summary>
        [FieldOffset(0)]
        internal int ARGB;

        /// <summary>
        /// ��ɫ
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
