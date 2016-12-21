using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;

namespace BCW.Graph
{
    #region �˲�����ɫ������OcTreeQuantizer
    /// <summary>
    /// �˲�����ɫ������
    /// </summary>
    internal class OcTreeQuantizer
    {
        private int maxColors = 0;
        OcTree tree;        
        internal OcTreeQuantizer(int colorDepth)
        {
            if (colorDepth > 8)
            {
                throw new ArgumentOutOfRangeException("colorDepth", colorDepth, "��ɫ��Ȳ��ܴ���8");
            }
            if (colorDepth <= 0)
            {
                throw new ArgumentOutOfRangeException("colorDepth", colorDepth, "��ɫ��Ȳ���С��1");
            }
            maxColors = 1 << colorDepth;
            tree = new OcTree(colorDepth);
        }
        internal Color32[] Quantizer(Bitmap bmp)
        {
            BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, bmp.PixelFormat);
            //��һ��ɨ�裬ɨ������ͼ�񣬸�������ֵ����˲���
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
