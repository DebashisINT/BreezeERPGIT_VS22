using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;
using BusinessLogicLayer;
using ClsDropDownlistNameSpace;
using EntityLayer.CommonELS;
using System.Web.Services;
using System.Collections.Generic;
using DataAccessLayer;
using System.Collections;
using System.Linq;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using System.Net.Http;

namespace ERP.OMS.Management.Master
{
    public partial class DocumentSegmentAdd : System.Web.UI.Page
    {
        //int DocumentSegmentId = 0, ErrorCode = 0;
        CommonBL CBL = new CommonBL();
        protected void Page_PreInit(object sender, EventArgs e) // lead add
        {
            this.Page.MasterPageFile = "~/OMS/MasterPage/PopUp.Master";
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            string SyncUsertoWhileCreating = CBL.GetSystemSettingsResult("SyncCustomertoFSMWhileCreating");
            hdnSyncCustomertoFSMWhileCreating.Value = SyncUsertoWhileCreating;
            if (!IsPostBack)
            {
                if (Request.QueryString["CustomerID"] != null)
                {
                    string CustId = Request.QueryString["CustomerID"];
                    hdnCustomerID.Value = CustId;

                    //   DataSet allDetails = PopulateDetails();

                    //ddlSegment.DataSource = allDetails.Tables[0];
                    //ddlSegment.ValueField = "ID";
                    //ddlSegment.TextField = "Segment";
                    //ddlSegment.DataBind();

                    //cmbParent.DataSource = allDetails.Tables[0];
                    //cmbParent.ValueField = "ID";
                    //cmbParent.TextField = "Segment";
                    //cmbParent.DataBind();
                }

                AllBranchBind();

                if (Request.QueryString["CustomerID"] != null)
                {
                    hdnCustomerID.Value = Request.QueryString["CustomerID"];
                    hdAddEdit.Value = "Add";
                    hdDocumentSegmentId.Value = "";
                }

                if (Request.QueryString["Key"] != null)
                {
                    string DocumentSegmentId = Request.QueryString["Key"];

                    hdAddEdit.Value = "Edit";
                    lblHeading.Text = "Modify Document Segment";
                    hdDocumentSegmentId.Value = DocumentSegmentId;

                    DataTable allEditDetails = GetCustomer_cnt_id(DocumentSegmentId);
                    if (allEditDetails.Rows.Count > 0)
                    {
                        hdnCustomerID.Value = Convert.ToString(allEditDetails.Rows[0]["cnt_id"]);
                    }
                    //DataRow HeaderRow = allEditDetails.Tables[0].Rows[0];

                    //txtcode.Text = Convert.ToString(HeaderRow["Code"]);
                    //txtName.Text = Convert.ToString(HeaderRow["Name"]);
                    //txtAddress1.Text = Convert.ToString(HeaderRow["Address"]);
                    //cmbCountry.Value = Convert.ToString(HeaderRow["CountryId"]);
                    //hdnCustomerID.Value = Convert.ToString(HeaderRow["cnt_id"]);

                    //String GSTIN = Convert.ToString(HeaderRow["GSTIN"]);
                    //if (GSTIN!="")
                    //{
                    //    txtGSTIN1.Text = GSTIN.Substring(0, 2);
                    //    txtGSTIN2.Text = GSTIN.Substring(2, 10);
                    //    txtGSTIN3.Text = GSTIN.Substring(12, 3);
                    //}



                    //cmbState.DataSource = allEditDetails.Tables[2];
                    //cmbState.ValueField = "ID";
                    //cmbState.TextField = "State";
                    //cmbState.DataBind();

                    //cmbDistrict.DataSource = allEditDetails.Tables[3];
                    //cmbDistrict.ValueField = "CityId";
                    //cmbDistrict.TextField = "City";
                    //cmbDistrict.DataBind();

                    //cmbPincode.DataSource = allEditDetails.Tables[4];
                    //cmbPincode.ValueField = "pin_id";
                    //cmbPincode.TextField = "pin_code";
                    //cmbPincode.DataBind();

                    //ddlSegment.DataSource = allEditDetails.Tables[5];
                    //ddlSegment.ValueField = "ID";
                    //ddlSegment.TextField = "Segment";
                    //ddlSegment.DataBind();

                    //cmbParent.DataSource = allEditDetails.Tables[5];
                    //cmbParent.ValueField = "ID";
                    //cmbParent.TextField = "Segment";
                    //cmbParent.DataBind();


                    //cmbState.Value = Convert.ToString(HeaderRow["StateID"]);
                    //cmbDistrict.Value = Convert.ToString(HeaderRow["CityID"]);
                    //cmbPincode.Value = Convert.ToString(HeaderRow["PinCode"]);

                    //ddlSegment.Value = Convert.ToString(HeaderRow["Segment_ID"]);
                    //cmbParent.Value = Convert.ToString(HeaderRow["ParentSegment_ID"]);

                    //string IsMandatory = Convert.ToString(HeaderRow["IsMandatory"]);

                    //ChkIsMandatory.Value = IsMandatory;
                    //if (IsMandatory == "True")
                    //{
                    //    ChkIsMandatory.Checked = true;
                    //}
                    //else
                    //{
                    //    ChkIsMandatory.Checked = false;
                    //}

                }
                if (Request.QueryString["req"] != null && Request.QueryString["req"] == "V")
                {
                    lblHeading.Text = "View Document Segment";
                    //  btnSaveCust.Visible = false;

                }

            }
        }

