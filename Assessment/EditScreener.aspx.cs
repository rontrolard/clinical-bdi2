using System;
using System.Web;
using System.Data;
using System.Web.UI.WebControls;
using BDI2Core.Common;
using BDI2Web.Common_Code;
using BDI2Core.Data;

namespace BDI2Web.Assessment
{
    public partial class EditScreener : SecureBasePage
    {
        #region Event Functions
        protected void Page_Load(object sender, EventArgs e)
        {
            StudentAssessment sa = null;
            string assessmentName = "";

            if (!IsPostBack)
            {
                Page.DataBind();

                ButtonSubmit.Style.Add("display", "none"); //hide this button

                bool canAssessChild = SessionData.Staff.Privileges.Contains(Privileges.InputAssessment);
                if (!canAssessChild)
                {
                    ButtonSave.Enabled = false;
                    ButtonDelete.Enabled = false;
                }

                if (Session["PAGE_20_studentID"] != null)
                {
                    LabelStudentID.Text = Session["PAGE_20_studentID"].ToString();
                }

                if (Session["PAGE_21_assessmentTypeID"] != null)
                {
                    LabelAssessmentTypeID.Text = Session["PAGE_21_assessmentTypeID"].ToString();
                    //Session["PAGE_21_assessmentTypeID"] = null; we need to keep this session until it is changed form the assessment home page.

                    if (int.Parse(LabelAssessmentTypeID.Text) == (int)AssessmentType.Screener)
                    {
                        assessmentName = "Screener";
                        screener1.Visible = true;
                        screener2.Visible = true;
                    }
                    else
                    {
                        assessmentName = "Complete Assessment";
                        complete1.Visible = true;
                        complete2.Visible = true;
                    }
                }

                if (Session["PAGE_21_studentAssessmentID"] != null)
                {
                    LabelStudentAssessmentID.Text = Session["PAGE_21_StudentAssessmentID"].ToString();
                    //Session["PAGE_21_studentAssessmentID"] = null; we need to keep this session until it is changed form the assessment home page.
                    //sa = new StudentAssessment(int.Parse(LabelStudentAssessmentID.Text));
                    //sa.LoadFromDB();

                    LabelScreenerTitle.Text = "Edit " + assessmentName + " for ";
                }
                else
                {
                    LabelScreenerTitle.Text = "New " + assessmentName + " for ";
                    //hide the delete button
                    ButtonDelete.Visible = false;
                }

                //load student name etc.
                BDI2Core.Common.Student student = new BDI2Core.Common.Student(int.Parse(LabelStudentID.Text));
                student.LoadFromDB();
                LabelScreenerTitle.Text += student.GetName();
                LabelAge.Text = "Birthdate: " + student.Birthdate.ToShortDateString();

                BuildPage();

                //show / hide
                if (int.Parse(LabelAssessmentTypeID.Text) == (int)AssessmentType.Screener)
                {
                    LabeSpanishIO.Visible = false;
                    LabeAE.Visible = false;
                    LabelPR.Visible = false;
                    LabelSS.Visible = false;
                }
            }
            sa = AssessmentCommon.GetSessionStudentAssessment();
            if (sa != null)
            {
                ButtonTotal.Enabled = !sa.DirtyFlag;
                ButtonProgramNote.Enabled = !sa.DirtyFlag;
                ButtonObservation.Enabled = !sa.DirtyFlag;

            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            StudentAssessment sa = AssessmentCommon.GetSessionStudentAssessment();
            if (sa != null)
            {
                //re-populate repeater
                DataSet ds = sa.FormatDomainList();
                RepeaterDomain.DataSource = ds;
                RepeaterDomain.DataBind();

                if (sa.AssessmentTypeID == (int)AssessmentType.Screener)
                {
                    AssmtTypeLiteral.Text = "Screener";
                }
                else
                {
                    AssmtTypeLiteral.Text = "Complete";
                }
            }
        }

        protected void RepeaterDomain_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            StudentAssessment sa = AssessmentCommon.GetSessionStudentAssessment();

            RepeaterItem item = e.Item;
            HiddenField domainTypeID = (HiddenField)item.FindControl("domainTypeID");
            Image scoreTypeImage = (Image)item.FindControl("ScoreTypeImage");
            DataRowView drv = (DataRowView)item.DataItem;
            CheckBox isSpanish = (CheckBox)item.FindControl("CheckBoxSpanish");
            string assessmentType = string.Empty;
            if (screener1.Visible)
                assessmentType = "SCREENER";
            else
                assessmentType = "COMPLETE";
            if (domainTypeID != null)
            {
                isSpanish.Attributes.Add("onclick", "SpanishAlert('" + isSpanish.ClientID.ToString() + "','" + assessmentType + "');");

                scoreTypeImage.Visible = false;
                if (drv["scoreType"].ToString().ToUpper() == "(DET)")
                {
                    scoreTypeImage.ImageUrl = "../Images/item_details.gif";
                    scoreTypeImage.Visible = true;
                }
                else if (drv["scoreType"].ToString().ToUpper() == "(RAW)")
                {
                    scoreTypeImage.ImageUrl = "../Images/raw_scores.gif";
                    scoreTypeImage.Visible = true;
                }

                if (int.Parse(domainTypeID.Value) == (int)DomainType.Domain)
                {
                    item.FindControl("domainSelect").Visible = false;
                    item.FindControl("TextBoxTestDate").Visible = false;
                    item.FindControl("TextBoxStaff").Visible = false;
                    item.FindControl("TextBoxScore").Visible = false;
                    item.FindControl("CheckBoxSpanish").Visible = false;
                    item.FindControl("CheckBoxSpanishIO").Visible = false;
                    item.FindControl("LabelAEScore").Visible = false;
                    item.FindControl("LabelPRScore").Visible = false;
                    item.FindControl("LabelSSScore").Visible = false;
                }
                else
                {
                    ((TextBox)item.FindControl("TextBoxScore")).Enabled = (!sa.DetailFlag && RadioButtonListInputType.SelectedIndex == 0);
                }

                if (int.Parse(LabelAssessmentTypeID.Text) == (int)AssessmentType.Screener)
                {
                    item.FindControl("CheckBoxSpanishIO").Visible = false;
                    item.FindControl("LabelAEScore").Visible = false;
                    item.FindControl("LabelPRScore").Visible = false;
                    item.FindControl("LabelSSScore").Visible = false;

                    if (int.Parse(domainTypeID.Value) == (int)DomainType.Domain)
                    {
                        item.FindControl("Description").Visible = false;
                    }
                }
            }
        }

