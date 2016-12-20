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

public partial class Manage_app_datasql : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected void Page_Load(object sender, EventArgs e)
    {

        Master.Title = "执行SQL语句";
        builder.Append(Out.Tab("<div class=\"title\">执行SQL语句</div>", ""));
        string act = Utils.GetRequest("act", "post", 1, "", "");
        if (act == "ok")
        {
            string SqlText = Request.Form["SqlText"];
            if (string.IsNullOrEmpty(SqlText))
            {
                Utils.Error("SQL语句不能为空", "");
            }
            SqlText = SqlText.ToLower();
            string SqlKey = "exec,declare,netuser,xp_cmdshell,drop,master,create,alter";//truncate
            string[] temp = SqlKey.Split(",".ToCharArray());
            for (int i = 0; i < temp.Length; i++)
            {
                if (SqlText.Contains(temp[i]))
                {
                    Utils.Error("为了系统安全，" + temp[i] + "方法已被禁止", "");
                }
            }
            builder.Append(Out.Tab("<div>", ""));
            try
            {
                if (!SqlText.Contains("select"))
                {
                    int rows = BCW.Data.SqlHelper.ExecuteSql(SqlText.Trim());
                    builder.Append("执行" + SqlText + "成功|影响" + rows + "行");
                }
                else
                {
                    builder.Append("执行" + SqlText + "<br />");
                    using (System.Data.SqlClient.SqlDataReader reader = BCW.Data.SqlHelper.ExecuteReader(SqlText))
                    {
                        int fieldCount = reader.FieldCount;
                        builder.Append("<table><tr>");
                        for (int i = 0; i < fieldCount; i++)
                        {
                            builder.Append("<td>" + reader.GetName(i) + "</td>");
                        }
                        builder.Append("</tr>");

                        while (reader.Read())
                        {
                            builder.Append("<tr>");
                            for (int i = 0; i < fieldCount; i++)
                            {
                                builder.Append("<td><textarea>");
                                if (!reader.IsDBNull(i))
                                {
                                    builder.Append(GetObjectValue(reader, i));
                                }
                                builder.Append("</textarea></td>");
                            }
                            builder.Append("</tr>");
                        }
                        builder.Append("</table>");
                    }
                }
            }
            catch
            {
                builder.Append("执行" + SqlText + "失败");
            }
            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("<a href=\"" + Utils.getUrl("datasql.aspx") + "\">返回上一级</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
        else
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("语句案例:<br />插入语句: insert into 表名(字段1,字段2)values('内容1','内容2')<br />更新语句: update 表名 set 字段1='内容1',字段2='内容2' where 字段3='内容3'<br />删除语句: delete from 表名 where 字段='内容'<br />查询语句: select top 显示的记录数目 字段1,字段2 from 表名 where 字段1='内容1'");
            builder.Append(Out.Tab("</div>", ""));
            string strText = "输入一条SQL语句:/,";
            string strName = "SqlText,act";
            string strType = "big,hidden";
            string strValu = "'ok";
            string strEmpt = "true,false";
            string strIdea = "/";
            string strOthe = "确定执行|reset,datasql.aspx,post,1,red|blue";

            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("重要提示:<br />必须先备份数据库再进行操作.<br />必须熟悉SQL语句才可以进行操作.");
            builder.Append(Out.Tab("</div>", ""));
        }
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">系统服务中心</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    private string GetObjectValue(System.Data.SqlClient.SqlDataReader reader, int i)
    {
        string fieldType = reader.GetFieldType(i).ToString();
        object FieldValue = null; ;
        if (fieldType.Contains("Int32"))
            FieldValue = reader.GetInt32(i);
        else if (fieldType.Contains("Int64"))
            FieldValue = reader.GetInt64(i);
        else if (fieldType.Contains("Decimal"))
            FieldValue = reader.GetDecimal(i);
        else if (fieldType.Contains("Double"))
            FieldValue = reader.GetDouble(i);
        else if (fieldType.Contains("DateTime"))
            FieldValue = reader.GetDateTime(i);
        else if (fieldType.Contains("String"))
            FieldValue = reader.GetString(i);
        else if (fieldType.Contains("Boolean"))
            FieldValue = reader.GetBoolean(i);
        else if (fieldType.Contains("Byte"))
            FieldValue = reader.GetByte(i);
        else if (fieldType.Contains("Guid"))
            FieldValue = reader.GetGuid(i);
        else
            FieldValue = reader.GetString(i);

        if (FieldValue != null)
            return FieldValue.ToString();
        else
            return "";
    }
}
