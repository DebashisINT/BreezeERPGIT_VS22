
using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ERP.OMS.Management.Master
{
    public partial class LayoutDetailsAddPl : System.Web.UI.Page
    {
        internal static System.Text.RegularExpressions.Regex DotsPathRegex = new System.Text.RegularExpressions.Regex(@"^[\.]+$");

        const string FileImageUrl = "~/TreeView/Images/FileSystem/file.png";
        const string DirImageUrl = "~/TreeView/Images/FileSystem/directory.png";
        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]); MULTI
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["key"] != "")
                {
                    int LayoutId = Convert.ToInt32(Request.QueryString["key"]);

                    Session["LayoutId"] = LayoutId;
                    LoadHeader(Convert.ToInt32(Session["LayoutId"]));
                    Session.Remove("dtRet");
                    TvLayout.RefreshVirtualTree();
                    TvLayout.ExpandAll();
                }
            }
        }
        private void LoadHeader(int LayoutId)
        {
            BusinessLogicLayer.GenericMethod oGenericMethod = new BusinessLogicLayer.GenericMethod();
            DataTable DtHeader = oGenericMethod.GetDataTable("SELECT [LAYOUT_NAME],[LAYOUT_DESCRIPTION] from TBL_TRANS_LAYOUT  where [LAYOUT_ID]=" + LayoutId);

            if (DtHeader != null && DtHeader.Rows.Count > 0)
            {
                txtLayoutName.Text = Convert.ToString(DtHeader.Rows[0]["LAYOUT_NAME"]);

                txtLayoutDescription.Text = Convert.ToString(DtHeader.Rows[0]["LAYOUT_DESCRIPTION"]);
            }
        }

        protected void TvLayout_VirtualModeCreateChildren(object source, DevExpress.Web.TreeViewVirtualModeCreateChildrenEventArgs e)
        {

            List<TreeViewVirtualNode> children = new List<TreeViewVirtualNode>();
            DataTable nodesTable = null;
            //if (Session["dtRet"] == null)
            //{
            nodesTable = GetDataTable();
            //}
            //else
            //{
            //    nodesTable = (DataTable)Session["dtRet"];
            //}
            for (int i = 0; i < nodesTable.Rows.Count; i++)
            {
                var Sequence = nodesTable.Rows[i]["LAYOUTDETAILS_Sequence"] == null ? "0" : nodesTable.Rows[i]["LAYOUTDETAILS_Sequence"].ToString();
                string parentName = e.NodeName != null ? e.NodeName.ToString() : "0";
                if (nodesTable.Rows[i]["LAYOUTDETAILS_ParentGroupID"].ToString() == parentName)
                {
                    TreeViewVirtualNode child = new TreeViewVirtualNode(nodesTable.Rows[i]["LAYOUTDETAILS_ID"].ToString(), nodesTable.Rows[i]["LAYOUTDETAILS_Code"].ToString() + " (" + Sequence + ")");
                    children.Add(child);
                    child.IsLeaf = !(Convert.ToInt32(nodesTable.Rows[i]["isExist"]) == 0 ? false : true);
                }
            }
            e.Children = children;
            //TvLayout.RefreshVirtualTree();
        }

        private DataTable GetDataTable()
        {


            DataTable dtRet = oDBEngine.GetDataTable("select * from (select *,1 as IsExist from [TBL_TRANS_LAYOUTDETAILS] where LAYOUTDETAILS_LAYOUTID=" + Convert.ToInt32(Request.QueryString["key"]) + " and LAYOUTDETAILS_ID in (select LAYOUTDETAILS_ParentGroupID from [TBL_TRANS_LAYOUTDETAILS] where  LAYOUTDETAILS_LAYOUTID=" + Convert.ToInt32(Request.QueryString["key"]) + ") union all select *,0 as IsExist from [TBL_TRANS_LAYOUTDETAILS] where LAYOUTDETAILS_LAYOUTID=" + Convert.ToInt32(Request.QueryString["key"]) + " and LAYOUTDETAILS_ID not in (select LAYOUTDETAILS_ParentGroupID from [TBL_TRANS_LAYOUTDETAILS] where  LAYOUTDETAILS_LAYOUTID=" + Convert.ToInt32(Request.QueryString["key"]) + "))tbl order by LAYOUTDETAILS_Sequence");

            return dtRet;
        }
        protected void cmbGroupType_Callback(object sender, CallbackEventArgsBase e)
        {

            BusinessLogicLayer.GenericMethod oGenericMethod = new BusinessLogicLayer.GenericMethod();

            string GrpName = Convert.ToString(cmbGroup.Text).Trim();
            //cmbGroupType.Text = Convert.ToString(dt.Rows[0]["AccountType"]);
            if (GrpName != "")
            {

                DataTable dtt = oGenericMethod.GetDataTable("SELECT [AccountGroupMap_ID],[AccountGroupMap_Type],[AccountGroupMap_GroupName] FROM [dbo].[tbl_AccountGroup_Type_Map] WHERE [AccountGroupMap_GroupName] = " + "'" + GrpName + "' UNION ALL SELECT '0' as  [AccountGroupMap_ID],'None' as[AccountGroupMap_Type],'' as [AccountGroupMap_GroupName]   order by AccountGroupMap_ID");
                cmbGroupType.DataSource = dtt;
                cmbGroupType.TextField = "AccountGroupMap_Type";
                cmbGroupType.ValueField = "AccountGroupMap_ID";
                cmbGroupType.DataBind();
                // SetDataSource();
            }
        }
        protected void SelectPanel_Callback(object sender, CallbackEventArgsBase e)
        {
            BusinessLogicLayer.GenericMethod oGenericMethod = new BusinessLogicLayer.GenericMethod();
            SelectPanel.JSProperties["cpAutoID"] = "";
            string Status = e.Parameter.ToString();
            string Action = "";
            var Id = "";
            if (Status != "")
            {
                Action = Status.Split('~')[0];

            }

            if (Action == "Save")
            {
                Id = Status.Split('~')[2];
                string ActionType = Status.Split('~')[1];
                string Act = ActionType;
                string DTid = "";
                string AccountGroupType = Convert.ToString(cmbGroup.Text);
                string AccountGroupCode = Convert.ToString(txtAccountShortName.Text);
                string AccountGroupName = Convert.ToString(txtAccountName.Text);
                string AccountGroupParentID = Convert.ToString(Id);
                string AccountType = Convert.ToString(cmbGroupType.Value);
                string AccountGroupSequence = null;
                string AccountParentGroupSequence = null;
                AccountGroupSequence = Convert.ToString(txtGroupSequence.Text);
                if (cmbGroup.Text == "Defined Total" || cmbGroup.Text == "Text")
                {
                    DTid = Convert.ToString(ParentId.Value);
                }


                var itemCode = ItemList.Value;


                string ItemName = "";
                string[] values = itemCode.Split(',');
                for (int i = 0; i < values.Length; i++)
                {
                    string TempItemName = Convert.ToString(values[i].Trim());
                    if (TempItemName != "" || TempItemName != null)
                    {
                        if (TempItemName == "+" || TempItemName == "-")
                        {
                            ItemName += TempItemName;
                        }
                        else
                        {
                            ItemName += "[" + TempItemName + "]";
                        }
                    }
                }

                
                //if (cmbGroup.Text == "Defined Total")
                //{
                //    if (ListFormula.Items.Count > 0)
                //    {
                //        for (int i = 0; i < ListFormula.Items.Count; i++)
                //        {
                //            string TempItemName =Convert.ToString(ListFormula.Items[i].Value);
                //            if (TempItemName != "" || TempItemName != null)
                //            {
                //                if (TempItemName == "+" || TempItemName == "-")
                //                {
                //                    ItemName += ListFormula.Items[i].Value;
                //                }
                //                else
                //                {
                //                    ItemName +="["+ ListFormula.Items[i].Value +"]";
                //                }
                //            }
                //        }

                //    }
                //}

                //if (ParentId.Value != "1")
                //{
                //    AccountGroupSequence = Convert.ToString(txtGroupSequence.Text);
                //}
                //else
                //{
                //    AccountParentGroupSequence = Convert.ToString(txtGroupSequence.Text);
                //}

                string AccountGroupSchedule = Convert.ToString(txtGroupSchedule.Text);
                int CreateUser = Convert.ToInt32(Convert.ToString(Session["userid"]));
                //string AccountGroupID = Convert.ToString(Id);
                try
                {
                    if (SaveData(Act, AccountGroupType, AccountGroupCode, AccountGroupName, AccountGroupParentID, AccountType, AccountGroupSequence, AccountGroupSchedule, CreateUser, AccountParentGroupSequence, DTid, ItemName))
                    {
                        //TvLayout.RefreshVirtualTree();
                        txtAccountShortName.Enabled = true;
                        TvLayout.RefreshVirtualTree();
                        TvLayout.ExpandAll();

                    }
                    else
                    {

                    }
                }
                catch
                {

                }
            }
            else if (Action == "Edit")
            {
                Id = Status.Split('~')[1];
                DataTable dt = GetDataTableData(Convert.ToInt32(Id));

                if (dt != null && dt.Rows.Count > 0)
                {
                    cmbGroup.Text = Convert.ToString(dt.Rows[0]["LayoutDetails_Type"]);
                    //cmbGroupType.Text = Convert.ToString(dt.Rows[0]["AccountType"]);
                    if (cmbGroup.Text != "")
                    {

                        DataTable dtt = oGenericMethod.GetDataTable("SELECT [AccountGroupMap_ID],[AccountGroupMap_Type],[AccountGroupMap_GroupName] FROM [dbo].[tbl_AccountGroup_Type_Map] WHERE [AccountGroupMap_GroupName] = " + "'" + cmbGroup.Text + "' UNION ALL SELECT '0' as  [AccountGroupMap_ID],'None' as[AccountGroupMap_Type],'' as [AccountGroupMap_GroupName]  order by AccountGroupMap_ID");
                        cmbGroupType.DataSource = dtt;
                        cmbGroupType.TextField = "AccountGroupMap_Type";
                        cmbGroupType.ValueField = "AccountGroupMap_ID";
                        cmbGroupType.DataBind();
                        // SetDataSource();
                    }
                    cmbGroupType.Value = Convert.ToString(dt.Rows[0]["LAYOUTDETAILS_AccountGroupMap_ID"]);
                    txtAccountName.Text = Convert.ToString(dt.Rows[0]["LayoutDetails_Name"]);
                    txtAccountShortName.Text = Convert.ToString(dt.Rows[0]["LayoutDetails_Code"]);
                    txtGroupSequence.Text = Convert.ToString(dt.Rows[0]["LAYOUTDETAILS_Sequence"]);
                    txtGroupSchedule.Text = Convert.ToString(dt.Rows[0]["LAYOUTDETAILS_Schedule"]);

                    if (Convert.ToString(dt.Rows[0]["LAYOUTDETAILS_ParentGroupID"]) == "")
                    {
                        SelectPanel.JSProperties["cpAutoID"] = "ParentNull";
                    }
                    else
                    {
                        SelectPanel.JSProperties["cpAutoID"] = "HasParent";
                    }

                    txtAccountShortName.ClientEnabled = false;
                    cmbGroupType.ClientEnabled = false;

                }
                //if (ParentId.Value != "1")
                //{
                //    GroupLabel.Text = "Child ";
                //}
                //else
                //{
                //    GroupLabel.Text = "Parent ";
                //}
                if (cmbGroup.Text == "Defined Total" || cmbGroup.Text == "Net Total")
                {
                    DataTable dtDefinedTotal = oDBEngine.GetDataTable("select * from [TBL_TRANS_LAYOUTDETAILS] where LAYOUTDETAILS_LAYOUTID=" + Convert.ToInt32(Request.QueryString["key"]) + " and LAYOUTDETAILS_Type not in ('Text') and LAYOUTDETAILS_Id<>"+Id);
                    ListAccount.DataSource = dtDefinedTotal;
                    ListAccount.ValueField = "LAYOUTDETAILS_ID";
                    ListAccount.TextField = "LAYOUTDETAILS_Code";
                    ListAccount.DataBind();


                    DataSet dsInst = new DataSet();
                    //SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]); MULTI
                    SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                    SqlCommand cmd = new SqlCommand("InsertInLayoutDetails", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Action", "LoadDefineTotal");//1

                    cmd.Parameters.AddWithValue("@groupId", Id);//

                    cmd.CommandTimeout = 0;
                    SqlDataAdapter Adap = new SqlDataAdapter();
                    Adap.SelectCommand = cmd;
                    Adap.Fill(dsInst);
                    cmd.Dispose();
                    con.Dispose();

                    ListFormula.Items.Clear();
                    if (dsInst != null )
                    {
                    ListFormula.DataSource = dsInst.Tables[0];
                    ListFormula.ValueField = "LAYOUTDETAILS_ID";
                    ListFormula.TextField = "LAYOUTDETAILS_Code";
                    ListFormula.DataBind();
                    }





                }

            }
            else if (Action == "Delete")
            {
                Id = Status.Split('~')[1];
                if (Id != "")
                {
                    if (DeleteData(Id, Action))
                    {
                        TvLayout.RefreshVirtualTree();
                        TvLayout.ExpandAll();
                    }

                }
            }
            else if (Action == "")
            {

            }
            else if (Action == "BindDefinedList")
            {
                DataTable dtDefinedTotal = oDBEngine.GetDataTable("select * from [TBL_TRANS_LAYOUTDETAILS] where LAYOUTDETAILS_LAYOUTID=" + Convert.ToInt32(Request.QueryString["key"]) + " and LAYOUTDETAILS_Type not in ('Text')");
                ListAccount.DataSource = dtDefinedTotal;
                ListAccount.ValueField = "LAYOUTDETAILS_ID";
                ListAccount.TextField = "LAYOUTDETAILS_Code";
                ListAccount.DataBind();
            }
            else if (Action == "CutPaste")
            {

                Id = Status.Split('~')[1];
                string ParentId = Status.Split('~')[2];

                if (ParentId == "R")
                {
                    ParentId = "0";
                }

                DataSet dsInst = new DataSet();
                //SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]); MULTI
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("InsertInLayoutDetails", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Action", "EditParent");//1
                cmd.Parameters.AddWithValue("@AccountGroupParentID", ParentId);//5
                cmd.Parameters.AddWithValue("@groupId", Id);//5
                cmd.Parameters.AddWithValue("@CreateUser", Convert.ToInt32(Convert.ToString(Session["userid"])));//
                cmd.Parameters.AddWithValue("@layoutId", Convert.ToInt32(Session["LayoutId"]));

                //@layoutId
                //cmd.Parameters.AddWithValue("@AccountGroupID", AccountGroupID);//26
                //SqlParameter output = new SqlParameter("@ReturnValueID", SqlDbType.Int);

                cmd.CommandTimeout = 0;
                SqlDataAdapter Adap = new SqlDataAdapter();
                Adap.SelectCommand = cmd;
                Adap.Fill(dsInst);
                cmd.Dispose();
                con.Dispose();
                if (dsInst != null && dsInst.Tables[0].Rows.Count > 0)
                {
                    TvLayout.RefreshVirtualTree();
                    TvLayout.ExpandAll();
                }
            }


        }

        private DataTable GetDataTableData(int ID)
        {
            BusinessLogicLayer.GenericMethod oGenericMethod = new BusinessLogicLayer.GenericMethod();
            DataTable dtt = oGenericMethod.GetDataTable("select * from TBL_TRANS_LAYOUTDETAILS where LAYOUTDETAILS_ID=" + ID);

            return dtt;
        }
        public bool SaveData(string Action, string AccountGroupType, string AccountGroupCode, string AccountGroupName, string AccountGroupParentID, string AccountType, string AccountGroupSequence, string AccountGroupSchedule, int CreateUser, string AccountParentGroupSequence, string DTid, string ItemName)
        {
            try
            {

                DataSet dsInst = new DataSet();
                //SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]); MULTI
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("InsertInLayoutDetails", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Action", Action);//1
                cmd.Parameters.AddWithValue("@AccountGroupType", AccountGroupType);//2
                cmd.Parameters.AddWithValue("@AccountGroupCode", AccountGroupCode);//3
                cmd.Parameters.AddWithValue("@AccountGroupName", AccountGroupName);//4
                cmd.Parameters.AddWithValue("@AccountGroupParentID", AccountGroupParentID);//5
                cmd.Parameters.AddWithValue("@AccountType", AccountType);//6
                cmd.Parameters.AddWithValue("@AccountGroupSequence", AccountGroupSequence);//7
                cmd.Parameters.AddWithValue("@AccountParentGroupSequence", AccountParentGroupSequence);//7
                cmd.Parameters.AddWithValue("@AccountGroupSchedule", AccountGroupSchedule);//8
                cmd.Parameters.AddWithValue("@CreateUser", CreateUser);//
                cmd.Parameters.AddWithValue("@layoutId", Convert.ToInt32(Session["LayoutId"]));
                cmd.Parameters.AddWithValue("@DtParentId", DTid);
                cmd.Parameters.AddWithValue("@FormulaString", ItemName);

                //@layoutId
                //cmd.Parameters.AddWithValue("@AccountGroupID", AccountGroupID);//26
                //SqlParameter output = new SqlParameter("@ReturnValueID", SqlDbType.Int);

                cmd.CommandTimeout = 0;
                SqlDataAdapter Adap = new SqlDataAdapter();
                Adap.SelectCommand = cmd;
                Adap.Fill(dsInst);
                cmd.Dispose();
                con.Dispose();
                if (dsInst.Tables[0] != null && dsInst.Tables[0].Rows.Count > 0)
                {
                    SelectPanel.JSProperties["cpAutoID"] = Action;
                    return true;
                }
                else
                {
                    SelectPanel.JSProperties["cpAutoID"] = "Error";
                    return false;
                }
            }
            catch (Exception e)
            {
                SelectPanel.JSProperties["cpAutoID"] = "Error";
                return false;
            }
        }
        private bool DeleteData(string AccountGroupID, string Action)
        {
            try
            {
                DataSet dsInst = new DataSet();
                //SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]); MULTI
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("InsertInLayoutDetails", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Action", "DeletePl");//1
                cmd.Parameters.AddWithValue("@AccountGroupID", AccountGroupID);//26
                cmd.CommandTimeout = 0;
                SqlDataAdapter Adap = new SqlDataAdapter();
                Adap.SelectCommand = cmd;
                Adap.Fill(dsInst);
                cmd.Dispose();
                con.Dispose();
                if (dsInst.Tables[0] != null && dsInst.Tables[0].Rows.Count > 0 && Convert.ToString(dsInst.Tables[0].Rows[0][0]) == "Success")
                {
                    SelectPanel.JSProperties["cpAutoID"] = Action;
                    return true;
                }
                else if (dsInst.Tables[0] != null && dsInst.Tables[0].Rows.Count > 0 && Convert.ToString(dsInst.Tables[0].Rows[0][0]) == "Mapped")
                {
                    SelectPanel.JSProperties["cpAutoID"] = "Mapped";
                    return false;
                }
                else
                {
                    SelectPanel.JSProperties["cpAutoID"] = "Error";
                    return false;
                }

            }
            catch
            {
                SelectPanel.JSProperties["cpAutoID"] = "Error";
                return false;
            }

        }

        protected void LedgerGrid_DataBinding(object sender, EventArgs e)
        {
            DataTable dtt = new DataTable();
            if (Session["LedgerDetails"] == null)
            {
                BusinessLogicLayer.GenericMethod oGenericMethod = new BusinessLogicLayer.GenericMethod();
                string id = Convert.ToString(GroupId.Value);
                dtt = (DataTable)Session["LedgerDetails"];
                //dtt = oGenericMethod.GetDataTable("select MainAccount_ReferenceID LedgerCode,MainAccount_AccountCode LedgerCodeName,''GroupCode,'' GroupName,MainAccount_Name LedgerName from Master_MainAccount where MainAccount_AccountType in ('Income','Expenses') and MainAccount_ReferenceID not in (select LEDGER_MAP_LEDGERID from TBL_TRANS_LAYOUTDETAILS_PL_LEDGER_MAP where LEDGER_MAP_LAYOUT_ID=" + Convert.ToInt32(Request.QueryString["key"]) + " AND LEDGER_MAP_LAYOUTDETAILS_ID<>" + id + ")");
            }
            else
            {
                dtt = (DataTable)Session["LedgerDetails"];
            }
            LedgerGrid.DataSource = dtt;
           // Session["LedgerDetails"] = null;
        }

        protected void LedgerPanel_Callback(object sender, CallbackEventArgsBase e)
        {
            BusinessLogicLayer.GenericMethod oGenericMethod = new BusinessLogicLayer.GenericMethod();
            LedgerPanel.JSProperties["cpAutoID"] = "";
            if (e.Parameter.ToString().Split('~')[0] == "ShowDetails")
            {


                string id = e.Parameter.ToString().Split('~')[1];
                DataSet dsInst = new DataSet();
                //SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]); MULTI
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("InsertInLayoutDetails", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Action", "LedgerBindPL");
                cmd.Parameters.AddWithValue("@layoutId", Convert.ToInt32(Request.QueryString["key"]));
                cmd.Parameters.AddWithValue("@detailsId", id);
                cmd.CommandTimeout = 0;
                SqlDataAdapter Adap = new SqlDataAdapter();
                Adap.SelectCommand = cmd;
                Adap.Fill(dsInst);
                cmd.Dispose();
                con.Dispose();

                DataTable dtt = dsInst.Tables[0];
                //DataTable dtt = oGenericMethod.GetDataTable("select MainAccount_ReferenceID LedgerCode,MainAccount_AccountCode LedgerCodeName,''GroupCode,'' GroupName,MainAccount_Name LedgerName from Master_MainAccount where MainAccount_AccountType in ('Asset','Liability') and MainAccount_ReferenceID not in (select LEDGER_MAP_LEDGERID from TBL_TRANS_LAYOUTDETAILS_PL_LEDGER_MAP where LEDGER_MAP_LAYOUT_ID=" + Convert.ToInt32(Request.QueryString["key"]) + " AND LEDGER_MAP_LAYOUTDETAILS_ID<>"+ id +")" );
                Session["LedgerDetails"] = dtt;
                LedgerGrid.DataSource = dtt;
                LedgerGrid.DataBind();



                DataTable dttKey = oGenericMethod.GetDataTable("select MainAccount_ReferenceID LedgerCode,MainAccount_AccountCode LedgerCodeName,''GroupCode,'' GroupName,MainAccount_Name LedgerName from Master_MainAccount where MainAccount_AccountType in ('Income','Expense') and MainAccount_ReferenceID  in (select LEDGER_MAP_LEDGERID from TBL_TRANS_LAYOUTDETAILS_PL_LEDGER_MAP where LEDGER_MAP_LAYOUT_ID=" + Convert.ToInt32(Request.QueryString["key"]) + " AND LEDGER_MAP_LAYOUTDETAILS_ID=" + id + ")");

                LedgerGrid.Selection.UnselectAll();
                foreach (DataRow dr in dttKey.Rows)
                {
                    LedgerGrid.Selection.SelectRowByKey(Convert.ToString(dr["LedgerCode"]));
                }






            }
            else if (e.Parameter.ToString().Split('~')[0] == "SaveMap")
            {
                string id = e.Parameter.ToString().Split('~')[1];
                string LayoutId = Convert.ToString(Request.QueryString["key"]);
                var SelectList = LedgerGrid.GetSelectedFieldValues("LedgerCode");

                DataTable DtTable = new DataTable();
                DtTable.Columns.Add("Id", typeof(System.Int32));

                foreach (var item in SelectList)
                {
                    DtTable.Rows.Add(Convert.ToInt32(item));
                }

                SaveLedgerMap(id, LayoutId, DtTable);
            }
        }

        private void SaveLedgerMap(string id, string LayoutId, DataTable SelectList)
        {
            try
            {

                DataSet dsInst = new DataSet();
                //SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]); MULTI
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("InsertInLayoutDetails", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Action", "MapLedgerPL");//1
                cmd.Parameters.AddWithValue("@groupId", id);//2
                cmd.Parameters.AddWithValue("@layoutId", LayoutId);//3
                cmd.Parameters.AddWithValue("@LedgerId", SelectList);//4


                cmd.CommandTimeout = 0;
                SqlDataAdapter Adap = new SqlDataAdapter();
                Adap.SelectCommand = cmd;
                Adap.Fill(dsInst);
                cmd.Dispose();
                con.Dispose();
                if (dsInst.Tables[0] != null && dsInst.Tables[0].Rows.Count > 0)
                {
                    LedgerPanel.JSProperties["cpAutoID"] = "Sucsess";

                }
                else
                {
                    LedgerPanel.JSProperties["cpAutoID"] = "Error";

                }
            }
            catch (Exception e)
            {
                LedgerPanel.JSProperties["cpAutoID"] = "Error";

            }
        }

        [WebMethod]
        public static bool CheckUniqueCode(string uniqueCode, string uniqueid, string LayoutId)
        {
            bool flag = false;
            BusinessLogicLayer.GenericMethod oGenericMethod = new BusinessLogicLayer.GenericMethod();
            try
            {

                DataTable dt = new DataTable();
                if (uniqueid == "0")
                {
                    dt = oGenericMethod.GetDataTable("SELECT * FROM [dbo].[TBL_TRANS_LAYOUTDETAILS] WHERE [LAYOUTDETAILS_Code] = " + "'" + uniqueCode + "'  and LAYOUTDETAILS_LAYOUTID=" + LayoutId + "");
                }
                else
                {
                    dt = oGenericMethod.GetDataTable("SELECT * FROM [dbo].[TBL_TRANS_LAYOUTDETAILS] WHERE [LAYOUTDETAILS_Code] = " + "'" + uniqueCode + "' and LAYOUTDETAILS_ID <>'" + uniqueid + "'  and LAYOUTDETAILS_LAYOUTID=" + LayoutId + "");
                }

                int cnt = dt.Rows.Count;
                if (cnt > 0)
                {
                    flag = true;
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
            }
            return flag;
        }

        [WebMethod]
        public static bool CheckUniqueName(string uniqueName, string uniqueid, string LayoutId)
        {
            bool flag = false;
            BusinessLogicLayer.GenericMethod oGenericMethod = new BusinessLogicLayer.GenericMethod();
            try
            {

                DataTable dt = new DataTable();
                if (uniqueid == "0")
                {
                    dt = oGenericMethod.GetDataTable("SELECT * FROM [dbo].[TBL_TRANS_LAYOUTDETAILS] WHERE [LAYOUTDETAILS_Name] = " + "'" + uniqueName + "'  and LAYOUTDETAILS_LAYOUTID=" + LayoutId + "");
                }
                else
                {
                    dt = oGenericMethod.GetDataTable("SELECT * FROM [dbo].[TBL_TRANS_LAYOUTDETAILS] WHERE [LAYOUTDETAILS_Name] = " + "'" + uniqueName + "' and LAYOUTDETAILS_ID <>'" + uniqueid + "'  and LAYOUTDETAILS_LAYOUTID=" + LayoutId + "");
                }

                int cnt = dt.Rows.Count;
                if (cnt > 0)
                {
                    flag = true;
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
            }
            return flag;
        }

        [WebMethod]
        public static bool CheckUniqueSequence(string uniqueid, string Sequence, string LayoutId)
        {
            bool flag = false;
            BusinessLogicLayer.GenericMethod oGenericMethod = new BusinessLogicLayer.GenericMethod();
            try
            {

                DataTable dt = new DataTable();
                if (uniqueid == "0")
                {
                    dt = oGenericMethod.GetDataTable("SELECT * FROM [dbo].[TBL_TRANS_LAYOUTDETAILS] WHERE [LAYOUTDETAILS_Sequence] = " + "'" + Sequence + "' and LAYOUTDETAILS_LAYOUTID=" + LayoutId + "");
                }
                else
                {
                    dt = oGenericMethod.GetDataTable("SELECT * FROM [dbo].[TBL_TRANS_LAYOUTDETAILS] WHERE [LAYOUTDETAILS_Sequence] = " + "'" + Sequence + "' and LAYOUTDETAILS_ID <>'" + uniqueid + "' and LAYOUTDETAILS_LAYOUTID=" + LayoutId + "");
                }

                int cnt = dt.Rows.Count;
                if (cnt > 0)
                {
                    flag = true;
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
            }
            return flag;
        }

    }
}