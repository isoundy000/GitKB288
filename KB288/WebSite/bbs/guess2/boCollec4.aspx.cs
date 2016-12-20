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
using TPR2.Common;

public partial class bbs_guess2_boCollec4 : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected string xmlPath = "/Controls/guess2.xml";
    protected void Page_Load(object sender, EventArgs e)
    {
        int ptype = int.Parse(Utils.GetRequest("ptype", "get", 1, @"^[0-9]$", "0"));
        int k = int.Parse(Utils.GetRequest("k", "get", 1, @"^[0-9]\d*$", "1"));

        string bo = new TPR2.Collec.Footbo().GetFootbolist(ptype, -1);

        string[] boTemp = Regex.Split(bo, @"<td class=.W1\s[\w\d]+.>");
        for (int i = 1; i < boTemp.Length; i++)
        {
            if (boTemp[i].Contains("<td>↑滾球</td>"))
            {
                //取p_id
                int p_id = 0;
                string strpattern = @"by=detail&amp;eid=(\d+).>析";
                Match mtitle = Regex.Match(boTemp[i], strpattern, RegexOptions.IgnoreCase);
                if (mtitle.Success)
                {
                    p_id = Utils.ParseInt(mtitle.Groups[1].Value);
                    Response.Write(p_id + "<br />");
                }

                //取主队名称
                string p_one = "";
                strpattern = @"<td class=""teamname"">([\s\S]+)<a href=""today.aspx";
                mtitle = Regex.Match(boTemp[i], strpattern, RegexOptions.IgnoreCase);
                if (mtitle.Success)
                {
                    p_one = mtitle.Groups[1].Value.Trim();

                    //这里取主队红牌数量
                    string strpatternHp = @"<span class=""rc"">(\d)</span>";
                    Match mtitleHp = Regex.Match(p_one, strpatternHp, RegexOptions.IgnoreCase);
                    if (mtitleHp.Success)
                    {
                        int hp_one = Utils.ParseInt(mtitleHp.Groups[1].Value);
                        if (hp_one > 0)
                        {
                            new TPR2.BLL.guess.BaList().Updatep_hp_one(p_id, hp_one);
                        }
                    }
                    Response.Write(p_one + "<br />");
                }
                //取客队名称
                string p_two = "";
                strpattern = @"<td>(\d){2}:(\d){2}</td><td class=""teamname"">([\s\S]+)<small>\[[\w\d]+\]</small>(<span class=""rc"">(\d)</span>)*";
                mtitle = Regex.Match(boTemp[i], strpattern, RegexOptions.IgnoreCase);
                if (mtitle.Success)
                {
                    p_two = mtitle.Groups[0].Value.Trim();

                    //这里取客队红牌数量
                    string strpatternHp2 = @"<span class=""rc"">(\d)</span>";
                    Match mtitleHp2 = Regex.Match(p_two, strpatternHp2, RegexOptions.IgnoreCase);
                    if (mtitleHp2.Success)
                    {
                        int hp_two = Utils.ParseInt(mtitleHp2.Groups[1].Value);
                        if (hp_two > 0)
                        {
                            new TPR2.BLL.guess.BaList().Updatep_hp_two(p_id, hp_two);
                        }
                    }
                    Response.Write(p_two + "<br />");
                }
                else
                {
                    strpattern = @"<td>(\d){2}:(\d){2}</td><td class=""teamname"">([\s\S]+)</td></tr>";
                    mtitle = Regex.Match(boTemp[i], strpattern, RegexOptions.IgnoreCase);
                    if (mtitle.Success)
                    {
                        p_two = mtitle.Groups[0].Value.Trim();

                        //这里取客队红牌数量
                        string strpatternHp2 = @"<span class=""rc"">(\d)</span>";
                        Match mtitleHp2 = Regex.Match(p_two, strpatternHp2, RegexOptions.IgnoreCase);
                        if (mtitleHp2.Success)
                        {
                            int hp_two = Utils.ParseInt(mtitleHp2.Groups[1].Value);
                            if (hp_two > 0)
                            {
                                new TPR2.BLL.guess.BaList().Updatep_hp_two(p_id, hp_two);
                            }
                        }
                    }
                    Response.Write(p_two + "<br />");
                }
            }
        }

        binddata();
    }

    private void binddata()
    {
        FileStream fs = new FileStream(Server.MapPath("/Files/sys/loading.gif"), FileMode.Open, FileAccess.Read);
        byte[] mydata = new byte[fs.Length];
        int Length = Convert.ToInt32(fs.Length);
        fs.Read(mydata, 0, Length);
        fs.Close();
        Response.Clear();
        Response.ContentType = "image/gif";
        Response.OutputStream.Write(mydata, 0, Length);
        Response.End();
    }
}