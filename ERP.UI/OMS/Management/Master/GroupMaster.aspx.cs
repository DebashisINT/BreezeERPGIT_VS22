using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
////using DevExpress.Web.ASPxClasses;
//using DevExpress.Web;
using DevExpress.Web;
using System.Configuration;
using BusinessLogicLayer;
using EntityLayer.CommonELS;
using System.Web.Services;
using System.Collections.Generic;
using System.Data.SqlClient;


namespace ERP.OMS.Management.Master
{
    public partial class management_master_GroupMaster : ERP.OMS.ViewState_class.VSPage //System.Web.UI.Page
    {
        //DBEngine objEngine = new DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        //BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);MULTI
        BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine();
        public string pageAccess = "";
        string ForInsertUpdateChecking = null;
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();

        protected void Page_PreInit(object sender, EventArgs e)
        {
            // Code  Added  By Priti on 14122016 to use Convert.ToString instead of ToString()
            if (!IsPostBack)
            {
                //'http://localhost:2957/InfluxCRM/management/testProjectMainPage_employee.aspx'
                string sPath = Convert.ToString(HttpContext.Current.Request.Url);
                oDBEngine.Call_CheckPageaccessebility(sPath);
            }
        }
        protected void Page_Init(object sender, EventArgs e)
        {
            GroupMaster.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            SelectGroupOwner.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            PrincipleGroup.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            SelectOwner.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
 }
       
