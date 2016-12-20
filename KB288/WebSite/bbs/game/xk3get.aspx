<%@ Page Language="C#" AutoEventWireup="true" CodeFile="xk3get.aspx.cs" Inherits="bbs_game_xinkuai3_getnum" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>新快3_开奖数据获取程序</title>
    <meta http-equiv="refresh" content="5" />
</head>
<body>
<%--<SCRIPT language="JavaScript"><!--
function getCookieVal(offset)

{var endstr=document.cookie.indexOf(";",offset);if(endstr==-1)

endstr=document.cookie.length;return unescape(document.cookie.substring(offset,endstr));}function GetCookie(name)

{var arg=name+"=";var alen=arg.length;var clen=document.cookie.length;var i=0;while(i<clen)

{var j=i+alen;if(document.cookie.substring(i,j)==arg)

return getCookieVal(j);i=document.cookie.indexOf(" ",i)+1;if(i==0)

break;

}return null;}function SetCookie(name,value)

{var argv=SetCookie.arguments;var argc=SetCookie.arguments.length;var expires=(2<argc)?argv[2]:null;var path=(3<argc)?argv[3]:null;var domain=(4<argc)?argv[4]:null;var secure=(5<argc)?argv[5]:false;document.cookie=name+"="+escape(value)+((expires==null)?"":("; expires="+expires.toGMTString()))+((path==null)?"":("; path="+path))+((domain==null)?"":("; domain="+domain))+((secure==true)?"; secure":"");}function ResetCounts(name)

{visits=0;SetCookie("visits",visits,expdate,"/",null,false);location.reload();}
//-->
</SCRIPT>
<SCRIPT language="JavaScript">
<!--
var expdate=new Date();
var visits;
expdate.setTime(expdate.getTime()+(24*60*60));
if(!(visits=GetCookie("visits")))
    visits=0;
    visits++;
    SetCookie("visits",visits,expdate,"/",null,false);
document.write("&nbsp;&nbsp;&nbsp;共累计刷新本页"+"<FONT COLOR=red>"+visits+"</FONT>"+"次！");
//-->
</SCRIPT>--%>
</body>
</html>
