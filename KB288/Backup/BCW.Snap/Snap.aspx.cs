using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text.RegularExpressions;
using BCW.Common;
using BCW.Graph;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace BCW.Snap
{
    public partial class Snap : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //输出颜色小框框
            if (Request["act"] == "color")
            {
                string colorname = Request.QueryString["colorname"];
                if (string.IsNullOrEmpty(colorname))
                    colorname = "FFFFFF";

                colorname = "#" + colorname;
                using (Bitmap image = new Bitmap(11, 11))
                {
                    using (Graphics g = Graphics.FromImage(image))
                    {
                        SolidBrush brush = new SolidBrush(ColorTranslator.FromHtml(colorname));
                        g.DrawRectangle(new Pen(Color.Black, 1), 0, 0, 11, 11);
                        g.FillRectangle(brush, 0, 0, 11, 11);
                        using (System.IO.MemoryStream stream = new System.IO.MemoryStream())
                        {
                            image.Save(stream, System.Drawing.Imaging.ImageFormat.Gif);

                            HttpContext.Current.Response.Cache.SetNoStore();
                            HttpContext.Current.Response.ClearContent();
                            HttpContext.Current.Response.ContentType = "image/Gif";
                            HttpContext.Current.Response.BinaryWrite(stream.ToArray());
                        }
                    }
                }
            }
            //充值验证码
            else if (Request["act"] == "paycode")
            {
                string imgid = Request.QueryString["imgid"];
                if (!string.IsNullOrEmpty(imgid))
                {
                    new ImageCode().CreateImage(DESEncrypt.Decrypt(imgid));
                }
                else
                {
                    new ImageCode().CreateImage(new ImageCode().GenerateCode());
                }
            }
            //手机网页浏览器
            else if (Request["act"] == "WapBrowser")
            {
                string url = string.Empty;
                url = Server.UrlDecode(Request.QueryString["url"]);
                url = url.Replace("&amp;", "&");
                HttpContext.Current.Response.ContentType = "text/html";
                string str = Utils.GetSourceTextByUrl(url);
                string str1 = str;
                if (str.IndexOf("<wml>") != -1)
                {
                    str1 = Regex.Replace(str, @"[\s\S]+?<wml>([\s\S]+?)", "$1", RegexOptions.IgnoreCase);
                    HttpContext.Current.Response.Charset = "utf-8";
                    HttpContext.Current.Response.Write("<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n");
                    HttpContext.Current.Response.Write("<!DOCTYPE html PUBLIC \"-//WAPFORUM//DTD XHTML Mobile 1.0//EN\" \"http://www.wapforum.org/DTD/xhtml-mobile10.dtd\">\r\n");
                    HttpContext.Current.Response.Write("<html xmlns=\"http://www.w3.org/1999/xhtml\" >\r\n");
                    HttpContext.Current.Response.Write("<head>\r\n");
                    HttpContext.Current.Response.Write("<meta http-equiv=\"Content-Type\" content=\"application/xhtml+xml;charset=UTF-8\"/>\r\n");
                    HttpContext.Current.Response.Write("<title>无标题页</title>\r\n");
                    HttpContext.Current.Response.Write("</head>\r\n");
                    HttpContext.Current.Response.Write("<body>");
                    str1 = str1.Replace("</wml>", "");
                }
                Response.Write(str1 + "</body></html>");
                Response.End();
            }
            else
            {
                string gourl = string.Empty;
                gourl = Server.UrlDecode(Request.QueryString["gourl"]);
                gourl = gourl.Replace("&amp;", "&");
                string url = string.Empty;
                if (gourl.IndexOf("WapBrowser") != -1)
                {
                    url = Regex.Replace(gourl, @"[\s\S]+?url=(http://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?)", "$1", RegexOptions.IgnoreCase);
                    gourl = "http://" + Request.Url.Host + "/Snap.aspx?act=WapBrowser&url=" + Server.UrlEncode(url) + "";
                }

                string x = Request.QueryString["x"];
                string y = Request.QueryString["y"];
                string w = Request.QueryString["w"];
                string h = Request.QueryString["h"];

                if (string.IsNullOrEmpty(x))
                    x = "1024";
                if (string.IsNullOrEmpty(y))
                    y = "768";
                if (string.IsNullOrEmpty(w))
                    w = "320";
                if (string.IsNullOrEmpty(h))
                    h = "240";

                int xx = Convert.ToInt32(x);
                int yy = Convert.ToInt32(y);
                int ww = Convert.ToInt32(w);
                int hh = Convert.ToInt32(h);

                string word = Request.QueryString["word"];
                string color = Request.QueryString["color"];
                string Position = Request.QueryString["Position"];

                try
                {
                    //GetImage thumb = new GetImage(gourl, 1024, 768, 240, 320);
                    GetImage thumb = new GetImage(gourl, xx, yy, ww, hh);
                    System.Drawing.Bitmap bx = thumb.GetBitmap();
                    //如果是文字则先保存图片再输出水印图片
                    if (!string.IsNullOrEmpty(word))
                    {
                        string sFile = Server.MapPath("~/Files") + "/temp/cuttemp.jpg";
                        bx.Save(sFile);
                        new ImageHelper().WaterMark(sFile, "", word, color, "Arial", 12, Convert.ToInt32(Position));
                        new ImageHelper().ResponseImage(sFile);
                    }
                    else
                    {
                        bx.Save(Response.OutputStream, System.Drawing.Imaging.ImageFormat.Jpeg);
                        Response.ContentType = "image/jpeg";
                    }
                }
                catch
                {

                }
            }
        }


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

        ////1.使用线程执行实例
        //protected void Page_Load(object sender, EventArgs e)
        //{
        //    System.Threading.Thread NewTh = new System.Threading.Thread(CaptureImage);
        //    NewTh.SetApartmentState(System.Threading.ApartmentState.STA);//必须启动单元线程
        //    NewTh.Start();
        //}

        ///// <summary>
        ///// 捕获屏幕
        ///// </summary>
        //private void CaptureImage()
        //{
        //    try
        //    {
        //        //string GetUrl = "http://" + Request.Url.Host + ":" + Request.Url.Port.ToString();
        //        string GetUrl = "http://nowtx.cn";
        //        GetImage thumb = new GetImage(GetUrl, 260, 300, 142, 122);
        //        System.Drawing.Bitmap x = thumb.GetBitmap();
        //        x.Save(Server.MapPath("~/Files/link") + "/1.jpg");
        //        x.Save(Response.OutputStream, System.Drawing.Imaging.ImageFormat.Jpeg);
        //        Response.ContentType = "image/jpeg";
        //    }
        //    catch
        //    {
        //    }
        //}
    }
}