<%@ Page Trace="false" EnableTheming="true" Theme="Default" EnableEventValidation="false"
    MasterPageFile="~/Main.Master" Title="New / Edit Screener" Language="C#" AutoEventWireup="true"
    CodeBehind="EditScreener.aspx.cs" Inherits="BDI2Web.Assessment.EditScreener" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="radT" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MCP" runat="server">
    <radT:RadCodeBlock ID="RadCodeBlock1" runat="server">

        <script type="text/javascript">

            function saveMessage() {

                var message = "1. Only the selected (checked) domain(s) will be saved (norm selections will apply to all domains). Click on OK to save. Click on Cancel to change the selections.\n\n2. By entering the Raw Score, you will be responsible for the Basal and Ceiling. Not all reports will be available using this Method."

                if (document.getElementById("ctl00_MCP_RadioButtonListInputType_1").checked) {

                    message = "1. Only the selected (checked) domain(s) will be saved (norm selections will apply to all domains). Click on OK to save. Click on Cancel to change the selections."
                }

                if (!confirm(message))
                    return false;
                return true;


            }
            function OnClientClose(radWindow) {
                document.getElementById("<%=ButtonSubmit.ClientID%>").click();
            }
            function OnWindowClose(radWindow) {

            }
            function SpanishAlert(clientID, assessmentType) {
                var message;
                if (assessmentType == 'SCREENER') {
                    message = "If Spanish is selected, only raw scores will be available for that Domain.";
                }
                else {
                    message = "If Spanish is selected for the Structured group, only raw scores will be available for that Domain and Sub-domain."
                }
                if (document.getElementById(clientID).checked == true) {
                    alert(message);
                }
            }

            function disableAjax4ModalWindow() {
                return false;
            }
            //              changed OnClientClick logic because of AJAX behavior
            //                function SaveMessage()
            //                {
            //                    if (confirm('1. Only the selected (checked) domain(s) will be saved. Click on OK to save. Click on Cancel to change the selections.\n\n2. By entering the Raw Score, you will be responsible for the Basal and Ceiling. Not all reports will be available using this Method.')) 
            //                        return true; 
            //                    else 
            //                        return false;
            //                }     
            //                function DeleteMessage()
            //                {
            //                    if (confirm('Are you sure you want to delete the selected domain(s)?')) 
            //                        return true; 
            //                    else 
            //                        return false;
            //                }     
        </script>

    </radT:RadCodeBlock>
    <asp:Label ID="LabelScreenerTitle" SkinID="Title" runat="server"></asp:Label>
    <div class="text">
        <div style="height: 5px">
            &nbsp;</div>
        <div class="leftIndent">
            Select the option (<i>Raw Scores</i> or <i>Item Details</i>) to indicate the type
            of scores you are entering for the assessment.</div>
        <div>
            <b>Note:</b> If you enter the raw scores, you will be responsible for the basal
            and ceiling for each score. Not all reports are available using this method.</div>
        <div class="leftIndent">
            Enter the examiner’s name and the test date for each subdomain assessed. <span runat="server"
                id="screener1" visible="false">If the assessment(s) were administered in Spanish,
                click the corresponding <b>Spanish</b> check box(es).</span> <span runat="server"
                    id="complete1" visible="false">If the assessments were administered in Spanish,
                    click the corresponding check boxes to indicate the procedure (structured [S] or
                    observation/interview [O/I]) used to administer the assessment for each subdomain.</span>
        </div>
        <div class="leftIndent">
            Enter the scores and test observations.</div>
        <div class="leftIndent">
            If you selected to enter raw scores, type the scores for each domain in the appropriate
            text box.</div>
        <div class="leftIndent" runat="server" id="screener2" visible="false">
            If you selected to enter item details, click the <b>Next</b> button and select options
            to indicate scores and procedures for each item that was administered. Click the
            <b>Done</b> button to proceed to the next domain or to skip a domain. After completing
            the entry of scores, click the <b>Domain Totals</b> button to view recommendations.</div>
        <div class="leftIndent" runat="server" id="complete2" visible="false">
            If you selected to enter item details, click the <b>Next</b> button and select options
            to indicate scores and procedures for each item that was administered. Click the
            <b>Done</b> button to proceed to the next domain or to skip a domain. After you
            complete entering the scores, the program lists the age-equivalent (AE), percentile
            rank (PR), and standard scores (SS) for each subdomain that was assessed. Click
            the <b>Domain Totals</b> button view complete totals for each assessed domain.</div>
        <div class="leftIndent">
            Click the <b>Save</b> button to save the assessment information to the record.</div>
        <div class="leftIndent">
            To access Program Notes from the
            <asp:Literal ID="AssmtTypeLiteral" runat="server" />
            Assessment page, click the Program Note button to review/add a free-format or pre-defined
            Program Note(s).</div>
    </div>
    <asp:Panel ID="ResultPanel" runat="server">
        <asp:Label ID="LabelFirstDate" runat="server"></asp:Label><br />
        <asp:Label ID="LabelAge" runat="server"></asp:Label><br />
        <asp:Label ID="LabelStudentID" runat="server" Visible="False"></asp:Label>
        <asp:Label ID="LabelAssessmentTypeID" runat="server" Visible="False"></asp:Label>
        <asp:Label ID="LabelStudentAssessmentID" runat="server" Visible="False"></asp:Label>
        <asp:Label ID="LabelColor" runat="server" Visible="False"></asp:Label><br />
        <div>
            <div>
                <asp:Label runat="server" ID="lblscoringmethods">Scoring Method:</asp:Label>
            </div>
            <div style="margin-left: 120px; margin-top: -22px;">
                <asp:RadioButtonList ID="RadioButtonListInputType" runat="server" RepeatDirection="Horizontal"
                    AutoPostBack="True" OnSelectedIndexChanged="RadioButtonListInputType_SelectedIndexChanged">
                    <asp:ListItem Selected="True" Value="0">Raw&nbsp;Scores&nbsp;<img title="Raw Scores" src="../Images/raw_scores.gif" /></asp:ListItem>
                    <asp:ListItem Value="1">Item&nbsp;Details&nbsp;<img title="Item Details" src="../Images/item_details.gif" /></asp:ListItem>
                </asp:RadioButtonList>
            </div>
            <div style="margin-left: 360px; margin-top: -32px;">
                <asp:Button ID="ButtonProgramNote" runat="server" Text="Program Note" OnClientClick="return disableAjax4ModelWindow();" />&nbsp;
                <asp:Button ID="ButtonObservation" runat="server" Text="Test Observations" OnClick="ButtonObservation_Click" />&nbsp;
                <asp:Button ID="ButtonTotal" runat="server" Text="Domain Totals" OnClientClick="return disableAjax4ModalWindow();" />&nbsp;
            </div>
        </div>
        <div style="margin-top: 20px;">
            <div>
                <asp:Label runat="server" ID="lblnormType">Norm Type:</asp:Label>
            </div>
            <div style="margin-left: 120px; margin-top: -22px;">
                <asp:UpdatePanel ID="updenorms" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:RadioButtonList ID="RadioButtonNorms" runat="server" RepeatDirection="Horizontal"
                            AutoPostBack="True" OnSelectedIndexChanged="RadioButtonList1_SelectedIndexChanged">
                            <asp:ListItem Value="0"><b>BDI-2 Norms</b></asp:ListItem>
                            <asp:ListItem Selected="True" Value="1"><b>BDI-2 NU Norms</b></asp:ListItem>
                        </asp:RadioButtonList>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="divtext">
                <span class="defaultLabelStyle" style="font-size: 10px; color: Red;">When choosing the
                    normative set for exit assessment, it is best practice to use the same normative
                    set used for entry assessment. </span>
            </div>
        </div>
        <%--<table style="width: 740px" cellspacing="0" cellpadding="0">
            <tr>
                <td colspan="1" style="width: 15%">
                    <asp:Label runat="server" ID="lblscoringmethods">Scoring Method:</asp:Label>
                </td>
                <td colspan="2">
                    <asp:RadioButtonList ID="RadioButtonListInputType" runat="server" RepeatDirection="Horizontal"
                        AutoPostBack="True" OnSelectedIndexChanged="RadioButtonListInputType_SelectedIndexChanged">
                        <asp:ListItem Selected="True" Value="0">Raw&nbsp;Scores&nbsp;<img title="Raw Scores" src="../Images/raw_scores.gif" /></asp:ListItem>
                        <asp:ListItem Value="1">Item&nbsp;Details&nbsp;<img title="Item Details" src="../Images/item_details.gif" /></asp:ListItem>
                    </asp:RadioButtonList>
                </td>
                <td colspan="2" align="right">
                    <asp:Button ID="ButtonProgramNote" runat="server" Text="Program Note" OnClientClick="return disableAjax4ModelWindow();" />&nbsp;
                    <asp:Button ID="ButtonObservation" runat="server" Text="Test Observations" OnClick="ButtonObservation_Click" />&nbsp;
                    <asp:Button ID="ButtonTotal" runat="server" Text="Domain Totals" OnClientClick="return disableAjax4ModalWindow();" />&nbsp;
                </td>
            </tr>
            <tr>
                <td colspan="1" class="tdwidthnorm">
                    <asp:Label runat="server" ID="lblnormType">Norm Type:</asp:Label>
                </td>
                <td colspan="4" class="tdwidth">
                    <asp:UpdatePanel ID="updenorms" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:RadioButtonList ID="RadioButtonNorms" runat="server" RepeatDirection="Horizontal"
                                AutoPostBack="True" OnSelectedIndexChanged="RadioButtonList1_SelectedIndexChanged">
                                <asp:ListItem Value="0"><b>BDI-2 Norms</b></asp:ListItem>
                                <asp:ListItem Selected="True" Value="1"><b>BDI-2 NU Norms</b></asp:ListItem>
                            </asp:RadioButtonList>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
                <td colspan="2" style="color: Red">
                    <span class="defaultLabelStyle" style="font-size: 10px;">
                        <p>
                            When choosing the normative set for exit assessment, it is best practice to use
                            the same normative set used for entry assessment.</p>
                    </span>
                </td>
            </tr>
        </table>--%>
        <table style="width: 740px" cellspacing="0" cellpadding="0">
            <tr>
                <td style="height: 5px;">
                </td>
            </tr>
            <tr>
                <td colspan="6">
                    <table cellspacing="0" cellpadding="0" style="width: 740px" border="0">
                        <tr>
                            <td style="width: 200px; height: 45px;" class="gridBackground" align="center" valign="middle">
                                <asp:Label ID="Label1" runat="server" ForeColor="White" CssClass="gridFontColor"
                                    Text="Select Domain"></asp:Label>
                            </td>
                            <td style="width: 50px; height: 45px;" class="gridBackground" align="left" valign="middle">
                                <asp:CheckBox ID="CheckBoxAll" runat="server" Text=" " OnCheckedChanged="CheckBoxAll_CheckedChanged"
                                    AutoPostBack="True" />
                                <asp:Label ID="Label3" runat="server" ForeColor="White" CssClass="gridFontColor"
                                    Text="All"></asp:Label>
                            </td>
                            <td style="width: 15px; height: 45px;" class="gridBackground" align="left" valign="middle">
                                <asp:Label ID="Label2" runat="server" ForeColor="White" CssClass="gridFontColor"
                                    Text="&nbsp;&nbsp;"></asp:Label>
                            </td>
                            <td style="width: 120px; height: 45px;" class="gridBackground" align="center" valign="middle">
                                <asp:Label ID="Label4" runat="server" ForeColor="White" CssClass="gridFontColor"
                                    Text="Test Date"></asp:Label>
                                <asp:ImageButton ID="ImageButtonCalendar" runat="server" ImageUrl="~/images/calendar.jpg" />
                            </td>
                            <td style="width: 150px; height: 45px;" class="gridBackground" align="center" valign="middle">
                                <asp:Label ID="Label5" runat="server" ForeColor="White" CssClass="gridFontColor"
                                    Text="Examiner"></asp:Label>
                                <asp:ImageButton ID="ImageButtonExaminer" runat="server" ImageUrl="~/images/little_guy2.gif" />
                            </td>
                            <td style="width: 50px; height: 45px;" class="gridBackground" align="center" valign="middle">
                                <asp:Label ID="Label6" runat="server" ForeColor="White" CssClass="gridFontColor"
                                    Text="Raw"></asp:Label><br />
                                <asp:Label ID="Label7" runat="server" ForeColor="White" CssClass="gridFontColor"
                                    Text="Score"></asp:Label>
                            </td>
                            <td style="width: 80px; height: 45px;" class="gridBackground" align="center" valign="middle">
                                <asp:Label ID="Label8" runat="server" ForeColor="White" CssClass="gridFontColor"
                                    Text="Spanish"></asp:Label>
                                <asp:Label ID="LabeSpanishIO" runat="server" ForeColor="White" Text="<br />S&nbsp;&nbsp;I/O"></asp:Label>
                            </td>
                            <td style="width: 20px; height: 45px;" class="gridBackground" align="center" valign="middle">
                                <asp:Label ID="LabeAE" runat="server" ForeColor="White" CssClass="gridFontColor"
                                    Text="AE"></asp:Label>
                            </td>
                            <td style="width: 35px; height: 45px;" class="gridBackground" align="center" valign="middle">
                                <asp:Label ID="LabelPR" runat="server" ForeColor="White" CssClass="gridFontColor"
                                    Text="&nbsp;PR&nbsp;"></asp:Label>
                            </td>
                            <td style="width: 20px; height: 45px;" class="gridBackground" align="center" valign="middle">
                                <asp:Label ID="LabelSS" runat="server" ForeColor="White" CssClass="gridFontColor"
                                    Text="SS"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td valign="top" colspan="6">
                    <asp:Panel ID="RepeaterPanel" runat="server">
                        <asp:Repeater ID="RepeaterDomain" runat="server" OnItemDataBound="RepeaterDomain_ItemDataBound">
                            <HeaderTemplate>
                                <table cellspacing="0" cellpadding="0" style="width: 740px" border="0">
                            </HeaderTemplate>
                            <ItemTemplate>
                                <tr>
                                    <td style="width: 200; background-color: <%#Eval("rowColor")%>;">
                                        <asp:HiddenField ID="studentDomainID" runat="server" Value='<%#Eval("resultStudentDomainID")%>' />
                                        <asp:HiddenField ID="parentstudentDomainID" runat="server" Value='<%#Eval("parentResultStudentDomainID")%>' />
                                        <asp:HiddenField ID="domainID" runat="server" Value='<%#Eval("domainID")%>' />
                                        <asp:HiddenField ID="domainTypeID" runat="server" Value='<%#Eval("domainTypeID")%>' />
                                        <asp:Label ID="Description" runat="server" Text='<%#Eval("description")%>'></asp:Label>
                                        <asp:Image ID="ScoreTypeImage" runat="server" />
                                    </td>
                                    <td style="width: 50; background-color: <%#Eval("rowColor")%>;" align="left">
                                        <asp:CheckBox ID="domainSelect" runat="server" Checked='<%#Eval("selection")%>'>
                                        </asp:CheckBox>
                                    </td>
                                    <td style="width: 15; background-color: <%#Eval("rowColor")%>;" align="left">
                                        <asp:Label ID="LabelValidation" runat="server" ForeColor="Red" Text='<%#Eval("invalid")%>'
                                            Font-Bold="True"></asp:Label>
                                    </td>
                                    <td style="width: 120; background-color: <%#Eval("rowColor")%>;" align="center">
                                        <asp:TextBox ID="TextBoxTestDate" Width="110" MaxLength="10" runat="server" Enabled="true"
                                            Text='<%#Eval("testDate")%>'></asp:TextBox>
                                    </td>
                                    <td style="width: 150; background-color: <%#Eval("rowColor")%>;" align="center">
                                        <asp:TextBox ID="TextBoxStaff" Width="140" MaxLength="50" runat="server" Enabled="true"
                                            Text='<%#Eval("staffName")%>'></asp:TextBox>
                                    </td>
                                    <td style="width: 50; background-color: <%#Eval("rowColor")%>;" align="center">
                                        <asp:TextBox ID="TextBoxScore" SkinID="rightAligned" Width="40" runat="server" Enabled="true"
                                            Text='<%#Eval("rawscore")%>'></asp:TextBox>
                                    </td>
                                    <td style="width: 80; background-color: <%#Eval("rowColor")%>;" align="center">
                                        <asp:CheckBox ID="CheckBoxSpanish" runat="server" Enabled="true" Checked='<%#Eval("isSpanish")%>' />
                                        <asp:CheckBox ID="CheckBoxSpanishIO" runat="server" Enabled="true" Checked='<%#Eval("isSpanishIO")%>' />
                                    </td>
                                    <td style="width: 20; background-color: <%#Eval("rowColor")%>;" align="center">
                                        <asp:Label ID="LabelAEScore" runat="server" Text='<%#Eval("AEScore")%>'></asp:Label>
                                    </td>
                                    <td style="width: 35; background-color: <%#Eval("rowColor")%>;" align="center">
                                        <asp:Label ID="LabelPRScore" runat="server" ForeColor="Blue" Text='<%#Eval("PRScore")%>'></asp:Label>
                                    </td>
                                    <td style="width: 20; background-color: <%#Eval("rowColor")%>;" align="center">
                                        <asp:Label ID="LabelSSScore" runat="server" Text='<%#Eval("SSScore")%>'></asp:Label>
                                    </td>
                                </tr>
                            </ItemTemplate>
                            <FooterTemplate>
                                <tr>
                                    <td style="width: 200px; background-color: #FCF0C7;">
                                        <img alt="" src="../images/transparent_50.gif" width="200" height="8" />
                                    </td>
                                    <td style="width: 50px; background-color: #FCF0C7;" align="left">
                                        <img alt="" src="../images/transparent_50.gif" width="50" height="8" />
                                    </td>
                                    <td style="width: 15px; background-color: #FCF0C7;" align="left">
                                        <img alt="" src="../images/transparent_50.gif" width="15" height="8" />
                                    </td>
                                    <td style="width: 120px; background-color: #FCF0C7;" align="center">
                                        <img alt="" src="../images/transparent_50.gif" width="120" height="8" />
                                    </td>
                                    <td style="width: 150px; background-color: #FCF0C7;" align="center">
                                        <img alt="" src="../images/transparent_50.gif" width="150" height="8" />
                                    </td>
                                    <td style="width: 50px; background-color: #FCF0C7;" align="center">
                                        <img alt="" src="../images/transparent_50.gif" width="50" height="8" />
                                    </td>
                                    <td style="width: 80px; background-color: #FCF0C7" align="center">
                                        <img alt="" src="../images/transparent_50.gif" width="80" height="8" />
                                    </td>
                                    <td style="width: 20px; background-color: #FCF0C7" align="center">
                                        <img alt="" src="../images/transparent_50.gif" width="20" height="8" />
                                    </td>
                                    <td style="width: 35px; background-color: #FCF0C7" align="center">
                                        <img alt="" src="../images/transparent_50.gif" width="35" height="8" />
                                    </td>
                                    <td style="width: 20px; background-color: #FCF0C7" align="center">
                                        <img alt="" src="../images/transparent_50.gif" width="20" height="8" />
                                    </td>
                                </tr>
                                </table>
                            </FooterTemplate>
                        </asp:Repeater>
                    </asp:Panel>
                </td>
            </tr>
            <tr>
                <td style="height: 8px;">
                </td>
            </tr>
            <tr>
                <td align="center" colspan="6" style="padding-left: 100px">
                    <asp:Button ID="ButtonCancel" runat="server" Text="Back" OnClick="ButtonCancel_Click" />
                    <asp:Label ID="SpacerLabel" Text="" runat="Server" />
                    <asp:Button ID="ButtonSave" runat="server" Text="Save" OnClick="ButtonSave_Click"
                        OnClientClick=" if(!saveMessage()) return false;" />
                    <asp:Button ID="ButtonDelete" runat="server" Text="Delete" OnClick="ButtonDelete_Click"
                        OnClientClick="if (!confirm('Are you sure you want to delete the selected domain(s)?')) return false;" />
                    <asp:Button ID="ButtonNext" runat="server" Text="Next" OnClick="ButtonNext_Click" />
                    <asp:Button ID="ButtonSubmit" runat="server" OnClick="ButtonSubmit_Click" Text="Submit Date" />
                </td>
            </tr>
        </table>
        <br />
        &nbsp; &nbsp;
        <br />
        <div style="position: relative; z-index: 200000">
            <radT:RadWindowManager ID="RadWindowManager1" VisibleOnPageLoad="false" runat="server"
                OnClientClose="OnClientClose" Overlay="true">
                <Windows>
                    <radT:RadWindow ID="Radwindow1" runat="server" Title="Select Date and Domain(s)"
                        NavigateUrl="~/Assessment/TestCalendar.aspx" OpenerElementID="<%# ImageButtonCalendar.ClientID %>"
                        VisibleStatusbar="false" Behaviors="Close" Top="60px" Width="370px" Height="500px"
                        Modal="true">
                    </radT:RadWindow>
                    <radT:RadWindow ID="Radwindow2" runat="server" Title="Select Examiner and Domain(s)"
                        NavigateUrl="~/Assessment/TestExaminer.aspx" OpenerElementID="<%# ImageButtonExaminer.ClientID %>"
                        VisibleStatusbar="false" Behaviors="Close" Top="60px" Width="580px" Height="500px"
                        Modal="true">
                    </radT:RadWindow>
                </Windows>
            </radT:RadWindowManager>
            <radT:RadWindowManager ID="RadWindowManager2" VisibleOnPageLoad="false" runat="server"
                OnClientClose="OnWindowClose">
                <Windows>
                    <radT:RadWindow ID="Radwindow3" runat="server" NavigateUrl="~/Assessment/DomainTotals.aspx"
                        OpenerElementID="<%# ButtonTotal.ClientID %>" VisibleStatusbar="false" Behaviors="Close"
                        Top="150px" Width="570px" Height="300px" Modal="true">
                    </radT:RadWindow>
                    <radT:RadWindow ID="Radwindow4" runat="server" NavigateUrl="~/Assessment/QuestionNote.aspx"
                        OpenerElementID="<%# ButtonProgramNote.ClientID %>" VisibleStatusbar="false"
                        Behaviors="Close" Top="150px" Width="438px" Height="280px" Modal="true" ReloadOnShow="true">
                    </radT:RadWindow>
                </Windows>
            </radT:RadWindowManager>
        </div>
    </asp:Panel>
    <br />
    <radT:RadAjaxManager ID="RadAjaxManager1" runat="server">
        <AjaxSettings>
            <radT:AjaxSetting AjaxControlID="RadioButtonListInputType">
                <UpdatedControls>
                    <radT:AjaxUpdatedControl ControlID="ResultPanel" />
                </UpdatedControls>
            </radT:AjaxSetting>
            <radT:AjaxSetting AjaxControlID="Radwindow1">
                <UpdatedControls>
                    <radT:AjaxUpdatedControl ControlID="RepeaterPanel" />
                </UpdatedControls>
            </radT:AjaxSetting>
            <radT:AjaxSetting AjaxControlID="Radwindow2">
                <UpdatedControls>
                    <radT:AjaxUpdatedControl ControlID="RepeaterPanel" />
                </UpdatedControls>
            </radT:AjaxSetting>
            <radT:AjaxSetting AjaxControlID="RepeaterPanel">
                <UpdatedControls>
                    <radT:AjaxUpdatedControl ControlID="MPErrorPanel" />
                </UpdatedControls>
            </radT:AjaxSetting>
        </AjaxSettings>
    </radT:RadAjaxManager>
</asp:Content>
