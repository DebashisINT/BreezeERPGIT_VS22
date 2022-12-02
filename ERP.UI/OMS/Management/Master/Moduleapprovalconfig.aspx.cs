using DevExpress.Web;
using EntityLayer.CommonELS;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

using DevExpress.Web.Data;

namespace ERP.OMS.Management.Master
{
    public partial class Moduleapprovalconfig : ERP.OMS.ViewState_class.VSPage
    {
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        public static EntityLayer.CommonELS.UserRightsForPage rights;

        #region page event
        protected void Page_Init(object sender, EventArgs e)
        {
            ((GridViewDataComboBoxColumn)approvalGrid.Columns["level1userids"]).PropertiesComboBox.DataSource = Cmblevel1();
            ((GridViewDataComboBoxColumn)approvalGrid.Columns["level2userids"]).PropertiesComboBox.DataSource = Cmblevel1();
            ((GridViewDataComboBoxColumn)approvalGrid.Columns["level3userids"]).PropertiesComboBox.DataSource = Cmblevel1();


            if (!IsPostBack)
            {

                approvalGrid.DataBind();


            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (HttpContext.Current.Session["userid"] != null)
            {
                if (!Page.IsPostBack)
                {
                    //To check User Rights 
                    Session["exportval"] = null;
                    rights = new UserRightsForPage();
                    rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/master/Moduleapprovalconfig.aspx");
                   

                    cmbbranch.DataSource = branch1();
                    cmbbranch.DataBind();
                    cmbbranch.Value = Convert.ToString(Session["userbranchID"]);

                }
            }
        }

        #endregion

        #region Grid Event

        protected void approvalGrid_BatchUpdate(object sender, DevExpress.Web.Data.ASPxDataBatchUpdateEventArgs e)
        {
            try
            {
                string issuccess = string.Empty;
                foreach (var args in e.UpdateValues)
                {
                    string moduleids = Convert.ToString(args.Keys["moduleids"]);
                    string active = Convert.ToString(args.NewValues["active"]);
                    string level1userids = Convert.ToString(args.NewValues["level1userids"]);
                    string isactiveanddependent = Convert.ToString(args.NewValues["active1"]);
                    string acid = Convert.ToString(args.NewValues["acid"]);
                    string level2userids = Convert.ToString(args.NewValues["level2userids"]);
                    string level3userids = Convert.ToString(args.NewValues["level3userids"]);
                    if (!string.IsNullOrEmpty(isactiveanddependent.Split(',')[0]) && active=="False")
                    {
                        if (moduleids == "1" && isactiveanddependent.Split(',')[0] == "QO" && active == "False")
                        {
                            approvalGrid.JSProperties["cpupdatemssg"] = Convert.ToString("Sorry,you cannot deactivate the Proforma/Quotation. it is being used.");
                            Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script>jAlert('Sorry,you cannot deactivate the Proforma/Quotation. it is being used.')</script>");
                            return;
                        }
                    }

                    DataSet dsEmail = new DataSet();
                    //String conn = ConfigurationSettings.AppSettings["DBConnectionDefault"];MULTI
                    String conn = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                    SqlConnection con = new SqlConnection(conn);
                    SqlCommand cmd3 = new SqlCommand("prc_Approvalconfig", con);
                    cmd3.CommandType = CommandType.StoredProcedure;

                    if (hdnselectedbranch.Value == "0")
                    {
                        cmd3.Parameters.AddWithValue("@branchid", Convert.ToInt32(Session["userbranchID"]));

                    }
                    else
                    {
                        cmd3.Parameters.AddWithValue("@branchid", Convert.ToInt32(hdnselectedbranch.Value));

                    }


                    if (!string.IsNullOrEmpty(acid))
                    {
                        cmd3.Parameters.AddWithValue("@approvalconfigID", Convert.ToInt32(acid));
                        cmd3.Parameters.AddWithValue("@actiontype", 1);
                        cmd3.Parameters.AddWithValue("@lastmodifyuser", Convert.ToInt32(HttpContext.Current.Session["userid"]));
                    }
                    else
                    {

                        cmd3.Parameters.AddWithValue("@actiontype", 2);
                        cmd3.Parameters.AddWithValue("@createdby", Convert.ToInt32(HttpContext.Current.Session["userid"]));
                    }
                    if (!string.IsNullOrEmpty(moduleids))
                    {
                        cmd3.Parameters.AddWithValue("@moduleid", Convert.ToInt32(moduleids));
                    }
                    if (!string.IsNullOrEmpty(level1userids))
                    {
                        cmd3.Parameters.AddWithValue("@level1", Convert.ToInt32(level1userids));
                        if (!string.IsNullOrEmpty(level2userids))
                        {

                            cmd3.Parameters.AddWithValue("@level2", Convert.ToInt32(level2userids));
                        }
                        else
                        {
                            cmd3.Parameters.AddWithValue("@level2", Convert.ToInt32(0));
                        }
                        if (!string.IsNullOrEmpty(level3userids))
                        {
                            cmd3.Parameters.AddWithValue("@level3", Convert.ToInt32(level3userids));
                        }
                        else
                        {
                            cmd3.Parameters.AddWithValue("@level3", Convert.ToInt32(0));
                        }
                    }
                    else
                    {
                        level1userids = "0";
                    }

                    if (active == "True")
                    {
                        cmd3.Parameters.AddWithValue("@Active", 1);
                    }
                    else
                    {
                        cmd3.Parameters.AddWithValue("@Active", 0);
                    }
                    if (active == "True" && level1userids == "0")
                    {
                        approvalGrid.JSProperties["cpupdatemssg"] = Convert.ToString("Sorry, Actived Approver's should not be blank.");
                        Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script>jAlert('Sorry Actived Approver's should not be blank.')</script>");
                        return;

                    }
                    else if (!string.IsNullOrEmpty(moduleids))
                    {
                        cmd3.CommandTimeout = 0;
                        SqlDataAdapter Adap = new SqlDataAdapter();
                        Adap.SelectCommand = cmd3;
                        Adap.Fill(dsEmail);
                        dsEmail.Clear();
                        cmd3.Dispose();
                        con.Dispose();
                        GC.Collect();
                        issuccess = "true";
                    }

                }
                if (issuccess == "true")
                {
                    approvalGrid.JSProperties["cpupdatemssg"] = Convert.ToString("Saved Successfully.");
                    Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script>jAlert('Saved Successfully.')</script>");
                }
                
            }
            catch (Exception ex)
            {

                Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script>jAlert('Error exception.Please try again.')</script>");
            }


        }
        protected void Grid_RowInserting(object sender, ASPxDataInsertingEventArgs e)
        {
            e.Cancel = true;
        }
        protected void Grid_RowUpdating(object sender, ASPxDataUpdatingEventArgs e)
        {
            e.Cancel = true;
        }
        protected void Grid_RowDeleting(object sender, ASPxDataDeletingEventArgs e)
        {
            e.Cancel = true;
        }
        protected void approvalGrid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            string strSplitCommand = e.Parameters.Split('~')[0];
            if (strSplitCommand == "branchwise")
            {
                ((GridViewDataComboBoxColumn)approvalGrid.Columns["level1userids"]).PropertiesComboBox.DataSource = Cmblevel1();
                approvalGrid.DataSource = GetGriddata();
                approvalGrid.DataBind();
            }
            else
            {
                approvalGrid.DataSource = GetGriddata();
                approvalGrid.DataBind();
            }


        }
        protected void approvalGrid_DataBinding(object sender, EventArgs e)
        {
            approvalGrid.DataSource = GetGriddata();
        }
        protected void approvalGrid_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
        {
           

            if (e.Column.FieldName == "level2userids")
            {
                if (e.KeyValue != null)
                {
                    object val = approvalGrid.GetRowValuesByKeyValue(e.KeyValue, "level1userids");
                    if (val == DBNull.Value) return;
                    string country = Convert.ToString(val);
                    ASPxComboBox combo = e.Editor as ASPxComboBox;
                    Filllevel2Combo(combo, country);
                    combo.Callback += new CallbackEventHandlerBase(Cmblevel2_OnCallback);
                }
                else
                {
                    ASPxComboBox combo = e.Editor as ASPxComboBox;
                    if (!approvalGrid.IsNewRowEditing)
                    {
                        object val = approvalGrid.GetRowValues(0, "level1userids");
                        if (val == DBNull.Value) return;
                        if (val != null & val != "")
                        {
                            string country = Convert.ToString(val);
                          
                            Filllevel2Combo(combo, country);
                          
                        }
                        else
                        {

                            string country = "1";
                          
                            Filllevel2Combo(combo, country);
                           
                        }
                    }
                    combo.Callback += new CallbackEventHandlerBase(Cmblevel2_OnCallback);


                }
            }
            if (e.Column.FieldName == "level3userids")
            {
                if (e.KeyValue != null)
                {
                    object val = approvalGrid.GetRowValuesByKeyValue(e.KeyValue, "level1userids");
                    object val2 = approvalGrid.GetRowValuesByKeyValue(e.KeyValue, "level2userids");
                    if (val == DBNull.Value && val2 == DBNull.Value) return;
                    string country = Convert.ToString(val + "," + val2);
                    ASPxComboBox combo = e.Editor as ASPxComboBox;
                    Filllevel2Combo(combo, country);
                    combo.Callback += new CallbackEventHandlerBase(Cmblevel2_OnCallback);
                }
                else
                {
                    ASPxComboBox combo = e.Editor as ASPxComboBox;
                    if (!approvalGrid.IsNewRowEditing)
                    {
                        object val = approvalGrid.GetRowValues(0, "level1userids");
                        object val2 = approvalGrid.GetRowValues(0, "level2userids");
                        if (val == DBNull.Value && val2 == DBNull.Value) return;
                        if ((val != null && val != "") && (val2 != null && val2 != ""))
                        {
                            string country = Convert.ToString(val + "," + val2);
                            
                            Filllevel2Combo(combo, country);
                            
                        }
                        else
                        {

                            string country = "1";
                          
                            Filllevel2Combo(combo, country);
                           
                        }
                    }
                    combo.Callback += new CallbackEventHandlerBase(Cmblevel3_OnCallback);


                }
            }

        }

        protected void Cmblevel1_Callback(object source, CallbackEventArgsBase e)
        {

            string WhichCall = e.Parameter.Split('|')[0];
            string level1SelectedValue = e.Parameter.Split('|')[1];
            string level1SelectedItem = e.Parameter.Split('|')[2];

            
            ASPxComboBox Cmblevel2 = (ASPxComboBox)approvalGrid.FindEditFormTemplateControl("Cmblevel2");

           
        }
        protected void Cmblevel2_OnCallback(object source, CallbackEventArgsBase e)
        {

            if (!string.IsNullOrEmpty(e.Parameter))
            {
                Filllevel2Combo(source as ASPxComboBox, e.Parameter);

            }



        }
        protected void Cmblevel3_OnCallback(object source, CallbackEventArgsBase e)
        {
            

            if (!string.IsNullOrEmpty(e.Parameter))
            {
                Filllevel2Combo(source as ASPxComboBox, e.Parameter);

            }

        }


        protected void level2userids_Init(object sender, EventArgs e)
        {
            // ASPxComboBox cityCombo = sender as ASPxComboBox;
            // GridViewEditItemTemplateContainer container = cityCombo.NamingContainer as GridViewEditItemTemplateContainer;
            // string countryID = Convert.ToString(container.Grid.GetRowValues(container.Grid.VisibleStartIndex, "level1userids"));
            // approvalGrid.JSProperties["cplevel1"] = countryID;
            // string InputSubValue = countryID;

            //cityCombo.DataSource= Cmblevel2(InputSubValue);
        }

        protected void level3userids_Init(object sender, EventArgs e)
        {
            // ASPxComboBox cityCombo = sender as ASPxComboBox;
            // GridViewEditItemTemplateContainer container = cityCombo.NamingContainer as GridViewEditItemTemplateContainer;
            // string countryID = Convert.ToString(container.Grid.GetRowValues(container.Grid.VisibleStartIndex, "level1userids"));
            // string countryID2 = Convert.ToString(container.Grid.GetRowValues(container.Grid.VisibleStartIndex, "level2userids"));
            // approvalGrid.JSProperties["cplevel1"] = countryID;
            // approvalGrid.JSProperties["cplevel2"] = countryID2;
            // string InputSubValue = countryID + "," + countryID2;
            //cityCombo.DataSource= Cmblevel2(InputSubValue);

        }

        public bool SetVisibility(object container)
        {
            bool vs = false;
            GridViewDataItemTemplateContainer c = container as GridViewDataItemTemplateContainer;
            if (!string.IsNullOrEmpty(Convert.ToString(c.KeyValue)))
            {
                int index = c.VisibleIndex;
                string isexist = ISdependencyExist();
                if (!string.IsNullOrEmpty(isexist))
                {
                    if (Convert.ToString(c.KeyValue) == "1" && !string.IsNullOrEmpty(isexist.Split(',')[0]))
                    {
                        vs = true;
                    }
                }

            }


            return vs;


        }

        public bool SetVisibilityedit(string container)
        {
            bool vs = false;

            if (!string.IsNullOrEmpty(Convert.ToString(container)))
            {

                string isexist = ISdependencyExist();
                if (!string.IsNullOrEmpty(isexist))
                {
                    if (Convert.ToString(container) == "1" && !string.IsNullOrEmpty(isexist.Split(',')[0]))
                    {
                        vs = true;
                    }
                }

            }


            return vs;


        }

        #endregion

        #region private event

        private IEnumerable GetGriddata()
        {

            List<Approvallist> approvallist = new List<Approvallist>();


            DataTable dt = new DataTable();
            DataTable dt2 = new DataTable();
            if (hdnselectedbranch.Value == "0")
            {
                dt = oDBEngine.GetDataTable("exec prc_Approvalconfig @actiontype=10,@branchid=" + Convert.ToString(Session["userbranchID"]) + " ");//it is use for action like 0/1/2/3/4 ->select/UPDATE/insert/delete/selectbyid
            }
            else
            {
                dt = oDBEngine.GetDataTable("exec prc_Approvalconfig @actiontype=10,@branchid=" + hdnselectedbranch.Value + " ");
            }


            if (dt.Rows.Count > 0)
            {
                dt.DefaultView.Sort = "OrderNo";
                dt2 = dt.DefaultView.ToTable();
            }
            for (int i = 0; i < dt2.Rows.Count; i++)
            {
                Approvallist Vouchers = new Approvallist();
                Vouchers.moduleids = Convert.ToString(dt2.Rows[i]["moduleids"]);
                Vouchers.acid = Convert.ToString(dt2.Rows[i]["acid"]);

                Vouchers.active = Convert.ToBoolean(dt2.Rows[i]["active"]);
                Vouchers.active1 = Convert.ToString(dt2.Rows[i]["active1"]);
                Vouchers.modulenames = Convert.ToString(dt2.Rows[i]["modulenames"]);

                if (Convert.ToString(dt2.Rows[i]["level1userids"]) != "0") { Vouchers.level1userids = Convert.ToString(dt2.Rows[i]["level1userids"]); }
                else
                {
                    Vouchers.level1userids = string.Empty;
                }
                if (Convert.ToString(dt2.Rows[i]["level2userids"]) != "0") { Vouchers.level2userids = Convert.ToString(dt2.Rows[i]["level2userids"]); } else { Vouchers.level2userids = string.Empty; }
                if (Convert.ToString(dt2.Rows[i]["level3userids"]) != "0") { Vouchers.level3userids = Convert.ToString(dt2.Rows[i]["level3userids"]); } else { Vouchers.level3userids = string.Empty; }


                approvallist.Add(Vouchers);
            }


            return approvallist;

        }

        public IEnumerable Cmblevel1()
        {

          
            List<Level> LevelList = new List<Level>();
            DataTable DT;
            if (cmbbranch.Value==null)
            {
                DT = new DataTable();
                //DT = oDBEngine.GetDataTable("select user_id ,user_name from tbl_master_user where user_branchId='" + Convert.ToString(Session["userbranchID"]) + "'  order by user_name asc");
               //Changed by Jitendra on 6-6-2017               
               // DT = oDBEngine.GetDataTable("select u.user_id,u.user_name from tbl_master_user u inner join  tbl_trans_employeeCTC e on e.emp_cntId=u.user_contactId where   e.emp_Organization=(select cmp_id from tbl_master_company where cmp_internalid='" + Convert.ToString(HttpContext.Current.Session["LastCompany"]) + "') order by u.user_name asc");
                // Jitendra-06-06-2017- Removed the Company filteration as suggested by sanjay and Pijush

                #region Code Commented and Added by Sam on 14062017 to Show Login Name concatinating Login ID due to Confusion for Multiple User with same Name
                DT = oDBEngine.GetDataTable("select u.user_id,u.user_name+'('+user_loginId+')' user_name from tbl_master_user u inner join  tbl_trans_employeeCTC e on e.emp_cntId=u.user_contactId  order by u.user_name asc");
                //DT = oDBEngine.GetDataTable("select u.user_id,u.user_name from tbl_master_user u inner join  tbl_trans_employeeCTC e on e.emp_cntId=u.user_contactId  order by u.user_name asc");
                #endregion Code Commented and Added by Sam on 14062017 to Show Login Name concatinating Login ID due to Confusion for Multiple User with same Name
            }
            else {
                DT = new DataTable();
                //DT = oDBEngine.GetDataTable("select u.user_id,u.user_name from tbl_master_user u inner join  tbl_trans_employeeCTC e on e.emp_cntId=u.user_contactId where   e.emp_Organization=(select cmp_id from tbl_master_company where cmp_internalid='" + Convert.ToString(HttpContext.Current.Session["LastCompany"]) + "') order by u.user_name asc");
                // Jitendra-06-06-2017- Removed the Company filteration as suggested by sanjay and Pijush
                #region Code Commented and Added by Sam on 14062017 to Show Login Name concatinating Login ID due to Confusion for Multiple User with same Name
                DT = oDBEngine.GetDataTable("select u.user_id,u.user_name+'('+user_loginId+')' user_name  from tbl_master_user u inner join  tbl_trans_employeeCTC e on e.emp_cntId=u.user_contactId  order by u.user_name asc");
                //DT = oDBEngine.GetDataTable("select u.user_id,u.user_name from tbl_master_user u inner join  tbl_trans_employeeCTC e on e.emp_cntId=u.user_contactId  order by u.user_name asc");
                #endregion Code Commented and Added by Sam on 14062017 to Show Login Name concatinating Login ID due to Confusion for Multiple User with same Name
            }
            for (int i = 0; i < DT.Rows.Count; i++)
            {
                Level Levels = new Level();
                Levels.LevelID = Convert.ToString(DT.Rows[i]["user_id"]);
                Levels.LevelName = Convert.ToString(DT.Rows[i]["user_name"]);
                LevelList.Add(Levels);
            }

            return LevelList;
        }
        public IEnumerable Cmblevel2(string InputSubValue)
        {
            List<Level> LevelList = new List<Level>();
            if (string.IsNullOrEmpty(InputSubValue))
            {
                InputSubValue = "0";
            }
            else if (InputSubValue.Contains(',') && InputSubValue.Length == 1)
            {
                InputSubValue = "0";
            }

            DataTable DT;
            if (cmbbranch.Value == null)
            {
                DT = new DataTable();
                //DT = oDBEngine.GetDataTable("select user_id ,user_name from tbl_master_user where user_id not in (" + InputSubValue + ") order by user_name asc");
                //DT = oDBEngine.GetDataTable("select u.user_id,u.user_name from tbl_master_user u inner join  tbl_trans_employeeCTC e on e.emp_cntId=u.user_contactId where    e.emp_Organization=(select cmp_id from tbl_master_company where   cmp_internalid='" + Convert.ToString(HttpContext.Current.Session["LastCompany"]) + "') u.user_id not in (" + InputSubValue + ") order by u.user_name asc");
                #region Code Commented and Added by Sam on 14062017 to Show Login Name concatinating Login ID due to Confusion for Multiple User with same Name
                DT = oDBEngine.GetDataTable("select u.user_id,u.user_name+'('+user_loginId+')' user_name  from tbl_master_user u inner join  tbl_trans_employeeCTC e on e.emp_cntId=u.user_contactId where  u.user_id not in (" + InputSubValue + ") order by u.user_name asc");
               //DT = oDBEngine.GetDataTable("select u.user_id,u.user_name from tbl_master_user u inner join  tbl_trans_employeeCTC e on e.emp_cntId=u.user_contactId where  u.user_id not in (" + InputSubValue + ") order by u.user_name asc");
                 #endregion Code Commented and Added by Sam on 14062017 to Show Login Name concatinating Login ID due to Confusion for Multiple User with same Name
            }
            else
            {
                DT = new DataTable();
                //DT = oDBEngine.GetDataTable("select u.user_id,u.user_name from tbl_master_user u inner join  tbl_trans_employeeCTC e on e.emp_cntId=u.user_contactId where   e.emp_Organization=(select cmp_id from tbl_master_company where   cmp_internalid='" + Convert.ToString(HttpContext.Current.Session["LastCompany"]) + "') u.user_id not in (" + InputSubValue + ") order by u.user_name asc");
                #region Code Commented and Added by Sam on 14062017 to Show Login Name concatinating Login ID due to Confusion for Multiple User with same Name
                //DT = oDBEngine.GetDataTable("select u.user_id,u.user_name from tbl_master_user u inner join  tbl_trans_employeeCTC e on e.emp_cntId=u.user_contactId where    u.user_id not in (" + InputSubValue + ") order by u.user_name asc");
                DT = oDBEngine.GetDataTable("select u.user_id,u.user_name+'('+user_loginId+')' user_name from tbl_master_user u inner join  tbl_trans_employeeCTC e on e.emp_cntId=u.user_contactId where    u.user_id not in (" + InputSubValue + ") order by u.user_name asc");
                #endregion Code Commented and Added by Sam on 14062017 to Show Login Name concatinating Login ID due to Confusion for Multiple User with same Name
            }
          

            for (int i = 0; i < DT.Rows.Count; i++)
            {
                Level Levels = new Level();
                Levels.LevelID = Convert.ToString(DT.Rows[i]["user_id"]);
                Levels.LevelName = Convert.ToString(DT.Rows[i]["user_name"]);
                LevelList.Add(Levels);
            }

            return LevelList;
        }

        protected void Filllevel2Combo(ASPxComboBox cmb, string state)
        {
            if (state == "1")
            {


            }
            else
            {
                if (string.IsNullOrEmpty(state))
                {
                    state = "0";
                }
                else if (state.Contains(',') && state.Length == 1)
                {
                    state = "0";
                }


                DataTable DT;
                if (cmbbranch.Value == null)
                {
                    DT = new DataTable();
                    // DT = oDBEngine.GetDataTable("select user_id ,user_name from tbl_master_user where user_id not in (" + state + ") order by user_name asc");
                    #region Code Commented and Added by Sam on 14062017 to Show Login Name concatinating Login ID due to Confusion for Multiple User with same Name
                    DT = oDBEngine.GetDataTable("select u.user_id,u.user_name+'('+user_loginId+')' user_name  from tbl_master_user u inner join  tbl_trans_employeeCTC e on e.emp_cntId=u.user_contactId where u.user_branchId=" + Convert.ToString(Session["userbranchID"]) + "   and e.emp_Organization=(select cmp_id from tbl_master_company where   cmp_internalid='" + Convert.ToString(HttpContext.Current.Session["LastCompany"]) + "') and u.user_id not in (" + state + ") order by u.user_name asc");
                    //DT = oDBEngine.GetDataTable("select u.user_id,u.user_name from tbl_master_user u inner join  tbl_trans_employeeCTC e on e.emp_cntId=u.user_contactId where u.user_branchId=" + Convert.ToString(Session["userbranchID"]) + "   and e.emp_Organization=(select cmp_id from tbl_master_company where   cmp_internalid='" + Convert.ToString(HttpContext.Current.Session["LastCompany"]) + "') and u.user_id not in (" + state + ") order by u.user_name asc");
                    #endregion Code Commented and Added by Sam on 14062017 to Show Login Name concatinating Login ID due to Confusion for Multiple User with same Name
                }
                else
                {
                    DT = new DataTable();
                    #region Code Commented and Added by Sam on 14062017 to Show Login Name concatinating Login ID due to Confusion for Multiple User with same Name
                    //DT = oDBEngine.GetDataTable("select u.user_id,u.user_name from tbl_master_user u inner join  tbl_trans_employeeCTC e on e.emp_cntId=u.user_contactId where u.user_branchId=" + cmbbranch.Value + "   and e.emp_Organization=(select cmp_id from tbl_master_company where   cmp_internalid='" + Convert.ToString(HttpContext.Current.Session["LastCompany"]) + "') and u.user_id not in (" + state + ") order by u.user_name asc");
                    DT = oDBEngine.GetDataTable("select u.user_id,u.user_name+'('+user_loginId+')' user_name  from tbl_master_user u inner join  tbl_trans_employeeCTC e on e.emp_cntId=u.user_contactId where u.user_branchId=" + cmbbranch.Value + "   and e.emp_Organization=(select cmp_id from tbl_master_company where   cmp_internalid='" + Convert.ToString(HttpContext.Current.Session["LastCompany"]) + "') and u.user_id not in (" + state + ") order by u.user_name asc");
                    #endregion Code Commented and Added by Sam on 14062017 to Show Login Name concatinating Login ID due to Confusion for Multiple User with same Name
                }

                cmb.Items.Clear();

                for (int i = 0; i < DT.Rows.Count; i++)
                {
                    cmb.Items.Add(Convert.ToString(DT.Rows[i]["user_name"]), Convert.ToString(DT.Rows[i]["user_id"]));
                }
            }
          
        }
        private string ISdependencyExist()
        {
            string Exist = string.Empty;


            DataTable DTs = oDBEngine.GetDataTable("select dbo.checkApprovalconfigDependency() as dependency");
            if (DTs.Rows.Count > 0)
            {
                if (!string.IsNullOrEmpty(Convert.ToString(DTs.Rows[0]["dependency"])))
                {
                    Exist = Convert.ToString(DTs.Rows[0]["dependency"]);
                }
            }

            return Exist;
             
        }


        private IEnumerable branch1()
        {
            List<branches> LevelList = new List<branches>();

            DataTable DT = oDBEngine.GetDataTable("select branch_id,branch_description from tbl_master_branch where branch_id in(" + Convert.ToString(Session["userbranchHierarchy"]) + ")  order by branch_description asc");

            for (int i = 0; i < DT.Rows.Count; i++)
            {
                branches Levels = new branches();

                Levels.branchID = Convert.ToString(DT.Rows[i]["branch_id"]);
                Levels.branchName = Convert.ToString(DT.Rows[i]["branch_description"]);



                LevelList.Add(Levels);
            }


            return LevelList;
        }
        #endregion

        #region class
        public class Approvallist
        {
            public string moduleids { get; set; }
            public string acid { get; set; }
            public bool active { get; set; }

            public string active1 { get; set; }
            public string modulenames { get; set; }
            public string level1userids { get; set; }
            public string level2userids { get; set; }
            public string level3userids { get; set; }
        }

        public class Level
        {
            public string LevelID { get; set; }
            public string LevelName { get; set; }
        }


        #endregion

        #region Export event

        public void bindexport(int Filter)
        {

            approvalGrid.Columns[1].Visible = false;
            approvalGrid.Columns[2].Visible = false;
            string filename = "Approval Configuration";
            exporter.FileName = filename;

            exporter.PageHeader.Left = "Approval Configuration";
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
        #endregion

    }
}