using BusinessLogicLayer;
using DataAccessLayer;
using DevExpress.Web;
using EntityLayer.CommonELS;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.XtraPrinting;
using DevExpress.Export;
using System.Drawing;
using ERP.Models;

//using Reports.Model;

namespace ERP.OMS.Management.Activities
{
    public partial class CRMSalesOpportunity : ERP.OMS.ViewState_class.VSPage
    {

        BusinessLogicLayer.Reports objReport = new BusinessLogicLayer.Reports();
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        //decimal TotalProduct = 0;
        //decimal TotalA = 0;
        //decimal TotalB = 0;
        //decimal TotalC = 0;
        //decimal TotalAPercentage=0;
        //decimal TotalBPercentage = 0;
        //decimal TotalCPercentage = 0;
        //string TotalAPercentageString = "";
        //string TotalBPercentageString = "";
        //string TotalCPercentageString = "";

        decimal TotalClosed = 0;
        decimal TotalOpen = 0;

        protected void Page_PreInit(object sender, EventArgs e) // lead add
        {
            if (!IsPostBack)
            {
                string sPath = Convert.ToString(HttpContext.Current.Request.Url);
                oDBEngine.Call_CheckPageaccessebility(sPath);
            }
            if (Request.QueryString.AllKeys.Contains("dashboard"))
            {
                this.Page.MasterPageFile = "~/OMS/MasterPage/PopUp.Master";
                //divcross.Visible = false;
            }
            else
            {
                this.Page.MasterPageFile = "~/OMS/MasterPage/ERP.Master";
                //divcross.Visible = true;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            rights = new UserRightsForPage();
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/ActivityManagement/CRMSalesOpportunity.aspx");
            if (!IsPostBack)
            {
                string GridHeader = "";
                BusinessLogicLayer.Reports GridHeaderDet = new BusinessLogicLayer.Reports();

                Session["IsCRMSalesOpportunity"] = null;
                GetClosedReason();


                string strFinYear = Convert.ToString(Session["LastFinYear"]);
                DataTable dt = oDBEngine.GetDataTable("Select FinYear_Code,FinYear_StartDate,FinYear_EndDate From Master_FinYear Where FinYear_Code='" + strFinYear + "'");
                if (dt != null && dt.Rows.Count > 0)
                {
                    string strStartDate = Convert.ToString(dt.Rows[0]["FinYear_StartDate"]);
                    DateTime StartDate = Convert.ToDateTime(strStartDate);

                    ASPxFromDate.Value = StartDate;
                    ASPxToDate.Value = DateTime.Now;
                }
                else
                {
                    ASPxFromDate.Value = DateTime.Now;
                    ASPxToDate.Value = DateTime.Now;
                }
                Date_finyearwise(Convert.ToString(Session["LastFinYear"]));
                

            }
        }
        public void Date_finyearwise(string Finyear)
        {
            CommonBL cbl = new CommonBL();
            DataTable fdtbl = new DataTable();
            DateTime MinDate, MaxDate;

            fdtbl = cbl.GetDateFinancila(Finyear);
            if (fdtbl.Rows.Count > 0)
            {

                ASPxFromDate.MaxDate = Convert.ToDateTime((fdtbl.Rows[0]["FinYear_EndDate"]));
                ASPxFromDate.MinDate = Convert.ToDateTime((fdtbl.Rows[0]["FinYear_StartDate"]));

                ASPxToDate.MaxDate = Convert.ToDateTime((fdtbl.Rows[0]["FinYear_EndDate"]));
                ASPxToDate.MinDate = Convert.ToDateTime((fdtbl.Rows[0]["FinYear_StartDate"]));

                DateTime MaximumDate = Convert.ToDateTime((fdtbl.Rows[0]["FinYear_EndDate"]));
                DateTime MinimumDate = Convert.ToDateTime((fdtbl.Rows[0]["FinYear_StartDate"]));

                DateTime TodayDate = Convert.ToDateTime(DateTime.Now);
                DateTime FinYearEndDate = MaximumDate;

                if (TodayDate > FinYearEndDate)
                {
                    ASPxToDate.Date = FinYearEndDate;
                    ASPxFromDate.Date = MinimumDate;
                }
                else
                {
                    ASPxToDate.Date = TodayDate;
                    ASPxFromDate.Date = MinimumDate;
                }

            }

        }
        #region Grid Details

        protected void CallbackPanel_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
        {
            string IsCRMSalesOpportunity = Convert.ToString(hfIsCRMSalesOpportunity.Value);
            Session["IsCRMSalesOpportunity"] = IsCRMSalesOpportunity;

            //Task PopulateStockTrialDataTask = new Task(() => GetCRMSalesOpportunity());
            //PopulateStockTrialDataTask.RunSynchronously();

            string strFromDate = Convert.ToDateTime(ASPxFromDate.Value).ToString("yyyy-MM-dd");
            string strToDate = Convert.ToDateTime(ASPxToDate.Value).ToString("yyyy-MM-dd");
            int UserId = Convert.ToInt32(HttpContext.Current.Session["userid"]);
            string cnt_internalId = Convert.ToString(HttpContext.Current.Session["owninternalid"]);

            DataSet dsGetData = new DataSet();
            SqlConnection con1 = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            SqlCommand cmd1 = new SqlCommand("PRC_CRMCREATEOPPORTUNITY", con1);
            cmd1.CommandType = CommandType.StoredProcedure;

            cmd1.Parameters.AddWithValue("@Action", "ShowOpportunities");
            cmd1.Parameters.AddWithValue("@cnt_internalId", cnt_internalId);
            cmd1.Parameters.AddWithValue("@FROMDATE", strFromDate);
            cmd1.Parameters.AddWithValue("@TODATE", strToDate);
            cmd1.Parameters.AddWithValue("@USERID", UserId);
            if (hdnOpenClosedClicked.Value.ToUpper() == "OPEN")
            {
                cmd1.Parameters.AddWithValue("@STATUS", "0");
            }
            else if (hdnOpenClosedClicked.Value.ToUpper() == "CLOSED")
            {
                cmd1.Parameters.AddWithValue("@STATUS", "1");
            }

            cmd1.CommandTimeout = 0;
            SqlDataAdapter Adap1 = new SqlDataAdapter();
            Adap1.SelectCommand = cmd1;
            Adap1.Fill(dsGetData);

            cmd1.Dispose();
            con1.Dispose();

            
           
        }
        protected void ShowGrid_SummaryDisplayText(object sender, ASPxGridViewSummaryDisplayTextEventArgs e)
        {
            e.Text = string.Format("{0}", e.Value);
        }

        #endregion

        #region Export Details

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
            string filename = "CRMSalesOpportunity_Report";
            exporter.FileName = filename;
            string FileHeader = "";

            BusinessLogicLayer.Reports RptHeader = new BusinessLogicLayer.Reports();

            FileHeader = RptHeader.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), true, true, true, true, true, true) + Environment.NewLine + "ABC Analysis" + Environment.NewLine + "For the period " + Convert.ToDateTime(ASPxFromDate.Date).ToString("dd-MM-yyyy") + " To " + Convert.ToDateTime(ASPxToDate.Date).ToString("dd-MM-yyyy");
            exporter.PageHeader.Left = FileHeader;
            exporter.PageHeader.Font.Size = 10;
            exporter.PageHeader.Font.Name = "Tahoma";

