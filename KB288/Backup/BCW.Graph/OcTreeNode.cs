using System;
using System.Collections.Generic;
using System.Text;

namespace BCW.Graph
{
    internal unsafe class OcTreeNode
    {
        private static int[] mask = new int[8] { 0x80, 0x40, 0x20, 0x10, 0x08, 0x04, 0x02, 0x01 };
        #region ��������
        internal int ColorDepth;
        internal int Level = 0;
        internal bool Leaf = false;     
        internal OcTreeNode[] Children;
        /// <summary>
        /// ��ɫ������
        /// </summary>
        internal int Red = 0;
        /// <summary>
        /// ��ɫ������
        /// </summary>
        internal int Green = 0;
        /// <summary>
        /// ��ɫ������
        /// </summary>
        internal int Blue = 0;
        //�������ɫ��������
        internal int PixelCount = 0;
        internal OcTreeNode NextReducible;
        private int paletteIndex = 0;
        #endregion

        #region �˲����Ĺ��캯��
        /// <summary>
        /// �˲����Ĺ��캯��
        /// </summary>
        /// <param name="leaf">�Ƿ���Ҷ�ӽڵ�</param>
        /// <param name="level">�㼶</param>
        /// <param name="parent">���ڵ�</param>
        internal OcTreeNode(int colorDepth,int level,OcTree tree)
        {
            this.ColorDepth = colorDepth;
            this.Leaf = (colorDepth==level);
            this.Level = level;
            if (!Leaf)
            {
                NextReducible = tree.ReducibleNodes[level];
                tree.ReducibleNodes[level] = this;
                Children = new OcTreeNode[8];
            }
            else
            {
                tree.IncrementLeaves();
            }
        }
        #endregion   
    
        internal void GetPalltte( List<Color32> palltte)
        {
            if (Leaf)
            {
                paletteIndex++;
                //����ﵽ��Ҷ�ӣ��������ɫ����
                Color32 color = new Color32();
                color.Alpha = 255;
                color.Red = (byte)(Red / PixelCount);
                color.Green = (byte)(Green / PixelCount);
                color.Blue = (byte)(Blue / PixelCount);
                palltte.Add(color);
            }
            else
            {
                for (int i = 0; i < ColorDepth; i++)
                {
                    if (Children[i] != null)
                    {
                        Children[i].GetPalltte(palltte);
                    }
                }
            }         
        }
        internal int Reduce()
        {
            Red = Green = Blue = 0;
            int childrenCount = 0;
            for (int i = 0; i < 8; i++)
            {
                if (Children[i] != null)
                {
                    Red += Children[i].Red;
                    Blue += Children[i].Blue;
                    Green += Children[i].Green;
                    PixelCount += Children[i].PixelCount;
                    childrenCount++;
                    Children[i] = null;
                }
            }
            Leaf = true;
            return childrenCount - 1;
        }
        internal void AddColor(Color32* pixel, int level,OcTree tree)
        {
            //�������Ҷ�ˣ���ʾһ����ɫ����������
            if (this.Leaf)
            {
                Increment(pixel);
                tree.TracePrevious(this);
                return;
            }
            int shift = 7 - level;
            int index = ((pixel->Red & mask[level]) >> (shift - 2)) |
                          ((pixel->Green & mask[level]) >> (shift - 1)) |
                          ((pixel->Blue & mask[level]) >> (shift));
            OcTreeNode child = Children[index];
            if (child == null)
            {
                child = new OcTreeNode(ColorDepth, level+1, tree);
                Children[index] = child;
            }            
            child.AddColor(pixel, ++level,tree);            
        }

        internal int GetPaletteIndex(Color32* pixel,int level)
        {
            int pindex = paletteIndex;
            if (!Leaf)
            {
                int shift = 7 - level;
                int index = ((pixel->Red & mask[level]) >> (shift - 2)) |
                              ((pixel->Green & mask[level]) >> (shift - 1)) |
                              ((pixel->Blue & mask[level]) >> (shift));
                OcTreeNode child = Children[index];
                if (child != null)
                {
                    child.GetPaletteIndex(pixel, level + 1);
                }
                else
                    throw new Exception("����Ԥ�ϵ����鷢����!");
            }
            return pindex;
        }


        internal void Increment(Color32* pixel)
        {
            Red += pixel->Red;
            Green += pixel->Green;
            Blue += pixel->Blue;
            PixelCount++;
        }
    }
}
