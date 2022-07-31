using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using BDI2Core.Common;
using BDI2Core.Configuration;
using BDI2Core.Data;

public partial class Controls_Export : System.Web.UI.UserControl
{
    #region Variable Declarations

    private String _HeaderCssClass;
    private String _ExportSummary;
    private String _FromDate;
    private String _ToDate;
    private String _delimiter = "\t";
    private String _crlf = "\r\n";

    #endregion

    #region Properties

    /// <summary>
    /// Set/Get the CssClass Name for the Headers.
    /// </summary>
    public String ExportSummary
    {
        get { return _ExportSummary; }
        set { _ExportSummary = value; }
    }
    public String strFromDate
    {
        get { return _FromDate; }
        set { _FromDate = value; }
    }
    public String strToDate
    {
        get { return _ToDate; }
        set { _ToDate = value; }
    }
    public String HeaderCssClass
    {
        get { return _HeaderCssClass; }
        set { _HeaderCssClass = value; }
    }
    public String Delimiter
    {
        get { return _delimiter; }
        set { _delimiter = value; }
    }
    public String CrLf
    {
        get { return _crlf; }
    }

    #endregion Properties

    #region Private Functions

    private void buildExportFile()
    {
        Delimiter = rblFormat.SelectedValue;
        string stFileName = "BDI2Ex" + rblFile.SelectedValue + (Delimiter == "," ? ".csv" : ".txt");
        string strContentType = "application/octet-stream";
        // set up the output file name
        Response.Clear();
        
        Response.AddHeader("Content-Disposition", "attachment; filename=" + stFileName);
		// tell browser its a file not html
		Response.ContentType = strContentType;
		// now extract and write the file
        if (rblFile.SelectedValue == "Organizations" || rblFile.SelectedValue == "Staff" || rblFile.SelectedValue == "Students")
        {
            if (ExecuteOrganizationBasedExtract(stFileName))
            {
                ;
            }
        }
        else
        {
            if (ExecuteDateBasedExtract(stFileName))
            {
                ;
            }
        }
		// end the process
		Response.Flush();
		Response.Close();

    }

    private bool ExecuteDateBasedExtract(string fileName)
    {
        //			string Value;
        string strExtractField;
        bool isValid;
        Int32 icust = Convert.ToInt32(Session["CustomerID"]);

        string stProcedure = "exim.eiExport" + rblFile.SelectedValue;

        try
        {
            bool hasNextRecord = false;
            StringBuilder sbLine = new StringBuilder();

            //SqlParameter[] userParams = new SqlParameter[3]; 
            //userParams[0] = new SqlParameter("@CustomerID", Convert.ToInt32(Session["CustomerID"]));
            //userParams[1] = new SqlParameter("@StartDate", Convert.ToDateTime(FromDate.SelectedDate.ToShortDateString()));
            //userParams[2] = new SqlParameter("@EndDate", Convert.ToDateTime(ToDate.SelectedDate.ToShortDateString()));
            String[] userParams = new String[3];
            userParams[0] = icust.ToString();
            userParams[1] = FromDate.SelectedDate.ToShortDateString();
            userParams[2] = ToDate.SelectedDate.ToShortDateString();

            SqlDataReader myReader = SQLHelper.ExecuteReader(BDI2Core.Configuration.Settings.ConnectionStringName, stProcedure, userParams);

            if (myReader.HasRows)
            {
                hasNextRecord = myReader.Read();
                while (hasNextRecord)
                {
                    // loop through the returned colums adding them to the output string
                    for (int i = 0; i < myReader.FieldCount; i++)
                    {
                        strExtractField = myReader[i].ToString().Trim();
                        sbLine.Append(strExtractField);
                        if (rblFormat.SelectedValue == ",")
                            sbLine.Append(",");
                        else
                            sbLine.Append("\t");
                    }
                    sbLine.Append(CrLf);

                    //write line
                    Response.Write(sbLine);
                    sbLine.Remove(0, sbLine.Length);

                    hasNextRecord = myReader.Read();
                }
                myReader.Close();
                isValid = true;
            }
            else
            {
                myReader.Close();
                isValid = false;
            }
        }
        catch (Exception ex)
        {
            //_msg = ex.Message.ToString();
            isValid = false;
            //todo: possible remove
            throw new Exception(ex.Message);
        }

        return isValid;
    }

