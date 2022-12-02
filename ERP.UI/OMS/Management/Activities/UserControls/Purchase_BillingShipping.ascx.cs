using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ERP.OMS.Management.Activities.UserControls
{
    public partial class Purchase_BillingShipping : System.Web.UI.UserControl
    {
        //BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);

        BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine();

        protected void Page_Load(object sender, EventArgs e)
        {
            if ((Request.FilePath.ToUpper() == "/OMS/MANAGEMENT/ACTIVITIES/SALESRETURN.ASPX") || (Request.FilePath.ToUpper() == "/OMS/MANAGEMENT/ACTIVITIES/RETURNMANUAL.ASPX") || (Request.FilePath.ToUpper() == "/OMS/MANAGEMENT/ACTIVITIES/PROJECTSALESRETURN.ASPX"))
            {
               //billing              
                dvlblcntperson.Style.Add("Display", "block");
                dvtxtCntperson.Style.Add("Display", "block");
                dvlblPhone.Style.Add("Display", "block");
                dvtxtPhone.Style.Add("Display", "block");
                dvlblemail.Style.Add("Display", "block");
                dvtxtemail.Style.Add("Display", "block");
                //shipping
                dvshiplblCntPerson.Style.Add("Display", "block");
                dvshiptxtCntPerson.Style.Add("Display", "block");
                dvshiplblPhone.Style.Add("Display", "block");
                dvshiptxtPhone.Style.Add("Display", "block");
                dvshiplblemail.Style.Add("Display", "block");
                dvshiptxtemail.Style.Add("Display", "block");
            }
            else
            {
                //billing              
                dvlblcntperson.Style.Add("Display", "none");
                dvtxtCntperson.Style.Add("Display", "none");
                dvlblPhone.Style.Add("Display", "none");
                dvtxtPhone.Style.Add("Display", "none");
                dvlblemail.Style.Add("Display", "none");
                dvtxtemail.Style.Add("Display", "none");
                //shipping
                dvshiplblCntPerson.Style.Add("Display", "none");
                dvshiptxtCntPerson.Style.Add("Display", "none");
                dvshiplblPhone.Style.Add("Display", "none");
                dvshiptxtPhone.Style.Add("Display", "none");
                dvshiplblemail.Style.Add("Display", "none");
                dvshiptxtemail.Style.Add("Display", "none");
            }

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


        
        public string GetBillingStateCode()
        {
            return hdStateCodeBilling.Value;
        }
        public string GetBillingStateId()
        {
            return hdStateIdBilling.Value;
        }

        public void GetGSTIN(string Gst)
        {
            hfVendorGSTIN.Value = Gst;
        }
        public string GetShippingStateId()
        {
            return hdStateIdShipping.Value;
        }

        public string GeteShippingStateCode()
        {
            return hdStateCodeShipping.Value;
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
            bsDetails.Columns.Add("GSTIN", typeof(System.String));
            bsDetails.Columns.Add("ShipToPartyId", typeof(System.String));
            if ((Request.FilePath.ToUpper() == "/OMS/MANAGEMENT/ACTIVITIES/SALESRETURN.ASPX") || (Request.FilePath.ToUpper() == "/OMS/MANAGEMENT/ACTIVITIES/RETURNMANUAL.ASPX") || (Request.FilePath.ToUpper() == "/OMS/MANAGEMENT/ACTIVITIES/PROJECTSALESRETURN.ASPX"))
            {
                bsDetails.Columns.Add("ContactPerson", typeof(System.String));
                bsDetails.Columns.Add("Phone", typeof(System.String));
                bsDetails.Columns.Add("Email", typeof(System.String));

            }

            DataRow BillShipDtls = bsDetails.NewRow();
            BillShipDtls["AddressType"] = "Billing";
            BillShipDtls["Address1"] = txtAddress1.Text;
            BillShipDtls["Address2"] = txtAddress2.Text;
            BillShipDtls["Address3"] = txtAddress3.Text;
            BillShipDtls["Landmark"] = txtlandmark.Text;
            //BillShipDtls["PinId"] = (Convert.ToInt64(hdBillingPin.Value));

            if (!string.IsNullOrEmpty(hdBillingPin.Value))
            {
                BillShipDtls["PinId"] = (Convert.ToInt64(hdBillingPin.Value));

            }
            else
            {
                BillShipDtls["PinId"] = Convert.ToInt64(0);
            }
            if (!string.IsNullOrEmpty(hdAreaIdBilling.Value))
            {
                BillShipDtls["AreaId"] = (Convert.ToInt64(hdAreaIdBilling.Value));
            }
            else
            {
                BillShipDtls["AreaId"] = 0;
            }

            BillShipDtls["GSTIN"] = (txtBillingGSTIN1.Text).Trim() + (txtBillingGSTIN2.Text).Trim() + (txtBillingGSTIN3.Text).Trim();

            if ((Request.FilePath.ToUpper() == "/OMS/MANAGEMENT/ACTIVITIES/SALESRETURN.ASPX") || (Request.FilePath.ToUpper() == "/OMS/MANAGEMENT/ACTIVITIES/RETURNMANUAL.ASPX") || (Request.FilePath.ToUpper() == "/OMS/MANAGEMENT/ACTIVITIES/PROJECTSALESRETURN.ASPX"))
            {
                BillShipDtls["ContactPerson"] = txtCntperson.Text;
                BillShipDtls["Phone"] = Salesbillingphone.Value;
                BillShipDtls["Email"] = txtEmail.Text;

            }



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


            BillShipDtls["GSTIN"] = (txtShippingGSTIN1.Text).Trim() + (txtShippingGSTIN2.Text).Trim() + (txtShippingGSTIN3.Text).Trim();
            //BillShipDtls["ShipToPartyId"] = hdShipToParty.Value;
            if (!string.IsNullOrEmpty(hdShipToParty.Value))
            {
                BillShipDtls["ShipToPartyId"] = Convert.ToString(hdShipToParty.Value);
            }
            else
            {
                BillShipDtls["ShipToPartyId"] = 0;
            }


            if ((Request.FilePath.ToUpper() == "/OMS/MANAGEMENT/ACTIVITIES/SALESRETURN.ASPX") || (Request.FilePath.ToUpper() == "/OMS/MANAGEMENT/ACTIVITIES/RETURNMANUAL.ASPX") || (Request.FilePath.ToUpper() == "/OMS/MANAGEMENT/ACTIVITIES/PROJECTSALESRETURN.ASPX"))
            {

                BillShipDtls["ContactPerson"] = txtshipCntPerson.Text;
                BillShipDtls["Phone"] = txtshipPhone.Text;
                BillShipDtls["Email"] = txtshipEmail.Text;

            }

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
                    if ((Request.FilePath.ToUpper() == "/OMS/MANAGEMENT/ACTIVITIES/SALESRETURN.ASPX") || (Request.FilePath.ToUpper() == "/OMS/MANAGEMENT/ACTIVITIES/RETURNMANUAL.ASPX") || (Request.FilePath.ToUpper() == "/OMS/MANAGEMENT/ACTIVITIES/PROJECTSALESRETURN.ASPX"))
                    {
                        txtCntperson.Text = Convert.ToString(BillingRow[0]["ContactPerson"]);
                        Salesbillingphone.Value = Convert.ToString(BillingRow[0]["Phone"]);
                        txtEmail.Text = Convert.ToString(BillingRow[0]["Email"]);

                    }

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


                    string GSTINS = Convert.ToString(ShippingRow[0]["GSTIN"]);
                    if (GSTINS.Length > 14)
                    {
                        txtShippingGSTIN1.Text = GSTINS.Substring(0, 2);
                        txtShippingGSTIN2.Text = GSTINS.Substring(2, GSTINS.Length - 5);
                        txtShippingGSTIN3.Text = GSTINS.Substring(GSTINS.Length - 3);
                    }
                    hdShipToParty.Value = Convert.ToString(ShippingRow[0]["ShipToPartyId"]);
                    // txtShipToPartyShippingAdd.Text = Convert.ToString(ShippingRow[0]["ShipToPartyName"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(ShippingRow[0]["ShipToPartyName"])))
                    {
                        txtShipToPartyShippingAdd.Text = Convert.ToString(ShippingRow[0]["ShipToPartyName"]);
                    }
                    else
                    {
                        txtShipToPartyShippingAdd.Text = "";
                    }
                    if ((Request.FilePath.ToUpper() == "/OMS/MANAGEMENT/ACTIVITIES/SALESRETURN.ASPX") || (Request.FilePath.ToUpper() == "/OMS/MANAGEMENT/ACTIVITIES/RETURNMANUAL.ASPX") || (Request.FilePath.ToUpper() == "/OMS/MANAGEMENT/ACTIVITIES/PROJECTSALESRETURN.ASPX"))
                    {
                        txtshipCntPerson.Text = Convert.ToString(ShippingRow[0]["ContactPerson"]);
                        txtshipPhone.Text = Convert.ToString(ShippingRow[0]["Phone"]);
                        txtshipEmail.Text = Convert.ToString(ShippingRow[0]["Email"]);
                    }

                }
            }

        }


        protected void Button1_Click(object sender, EventArgs e)
        {
            GetBillingShippingTable();
        }


    }
}