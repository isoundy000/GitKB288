using System;
using System.Web;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace BCW.Graph
{
    /// <summary>
    /// 图像处理
    /// </summary>
    public class ImageHelper
    {
        /// <summary>
        /// 构造方法
        /// </summary>
        public ImageHelper()
        {
        }
        #region 将十六进制颜色转换为颜色对象
        /// <summary>
        /// 将十六进制颜色转换为颜色对象
        /// </summary>
        /// <param name="strcolor">十六进制颜色(如#FF0000)</param>
        /// <returns></returns>
        public static Color FormatColor(string strcolor)
        {
            return Color.FromArgb(255, Convert.ToInt32(strcolor.Substring(1, 2), 16),
                Convert.ToInt32(strcolor.Substring(3, 2), 16),
                Convert.ToInt32(strcolor.Substring(5, 2), 16));
        }
        #endregion

        #region 向浏览器输入JPEG文件
        /// <summary>
        /// 向浏览器输入JPEG文件
        /// </summary>
        /// <param name="p_imageFile"></param>
        public void ResponseImage(string p_imageFile)
        {
            System.IO.FileStream fs = new System.IO.FileStream(p_imageFile, System.IO.FileMode.Open, System.IO.FileAccess.Read);
            byte[] mydata = new byte[fs.Length];
            int Length = Convert.ToInt32(fs.Length);
            fs.Read(mydata, 0, Length);
            fs.Close();
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.ContentType = "image/jpeg";
            HttpContext.Current.Response.OutputStream.Write(mydata, 0, Length);
            HttpContext.Current.Response.End();
        }

        #endregion

        #region 取得图片尺寸/分辨率信息

        /// <summary>
        /// 取得图片尺寸/分辨率信息
        /// </summary>
        /// <param name="p_imageFile">图像文件</param>
        /// <param name="ptype">输出类型</param>
        /// <returns></returns>
        public string GetPicxywh(string p_imageFile, int ptype)
        {
            string Return = string.Empty;
            // 创建一个图片的实例
            Image MyImage = Image.FromFile(p_imageFile);
            if (ptype == 1)
            {
                string x = MyImage.HorizontalResolution.ToString();
                string y = MyImage.VerticalResolution.ToString();
                Return = x + "*" + y + "像素";
            }
            else
            {
                string w = MyImage.Width.ToString();
                string h = MyImage.Height.ToString();
                Return = w + "*" + h + "像素";
            }
            MyImage.Dispose();
            return Return;
        }
        #endregion

        #region 调整图像宽度高度
        /// <summary>
        /// 调整图像宽度高度
        /// </summary>
        /// <param name="p_imageFile">图像文件</param>
        /// <param name="p_width">图像大于该宽度将会被调整为该宽度尺寸</param>
        /// <param name="p_height">图像大于该高度将会被调整为该高度尺寸</param>
        /// <param name="p_bool">是否保持比例</param>
        public void ResizeImage(string p_imageFile, int p_width, int p_height, bool p_bool, out string SavePath)
        {
            SavePath = "/Files/temp.jpg";
            ResizeImage(p_imageFile, HttpContext.Current.Server.MapPath(SavePath), p_width, p_height, p_bool);
        }

        /// <summary>
        /// 调整图像宽度高度 (保持比例)
        /// </summary>
        /// <param name="p_imageFile">图像文件</param>
        /// <param name="p_saveFile">图像保存路径</param>
        /// <param name="p_width">图像大于该宽度将会被调整为该宽度尺寸</param>
        /// <param name="p_height">图像大于该高度将会被调整为该高度尺寸</param>
        /// <param name="p_bool">是否保持比例</param>
        public void ResizeImage(string p_imageFile, string p_saveFile, int p_width, int p_height, bool p_bool)
        {
            Image originalImage = Image.FromFile(p_imageFile);
            int originalWidth = originalImage.Width;
            int originalHeight = originalImage.Height;
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
            using (Image newImage = new Bitmap(newWidth, newHeight))
            {
                Graphics g = Graphics.FromImage(newImage);

                g.CompositingQuality = CompositingQuality.HighQuality;
                g.InterpolationMode = InterpolationMode.Default;
                g.SmoothingMode = SmoothingMode.HighQuality;

                g.Clear(Color.Transparent);
                g.DrawImage(originalImage, new Rectangle(0, 0, newWidth, newHeight));

                originalImage.Dispose();
                g.Dispose();

                if (string.IsNullOrEmpty(p_saveFile))
                    newImage.Save(p_imageFile, ImageFormat.Jpeg);
                else
                    newImage.Save(p_saveFile, ImageFormat.Jpeg);
            }
        }
        #endregion

        #region 添加文字水印
        /// <summary>
        /// 添加文字水印
        /// </summary>
        /// <param name="p_imageFile">图像文件</param>
        /// <param name="p_saveFile">图像保存路径</param>
        /// <param name="p_text">水印文字</param>
        /// <param name="p_color">水印文字色</param>
        /// <param name="p_font">水印文字字体</param>
        /// <param name="p_fontsize">水印文字大小</param>
        /// <param name="p_position">水印位置</param>
        public void WaterMark(string p_ImageFile, string p_saveFile, string p_text, string p_color, string p_font, float p_fontsize, int p_position)
        {
            Image p_imageFile = new Bitmap(p_ImageFile);
            using (Bitmap newImage = new Bitmap(p_imageFile.Width, p_imageFile.Height))
            {
                Graphics g = Graphics.FromImage(newImage);

                g.DrawImage(p_imageFile, 0, 0, p_imageFile.Width, p_imageFile.Height);
                g.CompositingQuality = CompositingQuality.HighQuality;
                g.InterpolationMode = InterpolationMode.Default;
                g.SmoothingMode = SmoothingMode.HighQuality;

                Font font = new Font(p_font, p_fontsize, true ? FontStyle.Bold : FontStyle.Regular, GraphicsUnit.Pixel);
                SizeF fontSize = g.MeasureString(p_text, font);

                SolidBrush brush = new SolidBrush(ColorTranslator.FromHtml(p_color));
                int x = 0;
                int y = 0;
                //水印位置
                getImageST(p_position, p_imageFile.Width, p_imageFile.Height, (int)fontSize.Width, (int)fontSize.Height, out x, out y);

                //绘图
                g.DrawString(p_text, font, brush, x, y);
                p_imageFile.Dispose();
                g.Dispose();
                if (string.IsNullOrEmpty(p_saveFile))
                    newImage.Save(p_ImageFile, ImageFormat.Jpeg);
                else
                    newImage.Save(p_saveFile, ImageFormat.Jpeg);

                newImage.Dispose();
            }
        }
        #endregion


        #region 添加图片边框
        /// <summary>
        /// 添加图片边框
        /// </summary>
        /// <param name="p_ImageFile">图像文件</param>
        /// <param name="p_saveFile">保存路径</param>
        /// <param name="Style">边框类型</param>
        public void BorderImage(string p_ImageFile, string p_saveFile, int Style)
        {
            Image img = Bitmap.FromFile(p_ImageFile);
            int bordwidth = Convert.ToInt32(img.Width * 0.1);
            int bordheight = Convert.ToInt32(img.Height * 0.1);

            int newheight = img.Height + bordheight;
            int newwidth = img.Width + bordwidth;

            Color bordcolor = Color.White;//白色边框
            Bitmap bmp = new Bitmap(newwidth, newheight);
            Graphics g = Graphics.FromImage(bmp);

            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;

            if (Style == 0)   // 整个边框.
            {
                //Changed: 修改rec区域, 将原图缩放. 适合边框内
                System.Drawing.Rectangle rec = new Rectangle(bordwidth / 2, bordwidth / 2, newwidth - bordwidth / 2, newheight - bordwidth / 2);
                g.DrawImage(img, rec, 0, 0, img.Width, img.Height, GraphicsUnit.Pixel);
                g.DrawRectangle(new Pen(bordcolor, bordheight), 0, 0, newwidth, newheight);
            }
            else if (Style == 1)   //上下边框.
            {
                System.Drawing.Rectangle rec = new Rectangle(0, bordwidth / 2, newwidth, newheight - bordwidth / 2);
                g.DrawImage(img, rec, 0, 0, img.Width, img.Height, GraphicsUnit.Pixel);
                g.DrawLine(new Pen(bordcolor, bordheight), 0, 0, newwidth, 0);
                g.DrawLine(new Pen(bordcolor, bordheight), 0, newheight, newwidth, newheight);
            }
            else if (Style == 2)   //左右边框.
            {
                System.Drawing.Rectangle rec = new Rectangle(bordwidth / 2, 0, newwidth - bordwidth / 2, newheight);
                g.DrawImage(img, rec, 0, 0, img.Width, img.Height, GraphicsUnit.Pixel);
                g.DrawLine(new Pen(bordcolor, bordheight), 0, 0, 0, newheight);
                g.DrawLine(new Pen(bordcolor, bordheight), newwidth, 0, newwidth, newheight);
            }
            img.Dispose();
            g.Dispose();

            if (string.IsNullOrEmpty(p_saveFile))
                bmp.Save(p_ImageFile, ImageFormat.Jpeg);
            else
                bmp.Save(p_saveFile, ImageFormat.Jpeg);

        }
        #endregion

        #region 添加图片水印

        /// <summary>
        /// 添加图片水印
        /// </summary>
        /// <param name="p_imageFile">图像文件</param>
        /// <param name="p_saveFile">图像保存路径</param>
        /// <param name="p_SimageFile">水印图片文件</param>
        /// <param name="p_position">水印位置</param>
        /// <param name="p_Transparent">透明度</param>
        public void WaterMark(string p_ImageFile, string p_saveFile, string p_SimageFile, int p_position, int p_Transparent)
        {
            Image p_imageFile = new Bitmap(p_ImageFile);
            int originalWidth = p_imageFile.Width;
            int originalHeight = p_imageFile.Height;

            using (Image newImage = new Bitmap(p_imageFile))
            {
                Graphics g = Graphics.FromImage(newImage);
                Image SnewImage = new Bitmap(p_SimageFile);
                if (SnewImage.Height < originalHeight && SnewImage.Width < originalWidth)
                {
                    ImageAttributes Property = new ImageAttributes();
                    ColorMap Mapping = new ColorMap();
                    Mapping.OldColor = Color.FromArgb(255, 0, 0, 0);
                    Mapping.NewColor = Color.FromArgb(0, 0, 0, 0);
                    Property.SetBrushRemapTable(new ColorMap[] { Mapping });

                    float Transparent = 0.5F;
                    if (p_Transparent >= 1 && p_Transparent <= 10)
                        Transparent = p_Transparent / 10.0F;

                    float[][] MatrixNode ={
                          new float[]{1.0f,0.0f,0.0f,0.0f,0.0f},
                          new float[]{0.0f,1.0f,0.0f,0.0f,0.0f},
                          new float[]{0.0f,0.0f,1.0f,0.0f,0.0f},
                          new float[]{0.0f,0.0f,0.0f,Transparent,0.0f},
                          new float[]{0.0f,0.0f,0.0f,0.0f,1.0f}};
                    ColorMatrix Matrix = new ColorMatrix(MatrixNode);
                    Property.SetColorMatrix(Matrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

                    int x = 0;
                    int y = 0;
                    //水印位置
                    getImageST(p_position, originalWidth, originalHeight, SnewImage.Width, SnewImage.Height, out x, out y);
                    //绘图
                    g.DrawImage(SnewImage, new Rectangle(x, y, SnewImage.Width, SnewImage.Height), 0, 0, SnewImage.Width, SnewImage.Height, GraphicsUnit.Pixel, Property);
                    p_imageFile.Dispose();
                    SnewImage.Dispose();
                    g.Dispose();
                    if (string.IsNullOrEmpty(p_saveFile))
                        newImage.Save(p_ImageFile, ImageFormat.Jpeg);
                    else
                        newImage.Save(p_saveFile, ImageFormat.Jpeg);
                }
            }
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