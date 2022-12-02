using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Linq;
using DevExpress.Web;
using BusinessLogicLayer;
using ClsDropDownlistNameSpace;
using EntityLayer.CommonELS;
using System.Web.Services;
using DataAccessLayer;


namespace ERP.OMS.Management.Master
{
    public partial class VehicleAddEdit : ERP.OMS.ViewState_class.VSPage
    {
        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);MULTI
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        clsDropDownList oclsDropDownList = new clsDropDownList();
        BusinessLogicLayer.RemarkCategoryBL reCat = new BusinessLogicLayer.RemarkCategoryBL();

        protected override void OnPreInit(EventArgs e)
        {
            if (!IsPostBack)
            {
                string sPath = Convert.ToString(HttpContext.Current.Request.Url);
                oDBEngine.Call_CheckPageaccessebility(sPath);
                if (Request.QueryString["id"] == "ADD")
                {
                    base.OnPreInit(e);
                }
            }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            BranchdataSource.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            branchdtl.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/Master/Vehicle.aspx");
            Session["CurrYear"] = Convert.ToString(DateTime.Now.Year);

            if (HttpContext.Current.Session["userid"] == null)
            {

            }





            if (!IsPostBack)
            {
               
        
                if (Convert.ToString(Request.QueryString["id"]) != "ADD")
                {
                    Keyval_internalId.Value = "VehicleMaster" + Convert.ToString(Request.QueryString["id"]);
                    lblAddEdit.Text = "Edit Vehicle Master";
                    HttpContext.Current.Session["VehicleListOrderBy"] = "ADD";
                    Session["KeyVal_InternalID"] = Convert.ToString(Request.QueryString["id"]);
                }
                else
                {
                    Keyval_internalId.Value = "Add";
                    Session["KeyVal_InternalID"] = "Add";
                    lblAddEdit.Text = "Add Vehicle Master";
                    ddlIsActive.SelectedIndex = ddlIsActive.Items.IndexOf(ddlIsActive.Items.FindByText("YES"));
                    TabPage page = ASPxPageControl1.TabPages.FindByName("Documents");
                    page.Visible = false;

                }

                //Populating Branch DDL
                //string[,] Data = oDBEngine.GetFieldValue("tbl_master_branch", "branch_id,(rtrim(ltrim(branch_description))+' ['+rtrim(ltrim(branch_code))+']') as branch_description", null, 2, "branch_description");
                //oclsDropDownList.AddDataToDropDownList(Data, cmbBranch);
                //cmbBranch.Items.Insert(0, "Select");


                branchdtl.SelectCommand = "select '0' as branch_id ,  '--ALL--' as branch_description union all select branch_id,branch_description from tbl_master_branch order by branch_description";
                cmbMultiBranches.DataBind();
                cmbMultiBranches.Value = "0";

                
               
            }

            if (Convert.ToString(Request.QueryString["id"]) != "ADD")
            {
                txtVehRegNo.Enabled = false;
                if (!IsPostBack)
                    ShowForm();
                
            }
            else
                txtVehRegNo.Enabled = true;

           // ShowSelectedBranchName();
        }


        private void SetBranchRecordToSessionTable(string Keyvalue)
        {
            DataTable branchListtable = oDBEngine.GetDataTable("select branch_id Branch_id from tbl_master_VehicleBranch_map where VehicleId='" + Keyvalue + "'");

           // DataTable branchListtable = oDBEngine.GetDataTable("select m.branch_id Branch_id,b.branch_description  from tbl_master_VehicleBranch_map m left join tbl_master_branch b on m.branch_id=b.branch_id where m.VehicleId='" + Keyvalue + "'");
            
            Session["BranchListTableForVehicle"] = branchListtable;
        }

