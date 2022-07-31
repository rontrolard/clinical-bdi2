using System;
using System.Data;
using System.Web.UI.WebControls;
using BDI2Core.Common;
using BDI2Web.Common_Code;

namespace BDI2Web.Assessment
{
    public partial class TestCalendar : SecureBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                StudentAssessment sa = AssessmentCommon.GetSessionStudentAssessment();
                if (sa != null)
                {
                    sa.ClearCalendarSelection();

                    DataSet ds = sa.FormatDomainList();
                    RepeaterDomain.DataSource = ds;
                    RepeaterDomain.DataBind();
                }
            }
        }

        #region Overridden Functions

        protected override void SetPageID()
        {
            PageID = 26; //Value from Page Table in db
        }

        #endregion Overridden Functions

        protected void CheckBoxAll_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox selectAll = sender as CheckBox;

            foreach (RepeaterItem item in RepeaterDomain.Items)
            {
                CheckBox domainSelect = (CheckBox)item.FindControl("domainSelect");
                if (domainSelect != null)
                {
                    domainSelect.Checked = selectAll.Checked;
                }
            }
        }

        protected void ButtonCancel_Click(object sender, EventArgs e)
        {

        }

        protected void ButtonContinue_Click(object sender, EventArgs e)
        {
            
            DateTime selectedDate = RadCalendar1.SelectedDate;

            if (selectedDate.CompareTo(new DateTime(0001, 1, 1)) == 0)
            {
                 lblMessage.Text = "Test Date is required";
                 lblMessage.Visible = true;
                 return;
            }
            if (selectedDate.CompareTo(DateTime.Today) > 0)
            {
                lblMessage.Text = "Test Date cannot be future date";
                lblMessage.Visible = true;
                return;
            }

            bool isDomainSelected = false;
            StudentAssessment sa = AssessmentCommon.GetSessionStudentAssessment();
            foreach (RepeaterItem item in RepeaterDomain.Items)
            {
                CheckBox domainSelect = (CheckBox)item.FindControl("domainSelect");
                if (domainSelect.Checked == true)
                {
                    isDomainSelected = true;
                    int domainID = int.Parse(((HiddenField)item.FindControl("domainID")).Value);
                    StudentDomain sd = sa.FindDomain(domainID);
                    if (sd != null)
                    {
                        if (sd.DomainTypeID == (int)DomainType.Subdomain)
                        {
                            sd.Usage_Selection = true;
                            sd.Calendar_Selection = true; //turn off in base window's OnButtonSubmit_Click function
                            sd.CalendarDate = selectedDate;
                        }
                    }
                }
            }

            if (isDomainSelected)
            {
                LiteralScript.Text = "<script>\n" + "CloseWindow();\n" + "</script>";
            }
            else
            {
                lblMessage.Text = "Please select Domain";
                lblMessage.Visible = true;
            }
        }

        protected void RepeaterDomain_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            RepeaterItem item = e.Item;
            HiddenField domainTypeID = (HiddenField)item.FindControl("domainTypeID");
            if (domainTypeID != null)
            {
                if (int.Parse(domainTypeID.Value) == (int)DomainType.Domain)
                {
                    item.FindControl("domainSelect").Visible = false;
                }
                else
                {
                    item.FindControl("Description").Visible = false;
                }
            }
        }
    }
}
