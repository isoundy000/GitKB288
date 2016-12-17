<%@ Page Language="C#" MasterPageFile="../MasterPage.master" AutoEventWireup="true" CodeFile="SetQuestion.aspx.cs" Inherits="bbs_pwd_SetQuestion" Title="设置密保问题" %>
<%@ MasterType TypeName="BCW.Common.BaseMaster" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<% = BCW.Common.Utils.ForWordType(builder.ToString())%>
</asp:Content>


