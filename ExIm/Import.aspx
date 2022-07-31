<%@ Page Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeBehind="Import.aspx.cs" Inherits="BDI2Web.ExIm.Import" Title="Import" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MCP" Runat="Server">
    <asp:Label ID="Label2" runat="server" Text="Import" SkinID="Title"/>    
    <div class="text" style="width: 700px">        
     <!--   Use this page to view group rosters and verify rerostering changes you make to your organization. You also can use it to locate and edit child records.
        <div style="height:5px">&nbsp;</div>
        <div class="leftIndent">To view the contents of a group, click the lowest level(s) of the expanded hierarchy list. Scroll down the hierarchy list to see additional groups.</div>
        <div><b>Tip:</b> You can adjust the view on the View Roster page by clicking the <b>^</b> on the blue split bar or by clicking and dragging the bar to expand or contract the size of the panes.</div>        
        <div class="leftIndent">Use the <b>Edit</b>, <b>Delete</b>, <b>Assessment</b>, <b>Report</b>, and <b>Notes</b> icons to make changes to the records listed in the bottom pane.</div>
     -->
     </div>
    <table>
        <tr>
            <td colspan="2">
                <span class="defaultLabelStyle">Click following link to see import file format:&nbsp;</span>
                <asp:HyperLink ID="FileFormatHyperLink" NavigateUrl="~/ExIm/ImportFormat.xls" Text="Import File Format" runat="server" />
                <br />
            </td>
        </tr>
        <tr>
            <td class="defaultLabelStyle" style="padding-left:10px; text-align:right;">Organization File :</td>
            <td style="width: 570px" >
                <asp:FileUpload ID="fuOrg" runat="server" Width="570px" />
            </td>
        </tr>
        <tr>
            <td class="defaultLabelStyle" style="padding-left:10px; text-align:right;">Staff File :</td>
            <td style="width: 570px" >
                <asp:FileUpload ID="fuStaff" runat="server" Width="570px" />
            </td>
        </tr>
        <tr>
            <td class="defaultLabelStyle" style="padding-left:10px; text-align:right;">Student File :</td>
            <td style="width: 570px">
                <asp:FileUpload ID="fuStudent" runat="server" Width="570px" />
            </td>
        </tr>
    </table>
    <br />
    <asp:TextBox ID="tbBatchID" runat="server" Visible="false" EnableViewState="false" Width="453px" />
    <br />
    <asp:Button ID="butImport" runat="server" Text="Import Data" Width="120px" OnClick="butImport_Click" />
    <br />        
    <asp:TextBox ID="tbMessage" runat="server" Width="700px" Visible="False" style="text-align:center;"></asp:TextBox> 
    <asp:BulletedList Width="660px" ID="blErrorList" runat="server" DataValueField="DisplayValue" DataSourceID="dsErrors"/>


    <asp:SqlDataSource ID="dsErrors" runat="server" ConnectionString="<%$ ConnectionStrings:Connection String %>"
        SelectCommand="SELECT '[' + TableName + ' - Row ' + CAST(RowId AS VarChar(10)) + '] - ' + messageTxt AS DisplayValue 
                       FROM exim.UploadLog WHERE (BatchId = @batchID) ORDER BY TableName, RowId, LoadTS"> 
        <SelectParameters>
             <asp:ControlParameter ControlID="tbBatchID" DefaultValue="0" Name="batchID" PropertyName="Text" />
        </SelectParameters>
    </asp:SqlDataSource>
</asp:Content>
