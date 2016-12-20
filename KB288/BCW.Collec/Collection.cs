using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Text.RegularExpressions;
using System.Text;
using System.Net;
using System.Collections;
using System.Drawing;
using BCW.Common;
using BCW.Files;

namespace BCW.Collec
{
    /// <summary>
    ///Collection 的摘要说明
    /// </summary>
    public class Collec
    {

        public Collec()
        {
            //
            //TODO: 在此处添加构造函数逻辑
            //
        }

        #region 获取远程文件源代码
        /// <summary>
        /// 获取远程文件源代码
        /// </summary>
        /// <param name="url">远程url</param>
        /// <param name="EnCodeType">编码</param>
        /// <returns></returns>
        public string GetHttpPageCode(string Url, Encoding EnCodeType)
        {
            string strResult = string.Empty;
            if (string.IsNullOrEmpty(Url) || Url.Length < 10)
                return "$UrlIsFalse";
            try
            {
                WebClient MyWebClient = new WebClient();
                MyWebClient.Credentials = CredentialCache.DefaultCredentials;
                MyWebClient.Encoding = EnCodeType;
                strResult = MyWebClient.DownloadString(Url);
            }
            catch (Exception)
            {
                strResult = "$GetFalse";
            }
            return strResult;
        }
        #endregion

        #region 根据表达式来截取字段
        /// <summary>
        /// 根据表达式来截取字段
        /// </summary>
        /// <param name="pageStr">原字符串</param>
        /// <param name="strStart">截取字符开始</param>
        /// <param name="strEnd">截取字符结束</param>
        /// <param name="inStart">是否包含strStart,false是不包含</param>
        /// <param name="inEnd">是否包含strEnd,false是不包含</param>
        /// <returns></returns>
        public string GetBody(string pageStr, string strStart, string strEnd, bool inStart, bool inEnd)
        {
        pageStr = pageStr.Trim();
        if (string.IsNullOrEmpty(strStart))
            return "$StartFalse";
        int start = pageStr.IndexOf(strStart);
        if (strStart.Length == 0 || start < 0)
            return "$StartFalse";
            pageStr = pageStr.Substring(start + strStart.Length, pageStr.Length - start - strStart.Length);
            int end = pageStr.IndexOf(strEnd);
            if (strEnd.Length == 0 || end < 0)
                return "$EndFalse";
            string strResult = pageStr.Substring(0, end);
            if (inStart)
                strResult = strStart + strResult;
            if (inEnd)
                strResult += strEnd;
            return strResult.Trim();
        }
        #endregion

        #region 根据正则获取链接地址
        /// <summary>
        /// 根据正则获取链接地址
        /// </summary>
        /// <param name="pageStr">原字符串</param>
        /// <param name="strStart">链接开始</param>
        /// <param name="strEnd">连接结束</param>
        /// <returns></returns>
        public ArrayList GetLinkArray(string pageStr, string strStart, string strEnd)
        {
            ArrayList linkArray = new ArrayList();
            int start = pageStr.IndexOf(strStart);
            if (strStart.Length == 0 || start < 0)
            {
                linkArray.Add("$StartFalse");
                return linkArray;
            }
            int end = pageStr.IndexOf(strEnd);
            if (strEnd.Length == 0 || end < 0)
            {
                linkArray.Add("$EndFalse");
                return linkArray;
            }
            Regex myRegex = new Regex(@"(" + strStart + ").+?(" + strEnd + ")", RegexOptions.IgnoreCase);
            MatchCollection matches = myRegex.Matches(pageStr);
            foreach (Match match in matches)
                linkArray.Add(match.ToString());
            if (linkArray.Count == 0)
            {
                linkArray.Add("$NoneLink");
                return linkArray;
            }
            string TempStr = string.Empty;
            for (int i = 0; i < linkArray.Count; i++)
            {
                TempStr = linkArray[i].ToString();
                TempStr = TempStr.Replace(strStart, "");
                TempStr = TempStr.Replace(strEnd, "");
                TempStr = TempStr.Replace("&amp;", "&");
                linkArray[i] = (object)TempStr;
            }
            return linkArray;
        }
        #endregion

