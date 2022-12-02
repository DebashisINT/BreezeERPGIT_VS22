using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace CRM.Models
{
    public class crmProducts
    {

        internal string SaveCRMProducts(System.Data.DataTable dt_activityproducts, string Module_Name, string Module_id)
        {
            try
            {
                int OutputId = 0;
                DataSet dsInst = new DataSet();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("crm_AddEditCRMProducts", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ACTION", "Add");
                cmd.Parameters.AddWithValue("@Module_Name", Module_Name);
                cmd.Parameters.AddWithValue("@Module_id", Module_id);
                cmd.Parameters.AddWithValue("@ActivityProducts", dt_activityproducts);
                cmd.CommandTimeout = 0;
                SqlDataAdapter Adap = new SqlDataAdapter();
                Adap.SelectCommand = cmd;
                Adap.Fill(dsInst);
                cmd.Dispose();
                con.Dispose();
                return "Product Added.";
            }
            catch (Exception ex)
            {
                return "Please try again later.";
            }
        }

        internal DataTable GetCRMProductsDetails(string Module_Name, string Module_id)
        {
            try
            {
                int OutputId = 0;
                DataSet dsInst = new DataSet();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("crm_AddEditCRMProducts", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ACTION", "EditDetails");
                cmd.Parameters.AddWithValue("@Module_Name", Module_Name);
                cmd.Parameters.AddWithValue("@Module_id", Module_id);
                cmd.CommandTimeout = 0;
                SqlDataAdapter Adap = new SqlDataAdapter();
                Adap.SelectCommand = cmd;
                Adap.Fill(dsInst);
                cmd.Dispose();
                con.Dispose();
                return dsInst.Tables[0];
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }

    public class crmProd
    {
        public string guid { get; set; }
        public int ActivityId { get; set; }
        public string Lead_Entity_id { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal Quantity { get; set; }
        public decimal Rate { get; set; }
        public string Remarks { get; set; }
        public string Frequency { get; set; }
        public decimal Amount { get; set; }
    }
}