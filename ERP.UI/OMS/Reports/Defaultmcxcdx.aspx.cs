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
    public partial class Reports_Defaultmcxcdx : System.Web.UI.Page
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
            //response.Write("Hello");
            string[] sArQuery = new string[1];
            string sQuery = "";
            string virtualpath = "";
            ////sQuery = "Select * from Mst_Company ";
            //str = string.Empty;
            //string[] str1 = null;
            ////if (Request.QueryString["str"].ToString() != "")
            ////{
            ////    sQuery = Request.QueryString["str"].ToString();
            ////}
            if (Request.QueryString["path"].ToString() != "")
            {
                virtualpath = Server.MapPath("ReportOutput") + Request.QueryString["path"].ToString();
                // virtualpath = Request.QueryString["path"].ToString();


            }
            ////sQuery = "exec [Contract_Report] 'COI0000001','1','NSE - CM','2009-10-23','''CLA0000060''','2782252','','S',1,'2009-10-23','','2009197','N'";
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

            // if (HttpContext.Current.Session["FundPayoutDate"] == null)
            // {
            //     HttpContext.Current.Session["FundPayoutDate"].ToString() = "";
            // }
            //////if (HttpContext.Current.Session["contractID"] == null)
            //////{
            //////    HttpContext.Current.Session["contractID"].ToString() = "";
            //////}
            //////if (HttpContext.Current.Session["AuthorizeName"] == null)
            //////{
            //////    HttpContext.Current.Session["AuthorizeName"].ToString() = "";
            //////}

            //ClientScript.RegisterStartupScript(this, this.GetType(), "jsscript44", "alert('OK!')", true);
            try
            {
                /* For Tier Structure 

                SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]);


                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = "[ICEXContract_Reportdos]";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CompanyID", HttpContext.Current.Session["LastCompany"].ToString());
                cmd.Parameters.AddWithValue("@DpId", HttpContext.Current.Session["usersegid"].ToString());
                cmd.Parameters.AddWithValue("@dp", "MCXSX - CDX");
                cmd.Parameters.AddWithValue("@tradedate", date);
                cmd.Parameters.AddWithValue("@CustomerID", HttpContext.Current.Session["Customer"].ToString());
                cmd.Parameters.AddWithValue("@ContractNote", "");
                cmd.Parameters.AddWithValue("@AuthorizeName", "");
                cmd.Parameters.AddWithValue("@Mode", HttpContext.Current.Session["mode"].ToString());
                cmd.Parameters.AddWithValue("@SegmentExchangeID", Convert.ToInt32(HttpContext.Current.Session["ExchangeSegmentID"].ToString()));
                cmd.Parameters.AddWithValue("@strFundPayoutDate", "");
                cmd.Parameters.AddWithValue("@BrkgFlag", "");
                cmd.Parameters.AddWithValue("@SettlementNumber", HttpContext.Current.Session["LastSettNo"].ToString().Substring(0, 7));
                cmd.Parameters.AddWithValue("@SettlementType", HttpContext.Current.Session["LastSettNo"].ToString().Substring(7, 1));
                cmd.Parameters.AddWithValue("@Branch", HttpContext.Current.Session["BranchID"].ToString());
                cmd.Parameters.AddWithValue("@Customer", HttpContext.Current.Session["CustomerID"].ToString());

         



                cmd.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(ds);

                */

                ds = ObjReport.Data_ICEXContract_Reportdos(HttpContext.Current.Session["LastCompany"].ToString(), HttpContext.Current.Session["usersegid"].ToString(),
                                                      "MCXSX - CDX", date.ToString(), HttpContext.Current.Session["Customer"].ToString(), "", "", HttpContext.Current.Session["mode"].ToString(),
                                                      HttpContext.Current.Session["ExchangeSegmentID"].ToString(), "", "",
                                                        HttpContext.Current.Session["LastSettNo"].ToString().Substring(0, 7), HttpContext.Current.Session["LastSettNo"].ToString().Substring(7, 1),
                                                        HttpContext.Current.Session["BranchID"].ToString(), HttpContext.Current.Session["CustomerID"].ToString());


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