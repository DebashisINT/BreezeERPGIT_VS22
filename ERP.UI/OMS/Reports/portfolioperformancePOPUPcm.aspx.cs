using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
//using BusinessLogicLayer;

namespace ERP.OMS.Reports
{
    public partial class Reports_portfolioperformancePOPUPcm : System.Web.UI.Page
    {
        BusinessLogicLayer.Converter oconverter = new BusinessLogicLayer.Converter();
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        DataSet ds = new DataSet();
        BusinessLogicLayer.Reports Reports = new BusinessLogicLayer.Reports();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (HttpContext.Current.Session["userid"] == null)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
            if (!IsPostBack)
            {
                hiddencount.Value = "0";
                procedure();
            }
        }
        void clienttype()
        {
            if (Request.QueryString["clienttype"].ToString() == "01")
            {
                ViewState["clienttype"] = "cnt_clienttype='Pro Trading'";
            }
            else if (Request.QueryString["clienttype"].ToString() == "02")
            {
                ViewState["clienttype"] = "cnt_clienttype='Pro Investment'";
            }
            else
            {
                ViewState["clienttype"] = "(cnt_clienttype not in ('Pro Investment','Pro Trading') or cnt_clienttype is null)";
            }
        }
        void procedure()
        {
            clienttype();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]))
            {

                //    SqlCommand cmd = new SqlCommand();
                //    cmd.Connection = con;
                //    if (Request.QueryString["consolidatedchk"].ToString().Trim() == "true")
                //    {
                //        cmd.CommandText = "[PerformanceReportCM_JournalVoucher]";
                //    }
                //    else
                //    {
                //        cmd.CommandText = "[PerformanceReportCM]";
                //    }

                //    cmd.CommandType = CommandType.StoredProcedure;
                //    cmd.Parameters.AddWithValue("@companyid", Session["LastCompany"].ToString());
                //    cmd.Parameters.AddWithValue("@segment", Request.QueryString["Segment"].ToString());

                //    string[] idlist = Request.QueryString["Date"].ToString().Split('~');

                //    if (idlist[1].ToString().Trim()=="1")
                //    {
                //        cmd.Parameters.AddWithValue("@fromdate", idlist[0].ToString());
                //        cmd.Parameters.AddWithValue("@todate", "NA");
                //    }
                //    else
                //    {
                //        cmd.Parameters.AddWithValue("@fromdate", idlist[0].ToString());
                //        cmd.Parameters.AddWithValue("@todate", idlist[1].ToString());
                //    }

                //    cmd.Parameters.AddWithValue("@clients", "'" + Request.QueryString["CUSTOMERID"].ToString() + "'");
                //    cmd.Parameters.AddWithValue("@Seriesid", Request.QueryString["MASTERPRODUCTID"].ToString());
                //    cmd.Parameters.AddWithValue("@finyear", HttpContext.Current.Session["LastFinYear"]);
                //    cmd.Parameters.AddWithValue("@grptype", Request.QueryString["grp"].ToString());
                //    cmd.Parameters.AddWithValue("@rpttype", "1");
                //    cmd.Parameters.AddWithValue("@PRINTCHK", "SHOW");
                //    cmd.Parameters.AddWithValue("@clienttype", ViewState["clienttype"].ToString());
                //    cmd.Parameters.AddWithValue("@closemethod", Request.QueryString["closemethod"].ToString().Trim());
                //    if(Request.QueryString["sqroffchk"].ToString().Trim()=="true")
                //    {
                //        cmd.Parameters.AddWithValue("@chkexcludesqr", "CHK");
                //    }
                //    else
                //    {
                //        cmd.Parameters.AddWithValue("@chkexcludesqr", "UNCHK");
                //    }
                //    if (Request.QueryString["consolidatedchk"].ToString().Trim() == "true")
                //    {
                //        cmd.Parameters.AddWithValue("@jvcreate", "NO");
                //        cmd.Parameters.AddWithValue("@SEGMENTJV", "NA");
                //    }
                //    cmd.Parameters.AddWithValue("@chkstt", "NA");
                //cmd.Parameters.AddWithValue("@ValuationDate", idlist[0].ToString().Trim());
                //    SqlDataAdapter da = new SqlDataAdapter();
                //    da.SelectCommand = cmd;
                //    cmd.CommandTimeout = 0;
                //    ds.Reset();
                //    da.Fill(ds);
                //    da.Dispose();
                //    ViewState["dataset"] = ds;
                string[] idlist = Request.QueryString["Date"].ToString().Split('~');
                ds.Reset();
                ds = Reports.PerformanceReportCM(Request.QueryString["consolidatedchk"].ToString().Trim(), Session["LastCompany"].ToString(),
                    Request.QueryString["Segment"].ToString(), idlist[1].ToString().Trim(), idlist[0].ToString(), idlist[1].ToString(),
                    "'" + Request.QueryString["CUSTOMERID"].ToString() + "'", Request.QueryString["MASTERPRODUCTID"].ToString(),
                   Convert.ToString(HttpContext.Current.Session["LastFinYear"]), Request.QueryString["grp"].ToString(), ViewState["clienttype"].ToString(),
                    Request.QueryString["closemethod"].ToString().Trim(), Request.QueryString["sqroffchk"].ToString().Trim(), idlist[0].ToString().Trim());
                ViewState["dataset"] = ds;
                htmltable();
            }
        }
        void htmltable()
        {
            ds = (DataSet)ViewState["dataset"];
            String strHtml = String.Empty;
            strHtml = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";

            DataView viewclient = new DataView();
            viewclient = ds.Tables[0].DefaultView;
            viewclient.RowFilter = "CUSTOMERID='" + Request.QueryString["CUSTOMERID"].ToString() + "'";
            DataTable dt1 = new DataTable();
            dt1 = viewclient.ToTable();

            strHtml += "<tr><td align=\"left\" style=\"color:Blue;\" colspan=10>Client Name:<b>" + dt1.Rows[0]["CLIENTNAME"].ToString().Trim() + "</b>[ <b>" + dt1.Rows[0]["UCC"].ToString().Trim() + "</b> ] ,Security Name :<b>" + dt1.Rows[0]["PRODUCTNAME"].ToString().Trim() + "</b></td></tr>";


            strHtml += "<tr style=\"background-color: #DBEEF3;\">";
            strHtml += "<td align=\"center\" ><b>Segmnt</b></td>";
            strHtml += "<td align=\"center\" ><b>Date</b></td>";
            strHtml += "<td align=\"center\" ><b>SettNo</b></td>";
            strHtml += "<td align=\"center\"><b>Buy </br> Qty</b></td>";
            strHtml += "<td align=\"center\"><b>Sell </br> Qty</b></td>";
            strHtml += "<td align=\"center\"><b>Rate</td>";
            strHtml += "<td align=\"center\"><b>Buy </br> Value</b></td>";
            strHtml += "<td align=\"center\"><b>Sell  </br> Value</b></td>";
            strHtml += "<td align=\"center\" ><b>Type</b></td>";
            if (Request.QueryString["sqroffchk"].ToString().Trim() == "false")
            {
                strHtml += "<td align=\"center\" ><b>Sqr  </br> Qty</b></td>";
                strHtml += "<td align=\"center\" ><b>Sqr-Of</br> P/L</b></td>";
            }
            strHtml += "<td align=\"center\"><b>Short</br> Term </br>  P/L </b></td>";
            strHtml += "<td align=\"center\"><b>Long </br> Term  </br> P/L</b></td>";
            strHtml += "<td align=\"center\"><b>Close </br>  Qty</b></td>";
            strHtml += "<td align=\"center\"><b>Close.  </br> Price</b></td>";
            strHtml += "<td align=\"center\"><b>Close.   </br> Val</b></td>";

            strHtml += "</tr>";

            int flag = 0;

            for (int i = 0; i < dt1.Rows.Count; i++)
            {
                flag = flag + 1;
                strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
                strHtml += "<td align=\"left\" style=\"font-size:xx-small;\" nowrap=\"nowrap\">" + dt1.Rows[i]["SEGMENT"].ToString() + "</td>";
                strHtml += "<td align=\"left\" style=\"font-size:xx-small;\" nowrap=\"nowrap\">" + dt1.Rows[i]["DATE"].ToString() + "</td>";
                strHtml += "<td align=\"left\" style=\"font-size:xx-small;\" nowrap=\"nowrap\">" + dt1.Rows[i]["SETTNO"].ToString() + "</td>";

                if (dt1.Rows[i]["BUYQTY"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\">" + dt1.Rows[i]["BUYQTY"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" >&nbsp;</td>";

                if (dt1.Rows[i]["SELLQTY"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\">" + dt1.Rows[i]["SELLQTY"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" >&nbsp;</td>";

                if (dt1.Rows[i]["RATE"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\">" + dt1.Rows[i]["RATE"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" >&nbsp;</td>";

                if (dt1.Rows[i]["BUYVALUE"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\">" + dt1.Rows[i]["BUYVALUE"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" >&nbsp;</td>";

                if (dt1.Rows[i]["SELLVALUE"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\">" + dt1.Rows[i]["SELLVALUE"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" >&nbsp;</td>";

                strHtml += "<td align=\"left\" style=\"font-size:xx-small;\" nowrap=\"nowrap\">" + dt1.Rows[i]["BRKGTYPE"].ToString() + "</td>";

                if (Request.QueryString["sqroffchk"].ToString().Trim() == "false")
                {
                    if (dt1.Rows.Count - 1 == i)
                    {
                        if (dt1.Rows[i]["SQRQTY"] != DBNull.Value)
                            strHtml += "<td align=\"right\" style=\"font-size:xx-small;\">" + dt1.Rows[i]["SQRQTY"].ToString() + "</td>";
                        else
                            strHtml += "<td align=\"left\" >&nbsp;</td>";

                        if (dt1.Rows[i]["SQRPL"] != DBNull.Value)
                            strHtml += "<td align=\"right\" style=\"font-size:xx-small;\">" + dt1.Rows[i]["SQRPL"].ToString() + "</td>";
                        else
                            strHtml += "<td align=\"left\" >&nbsp;</td>";
                    }
                    else
                    {
                        strHtml += "<td align=\"left\" >&nbsp;</td>";
                        strHtml += "<td align=\"left\" >&nbsp;</td>";
                    }


                }

                if (dt1.Rows[i]["STPL"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\">" + dt1.Rows[i]["STPL"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" >&nbsp;</td>";

                if (dt1.Rows[i]["LTPL"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\">" + dt1.Rows[i]["LTPL"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" >&nbsp;</td>";

                if (dt1.Rows[i]["CLOSINGQTY"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\">" + dt1.Rows[i]["CLOSINGQTY"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" >&nbsp;</td>";

                if (dt1.Rows[i]["CLOSINGAVGCOST"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\">" + dt1.Rows[i]["CLOSINGAVGCOST"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" >&nbsp;</td>";

                if (dt1.Rows[i]["CLOSINGVALUE"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\">" + dt1.Rows[i]["CLOSINGVALUE"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" >&nbsp;</td>";


                strHtml += "</tr>";
            }
            strHtml += "</table>";

            display.InnerHtml = strHtml;

        }
        protected string GetRowColor(int i)
        {
            if (i++ % 2 == 0)
                return "#fff0f5";
            else
                return "White";
        }
    }
}