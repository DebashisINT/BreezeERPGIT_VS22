using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ERP.OMS.Management.Master
{
    public partial class Root_AddCashBank : System.Web.UI.Page
    {
        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);MULTI
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        protected void Page_Init(object sender, EventArgs e)
        {
            CashBankDataSource.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
              
                CashBankLookUp.Focus();
                BindCashBankAccount();
                //string userid = Request.QueryString["id"];
                //hdnUserID.Value = userid;
            }
        }
        public void BindCashBankAccount()
        {
            DataTable dtCashBank = new DataTable();
            string CompanyId = Convert.ToString(Session["LastCompany"]);
            string userid = Request.QueryString["id"];
            dtCashBank = GetCashBankGridDetails(userid);
            if (dtCashBank.Rows.Count > 0)
            {
                CashBankMapping.DataSource = dtCashBank;
                CashBankMapping.DataBind();
            }
            else
            {
                CashBankMapping.DataSource = dtCashBank;
                CashBankMapping.DataBind();
            }

        }
        public DataTable GetCashBankGridDetails(string userid)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_UserDetail");
            proc.AddVarcharPara("@Action", 100, "GetCashBankGridDetails");
            proc.AddBigIntegerPara("@userid", Convert.ToInt32(userid));
            //proc.AddVarcharPara("@CompanyId", 100, CompanyId);

            ds = proc.GetTable();
            return ds;
        }
        protected void BtnSave_Click(object sender, EventArgs e)
        {

            String CashBank_IDS = "";
            //List<object> CashBankList = CashBankLookUp.GridView.GetSelectedFieldValues("MainAccount_ReferenceID");
            //foreach (object Pro in CashBankList)
            //{
            //    CashBank_IDS += "," + Pro;
            //}
            //CashBank_IDS = CashBank_IDS.TrimStart(',');

            CashBank_IDS = Convert.ToString(CashBankLookUp.Value);
            if(CashBank_IDS!="")
            {           
                string userid = Request.QueryString["id"];

                DataSet dsInst = new DataSet();
                //SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]);MULTI
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("prc_UserDetail", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Action", "UserCashBankInsert");//1
                cmd.Parameters.AddWithValue("@userid", Convert.ToInt32(userid));//6
                cmd.Parameters.AddWithValue("@Entity_IDS", CashBank_IDS);//2
                cmd.Parameters.AddWithValue("@ModuleName", "UserCashBank");//3
                SqlParameter output = new SqlParameter("@ReturnValue", SqlDbType.Int);
                output.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(output);

                cmd.CommandTimeout = 0;
                SqlDataAdapter Adap = new SqlDataAdapter();
                Adap.SelectCommand = cmd;
                Adap.Fill(dsInst);
                cmd.Dispose();
                con.Dispose();


                Response.Redirect("Root_AddCashBank.aspx?id=" + userid);
            }
            else
            {

                ScriptManager.RegisterStartupScript(this, GetType(), "JScript1", "jAlert('Please Select Cash/Bank!!');", true);
            }
        }
        protected void CashBankMapping_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            if (e.Parameters != "")
            {
                string tranid = Convert.ToString(e.Parameters);
                oDBEngine.DeleteValue("tbl_trans_CashBankMapping ", "CashBankMapID ='" + tranid + "'");              
            }
            BindCashBankAccount();
        }

        protected void CashBankMapping_DataBinding(object sender, EventArgs e)
        {
            DataTable dtCashBank = new DataTable();
           // string CompanyId = Convert.ToString(Session["LastCompany"]);
            string userid = Request.QueryString["id"];
            dtCashBank = GetCashBankGridDetails(userid);
            if (dtCashBank.Rows.Count > 0)
            {
                CashBankMapping.DataSource = dtCashBank;
                
            }
        }
    }
}