using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrystalDecisions.CrystalReports.Engine;
using BusinessLogicLayer;

namespace ERP.OMS.Reports
{
    public partial class Reports_frmReport_RegisterOfSecurity : System.Web.UI.Page, System.Web.UI.ICallbackEventHandler
    {
        StatutoryReports sr = new StatutoryReports();
        BusinessLogicLayer.Converter oconverter = new BusinessLogicLayer.Converter();
        BusinessLogicLayer.GenericMethod oGenericMethod = new BusinessLogicLayer.GenericMethod();
        BusinessLogicLayer.DBEngine oDbEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        // DBEngine oDBEngine = new DBEngine(string.Empty);
        DataSet ds = new DataSet();
        DataTable dtgroupcontactid = new DataTable();
        DataTable Distinctclient = new DataTable();
        DataTable dtsp = new DataTable();
        string data;
        string SingleDouble = null;
        string CombinedGroupByQuery = null;
        ReportDocument ReportDocument = new ReportDocument();
        string[] connPath = ConfigurationManager.AppSettings["DBConnectionDefault"].Split(';');
        protected void Page_Load(object sender, EventArgs e)
        {

            if (HttpContext.Current.Session["userid"] == null)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
            if (!IsPostBack)
            {

                Page.ClientScript.RegisterStartupScript(GetType(), "JScript12", "<script>Page_Load();</script>");
                date();
                string segname = Session["Segmentname"].ToString();
                litSegment.InnerText = segname;
                HiddenField_Segment.Value = Session["UsersegId"].ToString();

            }
            //_____For performing operation without refreshing page___//
            String cbReference = Page.ClientScript.GetCallbackEventReference(this, "arg", "ReceiveServerData", "context");
            String callbackScript = "function CallServer(arg, context){ " + cbReference + ";}";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "CallServer", callbackScript, true);
        }
        void date()
        {
            dtFrom.EditFormatString = oconverter.GetDateFormat("Date");
            dtTo.EditFormatString = oconverter.GetDateFormat("Date");

            DateTime first = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).AddMonths(-1);
            DateTime last = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day);
            dtFrom.Value = Convert.ToDateTime(first);
            dtTo.Value = Convert.ToDateTime(last);
        }

        #region CallAjax
        void CallUserList(string WhichCall)
        {

            //oGenericMethod = new GenericMethod();
            if (WhichCall == "CallAjax-Segment")
            {
                CombinedGroupByQuery = oGenericMethod.GetExchangesDetail("S", Session["LastCompany"].ToString());
            }
        }
        #endregion

        void ICallbackEventHandler.RaiseCallbackEvent(string eventArgument)
        {

            string id1 = eventArgument.ToString().Split('~')[0];
            if (id1 == "CallAjax-Segment")
            {

                CallUserList(id1);
                CombinedGroupByQuery = CombinedGroupByQuery.Replace("\\'", "'");
                data = "AjaxQuery~" + CombinedGroupByQuery;
            }
            else
            {
                //string[] idlist = id.Split('^');
                //string recieveServerIDs = "";
                //for (int i = 0; i < idlist.Length; i++)
                //{
                //    string[] strVal = idlist[i].Split('!');
                //    string[] ids = strVal[0].Split('~');
                //    string whichCall = ids[ids.Length - 1];
                //    if (whichCall == "CLIENT")
                //    {
                //        if (recieveServerIDs == "")
                //            //recieveServerIDs = idlist[i];
                //            recieveServerIDs = "'" + ids[4] + "'";
                //        else
                //            //recieveServerIDs += "^" + idlist[i];
                //            recieveServerIDs += ",'" + ids[4] + "'";
                //        data = "Client@" + recieveServerIDs.ToString();
                //    }
                //}






                string id = eventArgument.ToString();
                string[] idlist = id.Split('~');
                string[] cl = idlist[1].Split(',');
                string str = "";
                string str1 = "";
                for (int i = 0; i < cl.Length; i++)
                {
                    if (idlist[0] == "Segment")
                    {
                        string[] val = cl[i].Split(';');
                        if (str == "")
                        {
                            str = val[0];
                            str1 = val[0] + ";" + val[1];
                        }
                        else
                        {
                            str += "," + val[0];
                            str1 += "," + val[0] + ";" + val[1];
                        }
                    }
                    else
                    {

                        string[] val = cl[i].Split(';');
                        string[] AcVal = val[0].Split('-');
                        if (str == "")
                        {

                            str = "'" + AcVal[0] + "'";
                            str1 = "'" + AcVal[0] + "'" + ";" + val[1];
                        }
                        else
                        {

                            str += ",'" + AcVal[0] + "'";
                            str1 += "," + "'" + AcVal[0] + "'" + ";" + val[1];
                        }
                    }

                }

                if (idlist[0] == "Clients")
                {

                    data = "Clients~" + str;
                }

                else if (idlist[0] == "Segment")
                {
                    //SegmentT = str;
                    ////  data = "Segment~" + str;
                    data = "Segment~" + str1;
                }


                else if (idlist[0] == "Group")
                {

                    data = "Group~" + str;
                }
                else if (idlist[0] == "Branch")
                {

                    data = "Branch~" + str;
                }
            }

        }
        string ICallbackEventHandler.GetCallbackResult()
        {
            return data;
        }

        protected void btnhide_Click(object sender, EventArgs e)
        {
            if (ddlGroup.SelectedItem.Value.ToString() == "1")
            {
                if (HiddenField_Group.Value.ToString().Trim() == "")
                {
                    BindGroup();
                }
            }
        }

        public void BindGroup()
        {

            ddlgrouptype.Items.Clear();
            DataTable DtGroup = oDbEngine.GetDataTable("tbl_master_groupmaster", "distinct gpm_Type", null);
            if (DtGroup.Rows.Count > 0)
            {
                ddlgrouptype.DataSource = DtGroup;
                ddlgrouptype.DataTextField = "gpm_Type";
                ddlgrouptype.DataValueField = "gpm_Type";
                ddlgrouptype.DataBind();
                ddlgrouptype.Items.Insert(0, new ListItem("Select GroupType", "0"));
                DtGroup.Dispose();

            }
            else
            {
                ddlgrouptype.Items.Insert(0, new ListItem("Select GroupType", "0"));
            }

        }

        void procedure()
        {

            string GRPTYPE = "";
            string Groupby = "";
            string SEGMENT = "";
            if (ddlGroup.SelectedItem.Value.ToString() == "0")
            {
                GRPTYPE = "BRANCH";
                if (rdbranchAll.Checked)
                {
                    Groupby = "ALL";
                }
                else
                {
                    Groupby = HiddenField_Branch.Value;
                }


            }
            else
            {
                GRPTYPE = ddlgrouptype.SelectedItem.Value.ToString();
                if (rdddlgrouptypeAll.Checked)
                {
                    Groupby = "ALL";
                }
                else
                {
                    Groupby = HiddenField_Group.Value;
                }

            }
            if (ddlGeneration.SelectedItem.Value == "1")
            {
                if (RblClient.Value == "S")

                    SEGMENT = HiddenField_Segment.Value;

                else
                    SEGMENT = "ALL";
            }
            else
                SEGMENT = Session["UsersegId"].ToString();

            ds = sr.CLIENTSECURITY(Session["LastFinYear"].ToString(), Convert.ToDateTime(dtFrom.Value).ToString("yyyy-MM-dd"), Convert.ToDateTime(dtTo.Value).ToString("yyyy-MM-dd"),
                Session["LastCompany"].ToString(), rdbClientALL.Checked ? "ALL" : HiddenField_Client.Value, Session["userbranchHierarchy"].ToString(),
                GRPTYPE, Groupby, "Security", txtHeader_hidden.Value, txtFooter_hidden.Value, ddlType.SelectedItem.Value.ToString(), ddlGeneration.SelectedItem.Text, SEGMENT);

            ViewState["dataset"] = ds;

            if (ds.Tables[0].Rows.Count == 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "NORECORD3", "NORECORD(1);", true);
            }
            else
            {
                if (ddlGeneration.SelectedItem.Value == "1")
                {
                    ExcelExport();
                }
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Display", "Display();", true);
            }
        }
        void ExcelExport()
        {
            DataSet ds = (DataSet)ViewState["dataset"];
            ExcelFile objExcel = new ExcelFile();
            DataTable dtExport = new DataTable();
            string searchCriteria = null;
            Converter oconverter = new Converter();

            searchCriteria = "From " + oconverter.ArrangeDate2(dtFrom.Value.ToString()) + "To " + oconverter.ArrangeDate2(dtTo.Value.ToString()) + " Register Of Security   ";

            dtExport = ds.Tables[0].Copy();
            GenericExcelExport oGenericExcelExport = new GenericExcelExport();
            string strDownloadFileName = "";
            string exlDateTime = oDbEngine.GetDate(113).ToString();
            string exlTime = exlDateTime.Replace(":", "");
            exlTime = exlTime.Replace(" ", "");
            string FileName = "ROS_" + exlTime;
            strDownloadFileName = "~/Documents/";
            //oGenericMethod = new GenericMethod();
            DataTable dtcompany = oGenericMethod.GetDataTable("select ltrim(rtrim(cmp_Name)) as company from tbl_master_company where cmp_internalid ='" + Session["LastCompany"].ToString() + "'");
            string[] strHead = new string[3];
            strHead[0] = exlDateTime;
            strHead[1] = searchCriteria;
            strHead[2] = "Register Of Security Of " + dtcompany.Rows[0]["company"];
            string ExcelVersion = "2007";                                                                 //Lots
            string[] ColumnType = { "V", "V", "V", "V", "V", "V", "V", "V", "V", "V", "I", "I", "I" };
            string[] ColumnSize = { "120", "120", "50", "150", "150", "150", "150", "150", "150", "150", "10", "10", "10" };
            string[] ColumnWidthSize = { "30", "25", "15", "20", "20", "20", "20", "20", "20", "20", "10", "10", "10" };
            oGenericExcelExport.ExportToExcel(ColumnType, ColumnSize, ColumnWidthSize, dtExport, Server.MapPath(strDownloadFileName), ExcelVersion, FileName, strHead, null);

        }

        protected void btnGenerate_Click(object sender, EventArgs e)
        {
            procedure();
            if (chkBothPrint.Checked == true)
                SingleDouble = "D";
            else
                SingleDouble = "S";
            DataSet ds = (DataSet)ViewState["dataset"];
            if (ds.Tables[0].Rows.Count == 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "NORECORD3", "NORECORD(1);", true);
            }
            else
            {

                ViewState["billprintdate"] = oconverter.ArrangeDate2(dtFrom.Value.ToString()) + " - " + oconverter.ArrangeDate2(dtTo.Value.ToString());
                string DateParameter = ViewState["billprintdate"].ToString();
                //ds.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\RegisterOFSecurity.xsd");
                string path = HttpContext.Current.Server.MapPath("..\\Reports\\RegisterOFSecurity.rpt");
                ReportDocument.Load(path);
                ReportDocument.SetDatabaseLogon(connPath[2].Substring(connPath[2].IndexOf("=")).Trim(), connPath[3].Substring(connPath[3].IndexOf("=")).Trim(), connPath[0].Substring(connPath[0].IndexOf("=")).Trim(), connPath[1].Substring(connPath[1].IndexOf("=")).Trim(), false);
                ReportDocument.SetDataSource(ds.Tables[0]);
                ReportDocument.Subreports["Header"].SetDataSource(ds.Tables[1]);
                ReportDocument.SetParameterValue("@DateFormat", (object)DateParameter);
                ReportDocument.SetParameterValue("@SingleDouble", SingleDouble.ToString());
                ReportDocument.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Landscape;
                ReportDocument.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, HttpContext.Current.Response, true, "Register Of Security");
            }
        }

    }
}