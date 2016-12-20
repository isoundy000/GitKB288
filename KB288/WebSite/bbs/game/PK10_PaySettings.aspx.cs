using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BCW.Common;
using BCW.PK10;

public partial class bbs_game_PK10_PaySettings : System.Web.UI.Page
{

    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected string GameName = "";
    protected string pk10FileName = "PK10.aspx";
    protected string myFileName = "PK10_PaySettings.aspx";
    protected string xmlPath = "/Controls/PK10.xml";
    protected PK10 _logic;
    protected string defaultPaySettings = "20万|200000#40万|400000#60万|600000#80万|800000#100万|1000000";
    protected void Page_Load(object sender, EventArgs e)
    {
        _logic = new PK10();
        GameName = ub.GetSub("GameName", xmlPath);
        Master.Title = GameName;
        string act = Utils.GetRequest("act", "all", 1, "", "");
        switch(act)
        {
            case "save":
                SaveSetting();
                break;
            default:
                ShowSettings();
                break;
        }

    }

    private void ShowSettings()
    {
        #region
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>");
        builder.Append("&gt; " + " <a href =\"" + Utils.getUrl(pk10FileName) + "\">" + GameName + "</a>");
        builder.Append("&gt; " + "快捷投注" );
        builder.Append(Out.Tab("</div>", ""));
        #endregion
        #region ----显示快捷投注       
        int[] values ={ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        string[] settings = GetSettings().Split('#');
        if(settings.Length>0)
        {
            for(int i=0;i<settings.Length;i++)
            {
                string[] vs = settings[i].Split('|');
                int v = 0;
                if (vs.Length==2)
                {
                    string cv = vs[1];
                    int.TryParse(cv, out v);
                }
                values[i] = v;
            }
        }
        string backurl = Utils.GetRequest("backurl", "all", 1, "", "");
        builder.Append(Out.Tab("<div>", ""));
        //
        string strText = "/,/,/,/,/,/,/,/,/,/,"+"/,/,";
        string strName = "value0,value1,value2,value3,value4,value5,value6,value7,value8,value9," + "act,backurl";
        string strType = "text,text,text,text,text,text,text,text,text,text," + "hidden,hidden";
        string strValu = values[0] + "'" + values[1] + "'" + values[2] + "'" + values[3] + "'" + values[4] + "'" + values[5] + "'" + values[6] + "'" + values[7] + "'" + values[8] + "'" + values[9] + "'" + "save" + "'" + backurl;
        string strEmpt = "true,true,true,true,true,true,true,true,true,true,"+"false,true";
        string strIdea = "/";
        string strOthe = "保存设置," + myFileName + ",post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        //
        builder.Append(Out.Tab("</div>", "<br />"));
        #endregion
        #region
        builder.Append(Out.Tab("<div>", "<br />"));
        backurl = Utils.getUrl(backurl);
        builder.Append("<a href=\"" + backurl + "\">返回游戏</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<font color=\"red\">");
        builder.Append("说明：自己可以添加自己想要的快捷下注数目或者删除已经添加的快捷下注数目,最多添加10个快捷,不能设置超过单注限额数量,以酷币数字输入,留空即为不添加快捷,设置成功后本游戏即能显示支持快捷方式的页面");
        builder.Append("</font>");
        builder.Append(Out.Tab("</div>", "<br />"));
        #endregion
    }

    private string GetSettings()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid <= 0)
            Utils.Login();
        //
        string settings = _logic.GetSettings(meid);
        if (string.IsNullOrEmpty(settings))
        {
            settings = ub.GetSub("defaultPaySettings", xmlPath);
            if (string.IsNullOrEmpty(settings))
                settings = defaultPaySettings;
        }
        //
        return settings;
    }
    private void SaveSetting()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid <= 0)
            Utils.Login();
        #region
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>");
        builder.Append("&gt; " + " <a href =\"" + Utils.getUrl(pk10FileName) + "\">" + GameName + "</a>");
        builder.Append("&gt; " + "快捷投注");
        builder.Append(Out.Tab("</div>", ""));
        #endregion
        #region
        string backurl = Utils.GetRequest("backurl", "all", 1, "", "");
        string[] cvalues = new string[10];
        int[] values = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        int count = 0;
        for (int i = 0; i < 10; i++)
        {
            int v = 0;
            string cv = Utils.GetRequest("value" + i, "all", 1, "", "");
            int.TryParse(cv, out v);
            if(v>0)
            {
                values[count] = v;
                int y = 0;
                int s = Math.DivRem(v, 10000, out y);
                string vn = s.ToString().Trim();
                if(y>0)
                {
                    vn += ".X";
                }
                vn += "万";
                cvalues[count] = vn;
                count++;
            }
        }
        string settings = "";
        string viewstring = "";
        for(int i=0;i<count;i++)
        {
            if (i > 0)
            {
                settings += "#";
                viewstring += " ";
            }
            settings += cvalues[i].ToString().Trim() + "|" + values[i].ToString().Trim();
            viewstring += cvalues[i].ToString().Trim();
        }
        _logic.SaveSettings(meid, settings);
        Utils.Success("成功设置！", "<font color=\"red\">" + viewstring + "</font>", Utils.getUrl(backurl), "2");
        #endregion
    }
}