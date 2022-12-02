using CRM.Models.DataContext;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace CRM.Models
{
    public class crmAccount
    {
        public string Action { get; set; }
        public string Account_InternalId { get; set; }
        public int Account_Id { get; set; }
        public string Account_Name { get; set; }
        public string AccountCode { get; set; }
        public Int64 OwnerID { get; set; }
        public Int64 AssignedID { get; set; }
        public Int64 Status_Id { get; set; }
        public decimal Est_Reveneu { get; set; }
        public DateTime? Est_Close_Date { get; set; }
        public string Mobile { get; set; }
        public string Fax { get; set; }
        public string ParrentAccount { get; set; }
        public string ParrentAccountName { get; set; }
        public string TickerSymbol { get; set; }
        public int Reletionship_Type { get; set; }
        public int Product_PriceList { get; set; }
        public AccountCustomerDetails acccustdetails { get; set; }
        public Int64 IndustryID { get; set; }
        public string GSTIN_CODE { get; set; }
        public int OwnershipId { get; set; }
        public string Description { get; set; }
        public int Credit_Limit { get; set; }
        public int Credit_Hold { get; set; }
        public int Payment_Terms { get;set;}
        public int Contact_Method { get; set; }

        public string crmcontacts_id { get; set; }
        public List<String> cntids { get; set; }

        public string crmoportunity_id { get; set; }
        public List<String> oppids { get; set; }


        public int Email { get; set; }
        public int Phone { get; set; }
        public int CNTFax { get; set; }
        public int Shipping_Method { get; set; }
        public int Freight_Terms { get; set; }

        public string Website { get; set; }
        public string stree1 { get; set; }
        public string stree2 { get; set; }
        public string stree3 { get; set; }
        public string Landmark { get; set; }
        public string pin { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string country { get; set; }

        public List<V_UserLIst> Users { get; set; }
        public List<v_StatusDetail> Status_Details { get; set; }
        public List<V_Industry> Industries { get; set; }
        public List<v_RelationshipType> listRelationshipType { get; set; }
        public List<v_PaymentTerm> listPaymentTerm { get; set; }
        public List<v_Ownership> listOwnership { get; set; }
        public List<v_ContactMethod> listContactMethod { get; set; }
        public List<v_Email> listEmail { get; set; }
        public List<v_Phone> listPhone { get; set; }
        public List<v_Fax> listFax { get; set; }
        public List<v_ShippingMethod> listShippingMethod { get; set; }
        public List<v_Freight> listFreight { get; set; }


        public List<v_crmContactSelect> contactList { get; set; }
        public List<v_crmOppSelect> oppList { get; set; }


        public string SaveAccount(crmAccount CRMAccounts)
        {

            try
            {
                string output = string.Empty;

                DataSet dsInst = new DataSet();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("PROC_CRM_ACCCOUNTINSERT", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", CRMAccounts.Action);
                cmd.Parameters.AddWithValue("@Account_Id", CRMAccounts.Account_Id);
                cmd.Parameters.AddWithValue("@Owner_id", CRMAccounts.OwnerID);
                cmd.Parameters.AddWithValue("@Assign_To", CRMAccounts.AssignedID);
                cmd.Parameters.AddWithValue("@Status_id", CRMAccounts.Status_Id);
                cmd.Parameters.AddWithValue("@Est_Revenue", CRMAccounts.Est_Reveneu);
                cmd.Parameters.AddWithValue("@EstCloseDate", CRMAccounts.Est_Close_Date);
                cmd.Parameters.AddWithValue("@AccountName", CRMAccounts.Account_Name);
                cmd.Parameters.AddWithValue("@AccountCode", CRMAccounts.AccountCode);
                cmd.Parameters.AddWithValue("@MobilePhone", CRMAccounts.Mobile);
                cmd.Parameters.AddWithValue("@Fax", CRMAccounts.Fax);
                cmd.Parameters.AddWithValue("@AccDetParent_internalId", CRMAccounts.ParrentAccount);
                cmd.Parameters.AddWithValue("@AccDet_TickerSymbol", CRMAccounts.TickerSymbol);
                cmd.Parameters.AddWithValue("@AccDet_RelationshipType", CRMAccounts.Reletionship_Type);
                cmd.Parameters.AddWithValue("@AccDet_ProductPriceList", CRMAccounts.Product_PriceList);

                cmd.Parameters.AddWithValue("@Website", CRMAccounts.acccustdetails.add_Website);
                cmd.Parameters.AddWithValue("@Address1", CRMAccounts.acccustdetails.Address1);
                cmd.Parameters.AddWithValue("@Address2", CRMAccounts.acccustdetails.Address2);
                cmd.Parameters.AddWithValue("@Address3", CRMAccounts.acccustdetails.Address3);
                cmd.Parameters.AddWithValue("@Landmark", CRMAccounts.acccustdetails.Landmark);
                cmd.Parameters.AddWithValue("@PinZip_Id", CRMAccounts.acccustdetails.PinId);
                cmd.Parameters.AddWithValue("@Country_Id", CRMAccounts.acccustdetails.CountryId);
                cmd.Parameters.AddWithValue("@State_Id", CRMAccounts.acccustdetails.StateId);
                cmd.Parameters.AddWithValue("@City_Id", CRMAccounts.acccustdetails.CityId);

                cmd.Parameters.AddWithValue("@Industry_Id", CRMAccounts.IndustryID);
                cmd.Parameters.AddWithValue("@AccDet_GSTINCODE", CRMAccounts.GSTIN_CODE);
                cmd.Parameters.AddWithValue("@AccDet_Ownership", CRMAccounts.OwnershipId);
                cmd.Parameters.AddWithValue("@AccDet_Description", CRMAccounts.Description);

                cmd.Parameters.AddWithValue("@AccDet_CreditLimit", CRMAccounts.Credit_Limit);
                cmd.Parameters.AddWithValue("@AccDet_CreditHold", CRMAccounts.Credit_Hold);
                cmd.Parameters.AddWithValue("@AccDet_PaymentTerms", CRMAccounts.Payment_Terms);


                cmd.Parameters.AddWithValue("@AccDet_ContactMethod", CRMAccounts.Contact_Method);
                cmd.Parameters.AddWithValue("@AccDet_Email", CRMAccounts.Email);
                cmd.Parameters.AddWithValue("@AccDet_Phone", CRMAccounts.Phone);
                cmd.Parameters.AddWithValue("@AccDet_contFax", CRMAccounts.CNTFax);
                cmd.Parameters.AddWithValue("@AccDet_ShippingMethod", CRMAccounts.Shipping_Method);
                cmd.Parameters.AddWithValue("@AccDet_FreightTerms", CRMAccounts.Freight_Terms);
                cmd.Parameters.AddWithValue("@AccDet_EnteredBy", HttpContext.Current.Session["userid"]);
                cmd.Parameters.AddWithValue("@contacts", CRMAccounts.crmcontacts_id);
                cmd.Parameters.AddWithValue("@opportunity", CRMAccounts.crmoportunity_id);

                

                cmd.Parameters.Add("@result", SqlDbType.Char, 50);
                cmd.Parameters["@result"].Direction = ParameterDirection.Output;


                cmd.CommandTimeout = 0;
                SqlDataAdapter Adap = new SqlDataAdapter();
                Adap.SelectCommand = cmd;
                Adap.Fill(dsInst);
                cmd.Dispose();
                output = (string)cmd.Parameters["@result"].Value;
                con.Dispose();

                return Convert.ToString("Data save");



            }
            catch (Exception ex)
            {
                return "Please try again later.";
            }
            return "";
        }

        public DataSet EditCRMeAccount(crmAccount CRMAccounts)
        {

            try
            {
                string output = string.Empty;
                DataSet dsInst = new DataSet();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("PROC_CRM_ACCCOUNTINSERT", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", "Edit");
                cmd.Parameters.AddWithValue("@Account_Id", CRMAccounts.Account_Id);
                cmd.Parameters.Add("@result", SqlDbType.Char, 50);
                cmd.Parameters["@result"].Direction = ParameterDirection.Output;
                cmd.CommandTimeout = 0;
                SqlDataAdapter Adap = new SqlDataAdapter();
                Adap.SelectCommand = cmd;
                Adap.Fill(dsInst);
                cmd.Dispose();
                output = (string)cmd.Parameters["@result"].Value;
                con.Dispose();

                return dsInst;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public string DeleteAccounts(crmAccount CRMAccounts)
        {

            try
            {
                string returnmessage = "";
                string output = string.Empty;
                DataSet dsInst = new DataSet();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("PROC_CRM_ACCCOUNTINSERT", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", "Delete");
                cmd.Parameters.AddWithValue("@Account_Id", CRMAccounts.Account_Id);
                cmd.Parameters.Add("@result", SqlDbType.Char, 50);
                cmd.Parameters["@result"].Direction = ParameterDirection.Output;
                cmd.CommandTimeout = 0;
                SqlDataAdapter Adap = new SqlDataAdapter();
                Adap.SelectCommand = cmd;
                Adap.Fill(dsInst);
                cmd.Dispose();
                output = (string)cmd.Parameters["@result"].Value;
                con.Dispose();
                if (output.Trim() == "1")
                {
                    returnmessage = "Deleted Successfully.";
                }
                else
                {
                    returnmessage = "Error Occured.";
                }
                return returnmessage;

            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
    public class AccountCustomerDetails
    {
        public string Id { get; set; }
        public string CustType { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public int CountryId { get; set; }
        public string CountryName { get; set; }
        public int StateId { get; set; }
        public string StateName { get; set; }
        public string StateCode { get; set; }
        public int CityId { get; set; }
        public string CityName { get; set; }
        public int AreaId { get; set; }
        public string AreaName { get; set; }
        public string PinId { get; set; }
        public string PinCode { get; set; }

        public decimal Distance { get; set; }

        public string GSTIN { get; set; }
        public string Landmark { get; set; }
        public string add_Website { get; set; }
        public bool Isdefault { get; set; }
        public string add_FaxNo { get; set; }


     

    }

    public class crmParentAccountModel
    {
        public string id { get; set; }
        public string Na { get; set; }
        public string Code { get; set; }
      
    }

}