        private void ShowBranchName(string Keyvalue)
        {
            string SelectedBranch = string.Empty;
             DataTable branchListtable = oDBEngine.GetDataTable("select m.branch_id Branch_id,b.branch_description  from tbl_master_VehicleBranch_map m left join tbl_master_branch b on m.branch_id=b.branch_id where m.VehicleId='" + Keyvalue + "'");

             if (branchListtable != null && branchListtable.Rows.Count > 0 && Convert.ToString(branchListtable.Rows[0]["Branch_Id"])=="0")
             { lblSelectedBranch.Text = "All Branch"; }
             else
             {
                 if (branchListtable != null)
                 {
                     foreach (DataRow dr in branchListtable.Rows)
                     {

                         SelectedBranch = SelectedBranch + ", " + dr["branch_description"];
                     }
                 }
                 if (SelectedBranch.Length > 1)
                 {
                     lblSelectedBranch.Text = SelectedBranch.Substring(1, SelectedBranch.Length - 1);
                 }
                 else
                 {
                     lblSelectedBranch.Text = "";
                 }
             }
            
           
        }
        protected void branchGrid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            string receviedString = e.Parameters;
            branchGrid.JSProperties["cpReceviedString"] = receviedString;


            if (receviedString == "SelectAllBranchesFromList")
            {
                branchGrid.Selection.SelectAll();
            }

            if (receviedString == "ClearSelectedBranch")
            {
                branchGrid.Selection.UnselectAll();
            }


