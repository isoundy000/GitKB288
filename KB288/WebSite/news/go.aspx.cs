using System;
using System.Collections.Generic;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.IO;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Text.RegularExpressions;
using BCW.Common;

public partial class go : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected string xmlPath = "/Controls/news.xml";
    protected void Page_Load(object sender, EventArgs e)
    {

        string act = Utils.GetRequest("act", "all", 1, "", "");
        switch (act)
        {
            case "txt":
                TXTPage();
                break;
            default:
                ReloadPage();
                break;
        }
    }

    private void ReloadPage()
    {
        string Date = DateTime.Now.ToString("yyyyMMdd");

        string str = new BCW.Service.GetNews().GetNewsXML(Date);

        //builder.Append(str);


        string[] Temp = Regex.Split(str, @"\],\[");
        for (int i = 1; i < Temp.Length; i++)
        {
            string Title = "";
            string strUrl = "";
            DateTime AddTime = DateTime.Now;
            int nid = 0;

            string[] temp = Regex.Split(Temp[i], ",\"");
            nid = Convert.ToInt32(temp[0]);
            Title = temp[1].Replace("\"", "");
            strUrl = temp[2].Replace("\"", "");
            AddTime = Convert.ToDateTime("" + DateTime.Now.Year + "/" + temp[3].Replace("\"", "").Replace("]", ""));

            //1,"遭中俄反对 美防长称未确定在东亚何地部署萨德","http://news.sohu.com/20150411/n411104811.shtml","04/11 01:13"

            //builder.Append("" + nid + "<br />");
            //builder.Append("" + Title + "<br />");
            //builder.Append("" + strUrl + "<br />");
            //builder.Append("" + AddTime + "<br />");
   
            //国内0
            //国际1
            //社会2
            //国际财经3
            //军事4
            //体育5
            //娱乐6
             //文化7
             //汽车8

            int Types = Convert.ToInt32(ub.GetSub("NewsTypes", xmlPath));//导入文章栏目还是帖子
            //ID对应文章分类ID
            int NodeId = 0;
            //发布的ID（导入帖子时用到）
            int meid = 0;

            NodeId = Convert.ToInt32(ub.GetSub("NewsID" + nid + "", xmlPath));
            if (Types == 1)
            {
                meid = Convert.ToInt32(ub.GetSub("NewsIDb" + nid + "", xmlPath));
            }

            if (!strUrl.Contains("http://pic.") && strUrl.Contains(".shtml"))
            {
                //抓取N分钟内的News
                int min = 1440;
                if (AddTime > DateTime.Now.AddMinutes(-min))
                {
                    //if (Types == 0)
                    //{
                    //    if (!new BCW.BLL.Detail().Exists(Title))
                    //    {
                    //        BCW.Model.Detail model = new BCW.Model.Detail();
                    //        model.Title = Title;
                    //        model.KeyWord = Out.CreateKeyWord(Title, 2);
                    //        model.Model = "";
                    //        model.IsAd = true;
                    //        model.Types = 11;
                    //        model.NodeId = NodeId;
                    //        model.Content = strUrl;
                    //        model.TarText = "";
                    //        model.LanText = "";
                    //        model.SafeText = "";
                    //        model.LyText = "";
                    //        model.UpText = "";
                    //        model.IsVisa = 0;
                    //        model.AddTime = AddTime;
                    //        model.Readcount = 0;
                    //        model.Recount = 0;
                    //        model.Cent = 0;
                    //        model.BzType = 0;
                    //        model.Hidden = 0;
                    //        model.UsID = -1;//采集的标识

                    //        new BCW.BLL.Detail().Add(model);
                    //    }
                    //}
                    //else
                    //{
                     
                        string mename = new BCW.BLL.User().GetUsName(meid);

                        DataSet ds = new BCW.BLL.Text().GetList("ID", "ForumId=" + NodeId + " and UsID=" + meid + " and Title='" + Title + "'");
                        if (ds == null || ds.Tables[0].Rows.Count == 0)
                        {

                            int Price = 0;
                            int Prices = 0;
                            int IsSeen = 0;
                            string PayCi = string.Empty;

                            BCW.Model.Text addmodel = new BCW.Model.Text();
                            addmodel.ForumId = NodeId;
                            addmodel.Types = 0;
                            addmodel.Title = Title;
                            addmodel.Content = strUrl;
                            addmodel.HideContent = "";
                            addmodel.UsID = meid;
                            addmodel.UsName = mename;
                            addmodel.Price = Price;
                            addmodel.Prices = Prices;
                            addmodel.BzType = 0;
                            addmodel.HideType = 9;//采集的标识
                            addmodel.PayCi = PayCi;
                            addmodel.IsSeen = IsSeen;
                            addmodel.AddTime = DateTime.Now;
                            addmodel.ReTime = DateTime.Now;
                            new BCW.BLL.Text().Add(addmodel);

                        }
                    //}
                }
            }
        }
        Master.Title = "采集新闻列表";

        Master.Refresh = 60;
        Master.Gourl = Utils.getUrl("go.aspx");
        builder.Append("采集完成，间隔60秒再次自动启动...");
    }


    private void TXTPage()
    {
        Master.Title = "采集新闻txt入库";

        Master.Refresh = 5;
        Master.Gourl = Utils.getUrl("go.aspx?act=txt");
        int Types = Convert.ToInt32(ub.GetSub("NewsTypes", xmlPath));//导入文章栏目还是帖子

        //if (Types == 0)
        //{
        //    DataSet ds = new BCW.BLL.Detail().GetList("ID,Content", "Types=11 and UsID=-1 and Istxt=0");
        //    if (ds != null && ds.Tables[0].Rows.Count > 0)
        //    {

        //        int id = int.Parse(ds.Tables[0].Rows[0]["ID"].ToString());
        //        string Content = ds.Tables[0].Rows[0]["Content"].ToString();
        //        //builder.Append(Content + "|"+id+"<br />");
        //        try
        //        {
        //            if (Content.Contains("sohu.com"))
        //            {
        //                BCW.Collec.Collec Cn = new BCW.Collec.Collec();

        //                string NewsNextUrl = Content;
        //                while (NewsNextUrl.Length > 0)
        //                {
        //                    string str2 = new BCW.Service.GetNews().GetNewsXML2(NewsNextUrl);
        //                    string str = str2;

        //                    //取图片
        //                    string photoUrl = "";
        //                    string Pic = "/files/Cache/pic/";
        //                    string WebUrl = "http://photocdn.sohu.com";
        //                    ArrayList bodyArray = Cn.ReplaceSaveRemoteFile(3, str, Pic, WebUrl, "1");
        //                    if (bodyArray.Count == 2)
        //                    {
        //                        photoUrl = bodyArray[1].ToString();

        //                    }
        //                    //更新图片地址到数据库
        //                    new BCW.BLL.Detail().UpdatePics(id, photoUrl);

        //                    string NextPageRegex = @"(?:<a class=""pages-wd"" href=""[\s\S]+?"">上一页</a>[\s\S]+?)?<a class=""pages-wd"" href=""([\s\S]+?)"">下一页</a>";
        //                    NewsNextUrl = Cn.GetRegValue(NextPageRegex, str2);
        //                }
        //            }
        //            builder.Append("ID" + id + "采集结束，正在采集下一条...");
        //        }
        //        catch
        //        {

        //            builder.Append("出现错误，请告知技术员");
        //        }
        //    }
        //    else
        //    {
        //        builder.Append("采集完成，请等待对方网站更新...");
        //    }
        //}
        //else
        //{
            DataSet ds = new BCW.BLL.Text().GetList("ID,Content,UsID,ForumID", "HideType=9 and Istxt=0");
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {

                int id = int.Parse(ds.Tables[0].Rows[0]["ID"].ToString());
                int UsID = int.Parse(ds.Tables[0].Rows[0]["UsID"].ToString());
                int ForumID = int.Parse(ds.Tables[0].Rows[0]["ForumID"].ToString());

                string Content = ds.Tables[0].Rows[0]["Content"].ToString();
                //builder.Append(Content + "|"+id+"<br />");
                try
                {
                    if (Content.Contains("sohu.com"))
                    {
                        BCW.Collec.Collec Cn = new BCW.Collec.Collec();

                        string NewsNextUrl = Content;
                        while (NewsNextUrl.Length > 0)
                        {
                            string str2 = new BCW.Service.GetNews().GetNewsXML2(NewsNextUrl);
                            string str = str2;

                            //取图片
                            string photoUrl = "";
                            string Pic = "/files/Cache/pic/";
                            string WebUrl = "http://photocdn.sohu.com";
                            ArrayList bodyArray = Cn.ReplaceSaveRemoteFile(3, str, Pic, WebUrl, "1");
                            if (bodyArray.Count == 2)
                            {
                                photoUrl = bodyArray[1].ToString();
                                string[] fTemp = Regex.Split(photoUrl, "#");
                                int kk = 0;

                                int width = Convert.ToInt32(ub.GetSub("UpaWidth", "/Controls/upfile.xml"));
                                int height = Convert.ToInt32(ub.GetSub("UpaHeight", "/Controls/upfile.xml"));

                                for (int i = 0; i < fTemp.Length; i++)
                                {
                                    BCW.Model.Upfile modelf = new BCW.Model.Upfile();
                                    modelf.FileExt = System.IO.Path.GetExtension(fTemp[i]).ToLower();

                                    string rep = modelf.FileExt.Replace(".", "s.");
                                    string prevPath = System.Web.HttpContext.Current.Request.MapPath(fTemp[i].Replace(modelf.FileExt, rep));
                                    try
                                    {
                                        new BCW.Graph.ImageHelper().ResizeImage(System.Web.HttpContext.Current.Request.MapPath(fTemp[i]), prevPath, width, height, true);
                                        modelf.PrevFiles = fTemp[i].Replace(modelf.FileExt, rep);
                                    }
                                    catch
                                    {
                                        modelf.PrevFiles = fTemp[i];
                                    }
                                    modelf.Types = 1;
                                    modelf.NodeId = 0;
                                    modelf.UsID = UsID;
                                    modelf.ForumID = ForumID;
                                    modelf.BID = id;
                                    modelf.ReID = 0;
                                    modelf.Files = fTemp[i];
                                    modelf.Content = "";
                                    modelf.FileSize = BCW.Files.FileTool.GetFileLength(fTemp[i]);
                                    modelf.DownNum = 0;
                                    modelf.Cent = 0;
                                    modelf.IsVerify = 0;
                                    modelf.AddTime = DateTime.Now;
                                    new BCW.BLL.Upfile().Add(modelf);
                                    kk++;
                                }
                                //更新帖子文件数
                                new BCW.BLL.Text().UpdateFileNum(id, kk);

                            }
                            //更新已更新标识
                            new BCW.BLL.Text().UpdateIstxt(id, 1);

                            string NextPageRegex = @"(?:<a class=""pages-wd"" href=""[\s\S]+?"">上一页</a>[\s\S]+?)?<a class=""pages-wd"" href=""([\s\S]+?)"">下一页</a>";
                            NewsNextUrl = Cn.GetRegValue(NextPageRegex, str2);
                        }
                    }
                    builder.Append("ID" + id + "采集结束，正在采集下一条...");
                }
                catch
                {

                    builder.Append("出现错误，请告知技术员");
                }
            }
            else
            {
                builder.Append("采集完成，请等待对方网站更新...");
            }
        
        //}
    }

}