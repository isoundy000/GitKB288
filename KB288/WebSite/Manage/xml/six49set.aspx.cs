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

public partial class Manage_xml_six49set : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected void Page_Load(object sender, EventArgs e)
    {
        Master.Title = "虚拟投注设置";
        builder.Append(Out.Tab("", ""));
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[0-9]\d*$", "0"));
        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        ub xml = new ub();
        string xmlPath = "/Controls/six49.xml";
        Application.Remove(xmlPath);//清缓存
        xml.ReloadSub(xmlPath); //加载配置
        if (Utils.ToSChinese(ac) == "确定修改")
        {
            if (ptype == 1)
            {
                string Name = Utils.GetRequest("Name", "post", 2, @"^[^\^]{1,20}$", "系统名称限1-20字内");
                string Logo = Utils.GetRequest("Logo", "post", 3, @"^[^\^]{1,200}$", "Logo地址限200字内");
                string Status = Utils.GetRequest("Status", "post", 2, @"^[0-1]$", "系统状态选择错误");
                string BeforeMin = Utils.GetRequest("BeforeMin", "post", 2, @"^[0-9]\d*$", "开奖前N分钟截止下注填写错误");
                string Expir = Utils.GetRequest("Expir", "post", 2, @"^[0-9]\d*$", "下注防刷时间填写错误");
                string SystemUbb = Utils.GetRequest("SystemUbb", "post", 3, @"^[\s\S]{1,10000}$", "界面Ubb限10000字内");
                string DemoIDS = Utils.GetRequest("DemoIDS", "post", 3, @"^[^\#]{1,50}(?:\#[^\#]{1,50}){0,500}$", "内测ID填写错误，可留空");
                string TopUbb = Utils.GetRequest("TopUbb", "post", 3, @"^[\s\S]{1,5000}$", "顶部Ubb限5000字内");
                string FootUbb = Utils.GetRequest("FootUbb", "post", 3, @"^[\s\S]{1,5000}$", "底部Ubb限5000字内");

                xml.dss["SixName"] = Name;
                xml.dss["SixLogo"] = Logo;
                xml.dss["SixStatus"] = Status;
                xml.dss["SixBeforeMin"] = BeforeMin;
                xml.dss["SixExpir"] = Expir;
                xml.dss["SixSystemUbb"] = SystemUbb;
                xml.dss["SixDemoIDS"] = DemoIDS;
                xml.dss["SixTopUbb"] = TopUbb;
                xml.dss["SixFootUbb"] = FootUbb;
            }
            else if (ptype == 2)
            {
                string TM = Utils.GetRequest("TM", "post", 2, @"^(\d)*(\.(\d){0,2})?$", "特.码赔率填写错误");
                string DS = Utils.GetRequest("DS", "post", 2, @"^(\d)*(\.(\d){0,2})?$", "单赔率填写错误");
                string DX = Utils.GetRequest("DX", "post", 2, @"^(\d)*(\.(\d){0,2})?$", "大赔率填写错误");
                string DS2 = Utils.GetRequest("DS2", "post", 2, @"^(\d)*(\.(\d){0,2})?$", "双赔率填写错误");
                string DX2 = Utils.GetRequest("DX2", "post", 2, @"^(\d)*(\.(\d){0,2})?$", "小赔率填写错误");
                string BS = Utils.GetRequest("BS", "post", 2, @"^(\d)*(\.(\d){0,2})?$", "波色赔率填写错误");
                string SX = Utils.GetRequest("SX", "post", 2, @"^(\d)*(\.(\d){0,2})?$", "生肖赔率填写错误");
                string QS = Utils.GetRequest("QS", "post", 2, @"^(\d)*(\.(\d){0,2})?$", "家禽生肖赔率填写错误");
                string QS2 = Utils.GetRequest("QS2", "post", 2, @"^(\d)*(\.(\d){0,2})?$", "野兽生肖赔率填写错误");
                string HDS = Utils.GetRequest("HDS", "post", 2, @"^(\d)*(\.(\d){0,2})?$", "合数合单赔率填写错误");
                string HDS2 = Utils.GetRequest("HDS2", "post", 2, @"^(\d)*(\.(\d){0,2})?$", "合数合双赔率填写错误");
                string HDX = Utils.GetRequest("HDX", "post", 2, @"^(\d)*(\.(\d){0,2})?$", "合数合大赔率填写错误");
                string HDX2 = Utils.GetRequest("HDX2", "post", 2, @"^(\d)*(\.(\d){0,2})?$", "合数合小赔率填写错误");
                string SIX = Utils.GetRequest("SIX", "post", 2, @"^(\d)*(\.(\d){0,2})?$", "六肖赔率填写错误");
                string WX = Utils.GetRequest("WX", "post", 2, @"^(\d)*(\.(\d){0,2})?$", "五行赔率填写错误");
                string TL = Utils.GetRequest("TL", "post", 2, @"^(\d)*(\.(\d){0,2})?$", "特连赔率填写错误");
                string SZS = Utils.GetRequest("SZS", "post", 2, @"^(\d)*(\.(\d){0,2})?$", "三中三赔率填写错误");
                string SZE = Utils.GetRequest("SZE", "post", 2, @"^(\d)*(\.(\d){0,2})?$", "三中二赔率填写错误");
                string SZE2 = Utils.GetRequest("SZE2", "post", 2, @"^(\d)*(\.(\d){0,2})?$", "三中二2赔率填写错误");
                string EZE = Utils.GetRequest("EZE", "post", 2, @"^(\d)*(\.(\d){0,2})?$", "二中二赔率填写错误");
                string YX = Utils.GetRequest("YX", "post", 2, @"^(\d)*(\.(\d){0,2})?$", "平肖赔率填写错误");
                string DP = Utils.GetRequest("DP", "post", 2, @"^(\d)*(\.(\d){0,2})?$", "独平赔率填写错误");
                string DW = Utils.GetRequest("DW", "post", 2, @"^(\d)*(\.(\d){0,2})?$", "独尾赔率填写错误");
                string PTLX = Utils.GetRequest("PTLX", "post", 3, @"^(\d)*(\.(\d){0,2})?$", "平特二肖赔率填写错误");
                string WBZ = Utils.GetRequest("WBZ", "post", 3, @"^(\d)*(\.(\d){0,2})?$", "五不中赔率填写错误");
                string vWM = Utils.GetRequest("vWM", "post", 3, @"^(\d)*(\.(\d){0,2})?$", "五门赔率填写错误");
                string vWSDX = Utils.GetRequest("vWSDX", "post", 3, @"^(\d)*(\.(\d){0,2})?$", "尾数大小赔率填写错误");
                string vWSDS = Utils.GetRequest("vWSDS", "post", 3, @"^(\d)*(\.(\d){0,2})?$", "尾数单双赔率填写错误");
                string vZFDX = Utils.GetRequest("vZFDX", "post", 3, @"^(\d)*(\.(\d){0,2})?$", "总分大小赔率填写错误");
                string vZFDS = Utils.GetRequest("vZFDS", "post", 3, @"^(\d)*(\.(\d){0,2})?$", "总分单双赔率填写错误");
                string vPTLX2 = Utils.GetRequest("vPTLX2", "post", 3, @"^(\d)*(\.(\d){0,2})?$", "平特三肖赔率填写错误");
                string vPTLX3 = Utils.GetRequest("vPTLX3", "post", 3, @"^(\d)*(\.(\d){0,2})?$", "平特四肖赔率填写错误");
                string vPTW = Utils.GetRequest("vPTW", "post", 3, @"^(\d)*(\.(\d){0,2})?$", "平特二尾赔率填写错误");
                string vPTW2 = Utils.GetRequest("vPTW2", "post", 3, @"^(\d)*(\.(\d){0,2})?$", "平特三尾赔率填写错误");
                string vPTW3 = Utils.GetRequest("vPTW3", "post", 3, @"^(\d)*(\.(\d){0,2})?$", "平特四尾赔率填写错误");

                xml.dss["SixTM"] = TM;
                xml.dss["SixDS"] = DS;
                xml.dss["SixDX"] = DX;
                xml.dss["SixDS2"] = DS2;
                xml.dss["SixDX2"] = DX2;
                xml.dss["SixBS"] = BS;
                xml.dss["SixSX"] = SX;
                xml.dss["SixQS"] = QS;
                xml.dss["SixQS2"] = QS2;
                xml.dss["SixHDS"] = HDS;
                xml.dss["SixHDS2"] = HDS2;
                xml.dss["SixHDX"] = HDX;
                xml.dss["SixHDX2"] = HDX2;
                xml.dss["SixSIX"] = SIX;
                xml.dss["SixWX"] = WX;
                xml.dss["SixTL"] = TL;
                xml.dss["SixSZS"] = SZS;
                xml.dss["SixSZE"] = SZE;
                xml.dss["SixSZE2"] = SZE2;
                xml.dss["SixEZE"] = EZE;
                xml.dss["SixYX"] = YX;
                xml.dss["SixDP"] = DP;
                xml.dss["SixDW"] = DW;
                xml.dss["SixPTLX"] = PTLX;
                xml.dss["SixWBZ"] = WBZ;
                xml.dss["SixvWM"] = vWM;
                xml.dss["SixvWSDX"] = vWSDX;
                xml.dss["SixvWSDS"] = vWSDS;
                xml.dss["SixvZFDX"] = vZFDX;
                xml.dss["SixvZFDS"] = vZFDS;
                xml.dss["SixvPTLX2"] = vPTLX2;
                xml.dss["SixvPTLX3"] = vPTLX3;
                xml.dss["SixvPTW"] = vPTW;
                xml.dss["SixvPTW2"] = vPTW2;
                xml.dss["SixvPTW3"] = vPTW3;
            }
            else if (ptype == 3)
            {
                string T0 = Utils.GetRequest("T0", "post", 2, @"^(\d)*(\.(\d){0,2})?$", "0头赔率填写错误");
                string T1 = Utils.GetRequest("T1", "post", 2, @"^(\d)*(\.(\d){0,2})?$", "1头赔率填写错误");
                string T2 = Utils.GetRequest("T2", "post", 2, @"^(\d)*(\.(\d){0,2})?$", "2头赔率填写错误");
                string T3 = Utils.GetRequest("T3", "post", 2, @"^(\d)*(\.(\d){0,2})?$", "3头赔率填写错误");
                string T4 = Utils.GetRequest("T4", "post", 2, @"^(\d)*(\.(\d){0,2})?$", "4头赔率填写错误");
                xml.dss["SixT0"] = T0;
                xml.dss["SixT1"] = T1;
                xml.dss["SixT2"] = T2;
                xml.dss["SixT3"] = T3;
                xml.dss["SixT4"] = T4;
            }
            else if (ptype == 4)
            {
                string W0 = Utils.GetRequest("W0", "post", 2, @"^(\d)*(\.(\d){0,2})?$", "0尾赔率填写错误");
                string W1 = Utils.GetRequest("W1", "post", 2, @"^(\d)*(\.(\d){0,2})?$", "1尾赔率填写错误");
                string W2 = Utils.GetRequest("W2", "post", 2, @"^(\d)*(\.(\d){0,2})?$", "2尾赔率填写错误");
                string W3 = Utils.GetRequest("W3", "post", 2, @"^(\d)*(\.(\d){0,2})?$", "3尾赔率填写错误");
                string W4 = Utils.GetRequest("W4", "post", 2, @"^(\d)*(\.(\d){0,2})?$", "4尾赔率填写错误");
                string W5 = Utils.GetRequest("W5", "post", 2, @"^(\d)*(\.(\d){0,2})?$", "5尾赔率填写错误");
                string W6 = Utils.GetRequest("W6", "post", 2, @"^(\d)*(\.(\d){0,2})?$", "6尾赔率填写错误");
                string W7 = Utils.GetRequest("W7", "post", 2, @"^(\d)*(\.(\d){0,2})?$", "7尾赔率填写错误");
                string W8 = Utils.GetRequest("W8", "post", 2, @"^(\d)*(\.(\d){0,2})?$", "8尾赔率填写错误");
                string W9 = Utils.GetRequest("W9", "post", 2, @"^(\d)*(\.(\d){0,2})?$", "9尾赔率填写错误");
                xml.dss["SixW0"] = W0;
                xml.dss["SixW1"] = W1;
                xml.dss["SixW2"] = W2;
                xml.dss["SixW3"] = W3;
                xml.dss["SixW4"] = W4;
                xml.dss["SixW5"] = W5;
                xml.dss["SixW6"] = W6;
                xml.dss["SixW7"] = W7;
                xml.dss["SixW8"] = W8;
                xml.dss["SixW9"] = W9;
            }
            else if (ptype == 8)
            {
                string PW0 = Utils.GetRequest("PW0", "post", 2, @"^(\d)*(\.(\d){0,2})?$", "0尾赔率填写错误");
                string PW1 = Utils.GetRequest("PW1", "post", 2, @"^(\d)*(\.(\d){0,2})?$", "1尾赔率填写错误");
                string PW2 = Utils.GetRequest("PW2", "post", 2, @"^(\d)*(\.(\d){0,2})?$", "2尾赔率填写错误");
                string PW3 = Utils.GetRequest("PW3", "post", 2, @"^(\d)*(\.(\d){0,2})?$", "3尾赔率填写错误");
                string PW4 = Utils.GetRequest("PW4", "post", 2, @"^(\d)*(\.(\d){0,2})?$", "4尾赔率填写错误");
                string PW5 = Utils.GetRequest("PW5", "post", 2, @"^(\d)*(\.(\d){0,2})?$", "5尾赔率填写错误");
                string PW6 = Utils.GetRequest("PW6", "post", 2, @"^(\d)*(\.(\d){0,2})?$", "6尾赔率填写错误");
                string PW7 = Utils.GetRequest("PW7", "post", 2, @"^(\d)*(\.(\d){0,2})?$", "7尾赔率填写错误");
                string PW8 = Utils.GetRequest("PW8", "post", 2, @"^(\d)*(\.(\d){0,2})?$", "8尾赔率填写错误");
                string PW9 = Utils.GetRequest("PW9", "post", 2, @"^(\d)*(\.(\d){0,2})?$", "9尾赔率填写错误");
                xml.dss["SixPW0"] = PW0;
                xml.dss["SixPW1"] = PW1;
                xml.dss["SixPW2"] = PW2;
                xml.dss["SixPW3"] = PW3;
                xml.dss["SixPW4"] = PW4;
                xml.dss["SixPW5"] = PW5;
                xml.dss["SixPW6"] = PW6;
                xml.dss["SixPW7"] = PW7;
                xml.dss["SixPW8"] = PW8;
                xml.dss["SixPW9"] = PW9;
            }
            else if (ptype == 5)
            {
                string Red0 = Utils.GetRequest("Red0", "post", 2, @"^(\d)*(\.(\d){0,2})?$", "红单赔率填写错误");
                string Red1 = Utils.GetRequest("Red1", "post", 2, @"^(\d)*(\.(\d){0,2})?$", "红双赔率填写错误");
                string Blue0 = Utils.GetRequest("Blue0", "post", 2, @"^(\d)*(\.(\d){0,2})?$", "蓝单赔率填写错误");
                string Blue1 = Utils.GetRequest("Blue1", "post", 2, @"^(\d)*(\.(\d){0,2})?$", "蓝双赔率填写错误");
                string Green0 = Utils.GetRequest("Green0", "post", 2, @"^(\d)*(\.(\d){0,2})?$", "绿单赔率填写错误");
                string Green1 = Utils.GetRequest("Green1", "post", 2, @"^(\d)*(\.(\d){0,2})?$", "绿双赔率填写错误");
                xml.dss["SixRed0"] = Red0;
                xml.dss["SixRed1"] = Red1;
                xml.dss["SixBlue0"] = Blue0;
                xml.dss["SixBlue1"] = Blue1;
                xml.dss["SixGreen0"] = Green0;
                xml.dss["SixGreen1"] = Green1;
              
            }
            else if (ptype == 6)
            {
                string red = Utils.GetRequest("red", "post", 2, @"^[^\#]{1,2}(?:\#[^\#]{1,2}){0,100}$", "号码请用#分开");
                string blue = Utils.GetRequest("blue", "post", 2, @"^[^\#]{1,2}(?:\#[^\#]{1,2}){0,100}$", "号码请用#分开");
                string green = Utils.GetRequest("green", "post", 2, @"^[^\#]{1,2}(?:\#[^\#]{1,2}){0,100}$", "号码请用#分开");
                string gold = Utils.GetRequest("gold", "post", 2, @"^[^\#]{1,2}(?:\#[^\#]{1,2}){0,100}$", "号码请用#分开");
                string wood = Utils.GetRequest("wood", "post", 2, @"^[^\#]{1,2}(?:\#[^\#]{1,2}){0,100}$", "号码请用#分开");
                string water = Utils.GetRequest("water", "post", 2, @"^[^\#]{1,2}(?:\#[^\#]{1,2}){0,100}$", "号码请用#分开");
                string fire = Utils.GetRequest("fire", "post", 2, @"^[^\#]{1,2}(?:\#[^\#]{1,2}){0,100}$", "号码请用#分开");
                string soil = Utils.GetRequest("soil", "post", 2, @"^[^\#]{1,2}(?:\#[^\#]{1,2}){0,100}$", "号码请用#分开");
                for (int i = 1; i <= 12; i++)
                {
                    xml.dss["Sixsx" + i + ""] = Utils.GetRequest("sx" + i + "", "post", 2, @"^[^\#]{1,2}(?:\#[^\#]{1,2}){0,100}$", "号码请用#分开");
                }
                xml.dss["Sixred"] = red;
                xml.dss["Sixblue"] = blue;
                xml.dss["Sixgreen"] = green;
                xml.dss["Sixgold"] = gold;
                xml.dss["Sixwood"] = wood;
                xml.dss["Sixwater"] = water;
                xml.dss["Sixfire"] = fire;
                xml.dss["Sixsoil"] = soil;

            }
            else if (ptype == 7)
            {
                string SmallPay = Utils.GetRequest("SmallPay", "post", 2, @"^[0-9]\d*$", "最小" + ub.Get("SiteBz") + "下注额填写错误");
                string BigPay = Utils.GetRequest("BigPay", "post", 2, @"^[0-9]\d*$", "最大" + ub.Get("SiteBz") + "下注额填写错误");
                string SmallPay2 = Utils.GetRequest("SmallPay2", "post", 2, @"^[0-9]\d*$", "最小" + ub.Get("SiteBz2") + "下注额填写错误");
                string BigPay2 = Utils.GetRequest("BigPay2", "post", 2, @"^[0-9]\d*$", "最大" + ub.Get("SiteBz2") + "下注额填写错误");
                string IDPayCents = Utils.GetRequest("IDPayCents", "post", 2, @"^[0-9]\d*$", "每ID每期下注" + ub.Get("SiteBz") + "上限填写错误");
                string PayCents = Utils.GetRequest("PayCents", "post", 2, @"^[0-9]\d*$", "每期下注" + ub.Get("SiteBz") + "上限填写错误");
                string IDPayCents2 = Utils.GetRequest("IDPayCents2", "post", 2, @"^[0-9]\d*$", "每ID每期下注" + ub.Get("SiteBz2") + "上限填写错误");
                string PayCents2 = Utils.GetRequest("PayCents2", "post", 2, @"^[0-9]\d*$", "每期下注" + ub.Get("SiteBz2") + "上限填写错误");
                xml.dss["SixSmallPay"] = SmallPay;
                xml.dss["SixBigPay"] = BigPay;
                xml.dss["SixSmallPay2"] = SmallPay2;
                xml.dss["SixBigPay2"] = BigPay2;
                xml.dss["SixIDPayCents"] = IDPayCents;
                xml.dss["SixPayCents"] = PayCents;
                xml.dss["SixIDPayCents2"] = IDPayCents2;
                xml.dss["SixPayCents2"] = PayCents2;
            }
            else if (ptype == 9)
            {
                for (int i = 1; i <= 49; i++)
                {
                    string TM = Utils.GetRequest("TM" + i + "", "post", 3, @"^(\d)*(\.(\d){0,2})?$", "特.码赔率" + i + "填写错误");
                    xml.dss["SixTM" + i + ""] = Request["TM" + i + ""];
                }
            }
            else if (ptype == 10)
            {
                for (int i = 1; i <= 12; i++)
                {
                    string XX = Utils.GetRequest("XX" + i + "", "post", 3, @"^(\d)*(\.(\d){0,2})?$", "生肖赔率" + i + "填写错误");
                    xml.dss["SixXX" + i + ""] = Request["XX" + i + ""];
                }
            }
            else if (ptype == 11)
            {
                for (int i = 1; i <= 49; i++)
                {
                    string TMSX = Utils.GetRequest("TMSX" + i + "", "post", 3, @"^(\d)*(\.(\d){0,2})?$", "特.码上限" + i + "填写错误");
                    xml.dss["SixTMSX" + i + ""] = Request["TMSX" + i + ""];
                }
            }
            else if (ptype == 12)
            {
                for (int i = 1; i <= 12; i++)
                {
                    string PTX = Utils.GetRequest("PTX" + i + "", "post", 3, @"^(\d)*(\.(\d){0,2})?$", "平特肖赔率" + i + "填写错误");
                    xml.dss["SixPTX" + i + ""] = Request["PTX" + i + ""];
                }
            }
            else if (ptype == 13)
            {
                for (int i = 1; i <= 12; i++)
                {
                    string BBB = Utils.GetRequest("BBB" + i + "", "post", 3, @"^(\d)*(\.(\d){0,2})?$", "半半波赔率" + i + "填写错误");
                    xml.dss["SixBBB" + i + ""] = Request["BBB" + i + ""];
                }
            }
            else if (ptype == 14)
            {
                string SBZ22 = Utils.GetRequest("SBZ22", "post", 3, @"^(\d)*(\.(\d){0,2})?$", "选不中赔率22填写错误");
                xml.dss["SixSBZ22"] = SBZ22;
                for (int i = 34; i <= 40; i++)
                {
                    string SBZ = Utils.GetRequest("SBZ" + i + "", "post", 3, @"^(\d)*(\.(\d){0,2})?$", "选不中赔率" + i + "填写错误");
                    xml.dss["SixSBZ" + i + ""] = Request["SBZ" + i + ""];
                }
            }
            System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            Utils.Success("虚拟投注设置", "设置成功，正在返回..", Utils.getUrl("six49set.aspx?ptype=" + ptype + "&amp;backurl=" + Utils.getPage(0) + ""), "1");
        }
        else
        {
            builder.Append(Out.Div("title", "投注系统设置"));

            if (ptype == 0)
            {
                builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                builder.Append("<a href=\"" + Utils.getUrl("six49set.aspx?ptype=1&amp;backurl=" + Utils.getPage(0) + "") + "\">投注配置</a><br />");
                builder.Append("<a href=\"" + Utils.getUrl("six49set.aspx?ptype=2&amp;backurl=" + Utils.getPage(0) + "") + "\">基本赔率</a><br />");
                builder.Append("<a href=\"" + Utils.getUrl("six49set.aspx?ptype=9&amp;backurl=" + Utils.getPage(0) + "") + "\">特.码赔率</a><br />");
                builder.Append("<a href=\"" + Utils.getUrl("six49set.aspx?ptype=10&amp;backurl=" + Utils.getPage(0) + "") + "\">生肖赔率</a><br />");
                builder.Append("<a href=\"" + Utils.getUrl("six49set.aspx?ptype=3&amp;backurl=" + Utils.getPage(0) + "") + "\">特头赔率</a><br />");
                builder.Append("<a href=\"" + Utils.getUrl("six49set.aspx?ptype=4&amp;backurl=" + Utils.getPage(0) + "") + "\">特尾赔率</a><br />");
                builder.Append("<a href=\"" + Utils.getUrl("six49set.aspx?ptype=12&amp;backurl=" + Utils.getPage(0) + "") + "\">平特肖赔率</a><br />");
                builder.Append("<a href=\"" + Utils.getUrl("six49set.aspx?ptype=8&amp;backurl=" + Utils.getPage(0) + "") + "\">平特尾赔率</a><br />");
                builder.Append("<a href=\"" + Utils.getUrl("six49set.aspx?ptype=5&amp;backurl=" + Utils.getPage(0) + "") + "\">半波赔率</a><br />");
                builder.Append("<a href=\"" + Utils.getUrl("six49set.aspx?ptype=13&amp;backurl=" + Utils.getPage(0) + "") + "\">半半波赔率</a><br />");
                builder.Append("<a href=\"" + Utils.getUrl("six49set.aspx?ptype=14&amp;backurl=" + Utils.getPage(0) + "") + "\">选不中赔率</a><br />");
                builder.Append("<a href=\"" + Utils.getUrl("six49set.aspx?ptype=6&amp;backurl=" + Utils.getPage(0) + "") + "\">属性配置</a><br />");
                builder.Append("<a href=\"" + Utils.getUrl("six49set.aspx?ptype=7&amp;backurl=" + Utils.getPage(0) + "") + "\">上限配置</a><br />");
                builder.Append("<a href=\"" + Utils.getUrl("six49set.aspx?ptype=11&amp;backurl=" + Utils.getPage(0) + "") + "\">特.码上限</a><br />");
                builder.Append("<a href=\"" + Utils.getUrl("spkadminset.aspx?ptype=7&amp;backurl=" + Utils.PostPage(true) + "") + "\">闲聊管理员</a>");
                builder.Append(Out.Tab("</div>", ""));
            }

            else if (ptype == 1)
            {
                string strText = "系统名称:/,系统Logo(可留空):/,系统状态:/,开奖前N分钟截止下注(分钟):/,下注仿刷(秒):/,界面Ubb:/,内测ID(多个用#分开，留空则正常开放):/,顶部Ubb:/,底部Ubb:/,,";
                string strName = "Name,Logo,Status,BeforeMin,Expir,SystemUbb,DemoIDS,TopUbb,FootUbb,ptype,backurl";
                string strType = "text,text,select,num,num,hidden,textarea,textarea,textarea,hidden,hidden";
                string strValu = "" + xml.dss["SixName"] + "'" + xml.dss["SixLogo"] + "'" + xml.dss["SixStatus"] + "'" + xml.dss["SixBeforeMin"] + "'" + xml.dss["SixExpir"] + "'" + xml.dss["SixSystemUbb"] + "'" + xml.dss["SixDemoIDS"] + "'" + xml.dss["SixTopUbb"] + "'" + xml.dss["SixFootUbb"] + "'" + ptype + "'" + Utils.getPage(0) + "";
                string strEmpt = "false,true,0|正常|1|维护,false,false,true,true,true,true,false,false";
                string strIdea = "/";
                string strOthe = "确定修改|reset,six49set.aspx,post,1,red|blue";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
                builder.Append(Out.Tab("<div>", " "));
                builder.Append("<a href=\"" + Utils.getUrl("six49set.aspx?backurl=" + Utils.getPage(0) + "") + "\">返回</a>");
                builder.Append(Out.Tab("</div>", ""));

            }
            else if (ptype == 2)
            {
                string strText = "特.码赔率:/,特.码单:/,特.码双:/,特.码大:/,特.码小:/,特.码波色:/,特.码生肖:/,家禽生肖:/,野兽生肖:/,合数合单:/,合数合双:/,合数合大:/,合数合小:/,六肖:/,五行:/,特连:/,平.码三中三:/,平.码三中二(中二):/,平.码三中二(中三):/,平.码二中二:/,平特一肖:/,独平:/,平特一尾:/,平特二肖:/,平特三肖:/,平特四肖:/,平特二尾:/,平特三尾:/,平特四尾:/,五号码不中:/,五门:/,尾数大小:/,尾数单双:/,总分大小:/,总分单双:/,,";
                string strName = "TM,DS,DS2,DX,DX2,BS,SX,QS,QS2,HDS,HDS2,HDX,HDX2,SIX,WX,TL,SZS,SZE,SZE2,EZE,YX,DP,DW,PTLX,vPTLX2,vPTLX3,vPTW,vPTW2,vPTW3,WBZ,vWM,vWSDX,vWSDS,vZFDX,vZFDS,ptype,backurl";
                string strType = "text,text,text,text,text,text,text,text,text,text,text,text,text,text,text,text,text,text,text,text,text,text,text,text,text,text,text,text,text,text,text,text,text,text,text,hidden,hidden";
                string strValu = "" + xml.dss["SixTM"] + "'" + xml.dss["SixDS"] + "'" + xml.dss["SixDS2"] + "'" + xml.dss["SixDX"] + "'" + xml.dss["SixDX2"] + "'" + xml.dss["SixBS"] + "'" + xml.dss["SixSX"] + "'" + xml.dss["SixQS"] + "'" + xml.dss["SixQS2"] + "'" + xml.dss["SixHDS"] + "'" + xml.dss["SixHDS2"] + "'" + xml.dss["SixHDX"] + "'" + xml.dss["SixHDX2"] + "'" + xml.dss["SixSIX"] + "'" + xml.dss["SixWX"] + "'" + xml.dss["SixTL"] + "'" + xml.dss["SixSZS"] + "'" + xml.dss["SixSZE"] + "'" + xml.dss["SixSZE2"] + "'" + xml.dss["SixEZE"] + "'" + xml.dss["SixYX"] + "'" + xml.dss["SixDP"] + "'" + xml.dss["SixDW"] + "'" + xml.dss["SixPTLX"] + "'" + xml.dss["SixvPTLX2"] + "'" + xml.dss["SixvPTLX3"] + "'" + xml.dss["SixvPTW"] + "'" + xml.dss["SixvPTW2"] + "'" + xml.dss["SixvPTW3"] + "'" + xml.dss["SixWBZ"] + "'" + xml.dss["SixvWM"] + "'" + xml.dss["SixvWSDX"] + "'" + xml.dss["SixvWSDS"] + "'" + xml.dss["SixvZFDX"] + "'" + xml.dss["SixvZFDS"] + "'" + ptype + "'" + Utils.getPage(0) + "";
                string strEmpt = "false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false";
                string strIdea = "/";
                string strOthe = "确定修改|reset,six49set.aspx,post,1,red|blue";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
                builder.Append(Out.Tab("<div>", " "));
                builder.Append("<a href=\"" + Utils.getUrl("six49set.aspx?backurl=" + Utils.getPage(0) + "") + "\">返回</a>");
                builder.Append(Out.Tab("</div>", ""));
                builder.Append(Out.Tab("<div>", "<br />"));
                builder.Append("温馨提示:赔率支持使用小数,如1.82");
                builder.Append(Out.Tab("</div>", ""));
            }
            else if (ptype == 3)
            {
                string strText = "=特头赔率配置=/0头:,1头:,2头:,3头:,4头:,,";
                string strName = "T0,T1,T2,T3,T4,ptype,backurl";
                string strType = "text,text,text,text,text,hidden,hidden";
                string strValu = "" + xml.dss["SixT0"] + "'" + xml.dss["SixT1"] + "'" + xml.dss["SixT2"] + "'" + xml.dss["SixT3"] + "'" + xml.dss["SixT4"] + "'" + ptype + "'" + Utils.getPage(0) + "";
                string strEmpt = "false,false,false,false,false,false,false";
                string strIdea = "/";
                string strOthe = "确定修改|reset,six49set.aspx,post,1,red|blue";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
                builder.Append(Out.Tab("<div>", " "));
                builder.Append("<a href=\"" + Utils.getUrl("six49set.aspx?backurl=" + Utils.getPage(0) + "") + "\">返回</a>");
                builder.Append(Out.Tab("</div>", ""));
            }
            else if (ptype == 4)
            {
                string strText = "=特尾赔率配置=/0尾:,1尾:,2尾:,3尾:,4尾:,5尾:,6尾:,7尾:,8尾:,9尾:,,";
                string strName = "W0,W1,W2,W3,W4,W5,W6,W7,W8,W9,ptype,backurl";
                string strType = "text,text,text,text,text,text,text,text,text,text,hidden,hidden";
                string strValu = "" + xml.dss["SixW0"] + "'" + xml.dss["SixW1"] + "'" + xml.dss["SixW2"] + "'" + xml.dss["SixW3"] + "'" + xml.dss["SixW4"] + "'" + xml.dss["SixW5"] + "'" + xml.dss["SixW6"] + "'" + xml.dss["SixW7"] + "'" + xml.dss["SixW8"] + "'" + xml.dss["SixW9"] + "'" + ptype + "'" + Utils.getPage(0) + "";
                string strEmpt = "false,false,false,false,false,false,false,false,false,false,false,false";
                string strIdea = "/";
                string strOthe = "确定修改|reset,six49set.aspx,post,1,red|blue";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
                builder.Append(Out.Tab("<div>", " "));
                builder.Append("<a href=\"" + Utils.getUrl("six49set.aspx?backurl=" + Utils.getPage(0) + "") + "\">返回</a>");
                builder.Append(Out.Tab("</div>", ""));
            }
            else if (ptype == 8)
            {
                string strText = "=平特尾赔率配置=/0尾:,1尾:,2尾:,3尾:,4尾:,5尾:,6尾:,7尾:,8尾:,9尾:,,";
                string strName = "PW0,PW1,PW2,PW3,PW4,PW5,PW6,PW7,PW8,PW9,ptype,backurl";
                string strType = "text,text,text,text,text,text,text,text,text,text,hidden,hidden";
                string strValu = "" + xml.dss["SixPW0"] + "'" + xml.dss["SixPW1"] + "'" + xml.dss["SixPW2"] + "'" + xml.dss["SixPW3"] + "'" + xml.dss["SixPW4"] + "'" + xml.dss["SixPW5"] + "'" + xml.dss["SixPW6"] + "'" + xml.dss["SixPW7"] + "'" + xml.dss["SixPW8"] + "'" + xml.dss["SixPW9"] + "'" + ptype + "'" + Utils.getPage(0) + "";
                string strEmpt = "false,false,false,false,false,false,false,false,false,false,false,false";
                string strIdea = "/";
                string strOthe = "确定修改|reset,six49set.aspx,post,1,red|blue";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
                builder.Append(Out.Tab("<div>", " "));
                builder.Append("<a href=\"" + Utils.getUrl("six49set.aspx?backurl=" + Utils.getPage(0) + "") + "\">返回</a>");
                builder.Append(Out.Tab("</div>", ""));
            }
            else if (ptype == 5)
            {
                string strText = "=半波赔率配置=/红单:,红双:,蓝单:,蓝双:,绿单:,绿双:,,";
                string strName = "Red0,Red1,Blue0,Blue1,Green0,Green1,ptype,backurl";
                string strType = "text,text,text,text,text,text,hidden,hidden";
                string strValu = "" + xml.dss["SixRed0"] + "'" + xml.dss["SixRed1"] + "'" + xml.dss["SixBlue0"] + "'" + xml.dss["SixBlue1"] + "'" + xml.dss["SixGreen0"] + "'" + xml.dss["SixGreen1"] + "'" + ptype + "'" + Utils.getPage(0) + "";
                string strEmpt = "false,false,false,false,false,false,false,false";
                string strIdea = "/";
                string strOthe = "确定修改|reset,six49set.aspx,post,1,red|blue";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
                builder.Append(Out.Tab("<div>", " "));
                builder.Append("<a href=\"" + Utils.getUrl("six49set.aspx?backurl=" + Utils.getPage(0) + "") + "\">返回</a>");
                builder.Append(Out.Tab("</div>", ""));
            }
            else if (ptype == 6)
            {
                string strText = "=生肖对照表=/鼠:,牛:,虎:,兔:,龙:,蛇:,马:,羊:,猴:,鸡:,狗:,猪:,=波色对照=/红波:/,蓝波:/,绿波:/,=五行对照=/金数:/,木数:/,水数:/,火数:/,土数:/,,";
                string strName = "sx1,sx2,sx3,sx4,sx5,sx6,sx7,sx8,sx9,sx10,sx11,sx12,red,blue,green,gold,wood,water,fire,soil,ptype,backurl";
                string strType = "text,text,text,text,text,text,text,text,text,text,text,text,textarea,textarea,textarea,textarea,textarea,textarea,textarea,textarea,hidden,hidden";
                string strValu = "" + xml.dss["Sixsx1"] + "'" + xml.dss["Sixsx2"] + "'" + xml.dss["Sixsx3"] + "'" + xml.dss["Sixsx4"] + "'" + xml.dss["Sixsx5"] + "'" + xml.dss["Sixsx6"] + "'" + xml.dss["Sixsx7"] + "'" + xml.dss["Sixsx8"] + "'" + xml.dss["Sixsx9"] + "'" + xml.dss["Sixsx10"] + "'" + xml.dss["Sixsx11"] + "'" + xml.dss["Sixsx12"] + "'" + xml.dss["Sixred"] + "'" + xml.dss["Sixblue"] + "'" + xml.dss["Sixgreen"] + "'" + xml.dss["Sixgold"] + "'" + xml.dss["Sixwood"] + "'" + xml.dss["Sixwater"] + "'" + xml.dss["Sixfire"] + "'" + xml.dss["Sixsoil"] + "'" + ptype + "'" + Utils.getPage(0) + "";
                string strEmpt = "false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false";
                string strIdea = "/";
                string strOthe = "确定修改|reset,six49set.aspx,post,1,red|blue";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
                builder.Append(Out.Tab("<div>", " "));
                builder.Append("<a href=\"" + Utils.getUrl("six49set.aspx?backurl=" + Utils.getPage(0) + "") + "\">返回</a>");
                builder.Append(Out.Tab("</div>", ""));
                builder.Append(Out.Tab("<div>", "<br />"));
                builder.Append("温馨提示:号码之间必须使用#分开");
                builder.Append(Out.Tab("</div>", ""));
            }
            else if (ptype == 7)
            {
                string strText = "=下注限制配置=/下注最小" + ub.Get("SiteBz") + ":/,下注最大" + ub.Get("SiteBz") + ":/,下注最小" + ub.Get("SiteBz2") + ":/,下注最大" + ub.Get("SiteBz2") + ":/,每ID每期下注" + ub.Get("SiteBz") + "上限:/,每期下注" + ub.Get("SiteBz") + "上限:/,每ID每期下注" + ub.Get("SiteBz2") + "上限:/,每期下注" + ub.Get("SiteBz2") + "上限:/,,";
                string strName = "SmallPay,BigPay,SmallPay2,BigPay2,IDPayCents,PayCents,IDPayCents2,PayCents2,ptype,backurl";
                string strType = "num,num,num,num,num,num,num,num,hidden,hidden";
                string strValu = "" + xml.dss["SixSmallPay"] + "'" + xml.dss["SixBigPay"] + "'" + xml.dss["SixSmallPay2"] + "'" + xml.dss["SixBigPay2"] + "'" + xml.dss["SixIDPayCents"] + "'" + xml.dss["SixPayCents"] + "'" + xml.dss["SixIDPayCents2"] + "'" + xml.dss["SixPayCents2"] + "'" + ptype + "'" + Utils.getPage(0) + "";
                string strEmpt = "false,false,false,false,false,false,false,false,false,false";
                string strIdea = "/";
                string strOthe = "确定修改|reset,six49set.aspx,post,1,red|blue";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
                builder.Append(Out.Tab("<div>", " "));
                builder.Append("提示:上限填写0则不限制<br /><a href=\"" + Utils.getUrl("six49set.aspx?backurl=" + Utils.getPage(0) + "") + "\">返回</a>");
                builder.Append(Out.Tab("</div>", ""));
            }
            else if (ptype == 9)
            {
                Master.Title = "特.码赔率配置";
                string strText = "";
                string strName = "";
                string strType = "";
                string strValu = "";
                string strEmpt = "";
                for (int i = 1; i <= 49; i++)
                {
                    strText += ",特.码" + i + ":";
                    strName += ",TM" + i + "";
                    strType += ",text";
                    strValu += "'" + xml.dss["SixTM" + i + ""] + "";
                    strEmpt += ",true";
                }
                strText += ",,";
                strName += ",ptype,backurl";
                strType += ",hidden,hidden";
                strValu += "'" + ptype + "'" + Utils.getPage(0) + "";
                strEmpt += ",false,false";
                strText = Utils.Mid(strText, 1, strText.Length);
                strName = Utils.Mid(strName, 1, strName.Length);
                strType = Utils.Mid(strType, 1, strType.Length);
                strValu = Utils.Mid(strValu, 1, strValu.Length);
                strEmpt = Utils.Mid(strEmpt, 1, strEmpt.Length);

                string strIdea = "/";
                string strOthe = "确定修改|reset,six49set.aspx,post,1,red|blue";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
                builder.Append(Out.Tab("<div>", " "));
                builder.Append("<a href=\"" + Utils.getUrl("six49set.aspx?backurl=" + Utils.getPage(0) + "") + "\">返回</a>");
                builder.Append(Out.Tab("</div>", ""));
                builder.Append(Out.Tab("<div>", "<br />"));
                builder.Append("温馨提示:如某项留空则调用全局特.码赔率进行计算.");
                builder.Append(Out.Tab("</div>", ""));
            }
            else if (ptype == 10)
            {
                string strText = "鼠:,牛:,虎:,兔:,龙:,蛇:,马:,羊:,猴:,鸡:,狗:,猪:,,";
                string strName = "XX1,XX2,XX3,XX4,XX5,XX6,XX7,XX8,XX9,XX10,XX11,XX12,ptype,backurl";
                string strType = "text,text,text,text,text,text,text,text,text,text,text,text,hidden,hidden";
                string strValu = "" + xml.dss["SixXX1"] + "'" + xml.dss["SixXX2"] + "'" + xml.dss["SixXX3"] + "'" + xml.dss["SixXX4"] + "'" + xml.dss["SixXX5"] + "'" + xml.dss["SixXX6"] + "'" + xml.dss["SixXX7"] + "'" + xml.dss["SixXX8"] + "'" + xml.dss["SixXX9"] + "'" + xml.dss["SixXX10"] + "'" + xml.dss["SixXX11"] + "'" + xml.dss["SixXX12"] + "'" + ptype + "'" + Utils.getPage(0) + "";
                string strEmpt = "true,true,true,true,true,true,true,true,true,true,true,true,false,false";
                string strIdea = "/";
                string strOthe = "确定修改|reset,six49set.aspx,post,1,red|blue";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
                builder.Append(Out.Tab("<div>", " "));
                builder.Append("<a href=\"" + Utils.getUrl("six49set.aspx?backurl=" + Utils.getPage(0) + "") + "\">返回</a>");
                builder.Append(Out.Tab("</div>", ""));
                builder.Append(Out.Tab("<div>", "<br />"));
                builder.Append("温馨提示:如某项留空则调用全局生肖赔率进行计算.");
                builder.Append(Out.Tab("</div>", ""));
            }
            else if (ptype == 11)
            {
                Master.Title = "特.码下注上限配置";
                string strText = "";
                string strName = "";
                string strType = "";
                string strValu = "";
                string strEmpt = "";
                for (int i = 1; i <= 49; i++)
                {
                    strText += ",特.码" + i + ":";
                    strName += ",TMSX" + i + "";
                    strType += ",text";
                    strValu += "'" + xml.dss["SixTMSX" + i + ""] + "";
                    strEmpt += ",true";
                }
                strText += ",,";
                strName += ",ptype,backurl";
                strType += ",hidden,hidden";
                strValu += "'" + ptype + "'" + Utils.getPage(0) + "";
                strEmpt += ",false,false";
                strText = Utils.Mid(strText, 1, strText.Length);
                strName = Utils.Mid(strName, 1, strName.Length);
                strType = Utils.Mid(strType, 1, strType.Length);
                strValu = Utils.Mid(strValu, 1, strValu.Length);
                strEmpt = Utils.Mid(strEmpt, 1, strEmpt.Length);

                string strIdea = "/";
                string strOthe = "确定修改|reset,six49set.aspx,post,1,red|blue";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
                builder.Append(Out.Tab("<div>", " "));
                builder.Append("<a href=\"" + Utils.getUrl("six49set.aspx?backurl=" + Utils.getPage(0) + "") + "\">返回</a>");
                builder.Append(Out.Tab("</div>", ""));
                builder.Append(Out.Tab("<div>", "<br />"));
                builder.Append("温馨提示:如某项留空则不限制该特.码下注上限.");
                builder.Append(Out.Tab("</div>", ""));
            }
            else if (ptype == 12)
            {
                string strText = "平特肖鼠:,平特肖牛:,平特肖虎:,平特肖兔:,平特肖龙:,平特肖蛇:,平特肖马:,平特肖羊:,平特肖猴:,平特肖鸡:,平特肖狗:,平特肖猪:,,";
                string strName = "PTX1,PTX2,PTX3,PTX4,PTX5,PTX6,PTX7,PTX8,PTX9,PTX10,PTX11,PTX12,ptype,backurl";
                string strType = "text,text,text,text,text,text,text,text,text,text,text,text,hidden,hidden";
                string strValu = "" + xml.dss["SixPTX1"] + "'" + xml.dss["SixPTX2"] + "'" + xml.dss["SixPTX3"] + "'" + xml.dss["SixPTX4"] + "'" + xml.dss["SixPTX5"] + "'" + xml.dss["SixPTX6"] + "'" + xml.dss["SixPTX7"] + "'" + xml.dss["SixPTX8"] + "'" + xml.dss["SixPTX9"] + "'" + xml.dss["SixPTX10"] + "'" + xml.dss["SixPTX11"] + "'" + xml.dss["SixPTX12"] + "'" + ptype + "'" + Utils.getPage(0) + "";
                string strEmpt = "true,true,true,true,true,true,true,true,true,true,true,true,false,false";
                string strIdea = "/";
                string strOthe = "确定修改|reset,six49set.aspx,post,1,red|blue";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
                builder.Append(Out.Tab("<div>", " "));
                builder.Append("<a href=\"" + Utils.getUrl("six49set.aspx?backurl=" + Utils.getPage(0) + "") + "\">返回</a>");
                builder.Append(Out.Tab("</div>", ""));
                builder.Append(Out.Tab("<div>", "<br />"));
                builder.Append("温馨提示:如某项留空则调用全局平特一肖赔率进行计算.");
                builder.Append(Out.Tab("</div>", ""));
            }
            else if (ptype == 13)
            {
                string strText = "=半半波赔率配置=/红大单:,红大双:,红小单:,红小双:,蓝大单:,蓝大双:,蓝小单:,蓝小双:,绿大单:,绿大双:,绿小单:,绿小双:,,";
                string strName = "BBB1,BBB2,BBB3,BBB4,BBB5,BBB6,BBB7,BBB8,BBB9,BBB10,BBB11,BBB12,ptype,backurl";
                string strType = "text,text,text,text,text,text,text,text,text,text,text,text,hidden,hidden";
                string strValu = "" + xml.dss["SixBBB1"] + "'" + xml.dss["SixBBB2"] + "'" + xml.dss["SixBBB3"] + "'" + xml.dss["SixBBB4"] + "'" + xml.dss["SixBBB5"] + "'" + xml.dss["SixBBB6"] + "'" + xml.dss["SixBBB7"] + "'" + xml.dss["SixBBB8"] + "'" + xml.dss["SixBBB9"] + "'" + xml.dss["SixBBB10"] + "'" + xml.dss["SixBBB11"] + "'" + xml.dss["SixBBB12"] + "'" + ptype + "'" + Utils.getPage(0) + "";
                string strEmpt = "false,false,false,false,false,false,false,false,false,false,false,false,false,false";
                string strIdea = "/";
                string strOthe = "确定修改|reset,six49set.aspx,post,1,red|blue";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
                builder.Append(Out.Tab("<div>", " "));
                builder.Append("<a href=\"" + Utils.getUrl("six49set.aspx?backurl=" + Utils.getPage(0) + "") + "\">返回</a>");
                builder.Append(Out.Tab("</div>", ""));
            }
            else if (ptype == 14)
            {
                string strText = "=选不中赔率配置=/五不中:,六不中:,七不中:,八不中:,九不中:,十不中:,十一不中:,十五不中:,,";
                string strName = "SBZ22,SBZ34,SBZ35,SBZ36,SBZ37,SBZ38,SBZ39,SBZ40,ptype,backurl";
                string strType = "text,text,text,text,text,text,text,text,hidden,hidden";
                string strValu = "" + xml.dss["SixSBZ22"] + "'" + xml.dss["SixSBZ34"] + "'" + xml.dss["SixSBZ35"] + "'" + xml.dss["SixSBZ36"] + "'" + xml.dss["SixSBZ37"] + "'" + xml.dss["SixSBZ38"] + "'" + xml.dss["SixSBZ39"] + "'" + xml.dss["SixSBZ40"] + "'" + ptype + "'" + Utils.getPage(0) + "";
                string strEmpt = "false,false,false,false,false,false,false,false,false,false";
                string strIdea = "/";
                string strOthe = "确定修改|reset,six49set.aspx,post,1,red|blue";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
                builder.Append(Out.Tab("<div>", " "));
                builder.Append("<a href=\"" + Utils.getUrl("six49set.aspx?backurl=" + Utils.getPage(0) + "") + "\">返回</a>");
                builder.Append(Out.Tab("</div>", ""));
            }
            builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getPage("game.aspx") + "\">返回上级</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">配置中心</a><br />");
            builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
            builder.Append(Out.Tab("</div>", "<br />"));
        }
    }
}
