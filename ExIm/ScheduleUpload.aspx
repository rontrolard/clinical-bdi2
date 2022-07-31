<%@ Page Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="ScheduleUpload.aspx.cs" Inherits="BDI2Web.ExIm.ScheduleUpload" Title="Schedule Upload" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="radT" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MCP" runat="server">
<radT:RadProgressManager ID="Radprogressmanager1" runat="server" />
<asp:Panel runat="server" ID="upldPanel" DefaultButton="buttonSubmit">
<table width="730px" border="0">
    <tr>
        <td colspan="2"><asp:Label ID="Label1" runat="server" Text="Schedule Upload" SkinID="Title"/></td>
    </tr>   
    <tr>
        <td colspan="2"><asp:Label ID="Label2" runat="server" Text="Fields with * are required fields" SkinID="Info"/></td>
    </tr>
    <tr>
        <td colspan="2">
            <div class="text" style="width: 700px">
                <div class="leftIndent">To initiate the import of a data file from a Data Manager Web account, click the appropriate browse button to identify a zipped (.zip) file for upload. The file will be added to the queue and will be processed in the order it was received.  This may take up to 24 hours. Once uploaded, the file needs to be rostered before import can be completed. Check the status of upload requests in the Scheduled Queue, located under the <b>Import/Export</b> tab.</div>           
                <%--<div style="height:5px">&nbsp;</div>
                <div class="leftIndent">Upload will complete in approximately 24 hours. Once uploaded, the file needs to be rostered before import can be completed. Check the status of upload requests in the Scheduled Queue, located under the Import/Export tab.</div>        --%>
            </div>            
        </td>
    </tr>
    <tr>
        <td colspan="2">&nbsp;</td>
    </tr>
    <tr>
        <td class="defaultLabelStyle" style="width: 100px">File Location:<asp:Label ID="Label3" runat="server" Text="*" SkinID="Info" Width="5px"/></td>
       <%--<td><radT:RadUpload runat="Server" ID="txtScheduleUpload" AllowedFileExtensions=".zip" ControlObjectsVisibility="none" InputSize="50" MaxFileSize="100000000"></radT:RadUpload></td>--%>
       <td><asp:FileUpload runat="server" ID="txtScheduleUpload" style="width:460px;" size="60"/></td>
    </tr>
    <%--<tr style="height:5px;">
        <td colspan="2">&nbsp;</td>
    </tr>  --%>
    <tr>
        <td class="defaultLabelStyle" style="width: 100px">E-mail:<asp:Label ID="Label4" runat="server" Text="*" SkinID="Info"/></td>      
       <td><asp:TextBox ID="txtEmailId" Width="175px" runat="server" /></td>
    </tr>
    <tr>
        <td colspan="2">&nbsp;</td>
    </tr>  
          
    <tr>
        <td style="width: 100px">&nbsp;</td>
        <td>
            <asp:Button ID="buttonSubmit" runat="server" Text="Upload File" OnClick="buttonSubmit_Click" />
        </td>
    </tr>
    <tr>
        <td colspan="2"> <radT:RadProgressArea id="progressArea1" runat="server" DisplayCancelButton="True" >
            <Localization Uploaded="Uploaded" />
        </radT:RadProgressArea></td>
    </tr> 
     
        
</table>

</asp:Panel>
</asp:Content>
