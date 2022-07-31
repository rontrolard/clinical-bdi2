using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using BDI2Core.Common;

namespace BDI2Web.Controls
{
    public partial class StudentNavigation : UserControl
    {
        #region Private Variables

        private int _pageID;
        private StaffPrivileges _privileges;

        #endregion Private Variables

        #region Properties
        /// <summary>
        /// Set/Get the CustomerNumber.
        /// </summary>
        public int PageID
        {
            get
            {
                return _pageID;
            }
            set
            {
                _pageID = value;
            }
        }
        public object StudentID
        {
            set
            {
                ViewState["Nav_StudentID"] = value;
            }
        }
        public StaffPrivileges Privileges
        {
            get
            {
                return _privileges;
            }
            set
            {
                _privileges = value;
            }
        }

        #endregion 

        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                int imageCount = 0;
                
                //Dyanmically adding Image buttons.
                if (Privileges != null && Privileges.Contains(BDI2Core.Common.Privileges.ManageChild))
                {
                    pnlImages.Controls.Add(AddImage("EDIT"));
                    pnlImages.Controls.Add(new LiteralControl("&nbsp;&nbsp;"));
                    imageCount++;
                }
                switch(PageID)
                {
                    case 20://StudentAssessment
                        pnlImages.Controls.Add(AddImage("REPORT"));
                        pnlImages.Controls.Add(new LiteralControl("&nbsp;&nbsp;"));
                        imageCount++;
                        pnlImages.Controls.Add(AddImage("NOTES"));
                        pnlImages.Controls.Add(new LiteralControl("&nbsp;&nbsp;"));
                        imageCount++;
                        break;
                    case 28://Report Criteria
                        if (Privileges != null && Privileges.Contains(BDI2Core.Common.Privileges.InputAssessment))
                        {
                            pnlImages.Controls.Add(AddImage("ASSESSMENT"));
                            pnlImages.Controls.Add(new LiteralControl("&nbsp;&nbsp;"));
                            imageCount++;
                        }
                        pnlImages.Controls.Add(AddImage("NOTES"));
                        pnlImages.Controls.Add(new LiteralControl("&nbsp;&nbsp;"));
                        imageCount++;
                        break;
                    case 39://General Notes
                        if (Privileges != null && Privileges.Contains(BDI2Core.Common.Privileges.InputAssessment))
                        {
                            pnlImages.Controls.Add(AddImage("ASSESSMENT"));
                            pnlImages.Controls.Add(new LiteralControl("&nbsp;&nbsp;"));
                            imageCount++;
                        }
                        pnlImages.Controls.Add(AddImage("REPORT"));
                        pnlImages.Controls.Add(new LiteralControl("&nbsp;&nbsp;"));
                        imageCount++;
                        break;
                }

                if (imageCount == 2)
                {
                    //Set initial width for two buttons
                    pnlImages.Width = 120;
                }
                else if (imageCount == 3)
                {
                    //Set width for three buttons.
                    pnlImages.Width = 150;
                }
            }
        }

       private ImageButton AddImage(string ID)
        {
            ImageButton imgButton = new ImageButton();
            if (ID.Equals("EDIT"))
            {
                imgButton.ID = "EditImage";
                imgButton.ToolTip = "Edit";
                imgButton.ImageUrl = "~/Images/edit.gif";
                Session["PAGE_17_studentID"] = ViewState["Nav_StudentID"];
                imgButton.PostBackUrl = "~/Student/EditChild.aspx";
                
            }
            else if (ID.Equals("REPORT"))
            {
                imgButton.ID = "ReportImage";
                imgButton.ToolTip = "Report";
                imgButton.ImageUrl = "~/Images/report.gif";
                Session["PAGE_28_studentID"] = ViewState["Nav_StudentID"];
                imgButton.PostBackUrl = "~/ReportSelection/ChildReportSelection.aspx";

            }
            else if(ID.Equals("NOTES"))
            {
                imgButton.ID = "NotesImage";
                imgButton.ToolTip = "Head Start Notes";
                imgButton.ImageUrl = "~/Images/notes.gif";
                Session["PAGE_39_studentID"] = ViewState["Nav_StudentID"];
                imgButton.PostBackUrl = "~/Notes/ChildNotes.aspx";               

            }
            else if (ID.Equals("ASSESSMENT"))
            {
                imgButton.ID = "AssessmentImage";
                imgButton.ToolTip = "Assessment";
                imgButton.ImageUrl = "~/Images/assessment.gif";
                Session["PAGE_20_studentID"] = ViewState["Nav_StudentID"];
                imgButton.PostBackUrl = "~/Assessment/ChildAssessment.aspx";                

            }
            imgButton.ImageAlign = ImageAlign.Middle;
            return imgButton;  
        }

        #endregion Events
    }
}