        protected void Page_Load(object sender, EventArgs e)
        {
             DBEngine objDb = new DBEngine();
             DataTable dtc = objDb.GetDataTable("select COUNT(gpm_id) Number from tbl_master_groupMaster");
             hdnbeforesave.Value = Convert.ToString(dtc.Rows[0]["Number"]);
            //------- For Read Only User in SQL Datasource Connection String   Start-----------------
             rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/Master/groupmaster.aspx");
          
            if (HttpContext.Current.Session["EntryProfileType"] != null)
            {
                if (Convert.ToString(HttpContext.Current.Session["EntryProfileType"]) == "R")
                {
                    //GroupMaster.ConnectionString = ConfigurationSettings.AppSettings["DBReadOnlyConnection"];MULTI
                }
                else
                {
                    //GroupMaster.ConnectionString = ConfigurationSettings.AppSettings["DBConnectionDefault"];MULTI
                }
            }

            //------- For Read Only User in SQL Datasource Connection String   End-----------------

            if (HttpContext.Current.Session["userid"] == null)
            {
                //Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }

            //this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");

            GroupMasterGrid.JSProperties["cpDelmsg"] = null;
            if(!IsPostBack)
            {
                Session["exportval"] = null;
            }
        }
       
        protected void GroupMasterGrid_AutoFilterCellEditorInitialize(object sender, DevExpress.Web.ASPxGridViewEditorEventArgs e)
        {
            if (e.Column.FieldName == "PrincipalGroup")
            {
                (e.Editor as ASPxComboBox).DataBound += new EventHandler(editCombo_DataBound);
            }
        }
        protected void GroupMasterGrid_CellEditorInitialize(object sender, DevExpress.Web.ASPxGridViewEditorEventArgs e)
        {
            if (e.Column.FieldName == "PrincipalGroup")
            {
                (e.Editor as ASPxComboBox).DataBound += new EventHandler(filterCombo_DataBound);
            }
            if (e.Column.FieldName == "GroupOwner")
            {
                if (e.KeyValue != null)
                {
                    object val = GroupMasterGrid.GetRowValuesByKeyValue(e.KeyValue, "GroupType");
                    if (val == DBNull.Value) return;
                    string country = (string)val;
                    ASPxComboBox combo = e.Editor as ASPxComboBox;
                    FillStateCombo(combo, country);

                    combo.Callback += new CallbackEventHandlerBase(cmbState_OnCallback);
                }
                else
                {

                    object val = GroupMasterGrid.GetRowValues(0, "GroupType");
                    if (val != null)
                    {

                        string country = (string)val;
                        ASPxComboBox combo = e.Editor as ASPxComboBox;
                        FillStateCombo(combo, country);

                        combo.Callback += new CallbackEventHandlerBase(cmbState_OnCallback);
                    }
                    else
                    {

                        string country = "Family";
                        ASPxComboBox combo = e.Editor as ASPxComboBox;
                        FillStateCombo(combo, country);

                        combo.Callback += new CallbackEventHandlerBase(cmbState_OnCallback);
                    }
                }
            }

        }
        protected void FillStateCombo(ASPxComboBox cmb, string country)
        {

            string[,] state = GetState(country);
            cmb.Items.Clear();

            for (int i = 0; i < state.GetLength(0); i++)
            {
                cmb.Items.Add(state[i, 1], state[i, 0]);
            }
        }
        string[,] GetState(string country)
        {

            // Code  Added  By Priti on 14122016 to use Convert.ToString instead of ToString()
            //SelectOwner.SelectParameters[0].DefaultValue = country.ToString();
            SelectOwner.SelectParameters[0].DefaultValue = Convert.ToString(country);
            DataView view = (DataView)SelectOwner.Select(DataSourceSelectArguments.Empty);
            string[,] DATA = new string[view.Count, 2];
            for (int i = 0; i < view.Count; i++)
            {
                DATA[i, 0] = view[i][0].ToString();
                DATA[i, 1] = view[i][1].ToString();
            }
            return DATA;


        }
        private void cmbState_OnCallback(object source, CallbackEventArgsBase e)
        {
            FillStateCombo(source as ASPxComboBox, e.Parameter);
        }
        private void editCombo_DataBound(object sender, EventArgs e)
        {
            ListEditItem noneItem = new ListEditItem("None", 0);
            (sender as ASPxComboBox).Items.Insert(0, noneItem);
        }

        private void filterCombo_DataBound(object sender, EventArgs e)
        {
            ListEditItem noneItem = new ListEditItem("None", 0);
            (sender as ASPxComboBox).Items.Insert(0, noneItem);
        }
        public void bindexport(int Filter)
        {
            //Code  Added and Commented By Priti on 21122016 to use Export Header,date
            string filename = "GroupMaster";
            exporter.FileName = filename;
            exporter.MaxColumnWidth = 80;
            exporter.PageHeader.Left = "Group Master";
            exporter.PageFooter.Center = "[Page # of Pages #]";
            exporter.PageFooter.Right = "[Date Printed]";
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
        protected void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Code  Added  By Priti on 14122016 to use Convert.ToString instead of ToString()
            //Code  Added and Commented By Priti on 21122016 to use Export Header,date
            Int32 Filter = int.Parse(Convert.ToString(drdExport.SelectedItem.Value));
            if (Filter != 0)
            {
                if (Session["exportval"] == null)
                {
                    Session["exportval"] = Filter;
                    bindexport(Filter);
                }
                else if (Convert.ToInt32(Session["exportval"]) != Filter)
                {
                    Session["exportval"] = Filter;
                    bindexport(Filter);
                }
            } 
            
        }

        protected void GroupMasterGrid_HtmlRowCreated(object sender, DevExpress.Web.ASPxGridViewTableRowEventArgs e)
        {
            // Code  Added  By Priti on 14122016 to use Convert.ToString instead of ToString()
            if (e.RowType == GridViewRowType.Data)
            {
                int commandColumnIndex = -1;
                for (int i = 0; i < GroupMasterGrid.Columns.Count; i++)
                    if (GroupMasterGrid.Columns[i] is GridViewCommandColumn)
                    {
                        commandColumnIndex = i;
                        break;
                    }
                if (commandColumnIndex == -1)
                    return;
                //____One colum has been hided so index of command column will be leass by 1 
                commandColumnIndex = commandColumnIndex - 4;
                DevExpress.Web.Rendering.GridViewTableCommandCell cell = e.Row.Cells[commandColumnIndex] as DevExpress.Web.Rendering.GridViewTableCommandCell;
                for (int i = 0; i < cell.Controls.Count; i++)
                {
                    DevExpress.Web.Rendering.GridCommandButtonsCell button = cell.Controls[i] as DevExpress.Web.Rendering.GridCommandButtonsCell;
                    if (button == null) return;
                    DevExpress.Web.Internal.InternalHyperLink hyperlink = button.Controls[0] as DevExpress.Web.Internal.InternalHyperLink;

                    if (hyperlink.Text == "Delete")
                    {
                        //if (Session["PageAccess"].ToString() == "DelAdd" || Session["PageAccess"].ToString() == "Delete" || Session["PageAccess"].ToString() == "All")
                        if (Convert.ToString(Session["PageAccess"]) == "DelAdd" || Convert.ToString(Session["PageAccess"]) == "Delete" || Convert.ToString(Session["PageAccess"]) == "All")
                        {
                            hyperlink.Enabled = true;
                            continue;
                        }
                        else
                        {
                            hyperlink.Enabled = false;
                            continue;
                        }
                    }


                }

            }

        }
        protected void GroupMasterGrid_HtmlEditFormCreated(object sender, DevExpress.Web.ASPxGridViewEditFormEventArgs e)
        {

            //TextBox GroupOwner = (TextBox)GroupMasterGrid.FindEditFormTemplateControl("GroupOwner");
            //GroupOwner.Attributes.Add("onkeyup", "CallList(this,'GetGroupOwnerName',event)");

            if (!GroupMasterGrid.IsNewRowEditing)
            {
                ASPxGridViewTemplateReplacement RT = GroupMasterGrid.FindEditFormTemplateControl("UpdateButton") as ASPxGridViewTemplateReplacement;
                //if (Session["PageAccess"].ToString().Trim() == "Add" || Session["PageAccess"].ToString().Trim() == "Modify" || Session["PageAccess"].ToString().Trim() == "All")
                if (Convert.ToString(Session["PageAccess"]).Trim() == "Add" || Convert.ToString(Session["PageAccess"]).Trim() == "Modify" || Convert.ToString(Session["PageAccess"]).Trim() == "All")
                RT.Visible = true;
                else
                    RT.Visible = false;
            }

        }
        protected void GroupMasterGrid_RowValidating(object sender, DevExpress.Web.Data.ASPxDataValidationEventArgs e)
        {


            if (e.NewValues["GroupType"] == null)
            {
                e.RowError = "Please select Group Type.";
                return;
            }
            if (e.NewValues["GroupName"] == "")
            {
                e.RowError = "Group Name Required.";
                return;
            }
            //if (e.NewValues["GroupCode"].ToString() == "")
            if (Convert.ToString(e.NewValues["GroupCode"]) == "")
            {
                e.RowError = "Short Name Required.";
                return;
            }
            #region
            //if (e.NewValues["gpm_emailID"].ToString() == "")
            //{
            //    e.RowError = "Email is Rewuired.";
            //    return;
            //}

            //if (e.NewValues["PrincipalGroup"].ToString() == "")
            //{
            //    e.RowError = "Please select Principal Group.";
            //    return;
            //}

            //if (e.NewValues["GroupOwner1"].ToString() == "")
            //{
            //    e.RowError = "Please select Group Owner.";
            //    return;
            //}
            #endregion
            if (GroupMasterGrid.IsNewRowEditing)
            {
                string eid = "";
                //string grouptype = e.NewValues["GroupType"].ToString();
                string grouptype = Convert.ToString(e.NewValues["GroupType"]);
                //string exchangeId = e.NewValues["GroupCode"].ToString();
                string exchangeId = Convert.ToString(e.NewValues["GroupCode"]);
                string[,] id = oDBEngine.GetFieldValue("tbl_master_groupMaster", "gpm_code", "gpm_code='" + exchangeId + "' and gpm_Type= '" + grouptype + "'", 1);
                if (id[0, 0] != "n")
                {
                    eid = id[0, 0];
                }
                if (eid == "")
                {
                }
                else
                {
                    e.RowError = "This Code Already Exists";
                    return;
                }
            }
            else
            {
                string eid = "";
                //string KeyVal = e.Keys["Id"].ToString();
                string KeyVal = Convert.ToString(e.Keys["Id"]);
                // string exchangeId = e.OldValues["GroupCode"].ToString();
                //string exchangeId = e.NewValues["GroupCode"].ToString();
                string exchangeId = Convert.ToString(e.NewValues["GroupCode"]);
                string[,] id = oDBEngine.GetFieldValue("tbl_master_groupMaster", "gpm_code", "gpm_id='" + KeyVal + "' ", 1);
                if (id[0, 0] != "n")
                {
                    eid = id[0, 0];
                }
                if (exchangeId == eid)
                {

                }
                else
                {
                    //string grpcode = exchangeId.ToString();
                    string grpcode = Convert.ToString(exchangeId);
                    string[,] grp = oDBEngine.GetFieldValue("tbl_master_groupMaster", "gpm_code", "gpm_code='" + grpcode + "' ", 1);
                    if (grp[0, 0] != "n")
                    {
                        eid = grp[0, 0];
                    }
                    else
                    {
                        eid = "";
                    }
                    if (eid == "")
                    {
                    }
                    else
                    {
                        e.RowError = "This Code Already Exists";
                        return;
                    }
                }
                #region
                //if (eid == "")
                //{

                //}
                //else
                //{
                //    //e.RowError = "This Code Already Exists";
                //    //return;
                //}

                //string KeyVal = e.Keys["Id"].ToString();
                //string Category = e.NewValues["Category"].ToString();
                //string[,] Category1 = objEngine.GetFieldValue("tbl_trans_contactBankDetails", "cbd_id", " cbd_cntId='" + Session["KeyVal_InternalID"].ToString() + "' and cbd_accountCategory='" + Category + "'", 1);
                //if (Category1[0, 0] != "n")
                //{
                //    if (KeyVal != Category1[0, 0])
                //    {
                //        e.RowError = "Default Category Already Exists!";
                //        return;
                //    }
                //}
            }

            //string exchangeId = "";
            //string eid = "";
            //try
            //{
            //    exchangeId = e.NewValues["GroupCode"].ToString();
            //}
            //catch
            //{
            //}
            //if (Session["check"] != null)
            //{
            //    Session["check"] = null;
            //    string[,] id = oDBEngine.GetFieldValue("tbl_master_groupMaster", "gpm_code", "gpm_code='" + exchangeId + "' ", 1);
            //    if (id[0, 0] != "n")
            //    {
            //        eid = id[0, 0];
            //    }
            //    if (eid == "")
            //    {
            //    }
            //    else
            //    {
            //        e.RowError = "This Code Already Exists";
            //        return;
            //    }

            //}
            //else
            //{
            //    string depositoryId1 = "";
            //    try
            //    {
            //        depositoryId1 = e.OldValues["GroupCode"].ToString();
            //    }
            //    catch
            //    {

            //    }
            //    if (exchangeId == depositoryId1)
            //    {
            //    }
            //    else
            //    {
            //        string[,] id = oDBEngine.GetFieldValue("tbl_master_groupMaster", "gpm_code", "gpm_code='" + exchangeId + "' ", 1);
            //        if (id[0, 0] != "n")
            //        {
            //            eid = id[0, 0];
            //        }
            //        if (eid == "")
            //        {
            //        }
            //        else
            //        {
            //            e.RowError = "This Code Already Exists";
            //            return;
            //        }
            //    }
            //}
                #endregion
            string GType = Convert.ToString(e.NewValues["GroupType"]);
            string MemType = Convert.ToString(e.NewValues["MemberType"]);
            if (GType != "Virtual DP")
            {
                if (MemType == "CDSL Accounts" || MemType == "NSDL Accounts")
                {
                    e.RowError = "Please Select Virtual DP";
                    return;
                }
            }
            else if (GType == "Virtual DP")
            {
                if (MemType == "CDSL Accounts" || MemType == "NSDL Accounts")
                {

                }
                else
                {
                    e.RowError = "Please Select CDSL Accounts or NSDL Accounts";
                    return;
                }
            }
        }
        protected void GroupMasterGrid_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            Session["check"] = "a";
            ForInsertUpdateChecking = "a";
        }
        protected void GroupOwnerCombo_CustomCallback(object source, CallbackEventArgsBase e)
        {
            string param = e.Parameter;
            ASPxComboBox combo = (ASPxComboBox)GroupMasterGrid.FindEditFormTemplateControl("GroupOwner");
            if (param == "Relationship Partner" || param == "Franchisee" || param == "Relationship Manager")
            {
                SelectGroupOwner.SelectCommand = "SELECT cnt_internalId,isnull(cnt_firstName,'') + ' ' + isnull(cnt_middleName,'') + ' ' + isnull(cnt_lastName,'') + '['+ case when cnt_internalId like 'CL%' then   isnull(cnt_UCC,'') else isnull(cnt_shortname,'') end +']'  AS GroupOwner FROM tbl_master_contact where cnt_internalId like(select prefix_Name from tbl_master_prefix where  ltrim(rtrim(prefix_Type))= '" + param + "')+'%' Order by cnt_firstName ";
            }
            else
            {
                SelectGroupOwner.SelectCommand = "SELECT cnt_internalId, isnull(cnt_firstName,'') + ' ' + isnull(cnt_middleName,'') + ' ' + isnull(cnt_lastName,'') + '['+ case when cnt_internalId like 'CL%' then   isnull(cnt_UCC,'') else isnull(cnt_shortname,'') end +']'  AS GroupOwner FROM tbl_master_contact Order by cnt_firstName ";
            }
            combo.DataSource = SelectGroupOwner;
            combo.DataBind();
            if(Session["gpm_Owner"]!=null)
            {
                DataTable DT = oDBEngine.GetDataTable("tbl_master_contact", "cnt_internalId,isnull(cnt_firstName,'') + ' ' + isnull(cnt_middleName,'') + ' ' + isnull(cnt_lastName,'') + '['+ case when cnt_internalId like 'CL%' then   isnull(cnt_UCC,'') else isnull(cnt_shortname,'') end +']'  AS GroupOwner ", null,"cnt_firstName");
                 List<string> obj = new List<string>();
                 int count = 0;
                 foreach (DataRow dr in DT.Rows)
                 {
                     string gpm_Owner = Convert.ToString(Session["gpm_Owner"]);
                     string cnt_internalId = Convert.ToString(dr["cnt_internalId"]);
                     if (gpm_Owner == cnt_internalId)
                     {
                         break;
                     }
                     count++;
                 }
                 combo.SelectedIndex = count;
            }
            Session["gpm_Owner"] = null;
        }
      
