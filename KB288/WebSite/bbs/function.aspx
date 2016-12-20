<%@ Page Language="C#" MasterPageFile="MasterPage.master" AutoEventWireup="true" CodeFile="function.aspx.cs" Inherits="bbs_function" Title="无标题页" %>
<%@ MasterType TypeName="BCW.Common.BaseMaster" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
   <% if (BCW.Common.Utils.Isie())%>
   <% {%>
<script language="javascript" type="text/javascript">
	function Submitshare(uid)
	{
			document.rdshare.ToId.value=uid;  
			document.rdshare.submit();   
	}
</script>
 <% }%>
<% = BCW.Common.Utils.ForWordType(builder.ToString())%>
</asp:Content>


