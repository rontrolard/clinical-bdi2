<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ProductOrder.ascx.cs" Inherits="Controls_ProductOrder" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server" />

<asp:MultiView ID="mvMain" runat="server" ActiveViewIndex="0">
    <asp:View ID="viewOrder" runat="server">
        <asp:FormView ID="FormView1" runat="server" DefaultMode="Insert" DataSourceID="dsNewOrder" DataKeyNames="OrderID" OnPreRender="FormView1_PreRender">
            <InsertItemTemplate>
                <!-- ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
                    Payment Information Selection
                 ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ -->
                <div class="<%=HeaderCssClass %>">Payment Information</div>
                <table>
                    <tr>
                        <td class="title" style="padding-right:10px;">Customer Name:</td>
                        <td>
                            <asp:Label ID="lblCustomerName" SkinID="RegularText" runat="server"/>
                        </td>
                    </tr>
                    <tr>
                        <td class="title" style="padding-right:10px;">Payment Type:</td>
                        <td>
                            <asp:Button ID="btnPaymentTypeCC" runat="server" Text="Credit Card" OnClick="btnPaymentTypeCC_Click" />&nbsp;
                            <asp:Button ID="btnPaymentTypePO" runat="server" Text="Purchase Order" OnClick="btnPaymentTypePO_Click" TabIndex="1" />
                        </td>
                    </tr>
                </table>
                <p></p>

                <div style="display: none;" >
                     <asp:TextBox runat="server" ID="hiddenTextBox1" />
                     <ajaxToolkit:CalendarExtender ID="hiddenTextBoxCE" runat="server" TargetControlID="hiddenTextBox1" />
                </div>
                <asp:UpdatePanel ID="upnlPaymentType" runat="server">
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnPaymentTypeCC" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnPaymentTypePO" EventName="Click" />
                    </Triggers>
                    <ContentTemplate>
                        <asp:MultiView ID="mvPaymentTypes" runat="server" ActiveViewIndex="0">
                            <asp:View ID="viewCC" runat="server">
                                <!-- *** Credit Card Information Section *** -->
                                <table cellpadding="1" cellspacing="0">
                                    <tr>
                                        <td class="title">Credit Card:
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidatorPaymentType" ControlToValidate="btnlstPaymentType" InitialValue="0" runat="server" Text="*"
                                                Display="Static" SetFocusOnError="true" ErrorMessage="A Credit Card type is required." ValidationGroup="ProductOrder"  />
                                        </td>
                                        <td>
                                            <asp:RadioButtonList ID="btnlstPaymentType" runat="server" RepeatDirection="Horizontal" TabIndex="2" SelectedValue='<%#Bind("PaymentType")%>'>
                                                <asp:ListItem Value="0" Selected="True">[None]</asp:ListItem>
                                                <asp:ListItem Value="1">Visa</asp:ListItem>
                                                <asp:ListItem Value="2">MasterCard</asp:ListItem>
                                                <asp:ListItem Value="3">American Express</asp:ListItem>
                                                <asp:ListItem Value="4">Diners Club</asp:ListItem>
                                                <asp:ListItem Value="5">Discover</asp:ListItem>
                                            </asp:RadioButtonList>                        
                                        </td>
                                    </tr>
                                </table>

                                <table cellpadding="1" cellspacing="1" >
                                    <tr>
                                        <td colspan="4">&nbsp;</td>
                                        <td class="smallText">(Exempt from state sales tax.)</td>
                                    </tr>
                                    <tr>
                                        <td class="text">Card&nbsp;Number:
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidatorCCNum" runat="server" ControlToValidate="txtCCNum" ErrorMessage="A Credit Card Number is Required" ValidationGroup="ProductOrder" 
                                                SetFocusOnError="true" Display="Static" Text="*">
                                            </asp:RequiredFieldValidator>
                                            <asp:RegularExpressionValidator ControlToValidate="txtCCNum" ErrorMessage="Invalid Credit Card Number" ValidationGroup="ProductOrder" ID="RegularExpressionValidatorCCNum" runat="server" 
                                                SetFocusOnError="true" Text="*" Display="Static" 
                                                ValidationExpression=" ^3(?:[47]\d([ -]?)\d{4}(?:\1\d{4}){2}|0[0-5]\d{11}|[68]\d{12})$|^4(?:\d\d\d)?([ -]?)\d{4}(?:\2\d{4}){2}$|^6011([ -]?)\d{4}(?:\3\d{4}){2}$|^5[1-5]\d\d([ -]?)\d{4}(?:\4\d{4}){2}$|^2014\d{11}$|^2149\d{11}$|^2131\d{11}$|^1800\d{11}$|^3\d{15}$">
                                            </asp:RegularExpressionValidator>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtCCNum" MaxLength="32" runat="server" Width="200px" TabIndex="3" Text='<%#Bind("CCNum")%>'/>
                                            <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" TargetControlID="txtCCNum" runat="server"
                                                FilterType="Custom, Numbers" ValidChars="-">
                                            </ajaxToolkit:FilteredTextBoxExtender>
                                        </td>
                                        <td rowspan="2" style="width:15px;">&nbsp;</td>
                                        <td class="text">Cert. no:</td>
                                        <td>
                                            <asp:TextBox ID="txtCCCertNo" MaxLength="50" runat="server" Width="200px" TabIndex="6"/>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="text">CC&nbsp;Exp&nbsp;Date:
                                            <asp:RequiredFieldValidator ControlToValidate="txtCCExpDate" ErrorMessage="A Credit Card Expiration Date is Required" ValidationGroup="ProductOrder" ID="RequiredFieldValidatorCCExpDate" runat="server"  
                                                SetFocusOnError="true" Display="Static" Text="*">
                                            </asp:RequiredFieldValidator>                        
                                            <asp:RegularExpressionValidator ControlToValidate="txtCCExpDate" ErrorMessage="Invalid Credit Card Expiration Date." ValidationGroup="ProductOrder" ID="RegularExpressionValidatorCCExpDate" runat="server" 
                                                SetFocusOnError="true" Text="*" Display="Static" 
                                                ValidationExpression="((0?[13578]|10|12)(-|\/)((0[0-9])|([12])([0-9]?)|(3[01]?))(-|\/)((\d{4})|(\d{2}))|(0?[2469]|11)(-|\/)((0[0-9])|([12])([0-9]?)|(3[0]?))(-|\/)((\d{4}|\d{2})))">
                                            </asp:RegularExpressionValidator>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtCCExpDate" MaxLength="10" runat="server" Width="180px" TabIndex="4" Text='<%#Bind("CCExpDate")%>'/>
                                            <asp:ImageButton runat="Server" ID="imgCCExpDate" ImageUrl="~/Images/Calendar.gif" AlternateText="Click to show calendar" ImageAlign="AbsMiddle" />
                                            <ajaxToolkit:CalendarExtender ID="calExndCCExpDate" runat="server" TargetControlID="txtCCExpDate" PopupButtonID="imgCCExpDate" Format="MM/dd/yyyy"/>
                                        </td>
                                        <td class="text">Cert.&nbsp;Exp&nbsp;Date:
                                            <asp:RegularExpressionValidator ControlToValidate="txtCCCertExpDate" ErrorMessage="Invalid Certification Expiration Date." ValidationGroup="ProductOrder" ID="RegularExpressionValidator2" runat="server" 
                                                SetFocusOnError="true" Text="*" Display="Static" 
                                                ValidationExpression="((0?[13578]|10|12)(-|\/)((0[0-9])|([12])([0-9]?)|(3[01]?))(-|\/)((\d{4})|(\d{2}))|(0?[2469]|11)(-|\/)((0[0-9])|([12])([0-9]?)|(3[0]?))(-|\/)((\d{4}|\d{2})))">
                                            </asp:RegularExpressionValidator>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtCCCertExpDate" MaxLength="10" runat="server" Width="180px" TabIndex="7"/>
                                            <asp:ImageButton runat="Server" ID="imgCCCertExpDate" ImageUrl="~/Images/Calendar.gif" AlternateText="Click to show calendar" ImageAlign="AbsMiddle" />
                                            <ajaxToolkit:CalendarExtender ID="calExndCCCertExpDate" runat="server" TargetControlID="txtCCCertExpDate" PopupButtonID="imgCCCertExpDate" Format="MM/dd/yyyy" />
                                        </td>
                                    </tr>                    
                                </table>
                            
                            </asp:View>
                            <asp:View ID="viewPO" runat="server">
                                <!-- *** Puchase Order Section *** -->
                                <div class="title">Purchase Order:</div>
                                <table cellpadding="1" cellspacing="1">
                                    <tr>
                                        <td class="text">Purchase Order Number:
                                            <asp:RequiredFieldValidator ControlToValidate="txtPONumber" ErrorMessage="A Purchase Order Number is Required" ValidationGroup="ProductOrder" ID="RequiredFieldValidatorPONumber" runat="server"  
                                                SetFocusOnError="true" Display="Static" Text="*" EnableClientScript="true">
                                            </asp:RequiredFieldValidator>                                               
                                        </td>
                                        <td><asp:TextBox ID="txtPONumber" MaxLength="50" runat="server" Width="200px" TabIndex="5" Text='<%#Bind("PONum")%>'/></td>
                                    </tr>
                                    <tr>
                                        <td class="text">Cert. no:</td>
                                        <td>
                                            <asp:TextBox ID="txtPOCertNo" MaxLength="50" runat="server" Width="200px" TabIndex="6"/>
                                            <span class="smallText">(Exempt from state sales tax.)</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="text">Cert. Expiration Date:
                                            <asp:RegularExpressionValidator ControlToValidate="txtPOCertExpDate" ErrorMessage="Invalid Cert Expiration Date." ValidationGroup="ProductOrder" ID="RegularExpressionValidatorPOCertExpDate" runat="server" 
                                                SetFocusOnError="true" Text="*" Display="Static" 
                                                ValidationExpression="((0?[13578]|10|12)(-|\/)((0[0-9])|([12])([0-9]?)|(3[01]?))(-|\/)((\d{4})|(\d{2}))|(0?[2469]|11)(-|\/)((0[0-9])|([12])([0-9]?)|(3[0]?))(-|\/)((\d{4}|\d{2})))">
                                            </asp:RegularExpressionValidator>                       
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtPOCertExpDate" MaxLength="10" runat="server" Width="184px" TabIndex="7"/>
                                            <asp:ImageButton runat="Server" ID="imgPOCertExpDate" ImageUrl="~/Images/Calendar.gif" AlternateText="Click to show calendar" />
                                            <ajaxToolkit:CalendarExtender ID="calExndPOCertExpDate" runat="server" TargetControlID="txtPOCertExpDate" PopupButtonID="imgPOCertExpDate" Format="MM/dd/yyyy"/>
                                        </td>
                                    </tr>
                                </table>
                            </asp:View>
                        </asp:MultiView>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <p></p>

                <!-- ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
                    Charge To Information Selection
                 ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ -->
                <div class="<%=HeaderCssClass %>">Charge To</div>

                <table>
                    <tr>
                        <td>
                            <!-- BillTo:  -->
                            <table cellpadding="1" cellspacing="1">
                                <tr>
                                    <td colspan="2" class="title">Billing Address</td>
                                </tr>              
                                <tr>
                                    <td class="text">Name:
                                        <asp:RequiredFieldValidator ControlToValidate="txtBillToName" ErrorMessage="A Bill To Name is Required" ValidationGroup="ProductOrder" ID="RequiredFieldValidator1" runat="server"  
                                            SetFocusOnError="true" Display="Static" Text="*">
                                        </asp:RequiredFieldValidator>                                                                   
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtBillToName" MaxLength="32" runat="server" Width="200px" TabIndex="8" Text='<%#Bind("BillToName")%>'/>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="text">Position:</td>
                                    <td>
                                        <asp:TextBox ID="txtBillToPosition" MaxLength="100" runat="server" Width="200px" TabIndex="9" Text='<%#Bind("BillToPosition")%>'/>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="text">Billing Address:
                                        <asp:RequiredFieldValidator ControlToValidate="txtBillToAddress" ErrorMessage="A Bill To Address is Required" ValidationGroup="ProductOrder" ID="RequiredFieldValidatorBillToAddress" runat="server"  
                                            SetFocusOnError="true" Display="Static" Text="*">
                                        </asp:RequiredFieldValidator>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtBillToAddress" MaxLength="100" runat="server" Width="200px" TabIndex="10" Text='<%#Bind("BillToAddress")%>'/>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="text">City:
                                        <asp:RequiredFieldValidator ControlToValidate="txtBillToCity" ErrorMessage="A Bill To City is Required" ValidationGroup="ProductOrder" ID="RequiredFieldValidatorBillToCity" runat="server"  
                                            SetFocusOnError="true" Display="Static" Text="*">
                                        </asp:RequiredFieldValidator>                    
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtBillToCity" MaxLength="50" runat="server" Width="200px" TabIndex="11" Text='<%#Bind("BillToCity")%>'/>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="text">State:
                                        <asp:RequiredFieldValidator ControlToValidate="ddlBillToState" ErrorMessage="A Bill To State is Required" ValidationGroup="ProductOrder" ID="RequiredFieldValidatorBillToState" runat="server"  
                                            SetFocusOnError="true" Display="Static" Text="*" InitialValue="">
                                        </asp:RequiredFieldValidator>                    
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlBillToState" runat="server" DataSourceID="dsStates" DataTextField="Abbreviation" DataValueField="Abbreviation" TabIndex="12" Text='<%#Bind("BillToState")%>' />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="text">Zip Code:
                                        <asp:RequiredFieldValidator ControlToValidate="txtBillToZip" ErrorMessage="A Bill To Zipcode is Required" ValidationGroup="ProductOrder" ID="RequiredFieldValidatorBillToZip" runat="server"  
                                            SetFocusOnError="true" Display="Static" Text="*">
                                        </asp:RequiredFieldValidator>                   
                                        <asp:RegularExpressionValidator ControlToValidate="txtBillToZip" ErrorMessage="Invalid Bill To Zipcode." ValidationGroup="ProductOrder" ID="RegularExpressionValidatorBillToZip" runat="server" 
                                            SetFocusOnError="true" Text="*" Display="Static" 
                                            ValidationExpression="^((\d{5}-\d{4})|(\d{5})|([AaBbCcEeGgHhJjKkLlMmNnPpRrSsTtVvXxYy]\d[A-Za-z]\s?\d[A-Za-z]\d))$">
                                        </asp:RegularExpressionValidator>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtBillToZip" MaxLength="10" runat="server" Width="200px" TabIndex="13" Text='<%#Bind("BillToZip")%>'/>
                                        <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" TargetControlID="txtBillToZip" runat="server"
                                            FilterType="Custom, Numbers" ValidChars="-">
                                        </ajaxToolkit:FilteredTextBoxExtender>

                                    </td>
                                </tr>
                                <tr>
                                    <td class="text">Phone:
                                        <asp:RequiredFieldValidator ControlToValidate="txtBillToPhone" ErrorMessage="A Bill To Phone Number is Required" ValidationGroup="ProductOrder" ID="RequiredFieldValidatorBillToPhone" runat="server"  
                                            SetFocusOnError="true" Display="Static" Text="*">
                                        </asp:RequiredFieldValidator>                           
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtBillToPhone" runat="server" MaxLength="20" Width="200px" TabIndex="14" Text='<%#Bind("BillToPhone")%>' />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="text">Fax:</td>
                                    <td>
                                        <asp:TextBox ID="txtBillToFax" runat="server" MaxLength="20" Width="200px" TabIndex="14" Text='<%#Bind("BillToFax")%>' />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="text">E-mail Address:
                                        <asp:RequiredFieldValidator ControlToValidate="txtBillToEmail" ErrorMessage="A Bill To E-mail Address is Required" ValidationGroup="ProductOrder" ID="RequiredFieldValidatorBillToEmail" runat="server"  
                                            SetFocusOnError="true" Display="Static" Text="*">
                                        </asp:RequiredFieldValidator>                                      
                                        <asp:RegularExpressionValidator ControlToValidate="txtBillToEmail" ErrorMessage="Invalid Bill To E-mail Address Date." ValidationGroup="ProductOrder" ID="RegularExpressionValidatorBillToEmail" runat="server" 
                                            SetFocusOnError="true" Text="*" Display="Static" 
                                            ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*">
                                        </asp:RegularExpressionValidator>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtBillToEmail" MaxLength="75" runat="server" Width="200px" TabIndex="14" Text='<%#Bind("BillToEmail")%>'/>
                                    </td>
                                </tr>
                            </table>        
                        </td>
                        <td style="border-left:1px solid;">&nbsp;</td>
                        <td>
                            <!-- ShipTo:  -->
                            <asp:UpdatePanel ID="upnlShippingAddress" runat="server">
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnShipSameAsBill" EventName="Click" />
                                </Triggers>
                                <ContentTemplate>
                                    <table cellpadding="1" cellspacing="1">
                                        <tr>
                                            <td colspan="2" class="title">Shipping Address</td>
                                        </tr>                        
                                        <tr>
                                            <td class="text">Name:</td>
                                            <td>
                                                <asp:TextBox ID="txtShipToName" MaxLength="32" runat="server" Width="200px" TabIndex="16" Text='<%#Bind("ShipToName")%>'/>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="text">Position:</td>
                                            <td>
                                                <asp:TextBox ID="txtShipToPosition" MaxLength="100" runat="server" Width="200px" TabIndex="17" Text='<%#Bind("ShipToPosition")%>'/>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="text">Shipping Address:</td>
                                            <td>
                                                <asp:TextBox ID="txtShipToAddress" MaxLength="100" runat="server" Width="200px" TabIndex="18" Text='<%#Bind("ShipToAddress")%>'/>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="text">City:</td>
                                            <td>
                                                <asp:TextBox ID="txtShipToCity" MaxLength="50" runat="server" Width="200px" TabIndex="19" Text='<%#Bind("ShipToCity")%>'/>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="text">State:</td>
                                            <td>
                                                <asp:DropDownList ID="ddlShipToState" runat="server" DataSourceID="dsStates" DataTextField="Abbreviation" DataValueField="Rate" TabIndex="20" Text='<%#Bind("ShipToState")%>' />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="text">Zip Code:
                                                <asp:RegularExpressionValidator ControlToValidate="txtShipToZip" ErrorMessage="Invalid Ship To Zipcode." ValidationGroup="ProductOrder" ID="RegularExpressionValidatorShipToZip" runat="server" 
                                                    SetFocusOnError="true" Text="*" Display="Static" 
                                                    ValidationExpression="^((\d{5}-\d{4})|(\d{5})|([AaBbCcEeGgHhJjKkLlMmNnPpRrSsTtVvXxYy]\d[A-Za-z]\s?\d[A-Za-z]\d))$">
                                                </asp:RegularExpressionValidator>                            
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtShipToZip" MaxLength="10" runat="server" Width="200px" TabIndex="21" Text='<%#Bind("ShipToZip")%>'/>
                                                <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" TargetControlID="txtShipToZip" runat="server"
                                                    FilterType="Custom, Numbers" ValidChars="-">
                                                </ajaxToolkit:FilteredTextBoxExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="text">Phone:</td>
                                            <td>
                                                <asp:TextBox ID="txtShipToPhone" runat="server" MaxLength="20" Width="200px" TabIndex="22" Text='<%#Bind("ShipToPhone")%>' />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="text">Fax:</td>
                                            <td>
                                                <asp:TextBox ID="txtShipToFax" runat="server" MaxLength="20" Width="200px" TabIndex="23" Text='<%#Bind("ShipToFax")%>' />
                                            </td>
                                        </tr>

                                        <tr>
                                            <td class="text">E-mail Address:
                                                <asp:RegularExpressionValidator ControlToValidate="txtShipToEmail" ErrorMessage="Invalid Ship To E-mail Addresss." ValidationGroup="ProductOrder" ID="RegularExpressionValidatorShipToEmail" runat="server" 
                                                    SetFocusOnError="true" Text="*" Display="Static" 
                                                    ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*">
                                                </asp:RegularExpressionValidator>                            
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtShipToEmail" MaxLength="100" runat="server" Width="200px" TabIndex="24" Text='<%#Bind("ShipToEmail")%>'/>
                                            </td>
                                        </tr>
                                    </table>        
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3" style="text-align:center; padding-top:10px;" >
                            <asp:Button ID="btnShipSameAsBill" runat="server" Text="Use the Billing Addresss for Shipping also" OnClick="btnShipSameAsBill_Click" TabIndex="15" />
                        </td>
                    </tr>
                </table>
            </InsertItemTemplate>
        </asp:FormView>

        <p></p>

        <!-- ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
            Product Information Selection
         ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ -->
        <asp:Repeater ID="rptProducts" runat="server" DataSourceID="dsProducts" OnItemDataBound="rptProducts_ItemDataBound">
            <HeaderTemplate>
                <table id="ProductTable" cellspacing="0">
                    <thead class="<%=HeaderCssClass %>">
                        <tr>
                            <th>Qty</th>
                            <th>Code&nbsp;#</th>
                            <th>Item and Packaging</th>
                            <th>Price</th>
                            <th>Total</th>
                        </tr>
                    </thead>
                    <tbody>
            </HeaderTemplate>
            <ItemTemplate>
                <tr>
                    <td style="text-align:left; vertical-align:top; white-space:nowrap;">
                        <asp:DropDownList ID="ddlProductQty" runat="server" DataSourceID="dsQty" DataTextField="Value" DataValueField="Value" AutoPostBack="true" OnSelectedIndexChanged="ddlProductQty_SelectedIndexChanged" TabIndex="23" />
                        <asp:TextBox ID="txtProductQty" runat="server" TabIndex="23" Width="45px" Text="0" Visible="false" MaxLength="5" OnTextChanged="txtProductQty_TextChanged" ToolTip="Press [Enter] to Calculate Item Total" />
                        <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" TargetControlID="txtProductQty" runat="server"
                            FilterType="Numbers" ValidChars="-">
                        </ajaxToolkit:FilteredTextBoxExtender>
                        <asp:RequiredFieldValidator ControlToValidate="txtProductQty" ID="RequiredFieldValidatorProductQty" runat="server" Text="*" ValidationGroup="ProductOrder" SetFocusOnError="true"
                            EnableClientScript="true" ErrorMessage="A Qty is Required for all of the Products" Visible="false" Display="Static"/>
                        <asp:RangeValidator ControlToValidate="txtProductQty" ID="RangeValidatorProductQty" runat="server" Text="*" ValidationGroup="ProductOrder" SetFocusOnError="true" 
                            EnableClientScript="True" ErrorMessage="Invalid Product Qty" Visible="false" Display="Static" MinimumValue="0" MaximumValue="99999"/>
                    </td>
                    <td style="text-align:center; vertical-align:top; white-space:nowrap;">
                        <asp:Literal ID="litCodeNum" Text='<%#Eval("CodeNum")%>' runat="server"/>
                        <asp:HiddenField ID="hfProductID" runat="server" Value='<%#Eval("ProductID")%>' />
                    </td>
                    <td style="width:450px;">
                        <asp:HiddenField ID="hfProductName" runat="server" Value='<%#Eval("ProductName")%>' />
                        <div class="title" runat="server" id="divProductName"><%#Eval("ProductName")%></div>                
                        <%#Eval("Description")%>
                        <div class="smallText"><%#Eval("SubDescription")%></div>
                    </td>
                    <td style="text-align:right; vertical-align:top;">
                        <%#Eval("Price","{0:C}")%>
                        <asp:HiddenField ID="hfPrice" runat="server" Value='<%#Eval("Price")%>' />
                    </td>
                    <td style="text-align:center; vertical-align:top; width:125px;">
                        <asp:UpdatePanel ID="upnlCalculateRowTotal" runat="server">
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddlProductQty" EventName="SelectedIndexChanged" />
                                <asp:AsyncPostBackTrigger ControlID="txtProductQty" EventName="TextChanged" />
                            </Triggers>
                            <ContentTemplate>
                                <asp:Label ID="lblTotal" runat="server" Text="------" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
            </ItemTemplate>
            <FooterTemplate>
                </tbody>
                </table>
            </FooterTemplate>
        </asp:Repeater>

        <!-- ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
            Product Totals Selection
         ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ -->
        <table cellpadding="0" cellspacing="1">
            <tr>
                <td class="smallText" style="width:450px; padding-right:50px;">
                    *Shipping is prepaid and added to the invoice along with a handling charge (estimate 10% 
                    for ground shipping; 20% for air shipping to AK, HI, and rush orders; and 25% for international 
                    shipments).  For orders under $100, estimate $10.00 for ground shipping and $25.00 for air shipping 
                    transportation available for AK and HI upon request – please contact Customer Service at 800.323.9540 
                    for assistance.    
                </td>
                <td>        
                    <asp:UpdatePanel ID="upnlTotals" runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional" >
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnRecalculateTotal" EventName="Click" />
                        </Triggers>
                        <ContentTemplate>
                            <table>
                                <tbody style="text-align:right;">
                                    <tr>
                                        <td class="title">Shipping Method:</td>
                                        <td rowspan="5" style="width:5px;">&nbsp;</td>
                                        <td><asp:DropDownList ID="ddlShippingMethod" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlShippingMethod_SelectedIndexChanged">
                                                <asp:ListItem Value="Ground" Text="Ground"></asp:ListItem>
                                                <asp:ListItem Value="Air" Text="Air"></asp:ListItem>
                                                <asp:ListItem Value="Express" Text="Express"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="subtitle" >Subtotal:</td>
                                        <td><asp:Label ID="lblSubTotal" runat="server"/></td>
                                    </tr>
                                    <tr>
                                        <td class="subtitle" >State Sales Tax:</td>
                                        <td><asp:label ID="lblTax" runat="server" SkinID="Title"/></td>
                                    </tr>
                                    <tr>
                                        <td class="subtitle"  style="border-bottom:1px solid black;">Shipping and Handling*:</td>
                                        <td style="border-bottom:1px solid black;">
                                            <asp:Label ID="lblSH" runat="server" SkinID="Title"/>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="subtitle" >Order Total:</td>
                                        <td><asp:Label ID="lblOrderTotal" runat="server" SkinID="Title"/></td>
                                    </tr>
                                </tbody>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
        </table>

        <div style="text-align:right; padding:10px 20px; vertical-align:top;">
            <asp:Button ID="btnRecalculateTotal" runat="server" Text="Recalculate" TabIndex="26" OnClick="btnRecalculateTotal_Click" />
            <asp:Button ID="btnSubmitOrder" runat="server" Text="Submit Order" TabIndex="27" ValidationGroup="ProductOrder" OnClick="btnSubmitOrder_Click" />
        </div>

    </asp:View>
    <asp:View ID="viewConfirm" runat="server" EnableViewState="false">
        <!-- ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
            Payment Information Selection
         ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ -->
        <div class="<%=HeaderCssClass %>">Payment Information</div>
        <table cellpadding="0" cellspacing="1">
            <tr>
                <td class="title" style="padding-right:10px; text-align:right;">Customer Name:</td>
                <td><asp:Label ID="lblConfirmCustomerName" SkinID="RegularText" runat="server"/></td>
                <td rowspan="4" style="width:50px;">&nbsp;</td>
                <td class="title" style="padding-right:10px; text-align:right;">PO Number:</td>
                <td><asp:Label ID="lblConfirmPONumber" SkinID="RegularText" runat="server"/></td>
            </tr>
            <tr>
                <td class="title" style="padding-right:10px; text-align:right;">Payment Type:</td>
                <td><asp:Label ID="lblConfirmPaymentType" SkinID="RegularText" runat="server"/></td>
                <td class="title" style="padding-right:10px; text-align:right;">Tax Cert No:</td>
                <td><asp:Label ID="lblConfirmCertNo" SkinID="RegularText" runat="server"/></td>
            </tr>
            <tr>
                <td class="title" style="padding-right:10px; text-align:right;">Credit Card:</td>
                <td><asp:Label ID="lblConfirmCC" SkinID="RegularText" runat="server"/></td>
                <td class="title" style="padding-right:10px; text-align:right;">Tax Cert Exp Date:</td>
                <td><asp:Label ID="lblConfirmCertExpDate" SkinID="RegularText" runat="server"/></td>
            </tr>
            <tr>
                <td class="title" style="padding-right:10px; text-align:right;">Credit Card #:</td>
                <td><asp:Label ID="lblConfirmCCNum" SkinID="RegularText" runat="server"/></td>
                <td class="title" style="padding-right:10px; text-align:right;">Card Exp Date:</td>
                <td><asp:Label ID="lblConfirmCCExpDate" SkinID="RegularText" runat="server"/></td>
            </tr>
        </table>
        <p></p>
        
        <!-- ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
            Charge To Information Selection
         ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ -->       
        <div class="<%=HeaderCssClass %>">Charge To</div>        
        <table cellpadding="1" cellspacing="1">
            <tr>
                <td class="title" style="border-bottom:solid 1px black;">Billing Address:</td>
                <td rowspan="2" style="width:120px;">&nbsp;</td>
                <td class="title" style="border-bottom:solid 1px black;">Shipping Address:</td>
            </tr>
            <tr>
                <td class="textNoPadding">
                    <asp:Label ID="lblBillToName" runat="server" SkinID="RegularTextNoPadding" /><br />
                    <asp:Label ID="lblBillToPosition" runat="server" SkinID="RegularTextNoPadding" /><br />
                    <asp:Label ID="lblBillToAddress" runat="server" SkinID="RegularTextNoPadding" /><br />
                    <asp:Label ID="lblBillToCity" runat="server" SkinID="RegularTextNoPadding" />,&nbsp;
                    <asp:Label ID="lblBillToState" runat="server" SkinID="RegularTextNoPadding" />&nbsp;
                    <asp:Label ID="lblBillToZip" runat="server" SkinID="RegularTextNoPadding" /><br />
                    Phone:&nbsp;<asp:Label ID="lblBillToPhone" runat="server" SkinID="RegularTextNoPadding" />
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    Fax:&nbsp;<asp:Label ID="lblBillToFax" runat="server" SkinID="RegularTextNoPadding" /><br />
                    Email:&nbsp;<asp:Label ID="lblBillToEmail" runat="server" SkinID="RegularTextNoPadding" /><br />
                </td>
                <td class="textNoPadding">
                    <asp:Label ID="lblShipToName" runat="server" SkinID="RegularTextNoPadding" /><br />
                    <asp:Label ID="lblShipToPosition" runat="server" SkinID="RegularTextNoPadding" /><br />
                    <asp:Label ID="lblShipToAddress" runat="server" SkinID="RegularTextNoPadding" /><br />
                    <asp:Label ID="lblShipToCity" runat="server" SkinID="RegularTextNoPadding" />,&nbsp;
                    <asp:Label ID="lblShipToState" runat="server" SkinID="RegularTextNoPadding" />&nbsp;
                    <asp:Label ID="lblShipToZip" runat="server" SkinID="RegularTextNoPadding" /><br />
                    Phone:&nbsp;<asp:Label ID="lblShipToPhone" runat="server" SkinID="RegularTextNoPadding" />
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    Fax:&nbsp;<asp:Label ID="lblShipToFax" runat="server" SkinID="RegularTextNoPadding" /><br />
                    Email:&nbsp;<asp:Label ID="lblShipToEmail" runat="server" SkinID="RegularTextNoPadding" /><br />
                </td>
            </tr>
        </table>
        <p></p>
        
        <!-- ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
            Product Information Selection
         ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ -->
        <table cellspacing="0">
            <thead class="<%=HeaderCssClass %>">
                <tr>
                    <th>Qty</th>
                    <th>Code&nbsp;#</th>
                    <th>Item and Packaging</th>
                    <th style="text-align:right; padding-right:10px;">Price</th>
                    <th style="text-align:right; padding-right:10px;">Total</th>
                </tr>
            </thead>
            <tbody>
                <%=HTMLItemDetails%>
            </tbody>
        </table>
        <hr />
        
        <!-- ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
            Product Totals Selection
         ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ -->
        <table cellpadding="0" cellspacing="1">
            <tr>
                <td class="smallText" style="width:500px; padding-right:50px;">
                    *Shipping is prepaid and added to the invoice along with a handling charge (estimate 10% 
                    for ground shipping; 20% for air shipping to AK, HI, and rush orders; and 25% for international 
                    shipments).  For orders under $100, estimate $10.00 for ground shipping and $25.00 for air shipping 
                    transportation available for AK and HI upon request – please contact Customer Service at 800.323.9540 
                    for assistance.    
                </td>
                <td>
                    <table>
                        <tbody style="text-align:right;">
                            <tr>
                                <td class="title">Shipping Method:</td>
                                <td rowspan="5" style="width:5px;">&nbsp;</td>
                                <td><asp:Label ID="lblConfirmShippingMethod" runat="server" SkinID="RegularText"/></td>
                            </tr>
                            <tr>
                                <td class="subtitle" >Subtotal:</td>
                                <td><asp:Label ID="lblConfirmSubTotal" runat="server"/></td>
                            </tr>
                            <tr>
                                <td class="subtitle" >State Sales Tax:</td>
                                <td><asp:label ID="lblConfirmTax" runat="server" SkinID="Title"/></td>
                            </tr>
                            <tr>
                                <td class="subtitle"  style="border-bottom:1px solid black;">Shipping and Handling*:</td>
                                <td style="border-bottom:1px solid black;">
                                    <asp:Label ID="lblConfirmSH" runat="server" SkinID="Title"/>
                                </td>
                            </tr>
                            <tr>
                                <td class="subtitle" >Order Total:</td>
                                <td><asp:Label ID="lblConfirmOrderTotal" runat="server" SkinID="Title"/></td>
                            </tr>
                        </tbody>
                    </table>

                </td>
            </tr>
        </table>
        
        <div style="text-align:center; padding-top:15px;">
            <asp:Button ID="btnCancel" Text="Cancel" runat="server" Width="150px" ToolTip="Change or Edit the order" OnClick="btnCancel_Click" />&nbsp;&nbsp;&nbsp;
            <asp:Button ID="btnSaveOrder" Text="Process Order" runat="server" Width="150px" ToolTip="Proceed with the Order" OnClientClick="return confirm('Do you with to proceed with your order?');" OnClick="btnSaveOrder_Click" />
        </div>                       
    </asp:View>    
    <asp:View ID="viewThanks" runat="server">
        <div class="title" style="padding-top:20px; text-align:center; width:290px;">
            <div style="padding:10px;">Your order has been submit</div>
            <asp:Button runat="server" ID="btnContinue" Text="Continue" OnClick="btnContinue_Click" />
        </div>
    </asp:View>
