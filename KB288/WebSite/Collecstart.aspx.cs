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

public partial class Collecstart : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "项目ID错误"));
        BCW.Model.Collec.CollecItem model = new BCW.BLL.Collec.CollecItem().GetCollecItem(id);
        if (model == null)
        {
            Utils.Error("不存在的采集项目", "");
        }
        //开始采集
        string test = string.Empty;
        new BCW.Collec.GetText().GetTest(model, 3, out test);
    }
}
