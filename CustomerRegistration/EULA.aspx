<%@ Page Language="C#" MasterPageFile="~/IntroOneLogo.Master" AutoEventWireup="true" CodeBehind="EULA.aspx.cs" Inherits="BDI2Web.CustomerRegistration.EULA" Title="EULA Acceptance" %>
<asp:Content ID="Content1" ContentPlaceHolderID="IntroContentPlaceHolder" runat="server">
    <asp:Label ID="lblTitle" runat="server" Text="Web Activation - License Agreement"></asp:Label>
    <div class="text" style="width: 700px">
        Read the following Web Activation License Agreement. Scroll down to view the entire agreement, and click the <b>Accept</b> button to proceed. To close the program without completing the setup, click the <b>Reject</b> button.
    </div>
    <div style="width: 500px ; height: 500px ; overflow: auto ; border-width: 2px ; border-color:ActiveBorder ; border-style: solid;">
        <span id="sEULA" class="text" runat="server"></span>        
    </div>
    <br />
    <asp:Button ID="btnAccept" runat="server" Text="Accept" OnClick="btnAccept_Click" />
    &nbsp; 
    <asp:Button ID="btnReject" runat="server" Text="Reject" OnClick="btnReject_Click" />
    <br />
    <br />            
</asp:Content>
