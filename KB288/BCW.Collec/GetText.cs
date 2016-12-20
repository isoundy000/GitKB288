using System;
using System.Collections.Generic;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Text;
using System.Web.Security;
using System.Text.RegularExpressions;
using BCW.Common;

namespace BCW.Collec
{
    public class GetText
    {
        /// <summary>
        /// 得到编码
        /// </summary>
        private Encoding GetEnType(int WebEncode)
        {
            Encoding enType = Encoding.Default;
            switch (WebEncode)
            {
                case 2:
                    enType = Encoding.GetEncoding("gb2312");
                    break;
                case 1:
                    enType = Encoding.UTF8;
                    break;
                case 0:
                    enType = Encoding.Unicode;
                    break;
            }

            return enType;
        }

        public void GetTest(BCW.Model.Collec.CollecItem model, int testType, out string test)
        {
            Encoding enType = GetEnType(model.WebEncode);

            test = string.Empty;
            //分析列表地址
            string Pic = string.Empty;
            if (model.Types == 1)
                Pic = "/files/text/";
            else
                Pic = "/files/pic/act/";

            BCW.Collec.Collec Cn = new BCW.Collec.Collec();
            string testList = Cn.GetHttpPageCode(model.ListUrl, enType);
            if (testList == "$UrlIsFalse")
            {
                new BCW.BLL.Collec.CollecItem().UpdateState(model.ID, 0);
                Utils.Error("列表地址设置错误", "");
            }
            if (testList == "$GetFalse")
            {
                new BCW.BLL.Collec.CollecItem().UpdateState(model.ID, 0);
                Utils.Error("无法连接列表页或连接超时", "");
            }
            testList = Cn.GetBody(testList, Out.WmlDecode(model.ListStart), Out.WmlDecode(model.ListEnd), false, false);
            if (testList == "$StartFalse")
            {
                new BCW.BLL.Collec.CollecItem().UpdateState(model.ID, 0);
                Utils.Error("列表开始标记设置错误,请重新设置", "");
            }
            if (testList == "$EndFalse")
            {
                new BCW.BLL.Collec.CollecItem().UpdateState(model.ID, 0);
                Utils.Error("列表结束标记设置错误,请重新设置", "");
            }
            if (testType == 0)
            {
                test = testList;
            }
            else
            {
                ArrayList linkArray = Cn.GetLinkArray(testList, Out.WmlDecode(model.LinkStart), Out.WmlDecode(model.LinkEnd));
                if (linkArray.Count == 0)
                {
                    new BCW.BLL.Collec.CollecItem().UpdateState(model.ID, 0);
                    Utils.Error("未取到链接,请检查链接设置", "");
                }
                else
                {
                    if (linkArray[0].ToString() == "$StartFalse")
                    {
                        new BCW.BLL.Collec.CollecItem().UpdateState(model.ID, 0);
                        Utils.Error("链接开始标记设置错误,请重新设置", "");
                    }
                    if (linkArray[0].ToString() == "$EndFalse")
                    {
                        new BCW.BLL.Collec.CollecItem().UpdateState(model.ID, 0);
                        Utils.Error("链接结束标记设置错误,请重新设置", "");
                    }
                    if (linkArray[0].ToString() == "$NoneLink")
                    {
                        new BCW.BLL.Collec.CollecItem().UpdateState(model.ID, 0);
                        Utils.Error("未取到链接,请检查链接设置", "");
                    }
                    if (model.IsDesc == 1)
                    {
                        linkArray.Reverse();
                    }
                    //if (model.CollecNum > 0 && linkArray.Count > model.CollecNum)
                    //{
                    //    linkArray.RemoveRange(model.CollecNum, linkArray.Count - model.CollecNum);
                    //}
                    string linkStr = string.Empty;

                    if (testType == 1)//链接地址
                    {
                        for (int i = 0; i < linkArray.Count; i++)
                        {
                            linkStr = Cn.DefiniteUrl(linkArray[i].ToString(), model.WebUrl);
                            if (linkStr != "$False")
                            {
                                linkStr = "<a href=\"" + Out.UBB(linkStr) + "\" target=\"_blank\">" + Out.UBB(linkStr) + "</a><br />";
                                test += linkStr;
                            }
                        }
                    }
                    if (testType == 2)//测试
                    {
                        linkStr = Cn.DefiniteUrl(linkArray[0].ToString(), model.WebUrl);
                        if (linkStr == "$False")
                        {
                            new BCW.BLL.Collec.CollecItem().UpdateState(model.ID, 0);
                            Utils.Error("获取到的链接地址无效,请检查链接设置", "");
                        }

                        string newsCode = Cn.GetHttpPageCode(linkStr, enType);
                        if (newsCode == "$UrlIsFalse")
                        {
                            new BCW.BLL.Collec.CollecItem().UpdateState(model.ID, 0);
                            Utils.Error("获取到的链接地址无效,请检查链接设置", "");
                        }
                        if (newsCode == "$GetFalse")
                        {
                            new BCW.BLL.Collec.CollecItem().UpdateState(model.ID, 0);
                            Utils.Error("无法连接内容页或连接超时", "");
                        }
                        string testTitle = Cn.GetBody(newsCode, Out.WmlDecode(model.TitleStart), Out.WmlDecode(model.TitleEnd), false, false);
                        string testKeyWord = Cn.GetBody(newsCode, Out.WmlDecode(model.KeyWordStart), Out.WmlDecode(model.KeyWordEnd), false, false);
                        string testDateTime = Cn.GetRegValue(model.DateRegex, newsCode);

                        //正文尾双重匹配
                        string testBody = string.Empty;
                        string keyContentEnd = model.ContentEnd;
                        if (keyContentEnd.Contains("$"))
                        {
                            string[] temp = keyContentEnd.Split('$');
                            for (int k = 0; k < temp.Length; k++)
                            {
                                testBody = Cn.GetBody(newsCode, Out.WmlDecode(model.ContentStart), Out.WmlDecode(temp[k]), false, false);
                                if (testBody != "$StartFalse" && testBody != "$EndFalse")
                                {
                                    break;
                                }
                            }
                        }
                        else
                        {
                            testBody = Cn.GetBody(newsCode, Out.WmlDecode(model.ContentStart), Out.WmlDecode(model.ContentEnd), false, false);
                        }

                        if (testTitle == "$StartFalse")
                        {
                            new BCW.BLL.Collec.CollecItem().UpdateState(model.ID, 0);
                            Utils.Error("标题开始标记设置错误,请重新设置", "");
                        }
                        if (testBody == "$StartFalse")
                        {
                            new BCW.BLL.Collec.CollecItem().UpdateState(model.ID, 0);
                            Utils.Error("正文开始标记设置错误,请重新设置", "");
                        }
                        if (testTitle == "$EndFalse")
                        {
                            new BCW.BLL.Collec.CollecItem().UpdateState(model.ID, 0);
                            Utils.Error("标题结束标记设置错误,请重新设置", "");
                        }
                        if (testBody == "$EndFalse")
                        {
                            new BCW.BLL.Collec.CollecItem().UpdateState(model.ID, 0);
                            Utils.Error("正文结束标记设置错误,请重新设置", "");
                        }
                        //------------------获取详细页内容的下一页开始-------------------
                        string NewsNextUrl = Cn.GetRegValue(Out.WmlDecode(model.NextPageRegex), newsCode);

                        int PageCount = 0;
                        while (NewsNextUrl.Length > 0 && PageCount < 5)
                        {
                            string NewsPaingNextCode = string.Empty;
                            string ContentTemp = string.Empty;
                            NewsNextUrl = Cn.DefiniteUrl(NewsNextUrl, model.WebUrl);

                            //NewsNextUrl = NewsNextUrl.Replace("_1_x", "_-1_x");//2012-3-31采集新闻新增替换规则2012-11-13再次修正


                            //HttpContext.Current.Response.Write(NewsNextUrl);
                            //HttpContext.Current.Response.End();

                            NewsPaingNextCode = Cn.GetHttpPageCode(NewsNextUrl, enType);
                            //正文尾双重匹配
                            string keyContentEnd2 = model.ContentEnd;
                            if (keyContentEnd2.Contains("$"))
                            {
                                string[] temp2 = keyContentEnd2.Split('$');
                                for (int k = 0; k < temp2.Length; k++)
                                {
                                    ContentTemp = Cn.GetBody(NewsPaingNextCode, Out.WmlDecode(model.ContentStart), Out.WmlDecode(temp2[k]), false, false);
                                    if (ContentTemp != "$StartFalse" && ContentTemp != "$EndFalse")
                                    {
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                ContentTemp = Cn.GetBody(NewsPaingNextCode, Out.WmlDecode(model.ContentStart), Out.WmlDecode(model.ContentEnd), false, false);
                            }

                            testBody = testBody + "$PageNext$" + ContentTemp;
                            string NewsNextUrl_1 = Cn.GetRegValue(Out.WmlDecode(model.NextPageRegex), NewsPaingNextCode);
                            if (NewsNextUrl_1.Length > 0)
                                NewsNextUrl = Cn.DefiniteUrl(NewsNextUrl_1, model.WebUrl);
                            else
                                break;
                            PageCount++;


                        }
                        //-----------------------获取详细页内容的下一页结束-------------------


                        //-----------------------过滤正文开始-----------------------
                        if (!string.IsNullOrEmpty(model.RemoveBodyStart) && !string.IsNullOrEmpty(model.RemoveBodyEnd))
                        {
                            string[] removeBodyStartArr = Out.WmlDecode(model.RemoveBodyStart).Split('$');
                            string[] removeBodyEndArr = Out.WmlDecode(model.RemoveBodyEnd).Split('$');
                            if (removeBodyStartArr.Length > 1 && removeBodyEndArr.Length > 1)
                            {
                                if (removeBodyStartArr.Length != removeBodyEndArr.Length)
                                {
                                    new BCW.BLL.Collec.CollecItem().UpdateState(model.ID, 0);
                                    Utils.Error("正文过滤中的开始和结束必须对应", "");
                                }
                                else
                                {
                                    for (int i = 0; i < removeBodyStartArr.Length; i++)
                                    {
                                        string remove = Cn.GetBody(testBody, removeBodyStartArr[i], removeBodyEndArr[i], true, true);
                                        testBody = testBody.Replace(remove, "");
                                    }
                                }
                            }
                            else
                            {
                                string remove = Cn.GetBody(testBody, model.RemoveBodyStart, model.RemoveBodyEnd, true, true);
                                testBody = testBody.Replace(remove, "");
                            }
                        }
                        //-----------------------过滤正文结束---------------------------
                        ArrayList testBodyArray = Cn.ReplaceSaveRemoteFile(model.Types, testBody, Pic, model.WebUrl, "0");
                        string cBody = testBodyArray[0].ToString();
                        string txtPic = string.Empty;
                        if (testBodyArray.Count == 2)
                        {
                            txtPic = testBodyArray[1].ToString();
                        }

                        //-------------------正文纯过滤开始-----------------------------
                        if (!string.IsNullOrEmpty(model.RemoveTitle))
                        {
                            string[] temp1 = model.RemoveTitle.Split('$');
                            string[] temp2 = model.RemoveContent.Split('$');
                            for (int k = 0; k < temp1.Length; k++)
                            {
                                string Replacestr = "";
                                try
                                {
                                    Replacestr = temp2[k];
                                }
                                catch
                                {

                                }
                                cBody = Regex.Replace(cBody, Out.WmlDecode(temp1[k]), Out.WmlDecode(Replacestr));
                                
                            }
                        }
                        //过滤烦人的“&**;”
                        cBody = Regex.Replace(cBody, @"[&|&amp;]*[\w\d]+;", "");
                        //-------------------纯过滤结束-----------------------------

                        //---------------过滤开始--------------------
                        if (model.Script_Html.Contains("Iframe"))
                            cBody = Cn.ScriptHtml(cBody, "Iframe", 1);
                        if (model.Script_Html.Contains("Object"))
                            cBody = Cn.ScriptHtml(cBody, "Object", 2);
                        if (model.Script_Html.Contains("Script"))
                            cBody = Cn.ScriptHtml(cBody, "Script", 2);
                        if (model.Script_Html.Contains("Div"))
                            cBody = Cn.ScriptHtml(cBody, "Div", 3);
                        if (model.Script_Html.Contains("Table"))
                            cBody = Cn.ScriptHtml(cBody, "Table", 2);
                        if (model.Script_Html.Contains("Span"))
                            cBody = Cn.ScriptHtml(cBody, "Span", 3);
                        if (model.Script_Html.Contains("Img"))
                            cBody = Cn.ScriptHtml(cBody, "Img", 3);
                        if (model.Script_Html.Contains("Font"))
                            cBody = Cn.ScriptHtml(cBody, "Font", 3);
                        if (model.Script_Html.Contains("A"))
                            cBody = Cn.ScriptHtml(cBody, "A", 3);
                        if (model.Script_Html.Contains("Html"))
                            cBody = Cn.HtmlScript(cBody);
                        //-------------------过滤结束-------------------------------   

                       
                        //组合采样显示
                        test += "标题:" + testTitle + "<br />";
                        test += "时间:" + testDateTime + "<br />";
                        test += "关键字:" + testKeyWord + "<br />";

                        cBody = cBody.Trim();
                        cBody = cBody.Replace(char.ConvertFromUtf32(10), "<br/>");
                        cBody = cBody.Replace("\r", "<br/>");

                        test += "内容:" + cBody + "";
                        if (!string.IsNullOrEmpty(txtPic))
                            test += "<br />图片地址采样" + txtPic;

                    }
                    if (testType == 3)//采集
                    {
                        int successNum = 0;
                        SetProcessBar("从" + model.WebUrl + "采集信息", true);
                        int ListCount = 0;
                        string lUrl = "";
                        while (ListCount >= 0)
                        {

                            for (int i = 0; i < linkArray.Count; i++)
                            {
                                string photoUrl = string.Empty;
                                ProcessBar(i, linkArray.Count);
                                linkStr = Cn.DefiniteUrl(linkArray[i].ToString(), model.WebUrl);
                                if (linkStr == "$False$")
                                {
                                    continue;
                                }
                               

                                string newsPageCode = Cn.GetHttpPageCode(linkStr, enType);
                                if (newsPageCode.Contains("全页显示全文</a>"))
                                {
                                    linkStr = linkStr.Replace("_1_x", "_-1_x");//2012-3-31采集新闻新增替换规则2012-11-13再次修正
                                    newsPageCode = Cn.GetHttpPageCode(linkStr, enType);
                                }
                                if (newsPageCode == "$UrlIsFalse" || newsPageCode == "$GetFalse")
                                {
                                    continue;
                                }
                                string cTitle = Cn.GetBody(newsPageCode, Out.WmlDecode(model.TitleStart), Out.WmlDecode(model.TitleEnd), false, false);
                                string cKeyWord = Cn.GetBody(newsPageCode, Out.WmlDecode(model.KeyWordStart), Out.WmlDecode(model.KeyWordEnd), false, false);
                                string cDateTime = Cn.GetRegValue(model.DateRegex, newsPageCode);
                                //正文尾双重匹配
                                string cBody = string.Empty;
                                string keyContentEnd = model.ContentEnd;
                                if (keyContentEnd.Contains("$"))
                                {
                                    string[] temp = keyContentEnd.Split('$');
                                    for (int k = 0; k < temp.Length; k++)
                                    {
                                        cBody = Cn.GetBody(newsPageCode, Out.WmlDecode(model.ContentStart), Out.WmlDecode(temp[k]), false, false);
                                        if (cBody != "$StartFalse" && cBody != "$EndFalse")
                                        {
                                            break;
                                        }
                                    }

                                }
                                else
                                {
                                    cBody = Cn.GetBody(newsPageCode, Out.WmlDecode(model.ContentStart), Out.WmlDecode(model.ContentEnd), false, false);
                                }
                                if (cTitle == "$StartFalse" || cBody == "$StartFalse" || cTitle == "$EndFalse" || cBody == "$EndFalse")
                                {
                                    continue;
                                }
                                //--------获取详细页内容的下一页开始---------------
                                string NewsNextUrl = Cn.GetRegValue(Out.WmlDecode(model.NextPageRegex), newsPageCode);
                                //int PageCount = 0;
                                while (NewsNextUrl.Length > 0)
                                {
                                    //String sLogFilePath = System.AppDomain.CurrentDomain.BaseDirectory.ToString() + "log.txt";
                                    //LogHelper.Write(sLogFilePath, NewsNextUrl);

                                    string NewsPaingNextCode = string.Empty;
                                    string ContentTemp = string.Empty;
                                    NewsNextUrl = Cn.DefiniteUrl(NewsNextUrl, model.WebUrl);
                                    NewsPaingNextCode = Cn.GetHttpPageCode(NewsNextUrl, enType);
                                    //正文尾双重匹配
                                    string keyContentEnd2 = model.ContentEnd;
                                    if (keyContentEnd2.Contains("$"))
                                    {
                                        string[] temp2 = keyContentEnd2.Split('$');
                                        for (int k = 0; k < temp2.Length; k++)
                                        {
                                            ContentTemp = Cn.GetBody(NewsPaingNextCode, Out.WmlDecode(model.ContentStart), Out.WmlDecode(temp2[k]), false, false);
                                            if (ContentTemp != "$StartFalse" && ContentTemp != "$EndFalse")
                                            {
                                                break;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        ContentTemp = Cn.GetBody(NewsPaingNextCode, Out.WmlDecode(model.ContentStart), Out.WmlDecode(model.ContentEnd), false, false);
                                    }
                                    
                                    cBody = cBody + "$PageNext$" + ContentTemp;
                                    string NewsNextUrl_1 = Cn.GetRegValue(Out.WmlDecode(model.NextPageRegex), NewsPaingNextCode);
                                    if (NewsNextUrl_1.Length > 0)
                                        NewsNextUrl = Cn.DefiniteUrl(NewsNextUrl_1, model.WebUrl);
                                    else
                                        break;
                                    //PageCount++;
                                }
                                //---------获取详细页内容的下一页结束--------------

                                ArrayList bodyArray = Cn.ReplaceSaveRemoteFile(model.Types, cBody, Pic, model.WebUrl, model.IsSaveImg.ToString());
                                if (bodyArray.Count == 2)
                                {
                                    photoUrl = bodyArray[1].ToString();
                                }
                                cBody = bodyArray[0].ToString();
                                cBody = cBody.Replace("'", "");

                                //-------过滤正文开始-----------------------
                                if (!string.IsNullOrEmpty(model.RemoveBodyStart) && !string.IsNullOrEmpty(model.RemoveBodyEnd))
                                {
                                    string[] removeBodyStartArr = Out.WmlDecode(model.RemoveBodyStart).Split('$');
                                    string[] removeBodyEndArr = Out.WmlDecode(model.RemoveBodyEnd).Split('$');
                                    if (removeBodyStartArr.Length > 1 && removeBodyEndArr.Length > 1)
                                    {
                                        for (int j = 0; j < removeBodyStartArr.Length; j++)
                                        {
                                            string remove = Cn.GetBody(cBody, removeBodyStartArr[j], removeBodyEndArr[j], true, true);
                                            cBody = cBody.Replace(remove, "");
                                        }
                                    }
                                    else
                                    {
                                        string remove = Cn.GetBody(cBody, Out.WmlDecode(model.RemoveBodyStart), Out.WmlDecode(model.RemoveBodyEnd), true, true);
                                        cBody = cBody.Replace(remove, "");
                                    }
                                }
                                //--------------过滤正文结束---------------------------

                                //-------------------正文纯过滤开始-----------------------------
                                if (!string.IsNullOrEmpty(model.RemoveTitle))
                                {
                                    string[] temp1 = model.RemoveTitle.Split('$');
                                    string[] temp2 = model.RemoveContent.Split('$');
                                    for (int k = 0; k < temp1.Length; k++)
                                    {
                                        string Replacestr = "";
                                        try
                                        {
                                            Replacestr = temp2[k];
                                        }
                                        catch
                                        {
                                        }
                                        cBody = Regex.Replace(cBody, Out.WmlDecode(temp1[k]), Out.WmlDecode(Replacestr));
                                    }
                                }
                                //过滤烦人的“&**;”
                                cBody = Regex.Replace(cBody, @"[&|&amp;]*[\w\d]+;", "");
                                //-------------------纯过滤结束-----------------------------

                                //---------------过滤开始--------------------
                                if (model.Script_Html.Contains("Iframe"))
                                    cBody = Cn.ScriptHtml(cBody, "Iframe", 1);
                                if (model.Script_Html.Contains("Object"))
                                    cBody = Cn.ScriptHtml(cBody, "Object", 2);
                                if (model.Script_Html.Contains("Script"))
                                    cBody = Cn.ScriptHtml(cBody, "Script", 2);
                                if (model.Script_Html.Contains("Div"))
                                    cBody = Cn.ScriptHtml(cBody, "Div", 3);
                                if (model.Script_Html.Contains("Table"))
                                    cBody = Cn.ScriptHtml(cBody, "Table", 2);
                                if (model.Script_Html.Contains("Span"))
                                    cBody = Cn.ScriptHtml(cBody, "Span", 3);
                                if (model.Script_Html.Contains("Img"))
                                    cBody = Cn.ScriptHtml(cBody, "Img", 3);
                                if (model.Script_Html.Contains("Font"))
                                    cBody = Cn.ScriptHtml(cBody, "Font", 3);
                                if (model.Script_Html.Contains("A"))
                                    cBody = Cn.ScriptHtml(cBody, "A", 3);
                                if (model.Script_Html.Contains("Html"))
                                    cBody = Cn.HtmlScript(cBody);
                                //-------------------过滤结束-------------------------------

                                cBody = cBody.Replace("\r", "");//过滤空行
              


                                //写入数据库
            
                                if (cDateTime == "")
                                    cDateTime = DateTime.Now.ToString();
     
                                //添加验证
                                if (!new BCW.BLL.Detail().Exists(Out.UBB(cTitle)))
                                {
                                    BCW.Model.Detail dmodel = new BCW.Model.Detail();
                                    dmodel.Title = Out.UBB(cTitle);
                                    dmodel.KeyWord = Out.CreateKeyWord(cTitle, 2);
                                    dmodel.Model = "";
                                    dmodel.IsAd = true;
                                    dmodel.Types = model.Types + 10;

                                    //取分类ID
                                    int NodeId = 0;
                                    string strpattern = @"http://m.news.cn/entityitem/(\d+)/(\d+)/[\s\S]+?shtml";
                                    Match mtitle = Regex.Match(linkStr, strpattern, RegexOptions.IgnoreCase);
                                    if (mtitle.Success)
                                    {
                                        NodeId = Convert.ToInt32(mtitle.Groups[1].Value);

                                    }

                                    dmodel.NodeId = NodeId;
                                
                                    dmodel.Content = Out.UBB(cBody.Trim()).Replace("$PageNext$", "");
                                    dmodel.TarText = "";
                                    dmodel.LanText = "";
                                    dmodel.SafeText = "";
                                    dmodel.LyText = "";
                                    dmodel.UpText = "";
                                    dmodel.IsVisa = 0;
                                    try
                                    {
                                        dmodel.AddTime = DateTime.Parse(cDateTime);
                                    }
                                    catch
                                    {
                                        dmodel.AddTime = DateTime.Now;
                                    }
                                    dmodel.Readcount = 0;
                                    dmodel.Recount = 0;
                                    dmodel.Cent = 0;
                                    dmodel.BzType = 0;
                                    dmodel.Hidden = 0;
                                    dmodel.UsID = 0;
                                    int newId = new BCW.BLL.Detail().Add(dmodel);
                                    //更新附件与封面
                                    string Pics = photoUrl;
                                    new BCW.BLL.Detail().UpdatePics(newId, Pics);
                                    if (Pics != "" && Pics.Contains("#"))
                                    {
                                        string[] sTemp = Pics.Split('#');
                                        string sPics = string.Empty;
                                        try
                                        {
                                            if (Pics.Contains("#"))
                                            {
                                                sPics = sTemp[sTemp.Length - 1];
                                            }
                                            else
                                            {
                                                sPics = Pics;
                                            }
                                        }
                                        catch { }
                                        sPics = sPics.Replace("act/", "act/cover/");
                                        sPics = sPics.Replace("text/", "text/cover/");
                                        new BCW.BLL.Detail().UpdateCover(newId, sPics);
                                    }
                                }

                                successNum++;
                            }
                            //--------获取列表下一页开始---------------
                            string ListNextUrl = "";
                            if (lUrl == "")
                            {
                                string gettestList = Cn.GetHttpPageCode(model.ListUrl, enType);
                                ListNextUrl = Cn.GetRegValue(Out.WmlDecode(model.NextListRegex), gettestList);
                            }
                            else
                                ListNextUrl = lUrl;

                            ListNextUrl = Cn.DefiniteUrl(ListNextUrl, model.WebUrl);
                            string testList2 = Cn.GetHttpPageCode(ListNextUrl, enType);
                            if (testList2 != "")
                            {
                                linkArray = Cn.GetLinkArray(testList2, Out.WmlDecode(model.LinkStart), Out.WmlDecode(model.LinkEnd));
                                if (linkArray.Count == 0 || linkArray[0].ToString().Contains("$"))
                                    ListCount = -1;
                                else
                                {
                                    string ListNextUrl_1 = Cn.GetRegValue(Out.WmlDecode(model.NextListRegex), testList2);
                                    if (ListNextUrl_1.Length > 0)
                                        lUrl = ListNextUrl_1;
                                    else
                                        break;

                                    ListCount++;
                                }
                            }
                            else
                            {
                                ListCount = -1;
                            }
                            //---------获取列表下一页结束--------------
                        }
                        HttpContext.Current.Response.End();
                        //HttpContext.Current.Response.Write("<script language='javascript' type='text/javascript'>alert('采集完成,成功采集 " + successNum + "条');window.location='" + Utils.getUrl("collecitem.aspx?act=view&amp;id=" + model.ID + "") + "'</script");

                    }
                }
            }
        }

        protected void SetProcessBar(string tit, bool IsInit)
        {
            if (IsInit)
            {
                string strFileName = HttpContext.Current.Server.MapPath("/Controls/progressbar.htm");
                System.IO.StreamReader sr = new System.IO.StreamReader(strFileName, System.Text.Encoding.Default);
                string strHtml = sr.ReadToEnd();
                HttpContext.Current.Response.Write(strHtml);
                sr.Close();
                HttpContext.Current.Response.Flush();
            }
            HttpContext.Current.Response.Write("<script language='javascript' type='text/javascript'>initPgb('" + tit + "');</script>");
            HttpContext.Current.Response.Flush();
        }

        protected void ProcessBar(int c, int t)
        {
            HttpContext.Current.Response.Write("<script language='javascript' type='text/javascript'>setPgb(" + ((c + 1) * 100) / t + ");</script>");
            HttpContext.Current.Response.Flush();
        }
    }
}
