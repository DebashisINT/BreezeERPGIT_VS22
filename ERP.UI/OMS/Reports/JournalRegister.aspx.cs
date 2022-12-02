using System;
using System.Data;
using System.Web.UI;
//using DevExpress.Web;
using DevExpress.Web;
using System.Configuration;
using BusinessLogicLayer;


namespace ERP.OMS.Reports
{

    public partial class Reports_JournalRegister : System.Web.UI.Page, System.Web.UI.ICallbackEventHandler
    {
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        DataTable dtparent = new DataTable();
        DataTable dtchild = new DataTable();
        string GenerationType = null;
        string BranchID = null;
        string SegmentID = null;
        string EntryUserID = null;
        string BranchID1 = null;
        string SegmentID1 = null;
        string EntryUserID1 = null;
        string VoucherDate = null;
        string segmentname = null;
        string VoucherNumber = null;
        string exchangesegment = "";
        string branchhrch = null;
        string prefix = null;
        static string Branch;
        static string Segment;
        static string EntryUser;
        DataSet Ds_Global = null;
        string data;


        AspxHelper oAspxHelper = null;
        public DataTable Dtjournal
        {
            get { return (DataTable)Session["journaltable"]; }
            set { Session["journaltable"] = value; }
        }
        public DataTable Dtjournalchild
        {
            get { return (DataTable)Session["journaltablechild"]; }
            set { Session["journaltablechild"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            AspxHelper oAspxHelper = new AspxHelper();
            Page.ClientScript.RegisterStartupScript(GetType(), "JS", "<script language='JavaScript'>Page_Load();</script>");
            if (!IsPostBack)
            {
                string fDate = null;
                string tDate = null;
                fDate = Convert.ToString(Session["FinYearStart"]);
                tDate = Convert.ToString(Session["FinYearEnd"]);
                dtFrom.Value = Convert.ToDateTime(fDate);
                dtTo.Value = Convert.ToDateTime(tDate);
                Dtjournal = null;
                Dtjournalchild = null;
                //dtFrom.EditFormatString = oconverter.GetDateFormat("Date");
                //dtTo.EditFormatString = oconverter.GetDateFormat("Date");
            }
            //grid.DetailRows.ExpandRow(0);
            if (Dtjournal != null)
            {
                if (Dtjournal.Rows.Count > 0)
                {
                    grid.Caption = "SUMMARY VIEW OF JOURNAL REGISTER";
                    oAspxHelper.BindGrid(Dtjournal, grid);
                }
            }
            String cbReference = Page.ClientScript.GetCallbackEventReference(this, "arg", "ReceiveServerData", "context");
            String callbackScript = "function CallServer(arg, context){ " + cbReference + ";}";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "CallServer", callbackScript, true);

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
            string[] cl = idlist[1].Split(',');
            for (int i = 0; i < cl.Length; i++)
            {
                string[] val = cl[i].Split(';');
                if (str == "")
                {
                    if (idlist[0] == "EntryUser")
                    {
                        str = val[0];
                        str1 = val[0] + ";" + val[1];
                    }
                    else
                    {
                        str = "'" + val[0] + "'";
                        str1 = val[0] + ";" + val[1];
                    }
                }
                else
                {
                    if (idlist[0] == "EntryUser")
                    {
                        str += "," + val[0];
                        str1 += "," + val[0] + ";" + val[1];
                    }
                    else
                    {
                        str += ",'" + val[0] + "'";
                        str1 += "," + val[0] + ";" + val[1];
                    }
                }
            }
            if (idlist[0] == "Branch")
            {
                Branch = str;
                data = "Branch~" + str1;
            }
            else if (idlist[0] == "Segment")
            {
                Segment = str;
                data = "Segment~" + str1;
            }
            else if (idlist[0] == "EntryUser")
            {
                EntryUser = str;
                data = "EntryUser~" + str1;
            }
        }
        protected void grid_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            AspxHelper oAspxHelper = new AspxHelper();
            Ds_Global = new DataSet();
            string whichcall = e.Parameters.Split('~')[0];
            grid.JSProperties["cpproperties"] = "";
            if (whichcall == "Show")
            {
                if (e.Parameters.Split('~')[1] != "EX")
                {
                    //oAspxHelper.BindGrid(grid);
                    if (chkIgnoreSystem.Checked == true)
                        GenerationType = "Y";
                    else
                        GenerationType = "N";
                    if (radSpecific.Checked == true)
                        prefix = txtAccountCode.Text.ToString().Trim().ToUpper();
                    else
                        prefix = "N";

                    if (RadSegmentAll.Checked == true)
                    {
                        SegmentID1 = "A";
                        SegmentID = "A";

                    }
                    else
                    {
                        if (Segment == null)
                        {
                            SegmentID1 = "B";
                            SegmentID = "B";
                            exchangesegment = Session["usersegid"].ToString();
                        }
                        else
                        {
                            SegmentID1 = "C";
                            SegmentID = "C";
                            exchangesegment = Segment.ToString().Trim();
                        }
                    }
                    if (RadBranchAll.Checked == true)
                    {

                        branchhrch = Session["userbranchHierarchy"].ToString();
                        BranchID1 = "A";
                        BranchID = "A";
                    }
                    else
                    {
                        BranchID1 = "B";
                        branchhrch = Branch;
                        BranchID = "B";
                    }
                    if (RadEntryAll.Checked == true)
                    {
                        EntryUserID1 = "N";
                        EntryUserID = "N";
                    }
                    else
                    {
                        EntryUserID = EntryUser;
                        EntryUserID1 = EntryUser;
                    }

                    Ds_Global = Cashbank(dtFrom.Value.ToString(), dtTo.Value.ToString(), GenerationType, prefix, SegmentID1, SegmentID, BranchID1, BranchID, EntryUserID, EntryUserID1, Session["LastCompany"].ToString().Trim(), exchangesegment, branchhrch, "SC");
                    if (Ds_Global != null)
                    {
                        if (Ds_Global.Tables.Count > 0)
                        {
                            if (Ds_Global.Tables[0].Rows.Count > 0)
                            {
                                Dtjournal = Ds_Global.Tables[0];
                                Dtjournalchild = Ds_Global.Tables[1];
                                grid.Caption = "SUMMARY VIEW OF JOURNAL REGISTER";
                                oAspxHelper.BindGrid(Dtjournal, grid);
                                grid.JSProperties["cpproperties"] = "withvalue";
                                grid.DetailRows.ExpandRow(0);
                            }
                        }
                    }
                    else
                    {
                        grid.JSProperties["cpproperties"] = "nullvalue";
                    }
                    // dtparent = oDBEngine.GetDataTable("Trans_JournalVoucherDetail", "convert(varchar(11),JournalVoucherDetail_TransactionDate,113) as Date,JournalVoucherDetail_VoucherNumber,(select top 1 MainAccount_Name from Master_MainAccount where MainAccount_AccountCode=Trans_JournalVoucherDetail.JournalVoucherDetail_MainAccountCode)+isnull((case when JournalVoucherDetail_SubAccountCode is null then '' else (' [' + (isnull((select isnull(ltrim(rtrim(cnt_firstname)),'')+' '+isnull(ltrim(rtrim(cnt_middlename)),'')+' '+isnull(ltrim(rtrim(cnt_lastname)),'')+' ['+isnull(ltrim(rtrim(cnt_ucc)),'')+']' from tbl_master_contact where cnt_internalid=Trans_JournalVoucherDetail.JournalVoucherDetail_SubAccountCode ),isnull((select top 1 subaccount_name+' ['+isnull(ltrim(rtrim(subaccount_code)),'')+']' from master_subaccount where cast(subaccount_code as varchar)=Trans_JournalVoucherDetail.JournalVoucherDetail_SubAccountCode and SubAccount_MainAcReferenceID=Trans_JournalVoucherDetail.JournalVoucherDetail_MainAccountCode),isnull((select subaccount_name+' ['+isnull(ltrim(rtrim(subaccount_code)),'')+']' from master_subaccount where cast(subaccount_referenceid as varchar)=Trans_JournalVoucherDetail.JournalVoucherDetail_SubAccountCode and SubAccount_MainAcReferenceID=Trans_JournalVoucherDetail.JournalVoucherDetail_MainAccountCode),isnull((select top 1 cdslclients_firstholdername+'['+isnull(ltrim(rtrim(cdslclients_benaccountnumber)),'')+']' from master_cdslclients where cast(cdslclients_benaccountnumber as varchar)=Trans_JournalVoucherDetail.JournalVoucherDetail_SubAccountCode),(select nsdlclients_benfirstholdername+' ['+isnull(ltrim(rtrim(nsdlclients_benaccountid)),'')+']' from master_nsdlclients where cast(nsdlclients_benaccountid as varchar)=Trans_JournalVoucherDetail.JournalVoucherDetail_SubAccountCode))))))+']') end),'') as AccountName,isnull((select top 1 JournalVoucher_Narration from Trans_JournalVoucher where JournalVoucher_VoucherNumber=Trans_JournalVoucherDetail.JournalVoucherDetail_VoucherNumber and JournalVoucher_TransactionDate=cast(DATEADD(dd, 0, DATEDIFF(dd, 0,Trans_JournalVoucherDetail.JournalVoucherDetail_TransactionDate)) as datetime) and JournalVoucher_ExchangeSegmentID=JournalVoucherDetail_ExchangeSegmentID),'')+' |'+isnull(JournalVoucherDetail_Narration,'') as Description,case when JournalVoucherDetail_AmountDr=0 then null else JournalVoucherDetail_AmountDr end as AmountDR,case when JournalVoucherDetail_AmountCR=0 then null else JournalVoucherDetail_AmountCR end as AmountCR,(select isnull(((select exh_shortName from tbl_master_Exchange where exh_cntId=exch_exchId)+'-'+exch_segmentId),exch_membershipType) from tbl_master_CompanyExchange where exch_internalID=JournalVoucherDetail_ExchangeSegmentID) as SegmentName", " JournalVoucherDetail_VoucherNumber in(select JournalVoucher_VoucherNumber from Trans_JournalVoucher where " + GenerationType + " " + SegmentID1 + " " + EntryUserID1 + " " + BranchID1 + " " + prefix + " and JournalVoucher_TransactionDate between cast(DATEADD(dd, 0, DATEDIFF(dd, 0,'" + dtFrom.Value + "')) as datetime) and cast(DATEADD(dd, 0, DATEDIFF(dd, 0,'" + dtTo.Value + "')) as datetime)) " + SegmentID + " " + BranchID + " " + EntryUserID + " and JournalVoucherDetail_TransactionDate between cast(DATEADD(dd, 0, DATEDIFF(dd, 0,'" + dtFrom.Value + "')) as datetime) and cast(DATEADD(dd, 0, DATEDIFF(dd, 0,'" + dtTo.Value + "')) as datetime)", " JournalVoucherDetail_TransactionDate,JournalVoucherDetail_VoucherNumber,JournalVoucherDetail_ExchangeSegmentID");

                }
                else
                {
                    grid.JSProperties["cpproperties"] = "Export";
                }

            }
            if (whichcall == "s")
            {
                grid.Caption = "SUMMARY VIEW OF JOURNAL REGISTER";
                grid.Settings.ShowFilterRow = true;
                oAspxHelper.BindGrid(Dtjournal, grid);
            }
            if (whichcall == "All")
            {
                grid.Caption = "SUMMARY VIEW OF JOURNAL REGISTER";
                grid.FilterExpression = string.Empty;
                grid.Settings.ShowFilterRow = false;
                oAspxHelper.BindGrid(Dtjournal, grid);
            }
        }