        #region 获取字符串图片并保存
        /// <summary>
        /// 获取字符串图片并保存
        /// </summary>
        /// <param name="pageStr">字符串</param>
        /// <param name="SavePath">保存路径</param>
        /// <param name="webUrl">指定网站url</param>
        /// <param name="isSave">是否保存图片</param>
        /// <returns></returns>
        public ArrayList ReplaceSaveRemoteFile(int Types, string pageStr, string SavePath, string webUrl, string isSave)
        {
            ArrayList replaceArray = new ArrayList();
            Regex imgReg = new Regex(@"<img.+?[^\>]>", RegexOptions.IgnoreCase);
            MatchCollection matches = imgReg.Matches(pageStr);
            string TempStr = string.Empty;
            string TitleImg = string.Empty;
            foreach (Match match in matches)
            {
                if (TempStr != string.Empty)
                    TempStr += "$Array$" + match.ToString();
                else
                    TempStr = match.ToString();
            }
            string[] TempArr = TempStr.Split(new string[] { "$Array$" }, StringSplitOptions.None);
            TempStr = string.Empty;
            imgReg = new Regex(@"src\s*=\s*.+?\.(gif|jpg|bmp|jpeg|psd|png|svg|dxf|wmf|tiff)", RegexOptions.IgnoreCase);
            for (int i = 0; i < TempArr.Length; i++)
            {
                matches = imgReg.Matches(TempArr[i]);
                foreach (Match match in matches)
                {
                    if (TempStr != string.Empty)
                        TempStr += "$Array$" + match.ToString();
                    else
                        TempStr = match.ToString();
                }
            }
            if (TempStr.Length > 0)
            {
                imgReg = new Regex(@"src\s*=\s*", RegexOptions.IgnoreCase);
                TempStr = imgReg.Replace(TempStr, "");
            }
            if (TempStr.Length == 0)
            {
                replaceArray.Add(pageStr);
                return replaceArray;
            }
            TempStr = TempStr.Replace("\"", "");
            TempStr = TempStr.Replace("'", "");
            TempStr = TempStr.Replace(" ", "");
            //去掉重复图片
            TempArr = TempStr.Split(new string[] { "$Array$" }, StringSplitOptions.None);
            TempStr = string.Empty;
            for (int i = 0; i < TempArr.Length; i++)
            {
                if (TempStr.IndexOf(TempArr[i]) == -1)
                    TempStr += "$Array$" + TempArr[i];
            }
            TempStr = TempStr.Substring(7);
            TempArr = TempStr.Split(new string[] { "$Array$" }, StringSplitOptions.None);
            TempStr = string.Empty;
            string ImageArr = string.Empty;
            for (int i = 0; i < TempArr.Length; i++)
            {
                imgReg = new Regex(TempArr[i]);
                string RemoteFileUrl = DefiniteUrl(TempArr[i], webUrl);
                if (isSave == "1")
                {

                    string DirPath = "";
                    if (BCW.Files.FileTool.CreateDirectory(SavePath, out DirPath))
                    {
                        string imgPath = System.Web.HttpContext.Current.Server.MapPath(DirPath);
                        string fileType = RemoteFileUrl.Substring(RemoteFileUrl.LastIndexOf('.'));
                        string filename = string.Empty;
                        filename = GetDateFile();
                        filename += fileType;
                        if (SavePhoto(Types, imgPath + "" + filename,filename, RemoteFileUrl, fileType, i, TempArr.Length))
                        {
                            RemoteFileUrl = DirPath + "" + filename;
                        }
                    }
                }
                pageStr = imgReg.Replace(pageStr, RemoteFileUrl);
                if (i == 0)
                {
                    TitleImg = RemoteFileUrl;
                    ImageArr = RemoteFileUrl;
                }
                else
                    ImageArr += "#" + RemoteFileUrl;
            }
            replaceArray.Add(pageStr);
            replaceArray.Add(ImageArr);
            return replaceArray;
        }
        #endregion

