using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace BCW.Graph
{
    #region �ṹPlainTextEx
    /// <summary>
    /// ���ı���չ(Plain Text Extension)��һ�����ǿ�ѡ�ģ���Ҫ89a�汾����
    /// ��������һ���򵥵��ı�ͼ����һ�������������ƵĴ��ı����ݣ�7-bit ASCII�ַ���
    /// �Ϳ��ƻ��ƵĲ�������ɡ������ı�������һ���ı���Text Grid��������߽磬����
    /// �����л��ֶ����Ԫ��ÿ���ַ�ռ��һ����Ԫ������ʱ�������ҡ����ϵ��µ�˳��
    /// ���ν��У�ֱ�����һ���ַ�����ռ�������ı���֮����ַ��������ԣ���˶�����
    /// ����Ĵ�СʱӦ��ע�⵽�Ƿ�������������ı����������ı�����ɫʹ��ȫ����ɫ�б�
    /// û�������ʹ��һ���Ѿ������ǰһ����ɫ�б����⣬ͼ���ı���չ��Ҳ����ͼ�ο�
    /// (Graphic Rendering Block)����������ǰ�涨��ͼ�ο�����չ�����ı�����ʽ��һ���޸ġ�
    /// </summary>
    internal struct PlainTextEx
    {

        #region �ṹ�ֶ�       

        /// <summary>
        /// Block Size - ���С���̶�ֵ12
        /// </summary>
        internal static readonly byte BlockSize = 0X0C;

        /// <summary>
        /// Text Glid Left Posotion - ����ֵ���ı������߼���Ļ����߽��
        /// </summary>
        internal short XOffSet;

        /// <summary>
        /// Text Glid Top Posotion - ����ֵ���ı������߼���Ļ���ϱ߽����
        /// </summary>
        internal short YOffSet;

        /// <summary>
        /// �ı���߶� Text Glid Width -����ֵ
        /// </summary>
        internal short Width;

        /// <summary>
        /// �ı���߶� Text Glid Height - ����ֵ
        /// </summary>
        internal short Height;

        /// <summary>
        /// �ַ���Ԫ���� Character Cell Width - ����ֵ��������Ԫ����
        /// </summary>
        internal byte CharacterCellWidth;

        /// <summary>
        /// �ַ���Ԫ��߶� Character Cell Height- ����ֵ��������Ԫ��߶�
        /// </summary>
        internal byte CharacterCellHeight;

        /// <summary>
        /// �ı�ǰ��ɫ���� Text Foreground Color Index - ǰ��ɫ��ȫ����ɫ�б��е�����
        /// </summary>
        internal byte ForegroundColorIndex;

        /// <summary>
        /// �ı�����ɫ���� Text Blackground Color Index - ����ɫ��ȫ����ɫ�б��е�����
        /// </summary>
        internal byte BgColorIndex;

          /// <summary>
        /// �ı����ݿ鼯��Plain Text Data - һ���������ݿ�(Data Sub-Blocks)��ɣ�����Ҫ����ʾ���ַ�����
        /// </summary>
        internal List<string> TextDatas;
  
       #endregion    

        #region ��������
        internal byte[] GetBuffer()
        {
            List<byte> list = new List<byte>();
            list.Add(GifExtensions.ExtensionIntroducer);
            list.Add(GifExtensions.PlainTextLabel);
            list.Add(BlockSize);
            list.AddRange(BitConverter.GetBytes(XOffSet));
            list.AddRange(BitConverter.GetBytes(YOffSet));
            list.AddRange(BitConverter.GetBytes(Width));
            list.AddRange(BitConverter.GetBytes(Height));
            list.Add(CharacterCellWidth);
            list.Add(CharacterCellHeight);
            list.Add(ForegroundColorIndex);
            list.Add(BgColorIndex);
            if (TextDatas != null)
            {
                foreach (string text in TextDatas)
                {
                    list.Add((byte)text.Length);
                    foreach (char c in text)
                    {
                        list.Add((byte)c);
                    }
                }
            }
            list.Add(GifExtensions.Terminator);
            return list.ToArray();
        }
        #endregion
    }
    #endregion
}
