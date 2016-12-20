using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace BCW.Graph
{
    #region ��LogicalScreenDescriptor
    /// <summary>
    /// �߼���Ļ��ʶ��(Logical Screen Descriptor)
    /// </summary>
    internal class LogicalScreenDescriptor
    {
        private short _width;
        /// <summary>
        /// �߼���Ļ��� ������������GIFͼ��Ŀ��
        /// </summary>
        internal short Width
        {
            get { return _width; }
            set { _width = value; }
        }

        private short _height;

        /// <summary>
        /// �߼���Ļ�߶� ������������GIFͼ��ĸ߶�
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
        /// ����ɫ,������ɫ(��ȫ����ɫ�б��е����������û��ȫ����ɫ�б���ֵû������)
        /// </summary>
        internal byte BgColorIndex
        {
            get { return _bgIndex; }
            set { _bgIndex = value; }
        }


        private byte _pixelAspect;
        /// <summary>
        /// ���ؿ�߱�,���ؿ�߱�(Pixel Aspect Radio)
        /// </summary>
        internal byte PixcelAspect
        {
            get { return _pixelAspect; }
            set { _pixelAspect = value; }
        }
        private bool _globalColorTableFlag;
        /// <summary>
        /// m - ȫ����ɫ�б��־(Global Color Table Flag)������λʱ��ʾ��ȫ����ɫ�б�pixelֵ������.
        /// </summary>
        internal bool GlobalColorTableFlag
        {
            get { return _globalColorTableFlag; }
            set { _globalColorTableFlag = value; }
        }

        private byte _colorResoluTion;
        /// <summary>
        /// cr - ��ɫ���(Color ResoluTion)��cr+1ȷ��ͼ�����ɫ���.
        /// </summary>
        internal byte ColorResoluTion
        {
            get { return _colorResoluTion; }
            set { _colorResoluTion = value; }
        }

        private int _sortFlag;

        /// <summary>
        /// s - �����־(Sort Flag)�������λ��ʾȫ����ɫ�б��������.
        /// </summary>
        internal int SortFlag
        {
            get { return _sortFlag; }
            set { _sortFlag = value; }
        }

        private int _globalColorTableSize;
        /// <summary>
        /// ȫ����ɫ�б��С��pixel+1ȷ����ɫ�б����������2��pixel+1�η���.
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