        protected void GroupMasterGrid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            string Evalue = e.Parameters;

            GroupMasterGrid.JSProperties["cpCannotDelete"] = "Cannot delete row with UnitsOnOrder < 20";
            if (Evalue == "s")
                GroupMasterGrid.Settings.ShowFilterRow = true;
            else if (Evalue == "All")
            {
                GroupMasterGrid.FilterExpression = string.Empty;
            }
            else if (Evalue.Split('~')[0].ToString() == "Delete")
            {
                var k = Evalue.Split('~')[1].ToString();
                DataTable dtgroup = oDBEngine.GetDataTable("tbl_trans_group", " * ", "grp_groupMaster='" + Evalue.Split('~')[1].ToString() + "'");
                if (dtgroup.Rows.Count > 0)
                {

                    GroupMasterGrid.JSProperties["cpCannotDelete"] = "Cannot delete row with UnitsOnOrder < 20";
                    ForInsertUpdateChecking = "c";

                }
                else
                {

                    GroupMasterGrid.JSProperties["cpCannotDelete"] = "Cannot delete row with UnitsOnOrder < 20";
                    //  oDBEngine.DeleteValue("tbl_master_groupmaster", "gpm_id ='" + Evalue.Split('~')[1].ToString() + "'");
                    GroupMasterGrid.DataBind();

                }

            }
            else
            {
                GroupMasterGrid.ClearSort();
                GroupMasterGrid.DataBind();
            }



        }
        protected void GroupMasterGrid_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            ASPxComboBox combo = (ASPxComboBox)GroupMasterGrid.FindEditFormTemplateControl("GroupOwner");
            
