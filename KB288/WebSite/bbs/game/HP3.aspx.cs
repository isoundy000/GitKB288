using System;
using System.Collections.Generic;
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
using BCW.HP3;
using BCW.JS;
/// <summary>
/// �۹��� 20160801 ���Ӷҽ���ˢ
/// ���ڽ� 20160922 �޸Ĳ�ѯ
/// ���ڽ� 20160923 ϵͳ�����ε�ͳ��ID
/// ���ڽ� 20161004 �Ż��Ű�
/// ���ڽ� 20161006 ��д��С��˫����
/// ���ڽ� 20161008 ������ʷ�����н�ע��
/// ���ڽ� 20161018 ǰ̨��������
/// ���ڽ� 20161019 ���Ӷ�̬
/// ���ڽ� 20161027 ��ӿ����ע
/// ���ڽ� 20161028 �����עת������
/// ���ڽ� 20161029 �Ż���ע
/// ���ڽ� 20161101 �޸���ע����
/// ���ڽ� 20161103 Ͷע��Ĭ����0�޸�Ϊ�� �޸���ע���޸�ʽ
///        20161128 top100�Ż�
/// </summary>
public partial class bbs_game_HP3 : System.Web.UI.Page
{
    protected string xmlPath = "/Controls/HappyPoker3.xml";
    protected int SWB = Convert.ToInt32(ub.GetSub("SWB", "/Controls/HappyPoker3.xml"));
    protected string GameName = ub.GetSub("HP3Name", "/Controls/HappyPoker3.xml");
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");

    protected void Page_Load(object sender, EventArgs e)
    {
        string GameName = ub.GetSub("HP3Name", xmlPath);
        string HtestID = ub.GetSub("HtestID", xmlPath);
        //ά����ʾ
        if (ub.GetSub("HP3Status", xmlPath) == "1")
        {
            Utils.Safe("����Ϸ");
        }
        if (ub.GetSub("HP3Status", xmlPath) == "2")//�ڲ�
        {
            //int meid = new BCW.User.Users().GetUsId();
            //if (meid == 0)
            //    Utils.Login();
            string[] sNum = Regex.Split(HtestID, "#");
            int sbsy = 0;
            for (int a = 0; a < sNum.Length; a++)
            {
                if (new BCW.User.Users().GetUsId() == int.Parse(sNum[a].Trim()))
                {
                    sbsy++;
                }
            }
            if (sbsy == 0)
            {
                Utils.Error("��û��ȡ���Դ���Ϸ���ʸ�����ͷ���ϵ��лл��", "/bbs/game");
            }
        }

        string act = Utils.GetRequest("act", "all", 1, "", "");
        switch (act)
        {
            case "case":
                CasePage();
                break;
            case "caseok":
                CaseOkPage();
                break;
            case "casepost":
                CasePostPage();
                break;
            case "info":
                InfoPage();
                break;
            case "pay":
                PayPage();
                break;
            case "mylist":
                MyListPage();
                break;
            case "mylistview":
                MyListViewPage();
                break;
            case "top":
                TopPage();
                break;
            case "rule":
                RulePage();
                break;
            case "list":
                ListPage();
                break;
            case "listview":
                ListViewPage();
                break;
            case "data":
                DataPage();
                break;
            case "bydate":
                ByDateBang();
                break;
            case "getmoney":
                GetMoney();
                break;
            case "trends":
                TrendsPage();
                break;
            default:
                ReloadPage();
                break;
        }
    }
    //��ҳ 
    private void ReloadPage()
    {
        try
        {
            UpdateState();//�Զ���ȡ������Ϣ
        }
        catch
        {

        }

        if (!Utils.Isie() && !Utils.GetUA().ToLower().Contains("opera/8"))
        {
            // Utils.Success("��ܰ��ʾ", "ʹ�òʰ棬ҳ���ֱ�ۣ�����ݣ������л�����...", "HP3.aspx?ve=2a&amp;u=" + Utils.getstrU() + "", "1");
        }
        string GameName = ub.GetSub("HP3Name", xmlPath);
        BCW.HP3.Model.HP3_kjnum model = null;
        try
        {
            model = new BCW.HP3.BLL.HP3_kjnum().GetListLastNull();
        }
        catch { model = new BCW.HP3.BLL.HP3_kjnum().GetListLast(); }
        BCW.HP3.Model.HP3_kjnum model2 = new BCW.HP3.Model.HP3_kjnum();
        model2 = new BCW.HP3.BLL.HP3_kjnum().GetListLast();

        int meid = new BCW.User.Users().GetUsId();
        int dnu;

        Master.Title = GameName;
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">��Ϸ����</a>&gt;" + GameName + "");
        builder.Append(Out.Tab("</div>", "<br />"));

        //��Ϸ��Ubb
        string Top = ub.GetSub("HP3Top", xmlPath);
        if (Top != "")
        {
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append(BCW.User.AdminCall.AdminUBB(Out.SysUBB(Top)));
            builder.Append(Out.Tab("</div>", ""));
        }

        string Logo = ub.GetSub("HP3Logo", xmlPath);
        if (Logo != "")
        {
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("<img src=\"" + Logo + "\"  alt=\"load\" />");
            builder.Append(Out.Tab("</div>", "<br />"));
        }

        builder.Append(Out.Tab("<div>", ""));
        //if (IsOpen() == false)
        //{
        //    builder.Append("��Ϸ����ʱ��:" + ub.GetSub("HP3OnTime", xmlPath) + "<br />");
        //}
        int Sec = Utils.ParseInt(ub.GetSub("HP3Sec", xmlPath));


        if (model2.datetime.AddMinutes(10) < DateTime.Now.AddSeconds(Sec))
        {
            //    dnu = int.Parse(model.datenum) + 1;
            //    string padu = dnu.ToString();
            //    int count = model.datenum.Length - 2;
            //    if (model2.datenum.Substring(count, 2) == "01")
            //    {
            //        //builder.Append("����" + dnu + "�ڿ���<br />");
            //        //djs("djs1", model2.datetime);
            //    }
            //    else if (model.datenum.Substring(count, 2) == "79" && model2.datenum.Substring(count, 2) != "01")
            //    {
            //        builder.Append("ÿ��79��,�����ѿ�79��");
            //    }
            //    else
            //    {
            //        builder.Append(dnu + "�����ڿ���,�����ĵȴ�<br />");
            //    }
        }
        else
        {
            dnu = int.Parse(model2.datenum) + 1;
            string padu = dnu.ToString();
            int end2 = int.Parse(GetLastStr(padu, 2));
        }


        BCW.HP3.Model.HP3_kjnum m = new BCW.HP3.BLL.HP3_kjnum().GetListLastNull();
        if (m != null)
        {
            builder.Append(m.datenum + "�ڿ���:<a href=\"" + Utils.getUrl("HP3.aspx?act=listview&amp;id=" + m.datenum + "") + "\">" + m.Fnum + m.Snum + m.Tnum + "</a><br />");
        }
        int LiangTing = int.Parse(ub.GetSub("HP3LianTing", xmlPath));
        BCW.HP3.Model.HP3_kjnum moo = new BCW.HP3.BLL.HP3_kjnum().GetListLast();
        int end3 = int.Parse(GetLastStr(moo.datenum, 2));


        if (end3 == 79 && moo.Fnum.Trim() != "null")
        {
            builder.Append(moo.datenum + "��Ͷע");
            djs("djs2", moo.datetime.AddSeconds(-Sec));
            builder.Append(" ��ֹ" + "<br />");
        }
        else
        {

            if (GetLastStr(moo.datenum, 2) != "01")
            {
                // if ((int.Parse(moo.datenum) - int.Parse(model.datenum) <= LiangTing))
                {
                    builder.Append(moo.datenum + "��Ͷע");
                    djs("djs2", moo.datetime.AddSeconds(-Sec));
                    builder.Append(" ��ֹ" + "<br />");
                }
            }
            else
            {
                builder.Append(moo.datenum + "��Ͷע");
                djs("djs2", moo.datetime.AddSeconds(-Sec));
                builder.Append(" ��ֹ" + "<br />");
            }
        }



        //builder.Append("|<a href=\"" + Utils.getUrl("HP3.aspx") + "\">ˢ��</a><br />");

        builder.Append("<a href=\"" + Utils.getUrl("HP3.aspx?act=case") + "\">��Ҫ�ҽ�</a>|");
        builder.Append("<a href=\"" + Utils.getUrl("HP3.aspx?act=mylist&amp;ptype=1") + "\">�ҵ�δ��</a>|");
        builder.Append("<a href=\"" + Utils.getUrl("HP3.aspx?act=mylist&amp;ptype=2") + "\">�ҵ���ʷ</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("HP3.aspx?act=rule") + "\">��Ϸ����</a>|");
        builder.Append("<a href=\"" + Utils.getUrl("HP3.aspx?act=data") + "\">���ڷ���</a>|");
        builder.Append("<a href=\"" + Utils.getUrl("HP3.aspx?act=top") + "\">TOP����</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        if (SWB == 0)
        {
            #region ��ʽ��
            builder.Append("�ҵĲƲ���<a href=\"" + Utils.getUrl("../finance.aspx") + "\"><b style=\"color:#f33\">" + Utils.ConvertGold(new BCW.BLL.User().GetGold(meid)) + "</b></a>" + ub.Get("SiteBz") + "<br />");
            #endregion
        }
        else
        {
            #region �����
            int ci = int.Parse(ub.GetSub("GETMONEYCI", xmlPath));
            BCW.HP3.Model.SWB swb = new BCW.HP3.Model.SWB();
            swb.HP3Money = 0;
            swb.HP3IsGet = DateTime.Now;
            if (new BCW.HP3.BLL.SWB().Exists(meid) == true)
            {
                swb = new BCW.HP3.BLL.SWB().GetModel(meid);
            }
            builder.Append("�ҵĲƲ���<a href=\"" + Utils.getUrl("../finance.aspx") + "\"><b style=\"color:#f33\">" + Utils.ConvertGold(swb.HP3Money) + "</b></a>" + "���ֱ�");
            decimal dt1 = Convert.ToDecimal(DateTime.Now.ToString("yyyyMMddHHmmss"));
            decimal dt2 = Convert.ToDecimal(swb.HP3IsGet.AddMinutes(ci).ToString("yyyyMMddHHmmss"));
            decimal dt3 = dt2 - dt1;
            if (dt3 <= 0)
            {
                builder.Append("<a href=\"" + Utils.getUrl("HP3.aspx?act=getmoney") + "\">|������ȡ</a><br />");
                builder.Append(Out.Tab("</div>", ""));
            }
            else
            {
                builder.Append(Out.Tab("</div>", ""));
                builder.Append(Out.Tab("<div>", "<br />"));

                string daojishi2 = new BCW.JS.somejs().newDaojishi("divHP3swb", swb.HP3IsGet.AddMinutes(ci));
                builder.Append("����<b style=\"color:#f33\">" + daojishi2 + "</b>������ȡ");
                builder.Append("|<a href=\"" + Utils.getUrl("HP3.aspx?act=getmoney") + "\">��ȡ</a><br />");

                builder.Append(Out.Tab("</div>", ""));
            }
            #endregion
        }
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("HP3.aspx?act=info&amp;ptype=1") + "\">ͬ��</a>.<a href=\"" + Utils.getUrl("HP3.aspx?act=info&amp;ptype=2") + "\">˳��</a> | <a href=\"" + Utils.getUrl("HP3.aspx?act=info&amp;ptype=3") + "\">Ѻͬ��˳</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("HP3.aspx?act=info&amp;ptype=6") + "\">��һֱѡ</a> | ");
        builder.Append("<a href=\"" + Utils.getUrl("HP3.aspx?act=info&amp;ptype=4") + "\">����</a>.<a href=\"" + Utils.getUrl("HP3.aspx?act=info&amp;ptype=5") + "\">����</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("HP3.aspx?act=info&amp;ptype=7") + "\">�ζ�ֱѡ</a> | <a href=\"" + Utils.getUrl("HP3.aspx?act=info&amp;ptype=8") + "\">�ζ�����</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("HP3.aspx?act=info&amp;ptype=9") + "\">����ֱѡ</a> | <a href=\"" + Utils.getUrl("HP3.aspx?act=info&amp;ptype=10") + "\">��������</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("HP3.aspx?act=info&amp;ptype=11") + "\">����ֱѡ</a> | <a href=\"" + Utils.getUrl("HP3.aspx?act=info&amp;ptype=12") + "\">���ĵ���</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("HP3.aspx?act=info&amp;ptype=13") + "\">����ֱѡ</a> | <a href=\"" + Utils.getUrl("HP3.aspx?act=info&amp;ptype=14") + "\">���嵨��</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("HP3.aspx?act=info&amp;ptype=15") + "\">����ֱѡ</a> | <a href=\"" + Utils.getUrl("HP3.aspx?act=info&amp;ptype=16") + "\">��������</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("HP3.aspx?act=info&amp;ptype=17") + "\">Ѻע��С</a> | <a href=\"" + Utils.getUrl("HP3.aspx?act=info&amp;ptype=18") + "\">Ѻע��˫</a><br />");
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
        builder.Append("����ʷ������");
        builder.Append(Out.Tab("</div>", ""));
        //������¼
        int SizeNum = 3;
        string strWhere = " Fnum!='null'";
        DataSet dataHP3 = new BCW.HP3.BLL.HP3_kjnum().GetListHistory(strWhere, SizeNum);
        if (dataHP3.Tables[0].Rows.Count > 0)
        {
            int i;
            int n = 1;
            for (i = 0; i < dataHP3.Tables[0].Rows.Count; i++)
            {
                object datenum = dataHP3.Tables[0].Rows[i][1];
                object Fnum = dataHP3.Tables[0].Rows[i][3];
                object Snum = dataHP3.Tables[0].Rows[i][4];
                object Tnum = dataHP3.Tables[0].Rows[i][5];
                object DaTime = dataHP3.Tables[0].Rows[i][2];
                builder.Append(Out.Tab("<div>", "<br />"));
                builder.Append("<a href=\"" + Utils.getUrl("HP3.aspx?act=listview&amp;id=" + datenum + "") + "\">" + datenum + "��:" + Fnum + Snum + Tnum + "</a>");
                builder.Append(Out.Tab("</div>", ""));
                n++;
            }
            if (n > SizeNum)
            {
                builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                builder.Append("<a href=\"" + Utils.getUrl("HP3.aspx?act=list&amp;") + "\">&gt;&gt;���࿪����¼</a>");
                builder.Append(Out.Tab("</div>", ""));
            }
        }

        #region ��Ϸ��̬

