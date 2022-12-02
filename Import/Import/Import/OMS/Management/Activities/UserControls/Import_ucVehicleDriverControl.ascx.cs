using BusinessLogicLayer;
using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Web;

namespace ERP.OMS.Management.Activities.UserControls
{
    public partial class Import_ucVehicleDriverControl : System.Web.UI.UserControl
    {
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        SlaesActivitiesBL objSlaesActivitiesBL = new SlaesActivitiesBL();
        SalesInvoiceBL objSalesInvoiceBL = new SalesInvoiceBL();
        CommonBL objCommonBL = new CommonBL();
        VehicleDriverBL ObjVehicleDriverBL = new VehicleDriverBL();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine();
                DataTable DT = objEngine.GetDataTable("Config_SystemSettings", " Variable_Value ", " Variable_Name='Show_VehicleDriver' AND IsActive=1");
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
                //radioregistercheck.Attributes.Add("onclick", "registeredCheckChangeEvent()");
                string userbranch = Convert.ToString(Session["userbranchHierarchy"]);
                string strCompanyID = Convert.ToString(Session["LastCompany"]);
                string strBranchID = Convert.ToString(Session["userbranchID"]);

                DataSet dstDT = new DataSet();

                dstDT.Clear();

                dstDT = ObjVehicleDriverBL.GetVehicleByActiveStatus("1"); // get all active vehicle. 
                if (dstDT.Tables[0] != null && dstDT.Tables[0].Rows.Count > 0)
                {
                    VDcmbTransporter.ValueField = "vehicle_Id";
                    VDcmbTransporter.TextField = "vehicle_regNo";
                    VDcmbTransporter.DataSource = dstDT.Tables[0];
                    VDcmbTransporter.DataBind();

                    VDcmbTransporter.Items.Insert(0, new ListEditItem("--Select--", ""));
                }
            }
        }
        protected void VDcmbTransporterType_Callback(object sender, CallbackEventArgsBase e)
        {
            string WhichCall = e.Parameter.Split('~')[0];
            string vehicle_Regno = Convert.ToString(e.Parameter.Split('~')[0]);
            if (vehicle_Regno != "")
            {
                Dictionary<string, object> obj = ObjVehicleDriverBL.PopulateVehicleDetails(vehicle_Regno);
                VDcmbTransporterType.JSProperties["cpVDBind"] = obj;
            }
            else
            {
                VDcmbTransporterType.JSProperties["cpVDBind"] = "";
            }

        }
        protected void VDComponentPanelMain_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            CommonBL objCommonBL = new CommonBL();

            string[] listData = e.Parameter.Split('~');

            if (listData[0] == "SaveVehicleDriverData" && listData.Length == 5)
            {
                DocwiseVehicledriverModel obj = new DocwiseVehicledriverModel()
                {
                    CreatedBy = Convert.ToInt32(HttpContext.Current.Session["userid"]),
                    DocId = listData[2],
                    DocType = listData[3],
                    DriversID = listData[4],
                    VehicleRegNo = listData[1]
                };

                int output = ObjVehicleDriverBL.SaveVehicleDriverData(obj);

                if (output > 0)
                {
                    hfDocId.Value = obj.DocId;
                    hfDocType.Value = obj.DocType;
                    VDcallBackuserControlPanelMain.JSProperties["cpSaveVehicleDriverData"] = "success";
                }
                else
                {
                    hfDocId.Value = obj.DocId;
                    hfDocType.Value = obj.DocType;
                    VDcallBackuserControlPanelMain.JSProperties["cpSaveVehicleDriverData"] = "error";
                }
            }
        }
        public string GetControl(string controlID)
        {
            string returnVal = string.Empty;

            switch (controlID)
            {
                case "hfDocId":
                    returnVal = Convert.ToString(hfDocId.Value);
                    break;

                case "hfDocType":
                    returnVal = Convert.ToString(hfDocType.Value);
                    break;
            }

            return returnVal;

        }
        public void SetControl(string controlID, string value)
        {
            switch (controlID)
            {
                case "hfDocId":
                    hfDocId.Value = value;
                    break;

                case "hfDocType":
                    hfDocType.Value = value;
                    break;
            }
        }
    }
}