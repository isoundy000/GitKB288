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

public partial class Manage_xml_skinset : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected void Page_Load(object sender, EventArgs e)
    {
        Master.Title = "皮肤模板管理";
        builder.Append(Out.Tab("", ""));

        string act = Utils.GetRequest("act", "all", 1, "", "");
        ub xml = new ub();
        string xmlPath = "/Controls/skin.xml";
        Application.Remove(xmlPath);//清缓存
        xml.ReloadSub(xmlPath); //加载配置
        if (act == "save")
        {
            int id = int.Parse(Utils.GetRequest("id", "post", 2, @"^[2-9]$", "ID错误"));

            string skinName = Utils.GetRequest("skinName" + id + "", "post", 2, @"^[\s\S]{1,10}$", "皮肤名称限1-10字");
            string background_color = Utils.GetRequest("background_color" + id + "", "post", 2, @"^[A-Za-z0-9]{6}$", "全局背景色填写错误，正确格式如:FF9900、408CFF");
            string a_color = Utils.GetRequest("a_color" + id + "", "post", 2, @"^[A-Za-z0-9]{6}$", "链接色填写错误，正确格式如:FF9900、408CFF");
            string a_hover = Utils.GetRequest("a_hover" + id + "", "post", 2, @"^[A-Za-z0-9]{6}$", "经过色填写错误，正确格式如:FF9900、408CFF");
            string a_visited = Utils.GetRequest("a_visited" + id + "", "post", 2, @"^[A-Za-z0-9]{6}$", "访问色填写错误，正确格式如:FF9900、408CFF");
            string input_color_red = Utils.GetRequest("input_color_red" + id + "", "post", 2, @"^[A-Za-z0-9]{6}$", "按钮一色填写错误，正确格式如:FF9900、408CFF");
            string input_color_blue = Utils.GetRequest("input_color_blue" + id + "", "post", 2, @"^[A-Za-z0-9]{6}$", "按钮二色填写错误，正确格式如:FF9900、408CFF");
            string div_title_color = Utils.GetRequest("div_title_color" + id + "", "post", 2, @"^[A-Za-z0-9]{6}$", "块深色填写错误，正确格式如:FF9900、408CFF");
            string div_text_color = Utils.GetRequest("div_text_color" + id + "", "post", 2, @"^[A-Za-z0-9]{6}$", "块浅色填写错误，正确格式如:FF9900、408CFF");
            string div_div_color = Utils.GetRequest("div_div_color" + id + "", "post", 2, @"^[A-Za-z0-9]{6}$", "块淡色填写错误，正确格式如:FF9900、408CFF");
            string div_ft_color = Utils.GetRequest("div_ft_color" + id + "", "post", 2, @"^[A-Za-z0-9]{6}$", "块底色填写错误，正确格式如:FF9900、408CFF");
            string word_acolor = Utils.GetRequest("word_acolor" + id + "", "post", 2, @"^[A-Za-z0-9]{6}$", "字深色填写错误，正确格式如:FF9900、408CFF");
            string word_bcolor = Utils.GetRequest("word_bcolor" + id + "", "post", 2, @"^[A-Za-z0-9]{6}$", "字浅色填写错误，正确格式如:FF9900、408CFF");
            string word_ccolor = Utils.GetRequest("word_ccolor" + id + "", "post", 2, @"^[A-Za-z0-9]{6}$", "字淡色填写错误，正确格式如:FF9900、408CFF");
            string body_padding = Utils.GetRequest("body_padding" + id + "", "post", 2, @"^[0-9]\d*$", "全局边框空隙填写错误");
            string body_font_size = Utils.GetRequest("body_font_size" + id + "", "post", 2, @"^[0-9]\d*$", "全局字体大小填写错误");
            string body_line_height = Utils.GetRequest("body_line_height" + id + "", "post", 2, @"^[0-9]\d*$", "全局字体间距填写错误");
            string body_width = Utils.GetRequest("body_width" + id + "", "post", 3, @"^[0-9]\d*(px|%)$|^0$", "全局屏幕宽度填写错误，正确格式如99%、360px");

            string body_background_image = Utils.GetRequest("body_background_image" + id + "", "post", 3, @"^[\s\S]{1,100}$", "全局背景图填写错误");
            string background_mid = Utils.GetRequest("background_mid" + id + "", "post", 3, @"^[\s\S]{1,100}$", "全局背景音乐填写错误");
            string div_title_image = Utils.GetRequest("div_title_image" + id + "", "post", 3, @"^[\s\S]{1,100}$", "块深背景图填写错误");
            string div_text_image = Utils.GetRequest("div_text_image" + id + "", "post", 3, @"^[\s\S]{1,100}$", "块浅背景图填写错误");
            string div_div_image = Utils.GetRequest("div_div_image" + id + "", "post", 3, @"^[\s\S]{1,100}$", "块淡背景图填写错误");
            string div_ft_image = Utils.GetRequest("div_ft_image" + id + "", "post", 3, @"^[\s\S]{1,100}$", "块底背景图填写错误");
            string skinOrder = Utils.GetRequest("skinOrder" + id + "", "post", 2, @"^[0-9]\d*$", "皮肤排序填写错误");

            xml.dss["skinName" + id + ""] = skinName;
            xml.dss["background_color" + id + ""] = background_color;
            xml.dss["a_color" + id + ""] = a_color;
            xml.dss["a_hover" + id + ""] = a_hover;
            xml.dss["a_visited" + id + ""] = a_visited;
            xml.dss["input_color_red" + id + ""] = input_color_red;
            xml.dss["input_color_blue" + id + ""] = input_color_blue;
            xml.dss["div_title_color" + id + ""] = div_title_color;
            xml.dss["div_text_color" + id + ""] = div_text_color;
            xml.dss["div_div_color" + id + ""] = div_div_color;
            xml.dss["div_ft_color" + id + ""] = div_ft_color;
            xml.dss["word_acolor" + id + ""] = word_acolor;
            xml.dss["word_bcolor" + id + ""] = word_bcolor;
            xml.dss["word_ccolor" + id + ""] = word_ccolor;
            xml.dss["body_padding" + id + ""] = body_padding;
            xml.dss["body_font_size" + id + ""] = body_font_size;
            xml.dss["body_line_height" + id + ""] = body_line_height;
            xml.dss["body_width" + id + ""] = body_width;
            xml.dss["body_background_image" + id + ""] = body_background_image;
            xml.dss["background_mid" + id + ""] = background_mid;
            xml.dss["div_title_image" + id + ""] = div_title_image;
            xml.dss["div_text_image" + id + ""] = div_text_image;
            xml.dss["div_div_image" + id + ""] = div_div_image;
            xml.dss["div_ft_image" + id + ""] = div_ft_image;
            xml.dss["skinOrder" + id + ""] = skinOrder;
            xml.dss["skin" + id + ""] = "1";
            System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            Utils.Success("皮肤模板设置", "设置成功，正在返回..", Utils.getUrl("skinset.aspx?act=edit&amp;id=" + id + ""), "1");
        }
        else if (act == "edit")
        {
            int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[2-9]$", "ID错误"));
            int n = int.Parse(Utils.GetRequest("n", "get", 1, @"^[0-9]\d*$", "-1"));
            string color = Utils.GetRequest("color", "get", 1, @"^[a-z]{3,20}$", "");
            if (color == "")
                n = -1;

            string background_color = "";
            string a_color = "";
            string a_hover = "";
            string a_visited = "";
            string input_color_red = "";
            string input_color_blue = "";
            string div_title_color = "";
            string div_text_color = "";
            string div_div_color = "";
            string div_ft_color = "";
            string word_acolor = "";
            string word_bcolor = "";
            string word_ccolor = "";
            if (n == 0)
                background_color = BCW.User.Color.getColorByName(color);
            else
                background_color = xml.dss["background_color" + id + ""].ToString();

            if (n == 1)
                a_color = BCW.User.Color.getColorByName(color);
            else
                a_color = xml.dss["a_color" + id + ""].ToString();

            if (n == 2)
                a_hover = BCW.User.Color.getColorByName(color);
            else
                a_hover = xml.dss["a_hover" + id + ""].ToString();

            if (n == 3)
                a_visited = BCW.User.Color.getColorByName(color);
            else
                a_visited = xml.dss["a_visited" + id + ""].ToString();

            if (n == 4)
                input_color_red = BCW.User.Color.getColorByName(color);
            else
                input_color_red = xml.dss["input_color_red" + id + ""].ToString();

            if (n == 5)
                input_color_blue = BCW.User.Color.getColorByName(color);
            else
                input_color_blue = xml.dss["input_color_blue" + id + ""].ToString();

            if (n == 6)
                div_title_color = BCW.User.Color.getColorByName(color);
            else
                div_title_color = xml.dss["div_title_color" + id + ""].ToString();

            if (n == 7)
                div_text_color = BCW.User.Color.getColorByName(color);
            else
                div_text_color = xml.dss["div_text_color" + id + ""].ToString();

            if (n == 8)
                div_div_color = BCW.User.Color.getColorByName(color);
            else
                div_div_color = xml.dss["div_div_color" + id + ""].ToString();

            if (n == 9)
                div_ft_color = BCW.User.Color.getColorByName(color);
            else
                div_ft_color = xml.dss["div_ft_color" + id + ""].ToString();

            if (n == 10)
                word_acolor = BCW.User.Color.getColorByName(color);
            else
                word_acolor = xml.dss["word_acolor" + id + ""].ToString();

            if (n == 11)
                word_bcolor = BCW.User.Color.getColorByName(color);
            else
                word_bcolor = xml.dss["word_bcolor" + id + ""].ToString();

            if (n == 12)
                word_ccolor = BCW.User.Color.getColorByName(color);
            else
                word_ccolor = xml.dss["word_ccolor" + id + ""].ToString();

            string[] GetValue = { background_color, a_color, a_hover, a_visited, input_color_red, input_color_blue, div_title_color, div_text_color, div_div_color, div_ft_color, word_acolor, word_bcolor, word_ccolor };
            builder.Append(Out.Div("title", "皮肤模板设置"));
            string strText = "皮肤名称:,全局背景色:,链接色:,经过色:,访问色:,按钮一色:,按钮二色:,块深色:,块浅色:,块淡色:,块底色:,字深色:,字浅色:,字淡色:,全局边框空隙(px):/,全局字体大小(px):/,全局字体间距(px):/,全局屏幕(单位%或px):/,占全局背景图:/,全局背景音乐:/,块深背景图:/,块浅背景图:/,块淡背景图:/,块底背景图:/,皮肤排序:/,,";
            string strName = "skinName" + id + ",background_color" + id + ",a_color" + id + ",a_hover" + id + ",a_visited" + id + ",input_color_red" + id + ",input_color_blue" + id + ",div_title_color" + id + ",div_text_color" + id + ",div_div_color" + id + ",div_ft_color" + id + ",word_acolor" + id + ",word_bcolor" + id + ",word_ccolor" + id + ",body_padding" + id + ",body_font_size" + id + ",body_line_height" + id + ",body_width" + id + ",body_background_image" + id + ",background_mid" + id + ",div_title_image" + id + ",div_text_image" + id + ",div_div_image" + id + ",div_ft_image" + id + ",skinOrder" + id + ",id,act";
            string strType = "stext,stext,stext,stext,stext,stext,stext,stext,stext,stext,stext,stext,stext,stext,num,num,num,text,text,text,text,text,text,text,hidden,hidden,hidden";
            string strValu = "" + xml.dss["skinName" + id + ""] + "'" + background_color + "'" + a_color + "'" + a_hover + "'" + a_visited + "'" + input_color_red + "'" + input_color_blue + "'" + div_title_color + "'" + div_text_color + "'" + div_div_color + "'" + div_ft_color + "'" + word_acolor + "'" + word_bcolor + "'" + word_ccolor + "'" + xml.dss["body_padding" + id + ""] + "'" + xml.dss["body_font_size" + id + ""] + "'" + xml.dss["body_line_height" + id + ""] + "'" + xml.dss["body_width" + id + ""] + "'" + xml.dss["body_background_image" + id + ""] + "'" + xml.dss["background_mid" + id + ""] + "'" + xml.dss["div_title_image" + id + ""] + "'" + xml.dss["div_text_image" + id + ""] + "'" + xml.dss["div_div_image" + id + ""] + "'" + xml.dss["div_ft_image" + id + ""] + "'" + xml.dss["skinOrder" + id + ""] + "'" + id + "'save";
            string strEmpt = "false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,true,true,true,true,true,true,false,false,false";
            string strIdea = "'" + getColor(id, GetValue) + "''''''''''''|/";
            string strOthe = "确定修改|reset,skinset.aspx,post,1,red|blue";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

            builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("温馨提示:<br />手动输入可以全部保存,选择颜色只可一个一个地保存.<br />设置全局屏幕宽度单位支持%与px，如99%、360px<br />当全局屏幕宽度填写0时则自动根据机型屏幕适配<br />");
            builder.Append("<a href=\"" + Utils.getUrl("skinset.aspx") + "\">皮肤模板</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">配置中心</a><br />");
            builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else if (act == "del")
        {
            string info = Utils.GetRequest("info", "all", 1, "", "");
            int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[2-9]$", "ID错误"));
            if (info != "ok")
            {
                Master.Title = "删除皮肤";
                builder.Append(Out.Tab("<div class=\"title\">", ""));
                builder.Append("确定删除此皮肤吗");
                builder.Append(Out.Tab("</div>", "<br />"));
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("<a href=\"" + Utils.getUrl("skinset.aspx?info=ok&amp;act=del&amp;id=" + id + "") + "\">确定删除</a><br />");
                builder.Append("<a href=\"" + Utils.getUrl("skinset.aspx") + "\">先留着吧..</a>");
                builder.Append(Out.Tab("</div>", "<br />"));
            }
            else
            {
                if (id == 2)
                {
                    Utils.Error("系统默认皮肤只可修改不可删除", "");
                }
                xml.dss["skinName" + id + ""] = "新的皮肤" + id + "";
                xml.dss["background_color" + id + ""] = "FFFFFF";
                xml.dss["a_color" + id + ""] = "0A63BB";
                xml.dss["a_hover" + id + ""] = "DE0000";
                xml.dss["a_visited" + id + ""] = "C300E0";
                xml.dss["input_color_red" + id + ""] = "FF9900";
                xml.dss["input_color_blue" + id + ""] = "408CFF";
                xml.dss["div_title_color" + id + ""] = "0E71D4";
                xml.dss["div_text_color" + id + ""] = "EAEAEA";
                xml.dss["div_div_color" + id + ""] = "FFFFFF";
                xml.dss["div_ft_color" + id + ""] = "EAEAEA";
                xml.dss["word_acolor" + id + ""] = "000000";
                xml.dss["word_bcolor" + id + ""] = "FFFFFF";
                xml.dss["word_ccolor" + id + ""] = "B6D1EB";
                xml.dss["body_padding" + id + ""] = "8";
                xml.dss["body_font_size" + id + ""] = "14";
                xml.dss["body_line_height" + id + ""] = "19";
                xml.dss["body_background_image" + id + ""] = "/Files/sys/bg.gif";
                xml.dss["background_mid" + id + ""] = "";
                xml.dss["div_title_image" + id + ""] = "";
                xml.dss["div_text_image" + id + ""] = "";
                xml.dss["div_div_image" + id + ""] = "";
                xml.dss["div_ft_image" + id + ""] = "/Files/sys/ft.png";
                xml.dss["skinOrder" + id + ""] = "0";
                xml.dss["skin" + id + ""] = "0";
                System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
                Utils.Success("删除皮肤", "删除皮肤成功..", Utils.getUrl("skinset.aspx"), "1");
            }
        }

        else if (act == "color")
        {
            string color = Utils.GetRequest("color", "get", 1, "", "");
            int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[2-9]$", "ID错误"));
            int n = int.Parse(Utils.GetRequest("n", "get", 2, @"^[0-9]\d*$", "选项错误"));
            if (n > 12)
            {
                Utils.Error("选项错误", "");
            }
            string[] colorName = { "白色", "烟白色", "黄色", "黄绿色", "艾利斯兰", "古董白", "浅绿色", "碧绿色", "天蓝色", "米色", "桔黄色", "黑色", "白杏色", "蓝色", "紫罗兰色", "褐色", "实木色", "军兰色", "黄绿色", "巧可力色", "珊瑚色", "菊兰色", "米绸色", "暗深红色", "青色", "暗蓝色", "暗青色", "暗金黄色", "暗灰色", "暗绿色", "暗黄褐色", "暗洋红", "暗橄榄绿", "暗桔黄色", "暗紫色", "暗红色", "暗肉色", "暗海兰色", "暗灰蓝色", "墨绿色", "暗宝石绿", "暗紫罗兰色", "深粉红色", "深天蓝色", "暗灰色", "闪兰色", "火砖色", "花白色", "森林绿", "紫红色", "淡灰色", "幽灵白", "金色", "金麒麟色", "灰色", "绿色", "黄绿色", "蜜色", "热粉红色", "印第安红", "靛青色", "象牙色", "黄褐色", "淡紫色", "淡紫红", "草绿色", "柠檬绸色", "亮蓝色", "亮珊瑚色", "亮青色", "亮金黄色", "亮绿色", "亮灰色", "亮粉红色", "亮肉色", "亮海蓝色", "亮天蓝色", "亮蓝灰", "亮钢兰色", "亮黄色", "酸橙色", "橙绿色", "亚麻色", "红紫色", "粟色", "间绿色", "间兰色", "间紫色", "间紫色", "间海蓝", "间暗蓝色", "间春绿色", "间绿宝石", "间紫罗兰色", "中灰兰色", "薄荷色", "浅玫瑰色", "鹿皮色", "纳瓦白", "海军色", "老花色", "橄榄色", "深绿褐色", "橙色", "红橙色", "淡紫色", "苍麒麟色", "苍绿色", "苍宝石绿", "苍紫罗兰色", "番木色", "桃色", "秘鲁色", "粉红色", "洋李色", "粉蓝色", "紫色", "红色", "褐玫瑰红", "皇家蓝", "重褐色", "鲜肉色", "沙褐色", "海绿色", "海贝色", "赭色", "银色", "天蓝色", "石蓝色", "灰石色", "雪白色", "春绿色", "钢兰色", "茶色", "水鸭色", "蓟色", "西红柿色", "青绿色", "粉紫罗兰色", "浅黄色" };
            string[] colorValue = { "white", "whitesmoke", "yellow", "yellowgreen", "aliceblue", "antiquewhite", "aqua", "aquamarine", "azure", "beige", "bisque", "black", "blanchedalmond", "blue", "blueviolet", "brown", "burlywood", "cadetblue", "chartreuse", "chocolate", "coral", "cornflowerblue", "cornsilk", "crimson", "cyan", "darkblue", "darkcyan", "darkgoldenrod", "darkgray", "darkgreen", "darkkhaki", "darkmagenta", "darkolivegreen", "darkorange", "darkorchid", "darkred", "darksalmon", "darkseagreen", "darkslateblue", "darkslategray", "darkturquoise", "darkviolet", "deeppink", "deepskyblue", "dimgray", "dodgerblue", "firebrick", "floralwhite", "forestgreen", "fuchsia", "gainsboro", "ghostwhite", "gold", "goldenrod", "gray", "green", "greenyellow", "honeydew", "hotpink", "indianred", "indigo", "ivory", "khaki", "lavender", "lavenderblush", "lawngreen", "lemonchiffon", "lightblue", "lightcoral", "lightcyan", "lightgoldenrodyellow", "lightgreen", "lightgrey", "lightpink", "lightsalmon", "lightseagreen", "lightskyblue", "lightslategray", "lightsteelblue", "lightyellow", "lime", "limegreen", "linen", "magenta", "maroon", "mediumaquamarine", "mediumblue", "mediumorchid", "mediumpurple", "mediumseagreen", "mediumslateblue", "mediumspringgreen", "mediumturquoise", "mediumvioletred", "midnightblue", "mintcream", "mistyrose", "moccasin", "navajowhite", "navy", "oldlace", "olive", "olivedrab", "orange", "orangered", "orchid", "palegoldenrod", "palegreen", "paleturquoise", "palevioletred", "papayawhip", "peachpuff", "peru", "pink", "plum", "powderblue", "purple", "red", "rosybrown", "royalblue", "saddlebrown", "salmon", "sandybrown", "seagreen", "seashell", "sienna", "silver", "skyblue", "slateblue", "slategray", "snow", "springgreen", "steelblue", "tan", "teal", "thistle", "tomato", "turquoise", "violet", "wheat" };

            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("选择颜色");
            builder.Append(Out.Tab("</div>", ""));
            int pageIndex;
            int recordCount;
            int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
            string[] pageValUrl = { "act", "id", "n", "backurl" };
            pageIndex = Utils.ParseInt(Request.QueryString["page"]);
            if (pageIndex == 0)
                pageIndex = 1;

            //总记录数
            recordCount = colorName.Length;

            int stratIndex = (pageIndex - 1) * pageSize;
            int endIndex = pageIndex * pageSize;
            int k = 0;
            for (int i = 0; i < colorName.Length; i++)
            {
                if (k >= stratIndex && k < endIndex)
                {
                    if ((k + 1) % 2 == 0)
                        builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                    else
                        builder.Append(Out.Tab("<div>", "<br />"));

                    string gUrl = Server.UrlDecode(Utils.getPage(0));
                    gUrl = gUrl.Replace("&amp;", "&");
                    gUrl = Regex.Replace(gUrl, @"([\s\S]+?)[&|?]{0,1}color=[^&]*&{0,}", @"$1&amp;", RegexOptions.IgnoreCase);
                    gUrl = Regex.Replace(gUrl, @"([\s\S]+?)[&|?]{0,1}n=[^&]*&{0,}", @"$1&amp;", RegexOptions.IgnoreCase);
                    gUrl = Out.UBB(gUrl);

                    builder.Append("" + (i + 1) + ".<a href=\"" + Utils.getUrl(gUrl + "&amp;n=" + n + "&amp;color=" + colorValue[i].ToString() + "") + "\">");
                    builder.Append(Out.Tab("<font color=\"#" + BCW.User.Color.getColorByName(colorValue[i].ToString()) + "\">■</font>", "<img src=\"/snap.aspx?act=color&amp;colorname=" + BCW.User.Color.getColorByName(colorValue[i].ToString()) + "\"/>"));
                    builder.Append("" + colorName[i].ToString() + "</a>");
                    builder.Append(Out.Tab("</div>", ""));
                }
                if (k == endIndex)
                    break;
                k++;
            }

            // 分页
            builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));

            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("skinset.aspx?act=edit&amp;id=" + id + "") + "\">返回上级</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">配置中心</a><br />");
            builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else
        {
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("皮肤模板管理");
            builder.Append(Out.Tab("</div>", ""));
            string strName = string.Empty;
            int skinid = 0;
            if (xml.dss["skin2"].ToString() == "1")
            {
                strName += ",2";
            }
            else
            {
                skinid = 2;
            }
            if (xml.dss["skin3"].ToString() == "1")
            {
                strName += ",3";
            }
            else
            {
                if (skinid == 0)
                    skinid = 3;
            }
            if (xml.dss["skin4"].ToString() == "1")
            {
                strName += ",4";
            }
            else
            {
                if (skinid == 0)
                    skinid = 4;
            }
            if (xml.dss["skin5"].ToString() == "1")
            {
                strName += ",5";
            }
            else
            {
                if (skinid == 0)
                    skinid = 5;
            }
            if (xml.dss["skin6"].ToString() == "1")
            {
                strName += ",6";
            }
            else
            {
                if (skinid == 0)
                    skinid = 6;
            }
            if (xml.dss["skin7"].ToString() == "1")
            {
                strName += ",7";
            }
            else
            {
                if (skinid == 0)
                    skinid = 7;
            }
            if (xml.dss["skin8"].ToString() == "1")
            {
                strName += ",8";
            }
            else
            {
                if (skinid == 0)
                    skinid = 8;
            }
            if (xml.dss["skin9"].ToString() == "1")
            {
                strName += ",9";
            }
            else
            {
                if (skinid == 0)
                    skinid = 9;
            }
            strName = Utils.Mid(strName, 1, strName.Length);
            string[] strNameTemp = strName.Split(",".ToCharArray());
            int k = 1;
            for (int i = 0; i < strNameTemp.Length; i++)
            {
                if (k % 2 == 0)
                    builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                else
                    builder.Append(Out.Tab("<div>", "<br />"));

                builder.Append("<a href=\"" + Utils.getUrl("skinset.aspx?act=edit&amp;id=" + strNameTemp[i] + "") + "\">");
                builder.Append(Out.Tab("<font color=\"#" + xml.dss["div_title_color" + strNameTemp[i] + ""] + "\">■</font>", "<img src=\"/snap.aspx?act=color&amp;colorname=" + xml.dss["div_title_color" + strNameTemp[i] + ""] + "\"/>"));
                builder.Append("" + xml.dss["skinName" + strNameTemp[i] + ""] + "</a> ");
                if (i == 0)
                    builder.Append("默认");
                else
                    builder.Append("<a href=\"" + Utils.getUrl("skinset.aspx?act=del&amp;id=" + strNameTemp[i] + "") + "\">删</a>");

                builder.Append(Out.Tab("</div>", ""));
                k++;
            }

            builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
            builder.Append(Out.Tab("<div>", ""));
            if (skinid > 0)
            {
                builder.Append("<a href=\"" + Utils.getUrl("skinset.aspx?act=edit&amp;id=" + skinid + "") + "\">新增皮肤</a><br />");
            }
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">配置中心</a><br />");
            builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
            builder.Append(Out.Tab("</div>", "<br />"));
        }
    }

    private string getColor(int id,string[] GetValue)
    {
        string str = string.Empty;
        for (int i = 0; i < 13; i++)
        {
            if (Utils.Isie())
                str += "<a href=\"" + Utils.getUrl("skinset.aspx?act=color&amp;id=" + id + "&amp;n=" + i + "&amp;backurl=" + Utils.PostPage(1) + "") + "\"><font color=\"#" + GetValue[i].ToString() + "\">■</font><／a>'";
            else
                str += "<a href=\"" + Utils.getUrl("skinset.aspx?act=color&amp;id=" + id + "&amp;n=" + i + "&amp;backurl=" + Utils.PostPage(1) + "") + "\"><img src=\"/snap.aspx?act=color&amp;colorname=" + GetValue[i].ToString() + "\"/><／a>'";
        }
        return str;
    }
}