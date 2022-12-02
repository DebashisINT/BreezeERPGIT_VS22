using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OpeningEntry.UserControls
{
    public partial class OpeningSales_BillingShipping : System.Web.UI.UserControl
    {

        BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DataTable DT = objEngine.GetDataTable("Config_SystemSettings", " Variable_Value ", " Variable_Name='BillingShipping_ShipToParty'");

                if (DT != null && DT.Rows.Count > 0)
                {
                    string IsVisible = Convert.ToString(DT.Rows[0]["Variable_Value"]).Trim();

                    if (IsVisible == "Yes")
                    {
                        divShipToParty.Style.Add("display", "block");
                    }
                    else
                    {
                        divShipToParty.Style.Add("display", "none");
                    }
                }
            }

        }


        public string GetShippingStateId()
        {
            return hdStateIdShipping.Value;
        }

        public string GeteShippingStateCode()
        {
            return hdStateCodeShipping.Value;
        }

        public string GetBillingStateCode()
        {
            return hdStateCodeBilling.Value;
        }
        public string GetBillingStateId()
        {
            return hdStateIdBilling.Value;
        }


        public DataTable GetBillingShippingTable()
        {
            DataTable bsDetails = new DataTable();
            bsDetails.Columns.Add("AddressType", typeof(System.String));
            bsDetails.Columns.Add("Address1", typeof(System.String));
            bsDetails.Columns.Add("Address2", typeof(System.String));
            bsDetails.Columns.Add("Address3", typeof(System.String));
            bsDetails.Columns.Add("Landmark", typeof(System.String));
            bsDetails.Columns.Add("PinId", typeof(System.Int64));
            bsDetails.Columns.Add("AreaId", typeof(System.Int64));
            bsDetails.Columns.Add("Distance", typeof(System.Decimal));
            bsDetails.Columns.Add("GSTIN", typeof(System.String));
            bsDetails.Columns.Add("ShipToPartyId", typeof(System.String));


            DataRow BillShipDtls = bsDetails.NewRow();
            BillShipDtls["AddressType"] = "Billing";
            BillShipDtls["Address1"] = txtAddress1.Text;
            BillShipDtls["Address2"] = txtAddress2.Text;
            BillShipDtls["Address3"] = txtAddress3.Text;
            BillShipDtls["Landmark"] = txtlandmark.Text;
            if (!string.IsNullOrEmpty(hdBillingPin.Value))
            {
                BillShipDtls["PinId"] = (Convert.ToInt64(hdBillingPin.Value));
            }
            else
            {
                BillShipDtls["PinId"] = Convert.ToInt64(0);
            }
            if (!string.IsNullOrEmpty(hdAreaIdBilling.Value))
                BillShipDtls["AreaId"] = Convert.ToInt64(hdAreaIdBilling.Value);
            else
                BillShipDtls["AreaId"] = Convert.ToInt64(0);
            BillShipDtls["Distance"] = txtDistance.Text;
            BillShipDtls["GSTIN"] = (txtBillingGSTIN1.Text).Trim() + (txtBillingGSTIN2.Text).Trim() + (txtBillingGSTIN3.Text).Trim();
            bsDetails.Rows.Add(BillShipDtls);

            BillShipDtls = bsDetails.NewRow();
            BillShipDtls["AddressType"] = "Shipping";
            BillShipDtls["Address1"] = txtsAddress1.Text;
            BillShipDtls["Address2"] = txtsAddress2.Text;
            BillShipDtls["Address3"] = txtsAddress3.Text;
            BillShipDtls["Landmark"] = txtslandmark.Text;
            //BillShipDtls["PinId"] = (Convert.ToInt64(hdShippingPin.Value));

            if (!string.IsNullOrEmpty(hdShippingPin.Value))
            {
                BillShipDtls["PinId"] = (Convert.ToInt64(hdShippingPin.Value));
            }
            else
            {
                BillShipDtls["PinId"] = Convert.ToInt64(0);
            }

            if (!string.IsNullOrEmpty(hdAreaIdShipping.Value))
            {
                BillShipDtls["AreaId"] = (Convert.ToInt64(hdAreaIdShipping.Value));
            }
            else
            {
                BillShipDtls["AreaId"] = 0;
            }

            BillShipDtls["Distance"] = txtDistanceShipping.Text;
            BillShipDtls["GSTIN"] = (txtShippingGSTIN1.Text).Trim() + (txtShippingGSTIN2.Text).Trim() + (txtShippingGSTIN3.Text).Trim();
            BillShipDtls["ShipToPartyId"] = hdShipToParty.Value;


            bsDetails.Rows.Add(BillShipDtls);


            return bsDetails;



        }

        public void SetBillingShippingTable(DataTable AddressTable)
        {

            if (AddressTable != null && AddressTable.Rows.Count > 0 && AddressTable.Rows.Count < 3)
            {
                DataRow[] BillingRow = AddressTable.Select("Addresstype='Billing'");
                DataRow[] ShippingRow = AddressTable.Select("Addresstype='Shipping'");

                if (BillingRow.Length > 0)
                {
                    ///////////Billing//////////
                    txtAddress1.Text = Convert.ToString(BillingRow[0]["Address1"]);
                    txtAddress2.Text = Convert.ToString(BillingRow[0]["Address2"]);
                    txtAddress3.Text = Convert.ToString(BillingRow[0]["Address3"]);
                    txtlandmark.Text = Convert.ToString(BillingRow[0]["Landmark"]);
                    txtbillingPin.Text = Convert.ToString(BillingRow[0]["Pincode"]);
                    hdBillingPin.Value = Convert.ToString(BillingRow[0]["PinId"]);
                    txtbillingCountry.Text = Convert.ToString(BillingRow[0]["CountryName"]);
                    txtbillingState.Text = Convert.ToString(BillingRow[0]["StateName"]);
                    txtbillingCity.Text = Convert.ToString(BillingRow[0]["CityName"]);
                    hdCountryIdBilling.Value = Convert.ToString(BillingRow[0]["CountryID"]);
                    hdStateIdBilling.Value = Convert.ToString(BillingRow[0]["StateId"]);
                    hdCityIdBilling.Value = Convert.ToString(BillingRow[0]["CityId"]);
                    hdStateCodeBilling.Value = Convert.ToString(BillingRow[0]["StateCode"]);
                    txtSelectBillingArea.Text = Convert.ToString(BillingRow[0]["AreaName"]);
                    hdAreaIdBilling.Value = Convert.ToString(BillingRow[0]["AreaId"]);
                    txtDistance.Text = Convert.ToString(BillingRow[0]["Distance"]);

                    string GSTIN = Convert.ToString(BillingRow[0]["GSTIN"]);
                    if (GSTIN.Length > 14)
                    {
                        txtBillingGSTIN1.Text = GSTIN.Substring(0, 2);
                        txtBillingGSTIN2.Text = GSTIN.Substring(2, GSTIN.Length - 5);
                        txtBillingGSTIN3.Text = GSTIN.Substring(GSTIN.Length - 3);
                    }
                    //txtBillingGSTIN1.Text = Convert.ToString(BillingRow[0]["GSTIN"]);
                    //txtShippingGSTIN2.Text = Convert.ToString(BillingRow[0]["GSTIN"]);
                    //txtShippingGSTIN3.Text = Convert.ToString(BillingRow[0]["GSTIN"]);
                }
                if (ShippingRow.Length > 0)
                {
                    ////////Shipping ////////////
                    txtsAddress1.Text = Convert.ToString(ShippingRow[0]["Address1"]);
                    txtsAddress2.Text = Convert.ToString(ShippingRow[0]["Address2"]);
                    txtsAddress3.Text = Convert.ToString(ShippingRow[0]["Address3"]);
                    txtslandmark.Text = Convert.ToString(ShippingRow[0]["Landmark"]);
                    txtShippingPin.Text = Convert.ToString(ShippingRow[0]["Pincode"]);
                    hdShippingPin.Value = Convert.ToString(ShippingRow[0]["PinId"]);
                    txtshippingCountry.Text = Convert.ToString(ShippingRow[0]["CountryName"]);
                    txtshippingState.Text = Convert.ToString(ShippingRow[0]["StateName"]);
                    txtshippingCity.Text = Convert.ToString(ShippingRow[0]["CityName"]);
                    hdCountryIdShipping.Value = Convert.ToString(ShippingRow[0]["CountryID"]);
                    hdStateIdShipping.Value = Convert.ToString(ShippingRow[0]["StateId"]);
                    hdCityIdShipping.Value = Convert.ToString(ShippingRow[0]["CityId"]);
                    hdStateCodeShipping.Value = Convert.ToString(ShippingRow[0]["StateCode"]);
                    txtSelectShippingArea.Text = Convert.ToString(ShippingRow[0]["AreaName"]);
                    hdAreaIdShipping.Value = Convert.ToString(ShippingRow[0]["AreaId"]);
                    txtDistanceShipping.Text = Convert.ToString(ShippingRow[0]["Distance"]);

                    string GSTINS = Convert.ToString(ShippingRow[0]["GSTIN"]);
                    if (GSTINS.Length > 14)
                    {
                        txtShippingGSTIN1.Text = GSTINS.Substring(0, 2);
                        txtShippingGSTIN2.Text = GSTINS.Substring(2, GSTINS.Length - 5);
                        txtShippingGSTIN3.Text = GSTINS.Substring(GSTINS.Length - 3);
                    }
                    //txtShippingGSTIN1.Text = Convert.ToString(ShippingRow[0]["GSTIN"]);
                    //txtShippingGSTIN2.Text = Convert.ToString(ShippingRow[0]["GSTIN"]);
                    //txtShippingGSTIN3.Text = Convert.ToString(ShippingRow[0]["GSTIN"]);

                    hdShipToParty.Value = Convert.ToString(ShippingRow[0]["ShipToPartyId"]);
                    txtShipToPartyShippingAdd.Text = Convert.ToString(ShippingRow[0]["ShipToPartyName"]);
                }
            }

        }


        protected void Button1_Click(object sender, EventArgs e)
        {
            GetBillingShippingTable();
        }
    }
}