            //exporter.PageFooter.Center = "[Page # of Pages #]";
            //exporter.PageFooter.Right = "[Date Printed]";
            exporter.GridViewID = "ShowGrid";
            exporter.RenderBrick += exporter_RenderBrick;
            switch (Filter)
            {
                case 1:
                    exporter.WriteXlsxToResponse(new XlsxExportOptionsEx() { ExportType = ExportType.WYSIWYG });
                    break;
                case 2:
                    exporter.WritePdfToResponse();
                    break;
                case 3:
                    exporter.WriteCsvToResponse();
                    break;
                case 4:
                    exporter.WriteRtfToResponse();
                    break;

                default:
                    return;
            }
        }

        protected void exporter_RenderBrick(object sender, ASPxGridViewExportRenderingEventArgs e)
        {
            e.BrickStyle.BackColor = Color.White;
            e.BrickStyle.ForeColor = Color.Black;
        }
        #endregion

        //#region Database Details

        //public void GetCRMSalesOpportunity()
        //{
        //    string strFromDate = Convert.ToDateTime(ASPxFromDate.Value).ToString("yyyy-MM-dd");
        //    string strToDate = Convert.ToDateTime(ASPxToDate.Value).ToString("yyyy-MM-dd");

        //    try
        //    {
        //        DataSet ds = new DataSet();
        //        SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));

        //        SqlCommand cmd = new SqlCommand("PRC_CRMSalesOpportunity_REPORT", con);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.AddWithValue("@COMPANYID", Convert.ToString(Session["LastCompany"]));
        //        cmd.Parameters.AddWithValue("@FINYEAR", Convert.ToString(Session["LastFinYear"]));
        //        cmd.Parameters.AddWithValue("@FROMDATE", strFromDate);
        //        cmd.Parameters.AddWithValue("@TODATE", strToDate);
        //        cmd.Parameters.AddWithValue("@CLASS", hdnClassId.Value);
        //        cmd.Parameters.AddWithValue("@CATEGORY", hdnBranndId.Value);
        //        cmd.Parameters.AddWithValue("@INDICATORA", Convert.ToDecimal(txtIndicatorA.Text));
        //        cmd.Parameters.AddWithValue("@INDICATORB", Convert.ToDecimal(txtIndicatorB.Text));
        //        cmd.Parameters.AddWithValue("@INDICATORC", Convert.ToDecimal(txtIndicatorC.Text));
        //        cmd.Parameters.AddWithValue("@CALCULATEON", ddlOnCriteria.SelectedValue);
        //        cmd.Parameters.AddWithValue("@VALTECH", ddlValTech.SelectedValue);
        //        cmd.Parameters.AddWithValue("@USERID", Convert.ToInt32(Session["userid"]));

        //        cmd.CommandTimeout = 0;
        //        SqlDataAdapter da = new SqlDataAdapter();
        //        da.SelectCommand = cmd;
        //        da.Fill(ds);

        //        cmd.Dispose();
        //        con.Dispose();
        //    }
        //    catch
        //    {
        //    }
        //}

        //#endregion

        #region LinQ
        protected void GenerateEntityServerModeDataSource_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "ID";
            SqlConnection connectionString = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            string Userid = Convert.ToString(HttpContext.Current.Session["userid"]);
            ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);

            string strFromDate = Convert.ToDateTime(ASPxFromDate.Value).ToString("yyyy-MM-dd");
            string strToDate = Convert.ToDateTime(ASPxToDate.Value).ToString("yyyy-MM-dd");

            if (Convert.ToString(Session["IsCRMSalesOpportunity"]) == "Y")
            {

                int UserId = Convert.ToInt32(HttpContext.Current.Session["userid"]);

                if (hdnOpenClosedClicked.Value.ToUpper() == "OPEN")
                {
                    var q = from d in dc.CRM_SalesOpportunities_Temps
                            where Convert.ToString(d.USERID) == Userid && d.closed == false
                            orderby d.ID ascending
                            select d;
                    e.QueryableSource = q;
                }
                else if (hdnOpenClosedClicked.Value.ToUpper() == "CLOSED")
                {
                    var q = from d in dc.CRM_SalesOpportunities_Temps
                            where Convert.ToString(d.USERID) == Userid && d.closed == true
                            orderby d.ID ascending
                            select d;
                    e.QueryableSource = q;
                }
                else if (hdnOpenClosedClicked.Value.ToUpper() == "FILTERED_VALUE" && hdnFilteredValue.Value.Trim() != "")
                {
                   string closeReasonID = hdnFilteredValue.Value ;

                   List<int> ReasonIDlist;
                   ReasonIDlist = new List<int>(Array.ConvertAll(closeReasonID.Split(','), int.Parse));

                   if (ReasonIDlist.Contains(999))
                   {
                       var q = from d in dc.CRM_SalesOpportunities_Temps
                               where Convert.ToString(d.USERID) == Userid && (ReasonIDlist.Contains(Convert.ToInt32(d.Close_ReasonID)) || d.Autoclose == true)
                               orderby d.ID ascending
                               select d;
                       e.QueryableSource = q;
                   }
                   else
                   {
                       var q = from d in dc.CRM_SalesOpportunities_Temps
                               where Convert.ToString(d.USERID) == Userid && ReasonIDlist.Contains(Convert.ToInt32(d.Close_ReasonID))
                               orderby d.ID ascending
                               select d;
                       e.QueryableSource = q;
                   } 
                   
                    
                }
                else
                {
                    var q = from d in dc.CRM_SalesOpportunities_Temps
                            where Convert.ToString(d.USERID) == Userid
                            orderby d.ID ascending
                            select d;
                    e.QueryableSource = q;


                    TotalClosed = (from d in dc.CRM_SalesOpportunities_Temps
                              where Convert.ToString(d.USERID) == Userid && d.closed == true
                              orderby d.ID
                              select d.ID).Count();

                    TotalOpen = (from d in dc.CRM_SalesOpportunities_Temps
                              where Convert.ToString(d.USERID) == Userid && d.closed == false
                              orderby d.ID
                              select d.ID).Count();

                    ShowGrid.JSProperties["cpLoad"] = "LOAD";
                    ShowGrid.JSProperties["cpOpenCnt"] = TotalOpen;
                    ShowGrid.JSProperties["cpClosedCnt"] = TotalClosed;
                }

                
            }
            else
            {
                var q = from d in dc.CRM_SalesOpportunities_Temps
                        where Convert.ToString(d.ID) == "0"
                        orderby d.ID ascending
                        select d;
                e.QueryableSource = q;
            }
            
            
        }
        
        #endregion       
 
       
        protected void ShowGrid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            
            string strFromDate = Convert.ToDateTime(ASPxFromDate.Value).ToString("yyyy-MM-dd");
            string strToDate = Convert.ToDateTime(ASPxToDate.Value).ToString("yyyy-MM-dd");
            int UserId = Convert.ToInt32(HttpContext.Current.Session["userid"]);

            ShowGrid.JSProperties["cpReturnMessage"] = null;

            if (e.Parameters == "AUTOCLOSE")
            {

                string strAutoCloseRem = hdnAutoCloseRem.Value;

                try
                {
                    string returnmessage = "";
                    string output = string.Empty;

                    DataSet dsInst = new DataSet();
                    SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                    SqlCommand cmd = new SqlCommand("PRC_CRMCREATEOPPORTUNITY", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Action", "SaveAutoClose");
                    cmd.Parameters.AddWithValue("@FROMDATE", strFromDate);
                    cmd.Parameters.AddWithValue("@TODATE", strToDate);
                    cmd.Parameters.AddWithValue("@USERID", UserId);
                    cmd.Parameters.AddWithValue("@AUTOCLOSEREM", strAutoCloseRem);

                    cmd.Parameters.Add("@RETURNMESSAGE", SqlDbType.Char, 500);
                    cmd.Parameters["@RETURNMESSAGE"].Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@RETURNCODE", SqlDbType.Char, 20);
                    cmd.Parameters["@RETURNCODE"].Direction = ParameterDirection.Output;

                    cmd.CommandTimeout = 0;
                    SqlDataAdapter Adap = new SqlDataAdapter();
                    Adap.SelectCommand = cmd;
                    Adap.Fill(dsInst);
                    cmd.Dispose();
                    output = (string)cmd.Parameters["@RETURNCODE"].Value;
                    returnmessage = (string)cmd.Parameters["@RETURNMESSAGE"].Value;

                    con.Dispose();
                    if (output.Trim() == "1")
                    {
                        ShowGrid.JSProperties["cpReturnMessage"] = "AutoCloseSuccess";

                        if (Convert.ToInt64(returnmessage) == 0)
                        {
                            ShowGrid.JSProperties["cpReturnMessageAutoClose"] = "No Open Opportunities";
                        }
                        else
                        {
                            ShowGrid.JSProperties["cpReturnMessageAutoClose"] = "AutoClosed " + returnmessage + " Open Opportunities";
                        }
                        
                    }
                    else
                    {
                        ShowGrid.JSProperties["cpReturnMessage"] = "AutoCloseFail";
                    }
                    
                    con.Dispose();

                }
                catch
                {
                }
            }
            if (e.Parameters == "CLOSE")
            {
                string strClosedStatus = "";
                if (hdnClosedStatus.Value == "0")
                {
                    strClosedStatus = "Order Won";
                }
                else
                {
                    strClosedStatus = "Order Lost";
                }

                string strCloseReason = hdnCloseReason.Value;
                decimal deciCloseQty = Convert.ToDecimal( hdnCloseQty.Value);
                string strCloseRemark = hdnCloseRemark.Value;

                string[] data = hdnOpportunityID.Value.Split('~');
                string strID = data[0];
                Boolean closed = Convert.ToBoolean(data[1]);

                //string strID = hdnOpportunityID.Value;

                try
                {
                    SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));

                    //int cntClosed = 0;
                    //DataTable dtCount = oDBEngine.GetDataTable("select count(0) Rec_Count from CRM_SalesOpportunities where ID='"+strID+"' and closed=1");
                    //if (dtCount.Rows.Count > 0)
                    //    cntClosed = Convert.ToInt32(dtCount.Rows[0]["Rec_Count"]);


                    if (closed == false)
                    {
                        string returnmessage = "";
                        string output = string.Empty;

                        DataSet dsInst = new DataSet();

                        SqlCommand cmd = new SqlCommand("PRC_CRMCREATEOPPORTUNITY", con);
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@Action", "SaveClosedInfo");
                        cmd.Parameters.AddWithValue("@OPPORTUNITY_ID", strID);
                        cmd.Parameters.AddWithValue("@CLOSEREASON", strCloseReason);
                        cmd.Parameters.AddWithValue("@CLOSEQTY", deciCloseQty);
                        cmd.Parameters.AddWithValue("@CLOSEREMARK", strCloseRemark);
                        cmd.Parameters.AddWithValue("@CLOSED_STATUS", strClosedStatus);

                        cmd.Parameters.Add("@RETURNMESSAGE", SqlDbType.Char, 500);
                        cmd.Parameters["@RETURNMESSAGE"].Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@RETURNCODE", SqlDbType.Char, 20);
                        cmd.Parameters["@RETURNCODE"].Direction = ParameterDirection.Output;

                        cmd.CommandTimeout = 0;
                        SqlDataAdapter Adap = new SqlDataAdapter();
                        Adap.SelectCommand = cmd;
                        Adap.Fill(dsInst);
                        cmd.Dispose();
                        output = (string)cmd.Parameters["@RETURNCODE"].Value;
                        con.Dispose();
                        if (output.Trim() == "1")
                        {
                            ShowGrid.JSProperties["cpReturnMessage"] = "ClosedSuccess";
                        }
                        else
                        {
                            ShowGrid.JSProperties["cpReturnMessage"] = "ClosedFailed";
                        }
                    }
                    else
                    {
                        ShowGrid.JSProperties["cpReturnMessage"] = "AlreadyClosed";
                    }


                    con.Dispose();

                }
                catch
                {
                }
            }
            if (e.Parameters == "REOPEN")
            {
                string[] data = hdnOpportunityID.Value.Split('~');
                string strID =  data[0];
                Boolean closed = Convert.ToBoolean(data[1]);
                Boolean Locked = Convert.ToBoolean(data[2]);
                
                string strReopenFeedback = hdnReopenFeedback.Value;
                
                try
                {
                    SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                    
                    //ERPDataClassesDataContext dc = new ERPDataClassesDataContext(con);
                    //string Userid = Convert.ToString(HttpContext.Current.Session["userid"]);

                    //int cntClosed = 0;

                    //TotalClosed = (from d in dc.CRM_SalesOpportunities_Temps
                    //               where Convert.ToString(d.USERID) == Userid && d.closed == true
                    //               orderby d.ID
                    //               select d.ID).Count();

                    //DataTable dtCount = oDBEngine.GetDataTable("select count(0) Rec_Count from CRM_SalesOpportunities where id='" + strID + "' and closed=0");
                    //if (dtCount.Rows.Count > 0)
                    //    cntClosed = Convert.ToInt32(dtCount.Rows[0]["Rec_Count"]);

                    if (closed == true && Locked == false)
                    {

                        string returnmessage = "";
                        string output = string.Empty;

                        DataSet dsInst = new DataSet();
                       
                        SqlCommand cmd = new SqlCommand("PRC_CRMCREATEOPPORTUNITY", con);
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@Action", "ReopenInfo");
                        cmd.Parameters.AddWithValue("@OPPORTUNITY_ID", strID);
                        cmd.Parameters.AddWithValue("@REOPENFEEDBACK", strReopenFeedback);

                        cmd.Parameters.Add("@RETURNMESSAGE", SqlDbType.Char, 500);
                        cmd.Parameters["@RETURNMESSAGE"].Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@RETURNCODE", SqlDbType.Char, 20);
                        cmd.Parameters["@RETURNCODE"].Direction = ParameterDirection.Output;

                        cmd.CommandTimeout = 0;
                        SqlDataAdapter Adap = new SqlDataAdapter();
                        Adap.SelectCommand = cmd;
                        Adap.Fill(dsInst);
                        cmd.Dispose();
                        output = (string)cmd.Parameters["@RETURNCODE"].Value;
                        con.Dispose();
                        if (output.Trim() == "1")
                        {
                            ShowGrid.JSProperties["cpReturnMessage"] = "ReopenSuccess";
                        }
                        else
                        {
                            ShowGrid.JSProperties["cpReturnMessage"] = "ReopneFailed";
                        }
                    }
                    else
                    {
                        ShowGrid.JSProperties["cpReturnMessage"] = "AlreadyOpen";
                    }


                    con.Dispose();

                }
                catch
                {
                }
            }

        }

        protected void GetClosedReason()
        {
            DataSet dsInst = new DataSet();
            SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            SqlCommand cmd = new SqlCommand("PRC_CRMCREATEOPPORTUNITY", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Action", "GetClosedReason");
            // Mantis Issue 24818
            cmd.Parameters.AddWithValue("@COMPANYID", Convert.ToString(Session["LastCompany"]));
            // End of Mantis Issue 24818

            cmd.CommandTimeout = 0;
            SqlDataAdapter Adap = new SqlDataAdapter();
            Adap.SelectCommand = cmd;
            Adap.Fill(dsInst);

            DataTable dtClosedReason = dsInst.Tables[0];
            CmbCloseReason.Items.Clear();

            CmbCloseReason.DataSource = dtClosedReason;
            CmbCloseReason.DataBind();

        }

        [WebMethod]
        public static List<ModelListClosedReason> GetSaleOpportunitiesCloseReason()
        {
            int UserId = Convert.ToInt32(HttpContext.Current.Session["userid"]);

            DataSet dsInst = new DataSet();
            SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            SqlCommand cmd = new SqlCommand("PRC_CRMCREATEOPPORTUNITY", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Action", "GetClosedReasonForFilter");
            cmd.Parameters.AddWithValue("@USERID", UserId);
            cmd.CommandTimeout = 0;
            SqlDataAdapter Adap = new SqlDataAdapter();
            Adap.SelectCommand = cmd;
            Adap.Fill(dsInst);

            List<ModelListClosedReason> omodel = new List<ModelListClosedReason>();
            DataTable dt = new DataTable();
            dt = dsInst.Tables[0];
            if (dt != null && dt.Rows.Count > 0)
            {
                omodel = UtilityLayer.APIHelperMethods.ToModelList<ModelListClosedReason>(dt);
            }
            return omodel;

        }


        [WebMethod]
        public static object GetChartData(string frmDate, string toDate, string salesmaId)
        {
            List<chartClass> lEfficency = new List<chartClass>();
            ProcedureExecute proc = new ProcedureExecute("PRC_CRMCREATEOPPORTUNITY");
            proc.AddVarcharPara("@Action", 100, "ShowOpportunityChart");
            proc.AddVarcharPara("@FROMDATE", 100, frmDate);
            proc.AddVarcharPara("@TODATE", 100, toDate);
            proc.AddVarcharPara("@Salesman_Id", 100, salesmaId);
            proc.AddVarcharPara("@USERID", 1000, Convert.ToString(HttpContext.Current.Session["userid"]));

            DataTable CallData = proc.GetTable();
            //var random = new Random();
            lEfficency = (from DataRow dr in CallData.Rows
                          select new chartClass()
                          {
                              Close_Reason = Convert.ToString(dr["Close_Reason"]),
                              Count = Convert.ToString(dr["Close_Reason_Count"]),
                             
                          }).ToList();
            return lEfficency;
        }

        [WebMethod]
        public static object GetChartDataForCloseReason(string frmDate, string toDate, string salesmaId)
        {
            List<chartClass> lEfficency = new List<chartClass>();
            ProcedureExecute proc = new ProcedureExecute("PRC_CRMCREATEOPPORTUNITY");
            proc.AddVarcharPara("@Action", 100, "ShowOpportunityChartAll");
            proc.AddVarcharPara("@FROMDATE", 100, frmDate);
            proc.AddVarcharPara("@TODATE", 100, toDate);
            proc.AddVarcharPara("@Salesman_Id", 100, salesmaId);
            proc.AddVarcharPara("@USERID", 1000, Convert.ToString(HttpContext.Current.Session["userid"]));

            DataTable CallData = proc.GetTable();
            //var random = new Random();
            lEfficency = (from DataRow dr in CallData.Rows
                          select new chartClass()
                          {
                              Close_Reason = Convert.ToString(dr["Close_Reason"]),
                              Count = Convert.ToString(dr["Close_Reason_Count"]),

                          }).ToList();
            return lEfficency;
        }

        [WebMethod]
        public static object GetSalemanHierarchy()
        {
            List<salesmanList> lEfficency = new List<salesmanList>();
            ProcedureExecute proc = new ProcedureExecute("PRC_CRMCREATEOPPORTUNITY");
            proc.AddVarcharPara("@Action", 100, "GetSalesmanlistH");
            proc.AddVarcharPara("@USERID", 1000, Convert.ToString(HttpContext.Current.Session["userid"]));

            DataTable CallData = proc.GetTable();
            //var random = new Random();
            lEfficency = (from DataRow dr in CallData.Rows
                          select new salesmanList()
                          {
                              Salesman_Id = Convert.ToString(dr["Salesman_Id"]),
                              Salesman_Name = Convert.ToString(dr["Salesman_Name"]),

                          }).ToList();
            return lEfficency;
        }
    }

    public class salesmanList
    {
        public String Salesman_Id { get; set; }
        public String Salesman_Name { get; set; }
        	
    }
    public class chartClass {
     public String Close_Reason { get; set; }
     public String Count { get; set; }
    }

    public class ModelListClosedReason
    {
        public long ID { get; set; }
        public String Close_Reason { get; set; }
        public long Close_Reason_Count { get; set; }
        public bool IsChecked { get; set; }
    }

}