using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAccessLayer;
using System.Data.SqlClient;
using System.Configuration;
using System.ComponentModel;
using System.Web;
using System.Data;

namespace BusinessLogicLayer
{
  public  class ReportLayout
    {
        public void insert_layout( byte[] data)
        {
             
            try
            {
               // SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]); MULTI
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));

                 SqlCommand com = new SqlCommand("insert into tbl_reportDesigner (LayoutData,ReportName,ModuleKey,CreatedBy) values (@layoutData,@ReportName,@ModuleKey,@CreatedBy)", con);
                con.Open();
                com.Parameters.AddWithValue("@layoutData",data);
                com.Parameters.AddWithValue("@ReportName", "TestReport");
                com.Parameters.AddWithValue("@ModuleKey", "ACT");
                com.Parameters.AddWithValue("@CreatedBy", HttpContext.Current.Session["userid"]);
                com.ExecuteNonQuery();
                con.Close();
                
            }

            catch (Exception ex)
            {
               
            }

            finally
            {
            }
        }

        public byte[] getLayout(int id)
        {
           // SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]); MULTI
            SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));

            SqlCommand com = new SqlCommand("select LayoutData from  tbl_reportDesigner where id=@id", con);
            com.Parameters.AddWithValue("@id", id);
            con.Open();
            var data= (byte[] ) com.ExecuteScalar();
            con.Close();
            return data;
        }

        public bool UpdateLayout(int id, byte[] data)
        {
            try
            {
                //using(SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"])) MULRI
                using (SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"])))
           
                { 
                SqlCommand com = new SqlCommand("ReportDesigner", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@id", id);
                com.Parameters.AddWithValue("@LayoutData", data);
                com.Parameters.AddWithValue("@Mode", "UPD");
                con.Open();
                com.ExecuteNonQuery();

                return true;
                }
            }
            catch
            {
                return false;
            }
           
        }

        public int copyLayout(int copyId, string fileName,string moduleKey)
        {
            int retvalue=0;
            try
            {
                //SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]);
                //SqlCommand com = new SqlCommand("insert into tbl_reportDesigner select LayoutData,'"+fileName+"','"+moduleKey+"',GETDATE()," +Convert.ToString( HttpContext.Current.Session["userid"]) + ",GETDATE() from tbl_reportDesigner where id=@id", con);
                //con.Open();
                //com.Parameters.AddWithValue("@id", copyId); 

               // using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"])) MULTI

                using (SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"])))
                {
                    SqlCommand com = new SqlCommand("ReportDesigner", con);
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@id", copyId);
                    com.Parameters.AddWithValue("@Mode", "CPY");
                    com.Parameters.AddWithValue("@fileName", fileName);
                    com.Parameters.AddWithValue("@ModuleKey", moduleKey);
                    com.Parameters.AddWithValue("@CreatedBy", HttpContext.Current.Session["userid"]);
                    con.Open();
                    com.ExecuteNonQuery();
                    com.CommandText = "select @@IDENTITY";
                    retvalue = Convert.ToInt32(com.ExecuteScalar());
                    con.Close();
                }

              
               
            }

            catch (Exception ex)
            {

            }
            return retvalue;
        }
    }
}
