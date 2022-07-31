<%@ Page Trace="false" EnableTheming="true" Theme="Default" EnableEventValidation="false"
    MasterPageFile="~/Main.Master" Title="Child Assessment" Language="C#" AutoEventWireup="true"
    CodeBehind="ChildAssessment.aspx.cs" Inherits="BDI2Web.Assessment.ChildAssessment" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="radT" %>
<%@ Register Src="~/Controls/StudentNavigation.ascx" TagName="StudentNav" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MCP" runat="server">

    <script language="JavaScript" type="text/javascript">
        function ShowNewWindow(studentAssessmentID)
        {
            //Show new window
            //not providing a name as a second parameter will create a new window
            var oWindow = window.radopen ("QuestionNote.aspx?type=a&qid=" + studentAssessmentID, null);

            //Using the reference to the window its clientside methods can be called
            oWindow.SetSize (438, 280);
            oWindow.SetTitle ("Input Note");
          
        }   
        
        function OnClientClose(radWindow)
        {
        }
        
        function CheckAssessmentDate( assesmentType )
        {
            var year = <%=g_testDate.Year%>
            var month = <%=g_testDate.Month%> 
            var day = <%=g_testDate.Day%>

            var s_year = <%=s_testDate.Year%>
            var s_month = <%=s_testDate.Month%> 
            var s_day = <%=s_testDate.Day%>
            
            var testDate = new Date(year, month-1, day);
            var s_testDate = new Date(s_year, s_month-1, s_day);
            var today = new Date();
            var one_day=1000*60*60*24
            var days = Math.ceil( (today.getTime() - testDate.getTime()) / (one_day) );
            var s_days = Math.ceil( (today.getTime() - s_testDate.getTime()) / (one_day) );
            
            if(assesmentType == 1) //complete
            {
                if(days <= 45 )
                {
                    return confirm("It has been less than 45 days since you entered a Complete Assessment. ");
                }
                else
                {
                    return true;
                }
            }
            else //screener
            {
                if(s_days <= 15 )
                {
                    return confirm("It has been less than 15 days since you entered a Screener Assessment.");
                }
                else
                {
                    return true;
                }
            }            
        }
    </script>

    <asp:Label ID="LabelAssessmentTitle" SkinID="Title" runat="server" Text="Child Assessment" />
    <asp:Label ID="AccessDeniedLabel" Visible="false" Text="You do not have the correct privilege to assess the assessment page."
        CssClass="errorMessage" runat="server" />
    <asp:Panel ID="AssessChildPanel" runat="server">
        <div class="text">
            Use this page to view a list of assessments administered to a child and to edit
            or add assessments to a record.
            <div style="height: 5px">
                &nbsp;</div>
            <div class="leftIndent">
                To view only complete assessments, click <b>Complete Assessment</b> button. To view
                only screener assessments, click <b>Screener</b> button. To view all the assessments
                for the child, click <b>All</b> button. To view assessments that were deleted from
                the record, click <b>Deleted</b> button.</div>
            <div class="leftIndent">
                To add a new assessment to the record, click the <b>New Complete Assessment</b>
                button. To add a new screener to the record, click the <b>New Screener</b> button.</div>
            <div class="leftIndent">
                To merge assessment information, click the check boxes preceding the assessments
                you want to combine, and then click the <b>Merge Selected</b> button.</div>
            <div>
                <b>Note:</b> You cannot undo a merge after it has been completed.</div>
            <div class="leftIndent">
                To delete an assessment from the child’s record, click the corresponding <b>Delete</b>
                icon. When the Delete confirmation dialog appears, click <b>OK</b> button to remove
                the assessment from the list.</div>
            <div class="leftIndent">
                To undelete an assessment, click the <b>Undelete</b> icon. Click <b>OK</b> button
                to reinsert the assessment into the record.</div>
            <div class="leftIndent">
                To access Program Notes from the Assessment(s) page, click the Program Note link
                to review/add a free-format or pre-defined Program Note(s).</div>
        </div>
        <asp:Label ID="LabelStudentID" runat="server" Visible="False"></asp:Label>
        <asp:Label ID="LabelAssessmentType" runat="server" Visible="False"></asp:Label><br />
        <table border="0" cellpadding="0" cellspacing="0" style="width: 740px">
            <tr>
                <td colspan="3" align="right" height="28px">
                    <uc1:StudentNav ID="StudentNav" runat="server" />
                </td>
            </tr>
            <tr>
                <td style="width: 400px">
                    <asp:RadioButtonList ID="RadioButtonListType" runat="server" RepeatDirection="Horizontal"
                        AutoPostBack="True" Width="340px" OnSelectedIndexChanged="RadioButtonListType_SelectedIndexChanged">
                        <asp:ListItem Value="1">Complete Assessment</asp:ListItem>
                        <asp:ListItem Value="2">Screener</asp:ListItem>
                        <asp:ListItem Value="4">Deleted</asp:ListItem>
                        <asp:ListItem Selected="True" Value="3">All</asp:ListItem>
                    </asp:RadioButtonList>
                </td>
                <td style="width: 160px" align="center" valign="middle">
                    <asp:Button ID="ButtonNewFull" runat="server" Text="New Complete Assessment" Width="166px"
                        OnClick="ButtonNewFull_Click" OnClientClick="if( CheckAssessmentDate(1) ) return true; else return false;" />
                </td>
                <td style="width: 160px" align="center" valign="middle">
                    <asp:Button ID="ButtonNewScreener" runat="server" Text="New Screener" Width="140px"
                        OnClick="ButtonNewScreener_Click" OnClientClick="if( CheckAssessmentDate(2) ) return true; else return false;" />
                </td>
            </tr>
            <tr>
                <td style="height: 8px" colspan="3">
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    <asp:Panel ID="ResultsPanel" runat="server">
                        <asp:Label ID="NoChildrenLabel" runat="Server" />
                        <radT:RadAjaxLoadingPanel runat="Server" ID="AjaxLoadingPanel1">
                            <asp:Image ID="Image2" ImageUrl="~/Images/loading3.gif" AlternateText="Loading" BorderWidth="0px"
                                runat="server"></asp:Image>
                        </radT:RadAjaxLoadingPanel>
                        <asp:Repeater ID="RepeaterAssessment" runat="server" OnItemCommand="RepeaterAssessment_ItemCommand">
                            <HeaderTemplate>
                                <table cellspacing="1" cellpadding="1" border="1" style="width: 740px; background-color: White;
                                    border-width: 1px; border-style: None;">
                                    <tr>
                                        <td align="center" class="gridBackground">
                                            <asp:Label ID="Label10" runat="server" SkinID="GridFontColor" Text="Merge Selection"></asp:Label>
                                        </td>
                                        <td align="center" class="gridBackground">
                                            <asp:Label ID="Label11" runat="server" SkinID="GridFontColor" Text="First Date"></asp:Label>
                                        </td>
                                        <td align="center" class="gridBackground">
                                            <asp:Label ID="Label12" runat="server" SkinID="GridFontColor" Text="Retest"></asp:Label>
                                        </td>
                                        <td align="center" class="gridBackground">
                                            <asp:Label ID="Label1" runat="server" SkinID="GridFontColor" Text="Norm"></asp:Label>
                                        </td>
                                        <td align="center" class="gridBackground">
                                            <asp:Label ID="Label13" runat="server" SkinID="GridFontColor" Text="Instrument"></asp:Label>
                                        </td>
                                        <td align="center" class="gridBackground">
                                            <asp:Label ID="Label14" runat="server" SkinID="GridFontColor" Text="ADP"></asp:Label>
                                        </td>
                                        <td align="center" class="gridBackground">
                                            <asp:Label ID="Label15" runat="server" SkinID="GridFontColor" Width="28px" Text="P-S"></asp:Label>
                                        </td>
                                        <td align="center" class="gridBackground">
                                            <asp:Label ID="Label16" runat="server" SkinID="GridFontColor" Text="COM"></asp:Label>
                                        </td>
                                        <td align="center" class="gridBackground">
                                            <asp:Label ID="Label17" runat="server" SkinID="GridFontColor" Text="MOT"></asp:Label>
                                        </td>
                                        <td align="center" class="gridBackground">
                                            <asp:Label ID="Label18" runat="server" SkinID="GridFontColor" Text="COG"></asp:Label>
                                        </td>
                                        <td align="center" class="gridBackground">
                                            <asp:Label ID="Label19" runat="server" SkinID="GridFontColor" Text="Observations"></asp:Label>
                                        </td>
                                        <td align="center" class="gridBackground">
                                            <asp:Label ID="Label20" runat="server" SkinID="GridFontColor" Text="Program Note"></asp:Label>
                                        </td>
                                        <td align="center" class="gridBackground">
                                            <asp:Label ID="Label21" runat="server" SkinID="GridFontColor" Text="Delete"></asp:Label>
                                        </td>
                                    </tr>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <tr>
                                    <td align="center">
                                        <asp:CheckBox ID="MergeSelect" runat="server" Text='' Enabled='<%#Eval("linkEnabled")%>' />
                                    </td>
                                    <td align="left">
                                        <asp:HiddenField ID="studentAssessmentID" runat="server" Value='<%#Eval("studentAssessmentID")%>' />
                                        <asp:HiddenField ID="assessmentID" runat="server" Value='<%#Eval("assessmentID")%>' />
                                        <asp:HiddenField ID="assessmentTypeID" runat="server" Value='<%#Eval("assessmentTypeID")%>' />
                                        &nbsp;&nbsp;<asp:LinkButton ID="LinkButtonDate" Enabled='<%#Eval("linkEnabled")%>'
                                            runat="server" CommandArgument="EDIT"><%#Eval("firstTestDate")%></asp:LinkButton>
                                    </td>
                                    <td align="center">
                                        <asp:Label ID="LabelRetest" runat="server" Text='<%#Eval("retest")%>'></asp:Label>
                                    </td>
                                    <td align="center" style="width:70px;">
                                        <asp:Label ID="LabelNorm" runat="server" Text='<%#Eval("NormType")%>'></asp:Label>
                                    </td>
                                    <td align="center">
                                        &nbsp;&nbsp;<asp:Label ID="LabelInstrument" runat="server" Text='<%#Eval("instrument")%>'></asp:Label>
                                    </td>
                                    <td align="center">
                                        <asp:Label ID="LabelADP" runat="server" Text='<%#Eval("ADP")%>'></asp:Label>
                                    </td>
                                    <td align="center">
                                        <asp:Label ID="LabelP_S" runat="server" Text='<%#Eval("P_S")%>'></asp:Label>
                                    </td>
                                    <td align="center">
                                        <asp:Label ID="LabelCOM" runat="server" Text='<%#Eval("COM")%>'></asp:Label>
                                    </td>
                                    <td align="center">
                                        <asp:Label ID="LabelMOT" runat="server" Text='<%#Eval("MOT")%>'></asp:Label>
                                    </td>
                                    <td align="center">
                                        <asp:Label ID="LabelCOG" runat="server" Text='<%#Eval("COG")%>'></asp:Label>
                                    </td>
                                    <td align="center">
                                        <asp:Label ID="LabelObeserver" runat="server" Text='<%#Eval("TestObserver")%>'></asp:Label>
                                    </td>
                                    <td align="center" style="width:80px;">
                                        <asp:HyperLink NavigateUrl='<%# "javascript:ShowNewWindow(" + Eval("studentAssessmentID") + ");" %>'
                                            runat="server" Enabled='<%#Eval("linkEnabled")%>' Text="Program Note"></asp:HyperLink>
                                    </td>
                                    <td align="center">
                                        <asp:ImageButton ID="ButtonDelete" runat="server" CommandArgument='<%#Eval("commandArgument")%>'
                                            AlternateText="Delete" ImageUrl='<%#Eval("deleteImage")%>' OnClientClick='<%#Eval("deleteConfirm")%>' />
                                    </td>
                                </tr>
                            </ItemTemplate>
                            <FooterTemplate>
                                </table>
                            </FooterTemplate>
                        </asp:Repeater>
                        <br />
                        <%--changed OnClientClick logic because of AJAX behavior--%>
                        <asp:Button ID="ButtonMerge" runat="server" OnClick="ButtonMerge_Click" Text="Merge Selected"
                            OnClientClick="if(!confirm('Are you sure that you want to merge the selected assessements?')) return false;" />
                </td>
    </asp:Panel>
    </td> </tr> </table> </asp:Panel>
    <radT:RadAjaxManager ID="RadAjaxManager1" runat="server">
        <AjaxSettings>
            <radT:AjaxSetting AjaxControlID="RadioButtonListType">
                <UpdatedControls>
                    <radT:AjaxUpdatedControl ControlID="ResultsPanel" LoadingPanelID="AjaxLoadingPanel1" />
                    <radT:AjaxUpdatedControl ControlID="MPErrorPanel" />
                    <radT:AjaxUpdatedControl ControlID="RadioButtonListType" />
                </UpdatedControls>
            </radT:AjaxSetting>
            <radT:AjaxSetting AjaxControlID="ButtonMerge">
                <UpdatedControls>
                    <radT:AjaxUpdatedControl ControlID="MPErrorPanel" />
                </UpdatedControls>
            </radT:AjaxSetting>
        </AjaxSettings>
    </radT:RadAjaxManager>
    <radT:RadWindowManager ID="RadWindowManager1" Modal="true" VisibleOnPageLoad="false"
        Behaviors="Close" VisibleStatusbar="false" runat="server" OnClientClose="OnClientClose">
    </radT:RadWindowManager>
</asp:Content>
