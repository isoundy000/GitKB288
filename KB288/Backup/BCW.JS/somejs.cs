using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;

/// <summary>
/// 新增添加到粘贴板JS
/// 黄国军 20160526
/// </summary>
namespace BCW.JS
{
    public class somejs
    {
        protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
        public somejs()
        { }
        #region  成员方法
        //倒计时JS,endtime为最后时间，可形成现在到最后时间的倒计时
        //彩色版本使用
        public string daojishi(string divname, DateTime endtime)
        {
            builder.Append("<script language=\"javascript\" type=\"text/javascript\"> ");
            builder.Append("var interval = 1000; ");
            builder.Append("function ShowCountDown(year,month,day,hours,minute,second,divname){ ");
            builder.Append("var now = new Date();");
            builder.Append("var endDate = new Date(year, month-1, day,hours,minute,second);");
            builder.Append("var leftTime=endDate.getTime()-now.getTime();");
            builder.Append("var leftsecond = parseInt(leftTime/1000);");
            builder.Append("var day1=Math.floor(leftsecond/(60*60*24)); ");
            builder.Append("var hour=Math.floor((leftsecond-day1*24*60*60)/3600); ");
            builder.Append("var minute=Math.floor((leftsecond-day1*24*60*60-hour*3600)/60);");
            builder.Append("var second=Math.floor(leftsecond-day1*24*60*60-hour*3600-minute*60);");
            builder.Append("var cc = document.getElementById(divname);");
            builder.Append("if(day1==0&&hour!=0&&minute!=0&&second!=0){");
            builder.Append("cc.innerHTML =hour+\"小时\"+minute+\"分\"+second+\"秒\"");
            builder.Append("}");
            builder.Append("else if(day1==0&&hour!=0&&minute==0&&second!=0){");
            builder.Append("cc.innerHTML =hour+\"小时\"+second+\"秒\"");
            builder.Append("}");
            builder.Append("else if(day1==0&&hour!=0&&minute!=0&&second==0){");
            builder.Append("cc.innerHTML =hour+\"小时\"+minute+\"分\"");
            builder.Append("}");
            builder.Append("else if(day1==0&&hour!=0&&minute==0&&second==0){");
            builder.Append("cc.innerHTML =hour+\"小时\"");
            builder.Append("}");
            builder.Append("else if(day1==0&&hour==0&&minute!=0&&second!=0){");
            builder.Append("cc.innerHTML =minute+\"分\"+second+\"秒\"");
            builder.Append("}");
            builder.Append("else if(day1==0&&hour==0&&minute!=0&&second==0){");
            builder.Append("cc.innerHTML =minute+\"分\"");
            builder.Append("}");
            builder.Append("else if(day1==0&&hour==0&&minute==0&&second!=0){");
            builder.Append("cc.innerHTML = second+\"秒\"");
            builder.Append("}");
            builder.Append("else if(day1==0&&hour==0&&minute==0&&second==0){");
            builder.Append("cc.innerHTML = 0+\"秒\";");
            builder.Append("location.reload(true);");
            builder.Append("}");
            builder.Append("else if(day1>=0){ ");
            builder.Append("cc.innerHTML = day1+\"天\"+hour+\"小时\"+minute+\"分\"+second+\"秒\"");
            builder.Append("}");
            builder.Append("}");
            int endyear = Convert.ToInt32(endtime.ToString("yyyy"));
            int endmon = Convert.ToInt32(endtime.ToString("MM"));
            int endday = Convert.ToInt32(endtime.ToString("dd"));
            int endhour = Convert.ToInt32(endtime.ToString("HH"));
            int endmin = Convert.ToInt32(endtime.ToString("mm"));
            int endsec = Convert.ToInt32(endtime.ToString("ss"));
            builder.Append("window.setInterval(function(){ShowCountDown(" + endyear + "," + endmon + "," + endday + "," + endhour + "," + endmin + "," + endsec + ",'" + divname + "');}, interval); ");
            builder.Append("</script>");
            builder.Append("<div id=\"" + divname + "\" style=\"color:red;padding:0;margin:0;display:inline\" ></div>");
            return builder.ToString().TrimEnd();
        }
        //倒计时JS,endtime为最后时间，可形成现在到最后时间的倒计时
        //1版本使用
        public string daojishi2(string divname, DateTime endtime)
        {
            builder.Append("<script language=\"javascript\" type=\"text/javascript\"> ");
            builder.Append("var interval = 1000; ");
            builder.Append("function ShowCountDown(year,month,day,hours,minute,second,divname){ ");
            builder.Append("var now = new Date();");
            builder.Append("var endDate = new Date(year, month-1, day,hours,minute,second);");
            builder.Append("var leftTime=endDate.getTime()-now.getTime();");
            builder.Append("var leftsecond = parseInt(leftTime/1000);");
            builder.Append("var day1=Math.floor(leftsecond/(60*60*24)); ");
            builder.Append("var hour=Math.floor((leftsecond-day1*24*60*60)/3600); ");
            builder.Append("var minute=Math.floor((leftsecond-day1*24*60*60-hour*3600)/60);");
            builder.Append("var second=Math.floor(leftsecond-day1*24*60*60-hour*3600-minute*60);");
            builder.Append("var cc = document.getElementById(divname);");
            builder.Append("if(day1==0&amp;&amp;hour!=0&amp;&amp;minute!=0&amp;&amp;second!=0){");
            builder.Append("cc.innerHTML =hour+\"小时\"+minute+\"分\"+second+\"秒\"");
            builder.Append("}");
            builder.Append("else if(day1==0&amp;&amp;hour!=0&amp;&amp;minute==0&amp;&amp;second!=0){");
            builder.Append("cc.innerHTML =hour+\"小时\"+second+\"秒\"");
            builder.Append("}");
            builder.Append("else if(day1==0&amp;&amp;hour!=0&amp;&amp;minute!=0&amp;&amp;second==0){");
            builder.Append("cc.innerHTML =hour+\"小时\"+minute+\"分\"");
            builder.Append("}");
            builder.Append("else if(day1==0&amp;&amp;hour!=0&amp;&amp;minute==0&amp;&amp;second==0){");
            builder.Append("cc.innerHTML =hour+\"小时\"");
            builder.Append("}");
            builder.Append("else if(day1==0&amp;&amp;hour==0&amp;&amp;minute!=0&amp;&amp;second!=0){");
            builder.Append("cc.innerHTML =minute+\"分\"+second+\"秒\"");
            builder.Append("}");
            builder.Append("else if(day1==0&amp;&amp;hour==0&amp;&amp;minute!=0&amp;&amp;second==0){");
            builder.Append("cc.innerHTML =minute+\"分\"");
            builder.Append("}");
            builder.Append("else if(day1==0&amp;&amp;hour==0&amp;&amp;minute==0&amp;&amp;second!=0){");
            builder.Append("cc.innerHTML = second+\"秒\"");
            builder.Append("}");
            builder.Append("else if(day1==0&amp;&amp;hour==0&amp;&amp;minute==0&amp;&amp;second==0){");
            builder.Append("cc.innerHTML = 0+\"秒\";");
            builder.Append("location.reload(true);");
            builder.Append("}");
            builder.Append("else if(day1>=0){ ");
            builder.Append("cc.innerHTML = day1+\"天\"+hour+\"小时\"+minute+\"分\"+second+\"秒\"");
            builder.Append("}");
            builder.Append("}");
            int endyear = Convert.ToInt32(endtime.ToString("yyyy"));
            int endmon = Convert.ToInt32(endtime.ToString("MM"));
            int endday = Convert.ToInt32(endtime.ToString("dd"));
            int endhour = Convert.ToInt32(endtime.ToString("HH"));
            int endmin = Convert.ToInt32(endtime.ToString("mm"));
            int endsec = Convert.ToInt32(endtime.ToString("ss"));
            builder.Append("window.setInterval(function(){ShowCountDown(" + endyear + "," + endmon + "," + endday + "," + endhour + "," + endmin + "," + endsec + ",'" + divname + "');}, interval); ");
            builder.Append("</script>");
            builder.Append("<div id=\"" + divname + "\" style=\"color:red;padding:0;margin:0;display:inline\" ></div>");
            return builder.ToString().TrimEnd();
        }
        public string daojishi3(string divname, DateTime endtime)
        {
            builder.Append("<script language=\"javascript\" type=\"text/javascript\"> ");
            builder.Append("var interval = 1000; ");
            builder.Append("function ShowCountDown(year,month,day,hours,minute,second,divname){ ");
            builder.Append("var now = new Date();");
            builder.Append("var endDate = new Date(year, month-1, day,hours,minute,second);");
            builder.Append("var leftTime=endDate.getTime()-now.getTime();");
            builder.Append("var leftsecond = parseInt(leftTime/1000);");
            builder.Append("var day1=Math.floor(leftsecond/(60*60*24)); ");
            builder.Append("var hour=Math.floor((leftsecond-day1*24*60*60)/3600); ");
            builder.Append("var minute=Math.floor((leftsecond-day1*24*60*60-hour*3600)/60);");
            builder.Append("var second=Math.floor(leftsecond-day1*24*60*60-hour*3600-minute*60);");
            builder.Append("var cc = document.getElementById(divname);");
            builder.Append("if(day1==0&amp;&amp;hour!=0&amp;&amp;minute!=0&amp;&amp;second!=0){");
            builder.Append("cc.innerHTML =hour+\"小时\"+minute+\"分\"+second+\"秒\"");
            builder.Append("}");
            builder.Append("else if(day1==0&amp;&amp;hour!=0&amp;&amp;minute==0&amp;&amp;second!=0){");
            builder.Append("cc.innerHTML =hour+\"小时\"+second+\"秒\"");
            builder.Append("}");
            builder.Append("else if(day1==0&amp;&amp;hour!=0&amp;&amp;minute!=0&amp;&amp;second==0){");
            builder.Append("cc.innerHTML =hour+\"小时\"+minute+\"分\"");
            builder.Append("}");
            builder.Append("else if(day1==0&amp;&amp;hour!=0&amp;&amp;minute==0&amp;&amp;second==0){");
            builder.Append("cc.innerHTML =hour+\"小时\"");
            builder.Append("}");
            builder.Append("else if(day1==0&amp;&amp;hour==0&amp;&amp;minute!=0&amp;&amp;second!=0){");
            builder.Append("cc.innerHTML =minute+\"分\"+second+\"秒\"");
            builder.Append("}");
            builder.Append("else if(day1==0&amp;&amp;hour==0&amp;&amp;minute!=0&amp;&amp;second==0){");
            builder.Append("cc.innerHTML =minute+\"分\"");
            builder.Append("}");
            builder.Append("else if(day1==0&amp;&amp;hour==0&amp;&amp;minute==0&amp;&amp;second!=0){");
            builder.Append("cc.innerHTML = second+\"秒\"");
            builder.Append("}");
            builder.Append("else if(day1==0&amp;&amp;hour==0&amp;&amp;minute==0&amp;&amp;second==0){");
            builder.Append("cc.innerHTML = 0+\"秒\";");
            builder.Append("location.reload(true);");
            builder.Append("}");
            builder.Append("else if(day1>=0){ ");
            builder.Append("cc.innerHTML = day1+\"天\"+hour+\"小时\"+minute+\"分\"+second+\"秒\"");
            builder.Append("}");
            builder.Append("}");
            int endyear = Convert.ToInt32(endtime.ToString("yyyy"));
            int endmon = Convert.ToInt32(endtime.ToString("MM"));
            int endday = Convert.ToInt32(endtime.ToString("dd"));
            int endhour = Convert.ToInt32(endtime.ToString("HH"));
            int endmin = Convert.ToInt32(endtime.ToString("mm"));
            int endsec = Convert.ToInt32(endtime.ToString("ss"));
            builder.Append("window.setInterval(function(){ShowCountDown(" + endyear + "," + endmon + "," + endday + "," + endhour + "," + endmin + "," + endsec + ",'" + divname + "');}, interval); ");
            builder.Append("</script>");
            builder.Append("<div id=\"" + divname + "\" style=\"color:red;padding:0;margin:0;display:inline\" ></div>");
            return builder.ToString().TrimEnd();
        }
        /// <summary>
        /// 文字列表向上滚动
        /// str为传入标签组
        /// height为滚动块的高度
        /// speed为滚动速度
        /// divname为滚动div块的ID，同一页面不可相同
        /// </summary>
        public string topfloat(string str, string divname, string height, string speed)
        {
            builder.Append("<style>");
            builder.Append("#" + divname + " a{height:30px;line-height: 30px;margin:0}");
            builder.Append("</style>");
            builder.Append("<div style=\"height:" + height + "px;width:100%;overflow:hidden;\">");
            builder.Append("<div id='" + divname + "'>");
            builder.Append(str);
            builder.Append("</div>");
            builder.Append("</div> ");
            builder.Append("<script language=\"javascript\" type=\"text/javascript\">");
            builder.Append("(function(){");
            builder.Append("var marquee = document.getElementById('" + divname + "');");
            builder.Append("var offset=0;");
            builder.Append("var scrollheight =marquee.offsetHeight;");
            builder.Append("var firstNode = marquee.children[0].cloneNode(true);");
            builder.Append("marquee.appendChild(firstNode);");
            builder.Append("setInterval(function(){");
            builder.Append("if(offset == scrollheight){");
            builder.Append("offset = 0;}");
            builder.Append("marquee.style.marginTop = \"-\"+offset+\"px\";");
            builder.Append("offset += 1;");
            builder.Append("}," + speed + ");");
            builder.Append("})();");
            builder.Append("</script>");
            return builder.ToString();
        }

