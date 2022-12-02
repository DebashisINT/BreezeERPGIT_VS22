using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxTreeList;
using System.Configuration;
using BusinessLogicLayer;

namespace ERP.OMS.Reports
{
    public partial class Reports_frmReport_BranchHirerchy : System.Web.UI.Page
    {
        private int childuser = 0;
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        ClsDropDownlistNameSpace.clsDropDownList clsdrp = new ClsDropDownlistNameSpace.clsDropDownList();
        DataTable DT_treeview = new DataTable();
        int j = 0;
        string[,] IdForColor;
        public string pageAccess = "";
        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //'http://localhost:2957/InfluxCRM/management/testProjectMainPage_employee.aspx'
                string sPath = HttpContext.Current.Request.Url.ToString();
                oDBEngine.Call_CheckPageaccessebility(sPath);
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (HttpContext.Current.Session["userid"] == null)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
            if (!IsPostBack)
            {
                lblMessage.Text = "";

                //string[,] branches = oDBEngine.GetFieldValue(" tbl_master_branch ", " branch_id,(branch_description+'['+branch_code+']') as branch ", " branch_id in (" + HttpContext.Current.Session["userbranchHierarchy"] + ")", 2, " branch_description ");
                //oDBEngine.AddDataToDropDownList(branches, cmbBranch, "All");
                //cmbBranch.SelectedIndex = 0;

                string[,] r = new string[1, 2];
                r[0, 0] = "All";
                r[0, 1] = "All";
                //oDBEngine.AddDataToDropDownList(r, cmbBranch, "");
                clsdrp.AddDataToDropDownList(r, cmbBranch, "");
                cmbBranch.Value = "All";


                //string[,] companies = oDBEngine.GetFieldValue(" tbl_master_company ", " cmp_id,cmp_name ", null, 2);
                //oDBEngine.AddDataToDropDownList(companies, cmbCompany, "All");
                //cmbCompany.SelectedIndex = 0;

                string[,] companies = oDBEngine.GetFieldValue(" TBL_MASTER_COUNTRY ", " COU_ID,COU_COUNTRY ", "COU_ID IN (SELECT DISTINCT BRANCH_COUNTRY FROM TBL_MASTER_BRANCH) ", 2);
                //oDBEngine.AddDataToDropDownList(companies, cmbCompany, "All"); 
                clsdrp.AddDataToDropDownList(companies, cmbCompany, "All");
                cmbCompany.SelectedIndex = 0;

                //this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");
            }
            if (IsPostBack)
            {
                if (cmbBranch.Value != null)
                {
                    if (cmbCompany.SelectedItem.Value.ToString() == "All" && cmbBranch.Value.ToString() == "All")
                    {
                        LoadTreeView();
                    }
                    else if (cmbCompany.SelectedItem.Value.ToString() != "All" && cmbBranch.Value.ToString() != "All")
                    {
                        LoadTreeViewWithCondition();
                    }
                }

            }
        }

