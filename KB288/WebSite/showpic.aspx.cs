using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using BCW.Common;
using System.Drawing;
using BCW.Files;

public partial class showpic : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected void Page_Load(object sender, EventArgs e)
    {
        string act = Utils.GetRequest("act", "all", 1, "", "");
        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        if (Utils.ToSChinese(ac) == "高级")
        {
            act = "info";
        }
        switch (act)
        {
            case "info":
                InfoPage();
                break;
            case "infosave":
                InfoSavePage();
                break;
            default:
                ReloadPage();
                break;
        }
    }

    private void ReloadPage()
    {
        string pic = Utils.GetRequest("pic", "all", 2, @"^[\s\S]{1,50}$", "图片地址错误");
        string picPath = Server.MapPath(pic);
        int chatid = int.Parse(Utils.GetRequest("chatid", "all", 1, @"\d*", "0"));
        int speakid = int.Parse(Utils.GetRequest("speakid", "all", 1, @"\d*", "2147483647"));
        int hbgn = int.Parse(Utils.GetRequest("hb", "all", 1, @"^[1-9]\d*$", "0"));
        Bitmap spic = null;
        try
        {
            spic = new Bitmap(picPath);

        }
        catch
        {
            Utils.Error("图片地址错误", "");
        }
        finally
        {
            spic.Dispose();
        }


        string act = Utils.GetRequest("act", "post", 1, "", "");
        if (act == "ok")
        {
            int width = int.Parse(Utils.GetRequest("width", "post", 1, @"^[0-9]\d*$", "128"));
            int height = int.Parse(Utils.GetRequest("height", "post", 1, @"^[0-9]\d*$", "160"));
            bool pbool = bool.Parse(Utils.GetRequest("pbool", "post", 1, @"^True|False$", "True"));
            if (width < 15 || height < 15)
            {
                Utils.Error("宽度和高度不能小于15", "");
            }
            //生成缩放图片
            string SavePath = string.Empty;
            string PicExt = FileTool.GetFileExt(picPath).ToLower();
            if (PicExt == ".gif")
            {
                new BCW.Graph.GifHelper().GetThumbnail(picPath, width, height, pbool, out SavePath);
            }
            else
            {
                new BCW.Graph.ImageHelper().ResizeImage(picPath, width, height, pbool, out SavePath);
            }
            //输出二进制流
            new BCW.Graph.ImageHelper().ResponseImage(Server.MapPath(SavePath));
        }
        Master.Title = "查看图片";
        Master.IsFoot = false;
        builder.Append(Out.Tab("<div class=\"title\">查看图片</div>", ""));

        builder.Append(Out.Tab("<div class=\"text\" >", ""));
        builder.Append("<img  style=\"width:450px\" src=\"" + pic + "\"  alt=\"load\"/>");
        builder.Append("<br /><a href=\"" + pic.Replace("prev", "act") + "\">下载原图(" + FileTool.GetFileContentLength(pic.Replace("prev", "act")) + ")</a>");
        builder.Append("<br />图片地址:" + pic + "");
        builder.Append("<br />自定义尺寸下载:");
        builder.Append(Out.Tab("</div>", ""));

        string strText = "宽度:,高度:,,,,";
        string strName = "width,height,pbool,pic,backurl,act";
        string strType = "text,text,select,hidden,hidden,hidden";
        string strValu = "240'320'True'" + pic + "'" + Utils.getPage(0) + "'ok";
        string strEmpt = "false,false,True|保持比例|False|固定尺寸,false,false,false";
        string strIdea = "/";
        string strOthe = "缩放图片|高级,showpic.aspx,post,1,red|blue";

        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("=EXIF信息=");
        builder.Append("<br />" + Utils.DelLastChar(BCW.Graph.EXIFextractor.GetExif(pic), "<br />"));
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">首页</a>-");
        if (chatid != 0)
        {
            builder.Append("<a href=\"" + Utils.getUrl("/bbs/chatroom.aspx?id=" + chatid + "&amp;hb="+hbgn) + "\">返回红包群</a>");
        }
        else if (speakid != 2147483647)
        {
            builder.Append("<a href=\"" + Utils.getUrl("/bbs/game/speak.aspx?ptype=" + speakid) + "\">返回闲聊</a>");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getPage("default.aspx") + "\">返回上级</a>");
        }
        builder.Append(Out.Tab("</div>", ""));
    }

    private void InfoPage()
    {
        Master.Title = "自定义图片";
        string pic = Utils.GetRequest("pic", "post", 1, "", "");
        int chatid = int.Parse(Utils.GetRequest("chatid", "all", 1, @"\d*", "0"));
        int speakid = int.Parse(Utils.GetRequest("speakid", "all", 1, @"\d*", "2147483647"));
        int hbgn = int.Parse(Utils.GetRequest("hb", "all", 1, @"^[1-9]\d*$", "0"));
        if (pic != "")
        {
            string picPath = Server.MapPath(pic);

            Bitmap spic = null;
            try
            {
                spic = new Bitmap(picPath);

            }
            catch
            {
                Utils.Error("图片地址错误", "");
            }
            finally
            {
                spic.Dispose();
            }

        }
        else
        {
            pic = "http://";

        }
        pic = pic.Replace("prev", "act");
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("自定义图片");
        builder.Append(Out.Tab("</div>", ""));

        string strText = "图片地址:/,缩放方式:/,图片宽度:/,图片高度:/,水印文字或者图片:/,文字颜色:/,水印图片透明度:/,水印位置:/,,";
        string strName = "Picurl,Types,width,height,word,color,Tran,Position,act,backurl";
        string strType = "text,select,num,num,text,select,num,select,hidden,hidden";
        string strValu = "" + pic + "'1'240'320''#DE0000'3'0'infosave'" + Utils.getPage(0) + "";
        string strEmpt = "false,0|不缩放|1|保持比例|2|固定尺寸,true,true,true,#DE0000|红色|#000000|黑色|#FFFFFF|白色|#0E71D4|蓝色|#0CB90D|绿色|#FD8300|黄色|#EF74DC|粉色,false,0|右下角|1|左上角|2|上中角|3|右上角|4|中左角|5|中心角|6|中右角|7|左下角|8|下中角,false,false";
        string strIdea = "/";
        string strOthe = "生成图片|reset,showpic.aspx,post,1,red|blue";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("温馨提示:<br />1.动态图片缩放、加水印后一样会动哦.<br />2.定义添加文字水印时,文字颜色有效,透明度无效.<br />3.定义添加图片水印时,文字颜色无效,透明度有效(建议1-10).<br />4.水印项留空则不添加水印.<br />5.动态图片添加文字水印时,选择文字颜色无效,系统根据图片色板自动调色.");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">首页</a>-");
        if (chatid != 0)
        {
            builder.Append("<a href=\"" + Utils.getUrl("/bbs/chatroom.aspx?id=" + chatid + "&amp;hb=" + hbgn) + "\">返回红包群</a>");
        }
        else if (speakid != 2147483647)
        {
            builder.Append("<a href=\"" + Utils.getUrl("/bbs/game/speak.aspx?ptype=" + speakid) + "\">返回闲聊</a>");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getPage("default.aspx") + "\">返回上级</a>");
        }
        builder.Append(Out.Tab("</div>", ""));

    }

    private void InfoSavePage()
    {
        Master.Title = "自定义图片";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("自定义图片");
        builder.Append(Out.Tab("</div>", ""));
        string Picurl = Utils.GetRequest("Picurl", "post", 2, @"^.+?.(gif|jpg|bmp|jpeg|png)$", "请正确输入图片地址");
        int Types = int.Parse(Utils.GetRequest("Types", "post", 2, @"^[0-2]\d*$", "缩放方式选择错误"));
        int width = int.Parse(Utils.GetRequest("width", "post", 2, @"^[0-9]\d*$", "保存图片宽度填写错误"));
        int height = int.Parse(Utils.GetRequest("height", "post", 2, @"^[0-9]\d*$", "保存图片高度填写错误"));
        string word = Utils.GetRequest("word", "post", 3, @"^[^\^]{1,200}$", "水印文字或者图片限200字内");
        string color = Utils.GetRequest("color", "post", 1, "", "");
        int Tran = int.Parse(Utils.GetRequest("Tran", "post", 1, @"^[0-9]\d*$", "0"));
        int Position = int.Parse(Utils.GetRequest("Position", "post", 2, @"^[0-8]$", "水印位置选择错误"));
        if (!Picurl.Contains("http://"))
            Picurl = "http://" + Utils.GetDomain() + "" + Picurl;

        //得到扩展名
        string PicExt = BCW.Files.FileTool.GetFileExt(Picurl).ToLower();
        string pPath = "/Files/temp/diy" + PicExt + "";
        //保存图片到本地
        string picPath = BCW.Files.FileTool.DownloadFile("/Files/temp/diy" + PicExt + "", Picurl);
        if (picPath == "")
            Utils.Error("请正确输入图片地址", "");

        picPath = Server.MapPath(picPath);

        //得到水印图片
        string spicPath = string.Empty;
        bool Ispic = false;
        //得到扩展名
        string wordExt = string.Empty;
        if (word.IndexOf(".") != -1)
        {
            wordExt = BCW.Files.FileTool.GetFileExt(word).ToLower();
            if (wordExt == ".gif" || wordExt == ".jpg" || wordExt == ".jpeg" || wordExt == ".png" || wordExt == ".bmp")
            {
                spicPath = BCW.Files.FileTool.SaveRemoteImg("/Files/temp/diy2" + wordExt + "", word);
                if (spicPath != "")
                {
                    Ispic = true;//图片水印类型
                    spicPath = Server.MapPath(spicPath);
                }
            }
        }

        string SavePath = picPath;
        bool pbool = false;
        if (Types != 0)
        {
            if (width < 15 || height < 15)
            {
                Utils.Error("宽度和高度不能小于15", "");
            }
            if (Types == 1)
                pbool = true;
        }
        if (PicExt == ".gif")
        {
            if (Types != 0)
                new BCW.Graph.GifHelper().GetThumbnail(picPath, "", width, height, pbool);

            if (!Ispic)
                new BCW.Graph.GifHelper().SmartWaterMark(picPath, "", word, color, "Arial", 12, Position);//文字水印
            else
                new BCW.Graph.GifHelper().WaterMark(picPath, "", spicPath, Position, Tran);//图片水印

        }
        else
        {
            if (Types != 0)
                new BCW.Graph.ImageHelper().ResizeImage(picPath, "", width, height, pbool);

            if (!Ispic)
                new BCW.Graph.ImageHelper().WaterMark(picPath, "", word, color, "Arial", 12, Position);//文字水印
            else
                new BCW.Graph.ImageHelper().WaterMark(picPath, "", spicPath, Position, Tran);//图片水印
        }

        new BCW.Graph.ImageHelper().ResponseImage(picPath);
    }
}
