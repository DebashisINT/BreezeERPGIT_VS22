using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web;
using LibDosPrint;
using BusinessLogicLayer;

namespace ERP.OMS.Reports
{
    public partial class Reports_CONTRACTNOTEFO : System.Web.UI.Page
    {

        BusinessLogicLayer.Converter objConverter = new BusinessLogicLayer.Converter();
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);
        string str = "";
        string date = "";
        string mode = "";
        string[] arr = null;
        DataSet ds = new DataSet();
        string sXMLFileName, sOutputFileName, sData;
        BusinessLogicLayer.Reports ObjReport = new BusinessLogicLayer.Reports();
        protected void Page_Load(object sender, EventArgs e)
        {
            //    string[] sArQuery = new string[1];
            //    string sQuery = "";
            //    string virtualpath = "";
            //    //sQuery = "Select * from Mst_Company ";

            //    if (Request.QueryString["str"].ToString() != "")
            //    {
            //        sQuery = Request.QueryString["str"].ToString();
            //    }
            //    if (Request.QueryString["path"].ToString() != "")
            //    {
            //        virtualpath = Request.QueryString["path"].ToString();
            //    }
            //    //sQuery = "exec [Contract_Report] 'COI0000001','1','NSE - CM','2009-10-23','''CLA0000060''','2782252','','S',1,'2009-10-23','','2009197','N'";

            //    sArQuery[0] = sQuery;
            //    string sXMLFileName, sOutputFileName, sData;
            //    sXMLFileName = Server.MapPath("ReportFormat") + "\\ContractNote_" + HttpContext.Current.Session["usersegid"].ToString() + ".xml";
            //    sOutputFileName = virtualpath;

            //    StreamReader fp;
            //    fp = File.OpenText(sXMLFileName);
            //    sData = fp.ReadToEnd();
            //    fp.Close();

            //    // Dim objPrint As New LibDosPrint.DosPrint(sData) ''xml file
            //    LibDosPrint.DosPrint objPrint = new LibDosPrint.DosPrint(sData, sArQuery);

            //    objPrint.PrintMainReport(sOutputFileName); //'' out put file
            //    objPrint = null;
            //    sData = null;

            //    fp = File.OpenText(sOutputFileName,null);
            //    sData = fp.ReadToEnd();
            //    fp.Close();

            //    Response.Write(sData);
            //}

            string[] sArQuery = new string[1];
            string sQuery = "";
            string virtualpath = "";
            ////sQuery = "Select * from Mst_Company ";
            //str = string.Empty;
            //string[] str1 = null;
            //if (Request.QueryString["str"].ToString() != "")
            //{
            //    sQuery = Request.QueryString["str"].ToString();
            //}
            if (Request.QueryString["path"].ToString() != "")
            {
                virtualpath = Server.MapPath("ReportOutput") + Request.QueryString["path"].ToString();
                // virtualpath = Request.QueryString["path"].ToString();


            }
            //sQuery = "exec [Contract_Report] 'COI0000001','1','NSE - CM','2009-10-23','''CLA0000060''','2782252','','S',1,'2009-10-23','','2009197','N'";
            sArQuery[0] = sQuery;


            sXMLFileName = Server.MapPath("ReportFormat") + "\\ContractNote_" + HttpContext.Current.Session["usersegid"].ToString() + ".xml";

            sOutputFileName = virtualpath;



            if (HttpContext.Current.Session["Tradedate"] != null)
            {
                arr = HttpContext.Current.Session["Tradedate"].ToString().Split('#');
                date = arr[0].ToString();

            }
            else if (HttpContext.Current.Session["date"] != null)
            {
                date = HttpContext.Current.Session["date"].ToString();
            }
            //if (HttpContext.Current.Session["Customer"] == null)
            //{
            //    HttpContext.Current.Session["Customer"].ToString() = "";
            //}
            //if (HttpContext.Current.Session["contractID"] == null)
            //{
            //    HttpContext.Current.Session["contractID"].ToString() = "";
            //}
            //if (HttpContext.Current.Session["AuthorizeName"] == null)
            //{
            //    HttpContext.Current.Session["AuthorizeName"].ToString() = "";
            //}
            if (HttpContext.Current.Session["FromNo"] != null && HttpContext.Current.Session["ToNo"] != null)
            {
                if (HttpContext.Current.Session["FromNo"].ToString() != "" && HttpContext.Current.Session["ToNo"].ToString() != "")
                {

                    try
                    {

                        /*
                        SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]);


                        SqlCommand cmd = new SqlCommand();
                        cmd.Connection = con;
                        cmd.CommandText = "[Contract_Report13]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@CompanyID", HttpContext.Current.Session["LastCompany"].ToString());
                        cmd.Parameters.AddWithValue("@DpId", HttpContext.Current.Session["usersegid"].ToString());
                        cmd.Parameters.AddWithValue("@dp", "NSE - FO");
                        cmd.Parameters.AddWithValue("@tradedate", date);
                        cmd.Parameters.AddWithValue("@CustomerID", HttpContext.Current.Session["CustomerID"].ToString());
                        cmd.Parameters.AddWithValue("@ContractNote", HttpContext.Current.Session["contractID"].ToString());
                        cmd.Parameters.AddWithValue("@AuthorizeName", "");
                        cmd.Parameters.AddWithValue("@Mode", HttpContext.Current.Session["mode"].ToString());
                        cmd.Parameters.AddWithValue("@SegmentExchangeID", Convert.ToInt32(HttpContext.Current.Session["ExchangeSegmentID"].ToString()));
                        cmd.Parameters.AddWithValue("@strFundPayoutDate", HttpContext.Current.Session["FundPayoutDate"].ToString());
                        cmd.Parameters.AddWithValue("@BrkgFlag", "");
                        cmd.Parameters.AddWithValue("@SettlementNumber", HttpContext.Current.Session["LastSettNo"].ToString().Substring(0, 7));
                        cmd.Parameters.AddWithValue("@SettlementType", HttpContext.Current.Session["LastSettNo"].ToString().Substring(7, 1));
                        cmd.Parameters.AddWithValue("@Branch", HttpContext.Current.Session["BranchID"].ToString());
                        cmd.Parameters.AddWithValue("@Customer", HttpContext.Current.Session["Customer"].ToString());
                        cmd.Parameters.AddWithValue("@FromNo", HttpContext.Current.Session["FromNo"].ToString());
                        cmd.Parameters.AddWithValue("@ToNo", HttpContext.Current.Session["ToNo"].ToString());
                        cmd.CommandTimeout = 0;
                        SqlDataAdapter da = new SqlDataAdapter();
                        da.SelectCommand = cmd;
                        da.Fill(ds);

                        */

                        ds = ObjReport.Get_Contract_Report13(HttpContext.Current.Session["LastCompany"].ToString(), HttpContext.Current.Session["usersegid"].ToString(),
                             "NSE - FO", date, HttpContext.Current.Session["CustomerID"].ToString(), HttpContext.Current.Session["contractID"].ToString(), "",
                             HttpContext.Current.Session["mode"].ToString(), HttpContext.Current.Session["ExchangeSegmentID"].ToString(),
                             HttpContext.Current.Session["FundPayoutDate"].ToString(), "", HttpContext.Current.Session["LastSettNo"].ToString().Substring(0, 7),
                             HttpContext.Current.Session["LastSettNo"].ToString().Substring(7, 1), HttpContext.Current.Session["BranchID"].ToString(),
                             HttpContext.Current.Session["CustomerID"].ToString(), HttpContext.Current.Session["FromNo"].ToString(),
                             HttpContext.Current.Session["ToNo"].ToString());


                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            StreamReader fp;
                            fp = File.OpenText(sXMLFileName);
                            sData = fp.ReadToEnd();
                            fp.Close();

                            //// Dim objPrint As New LibDosPrint.DosPrint(sData) ''xml file
                            DosPrint objPrint = new DosPrint(sData, sArQuery);

                            objPrint.PrintMainReport(sOutputFileName, ds); //'' out put file
                            objPrint = null;
                            sData = null;

                            fp = File.OpenText(sOutputFileName);
                            sData = fp.ReadToEnd();
                            fp.Close();

                            //ClientScript.RegisterStartupScript(this, this.GetType(), "jsscript44", "alert('OK!')", true);
                        }
                        else
                        {
                            sData = "";
                            //ClientScript.RegisterStartupScript(this, this.GetType(), "jsscript44", "alert('No Data Found!')", true);
                        }

                        Response.Write(sData);


                    }
                    catch (Exception ex)
                    {
                    }
                    finally
                    {
                    }
                }
                else
                {
                    try
                    {
                        /* For Tier Structure --------
                        SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]);


                        SqlCommand cmd = new SqlCommand();
                        cmd.Connection = con;
                        cmd.CommandText = "[Contract_Report13]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@CompanyID", HttpContext.Current.Session["LastCompany"].ToString());
                        cmd.Parameters.AddWithValue("@DpId", HttpContext.Current.Session["usersegid"].ToString());
                        cmd.Parameters.AddWithValue("@dp", "NSE - FO");
                        cmd.Parameters.AddWithValue("@tradedate", date);
                        cmd.Parameters.AddWithValue("@CustomerID", HttpContext.Current.Session["CustomerID"].ToString());
                        cmd.Parameters.AddWithValue("@ContractNote", HttpContext.Current.Session["contractID"].ToString());
                        cmd.Parameters.AddWithValue("@AuthorizeName", "");
                        cmd.Parameters.AddWithValue("@Mode", HttpContext.Current.Session["mode"].ToString());
                        cmd.Parameters.AddWithValue("@SegmentExchangeID", Convert.ToInt32(HttpContext.Current.Session["ExchangeSegmentID"].ToString()));
                        cmd.Parameters.AddWithValue("@strFundPayoutDate", HttpContext.Current.Session["FundPayoutDate"].ToString());
                        cmd.Parameters.AddWithValue("@BrkgFlag", "");
                        cmd.Parameters.AddWithValue("@SettlementNumber", HttpContext.Current.Session["LastSettNo"].ToString().Substring(0, 7));
                        cmd.Parameters.AddWithValue("@SettlementType", HttpContext.Current.Session["LastSettNo"].ToString().Substring(7, 1));
                        cmd.Parameters.AddWithValue("@Branch", HttpContext.Current.Session["BranchID"].ToString());
                        cmd.Parameters.AddWithValue("@Customer", HttpContext.Current.Session["Customer"].ToString());
                        cmd.Parameters.AddWithValue("@FromNo", "");
                        cmd.Parameters.AddWithValue("@ToNo", "");

                        cmd.CommandTimeout = 0;
                        SqlDataAdapter da = new SqlDataAdapter();
                        da.SelectCommand = cmd;
                        da.Fill(ds);

                        */

                        ds = ObjReport.Get_Contract_Report13(HttpContext.Current.Session["LastCompany"].ToString(), HttpContext.Current.Session["usersegid"].ToString(),
                            "NSE - FO", date, HttpContext.Current.Session["CustomerID"].ToString(), HttpContext.Current.Session["contractID"].ToString(), "",
                            HttpContext.Current.Session["mode"].ToString(), HttpContext.Current.Session["ExchangeSegmentID"].ToString(),
                            HttpContext.Current.Session["FundPayoutDate"].ToString(), "", HttpContext.Current.Session["LastSettNo"].ToString().Substring(0, 7),
                            HttpContext.Current.Session["LastSettNo"].ToString().Substring(7, 1), HttpContext.Current.Session["BranchID"].ToString(),
                            HttpContext.Current.Session["CustomerID"].ToString(), "", "");


                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            StreamReader fp;
                            fp = File.OpenText(sXMLFileName);
                            sData = fp.ReadToEnd();
                            fp.Close();

                            //// Dim objPrint As New LibDosPrint.DosPrint(sData) ''xml file
                            DosPrint objPrint = new DosPrint(sData, sArQuery);

                            objPrint.PrintMainReport(sOutputFileName, ds); //'' out put file
                            objPrint = null;
                            sData = null;

                            fp = File.OpenText(sOutputFileName);
                            sData = fp.ReadToEnd();
                            fp.Close();

                            //ClientScript.RegisterStartupScript(this, this.GetType(), "jsscript44", "alert('OK!')", true);
                        }
                        else
                        {
                            sData = "";
                            //ClientScript.RegisterStartupScript(this, this.GetType(), "jsscript44", "alert('No Data Found!')", true);
                        }

                        Response.Write(sData);


                    }
                    catch (Exception ex)
                    {
                    }
                    finally
                    {
                    }
                }
            }
        }
    }
}