using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Net.Mail;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BDI2Web.Controls
{
    public partial class ProductOrder1 : UserControl
    {
        #region Variable Declarations

        private String _CustomerName = String.Empty;
        private String _HeaderCssClass = String.Empty;
        private ArrayList _ItemDetails;
        private String _RedirectURL = "~";
        private readonly String ConnString = ConfigurationManager.ConnectionStrings["LocalSqlServer"].ToString();
        private readonly string CustomerServiceEmail = ConfigurationManager.AppSettings["CustomerServiceEmail"].ToString();
        private readonly string SoftwareSupportEmail = ConfigurationManager.AppSettings["SoftwareSupportEmail"].ToString();
        private readonly String VirtualPath = "~/App_Data/HTMLTemplates/";

        private Nullable<Int32> _OrderID;
        private Nullable<Decimal> _SalesTax;
        private Nullable<Decimal> _SubTotal;
        private Nullable<Decimal> _ShippingHandling;

        private const Decimal GroundShippingRate1 = 0.10m;   // Inside the Continetal U.S.
        private const Decimal GroundShippingRate2 = 0.20m;   // Hawaii and Alaska
        private const Decimal GroundShippingCost1Under100 = 10.0m; // Inside the Continetal U.S.
        private const Decimal GroundShippingCost2Under100 = 10.0m; // Hawaii and Alaska
        private const Decimal AirShippingRate1 = 0.20m;  // Inside the Continetal U.S.
        private const Decimal AirShippingRate2 = 0.20m;  // Hawaii and Alaska
        //private const Decimal AirShippingCost1Under100 = 25.0m;  // Inside the Continetal U.S.
        //private const Decimal AirShippingCost2Under100 = 25.0m;  // Hawaii and Alaska

        #endregion

        #region Properties

        #region BillTo Information

        private String BillToAddress
        {
            get
            {
                TextBox TextBox1 = (TextBox)FormView1.FindControl("txtBillToAddress");
                return TextBox1.Text;
            }
        }

        private String BillToCity
        {
            get
            {
                TextBox TextBox1 = (TextBox)FormView1.FindControl("txtBillToCity");
                return TextBox1.Text;
            }
        }

        private String BillToEmail
        {
            get
            {
                TextBox TextBox1 = (TextBox)FormView1.FindControl("txtBillToEmail");
                return TextBox1.Text;
            }
        }

        private String BillToFax
        {
            get
            {
                TextBox TextBox1 = (TextBox)FormView1.FindControl("txtBillToFax");
                return TextBox1.Text;
            }
        }

        private String BillToName
        {
            get
            {
                TextBox TextBox1 = (TextBox)FormView1.FindControl("txtBillToName");
                return TextBox1.Text;
            }
        }

        private String BillToPhone
        {
            get
            {
                TextBox TextBox1 = (TextBox)FormView1.FindControl("txtBillToPhone");
                return TextBox1.Text;
            }
        }

        private String BillToPosition
        {
            get
            {
                TextBox TextBox1 = (TextBox)FormView1.FindControl("txtBillToPosition");
                return TextBox1.Text;
            }
        }

        private String BillToState
        {
            get
            {
                DropDownList DropDownlist1 = (DropDownList)FormView1.FindControl("ddlBillToState");
                return DropDownlist1.SelectedItem.Text;
            }
        }

        private String BillToZip
        {
            get
            {
                TextBox TextBox1 = (TextBox)FormView1.FindControl("txtBillToZip");
                return TextBox1.Text;
            }
        }

        #endregion BillTo Information

        #region Credit Card & PO Number
        /// <summary>
        /// Credit Card Name (Visa, Mastercard, Discover, AMEX, etc...)
        /// </summary>
        private String CC
        {
            get
            {
                RadioButtonList btnlstPaymentType = (RadioButtonList)FormView1.FindControl("btnlstPaymentType");
                if (btnlstPaymentType.SelectedIndex == -1)
                    return "None";
                else
                    return btnlstPaymentType.SelectedItem.Text;
            }
        }

        /// <summary>
        /// Credit Card Number
        /// </summary>
        private String CCNum
        {
            get
            {
                TextBox TextBox1 = (TextBox)FormView1.FindControl("txtCCNum");
                return TextBox1.Text;
            }
        }

        /// <summary>
        /// Credit Card Expiration Date
        /// </summary>
        private String CCExpDate
        {
            get
            {
                TextBox TextBox1 = (TextBox)FormView1.FindControl("txtCCExpDate");
                return TextBox1.Text;
            }
        }

        /// <summary>
        /// Tax Exemption Certification Number.
        /// </summary>
        private String CertNo
        {
            get
            {
                TextBox TextBox1;
                MultiView mv1 = (MultiView)FormView1.FindControl("mvPaymentTypes");
                if (mv1.ActiveViewIndex == 0)
                {
                    // Credit Card
                    TextBox1 = (TextBox)FormView1.FindControl("txtCCCertNo");
                }
                else
                {
                    // Purchase Order
                    TextBox1 = (TextBox)FormView1.FindControl("txtPOCertNo");
                }
                return TextBox1.Text;
            }
        }

        /// <summary>
        /// Tax Exemption Certification Expiration Date.
        /// </summary>
        private String CertExpDate
        {
            get
            {
                TextBox TextBox1;
                MultiView mv1 = (MultiView)FormView1.FindControl("mvPaymentTypes");
                if (mv1.ActiveViewIndex == 0)
                {
                    // Credit Card
                    TextBox1 = (TextBox)FormView1.FindControl("txtCCCertExpDate");
                }
                else
                {
                    // Purchase Order
                    TextBox1 = (TextBox)FormView1.FindControl("txtPOCertExpDate");
                }
                return TextBox1.Text;
            }
        }

        #endregion Credit Card & PO Number
        
        /// <summary>
        /// Customer CustomerID. Returns 0 if no value is available.
        /// </summary>
        protected Int32 CustomerID
        {
            get
            {
                if (Session["CustomerID"] == null)
                    return 0;
                return Convert.ToInt32(Session["CustomerID"]);
            }
        }

        /// <summary>
        /// Returns the Customer's Name.
        /// </summary>
        private String CustomerName
        {
            get
            {
                // RULE: Only Query the DB once for the CustomerName.
                if (_CustomerName != String.Empty)
                {
                    return _CustomerName;
                }

                // Query the DB for the Customer Name.
                String cmdText = String.Format("SELECT CustomerName FROM dbo.Customer WHERE CustomerID = {0};", Session["CustomerID"]);
                SqlConnection Conn = new SqlConnection(ConnString);
                SqlCommand SqlCmd = new SqlCommand(cmdText, Conn);
                SqlCmd.Connection.Open();

                try
                {
                    _CustomerName = Convert.ToString(SqlCmd.ExecuteScalar());
                }
                catch (Exception ex)
                {
                    Debug.Assert(true, ex.Message);
                }
                finally
                {
                    if (SqlCmd.Connection.State != ConnectionState.Closed)
                        SqlCmd.Connection.Close();
                }
                return _CustomerName;
            }
        }

        /// <summary>
        /// Set/Get the CssClass Name for the Headers.
        /// </summary>
        public String HeaderCssClass
        {
            get
            {
                return _HeaderCssClass;
            }
            set
            {
                _HeaderCssClass = value;
            }
        }

        /// <summary>
        /// Returns a StringBuilder in HTML format of the Product order.
        /// </summary>
        /// <returns></returns>
        private StringBuilder HTMLInvoice
        {
            get
            {
                StringBuilder HTMLTemplate = new StringBuilder();
                try
                {
                    // STEP 1: Load the HTMLTemplate file into StringBuilder object.
                    HTMLTemplate.Append(File.ReadAllText(HTMLInvoiceTemplateFile));

                    // STEP 2: Insert Data Values.
                    HTMLTemplate.Replace("{OrderID}", OrderID.ToString());
                    HTMLTemplate.Replace("{OrderDate}", String.Format("{0:d}", DateTime.Now));
                    HTMLTemplate.Replace("{PONum}", PONumber);
                    HTMLTemplate.Replace("{TaxCertNum}", CertNo);
                    HTMLTemplate.Replace("{TaxCertExpDate}", CertExpDate);
                    HTMLTemplate.Replace("{CC}", CC);
                    HTMLTemplate.Replace("{CCNum}", CCNum);
                    HTMLTemplate.Replace("{CCExpDate}", CCExpDate);

                    HTMLTemplate.Replace("{BillToName}", BillToName);
                    HTMLTemplate.Replace("{BillToPosition}", BillToPosition);
                    HTMLTemplate.Replace("{BillToAddress}", BillToAddress);
                    HTMLTemplate.Replace("{BillToCity}", BillToCity);
                    HTMLTemplate.Replace("{BillToState}", BillToState);
                    HTMLTemplate.Replace("{BillToZip}", BillToZip);
                    HTMLTemplate.Replace("{BillToPhone}", BillToPhone);
                    HTMLTemplate.Replace("{BillToFax}", BillToFax);
                    HTMLTemplate.Replace("{BillToEmail}", BillToEmail);

                    HTMLTemplate.Replace("{ShipToName}", ShipToName);
                    HTMLTemplate.Replace("{ShipToPosition}", ShipToPosition);
                    HTMLTemplate.Replace("{ShipToAddress}", ShipToAddress);
                    HTMLTemplate.Replace("{ShipToCity}", ShipToCity);
                    HTMLTemplate.Replace("{ShipToState}", ShipToState);
                    HTMLTemplate.Replace("{ShipToZip}", ShipToZip);
                    HTMLTemplate.Replace("{ShipToPhone}", ShipToPhone);
                    HTMLTemplate.Replace("{ShipToFax}", ShipToFax);
                    HTMLTemplate.Replace("{ShipToEmail}", ShipToEmail);

                    HTMLTemplate.Replace("{ItemDetails}", HTMLItemDetails);

                    HTMLTemplate.Replace("{SubTotal}", String.Format("{0:c}", SubTotal));
                    HTMLTemplate.Replace("{SalesTaxRate}", SalesTaxRate.ToString("F"));
                    HTMLTemplate.Replace("{SalesTax}", String.Format("{0:c}", SalesTax));
                    HTMLTemplate.Replace("{ShippingHandling}", String.Format("{0:c}", ShippingHandling));
                    HTMLTemplate.Replace("{OrderTotal}", String.Format("{0:c}", OrderTotal));
                }
                catch (Exception ex)
                {
                    Debug.Assert(true, ex.Message);
                }
                return HTMLTemplate;
            }
        }

        /// <summary>
        /// Filename of the HTML Template file for Product Orders.
        /// </summary>
        private String HTMLInvoiceTemplateFile
        {
            get
            {
                return Server.MapPath(VirtualPath + "ProductOrder.html");
            }
        }

        /// <summary>
        /// Returns HTML TABLERows of the Order Product Detail.
        /// </summary>
        protected String HTMLItemDetails
        {
            get
            {
                StringBuilder HTML = new StringBuilder();
                try
                {
                    foreach (Hashtable ht in ItemDetails)
                    {
                        String[] Item = new String[5];
                        Item[0] = ht["Qty"].ToString();
                        Item[1] = ht["CodeNum"].ToString();
                        Item[2] = ht["ProductName"].ToString();
                        Item[3] = String.Format("{0:c}", ht["Price"]);
                        Item[4] = String.Format("{0:c}", ht["TotalPrice"]);

                        // HTLM Format the data, then append it to the StringBuilder.
                        HTML.Append(String.Format(HTMLItemTemplate, Item));
                    }
                }
                catch (Exception ex)
                {
                    Debug.Assert(true, ex.Message);
                }
                return HTML.ToString();
            }
        }

        /// <summary>
        /// FormatString that will be used for the Item Details in the HTMl Invoice.
        /// </summary>
        private String HTMLItemTemplate
        {
            get
            {
                StringBuilder Template = new StringBuilder();
                try
                {
                    Template.Append("<tr>");
                    Template.Append("<td class='text' style='text-align:center;vertical-align:top; width:50px;'>");
                    Template.Append("   {0}");  // Column #1 (Qty)
                    Template.Append("</td>");
                    Template.Append("<td class='text' style='text-align:center; vertical-align:top; white-space:nowrap;'>");
                    Template.Append("   {1}");  // Column #2 (CodeNumber)
                    Template.Append("</td>");
                    Template.Append("<td style='padding-left:15px;'>");    // Column #3 (Item & Packaging)
                    Template.Append("   <div class='title'>{2}</div>");
                    Template.Append("   <div class='text'></div>");
                    Template.Append("   <div class='smallText'></div>");
                    Template.Append("</td>");
                    Template.Append("<td class='text' style='text-align:right; vertical-align:top; width:105px;'>");
                    Template.Append("   {3}");  // Column #4 (Catalog Price)
                    Template.Append("</td>");
                    Template.Append("<td class='text' style='text-align:right; vertical-align:top; width:105px;'>");
                    Template.Append("   {4}");  // Column #5 (Total Price)
                    Template.Append("</td>");
                }
                catch (Exception ex)
                {
                    Debug.Assert(true, ex.Message);
                }
                return Template.ToString();
            }
        }

        /// <summary>
        /// Return TRUE if the user provided Tax Exemption information on the UI.
        /// </summary>
        private Boolean IsTaxExempt
        {
            get
            {
                // RULE: If the User Provided Tax Exemption information then they are Tax Exempt.
                TextBox TextBox1 = (TextBox)FormView1.FindControl("txtCCCertNo");
                TextBox TextBox2 = (TextBox)FormView1.FindControl("txtPOCertNo");
                if (Convert.ToBoolean(TextBox1.Text.Trim() != String.Empty) ||
                    Convert.ToBoolean(TextBox2.Text.Trim() != String.Empty))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Arraylist of the Products for this order.
        /// </summary>
        private ArrayList ItemDetails
        {
            get
            {
                if (_ItemDetails == null)
                    _ItemDetails = new ArrayList();
                return _ItemDetails;
            }
            set
            {
                _ItemDetails = value;
            }
        }

        /// <summary>
        /// Primary Key for the Order.
        /// </summary>
        private Int32 OrderID
        {
            get
            {
                // Use the Existing value if Available.
                if (_OrderID.HasValue)
                {
                    return _OrderID.Value;
                }
                else
                {
                    return 0;
                }
            }
            set
            {
                _OrderID = value;
            }
        }

        /// <summary>
        /// Total amount of the Order.
        /// </summary>
        private Decimal OrderTotal
        {
            get
            {
                return SalesTax + SubTotal + ShippingHandling;
            }
        }

        private String PaymentType
        {
            get
            {
                RadioButtonList btnlstPaymentType = (RadioButtonList)FormView1.FindControl("btnlstPaymentType");
                if (btnlstPaymentType.SelectedIndex == -1)
                {
                    return "Purchase Order";
                }
                else
                {
                    return "Credit Card";
                }
            }
        }

        /// <summary>
        /// Physical Filepath to the HTMTemplates Directory.
        /// </summary>
        private String PhysicalPath
        {
            get
            {
                return Request.MapPath(VirtualPath);
            }
        }

        /// <summary>
        /// Purchase Order Number
        /// </summary>
        private String PONumber
        {
            get
            {
                TextBox TextBox1 = (TextBox)FormView1.FindControl("txtPONumber");
                return TextBox1.Text;
            }
        }

        /// <summary>
        /// URL the application will be redirected to, on the completion of a Order.
        /// </summary>
        private String RedirectURL
        {
            get
            {
                return _RedirectURL;
            }
            set
            {
                _RedirectURL = value;
            }
        }

        /// <summary>
        /// Calculates and Returns the States Sales Tax for the Order.
        /// </summary>
        private Decimal SalesTax
        {
            get
            {
                // Use the Existing value if Available.
                if (_SalesTax.HasValue)
                {
                    return _SalesTax.Value;
                }

                if (IsTaxExempt)
                {
                    _SalesTax = 0m;
                }
                else
                {
                    _SalesTax = SalesTaxRate * SubTotal / 100m;
                }
                return _SalesTax.Value;
            }
        }

        /// <summary>
        /// State Sales Tax Rate.
        /// </summary>
        private Decimal SalesTaxRate
        {
            get
            {
                //DropDownList ddl = (DropDownList) FormView1.FindControl("ddlShipToState");
                //return Convert.ToDecimal(ddl.SelectedValue);
                if (txtSalesTaxRate.Text.Trim().Equals(""))
                {
                    return 0;
                }
                else
                {
                    return Convert.ToDecimal(txtSalesTaxRate.Text);
                }
            }
        }

        /// <summary>
        /// Calculates and Returns the Shipping Cost for the Order.
        /// </summary>
        private Decimal ShippingHandling
        {
            get
            {
                // Use the Existing value if Available.
                if (_ShippingHandling.HasValue)
                {
                    return _ShippingHandling.Value;
                }
                DropDownList ddl = (DropDownList)FormView1.FindControl("ddlShipToState");
                if (SubTotal <= 100)
                {
                    // RULE: $10 Ground shipping; $25 Air Shipping
                    switch (ddl.SelectedItem.Text.Trim())
                    {
                        case "AK":
                            _ShippingHandling = GroundShippingCost2Under100;
                            break;
                        case "HI":
                            _ShippingHandling = GroundShippingCost2Under100;
                            break;
                        default:
                            _ShippingHandling = GroundShippingCost1Under100;
                            break;
                    }
                }
                else
                {
                    switch (ddlShippingMethod.SelectedValue)
                    {
                        case "Ground":
                            switch (ddl.SelectedItem.Text.Trim())
                            {
                                case "AK":
                                    _ShippingHandling = SubTotal*GroundShippingRate2;
                                    break;
                                case "HI":
                                    _ShippingHandling = SubTotal*GroundShippingRate2;
                                    break;
                                default:
                                    _ShippingHandling = SubTotal*GroundShippingRate1;
                                    break;
                            }
                            break;
                        case "Air":
                            switch (ddl.SelectedItem.Text.Trim())
                            {
                                case "AK":
                                    _ShippingHandling = SubTotal * AirShippingRate2;
                                    break;
                                case "HI":
                                    _ShippingHandling = SubTotal * AirShippingRate2;
                                    break;
                                default:
                                    _ShippingHandling = SubTotal * AirShippingRate1;
                                    break;
                            }
                            break;
                        case "Express":
                            switch (ddl.SelectedItem.Text.Trim())
                            {
                                case "AK":
                                    _ShippingHandling = SubTotal * AirShippingRate2;
                                    break;
                                case "HI":
                                    _ShippingHandling = SubTotal * AirShippingRate2;
                                    break;
                                default:
                                    _ShippingHandling = SubTotal * AirShippingRate1;
                                    break;
                            }
                            break;
                        default:
                            break;
                    }
                }
                return _ShippingHandling.Value;
            }
        }

        /// <summary>
        /// Shipping Method
        /// </summary>
        private String ShipMethod
        {
            get
            {
                return ddlShippingMethod.SelectedValue;
            }
        }

        #region ShipTo Information

        private String ShipToAddress
        {
            get
            {
                TextBox TextBox1 = (TextBox)FormView1.FindControl("txtShipToAddress");
                return TextBox1.Text;
            }
        }

        private String ShipToCity
        {
            get
            {
                TextBox TextBox1 = (TextBox)FormView1.FindControl("txtShipToCity");
                return TextBox1.Text;
            }
        }

        private String ShipToEmail
        {
            get
            {
                TextBox TextBox1 = (TextBox)FormView1.FindControl("txtShipToEmail");
                return TextBox1.Text;
            }
        }

        private String ShipToFax
        {
            get
            {
                TextBox TextBox1 = (TextBox)FormView1.FindControl("txtShipToFax");
                return TextBox1.Text;
            }
        }

        private String ShipToName
        {
            get
            {
                TextBox TextBox1 = (TextBox)FormView1.FindControl("txtShipToName");
                return TextBox1.Text;
            }
        }

        private String ShipToPhone
        {
            get
            {
                TextBox TextBox1 = (TextBox)FormView1.FindControl("txtShipToPhone");
                return TextBox1.Text;
            }
        }

        private String ShipToPosition
        {
            get
            {
                TextBox TextBox1 = (TextBox)FormView1.FindControl("txtShipToPosition");
                return TextBox1.Text;
            }
        }

        private String ShipToState
        {
            get
            {
                DropDownList DropDownlist1 = (DropDownList)FormView1.FindControl("ddlShipToState");
                return DropDownlist1.SelectedItem.Text;
            }
        }

        private String ShipToZip
        {
            get
            {
                TextBox TextBox1 = (TextBox)FormView1.FindControl("txtShipToZip");
                return TextBox1.Text;
            }
        }

        #endregion ShipToTo Information

        /// <summary>
        /// Calculate the Return the Order SubTotal.
        /// </summary>
        /// <param name="Refresh"></param>
        /// <returns></returns>
        private Decimal SubTotal
        {
            get
            {
                // Use the Existing value if Available.
                if (_SubTotal.HasValue)
                {
                    return _SubTotal.Value;
                }

                // Calculate the SubTotal.
                Decimal RetVal = 0m;
                foreach (RepeaterItem Row in rptProducts.Items)
                {
                    // Create a Reference the UI controls in the Repeater Control.
                    Label lblTotal = (Label)Row.FindControl("lblTotal");
                    HiddenField ProductID = (HiddenField)Row.FindControl("hfProductID");
                    HiddenField hfPrice = (HiddenField)Row.FindControl("hfPrice");
                    DropDownList ddlProductQty = (DropDownList)Row.FindControl("ddlProductQty");
                    TextBox txtProductQty = (TextBox)Row.FindControl("txtProductQty");

                    // Extract the Numeric values from the UI Controls.
                    Int32 Qty = (ProductID.Value == "1") ? Qty = Convert.ToInt32(txtProductQty.Text) : Convert.ToInt32(ddlProductQty.Text);
                    Decimal Price = Convert.ToDecimal(hfPrice.Value);
                    Decimal TotalPrice = Qty * Price;

                    // Creating a Running Total.
                    RetVal = RetVal + TotalPrice;
                }
                _SubTotal = RetVal;
                return RetVal;
            }
        }

        #endregion

        #region UI Events

        #region Button Click Events

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            mvMain.SetActiveView(viewOrder);
        }

        protected void btnContinue_Click(object sender, EventArgs e)
        {
            
            Response.Redirect("~/Welcome/Homepage.aspx", true);
        }

        protected void btnPaymentTypeCC_Click(object sender, EventArgs e)
        {
            MultiView mv1 = (MultiView)FormView1.FindControl("mvPaymentTypes");
            mv1.SetActiveView((View)FormView1.FindControl("viewCC"));
        }

        protected void btnPaymentTypePO_Click(object sender, EventArgs e)
        {
            MultiView mv1 = (MultiView)FormView1.FindControl("mvPaymentTypes");
            mv1.SetActiveView((View)FormView1.FindControl("viewPO"));
            RadioButtonList btnlstPaymentType = (RadioButtonList)FormView1.FindControl("btnlstPaymentType");
            btnlstPaymentType.SelectedIndex = -1;
        }

        protected void btnRecalculateTotal_Click(object sender, EventArgs e)
        {
            RefreshOrderTotals();
        }

        protected void btnSaveOrder_Click(object sender, EventArgs e)
        {
            FormView1.InsertItem(false);
            mvMain.SetActiveView(viewThanks);
        }

        protected void btnShipSameAsBill_Click(object sender, EventArgs e)
        {
            CopyBillAddressToShipping();
        }

        protected void btnSubmitOrder_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                if (SubTotal > 0)
                {
                    DisplayOrderConfirmation();
                    ErrorLabel.Text = "";
                }
                else
                {
                    ErrorLabel.Text = "Please select a product.";
                }
            }
        }

        #endregion Button Click Events

        protected void ddlProductQty_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddlProductQty = (DropDownList)sender;
            CalculateLineTotal(ddlProductQty);
            RefreshOrderTotals();
            upnlTotals.Update();
        }

        protected void ddlShipToState_OnSelectedIndexChange(object sender, EventArgs e)
        {
            DropDownList ddlTemp = (DropDownList)sender;
            DropDownList ddlStateRate = (DropDownList)FormView1.FindControl("ddlShiptoStateRates");
            ddlStateRate.SelectedIndex = ddlTemp.SelectedIndex;
            txtSalesTaxRate.Text = ddlStateRate.SelectedValue;

            RefreshOrderTotals();
            upnlTotals.Update();
        }

        protected void ddlShippingMethod_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshOrderTotals();
            upnlTotals.Update();
        }

        protected void FormView1_PreRender(object sender, EventArgs e)
        {
            if (!IsPostBack)
                LoadCustomerInfo();
        }

        protected void rptProducts_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            RepeaterItem Row = (RepeaterItem)e.Item;
            if (Row.ItemType == ListItemType.Item)
            {
                ToggleQtyControls(Row);
            }
        }

        protected void dsNewOrder_Inserted(object sender, SqlDataSourceStatusEventArgs e)
        {
            try
            {
                if (e.AffectedRows > 0)
                {
                    OrderID = (Int32)e.Command.Parameters["@OrderID"].Value;
                    SaveOrderDetails();
                    SendInvoiceToCustomer();
                }
            }
            catch
            {
            }
        }

        protected void dsNewOrder_Inserting(object sender, SqlDataSourceCommandEventArgs e)
        {
            UpdateInsertParameters(e);
        }

        protected void txtProductQty_TextChanged(object sender, EventArgs e)
        {
            TextBox txtProductQty = (TextBox)sender;
            CalculateLineTotal(txtProductQty);
            RefreshOrderTotals();
            upnlTotals.Update();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Calculates the total of 1 Product Line item.
        /// </summary>
        /// <param name="ddlProductQty"></param>
        private void CalculateLineTotal(DropDownList ddlProductQty)
        {
            try
            {
                RepeaterItem Row = (RepeaterItem)ddlProductQty.Parent;
                HiddenField hfPrice = (HiddenField)Row.FindControl("hfPrice");
                Label lblTotal = (Label)Row.FindControl("lblTotal");

                Int32 Qty = Convert.ToInt32(ddlProductQty.Text);
                Decimal Price = Convert.ToDecimal(hfPrice.Value);
                Decimal TotalPrice = Qty * Price;

                if (Qty == 0)
                {
                    lblTotal.Text = "------";
                }
                else
                {
                    lblTotal.Text = string.Format("{0:C}", TotalPrice);
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// Calculates the total of 1 Product Line item.
        /// </summary>
        /// <param name="txtProductQty"></param>
        private static void CalculateLineTotal(TextBox txtProductQty)
        {
            try
            {
                RepeaterItem Row = (RepeaterItem)txtProductQty.Parent;
                HiddenField hfPrice = (HiddenField)Row.FindControl("hfPrice");
                Label lblTotal = (Label)Row.FindControl("lblTotal");

                Int32 Qty = Convert.ToInt32(txtProductQty.Text);
                Decimal Price = Convert.ToDecimal(hfPrice.Value);
                Decimal TotalPrice = Qty * Price;

                if (Qty == 0)
                {
                    lblTotal.Text = "------";
                }
                else
                {
                    lblTotal.Text = string.Format("{0:C}", TotalPrice);
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// Copies all of the Billing Information into the Shipping Info UI controls.
        /// </summary>
        private void CopyBillAddressToShipping()
        {
            try
            {
                DropDownList BillToState = (DropDownList)FormView1.FindControl("ddlBillToState");
                DropDownList ShipToState = (DropDownList)FormView1.FindControl("ddlShipToState");
                DropDownList ShipToStateRate = (DropDownList)FormView1.FindControl("ddlShiptoStateRates");
                ShipToStateRate.SelectedIndex = BillToState.SelectedIndex;
                ShipToState.SelectedIndex = BillToState.SelectedIndex;
                txtSalesTaxRate.Text = ShipToStateRate.SelectedValue;
               
                ((TextBox)FormView1.FindControl("txtShipToName")).Text = BillToName;
                ((TextBox)FormView1.FindControl("txtShipToPosition")).Text = BillToPosition;
                ((TextBox)FormView1.FindControl("txtShipToAddress")).Text = BillToAddress;
                ((TextBox)FormView1.FindControl("txtShipToCity")).Text = BillToCity;
                ((TextBox)FormView1.FindControl("txtShipToZip")).Text = BillToZip;
                ((TextBox)FormView1.FindControl("txtShipToEmail")).Text = BillToEmail;
                ((TextBox)FormView1.FindControl("txtShipToPhone")).Text = BillToPhone;
                ((TextBox)FormView1.FindControl("txtShipToFax")).Text = BillToFax;
                upnlTotals.Update();
            }
            catch
            {
            }
        }

        /// <summary>
        /// Display a Readonly version of the Order to the user for confirmation.
        /// </summary>
        private void DisplayOrderConfirmation()
        {
            try
            {
                Parse_Details();
                mvMain.SetActiveView(viewConfirm);

                // Payment Information
                lblConfirmCustomerName.Text = CustomerName;
                lblConfirmPaymentType.Text = PaymentType;
                lblConfirmPONumber.Text = PONumber;
                lblConfirmCertNo.Text = CertNo;
                lblConfirmCertExpDate.Text = CertExpDate;
                lblConfirmCC.Text = CC;
                lblConfirmCCNum.Text = CCNum;
                lblConfirmCCExpDate.Text = CCExpDate;

                // BillTo Information
                TextBox BillToName = (TextBox)FormView1.FindControl("txtBillToName");
                lblBillToName.Text = BillToName.Text;
                TextBox BillToPosition = (TextBox)FormView1.FindControl("txtBillToPosition");
                lblBillToPosition.Text = BillToPosition.Text;
                TextBox BillToAddress = (TextBox)FormView1.FindControl("txtBillToAddress");
                lblBillToAddress.Text = BillToAddress.Text;
                TextBox BillToCity = (TextBox)FormView1.FindControl("txtBillToCity");
                lblBillToCity.Text = BillToCity.Text;
                TextBox BillToZip = (TextBox)FormView1.FindControl("txtBillToZip");
                lblBillToZip.Text = BillToZip.Text;
                TextBox BillToPhone = (TextBox)FormView1.FindControl("txtBillToPhone");
                lblBillToPhone.Text = BillToPhone.Text;
                TextBox BillToFax = (TextBox)FormView1.FindControl("txtBillToFax");
                lblBillToFax.Text = BillToFax.Text;
                TextBox BillToEmail = (TextBox)FormView1.FindControl("txtBillToEmail");
                lblBillToEmail.Text = BillToEmail.Text;
                DropDownList BillToState = (DropDownList)FormView1.FindControl("ddlBillToState");
                lblBillToState.Text = BillToState.SelectedItem.Text;

                // ShipTo Information
                lblShipToName.Text = ShipToName;
                lblShipToPosition.Text = ShipToPosition;
                lblShipToAddress.Text = ShipToAddress;
                lblShipToCity.Text = ShipToCity;
                lblShipToState.Text = ShipToState;
                lblShipToZip.Text = ShipToZip;
                lblShipToPhone.Text = ShipToPhone;
                lblShipToFax.Text = ShipToFax;
                lblShipToEmail.Text = ShipToEmail;

                // Order Totals
                lblConfirmShippingMethod.Text = ShipMethod;
                lblConfirmSubTotal.Text = String.Format("{0:c}", SubTotal);
                lblSalesTaxRate.Text = SalesTaxRate.ToString("F");
                lblConfirmTax.Text = String.Format("{0:c}", SalesTax);
                lblConfirmSH.Text = String.Format("{0:c}", ShippingHandling);
                lblConfirmOrderTotal.Text = String.Format("{0:c}", OrderTotal);
            }
            catch (Exception ex)
            {
                Debug.Assert(false, ex.Message);
            }
        }

        /// <summary>
        /// Queries the DB for the CustomerName
        /// </summary>
        private void LoadCustomerInfo()
        {
            try
            {
                Label lblCustomerName = (Label)FormView1.FindControl("lblCustomerName");
                lblCustomerName.Text = CustomerName;
            }
            catch (Exception ex)
            {
                Debug.Assert(true, ex.Message);
            }
        }

        /// <summary>
        /// Load the Product Item Details into an ArrayList (that Contains HashTables of Product Details). 
        /// </summary>
        private void Parse_Details()
        {
            try
            {
                ItemDetails.Clear();

                // Load Item Details in an ArrayList that Contains a HashTables of Product Details. 
                foreach (RepeaterItem Row in rptProducts.Items)
                {
                    // Create a Reference the UI controls in the Repeater Control.
                    Label lblTotal = (Label)Row.FindControl("lblTotal");
                    Literal CodeNum = (Literal)Row.FindControl("litCodeNum");
                    HiddenField hfPrice = (HiddenField)Row.FindControl("hfPrice");
                    HiddenField ProductID = (HiddenField)Row.FindControl("hfProductID");
                    HiddenField ProductName = (HiddenField)Row.FindControl("hfProductName");
                    ProductName.Value = ProductName.Value.Replace("®", "&#174;");
                    DropDownList ddlProductQty = (DropDownList)Row.FindControl("ddlProductQty");
                    TextBox txtProductQty = (TextBox)Row.FindControl("txtProductQty");

                    // Extract the Numeric values from the UI Controls.
                    Int32 Qty = (ProductID.Value == "1") ? Qty = Convert.ToInt32(txtProductQty.Text) : Convert.ToInt32(ddlProductQty.Text);
                    Decimal Price = Convert.ToDecimal(hfPrice.Value);
                    Decimal TotalPrice = Qty * Price;

                    if (Qty > 0)
                    {
                        Hashtable Item = new Hashtable();
                        Item.Add("ProductID", Convert.ToInt32(ProductID.Value));
                        Item.Add("Qty", Qty);
                        Item.Add("CodeNum", CodeNum.Text);
                        Item.Add("ProductName", ProductName.Value);
                        Item.Add("Description", String.Empty);
                        Item.Add("SubDescription", String.Empty);
                        Item.Add("Price", Price);
                        Item.Add("TotalPrice", TotalPrice);
                        ItemDetails.Add(Item);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Assert(true, ex.Message);
            }
        }

        /// <summary>
        /// Send an HTML copy of the Invoice to the Customer.
        /// </summary>
        private void SendInvoiceToCustomer()
        {
            try
            {
                String EmailAddresses = BillToEmail + ";" + ShipToEmail + ";" + CustomerServiceEmail;
                SendMailWithIIS(EmailAddresses, SoftwareSupportEmail, "Riverside Publishing", "Order #" + OrderID.ToString() + " (Riverside Publishing)", HTMLInvoice.ToString(), MailPriority.Normal);
            }
            catch (Exception ex)
            {
                Debug.Assert(true, ex.Message);
            }

        }

        /// <summary>
        /// Refreshes the Order Total Labels on the UI.
        /// </summary>
        private void RefreshOrderTotals()
        {
            Decimal OrderTotal = SubTotal + SalesTax + ShippingHandling;
            lblSubTotal.Text = string.Format("{0:C}", _SubTotal);
            lblTax.Text = string.Format("{0:C}", SalesTax);
            lblSH.Text = string.Format("{0:C}", ShippingHandling);
            lblOrderTotal.Text = string.Format("{0:C}", OrderTotal);
        }

        /// <summary>
        /// Iterates through all of the Products Saving the ones the user selected.
        /// </summary>
        /// <param name="OrderID"></param>
        private void SaveOrderDetails()
        {
            Parse_Details();
            foreach (Hashtable ht in ItemDetails)
            {
                // Insert the Product Detail into the Database.
                dsNewOrderDetail.InsertParameters["OrderID"].DefaultValue = OrderID.ToString();
                dsNewOrderDetail.InsertParameters["CodeNum"].DefaultValue = ht["CodeNum"].ToString();
                dsNewOrderDetail.InsertParameters["Price"].DefaultValue = ht["Price"].ToString();
                dsNewOrderDetail.InsertParameters["Qty"].DefaultValue = ht["Qty"].ToString();
                dsNewOrderDetail.InsertParameters["ProductName"].DefaultValue = ht["ProductName"].ToString();
                dsNewOrderDetail.Insert();
            }
        }

        /// <summary>
        /// Relays an Email message through the IIS SMTP Service.
        /// </summary>
        /// <param name="strTo"></param>
        private static void SendMailWithIIS(String strTo, String strFromEmail, String strFromDisplayName, String strSubject, String strBody, MailPriority Priority)
        {
            try
            {
                SendMailWithIIS(strTo, strFromEmail, strFromDisplayName, strSubject, strBody, string.Empty, string.Empty, Priority);
            }
            catch (Exception ex)
            {
                Debug.Assert(true, ex.Message);
            }
        }

        /// <summary>
        /// Relays an Email message through the IIS SMTP Service.
        /// </summary>
        /// <param name="strTo"></param>
        private static void SendMailWithIIS(String strTo, String strFromEmail, String strFromDisplayName, String strSubJect, String strBody, String strCC, String strBCC, MailPriority Priority)
        {
            try
            {
                // STEP 1: Variable Declarations            
                MailMessage MailMsg = new MailMessage();
                MailAddress MailFrom = new MailAddress(strFromEmail, strFromDisplayName);
                SmtpClient Client = new SmtpClient();

                // STEP 2: Configure the Mail message object
                MailMsg.IsBodyHtml = true;
                MailMsg.Body = strBody;
                MailMsg.BodyEncoding = Encoding.ASCII;
                MailMsg.Subject = strSubJect;
                MailMsg.From = MailFrom;
                MailMsg.Priority = Priority;

                // STEP 3a: Insert Each address in the "To" 1 at a time.
                String[] ToAddresses;
                Char[] splitter = { ';' };
                ToAddresses = strTo.Split(splitter);
                foreach (String Email in ToAddresses)
                    MailMsg.To.Add(Email);

                // STEP 3b:Add Carbon Copy and Blind Carbon Copy addresses.
                if (strCC != "")
                    MailMsg.CC.Add(strCC);
                if (strBCC != "")
                    MailMsg.Bcc.Add(strBCC);

                // STEP 4: Transmit Email            
                Client.Send(MailMsg);
                Client = null;
            }
            catch (Exception ex)
            {
                Debug.Assert(true, ex.Message);
            }
        }

        /// <summary>
        /// Toggles between a Textbox and DropdownList control for the Product Qty.
        /// </summary>
        /// <param name="Row"></param>
        private void ToggleQtyControls(RepeaterItem Row)
        {
            try
            {
                HiddenField hfProductID = (HiddenField)Row.FindControl("hfProductID");
                UpdatePanel upnlCalculateRowTotal = (UpdatePanel)Row.FindControl("upnlCalculateRowTotal");
                if (hfProductID.Value == "1")
                {
                    DropDownList ddlProductQty = (DropDownList)Row.FindControl("ddlProductQty");
                    RequiredFieldValidator RequiredFieldValidatorProductQty = (RequiredFieldValidator)Row.FindControl("RequiredFieldValidatorProductQty");
                    RangeValidator RangeValidatorProductQty = (RangeValidator)Row.FindControl("RangeValidatorProductQty");
                    TextBox txtProductQty = (TextBox)Row.FindControl("txtProductQty");

                    ddlProductQty.Visible = false;
                    RequiredFieldValidatorProductQty.Visible = true;
                    RangeValidatorProductQty.Visible = true;
                    txtProductQty.Visible = true;
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// Updates the Insert Paramters values of the ProductOrder SqlDataSource.
        /// </summary>
        private void UpdateInsertParameters(SqlDataSourceCommandEventArgs e)
        {
            try
            {
                // Charge Information
                e.Command.Parameters["@PONum"].Value = PONumber;
                e.Command.Parameters["@CCNum"].Value = CCNum;
                e.Command.Parameters["@CCExpDate"].Value = CCExpDate == "" ? e.Command.Parameters["@CCExpDate"].Value = DBNull.Value : CCExpDate;
                e.Command.Parameters["@TaxCertNum"].Value = CertNo;
                e.Command.Parameters["@TaxCertExpDate"].Value = CertExpDate == "" ? e.Command.Parameters["@TaxCertExpDate"].Value=DBNull.Value : CertExpDate;
                e.Command.Parameters["@IsTaxExempt"].Value = IsTaxExempt;

                // StateTax Rate, SubTotals, Shipping & Handling.
                e.Command.Parameters["@StateTaxRate"].Value = SalesTaxRate;
                e.Command.Parameters["@SubTotal"].Value = SubTotal;
                e.Command.Parameters["@ShippingHandling"].Value = ShippingHandling;
                e.Command.Parameters["@ShipMethod"].Value = ShipMethod;

                // Shipping Information
                e.Command.Parameters["@ShipToName"].Value = ShipToName;
                e.Command.Parameters["@ShipToPosition"].Value = ShipToPosition;
                e.Command.Parameters["@ShipToAddress"].Value = ShipToAddress;
                e.Command.Parameters["@ShipToCity"].Value = ShipToCity;
                e.Command.Parameters["@ShipToState"].Value = ShipToState;
                e.Command.Parameters["@ShipToZip"].Value = ShipToZip;
                e.Command.Parameters["@ShipToEmail"].Value = ShipToEmail;
                e.Command.Parameters["@ShipToPhone"].Value = ShipToPhone;
                e.Command.Parameters["@ShipToFax"].Value = ShipToFax;
            }
            catch
            {
            }
        }
       public void ValidateCCDate(object source, ServerValidateEventArgs args) 
       {
           args.IsValid = true;
           //string exp = @"((0?[13578]|10|12)(-|\/)((0[0-9])|([12])([0-9]?)|(3[01]?))(-|\/)((\d{4})|(\d{2}))|(0?[2469]|11)(-|\/)((0[0-9])|([12])([0-9]?)|(3[0]?))(-|\/)((\d{4}|\d{2})))";
           string exp = @"^((0[1-9])|(1[02]))/\d{2}$";
           if (!System.Text.RegularExpressions.Regex.IsMatch(args.Value, exp))
           {
               ((CustomValidator)source).ErrorMessage = "Invalid Credit Card Expiration Date. Format: MM/YY.";
               args.IsValid = false;
           }
           else
           {
               int year = Convert.ToInt32(args.Value.Substring(3, 2));
               year = year + 2000;
               DateTime ccdate = new DateTime(year, Convert.ToInt32(args.Value.Substring(0,2)), 1);
               bool ccDateExpired = false;

               if (ccdate.Year < DateTime.Now.Year)
               {
                   ccDateExpired = true;
               }
               else
               {
                   if (ccdate.Year == DateTime.Now.Year)
                   {
                       if (ccdate.Month < DateTime.Now.Month)
                       {
                           ccDateExpired = true;
                       }
                   }
               }
               if (ccDateExpired)
               {
                   ((CustomValidator)source).ErrorMessage = "Credit Card date is expired.";
                   args.IsValid = false;
               }
           }
       }

       #endregion
    }
}