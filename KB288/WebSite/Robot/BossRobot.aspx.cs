using System;
using System.Data;
using BCW.Common;

public partial class Robot_BossRobot : System.Web.UI.Page
{
    protected string xmlPath = "/Controls/bossRobot.xml";//总机械人XML

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Utils.ParseInt(ub.GetSub("robotopenstate", xmlPath)) == 0)//1开，关0
        {
            Response.Write("<b>总机械人处于关闭状态close1</b><br/>");
        }
        else
        {
            setRobot();
        }
    }

    private void setRobot()
    {
        ub xml = new ub();
        Application.Remove(xmlPath);//清缓存
        xml.ReloadSub(xmlPath); //加载配置

        DateTime data = DateTime.Parse(xml.dss["data"].ToString()); //开始换组的日期
        DateTime startTime = DateTime.Parse(xml.dss["startTime"].ToString()); //开始时间
        DateTime endTime = DateTime.Parse(xml.dss["endTime"].ToString()); //结束时间
        string allRobotID = xml.dss["allRobotID"].ToString();//所有的机械人
        int dayToChange = int.Parse(xml.dss["dayToChange"].ToString());//获取多少天更换一组
        int zushu = int.Parse(xml.dss["zushu"].ToString());//获取在用第几组
        string b_1 = xml.dss["b_1"].ToString();
        string b_2 = xml.dss["b_2"].ToString();
        string b_3 = xml.dss["b_3"].ToString();
        string y_1 = xml.dss["y_1"].ToString();
        string y_2 = xml.dss["y_2"].ToString();
        string y_3 = xml.dss["y_3"].ToString();
        Response.Write("原始换班日期：" + data.ToString("yyyy-MM-dd") + "<br/><br/>");

        //判断是否够3天换班
        //if ((DateTime.Now.Subtract(data).Days) >= dayToChange)
        if ((DateTime.Now) >= data.AddDays(dayToChange))
        {
            zushu++;
            if (zushu > 3)
            {
                zushu = 1;
            }
            xml.dss["zushu"] = zushu;
            xml.dss["data"] = DateTime.Now.ToString("yyyy-MM-dd");
            System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
        }

        //判断现在是否为白班或夜班
        Boolean flag = false; //是否在白天
        flag = DateTime.Now > startTime && DateTime.Now < endTime;
        string robotID = "";//得到一组机器人
        int zz = 0;
        if (flag)//白天
        {
            if (zushu == 1)
            {
                robotID = xml.dss["b_1"].ToString();
                zz = 1;
            }
            else if (zushu == 2)
            {
                robotID = xml.dss["b_2"].ToString();
                zz = 2;
            }
            else
            {
                robotID = xml.dss["b_3"].ToString();
                zz = 3;
            }
            Response.Write("早班第" + zz + "组：" + robotID + "<br/><br/>");
        }
        else
        {
            if (zushu == 1)
            {
                robotID = xml.dss["y_1"].ToString();
                zz = 1;
            }
            else if (zushu == 2)
            {
                robotID = xml.dss["y_2"].ToString();
                zz = 2;
            }
            else
            {
                robotID = xml.dss["y_3"].ToString();
                zz = 3;
            }
            Response.Write("晚班第" + zz + "组：" + robotID + ".<br/><br/>");
        }

        //判断该组机器人个数是否大于游戏需要的个数
        if (robotID.Split('#').Length >= new BCW.JS.BLL.bossrobot().Get_num())
        {
            string[] ss = robotID.Split('#');//分隔机器人ID
            DataSet ds = new BCW.JS.BLL.bossrobot().GetList("*", "type=1 and XML!='' and robotnum>0");
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                int youbiao = 0;//游标
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string setnum = "";//每组机器人ID
                    int ID = int.Parse(ds.Tables[0].Rows[i]["ID"].ToString());
                    int num = int.Parse(ds.Tables[0].Rows[i]["robotnum"].ToString());
                    string GameName = ds.Tables[0].Rows[i]["GameName"].ToString();
                    string xxml = ds.Tables[0].Rows[i]["XML"].ToString();
                    string ziduan = ds.Tables[0].Rows[i]["ziduan"].ToString();

                    for (int aa1 = youbiao; aa1 < (num + youbiao); aa1++)
                    {
                        setnum += ss[aa1] + "#";
                    }
                    setnum = setnum.Substring(0, setnum.Length - 1);
                    Response.Write(GameName + "要：" + setnum + "--------<h style=\"color:red\">总数" + num + "个</h><br/><br/>");
                    youbiao = youbiao + num;

                    try
                    {
                        ub xml1 = new ub();
                        Application.Remove(xxml);//清缓存
                        xml1.ReloadSub(xxml); //加载配置 
                        xml1.dss[ziduan] = setnum;//加机器人ID
                        System.IO.File.WriteAllText(Server.MapPath(xxml), xml1.Post(xml1.dss), System.Text.Encoding.UTF8);
                    }
                    catch
                    {
                        Response.Write(GameName + "XML设置失败！error1");
                    }
                }
            }
            else
                Response.Write("暂无游戏设置！ok1");
        }
        else
        {
            Response.Write("每组机械人个数太少,请添加！error1");
        }
    }

}
