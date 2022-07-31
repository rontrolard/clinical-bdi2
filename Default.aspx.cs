using System;
using BDI2Core.Common;
using BDI2Web.Common_Code;
using System.Web.Security;
using System.Web.UI.WebControls;
using System.Web.SessionState;

namespace BDI2Web
{
    public partial class Default : BasePage
    {
        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SetFocus("txtLogin");
                if (Request.QueryString.Count > 0)
                {
                    if (!String.IsNullOrEmpty(Request.QueryString["Logoff"]))
                    {
                        if (string.Compare(Request.QueryString["Logoff"], "0") == 0)
                        {
                            SessionData.StatusMessage = "You have been logged out due to inactivity.";
                        }
                        else if (string.Compare(Request.QueryString["Logoff"], "1") == 0)
                        {
                            SessionData.StatusMessage = "You are now logged out of the application.";
                        }
                    }
                    Response.Redirect("~/default.aspx");
                }
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            if (Request.RequestType == "POST")
            {
                if (Page.IsValid)
                {
                    var IsUser = Membership.GetUser(txtLogin.Text.Trim());

                    //bool oldpasswordValue = Staff.GetStaffByUserNamePassword(txtLogin.Text.Trim(), txtPassword.Text.Trim());

                    Staff staff = IsUser != null ? Staff.GetStaffByUserNamePassword(txtLogin.Text.Trim()) : Staff.GetStaffByUserNamePassword(txtLogin.Text.Trim(), txtPassword.Text.Trim()); //old password

                    Session.Clear();

                    if (IsUser == null && staff != null)
                    {
                        MembershipUser newUser = Membership.CreateUser(txtLogin.Text.Trim(), txtPassword.Text.Trim(), staff.Email);
                        staff.AccountUserPasswordUpdate(staff.StaffID, txtPassword.Text.Trim());
                    }
                    int userLoginAtemptCount = 0;

                    staff = Staff.GetStaffByUserNamePassword(txtLogin.Text.Trim());
                    if (staff != null && staff.DeleteDate == DateTime.MinValue)
                    {
                        userLoginAtemptCount = staff.AccountLoginFailCount(staff.StaffID, "AttemptCount", staff.UserName);
                        if (userLoginAtemptCount < 5)
                        {
                            if (Membership.ValidateUser(txtLogin.Text, txtPassword.Text))
                            {


                                staff.AccountLoginFailUpdate(staff.StaffID, "SuccessfullAttempt", staff.UserName);
                                if (staff.OrganizationIDs.Count == 0)
                                {
                                    SetErrorMessage("This user does not have organizations assigned to them.  Contact customer service.");
                                }
                                else
                                {
                                    // Set Customer Object
                                    Customer customer = Customer.GetCustomerByID(staff.CustomerID);

                                    if (customer.DeleteDate == DateTime.MinValue)
                                    {
                                        //Set logon History
                                        staff.PublishLogonHistory();
                                        //Set Customer ID
                                        SessionData.CustomerID = staff.CustomerID;
                                        SessionData.Staff = staff;
                                        SessionData.MDSUnlimited = Customer.GetCustomerByID(staff.CustomerID).MDSUnlimited;

                                        if (staff.IsActive)
                                        {
                                            if (!customer.IsEulaAccepted)
                                            {
                                                Response.Redirect("~/CustomerRegistration/CustomerRegistration.aspx");
                                            }
                                            else
                                            {
                                                if (SetupHelper.IsCustomerInitialized(staff.CustomerID))
                                                {
                                                    if (!string.IsNullOrEmpty(staff.Email))
                                                        Response.Redirect("~/Welcome/HomePage.aspx");
                                                    else
                                                        Response.Redirect("~/ProfilePage.aspx");
                                                }
                                                else
                                                {
                                                    Response.Redirect("~/Setup/SelectSetupType.aspx");
                                                }
                                            }
                                        }
                                        else
                                        {
                                            Response.Redirect("~/ResetPassword.aspx");
                                        }
                                    }

                                    else
                                    {
                                        SetErrorMessage("Customer is deactivated. Contact customer service.");
                                    }
                                }
                            }
                            else
                            {
                                if ((Membership.GetUser(txtLogin.Text.Trim()) != null && Membership.GetUser(txtLogin.Text.Trim()).IsApproved) || staff != null) // never created a password yet
                                {
                                    staff.AccountLoginFailUpdate(staff.StaffID, "FailledAttempt", staff.UserName);
                                    userLoginAtemptCount = staff.AccountLoginFailCount(staff.StaffID, "AttemptCount", staff.UserName);
                                    if (userLoginAtemptCount == 5)
                                    {
                                        if (staff.Email != "")
                                        {
                                            if (Membership.GetUser(txtLogin.Text.Trim()) == null)
                                            {
                                                MembershipUser newUser = Membership.CreateUser(txtLogin.Text.Trim(), txtPassword.Text.Trim(), staff.Email);
                                            }
                                            BDIEmail.SendLockedUserEmail(staff.Email, staff.UserName);
                                        }
                                        SetErrorMessage("The username and/or password you entered is incorrect.<br/>If this was your fifth unsuccessful login attempt, and you are a registered user, you have been temporarily locked out and a message will be sent to your email address with information on how to unlock your account. If you do not have an email in the system, please contact your organization's BDI-2 Data Manager Account Holder.");
                                    }
                                    else
                                    {
                                        SetErrorMessage("Login ID and/or Password are not correct.");
                                    }
                                }
                                else
                                {
                                    SetErrorMessage("Login ID and/or Password are not correct.");
                                }
                            }
                        }
                        //else
                        //{
                        //    if (staff != null)
                        //        staff.AccountLoginFailUpdate(staff.StaffID, "FailledAttempt");
                        //    SetErrorMessage("Login ID and/or Password are not correct.");
                        //}
                        //}
                        else
                        {
                            SetErrorMessage("The username and/or password you entered is incorrect.<br/>If this was your fifth unsuccessful login attempt, and you are a registered user, you have been temporarily locked out and a message will be sent to your email address with information on how to unlock your account. If you do not have an email in the system, please contact your organization's BDI-2 Data Manager Account Holder.");
                        }
                    }
                    else
                    {
                        SetErrorMessage("Login ID and/or Password are not correct.");
                    }
                }
                SetFocus("txtLogin");
            }
        }

        #endregion Events

        #region Overridden Functions

        protected override void SetPageID()
        {
            PageID = 1; //Value from Page Table in db
        }

        #endregion Overridden Functions
    }
}