    private bool ExecuteOrganizationBasedExtract(string fileName)
    {
        //			string Value;
        string strExtractField;
        bool isValid;
        string stProcedure = "exim.eiExport" + rblFile.SelectedValue;
        Int32 icust = Convert.ToInt32(Session["CustomerID"]);

        try
        {
            bool hasNextRecord = false;
            StringBuilder sbLine = new StringBuilder();

            //SqlParameter[] userParams = new SqlParameter[2];
            //userParams[0] = new SqlParameter("@CustomerID", icust);
            //userParams[1] = new SqlParameter("@Organization", LabelOrgSelected.Text);

            String[] userParams = new String[2];
            userParams[0] = icust.ToString();
            userParams[1] = LabelOrgSelected.Text;

            SqlDataReader myReader = SQLHelper.ExecuteReader(BDI2Core.Configuration.Settings.ConnectionStringName, stProcedure, userParams);

            if (myReader.HasRows)
            {
                hasNextRecord = myReader.Read();
                while (hasNextRecord)
                {
                    // loop through the returned colums adding them to the output string
                    for (int i = 0; i < myReader.FieldCount; i++)
                    {
                        strExtractField = myReader[i].ToString().Trim();
                        sbLine.Append(strExtractField);
                        if (rblFormat.SelectedValue == ",")
                            sbLine.Append(",");
                        else
                            sbLine.Append("\t");
                    }
                    sbLine.Append(CrLf);

                    //write line
                    Response.Write(sbLine);
                    sbLine.Remove(0, sbLine.Length);

                    hasNextRecord = myReader.Read();
                }
                myReader.Close();
                isValid = true;
            }
            else
            {
                myReader.Close();
                isValid = false;
            }
        }
        catch (Exception ex)
        {
            //_msg = ex.Message.ToString();
            isValid = false;
            //todo: possible remove
            throw new Exception(ex.Message);
        }

        return isValid;
    }


    #endregion Private Functions

    #region UI Events

    protected void Page_Load(object sender, EventArgs e)
    {
        //support staff under multiple parents now
        //DataSet ds = OrganizationHelper.GetOrganizationTreeForStaff((int)SessionData.Staff.StaffID, true);
        DataSet ds = OrganizationHelper.GetOrganizationTreeForStaff(67, true);

        rtvOrganization.DataSource = ds;
        rtvOrganization.DataBind();
        rtvOrganization.CollapseAllNodes();
        //selection
        if (Session["Export_SelectionIndex"] != null)
        {
            int selectedOrgID = (int)Session["Export_SelectionIndex"];
            ArrayList selectionTree = new ArrayList();
            OrganizationHelper.BuildSelectionTree(ref selectionTree, selectedOrgID);

            if (selectionTree.Count > 1)
            {
                Telerik.WebControls.RadTreeNode currentNode = rtvOrganization.Nodes[0];
                currentNode.Expanded = true;

                while (currentNode.Nodes.Count > 0)
                {
                    bool bFound = false;
                    foreach (Telerik.WebControls.RadTreeNode node in currentNode.Nodes)
                    {
                        if (selectionTree.Contains(node.Value))
                        {
                            bFound = true;
                            node.Expanded = true;
                            currentNode = node;
                            break;
                        }
                    }
                    if (!bFound)
                    {
                        break;
                    }

                    if (currentNode.Value == selectedOrgID.ToString())
                    {
                        break;
                    }
                }
            }
        }
    }

    protected void butExport_Click(object sender, EventArgs e)
    {
        buildExportFile();
        butExport.Enabled = false;
        lblFrom.Visible = false;
        lblTo.Visible = false;
        FromDate.Enabled = false;
        FromDate.Visible = false;
        ToDate.Enabled = false;
        ToDate.Visible = false;
        rtvOrganization.Enabled = false;
        rtvOrganization.Visible = false;
    }

