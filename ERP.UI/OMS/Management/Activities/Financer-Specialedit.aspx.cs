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
    public partial class Financer_Specialedit : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                Session["SI_ComponentData_Financer_Special"] = null;
            }
        }

        [WebMethod]
        public static object GetInvoiceDetails(string invoiceNumber)
        {
            //  List<InvoiceFinancer> omodel = new List<InvoiceFinancer>();
            string invoiceId = "";
            string invoceNumbr = "";

            ProcedureExecute proc = new ProcedureExecute("Get_FinancerSpecialEdit");
            proc.AddPara("@Action", "InvoiceCheck");
            proc.AddPara("@InvoiceNumber", invoiceNumber);
            DataTable dt = proc.GetTable();
            if (dt.Rows.Count > 0)
            {
                invoiceId = dt.Rows[0]["Invoice_Id"].ToString();
                invoceNumbr = dt.Rows[0]["Invoice_Number"].ToString();
            }

            var returnObject = new { status = "Ok", invoiceId = invoiceId, invoceNumbr = invoceNumbr };
            return returnObject;
        }



        [WebMethod]
        public static object ChangeFinancerInvoice(string invoiceid, string financerid)
        {
            //  List<InvoiceFinancer> omodel = new List<InvoiceFinancer>();
            string invoiceId = "";
            string invoceNumbr = "";

            ProcedureExecute proc = new ProcedureExecute("Change_Financer");
            proc.AddPara("@invoice_id", invoiceid);
            proc.AddPara("@FinId", financerid);
            DataTable dt = proc.GetTable();
           

            var returnObject = new { status = "Ok", invoiceId = invoiceId, invoceNumbr = invoceNumbr };
            return returnObject;
        }



        //public class Invoicedetails
        //{
        //    public string status { get; set; }
        //    public string invoiceId { get; set; }
        //    public List<InvoiceFinancer> financerdetails { get; set; }
        //}

        //public  class InvoiceFinancer
        //{
        //    public string Fin_name { get; set; }
        //    public string Fin_Id{ get; set; }
        //}

        #region Financer Populate

        protected void Componentfinancer_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            string FinYear = Convert.ToString(Session["LastFinYear"]);
            string BRANCH_ID = "";
            string QuoComponent = "";

            if (e.Parameter.Split('~')[0] == "Finacerbind")
            {
                DataTable ComponentTable = new DataTable();
                string ivoiceid = e.Parameter.Split('~')[1];

                ProcedureExecute proc = new ProcedureExecute("Get_FinancerSpecialEdit");
                proc.AddPara("@Action", "Financerdetails");
                proc.AddPara("@InvoiceNumber", ivoiceid);
                DataTable dt = proc.GetTable();


                if (dt.Rows.Count > 0)
                {
                    gridfinancerLookup.DataSource = dt;
                    gridfinancerLookup.DataBind();
                    Session["SI_ComponentData_Financer_Special"] = dt;
                }
                else
                {
                    gridfinancerLookup.DataSource = null;
                    gridfinancerLookup.DataBind();
                }
            }
        }




        protected void lookup_financer_DataBinding(object sender, EventArgs e)
        {
            if (Session["SI_ComponentData_Financer_Special"] != null)
            {
                gridfinancerLookup.DataSource = (DataTable)Session["SI_ComponentData_Financer_Special"];
            }
        }

        #endregion


    }
}