using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace ERP.OMS.Management.DailyTask
{
    public partial class management_DailyTask_frm_BankTransactionFile : ERP.OMS.ViewState_class.VSPage
    {
        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);

        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

        BusinessLogicLayer.Converter OConvert = new BusinessLogicLayer.Converter();
        BusinessLogicLayer.Converter oconverter = new BusinessLogicLayer.Converter();
        BusinessLogicLayer.DailyTaskOther oDailyTaskOther = new BusinessLogicLayer.DailyTaskOther();
        string SelectDate = "";
        string NullSelect = "N";
        DataSet ds = new DataSet();
        DataTable DT = new DataTable();
        string savefilepath;
        string path;
        ExcelFile objExcel = new ExcelFile();
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
            if (!IsPostBack)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>PageLoad();</script>");
                txtTranDate.Value = Convert.ToDateTime(DateTime.Today);
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


            }
        }
        protected void btnGenerate_Click(object sender, EventArgs e)
        {
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
                if (TranDate == "")
                {
                    TranDate = "Not Available";
                }
                ds = oDailyTaskOther.Fetch_BankTransactionDetails(
                    Convert.ToString(txtBankName_hidden.Value),
                     Convert.ToString(txtTranDate.Value),
                     Convert.ToString(HttpContext.Current.Session["usersegid"]),
                     Convert.ToString(TranDate),
                     Convert.ToString(DateNull),
                     Convert.ToString(ddlChequeType.SelectedItem.Text + "%"),
                     Convert.ToString(txtNarration.Text),
                     Convert.ToString(Session["LastCompany"])
                    );
                ViewState["dataset"] = ds;
                GridSelect.DataSource = ds.Tables[0];
                GridSelect.DataBind();
                this.Page.ClientScript.RegisterStartupScript(GetType(), "height45", "<script>ShowHide();</script>");


            }
        }
        void print()
        {

            ds = (DataSet)ViewState["dataset"];
            ds.Tables[0].Columns.Remove(ds.Tables[0].Columns["CASHBANKDETAIL_SUBACCOUNTID"]);
            ds.Tables[0].Columns.Remove(ds.Tables[0].Columns["CashBankDetail_ID"]);
            ds.Tables[0].Columns.Remove(ds.Tables[0].Columns["CilentName"]);
            ds.Tables[0].Columns.Remove(ds.Tables[0].Columns["BankName"]);

            if (ds.Tables[0].Rows.Count > 0)
            {

                savefilepath = @"ExportFiles/BankTransaction/" + ds.Tables[1].Rows[0]["filename"].ToString() + ds.Tables[1].Rows[0]["date"].ToString(); ///////////FILE SAVE INTO DATABASE
                path = Server.MapPath(@"../ExportFiles/BankTransaction/") + ds.Tables[1].Rows[0]["filename"].ToString() + ds.Tables[1].Rows[0]["date"].ToString() + "." + ds.Tables[1].Rows[0]["record"].ToString();///////////FILE SAVE INTO FOLDER

                if (!Directory.Exists(Server.MapPath(@"../ExportFiles")))
                    Directory.CreateDirectory(Server.MapPath(@"../ExportFiles"));

                if (!Directory.Exists(Server.MapPath(@"../ExportFiles/BankTransaction")))
                    Directory.CreateDirectory(Server.MapPath(@"../ExportFiles/BankTransaction"));

                using (StreamWriter sw = new StreamWriter(path, false))
                {

                    int colCount = ds.Tables[0].Columns.Count;
                    sw.Write("ACCOUNT");
                    sw.Write(",C");
                    sw.Write(",AMOUNT");
                    sw.Write(",NARRATION");
                    sw.Write(sw.NewLine);
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        for (int j = 0; j < colCount; j++)
                        {

                            if (!Convert.IsDBNull(dr[j]))
                            {
                                if (j == colCount - 1)
                                {
                                    sw.Write(dr[j]);
                                }
                                else
                                {
                                    sw.Write(dr[j] + ",");
                                }

                            }
                        }

                        sw.Write(sw.NewLine);
                    }


                }

                oDailyTaskOther.sp_Insert_ExportFiles(
                Convert.ToString(Session["usersegid"]),
                 "Bank Transaction",
                 Convert.ToString(ds.Tables[1].Rows[0]["filename"].ToString() + ds.Tables[1].Rows[0]["date"].ToString() + '_'),
                 Convert.ToString(HttpContext.Current.Session["userid"]),
                 Convert.ToString(ds.Tables[1].Rows[0]["record"]),
                 Convert.ToString(savefilepath)
                );

                fn(savefilepath);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "norecord", "norecord();", true);

            }


        }

        protected void fn(string STR)
        {
            ds = (DataSet)ViewState["dataset"];
            string filename = Server.MapPath("..\\" + STR) + "." + ds.Tables[1].Rows[0]["record"].ToString(); ;
            FileInfo fileInfo = new FileInfo(filename);

            if (fileInfo.Exists)
            {
                Response.Clear();
                Response.AddHeader("Content-Disposition", "attachment;filename=" + fileInfo.Name);
                Response.AddHeader("Content-Length", fileInfo.Length.ToString());
                Response.ContentType = "application/octet-stream";
                Response.Flush();
                Response.WriteFile(fileInfo.FullName);
                Response.End();
            }
        }


        protected void Button1_Click(object sender, EventArgs e)
        {

            BindGrid();
        }
        protected void BindGrid()
        {
            DataSet dsB = new DataSet();
            dsB = oDailyTaskOther.Fetch_BankTransaction(
                  Convert.ToString(txtBankName_hidden.Value),
                   Convert.ToString(HttpContext.Current.Session["usersegid"]),
                   Convert.ToString(txtTranDate.Value),
                   Convert.ToString(ddlChequeType.SelectedItem.Text + "%")
                  );
            ViewState["dataset"] = dsB;

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
                    DSn.Tables[0].Rows[j].Delete();
                }
            }
            DSn.Tables[0].AcceptChanges();
            ViewState["PirntData"] = DSn;
            oDBEngine.SetFieldValue("TRANS_CASHBANKDETAIL", " CashBankDetail_SlipPrintDateTime= '" + oDBEngine.GetDate() + "'", "CashBankDetail_ID in (" + TotTran + ")");
            print();
        }
    }

}