        private void LoadTreeView()
        {
            DT_treeview = new DataTable();
            //___________DataTable preparation________//
            DataColumn DC_id = new DataColumn("ID");
            DataColumn DC_parentID = new DataColumn("ParentID");
            DataColumn DC_Name = new DataColumn("Name");
            DT_treeview.Columns.Add(DC_id);
            DT_treeview.Columns.Add(DC_parentID);
            DT_treeview.Columns.Add(DC_Name);
            //____________

            //DBEngine oDBEngine = new DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);
            TVOrgHirchy.Nodes.Clear();
            DataTable DT = new DataTable();

            //string HeadOfTheCompanyHead = oDBEngine.GetFieldValue(" tbl_master_employee, tbl_trans_employeeCTC ", " tbl_master_employee.emp_id ", " tbl_trans_employeeCTC.emp_reportTo=0 and tbl_trans_employeeCTC.emp_cntId=tbl_master_employee.emp_contactId ", 1)[0, 0];
            string HeadOfTheCompanyHead = oDBEngine.GetFieldValue("TBL_MASTER_BRANCH ", " BRANCH_ID ", " BRANCH_PARENTID=0 ", 1)[0, 0];

            //IdForColor = oDBEngine.GetFieldValue(" tbl_master_employee, tbl_trans_employeeCTC ", " tbl_master_employee.emp_id ", " tbl_trans_employeeCTC.emp_reportTo=0 and tbl_trans_employeeCTC.emp_cntId=tbl_master_employee.emp_contactId ", 1);
            IdForColor = oDBEngine.GetFieldValue(" TBL_MASTER_BRANCH ", " BRANCH_ID ", " BRANCH_PARENTID=0  ", 1);

            //DT = oDBEngine.GetDataTable(" tbl_trans_employeeCTC INNER JOIN tbl_master_employee ON tbl_trans_employeeCTC.emp_cntId = tbl_master_employee.emp_contactId INNER JOIN   tbl_master_contact ON tbl_master_employee.emp_contactId = tbl_master_contact.cnt_internalId INNER JOIN    tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id INNER JOIN   tbl_master_company ON tbl_trans_employeeCTC.emp_Organization = tbl_master_company.cmp_id INNER JOIN   tbl_master_designation ON tbl_trans_employeeCTC.emp_Designation = tbl_master_designation.deg_id INNER JOIN tbl_master_user ON tbl_trans_employeeCTC.emp_cntId = tbl_master_user.user_contactId   ", " tbl_master_employee.emp_id, tbl_master_contact.cnt_firstName + ' ' + tbl_master_contact.cnt_lastName + '  [' + tbl_master_company.cmp_Name + ' : ' + tbl_master_branch.branch_description  + ' : ' + tbl_master_designation.deg_designation+']'  AS Name, tbl_master_user.user_contactId ", " tbl_master_employee.emp_id =" + HeadOfTheCompanyHead);
            DT = oDBEngine.GetDataTable(" TBL_MASTER_BRANCH  ", "  BRANCH_ID,BRANCH_DESCRIPTION,BRANCH_INTERNALID ", " BRANCH_ID =" + HeadOfTheCompanyHead);

            DataRow DR1 = DT_treeview.NewRow();
            DR1["ID"] = DT.Rows[0][0].ToString();
            DR1["ParentID"] = "0";
            DR1["Name"] = DT.Rows[0][1].ToString();

            DT_treeview.Rows.Add(DR1);

            TreeNode TreeNode = new TreeNode();
            TreeNode.Value = "0";
            TreeNode.Text = DT.Rows[0][1].ToString();
            TVOrgHirchy.Nodes.Add(TreeNode);

            TreeNode tNode = new TreeNode();
            tNode = TVOrgHirchy.Nodes[0];
            populatetreenview(DT.Rows[0][0].ToString(), tNode);
            //TVOrgHirchy.ExpandAll();

            TVorgHir.DataSource = DT_treeview.DefaultView;
            TVorgHir.DataBind();
        }




