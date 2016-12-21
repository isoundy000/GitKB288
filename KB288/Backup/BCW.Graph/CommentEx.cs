﻿using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace BCW.Graph
{
    #region 结构CommentEx
    /// <summary>
    /// 注释扩展(Comment Extension)
    /// 这一部分是可选的（需要89a版本），可以用来记录图形、版权、描述等任何的非图形和控制
    /// 的纯文本数据(7-bit ASCII字符)，注释扩展并不影响对图象数据流的处理，解码器完全可以忽
    /// 略它。存放位置可以是数据流的任何地方，最好不要妨碍控制和数据块，推荐放在数据流的开始或结尾
    /// </summary>
    internal struct CommentEx
    {

        #region 结构字段  
        /// <summary>
        /// Comment Data - 一个或多个数据块组成
        /// </summary>
        internal List<string> CommentDatas;
        #endregion

        #region 方法函数
        internal byte[] GetBuffer()
        {            
            List<byte> list = new List<byte>();
            list.Add(GifExtensions.ExtensionIntroducer);
            list.Add(GifExtensions.CommentLabel);
            foreach (string coment in CommentDatas)
            {
                char[] commentCharArray = coment.ToCharArray();
                list.Add((byte)commentCharArray.Length);
                foreach (char c in commentCharArray)
                {
                    list.Add((byte)c);
                }
            }
            list.Add(GifExtensions.Terminator);
            return list.ToArray();
        }
        #endregion
    }
    #endregion
}
