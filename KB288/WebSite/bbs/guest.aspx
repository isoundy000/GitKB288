<%@ Page Language="C#" MasterPageFile="~/bbs/MasterPage.master" AutoEventWireup="true" CodeFile="guest.aspx.cs" Inherits="bbs_guest" Title="无标题页" %>
<%@ MasterType TypeName="BCW.Common.BaseMaster" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<% = BCW.Common.Utils.ForWordType(builder.ToString())%>
</asp:Content>

