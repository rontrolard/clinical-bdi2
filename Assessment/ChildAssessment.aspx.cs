using System;
using System.Data;
using System.Web.UI.WebControls;
using BDI2Core.Common;
using BDI2Web.Common_Code;

namespace BDI2Web.Assessment
{
    public partial class ChildAssessment : SecureBasePage
    {
        public DateTime g_testDate = DateTime.MinValue;
        public DateTime s_testDate = DateTime.MinValue;

        protected void Page_Load(object sender, EventArgs e)
        {
            AssessmentCommon.ClearSessionStudentAssessment();
            Session["PAGE_21_ScoreTypeID"] = null;
            Session["PAGE_21_Normid"] = null;
            //Session["PAGE_21_Normid"] = null;
            if (!IsPostBack)
            {
                if (UserCanAssessChild())
                {
                    if (Session["PAGE_20_studentID"] != null)
                    {
                        StudentNav.PageID = PageID;
                        StudentNav.StudentID = Session["PAGE_20_studentID"];
                        StudentNav.Privileges = SessionData.Staff.Privileges;
                        LabelStudentID.Text = Session["PAGE_20_studentID"].ToString();
                        //Session["PAGE_20_studentID"] = null; we need to keep this session until student selection changed
                        BDI2Core.Common.Student child = new BDI2Core.Common.Student(Convert.ToInt32(LabelStudentID.Text));
                        child.LoadFromDB();
                        LabelAssessmentTitle.Text = "Assessment(s) for " + child.GetName();
                    }

                    DataSet ds = AssessmentHelper.GetAssessmentListForStudent(int.Parse(LabelStudentID.Text), (int)(AssessmentType.All));
                    
                    //get the latest test date
                    DateTime testDate;
                    string command = "";
                    string completeStatus = "";
                    string assmentType = "";
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        command = dr["commandArgument"].ToString();
                        completeStatus = dr["completeStatus"].ToString();
                        assmentType = dr["assessmentTypeID"].ToString();
                        if (command == "DELETE" && (int.Parse(completeStatus) == (int)AssessmentStatus.Complete))
                        {
                            testDate = DateTime.Parse(dr["firstTestDate"].ToString());

                            if (int.Parse(assmentType) == (int)AssessmentType.Full)
                            {
                                if (testDate > g_testDate)
                                {
                                    g_testDate = testDate;
                                }
                            }
                            else if (int.Parse(assmentType) == (int)AssessmentType.Screener)
                            {
                                if (testDate > s_testDate)
                                {
                                    s_testDate = testDate;
                                }
                            }
                        }
                    }

                    RepeaterAssessment.DataSource = ds;
                    RepeaterAssessment.DataBind();
                    RepeaterAssessment.Visible = ds.Tables[0].DefaultView.Count > 0;
                    ButtonMerge.Visible = ds.Tables[0].DefaultView.Count > 0;
                    LabelAssessmentType.Text = ((int)AssessmentType.All).ToString();
                    if (ds.Tables[0].DefaultView.Count ==0)
                    {
                        NoChildrenLabel.Text = "There are no assessment(s) for this child";
                    }
                    else
                    {
                        NoChildrenLabel.Text = "";
                    }
                }
            }
        }

        protected void ButtonNewFull_Click(object sender, EventArgs e)
        {
            Session["PAGE_21_assessmentTypeID"] = (int)AssessmentType.Full;
            Response.Redirect("EditScreener.aspx");
        }

        protected void ButtonNewScreener_Click(object sender, EventArgs e)
        {
            Session["PAGE_21_assessmentTypeID"] = (int)AssessmentType.Screener;
            Response.Redirect("EditScreener.aspx");
        }

