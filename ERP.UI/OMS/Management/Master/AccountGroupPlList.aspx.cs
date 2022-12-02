using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ERP.OMS.Management.Master
{
    public partial class AccountGroupPlList : System.Web.UI.Page
    {
        public EntityLayer.CommonELS.UserRightsForPage rights = new EntityLayer.CommonELS.UserRightsForPage();

        protected void Page_Init(object sender, EventArgs e)
        {
            LayoutDbSource.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/Master/AccountGroupPlList.aspx");
        }

        protected void SelectPanel_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
        {
            string LayoutName = txtLayoutName.Text;
            string LayoutDescription = txtLayoutDescription.Text;

            SaveNewLayout(LayoutName, LayoutDescription);
        }

        private void SaveNewLayout(string LayoutName, string LayoutDescription)
        {
            try
            {

                DataSet dsInst = new DataSet();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("prc_LayoutInsertDelete", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Action", "Insert");//1
                cmd.Parameters.AddWithValue("@layoutName", LayoutName);//2
                cmd.Parameters.AddWithValue("@Layoutdescription", LayoutDescription);//3
                cmd.Parameters.AddWithValue("@userid", Convert.ToInt32(Convert.ToString(Session["userid"])));//4
                cmd.CommandTimeout = 0;
                SqlDataAdapter Adap = new SqlDataAdapter();
                Adap.SelectCommand = cmd;
                Adap.Fill(dsInst);
                cmd.Dispose();
                con.Dispose();
                if (dsInst.Tables[0] != null && dsInst.Tables[0].Rows.Count > 0)
                {
                    SelectPanel.JSProperties["cpAutoID"] = "Success";
                    
                }
                else
                {
                    SelectPanel.JSProperties["cpAutoID"] = "Error";
                    
                }
            }
            catch (Exception e)
            {
                SelectPanel.JSProperties["cpAutoID"] = "Error";
                
            }
        }

        [WebMethod]
        public static bool DoActivate(string Key)
        {
            DataSet dsInst = new DataSet();
            SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            SqlCommand cmd = new SqlCommand("InsertInLayoutDetails", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Action", "ActivateLayout");//1
            cmd.Parameters.AddWithValue("@layoutId", Key);//2
            cmd.CommandTimeout = 0;
            SqlDataAdapter Adap = new SqlDataAdapter();
            Adap.SelectCommand = cmd;
            Adap.Fill(dsInst);
            cmd.Dispose();
            con.Dispose();


            return true;
        }


        [WebMethod]
        public static bool DoDelete(string Key)
        {
            DataSet dsInst = new DataSet();
            SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            SqlCommand cmd = new SqlCommand("InsertInLayoutDetails", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Action", "DeleteLayout");//1
            cmd.Parameters.AddWithValue("@layoutId", Key);//2
            cmd.CommandTimeout = 0;
            SqlDataAdapter Adap = new SqlDataAdapter();
            Adap.SelectCommand = cmd;
            Adap.Fill(dsInst);
            cmd.Dispose();
            con.Dispose();

            if (dsInst.Tables[0].Rows[0][0] == "Success")
                return true;
            else
                return false;
        }

    }
}