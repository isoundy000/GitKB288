using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace BCW.Snap
{
    public class GetImage
    {
        int S_Height;
        int S_Width;
        int F_Height;
        int F_Width;
        string MyURL;

        public int ScreenHeight
        {
            get
            {
                return S_Height;
            }
            set
            {
                S_Height = value;
            }
        }

        public int ScreenWidth
        {
            get
            {
                return S_Width;
            }
            set
            {
                S_Width = value;
            }
        }

        public int ImageHeight
        {
            get
            {
                return F_Height;
            }
            set
            {
                F_Height = value;
            }
        }

        public int ImageWidth
        {
            get
            {
                return F_Width;
            }
            set
            {
                F_Width = value;
            }
        }

        public string WebSite
        {
            get
            {
                return MyURL;
            }
            set
            {
                MyURL = value;
            }
        }

        public GetImage(string WebSite, int ScreenWidth, int ScreenHeight, int ImageWidth, int ImageHeight)
        {
            this.WebSite = WebSite;
            this.ScreenHeight = ScreenHeight;
            this.ScreenWidth = ScreenWidth;
            this.ImageHeight = ImageHeight;
            this.ImageWidth = ImageWidth;
        }

        public  Bitmap GetBitmap()
        {
            WebPageBitmap Shot = new WebPageBitmap(this.WebSite, this.ScreenWidth, this.ScreenHeight);

            Shot.GetIt();
            Bitmap Pic = Shot.DrawBitmap(this.ImageHeight, this.ImageWidth);
            return Pic;
        }
    }

    public class WebPageBitmap
    {
        WebBrowser MyBrowser;
        string URL;
        int Height;
        int Width;

        public  WebPageBitmap(string url, int width, int height)
        {
            this.URL = url;
            this.Width = width;
            this.Height = height;
            MyBrowser = new WebBrowser();
            MyBrowser.ScrollBarsEnabled = false;
            MyBrowser.Size = new Size(this.Width, this.Height);
        }

        public void GetIt()
        {
            MyBrowser.Navigate(this.URL);
            while (MyBrowser.ReadyState != WebBrowserReadyState.Complete)
            {
                Application.DoEvents();
            }
        }

        public Bitmap DrawBitmap(int theight, int twidth)
        {
            Bitmap myBitmap = new Bitmap(this.Width, this.Height);
            Rectangle DrawRect = new Rectangle(0, 0, this.Width, this.Height);
            MyBrowser.DrawToBitmap(myBitmap, DrawRect);
            System.Drawing.Image imgOutput = myBitmap;
            System.Drawing.Bitmap oThumbNail = new Bitmap(twidth, theight, imgOutput.PixelFormat);
            Graphics g = Graphics.FromImage(oThumbNail);

            g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighSpeed;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighSpeed;
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBilinear;

            Rectangle oRectangle = new Rectangle(0, 0, twidth, theight);
            g.DrawImage(imgOutput, oRectangle);

            try
            {
                return oThumbNail;
            }
            catch
            {
                return null;
            }
            finally
            {
                imgOutput.Dispose();
                imgOutput = null;
                MyBrowser.Dispose();
                MyBrowser = null;
            }

        }
    }
}