        protected void detailGrid_Init(object sender, EventArgs e)
        {
            ASPxGridView childGrid = sender as ASPxGridView;
            DataTable dtchildbind = new DataTable();
            dtchildbind = Dtjournalchild.Copy();
            string date = childGrid.GetMasterRowFieldValues("date").ToString();
            string voucherno = childGrid.GetMasterRowFieldValues("voucherno").ToString();
            string segmentname = childGrid.GetMasterRowFieldValues("segname").ToString();


            //dtchild = oDBEngine.GetDataTable("create table #tmpjournal (date varchar(50),voucherno varchar(50),accountname varchar(max),description varchar(max),amntdr numeric(28,2),amntcr numeric(28,2),segname varchar(50))insert into #tmpjournal Select convert(varchar(11),JournalVoucherDetail_TransactionDate,113) as Date,JournalVoucherDetail_VoucherNumber, (select top 1 MainAccount_Name from Master_MainAccount where MainAccount_AccountCode=Trans_JournalVoucherDetail.JournalVoucherDetail_MainAccountCode)+isnull((case when JournalVoucherDetail_SubAccountCode is null then '' else (' [' + (isnull((select isnull(ltrim(rtrim(cnt_firstname)),'')+' '+isnull(ltrim(rtrim(cnt_middlename)),'')+' '+isnull(ltrim(rtrim(cnt_lastname)),'')+' ['+isnull(ltrim(rtrim(cnt_ucc)),'')+']' from tbl_master_contact where cnt_internalid=Trans_JournalVoucherDetail.JournalVoucherDetail_SubAccountCode ),isnull((select top 1 subaccount_name+' ['+isnull(ltrim(rtrim(subaccount_code)),'')+']' from master_subaccount where cast(subaccount_code as varchar)=Trans_JournalVoucherDetail.JournalVoucherDetail_SubAccountCode and SubAccount_MainAcReferenceID=Trans_JournalVoucherDetail.JournalVoucherDetail_MainAccountCode),isnull((select subaccount_name+' ['+isnull(ltrim(rtrim(subaccount_code)),'')+']' from master_subaccount where cast(subaccount_referenceid as varchar)=Trans_JournalVoucherDetail.JournalVoucherDetail_SubAccountCode and SubAccount_MainAcReferenceID=Trans_JournalVoucherDetail.JournalVoucherDetail_MainAccountCode),isnull((select top 1 cdslclients_firstholdername+'['+isnull(ltrim(rtrim(cdslclients_benaccountnumber)),'')+']' from master_cdslclients where cast(cdslclients_benaccountnumber as varchar)=Trans_JournalVoucherDetail.JournalVoucherDetail_SubAccountCode),(select nsdlclients_benfirstholdername+' ['+isnull(ltrim(rtrim(nsdlclients_benaccountid)),'')+']' from master_nsdlclients where cast(nsdlclients_benaccountid as varchar)=Trans_JournalVoucherDetail.JournalVoucherDetail_SubAccountCode))))))+']') end),'') as AccountName,isnull((select top 1 JournalVoucher_Narration from Trans_JournalVoucher where JournalVoucher_VoucherNumber=Trans_JournalVoucherDetail.JournalVoucherDetail_VoucherNumber and JournalVoucher_TransactionDate=cast(DATEADD(dd, 0, DATEDIFF(dd, 0,Trans_JournalVoucherDetail.JournalVoucherDetail_TransactionDate)) as datetime) and JournalVoucher_ExchangeSegmentID=JournalVoucherDetail_ExchangeSegmentID),'')+' |'+isnull(JournalVoucherDetail_Narration,'') as Description,case when JournalVoucherDetail_AmountDr=0 then null else JournalVoucherDetail_AmountDr end as AmountDR,case when JournalVoucherDetail_AmountCR=0 then null else JournalVoucherDetail_AmountCR end as AmountCR,(select isnull(((select exh_shortName from tbl_master_Exchange where exh_cntId=exch_exchId)+'-'+exch_segmentId),exch_membershipType) from tbl_master_CompanyExchange where exch_internalID=JournalVoucherDetail_ExchangeSegmentID) as SegmentName from Trans_JournalVoucherDetail WHERE  JournalVoucherDetail_VoucherNumber in(select JournalVoucher_VoucherNumber from Trans_JournalVoucher where  JournalVoucher_Prefix not in(select VoucherType_Code from Master_VoucherType)   and JournalVoucher_ExchangeSegmentID in(select A.EXCH_INTERNALID AS SEGMENTID from (SELECT TMCE.* FROM TBL_MASTER_COMPANYEXCHANGE AS TMCE WHERE  TMCE.EXCH_COMPID='COR0000002' ) AS A LEFT OUTER JOIN   TBL_MASTER_EXCHANGE AS TME ON TME.EXH_CNTID=A.EXCH_EXCHID )   and JournalVoucher_BranchID in(select  branch_id from tbl_master_branch where branch_id in(130,131,133,134,135,137,139,142,143,145,149,129,150,153,157,161,162,164,166,167,169,170,174,178,180,185,186,188,189,190,191,192,193,196,197,198,199,200,204,1))  and JournalVoucher_TransactionDate between cast(DATEADD(dd, 0, DATEDIFF(dd, 0,'4/1/2012 12:00:00 AM')) as datetime) and cast(DATEADD(dd, 0, DATEDIFF(dd, 0,'4/20/2012 12:00:00 AM')) as datetime))  and JournalVoucherDetail_ExchangeSegmentID in(select A.EXCH_INTERNALID AS SEGMENTID from (SELECT TMCE.* FROM TBL_MASTER_COMPANYEXCHANGE AS TMCE WHERE  TMCE.EXCH_COMPID='COR0000002' ) AS A LEFT OUTER JOIN   TBL_MASTER_EXCHANGE AS TME ON TME.EXH_CNTID=A.EXCH_EXCHID )  and JournalVoucherDetail_BranchID in(select  branch_id from tbl_master_branch where branch_id in(130,131,133,134,135,137,139,142,143,145,149,129,150,153,157,161,162,164,166,167,169,170,174,178,180,185,186,188,189,190,191,192,193,196,197,198,199,200,204,1))  and JournalVoucherDetail_TransactionDate between cast(DATEADD(dd, 0, DATEDIFF(dd, 0,'4/1/2012 12:00:00 AM')) as datetime) and cast(DATEADD(dd, 0, DATEDIFF(dd, 0,'4/20/2012 12:00:00 AM')) as datetime) Order By  JournalVoucherDetail_TransactionDate,JournalVoucherDetail_VoucherNumber,JournalVoucherDetail_ExchangeSegmentID select * from #tmpjournal drop table #tmpjournal");
            for (int pom = 0; pom < dtchildbind.Rows.Count; pom++)
            {
                if (dtchildbind.Rows[pom]["date"].ToString().Trim() == date.ToString().Trim() && dtchildbind.Rows[pom]["voucherno"].ToString().Trim() == voucherno.ToString().Trim() && dtchildbind.Rows[pom]["segname"].ToString().Trim() == segmentname.ToString().Trim())
                {



                }
                else
                {
                    dtchildbind.Rows[pom].Delete();
                }
            }
            dtchildbind.AcceptChanges();

            childGrid.Caption = "DETAIL VIEW OF JOURNAL REGISTER";
            childGrid.DataSource = dtchildbind;
            childGrid.DataBind();
        }
        DataSet Cashbank(string fromdate, string todate, string generationtype, string prefix, string segmentid1,
                        string segmentid, string branchID1, string branchID, string entryUserID, string entryUserID1,
                        string companyid, string exchangesegment, string branchhrchy, string outputtype)
        {
            string[] InputName = new string[14];
            string[] InputType = new string[14];
            string[] InputValue = new string[14];

            DataSet DsCashBank = new DataSet();
            InputName[0] = "fromdate";
            InputName[1] = "todate";
            InputName[2] = "GenerationType";
            InputName[3] = "prefix";
            InputName[4] = "SegmentID1";
            InputName[5] = "SegmentID";
            InputName[6] = "BranchID1";
            InputName[7] = "BranchID";
            InputName[8] = "EntryUserID";
            InputName[9] = "EntryUserID1";
            InputName[10] = "companyid";
            InputName[11] = "exchangesegment";
            InputName[12] = "Branchhrch";
            InputName[13] = "OutputType";


            InputType[0] = "V";
            InputType[1] = "V";
            InputType[2] = "V";
            InputType[3] = "V";
            InputType[4] = "V";
            InputType[5] = "V";
            InputType[6] = "V";
            InputType[7] = "V";
            InputType[8] = "V";
            InputType[9] = "V";
            InputType[10] = "V";
            InputType[11] = "V";
            InputType[12] = "V";
            InputType[13] = "V";


            InputValue[0] = fromdate;
            InputValue[1] = todate;
            InputValue[2] = generationtype;
            InputValue[3] = prefix;
            InputValue[4] = SegmentID1;
            InputValue[5] = segmentid;
            InputValue[6] = branchID1;
            InputValue[7] = branchID;
            InputValue[8] = entryUserID;
            InputValue[9] = entryUserID1;
            InputValue[10] = companyid;
            InputValue[11] = exchangesegment;
            InputValue[12] = branchhrchy;
            InputValue[13] = outputtype;


            DsCashBank = BusinessLogicLayer.SQLProcedures.SelectProcedureArrDS("Fetch_journalregisterhead", InputName, InputType, InputValue);
            if (DsCashBank.Tables.Count > 0)
                if (DsCashBank.Tables[0].Rows.Count > 0)


                    return DsCashBank;



            return null;
        }
        protected void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            ExcelFile objExcel = new ExcelFile();
            //ExcelFile objExcel = new ExcelFile();
            string searchCriteria = null;
            BusinessLogicLayer.Converter oconverter = new BusinessLogicLayer.Converter();
            searchCriteria = "Period " + oconverter.ArrangeDate2(dtFrom.Value.ToString()) + " - " + oconverter.ArrangeDate2(dtTo.Value.ToString());
            if (chkIgnoreSystem.Checked == true)
            {
                GenerationType = "Y";
                //searchCriteria += "";
            }
            else
                GenerationType = "N";
            if (radSpecific.Checked == true)
                prefix = txtAccountCode.Text.ToString().Trim().ToUpper();
            else
                prefix = "N";

