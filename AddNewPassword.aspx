<%@ Page Language="C#" MasterPageFile="~/Intro.Master" AutoEventWireup="true" CodeBehind="AddNewPassword.aspx.cs" Inherits="BDI2Web.AddNewPassword" Title="Welcome to BDI-2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="IntroContentPlaceHolder" runat="server">
    
    <div id="AddNewPasswordPage" runat="server" visible="false">
    <br />
    <asp:Label ID="Label1" runat="server" Text="Welcome"/><br />
    <asp:Label ID="lblUserName" runat="server" Text=""/><br />
   
    <table style="width: 344px;" cellpadding="0" cellspacing="0" border="0px">
        <tr>
            <td style="padding:2px;height:30px;" valign="middle" colspan="2">
             <span  class="text">Please create a new password to activate your account.</span>
            <br />
            </td>
        </tr>
         <tr>
            <td  style="padding:6px;width:125px">
                <input style="display:none" type="password" name="fakepasswordremembered"/>
                <asp:TextBox ID="txtPassword" runat="server" Height="15px" Width="120px"  TextMode="Password" placeholder="Password"></asp:TextBox>
            </td>
            <td>    
                <asp:RequiredFieldValidator ID="PasswordRFV" ControlToValidate="txtPassword" CssClass="errorMessage" ErrorMessage="Password is required" runat="server"  SetFocusOnError="True" Display="Dynamic"/>
                 <asp:CustomValidator runat="server" id="Passwordvalid" controltovalidate="txtPassword" onservervalidate="Passwordvalidation" errormessage="Password does not meet standards." 
           CssClass="errorMessage" SetFocusOnError="True" Display="Dynamic"/>
            </td>
          </tr>
         <tr>
            <td style="padding:6px;width:125px">
                <asp:TextBox ID="txtConfPassword" runat="server" Width="120px" Height="15px" TextMode="Password" placeholder="Re-enter Password"></asp:TextBox>          
            </td>
            <td> 
            <asp:CompareValidator ID="ComparePasswordRFV" runat="server" ControlToValidate="txtConfPassword" CssClass="errorMessage" ControlToCompare="txtPassword" ErrorMessage="Passwords do not match." 
            ToolTip="Password must be the same" SetFocusOnError="True" Display="Dynamic"/>
             <asp:RequiredFieldValidator ID="ConfPasswordRFV" ControlToValidate="txtConfPassword" CssClass="errorMessage" ErrorMessage="Passwords do not match." runat="server" 
             SetFocusOnError="True" Display="Dynamic"/>
            </td>
        </tr>      
        <tr>
          <td style="padding:4px;height:25px;" colspan="2">
                <asp:Button ID="btnSave" runat="server" Text="Save" Height="20px" 
                    onclick="btnSave_Click"/>
            </td>
        </tr>
    </table>
    
    </div>
  
    <asp:Label ID="lblError" runat="server" ForeColor="Red" Text="" Width="391px"></asp:Label>
</asp:Content>
