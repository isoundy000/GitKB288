using System;
using System.Collections.Generic;
using System.Text;

namespace BCW.Graph
{
    #region 类GifExtensions
    /// <summary>
    /// 扩展块中的一些常量
    /// </summary>
    internal class GifExtensions
    {
        /// <summary>
        /// Extension Introducer - 标识这是一个扩展块，固定值0x21
        /// </summary>          
        internal const byte ExtensionIntroducer = 0x21;

        /// <summary>
        /// lock Terminator - 标识注释块结束，固定值0
        /// </summary>
        internal const byte Terminator = 0;


        /// <summary>
        /// Application Extension Label - 标识这是一个应用程序扩展块，固定值0xFF 
        /// </summary>
        internal const byte ApplicationExtensionLabel = 0xFF;


        /// <summary>
        /// Comment Label - 标识这是一个注释块，固定值0xFE
        /// </summary>
        internal const byte CommentLabel = 0xFE;


        /// <summary>
        /// 图象标识符开始，固定值为','
        /// </summary>
        internal const byte ImageDescriptorLabel = 0x2C;

        /// <summary>
        /// Plain Text Label - 标识这是一个图形文本扩展块，固定值0x01
        /// </summary>
        internal const byte PlainTextLabel = 0x01;

        /// <summary>
        /// Graphic Control Label - 标识这是一个图形控制扩展块，固定值0xF9
        /// </summary>
        internal const byte GraphicControlLabel = 0xF9;

        /// <summary>
        /// 图像的标示
        /// </summary>
        internal const byte ImageLabel = 0x2C;

        /// <summary>
        /// 文件结尾
        /// </summary>
        internal const byte EndIntroducer = 0x3B;
    }
    #endregion
}
