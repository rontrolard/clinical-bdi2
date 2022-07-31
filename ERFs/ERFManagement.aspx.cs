using System;
using System.Data;
using System.Web.UI.WebControls;
using BDI2Core.Common;
using BDI2Core.Data;
using BDI2Web.Common_Code;
using System.Collections.Generic;
using Telerik.Web.UI;
using System.Linq;

namespace BDI2Web.ERFs
{
    public partial class ERFManagement : SecureBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ERFManagementRootLevel.SessionData = SessionData;

            trErrorMessage.Visible = false;
            trErrorMessageRequired.Visible = false;

            if (!IsPostBack)
            {
                if (!SessionData.Staff.Privileges.Contains(Privileges.ManageERFMDS))
                {
                    MultiView1.ActiveViewIndex = 0;
                }
                else if (SessionData.Staff.LevelType == LevelType.TopLevel)
                {
                    MultiView1.ActiveViewIndex = 1;                    
                    ERFManagementRootLevel.Initialize();
                }
                else
                {
                    MultiView1.ActiveViewIndex = 2;

                    OrgTr.Visible = false;
                    GridTr.Visible = false;

                    OrganizationDropDownList.DataSource = OrganizationHelper
                        .GetCurrentLevelOrganizationsForStaff(SessionData.Staff.HierarchyLevelID, SessionData.Staff.StaffID.Value);

                    OrganizationDropDownList.DataBind();
                }
            }
        }

        protected void OrganizationDropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (OrganizationDropDownList.SelectedValue == "-1")
            {
                OrgTr.Visible = false;
                GridTr.Visible = false;
            }
            else
            {
                UpdateBaseView();
                OrgTr.Visible = true;
                GridTr.Visible = true;
            }
        }

        protected void Assign_Clicked(object sender, EventArgs e)
        {
            var organizationID = int.Parse(OrganizationDropDownList.SelectedValue);            
            var countTextBox = (RadNumericTextBox)((Button)sender).Parent.FindControl("DeltaCount");

            if (countTextBox.Value == null || countTextBox.Value == 0)
            {
                trErrorMessageRequired.Visible = true;
                countTextBox.Text = string.Empty;
                FocusOnErrorMessage();
                return;
            }

            var owned = int.Parse(ErfsOwned.Text);
            var assignedFromHigherLevel = int.Parse(ErfsAssignedFromHigherLevel.Text);
            var assignedFromRoot = int.Parse(ErfsAssignedFromRoot.Text);
            var available = owned + assignedFromRoot + assignedFromHigherLevel;

            if (countTextBox.Value > (available < 0 ? 0 : available))
            {
                trErrorMessage.Visible = true;
                countTextBox.Text = string.Empty;
                FocusOnErrorMessage();
                return;
            }

            var erfsToAssign = (int)countTextBox.Value;
            var destinationOrganizationID = int.Parse(((Button)sender).CommandArgument);
            OrganizationHelper.AssignERFs(organizationID, destinationOrganizationID, erfsToAssign, SessionData.Staff.StaffID.Value);
            UpdateBaseView();
        }

        protected void Remove_Clicked(object sender, EventArgs e)
        {
            var countTextBox = (RadNumericTextBox)((Button)sender).Parent.FindControl("DeltaCount");

            if (countTextBox.Value == null || countTextBox.Value == 0)
            {
                trErrorMessageRequired.Visible = true;
                countTextBox.Text = string.Empty;
                FocusOnErrorMessage();
                return;
            }

            var assigned = int.Parse(((GridViewRow)((Button)sender).Parent.Parent).Cells[3].Text);

            if (countTextBox.Value > assigned)
            {
                trErrorMessage.Visible = true;
                countTextBox.Text = string.Empty;
                FocusOnErrorMessage();
                return;
            }

            var erfsToRemove = (int)countTextBox.Value;
            var organizationID = int.Parse(((Button)sender).CommandArgument);
            OrganizationHelper.RemoveERFs(organizationID, erfsToRemove, SessionData.Staff.StaffID.Value);
            UpdateBaseView();
        }

        private void UpdateBaseView()
        {
            var orgId = int.Parse(OrganizationDropDownList.SelectedValue);
            var ds = OrganizationHelper.GetERFsForOrganizationAndChildren(orgId);

            ErfsOwned.Text = ((int)ds.Tables[0].Rows[0]["Owned"]).ToString();
            ErfsAssignedFromRoot.Text = ((int)ds.Tables[0].Rows[0]["AssignedFromRoot"]).ToString();
            ErfsAssignedFromHigherLevel.Text = ((int)ds.Tables[0].Rows[0]["AssignedFromHigherLevel"]).ToString();

            GridView1.DataSource = ds.Tables[1];
            GridView1.DataBind();
        }

        protected override void SetPageID()
        {
            PageID = 70; //Value from Page Table in db
        }

        private void FocusOnErrorMessage()
        {
             System.Web.UI.ScriptManager.RegisterStartupScript(
                    this, this.GetType(), "foo", "(document.getElementById('" + Label1.ClientID + "')).scrollIntoView();", true);
        }
    }
}