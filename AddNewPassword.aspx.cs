using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using BDI2Core.Common;
using BDI2Web.Common_Code;
using System.Text.RegularExpressions;

namespace BDI2Web
{
    public partial class AddNewPassword : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    if (Request.QueryString.ToString() != "")
                    {
                        AddNewPasswordPage.Visible = true;
                        string UserProvidedKey = Request.QueryString["Token"].ToString();
                        Guid userId = Guid.Empty;
                        userId = new Guid(UserProvidedKey);
                        MembershipUser user = Membership.GetUser(userId);
                        if (user != null)
                        {
                            if (!user.IsApproved)
                            {
                                lblUserName.Text = user.UserName;
                                AddNewPasswordPage.Visible = true;
                            }
                            else
                            {
                                lblError.Text = "This activity has expired, please contact support.";
                                AddNewPasswordPage.Visible = false;
                            }
                        }
                        else
                        {
                            lblError.Text = "This activity has expired, please contact support.";
                            AddNewPasswordPage.Visible = false;
                        }
                    }
                    else
                    {
                        lblError.Text = "This activity has expired, please contact support.";
                        AddNewPasswordPage.Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Redirect("~/Default.aspx");
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (Page.IsValid)
                {
                    //string url = Request.Url.AbsoluteUri;
                    string UserProvidedKey = Request.QueryString["Token"].ToString();
                    Guid userId = Guid.Empty;
                    userId = new Guid(UserProvidedKey);
                    MembershipUser user = Membership.GetUser(userId);
                    string userName = user.UserName;
                    if (!user.IsApproved)
                    {
                        if (user.IsLockedOut)
                            user.UnlockUser();
                        string ResetPwd = user.ResetPassword();
                        bool isChanged = user.ChangePassword(ResetPwd, txtPassword.Text.Trim());
                        if (isChanged)
                        {
                            user.IsApproved = true;
                            Membership.UpdateUser(user);
                            Staff staff = Staff.GetStaffByUserNamePassword(userName.Trim());
                            staff.AccountUserPasswordUpdate(staff.StaffID, txtPassword.Text.Trim());
                            if (staff != null && staff.DeleteDate == DateTime.MinValue)
                            {
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
                                        staff = Staff.GetStaffByUserNamePassword(userName.Trim()); //Get the new password to the session
                                        //Set logon History
                                        staff.PublishLogonHistory();
                                        //Set Customer ID
                                        SessionData.CustomerID = staff.CustomerID;
                                        SessionData.Staff = staff;
                                        SessionData.MDSUnlimited = Customer.GetCustomerByID(staff.CustomerID).MDSUnlimited;
                                        staff.IsActive = true;
                                        staff.ResetStaffPasswordActive(staff.StaffID, staff.IsActive);

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
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message.ToString();
            }
        }

        protected void Passwordvalidation(object sender, ServerValidateEventArgs e)
        {
            string UserProvidedKey = Request.QueryString["Token"].ToString();
            Guid userId = Guid.Empty;
            userId = new Guid(UserProvidedKey);
            string UserName = Membership.GetUser(userId).UserName;
            if ((txtPassword.Text == txtConfPassword.Text) && (UserName != txtPassword.Text.Trim()))
            {
                if (e.Value.Trim().Length < 8 || e.Value.Trim().Length > 10)
                {
                    e.IsValid = false;
                }
                else
                {
                    const string reg1 = @"[A-ZÆÐƎƏƐƔĲŊŒẞǷȜĄƁÇĐƊĘĦĮƘŁØƠŞȘŢȚŦŲƯY̨ƳÁÀÂÄǍĂĀÃÅǺĄÆǼǢƁĆĊĈČÇĎḌĐƊÐÉÈĖÊËĚĔĒĘẸƎƏƐĠĜǦĞĢƔĤḤĦIÍÌİÎÏǏĬĪĨĮỊĲĴĶƘĹĻŁĽĿʼNŃN̈ŇÑŅŊÓÒÔÖǑŎŌÕŐỌØǾƠŒŔŘŖŚŜŠŞȘṢẞŤŢṬŦÞÚÙÛÜǓŬŪŨŰŮŲỤƯẂẀŴẄǷÝỲŶŸȲỸƳŹŻŽẒ]";
                    //@"[A-ZÆÐƎƏƐƔĲŊŒẞÞǷȜæðǝəɛɣĳŋœĸſßþƿȝĄƁÇĐƊĘĦĮƘŁØƠŞȘŢȚŦŲƯY̨Ƴąɓçđɗęħįƙłøơşșţțŧųưy̨ƴÁÀÂÄǍĂĀÃÅǺĄÆǼǢƁĆĊĈČÇĎḌĐƊÐÉÈĖÊËĚĔĒĘẸƎƏƐĠĜǦĞĢƔáàâäǎăāãåǻąæǽǣɓćċĉčçďḍđɗðéèėêëěĕēęẹǝəɛġĝǧğģɣĤḤĦIÍÌİÎÏǏĬĪĨĮỊĲĴĶƘĹĻŁĽĿʼNŃN̈ŇÑŅŊÓÒÔÖǑŎŌÕŐỌØǾƠŒĥḥħıíìiîïǐĭīĩįịĳĵķƙĸĺļłľŀŉńn̈ňñņŋóòôöǒŏōõőọøǿơœŔŘŖŚŜŠŞȘṢẞŤŢṬŦÞÚÙÛÜǓŬŪŨŰŮŲỤƯẂẀŴẄǷÝỲŶŸȲỸƳŹŻŽẒŕřŗſśŝšşșṣßťţṭŧþúùûüǔŭūũűůųụưẃẁŵẅƿýỳŷÿȳỹƴźżžẓ]";
                    const string reg2 =
                        @"[a-zÞæðǝəɛɣĳŋœĸſßþƿȝąɓçđɗęħįƙłøơşșţțŧųưy̨áàâäǎăāãåǻąæǽǣɓćċĉčçďḍđɗðéèėêëěĕēęẹǝəɛġĝǧğģɣĥḥħıíìiîïǐĭīĩįịĳĵķƙĸĺļłľŀŉńn̈ňñņŋóòôöǒŏōõőọøǿơœÞŕřŗſśŝšşșṣßťţṭŧþúùûüǔŭūũűůųụưẃẁŵẅƿýỳŷÿȳỹƴźżžẓ]";
                    //[a-zÆÐƎƏƐƔĲŊŒẞÞǷȜæðǝəɛɣĳŋœĸſßþƿȝĄƁÇĐƊĘĦĮƘŁØƠŞȘŢȚŦŲƯY̨Ƴąɓçđɗęħįƙłøơşșţțŧųưy̨ƴÁÀÂÄǍĂĀÃÅǺĄÆǼǢƁĆĊĈČÇĎḌĐƊÐÉÈĖÊËĚĔĒĘẸƎƏƐĠĜǦĞĢƔáàâäǎăāãåǻąæǽǣɓćċĉčçďḍđɗðéèėêëěĕēęẹǝəɛġĝǧğģɣĤḤĦIÍÌİÎÏǏĬĪĨĮỊĲĴĶƘĹĻŁĽĿʼNŃN̈ŇÑŅŊÓÒÔÖǑŎŌÕŐỌØǾƠŒĥḥħıíìiîïǐĭīĩįịĳĵķƙĸĺļłľŀŉńn̈ňñņŋóòôöǒŏōõőọøǿơœŔŘŖŚŜŠŞȘṢẞŤŢṬŦÞÚÙÛÜǓŬŪŨŰŮŲỤƯẂẀŴẄǷÝỲŶŸȲỸƳŹŻŽẒŕřŗſśŝšşșṣßťţṭŧþúùûüǔŭūũűůųụưẃẁŵẅƿýỳŷÿȳỹƴźżžẓ]
                    const string reg3 = @"[0-9]";
                    const string reg4 = @"[~!@#$%^&*_\-+=`|\\(\){}\[\]:;'<>,.?\/""]";
                    const string reg5 = @"[\s]";
                    var result1 = Regex.Match(e.Value, reg1).Success;
                    var result2 = Regex.Match(e.Value, reg2).Success;
                    var result3 = Regex.Match(e.Value, reg3).Success;
                    var result4 = Regex.Match(e.Value, reg4).Success;
                    if (((result1 && result2 && result3) || (result1 && result2 && result4) || (result1 && result3 && result4) || (result2 && result3 && result4)) && (!Regex.Match(e.Value, reg5).Success))
                    {
                        e.IsValid = true;
                    }
                    else
                    {
                        e.IsValid = false;
                        txtPassword.Focus();
                    }
                }
            }
            else
            {
                e.IsValid = false;
                txtPassword.Focus();
            }
        }

        #region Overridden Functions

        protected override void SetPageID()
        {
            PageID = 74; //Value from Page Table in db
        }

        #endregion Overridden Functions
    }

}