        public void AllBranchBind()
        {

            DataTable BranchDT = GetAllBranch();

            if (BranchDT != null && BranchDT.Rows.Count > 0)
            {
                ddlServiceBranch.DataTextField = "branch_description";
                ddlServiceBranch.DataValueField = "branch_id";
                ddlServiceBranch.DataSource = BranchDT;
                ddlServiceBranch.DataBind();
            }

        }
        public DataTable GetAllBranch()
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_SalesCRM_Details");
            proc.AddVarcharPara("@Action", 100, "GetAllBranchDocument");
            ds = proc.GetTable();
            return ds;
        }
        public DataTable GetCustomer_cnt_id(string DocumentSegmentId)
        {
            ProcedureExecute proc = new ProcedureExecute("prc_ENTITY_SEGMENT_MAP");
            proc.AddVarcharPara("@Action", 50, "GetCustomer_cnt_id");
            proc.AddVarcharPara("@Segment_Map_ID", 50, DocumentSegmentId);
            return proc.GetTable();
        }
        public DataSet PopulateDetails()
        {
            ProcedureExecute proc = new ProcedureExecute("prc_ENTITY_SEGMENT_MAP");
            proc.AddVarcharPara("@Action", 50, "PageLoadData");
            //proc.AddVarcharPara("@companyId", 20, Convert.ToString(HttpContext.Current.Session["LastCompany"]));
            //proc.AddVarcharPara("@FinYear", 10, Convert.ToString(HttpContext.Current.Session["LastFinYear"]));
            //proc.AddVarcharPara("@BranchList", -1, Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]));
            proc.AddVarcharPara("@InetrnalId", 50, hdnCustomerID.Value);
            return proc.GetDataSet();
        }

        //protected void btnSave_Click(object sender, EventArgs e)
        //{
        //    string SegmentID=Convert.ToString(ddlSegment.Value);
        //    string ParentSegmentID = Convert.ToString(cmbParent.Value);
        //    string Code = txtcode.Text;
        //    string Name = txtName.Text;
        //    string Address1 = txtAddress1.Text;
        //    string Address2 = txtAddress2.Text;
        //    string Address3 = txtAddress3.Text;
        //    string CountryID = Convert.ToString(cmbCountry.Value);
        //    string StateID = Convert.ToString(cmbState.Value);
        //    string DistrictID = Convert.ToString(cmbDistrict.Value);
        //    string PincodeID = Convert.ToString(cmbPincode.Value);

        //    string GSTIN = txtGSTIN1.Text.Trim() + txtGSTIN2.Text.Trim() + txtGSTIN3.Text.Trim();

        //    string ModuleList=hdnModuleList.Value;

        //    Boolean IsMandatory;
        //    if (ChkIsMandatory.Checked)
        //    {
        //        IsMandatory = true;
        //    }
        //    else
        //    {
        //        IsMandatory = false;
        //    }

        //    string mode=Convert.ToString(hdAddEdit.Value);
        //    string Document_SegmentId=Convert.ToString(hdDocumentSegmentId.Value);
        //    string InternalID=Convert.ToString(hdnCustomerID.Value);

        //    AddEditDocumentSegment(mode, SegmentID, ParentSegmentID, Code, Name, Address1, Address2, Address3,CountryID, StateID, DistrictID, PincodeID, GSTIN, ModuleList
        //      ,Document_SegmentId,IsMandatory,InternalID, ref DocumentSegmentId );

