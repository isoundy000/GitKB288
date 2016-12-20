<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="boCollecView.aspx.cs" Inherits="bbs_guess2_boCollecView" Title="波胆抓取页" %>

<%@ MasterType TypeName="BCW.Common.BaseMaster" %>
<asp:content id="Content1" contentplaceholderid="ContentPlaceHolder1" runat="Server">
<% = BCW.Common.Utils.ForWordType(builder.ToString())%>
</asp:content>