        #region 相对路径转换绝对路径
        /// <summary>
        /// 相对路径转换绝对路径
        /// </summary>
        /// <param name="PrimitiveUrl">要转换地址</param>
        /// <param name="ConsultUrl">指定网站地址</param>
        /// <returns></returns>
        public string DefiniteUrl(string PrimitiveUrl, string ConsultUrl)
        {

            if (ConsultUrl.Substring(0, 7) != "http://")
                ConsultUrl = "http://" + ConsultUrl;
            ConsultUrl = ConsultUrl.Replace(@"\", "/");
            ConsultUrl = ConsultUrl.Replace("://", @":\\");
            PrimitiveUrl = PrimitiveUrl.Replace(@"\", "/");

            if (ConsultUrl.Substring(ConsultUrl.Length - 1) != "/")//如果没有/加个/
            {
                if (ConsultUrl.IndexOf('/') > 0)
                {
                    if (ConsultUrl.Substring(ConsultUrl.LastIndexOf("/"), ConsultUrl.Length - ConsultUrl.LastIndexOf("/")).IndexOf('.') == -1)
                        ConsultUrl += "/";
                }
                else
                    ConsultUrl += "/";
            }
            string[] ConArray = ConsultUrl.Split('/');
            string returnStr = string.Empty;
            string[] PriArray;
            int pi = 0;

            //ADDTIME：2012-2-3 
            if (string.IsNullOrEmpty(PrimitiveUrl))
            {
                returnStr = "$False";
                return returnStr;
            }

            if (PrimitiveUrl.Substring(0, 7) == "http://")
                returnStr = PrimitiveUrl.Replace("://", @":\\");
            else if (PrimitiveUrl.Substring(0, 1) == "/")
                returnStr = ConArray[0] + PrimitiveUrl;
            else if (PrimitiveUrl.Substring(0, 2) == "./")
            {
                PrimitiveUrl = PrimitiveUrl.Substring(PrimitiveUrl.Length - 2, 2);
                if (ConsultUrl.Substring(ConsultUrl.Length - 1) == "/")
                    returnStr = ConsultUrl + PrimitiveUrl;
                else
                    returnStr = ConsultUrl.Substring(0, ConsultUrl.LastIndexOf('/')) + PrimitiveUrl;
            }
            else if (PrimitiveUrl.Substring(0, 3) == "../")
            {
                while (PrimitiveUrl.Substring(0, 3) == "../")
                {
                    PrimitiveUrl = PrimitiveUrl.Substring(3);
                    pi++;
                }
                for (int i = 0; i < ConArray.Length - 1 - pi; i++)
                {
                    if (returnStr.Length > 0)
                        returnStr = returnStr + ConArray[i];
                    else
                        returnStr = ConArray[i];
                }
                returnStr = returnStr + PrimitiveUrl;
            }
            else
            {
                if (PrimitiveUrl.IndexOf('/') > -1)
                {
                    PriArray = PrimitiveUrl.Split('/');
                    if (PriArray[0].IndexOf('.') > -1)
                    {
                        if (PrimitiveUrl.Substring(PrimitiveUrl.Length - 1) == "/")
                            returnStr = "http://" + PrimitiveUrl;
                        {
                            if (PriArray[PriArray.Length - 1].IndexOf('.') > -1)
                                returnStr = "http:\\" + PrimitiveUrl;
                            else
                                returnStr = "http:\\" + PrimitiveUrl + "/";
                        }
                    }
                    else
                    {
                        if (ConsultUrl.Substring(ConsultUrl.Length - 1) == "/")
                            returnStr = ConsultUrl + PrimitiveUrl;
                        else
                            returnStr = ConsultUrl.Substring(0, ConsultUrl.LastIndexOf('/')) + PrimitiveUrl;
                    }
                }
                else
                {
                    if (PrimitiveUrl.IndexOf('.') > -1)
                    {
                        string lastUrl = ConsultUrl.Substring(ConsultUrl.LastIndexOf('.'));
                        if (ConsultUrl.Substring(ConsultUrl.Length - 1) == "/")
                        {
                            if (lastUrl == "com" || lastUrl == "cn" || lastUrl == "net" || lastUrl == "org")
                                returnStr = "http:\\" + PrimitiveUrl + "/";
                            else
                                returnStr = ConsultUrl + PrimitiveUrl;
                        }
                        else
                        {
                            if (lastUrl == "com" || lastUrl == "cn" || lastUrl == "net" || lastUrl == "org")
                                returnStr = "http:\\" + PrimitiveUrl + "/";
                            else
                                returnStr = ConsultUrl.Substring(0, ConsultUrl.LastIndexOf('/')) + "/" + PrimitiveUrl;
                        }
                    }
                    else
                    {
                        if (ConsultUrl.Substring(ConsultUrl.Length - 1) == "/")
                            returnStr = ConsultUrl + PrimitiveUrl + "/";
                        else
                            returnStr = ConsultUrl.Substring(0, ConsultUrl.LastIndexOf('/')) + "/" + PrimitiveUrl + "/";
                    }
                }
            }

            if (returnStr.Substring(0, 1) == "/")
                returnStr = returnStr.Substring(1);
            if (returnStr.Length > 0)
            {
                returnStr = returnStr.Replace("//", "/");
                returnStr = returnStr.Replace(@":\\", "://");
            }
            else
                returnStr = "$False";
            return returnStr;
        }
        #endregion

        #region 保存图片
        /// <summary>
        /// 保存图片
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="RemoteFileUrl">url</param>
        /// <returns></returns>
        public bool SavePhoto(int Types, string SavePath, string fileName, string RemoteFileUrl, string fileExtension, int iFile, int iFileCount)
        {
            try
            {
                WebRequest request = WebRequest.Create(RemoteFileUrl);
                request.Timeout = 20000;
                Stream stream = request.GetResponse().GetResponseStream();
                Image getImage = Image.FromStream(stream);
                getImage.Save(SavePath);
                stream.Dispose();
                getImage.Dispose();
                if (Types != 3)
                {
                    string xmlPath = "/Controls/upfile.xml";

                    if (Types == 1)//文章图片
                    {

                    }
                    else if (Types == 2)
                    {
                        //图片系统缩略图设置
                        int ThumbType = int.Parse(ub.GetSub("UpbThumbType", xmlPath));
                        int width = int.Parse(ub.GetSub("UpbWidth", xmlPath));
                        int height = int.Parse(ub.GetSub("UpbHeight", xmlPath));
                        if (ThumbType > 0)
                        {
                            string prevDirPath = string.Empty;
                            string prevPath = "/Files/pic/prev/";
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

                    //添加水印
                    int IsThumb = 0;
                    if (Types == 1)
                        IsThumb = Convert.ToInt32(ub.GetSub("UpIsTextThumb", xmlPath));
                    else if (Types == 2)
                        IsThumb = Convert.ToInt32(ub.GetSub("UpIsPicThumb", xmlPath));

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
                                    new BCW.Graph.GifHelper().WaterMark(SavePath, "", HttpContext.Current.Server.MapPath(ub.GetSub("UpbWord", xmlPath)), Convert.ToInt32(ub.GetSub("UpbPosition", xmlPath)), Convert.ToInt32(ub.GetSub("UpbTran", xmlPath)));//图片水印

                            }
                            else
                            {
                                if (IsThumbType == 1)
                                    new BCW.Graph.ImageHelper().WaterMark(SavePath, "", ub.GetSub("UpbWord", xmlPath), ub.GetSub("UpbWordColor", xmlPath), "Arial", 12, Convert.ToInt32(ub.GetSub("UpbPosition", xmlPath)));//文字水印
                                else if (IsThumbType == 2)
                                    new BCW.Graph.ImageHelper().WaterMark(SavePath, "", HttpContext.Current.Server.MapPath(ub.GetSub("UpbWord", xmlPath)), Convert.ToInt32(ub.GetSub("UpbPosition", xmlPath)), Convert.ToInt32(ub.GetSub("UpbTran", xmlPath)));//图片水印

                            }
                        }
                        catch { }
                    }
                    //添加封面

                    if (fileExtension == ".gif" || fileExtension == ".jpg" || fileExtension == ".jpeg" || fileExtension == ".png" || fileExtension == ".bmp")
                    {
                        if ((iFile + 1) == iFileCount)
                        {
                            string CoverPath = "";
                            if (Types == 1)
                                CoverPath = @"\Files\text\cover\";
                            else
                                CoverPath = @"\Files\pic\act\cover\";

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

                                }
                                catch { }
                            }
                        }
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region 文件创建
        /// <summary>
        /// 根据时间得到目录名
        /// </summary>
        /// <returns></returns>
        public string GetDateDir()
        {
            return DateTime.Now.ToString("yyyyMMdd");
        }
        /// <summary>
        /// 根据时间得到文件名
        /// </summary>
        /// <returns></returns>
        public string GetDateFile()
        {
            return DateTime.Now.ToString("HHmmssff");
        }
        #endregion

        #region 过滤代码中的标签
        /// <summary>
        /// 过滤代码中的标签
        /// </summary>
        /// <param name="ConStr">代码</param>
        /// <param name="TagName">标签</param>
        /// <param name="FType">过滤类型</param>
        /// <returns></returns>
        public string ScriptHtml(string ConStr, string TagName, int FType)
        {
            Regex myReg;
            switch (FType)
            {
                case 1:
                    myReg = new Regex("<" + TagName + "([^>])*>", RegexOptions.IgnoreCase);
                    ConStr = myReg.Replace(ConStr, "");
                    break;
                case 2:
                    myReg = new Regex("<" + TagName + "([^>])*>.*?</" + TagName + "([^>])*>", RegexOptions.IgnoreCase);
                    ConStr = myReg.Replace(ConStr, "");
                    break;
                case 3:
                    myReg = new Regex("<" + TagName + "([^>])*>", RegexOptions.IgnoreCase);
                    ConStr = myReg.Replace(ConStr, "");
                    myReg = new Regex("</" + TagName + "([^>])*>", RegexOptions.IgnoreCase);
                    ConStr = myReg.Replace(ConStr, "");
                    break;
            }
            return ConStr;
        }
        #endregion

        #region 过滤html标签
        /// <summary>
        /// 过滤html标签
        /// </summary>
        /// <param name="ContentStr"></param>
        /// <returns></returns>
        public string HtmlScript(string ContentStr)
        {
            ContentStr = Regex.Replace(ContentStr, "<[^>]*>", "");
            return ContentStr;
        }
        #endregion

        #region 执行正则提取出值
        /// <summary>
        /// 执行正则提取出值
        /// </summary>
        /// <param name="RegexString">正则表达式</param>
        /// <param name="pageStr">HtmlCode源代码</param>
        /// <returns></returns>
        public string GetRegValue(string RegexString, string pageStr)
        {
            string resString = "";
            Regex reg = new Regex(RegexString, RegexOptions.IgnoreCase);
            MatchCollection matches = reg.Matches(pageStr);
            foreach (Match match in matches)
            {
                resString += match.Groups[1].Value;
            }
            resString = resString.Replace("&amp;", "&");
            return resString;
        }
        #endregion

        #region 执行正则提取出值
        /// <summary>
        /// 执行正则提取出值
        /// </summary>
        /// <param name="RegexString">正则表达式</param>
        /// <param name="pageStr">HtmlCode源代码</param>
        /// <returns></returns>
        public string GetRegValue2(string RegexString, string pageStr)
        {
            string resString = "";
            Match m = Regex.Match(pageStr, RegexString, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            if (m.Success)
            {
                resString = m.Groups[1].Value;
            }
            resString = resString.Replace("&amp;", "&");
            return resString;
        }
        #endregion
    }
}