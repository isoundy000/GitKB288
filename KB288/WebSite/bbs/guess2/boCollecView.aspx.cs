using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BCW.Common;
using TPR2.Common;
using System.Data;

/// <summary>
/// 判断反向访问是否成功
/// 黄国军 20160704
/// 更新代理IP
/// 黄国军 20160528
/// </summary>
public partial class bbs_guess2_boCollecView : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected string xmlPath = "/Controls/guess2.xml";

    protected void Page_Load(object sender, EventArgs e)
    {
        Master.Title = "更新代理IP";
        string act = Utils.GetRequest("act", "all", 1, "", "");
        #region 页面判定 act
        switch (act)
        {
            default:
                GetBoViewIP();       //代理IP
                break;
        }
        #endregion
    }

    #region 更新IP
    public void GetBoViewIP()
    {
        try
        {
            string ip = "http://" + Utils.GetUsIP() + ":8055/";
            string gstr = GetSourceTextByUrl(ip, "UTF-8");
            if (gstr != "")
            {
                ub xml = new ub();
                string xmlPath = "/Controls/guess2.xml";
                Application.Remove(xmlPath);//清缓存
                xml.ReloadSub(xmlPath); //加载配置
                xml.dss["SiteViewStatus"] = ip;
                System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
                builder.Append("##" + ip + "##");
            }
            else
            {
                builder.Append("##" + ip + ",err##");
            }
        }
        catch (Exception)
        {
        }
    }
    #endregion

    #region 抓取网页源码
    /// <summary>
    /// 抓取网页源码
    /// </summary>
    /// <param name="url"></param>
    /// <param name="Encoding"></param>
    /// <returns></returns>
    private string GetSourceTextByUrl(string url, string Encoding)
    {
        try
        {
            System.Net.WebRequest request = System.Net.WebRequest.Create(url);
            request.Timeout = 20000;
            System.Net.WebResponse response = request.GetResponse();

            System.IO.Stream resStream = response.GetResponseStream();
            System.IO.StreamReader sr = new System.IO.StreamReader(resStream, System.Text.Encoding.GetEncoding(Encoding));
            return sr.ReadToEnd();
        }
        catch
        {
            return "";
        }
    }
    #endregion

}