        protected void ButtonObservation_Click(object sender, EventArgs e)
        {
            GetPageValue("SUBMIT");
            Response.Redirect("TestObservations.aspx");
        }
        protected void CheckPageControls()
        {
            CheckBox control;
            TextBox date;
            TextBox staffName;
            TextBox score;
            CheckBox isSpanish;
            CheckBox isSpanishIO;

            foreach (RepeaterItem item in RepeaterDomain.Items)
            {
                control = (CheckBox)item.FindControl("domainSelect");
                date = (TextBox)item.FindControl("TextBoxTestDate");
                staffName = (TextBox)item.FindControl("TextBoxStaff");
                score = (TextBox)item.FindControl("TextBoxScore");
                isSpanish = (CheckBox)item.FindControl("CheckBoxSpanish");
                isSpanishIO = (CheckBox)item.FindControl("CheckBoxSpanishIO");

                if ((!String.IsNullOrEmpty(date.Text)
                        || !String.IsNullOrEmpty(staffName.Text)
                        || !String.IsNullOrEmpty(score.Text)
                        || isSpanish.Checked
                        || isSpanishIO.Checked)
                        && (!control.Checked && control.Visible))
                {
                    control.Checked = true;

                }

            }
        }
        protected void ButtonSave_Click(object sender, EventArgs e)
        {
            int normtype = RadioButtonNorms.SelectedIndex + 1;
            StudentAssessment sa = AssessmentCommon.GetSessionStudentAssessment();

            if (sa.NormType != normtype && sa.StudentAssessmentID > 0) //Check if user changed the norm then need to save everything on edit
            {

                CheckPageControls();
            }
            sa.ScoreTypeID = RadioButtonListInputType.SelectedIndex + 1;
            sa = GetPageValue("SAVE");
            if (sa != null)
            {

                if (sa.HasSelection())
                {
                    if (ValidatePageValue())
                    {
                        ButtonTotal.Enabled = true;
                        ButtonProgramNote.Enabled = true;
                        ButtonObservation.Enabled = true;
                        sa.DirtyFlag = true;
                        sa.NormType = normtype;
                       // sa.ScoreTypeID = 
                        sa.CalculateDate();
                        sa.CalculateScore();
                        sa.EditRecord();
                        Lookups.DeleteCopyStudentAssessment(sa.StudentAssessmentID, null);
                        if (sa.FirstTestDate > DateTime.MinValue && sa.FirstTestDate < DateTime.MaxValue)
                        {
                            LabelFirstDate.Text = "First Test Date: " + sa.FirstTestDate.ToShortDateString();
                        }

                        //saved, show the delete button now.
                        ButtonDelete.Visible = true;
                    }
                    else
                    {
                        if (sa.StudentAssessmentID <= 0)
                        {
                            //Invalid assessment, disable Totals Button.
                            ButtonTotal.Enabled = false;
                            ButtonProgramNote.Enabled = false;
                            ButtonObservation.Enabled = false;
                        }
                    }
                    ClearStatusMessageLabel();
                }
                else
                {
                    SetErrorMessage("A Domain/Subdomain must be selected in order to Save/Delete. Please select a Domain/Subdomain.");
                }
            }
        }


