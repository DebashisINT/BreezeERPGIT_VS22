using DataAccessLayer;
using ERP.Models; 
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DevExpress.Web;
using System.Web.Services;
using EntityLayer.CommonELS;
using DevExpress.XtraPrinting;
using DevExpress.Export;
namespace ERP.OMS.Management.Followup
{
    public partial class Followup : System.Web.UI.Page
    {
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();

        protected void Page_Load(object sender, EventArgs e)
        {
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/Management/Followup/Followup.aspx");
            if (!IsPostBack)
            {
                loadDropdown();
                DateTime dt = new DateTime(2000, 04, 01);


                // FormDate.Date = Convert.ToDateTime(Session["FinYearStart"]);
                FormDate.Date = Convert.ToDateTime(dt);
                

                if (Convert.ToDateTime(Session["FinYearEnd"]) < DateTime.Now)
                    toDate.Date = Convert.ToDateTime(Session["FinYearEnd"]);
                else
                   toDate.Date = DateTime.Now;

              //  FormDate.MinDate = Convert.ToDateTime(Session["FinYearStart"]);
                FormDate.MaxDate = Convert.ToDateTime(Session["FinYearEnd"]);

                toDate.MinDate = Convert.ToDateTime(Session["FinYearStart"]);
                toDate.MaxDate = Convert.ToDateTime(Session["FinYearEnd"]);
            }
        }

        private void loadDropdown()
        {
            ProcedureExecute proc = new ProcedureExecute("prc_Followup");
            proc.AddVarcharPara("@action", 100, "populateParent");
            DataTable dt = proc.GetTable();

            cmbMainUnit.DataSource = dt;
            cmbMainUnit.TextField = "branch_description";
            cmbMainUnit.ValueField = "branch_id";
            cmbMainUnit.DataBind();
        }


        protected void EntityServerModeDataSource_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {

            e.KeyExpression = "Slno";


           // string connectionString = ConfigurationManager.ConnectionStrings["crmConnectionString"].ConnectionString;

            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);



            if (!IsPostBack)
            {
                ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
                var q = from d in dc.tbl_followup_reports
                        where d.CustId == null
                        select d;
                e.QueryableSource = q;
            }
            else
            {
                ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
                var q = from d in dc.tbl_followup_reports
                        where d.UserId == Convert.ToInt64(Session["userid"])
                        select d;
                e.QueryableSource = q;
            }
             
            
            

            
        }

        protected void cmbSubUnit_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
        {
            //ProcedureExecute proc = new ProcedureExecute("prc_Followup");
            //proc.AddVarcharPara("@action", 100, "populateChild");
            //proc.AddVarcharPara("@Parentbranch", 100,Convert.ToString(cmbMainUnit.Value));
            //DataTable dt = proc.GetTable();
            //cmbSubUnit.DataSource = dt;
            //cmbSubUnit.TextField = "branch_description";
            //cmbSubUnit.ValueField = "branch_id";
            //cmbSubUnit.DataBind();
        }

        protected void GridFullYear_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            ProcedureExecute proc = new ProcedureExecute("prc_Followup");
            proc.AddVarcharPara("@action", 100, "PopulateFollowUp");
            proc.AddVarcharPara("@FromDate", 10, FormDate.Date.ToString("yyyy-MM-dd"));
            proc.AddVarcharPara("@ToDate", 10, toDate.Date.ToString("yyyy-MM-dd"));
            proc.AddVarcharPara("@BranchList", -1, hdBranchList.Value);
            proc.AddBooleanPara("@showAllCustomer", chkShowAll.Checked);
            proc.AddVarcharPara("@Customer", 20, hdnCustomerId.Value);
            proc.AddVarcharPara("@UserId", 20, Convert.ToString(Session["userid"]));
            proc.AddBooleanPara("@showZeroBal", chkzeroBal.Checked);
            proc.RunActionQuery();

