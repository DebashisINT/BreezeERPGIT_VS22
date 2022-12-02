using System;
using System.Data;
using System.IO;
using System.Web;
using LibDosPrint;
using System.Configuration;
using BusinessLogicLayer;


namespace ERP.OMS.Reports
{

    public partial class Reports_Defaultnew : System.Web.UI.Page
    {
        Converter objConverter = new Converter();
        DBEngine oDBEngine = new DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);
        string str = "";
        string date = "";
        string mode = "";
        string[] arr = null;
        DataSet ds = new DataSet();
        string sXMLFileName, sOutputFileName, sData;
        protected void Page_Load(object sender, EventArgs e)
        {
            string[] sArQuery = new string[1];
            string sQuery = "";
            string virtualpath = "";
            if (Request.QueryString["path"].ToString() != "")
                virtualpath = Server.MapPath("ReportOutput") + Request.QueryString["path"].ToString();
            sArQuery[0] = sQuery;

            if (Request.QueryString["reportformat"].ToString() == "")
                sXMLFileName = Server.MapPath("ReportFormat") + "\\ContractNote_" + HttpContext.Current.Session["usersegid"].ToString() + ".xml";
            else
                sXMLFileName = Server.MapPath("ReportFormat") + "\\ContractNote_Combined" + HttpContext.Current.Session["LastCompany"].ToString() + ".xml";
            sOutputFileName = virtualpath;


            ds = (DataSet)Session["a1"];
            Session["a1"] = null;
            try
            {
                if (ds.Tables[0].Rows.Count > 0)
                {

                    StreamReader fp;
                    fp = File.OpenText(sXMLFileName);
                    sData = fp.ReadToEnd();
                    fp.Close();
                    DosPrint objPrint = new DosPrint(sData, sArQuery);
                    objPrint.PrintMainReport(sOutputFileName, ds); //'' out put file
                    objPrint = null;
                    sData = null;
                    fp = File.OpenText(sOutputFileName);
                    sData = fp.ReadToEnd();
                    fp.Close();
                    Response.Write(sData);
                }
            }
            catch (Exception ex)
            {
            }

        }


    }
}