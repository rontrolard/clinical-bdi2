using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BDI2Core.Common;
using BDI2Web.Common_Code;
using BDI2Core.Data;



namespace BDI2Web.CustomerRegistration
{
    public partial class CustomerRegistration : SecureBasePage
    {
        #region events
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            LoadRegistrationControls(!IsPostBack);
        }
        #endregion

        #region private functions
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            Customer customer = new Customer();
            customer.CustomerID = SessionData.CustomerID.Value;

            //validate & save registration settings for customer
            char[] delimit = new char[] { ';' };
            foreach (string substr in SettingControlsIdentifiers.Value.Split(delimit))
            {
                if (substr.Length > 0)
                {
                    string settingValue = "";
                    string[] str = substr.Split(new char[] { '_' });
                    switch (str[0])
                    {
                        case "textbox":
                            HtmlInputText textbox = (HtmlInputText) mainPanel.FindControl(substr);
                            settingValue = textbox.Value;
                            break;
                        case "checkbox":
                            HtmlInputCheckBox checkbox = (HtmlInputCheckBox) mainPanel.FindControl(substr);
                            settingValue = checkbox.Checked ? "1" : "" ;
                            break;
                        default:
                            break;
                    }
                    customer.SaveRegistrationSetting(Convert.ToInt32(str[1]), settingValue.ToString());
                }
            }

