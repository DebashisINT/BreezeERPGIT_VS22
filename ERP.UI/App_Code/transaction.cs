using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.ReportSource;
using System.Data.SqlClient;
using System.Runtime.Serialization;
using BusinessLogicLayer;

/// <summary>
/// Summary description for transaction
/// </summary>
public class transaction
{
	public transaction()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    //test
    string dp = "CDSL";
    private static DataTable DT = new DataTable();
    String dpId, SegmentId, billnoFinYear, financilaYear;
    static string isinid = "", SettlementID = "";
    static int counter = 0, pageindex = 0, totolRecord = 0;
    PagedDataSource pds = new PagedDataSource();
    private int Repcounter = 0;
    Converter objConverter = new Converter();
    //DBEngine oDBEngine = new DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]); MULTI
    DBEngine oDBEngine = new DBEngine();
    private string companyId()
    {
        string[] yearSplit;

        financilaYear = HttpContext.Current.Session["LastFinYear"].ToString(); //HttpContext.Current.Session["LastFinYear"].ToString();




        yearSplit = financilaYear.Split('-');

        billnoFinYear = "-" + yearSplit[0].Substring(2) + yearSplit[1].Substring(2).Trim() + "-";

        DataTable lastSegMemt = oDBEngine.GetDataTable("(select exch_compId,exch_internalId,exch_TMCode," +
                                                " isnull((select exh_shortName from tbl_master_Exchange where exh_cntId=tbl_master_companyExchange.exch_exchId)+'-'+exch_segmentId,exch_membershiptype) as Segment from tbl_master_companyExchange where exch_compid in " +
                                                    " (select top 1 ls_lastCompany from tbl_trans_Lastsegment where ls_lastSegment=" + HttpContext.Current.Session["userlastsegment"] + ")) as D ", "*", "Segment in(select seg_name from tbl_master_segment where seg_id=" + HttpContext.Current.Session["userlastsegment"] + ")");

        //companyId = lastSegMemt.Rows[0][0].ToString();
        //dpId = lastSegMemt.Rows[0][2].ToString();
        //SegmentId = lastSegMemt.Rows[0][1].ToString();

        return lastSegMemt.Rows[0][0].ToString();
    }

