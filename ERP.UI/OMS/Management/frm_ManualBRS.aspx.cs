using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
////using DevExpress.Web;
using System.Configuration;
using DevExpress.Web;


namespace ERP.OMS.Management
{
    public partial class management_frm_ManualBRS : System.Web.UI.Page
    {
        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);MULTI
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        BusinessLogicLayer.Converter ObjConvert = new BusinessLogicLayer.Converter();
        int PageSize = 1000;
        int PageIndex;
        int UpperBound = 0;
        string CheckStatus = "";
        int j = 0;
        DataTable DT = new DataTable();
        DataTable DT1 = new DataTable();
        protected void Page_Init(object sender, EventArgs e)
        {
            SqlDetails.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                hdActivity.Value = "Y";
                hdPageIndex.Value = "0";
                PageIndex = 0;
                DateTime FromDateForSplit = new DateTime();
                FromDate.EditFormatString = ObjConvert.GetDateFormat("Date");
                FromDateForSplit = Convert.ToDateTime(oDBEngine.GetDate().ToString()).AddMonths(-1);
                string mm = FromDateForSplit.Month.ToString();
                string yy = FromDateForSplit.Year.ToString();
                FromDate.Value = Convert.ToDateTime(mm + "-" + "01" + "-" + yy);
                DateTo.EditFormatString = ObjConvert.GetDateFormat("Date");
                DateTo.Value = Convert.ToDateTime(oDBEngine.GetDate().ToString());
                txtBankName.Attributes.Add("onkeyup", "ShowBankName(this,'SearchBankNameFromMainAccount',event)");
                //hpAllRecords.Attributes.Add("onclick", "SearchVisible('N')");
                //hpFilterRecords.Attributes.Add("onclick", "SearchVisible('Y')");
                Page.ClientScript.RegisterStartupScript(GetType(), "InitialVisibility", "<script language='JavaScript'> SearchVisible('N');</script>");

                if (RdAll.Checked == true)
                {
                    CheckStatus = "A";
                }
                else if (RdUnCleared.Checked == true)
                {
                    CheckStatus = "U";
                }
                else if (RdCleared.Checked = true)
                {
                    CheckStatus = "C";
                }

                BindTable();
                if (hdTotalPages.Value.ToString() != "")
                {
                    if (int.Parse(hdPageIndex.Value) == int.Parse(hdTotalPages.Value) - 1)
                    {
                        Page.ClientScript.RegisterStartupScript(GetType(), "hidebind", "<script language='Javascript'>Disable('O')</script>");
                    }
                    else
                    {
                        Page.ClientScript.RegisterStartupScript(GetType(), "hidebind1", "<script language='Javascript'>Disable('P')</script>");

                    }
                }
                Page.ClientScript.RegisterStartupScript(GetType(), "Height", "<script language='Javascript'>height()</script>");

            }
            else
            {
                BindTable();
            }

        }
        public void BindTable()
        {
            int total = 0;
            if (hdActivity.Value == "Y")
            {
                if (RdAll.Checked == true)
                {
                    CheckStatus = "A";
                }
                else if (RdUnCleared.Checked == true)
                {
                    CheckStatus = "U";
                }
                else if (RdCleared.Checked = true)
                {
                    CheckStatus = "C";
                }
                if (CheckStatus == "U")
                {
                    DT = oDBEngine.GetDataTable("Trans_CashBankVouchers  as a inner join Trans_CashBankDetail as b on a.cashbank_id=b.cashbankdetail_voucherid inner join master_mainaccount as c on b.cashbankdetail_mainaccountid=c.MAinaccount_accountcode inner join master_subaccount as d on b.cashbankdetail_subAccountid=d.subaccount_code", "distinct a.cashbank_transactionDate as cashbank_transactionDate_test,(convert(varchar(11),a.cashbank_transactionDate,113))as cashbank_transactionDate,a.cashbank_vouchernumber,(convert(varchar(11),b.cashbankdetail_instrumentdate,113))as cashbankdetail_instrumentdate,b.cashbankdetail_instrumentdate as cashbankdetail_instrumentdate_test,b.cashbankdetail_instrumentnumber,c.mainaccount_name +'['+d.subaccount_name+'['+ (isnull((select isnull(ltrim(rtrim(cnt_ucc)),'') from tbl_master_contact where cnt_internalid=d.subaccount_code),(select top 1 subaccount_code from master_subaccount where cast(subaccount_code as varchar)=d.subaccount_code)))+']' +']' as AccountCode,case b.cashbankdetail_paymentamount when 0 then null else b.cashbankdetail_paymentamount end as cashbankdetail_paymentamount,case b.cashbankdetail_receiptamount when 0 then null else b.cashbankdetail_receiptamount end as cashbankdetail_receiptamount,case b.cashbankdetail_bankvaluedate when '1900-01-01 00:00:00.000' then null else b.cashbankdetail_bankvaluedate end cashbankdetail_bankvaluedate,b.cashbankdetail_id,d.subaccount_code,b.cashbankdetail_mainaccountid,d.subaccount_name ", " a.cashbank_transactionDate>='" + FromDate.Value + "' and a.cashbank_transactionDate<='" + DateTo.Value + "' and (b.cashbankdetail_bankvaluedate is null or b.cashbankdetail_bankvaluedate ='1900-01-01 00:00:00.000') and b.cashbankdetail_cashbankid='" + txtBankName_hidden.Value.ToString() + "'", "cashbank_transactionDate");
                }
                else if (CheckStatus == "C")
                {
                    DT = oDBEngine.GetDataTable("Trans_CashBankVouchers  as a inner join Trans_CashBankDetail as b on a.cashbank_id=b.cashbankdetail_voucherid inner join master_mainaccount as c on b.cashbankdetail_mainaccountid=c.MAinaccount_accountcode inner join master_subaccount as d on b.cashbankdetail_subAccountid=d.subaccount_code", "distinct a.cashbank_transactionDate as cashbank_transactionDate_test,(convert(varchar(11),a.cashbank_transactionDate,113))as cashbank_transactionDate,a.cashbank_vouchernumber,(convert(varchar(11),b.cashbankdetail_instrumentdate,113))as cashbankdetail_instrumentdate,b.cashbankdetail_instrumentdate as cashbankdetail_instrumentdate_test,b.cashbankdetail_instrumentnumber,c.mainaccount_name +'['+d.subaccount_name+'['+ (isnull((select isnull(ltrim(rtrim(cnt_ucc)),'') from tbl_master_contact where cnt_internalid=d.subaccount_code),(select top 1 subaccount_code from master_subaccount where cast(subaccount_code as varchar)=d.subaccount_code)))+']' +']' as AccountCode,case b.cashbankdetail_paymentamount when 0 then null else b.cashbankdetail_paymentamount end as cashbankdetail_paymentamount,case b.cashbankdetail_receiptamount when 0 then null else b.cashbankdetail_receiptamount end as cashbankdetail_receiptamount,case b.cashbankdetail_bankvaluedate when '1900-01-01 00:00:00.000' then null else b.cashbankdetail_bankvaluedate end cashbankdetail_bankvaluedate,b.cashbankdetail_id,d.subaccount_code,b.cashbankdetail_mainaccountid,d.subaccount_name ", " a.cashbank_transactionDate>='" + FromDate.Value + "' and a.cashbank_transactionDate<='" + DateTo.Value + "' and (b.cashbankdetail_bankvaluedate is not null  and b.cashbankdetail_bankvaluedate <>'1900-01-01 00:00:00.000') and b.cashbankdetail_cashbankid='" + txtBankName_hidden.Value.ToString() + "'", "cashbank_transactionDate");

                }
                else if (CheckStatus == "A")
                {
                    DT = oDBEngine.GetDataTable("Trans_CashBankVouchers  as a inner join Trans_CashBankDetail as b on a.cashbank_id=b.cashbankdetail_voucherid inner join master_mainaccount as c on b.cashbankdetail_mainaccountid=c.MAinaccount_accountcode inner join master_subaccount as d on b.cashbankdetail_subAccountid=d.subaccount_code", "distinct a.cashbank_transactionDate as cashbank_transactionDate_test,(convert(varchar(11),a.cashbank_transactionDate,113))as cashbank_transactionDate,a.cashbank_vouchernumber,(convert(varchar(11),b.cashbankdetail_instrumentdate,113))as cashbankdetail_instrumentdate,b.cashbankdetail_instrumentdate as cashbankdetail_instrumentdate_test,b.cashbankdetail_instrumentnumber,c.mainaccount_name +'['+d.subaccount_name+'['+ (isnull((select isnull(ltrim(rtrim(cnt_ucc)),'') from tbl_master_contact where cnt_internalid=d.subaccount_code),(select top 1 subaccount_code from master_subaccount where cast(subaccount_code as varchar)=d.subaccount_code)))+']' +']' as AccountCode,case b.cashbankdetail_paymentamount when 0 then null else b.cashbankdetail_paymentamount end as cashbankdetail_paymentamount,case b.cashbankdetail_receiptamount when 0 then null else b.cashbankdetail_receiptamount end as cashbankdetail_receiptamount,case b.cashbankdetail_bankvaluedate when '1900-01-01 00:00:00.000' then null else b.cashbankdetail_bankvaluedate end cashbankdetail_bankvaluedate,b.cashbankdetail_id,d.subaccount_code,b.cashbankdetail_mainaccountid,d.subaccount_name ", " a.cashbank_transactionDate>='" + FromDate.Value + "' and a.cashbank_transactionDate<='" + DateTo.Value + "' and b.cashbankdetail_cashbankid='" + txtBankName_hidden.Value.ToString() + "'", "cashbank_transactionDate");

                }
            }
            else
            {
                if ((DataTable)ViewState["TableForThePage"] == null)
                {
                    if (RdAll.Checked == true)
                    {
                        CheckStatus = "A";
                    }
                    else if (RdUnCleared.Checked == true)
                    {
                        CheckStatus = "U";
                    }
                    else if (RdCleared.Checked = true)
                    {
                        CheckStatus = "C";
                    }
                    if (CheckStatus == "U")
                    {
                        DT = oDBEngine.GetDataTable("Trans_CashBankVouchers  as a inner join Trans_CashBankDetail as b on a.cashbank_id=b.cashbankdetail_voucherid inner join master_mainaccount as c on b.cashbankdetail_mainaccountid=c.MAinaccount_accountcode inner join master_subaccount as d on b.cashbankdetail_subAccountid=d.subaccount_code", "distinct a.cashbank_transactionDate as cashbank_transactionDate_test,(convert(varchar(11),a.cashbank_transactionDate,113))as cashbank_transactionDate,a.cashbank_vouchernumber,(convert(varchar(11),b.cashbankdetail_instrumentdate,113))as cashbankdetail_instrumentdate,b.cashbankdetail_instrumentdate as cashbankdetail_instrumentdate_test,b.cashbankdetail_instrumentnumber,c.mainaccount_name +'['+d.subaccount_name+'['+ (select  ltrim(rtrim(cnt_ucc)) from tbl_master_contact where cnt_internalid=d.subaccount_code )+']' +']' as AccountCode,case b.cashbankdetail_paymentamount when 0 then null else b.cashbankdetail_paymentamount end as cashbankdetail_paymentamount,case b.cashbankdetail_receiptamount when 0 then null else b.cashbankdetail_receiptamount end as cashbankdetail_receiptamount,b.cashbankdetail_bankvaluedate,b.cashbankdetail_id,d.subaccount_code,b.cashbankdetail_mainaccountid ", " a.cashbank_transactionDate>='" + FromDate.Value + "' and a.cashbank_transactionDate<='" + DateTo.Value + "' and (b.cashbankdetail_bankvaluedate is null or b.cashbankdetail_bankvaluedate ='1900-01-01 00:00:00.000') and b.cashbankdetail_cashbankid='" + txtBankName_hidden.Value.ToString() + "'", "cashbank_transactionDate");
                    }
                    else if (CheckStatus == "C")
                    {
                        DT = oDBEngine.GetDataTable("Trans_CashBankVouchers  as a inner join Trans_CashBankDetail as b on a.cashbank_id=b.cashbankdetail_voucherid inner join master_mainaccount as c on b.cashbankdetail_mainaccountid=c.MAinaccount_accountcode inner join master_subaccount as d on b.cashbankdetail_subAccountid=d.subaccount_code", "distinct a.cashbank_transactionDate as cashbank_transactionDate_test,(convert(varchar(11),a.cashbank_transactionDate,113))as cashbank_transactionDate,a.cashbank_vouchernumber,(convert(varchar(11),b.cashbankdetail_instrumentdate,113))as cashbankdetail_instrumentdate,b.cashbankdetail_instrumentdate as cashbankdetail_instrumentdate_test,b.cashbankdetail_instrumentnumber,c.mainaccount_name +'['+d.subaccount_name+'['+ (select  ltrim(rtrim(cnt_ucc)) from tbl_master_contact where cnt_internalid=d.subaccount_code )+']' +']' as AccountCode,case b.cashbankdetail_paymentamount when 0 then null else b.cashbankdetail_paymentamount end as cashbankdetail_paymentamount,case b.cashbankdetail_receiptamount when 0 then null else b.cashbankdetail_receiptamount end as cashbankdetail_receiptamount,b.cashbankdetail_bankvaluedate,b.cashbankdetail_id,d.subaccount_code,b.cashbankdetail_mainaccountid ", " a.cashbank_transactionDate>='" + FromDate.Value + "' and a.cashbank_transactionDate<='" + DateTo.Value + "' and (b.cashbankdetail_bankvaluedate is not null  and b.cashbankdetail_bankvaluedate <>'1900-01-01 00:00:00.000') and b.cashbankdetail_cashbankid='" + txtBankName_hidden.Value.ToString() + "'", "cashbank_transactionDate");

                    }
                    else if (CheckStatus == "A")
                    {
                        DT = oDBEngine.GetDataTable("Trans_CashBankVouchers  as a inner join Trans_CashBankDetail as b on a.cashbank_id=b.cashbankdetail_voucherid inner join master_mainaccount as c on b.cashbankdetail_mainaccountid=c.MAinaccount_accountcode inner join master_subaccount as d on b.cashbankdetail_subAccountid=d.subaccount_code", "distinct a.cashbank_transactionDate as cashbank_transactionDate_test,(convert(varchar(11),a.cashbank_transactionDate,113))as cashbank_transactionDate,a.cashbank_vouchernumber,(convert(varchar(11),b.cashbankdetail_instrumentdate,113))as cashbankdetail_instrumentdate,b.cashbankdetail_instrumentdate as cashbankdetail_instrumentdate_test,b.cashbankdetail_instrumentnumber,c.mainaccount_name +'['+d.subaccount_name+'['+ (select  ltrim(rtrim(cnt_ucc)) from tbl_master_contact where cnt_internalid=d.subaccount_code )+']' +']' as AccountCode,case b.cashbankdetail_paymentamount when 0 then null else b.cashbankdetail_paymentamount end as cashbankdetail_paymentamount,case b.cashbankdetail_receiptamount when 0 then null else b.cashbankdetail_receiptamount end as cashbankdetail_receiptamount,b.cashbankdetail_bankvaluedate,b.cashbankdetail_id,d.subaccount_code,b.cashbankdetail_mainaccountid ", " a.cashbank_transactionDate>='" + FromDate.Value + "' and a.cashbank_transactionDate<='" + DateTo.Value + "' and b.cashbankdetail_cashbankid='" + txtBankName_hidden.Value.ToString() + "'", "cashbank_transactionDate");

                    }
                }
                else
                {
                    DT = (DataTable)ViewState["TableForThePage"];
                }
            }
            #region databind

            if (DT.Rows.Count > 0)
            {
                ViewState["TableForThePage"] = DT;
                tdTable.Visible = true;
                trhypertext.Visible = true;
                //Creating Header for the Table
                Label lblheader1 = new Label();
                Label lblheader2 = new Label();
                Label lblheader3 = new Label();
                Label lblheader4 = new Label();
                Label lblheader5 = new Label();
                Label lblheader6 = new Label();
                Label lblheader7 = new Label();
                Label lblheader8 = new Label();
                Label lblheader9 = new Label();
                TableRow RowHeader = new TableRow();
                lblheader1.Text = "Tr. Date";
                lblheader2.Text = "Voucher No.";
                lblheader3.Text = "Instrument Date";
                lblheader4.Text = "Ins No.";
                lblheader5.Text = "Account Name [ Code ]";
                lblheader6.Text = "Pmt. Amt.";
                lblheader7.Text = "Rcpt. Amt.";
                lblheader8.Text = "Value Date";
                lblheader1.Font.Bold = true;
                lblheader2.Font.Bold = true;
                lblheader3.Font.Bold = true;
                lblheader4.Font.Bold = true;
                lblheader5.Font.Bold = true;
                lblheader6.Font.Bold = true;
                lblheader7.Font.Bold = true;
                lblheader8.Font.Bold = true;
                lblheader9.Font.Bold = true;
                TableCell TblCellHeader1 = new TableCell();
                TblCellHeader1.Controls.Add(lblheader1);
                RowHeader.Cells.Add(TblCellHeader1);

                TableCell TblCellHeader2 = new TableCell();
                TblCellHeader2.Controls.Add(lblheader2);
                RowHeader.Cells.Add(TblCellHeader2);

                TableCell TblCellHeader3 = new TableCell();
                TblCellHeader3.Controls.Add(lblheader3);
                RowHeader.Cells.Add(TblCellHeader3);

                TableCell TblCellHeader4 = new TableCell();
                TblCellHeader4.Controls.Add(lblheader4);
                RowHeader.Cells.Add(TblCellHeader4);

                TableCell TblCellHeader5 = new TableCell();
                TblCellHeader5.Controls.Add(lblheader5);
                RowHeader.Cells.Add(TblCellHeader5);

                TableCell TblCellHeader6 = new TableCell();
                TblCellHeader6.Controls.Add(lblheader6);
                RowHeader.Cells.Add(TblCellHeader6);

                TableCell TblCellHeader7 = new TableCell();
                TblCellHeader7.Controls.Add(lblheader7);
                RowHeader.Cells.Add(TblCellHeader7);

                TableCell TblCellHeader8 = new TableCell();
                TblCellHeader8.Controls.Add(lblheader8);
                RowHeader.Cells.Add(TblCellHeader8);
                TblCellHeader8.HorizontalAlign.Equals("Center");
                RowHeader.CssClass = "EHEADER";
                tblDetails.Rows.Add(RowHeader);

                //Creating pagination
                if (DT.Rows.Count % PageSize == 0)
                {
                    total = DT.Rows.Count / PageSize;
                }
                else
                {
                    total = DT.Rows.Count / PageSize + 1;
                }
                if (tdTable.Visible = true)
                {
                    hdTotalPages.Value = total.ToString();
                }
                if (hdPageIndex.Value.ToString() != "")
                {
                    //PageIndex = (int.Parse(hdPageIndex.Value));
                    if (int.Parse(hdPageIndex.Value) == int.Parse(hdTotalPages.Value) - 1)
                    {
                        UpperBound = DT.Rows.Count - 1;
                    }
                    else
                    {
                        UpperBound = ((int.Parse(hdPageIndex.Value) + 1) * PageSize) - 1;
                    }

                }
                else
                {
                    UpperBound = ((int.Parse(hdPageIndex.Value) + 1) * PageSize) - 1;
                }
                //Creating Data Rows
                for (int i = int.Parse(hdPageIndex.Value) * PageSize; i <= UpperBound; i++)
                {

                    //Label lbl1 = new Label();
                    TextBox lbl1 = new TextBox();
                    lbl1.ID = "lbl1" + i;
                    lbl1.Text = DT.Rows[i]["cashbank_transactionDate"].ToString();
                    lbl1.Width = 80;
                    lbl1.ReadOnly = true;
                    TextBox lbl2 = new TextBox();
                    lbl2.ID = "lbl2" + i;
                    lbl2.Text = DT.Rows[i]["cashbank_vouchernumber"].ToString();
                    lbl2.Width = 80;
                    lbl2.ReadOnly = true;
                    TextBox lbl3 = new TextBox();
                    lbl3.ID = "lbl3" + i;
                    lbl3.Text = DT.Rows[i]["cashbankdetail_instrumentdate"].ToString();
                    lbl3.Width = 100;
                    lbl3.ReadOnly = true;
                    TextBox lbl4 = new TextBox();
                    lbl4.ID = "lbl4" + i;
                    lbl4.Style.Add(HtmlTextWriterStyle.TextAlign, "Right");
                    lbl4.Text = DT.Rows[i]["cashbankdetail_instrumentnumber"].ToString().Trim();
                    lbl4.Width = 80;
                    lbl4.ReadOnly = true;
                    TextBox lbl5 = new TextBox();
                    lbl5.ID = "lbl5" + i;
                    lbl5.Text = DT.Rows[i]["AccountCode"].ToString();
                    lbl5.Width = 320;
                    lbl5.ReadOnly = true;
                    TextBox lbl6 = new TextBox();
                    lbl6.ID = "lbl6" + i;
                    lbl6.Style.Add(HtmlTextWriterStyle.TextAlign, "Right");
                    if (DT.Rows[i]["cashbankdetail_paymentamount"].ToString() != "")
                    {
                        lbl6.Text = ObjConvert.getFormattedvaluewithoriginalsign(Convert.ToDecimal(DT.Rows[i]["cashbankdetail_paymentamount"]));
                    }
                    else
                    {
                        lbl6.Text = "";
                    }
                    lbl6.Width = 80;
                    lbl6.ReadOnly = true;
                    TextBox lbl7 = new TextBox();
                    lbl7.ID = "lbl7" + i;
                    lbl7.Style.Add(HtmlTextWriterStyle.TextAlign, "Right");
                    if (DT.Rows[i]["cashbankdetail_receiptamount"].ToString() != "")
                    {
                        lbl7.Text = ObjConvert.getFormattedvaluewithoriginalsign(Convert.ToDecimal(DT.Rows[i]["cashbankdetail_receiptamount"]));
                    }
                    else
                    {
                        lbl7.Text = "";
                    }
                    lbl7.Width = 80;
                    lbl7.ReadOnly = true;

                    TextBox ValueDate = new TextBox();
                    ValueDate.Attributes.Add("alt", "date");
                    //ASPxDateEdit ValueDate = new ASPxDateEdit();
                    //ValueDate.EditFormatString = ObjConvert.GetDateFormat("Date");
                    //ValueDate.EditFormat = EditFormat.Custom;
                    //ValueDate.UseMaskBehavior = true;
                    //ValueDate.Value = DT.Rows[i]["cashbankdetail_bankvaluedate"];
                    //ValueDate.ClientInstanceName = "ff1" + i.ToString();
                    //ValueDate.ID = "ff1" + i;
                    //ValueDate.Width = 120;
                    //ValueDate.ClientSideEvents.GotFocus = "function(s, e) {setvalue('ff1','" +i.ToString() + "');}";

                    int index = i + 1;
                    ValueDate.TabIndex = Int16.Parse(index.ToString());
                    Label lbl8 = new Label();
                    lbl8.ID = "lbl8" + i;
                    lbl8.Text = DT.Rows[i]["cashbankdetail_id"].ToString();
                    lbl8.Visible = false;
                    TableCell TblCell = new TableCell();
                    TableCell TblCell1 = new TableCell();
                    TableCell TblCell2 = new TableCell();
                    TableCell TblCell3 = new TableCell();
                    TableCell TblCell4 = new TableCell();
                    TableCell TblCell5 = new TableCell();
                    TableCell TblCell6 = new TableCell();
                    TableCell TblCell7 = new TableCell();
                    TableCell TblCell8 = new TableCell();
                    TableCell tblcell9 = new TableCell();
                    TableRow row = new TableRow();
                    TblCell.Controls.Add(lbl1);
                    row.Cells.Add(TblCell);
                    TblCell1.Controls.Add(lbl2);
                    row.Cells.Add(TblCell1);
                    TblCell2.Controls.Add(lbl3);
                    row.Cells.Add(TblCell2);
                    TblCell3.Controls.Add(lbl4);
                    row.Cells.Add(TblCell3);
                    TblCell4.Controls.Add(lbl5);
                    row.Cells.Add(TblCell4);
                    TblCell5.Controls.Add(lbl6);
                    TblCell5.HorizontalAlign.Equals("Right");
                    row.Cells.Add(TblCell5);
                    TblCell6.Controls.Add(lbl7);
                    TblCell6.HorizontalAlign.Equals("Right");
                    row.Cells.Add(TblCell6);
                    TblCell8.Controls.Add(ValueDate);
                    row.Cells.Add(TblCell8);
                    tblcell9.Controls.Add(lbl8);
                    row.Cells.Add(tblcell9);
                    tblDetails.Rows.Add(row);
                    lbltotal.Text = "Total No Of Records " + DT.Rows.Count.ToString();
                    lbltotal.Font.Bold = true;

                }
                int PN = int.Parse(hdPageIndex.Value) + 1;
                lblPageNo.Text = "Page No " + PN.ToString();
                lblPageNo.Font.Bold = true;
                lblPageNo.ForeColor = System.Drawing.Color.Blue;
                if (hdTotalPages.Value == "1")
                {
                    Page.ClientScript.RegisterStartupScript(GetType(), "ForOnePageOnly", "<script language='JavaScript'>Disable('O');</script>");
                }
                if (hdPageIndex.Value == "0")
                {
                    Page.ClientScript.RegisterStartupScript(GetType(), "ForOnePageOnly", "<script language='JavaScript'>Disable('P');</script>");

                }
            }
            else
            {
                tdTable.Visible = false;
                trhypertext.Visible = false;
            }
            #endregion
        }
        protected void btnUpdate_Click(object sender, EventArgs e)
        {

            if (int.Parse(hdPageIndex.Value) == int.Parse(hdTotalPages.Value) - 1)
            {
                UpperBound = DT.Rows.Count - 1;
            }
            else
            {
                UpperBound = ((int.Parse(hdPageIndex.Value) + 1) * PageSize) - 1;
            }
            //for (j = int.Parse(hdPageIndex.Value) * PageSize; j <= UpperBound; j++)
            //{
            SaveDataIntoDataTable();
            //}
            for (int i = 0; i <= UpperBound; i++)
            {

                if (DT.Rows[i]["cashbankdetail_bankvaluedate"].ToString() != "")
                {
                    oDBEngine.SetFieldValue("Trans_CashBankDetail", "cashbankdetail_bankvaluedate='" + DT.Rows[i]["cashbankdetail_bankvaluedate"] + "'", "cashbankdetail_id='" + DT.Rows[i]["cashbankdetail_id"] + "'");
                    oDBEngine.SetFieldValue("trans_accountsledger", "accountsledger_valuedate='" + DT.Rows[i]["cashbankdetail_bankvaluedate"] + "'", "accountsledger_subaccountid='" + DT.Rows[i]["subaccount_code"].ToString() + "' and accountsledger_Mainaccountid='" + DT.Rows[i]["cashbankdetail_mainaccountid"].ToString() + "' and accountsledger_transactionreferenceid='" + DT.Rows[i]["cashbank_vouchernumber"].ToString().Trim() + "' and accountsledger_instrumentnumber='" + DT.Rows[i]["cashbankdetail_instrumentnumber"].ToString().Trim() + "' and accountsledger_transactiondate='" + DT.Rows[i]["cashbank_transactionDate"] + "'");
                }

            }
            if (hdTotalPages.Value == "1")
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "ForOnePageOnly", "<script language='JavaScript'>Disable('O');</script>");
            }
            else if (int.Parse(hdPageIndex.Value) == int.Parse(hdTotalPages.Value) - 1)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "hide12", "<script language='Javascript'>Disable('N')</script>");
            }
            else if (int.Parse(hdPageIndex.Value) == 0)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "hide1", "<script language='Javascript'>Disable('P')</script>");
            }

            else
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "hide123", "<script language='Javascript'>Disable('Nc')</script>");

            }

            Page.ClientScript.RegisterStartupScript(GetType(), "RowVisibilityLast", "<script language='JavaScript'>SearchVisible('N');</script>");

        }
        protected void FirstPage_Click(object sender, ImageClickEventArgs e)
        {
            hdActivity.Value = "N";
            tblDetails.Rows.Clear();
            hdPageIndex.Value = "0";
            //Creating Header for the Table
            BindTable();

            int PN = int.Parse(hdPageIndex.Value) + 1;
            lblPageNo.Text = "Page No " + PN.ToString();
            lblPageNo.Font.Bold = true;
            lblPageNo.ForeColor = System.Drawing.Color.Blue;

            Page.ClientScript.RegisterStartupScript(GetType(), "hideFirst", "<script language='Javascript'>Disable('P')</script>");
            Page.ClientScript.RegisterStartupScript(GetType(), "RowVisibilityFirst", "<script language='JavaScript'>SearchVisible('N');</script>");

        }
        protected void PreviousPage_Click(object sender, ImageClickEventArgs e)
        {
            hdActivity.Value = "N";
            int UpperBound1;
            tblDetails.Rows.Clear();
            //SaveDataIntoDataTable();
            int no = int.Parse(hdPageIndex.Value) - 1;
            hdPageIndex.Value = no.ToString();
            BindTable();
            if (int.Parse(hdPageIndex.Value) == 0)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "hide1", "<script language='Javascript'>Disable('P')</script>");
            }

            else
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "VisibleAllP", "<script language='Javascript'>Disable('Nc')</script>");

            }
            int PN = int.Parse(hdPageIndex.Value) + 1;
            lblPageNo.Text = "Page No " + PN.ToString();
            lblPageNo.Font.Bold = true;
            lblPageNo.ForeColor = System.Drawing.Color.Blue;
            Page.ClientScript.RegisterStartupScript(GetType(), "RowVisibilityPre", "<script language='JavaScript'>SearchVisible('N');</script>");

        }
        protected void NextPage_Click(object sender, ImageClickEventArgs e)
        {
            SaveDataIntoDataTable();
            hdActivity.Value = "N";
            tblDetails.Rows.Clear();
            SaveDataIntoDataTable();
            int no = int.Parse(hdPageIndex.Value) + 1;
            hdPageIndex.Value = no.ToString();
            BindTable();
            if (int.Parse(hdPageIndex.Value) == int.Parse(hdTotalPages.Value) - 1)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "hide12", "<script language='Javascript'>Disable('N')</script>");
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "VisibleAll", "<script language='Javascript'>Disable('Nc')</script>");
            }
            Page.ClientScript.RegisterStartupScript(GetType(), "RowVisibilityNext", "<script language='JavaScript'>SearchVisible('N');</script>");

        }
        protected void LastPage_Click(object sender, ImageClickEventArgs e)
        {
            hdActivity.Value = "N";
            tblDetails.Rows.Clear();
            int no = int.Parse(hdTotalPages.Value) - 1;
            hdPageIndex.Value = no.ToString();
            //Creating Header for the Table
            BindTable();
            //hdPageIndex.Value = PageIndex.ToString();
            int PN = int.Parse(hdPageIndex.Value) + 1;
            lblPageNo.Text = "Page No " + PN.ToString();
            lblPageNo.Font.Bold = true;
            lblPageNo.ForeColor = System.Drawing.Color.Blue;

            Page.ClientScript.RegisterStartupScript(GetType(), "hide1", "<script language='Javascript'>Disable('N')</script>");
            Page.ClientScript.RegisterStartupScript(GetType(), "RowVisibilityLast", "<script language='JavaScript'>SearchVisible('N');</script>");

        }
        protected void btnShow_Click1(object sender, EventArgs e)
        {
            tdMainContent.Visible = true;
            tblDetails.Rows.Clear();
            hdActivity.Value = "Y";
            hdPageIndex.Value = "0";
            BindTable();
            if (tblDetails.Rows.Count == 0)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "MessageForBlankRecord", "<script language='JavaScript'>alert('No Record Found');</script>");

            }
            Page.ClientScript.RegisterStartupScript(GetType(), "hidex", "<script language='Javascript'>height();</script>");
            Page.ClientScript.RegisterStartupScript(GetType(), "VisibilityOnShow", "<script language='JavaScript'> SearchVisible('N');</script>");
        }
        protected void SaveDataIntoDataTable()
        {
            for (int i = int.Parse(hdPageIndex.Value) * PageSize; i <= UpperBound; i++)
            {
                //Table tbl = (Table)form1.FindControl("tblDetails");
                Table tbl = (Table)Page.FindControl("tblDetails");
                ASPxDateEdit DateEdit = (ASPxDateEdit)tblDetails.FindControl("ff1" + i.ToString());
                Label lblId = (Label)tblDetails.FindControl("lbl8" + i.ToString());
                if (DateEdit.Value != null)
                {
                    DT.Rows[i]["cashbankdetail_bankvaluedate"] = DateEdit.Value;

                }
                else
                {
                    DT.Rows[i]["cashbankdetail_bankvaluedate"] = "1900-01-01 00:00:00.000";
                }
            }
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            ViewState["OriginalTableBeforeSearch"] = DT;
            DT1 = DT.Copy();
            DT1.Clear();
            DataRow[] Dr;
            if (AspTranDate.Value != null)
            {
                Dr = DT.Select("cashbank_transactionDate_test = '" + AspTranDate.Value.ToString() + "'");
                //Dr = DT.Select("cashbank_transactionDate = '" + AspTranDate.Value.ToString() + "'");
                CreateNewDataTableForSearch(Dr.Length, Dr);
            }
            if (txtVoucherNo.Text != "" && txtVoucherNo.Text != "Voucher No")
            {
                Dr = DT.Select("cashbank_vouchernumber like '" + txtVoucherNo.Text.ToString() + "%'");
                CreateNewDataTableForSearch(Dr.Length, Dr);
            }
            if (AspInsDate.Value != null)
            {

                Dr = DT.Select("cashbankdetail_instrumentdate_test = '" + AspInsDate.Value.ToString() + "'");
                CreateNewDataTableForSearch(Dr.Length, Dr);
            }
            if (txtInsNo.Text.ToString() != "" && txtInsNo.Text.ToString() != "Instrument No")
            {
                Dr = DT.Select("cashbankdetail_instrumentnumber like '" + txtInsNo.Text.ToString() + "%'");
                CreateNewDataTableForSearch(Dr.Length, Dr);

            }
            if (txtAccName.Text.ToString() != "" && txtAccName.Text.ToString() != "Main Account")
            {
                Dr = DT.Select("AccountCode like '" + txtAccName.Text.ToString() + "%'");
                CreateNewDataTableForSearch(Dr.Length, Dr);
            }
            if (txtPayAmt.Text.ToString() != "" && txtPayAmt.Text.ToString() != "Payment Amount")
            {
                Dr = DT.Select("cashbankdetail_paymentamount = '" + txtPayAmt.Text.ToString() + "'");
                CreateNewDataTableForSearch(Dr.Length, Dr);
            }
            if (txtReptAmt.Text.ToString() != "" && txtReptAmt.Text.ToString() != "Receipt Amount")
            {
                Dr = DT.Select("cashbankdetail_receiptamount = '" + txtReptAmt.Text.ToString() + "'");
                CreateNewDataTableForSearch(Dr.Length, Dr);
            }
            if (AspValueDate.Value != null)
            {

                Dr = DT.Select("cashbankdetail_bankvaluedate = '" + AspValueDate.Value.ToString() + "'");
                CreateNewDataTableForSearch(Dr.Length, Dr);

            }
            if (txtSubName.Text.ToString() != "" && txtSubName.Text.ToString() != "SubAccount")
            {
                Dr = DT.Select("subaccount_name like '" + txtSubName.Text.ToString() + "%'");
                CreateNewDataTableForSearch(Dr.Length, Dr);
            }
            if (DT1.Rows.Count == 0)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "Message", "<script language='JavaScript'>alert('No record found');</script>");
            }

            ViewState["TableForThePage"] = DT1;
            hdActivity.Value = "N";
            tblDetails.Rows.Clear();
            BindTable();
            trhypertext.Visible = true;
        }
        public void CreateNewDataTableForSearch(int length, DataRow[] DataRow)
        {
            tblDetails.Rows.Clear();
            if (length == 0)
            {
                DT1.Rows.Clear();
            }
            for (int count = 0; count < length; count++)
            {
                DataRow drnew = DT1.NewRow();
                drnew["cashbank_transactionDate_test"] = DataRow[count]["cashbank_transactionDate_test"].ToString();
                drnew["cashbank_transactionDate"] = DataRow[count]["cashbank_transactionDate"].ToString();
                drnew["cashbank_vouchernumber"] = DataRow[count]["cashbank_vouchernumber"].ToString();
                drnew["cashbankdetail_instrumentdate"] = DataRow[count]["cashbankdetail_instrumentdate"].ToString();
                drnew["cashbankdetail_instrumentdate_test"] = DataRow[count]["cashbankdetail_instrumentdate_test"].ToString();
                drnew["cashbankdetail_instrumentnumber"] = DataRow[count]["cashbankdetail_instrumentnumber"].ToString();
                drnew["AccountCode"] = DataRow[count]["AccountCode"].ToString();
                drnew["cashbankdetail_paymentamount"] = DataRow[count]["cashbankdetail_paymentamount"];
                drnew["cashbankdetail_receiptamount"] = DataRow[count]["cashbankdetail_receiptamount"];
                drnew["cashbankdetail_bankvaluedate"] = DataRow[count]["cashbankdetail_bankvaluedate"];
                drnew["cashbankdetail_id"] = DataRow[count]["cashbankdetail_id"].ToString();
                drnew["subaccount_code"] = DataRow[count]["subaccount_code"].ToString();
                drnew["cashbankdetail_mainaccountid"] = DataRow[count]["cashbankdetail_mainaccountid"].ToString();

                DT1.Rows.Add(drnew);
                //DT1[count]["cashbank_transactionDate"] = Dr[count]["cashbank_transactionDate"].ToString();
            }
        }
        protected void lnAllRecords_Click(object sender, EventArgs e)
        {
            ClearControls();
            Page.ClientScript.RegisterStartupScript(GetType(), "RowVisibility", "<script language='JavaScript'>SearchVisible('N');</script>");
            DT = (DataTable)ViewState["OriginalTableBeforeSearch"];
            ViewState["TableForThePage"] = DT;
            hdActivity.Value = "N";
            tblDetails.Rows.Clear();
            BindTable();
        }
        public void ClearControls()
        {
            AspTranDate.Value = null;
            txtVoucherNo.Text = "Voucher No";
            txtVoucherNo.ToolTip = "Voucher No";
            txtAccName.Text = "Main Account";
            txtAccName.ToolTip = "Main Account";
            AspInsDate.Value = null;
            txtInsNo.Text = "Instrument No";
            txtInsNo.ToolTip = "Instrument No";
            txtPayAmt.Text = "Payment Amount";
            txtPayAmt.ToolTip = "Payment Amount";
            txtReptAmt.Text = "Receipt Amount";
            txtReptAmt.ToolTip = "Receipt Amount";
            txtSubName.Text = "SubAccount";
            txtSubName.ToolTip = "SubAccount";
            AspValueDate.Value = null;
        }
    }
}
