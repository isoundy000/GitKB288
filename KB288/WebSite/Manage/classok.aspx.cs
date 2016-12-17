using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Net;
using System.IO;
using System.Drawing.Imaging;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Text.RegularExpressions;
using BCW.Common;
using BCW.Files;
public partial class Manage_classok : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected string xmlPath = "/Controls/upfile.xml";
    protected void Page_Load(object sender, EventArgs e)
    {
        //防止缓存
        Response.Expires = -1;
        Response.Cache.SetNoStore();
        Response.ExpiresAbsolute = DateTime.Now.AddDays(-1);

        int leibie = int.Parse(Utils.GetRequest("leibie", "all", 1, @"^[0-9]\d*$", "0"));
        int id = int.Parse(Utils.GetRequest("id", "all", 1, @"^[0-9]\d*$", "0"));
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 2, @"^[0-9]\d*$", "类型选择错误"));
        int nid = int.Parse(Utils.GetRequest("nid", "all", 1, @"^[0-9]\d*$", "0"));
        string act = Utils.GetRequest("act", "all", 1, "", "");
        builder.Append(Out.Tab("", ""));
        switch (act)
        {
            case "text":
                UpdateOther(act, "文章", ptype, nid, id);
                break;
            case "pic":
            case "picget":
                UpdateOther(act, "图片", ptype, nid, id);
                break;
            case "soft":
                UpdateOther(act, "文件", ptype, nid, id);
                break;
            case "shop":
                UpdateShop(act, "商品", ptype, nid, id);
                break;
            case "pic2":
                UiPic(act, ptype, nid, id);
                break;
            case "pic2save":
                UpdatePic(act, ptype, nid, id);
                break;
            default:
                UpdateClass(ptype, nid, id);
                break;
        }
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        if (id != 0)
        {
            if (string.IsNullOrEmpty(act))
            {
                builder.Append("<a href=\"" + Utils.getUrl("class.aspx?act=view&amp;id=" + id + "") + "\">返回菜单</a><br />");
            }
        }
        else
        {
            if (string.IsNullOrEmpty(act))
            {
                builder.Append("<a href=\"" + Utils.getUrl("classact.aspx?leibie=" + leibie + "&amp;nid=" + nid + "") + "\">继续添加</a><br />");
            }
            else
            {
                builder.Append("<a href=\"" + Utils.getUrl("classact.aspx?act=" + act + "&amp;nid=" + nid + "&amp;ptype=" + ptype + "") + "\">继续添加</a><br />");
            }
        }
        if (nid != 0)
        {
            builder.Append("<a href=\"" + Utils.getUrl("class.aspx?act=view&amp;id=" + nid + "") + "\">返回上一级</a><br />");
        }
        builder.Append("<a href=\"" + Utils.getUrl("class.aspx?leibie=" + leibie + "") + "\">返回设计中心</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    /// <summary>
    /// 添加/编辑菜单
    /// </summary>
    private void UpdateClass(int ptype, int nid, int id)
    {
        int leibie = int.Parse(Utils.GetRequest("leibie", "all", 2, @"^[0-9]\d*$", "类型错误"));
        string Title = Utils.GetRequest("Title", "post", 2, @"^[\s\S]{1,50}$", "名称限1-50字");
        int IsBr = int.Parse(Utils.GetRequest("IsBr", "post", 2, @"^[0-9]\d*$", "换行选择错误"));
        int Paixu = int.Parse(Utils.GetRequest("Paixu", "post", 2, @"^[0-9]\d*$", "排序填写错误"));
        int Hidden = int.Parse(Utils.GetRequest("Hidden", "post", 2, @"^[0-2]$", "显示状态选择错误"));
        int IsPc = 0;
        int VipLeven = 0;
        int Cent = 0;
        int BzType = 0;
        int SellTypes = 0;
        string InPwd = "";
        string Content = "";
        if (ptype <= 5)
        {
            VipLeven = int.Parse(Utils.GetRequest("VipLeven", "post", 2, @"^[0-9]\d*$", "VIP限制选择错误"));
        }
        if (ptype == 1)
        {
            IsPc = int.Parse(Utils.GetRequest("IsPc", "post", 2, @"^[0-9]\d*$", "浏览器限制选择错误"));
            Cent = int.Parse(Utils.GetRequest("Cent", "post", 2, @"^[0-9]\d*$", "收费必须为数字，不收费请填写0"));
            BzType = int.Parse(Utils.GetRequest("BzType", "post", 2, @"^[0-1]$", "收费币种选择错误"));
            SellTypes = int.Parse(Utils.GetRequest("SellTypes", "post", 2, @"^[0-9]\d*$", "消费方式选择出错"));
            InPwd = Utils.GetRequest("InPwd", "post", 3, @"^[A-Za-z0-9]{3,20}$", "访问密码限3-20位字母或数字");
        }
        if (ptype > 1 && ptype < 10)
        {
            Content = Utils.GetRequest("Content", "post", 2, @"^[\s\S]{1,}$", "内容不能为空");
        }

        string aName = string.Empty;
        if (id == 0)
        {
            aName = "添加菜单";
        }
        else
        {
            aName = "编辑菜单";
        }
        Master.Title = aName;

        BCW.Model.Topics model = new BCW.Model.Topics();
        model.Title = Title;
        model.IsBr = IsBr;
        model.Paixu = Paixu;
        model.Leibie = leibie;
        model.Types = ptype;
        model.NodeId = nid;
        model.IsPc = IsPc;
        model.VipLeven = VipLeven;
        model.Cent = Cent;
        model.BzType = BzType;
        model.SellTypes = SellTypes;
        model.Content = Content;
        model.InPwd = InPwd;
        model.Hidden = Hidden;
        if (id == 0)
        {
            new BCW.BLL.Topics().Add(model);
        }
        else
        {
            model.ID = id;
            new BCW.BLL.Topics().Update(model);
        }
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("" + aName + "成功");
        builder.Append(Out.Tab("</div>", ""));
    }

    /// <summary>
    /// 添加/编辑商品
    /// </summary>
    private void UpdateShop(string act, string TypeName, int ptype, int nid, int id)
    {
        string aName = string.Empty;
        if (id == 0)
        {
            aName = "添加" + TypeName + "";
        }
        else
        {
            aName = "编辑" + TypeName + "";
        }
        Master.Title = aName;
        string ac = Utils.GetRequest("ac", "post", 1, "", "");
        string Title = Utils.GetRequest("Title", "post", 2, @"^[\s\S]{1,50}$", "商品名称限1-50字");
        string Config = Utils.GetRequest("Config", "post", 3, @"^[\s\S]{1,500}$", "商品配置最多500字");
        string Content = Utils.GetRequest("Content", "post", 2, @"^[\s\S]{1,}$", "商品内容不能为空");
        bool IsAd = bool.Parse(Utils.GetRequest("IsAd", "post", 2, @"^False|True$", "商品性质选择错误"));
        int PostType = int.Parse(Utils.GetRequest("PostType", "post", 2, @"^[0-2]$", "付款方式选择错误"));
        decimal CityMoney = Convert.ToDecimal(Utils.GetRequest("CityMoney", "post", 2, @"^(\d)*(\.(\d){0,2})?$", "市场价格填写错误"));
        decimal VipMoney = Convert.ToDecimal(Utils.GetRequest("VipMoney", "post", 2, @"^(\d)*(\.(\d){0,2})?$", "市场价格填写错误"));
        int StockCount = int.Parse(Utils.GetRequest("StockCount", "post", 2, @"^[0-9]\d*$", "出售总量填写错误"));
        int SellCount = int.Parse(Utils.GetRequest("SellCount", "post", 1, @"^[0-9]\d*$", "0"));
        int PayCount = int.Parse(Utils.GetRequest("PayCount", "post", 1, @"^[0-9]\d*$", "0"));
        string Mobile = Utils.GetRequest("Mobile", "post", 2, @"^[\s\S]{1,200}$", "联系方式限200字");
        int PayType = int.Parse(Utils.GetRequest("PayType", "post", 2, @"^[0-9]\d*$", "送货方式选择错误"));
        string PostMoney = Utils.GetRequest("PostMoney", "post", 3, @"^[^\|]{1,50}(?:\|[^\|]{1,50}){1,500}$", "邮递邮费填写错误，可以留空");
        string KeyWord = Utils.GetRequest("KeyWord", "post", 1, "", "");
        string sFiles = string.Empty;
        int newId = 0;
        //关键字的生成
        if (string.IsNullOrEmpty(KeyWord))
        {
            KeyWord = Out.CreateKeyWord(Title, 2);
        }
        else
        {
            if (KeyWord.Length > 500)
            {
                Utils.Error("关键字不能超500字", "");
            }
            KeyWord = Utils.GetRequest("KeyWord", "post", 2, @"^[^\#]{1,50}(?:\#[^\#]{1,50}){0,500}$", "关键字填写格式错误");
        }

        //----------计算邮递邮费合法性开始
        if (PostMoney != "")
        {
            int GetNum = Utils.GetStringNum(PostMoney, "|");
            if (GetNum % 2 == 0)
            {
                Utils.Error("邮递邮费填写错误，可以留空", "");
            }
        }


        //添加验证
        if (id == 0)
        {
            if (new BCW.BLL.Goods().Exists(Title))
            {
                Utils.Error("数据库记录已存在“" + Title + "”", "");
            }
        }

        //写进数据库
        BCW.Model.Goods model = new BCW.Model.Goods();
        model.Title = Title;
        model.KeyWord = KeyWord;
        model.IsAd = IsAd;
        model.Mobile = Mobile;
        model.CityMoney = CityMoney;
        model.VipMoney = VipMoney;
        model.SellCount = SellCount;
        model.StockCount = StockCount;
        model.Paycount = PayCount;
        model.PayType = PayType;
        model.PostType = PostType;
        model.PostMoney = PostMoney;
        model.UsId = 0;
        model.NodeId = nid;
        model.Content = Content;
        model.AddTime = DateTime.Now;
        model.Readcount = 0;
        model.Evcount = 0;
        model.Config = Config;
        if (id == 0)
        {
            newId = new BCW.BLL.Goods().Add(model);
        }
        else
        {
            newId = id;
            model.ID = id;
            new BCW.BLL.Goods().Update(model);
        }

        //商品附图上传开始
        if (Utils.ToSChinese(ac) == "上传")
        {
            if (SaveFiles(ptype, newId, out sFiles))
            {
                aName += "/上传商品附图";
            }
        }
        //商品附图上传结束
        //得到截图文件
        string sPics = "";
        string Pics = "";
        if (sFiles != "")
            sPics = Utils.Mid(sFiles, 1, sFiles.Length);
        if (sPics == "#")
            sPics = "";

        if (sPics != "")
        {
            Pics = new BCW.BLL.Goods().GetFiles(newId);
            if (Pics != "")
                sPics = Pics + "#" + sPics;

            new BCW.BLL.Goods().UpdateFiles(newId, sPics);
        } 

        builder.Append(Out.Tab("<div class=\"title\">", "<br />"));
        builder.Append("" + aName + "成功");
        builder.Append(Out.Tab("</div>", ""));
    }

    /// <summary>
    /// 添加/编辑文章/图片/文件
    /// </summary>
    private void UpdateOther(string act, string TypeName, int ptype, int nid, int id)
    {
        string ac = Utils.GetRequest("ac", "post", 1, "", "");
        string Title = Utils.GetRequest("Title", "post", 2, @"^[\s\S]{1,50}$", "" + TypeName + "标题限1-50字");
        string KeyWord = Utils.GetRequest("KeyWord", "post", 1, "", "");
        string Content = string.Empty;
        if (ptype != 12)
        {
            Content = Utils.GetRequest("Content", "post", 2, @"^[\s\S]{1,}$", "" + TypeName + "内容不能为空");
        }
        string TarText = string.Empty;
        string LanText = string.Empty;
        string SafeText = string.Empty;
        string LyText = string.Empty;
        string UpText = string.Empty;
        int IsVisa = 0;
        if (ptype == 13)
        {
            TarText = Utils.GetRequest("TarText", "post", 3, @"^[\s\S]{1,50}$", "资费说明限50字内，可留空");
            LanText = Utils.GetRequest("LanText", "post", 3, @"^[\s\S]{1,50}$", "语言说明限50字内，可留空");
            SafeText = Utils.GetRequest("SafeText", "post", 3, @"^[\s\S]{1,50}$", "检查说明限50字内，可留空");
            LyText = Utils.GetRequest("LyText", "post", 3, @"^[\s\S]{1,50}$", "来源说明限50字内，可留空");
            UpText = Utils.GetRequest("UpText", "post", 3, @"^[\s\S]{1,50}$", "更新说明限50字内，可留空");
            IsVisa = int.Parse(Utils.GetRequest("IsVisa", "post", 1, @"^[0-3]$", "0"));
        }
        bool IsAd = bool.Parse(Utils.GetRequest("IsAd", "post", 2, @"^False|True$", "" + TypeName + "性质选择错误"));
        bool blpic = bool.Parse(Utils.GetRequest("blpic", "post", 1, @"^False|True$", "False"));//文件是否上传截图
        int BzType = int.Parse(Utils.GetRequest("BzType", "post", 1, @"^[0-1]$", "0"));
        int Cent = int.Parse(Utils.GetRequest("Cent", "post", 1, @"^[0-9]\d*$", "0"));
        string sFiles = string.Empty;
        string aName = string.Empty;
        string Model = Utils.GetRequest("Model", "post", 1, "", "");
        int newId = 0;

        //关键字的生成
        if (string.IsNullOrEmpty(KeyWord))
        {
            KeyWord = Out.CreateKeyWord(Title, 2);
        }
        else
        {
            if (KeyWord.Length > 500)
            {
                Utils.Error("关键字不能超500字", "");
            }
            KeyWord = Utils.GetRequest("KeyWord", "post", 2, @"^[^\#]{1,50}(?:\#[^\#]{1,50}){0,500}$", "关键字填写格式错误");
        }
        if (blpic != true)
        {
            if (id == 0)
            {
                aName = "添加" + TypeName + "";
            }
            else
            {
                aName = "编辑" + TypeName + "";
            }
        }
        else
        {
            aName = "上传截图";
        }
        Master.Title = aName;

        //添加验证
        if (id == 0)
        {
            if (new BCW.BLL.Detail().Exists(Title))
            {
                Utils.Error("数据库记录已存在“" + Title + "”", "");
            }
        }

        BCW.Model.Detail model = new BCW.Model.Detail();
        model.Title = Title;
        model.KeyWord = KeyWord;
        model.Model = Model.ToUpper().Replace("，", ",");
        model.IsAd = IsAd;
        model.Types = ptype;
        model.NodeId = nid;
        model.Content = Content;
        model.TarText = TarText;
        model.LanText = LanText;
        model.SafeText = SafeText;
        model.LyText = LyText;
        model.UpText = UpText;
        model.IsVisa = IsVisa;
        model.AddTime = DateTime.Now;
        model.Readcount = 0;
        model.Recount = 0;
        model.Cent = Cent;
        model.BzType = BzType;
        model.Hidden = 0;
        model.UsID = 0;
        if (id == 0)
        {
            newId = new BCW.BLL.Detail().Add(model);
        }
        else
        {
            newId = id;
            model.ID = id;
            new BCW.BLL.Detail().Update(model);
        }

        //-----------------文章附件/图片附件的提交开始
        if (ptype == 11 || ptype == 12)
        {
            //文章上传文件开始
            if (Utils.ToSChinese(ac) == "上传")
            {
                if (SaveFiles(ptype, newId, out sFiles))
                {
                    aName += "/上传文章附件";
                }
            }
            //文章上传文件结束

            else if (Utils.ToSChinese(ac) == "上传图片")
            {
                if (SaveFiles(ptype, newId, out sFiles))
                {
                    aName += "/上传图片";
                }
            }
            //添加图片结束
        }
        //-----------------文章附件/图片附件的提交结束

        //-----------------文件附件提交开始
        else if (ptype == 13)
        {
            if (Utils.ToSChinese(ac) == "上传文件")
            {
                if (SaveFiles(ptype, newId, out sFiles))
                {
                    aName += "/上传文件";
                }
            }

        }
        //-----------------文件附件提交结束
        //得到截图文件
        string sPics = "";
        string Pics = "";
        if (sFiles != "")
            sPics = Utils.Mid(sFiles, 1, sFiles.Length);
        if (sPics == "#")
            sPics = "";

        if (sPics != "")
        {
            Pics = new BCW.BLL.Detail().GetPics(newId);
            if (Pics != "")
                sPics = Pics + "#" + sPics;

            new BCW.BLL.Detail().UpdatePics(newId, sPics);
        } 
       
        //截图上传开始

        if (blpic == true)
        {
            UiPic(act, ptype, nid, newId);
        }
        else
        {
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("" + aName + "成功");
            builder.Append(Out.Tab("</div>", ""));
        }
    }

    /// <summary>
    /// 更新截图进数据库
    /// </summary>
    /// <param name="act"></param>
    /// <param name="ptype"></param>
    /// <param name="nid"></param>
    /// <param name="id"></param>
    private void UpdatePic(string act, int ptype, int nid, int id)
    {
        string ac = Utils.GetRequest("ac", "post", 1, "", "");
        string sFiles = string.Empty;
        string aName = string.Empty;
        if (Utils.ToSChinese(ac) == "上传截图")
        {
            if (SaveFiles(15, id, out sFiles))
            {
                aName = "上传截图成功";
                string sPics = "";
                if (sFiles != "")
                    sPics = Utils.Mid(sFiles, 1, sFiles.Length);
                if (sPics == "#")
                    sPics = "";

                if (sPics != "")
                {
                    string Pics = new BCW.BLL.Detail().GetPics(id);
                    if (Pics != "")
                        sPics = Pics + "#" + sPics;

                    new BCW.BLL.Detail().UpdatePics(id, sPics);
                }
            }
            else
            {
                aName = "上传截图失败";
            }
            builder.Append(Out.Tab("<div class=\"title\">", "<br />"));
            builder.Append("" + aName + "");
            builder.Append(Out.Tab("</div>", ""));
        }
    }


    /// <summary>
    /// 上传文件
    /// </summary>
    /// <param name="sFiles"></param>
    /// <returns></returns>
    private Boolean SaveFiles(int ptype, int newId, out string sFiles)
    {
        //图片系统缩略图设置
        int ThumbType = int.Parse(Utils.GetRequest("ThumbType", "post", 1, @"^[0-2]$", "0"));
        int width = int.Parse(Utils.GetRequest("width", "post", 1, @"^[1-9]\d*$", "75"));
        int height = int.Parse(Utils.GetRequest("height", "post", 1, @"^[1-9]\d*$", "100"));

        //地址采集上传图片
        if ((!Utils.Isie() && ptype == 12) || Request["act"] == "picget")
        {
            int k = 0;
            int num = int.Parse(Utils.GetRequest("num", "post", 1, @"^[1-9]\d*$", "0"));
            for (int pic = 1; pic <= num; pic++)
            {
                string FileName = Utils.GetRequest("file" + pic + "", "post", 1, @"^.+?.(gif|jpg|bmp|jpeg|png)$", "");
                if (FileName != "")
                {
                    string UpExt = ub.GetSub("UpbFileExt", xmlPath);
                    string fileExtension = System.IO.Path.GetExtension(FileName).ToLower();
                    if (UpExt.IndexOf(fileExtension) == -1)
                    {
                        continue;
                    }
                    string DirPath = string.Empty;
                    string prevDirPath = string.Empty;
                    string Path = "/Files/pic/act/";
                    string prevPath = "/Files/pic/prev/";
                    if (FileTool.CreateDirectory(Path, out DirPath))
                    {
                        string sPath = BCW.Files.FileTool.DownloadFile(Path, pic, FileName);
                        if (sPath != FileName)
                        {
                            //缩略图生成
                            string fileName = System.IO.Path.GetFileName(sPath);
                            string SavePath = System.Web.HttpContext.Current.Request.MapPath(sPath);
                            string prevSavePath = string.Empty;
                            if (ThumbType > 0)
                            {
                                bool pbool = false;
                                if (ThumbType == 1)
                                    pbool = true;
                                if (FileTool.CreateDirectory(prevPath, out prevDirPath))
                                {
                                    prevSavePath = SavePath.Replace("act", "prev");
                                    if (fileExtension == ".gif")
                                    {
                                        if (ThumbType > 0)
                                            new BCW.Graph.GifHelper().GetThumbnail(SavePath, prevSavePath, width, height, pbool);

                                    }
                                    else
                                    {
                                        if (ThumbType > 0)
                                            new BCW.Graph.ImageHelper().ResizeImage(SavePath, prevSavePath, width, height, pbool);
                                    }
                                }
                            }
                            BCW.Model.File model = new BCW.Model.File();
                            model.Types = 1;
                            model.NodeId = newId;
                            model.Files = sPath;
                            if (!string.IsNullOrEmpty(prevDirPath))
                                model.PrevFiles = sPath.Replace("act", "prev");
                            else
                                model.PrevFiles = "";

                            model.FileSize = BCW.Files.FileTool.GetFileLength(sPath);
                            model.FileExt = fileExtension;
                            model.Content = "";
                            model.DownNum = 0;
                            model.AddTime = DateTime.Now;
                            new BCW.BLL.File().Add(model);

                            //封面设置
                            InsertCover(ptype, Path, SavePath, fileName, fileExtension, pic, num, newId);
                            //添加水印
                            UpdateThumb(ptype, fileExtension, SavePath);
                            k++;
                        }
                    }

                }
            }

            if (k > 0)
            {
                sFiles = "";
                return true;
            }
            else
            {
                sFiles = "";
                return false;
            }
        }
        else
        {

            //遍历File表单元素
            System.Web.HttpFileCollection files = System.Web.HttpContext.Current.Request.Files;
            try
            {
                int k = 0;
                int j = 1;
                string GetFiles = string.Empty;
                for (int iFile = 0; iFile < files.Count; iFile++)
                {
                    //检查文件扩展名字
                    System.Web.HttpPostedFile postedFile = files[iFile];
                    string fileName, fileExtension;
                    fileName = System.IO.Path.GetFileName(postedFile.FileName);
                    string UpExt = ub.GetSub("UpbFileExt", xmlPath);
                    if (fileName != "")
                    {
                        fileExtension = System.IO.Path.GetExtension(fileName).ToLower();
                        if (UpExt.IndexOf(fileExtension) == -1)
                        {
                            continue;
                        }
                        //非法上传
                        if (fileExtension == ".asp" || fileExtension == ".aspx" || fileExtension == ".jsp" || fileExtension == ".php" || fileExtension == ".asa" || fileExtension == ".cer" || fileExtension == ".cdx" || fileExtension == ".htr" || fileExtension == ".exe")
                        {
                            continue;
                        }
                        string DirPath = string.Empty;
                        string prevDirPath = string.Empty;
                        string Path = string.Empty;
                        string prevPath = string.Empty;
                        if (ptype == 11)
                        {
                            Path = "/Files/text/";
                        }
                        else if (ptype == 12)
                        {
                            Path = "/Files/pic/act/";
                            prevPath = "/Files/pic/prev/";
                        }
                        else if (ptype == 13)
                        {
                            Path = "/Files/soft/";
                        }
                        else if (ptype == 14)
                        {
                            Path = "/Files/shop/";
                        }
                        else
                        {
                            Path = "/Files/soft/";
                        }
                        if (FileTool.CreateDirectory(Path, out DirPath))
                        {
                            //生成随机文件名
                            fileName = DT.getDateTimeNum() + iFile + fileExtension;
                            string SavePath = System.Web.HttpContext.Current.Request.MapPath(DirPath) + fileName;
                            postedFile.SaveAs(SavePath);
                            //图片系统缩略图
                            if (ptype == 12)
                            {
                                if (ThumbType > 0)
                                {
                                    bool pbool = false;
                                    if (ThumbType == 1)
                                        pbool = true;
                                    if (FileTool.CreateDirectory(prevPath, out prevDirPath))
                                    {
                                        string prevSavePath = System.Web.HttpContext.Current.Request.MapPath(prevDirPath) + fileName;
                                        if (fileExtension == ".gif")
                                        {
                                            if (ThumbType > 0)
                                                new BCW.Graph.GifHelper().GetThumbnail(SavePath, prevSavePath, width, height, pbool);

                                        }
                                        else
                                        {
                                            if (ThumbType > 0)
                                                new BCW.Graph.ImageHelper().ResizeImage(SavePath, prevSavePath, width, height, pbool);
                                        }
                                    }
                                }
                            }
                            if (ptype == 11 || ptype == 14)
                            {

                                GetFiles += "#" + DirPath + fileName + "";
                            }
                            else if (ptype == 12)
                            {
                                BCW.Model.File model = new BCW.Model.File();
                                model.Types = 1;
                                model.NodeId = newId;
                                model.Files = DirPath + fileName;
                                if (!string.IsNullOrEmpty(prevDirPath))
                                    model.PrevFiles = prevDirPath + fileName;
                                else
                                    model.PrevFiles = "";

                                model.FileSize = Convert.ToInt64(postedFile.ContentLength);
                                model.FileExt = fileExtension;
                                model.Content = "";
                                model.DownNum = 0;
                                model.AddTime = DateTime.Now;
                                new BCW.BLL.File().Add(model);

                            }
                            else if (ptype == 13)
                            {
                                //取显示的名称
                                string fname = string.Empty;
                                if (!string.IsNullOrEmpty(Request.Form["stext" + j + ""].ToString()))
                                    fname = Out.UBB(Request.Form["stext" + j + ""].Trim());
                                else
                                    fname = fileName;

                                BCW.Model.File model = new BCW.Model.File();
                                model.Types = 2;
                                model.NodeId = newId;
                                model.Files = DirPath + fileName;
                                model.PrevFiles = "";
                                model.FileSize = Convert.ToInt64(postedFile.ContentLength);
                                model.FileExt = fileExtension;
                                model.Content = fname;
                                model.DownNum = 0;
                                model.AddTime = DateTime.Now;
                                new BCW.BLL.File().Add(model);
                            }
                            else
                            {
                                GetFiles += "#" + DirPath + fileName + "";
                            }
                            //封面设置
                            InsertCover(ptype, Path, SavePath, fileName, fileExtension, iFile, files.Count, newId);
                            //添加水印
                            UpdateThumb(ptype, fileExtension, SavePath);

                            k++;
                        }
                        j++;
                    }
                }
                sFiles = GetFiles;
                if (k > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                sFiles = "";
                return false;
            }
        }
    }

    /// <summary>
    /// 文件上传截图
    /// </summary>
    /// <param name="act"></param>
    /// <param name="ptype"></param>
    /// <param name="nid"></param>
    /// <param name="id"></param>
    private void UiPic(string act, int ptype, int nid, int id)
    {
        if (act == "pic2")
        {
            Master.Title = "添加文件";
            builder.Append(Out.Div("title", "上传截图"));
        }
        //上传个数
        int nn = int.Parse(Utils.GetRequest("nn", "all", 1, @"^[0-9]\d*$", "1"));
        if (nn == 0)
            nn = 1;
        if (nn > 10)
            nn = 10;
        string strText = string.Empty;
        string strName = string.Empty;
        string strType = string.Empty;
        string strValu = string.Empty;
        string strEmpt = string.Empty;
        string strIdea = string.Empty;
        string strOthe = string.Empty;
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("截图:");
        for (int i = 1; i < 10; i++)
        {
            builder.Append("<a href=\"" + Utils.getUrl("classok.aspx?act=pic2&amp;nid=" + nid + "&amp;id=" + id + "&amp;ptype=" + ptype + "&amp;nn=" + i + "") + "\"><b>" + i + "</b></a> ");
        }
        builder.Append("张");
        builder.Append(Out.Tab("</div>", ""));
        for (int i = 0; i < nn; i++)
        {
            string y = ",";
            if (nn == 1)
            {
                strText = strText + y + "选择截图:/";
            }
            else
            {
                strText = strText + y + "第" + (i + 1) + "张截图:/";
            }
            strName = strName + y + "pic" + (i + 1);
            strType = strType + y + "file";
            strValu = strValu + "'";
            strEmpt = strEmpt + y;
        }
        strText += ",,,,,";
        strName += ",ptype,nid,id,act,nn";
        strType += ",hidden,hidden,hidden,hidden,hidden";
        strValu += "'" + ptype + "'" + nid + "'" + id + "'pic2save'" + nn + "";
        strEmpt += ",,,,,";
        strIdea += "/";
        strOthe = "上传截图|reset,classok.aspx,post,2,red|blue|blue";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
    }

    /// <summary>
    /// 添加水印
    /// </summary>
    private void UpdateThumb(int ptype, string fileExtension, string SavePath)
    {
        if (fileExtension == ".gif" || fileExtension == ".jpg" || fileExtension == ".jpeg" || fileExtension == ".png" || fileExtension == ".bmp")
        {
            int IsThumb = 0;
            if (ptype == 11)
                IsThumb = Convert.ToInt32(ub.GetSub("UpIsTextThumb", xmlPath));
            else if (ptype == 12)
                IsThumb = Convert.ToInt32(ub.GetSub("UpIsPicThumb", xmlPath));
            else if (ptype == 13 || ptype == 15)
                IsThumb = Convert.ToInt32(ub.GetSub("UpIsFileThumb", xmlPath));
            else if (ptype == 14)
                IsThumb = Convert.ToInt32(ub.GetSub("UpIsShopThumb", xmlPath));

            if (IsThumb > 0)
            {
                try
                {
                    int IsThumbType = 0;
                    IsThumbType = Convert.ToInt32(ub.GetSub("UpbIsThumb", xmlPath));
                    if (fileExtension == ".gif")
                    {

                        if (IsThumbType == 1)
                            new BCW.Graph.GifHelper().SmartWaterMark(SavePath, "", ub.GetSub("UpbWord", xmlPath), ub.GetSub("UpbWordColor", xmlPath), "Arial", 12, Convert.ToInt32(ub.GetSub("UpbPosition", xmlPath)));//文字水印
                        else if (IsThumbType == 2)
                            new BCW.Graph.GifHelper().WaterMark(SavePath, "", Server.MapPath(ub.GetSub("UpbWord", xmlPath)), Convert.ToInt32(ub.GetSub("UpbPosition", xmlPath)), Convert.ToInt32(ub.GetSub("UpbTran", xmlPath)));//图片水印

                    }
                    else
                    {
                        if (IsThumbType == 1)
                            new BCW.Graph.ImageHelper().WaterMark(SavePath, "", ub.GetSub("UpbWord", xmlPath), ub.GetSub("UpbWordColor", xmlPath), "Arial", 12, Convert.ToInt32(ub.GetSub("UpbPosition", xmlPath)));//文字水印
                        else if (IsThumbType == 2)
                            new BCW.Graph.ImageHelper().WaterMark(SavePath, "", Server.MapPath(ub.GetSub("UpbWord", xmlPath)), Convert.ToInt32(ub.GetSub("UpbPosition", xmlPath)), Convert.ToInt32(ub.GetSub("UpbTran", xmlPath)));//图片水印

                    }
                }
                catch { }
            }
        }
    }

    /// <summary>
    /// 设置列表封面
    /// </summary>
    private void InsertCover(int ptype, string Path, string SavePath, string fileName, string fileExtension, int iFile, int iFileCount, int newId)
    {
        if (fileExtension == ".gif" || fileExtension == ".jpg" || fileExtension == ".jpeg" || fileExtension == ".png" || fileExtension == ".bmp")
        {
            if ((iFile + 1) == iFileCount)
            {
                string CoverPath = Path + "cover/";
                string CoverDirPath = "";
                if (FileTool.CreateDirectory(CoverPath, out CoverDirPath))
                {
                    string CoverSavePath = System.Web.HttpContext.Current.Request.MapPath(CoverDirPath) + fileName;
                    int width = Convert.ToInt32(ub.GetSub("UpCoverWidth", xmlPath));
                    int height = Convert.ToInt32(ub.GetSub("UpCoverHeight", xmlPath));
                    bool pbool = false;
                    try
                    {
                        if (fileExtension == ".gif")
                        {
                            new BCW.Graph.GifHelper().GetThumbnail(SavePath, CoverSavePath, width, height, pbool);

                        }
                        else
                        {
                            new BCW.Graph.ImageHelper().ResizeImage(SavePath, CoverSavePath, width, height, pbool);
                        }
                        //更新封面
                        string Cover = "";
                        if (ptype != 14)
                        {
                            Cover = new BCW.BLL.Detail().GetCover(newId);
                        }
                        else
                        {
                            Cover = new BCW.BLL.Goods().GetCover(newId);
                        }
                        if (Cover != "")
                        {
                            BCW.Files.FileTool.DeleteFile(Cover);
                        }
                        if (ptype != 14)
                            new BCW.BLL.Detail().UpdateCover(newId, CoverDirPath + fileName);
                        else
                            new BCW.BLL.Goods().UpdateCover(newId, CoverDirPath + fileName);

                    }
                    catch { }
                }
            }
        }
    }

}