<%@ Page Language="C#" MasterPageFile="~/Main.Master" EnableEventValidation="false" Buffer="false" AutoEventWireup="true" CodeBehind="Export.aspx.cs" Inherits="BDI2Web.ExIm.Export" Title="Export" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="radT" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MCP" runat="server">
<radT:RadCodeBlock ID="rcb1" runat="server">
<script type="text/javascript">
    function mngRequestStarted(ajaxManager, eventArgs) 
    { 
        if (eventArgs.get_eventTarget().indexOf("butExport") != -1)
            eventArgs.set_enableAjax(false); 
        else
            eventArgs.set_enableAjax(true); 
    } 
 </script>
</radT:RadCodeBlock>    
    <asp:Label ID="Label2" runat="server" Text="Export" SkinID="Title"/><br />
    <asp:Label ID="Label1" runat="server" Text="Fields with * are required fields" SkinID="Info"/>
    <asp:Panel ID="TestExportPanel" Visible="false" runat="server">
        <asp:Button ID="RunScheduleJobButton" runat="server" Text="Run Export" OnClick="RunScheduleJobButton_Click" />
        <br />
        <asp:TextBox ID="ScheduleJobIDTextBox" runat="server" />
    </asp:Panel>
    <table>
        <tr>
            <td align="center"  class="defaultLabelStyle">Important Information!</td>
        </tr>
        <tr>
            <td class="text">                
                The Migration Export, the Student and Assessment Summary, the Assessment Domains export, and the Assessment Details export are scheduled exports. Upon request, these exports will be added to the queue and will be processed in the order they are received. This may take up to 24 hours and will be available in the Scheduled Queue, located under the <b>Import/Export</b> tab.
            </td>
        </tr>
        <tr>
            <td class="text">
                All other exports are generated on demand and can take up to 20 minutes. Do not close your Internet browser or rerequest these exports while they are generating as it will cause additional delay.
            </td>
        </tr>
    </table>  
    <table border="0" style="width: 760px;">
        <tr>
            <td style="width: 400px;">
                <span class="defaultLabelStyle">Select File to export:<asp:Label ID="Label4" runat="server" Text="*" SkinID="Info"/></span> 
                <br />
                <asp:RadioButtonList ID="rblFile" runat="server" AutoPostBack="true" OnSelectedIndexChanged="rblFile_SelectedIndexChanged">
                </asp:RadioButtonList>
                <br />
            </td>
            <td style="width: 60px;">
                 <radT:RadAjaxLoadingPanel runat="Server" ID="LoadingPanel1">
                    <asp:Image ID="Image2" ImageUrl="~/Images/loading3.gif" AlternateText="Loading" BorderWidth="0px"
                        runat="server"></asp:Image>
                </radT:RadAjaxLoadingPanel>
            </td>
            <td align="center" style="width: 300px;">       
                <asp:HyperLink ID="FileFormatHyperLink" NavigateUrl="~/ExIm/ExportFormat.xls" Text="Export File Format" runat="server" />                
                <br /> 
                <br /> 
                <span class="defaultLabelStyle">Select File Delimiter</span>
                <asp:RadioButtonList ID="rblFormat" runat="server" ToolTip="select output file delimiter" Width="130px">
                    <asp:ListItem Value="\t" Text="Tab&amp;nbsp;&amp;nbsp;&amp;nbsp;&amp;nbsp;&amp;nbsp;" Selected="True" />
                    <asp:ListItem Value="," Text="Comma " />
                    <asp:ListItem Value="Xml">Xml&amp;nbsp;&amp;nbsp;&amp;nbsp;&amp;nbsp;</asp:ListItem>
                    <asp:ListItem Value="BDI3" Text="BDI-3" />
                </asp:RadioButtonList>
                <br />
                <asp:Panel ID="EmailPanel" Visible="false" runat="server">
                    <span class="defaultLabelStyle">E-mail:<asp:Label ID="Label3" runat="server" Text="*" SkinID="Info"/> </span><asp:TextBox ID="EmailTextBox" Width="150px" runat="server" />
                    <br /><br />
                </asp:Panel>
                <asp:Button ID="butExport" runat="server" Enabled="False" Text="Export File" Width="135px" OnClick="butExport_Click" />&nbsp;            
                <br />
                <br />
            </td>
        </tr>
    </table>   
    <table style="width: 760px;" border="0">        
        <tr>
            <td style="width:170px;">
                <asp:Label ID="Label5" runat="server" Text="Selected Organization:"></asp:Label><asp:Label ID="Label6" runat="server" Text="*" SkinID="Info"/>&nbsp;           
            </td>
            <td style="width:430px">
                <asp:Label ID="LabelOrgSelected" runat="server" Text=""></asp:Label>
            </td>        
            <td style="width:160px;">
                <asp:Label ID="LabelOrganizationID" runat="server" Visible="False"></asp:Label>
                <asp:Label ID="LabelHierarchyLevelID" runat="server" Visible="False"></asp:Label>
            </td>
        </tr>
     </table>
    <table border="0">
        <tr>
            <td rowspan="2" style="vertical-align:top;">
                <asp:Panel ID="OrgTreePanel" runat="server" Visible="false">                   
                    <table>
                      <tr>
                        <td class="treeBorder" style="width: 380px; height: 135px;vertical-align:top;">
                          <div>
                            <radT:RadTreeView ID="t1" runat="server" OnNodeClick="rtvOrganization_NodeClick" PostBackUrl="Export.aspx"
                                DataFieldID="organizationID" DataFieldParentID="parentOrganizationID"
                                DataTextField="description" DataValueField="organizationID">
                            </radT:RadTreeView>
                          </div>
                        </td>
                      </tr>
                    </table>
                 </asp:Panel>
                </td>
                <td style="vertical-align:top;">
                <asp:Panel ID="DatesPanel" runat="server" Visible="false">
                    <table>
                        <tr>
                            <td style="width:200px;vertical-align:top;">
                                <div>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblFrom" runat="server" Text="Export Beginning Date" ></asp:Label><asp:Label ID="Label7" runat="server" Text="*" SkinID="Info"/>
                                                <radT:raddatepicker id="FromDate"  Calendar-ShowRowHeaders="false"
                                                style="DISPLAY: block" Runat="server">
                                            </radT:raddatepicker>
                                            </td>
                                       </tr>
                                       <tr> 
                                            <td>
                                                <asp:Label ID="lblTo" runat="server" Text="Export Ending Date" ></asp:Label><asp:Label ID="Label8" runat="server" Text="*" SkinID="Info"/>
                                                <radT:raddatepicker id="ToDate"  Calendar-ShowRowHeaders="false" 
                                                style="DISPLAY: block" Runat="server">
                                            </radT:raddatepicker>
                                            </td>
                                       </tr>
                                    </table>                           
                                    <asp:Panel ID="ProgramNotesPanel" runat="server" Visible="false"  Width="300px" Height="125px"  >
                                        <table  style="width:300px;height:100px;">
                                           <tr>
                                                <td  class="defaultLabelStyle" >
                                                Select Program Note Criteria:
                                                </td>
                                           </tr>
                                           <tr>
                                                <td>
                                                    <asp:Panel ID="ProgramNotesPanel2" runat="server" BorderStyle="Solid" BorderWidth="1" Width="300px" Height="100px" BorderColor="#105fad"  >
                                                        <table style="width:300px;">
                                                            <tr> 
                                                                <td style="width:5px;">&nbsp;</td>
                                                                <td style="width:295px;">
                                                                    <asp:Label ID="lblProgramNote1" runat="server" Text="Criteria 1:" />&nbsp;
                                                                    <asp:DropDownList ID="ddlProgramNote1" AutoPostBack="true" OnSelectedIndexChanged="ddlProgramNote1_SelectedIndexChanged" runat="server" />&nbsp; 
                                                                    <asp:DropDownList ID="ddlProgramNoteJoin" Width="50px"  AutoPostBack="true" runat="server" OnSelectedIndexChanged="ddlProgramNoteJoin_SelectedIndexChanged"/>
                                                                </td>
                                                           </tr>           
                                                           <tr> 
                                                                <td style="width:5px;">&nbsp;</td>
                                                                <td  style="width:295px;">
                                                                    <asp:Label ID="lblProgramNote2" runat="server" Text="Criteria 2:" />&nbsp;
                                                                    <asp:DropDownList ID="ddlProgramNote2" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlProgramNote2_SelectedIndexChanged"/>
                                                                </td>
                                                           </tr>           
                                                        </table>
                                                    </asp:Panel>
                                                </td>
                                           </tr>                                                
                                        </table>   
                                    </asp:Panel>
                                </div>
                            </td>                            
                        </tr>
                    </table>
                </asp:Panel>
            </td>
        </tr>
    </table>
    <radT:RadAjaxManager ID="RAM1" runat="server" ClientEvents-OnRequestStart="mngRequestStarted">
        <AjaxSettings>
            <radT:AjaxSetting AjaxControlID="rblFile">
                <UpdatedControls>
                    <%--<radT:AjaxUpdatedControl ControlID="lblInstruction" />--%>
                    <%--<radT:AjaxUpdatedControl ControlID="OrgTreePanel" LoadingPanelID="LoadingPanel1" />--%>
                    <radT:AjaxUpdatedControl ControlID="DatesPanel" LoadingPanelID="LoadingPanel1" />
                    <radT:AjaxUpdatedControl ControlID="EmailPanel" />
                    <radT:AjaxUpdatedControl ControlID="butExport" />
                    <radT:AjaxUpdatedControl ControlID="rblFormat" />
                    <radT:AjaxUpdatedControl ControlID="rblFile" />
                    <radT:AjaxUpdatedControl ControlID="ProgramNotesPanel" />
                </UpdatedControls>
            </radT:AjaxSetting>
            
            <radT:AjaxSetting AjaxControlID="t1">
                <UpdatedControls>
                    <radT:AjaxUpdatedControl ControlID="LabelOrgSelected" LoadingPanelID="LoadingPanel1" />
                    <radT:AjaxUpdatedControl ControlID="butExport" />
                </UpdatedControls>
            </radT:AjaxSetting>                           
            <radT:AjaxSetting AjaxControlID="ddlProgramNoteJoin">
                <UpdatedControls>
                    <radT:AjaxUpdatedControl ControlID="ProgramNotesPanel2" />
                </UpdatedControls>
            </radT:AjaxSetting>   
            <radT:AjaxSetting AjaxControlID="ddlProgramNote1">
                <UpdatedControls>
                    <radT:AjaxUpdatedControl ControlID="ProgramNotesPanel2" />
                </UpdatedControls>
            </radT:AjaxSetting>
            <radT:AjaxSetting AjaxControlID="ddlProgramNote2">
                <UpdatedControls>
                    <radT:AjaxUpdatedControl ControlID="ProgramNotesPanel2" />
                </UpdatedControls>
            </radT:AjaxSetting>
            <radT:AjaxSetting AjaxControlID="butExport">
                <UpdatedControls>
                    <radT:AjaxUpdatedControl ControlID="MPErrorPanel" />
                </UpdatedControls>
            </radT:AjaxSetting>   
        </AjaxSettings>
        <ClientEvents OnRequestStart="mngRequestStarted" />
    </radT:RadAjaxManager></asp:Content>
