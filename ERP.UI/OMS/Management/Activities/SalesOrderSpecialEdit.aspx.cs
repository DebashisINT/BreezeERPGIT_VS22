using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using UtilityLayer;

namespace ERP.OMS.Management.Activities
{
    public partial class SalesOrderSpecialEdit : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                Session["SI_ComponentData_Salesorder_Special"] = null;
            }
        }

        [WebMethod]
        public static object SaleorderDetails(string saleordernumber)
        {
            string OrderId = "";
            string Oredernumber = "";

            ProcedureExecute proc = new ProcedureExecute("Get_SaleorderSpecialEdit");
            proc.AddPara("@Action", "Salordercheck");
            proc.AddPara("@SaleordrNumber", saleordernumber);
            DataTable dt = proc.GetTable();

            if (dt.Rows.Count > 0)
            {
                OrderId = dt.Rows[0]["Order_Id"].ToString();
                Oredernumber = dt.Rows[0]["Order_Number"].ToString();
            }

            var returnObject = new { status = "Ok", OrderId = OrderId, Oredernumber = Oredernumber };
            return returnObject;
        }

        [WebMethod]
        public static object ChangeSaleOrderInvoice(string ordernumber, string userId)
        {

            string invoiceId = "";
            string invoceNumbr = "";


            ProcedureExecute proc = new ProcedureExecute("proc_SalesOrderSpecialEdit");
            proc.AddPara("@OrderNumber", ordernumber);
            proc.AddPara("@NewCreatedBy", userId);
            proc.AddPara("@Action", "ChangeCreateatedBy");
            DataTable dt = proc.GetTable();
           

            var returnObject = new { status = "Ok", invoiceId = invoiceId, invoceNumbr = invoceNumbr };
            return returnObject;

        }


        #region User Populate

        protected void ComponentUserLogin_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            string FinYear = Convert.ToString(Session["LastFinYear"]);
            string BRANCH_ID = "";
            string QuoComponent = "";

            if (e.Parameter.Split('~')[0] == "Userbind")
            {
                DataTable ComponentTable = new DataTable();
                string saleorderid = e.Parameter.Split('~')[1];
                if (saleorderid != "0")
                {
                    ProcedureExecute proc = new ProcedureExecute("Get_SaleorderSpecialEdit");
                    proc.AddPara("@Action", "Userdetails");
                    proc.AddPara("@SaleordrNumber", saleorderid);
                    DataTable dt = proc.GetTable();


                    if (dt.Rows.Count > 0)
                    {
                        Session["SI_ComponentData_Salesorder_Special"] = dt;
                        griduserloginIdLookup.DataSource = dt;
                        griduserloginIdLookup.DataBind();
                       
                    }
                    else
                    {
                        griduserloginIdLookup.DataSource = null;
                        griduserloginIdLookup.DataBind();
                    }
                }
                else
                {
                    griduserloginIdLookup.DataSource = null;
                    griduserloginIdLookup.DataBind();
                    Session["SI_ComponentData_Salesorder_Special"] = null;

                }
            }
        }


        protected void lookup_LoginID_DataBinding(object sender, EventArgs e)
        {
            if (Session["SI_ComponentData_Salesorder_Special"] != null)
            {
                griduserloginIdLookup.DataSource = (DataTable)Session["SI_ComponentData_Salesorder_Special"];
            }
            else
            {
                griduserloginIdLookup.DataSource = null;

            }
        }

        #endregion


    }
}