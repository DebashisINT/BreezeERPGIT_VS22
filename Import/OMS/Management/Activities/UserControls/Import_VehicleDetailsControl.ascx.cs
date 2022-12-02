using BusinessLogicLayer;
using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ERP.OMS.Management.Activities.UserControls
{
    public partial class Import_VehicleDetailsControl : System.Web.UI.UserControl
    {
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        SlaesActivitiesBL objSlaesActivitiesBL = new SlaesActivitiesBL();
        SalesInvoiceBL objSalesInvoiceBL = new SalesInvoiceBL();
        CommonBL objCommonBL = new CommonBL();
        //string IsVisible = string.Empty;

        //protected void VehicleOutDate_Init(object sender, EventArgs e)
        //{
        //    dtVehicleOutDate.Date = DateTime.Now;
        //}

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);

                #region Show Transporter
                string Variable_Name = string.Empty;
                if (Request.QueryString["type"] != null && Convert.ToString(Request.QueryString["type"]) != "")
                {
                    string Type = Convert.ToString(Request.QueryString["type"]);
                    Variable_Name = "Transporter_" + Type + "Show";
                }
                else
                {
                    try
                    {
                        HiddenField ctl = (HiddenField)this.Parent.FindControl("hfTermsConditionDocType");
                        string DocType = ctl.Value;
                        Variable_Name = "Transporter_" + DocType + "Show";
                    }
                    catch (Exception ex) { Variable_Name = ""; }
                }

                DataTable DT = objEngine.GetDataTable("Config_SystemSettings", " Variable_Value ", " Variable_Name='" + Variable_Name + "' AND IsActive=1");

                if (DT != null && DT.Rows.Count > 0)
                {
                    string IsVisible = Convert.ToString(DT.Rows[0]["Variable_Value"]).Trim();
                    Session.Add("TransporterVisibilty", IsVisible);
                    if (IsVisible == "Yes")
                    {
                        this.Visible = true;
                    }
                    else
                    {
                        this.Visible = false;
                    }
                }
                #endregion

                //dtVehicleOutDate.Date = DateTime.Now;

                //DataTable DT = objEngine.GetDataTable("Config_SystemSettings", " Variable_Value ", " Variable_Name='Show_Transporter' AND IsActive=1");
                //if (DT != null && DT.Rows.Count > 0)
                //{
                //    string IsVisible = Convert.ToString(DT.Rows[0]["Variable_Value"]).Trim();
                //    Session.Add("TransporterVisibilty", IsVisible);
                //    if (IsVisible == "Yes")
                //    {
                //        this.Visible = true;
                //    }
                //    else
                //    {
                //        this.Visible = false;
                //    }
                //}
                radioregistercheck.Attributes.Add("onclick", "registeredCheckChangeEvent()");
                string userbranch = Convert.ToString(Session["userbranchHierarchy"]);
                string strCompanyID = Convert.ToString(Session["LastCompany"]);
                string strBranchID = Convert.ToString(Session["userbranchID"]);

                DataSet dstDT = new DataSet();
                dstDT = objSalesInvoiceBL.GetAllDropDownDetailForSalesInvoice(userbranch, strCompanyID, strBranchID);

                dstDT.Clear();

                dstDT = objCommonBL.GetContactTypeTransporter("TR");
                if (dstDT.Tables[0] != null && dstDT.Tables[0].Rows.Count > 0)
                {
                    cmbTransporter.ValueField = "cnt_internalId";
                    cmbTransporter.TextField = "cnt_Name";
                    cmbTransporter.DataSource = dstDT.Tables[0];
                    cmbTransporter.DataBind();

                    cmbTransporter.Items.Insert(0, new ListEditItem("--Select--", ""));
                }

                if (dstDT.Tables[0] != null && dstDT.Tables[0].Rows.Count > 0)
                {
                    cmbFinalTransporter.ValueField = "cnt_internalId";
                    cmbFinalTransporter.TextField = "cnt_Name";
                    cmbFinalTransporter.DataSource = dstDT.Tables[0];
                    cmbFinalTransporter.DataBind();

                    cmbFinalTransporter.Items.Insert(0, new ListEditItem("--Select--", ""));
                }

                if (dstDT.Tables[1] != null && dstDT.Tables[1].Rows.Count > 0)
                {
                    vcCmbCountry.TextField = "Country";
                    vcCmbCountry.ValueField = "cou_id";
                    vcCmbCountry.DataSource = dstDT.Tables[1];
                    vcCmbCountry.DataBind();

                    vcCmbCountry.Items.Insert(0, new ListEditItem("--Select--", ""));
                }

                if (dstDT.Tables[2] != null && dstDT.Tables[2].Rows.Count > 0)
                {
                    vcCmbFreightArea.TextField = "FreightArea_AreaName";
                    vcCmbFreightArea.ValueField = "FreightArea_ID";
                    vcCmbFreightArea.DataSource = dstDT.Tables[2];
                    vcCmbFreightArea.DataBind();
                }
                ////######## Coded By Samrat Roy -- This Functionality is to bind data into Transporter Control by Sales Order ID 
                if (Request.QueryString["key"] != null && Request.QueryString["key"] != "ADD")
                {
                    BindDataByDocID(Request.QueryString["key"], Request.QueryString["type"]);
                    pnl_ChallanSpeceficFields.Visible = ControlVisibilityByDocType(Request.QueryString["type"]);
                }
            }
        }

        protected void cmbTransporterType_Callback(object sender, CallbackEventArgsBase e)
        {
            string WhichCall = e.Parameter.Split('~')[0];
            string InternalId = Convert.ToString(e.Parameter.Split('~')[0]);
            List<string> obj = PopulateTransporterDetails(InternalId);
            cmbTransporterType.JSProperties["cpBind"] = obj;
        }

        private List<string> PopulateTransporterDetails(string TransID)
        {
            List<string> obj = new List<string>();
            CommonBL objCommonBL = new CommonBL();
            try
            {

                DataSet dsVehicleControlDetails = objCommonBL.GetVehicleControlDetails(TransID);

                cmbTransporterType.ValueField = "lgl_id";
                cmbTransporterType.TextField = "lgl_legalStatus";
                cmbTransporterType.DataSource = dsVehicleControlDetails.Tables[5];
                cmbTransporterType.DataBind();

                if (Convert.ToString(dsVehicleControlDetails.Tables[0].Rows[0]["cnt_legalStatus"]) == "55")
                {
                    string[] strArr = new string[] { };
                }

                if (dsVehicleControlDetails.Tables[0].Rows.Count > 0) // Transporter Details
                {
                    obj.Add(Convert.ToString(dsVehicleControlDetails.Tables[0].Rows[0]["cnt_legalStatus"]));
                    obj.Add((string.IsNullOrEmpty(Convert.ToString(dsVehicleControlDetails.Tables[0].Rows[0]["CNT_GSTIN"])) ? "0" : "1"));
                    obj.Add((Convert.ToString(dsVehicleControlDetails.Tables[0].Rows[0]["CNT_GSTIN"])));
                }

                if (dsVehicleControlDetails.Tables[1].Rows.Count > 0) // Vehicle Details
                {
                    obj.Add(Convert.ToString(dsVehicleControlDetails.Tables[1].Rows[0]["VehicleNos"]));
                }
                else
                {
                    obj.Add(string.Empty);
                }

                if (dsVehicleControlDetails.Tables[2].Rows.Count > 0) // Address Details
                {
                    obj.Add(Convert.ToString(dsVehicleControlDetails.Tables[2].Rows[0]["Address"]) + "|" +
                        Convert.ToString(dsVehicleControlDetails.Tables[2].Rows[0]["add_country"]) + "|" +
                        Convert.ToString(dsVehicleControlDetails.Tables[2].Rows[0]["add_state"]) + "|" +
                        Convert.ToString(dsVehicleControlDetails.Tables[2].Rows[0]["add_city"]) + "|" +
                        Convert.ToString(dsVehicleControlDetails.Tables[2].Rows[0]["add_pin"]) + "|" +
                        Convert.ToString(dsVehicleControlDetails.Tables[2].Rows[0]["add_area"])
                    );
                }
                else
                {
                    obj.Add(string.Empty);
                }

                if (dsVehicleControlDetails.Tables[3].Rows.Count > 0) // Phone Details
                {
                    obj.Add(Convert.ToString(dsVehicleControlDetails.Tables[3].Rows[0]["phf_phoneNumber"]));
                }
                else
                {
                    obj.Add(string.Empty);
                }
            }
            catch (Exception ex)
            {

            }
            return obj;

        }

        protected void cmbState_OnCallback(object source, CallbackEventArgsBase e)
        {

            if (e.Parameter != "")
            {
                CommonBL objCommonBL = new CommonBL();

                DataSet dsState = objCommonBL.GetStateByCountry(e.Parameter);

                vcCmbState.ValueField = "state_id";
                vcCmbState.TextField = "state_name";
                vcCmbState.DataSource = dsState.Tables[0];
                vcCmbState.DataBind();

            }
            vcCmbState.Items.Insert(0, new ListEditItem("--Select--", ""));
        }

        protected void cmbCity_OnCallback(object source, CallbackEventArgsBase e)
        {

            if (e.Parameter != "")
            {
                CommonBL objCommonBL = new CommonBL();

                DataSet dsState = objCommonBL.GetCityByState(e.Parameter);

                vcCmbCity.ValueField = "city_id";
                vcCmbCity.TextField = "city_name";
                vcCmbCity.DataSource = dsState.Tables[0];
                vcCmbCity.DataBind();


            }
            vcCmbCity.Items.Insert(0, new ListEditItem("--Select--", ""));
        }

        protected void cmbPin_OnCallback(object source, CallbackEventArgsBase e)
        {

            if (e.Parameter != "")
            {
                CommonBL objCommonBL = new CommonBL();

                DataSet dsState = objCommonBL.GetPinByCity(e.Parameter);

                vcCmbPin.ValueField = "pin_id";
                vcCmbPin.TextField = "pin_code";
                vcCmbPin.DataSource = dsState.Tables[0];
                vcCmbPin.DataBind();


            }
            vcCmbPin.Items.Insert(0, new ListEditItem("--Select--", ""));
        }

        protected void cmbArea_OnCallback(object source, CallbackEventArgsBase e)
        {
            if (e.Parameter != "")
            {
                CommonBL objCommonBL = new CommonBL();

                DataSet dsState = objCommonBL.GetAreaByCity(e.Parameter);

                vcCmbArea.ValueField = "area_id";
                vcCmbArea.TextField = "area_name";
                vcCmbArea.DataSource = dsState.Tables[0];
                vcCmbArea.DataBind();


            }
            vcCmbArea.Items.Insert(0, new ListEditItem("--Select--", ""));
        }

        protected void ComponentPanel_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            CommonBL objCommonBL = new CommonBL();
            DataSet dst = new DataSet();
            dst = objCommonBL.GetContactTypeTransporter("TR");

            vcCmbCountry.TextField = "Country";
            vcCmbCountry.ValueField = "cou_id";
            vcCmbCountry.DataSource = dst.Tables[1];
            vcCmbCountry.DataBind();
            vcCmbCountry.Items.Insert(0, new ListEditItem("--Select--", ""));
            dst.Clear();

            string[] listData = e.Parameter.Split('~');

            #region ######### Exisiting Process ############
            if (listData[0] == "N") // Existing Process
            {
                if (!string.IsNullOrEmpty(listData[3])) //// while Country,State,City etc data are available
                {
                    string[] data = listData[3].Split('|');

                    dst = objCommonBL.GetStateByCountry(data[1]);
                    vcCmbState.ValueField = "state_id";
                    vcCmbState.TextField = "state_name";
                    vcCmbState.DataSource = dst.Tables[0];
                    vcCmbState.DataBind();
                    vcCmbState.Items.Insert(0, new ListEditItem("--Select--", ""));
                    dst.Clear();

                    dst = objCommonBL.GetCityByState(data[2]);
                    vcCmbCity.ValueField = "city_id";
                    vcCmbCity.TextField = "city_name";
                    vcCmbCity.DataSource = dst.Tables[0];
                    vcCmbCity.DataBind();
                    vcCmbCity.Items.Insert(0, new ListEditItem("--Select--", ""));
                    dst.Clear();

                    dst = objCommonBL.GetPinByCity(data[3]);
                    vcCmbPin.ValueField = "pin_id";
                    vcCmbPin.TextField = "pin_code";
                    vcCmbPin.DataSource = dst.Tables[0];
                    vcCmbPin.DataBind();
                    vcCmbPin.Items.Insert(0, new ListEditItem("--Select--", ""));
                    dst.Clear();

                    dst = objCommonBL.GetAreaByCity(data[3]);
                    vcCmbArea.ValueField = "area_id";
                    vcCmbArea.TextField = "area_name";
                    vcCmbArea.DataSource = dst.Tables[0];
                    vcCmbArea.DataBind();
                    vcCmbArea.Items.Insert(0, new ListEditItem("--Select--", ""));

                    vcCmbCountry.Value = data[1].ToString();
                    vcCmbState.Value = data[2].ToString();
                    vcCmbCity.Value = data[3].ToString();
                    vcCmbPin.Value = data[4].ToString();
                    vcCmbArea.Value = data[5].ToString();
                }
                else //// while Country,State,City etc data are not available
                {
                    vcCmbCountry.Value = string.Empty;

                    vcCmbState.Items.Clear();
                    vcCmbState.Items.Insert(0, new ListEditItem("--Select--", ""));
                    vcCmbState.Value = string.Empty;

                    vcCmbCity.Items.Clear();
                    vcCmbCity.Items.Insert(0, new ListEditItem("--Select--", ""));
                    vcCmbCity.Value = string.Empty;

                    vcCmbPin.Items.Clear();
                    vcCmbPin.Items.Insert(0, new ListEditItem("--Select--", ""));
                    vcCmbPin.Value = string.Empty;

                    vcCmbArea.Items.Clear();
                    vcCmbArea.Items.Insert(0, new ListEditItem("--Select--", ""));
                    vcCmbArea.Value = string.Empty;
                }
            }
            #endregion
        }

        protected void ComponentPanelMain_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            CommonBL objCommonBL = new CommonBL();

            string[] listData = e.Parameter.Split('~');

            #region ####### Tagging Process ##############
            if (listData[0] == "E") // Tagging Property
            {
                BindDataByDocID(listData[1], listData[2]);
                pnl_ChallanSpeceficFields.Visible = ControlVisibilityByDocType(listData[2], true);
                if (cmbTransporter.Value != null)
                {
                    string data = cmbTransporter.Value + "|" + cmbTransporterType.Value + "|" + radioregistercheck.SelectedValue + "|" +
                   txtGSTIN1.Value + txtGSTIN2.Value + txtGSTIN3.Value + "|" + VehicleNo_hidden.Value + "|" + txtAddress.Value + "|" +
                   ((vcCmbCountry.Value == null) ? "null" : vcCmbCountry.Value) + "|" + ((vcCmbState.Value == null) ? "null" : vcCmbState.Value) + "|" +
                   ((vcCmbCity.Value == null) ? "null" : vcCmbCity.Value) + "|" + ((vcCmbPin.Value == null) ? "null" : vcCmbPin.Value) + "|" +
                  ((vcCmbArea.Value == null) ? "null" : vcCmbArea.Value) + "|" + txtPhone.Value;

                    hfControlSaveData.Value = data;
                }

            }
            #endregion
        }

        public void BindDataByDocID(string docID, string docType)
        {
            DataSet dsControlDetails = objCommonBL.GetTransporterControlDetails(docID, docType);
            DataSet dst = objCommonBL.GetContactTypeTransporter("TR");
            string undeliveredList = hddnFlagFromPendingDlv.Value;
            string DelDetails = string.Empty;
            if (dsControlDetails.Tables[2] != null && dsControlDetails.Tables[2].Rows.Count > 0)
            {
                DelDetails = Convert.ToString(dsControlDetails.Tables[2].Rows[0]["DelDetails"]);
            }
            if (dst.Tables[0] != null && dst.Tables[0].Rows.Count > 0)
            {
                cmbTransporter.ValueField = "cnt_internalId";
                cmbTransporter.TextField = "cnt_Name";
                cmbTransporter.DataSource = dst.Tables[0];
                cmbTransporter.DataBind();

                cmbTransporter.Items.Insert(0, new ListEditItem("--Select--", ""));
            }

            if (dst.Tables[0] != null && dst.Tables[0].Rows.Count > 0)
            {
                cmbFinalTransporter.ValueField = "cnt_internalId";
                cmbFinalTransporter.TextField = "cnt_Name";
                cmbFinalTransporter.DataSource = dst.Tables[0];
                cmbFinalTransporter.DataBind();

                cmbFinalTransporter.Items.Insert(0, new ListEditItem("--Select--", ""));
            }

            if (dst.Tables[1] != null && dst.Tables[1].Rows.Count > 0)
            {
                vcCmbCountry.TextField = "Country";
                vcCmbCountry.ValueField = "cou_id";
                vcCmbCountry.DataSource = dst.Tables[1];
                vcCmbCountry.DataBind();

                vcCmbCountry.Items.Insert(0, new ListEditItem("--Select--", ""));
            }


            if (dsControlDetails.Tables[0].Rows.Count > 0)
            {
                DataSet dsVehicleControlDetails = objCommonBL.GetVehicleControlDetails(Convert.ToString(dsControlDetails.Tables[0].Rows[0]["trp_InternalId"]));
                cmbTransporterType.ValueField = "lgl_id";
                cmbTransporterType.TextField = "lgl_legalStatus";
                cmbTransporterType.DataSource = dsVehicleControlDetails.Tables[5];
                cmbTransporterType.DataBind();
                cmbTransporterType.Value = Convert.ToString(dsControlDetails.Tables[0].Rows[0]["trp_TransporterType"]);

                if (dsVehicleControlDetails.Tables[1].Rows.Count > 0)
                {
                    MasterVehicleNo_hidden.Value = dsVehicleControlDetails.Tables[1].Rows[0]["VehicleNos"].ToString();
                }

                vcCmbCountry.Value = Convert.ToString(dsControlDetails.Tables[0].Rows[0]["trp_CountryId"]);

                if (undeliveredList.ToUpper() == "UNDELIVEREDLIST")
                {
                    //if (DelDetails != "1")
                    //{
                        cmbTransporter.Value = Convert.ToString(dsControlDetails.Tables[0].Rows[0]["trp_InternalId"]);
                        if (!string.IsNullOrEmpty(Convert.ToString(dsControlDetails.Tables[0].Rows[0]["trp_FinalTransId"])))
                        {
                            if (Convert.ToString(dsControlDetails.Tables[0].Rows[0]["trp_FinalTransId"]).ToUpper()=="NULL")
                            {
                                cmbFinalTransporter.SelectedIndex = 0;
                            }
                            else
                            {
                                cmbFinalTransporter.Value = Convert.ToString(dsControlDetails.Tables[0].Rows[0]["trp_FinalTransId"]);
                            }
                            
                        }
                    //}

                }
                else if (undeliveredList.ToUpper() == "PENDINGDELIVERY")
                {
                    cmbTransporter.Value = Convert.ToString(dsControlDetails.Tables[0].Rows[0]["trp_InternalId"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dsControlDetails.Tables[0].Rows[0]["trp_FinalTransId"])))
                    {
                        cmbFinalTransporter.Value = Convert.ToString(dsControlDetails.Tables[0].Rows[0]["trp_FinalTransId"]);
                    }
                }
                else
                {
                    cmbTransporter.Value = Convert.ToString(dsControlDetails.Tables[0].Rows[0]["trp_InternalId"]);
                }
                

                if (dsControlDetails.Tables[0].Rows[0]["trp_GSTIN"].ToString() != "")
                {
                    txtGSTIN1.Value = dsControlDetails.Tables[0].Rows[0]["trp_GSTIN"].ToString().Substring(0, 2);
                    txtGSTIN2.Value = dsControlDetails.Tables[0].Rows[0]["trp_GSTIN"].ToString().Substring(2, 10);
                    txtGSTIN3.Value = dsControlDetails.Tables[0].Rows[0]["trp_GSTIN"].ToString().Substring(12, 3);
                }
                txtAddress.Value = Convert.ToString(dsControlDetails.Tables[0].Rows[0]["trp_Address"]);
                txtPhone.Value = Convert.ToString(dsControlDetails.Tables[0].Rows[0]["trp_Phone"]);
                if (dsControlDetails.Tables[1].Rows.Count > 0)
                {

                    VehicleNo_hidden.Value = Convert.ToString(dsControlDetails.Tables[1].Rows[0]["trpVeh_VechilesNos"]);
                }
                else
                {
                    VehicleNo_hidden.Value = string.Empty;
                }
                radioregistercheck.SelectedValue = Convert.ToString(Convert.ToInt16(dsControlDetails.Tables[0].Rows[0]["trp_IsRegistered"]));
                DataSet dsState = objCommonBL.GetStateByCountry(Convert.ToString(dsControlDetails.Tables[0].Rows[0]["trp_CountryId"]));

                vcCmbState.ValueField = "state_id";
                vcCmbState.TextField = "state_name";
                vcCmbState.DataSource = dsState.Tables[0];
                vcCmbState.DataBind();
                vcCmbState.Items.Insert(0, new ListEditItem("--Select--", ""));

                vcCmbState.Value = Convert.ToString(dsControlDetails.Tables[0].Rows[0]["trp_StateId"]);
                dsState.Clear();

                dsState = objCommonBL.GetCityByState(Convert.ToString(dsControlDetails.Tables[0].Rows[0]["trp_StateId"]));

                vcCmbCity.ValueField = "city_id";
                vcCmbCity.TextField = "city_name";
                vcCmbCity.DataSource = dsState.Tables[0];
                vcCmbCity.DataBind();
                vcCmbCity.Items.Insert(0, new ListEditItem("--Select--", ""));
                vcCmbCity.Value = Convert.ToString(dsControlDetails.Tables[0].Rows[0]["trp_CityId"]);
                dsState.Clear();

                dsState = objCommonBL.GetPinByCity(Convert.ToString(dsControlDetails.Tables[0].Rows[0]["trp_CityId"]));
                vcCmbPin.ValueField = "pin_id";
                vcCmbPin.TextField = "pin_code";
                vcCmbPin.DataSource = dsState.Tables[0];
                vcCmbPin.DataBind();
                vcCmbPin.Items.Insert(0, new ListEditItem("--Select--", ""));
                vcCmbPin.Value = Convert.ToString(dsControlDetails.Tables[0].Rows[0]["trp_PinCode"]);
                dsState.Clear();

                dsState = objCommonBL.GetAreaByCity(Convert.ToString(dsControlDetails.Tables[0].Rows[0]["trp_CityId"]));
                vcCmbArea.ValueField = "area_id";
                vcCmbArea.TextField = "area_name";
                vcCmbArea.DataSource = dsState.Tables[0];
                vcCmbArea.DataBind();
                vcCmbArea.Items.Insert(0, new ListEditItem("--Select--", ""));
                vcCmbArea.Value = Convert.ToString(dsControlDetails.Tables[0].Rows[0]["trp_Area"]);
                dsState.Clear();

                txtFreight.Text = Convert.ToString(dsControlDetails.Tables[0].Rows[0]["trp_FreightCharge"]);
                txtPoint.Text = Convert.ToString(dsControlDetails.Tables[0].Rows[0]["trp_LocationPoint"]);
                txtLoading.Text = Convert.ToString(dsControlDetails.Tables[0].Rows[0]["trp_LoadingCharge"]);
                txtUnloading.Text = Convert.ToString(dsControlDetails.Tables[0].Rows[0]["trp_UnloadingCharge"]);
                txtParking.Text = Convert.ToString(dsControlDetails.Tables[0].Rows[0]["trp_ParkingCharge"]);
                txtWeighment.Text = Convert.ToString(dsControlDetails.Tables[0].Rows[0]["trp_Weight"]);
                txtTollTax.Text = Convert.ToString(dsControlDetails.Tables[0].Rows[0]["trp_TollTax"]);
                txtLrno.Text = Convert.ToString(dsControlDetails.Tables[0].Rows[0]["Lrno"]);

                txtTotalCharges.Text = Convert.ToString(dsControlDetails.Tables[0].Rows[0]["trp_TotalCharges"]);
                vcCmbTrip.Value = Convert.ToString(dsControlDetails.Tables[0].Rows[0]["trp_Trip"]);
                vcCmbFreightArea.Value = (Convert.ToString(dsControlDetails.Tables[0].Rows[0]["trp_FreightArea"]) == "0") ? string.Empty : Convert.ToString(dsControlDetails.Tables[0].Rows[0]["trp_FreightArea"]);

                if (dsControlDetails.Tables[0].Rows[0]["VehicleOutDate"] != null && dsControlDetails.Tables[0].Rows[0]["VehicleOutDate"].ToString() != "")
                {
                    dtVehicleOutDate.Date = Convert.ToDateTime(dsControlDetails.Tables[0].Rows[0]["VehicleOutDate"].ToString());
                }
            }
        }

        public string GetControlValue(string controlID)
        {
            string returnVal = string.Empty;

            switch (controlID)
            {
                case "cmbTransporter":
                    returnVal = Convert.ToString(cmbTransporter.Value);
                    break;
            }

            return returnVal;

        }

        public bool ControlVisibilityByDocType(string DocType, bool TagStatus = false)
        {
            //bool returnVal = false;
            bool returnVal = true; // Temporary Visible for all modules
            try
            {
                switch (TagStatus)
                {
                    case true:
                        if (DocType.ToUpper().Trim() == "QO" || DocType.ToUpper().Trim() == "SO" || DocType.ToUpper().Trim() == "PB" || DocType.ToUpper().Trim() == "SC" || DocType.ToUpper().Trim() == "PO" || DocType.ToUpper().Trim() == "PC")
                        {
                            returnVal = true;
                        }

                        break;
                    case false:
                        if (DocType.ToUpper().Trim() == "SI" || DocType.ToUpper().Trim() == "PB" || DocType.ToUpper().Trim() == "SC" || DocType.ToUpper().Trim() == "PC")
                        {
                            returnVal = true;
                        }

                        break;
                }
                return returnVal;
            }
            catch (Exception ex) { return true; }
        }

    }
}