using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;
using BusinessLogicLayer;
using ClsDropDownlistNameSpace;
using EntityLayer.CommonELS;
using System.Web.Services;
using System.Collections.Generic;
using ClsDropDownlistNameSpace;
using System.Collections;
using DevExpress.Web.Data;
using DataAccessLayer;
using System.ComponentModel;
using System.Linq;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace ERP.OMS.Management.Activities
{
    public partial class CustomerDelivery : ERP.OMS.ViewState_class.VSPage //System.Web.UI.Page
    {
        BusinessLogicLayer.Converter objConverter = new BusinessLogicLayer.Converter();


       // BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

        clsDropDownList oclsDropDownList = new clsDropDownList();
        BusinessLogicLayer.Contact oContactGeneralBL = new BusinessLogicLayer.Contact();
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        BusinessLogicLayer.RemarkCategoryBL reCat = new BusinessLogicLayer.RemarkCategoryBL();
        BusinessLogicLayer.Others objBL = new BusinessLogicLayer.Others();
        static string ForJournalDate = null;
        SlaesActivitiesBL objSlaesActivitiesBL = new SlaesActivitiesBL();
        string UniqueQuotation = string.Empty;
        public string pageAccess = "";
        string userbranch = "";


        BusinessLogicLayer.CustomerDeliveryBL objCustomerDeliveryBL = new CustomerDeliveryBL();


        protected void Page_PreInit(object sender, EventArgs e) // lead add
        {
            if (!IsPostBack)
            {
                string sPath = Convert.ToString(HttpContext.Current.Request.Url);


                oDBEngine.Call_CheckPageaccessebility(sPath);
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //GetAllDropDownDetailForCustDelivery();
                if (Convert.ToString(Request.QueryString["key"]) != "ADD")
                {
                    ltrTitle.Text = "Modify Customer Delivery";

                    string deliveryID = Convert.ToString(Request.QueryString["key"]);
                    DataSet dst = GetDeliveryList(deliveryID);
                    SetEditableData(dst);
                    if (dst.Tables.Count > 1)
                        Session.Add("sessCustDelChallanList", dst.Tables[1]);
                    ddl_numbering.Visible = false;
                    cdtxtDeliveryNumber.ReadOnly = true;
                    //cdCmbBranch.Enabled = false;

                    if (Session["sessCustDelChallanList"] != null)
                    {
                        DataTable dtDelChallanList = new DataTable();
                        dtDelChallanList = (DataTable)Session["sessCustDelChallanList"];

                        grid.DataSource = dtDelChallanList;
                        grid.DataBind();
                    }


                    #region Samrat Roy -- Hide Save Button in Edit Mode
                    if (Request.QueryString["req"] != null && Request.QueryString["req"] == "V")
                    {
                        ltrTitle.Text = "View Customer Delivery";
                        lbl_quotestatusmsg.Text = "*** View Mode Only";
                        btn_SaveRecords.Visible = false;
                        ASPxButton12.Visible = false;
                        btnLoadChallan.Visible = false;
                        lbl_quotestatusmsg.Visible = true;
                    }
                    #endregion
                }
                else
                {
                    GetAllDropDownDetailForCustDelivery();
                    if (Session["sessCustDelChallanList"] != null)
                    {
                        Session.Remove("sessCustDelChallanList");
                    }
                }

                if (Request.QueryString["Permission"] != null)
                {
                    if (Convert.ToString(Request.QueryString["Permission"]) == "1")
                    {
                        pnl_CustomerDelivery.Enabled = true;
                    }
                    else if (Convert.ToString(Request.QueryString["Permission"]) == "2")
                    {
                        pnl_CustomerDelivery.Enabled = true;
                    }
                    else if (Convert.ToString(Request.QueryString["Permission"]) == "3")
                    {
                        pnl_CustomerDelivery.Enabled = true;
                    }
                }

                NumberingSystemBind();
            }
        }

        public void SetFinYearCurrentDate()
        {
            cdDeliveryDate.EditFormatString = objConverter.GetDateFormat("Date");
            string fDate = null;

            //DateTime dt = DateTime.ParseExact("3/31/2016", "MM/dd/yyy", CultureInfo.InvariantCulture);
            string[] FinYEnd = Convert.ToString(Session["FinYearEnd"]).Split(' ');
            string FinYearEnd = FinYEnd[0];

            DateTime date3 = DateTime.ParseExact(FinYearEnd, "M/d/yyyy", System.Globalization.CultureInfo.InvariantCulture);

            ForJournalDate = Convert.ToString(date3);

            //ForJournalDate =Session["FinYearEnd"].ToString();
            int month = oDBEngine.GetDate().Month;
            int date = oDBEngine.GetDate().Day;
            int Year = oDBEngine.GetDate().Year;

            if (date3 < oDBEngine.GetDate().Date)
            {
                fDate = Convert.ToString(Convert.ToDateTime(ForJournalDate).Month) + "/" + Convert.ToString(Convert.ToDateTime(ForJournalDate).Day) + "/" + Convert.ToString(Convert.ToDateTime(ForJournalDate).Year);
            }
            else
            {
                fDate = Convert.ToString(Convert.ToDateTime(oDBEngine.GetDate().Date).Month) + "/" + Convert.ToString(Convert.ToDateTime(oDBEngine.GetDate().Date).Day) + "/" + Convert.ToString(Convert.ToDateTime(oDBEngine.GetDate().Date).Year);
            }

            cdDeliveryDate.Value = DateTime.ParseExact(fDate, @"M/d/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        }
        public void GetFinacialYearBasedQouteDate()
        {
            String finyear = "";
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
                        cdDeliveryDate.MinDate = Convert.ToDateTime(Convert.ToString(Session["FinYearStartDate"]));
                    }
                    if (Session["FinYearEndDate"] != null)
                    {
                        cdDeliveryDate.MaxDate = Convert.ToDateTime(Convert.ToString(Session["FinYearEndDate"]));
                    }
                }
            }
            //dt_PLQuote.Value = Convert.ToDateTime(oDBEngine.GetDate().ToString());
        }

        #region ###### Added By : Samrat Roy ##############

        protected void CallBackCustDeliveryHeader_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            callBackCustDeliveryHeader.JSProperties["cpEndCallBackReqFlag"] = 0;
            callBackCustDeliveryHeader.JSProperties["cpNumberingScheme"] = 0;
            callBackCustDeliveryHeader.JSProperties["cpNumberingSchemeType"] = string.Empty;
            callBackCustDeliveryHeader.JSProperties["cpCheckFlag"] = -1;
            callBackCustDeliveryHeader.JSProperties["cpDriverFoccus"] = 0;

            string[] listData = e.Parameter.Split('~');
            string schemeType = "";
            switch (listData[0])
            {
                case "BindAreaPin":
                    if (listData.Length > 2)
                    {
                        GetAllDropDownDetailForCustDelivery(listData[4]);
                        cdCmbBranch.Value = listData[4];
                        cdCmbBranch.Enabled = false;
                        if (listData[2] == "0")
                        {
                            cdtxtDeliveryNumber.Text = "";
                            cdtxtDeliveryNumber.Enabled = true;
                        }
                        else if (listData[2] == "1")
                        {
                            schemeType = listData[2];
                            cdtxtDeliveryNumber.Text = "Auto";
                            cdtxtDeliveryNumber.Enabled = false;
                        }
                        else
                        {
                            cdtxtDeliveryNumber.Text = "";
                            cdtxtDeliveryNumber.Enabled = true;
                        }
                    }
                    else
                    {
                        cdCmbBranch.Enabled = true;
                        GetAllDropDownDetailForCustDelivery();
                    }
                    callBackCustDeliveryHeader.JSProperties["cpEndCallBackReqFlag"] = 0;
                    callBackCustDeliveryHeader.JSProperties["cpNumberingScheme"] = 1;
                    callBackCustDeliveryHeader.JSProperties["cpNumberingSchemeType"] = schemeType;
                    break;
                case "EditDetails":
                    if (Request.QueryString["key"] != null && Request.QueryString["key"] != "ADD")
                    {
                        string deliveryID = Convert.ToString(Request.QueryString["key"]);
                        DataSet dst = GetDeliveryList(deliveryID);
                        SetEditableData(dst);
                        if (dst.Tables.Count > 1)
                            Session.Add("sessCustDelChallanList", dst.Tables[1]);
                        ddl_numbering.Visible = false;
                        cdtxtDeliveryNumber.ReadOnly = true;
                    }
                    break;
                case "LoadDriver":
                    string branchID = Convert.ToString(cdCmbBranch.Value);
                    string vehicleNo = "";//Convert.ToString(cdCmbVehicle.Value);
                    string areaID = Convert.ToString(cdCmbArea.Value);
                    string pinCode = Convert.ToString(cdCmbPin.Value);
                    GetAllDropDownDetailForCustDelivery(branchID);
                    bool byHandFlag = objCustomerDeliveryBL.GetByHandFlagByVehicle(vehicleNo, branchID);
                    if (byHandFlag == false)
                    {
                        BindDriverDropdown(objCustomerDeliveryBL.GetDriverDetailsByVehicle(vehicleNo, branchID));
                    }
                    cdCmbBranch.Value = branchID;
                    cdCmbArea.Value = areaID;
                    cdCmbPin.Value = pinCode;
                    //cdCmbVehicle.Value = vehicleNo;
                    callBackCustDeliveryHeader.JSProperties["cpNumberingScheme"] = 0;
                    callBackCustDeliveryHeader.JSProperties["cpEndCallBackReqFlag"] = 0;
                    callBackCustDeliveryHeader.JSProperties["cpDriverFoccus"] = 1;
                    callBackCustDeliveryHeader.JSProperties["cpByHandFlag"] = byHandFlag;
                    break;
                case "SaveData":
                    string actionType = "ADD";
                    long keyValue = 0;
                    if (Request.QueryString["key"] != null && Request.QueryString["key"] != "ADD") { actionType = "EDIT"; keyValue = Convert.ToInt64(Request.QueryString["key"]); }
                    callBackCustDeliveryHeader.JSProperties["cpEndCallBackReqFlag"] = 0;
                    string[] SchemeList = ddl_numberingScheme.SelectedValue.Split(new string[] { "~" }, StringSplitOptions.None);
                    string validate = string.Empty;

                    if (Request.QueryString["key"] == "ADD")
                    {
                        if (SchemeList[0] != "")
                        {
                            validate = checkNMakeJVCode(Convert.ToString(cdtxtDeliveryNumber.Value), Convert.ToInt32(SchemeList[0]));
                        }

                        if (validate == "ok")
                        {
                            // Do Save the process 

                            ArrayList arrList = SaveCustDeliveryData(actionType, keyValue);
                            arrList.Add(listData[1]);
                            if (Convert.ToInt32(arrList[0]) == 1)
                            {
                                callBackCustDeliveryHeader.JSProperties["cpEndCallBackReqFlag"] = 1;
                                callBackCustDeliveryHeader.JSProperties["cpCheckFlag"] = arrList;
                            }
                            else
                            {
                                // Error... need to check
                                callBackCustDeliveryHeader.JSProperties["cpCheckFlag"] = arrList;
                            }
                        }
                        else
                        {
                            // Do not Save the process
                            callBackCustDeliveryHeader.JSProperties["cpCheckFlag"] = -1;
                        }
                    }
                    else  //for edit process
                    {
                        ArrayList arrList = SaveCustDeliveryData(actionType, keyValue);
                        arrList.Add(listData[1]);
                        if (Convert.ToInt32(arrList[0]) == 2)
                        {
                            callBackCustDeliveryHeader.JSProperties["cpEndCallBackReqFlag"] = 1;
                            callBackCustDeliveryHeader.JSProperties["cpCheckFlag"] = arrList;
                        }
                        else
                        {
                            // Error... need to check
                            callBackCustDeliveryHeader.JSProperties["cpCheckFlag"] = arrList;
                        }
                    }
                    break;
                default:
                    GetAllDropDownDetailForCustDelivery();
                    cdtxtDeliveryNumber.Enabled = false;
                    break;
            }


        }

        //protected void cdCmbDriver_Callback(object sender, CallbackEventArgsBase e)
        //{
        //    BindDriverDropdown(objCustomerDeliveryBL.GetDriverDetailsByVehicle(Convert.ToString(cdCmbVehicle.Value), Convert.ToString(cdCmbBranch.Value)));

        //    string[] listData = e.Parameter.Split('~');
        //    if (listData.Length > 0)
        //    {
        //        string driverID = Convert.ToString(listData[0]);
        //        cdCmbDriver.JSProperties["cpDriverID"] = driverID;
        //        cdCmbDriver.JSProperties["cpDriverPhoneNo"] = string.Empty;
        //        if (driverID.Trim() != "0")
        //        {
        //            DataTable dt = objCustomerDeliveryBL.GetDriverPhoneNo(driverID);
        //            if (dt.Rows.Count > 0)
        //            {
        //                cdCmbDriver.JSProperties["cpDriverPhoneNo"] = Convert.ToString(dt.Rows[0]["PhoneNo"]);
        //            }
        //        }

        //    }
        //}

        private ArrayList SaveCustDeliveryData(string actionType, long keyValue)
        {
            //int checkFlag = 0;
            ArrayList arrList = new ArrayList();

            string numeringSystem = ddl_numberingScheme.SelectedValue;
            string deliveryNumber = (Convert.ToString(cdtxtDeliveryNumber.Value) == "Auto") ? UniqueQuotation : Convert.ToString(cdtxtDeliveryNumber.Value);
            string delDate = cdDeliveryDate.Date.ToString("yyyy-MM-dd");
            string finYear = Convert.ToString(Session["LastFinYear"]);
            int branch = (cdCmbBranch.Value != null && cdCmbBranch.Value != "") ? Convert.ToInt32(cdCmbBranch.Value) : 0;
            string reference = Convert.ToString(txt_Refference.Value);

            //string vehicleNo = Convert.ToString(cdCmbVehicle.SelectedValue);
            string vehicleNo = Convert.ToString(ddl_vehicleNo.SelectedValue);
            //string driverName = (cdCmbDriver.Items.Count > 0) ? Convert.ToString(cdCmbDriver.SelectedValue) : string.Empty;
            string driverName = (!string.IsNullOrEmpty(hdnddl_DriverName.Value)) ? Convert.ToString(hdnddl_DriverName.Value) : string.Empty;
           // string driverName = (ddl_DriverName.Items.Count > 0) ? Convert.ToString(ddl_DriverName.SelectedValue) : string.Empty;
            string driverPhNo = Convert.ToString(txtDriverPhoneNo.Value);
            string routeNo = Convert.ToString(txtRouteNo.Value);
            string area = Convert.ToString(cdCmbArea.Value) == "0" ? "" : Convert.ToString(cdCmbArea.Value);
            string pincode = Convert.ToString(cdCmbPin.Value) == "0" ? "" : Convert.ToString(cdCmbPin.Value);
            string eWayBillNo = Convert.ToString(txtEWayBillNo.Value);
            string action = actionType;
            long deliveryID = keyValue;
            int userID = Convert.ToInt32(Session["userid"]);
            string companyID = Convert.ToString(Session["LastCompany"]);
            //string byHand = (cdCmbDriver.Items.Count == 0) ? Convert.ToString(txtByHand.Value) : string.Empty;
            string byHand = (ddl_DriverName.Items.Count == 0) ? Convert.ToString(txtByHand.Value) : string.Empty;

            DataTable dtCustDelChallanList = (DataTable)Session["sessCustDelChallanList"];

            if (dtCustDelChallanList.Rows.Count > 0)
            {
                string challanDetailsIDs = string.Empty;
                foreach (DataRow dr in dtCustDelChallanList.Rows)
                {
                    challanDetailsIDs += "," + dr["UniqueID"].ToString();
                }
                challanDetailsIDs = challanDetailsIDs.TrimStart(',');

                arrList = objCustomerDeliveryBL.InsertUpdateCustDelivery(
                        challanDetailsIDs, deliveryNumber, delDate, eWayBillNo, finYear, companyID, branch, reference, vehicleNo, driverName, driverPhNo, routeNo, area, pincode, action, userID, keyValue,byHand);
            }


            return arrList;

        }

        #region ######## Private Method ##################

        private void NumberingSystemBind()
        {
            string strCompanyID = Convert.ToString(Session["LastCompany"]);
            string strBranchID = Convert.ToString(Session["userbranchID"]);
            string userbranchHierarchy = Convert.ToString(Session["userbranchHierarchy"]);
            string FinYear = Convert.ToString(Session["LastFinYear"]);

            SlaesActivitiesBL objSlaesActivitiesBL1 = new SlaesActivitiesBL();
            DataTable Schemadt = objSlaesActivitiesBL1.GetNumberingSchema(strCompanyID, userbranchHierarchy, FinYear, "37", "Y");
            if (Schemadt != null && Schemadt.Rows.Count > 0)
            {
                ddl_numberingScheme.DataTextField = "SchemaName";
                ddl_numberingScheme.DataValueField = "Id";
                ddl_numberingScheme.DataSource = Schemadt;
                ddl_numberingScheme.DataBind();
            }
        }
        public void GetAllDropDownDetailForCustDelivery(string strBranchID = "")
        {
            DataSet dst = new DataSet();
            if (string.IsNullOrEmpty(strBranchID.Trim()))
            {
                strBranchID = Convert.ToString(Session["userbranchID"]);
            }

            strBranchID = oDBEngine.getBranch(strBranchID, "") + strBranchID;

            dst = objSlaesActivitiesBL.GetAllDropDownDetailForSalesChallan(strBranchID);

            if (dst.Tables[1] != null && dst.Tables[1].Rows.Count > 0)
            {
                cdCmbBranch.TextField = "branch_description";
                cdCmbBranch.ValueField = "branch_id";
                cdCmbBranch.DataSource = dst.Tables[1];
                cdCmbBranch.DataBind();
                cdCmbBranch.Items.Insert(0, new ListEditItem("--Select--", "0"));
                cdCmbBranch.SelectedIndex = 0;
            }



            dst.Clear();


            BindVehicle(strBranchID);

            if (Session["userbranchID"] != null)
            {
                //if (ddl_transferFrom_Branch.Items.Count > 0)
                if (cdCmbBranch.Items.Count > 0)
                {
                    int branchindex = 0;
                    int cnt = 0;

                    cdCmbBranch.Value = Convert.ToString(Session["userbranchID"]);



                    // branchindex = 70;
                    BindAreaPinDropdown(objCustomerDeliveryBL.GetAreaPinByBranch(strBranchID));
                }
            }
        }

        private void BindAreaPinDropdown(DataSet dst)
        {
            cdCmbArea.TextField = "area_name";
            cdCmbArea.ValueField = "area_id";
            cdCmbArea.DataSource = dst.Tables[0];
            cdCmbArea.DataBind();
            cdCmbArea.Items.Insert(0, new ListEditItem("", "0"));
            cdCmbArea.SelectedIndex = 0;

            cdCmbPin.TextField = "pin_code";
            cdCmbPin.ValueField = "pin_id";
            cdCmbPin.DataSource = dst.Tables[1];
            cdCmbPin.DataBind();
            cdCmbPin.Items.Insert(0, new ListEditItem("", "0"));
            cdCmbPin.SelectedIndex = 0;
        }

        //private void BindVehicle(DataTable dt)
        private void BindVehicle(string branchID)
        {
            DataSet dst = objCustomerDeliveryBL.GetVehicleDetails("0", branchID); //"0" referes all vehicles...
            if (dst.Tables[0].Rows.Count > 0)
            {
                //cdCmbVehicle.TextField = "vehicle_regNo";
                //cdCmbVehicle.ValueField = "vehicle_regNo";
                //cdCmbVehicle.DataSource = dst.Tables[0];
                //cdCmbVehicle.DataBind();

                ddl_vehicleNo.DataTextField = "vehicle_regNo";
                ddl_vehicleNo.DataValueField = "vehicle_regNo";
                ddl_vehicleNo.DataSource = dst.Tables[0];
                ddl_vehicleNo.DataBind();
            }
            //cdCmbVehicle.Items.Insert(0, new ListEditItem("", "0"));
            //cdCmbVehicle.SelectedIndex = 0;

            ddl_vehicleNo.Items.Insert(0,new ListItem("select","0"));
            ddl_vehicleNo.SelectedIndex = 0;
        }

        private void BindDriverDropdown(DataSet dst, string reqType = "")
        {
            
            if (dst.Tables.Count > 0)
            {
                if (dst.Tables[0].Rows.Count > 0)
                {
                    //cdCmbDriver.TextField = "Name";
                    //cdCmbDriver.ValueField = "InternalID";
                    //cdCmbDriver.DataSource = dst.Tables[0];
                    //cdCmbDriver.DataBind();
                    //cdCmbDriver.Items.Insert(0, new ListEditItem("--Select--", "0"));
                    //cdCmbDriver.SelectedIndex = 0;

                    ddl_DriverName.DataTextField = "Name";
                    ddl_DriverName.DataValueField = "InternalID";
                    ddl_DriverName.DataSource = dst.Tables[0];
                    ddl_DriverName.DataBind();

                    //if (Convert.ToString(cdCmbVehicle.Value).Trim() == "0" && reqType == "")
                    //{
                    //    //cdCmbDriver.Items.Clear();
                    //    cdCmbDriver.DataSource = null;
                    //    cdCmbDriver.DataBind();
                    //}
                }
                else
                {
                    //cdCmbDriver.Items.Clear();
                    //cdCmbDriver.DataSource = null;
                    //cdCmbDriver.DataBind();
                }
            }

        }


        [WebMethod]
        public static string GetPhoneNumberByDriver(string ddl_driver)
        {


            CustomerDeliveryBL objCustomerDeliveryBL = new CustomerDeliveryBL();
            string PhoneNo = string.Empty;
            DataTable dt = objCustomerDeliveryBL.GetDriverPhoneNo(ddl_driver);
            if (dt.Rows.Count > 0)
            {
                PhoneNo = Convert.ToString(dt.Rows[0]["PhoneNo"]);
            }



            return PhoneNo;
        }

        [WebMethod]
        public static List<VehicleMaster> GetVehicles(string ddl_vehicle,string BranchId)
        {


            CustomerDeliveryBL objCustomerDeliveryBL = new CustomerDeliveryBL();
            List<VehicleMaster> vehicles = new List<VehicleMaster>();
            DataSet ds = objCustomerDeliveryBL.GetDriverDetailsByVehicle(ddl_vehicle, BranchId);

            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    vehicles.Add(new VehicleMaster
                    {
                        InternalID = Convert.ToString(ds.Tables[0].Rows[i]["InternalID"]),
                        Name = Convert.ToString(ds.Tables[0].Rows[i]["Name"])
                    });
                }
            }



            return vehicles;
        }

        private DataTable GetSalesChallanList(string branchID, string enteredDate, string areaID, string pincode)
        {
            DataSet dst = objCustomerDeliveryBL.GetSalesChallanForCustDelivery(branchID, enteredDate, areaID, pincode);
            return dst.Tables[0];
        }

        private DataSet GetDeliveryList(string deliveryID)
        {

            DataSet dst = objCustomerDeliveryBL.GetCustomerDliveryDetailsListBackup(deliveryID, "0", "", "");
            return dst;
        }

        private void SetEditableData(DataSet dst)
        {
            cdtxtDeliveryNumber.Text = Convert.ToString(dst.Tables[0].Rows[0]["Delievry_Number"]);
            cdDeliveryDate.Date = Convert.ToDateTime(dst.Tables[0].Rows[0]["RawDeliveryDate"]);
            txt_Refference.Text = Convert.ToString(dst.Tables[0].Rows[0]["Delievry_Reference"]);
            txtRouteNo.Text = Convert.ToString(dst.Tables[0].Rows[0]["RouteNo"]);
            txtEWayBillNo.Text = Convert.ToString(dst.Tables[0].Rows[0]["EWayBillNo"]);
            string branchID = Convert.ToString(dst.Tables[0].Rows[0]["Delievry_BranchId"]);
            GetAllDropDownDetailForCustDelivery(branchID);

            branchID = oDBEngine.getBranch(branchID, "") + branchID;

            BindAreaPinDropdown(objCustomerDeliveryBL.GetAreaPinByBranch(branchID));

            BindVehicle(branchID);

            BindDriverDropdown(objCustomerDeliveryBL.GetDriverDetailsByVehicle(Convert.ToString(dst.Tables[0].Rows[0]["VehicleNumber"]), branchID), "E");

            cdCmbBranch.Value = Convert.ToString(dst.Tables[0].Rows[0]["Delievry_BranchId"]);
            //cdCmbVehicle.Value = Convert.ToString(dst.Tables[0].Rows[0]["VehicleNumber"]);
            //cdCmbDriver.Value = Convert.ToString(dst.Tables[0].Rows[0]["DriverID"]);

            ddl_vehicleNo.SelectedValue = Convert.ToString(dst.Tables[0].Rows[0]["VehicleNumber"]);
            ddl_DriverName.SelectedValue = Convert.ToString(dst.Tables[0].Rows[0]["DriverID"]);

            txtDriverPhoneNo.Text = Convert.ToString(dst.Tables[0].Rows[0]["DriverPhNo"]);
            cdCmbArea.Value = Convert.ToString(dst.Tables[0].Rows[0]["Area"]);
            cdCmbPin.Value = Convert.ToString(dst.Tables[0].Rows[0]["Pincode"]);
            txtByHand.Text = Convert.ToString(dst.Tables[0].Rows[0]["Delivery_ByHand"]);
        }

        #endregion

        #region ########### Grid #############

        //SUBHABRATA
        protected void grid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            string[] listData = e.Parameters.Split('~');
            DataTable dtDelChallanList = new DataTable();
            switch (listData[0])
            {
                case "BindMainGrid":

                    if (Session["sessCustDelChallanList"] != null)
                    {
                        dtDelChallanList = (DataTable)Session["sessCustDelChallanList"];

                        grid.DataSource = dtDelChallanList;
                        grid.DataBind();
                    }
                    break;
                case "Delete":
                    string challanId = listData[1];
                    string docType = listData[2];
                    if (Session["sessCustDelChallanList"] != null)
                    {
                        dtDelChallanList = (DataTable)Session["sessCustDelChallanList"];

                        for (int i = dtDelChallanList.Rows.Count - 1; i >= 0; i--)
                        {
                            DataRow dr = dtDelChallanList.Rows[i];
                            if (Convert.ToString(dr["DocDetID"]) == challanId.Trim() && Convert.ToString(dr["DocType"]) == docType.Trim())
                                dr.Delete();
                        }

                        dtDelChallanList.AcceptChanges();

                        Session.Add("sessCustDelChallanList", dtDelChallanList);

                        grid.DataSource = dtDelChallanList;
                        grid.DataBind();
                    }
                    break;
            }
        }

        protected void grid_DataBinding(object sender, EventArgs e)
        {
            if (Session["sessCustDelChallanList"] != null)
            {
                DataTable dtDelChallanList = (DataTable)Session["sessCustDelChallanList"];
                grid.DataSource = GetSalesChallanToList(dtDelChallanList);
            }
        }

        public IEnumerable GetSalesChallanToList(DataTable dtSaleChallan)
        {
            List<SalesChallan> salesChallanList = new List<SalesChallan>();
            //DataTable Orderdt = GetOrderData().Tables[0];
            DataColumnCollection dtC = dtSaleChallan.Columns;
            for (int i = 0; i < dtSaleChallan.Rows.Count; i++)
            {
                SalesChallan salesChallan = new SalesChallan();

                salesChallan.SrlNo = Convert.ToString(dtSaleChallan.Rows[i]["SrlNo"]);
                salesChallan.DocDetID = Convert.ToString(dtSaleChallan.Rows[i]["DocDetID"]);
                salesChallan.DocNo = Convert.ToString(dtSaleChallan.Rows[i]["DocNo"]);
                salesChallan.DocType = Convert.ToString(dtSaleChallan.Rows[i]["DocType"]);
                salesChallan.DocDate = Convert.ToString(dtSaleChallan.Rows[i]["DocDate"]);
                salesChallan.ProductDesc = Convert.ToString(dtSaleChallan.Rows[i]["ProductDesc"]);
                salesChallan.Qty = Convert.ToString(dtSaleChallan.Rows[i]["Qty"]);
                salesChallan.UOM_Name = Convert.ToString(dtSaleChallan.Rows[i]["UOM_Name"]);
                salesChallan.CustomerName = Convert.ToString(dtSaleChallan.Rows[i]["CustomerName"]);
                salesChallan.Brand_Name = Convert.ToString(dtSaleChallan.Rows[i]["Brand_Name"]);
                salesChallan.ProductClass_Description = Convert.ToString(dtSaleChallan.Rows[i]["ProductClass_Description"]);
                salesChallan.state = Convert.ToString(dtSaleChallan.Rows[i]["state"]);
                salesChallan.pin_code = Convert.ToString(dtSaleChallan.Rows[i]["pin_code"]);
                salesChallan.DeliveryDate = Convert.ToString(dtSaleChallan.Rows[i]["DeliveryDate"]);
                salesChallan.ProductSerials = Convert.ToString(dtSaleChallan.Rows[i]["ProductSerials"]);
                salesChallan.UniqueID = Convert.ToString(dtSaleChallan.Rows[i]["UniqueID"]);
                salesChallanList.Add(salesChallan);
            }
            return salesChallanList;
        }

        public class SalesChallan
        {
            public string SrlNo { get; set; }
            public string DocDetID { get; set; }
            public string DocNo { get; set; }
            public string DocType { get; set; }
            public string DocDate { get; set; }
            public string ProductDesc { get; set; }
            public string Qty { get; set; }
            public string UOM_Name { get; set; }
            public string CustomerName { get; set; }
            public string pin_code { get; set; }
            public string DeliveryDate { get; set; }
            public string state { get; set; }
            public string ProductClass_Description
            { get; set; }
            public string Brand_Name { get; set; }
            public string ProductSerials { get; set; }
            public string UniqueID { get; set; }
        }

        #endregion

        #region ########## POP-UP Grid #############

        protected void cgridProducts_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            string[] listData = e.Parameters.Split('~');

            switch (listData[0])
            {
                case "BindGrid":
                    grid_Products.JSProperties["cpEndCallBackReq"] = 1;
                    if (grid_Products.GetSelectedFieldValues("UniqueID").Count != 0)
                    {
                        string challanIds = string.Empty;
                        for (int i = 0; i < grid_Products.GetSelectedFieldValues("UniqueID").Count; i++)
                        {
                            challanIds += "," + grid_Products.GetSelectedFieldValues("UniqueID")[i];
                        }
                        challanIds = challanIds.TrimStart(',');

                        DataTable dtPopChallanList = (DataTable)Session["sessPopCustDelChallanList"];

                        DataTable dtDelChallanList = new DataTable();

                        //if (Session["sessCustDelChallanList"] != null)
                        //{
                        //    //dtDelChallanList = (DataTable)Session["sessCustDelChallanList"];
                        //    Session["sessCustDelChallanList"] = null;
                        //}
                        //else
                        //{
                        dtDelChallanList.Columns.Add("SrlNo", typeof(string));
                        dtDelChallanList.Columns.Add("DocNo", typeof(string));
                        dtDelChallanList.Columns.Add("DocType", typeof(string));
                        dtDelChallanList.Columns.Add("DocDetID", typeof(string));
                        dtDelChallanList.Columns.Add("DocDate", typeof(string));
                        dtDelChallanList.Columns.Add("ProductDesc", typeof(string));
                        dtDelChallanList.Columns.Add("Qty", typeof(string));
                        dtDelChallanList.Columns.Add("UOM_Name", typeof(string));
                        dtDelChallanList.Columns.Add("CustomerName", typeof(string));
                        dtDelChallanList.Columns.Add("Brand_Name", typeof(string));
                        dtDelChallanList.Columns.Add("ProductClass_Description", typeof(string));
                        dtDelChallanList.Columns.Add("state", typeof(string));
                        dtDelChallanList.Columns.Add("pin_code", typeof(string));
                        dtDelChallanList.Columns.Add("DeliveryDate", typeof(string));
                        dtDelChallanList.Columns.Add("ProductSerials", typeof(string));
                        dtDelChallanList.Columns.Add("UniqueID", typeof(string));
                        //}

                        foreach (string str in challanIds.Split(','))
                        {
                            string[] lstStr = str.Split('~');
                            // DataRow[] drMain = dtDelChallanList.Select("DocDetID = '" + Convert.ToString(lstStr[0]) + "' AND DocType = '" + Convert.ToString(lstStr[1]) + "'");
                            //if (drMain.Length < 1)
                            //{
                            //"Size >= 230 AND Sex = 'm'"
                            //DataRow[] dr = dtPopChallanList.Select("DocDetID = '" + Convert.ToString(lstStr[0]) + "'");
                            DataRow[] dr = dtPopChallanList.Select("DocDetID = '" + Convert.ToString(lstStr[0]) + "' AND DocType = '" + Convert.ToString(lstStr[1]) + "'");
                            DataRow drAdd = dtDelChallanList.NewRow();
                            drAdd["SrlNo"] = 1;
                            drAdd["DocNo"] = Convert.ToString(dr[0]["DocNo"]);
                            drAdd["DocType"] = Convert.ToString(dr[0]["DocType"]);
                            drAdd["DocDetID"] = Convert.ToString(dr[0]["DocDetID"]);
                            drAdd["DocDate"] = Convert.ToString(dr[0]["DocDate"]);
                            drAdd["ProductDesc"] = Convert.ToString(dr[0]["ProductDesc"]);
                            drAdd["Qty"] = Convert.ToString(dr[0]["Qty"]);
                            drAdd["UOM_Name"] = Convert.ToString(dr[0]["UOM_Name"]);
                            drAdd["CustomerName"] = Convert.ToString(dr[0]["CustomerName"]);
                            drAdd["Brand_Name"] = Convert.ToString(dr[0]["Brand_Name"]);
                            drAdd["ProductClass_Description"] = Convert.ToString(dr[0]["ProductClass_Description"]);
                            drAdd["state"] = Convert.ToString(dr[0]["state"]);
                            drAdd["pin_code"] = Convert.ToString(dr[0]["pin_code"]);
                            drAdd["DeliveryDate"] = Convert.ToString(dr[0]["DeliveryDate"]);
                            drAdd["ProductSerials"] = Convert.ToString(dr[0]["ProductSerials"]);
                            drAdd["UniqueID"] = Convert.ToString(dr[0]["UniqueID"]);

                            dtDelChallanList.Rows.Add(drAdd);
                            // }
                        }
                        Session.Add("sessCustDelChallanList", dtDelChallanList);
                        Session.Remove("sessPopCustDelChallanList");
                    }
                    break;

                case "LoadChallan":
                    //grid_Products.JSProperties["cp_DataFlag"] = "0";
                    var date = cdDeliveryDate.Date;
                    grid_Products.JSProperties["cpEndCallBackReq"] = 0;
                    string branchID = oDBEngine.getBranch(Convert.ToString(cdCmbBranch.Value), "") + Convert.ToString(cdCmbBranch.Value);
                    DataTable dtChallanList = GetSalesChallanList(branchID, cdDeliveryDate.Date.ToString("yyyy-MM-dd"), Convert.ToString(cdCmbArea.Value), Convert.ToString(cdCmbPin.Value));

                    if (dtChallanList.Rows.Count > 0)
                    {
                        Session.Add("sessPopCustDelChallanList", dtChallanList);
                        grid_Products.DataSource = dtChallanList;
                        grid_Products.DataBind();

                        // grid_Products.JSProperties["cp_DataFlag"] = "1";
                    }
                    else
                    {
                        Session.Add("sessPopCustDelChallanList", null);
                        grid_Products.DataSource = null;
                        grid_Products.DataBind();
                    }
                    break;
                case "SelectAndDeSelectProducts":
                    ASPxGridView gv = sender as ASPxGridView;
                    string command = e.Parameters.ToString();
                    string State = Convert.ToString(e.Parameters.Split('~')[1]);
                    if (State == "SelectAll")
                    {
                        for (int i = 0; i < gv.VisibleRowCount; i++)
                        {
                            gv.Selection.SelectRow(i);
                        }
                    }
                    if (State == "UnSelectAll")
                    {
                        for (int i = 0; i < gv.VisibleRowCount; i++)
                        {
                            gv.Selection.UnselectRow(i);
                        }
                    }
                    if (State == "Revart")
                    {
                        for (int i = 0; i < gv.VisibleRowCount; i++)
                        {
                            if (gv.Selection.IsRowSelected(i))
                                gv.Selection.UnselectRow(i);
                            else
                                gv.Selection.SelectRow(i);
                        }
                    }
                    break;
            }



        }

        protected void grid_Products_DataBinding(object sender, EventArgs e)
        {
            if (Session["sessPopCustDelChallanList"] != null)
            {
                DataTable dtCustDelSChallanList = (DataTable)Session["sessPopCustDelChallanList"];
                grid_Products.DataSource = GetSalesChallanToList(dtCustDelSChallanList);
            }
        }

        #endregion

        #region Unique Code Generated Section Start


        [WebMethod]
        public static string getSchemeType(string sel_scheme_id)
        {

            string strschematype = "", strschemalength = "", strschemavalue = "";


            //BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
            BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine();

            string[] schematypedtl = new string[] { };
            schematypedtl = sel_scheme_id.Split('~');
            string schematype = Convert.ToString(schematypedtl[0]);

            DataTable DT = objEngine.GetDataTable("tbl_master_Idschema", " schema_type,length ", " Id = " + Convert.ToInt32(schematype));

            for (int i = 0; i < DT.Rows.Count; i++)
            {
                strschematype = Convert.ToString(DT.Rows[i]["schema_type"]);
                strschemalength = Convert.ToString(DT.Rows[i]["length"]);
                strschemavalue = strschematype + "~" + strschemalength;
            }
            return Convert.ToString(strschemavalue);
        }


        public string checkNMakeJVCode(string manual_str, int sel_schema_Id)
        {

            //oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
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
                    prefCompCode = dtSchema.Rows[0]["prefix"].ToString();
                    sufxCompCode = dtSchema.Rows[0]["suffix"].ToString();
                    prefLen = Convert.ToInt32(prefCompCode.Length);
                    sufxLen = Convert.ToInt32(sufxCompCode.Length);

                    sqlQuery = "SELECT max(tjv.Delievry_Number) FROM tbl_trans_CustomerDelivery tjv WHERE dbo.RegexMatch('";
                    if (prefLen > 0)
                        sqlQuery += "^[" + prefCompCode + "]{" + prefLen + "}";
                    else if (scheme_type == 2)
                        sqlQuery += "^";
                    sqlQuery += "[0-9]{" + paddCounter + "}";
                    if (sufxLen > 0)
                        sqlQuery += "[" + sufxCompCode + "]{" + sufxLen + "}";
                    //sqlQuery += "?$', LTRIM(RTRIM(tjv.Delievry_Number))) = 1";
                    sqlQuery += "?$', LTRIM(RTRIM(tjv.Delievry_Number))) = 1 and Delievry_Number like '" + prefCompCode + "%'";
                    if (scheme_type == 2)
                        sqlQuery += " AND CONVERT(DATE, CreatedDate) = CONVERT(DATE, GETDATE())";
                    dtC = oDBEngine.GetDataTable(sqlQuery);

                    if (dtC.Rows[0][0].ToString() == "")
                    {
                        sqlQuery = "SELECT max(tjv.Delievry_Number) FROM tbl_trans_CustomerDelivery tjv WHERE dbo.RegexMatch('";
                        if (prefLen > 0)
                            sqlQuery += "^[" + prefCompCode + "]{" + prefLen + "}";
                        else if (scheme_type == 2)
                            sqlQuery += "^";
                        sqlQuery += "[0-9]{" + (paddCounter - 1) + "}";
                        if (sufxLen > 0)
                            sqlQuery += "[" + sufxCompCode + "]{" + sufxLen + "}";
                        // sqlQuery += "?$', LTRIM(RTRIM(tjv.Delievry_Number))) = 1";
                        sqlQuery += "?$', LTRIM(RTRIM(tjv.Delievry_Number))) = 1 and Delievry_Number like '" + prefCompCode + "%'";
                        if (scheme_type == 2)
                            sqlQuery += " AND CONVERT(DATE, CreatedDate) = CONVERT(DATE, GETDATE())";
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
                            UniqueQuotation = prefCompCode + paddedStr + sufxCompCode;
                            return "ok";
                        }
                    }
                    else
                    {
                        UniqueQuotation = startNo.PadLeft(paddCounter, '0');
                        UniqueQuotation = prefCompCode + paddedStr + sufxCompCode;
                        return "ok";
                    }
                }
                else
                {
                    sqlQuery = "SELECT Delievry_Number FROM tbl_trans_CustomerDelivery WHERE Delievry_Number LIKE '" + manual_str.Trim() + "'";
                    dtC = oDBEngine.GetDataTable(sqlQuery);
                    // duplicate manual entry check
                    if (dtC.Rows.Count > 0 && dtC.Rows[0][0].ToString().Trim() != "")
                    {
                        return "duplicate";
                    }

                    UniqueQuotation = manual_str.Trim();
                    return "ok";
                }
            }
            else
            {
                return "noid";
            }
        }

        #endregion Unique Code Generated Section End

        #region ####### CallBack Vehicle ############

        protected void CallBackVehicle_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            //string[] listData = e.Parameter.Split('~');

            //BindDriverDropdown(objCustomerDeliveryBL.GetDriverDetailsByVehicle(Convert.ToString(cdCmbVehicle.Value), Convert.ToString(cdCmbBranch.Value)));
        }

        #endregion


        [WebMethod]
        public static bool CheckUniqueCode(string GCHNo)
        {
            bool flag = false;
            BusinessLogicLayer.GenericMethod oGenericMethod = new BusinessLogicLayer.GenericMethod();
            try
            {
                MShortNameCheckingBL objShortNameChecking = new MShortNameCheckingBL();
                flag = objShortNameChecking.CheckUnique(GCHNo, "0", "GCHNo_Check");
            }
            catch (Exception ex)
            {

            }
            finally
            {
            }
            return flag;
        }

        #endregion
        /// ----------------------- END : SAMRAT ROY : ---------------------------

        #region  ####### Grid Event (Optional) ############

        //protected void grid_CustomUnboundColumnData(object sender, ASPxGridViewColumnDataEventArgs e)
        //{
        //    if (e.Column.FieldName == "Number")
        //    {
        //        e.Value = string.Format("Item #{0}", e.ListSourceRowIndex);
        //    }
        //    if (e.Column.FieldName == "Warehouse")
        //    {
        //        //e.Value = string.Format("Item #{0}", e.ListSourceRowIndex);
        //        //e.Row.Cells[6].Attributes.Add("onclick", "javascript:ShowMissingData('" + ContactID + "');");
        //    }

        //}
        //protected void grid_HtmlRowCreated(object sender, ASPxGridViewTableRowEventArgs e)
        //{
        //    //if (e.RowType == GridViewRowType.Data)
        //    //{
        //    //    string AssetVal = Request.QueryString["accountType"].ToString();
        //    //    string AssetVal = Convert.ToString(Request.QueryString["accountType"]);
        //    //    //string kv = e.GetValue("SubAccount_Code").ToString();
        //    //    string kv = Convert.ToString(e.GetValue("SubAccount_Code"));
        //    //    //string cellv = e.GetValue("SubAccount_MainAcReferenceID").ToString();
        //    //    string cellv = Convert.ToString(e.GetValue("SubAccount_MainAcReferenceID"));
        //    //    //string subaccountcode123 = Request.QueryString["accountcode"].ToString().Trim();
        //    //    string subaccountcode123 = Convert.ToString(Request.QueryString["accountcode"]).Trim();
        //    //    if (Segment == "5")
        //    //    {
        //    //        if (AssetVal == "Asset" && Segment == "5")
        //    //        {
        //    //            e.Row.Cells[6].Style.Add("cursor", "hand");
        //    //            // e.Row.Cells[12].Attributes.Add("onclick", "javascript:ShowMissingData('" + ContactID + "');");
        //    //            //e.Row.Cells[6].Attributes.Add("onclick", "javascript:showhistory('" + kv + cellv + "');");
        //    //            e.Row.Cells[6].Attributes.Add("onclick", "javascript:showhistory('" + ViewState["MainAccountCode"] + "-" + kv + "');");
        //    //            e.Row.Cells[6].ToolTip = "ADD/VIEW";
        //    //            e.Row.Cells[6].Style.Add("color", "Blue");
        //    //        }
        //    //        else
        //    //        {
        //    //            e.Row.Cells[5].Style.Add("cursor", "hand");
        //    //            // e.Row.Cells[12].Attributes.Add("onclick", "javascript:ShowMissingData('" + ContactID + "');");
        //    //            //e.Row.Cells[5].Attributes.Add("onclick", "javascript:showhistory('" + kv + cellv + "');");
        //    //            e.Row.Cells[5].Attributes.Add("onclick", "javascript:showhistory('" + ViewState["MainAccountCode"] + "-" + kv + "');");
        //    //            e.Row.Cells[5].ToolTip = "ADD/VIEW";
        //    //            e.Row.Cells[5].Style.Add("color", "Blue");
        //    //        }
        //    //    }
        //    //}
        //}
        //protected void grid_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        //{
        //    //if (e.DataColumn.FieldName == "Warehouse")
        //    //{
        //    //    //string kv = e.GetValue("SubAccount_Code").ToString();
        //    //    //string cellv = e.GetValue("SubAccount_MainAcReferenceID").ToString();
        //    //    string cellv = Convert.ToString(e.GetValue("SubAccount_MainAcReferenceID"));
        //    //    e.Cell.Style.Value = "cursor:pointer;color: #000099;text-align:center;";
        //    //    e.Cell.Attributes.Add("onclick", "ShowCustom()");
        //    //    //e.Cell.Attributes.Add("onclick", "ShowCustom('" + kv + "','" + Request.QueryString["id"].ToString() + "')");
        //    //    //e.Cell.Attributes.Add("onclick", "ShowCustom('" + kv + "','" + Convert.ToString(Request.QueryString["id"]) + "')");
        //    //}
        //}

        #endregion

    }
}