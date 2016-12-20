using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BCW.Common;

/// <summary>
/// 清空网站XML缓存
/// 
/// 黄国军 20160301
/// </summary>
public partial class rebuildcache : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected void Page_Load(object sender, EventArgs e)
    {
        ub xml = new ub();
        string[] files = "guess2.xml,wap.xml,reg.xml,Dawnlife.xml,bbs.xml".Split(',');
        string xmlPath = "/Controls/";
        if (files.Length > 0)
        {
            for (int i = 0; i < files.Length; i++)
            {
                Application.Remove(xmlPath + files[i]);//清缓存
                xml.ReloadSub(xmlPath + files[i]); //加载配置
                builder.Append("清空" + files[i] + "缓存完成<br />");
            }
        }
    }
}