        private void LoadTreeViewWithCondition()
        {
            DT_treeview = new DataTable();
            //___________DataTable preparation________//
            DataColumn DC_id = new DataColumn("ID");
            DataColumn DC_parentID = new DataColumn("ParentID");
            DataColumn DC_Name = new DataColumn("Name");
            DT_treeview.Columns.Add(DC_id);
            DT_treeview.Columns.Add(DC_parentID);
            DT_treeview.Columns.Add(DC_Name);
            //____________

            //DBEngine oDBEngine = new DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);
            TVOrgHirchy.Nodes.Clear();
            DataTable DT = new DataTable();
            string HeadOfThebranch = "";
            DataTable HeadOfTheCompany = new DataTable();
            if (cmbCompany.SelectedItem.Value.ToString().Trim() != "All" && cmbBranch.Value.ToString().Trim() != "All")
            {

                //HeadOfThebranch = oDBEngine.GetFieldValue(" TBL_MASTER_BRANCH ", " BRANCH_ID ", " BRANCH_Country='" + cmbCompany.SelectedItem.Value.ToString() + "' and  branch_regionid= '" + cmbBranch.Value.ToString() + "' and BRANCH_PARENTID not in (SELECT BRANCH_ID FROM TBL_MASTER_BRANCH WHERE BRANCH_Country='" + cmbCompany.SelectedItem.Value.ToString() + "' )", 1)[0, 0];
                //IdForColor = oDBEngine.GetFieldValue(" TBL_MASTER_BRANCH ", "BRANCH_ID ", " BRANCH_ID='" + HeadOfThebranch + "'", 1);

                HeadOfThebranch = oDBEngine.GetFieldValue(" TBL_MASTER_BRANCH ", " BRANCH_ID ", " BRANCH_Country='" + cmbCompany.SelectedItem.Value.ToString() + "' and  branch_regionid= '" + cmbBranch.Value.ToString() + "' ", 1)[0, 0];
                if (HeadOfThebranch != "n")
                {
                    IdForColor = oDBEngine.GetFieldValue(" TBL_MASTER_BRANCH ", "BRANCH_ID ", " BRANCH_ID='" + HeadOfThebranch + "'", 1);
                }

            }



            if (cmbCompany.SelectedItem.Value.ToString() != "All" && cmbBranch.Value.ToString() != "All")
            {
                if (HeadOfThebranch != "n")
                {
                    DT = oDBEngine.GetDataTable(" TBL_MASTER_BRANCH  ", "BRANCH_ID,BRANCH_DESCRIPTION,BRANCH_INTERNALID ", " branch_id ='" + HeadOfThebranch + "' and BRANCH_country='" + cmbCompany.SelectedItem.Value.ToString() + "' ");

                    if (DT.Rows.Count > 0)
                    {
                        DataRow DR1 = DT_treeview.NewRow();
                        DR1["ID"] = DT.Rows[0][0].ToString();
                        DR1["ParentID"] = "0";
                        DR1["Name"] = DT.Rows[0][1].ToString();

                        DT_treeview.Rows.Add(DR1);

                        TreeNode TreeNode = new TreeNode();
                        TreeNode.Value = "0";
                        TreeNode.Text = DT.Rows[0][1].ToString();
                        TVOrgHirchy.Nodes.Add(TreeNode);

                        TreeNode tNode = new TreeNode();
                        tNode = TVOrgHirchy.Nodes[0];
                        populatetreenviewwithcondition(DT.Rows[0][0].ToString(), tNode, cmbBranch.Value.ToString(), cmbCompany.SelectedItem.Value.ToString());

                        TVOrgHirchy.ExpandAll();


                        TVorgHir.DataSource = DT_treeview.DefaultView;
                        TVorgHir.DataBind();
                    }
                }
            }



        }

