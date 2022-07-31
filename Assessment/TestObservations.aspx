<%@ Page Trace="false" EnableTheming="true" Theme="Default" MasterPageFile="~/Main.Master" Title="Test Observation" Language="C#" AutoEventWireup="true" CodeBehind="TestObservations.aspx.cs" Inherits="BDI2Web.Assessment.TestObservations" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MCP" runat="server">
<script type="text/javascript" language="javascript">
</script>
<table cellspacing="0" cellpadding="0" width="740" border="0">
<tr valign="top">
    <td align="left" colspan="3">
        <asp:Label ID="LabelTitle" runat="server" Text=""></asp:Label><br />
        <asp:Label ID="LabelTestDate" runat="server" Text=""></asp:Label><br />
        <asp:Label ID="LabelAge" runat="server" Visible="False"></asp:Label><br />
        </td>
</tr>    

<tr><td colspan="2" style="height: 10px;"></td></tr>

<tr>
    <td valign="top" align="left">
    <!------------>
    <table cellspacing="0" cellpadding="0" width="210" border="0">
    <tr>
    <td align="center">
        <asp:Label ID="Label1" runat="server" Text="Click a Category" Font-Bold="True"></asp:Label>
    </td>
    </tr>
    
    <tr>
    <td style="width: 200px; height: 154px;" valign="top" align="left">
            <asp:Repeater ID="RepeaterCategory" runat="server"  OnItemDataBound="RepeaterCategory_ItemDataBound" OnItemCommand="RepeaterCategory_ItemCommand">
                <HeaderTemplate>
                    <table style=" background-color:White; BORDER-RIGHT: #bebebe 1px solid; BORDER-TOP: #bebebe 1px solid; BORDER-LEFT: #bebebe 1px solid; BORDER-BOTTOM: #bebebe 1px solid" cellspacing="0" cellpadding="2" width="100%" border="0">
                </HeaderTemplate>
                <ItemTemplate>
                    <tr style="CURSOR: hand" onmouseover="style.backgroundColor='#fef5de';" onmouseout="style.backgroundColor='#FFFFFF'">
                        <td align="center" style="width:20;">
                        <asp:HiddenField ID="ObservationCategoryID" runat="server" Value='<%#Eval("observationCategoryID")%>' />                        
                            <asp:Label ID="LabelMark" runat="server"  Visible="false" ForeColor="Blue" Text=">"></asp:Label>
                        <td>
                        <td style="width:210;">
                            <asp:LinkButton ID="Description" CommandArgument='<%#Eval("observationCategoryID")%>' runat="server" Text='<%#Eval("description")%>'></asp:LinkButton>
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
    <!------------>    
    </td>
    
    <td style="width:10px;"></td>
    
    <td style="width: 530;" valign="top" align="left">
    <!------------>    
        <asp:Panel ID="Panel1" runat="server" Width="520px">
        
        <asp:Repeater ID="RepeaterQuestion" runat="server" OnItemDataBound="RepeaterQuestion_ItemDataBound">
            <HeaderTemplate>
                <table cellspacing="1" cellpadding="1" style="width:530px;">
            </HeaderTemplate>
            <ItemTemplate>            
                <tr>
                    <td style='width:120px; background-color: <%#Eval("rowColor")%>' align="center">
                        <asp:RadioButtonList ID="RadioButtonListYesNo" runat="server" RepeatDirection="Horizontal"
                            RepeatLayout="Flow">
                            <asp:ListItem Value="0" Text="Yes"></asp:ListItem>
                            <asp:ListItem Value="1" Text="No"></asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                    <td style='width:8px;background-color: <%#Eval("rowColor")%>'></td>
                    <td style='width:380px; background-color: <%#Eval("rowColor")%>'  align="left">
                        <asp:HiddenField ID="CategoryID" runat="server" Value='<%#Eval("observationCategoryID")%>' />
                        <asp:HiddenField ID="QuestionID" runat="server" Value='<%#Eval("observationQuestionID")%>' />                   
                        <asp:HiddenField ID="AnswerType" runat="server" Value='<%#Eval("answerType")%>' />                   
                        <asp:HiddenField ID="YesNoNA" runat="server" Value='<%#Eval("yesNoNA")%>' />                   
                        <asp:Label ID="description" runat="server" Text='<%#Eval("description")%>'></asp:Label>
                    </td>
                </tr>
            </ItemTemplate>
            <FooterTemplate>
            </table>
            </FooterTemplate>
        </asp:Repeater>   
        
        <table cellspacing="1" cellpadding="1" style="width:530px;">
        <tr>
        <td style="width:530px; background-color:#FEF5DE;">
        <asp:Label ID="Label100" runat="server" Text="Child was observed "></asp:Label>
            <asp:TextBox ID="TextBoxObservedTimes" runat="server" Width="30px"></asp:TextBox>
            <asp:Label ID="Label2" runat="server" Text="times over"></asp:Label>
            <asp:TextBox ID="TextBoxOverDays" runat="server" Width="30px"></asp:TextBox>
            <asp:Label ID="Label3" runat="server" Text="days (approximately"></asp:Label>
            <asp:TextBox ID="TextBoxMinutesTotal" runat="server" Width="30px"></asp:TextBox>
            <asp:Label ID="Label4" runat="server" Text="minutes total)."></asp:Label>
        </td>
        </tr>
        </table>
        <asp:Repeater ID="RepeaterQuestion2" runat="server" OnItemDataBound="RepeaterQuestion2_ItemDataBound">
            <HeaderTemplate>
                <table cellspacing="1" cellpadding="1" style="width:530px;">
            </HeaderTemplate>
            <ItemTemplate>            
                <tr>
                    <td style='width:130px; background-color: <%#Eval("rowColor")%>' align="center">
                        <asp:RadioButtonList ID="RadioButtonListYesNo2" runat="server" RepeatDirection="Horizontal"
                            RepeatLayout="Flow">
                            <asp:ListItem Value="0" Text="Yes"></asp:ListItem>
                            <asp:ListItem Value="1" Text="No"></asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                    <td style='width:8px;background-color: <%#Eval("rowColor")%>'></td>
                    <td style='width:380px; background-color: <%#Eval("rowColor")%>'  align="left">
                        <asp:HiddenField ID="CategoryID2" runat="server" Value='<%#Eval("observationCategoryID")%>' />
                        <asp:HiddenField ID="QuestionID2" runat="server" Value='<%#Eval("observationQuestionID")%>' />                   
                        <asp:HiddenField ID="AnswerType2" runat="server" Value='<%#Eval("answerType")%>' />                   
                        <asp:HiddenField ID="YesNoNA2" runat="server" Value='<%#Eval("yesNoNA")%>' />                   
                        <asp:Label ID="description2" runat="server" Text='<%#Eval("description")%>'></asp:Label>
                    </td>
                </tr>
            </ItemTemplate>
            <FooterTemplate>
            </table>
            </FooterTemplate>
        </asp:Repeater>   
              
        </asp:Panel>
        
        <asp:Panel ID="Panel2" runat="server" Width="500px">
            <asp:Repeater ID="RepeaterNotes" runat="server">
                <HeaderTemplate>
                    <table cellspacing="2" cellpadding="2" width="100%" border="0">
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:Panel ID="NotesPanel" runat="server">
                    <tr>
                        <td align="center" style="width: 500;">
                            <asp:HiddenField ID="observationCategoryID" runat="server" Value='<%#Eval("observationCategoryID")%>' />
                            <asp:HiddenField ID="observationQuestionID" runat="server" Value='<%#Eval("observationQuestionID")%>' />                                               
                            <asp:Label ID="Description" runat="server" Text='<%#Eval("description")%>'></asp:Label>
                            <asp:TextBox ID="TextBoxNote" runat="server" Height="100px" TextMode="MultiLine" Width="480px" Text='<%#Eval("note")%>'></asp:TextBox>
                       </td>                       
                    </tr>
                    </asp:Panel>
                </ItemTemplate>
                <FooterTemplate>
                    </table>
                </FooterTemplate>
            </asp:Repeater>        
        </asp:Panel>        
    <!------------>        
    </td>
</tr>

<tr><td colspan="3" style="height: 10px;"></td></tr>

<tr>
    <td colspan="2"></td>

    <td align="center">
        <asp:Button ID="ButtonSave" runat="server" Text="Save" OnClick="ButtonSave_Click"/>
        <asp:Button ID="ButtonClear" runat="server" Text="Clear" OnClick="ButtonClear_Click" />
        <asp:Button ID="ButtonBack" runat="server" Text="Cancel" OnClick="ButtonBack_Click"/>
    </td>
</tr>

</table>
</asp:Content>
