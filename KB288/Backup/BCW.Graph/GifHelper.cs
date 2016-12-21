using System;
using System.Web;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections;

namespace BCW.Graph
{
    public enum SizeMode
    {
        Large,
        Normal
    }

    public class GifHelper
    {
                    
        /// <summary>
        /// 构造方法
        /// </summary>
        public GifHelper()
        {
        }

        #region 对gif动画添加水印-字体颜色确定
        /// <summary>
        /// 对gif动画添加水印
        /// </summary>
        /// <param name="gifFilePath">原gif动画的路径</param>
        /// <param name="text">水印文字</param>
        /// <param name="textForceColor">水印文字的颜色，因为gif不是真彩色图片，所以在显示的时候，该颜色可能有所误差，但基本上可以确定颜色范围</param>
        /// <param name="font">字体</param>
        /// <param name="x">水印位置横坐标</param>
        /// <param name="y">水印位置纵坐标</param>
        /// <param name="outputPath">输出路径</param>
        public void WaterMark(string gifFilePath, SizeMode sizeMode, string text, Color textForceColor, Font font, float x, float y, string outputPath)
        {
            if (!File.Exists(gifFilePath))
            {
                throw new IOException(string.Format("文件{0}不存在!", gifFilePath));
            }
            using (Bitmap ora_Img = new Bitmap(gifFilePath))
            {
                if (ora_Img.RawFormat.Guid != ImageFormat.Gif.Guid)
                {
                    throw new IOException(string.Format("文件{0}!", gifFilePath));
                }
            }
            GifImage gifImage = GifDecoder.Decode(gifFilePath);
            if (sizeMode == SizeMode.Large)
            {
                ThinkDisposalMethod(gifImage);
            }
            Color textColor = textForceColor;
            int frameCount = 0;
            foreach (GifFrame f in gifImage.Frames)
            {
                if ((sizeMode == SizeMode.Normal && frameCount++ == 0) || sizeMode == SizeMode.Large)
                {
                    Graphics g = Graphics.FromImage(f.Image);
                    g.DrawString(text, font, new SolidBrush(textColor), new PointF(x, y));
                    g.Dispose();
                    bool hasTextColor = false;
                    Color32[] colors = PaletteHelper.GetColor32s(f.LocalColorTable);
                    foreach (Color32 c in colors)
                    {
                        if (c.ARGB == textColor.ToArgb())
                        {
                            hasTextColor = true;
                            break;
                        }
                    }
                    if (!hasTextColor)
                    {
                        if (f.Palette.Length < 256)
                        {
                            int newSize = f.Palette.Length * 2;
                            Color32[] newColors = new Color32[newSize];
                            newColors[f.Palette.Length] = new Color32(textColor.ToArgb());
                            Array.Copy(colors, newColors, colors.Length);
                            byte[] lct = new byte[newColors.Length * 3];
                            int index = 0;
                            foreach (Color32 c in newColors)
                            {
                                lct[index++] = c.Red;
                                lct[index++] = c.Green;
                                lct[index++] = c.Blue;
                            }
                            f.LocalColorTable = lct;
                            f.ImageDescriptor.LctFlag = true;
                            f.ImageDescriptor.LctSize = newSize;
                            f.ColorDepth = f.ColorDepth + 1;
                        }
                        else
                        {
                            OcTreeQuantizer q = new OcTreeQuantizer(8);
                            Color32[] cs = q.Quantizer(f.Image);
                            byte[] lct = new byte[cs.Length * 3];
                            int index = 0;
                            int colorCount = 0;
                            foreach (Color32 c in cs)
                            {
                                lct[index++] = c.Red;
                                lct[index++] = c.Green;
                                lct[index++] = c.Blue;
                                if (c.ARGB == f.BgColor.ARGB)
                                {
                                    f.GraphicExtension.TranIndex = (byte)colorCount;
                                }
                                colorCount++;
                            }
                            Quantizer(f.Image, cs);
                            f.LocalColorTable = lct;
                            f.ImageDescriptor.LctFlag = true;
                            f.ImageDescriptor.LctSize = 256;
                            f.ColorDepth = 8;
                        }
                    }
                }
            }
            GifEncoder.Encode(gifImage, outputPath);
        }
        #endregion

