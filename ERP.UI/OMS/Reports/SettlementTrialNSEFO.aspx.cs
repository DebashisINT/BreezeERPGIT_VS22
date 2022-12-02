using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Configuration;
using BusinessLogicLayer;

namespace ERP.OMS.Reports
{
    public partial class Reports_SettlementTrialNSEFO : System.Web.UI.Page, System.Web.UI.ICallbackEventHandler
    {
        BusinessLogicLayer.Converter oconverter = new BusinessLogicLayer.Converter();
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        DataSet ds = new DataSet();

        ExcelFile objExcel = new ExcelFile();
        DataTable dtExport = new DataTable();
        string data;
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
                //Bind GroupType
                ddlGroupType.DataSource = oDBEngine.GetDataTable("tbl_master_groupmaster", "distinct gpm_type", null);
                ddlGroupType.DataTextField = "gpm_type";
                ddlGroupType.DataValueField = "gpm_type";
                ddlGroupType.DataBind();

            }
            //_____For performing operation without refreshing page___//
            String cbReference = Page.ClientScript.GetCallbackEventReference(this, "arg", "ReceiveServerData", "context");
            String callbackScript = "function CallServer(arg, context){ " + cbReference + ";}";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "CallServer", callbackScript, true);
            //___________-end here___//

        }
        void Date()
        {
            DtFor.EditFormatString = oconverter.GetDateFormat("Date");
            DtFor.Value = Convert.ToDateTime(oDBEngine.GetDate().ToShortDateString());


        }
        string ICallbackEventHandler.GetCallbackResult()
        {
            return data;
        }
        void ICallbackEventHandler.RaiseCallbackEvent(string eventArgument)
        {
            string id = eventArgument.ToString();
            string[] idlist = id.Split('~');
            string str = "";
            string str1 = "";
            if (idlist[0] == "ComboChange")
            {
                //MainAcID = idlist[1];
            }
            else
            {
                string[] cl = idlist[1].Split(',');
                for (int i = 0; i < cl.Length; i++)
                {
                    string[] val = cl[i].Split(';');
                    if (str == "")
                    {
                        str = val[0];
                        str1 = val[1];
                    }
                    else
                    {
                        str += "," + val[0];
                        str1 += "," + val[1];
                    }
                }


                data = idlist[0] + '~' + str;

            }
        }

        protected void btnScreen_Click(object sender, EventArgs e)
        {
            DisPlay("Screen");
        }
        protected void btnMail_Click(object sender, EventArgs e)
        {
            DisPlay("Email");
        }
        protected void btnExcel_Click(object sender, EventArgs e)
        {
            DisPlay("Excel");
        }
        DataSet Procedure(string DisPlayType)
        {
            string[] InputName = new string[6];
            string[] InputType = new string[6];
            string[] InputValue = new string[6];

            /////////////////Parameter Name
            InputName[0] = "Date";
            InputName[1] = "Segment";
            InputName[2] = "Companyid";
            InputName[3] = "MasterSegment";
            InputName[4] = "Rptview";
            InputName[5] = "GrpCodeName";


            /////////////////Parameter Data Type
            InputType[0] = "V";
            InputType[1] = "V";
            InputType[2] = "V";
            InputType[3] = "V";
            InputType[4] = "V";
            InputType[5] = "V";

            /////////////////Parameter Value
            InputValue[0] = DtFor.Value.ToString().Trim();
            InputValue[1] = Session["usersegid"].ToString().Trim();
            InputValue[2] = Session["LastCompany"].ToString().Trim();
            InputValue[3] = Session["ExchangeSegmentID"].ToString().Trim();
            InputValue[4] = dllrptview.SelectedItem.Value.ToString().Trim();
            if (dllrptview.SelectedValue.Trim() == "Group" || dllrptview.SelectedValue.Trim() == "GroupClient")
                InputValue[5] = ddlGroupType.SelectedValue.Trim();
            else
                InputValue[5] = String.Empty;

            //////////////Sp Call
            ds = SQLProcedures.SelectProcedureArrDS("[Report_SettlementTrialNSEFO]", InputName, InputType, InputValue);
            ViewState["dataset"] = ds;
            return ds;
        }
        void DisPlay(string DisPlayType)
        {
            ds = Procedure(DisPlayType.ToString().Trim());
            if (ds.Tables[0].Rows[0]["Status"].ToString().Trim() == "2")
            {
                if (ds.Tables[1].Rows.Count > 0)
                {
                    if (DisPlayType.ToString().Trim() == "Screen")
                        FnHtml(ds);
                    if (DisPlayType.ToString().Trim() == "Excel")
                        Export(ds);
                    if (DisPlayType.ToString().Trim() == "Email")
                        EmailUserWiseEmail(ds);
                }
                else
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "fnNoRecord", "FnAlert('3');", true);
            }
            else
                ScriptManager.RegisterStartupScript(this, this.GetType(), "fnNoRecord", "FnAlert('1');", true);
        }
        string FnHtml(DataSet ds)
        {
            //////////For header
            String strHtmlheader = String.Empty;
            string str = null;
            str = "Report For :" + oconverter.ArrangeDate2(DtFor.Value.ToString());

            strHtmlheader = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";
            strHtmlheader += "<tr><td align=\"left\" colspan=" + ds.Tables[1].Columns.Count + " style=\"color:Blue;\">" + str + "</td></tr></table>";


            ////////Detail Display
            String strHtml = String.Empty;
            strHtml = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";


            //////////////TABLE HEADER BIND
            strHtml += "<tr style=\"background-color: #DBEEF3;\">";
            for (int i = 0; i < ds.Tables[1].Columns.Count; i++)
            {
                strHtml += "<td align=\"center\" style=\"font-size:smaller;\" nowrap=nowrap;><b>" + ds.Tables[1].Columns[i].ColumnName + "</b></td>";
            }
            strHtml += "</tr>";

            int flag = 0;
            foreach (DataRow dr1 in ds.Tables[1].Rows)
            {
                flag = flag + 1;
                strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
                for (int j = 0; j < ds.Tables[1].Columns.Count; j++)
                {
                    if (dr1[j] != DBNull.Value)
                    {
                        if (dr1[j].ToString().Trim().StartsWith("Client Total:") || dr1[j].ToString().Trim().StartsWith("Grand Client Total:") || dr1[j].ToString().Trim().StartsWith("Branch") || dr1[j].ToString().Trim().StartsWith("Group") || dr1[j].ToString().Trim().StartsWith("Total:") || dr1[j].ToString().Trim().StartsWith("Client Net :") || dr1[j].ToString().Trim().StartsWith("Round") || dr1[j].ToString().Trim().StartsWith("NoGroup[XXX"))
                        {
                            strHtml += "<td align=\"left\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + ds.Tables[1].Columns[j].ColumnName + "\"><b>" + dr1[j] + "</b></td>";
                        }
                        else if (dr1[j].ToString().Trim().StartsWith("Test") || dr1[j].ToString().Trim().StartsWith("ZZ"))
                        {
                            strHtml += "<td>&nbsp;</td>";
                        }
                        else if (dr1[j].ToString().Trim().StartsWith("Diff (If Any) :"))
                        {
                            strHtml += "<td align=\"left\" style=\"font-size:smaller;color:red;\"  nowrap=nowrap; title=\"" + ds.Tables[1].Columns[j].ColumnName + "\"><b>" + dr1[j] + "</b></td>";
                        }
                        else if (dr1[j].ToString().Trim().StartsWith("Exchange Obligation :"))
                        {
                            strHtml += "<td align=\"left\" style=\"font-size:smaller;color:green;\"  nowrap=nowrap; title=\"" + ds.Tables[1].Columns[j].ColumnName + "\"><b>" + dr1[j] + "</b></td>";
                        }
                        else
                        {
                            if (IsNumeric(dr1[j].ToString()) == true)
                            {
                                strHtml += "<td align=\"right\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + ds.Tables[1].Columns[j].ColumnName + "\">" + dr1[j] + "</td>";

                            }
                            else
                            {
                                strHtml += "<td align=\"left\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + ds.Tables[1].Columns[j].ColumnName + "\">" + dr1[j] + "</td>";

                            }
                        }
                    }
                    else
                    {
                        strHtml += "<td>&nbsp;</td>";
                    }
                }

                strHtml += "</tr>";
            }
            strHtml += "</table>";
            if (ddlGeneration.SelectedItem.Value.ToString().Trim() == "1")
            {
                DivHeader.InnerHtml = strHtmlheader;
                DivdisPlay.InnerHtml = strHtml;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "fnRecord", "FnAlert('2');", true);
            }

            return strHtml;


        }
        protected string GetRowColor(int i)
        {
            if (i++ % 2 == 0)
                return "#fff0f5";
            else
                return "White";
        }
        public static bool IsNumeric(object value)
        {
            double dbl;
            return double.TryParse(value.ToString(), System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.InvariantInfo, out dbl);

        }
        void Export(DataSet ds)
        {
            DataTable dtExport = ds.Tables[1].Copy();


            string str = null;
            str = "Settlement Trial [ Report For :" + oconverter.ArrangeDate2(DtFor.Value.ToString()) + "]";


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

            if (cmbExport.SelectedItem.Value.ToString().Trim() == "E")
                objExcel.ExportToExcelforExcel(dtExport, "Settlement Trial", "Diff (If Any) :", dtReportHeader, dtReportFooter);
            if (cmbExport.SelectedItem.Value.ToString().Trim() == "P")
                objExcel.ExportToPDF(dtExport, "Settlement Trial", "Diff (If Any) :", dtReportHeader, dtReportFooter);
        }
        protected void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            ds = (DataSet)ViewState["dataset"];
            Export(ds);
        }
        void EmailUserWiseEmail(DataSet ds)
        {
            if (HiddenField_emmail.Value.ToString().Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "FnAlert", "FnAlert('7');", true);
            }
            else
            {
                string MainstrEmail = "UserMail", mailsendresult = "no";


                MainstrEmail = FnHtml(ds);
                string[] clnt = HiddenField_emmail.Value.ToString().Split(',');
                int kk = clnt.Length;
                for (int i = 0; i < clnt.Length; i++)
                {
                    if (oDBEngine.SendReportSt(MainstrEmail.ToString().Trim(), clnt[i].ToString().Trim(), oconverter.ArrangeDate2(DtFor.Value.ToString().Trim()), "Settlement Trial For : [" + oconverter.ArrangeDate2(DtFor.Value.ToString().Trim()) + "]") == true)
                    {
                        if (mailsendresult.ToString().Trim() == "errorsuccess")
                            mailsendresult = "someclienterror";
                        else
                            mailsendresult = "success";
                    }
                    else
                        mailsendresult = "errorsuccess";
                }

                if (mailsendresult.ToString().Trim() == "someclienterror")
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "fnRecord", "FnAlert(6);", true);
                if (mailsendresult.ToString().Trim() == "success")
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "fnRecord", "FnAlert(4);", true);
                if (mailsendresult.ToString().Trim() == "errorsuccess")
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "fnRecord", "FnAlert(5);", true);
            }
        }
    }
}