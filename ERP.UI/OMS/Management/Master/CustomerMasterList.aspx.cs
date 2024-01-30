/***********************************************************************************************************
 * 1.0    02-01-2024       V2.0.42     Sanchita     Settings required to Check Duplicate Customer Master Name. Mantis: 27125
 * 2.0    30-01-2024       V2.0.43     Sanchita     27208: Customer Industry Mapping - features required through Import facility.
 **************************************************************************************************************/
using System;
using System.Data;
using System.Web;
using DevExpress.Web;
using BusinessLogicLayer;
using EntityLayer.CommonELS;
using ClsDropDownlistNameSpace;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using DevExpress.XtraPrinting;
using System.Net.Mime;
using ERP.Models;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Services;
using DataAccessLayer;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Text.RegularExpressions;
using System.Xml;

namespace ERP.OMS.Management.Master
{
    public partial class CustomerMasterList : System.Web.UI.Page
    {
        int NoOfRow = 0;
        string AllUserCntId;
        string cBranchId = "";
        public string pageAccess = "";
        static string Checking = null;
        public static string IsLighterCustomePage = string.Empty;
        clsDropDownList oclsDropDownList = new clsDropDownList();
        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);MULTI
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        public static EntityLayer.CommonELS.UserRightsForPage rights;
        public string ShowSegment { get; set; }
        List<string> relationship = new List<string>();
        string ContType;
        private static String path, path1, FileName, s, time, cannotParse;
        string FilePath = "";
        public string[] InputName = new string[20];
        public string[] InputType = new string[20];
        public string[] InputValue = new string[20];
        public string[] InputName1 = new string[20];
        public string[] InputType1 = new string[20];
        public string[] InputValue1 = new string[20];
        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //'http://localhost:2957/InfluxCRM/management/testProjectMainPage_employee.aspx'

                string sPath = Convert.ToString(HttpContext.Current.Request.Url);
                oDBEngine.Call_CheckPageaccessebility(sPath);
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {

            CommonBL ComBL = new CommonBL();
            MasterSettings objmaster = new MasterSettings();
            string UniqueAutoNumberCustomerMaster = ComBL.GetSystemSettingsResult("UniqueAutoNumberCustomerMaster");
            string DuplicateGSTINCustomer = ComBL.GetSystemSettingsResult("DuplicateGSTINCustomer");
            hdnDocumentSegmentSettings.Value = objmaster.GetSettings("DocumentSegment");
            ShowSegment = hdnDocumentSegmentSettings.Value;
            if (hdnDocumentSegmentSettings.Value == "0")
            {
               // ImportSegment.Style.Add("display", "none");   
            }
            else
            {
                //ImportSegment.Style.Add("display", "block");
            }
            
            if (Session["ExchangeSegmentID"] == null)
            {
                TxtSeg.Value = "N";
            }
            else
            {
                TxtSeg.Value = "Y";
            }

            if (!Page.IsPostBack)
            {
                EmployeeDataSource.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                rights = new UserRightsForPage();

                rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/master/CustomerMasterList.aspx");
                //rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/master/frmContactMain.aspx?requesttype=customer");
                ContType = "customer";
                hidIsLigherContactPage.Value = "0";
                IsLighterCustomePage = "";

                Session["exportval"] = null;
              
                Session["Contactrequesttype"] = "customer";
                Session["requesttype"] = "Customer/Client";


                //Chinmoy added start 02-04-2020	
                if (!String.IsNullOrEmpty(UniqueAutoNumberCustomerMaster))
                {
                    if (UniqueAutoNumberCustomerMaster == "Yes")
                    {
                        hdnAutoNumStg.Value = "1";
                        hdnTransactionType.Value = "CL";
                        dvIdType.Style.Add("display", "none");
                        dvUniqueId.Style.Add("display", "none");
                        ddl_Num.Style.Add("display", "block");
                        dvCustDocNo.Style.Add("display", "block");
                        NumberingSchemeBind();
                    }
                    else if (UniqueAutoNumberCustomerMaster.ToUpper().Trim() == "NO")
                    {
                        hdnAutoNumStg.Value = "0";
                        hdnTransactionType.Value = "";
                        dvIdType.Style.Add("display", "block");
                        dvUniqueId.Style.Add("display", "block");
                        //ddl_Num.Visible = false;	
                        //dvCustDocNo.Visible = false;	
                    }
                }
                //End

                if (!String.IsNullOrEmpty(DuplicateGSTINCustomer))
                {
                    if (DuplicateGSTINCustomer == "Yes")
                    {
                        hdnDuplicateGSTN.Value = "Yes";                        
                    }
                    else if (DuplicateGSTINCustomer.ToUpper().Trim() == "NO")
                    {
                        hdnDuplicateGSTN.Value = "No";                        
                    }
                }

                //        ddl_Num.Style.Add("display", "block");
                //        dvCustDocNo.Style.Add("display", "block");
                //        NumberingSchemeBind();
                //    }
                //    else if (UniqueAutoNumberCustomerMaster.ToUpper().Trim() == "NO")
                //    {
                //        hdnAutoNumStg.Value = "0";
                //        hdnTransactionType.Value = "";

                //        dvIdType.Style.Add("display", "block");
                //        dvUniqueId.Style.Add("display", "block");

                //        //ddl_Num.Visible = false;	
                //        //dvCustDocNo.Visible = false;	
                //    }
                //}
                //End

                CommonBL cbl = new CommonBL();
                string ISLigherpage = cbl.GetSystemSettingsResult("LighterCustomerEntryPage");
                if (!String.IsNullOrEmpty(ISLigherpage))
                {
                    if (ISLigherpage == "Yes")
                    {
                        hidIsLigherContactPage.Value = "1";
                        IsLighterCustomePage = "1";
                    }
                }

                //-----------------------Subhra 16-05-2019--------------------------------
              
                string[,] DataAssignTo = oDBEngine.GetFieldValue("tbl_master_user", "user_id, user_name", "user_inactive='N'", 2, "user_name");
                oclsDropDownList.AddDataToDropDownList(DataAssignTo, cmbAssignTo);

              

                //-------------------------------------------------------------------------------
            }
        }
      
