<%@ Page Language="C#" MasterPageFile="MasterPage.master" AutoEventWireup="true" CodeFile="BQC.aspx.cs" Inherits="bbs_game_6changban" Title="6场半" %>
<%@ MasterType TypeName="BCW.Common.BaseMaster" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<% = BCW.Common.Utils.ForWordType(builder.ToString())%>
</asp:Content>