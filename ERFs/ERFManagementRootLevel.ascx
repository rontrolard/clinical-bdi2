<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ERFManagementRootLevel.ascx.cs" Inherits="BDI2Web.ERFs.ERFManagementRootLevel" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="radT" %>


<radT:RadAjaxManager runat="server" ID="RadAjaxManager1">
    <AjaxSettings>
        <radT:AjaxSetting AjaxControlID="SelectOrganization" EventName="OnSelectedIndexChanged">
            <UpdatedControls>
                <radT:AjaxUpdatedControl ControlID="Panel1" />
                <radT:AjaxUpdatedControl ControlID="T" />
            </UpdatedControls>
        </radT:AjaxSetting>
        <radT:AjaxSetting AjaxControlID="Panel1" >
            <UpdatedControls>
                <radT:AjaxUpdatedControl ControlID="Panel1" />
                <radT:AjaxUpdatedControl ControlID="spErrorMessage" />
                <radT:AjaxUpdatedControl ControlID="spErrorMessageRequired" />
                <radT:AjaxUpdatedControl ControlID="ErfsAvailable" />
                <radT:AjaxUpdatedControl ControlID="ErfsOwned" />
            </UpdatedControls>
        </radT:AjaxSetting>
        <radT:AjaxSetting AjaxControlID="T" >
            <UpdatedControls>
                <radT:AjaxUpdatedControl ControlID="T" />
                <radT:AjaxUpdatedControl ControlID="Panel1"/>
                <radT:AjaxUpdatedControl ControlID="SelectOrganization" />
            </UpdatedControls>
        </radT:AjaxSetting>
    </AjaxSettings>
</radT:RadAjaxManager>

<radT:RadScriptBlock ID="RadScriptBlock1" runat='server'>
    <script type="text/javascript">
        function ScrollToSelectedNode()
        {
            var tree = $find("<%= T.ClientID %>");
            var selectedNode = tree.get_selectedNode();
            if (selectedNode != null)
            {
                selectedNode.scrollIntoView();
            }
        }
    </script>
</radT:RadScriptBlock>

<table width="95%">
    <tr>
        <td>
             <asp:Label ID="Label1"  runat="server" Text="Root Level's Available ERFs" SkinID="Title"/><br /> 
             <table border="1" width="500px">
                <th class="defaultLabelStyle">Purchased</th>
                <th class="defaultLabelStyle">Available</th>
                <tr>
                    <td align="center"><asp:Label ID="ErfsOwned" runat="server"></asp:Label></td>
                    <td align="center"><asp:Label ID="ErfsAvailable" runat="server"></asp:Label></td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td> 
            <radT:RadComboBox ID="SelectOrganization" runat="server" Height="180px" Width="220px"
                OnItemsRequested="SelectOrganization_ItemsRequested" Label="Search Organization :" LabelCssClass="defaultLabelStyle"
                EnableLoadOnDemand="true" ShowMoreResultsBox="true" EnableVirtualScrolling="true"
                ItemsPerRequest="10" OnSelectedIndexChanged="SelectOrganization_SelectedIndexChanged" 
                AutoPostBack="true" >
            </radT:RadComboBox>           
                  
            <radT:RadTreeView ID="T" runat="server" Style="white-space: normal; margin-top: 10px;" CssClass="treeBorder" Height="130px" DataTextField="description" 
                DataValueField="organizationID" OnNodeClick="T_NodeClick" OnNodeExpand="T_NodeExpand" ></radT:RadTreeView>
        </td>
    </tr>
    <tr>
        <td colspan="2" >
            <span id="spErrorMessage" runat="server" class="errorMessage">There are not enough ERFs available to allocate the requested amount.</span>            
        </td>
    </tr>
    <tr>
        <td colspan="2">
            <span id="spErrorMessageRequired" runat="server" class="errorMessage">Please specify a quantity of ERFs to allocate or remove.</span>            
        </td>
    </tr>
    <tr id="GridTr" runat="server">
        <td colspan="2">
            <asp:Panel ID="Panel1" runat="server">
                <div id="Div1" runat="server">
                    <asp:Label ID="Label3"  runat="server" Text="Lower Level Organization's ERFs" SkinID="Title"/><br /> 
                    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="false" Width="100%" DataKeyNames="OrganizationID">
                        <Columns>
                            <asp:BoundField DataField="description" ItemStyle-CssClass="wrapster" HeaderText="Lower Level Organization" ItemStyle-HorizontalAlign="Left" ItemStyle-BorderStyle="solid" ItemStyle-BorderColor="#CC9966" ItemStyle-BorderWidth="1px"/>
                            <asp:BoundField DataField="Owned" HeaderText="Purchased" ItemStyle-Width="80px" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderStyle="solid" ItemStyle-BorderColor="#CC9966" ItemStyle-BorderWidth="1px"/>
                            <asp:BoundField DataField="AssignedFromRoot" HeaderText="Allocated from Root" ItemStyle-Width="80px" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderStyle="solid" ItemStyle-BorderColor="#CC9966" ItemStyle-BorderWidth="1px"/>
                            <asp:BoundField DataField="AssignedFromHigherLevel" HeaderText="Allocated from Higher Level" ItemStyle-Width="80px" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderStyle="solid" ItemStyle-BorderColor="#CC9966" ItemStyle-BorderWidth="1px"/>
                            <asp:TemplateField HeaderText="" HeaderStyle-Width="200px" ItemStyle-Width="200px" ItemStyle-BorderStyle="solid" ItemStyle-BorderColor="#CC9966" ItemStyle-BorderWidth="1px">
                                <ItemTemplate>
                                    <radT:RadNumericTextBox Width="30px" Id="DeltaCount" Type="Number" MinValue="0" runat="server" AllowOutOfRangeAutoCorrect="false" >
                                        <NumberFormat DecimalDigits="0" GroupSeparator="" DecimalSeparator="," />
                                        <ClientEvents OnKeyPress="KeyPress" />
                                        <IncrementSettings InterceptMouseWheel="false" />
                                    </radT:RadNumericTextBox>
                                    <asp:Button ID="Button1" runat="server" Text="Allocate" OnClick="Assign_Clicked"  CommandArgument='<% #Eval("OrganizationID") %>' />
                                    <asp:Button ID="Button2" runat="server" Text="Remove" OnClick="Remove_Clicked" CommandArgument='<% #Eval("OrganizationID") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <EmptyDataTemplate>
                            No Lower Level Organizations exist.
                        </EmptyDataTemplate>
                    </asp:GridView>
                </div>
            </asp:Panel>
        </td>
    </tr>
</table>