<%@ Page Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="RunScheduleJob.aspx.cs" Inherits="BDI2Web.ExIm.RunScheduleJob" Title="Run Scheduled Job" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="radT" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MCP" runat="server">
    <asp:Label ID="TitleLabel" runat="server" Text="Run Scheduled Job" SkinID="Title" />
    <asp:Panel ID="PasswordPanel" runat="server">
        <div class="text" style="width: 700px">
            <div>Password: <input style="display:none" type="password" name="fakepasswordremembered"/><asp:TextBox ID="RunJobPasswordTextBox" TextMode="Password" MaxLength="20" runat="server" />&nbsp;&nbsp;
                <asp:Button ID="PasswordButton" runat="server" Text="Submit" OnClick="PasswordButton_Click" />
            </div>       
        </div> 
    </asp:Panel>
    <br /><br />
    <asp:Panel ID="RunJobPanel" Width="80%" Visible="false" runat="server">   
        <div class="text" style="width: 700px">Run Job ID: <asp:TextBox ID="JobIDTextBox" runat="server" />&nbsp;&nbsp;
            <asp:Button ID="ProcessJobButton" Text="ProcessJob" runat="server" OnClick="ProcessJobButton_Click" />
        </div>
        <br /><br />
        <div class="text" style="width:700px">
            <table>
                <tr>
                    <td>
                        Job ID:
                    </td>
                    <td>
                        <asp:TextBox ID="EditJobIDTextBox" runat="server"/>
                    </td>
                </tr>
                <tr>
                    <td>
                        Schedule Status ID:
                    </td>
                    <td>
                        <asp:TextBox ID="ScheduleStatusIDTextBox" runat="server"/>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td><asp:Button ID="ChangeStatusButton" Text="Change Schedule Status" 
                            runat="server" onclick="ChangeStatusButton_Click" /> </td>
                </tr>
            </table>
           <br /><br />
           <table>
            <tr>
                <td>Get File for Job ID:</td>
                <td><asp:TextBox ID="GetFileJobIDTextBox" runat="server" /></td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td><asp:Button ID="GetFileButton" Text="Get File" runat="server" 
                        onclick="GetFileButton_Click" /></td>
            </tr>
           </table> 
           <br />
           <table>
                <tr>
                    <td>Start Date:</td>
                    <td><radt:RadDatePicker ID="StartDateRDP" runat="server" Calendar-ShowRowHeaders="false" /></td>
                </tr>
                <tr>
                    <td>End Date:</td>
                    <td><radT:RadDatePicker ID="EndDateRDP" Calendar-ShowRowHeaders="false" runat="server" /></td>
                </tr>
                <tr>
                    <td colspan="2"><asp:Button ID="ExpiredCustomerButton" Text="Expired Customer Report" runat="server" 
                            onclick="ExpiredCustomerButton_Click" /></td>
                </tr>
           </table>  
           <radT:RadGrid ID="ResultGrid" runat="server" Visible="false" />
        </div>
    </asp:Panel>
</asp:Content>
