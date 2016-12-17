<%@ Page Language="C#" MasterPageFile="../MasterPage.master" AutoEventWireup="true" CodeFile="MyPhone.aspx.cs" Inherits="bbs_pwd_MyPhone" Title="我的手机号码" %>
<%@ MasterType TypeName="BCW.Common.BaseMaster" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<% = BCW.Common.Utils.ForWordType(builder.ToString())%>
</asp:Content>