        #region  对gif动画添加水印
        /// <summary>
        /// 对gif动画添加水印
        /// </summary>
        /// <param name="gifFilePath">图像文件</param>
        /// <param name="outputPath">图像保存路径</param>
        /// <param name="p_text">水印文字</param>
        /// <param name="p_color">水印文字色</param>
        /// <param name="p_font">水印文字字体</param>
        /// <param name="p_fontsize">水印文字大小</param>
        /// <param name="p_position">水印位置</param>
        /// 
        public void SmartWaterMark(string gifFilePath, string outputPath, string p_text, string p_color, string p_font, float p_fontsize, int p_position)
        {
            if (!File.Exists(gifFilePath))
            {
                throw new IOException(string.Format("文件{0}不存在!", gifFilePath));
            }
            using (Bitmap ora_Img = new Bitmap(gifFilePath))
            {
                if (ora_Img.RawFormat.Guid != ImageFormat.Gif.Guid)
                {
                    throw new IOException(string.Format("文件{0}!", gifFilePath));
                }
            }

            GifImage gifImage = GifDecoder.Decode(gifFilePath);

            ThinkDisposalMethod(gifImage);
            foreach (GifFrame f in gifImage.Frames)
            {
                Graphics g = Graphics.FromImage(f.Image);
                Font font = new Font(p_font, p_fontsize, true ? FontStyle.Bold : FontStyle.Regular, GraphicsUnit.Pixel);
                SizeF fontSize = g.MeasureString(p_text, font);

                SolidBrush brush = new SolidBrush(ColorTranslator.FromHtml(p_color));
                int x = 0;
                int y = 0;
                //水印位置
                getImageST(p_position, f.ImageDescriptor.Width, f.ImageDescriptor.Height, (int)fontSize.Width, (int)fontSize.Height, out x, out y);

                g.DrawString(p_text, font, brush, x, y);
                g.Dispose();
            }

            if (string.IsNullOrEmpty(outputPath))
                GifEncoder.Encode(gifImage, gifFilePath);
            else
                GifEncoder.Encode(gifImage, outputPath);
        }
        #endregion

        #region 对gif动画添加图片水印
        /// <summary>
        /// 对gif动画添加图片水印
        /// </summary>
        /// <param name="gifFilePath">图像文件</param>
        /// <param name="outputPath">图像保存路径</param>
        /// <param name="p_SimageFile">水印图片文件</param>
        /// <param name="p_position">水印位置</param>
        /// <param name="p_Transparent">透明度</param>
        public void WaterMark(string gifFilePath, string outputPath, string p_SimageFile, int p_position, int p_Transparent)
        {
            if (!File.Exists(gifFilePath))
            {
                throw new IOException(string.Format("文件{0}不存在!", gifFilePath));
            }
            using (Bitmap ora_Img = new Bitmap(gifFilePath))
            {
                if (ora_Img.RawFormat.Guid != ImageFormat.Gif.Guid)
                {
                    throw new IOException(string.Format("文件{0}!", gifFilePath));
                }
            }
            GifImage gifImage = GifDecoder.Decode(gifFilePath);
            Image waterImg = new Bitmap(p_SimageFile);
            ThinkDisposalMethod(gifImage);
            foreach (GifFrame f in gifImage.Frames)
            {
                Graphics g = Graphics.FromImage(f.Image);

                int x = 0;
                int y = 0;
                //水印位置
                getImageST(p_position, f.ImageDescriptor.Width, f.ImageDescriptor.Height, waterImg.Width, waterImg.Height, out x, out y);

                g.DrawImage(waterImg, x, y);
                g.Dispose();
                OcTreeQuantizer q = new OcTreeQuantizer(8);
                Color32[] cs = q.Quantizer(f.Image);
                byte[] lct = new byte[cs.Length * 3];
                int index = 0;
                int colorCount = 0;
                foreach (Color32 c in cs)
                {
                    lct[index++] = c.Red;
                    lct[index++] = c.Green;
                    lct[index++] = c.Blue;
                    if (c.ARGB == f.BgColor.ARGB)
                    {
                        f.GraphicExtension.TranIndex = (byte)colorCount;
                    }
                    colorCount++;
                }
                Quantizer(f.Image, cs);
                f.LocalColorTable = lct;
                f.ImageDescriptor.LctFlag = true;
                f.ImageDescriptor.LctSize = 256;
                f.ColorDepth = 8;
            }
            if (string.IsNullOrEmpty(outputPath))
                GifEncoder.Encode(gifImage, gifFilePath);
            else
                GifEncoder.Encode(gifImage, outputPath);

        }
        #endregion

