using System;
using System.Collections.Generic;
using System.Text;

namespace BCW.Graph
{

    internal class BitEncoder
    {
        /// <summary>
        /// 上一次处理剩余的bit数
        /// </summary>
        private int current_Bit = 0;

        /// <summary>
        /// 输出字节数据的集合
        /// </summary>
        internal List<Byte> OutList = new List<byte>();

        /// <summary>
        /// 当前输出字节数据长度
        /// </summary>
        internal int Length
        {
            get
            {
                return OutList.Count;
            }
        }
        int current_Val;     

        internal int inBit = 8;

        internal BitEncoder(int init_bit)
        {
            this.inBit = init_bit;
        }

        /// <summary>
        /// 编码
        /// </summary>
        /// <param name="inByte">输入数据</param>
        /// <param name="inBit">输入数据的bit位数</param>
        internal void Add(int inByte)
        {         
         
            current_Val  |= (inByte << (current_Bit));

            current_Bit += inBit;
         
            while (current_Bit >= 8)
            {   
                byte out_Val = (byte)(current_Val & 0XFF);
                current_Val = current_Val >> 8;
                current_Bit -= 8;             
                OutList.Add(out_Val);
            }
        }


        internal void End()
        {
            while (current_Bit > 0)
            {
                byte out_Val = (byte)(current_Val & 0XFF);
                current_Val = current_Val >> 8;
                current_Bit -= 8;
                OutList.Add((byte)out_Val);
            }
        }
    }
}
