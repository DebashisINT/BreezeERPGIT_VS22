using BusinessLogicLayer;
using DevExpress.Web;
using EntityLayer.CommonELS;
using ERP.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;


namespace ERP.OMS.Management.Activities
{
    public partial class OldUnitReceived : ERP.OMS.ViewState_class.VSPage
    {
        OldUnitAssignReceivedBL oBL = new OldUnitAssignReceivedBL();
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
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


            if (!IsPostBack)
            {
                string userbranchHierachy = Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]);
                PopulateBranchByHierchy(userbranchHierachy);


                #region Session Remove Section Start
                Session.Remove("PendingApproval");
                Session.Remove("UserWiseERPDocCreation");
                Session.Remove("SaveModePB");  // Use this session to remove default cursor from manual oe auto schema type
                Session.Remove("schemavaluePB"); // Use this session to remove default cursor from manual oe auto schema type

                #endregion Session Remove Section End


            }
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/Management/Activities/OldUnitReceived.aspx");
            //fillGrid();
        }

        protected void EntityServerModeDataSource_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
       {
            e.KeyExpression = "Invoice_Id";

            //string connectionString = ConfigurationManager.ConnectionStrings["crmConnectionString"].ConnectionString;
            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);



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
                    var q = from d in dc.V_OldUnitReceivedLists
                            where d.Invoice_DateTimeFormat >= Convert.ToDateTime(strFromDate) &&
                                  d.Invoice_DateTimeFormat <= Convert.ToDateTime(strToDate) 
                            orderby d.Invoice_Date descending
                            select d;
                    e.QueryableSource = q;
                }
                else
                {
                    branchidlist = new List<int>(Array.ConvertAll(strBranchID.Split(','), int.Parse));
                    var q = from d in dc.V_OldUnitReceivedLists
                            where d.Invoice_DateTimeFormat >= Convert.ToDateTime(strFromDate) &&
                                  d.Invoice_DateTimeFormat <= Convert.ToDateTime(strToDate) &&
                                  branchidlist.Contains(Convert.ToInt32(d.branch_id))
                            orderby d.Invoice_DateTimeFormat descending
                            select d;
                    e.QueryableSource = q;
                }
            }
            else
            {
                var q = from d in dc.V_OldUnitReceivedLists
                        where d.branch_id  == '0'
                        orderby d.Invoice_DateTimeFormat descending
                        select d;
                e.QueryableSource = q;
            }
        }
        /* Abhisek  public void fillGrid()
        {
            DataTable DT = oBL.GetOldUnitReceivedData(Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]), "OldUnitReceived");

            if (DT.Rows.Count > 0)
            {
                gridStatus.DataSource = DT;
                gridStatus.DataBind();
            }
        }*/
        [WebMethod]
        [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
        public static string SaveReceivedBranch(tbl_trans_oldunit tbl_trans_oldunit)
        {
            try
            {
                tbl_trans_oldunit.received_by = Convert.ToInt32(HttpContext.Current.Session["userid"]);
                return OldUnitAssignReceivedBL.ReceivedBranch((object)tbl_trans_oldunit);
            }
            catch (Exception ex)
            {
                return "Error occured";
            }
        }

        protected void SelectPanel_Callback(object sender, CallbackEventArgsBase e)
        {
            string strSplitCommand = e.Parameter.Split('~')[0];
            if (strSplitCommand == "Bindalldesignes")
            {
                string[] filePaths = new string[] { };
                string DesignPath = "";
                if (ConfigurationManager.AppSettings["IsDevelopedZone"] != null)
                {
                    DesignPath = @"Reports\Reports\RepxReportDesign\OldUnitReceived\DocDesign\Normal";
                }
                else
                {
                    DesignPath = @"Reports\RepxReportDesign\OldUnitReceived\DocDesign\Normal";
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
                SelectPanel.JSProperties["cpChecked"] = "";
            }
            else
            {
                string DesignPath = @"Reports\Reports\REPXReports";
                string fullpath = Server.MapPath("~");
                fullpath = fullpath.Replace("ERP.UI\\", "");
                string filename = @"\RepxReportViewer.aspx";
                string DesignFullPath = fullpath + DesignPath + filename;
                string reportName = Convert.ToString(CmbDesignName.Value);
                string NoofCopy = "";
                if (selectOriginal.Checked == true)
                {
                    NoofCopy += 1 + ",";
                }
                if (selectDuplicate.Checked == true)
                {
                    NoofCopy += 2 + ",";
                }
                if (selectTriplicate.Checked == true)
                {
                    NoofCopy += 4 + ",";
                }
                SelectPanel.JSProperties["cpSuccess"] = NoofCopy;
                SelectPanel.JSProperties["cpChecked"] = "Checked";
                //SelectPanel.JSProperties["cpSuccess"] = "Success";
            }
        }
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
            if (gridStatus.VisibleRowCount > 0)
            {

                //gridStatus.Columns[12].Visible = false;
                //gridStatus.Columns[13].Visible = false;
                string filename = "Old Unit Receive";
                exporter.FileName = filename;
                //exporter.FileName = "PurchaseInvoice";

                exporter.PageHeader.Left = "Old Unit Receive";
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
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Notify", "alert('There is no record to export.');", true);
                return;
            }
        }

        protected void FormDate_Init(object sender, EventArgs e)
        {

            FormDate.Date = DateTime.Now;
            //FormDate.Date = new DateTime(DateTime.Now.Year, DateTime.Now.Month , DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month));
        }

        protected void toDate_Init(object sender, EventArgs e)
        {
            toDate.Date = DateTime.Now;
        }

        private void PopulateBranchByHierchy(string userbranchhierchy)
        {
            OldUnitAssignReceivedBL oldUnitAssignReceivedBL = new OldUnitAssignReceivedBL();
            DataTable branchtable = oldUnitAssignReceivedBL.getBranchListByHierchy(userbranchhierchy);
            cmbBranchfilter.DataSource = branchtable;
            cmbBranchfilter.ValueField = "branch_id";
            cmbBranchfilter.TextField = "branch_description";
            cmbBranchfilter.DataBind();
            cmbBranchfilter.SelectedIndex = 0;

            DataRow[] name = branchtable.Select("branch_id=" + Convert.ToString(Session["userbranchID"]));
            if (name.Length > 0)
            {
                //branchName.Text = Convert.ToString(name[0]["branch_description"]);
            }


        }

        /* Abhisek  public void PopulateGridByFilter(string fromdate, string toDate, string branch)
        {
            DataTable dtdata = new DataTable();
            //dtdata = oBL.GetOldUnitListGridDataByDate(Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]), Convert.ToString(HttpContext.Current.Session["LastCompany"]), fromdate, toDate, branch);

            dtdata = oBL.GetOldUnitReceivedData(Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]), "OldUnitReceived");
            //if (dtdata != null)
            //{
            Session["PBList"] = dtdata;
            gridStatus.DataSource = dtdata;
            gridStatus.DataBind();
            //}
        } */

        protected void gridStatus_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {

            string WhichCall = Convert.ToString(e.Parameters).Split('~')[0];
            if (WhichCall == "FilterGridByDate")
            // if (WhichCall == "OldUnitReceived")
            {
                string fromdate = e.Parameters.Split('~')[1];
                string toDate = e.Parameters.Split('~')[2];
                string branch = e.Parameters.Split('~')[3];
              /* Abhisek    //Session["PBFromDate"] = fromdate;
                //Session["PBtoDate"] = toDate;
                //Session["PBfilteredbranch"] = branch;
                //DataTable dtdata = new DataTable();
                //dtdata = oBL.GetOldUnitListGridDataByDate(Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]), Convert.ToString(HttpContext.Current.Session["LastCompany"]), fromdate, toDate, branch);
                //if (dtdata != null)
                //{
                //    Session["PBList"] = dtdata;
                //    gridStatus.DataSource = dtdata;
                //    gridStatus.DataBind();*/
                //}
            }
        }

        protected void Grid_OldUnit_DataBinding(object sender, EventArgs e)
        {   
            /* Abhisek 
            if (Session["PBList"] != null)
            {
                gridStatus.DataSource = (DataTable)Session["PBList"];
            }
            else
            {
                gridStatus.DataSource = null;

            } */




            //string BranchID = Convert.ToString(cmbBranchfilter.Value);
            //DateTime FromDate = Convert.ToDateTime(Convert.ToDateTime(FormDate.Value).ToString("yyyy-MM-dd"));
            //DateTime ToDate = Convert.ToDateTime(Convert.ToDateTime(toDate.Value).ToString("yyyy-MM-dd"));

            //string lastCompany = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
            //string userbranch = Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]);
            //string finyear = Convert.ToString(Session["LastFinYear"]);

            //DataTable dtdata = new DataTable();
            //dtdata = GetPOListGridDataByFilter(userbranch, lastCompany, finyear, BranchID, FromDate, ToDate);
            //if (dtdata != null && dtdata.Rows.Count > 0)
            //{
            //    Grid_PurchaseOrder.DataSource = dtdata;

            //}
            //else
            //{
            //    Grid_PurchaseOrder.DataSource = null;

            //}
        }
    }
}