</asp:MultiView>

<asp:SqlDataSource ID="dsStates" runat="server" ConnectionString="<%$ ConnectionStrings:LocalSqlServer %>"
    CacheDuration="Infinite"
    SelectCommand="SELECT Abbreviation, Rate FROM CustomerService.States
                   UNION SELECT '',0.00
                   ORDER BY Abbreviation;">
</asp:SqlDataSource> 
<asp:SqlDataSource ID="dsProducts" runat="server" ConnectionString="<%$ ConnectionStrings:LocalSqlServer %>"
    CacheDuration="Infinite"
    SelectCommand="SELECT * FROM CustomerService.Products WHERE IsAvailable = 1 ORDER BY Sequence;">        
</asp:SqlDataSource>
<asp:SqlDataSource ID="dsQty" runat="server" ConnectionString="<%$ ConnectionStrings:LocalSqlServer %>"        
    SelectCommand="NumberedList" SelectCommandType="StoredProcedure" CacheDuration="Infinite">
    <SelectParameters>
        <asp:Parameter Name="StartNumber" Type="int32" DefaultValue="0"/>
        <asp:Parameter Name="EndNumber" Type="int32" DefaultValue="100"/>
        <asp:Parameter Name="Increment" Type="int32" DefaultValue="1"/>
    </SelectParameters>
