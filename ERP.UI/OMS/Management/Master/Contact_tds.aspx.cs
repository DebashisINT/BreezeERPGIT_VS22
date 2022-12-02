using BusinessLogicLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ERP.OMS.Management.Master
{
    public partial class Contact_tds : System.Web.UI.Page
    {
        BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine();
        VendorTDSBl tdsdetails = new VendorTDSBl();
        protected void Page_Load(object sender, EventArgs e)
        {
            string InternalId = Convert.ToString(Session["KeyVal_InternalID"]);
            if (InternalId != "")
            {
                if (!IsPostBack)
                {
                    showDtat(InternalId);
                }
            }
        }

        private void showDtat(string InternalId)
        {
            #region TaxGroupType DropDown Start

            DataTable tdsMaster = tdsdetails.GetTDSMASTERLIST();
            if (tdsMaster != null && tdsMaster.Rows.Count > 0)
            {
                aspxDeducteesNew.TextField = "TYPE_NAME";
                aspxDeducteesNew.ValueField = "ID";
                aspxDeducteesNew.DataSource = tdsMaster;
                aspxDeducteesNew.DataBind();
            }
            #endregion TaxGroupType DropDown Start
            DataTable tdsDetails = tdsdetails.GetVendorTdsDetails(InternalId);
            if (tdsDetails.Rows.Count > 0)
            {
                HdMode.Value = "Edit";
                aspxDeductees.Value = tdsDetails.Rows[0]["TDS_Deductees"];
            }
            else
            {
                HdMode.Value = "Add";
            }

            DataTable tdsDetailsContact = tdsdetails.GetVendorTdsDetailsContact(InternalId);
            if (tdsDetailsContact.Rows.Count > 0)
            {
                HdMode.Value = "Edit";
                aspxDeducteesNew.Value = tdsDetailsContact.Rows[0]["TDSRATE_TYPE"].ToString();
            }
            else
            {
                HdMode.Value = "Add";
            }
        }

        protected void BtnSave_Click(object sender, EventArgs e)
        {
            string InternalId = Convert.ToString(Session["KeyVal_InternalID"]);
            if (Convert.ToString(HdMode.Value) == "Add")
            {
                // tdsdetails.SaveVendorTDSMap(InternalId, Convert.ToString(aspxDeductees.Value), Convert.ToInt32(Session["userid"]));
                tdsdetails.UpdateTDSMap(InternalId, Convert.ToString(aspxDeducteesNew.Value));
                HdMode.Value = "Edit";
            }
            else if (Convert.ToString(HdMode.Value) == "Edit")
            {
                // tdsdetails.UpdateVendorTDSMap(InternalId, Convert.ToString(aspxDeductees.Value), Convert.ToInt32(Session["userid"]));
                tdsdetails.UpdateTDSMap(InternalId, Convert.ToString(aspxDeducteesNew.Value));
            }

        }
    }
}