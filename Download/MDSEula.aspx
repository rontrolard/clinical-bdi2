<%@ Page Title="Mobile Data Solution Eula" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="MDSEula.aspx.cs" Inherits="BDI2Web.Download.MDSEula" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MCP" runat="server">
    <asp:Label ID="lblTitle" runat="server" Text="Mobile Data Solution - License Agreement"></asp:Label>
    <div class="text" style="width: 700px">
        Read the following Mobile Data Solution License Agreement. Scroll down to view the entire agreement, and click the <b>I Accept</b> button to proceed. To close the program without completing the setup, click the <b>I DO NOT Accept</b> button.
    </div>
    <table>
        <tr>
            <td align="center">
                <div style="width: 500px ; height: 500px ; overflow: auto ; border-width: 2px ; border-color:ActiveBorder ; border-style: solid;">
                    <span id="sEULA" class="text" runat="server"></span>        
                </div>
                <br />
                <asp:Button ID="btnAccept" runat="server" Text="I Accept" OnClick="btnAccept_Click" />
                &nbsp; 
                <asp:Button ID="btnReject" runat="server" Text="I DO NOT Accept" OnClick="btnReject_Click" />
                <br />
            </td>
        </tr>
    </table>

</asp:Content>
