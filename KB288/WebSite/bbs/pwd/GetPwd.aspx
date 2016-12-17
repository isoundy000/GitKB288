<%@ Page Language="C#" MasterPageFile="../MasterPage.master" AutoEventWireup="true" CodeFile="GetPwd.aspx.cs" Inherits="bbs_pwd_GetPwd" Title="密保问题" %>
<%@ MasterType TypeName="BCW.Common.BaseMaster" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<% = BCW.Common.Utils.ForWordType(builder.ToString())%>
</asp:Content>


