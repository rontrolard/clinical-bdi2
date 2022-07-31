<%@ Page Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="ErrorList.aspx.cs" Inherits="BDI2Web.ErrorLog.ErrorList" Title="Error Log" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MCP" runat="server">        
    <table border="0">
        <tr align="left">
            <td colspan="2">Enter date range and/or error message to search for errors.  Leave all blank to list all errors by date descending.</td>
        </tr>
        <tr >
            <td align="right">Begin Date:</td>
            <td align="left"><asp:TextBox ID="BeginDateTextBox" runat="server"/> <asp:CompareValidator ID="cvBeginDate" ControlToValidate="BeginDateTextBox" Text="Begin Date must have MM/DD/YYYY format." Display="dynamic" Operator="datatypecheck" Type="date" runat="server" /> </td>                
        </tr>
        <tr>
            <td align="right">End Date:</td>
            <td align="left"><asp:TextBox ID="EndDateTextBox" runat="server" /> <asp:CompareValidator ID="CompareValidator1" ControlToValidate="EndDateTextBox" Text="End Date must have MM/DD/YYYY format." Display="dynamic" Operator="datatypecheck" Type="date" runat="server" /></td>
        </tr>
        <tr >
            <td align="right">Error Message:</td>
            <td align="left"><asp:TextBox ID="ErrorMessageTextBox" Width="350" MaxLength="45" runat="server" /></td>
        </tr>
        <tr >
            <td align="right">&nbsp;</td>
            <td align="left"><asp:Button ID="ResetSearchButton" Text="Reset" runat="server" UseSubmitBehavior="false" OnClick="ResetSearchButton_Click" />&nbsp;&nbsp;&nbsp;<asp:Button ID="Search" Text="Search" runat="server" OnClick="Search_Click" />&nbsp;&nbsp;<asp:CompareValidator ID="cvBeginLessThanEnd" ControlToValidate="BeginDateTextBox" ControlToCompare="EndDateTextBox" Display="dynamic" ErrorMessage="Begin date must be less than End Date." Operator="LessThan" runat="server" Type="date" /></td>
        </tr>
    </table>        
    <br /><br />
    <table>
        <tr>
            <td valign="top">
                <asp:DetailsView ID="dvErrorItem" HeaderText="Error Detail:" AutoGenerateRows="False" runat="server" GridLines="Both">
                    <Fields>
                        <asp:BoundField HeaderText="Date" DataField="TimeStamp" />
                        <asp:BoundField HeaderText="Message" DataField="Message" />
                        <asp:BoundField HeaderText="Stack Trace" HeaderStyle-VerticalAlign="top" DataField="FormattedMessage" />
                    </Fields>
                </asp:DetailsView>
                <asp:Label ID="NoRecordsFound" Text="No records found." Visible="False" runat="server" ForeColor="Red" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:GridView ID="gvErrorList" DataKeyNames="logid" AllowPaging="True" AutoGenerateColumns="False" runat="server" GridLines="Both" OnSelectedIndexChanged="gvErrorList_SelectedIndexChanged" OnPageIndexChanging="gvErrorList_PageIndexChanging">
                    <Columns>
                        <asp:BoundField HeaderText="Date" DataField="TimeStamp" SortExpression="TimeStamp">
                            <ItemStyle Width="15%" />
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="Message">
                            <ItemTemplate>
                            <asp:LinkButton ID="LinkButton1" CssClass="grid-link" CommandName="Select" Runat="Server">
                              <%# Eval("FormattedMessage")%>
                            </asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>                          
                    </Columns>                                              
                </asp:GridView>                    
            </td>
        </tr>
    </table>
</asp:Content>
