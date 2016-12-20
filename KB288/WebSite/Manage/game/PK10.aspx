<%@ Page Language="C#" MasterPageFile="../MasterPage.master" AutoEventWireup="true" CodeFile="PK10.aspx.cs" Inherits="Manage_game_PK10" Title="无标题页" %>
<%@ MasterType TypeName="BCW.Common.BaseMaster" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<% = BCW.Common.Utils.ForWordType(builder.ToString())%>
</asp:Content>