        protected void RadioButtonListType_SelectedIndexChanged(object sender, EventArgs e)
        {
            int assessmentType = int.Parse(RadioButtonListType.SelectedValue.ToString());
            LabelAssessmentType.Text = assessmentType.ToString();

            DataSet ds = AssessmentHelper.GetAssessmentListForStudent(int.Parse(LabelStudentID.Text), assessmentType);
            RepeaterAssessment.DataSource = ds;
            RepeaterAssessment.DataBind();
            RepeaterAssessment.Visible = ds.Tables[0].DefaultView.Count > 0;
            ButtonMerge.Visible = ds.Tables[0].DefaultView.Count > 0;
            ButtonMerge.Enabled = !(assessmentType == (int)AssessmentType.Deleted);
            if (ds.Tables[0].DefaultView.Count == 0)
            {
                string typeLabel = "";
                switch(assessmentType)
                {
                    case ((int)AssessmentType.Deleted):
                        typeLabel = "deleted";
                        break;
                    case ((int)AssessmentType.Full):
                        typeLabel = "complete";
                        break;
                    case ((int)AssessmentType.Screener):
                        typeLabel = "screener";
                        break;
                    
                }
                NoChildrenLabel.Text = "There are no " + typeLabel + " assessment(s) for this child";
            }
            else
            {
                NoChildrenLabel.Text = "";
            }

            ClearStatusMessageLabel();
        }

        protected void RepeaterAssessment_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            string command = e.CommandArgument.ToString();
            int studentAssessmentID = int.Parse(((HiddenField)e.Item.FindControl("studentAssessmentID")).Value);

            if (command == "EDIT")
            {
                int assessmentTypeID = int.Parse(((HiddenField)e.Item.FindControl("assessmentTypeID")).Value);

                Session["PAGE_21_assessmentTypeID"] = assessmentTypeID;
                Session["Page_21_studentAssessmentID"] = studentAssessmentID;
                Response.Redirect("EditScreener.aspx");
            }
            else // DELETE
            {
                if (studentAssessmentID > 0)
                {
                    StudentAssessment assessment = new StudentAssessment(studentAssessmentID);
                    if (command=="UNDELETE")
                    {
                        assessment.DeleteRecord(false); //un-delete
                    }
                    else
                    {
                        assessment.DeleteRecord(true); 
                    }
                }
                Response.Redirect("ChildAssessment.aspx");
            }
        }

        protected void ButtonMerge_Click(object sender, EventArgs e)
        {
            string errorMessage = "";
            AssessmentMerge merge = new AssessmentMerge();

            foreach (RepeaterItem item in RepeaterAssessment.Items)
            {
                HiddenField studentAssessmentID = (HiddenField)item.FindControl("studentAssessmentID");
                if (studentAssessmentID != null)
                {
                    CheckBox selection = (CheckBox)item.FindControl("MergeSelect");
                    if (selection.Checked)
                    {
                        HiddenField assessmentTypeID = (HiddenField)item.FindControl("assessmentTypeID");

                        AssessmentMergeItem mergeItem = new AssessmentMergeItem();
                        mergeItem.StudentAssessmentID = int.Parse(studentAssessmentID.Value.ToString());
                        mergeItem.AssessmentTypeID = int.Parse(assessmentTypeID.Value.ToString());

                        merge.SelectedList.Add(mergeItem);
                    }
                }
            }

            errorMessage = merge.Validate();
            if (errorMessage != "")
            {
                SetErrorMessage(errorMessage);
            }
            else
            {
                merge.Merge( (int)SessionData.Staff.StaffID );
                Response.Redirect("ChildAssessment.aspx");
            }
        }

        #region private functions

        private bool UserCanAssessChild()
        {
            bool canAssessChild = SessionData.Staff.Privileges.Contains(Privileges.InputAssessment);

            AccessDeniedLabel.Visible = !canAssessChild;
            AssessChildPanel.Visible = canAssessChild;
            ButtonNewFull.Enabled = canAssessChild;
            ButtonNewScreener.Enabled = canAssessChild;
  
            return canAssessChild;
        }

        #endregion

        #region Overridden Functions

        protected override void SetPageID()
        {
            PageID = 20; //Value from Page Table in db
        }

        #endregion Overridden Functions
    }
}
