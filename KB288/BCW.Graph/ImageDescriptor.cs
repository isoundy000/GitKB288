using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace BCW.Graph
{
    #region ��ImageDescriptor
    /// <summary>
    /// ͼ���ʶ��(Image Descriptor)һ��GIF�ļ��ڿ��԰������ͼ��
    /// һ��ͼ�����֮�����������һ��ͼ��ı�ʶ����ͼ���ʶ����0x2C(',')
    /// �ַ���ʼ���������������ͼ������ʣ�����ͼ��������߼���Ļ�߽��ƫ������
    /// ͼ���С�Լ����޾ֲ���ɫ�б����ɫ�б��С����10���ֽ����
    /// </summary>
    internal class ImageDescriptor
    {
        #region �ṹ�ֶ�      

        /// <summary>
        /// X����ƫ����
        /// </summary>
        internal short XOffSet;

        /// <summary>
        /// X����ƫ����
        /// </summary>
        internal short YOffSet;

        /// <summary>
        /// ͼ����
        /// </summary>
        internal short Width;

        /// <summary>
        /// ͼ��߶�
        /// </summary>
        internal short Height;     

        /// <summary>
        /// packed
        /// </summary>
        internal byte Packed;

        /// <summary>
        /// �ֲ���ɫ�б��־(Local Color Table Flag)
        /// ��λʱ��ʶ������ͼ���ʶ��֮����һ���ֲ���ɫ�б�����������֮���һ��ͼ��ʹ�ã�
        /// ֵ��ʱʹ��ȫ����ɫ�б�����pixelֵ��
        /// </summary>
        internal bool LctFlag;    

        /// <summary>
        /// ��֯��־(Interlace Flag)����λʱͼ������ʹ��������ʽ���У�����ʹ��˳�����С�
        /// </summary>
        internal bool InterlaceFlag;

        /// <summary>
        ///  �����־(Sort Flag)�������λ��ʾ�����ŵľֲ���ɫ�б��������.
        /// </summary>
        internal bool SortFlag;

        /// <summary>
        ///  pixel - �ֲ���ɫ�б��С(Size of Local Color Table)��pixel+1��Ϊ��ɫ�б��λ��
        /// </summary>
        internal int LctSize;
        #endregion     

        #region ��������
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