        protected int GetScoreCopy(int normtypeid, int StudentAssessmentID)
        {
            return 0;
        }
        protected void ButtonNext_Click(object sender, EventArgs e)
        {
            StudentAssessment sa = GetPageValue("NEXT");

            if (sa != null)
            {
                if (ValidatePageValue())
                {
                    if (sa.HasSelection())
                    {
                        sa.CurrentDomainID = int.MinValue; //reset the current position
                        Response.Redirect("EditAssessment.aspx");
                    }
                    else
                    {
                        SetErrorMessage("A Domain/Subdomain must be selected in order to Save/Delete. Please select a Domain/Subdomain.");
                    }
                }
            }
        }

        protected void ButtonCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("ChildAssessment.aspx");
        }

        protected void ButtonDelete_Click(object sender, EventArgs e)
        {
            StudentAssessment sa = GetPageValue("DELETE");

            if (sa != null)
            {
                if (sa.HasSelection())
                {
                    sa.DirtyFlag = true;
                    sa.CalculateDate();
                    sa.CalculateScore();
                    sa.EditRecord();

                    Response.Redirect("EditScreener.aspx");
                }
                else
                {
                    SetErrorMessage("A Domain/Subdomain must be selected in order to Save/Delete. Please select a Domain/Subdomain.");
                }
            }
        }

        protected void ButtonSubmit_Click(object sender, EventArgs e) // hidden button, submit the test date and exzaminer name.
        {
            GetPageValue("SUBMIT");
        }