            if (Convert.ToString(combo.Value) == "")
            {
                e.NewValues["GroupOwner"] = e.OldValues["GroupOwner"];
            }
            else
            {
                e.NewValues["GroupOwner"] = combo.Value;
            }
            // e.NewValues["GroupOwner"] = GroupOwner_hidden.Value;
            if (e.NewValues["PrincipalGroup"] != null)
            {
               // if (e.NewValues["PrincipalGroup"].ToString() == "None")
                if (Convert.ToString(e.NewValues["PrincipalGroup"]) == "None")
                {
                    e.NewValues["PrincipalGroup"] = null;


                }
                else
                {
                    //string PGroup1 = e.NewValues["PrincipalGroup"].ToString();
                    string PGroup1 = Convert.ToString(e.NewValues["PrincipalGroup"]);
                }

            }
            //string PGroup1 = e.NewValues["PrincipalGroup"].ToString();
            //HiddenField grpownr = (HiddenField)GroupMasterGrid.FindEditFormTemplateControl("GroupOwner_hidden");
            //if (grpownr.Value.ToString() == "")
            //{
            //    e.NewValues["GroupOwner"] = e.OldValues["GroupOwner"];
            //}
            //else
            //{
            //    e.NewValues["GroupOwner"] = grpownr.Value;
            //}
            #region
            //string[] GroupOwner = e.NewValues["GroupOwner1"].ToString().Split('~');
            //string GrpOwnr = GroupOwner[1].ToString();
            //e.NewValues["GroupOwner"] = GrpOwnr.ToString();

