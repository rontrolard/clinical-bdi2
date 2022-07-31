using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using System.Data;

using BDI2Core.Common;
using BDI2Core.Data;
using BDI2Web.Common_Code;

namespace BDI2Web.ERFs
{
    public partial class ERFManagementRootLevel : System.Web.UI.UserControl
    {
        public SessionData SessionData { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            spErrorMessageRequired.Visible = false;
            spErrorMessage.Visible = false;
        }

        public void Initialize()
        {
            var customerID = SessionData.CustomerID.Value;
            LoadNodes(T, OrganizationHelper.GetRootOrganizationTreeNode(customerID).Tables[0], T.Nodes);
            SetRootERFView();
            Div1.Visible = false;
        }

        protected void SelectOrganization_ItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
        {
            DataTable data = OrganizationHelper.GetMatchingOrganizations(SessionData.CustomerID.Value, e.Text).Tables[0];

            int itemOffset = e.NumberOfItems;
            int endOffset = Math.Min(itemOffset + 10, data.Rows.Count);
            e.EndOfItems = endOffset == data.Rows.Count;

            for (int i = itemOffset; i < endOffset; i++)
            {
                SelectOrganization.Items.Add(
                    new RadComboBoxItem(
                        data.Rows[i]["description"].ToString(),
                        data.Rows[i]["organizationID"].ToString()));
            }

            e.Message = GetStatusMessage(endOffset, data.Rows.Count);
        }

        private static string GetStatusMessage(int offset, int total)
        {
            if (total <= 0)
                return "No matches";

            return String.Format("Items <b>1</b>-<b>{0}</b> out of <b>{1}</b>", offset, total);
        }

        protected void SelectOrganization_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (SelectOrganization.SelectedValue != string.Empty)
            {
                var node = T.FindNodeByValue(SelectOrganization.SelectedValue);
                if (node == null)
                    node = SetUpNodeChain(int.Parse(SelectOrganization.SelectedValue));
                ExpandNodeChain(node);
                node.Selected = true;
                RadAjaxManager1.ResponseScripts.Add("ScrollToSelectedNode();");
                SetSelectedNodeERFView(int.Parse(node.Value));
                Div1.Visible = true;
            }
            else
            {
                T.Nodes[0].Selected = true;
                T.Nodes[0].Selected = false;
                Div1.Visible = false;
            }
        }

        protected RadTreeNode SetUpNodeChain(int organizationId)
        {
            var parentOrganizationID = (int) OrganizationHelper.GetParentOrganizationID(organizationId);
            var node = T.FindNodeByValue(parentOrganizationID.ToString());

            if (node == null)
            {
                node = SetUpNodeChain(parentOrganizationID);
            }

            ExpandNode(node);

            return T.FindNodeByValue(organizationId.ToString());
        }

        protected void T_NodeExpand(object sender, RadTreeNodeEventArgs e)
        {
            ExpandNode(e.Node);
        }

        private void ExpandNode(RadTreeNode node)
        {
            var ds = OrganizationHelper.GetChildOrganizationsTreeNodes(int.Parse(node.Value));
            LoadNodes(T, ds.Tables[0], node.Nodes);
            node.Expanded = true;
            node.ExpandMode = TreeNodeExpandMode.ClientSide;
        }

        protected void ExpandNodeChain(RadTreeNode selectedNode)
        {
            if (selectedNode.ParentNode != null)
            {
                selectedNode.ParentNode.Expanded = true;
                ExpandNodeChain(selectedNode.ParentNode);
            }
        }

        protected void T_NodeClick(object sender, RadTreeNodeEventArgs e)
        {
            Div1.Visible = true;
            SelectOrganization.Text = e.Node.Text;
            SetSelectedNodeERFView(int.Parse(e.Node.Value));
        }

        protected void SetSelectedNodeERFView(int organizationID)
        {
            var ds = OrganizationHelper.GetERFSForChildOrganizations(organizationID);
            GridView1.DataSource = ds;
            GridView1.DataBind();
        }

        protected void SetRootERFView()
        {
            var ds = OrganizationHelper.GetRootOrganizationERFCounts(SessionData.CustomerID.Value);
            ErfsOwned.Text = ((int)ds.Tables[0].Rows[0]["Owned"]).ToString();
            ErfsAvailable.Text = ((int)ds.Tables[0].Rows[0]["Available"]).ToString();
        }

        private void LoadNodes(RadTreeView treeView, DataTable tb, RadTreeNodeCollection nodeCollection)
        {
            foreach (DataRow row in tb.Rows)            
            {                
                RadTreeNode node = new RadTreeNode();                
                node.Text = row["Description"].ToString();               
                node.Value = row["OrganizationID"].ToString();                
                if (Convert.ToInt32(row["ChildCount"]) > 0)        
                {
                    node.ExpandMode = TreeNodeExpandMode.ServerSide;
                }
                nodeCollection.Add(node);            
            }        
        }


        protected void Assign_Clicked(object sender, EventArgs e)
        {
            var organizationID = int.Parse(T.Nodes[0].Value);
            var countTextBox = (RadNumericTextBox)((Button)sender).Parent.FindControl("DeltaCount");

            if (countTextBox.Value == null || countTextBox.Value == 0)
            {
                spErrorMessageRequired.Visible = true;
                countTextBox.Text = string.Empty;
                RadAjaxManager1.ResponseScripts.Add("(document.getElementById('" + spErrorMessageRequired.ClientID + "')).scrollIntoView();");
                return;
            }

            var available = int.Parse(ErfsAvailable.Text);

            if (countTextBox.Value > (available < 0 ? 0 : available))
            {
                spErrorMessage.Visible = true;
                countTextBox.Text = string.Empty;
                RadAjaxManager1.ResponseScripts.Add("(document.getElementById('" + spErrorMessage.ClientID + "')).scrollIntoView();");
                return;
            }

            var erfsToAssign = (int)countTextBox.Value;
            var destinationOrganizationID = int.Parse(((Button)sender).CommandArgument);
            OrganizationHelper.AssignERFs(organizationID, destinationOrganizationID, erfsToAssign, SessionData.Staff.StaffID.Value);

            SetRootERFView();
            SetSelectedNodeERFView(CurrentOrganizationID);
        }

        protected void Remove_Clicked(object sender, EventArgs e)
        {
            var countTextBox = (RadNumericTextBox)((Button)sender).Parent.FindControl("DeltaCount");

            if (countTextBox.Value == null || countTextBox.Value == 0)
            {
                spErrorMessageRequired.Visible = true;
                countTextBox.Text = string.Empty;
                RadAjaxManager1.ResponseScripts.Add("(document.getElementById('" + spErrorMessageRequired.ClientID + "')).scrollIntoView();");
                return;
            }

            var assigned = int.Parse(((GridViewRow)((Button)sender).Parent.Parent).Cells[2].Text);

            if (countTextBox.Value > assigned)
            {
                spErrorMessage.Visible = true;
                countTextBox.Text = string.Empty;
                RadAjaxManager1.ResponseScripts.Add("(document.getElementById('" + spErrorMessage.ClientID + "')).scrollIntoView();");
                return;
            }

            var erfsToRemove = (int)countTextBox.Value;
            var organizationID = int.Parse(((Button)sender).CommandArgument);
            OrganizationHelper.RemoveErfsOwnedByRootLevel(organizationID, erfsToRemove, SessionData.Staff.StaffID.Value);

            SetRootERFView();
            SetSelectedNodeERFView(CurrentOrganizationID);
        }

        private int CurrentOrganizationID { get { return int.Parse(T.SelectedValue); } }
    }
}