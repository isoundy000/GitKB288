<%@ Page Language="C#" MasterPageFile="MasterPage.master" AutoEventWireup="true" CodeFile="jqc.aspx.cs" Inherits="bbs_game_jqc" Title="进球彩" %>
<%@ MasterType TypeName="BCW.Common.BaseMaster" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<% = BCW.Common.Utils.ForWordType(builder.ToString())%>
</asp:Content>