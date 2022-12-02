using CRM.Models.DataContext;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace CRM.Models
{
    public class crmMembers
    {
        public List<v_EntityList> EntityList { get; set; }
        public List<string> Selectedvalues { get; set; }

        internal string SaveCRMMember(string Module_Name, string Module_Id, string Entity_list)
        {
            try
            {
                int OutputId = 0;
                DataSet dsInst = new DataSet();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("crm_AddMembers", con);

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ACTION", "Add");
                cmd.Parameters.AddWithValue("@Module_Name", Module_Name);
                cmd.Parameters.AddWithValue("@Module_Id", Module_Id);
                cmd.Parameters.AddWithValue("@CustomerList", Entity_list);
                cmd.Parameters.AddWithValue("@userid", Convert.ToInt32(System.Web.HttpContext.Current.Session["userid"]));

                SqlParameter output = new SqlParameter("@ReturnMessage", SqlDbType.VarChar, 500);
                output.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(output);

                cmd.CommandTimeout = 0;
                SqlDataAdapter Adap = new SqlDataAdapter();
                Adap.SelectCommand = cmd;
                Adap.Fill(dsInst);
                cmd.Dispose();
                con.Dispose();
                                
                string strCPRID = Convert.ToString(cmd.Parameters["@ReturnMessage"].Value.ToString());
                return strCPRID;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        internal DataTable GetEditedData(string Module_Name, string Module_Id)
        {
            try
            {
                int OutputId = 0;
                DataSet dsInst = new DataSet();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("crm_AddMembers", con);

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ACTION", "EditDetails");
                cmd.Parameters.AddWithValue("@Module_Name", Module_Name);
                cmd.Parameters.AddWithValue("@Module_Id", Module_Id);
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

    public class crmMemberSelelected
    {
        public string cnt_internalid { get; set; }
        public string cnt_ContactType { get; set; }
    }
}