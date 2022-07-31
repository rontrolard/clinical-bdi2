<%@ Page Language="C#" MasterPageFile="~/IntroOneLogo.Master" AutoEventWireup="true" CodeBehind="CustomerRegistration.aspx.cs" Inherits="BDI2Web.CustomerRegistration.CustomerRegistration" Title="Customer Registration" %>
<asp:Content ID="Content1" ContentPlaceHolderID="IntroContentPlaceHolder" runat="server">
        <table width="600px">
            <tr>
                <td align="left" valign="middle">
                    <asp:Label ID='lblTitle' runat='server'>Registration</asp:Label>
                </td>
            </tr>
            <tr>
                <td class="text">
                    Enter the contact information for your organization. The fields marked with a red asterisk (<span class="Info">*</span>) are required. Click the <b>Submit</b> button to proceed.
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Panel ID="mainPanel" runat="server">
                    </asp:Panel>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Button ID='btnSubmit' runat='server' Text='Submit' OnClick="btnSubmit_Click" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Panel ID='panelValidationMsg' runat="server"></asp:Panel>
                </td>
            </tr>          
        </table>
        <input type="hidden" id="SettingControlsIdentifiers" runat="server" />
</asp:Content>