    public void showCrystalReport()
    {

        string select, where;
        //string stdate, endDate;
        int logoStatus;

        DataSet transDs = new DataSet();
        DataSet holdingDs = new DataSet();
        DataSet logo = new DataSet();

        DataRow drow;
        byte[] logoinByte;

       
        string cmpid = HttpContext.Current.Session["userid"].ToString();
        string startdate = HttpContext.Current.Session["fromdate"] + " 00:00:00";
        string enddt = HttpContext.Current.Session["todate"] + " 23:59:00";
        logoStatus = 1;

        logo.Tables.Add();
        logo.Tables[0].Columns.Add("Image", System.Type.GetType("System.Byte[]"));
        drow = logo.Tables[0].NewRow();


        if (objConverter.getLogoImage(HttpContext.Current.Server.MapPath(@"..\images\logo.jpg"), out logoinByte) == 1)
        {
            drow["Image"] = logoinByte;

        }
        else
        {
            logoStatus = 0;
            

        }

        logo.Tables[0].Rows.Add(drow);


        if (logoStatus == 1)
        {

            //using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]))  MULTI
            using (SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"])))
            {
                SqlCommand cmd = new SqlCommand("cdslTransctionShowwithDematandPledge", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@stdate", startdate);
                cmd.Parameters.AddWithValue("@eddate", enddt);
                cmd.Parameters.AddWithValue("@compID", companyId());
                cmd.Parameters.AddWithValue("@dp", dp);


                if (cmpid != "")
                {

                    cmd.Parameters.AddWithValue("@BoID", cmpid);

                }
                else
                {
                    cmd.Parameters.AddWithValue("@BoID", "na");
                }

                if (isinid != "")
                {

                    cmd.Parameters.AddWithValue("@isin", isinid);

                }
                else
                {
                    cmd.Parameters.AddWithValue("@isin", "na");
                }

                if (SettlementID != "")
                {

                    cmd.Parameters.AddWithValue("@SettlementID", SettlementID);

                }
                else
                {
                    cmd.Parameters.AddWithValue("@SettlementID", "na");
                }

                
                    cmd.Parameters.AddWithValue("@boStatus", "na");              




                cmd.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(transDs);


                SqlCommand cmdHolding = new SqlCommand("cdslTransctionHolding", con);
                cmdHolding.CommandType = CommandType.StoredProcedure;
                //cmdHolding.Parameters.AddWithValue("@stdate", txtendDate.Text + " 00:00:00");
                //cmdHolding.Parameters.AddWithValue("@eddate", txtendDate.Text + " 23:59:00");
                cmdHolding.Parameters.AddWithValue("@stdate", startdate);
                cmdHolding.Parameters.AddWithValue("@eddate", enddt);

                if (cmpid != "")
                {

                    cmdHolding.Parameters.AddWithValue("@boid", cmpid);

                }
                else
                {
                    cmdHolding.Parameters.AddWithValue("@boid", "na");
                }

                cmdHolding.CommandTimeout = 0;
                SqlDataAdapter daHolding = new SqlDataAdapter();
                daHolding.SelectCommand = cmdHolding;


                daHolding.Fill(holdingDs);



            }


            for (int k = 0; k < transDs.Tables[0].Rows.Count; k++)
            {

                if (k > 0)
                {
                    if (transDs.Tables[0].Rows[k - 1]["CDSLISIN_Number"].ToString() == transDs.Tables[0].Rows[k]["CDSLISIN_Number"].ToString() && transDs.Tables[0].Rows[k - 1]["CdslTransaction_SettlementID"].ToString() == transDs.Tables[0].Rows[k]["CdslTransaction_SettlementID"].ToString()
                                                 && transDs.Tables[0].Rows[k - 1]["transactionType"].ToString() == transDs.Tables[0].Rows[k]["transactionType"].ToString())
                    {
                        transDs.Tables[0].Rows[k]["CdslTransaction_Quantity"] = Convert.ToString(Convert.ToDecimal(transDs.Tables[0].Rows[k - 1]["CdslTransaction_Quantity"].ToString()) + Convert.ToDecimal(transDs.Tables[0].Rows[k]["credit"].ToString()) - Convert.ToDecimal(transDs.Tables[0].Rows[k]["debit"].ToString()));
                    }
                    else
                    {
                        transDs.Tables[0].Rows[k]["CdslTransaction_Quantity"] = Convert.ToString(Convert.ToDecimal(transDs.Tables[0].Rows[k]["openingbalance"].ToString()) + Convert.ToDecimal(transDs.Tables[0].Rows[k]["credit"].ToString()) - Convert.ToDecimal(transDs.Tables[0].Rows[k]["debit"].ToString()));
                    }
                }
                else
                {
                    transDs.Tables[0].Rows[0]["CdslTransaction_Quantity"] = Convert.ToString(Convert.ToDecimal(transDs.Tables[0].Rows[0]["openingbalance"].ToString()) + Convert.ToDecimal(transDs.Tables[0].Rows[0]["credit"].ToString()) - Convert.ToDecimal(transDs.Tables[0].Rows[0]["debit"].ToString()));
                }

            }

            for (int k = 0; k < transDs.Tables[0].Rows.Count; k++)
            {
                if (transDs.Tables[0].Rows[k]["credit"].ToString() != "0.000")
                    transDs.Tables[0].Rows[k]["credit"] = objConverter.getFormattedvalueWithCheckingDecimalPlaceOriginalSign(Convert.ToDecimal(transDs.Tables[0].Rows[k]["credit"].ToString()));
                else
                    transDs.Tables[0].Rows[k]["credit"] = DBNull.Value;

                if (transDs.Tables[0].Rows[k]["debit"].ToString() != "0.000")
                    transDs.Tables[0].Rows[k]["debit"] = objConverter.getFormattedvalueWithCheckingDecimalPlaceOriginalSign(Convert.ToDecimal(transDs.Tables[0].Rows[k]["debit"].ToString()));
                else
                    transDs.Tables[0].Rows[k]["debit"] = DBNull.Value;

                transDs.Tables[0].Rows[k]["CdslTransaction_Quantity"] = objConverter.getFormattedvalueWithCheckingDecimalPlaceOriginalSign(Convert.ToDecimal(transDs.Tables[0].Rows[k]["CdslTransaction_Quantity"].ToString()));
                transDs.Tables[0].Rows[k]["openingbalance"] = objConverter.getFormattedvalueWithCheckingDecimalPlaceOriginalSign(Convert.ToDecimal(transDs.Tables[0].Rows[k]["openingbalance"].ToString()));
                transDs.Tables[0].Rows[k]["closingbalance"] = objConverter.getFormattedvalueWithCheckingDecimalPlaceOriginalSign(Convert.ToDecimal(transDs.Tables[0].Rows[k]["closingbalance"].ToString()));
            }


            if (transDs.Tables[0].Rows.Count > 0)
            {


                // transDs.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"]+"\\Reports\\cdsltransction.xsd");
                //  transDs.WriteXml(ConfigurationManager.AppSettings["SaveCSVsql"]+"\\Reports\\cdsltransction.xml");
                DataTable tmp = new DataTable();

                tmp = holdingDs.Tables[0].Copy();
                holdingDs.Tables[0].Reset();

                holdingDs.Tables[0].Columns.Add("holdingDate", Type.GetType("System.String"));
                holdingDs.Tables[0].Columns.Add("Holdingboid", Type.GetType("System.String"));
                holdingDs.Tables[0].Columns.Add("CdslHolding_ISIN", Type.GetType("System.String"));
                holdingDs.Tables[0].Columns.Add("CDSLISIN_ShortName", Type.GetType("System.String"));
                holdingDs.Tables[0].Columns.Add("CdslHolding_SettlementID", Type.GetType("System.String"));


                holdingDs.Tables[0].Columns.Add("CdslHolding_CurrentBalance", Type.GetType("System.String"));
                holdingDs.Tables[0].Columns.Add("CdslHolding_FreeBalance", Type.GetType("System.String"));
                holdingDs.Tables[0].Columns.Add("CdslHolding_PledgeBalance", Type.GetType("System.String"));
                holdingDs.Tables[0].Columns.Add("CdslHolding_EarmarkedBalance", Type.GetType("System.String"));
                holdingDs.Tables[0].Columns.Add("CdslHolding_PendingRematBalance", Type.GetType("System.String"));
                holdingDs.Tables[0].Columns.Add("CdslHolding_PendingDematBalance", Type.GetType("System.String"));




                if (tmp.Rows.Count == 0)
                {
                    DataRow dataRow = holdingDs.Tables[0].NewRow();
                    dataRow[0] = "00000";
                    dataRow[2] = "00000";
                    dataRow[3] = "00000";
                    dataRow[4] = "00000";
                    dataRow[5] = "00000";
                    dataRow[6] = "00000";
                    dataRow[7] = "00000";
                    dataRow[8] = "00000";
                    dataRow[9] = "00000";
                    holdingDs.Tables[0].Rows.Add(dataRow);

                }
                else
                {
                    for (int j = 0; j < tmp.Rows.Count; j++)
                    {
                        DataRow dataRow = holdingDs.Tables[0].NewRow();

                        dataRow["holdingDate"] = tmp.Rows[j]["holdingDate"];
                        dataRow["Holdingboid"] = tmp.Rows[j]["Holdingboid"];
                        dataRow["CdslHolding_ISIN"] = tmp.Rows[j]["CdslHolding_ISIN"];
                        dataRow["CDSLISIN_ShortName"] = tmp.Rows[j]["CDSLISIN_ShortName"];
                        dataRow["CdslHolding_SettlementID"] = tmp.Rows[j]["CdslHolding_SettlementID"];

                        if (tmp.Rows[j]["CdslHolding_CurrentBalance"].ToString() != "0.000")
                            dataRow["CdslHolding_CurrentBalance"] = objConverter.getFormattedvalueWithCheckingDecimalPlaceOriginalSign(Convert.ToDecimal(tmp.Rows[j]["CdslHolding_CurrentBalance"].ToString()));
                        else
                            dataRow["CdslHolding_CurrentBalance"] = DBNull.Value;

                        if (tmp.Rows[j]["CdslHolding_FreeBalance"].ToString() != "0.000")
                            dataRow["CdslHolding_FreeBalance"] = objConverter.getFormattedvalueWithCheckingDecimalPlaceOriginalSign(Convert.ToDecimal(tmp.Rows[j]["CdslHolding_FreeBalance"].ToString()));
                        else
                            dataRow["CdslHolding_FreeBalance"] = DBNull.Value;

                        if (tmp.Rows[j]["CdslHolding_PledgeBalance"].ToString() != "0.000")
                            dataRow["CdslHolding_PledgeBalance"] = objConverter.getFormattedvalueWithCheckingDecimalPlaceOriginalSign(Convert.ToDecimal(tmp.Rows[j]["CdslHolding_PledgeBalance"].ToString()));
                        else
                            dataRow["CdslHolding_PledgeBalance"] = DBNull.Value;

                        if (tmp.Rows[j]["CdslHolding_EarmarkedBalance"].ToString() != "0.000")
                            dataRow["CdslHolding_EarmarkedBalance"] = objConverter.getFormattedvalueWithCheckingDecimalPlaceOriginalSign(Convert.ToDecimal(tmp.Rows[j]["CdslHolding_EarmarkedBalance"].ToString()));
                        else
                            dataRow["CdslHolding_EarmarkedBalance"] = DBNull.Value;

                        if (tmp.Rows[j]["CdslHolding_PendingRematBalance"].ToString() != "0.000")
                            dataRow["CdslHolding_PendingRematBalance"] = objConverter.getFormattedvalueWithCheckingDecimalPlaceOriginalSign(Convert.ToDecimal(tmp.Rows[j]["CdslHolding_PendingRematBalance"].ToString()));
                        else
                            dataRow["CdslHolding_PendingRematBalance"] = DBNull.Value;

                        if (tmp.Rows[j]["CdslHolding_PendingDematBalance"].ToString() != "0.000")
                            dataRow["CdslHolding_PendingDematBalance"] = objConverter.getFormattedvalueWithCheckingDecimalPlaceOriginalSign(Convert.ToDecimal(tmp.Rows[j]["CdslHolding_PendingDematBalance"].ToString()));
                        else
                            dataRow["CdslHolding_PendingDematBalance"] = DBNull.Value;

                        holdingDs.Tables[0].Rows.Add(dataRow);

                    }
                }
                holdingDs.AcceptChanges();
                //holdingDs.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\cdslholding.xsd");
                // holdingDs.WriteXml(ConfigurationManager.AppSettings["SaveCSVsql"]+"\\Reports\\cdslholding.xml");




                ReportDocument cdslTransctionReportDocu = new ReportDocument();

                //string[] connPath = ConfigurationSettings.AppSettings["DBConnectionDefault"].Split(';');  MULTI
                string[] connPath = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]).Split(';');






                string path = HttpContext.Current.Server.MapPath("..\\Reports\\cdsltrans_holding.rpt");
                cdslTransctionReportDocu.Load(path);

                cdslTransctionReportDocu.SetDatabaseLogon(connPath[2].Substring(connPath[2].IndexOf("=")).Trim(), connPath[3].Substring(connPath[3].IndexOf("=")).Trim(), connPath[0].Substring(connPath[0].IndexOf("=")).Trim(), connPath[1].Substring(connPath[1].IndexOf("=")).Trim(), false);
                cdslTransctionReportDocu.SetDataSource(transDs);

                cdslTransctionReportDocu.Subreports["logo"].SetDataSource(logo);
                cdslTransctionReportDocu.Subreports["holding"].SetDataSource(holdingDs);








                HttpContext.Current.Session["fromdate"] = HttpContext.Current.Session["fromdate"].ToString().Split('/')[1] + "/" + HttpContext.Current.Session["fromdate"].ToString().Split('/')[0] + "/" + HttpContext.Current.Session["fromdate"].ToString().Split('/')[2];
                HttpContext.Current.Session["todate"] = HttpContext.Current.Session["todate"].ToString().Split('/')[1] + "/" + HttpContext.Current.Session["todate"].ToString().Split('/')[0] + "/" + HttpContext.Current.Session["todate"].ToString().Split('/')[2];
                cdslTransctionReportDocu.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, HttpContext.Current.Server.MapPath("../Documents") + "\\" + transDs.Tables[0].Rows[0][9].ToString().Trim() + "_" + HttpContext.Current.Session["fromdate"].ToString().Trim().Replace('/', '-') + "_" + HttpContext.Current.Session["todate"].ToString().Trim().Replace('/', '-') + ".pdf");


                HttpContext.Current.Session["mailpath"] = HttpContext.Current.Session["mailpath"].ToString() + HttpContext.Current.Server.MapPath("../Documents") + "\\" + transDs.Tables[0].Rows[0][9].ToString().Trim() + "_" + HttpContext.Current.Session["fromdate"].ToString().Trim().Replace('/', '-') + "_" + HttpContext.Current.Session["todate"].ToString().Trim().Replace('/', '-') + ".pdf" + "~";







                transDs.Clear();
                transDs.Dispose();
                holdingDs.Clear();
                holdingDs.Dispose();


            }
            else
            {
                
            }

        }

    }
}
