using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;
using BusinessLogicLayer;
using EntityLayer.CommonELS;
using System.Web.Services;
using System.Collections.Generic;
using System.Collections;
using DevExpress.Web.Data;
using DataAccessLayer;
using System.ComponentModel;


namespace ERP.OMS.Management.Activities
{
    public partial class Stock_journalTransferList : System.Web.UI.Page
    {
        Stocktransferjournal objjournal = new Stocktransferjournal();
        SalesInvoiceBL objSalesInvoiceBL = new SalesInvoiceBL();
       // BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);

        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();


        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();


        #region Page Load Section Start
        protected void Page_PreInit(object sender, EventArgs e) // lead add
        {
            if (!IsPostBack)
            {
                string sPath = Convert.ToString(HttpContext.Current.Request.Url);
                oDBEngine.Call_CheckPageaccessebility(sPath);

                //     Bind_BranchCombo();

            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            String ctfinyear = "";
            ctfinyear = Convert.ToString(Session["LastFinYear"]).Trim();
            SlaesActivitiesBL objSlaesActivitiesBL = new SlaesActivitiesBL();
            DataTable dtFinYear = objSlaesActivitiesBL.GetFinacialYearBasedQouteDate(ctfinyear);
            Session["FinYearStartDate"] = Convert.ToString(dtFinYear.Rows[0]["finYearStartDate"]);
            Session["FinYearEndDate"] = Convert.ToString(dtFinYear.Rows[0]["finYearEndDate"]);

            FormDate.MaxDate = Convert.ToDateTime(Session["FinYearEndDate"]);
            FormDate.MinDate = Convert.ToDateTime(Session["FinYearStartDate"]);
            toDate.MaxDate = Convert.ToDateTime(Session["FinYearEndDate"]);
            toDate.MinDate = Convert.ToDateTime(Session["FinYearStartDate"]);

            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/Activities/Stock-journalTransferList.aspx");
            #region Jsproperties Section Initialized Section Start
            gridjournal.JSProperties["cpinsert"] = null;
            gridjournal.JSProperties["cpEdit"] = null;
            gridjournal.JSProperties["cpUpdate"] = null;
            gridjournal.JSProperties["cpDelete"] = null;
            gridjournal.JSProperties["cpExists"] = null;
            gridjournal.JSProperties["cpUpdateValid"] = null;
            #endregion Jsproperties Section Initialized Section Start

            if (!IsPostBack)
            {
                //  GetStockJournalListGridData(Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]));
                FormDate.Date = DateTime.Now;
                toDate.Date = DateTime.Now;
                Session["FromDate"] = null; Session["todate"] = null;

            }
        }
        #endregion Page Load Section End

        #region Main Grid Event Section Start
        public void GetStockJournalListGridData(string userbranch)
        {
            DataTable dtdata = new DataTable();
            string finYear = Convert.ToString(Session["LastFinYear"]);
            dtdata = objjournal.GetListJournals(userbranch, finYear);

            if (dtdata != null && dtdata.Rows.Count > 0)
            {
                gridjournal.DataSource = dtdata;
                gridjournal.DataBind();
            }
            else
            {
                gridjournal.DataSource = null;
                gridjournal.DataBind();
            }

        }


        public void GetStockJournalListGridData(string userbranch, string fromdate, string todate)
        {
            DataTable dtdata = new DataTable();
            string finYear = Convert.ToString(Session["LastFinYear"]);
            dtdata = objjournal.GetListJournals(userbranch, finYear, fromdate, todate, "");

            if (dtdata != null && dtdata.Rows.Count > 0)
            {
                Session["FromDate"] = fromdate;
                Session["todate"] = todate;
                gridjournal.DataSource = dtdata;
                gridjournal.DataBind();
            }
            else
            {
                Session["FromDate"] = null;
                Session["todate"] = null;
                gridjournal.DataSource = null;
                gridjournal.DataBind();
            }

        }


        protected void gridJournal_DataBinding(object sender, EventArgs e)
        {
            string finYear = Convert.ToString(Session["LastFinYear"]);
            if (Session["FromDate"] != null && Session["todate"] != null)
            {

                gridjournal.DataSource = objjournal.GetListJournals(Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]), finYear, Convert.ToString(Session["FromDate"]), Convert.ToString(Session["todate"]), "");
            
            }
            else
            {
             ///gridjournal.DataSource = objjournal.GetListJournals(Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]), finYear);
                gridjournal.DataSource = null;
            }
        }




