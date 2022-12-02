using DataAccessLayer;
using EntityLayer.CommonELS;
using ERP.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ERP.OMS.Management.Activities
{
    public partial class partyjournalList : System.Web.UI.Page
    {
        public static EntityLayer.CommonELS.UserRightsForPage rights;
        protected void Page_Load(object sender, EventArgs e)
        {
            rights = new UserRightsForPage();
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/Activities/partyjournalList.aspx");

            if (!IsPostBack)
            {
                Session["GridfullInfo"] = null;
            }
        }

        protected void EntityServerModeDataSource_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "ID";

            //string connectionString = ConfigurationManager.ConnectionStrings["crmConnectionString"].ConnectionString;
            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);



            string IsFilter = Convert.ToString(hfIsFilter.Value);
            string strFromDate = Convert.ToString(toDate.Text);
            string strToDate = Convert.ToString(toDate.Text);

            List<int> branchidlist;

            if (IsFilter == "Y")
            {

                string BranchList = Convert.ToString(Session["userbranchHierarchy"]);
                branchidlist = new List<int>(Array.ConvertAll(BranchList.Split(','), int.Parse));

                ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
                var q = from d in dc.V_PARTYJOURALs
                        where
                        d.DOCUMENT_DATE >= FormDate.Date && d.DOCUMENT_DATE <= toDate.Date
                        orderby d.DOCUMENT_DATE descending
                        select d;
                e.QueryableSource = q;

            }
            else
            {
                ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
                var q = from d in dc.V_PARTYJOURALs
                        where "1" == "0"
                        orderby d.DOCUMENT_DATE descending
                        select d;
                e.QueryableSource = q;
            }
        }

        protected void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 Filter = int.Parse(Convert.ToString(drdExport.SelectedValue));
            drdExport.SelectedValue = "0";

            if (Filter != 0)
            {
                if (Session["exportval"] == null)
                {
                    Session["exportval"] = Filter;
                    bindexport(Filter);
                }
                else if (Convert.ToInt32(Session["exportval"]) != Filter)
                {
                    Session["exportval"] = Filter;
                    bindexport(Filter);
                }
            }
        }
        public void bindexport(int Filter)
        {
            // GvJvSearch.Columns[11].Visible = false;

            string filename = "Customer Debit/Credit Note";
            exporter.FileName = filename;

            exporter.PageHeader.Left = "Debit/Credit Note";
            exporter.PageFooter.Center = "[Page # of Pages #]";
            exporter.PageFooter.Right = "[Date Printed]";

            switch (Filter)
            {
                case 1:
                    exporter.WritePdfToResponse();
                    break;
                case 2:
                    exporter.WriteXlsToResponse();
                    break;
                case 3:
                    exporter.WriteRtfToResponse();
                    break;
                case 4:
                    exporter.WriteCsvToResponse();
                    break;
            }
        }

        protected void GridFullInfo_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            int updtcnt = 0;
            int deletecnt = 0;
            string WhichCall = Convert.ToString(e.Parameters).Split('~')[0];
            string WhichType = null;
            string QuoteStatus = "";
            string remarks = "";

            if (WhichCall == "FilterGridByDate")
            {

                string key = Convert.ToString(e.Parameters.Split('~')[1]);
                Session["GridfullInfo"] = FillSearchGridGridFullInfo(key);
                GridFullInfo.DataSource = (DataTable)Session["GridfullInfo"];
                GridFullInfo.DataBind();
            }
        }
        public DataTable FillSearchGridGridFullInfo(string key)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("PRC_PARTYJOURNAL");
            proc.AddVarcharPara("@Action", 500, "GetGridDetailsFull");
            proc.AddVarcharPara("@key", 500, key);
            dt = proc.GetTable();
            return dt;

        }

        protected void GridFullInfo_DataBinding(object sender, EventArgs e)
        {
            if (Session["GridfullInfo"] != null)
            {
                GridFullInfo.DataSource = (DataTable)Session["GridfullInfo"];
            }
        }
    }
}