using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using DevExpress.Web;
using DevExpress.Web.ASPxTreeList;
using System.Configuration;
using BusinessLogicLayer;
using ClsDropDownlistNameSpace;

namespace ERP.OMS.Management
{
    public partial class management_root_UserGroup1 : System.Web.UI.Page
    {
        //DBEngine oDBEngine = new DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);
        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);MULTI
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        string data;
        int RowNo = 0;
        DataTable DT_tree = new DataTable();
        string[,] dummy = new string[1, 1];
        public string pageAccess = "";
        string SelectedIds = "";
        string Checking = "";

        clsDropDownList OclsDropDownList = new clsDropDownList();

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
            // Page.ClientScript.RegisterStartupScript(GetType(), "pageload", "<script>pageload();</script>");
            if (HttpContext.Current.Session["userid"] == null)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
            if (!IsPostBack)
            {
                ListSection.Visible = false;
                btnCancel.Visible = false;
                EditSection.Visible = false;
                trSelect.Visible = false;
                tr_2nd.Visible = false;
                //btnDelete.Visible = false;
                td_onlyadd.Visible = false;
                td_cancelonly.Visible = false;
                Session["KeyVal"] = "";
                fillGrid();
                //this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");
            }
            //lblMessage.Text = "";
            dummy[0, 0] = "n";
            if (Session["KeyVal"].ToString() != "")
            {
                string[,] SelectMenu = selectedMenu();
                // string SelectSubMenu = (string)selectedSubMenu().ToString();
                LoadTreeView(cmbSegmentForAdd.SelectedItem.Value, SelectMenu);
                TLgrid.DataSource = DT_tree.DefaultView;
                TLgrid.DataBind();
                TLgrid.ExpandAll();
            }
            else
            {

                LoadTreeView(cmbSegmentForAdd.SelectedValue, dummy);
                TLgrid.DataSource = DT_tree.DefaultView;
                TLgrid.DataBind();
                TLgrid.ExpandAll();

                fillGridforfilter();
                this.Page.ClientScript.RegisterStartupScript(GetType(), "heightZZ", "<script>height();</script>");
            }
        }
        protected void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            fillGrid();
            Int32 Filter = int.Parse(cmbExport.SelectedItem.Value.ToString());
            switch (Filter)
            {
                case 1:
                    exporter.WritePdfToResponse();
                    break;
                case 2:
                    exporter.WriteXlsToResponse();
                    break;
                case 3:
                    exporter.WriteRtfToResponse();
                    break;
                case 4:
                    exporter.WriteCsvToResponse();
                    break;
            }
        }
        //protected void GridUserGroup_CustomJSProperties(object sender, DevExpress.Web.ASPxGridViewClientJSPropertiesEventArgs e)
        //{

        //    //e.Properties["cpHeight"] = "2";
        //    //fillGrid();
        //    //fillGrid();
        //    this.Page.ClientScript.RegisterStartupScript(GetType(), "heightk", "<script>height();</script>");


        //}
        protected void GridUserGroup_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            //fillGrid();
            GridUserGroup.ClearSort();
            if (e.Parameters == "s")
            {
                GridUserGroup.Settings.ShowFilterRow = true;
                fillGrid();
            }
            else
            {
                GridUserGroup.FilterExpression = string.Empty;
                fillGrid();
            }
        }
        private void fillGrid()
        {

            DataTable DT = oDBEngine.GetDataTable(" tbl_master_userGroup", " grp_id,grp_name,grp_segmentId,(select seg_name from tbl_master_segment where seg_id=grp_segmentId) as segname ", null, "grp_segmentId");

            GridUserGroup.DataSource = DT.DefaultView;
            GridUserGroup.DataBind();
            string[,] data = oDBEngine.GetFieldValue(" tbl_master_segment", " seg_id,seg_name ", null, 2, " seg_name");
            // oDBEngine.AddDataToDropDownList(data, cmbSegmentForAdd);
            OclsDropDownList.AddDataToDropDownList(data, cmbSegmentForAdd);

        }
        private void fillGridforfilter()
        {

            DataTable DT = oDBEngine.GetDataTable(" tbl_master_userGroup", " grp_id,grp_name,grp_segmentId,(select seg_name from tbl_master_segment where seg_id=grp_segmentId) as segname ", null, "grp_segmentId");

            GridUserGroup.DataSource = DT.DefaultView;
            GridUserGroup.DataBind();
            //string[,] data = oDBEngine.GetFieldValue(" tbl_master_segment", " seg_id,seg_name ", null, 2, " seg_name");
            //oDBEngine.AddDataToDropDownList(data, cmbSegmentForAdd);
        }
        //protected void GridUserGroup_RowDataBound(object sender, GridViewRowEventArgs e)
        //{
        //    if (e.Row.RowType == DataControlRowType.DataRow)    //___This condition is for filtering header and footer, application will done on Rows only___//
        //    {
        //        CheckBox chkbox = new CheckBox();
        //        chkbox = (CheckBox)e.Row.FindControl("chkDel");
        //        Label lblId = new Label();
        //        lblId = (Label)e.Row.FindControl("lblId");
        //        chkbox.Attributes.Add("onclick", "javascript:chkclicked(this,'" + lblId.Text + "');");
        //    }
        //}
        //protected void GridUserGroup_PageIndexChanging(object sender, GridViewPageEventArgs e)
        //{
        //    //GridUserGroup.PageIndex = e.NewPageIndex;
        //   e.NewPageIndex;
        //    fillGrid();

        //}

        protected void btnAdd_Mod_Click(object sender, EventArgs e)
        {

            Session["KeyVal"] = "";
            EditSection.Visible = true;
            btnCancel.Visible = false;
            //btnDelete.Visible = false;
            ListSection.Visible = false;
            //trSelect.Visible = true;
            tr_2nd.Visible = false;
            trSelect.Visible = false;
            tr_3rd.Visible = true;
            td_onlyadd.Visible = true;
            td_cancelonly.Visible = false;
            td_false.Visible = false;

            //lblMessage.Text = "";
            // checkForID();

            if (Session["KeyVal"].ToString() != "")
            {
                string[,] data = oDBEngine.GetFieldValue(" tbl_master_userGroup", " grp_name,grp_segmentId ", " grp_id=" + Session["KeyVal"], 2, "grp_name");
                if (data[0, 0] != "n")
                {
                    txtGroupName.Text = data[0, 0].ToString();
                    cmbSegmentForAdd.SelectedValue = data[0, 1].ToString();
                }
                ClearCheckedList();
                ////____________This is for treelist__//

                string[,] SelectMenu = selectedMenu();
                //string SelectSubMenu = selectedSubMenu().ToString();
                LoadTreeView(cmbSegmentForAdd.SelectedItem.Value, SelectMenu);
                ////____________End___________________//


            }
            else
            {
                LoadTreeView(cmbSegmentForAdd.SelectedValue, dummy);
                txtGroupName.Text = "";
                txtGroupName.Enabled = true;
                cmbSegmentForAdd.Enabled = true;
                EditSection.Disabled = false;

            }
            TLgrid.DataSource = DT_tree.DefaultView;
            TLgrid.DataBind();
            TLgrid.ExpandAll();
            this.Page.ClientScript.RegisterStartupScript(GetType(), "height", "<script>height();</script>");
        }

        private void LoadTreeView(string Segment, string[,] AllowedMenu)
        {
            //________Data table for tree grid____//
            if (DT_tree.Columns.Count == 0)
            {
                DataColumn col1 = new DataColumn("name");
                DataColumn col2 = new DataColumn("menuID");
                DataColumn col3 = new DataColumn("menuParentID");
                DataColumn col4 = new DataColumn("checked", typeof(Boolean));
                DataColumn col5 = new DataColumn("Mode");
                //DataColumn col6 = new DataColumn("HaveSubMenu");
                DT_tree.Columns.Add(col1);
                DT_tree.Columns.Add(col2);
                DT_tree.Columns.Add(col3);
                DT_tree.Columns.Add(col4);
                DT_tree.Columns.Add(col5);
                // DT_tree.Columns.Add(col6);
            }
            else
                DT_tree.Rows.Clear();

            DataTable DT = oDBEngine.GetDataTable(" tbl_trans_menu ", " mnu_id, mnu_menuName, mun_parentId,mnu_menuLink,mnu_HaveSubMenu ", "  mnu_segmentId=" + Segment);
            PopulateTreeView(0, Segment, AllowedMenu, DT);
        }

        private void PopulateTreeView(int Node, string Segment, string[,] AllowedMenu, DataTable Allmenu)
        {
            string expression = "mun_parentId=" + Node;
            DataRow[] FilteredMenu = Allmenu.Select(expression);
            if (FilteredMenu.Length != 0)
            {
                for (int i = 0; i < FilteredMenu.Length; i++)
                {

                    DataRow rownew = DT_tree.NewRow();
                    rownew["name"] = FilteredMenu[i]["mnu_menuName"].ToString();
                    rownew["menuID"] = FilteredMenu[i]["mnu_id"].ToString();
                    rownew["menuParentID"] = FilteredMenu[i]["mun_parentId"].ToString();
                    string returnedVal = Allowed_submenus(AllowedMenu, FilteredMenu[i]["mnu_id"].ToString());
                    string[] rtval = returnedVal.Split('~');
                    if (rtval[0] != "n")
                    {
                        rownew["checked"] = true;
                        rownew["Mode"] = rtval[1];
                    }
                    else
                    {
                        rownew["checked"] = false;
                        rownew["Mode"] = "All";
                    }

                    //rownew["HaveSubMenu"] = FilteredMenu[i]["mnu_HaveSubMenu"].ToString();
                    DT_tree.Rows.Add(rownew);

                    PopulateTreeView(int.Parse(FilteredMenu[i]["mnu_id"].ToString()), Segment, AllowedMenu, Allmenu);
                }
            }
        }

        private string Allowed_submenus(string[,] Allowed_menu, string menu)
        {
            string status = "n~n";
            if (Allowed_menu.Length > 0)
            {
                for (int i = 0; i < Allowed_menu.Length / 2; i++)
                {
                    if (Allowed_menu[i, 0].ToString() == menu)
                    {
                        status = "y~" + Allowed_menu[i, 1].ToString();
                        break;
                    }
                }
            }
            return status;
        }

        private string[,] selectedMenu()
        {
            string[,] data = oDBEngine.GetFieldValue(" tbl_trans_access ", " acc_menuId,acc_view ", " acc_userGroupId=" + Session["KeyVal"], 2);

            return data;
        }

        //private object selectedSubMenu()
        //{
        //    //DBEngine oDBEngine = new DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);
        //    string[,] data = oDBEngine.GetFieldValue(" tbl_trans_subaccess ", " sac_submenuId ", " sac_userGroupId=" + Session["KeyVal"], 1);
        //    string ID = "";
        //    if (data[0, 0] != "n")
        //    {

        //        for (int i = 0; i < data.Length; i++)
        //        {
        //            if (ID != "")
        //                ID = ID + "," + data[0, 0].ToString();
        //            else
        //                ID = data[0, 0].ToString();
        //        }
        //    }
        //    return ID;
        //}

        private void ClearCheckedList()
        {
            //if (GridUserGroup.Rows.Count > 0)
            //{
            //    for (int i = 0; i < GridUserGroup.Rows.Count; i++)
            //    {
            //        GridViewRow rownew = GridUserGroup.Rows[i];
            //        CheckBox chkbox = new CheckBox();
            //        chkbox = (CheckBox)rownew.FindControl("chkDel");
            //        if (chkbox.Checked)
            //        {
            //            chkbox.Checked = false;
            //        }
            //    }
            //}

            for (int i = 0; i < GridUserGroup.VisibleRowCount; i++)
            {
                //DataView dr= gridJournalVouchar.GetDataRow(i);//.GetRow(i);

                if (GridUserGroup.Selection.IsRowSelected(i))
                {
                    if (SelectedIds == "")
                        SelectedIds = GridUserGroup.GetRowValues(i, "grp_id").ToString();
                    else
                        SelectedIds += "," + GridUserGroup.GetRowValues(i, "grp_id").ToString();
                }

            }
        }
        private void checkForID()
        {
            //if (GridUserGroup.Rows.Count > 0)
            //{
            //    for (int i = 0; i < GridUserGroup.Rows.Count; i++)
            //    {
            //        GridViewRow rownew = GridUserGroup.Rows[i];
            //        CheckBox chkbox = new CheckBox();
            //chkbox = (CheckBox)rownew.FindControl("chkDel");
            //        if (chkbox.Checked)
            //        {
            //            Label lblId = new Label();
            //            lblId = (Label)rownew.FindControl("lblId");
            //            Session["KeyVal"] = lblId.Text;
            //        }
            //    }
            //}
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            EditSection.Visible = false;
            ListSection.Visible = false;
            btnCancel.Visible = false;
            //btnDelete.Visible = true;
            //btnDelete.Visible = false;
            trSelect.Visible = false;
            tr_2nd.Visible = false;
            Session["KeyVal"] = "";
            txtGroupName.Text = "";
            td_onlyadd.Visible = false;
            td_cancelonly.Visible = false;
            td_false.Visible = true;
            //cmbSegmentForAdd.SelectedIndex = 0;
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            string[,] userID;
            string userID1 = "";
            int NoOfRowsEffected;
            ListSection.Visible = true;
            int noofupdate = 0;
            if (Session["KeyVal"].ToString() != "")
            {
                //if (Session["PageAccess"].ToString().Trim() == "Add" || Session["PageAccess"].ToString().Trim() == "Modify" || Session["PageAccess"].ToString().Trim() == "All")
                //{
                //noofupdate = oDBEngine.SetFieldValue("tbl_master_userGroup", "grp_name='" + txtGroupName.Text.ToString().Trim() + "',lastmodifydate='" + oDBEngine.GetDate().ToString() + "',lastmodifyuser='" + Session["userid"].ToString() + "'", "grp_id='" + Session["KeyVal"].ToString() + "'");
                string fieldWval = " grp_name='" + txtGroupName.Text + "',grp_segmentId=" + cmbSegmentForAdd.SelectedValue;
                NoOfRowsEffected = oDBEngine.DeleteValue(" tbl_trans_access ", " acc_userGroupId=" + Session["KeyVal"]);

                NoOfRowsEffected = oDBEngine.SetFieldValue(" tbl_master_userGroup ", fieldWval, " grp_id=" + Session["KeyVal"]);
                userID1 = Session["KeyVal"].ToString();
                //}
                //else
                //{
                //    Page.ClientScript.RegisterStartupScript(GetType(), "OnClick", "<script language='javascript'> alert('Not Authorised To Modify Records!') </script>");
                //}
            }
            else
            {
                //if (Session["PageAccess"].ToString().Trim() == "All" || Session["PageAccess"].ToString().Trim() == "Add" || Session["PageAccess"].ToString().Trim() == "DelAdd")
                //{
                ////////string fields = " grp_name,grp_segmentId,CreateDate,CreateUser ";
                ////////string value = "'" + txtGroupName.Text + "'," + cmbSegmentForAdd.SelectedValue + ",'" + oDBEngine.GetDate() + "'," + HttpContext.Current.Session["userid"];
                ////////NoOfRowsEffected = oDBEngine.InsurtFieldValue(" tbl_master_userGroup ", fields, value);
                userID = oDBEngine.GetFieldValue(" tbl_master_userGroup", " Top 1 grp_id ", " grp_name='" + Session["name"] + "' and CreateUser='" + Session["userid"] + "'", 1, " grp_id desc");
                userID1 = userID[0, 0].ToString();
                Session["name"] = null;
                //}
                //else
                //{
                //    Page.ClientScript.RegisterStartupScript(GetType(), "OnClick", "<script language='javascript'> alert('Not Authorised To Add Records!') </script>");

                //}
            }

            List<TreeListNode> nodes = TLgrid.GetSelectedNodes();
            try
            {
                for (int i = 0; i < nodes.Count; i++)
                {
                    string nodekey = nodes[i]["menuID"].ToString();
                    ASPxTextBox txttype = (ASPxTextBox)TLgrid.FindDataCellTemplateControl(nodekey, null, "ASPxTextBox1");
                    string val = txttype.Text;

                    NoOfRowsEffected = oDBEngine.InsurtFieldValue(" tbl_trans_access ", " acc_userGroupId,acc_menuId,acc_view,CreateDate,CreateUser ", userID1 + "," + nodekey + ",'" + val + "',getdate()," + HttpContext.Current.Session["userid"]);
                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "exception", "<script language='javascript'> alert('" + ex.Message + "') </script>");
            }



            fillGrid();
            txtGroupName.Text = "";
            cmbSegmentForAdd.SelectedIndex = 0;
            EditSection.Visible = false;
            ListSection.Visible = false;
            btnCancel.Visible = false;
            tr_3rd.Visible = false;
            // btnDelete.Visible = true;
            // btnDelete.Visible = false;
            trSelect.Visible = false;
            tr_2nd.Visible = false;
            td_onlyadd.Visible = false;
            td_cancelonly.Visible = false;
            td_false.Visible = true;

            txtGroupName.Enabled = true;
            cmbSegmentForAdd.Enabled = true;
            EditSection.Disabled = false;
            this.Page.ClientScript.RegisterStartupScript(GetType(), "height1", "<script>height();</script>");

            //this.Page.ClientScript.RegisterStartupScript(GetType(), "Reload", "<script>Reload();</script>");
            //Page.ClientScript.RegisterStartupScript(GetType(), "Reload1", "<script language='javascript'> Reload() </script>");
        }

        //protected void btnDelete_Click(object sender, EventArgs e)
        //{
        //    //checkForID();
        //    if (Session["KeyVal"].ToString() != "")
        //    {
        //        int noOfRowEffected = oDBEngine.DeleteValue(" tbl_master_userGroup ", " grp_id=" + Session["KeyVal"].ToString());

        //    }
        //    else
        //       // lblMessage.Text = "Please Select an User Group !";
        //    fillGrid();
        //}

        protected void cmbSegmentForAdd_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Session["KeyVal"].ToString() != "")
            {
                string[,] SelectMenu = selectedMenu();
                //string SelectSubMenu = (string)selectedSubMenu().ToString();
                LoadTreeView(cmbSegmentForAdd.SelectedItem.Value, SelectMenu);
            }
            else
                LoadTreeView(cmbSegmentForAdd.SelectedValue, dummy);
            TLgrid.DataSource = DT_tree.DefaultView;
            TLgrid.DataBind();
            TLgrid.ExpandAll();
            ListSection.Visible = false;
            EditSection.Visible = true;
            tr_2nd.Visible = false;
            trSelect.Visible = false;
            td_onlyadd.Visible = true;
            td_cancelonly.Visible = false;
            Page.ClientScript.RegisterStartupScript(GetType(), "last", "<script>height();</script>");
        }

        //protected void TLgrid_HtmlDataCellPrepared(object sender, TreeListHtmlDataCellEventArgs e)
        //{
        //    if (e.Column.FieldName != "HaveSubMenu")// return;
        //    {
        //        string val = e.GetValue("HaveSubMenu").ToString();
        //        if (e.GetValue("HaveSubMenu").ToString() != "Y" || e.GetValue("HaveSubMenu").ToString() == "")
        //        {
        //            ASPxHyperLink link = TLgrid.FindDataCellTemplateControl(e.NodeKey, TLgrid.Columns["HaveSubMenu"] as TreeListDataColumn, "ASPxHyperLink1") as ASPxHyperLink;
        //            //link.Enabled = false;
        //            link.Visible = false;
        //        }
        //    }


        //}
        protected void TLgrid_HtmlRowPrepared(object sender, TreeListHtmlRowEventArgs e)
        {
            if (e.RowKind == TreeListRowKind.Data)
            {
                if (Boolean.Parse(e.GetValue("checked").ToString()))
                {
                    TreeListNode node = TLgrid.FindNodeByKeyValue(e.NodeKey.ToString());// treelist.FindNodeByKeyValue(key.ToString());
                    node.Selected = true;
                }
            }
        }
        protected void TLgrid_CustomJSProperties(object sender, TreeListCustomJSPropertiesEventArgs e)
        {
            e.Properties["cpHeight"] = "2";
            //this.Page.ClientScript.RegisterStartupScript(GetType(), "heightxyz", "<script>height();</script>");
        }
        protected void GridUserGroup_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            //if (Session["KeyVal"].ToString() != "")
            //{
            string Code = e.Values[0].ToString();
            string code1 = e.Keys[0].ToString();
            string particular = '%' + code1 + '%';
            int noOfRowEffected = oDBEngine.DeleteValue(" tbl_master_userGroup ", " grp_id=" + code1.ToString());
            int noOfRowEffected1 = oDBEngine.DeleteValue(" tbl_trans_access ", " acc_userGroupId = " + code1.ToString());
            DataTable dtuser = oDBEngine.GetDataTable("select user_id,user_group from tbl_master_user where user_group like '" + particular + "' ");
            for (int i = 0; i < dtuser.Rows.Count; i++)
            {
                string usergroupid = dtuser.Rows[i]["user_group"].ToString().Trim();
                string userid = dtuser.Rows[i]["user_id"].ToString().Trim();
                usergroupid = usergroupid.Replace(code1, "");
                if (usergroupid.Contains(",,"))
                {
                    usergroupid = usergroupid.Replace(",,", ",");
                }
                if (usergroupid.StartsWith(","))
                {
                    //string sub= compare.Substring(0,1);
                    usergroupid = usergroupid.Remove(0, 1);
                }
                if (usergroupid.EndsWith(","))
                {
                    usergroupid = usergroupid.Remove(usergroupid.Length - 1) + "";
                }
                int nowofrowsaffested = oDBEngine.SetFieldValue("tbl_master_user", "user_group='" + usergroupid + "'", "user_id='" + userid + "'");
            }
            e.Cancel = true;
            //}
            //else
            //    lblMessage.Text = "Please Select an User Group !";
            fillGrid();

        }
        protected void GridUserGroup_RowCommand(object sender, ASPxGridViewRowCommandEventArgs e)
        {
            string Rowid = e.KeyValue.ToString();
            Session["KeyVal"] = Rowid;
            //EditSection.Visible = true;
            tr_2nd.Visible = true;
            btnCancel.Visible = true;
            // btnDelete.Visible = false;
            ListSection.Visible = true;
            trSelect.Visible = true;
            td_false.Visible = false;
            EditSection.Visible = true;
            cmbSegmentForAdd.Enabled = false;
            // td_false2.Visible = false;
            // GridUserGroup.Visible = false;
            // lblMessage.Text = "";
            tr_3rd.Visible = true;
            td_onlyadd.Visible = false;
            td_cancelonly.Visible = false;

            //LoadTreeView(cmbSegmentForAdd.SelectedValue, dummy, "");
            //TLgrid.DataSource = DT_tree.DefaultView;
            //TLgrid.DataBind();
            //TLgrid.ExpandAll();

            fillGrid();
            string[,] data = oDBEngine.GetFieldValue(" tbl_master_userGroup", " grp_name,grp_segmentId ", " grp_id=" + Session["KeyVal"], 2);
            if (data[0, 0] != "n")
            {
                txtGroupName.Text = data[0, 0].ToString();
                cmbSegmentForAdd.SelectedValue = data[0, 1].ToString();
            }

            //ClearCheckedList();
            ////____________This is for treelist__//

            string[,] SelectMenu = selectedMenu();
            //string SelectSubMenu = selectedSubMenu().ToString();
            LoadTreeView(cmbSegmentForAdd.SelectedItem.Value, SelectMenu);
            TLgrid.DataSource = DT_tree.DefaultView;
            TLgrid.DataBind();
            TLgrid.ExpandAll();
            chkAll.Checked = false;
            txtGroupName.Visible = true;
            txtGroupName.Enabled = true;
            this.Page.ClientScript.RegisterStartupScript(GetType(), "heightzzz", "<script>height();</script>");

        }
        protected void GridUserGroup_PageIndexChanging(object sender, EventArgs e)
        {
            fillGrid();
        }

        protected void btnsaveaccount_Click(object sender, EventArgs e)
        {
            string[,] userID;
            string userID1 = "";
            int NoOfRowsEffected;

            string name = txtGroupName.Text.Trim();
            Session["name"] = name;
            string[,] compare = oDBEngine.GetFieldValue("tbl_master_userGroup", "grp_name", "grp_name='" + name.ToString().Trim() + "' and grp_segmentId='" + cmbSegmentForAdd.SelectedValue + "'", 1);
            string fields = " grp_name,grp_segmentId,CreateDate,CreateUser ";
            //string value = "'" + name.ToString().Trim() + "'," + cmbSegmentForAdd.SelectedValue + ",'" + oDBEngine.GetDate() + "'," + HttpContext.Current.Session["userid"];
            string value = "'" + name.ToString().Trim() + "'," + cmbSegmentForAdd.SelectedValue + ",'" + oDBEngine.GetDate() + "'," + HttpContext.Current.Session["userid"];
            if (compare[0, 0] == "n")
            {
                NoOfRowsEffected = oDBEngine.InsurtFieldValue(" tbl_master_userGroup ", fields, value);
                ListSection.Visible = true;
                tr_2nd.Visible = true;
                td_onlyadd.Visible = false;
                btnCancel.Visible = false;
                EditSection.Disabled = true;
                trSelect.Visible = true;
                td_cancelonly.Visible = false;
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "JScript34", "<script language='javascript'>alert('Group name Already Exist');</script>");
                EditSection.Visible = false;
                ListSection.Visible = false;
                btnCancel.Visible = false;
                //btnDelete.Visible = true;
                //btnDelete.Visible = false;
                trSelect.Visible = false;
                tr_2nd.Visible = false;
                Session["KeyVal"] = "";
                txtGroupName.Text = "";
                td_onlyadd.Visible = false;
                tr_3rd.Visible = false;
                td_cancelonly.Visible = false;

            }
            chkAll.Checked = false;
            //userID = oDBEngine.GetFieldValue(" tbl_master_userGroup", " Top 1 grp_id ", " grp_name='" + Session["name"] + "' and CreateUser='"+ Session["userid"] +"'", 1, " grp_id desc");
            //userID1 = userID[0, 0].ToString();
            this.Page.ClientScript.RegisterStartupScript(GetType(), "height2", "<script>height();</script>");
        }
        protected void btncancelaccount_Click(object sender, EventArgs e)
        {
            EditSection.Visible = false;
            ListSection.Visible = false;
            btnCancel.Visible = false;
            //btnDelete.Visible = true;
            //btnDelete.Visible = false;
            trSelect.Visible = false;
            tr_2nd.Visible = false;
            Session["KeyVal"] = "";
            txtGroupName.Text = "";
            td_onlyadd.Visible = false;
            tr_3rd.Visible = false;
            td_cancelonly.Visible = false;
            td_false.Visible = true;
        }
        protected void GridUserGroup_BeforeGetCallbackResult(object sender, EventArgs e)
        {
            fillGrid();
        }
    }

}