        #region gif动画缩略
        /// <summary>
        /// 获取gif动画的缩略图
        /// </summary>
        /// <param name="gifFilePath">原gif图片路径</param>
        /// <param name="rate">缩放大小</param>
        /// <param name="outputPath">缩略图大小</param>
        public void GetThumbnail(string gifFilePath, double rate, string outputPath)
        {
            if (!File.Exists(gifFilePath))
            {
                throw new IOException(string.Format("文件{0}不存在!", gifFilePath));
            }
            using (Bitmap ora_Img = new Bitmap(gifFilePath))
            {
                if (ora_Img.RawFormat.Guid != ImageFormat.Gif.Guid)
                {
                    throw new IOException(string.Format("文件{0}!", gifFilePath));
                }
            }
            GifImage gifImage = GifDecoder.Decode(gifFilePath);
            if (rate != 1.0)
            {
                gifImage.LogicalScreenDescriptor.Width = (short)(gifImage.LogicalScreenDescriptor.Width * rate);
                gifImage.LogicalScreenDescriptor.Height = (short)(gifImage.LogicalScreenDescriptor.Height * rate);
                int index = 0;
                foreach (GifFrame f in gifImage.Frames)
                {
                    f.ImageDescriptor.XOffSet = (short)(f.ImageDescriptor.XOffSet * rate);
                    f.ImageDescriptor.YOffSet = (short)(f.ImageDescriptor.YOffSet * rate);
                    f.ImageDescriptor.Width = (short)(f.ImageDescriptor.Width * rate);
                    f.ImageDescriptor.Height = (short)(f.ImageDescriptor.Height * rate);
                    if (f.ImageDescriptor.Width == 0)
                    {
                        f.ImageDescriptor.Width = 1;
                    }
                    if (f.ImageDescriptor.Height == 0)
                    {
                        f.ImageDescriptor.Height = 1;
                    }
                    Bitmap bmp = new Bitmap(f.ImageDescriptor.Width, f.ImageDescriptor.Height);
                    Graphics g = Graphics.FromImage(bmp);
                    g.DrawImage(f.Image, new Rectangle(0, 0, f.ImageDescriptor.Width, f.ImageDescriptor.Height));
                    g.Dispose();                  
                    Quantizer(bmp, f.Palette);
                    f.Image.Dispose();
                    f.Image = bmp;
                    index++;
                }
                GifEncoder.Encode(gifImage, outputPath);
            }
        }

        /// <summary>
        /// 调整图像宽度高度 (保持比例)
        /// </summary>
        /// <param name="p_imageFile">图像文件</param>
        /// <param name="p_width">图像大于该宽度将会被调整为该宽度尺寸</param>
        /// <param name="p_height">图像大于该高度将会被调整为该高度尺寸</param>
        /// <param name="p_bool">是否保持比例</param>
        public void GetThumbnail(string p_imageFile, int p_width, int p_height, bool p_bool, out string SavePath)
        {
            SavePath = "/Files/temp.jpg";
            GetThumbnail(p_imageFile, HttpContext.Current.Server.MapPath(SavePath), p_width, p_height, p_bool);
        }

        /// <summary>
        /// 获取gif动画的缩略图
        /// </summary>
        /// <param name="gifFilePath">原gif图片路径</param>
        /// <param name="p_width">图像大于该宽度将会被调整为该宽度尺寸</param>
        /// <param name="p_height">图像大于该高度将会被调整为该高度尺寸</param>
        /// <param name="p_bool">是否保持比例</param>
        /// <param name="outputPath">输出缩略图</param>
        public void GetThumbnail(string gifFilePath, string outputPath, int p_width, int p_height, bool p_bool)
        {
            if (!File.Exists(gifFilePath))
            {
                throw new IOException(string.Format("文件{0}不存在!", gifFilePath));
            }
            using (Bitmap ora_Img = new Bitmap(gifFilePath))
            {
                if (ora_Img.RawFormat.Guid != ImageFormat.Gif.Guid)
                {
                    throw new IOException(string.Format("文件{0}!", gifFilePath));
                }
            }
            GifImage gifImage = GifDecoder.Decode(gifFilePath);
            //计算比例
            int originalWidth = gifImage.Width;
            int originalHeight = gifImage.Height;

            double scaleWidth = 0;
            double scaleHeight = 0;
            double scale;

            int newWidth = 0;
            int newHeight = 0;
            if (p_bool)
            {
                // 计算图像缩小的比例
                if (originalWidth > p_width)
                    scaleWidth = (double)originalWidth / (double)p_width;

                if (originalHeight > p_height)
                    scaleHeight = (double)originalHeight / (double)p_height;

                if (scaleWidth != 0 || scaleHeight != 0)
                {
                    // 比较缩小比例, 取缩小比例大的值
                    if (scaleWidth > scaleHeight)
                    {
                        scale = scaleWidth;
                        newWidth = p_width;
                        newHeight = (int)((double)originalHeight / scale);
                    }
                    else
                    {
                        scale = scaleHeight;
                        newWidth = (int)((double)originalWidth / scale);
                        newHeight = p_height;
                    }
                }
                else
                {
                    newWidth = originalWidth;
                    newHeight = originalHeight;
                }
            }
            else
            {
                newWidth = p_width;
                newHeight = p_height;
            }
            double rate1 = (double)newWidth / (double)originalWidth;
            double rate2 = (double)newHeight / (double)originalHeight;
            gifImage.LogicalScreenDescriptor.Width = (short)(gifImage.LogicalScreenDescriptor.Width * rate1);
            gifImage.LogicalScreenDescriptor.Height = (short)(gifImage.LogicalScreenDescriptor.Height * rate2);
            int index = 0;
            foreach (GifFrame f in gifImage.Frames)
            {
                f.ImageDescriptor.XOffSet = (short)(f.ImageDescriptor.XOffSet * rate1);
                f.ImageDescriptor.YOffSet = (short)(f.ImageDescriptor.YOffSet * rate2);
                f.ImageDescriptor.Width = (short)(f.ImageDescriptor.Width * rate1);
                f.ImageDescriptor.Height = (short)(f.ImageDescriptor.Height * rate2);
                if (f.ImageDescriptor.Width == 0)
                {
                    f.ImageDescriptor.Width = 1;
                }
                if (f.ImageDescriptor.Height == 0)
                {
                    f.ImageDescriptor.Height = 1;
                }
                Bitmap bmp = new Bitmap(f.ImageDescriptor.Width, f.ImageDescriptor.Height);
                Graphics g = Graphics.FromImage(bmp);
                g.DrawImage(f.Image, new Rectangle(0, 0, f.ImageDescriptor.Width, f.ImageDescriptor.Height));
                g.Dispose();
                Quantizer(bmp, f.Palette);
                f.Image.Dispose();
                f.Image = bmp;
                index++;
            }

            if (string.IsNullOrEmpty(outputPath))
                GifEncoder.Encode(gifImage, gifFilePath);
            else
                GifEncoder.Encode(gifImage, outputPath);
        }


