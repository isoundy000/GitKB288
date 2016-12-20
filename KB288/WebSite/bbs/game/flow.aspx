<%@ Page Language="C#" MasterPageFile="MasterPage.master" AutoEventWireup="true" CodeFile="flow.aspx.cs" Inherits="bbs_game_flow" Title="无标题页" %>
<%@ MasterType TypeName="BCW.Common.BaseMaster" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
   <% if (BCW.Common.Utils.Isie())%>
   <% {%>
<style type=\"text/css\">
.btn-input2     {height:21px;border-width:0px;border-style:solid;background-color:#FFFFFF;color:#0A63BB }
form,input{display: inline;}
</style>
 <% }%>
<% = BCW.Common.Utils.ForWordType(builder.ToString())%>
</asp:Content>

