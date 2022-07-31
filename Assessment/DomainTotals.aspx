<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DomainTotals.aspx.cs" Inherits="BDI2Web.Assessment.DomainTotals" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Domain Totals</title>
    <style type="text/css">
		@import url( ../StyleSheets/BDI2.css );		
	</style>
</head>
<body style="font-size: 11px; font-family: verdana;">
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
        <div style="z-index: 101; left: 10px; width: 540px; position: absolute; top: 10px">
		<table width="100%" border="0" cellpadding="2" cellspacing="0" style="border:1px solid #BEBEBE; width:400px">            
            <tr>
                <td style="width: 540px">
                <asp:Repeater ID="RepeaterDomain" runat="server" OnItemDataBound="RepeaterDomain_ItemDataBound">
                    <HeaderTemplate>
                        <table cellspacing="0" cellpadding="0" style="width: 540px" border="0">
                        <tr>
                        <td colspan="4" style="background-color: #105fad; height: 5px;"></td>
                        </tr>                                           
                        <tr>
                            <td colspan="4" style="background-color: #105fad;">
                                &nbsp;<asp:Label ID="LabelTitle" runat="server" ForeColor="White" Text="Complete Assessment Domain Total"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                        <td colspan="4" style="background-color: #105fad; height: 5px;"></td>
                        </tr>                                           
                        <tr>
                        <td colspan="4" style="background-color: #fde5a3; height: 5px;"></td>
                        </tr>                                           
                        <tr>
                        <td style="width: 220px; background-color: #fde5a3">
                                &nbsp;<asp:Label ID="Label11" runat="server" Text='Domain'></asp:Label>                        
                        </td>
                        <td align="center"style="width: 100px; background-color: #fde5a3">
                                <asp:Label ID="LabelSS" runat="server" Text='Scaled Score'></asp:Label>                        
                        </td>
                        <td align="center" style="width: 100px; background-color: #fde5a3">
                                <asp:Label ID="LabelDQ" runat="server" Text='Dev. Quotient'></asp:Label>                        
                        </td>
                        <td align="center" style="width: 120px; background-color: #fde5a3">
                                <asp:Label ID="LabelPR" runat="server" Text='Percentile Rank'></asp:Label>                        
                        </td>
                        </tr>
                        <tr>
                        <td colspan="4" style="background-color: #fde5a3; height: 5px;"></td>
                        </tr>                                           
                    </HeaderTemplate>
                    
                    <ItemTemplate>
                        <tr>
                            <td style="width: 220; background-color: #fef5de;">
                                <asp:HiddenField ID="studentDomainID" runat="server" Value='<%#Eval("resultStudentDomainID")%>' />
                                <asp:HiddenField ID="domainID" runat="server" Value='<%#Eval("domainID")%>' />
                                &nbsp;<asp:Label ID="Description" runat="server" Text='<%#Eval("description")%>'></asp:Label>
                            </td>

                            <td align="center" style="width: 100; background-color: #fef5de;">
                                <asp:Label ID="LabelSSContent" runat="server" Text='<%#Eval("scaledScore")%>'></asp:Label>
                            </td>

                            <td align="center" style="width: 100; background-color: #fef5de;">
                                <asp:Label ID="LabelDQContent" runat="server" Text='<%#Eval("DQScore")%>'></asp:Label>
                            </td>

                            <td align="center" style="width: 120; background-color: #fef5de;">
                                <asp:Label ID="LabelPRContent" runat="server" Text='<%#Eval("PRScore")%>'></asp:Label>
                            </td>
                        </tr>
                        <tr>
                        <td colspan="4" style="background-color: #fef5de; height: 5px;"></td>
                        </tr>                   
                   </ItemTemplate>
                    
                    <FooterTemplate>
                        <tr>
                        <td colspan="4" style="background-color: #fde5a3; height: 5px;"></td>
                        </tr>                                       
                        <tr>
                        <td style="background-color: #fde5a3">
                                &nbsp;<asp:Label ID="LabelTotal" runat="server" Text='BDI-2 Total'></asp:Label>                        
                        </td>
                        <td align="center" style="background-color: #fde5a3">
                                <asp:Label ID="LabelTotal1" runat="server" Text='<%=strSSScore%>'></asp:Label>                        
                        </td>
                        <td align="center" style="background-color: #fde5a3">
                                <asp:Label ID="LabelTotal2" runat="server" Text='<%=strDQScore%>'></asp:Label>                        
                        </td>
                        <td align="center" style="background-color: #fde5a3">
                                <asp:Label ID="LabelTotal3" runat="server" Text='<%=strPRScore%>'></asp:Label>                        
                        </td>
                        </tr> 
                        <tr>
                        <td colspan="4" style="background-color: #fde5a3; height: 5px;"></td>
                        </tr>                   
                    </table>
                    </FooterTemplate>
                </asp:Repeater>
                </td>
            </tr>
            
            <tr>
            <td style="height:5px;"></td>
            </tr>

            <tr>
            <td style="background-color: <%=strAEColor%>;"><asp:Label ID="LabelAE" runat="server" Visible="False"></asp:Label></td>
            </tr>
            
            <tr>
            <td style="height:5px;"></td>
            </tr>
            <tr>
            <td align="center"><asp:Button ID="ButtonClose" runat="server" OnClick="ButtonClose_Click" Text="Close" /></td>
            </tr>
            <tr>
            <td style="height:8px;"><asp:Literal ID="LiteralScript" runat="server"></asp:Literal></td>
            </tr>
		</table>
        </div>
    </form>
</body>
</html>