        #region 对图像进行量化，使其适应调色板
        /// <summary>
        /// 对图像进行量化，使其适应调色板
        /// </summary>
        /// <param name="bmp">图像</param>
        /// <param name="colorTab">调色板</param>
        void Quantizer(Bitmap bmp, Color32[] colorTab)
        {
            BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadWrite, bmp.PixelFormat);
            Hashtable table = new Hashtable();
            unsafe
            {
                int* bmpScan = (int*)bmpData.Scan0.ToPointer();
                for (int i = 0; i < bmp.Height * bmp.Width; i++)
                {
                    Color c = Color.FromArgb(bmpScan[i]);
                    int rc = FindCloser(c, colorTab, table);
                    Color newc = Color.FromArgb(rc);
                    bmpScan[i] = rc;
                }
            }
            bmp.UnlockBits(bmpData);
        }
        /// <summary>
        /// 对图像进行量化，使其适应调色板
        /// </summary>
        /// <param name="bmp">图像</param>
        /// <param name="colorTab">调色板</param>
        void Quantizer(Bitmap bmp, int[] colorTab)
        {
            BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadWrite, bmp.PixelFormat);
            Hashtable table = new Hashtable();
            unsafe
            {
                int* bmpScan = (int*)bmpData.Scan0.ToPointer();
                for (int i = 0; i < bmp.Height * bmp.Width; i++)
                {
                    Color c = Color.FromArgb(bmpScan[i]);
                    int rc = FindCloser(c, colorTab, table);
                    Color newc = Color.FromArgb(rc);
                    bmpScan[i] = rc;
                }
            }
            bmp.UnlockBits(bmpData);
        }
        int FindCloser(Color c, Color32[] act, Hashtable table)
        {
            if (table.Contains(c))
            {
                return ((Color32)table[c]).ARGB;
            }
            int index = 0;
            int min = 0;
            int minIndex = 0;
            while (index < act.Length)
            {
                Color ac = act[index].Color;
                int tempIndex = index;
                int cr = Math.Abs(c.R - ac.R);
                int cg = Math.Abs(c.G - ac.G);
                int cb = Math.Abs(c.B - ac.B);
                int ca = Math.Abs(c.A - ac.A);
                int result = cr + cg + cb + ca;
                if (result == 0)
                {
                    minIndex = tempIndex;
                    break;
                }
                if (tempIndex == 0)
                {
                    min = result;
                }
                else
                {
                    if (result < min)
                    {
                        min = result;
                        minIndex = tempIndex;
                    }
                }
                index++;
            }
            if (!table.Contains(c))
            {
                table.Add(c, act[minIndex]);
            }
            return act[minIndex].ARGB;
        }
        int FindCloser(Color c, int[] act, Hashtable table)
        {
            if (table.Contains(c))
            {
                return Convert.ToInt32(table[c]);
            }
            int index = 0;
            int min = 0;
            int minIndex = 0;
            while (index < act.Length)
            {
                Color ac = Color.FromArgb(act[index]);
                int tempIndex = index;
                int cr = Math.Abs(c.R - ac.R);
                int cg = Math.Abs(c.G - ac.G);
                int cb = Math.Abs(c.B - ac.B);
                int ca = Math.Abs(c.A - ac.A);
                int result = cr + cg + cb + ca;
                if (result == 0)
                {
                    minIndex = tempIndex;
                    break;
                }
                if (tempIndex == 0)
                {
                    min = result;
                }
                else
                {
                    if (result < min)
                    {
                        min = result;
                        minIndex = tempIndex;
                    }
                }
                index++;
            }
            if (!table.Contains(c))
            {
                table.Add(c, act[minIndex]);
            }
            return act[minIndex];
        }
        #endregion

        #endregion

        #region Gif动画单色化
        /// <summary>
        /// Gif动画单色化
        /// </summary>
        /// <param name="gifFilePath">原动画路径</param>
        /// <param name="outputPath">单色后动画路径</param>
        public void Monochrome(string gifFilePath, string outputPath)
        {
            if (!File.Exists(gifFilePath))
            {
                throw new IOException(string.Format("文件{0}不存在!", gifFilePath));
            }
            using (Bitmap ora_Img = new Bitmap(gifFilePath))
            {
                if (ora_Img.RawFormat.Guid != ImageFormat.Gif.Guid)
                {
                    throw new IOException(string.Format("文件{0}!", gifFilePath));
                }
            }
            GifImage gifImage = GifDecoder.Decode(gifFilePath);
            int transIndex = gifImage.LogicalScreenDescriptor.BgColorIndex;
            int c1 = (255 << 24) | (158 << 16) | (128 << 8) | 128;
            int c2 = (255 << 24) | (0 << 16) | (0 << 8) | 0;
            int c3 = (255 << 24) | (255 << 16) | (255 << 8) | 255;
            int c4 = (255 << 24) | (0 << 16) | (0 << 8) | 0;
            int[] palette = new int[] { c1, c2, c3, c4 };
            byte[] buffer = new byte[12] { 128, 128, 128, 0, 0, 0, 255, 255, 255, 0, 0, 0 };
            gifImage.GlobalColorTable = buffer;
            gifImage.LogicalScreenDescriptor.BgColorIndex = 0;
            gifImage.LogicalScreenDescriptor.GlobalColorTableSize = 4;
            gifImage.LogicalScreenDescriptor.GlobalColorTableFlag = true;
            int index = 0;
            foreach (GifFrame f in gifImage.Frames)
            {
                Color32[] act = PaletteHelper.GetColor32s(f.LocalColorTable);
                f.LocalColorTable = buffer;
                Color bgC = act[(transIndex / 3)].Color;
                byte bgGray = (byte)(bgC.R * 0.3 + bgC.G * 0.59 + bgC.B * 0.11);
                BitmapData bmpData = f.Image.LockBits(new Rectangle(0, 0, f.Image.Width, f.Image.Height), ImageLockMode.ReadWrite, f.Image.PixelFormat);
                unsafe
                {
                    int* p = (int*)bmpData.Scan0.ToPointer();
                    for (int i = 0; i < f.Image.Width * f.Image.Height; i++)
                    {
                        if (p[i] == 0)
                        {
                            p[i] = c1;

                        }
                        else
                        {
                            Color c = Color.FromArgb(p[i]);
                            int gray = (byte)(c.R * 0.3 + c.G * 0.59 + c.B * 0.11);
                            if (gray > bgGray)
                            {
                                if (bgGray > 128)
                                {
                                    p[i] = c2;
                                }
                                else
                                {
                                    p[i] = c3;
                                }
                            }
                            else if (gray < bgGray)
                            {
                                if (bgGray > 128)
                                {
                                    p[i] = c3;
                                }
                                else
                                {
                                    p[i] = c2;
                                }
                            }
                            else
                            {
                                p[i] = c1;
                            }
                        }
                    }
                }
                f.Image.UnlockBits(bmpData);
                f.GraphicExtension.TranIndex = 0;
                f.ColorDepth = 2;
                f.ImageDescriptor.LctFlag = false;
                index++;
            }
            GifEncoder.Encode(gifImage, outputPath);
        }
        #endregion

        #region 合并多个gif动画,在时间坐标上
        Size FindMaxSize(List<string> sources)
        {
            List<int> widths = new List<int>();
            List<int> heights = new List<int>();
            foreach (string s in sources)
            {
                Bitmap bmp = new Bitmap(s);
                widths.Add(bmp.Width);
                heights.Add(bmp.Height);
                bmp.Dispose();
            }
            widths.Sort();
            heights.Sort();
            return new Size(widths[widths.Count - 1], heights[heights.Count - 1]);
        }

        /// <summary>
        /// 合并多个gif文件
        /// </summary>
        /// <param name="sourceGifs">原图像路径集合</param>
        /// <param name="outGif">合并后图像路径</param>
        /// <param name="delay">间隔时间</param>
        /// <param name="repeat">是否重复播放</param> 
        public void Merge(List<string> sourceGifs, string outGif, short delay, bool repeat)
        {
            GifImage gifImage = null;
            int index = 0;
            short lastDelay = delay;
            foreach (string source in sourceGifs)
            {
                if (!File.Exists(source))
                {
                    throw new IOException(string.Format("文件{0}不存在!", source));
                }
                using (Bitmap ora_Img = new Bitmap(source))
                {
                    if (ora_Img.RawFormat.Guid != ImageFormat.Gif.Guid)
                    {
                        throw new IOException(string.Format("文件{0}!", source));
                    }
                }
                GifImage gif = GifDecoder.Decode(source);
                if (index == 0)
                {
                    gifImage = gif;
                }
                int frameCount = 0;
                foreach (GifFrame f in gif.Frames)
                {
                    if (frameCount == 0 && f.GraphicExtension.DisposalMethod == 0)
                    {
                        f.GraphicExtension.DisposalMethod = 2;
                    }
                    if (!f.ImageDescriptor.LctFlag)
                    {
                        f.ImageDescriptor.LctSize = f.LocalColorTable.Length / 3;
                        f.ImageDescriptor.LctFlag = true;
                        f.GraphicExtension.TranIndex = gif.LogicalScreenDescriptor.BgColorIndex;
                        f.LocalColorTable = gif.GlobalColorTable;
                    }
                    if (frameCount == 0)
                    {
                        f.Delay = f.GraphicExtension.Delay = lastDelay;
                    }
                    if (f.Delay == 0)
                    {
                        f.Delay = f.GraphicExtension.Delay = lastDelay;
                    }
                    f.ColorDepth = (byte)(Math.Log(f.ImageDescriptor.LctSize, 2));
                    lastDelay = f.GraphicExtension.Delay;
                    frameCount++;
                }
                gifImage.Frames.AddRange(gif.Frames);
                index++;
            }

            if (repeat && gifImage.ApplictionExtensions.Count == 0)
            {
                ApplicationEx ae = new ApplicationEx();
                gifImage.ApplictionExtensions.Add(ae);
            }
            gifImage.LogicalScreenDescriptor.PixcelAspect = 0;
            Size maxSize = FindMaxSize(sourceGifs);
            gifImage.LogicalScreenDescriptor.Width = (short)maxSize.Width;
            gifImage.LogicalScreenDescriptor.Height = (short)maxSize.Height;
            GifEncoder.Encode(gifImage, outGif);

        }
        #endregion

        #region 合并多个gif动画,在空间坐标上
        /// <summary>
        /// 合并多个gif动画,在空间坐标上
        /// </summary>
        /// <param name="sourceGifs">原图像</param>
        /// <param name="outPath">合并后图像</param>
        public void Merge(List<string> sourceGifs, string outPath)
        {
            List<List<GifFrame>> frames = new List<List<GifFrame>>();
            foreach (string source in sourceGifs)
            {
                if (!File.Exists(source))
                {
                    throw new IOException(string.Format("文件{0}不存在!", source));
                }
                using (Bitmap ora_Img = new Bitmap(source))
                {
                    if (ora_Img.RawFormat.Guid != ImageFormat.Gif.Guid)
                    {
                        throw new IOException(string.Format("文件{0}!", source));
                    }
                }
                GifImage gifImage = GifDecoder.Decode(source);
                ThinkDisposalMethod(gifImage);
                int index = 0;
                foreach (GifFrame f in gifImage.Frames)
                {
                    if (frames.Count <= index)
                    {
                        List<GifFrame> list = new List<GifFrame>();
                        frames.Add(list);
                    }
                    List<GifFrame> frameList = frames[index];
                    frameList.Add(f);
                    index++;
                }
            }
            List<GifFrame> frameCol = new List<GifFrame>();
            int frameIndex = 0;
            foreach (List<GifFrame> fs in frames)
            {
                GifFrame frame = Merge(fs);
                frameCol.Add(frame);
                if (frame.Image.Width != frameCol[0].Image.Width
                    || frame.Image.Height != frameCol[0].Image.Height)
                {
                    frame.ImageDescriptor.XOffSet = frames[frameIndex][0].ImageDescriptor.XOffSet;
                    frame.ImageDescriptor.YOffSet = frames[frameIndex][0].ImageDescriptor.YOffSet;
                    frame.GraphicExtension.DisposalMethod = frames[frameIndex][0].GraphicExtension.DisposalMethod;
                }
                frame.GraphicExtension.Delay = frame.Delay = frames[frameIndex][0].Delay;
                frameIndex++;
            }
            GifImage gifImg = new GifImage();
            gifImg.Header = "GIF89a";
            LogicalScreenDescriptor lcd = new LogicalScreenDescriptor();
            lcd.Width = (short)frameCol[0].Image.Width;
            lcd.Height = (short)frameCol[0].Image.Height;
            gifImg.LogicalScreenDescriptor = lcd;
            ApplicationEx ape = new ApplicationEx();
            List<ApplicationEx> apps = new List<ApplicationEx>();
            apps.Add(ape);
            gifImg.ApplictionExtensions = apps;
            gifImg.Frames = frameCol;
            GifEncoder.Encode(gifImg, outPath);
        }
        #endregion

        #region 将Gif图片进行旋转或者翻转
        /// <summary>
        /// 将Gif图片进行旋转或者翻转
        /// </summary>
        /// <param name="gifFilePath">原图像路径</param>
        /// <param name="rotateType">翻转或者旋转方式</param>
        /// <param name="outputPath">输出路径</param>
        public void Rotate(string gifFilePath, RotateFlipType rotateType, string outputPath)
        {
            if (!File.Exists(gifFilePath))
            {
                throw new IOException(string.Format("文件{0}不存在!", gifFilePath));
            }
            using (Bitmap ora_Img = new Bitmap(gifFilePath))
            {
                if (ora_Img.RawFormat.Guid != ImageFormat.Gif.Guid)
                {
                    throw new IOException(string.Format("文件{0}!", gifFilePath));
                }
            }
            GifImage gifImage = GifDecoder.Decode(gifFilePath);
            ThinkDisposalMethod(gifImage);
            int index = 0;
            foreach (GifFrame f in gifImage.Frames)
            {
                f.Image.RotateFlip(rotateType);
                f.ImageDescriptor.Width = (short)f.Image.Width;
                f.ImageDescriptor.Height = (short)f.Image.Height;
                if (index++ == 0)
                {
                    gifImage.LogicalScreenDescriptor.Width = (short)f.Image.Width;
                    gifImage.LogicalScreenDescriptor.Height = (short)f.Image.Height;
                }
            }
            GifEncoder.Encode(gifImage, outputPath);
        }
        #endregion

        #region 对Gif图片进行剪裁
        /// <summary>
        /// 对Gif图片进行剪裁
        /// </summary>
        /// <param name="gifFilePath">原图像</param>
        /// <param name="rect">剪裁区域</param>
        /// <param name="outFilePath">输出路径</param>
        public void Crop(string gifFilePath, Rectangle rect, string outFilePath)
        {
            if (!File.Exists(gifFilePath))
            {
                throw new IOException(string.Format("文件{0}不存在!", gifFilePath));
            }
            using (Bitmap ora_Img = new Bitmap(gifFilePath))
            {
                if (ora_Img.RawFormat.Guid != ImageFormat.Gif.Guid)
                {
                    throw new IOException(string.Format("文件{0}!", gifFilePath));
                }
            }
            GifImage gifImage = GifDecoder.Decode(gifFilePath);
            ThinkDisposalMethod(gifImage);
            int index = 0;
            foreach (GifFrame f in gifImage.Frames)
            {
                f.Image = f.Image.Clone(rect, f.Image.PixelFormat);
                f.ImageDescriptor.Width = (short)rect.Width;
                f.ImageDescriptor.Height = (short)rect.Height;
                if (index++ == 0)
                {
                    gifImage.LogicalScreenDescriptor.Width = (short)rect.Width;
                    gifImage.LogicalScreenDescriptor.Height = (short)rect.Height;
                }
            }
            GifEncoder.Encode(gifImage, outFilePath);
        }
        #endregion

        #region 私有方法
        void ThinkDisposalMethod(GifImage gifImage)
        {
            int lastDisposal = 0;
            Bitmap lastImage = null;
            int index = 0;
            short width = gifImage.Width;
            short height = gifImage.Height;
            foreach (GifFrame f in gifImage.Frames)
            {
                int xOffSet = f.ImageDescriptor.XOffSet;
                int yOffSet = f.ImageDescriptor.YOffSet;
                int iw = f.ImageDescriptor.Width;
                int ih = f.ImageDescriptor.Height;
                if ((f.Image.Width != width || f.Image.Height != height))
                {
                    f.ImageDescriptor.XOffSet = 0;
                    f.ImageDescriptor.YOffSet = 0;
                    f.ImageDescriptor.Width = (short)width;
                    f.ImageDescriptor.Height = (short)height;
                }
                int transIndex = -1;
                if (f.GraphicExtension.TransparencyFlag)
                {
                    transIndex = f.GraphicExtension.TranIndex;
                }
                if (iw == width && ih == height && index == 0)
                {

                }
                else
                {
                    int bgColor = Convert.ToInt32(gifImage.GlobalColorIndexedTable[f.GraphicExtension.TranIndex]);
                    Color c = Color.FromArgb(bgColor);
                    Bitmap newImg = null;
                    Graphics g;
                    newImg = new Bitmap(width, height);
                    g = Graphics.FromImage(newImg);
                    if (lastImage != null)
                    {
                        g.DrawImageUnscaled(lastImage, new Point(0, 0));
                    }
                    if (f.GraphicExtension.DisposalMethod == 1)
                    {
                        g.DrawRectangle(new Pen(new SolidBrush(c)), new Rectangle(xOffSet, yOffSet, iw, ih));
                    }
                    if (f.GraphicExtension.DisposalMethod == 2 && lastDisposal != 1)
                    {
                        g.Clear(c);
                    }
                    g.DrawImageUnscaled(f.Image, new Point(xOffSet, yOffSet));
                    g.Dispose();
                    f.Image.Dispose();
                    f.Image = newImg;
                }
                lastImage = f.Image;
                Quantizer(f.Image, f.Palette);
                lastDisposal = f.GraphicExtension.DisposalMethod;
                index++;
            }
        }

        GifFrame Merge(List<GifFrame> frames)
        {
            Bitmap bmp = null;
            Graphics g = null;
            foreach (GifFrame f in frames)
            {
                if (bmp == null)
                {
                    bmp = f.Image;
                    g = Graphics.FromImage(bmp);
                }
                else
                {
                    g.DrawImageUnscaled(f.Image, new Point(f.ImageDescriptor.XOffSet, f.ImageDescriptor.YOffSet));
                }
            }
            if (g != null)
            {
                g.Dispose();
            }
            GifFrame frame = new GifFrame();
            Color32[] pellatte = new OcTreeQuantizer(8).Quantizer(bmp);
            Quantizer(bmp, pellatte);
            frame.Image = bmp;
            frame.LocalColorTable = GetColorTable(pellatte);
            frame.ImageDescriptor = new ImageDescriptor();
            frame.ImageDescriptor.LctFlag = true;
            frame.ImageDescriptor.LctSize = pellatte.Length;
            frame.ImageDescriptor.Width = (short)bmp.Width;
            frame.ImageDescriptor.Height = (short)bmp.Height;
            frame.ColorDepth = 8;
            frame.GraphicExtension = new GraphicEx();
            frame.GraphicExtension.DisposalMethod = 0;
            frame.GraphicExtension.TransparencyFlag = true;
            frame.GraphicExtension.TranIndex = 255;
            return frame;
        }

        byte[] GetColorTable(Color32[] pellatte)
        {
            byte[] buffer = new byte[pellatte.Length * 3];
            int index = 0;
            for (int i = 0; i < pellatte.Length; i++)
            {
                buffer[index++] = pellatte[i].Red;
                buffer[index++] = pellatte[i].Green;
                buffer[index++] = pellatte[i].Blue;
            }
            return buffer;
        }
        #endregion

        #region 获取水印内容的位置
        /// <summary>
        /// 获取水印内容的位置
        /// </summary>
        /// <param name="ST">绘图位置</param>
        /// <param name="Width">图片宽度</param>
        /// <param name="Hight">图片高度</param>
        /// <param name="ImageWidth">水印宽度</param>
        /// <param name="ImageHight">水印高度</param>
        /// <param name="x">坐标X</param>
        /// <param name="y">坐标Y</param>
        private void getImageST(int ST, int Width, int Hight, int ImageWidth, int ImageHight, out int x, out int y)
        {
            switch (ST)
            {
                case 1://左上
                    x = (int)(Width * (float).01);
                    y = (int)(Hight * (float).01);
                    break;
                case 2://上中
                    x = (int)((Width * (float).50) - (ImageWidth / 2));
                    y = (int)(Hight * (float).01);
                    break;
                case 3://右上
                    x = (int)((Width * (float).99) - ImageWidth);
                    y = (int)(Hight * (float).01);
                    break;
                case 4://中左
                    x = (int)(Width * (float).01);
                    y = (int)(Hight * (float).50) - (ImageHight / 2);
                    break;
                case 5://中心
                    x = (int)((Width * (float).50) - (ImageWidth / 2));
                    y = (int)((Hight * (float).50) - (ImageHight / 2));
                    break;
                case 6://中右
                    x = (int)((Width * (float).99) - ImageWidth);
                    y = (int)((Hight * (float).50) - (ImageHight / 2));
                    break;
                case 7://左下
                    x = (int)(Width * (float).01);
                    y = (int)((Hight * .99) - ImageHight);
                    break;
                case 8://下中
                    x = (int)((Width * (float).50) - (ImageWidth / 2));
                    y = (int)((Hight * (float).99) - ImageHight);
                    break;
                default://右下
                    x = (int)((Width * (float).99) - ImageWidth);
                    y = (int)((Hight * (float).99) - ImageHight);
                    break;
            }
        }
        #endregion
    }
}
