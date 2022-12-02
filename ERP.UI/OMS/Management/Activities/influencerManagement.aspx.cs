using BusinessLogicLayer;
using ERP.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ERP.OMS.Management.Activities
{
    public partial class influencerManagement : System.Web.UI.Page
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            SqlSchematype.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);

        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                PopulateBranchByHierchy(Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]));
            }
        }

        private void PopulateBranchByHierchy(string userbranchhierchy)
        {
            PosSalesInvoiceBl posSale = new PosSalesInvoiceBl();
            DataTable branchtable = posSale.getBranchListByHierchy(userbranchhierchy);
            cmbBranchfilter.DataSource = branchtable;
            cmbBranchfilter.ValueField = "branch_id";
            cmbBranchfilter.TextField = "branch_description";
            cmbBranchfilter.DataBind();
            cmbBranchfilter.SelectedIndex = 0;
        }


        protected void EntityServerModeDataSource_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "Invoice_Id";
            DBEngine objdb = new DBEngine();
            var str = objdb.GetFieldValue("tbl_Master_SystemControl", "Last_YearDb", "1=1", 1);

            string connectionstring = "";
            //string connectionString = ConfigurationManager.ConnectionStrings["crmConnectionString"].ConnectionString;
            ReadWriteMasterDatabaseBL objMasterbl = new ReadWriteMasterDatabaseBL();
            string connectionString = objMasterbl.GetConnectionString(Convert.ToString(str[0, 0]));




            string IsFilter = Convert.ToString(hfIsFilter.Value);
            string strFromDate = Convert.ToString(hfFromDate.Value);
            string strToDate = Convert.ToString(hfToDate.Value);
            string strBranchID = (Convert.ToString(hfBranchID.Value) == "") ? "0" : Convert.ToString(hfBranchID.Value);

            List<int> branchidlist;
            ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
            if (IsFilter == "Y")
            {
                if (strBranchID == "0")
                {
                    string BranchList = Convert.ToString(Session["userbranchHierarchy"]);
                    branchidlist = new List<int>(Array.ConvertAll(BranchList.Split(','), int.Parse));
                    var q = from d in dc.v_posListInfluencers
                            where d.Invoice_DateTimeFormat >= Convert.ToDateTime(strFromDate) &&
                                  d.Invoice_DateTimeFormat <= Convert.ToDateTime(strToDate) &&
                                  d.Invoice_CompanyID == Convert.ToString(Session["LastCompany"]) &&
                                  branchidlist.Contains(Convert.ToInt32(d.Invoice_branchId))
                            orderby d.Invoice_DateTimeFormat descending
                            select d;
                    e.QueryableSource = q;
                }
                else
                {
                    branchidlist = new List<int>(Array.ConvertAll(strBranchID.Split(','), int.Parse));
                    var q = from d in dc.v_posListInfluencers
                            where d.Invoice_DateTimeFormat >= Convert.ToDateTime(strFromDate) &&
                                  d.Invoice_DateTimeFormat <= Convert.ToDateTime(strToDate) &&
                                  d.Invoice_CompanyID == Convert.ToString(Session["LastCompany"]) &&
                                  branchidlist.Contains(Convert.ToInt32(d.Invoice_branchId))
                            orderby d.Invoice_DateTimeFormat descending
                            select d;
                    e.QueryableSource = q;
                }
            }
            else
            {
                var q = from d in dc.v_posListInfluencers
                        where d.Invoice_branchId == '0' &&
                                d.Invoice_CompanyID == Convert.ToString(Session["LastCompany"])
                        orderby d.Invoice_DateTimeFormat descending
                        select d;
                e.QueryableSource = q;
            }
        }
        [WebMethod]
        public static object GetInfluencerDetails(string invid)
        {
            ReadWriteMasterDatabaseBL objMasterbl = new ReadWriteMasterDatabaseBL();
            DBEngine objdb = new DBEngine();
            var str = objdb.GetFieldValue("tbl_Master_SystemControl", "Last_YearDb","1=1",  1);

            string connectionstring = "";
            DataSet ds = new DataSet();
            opInfluencerBl posSale = new opInfluencerBl();
            ds = posSale.GetInfluencerDetails(invid, Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));

            Inf_Header_Details IHD = new Inf_Header_Details();

            INF_Inv_Details inv = new INF_Inv_Details();


            inv.Inv_Id = Convert.ToString(ds.Tables[0].Rows[0]["Inv_Id"]);
            inv.Inv_No = Convert.ToString(ds.Tables[0].Rows[0]["Inv_No"]);
            inv.Amount = Convert.ToString(ds.Tables[0].Rows[0]["Amount"]);
            inv.Inv_BranchId = Convert.ToString(ds.Tables[0].Rows[0]["Inv_BranchId"]);

            IHD.INF_Inv_Details = inv;

            Influencer inf = new Influencer();
            if (ds.Tables[2] != null && ds.Tables[2].Rows.Count > 0)
            {
                inf.MainAccount_AccountCode = Convert.ToString(ds.Tables[2].Rows[0]["MainAccount_AccountCode"]);
                inf.CALCULATED_AMOUNT = Convert.ToString(ds.Tables[2].Rows[0]["CALCULATED_AMOUNT"]);
                inf.MAINACCOUNT_DR = Convert.ToString(ds.Tables[2].Rows[0]["MAINACCOUNT_DR"]);
                inf.AMOUNT_DR = Convert.ToString(ds.Tables[2].Rows[0]["AMOUNT_DR"]);
                inf.AUTOJV_ID = Convert.ToString(ds.Tables[2].Rows[0]["AUTOJV_ID"]);
                inf.AUTOJV_NUMBER = Convert.ToString(ds.Tables[2].Rows[0]["AUTOJV_NUMBER"]);
                inf.COMM_AMOUNT = Convert.ToString(ds.Tables[2].Rows[0]["COMM_AMOUNT"]);
                inf.POSTING_DATE = Convert.ToDateTime(ds.Tables[2].Rows[0]["POSTING_DATE"]);
                inf.IsTagged = Convert.ToString(ds.Tables[2].Rows[0]["TaggedCount"]);
            }

            IHD.Influencer = inf;




            IHD.INF_Inv_Products = (from DataRow dr in ds.Tables[1].Rows
                                    select new INF_Inv_Products()
                                    {

                                        prod_details_id = Convert.ToString(dr["prod_details_id"]),
                                        Prod_id = Convert.ToString(dr["Prod_id"]),
                                        Prod_description = Convert.ToString(dr["Prod_description"]),
                                        prod_Qty = Convert.ToString(dr["prod_Qty"]),
                                        prod_Salesprice = Convert.ToString(dr["prod_Salesprice"]),
                                        prod_amt = Convert.ToString(dr["prod_amt"]),
                                        prod_SalespriceWithGST = Convert.ToString(dr["prod_amtWGST"]),
                                        Prod_Percentage = Convert.ToString(dr["Prod_Percentage"]),
                                        Applicable_On = Convert.ToString(dr["Applicable_On"]),
                                        PROD_COMM_AMOUNT = Convert.ToString(dr["PROD_COMM_AMOUNT"])


                                    }).ToList();

            IHD.Influencer_Details = (from DataRow dr in ds.Tables[3].Rows
                                      select new Influencer_Details()
                                      {

                                          DET_AMOUNT_CR = Convert.ToString(dr["DET_AMOUNT_CR"]),
                                          DET_INFLUENCER_ID = Convert.ToString(dr["DET_INFLUENCER_ID"]),
                                          INF_Name = Convert.ToString(dr["INF_Name"]),
                                          DET_MAINACCOUNT_CR = Convert.ToString(dr["DET_MAINACCOUNT_CR"]),
                                          DET_MAINACCOUNT_NAME = Convert.ToString(dr["DET_MAINACCOUNT_NAME"])
                                      }).ToList();



            if (ds.Tables[4] != null && ds.Tables[4].Rows.Count > 0)
            {
                IHD.RemainingBalance = Convert.ToString(ds.Tables[4].Rows[0]["RemainingQty"]);
            }


            return IHD;
        }

        [WebMethod]
        public static object GetInfluencerReturnDetails(string invid)
        {
            DataSet ds = new DataSet();
            PosSalesInvoiceBl posSale = new PosSalesInvoiceBl();
            ds = posSale.GetInfluencerReturnDetails(invid);

            infulencerReturnAdjustMent IHD = new infulencerReturnAdjustMent();
            //infulencerReturnAdjustMentDetails inv = new infulencerReturnAdjustMentDetails();
            //infulencerReturnAdjustMentDetails ret = new infulencerReturnAdjustMentDetails();

            IHD.Invoice_Data = (from DataRow dr in ds.Tables[0].Rows
                                select new infulencerReturnAdjustMentDetails()
                                {
                                    DOC_ID = Convert.ToString(dr["INVOICE_ID"]),
                                    CON_ID = Convert.ToString(dr["CON_ID"]),
                                    NAME = Convert.ToString(dr["NAME"]),
                                    DOC_NUMBER = Convert.ToString(dr["INVOICE_NUMBER"]),
                                    DOC_DATE = Convert.ToDateTime(dr["INVOICE_DATE"]).ToString("dd-MM-yyyy"),
                                    Total_Comm = Convert.ToString(dr["Total_Comm"]),
                                    Unpaid = Convert.ToString(dr["Unpaid"])
                                }).ToList();
            IHD.Return_Data = (from DataRow dr in ds.Tables[1].Rows
                               select new infulencerReturnAdjustMentDetails()
                               {
                                   DOC_ID = Convert.ToString(dr["RETURN_ID"]),
                                   CON_ID = Convert.ToString(dr["CON_ID"]),
                                   NAME = Convert.ToString(dr["NAME"]),
                                   DOC_NUMBER = Convert.ToString(dr["RETURN_NUMBER"]),
                                   DOC_DATE = Convert.ToDateTime(dr["RETURN_DATE"]).ToString("dd-MM-yyyy"),
                                   Total_Comm = Convert.ToString(dr["Total_Comm"]),
                                   Unpaid = Convert.ToString(dr["Unpaid"])
                               }).ToList();



            return IHD;
        }
        public static DataTable CreateDataTable<T>(IEnumerable<T> list)
        {
            Type type = typeof(T);
            var properties = type.GetProperties();
            DataTable dataTable = new DataTable();
            foreach (PropertyInfo info in properties)
            {
                dataTable.Columns.Add(new DataColumn(info.Name, typeof(System.String)));
            }
            foreach (T entity in list)
            {
                object[] values = new object[properties.Length];
                for (int i = 0; i < properties.Length; i++)
                {
                    values[i] = properties[i].GetValue(entity);
                } dataTable.Rows.Add(values);
            }
            return dataTable;
        }

        [WebMethod]
        public static object SaveInfluencer(infulencerSaveData infsave)
        {
            DataTable Prod = CreateDataTable(infsave.product);
            DataTable Influencer = CreateDataTable(infsave.Influencer);

            InfluencerBL ibl = new InfluencerBL();
            string output = ibl.SaveInfluencerData(infsave, Prod, Influencer, "");

            return output;
        }

        [WebMethod]
        public static object SaveInfluencerAdj(List<INF_ADJ> invoice, List<INF_ADJ> returns)
        {
            DataTable dtInvoice = new DataTable();
            DataTable dtReturn = new DataTable();

            //DataTable dtInvoice = CreateDataTable(invoice);
            //DataTable dtReturn = CreateDataTable(returns);

            dtInvoice.Columns.Add("AMOUNT", typeof(decimal));
            dtInvoice.Columns.Add("DOC_ID", typeof(string));
            dtInvoice.Columns.Add("INF_ID", typeof(string));

            dtReturn.Columns.Add("AMOUNT", typeof(decimal));
            dtReturn.Columns.Add("DOC_ID", typeof(string));
            dtReturn.Columns.Add("INF_ID", typeof(string));


            foreach (INF_ADJ invDet in invoice)
            {
                dtInvoice.Rows.Add(invDet.AMOUNT, invDet.DOC_ID, invDet.INF_ID);
            }
            foreach (INF_ADJ retDet in returns)
            {
                dtReturn.Rows.Add(retDet.AMOUNT, retDet.DOC_ID, retDet.INF_ID);

            }

            InfluencerBL ibl = new InfluencerBL();
            string output = ibl.SaveInfluencerAdjustmentData(dtInvoice, dtReturn);

            return output;
        }

        [WebMethod]
        public static object DeleteInfluencer(string Invoice_Id)
        {

            InfluencerBL ibl = new InfluencerBL();
            string output = ibl.DeleteInfluencerData(Invoice_Id);

            return output;
        }


    }
}