        #region 复制到粘贴板 CopyToClipboard
        /// <summary>
        /// 复制到粘贴板
        /// </summary>
        /// <returns></returns>
        public string CopyToClipboard()
        {
            builder.Append("<script>");
            builder.Append("function CopyText(id)");
            builder.Append("{copy(document.getElementById(id).value);}");

            builder.Append("function copy(text2copy) {");
            builder.Append("if (window.clipboardData)");
            builder.Append("{window.clipboardData.setData(\"Text\", text2copy);}");
            builder.Append("else {");
            builder.Append("var flashcopier = 'flashcopier';");
            builder.Append("if (!document.getElementById(flashcopier))");
            builder.Append("{");
            builder.Append("var divholder = document.createElement('div');");
            builder.Append("divholder.id = flashcopier;");
            builder.Append("document.body.appendChild(divholder);");
            builder.Append("}");
            builder.Append("document.getElementById(flashcopier).innerHTML = '';");
            //这里是关键  
            builder.Append("var divinfo='<embed src=\"_clipboard.swf\" FlashVars=\"clipboard=' + escape(text2copy) + '\" width=\"0\" height=\"0\" type=\"application/x-shockwave-flash\"></embed>';");
            builder.Append("document.getElementById(flashcopier).innerHTML = divinfo;");
            builder.Append("alert('Text copied');");
            builder.Append("}");
            builder.Append("}");
            builder.Append("function copyToClipboard(meintext)");
            builder.Append("{");
            builder.Append("if (window.clipboardData)");
            builder.Append("{");
            builder.Append("window.clipboardData.setData(\"Text\", meintext);");
            builder.Append("}");
            builder.Append("else if (window.netscape)");
            builder.Append("{");
            builder.Append("try");
            builder.Append("{");
            builder.Append("netscape.security.PrivilegeManager.enablePrivilege(\"UniversalXPConnect\");");
            builder.Append("}");
            builder.Append("catch (e)");
            builder.Append("{");
            builder.Append("alert(\"被浏览器拒绝！\");");
            builder.Append("}");
            builder.Append("var clip = Components.classes['@mozilla.org/widget/clipboard;1'].");
            builder.Append("createInstance(Components.interfaces.nsIClipboard);");
            builder.Append("if (!clip) return;");
            builder.Append("var trans = Components.classes['@mozilla.org/widget/transferable;1'].");
            builder.Append("createInstance(Components.interfaces.nsITransferable);");
            builder.Append("if (!trans) return;");
            builder.Append("trans.addDataFlavor('text/unicode');");
            builder.Append("var str = new Object();");
            builder.Append("var len = new Object();");
            builder.Append("var str = Components.classes[\"@mozilla.org/supports-string;1\"].");
            builder.Append("createInstance(Components.interfaces.nsISupportsString);");
            builder.Append("var copytext = meintext;");
            builder.Append("str.data = copytext;");
            builder.Append("trans.setTransferData(\"text/unicode\", str, copytext.length * 2);");
            builder.Append("var clipid = Components.interfaces.nsIClipboard;");
            builder.Append("if (!clip) return false;");
            builder.Append("clip.setData(trans, null, clipid.kGlobalClipboard);");
            builder.Append("}");
            builder.Append("alert(\"复制成功: \" + meintext);");
            builder.Append("return false;");
            builder.Append("}");
            builder.Append("</script>");
            return builder.ToString().TrimEnd();
        }
        #endregion

        public string LoadJQuery()
        {
            builder.Append("<script type =\"text/javascript\" src =\"..\\..\\Javascript\\jquery-3.1.0.min.js\" ></script>");
            return builder.ToString();
        }
        #endregion  成员方法
        public string newDaojishi(string divname,DateTime endtime)
        {
            builder.Append("<script type =\"text/javascript\" src =\"..\\..\\Javascript\\daojishi.js\" ></script>");
            builder.Append("<div id=\"" + divname + "\" style=\"color:red;padding:0;margin:0;display:inline\" ></div>");
            builder.Append("<script type =\"text/javascript\" >");
            builder.Append("daojishi('"+ divname + "','"+endtime+"','"+DateTime.Now+"')");
            builder.Append("</script>");
            return builder.ToString();
        }
    }
}
