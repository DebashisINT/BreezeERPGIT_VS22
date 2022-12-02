using BusinessLogicLayer;
using DevExpress.Web;
using EntityLayer.CommonELS;
using ERP.OMS.Management.Master;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.IO;
using DataAccessLayer;
using System.Configuration;
using ERP.Models;

namespace ERP.OMS.Management.Activities
{
    public partial class InstallationCoupon : ERP.OMS.ViewState_class.VSPage
    {
        InstallationCouponBL oBL = new InstallationCouponBL();
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        
        protected void Page_Load(object sender, EventArgs e)
        {
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/Management/Activities/InstallationCoupon.aspx");

            //CmbBranch.DataSource = GetBranch();
            //CmbBranch.DataSource = GetAllBranch();
            //CmbBranch.DataBind();
            //CmbBranch.Value = Convert.ToString(Session["userbranchID"]);
            //CmbBranch.Value = "all";
            //fillGrid();
        }
        private IEnumerable GetBranch()
        {
            List<branches> LevelList = new List<branches>();

            DataTable DT = oDBEngine.GetDataTable("select branch_id,branch_description from tbl_master_branch where branch_id in(" + Convert.ToString(Session["userbranchHierarchy"]) + ")  order by branch_description asc");

            for (int i = 0; i < DT.Rows.Count; i++)
            {
                branches Levels = new branches();

                Levels.branchID = Convert.ToString(DT.Rows[i]["branch_id"]);
                Levels.branchName = Convert.ToString(DT.Rows[i]["branch_description"]);
                LevelList.Add(Levels);
            }


            return LevelList;
        }
        private IEnumerable GetAllBranch()
        {
            List<branches> LevelList = new List<branches>();

            DataTable DT = oDBEngine.GetDataTable("select branch_id,branch_description from tbl_master_branch order by branch_description asc");

            branches _Objbranches = new branches();
            _Objbranches.branchID = Convert.ToString("all");
            _Objbranches.branchName = Convert.ToString("All");
            LevelList.Add(_Objbranches);

            for (int i = 0; i < DT.Rows.Count; i++)
            {
                branches Levels = new branches();

                Levels.branchID = Convert.ToString(DT.Rows[i]["branch_id"]);
                Levels.branchName = Convert.ToString(DT.Rows[i]["branch_description"]);
                LevelList.Add(Levels);
            }

            return LevelList;
        }
        protected void EntityServerModeDataSource_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "InvoiceDetails_Id";

           // string connectionString = ConfigurationManager.ConnectionStrings["crmConnectionString"].ConnectionString;


           // string connectionString = ConfigurationManager.ConnectionStrings["crmConnectionString"].ConnectionString;
            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);




            List<int> branchidlist;
            ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
            string BranchList = Convert.ToString(Session["userbranchHierarchy"]);
            branchidlist = new List<int>(Array.ConvertAll(BranchList.Split(','), int.Parse));
            var q = from d in dc.v_InstallationInvoiceDetailsLists
                    where branchidlist.Contains(Convert.ToInt32(d.Invoice_BranchId))
                    orderby d.Invoice_DateTimeFormat descending
                    select d;
            e.QueryableSource = q;
        }
        /* Abhisek
        public void fillGrid()
        {
            DataTable DT = oBL.GetInstallationInvoiceDetails("all", Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]), "GetInstallationInvoiceDetails");
            //DataTable DT = oBL.GetInstallationInvoiceDetails(CmbBranch.Value.ToString(), "GetInstallationInvoiceDetails");
            if (DT != null && DT.Rows.Count > 0)
            {
                gridInstallationCoupon.DataSource = DT;
                gridInstallationCoupon.DataBind();
            }
        }
         */
        [WebMethod]
        [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
        public static string verified(string InvoiceDetails_Id, string IsInstallVerified)
        {
            try
            {
                return InstallationCouponBL.SaveVerification(InvoiceDetails_Id, IsInstallVerified);
            }
            catch (Exception ex)
            {
                return "Error occured";
            }
        }

        /*Abhisek
        protected void CmbBranch_SelectedIndexChanged(object sender, EventArgs e)
        {
            fillGrid();
        }
         */ 

        #region  Print

        protected void SelectPanel_Callback(object sender, CallbackEventArgsBase e)
        {
            string strSplitCommand = e.Parameter.Split('~')[0];
            if (strSplitCommand == "Bindalldesignes")
            {
                string[] filePaths = new string[] { };
                string DesignPath = "";
                if (ConfigurationManager.AppSettings["IsDevelopedZone"] != null)
                {
                    DesignPath = @"Reports\Reports\RepxReportDesign\SalesInvoice\DocDesign\InstCoupon";
                }
                else
                {
                    DesignPath = @"Reports\RepxReportDesign\SalesInvoice\DocDesign\InstCoupon";
                }
                string fullpath = Server.MapPath("~");
                fullpath = fullpath.Replace("ERP.UI\\", "");
                string DesignFullPath = fullpath + DesignPath;
                filePaths = System.IO.Directory.GetFiles(DesignFullPath, "*.repx");

                foreach (string filename in filePaths)
                {
                    string reportname = Path.GetFileNameWithoutExtension(filename);
                    string name = "";
                    if (reportname.Split('~').Length > 1)
                    {
                        name = reportname.Split('~')[0];
                    }
                    else
                    {
                        name = reportname;
                    }
                    string reportValue = reportname;
                    CmbDesignName.Items.Add(name, reportValue);
                }
                CmbDesignName.SelectedIndex = 0;
            }
            else
            {
                string DesignPath = @"Reports\Reports\REPXReports";
                string fullpath = Server.MapPath("~");
                fullpath = fullpath.Replace("ERP.UI\\", "");
                string filename = @"\RepxReportViewer.aspx";
                string DesignFullPath = fullpath + DesignPath + filename;
                string reportName = Convert.ToString(CmbDesignName.Value);             
                SelectPanel.JSProperties["cpSuccess"] = "Success";
            }
        }


        #endregion
    }
}