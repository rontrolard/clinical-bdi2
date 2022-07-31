<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AppliedFilters.aspx.cs" Inherits="BDI2Web.ExIm.AppliedFilters" Title="Scheduled Job Filters" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Scheduled Job Filters</title>
    <style type="text/css">
		@import url( ../StyleSheets/BDI2.css );		
	</style>
</head>
<body>
    <script type="text/javascript">  
        function GetRadWindow()
        {
            var oWindow = null;
            if (window.radWindow) oWindow = window.radWindow;
            else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
            return oWindow;
        } 
    
        function CloseWindow()
        {
            var oWin = GetRadWindow();
            oWin.Close();
        }
    </script>
    <form id="form1" runat="server">
        <table border="0" width="420px">
            <tr>
                <td class="defaultLabelStyle" align="right" style="width:170px;">Job ID: </td>
                <td style="width:230px;"><asp:Label ID="JobIDLabel" SkinID="RegularText" runat="server" /></td>
            </tr>
            <tr>
                <td class="defaultLabelStyle" align="right" style="width:170px;">Description: </td>
                <td style="width:230px;"><asp:Label ID="DescriptionLabel" SkinID="RegularText" runat="server" /></td>
            </tr>
            <tr id="organizationRow" runat="server">
                <td class="defaultLabelStyle" align="right" style="width:170px;">Organization Name: </td>
                <td style="width:230px;"><asp:Label ID="OrganizationLabel" SkinID="regularText" runat="server" /></td>
            </tr>
            <tr id="FromDateRow" runat="server">
                <td class="defaultLabelStyle" align="right" style="width:170px;"><%=fromDateMsg%> </td>
                <td style="width:230px;"><asp:Label ID="FromDateLabel" SkinID="regularText" runat="server" /></td>
            </tr>
            <tr id="ToDateRow" runat="server">
                <td class="defaultLabelStyle" align="right" style="width:170px;"><%=toDateMsg%> </td>
                <td style="width:230px;"><asp:Label ID="ToDateLabel" SkinID="regularText" runat="server" /></td>
            </tr>                                  
            <tr id="FileOptionRow" runat="server">
                <td class="defaultLabelStyle" align="right" style="width:170px; padding-top:3px;" valign="top"><%=fileOptionsMsg%></td>
                <td valign="top" style="width:230px;"><asp:Label ID="DelimitorLabel" SkinID="regularText" runat="server"/></td>
            </tr>            
        </table>
        <asp:Panel ID="pnlNotesFilter" runat="server" Height="50px" > 
        <table>
            <tr >      
                <td class="defaultLabelStyle" align="right"   style="width:170px;">Program Note Criteria 1: </td>
                <td style="width:230px;"><asp:Label ID="ProgramNote1Label" SkinID="RegularText" runat="server" /></td>
                
            </tr>
            <tr>
                <td class="defaultLabelStyle" align="right" style="width:170px;">And/Or Operator: </td>
                <td style="width:230px;"><asp:Label ID="ProgramNoteOperatorLabel" SkinID="RegularText" runat="server" /></td>
            </tr>
            <tr>
                <td class="defaultLabelStyle" align="right" style="width:170px;">Program Note Criteria 2:</td>
                <td style="width:230px;"><asp:Label ID="ProgramNote2Label" SkinID="regularText" runat="server" /></td>
            </tr>                        
        </table>
        </asp:Panel>
        <asp:Panel ID="RosterInfoPanel" runat="server" Visible="false">
            <table cellspacing="0" border="0" width="420px">
                <tr>
                    <td class="defaultLabelStyle" colspan="2" align="left">Roster Mapping:</td>                    
                </tr>
                <tr>
                    <td valign="top" colspan="2">                        
                        <asp:GridView ID="ResultsGridView" runat="server" 
                            AllowPaging="false" AllowSorting="false" 
                            AutoGenerateColumns="False" Width="420px">                             
                            <Columns>
                                <asp:TemplateField HeaderText="Source" ItemStyle-Width="205px" HeaderStyle-Width="205px" ItemStyle-BorderStyle="solid" ItemStyle-BorderColor="#CC9966" ItemStyle-BorderWidth="1px">
                                    <ItemTemplate >
                                        <asp:Label ID="L1" runat="server" Text='<%# Eval("From")%>' SkinID="text" style="text-align:left;"></asp:Label>
                                        <asp:Label ID="Label1" runat="server" Text='<%# "<br>Students: " + Eval("StudentCount")%>' SkinID="text" style="text-align:left;"></asp:Label>
                                        <asp:Label ID="Label2" runat="server" Text='<%# "<br>Assessments: " + Eval("AssessmentCount")%>' SkinID="text" style="text-align:left;"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>                                
                                <asp:BoundField DataField="To" HeaderText="Destination" ItemStyle-Width="190px" HeaderStyle-Width="190px"  ItemStyle-BorderStyle="solid" ItemStyle-BorderColor="#CC9966" ItemStyle-BorderWidth="1px"/>                                                                              
                            </Columns>
                        </asp:GridView>                                               
                    </td>
                </tr>                           
            </table>
        </asp:Panel> 
        <asp:Panel ID="ChildIdPanel" runat="server" Visible="false">
            <table cellspacing="0" border="0" width="420px">
                <tr>
                    <td class="defaultLabelStyle" align="left" colspan="2">The following child IDs have been changed:</td>                  
                </tr>
                <tr>
                    <td valign="top" colspan="2">                        
                        <asp:GridView ID="ChildIdGridView" runat="server" 
                            AllowPaging="false" AllowSorting="false" 
                            AutoGenerateColumns="False" Width="420">                             
                            <Columns>                                
                                <asp:BoundField DataField="lastName" HeaderText="Last Name" ItemStyle-BorderStyle="solid" ItemStyle-BorderColor="#CC9966" ItemStyle-BorderWidth="1px"/>                                                                              
                                <asp:BoundField DataField="firstName" HeaderText="First Name" ItemStyle-BorderStyle="solid" ItemStyle-BorderColor="#CC9966" ItemStyle-BorderWidth="1px"/>                                                                              
                                <asp:BoundField DataField="oldChildID" HeaderText="Old Child ID" ItemStyle-BorderStyle="solid" ItemStyle-BorderColor="#CC9966" ItemStyle-BorderWidth="1px"/>
                                <asp:BoundField DataField="newChildID" HeaderText="New Child ID" ItemStyle-BorderStyle="solid" ItemStyle-BorderColor="#CC9966" ItemStyle-BorderWidth="1px"/>
                            </Columns>
                        </asp:GridView>                                               
                    </td>
                </tr>                           
            </table>
        </asp:Panel> 
        <asp:Panel ID="GroupReportPanel" runat="server" Visible="false">
            <table border="0" width="420px">
                <tr id="StdDevRow" runat="server">      
                    <td class="defaultLabelStyle" align="right"   style="width:170px;">Standard Deviation: </td>
                    <td style="width:230px;"><asp:Label ID="StdDevLabel" SkinID="RegularText" runat="server" /></td>                    
                </tr>
                <tr id="ScoreTypeRow" runat="server">
                    <td class="defaultLabelStyle" align="right" valign="top" style="width:170px;">Score Type(s): </td>
                    <td style="width:230px;"><asp:Label ID="ScoreTypeLabel" SkinID="RegularText" runat="server" /> </td>
                </tr>
                <tr id="FromDate1Row" runat="server">
                    <td class="defaultLabelStyle" align="right" style="width:170px;">From Date 1:</td>
                    <td style="width:230px;"><asp:Label ID="FromDate1Label" SkinID="regularText" runat="server" /> </td>
                </tr>         
                <tr id="ToDate1Row" runat="server">
                    <td class="defaultLabelStyle" align="right" style="width:170px;">To Date 1:</td>
                    <td style="width:230px;"><asp:Label ID="ToDate1Label" SkinID="regularText" runat="server" /> </td>
                </tr> 
                <tr id="FromDate2Row" runat="server">
                    <td class="defaultLabelStyle" align="right" style="width:170px;">From Date 2:</td>
                    <td style="width:230px;"><asp:Label ID="FromDate2Label" SkinID="regularText" runat="server" /> </td>
                </tr> 
                <tr id="ToDate2Row" runat="server">
                    <td class="defaultLabelStyle" align="right" style="width:170px;">To Date 2:</td>
                    <td style="width:230px;"><asp:Label ID="ToDate2Label" SkinID="regularText" runat="server" /> </td>
                </tr> 
                <tr id="FromDate3Row" runat="server">
                    <td class="defaultLabelStyle" align="right" style="width:170px;">From Date 3:</td>
                    <td style="width:230px;"><asp:Label ID="FromDate3Label" SkinID="regularText" runat="server" /> </td>
                </tr> 
                <tr id="ToDate3Row" runat="server">
                    <td class="defaultLabelStyle" align="right" style="width:170px;">To Date 3:</td>
                    <td style="width:230px;"><asp:Label ID="ToDate3Label" SkinID="regularText" runat="server" /> </td>
                </tr>                
            </table>
        </asp:Panel>

</form>
</body>
</html>