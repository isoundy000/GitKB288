using System;
using System.Web;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace BCW.Graph
{
    /// <summary>
    /// ͼ����
    /// </summary>
    public class ImageHelper
    {
        /// <summary>
        /// ���췽��
        /// </summary>
        public ImageHelper()
        {
        }
        #region ��ʮ��������ɫת��Ϊ��ɫ����
        /// <summary>
        /// ��ʮ��������ɫת��Ϊ��ɫ����
        /// </summary>
        /// <param name="strcolor">ʮ��������ɫ(��#FF0000)</param>
        /// <returns></returns>
        public static Color FormatColor(string strcolor)
        {
            return Color.FromArgb(255, Convert.ToInt32(strcolor.Substring(1, 2), 16),
                Convert.ToInt32(strcolor.Substring(3, 2), 16),
                Convert.ToInt32(strcolor.Substring(5, 2), 16));
        }
        #endregion

        #region �����������JPEG�ļ�
        /// <summary>
        /// �����������JPEG�ļ�
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

        #region ȡ��ͼƬ�ߴ�/�ֱ�����Ϣ

        /// <summary>
        /// ȡ��ͼƬ�ߴ�/�ֱ�����Ϣ
        /// </summary>
        /// <param name="p_imageFile">ͼ���ļ�</param>
        /// <param name="ptype">�������</param>
        /// <returns></returns>
        public string GetPicxywh(string p_imageFile, int ptype)
        {
            string Return = string.Empty;
            // ����һ��ͼƬ��ʵ��
            Image MyImage = Image.FromFile(p_imageFile);
            if (ptype == 1)
            {
                string x = MyImage.HorizontalResolution.ToString();
                string y = MyImage.VerticalResolution.ToString();
                Return = x + "*" + y + "����";
            }
            else
            {
                string w = MyImage.Width.ToString();
                string h = MyImage.Height.ToString();
                Return = w + "*" + h + "����";
            }
            MyImage.Dispose();
            return Return;
        }
        #endregion

        #region ����ͼ���ȸ߶�
        /// <summary>
        /// ����ͼ���ȸ߶�
        /// </summary>
        /// <param name="p_imageFile">ͼ���ļ�</param>
        /// <param name="p_width">ͼ����ڸÿ�Ƚ��ᱻ����Ϊ�ÿ�ȳߴ�</param>
        /// <param name="p_height">ͼ����ڸø߶Ƚ��ᱻ����Ϊ�ø߶ȳߴ�</param>
        /// <param name="p_bool">�Ƿ񱣳ֱ���</param>
        public void ResizeImage(string p_imageFile, int p_width, int p_height, bool p_bool, out string SavePath)
        {
            SavePath = "/Files/temp.jpg";
            ResizeImage(p_imageFile, HttpContext.Current.Server.MapPath(SavePath), p_width, p_height, p_bool);
        }

        /// <summary>
        /// ����ͼ���ȸ߶� (���ֱ���)
        /// </summary>
        /// <param name="p_imageFile">ͼ���ļ�</param>
        /// <param name="p_saveFile">ͼ�񱣴�·��</param>
        /// <param name="p_width">ͼ����ڸÿ�Ƚ��ᱻ����Ϊ�ÿ�ȳߴ�</param>
        /// <param name="p_height">ͼ����ڸø߶Ƚ��ᱻ����Ϊ�ø߶ȳߴ�</param>
        /// <param name="p_bool">�Ƿ񱣳ֱ���</param>
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
                // ����ͼ����С�ı���
                if (originalWidth > p_width)
                    scaleWidth = (double)originalWidth / (double)p_width;

                if (originalHeight > p_height)
                    scaleHeight = (double)originalHeight / (double)p_height;

                if (scaleWidth != 0 || scaleHeight != 0)
                {
                    // �Ƚ���С����, ȡ��С�������ֵ
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

        #region �������ˮӡ
        /// <summary>
        /// �������ˮӡ
        /// </summary>
        /// <param name="p_imageFile">ͼ���ļ�</param>
        /// <param name="p_saveFile">ͼ�񱣴�·��</param>
        /// <param name="p_text">ˮӡ����</param>
        /// <param name="p_color">ˮӡ����ɫ</param>
        /// <param name="p_font">ˮӡ��������</param>
        /// <param name="p_fontsize">ˮӡ���ִ�С</param>
        /// <param name="p_position">ˮӡλ��</param>
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
                //ˮӡλ��
                getImageST(p_position, p_imageFile.Width, p_imageFile.Height, (int)fontSize.Width, (int)fontSize.Height, out x, out y);

                //��ͼ
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


        #region ���ͼƬ�߿�
        /// <summary>
        /// ���ͼƬ�߿�
        /// </summary>
        /// <param name="p_ImageFile">ͼ���ļ�</param>
        /// <param name="p_saveFile">����·��</param>
        /// <param name="Style">�߿�����</param>
        public void BorderImage(string p_ImageFile, string p_saveFile, int Style)
        {
            Image img = Bitmap.FromFile(p_ImageFile);
            int bordwidth = Convert.ToInt32(img.Width * 0.1);
            int bordheight = Convert.ToInt32(img.Height * 0.1);

            int newheight = img.Height + bordheight;
            int newwidth = img.Width + bordwidth;

            Color bordcolor = Color.White;//��ɫ�߿�
            Bitmap bmp = new Bitmap(newwidth, newheight);
            Graphics g = Graphics.FromImage(bmp);

            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;

            if (Style == 0)   // �����߿�.
            {
                //Changed: �޸�rec����, ��ԭͼ����. �ʺϱ߿���
                System.Drawing.Rectangle rec = new Rectangle(bordwidth / 2, bordwidth / 2, newwidth - bordwidth / 2, newheight - bordwidth / 2);
                g.DrawImage(img, rec, 0, 0, img.Width, img.Height, GraphicsUnit.Pixel);
                g.DrawRectangle(new Pen(bordcolor, bordheight), 0, 0, newwidth, newheight);
            }
            else if (Style == 1)   //���±߿�.
            {
                System.Drawing.Rectangle rec = new Rectangle(0, bordwidth / 2, newwidth, newheight - bordwidth / 2);
                g.DrawImage(img, rec, 0, 0, img.Width, img.Height, GraphicsUnit.Pixel);
                g.DrawLine(new Pen(bordcolor, bordheight), 0, 0, newwidth, 0);
                g.DrawLine(new Pen(bordcolor, bordheight), 0, newheight, newwidth, newheight);
            }
            else if (Style == 2)   //���ұ߿�.
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

        #region ���ͼƬˮӡ

        /// <summary>
        /// ���ͼƬˮӡ
        /// </summary>
        /// <param name="p_imageFile">ͼ���ļ�</param>
        /// <param name="p_saveFile">ͼ�񱣴�·��</param>
        /// <param name="p_SimageFile">ˮӡͼƬ�ļ�</param>
        /// <param name="p_position">ˮӡλ��</param>
        /// <param name="p_Transparent">͸����</param>
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
                    //ˮӡλ��
                    getImageST(p_position, originalWidth, originalHeight, SnewImage.Width, SnewImage.Height, out x, out y);
                    //��ͼ
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

        #region ��ȡˮӡ���ݵ�λ��
        /// <summary>
        /// ��ȡˮӡ���ݵ�λ��
        /// </summary>
        /// <param name="ST">��ͼλ��</param>
        /// <param name="Width">ͼƬ���</param>
        /// <param name="Hight">ͼƬ�߶�</param>
        /// <param name="ImageWidth">ˮӡ���</param>
        /// <param name="ImageHight">ˮӡ�߶�</param>
        /// <param name="x">����X</param>
        /// <param name="y">����Y</param>
        private void getImageST(int ST, int Width, int Hight, int ImageWidth, int ImageHight, out int x, out int y)
        {
            switch (ST)
            {
                case 1://����
                    x = (int)(Width * (float).01);
                    y = (int)(Hight * (float).01);
                    break;
                case 2://����
                    x = (int)((Width * (float).50) - (ImageWidth / 2));
                    y = (int)(Hight * (float).01);
                    break;
                case 3://����
                    x = (int)((Width * (float).99) - ImageWidth);
                    y = (int)(Hight * (float).01);
                    break;
                case 4://����
                    x = (int)(Width * (float).01);
                    y = (int)(Hight * (float).50) - (ImageHight / 2);
                    break;
                case 5://����
                    x = (int)((Width * (float).50) - (ImageWidth / 2));
                    y = (int)((Hight * (float).50) - (ImageHight / 2));
                    break;
                case 6://����
                    x = (int)((Width * (float).99) - ImageWidth);
                    y = (int)((Hight * (float).50) - (ImageHight / 2));
                    break;
                case 7://����
                    x = (int)(Width * (float).01);
                    y = (int)((Hight * .99) - ImageHight);
                    break;
                case 8://����
                    x = (int)((Width * (float).50) - (ImageWidth / 2));
                    y = (int)((Hight * (float).99) - ImageHight);
                    break;
                default://����
                    x = (int)((Width * (float).99) - ImageWidth);
                    y = (int)((Hight * (float).99) - ImageHight);
                    break;
            }
        }
        #endregion

    }
}