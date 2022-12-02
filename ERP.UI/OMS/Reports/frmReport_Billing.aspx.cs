using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using BusinessLogicLayer;

namespace ERP.OMS.Reports
{
    public partial class Reports_frmReport_Billing : System.Web.UI.Page
    {
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);
        BusinessLogicLayer.Converter oconverter = new BusinessLogicLayer.Converter();
        BusinessLogicLayer.DailyTaskOther oDailyTaskOther = new BusinessLogicLayer.DailyTaskOther();
        DataSet ds = new DataSet();
        int ExchangeSegment = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script language='javascript'>Page_Load();</script>");
                dtFrom.EditFormatString = oconverter.GetDateFormat("Date");
                dtTo.EditFormatString = oconverter.GetDateFormat("Date");
                DateTime fromdate = Convert.ToDateTime(oDBEngine.GetDate().ToShortDateString());
                string dd1 = (HttpContext.Current.Session["LastFinYear"].ToString());
                string[] dd2 = dd1.Split('-');
                int year = Convert.ToInt32(dd2[1].ToString().Trim());
                DateTime startDate = new DateTime(year, 3, 31); // 1st Feb this year
                if (fromdate >= startDate)
                {
                    dtFrom.Value = Convert.ToDateTime(startDate.ToShortDateString());
                    dtTo.Value = Convert.ToDateTime(startDate.ToShortDateString());
                }
                else
                {
                    dtTo.Value = Convert.ToDateTime(oDBEngine.GetDate().ToShortDateString());
                    dtFrom.Value = Convert.ToDateTime(oDBEngine.GetDate().ToShortDateString());
                }