</asp:SqlDataSource>
<asp:SqlDataSource ID="dsNewOrder" runat="server" ConnectionString="<%$ ConnectionStrings:LocalSqlServer %>"        
    InsertCommand="INSERT INTO [CustomerService].[ProductOrders](
                        [CustomerID], [PaymentType], [CCNum], [CCExpDate], [PONum], [TaxCertNum], [TaxCertExpDate], 
                        [IsTaxExempt], [BillToName], [BillToPosition], [BillToAddress], [BillToCity], [BillToState], 
                        [BillToZip], [BillToEmail], [BillToPhone], [BillToFax], [ShipToName], [ShipToPosition], 
                        [ShipToAddress], [ShipToCity], [ShipToState], [ShipToZip], [ShipToEmail], [ShipToPhone],
                        [ShipToFax], [ShipMethod], [StateTaxRate], [SubTotal], [ShippingHandling], 
                        [OrderedBy], [OrderedOn], [OrderStatusID]) 
                    VALUES (
                        @CustomerID, @PaymentType, @CCNum, @CCExpDate, @PONum, @TaxCertNum, @TaxCertExpDate, 
                        @IsTaxExempt, @BillToName, @BillToPosition, @BillToAddress, @BillToCity, @BillToState, @BillToZip, 
                        @BillToEmail, @BillToPhone, @BilltoFax, @ShipToName, @ShipToPosition, @ShipToAddress, @ShipToCity, 
                        @ShipToState, @ShipToZip, @ShipToEmail, @ShipToPhone, @ShipToFax, @ShipMethod, @StateTaxRate, @SubTotal, @ShippingHandling, 
                        @OrderedBy, GetDate(), @OrderStatusID);
                    SELECT @OrderID = SCOPE_IDENTITY();" 
        OnInserting="dsNewOrder_Inserting" InsertCommandType="Text" OnInserted="dsNewOrder_Inserted">
    <InsertParameters>
        <asp:Parameter Name="OrderID" Type="Int32" Direction="Output" />
        <asp:SessionParameter Name="CustomerID" Type="Int32" SessionField="CustomerID" />
        <asp:Parameter Name="PaymentType" Type="Int32" DefaultValue="1" />
        <asp:Parameter Name="CCNum" Type="String" ConvertEmptyStringToNull="true"/>
        <asp:Parameter Name="CCExpDate" Type="DateTime" ConvertEmptyStringToNull="true"/>
        <asp:Parameter Name="PONum" Type="String" ConvertEmptyStringToNull="true"/>
        <asp:Parameter Name="TaxCertNum" Type="String" ConvertEmptyStringToNull="true" />
        <asp:Parameter Name="TaxCertExpDate" Type="DateTime" ConvertEmptyStringToNull="true"/>
        <asp:Parameter Name="IsTaxExempt" Type="Boolean" />
        <asp:Parameter Name="BillToName" Type="String" />
        <asp:Parameter Name="BillToPosition" Type="String" />
        <asp:Parameter Name="BillToAddress" Type="String" />
        <asp:Parameter Name="BillToCity" Type="String" />
        <asp:Parameter Name="BillToState" Type="String" />
        <asp:Parameter Name="BillToZip" Type="String" />
        <asp:Parameter Name="BillToEmail" Type="String" />
        <asp:Parameter Name="BillToPhone" Type="String" />
        <asp:Parameter Name="BillToFax" Type="String" ConvertEmptyStringToNull="true"/>        
        <asp:Parameter Name="ShipToName" Type="String" />
        <asp:Parameter Name="ShipToPosition" Type="String" />
        <asp:Parameter Name="ShipToAddress" Type="String" />
        <asp:Parameter Name="ShipToCity" Type="String" />
        <asp:Parameter Name="ShipToState" Type="String" />
        <asp:Parameter Name="ShipToZip" Type="String" />
        <asp:Parameter Name="ShipToEmail" Type="String" ConvertEmptyStringToNull="true" />
        <asp:Parameter Name="ShipToPhone" Type="String" ConvertEmptyStringToNull="true" />
        <asp:Parameter Name="ShipToFax" Type="String" ConvertEmptyStringToNull="true"/>        
        <asp:Parameter Name="ShipMethod" Type="String" />
        <asp:Parameter Name="StateTaxRate" Type="Decimal" />
        <asp:Parameter Name="SubTotal" Type="Decimal" />
        <asp:Parameter Name="ShippingHandling" Type="Decimal" />
        <asp:SessionParameter Name="OrderedBy" Type="String" SessionField="StaffName" DefaultValue="CustomerService" />
        <asp:Parameter Name="OrderStatusID" Type="Int32" DefaultValue="1" />
    </InsertParameters>
</asp:SqlDataSource>
<asp:SqlDataSource ID="dsNewOrderDetail" runat="server" ConnectionString="<%$ ConnectionStrings:LocalSqlServer %>"
    InsertCommand="INSERT INTO [CustomerService].[ProductOrderDetails] ([OrderID], [CodeNum], [Price], [Qty], [ProductName])
                   VALUES (@OrderID, @CodeNum, @Price, @Qty, @ProductName);">
    <InsertParameters>
        <asp:Parameter Name="OrderID" Type="Int32" />
        <asp:Parameter Name="CodeNum" Type="String" />
        <asp:Parameter Name="Price" Type="Decimal" />
        <asp:Parameter Name="Qty" Type="Int32" />
        <asp:Parameter Name="ProductName" Type="String" />
    </InsertParameters>
</asp:SqlDataSource>
