using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace BCW.Graph
{
    #region 类ImageDescriptor
    /// <summary>
    /// 图象标识符(Image Descriptor)一个GIF文件内可以包含多幅图象，
    /// 一幅图象结束之后紧接着下是一幅图象的标识符，图象标识符以0x2C(',')
    /// 字符开始，定义紧接着它的图象的性质，包括图象相对于逻辑屏幕边界的偏移量、
    /// 图象大小以及有无局部颜色列表和颜色列表大小，由10个字节组成
    /// </summary>
    internal class ImageDescriptor
    {
        #region 结构字段      

        /// <summary>
        /// X方向偏移量
        /// </summary>
        internal short XOffSet;

        /// <summary>
        /// X方向偏移量
        /// </summary>
        internal short YOffSet;

        /// <summary>
        /// 图象宽度
        /// </summary>
        internal short Width;

        /// <summary>
        /// 图象高度
        /// </summary>
        internal short Height;     

        /// <summary>
        /// packed
        /// </summary>
        internal byte Packed;

        /// <summary>
        /// 局部颜色列表标志(Local Color Table Flag)
        /// 置位时标识紧接在图象标识符之后有一个局部颜色列表，供紧跟在它之后的一幅图象使用；
        /// 值否时使用全局颜色列表，忽略pixel值。
        /// </summary>
        internal bool LctFlag;    

        /// <summary>
        /// 交织标志(Interlace Flag)，置位时图象数据使用连续方式排列，否则使用顺序排列。
        /// </summary>
        internal bool InterlaceFlag;

        /// <summary>
        ///  分类标志(Sort Flag)，如果置位表示紧跟着的局部颜色列表分类排列.
        /// </summary>
        internal bool SortFlag;

        /// <summary>
        ///  pixel - 局部颜色列表大小(Size of Local Color Table)，pixel+1就为颜色列表的位数
        /// </summary>
        internal int LctSize;
        #endregion     

        #region 方法函数
        internal byte[] GetBuffer()
        {
            List<byte> list = new List<byte>();
            list.Add(GifExtensions.ImageDescriptorLabel);
            list.AddRange(BitConverter.GetBytes(XOffSet));
            list.AddRange(BitConverter.GetBytes(YOffSet));
            list.AddRange(BitConverter.GetBytes(Width));
            list.AddRange(BitConverter.GetBytes(Height));
            byte packed = 0;
            int m = 0;
            if (LctFlag)
            {
                m = 1;
            }
            int i = 0;
            if (InterlaceFlag)
            {
                i = 1;
            }
            int s = 0;
            if (SortFlag)
            {
                s = 1;
            }
            byte pixel = (byte)(Math.Log(LctSize,2) - 1);
            packed = (byte)(pixel | (s << 5) | (i << 6) | (m << 7));
            list.Add(packed);          
            return list.ToArray();
        }
        #endregion
    }
    #endregion
}