        protected void Grdstockjournal_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            CRMSalesDtlBL objCRMSalesDtlBL = new CRMSalesDtlBL();
            gridjournal.JSProperties["cpinsert"] = null;
            gridjournal.JSProperties["cpEdit"] = null;
            gridjournal.JSProperties["cpUpdate"] = null;
            gridjournal.JSProperties["cpDelete"] = null;

            int insertcount = 0;

            int updtcnt = 0;
            int deletecnt = 0;
            string WhichCall = Convert.ToString(e.Parameters).Split('~')[0];
            string WhichType = null;
            string QuoteStatus = "";
            string remarks = "";


            if (Convert.ToString(e.Parameters).Contains("~"))
            {
                if (Convert.ToString(e.Parameters).Split('~')[1] != "")
                {
                    WhichType = Convert.ToString(e.Parameters).Split('~')[1];
                }
            }

            if (WhichCall == "Edit")
            {

                DataTable dtQuotationStatus = objCRMSalesDtlBL.GetQuotationStatusByQuotationID(WhichType);


                if (dtQuotationStatus.Rows.Count > 0 && dtQuotationStatus != null)
                {
                    string quoteid = Convert.ToString(dtQuotationStatus.Rows[0]["quoteid"]);
                    string quoteNumber = Convert.ToString(dtQuotationStatus.Rows[0]["quoteNumber"]);
                    string Status = Convert.ToString(dtQuotationStatus.Rows[0]["Status"]);
                    string Remarks = Convert.ToString(dtQuotationStatus.Rows[0]["Remarks"]);
                    string CustomerName = Convert.ToString(dtQuotationStatus.Rows[0]["CustomerName"]);
                    gridjournal.JSProperties["cpEdit"] = quoteid + "~"
                                                    + quoteNumber + "~"
                                                    + Status + "~"
                                                    + Remarks + "~"
                                                    + CustomerName;

                }
            }

            if (WhichCall == "Delete")
            {
                if (!IsStockTransactionExist(Convert.ToString(e.Parameters.Split('~')[1])))
                {
                    deletecnt = objjournal.DeleteJournal(Convert.ToString(e.Parameters).Split('~')[1]);
                    if (deletecnt > 0)
                    {
                        GetStockJournalListGridData(Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]));
                        gridjournal.JSProperties["cpDelete"] = "Deleted successfully";
                    }
                    else
                    {
                        gridjournal.JSProperties["cpDelete"] = "Used in other module.can not delete.";
                    }
                }
                else
                {
                    gridjournal.JSProperties["cpDelete"] = "Available stock will become negative. Cannot Delete.";
                }
            }

            if (WhichCall == "Display")
            {
                GetStockJournalListGridData(Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]));

            }


            else if (WhichCall == "FilterGridByDate")
            {
                string FromDate = (e.Parameters.Split('~')[1]);
                string ToDate = (e.Parameters.Split('~')[2]);
                GetStockJournalListGridData(Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]), FromDate, ToDate);

            }

        }
        private bool IsStockTransactionExist(string pcid)
        {
            bool IsExist = false;
            if (pcid != "" && Convert.ToString(pcid).Trim() != "")
            {
                DataTable dt = new DataTable();
                dt = CheckTranslation(pcid);
                if (dt.Rows.Count > 0 && Convert.ToString(dt.Rows[0]["isexist"]) != "0")
                {
                    IsExist = true;
                }
            }

            return IsExist;
        }
        public DataTable CheckTranslation(string vendorid)
        {
            DataTable dt = new DataTable();

            ProcedureExecute proc = new ProcedureExecute("StockJournal_Modification");
            proc.AddVarcharPara("@Action", 100, "IS_Translation");
            proc.AddVarcharPara("@JournalID", 200, vendorid);

            dt = proc.GetTable();
            return dt;
        }

        #endregion Main Grid Event Section End

        #region Export Grid Section Start
        protected void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 Filter = int.Parse(Convert.ToString(drdExport.SelectedItem.Value));
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
            //GrdReplacement.Columns[6].Visible = false;
            string filename = "Stock Journal";
            exporter.FileName = filename;
            exporter.FileName = "Stock Journal";

            exporter.PageHeader.Left = "Stock Journal Transfer";
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

        #endregion Export Grid Section End


        protected void Grid_b2cs_SummaryDisplayText(object sender, ASPxGridViewSummaryDisplayTextEventArgs e)
        {

            e.Text = string.Format("{0}", e.Value);

            //if (e.Item == grid_b2cs.TotalSummary["Taxable value"])
            //{
            //    e.Text = string.Format("Total Taxable Value={0}", e.Value);

            //}
        }

    }
}