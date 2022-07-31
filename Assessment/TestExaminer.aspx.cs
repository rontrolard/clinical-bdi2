using System;
using System.Data;
using System.Web.UI.WebControls;
using BDI2Core.Common;
using BDI2Core.Data;
using BDI2Web.Common_Code;

namespace BDI2Web.Assessment
{
    public partial class TestExaminer : SecureBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                StudentAssessment sa = AssessmentCommon.GetSessionStudentAssessment();
                if (sa != null)
                {
                    DataSet ds;
                    //staff view
                    ds = Lookups.GetStaffAsExaminer(SessionData.CustomerID.Value, SessionData.Staff.StaffID.Value,"","","");
                    GridView1.DataSource = ds;
                    GridView1.DataBind();

                    //domain list
                    sa.ClearExaminerSelection();
                    ds = sa.FormatDomainList();
                    RepeaterDomain.DataSource = ds;
                    RepeaterDomain.DataBind();
                }
            }
        }

        #region Overridden Functions

        protected override void SetPageID()
        {
            PageID = 27; //Value from Page Table in db
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

        protected void ButtonContinue_Click(object sender, EventArgs e)
        {
            if (LabelSelectedStaffID.Text != "")
            {
                StudentAssessment sa = AssessmentCommon.GetSessionStudentAssessment();
                foreach (RepeaterItem item in RepeaterDomain.Items)
                {
                    CheckBox domainSelect = (CheckBox)item.FindControl("domainSelect");
                    if (domainSelect.Checked == true)
                    {
                        int domainID = int.Parse(((HiddenField)item.FindControl("domainID")).Value);
                        StudentDomain sd = sa.FindDomain(domainID);
                        if (sd != null)
                        {
                            if (sd.DomainTypeID == (int)DomainType.Subdomain)
                            {
                                sd.Usage_Selection = true;
                                sd.Examiner_Selection = true; //clean it up in base window's post back function
                                sd.StaffID = int.MinValue; //int.Parse(LabelSelectedStaffID.Text); (we do not use StaffID any more. )
                                sd.ExaminerName = LabelSelectedStaffName.Text;
                            }
                        }
                    }
                }
            }
            LiteralScript.Text = "<script>\n" + "CloseWindow();\n" + "</script>";
        }

        protected void ButtonCancel_Click(object sender, EventArgs e)
        {
            LiteralScript.Text = "<script>\n" + "CloseWindow();\n" + "</script>";  
        }
       protected void ButtonSearch_Click(object sender, EventArgs e)
        {
            //clear selection
            LabelSelectedStaffID.Text = "";
            LabelSelectedStaffName.Text = "";

            StudentAssessment sa = AssessmentCommon.GetSessionStudentAssessment();
            if (sa != null)
            {
                //staff view

                DataSet ds = Lookups.GetStaffAsExaminer(SessionData.CustomerID.Value, SessionData.Staff.StaffID.Value, TextBoxFirstName.Text.Trim(), TextBoxLastName.Text.Trim(), TextBoxSiteName.Text.Trim());
                GridView1.DataSource = ds;
                GridView1.DataBind();
            }
        }
      protected void RadioButtonSelect_CheckedChanged(object sender, EventArgs e)
        {
          // Retrieve selected radiobutton ClientID.
          string selectedID = (sender as RadioButton).ClientID;
          //Loop through all the rows in the grid.
           foreach (GridViewRow gr in GridView1.Rows)
           {
               // Retrieve ClientID for radio button in each row.
               string rowID = ((RadioButton)gr.FindControl("RadioButtonSelect")).ClientID;
               // IF ClientID matches then retireve staffid etc..
               if (selectedID.Equals(rowID))
               {
                 int staffID = int.Parse(GridView1.DataKeys[gr.RowIndex].Value.ToString());
                 LabelSelectedStaffID.Text = "" + staffID.ToString();
                 LabelSelectedStaffName.Text = "" + gr.Cells[1].Text;
               }   
               else // If doesn't match make checked false.
               {
                 ((RadioButton)gr.FindControl("RadioButtonSelect")).Checked = false;
               }
               
           }
        }
    }
}
