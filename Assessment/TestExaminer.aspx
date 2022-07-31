<%@ Page Language="C#" EnableTheming="true" Theme="default" AutoEventWireup="true" CodeBehind="TestExaminer.aspx.cs" Inherits="BDI2Web.Assessment.TestExaminer" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title></title>
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
    <div>
    <table class="searchTable" id="searchTable" style="width:530; BACKGROUND-COLOR: #fde5a3;" cellpadding="0" border="0" cellspacing="0">
    <caption style= "background-color: #105fad; color:#FFFFCC; font-weight:bold">Search for Examiner</caption>
    <tr>
        <td align="left" >First&nbsp;Name:</td>	
        <td align="left" >
            <asp:TextBox ID="TextBoxFirstName" runat="server" Width="130px" MaxLength="20"></asp:TextBox>
        </td>
        <td align="left">&nbsp;Last&nbsp;Name:</td>
	    <td align="left">
            <asp:TextBox ID="TextBoxLastName" runat="server" Width="130px" MaxLength="30"></asp:TextBox></td>
	</tr>
	
	<tr>
        <td align="left" >Site Name:</td>		
        <td align="left" >
            <asp:TextBox ID="TextBoxSiteName" runat="server" Width="130px" MaxLength="30"></asp:TextBox>
        </td>
        <td align="left" >&nbsp; </td>
        <td align="left" > 
          <asp:Button ID="ButtonSearch" runat="server" Text="Search" OnClick="ButtonSearch_Click" />
        </td>
	</tr>
	
    <tr><td style="height: 8px;" colspan="4">
        </td></tr>   
    
                <tr>
                    <th align="left" colspan="4" style="font-weight: bold; height: 20px; background-color: #105fad; color:#FFFFCC;">
                        Select Examiner by clicking on the radio button</th>
                </tr>
                <tr>
                    <td colspan="4" style="background-color: #fef5de">
                        <asp:GridView ID="GridView1" DataKeyNames="staffID" runat="server" Height="200px" Width="530px" AutoGenerateColumns="False" HorizontalAlign="Left">
                            <Columns>
                                <asp:TemplateField HeaderText="Select" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
                                    <ItemTemplate>
                                         <asp:RadioButton ID="RadioButtonSelect"  runat="server" Checked="false" OnCheckedChanged="RadioButtonSelect_CheckedChanged" AutoPostBack="true"/>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="fullName" HeaderText="Examiner Name" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle" />
                                <asp:BoundField DataField="siteName" HeaderText="Site Name" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle" />
                            </Columns>
                            <RowStyle BackColor="#FEF5DE" />
                            <HeaderStyle BackColor="#FDE5A3" />
                        </asp:GridView>
                    </td>
                </tr>
                <tr>
                    <td align="center" colspan="4" style="height: 14px; background-color: #fef5de"></td>
                </tr>
                
                <tr>
                    <th align="left" colspan="4" style="font-weight: bold; height: 20px;
                        background-color: #105fad; color:#FFFFCC;">
                        Choose domains for the selected examiner</th>
                </tr>
                <tr>
                    <td colspan="4" align="Left" style="font-weight:bold; height: 31px; background-color: #fde5a3; width: 256px;">
                        <asp:CheckBox ID="CheckBoxAll" runat="server" Text="All" OnCheckedChanged="CheckBoxAll_CheckedChanged" AutoPostBack="True" /></td>
                </tr>
                <tr>
                    <td  colspan="4" style="background-color: #fef5de; width: 530px;" >
                        <asp:Repeater ID="RepeaterDomain" runat="server" OnItemDataBound="RepeaterDomain_ItemDataBound">
                            <HeaderTemplate>
                                <table cellspacing="0" cellpadding="0" style="width: 530px" border="0">
                            </HeaderTemplate>
                            <ItemTemplate>
                                <tr>
                                    <td style="width: 530; background-color: <%#Eval("rowColor")%>;">
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
                        <asp:Label ID="LabelSelectedStaffID" runat="server" Visible="False"></asp:Label>
                        <asp:Label ID="LabelSelectedStaffName" runat="server" Visible="False"></asp:Label></td>
                </tr>                
                <tr>
                    <td align="center" colspan="4" style="height: 14px; background-color: #ffffff">
                        &nbsp;<asp:Literal ID="LiteralScript" runat="server"></asp:Literal></td>
                </tr>
                <tr>
                    <td align="center" colspan="4" style="height: 14px; background-color: #ffffff">
                        <asp:Button ID="ButtonContinue" runat="server" Text="Select and Continue" OnClick="ButtonContinue_Click" />&nbsp;
                        <asp:Button ID="ButtonCancel" runat="server" Text="Cancel" OnClientClick="CloseWindow()" OnClick="ButtonCancel_Click"/></td>
                </tr>
            </table>
        </div>
            
    </form>
</body>
</html>
