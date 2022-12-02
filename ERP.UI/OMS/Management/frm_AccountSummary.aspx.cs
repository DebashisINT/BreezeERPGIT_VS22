using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
//////using DevExpress.Web;
using DevExpress.Web;
using BusinessLogicLayer;
namespace ERP.OMS.Management
{
    public partial class management_frm_AccountSummary : System.Web.UI.Page
    {
        #region Developer Veriables
        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);MULTI
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        public string strname = "";
        public string ShowList = "";
        DataTable DTTotal = new DataTable();
        Decimal StrTotalCr = 0;
        Decimal StrTotalDr = 0;
        string strOpeningCr;
        string strOpeningDr;

        Management_BL ObjMang = new Management_BL();

        #endregion

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
            //this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");
            //if (!IsPostBack)
            //{
            string str = Request.QueryString["id"].ToString();
            if (Request.QueryString["name"] != null)
            {
                strname = Request.QueryString["name"].ToString();
                if (strname.ToString() == "A")
                {
                    //Retrive Data from master main Account.
                    DataTable DTMainAccount = oDBEngine.GetDataTable("(SELECT a.MainAccount_ReferenceID as AccountRefID,a.mainAccount_Name as AccountName, a.MainAccount_accountCode as AccountCode,b.MainAccountSummary_MainAccountID as TransAccountRefID,cast(b.MainAccountSummary_OpeningCr  as decimal(18,2)) as openingCR ,cast(b.MainAccountSummary_OpeningDr  as decimal(18,2)) as openingDR , 'MainAccount'as AccountType,a.mainAccount_SubledgerType FROM Master_MainAccount as a LEFT OUTER JOIN Trans_MainAccountSummary as b ON b.MainAccountSummary_MainAccountID =a.MainAccount_ReferenceID) AS c", "*", "c.mainAccount_SubledgerType='None'");
                    //Retrive Data from Master Main Account Where subledger type="Custom"
                    //DataTable DTCustom = oDBEngine.GetDataTable("(Select a.*,b.* from Master_MainAccount as a Left outer join Master_SubAccount as b on a.MainAccount_ReferenceID=b.SubAccount_MainAcReferenceID and a.MainAccount_SubLedgerType='Custom') as c left outer join Trans_SubAccountSummary as d on c.SubAccount_MainAcReferenceID=d.SubAccountSummary_SubAccountID", "c.MainAccount_ReferenceID as AccountRefID,c.SubAccount_Name as AccountName,c.SubAccount_Code as AccountCode, c.SubAccount_MainAcReferenceID as TransAccountRefID , d.SubAccountSummary_OpeningCr as openingCR,d.SubAccountSummary_OpeningDr as openingDR ,'SubAccount'as AccountType, 'Custom' as mainAccount_SubledgerType", null);
                    DataTable DTCustom = oDBEngine.GetDataTable("MASTER_MAINACCOUNT AS MMA INNER JOIN MASTER_SUBACCOUNT AS MSA ON Cast(MMA.MAINACCOUNT_REFERENCEID as varchar)=MSA.SUBACCOUNT_MAINACREFERENCEID AND MMA.MAINACCOUNT_SUBLEDGERTYPE='Custom' LEFT OUTER JOIN TRANS_SubAccountSummary AS TRANS ON MSA.SUBACCOUNT_REFERENCEID=TRANS.SUBACCOUNTSUMMARY_SUBACCOUNTID", "MMA.MainAccount_ReferenceID as AccountRefID,MSA.SubAccount_Name as AccountName,MSA.SubAccount_Code as AccountCode,TRANS.SUBACCOUNTSUMMARY_SUBACCOUNTID AS TransAccountRefID, cast(TRANS.SubAccountSummary_OpeningCr  as decimal(18,2)) as openingCR,cast(TRANS.SubAccountSummary_OpeningDr  as decimal(18,2))as openingDR ,'SubAccount'as AccountType, MMA.mainAccount_SubledgerType as mainAccount_SubledgerType", null);

                    //Retrive Data from Master Main Account Where subledger type="Employees" or "Customers"
                    //Comment for Testing Done
                    DataTable DTEmployeeCustomers = oDBEngine.GetDataTable("(SELECT * FROM (SELECT * FROM (SELECT MMC.MainAccount_ReferenceID,MMC.MainAccount_SubLedgerType,MCT.cnttpy_contactType,MCT.cnt_prefix ,TMC.cnt_id,TMC.Cnt_internalid,TMC.cnt_branchid,TMC.cnt_firstName,TMC.cnt_middlename,TMC.cnt_lastName FROM Master_MainAccount MMC INNER JOIN tbl_master_contactType MCT ON MCT.cnttpy_contactType= MMC.MainAccount_SubLedgerType INNER JOIN tbl_master_contact TMC ON TMC.cnt_contactType=MCT.cnt_prefix WHERE MMC.MainAccount_SubLedgerType='Employees' or MMC.MainAccount_SubLedgerType='customers') AS A Left outer join Master_SubAccount as B on A.cnt_internalid=B.SubAccount_Code) AS C Left outer join Trans_SubAccountSummary as D on C.SubAccount_ReferenceID=D.SubAccountSummary_SubAccountID) AS FINALRES ", "DISTINCT FINALRES.Mainaccount_ReferenceID as AccountRefID  ,(isnull(FINALRES.cnt_firstName,'') + ' ' + isnull(FINALRES.cnt_middlename,'' )+ ' ' +isnull(FINALRES.cnt_lastName,'')) as AccountName, FINALRES.cnt_internalid as AccountCode,FINALRES.SubAccountSummary_SubAccountID as TransAccountRefID,cast(FINALRES.SubAccountSummary_OpeningDr as decimal(18,2) )as openingDR,cast(FINALRES.SubAccountSummary_OpeningCr as decimal(18,2)) as openingCR,'SubAccount'as AccountType, FINALRES.MainAccount_SubledgerType as mainAccount_SubledgerType", null);


                    //Retrive Data from Master Main Account Where subledger type= except "Employees" and "Customers"
                    //Comment for Testing.Done
                    DataTable DTnotEmployeeCustomers = oDBEngine.GetDataTable(" (SELECT * FROM (SELECT * FROM (SELECT MMC.MainAccount_ReferenceID,MMC.MainAccount_SubLedgerType,MCT.cnttpy_contactType,MCT.cnt_prefix ,TMC.cnt_id,TMC.Cnt_internalid,TMC.cnt_branchid,TMC.cnt_firstName,TMC.cnt_middlename,TMC.cnt_lastName FROM Master_MainAccount MMC INNER JOIN tbl_master_contactType MCT ON MCT.cnttpy_contactType= MMC.MainAccount_SubLedgerType INNER JOIN tbl_master_contact TMC ON TMC.cnt_contactType=MCT.cnt_prefix WHERE MMC.MainAccount_SubLedgerType!='Employees' and MMC.MainAccount_SubLedgerType!='customers') AS A Left outer join Master_SubAccount as B on A.cnt_internalid=B.SubAccount_Code) AS C Left outer join Trans_SubAccountSummary as D on C.SubAccount_ReferenceID=D.SubAccountSummary_SubAccountID) AS FINALRES", "FINALRES.Mainaccount_ReferenceID as AccountRefID,(isnull(FINALRES.cnt_firstName,'') + ' ' + Isnull(FINALRES.cnt_middlename,'') + ' ' + Isnull(FINALRES.cnt_lastName,'') ) as AccountName, FINALRES.cnt_internalid as AccountCode,FINALRES.SubAccountSummary_SubAccountID as TransAccountRefID,cast(FINALRES.SubAccountSummary_OpeningDr  as decimal(18,2)) as openingDR,cast(FINALRES.SubAccountSummary_OpeningCr  as decimal(18,2)) as openingCR,'SubAccount'as AccountType, FINALRES.MainAccount_SubledgerType as mainAccount_SubledgerType", null);

                    DTTotal.Merge(DTMainAccount, true);
                    DTTotal.Merge(DTCustom, true);

                    DTTotal.Merge(DTEmployeeCustomers, true);
                    DTTotal.Merge(DTnotEmployeeCustomers, true);
                    DTTotal.AcceptChanges();
                    //newDataSet.Tables.Add(DTTotal);
                    hdnNOR.Value = DTTotal.Rows.Count.ToString();
                    //ShowList = ShowList1();
                    ShowList2(DTTotal);

                }
                else
                {
                    if (Request.QueryString["id"] != null)
                        LoaddataForSpecific(Request.QueryString["id"].ToString());
                    else
                        Response.Write("<tr><td style='color:red'>Please Go back and come again to this page</td></tr>");
                }

            }
            else
            {

                Response.Redirect("Login.aspx", false);
            }