        protected void EntityServerModeDataSource_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "cnt_Id";
            //string connectionString = ConfigurationManager.ConnectionStrings["crmConnectionString"].ConnectionString; MULTI
            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);

            var q = from d in dc.v_CustomerMasterLists
                    orderby d.cnt_Id descending                   
                    select d;
            e.QueryableSource = q;
        }

        //chinmoy added start 02-04-2020	
        public void NumberingSchemeBind()
        {
            string userbranchHierarchy = Convert.ToString(Session["userbranchHierarchy"]);
            string actionqry = "GetAllDropDownDetailForCustomerMaster";
            DataTable Schemadt = GetAllDropDownDetailForCustomerMaster(userbranchHierarchy, actionqry);
            if (Schemadt != null && Schemadt.Rows.Count > 0)
            {
                ddl_numberingScheme.DataTextField = "SchemaName";
                ddl_numberingScheme.DataValueField = "Id";
                ddl_numberingScheme.DataSource = Schemadt;
                ddl_numberingScheme.DataBind();
            }
        }
        public DataTable GetAllDropDownDetailForCustomerMaster(string UserBranch, string Qry)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_SalesActivity");
            proc.AddVarcharPara("@Action", 100, Qry);
            proc.AddVarcharPara("@userbranchlist", 4000, UserBranch);
            ds = proc.GetTable();
            return ds;
        }
        //End
        public void bindexport(int Filter)
        {
            EmployeeGrid.Columns[7].Visible = false;
            string filename = "Customer";
            exporter.FileName = filename;

            exporter.PageHeader.Left = filename;
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
                case 5:
                    exporter.WriteXlsxToResponse();
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
        protected void EmployeeGrid_CustomJSProperties(object sender, ASPxGridViewClientJSPropertiesEventArgs e)
        {
            e.Properties["cpInsertError"] = "ADD";
        }

        //Rev Rajdip
        [WebMethod]
        //rev srijeeta mantis issue 0024515
        //public static object SaveCustomerMaster(string sex, string dob, string hdnanniversarydate, string IsAllCopy, string oldcustomerid, string UniqueID, string Name, string BillingAddress1, 
        //    string BillingAddress2, string BillingPin, string shippingAddress1, string shippingAddress2, string shippingPin, string GSTIN, string BillingPhone, string ShippingPhone,
        //    string contactperson, string IdTypeval, string GrpCust = null, int NumberingId = 0, bool TCSApplicable=false)
        public static object SaveCustomerMaster(string sex, string dob, string hdnanniversarydate, string IsAllCopy, string oldcustomerid, string UniqueID,string Alternative_Code,  string Name, string BillingAddress1,
            string BillingAddress2, string BillingPin, string shippingAddress1, string shippingAddress2, string shippingPin, string GSTIN, string BillingPhone, string ShippingPhone,
            string contactperson, string IdTypeval, string GrpCust = null, int NumberingId = 0, bool TCSApplicable = false)
        //end of rev srijeeta mantis issue 0024515
        {
            BusinessLogicLayer.Contact oContactGeneralBL = new BusinessLogicLayer.Contact();
            MShortNameCheckingBL mshort = new MShortNameCheckingBL();
            //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);MULTI
            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
            CommonBL ComBL = new CommonBL();
            DateTime CreateDate = Convert.ToDateTime(oDBEngine.GetDate().ToShortDateString());
            try
            {
                string entityName = "";
                if (GSTIN != "")
                {
                    string DuplicateGSTINCustomer = ComBL.GetSystemSettingsResult("DuplicateGSTINCustomer");
                    if (!String.IsNullOrEmpty(DuplicateGSTINCustomer))
                    {
                        if (DuplicateGSTINCustomer == "No")
                        {
                            if (ContactGeneralBL.ISUniqueGSTIN("", "0", GSTIN, "CL"))
                            {
                                return new { status = "DuplicateGSTIN", Msg = "Duplicate GSTIN" };
                            }
                        }
                    }
                }

                if (!mshort.CheckUniqueWithtypeContactMaster(UniqueID, "", "MasterContactType", "CL", ref entityName))
                {
                    //rev srijeeta mantis issue 0024515
                    //string InternalId = oContactGeneralBL.Insert_ContactGeneralCopyToProduct(oldcustomerid,"Customer/Client", UniqueID, "1", //4	

                    //                                Name, "", "",//3	
                    //                                "", Convert.ToString(HttpContext.Current.Session["userbranchID"]), "2",//3	
                    //                                "0", new DateTime(1900, 1, 1, 0, 0, 0, 0), new DateTime(1900, 1, 1, 0, 0, 0, 0), "1",//4	
                    //                                "1", "1", "",//3	
                    //                                "0", "0", "0",//3	
                    //                                "1", "", "", "CL",//4	
                    //                                "1", DateTime.Now, "1", "",//4	
                    //                                "1", "No", "", Convert.ToString(HttpContext.Current.Session["userid"]), "",//5	
                    //                                DateTime.Now, "", "78", false, 0,//5	
                    //                                0, "A",//2	
                    //                                "", "", "", "", "", NumberingId, TCSApplicable);//6	
                    string InternalId = oContactGeneralBL.Insert_ContactGeneralCopyToProduct(oldcustomerid, "Customer/Client", UniqueID, Alternative_Code, "1", //4	

                                                    Name, "", "",//3	
                                                    "", Convert.ToString(HttpContext.Current.Session["userbranchID"]), "2",//3	
                                                    "0", new DateTime(1900, 1, 1, 0, 0, 0, 0), new DateTime(1900, 1, 1, 0, 0, 0, 0), "1",//4	
                                                    "1", "1", "",//3	
                                                    "0", "0", "0",//3	
                                                    "1", "", "", "CL",//4	
                                                    "1", DateTime.Now, "1", "",//4	
                                                    "1", "No", "", Convert.ToString(HttpContext.Current.Session["userid"]), "",//5	
                                                    DateTime.Now, "", "78", false, 0,//5	
                                                    0, "A",//2	
                                                    "", "", "", "", "", NumberingId, TCSApplicable);//6	
                    //end of rev srijeeta mantis issue 0024515
                    DataTable dts = new DataTable();	
                    dts = oDBEngine.GetDataTable("select isnull(cnt_UCC,'') cnt_UCC from tbl_master_contact where cnt_internalId='" + InternalId + "'");	
                    if (dts.Rows.Count == 1)	
                    {	
                        if (Convert.ToString(dts.Rows[0]["cnt_UCC"]) == "Auto")	
                        {	
                            ProcedureExecute pr = new ProcedureExecute("Prc_DeleteContactInsert");	
                            pr.AddVarcharPara("@InternalId", 200, InternalId);	
                            pr.RunActionQuery();	
                        }	
                    }	
                    if (dts.Rows.Count == 1 && Convert.ToString(dts.Rows[0]["cnt_UCC"]) != "Auto")	
                    {

                    ProcedureExecute proc = new ProcedureExecute("prc_GetSetCustomerPopup_Lightwet");
                    proc.AddVarcharPara("@action", 500, "SaveBillingShipping");
                    proc.AddVarcharPara("@pin", 10, BillingPin);
                    proc.AddVarcharPara("@billingAddress1", 100, BillingAddress1);
                    proc.AddVarcharPara("@billingAddress2", 100, BillingAddress2);
                    proc.AddVarcharPara("@shippingpin", 10, shippingPin);
                    proc.AddVarcharPara("@shippingbillingAddress1", 100, shippingAddress1);
                    proc.AddVarcharPara("@shippingbillingAddress2", 100, shippingAddress2);
                    proc.AddVarcharPara("@customerInternalId", 10, InternalId);
                    proc.AddVarcharPara("@cntUcc", 50, UniqueID);
                    //rev srijeeta mantis issue 0024515
                    proc.AddVarcharPara("@Alternative_Code", 100, Alternative_Code);
                    //end of rev srijeeta mantis issue 0024515
                    proc.AddVarcharPara("@GSTIN", 20, GSTIN);
                    proc.AddVarcharPara("@BillingPhone", 20, BillingPhone);
                    proc.AddVarcharPara("@ShippingPhone", 20, ShippingPhone);
                    proc.AddVarcharPara("@contactperson", 40, contactperson);

                    proc.AddIntegerPara("@cnt_IdType", Convert.ToInt32(IdTypeval));
                   // proc.AddBooleanPara("@TCSApplicable", Convert.ToBoolean(TCSApplicable));
                    proc.AddIntegerNullPara("@BillCountry", QueryParameterDirection.Output);
                    proc.AddIntegerNullPara("@BillState", QueryParameterDirection.Output);
                    proc.AddIntegerNullPara("@BillCity", QueryParameterDirection.Output);
                    proc.AddIntegerNullPara("@Billpin", QueryParameterDirection.Output);

                    proc.AddIntegerNullPara("@ShipCountry", QueryParameterDirection.Output);
                    proc.AddIntegerNullPara("@ShipState", QueryParameterDirection.Output);
                    proc.AddIntegerNullPara("@ShipCity", QueryParameterDirection.Output);
                    proc.AddIntegerNullPara("@Shippin", QueryParameterDirection.Output);


                    proc.RunActionQuery();



                    int billCountry = Convert.ToInt32(proc.GetParaValue("@BillCountry"));
                    int billState = Convert.ToInt32(proc.GetParaValue("@BillState"));
                    int billcity = Convert.ToInt32(proc.GetParaValue("@BillCity"));
                    int billpin = Convert.ToInt32(proc.GetParaValue("@Billpin"));


                    int ShipCountry = Convert.ToInt32(proc.GetParaValue("@ShipCountry"));
                    int ShipState = Convert.ToInt32(proc.GetParaValue("@ShipState"));
                    int Shipcity = Convert.ToInt32(proc.GetParaValue("@ShipCity"));
                    int Shippin = Convert.ToInt32(proc.GetParaValue("@Shippin"));



                    DataTable StateDetails = oDBEngine.GetDataTable("select state+' (State Code:'+StateCode+')' stateText,id   from tbl_master_state where id=" + billState + " union all select state+' (State Code:'+StateCode+')' stateText,id   from tbl_master_state where id=" + ShipState);
                    string BillingStateText = "", BillingStateCode = "", ShippingStateText = "", ShippingStateCode = "";
                    if (StateDetails.Rows.Count > 0)
                    {
                        BillingStateText = Convert.ToString(StateDetails.Rows[0]["stateText"]);
                        BillingStateCode = Convert.ToString(StateDetails.Rows[0]["id"]);

                        ShippingStateText = Convert.ToString(StateDetails.Rows[1]["stateText"]);
                        ShippingStateCode = Convert.ToString(StateDetails.Rows[1]["id"]);
                    }
                    //Rev Rajdip
                    DataTable IsBankdetailsexist = oDBEngine.GetDataTable("select * from tbl_trans_contactBankDetails where cbd_cntId IN(select cnt_internalId from tbl_master_contact where cnt_id='"+oldcustomerid+"')");
                    if (IsBankdetailsexist.Rows.Count > 0 && IsAllCopy=="1")
                    {
                        ProcedureExecute procBankdetailsinsertofcopyproduct = new ProcedureExecute("prc_Bankdetailsinsertofcopyproduct");
                        procBankdetailsinsertofcopyproduct.AddVarcharPara("@cnt_id", 100, oldcustomerid);
                        procBankdetailsinsertofcopyproduct.AddVarcharPara("@UNIQUEID", 100, UniqueID);
                        procBankdetailsinsertofcopyproduct.RunActionQuery();
                    }
                   
                    
                    
                    //End Rev Rajdip
                    if (GrpCust != "0")
                    {
                        int a = oDBEngine.ExeInteger("insert into tbl_trans_group(grp_contactId,grp_groupMaster,grp_groupType,CreateDate,CreateUser) values('" + InternalId + "','" + GrpCust + "','Customers','" + CreateDate + "','" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + "')");
                    }

                    return new { status = "Ok", InternalId = InternalId, BillingStateText = BillingStateText, BillingStateCode = BillingStateCode, ShippingStateText = ShippingStateText, ShippingStateCode = ShippingStateCode };
                    }
                    else
                    {
                        return new { status = "AUTOError" };
                    }                
                }

                else
                {
                    return new { status = "Error", Msg = "Cutomer Unique Id Already Exists" };
                }

            }
            catch (Exception ex)
            {
                return new { status = "Error", Msg = ex.Message };
            }

        }

        //End Rev Rajdip
        protected void EmployeeGrid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            EmployeeGrid.JSProperties["cpDelete"] = null;
            string WhichCall = Convert.ToString(e.Parameters).Split('~')[0];
            string WhichType = null;
            int deletecnt = 0;
            if (Convert.ToString(e.Parameters).Contains("~"))
            {
                if (Convert.ToString(e.Parameters).Split('~')[1] != "")
                {
                    WhichType = Convert.ToString(e.Parameters).Split('~')[1];

                }
                if (WhichCall == "Delete")
                {
                    MasterDataCheckingBL objMasterDataCheckingBL = new MasterDataCheckingBL();

                    deletecnt = objMasterDataCheckingBL.DeleteLeadOrContact(WhichType);
                    if (deletecnt > 0)
                    {
                        EmployeeGrid.JSProperties["cpDelete"] = "Success";
                       
                    }
                    else
                        EmployeeGrid.JSProperties["cpDelete"] = "Fail";
                }
            }
            EmployeeGrid.ClearSort();

            if (e.Parameters == "ssss")
            {

                EmployeeGrid.Settings.ShowFilterRow = true;

            }
          

        }
        protected void EmployeeGrid_HtmlRowCreated(object sender, ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType == GridViewRowType.Data)
            {
               
            }

        }
        protected void EmployeeGrid_DataBound(object sender, EventArgs e)
        {
        }
        //---------------Subhra 16-05-2019-------------------------------
        protected void PopupAssign_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            if (Convert.ToString(Session["Contactrequesttype"]) == "customer")
            {
                string strSplitCommand = e.Parameter.Split('~')[0];
                string entity_id = e.Parameter.Split('~')[1];


                if (strSplitCommand == "Load")
                {
                    string name = e.Parameter.Split('~')[2];
                    DataTable dtchkassign = new DataTable();
                    dtchkassign = oDBEngine.GetDataTable("select Assign_To,Assign_Remarks,CreateUser from tbl_master_contact where cnt_internalId='" + entity_id + "'");
                    if (dtchkassign.Rows[0][0].ToString() == "")
                    {
                        CallbackPanelAssign.JSProperties["cpName"] = name;
                        CallbackPanelAssign.JSProperties["cpAssignTo"] = "";
                        CallbackPanelAssign.JSProperties["cpRemarks"] = "";
                        CallbackPanelAssign.JSProperties["cpEnteredBy"] = Convert.ToInt32(dtchkassign.Rows[0][2]);
                        CallbackPanelAssign.JSProperties["cpAssignSave"] = "";
                    }
                    else
                    {
                        CallbackPanelAssign.JSProperties["cpName"] = name;
                        CallbackPanelAssign.JSProperties["cpAssignTo"] = dtchkassign.Rows[0][0].ToString();
                        CallbackPanelAssign.JSProperties["cpRemarks"] = dtchkassign.Rows[0][1].ToString();
                        CallbackPanelAssign.JSProperties["cpEnteredBy"] = 0;
                        CallbackPanelAssign.JSProperties["cpAssignSave"] = "";
                    }
                    //hdnBranchSelection.Value = dtBranchSelection.Rows[0][0].ToString();

                }
                else if (strSplitCommand == "Assign")
                {
                    int OutputId = 0;
                    DataSet dsInst = new DataSet();
                    SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                    SqlCommand cmd = new SqlCommand("PRC_LEAD_ASSIGN", con);
                    DataTable dtReceipt = new DataTable();

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@ACTION_TYPE", "ASSIGN");
                    cmd.Parameters.AddWithValue("@LEAD_ENTITY_ID", entity_id);
                    cmd.Parameters.AddWithValue("@LEAD_CONTACTTYPE", "LD");
                    cmd.Parameters.AddWithValue("@USERID", cmbAssignTo.SelectedItem.Value);
                    cmd.Parameters.AddWithValue("@ASSIGN_REMARKS", txtRemarks.Text);
                    cmd.Parameters.AddWithValue("@CREATED_BY", Convert.ToInt32(Session["userid"]));

                    SqlParameter output = new SqlParameter("@ReturnMessage", SqlDbType.VarChar, 500);
                    output.Direction = ParameterDirection.Output;

                    SqlParameter outputText = new SqlParameter("@ReturnCode", SqlDbType.VarChar, 20);
                    outputText.Direction = ParameterDirection.Output;


                    cmd.Parameters.Add(output);
                    cmd.Parameters.Add(outputText);

                    cmd.CommandTimeout = 0;
                    SqlDataAdapter Adap = new SqlDataAdapter();
                    Adap.SelectCommand = cmd;
                    Adap.Fill(dsInst);
                    cmd.Dispose();
                    con.Dispose();


                    OutputId = Convert.ToInt32(cmd.Parameters["@ReturnCode"].Value.ToString());

                    string strCPRID = Convert.ToString(cmd.Parameters["@ReturnMessage"].Value.ToString());
                    CallbackPanelAssign.JSProperties["cpAssignSave"] = "Save";
                }
                else if (strSplitCommand == "Unassign")
                {
                    int OutputId = 0;
                    DataSet dsInst = new DataSet();
                    SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                    SqlCommand cmd = new SqlCommand("PRC_LEAD_ASSIGN", con);
                    DataTable dtReceipt = new DataTable();

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@ACTION_TYPE", "UNASSIGN");
                    cmd.Parameters.AddWithValue("@LEAD_ENTITY_ID", entity_id);
                    cmd.Parameters.AddWithValue("@LEAD_CONTACTTYPE", "CL");
                    cmd.Parameters.AddWithValue("@USERID", cmbAssignTo.SelectedItem.Value);
                    cmd.Parameters.AddWithValue("@ASSIGN_REMARKS", txtRemarks.Text);
                    cmd.Parameters.AddWithValue("@CREATED_BY", Convert.ToInt32(Session["userid"]));


                    SqlParameter output = new SqlParameter("@ReturnMessage", SqlDbType.VarChar, 500);
                    output.Direction = ParameterDirection.Output;

                    SqlParameter outputText = new SqlParameter("@ReturnCode", SqlDbType.VarChar, 20);
                    outputText.Direction = ParameterDirection.Output;



                    cmd.Parameters.Add(output);
                    cmd.Parameters.Add(outputText);

                    cmd.CommandTimeout = 0;
                    SqlDataAdapter Adap = new SqlDataAdapter();
                    Adap.SelectCommand = cmd;
                    Adap.Fill(dsInst);
                    cmd.Dispose();
                    con.Dispose();


                    OutputId = Convert.ToInt32(cmd.Parameters["@ReturnCode"].Value.ToString());

                    string strCPRID = Convert.ToString(cmd.Parameters["@ReturnMessage"].Value.ToString());
                    CallbackPanelAssign.JSProperties["cpAssignSave"] = "Save";
                }


            }
        }
        //----------------------------------------------------------------------------------------------------

        [WebMethod]
        public static string CheckSegmentPresentOrNot(string CustomerID)
        {
            // string userbranchHierarchy = Convert.ToString(Session["userbranchHierarchy"]);
            string actionqry = "CheckSegmentPresentOrNot";

            string Isexit ="0";
            DataTable addtab = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_ENTITY_SEGMENT_MAP");
            proc.AddVarcharPara("@action", 100, actionqry);
            proc.AddVarcharPara("@InetrnalId", 100, CustomerID);
           

            addtab = proc.GetTable();
            if (addtab.Rows.Count > 0)
            {
                Isexit = Convert.ToString(addtab.Rows[0]["Isexit"]);
            }


            return Isexit;
        }
        //Rev work start 03.06.2022 Mantise issue:0024783: Customer & Product master import required for ERP
        protected void lnlDownloaderexcel_Click(object sender, EventArgs e)
        {

            string strFileName = "BreezeERP_Customer_Import.xlsx";
            //string strFileName = "BreezeERP_Customer_Import.xls";
            string strPath = (Convert.ToString(System.AppDomain.CurrentDomain.BaseDirectory) + Convert.ToString(ConfigurationManager.AppSettings["SaveFile"]) + strFileName);

            Response.ContentType = "application/xlsx";
            Response.AppendHeader("Content-Disposition", "attachment; filename=BreezeERP_Customer_Import.xlsx");
            //Response.AppendHeader("Content-Disposition", "attachment; filename=BreezeERP_Customer_Import.xls");
            Response.TransmitFile(strPath);
            Response.End();

        }
        protected void BtnSaveexcel_Click1(object sender, EventArgs e)
        {
            string fName = string.Empty;
            Boolean HasLog = false;
            if (OFDBankSelect.FileContent.Length != 0)
            {
                path = String.Empty;
                path1 = String.Empty;
                FileName = String.Empty;
                s = String.Empty;
                time = String.Empty;
                cannotParse = String.Empty;
                string strmodule = "InsertTradeData";

                BusinessLogicLayer.TransctionDescription td = new BusinessLogicLayer.TransctionDescription();

                FilePath = Path.GetFullPath(OFDBankSelect.PostedFile.FileName);
                FileName = Path.GetFileName(FilePath);
                string fileExtension = Path.GetExtension(FileName);

                if (fileExtension.ToUpper() != ".XLS" && fileExtension.ToUpper() != ".XLSX")
                {
                    Page.ClientScript.RegisterStartupScript(GetType(), "PageScript", "<script language='javascript'>jAlert('Uploaded file format not supported by the system');</script>");
                    return;
                }

                if (fileExtension.Equals(".xlsx"))
                {
                    fName = FileName.Replace(".xlsx", DateTime.Now.ToString("ddMMyyyyhhmmss") + ".xlsx");
                }

                else if (fileExtension.Equals(".xls"))
                {
                    fName = FileName.Replace(".xls", DateTime.Now.ToString("ddMMyyyyhhmmss") + ".xls");
                }

                else if (fileExtension.Equals(".csv"))
                {
                    fName = FileName.Replace(".csv", DateTime.Now.ToString("ddMMyyyyhhmmss") + ".csv");
                }

                Session["FileName"] = fName;

                String UploadPath = Server.MapPath((Convert.ToString(ConfigurationManager.AppSettings["SaveCSV"]) + Session["FileName"].ToString()));
                OFDBankSelect.PostedFile.SaveAs(UploadPath);

                ClearArray();

                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
                try
                {
                    HttpPostedFile file = OFDBankSelect.PostedFile;
                    String extension = Path.GetExtension(FileName);
                    HasLog = Import_To_Grid(UploadPath, extension, file);
                }
                catch (Exception ex)
                {
                    HasLog = false;
                }

                Page.ClientScript.RegisterStartupScript(GetType(), "PageScript", "<script language='javascript'>jAlert('Import Process Successfully Completed!'); ShowLogData('" + HasLog + "');</script>");
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "PageScript", "<script language='javascript'>jAlert('Selected File Cannot Be Blank');</script>");
            }

        }
        public void ClearArray()
        {
            Array.Clear(InputName, 0, InputName.Length - 1);
            Array.Clear(InputType, 0, InputType.Length - 1);
            Array.Clear(InputValue, 0, InputValue.Length - 1);
        }
        public Boolean Import_To_Grid(string FilePath, string Extension, HttpPostedFile file)
        {
            Contact objCustomer = new Contact();
            Boolean Success = false;
            Boolean HasLog = false;
            int loopcounter = 1;

            if (file.FileName.Trim() != "")
            {

                if (Extension.ToUpper() == ".XLS" || Extension.ToUpper() == ".XLSX")
                {
                    DataTable dt = new DataTable();

                    using (SpreadsheetDocument spreadSheetDocument = SpreadsheetDocument.Open(FilePath, false))
                    {

                        Sheet sheet = spreadSheetDocument.WorkbookPart.Workbook.Sheets.GetFirstChild<Sheet>();
                        Worksheet worksheet = (spreadSheetDocument.WorkbookPart.GetPartById(sheet.Id.Value) as WorksheetPart).Worksheet;
                        IEnumerable<Row> rows = worksheet.GetFirstChild<SheetData>().Descendants<Row>();
                        foreach (Row row in rows)
                        {
                            if (row.RowIndex.Value == 1)
                            {
                                foreach (Cell cell in row.Descendants<Cell>())
                                {
                                    if (cell.CellValue != null)
                                    {
                                        dt.Columns.Add(GetValue(spreadSheetDocument, cell));
                                    }
                                }
                            }
                            else
                            {
                                DataRow tempRow = dt.NewRow();
                                int columnIndex = 0;
                                foreach (Cell cell in row.Descendants<Cell>())
                                {
                                    // Gets the column index of the cell with data
                                    int cellColumnIndex = (int)GetColumnIndexFromName(GetColumnName(cell.CellReference));
                                    cellColumnIndex--; //zero based index
                                    if (columnIndex < cellColumnIndex)
                                    {
                                        do
                                        {
                                            tempRow[columnIndex] = ""; //Insert blank data here;
                                            columnIndex++;
                                        }
                                        while (columnIndex < cellColumnIndex);
                                    }
                                    try
                                    {
                                        tempRow[columnIndex] = GetValue(spreadSheetDocument, cell);
                                    }
                                    catch
                                    {
                                        tempRow[columnIndex] = "";
                                    }

                                    columnIndex++;
                                }
                                dt.Rows.Add(tempRow);
                            }
                        }
                    }

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        string CustomerCode = string.Empty;
                        string ContType = "Customer/Client";
                        // Rev 1.0
                        //string GroupID =string.Empty;
                        int GroupID = 0;
                        // End of Rev 1.0
                        int NumberSchemaId = 0;
                        string ISACTIVE = string.Empty;
                        int SCHEMA_TYPE = 0;
                        bool CheckUniqueCode = false;

                        string BillPin = string.Empty;
                        string ShipPin = string.Empty;

                        string bill_city_id = string.Empty;
                        string bill_state_id = string.Empty;
                        string bill_countryId = string.Empty;

                        string ship_city_id = string.Empty;
                        string ship_state_id = string.Empty;
                        string ship_countryId = string.Empty;

                        foreach (DataRow row in dt.Rows)
                        {
                            loopcounter++;
                            try
                            {
                                /*GroupID Checking start*/
                                DataTable dtgroupcode = oDBEngine.GetDataTable("SELECT gpm_id FROM TBL_MASTER_GROUPMASTER WHERE  GPM_CODE='" + Convert.ToString(row["Group Code"]) + "'");
                                if (dtgroupcode.Rows.Count > 0)
                                {
                                    // Rev 1.0
                                    //GroupID = dtgroupcode.Rows[0]["gpm_id"].ToString();
                                    GroupID = Convert.ToInt32(dtgroupcode.Rows[0]["gpm_id"]);
                                    // End of Rev 1.0
                                }
                                /*GroupID Checking close*/

                                /*Billing and ShipingPin check start*/
                                DataTable BillPinDt = oDBEngine.GetDataTable("SELECT pin_id FROM tbl_master_pinzip WHERE pin_code='" + Convert.ToString(row["Billing-Pin*"]) + "'");
                                if(BillPinDt.Rows.Count>0)
                                {
                                    BillPin = BillPinDt.Rows[0]["pin_id"].ToString();
                                }
                                /*Billing and ShipingPin check close*/

                                /*Shipping and ShipingPin check start*/
                                DataTable ShipPinDt = oDBEngine.GetDataTable("SELECT pin_id FROM tbl_master_pinzip WHERE pin_code='" + Convert.ToString(row["Shipping-Pin*"]) + "'");
                                if (ShipPinDt.Rows.Count > 0)
                                {
                                    ShipPin = ShipPinDt.Rows[0]["pin_id"].ToString();
                                }
                                /*Shipping and ShipingPin check close*/

                                /*Billing Country,state,district fetch start*/
                                //DataTable Billdt = oDBEngine.GetDataTable("SELECT p.city_id,c.state_id,s.countryId FROM tbl_master_pinzip p,tbl_master_city c,tbl_master_state s,tbl_master_country cn " +
                                //" WHERE p.city_id=c.city_id and c.state_id=s.id and s.countryId=cn.cou_id and p.pin_id='" + Convert.ToInt32(BillPin) + "'");
                                //if (Billdt.Rows.Count > 0)
                                //{
                                //    bill_city_id = Billdt.Rows[0]["city_id"].ToString();
                                //    bill_state_id = Billdt.Rows[0]["state_id"].ToString();
                                //    bill_countryId = Billdt.Rows[0]["countryId"].ToString();
                                //}
                                /*Billing Country,state,district fetch close*/

                                /*shipping Country,state,district fetch start*/
                                //DataTable Shipdt = oDBEngine.GetDataTable("SELECT p.city_id,c.state_id,s.countryId FROM tbl_master_pinzip p,tbl_master_city c,tbl_master_state s,tbl_master_country cn " +
                                //" WHERE p.city_id=c.city_id and c.state_id=s.id and s.countryId=cn.cou_id and p.pin_id='" + Convert.ToInt32(ShipPin) + "'");
                                //if (Shipdt.Rows.Count > 0)
                                //{
                                //    ship_city_id = Shipdt.Rows[0]["city_id"].ToString();
                                //    ship_state_id = Shipdt.Rows[0]["state_id"].ToString();
                                //    ship_countryId = Shipdt.Rows[0]["countryId"].ToString();
                                //}
                                /*Billing Country,state,district fetch close*/

                                CommonBL ComBL = new CommonBL();
                                string UniqueAutoNumberCustomerMaster = ComBL.GetSystemSettingsResult("UniqueAutoNumberCustomerMaster");                                
                                if(UniqueAutoNumberCustomerMaster=="Yes")
                                {
                                    DataTable NumberSchemeDT = oDBEngine.GetDataTable("SELECT ID,ISNULL(ISACTIVE,0)ISACTIVE,SCHEMA_TYPE FROM TBL_MASTER_IDSCHEMA WHERE TYPE_ID=126 AND  ISNULL(ISACTIVE,0)=1 AND SCHEMANAME='" + Convert.ToString(row["Numbering Scheme"]) + "'");
                                    if (NumberSchemeDT.Rows.Count > 0)
                                    {
                                        
                                        ISACTIVE = NumberSchemeDT.Rows[0]["ISACTIVE"].ToString();
                                        SCHEMA_TYPE = Convert.ToInt32(NumberSchemeDT.Rows[0]["SCHEMA_TYPE"].ToString());
                                        if (SCHEMA_TYPE == 1)
                                        {
                                            NumberSchemaId = Convert.ToInt32(NumberSchemeDT.Rows[0]["ID"].ToString());
                                            CustomerCode = "Auto";
                                        }
                                        else
                                        {
                                           /* CheckUniqueCode=CheckUniqueNumberingCode(Convert.ToString(row["Customer Code*"]), "Mastercustomerclient");
                                            if (CheckUniqueCode == true)
                                            {
                                                //Page.ClientScript.RegisterStartupScript(GetType(), "PageScript", "<script language='javascript'>jAlert('Please enter unique No.');</script>");
                                                return true;
                                            }
                                            else
                                            {
                                                NumberSchemaId = Convert.ToInt32(NumberSchemeDT.Rows[0]["ID"].ToString());
                                                CustomerCode = Convert.ToString(row["Customer Code*"]);
                                            }*/
                                            NumberSchemaId = Convert.ToInt32(NumberSchemeDT.Rows[0]["ID"].ToString());
                                            CustomerCode = Convert.ToString(row["Customer Code*"]);
                                        }
                                    }
                                } 
                                else
                                {
                                    NumberSchemaId = 0;
                                    CustomerCode = Convert.ToString(row["Customer Code*"]);
                                }
                              
                                
                                string CustomerName = Convert.ToString(row["Customer Name*"]);
                                string PrintName = Convert.ToString(row["Print Name"]);                                
                                string CreditDays = Convert.ToString(row["Credit Days"]);
                                string CreditLimit = Convert.ToString(row["Credit Limit"]);
                                string Account = Convert.ToString(row["Account"]);
                                string Registered = Convert.ToString(row["Registered"]);
                                string GSTIN = Convert.ToString(row["GSTIN"]);
                                string TransactionCategory= Convert.ToString(row["Transaction Category*"]);
                                string AddressBillType = Convert.ToString(row["Address Type*"]);
                                string EMAILID = Convert.ToString(row["EMAIL ID"]);

                                string BillingContactPerson = Convert.ToString(row["Billing-Contact Person"]);
                                string BillingAddress1 = Convert.ToString(row["Billing-Address1*"]);
                                string BillingAddress2 = Convert.ToString(row["Billing-Address2"]);
                                string BillingAddress3 = Convert.ToString(row["Billing-Address3"]);
                                string BillingLandmark = Convert.ToString(row["Billing Landmark"]);
                                string BillingPhone = Convert.ToString(row["Billing-Phone*"]);
                                string BillingPin = BillPin;
                                /*string BillingDist = bill_city_id;
                                string BillingState = bill_state_id;
                                string BillingCnt = bill_countryId;*/


                                string AddressShipType = Convert.ToString(row["Address Type1*"]);
                                string ShippingAddress1 = Convert.ToString(row["Shipping-Address1*"]);
                                string ShippingAddress2 = Convert.ToString(row["Shipping-Address2"]);
                                string ShippingAddress3 = Convert.ToString(row["Shipping-Address3"]);
                                string ShippingLandmark = Convert.ToString(row["Shipping Landmark"]);
                                string ShippingPhone = Convert.ToString(row["Shipping-Phone*"]);
                                string ShippingPin = ShipPin;
                                /*string ShippingDist = ship_city_id;
                                string ShippingState = ship_state_id;
                                string ShippingCnt = ship_countryId;*/

                                // Rev 1.0
                                //string GroupCode = GroupID;
                                int GroupCode = GroupID;
                                // End of Rev 1.0
                                string UserId = Convert.ToString(HttpContext.Current.Session["userid"]);
                                string ContactType = Convert.ToString(ContType);
                               

                                //DataSet dt2 = objCustomer.InsertCustomerDataFromExcel
                                //    (CustomerCode, CustomerName, PrintName, CreditDays, CreditLimit, Account,
                                //    Registered, GSTIN,TransactionCategory, AddressBillType, EMAILID, BillingContactPerson, BillingAddress1, BillingAddress2,
                                //        BillingAddress3,BillingLandmark, BillingPhone, BillingPin,BillingDist, BillingState,BillingCnt,
                                //        AddressShipType, ShippingAddress1, ShippingAddress2, ShippingAddress3,ShippingLandmark,
                                //        ShippingPhone, ShippingPin,ShippingDist,ShippingState,ShippingCnt, GroupCode, UserId, ContactType, NumberSchemaId);

                                DataSet dt2 = objCustomer.InsertCustomerDataFromExcel
                                    (CustomerCode, CustomerName, PrintName, CreditDays, CreditLimit, Account,
                                    Registered, GSTIN, TransactionCategory, AddressBillType, EMAILID, BillingContactPerson, BillingAddress1, BillingAddress2,
                                        BillingAddress3, BillingLandmark, BillingPhone, BillingPin, 
                                        AddressShipType, ShippingAddress1, ShippingAddress2, ShippingAddress3, ShippingLandmark,
                                        ShippingPhone, ShippingPin,  GroupCode, UserId, ContactType, NumberSchemaId);
                               
                                if (dt2 != null && dt2.Tables[0].Rows.Count > 0)
                                {
                                    foreach (DataRow row2 in dt2.Tables[0].Rows)
                                    {
                                        Success = Convert.ToBoolean(row2["Success"]);
                                        HasLog = Convert.ToBoolean(row2["HasLog"]);
                                    }
                                }

                                if (!HasLog)
                                {
                                    string description = Convert.ToString(dt2.Tables[0].Rows[0]["MSG"]);
                                    int loginsert = objCustomer.InsertCustomerImportLOg(CustomerCode, loopcounter, CustomerName, UserId, Session["FileName"].ToString(), description, "Failed");
                                }

                                else
                                {
                                    string description = Convert.ToString(dt2.Tables[0].Rows[0]["MSG"]);
                                    int loginsert = objCustomer.InsertCustomerImportLOg(CustomerCode, loopcounter, CustomerName, UserId, Session["FileName"].ToString(), description, "Success");
                                }

                            }
                            catch (Exception ex)
                            {
                                Success = false;
                                HasLog = false;
                                // string description = Convert.ToString(dt2.Tables[0].Rows[0]["MSG"]);
                                int loginsert = objCustomer.InsertCustomerImportLOg(CustomerCode, loopcounter, "", "", Session["FileName"].ToString(), ex.Message.ToString(), "Failed");
                            }

                        }
                    }

                }
                else
                {

                }
            }
            return HasLog;
        }
        private string GetValue(SpreadsheetDocument doc, Cell cell)
        {
            string value = cell.CellValue.InnerText;
            if (cell.DataType != null && cell.DataType.Value == CellValues.SharedString)
            {
                return doc.WorkbookPart.SharedStringTablePart.SharedStringTable.ChildElements.GetItem(int.Parse(value)).InnerText;
            }
            return value;
        }
        public static int? GetColumnIndexFromName(string columnName)
        {
            //return columnIndex;
            string name = columnName;
            int number = 0;
            int pow = 1;
            for (int i = name.Length - 1; i >= 0; i--)
            {
                number += (name[i] - 'A' + 1) * pow;
                pow *= 26;
            }
            return number;
        }
        public static string GetColumnName(string cellReference)
        {
            // Create a regular expression to match the column name portion of the cell name.
            Regex regex = new Regex("[A-Za-z]+");
            Match match = regex.Match(cellReference);

            return match.Value;
        }
        public static bool CheckUniqueNumberingCode(string uccName, string Type)
        {
            bool flag = false;
            BusinessLogicLayer.GenericMethod oGenericMethod = new BusinessLogicLayer.GenericMethod();
            try
            {
                MShortNameCheckingBL objShortNameChecking = new MShortNameCheckingBL();
                flag = objShortNameChecking.CheckUnique(uccName, "0", Type);
            }
            catch (Exception ex)
            {

            }
            finally
            {
            }
            return flag;
        }
        protected void GvJvSearch_DataBinding(object sender, EventArgs e)
        {
            Contact objCust = new Contact();
            string fileName = Convert.ToString(Session["FileName"]);
            DataSet dt2 = objCust.GetCustomerLog(fileName);
            GvJvSearch.DataSource = dt2.Tables[0];
        }

        //Rev work close 03.06.2022 Mantise issue:0024783: Customer & Product master import required for ERP

        // Rev 2.0
        protected void lnlDownloaderexcelIndustry_Click(object sender, EventArgs e)
        {

            string strFileName = "BreezeERP_IndustryMap_Import.xlsx";
            string strPath = (Convert.ToString(System.AppDomain.CurrentDomain.BaseDirectory) + Convert.ToString(ConfigurationManager.AppSettings["SaveFile"]) + strFileName);

            Response.ContentType = "application/xlsx";
            Response.AppendHeader("Content-Disposition", "attachment; filename=BreezeERP_IndustryMap_Import.xlsx");
            Response.TransmitFile(strPath);
            Response.End();

        }
        protected void BtnBulkImportMapIndustry_Click(object sender, EventArgs e)
        {
            string fName = string.Empty;
            Boolean HasLog = false;
            if (OFDIndustrySelect.FileContent.Length != 0)
            {
                path = String.Empty;
                path1 = String.Empty;
                FileName = String.Empty;
                s = String.Empty;
                time = String.Empty;
                cannotParse = String.Empty;
               // string strmodule = "InsertTradeData";

                BusinessLogicLayer.TransctionDescription td = new BusinessLogicLayer.TransctionDescription();

                FilePath = Path.GetFullPath(OFDIndustrySelect.PostedFile.FileName);
                FileName = Path.GetFileName(FilePath);
                string fileExtension = Path.GetExtension(FileName);

                if (fileExtension.ToUpper() != ".XLS" && fileExtension.ToUpper() != ".XLSX")
                {
                    Page.ClientScript.RegisterStartupScript(GetType(), "PageScript", "<script language='javascript'>jAlert('Uploaded file format not supported by the system');</script>");
                    return;
                }

                if (fileExtension.Equals(".xlsx"))
                {
                    fName = FileName.Replace(".xlsx", DateTime.Now.ToString("ddMMyyyyhhmmss") + ".xlsx");
                }

                else if (fileExtension.Equals(".xls"))
                {
                    fName = FileName.Replace(".xls", DateTime.Now.ToString("ddMMyyyyhhmmss") + ".xls");
                }

                else if (fileExtension.Equals(".csv"))
                {
                    fName = FileName.Replace(".csv", DateTime.Now.ToString("ddMMyyyyhhmmss") + ".csv");
                }

                Session["FileName"] = fName;

                String UploadPath = Server.MapPath((Convert.ToString(ConfigurationManager.AppSettings["SaveCSV"]) + Session["FileName"].ToString()));
                OFDIndustrySelect.PostedFile.SaveAs(UploadPath);

                ClearArray();

                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
                try
                {
                    HttpPostedFile file = OFDIndustrySelect.PostedFile;
                    String extension = Path.GetExtension(FileName);
                    HasLog = ImportIndustryMap_To_Grid(UploadPath, extension, file);
                }
                catch (Exception ex)
                {
                    HasLog = false;
                }

                Page.ClientScript.RegisterStartupScript(GetType(), "PageScript", "<script language='javascript'>jAlert('Import Process Successfully Completed!'); ShowBulkImportIndustryMapLogData('" + HasLog + "');</script>");
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "PageScript", "<script language='javascript'>jAlert('Selected File Cannot Be Blank');</script>");
            }

        }

        public Boolean ImportIndustryMap_To_Grid(string FilePath, string Extension, HttpPostedFile file)
        {
            Contact objCustomer = new Contact();
            Boolean Success = false;
            Boolean HasLog = false;
            int loopcounter = 1;

            if (file.FileName.Trim() != "")
            {

                if (Extension.ToUpper() == ".XLS" || Extension.ToUpper() == ".XLSX")
                {
                    DataTable dt = new DataTable();

                    using (SpreadsheetDocument spreadSheetDocument = SpreadsheetDocument.Open(FilePath, false))
                    {

                        Sheet sheet = spreadSheetDocument.WorkbookPart.Workbook.Sheets.GetFirstChild<Sheet>();
                        Worksheet worksheet = (spreadSheetDocument.WorkbookPart.GetPartById(sheet.Id.Value) as WorksheetPart).Worksheet;
                        IEnumerable<Row> rows = worksheet.GetFirstChild<SheetData>().Descendants<Row>();
                        foreach (Row row in rows)
                        {
                            if (row.RowIndex.Value == 1)
                            {
                                foreach (Cell cell in row.Descendants<Cell>())
                                {
                                    if (cell.CellValue != null)
                                    {
                                        dt.Columns.Add(GetValue(spreadSheetDocument, cell));
                                    }
                                }
                            }
                            else
                            {
                                DataRow tempRow = dt.NewRow();
                                int columnIndex = 0;
                                foreach (Cell cell in row.Descendants<Cell>())
                                {
                                    // Gets the column index of the cell with data
                                    int cellColumnIndex = (int)GetColumnIndexFromName(GetColumnName(cell.CellReference));
                                    cellColumnIndex--; //zero based index
                                    if (columnIndex < cellColumnIndex)
                                    {
                                        do
                                        {
                                            tempRow[columnIndex] = ""; //Insert blank data here;
                                            columnIndex++;
                                        }
                                        while (columnIndex < cellColumnIndex);
                                    }
                                    try
                                    {
                                        tempRow[columnIndex] = GetValue(spreadSheetDocument, cell);
                                    }
                                    catch
                                    {
                                        tempRow[columnIndex] = "";
                                    }

                                    columnIndex++;
                                }
                                dt.Rows.Add(tempRow);
                            }
                        }
                    }

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        string CustomerUniqueID = string.Empty;
                        string CustomerName = string.Empty;
                        string IndustryName = string.Empty;

                        foreach (DataRow row in dt.Rows)
                        {
                            loopcounter++;
                            try
                            {
                                CustomerUniqueID = Convert.ToString(row["Customer Unique ID*"]).Trim();
                                CustomerName = Convert.ToString(row["Customer Name*"]).Trim();
                                IndustryName = Convert.ToString(row["Industry Name*"]).Trim();

                                if (CustomerUniqueID != "" && CustomerName != "" && IndustryName != "")
                                {
                                    DataSet ds = new DataSet();
                                    ProcedureExecute proc = new ProcedureExecute("PRC_INDUSTRYMAPIMPORTFROMEXCEL");
                                    proc.AddVarcharPara("@Action", 100, "InsertIndustryMapDataFromExcel");
                                    proc.AddVarcharPara("@CustomerUniqueID", 100, CustomerUniqueID);
                                    proc.AddVarcharPara("@CustomerName", 200, CustomerName);
                                    proc.AddVarcharPara("@IndustryName", 100, IndustryName);
                                    proc.AddVarcharPara("@FileName", 500, file.FileName.Trim());
                                    proc.AddVarcharPara("@UserId", 100, Convert.ToString(HttpContext.Current.Session["userid"]));
                                    ds = proc.GetDataSet();


                                    if (ds != null && ds.Tables[0].Rows.Count > 0)
                                    {
                                        foreach (DataRow row2 in ds.Tables[0].Rows)
                                        {
                                            Success = Convert.ToBoolean(row2["Success"]);
                                            HasLog = Convert.ToBoolean(row2["HasLog"]);
                                        }
                                    }

                                    if (!HasLog)
                                    {
                                        string description = Convert.ToString(ds.Tables[0].Rows[0]["MSG"]);
                                        //int loginsert = objCustomer.InsertCustomerImportLOg(CustomerCode, loopcounter, CustomerName, UserId, Session["FileName"].ToString(), description, "Failed");
                                    }

                                    else
                                    {
                                        string description = Convert.ToString(ds.Tables[0].Rows[0]["MSG"]);
                                        //int loginsert = objCustomer.InsertCustomerImportLOg(CustomerCode, loopcounter, CustomerName, UserId, Session["FileName"].ToString(), description, "Success");
                                    }
                                }

                            }
                            catch (Exception ex)
                            {
                                Success = false;
                                HasLog = false;
                                // string description = Convert.ToString(dt2.Tables[0].Rows[0]["MSG"]);
                                //int loginsert = objCustomer.InsertCustomerImportLOg(CustomerCode, loopcounter, "", "", Session["FileName"].ToString(), ex.Message.ToString(), "Failed");
                            }

                        }
                    }

                }
                else
                {

                }
            }
            return HasLog;
        }

        protected void GvIndustryMapLog_DataBinding(object sender, EventArgs e)
        {
            DataSet dt2 = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("PRC_INDUSTRYMAPIMPORTFROMEXCEL");
            proc.AddVarcharPara("@action", 150, "getCustomerIndustryMapLog");
            dt2 = proc.GetDataSet();

            GvIndustryMapLog.DataSource = dt2.Tables[0];
        }
        // End of Rev 2.0
    }
}
