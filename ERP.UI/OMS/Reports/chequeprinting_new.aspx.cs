using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using DevExpress.Web;
using System.Web;

namespace ERP.OMS.Reports
{

    public partial class Reports_chequeprinting_new : System.Web.UI.Page
    {
        AspxHelper oAspxHelper = new AspxHelper();
        BusinessLogicLayer.Converter oconverter = new BusinessLogicLayer.Converter();
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        BusinessLogicLayer.GenericExcelExport oGenericExcelExport;
        string WhichCall;
        string SubAccountRefID;
        DataSet Ds_Global = new DataSet();
        BusinessLogicLayer.GenericMethod Gm = new BusinessLogicLayer.GenericMethod();
        string Rowid = "";
        string SelectedIds = null;
        string DeSelectedIds = null;
        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //'http://localhost:2957/InfluxCRM/management/testProjectMainPage_employee.aspx'
                string sPath = HttpContext.Current.Request.Url.ToString();
                oDBEngine.Call_CheckPageaccessebility(sPath);
            }
        }
        public DataSet P_PageNumAfterNav
        {
            get { return (DataSet)Session["PageNumAfterNav"]; }
            set { Session["PageNumAfterNav"] = value; }
        }
        public DataTable DtChequePrintSession1
        {
            get { return (DataTable)Session["DtChequePrintSession1"]; }
            set { Session["DtChequePrintSession1"] = value; }
        }
        public DataTable DtChequePrintforupdate
        {
            get { return (DataTable)Session["DtChequePrintforupdate"]; }
            set { Session["DtChequePrintforupdate"] = value; }
        }
        public string P_stepid
        {
            get { return (string)Session["stepid"]; }
            set { Session["stepid"] = value; }
        }
        public string P_subid
        {
            get { return (string)Session["subid"]; }
            set { Session["subid"] = value; }
        }
        public string P_voucherid
        {
            get { return (string)Session["voucherid"]; }
            set { Session["voucherid"] = value; }
        }
        public string P_transactiondate
        {
            get { return (string)Session["transactiondate"]; }
            set { Session["transactiondate"] = value; }
        }
        public string P_Bankid
        {
            get { return (string)Session["Bankid"]; }
            set { Session["Bankid"] = value; }
        }
        public string P_Rowid
        {
            get { return (string)Session["Rowid"]; }
            set { Session["Rowid"] = value; }
        }
        public string P_customertype
        {
            get { return (string)Session["customertype"]; }
            set { Session["customertype"] = value; }
        }

        protected string fDate;
        protected string tDate;

        string data;
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script language='javascript'>Page_Load()</script>");
            if (!IsPostBack)
            {

                P_stepid = "";
                P_PageNumAfterNav = null;
                Session["doneupdate"] = null;
                string intrudt = null;
                intrudt = oDBEngine.GetDate().ToString();
                fDate = oDBEngine.GetDate().ToString();
                tDate = oDBEngine.GetDate().ToString();
                dtDate.Value = Convert.ToDateTime(fDate);
                dtToDate.Value = Convert.ToDateTime(tDate);
                dtinstrudate.Value = Convert.ToDateTime(intrudt);

            }

            if (P_stepid == "1")
            {
                if (P_PageNumAfterNav != null)
                {
                    gridasset.Caption = "STEP 1 OF 3 [ Select Cheque to Print ]";
                    oAspxHelper.BindGrid(gridasset, P_PageNumAfterNav);
                }
            }

            else
            {
                if (P_PageNumAfterNav != null)
                {
                    gridasset.Caption = "STEP 1 OF 3 [ Select Cheque to Print ]";
                    oAspxHelper.BindGrid(gridasset, P_PageNumAfterNav);
                }
            }
            P_Rowid = "";
            Rowid = hdnfrequency1.Value;
            P_Rowid = Rowid;
            gridasset.JSProperties["cpproperties"] = "";
            txtbank.Attributes.Add("onkeyup", "CallAjax(this,'GenericAjaxList',event,'" + Gm.GetCashBankAccAQuery("S", Session["Usersegid"].ToString(), "CashBank") + "')");

        }
        protected void cbAll_Init(object sender, EventArgs e)
        {
            ASPxCheckBox chk = sender as ASPxCheckBox;
            ASPxGridView grid = (chk.NamingContainer as GridViewHeaderTemplateContainer).Grid;
            chk.Checked = (grid.Selection.Count == grid.VisibleRowCount);

        }

        protected void InstrumentDetail_CallbackPanel_Callback(object source, CallbackEventArgsBase e)
        {

        }
        DataSet Bind_Combo(string strcmd, string parametername, string parametervalue)
        {
            DataSet Ds = new DataSet();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]))
            {
                using (SqlCommand com = new SqlCommand(strcmd, con))
                {
                    com.Parameters.AddWithValue(parametername, parametervalue);
                    using (SqlDataAdapter Da = new SqlDataAdapter(com))
                    {
                        Da.Fill(Ds);
                    }
                }
            }
            return Ds;
        }
        protected void CmbClientBank_OnCallback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            string command = e.Parameter.Split('~')[0];
            if (command == "ClientBankBind")
            {
                SubAccountRefID = e.Parameter.Split('~')[1];
                P_subid = SubAccountRefID;
                P_voucherid = e.Parameter.Split('~')[2].ToString().Trim();
                P_transactiondate = e.Parameter.Split('~')[3].ToString().Trim();
                string StrQuery = "Select * from (select A.* , MB.bnk_id,ltrim(rtrim(MB.bnk_bankName)) as bnk_bankName,MB.bnk_BranchName,MB.bnk_micrno from (Select TCBD.cbd_id,TCBD.cbd_cntId,TCBD.cbd_bankCode, TCBD.cbd_Accountcategory,TCBD.cbd_Accountcategory as AccountType,ltrim(rtrim(TCBD.cbd_accountNumber)) as cbd_accountNumber,TCBD.cbd_accountType,cbd_accountName from tbl_trans_contactBankDetails as  TCBD where TCBD.cbd_cntId=@SubAccountCode) as A inner  join tbl_master_Bank as MB on MB.bnk_id=a.cbd_bankCode) as temp";
                DataSet Ds_CmbClientBank = Bind_Combo(StrQuery, "SubAccountCode", SubAccountRefID);
                ASPxCallbackPanel obj_CallBackPanel = (ASPxCallbackPanel)PopUp_InstrumentDetail.FindControl("InstrumentDetail_CallbackPanel");
                ASPxComboBox Obj_CmbClientBank = (ASPxComboBox)obj_CallBackPanel.FindControl("CmbClientBank");
                if (Obj_CmbClientBank.Items.Count > 0) Obj_CmbClientBank.Items.Clear();
                if (Ds_CmbClientBank.Tables.Count > 0)
                {
                    if (Ds_CmbClientBank.Tables[0].Rows.Count > 0)
                    {
                        Obj_CmbClientBank.DataSource = Ds_CmbClientBank;
                        Obj_CmbClientBank.DataBind();

                    }
                    else
                    {
                        Obj_CmbClientBank.DataSource = null;
                        Obj_CmbClientBank.DataBind();

                    }
                }
                else
                {
                    Obj_CmbClientBank.DataSource = null;
                    Obj_CmbClientBank.DataBind();

                }

            }
        }
        protected void gridasset_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            gridasset.JSProperties["cpproperties"] = "";
            P_stepid = "1";
            WhichCall = e.Parameters.Split('~')[0];
            string bankid = txtbank_hidden.Text.ToString().Split('~')[0];
            P_Bankid = bankid;
            string teststst = P_Rowid;
            string customertype = ddlbasis.SelectedItem.Value.ToString();
            //if (gridasset.VisibleColumns.Count == 5)
            //gridasset.VisibleColumns[5].Visible = true;
            if (WhichCall == "Show")
            {

                P_customertype = customertype;

                Ds_Global = Cashbank(bankid, dtDate.Text.Split('-')[2] + "-" + dtDate.Text.Split('-')[1] + "-" + dtDate.Text.Split('-')[0],
                                     dtToDate.Text.Split('-')[2] + "-" + dtToDate.Text.Split('-')[1] + "-" + dtToDate.Text.Split('-')[0],
                                     Session["LastFinYear"].ToString(), customertype, "F", "123456789", "", Session["usersegid"].ToString());
                if (Ds_Global != null)
                {
                    if (Ds_Global.Tables.Count > 0)
                    {
                        if (Ds_Global.Tables[0].Rows.Count > 0)
                        {

                            gridasset.Caption = "STEP 1 OF 3 [ Select Cheque to Print ]";
                            if (P_customertype != "C")
                            {
                                if (gridasset.VisibleColumns.Count == 6)
                                    //gridasset.VisibleColumns[5].Visible = false;
                                    gridasset.Columns["accountname"].Visible = false;
                            }
                            else
                                //gridasset.VisibleColumns[5].Visible = true;
                                gridasset.Columns["accountname"].Visible = true;

                            oAspxHelper.BindGrid(gridasset, Ds_Global);
                            if (P_customertype == "C")
                                gridasset.JSProperties["cpproperties"] = "unchk";
                            else
                                gridasset.JSProperties["cpproperties"] = "forother";
                        }
                        else
                            oAspxHelper.BindGrid(gridasset);
                    }

                    P_PageNumAfterNav = Ds_Global;
                }
                else
                {
                    if (Session["doneupdate"] != null)
                    {
                        if (Session["doneupdate"] == "update")
                        {
                            gridasset.JSProperties["cpproperties"] = "nullvaluerespective";
                            Session["doneupdate"] = null;
                        }
                    }
                    else
                    {
                        gridasset.JSProperties["cpproperties"] = "nullvalue";
                        Session["doneupdate"] = null;
                    }
                }

            }

            if (WhichCall == "Next")
            {
                int TotalRecord = gridasset.VisibleRowCount;
                string chkucc = "";
                string chkacc = "";
                string instdate = "";
                string fromdate = "";
                string todate = "";
                string custtype = "";

                string dateformatforprint = "";
                string chqno = "";
                if (chkparamacc.Checked == true)
                    chkacc = "true";
                else
                    chkacc = "false";
                if (chkparamucc.Checked == true)
                    chkucc = "true";
                else
                    chkucc = "false";
                //instdate = dtinstrudate.Date.ToString();
                instdate = dtinstrudate.Text.ToString();
                fromdate = dtDate.Text.ToString();
                todate = dtToDate.Text.ToString();
                custtype = ddlbasis.SelectedItem.Value;
                string bankii = P_Bankid;
                if (cmbdtfarmat.SelectedItem.Value == "d1")
                {

                    dateformatforprint = "true";
                }
                else
                {

                    dateformatforprint = "false";

                }
                chqno = txtchqno.Text.ToString().Trim();
                for (int i = 0; i < TotalRecord; i++)
                {
                    if (gridasset.Selection.IsRowSelected(i))
                    {
                        SelectedIds = SelectedIds + gridasset.GetRowValues(i, "CashBankDetail_ID").ToString() + ",";
                    }
                    DeSelectedIds = DeSelectedIds + gridasset.GetRowValues(i, "CashBankDetail_ID").ToString() + ",";
                }
                SelectedIds = (SelectedIds == null) ? "0" : SelectedIds.Substring(0, SelectedIds.LastIndexOf(','));
                DeSelectedIds = DeSelectedIds.Substring(0, DeSelectedIds.LastIndexOf(','));
                //Session["chequedate"]=" acc=" + chkacc + "&ucc=" + chkucc + "&date=" + instdate + "&dtformat=" + dateformat + "&chqno=" + chqno + "&rowid=" + RowID + "&bankid=" + bankid + "&fromdate=" + fromdt + "&todate=" + todt + "&custype=" + custtype";
                if (SelectedIds != "0")
                {
                    Session["chequedate"] = chkacc + "~" + chkucc + "~" + instdate + "~" + dateformatforprint + "~" + chqno + "~" + SelectedIds + "~" + P_Bankid + "~" + fromdate + "~" + todate + "~" + custtype + "~" + ddlbank.SelectedItem.Value.ToString().Trim();
                    gridasset.JSProperties["cpproperties"] = "senddata";
                }
                else
                {
                    gridasset.JSProperties["cpproperties"] = "noitem";
                }
            }
            if (WhichCall == "gridbindfrompopup")
            {
                string test = CmbClientBank.SelectedItem.Text;
                if (P_PageNumAfterNav.Tables[0].Rows.Count > 0)
                {
                    for (int M = 0; M < P_PageNumAfterNav.Tables[0].Rows.Count; M++)
                    {
                        if (P_PageNumAfterNav.Tables[0].Rows[M]["CashBankDetail_Subaccountid"].ToString() == P_subid && P_PageNumAfterNav.Tables[0].Rows[M]["cashbank_vouchernumber"].ToString().Trim() == P_voucherid && P_PageNumAfterNav.Tables[0].Rows[M]["CashBank_TransactionDate"].ToString().Trim() == P_transactiondate)
                        {
                            P_PageNumAfterNav.Tables[0].Rows[M]["accountname"] = CmbClientBank.SelectedItem.Text;
                            P_PageNumAfterNav.Tables[0].Rows[M]["cbd_id"] = CmbClientBank.SelectedItem.Value;
                            P_PageNumAfterNav.AcceptChanges();
                            break;
                        }

                    }
                }


                gridasset.Caption = "STEP 1 OF 3 [ Select Cheque to Print ]";
                //gridasset.JSProperties["cpRefreshNavPanel"] = "ShowBtnClick~1~" + TotalPage.ToString() + '~' + TotalItems.ToString();
                oAspxHelper.BindGrid(gridasset, P_PageNumAfterNav);



            }
            if (WhichCall == "Showforprevious")
            {
                gridasset.Caption = "STEP 1 OF 3 [ Select Cheque to Print ]";
                oAspxHelper.BindGrid(gridasset, P_PageNumAfterNav);
            }
            if (WhichCall == "s")
            {
                gridasset.Caption = "STEP 1 OF 3 [ Select Cheque to Print ]";
                gridasset.Settings.ShowFilterRow = true;
                oAspxHelper.BindGrid(gridasset, P_PageNumAfterNav);
            }
            if (WhichCall == "All")
            {
                gridasset.Caption = "STEP 1 OF 3 [ Select Cheque to Print ]";
                gridasset.FilterExpression = string.Empty;
                gridasset.Settings.ShowFilterRow = false;
                oAspxHelper.BindGrid(gridasset, P_PageNumAfterNav);
            }
            if (P_PageNumAfterNav != null)
                DtChequePrintSession1 = P_PageNumAfterNav.Tables[0].Copy();


        }
        protected void gridasset_HtmlRowCreated(object sender, ASPxGridViewTableRowEventArgs e)
        {

            if (e.RowType != DevExpress.Web.GridViewRowType.Data) return;
            int rowindex = e.VisibleIndex;
            DataTable dtbank = new DataTable();
            if (P_customertype == "C")
            {
                string subid = gridasset.GetRowValues(rowindex, "CashBankDetail_Subaccountid").ToString();
                string voucherid = gridasset.GetRowValues(rowindex, "cashbank_vouchernumber").ToString();
                string transactiondate = gridasset.GetRowValues(rowindex, "CashBank_TransactionDate").ToString();
                string bankcode = gridasset.GetRowValues(rowindex, "cbd_bankCode").ToString();
                if (subid != "" && bankcode != "")
                {
                    string append = "";
                    dtbank = oDBEngine.GetDataTable("select distinct LTRIM(rtrim(isnull(cbd_accountCategory,'')))+'~'+ (select ltrim(rtrim(isnull(bnk_bankName,'')))  from tbl_master_Bank where bnk_id=cbd_bankCode )+' [ '+LTRIM(rtrim(isnull(cbd_accountNumber,'')))+' ] ' from tbl_trans_contactBankDetails where  cbd_cntId='" + subid + "'");
                    if (dtbank.Rows.Count > 0)
                    {
                        if (dtbank.Rows[0][0] != "" || dtbank.Rows[0][0] != null)
                        {
                            for (int M = 0; M < dtbank.Rows.Count; M++)
                            {
                                append = append + dtbank.Rows[M][0].ToString();

                            }
                            append = "Click To Change Acc @ " + append.Replace("@", Environment.NewLine);
                        }
                    }
                    dtbank.Dispose();

                    e.Row.Cells[5].Style.Add("cursor", "hand");
                    e.Row.Cells[5].Style.Add("color", "Blue");
                    e.Row.Cells[5].Style.Add("text-decoration", "underline");
                    e.Row.Cells[5].ToolTip = append.ToString();
                    e.Row.Cells[5].Attributes.Add("onclick", "javascript:ShowInstTypePopUp('" + subid + '~' + voucherid + '~' + transactiondate + "');");
                }


            }

        }
        protected void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            Page.ClientScript.RegisterStartupScript(GetType(), "JScriptpopup", "<script language='javascript'>openpopup()</script>");
        }
        protected void gridasset_PageIndexChanging(object sender, EventArgs e)
        {
            P_stepid = "1";
            gridasset.Caption = "STEP 1 OF 3  [ Select Cheque to Print ]";
            oAspxHelper.BindGrid(gridasset, P_PageNumAfterNav);
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


            DsCashBank = BusinessLogicLayer.SQLProcedures.SelectProcedureArrDS("Data_Chequeprinting", InputName, InputType, InputValue);
            if (DsCashBank.Tables.Count > 0)
                if (DsCashBank.Tables[0].Rows.Count > 0)
                    return DsCashBank;

            return null;
        }

    }
}