using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;


namespace CRM.DAL
{
    public class Validation
    {

        public int CheckExistancy(string tablename,string columnname,List<ValidCondition> valid)
        {
            int existcheck = 0;
            string condition = "";
            foreach (var item in valid)
            {
                if (condition=="")
                {
                    condition=item.column+"="+"'"+item.value+"'";
                }
                else
                {
                    condition = condition+" and "+ item.column + "=" +"'"+ item.value+"'";
                }
            }

            string query = "";
            query = query + "select " + columnname + " from " + tablename + " where " + condition + " ";



            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            SqlCommand cmd = new SqlCommand(query, con);
            
            SqlDataAdapter Adap = new SqlDataAdapter();
            Adap.SelectCommand = cmd;
            Adap.Fill(dt);
            if(dt.Rows.Count>0){
                existcheck = 1;
            }
            else
            {
                existcheck = 0;
            }
            cmd.Dispose();
            con.Dispose();


            return existcheck;
        }
    }
    public class ValidCondition
    {
        public string column{get;set;}
        public string value{get;set;}
    }
}