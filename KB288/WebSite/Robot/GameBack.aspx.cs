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
using System.Text.RegularExpressions;
using BCW.Common;

public partial class Robot_GameBack : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {

        //防止缓存
        Response.Buffer = true;
        Response.ExpiresAbsolute = DateTime.Now.AddSeconds(-1);
        Response.Expires = 0;
        Response.CacheControl = "no-cache";

        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[1-2]$", "1"));

        if (ptype == 1)
        {
            new BCW.User.Game.Horse().OpenNext();
            Response.Write("[" + DateTime.Now + "]更新跑马下期成功!");

        }
        else if (ptype == 2)
        {
            new BCW.User.Game.Dice().DicePage();
            Response.Write("[" + DateTime.Now + "]更新挖宝开奖成功!");
        }
    }

}