    protected void rblFile_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rblFile.SelectedValue == "Organizations" || rblFile.SelectedValue == "Staff" || rblFile.SelectedValue == "Students")
        {
            lblInstruction.Text = "Select organization from tree and press the Export File button";
            lblInstruction.Visible = true;
            rtvOrganization.Enabled = true;
            rtvOrganization.Visible = true;
            lblFrom.Visible = false;
            lblTo.Visible = false;
            FromDate.Enabled = false;
            FromDate.Visible = false;
            ToDate.Enabled = false;
            ToDate.Visible = false;
        }
        else
        {
            lblInstruction.Text = "Select the beginning and ending dates for the export and press the Export File button";
            lblInstruction.Visible = true;
            lblFrom.Visible = true;
            lblTo.Visible = true;
            FromDate.Enabled = true;
            FromDate.Visible = true;
            ToDate.Enabled = true;
            ToDate.Visible = true;
            rtvOrganization.Enabled = false;
            rtvOrganization.Visible = false;
        }
    }

    protected void FromDate_SelectionChanged(object sender, EventArgs e)
        {
        bool canExport = false;

        //HandleStaffSecurity(orgRec.HierarchyLevelID, orgRec.LevelTypeID, ref canExport);

        if (ToDate.SelectedDate.ToLongDateString() != "")    // to do check can Export
        {
            butExport.Enabled = true;
        }
    }

    protected void ToDate_SelectionChanged(object sender, EventArgs e)
    {
        bool canExport = false;

        //HandleStaffSecurity(orgRec.HierarchyLevelID, orgRec.LevelTypeID, ref canExport);

        if (FromDate.SelectedDate.ToLongDateString() != "")   // to do check can Export
        {
            butExport.Enabled = true;
        }
    }

    protected void rtvOrganization_NodeClick(object o, Telerik.WebControls.RadTreeNodeEventArgs e)
    {
        bool canExport = false;

        int organizationID = int.Parse(e.NodeClicked.Value);
        LabelOrgSelected.Text = e.NodeClicked.Text;
        Session["Export_SelectionIndex"] = organizationID;

        LabelOrganizationID.Text = organizationID.ToString();
        OrganizationRecord orgRec = new OrganizationRecord(organizationID);
        orgRec.LoadFromDB();
        LabelHierarchyLevelID.Text = orgRec.HierarchyLevelID.ToString();

        HandleStaffSecurity(orgRec.HierarchyLevelID, orgRec.LevelTypeID, ref canExport);

        if (canExport)
        {
            butExport.Enabled = true;
        }
    }

    #endregion

    #region Security
    protected void HandleStaffSecurity(int orgHierarchyLevelID, int levelTypeID, ref bool canExport)
    {
        canExport = false;

        //if (SessionData.Staff.Privileges.Contains(Privileges.ManageOrganization))
        //{
        //    int staffLevel = SessionData.Staff.HierarchyLevelID;
        //    int nResult = HierarchyLevelHelper.CompareHierarchy((int)SessionData.CustomerID, orgHierarchyLevelID, staffLevel);

        //    if (levelTypeID == (int)LevelType.TopLevel)
        //    {
        //        canDelete = false;

        //        if (nResult <= 0)
        //        {
        //            canEdit = true;
        //            canAdd = true;
        //        }

        //    }
        //    else if (levelTypeID == (int)LevelType.MiddleLevel)
        //    {
        //        if (nResult < 0)
        //        {
        //            canEdit = true;
        //            canDelete = true;
        //        }

        //        if (nResult <= 0)
        //        {
        //            canAdd = true;
        //        }
        //    }
        //    else // buttom levels
        //    {
        //        if (nResult <= 0)
        //        {
        //            canEdit = true;
        //            canDelete = true;
        //        }

        //        canAdd = false;
        //    }
        //}
        canExport = true;
    }

    #endregion

}
