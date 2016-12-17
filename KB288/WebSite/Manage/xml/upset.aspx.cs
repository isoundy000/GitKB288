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

public partial class Manage_xml_upset : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected void Page_Load(object sender, EventArgs e)
    {
        Master.Title = "上传系统设置";
        builder.Append(Out.Tab("", ""));
          
        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        ub xml = new ub();
        string xmlPath = "/Controls/upfile.xml";
        Application.Remove(xmlPath);//清缓存
        xml.ReloadSub(xmlPath); //加载配置
        if (Utils.ToSChinese(ac) == "确定修改")
        {
            string aFileExt = Utils.GetRequest("aFileExt", "post", 2, @"^[^\,]{1,50}(?:\,[^\,]{1,50}){0,500}$", "上传格式请用,分隔");
            string IsVerify = Utils.GetRequest("IsVerify", "post", 2, @"^[0-2]$", "是否启用审核选择出错");
            string AddNum = Utils.GetRequest("AddNum", "post", 2, @"^[0-9]\d*$", "上传个数填写错误");
            string aMaxFileNum = Utils.GetRequest("aMaxFileNum", "post", 2, @"^[1-9]\d*$", "最大上传文件数填写错误");
            string aMaxFileSize = Utils.GetRequest("aMaxFileSize", "post", 2, @"^[1-9]\d*$", "每个文件限多少K填写错误");
            string aHeight = Utils.GetRequest("aHeight", "post", 2, @"^[1-9]\d*$", "高度必须是整数,而且必须大于15");
            string aWidth = Utils.GetRequest("aWidth", "post", 3, @"^[1-9]\d*$", "宽度必须是整数,而且必须大于15");
            string aThumbType = Utils.GetRequest("aThumbType", "post", 2, @"^[1-2]$", "缩放性质选择出错");
            string aIsThumb = Utils.GetRequest("aIsThumb", "post", 2, @"^[0-2]$", "添加水印选择出错");
            string aWord = Utils.GetRequest("aWord", "post", 3, @"^[^\^]{1,100}$", "水印文字或者图片虚拟路径限100字内");
            string aWordColor = Utils.GetRequest("aWordColor", "post", 3, @"^[^\^]{7}$", "水印文字颜色选择错误");
            string aTran = Utils.GetRequest("aTran", "post", 3, @"^[0-9]\d*$", "图片水印透明度填写错误");
            string aPosition = Utils.GetRequest("aPosition", "post", 2, @"^[0-8]$", "水印位置选择错误");

            string CoverHeight = Utils.GetRequest("CoverHeight", "post", 2, @"^[1-9]\d*$", "封面高度必须是整数,而且必须大于15");
            string CoverWidth = Utils.GetRequest("CoverWidth", "post", 2, @"^[1-9]\d*$", "封面宽度必须是整数,而且必须大于15");
            string IsTextThumb = Utils.GetRequest("IsTextThumb", "post", 2, @"^[0-1]$", "文章附图水印选择出错");
            string IsPicThumb = Utils.GetRequest("IsPicThumb", "post", 2, @"^[0-1]$", "图片水印选择出错");
            string IsFileThumb = Utils.GetRequest("IsFileThumb", "post", 2, @"^[0-1]$", "文件附图水印选择出错");
            string IsShopThumb = Utils.GetRequest("IsShopThumb", "post", 2, @"^[0-1]$", "商品附图水印选择出错");
            string aPhFileSize = Utils.GetRequest("aPhFileSize", "post", 2, @"^[1-9]\d*$", "头像文件限多少K填写错误");

            string bFileExt = Utils.GetRequest("bFileExt", "post", 2, @"^[^\,]{1,50}(?:\,[^\,]{1,50}){0,500}$", "上传格式请用,分隔");
            string bMaxFileNum = Utils.GetRequest("bMaxFileNum", "post", 2, @"^[1-9]\d*$", "最大上传文件数填写错误");
            string bHeight = Utils.GetRequest("bHeight", "post", 2, @"^[1-9]\d*$", "高度必须是整数,而且必须大于15");
            string bWidth = Utils.GetRequest("bWidth", "post", 2, @"^[1-9]\d*$", "宽度必须是整数,而且必须大于15");
            string bThumbType = Utils.GetRequest("bThumbType", "post", 2, @"^[0-2]$", "生成缩略图选择出错");
            string bIsThumb = Utils.GetRequest("bIsThumb", "post", 2, @"^[1-2]$", "添加水印选择出错");
            string bWord = Utils.GetRequest("bWord", "post", 3, @"^[^\^]{1,100}$", "水印文字或者图片虚拟路径限100字内");
            string bWordColor = Utils.GetRequest("bWordColor", "post", 3, @"^[^\^]{7}$", "水印文字颜色选择错误");
            string bTran = Utils.GetRequest("bTran", "post", 3, @"^[0-9]\d*$", "图片水印透明度填写错误");
            string bPosition = Utils.GetRequest("bPosition", "post", 2, @"^[0-8]$", "水印位置选择错误");

            xml.dss["UpaFileExt"] = aFileExt;
            xml.dss["UpIsVerify"] = IsVerify;
            xml.dss["UpAddNum"] = AddNum;
            xml.dss["UpaMaxFileNum"] = aMaxFileNum;
            xml.dss["UpaMaxFileSize"] = aMaxFileSize;
            xml.dss["UpaHeight"] = aHeight;
            xml.dss["UpaWidth"] = aWidth;
            xml.dss["UpaThumbType"] = aThumbType;
            xml.dss["UpaIsThumb"] = aIsThumb;
            xml.dss["UpaWord"] = aWord;
            xml.dss["UpaWordColor"] = aWordColor;
            xml.dss["UpaTran"] = aTran;
            xml.dss["UpaPosition"] = aPosition;

            xml.dss["UpCoverHeight"] = CoverHeight;
            xml.dss["UpCoverWidth"] = CoverWidth;
            xml.dss["UpIsTextThumb"] = IsTextThumb;
            xml.dss["UpIsPicThumb"] = IsPicThumb;
            xml.dss["UpIsFileThumb"] = IsFileThumb;
            xml.dss["UpIsShopThumb"] = IsShopThumb;
            xml.dss["UpaPhFileSize"] = aPhFileSize;

            xml.dss["UpbFileExt"] = bFileExt;
            xml.dss["UpbMaxFileNum"] = bMaxFileNum;
            xml.dss["UpbHeight"] = bHeight;
            xml.dss["UpbWidth"] = bWidth;
            xml.dss["UpbThumbType"] = bThumbType;
            xml.dss["UpbIsThumb"] = bIsThumb;
            xml.dss["UpbWord"] = bWord;
            xml.dss["UpbWordColor"] = bWordColor;
            xml.dss["UpbTran"] = bTran;
            xml.dss["UpbPosition"] = bPosition;
            System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            Utils.Success("上传系统设置", "设置成功，正在返回..", Utils.getUrl("upset.aspx"), "1");
        }
        else
        {
            builder.Append(Out.Div("title", "上传系统设置"));

            string strText = "=社区上传设置=/社区允许上传格式:/,是否启用审核:/,每人每天限上传个数(填0则不限):/,一次性最大上传文件数:/,每个文件限多少K:/,图片缩略图高度:/,图片缩略图宽度:/,缩放性质:/,添加水印:/,水印文字或者图片虚拟路径:/,水印文字颜色:/,图片水印透明度:/,水印位置:/,社区上传头像限多少K:/,=后台上传系统=/允许上传格式:/,添加水印:/,水印文字或者图片虚拟路径:/,水印文字颜色:/,图片水印透明度:/,水印位置:/,封面图片高度:/,封面图片宽度:/,文章附图水印:/,图片水印:/,文件截图水印:/,商品附图水印:/,=图片系统设置=/一次性最大上传文件数:/,图片缩略图高度(作默认):/,图片缩略图宽度(作默认):/,生成缩略图(作默认):/";
            string strName = "aFileExt,IsVerify,AddNum,aMaxFileNum,aMaxFileSize,aHeight,aWidth,aThumbType,aIsThumb,aWord,aWordColor,aTran,aPosition,aPhFileSize,bFileExt,bIsThumb,bWord,bWordColor,bTran,bPosition,CoverHeight,CoverWidth,IsTextThumb,IsPicThumb,IsFileThumb,IsShopThumb,bMaxFileNum,bHeight,bWidth,bThumbType";
            string strType = "textarea,select,snum,snum,snum,snum,snum,select,select,text,select,snum,select,snum,textarea,select,text,select,snum,select,snum,snum,select,select,select,select,snum,snum,snum,select";
            string strValu = "" + xml.dss["UpaFileExt"] + "'" + xml.dss["UpIsVerify"] + "'" + xml.dss["UpAddNum"] + "'" + xml.dss["UpaMaxFileNum"] + "'" + xml.dss["UpaMaxFileSize"] + "'" + xml.dss["UpaHeight"] + "'" + xml.dss["UpaWidth"] + "'" + xml.dss["UpaThumbType"] + "'" + xml.dss["UpaIsThumb"] + "'" + xml.dss["UpaWord"] + "'" + xml.dss["UpaWordColor"] + "'" + xml.dss["UpaTran"] + "'" + xml.dss["UpaPosition"] + "'" + xml.dss["UpaPhFileSize"] + "'" + xml.dss["UpbFileExt"] + "'" + xml.dss["UpbIsThumb"] + "'" + xml.dss["UpbWord"] + "'" + xml.dss["UpbWordColor"] + "'" + xml.dss["UpbTran"] + "'" + xml.dss["UpbPosition"] + "'" + xml.dss["UpCoverHeight"] + "'" + xml.dss["UpCoverWidth"] + "'" + xml.dss["UpIsTextThumb"] + "'" + xml.dss["UpIsPicThumb"] + "'" + xml.dss["UpIsFileThumb"] + "'" + xml.dss["UpIsShopThumb"] + "'" + xml.dss["UpbMaxFileNum"] + "'" + xml.dss["UpbHeight"] + "'" + xml.dss["UpbWidth"] + "'" + xml.dss["UpbThumbType"] + "";
            string strEmpt = "false,0|不用审核|1|审核图片|2|审核全部,false,false,false,false,false,1|保持比例|2|固定尺寸,0|不加水印|1|文字水印|2|图片水印,true,#DE0000|红色|#000000|黑色|#FFFFFF|白色|#0E71D4|蓝色|#0CB90D|绿色|#FD8300|黄色|#EF74DC|粉色,true,0|右下角|1|左上角|2|上中角|3|右上角|4|中左角|5|中心角|6|中右角|7|左下角|8|下中角,false,false,1|文字水印|2|图片水印,true,#DE0000|红色|#000000|黑色|#FFFFFF|白色|#0E71D4|蓝色|#0CB90D|绿色|#FD8300|黄色|#EF74DC|粉色,true,0|右下角|1|左上角|2|上中角|3|右上角|4|中左角|5|中心角|6|中右角|7|左下角|8|下中角,false,false,0|不加水印|1|添加水印,0|不加水印|1|添加水印,0|不加水印|1|添加水印,0|不加水印|1|添加水印,false,false,false,0|不生成|1|保持比例|2|固定尺寸";
            string strIdea = "/";
            string strOthe = "确定修改|reset,upset.aspx,post,1,red|blue";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("温馨提示:<br />1.动态图片缩放、加水印后一样会动哦.<br />2.定义添加文字水印时,文字颜色有效,透明度无效.<br />3.定义添加图片水印时,文字颜色无效,透明度有效(建议1-10).<br />5.动态图片添加文字水印时,选择文字颜色无效,系统根据图片色板自动调色.<br />6.多个上传格式使用半角逗号分隔,例子:.gif,.jpg,.mp3,.jar");
            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">配置中心</a><br />");
            builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
            builder.Append(Out.Tab("</div>", "<br />"));
        }
    }
}
