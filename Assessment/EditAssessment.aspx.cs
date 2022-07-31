using System;
using System.Data;
using System.Drawing;
using System.Web.UI.WebControls;
using BDI2Core.Common;
using BDI2Web.Common_Code;

namespace BDI2Web.Assessment
{
    public partial class EditAssessment : SecureBasePage
    {
        bool reTest = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.DataBind();

                ButtonDone2.Style.Add("display", "none"); //hide this button
                ButtonOrgTest.Visible = false; //always false on first load

                bool canAssessChild = SessionData.Staff.Privileges.Contains(Privileges.InputAssessment);
                if (!canAssessChild)
                {
                    ButtonDone.Enabled = false;
                }
                
                StudentAssessment sa = AssessmentCommon.GetSessionStudentAssessment();
                if (sa != null)
                {
                    string domainStatus = sa.GetCurrentDimainStatus();
                    if (domainStatus != "END")
                    {
                        StudentDomain domain = sa.GetCurrentDomain();
                        domain.DirtyFlag = true;

                        //retest button
                        if (domain.ScoreTypeID !=  (int)ScoreType.ItemDetails || domain.CompleteStatus != DomainStatus.Ceiling 
                            || domain.IsSpanish || domain.IsSpanishIO)
                        {
                            ButtonReTest.Visible = false;
                        }

                        if (domainStatus == DomainStatus.NotUsed)
                        {
                            domain.LoadNewQuestions();
                            ButtonReTest.Visible = false;
                        }
                        else
                        {
                            //maybe switched from Raw score, need to check
                            //if (domain.ListStudentQuestions.Count <= 0)
                            {
                                domain.LoadQuestions();
                                if (domain.ListStudentQuestions.Count <= 0)
                                {
                                    domain.LoadNewQuestions();
                                    ButtonReTest.Visible = false;
                                }
                            }

                        }

                        SetPageDomainInfo(sa);

                        DataSet ds = domain.FormatQuestionSet(reTest);
                        RepeaterQuestion.DataSource = ds;
                        RepeaterQuestion.DataBind();
                    }
                    else
                    {
                        Response.Redirect("EditScreener.aspx");
                    }
                }
            }
            else
            {
                LiteralScript.Text = "";
                reTest = bool.Parse(LabelReTest.Text);
            }
        }

        protected void SetPageDomainInfo(StudentAssessment sa)
        {
            string action;
            string assessmentTypeName;

            action = (sa.StudentAssessmentID > 0? "Edit" : "New");
            assessmentTypeName = (sa.AssessmentTypeID == (int)AssessmentType.Full? "Complete Assessment" : "Screener");
            BDI2Core.Common.Student st = new BDI2Core.Common.Student(sa.StudentID);
            st.LoadFromDB();
            LabelQuestionTitle.Text = action + " " + assessmentTypeName + " for " + st.GetName();
            if (sa.AssessmentTypeID == (int)AssessmentType.Screener)
            {
                LabelSubdomainTitle.Visible = false;
            }

            StudentDomain sd = sa.GetCurrentDomain();
            if (sd != null)
            {
                if (sa.AssessmentTypeID == (int)AssessmentType.Full)
                {
                    StudentDomain sdParent = sa.GetCurrentParentDomain();
                    LabelDomain.Text = sdParent.Description;
                    LabelSubdomain.Text = sd.Description;
                }
                else
                {
                    LabelDomain.Text = sd.Description;
                    LabelSubdomain.Text = "";
                }

                LabelDate.Text = sd.TestDate.ToShortDateString();
                LabelAgeMonths.Text = AssessmentHelper.GetChildMonths(st.Birthdate, sd.TestDate).ToString();
                LabelAge.Text = LabelAgeMonths.Text;

                int childMonth = AssessmentHelper.GetChildMonths(sa.StudentBirthday, sa.FirstTestDate);
                sd.CalculateBC(reTest, childMonth);
                LabelBasal.Text = sd.Basal;
                LabelCeiling.Text = sd.Ceiling;
            }
        }

        protected void SetSessionDomainInfo(StudentDomain currentDomain, int childMonth)
        {
            foreach (RepeaterItem item in RepeaterQuestion.Items)
            {
                HiddenField questionID = (HiddenField)item.FindControl("questionID");
                if (questionID != null)
                {
                    RadioButtonList sList = (RadioButtonList)item.FindControl("RadioButtonListScore");
                    int score = (sList.SelectedItem == null ? int.MinValue : int.Parse(sList.SelectedValue));

                    RadioButtonList pList = (RadioButtonList)item.FindControl("RadioButtonListProcdure");
                    int procedureID = (pList.SelectedItem == null ? int.MinValue : int.Parse(pList.SelectedValue));

                    HiddenField clearRow = (HiddenField)item.FindControl("HiddenFieldClear");

                    if (score >= 0) // have score selection for this question
                    {
                        currentDomain.SetQuestionValue(int.Parse(questionID.Value), score, procedureID, reTest); //set question dirty in this function
                        clearRow.Value = "0"; //resent
                    }
                    else
                    {
                        if (clearRow.Value.ToString() == "1")
                        {
                            currentDomain.ClearQuestionValue(int.Parse(questionID.Value), reTest);
                            clearRow.Value = "0"; //resent
                            pList.SelectedIndex = -1;
                        }
                        else
                        {
                            if (procedureID >= 0) // have proc selection for this question
                            {
                                currentDomain.SetQuestionValue(int.Parse(questionID.Value), score, procedureID, reTest); //set question dirty in this function
                            }
                        }
                    }
                    /*
                    if (score >= 0 || procedureID >= 0) // have selection for this question
                    {
                        currentDomain.SetQuestionValue(int.Parse(questionID.Value), score, procedureID, reTest); //set question dirty in this function
                    }
                    else // for the new "clear" link
                    {
                        currentDomain.ClearQuestionValue(int.Parse(questionID.Value), reTest);
                    }
                    */
                }
            }

            currentDomain.CalculateBC(reTest, childMonth); //also calclate the domain completion status.
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            StudentAssessment sa = AssessmentCommon.GetSessionStudentAssessment();
            if (sa != null)
            {
                StudentDomain currentDomain = sa.GetCurrentDomain();
                if (currentDomain != null)
                {
                    DataSet ds = currentDomain.FormatQuestionSet(reTest);

                    RepeaterQuestion.DataSource = ds;
                    RepeaterQuestion.DataBind();
                }
            }
        }

        protected void RepeaterQuestion_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            Color grayColor = Color.DimGray;

            string procedureName;
            string scoreName;

            string controlName = "";
            HiddenField procedureSelection = null;
            HiddenField scoreSelection = null;

            //age color
            HiddenField developmantAge = (HiddenField)e.Item.FindControl("developmentAgeMax");
            if (developmantAge != null)
            {
                int developmantMonths = int.Parse(developmantAge.Value);
                int studentMonths = int.Parse(LabelAgeMonths.Text);
                if (developmantMonths <= studentMonths)
                {
                    ((Label)e.Item.FindControl("questionItemNumber")).ForeColor = grayColor;
                    ((Label)e.Item.FindControl("questionItemNumber")).Font.Bold = false;
                    ((Label)e.Item.FindControl("questionAge")).ForeColor = grayColor;
                    ((Label)e.Item.FindControl("questionAge")).Font.Bold = false;
                    ((Label)e.Item.FindControl("description")).ForeColor = grayColor;
                    ((Label)e.Item.FindControl("description")).Font.Bold = false;
                }
            }

            //disable and set selection of the radio buttons
            RadioButtonList rList = (RadioButtonList)e.Item.FindControl("RadioButtonListProcdure");
            if (rList != null) // not header
            {
                if (reTest)
                {
                    procedureName = "reTestAdminProcedureID";
                }
                else
                {
                    procedureName = "adminProcedureID";
                }
                procedureSelection = (HiddenField)e.Item.FindControl(procedureName);

                foreach (ListItem item in rList.Items)
                {
                    bool selected = false;
                    switch (item.Value)
                    {
                        case "1": //"S"
                            controlName = "structured";
                            if (int.Parse(procedureSelection.Value) == (int)AdminProcedure.Structure) selected = true;
                            break;
                        case "2": //"O"
                            controlName = "observation";
                            if (int.Parse(procedureSelection.Value) ==(int)AdminProcedure.Observation) selected = true;
                            break;
                        case "3": //"I"
                            controlName = "interview";
                            if (int.Parse(procedureSelection.Value) == (int)AdminProcedure.Interview) selected = true;
                            break;
                    }
                    HiddenField procedureEnable = (HiddenField)e.Item.FindControl(controlName);

                    item.Enabled = bool.Parse(procedureEnable.Value);
                    item.Selected = selected;
                }
            }

            rList = (RadioButtonList)e.Item.FindControl("RadioButtonListScore");
            if (rList != null) // not header
            {
                if (reTest)
                {
                    scoreName = "reTestScore";
                }
                else
                {
                    scoreName = "score";
                }
                scoreSelection = (HiddenField)e.Item.FindControl(scoreName);
                foreach (ListItem item in rList.Items)
                {
                    bool selected = false;
                    switch (item.Value)
                    {
                        case "0":
                            controlName = "score0";
                            if (scoreSelection.Value == "0") selected = true;
                            break;
                        case "1":
                            controlName = "score1";
                            if (scoreSelection.Value == "1") selected = true;
                            break;
                        case "2":
                            controlName = "score2";
                            if (scoreSelection.Value == "2") selected = true;
                            break;
                    }

                    HiddenField scoreEnable = (HiddenField)e.Item.FindControl(controlName);

                    item.Enabled = bool.Parse(scoreEnable.Value);
                    item.Selected = selected;
                }
            }

            //set validation marks
            Label labelValidation = (Label)e.Item.FindControl("LabelValidation");
            if (labelValidation != null)
            {
                bool valid = false;
                if(scoreSelection != null && procedureSelection != null)
                {
                    if ((int.Parse(scoreSelection.Value) >= 0 && int.Parse(procedureSelection.Value) >= 0) ||
                       (int.Parse(scoreSelection.Value) < 0 && int.Parse(procedureSelection.Value) < 0))
                    {
                        valid = true;
                    }
                }

                if (valid)
                {
                    labelValidation.Text = "&nbsp;";
                }
            }
        }

        protected void ButtonClear_Click(object sender, EventArgs e)
        {
            StudentAssessment sa = AssessmentCommon.GetSessionStudentAssessment();
            if (sa != null)
            {
                StudentDomain currentDomain = sa.GetCurrentDomain();
                if (currentDomain != null)
                {
                    currentDomain.ClearQuestions(reTest); //will set dirty flag to true here.
                }
            }
            
            LabelBasal.Text = "";
            LabelCeiling.Text = "";

            ClearStatusMessageLabel();
        }

        protected void ButtonCancel_Click(object sender, EventArgs e)
        {
            StudentAssessment sa = AssessmentCommon.GetSessionStudentAssessment();
            if (sa != null)
            {
                StudentDomain currentDomain = sa.GetCurrentDomain();
                if (currentDomain != null)
                {
                    currentDomain.ListStudentQuestions.Clear();
                }
            }
            Response.Redirect("EditScreener.aspx");
        }

        protected bool SaveAndValidInput()
        {
            StudentAssessment sa = AssessmentCommon.GetSessionStudentAssessment();            

            if (sa != null)
            {
                StudentDomain currentDomain;
                currentDomain = sa.GetCurrentDomain();
                if (currentDomain != null)
                {
                    int childMonth = AssessmentHelper.GetChildMonths(sa.StudentBirthday, sa.FirstTestDate);
                    SetSessionDomainInfo(currentDomain, childMonth);
                }
            }

            //check validation marks
            bool hasInvalidRow = false;
            foreach (RepeaterItem item in RepeaterQuestion.Items)
            {
                RadioButtonList sList = (RadioButtonList)item.FindControl("RadioButtonListScore");
                int score = (sList.SelectedValue == "" ? int.MinValue : int.Parse(sList.SelectedValue));

                RadioButtonList pList = (RadioButtonList)item.FindControl("RadioButtonListProcdure");
                int procedureID = (pList.SelectedValue == "" ? int.MinValue : int.Parse(pList.SelectedValue));

                if ((score >= 0 && procedureID < 0) || (score < 0 && procedureID >= 0))
                {
                    hasInvalidRow = true;
                    break;
                }
            }

            if (hasInvalidRow)
            {
                SetErrorMessage("Please select both score and procedure on the row(s) marked with \"<\"");
            }
            else
            {
                ClearStatusMessageLabel();
            }

            return (!hasInvalidRow);
        }


        #region Overridden Functions

        protected override void SetPageID()
        {
            PageID = 23; //Value from Page Table in db
        }

        #endregion Overridden Functions

        protected void ButtonReTest_Click(object sender, EventArgs e)
        {
            if (SaveAndValidInput())
            {

                LabelReTest.Text = "true";
                reTest = true;
                ButtonOrgTest.Visible = true;
                ButtonReTest.Visible = false;
                LabelSpanshRetest.Visible = true;

                StudentAssessment sa = AssessmentCommon.GetSessionStudentAssessment();
                if (sa != null)
                {
                    StudentDomain currentDomain = sa.GetCurrentDomain();
                    if (currentDomain != null)
                    {
                        int childMonth = AssessmentHelper.GetChildMonths(sa.StudentBirthday, sa.FirstTestDate);
                        currentDomain.CalculateBC(reTest, childMonth);
                        LabelBasal.Text = currentDomain.ReTestBasal;
                        LabelCeiling.Text = currentDomain.ReTestCeiling;
                    }
                }
            }
        }

        protected void ButtonOrgTest_Click(object sender, EventArgs e)
        {
            if (SaveAndValidInput())
            {
                LabelReTest.Text = "false";
                reTest = false;
                ButtonOrgTest.Visible = false;
                ButtonReTest.Visible = true;
                LabelSpanshRetest.Visible = false;
                StudentAssessment sa = AssessmentCommon.GetSessionStudentAssessment();
                if (sa != null)
                {
                    StudentDomain currentDomain = sa.GetCurrentDomain();
                    if (currentDomain != null)
                    {
                        int childMonth = AssessmentHelper.GetChildMonths(sa.StudentBirthday, sa.FirstTestDate);
                        currentDomain.CalculateBC(reTest, childMonth);
                        LabelBasal.Text = currentDomain.Basal;
                        LabelCeiling.Text = currentDomain.Ceiling;
                    }
                }
            }
        }

        protected void ButtonBC_Click(object sender, EventArgs e)
        {
            if (SaveAndValidInput())
            {
                StudentAssessment sa = AssessmentCommon.GetSessionStudentAssessment();
                if (sa != null)
                {
                    StudentDomain currentDomain = sa.GetCurrentDomain();
                    if (currentDomain != null)
                    {
                        //change label
                        if (reTest)
                        {
                            LabelBasal.Text = currentDomain.ReTestBasal;
                            LabelCeiling.Text = currentDomain.ReTestCeiling;
                        }
                        else
                        {
                            LabelBasal.Text = currentDomain.Basal;
                            LabelCeiling.Text = currentDomain.Ceiling;
                        }

                        //set error messages
                        this.ClearStatusMessageLabel();
                        if (currentDomain.JumpFlag == true)
                        {
                            SetErrorMessage("Items may not be skipped. A raw score and entry method must be entered for each item between the basal and the ceiling.");
                        }
                        else
                        {
                            if (currentDomain.StartLateFlag == true)
                            {
                                SetErrorMessage("The first question answered is a question after the designed first question, no basal or ceiling will be achieved.");
                            }
                            else
                            {
                                if (reTest)
                                {
                                    if (currentDomain.ReTestBasal == "" && currentDomain.ReTestCeiling == "")
                                    {
                                        SetErrorMessage("There is no basal or ceiling. The basal level is reached when a child scores 2 points on the 3 consecutive lowest administered items or first item. The ceiling level is reached when a child scores 0 points on 3 consecutive items or reaches the last item.");
                                    }
                                    else
                                    {
                                        if (currentDomain.ReTestBasal == "")
                                        {
                                            SetErrorMessage("There is no basal. The basal level is reached when a child scores 2 points on the 3 consecutive lowest administered items or first item.");
                                        }
                                        else if (currentDomain.ReTestCeiling == "")
                                        {
                                            SetErrorMessage("There is no ceiling. The ceiling level is reached when a child scores 0 points on 3 consecutive items or reaches the last item.");
                                        }
                                    }
                                }
                                else
                                {
                                    if (currentDomain.Basal == "" && currentDomain.Ceiling == "")
                                    {
                                        SetErrorMessage("There is no basal or ceiling. The basal level is reached when a child scores 2 points on the 3 consecutive lowest administered items or first item. The ceiling level is reached when a child scores 0 points on 3 consecutive items or reaches the last item.");
                                    }
                                    else
                                    {
                                        if (currentDomain.Basal == "")
                                        {
                                            SetErrorMessage("There is no basal. The basal level is reached when a child scores 2 points on the 3 consecutive lowest administered items or first item.");
                                        }
                                        else if (currentDomain.Ceiling == "")
                                        {
                                            SetErrorMessage("There is no ceiling. The ceiling level is reached when a child scores 0 points on 3 consecutive items or reaches the last item.");
                                        }
                                    }
                                }                                 
                            }
                        }
                    }
                }
            }
        }

        protected void ButtonDone_Click(object sender, EventArgs e)
        {
            if (SaveAndValidInput())
            {
                string domainStatus = "";

                StudentAssessment sa = AssessmentCommon.GetSessionStudentAssessment();

                if (sa != null)
                {
                    StudentDomain currentDomain = sa.GetCurrentDomain();

                    if (currentDomain != null)
                    {
                        int childMonth = AssessmentHelper.GetChildMonths(sa.StudentBirthday, sa.FirstTestDate);
                        domainStatus = currentDomain.CalculateBC(false, childMonth); //reclate B/C for the org test

                        if (domainStatus == DomainStatus.Ceiling)
                        {
                            ButtonDone2_Click(sender, e);
                        }
                        else
                        {
                            if (currentDomain.StartLateFlag)
                            {
                                LiteralScript.Text = "<br/><script>\n" + "OnDone3();\n" + "</script>";
                            }
                            else
                            {
                                LiteralScript.Text = "<br/><script>\n" + "OnDone2();\n" + "</script>";
                            }
                        }
                    }
                }
            }
        }

        protected void ButtonDone2_Click(object sender, EventArgs e)
        {
            Button buttonSender = sender as Button;
            bool valid = true;

            if (buttonSender.Text == "Done2") //from "Done2", need validation
            {
                valid = SaveAndValidInput();
            }

            if (valid)
            {
                //save
                StudentAssessment sa = AssessmentCommon.GetSessionStudentAssessment();

                if (sa != null)
                {
                    StudentDomain currentDomain = sa.GetCurrentDomain();

                    if (currentDomain != null)
                    {
                        currentDomain.Usage_Selection = false; //uncheck
                        currentDomain.DirtyFlag = true;
                       
                        currentDomain.ScoreTypeID = (int)ScoreType.ItemDetails;

                        int childMonth = AssessmentHelper.GetChildMonths(sa.StudentBirthday, sa.FirstTestDate);
                        currentDomain.CalculateBC(false, childMonth); //reclate B/C for the org test
                        currentDomain.CalculateScore(sa.AssessmentTypeID, reTest);

                        //SS, AE score and required flag
                        DateTime testDate;
                        if (sa.FirstTestDate == DateTime.MinValue)
                        {
                            testDate = currentDomain.TestDate;
                        }
                        else
                        {
                            testDate = (currentDomain.TestDate < sa.FirstTestDate ? currentDomain.TestDate : sa.FirstTestDate);
                        }
                        int months = AssessmentHelper.GetChildMonths(sa.StudentBirthday, testDate);
                        currentDomain.GetSubdomainScore(sa.AssessmentTypeID, months);

                        if (currentDomain.ParentDomainID > 0)
                        {
                            sa.FindDomain(currentDomain.ParentDomainID).DirtyFlag = true; // set parent domain dirty.
                        }

                        sa.DirtyFlag = true;
                        sa.CalculateDate();
                        sa.CalculateScore(); // calculate / set parent domain scores here
                        if (reTest)
                        {
                            sa.ReTestTaken = true; // set assessment re-test flag
                        }
                        sa.EditRecord();

                        ClearStatusMessageLabel();
                        //Need to do a Redirect instead of transfer because of ajax controls.
                        Response.Redirect("EditAssessment.aspx");
                    }
                }
            }
        }
    }
}
