<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="rebuildcache.aspx.cs" Inherits="rebuildcache" Title="重建缓存" %>
<%@ MasterType TypeName="BCW.Common.BaseMaster" %>
<asp:content id="Content1" contentplaceholderid="ContentPlaceHolder1" runat="Server">
<% = BCW.Common.Utils.ForWordType(builder.ToString())%>
</asp:content>