            GridFollowup.DataBind();
            
        }



        [WebMethod]
        public static object GetAllDetailsByBranch(string BranchId )
        {
            ProcedureExecute proc = new ProcedureExecute("prc_Followup");
            proc.AddVarcharPara("@action", 100, "populateChild"); 
            proc.AddPara("@userbranchHierarchy", Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]));
            proc.AddVarcharPara("@Parentbranch", 100, BranchId);
            DataTable dt = proc.GetTable();
            List<ERP.OMS.Management.Activities.PosSalesInvoice.KeyValueClass> BranchChild = new List<ERP.OMS.Management.Activities.PosSalesInvoice.KeyValueClass>();
            BranchChild = (from DataRow dr in dt.Rows
                           select new ERP.OMS.Management.Activities.PosSalesInvoice.KeyValueClass()
                         {
                             Id = dr["branch_id"].ToString(),
                             Name = dr["branch_description"].ToString()
                         }).ToList();

            return BranchChild;
        }

        protected void drdExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 Filter = int.Parse(Convert.ToString(drdExport.SelectedItem.Value));
            if (Filter != 0)
            {   
                    bindexport(Filter);
                 
            }
        }


        public void bindexport(int Filter)
        {
             

                exporter.PageHeader.Left = "Payment Follow-up";
                exporter.PageFooter.Center = "[Page # of Pages #]";
                exporter.PageFooter.Right = "[Date Printed]";
                exporter.Landscape = true;
            
            switch (Filter)
            {
                case 1:
                    exporter.WritePdfToResponse();
                    break;
                case 2:
                    exporter.WriteXlsxToResponse(new XlsxExportOptionsEx() { ExportType = ExportType.WYSIWYG });
                    break;
                case 3:
                    exporter.WriteRtfToResponse();
                    break;
                case 4:
                    exporter.WriteCsvToResponse();
                    break;
            }
        }

        [WebMethod]
        public static object GetDocumentDetailsForCustomer(string dateStr,string CustId)
        {
            ProcedureExecute proc = new ProcedureExecute("prc_Followup");
            proc.AddVarcharPara("@action", 100, "PopulateDocument");
            proc.AddPara("@FromDate", dateStr);
            proc.AddVarcharPara("@Customer", 20, CustId);
            DataSet ds = proc.GetDataSet();
            DataTable dt = ds.Tables[0];
            ListOdFocument listOdFocument = new ListOdFocument();


            List<DocumentDetails> DocList = new List<DocumentDetails>();
            DocList = (from DataRow dr in dt.Rows
                           select new DocumentDetails()
                           {
                               docId = Convert.ToString(dr["docId"]),
                               DocType = Convert.ToString(dr["DocType"]),
                               Branch = dr["Branch"].ToString(),
                               DocNo = dr["DocNo"].ToString(),
                               DocDate =Convert.ToString(dr["DocDate"]),
                               TotalAmount = Convert.ToString(dr["TotalAmount"]),
                               UnPaidAmount = Convert.ToString(dr["UnPaidAmount"]),
                               followedByname = Convert.ToString(dr["followedByname"]),
                               FollowedOn = Convert.ToString(dr["FollowedOn"]),
                               adjAmount = Convert.ToString(dr["adjAmount"]) 

                           }).ToList();


            dt = ds.Tables[1];
            List<DocumentProductDetails> documentProductDetails = new List<DocumentProductDetails>();
            documentProductDetails = (from DataRow dr in dt.Rows
                                      select new DocumentProductDetails()
                           {
                               docId = Convert.ToString(dr["docId"]),
                               DocType = Convert.ToString(dr["DocType"]),
                               Prod = dr["Prod"].ToString(),
                               qty = dr["qty"].ToString(),
                               Price = Convert.ToString(dr["Price"]),
                               tax = Convert.ToString(dr["tax"]),
                               totAmt = Convert.ToString(dr["totAmt"]) 

                           }).ToList();


            listOdFocument.documentDetails = DocList;
            listOdFocument.documentProductDetails = documentProductDetails;

            return listOdFocument;
        }

        public class ListOdFocument {
            public List<DocumentDetails> documentDetails { get; set; }
            public List<DocumentProductDetails> documentProductDetails { get; set; }
        }


        public class DocumentDetails {
            public string docId { get; set; }
            public string DocType { get; set; }
            public string Branch { get; set; }
            public string DocNo { get; set; }
            public string DocDate { get; set; }
            public string TotalAmount { get; set; }
            public string UnPaidAmount { get; set; }
            public string adjAmount { get; set; }
            public string followedByname { get; set; }
            public string FollowedOn { get; set; }
        }

        public class DocumentProductDetails {
            public string docId { get; set; }
            public string DocType { get; set; }
            public string Prod { get; set; }
            public string qty { get; set; }
            public string Price { get; set; }
            public string tax { get; set; }
            public string totAmt { get; set; }
        }

    }
}