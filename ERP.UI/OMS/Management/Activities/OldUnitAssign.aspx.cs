using BusinessLogicLayer;
using DataAccessLayer;
using DevExpress.Web;
using EntityLayer.CommonELS;
using ERP.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ERP.OMS.Management.Activities
{
    public partial class OldUnitAssign : ERP.OMS.ViewState_class.VSPage
    {
        OldUnitAssignReceivedBL oBL = new OldUnitAssignReceivedBL();
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        PosSalesInvoiceBl PosData = new PosSalesInvoiceBl();


        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();


        SlaesActivitiesBL objSlaesActivitiesBL = new SlaesActivitiesBL();
        string JVNumStr = string.Empty;
        public static bool IsNumberingSchema = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/activities/oldunitassign.aspx");
            //fillGrid();

            if (!Page.IsPostBack)
            {

                oldUnitDataSource.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);

                Bind_BranchCombo(CmbBranch);
                GetScheme();
                GetFinacialYearBasedQouteDate();
               // tDate
            }
        }
        public void GetFinacialYearBasedQouteDate()
        {
            String finyear = "";
            string setdate = null;
            if (Session["LastFinYear"] != null)
            {
                finyear = Convert.ToString(Session["LastFinYear"]).Trim();
                DataTable dtFinYear = objSlaesActivitiesBL.GetFinacialYearBasedQouteDate(finyear);
                if (dtFinYear != null && dtFinYear.Rows.Count > 0)
                {
                    Session["FinYearStartDate"] = Convert.ToString(dtFinYear.Rows[0]["finYearStartDate"]);
                    Session["FinYearEndDate"] = Convert.ToString(dtFinYear.Rows[0]["finYearEndDate"]);
                    if (Session["FinYearStartDate"] != null)
                    {
                        tDate.MinDate = Convert.ToDateTime(Convert.ToString(Session["FinYearStartDate"]));
                    }
                    if (Session["FinYearEndDate"] != null)
                    {
                        tDate.MaxDate = Convert.ToDateTime(Convert.ToString(Session["FinYearEndDate"]));
                    }
                    if (oDBEngine.GetDate().Date >= Convert.ToDateTime(Convert.ToString(Session["FinYearStartDate"])) && oDBEngine.GetDate().Date <= Convert.ToDateTime(Convert.ToString(Session["FinYearEndDate"])))
                    {

                    }
                    else if (oDBEngine.GetDate().Date >= Convert.ToDateTime(Convert.ToString(Session["FinYearStartDate"])) && oDBEngine.GetDate().Date >= Convert.ToDateTime(Convert.ToString(Session["FinYearEndDate"])))
                    {
                        setdate = Convert.ToString(Convert.ToDateTime(Session["FinYearEndDate"]).Month) + "/" + Convert.ToString(Convert.ToDateTime(Session["FinYearEndDate"]).Day) + "/" + Convert.ToString(Convert.ToDateTime(Session["FinYearEndDate"]).Year);
                        tDate.Value = DateTime.ParseExact(setdate, @"M/d/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        //dt_PLQuote.Value = DateTime.ParseExact(Convert.ToString(Session["FinYearStartDate"]), @"M/d/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    }
                    else if (oDBEngine.GetDate().Date <= Convert.ToDateTime(Convert.ToString(Session["FinYearStartDate"])) && oDBEngine.GetDate().Date <= Convert.ToDateTime(Convert.ToString(Session["FinYearEndDate"])))
                    {
                        setdate = Convert.ToString(Convert.ToDateTime(Session["FinYearStartDate"]).Month) + "/" + Convert.ToString(Convert.ToDateTime(Session["FinYearStartDate"]).Day) + "/" + Convert.ToString(Convert.ToDateTime(Session["FinYearStartDate"]).Year);
                        tDate.Value = DateTime.ParseExact(setdate, @"M/d/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                        //dt_PLQuote.Value = DateTime.ParseExact(Convert.ToString(Session["FinYearEndDate"]), @"M/d/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    }
                }
            }
        }
        private void Bind_BranchCombo(DevExpress.Web.ASPxComboBox CmbBranch)
        {
            var data = oBL.GetBranch(Convert.ToInt32(HttpContext.Current.Session["userbranchID"]), Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]));

            if (data != null && data.Rows.Count > 0)
            {
                CmbBranch.Items.Clear();
                CmbBranch.TextField = "branch_code";
                CmbBranch.ValueField = "branch_id";
                CmbBranch.DataSource = data;
                CmbBranch.DataBind();
                //CmbBranch.Items.Insert(0, new ListEditItem("Select Branch", 0));
            }
        }
        protected void EntityServerModeDataSource_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "Invoice_Id";
            List<int> branchidlist;

            //string connectionString = ConfigurationManager.ConnectionStrings["crmConnectionString"].ConnectionString;
            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);


            string BranchList = Convert.ToString(Session["userbranchHierarchy"]);
            branchidlist = new List<int>(Array.ConvertAll(BranchList.Split(','), int.Parse));
            

            ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
            var q = from d in dc.V_SalesChallan_Details_OldUnitAssignLists
                    where

                    branchidlist.Contains(Convert.ToInt32(d.pos_assignBranch)) //Indent_BranchIdTo
                    orderby d.Invoice_DateTimeFormat descending
                    select d;
            e.QueryableSource = q;
            
        }
        public void fillGrid()
        {
            DataTable DT = oBL.GetOldUnitAssignData(Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]), "OldUnitAssign");

            if (DT != null && DT.Rows.Count > 0)
            {
                gridStatus.DataSource = DT;
                gridStatus.DataBind();
            }
        }
        [WebMethod]
        [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
        public static string SaveAssignedBranch(tbl_trans_oldunit tbl_trans_oldunit)
        {
            try
            {
                tbl_trans_oldunit.financial_year = Convert.ToString(HttpContext.Current.Session["LastFinYear"]);
                tbl_trans_oldunit.assign_from_branch = Convert.ToString(HttpContext.Current.Session["userbranchID"]);
                tbl_trans_oldunit.assigned_by = Convert.ToInt32(HttpContext.Current.Session["userid"]);
                tbl_trans_oldunit.company_Id = Convert.ToString(HttpContext.Current.Session["LastCompany"]);

                return OldUnitAssignReceivedBL.AssignedBranch((object)tbl_trans_oldunit);
            }
            catch (Exception ex)
            {
                return "Error occured";
            }
        }
        [WebMethod]
        public static string getSchemeType(string sel_scheme_id)
        {
            string strschematype = "", strschemalength = "", strschemavalue = "", strbranchID = "";

            //BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
            BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine();


            DataTable DT = objEngine.GetDataTable("tbl_master_Idschema", " schema_type,length,IsNull(Branch,0) as Branch", " Id = " + Convert.ToInt32(sel_scheme_id));

            for (int i = 0; i < DT.Rows.Count; i++)
            {
                strschematype = Convert.ToString(DT.Rows[i]["schema_type"]);
                strschemalength = Convert.ToString(DT.Rows[i]["length"]);
                strbranchID = Convert.ToString(DT.Rows[i]["Branch"]);

                strschemavalue = strschematype + "~" + strschemalength + "~" + strbranchID;
            }

            //BusinessLogicLayer.DBEngine oDbEngine1 = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
            //string[] scheme = oDbEngine1.GetFieldValue1("tbl_master_Idschema", " schema_type,length ", "Id = " + Convert.ToInt32(sel_scheme_id), 1);

            return Convert.ToString(strschemavalue);
        }
        [WebMethod]
        public static bool CheckUniqueName(string VoucherNo, string Type)
        {
            MShortNameCheckingBL objMShortNameCheckingBL = new MShortNameCheckingBL();
            DataTable dt = new DataTable();
            Boolean status = false;
            BusinessLogicLayer.GenericMethod oGeneric = new BusinessLogicLayer.GenericMethod();

            if (VoucherNo != "" && Convert.ToString(VoucherNo).Trim() != "")
            {
                status = objMShortNameCheckingBL.CheckUnique(Convert.ToString(VoucherNo).Trim(), Type, "OldUnit_Check");
            }
            return status;
        }
        #region oldUnitValue
        private void createOldUnittable()
        {
            DataTable oldUnitTable = new DataTable();
            oldUnitTable.Columns.Add("oldUnit_id", typeof(System.Int64));
            oldUnitTable.Columns.Add("Product_id", typeof(System.Int64));
            oldUnitTable.Columns.Add("Product_Des", typeof(System.String));
            oldUnitTable.Columns.Add("oldUnit_Uom", typeof(System.String));
            oldUnitTable.Columns.Add("oldUnit_qty", typeof(System.Decimal));
            oldUnitTable.Columns.Add("oldUnit_value", typeof(System.Decimal));

            Session["PosOldUnittable_ADVICE" + Convert.ToString(hfInvoice_Id.Value)] = oldUnitTable;
        }
        protected void OldUnitGrid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            string requestedString = e.Parameters.Split('~')[0];
            OldUnitGrid.JSProperties["cpReturnString"] = e.Parameters;
            OldUnitGrid.JSProperties["cpSave"] = "";

            #region SaveOldUnit
            if (requestedString == "SaveOldUnit")
            {
                string Invoice_Id = e.Parameters.Split('~')[1];
                //Old Unit Table 
                DataTable oldUnitTable = (DataTable)Session["PosOldUnittable_ADVICE" + Convert.ToString(hfInvoice_Id.Value)];

                if (oldUnitTable == null)
                {
                    createOldUnittable();
                    oldUnitTable = (DataTable)Session["PosOldUnittable_ADVICE" + Convert.ToString(hfInvoice_Id.Value)];
                }

                if (Invoice_Id != "" && oldUnitTable != null)
                {
                    string SchemaID = (Convert.ToString(hdnSchemaID.Value) == "") ? "0" : Convert.ToString(hdnSchemaID.Value);
                    string validate = checkNMakeJVCode(Convert.ToString(txtBillNo.Text), Convert.ToInt32(SchemaID));

                    if (validate == "ok")
                    {
                        {
                            DataTable dtSchema = oDBEngine.GetDataTable("tbl_master_idschema", "prefix, suffix, digit, startno, schema_type", "id=" + Convert.ToInt32(SchemaID));
                            int scheme_type = Convert.ToInt32(dtSchema.Rows[0]["schema_type"]);
                            if (scheme_type != 0)
                            {
                                OldUnitGrid.JSProperties["cpVouvherNo"] = JVNumStr;
                                OldUnitAssignReceivedBL.SaveOldUnit(oldUnitTable, Invoice_Id, tDate.Date.ToString("yyyy-MM-dd"), JVNumStr);
                                OldUnitGrid.JSProperties["cpSave"] = "Old Unit Product Added Successfully.";
                            }
                        }
                    }

                }
                else
                {
                    OldUnitGrid.JSProperties["cpSave"] = "";
                }
            }
            #endregion
            #region DisplayOldUnit
            else if (requestedString == "DisplayOldUnit")
            {
                try
                {
                    //DataTable oldUnitTable = (DataTable)Session["PosOldUnittable_ADVICE" + Convert.ToString(hfInvoice_Id.Value)];
                    DataSet oldUnitTable = OldUnitAssignReceivedBL.GetOldUnitDataByInvoiceId(Convert.ToString(hfInvoice_Id.Value));

                    if (oldUnitTable != null)
                    {
                        OldUnitGrid.DataSource = oldUnitTable.Tables[1];
                        OldUnitGrid.DataBind();
                        txtBillNo.Enabled = false;
                        txtBillNo.Text = Convert.ToString(oldUnitTable.Tables[0].Rows[0]["OldUnitAdvice_DocNo"]);
                        tDate.Enabled = false;
                        //tDate.Value = Convert.ToString(oldUnitTable.Tables[0].Rows[0]["OldUnitAdvice_DocDate"]).Equals("") ? null : oldUnitTable.Tables[0].Rows[0]["OldUnitAdvice_DocDate"];
                        if (oldUnitTable.Tables[0].Rows[0]["OldUnitAdvice_DocDate"] != null && !Convert.ToString(oldUnitTable.Tables[0].Rows[0]["OldUnitAdvice_DocDate"]).Equals(""))
                            tDate.Date = Convert.ToDateTime(Convert.ToString(oldUnitTable.Tables[0].Rows[0]["OldUnitAdvice_DocDate"]));
                    }
                    else
                    {
                        oldUnitTable = OldUnitAssignReceivedBL.GetOldUnitDataByInvoiceId(Convert.ToString(hfInvoice_Id.Value));

                        if (oldUnitTable != null)
                        {
                            OldUnitGrid.DataSource = oldUnitTable.Tables[1];
                            OldUnitGrid.DataBind();
                            Session["PosOldUnittable_ADVICE" + Convert.ToString(hfInvoice_Id.Value)] = oldUnitTable.Tables[1];
                            txtBillNo.Enabled = false;
                            txtBillNo.Text = "";
                            tDate.Enabled = false;

                        }
                        else
                        {
                            OldUnitGrid.DataSource = null;
                            OldUnitGrid.DataBind();
                            CmbScheme.Visible = true;
                            txtBillNo.Enabled = true;
                            tDate.Enabled = true;
                        }
                    }
                }
                catch (Exception ex) { }
            }
            #endregion
            #region Add OldUnit
            else if (requestedString == "AddDataToTable")
            {
                DataTable oldUnitTable = (DataTable)Session["PosOldUnittable_ADVICE" + Convert.ToString(hfInvoice_Id.Value)];
                if (oldUnitTable == null)
                {
                    createOldUnittable();
                }
                oldUnitTable = (DataTable)Session["PosOldUnittable_ADVICE" + Convert.ToString(hfInvoice_Id.Value)];

                if (Session["updatedId" + Convert.ToString(hfInvoice_Id.Value)] != null && Convert.ToInt64(Session["updatedId" + Convert.ToString(hfInvoice_Id.Value)]) != 0) //Update
                {
                    DataRow[] existingRow = oldUnitTable.Select("oldUnit_id='" + Convert.ToInt64(Session["updatedId"] + Convert.ToString(hfInvoice_Id.Value)) + "'");
                    if (existingRow.Length > 0)
                    {
                        string productId = Convert.ToString(oldUnitProductLookUp.Value == null ? 0 : oldUnitProductLookUp.Value);
                        existingRow[0]["Product_id"] = productId.Split(new string[] { "|@|" }, StringSplitOptions.None)[0];
                        existingRow[0]["Product_Des"] = oldUnitProductLookUp.Text;
                        existingRow[0]["oldUnit_Uom"] = txtOldUnitUom.Text;
                        existingRow[0]["oldUnit_qty"] = Convert.ToDecimal(txtOldUnitqty.Text); ;
                        existingRow[0]["oldUnit_value"] = Convert.ToDecimal(txtoldUnitValue.Text);
                        Session["updatedId" + Convert.ToString(hfInvoice_Id.Value)] = 0;
                        bindOldUnitGrid();
                    }
                }
                else //Add
                {
                    Random r = new Random();
                    int rInt = r.Next(0, 1000); //for ints
                    DataRow oldUnitRow = oldUnitTable.NewRow();
                    oldUnitRow["oldUnit_id"] = rInt; // Guid.NewGuid().ToString();
                    string productId = Convert.ToString(oldUnitProductLookUp.Value == null ? 0 : oldUnitProductLookUp.Value);
                    oldUnitRow["Product_id"] = productId.Split(new string[] { "|@|" }, StringSplitOptions.None)[0];
                    oldUnitRow["Product_Des"] = oldUnitProductLookUp.Text;
                    oldUnitRow["oldUnit_Uom"] = txtOldUnitUom.Text;
                    oldUnitRow["oldUnit_qty"] = Convert.ToDecimal(txtOldUnitqty.Text);
                    oldUnitRow["oldUnit_value"] = Convert.ToDecimal(txtoldUnitValue.Text);
                    oldUnitTable.Rows.Add(oldUnitRow);
                    Session["PosOldUnittable_ADVICE" + Convert.ToString(hfInvoice_Id.Value)] = oldUnitTable;
                    Session["updatedId" + Convert.ToString(hfInvoice_Id.Value)] = 0;
                    bindOldUnitGrid();
                }
            }
            #endregion
            #region Delete
            else if (requestedString == "DeleteFromTable")
            {
                string id = e.Parameters.Split('~')[1];
                DataTable oldUnitTable = (DataTable)Session["PosOldUnittable_ADVICE" + Convert.ToString(hfInvoice_Id.Value)];
                if (oldUnitTable != null)
                {
                    DataRow[] existingRow = oldUnitTable.Select("oldUnit_id='" + id + "'");
                    if (existingRow.Length > 0)
                    {
                        foreach (DataRow dr in existingRow)
                        {
                            oldUnitTable.Rows.Remove(dr);
                        }

                    }
                    //if (Convert.ToString(hfInvoice_Id.Value) != "" && oldUnitTable != null)
                    //{
                    //    OldUnitAssignReceivedBL.SaveOldUnit(oldUnitTable, Convert.ToString(hfInvoice_Id.Value));
                    //}
                    Session["PosOldUnittable_ADVICE" + Convert.ToString(hfInvoice_Id.Value)] = oldUnitTable;
                    bindOldUnitGrid();

                }

            }
            #endregion
            #region DeleteAll
            else if (requestedString == "DeleteAllRecord")
            {
                createOldUnittable();
            }
            #endregion

            OldUnitGrid.JSProperties["cpTotalOldUnit"] = GetTotalOldUnitValue();

        }

        public string GetTotalOldUnitValue()
        {
            string ReturnValue = "0";
            DataTable oldUnitTable = (DataTable)Session["PosOldUnittable_ADVICE" + Convert.ToString(hfInvoice_Id.Value)];
            if (oldUnitTable != null)
            {
                ReturnValue = Convert.ToString(oldUnitTable.Compute("SUM(oldUnit_value)", string.Empty));
            }
            if (ReturnValue.Trim() == "")
            {
                ReturnValue = "0";
            }
            return ReturnValue;
        }

        private void bindOldUnitGrid()
        {
            DataTable oldUnitTable = (DataTable)Session["PosOldUnittable_ADVICE" + Convert.ToString(hfInvoice_Id.Value)];
            if (oldUnitTable != null)
            {
                OldUnitGrid.DataSource = oldUnitTable;
                OldUnitGrid.DataBind();
            }
        }

        protected void oldUnitUpdatePanel_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            string requestedString = e.Parameter.Split('~')[0];
            //OldUnitGrid.JSProperties["cpReturnString"] = e.Parameter;
            //OldUnitGrid.JSProperties["cpSave"] = "";
            oldUnitUpdatePanel.JSProperties["cpReturnString"] = e.Parameter;
            oldUnitUpdatePanel.JSProperties["cpSave"] = "";
            oldUnitUpdatePanel.JSProperties["cpDuplicateProduct"] = "";
            oldUnitUpdatePanel.JSProperties["cpClear"] = "";
            oldUnitUpdatePanel.JSProperties["cpDisplay"] = "";

            #region Update
            if (requestedString == "Update")
            {
                string updatedId = e.Parameter.Split('~')[1];
                DataTable oldUnitTable = (DataTable)Session["PosOldUnittable_ADVICE" + Convert.ToString(hfInvoice_Id.Value)];
                DataRow[] existingRow = oldUnitTable.Select("oldUnit_id='" + updatedId + "'");
                if (existingRow.Length > 0)
                {
                    oldUnitProductLookUp.GridView.Selection.SelectRowByKey(Convert.ToString(existingRow[0]["Product_id"]) + "|@|" + Convert.ToString(existingRow[0]["oldUnit_Uom"]));
                    txtOldUnitUom.Text = Convert.ToString(existingRow[0]["oldUnit_Uom"]);
                    txtOldUnitqty.Text = Convert.ToString(existingRow[0]["oldUnit_qty"]);  //Convert.ToString(Convert.ToDecimal(existingRow[0]["oldUnit_qty"]));
                    txtoldUnitValue.Text = Convert.ToString(existingRow[0]["oldUnit_value"]);
                    Session["updatedId" + Convert.ToString(hfInvoice_Id.Value)] = updatedId;
                    txtBillNo.Enabled = false;
                    tDate.Enabled = false;
                    DataSet DS = OldUnitAssignReceivedBL.GetOldUnitDataByInvoiceId(Convert.ToString(hfInvoice_Id.Value));
                    if (DS != null && DS.Tables[0].Rows.Count > 0)
                    {
                        //Edit Mode
                        pnlschema.Visible = false;
                        pnlschema.Enabled = false;
                    }
                    else
                    {
                        //Add Mode
                        pnlschema.Visible = true;
                        pnlschema.Enabled = true;
                    }
                }
            }
            #endregion
            #region SaveOldUnit
            if (requestedString == "SaveOldUnit")
            {
                string Invoice_Id = e.Parameter.Split('~')[1];
                //Old Unit Table 
                DataTable oldUnitTable = (DataTable)Session["PosOldUnittable_ADVICE" + Convert.ToString(hfInvoice_Id.Value)];

                if (oldUnitTable == null)
                {
                    createOldUnittable();
                    oldUnitTable = (DataTable)Session["PosOldUnittable_ADVICE" + Convert.ToString(hfInvoice_Id.Value)];
                }

                if (Invoice_Id != "" && oldUnitTable != null)
                {
                    string SchemaID = (Convert.ToString(hdnSchemaID.Value) == "") ? "0" : Convert.ToString(hdnSchemaID.Value);
                    string validate = checkNMakeJVCode(Convert.ToString(txtBillNo.Text), Convert.ToInt32(SchemaID));

                    if (validate == "ok")
                    {
                        {
                            DataTable dtSchema = oDBEngine.GetDataTable("tbl_master_idschema", "prefix, suffix, digit, startno, schema_type", "id=" + Convert.ToInt32(SchemaID));
                            int scheme_type = Convert.ToInt32(dtSchema.Rows[0]["schema_type"]);
                            //if (scheme_type != 0)
                            //{
                            oldUnitUpdatePanel.JSProperties["cpVouvherNo"] = JVNumStr;
                            OldUnitAssignReceivedBL.SaveOldUnit(oldUnitTable, Invoice_Id, tDate.Date.ToString("yyyy-MM-dd"), JVNumStr);
                            oldUnitUpdatePanel.JSProperties["cpSave"] = "Old Unit Product Added Successfully.";
                            hdnSchemaID.Value = "0";
                            //}
                        }
                    }
                    else if (validate == "noid" || validate == "duplicate")
                    {
                        DataSet DS = OldUnitAssignReceivedBL.GetOldUnitDataByInvoiceId(Convert.ToString(hfInvoice_Id.Value));
                        if (DS != null && DS.Tables[0].Rows.Count > 0)
                        {
                            JVNumStr = Convert.ToString(DS.Tables[0].Rows[0]["OldUnitAdvice_DocNo"]);
                            oldUnitUpdatePanel.JSProperties["cpVouvherNo"] = JVNumStr;
                            OldUnitAssignReceivedBL.SaveOldUnit(oldUnitTable, Invoice_Id, tDate.Date.ToString("yyyy-MM-dd"), JVNumStr);
                            oldUnitUpdatePanel.JSProperties["cpSave"] = "Old Unit Product Updated Successfully.";
                            hdnSchemaID.Value = "0";
                        }
                    }
                    else
                    {
                        //do nothing
                    }

                }
                else
                {
                    oldUnitUpdatePanel.JSProperties["cpSave"] = "";
                }
            }
            #endregion
            #region DisplayOldUnit
            else if (requestedString == "DisplayOldUnit")
            {
                try
                {
                    DataTable oldUnitTable1 = (DataTable)Session["PosOldUnittable_ADVICE" + Convert.ToString(hfInvoice_Id.Value)];
                    DataSet oldUnitTable = OldUnitAssignReceivedBL.GetOldUnitDataByInvoiceId(Convert.ToString(hfInvoice_Id.Value));
                    Session["updatedId" + Convert.ToString(hfInvoice_Id.Value)] = 0;

                    if (oldUnitTable != null && oldUnitTable.Tables[0].Rows.Count > 0)
                    {
                        OldUnitGrid.DataSource = oldUnitTable.Tables[1];
                        OldUnitGrid.DataBind();
                        txtBillNo.Enabled = false;
                        txtBillNo.Text = Convert.ToString(oldUnitTable.Tables[0].Rows[0]["OldUnitAdvice_DocNo"]);
                        IsNumberingSchema = true; // numbering schema will not appear in edit mode
                        tDate.Enabled = false;
                        //tDate.Value = Convert.ToString(oldUnitTable.Tables[0].Rows[0]["OldUnitAdvice_DocDate"]).Equals("") ? null : oldUnitTable.Tables[0].Rows[0]["OldUnitAdvice_DocDate"];
                        if (oldUnitTable.Tables[0].Rows[0]["OldUnitAdvice_DocDate"] != null && !Convert.ToString(oldUnitTable.Tables[0].Rows[0]["OldUnitAdvice_DocDate"]).Equals(""))
                            tDate.Date = Convert.ToDateTime(Convert.ToString(oldUnitTable.Tables[0].Rows[0]["OldUnitAdvice_DocDate"]));
                        Session["PosOldUnittable_ADVICE" + Convert.ToString(hfInvoice_Id.Value)] = oldUnitTable.Tables[1];
                        OldUnitPopUpControl.HeaderText = "Old Unit Add";
                        //Edit Mode
                        pnlschema.Visible = false;
                        pnlschema.Enabled = false;
                        oldUnitUpdatePanel.JSProperties["cpDisplay"] = "Old Unit Edit";
                    }
                    else
                    {
                        //oldUnitTable = OldUnitAssignReceivedBL.GetOldUnitDataByInvoiceId(Convert.ToString(hfInvoice_Id.Value));

                        //if (oldUnitTable != null && oldUnitTable.Tables[0].Rows.Count > 0)
                        //{
                        //    OldUnitGrid.DataSource = oldUnitTable.Tables[1];
                        //    OldUnitGrid.DataBind();
                        //    Session["PosOldUnittable_ADVICE" + Convert.ToString(hfInvoice_Id.Value)] = oldUnitTable.Tables[1];
                        //    txtBillNo.Enabled = false;
                        //    txtBillNo.Text = "";
                        //    tDate.Enabled = false;

                        //}
                        //else
                        //{
                        OldUnitGrid.DataSource = null;
                        OldUnitGrid.DataBind();
                        CmbScheme.Visible = true;
                        pnlschema.Visible = true;
                        txtBillNo.Enabled = true;
                        txtBillNo.Text = "";
                        tDate.Enabled = true;
                        OldUnitPopUpControl.HeaderText = "Old Unit Edit";
                        Session["PosOldUnittable_ADVICE" + Convert.ToString(hfInvoice_Id.Value)] = null;
                        GetScheme();
                        //Add Mode
                        pnlschema.Visible = true;
                        pnlschema.Enabled = true;
                        oldUnitUpdatePanel.JSProperties["cpDisplay"] = "Old Unit Add";
                        //}
                    }
                }
                catch (Exception ex) { }
            }
            #endregion
            #region Add OldUnit
            else if (requestedString == "AddDataToTable")
            {
                DataTable oldUnitTable = (DataTable)Session["PosOldUnittable_ADVICE" + Convert.ToString(hfInvoice_Id.Value)];
                DataSet DS = OldUnitAssignReceivedBL.GetOldUnitDataByInvoiceId(Convert.ToString(hfInvoice_Id.Value));
                if (DS != null && DS.Tables[0].Rows.Count > 0)
                {
                    //Edit Mode
                    pnlschema.Visible = false;
                    pnlschema.Enabled = false;
                    txtBillNo.Enabled = false;
                    tDate.Enabled = false;
                }
                else
                {
                    //Add Mode
                    pnlschema.Visible = true;
                    pnlschema.Enabled = true;
                    txtBillNo.Enabled = true;
                    tDate.Enabled = true;
                }

                if (oldUnitTable == null)
                {
                    createOldUnittable();
                }
                oldUnitTable = (DataTable)Session["PosOldUnittable_ADVICE" + Convert.ToString(hfInvoice_Id.Value)];

                if (Session["updatedId" + Convert.ToString(hfInvoice_Id.Value)] != null && Convert.ToInt64(Session["updatedId" + Convert.ToString(hfInvoice_Id.Value)]) != 0) //Update
                {
                    //string productId = Convert.ToString(oldUnitProductLookUp.Value == null ? 0 : oldUnitProductLookUp.Value);
                    //DataRow[] existingProductRow = oldUnitTable.Select("Product_id='" + productId.Split(new string[] { "|@|" }, StringSplitOptions.None)[0] + "'");
                    //if (existingProductRow.Length > 0)
                    //{
                    //    oldUnitUpdatePanel.JSProperties["cpDuplicateProduct"] = "1";
                    //    Session["PosOldUnittable_ADVICE" + Convert.ToString(hfInvoice_Id.Value)] = oldUnitTable;
                    //    Session["updatedId" + Convert.ToString(hfInvoice_Id.Value)] = 0;
                    //    bindOldUnitGrid();
                    //}
                    //else
                    //{
                    DataRow[] existingRow = oldUnitTable.Select("oldUnit_id='" + Convert.ToInt64(Session["updatedId" + Convert.ToString(hfInvoice_Id.Value)]) + "'");
                    if (existingRow.Length > 0)
                    {
                        string productId = Convert.ToString(oldUnitProductLookUp.Value == null ? 0 : oldUnitProductLookUp.Value);
                        existingRow[0]["Product_id"] = productId.Split(new string[] { "|@|" }, StringSplitOptions.None)[0];
                        existingRow[0]["Product_Des"] = oldUnitProductLookUp.Text;
                        existingRow[0]["oldUnit_Uom"] = txtOldUnitUom.Text;
                        existingRow[0]["oldUnit_qty"] = Convert.ToDecimal(txtOldUnitqty.Text); ;
                        existingRow[0]["oldUnit_value"] = Convert.ToDecimal(txtoldUnitValue.Text);
                        Session["updatedId" + Convert.ToString(hfInvoice_Id.Value)] = 0;
                        bindOldUnitGrid();
                        txtBillNo.Enabled = false;
                        tDate.Enabled = false;
                    }
                    //}

                }
                else //Add
                {
                    string productId = Convert.ToString(oldUnitProductLookUp.Value == null ? 0 : oldUnitProductLookUp.Value);
                    DataRow[] existingProductRow = oldUnitTable.Select("Product_id='" + productId.Split(new string[] { "|@|" }, StringSplitOptions.None)[0] + "'");
                    if (existingProductRow.Length > 0)
                    {
                        oldUnitUpdatePanel.JSProperties["cpDuplicateProduct"] = "1";
                        Session["PosOldUnittable_ADVICE" + Convert.ToString(hfInvoice_Id.Value)] = oldUnitTable;
                        Session["updatedId" + Convert.ToString(hfInvoice_Id.Value)] = 0;
                        bindOldUnitGrid();
                    }
                    else
                    {
                        Random r = new Random();
                        int rInt = r.Next(0, 1000); //for ints
                        DataRow oldUnitRow = oldUnitTable.NewRow();
                        oldUnitRow["oldUnit_id"] = rInt; // Guid.NewGuid().ToString();
                        oldUnitRow["Product_id"] = productId.Split(new string[] { "|@|" }, StringSplitOptions.None)[0];
                        oldUnitRow["Product_Des"] = oldUnitProductLookUp.Text;
                        oldUnitRow["oldUnit_Uom"] = txtOldUnitUom.Text;
                        oldUnitRow["oldUnit_qty"] = Convert.ToDecimal(txtOldUnitqty.Text);
                        oldUnitRow["oldUnit_value"] = Convert.ToDecimal(txtoldUnitValue.Text);
                        oldUnitTable.Rows.Add(oldUnitRow);
                        Session["PosOldUnittable_ADVICE" + Convert.ToString(hfInvoice_Id.Value)] = oldUnitTable;
                        Session["updatedId" + Convert.ToString(hfInvoice_Id.Value)] = 0;
                        bindOldUnitGrid();
                    }

                }
            }
            #endregion
            #region Delete
            else if (requestedString == "DeleteFromTable")
            {
                string id = e.Parameter.Split('~')[1];
                DataTable oldUnitTable = (DataTable)Session["PosOldUnittable_ADVICE" + Convert.ToString(hfInvoice_Id.Value)];
                if (oldUnitTable != null)
                {
                    DataRow[] existingRow = oldUnitTable.Select("oldUnit_id='" + id + "'");
                    if (existingRow.Length > 0)
                    {
                        foreach (DataRow dr in existingRow)
                        {
                            oldUnitTable.Rows.Remove(dr);
                        }

                    }
                    //if (Convert.ToString(hfInvoice_Id.Value) != "" && oldUnitTable != null)
                    //{
                    //    OldUnitAssignReceivedBL.SaveOldUnit(oldUnitTable, Convert.ToString(hfInvoice_Id.Value));
                    //}
                    DataSet DS = OldUnitAssignReceivedBL.GetOldUnitDataByInvoiceId(Convert.ToString(hfInvoice_Id.Value));
                    if (DS != null && DS.Tables[0].Rows.Count > 0)
                    {
                        //Edit Mode
                        pnlschema.Visible = false;
                        pnlschema.Enabled = false;
                    }
                    else
                    {
                        //Add Mode
                        pnlschema.Visible = true;
                        pnlschema.Enabled = true;
                    }
                    txtBillNo.Enabled = false;
                    tDate.Enabled = false;
                    Session["PosOldUnittable_ADVICE" + Convert.ToString(hfInvoice_Id.Value)] = oldUnitTable;
                    bindOldUnitGrid();
                    oldUnitUpdatePanel.JSProperties["cpClear"] = "1";
                }

            }
            #endregion
            #region DeleteAll
            else if (requestedString == "DeleteAllRecord")
            {
                createOldUnittable();
            }
            #endregion
            #region Clear
            else if (requestedString == "Clear")
            {
                DataSet DS = OldUnitAssignReceivedBL.GetOldUnitDataByInvoiceId(Convert.ToString(hfInvoice_Id.Value));
                if (DS != null && DS.Tables[0].Rows.Count > 0)
                {
                    //Edit Mode
                    pnlschema.Visible = false;
                    pnlschema.Enabled = false;
                    txtBillNo.Enabled = false;
                    tDate.Enabled = false;
                }
                else
                {
                    //Add Mode
                    pnlschema.Visible = true;
                    pnlschema.Enabled = true;
                    txtBillNo.Enabled = true;
                    tDate.Enabled = true;
                }
                Session["updatedId" + Convert.ToString(hfInvoice_Id.Value)] = 0;
                oldUnitUpdatePanel.JSProperties["cpClear"] = "1";
            }
            #endregion

            oldUnitUpdatePanel.JSProperties["cpTotalOldUnit"] = GetTotalOldUnitValue();

        }

        private void SetOldUnitDetails(string invid)
        {
            DataTable QuotationEditdt = PosData.GetOldUnitDetails(invid);
            Session["PosOldUnittable_ADVICE" + Convert.ToString(hfInvoice_Id.Value)] = QuotationEditdt;
        }

        #endregion
        public void GetScheme()
        {
            string sel_type_id = "Cr";
            string LastCompany = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
            string userbranchHierarchy = Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]);
            string LastFinYear = Convert.ToString(HttpContext.Current.Session["LastFinYear"]);


            //BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
            BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine();



            //string query = "(Select '0' as ID,'Select' as SchemaName) UNION (Select ID,SchemaName +(Case When (SELECT Isnull(branch_description,'') FROM tbl_master_branch where branch_id=Branch) Is Null Then '' Else ' ('+(SELECT Isnull(branch_description,'') FROM tbl_master_branch where branch_id=Branch)+')' End) as SchemaName From tbl_master_Idschema  Where TYPE_ID=(Case When '" + sel_type_id + "'='Dr' Then '25' Else '26' End) AND IsActive=1 AND Isnull(Branch,'') in (select s FROM dbo.GetSplit(',','" + userbranchHierarchy + "')) AND Isnull(comapanyInt,'')='" + LastCompany + "' AND financial_year_id=(Select Top 1 FinYear_ID FROM Master_FinYear WHERE FinYear_Code='" + LastFinYear + "'))";
            string query = "(Select '0' as ID,'Select' as SchemaName) UNION (Select ID,SchemaName +(Case When (SELECT Isnull(branch_description,'') FROM tbl_master_branch where branch_id=Branch) Is Null Then '' Else ' ('+(SELECT Isnull(branch_description,'') FROM tbl_master_branch where branch_id=Branch)+')' End) as SchemaName From tbl_master_Idschema  Where TYPE_ID=53 AND IsActive=1 AND Isnull(Branch,'') in (select s FROM dbo.GetSplit(',','" + userbranchHierarchy + "')) AND Isnull(comapanyInt,'')='" + LastCompany + "' AND financial_year_id=(Select Top 1 FinYear_ID FROM Master_FinYear WHERE FinYear_Code='" + LastFinYear + "'))";

            DataTable DT = objEngine.GetDataTable(query);

            if (DT != null && DT.Rows.Count > 0)
            {
                CmbScheme.DataSource = DT;
                CmbScheme.DataTextField = "SchemaName";
                CmbScheme.DataValueField = "ID";
                CmbScheme.DataBind();
            }

        }
        protected string checkNMakeJVCode(string manual_str, int sel_schema_Id)
        {
            DataTable dtSchema = new DataTable();
            DataTable dtC = new DataTable();
            string prefCompCode = string.Empty, sufxCompCode = string.Empty, startNo, paddedStr, sqlQuery = string.Empty;
            int EmpCode, prefLen, sufxLen, paddCounter;

            if (sel_schema_Id > 0)
            {
                dtSchema = oDBEngine.GetDataTable("tbl_master_idschema", "prefix, suffix, digit, startno, schema_type", "id=" + sel_schema_Id);
                int scheme_type = Convert.ToInt32(dtSchema.Rows[0]["schema_type"]);

                if (scheme_type != 0)
                {
                    startNo = dtSchema.Rows[0]["startno"].ToString();
                    paddCounter = Convert.ToInt32(dtSchema.Rows[0]["digit"]);
                    paddedStr = startNo.PadLeft(paddCounter, '0');
                    prefCompCode = dtSchema.Rows[0]["prefix"].ToString();
                    sufxCompCode = dtSchema.Rows[0]["suffix"].ToString();
                    prefLen = Convert.ToInt32(prefCompCode.Length);
                    sufxLen = Convert.ToInt32(sufxCompCode.Length);

                    sqlQuery = "SELECT max(tjv.DCNote_DocumentNumber) FROM Trans_CustDebitCreditNote tjv WHERE dbo.RegexMatch('";
                    if (prefLen > 0)
                        sqlQuery += "^[" + prefCompCode + "]{" + prefLen + "}";
                    else if (scheme_type == 2)
                        sqlQuery += "^";
                    sqlQuery += "[0-9]{" + paddCounter + "}";
                    if (sufxLen > 0)
                        sqlQuery += "[" + sufxCompCode + "]{" + sufxLen + "}";
                    //sqlQuery += "?$', LTRIM(RTRIM(tjv.DCNote_DocumentNumber))) = 1";
                    sqlQuery += "?$', LTRIM(RTRIM(tjv.DCNote_DocumentNumber))) = 1 and DCNote_DocumentNumber like '" + prefCompCode + "%'";
                    if (scheme_type == 2)
                        sqlQuery += " AND CONVERT(DATE, JournalVoucher_CreateDateTime) = CONVERT(DATE, GETDATE())";
                    dtC = oDBEngine.GetDataTable(sqlQuery);

                    if (dtC.Rows[0][0].ToString() == "")
                    {
                        sqlQuery = "SELECT max(tjv.DCNote_DocumentNumber) FROM Trans_CustDebitCreditNote tjv WHERE dbo.RegexMatch('";
                        if (prefLen > 0)
                            sqlQuery += "^[" + prefCompCode + "]{" + prefLen + "}";
                        else if (scheme_type == 2)
                            sqlQuery += "^";
                        sqlQuery += "[0-9]{" + (paddCounter - 1) + "}";
                        if (sufxLen > 0)
                            sqlQuery += "[" + sufxCompCode + "]{" + sufxLen + "}";
                        // sqlQuery += "?$', LTRIM(RTRIM(tjv.DCNote_DocumentNumber))) = 1";
                        sqlQuery += "?$', LTRIM(RTRIM(tjv.DCNote_DocumentNumber))) = 1 and DCNote_DocumentNumber like '" + prefCompCode + "%'";
                        if (scheme_type == 2)
                            sqlQuery += " AND CONVERT(DATE, JournalVoucher_CreateDateTime) = CONVERT(DATE, GETDATE())";
                        dtC = oDBEngine.GetDataTable(sqlQuery);
                    }

                    if (dtC.Rows.Count > 0 && dtC.Rows[0][0].ToString().Trim() != "")
                    {
                        string uccCode = dtC.Rows[0][0].ToString().Trim();
                        int UCCLen = uccCode.Length;
                        int decimalPartLen = UCCLen - (prefCompCode.Length + sufxCompCode.Length);
                        string uccCodeSubstring = uccCode.Substring(prefCompCode.Length, decimalPartLen);
                        EmpCode = Convert.ToInt32(uccCodeSubstring) + 1;
                        // out of range journal scheme
                        if (EmpCode.ToString().Length > paddCounter)
                        {
                            return "outrange";
                        }
                        else
                        {
                            paddedStr = EmpCode.ToString().PadLeft(paddCounter, '0');
                            JVNumStr = prefCompCode + paddedStr + sufxCompCode;
                            return "ok";
                        }
                    }
                    else
                    {
                        JVNumStr = startNo.PadLeft(paddCounter, '0');
                        JVNumStr = prefCompCode + paddedStr + sufxCompCode;
                        return "ok";
                    }
                }
                else
                {
                    sqlQuery = "SELECT DCNote_DocumentNumber FROM Trans_CustDebitCreditNote WHERE DCNote_DocumentNumber LIKE '" + manual_str.Trim() + "'";
                    dtC = oDBEngine.GetDataTable(sqlQuery);
                    // duplicate manual entry check
                    if (dtC.Rows.Count > 0 && dtC.Rows[0][0].ToString().Trim() != "")
                    {
                        return "duplicate";
                    }

                    JVNumStr = manual_str.Trim();
                    return "ok";
                }
            }
            else
            {
                return "noid";
            }
        }
    }
}