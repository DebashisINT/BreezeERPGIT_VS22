using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using CrystalDecisions.CrystalReports.Engine;
using DevExpress.Web;
//using DevExpress.Web.ASPxEditors;
//using DevExpress.Web;
using BusinessLogicLayer;

namespace ERP.OMS.Reports
{
    public partial class Reports_changedpacc_cheque : System.Web.UI.Page
    {

        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);
        DataTable dtbank = new DataTable();
        AspxHelper oAspxHelper = new AspxHelper();
        BusinessLogicLayer.Converter oconverter = new BusinessLogicLayer.Converter();
        string data;
        string WhichCall;
        string bankid = "";
        string Rowid1 = "";
        string Account = "";
        string ClientUcc = "";
        string date = "";
        string[] splitsession = new string[11];
        string SelectedIds = null;
        string DeSelectedIds = null;

        BusinessLogicLayer.Reports ObjReports = new BusinessLogicLayer.Reports();

        public DataTable Dtgridbindforstage2
        {
            get { return (DataTable)Session["DtChequePrintgrid"]; }
            set { Session["DtChequePrintgrid"] = value; }
        }
        public DataTable Dtgridbindforstage3
        {
            get { return (DataTable)Session["DtChequePrintgrid3"]; }
            set { Session["DtChequePrintgrid3"] = value; }
        }
        public DataTable DtChequePrintpdf
        {
            get { return (DataTable)Session["DtChequePrintpdf"]; }
            set { Session["DtChequePrintpdf"] = value; }
        }
        public string P_stepid
        {
            get { return (string)Session["stepid"]; }
            set { Session["stepid"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //bankid = Request.QueryString["bankid"].ToString().Split('~')[0];
            Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script language='javascript'>Page_Load()</script>");
            if (Session["chequedate"] != null)
            {
                splitsession = Session["chequedate"].ToString().Split('~');

            }
            if (!IsPostBack)
            {
                Session["doneupdate"] = null;
                bankid = splitsession[6].ToString();
                Dtgridbindforstage2 = null;
                P_stepid = "2";

                Account = splitsession[0].ToString();

                ClientUcc = splitsession[1].ToString();

                date = splitsession[2].ToString();
                string Rowid = splitsession[5].ToString();
                string cbid = "";
                string cbidstatic = "";
                string cbdetailid = "";
                string cbdetailidstatic = "";
                string customertype = splitsession[9].ToString();
                string fromdate = splitsession[7].ToString();
                string todate = splitsession[8].ToString();
                DtChequePrintpdf = (DataTable)Session["DtChequePrintSession1"];
                bool countcol = false;
                string date1 = splitsession[2].ToString().Split('-')[0];
                string date2 = splitsession[2].ToString().Split('-')[1];
                string date3 = splitsession[2].ToString().Split('-')[2];
                int chqno = Convert.ToInt32(splitsession[4].ToString());
                if (date2.Length == 1)
                {
                    date2 = "0" + date2;
                }
                if (date3.Length == 1)
                {
                    date3 = "0" + date3;
                }
                //string totaldate = date1 + "-" + date2 + "-" + date3;
                string totaldate = date3 + "-" + date2 + "-" + date1;
                totaldate = oconverter.ArrangeDate2(totaldate);
                if (Rowid != "n" || Rowid != "undefined" || Rowid != null)
                {

                    cbdetailidstatic = Rowid.ToString();
                }
                else
                {
                    cbdetailidstatic = "00000";
                }
                DataTable Dtgridbindforstage23 = new DataTable();
                if (customertype == "C")
                {
                    Dtgridbindforstage23.Columns.Add("accountname", System.Type.GetType("System.String"));
                    Dtgridbindforstage23.Columns.Add("cbd_id", System.Type.GetType("System.String"));
                    Dtgridbindforstage23.Columns.Add("cbd_bankCode", System.Type.GetType("System.String"));
                    Dtgridbindforstage23.Columns.Add("CashBankDetail_ID", System.Type.GetType("System.String"));
                    Dtgridbindforstage23.Columns.Add("MainAccount_Name", System.Type.GetType("System.String"));
                    Dtgridbindforstage23.Columns.Add("cashbank_vouchernumber", System.Type.GetType("System.String"));
                    Dtgridbindforstage23.Columns.Add("CashBank_TransactionDate", System.Type.GetType("System.String"));
                    Dtgridbindforstage23.Columns.Add("Payment", System.Type.GetType("System.String"));
                    Dtgridbindforstage23.Columns.Add("CashBankDetail_Subaccountid", System.Type.GetType("System.String"));
                    Dtgridbindforstage23.Columns.Add("cashbankdetail_mainaccountid", System.Type.GetType("System.String"));
                    Dtgridbindforstage23.Columns.Add("cnt_branchID", System.Type.GetType("System.String"));
                    Dtgridbindforstage23.Columns.Add("BranchDescription", System.Type.GetType("System.String"));
                    Dtgridbindforstage23.Columns.Add("CashBankDetail_InstrumentNumber", System.Type.GetType("System.String"));
                    Dtgridbindforstage23.Columns.Add("CashBankDetail_InstrumentDate", System.Type.GetType("System.String"));
                    Dtgridbindforstage23.Columns.Add("ClientBankName", System.Type.GetType("System.String"));
                }
                else
                {

                    Dtgridbindforstage23.Columns.Add("CashBankDetail_ID", System.Type.GetType("System.String"));
                    Dtgridbindforstage23.Columns.Add("MainAccount_Name", System.Type.GetType("System.String"));
                    Dtgridbindforstage23.Columns.Add("cashbank_vouchernumber", System.Type.GetType("System.String"));
                    Dtgridbindforstage23.Columns.Add("CashBank_TransactionDate", System.Type.GetType("System.String"));
                    Dtgridbindforstage23.Columns.Add("Payment", System.Type.GetType("System.String"));
                    Dtgridbindforstage23.Columns.Add("CashBankDetail_Subaccountid", System.Type.GetType("System.String"));
                    Dtgridbindforstage23.Columns.Add("cashbankdetail_mainaccountid", System.Type.GetType("System.String"));

                    Dtgridbindforstage23.Columns.Add("CashBankDetail_InstrumentNumber", System.Type.GetType("System.String"));
                    Dtgridbindforstage23.Columns.Add("CashBankDetail_InstrumentDate", System.Type.GetType("System.String"));

                }

                string[] arr = Rowid.Split(',');
                foreach (string S in arr)
                {
                    foreach (DataRow dr in DtChequePrintpdf.Rows)
                    {

                        if (S == dr["CashBankDetail_ID"].ToString())
                        {

                            Dtgridbindforstage23.Rows.Add(dr.ItemArray);
                            break;
                        }
                    }
                    Dtgridbindforstage23.AcceptChanges();
                }


                Dtgridbindforstage2 = Dtgridbindforstage23.Copy();

                if (Dtgridbindforstage2 != null)
                {
                    if (Dtgridbindforstage2.Rows.Count > 0)
                    {
                        gridpreview.Caption = "STEP 2 OF 3 [ Final Cheques To Print ]";
                        string strHeaderbankname = String.Empty;
                        DataTable dtbankname = oDBEngine.GetDataTable("Select top 1 MainAccount_AccountCode+'-'+MainAccount_Name+' [ '+MainAccount_BankAcNumber+' ]' from Master_MainAccount where MainAccount_AccountCode='" + bankid.ToString().Trim() + "'");
                        strHeaderbankname = dtbankname.Rows[0][0].ToString();
                        if (customertype == "C")
                        {
                            foreach (DataColumn col in Dtgridbindforstage2.Columns)
                            {
                                if (col.ColumnName == "CashBankDetail_InstrumentNumber")
                                {
                                    countcol = true;
                                }
                                if (col.ColumnName == "ClientBankName")
                                {
                                    countcol = true;
                                }
                                if (col.ColumnName == "CashBankDetail_InstrumentDate")
                                {
                                    countcol = true;
                                }
                            }

                            if (countcol == false)
                            {
                                Dtgridbindforstage2.Columns.Add("CashBankDetail_InstrumentNumber", System.Type.GetType("System.String"));
                                Dtgridbindforstage2.Columns.Add("CashBankDetail_InstrumentDate", System.Type.GetType("System.String"));
                                Dtgridbindforstage2.Columns.Add("ClientBankName", System.Type.GetType("System.String"));
                            }

                            for (int M = 0; M < Dtgridbindforstage2.Rows.Count; M++)
                            {
                                Dtgridbindforstage2.Rows[M]["CashBankDetail_InstrumentDate"] = totaldate;
                                Dtgridbindforstage2.Rows[M]["CashBankDetail_InstrumentNumber"] = chqno;
                                Dtgridbindforstage2.Rows[M]["ClientBankName"] = strHeaderbankname;

                                chqno = chqno + 1;
                            }
                        }
                        else
                        {
                            foreach (DataColumn col in Dtgridbindforstage2.Columns)
                            {
                                if (col.ColumnName == "CashBankDetail_InstrumentNumber")
                                {
                                    countcol = true;
                                }

                                if (col.ColumnName == "CashBankDetail_InstrumentDate")
                                {
                                    countcol = true;
                                }
                            }

                            if (countcol == false)
                            {
                                Dtgridbindforstage2.Columns.Add("CashBankDetail_InstrumentNumber", System.Type.GetType("System.String"));
                                Dtgridbindforstage2.Columns.Add("CashBankDetail_InstrumentDate", System.Type.GetType("System.String"));

                            }

                            for (int M = 0; M < Dtgridbindforstage2.Rows.Count; M++)
                            {
                                Dtgridbindforstage2.Rows[M]["CashBankDetail_InstrumentDate"] = totaldate;
                                Dtgridbindforstage2.Rows[M]["CashBankDetail_InstrumentNumber"] = chqno;


                                chqno = chqno + 1;
                            }
                        }
                        if (customertype != "C")
                        {
                            if (gridpreview.VisibleColumns.Count == 7)

                                gridpreview.Columns["accountname"].Visible = false;
                        }
                        else

                            gridpreview.Columns["accountname"].Visible = true;


                        oAspxHelper.BindGrid(Dtgridbindforstage2, gridpreview);
                    }
                    else
                    {
                        gridpreview.Caption = "No Record Found As On Your Criteria Please Go back & Change Your Selection Again";
                        oAspxHelper.BindGrid(gridpreview);

                        Page.ClientScript.RegisterStartupScript(GetType(), "JScriptf", "<script language='javascript'>visibilefalse()</script>");
                    }
                }
                else
                {
                    gridpreview.Caption = "No Record Found As On Your Criteria Please Go back & Change Your Selection Again";
                    oAspxHelper.BindGrid(gridpreview);

                    Page.ClientScript.RegisterStartupScript(GetType(), "JScriptv", "<script language='javascript'>visibilefalse()</script>");
                }
            }
            if (P_stepid == "2")
            {
                if (Dtgridbindforstage2.Rows.Count > 0)
                {
                    gridpreview.Caption = "STEP 2 OF 3 [ Final Cheques To Print ]";
                    oAspxHelper.BindGrid(Dtgridbindforstage2, gridpreview);
                }
                else
                {
                    gridpreview.Caption = "No Record Found As On Your Criteria Please Go back & Change Your Selection Again";
                    oAspxHelper.BindGrid(Dtgridbindforstage2, gridpreview);
                }
            }
            else if (P_stepid == "3")
            {
                gridassetfinal.Caption = "STEP 3 OF 3 [ Select Cheques To Update ]";
                oAspxHelper.BindGrid(Dtgridbindforstage2, gridassetfinal);
            }
            else
            {
                if (Dtgridbindforstage2 != null)
                {
                    oAspxHelper.BindGrid(Dtgridbindforstage2, gridassetfinal);
                }
            }
            Rowid1 = hdnfrequency.Value;
            gridassetfinal.JSProperties["cpproperties"] = "pdfindex";
            if (splitsession[9].ToString() != "C")
                Page.ClientScript.RegisterStartupScript(GetType(), "JScript123", "<script language='javascript'>hideshow()</script>");


        }

        protected void gridassetfinal_PageIndexChanging(object sender, EventArgs e)
        {
            P_stepid = "3";
            gridassetfinal.Caption = "STEP 3 OF 3 [ Select Cheques To Update ]";
            oAspxHelper.BindGrid(Dtgridbindforstage2, gridassetfinal);
            gridassetfinal.JSProperties["cpproperties"] = "pdfindex";
        }

        protected void gridassetfinal_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            P_stepid = "3";
            WhichCall = e.Parameters.Split('~')[0];
            if (WhichCall == "pdftoprint")
            {


                if (Dtgridbindforstage2 != null)
                {
                    gridassetfinal.Caption = "STEP 3 OF 3 [ Select Cheques To Update ]";
                    if (splitsession[9].ToString() != "C")
                    {
                        if (gridassetfinal.VisibleColumns.Count == 8)

                            gridassetfinal.Columns["accountname"].Visible = false;
                    }
                    else
                        gridassetfinal.Columns["accountname"].Visible = true;

                    oAspxHelper.BindGrid(Dtgridbindforstage2, gridassetfinal);
                    gridassetfinal.JSProperties["cpproperties"] = "pdf";
                }
                else
                {
                    gridassetfinal.JSProperties["cpproperties"] = "false";
                }




            }
            if (WhichCall == "Done")
            {

                int G = 0;
                int P = 0;

                DataTable DtChequePrintforupdate = new DataTable();
                if (Rowid1 != "")
                {
                    if (splitsession[9].ToString() == "C")
                    {
                        DtChequePrintforupdate.Columns.Add("accountname", System.Type.GetType("System.String"));
                        DtChequePrintforupdate.Columns.Add("cbd_id", System.Type.GetType("System.String"));
                        DtChequePrintforupdate.Columns.Add("cbd_bankCode", System.Type.GetType("System.String"));
                        DtChequePrintforupdate.Columns.Add("CashBankDetail_ID", System.Type.GetType("System.String"));
                        DtChequePrintforupdate.Columns.Add("MainAccount_Name", System.Type.GetType("System.String"));
                        DtChequePrintforupdate.Columns.Add("cashbank_vouchernumber", System.Type.GetType("System.String"));
                        DtChequePrintforupdate.Columns.Add("CashBank_TransactionDate", System.Type.GetType("System.String"));
                        DtChequePrintforupdate.Columns.Add("Payment", System.Type.GetType("System.String"));
                        DtChequePrintforupdate.Columns.Add("CashBankDetail_Subaccountid", System.Type.GetType("System.String"));
                        DtChequePrintforupdate.Columns.Add("cashbankdetail_mainaccountid", System.Type.GetType("System.String"));
                        DtChequePrintforupdate.Columns.Add("cnt_branchID", System.Type.GetType("System.String"));
                        DtChequePrintforupdate.Columns.Add("BranchDescription", System.Type.GetType("System.String"));
                        DtChequePrintforupdate.Columns.Add("CashBankDetail_InstrumentNumber", System.Type.GetType("System.String"));
                        DtChequePrintforupdate.Columns.Add("CashBankDetail_InstrumentDate", System.Type.GetType("System.String"));
                        DtChequePrintforupdate.Columns.Add("ClientBankName", System.Type.GetType("System.String"));
                    }
                    else
                    {

                        DtChequePrintforupdate.Columns.Add("CashBankDetail_ID", System.Type.GetType("System.String"));
                        DtChequePrintforupdate.Columns.Add("MainAccount_Name", System.Type.GetType("System.String"));
                        DtChequePrintforupdate.Columns.Add("cashbank_vouchernumber", System.Type.GetType("System.String"));
                        DtChequePrintforupdate.Columns.Add("CashBank_TransactionDate", System.Type.GetType("System.String"));
                        DtChequePrintforupdate.Columns.Add("Payment", System.Type.GetType("System.String"));
                        DtChequePrintforupdate.Columns.Add("CashBankDetail_Subaccountid", System.Type.GetType("System.String"));
                        DtChequePrintforupdate.Columns.Add("cashbankdetail_mainaccountid", System.Type.GetType("System.String"));

                        DtChequePrintforupdate.Columns.Add("CashBankDetail_InstrumentNumber", System.Type.GetType("System.String"));
                        DtChequePrintforupdate.Columns.Add("CashBankDetail_InstrumentDate", System.Type.GetType("System.String"));

                    }
                    int TotalRecord = gridassetfinal.VisibleRowCount;
                    for (int i = 0; i < TotalRecord; i++)
                    {
                        if (gridassetfinal.Selection.IsRowSelected(i))
                        {
                            SelectedIds = SelectedIds + gridassetfinal.GetRowValues(i, "CashBankDetail_ID").ToString() + ",";
                        }
                        DeSelectedIds = DeSelectedIds + gridassetfinal.GetRowValues(i, "CashBankDetail_ID").ToString() + ",";
                    }
                    SelectedIds = (SelectedIds == null) ? "0" : SelectedIds.Substring(0, SelectedIds.LastIndexOf(','));
                    DeSelectedIds = DeSelectedIds.Substring(0, DeSelectedIds.LastIndexOf(','));
                    //string[] arr = Rowid1.Split(',');
                    string[] arr = SelectedIds.Split(',');
                    foreach (string S in arr)
                    {
                        foreach (DataRow dr in Dtgridbindforstage2.Rows)
                        {

                            if (S == dr["CashBankDetail_ID"].ToString())
                            {

                                DtChequePrintforupdate.Rows.Add(dr.ItemArray);
                                break;
                            }
                        }
                        DtChequePrintforupdate.AcceptChanges();
                    }

                }
                DtChequePrintforupdate.TableName = "ChequePrint";
                DataSet dtxml = new DataSet();
                DataSet dts = new DataSet();
                if (DtChequePrintforupdate.Rows.Count > 0)
                {
                    dtxml.Tables.Add(DtChequePrintforupdate.Copy());

                    /* For Tier Structure----------
                    using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]))
                    {
                        if (con.State == ConnectionState.Open) con.Close();
                        using (SqlCommand com = new SqlCommand("Update_Cheque", con))
                        {
                            com.CommandType = CommandType.StoredProcedure;
                            com.Parameters.AddWithValue("@Doc", dtxml.GetXml());
                            com.Parameters.AddWithValue("@UserID", Session["userid"].ToString());
                            com.Parameters.AddWithValue("@UsersegID", Session["usersegid"].ToString());
                            com.Parameters.AddWithValue("@Company", Session["Lastcompany"].ToString());
                            com.Parameters.AddWithValue("@customertype", splitsession[9].ToString().Trim());

                            con.Open();
                            com.CommandTimeout = 0;
                            com.ExecuteNonQuery();
                            con.Close();
                            Session["chequedate"] = null;
                            Session["doneupdate"] = "update";
                        }
                    }

                    */

                    //---------------------------------



                    ObjReports.Update_Cheque(dtxml.GetXml(), Session["userid"].ToString(), Session["usersegid"].ToString(),
                                             Session["Lastcompany"].ToString(), splitsession[9].ToString().Trim());

                    Session["chequedate"] = null;
                    Session["doneupdate"] = "update";


                    //--------------------------------------------

                }


                gridassetfinal.JSProperties["cpproperties"] = "update";


            }
            if (WhichCall == "s3")
            {

                gridassetfinal.Settings.ShowFilterRow = true;
                gridassetfinal.Caption = "STEP 3 OF 3 [ Select Cheques To Update ]";
                oAspxHelper.BindGrid(Dtgridbindforstage2, gridassetfinal);
            }
            if (WhichCall == "All3")
            {
                gridassetfinal.Caption = "STEP 3 OF 3 [ Select Cheques To Update ]";
                gridassetfinal.FilterExpression = string.Empty;
                gridassetfinal.Settings.ShowFilterRow = false;
                oAspxHelper.BindGrid(Dtgridbindforstage2, gridassetfinal);
            }

        }
        protected void cbAll_Init(object sender, EventArgs e)
        {
            ASPxCheckBox chk = sender as ASPxCheckBox;
            ASPxGridView grid = (chk.NamingContainer as GridViewHeaderTemplateContainer).Grid;
            chk.Checked = (grid.Selection.Count == grid.VisibleRowCount);

        }


        protected void gridpreview_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            P_stepid = "2";
            WhichCall = e.Parameters.Split('~')[0];
            if (WhichCall == "s2")
            {

                gridpreview.Settings.ShowFilterRow = true;
                gridpreview.Caption = "STEP 2 OF 3 [ Final Cheques To Print ]";
                oAspxHelper.BindGrid(Dtgridbindforstage2, gridpreview);
            }
            if (WhichCall == "All2")
            {
                gridpreview.Caption = "STEP 2 OF 3 [ Final Cheques To Print ]";
                gridpreview.FilterExpression = string.Empty;
                gridpreview.Settings.ShowFilterRow = false;
                oAspxHelper.BindGrid(Dtgridbindforstage2, gridpreview);
            }
            if (WhichCall == "Cancel")
            {

                gridpreview.Caption = "STEP 2 OF 3 [ Final Cheques To Print ]";
                oAspxHelper.BindGrid(Dtgridbindforstage2, gridpreview);
            }
        }
        protected void gridpreview_PageIndexChanging(object sender, EventArgs e)
        {

            P_stepid = "2";
            gridpreview.Caption = "STEP 2 OF 3 [ Final Cheques To Print ]";
            oAspxHelper.BindGrid(Dtgridbindforstage2, gridpreview);

        }
        protected void cmbExport1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ReportDocument PrintDocument = new ReportDocument();
            string[] connPath = ConfigurationManager.AppSettings["DBConnectionDefault"].Split(';');
            string cbid = "";
            string cbidstatic = "";
            string cbdetailid = "";
            string cbdetailidstatic = "";
            string Account = "";
            string ClientUcc = "";
            string date = "";
            string path = "";
            string customertype = splitsession[9].ToString();
            string fromdate = splitsession[7].ToString();
            string todate = splitsession[8].ToString();
            string date1 = splitsession[2].ToString().Split('-')[0];
            string date2 = splitsession[2].ToString().Split('-')[1];
            string date3 = splitsession[2].ToString().Split('-')[2];
            int chqno = Convert.ToInt32(splitsession[4].ToString());
            if (date2.Length == 1)
            {
                date2 = "0" + date2;
            }
            if (date3.Length == 1)
            {
                date3 = "0" + date3;
            }
            //string totaldate = date1 + "-" + date2 + "-" + date3;
            string totaldate = date3 + "-" + date2 + "-" + date1;
            totaldate = oconverter.ArrangeDate2(totaldate);
            if (customertype == "C")
            {
                for (int M = 0; M < Dtgridbindforstage2.Rows.Count; M++)
                {
                    cbid = Dtgridbindforstage2.Rows[M]["cbd_id"].ToString();
                    cbdetailid = Dtgridbindforstage2.Rows[M]["CashBankDetail_ID"].ToString();
                    if (cbidstatic == "")
                    {
                        cbidstatic = cbid;
                    }
                    else
                    {
                        cbidstatic += "," + cbid;
                    }
                    if (cbdetailidstatic == "")
                    {
                        cbdetailidstatic = cbdetailid;
                    }
                    else
                    {
                        cbdetailidstatic += "," + cbdetailid;
                    }
                }
                DataSet DtChequePrint = Cashbank(bankid, fromdate, todate, Session["LastFinYear"].ToString(), customertype, "P", cbdetailidstatic, cbidstatic, Session["usersegid"].ToString());
                DtChequePrint.Tables[0].TableName = "DtChequePrint";


                path = string.Empty;
                if (splitsession[10] == "H")
                    path = HttpContext.Current.Server.MapPath("..\\Reports\\ChequePrintlazerfinal.rpt");
                else
                    path = HttpContext.Current.Server.MapPath("..\\Reports\\ChequePrintlazerfinalAxis.rpt");
                PrintDocument.Load(path);
                PrintDocument.SetDatabaseLogon(connPath[2].Substring(connPath[2].IndexOf("=")).Trim(), connPath[3].Substring(connPath[3].IndexOf("=")).Trim(), connPath[0].Substring(connPath[0].IndexOf("=")).Trim(), connPath[1].Substring(connPath[1].IndexOf("=")).Trim(), false);
                for (int M = 0; M < DtChequePrint.Tables[0].Rows.Count; M++)
                {
                    DtChequePrint.Tables[0].Rows[M]["CashBankDetail_InstrumentDate"] = totaldate;
                }
                PrintDocument.SetDataSource(DtChequePrint);
                PrintDocument.DataSourceConnections.Clear();
                Account = splitsession[0].ToString();

                ClientUcc = splitsession[1].ToString();

                date = splitsession[3].ToString();

                PrintDocument.SetParameterValue("@Field", (object)date);
                PrintDocument.SetParameterValue("@Field2", (object)"A/C No.");
                PrintDocument.SetParameterValue("@Field3", (object)Account);
                PrintDocument.SetParameterValue("@Field4", (object)ClientUcc);
            }
            else
            {
                for (int pom = 0; pom < Dtgridbindforstage2.Rows.Count; pom++)
                {

                    cbdetailid = Dtgridbindforstage2.Rows[pom]["CashBankDetail_ID"].ToString();

                    if (cbdetailidstatic == "")
                    {
                        cbdetailidstatic = cbdetailid;
                    }
                    else
                    {
                        cbdetailidstatic += "," + cbdetailid;
                    }
                }
                DataSet DtChequePrint = Cashbank(bankid, fromdate, todate, Session["LastFinYear"].ToString(), customertype, "P", cbdetailidstatic, "", Session["usersegid"].ToString());
                DtChequePrint.Tables[0].TableName = "DtChequePrint";
                path = string.Empty;
                path = HttpContext.Current.Server.MapPath("..\\Reports\\ChequeLazerPrint1.rpt");

                PrintDocument.Load(path);
                PrintDocument.SetDatabaseLogon(connPath[2].Substring(connPath[2].IndexOf("=")).Trim(), connPath[3].Substring(connPath[3].IndexOf("=")).Trim(), connPath[0].Substring(connPath[0].IndexOf("=")).Trim(), connPath[1].Substring(connPath[1].IndexOf("=")).Trim(), false);
                for (int gab = 0; gab < DtChequePrint.Tables[0].Rows.Count; gab++)
                {
                    DtChequePrint.Tables[0].Rows[gab]["CashBankDetail_InstrumentDate"] = totaldate;
                }
                PrintDocument.SetDataSource(DtChequePrint);
                PrintDocument.DataSourceConnections.Clear();

                Account = "false";

                ClientUcc = "false";

                date = splitsession[2].ToString();
                PrintDocument.SetParameterValue("@Field", (object)date);
                PrintDocument.SetParameterValue("@Field2", (object)"A/C No.");
                PrintDocument.SetParameterValue("@Field3", (object)Account);
                PrintDocument.SetParameterValue("@Field4", (object)ClientUcc);
            }
            PrintDocument.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, HttpContext.Current.Response, true, "Cheque");

        }
        protected void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable DtChequePrintSession = Dtgridbindforstage2.Clone();
            DtChequePrintSession.Columns["Payment"].DataType = typeof(Decimal);
            foreach (DataRow row in Dtgridbindforstage2.Rows)
            {
                DtChequePrintSession.ImportRow(row);
            }


            ExportToPDF(DtChequePrintSession);
        }
        void ExportToPDF(DataTable DtChequePrintSession)
        {
            ReportDocument PrintDocument = new ReportDocument();
            string[] connPath = ConfigurationManager.AppSettings["DBConnectionDefault"].Split(';');
            string path;
            path = string.Empty;
            path = HttpContext.Current.Server.MapPath("..\\Reports\\chequereport.rpt");
            //DtChequePrintSession.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "//Reports//chequereport.xsd");
            PrintDocument.Load(path);
            PrintDocument.SetDatabaseLogon(connPath[2].Substring(connPath[2].IndexOf("=")).Trim(), connPath[3].Substring(connPath[3].IndexOf("=")).Trim(), connPath[0].Substring(connPath[0].IndexOf("=")).Trim(), connPath[1].Substring(connPath[1].IndexOf("=")).Trim(), false);
            PrintDocument.SetDataSource(DtChequePrintSession);
            PrintDocument.DataSourceConnections.Clear();
            PrintDocument.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, HttpContext.Current.Response, true, "Cheque Report");
        }
        DataSet Cashbank(string mainaccountid, string fromdate, string todate, string finyear, string customertype,
                         string fetchtype, string cbdetailid, string cbid, string segmentid)
        {
            string[] InputName = new string[9];
            string[] InputType = new string[9];
            string[] InputValue = new string[9];

            DataSet DsCashBank = new DataSet();
            InputName[0] = "mainaccountid";
            InputName[1] = "fromdate";
            InputName[2] = "todate";
            InputName[3] = "finyear";
            InputName[4] = "customertype";
            InputName[5] = "fetchtype";
            InputName[6] = "cbdetailid";
            InputName[7] = "cbid";
            InputName[8] = "segmentid";


            InputType[0] = "V";
            InputType[1] = "V";
            InputType[2] = "V";
            InputType[3] = "V";
            InputType[4] = "V";
            InputType[5] = "V";
            InputType[6] = "V";
            InputType[7] = "V";
            InputType[8] = "V";


            InputValue[0] = mainaccountid;
            InputValue[1] = fromdate;
            InputValue[2] = todate;
            InputValue[3] = finyear;
            InputValue[4] = customertype;
            InputValue[5] = fetchtype;
            InputValue[6] = cbdetailid;
            InputValue[7] = cbid;
            InputValue[8] = segmentid;


            DsCashBank = SQLProcedures.SelectProcedureArrDS("Data_Chequeprinting", InputName, InputType, InputValue);
            if (DsCashBank.Tables.Count > 0)
                if (DsCashBank.Tables[0].Rows.Count > 0)
                    return DsCashBank;

            return null;
        }
    }
}