            if (RadSegmentAll.Checked == true)
            {
                SegmentID1 = "A";
                SegmentID = "A";

            }
            else
            {
                if (Segment == null)
                {
                    SegmentID1 = "B";
                    SegmentID = "B";
                    exchangesegment = Session["usersegid"].ToString();
                }
                else
                {
                    SegmentID1 = "C";
                    SegmentID = "C";
                    exchangesegment = Segment.ToString().Trim();
                }
            }
            if (RadBranchAll.Checked == true)
            {

                branchhrch = Session["userbranchHierarchy"].ToString();
                BranchID1 = "A";
                BranchID = "A";
            }
            else
            {
                BranchID1 = "B";
                branchhrch = Branch;
                BranchID = "B";
            }
            if (RadEntryAll.Checked == true)
            {
                EntryUserID1 = "N";
                EntryUserID = "N";
            }
            else
            {
                EntryUserID = EntryUser;
                EntryUserID1 = EntryUser;
            }

            Ds_Global = Cashbank(dtFrom.Value.ToString(), dtTo.Value.ToString(), GenerationType, prefix, SegmentID1, SegmentID, BranchID1, BranchID, EntryUserID, EntryUserID1, Session["LastCompany"].ToString().Trim(), exchangesegment, branchhrch, "EX");
            DataTable dtExport = Ds_Global.Tables[0].Copy();
            BusinessLogicLayer.GenericExcelExport oGenericExcelExport = new BusinessLogicLayer.GenericExcelExport();
            string strDownloadFileName = "";
            string exlDateTime = oDBEngine.GetDate(113).ToString();
            string exlTime = exlDateTime.Replace(":", "");
            exlTime = exlTime.Replace(" ", "");
            string FileName = "JournalRegister_" + exlTime;
            strDownloadFileName = "~/Documents/";
            DataTable dtcompany = oDBEngine.GetDataTable("select ltrim(rtrim(cmp_Name)) as company from tbl_master_company where cmp_internalid ='" + Session["LastCompany"].ToString() + "'");
            string[] strHead = new string[3];
            strHead[0] = exlDateTime;
            strHead[1] = searchCriteria;
            strHead[2] = "Journal Register Of " + dtcompany.Rows[0]["company"];
            string ExcelVersion = "2007";                                                                 //Lots
            string[] ColumnType = { "V", "V", "V", "V", "N", "N" };
            string[] ColumnSize = { "20", "50", "200", "100", "20,2", "20,2" };
            string[] ColumnWidthSize = { "10", "18", "30", "30", "15", "15" };
            oGenericExcelExport.ExportToExcel(ColumnType, ColumnSize, ColumnWidthSize, dtExport, Server.MapPath(strDownloadFileName), ExcelVersion, FileName, strHead, null);
            //objExcel.ExportToPDF(dtExport, "Journal Register", "Total", dtcompany, dtcompany);
        }


    }
}