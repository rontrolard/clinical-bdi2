<%@ Control Language="C#" AutoEventWireup="true" CodeFile="InfrastructureImport.ascx.cs" Inherits="Controls_InfrastructureImport" %>
&nbsp;<div class="<%=HeaderCssClass %>">
    Enter
    File Locations</div>

<table>
    <!-- No longeg loading hierarchy   tr>
        <td class="title" style="padding-left:10px; text-align:right;">Hierarchy File :</td>
        <td style="width: 570px" >
            <asp:FileUpload ID="fuHier" runat="server" />
        </td>
    </tr-->
    <tr>
        <td class="title" style="padding-left:10px; text-align:right;">Organization File :</td>
        <td style="width: 570px" >
            <asp:FileUpload ID="fuOrg" runat="server" Width="570px" />
        </td>
    </tr>
    <tr>
        <td class="title" style="padding-left:10px; text-align:right;">Staff File :</td>
        <td style="width: 570px" >
            <asp:FileUpload ID="fuStaff" runat="server" Width="570px" />
        </td>
    </tr>
    <tr>
        <td class="title" style="padding-left:10px; text-align:right;">Student File :</td>
        <td style="width: 570px">
            <asp:FileUpload ID="fuStudent" runat="server" Width="570px" />
        </td>
    </tr>
</table>
<br />
<asp:TextBox ID="tbBatchID" runat="server" Visible="false" EnableViewState="false" Width="453px" />
<asp:Button ID="butCancel" runat="server" Text="Cancel" Width="120px" OnClick="butCancel_Click" />
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
