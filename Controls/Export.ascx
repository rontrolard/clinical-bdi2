<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Export.ascx.cs" Inherits="Controls_Export" %>
<%@ Register TagPrefix="rada" Namespace="Telerik.WebControls" Assembly="RadAjax.NET2" %>
<%@ Register Assembly="RadTreeView.Net2" Namespace="Telerik.WebControls" TagPrefix="radT" %>
<table>
    <tr>
        <td style="width: 50px;">
        </td>
        <td>
            Select File to export 
            <br />
            <asp:RadioButtonList ID="rblFile" runat="server" AutoPostBack="true" OnSelectedIndexChanged="rblFile_SelectedIndexChanged">
                <asp:ListItem>Organizations</asp:ListItem>
                <asp:ListItem>Staff</asp:ListItem>
                <asp:ListItem>Students</asp:ListItem>
                <asp:ListItem Value="AssessmentSummary">Assessment Summary</asp:ListItem>
                <asp:ListItem Value="AssessmentDomain">Assessment Domains</asp:ListItem>
                <asp:ListItem Value="AssessmentDetail">Assessment Details</asp:ListItem>
                <asp:ListItem Value="AssessmentObservation">Assessment Observations</asp:ListItem>
            </asp:RadioButtonList>
            <br />
        </td>
        <td style="width: 50px;">
        </td>
        <td align="center">
            <asp:Label ID="lblInstruction" runat="server" Visible="False"></asp:Label>
            <br /> 
            <br /> 
            Select File Delimeter
            <asp:RadioButtonList ID="rblFormat" runat="server" ToolTip="select output file delimiter">
                <asp:ListItem Value="\t" Text="Tab&amp;nbsp;&amp;nbsp;&amp;nbsp;&amp;nbsp;&amp;nbsp;" Selected="True" />
                <asp:ListItem Value="," Text="Comma " />
            </asp:RadioButtonList>
            <br />
            <asp:Button ID="butExport" runat="server" Enabled="False" Text="Export File" Width="135px" OnClick="butExport_Click" />&nbsp;<br />
            <br />
        </td>
    </tr>
    <tr>
        <td class="treeBorder" style="width: 380px; height: 135px;" valign="top" colspan="3">
            <radT:RadTreeView ID="rtvOrganization" runat="server" OnNodeClick="rtvOrganization_NodeClick"
                AutoPostBack="True" DataFieldID="organizationID" DataFieldParentID="parentOrganizationID"
                DataTextField="description" DataValueField="organizationID" Enabled="False" Visible="False">
            </radT:RadTreeView>
        </td>
        <td>
            <asp:Label ID="lblFrom" runat="server" Text="Export Beginning Date" Visible="False"></asp:Label>
            <asp:Calendar ID="FromDate" runat="server" Enabled="False" Visible="False" OnSelectionChanged="FromDate_SelectionChanged"></asp:Calendar>
            <asp:Label ID="lblTo" runat="server" Text="Export Ending Date" Visible="False"></asp:Label>
            <asp:Calendar ID="ToDate" runat="server" Enabled="False" Visible="False" OnSelectionChanged="ToDate_SelectionChanged"></asp:Calendar>
        </td>
    </tr>
    <tr>
        <td colspan="3" >
            <asp:Label ID="Label5" runat="server" Text="Selected Organization:"></asp:Label>&nbsp;
            <asp:Label ID="LabelOrgSelected" runat="server" Text=""></asp:Label>
        </td>
        <td>
            <asp:Label ID="LabelOrganizationID" runat="server" Visible="False"></asp:Label>
            <asp:Label ID="LabelHierarchyLevelID" runat="server" Visible="False"></asp:Label>
        </td>
    </tr>
</table>
   
<rada:RadAjaxManager ID="RadAjaxManager1"  runat="server">
    <AjaxSettings>
        <rada:AjaxSetting AjaxControlID="RadTreeView1">
            <UpdatedControls>                    
                <rada:AjaxUpdatedControl ControlID="ButtonPanel" />
                <rada:AjaxUpdatedControl ControlID="MPErrorPanel" />
            </UpdatedControls>
        </rada:AjaxSetting>
    </AjaxSettings>
</rada:RadAjaxManager>