            //display EULA agreement
            Response.Redirect("~/CustomerRegistration/EULA.aspx");
        }

        protected void LoadRegistrationControls(bool DisplayDataFromDatabase)
        {
            // get list of registration categories


            DataSet RegistrationCategory = Lookups.GetRegistrationCategory();
            foreach (DataRow Category in RegistrationCategory.Tables[0].Rows)
            {
                HtmlTable tableCategory = new HtmlTable();

                // get category's settings

                DataSet RegistrationSetting = Lookups.GetRegistrationSetting((int)(Category["registrationCategoryID"]),(int) SessionData.CustomerID);

                int totalSettings = RegistrationSetting.Tables[0].Rows.Count;
                if (totalSettings > 0)
                {
                    HtmlTableRow trSettings = new HtmlTableRow();
                    HtmlTableCell tdSettings = new HtmlTableCell();

                    HtmlTable tableSubCategories = new HtmlTable();
                    HtmlTableRow trSubCategories = new HtmlTableRow();

                    String subCategory = RegistrationSetting.Tables[0].Rows[0]["SubCategory"].ToString();
                    string tmpSubCategory = subCategory;
                    int counter = 0;
                    while (counter < totalSettings)
                    {
                        HtmlTableCell tdSubCategory = new HtmlTableCell();
                        subCategory = RegistrationSetting.Tables[0].Rows[counter]["SubCategory"].ToString();

                        HtmlTable tableControls = new HtmlTable();
                        HtmlTableRow trSubCategoryName = new HtmlTableRow();
                        HtmlTableCell tdSubCategoryName = new HtmlTableCell();
                        tdSubCategoryName.ColSpan = 2;
                        tdSubCategoryName.InnerText = subCategory;
                        trSubCategoryName.Cells.Add(tdSubCategoryName);
                        tableControls.Rows.Add(trSubCategoryName);
                        while (subCategory == tmpSubCategory)
                        {
                            HtmlTableRow trControls = new HtmlTableRow();
                            HtmlTableCell tdSettingName = new HtmlTableCell();
                            HtmlTableCell tdSettingControl = new HtmlTableCell();

                            HtmlGenericControl spanSettingName = new HtmlGenericControl();
                            bool isRequired = (bool)RegistrationSetting.Tables[0].Rows[counter]["isRequired"];
                            spanSettingName.InnerHtml = string.Concat(isRequired ? "<font color='red'>*</font>" : "", "<span class='defaultLabelStyle'>" + RegistrationSetting.Tables[0].Rows[counter]["SettingName"].ToString() + "</span>");
                            switch (RegistrationSetting.Tables[0].Rows[counter]["settingType"].ToString())
                            {
                                case "checkbox":
                                    HtmlInputCheckBox checkbox = new HtmlInputCheckBox();
                                    checkbox.ID = string.Concat("checkbox_", RegistrationSetting.Tables[0].Rows[counter]["registrationSettingID"].ToString());
                                    SettingControlsIdentifiers.Value = string.Concat(SettingControlsIdentifiers.Value, checkbox.ID,";");
                                    if (DisplayDataFromDatabase)
                                    {
                                        checkbox.Checked = (RegistrationSetting.Tables[0].Rows[counter]["settingValue"].ToString()== "1") ? true : false;
                                    }                                    
                                    tdSettingControl.Controls.Add(checkbox);
                                    tdSettingName.Controls.Add(spanSettingName);
                                    trControls.Cells.Add(tdSettingControl);
                                    trControls.Cells.Add(tdSettingName);
                                    break;
                                case "textbox":
                                    HtmlInputText textbox = new HtmlInputText();
                                    textbox.ID=string.Concat("textbox_", RegistrationSetting.Tables[0].Rows[counter]["registrationSettingID"].ToString());
                                    if (DisplayDataFromDatabase)
                                    {
                                        textbox.Value = RegistrationSetting.Tables[0].Rows[counter]["settingValue"].ToString();
                                    }
                                   
                                    SettingControlsIdentifiers.Value = string.Concat(SettingControlsIdentifiers.Value, textbox.ID,";");
                                    tdSettingControl.Controls.Add(textbox);

                                    if (isRequired)
                                    {
                                        RequiredFieldValidator validator = new RequiredFieldValidator();
                                        validator.Text = "&nbsp;(required!)";
                                        validator.ControlToValidate = textbox.ID.ToString();
                                        tdSettingControl.Controls.Add(validator);
                                    }

                                   if (RegistrationSetting.Tables[0].Rows[counter]["regularExpressionValidator"] != DBNull.Value)
                                    {
                                        if (!RegistrationSetting.Tables[0].Rows[counter]["SettingName"].ToString().Equals("Email"))
                                        {
                                            RegularExpressionValidator validator = new RegularExpressionValidator();
                                            validator.ValidationExpression = RegistrationSetting.Tables[0].Rows[counter]["regularExpressionValidator"].ToString();
                                            validator.Text = string.Concat("<li>", RegistrationSetting.Tables[0].Rows[counter]["SettingName"].ToString(), ":&nbsp;", RegistrationSetting.Tables[0].Rows[counter]["validationMsg"].ToString());
                                            validator.ControlToValidate = textbox.ID.ToString();
                                            panelValidationMsg.Controls.Add(validator);
                                            panelValidationMsg.Controls.Add(new LiteralControl("<br />"));
                                            //tdSettingControl.Controls.Add(validator);
                                        }
                                    }
                                    
                                    tdSettingName.Controls.Add(spanSettingName);
                                    trControls.Cells.Add(tdSettingName);
                                    trControls.Cells.Add(tdSettingControl);
                                
                                    break;
                                default:
                                    break;
                            }
                            tableControls.Rows.Add(trControls);
                            counter++;
                            if (counter < totalSettings)
                            {
                                tmpSubCategory = RegistrationSetting.Tables[0].Rows[counter]["SubCategory"].ToString();
                            }
                            else
                            {
                                tmpSubCategory = null;
                            }
                        }
                        tdSubCategory.Controls.Add(tableControls);
                        trSubCategories.Cells.Add(tdSubCategory);
                    }

                    tableSubCategories.Rows.Add(trSubCategories);

                    tdSettings.Controls.Add(tableSubCategories);

                    trSettings.Cells.Add(tdSettings);
                    tableCategory.Rows.Add(trSettings);
                }

                mainPanel.Controls.Add(tableCategory);
                mainPanel.Controls.Add(new HtmlGenericControl("P"));
            }

        }
        #endregion

        #region Overridden Functions

        protected override void SetPageID()
        {
            PageID = 32; //Value from Page Table in db
        }

        #endregion Overridden Functions
    }
}
