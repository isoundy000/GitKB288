using System;
using System.Collections.Generic;
using System.Text;

namespace BCW.Graph
{
    #region ��GifFrame
    /// <summary>
    /// Gif�ļ��п��԰������ͼ��ÿ��ͼ�����ͼ���һЩ�������������֡:GifFrame
    /// </summary>
    internal class GifFrame
    {
        #region private fields
        private ImageDescriptor _imgDes;
        private System.Drawing.Bitmap _img;
        private int _colorSize = 3;
        private byte[] _lct;
        private GraphicEx _graphicEx;
        private byte[] _buffer;        
        #endregion

        #region internal property
        /// <summary>
        /// ����ı���ɫ
        /// </summary>
        public Color32 BgColor
        {
            get
            {
                Color32[] act = PaletteHelper.GetColor32s(LocalColorTable);
                return act[GraphicExtension.TranIndex];
            }            
        }
        /// <summary>
        /// ͼ���ʶ��(Image Descriptor)
        /// һ��GIF�ļ��ڿ��԰������ͼ��
        /// һ��ͼ�����֮�����������һ��ͼ��ı�ʶ����
        /// ͼ���ʶ����0x2C(',')�ַ���ʼ��
        /// �������������ͼ������ʣ�����ͼ��������߼���Ļ�߽��ƫ������
        /// ͼ���С�Լ����޾ֲ���ɫ�б����ɫ�б��С
        /// </summary>
        internal ImageDescriptor ImageDescriptor
        {
            get { return _imgDes; }
            set { _imgDes = value; }
        }
               
        /// <summary>
        /// Gif�ĵ�ɫ��
        /// </summary>
        internal Color32[] Palette
        {
            get
            {
                Color32[] act = PaletteHelper.GetColor32s(LocalColorTable);
                if (GraphicExtension != null && GraphicExtension.TransparencyFlag)
                {
                    act[GraphicExtension.TranIndex] = new Color32(0);
                }
                return act;
            }
        }

        /// <summary>
        /// ͼ��
        /// </summary>
        internal System.Drawing.Bitmap Image
        {
            get { return _img; }
            set { _img = value; }
        }
        
        /// <summary>
        /// ����λ��С
        /// </summary>
        internal int ColorDepth
        {
            get
            {
                return _colorSize;
            }
            set
            {
                _colorSize = value;
            }
        }
        
        /// <summary>
        /// �ֲ���ɫ�б�(Local Color Table)
        /// �������ľֲ���ɫ�б��־��λ�Ļ�������Ҫ�����������ͼ���ʶ��֮��
        /// ����һ���ֲ���ɫ�б��Թ�����������ͼ��ʹ�ã�ע��ʹ��ǰӦ�߱���ԭ������ɫ�б�
        /// ʹ�ý���֮��ظ�ԭ�������ȫ����ɫ�б����һ��GIF�ļ���û���ṩȫ����ɫ�б�
        /// Ҳû���ṩ�ֲ���ɫ�б������Լ�����һ����ɫ�б���ʹ��ϵͳ����ɫ�б�
        /// RGBRGB......
        /// </summary>
        internal byte[] LocalColorTable
        {
            get { return _lct; }
            set { _lct = value; }
        }
        
        /// <summary>
        /// ͼ�ο�����չ(Graphic Control Extension)��һ�����ǿ�ѡ�ģ���Ҫ89a�汾����
        /// ���Է���һ��ͼ���(����ͼ���ʶ�����ֲ���ɫ�б��ͼ������)���ı���չ���ǰ�棬
        /// �������Ƹ���������ĵ�һ��ͼ�󣨻��ı�������Ⱦ(Render)��ʽ
        /// </summary>
        internal GraphicEx GraphicExtension
        {
            get { return _graphicEx; }
            set { _graphicEx = value; }
        }

        /// <summary>
        /// �ӳ�-����һ֮֡���ʱ����
        /// </summary>
        internal short Delay
        {
            get { return _graphicEx.Delay; }
            set { _graphicEx.Delay = value; }
        }
       
        /// <summary>
        /// ����Ǿ���LZWѹ���㷨���������
        /// </summary>
        internal byte[] IndexedPixel
        {
            get { return _buffer; }
            set { _buffer = value; }
        }
        #endregion
    }
    #endregion
}
