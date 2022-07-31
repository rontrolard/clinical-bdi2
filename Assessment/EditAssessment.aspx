<%@ Page  Trace="false" EnableTheming="true" Theme="Default"  EnableEventValidation="false" MaintainScrollPositionOnPostback="true" MasterPageFile="~/Main.Master" Title="Edit Assessment" Language="C#" AutoEventWireup="true" CodeBehind="EditAssessment.aspx.cs" Inherits="BDI2Web.Assessment.EditAssessment" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="radT" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MCP" runat="server">
    <script language="JavaScript" type="text/javascript">
        
        var msgIndex = 1;
        
        function GetMsg()
        {
            if(msgIndex == 1)
            {
                return "A {basal} {ceiling} have not been established. Click OK to save this assessment.";
            }
            else //2
            {
                return "You may not begin the assessment at an age higher than your subject's chronological age. The age-appropriate starting point is indicated by the first item that appears in black text on the screen. You must enter data at the suggested starting point or lower. Refer to Chapter 3 of the Examiner's Manual for additional information.";
            }
        }
        function ShowNewWindow(questionID)
        {
            //Show new window
            //not providing a name as a second parameter will create a new window
            var oWindow = window.radopen ("QuestionNote.aspx?type=q&qid=" + questionID, null);

            //Using the reference to the window its clientside methods can be called
            oWindow.SetSize (438, 330);
            oWindow.SetTitle ("Input Note");      
        }   
        
        function OnClientClose(radWindow)
        {
        }
        
        function ClearSection(questionID)
        {
            var oTDA;
            var oTDB;
            var oRadios;

            oTDA = document.getElementById(questionID + '_score');                        
            oRadios = oTDA.getElementsByTagName('input');
            for (i = oRadios.length-1; i > -1; i--) 
            {
                oRadios[i].checked = false;
            }
            
            oTDB = document.getElementById(questionID + '_proc');
            oRadios = oTDB.getElementsByTagName('input');
            for (i = oRadios.length-1; i > -1; i--) 
            {
                oRadios[i].checked = false;
            }
            oRadios[3].value = '1';
        }
    </script>

    <asp:Label ID="LabelQuestionTitle" runat="server"></asp:Label>
    <asp:Label ID="LabelSpanshRetest" runat="server" Text="&nbsp;(Spanish Retest)" Visible="False"></asp:Label><br />
    <asp:Label ID="LabelReTest" runat="server" Visible="False">false</asp:Label>
    <asp:Label ID="LabelAgeMonths" runat="server" Visible="False"></asp:Label><br />
    <table width="720px" border="0" cellpadding="1" cellspacing="0" style="border-color:#CC9966;border-width:1px;border-style:solid;">
					<tr align="center" valign="middle">
						<td class="gridBackground" style="font-weight: bold; height: 21px;">
						<asp:Label ID="Label4" runat="server" Text="Domain" ForeColor="White"></asp:Label>
						</td>
						<td class="gridBackground" style="font-weight: bold; height: 21px;">
						<asp:Label ID="LabelSubdomainTitle" runat="server" Text="SubDomain" ForeColor="White"></asp:Label>
						</td>
						<td class="gridBackground" style="font-weight: bold; height: 21px;">
						<asp:Label ID="Label5" runat="server" Text="Date" ForeColor="White"></asp:Label>
						</td>
						<td class="gridBackground" style="font-weight: bold; height: 21px;">
						<asp:Label ID="Label6" runat="server" Text="Chronological Age" ForeColor="White"></asp:Label>
						</td>
						<td class="gridBackground" style="font-weight: bold; height: 21px;">
						<asp:Label ID="Label7" runat="server" Text="Basal" ForeColor="White"></asp:Label>
						</td>
						<td class="gridBackground" style="font-weight: bold; height: 21px;">
						<asp:Label ID="Label8" runat="server" Text="Ceiling" ForeColor="White"></asp:Label>
						</td>
					</tr>
					<tr align="center" valign="middle">
						<td  style="background-color: #fef5de">
                        <asp:Label ID="LabelDomain" runat="server" Text=""></asp:Label>
						</td>
						<td  style="background-color: #fef5de">
                        <asp:Label ID="LabelSubdomain" runat="server" Text=""></asp:Label>
						</td>
						<td  style="background-color: #fef5de">
                        <asp:Label ID="LabelDate" runat="server" Text=""></asp:Label>
						</td>
						<td  style="background-color: #fef5de">
                        <asp:Label ID="LabelAge" runat="server" Text=""></asp:Label>
						</td>
						<td  style="background-color: #fef5de">
                        <asp:Label ID="LabelBasal" runat="server" Text=""></asp:Label>
						</td>
						<td  style="background-color: #fef5de">
                        <asp:Label ID="LabelCeiling" runat="server" Text=""></asp:Label>
						</td>
					</tr>
	</table>
    <br />
    <asp:Panel ID="QuestionPanel" runat="server">
    <table width="720px" border="0" cellpadding="0" cellspacing="0">
    <tr>
    <td>
        <asp:Button ID="ButtonBC" runat="server" Text="Show Basal / Ceiling" OnClick="ButtonBC_Click" />
    </td>
    <td align="right">
        <asp:Button ID="ButtonOrgTest" runat="server" Text="Original Test" OnClick="ButtonOrgTest_Click" />
        <asp:Button ID="ButtonReTest" runat="server" Text="Spanish Retest" OnClick="ButtonReTest_Click" />
        <asp:Button ID="ButtonClear" runat="server" Text="Clear" OnClick="ButtonClear_Click" />
        <asp:Button ID="ButtonCancel" runat="server" Text="Cancel" OnClick="ButtonCancel_Click" OnClientClick="if (confirm('Canceling will NOT save current information. Are you sure you want to cancel? Click OK to end Assessment WITHOUT saving. Click Cancel to continue with this Assessment.')) return true; else return false;" />
        <asp:Button ID="ButtonDone" runat="server" Text="Done" OnClick="ButtonDone_Click" />
        <asp:Button ID="ButtonDone2" runat="server" Text="Done2" OnClick="ButtonDone2_Click" OnClientClick = "if (confirm(GetMsg())) return true; else return false;" />
    </td>
    </tr>
    <tr><td colspan="2" style="height: 8px"></td></tr>
    <tr>
    <td colspan="2" style="width: 720px">
        <asp:Repeater ID="RepeaterQuestion" runat="server" OnItemDataBound="RepeaterQuestion_ItemDataBound">
            <HeaderTemplate>
                <table cellspacing="1" cellpadding="1" style="width:720px; background-color:#CC9966; border-color:#CC9966; border-width:1px; border-style:solid;">
                <tr>
                <td class="gridBackground" style="width: 20px;" align="center">
					<asp:Label ID="Label4" runat="server" Text="&nbsp;&nbsp;&nbsp;" ForeColor="White"></asp:Label>                    
                </td>
                <td class="gridBackground" style="width: 30px;" align="center">
					<asp:Label ID="Label9" runat="server" Text="Item<br />#" ForeColor="White"></asp:Label>                    
                </td>
                <td class="gridBackground" style="width: 30px;" align="center">
					<asp:Label ID="Label10" runat="server" Text="Dev<br />Age" ForeColor="White"></asp:Label>                    
                </td>
                <td class="gridBackground" style="width: 370px;" align="center">
					<asp:Label ID="Label11" runat="server" Text="Description" ForeColor="White"></asp:Label>                    
                </td>
                <td class="gridBackground" style="width: 20px;" align="center">
					<asp:Label ID="Label1" runat="server" Text="&nbsp;&nbsp;&nbsp;" ForeColor="White"></asp:Label>                    
                </td>                
                <td class="gridBackground" style="width: 70px;" align="center">
					<asp:Label ID="Label12" runat="server" Text="Score" ForeColor="White"></asp:Label><br />
                    <img alt="" src="../images/2score.gif" width="16" height="16" />						
                    <img alt="" src="../images/1score.gif" width="16" height="16" />						
                    <img alt="" src="../images/0score.gif" width="16" height="16" />						
                </td>
                <td class="gridBackground" style="width: 70px;" align="center">
					<asp:Label ID="Label13" runat="server" Text="Procedure" ForeColor="White"></asp:Label><br />
					<img alt="" src="../images/smethod.gif" width="16" height="16" />
					<img alt="" src="../images/omethod.gif" width="16" height="16" />
					<img alt="" src="../images/imethod.gif" width="16" height="16" />
                </td>
                <td class="gridBackground" style="width: 20px;" align="center">
					<asp:Label ID="Label2" runat="server" Text="&nbsp;&nbsp;&nbsp;" ForeColor="White"></asp:Label>                    
                </td>
                <td class="gridBackground" style="width: 20px;" align="center">
					<asp:Label ID="LabelComment" runat="server" Text="Note" ForeColor="White"></asp:Label>
				</td>
                </tr>
            </HeaderTemplate>
            <ItemTemplate>
                <tr>
                    <td style="background-color: #FEF5DE;">
                        <asp:HiddenField ID="studentDomainID" runat="server" Value='<%#Eval("resultQuestionID")%>' />
                        <asp:HiddenField ID="questionID" runat="server" Value='<%#Eval("questionID")%>' />
                        <asp:HiddenField ID="developmentAgeMax" runat="server" Value='<%#Eval("developmentAgeMax")%>' />
                        <asp:HiddenField ID="score" runat="server" Value='<%#Eval("score")%>' />
                        <asp:HiddenField ID="reTestScore" runat="server" Value='<%#Eval("reTestScore")%>' />
                        <asp:HiddenField ID="adminProcedureID" runat="server" Value='<%#Eval("adminProcedureID")%>' />
                        <asp:HiddenField ID="reTestAdminProcedureID" runat="server" Value='<%#Eval("reTestAdminProcedureID")%>' />
                        <asp:HiddenField ID="Score0" runat="server" Value='<%#Eval("Score0")%>' />
                        <asp:HiddenField ID="Score1" runat="server" Value='<%#Eval("Score1")%>' />
                        <asp:HiddenField ID="Score2" runat="server" Value='<%#Eval("Score2")%>' />
                        <asp:HiddenField ID="structured" runat="server" Value='<%#Eval("structured")%>' />
                        <asp:HiddenField ID="interview" runat="server" Value='<%#Eval("interview")%>' />
                        <asp:HiddenField ID="observation" runat="server" Value='<%#Eval("observation")%>' />
                        <asp:Label ID="questionStatus" runat="server" ForeColor="Blue" Text='<%#Eval("questionStatus")%>'></asp:Label>
                    </td>
                    <td style="background-color: #FEF5DE;" align="left">
                        <asp:Label ID="questionItemNumber" runat="server" Text='<%#Eval("itemNumber")%>'></asp:Label>
                    </td>
                    <td style="background-color: #FEF5DE;" align="left">
                        <asp:Label ID="questionAge" runat="server" Text='<%#Eval("developmentAge")%>'></asp:Label>
                    </td>
                    <td style="background-color: #FEF5DE;" align="left">
                        <asp:Label ID="description" runat="server" Text='<%#Eval("description")%>'></asp:Label>
                    </td>
                    <td style="background-color: #FEF5DE;" align="center">
                        <a href="#" onclick="ClearSection(<%#Eval("questionID")%>);">Clear</a>
                    </td>                    
                    <td style="background-color: #FEF5DE;" align="center" id='<%#Eval("questionID")%>_score'>
                        <asp:RadioButtonList ID="RadioButtonListScore"  DataTextField='<%#Eval("questionID")%>' runat="server" RepeatDirection="Horizontal"
                            RepeatLayout="Flow" ToolTip="2 - 1 - 0">
                            <asp:ListItem Value="2" Text=""></asp:ListItem>
                            <asp:ListItem Value="1" Text=""></asp:ListItem>
                            <asp:ListItem Value="0" Text=""></asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                    <td style="background-color: #FEF5DE;" align="center" id='<%#Eval("questionID")%>_proc'>
                        <asp:RadioButtonList ID="RadioButtonListProcdure" runat="server" RepeatDirection="Horizontal"
                            RepeatLayout="Flow" ToolTip="S - O - I">
                            <asp:ListItem Value="1" Text=""></asp:ListItem>
                            <asp:ListItem Value="2" Text=""></asp:ListItem>
                            <asp:ListItem Value="3" Text=""></asp:ListItem>
                        </asp:RadioButtonList>
                        <asp:HiddenField ID="HiddenFieldClear" runat="server" Value="0" />
                    <td style="background-color: #FEF5DE;" align="center">
                        <asp:Label ID="LabelValidation" runat="server" ForeColor="Red" Text="<" Font-Bold="True"></asp:Label>
                    </td>
                    </td>
                    <td style="background-color: #FEF5DE;" align="center">
                        <a href="#" onclick="ShowNewWindow(<%#Eval("questionID")%>)">Note</a>
                    </td>
                </tr>
            </ItemTemplate>
            <FooterTemplate>
            </table>
            </FooterTemplate>
        </asp:Repeater>   
    </td>
    </tr>
    </table> 
    </asp:Panel>
    
    <radT:RadWindowManager
        Id="RadWindowManager1"
        modal="true"             
        VisibleOnPageLoad="false"  
        VisibleStatusbar="false"      
        runat="server"
        Behaviors="Close"
        OnClientClose = "OnClientClose"
        >        
    </radT:RadWindowManager>
    <script language="JavaScript" type="text/javascript">
        function OnDone2()
        {
            msgIndex = 1;
            setTimeout('afterHalfSecond()', 500)
        }    
        function OnDone3()
        {
            msgIndex = 2;
            setTimeout('afterHalfSecond()', 500)
        }    
        function afterHalfSecond()
        {
            document.getElementById("<%=ButtonDone2.ClientID%>").click();
        }
    </script>
    <asp:Literal ID="LiteralScript" runat="server"></asp:Literal>        
</asp:Content>
