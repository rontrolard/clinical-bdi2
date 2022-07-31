<%@ Page Language="C#" EnableTheming="true" Theme="default" AutoEventWireup="true" CodeBehind="TestCalendar.aspx.cs" Inherits="BDI2Web.Assessment.TestCalendar" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="radT" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Select Date and Domain(s)</title>
     <style type="text/css">
		@import url( ../StyleSheets/BDI2.css );		
	</style>
</head>
<body>
     <script type="text/javascript">  
        function GetRadWindow()
        {
            var oWindow = null;
            if (window.radWindow) oWindow = window.radWindow;
            else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
            return oWindow;
        } 
    
        function CloseWindow()
        {
            var oWin = GetRadWindow();
            oWin.Close();
        }
    </script>
    <form id="form1" runat="server">
    <radT:RadScriptManager ID="sm1" runat="server" />
    <div style="z-index: 101; left: 11px; width: 319px; position: absolute; top: 11px;
            height: 100px">
        <table style="font-size: 11px; width: 315px; font-family: verdana" cellspacing="0">
            <tr>
                <th align="left" colspan="4" style="font-weight: bold; height: 20px; width: 256px; background-color: #105fad; color:#FFFFCC;">
                    Select Date&nbsp;
                </th>
            </tr>
             <tr>
                    <td colspan="4" align="left">
                     <asp:Label ID="lblMessage" runat="server" SkinID="errorMessage"></asp:Label> </td>
             </tr>
            <tr>
                <th align="left" colspan="4" style="background-color: #ffffff; width: 256px; height: 163px;" valign="middle">
                    <radT:RadCalendar id="RadCalendar1" ShowRowHeaders="false" runat="server" font-names="Arial, Verdana, Tahoma"
                        forecolor="Black" style="border-left-color: #ececec; border-bottom-color: #ececec;
                        border-top-color: #ececec; border-right-color: #ececec" width="310px" enablemultiselect="False"></radT:RadCalendar>
                </th>
            </tr>
            <tr>
                    <th align="left" colspan="4" style="font-weight: bold; height: 20px;
                        background-color: #105fad; color:#FFFFCC; width: 256px;">
                        Choose domains for the selected date</th>
                </tr>
                <tr>
                    <td colspan="4" align="Left" style="font-weight:bold; height: 31px; background-color: #fde5a3; width: 256px;">
                        <asp:CheckBox ID="CheckBoxAll" runat="server" Text="All" OnCheckedChanged="CheckBoxAll_CheckedChanged" AutoPostBack="True" /></td>
                </tr>
                <tr>
                    <td  colspan="4" style="height: 14px; background-color: #fef5de; width: 250px;" >
                        <asp:Repeater ID="RepeaterDomain" runat="server" OnItemDataBound="RepeaterDomain_ItemDataBound">
                            <HeaderTemplate>
                                <table cellspacing="0" cellpadding="0" style="width: 315PX" border="0">
                            </HeaderTemplate>
                            <ItemTemplate>
                                <tr>
                                    <td style="width: 250; background-color: <%#Eval("rowColor")%>;">
                                        <asp:HiddenField ID="studentDomainID" runat="server" Value='<%#Eval("resultStudentDomainID")%>' />
                                        <asp:HiddenField ID="parentstudentDomainID" runat="server" Value='<%#Eval("parentResultStudentDomainID")%>' />
                                        <asp:HiddenField ID="domainID" runat="server" Value='<%#Eval("domainID")%>' />
                                        <asp:HiddenField ID="domainTypeID" runat="server" Value='<%#Eval("domainTypeID")%>' />
                                        <asp:CheckBox ID="domainSelect" runat="server" Checked="false" Text='<%#Eval("description")%>'></asp:CheckBox>
                                        <asp:Label ID="Description" runat="server" Text='<%#Eval("description")%>'></asp:Label>
                                    </td>
                                </tr>
                            </ItemTemplate>
                            <FooterTemplate>
                            </table>
                            </FooterTemplate>
                        </asp:Repeater>
                    </td>
                </tr>
                <tr>
                    <td colspan="4" style="height: 14px; background-color: #ffffff; width: 256px;">
                        <br />
                    </td>
                </tr>
            <tr>
                <td align="center" colspan="4" style="background-color: #ffffff; width: 256px;">
                    <asp:Button ID="ButtonContinue" runat="server" Text="Select and Continue" OnClick="ButtonContinue_Click"/>
                    <asp:Button ID="ButtonCancel" runat="server" Text="Cancel" OnClientClick="CloseWindow()" OnClick="ButtonCancel_Click"/></td>
            </tr>
        </table>
        <asp:Literal ID="LiteralScript" runat="server"></asp:Literal>
    </div>    
    </form>
</body>
</html>