                //dtTo.Value = Convert.ToDateTime(oDBEngine.GetDate().ToShortDateString());
            }

            //Get Currenct Exchange Segment
            ExchangeSegment = Convert.ToInt32(Session["ExchangeSegmentID"].ToString());
        }

        protected void btnGenerate_Click(object sender, EventArgs e)
        {
            if (dllGenerate.SelectedItem.Value.ToString().Trim() == "G")/////For Billing
            {
                if (ExchangeSegment != 10 && ExchangeSegment != 18 && ExchangeSegment != 21 && ExchangeSegment != 22 && ExchangeSegment != 23 && ExchangeSegment != 25 && ExchangeSegment != 26 && ExchangeSegment != 27)
                    ProcedureBill();
                else
                    ProcedureBill("Check");
            }
            if (dllGenerate.SelectedItem.Value.ToString().Trim() == "D")/////For Delete Billing
            {
                if (ExchangeSegment != 10 && ExchangeSegment != 18 && ExchangeSegment != 21 && ExchangeSegment != 22 && ExchangeSegment != 23 && ExchangeSegment != 25 && ExchangeSegment != 26 && ExchangeSegment != 27)
                    ProcedureBill();
                else
                    ProcedureDel();
            }
        }

        ///Old 
        void ProcedureBill(string SpCall)
        {
            if (SpCall.ToString().Trim() == "Check")
            {
                ds = oDailyTaskOther.BillingCommCurrencyCheck(
                    Convert.ToString(dtFrom.Value),
                       Convert.ToString(dtTo.Value),
                       Convert.ToString(Session["usersegid"]),
                       Convert.ToString(Session["LastCompany"]),
                       Convert.ToString(HttpContext.Current.Session["ExchangeSegmentID"]),
                       Convert.ToString(HttpContext.Current.Session["LastFinYear"]),
                       Convert.ToString(Session["LastSettNo"].ToString().Trim().Substring(0, 7)),
                         Convert.ToString(Session["LastSettNo"].ToString().Trim().Substring(7, 1)),
                       Convert.ToString(HttpContext.Current.Session["userid"])
                    );
            }
            else
            {
                ds = oDailyTaskOther.BillingCommCurrencyDateWise(
                      Convert.ToString(dtFrom.Value),
                       Convert.ToString(dtTo.Value),
                       Convert.ToString(Session["usersegid"]),
                       Convert.ToString(Session["LastCompany"]),
                       Convert.ToString(HttpContext.Current.Session["ExchangeSegmentID"]),
                       Convert.ToString(HttpContext.Current.Session["LastFinYear"]),
                       Convert.ToString(Session["LastSettNo"].ToString().Trim().Substring(0, 7)),
                         Convert.ToString(Session["LastSettNo"].ToString().Trim().Substring(7, 1)),
                       Convert.ToString(HttpContext.Current.Session["userid"]));
            }

            if (SpCall.ToString().Trim() == "Check")
            {
                HtmlBind(ds);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "fnAlert", "fnAlert(" + ds.Tables[0].Rows[0]["StatusName"].ToString().Trim() + ");", true);
            }

        }
        ///Old Billing
        void HtmlBind(DataSet ds)
        {
            if (ds.Tables[0].Rows[0]["StatusName"].ToString().Trim() == "1" || ds.Tables[0].Rows[0]["StatusName"].ToString().Trim() == "2")
            {
                String strHtml = String.Empty;
                strHtml = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";

                string str = null;

                strHtml += "<tr><td align=\"left\" colspan=" + ds.Tables[1].Columns.Count + " style=\"color:Blue;\"><b> Following Information is Missing.....</b></td></tr>";

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
                    strHtml += "<tr style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
                    for (int j = 0; j < ds.Tables[1].Columns.Count; j++)
                    {
                        if (dr1[j] != DBNull.Value)
                        {
                            strHtml += "<td align=\"left\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + ds.Tables[1].Columns[j].ColumnName + "\">" + dr1[j] + "</td>";
                        }
                        else
                        {
                            strHtml += "<td>&nbsp;</td>";
                        }
                    }

                    strHtml += "</tr>";
                }
                if (ds.Tables[0].Rows[0]["StatusName"].ToString().Trim() == "1")
                {
                    strHtml += "<tr><td align=\"left\" colspan=" + ds.Tables[1].Columns.Count + " style=\"color:Blue;\"><b> Please Complete The Process For Above Days and Generate Bills...</b></td></tr>";
                }

                strHtml += "</table>";
                Divdisplay.InnerHtml = strHtml;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "fnAlert", "fnAlert(" + ds.Tables[0].Rows[0]["StatusName"].ToString().Trim() + ");", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "fnAlert", "fnAlert(" + ds.Tables[0].Rows[0]["StatusName"].ToString().Trim() + ");", true);
            }
        }
        ///---------------------------------------------------------------////
        ///New
        void ProcedureBill()
        {
            string[] sqlParameterName = new string[10];
            string[] sqlParameterType = new string[10];
            string[] sqlParameterValue = new string[10];
            string[] sqlParameterSize = new string[10];

            if (ExchangeSegment != 10 && ExchangeSegment != 18 && ExchangeSegment != 21 && ExchangeSegment != 22 && ExchangeSegment != 23 && ExchangeSegment != 25 && ExchangeSegment != 26 && ExchangeSegment != 27)
            {
                sqlParameterName = new string[10];
                sqlParameterType = new string[10];
                sqlParameterValue = new string[10];
                sqlParameterSize = new string[10];
            }
            else
            {
                sqlParameterName = new string[9];
                sqlParameterType = new string[9];
                sqlParameterValue = new string[9];
                sqlParameterSize = new string[9];
            }

            string ErrorMsg = String.Empty;
            DataSet DsBillProcess = new DataSet();
            sqlParameterName[0] = "DateFrom";
            sqlParameterValue[0] = dtFrom.Value.ToString();
            sqlParameterType[0] = "D";
            sqlParameterSize[0] = "20";

            sqlParameterName[1] = "DateTo";
            sqlParameterValue[1] = dtTo.Value.ToString();
            sqlParameterType[1] = "D";
            sqlParameterSize[1] = "20";

            sqlParameterName[2] = "Company";
            sqlParameterValue[2] = Session["LastCompany"].ToString().Trim();
            sqlParameterType[2] = "V";
            sqlParameterSize[2] = "200";

            sqlParameterName[3] = "SegmentID";
            sqlParameterValue[3] = Session["usersegid"].ToString().Trim();
            sqlParameterType[3] = "V";
            sqlParameterSize[3] = "200";

            sqlParameterName[4] = "MasterSegmentID";
            sqlParameterValue[4] = HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim();
            sqlParameterType[4] = "V";
            sqlParameterSize[4] = "10";

            sqlParameterName[5] = "FinYear";
            sqlParameterValue[5] = HttpContext.Current.Session["LastFinYear"].ToString().Trim();
            sqlParameterType[5] = "V";
            sqlParameterSize[5] = "10";

            sqlParameterName[6] = "SettNumber";
            sqlParameterValue[6] = Session["LastSettNo"].ToString().Trim().Substring(0, 7);
            sqlParameterType[6] = "V";
            sqlParameterSize[6] = "7";

            sqlParameterName[7] = "SettType";
            sqlParameterValue[7] = Session["LastSettNo"].ToString().Trim().Substring(7, 1);
            sqlParameterType[7] = "V";
            sqlParameterSize[7] = "2";

            sqlParameterName[8] = "CreateUser";
            sqlParameterValue[8] = HttpContext.Current.Session["userid"].ToString().Trim();
            sqlParameterType[8] = "V";
            sqlParameterSize[8] = "10";


            sqlParameterName[9] = "Type";
            sqlParameterValue[9] = dllGenerate.SelectedItem.Value.ToString().Trim();
            sqlParameterType[9] = "V";
            sqlParameterSize[9] = "2";

            DsBillProcess = SQLProcedures.SelectProcedureArrDS("Billing_ComCDX", sqlParameterName, sqlParameterType, sqlParameterValue);
            if (DsBillProcess.Tables.Count > 0)
            {
                if (DsBillProcess.Tables[0].Rows.Count > 0)
                {
                    if (DsBillProcess.Tables[0].Rows[0][0].ToString().Contains("Locked") || DsBillProcess.Tables[0].Rows[0][0].ToString().Contains("Another User") ||
                        DsBillProcess.Tables[0].Rows[0][0].ToString().Contains("Successfully") | DsBillProcess.Tables[0].Rows[0][0].ToString().Contains("Unable To Process"))
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Msg", "alert('" + DsBillProcess.Tables[0].Rows[0][0].ToString().Trim() + "');", true);
                    else
                        if (dllGenerate.SelectedItem.Value.ToString().Trim() == "G")
                            HtmlBindNewBilling(DsBillProcess);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "fnAlert", "fnAlert(5);", true);
                }
            }


        }
        ///for New Billing
        void HtmlBindNewBilling(DataSet ds)
        {
            String strHtml = String.Empty;
            strHtml = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";

            string str = null;

            strHtml += "<tr><td align=\"left\" colspan=" + ds.Tables[0].Columns.Count + " style=\"color:Blue;\"><b> Following Information is Missing.....</b></td></tr>";

            //////////////TABLE HEADER BIND
            strHtml += "<tr style=\"background-color: #DBEEF3;\">";
            for (int i = 0; i < ds.Tables[0].Columns.Count; i++)
            {
                strHtml += "<td align=\"center\" style=\"font-size:smaller;\" nowrap=nowrap;><b>" + ds.Tables[0].Columns[i].ColumnName + "</b></td>";
            }
            strHtml += "</tr>";

            int flag = 0;
            foreach (DataRow dr1 in ds.Tables[0].Rows)
            {
                flag = flag + 1;
                strHtml += "<tr style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
                for (int j = 0; j < ds.Tables[0].Columns.Count; j++)
                {
                    if (dr1[j] != DBNull.Value)
                    {
                        strHtml += "<td align=\"left\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + ds.Tables[0].Columns[j].ColumnName + "\">" + dr1[j] + "</td>";
                    }
                    else
                    {
                        strHtml += "<td>&nbsp;</td>";
                    }
                }

                strHtml += "</tr>";
            }

            strHtml += "<tr><td align=\"left\" colspan=" + ds.Tables[0].Columns.Count + " style=\"color:Blue;\"><b> Please Complete The Process For Above Days and Generate Bills...</b></td></tr>";
            strHtml += "</table>";
            Divdisplay.InnerHtml = strHtml;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "fnAlert", "fnAlert(1);", true);
        }

        protected string GetRowColor(int i)
        {
            if (i++ % 2 == 0)
                return "#fff0f5";
            else
                return "White";
        }
        protected void BtnRemainingBill_Click(object sender, EventArgs e)
        {
            ProcedureBill("Remaining");
        }
        void ProcedureDel()
        {
            ds = oDailyTaskOther.BillingCommCurrencyDelete(
                           Convert.ToString(dtFrom.Value),
                            Convert.ToString(dtTo.Value),
                            Convert.ToString(Session["usersegid"]),
                             Convert.ToString(Session["LastCompany"])
                          );
            ScriptManager.RegisterStartupScript(this, this.GetType(), "fnAlert", "fnAlert(" + ds.Tables[0].Rows[0]["StatusName"].ToString().Trim() + ");", true);

        }
    }
}