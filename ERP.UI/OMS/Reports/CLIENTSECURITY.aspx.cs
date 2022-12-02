using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using BusinessLogicLayer;

namespace ERP.OMS.Reports
{
    public partial class Reports_CLIENTSECURITY : System.Web.UI.Page, System.Web.UI.ICallbackEventHandler
    {
        BusinessLogicLayer.Converter oconverter = new BusinessLogicLayer.Converter();
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        DataSet ds = new DataSet();
        ExcelFile objExcel = new ExcelFile();
        DataTable dtExport = new DataTable();
        string data;
        int pageindex = 0;

        BusinessLogicLayer.Reports ObjReports = new BusinessLogicLayer.Reports();

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
                date();
            }
            //_____For performing operation without refreshing page___//
            String cbReference = Page.ClientScript.GetCallbackEventReference(this, "arg", "ReceiveServerData", "context");
            String callbackScript = "function CallServer(arg, context){ " + cbReference + ";}";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "CallServer", callbackScript, true);
            //___________-end here___//

        }
        void date()
        {
            dtfrom.EditFormatString = oconverter.GetDateFormat("Date");
            dtto.EditFormatString = oconverter.GetDateFormat("Date");
            dtfrom.Value = Convert.ToDateTime(oDBEngine.GetDate().ToShortDateString());
            dtto.Value = Convert.ToDateTime(oDBEngine.GetDate().ToShortDateString());

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

        }

        void procedure()
        {

            /* For Tier Structure ----------------

            using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]))
            {

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = "[CLIENTSECURITY]";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@FINYEAR", HttpContext.Current.Session["LastFinYear"]);
                cmd.Parameters.AddWithValue("@FROMDATE", dtfrom.Value);
                cmd.Parameters.AddWithValue("@TODATE", dtto.Value);
                cmd.Parameters.AddWithValue("@SEGMENT", Convert.ToInt32(Session["usersegid"].ToString()));
                cmd.Parameters.AddWithValue("@COMPANYID", Session["LastCompany"].ToString());
                if (rdbClientsAll.Checked)
                {
                    cmd.Parameters.AddWithValue("@CLIENTS", "ALL");
                }
                else
                {
                    cmd.Parameters.AddWithValue("@CLIENTS", HiddenField_Client.Value);
                }
                cmd.Parameters.AddWithValue("@BRANCHID", Session["userbranchHierarchy"].ToString());
         
           

                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                cmd.CommandTimeout = 0;
                ds.Reset();
                da.Fill(ds);
                da.Dispose();

          

                ViewState["dataset"] = ds;
            }

            */

            string vCLIENTS = "";
            if (rdbClientsAll.Checked)
            {
                vCLIENTS = "ALL";
            }
            else
            {
                vCLIENTS = HiddenField_Client.Value;
            }

            ds = ObjReports.Get_CLIENTSECURITY(HttpContext.Current.Session["LastFinYear"].ToString(), dtfrom.Value.ToString(), dtto.Value.ToString(),
                                                   Session["usersegid"].ToString(), Session["LastCompany"].ToString(), vCLIENTS,
                                                   Session["userbranchHierarchy"].ToString());

            ViewState["dataset"] = ds;

        }

        protected void btnexcel_Click(object sender, EventArgs e)
        {
            procedure();
            otherexport();
        }
        void ddlbandforclient()
        {
            ds = (DataSet)ViewState["dataset"];
            DataView viewgroup = new DataView(ds.Tables[0]);
            DataTable dtgroupcontactid = viewgroup.ToTable(true, new string[] { "CUSTOMERID", "CLIENTNAME" });
            if (dtgroupcontactid.Rows.Count > 0)
            {
                cmbgroup.DataSource = dtgroupcontactid;
                cmbgroup.DataValueField = "CUSTOMERID";
                cmbgroup.DataTextField = "CLIENTNAME";
                cmbgroup.DataBind();

            }

        }
        void otherexport()
        {
            ds = (DataSet)ViewState["dataset"];
            DataTable dt = new DataTable();
            dt = (DataTable)ds.Tables[0];
            string str = "Register of Security Report Period :" + oconverter.ArrangeDate2(dtfrom.Value.ToString()) + " - " + oconverter.ArrangeDate2(dtto.Value.ToString());
            oconverter.ExportToExcel(dt, str);

        }
        void export()
        {
            ds = (DataSet)ViewState["dataset"];
            DataTable dtExport = new DataTable();
            dtExport.Clear();
            dtExport.Columns.Add("Tran.Date", Type.GetType("System.String"));
            dtExport.Columns.Add("Purpose", Type.GetType("System.String"));
            dtExport.Columns.Add("Received From", Type.GetType("System.String"));
            dtExport.Columns.Add("Sett. From", Type.GetType("System.String"));
            dtExport.Columns.Add("Delivered To", Type.GetType("System.String"));
            dtExport.Columns.Add("Sett. To", Type.GetType("System.String"));
            dtExport.Columns.Add("ISIN", Type.GetType("System.String"));
            dtExport.Columns.Add("Remarks", Type.GetType("System.String"));
            dtExport.Columns.Add("Qty Recvd.[To Dlvr]", Type.GetType("System.String"));
            dtExport.Columns.Add("Qty Dlvrd.[To Recv]", Type.GetType("System.String"));
            dtExport.Columns.Add("Balance", Type.GetType("System.String"));

            ddlbandforclient();
            for (int j = 0; j < cmbgroup.Items.Count; j++)
            {
                DataRow row = dtExport.NewRow();
                row[0] = "Client Name:" + cmbgroup.Items[j].Text.ToString().Trim();
                row[1] = "Test";
                dtExport.Rows.Add(row);

                DataView viewgrp = new DataView();
                viewgrp = ds.Tables[0].DefaultView;
                viewgrp.RowFilter = "CUSTOMERID='" + cmbgroup.Items[j].Value + "'";
                DataTable dt = new DataTable();
                dt = viewgrp.ToTable();

                DataView viewSCRIP = new DataView(dt);
                DataTable DistinctSCRIP = new DataTable();
                DistinctSCRIP = viewSCRIP.ToTable(true, new string[] { "SYMBOLNAME", "PRODUCTID" });

                if (DistinctSCRIP.Rows.Count > 0)
                {
                    cmbscrip.Items.Clear();
                    cmbscrip.DataSource = DistinctSCRIP;
                    cmbscrip.DataValueField = "PRODUCTID";
                    cmbscrip.DataTextField = "SYMBOLNAME";
                    cmbscrip.DataBind();

                }
                for (int k = 0; k < cmbscrip.Items.Count; k++)
                {
                    DataRow row1 = dtExport.NewRow();
                    row1[0] = "Scrip Name:" + cmbscrip.Items[k].Text.ToString().Trim();
                    row1[1] = "Test";
                    dtExport.Rows.Add(row1);

                    DataView viewSCRIP1 = new DataView();
                    viewSCRIP1 = ds.Tables[0].DefaultView;
                    viewSCRIP1.RowFilter = "PRODUCTID='" + cmbscrip.Items[k].Value + "'";
                    DataTable dt1 = new DataTable();
                    dt1 = viewSCRIP1.ToTable();

                    for (int i = 0; i < dt1.Rows.Count; i++)
                    {
                        DataRow row2 = dtExport.NewRow();
                        row2[0] = dt1.Rows[i]["TRADEDATE1"].ToString();
                        row2[1] = dt1.Rows[i]["PURPOSE"].ToString();
                        row2[2] = dt1.Rows[i]["RECEIVEDFROM"].ToString();
                        row2[3] = dt1.Rows[i]["SETTFROM"].ToString();
                        row2[4] = dt1.Rows[i]["DELIVEREDTO"].ToString();
                        row2[5] = dt1.Rows[i]["SETTTO"].ToString();
                        row2[6] = dt1.Rows[i]["ISINNO"].ToString();
                        row2[7] = dt1.Rows[i]["REMARKS"].ToString();
                        if (dt1.Rows[i]["QTYIN"] != DBNull.Value)
                            row2[8] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["QTYIN"].ToString()));
                        if (dt1.Rows[i]["QTYOUT"] != DBNull.Value)
                            row2[9] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["QTYOUT"].ToString()));
                        if (dt1.Rows[i]["RUNNINGBALN"] != DBNull.Value)
                            row2[10] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["RUNNINGBALN"].ToString()));
                        dtExport.Rows.Add(row2);

                    }
                }
            }




            DataTable dtReportHeader = new DataTable();
            DataTable dtReportFooter = new DataTable();

            DataTable CompanyName = oDBEngine.GetDataTable("tbl_master_company", "cmp_Name", " cmp_internalId='" + Session["LastCompany"].ToString() + "'");
            dtReportHeader.Columns.Add(new DataColumn("Header", typeof(String))); //0
            DataRow HeaderRow = dtReportHeader.NewRow();
            HeaderRow[0] = CompanyName.Rows[0][0].ToString();
            dtReportHeader.Rows.Add(HeaderRow);
            DataRow DrRowR1 = dtReportHeader.NewRow();

            string str = oconverter.ArrangeDate2(dtfrom.Value.ToString()) + " - " + oconverter.ArrangeDate2(dtto.Value.ToString());

            DrRowR1[0] = "Register of Security Report Period" + str;

            dtReportHeader.Rows.Add(DrRowR1);
            DataRow DrRowR2 = dtReportHeader.NewRow();
            dtReportHeader.Rows.Add(DrRowR2);
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

            objExcel.ExportToExcelforExcel(dtExport, "Register of Security", "Total", dtReportHeader, dtReportFooter);


        }
    }
}