        //    if (DocumentSegmentId > 0)
        //    {
        //        Page.ClientScript.RegisterStartupScript(GetType(), "JScript3", "<script language='javascript'>PageLoad();</script>");
        //    }
        //}
        public void AddEditDocumentSegment(string mode, string SegmentID, string ParentSegmentID, string Code, string Name, string Address1, string Address2, string Address3, string CountryID
            , string StateID, string DistrictID, string PincodeID, string GSTIN, string ModuleList, string Document_SegmentId, Boolean IsMandatory, string InternalID, ref int DocumentSegmentId)
        {
            DataTable dsInst = new DataTable();

            SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));

            SqlCommand cmd = new SqlCommand("prc_DocumentSegmentAddEdit", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Action", mode);
            cmd.Parameters.AddWithValue("@SegmentID", SegmentID);
            cmd.Parameters.AddWithValue("@ParentSegmentID", ParentSegmentID);
            cmd.Parameters.AddWithValue("@Code", Code);
            cmd.Parameters.AddWithValue("@Name", Name);
            cmd.Parameters.AddWithValue("@Address1", Address1);
            cmd.Parameters.AddWithValue("@Address2", Address2);
            cmd.Parameters.AddWithValue("@Address3", Address3);
            cmd.Parameters.AddWithValue("@CountryID", CountryID);
            cmd.Parameters.AddWithValue("@StateID", StateID);
            cmd.Parameters.AddWithValue("@DistrictID", DistrictID);
            cmd.Parameters.AddWithValue("@PincodeID", PincodeID);
            cmd.Parameters.AddWithValue("@GSTIN", GSTIN);
            cmd.Parameters.AddWithValue("@ModuleLIst", ModuleList);
            cmd.Parameters.AddWithValue("@InternalID", InternalID);
            cmd.Parameters.AddWithValue("@IsMandatory", IsMandatory);
            cmd.Parameters.AddWithValue("@userId", Convert.ToString(Session["userid"]));
            cmd.Parameters.AddWithValue("@FinYear", HttpContext.Current.Session["LastFinYear"].ToString());
            cmd.Parameters.AddWithValue("@CompanyID", HttpContext.Current.Session["LastCompany"].ToString());


            cmd.Parameters.Add("@ReturnId", SqlDbType.VarChar, 10);



            cmd.Parameters["@ReturnId"].Direction = ParameterDirection.Output;

            cmd.CommandTimeout = 0;
            SqlDataAdapter Adap = new SqlDataAdapter();
            Adap.SelectCommand = cmd;
            Adap.Fill(dsInst);

            DocumentSegmentId = Convert.ToInt32(cmd.Parameters["@ReturnId"].Value);



            cmd.Dispose();
            con.Dispose();
        }



        [WebMethod]
        public static List<SegmentDetails> PopulateSEgment(string CustomerID)
        {
            //string userbranchHierarchy = Convert.ToString(Session["userbranchHierarchy"]);
            string actionqry = "PageLoadData";



            DataTable addtab = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_ENTITY_SEGMENT_MAP");
            proc.AddVarcharPara("@action", 100, actionqry);
            proc.AddVarcharPara("@InetrnalId", 100, CustomerID);
            addtab = proc.GetTable();

            List<SegmentDetails> GrpDet = new List<SegmentDetails>();
            GrpDet = (from DataRow dr in addtab.Rows
                      select new SegmentDetails()
                      {
                          ID = Convert.ToString(dr["ID"]),
                          Segment = Convert.ToString(dr["Segment"]),
                          //Type = Convert.ToString(dr["Type"])

                      }).ToList();

            return GrpDet;
        }

        public class SegmentDetails
        {

            public string ID { get; set; }
            public string Segment { get; set; }
        }
        public class EditDetails
        {
            public string ID { get; set; }
            public string cnt_id { get; set; }

            public string InternalID { get; set; }
            public string Segment_ID { get; set; }

            public string Code { get; set; }
            public string Name { get; set; }
            public string Address1 { get; set; }
            public string Address2 { get; set; }
            public string PinCode { get; set; }
            public string GSTIN { get; set; }
            public string CountryId { get; set; }
            public string StateID { get; set; }
            public string CityID { get; set; }
            public string ParentSegment_ID { get; set; }
            public string shippingAddress1 { get; set; }

            public string shippingAddress2 { get; set; }
            public string ServiceCountryID { get; set; }
            public string ServiceStateID { get; set; }
            public string ServiceCityID { get; set; }
            public string ServicePinID { get; set; }
            public string PanNo { get; set; }
            public string ContactName { get; set; }


            public string billCountryName { get; set; }
            public string serCountryName { get; set; }
            public string BillstateName { get; set; }
            public string SerstateName { get; set; }
            public string billcity_name { get; set; }
            public string sercity_name { get; set; }
            public string BillPincode { get; set; }
            public string SerPincode { get; set; }

            public string BillingLatitude { get; set; }
            public string BillingLongitude { get; set; }
            public string ServiceLatitude { get; set; }
            public string ServiceLongitude { get; set; }

            public string BillPhoneNo { get; set; }
            public string ServicePhoneNo { get; set; }


            public string TreatmentArea { get; set; }
            public string ServiceBranch { get; set; }


        }

        [WebMethod]
        public static List<SegmentDetails> PopulateParentSegment(string SegmentID, string CustomerID)
        {
            // string userbranchHierarchy = Convert.ToString(Session["userbranchHierarchy"]);
            string actionqry = "PopulateParentSegment";

            List<SegmentDetails> GrpDet = new List<SegmentDetails>();
            {
                DataTable addtab = new DataTable();
                ProcedureExecute proc = new ProcedureExecute("prc_ENTITY_SEGMENT_MAP");
                proc.AddVarcharPara("@action", 100, actionqry);
                proc.AddVarcharPara("@InetrnalId", 100, CustomerID);
                proc.AddVarcharPara("@SegmentId", 100, SegmentID);

                addtab = proc.GetTable();
                GrpDet = (from DataRow dr in addtab.Rows
                          select new SegmentDetails
                          {
                              ID = Convert.ToString(dr["ID"]),
                              Segment = Convert.ToString(dr["Code"]),
                              //Type = Convert.ToString(dr["Type"])

                          }).ToList();

            }
            return GrpDet;
        }

        [WebMethod]
        public static string GetMaxLenghtSegment(string SegmentID, string CustomerID)
        {
            // string userbranchHierarchy = Convert.ToString(Session["userbranchHierarchy"]);
            string actionqry = "GetMaxLength";

            string Maxlength = "";
            DataTable addtab = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_ENTITY_SEGMENT_MAP");
            proc.AddVarcharPara("@action", 100, actionqry);
            proc.AddVarcharPara("@InetrnalId", 100, CustomerID);
            proc.AddVarcharPara("@SegmentId", 100, SegmentID);

            addtab = proc.GetTable();
            if (addtab.Rows.Count > 0)
            {
                Maxlength = Convert.ToString(addtab.Rows[0]["SegmentValue"]);
            }


            return Maxlength;
        }

        [WebMethod]
        public static object CheckuniqueId(string uniqueId, string CustomerID)
        {
            MShortNameCheckingBL mshort = new MShortNameCheckingBL();
            bool IsPresent = false;
            string entityName = "";
            IsPresent = CheckUniqueWithtypeContactMaster(uniqueId, CustomerID);
            return new { IsPresent = IsPresent, entityName = entityName };
        }

        public static bool CheckUniqueWithtypeContactMaster(string shortname, string CustomerID)
        {
            ProcedureExecute proc;

            try
            {
                using (proc = new ProcedureExecute("prc_ENTITY_SEGMENT_MAP"))
                {
                    proc.AddVarcharPara("@action", 50, "CheckUniqueID");
                    proc.AddBooleanPara("@ReturnUniqueID", false, QueryParameterDirection.Output);
                    proc.AddVarcharPara("@ShortName", 50, shortname);
                    proc.AddVarcharPara("@InetrnalId", 100, CustomerID);

                    int i = proc.RunActionQuery();
                    var retData = Convert.ToBoolean(proc.GetParaValue("@ReturnUniqueID"));

                    return retData;
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }

            finally
            {
                proc = null;
            }


            return false;
        }


        [WebMethod]
        public static object GetAddressdetails(string pinCode)
        {
            if (pinCode.Trim() != "")
            {

                DataTable fetchTable = new DataTable();
                ProcedureExecute proc = new ProcedureExecute("prc_ENTITY_SEGMENT_MAP");
                proc.AddVarcharPara("@action", 500, "GETPINDETAILS");
                proc.AddVarcharPara("@pin", 6, pinCode);
                fetchTable = proc.GetTable();


                if (fetchTable.Rows.Count > 0)
                {
                    string country, state, city, countryID, stateId, cityID, PinID;
                    country = Convert.ToString(fetchTable.Rows[0]["cou_country"]);
                    state = Convert.ToString(fetchTable.Rows[0]["state"]);
                    city = Convert.ToString(fetchTable.Rows[0]["city_name"]);

                    countryID = Convert.ToString(fetchTable.Rows[0]["cou_id"]);
                    stateId = Convert.ToString(fetchTable.Rows[0]["id"]);
                    cityID = Convert.ToString(fetchTable.Rows[0]["city_id"]);
                    PinID = Convert.ToString(fetchTable.Rows[0]["pin_id"]);

                    string rreturnString = country + "||" + state + "||" + city + "||" + countryID + "||" + stateId + "||" + cityID + "||" + PinID;

                    var storiesObj = new { status = "Ok", Country = country, state = state, city = city, countryID = countryID, stateId = stateId, cityID = cityID, PinID = PinID };
                    return storiesObj;

                }
            }
            var storiesObj1 = new { status = "Not Found" };
            return storiesObj1;
        }


        public static bool IsBannedPAN(string pan)
        {
            // .............................Code Commented and Added by Sam on 07122016 to use Convert.tostring instead of tostring(). ................
            DataTable dtBannedPanCard = new DataTable();
            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
            //   dtBannedPanCard = oDBEngine.GetDataTable("master_bannedentity", " convert(varchar(10), bannedentity_OrderDate,105) as bannedentity_OrderDate,bannedentity_BanPeriod,bannedentity_NSECircularNumber ", "rtrim(ltrim(bannedentity_PAN)) ='" + pan.ToString().Trim() + "'");
            dtBannedPanCard = oDBEngine.GetDataTable("master_bannedentity", "'Circular No : '+BannedEntity_NSECircularNumber +'Order Date : '+ Convert(varchar(20),BannedEntity_OrderDate,105)+ ', Order Period : '+BannedEntity_BanPeriod as Msg", "rtrim(ltrim(bannedentity_PAN)) ='" + Convert.ToString(pan.Trim()) + "'");
            if (dtBannedPanCard != null && dtBannedPanCard.Rows.Count > 0)
            {

                return true;
            }
            else
            {

                return false;
            }

            // .............................Code Above Commented and Added by Sam on 07122016 to use Convert.tostring instead of tostring(). ..................................... 
        }
        [WebMethod]
        public static object SaveCustomer(string mode, string cnt_id, string SegmentID, string UniqueID, string Name, string ParentSegmentID, string BillingAddress1, string BillingAddress2, string BillingPin, string shippingAddress1, string shippingAddress2,
            string shippingPin, string GSTIN, string BillingPhone, string ShippingPhone, string contactperson
            , string PANValue
            , string BillingCountryID, string BillingStateID, string BillingCityID, string BillingPinID
            , string ServiceCountryID, string ServiceStateID, string ServiceCityID, string ServicePinID
            , string BillingLatitude, string BillingLongitude, string ServiceLatitude, string ServiceLongitude, string DocumentSegmentId
            , string TreatmentArea, string ServiceBranch, string SyncCustomertoFSMWhileCreating

            )
        {
            BusinessLogicLayer.Contact oContactGeneralBL = new BusinessLogicLayer.Contact();

            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
            CommonBL ComBL = new CommonBL();
            DateTime CreateDate = Convert.ToDateTime(oDBEngine.GetDate().ToShortDateString());

            string duplicate_PAN_Number = ComBL.GetSystemSettingsResult("duplicate_PAN_Number");
            string CreateLeadfromAddOnFlyCustomer = ComBL.GetSystemSettingsResult("CreateLeadfromAddOnFlyCustomer");

            try
            {

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
                //if (Convert.ToString(PANValue) != "")
                //{
                //    if (IsBannedPAN(PANValue) == true)
                //    {
                //        return new { status = "BannedPAN", Msg = "This PAN is banned by SEBI." };
                //    }
                //    if (duplicate_PAN_Number.ToUpper().ToString() != "YES")
                //    {
                //        if (checkPANExistance(PANValue) == true)
                //        {
                //            return new { status = "DuplicatePAN", Msg = "Duplicate PAN number is not allowed." };
                //        }
                //    }
                //}


                if (CheckUniqueWithtypeContactMaster(UniqueID, cnt_id) || !string.IsNullOrEmpty(Convert.ToString(Name)))
                {
                    DataTable dsInst = new DataTable();

                    SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));

                    SqlCommand cmd = new SqlCommand("prc_DocumentSegmentAddEdit", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Action", mode);
                    cmd.Parameters.AddWithValue("@SegmentID", SegmentID);
                    cmd.Parameters.AddWithValue("@ParentSegmentID", ParentSegmentID);
                    cmd.Parameters.AddWithValue("@Code", UniqueID);
                    cmd.Parameters.AddWithValue("@Name", Name);

                    cmd.Parameters.AddWithValue("@Pan", PANValue);
                    cmd.Parameters.AddWithValue("@contactperson", contactperson);

                    cmd.Parameters.AddWithValue("@Address1", BillingAddress1);
                    cmd.Parameters.AddWithValue("@Address2", BillingAddress2);

                    cmd.Parameters.AddWithValue("@CountryID", BillingCountryID);
                    cmd.Parameters.AddWithValue("@StateID", BillingStateID);
                    cmd.Parameters.AddWithValue("@DistrictID", BillingCityID);
                    cmd.Parameters.AddWithValue("@PincodeID", BillingPinID);
                    cmd.Parameters.AddWithValue("@GSTIN", GSTIN);


                    cmd.Parameters.AddWithValue("@shippingAddress1", shippingAddress1);
                    cmd.Parameters.AddWithValue("@shippingAddress2", shippingAddress2);

                    cmd.Parameters.AddWithValue("@ServiceCountryID", ServiceCountryID);
                    cmd.Parameters.AddWithValue("@ServiceStateID", ServiceStateID);
                    cmd.Parameters.AddWithValue("@ServiceCityID", ServiceCityID);
                    cmd.Parameters.AddWithValue("@ServicePinID", ServicePinID);


                    cmd.Parameters.AddWithValue("@BillingLatitude", BillingLatitude);
                    cmd.Parameters.AddWithValue("@BillingLongitude", BillingLongitude);
                    cmd.Parameters.AddWithValue("@ServiceLatitude", ServiceLatitude);
                    cmd.Parameters.AddWithValue("@ServiceLongitude", ServiceLongitude);


                    cmd.Parameters.AddWithValue("@InternalID", cnt_id);
                    cmd.Parameters.AddWithValue("@Segment_Map_ID", DocumentSegmentId);

                    cmd.Parameters.AddWithValue("@BillPhoneNo", BillingPhone);
                    cmd.Parameters.AddWithValue("@ServicePhoneNo", ShippingPhone);

                    cmd.Parameters.AddWithValue("@TreatmentArea", TreatmentArea);
                    cmd.Parameters.AddWithValue("@ServiceBranch", ServiceBranch);

                    cmd.Parameters.AddWithValue("@userId", Convert.ToString(HttpContext.Current.Session["userid"]));
                    cmd.Parameters.AddWithValue("@FinYear", HttpContext.Current.Session["LastFinYear"].ToString());
                    cmd.Parameters.AddWithValue("@CompanyID", HttpContext.Current.Session["LastCompany"].ToString());

                    cmd.Parameters.Add("@ReturnId", SqlDbType.VarChar, 10);
                    cmd.Parameters["@ReturnId"].Direction = ParameterDirection.Output;

                    cmd.CommandTimeout = 0;
                    SqlDataAdapter Adap = new SqlDataAdapter();
                    Adap.SelectCommand = cmd;
                    Adap.Fill(dsInst);

                    Int32 Document_SegmentId = Convert.ToInt32(cmd.Parameters["@ReturnId"].Value);

                    cmd.Dispose();
                    con.Dispose();


                    //add for Segment Sync Tanmoy
                    if (SyncCustomertoFSMWhileCreating == "Yes")
                    {
                        int k = segmentsynctoFSM(Document_SegmentId, SegmentID, cnt_id);
                    }
                    //add for Segment Sync Tanmoy

                    return new { status = "OK", Msg = "Save successfully" };

                }
                else
                {
                    return new { status = "Error", Msg = "Already Exists" };
                }



            }
            catch (Exception ex)
            {
                return new { status = "Error", Msg = ex.Message };
            }
        }

        public static int segmentsynctoFSM(Int32 SegmentID, string SegmentType, String cnt_id)
        {
            String weburl = System.Configuration.ConfigurationSettings.AppSettings["FSMAPIBaseUrl"];
            string apiUrl = weburl + "ShopRegisterPortal/CustomerSyncinShop";
            RegisterShopOutput oview = new RegisterShopOutput();
            int userid = Convert.ToInt32(HttpContext.Current.Session["UserID"]);
            RegisterShopInputPortal empDtls = new RegisterShopInputPortal();
            int i = 0;
            DataTable dt = new DataTable();
            ProcedureExecute proc2 = new ProcedureExecute("PRC_SegmentDetailsSyncInCreation");
            proc2.AddPara("@SegmentID", SegmentID);
            proc2.AddPara("@SegmentType", SegmentType);
            dt = proc2.GetTable();
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow item in dt.Rows)
                {
                    String Status = "Failed";
                    String FailedReason = "";

                    if (Convert.ToString(item["IsCustomerSync"]) == "1")
                    {
                        //if (Convert.ToString(item["IsSeg1Sync"]) == "1" && SegmentType != 1)
                        //{
                            DateTime date1 = DateTime.Parse("1970-01-01");
                            DateTime date2 = System.DateTime.Now;
                            var Difference_In_Time = Convert.ToString((int)DateTime.Now.Subtract(new DateTime(1970, 1, 1)).TotalSeconds);
                            var middle = (Math.Round(Convert.ToDecimal(Difference_In_Time) / 1000) * 1155) + 1;

                            empDtls.session_token = "zksjfhjsdjkskjdh";
                            empDtls.user_id = Convert.ToString(378);
                            empDtls.shop_name = Convert.ToString(item["PartyName"]);
                            empDtls.address = Convert.ToString(item["ADDRESS1"]);
                            empDtls.pin_code = Convert.ToString(item["PinCode"]);
                            empDtls.shop_lat = Convert.ToString(item["PartyLocationLat"]);
                            empDtls.shop_long = Convert.ToString(item["PartyLocationLong"]);
                            empDtls.owner_name = Convert.ToString(item["Owner"]);
                            empDtls.owner_contact_no = Convert.ToString(item["Contact"]);
                            empDtls.owner_email = Convert.ToString(item["Email"]);
                            empDtls.type = Convert.ToInt32(item["Type"]);
                            empDtls.dob = Convert.ToString(item["DOB"]);
                            empDtls.date_aniversary = Convert.ToString(item["Anniversary"]);
                            empDtls.shop_id = Convert.ToString(item["AssignToUser"]) + "_" + Convert.ToString(Difference_In_Time);
                            empDtls.added_date = Convert.ToString(System.DateTime.Now);
                            empDtls.assigned_to_pp_id = Convert.ToString(item["assigned_to_pp_id"]); ;
                            empDtls.assigned_to_dd_id = Convert.ToString(item["assigned_to_dd_id"]); ;
                            empDtls.EntityCode = Convert.ToString(item["PartyCode"]);
                            empDtls.Entity_Location = Convert.ToString(item["Location"]);
                            empDtls.Alt_MobileNo = Convert.ToString(item["AlternateContact"]);
                            empDtls.Entity_Status = Convert.ToString(item["Status"]);
                            empDtls.Entity_Type = Convert.ToString(item["EntityCategory"]);
                            empDtls.ShopOwner_PAN = Convert.ToString(item["OwnerPAN"]);
                            empDtls.ShopOwner_Aadhar = Convert.ToString(item["OwnerAadhaar"]);
                            empDtls.Remarks = Convert.ToString(item["Remarks"]);
                            empDtls.AreaId = Convert.ToString(item["Area"]);
                            empDtls.CityId = Convert.ToString(item["District"]);
                            empDtls.Entered_by = Convert.ToString(userid);
                            empDtls.retailer_id = Convert.ToString("0");
                            empDtls.dealer_id = Convert.ToString("0");
                            empDtls.entity_id = Convert.ToString("0");
                            empDtls.party_status_id = Convert.ToString(item["PartyStatus"]);
                            empDtls.beat_id = Convert.ToString(item["GroupBeat"]);
                            empDtls.IsServicePoint = Convert.ToString(dt.Rows[0]["IsServicePoint"]);

                            string data = JsonConvert.SerializeObject(empDtls);

                            HttpClient httpClient = new HttpClient();
                            MultipartFormDataContent form = new MultipartFormDataContent();
                            //byte[] fileBytes = new byte[1];
                            //var fileContent = new StreamContent(null);
                            form.Add(new StringContent(data), "data");
                            //form.Add(emp, "shop_image", null);
                            var result = httpClient.PostAsync(apiUrl, form).Result;

                            oview = JsonConvert.DeserializeObject<RegisterShopOutput>(result.Content.ReadAsStringAsync().Result);

                            //oview.status = "200";

                            if (Convert.ToString(oview.status) == "200")
                            {
                                Status = "Success";
                            }
                            else if (Convert.ToString(oview.status) == "202")
                            {
                                FailedReason = "Customer Name Not found";
                            }
                            else if (Convert.ToString(oview.status) == "203")
                            {
                                FailedReason = "Entity Code not found";
                            }
                            else if (Convert.ToString(oview.status) == "204")
                            {
                                FailedReason = "Owner Name Not found";
                            }
                            else if (Convert.ToString(oview.status) == "205")
                            {
                                FailedReason = "Customer Address not found";
                            }
                            else if (Convert.ToString(oview.status) == "206")
                            {
                                FailedReason = "Pin Code not found";
                            }
                            else if (Convert.ToString(oview.status) == "207")
                            {
                                FailedReason = "Customer Contact number not found";
                            }
                            else if (Convert.ToString(oview.status) == "208")
                            {
                                FailedReason = "User or session token not matched";
                            }
                            else if (Convert.ToString(oview.status) == "209")
                            {
                                FailedReason = "Duplicate Customer Id or contact number";
                            }
                            else if (Convert.ToString(oview.status) == "210")
                            {
                                FailedReason = "Duplicate contact number";
                            }
                        //}
                        //else
                        //{
                        //    FailedReason = "Parent segment not Sync";
                        //}
                    }
                    else
                    {
                        FailedReason = "Customer not Sync";
                    }


                    ProcedureExecute proc1 = new ProcedureExecute("PRC_SegmentDetailsForSync");
                    proc1.AddPara("@ACTION", "UpdateSegment");
                    proc1.AddPara("@ContactID", cnt_id);
                    proc1.AddPara("@SegemntName", Convert.ToString(item["PartyName"]));
                    proc1.AddPara("@SegemntAddress", Convert.ToString(item["ADDRESS1"]));
                    proc1.AddPara("@SegemntPhone", Convert.ToString(item["Contact"]));
                    proc1.AddPara("@SyncBy", userid);
                    proc1.AddPara("@Status", Status);
                    proc1.AddPara("@FailedReason", FailedReason);
                    proc1.AddPara("@Shop_Code", empDtls.shop_id);
                    proc1.AddPara("@SegmentID", Convert.ToString(item["ID"]));
                    proc1.AddPara("@SegmentCode", Convert.ToString(item["PartyCode"]));
                    proc1.AddPara("@InternalID", Convert.ToString(item["InternalID"]));
                    i = proc1.RunActionQuery();
                }
            }
            return i;
        }


        public class RegisterShopInputPortal
        {
            public string session_token { get; set; }
            //[Required]
            public string user_id { get; set; }
            //[Required]
            public string shop_name { get; set; }
            //[Required]
            public string address { get; set; }
            //[Required]
            public string pin_code { get; set; }
            //[Required]
            public string shop_lat { get; set; }
            //[Required]
            public string shop_long { get; set; }
            //[Required]
            public string owner_name { get; set; }
            //[Required]
            public string owner_contact_no { get; set; }
            //[Required]
            public string owner_email { get; set; }
            public int? type { get; set; }
            public string dob { get; set; }
            public string date_aniversary { get; set; }
            public string shop_id { get; set; }
            public string added_date { get; set; }
            public string assigned_to_pp_id { get; set; }
            public string assigned_to_dd_id { get; set; }
            public string amount { get; set; }
            public Nullable<DateTime> family_member_dob { get; set; }
            public string director_name { get; set; }
            public string key_person_name { get; set; }
            public string phone_no { get; set; }
            public Nullable<DateTime> addtional_dob { get; set; }
            public Nullable<DateTime> addtional_doa { get; set; }
            public Nullable<DateTime> doc_family_member_dob { get; set; }
            public string specialization { get; set; }
            public string average_patient_per_day { get; set; }
            public string category { get; set; }
            public string doc_address { get; set; }
            public string doc_pincode { get; set; }
            public string is_chamber_same_headquarter { get; set; }
            public string is_chamber_same_headquarter_remarks { get; set; }
            public string chemist_name { get; set; }
            public string chemist_address { get; set; }
            public string chemist_pincode { get; set; }
            public string assistant_name { get; set; }
            public string assistant_contact_no { get; set; }
            public Nullable<DateTime> assistant_dob { get; set; }
            public Nullable<DateTime> assistant_doa { get; set; }
            public Nullable<DateTime> assistant_family_dob { get; set; }
            public string EntityCode { get; set; }
            public string Entity_Location { get; set; }
            public string Alt_MobileNo { get; set; }
            public string Entity_Status { get; set; }
            public string Entity_Type { get; set; }
            public string ShopOwner_PAN { get; set; }
            public string ShopOwner_Aadhar { get; set; }
            public string Remarks { get; set; }
            public string AreaId { get; set; }
            public string CityId { get; set; }
            public string Entered_by { get; set; }
            public string entity_id { get; set; }
            public string party_status_id { get; set; }
            public string retailer_id { get; set; }
            public string dealer_id { get; set; }
            public string beat_id { get; set; }
            public string IsServicePoint { get; set; }
        }

        public class RegisterShopOutput
        {
            public string status { get; set; }
            public string message { get; set; }
            public string session_token { get; set; }
        }

        [WebMethod]
        public static List<EditDetails> PopulateEditDate(string DocumentSegmentId)
        {
            // string userbranchHierarchy = Convert.ToString(Session["userbranchHierarchy"]);
            string actionqry = "EditMaster_Entity_Segment";

            List<EditDetails> GrpDet = new List<EditDetails>();
            {
                DataTable addtab = new DataTable();
                ProcedureExecute proc = new ProcedureExecute("prc_ENTITY_SEGMENT_MAP");
                proc.AddVarcharPara("@action", 100, actionqry);
                proc.AddVarcharPara("@Segment_Map_ID", 100, DocumentSegmentId);


                addtab = proc.GetTable();
                GrpDet = (from DataRow dr in addtab.Rows
                          select new EditDetails
                          {
                              ID = Convert.ToString(dr["ID"]),
                              cnt_id = Convert.ToString(dr["cnt_id"]),

                              InternalID = Convert.ToString(dr["InternalID"]),
                              Segment_ID = Convert.ToString(dr["Segment_ID"]),
                              Code = Convert.ToString(dr["Code"]),
                              Name = Convert.ToString(dr["Name"]),
                              Address1 = Convert.ToString(dr["Address1"]),
                              Address2 = Convert.ToString(dr["Address2"]),
                              PinCode = Convert.ToString(dr["PinCode"]),
                              CountryId = Convert.ToString(dr["CountryId"]),
                              GSTIN = Convert.ToString(dr["GSTIN"]),
                              StateID = Convert.ToString(dr["StateID"]),
                              CityID = Convert.ToString(dr["CityID"]),
                              ParentSegment_ID = Convert.ToString(dr["ParentSegment_ID"]),
                              shippingAddress1 = Convert.ToString(dr["shippingAddress1"]),
                              shippingAddress2 = Convert.ToString(dr["shippingAddress2"]),
                              ServiceCountryID = Convert.ToString(dr["ServiceCountryID"]),
                              ServiceStateID = Convert.ToString(dr["ServiceStateID"]),

                              ServiceCityID = Convert.ToString(dr["ServiceCityID"]),
                              ServicePinID = Convert.ToString(dr["ServicePinID"]),
                              ContactName = Convert.ToString(dr["ContactName"]),
                              billCountryName = Convert.ToString(dr["billCountryName"]),

                              serCountryName = Convert.ToString(dr["serCountryName"]),
                              BillstateName = Convert.ToString(dr["BillstateName"]),
                              SerstateName = Convert.ToString(dr["SerstateName"]),
                              billcity_name = Convert.ToString(dr["billcity_name"]),

                              sercity_name = Convert.ToString(dr["sercity_name"]),
                              BillPincode = Convert.ToString(dr["BillPincode"]),
                              SerPincode = Convert.ToString(dr["SerPincode"]),

                              BillingLatitude = Convert.ToString(dr["BillingLatitude"]),
                              BillingLongitude = Convert.ToString(dr["BillingLongitude"]),
                              ServiceLatitude = Convert.ToString(dr["ServiceLatitude"]),
                              ServiceLongitude = Convert.ToString(dr["ServiceLongitude"]),

                              BillPhoneNo = Convert.ToString(dr["BillPhoneNo"]),
                              ServicePhoneNo = Convert.ToString(dr["ServicePhoneNo"]),
                              TreatmentArea = Convert.ToString(dr["TreatmentArea"]),
                              ServiceBranch = Convert.ToString(dr["ServiceBranchID"]),



                          }).ToList();

            }
            return GrpDet;
        }


        //protected void FillStateCombo(ASPxComboBox cmb, int country)
        //{

        //    string[,] state = GetState(country);
        //    cmb.Items.Clear();

        //    for (int i = 0; i < state.GetLength(0); i++)
        //    {
        //        cmb.Items.Add(state[i, 1], state[i, 0]);
        //    }
        //    // cmb.Items.Insert(0, new ListEditItem("Select", "0"));
        //}
        //string[,] GetState(int country)
        //{
        //    StateSelect.SelectParameters[0].DefaultValue = Convert.ToString(country);
        //    DataView view = (DataView)StateSelect.Select(DataSourceSelectArguments.Empty);
        //    string[,] DATA = new string[view.Count, 2];
        //    for (int i = 0; i < view.Count; i++)
        //    {
        //        DATA[i, 0] = Convert.ToString(view[i][0]);
        //        DATA[i, 1] = Convert.ToString(view[i][1]);
        //    }
        //    return DATA;

        //}

        //protected void cmbState_Callback(object sender, CallbackEventArgsBase e)
        //{
        //    //FillStateCombo(sender as ASPxComboBox, Convert.ToInt32(e.Parameter));
        //    DataTable dt = new DataTable();
        //    dt = BindState(Convert.ToInt32(e.Parameter));
        //    cmbState.DataSource = dt;
        //    cmbState.ValueField = "ID";
        //    cmbState.TextField = "State";
        //    cmbState.DataBind();

        //}
        //public DataTable BindState(int CountryId)
        //{
        //    ProcedureExecute proc = new ProcedureExecute("prc_ENTITY_SEGMENT_MAP");
        //    proc.AddVarcharPara("@Action", 50, "BindState");
        //    proc.AddIntegerPara("@CountryId", CountryId);            
        //    return proc.GetTable();
        //}


        //protected void cmbParent_Callback(object sender, CallbackEventArgsBase e)
        //{
        //    string segmentId = e.Parameter.Split('~')[0];

        //    DataTable dt=PopulateParentSegmentDetails(segmentId);
        //    cmbParent.DataSource = dt;
        //    cmbParent.ValueField = "ID";
        //    cmbParent.TextField = "Segment";
        //    cmbParent.DataBind();
        //}

        //public DataTable PopulateParentSegmentDetails(string segmentId)
        //{
        //    ProcedureExecute proc = new ProcedureExecute("prc_ENTITY_SEGMENT_MAP");
        //    proc.AddVarcharPara("@Action", 50, "PopulateParentSegment");            
        //    proc.AddVarcharPara("@InetrnalId", 50, hdnCustomerID.Value);
        //    proc.AddVarcharPara("@SegmentId", 50, segmentId);
        //    return proc.GetTable();

        //}

        //protected void cmbDistrict_Callback(object sender, CallbackEventArgsBase e)
        //{
        //    DataTable dt = new DataTable();
        //    dt = BindDistrict(Convert.ToInt32(e.Parameter));
        //    cmbDistrict.DataSource = dt;
        //    cmbDistrict.ValueField = "CityId";
        //    cmbDistrict.TextField = "City";
        //    cmbDistrict.DataBind();
        //}
        //public DataTable BindDistrict(int StateID)
        //{
        //    ProcedureExecute proc = new ProcedureExecute("prc_ENTITY_SEGMENT_MAP");
        //    proc.AddVarcharPara("@Action", 50, "Bindcity");
        //    proc.AddIntegerPara("@StateID", StateID);
        //    return proc.GetTable();
        //}

        //protected void FillCityCombo(ASPxComboBox cmb, int state)
        //{
        //    string[,] cities = GetCities(state);
        //    cmb.Items.Clear();
        //    for (int i = 0; i < cities.GetLength(0); i++)
        //    {
        //        cmb.Items.Add(cities[i, 1], cities[i, 0]);
        //    }            
        //}
        //string[,] GetCities(int state)
        //{
        //    SelectCity.SelectParameters[0].DefaultValue = Convert.ToString(state);
        //    DataView view = (DataView)SelectCity.Select(DataSourceSelectArguments.Empty);
        //    string[,] DATA = new string[view.Count, 2];
        //    for (int i = 0; i < view.Count; i++)
        //    {
        //        DATA[i, 0] = Convert.ToString(view[i][0]);
        //        DATA[i, 1] = Convert.ToString(view[i][1]);
        //    }
        //    return DATA;
        //}

        //protected void cmbPincode_Callback(object sender, CallbackEventArgsBase e)
        //{
        //    DataTable dt = new DataTable();
        //    dt = BindPincode(Convert.ToInt32(e.Parameter));
        //    cmbPincode.DataSource = dt;
        //    cmbPincode.ValueField = "pin_id";
        //    cmbPincode.TextField = "pin_code";
        //    cmbPincode.DataBind();
        //}

        //public DataTable BindPincode(int CityID)
        //{
        //    ProcedureExecute proc = new ProcedureExecute("prc_ENTITY_SEGMENT_MAP");
        //    proc.AddVarcharPara("@Action", 50, "BindPin");
        //    proc.AddIntegerPara("@CityID", CityID);
        //    return proc.GetTable();
        //}
        //protected void FillPinCombo(ASPxComboBox cmb, int city)
        //{
        //    string[,] pin = GetPin(city);
        //    cmb.Items.Clear();

        //    for (int i = 0; i < pin.GetLength(0); i++)
        //    {
        //        cmb.Items.Add(pin[i, 1], pin[i, 0]);
        //    }

        //}
        //string[,] GetPin(int city)
        //{
        //    SelectPin.SelectParameters[0].DefaultValue = Convert.ToString(city);
        //    DataView view = (DataView)SelectPin.Select(DataSourceSelectArguments.Empty);
        //    string[,] DATA = new string[view.Count, 2];
        //    for (int i = 0; i < view.Count; i++)
        //    {
        //        DATA[i, 0] = Convert.ToString(view[i][0]);
        //        DATA[i, 1] = Convert.ToString(view[i][1]);
        //    }
        //    return DATA;
        //}

        //[WebMethod]
        //public static List<ModuleList> GetModuleList(string Segment_Map_ID)
        //{
        //    BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine();
        //    List<ModuleList> omodel = new List<ModuleList>();
        //    DataTable dt = new DataTable();
        //    ProcedureExecute proc = new ProcedureExecute("prc_ENTITY_SEGMENT_MAP");
        //    proc.AddPara("@action", "Populateodule");
        //    proc.AddPara("@Segment_Map_ID", Segment_Map_ID);

        //    dt = proc.GetTable();
        //    if (dt != null && dt.Rows.Count > 0)
        //    {
        //        omodel = UtilityLayer.APIHelperMethods.ToModelList<ModuleList>(dt);
        //    }
        //    return omodel;
        //}
        //public class ModuleList
        //{
        //    public long Module_Id { get; set; }
        //    public String Module_Name { get; set; }
        //    public bool IsChecked { get; set; }


        //}
    }


}

