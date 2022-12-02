using BusinessLogicLayer;
using BusinessLogicLayer.ServiceManagement;
using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using UtilityLayer;

namespace ServiceManagement.ServiceManagement.Transaction.Jobsheet
{
    public partial class jobsheetEntry : System.Web.UI.Page
    {
        SrvAssignJobBL obj = new SrvAssignJobBL();
        CommonBL ComBL = new CommonBL();
        String userbranch = null;
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        protected void Page_Load(object sender, EventArgs e)
        {
            //DataTable dt = new DataTable();
            string MultiBranchNumberingScheme = ComBL.GetSystemSettingsResult("NumberingSchemeWithMultiBranch");
            string AllowOnlinePrintinServiceManagement = ComBL.GetSystemSettingsResult("AllowOnlinePrintinServiceManagement");
            hdnOnlinePrint.Value = AllowOnlinePrintinServiceManagement;

            string IsEntityInformationEditableInJobsheet = ComBL.GetSystemSettingsResult("IsEntityInformationEditableInJobsheet");
            hdnIsEntityInformationEditableInJobsheet.Value = IsEntityInformationEditableInJobsheet;

            if (!String.IsNullOrEmpty(MultiBranchNumberingScheme))
            {
                if (MultiBranchNumberingScheme.ToUpper().Trim() == "YES")
                {
                    userbranch = EmployeeBranchMap();
                }
                else
                {
                    userbranch = Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]);
                }
            }
            else
            {
                userbranch = Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]);
            }
            string ComponentWithQtyChecking = ComBL.GetSystemSettingsResult("ComponentWithQtyChecking");
            hdnComponentQty.Value = ComponentWithQtyChecking;
            if (!IsPostBack)
            {
                Session["DeviceDetails"] = null;
                Session["ComponentDetails"] = null;
                //string userbranchHierachy = Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]);
                PopulateBranchByHierchy(userbranch);
                //ddlBranch.Value = Convert.ToString(Session["userbranchID"]);
                PopulateEntryType();
                DataTable dt = obj.GetAssignJobDetails(userbranch);

                if (dt != null && dt.Rows.Count > 0)
                {
                    ddlTechnician.DataSource = dt;
                    ddlTechnician.ValueField = "cnt_id";
                    ddlTechnician.TextField = "cnt_firstName";
                    ddlTechnician.DataBind();
                    ddlTechnician.SelectedIndex = 0;
                }
                if (Request.QueryString["Key"] != "ADD")
                {
                    hdnEntryMode.Value = "Edit";

                    Hidden_add_edit.Value = "Update";
                    hdnJobSheetID.Value = Request.QueryString["id"];

                    PopulateJobSheetDetailsById(Request.QueryString["id"]);

                    if (Request.QueryString["Key"] == "Edit")
                    {
                        HeaderName.InnerText = " Edit Jobsheet";
                        Hidden_add_edit.Value = "Update";
                    }
                    else
                    {
                        HeaderName.InnerText = " View Jobsheet";
                        Hidden_add_edit.Value = "View";
                    }
                }
                else
                {
                    HeaderName.InnerText = " Add Jobsheet";
                    CmbScheme.DataSource = BindNumberingScheme("138");
                    CmbScheme.ValueField = "ID";
                    CmbScheme.TextField = "SchemaName";
                    CmbScheme.DataBind();
                    CmbScheme.SelectedIndex = 0;
                    Hidden_add_edit.Value = "Insert";
                    hdnJobSheetID.Value = "0";

                    hdnEntryMode.Value = "Add";
                }
            }
        }

        private void PopulateBranchByHierchy(string userbranchhierchy)
        {
            PosSalesInvoiceBl posSale = new PosSalesInvoiceBl();
            DataTable branchtable = posSale.getBranchListByHierchy(userbranchhierchy);
            ddlBranch.DataSource = branchtable;
            ddlBranch.ValueField = "branch_id";
            ddlBranch.TextField = "branch_description";
            ddlBranch.DataBind();
            ddlBranch.SelectedIndex = 0;
        }

        private void PopulateEntryType()
        {
            DataSet branchtable = dsFetchAll();

            ddlServiceAction.DataSource = branchtable.Tables[0];
            ddlServiceAction.ValueField = "SrvActionID";
            ddlServiceAction.TextField = "SrvActionDesc";
            ddlServiceAction.DataBind();
            ddlServiceAction.SelectedIndex = 0;

            ddlProblem.DataSource = branchtable.Tables[1];
            ddlProblem.ValueField = "ProblemID";
            ddlProblem.TextField = "ProblemDesc";
            ddlProblem.DataBind();
            ddlProblem.SelectedIndex = 0;

            //ddlTechnician.DataSource = branchtable.Tables[2];
            //ddlTechnician.ValueField = "cnt_id";
            //ddlTechnician.TextField = "cnt_firstName";
            //ddlTechnician.DataBind();
            //ddlTechnician.SelectedIndex = 0;

            ddlReturnReason.DataSource = branchtable.Tables[3];
            ddlReturnReason.ValueField = "ReasonID";
            ddlReturnReason.TextField = "ReasonDesc";
            ddlReturnReason.DataBind();
            ddlReturnReason.SelectedIndex = 0;

            ddlModel.DataSource = branchtable.Tables[4];
            ddlModel.DataValueField = "ModelID";
            ddlModel.DataTextField = "ModelDesc";
            ddlModel.DataBind();
            ddlModel.SelectedIndex = 0;

            DiviceTyp.DataSource = branchtable.Tables[5];
            DiviceTyp.ValueField = "ID";
            DiviceTyp.TextField = "DeviceType";
            DiviceTyp.DataBind();
            DiviceTyp.SelectedIndex = 1;

            // Mantis Issue 24413/24417
            selModel.DataSource = branchtable.Tables[6];
            selModel.ValueField = "ModelID";
            selModel.TextField = "ModelDesc";
            selModel.DataBind();
            selModel.SelectedIndex = 0;
            // End of Mantis Issue 24413/24417
        }

        public DataSet dsFetchAll()
        {
            ProcedureExecute proc = new ProcedureExecute("PRC_SRVJobSheetDetails");
            proc.AddVarcharPara("@ACTION", 500, "FETCH");
            DataSet ds = proc.GetDataSet();
            return ds;
        }

        public DataTable BindNumberingScheme(string ReceiptType)
        {
            string strCompanyID = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
            string FinYear = Convert.ToString(HttpContext.Current.Session["LastFinYear"]);
            // string userbranchHierarchy = Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]);
            //DataTable Schemadt = GetNumberingSchema(strCompanyID, userbranchHierarchy, FinYear, ReceiptType, "Y");
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("PRC_SRVJobSheetDetails");
            proc.AddVarcharPara("@ACTION", 500, "GetNumberingSchema");
            proc.AddVarcharPara("@CompanyID", 100, strCompanyID);
            proc.AddVarcharPara("@BranchID", 4000, userbranch);
            proc.AddVarcharPara("@FinYear", 100, FinYear);
            proc.AddVarcharPara("@Type", 100, ReceiptType);
            proc.AddVarcharPara("@IsSplit", 100, "Y");
            ds = proc.GetTable();
            return ds;
        }

        [WebMethod]
        public static string save(SrvJobSheetInput apply)
        {
            MasterSettings masterbl = new MasterSettings();
            string mastersettings = masterbl.GetSettings("StkAdjSrv");

            string strCompanyID = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
            string FinYear = Convert.ToString(HttpContext.Current.Session["LastFinYear"]);
            string output = string.Empty;
            string DocumentNo = "";
            string strPurchaseNumber = Convert.ToString(apply.ChallanNumber);
            if (apply.Action != "Update")
            {
                string schmID = apply.NumberingScheme.Split('~')[0].ToString();
                string docNo = checkNMakeJVCode(strPurchaseNumber, Convert.ToInt32(schmID));

                if (docNo.Split('~')[0].ToString() == "ok")
                {
                    DocumentNo = docNo.Split('~')[1].ToString();
                }
                else
                {
                    return "";
                }
            }
            else
            {
                DocumentNo = apply.ChallanNumber;
            }

            DataTable Devicedt = (DataTable)HttpContext.Current.Session["DeviceDetails"];
            DataTable dt = new DataTable();
            DataTable dtableComp = (DataTable)HttpContext.Current.Session["ComponentDetails"];

            try
            {
                if (Devicedt != null && Devicedt.Rows.Count > 0)
                {
                    if (HttpContext.Current.Session["userid"] != null)
                    {
                        int user_id = Convert.ToInt32(HttpContext.Current.Session["userid"]);
                        ProcedureExecute proc = new ProcedureExecute("PRC_SRVJobSheetDetails");
                        proc.AddPara("@Action", Convert.ToString(apply.Action));
                        proc.AddPara("@NumberingScheme", Convert.ToString(apply.NumberingScheme));
                        proc.AddPara("@ChallanNumber", Convert.ToString(DocumentNo));
                        proc.AddPara("@RefJobsheet", Convert.ToString(apply.RefJobsheet));
                        proc.AddPara("@TechnicianID", Convert.ToString(apply.AssignTo));
                        proc.AddPara("@WorkDone_Date", apply.WorkDoneOn);
                        proc.AddPara("@BranchID", Convert.ToString(apply.Location));
                        proc.AddPara("@HeaderRemarks", apply.HeaderRemarks);
                        proc.AddPara("@EntityCode", apply.EntityCode);
                        proc.AddPara("@NetworkName", apply.NetworkName);
                        proc.AddPara("@ContactPerson", apply.ContactPerson);
                        proc.AddPara("@ContactNumber", apply.ContactNumber);
                        proc.AddPara("@UserID", user_id);
                        proc.AddPara("@DeviceNumber", Convert.ToString(apply.SerialNumber));
                        proc.AddPara("@DeviceType", Convert.ToString(apply.DeviceType));
                        proc.AddPara("@Model", Convert.ToString(apply.Model));
                        proc.AddPara("@Problem", Convert.ToString(apply.Problem));
                        proc.AddPara("@OtherProblem", apply.OtherProblem);
                        proc.AddPara("@ServiceAction", Convert.ToString(apply.ServiceAction));
                        proc.AddPara("@Warranty", Convert.ToString(apply.Warranty));
                        proc.AddPara("@ReturnReason", apply.ReturnReason);
                        proc.AddPara("@NewModel", apply.NewModel);
                        proc.AddPara("@DetailsRemarks", apply.DetailsRemarks);
                        proc.AddPara("@IsBillable", apply.Billable);
                        proc.AddPara("@COMPONENT", apply.Components);
                        proc.AddPara("@JobsheetID", apply.JobsheetID);
                        proc.AddPara("@CompanyID", strCompanyID);
                        proc.AddPara("@FinYear", FinYear);
                        proc.AddPara("@StockAdj_Require", mastersettings);
                        proc.AddPara("@udt_JobSheetDetails", Devicedt);
                        proc.AddPara("@udt_JobSheetComponentDetails", dtableComp);

                        proc.AddPara("@PostingDate", apply.PostingDate);
                        dt = proc.GetTable();
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            output = dt.Rows[0]["Status"].ToString() + "~" + DocumentNo + "~" + dt.Rows[0]["msg"].ToString() + "~" + dt.Rows[0]["ID"].ToString();
                        }
                    }
                }
                else
                {
                    output = "~~Please Add Device.~";
                }

            }
            catch (Exception ex)
            {
                output = ex.Message.ToString();
            }
            return output;
        }

        public static string checkNMakeJVCode(string manual_str, int sel_schema_Id)
        {
            DateTime date = DateTime.Today;
            int day = date.Day;
            int month = date.Month;
            int year = date.Year;
            string ddtat = DateTime.Today.ToString("ddMMyyyy");

            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
            string UniqueQuotation = "";
            //  oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
            oDBEngine = new BusinessLogicLayer.DBEngine();


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
                    if (scheme_type == 2)
                    {
                        paddCounter = paddCounter + 8;
                    }
                    prefCompCode = dtSchema.Rows[0]["prefix"].ToString();
                    sufxCompCode = dtSchema.Rows[0]["suffix"].ToString();
                    prefLen = Convert.ToInt32(prefCompCode.Length);
                    sufxLen = Convert.ToInt32(sufxCompCode.Length);

                    sqlQuery = "SELECT max(tjv.ChallanNumber) FROM SRV_JobSheetHeader tjv WHERE dbo.RegexMatch('";
                    if (prefLen > 0)
                        sqlQuery += "^[" + prefCompCode + "]{" + prefLen + "}";
                    else if (scheme_type == 2)
                        sqlQuery += "^";
                    sqlQuery += "[0-9]{" + paddCounter + "}";
                    if (sufxLen > 0)
                        sqlQuery += "[" + sufxCompCode + "]{" + sufxLen + "}";
                    //sqlQuery += "?$', LTRIM(RTRIM(tjv.ChallanNumber))) = 1";
                    sqlQuery += "?$', LTRIM(RTRIM(tjv.ChallanNumber))) = 1 and ChallanNumber like '" + prefCompCode + "%'";
                    if (scheme_type == 2)
                        sqlQuery += " AND CONVERT(DATE, CreateDate) = CONVERT(DATE, GETDATE())";
                    dtC = oDBEngine.GetDataTable(sqlQuery);

                    if (dtC.Rows[0][0].ToString() == "")
                    {
                        sqlQuery = "SELECT max(tjv.ChallanNumber) FROM SRV_JobSheetHeader tjv WHERE dbo.RegexMatch('";
                        if (prefLen > 0)
                            sqlQuery += "^[" + prefCompCode + "]{" + prefLen + "}";
                        else if (scheme_type == 2)
                            sqlQuery += "^";
                        sqlQuery += "[0-9]{" + (paddCounter - 1) + "}";
                        if (sufxLen > 0)
                            sqlQuery += "[" + sufxCompCode + "]{" + sufxLen + "}";
                        //sqlQuery += "?$', LTRIM(RTRIM(tjv.ChallanNumber))) = 1";
                        sqlQuery += "?$', LTRIM(RTRIM(tjv.ChallanNumber))) = 1 and ChallanNumber like '" + prefCompCode + "%' and ChallanNumber like '%" + sufxCompCode + "'";
                        if (scheme_type == 2)
                            sqlQuery += " AND CONVERT(DATE, CreateDate) = CONVERT(DATE, GETDATE())";
                        dtC = oDBEngine.GetDataTable(sqlQuery);
                    }

                    if (dtC.Rows.Count > 0 && dtC.Rows[0][0].ToString().Trim() != "")
                    {
                        string uccCode = dtC.Rows[0][0].ToString().Trim();
                        int UCCLen = uccCode.Length;
                        int decimalPartLen = 0;
                        string uccCodeSubstring = "";
                        if (scheme_type == 2)
                        {
                            decimalPartLen = UCCLen - (prefCompCode.Length + sufxCompCode.Length) - 8;
                            uccCodeSubstring = uccCode.Substring(prefCompCode.Length + 8, decimalPartLen);
                        }
                        else
                        {
                            decimalPartLen = UCCLen - (prefCompCode.Length + sufxCompCode.Length);
                            uccCodeSubstring = uccCode.Substring(prefCompCode.Length, decimalPartLen);
                        }


                        EmpCode = Convert.ToInt32(uccCodeSubstring) + 1;
                        // out of range journal scheme
                        if (EmpCode.ToString().Length > paddCounter)
                        {
                            return "outrange~";
                        }
                        else
                        {
                            if (scheme_type == 2)
                            {
                                paddedStr = EmpCode.ToString().PadLeft(paddCounter - 8, '0');
                                UniqueQuotation = prefCompCode + ddtat.ToString() + paddedStr + sufxCompCode;
                            }
                            else
                            {
                                paddedStr = EmpCode.ToString().PadLeft(paddCounter, '0');
                                UniqueQuotation = prefCompCode + paddedStr + sufxCompCode;
                            }

                            return "ok~" + UniqueQuotation;
                        }
                    }
                    else
                    {
                        if (scheme_type == 2)
                        {
                            UniqueQuotation = startNo.PadLeft(paddCounter - 8, '0');
                            UniqueQuotation = prefCompCode + ddtat.ToString() + paddedStr + sufxCompCode;
                        }
                        else
                        {
                            UniqueQuotation = startNo.PadLeft(paddCounter, '0');
                            UniqueQuotation = prefCompCode + paddedStr + sufxCompCode;
                        }
                        return "ok~" + UniqueQuotation;
                    }
                }
                else
                {
                    sqlQuery = "SELECT ChallanNumber FROM SRV_JobSheetHeader WHERE ChallanNumber LIKE '" + manual_str.Trim() + "'";
                    dtC = oDBEngine.GetDataTable(sqlQuery);
                    // duplicate manual entry check
                    if (dtC.Rows.Count > 0 && dtC.Rows[0][0].ToString().Trim() != "")
                    {
                        return "duplicate~";
                    }

                    UniqueQuotation = manual_str.Trim();
                    return "ok~" + UniqueQuotation;
                }
            }
            else
            {
                return "noid~";
            }
        }

        private void PopulateJobSheetDetailsById(string JobSheetID)
        {
            string output = string.Empty;
            DataSet ds = new DataSet();
            try
            {
                if (HttpContext.Current.Session["userid"] != null)
                {
                    ProcedureExecute proc = new ProcedureExecute("PRC_SRVJobSheetDetails");
                    proc.AddVarcharPara("@ACTION", 300, "EditDetails");
                    proc.AddVarcharPara("@JobsheetID", 100, JobSheetID);
                    ds = proc.GetDataSet();
                    if (ds != null && ds.Tables.Count > 0)
                    {
                        divNumberingScheme.Style.Add("display", "none");
                        txtDocumentNumber.Text = ds.Tables[0].Rows[0]["ChallanNumber"].ToString();
                        //txtDocumentNumber.Style.Add("disable", "disable");
                        txtDocumentNumber.Enabled = false;

                        txtRefJobsheet.Value = Convert.ToString(ds.Tables[0].Rows[0]["RefJobsheet"].ToString());
                        ddlTechnician.Value = Convert.ToString(ds.Tables[0].Rows[0]["TechnicianID"].ToString());
                        FormDate.Date = Convert.ToDateTime(ds.Tables[0].Rows[0]["WorkDone_Date"].ToString());
                        //FormDate.ClientEnabled = false;
                        ddlBranch.Value = Convert.ToString(ds.Tables[0].Rows[0]["Location"].ToString());
                        ddlBranch.ClientEnabled = false;
                        txtHeaderRemarks.Value = ds.Tables[0].Rows[0]["Remarks"].ToString();
                        txtEntityCode.Value = ds.Tables[0].Rows[0]["EntityCode"].ToString();
                        txtEntityCode.Style.Add("disabled", "disabled");
                        txtNetworkName.Value = ds.Tables[0].Rows[0]["NetworkName"].ToString();
                        txtNetworkName.Style.Add("disabled", "disabled");
                        txtContactPerson.Value = ds.Tables[0].Rows[0]["ContactPerson"].ToString();
                        txtContactPerson.Style.Add("disabled", "disabled");
                        txtContactNumber.Value = ds.Tables[0].Rows[0]["ContactNumber"].ToString();
                        txtContactNumber.Style.Add("disabled", "disabled");
                        hdnEntityCode.Value = ds.Tables[0].Rows[0]["EntityCode"].ToString();

                        PostingDate.Date = Convert.ToDateTime(ds.Tables[0].Rows[0]["PostingDate"].ToString());
                        PostingDate.ClientEnabled = false;

                        if (ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0)
                        {

                            DataTable dtable = new DataTable();

                            dtable.Clear();
                            dtable.Columns.Add("HIddenID", typeof(System.Guid));
                            dtable.Columns.Add("SerialNumber", typeof(System.String));
                            dtable.Columns.Add("DeviceType", typeof(System.String));
                            dtable.Columns.Add("DeviceTypeID", typeof(System.String));
                            dtable.Columns.Add("Model", typeof(System.String));
                            dtable.Columns.Add("Problem", typeof(System.String));
                            dtable.Columns.Add("ProblemID", typeof(System.String));
                            dtable.Columns.Add("Other", typeof(System.String));
                            dtable.Columns.Add("ServiceAction", typeof(System.String));
                            dtable.Columns.Add("ServiceActionID", typeof(System.String));
                            dtable.Columns.Add("Components", typeof(System.String));
                            dtable.Columns.Add("ComponentsID", typeof(System.String));
                            dtable.Columns.Add("Warranty", typeof(System.String));
                            dtable.Columns.Add("ReturnReason", typeof(System.String));
                            dtable.Columns.Add("ReturnReasonID", typeof(System.String));
                            dtable.Columns.Add("Remarks", typeof(System.String));
                            dtable.Columns.Add("Billable", typeof(System.String));
                            dtable.Columns.Add("DetailsID", typeof(System.String));

                            DataTable dtableComp = new DataTable();
                            dtableComp.Clear();
                            dtableComp.Columns.Add("HIddenID", typeof(System.Guid));
                            dtableComp.Columns.Add("SerialNumber", typeof(System.String));
                            dtableComp.Columns.Add("ComponentId", typeof(System.String));
                            dtableComp.Columns.Add("Qty", typeof(System.String));

                            foreach (DataRow item in ds.Tables[1].Rows)
                            {
                                System.Guid guid = Guid.NewGuid();
                                object[] trow = { guid, item["DeviceNumber"].ToString(), item["DeviceType_Name"].ToString(), item["DeviceType"].ToString(), item["Model"].ToString(),
                                                        item["ProblemDesc"].ToString(), item["Problem"].ToString(), item["OtherProblem"].ToString(), item["SrvActionDesc"].ToString(),
                                                        item["ServiceAction"].ToString(), item["Component"].ToString(), item["ComponentID"].ToString(), item["Warrantydt"].ToString()
                                                , item["ReasonDesc"].ToString(), item["ReturnReason"].ToString(), item["Remarks"].ToString(), item["BILLABLE"].ToString(),
                                                item["Jobsheet_DetailsID"].ToString()};
                                dtable.Rows.Add(trow);


                                if (ds.Tables[2] != null && ds.Tables[2].Rows.Count > 0)
                                {
                                    foreach (DataRow Compitem in ds.Tables[2].Rows)
                                    {
                                        if (item["Jobsheet_DetailsID"].ToString() == Compitem["Jobsheet_DetailsID"].ToString())
                                        {
                                            object[] trow1 = { guid, item["DeviceNumber"].ToString(), Compitem["ComponentsID"].ToString(), Compitem["Quantity"].ToString() };
                                            dtableComp.Rows.Add(trow1);
                                        }
                                    }
                                }
                            }

                            HttpContext.Current.Session["ComponentDetails"] = dtableComp;
                            HttpContext.Current.Session["DeviceDetails"] = dtable;

                            GrdDevice.DataBind();
                        }

                        //txtDeviceNumber.Value = ds.Tables[1].Rows[0]["DeviceNumber"].ToString();
                        //DiviceTyp.Value = ds.Tables[1].Rows[0]["DeviceType"].ToString();
                        //txtDeviceModel.Value = ds.Tables[1].Rows[0]["Model"].ToString();
                        //ddlProblem.Value = ds.Tables[1].Rows[0]["Problem"].ToString();
                        //txtOtherProblem.Value = ds.Tables[1].Rows[0]["OtherProblem"].ToString();
                        //ddlServiceAction.Value = ds.Tables[1].Rows[0]["ServiceAction"].ToString();
                        //txtWarranty.Value = ds.Tables[1].Rows[0]["Warranty"].ToString();
                        //ddlReturnReason.Value = ds.Tables[1].Rows[0]["ReturnReason"].ToString();
                        //ddlModel.SelectedValue = ds.Tables[1].Rows[0]["NewModel"].ToString();
                        //txtDetailsRemarks.Value = ds.Tables[1].Rows[0]["Remarks"].ToString();
                        //chkBillable.Checked = Convert.ToBoolean(ds.Tables[1].Rows[0]["IsBillable"].ToString());
                        //String Componentid = "";
                        //String ComponentName = "";
                        //if (ds.Tables[2]!=null && ds.Tables[2].Rows.Count>0)
                        //{
                        //    BindLookUp(ds.Tables[1].Rows[0]["Model"].ToString());
                        //    foreach (DataRow item in ds.Tables[2].Rows)
                        //    {
                        //        if (Componentid == "")
                        //        {
                        //            Componentid = item["ComponentsID"].ToString();
                        //            ComponentName = item["sProducts_Name"].ToString();
                        //        }
                        //        else
                        //        {
                        //            Componentid = Componentid + ',' + item["ComponentsID"].ToString();
                        //            ComponentName = ComponentName + ',' + item["sProducts_Name"].ToString();
                        //        }

                        //        lookup_Component.GridView.Selection.SelectRowByKey(Convert.ToInt32(item["ComponentsID"].ToString()));
                        //    }
                        //    hdncWiseProductId.Value = Componentid;
                        //   // txtProdName.Value = ComponentName;
                        //}
                        //  txtDeviceNumber.Value = ds.Tables[1].Rows[0]["ContactNo"].ToString();

                    }
                }
            }
            catch (Exception ex)
            {
                output = ex.Message.ToString();
            }
        }

        [WebMethod]
        public static List<ServiceEntryHistory> ServiceEntryHistoryList(String model, String DeviceNumber)
        {
            List<ServiceEntryHistory> listStatues = new List<ServiceEntryHistory>();
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("PRC_SRVJobSheetDetails");
            proc.AddVarcharPara("@ACTION", 500, "ServiceEntryHistory");
            proc.AddPara("@Model", model);
            proc.AddPara("@DeviceNumber", DeviceNumber);
            dt = proc.GetTable();

            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow item in dt.Rows)
                {
                    listStatues.Add(new ServiceEntryHistory
                    {
                        EntityCode = item["EntityCode"].ToString(),
                        ReceiptNo = item["DocumentNumber"].ToString(),
                        ServiceAction = item["SrvActionDesc"].ToString(),
                        Remarks = item["Remarks"].ToString(),
                        Billable = item["Billable"].ToString()
                    });
                }
            }
            return listStatues;
        }

        [WebMethod]
        public static ServiceEntryCount ServiceEntryCountShow(String model, String DeviceNumber)
        {
            int user_id = Convert.ToInt32(HttpContext.Current.Session["userid"]);
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("PRC_SRVJobSheetDetails");
            proc.AddVarcharPara("@ACTION", 500, "ServiceEntryCount");
            proc.AddPara("@Model", model);
            proc.AddPara("@DeviceNumber", DeviceNumber);
            ds = proc.GetTable();
            ServiceEntryCount ret = new ServiceEntryCount();
            if (ds != null && ds.Rows.Count > 0)
            {
                foreach (DataRow item in ds.Rows)
                {
                    ret.Repaired = item["Repaired"].ToString();
                    ret.Exchanged = item["Exchanged"].ToString();
                    break;
                }
            }
            return ret;
        }

        [WebMethod]
        public static string GetWarrantyDate(String DeviceNumber)
        {
            String warranty = "";
            int user_id = Convert.ToInt32(HttpContext.Current.Session["userid"]);
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("PRC_SRVWarrantyUpdateFetch");
            proc.AddVarcharPara("@ACTION", 500, "GetWarrantyDate");
            proc.AddPara("@DeviceNumber", DeviceNumber);
            dt = proc.GetTable();

            if (dt != null && dt.Rows.Count > 0)
            {
                warranty = dt.Rows[0]["Warranty"].ToString();
            }
            return warranty;
        }

        [WebMethod]
        public static string GeSerialValidation(String DeviceNumber)
        {
            String status = "";
            int user_id = Convert.ToInt32(HttpContext.Current.Session["userid"]);
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("PRC_SRVReceiptChallanDetails");
            proc.AddVarcharPara("@ACTION", 500, "SerialValidation");
            proc.AddPara("@DeviceNumber", DeviceNumber);
            dt = proc.GetTable();

            if (dt != null && dt.Rows.Count > 0)
            {
                status = dt.Rows[0]["STATUS"].ToString();
            }
            return status;
        }

        #region Component Populate

        protected void Component_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            if (e.Parameter.Split('~')[0] == "BindComponentGrid")
            {
                DataTable ComponentTable = new DataTable();
                string ModelDesc = e.Parameter.Split('~')[1];
                //if (Session["userbranchHierarchy"] != null)
                //{
                ComponentTable = oDBEngine.GetDataTable("select prod.sProducts_ID as ID,prod.sProducts_Name,prod.sProducts_Code,CASE WHEN prod.sProduct_IsReplaceable=1 THEN 'Yes' ELSE 'No' END AS Replaceable from master_sproducts prod INNER JOIN SRV_ProductModelMap MAP ON MAP.Product_id=prod.sProducts_ID INNER JOIN Master_Model MDL ON MDL.ModelID=MAP.Model_id WHERE prod.isComponentService=1 AND MDL.ModelDesc='" + ModelDesc + "' AND prod.sProduct_Status!='D' order by sProducts_Name asc");
                // }
                if (ComponentTable.Rows.Count > 0)
                {
                    Session["JobSheetComponentData"] = ComponentTable;
                    lookup_Component.DataSource = ComponentTable;
                    lookup_Component.DataBind();
                }
                else
                {
                    Session["JobSheetComponentData"] = ComponentTable;
                    lookup_Component.DataSource = null;
                    lookup_Component.DataBind();
                }
            }
            else if (e.Parameter.Split('~')[0] == "SetComponentGrid")
            {
                string ModelDesc = e.Parameter.Split('~')[2];
                BindLookUp(ModelDesc);

                DataTable Componentdt = new DataTable();
                string dtlsID = e.Parameter.Split('~')[1];
                string JobSheetID = hdnJobSheetID.Value;
                //Componentdt = oDBEngine.GetDataTable("select ComponentsID from SRV_JobsheetComponentMap where JobsheetID=" + JobSheetID + " and Jobsheet_DetailsID=" + dtlsID + "");
                String[] compnts = dtlsID.Split(',');

                lookup_Component.GridView.Selection.CancelSelection();
                lookup_Component.GridView.Selection.CancelSelection();
                foreach (string item in compnts)
                {
                    if (item != "")
                    {
                        lookup_Component.GridView.Selection.SelectRowByKey(Convert.ToInt32(item));
                    }
                }
            }
        }

        protected void lookup_Component_DataBinding(object sender, EventArgs e)
        {
            if (Session["JobSheetComponentData"] != null)
            {
                lookup_Component.DataSource = (DataTable)Session["JobSheetComponentData"];
            }
        }

        protected void BindLookUp(string ModelDesc)
        {
            DataTable ComponentTable = oDBEngine.GetDataTable("select prod.sProducts_ID as ID,prod.sProducts_Name,prod.sProducts_Code,CASE WHEN prod.sProduct_IsReplaceable=1 THEN 'Yes' ELSE 'No' END AS Replaceable from master_sproducts prod INNER JOIN SRV_ProductModelMap MAP ON MAP.Product_id=prod.sProducts_ID INNER JOIN Master_Model MDL ON MDL.ModelID=MAP.Model_id WHERE prod.isComponentService=1 AND MDL.ModelDesc='" + ModelDesc + "' AND prod.sProduct_Status!='D' order by sProducts_Name asc");
            lookup_Component.GridView.Selection.CancelSelection();

            lookup_Component.GridView.Selection.CancelSelection();
            lookup_Component.DataSource = ComponentTable;
            lookup_Component.DataBind();

            Session["JobSheetComponentData"] = ComponentTable;
        }

        #endregion

        public String EmployeeBranchMap()
        {
            String branches = null;
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("PRC_EmployeeBranchMap");
            proc.AddVarcharPara("@USER_ID", 100, Session["userid"].ToString());
            ds = proc.GetTable();
            if (ds != null && ds.Rows.Count > 0)
            {
                branches = ds.Rows[0]["BranchId"].ToString();
            }
            return branches;
        }

        public class ServiceEntryHistory
        {
            public String EntityCode { get; set; }
            public String ReceiptNo { get; set; }
            public String ServiceAction { get; set; }
            public String Remarks { get; set; }
            public String Billable { get; set; }
        }

        public class ServiceEntryCount
        {
            public String Repaired { get; set; }
            public String Exchanged { get; set; }
        }

        protected void GrdDevice_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            if (Session["DeviceDetails"] != null)
            {
                GrdDevice.DataBind();
            }
        }

        protected void grid_DataBinding(object sender, EventArgs e)
        {
            if (Session["DeviceDetails"] != null)
            {
                GrdDevice.DataSource = (DataTable)Session["DeviceDetails"];
            }
        }

        [WebMethod]
        public static String AddData(DeviceDetails model)
        {
            DataTable dt = (DataTable)HttpContext.Current.Session["DeviceDetails"];
            DataTable dt2 = new DataTable();


            DataTable dtableCompValidation = new DataTable();
            dtableCompValidation.Clear();
            dtableCompValidation.Columns.Add("HIddenID", typeof(System.Guid));
            dtableCompValidation.Columns.Add("SerialNumber", typeof(System.String));
            dtableCompValidation.Columns.Add("ComponentId", typeof(System.String));
            dtableCompValidation.Columns.Add("Qty", typeof(System.String));

            int Count = 1;
            foreach (JobComponentKeyValue item in model.com_qty)
            {
                dtableCompValidation.Rows.Add(new Guid(), Count, item.id, item.Value);
                Count = Count + 1;
            }



            String strMsg = GetStockUpdate(model.DetailsID, model.JobsheetID, "LineLevelAdd", dtableCompValidation, model.TechnicianID);
            String[] str = strMsg.Split('~');
            if (dt == null)
            {
                DataTable dtable = new DataTable();

                dtable.Clear();
                dtable.Columns.Add("HIddenID", typeof(System.Guid));
                dtable.Columns.Add("SerialNumber", typeof(System.String));
                dtable.Columns.Add("DeviceType", typeof(System.String));
                dtable.Columns.Add("DeviceTypeID", typeof(System.String));
                dtable.Columns.Add("Model", typeof(System.String));
                dtable.Columns.Add("Problem", typeof(System.String));
                dtable.Columns.Add("ProblemID", typeof(System.String));
                dtable.Columns.Add("Other", typeof(System.String));
                dtable.Columns.Add("ServiceAction", typeof(System.String));
                dtable.Columns.Add("ServiceActionID", typeof(System.String));
                dtable.Columns.Add("Components", typeof(System.String));
                dtable.Columns.Add("ComponentsID", typeof(System.String));
                dtable.Columns.Add("Warranty", typeof(System.String));
                dtable.Columns.Add("ReturnReason", typeof(System.String));
                dtable.Columns.Add("ReturnReasonID", typeof(System.String));
                dtable.Columns.Add("Remarks", typeof(System.String));
                dtable.Columns.Add("Billable", typeof(System.String));
                dtable.Columns.Add("DetailsID", typeof(System.String));

                DataTable dtableComp = new DataTable();
                dtableComp.Clear();
                dtableComp.Columns.Add("HIddenID", typeof(System.Guid));
                dtableComp.Columns.Add("SerialNumber", typeof(System.String));
                dtableComp.Columns.Add("ComponentId", typeof(System.String));
                dtableComp.Columns.Add("Qty", typeof(System.String));

                if (str[1].ToString() == "Success")
                {
                    System.Guid gid = Guid.NewGuid();
                    object[] trow = { gid, model.SerialNumber, model.DeviceType, model.DeviceTypeID, model.Model, model.Problem, model.ProblemID, model.Other, model.ServiceAction,
                                    model.ServiceActionID, model.Components, model.ComponentsID,model.Warranty,model.ReturnReason,model.ReturnReasonID,model.Remarks,model.Billable,model.DetailsID };// Add new parameter Here
                    dtable.Rows.Add(trow);


                    if (model.com_qty != null && model.com_qty.Count > 0)
                    {
                        foreach (var s2 in model.com_qty)
                        {
                            if (s2.id.ToString()!="")
                            {
                                object[] trow1 = { gid, model.SerialNumber, s2.id.ToString(), s2.Value.ToString() };
                                dtableComp.Rows.Add(trow1);
                            }
                           
                        }
                    }

                    DataTable dtComp2 = null;
                    dtComp2 = dtableComp;
                    string[] data = model.ComponentsID.Split(',');
                    for (int i = 0; i < data.Length; i++)
                    {
                        if (dtableComp != null && dtableComp.Rows.Count > 0)
                        {
                            var row = dtComp2.AsEnumerable().FirstOrDefault(r => r.Field<string>("ComponentId") == data[i]);
                            if (row == null)
                            {
                                if (data[i].ToString() != "")
                                {
                                    object[] trow2 = { gid, model.SerialNumber, data[i], "0" };
                                    dtableComp.Rows.Add(trow2);
                                }
                            }
                        }
                    }
                }

                HttpContext.Current.Session["ComponentDetails"] = dtableComp;

                HttpContext.Current.Session["DeviceDetails"] = dtable;
            }
            else
            {
                if (string.IsNullOrEmpty(model.Guid))
                {
                    if (dt.Rows.Count < 10)
                    {
                        dt2 = null;
                        dt2 = dt;
                        //DataView dv = new DataView(dt2);
                        //dv.RowFilter = "DeviceNumber=" +Convert.ToString(DeviceNumber.ToString());
                        var row = dt2.AsEnumerable().FirstOrDefault(r => r.Field<string>("SerialNumber") == model.SerialNumber);
                        if (row == null)
                        {
                            if (str[1].ToString() == "Success")
                            {
                                System.Guid gid = Guid.NewGuid();
                                object[] trow = { gid,model.SerialNumber, model.DeviceType, model.DeviceTypeID, model.Model, model.Problem, model.ProblemID, model.Other, model.ServiceAction,
                                    model.ServiceActionID, model.Components, model.ComponentsID,model.Warranty,model.ReturnReason,model.ReturnReasonID,model.Remarks,model.Billable,model.DetailsID };// Add new parameter Here
                                dt.Rows.Add(trow);

                                DataTable dtableComp = (DataTable)HttpContext.Current.Session["ComponentDetails"];

                                if (model.com_qty != null && model.com_qty.Count > 0)
                                {
                                    foreach (var s2 in model.com_qty)
                                    {
                                        object[] trow1 = { gid, model.SerialNumber, s2.id.ToString(), s2.Value.ToString() };
                                        dtableComp.Rows.Add(trow1);
                                    }
                                }

                                DataTable dtComp2 = null;
                                dtComp2 = dtableComp;
                                string[] data = model.ComponentsID.Split(',');
                                for (int i = 0; i < data.Length; i++)
                                {
                                    if (dtableComp != null && dtableComp.Rows.Count > 0)
                                    {
                                        //var row1 = dtComp2.AsEnumerable().FirstOrDefault(r => r.Field<string>("ComponentId") == data[i]);
                                        //if (row1 == null)
                                        //{
                                        //    object[] trow1 = { gid, model.SerialNumber, data[i], "0" };
                                        //    dtableComp.Rows.Add(trow1);
                                        //}
                                        if (data[i] != "")
                                        {
                                            if (model.com_qty != null && model.com_qty.Count > 0)
                                            {
                                                var row1 = model.com_qty.AsEnumerable().FirstOrDefault(r => r.id == data[i]);
                                                if (row1 == null)
                                                {
                                                    object[] trow1 = { gid, model.SerialNumber, data[i], "0" };
                                                    dtableComp.Rows.Add(trow1);
                                                }
                                            }
                                            else
                                            {
                                                object[] trow1 = { gid, model.SerialNumber, data[i], "0" };
                                                dtableComp.Rows.Add(trow1);
                                            }
                                        }
                                    }
                                }
                                HttpContext.Current.Session["ComponentDetails"] = dtableComp;
                            }
                        }
                        else
                        {
                            return "Serial number already exists.~Error";
                        }
                    }
                    else
                    {
                        return "You can't add more device.~Error";
                    }
                }
                else
                {
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow item in dt.Rows)
                        {
                            if (model.Guid.ToString() == item["HIddenID"].ToString())
                            {
                                if (str[1].ToString() == "Success")
                                {
                                    item["SerialNumber"] = model.SerialNumber;
                                    item["DeviceType"] = model.DeviceType;
                                    item["DeviceTypeID"] = model.DeviceTypeID;
                                    item["Model"] = model.Model;
                                    item["Problem"] = model.Problem;
                                    item["ProblemID"] = model.ProblemID;
                                    item["Other"] = model.Other;
                                    item["ServiceAction"] = model.ServiceAction;
                                    item["ServiceActionID"] = model.ServiceActionID;
                                    item["Components"] = model.Components;
                                    item["ComponentsID"] = model.ComponentsID;
                                    item["Warranty"] = model.Warranty;
                                    item["ReturnReason"] = model.ReturnReason;
                                    item["ReturnReasonID"] = model.ReturnReasonID;
                                    item["Remarks"] = model.Remarks;
                                    item["Billable"] = model.Billable;
                                    item["DetailsID"] = model.DetailsID;
                                }
                            }
                        }

                        DataTable dtableComp = (DataTable)HttpContext.Current.Session["ComponentDetails"];
                        if (dtableComp != null && dtableComp.Rows.Count > 0)
                        {
                            DataTable dtcomp = new DataTable();
                            dtcomp = dtableComp.Copy();
                            //foreach (DataRow item in dtcomp.Rows)
                            //{
                            //    if (model.Guid.ToString() == item["HIddenID"].ToString())
                            //    {
                            //        item.Delete();
                            //    }
                            //    dtcomp.AcceptChanges();
                            //}

                            for (int i = dtableComp.Rows.Count - 1; i >= 0; i--)
                            {
                                DataRow dr = dtableComp.Rows[i];
                                string HIddenID = Convert.ToString(dr["HIddenID"]);

                                if (HIddenID == model.Guid.ToString())
                                    dr.Delete();
                            }
                            dtableComp.AcceptChanges();

                            //dtableComp.Rows.Cast<DataRow>().Where(r => r.ItemArray[1] == model.SerialNumber).ToList().ForEach(r => r.Delete());
                            //dtableComp = dtcomp.AsEnumerable().Where(r => r.Field<string>("SerialNumber") != model.SerialNumber).CopyToDataTable();

                            if (model.com_qty != null && model.com_qty.Count > 0)
                            {
                                foreach (var s2 in model.com_qty)
                                {
                                    object[] trow1 = { model.Guid.ToString(), model.SerialNumber, s2.id.ToString(), s2.Value.ToString() };
                                    dtableComp.Rows.Add(trow1);
                                }
                            }

                            DataTable dtComp2 = null;
                            dtComp2 = dtableComp;
                            string[] data = model.ComponentsID.Split(',');
                            for (int i = 0; i < data.Length; i++)
                            {
                                if (dtableComp != null && dtableComp.Rows.Count > 0)
                                {
                                    //var row1 = dtComp2.AsEnumerable().FirstOrDefault(r => r.Field<string>("ComponentId") == data[i]);
                                    if (data[i] != "")
                                    {
                                        if (model.com_qty != null && model.com_qty.Count > 0)
                                        {
                                            var row1 = model.com_qty.AsEnumerable().FirstOrDefault(r => r.id == data[i]);
                                            if (row1 == null)
                                            {
                                                object[] trow1 = { model.Guid.ToString(), model.SerialNumber, data[i], "0" };
                                                dtableComp.Rows.Add(trow1);
                                            }
                                        }
                                        else
                                        {
                                            object[] trow1 = { model.Guid.ToString(), model.SerialNumber, data[i], "0" };
                                            dtableComp.Rows.Add(trow1);
                                        }
                                    }
                                }
                            }
                            HttpContext.Current.Session["ComponentDetails"] = dtableComp;
                        }
                    }
                }
                HttpContext.Current.Session["DeviceDetails"] = dt;
            }

            return str[0].ToString() + "~" + str[1].ToString();// "Device Added Successfully.";
        }

        [WebMethod]
        public static DeviceDetails EditData(String HiddenID)
        {
            DataTable dt = (DataTable)HttpContext.Current.Session["DeviceDetails"];
            DeviceDetails ret = new DeviceDetails();
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow item in dt.Rows)
                {
                    if (HiddenID.ToString() == item["HIddenID"].ToString())
                    {
                        ret.Guid = item["HIddenID"].ToString();
                        ret.SerialNumber = item["SerialNumber"].ToString();
                        ret.DeviceType = item["DeviceType"].ToString();
                        ret.DeviceTypeID = item["DeviceTypeID"].ToString();
                        ret.Model = item["Model"].ToString();
                        ret.Problem = item["Problem"].ToString();
                        ret.ProblemID = item["ProblemID"].ToString();
                        ret.Other = item["Other"].ToString();
                        ret.ServiceAction = item["ServiceAction"].ToString();
                        ret.ServiceActionID = item["ServiceActionID"].ToString();
                        ret.Components = item["Components"].ToString();
                        ret.ComponentsID = item["ComponentsID"].ToString();
                        ret.Warranty = item["Warranty"].ToString();
                        ret.ReturnReason = item["ReturnReason"].ToString();
                        ret.ReturnReasonID = item["ReturnReasonID"].ToString();
                        ret.Remarks = item["Remarks"].ToString();
                        ret.Billable = item["Billable"].ToString();
                        ret.DetailsID = item["DetailsID"].ToString();
                        break;
                    }
                }
            }
            return ret;// "Holiday Remove Sucessfylly";
        }

        [WebMethod]
        public static String DeleteData(string HiddenID, String JobSheetID, String TechnicianID)
        {
            String MSG = "";
            String Status = "";
            DataTable dt = (DataTable)HttpContext.Current.Session["DeviceDetails"];
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow item in dt.Rows)
                {
                    if (HiddenID.ToString() == item["HIddenID"].ToString())
                    {
                        String strMsg = GetStockUpdate(item["DetailsID"].ToString(), JobSheetID, "LineLevelDelete", null, TechnicianID);
                        String[] str = strMsg.Split('~');
                        MSG = str[0].ToString() + "~" + str[1].ToString();
                        Status = str[1].ToString();
                        if (str[1].ToString() == "Success")
                        {
                            dt.Rows.Remove(item);
                        }
                        break;
                    }
                }

                if (Status == "Success")
                {
                    DataTable dtableComp = (DataTable)HttpContext.Current.Session["ComponentDetails"];
                    if (dtableComp != null && dtableComp.Rows.Count > 0)
                    {
                        for (int i = dtableComp.Rows.Count - 1; i >= 0; i--)
                        {
                            DataRow dr = dtableComp.Rows[i];
                            string HIddenID = Convert.ToString(dr["HIddenID"]);

                            if (HIddenID == HiddenID.ToString())
                                dr.Delete();
                        }
                        dtableComp.AcceptChanges();
                    }

                    HttpContext.Current.Session["ComponentDetails"] = dtableComp;
                }

            }
            return MSG;// "Device Remove Successfully.";
        }

        public static string GetStockUpdate(String DetailsID, String JobsheetID, String Action, DataTable Component, String TechnicianID)
        {
            MasterSettings masterbl = new MasterSettings();
            string mastersettings = masterbl.GetSettings("StkAdjSrv");
            String Returan_msg = "";
            int user_id = Convert.ToInt32(HttpContext.Current.Session["userid"]);
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("PRC_SRVJobSheetDetails");
            proc.AddVarcharPara("@Action", 500, Action);
            proc.AddPara("@StockAdj_Require", mastersettings);
            proc.AddPara("@JobsheetDetID", DetailsID);
            proc.AddPara("@JobsheetID", JobsheetID);
            proc.AddPara("@udt_JobSheetComponentDetails", Component);
            proc.AddPara("@TechnicianID", TechnicianID);
            dt = proc.GetTable();

            if (dt != null && dt.Rows.Count > 0)
            {
                Returan_msg = dt.Rows[0]["msg"].ToString() + "~" + dt.Rows[0]["Status"].ToString();
            }
            return Returan_msg;
        }

        public class DeviceDetails
        {
            public String Guid { get; set; }
            public String SerialNumber { get; set; }
            public String DeviceType { get; set; }
            public String DeviceTypeID { get; set; }
            public String Model { get; set; }
            public String Problem { get; set; }
            public String ProblemID { get; set; }
            public String Other { get; set; }
            public String ServiceAction { get; set; }
            public String ServiceActionID { get; set; }
            public String Components { get; set; }
            public String ComponentsID { get; set; }
            public String Warranty { get; set; }
            public String ReturnReason { get; set; }
            public String ReturnReasonID { get; set; }
            public String Remarks { get; set; }
            public String Billable { get; set; }
            public String JobsheetID { get; set; }
            public String DetailsID { get; set; }
            public String TechnicianID { get; set; }
            public List<JobComponentKeyValue> com_qty { get; set; }
        }

        public class JobComponentKeyValue
        {
            public String id { get; set; }
            public String Value { get; set; }
        }

        [WebMethod]
        public static ServiceRegisterReports SingleAssignTechnician(String BranchID)
        {
            SrvAssignJobBL obj = new SrvAssignJobBL();
            ServiceRegisterReports reports = new ServiceRegisterReports();
            List<Tecchnician> TecchnicianList = new List<Tecchnician>();
            DataTable dt = obj.GetAssignJobDetails(BranchID);
            if (dt != null && dt.Rows.Count > 0)
            {
                TecchnicianList = APIHelperMethods.ToModelList<Tecchnician>(dt);
                reports.TecchnicianList = TecchnicianList;
            }
            return reports;
        }

        [WebMethod]
        public static List<Srv_ServiceComponentList> ShowComponentQty(String ComponentID, String hiddenid, String serialno, String EntryMode)
        {

            DataTable dtableComp = (DataTable)HttpContext.Current.Session["ComponentDetails"];

            int user_id = Convert.ToInt32(HttpContext.Current.Session["userid"]);
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("PRC_SRVJobSheetDetails");
            if (hiddenid == "")
            {
                proc.AddVarcharPara("@ACTION", 500, "ComponentListAdd");
            }
            else if (EntryMode != "")
            {
                proc.AddVarcharPara("@ACTION", 500, "ComponentListEdit");
            }
            proc.AddPara("@ComponentID", ComponentID);
            proc.AddPara("@udt_JobSheetComponentDetails", dtableComp);
            proc.AddPara("@guid", hiddenid);
            dt = proc.GetTable();

            List<Srv_ServiceComponentList> listStatues = new List<Srv_ServiceComponentList>();
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow item in dt.Rows)
                {
                    listStatues.Add(new Srv_ServiceComponentList
                    {
                        ProductCode = item["ProductCode"].ToString(),
                        ProductName = item["ProductName"].ToString(),
                        Replaceable = item["Replaceable"].ToString(),
                        TEXTBOX = item["TEXTBOX"].ToString(),
                        Productid = item["ID"].ToString()
                    });
                }
            }
            return listStatues;
        }

        // Mantis Issue 24412
        [WebMethod]
        public static string GetNetworkName(String EntityCode)
        {
            String NetworkName = "";
            int user_id = Convert.ToInt32(HttpContext.Current.Session["userid"]);
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("PRC_SRVWarrantyUpdateFetch");
            proc.AddVarcharPara("@ACTION", 500, "GetNetworkName");
            proc.AddPara("@EntityCode", EntityCode);
            dt = proc.GetTable();

            if (dt != null && dt.Rows.Count > 0)
            {
                NetworkName = dt.Rows[0]["NetworkName"].ToString();
            }
            return NetworkName;
        }
        // End of Mantis Issue 24412

    }
}