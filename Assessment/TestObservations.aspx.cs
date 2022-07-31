using System;
using System.Data;
using System.Web.UI.WebControls;
using BDI2Core.Common;
using BDI2Core.Data;
using BDI2Web.Common_Code;


namespace BDI2Web.Assessment
{
    public partial class TestObservations : SecureBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                StudentAssessment sa = AssessmentCommon.GetSessionStudentAssessment();
                if (sa != null)
                {
                    //title labels
                    LabelTitle.Text = "Test Observations For ";
                    BDI2Core.Common.Student st = new BDI2Core.Common.Student(sa.StudentID);
                    st.LoadFromDB();
                    LabelTitle.Text += (st.FirstName + " " + st.LastName);

                    LabelTestDate.Text = "Test Date: ";
                    if (sa.FirstTestDate != DateTime.MinValue)
                    {
                        LabelTestDate.Text += sa.FirstTestDate.ToShortDateString();
                    }

                    LabelAge.Text = "Age (Months): ";
                    LabelAge.Text += AssessmentHelper.GetChildMonths(st.Birthdate, DateTime.Today);

                    //cat repeaters
                    DataSet ds = Lookups.GetObservationCat();
                    RepeaterCategory.DataSource = ds;
                    RepeaterCategory.DataBind();

                    sa.LoadObservation(true); //load new flag is true

                    StudentObservation so_curr =  sa.FindObservationByAType((int)ObservationAnswerType.ObservedTimes);
                    TextBoxObservedTimes.Text = ( so_curr == null? "" : so_curr.Note);
                    so_curr =sa.FindObservationByAType((int)ObservationAnswerType.OverDays);
                    TextBoxOverDays.Text = (so_curr == null ? "" : so_curr.Note);
                    so_curr = sa.FindObservationByAType((int)ObservationAnswerType.MinutesTotal);
                    TextBoxMinutesTotal.Text = (so_curr == null ? "" : so_curr.Note);

                    //other repeaters
                    ds = sa.FormatObservationSet((int)ObservationCategoryType.Validity, 1, 14);
                    RepeaterQuestion.DataSource = ds;
                    RepeaterQuestion.DataBind();

                    ds = sa.FormatObservationSet((int)ObservationCategoryType.Validity, 18, 24);
                    RepeaterQuestion2.DataSource = ds;
                    RepeaterQuestion2.DataBind();

                    ds = sa.FormatObservationSet((int)ObservationCategoryType.Other, 0, 0);
                    RepeaterNotes.DataSource = ds;
                    RepeaterNotes.DataBind();

                    //hide panel
                    Panel2.Visible = false;
                }
            }
        }

        #region Overridden Functions

        protected override void SetPageID()
        {
            PageID = 24; //Value from Page Table in db
        }

        #endregion Overridden Functions

        protected void RepeaterQuestion_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            HiddenField answerType = (HiddenField)e.Item.FindControl("AnswerType");
            if (answerType != null)
            {
                HiddenField yesNoNA = (HiddenField)e.Item.FindControl("YesNoNA");
                RadioButtonList rList = (RadioButtonList)e.Item.FindControl("RadioButtonListYesNo");
                if (answerType.Value.ToString() == "1")
                {
                    rList.Visible = false;
                }
                else
                {
                    rList.SelectedIndex = (yesNoNA.Value == "" ? 2 : int.Parse(yesNoNA.Value)); //default to 2 (N/A)
                }
            }
        }

        protected void RepeaterQuestion2_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            HiddenField answerType = (HiddenField)e.Item.FindControl("AnswerType2");
            if (answerType != null)
            {
                HiddenField yesNoNA = (HiddenField)e.Item.FindControl("YesNoNA2");
                RadioButtonList rList = (RadioButtonList)e.Item.FindControl("RadioButtonListYesNo2");
                if (answerType.Value.ToString() == "1")
                {
                    rList.Visible = false;
                }
                else
                {
                    rList.SelectedIndex = (yesNoNA.Value == "" ? 2 : int.Parse(yesNoNA.Value)); //default to 2 (N/A)
                }
            }
        }

        protected void RepeaterCategory_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            LinkButton link = (LinkButton)e.Item.FindControl("Description");
            if (link != null)
            {
                string catName = link.Text;
                if (catName == "Validity")
                {
                    Label mark = (Label)e.Item.FindControl("LabelMark");
                    mark.Visible = true; // turn on the first one.
                }
            }
        }

        protected void RepeaterCategory_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            int observationCatID = int.Parse(e.CommandArgument.ToString());
            string catName = ((LinkButton)e.Item.FindControl("Description")).Text;


            foreach (RepeaterItem cat_item in RepeaterCategory.Items)
            {
                HiddenField ID = (HiddenField)cat_item.FindControl("ObservationCategoryID");
                if (ID != null)
                {
                    Label mark = (Label)cat_item.FindControl("LabelMark");
                    if (observationCatID == int.Parse(ID.Value))
                    {
                        mark.Visible = true;
                    }
                    else
                    {
                        mark.Visible = false;
                    }
                }
            }


            if (catName == "Validity")
            {
                Panel1.Visible = true;
                Panel2.Visible = false;
            }
            else
            {
                Panel1.Visible = false;
                Panel2.Visible = true;

                foreach (RepeaterItem item in RepeaterNotes.Items)
                {
                    HiddenField catID = (HiddenField)item.FindControl("observationCategoryID");
                    if (catID != null)
                    {
                        Label desc = (Label)item.FindControl("Description");
                        TextBox text = (TextBox)item.FindControl("TextBoxNote");
                        Panel panel = (Panel) item.FindControl("NotesPanel");
                        if (observationCatID == int.Parse(catID.Value))
                        {
                            panel.Visible = true;
                            desc.Visible = true;
                            text.Visible = true;
                        }
                        else
                        {
                            panel.Visible = false;
                            desc.Visible = false;
                            text.Visible = false;
                        }
                    }
                }
            }
        }

        protected void ButtonBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("ChildAssessment.aspx");
        }

        protected void ButtonClear_Click(object sender, EventArgs e)
        {
            TextBoxObservedTimes.Text = "";
            TextBoxOverDays.Text = "";
            TextBoxMinutesTotal.Text = "";

            foreach (RepeaterItem item in RepeaterQuestion.Items)
            {
                RadioButtonList rList = (RadioButtonList)item.FindControl("RadioButtonListYesNo");
                if (rList != null)
                {
                    rList.Items[0].Selected = false;
                    rList.Items[1].Selected = false;
                    //rList.Items[2].Selected = true;
                }
            }

            foreach (RepeaterItem item in RepeaterQuestion2.Items)
            {
                RadioButtonList rList = (RadioButtonList)item.FindControl("RadioButtonListYesNo2");
                if (rList != null)
                {
                    rList.Items[0].Selected = false;
                    rList.Items[1].Selected = false;
                    //rList.Items[2].Selected = true;
                }
            }

            foreach (RepeaterItem item in RepeaterNotes.Items)
            {
                TextBox text = (TextBox)item.FindControl("TextBoxNote");
                if (text != null)
                {
                    text.Text = "";
                }
            }
        }

        protected void ButtonSave_Click(object sender, EventArgs e)
        {
            StudentObservation so;

            StudentAssessment sa = AssessmentCommon.GetSessionStudentAssessment();
            if (sa != null)
            {
                so = sa.FindObservationByAType((int)ObservationAnswerType.ObservedTimes);
                if (so != null)
                {
                    if (Utilities.IsNumeric(TextBoxObservedTimes.Text))
                    {
                        so.Note = TextBoxObservedTimes.Text;
                    }
                    else
                    {
                        SetErrorMessage("Please enter valid numric data for Observed Times.");
                        Response.Redirect("TestObservations.aspx");
                    }
                    so.YesNoNA = 2; // N/A
                    so.DirtyFlag = true;
                }
                so = sa.FindObservationByAType((int)ObservationAnswerType.OverDays);
                if (so != null)
                {
                    if (Utilities.IsNumeric(TextBoxOverDays.Text))
                    {
                        so.Note = TextBoxOverDays.Text;
                    }
                    else
                    {
                        SetErrorMessage("Please enter valid numric data for Over Days.");
                        Response.Redirect("TestObservations.aspx");
                    }
                    so.YesNoNA = 2; // N/A
                    so.DirtyFlag = true;
                }
                so = sa.FindObservationByAType((int)ObservationAnswerType.MinutesTotal);
                if (so != null)
                {
                    if (Utilities.IsNumeric(TextBoxMinutesTotal.Text))
                    {
                        so.Note = TextBoxMinutesTotal.Text;
                    }
                    else
                    {
                        SetErrorMessage("Please enter valid numric data for Minutes Total.");
                        Response.Redirect("TestObservations.aspx");
                    }
                    so.YesNoNA = 2; // N/A
                    so.DirtyFlag = true;
                }

                foreach (RepeaterItem item in RepeaterQuestion.Items)
                {
                    HiddenField observationQuestionID = (HiddenField)item.FindControl("QuestionID");
                    if (observationQuestionID != null)
                    {
                        so = sa.FindObservationByQID(int.Parse(observationQuestionID.Value));
                        RadioButtonList rList = (RadioButtonList)item.FindControl("RadioButtonListYesNo");
                        so.YesNoNA = (rList.SelectedValue == "" ? 2 : int.Parse(rList.SelectedValue.ToString())); // 2 is N/A
                        so.DirtyFlag = true;
                    }
                }

                foreach (RepeaterItem item in RepeaterQuestion2.Items)
                {
                    HiddenField observationQuestionID = (HiddenField)item.FindControl("QuestionID2");
                    if (observationQuestionID != null)
                    {
                        so = sa.FindObservationByQID(int.Parse(observationQuestionID.Value));
                        RadioButtonList rList = (RadioButtonList)item.FindControl("RadioButtonListYesNo2");
                        so.YesNoNA = (rList.SelectedValue == "" ? 2 : int.Parse(rList.SelectedValue.ToString())); // 2 is N/A
                        so.DirtyFlag = true;
                    }
                }

                foreach (RepeaterItem item in RepeaterNotes.Items)
                {
                    HiddenField observationQuestionID = (HiddenField)item.FindControl("observationQuestionID");
                    if (observationQuestionID != null)
                    {
                        so = sa.FindObservationByQID(int.Parse(observationQuestionID.Value));
                        TextBox note = (TextBox)item.FindControl("TextBoxNote");
                        so.Note = "" + note.Text;
                        so.YesNoNA = 2; // 2 is N/A
                        so.DirtyFlag = true;
                    }
                }

                sa.DirtyFlag = true;
                sa.EditRecord();

                SessionData.StatusMessage = "Observations saved.";
                Response.Redirect("EditScreener.aspx");
            }
        }
    }
}
