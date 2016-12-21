using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace BCW.Graph
{
    #region 类LogicalScreenDescriptor
    /// <summary>
    /// 逻辑屏幕标识符(Logical Screen Descriptor)
    /// </summary>
    internal class LogicalScreenDescriptor
    {
        private short _width;
        /// <summary>
        /// 逻辑屏幕宽度 像素数，定义GIF图象的宽度
        /// </summary>
        internal short Width
        {
            get { return _width; }
            set { _width = value; }
        }

        private short _height;

        /// <summary>
        /// 逻辑屏幕高度 像素数，定义GIF图象的高度
        /// </summary>
        internal short Height
        {
            get { return _height; }
            set { _height = value; }
        }


        private byte _packed;

        internal byte Packed
        {
            get { return _packed; }
            set { _packed = value; }
        }

        private byte _bgIndex;
        /// <summary>
        /// 背景色,背景颜色(在全局颜色列表中的索引，如果没有全局颜色列表，该值没有意义)
        /// </summary>
        internal byte BgColorIndex
        {
            get { return _bgIndex; }
            set { _bgIndex = value; }
        }


        private byte _pixelAspect;
        /// <summary>
        /// 像素宽高比,像素宽高比(Pixel Aspect Radio)
        /// </summary>
        internal byte PixcelAspect
        {
            get { return _pixelAspect; }
            set { _pixelAspect = value; }
        }
        private bool _globalColorTableFlag;
        /// <summary>
        /// m - 全局颜色列表标志(Global Color Table Flag)，当置位时表示有全局颜色列表，pixel值有意义.
        /// </summary>
        internal bool GlobalColorTableFlag
        {
            get { return _globalColorTableFlag; }
            set { _globalColorTableFlag = value; }
        }

        private byte _colorResoluTion;
        /// <summary>
        /// cr - 颜色深度(Color ResoluTion)，cr+1确定图象的颜色深度.
        /// </summary>
        internal byte ColorResoluTion
        {
            get { return _colorResoluTion; }
            set { _colorResoluTion = value; }
        }

        private int _sortFlag;

        /// <summary>
        /// s - 分类标志(Sort Flag)，如果置位表示全局颜色列表分类排列.
        /// </summary>
        internal int SortFlag
        {
            get { return _sortFlag; }
            set { _sortFlag = value; }
        }

        private int _globalColorTableSize;
        /// <summary>
        /// 全局颜色列表大小，pixel+1确定颜色列表的索引数（2的pixel+1次方）.
        /// </summary>
        internal int GlobalColorTableSize
        {
            get { return _globalColorTableSize; }
            set { _globalColorTableSize = value; }
        }
           

        internal byte[] GetBuffer()
        {
            byte[] buffer = new byte[7];
            Array.Copy(BitConverter.GetBytes(_width), 0, buffer, 0, 2);
            Array.Copy(BitConverter.GetBytes(_height), 0, buffer, 2, 2);
            int m = 0;
            if (_globalColorTableFlag)
            {
                m = 1;
            }
            byte pixel = (byte)(Math.Log(_globalColorTableSize,2) - 1);
            _packed = (byte)(pixel | (_sortFlag << 4)|(_colorResoluTion<<5)|(m<<7));
            buffer[4] = _packed;
            buffer[5] = _bgIndex;
            buffer[6] = _pixelAspect;
            return buffer;
        }
    }
    #endregion
}
