<!doctype html>
<!--[if lt IE 7]> <html class="ie6 oldie"> <![endif]-->
<!--[if IE 7]>    <html class="ie7 oldie"> <![endif]-->
<!--[if IE 8]>    <html class="ie8 oldie"> <![endif]-->
<!--[if gt IE 8]><!-->
<html class="">
<!--<![endif]-->
<head>
<meta charset="UTF-8">
<meta name="viewport" content="width=device-width, initial-scale=1">
<title></title>
<link href="boilerplate.css" rel="stylesheet" type="text/css">
<link href="stepy.css" rel="stylesheet" type="text/css">
<!-- 
要详细了解文件顶部 html 标签周围的条件注释:
paulirish.com/2008/conditional-stylesheets-vs-css-hacks-answer-neither/

如果您使用的是 modernizr (http://www.modernizr.com/) 的自定义内部版本，请执行以下操作:
* 在此处将链接插入 js 
* 将下方链接移至 html5shiv
* 将“no-js”类添加到顶部的 html 标签
* 如果 modernizr 内部版本中包含 MQ Polyfill，您也可以将链接移至 respond.min.js 
-->
<!--[if lt IE 9]>
<script src="//html5shiv.googlecode.com/svn/trunk/html5.js"></script>
<![endif]-->
<script src="respond.min.js"></script>
</head>
<body>
<div class="gridContainer clearfix">
  <div id="LayoutDiv1">
  <!--#include file="conn.asp"-->
<%
id=request("threadId")
set rs=server.CreateObject("adodb.recordset")
rs.open "select * from tb_text where ID="&id&"",conn,1,1
%>
<%
set rss=server.CreateObject("adodb.recordset")
rss.open "select Photo from tb_User where ID="&rs("UsID")&"",conn,1,1
%>

 <table width="100%" border="0" cellpadding="0" cellspacing="0">
  <tr>
    <td><table width="100%" border="0" cellspacing="0" cellpadding="0">
      <tr>


        <td width="12%" rowspan="2"  class="img">
               <%  =""  then%>
		 <img src="images/weixi.png">
         <%else%>
         <img src="<%=rss("Photo")%>">
         <%end if%>

	</td>
         <td width="3%" rowspan="2" >&nbsp;</td>
        <td colspan="3"  class="title"><%=rs("title")%></td>
      </tr>
      <tr> 
        <td width="22%"><%=rs("usname")%></td>
        <td width="50%" class="time"><%=rs("addTime")%></td>
        <td width="13%" class="ReadNum"  ><img src="images/bbs_view_ico@2x.png" height="11" width="11" ><%=rs("ReadNum")%></td>
      </tr>
    </table></td>
  </tr>
  <tr>
    <td  height="10"></td>
  </tr>
    <tr>
    <td class="line"></td>
  </tr>
  <tr>
    <td class="conter"><%=rs("content")%></td>
  </tr>
</table>

  </div>
</div>
</body>
</html>
