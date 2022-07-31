<%@ Page Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" Title="ERF Management" CodeBehind="ERFManagement.aspx.cs" Inherits="BDI2Web.ERFs.ERFManagement" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="radT" %>
<%@ Register Src="~/ERFs/ERFManagementRootLevel.ascx" TagName="ERFManagementRootLevel" TagPrefix="custom" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MCP" runat="server">
    
    <script type="text/javascript">
        function KeyPress(sender, args) {
            var c = args.get_keyCharacter();
            var nf = sender.get_numberFormat();
            if (c == nf.NegativeSign || c == nf.DecimalSeparator) {
                args.set_cancel(true);
            }
        }

        var rm = Sys.WebForms.PageRequestManager.getInstance();
        rm.add_initializeRequest(initializeRequestPage);
        rm.add_endRequest(endRequestPage);

        var currentLoadingPanel = null;

        function initializeRequestPage(sender, args) {
            currentLoadingPanel = $find("<%= LoadingPanel1.ClientID %>");
            currentLoadingPanel.show('MainContent');
        }

        function endRequestPage(sender, args) {
            currentLoadingPanel.hide('MainContent');
        }
    </script>

    <radT:RadAjaxLoadingPanel ID="LoadingPanel1" runat="server" Skin="Simple" BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
    
    <div style="width: 95%" >
    <asp:Label ID="Label6"  runat="server" Text="ERF Management" SkinID="Title" /><br />    
        <asp:Label ID="Label2" runat="server" SkinID="RegularText">
            Use this page to manage ERFs in different organizations.
            <div style="height:5px">&nbsp;</div>
            <div class="leftIndent">Click the plus <img alt="Plus" title="Plus" src="../images/add.png" /> sign(s) to expand the hierarchy list and view the levels.</div>
            <div class="leftIndent">Select a name in the hierarchy list to view and allocate ERF's to the level(s) below it.</div>
            <div class="leftIndent">Allocating and removing ERFs can be done only in organizations that the user has access to.</div>
            <div class="leftIndent">To allocate an ERF, enter the number of ERFs to be allocated in the number field of an organization then click the Allocate button. ERFs are allocated in the order: <b>Allocated from Root</b>, <b>Allocated from Higher Level</b> and <b>Purchased</b>. Each column is used up first before moving onto the next column.</div>
            <div class="leftIndent">To remove an ERF, enter the number of ERFs to be removed in the number field of an organization then click the Remove button. Root users will be able to remove from the <b>Allocated from Root</b> column only. Non Root users will be able to remove from the <b>Allocated from Higher Level</b> column only.</div>
            <div class="leftIndent">For more information, please visit the Help Link. </div>
    </asp:Label>
    </div>
    <br />
                    
    <asp:MultiView ID="MultiView1" runat="server" >
        <asp:View runat="server">
            <asp:Label ID="AccessDeniedLabel" Text="You do not have the correct privilege to manage ERFs." CssClass="errorMessage" runat="server"/>
        </asp:View>
        <asp:View ID="RootView" runat="server">
            <custom:ERFManagementRootLevel ID="ERFManagementRootLevel" runat="server" />
        </asp:View>
        <asp:View ID="View1" runat="server">
            <radT:RadAjaxPanel ID="UpdatePanel1" runat="Server" >
                <table style="width: 95%">
                    <tr>
                        <td colspan="2" style="height: 5px;"></td>
                    </tr>    
                    <tr>
                        <td colspan="2">
                            <asp:Label runat="server" Text="Select Organization" SkinID="Title"/>:
                            <asp:DropDownList ID="OrganizationDropDownList" DataTextField="description" 
                                OnSelectedIndexChanged="OrganizationDropDownList_SelectedIndexChanged" AutoPostBack="true"
                                DataValueField="organizationID" runat="server" AppendDataBoundItems="true">
                                <asp:ListItem Text="-- Select Organization --" Value="-1"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" style="height: 20px;"></td>
                    </tr>    
                    <tr id="OrgTr" runat="server">
                        <td colspan="2">
                             <asp:Label ID="Label1"  runat="server" Text="Organization's Available ERFs" SkinID="Title"/><br /> 
                             <table border=1 width="500px">
                                <th class="defaultLabelStyle">Purchased</th>
                                <th class="defaultLabelStyle">Allocated from Root</th>
                                <th class="defaultLabelStyle">Allocated from Higher Level</th>
                                <tr>
                                    <td align="center"><asp:Label ID="ErfsOwned" runat="server"></asp:Label></td>
                                    <td align="center"><asp:Label ID="ErfsAssignedFromRoot" runat="server"></asp:Label></td>
                                    <td align="center"><asp:Label ID="ErfsAssignedFromHigherLevel" runat="server"></asp:Label></td>
                                </tr>
                            </table>
                        </td>
                    </tr>    
                    <tr>
                        <td colspan="2" style="height: 20px;"></td>
                    </tr>    
                    <tr>
                        <td id="trErrorMessage" colspan="2" runat="server">
                            <span class="errorMessage" >There are not enough ERFs available to allocate the requested amount.</span>            
                        </td>
                    </tr>
                    <tr>
                        <td id="trErrorMessageRequired" colspan="2" runat="server">
                            <span class="errorMessage">Please specify a quantity of ERFs to allocate or remove.</span>            
                        </td>
                    </tr>
                    <tr id="GridTr" runat="server">     
                        <td colspan="2">
                            <asp:Label ID="Label3"  runat="server" Text="Lower Level Organization's ERFs" SkinID="Title"/><br /> 
                            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="false" Width="100%" DataKeyNames="OrganizationID">
                                <Columns>
                                    <asp:BoundField DataField="description" ItemStyle-CssClass="wrapster" HeaderText="Lower Level Organization" ItemStyle-HorizontalAlign="Left" ItemStyle-BorderStyle="solid" ItemStyle-BorderColor="#CC9966" ItemStyle-BorderWidth="1px"/>
                                    <asp:BoundField DataField="Owned" HeaderText="Purchased" ItemStyle-Width="80px" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderStyle="solid" ItemStyle-BorderColor="#CC9966" ItemStyle-BorderWidth="1px"/>
                                    <asp:BoundField DataField="AssignedFromRoot" HeaderText="Allocated from Root" ItemStyle-Width="80px" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderStyle="solid" ItemStyle-BorderColor="#CC9966" ItemStyle-BorderWidth="1px"/>
                                    <asp:BoundField DataField="AssignedFromHigherLevel" HeaderText="Allocated from Higher Level" ItemStyle-Width="80px" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderStyle="solid" ItemStyle-BorderColor="#CC9966" ItemStyle-BorderWidth="1px"/>
                                    <asp:TemplateField HeaderText="" HeaderStyle-Width="200px" ItemStyle-Width="200px" ItemStyle-BorderStyle="solid" ItemStyle-BorderColor="#CC9966" ItemStyle-BorderWidth="1px">
                                        <ItemTemplate>
                                            <radT:RadNumericTextBox Width="30px" Id="DeltaCount" Type="Number" MinValue="0" runat="server"  AllowOutOfRangeAutoCorrect="false" >
                                                <NumberFormat DecimalDigits="0" GroupSeparator="" DecimalSeparator="," />
                                                <ClientEvents OnKeyPress="KeyPress" />
                                                <IncrementSettings InterceptMouseWheel="false" />
                                            </radT:RadNumericTextBox>
                                            <asp:Button runat="server" Text="Allocate" OnClick="Assign_Clicked"  CommandArgument='<% #Eval("OrganizationID") %>' />
                                            <asp:Button runat="server" Text="Remove" OnClick="Remove_Clicked" CommandArgument='<% #Eval("OrganizationID") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <EmptyDataTemplate>
                                    No Lower Level Organizations exist.
                                </EmptyDataTemplate>
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
            </radT:RadAjaxPanel>
        </asp:View>
    </asp:MultiView>
</asp:Content>
