<%@ Page Language="C#" MasterPageFile="MasterPage.master" AutoEventWireup="true" CodeFile="book.aspx.cs" Inherits="Manage_book" Title="无标题页" %>
<%@ MasterType TypeName="BCW.Common.BaseMaster" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<% = BCW.Common.Utils.ForWordType(builder.ToString())%>
</asp:Content>