        protected void RadioButtonListInputType_SelectedIndexChanged(object sender, EventArgs e)
        {
            StudentAssessment sa = AssessmentCommon.GetSessionStudentAssessment();
            Session["PAGE_21_ScoreTypeID"] = RadioButtonListInputType.SelectedIndex;
            if (RadioButtonListInputType.Items[0].Selected)
            {
                if (sa != null)
                {
                    sa.DetailFlag = false;
                    sa.ScoreTypeID = 1;
                }
                ButtonSave.Visible = true;
                ButtonNext.Visible = false;
                //SpacerLabel.Text = "&nbsp;&nbsp;";
            }
            else
            {
                if (sa != null) 
                {  
                  sa.DetailFlag = true;
                  sa.ScoreTypeID = 2;
                }
                ButtonSave.Visible = true;// Session["PAGE_21_studentAssessmentID"] != null && sa.ScoreTypeID == (int)ScoreType.ItemDetails;
                ButtonNext.Visible = true;
                SpacerLabel.Text = "";
            }

            GetPageValue("SUBMIT");
        }

        protected void CheckBoxAll_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox all = sender as CheckBox;

            foreach (RepeaterItem item in RepeaterDomain.Items)
            {
                HiddenField domainTypeID = (HiddenField)item.FindControl("domainTypeID");
                CheckBox control = (CheckBox)item.FindControl("domainSelect");
                if (domainTypeID != null)
                {
                    if (int.Parse(domainTypeID.Value) == (int)DomainType.Subdomain)
                    {
                        control.Checked = all.Checked;
                    }
                }
            }