            if (receviedString == "SetAllRecordToDataTable")
            {
                List<object> branchList = branchGrid.GetSelectedFieldValues("branch_id");
                CreateBranchTable();
                DataTable branchListtable = (DataTable)Session["BranchListTableForVehicle"];
                foreach (object obj in branchList)
                {
                    if (Convert.ToInt32(obj) != 0)
                        branchListtable.Rows.Add(Convert.ToInt32(obj));

                }

                if (hdnBranchAllSelected.Value == "0")
                {
                    if (branchListtable.Rows.Count>0)
                    {
                        branchGrid.JSProperties["cpBrselected"] = 1;

                    }
                    else { branchGrid.JSProperties["cpBrselected"] = 0; }
                   // Session["BranchListTableForVehicle"] = "";
                    
                }

                //else
                //{
                //    if (branchListtable.Rows.Count > 0)
                //    {
                //        branchGrid.JSProperties["cpBrselected"] = 1;

                //    }
                //    else { branchGrid.JSProperties["cpBrselected"] = 0; }
                //}
                //else
                //{
                    Session["BranchListTableForVehicle"] = branchListtable;
                //}
            }
            else if (receviedString == "SetAllSelectedRecord")
            {
                //ShowSelectedBranchName();
               
                DataTable branchListtable = (DataTable)Session["BranchListTableForVehicle"];
                branchGrid.Selection.UnselectAll();
                if (branchListtable != null)
                {
                    foreach (DataRow dr in branchListtable.Rows)
                    {
                        branchGrid.Selection.SelectRowByKey(dr["Branch_id"]);

                        if (Convert.ToString(dr["Branch_id"]) == "0")
                        {
                            //chkAllBranch.Checked = true; 
                           
                                branchGrid.JSProperties["cpBrChecked"] = 1;

                                //hdnBranchAllSelected.Value = "0";
                        }
                       // else { hdnBranchAllSelected.Value = "1"; }
                    }
                }
               
            }


        }

        //public void ShowSelectedBranchName()
        //{
        //    string SelectedBranch = string.Empty;
        //    DataTable branchListtable = (DataTable)Session["BranchListTableForVehicle"];
        //   // branchGrid.Selection.UnselectAll();
        //    if (branchListtable != null)
        //    {
        //        foreach (DataRow dr in branchListtable.Rows)
        //        {
        //            //branchGrid.Selection.SelectRowByKey(dr["Branch_id"]);
        //           // SelectedBranch = SelectedBranch + ", "+ dr["branch_description"];
        //        }
        //    }
        //    //if (SelectedBranch.Length > 1)
        //    //{
        //    //    lblSelectedBranch.Text = SelectedBranch.Substring(1, SelectedBranch.Length - 1);
        //    //}
        //    //else
        //    //{
        //    //    lblSelectedBranch.Text = "";
        //    //}
        //}
        public void CreateBranchTable()
        {
            DataTable branchListtable = new DataTable();
            branchListtable.Columns.Add("Branch_id", typeof(System.Int32));
            Session["BranchListTableForVehicle"] = branchListtable;
        }
        public string GetBranchList()
        {
            DataTable branchListtable = (DataTable)Session["BranchListTableForVehicle"];
            string branchlist = "";
            if (branchListtable != null)
            {
                foreach (DataRow dr in branchListtable.Rows)
                {
                    branchlist = branchlist + "," + Convert.ToString(dr["Branch_id"]);

                }
            }

            branchlist = branchlist.TrimStart(',');
            return branchlist;
        }
        protected void ShowForm()
        {
            if (Convert.ToString(Request.QueryString["id"]) == "ADD")
            {

            }
            else
            {
                if (Request.QueryString["id"] != null)
                {
                    
                    SetBranchRecordToSessionTable(Convert.ToString(Request.QueryString["id"]));
                                        
                    //Session["cnt_id"] = Convert.ToString(Request.QueryString["id"]);                                                                                  --5                                                                                                                                                                                                                               
                    string[,] DT = oDBEngine.GetFieldValue("tbl_master_vehicle ", "vehicle_regNo, vehicle_engineNo, vehicle_engineCC, vehicle_Type, vehicle_maker, vehicle_model, vehicle_yearReg, vehicle_fuelType, vehicle_isActive, vehicle_LogBookStatus, vehicle_AllotedTo, vehicle_isFleetCardApplied, vehicle_FleetCardNumber, vehicle_HappyCard, vehicle_InsurerName, vehicle_PolicyNo, vehicle_PolicyValidUpto, vehicle_InsuranceGivenTo, vehicle_TaxTokenNo, vehicle_TaxValidUpto, vehicle_PollutionCaseDtl, vehicle_PollutionCertValidUpto, vehicle_isAuthLetter, vehicle_AuthLetterValidUpto, vehicle_BlueBook, vehicle_CFDetails, vehicle_CFValidUpto, vehicle_vehOwnerType, vehicle_isGPSInstalled, vehicle_ChassisNo, vehicle_Pollution,ByHand", "vehicle_Id=" + Request.QueryString["id"].ToString(), 32);
                    if (DT[0, 0] != "n")
                        HttpContext.Current.Session["KeyVal"] = Convert.ToString(Request.QueryString["id"]);

                    txtVehRegNo.Text = DT[0, 0];
                    lblHeader.Text = DT[0, 0];
                    txtEngineNo.Text = DT[0, 1];
                    txtChassisNo.Text = DT[0, 29];
                    ddlVehicleType.SelectedIndex = ddlVehicleType.Items.IndexOf(ddlVehicleType.Items.FindByValue(Convert.ToString(DT[0, 3])));
                    txtVehicleMaker.Text = DT[0, 4];
                    txtVehicleModel.Text = DT[0, 5];
                    txtRegnYear.Text = DT[0, 6];
                    ddlOwnershipType.SelectedIndex = ddlOwnershipType.Items.IndexOf(ddlOwnershipType.Items.FindByValue(Convert.ToString(DT[0, 27])));
                    txtEngineCC.Text = DT[0, 2];


                    ddlGPSInstallationStatus.SelectedIndex = ddlGPSInstallationStatus.Items.IndexOf(ddlGPSInstallationStatus.Items.FindByValue(Convert.ToString(DT[0, 28])));
                    ddlLogBookStatus.SelectedIndex = ddlLogBookStatus.Items.IndexOf(ddlLogBookStatus.Items.FindByValue(Convert.ToString(DT[0, 9])));
                   
                   // cmbBranch.SelectedIndex = cmbBranch.Items.IndexOf(cmbBranch.Items.FindByValue(DT[0, 10]));
                    cmbMultiBranches.SelectedIndex = cmbMultiBranches.Items.IndexOf(cmbMultiBranches.Items.FindByValue(DT[0, 10]));

                    ddlFleetCardApplied.SelectedIndex = ddlFleetCardApplied.Items.IndexOf(ddlFleetCardApplied.Items.FindByValue(Convert.ToString(DT[0, 11])));
                    txtFleetCardNo.Text = DT[0, 12];
                    txtHappyCard.Text = DT[0, 13];
                    txtInsName.Text = DT[0, 14];
                    txtInsPolicyNo.Text = DT[0, 15];
                    if (DT[0, 16] != "")
                    { txtInsValidUpto.Date = Convert.ToDateTime(DT[0, 16]); }
                    else
                    { txtInsValidUpto.Text = ""; }
                    txtInsGivenTo.Text = DT[0, 17];


                    txtTaxTokenNo.Text = DT[0, 18];
                    if (DT[0, 19] != "")
                    { ASPxDateEditTaxValidUpto.Date = Convert.ToDateTime(DT[0, 19]); }
                    else
                    { ASPxDateEditTaxValidUpto.Text = ""; }
                    txtPollution.Text = DT[0, 30];
                    if (DT[0, 21] != "")
                    { ASPxDateEditPolluValidUpto.Date = Convert.ToDateTime(DT[0, 21]); }
                    else
                    { ASPxDateEditPolluValidUpto.Text = ""; }
                    txtPollutionCaseDtl.Text = DT[0, 20];
                    ddlAuthLettr.SelectedIndex = ddlAuthLettr.Items.IndexOf(ddlAuthLettr.Items.FindByValue(Convert.ToString(DT[0, 22])));
                    if (DT[0, 23] != "")
                    { ASPxDateAuthLttrValidUpto.Date = Convert.ToDateTime(DT[0, 23]); }
                    else
                    { ASPxDateAuthLttrValidUpto.Text = ""; }
                    ddlBlueBook.SelectedIndex = ddlBlueBook.Items.IndexOf(ddlBlueBook.Items.FindByValue(Convert.ToString(DT[0, 24])));
                    txtCFCaseDetails.Text = DT[0, 25];
                    if (DT[0, 26] != "")
                    { ASPxDateCFValidUpto.Date = Convert.ToDateTime(DT[0, 26]); }
                    else
                    { ASPxDateCFValidUpto.Text = ""; }
                    //This field is shown Vehicle.aspx page in the grid.
                    ddlFuelType.SelectedIndex = ddlFuelType.Items.IndexOf(ddlFuelType.Items.FindByText(Convert.ToString(DT[0, 7])));

                    if (DT[0, 8] != "")
                    {
                        ddlIsActive.SelectedIndex = ddlIsActive.Items.IndexOf(ddlIsActive.Items.FindByValue(Convert.ToString(DT[0, 8])));
                    }
                    else
                    {
                        ddlIsActive.SelectedIndex = 0;
                    }
                    chkByHand.Checked = Convert.ToBoolean(DT[0, 31]);
                   

                    SetBranchRecordToSessionTable(Convert.ToString(HttpContext.Current.Session["KeyVal"]));

                    ShowBranchName(Convert.ToString(HttpContext.Current.Session["KeyVal"]));
                }
            }
        }


        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("Vehicle.aspx");
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                VehicleBL objVehicleBL = new VehicleBL();

                if (Convert.ToString(Request.QueryString["id"]) == "ADD")
                {

                    string vehRegnNo = txtVehRegNo.Text.Trim().ToUpper();
                    if (txtVehRegNo.Text.Trim() == "")
                    {
                        //ScriptManager.RegisterStartupScript(this, this.GetType(), "Notify", "alert('Please enter vehicle registration number!');", true);
                        //return;
                    }
                    string engineNo = txtEngineNo.Text.Trim().ToUpper();
                    string chasisNo = txtChassisNo.Text.Trim().ToUpper();
                    string vehType = Convert.ToString(ddlVehicleType.SelectedItem.Value).Trim();

                    string vehMake = txtVehicleMaker.Text.Trim();
                    string vehModel = txtVehicleModel.Text.Trim();
                    int regnYear = Convert.ToInt32(txtRegnYear.Text.Trim() == "" ? "0" : txtRegnYear.Text.Trim());
                    string ownr = Convert.ToString(ddlOwnershipType.SelectedItem.Value).Trim();
                    string engineCC = txtEngineCC.Text.Trim();

                    string isGPS = Convert.ToString(ddlGPSInstallationStatus.SelectedItem.Value).Trim();
                    string logBook = Convert.ToString(ddlLogBookStatus.SelectedItem.Value).Trim();
                    //string vehAllotTo = Convert.ToString(cmbBranch.SelectedItem.Value).Trim();
                    string vehAllotTo = Convert.ToString(cmbMultiBranches.SelectedItem.Value).Trim();

                    string fleetCardApplied = Convert.ToString(ddlFleetCardApplied.SelectedItem.Value).Trim();
                    string fleetCardNo = txtFleetCardNo.Text.Trim();
                    string hppyCard = txtHappyCard.Text.Trim();

                    string insurName = txtInsName.Text.Trim().ToUpper();
                    string insurPolicyNo = txtInsPolicyNo.Text.Trim().ToUpper();
                    string insurValidUpto = "";
                    if (txtInsValidUpto.Value != null)
                        insurValidUpto = Convert.ToString(txtInsValidUpto.Date); // Datetime
                    string insurGivenTo = txtInsGivenTo.Text.Trim();

                    string taxTokenNo = txtTaxTokenNo.Text.Trim().ToUpper();
                    string tokenValidUpto = "";
                    if (ASPxDateEditTaxValidUpto.Value != null)
                        tokenValidUpto = Convert.ToString(ASPxDateEditTaxValidUpto.Date);// Datetime
                    string Pollution = txtPollution.Text.Trim();
                    string pollutValidUpto = "";
                    if (ASPxDateEditPolluValidUpto.Value != null)
                        pollutValidUpto = Convert.ToString(ASPxDateEditPolluValidUpto.Date);// Datetime
                    string pollutCaseDtl = Convert.ToString(txtPollutionCaseDtl.Text.Trim());
                    string isAuthLettr = Convert.ToString(ddlAuthLettr.SelectedItem.Value).Trim();
                    string authLettrValidUpto = "";
                    if (ASPxDateAuthLttrValidUpto.Value != null)
                        authLettrValidUpto = Convert.ToString(ASPxDateAuthLttrValidUpto.Date);// Datetime
                    string blueBook = Convert.ToString(ddlBlueBook.SelectedItem.Value).Trim();
                    string cfCaseDtl = txtCFCaseDetails.Text.Trim();
                    string cfValidUpto = "";
                    if (ASPxDateCFValidUpto.Value != null)
                        cfValidUpto = Convert.ToString(ASPxDateCFValidUpto.Date); // Datetime

                    string vehFuelType = Convert.ToString(ddlFuelType.SelectedItem.Text).Trim();
                    string vehIsActive = Convert.ToString(ddlIsActive.SelectedItem.Value).Trim();
                    int usrID = Convert.ToInt32(HttpContext.Current.Session["userid"]);
                    
                    Boolean IsByHand = chkByHand.Checked;


                    int VID = objVehicleBL.InsertUpdateVehicleMaster("ADD", vehRegnNo, engineNo, chasisNo, vehType, vehMake, vehModel, regnYear, ownr, engineCC, isGPS, logBook, vehAllotTo,
                                                                           fleetCardApplied, fleetCardNo, hppyCard, insurName, insurPolicyNo, insurValidUpto, insurGivenTo,
                                                                           taxTokenNo, tokenValidUpto, Pollution, pollutValidUpto, pollutCaseDtl, isAuthLettr, authLettrValidUpto,
                                                                           blueBook, cfCaseDtl, cfValidUpto, vehFuelType, vehIsActive, usrID, IsByHand);



                     //Add branches for Vehicle
                    string BranchList = GetBranchList();
                    //if (BranchList != "")
                    //{
                        int brmap = reCat.insertVehicleBranchMap(BranchList, Convert.ToString(VID), Convert.ToInt16(vehAllotTo));
                    //}
                   
                    HttpContext.Current.Session["VehicleListOrderBy"] = "ADD"; //To make the Listing Order by in the Vehicle List page
                    if (VID > 0)
                    {                        
                        Page.ClientScript.RegisterStartupScript(GetType(), "pagecall", "<script>popUpRedirect('Vehicle.aspx');</script>");
                    }
                }
                else
                {

                    if (Request.QueryString["id"] != null)
                    {
                        int vehID = Convert.ToInt32(Request.QueryString["id"].ToString());
                        string vehRegnNo = txtVehRegNo.Text.Trim().ToUpper();

                        string engineNo = txtEngineNo.Text.Trim().ToUpper();
                        string chasisNo = txtChassisNo.Text.Trim().ToUpper();
                        string vehType = Convert.ToString(ddlVehicleType.SelectedItem.Value).Trim(); //1

                        string vehMake = txtVehicleMaker.Text.Trim();
                        string vehModel = txtVehicleModel.Text.Trim();
                        int regnYear = Convert.ToInt32(txtRegnYear.Text.Trim() == "" ? "0" : txtRegnYear.Text.Trim());
                        string ownr = Convert.ToString(ddlOwnershipType.SelectedItem.Value).Trim();//10
                        string engineCC = txtEngineCC.Text.Trim();

                        string isGPS = Convert.ToString(ddlGPSInstallationStatus.SelectedItem.Value).Trim();//2

                        string logBook = Convert.ToString(ddlLogBookStatus.SelectedItem.Value).Trim();//3

                        //string vehAllotTo = Convert.ToString(cmbBranch.SelectedItem.Value).Trim(); 
                        string vehAllotTo = "0"; Convert.ToString(cmbMultiBranches.SelectedItem.Value).Trim();

                        string fleetCardApplied = Convert.ToString(ddlFleetCardApplied.SelectedItem.Value).Trim(); //4
                        string fleetCardNo = txtFleetCardNo.Text.Trim();
                        string hppyCard = txtHappyCard.Text.Trim();

                        string insurName = txtInsName.Text.Trim().ToUpper();
                        string insurPolicyNo = txtInsPolicyNo.Text.Trim().ToUpper();
                        string insurValidUpto = "";
                        if (txtInsValidUpto.Value != null)
                            insurValidUpto = Convert.ToString(txtInsValidUpto.Date); // Datetime
                        string insurGivenTo = txtInsGivenTo.Text.Trim();

                        string taxTokenNo = txtTaxTokenNo.Text.Trim().ToUpper();
                        string tokenValidUpto = "";
                        if (ASPxDateEditTaxValidUpto.Value != null)
                            tokenValidUpto = Convert.ToString(ASPxDateEditTaxValidUpto.Date);// Datetime
                        string Pollution = txtPollution.Text.Trim();
                        string pollutValidUpto = "";
                        if (ASPxDateEditPolluValidUpto.Value != null)
                            pollutValidUpto = Convert.ToString(ASPxDateEditPolluValidUpto.Date);// Datetime
                        string pollutCaseDtl = Convert.ToString(txtPollutionCaseDtl.Text.Trim());
                        string isAuthLettr = Convert.ToString(ddlAuthLettr.SelectedItem.Value).Trim(); //5
                        string authLettrValidUpto = "";
                        if (ASPxDateAuthLttrValidUpto.Value != null)
                            authLettrValidUpto = Convert.ToString(ASPxDateAuthLttrValidUpto.Date);// Datetime
                        string blueBook = Convert.ToString(ddlBlueBook.SelectedItem.Value).Trim(); //6
                        string cfCaseDtl = txtCFCaseDetails.Text.Trim();
                        string cfValidUpto = "";
                        if (ASPxDateCFValidUpto.Value != null)
                            cfValidUpto = Convert.ToString(ASPxDateCFValidUpto.Date); // Datetime

                        string vehFuelType = Convert.ToString(ddlFuelType.SelectedItem.Text).Trim(); //7

                        string vehIsActive = Convert.ToString(ddlIsActive.SelectedItem.Value).Trim(); //8

                        int usrID = Convert.ToInt32(HttpContext.Current.Session["userid"]);
                        Boolean IsByHand = chkByHand.Checked;

                        int retData = objVehicleBL.UpdateVehicleMaster("Edit",
                                                                        vehID, vehRegnNo, engineNo, chasisNo, vehType, vehMake, vehModel, regnYear, ownr, engineCC, isGPS,
                                                                        logBook, vehAllotTo, fleetCardApplied, fleetCardNo, hppyCard, insurName, insurPolicyNo, insurValidUpto, insurGivenTo, taxTokenNo, tokenValidUpto,
                                                                        Pollution, pollutValidUpto, pollutCaseDtl, isAuthLettr, authLettrValidUpto, blueBook, cfCaseDtl, cfValidUpto, vehFuelType, vehIsActive, usrID, IsByHand);


                        // Add branches for Vehicle
                        string BranchList = GetBranchList();

                        if (hdnBranchAllSelected.Value == "0")
                        {
                            BranchList = "0";
                        }
                        //if (BranchList != "")
                        //{
                            int brmap = reCat.insertVehicleBranchMap(BranchList, Convert.ToString(HttpContext.Current.Session["KeyVal"]), Convert.ToInt16(vehAllotTo));
                        //}


                        HttpContext.Current.Session["VehicleListOrderBy"] = "EDIT"; //To make the Vehicle List page Listing Order by LastEditedOrder

                        if (retData == 2)
                        {
                            Page.ClientScript.RegisterStartupScript(GetType(), "pagecall", "<script>popUpRedirect('Vehicle.aspx');</script>");
                        }
                    }
                }
            }
            catch { }
            finally { }


        }

        [WebMethod]
        public static bool checkRegNoAvailability(string RegistrationNo)
        {
            Boolean status = false;
            VehicleBL vehBLObject = new VehicleBL();
            if (RegistrationNo.Trim() != "")
            {
                if (vehBLObject.isRegNoAvailable(RegistrationNo) > 0)
                {
                    status = true;
                }
                else
                {
                    status = false;
                }
            }
            else
            {
                status = true;
            }
            return status;
        }


        [WebMethod]
        public static bool checkEngineNoAvailability(string EngineNo)
        {
            Boolean status = false;
            VehicleBL vehBLObject = new VehicleBL();
            if (EngineNo.Trim() != "")
            {
                if (vehBLObject.isEngNoAvailable(EngineNo) > 0)
                {
                    status = true;
                }
                else
                {
                    status = false;
                }
            }
            else
            {
                status = false;
            }
            return status;
        }


        [WebMethod]
        public static bool checkChassNoAvailability(string ChassisNo)
        {
            Boolean status = false;
            VehicleBL vehBLObject = new VehicleBL();
            if (ChassisNo.Trim() != "")
            {
                if (vehBLObject.isChassNoAvailable(ChassisNo) > 0)
                {
                    status = true;
                }
                else
                {
                    status = false;
                }
            }
            else
            {
                status = false;
            }
            return status;
        }

    }
}