        private void populatetreenview(string ParentID, TreeNode tNode)
        {
            try
            {

                //DataRow parentRow = new DataRow();
                DataTable ParentTable = new DataTable();    //___child users of ParentID
                // ParentTable = oDBEngine.GetDataTable(" tbl_trans_employeeCTC INNER JOIN   tbl_master_employee ON tbl_trans_employeeCTC.emp_cntId = tbl_master_employee.emp_contactId INNER JOIN   tbl_master_contact ON tbl_master_employee.emp_contactId = tbl_master_contact.cnt_internalId INNER JOIN  tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id INNER JOIN  tbl_master_company ON tbl_trans_employeeCTC.emp_Organization = tbl_master_company.cmp_id INNER JOIN  tbl_master_designation ON tbl_trans_employeeCTC.emp_Designation = tbl_master_designation.deg_id ", " tbl_master_employee.emp_id, tbl_master_contact.cnt_firstName + ' ' + tbl_master_contact.cnt_lastName + '  [' + tbl_master_company.cmp_Name + ' : ' + tbl_master_branch.branch_description + ' : ' + tbl_master_designation.deg_designation + ']' AS Name ", " tbl_trans_employeeCTC.emp_reportTo=" + ParentID + " and tbl_trans_employeeCTC.emp_effectiveuntil is null AND ((tbl_master_employee.emp_dateofLeaving IS NULL OR tbl_master_employee.emp_dateofLeaving = '1/1/1900' OR tbl_master_employee.emp_dateofLeaving = '01/01/1900' OR tbl_master_employee.emp_dateofLeaving is null)) ", " tbl_master_contact.cnt_firstName ");
                ParentTable = oDBEngine.GetDataTable(" TBL_MASTER_BRANCH ", "  BRANCH_ID,BRANCH_DESCRIPTION,BRANCH_INTERNALID ", " BRANCH_PARENTID=" + ParentID + " ", " BRANCH_DESCRIPTION ");
                if (ParentTable.Rows.Count > 0)
                {
                    for (int i = 0; i < ParentTable.Rows.Count; i++)
                    {
                        DataRow DR_new = DT_treeview.NewRow();
                        string name = "";
                        TreeNode parentNode = new TreeNode();
                        parentNode = new TreeNode(ParentTable.Rows[i][1].ToString());// + "[" + childidsSplited.Length.ToString() + "]");
                        name = ParentTable.Rows[i][1].ToString();// +"[" + childidsSplited.Length.ToString() + "]";

                        parentNode.ImageUrl = "";

                        //___data row items_////
                        DR_new["ID"] = ParentTable.Rows[i][0].ToString();
                        DR_new["ParentID"] = ParentID;
                        DR_new["Name"] = name;
                        DT_treeview.Rows.Add(DR_new);
                        //______________________
                        populatetreenview(ParentTable.Rows[i][0].ToString(), parentNode);
                    }
                }

            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
            }
        }
        private void populatetreenviewwithcondition(string ParentID, TreeNode tNode, string BranchId, string CompanyId)
        {
            int i = 0;
            try
            {

                string WhereClause = "";
                DataTable ParentTable = new DataTable();    //___child users of ParentID


                ParentTable = oDBEngine.GetDataTable(" TBL_MASTER_BRANCH ", " BRANCH_ID,BRANCH_DESCRIPTION,BRANCH_INTERNALID ", " BRANCH_PARENTID=" + ParentID + "  and  branch_regionid='" + BranchId.ToString() + "' and BRANCH_country = '" + CompanyId.ToString() + "' ", " BRANCH_DESCRIPTION");

                if (ParentTable.Rows.Count > 0)
                {
                    for (i = 0; i < ParentTable.Rows.Count; i++)
                    {
                        DataRow DR_new = DT_treeview.NewRow();
                        string name = "";
                        TreeNode parentNode = new TreeNode();
                        parentNode = new TreeNode(ParentTable.Rows[i][1].ToString());// + "[" + childidsSplited.Length.ToString() + "]");
                        name = ParentTable.Rows[i][1].ToString();// +"[" + childidsSplited.Length.ToString() + "]";
                        parentNode.ImageUrl = "";
                        //___data row items_////
                        DR_new["ID"] = ParentTable.Rows[i][0].ToString();
                        DR_new["ParentID"] = ParentID;
                        DR_new["Name"] = name;
                        DT_treeview.Rows.Add(DR_new);
                        //______________________

                        populatetreenviewwithcondition(ParentTable.Rows[i][0].ToString(), parentNode, BranchId, CompanyId);
                    }
                }

            }


            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
            }
        }

        protected void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbExport.Value.ToString() != "")
            {
                if (cmbExport.Value.ToString() == "Pdf")
                    ASPxTreeListExporter1.WritePdfToResponse();
                if (cmbExport.Value.ToString() == "Xls")
                    ASPxTreeListExporter1.WriteXlsToResponse();
                if (cmbExport.Value.ToString() == "Rtf")
                    ASPxTreeListExporter1.WriteRtfToResponse();

                cmbExport.SelectedIndex = 0;
            }
        }

        protected void TVorgHir_CustomCallback(object sender, DevExpress.Web.ASPxTreeList.TreeListCustomCallbackEventArgs e)
        {
            if (cmbBranch.Value != null)
            {

                if (cmbCompany.SelectedItem.Value.ToString() == "All" && cmbBranch.Value.ToString() == "All")
                {
                    LoadTreeView();
                }
                else
                {
                    LoadTreeViewWithCondition();
                }
            }

        }

        protected void TVorgHir_HtmlDataCellPrepared(object sender, TreeListHtmlDataCellEventArgs e)
        {

            //if (cmbCompany.SelectedItem.Value.ToString() == "All" && cmbBranch.Value.ToString() == "All")
            //{
            //    if (e.Column.FieldName == "Name")
            //    {
            //        string id = e.NodeKey.ToString();

            //        string[,] ids = oDBEngine.GetFieldValue(" TBL_MASTER_BRANCH ", " branch_id", " branch_id=" + id, 1);
            //        int len = 0;
            //        if (ids[0, 0].ToString() != "n")
            //        {
            //            string temp = "";
            //            string childuser = oDBEngine.getAllEmployeeInHierarchy(ids[0, 0], temp);

            //            len = childuser.Split(',').Length;
            //        }
            //        string value = e.CellValue.ToString();
            //        if (len > 0)
            //            e.Cell.Text = value + "[" + len.ToString() + "]";
            //        else
            //            e.Cell.Text = value + "[1]";
            //    }
            //}
            //else
            //{
            //    if (e.Column.FieldName == "Name")
            //    {
            //        string id = e.NodeKey.ToString();

            //        string[,] ids = oDBEngine.GetFieldValue(" TBL_MASTER_BRANCH ", " branch_id ", " branch_id=" + id, 1);
            //        int len = 0;
            //        if (ids[0, 0].ToString() != "n")
            //        {
            //            string temp = "";

            //            DataTable dcompany = new DataTable();

            //            dcompany = oDBEngine.GetDataTable(" tbl_master_employee inner join tbl_trans_employeectc on tbl_master_employee.emp_contactid=tbl_trans_employeectc.emp_cntid  ", " emp_Organization ", " tbl_master_employee.emp_id='" + id + "'", "emp_organization");
            //            string childuser = oDBEngine.getAllEmployeeInHierarchy(ids[0, 0], temp, cmbBranch.Value.ToString(), dcompany.Rows[0][0].ToString());

            //            len = childuser.Split(',').Length;
            //            string value = e.CellValue.ToString();
            //            if (len > 0)
            //                e.Cell.Text = value + "[" + len.ToString() + "]";
            //            else
            //                e.Cell.Text = value + "[1]";

            //        }

            //    }

            //}
            //for (int l = 0; l < IdForColor.Length; l++)
            //{
            //    if (e.NodeKey.ToString() == IdForColor[l, 0].ToString())
            //    {
            //        e.Cell.BackColor = System.Drawing.Color.Bisque;
            //    }
            //}
        }
        protected void TVorgHir_CustomJSProperties(object sender, TreeListCustomJSPropertiesEventArgs e)
        {
            e.Properties["cpHeight"] = "a";
        }
        protected void TVOrgHirchy_SelectedNodeChanged(object sender, EventArgs e)
        {

        }

        protected void cmbBranch_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            string CountryID = e.Parameter;
            if (CountryID != "All")
            {
                string[,] branches = oDBEngine.GetFieldValue(" TBL_MASTER_REGIONS ", " reg_id,reg_region ", " reg_countryid ='" + CountryID + "' ", 2, " reg_region ");
                clsdrp.AddDataToDropDownList(branches, cmbBranch, "All");
                cmbBranch.SelectedIndex = 0;
            }
            else
            {


                string[,] r = new string[1, 2];
                r[0, 0] = "All";
                r[0, 1] = "All";
                clsdrp.AddDataToDropDownList(r, cmbBranch, "");
            }
        }
    }
}