using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Configuration;
using BusinessLogicLayer;
namespace ERP.OMS.Reports
{
    public partial class Reports_GapTradingAnalysis : System.Web.UI.Page, System.Web.UI.ICallbackEventHandler
    {
        BusinessLogicLayer.Converter oconverter = new BusinessLogicLayer.Converter();
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        DataSet ds = new DataSet();
        string data;
        ExcelFile objExcel = new ExcelFile();
        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //'http://localhost:2957/InfluxCRM/management/testProjectMainPage_employee.aspx'
                string sPath = HttpContext.Current.Request.Url.ToString();
                oDBEngine.Call_CheckPageaccessebility(sPath);
            }
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            if (HttpContext.Current.Session["userid"] == null)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
            if (!IsPostBack)
            {

                Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script language='javascript'>Page_Load();</script>");
                Date();
                SegmentnameFetch();
            }

            //_____For performing operation without refreshing page___//
            String cbReference = Page.ClientScript.GetCallbackEventReference(this, "arg", "ReceiveServerData", "context");
            String callbackScript = "function CallServer(arg, context){ " + cbReference + ";}";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "CallServer", callbackScript, true);
            //___________-end here___//

        }
        void Date()
        {
            DtFrom.EditFormatString = oconverter.GetDateFormat("Date");
            DtTo.EditFormatString = oconverter.GetDateFormat("Date");
            DtFor.EditFormatString = oconverter.GetDateFormat("Date");

            DtFrom.Value = Convert.ToDateTime(oDBEngine.GetDate().ToShortDateString());
            DtTo.Value = Convert.ToDateTime(oDBEngine.GetDate().ToShortDateString());
            DtFor.Value = Convert.ToDateTime(oDBEngine.GetDate().ToShortDateString());
        }
        void SegmentnameFetch()
        {
            if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "1")
                HiddenField_SegmentName.Value = "NSE - CM";
            if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "4")
                HiddenField_SegmentName.Value = "BSE - CM";
            if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "15")
                HiddenField_SegmentName.Value = "CSE - CM";
            if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "19")
                HiddenField_SegmentName.Value = "MCXSX - CM";
            if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "2")
                HiddenField_SegmentName.Value = "NSE - FO";
            if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "5")
                HiddenField_SegmentName.Value = "BSE - FO";
            if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "20")
                HiddenField_SegmentName.Value = "MCXSX - FO";
            if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "3")
                HiddenField_SegmentName.Value = "NSE - CDX";
            if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "6")
                HiddenField_SegmentName.Value = "BSE - CDX";
            if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "7")
                HiddenField_SegmentName.Value = "MCX - COMM";
            if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "8")
                HiddenField_SegmentName.Value = "MCXSX - CDX";
            if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "9")
                HiddenField_SegmentName.Value = "NCDEX - COMM";
            if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "10")
                HiddenField_SegmentName.Value = "DGCX - COMM";
            if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "11")
                HiddenField_SegmentName.Value = "NMCE - COMM";
            if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "12")
                HiddenField_SegmentName.Value = "ICEX - COMM";
            if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "13")
                HiddenField_SegmentName.Value = "USE - CDX";
            if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "14")
                HiddenField_SegmentName.Value = "NSEL - SPOT";
            if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "18")
                HiddenField_SegmentName.Value = "INMX - COMM";
            if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "19")
                HiddenField_SegmentName.Value = "MCXSX - CM";
            if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "20")
                HiddenField_SegmentName.Value = "MCXSX - FO";
            if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "21")
                HiddenField_SegmentName.Value = "BFX - COMM";
            if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "22")
                HiddenField_SegmentName.Value = "INSX - COMM";
            if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "23")
                HiddenField_SegmentName.Value = "INFX - COMM";
        }
        string ICallbackEventHandler.GetCallbackResult()
        {
            return data;
        }
        void ICallbackEventHandler.RaiseCallbackEvent(string eventArgument)
        {
            string id = eventArgument.ToString();
            string[] idlist = id.Split('~');
            string[] cl = idlist[1].Split(',');
            string str = "";
            string str1 = "";
            for (int i = 0; i < cl.Length; i++)
            {
                if (idlist[0].ToString().Trim() == "Client")
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

                else
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

            }
            data = idlist[0] + "~" + str;


        }

        public void BindGroup()
        {
            DataTable DtGroup = oDBEngine.GetDataTable("tbl_master_groupmaster", "distinct gpm_Type", null);
            if (DtGroup.Rows.Count > 0)
            {
                DdlGrpType.DataSource = DtGroup;
                DdlGrpType.DataTextField = "gpm_Type";
                DdlGrpType.DataValueField = "gpm_Type";
                DdlGrpType.DataBind();
                DtGroup.Dispose();

            }

        }
        protected void BtnGroup_Click(object sender, EventArgs e)
        {
            if (DdlGroupBy.SelectedItem.Value.ToString().Trim() == "Group")
            {
                BindGroup();
            }
        }
        protected void BtnExport_Click(object sender, EventArgs e)
        {
            SPCall();
        }
        void SPCall()
        {
            string[] InputName = new string[10];
            string[] InputType = new string[10];
            string[] InputValue = new string[10];



            /////////////////Parameter Name
            InputName[0] = "Companyid";
            InputName[1] = "Segment";
            InputName[2] = "ForDate";
            InputName[3] = "Gap";
            InputName[4] = "GrpType";
            InputName[5] = "GrpId";
            InputName[6] = "BranchHierchy";
            InputName[7] = "Type";
            InputName[8] = "MasterSegment";
            InputName[9] = "SegmnetName";

            /////////////////Parameter Data Type
            InputType[0] = "V";
            InputType[1] = "V";
            InputType[2] = "V";
            InputType[3] = "DE";
            InputType[4] = "V";
            InputType[5] = "V";
            InputType[6] = "V";
            InputType[7] = "V";
            InputType[8] = "V";
            InputType[9] = "V";


            /////////////////Parameter Value
            InputValue[0] = HttpContext.Current.Session["LastCompany"].ToString().Trim();
            InputValue[1] = HttpContext.Current.Session["usersegid"].ToString().Trim();
            if (RdbGap1st.Checked)
                InputValue[2] = DtFor.Value.ToString().Trim();
            else
                InputValue[2] = "'" + DtFrom.Value.ToString().Trim() + "' and '" + DtTo.Value.ToString().Trim() + "'";

            InputValue[3] = txtNoOfGap.Value.ToString().Trim();

            if (DdlGroupBy.SelectedItem.Value.ToString().Trim() == "Group")
                InputValue[4] = DdlGrpType.SelectedItem.Value.ToString().Trim();
            else
                InputValue[4] = DdlGroupBy.SelectedItem.Value.ToString().Trim();

            if (rdAll.Checked)
                InputValue[5] = "ALL";
            else
                InputValue[5] = HiddenField_ALL.Value.ToString().Trim();
            InputValue[6] = HttpContext.Current.Session["userbranchHierarchy"].ToString().Trim();

            if (RdbGap1st.Checked)
                InputValue[7] = "1";
            else
                InputValue[7] = "2";

            InputValue[8] = HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim();
            InputValue[9] = HiddenField_SegmentName.Value.ToString().Trim();


            ds = SQLProcedures.SelectProcedureArrDS("[Report_GapTradingAnalysis]", InputName, InputType, InputValue);

            if (ds.Tables[0].Rows.Count > 0)
                Export(ds);
            else
                ScriptManager.RegisterStartupScript(this, this.GetType(), "FnAlert", "FnAlert('No Record Found !!');", true);
        }
        void Export(DataSet ds)
        {
            DataTable dtExport = ds.Tables[0].Copy();


            string str = "Gap Trading Analysis For ";
            if (RdbGap1st.Checked)
                str = str.ToString().Trim() + " List of clients traded on  " + oconverter.ArrangeDate2(DtFor.Value.ToString()) + " after a gap of more than " + txtNoOfGap.Value.ToString().Trim() + " days";
            else
            {
                str = str.ToString().Trim() + " Client wise average gap between " + oconverter.ArrangeDate2(DtFrom.Value.ToString()) + " - " + oconverter.ArrangeDate2(DtTo.Value.ToString()) + " and Total Working Days " + ds.Tables[0].Rows[0]["TotalTrade"].ToString().Trim();
                dtExport.Columns.Remove("TotalTrade");
            }



            DataTable dtReportHeader = new DataTable();
            DataTable dtReportFooter = new DataTable();

            DataTable CompanyName = oDBEngine.GetDataTable("tbl_master_company", "cmp_Name", " cmp_internalId='" + Session["LastCompany"].ToString() + "'");
            dtReportHeader.Columns.Add(new DataColumn("Header", typeof(String))); //0
            DataRow HeaderRow = dtReportHeader.NewRow();
            HeaderRow[0] = CompanyName.Rows[0][0].ToString();
            dtReportHeader.Rows.Add(HeaderRow);
            DataRow DrRowR1 = dtReportHeader.NewRow();
            DrRowR1[0] = str.ToString().Trim();
            dtReportHeader.Rows.Add(DrRowR1);


            DataRow HeaderRow1 = dtReportHeader.NewRow();
            dtReportHeader.Rows.Add(HeaderRow1);
            DataRow HeaderRow2 = dtReportHeader.NewRow();
            dtReportHeader.Rows.Add(HeaderRow2);

            dtReportFooter.Columns.Add(new DataColumn("Footer", typeof(String))); //0
            DataRow FooterRow1 = dtReportFooter.NewRow();
            dtReportFooter.Rows.Add(FooterRow1);
            DataRow FooterRow2 = dtReportFooter.NewRow();
            dtReportFooter.Rows.Add(FooterRow2);
            DataRow FooterRow = dtReportFooter.NewRow();
            FooterRow[0] = "* * *  End Of Report * * *   ";
            dtReportFooter.Rows.Add(FooterRow);

            objExcel.ExportToExcelforExcel(dtExport, "Gap Trading Analysis", "Diff (If Any) :", dtReportHeader, dtReportFooter);
        }
    }
}