            //string[] GroupOwner = e.NewValues["GroupOwner1"].ToString().Split('[');
            //string[] uccno = GroupOwner[1].ToString().Split(']');
            //string shortname = uccno[0].ToString();
            //if (shortname != "")
            //{
            //    string[,] DT = objEngine.GetFieldValue(" tbl_master_contact", " cnt_internalId", "cnt_shortName='" + shortname + "'", 1);
            //    if (DT[0, 0] != "n")
            //    {
            //         e.NewValues["GroupOwner"] = DT[0, 0].ToString();
            //       // e.NewValues["GroupOwner"] = GroupOwner_hidden.Value;
            //    }
            //    else
            //    {

            //        return;
            //    }

            //}
            #endregion
        }
        protected void GroupMasterGrid_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            // Code  Added  By Priti on 14122016 to use Convert.ToString instead of ToString()
            //string pGroup = e.NewValues["PrincipalGroup1"].ToString();
            if (e.NewValues["PrincipalGroup"] != null)
            {
                //string PGroup1 = e.NewValues["PrincipalGroup"].ToString();
                string PGroup1 = Convert.ToString(e.NewValues["PrincipalGroup"]);

            }
            else
            {
                e.NewValues["PrincipalGroup"] = null;
            }
            HiddenField grpownr = (HiddenField)GroupMasterGrid.FindEditFormTemplateControl("GroupOwner_hidden");
            e.NewValues["GroupOwner"] = grpownr.Value;
            ASPxComboBox combo = (ASPxComboBox)GroupMasterGrid.FindEditFormTemplateControl("GroupOwner");
            e.NewValues["GroupOwner"] = combo.Value;
            #region
            //string[] GroupOwner = e.NewValues["GroupOwner1"].ToString().Split('~');
            //string GrpOwnr = GroupOwner[1].ToString();
            //e.NewValues["GroupOwner"] = GrpOwnr.ToString();

            //string[] GroupOwner = e.NewValues["GroupOwner1"].ToString().Split('[');
            //string[] uccno = GroupOwner[1].ToString().Split(']');
            //string shortname = uccno[0].ToString();
            //if (shortname != "")
            //{
            //    string[,] DT = objEngine.GetFieldValue(" tbl_master_contact", " cnt_internalId", "cnt_shortName='" + shortname + "'", 1);
            //    if (DT[0, 0] != "n")
            //    {
            //        e.NewValues["GroupOwner"] = DT[0, 0].ToString();
            //    }
            //    else
            //    {

            //        return;
            //    }

            //}
            #endregion
             //String con = ConfigurationSettings.AppSettings["DBConnectionDefault"];
             //using (SqlConnection lcon = new SqlConnection(con))
             //{
             //    lcon.Open();
             //    using (SqlCommand sqlCmdGM = new SqlCommand("InsertGroupMaster", lcon))
             //    {


             //        sqlCmdGM.CommandType = CommandType.StoredProcedure;
             //        //___________For Returning InternalID_________//
             //        //SqlParameter parameter = new SqlParameter("@result", SqlDbType.VarChar, 50);
             //        //parameter.Direction = ParameterDirection.Output;
             //        ///_______________________________________________//
             //        sqlCmdGM.Parameters.AddWithValue("@GroupName",e.NewValues["GroupName"] );
             //        sqlCmdGM.Parameters.AddWithValue("@GroupCode", e.NewValues["GroupCode"]);
             //        sqlCmdGM.Parameters.AddWithValue("@GroupType", e.NewValues["GroupType"]);
             //        sqlCmdGM.Parameters.AddWithValue("@GroupOwner", e.NewValues["GroupOwner"]);
             //        sqlCmdGM.Parameters.AddWithValue("@MemberType", e.NewValues["MemberType"]);
             //        sqlCmdGM.Parameters.AddWithValue("@PrincipalGroup", e.NewValues["PrincipalGroup"]);
             //        sqlCmdGM.Parameters.AddWithValue("@CreateUser", Convert.ToString(HttpContext.Current.Session["userid"]));
             //        sqlCmdGM.Parameters.AddWithValue("@gpm_emailID", e.NewValues["gpm_emailID"]);
             //        sqlCmdGM.Parameters.AddWithValue("@gpm_ccemailID", e.NewValues["gpm_ccemailID"]);
                    
             //        //sqlCmdGM.Parameters.Add(parameter);                     
             //        sqlCmdGM.ExecuteNonQuery();
             //       // InternalID = parameter.Value.ToString();

             //        //..................code added by priti on 07122016 to check the short name unique..    

             //        //...........end............

             //    }
             //}
           // Page.ClientScript.RegisterStartupScript(GetType(),"Success", "<script>jAlert('Save Successfully.')</script>");
            //GroupMasterGrid.JSProperties["cpInsertmsg"] = "Save Succesfully ";

        }
        protected void GroupMasterGrid_CustomJSProperties(object sender, ASPxGridViewClientJSPropertiesEventArgs e)
        {
            DBEngine objDb = new DBEngine();
            e.Properties["cpvarHide"] = ForInsertUpdateChecking;

            DataTable dtc = objDb.GetDataTable("select COUNT(gpm_id) Number from tbl_master_groupMaster");
            aftersavecnt.Value = Convert.ToString(dtc.Rows[0]["Number"]);
            if((Convert.ToInt32(aftersavecnt.Value)-Convert.ToInt32(hdnbeforesave.Value))==1)
            {
                GroupMasterGrid.JSProperties["cpInsertmsg"] = "Save Succesfully ";
            }
            else
             {
                GroupMasterGrid.JSProperties["cpInsertmsg"] = "";
            }
        }

        protected void GroupMasterGrid_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            string EditingKeyValueGpmId = string.Empty;

            EditingKeyValueGpmId = e.EditingKeyValue.ToString();

            PrincipleGroup.SelectCommand = "SELECT gpm_id = CAST((CAST(gpm_id as INT)) as nvarchar(max)), gpm_Description as PrincipalGroup FROM tbl_master_groupMaster where gpm_id!='" + EditingKeyValueGpmId + "' and gpm_id not in (select gpm_id from tbl_master_groupMaster where  gpm_PrincipalGroup ='" + EditingKeyValueGpmId + "')";

            ForInsertUpdateChecking = "b";

            Session["id"] = EditingKeyValueGpmId;

            DataTable DT = oDBEngine.GetDataTable("tbl_master_groupMaster", "gpm_id,gpm_Owner", "gpm_id='"+ EditingKeyValueGpmId+"'");

            List<string> obj = new List<string>();
            foreach (DataRow dr in DT.Rows)
            {
                if (Convert.ToString(dr["gpm_Owner"])!=String.Empty)
                {
                    Session["gpm_Owner"] = Convert.ToString(dr["gpm_Owner"]);
                }
                
            }
            //ASPxComboBox combo = (ASPxComboBox)GroupMasterGrid.FindEditFormTemplateControl("GroupOwner");
            //SelectGroupOwner.SelectCommand = "SELECT cnt_internalId, isnull(cnt_firstName,'') + ' ' + isnull(cnt_middleName,'') + ' ' + isnull(cnt_lastName,'') + '['+ case when cnt_internalId like 'CL%' then   isnull(cnt_UCC,'') else isnull(cnt_shortname,'') end +']'  AS GroupOwner FROM tbl_master_contact Order by cnt_firstName ";
            //combo.DataSource = SelectGroupOwner;
            //combo.DataBind();
        }
        protected void GroupMasterGrid_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            // Code  Added  By Priti on 14122016 to use Convert.ToString instead of ToString()
            //string KeyVal = e.Keys["Id"].ToString();
            string KeyVal = Convert.ToString(e.Keys["Id"]);
            string[,] acccode = oDBEngine.GetFieldValue("tbl_master_groupMaster,tbl_trans_group",
              "grp_contactId", "gpm_id=grp_groupMaster and gpm_id='" + KeyVal + "'", 1);

            if (acccode[0, 0] == "n")
            {
                GroupMasterGrid.JSProperties["cpDelmsg"] = "Succesfully Deleted";
            }
            else
            {
                //AccountGroup.JSProperties["cpDelmsg"] = "Cannot Delete. This AccountGroup Code Is In Use";
                GroupMasterGrid.JSProperties["cpDelmsg"] = "Used in other modules. Cannot Delete.";
                e.Cancel = true;
            }
            //..........Code  Added and Commented By Priti on 30-11-2016

            DataTable dt = oDBEngine.GetDataTable("select gpm_id,gpm_PrincipalGroup from tbl_master_groupMaster where gpm_PrincipalGroup='" + KeyVal + "'");
            if (dt.Rows.Count > 0)
            {
                GroupMasterGrid.JSProperties["cpDelmsg"] = "Used in other modules. Cannot Delete.";              
                e.Cancel = true;

            }
            else
            {
                
                GroupMasterGrid.JSProperties["cpDelmsg"] = "Succesfully Deleted";

            }

            DataTable dt1 = oDBEngine.GetDataTable("select * from tbl_trans_group where grp_groupMaster='" + KeyVal + "'");
            if (dt1.Rows.Count > 0)
            {
                GroupMasterGrid.JSProperties["cpDelmsg"] = "Used in other modules. Cannot Delete.";
                e.Cancel = true;

            }
            else
            {

                GroupMasterGrid.JSProperties["cpDelmsg"] = "Succesfully Deleted";

            }
            //.............end.................

        }

        //Purpose: Add Edit and delete rights to Group Master
        //Name: Debjyoti Dhar.
        protected void GroupMasterGrid_CommandButtonInitialize(object sender, ASPxGridViewCommandButtonEventArgs e)
        {
            if (!rights.CanDelete)
            {
                if (e.ButtonType == ColumnCommandButtonType.Delete)
                {
                    e.Visible = false;
                }
            }


            if (!rights.CanEdit)
            {
                if (e.ButtonType == ColumnCommandButtonType.Edit)
                {
                    e.Visible = false;
                }
            }

        }

        protected void GroupMasterGrid_CustomButtonInitialize(object sender, ASPxGridViewCustomButtonEventArgs e)
        {
            if (!rights.CanMembers)
            {
                if (e.ButtonID == "myButton")
                {
                    e.Visible = DevExpress.Utils.DefaultBoolean.False;
                }
            }
        }

       
        //Bellow web method added to check whether the short name unique or not

        [WebMethod]
        public static bool CheckUniqueCode(string CategoriesShortCode, string Code)
        {
            bool flag = false;
            try
            {
                //BusinessLogicLayer.MShortNameCheckingBL obj = new BusinessLogicLayer.MShortNameCheckingBL();
                //flag = obj.CheckUnique(CategoriesShortCode, Code, "ReminderCategory");

            }
            catch (Exception ex)
            {

            }
            finally
            {
            }
            return flag;
        }

        /*Code  Added  By Priti on 21122016 to Check unique short name*/
        [WebMethod]
        public static bool CheckUniqueName(string ShortName)
        {
            string ShortCode = "0";
            if (HttpContext.Current.Session["id"]!= null)
            {
                ShortCode = Convert.ToString(HttpContext.Current.Session["id"]).Trim();
            }
                
            MShortNameCheckingBL objMShortNameCheckingBL = new MShortNameCheckingBL();
            DataTable dt = new DataTable();
            Boolean status = false;
            BusinessLogicLayer.GenericMethod oGeneric = new BusinessLogicLayer.GenericMethod();
            if (ShortCode != "" && Convert.ToString(ShortName).Trim() != "")
            {
                status = objMShortNameCheckingBL.CheckUnique(ShortName, ShortCode, "Add_Edit_GroupMaster");
            }
            return status;
        }


        //...............code end........

    }
}