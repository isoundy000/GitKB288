using System;
using System.Collections.Generic;
using System.Text;

namespace BCW.Graph
{
    #region ��GifExtensions
    /// <summary>
    /// ��չ���е�һЩ����
    /// </summary>
    internal class GifExtensions
    {
        /// <summary>
        /// Extension Introducer - ��ʶ����һ����չ�飬�̶�ֵ0x21
        /// </summary>          
        internal const byte ExtensionIntroducer = 0x21;

        /// <summary>
        /// lock Terminator - ��ʶע�Ϳ�������̶�ֵ0
        /// </summary>
        internal const byte Terminator = 0;


        /// <summary>
        /// Application Extension Label - ��ʶ����һ��Ӧ�ó�����չ�飬�̶�ֵ0xFF 
        /// </summary>
        internal const byte ApplicationExtensionLabel = 0xFF;


        /// <summary>
        /// Comment Label - ��ʶ����һ��ע�Ϳ飬�̶�ֵ0xFE
        /// </summary>
        internal const byte CommentLabel = 0xFE;


        /// <summary>
        /// ͼ���ʶ����ʼ���̶�ֵΪ','
        /// </summary>
        internal const byte ImageDescriptorLabel = 0x2C;

        /// <summary>
        /// Plain Text Label - ��ʶ����һ��ͼ���ı���չ�飬�̶�ֵ0x01
        /// </summary>
        internal const byte PlainTextLabel = 0x01;

        /// <summary>
        /// Graphic Control Label - ��ʶ����һ��ͼ�ο�����չ�飬�̶�ֵ0xF9
        /// </summary>
        internal const byte GraphicControlLabel = 0xF9;

        /// <summary>
        /// ͼ��ı�ʾ
        /// </summary>
        internal const byte ImageLabel = 0x2C;

        /// <summary>
        /// �ļ���β
        /// </summary>
        internal const byte EndIntroducer = 0x3B;
    }
    #endregion
}
