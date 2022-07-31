<%@ Page Language="C#" MasterPageFile="~/Intro.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="BDI2Web.Default" Title="Welcome to BDI-2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="IntroContentPlaceHolder" runat="server">
    <br /><br />
    <asp:Label ID="Label1" runat="server" Text="Please provide your login credentials"/><br />
    <br />
    <table style="width: 344px">
        <tr>
            <td style="width: 44px">
                <asp:Label ID="Label2" runat="server" SkinID="RegularText" Text="Login ID:" Width="57px"></asp:Label>
            </td>
            <td style="width: 100px">
                <asp:TextBox ID="txtLogin" runat="server" Height="15px" Width="100px"></asp:TextBox>
            </td>
            <td>&nbsp;
                <asp:RequiredFieldValidator ID="UserNameRFV" ControlToValidate="txtLogin" CssClass="errorMessage" ErrorMessage="User Name is required" runat="server" />
            </td>
        </tr>
        <tr>
            <td style="width: 44px">
                <asp:Label ID="Label3" runat="server" SkinID="RegularText" Text="Password:" Width="56px"></asp:Label>
            </td>
            <td style="width: 100px">
                <input style="display:none" type="password" name="fakepasswordremembered"/>
                <asp:TextBox ID="txtPassword" runat="server" Width="100px" Height="15px" TextMode="Password"></asp:TextBox> 
            </td>
            <td>&nbsp;
                <asp:RequiredFieldValidator ID="PasswordRFV" ControlToValidate="txtPassword" CssClass="errorMessage" ErrorMessage="Password is required" runat="server" />
            </td>
        </tr>
        <tr>
            <td colspan="3">
                <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="SendPassword.aspx" Width="156px">Forgot my password</asp:HyperLink>
            </td>
        </tr>
        <tr>
            <td colspan="3" style="width: 44px">
                <br />
                <asp:Button ID="btnLogin" runat="server" Text="Login" Height="20px" OnClick="btnLogin_Click"/>
            </td>
        </tr>
    </table>
    <asp:Label ID="lblError" runat="server" ForeColor="Red" Text="Label" Visible="False" Width="391px"></asp:Label>
</asp:Content>
