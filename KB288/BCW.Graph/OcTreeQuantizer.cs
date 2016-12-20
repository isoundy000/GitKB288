using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;

namespace BCW.Graph
{
    #region 八叉树颜色量化器OcTreeQuantizer
    /// <summary>
    /// 八叉树颜色量化器
    /// </summary>
    internal class OcTreeQuantizer
    {
        private int maxColors = 0;
        OcTree tree;        
        internal OcTreeQuantizer(int colorDepth)
        {
            if (colorDepth > 8)
            {
                throw new ArgumentOutOfRangeException("colorDepth", colorDepth, "颜色深度不能大于8");
            }
            if (colorDepth <= 0)
            {
                throw new ArgumentOutOfRangeException("colorDepth", colorDepth, "颜色深度不能小于1");
            }
            maxColors = 1 << colorDepth;
            tree = new OcTree(colorDepth);
        }
        internal Color32[] Quantizer(Bitmap bmp)
        {
            BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, bmp.PixelFormat);
            //第一次扫描，扫描整个图像，根据像素值构造八叉树
            FirstPass(bmpData, bmp.Width * bmp.Height);
            Color32[] palltte = tree.Pallette();
            bmp.UnlockBits(bmpData);
            Color32[] list = new Color32[maxColors];
            Array.Copy(palltte, list, palltte.Length);
            for (int i = palltte.Length; i < maxColors-1; i++)
            {
                list[i] = new Color32();
            }
            list[maxColors-1] = new Color32();
            return list;
        }    
        void FirstPass(BitmapData bmpData,int pixelCount)
        {            
            unsafe
            {
                Color32* colorPointer = (Color32*)bmpData.Scan0.ToPointer();              
                for (int i = 0; i < pixelCount; i++)
                {
                    tree.AddColor(colorPointer);
                    colorPointer++;
                }
            }           
        }
    }
    #endregion
}
