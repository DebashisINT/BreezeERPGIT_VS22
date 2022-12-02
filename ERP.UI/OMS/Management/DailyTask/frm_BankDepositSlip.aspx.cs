using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrystalDecisions.CrystalReports.Engine;
using BusinessLogicLayer;
namespace ERP.OMS.Management.DailyTask
{


    public partial class management_DailyTask_frm_BankDepositSlip : System.Web.UI.Page
    {
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        BusinessLogicLayer.Converter OConvert = new BusinessLogicLayer.Converter();
        BusinessLogicLayer.Converter oconverter = new BusinessLogicLayer.Converter();
        string SelectDate = "";
        string NullSelect = "N";
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

               //Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }


            txtTranDate.EditFormatString = OConvert.GetDateFormat("Date");
            txtDipDate.EditFormatString = OConvert.GetDateFormat("Date");
            if (!IsPostBack)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>PageLoad();</script>");
                txtTranDate.Value = Convert.ToDateTime(DateTime.Today);
                txtDipDate.Value = Convert.ToDateTime(DateTime.Today);
            }
            //txtBankName.Attributes.Add("onkeyup", "ShowBankName(this,'SearchBankNameFromMainAccount',event)");
            //this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");
        }


        protected void GridSelect_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                CheckBox myBox = (CheckBox)e.Row.FindControl("chb2");
                myBox.Checked = true;


                //string lcVar1 = ((DataRowView)e.Row.DataItem)["CashBankDetail_SlipPrintDateTime"].ToString();
                //if (lcVar1 == "")
                //{
                //    CheckBox myBox = (CheckBox)e.Row.FindControl("chb2");
                //    myBox.Checked = true;

                //}
                //else
                //{
                //    CheckBox myBox = (CheckBox)e.Row.FindControl("chb2");
                //    myBox.Checked = false;

                //}
            }
        }
        protected void btnGenerate_Click(object sender, EventArgs e)
        {
            int i = 0;
            string DateNull = "N";
            string TranDate = "";
            foreach (GridViewRow gRow in gridSummary.Rows)
            {

                CheckBox myBox = (CheckBox)gRow.FindControl("chb1");
                if (myBox.Checked == true)
                {
                    Label Count = (Label)gRow.FindControl("lblCount");
                    Label TDate = (Label)gRow.FindControl("lblDate");
                    string TotTran = Count.Text;

                    if (TDate.Text == "")
                    {
                        DateNull = "Y";
                    }
                    else
                    {
                        if (TranDate == "")
                        {
                            TranDate = TranDate + "'" + TDate.Text + "'";

                        }
                        else
                        {
                            TranDate = TranDate + ",'" + TDate.Text + "'";

                        }

                    }
                }
            }
            SelectDate = TranDate;
            NullSelect = DateNull;
            if (DateNull != "N" || TranDate != "")
            {
                DataSet ds = new DataSet();
                // using(SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]))
                //  {
                if (TranDate == "")
                {
                    TranDate = "Not Available";
                }


                ExportRoutines objExportRoutines = new ExportRoutines();

                objExportRoutines.Fetch_CashBankDepositSlipsDetails(Convert.ToString(txtBankName_hidden.Value),
                    Convert.ToString(txtTranDate.Value), TranDate, DateNull, Session["LastCompany"].ToString());


                //using (SqlDataAdapter da = new SqlDataAdapter("Fetch_CashBankDepositSlipsDetails", con))
                //{
                //    if (TranDate == "")
                //    {
                //        TranDate = "Not Available";
                //    }

                //    da.SelectCommand.Parameters.AddWithValue("@BankID", txtBankName_hidden.Value.ToString());
                //    da.SelectCommand.Parameters.AddWithValue("@TransactionDate", txtTranDate.Value);
                //    da.SelectCommand.Parameters.AddWithValue("@PrintDateTime", TranDate);
                //    da.SelectCommand.Parameters.AddWithValue("@Checknull", DateNull);
                //    da.SelectCommand.Parameters.AddWithValue("@CompanyID", Session["LastCompany"].ToString());
                //    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                //    da.SelectCommand.CommandTimeout = 0;

                //    if (con.State == ConnectionState.Closed)
                //    con.Open();
                //    ds.Reset();
                //    da.Fill(ds);
                //    ViewState["dataset"] = ds;


                //    GridSelect.DataSource = ds.Tables[0];
                //    GridSelect.DataBind();
                //   //ScriptManager.RegisterStartupScript(this, this.GetType(), "JSct", "ShowHide();", true);
                //    this.Page.ClientScript.RegisterStartupScript(GetType(), "height45", "<script>ShowHide();</script>");
                //  //  print();
                //}
                // }


            }
            else
            {
                this.Page.ClientScript.RegisterStartupScript(GetType(), "heigh1", "<script>alert('Please select Transaction');</script>");

            }

        }
        void print()
        {
            DataSet ds = new DataSet();
            DataTable dtComp = oDBEngine.GetDataTable("master_mainAccount", "MainAccount_ReferenceID,MainAccount_Name as ACCName,MainAccount_BankAcNumber as ACCNo,(select Top 1 cmp_Name from tbl_master_company where cmp_internalid=MainAccount_BankCompany)as ConpanyName,(select Top 1 cmp_panNo from tbl_master_company where cmp_internalid=MainAccount_BankCompany)as CompanyPAN", " mainaccount_accountcode='" + txtBankName_hidden.Value.ToString() + "'");
            //ds = (DataSet)ViewState["dataset"];
            ds = (DataSet)ViewState["PirntData"];
            byte[] logoinByte;
            ds.Tables[0].Columns.Add("Image", System.Type.GetType("System.Byte[]"));
            decimal mTotAmt = 0;
            if (oconverter.getLogoImage(HttpContext.Current.Server.MapPath(@"..\images\logo.bmp"), out logoinByte) != 1)
            {
                ScriptManager.RegisterStartupScript(HttpContext.Current.Handler as Page, HttpContext.Current.GetType(), "logo", "alert('Logo not Found.');", true);
            }
            else
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    ds.Tables[0].Rows[i]["Image"] = logoinByte;
                    mTotAmt += Convert.ToDecimal(ds.Tables[0].Rows[i]["RecAmtN"].ToString());
                    //oDBEngine.SetFieldValue("TRANS_CASHBANKDETAIL", "CashBankDetail_SlipPrintDateTime='" + oDBEngine.GetDate() + "'", "CashBankDetail_ID='" + ds.Tables[0].Rows[i]["CashBankDetail_ID"] + "'");
                }
            }
            DataSet dn = new DataSet();
           // using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]))

            using (SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"])))
            
            {
                using (SqlDataAdapter da = new SqlDataAdapter(" select dbo.fn_FormatNumber('" + mTotAmt + "','N') ", con))
                {


                    da.SelectCommand.CommandType = CommandType.Text;
                    da.SelectCommand.CommandTimeout = 0;
                    if (con.State == ConnectionState.Closed)
                        con.Open();
                    dn.Reset();
                    da.Fill(dn);


                }
            }

            String TotNo = dn.Tables[0].Rows[0][0].ToString();

            //BindGrid();
            ReportDocument report = new ReportDocument();
            //    ds.Tables[0].WriteXmlSchema("E:\\RPTXSD\\DepositSlip.xsd");
            //ds.Tables[1].WriteXmlSchema("E:\\RPTXSD\\ClientMasterMainDetail.xsd");

            report.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Portrait;
            string tmpPdfPath = string.Empty;
            tmpPdfPath = HttpContext.Current.Server.MapPath("..\\management\\DepositSlip.rpt");
            report.Load(tmpPdfPath);
            report.SetDataSource(ds.Tables[0]);

            report.VerifyDatabase();



            if (mTotAmt != 0)
            {
                report.SetParameterValue("@SumAmtwithoutFormat", (object)mTotAmt);
            }
            else
            {
                report.SetParameterValue("@SumAmtwithoutFormat", (object)0);
            }

            if (TotNo.ToString() != "")
            {
                report.SetParameterValue("@SumAmtFormat", (object)TotNo.ToString());
            }
            else
            {
                report.SetParameterValue("@SumAmtFormat", (object)"");
            }
            if (dtComp.Rows.Count > 0)
            {

                if (dtComp.Rows[0]["ACCNo"].ToString() != "")
                {
                    report.SetParameterValue("@AccountNo", (object)dtComp.Rows[0]["ACCNo"].ToString());
                }
                else
                {
                    report.SetParameterValue("@AccountNo", (object)"NO A/C");
                }
                if (dtComp.Rows[0]["ConpanyName"].ToString() != "")
                {
                    report.SetParameterValue("@CompanyName", (object)dtComp.Rows[0]["ConpanyName"].ToString());
                }
                else
                {
                    report.SetParameterValue("@CompanyName", (object)"COMPANY NAME");
                }
                if (dtComp.Rows[0]["CompanyPAN"].ToString() != "")
                {
                    report.SetParameterValue("@CompanyPAN", (object)dtComp.Rows[0]["CompanyPAN"].ToString());
                }
                else
                {
                    report.SetParameterValue("@CompanyPAN", (object)"COMPANY PAN");
                }

            }
            if (txtDipDate.Value != "")
            {
                report.SetParameterValue("@DipostiDate", (object)txtDipDate.Date);
            }
            else
            {
                report.SetParameterValue("@DipostiDate", (object)"COMPANY PAN");
            }

            report.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, HttpContext.Current.Response, true, "DepositSlip");
            report.Dispose();
            GC.Collect();
        }
        protected void Button1_Click(object sender, EventArgs e)
        {

            BindGrid();
        }
        protected void BindGrid()
        {
            DataSet dsB = new DataSet();
            //  using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]))
            // {
            //using (SqlDataAdapter da = new SqlDataAdapter("Fetch_CashBankDepositSlips", con))
            //{ 
            ExportRoutines objExportRoutines = new ExportRoutines();
            dsB = objExportRoutines.Fetch_CashBankDepositSlips(txtBankName_hidden.Value.ToString(), txtTranDate.Value.ToString());

            //da.SelectCommand.Parameters.AddWithValue("@BankID", txtBankName_hidden.Value.ToString());
            //da.SelectCommand.Parameters.AddWithValue("@TransactionDate", txtTranDate.Value);
            //da.SelectCommand.CommandType = CommandType.StoredProcedure;
            //da.SelectCommand.CommandTimeout = 0; 
            //    if (con.State == ConnectionState.Closed)
            //    con.Open();
            //    dsB.Reset();
            //    da.Fill(dsB); 
            //}
            //  }

            ViewState["DsBind"] = dsB.Tables[0];
            gridSummary.DataSource = dsB.Tables[0];
            gridSummary.DataBind();


        }
        protected void gridSummary_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string lcVar1 = ((DataRowView)e.Row.DataItem)["PrintDate"].ToString();
                if (lcVar1 == "")
                {
                    CheckBox myBox = (CheckBox)e.Row.FindControl("chb1");
                    myBox.Checked = true;

                }
                else
                {
                    CheckBox myBox = (CheckBox)e.Row.FindControl("chb1");
                    myBox.Checked = false;

                }
            }
        }

        protected void btnShow_Click(object sender, EventArgs e)
        {
            DataSet DSn = new DataSet();
            int k = 0;
            string TotTran = "";
            DSn = (DataSet)ViewState["dataset"];
            DSn.Tables[0].Columns.Remove("CashBankDetail_SlipPrintDateTime");
            foreach (GridViewRow gRow in GridSelect.Rows)
            {

                CheckBox myBox = (CheckBox)gRow.FindControl("chb2");
                if (myBox.Checked == true)
                {
                    k = k + 1;
                    Label Count = (Label)gRow.FindControl("lblCashBankID");
                    if (k == 1)
                    {
                        TotTran = Count.Text;
                    }
                    else
                    {
                        TotTran = TotTran + "," + Count.Text;
                    }


                }
                else
                {
                    int j = gRow.RowIndex;
                    // Label Count = (Label)gRow.FindControl("lblCashBankID");
                    DSn.Tables[0].Rows[j].Delete();

                    // DSn= ViewState["dataset"];
                    // DSn.Tables[0].Rows[]
                }
            }
            DSn.Tables[0].AcceptChanges();
            ViewState["PirntData"] = DSn;
            oDBEngine.SetFieldValue("TRANS_CASHBANKDETAIL", " CashBankDetail_SlipPrintDateTime= '" + oDBEngine.GetDate() + "'", "CashBankDetail_ID in (" + TotTran + ")");
            print();
        }
    }

}