            GetPageValue("SUBMIT");
        }
        #endregion

        #region Page Functions
        protected void BuildPage()
        {
            LabelFirstDate.Text = "First Test Date: ";

            //set session assessment object
            StudentAssessment sa = AssessmentCommon.GetSessionStudentAssessment();

            if (sa == null)
            {
                if (LabelStudentAssessmentID.Text == "") //need a new one
                {
                    BDI2Core.Common.Assessment assessment = new BDI2Core.Common.Assessment();
                    assessment.LoadFromDB(int.Parse(LabelAssessmentTypeID.Text));
                    assessment.LoadDomains(false);
                    sa = new StudentAssessment(assessment);
                   
                    sa.StudentID = int.Parse(LabelStudentID.Text);
                    //Session["NORMSMODE"] = "NEW";
                }
                else
                {
                    sa = new StudentAssessment(int.Parse(LabelStudentAssessmentID.Text));
                    sa.LoadFromDB();
                    sa.LoadDomains(false);

                    //Session["NORMSMODE"] = "EDIT";
                }
                AssessmentCommon.SetSessionStudentAssessment(sa);
            }

            LabelFirstDate.Text += (sa.FirstTestDate == DateTime.MinValue ? "" : sa.FirstTestDate.ToShortDateString());

            //populate repeater
            DataSet ds = sa.FormatDomainList();
            RepeaterDomain.DataSource = ds;
            RepeaterDomain.DataBind();

            //detail input selection
            //RadioButtonListInputType.Items[0].Selected = !sa.DetailFlag;
            //RadioButtonListInputType.Items[1].Selected = sa.DetailFlag;
           
            sa.DetailFlag = sa.ScoreTypeID == 2;
            //if (Session["PAGE_21_Normid"] != null)
            //{
            //    RadioButtonNorms.SelectedIndex = int.Parse(Session["PAGE_21_Normid"].ToString());
            //}
            //else
            //{
             
            //}
            //ButtonSave.Visible = !sa.DetailFlag;

            if (Session["PAGE_21_Normid"] != null)
            {
                sa.NormType = int.Parse(Session["PAGE_21_Normid"].ToString()) + 1;
                RadioButtonNorms.SelectedIndex = int.Parse(Session["PAGE_21_Normid"].ToString());
            }
            else if (Session["PAGE_21_NormidDefault"] != null && Session["PAGE_21_studentAssessmentID"]==null)
            {
                RadioButtonNorms.SelectedIndex = int.Parse(Session["PAGE_21_NormidDefault"].ToString());
                sa.NormType = int.Parse(Session["PAGE_21_NormidDefault"].ToString()) + 1;
            }

            else
            {
                RadioButtonNorms.SelectedIndex = sa.NormType - 1;
            }

            if (Session["PAGE_21_ScoreTypeID"] != null)
            {
                RadioButtonListInputType.SelectedIndex = int.Parse(Session["PAGE_21_ScoreTypeID"].ToString());
                ButtonNext.Visible = true;
                SpacerLabel.Text = "";
            }
            else
            {
                RadioButtonListInputType.SelectedIndex = sa.ScoreTypeID - 1;
                ButtonNext.Visible = sa.DetailFlag;
            }
            foreach (RepeaterItem item in RepeaterDomain.Items)
            {
                TextBox score = (TextBox)item.FindControl("TextBoxScore");
                score.Enabled = !sa.DetailFlag;
            }

        }

        protected bool ValidatePageValue()
        {
            bool valid = true;

            StudentAssessment sa = AssessmentCommon.GetSessionStudentAssessment();

            if (sa != null)
            {
                ClearStatusMessageLabel();

                sa.CalculateDate(); //we user the assessment's first test date now.
                valid = sa.IsValid;
                if (!valid)
                {
                    SetErrorMessage((string)sa.ErrorMessages[0]);
                }
                else
                {
                    ClearStatusMessageLabel();
                }
            }
            else
            {
                valid = false;
            }

            return valid;
        }

        protected StudentAssessment GetPageValue(string strFor)
        {
            StudentAssessment sa = AssessmentCommon.GetSessionStudentAssessment();


            if (sa != null)
            {

                ClearStatusMessageLabel();

                int normtype = RadioButtonNorms.SelectedIndex + 1;
                //sa.NormType = normtype;
                foreach (RepeaterItem item in RepeaterDomain.Items)
                {
                    HiddenField domainID = (HiddenField)item.FindControl("domainID");
                    if (domainID != null)
                    {
                        StudentDomain sd = sa.FindDomain(int.Parse(domainID.Value));
                        if (sd != null)
                        {
                            sd.NormType = normtype;

                            GetPageRowValue(ref sa, ref sd, item, strFor);
                        }
                    }
                }
                ButtonTotal.Enabled = !sa.DirtyFlag;
                ButtonProgramNote.Enabled = !sa.DirtyFlag;
                ButtonObservation.Enabled = !sa.DirtyFlag;
            }

            return sa;
        }

        protected static void GetPageRowValue(ref StudentAssessment sa, ref StudentDomain sd, RepeaterItem item, string strFor)
        {
            CheckBox control = (CheckBox)item.FindControl("domainSelect");
            TextBox date = (TextBox)item.FindControl("TextBoxTestDate");
            TextBox staffName = (TextBox)item.FindControl("TextBoxStaff");
            TextBox score = (TextBox)item.FindControl("TextBoxScore");
            CheckBox isSpanish = (CheckBox)item.FindControl("CheckBoxSpanish");
            CheckBox isSpanishIO = (CheckBox)item.FindControl("CheckBoxSpanishIO");

            sd.ExValid = true;
            sd.Usage_Selection = control.Checked;

            //get value from base window
            sd.IsSpanish = isSpanish.Checked;
            if (isSpanishIO != null) sd.IsSpanishIO = isSpanishIO.Checked;

            //raw score, no validation before, so , we do a try here
            int rawScore = int.MinValue;
            try { rawScore = int.Parse(score.Text); }
            catch { }
            sd.Rawscore = rawScore;

            //test date, no validation before, so , we do a try here
            DateTime testDate = DateTime.MinValue;
            try { testDate = DateTime.Parse(date.Text); }
            catch { }
            sd.TestDate = testDate;

            //staff
            sd.StaffID = int.MinValue; //int.Parse(staffID.Value); (we do not use the ID any more)
            sd.StaffName = "" + staffName.Text;

            //get value from popup window
            if (sd.Calendar_Selection == true)
            {
                sd.Usage_Selection = true;
                sd.TestDate = sd.CalendarDate;
                //clean up
                sd.Calendar_Selection = false;
                sd.CalendarDate = DateTime.MinValue;
            }

            if (sd.Examiner_Selection == true)
            {
                sd.Usage_Selection = true;
                sd.StaffName = sd.ExaminerName;
                //clean up
                sd.Examiner_Selection = false;
                sd.ExaminerName = "";
            }

            //other buttons (Save, Next, Delete)
            if (strFor != "SUBMIT")
            {
                if (control.Checked)
                {
                    StudentDomain sdParent = null;

                    switch (strFor)
                    {
                        case "SAVE":
                            sd.DirtyFlag = true; //set dirty flag here
                            sd.ScoreTypeID = sa.ScoreTypeID;
                            //if (sd.ScoreTypeID != Int32.MinValue)
                            //sd.Usage_Selection = true;
                            sd.btnAction = "SAVE";
                            
                            if (sd.ExValidate(false, sa.StudentBirthday))
                            {
                                sd.ScoreTypeID = !sa.DetailFlag ? (int)ScoreType.Raw : (int)ScoreType.ItemDetails; //set score type here

                                sd.CompleteStatus = DomainStatus.Ceiling; //completed
                            }
                            else
                            {
                                sd.ExValid = true;
                            }
                            //SS, AE score and required flag
                            DateTime useTestDate;
                            if (sa.FirstTestDate == DateTime.MinValue) //new assessment
                            {
                                useTestDate = sd.TestDate;
                            }
                            else
                            {
                                useTestDate = (sd.TestDate < sa.FirstTestDate ? sd.TestDate : sa.FirstTestDate);
                            }
                            int months = AssessmentHelper.GetChildMonths(sa.StudentBirthday, useTestDate);
                            sd.GetSubdomainScore(sa.AssessmentTypeID, months);


                            sdParent = sa.FindDomain(sd.ParentDomainID);
                            if (sdParent != null)
                            {
                                sdParent.DirtyFlag = true; // set parent domain dirty.
                            }
                            break;
                        case "NEXT":
                            sd.btnAction = "NEXT";
                             
                            //we will set sd.DirtyFlag and score type when question is done (in case cancel)
                            break;
                        case "DELETE":
                            if (sd.CompleteStatus != DomainStatus.NotUsed)
                            {
                                sdParent = sa.FindDomain(sd.ParentDomainID);
                                if (sdParent != null)
                                {
                                    sdParent.DirtyFlag = true; // set parent domain dirty.
                                }
                                sd.DeleteRecord();
                                sd.Clear();
                                sd.DirtyFlag = false;
                                sd.Usage_Selection = true; //we did select this one.
                            }
                            else
                            {
                                sd.Clear();
                            }
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    if (sd.ResultStudentDomainID < 0) //new domain
                    {
                        sd.CompleteStatus = DomainStatus.NotUsed;
                    }

                    if (sd.CompleteStatus == DomainStatus.NotUsed)
                    {
                        //clean up
                        sd.Clear();
                    }
                    if (sd.ResultStudentDomainID > 0 && sd.DomainTypeID == (int)DomainType.Subdomain && sd.DirtyFlag) //existing subdomain
                    {
                        sd.DirtyFlag = false;
                        DataSet ds = Lookups.GetResultStudentDomain(sd.ResultStudentDomainID);
                        DataTable dt = ds.Tables[0];
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            sd.LoadFromDB(dt.Rows[0]);

                        }
                    }

                }
            }
        }
        #endregion

        #region Overridden Functions

        protected override void SetPageID()
        {
            PageID = 21; //Value from Page Table in db
        }

        #endregion Overridden Functions

        protected void RadioButtonList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session["PAGE_21_Normid"] = RadioButtonNorms.SelectedIndex;
            Session["PAGE_21_NormidDefault"] = RadioButtonNorms.SelectedIndex;

            GetPageValue("SUBMIT");
        }
    }
}
