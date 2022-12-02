using BusinessLogicLayer;
using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OpeningEntry.UserControls
{
    public partial class VehicleDetailsControl : System.Web.UI.UserControl
    {
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        SlaesActivitiesBL objSlaesActivitiesBL = new SlaesActivitiesBL();
        SalesInvoiceBL objSalesInvoiceBL = new SalesInvoiceBL();
        CommonBL objCommonBL = new CommonBL();

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
                DataTable DT = objEngine.GetDataTable("Config_SystemSettings", " Variable_Value ", " Variable_Name='Show_Transporter' AND IsActive=1");
                if (DT != null && DT.Rows.Count > 0)
                {
                    string IsVisible = Convert.ToString(DT.Rows[0]["Variable_Value"]).Trim();

                    if (IsVisible == "Yes")
                    {
                        this.Visible = true;
                    }
                    else
                    {
                        this.Visible = false;
                    }
                }
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

                if (dstDT.Tables[1] != null && dstDT.Tables[1].Rows.Count > 0)
                {
                    vcCmbCountry.TextField = "Country";
                    vcCmbCountry.ValueField = "cou_id";
                    vcCmbCountry.DataSource = dstDT.Tables[1];
                    vcCmbCountry.DataBind();

                    vcCmbCountry.Items.Insert(0, new ListEditItem("--Select--", ""));
                }
                ////######## Coded By Samrat Roy -- This Functionality is to bind data into Transporter Control by Sales Order ID 
                if (Request.QueryString["key"] != null && Request.QueryString["key"] != "ADD")
                {
                    BindDataByDocID(Request.QueryString["key"], Request.QueryString["type"]);
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
            }
            #endregion
        }

        private void BindDataByDocID(string docID, string docType)
        {
            DataSet dsControlDetails = objCommonBL.GetTransporterControlDetails(docID, docType);
            DataSet dst = objCommonBL.GetContactTypeTransporter("TR");
            if (dst.Tables[0] != null && dst.Tables[0].Rows.Count > 0)
            {
                cmbTransporter.ValueField = "cnt_internalId";
                cmbTransporter.TextField = "cnt_Name";
                cmbTransporter.DataSource = dst.Tables[0];
                cmbTransporter.DataBind();

                cmbTransporter.Items.Insert(0, new ListEditItem("--Select--", ""));
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
                cmbTransporter.Value = Convert.ToString(dsControlDetails.Tables[0].Rows[0]["trp_InternalId"]);

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
    }
}