            //}

        }


        private void LoaddataForSpecific(string id)
        {
            DataTable DT = oDBEngine.GetDataTable("Master_MainAccount", "Mainaccount_SubledgerType as Subledger ", "MainAccount_ReferenceID='" + id.ToString() + "'");

            if (DT.Rows.Count != 0)
            {
                if (DT.Rows[0]["Subledger"].ToString() == "None")
                {
                    //No Subaccount it Should be insert in Main account, because SubLedger type is NONE.
                    DataTable Specific = oDBEngine.GetDataTable("(Select MMA.* from master_mainaccount as MMA WHERE MMA.MainAccount_ReferenceID='" + id.ToString() + "') AS A Left outer join Trans_MainAccountSummary as TMAS on A.MainAccount_ReferenceID=TMAS.MainAccountSummary_MainAccountID", "A.MainAccount_ReferenceID as AccountRefiD,A.MainAccount_Name as AccountName,A.mainAccount_AccountCode as AccountCode,TMAS.Mainaccountsummary_MainaccountID as TransAccountRefID ,cast(TMAS.MainAccountSummary_OpeningCr  as decimal(18,2)) as openingCR,cast(TMAS.MainAccountSummary_OpeningDr  as decimal(18,2)) as openingDR ,'MainAccount'as AccountType,A.MainAccount_SubledgerType", null);
                    DTTotal.Merge(Specific, true);
                    DTTotal.AcceptChanges();
                    //ShowList3(DTTotal);
                    // DTTotal.Merge(specificNone, true);
                }
                if (DT.Rows[0]["Subledger"].ToString().Trim() == "Custom")
                {
                    //No Subaccount it Should be insert in Main account, because SubLedger type is NONE.
                    DTTotal = oDBEngine.GetDataTable("( SELECT MMAA.*,MSA.* FROM (Select MMA.* from master_mainaccount as MMA WHERE MMA.MainAccount_ReferenceID='" + id.ToString() + "') AS MMAA INNER  JOIN Master_SubAccount as MSA ON MMAA.MainAccount_ReferenceID=MSA.SubAccount_MainAcReferenceID) AS MTRANSA LEFT OUTER JOIN Trans_SubAccountSummary  AS MTRANSSA ON  MTRANSA.SubAccount_ReferenceID=MTRANSSA.SubAccountSummary_SubAccountID", "MTRANSA.MainAccount_ReferenceID as AccountRefID,MTRANSA.MainAccount_name as AccountName,MTRANSA.SubAccount_Code as AccountCode,MTRANSSA.SubAccountSummary_SubAccountID as TransaccountRefID,cast(MTRANSSA.SubAccountSummary_OpeningCr  as decimal(18,2)) as openingCR,cast(MTRANSSA.SubAccountSummary_OpeningDr  as decimal(18,2))as openingDR ,'SubAccount'as AccountType, MTRANSA.MainAccount_SubledgerType", null);
                    // ShowList3(DTTotal);
                    //DTTotal.Merge(specificCustom, true);
                }
                else
                {
                    DTTotal = oDBEngine.GetDataTable("(SELECT * FROM (SELECT * FROM (SELECT MMC.MainAccount_ReferenceID,MMC.MainAccount_SubLedgerType,MCT.cnttpy_contactType,MCT.cnt_prefix ,TMC.cnt_id,TMC.cnt_internalId,TMC.cnt_branchid,TMC.cnt_accessLevel,TMC.cnt_addDate,TMC.cnt_modUserId,TMC.cnt_modDate,TMC.cnt_UCC,TMC.cnt_salutation,TMC.cnt_firstName,TMC.cnt_middleName,TMC.cnt_lastName,TMC.cnt_shortName,TMC.cnt_contactSource,TMC.cnt_contactType,TMC.cnt_legalStatus,TMC.cnt_referedBy,TMC.cnt_relation,TMC.cnt_contactStatus,TMC.cnt_speakLanguage,TMC.cnt_writeLanguage,TMC.cnt_dOB,TMC.cnt_maritalStatus,TMC.cnt_anniversaryDate,TMC.cnt_education,TMC.cnt_profession,TMC.cnt_jobResponsibility,TMC.cnt_organization,TMC.cnt_industry,TMC.cnt_designation,TMC.cnt_preferedContact,TMC.cnt_sex,TMC.cnt_UserAccess,TMC.cnt_RelationshipManager,TMC.cnt_salesRepresentative,TMC.cnt_LeadId,TMC.cnt_RegistrationDate,TMC.cnt_rating,TMC.cnt_reason,TMC.cnt_status,TMC.cnt_Lead_Stage,TMC.cnt_bloodgroup,TMC.LastModifyDate_DLMAST,TMC.LastLoginDateTime,TMC.WebLogIn,TMC.PassWord,TMC.cnt_clienttype FROM Master_MainAccount MMC INNER JOIN tbl_master_contactType MCT ON MCT.cnttpy_contactType= MMC.MainAccount_SubLedgerType INNER JOIN tbl_master_contact TMC ON TMC.cnt_contactType=MCT.cnt_prefix WHERE MMC.MainAccount_SubLedgerType!='Employees' and MMC.MainAccount_SubLedgerType!='customers') AS A Left outer join Master_SubAccount as B on A.cnt_internalid=B.SubAccount_Code) AS C Left outer join Trans_SubAccountSummary as D on C.SubAccount_ReferenceID=D.SubAccountSummary_SubAccountID) AS FINALRES", "FINALRES.Mainaccount_ReferenceID as AccountRefID,(isnull(FINALRES.cnt_firstName,'') + ' ' + Isnull(FINALRES.cnt_middlename,'') + ' ' + Isnull(FINALRES.cnt_lastName,'') ) as AccountName, FINALRES.cnt_internalid as AccountCode,FINALRES.SubAccountSummary_SubAccountID as TransAccountRefID,FINALRES.SubAccountSummary_OpeningDr as openingDR,FINALRES.SubAccountSummary_OpeningCr as openingCR,'SubAccount'as AccountType, FINALRES.MainAccount_SubledgerType as mainAccount_SubledgerType", null);
                    //DataTable  specificOther = oDBEngine.GetDataTable("(SELECT a.mainAccount_Name as AccountName, a.MainAccount_ReferenceID ,a.MainAccount_accountCode as AccountCode,b.SubAccountSummary_MainAccountID ,b.SubAccountSummary_SubAccountID,b.SubAccountSummary_OpeningCr as openingCR,b.SubAccountSummary_OpeningDr as openingDR FROM Master_MainAccount as a LEFT OUTER JOIN Trans_SubAccountSummary as b ON b.SubAccountSummary_MainAccountID =a.MainAccount_ReferenceID) AS c", "*", "c.MainAccount_ReferenceID='" + id.ToString() + "'");
                    //DTTotal.Merge(specificOther, true);


                }

                ShowList2(DTTotal);

            }
        }


        public void ShowList2(DataTable DTTotal)
        {
            StrTotalCr = 0;
            StrTotalDr = 0;

            if (DTTotal.Rows.Count != 0)
            {
                //hdnNOR-Id of a hiddenfield used for keep track Total No of records retrived from DB.

                for (int i = 0; i < 15; i++)
                {
                    TableRow tableRow = new TableRow();

                    //Account Name
                    TableCell tableCellName = new TableCell();
                    tableCellName.Width = 250;
                    Label lblName = new Label();
                    lblName.Text = DTTotal.Rows[i]["AccountName"].ToString();
                    lblName.ID = "lblName" + i;
                    lblName.Width = 150;
                    tableCellName.CssClass = "gridcellleft";
                    lblName.ForeColor = System.Drawing.Color.Black;
                    tableCellName.Controls.Add(lblName);
                    tableRow.Cells.Add(tableCellName);

                    //Account Code.
                    TableCell tableCellCode = new TableCell();
                    tableCellCode.CssClass = "gridcellleft";
                    tableCellCode.Width = 250;
                    Label lblCode = new Label();
                    lblCode.Text = DTTotal.Rows[i]["AccountCode"].ToString();
                    lblCode.ID = "lblCode" + i;
                    lblCode.Width = 150;
                    lblCode.ForeColor = System.Drawing.Color.Black;
                    tableCellCode.Controls.Add(lblCode);
                    tableRow.Cells.Add(tableCellCode);

                    //Opening DR.
                    TableCell tableCellDR = new TableCell();
                    tableCellDR.CssClass = "gridcellleft";
                    tableCellDR.Width = 250;
                    ASPxTextBox txtOpeningDR = new ASPxTextBox();
                    txtOpeningDR.MaskSettings.Mask = "<Rs. |*Rs. >" + "<0..999999g>.<00..99>";
                    txtOpeningDR.MaskSettings.IncludeLiterals = MaskIncludeLiteralsMode.DecimalSymbol;

                    if (DBNull.Value != DTTotal.Rows[i]["openingDR"])
                    {
                        txtOpeningDR.Text = DTTotal.Rows[i]["openingDR"].ToString();

                        StrTotalDr = StrTotalDr + Convert.ToDecimal(DTTotal.Rows[i]["openingDR"]);
                    }
                    else
                    {
                        txtOpeningDR.Text = "0.00";
                        StrTotalDr = StrTotalDr + 0;

                    }
                    txtOpeningDR.ID = "Rtxt" + i;
                    txtOpeningDR.ClientInstanceName = "Dtxt" + i;
                    txtOpeningDR.Width = 150;
                    txtOpeningDR.ClientSideEvents.KeyUp = "function(s,e){aaa(this,event," + txtOpeningDR.ClientID + ");}";
                    txtOpeningDR.ClientSideEvents.TextChanged = "function(s,e){Ctxt" + i.ToString() + ".SetText('000000.00');}";
                    tableCellDR.Controls.Add(txtOpeningDR);
                    tableRow.Cells.Add(tableCellDR);

                    //Opening CR.
                    TableCell tableCellCR = new TableCell();
                    tableCellCR.CssClass = "gridcellleft";
                    tableCellCR.Width = 250;
                    ASPxTextBox txtOpeningCR = new ASPxTextBox();
                    //txtOpeningCR.EnableClientSideAPI = true;
                    txtOpeningCR.MaskSettings.Mask = "<Rs. |*Rs. >" + "<0..999999g>.<00..99>";
                    txtOpeningCR.MaskSettings.IncludeLiterals = MaskIncludeLiteralsMode.DecimalSymbol;
                    if (DBNull.Value != DTTotal.Rows[i]["openingCR"])
                    {
                        txtOpeningCR.Text = DTTotal.Rows[i]["openingCR"].ToString();

                        StrTotalCr = StrTotalCr + Convert.ToDecimal(DTTotal.Rows[i]["openingCR"]);
                    }
                    else
                    {
                        txtOpeningCR.Text = "0.00";
                        //StrXYZTotalCr = StrXYZTotalCr + 0;
                        StrTotalCr = StrTotalCr + 0;
                    }
                    txtOpeningCR.ID = "Ltxt" + i;
                    txtOpeningCR.ClientInstanceName = "Ctxt" + i;
                    txtOpeningCR.Width = 150;
                    txtOpeningCR.ClientSideEvents.KeyUp = "function(s,e){aaa(this,event," + txtOpeningCR.ClientID + ");}";
                    txtOpeningCR.ClientSideEvents.TextChanged = "function(s,e){Dtxt" + i.ToString() + ".SetText('0000000.00');}";
                    tableCellCR.Controls.Add(txtOpeningCR);
                    tableRow.Cells.Add(tableCellCR);
                    //Region For Alternative color change.
                    if (i % 2 == 0)
                    {
                        tableRow.CssClass = "trnew";
                        txtOpeningCR.CssClass = "trnew";
                        txtOpeningDR.CssClass = "trnew";

                    }
                    else
                    {
                        tableRow.CssClass = "trnew1";
                        txtOpeningCR.CssClass = "trnew1";
                        txtOpeningDR.CssClass = "trnew1";

                    }

                    //Finally Add in table.
                    tblData.Rows.Add(tableRow);


                }

                txtTotalCr.Text = "";
                txtTotalDr.Text = "";
                txtTotalCr.Text = StrTotalCr.ToString();
                txtTotalDr.Text = StrTotalDr.ToString();
            }

        }


        public void ShowList1()
        {
            int StrXYZTotalCr = 0;
            int StrXYZTotalDr = 0;

            if (DTTotal.Rows.Count != 0)
            {
                //hdnNOR-Id of a hiddenfield used for keep track Total No of records retrived from DB.

                for (int i = 0; i < DTTotal.Rows.Count; i++)
                {

                    Response.Write("<tr>");

                    Response.Write("<td> <asp:HiddenField ID='HiddenField1' runat='server' /> </td>");
                    Response.Write("<td style='width: 230px'><label id=''>" + DTTotal.Rows[i]["AccountName"] + "</label></td>");
                    Response.Write(" <td style='width: 200px'><label id=''>" + DTTotal.Rows[i]["AccountCode"] + "</label></td>");

                    if (DBNull.Value != DTTotal.Rows[i]["openingCR"])
                    {
                        Response.Write("<td style='width: 225px'><input  type='text' id='Ltxt" + i + "' runat='server' value='" + DTTotal.Rows[i]["openingCR"] + "' onkeyup='javascript:aaa(this,event)'/></td>");
                        StrXYZTotalCr = StrXYZTotalCr + Convert.ToInt32(DTTotal.Rows[i]["openingCR"]);
                    }
                    else
                    {
                        Response.Write("<td style='width: 225px'><input  type='text' id='Ltxt" + i + "' runat='server'  value='" + 0 + "' onkeyup='javascript:aaa(this,event)'/></td>");
                        StrXYZTotalCr = StrXYZTotalCr + 0;
                    }
                    if (DBNull.Value != DTTotal.Rows[i]["openingDR"])
                    {
                        Response.Write("<td style='width: 225px'><input  type='text' id='Rtxt" + i + "' runat='server'  value='" + DTTotal.Rows[i]["openingDR"] + "' onkeyup='javascript:aaa(this,event)'/></td>");
                        StrXYZTotalDr = StrXYZTotalDr + Convert.ToInt32(DTTotal.Rows[i]["openingDR"]);
                    }
                    else
                    {
                        Response.Write("<td style='width: 225px'><input  type='text' id='Rtxt" + i + "' runat='server'  value='" + 0 + "' onkeyup='javascript:aaa(this,event)'/></td>");
                        StrXYZTotalDr = StrXYZTotalDr + 0;
                    }

                    Response.Write("</tr>");

                    // tblData.Rows.Add();

                    if (hdnOpeningCR.Value != "")
                    {
                        hdnOpeningCR.Value = hdnOpeningCR.Value + "," + DTTotal.Rows[i]["openingCR"].ToString();
                    }
                    else
                    {
                        hdnOpeningCR.Value = DTTotal.Rows[i]["openingDR"].ToString();
                    }
                    if (hdnOpeningDR.Value != "")
                    {
                        hdnOpeningDR.Value = hdnOpeningDR.Value + "," + DTTotal.Rows[i]["openingDR"].ToString();
                    }
                    else
                    {
                        hdnOpeningDR.Value = DTTotal.Rows[i]["openingDR"].ToString();
                    }
                }

                txtTotalCr.Text = StrXYZTotalCr.ToString();
                txtTotalDr.Text = StrXYZTotalDr.ToString();
            }

        }

        protected void BtnAdd_Click(object sender, EventArgs e)
        {
            if (DTTotal.Rows.Count != 0)
            {
                StrTotalCr = 0;
                StrTotalDr = 0;
                for (int i = 0; i < DTTotal.Rows.Count; i++)
                {
                    ASPxTextBox openingCr = (ASPxTextBox)tblData.FindControl("Ltxt" + i);
                    strOpeningCr = openingCr.Text.Remove(0, 3);
                    StrTotalCr = StrTotalCr + Convert.ToDecimal(strOpeningCr.ToString());
                    ASPxTextBox openingDr = (ASPxTextBox)tblData.FindControl("Rtxt" + i);
                    strOpeningDr = openingDr.Text.Remove(0, 3);
                    StrTotalDr = StrTotalDr + Convert.ToDecimal(strOpeningDr.ToString());

                    if (DTTotal.Rows[i]["AccountType"].ToString() == "MainAccount")
                    {
                        if (updateMainAccount(i) == 0)
                        {

                            if (openingCr.Text != "0.00" || openingDr.Text != "0.00")
                            {
                                //Insert Here.
                                InsertInTransMainAccount(i);
                            }

                        }

                    }
                    else
                    {
                        //Comment  for Test

                        if (updateSubAccount(i) == 0)
                        {
                            //TextBox openingCr = (TextBox)tblData.FindControl("Ltxt" + i);
                            //TextBox openingDr = (TextBox)tblData.FindControl("Rtxt" + i);
                            if (openingCr.Text != "0.00" || openingDr.Text != "0.00")
                            {
                                //Insert in SubAccount
                                string strSubAccountCode;
                                if (DTTotal.Rows[i]["Mainaccount_Subledgertype"].ToString().Trim() == "Custom")
                                {
                                    //Subledger Exit get SubAccount reference ID.
                                    DataTable DT = oDBEngine.GetDataTable("Master_SubAccount", "Subaccount_ReferenceID", "SubAccount_code ='" + DTTotal.Rows[i]["AccountCode"].ToString() + "'");
                                    if (DT.Rows.Count != 0)
                                    {

                                        strSubAccountCode = DT.Rows[0]["Subaccount_ReferenceID"].ToString();
                                        InsertInTransSubAccount(i, strSubAccountCode);
                                    }
                                }
                                else
                                {
                                    strSubAccountCode = CreateSubAccount(i);
                                    InsertInTransSubAccount(i, strSubAccountCode);
                                    //Insert in Trans SubAccount

                                }



                            }

                        }


                    }

                }
                txtTotalCr.Text = StrTotalCr.ToString();
                txtTotalDr.Text = StrTotalDr.ToString();
            }


        }

        protected void BtnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("AccountSummary.aspx");

        }
        private void InsertInTransSubAccount(int i, string strSubAccountCode)
        {


            ASPxTextBox openingCr = (ASPxTextBox)tblData.FindControl("Ltxt" + i);
            ASPxTextBox openingDr = (ASPxTextBox)tblData.FindControl("Rtxt" + i);
            strOpeningCr = openingCr.Text.Remove(0, 3);
            strOpeningDr = openingDr.Text.Remove(0, 3);

            /* For Tier Structure */

            //SqlConnection lcon = new SqlConnection(ConfigurationSettings.AppSettings["DBConnectionDefault"]);MULTI
            SqlConnection lcon = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            lcon.Open();
            SqlCommand lcmd = new SqlCommand("InsertTransSubAccount", lcon);
            lcmd.CommandType = CommandType.StoredProcedure;


            lcmd.Parameters.Add("@FinancialYear", SqlDbType.VarChar).Value = "2009-2010";
            lcmd.Parameters.Add("@MainAccount_ReferenceID", SqlDbType.BigInt).Value = Convert.ToInt64(DTTotal.Rows[i]["AccountRefID"].ToString());
            lcmd.Parameters.Add("@SubAccount_ReferenceID", SqlDbType.BigInt).Value = Convert.ToInt64(strSubAccountCode.ToString());
            lcmd.Parameters.Add("@openingCR", SqlDbType.Decimal).Value = Convert.ToDecimal(strOpeningCr.ToString());
            lcmd.Parameters.Add("@openingDR", SqlDbType.Decimal).Value = Convert.ToDecimal(strOpeningDr.ToString());


            int NoOfRowEffected = lcmd.ExecuteNonQuery();
            lcmd.Dispose();
            lcon.Close();
            lcon.Dispose();





            //int NoOfRowEffected = ObjMang.InsertTransSubAccount("2009-2010",DTTotal.Rows[i]["AccountRefID"].ToString(),strSubAccountCode.ToString(),
            //                                   strOpeningCr.ToString(),strOpeningDr.ToString());




        }

        private string CreateSubAccount(int i)
        {

            Label Name = (Label)tblData.FindControl("lblName" + i);
            Label Code = (Label)tblData.FindControl("lblCode" + i);

            /*  */
            //SqlConnection lcon = new SqlConnection(ConfigurationSettings.AppSettings["DBConnectionDefault"]);MULTI
            SqlConnection lcon = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            lcon.Open();
            SqlCommand lcmd = new SqlCommand("CreateSubAccount", lcon);
            lcmd.CommandType = CommandType.StoredProcedure;

            lcmd.Parameters.Add("@SubAccount_MainAcReferenceID", SqlDbType.BigInt).Value = DTTotal.Rows[i]["AccountRefID"];
            lcmd.Parameters.Add("@SubAccount_Name", SqlDbType.VarChar).Value = Name.Text.ToString();
            lcmd.Parameters.Add("@SubAccount_Code", SqlDbType.VarChar).Value = Code.Text.ToString();
            lcmd.Parameters.Add("@id", SqlDbType.BigInt).Direction = ParameterDirection.Output;
            int NoOfRowEffected = lcmd.ExecuteNonQuery();
            string SubAccountID = lcmd.Parameters[3].Value.ToString();
            lcmd.Dispose();
            lcon.Close();
            lcon.Dispose();




            // string SubAccountID=  ObjMang.CreateSubAccount(DTTotal.Rows[i]["AccountRefID"].ToString(), Name.Text.ToString(), Code.Text.ToString());

            return SubAccountID;


        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="intvarValue"></param>
        private void InsertInTransMainAccount(int intvarValue)
        {

            ASPxTextBox openingCr = (ASPxTextBox)tblData.FindControl("Ltxt" + intvarValue);
            ASPxTextBox openingDr = (ASPxTextBox)tblData.FindControl("Rtxt" + intvarValue);
            strOpeningCr = openingCr.Text.Remove(0, 3);
            strOpeningDr = openingDr.Text.Remove(0, 3);

            /* */
            //SqlConnection lcon = new SqlConnection(ConfigurationSettings.AppSettings["DBConnectionDefault"]);MULTI
            SqlConnection lcon = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            lcon.Open();
            SqlCommand lcmd = new SqlCommand("InsertTransMainAccountSummary", lcon);
            lcmd.CommandType = CommandType.StoredProcedure;
            lcmd.Parameters.Add("@FinancialYear", SqlDbType.VarChar).Value = "2009-2010";
            lcmd.Parameters.Add("@MainAccount_ReferenceID", SqlDbType.BigInt).Value = Convert.ToInt64(DTTotal.Rows[intvarValue]["AccountRefID"].ToString());
            lcmd.Parameters.Add("@openingCR", SqlDbType.Decimal).Value = Convert.ToDecimal(strOpeningCr.ToString());
            lcmd.Parameters.Add("@openingDR", SqlDbType.Decimal).Value = Convert.ToDecimal(strOpeningDr.ToString());
            int NoOfRowEffected = lcmd.ExecuteNonQuery();
            lcmd.Dispose();
            lcon.Close();
            lcon.Dispose();




            //int NoOfRowEffected = ObjMang.InsertTransMainAccountSummary("2009-2010", DTTotal.Rows[intvarValue]["AccountRefID"].ToString(),
            //                                   strOpeningCr.ToString(), strOpeningDr.ToString());


        }

        private int updateMainAccount(int intvarValue)
        {

            ASPxTextBox openingCr = (ASPxTextBox)tblData.FindControl("Ltxt" + intvarValue);
            ASPxTextBox openingDr = (ASPxTextBox)tblData.FindControl("Rtxt" + intvarValue);
            strOpeningCr = openingCr.Text.Remove(0, 3);
            strOpeningDr = openingDr.Text.Remove(0, 3);

            /* For Tier Structure   
            */

            //SqlConnection lcon = new SqlConnection(ConfigurationSettings.AppSettings["DBConnectionDefault"]);MULTI
            SqlConnection lcon = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            lcon.Open();
            SqlCommand lcmd = new SqlCommand("UpDateAccountSummary", lcon);
            lcmd.CommandType = CommandType.StoredProcedure;
            lcmd.Parameters.Add("@Module", SqlDbType.VarChar).Value = "UpdateMainAccount";
            lcmd.Parameters.Add("@FinancialYear", SqlDbType.VarChar).Value = "2009-2010";
            lcmd.Parameters.Add("@AccountRefID", SqlDbType.BigInt).Value = DTTotal.Rows[intvarValue]["AccountRefID"];
            lcmd.Parameters.Add("@openingCR", SqlDbType.Decimal).Value = Convert.ToDecimal(strOpeningCr.ToString());
            lcmd.Parameters.Add("@openingDR", SqlDbType.Decimal).Value = Convert.ToDecimal(strOpeningDr.ToString());
            int NoOfRowEffected = lcmd.ExecuteNonQuery();
            lcmd.Dispose();
            lcon.Close();
            lcon.Dispose();



            //int NoOfRowEffected  =ObjMang.UpDateAccountSummary("UpdateMainAccount", "2009-2010", DTTotal.Rows[intvarValue]["AccountRefID"].ToString(),
            //                              strOpeningCr.ToString(), strOpeningDr.ToString());

            return NoOfRowEffected;

        }

        private int updateSubAccount(int intvarValue)
        {


            ASPxTextBox openingCr = (ASPxTextBox)tblData.FindControl("Ltxt" + intvarValue);
            ASPxTextBox openingDr = (ASPxTextBox)tblData.FindControl("Rtxt" + intvarValue);
            strOpeningCr = openingCr.Text.Remove(0, 3);
            strOpeningDr = openingDr.Text.Remove(0, 3);

            /* For Tier Structure  */

            //SqlConnection lcon = new SqlConnection(ConfigurationSettings.AppSettings["DBConnectionDefault"]);MULTI
            SqlConnection lcon = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            lcon.Open();
            SqlCommand lcmd = new SqlCommand("UpDateAccountSummary", lcon);
            lcmd.CommandType = CommandType.StoredProcedure;
            lcmd.Parameters.Add("@Module", SqlDbType.VarChar).Value = "UpDateSubAccountSummary";
            lcmd.Parameters.Add("@FinancialYear", SqlDbType.VarChar).Value = "2009-2010";
            if (DTTotal.Rows[intvarValue]["TransAccountRefID"] != null)
            {
                lcmd.Parameters.Add("@AccountRefID", SqlDbType.BigInt).Value = DTTotal.Rows[intvarValue]["TransAccountRefID"];


                lcmd.Parameters.Add("@openingDR", SqlDbType.VarChar).Value = Convert.ToDecimal(strOpeningCr.ToString());
                lcmd.Parameters.Add("@openingCR", SqlDbType.VarChar).Value = Convert.ToDecimal(strOpeningDr.ToString());
                int NoOfRowEffected = lcmd.ExecuteNonQuery();
                lcmd.Dispose();
                lcon.Close();
                lcon.Dispose();

                return NoOfRowEffected;
            }
            else
                return 0;


            //---------------------------------------------------------------------------


            //if (DTTotal.Rows[intvarValue]["TransAccountRefID"] != null)
            //{

            //    int NoOfRowEffected = ObjMang.UpDateAccountSummary("UpDateSubAccountSummary", "2009-2010",
            //        DTTotal.Rows[intvarValue]["TransAccountRefID"].ToString(),
            //                             strOpeningCr.ToString(), strOpeningDr.ToString());

            //    return NoOfRowEffected;
            //}
            //else
            //    return 0;

        }
    }

}













