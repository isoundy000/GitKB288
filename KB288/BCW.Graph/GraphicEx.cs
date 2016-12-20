using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace BCW.Graph
{
    #region 类GraphicEx
    /// <summary>
    /// 图形控制扩展(Graphic Control Extension)这一部分是可选的（需要89a版本），
    /// 可以放在一个图象块(包括图象标识符、局部颜色列表和图象数据)或文本扩展块的前面，
    /// 用来控制跟在它后面的第一个图象（或文本）的渲染(Render)形式
    /// </summary>
    internal class GraphicEx:ExData
    {
        #region private fields
        byte _packed;
        short _delay;
        byte _tranIndex;
        bool _transFlag;
        int _disposalMethod;
        #endregion

        /// <summary>
        /// Block Size - 不包括块终结器，固定值4
        /// </summary>
        internal static readonly byte BlockSize = 4;    

        /// <summary>
        /// i - 用户输入标志
        /// </summary>
        internal bool TransparencyFlag
        {
            get { return _transFlag; }
            set { _transFlag = value; }
        }
      
        /// <summary>
        /// 处置方法(Disposal Method)：指出处置图形的方法，当值为：
        /// 0 - 不使用处置方法
        /// 1 - 不处置图形，把图形从当前位置移去
        /// 2 - 回复到背景色
        /// 3 - 回复到先前状态
        /// 4-7 - 自定义
        /// </summary>
        internal int DisposalMethod
        {
            get { return _disposalMethod; }
            set { _disposalMethod = value; }
        }	
        /// <summary>
        /// Packed
        /// </summary>
        internal byte Packed
        {
            get
            {
                return _packed;
            }
            set
            {
                _packed = value;
            }
        }
        /// <summary>
        /// Delay Time - 单位1/100秒，如果值不为1，表示暂停规定的时间后再继续往下处理数据流
        /// </summary>
        internal short Delay
        {
            get
            {
                return _delay;
            }
            set
            {
                _delay = value;
            }
        }
        /// <summary>
        /// Transparent Color Index - 透明色索引值
        /// </summary>
        internal byte TranIndex
        {
            get
            {
                return _tranIndex;
            }
            set
            {
                _tranIndex = value;
            }
        }
        internal GraphicEx()
        {
        }
        
        internal byte[] GetBuffer()
        {
            List<byte> list = new List<byte>();
            list.Add(GifExtensions.ExtensionIntroducer);
            list.Add(GifExtensions.GraphicControlLabel);
            list.Add(BlockSize);
            int t = 0;
            if (_transFlag)
            {
                t = 1;
            }
            _packed = (byte)((_disposalMethod << 2) | t);
            list.Add(_packed);
            list.AddRange(BitConverter.GetBytes(_delay));
            list.Add(_tranIndex);
            list.Add(GifExtensions.Terminator);
            return list.ToArray();
        }
    }
    #endregion
}
