<%@ Page Language="C#" EnableTheming="true" Theme="Default" AutoEventWireup="true" CodeBehind="QuestionNote.aspx.cs" Inherits="BDI2Web.Assessment.QuestionNote" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Assessment Note</title>
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
        <div style="z-index: 101; left: 19px; width: 400px; position: absolute; top: 13px; height: 100px">
		<table width="100%" border="0" cellpadding="2" cellspacing="0">
            <tr valign="bottom">
                <td align="left" class="columnHeaders" colspan="4" style="width: 390px; font-weight: bold;
                    font-size: 11px; font-family: verdana; background-color: #105fad"
                    valign="middle">
                    <asp:Label ID="LabelTitle" runat="server" Text="Note:" ForeColor="White"></asp:Label></td>                 
            </tr>
			<tr valign="bottom">
				<td align="center" >
                    <asp:TextBox ID="TextBoxNote" runat="server" Height="54px" Width="391px" TextMode="MultiLine"></asp:TextBox></td>
			</tr>
			<tr><td style="width: 390px; height: 8px;">
                <asp:Label ID="LabelQuestionID" runat="server" Visible="False"></asp:Label></td></tr>          
                
            <tr>
                <td>
                    <asp:Panel runat="server" ID="PanelProgramNotes" Visible="false">
                    <table  width="100%" border="0" cellpadding="2" cellspacing="0">
                        <tr>
                            <td style="font-weight: bold;
                                font-size: 11px; font-family: verdana;">Program Note 2:&nbsp;
                                <asp:DropDownList ID="DropDownProgramNote2" runat="server">
                                </asp:DropDownList>
                            </td>
                        </tr>                
                        <tr>
                            <td style="font-weight: bold;
                                font-size: 11px; font-family: verdana;">Program Note 3:&nbsp;
                                <asp:DropDownList ID="DropDownProgramNote3" runat="server">
                                </asp:DropDownList>
                            </td>
                        </tr>  
                        <tr>
                            <td  style="font-weight: bold;
                                font-size: 11px; font-family: verdana;">Program Note 4:&nbsp;
                                <asp:DropDownList ID="DropDownProgramNote4" runat="server">
                                </asp:DropDownList>
                            </td>
                        </tr>  
                        <tr>
                            <td  style="font-weight: bold;
                                font-size: 11px; font-family: verdana;">Program Note 5:&nbsp;
                                <asp:DropDownList ID="DropDownProgramNote5" runat="server">
                                </asp:DropDownList>
                            </td>
                        </tr>                                          
                    </table>
                    </asp:Panel>                
                </td>            
            </tr>                
                
			<tr>
				<td style="width: 390px;" align="center">        
                    <asp:Button ID="ButtonSave" runat="server" Text="Save" OnClick="ButtonSave_Click" />
                    <asp:Button ID="ButtonCancel" runat="server" Text="Cancel" OnClick="ButtonCancel_Click" OnClientClick="CloseWindow()" />
                 </td>
   			</tr>            
        </table>    
            <asp:Literal ID="LiteralScript" runat="server"></asp:Literal></div>
    </form>
</body>
</html>