        //3��ʵʱ��̬
        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("����Ϸ��̬��");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        try
        {
            DataSet ds5 = new BCW.HP3.BLL.HP3Buy().GetList("top 3 *", " BuyID>0 order by ID desc");
            if (ds5 != null && ds5.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds5.Tables[0].Rows.Count; i++)
                {
                    int UsID = int.Parse(ds5.Tables[0].Rows[i]["BuyID"].ToString());
                    string UsName = new BCW.BLL.User().GetUsName(UsID);
                    string addTime = ds5.Tables[0].Rows[i]["BuyTime"].ToString();
                    int qishu = int.Parse(ds5.Tables[0].Rows[i]["BuyDate"].ToString());
                    int Num = int.Parse(ds5.Tables[0].Rows[i]["BuyMoney"].ToString());
                    TimeSpan time = DateTime.Now - Convert.ToDateTime(addTime);

                    int d1 = time.Days;
                    int d = time.Hours;
                    int e = time.Minutes;
                    int f = time.Seconds;
                    if (d1 == 0)
                    {
                        if (d == 0 && e == 0 && f == 0)
                        {
                            builder.Append("<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + UsID + "") + "\">" + UsName + "</a>" + "��" + GameName + "��" + qishu + "����ע**" + ub.Get("SiteBz") + "<br />");// + Num
                        }
                        else if (d == 0 && e == 0)
                            builder.Append(f + "��ǰ" + " " + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + UsID + "") + "\">" + UsName + "</a>" + "��" + GameName + "��" + qishu + "����ע**" + ub.Get("SiteBz") + "<br />");//+ Num 
                        else if (d == 0)
                            builder.Append(e + "��" + f + "��ǰ" + " " + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + UsID + "") + "\">" + UsName + "</a>" + "��" + GameName + "��" + qishu + "����ע**" + ub.Get("SiteBz") + "<br />");// + Num 
                        else
                            builder.Append(d + "Сʱ" + e + "��" + f + "��ǰ" + " " + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + UsID + "") + "\">" + UsName + "</a>" + "��" + GameName + "��" + qishu + "����ע**" + ub.Get("SiteBz") + "<br />");//+ Num
                    }
                    else
                        builder.Append(d1 + "��ǰ" + " " + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + UsID + "") + "\">" + UsName + "</a>" + "��" + GameName + "��" + qishu + "����ע**" + ub.Get("SiteBz") + "<br />");// + Num

                }
            }
            else
            {
                builder.Append("û�и�������...<br />");
            }
        }
        catch
        {
            builder.Append("û������");
        }
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("hp3.aspx?act=trends") + "\">>>���ද̬</a>");
        builder.Append(Out.Tab("</div>", ""));

        #endregion

        //������ʾ
        builder.Append(Out.SysUBB(BCW.User.Users.ShowSpeak(28, "HP3.aspx", 5, 0)));

        //��Ϸ�ײ�Ubb
        string Foot = ub.GetSub("HP3Foot", xmlPath);
        if (Foot != "")
        {
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append(BCW.User.AdminCall.AdminUBB(Out.SysUBB(Foot)));
            builder.Append(Out.Tab("</div>", ""));
        }
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">��ҳ</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">��Ϸ����</a>");
        builder.Append(Out.Tab("</div>", ""));

    }

    #region ��̬
    private void TrendsPage()
    {

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">��Ϸ����</a>&gt;");
        builder.Append("<a href =\"" + Utils.getUrl("hp3.aspx") + "\">" + GameName + "</a>&gt;��Ϸ��̬");
        builder.Append(Out.Tab("</div>", "<br />"));


        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<b>����Ϸ��̬��</b>");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        int pageIndex = 0;//��ǰҳ
        int recordCount;//��¼������
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));//��ҳ��С
        string strWhere = "";
        strWhere = " BuyID>0 ";
        string[] pageValUrl = { "act", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        IList<BCW.HP3.Model.HP3Buy> GetPay = new BCW.HP3.BLL.HP3Buy().GetHP3ListByPage(pageIndex, pageSize, strWhere, out recordCount);

        if (GetPay.Count > 0)
        {
            int k = 1;
            foreach (BCW.HP3.Model.HP3Buy model1 in GetPay)
            {
                if (k % 2 == 0)
                    builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                else
                {
                    if (k == 1)
                        builder.Append(Out.Tab("<div>", ""));
                    else
                        builder.Append(Out.Tab("<div>", "<br />"));
                }
                int d = (pageIndex - 1) * 10 + k;
                string sText = string.Empty;
                string UsName = new BCW.BLL.User().GetUsName(Convert.ToInt32(model1.BuyID));
                sText = "." + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + model1.BuyID + "") + "\">" + UsName + "</a>" + "��" + GameName + "��" + model1.BuyDate + "����ע**" + ub.Get("SiteBz") + "��" + Convert.ToDateTime(model1.BuyTime).ToString("yyyy-MM-dd HH:mm:ss") + "��";//+ model1.BuyMoney 
                builder.AppendFormat("<a href=\"" + Utils.getUrl("hp3.aspx?act=trends&amp;id=" + model1.BuyDate + "&amp;backurl=" + Utils.PostPage(1) + "") + "\"></a>" + d + sText);


                k++;
                builder.Append(Out.Tab("</div>", ""));
            }
        }

        else
        {
            builder.Append(Out.Div("div", "û����ؼ�¼..") + "<br />");
        }
        builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0) + "");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">��ҳ</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">��Ϸ</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("HP3.aspx") + "\">" + GameName + "</a>");
        builder.Append(Out.Tab("</div>", ""));

    }
    #endregion

    //�ҽ�ҳ��
    private void CasePage()
    {
        string GameName = ub.GetSub("HP3Name", xmlPath);
        Master.Title = "�ҽ�����";
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">��Ϸ����</a>&gt;<a href=\"" + Utils.getUrl("HP3.aspx") + "\">" + GameName + "</a>&gt;�ҽ�����");
        builder.Append(Out.Tab("</div>", "<br />"));

        if (SWB == 0)
        {
            #region ��ʽ��
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append("��������" + new BCW.BLL.User().GetGold(meid) + "" + ub.Get("SiteBz") + "");
            builder.Append(Out.Tab("</div>", "<br />"));
            string myid = meid.ToString();
            BCW.HP3.Model.HP3Buy my_buy = new BCW.HP3.Model.HP3Buy();
            BCW.HP3.Model.HP3Winner my_win = new BCW.HP3.Model.HP3Winner();
            DataSet mycase = new BCW.HP3.BLL.HP3Winner().GetMyWinList(myid.Trim());
            string allID = "";
            int pageIndex;
            int recordCount = mycase.Tables[0].Rows.Count;
            int pageSize = 10;
            string[] pageValUrl = { "act", "backurl" };
            pageIndex = Utils.ParseInt(Request.QueryString["page"]);
            if (pageIndex == 0)
                pageIndex = 1;
            int ss = (pageIndex - 1) * 10;
            int som;
            if (recordCount > pageSize * pageIndex)
            {
                som = pageIndex * 10;
            }
            else
            {
                som = recordCount;
            }
            if (som > 0)
            {
                int k = 1;
                string HP3qi = "";
                k = (pageIndex - 1) * 10 + k;
                for (int i = ss; i < som; i++)
                {
                    my_win.ID = Convert.ToInt32(mycase.Tables[0].Rows[i][0]);
                    my_win.WinUserID = Convert.ToInt32(mycase.Tables[0].Rows[i][1]);
                    my_win.WinDate = Convert.ToString(mycase.Tables[0].Rows[i][2]);
                    my_win.WinMoney = Convert.ToInt64(mycase.Tables[0].Rows[i][3]);
                    my_win.WinBool = Convert.ToInt32(mycase.Tables[0].Rows[i][4]);
                    try
                    {
                        my_win.WinZhu = Convert.ToInt32(mycase.Tables[0].Rows[i][5]);
                    }
                    catch
                    {

                        my_win.WinZhu = 0;
                    }

                    my_buy.BuyType = Convert.ToInt32(mycase.Tables[0].Rows[i][9]);
                    my_buy.BuyNum = Convert.ToString(mycase.Tables[0].Rows[i][10]);
                    my_buy.BuyMoney = Convert.ToInt64(mycase.Tables[0].Rows[i][11]);
                    my_buy.BuyZhu = Convert.ToInt32(mycase.Tables[0].Rows[i][12]);
                    my_buy.BuyTime = Convert.ToDateTime(mycase.Tables[0].Rows[i][13]);
                    my_buy.Odds = Convert.ToDecimal(mycase.Tables[0].Rows[i][14]);
                    if (my_win.WinDate.ToString() != HP3qi)
                        builder.Append(Out.Tab("<div>=��" + my_win.WinDate + "��=</div>", "<br />=��" + my_win.WinDate + "��=<br />"));
                    if (k % 2 == 0)
                        builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                    else
                    {
                        if (k == 1)
                            builder.Append(Out.Tab("<div>", ""));
                        else
                            builder.Append(Out.Tab("<div>", "<br />"));
                    }
                    if (my_buy.BuyType == 1)
                    {
                        string st = "null";
                        #region st��ֵ
                        switch (my_buy.BuyNum)
                        {
                            case "1":
                            case "2":
                            case "3":
                            case "4":
                            case "5":
                                st = "ͬ��Ͷע";
                                break;
                            case "6":
                            case "7":
                            case "8":
                            case "9":
                            case "10":
                            case "11":
                            case "12":
                            case "13":
                            case "14":
                            case "15":
                            case "16":
                            case "17":
                            case "18":
                                st = "˳��Ͷע";
                                break;
                            case "19":
                            case "20":
                            case "21":
                            case "22":
                            case "23":
                                st = "ͬ��˳Ͷע";
                                break;
                            case "24":
                            case "25":
                            case "26":
                            case "27":
                            case "28":
                            case "29":
                            case "30":
                            case "31":
                            case "32":
                            case "33":
                            case "34":
                            case "35":
                            case "36":
                            case "37":
                                st = "����Ͷע";
                                break;
                            case "38":
                            case "39":
                            case "40":
                            case "41":
                            case "42":
                            case "43":
                            case "44":
                            case "45":
                            case "46":
                            case "47":
                            case "48":
                            case "49":
                            case "50":
                            case "51":
                                st = "����Ͷע";
                                break;

                        }
                        #endregion
                        builder.Append("<b>" + k + ".[" + st + "]</b>λ��:" + speChoose(Convert.ToInt32(my_buy.BuyNum)) + "/ÿע" + my_buy.BuyMoney + "" + ub.Get("SiteBz") + "/��" + my_buy.BuyZhu + "ע/����" + my_buy.Odds + "[" + DT.FormatDate(my_buy.BuyTime, 1) + "]Ӯ<b style=\"color:red\">" + my_win.WinMoney + "</b>" + ub.Get("SiteBz"));
                        builder.Append("[��" + my_win.WinZhu + "ע]");
                        builder.Append("<a style=\"background:#ADD8E6\" href=\"" + Utils.getUrl("HP3.aspx?act=caseok&amp;pid=" + my_win.ID + "") + "\">�ҽ�</a>");
                    }
                    else if (my_buy.BuyType == 17)
                    {
                        string st = "null";
                        string weihao = "null";
                        #region st��ֵ
                        switch (my_buy.BuyNum)
                        {
                            case "1":
                                st = "��СͶע";
                                weihao = "��";
                                break;
                            case "2":
                                st = "��СͶע";
                                weihao = "С";
                                break;
                            case "3":
                                st = "��˫Ͷע";
                                weihao = "��";
                                break;
                            case "4":
                                st = "��˫Ͷע";
                                weihao = "˫";

                                break;
                        }
                        #endregion
                        builder.Append("<b>" + k + ".[" + st + "]</b>λ��:" + weihao + "/ÿע" + my_buy.BuyMoney + "" + ub.Get("SiteBz") + "/��" + my_buy.BuyZhu + "ע/����" + my_buy.Odds + "[" + DT.FormatDate(my_buy.BuyTime, 1) + "]Ӯ<b style=\"color:red\">" + my_win.WinMoney + "</b>" + ub.Get("SiteBz"));
                        builder.Append("[��" + my_win.WinZhu + "ע]");
                        builder.Append("<a style=\"background:#ADD8E6\" href=\"" + Utils.getUrl("HP3.aspx?act=caseok&amp;pid=" + my_win.ID + "") + "\">�ҽ�</a>");
                    }
                    else
                    {
                        string st = "null";
                        #region st��ֵ
                        switch (my_buy.BuyType)
                        {
                            case 6:
                                st = "��ѡһͶע";
                                break;
                            case 7:
                            case 8:
                                st = "��ѡ��Ͷע";
                                break;
                            case 9:
                            case 10:
                                st = "��ѡ��Ͷע";
                                break;
                            case 11:
                            case 12:
                                st = "��ѡ��Ͷע";
                                break;
                            case 13:
                            case 14:
                                st = "��ѡ��Ͷע";
                                break;
                            case 15:
                            case 16:
                                st = "��ѡ��Ͷע";
                                break;
                        }
                        #endregion
                        builder.Append("<b>" + k + ".[" + st + "]</b>λ��:" + my_buy.BuyNum + "/ÿע" + my_buy.BuyMoney + "" + ub.Get("SiteBz") + "/��" + my_buy.BuyZhu + "ע/����" + my_buy.Odds + "[" + DT.FormatDate(my_buy.BuyTime, 1) + "]Ӯ<b style=\"color:red\">" + my_win.WinMoney + "</b>" + ub.Get("SiteBz"));
                        builder.Append("[��" + my_win.WinZhu + "ע]");
                        builder.Append("<a style=\"background:#ADD8E6\" href=\"" + Utils.getUrl("HP3.aspx?act=caseok&amp;pid=" + my_win.ID + "") + "\">�ҽ�</a>");
                    }
                    k++;
                    HP3qi = my_win.WinDate.ToString();
                    allID = allID + " " + my_win.ID;
                    builder.Append(Out.Tab("</div>", ""));
                }
                // ��ҳ
                builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 1));
            }
            else
            {
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("û����ؼ�¼..");
                builder.Append(Out.Tab("</div>", ""));
            }
            if (!string.IsNullOrEmpty(allID))
            {
                builder.Append(Out.Tab("", "<br />"));
                allID = allID.Trim();
                allID = allID.Replace(" ", ",");
                string strName = "allID,act";
                string strValu = "" + allID + "'casepost";
                string strOthe = "ȫ���ҽ�,HP3.aspx,post,0,red";
                builder.Append(Out.wapform(strName, strValu, strOthe));
            }
            #endregion
        }
        else
        {
            #region �����
            if (ub.GetSub("HP3SWKQ", xmlPath) == "1")
            {
                if (new BCW.HP3.BLL.SWB().Exists(meid) == false)
                {
                    Utils.Error("�ܱ�Ǹ��û�л����Ϸ�Ĳ���Ȩ�ޣ�", "");
                }
            }
            else
            {
                if (new BCW.HP3.BLL.SWB().Exists(meid) == false)
                {
                    Utils.Success("��ܰ��ʾ", "�����ɹ������Ϸ���...", Utils.getUrl("HP3.aspx?getmoney"), "1");
                }
            }
            BCW.HP3.Model.SWB swb = new BCW.HP3.BLL.SWB().GetModel(meid);
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("��������" + swb.HP3Money + "" + "���ֱ�" + "");
            builder.Append(Out.Tab("</div>", "<br />"));
            string myid = meid.ToString();
            BCW.HP3.Model.HP3BuySY my_buy = new BCW.HP3.Model.HP3BuySY();
            BCW.HP3.Model.HP3WinnerSY my_win = new BCW.HP3.Model.HP3WinnerSY();
            DataSet mycase = new BCW.HP3.BLL.HP3WinnerSY().GetMyWinList(myid);
            string allID = "";
            int pageIndex;
            int recordCount = mycase.Tables[0].Rows.Count;
            int pageSize = 10;
            string[] pageValUrl = { "act", "backurl" };
            pageIndex = Utils.ParseInt(Request.QueryString["page"]);
            if (pageIndex == 0)
                pageIndex = 1;
            int ss = (pageIndex - 1) * 10;
            int som;
            if (recordCount > pageSize * pageIndex)
            {
                som = pageIndex * 10;
            }
            else
            {
                som = recordCount;
            }
            if (som >= 0)
            {
                int k = 1;
                string HP3qi = "";
                k = (pageIndex - 1) * 10 + k;
                for (int i = ss; i < som; i++)
                {
                    my_win.ID = Convert.ToInt32(mycase.Tables[0].Rows[i][0]);
                    my_win.WinUserID = Convert.ToInt32(mycase.Tables[0].Rows[i][1]);
                    my_win.WinDate = Convert.ToString(mycase.Tables[0].Rows[i][2]);
                    my_win.WinMoney = Convert.ToInt64(mycase.Tables[0].Rows[i][3]);
                    my_win.WinBool = Convert.ToInt32(mycase.Tables[0].Rows[i][4]);
                    try
                    {
                        my_win.WinZhu = Convert.ToInt32(mycase.Tables[0].Rows[i][5]);
                    }
                    catch
                    {
                        my_win.WinZhu = 0;
                    }
                    my_buy.BuyType = Convert.ToInt32(mycase.Tables[0].Rows[i][9]);
                    my_buy.BuyNum = Convert.ToString(mycase.Tables[0].Rows[i][10]);
                    my_buy.BuyMoney = Convert.ToInt64(mycase.Tables[0].Rows[i][11]);
                    my_buy.BuyZhu = Convert.ToInt32(mycase.Tables[0].Rows[i][12]);
                    my_buy.BuyTime = Convert.ToDateTime(mycase.Tables[0].Rows[i][13]);
                    if (my_win.WinDate.ToString() != HP3qi)
                        builder.Append(Out.Tab("<div>=��" + my_win.WinDate + "��=</div>", "=��" + my_win.WinDate + "��=<br />"));
                    if (k % 2 == 0)
                        builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                    else
                    {
                        if (k == 1)
                            builder.Append(Out.Tab("<div>", ""));
                        else
                            builder.Append(Out.Tab("<div>", "<br />"));
                    }
                    if (my_buy.BuyType == 1)
                    {
                        string st = "null";
                        switch (my_buy.BuyNum)
                        {
                            case "1":
                            case "2":
                            case "3":
                            case "4":
                            case "5":
                                st = "ͬ��Ͷע";
                                break;
                            case "6":
                            case "7":
                            case "8":
                            case "9":
                            case "10":
                            case "11":
                            case "12":
                            case "13":
                            case "14":
                            case "15":
                            case "16":
                            case "17":
                            case "18":
                                st = "˳��Ͷע";
                                break;
                            case "19":
                            case "20":
                            case "21":
                            case "22":
                            case "23":
                                st = "ͬ��˳Ͷע";
                                break;
                            case "24":
                            case "25":
                            case "26":
                            case "27":
                            case "28":
                            case "29":
                            case "30":
                            case "31":
                            case "32":
                            case "33":
                            case "34":
                            case "35":
                            case "36":
                            case "37":
                                st = "����Ͷע";
                                break;
                            case "38":
                            case "39":
                            case "40":
                            case "41":
                            case "42":
                            case "43":
                            case "44":
                            case "45":
                            case "46":
                            case "47":
                            case "48":
                            case "49":
                            case "50":
                            case "51":
                                st = "����Ͷע";
                                break;

                        }
                        builder.Append("<b>" + k + ".[" + st + "]</b>λ��:" + speChoose(Convert.ToInt32(my_buy.BuyNum)) + "/ÿע" + my_buy.BuyMoney + "" + "���ֱ�" + "/��" + my_buy.BuyZhu + "ע[" + my_buy.BuyTime + "]Ӯ<b style=\"color:red\">" + my_win.WinMoney + "</b>" + "���ֱ�");
                        if (my_win.WinZhu != 0)
                        {
                            builder.Append("[��" + my_win.WinZhu + "ע]");
                        }
                        builder.Append("<a style=\"background:#ADD8E6\" href=\"" + Utils.getUrl("HP3.aspx?act=caseok&amp;pid=" + my_win.ID + "") + "\">�ҽ�</a>");
                    }
                    else if (my_buy.BuyType == 17)
                    {
                        string st = "null";
                        string weihao = "null";
                        switch (my_buy.BuyNum)
                        {
                            case "1":
                                st = "��СͶע";
                                weihao = "��";
                                break;
                            case "2":
                                st = "��СͶע";
                                weihao = "С";
                                break;
                            case "3":
                                st = "��˫Ͷע";
                                weihao = "��";
                                break;
                            case "4":
                                st = "��˫Ͷע";
                                weihao = "˫";
                                break;
                        }
                        builder.Append("<b>" + k + ".[" + st + "]</b>λ��:" + weihao + "/ÿע" + my_buy.BuyMoney + "" + "���ֱ�" + "/��" + my_buy.BuyZhu + "ע[" + my_buy.BuyTime + "]Ӯ<b style=\"color:red\">" + my_win.WinMoney + "</b>" + "���ֱ�");
                        if (my_win.WinZhu != 0)
                        {
                            builder.Append("[��" + my_win.WinZhu + "ע]");
                        }
                        builder.Append("<a style=\"background:#ADD8E6\" href=\"" + Utils.getUrl("HP3.aspx?act=caseok&amp;pid=" + my_win.ID + "") + "\">�ҽ�</a>");
                    }
                    else
                    {
                        string st = "null";
                        switch (my_buy.BuyType)
                        {
                            case 6:
                                st = "��ѡһͶע";
                                break;
                            case 7:
                            case 8:
                                st = "��ѡ��Ͷע";
                                break;
                            case 9:
                            case 10:
                                st = "��ѡ��Ͷע";
                                break;
                            case 11:
                            case 12:
                                st = "��ѡ��Ͷע";
                                break;
                            case 13:
                            case 14:
                                st = "��ѡ��Ͷע";
                                break;
                            case 15:
                            case 16:
                                st = "��ѡ��Ͷע";
                                break;
                        }
                        builder.Append("<b>" + k + ".[" + st + "]</b>λ��:" + my_buy.BuyNum + "/ÿע" + my_buy.BuyMoney + "" + "���ֱ�" + "/��" + my_buy.BuyZhu + "ע[" + my_buy.BuyTime + "]Ӯ<b style=\"color:red\">" + my_win.WinMoney + "</b>" + "���ֱ�");
                        if (my_win.WinZhu != 0)
                        {
                            builder.Append("[��" + my_win.WinZhu + "ע]");
                        }
                        builder.Append("<a style=\"background:#ADD8E6\" href=\"" + Utils.getUrl("HP3.aspx?act=caseok&amp;pid=" + my_win.ID + "") + "\">�ҽ�</a>");
                    }
                    k++;
                    HP3qi = my_win.WinDate.ToString();
                    allID = allID + " " + my_win.ID;
                    builder.Append(Out.Tab("</div>", ""));
                }
                // ��ҳ
                builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 1));
            }
            else
            {
                builder.Append(Out.Div("div", "û����ؼ�¼.."));
            }
            if (!string.IsNullOrEmpty(allID))
            {
                builder.Append(Out.Tab("", "<br />"));
                allID = allID.Trim();
                allID = allID.Replace(" ", ",");
                string strName = "allID,act";
                string strValu = "" + allID + "'casepost";
                string strOthe = "ȫ���ҽ�,HP3.aspx,post,0,red";
                builder.Append(Out.wapform(strName, strValu, strOthe));
            }
            #endregion
        }

        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("HP3.aspx?act=mylist&amp;ptype=1") + "\">δ��Ͷע</a> ");
        builder.Append("<a href=\"" + Utils.getUrl("HP3.aspx?act=mylist&amp;ptype=2") + "\">��ʷͶע</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">��ҳ</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">��Ϸ</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("HP3.aspx") + "\">" + GameName + "</a>");
        builder.Append(Out.Tab("</div>", ""));
    }
    //ÿ���ҽ�
    private void CaseOkPage()
    {
        if (SWB == 0)
        {
            #region ��ʽ��
            string GameName = ub.GetSub("HP3Name", xmlPath);
            int meid = new BCW.User.Users().GetUsId();
            //meid = 1555;
            if (meid == 0)
                Utils.Login();
            int pid = Utils.ParseInt(Utils.GetRequest("pid", "get", 2, @"^[0-9]\d*$", "ѡ��ҽ���Ч"));

            if (new BCW.HP3.BLL.HP3Winner().Exists3(pid, meid))
            {
                //������
                BCW.HP3.Model.HP3Winner model = new BCW.HP3.BLL.HP3Winner().GetModel(pid);
                BCW.HP3.Model.HP3Buy model2 = new BCW.HP3.BLL.HP3Buy().GetModel(pid);
                long winMoney = Convert.ToInt64(model.WinMoney);
                BCW.User.Users.IsFresh("hp3", 1);//��ˢ
                new BCW.HP3.BLL.HP3Winner().UpdateByID(pid);

                new BCW.BLL.User().UpdateiGold(meid, winMoney, "" + GameName + "-" + "[url=./game/HP3.aspx?act=BuyWin&qihaos=" + model.WinDate + "&amp;ptype=1]" + model.WinDate + "[/url]" + "-�ҽ�ID" + pid + "");
                if (new BCW.BLL.User().GetIsSpier(meid) != 1)
                    new BCW.BLL.User().UpdateiGold(107, new BCW.BLL.User().GetUsName(107), -winMoney, "ID:[url=forumlog.aspx?act=xview&amp;uid=" + meid + "]" + meid + "[/url]" + GameName + "��[url=./game/HP3.aspx?act=BuyWin&amp;ptype=1&amp;qihaos=" + model.WinDate + "]" + model.WinDate + "[/url]�ڶҽ�" + winMoney + "(��ʶID" + pid + ")");
                Utils.Success("�ҽ�", "��ϲ���ɹ��ҽ�" + winMoney + "" + ub.Get("SiteBz") + "", Utils.getUrl("HP3.aspx?act=case"), "1");
            }
            else
            {
                Utils.Success("�ҽ�", "��ϲ���ظ��ҽ���û�п��Զҽ��ļ�¼", Utils.getUrl("HP3.aspx?act=case"), "1");
            }
            #endregion
        }
        else
        {
            #region �����
            string GameName = ub.GetSub("HP3Name", xmlPath);
            int meid = new BCW.User.Users().GetUsId();
            //meid = 1555;
            if (meid == 0)
                Utils.Login();
            int pid = Utils.ParseInt(Utils.GetRequest("pid", "get", 2, @"^[0-9]\d*$", "ѡ��ҽ���Ч"));

            if (new BCW.HP3.BLL.HP3WinnerSY().Exists3(pid, meid))
            {
                //������
                BCW.HP3.Model.HP3WinnerSY model = new BCW.HP3.BLL.HP3WinnerSY().GetModel(pid);
                long winMoney = Convert.ToInt64(model.WinMoney);
                BCW.User.Users.IsFresh("hp3sw", 1);//��ˢ
                new BCW.HP3.BLL.HP3WinnerSY().UpdateByID(pid);
                new BCW.HP3.BLL.SWB().UpdateHP3Money(meid, winMoney);
                Utils.Success("�ҽ�", "��ϲ���ɹ��ҽ�" + winMoney + "" + "���ֱ�" + "", Utils.getUrl("HP3.aspx?act=case"), "1");
            }
            else
            {
                Utils.Success("�ҽ�", "��ϲ���ظ��ҽ���û�п��Զҽ��ļ�¼", Utils.getUrl("HP3.aspx?act=case"), "1");
            }
            #endregion
        }

    }
    //ȫ���ҽ�
    private void CasePostPage()
    {
        if (SWB == 0)
        {
            #region ��ʽ��
            string GameName = ub.GetSub("HP3Name", xmlPath);
            int meid = new BCW.User.Users().GetUsId();
            if (meid == 0)
                Utils.Login();

            string allID = "";
            allID = Utils.GetRequest("allID", "post", 1, "", "");
            if (!Utils.IsRegex(allID.Replace(",", ""), @"^[0-9]\d*$"))
            {
                Utils.Error("ѡ��ȫ���ҽ�����", "");
            }
            string[] strAllID = allID.Split(",".ToCharArray());
            long getwinMoney = 0;
            long winMoney = 0;
            BCW.User.Users.IsFresh("hp3", 1);//��ˢ
            for (int i = 0; i < strAllID.Length; i++)
            {
                int pid = Convert.ToInt32(strAllID[i]);
                if (new BCW.HP3.BLL.HP3Winner().Exists3(pid, meid))
                {
                    //������
                    BCW.HP3.Model.HP3Winner model = new BCW.HP3.BLL.HP3Winner().GetModel(pid);
                    BCW.HP3.Model.HP3Buy model2 = new BCW.HP3.BLL.HP3Buy().GetModel(pid);
                    winMoney = Convert.ToInt64(model.WinMoney);
                    new BCW.HP3.BLL.HP3Winner().UpdateByID(pid);
                    new BCW.BLL.User().UpdateiGold(meid, winMoney, "" + GameName + "-" + "[url=./game/HP3.aspx?act=BuyWin&qihaos=" + model.WinDate + "&amp;ptype=1]" + model.WinDate + "[/url]" + "-�ҽ�-ID" + pid + "");
                    if (new BCW.BLL.User().GetIsSpier(meid) != 1)
                        new BCW.BLL.User().UpdateiGold(107, new BCW.BLL.User().GetUsName(107), -winMoney, "ID:[url=forumlog.aspx?act=xview&amp;uid=" + meid + "]" + meid + "[/url]" + GameName + "��[url=./game/HP3.aspx?act=BuyWin&amp;ptype=1&amp;qihaos=" + model.WinDate + "]" + model.WinDate + "[/url]�ڶҽ�" + winMoney + "(��ʶID" + pid + ")");
                }

                getwinMoney = getwinMoney + winMoney;
            }
            Utils.Success("�ҽ�", "��ϲ���ɹ��ҽ�" + getwinMoney + "" + ub.Get("SiteBz") + "", Utils.getUrl("HP3.aspx?act=case"), "1");
            #endregion
        }
        else
        {
            #region �����
            string GameName = ub.GetSub("HP3Name", xmlPath);
            int meid = new BCW.User.Users().GetUsId();
            if (meid == 0)
                Utils.Login();

            string allID = "";
            allID = Utils.GetRequest("allID", "post", 1, "", "");
            if (!Utils.IsRegex(allID.Replace(",", ""), @"^[0-9]\d*$"))
            {
                Utils.Error("ѡ��ȫ���ҽ�����", "");
            }
            string[] strAllID = allID.Split(",".ToCharArray());
            long getwinMoney = 0;
            long winMoney = 0;
            BCW.User.Users.IsFresh("hp3sw", 1);//��ˢ
            for (int i = 0; i < strAllID.Length; i++)
            {
                int pid = Convert.ToInt32(strAllID[i]);
                if (new BCW.HP3.BLL.HP3WinnerSY().Exists3(pid, meid))
                {
                    //������
                    BCW.HP3.Model.HP3WinnerSY model = new BCW.HP3.BLL.HP3WinnerSY().GetModel(pid);
                    winMoney = Convert.ToInt64(model.WinMoney);
                    new BCW.HP3.BLL.HP3WinnerSY().UpdateByID(pid);
                    new BCW.HP3.BLL.SWB().UpdateHP3Money(meid, winMoney);
                }

                getwinMoney = getwinMoney + winMoney;
            }
            Utils.Success("�ҽ�", "��ϲ���ɹ��ҽ�" + getwinMoney + "" + "���ֱ�" + "", Utils.getUrl("HP3.aspx?act=case"), "1");
            #endregion
        }
    }
    //����ѡ��ҳ��
    private void InfoPage()
    {
        if (SWB == 0)
        {
            #region ��ʽ��
            string GameName = ub.GetSub("HP3Name", xmlPath);
            int LiangTing = int.Parse(ub.GetSub("HP3LianTing", xmlPath));
            int meid = new BCW.User.Users().GetUsId();
            if (meid == 0)
                Utils.Login();
            BCW.HP3.Model.HP3_kjnum model = new BCW.HP3.Model.HP3_kjnum();
            BCW.HP3.Model.HP3_kjnum model2 = new BCW.HP3.Model.HP3_kjnum();
            try
            {
                model = new BCW.HP3.BLL.HP3_kjnum().GetListLastNull();
            }
            catch { model = new BCW.HP3.BLL.HP3_kjnum().GetListLast(); }
            model2 = new BCW.HP3.BLL.HP3_kjnum().GetListLast();
            int dnu = int.Parse(model2.datenum);
            //if (model.datenum == "0")
            //{
            //    Utils.Error("���ڴ����������Ժ�...", Utils.getUrl("HP3.aspx"));
            //}
            int ptype = Utils.ParseInt(Utils.GetRequest("ptype", "get", 2, @"^[1-9]$|^10$|^11$|^12$|^13$|^14$|^15$|^16$|^17$|^18$", "����ѡ�����"));

            if (!Utils.Isie() && !Utils.GetUA().ToLower().Contains("opera/8"))
            {
                //   Utils.Success("��ܰ��ʾ", "ʹ�òʰ棬��ע��ֱ�ۣ�����ݣ����ڽ���...", "HP3.aspx?act=info&amp;ptype=" + ptype + "&amp;ve=2a&amp;u=" + Utils.getstrU() + "", "1");

            }
            builder.Append(Out.Tab("<div>", ""));
            model2.Fnum = model2.Fnum.Trim();
            if (model2.Fnum != "null")
            {
                dnu = int.Parse(model.datenum) + 1;
            }
            int Sec = Utils.ParseInt(ub.GetSub("HP3Sec", xmlPath));
            int end2 = int.Parse(GetLastStr(model2.datenum, 2));
            //if (int.Parse(model2.datenum) - int.Parse(model.datenum) > LiangTing && end2 != 1)//��Ʊ��ͣͣ��
            //{
            //    Utils.Error("�����ĵȴ�����...", Utils.getUrl("HP3.aspx"));
            //}
            if (DateTime.Now.TimeOfDay >= Convert.ToDateTime("22:00").TimeOfDay && end2 == 79)
            {
                dnu = Convert.ToInt32(DateTime.Now.AddDays(1).Date.ToString("yyyyMMdd") + "01");
            }
            else if (DateTime.Now.TimeOfDay <= Convert.ToDateTime("08:50").TimeOfDay && end2 == 79)
            {
                dnu = Convert.ToInt32(DateTime.Now.Date.ToString("yyyyMMdd") + "01");
            }


            if (GetLastStr(dnu.ToString(), 2) == "01")
            {
                if (DateTime.Now.TimeOfDay > Convert.ToDateTime("09:00").TimeOfDay && DateTime.Now.TimeOfDay < Convert.ToDateTime("22:00").TimeOfDay)
                {
                    Utils.Error("��" + dnu + "���ѽ�ֹ��ע,�ȴ�����...", Utils.getUrl("HP3.aspx"));
                }
            }
            else
            {
                if (model2.Fnum != "null")
                {
                    if (model.datetime.AddMinutes(10) < DateTime.Now.AddSeconds(Sec))
                    {
                        Utils.Error("��" + dnu + "���ѽ�ֹ��ע,�ȴ�����...", Utils.getUrl("HP3.aspx"));
                    }
                }
                else
                {
                    if (model2.datetime < DateTime.Now.AddSeconds(Sec))
                    {
                        Utils.Error("��" + dnu + "���ѽ�ֹ��ע,�ȴ�����...", Utils.getUrl("HP3.aspx"));
                    }
                }
            }
            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">��Ϸ����</a>&gt;<a href=\"" + Utils.getUrl("HP3.aspx") + "\">" + GameName + "</a>&gt;Ͷע");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("��" + dnu + " ��");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            if (GetLastStr(dnu.ToString(), 2) == "01")
            {
                if (DateTime.Now.TimeOfDay >= Convert.ToDateTime("22:00").TimeOfDay)
                {
                    djs("div1", Convert.ToDateTime("9:00").AddDays(1).AddSeconds(-Sec));
                }
                else
                {
                    djs("div1", Convert.ToDateTime("9:00").AddSeconds(-Sec));
                }
            }
            else
            {
                if (model2.Fnum != "null")
                {
                    djs("div1", model.datetime.AddSeconds(-Sec));
                }
                else
                {
                    djs("div1", model2.datetime.AddSeconds(-Sec));
                }
            }
            builder.Append("��ֹ|<a href=\"" + Utils.getUrl("HP3.aspx?act=info&amp;ptype=" + ptype + "") + "\">ˢ��</a><br />");
            builder.Append(Out.Tab("</div>", ""));
            builder.Append("<form id=\"form1\" method=\"post\" action=\"HP3.aspx\">");
            //Ͷע�б�
            if (ptype == 1)
            {
                builder.Append(Out.Tab("<div class=\"text\">", ""));
                builder.Append("���ͣ�<b>ͬ��Ͷע</b>");
                builder.Append(Out.Tab("</div>", "<br />"));
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("<b>����ͬ��</b><input type=\"radio\" name=\"Num1\" value=\"1\" /><br />");
                builder.Append("<b style=\"color:red\">����ͬ��</b><input type=\"radio\" name=\"Num1\" value=\"2\" /><br />");
                builder.Append("<b>÷��ͬ��</b><input type=\"radio\" name=\"Num1\" value=\"3\" /><br />");
                builder.Append("<b style=\"color:red\">����ͬ��</b><input type=\"radio\" name=\"Num1\" value=\"4\" /><br />");
                builder.Append("<b style=\"color:#8A7B66\">ͬ��ȫ��</b><input type=\"radio\" name=\"Num1\" value=\"5\" />");
                builder.Append(Out.Tab("</div>", "<br />"));
            }
            else if (ptype == 2)
            {
                builder.Append(Out.Tab("<div class=\"text\">", ""));
                builder.Append("���ͣ�<b>˳��Ͷע</b>");
                builder.Append(Out.Tab("</div>", "<br />"));
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("<table>");
                builder.Append("<tr><td><b>A23</b></td><td><input type=\"radio\" name=\"Num1\" value=\"6\" /></td><td>");
                builder.Append("<b>234</b></td><td><input type=\"radio\" name=\"Num1\" value=\"7\" /> </td><td>");
                builder.Append("<b>345</b></td><td><input type=\"radio\" name=\"Num1\" value=\"8\" /> </td></tr>");
                builder.Append("<tr><td><b>456</b></td><td><input type=\"radio\" name=\"Num1\" value=\"9\" /> </td><td>");
                builder.Append("<b>567</b></td><td><input type=\"radio\" name=\"Num1\" value=\"10\" /> </td><td>");
                builder.Append("<b>678</b></td><td><input type=\"radio\" name=\"Num1\" value=\"11\" /> </td></tr>");
                builder.Append("<tr><td><b>789</b></td><td><input type=\"radio\" name=\"Num1\" value=\"12\" /> </td><td>");
                builder.Append("<b>8910</b></td><td><input type=\"radio\" name=\"Num1\" value=\"13\" /> </td><td>");
                builder.Append("<b>910J</b></td><td><input type=\"radio\" name=\"Num1\" value=\"14\" /> </td></tr>");
                builder.Append("<tr><td><b>10JQ</b></td><td><input type=\"radio\" name=\"Num1\" value=\"15\" /></td><td> ");
                builder.Append("<b>JQK</b></td><td><input type=\"radio\" name=\"Num1\" value=\"16\" /></td><td>");
                builder.Append("<b>QKA</b></td><td><input type=\"radio\" name=\"Num1\" value=\"17\" /> </td></tr>");
                builder.Append("</table>");
                builder.Append("<b style=\"color:#8A7B66\">˳��ȫ��</b><input type=\"radio\" name=\"Num1\" value=\"18\" />");
                builder.Append(Out.Tab("</div>", "<br />"));

            }
            else if (ptype == 3)
            {
                builder.Append(Out.Tab("<div class=\"text\">", ""));
                builder.Append("���ͣ�<b>ͬ��˳Ͷע</b>");
                builder.Append(Out.Tab("</div>", "<br />"));
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("<b>����ͬ��˳</b><input type=\"radio\" name=\"Num1\" value=\"19\" /><br />");
                builder.Append("<b style=\"color:red\">����ͬ��˳</b><input type=\"radio\" name=\"Num1\" value=\"20\" /> <br />");
                builder.Append("<b>÷��ͬ��˳</b><input type=\"radio\" name=\"Num1\" value=\"21\"/> <br />");
                builder.Append("<b style=\"color:red\">����ͬ��˳</b><input type=\"radio\" name=\"Num1\" value=\"22\" /><br />");
                builder.Append("<b style=\"color:#8A7B66\">ͬ��˳ȫ��</b><input type=\"radio\" name=\"Num1\" value=\"23\" />");
                builder.Append(Out.Tab("</div>", "<br />"));

            }
            else if (ptype == 4)
            {
                builder.Append(Out.Tab("<div class=\"text\">", ""));
                builder.Append("���ͣ�<b>����Ͷע</b>");
                builder.Append(Out.Tab("</div>", "<br />"));
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("<table>");
                builder.Append("<tr><td><b>AAA</b></td><td><input type=\"radio\" name=\"Num1\" value=\"24\" /></td><td>");
                builder.Append("<b>222</b></td><td><input type=\"radio\" name=\"Num1\" value=\"25\" /></td><td>");
                builder.Append("<b>333</b></td><td><input type=\"radio\" name=\"Num1\" value=\"26\" /> </td></tr>");
                builder.Append("<tr><td><b>444</b></td><td><input type=\"radio\" name=\"Num1\" value=\"27\" /> </td><td>");
                builder.Append("<b>555</b></td><td><input type=\"radio\" name=\"Num1\" value=\"28\" /> </td><td>");
                builder.Append("<b>666</b></td><td><input type=\"radio\" name=\"Num1\" value=\"29\" /> </td></tr>");
                builder.Append("<tr><td><b>777</b></td><td><input type=\"radio\" name=\"Num1\" value=\"30\" /></td><td>");
                builder.Append("<b>888</b></td><td><input type=\"radio\" name=\"Num1\" value=\"31\" /> </td><td>");
                builder.Append("<b>999</b></td><td><input type=\"radio\" name=\"Num1\" value=\"32\" /> </td></tr>");
                builder.Append("<tr><td><b>101010</b></td><td><input type=\"radio\" name=\"Num1\" value=\"33\" /></td><td> ");
                builder.Append("<b>JJJ</b></td><td><input type=\"radio\" name=\"Num1\" value=\"34\" /></td><td>");
                builder.Append("<b>QQQ</b></td><td><input type=\"radio\" name=\"Num1\" value=\"35\" /></td></tr>");
                builder.Append("<tr><td><b>KKK</b></td><td><input type=\"radio\" name=\"Num1\" value=\"36\" /> </td></tr>");
                builder.Append("</table>");
                builder.Append("<b style=\"color:#8A7B66\">����ȫ��</b><input type=\"radio\" name=\"Num1\" value=\"37\" />");
                builder.Append(Out.Tab("</div>", "<br />"));

            }
            else if (ptype == 5)
            {
                builder.Append(Out.Tab("<div class=\"text\">", ""));
                builder.Append("���ͣ�<b>����Ͷע</b>");
                builder.Append(Out.Tab("</div>", "<br />"));
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("<table>");
                builder.Append("<tr><td><b>AA</b></td><td><input type=\"radio\" name=\"Num1\" value=\"38\" /></td><td>");
                builder.Append("<b>22</b></td><td><input type=\"radio\" name=\"Num1\" value=\"39\" /> </td><td>");
                builder.Append("<b>33</b></td><td><input type=\"radio\" name=\"Num1\" value=\"40\" /> </td></tr>");
                builder.Append("<tr><td><b>44</b></td><td><input type=\"radio\" name=\"Num1\" value=\"41\" /></td><td>");
                builder.Append("<b>55</b></td><td><input type=\"radio\" name=\"Num1\" value=\"42\" /></td><td>");
                builder.Append("<b>66</b></td><td><input type=\"radio\" name=\"Num1\" value=\"43\" /> </td></tr>");
                builder.Append("<tr><td><b>77</b></td><td><input type=\"radio\" name=\"Num1\" value=\"44\" /></td><td>");
                builder.Append("<b>88</b></td><td><input type=\"radio\" name=\"Num1\" value=\"45\" /> </td><td>");
                builder.Append("<b>99</b></td><td><input type=\"radio\" name=\"Num1\" value=\"46\" /></td></tr>");
                builder.Append("<tr><td><b>1010</b></td><td><input type=\"radio\" name=\"Num1\" value=\"47\" /></td><td> ");
                builder.Append("<b>JJ</b></td><td><input type=\"radio\" name=\"Num1\" value=\"48\" /></td><td> ");
                builder.Append("<b>QQ</b></td><td><input type=\"radio\" name=\"Num1\" value=\"49\" /></td></tr>");
                builder.Append("<tr><td><b>KK</b></td><td><input type=\"radio\" name=\"Num1\" value=\"50\" /> </td></tr>");
                builder.Append("</table>");
                builder.Append("<b style=\"color:#8A7B66\">����ȫ��</b><input type=\"radio\" name=\"Num1\" value=\"51\" />");
                builder.Append(Out.Tab("</div>", "<br />"));

            }
            else if (ptype == 17)//��СͶע
            {
                builder.Append(Out.Tab("<div class=\"text\">", ""));
                builder.Append("���ͣ�<b>��СͶע</b>");
                builder.Append(Out.Tab("</div>", "<br />"));
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("��ǰ���ʣ���<b style=\"color:red\">" + Convert.ToDecimal(XML(21)) + "</b>&nbsp;&nbsp;&nbsp;С<b style=\"color:red\">" + Convert.ToDecimal(XML(22)) + "</b><br />");
                builder.Append("<b>Ѻ��</b><input type=\"radio\" name=\"Num5\" value=\"1\" /> &nbsp;");
                builder.Append("<b>ѺС</b><input type=\"radio\" name=\"Num5\" value=\"2\" /> &nbsp;");
                builder.Append(Out.Tab("</div>", "<br />"));
            }
            else if (ptype == 18)//��˫Ͷע
            {
                builder.Append(Out.Tab("<div class=\"text\">", ""));
                builder.Append("���ͣ�<b>��˫Ͷע</b>");
                builder.Append(Out.Tab("</div>", "<br />"));
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("��ǰ���ʣ���<b style=\"color:red\">" + Convert.ToDecimal(XML(23)) + "</b>&nbsp;&nbsp;&nbsp;˫<b style=\"color:red\">" + Convert.ToDecimal(XML(24)) + "</b><br />");
                builder.Append("<b>Ѻ��</b><input type=\"radio\" name=\"Num5\" value=\"3\" /> &nbsp;");
                builder.Append("<b>Ѻ˫</b><input type=\"radio\" name=\"Num5\" value=\"4\" /> &nbsp;");
                builder.Append(Out.Tab("</div>", "<br />"));

            }
            else
            {
                //6��16Ϊ��ѡͶע��6��7��9��11��13��15ΪֱѡͶע��8��10��12��14��16Ϊ����Ͷע��
                if (ptype == 6 || ptype == 7 || ptype == 9 || ptype == 11 || ptype == 13 || ptype == 15)
                {
                    if (ptype == 6)
                    {
                        builder.Append(Out.Tab("<div class=\"text\">", ""));
                        builder.Append("���ͣ�<b>" + "��ѡһֱѡ" + "</b>");
                        builder.Append(Out.Tab("</div>", "<br />"));
                    }
                    else if (ptype == 7)
                    {
                        builder.Append(Out.Tab("<div class=\"text\">", ""));
                        builder.Append("���ͣ�<b>" + "��ѡ��ֱѡ" + "</b>");
                        builder.Append(Out.Tab("</div>", "<br />"));
                    }
                    else if (ptype == 9)
                    {
                        builder.Append(Out.Tab("<div class=\"text\">", ""));
                        builder.Append("���ͣ�<b>" + "��ѡ��ֱѡ" + "</b>");
                        builder.Append(Out.Tab("</div>", "<br />"));
                    }
                    else if (ptype == 11)
                    {
                        builder.Append(Out.Tab("<div class=\"text\">", ""));
                        builder.Append("���ͣ�<b>" + "��ѡ��ֱѡ" + "</b>");
                        builder.Append(Out.Tab("</div>", "<br />"));

                    }
                    else if (ptype == 13)
                    {
                        builder.Append(Out.Tab("<div class=\"text\">", ""));
                        builder.Append("���ͣ�<b>" + "��ѡ��ֱѡ" + "</b>");
                        builder.Append(Out.Tab("</div>", "<br />"));

                    }
                    else if (ptype == 15)
                    {
                        builder.Append(Out.Tab("<div class=\"text\">", ""));
                        builder.Append("���ͣ�<b>" + "��ѡ��ֱѡ" + "</b>");
                        builder.Append(Out.Tab("</div>", "<br />"));
                    }
                    builder.Append(Out.Tab("<div>", ""));
                    builder.Append("<b>A</b><input type=\"checkbox\" name=\"Num2\" value=\"A\" /> ");
                    builder.Append("<b>2</b><input type=\"checkbox\" name=\"Num2\" value=\"2\" /> ");
                    builder.Append("<b>3</b><input type=\"checkbox\" name=\"Num2\" value=\"3\" /> ");
                    builder.Append("<b>4</b><input type=\"checkbox\" name=\"Num2\" value=\"4\" /> ");
                    builder.Append("<b>5</b><input type=\"checkbox\" name=\"Num2\" value=\"5\" /> ");
                    builder.Append("<b>6</b><input type=\"checkbox\" name=\"Num2\" value=\"6\" /> ");
                    builder.Append("<b>7</b><input type=\"checkbox\" name=\"Num2\" value=\"7\" /><br /> ");
                    builder.Append("<b>8 </b><input type=\"checkbox\" name=\"Num2\" value=\"8\" /> ");
                    builder.Append("<b>9</b><input type=\"checkbox\" name=\"Num2\" value=\"9\" />");
                    builder.Append("<b>10</b><input type=\"checkbox\" name=\"Num2\" value=\"0\" />");
                    builder.Append("<b>J</b><input type=\"checkbox\" name=\"Num2\" value=\"J\" />");
                    builder.Append("<b>Q</b><input type=\"checkbox\" name=\"Num2\" value=\"Q\" />");
                    builder.Append("<b>K</b><input type=\"checkbox\" name=\"Num2\" value=\"K\" />");
                    builder.Append(Out.Tab("</div>", "<br />"));
                }
                else
                {
                    if (ptype == 8)
                    {
                        builder.Append(Out.Tab("<div class=\"text\">", ""));
                        builder.Append("���ͣ�<b>" + "��ѡ������" + "</b>");
                        builder.Append(Out.Tab("</div>", "<br />"));
                    }
                    else if (ptype == 10)
                    {
                        builder.Append(Out.Tab("<div class=\"text\">", ""));
                        builder.Append("���ͣ�<b>" + "��ѡ������" + "</b>");
                        builder.Append(Out.Tab("</div>", "<br />"));
                    }
                    else if (ptype == 12)
                    {
                        builder.Append(Out.Tab("<div class=\"text\">", ""));
                        builder.Append("���ͣ�<b>" + "��ѡ�ĵ���" + "</b>");
                        builder.Append(Out.Tab("</div>", "<br />"));

                    }
                    else if (ptype == 14)
                    {
                        builder.Append(Out.Tab("<div class=\"text\">", ""));
                        builder.Append("���ͣ�<b>" + "��ѡ�嵨��" + "</b>");
                        builder.Append(Out.Tab("</div>", "<br />"));
                    }
                    else if (ptype == 16)
                    {
                        builder.Append(Out.Tab("<div class=\"text\">", ""));
                        builder.Append("���ͣ�<b>" + "��ѡ������" + "</b>");
                        builder.Append(Out.Tab("</div>", "<br />"));
                    }
                    builder.Append(Out.Tab("<div>", ""));
                    builder.Append("<b style=\"color:red\">����ѡ��</b><br /><b>A</b><input type=\"checkbox\" name=\"Num3\" value=\"A\" /> ");
                    builder.Append("<b>2</b><input type=\"checkbox\" name=\"Num3\" value=\"2\" /> ");
                    builder.Append("<b>3</b><input type=\"checkbox\" name=\"Num3\" value=\"3\" /> ");
                    builder.Append("<b>4</b><input type=\"checkbox\" name=\"Num3\" value=\"4\" /> ");
                    builder.Append("<b>5</b><input type=\"checkbox\" name=\"Num3\" value=\"5\" /> ");
                    builder.Append("<b>6</b><input type=\"checkbox\" name=\"Num3\" value=\"6\" /> ");
                    builder.Append("<b>7</b><input type=\"checkbox\" name=\"Num3\" value=\"7\" /><br /> ");
                    builder.Append("&nbsp;<b>8</b><input type=\"checkbox\" name=\"Num3\" value=\"8\" /> ");
                    builder.Append("<b>9</b><input type=\"checkbox\" name=\"Num3\" value=\"9\" />");
                    builder.Append("<b>10</b><input type=\"checkbox\" name=\"Num3\" value=\"0\" />");
                    builder.Append("<b>J</b><input type=\"checkbox\" name=\"Num3\" value=\"J\" />");
                    builder.Append("<b>Q</b><input type=\"checkbox\" name=\"Num3\" value=\"Q\" />");
                    builder.Append("<b>K</b><input type=\"checkbox\" name=\"Num3\" value=\"K\" />");
                    builder.Append(Out.Tab("</div>", "<br />"));
                    builder.Append(Out.Tab("<div>", ""));
                    builder.Append("<b style=\"color:red\">����ѡ��</b><br /><b>A</b><input type=\"checkbox\" name=\"Num4\" value=\"A\" /> ");
                    builder.Append("<b>2</b><input type=\"checkbox\" name=\"Num4\" value=\"2\" /> ");
                    builder.Append("<b>3</b><input type=\"checkbox\" name=\"Num4\" value=\"3\" /> ");
                    builder.Append("<b>4</b><input type=\"checkbox\" name=\"Num4\" value=\"4\" /> ");
                    builder.Append("<b>5</b><input type=\"checkbox\" name=\"Num4\" value=\"5\" /> ");
                    builder.Append("<b>6</b><input type=\"checkbox\" name=\"Num4\" value=\"6\" /> ");
                    builder.Append("<b>7</b><input type=\"checkbox\" name=\"Num4\" value=\"7\" /><br /> ");
                    builder.Append("&nbsp;<b>8</b><input type=\"checkbox\" name=\"Num4\" value=\"8\" /> ");
                    builder.Append("<b>9</b><input type=\"checkbox\" name=\"Num4\" value=\"9\" />");
                    builder.Append("<b>10</b><input type=\"checkbox\" name=\"Num4\" value=\"0\" />");
                    builder.Append("<b>J</b><input type=\"checkbox\" name=\"Num4\" value=\"J\" />");
                    builder.Append("<b>Q</b><input type=\"checkbox\" name=\"Num4\" value=\"Q\" />");
                    builder.Append("<b>K</b><input type=\"checkbox\" name=\"Num4\" value=\"K\" />");
                    builder.Append(Out.Tab("</div>", "<br />"));
                }

            }
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<input type=\"hidden\" name=\"act\" Value=\"pay\"/>");
            builder.Append("<input type=\"hidden\" name=\"ptype\" Value=\"" + ptype + "\"/>");
            string VE = ConfigHelper.GetConfigString("VE");
            string SID = ConfigHelper.GetConfigString("SID");
            builder.AppendFormat("<input type=\"hidden\" name=\"" + VE + "\" value=\"{0}\"/>", Utils.getstrVe());
            builder.AppendFormat("<input type=\"hidden\" name=\"" + SID + "\" value=\"{0}\"/>", Utils.getstrU());
            builder.Append("<input class=\"btn-red\" type=\"submit\" value=\"��һ��\"/>");
            builder.Append(Out.Tab("</div>", ""));
            builder.Append("</form>");

            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("HP3.aspx?act=info&amp;ptype=" + ptype + "") + "\">���ѡ��</a><br />");
            builder.Append(Out.Tab("</div>", ""));

            OutRule(ptype);

            //������¼
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("<b>���ڿ���</b><br/>");
            builder.Append(Out.Tab("</div>", ""));
            try
            {
                int SizeNum = 5;
                string strWhere = " Fnum!='null'";
                DataSet dataHP3 = new BCW.HP3.BLL.HP3_kjnum().GetListHistory(strWhere, SizeNum);
                if (dataHP3.Tables[0].Rows.Count > 0)
                {
                    int i;
                    int n = 1;
                    for (i = 0; i < dataHP3.Tables[0].Rows.Count; i++)
                    {
                        object datenum = dataHP3.Tables[0].Rows[i][1];
                        object Fnum = dataHP3.Tables[0].Rows[i][3];
                        object Snum = dataHP3.Tables[0].Rows[i][4];
                        object Tnum = dataHP3.Tables[0].Rows[i][5];
                        string Winum = Convert.ToString(dataHP3.Tables[0].Rows[i][6]);
                        object DaTime = dataHP3.Tables[0].Rows[i][2];
                        if (i == 0)
                        {
                            builder.Append(Out.Tab("<div>", ""));
                        }
                        else
                            builder.Append(Out.Tab("<div>", "<br />"));
                        builder.Append(datenum + "��:" + Fnum + Snum + Tnum);
                        string[] winums = Winum.Split(',');
                        if (winums[0].Trim() == "0")
                        {
                            builder.Append(" ɢ�� ");
                        }
                        else
                        {
                            string ts = Convert.ToString(winums[0].Trim());
                            switch (ts)
                            {
                                case "1":
                                case "2":
                                case "3":
                                case "4":
                                case "5":
                                    ts = "ͬ��";
                                    break;
                                case "6":
                                case "7":
                                case "8":
                                case "9":
                                case "10":
                                case "11":
                                case "12":
                                case "13":
                                case "14":
                                case "15":
                                case "16":
                                case "17":
                                case "18":
                                    ts = "˳��";
                                    break;
                                case "24":
                                case "25":
                                case "26":
                                case "27":
                                case "28":
                                case "29":
                                case "30":
                                case "31":
                                case "32":
                                case "33":
                                case "34":
                                case "35":
                                case "36":
                                case "37":
                                    ts = "����";
                                    break;
                                case "38":
                                case "39":
                                case "40":
                                case "41":
                                case "42":
                                case "43":
                                case "44":
                                case "45":
                                case "46":
                                case "47":
                                case "48":
                                case "49":
                                case "50":
                                case "51":
                                    ts = "����";
                                    break;
                                default:
                                    ts = "ͬ��˳";
                                    break;
                            }
                            builder.Append(" <b style=\"color:#8A7B66\">" + ts + "</b> ");
                        }
                        if (winums[1].Trim() == winums[2].Trim() && winums[2].Trim() == winums[3].Trim())
                        {
                        }
                        else
                        {
                            int weisu = Convert.ToInt32(winums[4].Trim());
                            if (weisu >= 5)
                            {
                                builder.Append("�� ");
                            }
                            else
                            {
                                builder.Append("С ");
                            }
                            if (weisu % 2 != 0)
                            {
                                builder.Append("��");
                            }
                            else
                            {
                                builder.Append("˫");
                            }
                        }
                        builder.Append(Out.Tab("</div>", ""));
                        n++;
                    }
                }
            }
            catch
            {
            }


            builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
            builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">��ҳ</a>-");
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">��Ϸ</a>-");
            builder.Append("<a href=\"" + Utils.getUrl("HP3.aspx") + "\">" + GameName + "</a>");
            builder.Append(Out.Tab("</div>", ""));
            #endregion
        }
        else
        {
            #region �����
            string GameName = ub.GetSub("HP3Name", xmlPath);
            int LiangTing = int.Parse(ub.GetSub("HP3LianTing", xmlPath));
            int meid = new BCW.User.Users().GetUsId();
            if (meid == 0)
                Utils.Login();
            if (ub.GetSub("HP3SWKQ", xmlPath) == "1")
            {
                if (new BCW.HP3.BLL.SWB().Exists(meid) == false)
                {
                    Utils.Error("�ܱ�Ǹ��û�л����Ϸ�Ĳ���Ȩ�ޣ�", "");
                }
            }
            else
            {
                if (new BCW.HP3.BLL.SWB().Exists(meid) == false)
                {
                    Utils.Success("��ܰ��ʾ", "�����ɹ������Ϸ���...", Utils.getUrl("HP3.aspx?getmoney"), "1");
                }
            }
            BCW.HP3.Model.HP3_kjnum model = new BCW.HP3.Model.HP3_kjnum();
            BCW.HP3.Model.HP3_kjnum model2 = new BCW.HP3.Model.HP3_kjnum();
            model = new BCW.HP3.BLL.HP3_kjnum().GetListLastNull();
            model2 = new BCW.HP3.BLL.HP3_kjnum().GetListLast();
            int dnu = int.Parse(model2.datenum);
            if (model.datenum == "0")
            {
                Utils.Error("���ڴ����������Ժ�...", Utils.getUrl("HP3.aspx"));
            }
            int ptype = Utils.ParseInt(Utils.GetRequest("ptype", "get", 2, @"^[1-9]$|^10$|^11$|^12$|^13$|^14$|^15$|^16$|^17$|^18$", "����ѡ�����"));

            if (!Utils.Isie() && !Utils.GetUA().ToLower().Contains("opera/8"))
            {
                Utils.Success("��ܰ��ʾ", "ʹ�òʰ棬��ע��ֱ�ۣ�����ݣ����ڽ���...", "HP3.aspx?act=info&amp;ptype=" + ptype + "&amp;ve=2a&amp;u=" + Utils.getstrU() + "", "1");

            }

            model2.Fnum = model2.Fnum.Trim();
            if (model2.Fnum != "null")
            {
                dnu = int.Parse(model.datenum) + 1;
            }
            int Sec = Utils.ParseInt(ub.GetSub("HP3Sec", xmlPath));
            int end2 = int.Parse(GetLastStr(model2.datenum, 2));
            if (int.Parse(model2.datenum) - int.Parse(model.datenum) > LiangTing && end2 != 1)//��Ʊ��ͣͣ��
            {
                Utils.Error("�����ĵȴ�����...", Utils.getUrl("HP3.aspx"));
            }
            if (DateTime.Now.TimeOfDay >= Convert.ToDateTime("22:00").TimeOfDay && end2 == 79)
            {
                dnu = Convert.ToInt32(DateTime.Now.AddDays(1).Date.ToString("yyyyMMdd") + "01");
            }
            else if (DateTime.Now.TimeOfDay <= Convert.ToDateTime("08:50").TimeOfDay && end2 == 79)
            {
                dnu = Convert.ToInt32(DateTime.Now.Date.ToString("yyyyMMdd") + "01");
            }


            if (GetLastStr(dnu.ToString(), 2) == "01")
            {
                if (DateTime.Now.TimeOfDay > Convert.ToDateTime("09:00").TimeOfDay && DateTime.Now.TimeOfDay < Convert.ToDateTime("22:00").TimeOfDay)
                {
                    Utils.Error("��" + dnu + "���ѽ�ֹ��ע,�ȴ�����...", Utils.getUrl("HP3.aspx"));
                }
            }
            else
            {
                if (model2.Fnum != "null")
                {
                    if (model.datetime.AddMinutes(10) < DateTime.Now.AddSeconds(Sec))
                    {
                        Utils.Error("��" + dnu + "���ѽ�ֹ��ע,�ȴ�����...", Utils.getUrl("HP3.aspx"));
                    }
                }
                else
                {
                    if (model2.datetime < DateTime.Now.AddSeconds(Sec))
                    {
                        Utils.Error("��" + dnu + "���ѽ�ֹ��ע,�ȴ�����...", Utils.getUrl("HP3.aspx"));
                    }
                }
            }

            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">��Ϸ����</a>&gt;<a href=\"" + Utils.getUrl("HP3.aspx") + "\">" + GameName + "</a>&gt;Ͷע");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("��" + dnu + " ��");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            if (GetLastStr(dnu.ToString(), 2) == "01")
            {
                if (DateTime.Now.TimeOfDay >= Convert.ToDateTime("22:00").TimeOfDay)
                {
                    djs("div1", Convert.ToDateTime("9:00").AddDays(1).AddSeconds(-Sec));
                }
                else
                {
                    djs("div1", Convert.ToDateTime("9:00").AddSeconds(-Sec));
                }
            }
            else
            {
                if (model2.Fnum != "null")
                {
                    djs("div1", model.datetime.AddSeconds(-Sec));
                }
                else
                {
                    djs("div1", model2.datetime.AddSeconds(-Sec));
                }
            }
            builder.Append("��ֹ|<a href=\"" + Utils.getUrl("HP3.aspx?act=info&amp;ptype=" + ptype + "") + "\">ˢ��</a><br />");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append("<form id=\"form1\" method=\"post\" action=\"HP3.aspx\">");
            //Ͷע�б�
            if (ptype == 1)
            {
                builder.Append(Out.Tab("<div class=\"text\">", ""));
                builder.Append("���ͣ�<b>ͬ��Ͷע</b>");
                builder.Append(Out.Tab("</div>", "<br />"));
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("<b>����ͬ��</b><input type=\"radio\" name=\"Num1\" value=\"1\" /><br />");
                builder.Append("<b style=\"color:red\">����ͬ��</b><input type=\"radio\" name=\"Num1\" value=\"2\" /><br />");
                builder.Append("<b>÷��ͬ��</b><input type=\"radio\" name=\"Num1\" value=\"3\" /><br />");
                builder.Append("<b style=\"color:red\">����ͬ��</b><input type=\"radio\" name=\"Num1\" value=\"4\" /><br />");
                builder.Append("<b style=\"color:#8A7B66\">ͬ��ȫ��</b><input type=\"radio\" name=\"Num1\" value=\"5\" />");
                builder.Append(Out.Tab("</div>", "<br />"));
            }
            else if (ptype == 2)
            {
                builder.Append(Out.Tab("<div class=\"text\">", ""));
                builder.Append("���ͣ�<b>˳��Ͷע</b>");
                builder.Append(Out.Tab("</div>", "<br />"));
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("<table>");
                builder.Append("<tr><td><b>A23</b></td><td><input type=\"radio\" name=\"Num1\" value=\"6\" /></td><td>");
                builder.Append("<b>234</b></td><td><input type=\"radio\" name=\"Num1\" value=\"7\" /> </td><td>");
                builder.Append("<b>345</b></td><td><input type=\"radio\" name=\"Num1\" value=\"8\" /> </td></tr>");
                builder.Append("<tr><td><b>456</b></td><td><input type=\"radio\" name=\"Num1\" value=\"9\" /> </td><td>");
                builder.Append("<b>567</b></td><td><input type=\"radio\" name=\"Num1\" value=\"10\" /> </td><td>");
                builder.Append("<b>678</b></td><td><input type=\"radio\" name=\"Num1\" value=\"11\" /> </td></tr>");
                builder.Append("<tr><td><b>789</b></td><td><input type=\"radio\" name=\"Num1\" value=\"12\" /> </td><td>");
                builder.Append("<b>8910</b></td><td><input type=\"radio\" name=\"Num1\" value=\"13\" /> </td><td>");
                builder.Append("<b>910J</b></td><td><input type=\"radio\" name=\"Num1\" value=\"14\" /> </td></tr>");
                builder.Append("<tr><td><b>10JQ</b></td><td><input type=\"radio\" name=\"Num1\" value=\"15\" /></td><td> ");
                builder.Append("<b>JQK</b></td><td><input type=\"radio\" name=\"Num1\" value=\"16\" /></td><td>");
                builder.Append("<b>QKA</b></td><td><input type=\"radio\" name=\"Num1\" value=\"17\" /> </td></tr>");
                builder.Append("</table>");
                builder.Append("<b style=\"color:#8A7B66\">˳��ȫ��</b><input type=\"radio\" name=\"Num1\" value=\"18\" />");
                builder.Append(Out.Tab("</div>", "<br />"));

            }
            else if (ptype == 3)
            {
                builder.Append(Out.Tab("<div class=\"text\">", ""));
                builder.Append("���ͣ�<b>ͬ��˳Ͷע</b>");
                builder.Append(Out.Tab("</div>", "<br />"));
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("<b>����ͬ��˳</b><input type=\"radio\" name=\"Num1\" value=\"19\" /><br />");
                builder.Append("<b style=\"color:red\">����ͬ��˳</b><input type=\"radio\" name=\"Num1\" value=\"20\" /> <br />");
                builder.Append("<b>÷��ͬ��˳</b><input type=\"radio\" name=\"Num1\" value=\"21\"/> <br />");
                builder.Append("<b style=\"color:red\">����ͬ��˳</b><input type=\"radio\" name=\"Num1\" value=\"22\" /><br />");
                builder.Append("<b style=\"color:#8A7B66\">ͬ��˳ȫ��</b><input type=\"radio\" name=\"Num1\" value=\"23\" />");
                builder.Append(Out.Tab("</div>", "<br />"));

            }
            else if (ptype == 4)
            {
                builder.Append(Out.Tab("<div class=\"text\">", ""));
                builder.Append("���ͣ�<b>����Ͷע</b>");
                builder.Append(Out.Tab("</div>", "<br />"));
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("<table>");
                builder.Append("<tr><td><b>AAA</b></td><td><input type=\"radio\" name=\"Num1\" value=\"24\" /></td><td>");
                builder.Append("<b>222</b></td><td><input type=\"radio\" name=\"Num1\" value=\"25\" /></td><td>");
                builder.Append("<b>333</b></td><td><input type=\"radio\" name=\"Num1\" value=\"26\" /> </td></tr>");
                builder.Append("<tr><td><b>444</b></td><td><input type=\"radio\" name=\"Num1\" value=\"27\" /> </td><td>");
                builder.Append("<b>555</b></td><td><input type=\"radio\" name=\"Num1\" value=\"28\" /> </td><td>");
                builder.Append("<b>666</b></td><td><input type=\"radio\" name=\"Num1\" value=\"29\" /> </td></tr>");
                builder.Append("<tr><td><b>777</b></td><td><input type=\"radio\" name=\"Num1\" value=\"30\" /></td><td>");
                builder.Append("<b>888</b></td><td><input type=\"radio\" name=\"Num1\" value=\"31\" /> </td><td>");
                builder.Append("<b>999</b></td><td><input type=\"radio\" name=\"Num1\" value=\"32\" /> </td></tr>");
                builder.Append("<tr><td><b>101010</b></td><td><input type=\"radio\" name=\"Num1\" value=\"33\" /></td><td> ");
                builder.Append("<b>JJJ</b></td><td><input type=\"radio\" name=\"Num1\" value=\"34\" /></td><td>");
                builder.Append("<b>QQQ</b></td><td><input type=\"radio\" name=\"Num1\" value=\"35\" /></td></tr>");
                builder.Append("<tr><td><b>KKK</b></td><td><input type=\"radio\" name=\"Num1\" value=\"36\" /> </td></tr>");
                builder.Append("</table>");
                builder.Append("<b style=\"color:#8A7B66\">����ȫ��</b><input type=\"radio\" name=\"Num1\" value=\"37\" />");
                builder.Append(Out.Tab("</div>", "<br />"));

            }
            else if (ptype == 5)
            {
                builder.Append(Out.Tab("<div class=\"text\">", ""));
                builder.Append("���ͣ�<b>����Ͷע</b>");
                builder.Append(Out.Tab("</div>", "<br />"));
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("<table>");
                builder.Append("<tr><td><b>AA</b></td><td><input type=\"radio\" name=\"Num1\" value=\"38\" /></td><td>");
                builder.Append("<b>22</b></td><td><input type=\"radio\" name=\"Num1\" value=\"39\" /> </td><td>");
                builder.Append("<b>33</b></td><td><input type=\"radio\" name=\"Num1\" value=\"40\" /> </td></tr>");
                builder.Append("<tr><td><b>44</b></td><td><input type=\"radio\" name=\"Num1\" value=\"41\" /></td><td>");
                builder.Append("<b>55</b></td><td><input type=\"radio\" name=\"Num1\" value=\"42\" /></td><td>");
                builder.Append("<b>66</b></td><td><input type=\"radio\" name=\"Num1\" value=\"43\" /> </td></tr>");
                builder.Append("<tr><td><b>77</b></td><td><input type=\"radio\" name=\"Num1\" value=\"44\" /></td><td>");
                builder.Append("<b>88</b></td><td><input type=\"radio\" name=\"Num1\" value=\"45\" /> </td><td>");
                builder.Append("<b>99</b></td><td><input type=\"radio\" name=\"Num1\" value=\"46\" /></td></tr>");
                builder.Append("<tr><td><b>1010</b></td><td><input type=\"radio\" name=\"Num1\" value=\"47\" /></td><td> ");
                builder.Append("<b>JJ</b></td><td><input type=\"radio\" name=\"Num1\" value=\"48\" /></td><td> ");
                builder.Append("<b>QQ</b></td><td><input type=\"radio\" name=\"Num1\" value=\"49\" /></td></tr>");
                builder.Append("<tr><td><b>KK</b></td><td><input type=\"radio\" name=\"Num1\" value=\"50\" /> </td></tr>");
                builder.Append("</table>");
                builder.Append("<b style=\"color:#8A7B66\">����ȫ��</b><input type=\"radio\" name=\"Num1\" value=\"51\" />");
                builder.Append(Out.Tab("</div>", "<br />"));

            }
            else if (ptype == 17)//��СͶע
            {
                builder.Append(Out.Tab("<div class=\"text\">", ""));
                builder.Append("���ͣ�<b>��СͶע</b>");
                builder.Append(Out.Tab("</div>", "<br />"));
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("��ǰ���ʣ���<b style=\"color:red\">" + Convert.ToDecimal(XML(21)) + "</b>&nbsp;&nbsp;&nbsp;С<b style=\"color:red\">" + Convert.ToDecimal(XML(22)) + "</b><br />");
                builder.Append("<b>Ѻ��</b><input type=\"radio\" name=\"Num5\" value=\"1\" /> &nbsp;");
                builder.Append("<b>ѺС</b><input type=\"radio\" name=\"Num5\" value=\"2\" /> &nbsp;");
                builder.Append(Out.Tab("</div>", "<br />"));
            }
            else if (ptype == 18)//��˫Ͷע
            {
                builder.Append(Out.Tab("<div class=\"text\">", ""));
                builder.Append("���ͣ�<b>��˫Ͷע</b>");
                builder.Append(Out.Tab("</div>", "<br />"));
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("��ǰ���ʣ���<b style=\"color:red\">" + Convert.ToDecimal(XML(23)) + "</b>&nbsp;&nbsp;&nbsp;˫<b style=\"color:red\">" + Convert.ToDecimal(XML(24)) + "</b><br />");
                builder.Append("<b>Ѻ��</b><input type=\"radio\" name=\"Num5\" value=\"3\" /> &nbsp;");
                builder.Append("<b>Ѻ˫</b><input type=\"radio\" name=\"Num5\" value=\"4\" /> &nbsp;");
                builder.Append(Out.Tab("</div>", "<br />"));

            }
            else
            {
                //6��16Ϊ��ѡͶע��6��7��9��11��13��15ΪֱѡͶע��8��10��12��14��16Ϊ����Ͷע��
                if (ptype == 6 || ptype == 7 || ptype == 9 || ptype == 11 || ptype == 13 || ptype == 15)
                {
                    if (ptype == 6)
                    {
                        builder.Append(Out.Tab("<div class=\"text\">", ""));
                        builder.Append("���ͣ�<b>" + "��ѡһֱѡ" + "</b>");
                        builder.Append(Out.Tab("</div>", "<br />"));
                    }
                    else if (ptype == 7)
                    {
                        builder.Append(Out.Tab("<div class=\"text\">", ""));
                        builder.Append("���ͣ�<b>" + "��ѡ��ֱѡ" + "</b>");
                        builder.Append(Out.Tab("</div>", "<br />"));
                    }
                    else if (ptype == 9)
                    {
                        builder.Append(Out.Tab("<div class=\"text\">", ""));
                        builder.Append("���ͣ�<b>" + "��ѡ��ֱѡ" + "</b>");
                        builder.Append(Out.Tab("</div>", "<br />"));
                    }
                    else if (ptype == 11)
                    {
                        builder.Append(Out.Tab("<div class=\"text\">", ""));
                        builder.Append("���ͣ�<b>" + "��ѡ��ֱѡ" + "</b>");
                        builder.Append(Out.Tab("</div>", "<br />"));

                    }
                    else if (ptype == 13)
                    {
                        builder.Append(Out.Tab("<div class=\"text\">", ""));
                        builder.Append("���ͣ�<b>" + "��ѡ��ֱѡ" + "</b>");
                        builder.Append(Out.Tab("</div>", "<br />"));

                    }
                    else if (ptype == 15)
                    {
                        builder.Append(Out.Tab("<div class=\"text\">", ""));
                        builder.Append("���ͣ�<b>" + "��ѡ��ֱѡ" + "</b>");
                        builder.Append(Out.Tab("</div>", "<br />"));
                    }
                    builder.Append(Out.Tab("<div>", ""));
                    builder.Append("<b>A</b><input type=\"checkbox\" name=\"Num2\" value=\"A\" /> ");
                    builder.Append("<b>2</b><input type=\"checkbox\" name=\"Num2\" value=\"2\" /> ");
                    builder.Append("<b>3</b><input type=\"checkbox\" name=\"Num2\" value=\"3\" /> ");
                    builder.Append("<b>4</b><input type=\"checkbox\" name=\"Num2\" value=\"4\" /> ");
                    builder.Append("<b>5</b><input type=\"checkbox\" name=\"Num2\" value=\"5\" /> ");
                    builder.Append("<b>6</b><input type=\"checkbox\" name=\"Num2\" value=\"6\" /> ");
                    builder.Append("<b>7</b><input type=\"checkbox\" name=\"Num2\" value=\"7\" /><br /> ");
                    builder.Append("<b>8 </b><input type=\"checkbox\" name=\"Num2\" value=\"8\" /> ");
                    builder.Append("<b>9</b><input type=\"checkbox\" name=\"Num2\" value=\"9\" />");
                    builder.Append("<b>10</b><input type=\"checkbox\" name=\"Num2\" value=\"0\" />");
                    builder.Append("<b>J</b><input type=\"checkbox\" name=\"Num2\" value=\"J\" />");
                    builder.Append("<b>Q</b><input type=\"checkbox\" name=\"Num2\" value=\"Q\" />");
                    builder.Append("<b>K</b><input type=\"checkbox\" name=\"Num2\" value=\"K\" />");
                    builder.Append(Out.Tab("</div>", "<br />"));
                }
                else
                {
                    if (ptype == 8)
                    {
                        builder.Append(Out.Tab("<div class=\"text\">", ""));
                        builder.Append("���ͣ�<b>" + "��ѡ������" + "</b>");
                        builder.Append(Out.Tab("</div>", "<br />"));
                    }
                    else if (ptype == 10)
                    {
                        builder.Append(Out.Tab("<div class=\"text\">", ""));
                        builder.Append("���ͣ�<b>" + "��ѡ������" + "</b>");
                        builder.Append(Out.Tab("</div>", "<br />"));
                    }
                    else if (ptype == 12)
                    {
                        builder.Append(Out.Tab("<div class=\"text\">", ""));
                        builder.Append("���ͣ�<b>" + "��ѡ�ĵ���" + "</b>");
                        builder.Append(Out.Tab("</div>", "<br />"));

                    }
                    else if (ptype == 14)
                    {
                        builder.Append(Out.Tab("<div class=\"text\">", ""));
                        builder.Append("���ͣ�<b>" + "��ѡ�嵨��" + "</b>");
                        builder.Append(Out.Tab("</div>", "<br />"));
                    }
                    else if (ptype == 16)
                    {
                        builder.Append(Out.Tab("<div class=\"text\">", ""));
                        builder.Append("���ͣ�<b>" + "��ѡ������" + "</b>");
                        builder.Append(Out.Tab("</div>", "<br />"));
                    }
                    builder.Append(Out.Tab("<div>", ""));
                    builder.Append("<b style=\"color:red\">����ѡ��</b><br /><b>A</b><input type=\"checkbox\" name=\"Num3\" value=\"A\" /> ");
                    builder.Append("<b>2</b><input type=\"checkbox\" name=\"Num3\" value=\"2\" /> ");
                    builder.Append("<b>3</b><input type=\"checkbox\" name=\"Num3\" value=\"3\" /> ");
                    builder.Append("<b>4</b><input type=\"checkbox\" name=\"Num3\" value=\"4\" /> ");
                    builder.Append("<b>5</b><input type=\"checkbox\" name=\"Num3\" value=\"5\" /> ");
                    builder.Append("<b>6</b><input type=\"checkbox\" name=\"Num3\" value=\"6\" /> ");
                    builder.Append("<b>7</b><input type=\"checkbox\" name=\"Num3\" value=\"7\" /><br /> ");
                    builder.Append("&nbsp;<b>8</b><input type=\"checkbox\" name=\"Num3\" value=\"8\" /> ");
                    builder.Append("<b>9</b><input type=\"checkbox\" name=\"Num3\" value=\"9\" />");
                    builder.Append("<b>10</b><input type=\"checkbox\" name=\"Num3\" value=\"0\" />");
                    builder.Append("<b>J</b><input type=\"checkbox\" name=\"Num3\" value=\"J\" />");
                    builder.Append("<b>Q</b><input type=\"checkbox\" name=\"Num3\" value=\"Q\" />");
                    builder.Append("<b>K</b><input type=\"checkbox\" name=\"Num3\" value=\"K\" />");
                    builder.Append(Out.Tab("</div>", "<br />"));
                    builder.Append(Out.Tab("<div>", ""));
                    builder.Append("<b style=\"color:red\">����ѡ��</b><br /><b>A</b><input type=\"checkbox\" name=\"Num4\" value=\"A\" /> ");
                    builder.Append("<b>2</b><input type=\"checkbox\" name=\"Num4\" value=\"2\" /> ");
                    builder.Append("<b>3</b><input type=\"checkbox\" name=\"Num4\" value=\"3\" /> ");
                    builder.Append("<b>4</b><input type=\"checkbox\" name=\"Num4\" value=\"4\" /> ");
                    builder.Append("<b>5</b><input type=\"checkbox\" name=\"Num4\" value=\"5\" /> ");
                    builder.Append("<b>6</b><input type=\"checkbox\" name=\"Num4\" value=\"6\" /> ");
                    builder.Append("<b>7</b><input type=\"checkbox\" name=\"Num4\" value=\"7\" /><br /> ");
                    builder.Append("&nbsp;<b>8</b><input type=\"checkbox\" name=\"Num4\" value=\"8\" /> ");
                    builder.Append("<b>9</b><input type=\"checkbox\" name=\"Num4\" value=\"9\" />");
                    builder.Append("<b>10</b><input type=\"checkbox\" name=\"Num4\" value=\"0\" />");
                    builder.Append("<b>J</b><input type=\"checkbox\" name=\"Num4\" value=\"J\" />");
                    builder.Append("<b>Q</b><input type=\"checkbox\" name=\"Num4\" value=\"Q\" />");
                    builder.Append("<b>K</b><input type=\"checkbox\" name=\"Num4\" value=\"K\" />");
                    builder.Append(Out.Tab("</div>", "<br />"));
                }

            }
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<input type=\"hidden\" name=\"act\" Value=\"pay\"/>");
            builder.Append("<input type=\"hidden\" name=\"ptype\" Value=\"" + ptype + "\"/>");
            string VE = ConfigHelper.GetConfigString("VE");
            string SID = ConfigHelper.GetConfigString("SID");
            builder.AppendFormat("<input type=\"hidden\" name=\"" + VE + "\" value=\"{0}\"/>", Utils.getstrVe());
            builder.AppendFormat("<input type=\"hidden\" name=\"" + SID + "\" value=\"{0}\"/>", Utils.getstrU());
            builder.Append("<input class=\"btn-red\" type=\"submit\" value=\"��һ��\"/>");
            builder.Append(Out.Tab("</div>", ""));
            builder.Append("</form>");

            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("HP3.aspx?act=info&amp;ptype=" + ptype + "") + "\">���ѡ��</a><br />");
            builder.Append(Out.Tab("</div>", ""));
            OutRule(ptype);
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<b>���ڿ���</b><br/>");
            builder.Append(Out.Tab("</div>", ""));
            //������¼
            try
            {
                int SizeNum = 5;
                string strWhere = " Fnum!='null'";
                DataSet dataHP3 = new BCW.HP3.BLL.HP3_kjnum().GetListHistory(strWhere, SizeNum);
                if (dataHP3.Tables[0].Rows.Count > 0)
                {
                    int i;
                    int n = 1;
                    for (i = 0; i < dataHP3.Tables[0].Rows.Count; i++)
                    {
                        object datenum = dataHP3.Tables[0].Rows[i][1];
                        object Fnum = dataHP3.Tables[0].Rows[i][3];
                        object Snum = dataHP3.Tables[0].Rows[i][4];
                        object Tnum = dataHP3.Tables[0].Rows[i][5];
                        string Winum = Convert.ToString(dataHP3.Tables[0].Rows[i][6]);
                        object DaTime = dataHP3.Tables[0].Rows[i][2];
                        builder.Append(Out.Tab("<div>", "<br />"));
                        builder.Append(datenum + "��:" + Fnum + Snum + Tnum);
                        string[] winums = Winum.Split(',');
                        if (winums[0].Trim() == "0")
                        {
                            builder.Append(" ɢ�� ");
                        }
                        else
                        {
                            string ts = Convert.ToString(winums[0].Trim());
                            switch (ts)
                            {
                                case "1":
                                case "2":
                                case "3":
                                case "4":
                                case "5":
                                    ts = "ͬ��";
                                    break;
                                case "6":
                                case "7":
                                case "8":
                                case "9":
                                case "10":
                                case "11":
                                case "12":
                                case "13":
                                case "14":
                                case "15":
                                case "16":
                                case "17":
                                case "18":
                                    ts = "˳��";
                                    break;
                                case "19":
                                case "20":
                                case "21":
                                case "22":
                                case "23":
                                    ts = "ͬ��˳";
                                    break;
                                case "24":
                                case "25":
                                case "26":
                                case "27":
                                case "28":
                                case "29":
                                case "30":
                                case "31":
                                case "32":
                                case "33":
                                case "34":
                                case "35":
                                case "36":
                                case "37":
                                    ts = "����";
                                    break;
                                case "38":
                                case "39":
                                case "40":
                                case "41":
                                case "42":
                                case "43":
                                case "44":
                                case "45":
                                case "46":
                                case "47":
                                case "48":
                                case "49":
                                case "50":
                                case "51":
                                    ts = "����";
                                    break;
                            }
                            builder.Append(" <b style=\"color:#8A7B66\">" + ts + "</b> ");
                        }
                        if (winums[1].Trim() == winums[2].Trim() && winums[2].Trim() == winums[3].Trim())
                        {
                        }
                        else
                        {
                            int weisu = Convert.ToInt32(winums[4].Trim());
                            if (weisu >= 5)
                            {
                                builder.Append("�� ");
                            }
                            else
                            {
                                builder.Append("С ");
                            }
                            if (weisu % 2 != 0)
                            {
                                builder.Append("��");
                            }
                            else
                            {
                                builder.Append("˫");
                            }
                        }
                        builder.Append(Out.Tab("</div>", ""));
                        n++;
                    }
                }
            }
            catch
            {
            }
            builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
            builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">��ҳ</a>-");
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">��Ϸ</a>-");
            builder.Append("<a href=\"" + Utils.getUrl("HP3.aspx") + "\">" + GameName + "</a>");
            builder.Append(Out.Tab("</div>", ""));
            #endregion
        }
    }
    //��ʾͶע����
    private void OutRule(int ptype)
    {
        switch (ptype)
        {
            case 1:
                //����
                builder.Append(Out.Tab("<div>", "<br />"));
                builder.Append("�淨��ʾ��<br />");
                builder.Append("�����ĺ��뻨ɫһ�£���Ͷע�Ļ�ɫ��֮��ͬ�����н�<b style=\"color:red\">" + XML(2) + "</b>����ͬ����ѡ���н�����<b style=\"color:red\">" + XML(1) + "</b>����<br />");
                builder.Append(Out.Tab("</div>", ""));
                break;
            case 2:
                //����
                builder.Append(Out.Tab("<div>", "<br />"));
                builder.Append("�淨��ʾ��<br />");
                builder.Append("���������ţ���Ͷע�ĺ�����֮��ͬ�����н�<b style=\"color:red\">" + XML(6) + "</b>����˳�Ӱ�ѡ���н�����<b style=\"color:red\">" + XML(5) + "</b>����<br /> ");
                builder.Append(Out.Tab("</div>", ""));
                break;
            case 3:
                //����
                builder.Append(Out.Tab("<div>", "<br />"));
                builder.Append("�淨��ʾ��<br />");
                builder.Append("����ͬ�������ţ���Ͷע�Ļ�ɫ��֮��ͬ�����н�<b style=\"color:red\">" + XML(4) + "</b>����ͬ��˳��ѡ���н�����<b style=\"color:red\">" + XML(3) + "</b>����<br />");
                builder.Append(Out.Tab("</div>", ""));
                break;
            case 4:
                builder.Append(Out.Tab("<div>", "<br />"));
                //����
                builder.Append("�淨��ʾ��<br />");
                builder.Append("������ͬ�ţ���Ͷע�ĺ�����֮��ͬ�����н�<b style=\"color:red\">" + XML(8) + "</b>�������Ӱ�ѡ���н�����<b style=\"color:red\">" + XML(7) + "</b>����");
                builder.Append(Out.Tab("</div>", ""));
                break;
            case 5:
                builder.Append(Out.Tab("<div>", "<br />"));
                //����
                builder.Append("�淨��ʾ��<br />");
                builder.Append("������ͬ��(��һ���벻ͬ)����Ͷע�ĺ�����֮��ͬ�����н�" + XML(10) + "�������Ӱ�ѡ���н�����" + XML(9) + "����");
                builder.Append(Out.Tab("</div>", ""));
                break;
            case 6:
                builder.Append(Out.Tab("<div>", "<br />"));
                builder.Append("�淨��ʾ��<br />");
                builder.Append("����ѡ��1������Ϊ1ע����עѡ���뿪������������1����ͬ���н�" + XML(11) + "����");
                builder.Append(Out.Tab("</div>", ""));
                break;
            case 7:
                builder.Append(Out.Tab("<div>", "<br />"));
                builder.Append("�淨��ʾ��<br />");
                builder.Append("����ѡ��2������Ϊ1ע����עѡ���뿪������������2����ͬ���н�" + XML(12) + "����");
                builder.Append(Out.Tab("</div>", ""));
                break;
            case 8:
                builder.Append(Out.Tab("<div>", "<br />"));
                builder.Append("�淨��ʾ��<br />");
                builder.Append("����Ϊ����Ϊ�س��ĺ��� ��ѡ��1��������Ϊ����Ϊ���ܳ��ֵĺ�������ѡ��2������,���ѡ��12�����������벻����ͬ�������������3���������ע��Ϊ2������������н����ͷ�ʽͬ��ѡ��ֱѡ��");
                builder.Append(Out.Tab("</div>", ""));
                break;
            case 9:
                builder.Append(Out.Tab("<div>", "<br />"));
                builder.Append("�淨��ʾ��<br />");
                builder.Append("����ѡ��3������Ϊ1ע����עѡ�Ű����������������к��뼴�н�" + XML(13) + "����");
                builder.Append(Out.Tab("</div>", ""));
                break;
            case 10:
                builder.Append(Out.Tab("<div>", "<br />"));
                builder.Append("�淨��ʾ��<br />");
                builder.Append("����Ϊ����Ϊ�س��ĺ����ѡ��1��2��������Ϊ����Ϊ���ܳ��ֵĺ�������ѡ��2������,���ѡ��12�����������벻����ͬ�������������4���������ע��Ϊ3������������н����ͷ�ʽͬ��ѡ��ֱѡ��");
                builder.Append(Out.Tab("</div>", ""));
                break;
            case 11:
                builder.Append(Out.Tab("<div>", "<br />"));
                builder.Append("�淨��ʾ��<br />");
                builder.Append("����ѡ��4������Ϊ1ע����עѡ�Ű����������������к��뼴�н�" + XML(14) + "����");
                builder.Append(Out.Tab("</div>", ""));
                break;
            case 12:
                builder.Append(Out.Tab("<div>", "<br />"));
                builder.Append("�淨��ʾ��<br />");
                builder.Append("����Ϊ����Ϊ�س��ĺ��� ��ѡ��1��3��������Ϊ����Ϊ���ܳ��ֵĺ�������ѡ��2������,���ѡ��12�����������벻����ͬ�������������5���������ע��Ϊ4������������н����ͷ�ʽͬ��ѡ��ֱѡ��");
                builder.Append(Out.Tab("</div>", ""));
                break;
            case 13:
                builder.Append(Out.Tab("<div>", "<br />"));
                builder.Append("�淨��ʾ��<br />");
                builder.Append("����ѡ��5������Ϊ1ע����עѡ�Ű����������������к��뼴�н�" + XML(15) + "����");
                builder.Append(Out.Tab("</div>", ""));
                break;
            case 14:
                builder.Append(Out.Tab("<div>", "<br />"));
                builder.Append("�淨��ʾ��<br />");
                builder.Append("����Ϊ����Ϊ�س��ĺ��� ��ѡ��1��4��������Ϊ����Ϊ���ܳ��ֵĺ�������ѡ��2������,���ѡ��12�����������벻����ͬ�������������6���������ע��Ϊ5������������н����ͷ�ʽͬ��ѡ��ֱѡ��");
                builder.Append(Out.Tab("</div>", ""));
                break;
            case 15:
                builder.Append(Out.Tab("<div>", "<br />"));
                builder.Append("�淨��ʾ��<br />");
                builder.Append("����ѡ��6������Ϊ1ע����עѡ�Ű����������������к��뼴�н�" + XML(16) + "����");
                builder.Append(Out.Tab("</div>", ""));
                break;
            case 16:
                builder.Append(Out.Tab("<div>", "<br />"));
                builder.Append("�淨��ʾ��<br />");
                builder.Append("����Ϊ����Ϊ�س��ĺ��� ��ѡ��1��5��������Ϊ����Ϊ���ܳ��ֵĺ��� ����ѡ��2������,���ѡ��12�����������벻����ͬ�������������7���������ע��Ϊ6������������н����ͷ�ʽͬ��ѡ��ֱѡ��");
                builder.Append(Out.Tab("</div>", ""));
                break;
            case 17:
                builder.Append(Out.Tab("<div>", "<br />"));
                builder.Append("�淨��ʾ��<br />");
                builder.Append("Ѻע�����Ƶĺ����ֵβ����С��С��ָ0��1��2��3��4������ָ5��6��7��8��9��(AΪ1��10��J��Q��KΪ0,��������,��Ѻ���ѺС����!)");
                builder.Append(Out.Tab("</div>", ""));
                break;
            case 18:
                builder.Append(Out.Tab("<div>", "<br />"));
                builder.Append("�淨��ʾ��<br />");
                builder.Append("Ѻע�����Ƶĺ����ֵβ����˫��˫��ָ0��2��4��6��8������ָ1��3��5��7��9��(AΪ1��10��J��Q��KΪ0,��������,��Ѻ����Ѻ˫����!)");
                builder.Append(Out.Tab("</div>", ""));
                break;
        }
    }
    //����ʱ���
    private void djs(string divname, DateTime datetime)
    {
        string daojishi = new BCW.JS.somejs().newDaojishi(divname, datetime);
        builder.Append("���� " + daojishi);
    }
    //֧��ҳ��
    private void PayPage()
    {
        if (SWB == 0)
        {
            #region ��ʽ��
            string GameName = ub.GetSub("HP3Name", xmlPath);
            int meid = new BCW.User.Users().GetUsId();
            int buytype = 0;
            if (meid == 0)
                Utils.Login();
            int ptype = Utils.ParseInt(Utils.GetRequest("ptype", "all", 2, @"^[1-9]$|^10$|^11$|^12$|^13$|^14$|^15$|^16$|^17$|^18$", "����ѡ�����"));
            string info = Utils.GetRequest("info", "all", 1, "", "");
            BCW.HP3.Model.HP3_kjnum model = new BCW.HP3.Model.HP3_kjnum();
            BCW.HP3.Model.HP3_kjnum model2 = new BCW.HP3.Model.HP3_kjnum();
            model = new BCW.HP3.BLL.HP3_kjnum().GetListLastNull();
            model2 = new BCW.HP3.BLL.HP3_kjnum().GetListLast();
            int dnu = int.Parse(model2.datenum);
            Master.Title = "��Ʊ��ע";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">��Ϸ����</a>&gt;<a href=\"" + Utils.getUrl("HP3.aspx") + "\">" + GameName + "</a>&gt;Ͷע");
            builder.Append(Out.Tab("</div>", "<br />"));
            int Sec = Utils.ParseInt(ub.GetSub("HP3Sec", xmlPath));
            String Num1 = Utils.GetRequest("Num1", "all", 1, @"^[\d((,)\d)?]+$", "");
            String Num2 = Utils.GetRequest("Num2", "all", 1, @"^[A-Z\d((,)A-Z\d)?]+$", "");
            String Num3 = Utils.GetRequest("Num3", "all", 1, @"^[A-Z\d((,)A-Z\d)?]+$", "");
            String Num4 = Utils.GetRequest("Num4", "all", 1, @"^[A-Z\d((,)A-Z\d)?]+$", "");
            String Num5 = Utils.GetRequest("Num5", "all", 1, @"^[A-Z\d((,)A-Z\d)?]+$", "");
            builder.Append(Out.Tab("<div>", ""));
            model2.Fnum = model2.Fnum.Trim();
            if (model2.Fnum != "null")
            {
                dnu = int.Parse(model.datenum) + 1;
            }
            int end2 = int.Parse(GetLastStr(model2.datenum, 2));
            if (DateTime.Now.TimeOfDay >= Convert.ToDateTime("22:00").TimeOfDay && end2 == 79)
            {
                dnu = Convert.ToInt32(DateTime.Now.AddDays(1).Date.ToString("yyyyMMdd") + "01");
            }
            else if (DateTime.Now.TimeOfDay <= Convert.ToDateTime("08:50").TimeOfDay && end2 == 79)
            {
                dnu = Convert.ToInt32(DateTime.Now.Date.ToString("yyyyMMdd") + "01");
            }


            if (GetLastStr(dnu.ToString(), 2) == "01")
            {
                if (DateTime.Now.TimeOfDay > Convert.ToDateTime("09:00").TimeOfDay && DateTime.Now.TimeOfDay < Convert.ToDateTime("22:00").TimeOfDay)
                {
                    Utils.Error("��" + dnu + "���ѽ�ֹ��ע,�ȴ�����...", Utils.getUrl("HP3.aspx"));
                }
            }
            else
            {
                if (model2.Fnum != "null")
                {
                    if (model.datetime.AddMinutes(10) < DateTime.Now.AddSeconds(Sec))
                    {
                        Utils.Error("��" + dnu + "���ѽ�ֹ��ע,�ȴ�����...", Utils.getUrl("HP3.aspx"));
                    }
                }
                else
                {
                    if (model2.datetime < DateTime.Now.AddSeconds(Sec))
                    {
                        Utils.Error("��" + dnu + "���ѽ�ֹ��ע,�ȴ�����...", Utils.getUrl("HP3.aspx"));
                    }
                }
            }

            builder.Append("��" + dnu + " ��");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div >", ""));
            if (GetLastStr(dnu.ToString(), 2) == "01")
            {
                if (DateTime.Now.TimeOfDay >= Convert.ToDateTime("22:00").TimeOfDay)
                {
                    djs("div1", Convert.ToDateTime("9:00").AddDays(1).AddSeconds(-Sec));
                }
                else
                {
                    djs("div1", Convert.ToDateTime("9:00").AddSeconds(-Sec));
                }
            }
            else
            {
                if (model2.Fnum != "null")
                {
                    djs("div1", model.datetime.AddSeconds(-Sec));
                }
                else
                {
                    djs("div1", model2.datetime.AddSeconds(-Sec));
                }
            }
            builder.Append("��ֹ<br />");
            builder.Append(Out.Tab("</div>", ""));
            string choose1 = Num1;
            string mynum = "";
            if (ptype == 1 || ptype == 2 || ptype == 3 || ptype == 4 || ptype == 5)
            {
                if (Num1 == "")
                {
                    Utils.Error("��ѡ������Ͷעѡ��", "");
                }
                buytype = 1;
                builder.Append(Out.Tab("<div class=\"text\">", ""));
                builder.Append("���ͣ�");
                switch (ptype)
                {
                    case 1:
                        builder.Append("ͬ��Ͷע");
                        break;
                    case 2:
                        builder.Append("˳��Ͷע");
                        break;
                    case 3:
                        builder.Append("ͬ��˳Ͷע");
                        break;
                    case 4:
                        builder.Append("����Ͷע");
                        break;
                    case 5:
                        builder.Append("����Ͷע");
                        break;
                }
                builder.Append("<br/>��ѡ���ˣ�");
                switch (choose1)
                {
                    case "1":
                        builder.Append(speChoose(1));
                        break;
                    case "2":
                        builder.Append(speChoose(2));
                        break;
                    case "3":
                        builder.Append(speChoose(3));
                        break;
                    case "4":
                        builder.Append(speChoose(4));
                        break;
                    case "5":
                        builder.Append(speChoose(5));
                        break;
                    case "6":
                        builder.Append(speChoose(6));
                        break;
                    case "7":
                        builder.Append(speChoose(7));
                        break;
                    case "8":
                        builder.Append(speChoose(8));
                        break;
                    case "9":
                        builder.Append(speChoose(9));
                        break;
                    case "10":
                        builder.Append(speChoose(10));
                        break;
                    case "11":
                        builder.Append(speChoose(11));
                        break;
                    case "12":
                        builder.Append(speChoose(12));
                        break;
                    case "13":
                        builder.Append(speChoose(13));
                        break;
                    case "14":
                        builder.Append(speChoose(14));
                        break;
                    case "15":
                        builder.Append(speChoose(15));
                        break;
                    case "16":
                        builder.Append(speChoose(16));
                        break;
                    case "17":
                        builder.Append(speChoose(17));
                        break;
                    case "18":
                        builder.Append(speChoose(18));
                        break;
                    case "19":
                        builder.Append(speChoose(19));
                        break;
                    case "20":
                        builder.Append(speChoose(20));
                        break;
                    case "21":
                        builder.Append(speChoose(21));
                        break;
                    case "22":
                        builder.Append(speChoose(22));
                        break;
                    case "23":
                        builder.Append(speChoose(23));
                        break;
                    case "24":
                        builder.Append(speChoose(24));
                        break;
                    case "25":
                        builder.Append(speChoose(25));
                        break;
                    case "26":
                        builder.Append(speChoose(26));
                        break;
                    case "27":
                        builder.Append(speChoose(27));
                        break;
                    case "28":
                        builder.Append(speChoose(28));
                        break;
                    case "29":
                        builder.Append(speChoose(29));
                        break;
                    case "30":
                        builder.Append(speChoose(30));
                        break;
                    case "31":
                        builder.Append(speChoose(31));
                        break;
                    case "32":
                        builder.Append(speChoose(32));
                        break;
                    case "33":
                        builder.Append(speChoose(33));
                        break;
                    case "34":
                        builder.Append(speChoose(34));
                        break;
                    case "35":
                        builder.Append(speChoose(35));
                        break;
                    case "36":
                        builder.Append(speChoose(36));
                        break;
                    case "37":
                        builder.Append(speChoose(37));
                        break;
                    case "38":
                        builder.Append(speChoose(38));
                        break;
                    case "39":
                        builder.Append(speChoose(39));
                        break;
                    case "40":
                        builder.Append(speChoose(40));
                        break;
                    case "41":
                        builder.Append(speChoose(41));
                        break;
                    case "42":
                        builder.Append(speChoose(42));
                        break;
                    case "43":
                        builder.Append(speChoose(43));
                        break;
                    case "44":
                        builder.Append(speChoose(44));
                        break;
                    case "45":
                        builder.Append(speChoose(45));
                        break;
                    case "46":
                        builder.Append(speChoose(46));
                        break;
                    case "47":
                        builder.Append(speChoose(47));
                        break;
                    case "48":
                        builder.Append(speChoose(48));
                        break;
                    case "49":
                        builder.Append(speChoose(49));
                        break;
                    case "50":
                        builder.Append(speChoose(50));
                        break;
                    case "51":
                        builder.Append(speChoose(51));
                        break;

                }
                builder.Append(Out.Tab("</div>", ""));
                mynum = Num1;
            }
            else if (ptype == 17 || ptype == 18)
            {
                buytype = 17;
                if (Num5 == "")
                {
                    Utils.Error("��ѡ������Ͷעѡ��", "");
                }
                DataSet dada = new BCW.HP3.BLL.HP3Buy().GetDaXiao(dnu.ToString(), "1");
                DataSet xiaoxiao = new BCW.HP3.BLL.HP3Buy().GetDaXiao(dnu.ToString(), "2");
                DataSet dandan = new BCW.HP3.BLL.HP3Buy().GetDaXiao(dnu.ToString(), "3");
                DataSet shuangshuang = new BCW.HP3.BLL.HP3Buy().GetDaXiao(dnu.ToString(), "4");
                int das = 0;
                int xiaos = 0;
                int dans = 0;
                int shuangs = 0;
                try
                {
                    das = Convert.ToInt32(dada.Tables[0].Rows[0][0]);
                }
                catch
                {
                    das = 0;
                }
                try
                {
                    xiaos = Convert.ToInt32(xiaoxiao.Tables[0].Rows[0][0]);
                }
                catch
                {
                    xiaos = 0;
                }
                try
                {
                    dans = Convert.ToInt32(dandan.Tables[0].Rows[0][0]);
                }
                catch
                {
                    dans = 0;
                }
                try
                {
                    shuangs = Convert.ToInt32(shuangshuang.Tables[0].Rows[0][0]);
                }
                catch
                {
                    shuangs = 0;
                }
                //int cazhi = Convert.ToInt32(XML(25));
                //if (Num5 == "1" && das - xiaos >= cazhi)
                //{
                //    Utils.Error("���Ѵﵽ���Ͷע�޶��ͶС����������ѡ��лл��", "");
                //}
                //if (Num5 == "2" && xiaos - das >= cazhi)
                //{
                //    Utils.Error("С�Ѵﵽ���Ͷע�޶��Ͷ�����������ѡ��лл��", "");
                //}
                //if (Num5 == "3" && dans - shuangs >= cazhi)
                //{
                //    Utils.Error("���Ѵﵽ���Ͷע�޶��Ͷ˫����������ѡ��лл��", "");
                //}
                //if (Num5 == "4" && shuangs - dans >= cazhi)
                //{
                //    Utils.Error("˫�Ѵﵽ���Ͷע�޶��Ͷ������������ѡ��лл��", "");
                //}
                builder.Append(Out.Tab("<div class=\"text\">", ""));
                builder.Append("���ͣ�");
                switch (ptype)
                {
                    case 17:
                        builder.Append("��СͶע");
                        break;
                    case 18:
                        builder.Append("��˫Ͷע");
                        break;
                }
                builder.Append("<br/>��Ѻ�ˣ�");
                switch (Num5)
                {
                    case "1":
                        builder.Append("<b>��</b>");
                        break;
                    case "2":
                        builder.Append("<b>С</b>");
                        break;
                    case "3":
                        builder.Append("<b>��</b>");
                        break;
                    case "4":
                        builder.Append("<b>˫</b>");
                        break;
                }
                builder.Append(Out.Tab("</div>", ""));
                mynum = Num5;
            }
            else if (ptype == 6 || ptype == 7 || ptype == 9 || ptype == 11 || ptype == 13 || ptype == 15)
            {
                buytype = ptype;
                int len = Num2.Length;
                if (Num2 == "")
                {
                    Utils.Error("��ѡ������Ͷעѡ��", "");
                }

                builder.Append(Out.Tab("<div class=\"text\">", ""));
                builder.Append("���ͣ�");
                switch (ptype)
                {
                    case 6:
                        builder.Append("��ѡһֱѡ");
                        break;
                    case 7:
                        builder.Append("��ѡ��ֱѡ");
                        break;
                    case 9:
                        builder.Append("��ѡ��ֱѡ");
                        break;
                    case 11:
                        builder.Append("��ѡ��ֱѡ");
                        break;
                    case 13:
                        builder.Append("��ѡ��ֱѡ");
                        break;
                    case 15:
                        builder.Append("��ѡ��ֱѡ");
                        break;
                }
                if (ptype == 7 && len < 3)
                {
                    Utils.Error("������ѡ��2������", "");
                }
                else if (ptype == 9 && len < 5)
                {
                    Utils.Error("������ѡ��3������", "");
                }
                else if (ptype == 11 && len < 7)
                {
                    Utils.Error("������ѡ��4������", "");
                }
                else if (ptype == 13 && len < 9)
                {
                    Utils.Error("������ѡ��5������", "");
                }
                else if (ptype == 15 && len < 11)
                {
                    Utils.Error("������ѡ��6������", "");
                }
                string num2 = Num2.Replace("0", "10");
                builder.Append("<br/>��ѡ���ˣ�");
                builder.Append("<b>" + num2 + "</b>");
                builder.Append(Out.Tab("</div>", ""));
                mynum = num2;

            }
            else if (ptype == 8 || ptype == 10 || ptype == 12 || ptype == 14 || ptype == 16)
            {
                buytype = ptype;
                int len1 = Num3.Length;
                int len2 = Num4.Length;
                if (Num3 == "" || Num4 == "")
                {
                    Utils.Error("��ѡ������Ͷעѡ��", "");
                }

                builder.Append(Out.Tab("<div class=\"text\">", ""));
                builder.Append("���ͣ�");
                switch (ptype)
                {
                    case 8:
                        builder.Append("��ѡ������");
                        break;
                    case 10:
                        builder.Append("��ѡ������");
                        break;
                    case 12:
                        builder.Append("��ѡ�ĵ���");
                        break;
                    case 14:
                        builder.Append("��ѡ�嵨��");
                        break;
                    case 16:
                        builder.Append("��ѡ������");
                        break;
                }
                string[] str3 = Num3.Split(',');
                string[] str4 = Num4.Split(',');
                int zyr = (len1 + 1) / 2;
                for (int i = 0; i < zyr; )
                {
                    bool id = ((IList)str4).Contains(str3[i]);
                    i++;
                    if (id)
                        Utils.Error("��������벻����ͬ", "");
                }
                if (ptype == 8)
                {
                    if (len1 != 1)
                        Utils.Error("���ѡ��1������", "");
                    if (len2 < 3)
                        Utils.Error("��������ѡ����ע����", "");
                }
                else if (ptype == 10)
                {
                    if (len1 > 3)
                        Utils.Error("���ѡ��2������", "");
                    if (len1 == 1 && len2 < 5)
                        Utils.Error("��������ѡ����ע����", "");
                    if (len1 == 2 && len2 < 3)
                        Utils.Error("��������ѡ����ע����", "");
                }
                else if (ptype == 12)
                {
                    if (len1 > 5)
                        Utils.Error("���ѡ��3������", "");
                    if (len1 == 1 && len2 < 7)
                        Utils.Error("��������ѡ����ע����", "");
                    if (len1 == 3 && len2 < 5)
                        Utils.Error("��������ѡ����ע����", "");
                    if (len1 == 5 && len2 < 3)
                        Utils.Error("��������ѡ����ע����", "");
                }
                else if (ptype == 14)
                {
                    if (len1 > 7)
                        Utils.Error("���ѡ��4������", "");
                    if (len1 == 1 && len2 < 9)
                        Utils.Error("��������ѡ����ע����", "");
                    if (len1 == 3 && len2 < 7)
                        Utils.Error("��������ѡ����ע����", "");
                    if (len1 == 5 && len2 < 5)
                        Utils.Error("��������ѡ����ע����", "");
                    if (len1 == 7 && len2 < 3)
                        Utils.Error("��������ѡ����ע����", "");

                }
                else if (ptype == 16)
                {
                    if (len1 > 9)
                        Utils.Error("���ѡ��5������", "");
                    if (len1 == 1 && len2 < 11)
                        Utils.Error("��������ѡ����ע����", "");
                    if (len1 == 3 && len2 < 9)
                        Utils.Error("��������ѡ����ע����", "");
                    if (len1 == 5 && len2 < 7)
                        Utils.Error("��������ѡ����ע����", "");
                    if (len1 == 7 && len2 < 5)
                        Utils.Error("��������ѡ����ע����", "");
                    if (len1 == 9 && len2 < 3)
                        Utils.Error("��������ѡ����ע����", "");
                }
                string num3 = Num3.Replace("0", "10");
                string num4 = Num4.Replace("0", "10");
                builder.Append("<br/>��ѡ���ˣ�");
                builder.Append("<b>" + num3 + "#" + num4 + "</b>");
                builder.Append(Out.Tab("</div>", ""));
                string s = num3 + "#";
                mynum = s + num4;
            }
            int zhu = 1;
            if (ptype < 6 || ptype == 17 || ptype == 18)
            {
                zhu = 1;
            }
            else
            {
                if (ptype == 6)//ֱ1
                {
                    int len = Num2.Length;
                    zhu = (len + 1) / 2;
                }
                else if (ptype == 7)//ֱ2
                {
                    int len = Num2.Length;
                    int Anum = (len + 1) / 2;
                    switch (Anum)
                    {
                        case 2:
                            zhu = 1;
                            break;
                        case 3:
                            zhu = 3;
                            break;
                        case 4:
                            zhu = 6;
                            break;
                        case 5:
                            zhu = 10;
                            break;
                        case 6:
                            zhu = 15;
                            break;
                        case 7:
                            zhu = 21;
                            break;
                        case 8:
                            zhu = 28;
                            break;
                        case 9:
                            zhu = 36;
                            break;
                        case 10:
                            zhu = 45;
                            break;
                        case 11:
                            zhu = 55;
                            break;
                        case 12:
                            zhu = 66;
                            break;
                        case 13:
                            zhu = 78;
                            break;
                    }
                }
                else if (ptype == 8)
                {
                    int len2 = Num4.Length;
                    zhu = (len2 + 1) / 2;
                }
                else if (ptype == 9)//ֱ3
                {
                    int len = Num2.Length;
                    int Anum = (len + 1) / 2;
                    switch (Anum)
                    {
                        case 3:
                            zhu = 1;
                            break;
                        case 4:
                            zhu = 4;
                            break;
                        case 5:
                            zhu = 10;
                            break;
                        case 6:
                            zhu = 20;
                            break;
                        case 7:
                            zhu = 35;
                            break;
                        case 8:
                            zhu = 56;
                            break;
                        case 9:
                            zhu = 84;
                            break;
                        case 10:
                            zhu = 120;
                            break;
                        case 11:
                            zhu = 165;
                            break;
                        case 12:
                            zhu = 220;
                            break;
                        case 13:
                            zhu = 286;
                            break;
                    }
                }
                else if (ptype == 10)
                {
                    int len1 = Num3.Length;
                    int Dan = (len1 + 1) / 2;
                    int len2 = Num4.Length;
                    int Tuo = (len2 + 1) / 2;
                    if (Dan == 1)
                    {
                        switch (Tuo)
                        {
                            case 3:
                                zhu = 3;
                                break;
                            case 4:
                                zhu = 6;
                                break;
                            case 5:
                                zhu = 10;
                                break;
                            case 6:
                                zhu = 15;
                                break;
                            case 7:
                                zhu = 21;
                                break;
                            case 8:
                                zhu = 28;
                                break;
                            case 9:
                                zhu = 36;
                                break;
                            case 10:
                                zhu = 45;
                                break;
                            case 11:
                                zhu = 55;
                                break;
                            case 12:
                                zhu = 66;
                                break;
                        }
                    }
                    else
                    {
                        switch (Tuo)
                        {
                            case 2:
                                zhu = 2;
                                break;
                            case 3:
                                zhu = 3;
                                break;
                            case 4:
                                zhu = 4;
                                break;
                            case 5:
                                zhu = 5;
                                break;
                            case 6:
                                zhu = 6;
                                break;
                            case 7:
                                zhu = 7;
                                break;
                            case 8:
                                zhu = 8;
                                break;
                            case 9:
                                zhu = 9;
                                break;
                            case 10:
                                zhu = 10;
                                break;
                            case 11:
                                zhu = 11;
                                break;
                        }
                    }

                }
                else if (ptype == 11)//ֱ4
                {
                    int len = Num2.Length;
                    int Anum = (len + 1) / 2;
                    switch (Anum)
                    {
                        case 4:
                            zhu = 1;
                            break;
                        case 5:
                            zhu = 5;
                            break;
                        case 6:
                            zhu = 15;
                            break;
                        case 7:
                            zhu = 35;
                            break;
                        case 8:
                            zhu = 70;
                            break;
                        case 9:
                            zhu = 126;
                            break;
                        case 10:
                            zhu = 210;
                            break;
                        case 11:
                            zhu = 330;
                            break;
                        case 12:
                            zhu = 495;
                            break;
                        case 13:
                            zhu = 715;
                            break;
                    }
                }
                else if (ptype == 12)
                {
                    int len1 = Num3.Length;
                    int Dan = (len1 + 1) / 2;
                    int len2 = Num4.Length;
                    int Tuo = (len2 + 1) / 2;
                    switch (Dan)
                    {
                        case 1:
                            switch (Tuo)
                            {
                                case 4:
                                    zhu = 4;
                                    break;
                                case 5:
                                    zhu = 10;
                                    break;
                                case 6:
                                    zhu = 20;
                                    break;
                                case 7:
                                    zhu = 35;
                                    break;
                                case 8:
                                    zhu = 56;
                                    break;
                                case 9:
                                    zhu = 84;
                                    break;
                                case 10:
                                    zhu = 120;
                                    break;
                                case 11:
                                    zhu = 165;
                                    break;
                                case 12:
                                    zhu = 220;
                                    break;
                            }
                            break;
                        case 2:
                            switch (Tuo)
                            {
                                case 3:
                                    zhu = 3;
                                    break;
                                case 4:
                                    zhu = 6;
                                    break;
                                case 5:
                                    zhu = 10;
                                    break;
                                case 6:
                                    zhu = 15;
                                    break;
                                case 7:
                                    zhu = 21;
                                    break;
                                case 8:
                                    zhu = 28;
                                    break;
                                case 9:
                                    zhu = 36;
                                    break;
                                case 10:
                                    zhu = 45;
                                    break;
                                case 11:
                                    zhu = 55;
                                    break;
                            }
                            break;
                        case 3:
                            zhu = Tuo;
                            break;
                    }
                }
                else if (ptype == 13)//ֱ5
                {
                    int len = Num2.Length;
                    int Anum = (len + 1) / 2;
                    switch (Anum)
                    {
                        case 5:
                            zhu = 1;
                            break;
                        case 6:
                            zhu = 6;
                            break;
                        case 7:
                            zhu = 21;
                            break;
                        case 8:
                            zhu = 56;
                            break;
                        case 9:
                            zhu = 126;
                            break;
                        case 10:
                            zhu = 252;
                            break;
                        case 11:
                            zhu = 462;
                            break;
                        case 12:
                            zhu = 792;
                            break;
                        case 13:
                            zhu = 1287;
                            break;
                    }
                }
                else if (ptype == 14)
                {
                    int len1 = Num3.Length;
                    int Dan = (len1 + 1) / 2;
                    int len2 = Num4.Length;
                    int Tuo = (len2 + 1) / 2;
                    switch (Dan)
                    {
                        case 1:
                            switch (Tuo)
                            {
                                case 5:
                                    zhu = 5;
                                    break;
                                case 6:
                                    zhu = 15;
                                    break;
                                case 7:
                                    zhu = 35;
                                    break;
                                case 8:
                                    zhu = 70;
                                    break;
                                case 9:
                                    zhu = 126;
                                    break;
                                case 10:
                                    zhu = 210;
                                    break;
                                case 11:
                                    zhu = 330;
                                    break;
                                case 12:
                                    zhu = 495;
                                    break;
                            }
                            break;
                        case 2:
                            switch (Tuo)
                            {
                                case 4:
                                    zhu = 4;
                                    break;
                                case 5:
                                    zhu = 10;
                                    break;
                                case 6:
                                    zhu = 20;
                                    break;
                                case 7:
                                    zhu = 35;
                                    break;
                                case 8:
                                    zhu = 56;
                                    break;
                                case 9:
                                    zhu = 84;
                                    break;
                                case 10:
                                    zhu = 120;
                                    break;
                                case 11:
                                    zhu = 165;
                                    break;
                            }
                            break;
                        case 3:
                            switch (Tuo)
                            {
                                case 3:
                                    zhu = 3;
                                    break;
                                case 4:
                                    zhu = 6;
                                    break;
                                case 5:
                                    zhu = 10;
                                    break;
                                case 6:
                                    zhu = 15;
                                    break;
                                case 7:
                                    zhu = 21;
                                    break;
                                case 8:
                                    zhu = 28;
                                    break;
                                case 9:
                                    zhu = 36;
                                    break;
                                case 10:
                                    zhu = 45;
                                    break;
                            }
                            break;
                        case 4:
                            zhu = Tuo;
                            break;
                    }
                }
                else if (ptype == 15)//ֱ6
                {
                    int len = Num2.Length;
                    int Anum = (len + 1) / 2;
                    switch (Anum)
                    {

                        case 6:
                            zhu = 1;
                            break;
                        case 7:
                            zhu = 7;
                            break;
                        case 8:
                            zhu = 28;
                            break;
                        case 9:
                            zhu = 84;
                            break;
                        case 10:
                            zhu = 210;
                            break;
                        case 11:
                            zhu = 462;
                            break;
                        case 12:
                            zhu = 924;
                            break;
                        case 13:
                            zhu = 1716;
                            break;
                    }
                }
                else if (ptype == 16)
                {
                    int len1 = Num3.Length;
                    int Dan = (len1 + 1) / 2;
                    int len2 = Num4.Length;
                    int Tuo = (len2 + 1) / 2;
                    switch (Dan)
                    {
                        case 1:
                            switch (Tuo)
                            {
                                case 6:
                                    zhu = 6;
                                    break;
                                case 7:
                                    zhu = 21;
                                    break;
                                case 8:
                                    zhu = 56;
                                    break;
                                case 9:
                                    zhu = 126;
                                    break;
                                case 10:
                                    zhu = 252;
                                    break;
                                case 11:
                                    zhu = 462;
                                    break;
                                case 12:
                                    zhu = 792;
                                    break;

                            }
                            break;
                        case 2:
                            switch (Tuo)
                            {
                                case 5:
                                    zhu = 5;
                                    break;
                                case 6:
                                    zhu = 15;
                                    break;
                                case 7:
                                    zhu = 35;
                                    break;
                                case 8:
                                    zhu = 70;
                                    break;
                                case 9:
                                    zhu = 126;
                                    break;
                                case 10:
                                    zhu = 210;
                                    break;
                                case 11:
                                    zhu = 330;
                                    break;
                            }
                            break;
                        case 3:
                            switch (Tuo)
                            {
                                case 4:
                                    zhu = 4;
                                    break;
                                case 5:
                                    zhu = 10;
                                    break;
                                case 6:
                                    zhu = 20;
                                    break;
                                case 7:
                                    zhu = 35;
                                    break;
                                case 8:
                                    zhu = 56;
                                    break;
                                case 9:
                                    zhu = 84;
                                    break;
                                case 10:
                                    zhu = 120;
                                    break;
                            }
                            break;
                        case 4:
                            switch (Tuo)
                            {
                                case 3:
                                    zhu = 3;
                                    break;
                                case 4:
                                    zhu = 6;
                                    break;
                                case 5:
                                    zhu = 10;
                                    break;
                                case 6:
                                    zhu = 15;
                                    break;
                                case 7:
                                    zhu = 21;
                                    break;
                                case 8:
                                    zhu = 28;
                                    break;
                                case 9:
                                    zhu = 36;
                                    break;

                            }
                            break;
                        case 5:
                            zhu = Tuo;
                            break;
                    }
                }
            }

            if (info == "ok2")
            {
                long Price = Utils.ParseInt64(Utils.GetRequest("Price", "post", 2, @"^[1-9]\d*$", "Ͷע����д����"));
                //֧����ȫ��ʾ
                string[] p_pageArr = { "Price", "mynum", "Num1", "Num2", "Num3", "Num4", "Num5", "ptype", "act", "info" };
                BCW.User.PaySafe.PaySafePage(meid, Utils.getPageUrl(), p_pageArr);

                long gold = new BCW.BLL.User().GetGold(meid);
                long prices = Convert.ToInt64(Price * zhu);
                //�Ƿ�ˢ��
                long small = Convert.ToInt64(ub.GetSub("HP3SmallPay", xmlPath));
                long big = Convert.ToInt64(ub.GetSub("HP3BigPay", xmlPath));
                string appName = "LIGHT_HP3";
                int Expir = Utils.ParseInt(ub.GetSub("HP3Expir", xmlPath));
                BCW.User.Users.IsFresh(appName, Expir, Price, small, big);

                if (gold < prices)
                {
                    Utils.Error("����" + ub.Get("SiteBz") + "����", "");
                }

                long xPrices = Utils.ParseInt64(ub.GetSub("HP3Price", xmlPath));
                if (xPrices > 0)
                {
                    DataSet ds = new BCW.HP3.BLL.HP3Buy().GetListByID("BuyMoney", meid, dnu.ToString());
                    int oPrices = 0;
                    DataTable dt = ds.Tables[0];
                    foreach (DataRow dr in dt.Rows)
                    {
                        int drs = int.Parse(dr[0].ToString());
                        oPrices = oPrices + drs;
                    }
                    if (oPrices + prices > xPrices)
                    {
                        if (oPrices >= xPrices)
                            Utils.Error("��������ע�Ѵ����ޣ���ȴ�����...", "");
                        else
                            Utils.Error("��������໹������ע" + (xPrices - oPrices) + "" + ub.Get("SiteBz") + "", "");
                    }
                }

                long xPricesc = 0;
                if (ptype == 1)
                {
                    xPricesc = Utils.ParseInt64(ub.GetSub("Oddsc1", xmlPath));
                }
                else if (ptype == 2)
                {
                    xPricesc = Utils.ParseInt64(ub.GetSub("Oddsc2", xmlPath));
                }
                else if (ptype == 3)
                {
                    xPricesc = Utils.ParseInt64(ub.GetSub("Oddsc3", xmlPath));
                }
                else if (ptype == 4)
                {
                    xPricesc = Utils.ParseInt64(ub.GetSub("Oddsc4", xmlPath));
                }
                else if (ptype == 5)
                {
                    xPricesc = Utils.ParseInt64(ub.GetSub("Oddsc5", xmlPath));
                }
                else if (ptype == 6)
                {
                    xPricesc = Utils.ParseInt64(ub.GetSub("Oddsc6", xmlPath));
                }
                else if (ptype == 7 || ptype == 8)
                {
                    xPricesc = Utils.ParseInt64(ub.GetSub("Oddsc7", xmlPath));
                }
                else if (ptype == 9 || ptype == 10)
                {
                    xPricesc = Utils.ParseInt64(ub.GetSub("Oddsc8", xmlPath));
                }
                else if (ptype == 11 || ptype == 12)
                {
                    xPricesc = Utils.ParseInt64(ub.GetSub("Oddsc9", xmlPath));
                }
                else if (ptype == 13 || ptype == 14)
                {
                    xPricesc = Utils.ParseInt64(ub.GetSub("Oddsc10", xmlPath));
                }
                else if (ptype == 15 || ptype == 16)
                {
                    xPricesc = Utils.ParseInt64(ub.GetSub("Oddsc11", xmlPath));
                }
                else if (ptype == 17)
                {
                    xPricesc = Utils.ParseInt64(ub.GetSub("Oddsc12", xmlPath));
                }
                else if (ptype == 18)
                {
                    xPricesc = Utils.ParseInt64(ub.GetSub("Oddsc13", xmlPath));
                }


                if (xPricesc > 0)
                {
                    long oPricesc = 0;
                    if (buytype > 1 && buytype < 17)
                    {
                        oPricesc = new BCW.HP3.BLL.HP3Buy().GetSumPricebyTypes(buytype, dnu);
                    }
                    else if (buytype == 17)
                    {
                        if (ptype == 17)
                        {
                            oPricesc = new BCW.HP3.BLL.HP3Buy().GetSumPricebyTypesda(17, dnu);
                        }
                        if (ptype == 18)
                        {
                            oPricesc = new BCW.HP3.BLL.HP3Buy().GetSumPricebyTypesdan(17, dnu);
                        }
                    }
                    else
                    {
                        if (ptype == 1)
                        {
                            oPricesc = new BCW.HP3.BLL.HP3Buy().GetSumPricebyTypes1(1, dnu);
                        }
                        if (ptype == 2)
                        {
                            oPricesc = new BCW.HP3.BLL.HP3Buy().GetSumPricebyTypes2(1, dnu);
                        }
                        if (ptype == 3)
                        {
                            oPricesc = new BCW.HP3.BLL.HP3Buy().GetSumPricebyTypes3(1, dnu);
                        }
                        if (ptype == 4)
                        {
                            oPricesc = new BCW.HP3.BLL.HP3Buy().GetSumPricebyTypes4(1, dnu);
                        }
                        if (ptype == 5)
                        {
                            oPricesc = new BCW.HP3.BLL.HP3Buy().GetSumPricebyTypes5(1, dnu);
                        }
                    }
                    if (oPricesc + prices > xPricesc)
                    {
                        if (oPricesc > xPricesc)
                            Utils.Error("����" + OutType(ptype) + "��ע�Ѵ����ޣ���ȴ����ڻ���ѡ������Ͷע...", "");
                        else
                            Utils.Error("����" + OutType(ptype) + "��໹������ע" + (xPricesc - oPricesc) + ub.Get("SiteBz") + "", "");
                    }
                }



                string mename = new BCW.BLL.User().GetUsName(meid);
                string buydate = dnu.ToString();
                string buynum = mynum;
                long buymoney = Price;

                BCW.HP3.Model.HP3Buy modelBuy = new BCW.HP3.Model.HP3Buy();
                modelBuy.BuyID = meid;
                modelBuy.BuyDate = buydate;
                modelBuy.BuyType = buytype;
                modelBuy.BuyNum = buynum;
                modelBuy.BuyMoney = buymoney;
                modelBuy.BuyZhu = zhu;
                modelBuy.BuyTime = DateTime.Now;
                modelBuy.Odds = Convert.ToDecimal(0);
                #region ���ʴ���
                if (modelBuy.BuyType == 1)
                {
                    switch (buynum)
                    {
                        case "1":
                        case "2":
                        case "3":
                        case "4":
                            modelBuy.Odds = Convert.ToDecimal(XML(2));
                            break;
                        case "5":
                            modelBuy.Odds = Convert.ToDecimal(XML(1));
                            break;
                        case "6":
                        case "7":
                        case "8":
                        case "9":
                        case "10":
                        case "11":
                        case "12":
                        case "13":
                        case "14":
                        case "15":
                        case "16":
                        case "17":
                            modelBuy.Odds = Convert.ToDecimal(XML(6));
                            break;
                        case "18":
                            modelBuy.Odds = Convert.ToDecimal(XML(5));
                            break;
                        case "19":
                        case "20":
                        case "21":
                        case "22":
                            modelBuy.Odds = Convert.ToDecimal(XML(4));
                            break;
                        case "23":
                            modelBuy.Odds = Convert.ToDecimal(XML(3));
                            break;
                        case "24":
                        case "25":
                        case "26":
                        case "27":
                        case "28":
                        case "29":
                        case "30":
                        case "31":
                        case "32":
                        case "33":
                        case "34":
                        case "35":
                        case "36":
                            modelBuy.Odds = Convert.ToDecimal(XML(8));
                            break;
                        case "37":
                            modelBuy.Odds = Convert.ToDecimal(XML(7));
                            break;
                        case "38":
                        case "39":
                        case "40":
                        case "41":
                        case "42":
                        case "43":
                        case "44":
                        case "45":
                        case "46":
                        case "47":
                        case "48":
                        case "49":
                        case "50":
                            modelBuy.Odds = Convert.ToDecimal(XML(10));
                            break;
                        case "51":
                            modelBuy.Odds = Convert.ToDecimal(XML(9));
                            break;
                    }

                }
                else if (modelBuy.BuyType == 6)
                {
                    modelBuy.Odds = Convert.ToDecimal(XML(11));
                }
                else if (modelBuy.BuyType == 7 || modelBuy.BuyType == 8)
                {
                    modelBuy.Odds = Convert.ToDecimal(XML(12));
                }
                else if (modelBuy.BuyType == 9 || modelBuy.BuyType == 10)
                {
                    modelBuy.Odds = Convert.ToDecimal(XML(13));
                }
                else if (modelBuy.BuyType == 11 || modelBuy.BuyType == 12)
                {
                    modelBuy.Odds = Convert.ToDecimal(XML(14));
                }
                else if (modelBuy.BuyType == 13 || modelBuy.BuyType == 14)
                {
                    modelBuy.Odds = Convert.ToDecimal(XML(15));
                }
                else if (modelBuy.BuyType == 15 || modelBuy.BuyType == 16)
                {
                    modelBuy.Odds = Convert.ToDecimal(XML(16));
                }
                else if (modelBuy.BuyType == 17)
                {
                    try
                    {
                        switch (modelBuy.BuyNum)
                        {
                            case "1":
                                modelBuy.Odds = Convert.ToDecimal(XML(21));
                                break;
                            case "2":
                                modelBuy.Odds = Convert.ToDecimal(XML(22));
                                break;
                            case "3":
                                modelBuy.Odds = Convert.ToDecimal(XML(23));
                                break;
                            case "4":
                                modelBuy.Odds = Convert.ToDecimal(XML(24));
                                break;
                        }
                    }
                    catch
                    {
                        switch (modelBuy.BuyNum)
                        {
                            case "1":
                                modelBuy.Odds = Convert.ToDecimal(XML(21));
                                break;
                            case "2":
                                modelBuy.Odds = Convert.ToDecimal(XML(22));
                                break;
                            case "3":
                                modelBuy.Odds = Convert.ToDecimal(XML(23));
                                break;
                            case "4":
                                modelBuy.Odds = Convert.ToDecimal(XML(24));
                                break;
                        }
                    }
                }
                #endregion

                int id = new BCW.HP3.BLL.HP3Buy().Add(modelBuy);
                new BCW.HP3.BLL.HP3Buy().UpdateWillGet(id, 0);
                string gameplay = "";
                switch (modelBuy.BuyType)
                {
                    case 1:
                        gameplay = "��ɫ����ͬ��Ͷע";
                        break;
                    case 17:
                        gameplay = "��С��˫Ͷע";
                        break;
                    default:
                        gameplay = "��ѡͶע";
                        break;
                }
                string xfjl = "";
                if (modelBuy.BuyType == 1)
                {
                    xfjl = speChoose(Convert.ToInt32(modelBuy.BuyNum));
                }
                else if (modelBuy.BuyType == 17)
                {
                    switch (modelBuy.BuyNum)
                    {
                        case "1":
                            xfjl = "��";
                            break;
                        case "2":
                            xfjl = "С";
                            break;
                        case "3":
                            xfjl = "��";
                            break;
                        case "4":
                            xfjl = "˫";
                            break;
                    }
                }
                else
                {
                    switch (modelBuy.BuyType)
                    {
                        case 6:
                            xfjl = "��ѡһ��" + modelBuy.BuyNum;
                            break;
                        case 7:
                            xfjl = "��ѡ����" + modelBuy.BuyNum;
                            break;
                        case 8:
                            xfjl = "��ѡ�����ϣ�" + modelBuy.BuyNum;
                            break;
                        case 9:
                            xfjl = "��ѡ����" + modelBuy.BuyNum;
                            break;
                        case 10:
                            xfjl = "��ѡ�����ϣ�" + modelBuy.BuyNum;
                            break;
                        case 11:
                            xfjl = "��ѡ�ģ�" + modelBuy.BuyNum;
                            break;
                        case 12:
                            xfjl = "��ѡ�ĵ��ϣ�" + modelBuy.BuyNum;
                            break;
                        case 13:
                            xfjl = "��ѡ�壺" + modelBuy.BuyNum;
                            break;
                        case 14:
                            xfjl = "��ѡ�嵨�ϣ�" + modelBuy.BuyNum;
                            break;
                        case 15:
                            xfjl = "��ѡ����" + modelBuy.BuyNum;
                            break;
                        case 16:
                            xfjl = "��ѡ�����ϣ�" + modelBuy.BuyNum;
                            break;
                    }

                }
                //��̬+���Ѽ�¼        BCW.HP3.Model.HP3Winner model = new BCW.HP3.BLL.HP3Winner().GetModel(pid);

                new BCW.BLL.User().UpdateiGold(meid, mename, -prices, "" + GameName + "��" + "[url=./game/HP3.aspx?act=BuyWin&qihaos=" + buydate + "&amp;ptype=2]" + buydate + "[/url]����" + xfjl + "|����" + modelBuy.Odds + "|ͶעID" + id);
                if (new BCW.BLL.User().GetIsSpier(meid) != 1)
                    new BCW.BLL.User().UpdateiGold(107, new BCW.BLL.User().GetUsName(107), prices, "ID:[url=forumlog.aspx?act=xview&amp;uid=" + meid + "]" + meid + "[/url]" + GameName + "��[url=./game/HP3.aspx?act=BuyWin&amp;ptype=2&amp;qihaos=" + buydate + "]" + buydate + "[/url]����" + xfjl + "��" + prices + ub.Get("SiteBz") + "-��ʶID" + id + "");
                string wText = "��[url=/bbs/game/HP3.aspx]" + GameName + "[/url]��" + gameplay + "����ע**" + "" + ub.Get("SiteBz") + "";//+ prices 
                new BCW.BLL.Action().Add(1003, id, meid, mename, wText);
                Utils.Success("��ע", "��ע�ɹ���������" + prices + "" + ub.Get("SiteBz") + "(��" + zhu + "ע)<br /><a href=\"" + Utils.getUrl("HP3.aspx?act=info&amp;ptype=" + ptype + "") + "\">&gt;������ע</a>", Utils.getUrl("HP3.aspx"), "5");

            }
            else if (info == "ok")
            {
                long Price = Utils.ParseInt64(Utils.GetRequest("Price", "post", 2, @"^[1-9]\d*$", "Ͷע����д����"));
                builder.Append(Out.Tab("</div>", "<br />"));
                builder.Append(Out.Tab("<div class=\"text\">", ""));
                builder.Append("ע����" + zhu + "ע<br />");
                builder.Append("ÿע��" + Price + "" + ub.Get("SiteBz") + "<br />");
                builder.Append("�軨�ѣ�" + (Price * zhu) + "" + ub.Get("SiteBz") + "<br />");
                long gold = new BCW.BLL.User().GetGold(meid);
                builder.Append("���Դ���" + Utils.ConvertGold(gold) + "" + ub.Get("SiteBz") + "<br />");
                builder.Append(Out.Tab("</div>", ""));
                string strName = "Price,Num5,Num4,Num3,Num2,Num1,ptype,act,info";
                string strValu = "" + Price + "'" + Num5 + "'" + Num4 + "'" + Num3 + "'" + Num2 + "'" + Num1 + "'" + ptype + "'pay'ok2";
                string strOthe = "ȷ��Ͷע,HP3.aspx,post,0,red";

                builder.Append(Out.wapform(strName, strValu, strOthe));
                builder.Append(Out.Tab("<div>", Out.Hr()));
                builder.Append("<a href=\"" + Utils.getUrl("HP3.aspx?act=info&amp;ptype=" + ptype + "") + "\">&lt;&lt;��������ѡ��</a>");
                builder.Append(Out.Tab("</div>", ""));
            }
            else
            {
                long Price = Utils.ParseInt64(Utils.GetRequest("Price", "all", 1, @"^[1-9]\d*$", "0"));
                builder.Append(Out.Tab("</div>", Out.LHr()));

                #region �����ע
                try
                {
                    builder.Append(Out.Tab("<div>", "<br />"));
                    builder.Append("�����ע<br />�N");
                    kuai(meid, 3, ptype, Num5, Num4, Num3, Num2, Num1);//�û�����Ϸ3����ע����,��ֵ5.4.3.2.1
                    builder.Append(Out.Tab("</div>", ""));
                }
                catch { }
                #endregion

                string strText = "ÿעͶע��(" + ub.GetSub("HP3SmallPay", xmlPath) + "-" + ub.GetSub("HP3BigPay", xmlPath) + "" + ub.Get("SiteBz") + "):/,,,,,,,,";
                string strName = "Price,Num5,Num4,Num3,Num2,Num1,ptype,act,info";
                string strType = "num,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden";
                string strValu = string.Empty;
                if (Price == 0)
                {
                    strValu = "" + "" + "'" + Num5 + "'" + Num4 + "'" + Num3 + "'" + Num2 + "'" + Num1 + "'" + ptype + "'pay'ok";
                }
                else
                {
                    strValu = "" + Price + "'" + Num5 + "'" + Num4 + "'" + Num3 + "'" + Num2 + "'" + Num1 + "'" + ptype + "'pay'ok";
                }
                string strEmpt = "true,false,false,false,false,false,false,false,false";
                string strIdea = "" + ub.Get("SiteBz") + "''''''''|/";
                string strOthe = "ȷ��Ͷע,HP3.aspx,post,1,red";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
                builder.Append(Out.Tab("<div>", Out.Hr()));
                builder.Append("<a href=\"" + Utils.getUrl("HP3.aspx?act=info&amp;ptype=" + ptype + "") + "\">&lt;&lt;��������ѡ��</a>");
                builder.Append(Out.Tab("</div>", ""));

                builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
                builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">��ҳ</a>-");
                builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">��Ϸ</a>-");
                builder.Append("<a href=\"" + Utils.getUrl("HP3.aspx") + "\">" + GameName + "</a>");
                builder.Append(Out.Tab("</div>", ""));
            }
            #endregion
        }
        else
        {
            #region �����
            string GameName = ub.GetSub("HP3Name", xmlPath);
            int meid = new BCW.User.Users().GetUsId();
            int buytype = 0;
            if (meid == 0)
                Utils.Login();
            int ptype = Utils.ParseInt(Utils.GetRequest("ptype", "post", 2, @"^[1-9]$|^10$|^11$|^12$|^13$|^14$|^15$|^16$|^17$|^18$", "����ѡ�����"));
            string info = Utils.GetRequest("info", "post", 1, "", "");
            BCW.HP3.Model.HP3_kjnum model = new BCW.HP3.Model.HP3_kjnum();
            BCW.HP3.Model.HP3_kjnum model2 = new BCW.HP3.Model.HP3_kjnum();
            model = new BCW.HP3.BLL.HP3_kjnum().GetListLastNull();
            model2 = new BCW.HP3.BLL.HP3_kjnum().GetListLast();
            int dnu = int.Parse(model2.datenum);
            Master.Title = "��Ʊ��ע";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">��Ϸ����</a>&gt;<a href=\"" + Utils.getUrl("HP3.aspx") + "\">" + GameName + "</a>&gt;Ͷע");
            builder.Append(Out.Tab("</div>", "<br />"));
            int Sec = Utils.ParseInt(ub.GetSub("HP3Sec", xmlPath));
            String Num1 = Utils.GetRequest("Num1", "post", 1, @"^[\d((,)\d)?]+$", "");
            String Num2 = Utils.GetRequest("Num2", "post", 1, @"^[A-Z\d((,)A-Z\d)?]+$", "");
            String Num3 = Utils.GetRequest("Num3", "post", 1, @"^[A-Z\d((,)A-Z\d)?]+$", "");
            String Num4 = Utils.GetRequest("Num4", "post", 1, @"^[A-Z\d((,)A-Z\d)?]+$", "");
            String Num5 = Utils.GetRequest("Num5", "post", 1, @"^[A-Z\d((,)A-Z\d)?]+$", "");
            builder.Append(Out.Tab("<div>", ""));
            model2.Fnum = model2.Fnum.Trim();
            if (model2.Fnum != "null")
            {
                dnu = int.Parse(model.datenum) + 1;
            }
            int end2 = int.Parse(GetLastStr(model2.datenum, 2));
            if (DateTime.Now.TimeOfDay >= Convert.ToDateTime("22:00").TimeOfDay && end2 == 79)
            {
                dnu = Convert.ToInt32(DateTime.Now.AddDays(1).Date.ToString("yyyyMMdd") + "01");
            }
            else if (DateTime.Now.TimeOfDay <= Convert.ToDateTime("08:50").TimeOfDay && end2 == 79)
            {
                dnu = Convert.ToInt32(DateTime.Now.Date.ToString("yyyyMMdd") + "01");
            }


            if (GetLastStr(dnu.ToString(), 2) == "01")
            {
                if (DateTime.Now.TimeOfDay > Convert.ToDateTime("09:00").TimeOfDay && DateTime.Now.TimeOfDay < Convert.ToDateTime("22:00").TimeOfDay)
                {
                    Utils.Error("��" + dnu + "���ѽ�ֹ��ע,�ȴ�����...", Utils.getUrl("HP3.aspx"));
                }
            }
            else
            {
                if (model2.Fnum != "null")
                {
                    if (model.datetime.AddMinutes(10) < DateTime.Now.AddSeconds(Sec))
                    {
                        Utils.Error("��" + dnu + "���ѽ�ֹ��ע,�ȴ�����...", Utils.getUrl("HP3.aspx"));
                    }
                }
                else
                {
                    if (model2.datetime < DateTime.Now.AddSeconds(Sec))
                    {
                        Utils.Error("��" + dnu + "���ѽ�ֹ��ע,�ȴ�����...", Utils.getUrl("HP3.aspx"));
                    }
                }
            }

            builder.Append("��" + dnu + " ��");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div >", ""));
            if (GetLastStr(dnu.ToString(), 2) == "01")
            {
                if (DateTime.Now.TimeOfDay >= Convert.ToDateTime("22:00").TimeOfDay)
                {
                    djs("div1", Convert.ToDateTime("9:00").AddDays(1).AddSeconds(-Sec));
                }
                else
                {
                    djs("div1", Convert.ToDateTime("9:00").AddSeconds(-Sec));
                }
            }
            else
            {
                if (model2.Fnum != "null")
                {
                    djs("div1", model.datetime.AddSeconds(-Sec));
                }
                else
                {
                    djs("div1", model2.datetime.AddSeconds(-Sec));
                }
            }
            builder.Append("��ֹ|<a href=\"" + Utils.getUrl("HP3.aspx?act=info&amp;ptype=" + ptype + "") + "\">ˢ��</a><br />");
            builder.Append(Out.Tab("</div>", "<br />"));
            string choose1 = Num1;
            string mynum = "";
            if (ptype == 1 || ptype == 2 || ptype == 3 || ptype == 4 || ptype == 5)
            {
                if (Num1 == "")
                {
                    Utils.Error("��ѡ������Ͷעѡ��", "");
                }
                buytype = 1;
                builder.Append(Out.Tab("<div class=\"text\">", ""));
                builder.Append("���ͣ�");
                switch (ptype)
                {
                    case 1:
                        builder.Append("ͬ��Ͷע");
                        break;
                    case 2:
                        builder.Append("˳��Ͷע");
                        break;
                    case 3:
                        builder.Append("ͬ��˳Ͷע");
                        break;
                    case 4:
                        builder.Append("����Ͷע");
                        break;
                    case 5:
                        builder.Append("����Ͷע");
                        break;
                }
                builder.Append("<br/>��ѡ���ˣ�");
                switch (choose1)
                {
                    case "1":
                        builder.Append(speChoose(1));
                        break;
                    case "2":
                        builder.Append(speChoose(2));
                        break;
                    case "3":
                        builder.Append(speChoose(3));
                        break;
                    case "4":
                        builder.Append(speChoose(4));
                        break;
                    case "5":
                        builder.Append(speChoose(5));
                        break;
                    case "6":
                        builder.Append(speChoose(6));
                        break;
                    case "7":
                        builder.Append(speChoose(7));
                        break;
                    case "8":
                        builder.Append(speChoose(8));
                        break;
                    case "9":
                        builder.Append(speChoose(9));
                        break;
                    case "10":
                        builder.Append(speChoose(10));
                        break;
                    case "11":
                        builder.Append(speChoose(11));
                        break;
                    case "12":
                        builder.Append(speChoose(12));
                        break;
                    case "13":
                        builder.Append(speChoose(13));
                        break;
                    case "14":
                        builder.Append(speChoose(14));
                        break;
                    case "15":
                        builder.Append(speChoose(15));
                        break;
                    case "16":
                        builder.Append(speChoose(16));
                        break;
                    case "17":
                        builder.Append(speChoose(17));
                        break;
                    case "18":
                        builder.Append(speChoose(18));
                        break;
                    case "19":
                        builder.Append(speChoose(19));
                        break;
                    case "20":
                        builder.Append(speChoose(20));
                        break;
                    case "21":
                        builder.Append(speChoose(21));
                        break;
                    case "22":
                        builder.Append(speChoose(22));
                        break;
                    case "23":
                        builder.Append(speChoose(23));
                        break;
                    case "24":
                        builder.Append(speChoose(24));
                        break;
                    case "25":
                        builder.Append(speChoose(25));
                        break;
                    case "26":
                        builder.Append(speChoose(26));
                        break;
                    case "27":
                        builder.Append(speChoose(27));
                        break;
                    case "28":
                        builder.Append(speChoose(28));
                        break;
                    case "29":
                        builder.Append(speChoose(29));
                        break;
                    case "30":
                        builder.Append(speChoose(30));
                        break;
                    case "31":
                        builder.Append(speChoose(31));
                        break;
                    case "32":
                        builder.Append(speChoose(32));
                        break;
                    case "33":
                        builder.Append(speChoose(33));
                        break;
                    case "34":
                        builder.Append(speChoose(34));
                        break;
                    case "35":
                        builder.Append(speChoose(35));
                        break;
                    case "36":
                        builder.Append(speChoose(36));
                        break;
                    case "37":
                        builder.Append(speChoose(37));
                        break;
                    case "38":
                        builder.Append(speChoose(38));
                        break;
                    case "39":
                        builder.Append(speChoose(39));
                        break;
                    case "40":
                        builder.Append(speChoose(40));
                        break;
                    case "41":
                        builder.Append(speChoose(41));
                        break;
                    case "42":
                        builder.Append(speChoose(42));
                        break;
                    case "43":
                        builder.Append(speChoose(43));
                        break;
                    case "44":
                        builder.Append(speChoose(44));
                        break;
                    case "45":
                        builder.Append(speChoose(45));
                        break;
                    case "46":
                        builder.Append(speChoose(46));
                        break;
                    case "47":
                        builder.Append(speChoose(47));
                        break;
                    case "48":
                        builder.Append(speChoose(48));
                        break;
                    case "49":
                        builder.Append(speChoose(49));
                        break;
                    case "50":
                        builder.Append(speChoose(50));
                        break;
                    case "51":
                        builder.Append(speChoose(51));
                        break;

                }
                builder.Append(Out.Tab("</div>", "<br />"));
                mynum = Num1;
            }
            else if (ptype == 17 || ptype == 18)
            {
                buytype = 17;
                if (Num5 == "")
                {
                    Utils.Error("��ѡ������Ͷעѡ��", "");
                }
                DataSet dada = new BCW.HP3.BLL.HP3BuySY().GetDaXiao(dnu.ToString(), "1");
                DataSet xiaoxiao = new BCW.HP3.BLL.HP3BuySY().GetDaXiao(dnu.ToString(), "2");
                DataSet dandan = new BCW.HP3.BLL.HP3BuySY().GetDaXiao(dnu.ToString(), "3");
                DataSet shuangshuang = new BCW.HP3.BLL.HP3BuySY().GetDaXiao(dnu.ToString(), "4");
                int das = 0;
                int xiaos = 0;
                int dans = 0;
                int shuangs = 0;
                try
                {
                    das = Convert.ToInt32(dada.Tables[0].Rows[0][0]);
                }
                catch
                {
                    das = 0;
                }
                try
                {
                    xiaos = Convert.ToInt32(xiaoxiao.Tables[0].Rows[0][0]);
                }
                catch
                {
                    xiaos = 0;
                }
                try
                {
                    dans = Convert.ToInt32(dandan.Tables[0].Rows[0][0]);
                }
                catch
                {
                    dans = 0;
                }
                try
                {
                    shuangs = Convert.ToInt32(shuangshuang.Tables[0].Rows[0][0]);
                }
                catch
                {
                    shuangs = 0;
                }
                int cazhi = Convert.ToInt32(XML(25));
                if (Num5 == "1" && das - xiaos >= cazhi)
                {
                    Utils.Error("���Ѵﵽ���Ͷע�޶��ͶС����������ѡ��лл��", "");
                }
                if (Num5 == "2" && xiaos - das >= cazhi)
                {
                    Utils.Error("С�Ѵﵽ���Ͷע�޶��Ͷ�����������ѡ��лл��", "");
                }
                if (Num5 == "3" && dans - shuangs >= cazhi)
                {
                    Utils.Error("���Ѵﵽ���Ͷע�޶��Ͷ˫����������ѡ��лл��", "");
                }
                if (Num5 == "4" && shuangs - dans >= cazhi)
                {
                    Utils.Error("˫�Ѵﵽ���Ͷע�޶��Ͷ������������ѡ��лл��", "");
                }
                builder.Append(Out.Tab("<div class=\"text\">", ""));
                builder.Append("���ͣ�");
                switch (ptype)
                {
                    case 17:
                        builder.Append("��СͶע");
                        break;
                    case 18:
                        builder.Append("��˫Ͷע");
                        break;
                }
                builder.Append("<br/>��Ѻ�ˣ�");
                switch (Num5)
                {
                    case "1":
                        builder.Append("<b>��</b>");
                        break;
                    case "2":
                        builder.Append("<b>С</b>");
                        break;
                    case "3":
                        builder.Append("<b>��</b>");
                        break;
                    case "4":
                        builder.Append("<b>˫</b>");
                        break;
                }
                builder.Append(Out.Tab("</div>", "<br />"));
                mynum = Num5;
            }
            else if (ptype == 6 || ptype == 7 || ptype == 9 || ptype == 11 || ptype == 13 || ptype == 15)
            {
                buytype = ptype;
                int len = Num2.Length;
                if (Num2 == "")
                {
                    Utils.Error("��ѡ������Ͷעѡ��", "");
                }

                builder.Append(Out.Tab("<div class=\"text\">", ""));
                builder.Append("���ͣ�");
                switch (ptype)
                {
                    case 6:
                        builder.Append("��ѡһֱѡ");
                        break;
                    case 7:
                        builder.Append("��ѡ��ֱѡ");
                        break;
                    case 9:
                        builder.Append("��ѡ��ֱѡ");
                        break;
                    case 11:
                        builder.Append("��ѡ��ֱѡ");
                        break;
                    case 13:
                        builder.Append("��ѡ��ֱѡ");
                        break;
                    case 15:
                        builder.Append("��ѡ��ֱѡ");
                        break;
                }
                if (ptype == 7 && len < 3)
                {
                    Utils.Error("������ѡ��2������", "");
                }
                else if (ptype == 9 && len < 5)
                {
                    Utils.Error("������ѡ��3������", "");
                }
                else if (ptype == 11 && len < 7)
                {
                    Utils.Error("������ѡ��4������", "");
                }
                else if (ptype == 13 && len < 9)
                {
                    Utils.Error("������ѡ��5������", "");
                }
                else if (ptype == 15 && len < 11)
                {
                    Utils.Error("������ѡ��6������", "");
                }
                string num2 = Num2.Replace("0", "10");
                builder.Append("<br/>��ѡ���ˣ�");
                builder.Append("<b>" + num2 + "</b>");
                builder.Append(Out.Tab("</div>", "<br />"));
                mynum = num2;

            }
            else if (ptype == 8 || ptype == 10 || ptype == 12 || ptype == 14 || ptype == 16)
            {
                buytype = ptype;
                int len1 = Num3.Length;
                int len2 = Num4.Length;
                if (Num3 == "" || Num4 == "")
                {
                    Utils.Error("��ѡ������Ͷעѡ��", "");
                }

                builder.Append(Out.Tab("<div class=\"text\">", ""));
                builder.Append("���ͣ�");
                switch (ptype)
                {
                    case 8:
                        builder.Append("��ѡ������");
                        break;
                    case 10:
                        builder.Append("��ѡ������");
                        break;
                    case 12:
                        builder.Append("��ѡ�ĵ���");
                        break;
                    case 14:
                        builder.Append("��ѡ�嵨��");
                        break;
                    case 16:
                        builder.Append("��ѡ������");
                        break;
                }
                string[] str3 = Num3.Split(',');
                string[] str4 = Num4.Split(',');
                int zyr = (len1 + 1) / 2;
                for (int i = 0; i < zyr; )
                {
                    bool id = ((IList)str4).Contains(str3[i]);
                    i++;
                    if (id)
                        Utils.Error("��������벻����ͬ", "");
                }
                if (ptype == 8)
                {
                    if (len1 != 1)
                        Utils.Error("���ѡ��1������", "");
                    if (len2 < 3)
                        Utils.Error("��������ѡ����ע����", "");
                }
                else if (ptype == 10)
                {
                    if (len1 > 3)
                        Utils.Error("���ѡ��2������", "");
                    if (len1 == 1 && len2 < 5)
                        Utils.Error("��������ѡ����ע����", "");
                    if (len1 == 2 && len2 < 3)
                        Utils.Error("��������ѡ����ע����", "");
                }
                else if (ptype == 12)
                {
                    if (len1 > 5)
                        Utils.Error("���ѡ��3������", "");
                    if (len1 == 1 && len2 < 7)
                        Utils.Error("��������ѡ����ע����", "");
                    if (len1 == 3 && len2 < 5)
                        Utils.Error("��������ѡ����ע����", "");
                    if (len1 == 5 && len2 < 3)
                        Utils.Error("��������ѡ����ע����", "");
                }
                else if (ptype == 14)
                {
                    if (len1 > 7)
                        Utils.Error("���ѡ��4������", "");
                    if (len1 == 1 && len2 < 9)
                        Utils.Error("��������ѡ����ע����", "");
                    if (len1 == 3 && len2 < 7)
                        Utils.Error("��������ѡ����ע����", "");
                    if (len1 == 5 && len2 < 5)
                        Utils.Error("��������ѡ����ע����", "");
                    if (len1 == 7 && len2 < 3)
                        Utils.Error("��������ѡ����ע����", "");

                }
                else if (ptype == 16)
                {
                    if (len1 > 9)
                        Utils.Error("���ѡ��5������", "");
                    if (len1 == 1 && len2 < 11)
                        Utils.Error("��������ѡ����ע����", "");
                    if (len1 == 3 && len2 < 9)
                        Utils.Error("��������ѡ����ע����", "");
                    if (len1 == 5 && len2 < 7)
                        Utils.Error("��������ѡ����ע����", "");
                    if (len1 == 7 && len2 < 5)
                        Utils.Error("��������ѡ����ע����", "");
                    if (len1 == 9 && len2 < 3)
                        Utils.Error("��������ѡ����ע����", "");
                }
                string num3 = Num3.Replace("0", "10");
                string num4 = Num4.Replace("0", "10");
                builder.Append("<br/>��ѡ���ˣ�");
                builder.Append("<b>" + num3 + "#" + num4 + "</b>");
                builder.Append(Out.Tab("</div>", "<br />"));
                string s = num3 + "#";
                mynum = s + num4;
            }
            int zhu = 1;
            if (ptype < 6 || ptype == 17 || ptype == 18)
            {
                zhu = 1;
            }
            else
            {
                if (ptype == 6)//ֱ1
                {
                    int len = Num2.Length;
                    zhu = (len + 1) / 2;
                }
                else if (ptype == 7)//ֱ2
                {
                    int len = Num2.Length;
                    int Anum = (len + 1) / 2;
                    switch (Anum)
                    {
                        case 2:
                            zhu = 1;
                            break;
                        case 3:
                            zhu = 3;
                            break;
                        case 4:
                            zhu = 6;
                            break;
                        case 5:
                            zhu = 10;
                            break;
                        case 6:
                            zhu = 15;
                            break;
                        case 7:
                            zhu = 21;
                            break;
                        case 8:
                            zhu = 28;
                            break;
                        case 9:
                            zhu = 36;
                            break;
                        case 10:
                            zhu = 45;
                            break;
                        case 11:
                            zhu = 55;
                            break;
                        case 12:
                            zhu = 66;
                            break;
                        case 13:
                            zhu = 78;
                            break;
                    }
                }
                else if (ptype == 8)
                {
                    int len2 = Num4.Length;
                    zhu = (len2 + 1) / 2;
                }
                else if (ptype == 9)//ֱ3
                {
                    int len = Num2.Length;
                    int Anum = (len + 1) / 2;
                    switch (Anum)
                    {
                        case 3:
                            zhu = 1;
                            break;
                        case 4:
                            zhu = 4;
                            break;
                        case 5:
                            zhu = 10;
                            break;
                        case 6:
                            zhu = 20;
                            break;
                        case 7:
                            zhu = 35;
                            break;
                        case 8:
                            zhu = 56;
                            break;
                        case 9:
                            zhu = 84;
                            break;
                        case 10:
                            zhu = 120;
                            break;
                        case 11:
                            zhu = 165;
                            break;
                        case 12:
                            zhu = 220;
                            break;
                        case 13:
                            zhu = 286;
                            break;
                    }
                }
                else if (ptype == 10)
                {
                    int len1 = Num3.Length;
                    int Dan = (len1 + 1) / 2;
                    int len2 = Num4.Length;
                    int Tuo = (len2 + 1) / 2;
                    if (Dan == 1)
                    {
                        switch (Tuo)
                        {
                            case 3:
                                zhu = 3;
                                break;
                            case 4:
                                zhu = 6;
                                break;
                            case 5:
                                zhu = 10;
                                break;
                            case 6:
                                zhu = 15;
                                break;
                            case 7:
                                zhu = 21;
                                break;
                            case 8:
                                zhu = 28;
                                break;
                            case 9:
                                zhu = 36;
                                break;
                            case 10:
                                zhu = 45;
                                break;
                            case 11:
                                zhu = 55;
                                break;
                            case 12:
                                zhu = 66;
                                break;
                        }
                    }
                    else
                    {
                        switch (Tuo)
                        {
                            case 2:
                                zhu = 2;
                                break;
                            case 3:
                                zhu = 3;
                                break;
                            case 4:
                                zhu = 4;
                                break;
                            case 5:
                                zhu = 5;
                                break;
                            case 6:
                                zhu = 6;
                                break;
                            case 7:
                                zhu = 7;
                                break;
                            case 8:
                                zhu = 8;
                                break;
                            case 9:
                                zhu = 9;
                                break;
                            case 10:
                                zhu = 10;
                                break;
                            case 11:
                                zhu = 11;
                                break;
                        }
                    }

                }
                else if (ptype == 11)//ֱ4
                {
                    int len = Num2.Length;
                    int Anum = (len + 1) / 2;
                    switch (Anum)
                    {
                        case 4:
                            zhu = 1;
                            break;
                        case 5:
                            zhu = 5;
                            break;
                        case 6:
                            zhu = 15;
                            break;
                        case 7:
                            zhu = 35;
                            break;
                        case 8:
                            zhu = 70;
                            break;
                        case 9:
                            zhu = 126;
                            break;
                        case 10:
                            zhu = 210;
                            break;
                        case 11:
                            zhu = 330;
                            break;
                        case 12:
                            zhu = 495;
                            break;
                        case 13:
                            zhu = 715;
                            break;
                    }
                }
                else if (ptype == 12)
                {
                    int len1 = Num3.Length;
                    int Dan = (len1 + 1) / 2;
                    int len2 = Num4.Length;
                    int Tuo = (len2 + 1) / 2;
                    switch (Dan)
                    {
                        case 1:
                            switch (Tuo)
                            {
                                case 4:
                                    zhu = 4;
                                    break;
                                case 5:
                                    zhu = 10;
                                    break;
                                case 6:
                                    zhu = 20;
                                    break;
                                case 7:
                                    zhu = 35;
                                    break;
                                case 8:
                                    zhu = 56;
                                    break;
                                case 9:
                                    zhu = 84;
                                    break;
                                case 10:
                                    zhu = 120;
                                    break;
                                case 11:
                                    zhu = 165;
                                    break;
                                case 12:
                                    zhu = 220;
                                    break;
                            }
                            break;
                        case 2:
                            switch (Tuo)
                            {
                                case 3:
                                    zhu = 3;
                                    break;
                                case 4:
                                    zhu = 6;
                                    break;
                                case 5:
                                    zhu = 10;
                                    break;
                                case 6:
                                    zhu = 15;
                                    break;
                                case 7:
                                    zhu = 21;
                                    break;
                                case 8:
                                    zhu = 28;
                                    break;
                                case 9:
                                    zhu = 36;
                                    break;
                                case 10:
                                    zhu = 45;
                                    break;
                                case 11:
                                    zhu = 55;
                                    break;
                            }
                            break;
                        case 3:
                            zhu = Tuo;
                            break;
                    }
                }
                else if (ptype == 13)//ֱ5
                {
                    int len = Num2.Length;
                    int Anum = (len + 1) / 2;
                    switch (Anum)
                    {
                        case 5:
                            zhu = 1;
                            break;
                        case 6:
                            zhu = 6;
                            break;
                        case 7:
                            zhu = 21;
                            break;
                        case 8:
                            zhu = 56;
                            break;
                        case 9:
                            zhu = 126;
                            break;
                        case 10:
                            zhu = 252;
                            break;
                        case 11:
                            zhu = 462;
                            break;
                        case 12:
                            zhu = 792;
                            break;
                        case 13:
                            zhu = 1287;
                            break;
                    }
                }
                else if (ptype == 14)
                {
                    int len1 = Num3.Length;
                    int Dan = (len1 + 1) / 2;
                    int len2 = Num4.Length;
                    int Tuo = (len2 + 1) / 2;
                    switch (Dan)
                    {
                        case 1:
                            switch (Tuo)
                            {
                                case 5:
                                    zhu = 5;
                                    break;
                                case 6:
                                    zhu = 15;
                                    break;
                                case 7:
                                    zhu = 35;
                                    break;
                                case 8:
                                    zhu = 70;
                                    break;
                                case 9:
                                    zhu = 126;
                                    break;
                                case 10:
                                    zhu = 210;
                                    break;
                                case 11:
                                    zhu = 330;
                                    break;
                                case 12:
                                    zhu = 495;
                                    break;
                            }
                            break;
                        case 2:
                            switch (Tuo)
                            {
                                case 4:
                                    zhu = 4;
                                    break;
                                case 5:
                                    zhu = 10;
                                    break;
                                case 6:
                                    zhu = 20;
                                    break;
                                case 7:
                                    zhu = 35;
                                    break;
                                case 8:
                                    zhu = 56;
                                    break;
                                case 9:
                                    zhu = 84;
                                    break;
                                case 10:
                                    zhu = 120;
                                    break;
                                case 11:
                                    zhu = 165;
                                    break;
                            }
                            break;
                        case 3:
                            switch (Tuo)
                            {
                                case 3:
                                    zhu = 3;
                                    break;
                                case 4:
                                    zhu = 6;
                                    break;
                                case 5:
                                    zhu = 10;
                                    break;
                                case 6:
                                    zhu = 15;
                                    break;
                                case 7:
                                    zhu = 21;
                                    break;
                                case 8:
                                    zhu = 28;
                                    break;
                                case 9:
                                    zhu = 36;
                                    break;
                                case 10:
                                    zhu = 45;
                                    break;
                            }
                            break;
                        case 4:
                            zhu = Tuo;
                            break;
                    }
                }
                else if (ptype == 15)//ֱ6
                {
                    int len = Num2.Length;
                    int Anum = (len + 1) / 2;
                    switch (Anum)
                    {

                        case 6:
                            zhu = 1;
                            break;
                        case 7:
                            zhu = 7;
                            break;
                        case 8:
                            zhu = 28;
                            break;
                        case 9:
                            zhu = 84;
                            break;
                        case 10:
                            zhu = 210;
                            break;
                        case 11:
                            zhu = 462;
                            break;
                        case 12:
                            zhu = 924;
                            break;
                        case 13:
                            zhu = 1716;
                            break;
                    }
                }
                else if (ptype == 16)
                {
                    int len1 = Num3.Length;
                    int Dan = (len1 + 1) / 2;
                    int len2 = Num4.Length;
                    int Tuo = (len2 + 1) / 2;
                    switch (Dan)
                    {
                        case 1:
                            switch (Tuo)
                            {

                                case 6:
                                    zhu = 6;
                                    break;
                                case 7:
                                    zhu = 21;
                                    break;
                                case 8:
                                    zhu = 56;
                                    break;
                                case 9:
                                    zhu = 126;
                                    break;
                                case 10:
                                    zhu = 252;
                                    break;
                                case 11:
                                    zhu = 462;
                                    break;
                                case 12:
                                    zhu = 792;
                                    break;

                            }
                            break;
                        case 2:
                            switch (Tuo)
                            {
                                case 5:
                                    zhu = 5;
                                    break;
                                case 6:
                                    zhu = 15;
                                    break;
                                case 7:
                                    zhu = 35;
                                    break;
                                case 8:
                                    zhu = 70;
                                    break;
                                case 9:
                                    zhu = 126;
                                    break;
                                case 10:
                                    zhu = 210;
                                    break;
                                case 11:
                                    zhu = 330;
                                    break;
                            }
                            break;
                        case 3:
                            switch (Tuo)
                            {
                                case 4:
                                    zhu = 4;
                                    break;
                                case 5:
                                    zhu = 10;
                                    break;
                                case 6:
                                    zhu = 20;
                                    break;
                                case 7:
                                    zhu = 35;
                                    break;
                                case 8:
                                    zhu = 56;
                                    break;
                                case 9:
                                    zhu = 84;
                                    break;
                                case 10:
                                    zhu = 120;
                                    break;
                            }
                            break;
                        case 4:
                            switch (Tuo)
                            {
                                case 3:
                                    zhu = 3;
                                    break;
                                case 4:
                                    zhu = 6;
                                    break;
                                case 5:
                                    zhu = 10;
                                    break;
                                case 6:
                                    zhu = 15;
                                    break;
                                case 7:
                                    zhu = 21;
                                    break;
                                case 8:
                                    zhu = 28;
                                    break;
                                case 9:
                                    zhu = 36;
                                    break;

                            }
                            break;
                        case 5:
                            zhu = Tuo;
                            break;
                    }
                }
            }

            if (info == "ok2")
            {
                long Price = Utils.ParseInt64(Utils.GetRequest("Price", "post", 2, @"^[1-9]\d*$", "Ͷע����д����"));
                //֧����ȫ��ʾ
                string[] p_pageArr = { "Price", "mynum", "Num1", "Num2", "Num3", "Num4", "Num5", "ptype", "act", "info" };
                BCW.User.PaySafe.PaySafePage(meid, Utils.getPageUrl(), p_pageArr);
                BCW.HP3.Model.SWB swb = new BCW.HP3.BLL.SWB().GetModel(meid);
                long gold = swb.HP3Money;
                long prices = Convert.ToInt64(Price * zhu);
                //�Ƿ�ˢ��
                long small = Convert.ToInt64(ub.GetSub("HP3SmallPay", xmlPath));
                long big = Convert.ToInt64(ub.GetSub("HP3BigPay", xmlPath));
                string appName = "LIGHT_HP3";
                int Expir = Utils.ParseInt(ub.GetSub("HP3Expir", xmlPath));
                BCW.User.Users.IsFresh(appName, Expir, Price, small, big);

                if (gold < prices)
                {
                    Utils.Error("����" + "���ֱ�" + "����", "");
                }

                long xPrices = Utils.ParseInt64(ub.GetSub("HP3Price", xmlPath));
                if (xPrices > 0)
                {
                    DataSet ds = new BCW.HP3.BLL.HP3BuySY().GetListByID("BuyMoney", meid, dnu.ToString());
                    int oPrices = 0;
                    DataTable dt = ds.Tables[0];
                    foreach (DataRow dr in dt.Rows)
                    {
                        int drs = int.Parse(dr[0].ToString());
                        oPrices = oPrices + drs;
                    }
                    if (oPrices + prices > xPrices)
                    {
                        if (oPrices >= xPrices)
                            Utils.Error("��������ע�Ѵ����ޣ���ȴ�����...", "");
                        else
                            Utils.Error("��������໹������ע" + (xPrices - oPrices) + "" + "���ֱ�" + "", "");
                    }
                }
                string mename = new BCW.BLL.User().GetUsName(meid);
                string buydate = dnu.ToString();
                string buynum = mynum;
                long buymoney = Price;
                new BCW.HP3.BLL.SWB().UpdateHP3Money(meid, -prices);
                BCW.HP3.Model.HP3BuySY modelBuy = new BCW.HP3.Model.HP3BuySY();
                modelBuy.BuyID = meid;
                modelBuy.BuyDate = buydate;
                modelBuy.BuyType = buytype;
                modelBuy.BuyNum = buynum;
                modelBuy.BuyMoney = buymoney;
                modelBuy.BuyZhu = zhu;
                modelBuy.BuyTime = DateTime.Now;
                modelBuy.Odds = Convert.ToDecimal(0);
                if (modelBuy.BuyType == 1)
                {
                    switch (buynum)
                    {
                        case "1":
                        case "2":
                        case "3":
                        case "4":
                            modelBuy.Odds = Convert.ToDecimal(XML(2));
                            break;
                        case "5":
                            modelBuy.Odds = Convert.ToDecimal(XML(1));
                            break;
                        case "6":
                        case "7":
                        case "8":
                        case "9":
                        case "10":
                        case "11":
                        case "12":
                        case "13":
                        case "14":
                        case "15":
                        case "16":
                        case "17":
                            modelBuy.Odds = Convert.ToDecimal(XML(6));
                            break;
                        case "18":
                            modelBuy.Odds = Convert.ToDecimal(XML(5));
                            break;
                        case "19":
                        case "20":
                        case "21":
                        case "22":
                            modelBuy.Odds = Convert.ToDecimal(XML(4));
                            break;
                        case "23":
                            modelBuy.Odds = Convert.ToDecimal(XML(3));
                            break;
                        case "24":
                        case "25":
                        case "26":
                        case "27":
                        case "28":
                        case "29":
                        case "30":
                        case "31":
                        case "32":
                        case "33":
                        case "34":
                        case "35":
                        case "36":
                            modelBuy.Odds = Convert.ToDecimal(XML(8));
                            break;
                        case "37":
                            modelBuy.Odds = Convert.ToDecimal(XML(7));
                            break;
                        case "38":
                        case "39":
                        case "40":
                        case "41":
                        case "42":
                        case "43":
                        case "44":
                        case "45":
                        case "46":
                        case "47":
                        case "48":
                        case "49":
                        case "50":
                            modelBuy.Odds = Convert.ToDecimal(XML(10));
                            break;
                        case "51":
                            modelBuy.Odds = Convert.ToDecimal(XML(9));
                            break;
                    }

                }
                else if (modelBuy.BuyType == 6)
                {
                    modelBuy.Odds = Convert.ToDecimal(XML(11));
                }
                else if (modelBuy.BuyType == 7 || modelBuy.BuyType == 8)
                {
                    modelBuy.Odds = Convert.ToDecimal(XML(12));
                }
                else if (modelBuy.BuyType == 9 || modelBuy.BuyType == 10)
                {
                    modelBuy.Odds = Convert.ToDecimal(XML(13));
                }
                else if (modelBuy.BuyType == 11 || modelBuy.BuyType == 12)
                {
                    modelBuy.Odds = Convert.ToDecimal(XML(14));
                }
                else if (modelBuy.BuyType == 13 || modelBuy.BuyType == 14)
                {
                    modelBuy.Odds = Convert.ToDecimal(XML(15));
                }
                else if (modelBuy.BuyType == 15 || modelBuy.BuyType == 16)
                {
                    modelBuy.Odds = Convert.ToDecimal(XML(16));
                }
                else if (modelBuy.BuyType == 17)
                {
                    try
                    {
                        switch (modelBuy.BuyNum)
                        {
                            case "1":
                                modelBuy.Odds = Convert.ToDecimal(XML(21));
                                break;
                            case "2":
                                modelBuy.Odds = Convert.ToDecimal(XML(22));
                                break;
                            case "3":
                                modelBuy.Odds = Convert.ToDecimal(XML(23));
                                break;
                            case "4":
                                modelBuy.Odds = Convert.ToDecimal(XML(24));
                                break;
                        }
                    }
                    catch
                    {
                        switch (modelBuy.BuyNum)
                        {
                            case "1":
                                modelBuy.Odds = Convert.ToDecimal(XML(21));
                                break;
                            case "2":
                                modelBuy.Odds = Convert.ToDecimal(XML(22));
                                break;
                            case "3":
                                modelBuy.Odds = Convert.ToDecimal(XML(23));
                                break;
                            case "4":
                                modelBuy.Odds = Convert.ToDecimal(XML(24));
                                break;
                        }
                    }
                }

                int id = new BCW.HP3.BLL.HP3BuySY().Add(modelBuy);
                new BCW.HP3.BLL.HP3BuySY().UpdateWillGet(id, 0);
                string gameplay = "";
                switch (modelBuy.BuyType)
                {
                    case 1:
                        gameplay = "��ɫ����ͬ��Ͷע";
                        break;
                    case 17:
                        gameplay = "��С��˫Ͷע";
                        break;
                    default:
                        gameplay = "��ѡͶע";
                        break;

                }
                //��̬
                string wText = "��[url=/bbs/game/HP3.aspx]" + GameName + "�����[/url]��" + gameplay + "����ע**" + "" + "���ֱ�" + "";//+ prices 
                new BCW.BLL.Action().Add(1003, id, meid, mename, wText);
                Utils.Success("��ע", "��ע�ɹ���������" + prices + "" + "���ֱ�" + "(��" + zhu + "ע)<br /><a href=\"" + Utils.getUrl("HP3.aspx?act=info&amp;ptype=" + ptype + "") + "\">&gt;������ע</a>", Utils.getUrl("HP3.aspx"), "5");

            }
            else if (info == "ok")
            {
                long Price = Utils.ParseInt64(Utils.GetRequest("Price", "post", 2, @"^[1-9]\d*$", "Ͷע����д����"));
                builder.Append(Out.Tab("</div>", "<br />"));
                builder.Append(Out.Tab("<div class=\"text\">", ""));
                builder.Append("ע����" + zhu + "ע<br />");
                builder.Append("ÿע��" + Price + "" + "���ֱ�" + "<br />");
                builder.Append("�軨�ѣ�" + (Price * zhu) + "" + "���ֱ�" + "<br />");
                BCW.HP3.Model.SWB swb = new BCW.HP3.BLL.SWB().GetModel(meid);
                builder.Append("���Դ���" + Utils.ConvertGold(swb.HP3Money) + "" + "���ֱ�" + "<br />");
                builder.Append(Out.Tab("</div>", ""));
                string strName = "Price,Num5,Num4,Num3,Num2,Num1,ptype,act,info";
                string strValu = "" + Price + "'" + Num5 + "'" + Num4 + "'" + Num3 + "'" + Num2 + "'" + Num1 + "'" + ptype + "'pay'ok2";
                string strOthe = "ȷ��Ͷע,HP3.aspx,post,0,red";

                builder.Append(Out.wapform(strName, strValu, strOthe));
                builder.Append(Out.Tab("<div>", Out.Hr()));
                builder.Append("<a href=\"" + Utils.getUrl("HP3.aspx?act=info&amp;ptype=" + ptype + "") + "\">&lt;&lt;��������ѡ��</a>");
                builder.Append(Out.Tab("</div>", ""));
            }
            else
            {
                builder.Append(Out.Tab("</div>", Out.LHr()));
                string strText = "ÿעͶע��(" + ub.GetSub("HP3SmallPay", xmlPath) + "-" + ub.GetSub("HP3BigPay", xmlPath) + "" + "���ֱ�" + "):/,,,,,,,,";
                string strName = "Price,Num5,Num4,Num3,Num2,Num1,ptype,act,info";
                string strType = "num,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden";
                string strValu = "'" + Num5 + "'" + Num4 + "'" + Num3 + "'" + Num2 + "'" + Num1 + "'" + ptype + "'pay'ok";
                string strEmpt = "true,false,false,false,false,false,false,false,false";
                string strIdea = "" + "���ֱ�" + "''''''''|/";
                string strOthe = "ȷ��Ͷע,HP3.aspx,post,1,red";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
                builder.Append(Out.Tab("<div>", Out.Hr()));
                builder.Append("<a href=\"" + Utils.getUrl("HP3.aspx?act=info&amp;ptype=" + ptype + "") + "\">&lt;&lt;��������ѡ��</a>");
                builder.Append(Out.Tab("</div>", ""));

                builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
                builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">��ҳ</a>-");
                builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">��Ϸ</a>-");
                builder.Append("<a href=\"" + Utils.getUrl("HP3.aspx") + "\">" + GameName + "</a>");
                builder.Append(Out.Tab("</div>", ""));
            }
            #endregion
        }
    }
    // Ͷע��ʽ
    private string OutType(int Types)
    {
        string pText = string.Empty;
        if (Types == 1)
            pText = "ͬ��";
        else if (Types == 2)
            pText = "˳��";
        else if (Types == 3)
            pText = "Ѻͬ��˳";
        else if (Types == 4)
            pText = "����";
        else if (Types == 5)
            pText = "����";
        else if (Types == 6)
            pText = "��һֱѡ";
        else if (Types == 7)
            pText = "�ζ�ֱѡ";
        else if (Types == 8)
            pText = "�ζ�����";
        else if (Types == 9)
            pText = "����ֱѡ";
        else if (Types == 10)
            pText = "��������";
        else if (Types == 11)
            pText = "����ֱѡ";
        else if (Types == 12)
            pText = "���ĵ���";
        else if (Types == 13)
            pText = "����ֱѡ";
        else if (Types == 14)
            pText = "���嵨��";
        else if (Types == 15)
            pText = "����ֱѡ";
        else if (Types == 16)
            pText = "��������";
        else if (Types == 17)
            pText = "Ѻע��С";
        else if (Types == 18)
            pText = "Ѻע��˫";
        return pText;
    }
    //���˶�����ʷ
    private void MyListPage()
    {

        string GameName = ub.GetSub("HP3Name", xmlPath);
        int meid = new BCW.User.Users().GetUsId();
        //meid = 1555;
        if (meid == 0)
            Utils.Login();

        int ptype = Utils.ParseInt(Utils.GetRequest("ptype", "get", 1, @"^[1-2]$", "1"));
        string strTitle = "";
        if (ptype == 1)
            strTitle = "�ҵ�δ����ע";
        else
            strTitle = "�ҵ���ʷ��ע";

        Master.Title = strTitle;
        BCW.HP3.Model.HP3_kjnum lastdate = new BCW.HP3.Model.HP3_kjnum();
        lastdate = new BCW.HP3.BLL.HP3_kjnum().GetListLastNull();
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">��Ϸ����</a>&gt;<a href=\"" + Utils.getUrl("HP3.aspx") + "\">" + GameName + "</a>&gt;" + strTitle + "");
        builder.Append(Out.Tab("</div>", "<br />"));
        //builder.Append(Out.Tab("<div class=\"title\">", ""));
        //builder.Append(strTitle);
        //builder.Append(Out.Tab("</div>", "<br />"));
        if (SWB == 0)
        {
            #region ��ʽ��
            int pageIndex;
            int recordCount;
            int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
            string strWhere = string.Empty;
            strWhere = "BuyID=" + meid + "";
            if (ptype == 1)
                strWhere += " and BuyDate>" + lastdate.datenum;
            else
                strWhere += " and BuyDate<=" + lastdate.datenum;

            string[] pageValUrl = { "act", "ptype", "backurl" };
            pageIndex = Utils.ParseInt(Request.QueryString["page"]);
            if (pageIndex == 0)
                pageIndex = 1;

            string HP3qi = "";
            // ��ʼ��ȡ�б�
            IList<BCW.HP3.Model.HP3Buy> listHP3 = new BCW.HP3.BLL.HP3Buy().GetHP3ListByPage(pageIndex, pageSize, strWhere, out recordCount);
            if (listHP3.Count > 0)
            {
                int k = 1;
                foreach (BCW.HP3.Model.HP3Buy n in listHP3)
                {
                    if (k % 2 == 0)
                        builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                    else
                    {
                        if (k == 1)
                            builder.Append(Out.Tab("<div>", ""));
                        else
                            builder.Append(Out.Tab("<div>", "<br />"));
                    }
                    if (n.BuyDate.ToString() != HP3qi)
                    {
                        if (ptype == 1)
                            builder.Append("=��" + n.BuyDate + "��=<br />");
                        else
                        {
                            BCW.HP3.Model.HP3_kjnum wins = new BCW.HP3.BLL.HP3_kjnum().GetDataByState(n.BuyDate);
                            if (wins.Fnum.Trim() != "null")
                                builder.Append("=��" + n.BuyDate + "��=" + wins.Fnum + wins.Snum + wins.Tnum + "<br />");
                            else
                                builder.Append("=��" + n.BuyDate + "��=<br />");
                        }
                    }




                    builder.Append("<b>" + ((pageIndex - 1) * pageSize + k) + ".</b>");
                    if (n.BuyType == 1)
                    {
                        string st = "null";
                        switch (n.BuyNum)
                        {
                            case "1":
                            case "2":
                            case "3":
                            case "4":
                            case "5":
                                st = "ͬ��Ͷע";
                                break;
                            case "6":
                            case "7":
                            case "8":
                            case "9":
                            case "10":
                            case "11":
                            case "12":
                            case "13":
                            case "14":
                            case "15":
                            case "16":
                            case "17":
                            case "18":
                                st = "˳��Ͷע";
                                break;
                            case "19":
                            case "20":
                            case "21":
                            case "22":
                            case "23":
                                st = "ͬ��˳Ͷע";
                                break;
                            case "24":
                            case "25":
                            case "26":
                            case "27":
                            case "28":
                            case "29":
                            case "30":
                            case "31":
                            case "32":
                            case "33":
                            case "34":
                            case "35":
                            case "36":
                            case "37":
                                st = "����Ͷע";
                                break;
                            case "38":
                            case "39":
                            case "40":
                            case "41":
                            case "42":
                            case "43":
                            case "44":
                            case "45":
                            case "46":
                            case "47":
                            case "48":
                            case "49":
                            case "50":
                            case "51":
                                st = "����Ͷע";
                                break;

                        }
                        builder.Append("<b>[" + st + "]</b>λ��:" + speChoose(Convert.ToInt32(n.BuyNum)) + "/ÿע" + n.BuyMoney + "" + ub.Get("SiteBz") + "/��" + n.BuyZhu + "ע/����" + n.Odds + "[" + DT.FormatDate(n.BuyTime, 1) + "]");
                    }
                    else if (n.BuyType == 17)
                    {
                        string st = "null";
                        string Buynums = "null";
                        switch (n.BuyNum)
                        {
                            case "1":
                                Buynums = "��";
                                st = "��СͶע";
                                break;
                            case "2":
                                Buynums = "С";
                                st = "��СͶע";
                                break;
                            case "3":
                                Buynums = "��";
                                st = "��˫Ͷע";
                                break;
                            case "4":
                                Buynums = "˫";
                                st = "��˫Ͷע";
                                break;
                        }
                        builder.Append("<b>[" + st + "]</b>λ��:" + Buynums + "/ÿע" + n.BuyMoney + "" + ub.Get("SiteBz") + "/��" + n.BuyZhu + "ע/����" + n.Odds + "[" + DT.FormatDate(n.BuyTime, 1) + "]");
                    }
                    else
                    {
                        string st = "null";
                        switch (n.BuyType)
                        {
                            case 6:
                                st = "��ѡһͶע";
                                break;
                            case 7:
                            case 8:
                                st = "��ѡ��Ͷע";
                                break;
                            case 9:
                            case 10:
                                st = "��ѡ��Ͷע";
                                break;
                            case 11:
                            case 12:
                                st = "��ѡ��Ͷע";
                                break;
                            case 13:
                            case 14:
                                st = "��ѡ��Ͷע";
                                break;
                            case 15:
                            case 16:
                                st = "��ѡ��Ͷע";
                                break;
                        }
                        builder.Append("<b>[" + st + "]</b>λ��:" + n.BuyNum + "/ÿע" + n.BuyMoney + "" + ub.Get("SiteBz") + "/��" + n.BuyZhu + "ע/����" + n.Odds + "[" + DT.FormatDate(n.BuyTime, 1) + "]");
                    }
                    bool kbb = new BCW.HP3.BLL.HP3Winner().Exists(n.ID);
                    if (kbb)
                    {
                        DataSet dds = new BCW.HP3.BLL.HP3Winner().GetList("ID=" + n.ID);
                        BCW.HP3.Model.HP3Winner model = new BCW.HP3.Model.HP3Winner();
                        model.WinMoney = Convert.ToInt64(dds.Tables[0].Rows[0][3]);
                        model.WinZhu = Convert.ToInt32(dds.Tables[0].Rows[0][5]);
                        if (model.WinMoney > 0)
                        {
                            builder.Append("Ӯ<b style=\"color:red\">" + model.WinMoney + "</b>" + ub.Get("SiteBz") + "[��" + model.WinZhu + "ע]");
                        }

                    }
                    builder.Append("<a href=\"" + Utils.getUrl("HP3.aspx?act=mylistview&amp;id=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">����&gt;&gt;</a>");

                    HP3qi = n.BuyDate.ToString();
                    k++;
                    builder.Append(Out.Tab("</div>", ""));
                }

                // ��ҳ
                builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));

            }
            else
            {
                builder.Append(Out.Div("div", "û����ؼ�¼.."));
            }
            builder.Append(Out.Tab("<div>", Out.Hr()));
            builder.Append("<a href=\"" + Utils.getUrl("HP3.aspx?act=mylist&amp;ptype=1") + "\">δ����ע</a> ");
            builder.Append("<a href=\"" + Utils.getUrl("HP3.aspx?act=mylist&amp;ptype=2") + "\">��ʷ��ע</a>");
            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
            builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">��ҳ</a>-");
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">��Ϸ</a>-");
            builder.Append("<a href=\"" + Utils.getUrl("HP3.aspx") + "\">" + GameName + "</a>");
            builder.Append(Out.Tab("</div>", ""));
            #endregion
        }
        else
        {
            #region �����
            int pageIndex;
            int recordCount;
            int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
            string strWhere = string.Empty;
            strWhere = "BuyID=" + meid + "";
            if (ptype == 1)
                strWhere += " and BuyDate>" + lastdate.datenum;
            else
                strWhere += " and BuyDate<=" + lastdate.datenum;

            string[] pageValUrl = { "act", "ptype", "backurl" };
            pageIndex = Utils.ParseInt(Request.QueryString["page"]);
            if (pageIndex == 0)
                pageIndex = 1;

            string HP3qi = "";
            // ��ʼ��ȡ�б�
            IList<BCW.HP3.Model.HP3BuySY> listHP3 = new BCW.HP3.BLL.HP3BuySY().GetHP3ListByPage(pageIndex, pageSize, strWhere, out recordCount);
            if (listHP3.Count > 0)
            {
                int k = 1;
                foreach (BCW.HP3.Model.HP3BuySY n in listHP3)
                {
                    if (k % 2 == 0)
                        builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                    else
                    {
                        if (k == 1)
                            builder.Append(Out.Tab("<div>", ""));
                        else
                            builder.Append(Out.Tab("<div>", "<br />"));
                    }
                    if (n.BuyDate.ToString() != HP3qi)
                    {
                        if (ptype == 1)
                            builder.Append("=��" + n.BuyDate + "��=<br />");
                        else
                        {
                            BCW.HP3.Model.HP3_kjnum wins = new BCW.HP3.BLL.HP3_kjnum().GetDataByState(n.BuyDate);
                            if (wins.Fnum.Trim() != "null")
                                builder.Append("=��" + n.BuyDate + "��=" + wins.Fnum + wins.Snum + wins.Tnum + "<br />");
                            else
                                builder.Append("=��" + n.BuyDate + "��=<br />");
                        }

                    }

                    builder.Append("<b>" + ((pageIndex - 1) * pageSize + k) + ".</b>");
                    if (n.BuyType == 1)
                    {
                        string st = "null";
                        switch (n.BuyNum)
                        {
                            case "1":
                            case "2":
                            case "3":
                            case "4":
                            case "5":
                                st = "ͬ��Ͷע";
                                break;
                            case "6":
                            case "7":
                            case "8":
                            case "9":
                            case "10":
                            case "11":
                            case "12":
                            case "13":
                            case "14":
                            case "15":
                            case "16":
                            case "17":
                            case "18":
                                st = "˳��Ͷע";
                                break;
                            case "19":
                            case "20":
                            case "21":
                            case "22":
                            case "23":
                                st = "ͬ��˳Ͷע";
                                break;
                            case "24":
                            case "25":
                            case "26":
                            case "27":
                            case "28":
                            case "29":
                            case "30":
                            case "31":
                            case "32":
                            case "33":
                            case "34":
                            case "35":
                            case "36":
                            case "37":
                                st = "����Ͷע";
                                break;
                            case "38":
                            case "39":
                            case "40":
                            case "41":
                            case "42":
                            case "43":
                            case "44":
                            case "45":
                            case "46":
                            case "47":
                            case "48":
                            case "49":
                            case "50":
                            case "51":
                                st = "����Ͷע";
                                break;

                        }
                        builder.Append("<b>[" + st + "]</b>λ��:" + speChoose(Convert.ToInt32(n.BuyNum)) + "/ÿע" + n.BuyMoney + "" + "���ֱ�" + "/��" + n.BuyZhu + "ע[" + n.BuyTime + "]");
                    }
                    else if (n.BuyType == 17)
                    {
                        string st = "null";
                        string Buynums = "null";
                        switch (n.BuyNum)
                        {
                            case "1":
                                Buynums = "��";
                                st = "��СͶע";
                                break;
                            case "2":
                                Buynums = "С";
                                st = "��СͶע";
                                break;
                            case "3":
                                Buynums = "��";
                                st = "��˫Ͷע";
                                break;
                            case "4":
                                Buynums = "˫";
                                st = "��˫Ͷע";
                                break;
                        }
                        builder.Append("<b>[" + st + "]</b>λ��:" + Buynums + "/ÿע" + n.BuyMoney + "" + "���ֱ�" + "/��" + n.BuyZhu + "ע[" + n.BuyTime + "]");
                    }
                    else
                    {
                        string st = "null";
                        switch (n.BuyType)
                        {
                            case 6:
                                st = "��ѡһͶע";
                                break;
                            case 7:
                            case 8:
                                st = "��ѡ��Ͷע";
                                break;
                            case 9:
                            case 10:
                                st = "��ѡ��Ͷע";
                                break;
                            case 11:
                            case 12:
                                st = "��ѡ��Ͷע";
                                break;
                            case 13:
                            case 14:
                                st = "��ѡ��Ͷע";
                                break;
                            case 15:
                            case 16:
                                st = "��ѡ��Ͷע";
                                break;
                        }
                        builder.Append("<b>[" + st + "]</b>λ��:" + n.BuyNum + "/ÿע" + n.BuyMoney + "" + "���ֱ�" + "/��" + n.BuyZhu + "ע[" + n.BuyTime + "]");
                    }
                    bool kbb = new BCW.HP3.BLL.HP3WinnerSY().Exists(n.ID);
                    if (kbb)
                    {
                        DataSet dds = new BCW.HP3.BLL.HP3WinnerSY().GetList("ID=" + n.ID);
                        BCW.HP3.Model.HP3WinnerSY model = new BCW.HP3.Model.HP3WinnerSY();
                        model.WinMoney = Convert.ToInt64(dds.Tables[0].Rows[0][3]);
                        if (model.WinMoney > 0)
                        {
                            builder.Append("Ӯ<b style=\"color:red\">" + model.WinMoney + "</b>" + "���ֱ�" + "");
                        }

                    }
                    builder.Append("<a href=\"" + Utils.getUrl("HP3.aspx?act=mylistview&amp;id=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">����&gt;&gt;</a>");

                    HP3qi = n.BuyDate.ToString();
                    k++;
                    builder.Append(Out.Tab("</div>", ""));
                }

                // ��ҳ
                builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));

            }
            else
            {
                builder.Append(Out.Div("div", "û����ؼ�¼.."));
            }
            builder.Append(Out.Tab("<div>", Out.Hr()));
            builder.Append("<a href=\"" + Utils.getUrl("HP3.aspx?act=mylist&amp;ptype=1") + "\">δ����ע</a> ");
            builder.Append("<a href=\"" + Utils.getUrl("HP3.aspx?act=mylist&amp;ptype=2") + "\">��ʷ��ע</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
            builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">��ҳ</a>-");
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">��Ϸ</a>-");
            builder.Append("<a href=\"" + Utils.getUrl("HP3.aspx") + "\">" + GameName + "</a>");
            builder.Append(Out.Tab("</div>", ""));
            #endregion
        }
    }
    //���˶�������
    private void MyListViewPage()
    {
        if (SWB == 0)
        {
            #region ��ʽ��
            string GameName = ub.GetSub("HP3Name", xmlPath);
            int meid = new BCW.User.Users().GetUsId();
            if (meid == 0)
                Utils.Login();

            int id = Utils.ParseInt(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "ID����"));
            BCW.HP3.Model.HP3Buy n = new BCW.HP3.BLL.HP3Buy().GetModel(id);
            if (n == null || n.BuyID != meid)
            {
                Utils.Error("�����ڵļ�¼", "");
            }

            Master.Title = "��" + n.BuyDate + "��";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">��Ϸ����</a>&gt;<a href=\"" + Utils.getUrl("HP3.aspx") + "\">" + GameName + "</a>&gt;��ʷ����");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("��" + n.BuyDate + "��");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            bool kss = new BCW.HP3.BLL.HP3_kjnum().Exists2(n.BuyDate);
            if (kss)
            {
                DataSet mool = new BCW.HP3.BLL.HP3_kjnum().GetList("datenum='" + n.BuyDate + "'");
                BCW.HP3.Model.HP3_kjnum ThatNum = new BCW.HP3.Model.HP3_kjnum();
                ThatNum.datenum = Convert.ToString(mool.Tables[0].Rows[0][0]);
                ThatNum.Fnum = Convert.ToString(mool.Tables[0].Rows[0][1]);
                ThatNum.Snum = Convert.ToString(mool.Tables[0].Rows[0][2]);
                ThatNum.Tnum = Convert.ToString(mool.Tables[0].Rows[0][3]);
                builder.Append("��������:" + ThatNum.Fnum + ThatNum.Snum + ThatNum.Tnum);
            }
            else
            {
                builder.Append("��������:δ����");
            }

            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            if (n.BuyType == 1)
            {
                string st = "null";
                switch (n.BuyNum)
                {
                    case "1":
                    case "2":
                    case "3":
                    case "4":
                    case "5":
                        st = "ͬ��Ͷע";
                        break;
                    case "6":
                    case "7":
                    case "8":
                    case "9":
                    case "10":
                    case "11":
                    case "12":
                    case "13":
                    case "14":
                    case "15":
                    case "16":
                    case "17":
                    case "18":
                        st = "˳��Ͷע";
                        break;
                    case "19":
                    case "20":
                    case "21":
                    case "22":
                    case "23":
                        st = "ͬ��˳Ͷע";
                        break;
                    case "24":
                    case "25":
                    case "26":
                    case "27":
                    case "28":
                    case "29":
                    case "30":
                    case "31":
                    case "32":
                    case "33":
                    case "34":
                    case "35":
                    case "36":
                    case "37":
                        st = "����Ͷע";
                        break;
                    case "38":
                    case "39":
                    case "40":
                    case "41":
                    case "42":
                    case "43":
                    case "44":
                    case "45":
                    case "46":
                    case "47":
                    case "48":
                    case "49":
                    case "50":
                    case "51":
                        st = "����Ͷע";
                        break;

                }
                builder.Append("����:<b>" + st + "</b><br />λ��:" + speChoose(Convert.ToInt32(n.BuyNum)));
            }
            else if (n.BuyType == 17 || n.BuyType == 18)
            {
                string st = "null";
                string Buynums = "null";
                switch (n.BuyNum)
                {
                    case "1":
                        Buynums = "��";
                        st = "��СͶע";
                        break;
                    case "2":
                        Buynums = "С";
                        st = "��СͶע";
                        break;
                    case "3":
                        Buynums = "��";
                        st = "��˫Ͷע";
                        break;
                    case "4":
                        Buynums = "˫";
                        st = "��˫Ͷע";
                        break;
                }
                builder.Append("����:<b>" + st + "</b><br />λ��:" + Buynums);
            }
            else
            {
                string st = "null";
                switch (n.BuyType)
                {
                    case 6:
                        st = "��ѡһͶע";
                        break;
                    case 7:
                    case 8:
                        st = "��ѡ��Ͷע";
                        break;
                    case 9:
                    case 10:
                        st = "��ѡ��Ͷע";
                        break;
                    case 11:
                    case 12:
                        st = "��ѡ��Ͷע";
                        break;
                    case 13:
                    case 14:
                        st = "��ѡ��Ͷע";
                        break;
                    case 15:
                    case 16:
                        st = "��ѡ��Ͷע";
                        break;
                }
                builder.Append("����:<b>" + st + "</b><br />λ��:" + n.BuyNum);
            }
            builder.Append("<br />ÿע:" + n.BuyMoney + "" + ub.Get("SiteBz") + "<br />ע��:" + n.BuyZhu + "ע<br />����:" + n.BuyMoney * n.BuyZhu + "" + ub.Get("SiteBz") + "<br />����:" + n.Odds);
            builder.Append("<br />��ע:" + n.BuyTime + "");
            bool kbb = new BCW.HP3.BLL.HP3Winner().Exists(n.ID);
            if (kbb)
            {
                DataSet dds = new BCW.HP3.BLL.HP3Winner().GetList("ID=" + n.ID);
                BCW.HP3.Model.HP3Winner model = new BCW.HP3.Model.HP3Winner();
                model.WinMoney = Convert.ToInt64(dds.Tables[0].Rows[0][3]);
                model.WinZhu = Convert.ToInt32(dds.Tables[0].Rows[0][5]);
                builder.Append("<br />���:Ӯ:<b style=\"color:red\">" + model.WinMoney + "</b>" + ub.Get("SiteBz") + "[��" + model.WinZhu + "ע]");
            }
            else
            {
                if (kss)
                    builder.Append("<br />���:δ�н�");
                else
                    builder.Append("<br />���:δ����");

            }
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getPage("HP3.aspx?act=mylist&amp;ptype=2") + "\">������һ��</a>");
            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
            builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">��ҳ</a>-");
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">��Ϸ</a>-");
            builder.Append("<a href=\"" + Utils.getUrl("HP3.aspx") + "\">" + GameName + "</a>");
            builder.Append(Out.Tab("</div>", ""));
            #endregion
        }
        else
        {
            #region �����
            string GameName = ub.GetSub("HP3Name", xmlPath);
            int meid = new BCW.User.Users().GetUsId();
            if (meid == 0)
                Utils.Login();

            int id = Utils.ParseInt(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "ID����"));
            BCW.HP3.Model.HP3BuySY n = new BCW.HP3.BLL.HP3BuySY().GetModel(id);
            if (n == null || n.BuyID != meid)
            {
                Utils.Error("�����ڵļ�¼", "");
            }

            Master.Title = "��" + n.BuyDate + "��";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("��" + n.BuyDate + "��");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            bool kss = new BCW.HP3.BLL.HP3_kjnum().Exists2(n.BuyDate);
            if (kss)
            {
                DataSet mool = new BCW.HP3.BLL.HP3_kjnum().GetList("datenum='" + n.BuyDate + "'");
                BCW.HP3.Model.HP3_kjnum ThatNum = new BCW.HP3.Model.HP3_kjnum();
                ThatNum.datenum = Convert.ToString(mool.Tables[0].Rows[0][0]);
                ThatNum.Fnum = Convert.ToString(mool.Tables[0].Rows[0][1]);
                ThatNum.Snum = Convert.ToString(mool.Tables[0].Rows[0][2]);
                ThatNum.Tnum = Convert.ToString(mool.Tables[0].Rows[0][3]);
                builder.Append("��������:" + ThatNum.Fnum + ThatNum.Snum + ThatNum.Tnum);
            }
            else
            {
                builder.Append("��������:δ����");
            }

            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            if (n.BuyType == 1)
            {
                string st = "null";
                switch (n.BuyNum)
                {
                    case "1":
                    case "2":
                    case "3":
                    case "4":
                    case "5":
                        st = "ͬ��Ͷע";
                        break;
                    case "6":
                    case "7":
                    case "8":
                    case "9":
                    case "10":
                    case "11":
                    case "12":
                    case "13":
                    case "14":
                    case "15":
                    case "16":
                    case "17":
                    case "18":
                        st = "˳��Ͷע";
                        break;
                    case "19":
                    case "20":
                    case "21":
                    case "22":
                    case "23":
                        st = "ͬ��˳Ͷע";
                        break;
                    case "24":
                    case "25":
                    case "26":
                    case "27":
                    case "28":
                    case "29":
                    case "30":
                    case "31":
                    case "32":
                    case "33":
                    case "34":
                    case "35":
                    case "36":
                    case "37":
                        st = "����Ͷע";
                        break;
                    case "38":
                    case "39":
                    case "40":
                    case "41":
                    case "42":
                    case "43":
                    case "44":
                    case "45":
                    case "46":
                    case "47":
                    case "48":
                    case "49":
                    case "50":
                    case "51":
                        st = "����Ͷע";
                        break;

                }
                builder.Append("����:<b>" + st + "</b><br />λ��:" + speChoose(Convert.ToInt32(n.BuyNum)));
            }
            else if (n.BuyType == 17 || n.BuyType == 18)
            {
                string st = "null";
                string Buynums = "null";
                switch (n.BuyNum)
                {
                    case "1":
                        Buynums = "��";
                        st = "��СͶע";
                        break;
                    case "2":
                        Buynums = "С";
                        st = "��СͶע";
                        break;
                    case "3":
                        Buynums = "��";
                        st = "��˫Ͷע";
                        break;
                    case "4":
                        Buynums = "˫";
                        st = "��˫Ͷע";
                        break;
                }
                builder.Append("����:<b>" + st + "</b><br />λ��:" + Buynums);
            }
            else
            {
                string st = "null";
                switch (n.BuyType)
                {
                    case 6:
                        st = "��ѡһͶע";
                        break;
                    case 7:
                    case 8:
                        st = "��ѡ��Ͷע";
                        break;
                    case 9:
                    case 10:
                        st = "��ѡ��Ͷע";
                        break;
                    case 11:
                    case 12:
                        st = "��ѡ��Ͷע";
                        break;
                    case 13:
                    case 14:
                        st = "��ѡ��Ͷע";
                        break;
                    case 15:
                    case 16:
                        st = "��ѡ��Ͷע";
                        break;
                }
                builder.Append("����:<b>" + st + "</b><br />λ��:" + n.BuyNum);
            }
            builder.Append("<br />ÿע:" + n.BuyMoney + "" + "���ֱ�" + "<br />ע��:" + n.BuyZhu + "ע<br />����:" + n.BuyMoney * n.BuyZhu + "" + "���ֱ�" + "");
            builder.Append("<br />��ע:" + n.BuyTime + "");
            bool kbb = new BCW.HP3.BLL.HP3WinnerSY().Exists(n.ID);
            if (kbb)
            {
                DataSet dds = new BCW.HP3.BLL.HP3WinnerSY().GetList("ID=" + n.ID);
                BCW.HP3.Model.HP3WinnerSY model = new BCW.HP3.Model.HP3WinnerSY();
                model.WinMoney = Convert.ToInt64(dds.Tables[0].Rows[0][3]);
                builder.Append("<br />���:Ӯ:<b style=\"color:red\">" + model.WinMoney + "</b>" + "���ֱ�" + "");
            }
            else
            {
                if (kss)
                    builder.Append("<br />���:δ�н�");
                else
                    builder.Append("<br />���:δ����");

            }
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getPage("HP3.aspx?act=mylist&amp;ptype=2") + "\">&lt;&lt;������һ��</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("HP3.aspx") + "\">&lt;&lt;����" + GameName + "</a>");
            builder.Append(Out.Tab("</div>", ""));
            #endregion
        }
    }
    //���а�ҳ��
    private void TopPage()
    {
        if (SWB == 0)
        {
            #region ��ʽ��
            int meid = new BCW.User.Users().GetUsId();
            string GameName = ub.GetSub("HP3Name", xmlPath);
            Master.Title = "" + GameName + "���а�";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">��Ϸ����</a>&gt;<a href=\"" + Utils.getUrl("HP3.aspx") + "\">" + GameName + "</a>&gt;���а�");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            int tops = int.Parse(Utils.GetRequest("tops", "all", 1, "", "1"));

            if (tops == 1)
                builder.Append("���˸����ܰ�|");
            else
                builder.Append("<a href=\"" + Utils.getUrl("HP3.aspx?act=top&amp;tops=1") + "\">���˸����ܰ�</a>|");
            if (tops == 2)
                builder.Append("���������ܰ�");
            else
                builder.Append("<a href=\"" + Utils.getUrl("HP3.aspx?act=top&amp;tops=2") + "\">���������ܰ�</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
            int pageIndex = 0;//��ǰҳ
            int recordCount;//��¼������
            int pageSize = 10;//��ҳ��С
            string[] pageValUrl = { "act", "backurl" };
            pageIndex = Utils.ParseInt(Request.QueryString["page"]);
            if (pageIndex == 0)
                pageIndex = 1;
            if (pageIndex >= 10)
                pageIndex = 10;
            if (tops == 1)
            {
                DataSet rowbang = new BCW.HP3.BLL.HP3Winner().GetListBang();
                recordCount = rowbang.Tables[0].Rows.Count;
                DataSet bang = new BCW.HP3.BLL.HP3Winner().GetListByPage(0, recordCount);//pageIndex * pageSize);
                if (recordCount >= 0)
                {
                    int k = 1;
                    int koo = (pageIndex - 1) * pageSize;
                    int skt = pageSize;
                    if (recordCount > koo + pageSize)
                    {
                        skt = pageSize;
                    }
                    else
                    {
                        skt = recordCount - koo;
                    }
                    for (int soms = 0; soms < skt; soms++)
                    {
                        int usid = Convert.ToInt32(bang.Tables[0].Rows[koo + soms][1]);
                        long usmoney = Convert.ToInt64(bang.Tables[0].Rows[koo + soms][2]);
                        if (k % 2 == 0)
                            builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                        else
                        {
                            if (k == 1)
                                builder.Append(Out.Tab("<div>", "<br />"));
                            else
                                builder.Append(Out.Tab("<div>", "<br />"));
                        }
                        string mename = new BCW.BLL.User().GetUsName(usid);
                        int wd = (pageIndex - 1) * 10 + k;
                        builder.Append("[<b style=\"color:red\">TOP" + wd + "</b>].<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + usid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + mename + "(" + usid + ")</a>��׬��" + "<b style=\"color:red\">" + usmoney + "</b>" + ub.Get("SiteBz") + "");
                        k++;
                        builder.Append(Out.Tab("</div>", ""));
                    }
                    int Pcount = 0;
                    if (recordCount <= 100)
                    {
                        Pcount = recordCount;
                    }
                    else
                    {
                        Pcount = 100;
                    }
                    builder.Append(BasePage.MultiPage(pageIndex, pageSize, Pcount, Utils.getPageUrl(), pageValUrl, "page", 0));
                }
                else
                {
                    builder.Append(Out.Div("div", "û����ؼ�¼.."));
                }
            }
            else
            {
                DataSet rowbang = new BCW.HP3.BLL.HP3Buy().GetListBang();
                int som = rowbang.Tables[0].Rows.Count - 1;
                if (som >= 0)
                {
                    int k = 1;
                    recordCount = rowbang.Tables[0].Rows.Count;
                    int n = 0;
                    if (som >= 10)
                    {
                        n = som - 9;
                    }
                    builder.Append(Out.Tab("<div>", ""));
                    builder.Append("<b style=\"color:#8A7B66\">����Top10</b>");
                    builder.Append(Out.Tab("</div>", ""));
                    for (; som >= n; som--)
                    {
                        int usid = Convert.ToInt32(rowbang.Tables[0].Rows[som][0]);
                        long usmoney = Convert.ToInt64(rowbang.Tables[0].Rows[som][1]);
                        if (k % 2 == 0)
                            builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                        else
                        {
                            if (k == 1)
                                builder.Append(Out.Tab("<div>", "<br />"));
                            else
                                builder.Append(Out.Tab("<div>", "<br />"));
                        }
                        string mename = new BCW.BLL.User().GetUsName(usid);
                        builder.Append("[<b style=\"color:red\">TOP" + k + "</b>]<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + usid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + mename + "(" + usid + ")</a>������" + "<b style=\"color:red\">" + usmoney + "</b>" + ub.Get("SiteBz") + "");
                        k++;
                        builder.Append(Out.Tab("</div>", ""));
                    }
                }
                else
                {
                    builder.Append(Out.Div("div", "û����ؼ�¼.."));
                }

            }
            string searchday = Utils.GetRequest("inputdate", "all", 1, @"^([0-9]{3}[1-9]|[0-9]{2}[1-9][0-9]{1}|[0-9]{1}[1-9][0-9]{2}|[1-9][0-9]{3})(((0[13578]|1[02])(0[1-9]|[12][0-9]|3[01]))|((0[469]|11)(0[1-9]|[12][0-9]|30))|(02(0[1-9]|[1][0-9]|2[0-8])))", DateTime.Now.ToString("yyyyMMdd") + "01");
            string searchday2 = Utils.GetRequest("inputdate2", "all", 1, @"^([0-9]{3}[1-9]|[0-9]{2}[1-9][0-9]{1}|[0-9]{1}[1-9][0-9]{2}|[1-9][0-9]{3})(((0[13578]|1[02])(0[1-9]|[12][0-9]|3[01]))|((0[469]|11)(0[1-9]|[12][0-9]|30))|(02(0[1-9]|[1][0-9]|2[0-8])))", DateTime.Now.ToString("yyyyMMdd") + "79");
            if (searchday == "")
            {
                searchday = DateTime.Now.ToString("yyyyMMdd") + "01";
            }
            if (searchday2 == "")
            {
                searchday = DateTime.Now.ToString("yyyyMMdd") + "79";
            }
            DataSet profit = new BCW.HP3.BLL.HP3Buy().GetList("sum(WillGet-BuyMoney*BuyZhu)", "BuyID=" + meid + " and BuyDate>='" + searchday + "' and BuyDate<='" + searchday2 + "'");
            long myprofit = 0;
            try
            {
                myprofit = Convert.ToInt64(profit.Tables[0].Rows[0][0]);
            }
            catch
            {
                myprofit = 0;
            }
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("" + "-----------" + "<br />");
            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a style=\"color:blue\" href=\"" + Utils.getUrl("HP3.aspx?act=bydate") + "\">���ںŲ�ѯ���а�</a>");
            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("" + "-----------" + "<br />");
            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append("<b>�ҵ�ӯ����</b>" + myprofit + "<b>" + ub.Get("SiteBz") + "</b>");
            builder.Append(Out.Tab("</div>", ""));
            string strText = "��ʼ�ںţ���ʽ����" + DateTime.Now.ToString("yyyyMMdd") + "01��/,�����ںţ���ʽ����" + DateTime.Now.ToString("yyyyMMdd") + "79��/";
            string strName = "inputdate,inputdate2";
            string strType = "num,num";
            string strValu = searchday + "'" + searchday2;
            string strEmpt = "true,true";
            string strIdea = "";
            string strOthe = "��ѯ,HP3.aspx?act=top&amp;page=1,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

            builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
            builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">��ҳ</a>-");
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">��Ϸ����</a>-");
            builder.Append("<a href=\"" + Utils.getUrl("HP3.aspx") + "\">" + GameName + "</a>");

            builder.Append(Out.Tab("</div>", ""));
            #endregion
        }
        else
        {
            #region �����
            int meid = new BCW.User.Users().GetUsId();
            string GameName = ub.GetSub("HP3Name", xmlPath);
            Master.Title = "" + GameName + "���а�";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">��Ϸ����</a>&gt;<a href=\"" + Utils.getUrl("HP3.aspx") + "\">" + GameName + "</a>&gt;���а�");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            int tops = int.Parse(Utils.GetRequest("tops", "all", 1, "", "1"));

            if (tops == 1)
                builder.Append("���������ܰ�|");
            else
                builder.Append("<a href=\"" + Utils.getUrl("HP3.aspx?act=top&amp;tops=1") + "\">���������ܰ�</a>|");
            if (tops == 2)
                builder.Append("���������ܰ�");
            else
                builder.Append("<a href=\"" + Utils.getUrl("HP3.aspx?act=top&amp;tops=2") + "\">���������ܰ�</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
            int pageIndex = 0;//��ǰҳ
            int recordCount;//��¼������
            int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));//��ҳ��С
            string[] pageValUrl = { "act", "backurl" };
            pageIndex = Utils.ParseInt(Request.QueryString["page"]);
            if (pageIndex == 0)
                pageIndex = 1;
            if (tops == 1)
            {
                DataSet rowbang = new BCW.HP3.BLL.HP3WinnerSY().GetListBang();
                recordCount = rowbang.Tables[0].Rows.Count;
                DataSet bang = new BCW.HP3.BLL.HP3WinnerSY().GetListByPage(0, recordCount);//pageIndex * pageSize);
                if (recordCount >= 0)
                {
                    int k = 1;
                    int koo = (pageIndex - 1) * pageSize;
                    int skt = pageSize;
                    if (recordCount > koo + pageSize)
                    {
                        skt = pageSize;
                    }
                    else
                    {
                        skt = recordCount - koo;
                    }
                    for (int soms = 0; soms < skt; soms++)
                    {
                        int usid = Convert.ToInt32(bang.Tables[0].Rows[koo + soms][1]);
                        long usmoney = Convert.ToInt64(bang.Tables[0].Rows[koo + soms][2]);
                        if (k % 2 == 0)
                            builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                        else
                        {
                            if (k == 1)
                                builder.Append(Out.Tab("<div>", "<br />"));
                            else
                                builder.Append(Out.Tab("<div>", "<br />"));
                        }
                        string mename = new BCW.BLL.User().GetUsName(usid);
                        int wd = (pageIndex - 1) * 10 + k;
                        builder.Append("[<b style=\"color:red\">TOP" + wd + "</b>].<a  href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + usid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + mename + "(" + usid + ")</a>��׬��" + "<b style=\"color:red\">" + usmoney + "</b>" + "���ֱ�" + "");
                        k++;
                        builder.Append(Out.Tab("</div>", "<br />"));
                    }
                    builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
                }
                else
                {
                    builder.Append(Out.Div("div", "û����ؼ�¼.."));
                }
            }
            else
            {
                DataSet rowbang = new BCW.HP3.BLL.HP3BuySY().GetListBang();
                int som = rowbang.Tables[0].Rows.Count - 1;
                if (som >= 0)
                {
                    int k = 1;
                    recordCount = rowbang.Tables[0].Rows.Count;
                    int n = 0;
                    if (som >= 10)
                    {
                        n = som - 9;
                    }
                    builder.Append(Out.Tab("<div>", ""));
                    builder.Append("<b style=\"color:#8A7B66\">����Top10</b>");
                    builder.Append(Out.Tab("</div>", ""));
                    for (; som >= n; som--)
                    {
                        int usid = Convert.ToInt32(rowbang.Tables[0].Rows[som][0]);
                        long usmoney = Convert.ToInt64(rowbang.Tables[0].Rows[som][1]);
                        if (k % 2 == 0)
                            builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                        else
                        {
                            if (k == 1)
                                builder.Append(Out.Tab("<div>", "<br />"));
                            else
                                builder.Append(Out.Tab("<div>", "<br />"));
                        }
                        string mename = new BCW.BLL.User().GetUsName(usid);
                        builder.Append("[<b style=\"color:red\">TOP" + k + "</b>]<a  href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + usid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + mename + "(" + usid + ")</a>������" + "<b style=\"color:red\">" + usmoney + "</b>" + "���ֱ�" + "");
                        k++;
                        builder.Append(Out.Tab("</div>", "<br />"));
                    }
                }
                else
                {
                    builder.Append(Out.Div("div", "û����ؼ�¼.."));
                }

            }
            string searchday = Utils.GetRequest("inputdate", "all", 1, @"^([0-9]{3}[1-9]|[0-9]{2}[1-9][0-9]{1}|[0-9]{1}[1-9][0-9]{2}|[1-9][0-9]{3})(((0[13578]|1[02])(0[1-9]|[12][0-9]|3[01]))|((0[469]|11)(0[1-9]|[12][0-9]|30))|(02-(0[1-9]|[1][0-9]|2[0-8])))", DateTime.Now.ToString("yyyyMMdd") + "01");
            string searchday2 = Utils.GetRequest("inputdate2", "all", 1, @"^([0-9]{3}[1-9]|[0-9]{2}[1-9][0-9]{1}|[0-9]{1}[1-9][0-9]{2}|[1-9][0-9]{3})(((0[13578]|1[02])(0[1-9]|[12][0-9]|3[01]))|((0[469]|11)(0[1-9]|[12][0-9]|30))|(02-(0[1-9]|[1][0-9]|2[0-8])))", DateTime.Now.ToString("yyyyMMdd") + "79");
            if (searchday == "")
            {
                searchday = DateTime.Now.ToString("yyyyMMdd") + "01";
            }
            if (searchday2 == "")
            {
                searchday = DateTime.Now.ToString("yyyyMMdd") + "79";
            }
            DataSet profit = new BCW.HP3.BLL.HP3BuySY().GetList("sum(WillGet-BuyMoney*BuyZhu)", "BuyID=" + meid + " and BuyDate>='" + searchday + "' and BuyDate<='" + searchday2 + "'");
            long myprofit = 0;
            try
            {
                myprofit = Convert.ToInt64(profit.Tables[0].Rows[0][0]);
            }
            catch
            {
                myprofit = 0;
            }
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("<a style=\"color:blue\" href=\"" + Utils.getUrl("HP3.aspx?act=bydate") + "\">���ںŲ�ѯ���а�</a>");
            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("", Out.Hr()));
            builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
            builder.Append("<b>�ҵ�ӯ����</b>" + myprofit + "<b>" + "���ֱ�" + "</b><br />");
            builder.Append(Out.Tab("</div>", ""));
            string strText = "��ʼ�ںţ���ʽ����" + DateTime.Now.ToString("yyyyMMdd") + "01��/,�����ںţ���ʽ����" + DateTime.Now.ToString("yyyyMMdd") + "79��/";
            string strName = "inputdate,inputdate2";
            string strType = "num,num";
            string strValu = searchday + "'" + searchday2;
            string strEmpt = "true,true";
            string strIdea = "";
            string strOthe = "��ѯ,HP3.aspx?act=top&amp;page=1,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));


            builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
            builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">��ҳ</a>-");
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">��Ϸ����</a>-");
            builder.Append("<a href=\"" + Utils.getUrl("HP3.aspx") + "\">" + GameName + "</a>");

            builder.Append(Out.Tab("</div>", ""));
            #endregion
        }
    }
    //���ںŲ�ѯ���а�
    private void ByDateBang()
    {
        if (SWB == 0)
        {
            #region ��ʽ��
            string GameName = ub.GetSub("HP3Name", xmlPath);
            Master.Title = "" + GameName + "���а�";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">��Ϸ����</a>&gt;<a href=\"" + Utils.getUrl("HP3.aspx") + "\">" + GameName + "</a>&gt;���а�");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            int startstate = int.Parse(Utils.GetRequest("startstate", "all", 1, @"^([0-9]{3}[1-9]|[0-9]{2}[1-9][0-9]{1}|[0-9]{1}[1-9][0-9]{2}|[1-9][0-9]{3})(((0[13578]|1[02])(0[1-9]|[12][0-9]|3[01]))|((0[469]|11)(0[1-9]|[12][0-9]|30))|(02(0[1-9]|[1][0-9]|2[0-8])))", DateTime.Now.ToString("yyyyMMdd") + "01"));
            int endstate = int.Parse(Utils.GetRequest("endstate", "all", 1, @"^([0-9]{3}[1-9]|[0-9]{2}[1-9][0-9]{1}|[0-9]{1}[1-9][0-9]{2}|[1-9][0-9]{3})(((0[13578]|1[02])(0[1-9]|[12][0-9]|3[01]))|((0[469]|11)(0[1-9]|[12][0-9]|30))|(02(0[1-9]|[1][0-9]|2[0-8])))", DateTime.Now.ToString("yyyyMMdd") + "79"));

            int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, "", "1"));
            if (ptype == 1)
                builder.Append("���˸�����|");
            else
                builder.Append("<a href=\"" + Utils.getUrl("HP3.aspx?act=bydate&amp;ptype=1&amp;startstate=" + startstate + "&amp;endstate=" + endstate + "") + "\">���˸�����</a>|");

            if (ptype == 2)
                builder.Append("����������");
            else
                builder.Append("<a href=\"" + Utils.getUrl("HP3.aspx?act=bydate&amp;ptype=2&amp;startstate=" + startstate + "&amp;endstate=" + endstate + "") + "\">����������</a>");

            //if (ptype == 3)
            //    builder.Append("|��׬���˰�");
            //else
            //    builder.Append("|<a href=\"" + Utils.getUrl("HP3.aspx?act=bydate&amp;ptype=3&amp;startstate=" + startstate + "&amp;endstate=" + endstate + "") + "\">��׬���˰�</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
            if (ptype == 1)
            {
                int pageIndex;
                int recordCount = 100;
                string strWhere = string.Empty;
                int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
                string[] pageValUrl = { "act", "startstate", "endstate", "ptype", "backurl" };
                pageIndex = Utils.ParseInt(Request.QueryString["page"]);
                if (pageIndex == 0)
                    pageIndex = 1;
                if (pageIndex >= 10)
                    pageIndex = 10;
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("��" + startstate + "������" + endstate + "������Ϊ��");
                builder.Append(Out.Tab("</div>", ""));
                DataSet rowbang = new BCW.HP3.BLL.HP3Winner().GetListBang2(startstate.ToString(), endstate.ToString());
                recordCount = rowbang.Tables[0].Rows.Count;
                DataSet bang = new BCW.HP3.BLL.HP3Winner().GetListByPage2(0, recordCount, startstate.ToString(), endstate.ToString());
                if (recordCount >= 0)
                {
                    int k = 1;
                    int koo = (pageIndex - 1) * pageSize;
                    int skt = pageSize;
                    if (recordCount > koo + pageSize)
                    {
                        skt = pageSize;
                    }
                    else
                    {
                        skt = recordCount - koo;
                    }

                    for (int soms = 0; soms < skt; soms++)
                    {
                        int usid;
                        long usmoney;
                        usid = Convert.ToInt32(bang.Tables[0].Rows[koo + soms][1]);
                        usmoney = Convert.ToInt64(bang.Tables[0].Rows[koo + soms][2]);

                        if (k % 2 == 0)
                            builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                        else
                        {
                            if (k == 1)
                                builder.Append(Out.Tab("<div>", "<br />"));
                            else
                                builder.Append(Out.Tab("<div>", "<br />"));
                        }
                        string mename = new BCW.BLL.User().GetUsName(usid);
                        int wd = (pageIndex - 1) * 10 + k;
                        builder.Append("[<b style=\"color:red\">TOP" + wd + "</b>].<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + usid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + mename + "(" + usid + ")</a>��׬��" + "<b style=\"color:red\">" + usmoney + "</b>" + ub.Get("SiteBz") + "");
                        k++;
                        builder.Append(Out.Tab("</div>", ""));
                    }
                    int Pcount = 0;
                    if (recordCount <= 100)
                    {
                        Pcount = recordCount;
                    }
                    else
                    {
                        Pcount = 100;
                    }
                    builder.Append(BasePage.MultiPage(pageIndex, pageSize, Pcount, Utils.getPageUrl(), pageValUrl, "page", 0));
                }
                else
                {
                    builder.Append(Out.Div("div", "û����ؼ�¼.."));
                }
            }
            else if (ptype == 3)
            {
                //int startstate = int.Parse(Utils.GetRequest("startstate", "all", 1, @"^([0-9]{3}[1-9]|[0-9]{2}[1-9][0-9]{1}|[0-9]{1}[1-9][0-9]{2}|[1-9][0-9]{3})(((0[13578]|1[02])(0[1-9]|[12][0-9]|3[01]))|((0[469]|11)(0[1-9]|[12][0-9]|30))|(02(0[1-9]|[1][0-9]|2[0-8])))", DateTime.Now.ToString("yyyyMMdd") + "01"));
                //int endstate = int.Parse(Utils.GetRequest("endstate", "all", 1, @"^([0-9]{3}[1-9]|[0-9]{2}[1-9][0-9]{1}|[0-9]{1}[1-9][0-9]{2}|[1-9][0-9]{3})(((0[13578]|1[02])(0[1-9]|[12][0-9]|3[01]))|((0[469]|11)(0[1-9]|[12][0-9]|30))|(02(0[1-9]|[1][0-9]|2[0-8])))", DateTime.Now.ToString("yyyyMMdd") + "79"));
                int pageIndex;
                int recordCount;
                string strWhere = string.Empty;
                int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
                string[] pageValUrl = { "act", "startstate", "endstate", "ptype", "backurl" };
                pageIndex = Utils.ParseInt(Request.QueryString["page"]);
                if (pageIndex == 0)
                    pageIndex = 1;
                if (pageIndex >= 10)
                    pageIndex = 10;
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("��" + startstate + "������" + endstate + "������Ϊ��");
                builder.Append(Out.Tab("</div>", ""));
                DataSet rowbang = new BCW.HP3.BLL.HP3Buy().GetBang(startstate.ToString(), endstate.ToString());
                recordCount = rowbang.Tables[0].Rows.Count;
                DataSet bang = new BCW.HP3.BLL.HP3Buy().GetBangByPage(0, recordCount, startstate.ToString(), endstate.ToString());
                if (recordCount >= 0)
                {
                    int k = 1;
                    int koo = (pageIndex - 1) * pageSize;
                    int skt = pageSize;
                    if (recordCount > koo + pageSize)
                    {
                        skt = pageSize;
                    }
                    else
                    {
                        skt = recordCount - koo;
                    }

                    for (int soms = 0; soms < skt; soms++)
                    {
                        int usid;
                        long usmoney;
                        usid = Convert.ToInt32(bang.Tables[0].Rows[koo + soms][1]);
                        usmoney = Convert.ToInt64(bang.Tables[0].Rows[koo + soms][2]);
                        if (k % 2 == 0)
                            builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                        else
                        {
                            if (k == 1)
                                builder.Append(Out.Tab("<div>", "<br />"));
                            else
                                builder.Append(Out.Tab("<div>", "<br />"));
                        }
                        string mename = new BCW.BLL.User().GetUsName(usid);
                        int wd = (pageIndex - 1) * 10 + k;
                        builder.Append("[<b style=\"color:red\">TOP" + wd + "</b>].<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + usid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + mename + "(" + usid + ")</a>��׬��" + "<b style=\"color:red\">" + usmoney + "</b>" + ub.Get("SiteBz") + "");
                        k++;
                        builder.Append(Out.Tab("</div>", ""));
                    }
                    int Pcount = 0;
                    if (recordCount <= 100)
                    {
                        Pcount = recordCount;
                    }
                    else
                    {
                        Pcount = 100;
                    }
                    builder.Append(BasePage.MultiPage(pageIndex, pageSize, Pcount, Utils.getPageUrl(), pageValUrl, "page", 0));
                }
                else
                {
                    builder.Append(Out.Div("div", "û����ؼ�¼.."));
                }

            }
            else
            {
                //int startstate = int.Parse(Utils.GetRequest("startstate", "all", 1, @"^([0-9]{3}[1-9]|[0-9]{2}[1-9][0-9]{1}|[0-9]{1}[1-9][0-9]{2}|[1-9][0-9]{3})(((0[13578]|1[02])(0[1-9]|[12][0-9]|3[01]))|((0[469]|11)(0[1-9]|[12][0-9]|30))|(02-(0[1-9]|[1][0-9]|2[0-8])))", DateTime.Now.ToString("yyyyMMdd") + "01"));
                //int endstate = int.Parse(Utils.GetRequest("endstate", "all", 1, @"^([0-9]{3}[1-9]|[0-9]{2}[1-9][0-9]{1}|[0-9]{1}[1-9][0-9]{2}|[1-9][0-9]{3})(((0[13578]|1[02])(0[1-9]|[12][0-9]|3[01]))|((0[469]|11)(0[1-9]|[12][0-9]|30))|(02-(0[1-9]|[1][0-9]|2[0-8])))", DateTime.Now.ToString("yyyyMMdd") + "79"));
                int pageIndex;
                int recordCount;
                string strWhere = string.Empty;
                int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
                string[] pageValUrl = { "act", "startstate", "endstate", "ptype", "backurl" };
                pageIndex = Utils.ParseInt(Request.QueryString["page"]);
                if (pageIndex == 0)
                    pageIndex = 1;
                if (pageIndex >= 10)
                    pageIndex = 10;
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("��" + startstate + "������" + endstate + "������Ϊ��");
                builder.Append(Out.Tab("</div>", ""));
                DataSet rowbang = new BCW.HP3.BLL.HP3Buy().GetListBang2(startstate.ToString(), endstate.ToString());
                recordCount = rowbang.Tables[0].Rows.Count;
                DataSet bang = new BCW.HP3.BLL.HP3Buy().GetListByPage(0, recordCount, startstate.ToString(), endstate.ToString());
                if (recordCount >= 0)
                {
                    int k = 1;
                    int koo = (pageIndex - 1) * pageSize;
                    int skt = pageSize;
                    if (recordCount > koo + pageSize)
                    {
                        skt = pageSize;
                    }
                    else
                    {
                        skt = recordCount - koo;
                    }

                    for (int soms = 0; soms < skt; soms++)
                    {
                        int usid;
                        long usmoney;
                        usid = Convert.ToInt32(bang.Tables[0].Rows[koo + soms][1]);
                        usmoney = Convert.ToInt64(bang.Tables[0].Rows[koo + soms][2]);
                        if (k % 2 == 0)
                            builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                        else
                        {
                            if (k == 1)
                                builder.Append(Out.Tab("<div>", "<br />"));
                            else
                                builder.Append(Out.Tab("<div>", "<br />"));
                        }
                        string mename = new BCW.BLL.User().GetUsName(usid);
                        int wd = (pageIndex - 1) * 10 + k;
                        builder.Append("[<b style=\"color:red\">TOP" + wd + "</b>].<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + usid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + mename + "(" + usid + ")</a>������" + "<b style=\"color:red\">" + usmoney + "</b>" + ub.Get("SiteBz") + "");
                        k++;
                        builder.Append(Out.Tab("</div>", ""));
                    }
                    int Pcount = 0;
                    if (recordCount <= 100)
                    {
                        Pcount = recordCount;
                    }
                    else
                    {
                        Pcount = 100;
                    }
                    builder.Append(BasePage.MultiPage(pageIndex, pageSize, Pcount, Utils.getPageUrl(), pageValUrl, "page", 0));
                }
                else
                {
                    builder.Append(Out.Div("div", "û����ؼ�¼.."));
                }

            }

            string strText = "���뿪ʼ�ں�:/,�����ֹ�ں�:/,";
            string strName = "startstate,endstate,backurl";
            string strType = "num,num,hidden";
            string strValu = "" + startstate + "'" + endstate + "'" + Utils.getPage(0) + "";
            string strEmpt = "true,true,false";
            string strIdea = "/";
            string strOthe = "��һ��,HP3.aspx?act=bydate&amp;ptype=" + ptype + ",post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("", Out.Hr()));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("HP3.aspx?act=top") + "\">�����ϼ�</a>");
            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
            builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">��ҳ</a>-");
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">��Ϸ����</a>-");
            builder.Append("<a href=\"" + Utils.getUrl("HP3.aspx") + "\">" + GameName + "</a>");

            builder.Append(Out.Tab("</div>", ""));
            #endregion
        }
        else
        {
            #region �����
            string GameName = ub.GetSub("HP3Name", xmlPath);
            Master.Title = "" + GameName + "���а�";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">��Ϸ����</a>&gt;<a href=\"" + Utils.getUrl("HP3.aspx") + "\">" + GameName + "</a>&gt;���а�");
            builder.Append(Out.Tab("</div>", "<br />"));

            builder.Append(Out.Tab("<div>", ""));
            int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, "", "1"));
            if (ptype == 1)
                builder.Append("���������|");
            else
                builder.Append("<a href=\"" + Utils.getUrl("HP3.aspx?act=bydate&amp;ptype=1") + "\">���������</a>|");

            if (ptype == 2)
                builder.Append("���������|");
            else
                builder.Append("<a href=\"" + Utils.getUrl("HP3.aspx?act=bydate&amp;ptype=2") + "\">���������</a>|");

            if (ptype == 3)
                builder.Append("������˰�");
            else
                builder.Append("<a href=\"" + Utils.getUrl("HP3.aspx?act=bydate&amp;ptype=3") + "\">������˰�</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
            if (ptype == 1)
            {
                int startstate = int.Parse(Utils.GetRequest("startstate", "all", 1, @"^([0-9]{3}[1-9]|[0-9]{2}[1-9][0-9]{1}|[0-9]{1}[1-9][0-9]{2}|[1-9][0-9]{3})(((0[13578]|1[02])(0[1-9]|[12][0-9]|3[01]))|((0[469]|11)(0[1-9]|[12][0-9]|30))|(02(0[1-9]|[1][0-9]|2[0-8])))", DateTime.Now.ToString("yyyyMMdd") + "01"));
                int endstate = int.Parse(Utils.GetRequest("endstate", "all", 1, @"^([0-9]{3}[1-9]|[0-9]{2}[1-9][0-9]{1}|[0-9]{1}[1-9][0-9]{2}|[1-9][0-9]{3})(((0[13578]|1[02])(0[1-9]|[12][0-9]|3[01]))|((0[469]|11)(0[1-9]|[12][0-9]|30))|(02(0[1-9]|[1][0-9]|2[0-8])))", DateTime.Now.ToString("yyyyMMdd") + "79"));
                int pageIndex;
                int recordCount;
                string strWhere = string.Empty;
                int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
                string[] pageValUrl = { "act", "startstate", "endstate", "ptype", "backurl" };
                pageIndex = Utils.ParseInt(Request.QueryString["page"]);
                if (pageIndex == 0)
                    pageIndex = 1;
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("��" + startstate + "������" + endstate + "������Ϊ��");
                builder.Append(Out.Tab("</div>", ""));
                DataSet rowbang = new BCW.HP3.BLL.HP3WinnerSY().GetListBang2(startstate.ToString(), endstate.ToString());
                recordCount = rowbang.Tables[0].Rows.Count;
                DataSet bang = new BCW.HP3.BLL.HP3WinnerSY().GetListByPage2(0, recordCount, startstate.ToString(), endstate.ToString());
                if (recordCount >= 0)
                {
                    int k = 1;
                    int koo = (pageIndex - 1) * pageSize;
                    int skt = pageSize;
                    if (recordCount > koo + pageSize)
                    {
                        skt = pageSize;
                    }
                    else
                    {
                        skt = recordCount - koo;
                    }

                    for (int soms = 0; soms < skt; soms++)
                    {
                        int usid;
                        long usmoney;
                        usid = Convert.ToInt32(bang.Tables[0].Rows[koo + soms][1]);
                        usmoney = Convert.ToInt64(bang.Tables[0].Rows[koo + soms][2]);
                        if (k % 2 == 0)
                            builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                        else
                        {
                            if (k == 1)
                                builder.Append(Out.Tab("<div>", "<br />"));
                            else
                                builder.Append(Out.Tab("<div>", "<br />"));
                        }
                        string mename = new BCW.BLL.User().GetUsName(usid);
                        int wd = (pageIndex - 1) * 10 + k;
                        builder.Append("[<b style=\"color:red\">TOP" + wd + "</b>].<a  href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + usid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + mename + "(" + usid + ")</a>��׬��" + "<b style=\"color:red\">" + usmoney + "</b>" + "���ֱ�" + "");
                        k++;
                        builder.Append(Out.Tab("</div>", ""));
                    }
                    builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
                }
                else
                {
                    builder.Append(Out.Div("div", "û����ؼ�¼.."));
                }
            }
            else if (ptype == 3)
            {
                int startstate = int.Parse(Utils.GetRequest("startstate", "all", 1, @"^([0-9]{3}[1-9]|[0-9]{2}[1-9][0-9]{1}|[0-9]{1}[1-9][0-9]{2}|[1-9][0-9]{3})(((0[13578]|1[02])(0[1-9]|[12][0-9]|3[01]))|((0[469]|11)(0[1-9]|[12][0-9]|30))|(02(0[1-9]|[1][0-9]|2[0-8])))", DateTime.Now.ToString("yyyyMMdd") + "01"));
                int endstate = int.Parse(Utils.GetRequest("endstate", "all", 1, @"^([0-9]{3}[1-9]|[0-9]{2}[1-9][0-9]{1}|[0-9]{1}[1-9][0-9]{2}|[1-9][0-9]{3})(((0[13578]|1[02])(0[1-9]|[12][0-9]|3[01]))|((0[469]|11)(0[1-9]|[12][0-9]|30))|(02(0[1-9]|[1][0-9]|2[0-8])))", DateTime.Now.ToString("yyyyMMdd") + "79"));
                int pageIndex;
                int recordCount;
                string strWhere = string.Empty;
                int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
                string[] pageValUrl = { "act", "startstate", "endstate", "ptype", "backurl" };
                pageIndex = Utils.ParseInt(Request.QueryString["page"]);
                if (pageIndex == 0)
                    pageIndex = 1;
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("��" + startstate + "������" + endstate + "������Ϊ��");
                builder.Append(Out.Tab("</div>", ""));
                DataSet rowbang = new BCW.HP3.BLL.HP3BuySY().GetBang(startstate.ToString(), endstate.ToString());
                recordCount = rowbang.Tables[0].Rows.Count;
                DataSet bang = new BCW.HP3.BLL.HP3BuySY().GetBangByPage(0, recordCount, startstate.ToString(), endstate.ToString());
                if (recordCount >= 0)
                {
                    int k = 1;
                    int koo = (pageIndex - 1) * pageSize;
                    int skt = pageSize;
                    if (recordCount > koo + pageSize)
                    {
                        skt = pageSize;
                    }
                    else
                    {
                        skt = recordCount - koo;
                    }

                    for (int soms = 0; soms < skt; soms++)
                    {
                        int usid;
                        long usmoney;
                        usid = Convert.ToInt32(bang.Tables[0].Rows[koo + soms][1]);
                        usmoney = Convert.ToInt64(bang.Tables[0].Rows[koo + soms][2]);
                        if (k % 2 == 0)
                            builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                        else
                        {
                            if (k == 1)
                                builder.Append(Out.Tab("<div>", "<br />"));
                            else
                                builder.Append(Out.Tab("<div>", "<br />"));
                        }
                        string mename = new BCW.BLL.User().GetUsName(usid);
                        int wd = (pageIndex - 1) * 10 + k;
                        builder.Append("[<b style=\"color:red\">TOP" + wd + "</b>].<a  href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + usid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + mename + "(" + usid + ")</a>��׬��" + "<b style=\"color:red\">" + usmoney + "</b>" + "���ֱ�" + "");
                        k++;
                        builder.Append(Out.Tab("</div>", ""));
                    }
                    builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
                }
                else
                {
                    builder.Append(Out.Div("div", "û����ؼ�¼.."));
                }

            }
            else
            {
                int startstate = int.Parse(Utils.GetRequest("startstate", "all", 1, @"^([0-9]{3}[1-9]|[0-9]{2}[1-9][0-9]{1}|[0-9]{1}[1-9][0-9]{2}|[1-9][0-9]{3})(((0[13578]|1[02])(0[1-9]|[12][0-9]|3[01]))|((0[469]|11)(0[1-9]|[12][0-9]|30))|(02(0[1-9]|[1][0-9]|2[0-8])))", DateTime.Now.ToString("yyyyMMdd") + "01"));
                int endstate = int.Parse(Utils.GetRequest("endstate", "all", 1, @"^([0-9]{3}[1-9]|[0-9]{2}[1-9][0-9]{1}|[0-9]{1}[1-9][0-9]{2}|[1-9][0-9]{3})(((0[13578]|1[02])(0[1-9]|[12][0-9]|3[01]))|((0[469]|11)(0[1-9]|[12][0-9]|30))|(02(0[1-9]|[1][0-9]|2[0-8])))", DateTime.Now.ToString("yyyyMMdd") + "79"));
                int pageIndex;
                int recordCount;
                string strWhere = string.Empty;
                int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
                string[] pageValUrl = { "act", "startstate", "endstate", "ptype", "backurl" };
                pageIndex = Utils.ParseInt(Request.QueryString["page"]);
                if (pageIndex == 0)
                    pageIndex = 1;
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("��" + startstate + "������" + endstate + "������Ϊ��");
                builder.Append(Out.Tab("</div>", ""));
                DataSet rowbang = new BCW.HP3.BLL.HP3BuySY().GetListBang2(startstate.ToString(), endstate.ToString());
                recordCount = rowbang.Tables[0].Rows.Count;
                DataSet bang = new BCW.HP3.BLL.HP3BuySY().GetListByPage(0, recordCount, startstate.ToString(), endstate.ToString());
                if (recordCount >= 0)
                {
                    int k = 1;
                    int koo = (pageIndex - 1) * pageSize;
                    int skt = pageSize;
                    if (recordCount > koo + pageSize)
                    {
                        skt = pageSize;
                    }
                    else
                    {
                        skt = recordCount - koo;
                    }

                    for (int soms = 0; soms < skt; soms++)
                    {
                        int usid;
                        long usmoney;
                        usid = Convert.ToInt32(bang.Tables[0].Rows[koo + soms][1]);
                        usmoney = Convert.ToInt64(bang.Tables[0].Rows[koo + soms][2]);
                        if (k % 2 == 0)
                            builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                        else
                        {
                            if (k == 1)
                                builder.Append(Out.Tab("<div>", "<br />"));
                            else
                                builder.Append(Out.Tab("<div>", "<br />"));
                        }
                        string mename = new BCW.BLL.User().GetUsName(usid);
                        int wd = (pageIndex - 1) * 10 + k;
                        builder.Append("[<b style=\"color:red\">TOP" + wd + "</b>].<a  href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + usid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + mename + "(" + usid + ")</a>������" + "<b style=\"color:red\">" + usmoney + "</b>" + "���ֱ�" + "");
                        k++;
                        builder.Append(Out.Tab("</div>", ""));
                    }
                    builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
                }
                else
                {
                    builder.Append(Out.Div("div", "û����ؼ�¼.."));
                }

            }

            string strText = "���뿪ʼ�ں�:/,�����ֹ�ں�:/,";
            string strName = "startstate,endstate,backurl";
            string strType = "num,num,hidden";
            string strValu = "'" + "'" + Utils.getPage(0) + "";
            string strEmpt = "true,true,false";
            string strIdea = "/";
            string strOthe = "��һ��,HP3.aspx?act=bydate&amp;ptype=" + ptype + ",post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

            builder.Append(Out.Tab("", Out.Hr()));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("HP3.aspx?act=top") + "\">�����ϼ�</a>");
            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
            builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">��ҳ</a>-");
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">��Ϸ����</a>-");
            builder.Append("<a href=\"" + Utils.getUrl("HP3.aspx") + "\">" + GameName + "</a>");

            builder.Append(Out.Tab("</div>", ""));
            #endregion
        }

    }
    //����ѡ�񷵻�
    private string speChoose(int Types)
    {
        string s1 = string.Empty;
        if (Types == 1)
            s1 = "<b>����ͬ��</b>";
        else if (Types == 2)
            s1 = "<b style=\"color:red\">����ͬ��</b>";
        else if (Types == 3)
            s1 = "<b>÷��ͬ��</b>";
        else if (Types == 4)
            s1 = "<b style=\"color:red\">����ͬ��</b>";
        else if (Types == 5)
            s1 = "<b style=\"color:#8A7B66\">ͬ��ȫ��</b>";
        else if (Types == 6)
            s1 = "<b>A23</b>";
        else if (Types == 7)
            s1 = "<b>234</b>";
        else if (Types == 8)
            s1 = "<b>345</b>";
        else if (Types == 9)
            s1 = "<b>456</b>";
        else if (Types == 10)
            s1 = "<b>567</b>";
        else if (Types == 11)
            s1 = "<b>678</b>";
        else if (Types == 12)
            s1 = "<b>789</b>";
        else if (Types == 13)
            s1 = "<b>8910</b>";
        else if (Types == 14)
            s1 = "<b>910J</b>";
        else if (Types == 15)
            s1 = "<b>10JQ</b>";
        else if (Types == 16)
            s1 = "<b>JQK</b>";
        else if (Types == 17)
            s1 = "<b>QKA</b>";
        else if (Types == 18)
            s1 = "<b style=\"color:#8A7B66\">˳��ȫ��</b>";
        else if (Types == 19)
            s1 = "<b>����ͬ��˳</b>";
        else if (Types == 20)
            s1 = "<b style=\"color:red\">����ͬ��˳</b>";
        else if (Types == 21)
            s1 = "<b>÷��ͬ��˳</b>";
        else if (Types == 22)
            s1 = "<b style=\"color:red\">����ͬ��˳</b>";
        else if (Types == 23)
            s1 = " <b style=\"color:#8A7B66\">ͬ��˳ȫ��</b>";
        else if (Types == 24)
            s1 = "<b>AAA</b>";
        else if (Types == 25)
            s1 = "<b>222</b>";
        else if (Types == 26)
            s1 = "<b>333</b>";
        else if (Types == 27)
            s1 = "<b>444</b>";
        else if (Types == 28)
            s1 = "<b>555</b>";
        else if (Types == 29)
            s1 = "<b>666</b>";
        else if (Types == 30)
            s1 = "<b>777</b>";
        else if (Types == 31)
            s1 = "<b>888</b>";
        else if (Types == 32)
            s1 = "<b>999</b>";
        else if (Types == 33)
            s1 = "<b>101010</b>";
        else if (Types == 34)
            s1 = "<b>JJJ</b>";
        else if (Types == 35)
            s1 = "<b>QQQ</b>";
        else if (Types == 36)
            s1 = "<b>KKK</b>";
        else if (Types == 37)
            s1 = "<b style=\"color:#8A7B66\">����ȫ��</b>";
        else if (Types == 38)
            s1 = "<b>AA</b>";
        else if (Types == 39)
            s1 = "<b>22</b>";
        else if (Types == 40)
            s1 = "<b>33</b>";
        else if (Types == 41)
            s1 = "<b>44</b>";
        else if (Types == 42)
            s1 = "<b>55</b>";
        else if (Types == 43)
            s1 = "<b>66</b>";
        else if (Types == 44)
            s1 = "<b>77</b>";
        else if (Types == 45)
            s1 = "<b>88</b>";
        else if (Types == 46)
            s1 = "<b>99</b>";
        else if (Types == 47)
            s1 = "<b>1010</b>";
        else if (Types == 48)
            s1 = "<b>JJ</b>";
        else if (Types == 49)
            s1 = "<b>QQ</b>";
        else if (Types == 50)
            s1 = "<b>KK</b>";
        else if (Types == 51)
            s1 = "<b style=\"color:#8A7B66\">����ȫ��</b>";
        return s1;
    }
    //��Ϸ����
    private void RulePage()
    {
        string GameName = ub.GetSub("HP3Name", xmlPath);
        Master.Title = "��Ϸ����";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">��Ϸ����</a>&gt;<a href=\"" + Utils.getUrl("HP3.aspx") + "\">" + GameName + "</a>&gt;��Ϸ����");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        //builder.Append("=" + GameName + "Ͷע���н�����=");
        //builder.Append(Out.Tab("</div>", "<br />"));
        //builder.Append(Out.Tab("<div>", ""));
        //builder.Append("<b>��" + GameName + "���ݸ�����Ʊ("+GameName+")ת����������Ϸ,����ʱ�䡢�������" + GameName + "��ͬ.</b><br />" + GameName + "��ָ��52�ţ����ҡ����ҡ�÷��������A-K��13�ţ��˿����е�����3���������Ͻ���Ͷע��ÿ��Ͷע��ϳ�Ϊһע��<br />ÿ�ڴ�52���˿����п���3����Ϊ�н����롣<br />" + GameName + "�淨�Ǿ���3���˿˵����ͺ����֡�<br />" + GameName + "  10���ӿ���,ÿ��79�ڣ���Ϸ����ʱ�䣺8:50��22:00��<br />");
        //builder.Append("<b style=\"color:#f33\">Ͷע����</b><br />");
        //builder.Append("<b>һ����ɫͶע��</b><br />��ָ������������ͬ��ɫ�ĺ������Ͷע�������Ϊ��<br />1. ͬ����ѡ�������л�ɫ��ͬ�ĺ�����Ͻ���Ͷע��<br />2. ͬ����ѡ����ĳһ�֣�����ң���ɫ��ͬ�ĺ������Ͷע��<br />");
        //builder.Append("<b>��������Ͷע��</b><br />��ָ���������������ĺ��루���ޣ�A23��234��345��456��567��678��789��89 10��9 10 J��10 JQ��JQK��QKA������Ͷע�������Ϊ��<br />1. ͬ��˳��ѡ�������л�ɫ��ͬ�������Ž���Ͷע��<br />2. ͬ��˳��ѡ����ĳһ�ֻ�ɫ�������Ž���Ͷע��<br />3. ˳�Ӱ�ѡ�������������ţ����ֻ�ɫ������Ͷע��<br />4. ˳�ӵ�ѡ����ĳһ�������ţ����ֻ�ɫ������Ͷע��<br />");
        //builder.Append("<b>����ͬ��Ͷע��</b><br />��ָ�԰���������������ͬ�������Ͻ���Ͷע�������Ϊ��<br />1. ���Ӱ�ѡ�������С����Ӻ��롱��3���������ͬ������Ͷע��<br />2. ���ӵ�ѡ��ѡ��ĳһ�������Ӻ��롱����Ͷע��<br />3. ���Ӱ�ѡ�������С����Ӻ��롱��3������������ֻ��2��������ͬ������Ͻ���Ͷע��<br />4. ���ӵ�ѡ���԰���ĳ������ͬ��������С����Ӻ��롱����Ͻ���Ͷע��<br />");
        //builder.Append("<b>�ġ���ѡͶע��</b><br />��ָ��A-K�����ֻ�ɫ����13����������ѡ1-6������Ͷע�������Ϊ��<br />1. ��ѡһ����A-K�й�13����������ѡ1���������Ͷע��<br />2. ��ѡ������A-K�й�13����������ѡ2����ͬ�������Ͷע��<br />3. ��ѡ������A-K�й�13����������ѡ3����ͬ�������Ͷע��<br />4. ��ѡ�ģ���A-K�й�13����������ѡ4����ͬ�������Ͷע��<br />5. ��ѡ�壺��A-K�й�13����������ѡ5����ͬ�������Ͷע��<br />6. ��ѡ������A-K�й�13����������ѡ6����ͬ�������Ͷע��<br />");
        //builder.Append("<b>�塢��С��˫Ͷע��</b><br />Ϊ����ǿ��Ϸ�Կᱬ�ر��Ƴ���С��˫�淨��<br />1.��С�淨��<br />Ѻע�����Ƶĺ����ֵβ����С��С��ָ0��1��2��3��4������ָ5��6��7��8��9��(AΪ1��10��J��Q��KΪ0,��������,��Ѻ���ѺС����!)��<br />2.��˫�淨��<br />Ѻע�����Ƶĺ����ֵβ����˫��˫��ָ0��2��4��6��8������ָ1��3��5��7��9��(AΪ1��10��J��Q��KΪ0,��������,��Ѻ����Ѻ˫����!)<br />");
        //builder.Append("<b style=\"color:#f33\">�н�����</b><br />");
        //builder.Append("<b>" + GameName + "����ͬͶע��ʽ�轱����Ϊ�̶���������涨���£�</b><br/>");
        //builder.Append("һ����ɫͶע<br />1. ͬ����ѡ����ע�̶�����<b style=\"color:red\">" + XML(1) + "</b>��;<br />2. ͬ����ѡ����ע�̶�����<b style=\"color:red\">" + XML(2) + "</b>����<br />��������Ͷע<br />1. ͬ��˳��ѡ����ע�̶�����<b style=\"color:red\">" + XML(3) + "</b>��;<br />2. ͬ��˳��ѡ����ע�̶�����<b style=\"color:red\">" + XML(4) + "</b>��;<br />3. ˳�Ӱ�ѡ����ע�̶�����<b style=\"color:red\">" + XML(5) + "</b>��;<br />4. ˳�ӵ�ѡ����ע�̶�����<b style=\"color:red\">" + XML(6) + "</b>����<br />����ͬ��Ͷע<br />1. ���Ӱ�ѡ����ע�̶�����<b style=\"color:red\">" + XML(7) + "</b>��;<br />2. ���ӵ�ѡ����ע�̶�����<b style=\"color:red\">" + XML(8) + "</b>��;<br />3. ���Ӱ�ѡ����ע�̶�����<b style=\"color:red\">" + XML(9) + "</b>��;<br />4. ���ӵ�ѡ����ע�̶�����<b style=\"color:red\">" + XML(10) + "</b>����<br />�ġ���ѡͶע<br />1. ��ѡһ��1����ע�̶�����<b style=\"color:red\">" + XML(11) + "</b>��;<br />2. ��ѡ����2����ע�̶�����<b style=\"color:red\">" + XML(12) + "</b>��;<br />3. ��ѡ����3����ע�̶�����<b style=\"color:red\">" + XML(13) + "</b>��;<br />4. ��ѡ����3����ע�̶�����<b style=\"color:red\">" + XML(14) + "</b>��;<br />5. ��ѡ����3����ע�̶�����<b style=\"color:red\">" + XML(15) + "</b>��;<br />6. ��ѡ����3����ע�̶�����<b style=\"color:red\">" + XML(16) + "</b>����<br />");
        //builder.Append("�塢��С��˫Ͷע<br />Ϊ�������ʣ������������Ϸҳ����Ϊ׼��<br />");

        builder.Append(Out.SysUBB(ub.GetSub("HP3OnTime", xmlPath)));//����ԭ��xml�޹���XML������Ϸʱ�䲻��Ҫ�����԰ѹ�����HP3OnTime����

        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", Out.RHr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">��ҳ</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">��Ϸ</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("HP3.aspx") + "\">" + GameName + "</a>");
        builder.Append(Out.Tab("</div>", ""));
    }
    //������ʷ
    private void ListPage()
    {
        string GameName = ub.GetSub("HP3Name", xmlPath);
        Master.Title = "��ʷ����";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">��Ϸ����</a>&gt;<a href=\"" + Utils.getUrl("HP3.aspx") + "\">" + GameName + "</a>&gt;��ʷ����");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("����ʷ������");
        builder.Append(Out.Tab("</div>", "<br />"));

        if (SWB == 0)
        {
            #region ��ʽ��
            int pageIndex;
            int recordCount;
            int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
            string strWhere = string.Empty;
            strWhere = "Fnum!='null'";
            string[] pageValUrl = { "act", "backurl" };
            pageIndex = Utils.ParseInt(Request.QueryString["page"]);
            if (pageIndex == 0)
                pageIndex = 1;

            // ��ʼ��ȡ�б�
            IList<BCW.HP3.Model.HP3_kjnum> listHP3 = new BCW.HP3.BLL.HP3_kjnum().GetHP3ListByPage(pageIndex, pageSize, strWhere, out recordCount);
            if (listHP3.Count > 0)
            {
                int k = 1;
                foreach (BCW.HP3.Model.HP3_kjnum n in listHP3)
                {
                    if (k % 2 == 0)
                        builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                    else
                    {
                        if (k == 1)
                            builder.Append(Out.Tab("<div>", ""));
                        else
                            builder.Append(Out.Tab("<div>", "<br />"));
                    }

                    builder.Append("<a href=\"" + Utils.getUrl("HP3.aspx?act=listview&amp;id=" + n.datenum + "") + "\">" + n.datenum + "��:" + n.Fnum + n.Snum + n.Tnum + "</a>");
                    k++;
                    builder.Append(Out.Tab("</div>", ""));
                }

                // ��ҳ
                builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));

            }
            else
            {
                builder.Append(Out.Div("div", "û����ؼ�¼.."));
            }
            #endregion
        }
        else
        {
            #region �����
            int pageIndex;
            int recordCount;
            int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
            string strWhere = string.Empty;
            strWhere = "Fnum!='null'";
            string[] pageValUrl = { "act", "backurl" };
            pageIndex = Utils.ParseInt(Request.QueryString["page"]);
            if (pageIndex == 0)
                pageIndex = 1;

            // ��ʼ��ȡ�б�
            IList<BCW.HP3.Model.HP3_kjnum> listHP3 = new BCW.HP3.BLL.HP3_kjnum().GetHP3ListByPage(pageIndex, pageSize, strWhere, out recordCount);
            if (listHP3.Count > 0)
            {
                int k = 1;
                foreach (BCW.HP3.Model.HP3_kjnum n in listHP3)
                {
                    if (k % 2 == 0)
                        builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                    else
                    {
                        if (k == 1)
                            builder.Append(Out.Tab("<div>", ""));
                        else
                            builder.Append(Out.Tab("<div>", "<br />"));
                    }

                    builder.Append("<a href=\"" + Utils.getUrl("HP3.aspx?act=listview&amp;id=" + n.datenum + "") + "\">" + n.datenum + "��:" + n.Fnum + n.Snum + n.Tnum + "</a>");
                    k++;
                    builder.Append(Out.Tab("</div>", ""));
                }

                // ��ҳ
                builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));

            }
            else
            {
                builder.Append(Out.Div("div", "û����ؼ�¼.."));
            }
            #endregion
        }

        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">��ҳ</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">��Ϸ</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("HP3.aspx") + "\">" + GameName + "</a>");
        builder.Append(Out.Tab("</div>", ""));
    }
    //������ʷ����
    private void ListViewPage()
    {
        string GameName = ub.GetSub("HP3Name", xmlPath);
        int id = Utils.ParseInt(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "ID����"));
        Master.Title = "��" + id.ToString() + "��Ͷע";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">��Ϸ����</a>&gt;<a href=\"" + Utils.getUrl("HP3.aspx") + "\">" + GameName + "</a>&gt;��" + id.ToString() + "��");
        builder.Append(Out.Tab("</div>", "<br />"));

        if (SWB == 0)
        {
            #region ��ʽ��
            bool s = new BCW.HP3.BLL.HP3Winner().Exists2(id.ToString());
            if (s)
            {
                DataSet mool = new BCW.HP3.BLL.HP3_kjnum().GetList("datenum='" + id + "'");
                BCW.HP3.Model.HP3_kjnum ThatNum = new BCW.HP3.Model.HP3_kjnum();
                ThatNum.datenum = Convert.ToString(mool.Tables[0].Rows[0][0]);
                ThatNum.Fnum = Convert.ToString(mool.Tables[0].Rows[0][1]);
                ThatNum.Snum = Convert.ToString(mool.Tables[0].Rows[0][2]);
                ThatNum.Tnum = Convert.ToString(mool.Tables[0].Rows[0][3]);
                BCW.HP3.Model.HP3Winner noo = new BCW.HP3.Model.HP3Winner();
                DataSet rows = new BCW.HP3.BLL.HP3Winner().GetList("WinDate=" + id);
                //  int n = rows.Tables[0].Rows.Count - 1;
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("��<b style=\"color:red\">" + id + "</b>�ڿ���:<b>" + ThatNum.Fnum + ThatNum.Snum + ThatNum.Tnum + "</b>");
                builder.Append("<br />��<b style=\"color:red\">" + rows.Tables[0].Rows.Count + "</b>���н�");
                builder.Append(Out.Tab("</div>", "<br />"));

                //int k = 1;
                //for (; n >= 0; n--)
                //{
                //    noo.ID = Convert.ToInt32(rows.Tables[0].Rows[n][0]);
                //    noo.WinUserID = Convert.ToInt32(rows.Tables[0].Rows[n][1]);
                //    noo.WinDate = Convert.ToString(rows.Tables[0].Rows[n][2]);
                //    noo.WinMoney = Convert.ToInt64(rows.Tables[0].Rows[n][3]);
                //    noo.WinBool = Convert.ToInt32(rows.Tables[0].Rows[n][4]);
                //    if (k % 2 == 0)
                //        builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                //    else
                //    {
                //        if (k == 1)
                //            builder.Append(Out.Tab("<div>", ""));
                //        else
                //            builder.Append(Out.Tab("<div>", "<br />"));
                //    }
                //    string mename = new BCW.BLL.User().GetUsName(noo.WinUserID);
                //    builder.Append(k + ".<a  href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + noo.WinUserID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + mename + "(" + noo.WinUserID + ")</a>" + "Ӯ<b style=\"color:red\">" + noo.WinMoney + "</b>" + ub.Get("SiteBz") + "");
                //    k++;
                //    builder.Append(Out.Tab("</div>", ""));

                //}

                #region  �����ݿ��е������������ļ�¼
                int pageIndex;
                int recordCount;
                int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
                string strWhere = string.Empty;
                strWhere += "WinDate=" + id + " ";

                string[] pageValUrl = { "act", "id", "WinDate", "datenum", "backurl" };
                pageIndex = Utils.ParseInt(Request.QueryString["page"]);
                if (pageIndex == 0)
                    pageIndex = 1;
                // ��ʼ��ȡ�б�
                IList<BCW.HP3.Model.HP3Winner> listhp3winner = new BCW.HP3.BLL.HP3Winner().GetListNes(pageIndex, pageSize, strWhere, out recordCount);
                if (listhp3winner.Count > 0)
                {
                    int k = 1;
                    foreach (BCW.HP3.Model.HP3Winner n in listhp3winner)
                    {
                        if (k % 2 == 0)
                            builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                        else
                        {
                            if (k == 1)
                                builder.Append(Out.Tab("<div>", ""));
                            else
                                builder.Append(Out.Tab("<div>", "<br />"));
                        }
                        string mename = new BCW.BLL.User().GetUsName(n.WinUserID);
                        builder.Append("" + ((pageIndex - 1) * pageSize + k) + ".<a  href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.WinUserID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + mename + "(" + n.WinUserID + ")</a>" + "Ӯ<b style=\"color:red\">" + n.WinMoney + "</b>" + ub.Get("SiteBz") + "");
                        k++;
                        builder.Append(Out.Tab("</div>", ""));
                    }
                    // ��ҳ
                    builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
                }
                else
                {
                    //builder.Append(Out.Tab("<div class=\"text\">", ""));
                    //builder.Append("��" + model.klsfId + "�ڿ���:<b>" + model.Result + "|��:" + GetHe(model.Result) + "</b>");
                    //builder.Append(Out.Tab("</div>", "<br />"));
                    //builder.Append(Out.Div("div", "û�з��ʻ�����ע��¼.."));
                }
                #endregion

            }
            else
            {
                builder.Append(Out.Div("div", "û�з��ʻ�����ע��¼.."));
            }
            #endregion
        }
        else
        {
            #region �����
            bool s = new BCW.HP3.BLL.HP3WinnerSY().Exists2(id.ToString());
            if (s)
            {
                DataSet mool = new BCW.HP3.BLL.HP3_kjnum().GetList("datenum='" + id + "'");
                BCW.HP3.Model.HP3_kjnum ThatNum = new BCW.HP3.Model.HP3_kjnum();
                ThatNum.datenum = Convert.ToString(mool.Tables[0].Rows[0][0]);
                ThatNum.Fnum = Convert.ToString(mool.Tables[0].Rows[0][1]);
                ThatNum.Snum = Convert.ToString(mool.Tables[0].Rows[0][2]);
                ThatNum.Tnum = Convert.ToString(mool.Tables[0].Rows[0][3]);
                BCW.HP3.Model.HP3WinnerSY noo = new BCW.HP3.Model.HP3WinnerSY();
                DataSet rows = new BCW.HP3.BLL.HP3WinnerSY().GetList("WinDate=" + id);
                int n = rows.Tables[0].Rows.Count - 1;
                builder.Append("��<b style=\"color:red\">" + id + "</b>�ڿ���:<b>" + ThatNum.Fnum + ThatNum.Snum + ThatNum.Tnum + "</b>");
                builder.Append("<br />��<b style=\"color:red\">" + rows.Tables[0].Rows.Count + "</b>���н�");
                int k = 1;
                for (; n >= 0; n--)
                {
                    noo.ID = Convert.ToInt32(rows.Tables[0].Rows[n][0]);
                    noo.WinUserID = Convert.ToInt32(rows.Tables[0].Rows[n][1]);
                    noo.WinDate = Convert.ToString(rows.Tables[0].Rows[n][2]);
                    noo.WinMoney = Convert.ToInt64(rows.Tables[0].Rows[n][3]);
                    noo.WinBool = Convert.ToInt32(rows.Tables[0].Rows[n][4]);
                    if (k % 2 == 0)
                        builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                    else
                    {
                        if (k == 1)
                            builder.Append(Out.Tab("<div>", ""));
                        else
                            builder.Append(Out.Tab("<div>", "<br />"));
                    }
                    string mename = new BCW.BLL.User().GetUsName(noo.WinUserID);
                    builder.Append(k + ".<a>" + mename + "(" + noo.WinUserID + ")</a>" + "Ӯ<b style=\"color:red\">" + noo.WinMoney + "</b>" + "���ֱ�" + "");
                    k++;
                    builder.Append(Out.Tab("</div>", ""));

                }

            }
            else
            {
                builder.Append(Out.Div("div", "û�з��ʻ�����ע��¼.."));
            }
            #endregion
        }

        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("<a href=\"" + Utils.getUrl("HP3.aspx?act=list") + "\">��ʷ����</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">��ҳ</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">��Ϸ</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("HP3.aspx") + "\">" + GameName + "</a>");
        builder.Append(Out.Tab("</div>", ""));

    }
    //����ʱ�����
    private bool IsOpen()
    {
        bool IsOpen = true;
        //string OnTime = ub.GetSub("HP3OnTime", xmlPath);
        //if (OnTime != "")
        //{
        //    if (Utils.IsRegex(OnTime, @"^[0-9]{2}:[0-9]{2}-[0-9]{2}:[0-9]{2}$"))
        //    {
        //        string[] temp = OnTime.Split("-".ToCharArray());
        //        DateTime dt1 = Convert.ToDateTime(temp[0]);
        //        DateTime dt2 = Convert.ToDateTime(temp[1]);
        //        if (DateTime.Now > dt1 && DateTime.Now < dt2)
        //        {
        //            IsOpen = true;
        //        }
        //        else
        //        {
        //            IsOpen = false;
        //        }
        //    }
        //}
        return IsOpen;
    }
    //��ȡxml������
    private string XML(int Types)
    {
        string xml = null;
        switch (Types)
        {
            case 1:
                xml = ub.GetSub("HP3TH1", xmlPath);
                break;
            case 2:
                xml = ub.GetSub("HP3TH2", xmlPath);
                break;
            case 3:
                xml = ub.GetSub("HP3THS1", xmlPath);
                break;
            case 4:
                xml = ub.GetSub("HP3THS2", xmlPath);
                break;
            case 5:
                xml = ub.GetSub("HP3SZ1", xmlPath);
                break;
            case 6:
                xml = ub.GetSub("HP3SZ2", xmlPath);
                break;
            case 7:
                xml = ub.GetSub("HP3BZ1", xmlPath);
                break;
            case 8:
                xml = ub.GetSub("HP3BZ2", xmlPath);
                break;
            case 9:
                xml = ub.GetSub("HP3DZ1", xmlPath);
                break;
            case 10:
                xml = ub.GetSub("HP3DZ2", xmlPath);
                break;
            case 11:
                xml = ub.GetSub("HP3RX1", xmlPath);
                break;
            case 12:
                xml = ub.GetSub("HP3RX2", xmlPath);
                break;
            case 13:
                xml = ub.GetSub("HP3RX3", xmlPath);
                break;
            case 14:
                xml = ub.GetSub("HP3RX4", xmlPath);
                break;
            case 15:
                xml = ub.GetSub("HP3RX5", xmlPath);
                break;
            case 16:
                xml = ub.GetSub("HP3RX6", xmlPath);
                break;
            case 21:
                xml = ub.GetSub("HP3DA", xmlPath);
                break;
            case 22:
                xml = ub.GetSub("HP3XIAO", xmlPath);
                break;
            case 23:
                xml = ub.GetSub("HP3DAN", xmlPath);
                break;
            case 24:
                xml = ub.GetSub("HP3SHUANG", xmlPath);
                break;
            case 25:
                xml = ub.GetSub("HP3ZUIDAMAX", xmlPath);
                break;
            case 26:
                xml = ub.GetSub("HP3FUDONG", xmlPath);
                break;
            case 27:
                xml = ub.GetSub("HP3DayChaMax", xmlPath);
                break;

        }
        return xml;
    }
    //���ݷ���
    private void DataPage()
    {
        #region ��ʽ��
        string GameName = ub.GetSub("HP3Name", xmlPath);
        Master.Title = "���ڷ���";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">��Ϸ����</a>&gt;<a href=\"" + Utils.getUrl("HP3.aspx") + "\">" + GameName + "</a>&gt;���ڷ���");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("�����ڷ�����");
        builder.Append(Out.Tab("</div>", "<br />"));
        string searchday = Utils.GetRequest("inputdate", "all", 1, @"^([0-9]{3}[1-9]|[0-9]{2}[1-9][0-9]{1}|[0-9]{1}[1-9][0-9]{2}|[1-9][0-9]{3})(((0[13578]|1[02])(0[1-9]|[12][0-9]|3[01]))|((0[469]|11)(0[1-9]|[12][0-9]|30))|(02(0[1-9]|[1][0-9]|2[0-8])))$", DateTime.Now.ToString("yyyyMMdd"));
        if (searchday == "")
        {
            searchday = DateTime.Now.ToString("yyyyMMdd");
        }
        string strText = "�����ڲ�ѯ����ʽ����" + DateTime.Now.ToString("yyyyMMdd") + "��/";
        string strName = "inputdate";
        string strType = "num";
        string strValu = searchday;
        string strEmpt = "";
        string strIdea = "";
        string strOthe = "��ѯ,HP3.aspx?act=data,post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        DataDeal(strValu);
        builder.Append(Out.Tab("<div>", ""));
        if (DataDeal(strValu) != "fucc")
        {
            string[] danum = DataDeal(strValu).Split(',');
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("" + "-----------" + "<br />");
            builder.Append(Out.Tab("</div>", ""));
            builder.Append("ͳ�����ݣ�" + DateTime.ParseExact(strValu, "yyyyMMdd", null).ToString("yyyy-MM-dd") + "<br />");
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("" + "-----------" + "<br />");
            builder.Append(Out.Tab("</div>", ""));
            builder.Append("<div class=\"text\" style=\"color:#FF7F00\">����ͳ��</div>");
            builder.Append("ͬ����̬��<br />");
            builder.Append("<table>");
            builder.Append("<tr><td>���ң�</td><td><b style=\"color:red\">" + danum[1] + "</b>��</td>");
            builder.Append("<td>���ң�</td><td><b style=\"color:red\">" + danum[2] + "</b>��</td></tr>");
            builder.Append("<tr><td>÷����</td><td><b style=\"color:red\">" + danum[3] + "</b>��</td><td>");
            builder.Append("���飺</td><td><b style=\"color:red\">" + danum[4] + "</b>��</td></tr>");
            builder.Append("</table>");
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("" + "-----------" + "<br />");
            builder.Append(Out.Tab("</div>", ""));
            builder.Append("ͬ��˳��̬��<br />");
            builder.Append("<table>");
            builder.Append("<tr><td>���ң�</td><td><b style=\"color:red\">" + danum[5] + "</b>��</td><td>");
            builder.Append("���ң�</td><td><b style=\"color:red\">" + danum[6] + "</b>��</td></tr>");
            builder.Append("<tr><td>÷����</td><td><b style=\"color:red\">" + danum[7] + "</b>��</td><td>");
            builder.Append("���飺</td><td><b style=\"color:red\">" + danum[8] + "</b>��</td></tr>");
            builder.Append("</table>");
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("" + "-----------" + "<br />");
            builder.Append(Out.Tab("</div>", ""));
            builder.Append("<table>");
            builder.Append("<tr><td>���ӣ�</td><td><b style=\"color:red\">" + danum[9] + "</b>��</td><td>");
            builder.Append("���ӣ�</td><td><b style=\"color:red\">" + danum[10] + "��</b></td></tr>");
            builder.Append("<tr><td>˳�ӣ�</td><td><b style=\"color:red\">" + danum[11] + "</b>��</td></tr>");
            builder.Append("</table>");
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("" + "-----------" + "<br />");
            builder.Append(Out.Tab("</div>", ""));
            builder.Append("<div class=\"text\" style=\"color:#FF7F00\">���Ƚ���ͳ��</div>");
            builder.Append("<table>");
            builder.Append("<tr><td>A��</td><td><b style=\"color:red\">" + danum[12] + "</b>��</td><td>");
            builder.Append("2��</td><td><b style=\"color:red\">" + danum[13] + "</b>��</td><td>");
            builder.Append("3��</td><td><b style=\"color:red\">" + danum[14] + "</b>��</td><td>");
            builder.Append("4��</td><td><b style=\"color:red\">" + danum[15] + "</b>��</td></tr>");
            builder.Append("<tr><td>5��</td><td><b style=\"color:red\">" + danum[16] + "</b>��</td><td>");
            builder.Append("6��</td><td><b style=\"color:red\">" + danum[17] + "</b>��</td><td>");
            builder.Append("7��</td><td><b style=\"color:red\">" + danum[18] + "</b>��</td><td>");
            builder.Append("8��</td><td><b style=\"color:red\">" + danum[19] + "</b>��</td></tr>");
            builder.Append("<tr><td>9��</td><td><b style=\"color:red\">" + danum[20] + "</b>��</td><td>");
            builder.Append("10��</td><td><b style=\"color:red\">" + danum[21] + "</b>��</td><td>");
            builder.Append("J��</td><td><b style=\"color:red\">" + danum[22] + "</b>��</td><td>");
            builder.Append("Q��</td><td><b style=\"color:red\">" + danum[23] + "</b>��</td></tr>");
            builder.Append("<tr><td>K��</td><td><b style=\"color:red\">" + danum[24] + "</b>��</td></tr>");
            builder.Append("</table>");
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("" + "-----------" + "<br />");
            builder.Append(Out.Tab("</div>", ""));
            builder.Append("<div class=\"text\" style=\"color:#FF7F00\">��С��˫ͳ��</div>");
            builder.Append("<table>");
            builder.Append("<tr><td>��</td><td><b style=\"color:red\">" + danum[25] + "</b>��</td><td>");
            builder.Append("С��</td><td><b style=\"color:red\">" + danum[26] + "</b>��</td></tr>");
            builder.Append("<tr><td>����</td><td><b style=\"color:red\">" + danum[27] + "</b>��</td><td>");
            builder.Append("˫��</td><td><b style=\"color:red\">" + danum[28] + "</b>��</td></tr>");
            builder.Append("</table>");
        }
        else
        {
            builder.Append(Out.Div("div", "<br />û����ؼ�¼.."));
        }
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">��ҳ</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">��Ϸ</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("HP3.aspx") + "\">" + GameName + "</a>");
        builder.Append(Out.Tab("</div>", ""));
        #endregion
    }
    //���ݷ�������ҳ��
    private string DataDeal(string searchday)
    {
        DataSet model = new BCW.HP3.BLL.HP3_kjnum().GetDatenumByDate(searchday);
        if (model.Tables[0].Rows.Count == 0)
        {
            return "fucc";
        }
        else
        {
            int[] count = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            for (int s = 0; s < model.Tables[0].Rows.Count; s++)
            {
                string Winum = Convert.ToString(model.Tables[0].Rows[s][0]);
                string[] num = Winum.Split(',');
                string[] danum = { "", "", "", "", "" };
                danum[0] = num[0].Trim();
                danum[1] = num[1].Trim();
                danum[2] = num[2].Trim();
                danum[3] = num[3].Trim();
                if (danum[0] == "1")
                {
                    count[0]++;
                }
                else if (danum[0] == "2")
                {
                    count[1]++;
                }
                else if (danum[0] == "3")
                {
                    count[2]++;
                }
                else if (danum[0] == "4")
                {
                    count[3]++;
                }
                else if (danum[0] == "19")
                {
                    count[4]++;
                }
                else if (danum[0] == "20")
                {
                    count[5]++;
                }
                else if (danum[0] == "21")
                {
                    count[6]++;
                }
                else if (danum[0] == "22")
                {
                    count[7]++;
                }
                else if (danum[0] == "24" || danum[0] == "25" || danum[0] == "26" || danum[0] == "27" || danum[0] == "28" || danum[0] == "29" || danum[0] == "30" || danum[0] == "31" || danum[0] == "32" || danum[0] == "33" || danum[0] == "34" || danum[0] == "35" || danum[0] == "36")
                {
                    count[8]++;
                }
                else if (danum[0] == "38" || danum[0] == "39" || danum[0] == "40" || danum[0] == "41" || danum[0] == "42" || danum[0] == "43" || danum[0] == "44" || danum[0] == "45" || danum[0] == "46" || danum[0] == "47" || danum[0] == "48" || danum[0] == "49" || danum[0] == "50")
                {
                    count[9]++;
                }
                else if (danum[0] == "6" || danum[0] == "7" || danum[0] == "8" || danum[0] == "9" || danum[0] == "10" || danum[0] == "11" || danum[0] == "12" || danum[0] == "13" || danum[0] == "14" || danum[0] == "15" || danum[0] == "16" || danum[0] == "17")
                {
                    count[10]++;
                }
                switch (danum[1])
                {
                    case "A":
                        count[11]++;
                        break;
                    case "2":
                        count[12]++;
                        break;
                    case "3":
                        count[13]++;
                        break;
                    case "4":
                        count[14]++;
                        break;
                    case "5":
                        count[15]++;
                        break;
                    case "6":
                        count[16]++;
                        break;
                    case "7":
                        count[17]++;
                        break;
                    case "8":
                        count[18]++;
                        break;
                    case "9":
                        count[19]++;
                        break;
                    case "10":
                        count[20]++;
                        break;
                    case "J":
                        count[21]++;
                        break;
                    case "Q":
                        count[22]++;
                        break;
                    case "K":
                        count[23]++;
                        break;
                }
                switch (danum[2])
                {
                    case "A":
                        count[11]++;
                        break;
                    case "2":
                        count[12]++;
                        break;
                    case "3":
                        count[13]++;
                        break;
                    case "4":
                        count[14]++;
                        break;
                    case "5":
                        count[15]++;
                        break;
                    case "6":
                        count[16]++;
                        break;
                    case "7":
                        count[17]++;
                        break;
                    case "8":
                        count[18]++;
                        break;
                    case "9":
                        count[19]++;
                        break;
                    case "10":
                        count[20]++;
                        break;
                    case "J":
                        count[21]++;
                        break;
                    case "Q":
                        count[22]++;
                        break;
                    case "K":
                        count[23]++;
                        break;
                }
                switch (danum[3])
                {
                    case "A":
                        count[11]++;
                        break;
                    case "2":
                        count[12]++;
                        break;
                    case "3":
                        count[13]++;
                        break;
                    case "4":
                        count[14]++;
                        break;
                    case "5":
                        count[15]++;
                        break;
                    case "6":
                        count[16]++;
                        break;
                    case "7":
                        count[17]++;
                        break;
                    case "8":
                        count[18]++;
                        break;
                    case "9":
                        count[19]++;
                        break;
                    case "10":
                        count[20]++;
                        break;
                    case "J":
                        count[21]++;
                        break;
                    case "Q":
                        count[22]++;
                        break;
                    case "K":
                        count[23]++;
                        break;
                }
                int sum = 0;
                try
                {
                    danum[4] = num[4].Trim();
                    sum = Convert.ToInt32(danum[4]);
                }
                catch
                {
                    sum = Convert.ToInt32("5");
                }

                if (sum >= 5)
                {
                    count[24]++;
                }
                else
                {
                    count[25]++;
                }
                if (sum % 2 != 0)
                {
                    count[26]++;
                }
                else
                {
                    count[27]++;
                }
                sum = 0;
            }
            string some = model.Tables[0].Rows.Count.ToString();
            for (int n = 0; n < 28; n++)
            {
                some = some + "," + count[n].ToString();
            }
            Array.Clear(count, 0, count.Length);
            return some;
        }

    }
    /// ��ȡ��λ��
    /// <param name="str">Ҫ��ȡ���ַ���</param>
    /// <param name="num">���صľ���λ��</param>
    /// <returns>���ؽ�����ַ���</returns>
    public string GetLastStr(string str, int num)
    {
        int count = 0;
        if (str.Length > num)
        {
            count = str.Length - num;
            str = str.Substring(count, num);
        }
        return str;
    }

    //��ȡ���ֱ�
    public void GetMoney()
    {
        int ci = int.Parse(ub.GetSub("GETMONEYCI", xmlPath));
        long max = Int64.Parse(ub.GetSub("GETMONEYMAX", xmlPath));
        long oneget = Int64.Parse(ub.GetSub("GETMONEY", xmlPath));
        Master.Title = "���ֱ���ȡ";
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        if (ub.GetSub("HP3SWKQ", xmlPath) == "1")
        {
            if (new BCW.HP3.BLL.SWB().Exists(meid) == false)
            {
                Utils.Error("�ܱ�Ǹ��û�л����Ϸ�Ĳ���Ȩ�ޣ�", "");
            }
        }
        else
        {
            if (new BCW.HP3.BLL.SWB().Exists(meid) == false)
            {
                BCW.HP3.Model.SWB swbnew = new BCW.HP3.Model.SWB();
                swbnew.UserID = meid;
                swbnew.HP3Money = oneget;
                swbnew.HP3IsGet = DateTime.Now;
                new BCW.HP3.BLL.SWB().Add(swbnew);
                Utils.Success("��ܰ��ʾ", "�����ɹ������Ϸ���...", Utils.getUrl("HP3.aspx"), "1");
            }

        }
        BCW.HP3.Model.SWB swb = new BCW.HP3.BLL.SWB().GetModel(meid);
        if (swb.HP3IsGet.AddMinutes(ci) < DateTime.Now && swb.HP3Money < max)
        {
            new BCW.HP3.BLL.SWB().UpdateHP3Money(meid, oneget);
            new BCW.HP3.BLL.SWB().UpdateHP3IsGet(meid, DateTime.Now);
            Utils.Success("��Ǯ", "��ϲ��ɹ���ȡ��" + oneget + "���ֱҡ�", Utils.getUrl("HP3.aspx"), "1");
        }
        if (swb.HP3IsGet.AddMinutes(ci) >= DateTime.Now)
        {
            Utils.Error("�����ĵȴ�����ʱ��ɺ���ȡ��", Utils.getUrl("HP3.aspx"));
        }
        if (swb.HP3Money >= max)
        {
            Utils.Error("���Ŀ��ֱ��Ѿ��ܶ��ˣ�������" + max + "���ֱ���������ȡ��", Utils.getUrl("HP3.aspx"));
        }
    }

    // ��������
    public string UpdateState()
    {
        string OnTime = "08:50-22:00";
        if (OnTime != "")
        {
            if (Utils.IsRegex(OnTime, @"^[0-9]{2}:[0-9]{2}-[0-9]{2}:[0-9]{2}$"))
            {
                string[] temp = OnTime.Split("-".ToCharArray());
                DateTime dt1 = Convert.ToDateTime(temp[0]);
                DateTime dt2 = Convert.ToDateTime(temp[1]);
                if (DateTime.Now <= dt2 && DateTime.Now >= dt1)
                {
                    string dt3 = DateTime.Now.AddMinutes(5).Subtract(dt1).Duration().TotalMinutes.ToString();
                    decimal dt4 = Convert.ToDecimal(dt3);
                    int dt5 = Convert.ToInt32(dt4 / 10);
                    string dt6 = dt5.ToString();
                    if (dt6.Length == 1)
                    {
                        dt6 = "0" + dt6;
                    }
                    string state = DateTime.Now.ToString("yyyyMMdd") + dt6;
                    string datee = string.Empty;
                    datee = DateTime.ParseExact((("" + state.Substring(0, 8)) + " 08:50:00"), "yyyyMMdd HH:mm:ss", null).ToString("yyyy-MM-dd HH:mm:ss");
                    BCW.HP3.Model.HP3_kjnum model = new BCW.HP3.Model.HP3_kjnum();
                    model.datenum = state;
                    if (Convert.ToInt32(dt6) < 10 && Convert.ToInt32(dt6) >= 01)
                    {
                        model.datetime = Convert.ToDateTime(datee).AddMinutes(10 * (int.Parse(GetLastStr(state, 1)))).AddSeconds(0);
                    }
                    else
                    {
                        model.datetime = Convert.ToDateTime(datee).AddMinutes(10 * (int.Parse(GetLastStr(state, 2)))).AddSeconds(0);
                    }
                    model.Fnum = "null";
                    model.Snum = "null";
                    model.Tnum = "null";
                    model.Winum = "null";
                    bool s = new BCW.HP3.BLL.HP3_kjnum().Exists(model.datenum);
                    switch (s)
                    {
                        case false:
                            new BCW.HP3.BLL.HP3_kjnum().Add(model);
                            break;
                        case true:
                            break;
                    }

                    return state;
                }
                else if (DateTime.Now > dt2 && DateTime.Now < Convert.ToDateTime("" + DateTime.Now.ToString("yyyy-MM-dd") + " 23:59:59" + ""))
                {
                    BCW.HP3.Model.HP3_kjnum model = new BCW.HP3.Model.HP3_kjnum();
                    string state = DateTime.Now.AddDays(1).ToString("yyyyMMdd") + "01";
                    model.datenum = state;
                    model.datetime = Convert.ToDateTime("" + DateTime.Now.AddDays(1).ToString("yyyy-MM-dd") + " 09:00:00" + "");
                    model.Fnum = "null";
                    model.Snum = "null";
                    model.Tnum = "null";
                    model.Winum = "null";
                    bool s = new BCW.HP3.BLL.HP3_kjnum().Exists(model.datenum);
                    switch (s)
                    {
                        case false:
                            new BCW.HP3.BLL.HP3_kjnum().Add(model);
                            break;
                        case true:
                            break;
                    }

                    return state;
                }
                else if (DateTime.Now > Convert.ToDateTime("" + DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00" + "") && DateTime.Now < dt1)
                {
                    BCW.HP3.Model.HP3_kjnum model = new BCW.HP3.Model.HP3_kjnum();
                    string state = DateTime.Now.ToString("yyyyMMdd") + "01";
                    model.datenum = state;
                    model.datetime = Convert.ToDateTime("" + DateTime.Now.ToString("yyyy-MM-dd") + " 09:00:00" + "");
                    model.Fnum = "null";
                    model.Snum = "null";
                    model.Tnum = "null";
                    model.Winum = "null";
                    bool s = new BCW.HP3.BLL.HP3_kjnum().Exists(model.datenum);
                    switch (s)
                    {
                        case false:
                            new BCW.HP3.BLL.HP3_kjnum().Add(model);
                            break;
                        case true:
                            break;
                    }

                    return state;
                }
            }
        }
        return "0";
    }

    /// <summary>
    /// �����עת����X��
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    private string ChangeToWan(string str)
    {
        string CW = string.Empty;
        try
        {
            if (str != "")
            {
                long first = 0;
                first = Convert.ToInt64(str.Trim());
                if (first >= 10000)
                {
                    if (first % 10000 == 0)
                    {
                        CW = (first / 10000) + "��";
                    }
                    else
                    {
                        CW = (first / 10000) + ".X��";
                    }
                }
                else
                {
                    CW = first.ToString();
                }
            }
        }
        catch { }
        return CW;
    }

    #region �����ע1
    private void kuai(int uid, int type, int ptype, string Num5, string Num4, string Num3, string Num2, string Num1)//�û�����Ϸ��ţ���ע����|���⴫ֵ5.4.3.2.1
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        string ac = Utils.GetRequest("ac", "all", 1, "", "");

        if (new BCW.QuickBet.BLL.QuickBet().ExistsUsID(meid))
        {

        }
        else//����Ա�Զ����Ĭ�ϵĿ����ע
        {
            BCW.QuickBet.Model.QuickBet model = new BCW.QuickBet.Model.QuickBet();
            model.UsID = meid;
            model.Game = new BCW.QuickBet.BLL.QuickBet().GetGame();//ʮ����ŵ���Ϸ|1:ʱʱ��|2����ʮ��|3:�����˿�3|4:6����|5:ʤ����
            model.Bet = new BCW.QuickBet.BLL.QuickBet().GetBety();
            new BCW.QuickBet.BLL.QuickBet().Add(model);
        }

        #region �����ע
        try
        {
            string game = new BCW.QuickBet.BLL.QuickBet().GetGame(meid);
            string bet = new BCW.QuickBet.BLL.QuickBet().GetBet(meid);
            string[] game1 = game.Split('#');
            string[] bet1 = bet.Split('#');
            for (int i = 0; i < game1.Length; i++)
            {
            }

            int j = 0;
            for (int i = 0; i < game1.Length; i++)
            {
                if (Convert.ToInt32(game1[i]) == type)//ȡ����Ӧ����Ϸ
                {
                    j = i;
                }
            }
            string gold = string.Empty;
            string st = string.Empty;
            string str = string.Empty;
            string[] kuai = bet1[j].Split('|');//ȡ����Ӧ�Ŀ����ע
            for (int i = 0; i < kuai.Length; i++)
            {
                if (kuai[i] != "0")
                {
                    //if (Convert.ToInt64(kuai[i]) >= 10000)
                    //{
                    //    if (Convert.ToInt64(kuai[i]) % 10000 == 0)
                    //    {
                    //        gold = Utils.ConvertGold(Convert.ToInt64(kuai[i]));//
                    //    }
                    //    else
                    //    {
                    //        st = (Convert.ToInt64(kuai[i]) / 10000) + ".X��";
                    //        gold = st;
                    //    }
                    //}
                    //else
                    //{
                    //    gold = Utils.ConvertGold(Convert.ToInt64(kuai[i]));//
                    //}

                    gold = ChangeToWan(kuai[i]);

                    //Ͷע�б�
                    if (ptype == 1 || ptype == 2 || ptype == 3 || ptype == 4 || ptype == 5)
                    {
                        builder.Append("<a href =\"" + Utils.getUrl("HP3.aspx?act=pay&amp;ptype=" + ptype + "&amp;Num1=" + Num1 + "&amp;Price=" + Convert.ToInt64(kuai[i]) + "") + "\">" + gold + "</a>" + "|");//                    
                    }
                    else if (ptype == 17 || ptype == 18)//��СͶע
                    {
                        builder.Append("<a href =\"" + Utils.getUrl("HP3.aspx?act=pay&amp;ptype=" + ptype + "&amp;Num5=" + Num5 + "&amp;Price=" + Convert.ToInt64(kuai[i]) + "") + "\">" + gold + "</a>" + "|");//            
                    }
                    else if (ptype == 6 || ptype == 7 || ptype == 9 || ptype == 11 || ptype == 13 || ptype == 15)
                    {
                        builder.Append("<a href =\"" + Utils.getUrl("HP3.aspx?act=pay&amp;ptype=" + ptype + "&amp;Num2=" + Num2 + "&amp;Price=" + Convert.ToInt64(kuai[i]) + "") + "\">" + gold + "</a>" + "|");//            
                    }
                    else //8��10��12��14��16Ϊ����Ͷע��
                    {
                        builder.Append("<a href =\"" + Utils.getUrl("HP3.aspx?act=pay&amp;ptype=" + ptype + "&amp;Num3=" + Num3 + "&amp;Num4=" + Num4 + "&amp;Price=" + Convert.ToInt64(kuai[i]) + "") + "\">" + gold + "</a>" + "|");//            
                    }
                }
            }

            builder.Append("<a href=\"" + Utils.getUrl("QuickBet.aspx?act=edit&amp;type=" + type + "&amp;ptype=" + ptype + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">��ɾ</a>");

        }
        catch { }
        #endregion
    }
    #endregion
}
