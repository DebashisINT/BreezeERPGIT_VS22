using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using BusinessLogicLayer;

namespace ERP.OMS.Reports
{
    public partial class Reports_PortfolioPerformanceFO_New : System.Web.UI.Page, System.Web.UI.ICallbackEventHandler
    {
        BusinessLogicLayer.Converter oconverter = new BusinessLogicLayer.Converter();
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        DataSet ds = new DataSet();
        DataSet ds1 = new DataSet();
        DataTable dtgroupcontactid = new DataTable();
        DataTable Distinctclient = new DataTable();
        ExcelFile objExcel = new ExcelFile();
        DataTable dtExport = new DataTable();
        BusinessLogicLayer.Reports reports = new BusinessLogicLayer.Reports();

        string data;
        int pageindex = 0;
        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //'http://localhost:2957/InfluxCRM/management/testProjectMainPage_employee.aspx'
                string sPath = HttpContext.Current.Request.Url.ToString();
                oDBEngine.Call_CheckPageaccessebility(sPath);
            }
        }
        public int CurrentPage
        {

            get
            {
                if (this.ViewState["CurrentPage"] == null)
                    return 0;
                else
                    return Convert.ToInt16(this.ViewState["CurrentPage"].ToString());
            }

            set
            {
                this.ViewState["CurrentPage"] = value;
            }

        }

        public int LastPage
        {
            get
            {
                if (this.ViewState["LastPage"] == null)
                    return 0;
                else
                    return Convert.ToInt16(this.ViewState["LastPage"].ToString());
            }
            set
            {
                this.ViewState["LastPage"] = value;
            }

        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (HttpContext.Current.Session["userid"] == null)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
            //_____For performing operation without refreshing page___//
            String cbReference = Page.ClientScript.GetCallbackEventReference(this, "arg", "ReceiveServerData", "context");
            String callbackScript = "function CallServer(arg, context){ " + cbReference + ";}";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "CallServer", callbackScript, true);
            //___________-end here___//
            if (!IsPostBack)
            {

                Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script language='javascript'>Page_Load();</script>");

                Date();
            }
        }
        void Date()
        {
            DtFor.EditFormatString = oconverter.GetDateFormat("Date");
            DtFrom.EditFormatString = oconverter.GetDateFormat("Date");
            DtTo.EditFormatString = oconverter.GetDateFormat("Date");
            DataTable dtexpiryeffectuntil = new DataTable();
            dtexpiryeffectuntil = oDBEngine.GetDataTable("master_equity", " DISTINCT top 2 equity_effectuntil", "  month(equity_effectuntil)<=month(getdate()) and year(equity_effectuntil)=year(getdate())", " equity_effectuntil desc");
            if (dtexpiryeffectuntil.Rows.Count == 2)
            {
                DtFrom.Value = Convert.ToDateTime(new DateTime(Convert.ToDateTime(dtexpiryeffectuntil.Rows[1][0].ToString()).Year, Convert.ToDateTime(dtexpiryeffectuntil.Rows[1][0].ToString()).Month, Convert.ToDateTime(dtexpiryeffectuntil.Rows[1][0].ToString()).Day).AddDays(1).ToString());
                DtTo.Value = Convert.ToDateTime(dtexpiryeffectuntil.Rows[0][0].ToString());
            }
            else
            {
                DtFrom.Value = Convert.ToDateTime(oDBEngine.GetDate().ToString());
                DtTo.Value = Convert.ToDateTime(oDBEngine.GetDate().ToString());
            }

            DtFor.Value = Convert.ToDateTime(oDBEngine.GetDate().ToShortDateString());

        }
        void ICallbackEventHandler.RaiseCallbackEvent(string eventArgument)
        {
            string id = eventArgument.ToString();
            string[] idlist = id.Split('~');
            string[] cl = idlist[1].Split(',');
            string str = "";
            string str1 = "";
            for (int i = 0; i < cl.Length; i++)
            {
                if (idlist[0] != "Clients" && idlist[0] != "Expiry")
                {
                    string[] val = cl[i].Split(';');
                    if (str == "")
                    {
                        str = val[0];
                        str1 = val[0] + ";" + val[1];
                    }
                    else
                    {
                        str += "," + val[0];
                        str1 += "," + val[0] + ";" + val[1];
                    }
                }
                //else
                //{

                //    string[] val = cl[i].Split(';');
                //    string[] AcVal = val[0].Split('-');
                //    if (str == "")
                //    {

                //        str = "'" + AcVal[0] + "'";
                //        str1 = "'" + AcVal[0] + "'" + ";" + val[1];
                //    }
                //    else
                //    {

                //        str += ",'" + AcVal[0] + "'";
                //        str1 += "," + "'" + AcVal[0] + "'" + ";" + val[1];
                //    }
                //}
                else
                {
                    string[] val = cl[i].Split(';');
                    string[] AcVal = val[0].Split('-');
                    if (str == "")
                    {
                        if (idlist[0] == "MAILEMPLOYEE")
                        {
                            str = AcVal[0];

                        }
                        else
                        {
                            str = "'" + AcVal[0] + "'";
                            str1 = "'" + AcVal[0] + "'" + ";" + val[1];
                        }
                    }
                    else
                    {
                        if (idlist[0] == "MAILEMPLOYEE")
                        {
                            str += "," + AcVal[0];
                        }
                        else
                        {
                            str += ",'" + AcVal[0] + "'";
                            str1 += "," + "'" + AcVal[0] + "'" + ";" + val[1];
                        }
                    }
                }

            }

            if (idlist[0] == "Clients")
            {
                data = "Clients~" + str;
            }
            else if (idlist[0] == "Expiry")
            {
                data = "Expiry~" + str;
            }
            else if (idlist[0] == "Product")
            {
                data = "Product~" + str;
            }
            else if (idlist[0] == "Group")
            {
                data = "Group~" + str;
            }
            else if (idlist[0] == "Branch")
            {
                data = "Branch~" + str;
            }
            else if (idlist[0] == "MAILEMPLOYEE")
            {
                data = "MAILEMPLOYEE~" + str;
            }

        }
        string ICallbackEventHandler.GetCallbackResult()
        {
            return data;
        }
        public void BindGroup()
        {
            ddlgrouptype.Items.Clear();
            DataTable DtGroup = oDBEngine.GetDataTable("tbl_master_groupmaster", "distinct gpm_Type", null);
            if (DtGroup.Rows.Count > 0)
            {
                ddlgrouptype.DataSource = DtGroup;
                ddlgrouptype.DataTextField = "gpm_Type";
                ddlgrouptype.DataValueField = "gpm_Type";
                ddlgrouptype.DataBind();
                ddlgrouptype.Items.Insert(0, new ListItem("Select GroupType", "0"));
                DtGroup.Dispose();

            }
            else
            {
                ddlgrouptype.Items.Insert(0, new ListItem("Select GroupType", "0"));
            }

        }
        protected void btnhide_Click(object sender, EventArgs e)
        {

            if (ddlGroup.SelectedItem.Value.ToString().Trim() == "1")
            {
                if (HiddenField_Group.Value.ToString().Trim() == "")
                {
                    BindGroup();
                }
            }
        }
        protected void btnScreen_Click(object sender, EventArgs e)
        {
            Procedure();
        }
        protected void btnExcel_Click(object sender, EventArgs e)
        {
            Procedure();
        }
        void FnSegment()
        {

            DataTable DtSeg = new DataTable();
            if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() != "2")
            {
                DtSeg = oDBEngine.GetDataTable("tbl_master_companyexchange", "EXCH_INTERNALID", "exch_segmentid='CM'", null);
            }
            else
            {
                DtSeg = oDBEngine.GetDataTable("tbl_master_companyexchange", "EXCH_INTERNALID", "exch_segmentid in ('CM','FO')", null);
            }
            if (DtSeg.Rows.Count > 0)
            {
                for (int i = 0; i < DtSeg.Rows.Count; i++)
                {
                    HiddenField_Segment.Value += "," + DtSeg.Rows[i][0].ToString();
                }

            }
        }
        void Procedure()
        {
            string[] InputName = new string[28];
            string[] InputType = new string[28];
            string[] InputValue = new string[28];

            InputName[0] = "Companyid";
            InputType[0] = "V";
            InputName[1] = "Segment";
            InputType[1] = "V";
            InputName[2] = "DateType";
            InputType[2] = "V";
            InputName[3] = "Fromdate";
            InputType[3] = "V";
            InputName[4] = "Todate";
            InputType[4] = "V";
            InputName[5] = "IgnoreBfPosition";
            InputType[5] = "V";
            InputName[6] = "ValueBfPositionAtPrevClose";
            InputType[6] = "V";
            InputName[7] = "RptView";
            InputType[7] = "V";
            InputName[8] = "RptType";
            InputType[8] = "V";
            InputName[9] = "GrpType";
            InputType[9] = "V";
            InputName[10] = "GrpId";
            InputType[10] = "V";
            InputName[11] = "BranchHierchy";
            InputType[11] = "V";
            InputName[12] = "ClientId";
            InputType[12] = "V";
            InputName[13] = "TerminalId";
            InputType[13] = "V";
            InputName[14] = "Assetid";
            InputType[14] = "V";
            InputName[15] = "Expiry";
            InputType[15] = "V";
            InputName[16] = "InstrumentType";
            InputType[16] = "V";
            InputName[17] = "ClubSpot";
            InputType[17] = "V";
            InputName[18] = "IncludeCharges";
            InputType[18] = "V";
            InputName[19] = "IncludeInterest";
            InputType[19] = "V";
            InputName[20] = "MtmCalBasis";
            InputType[20] = "V";
            InputName[21] = "ShowClient";
            InputType[21] = "V";
            InputName[22] = "ShowOnlyOpenPosition";
            InputType[22] = "V";
            InputName[23] = "ClosingPrice";
            InputType[23] = "V";
            InputName[24] = "PositionSign";
            InputType[24] = "V";
            InputName[25] = "NetPrm";
            InputType[25] = "V";
            InputName[26] = "TradeValCal";
            InputType[26] = "V";
            InputName[27] = "email";
            InputType[27] = "V";

            ////////////////Fetch Instrument
            string chkcalbasis = null;
            HiddenField_Segment.Value = Session["usersegid"].ToString().Trim();
            foreach (ListItem listitem in chkinstrutype.Items)
            {
                if (listitem.Selected)
                {
                    if (listitem.Value == "FUT")
                    {
                        if (chkcalbasis == null)
                            chkcalbasis = "(EQUITY_FOIDENTIFIER LIKE " + " '" + listitem.Value + "%" + "'";
                        else
                            chkcalbasis += " or " + "EQUITY_FOIDENTIFIER LIKE " + "'" + listitem.Value + "%" + "'";


                    }
                    else if (listitem.Value == "C" || listitem.Value == "P")
                    {
                        if (chkcalbasis == null)
                            chkcalbasis = "((EQUITY_FOIDENTIFIER LIKE 'OPT%' AND EQUITY_SERIES LIKE " + " '" + listitem.Value + "%" + "')";
                        else
                            chkcalbasis += " or " + "(EQUITY_FOIDENTIFIER LIKE 'OPT%' AND EQUITY_SERIES LIKE " + " '" + listitem.Value + "%" + "')";


                    }
                    else
                    {
                        if (chkcalbasis == null)
                            chkcalbasis = "(EQUITY_FOIDENTIFIER is null";
                        else
                            chkcalbasis += " or " + "EQUITY_FOIDENTIFIER is null ";

                        FnSegment();
                    }
                }
            }
            if (chkcalbasis != null)
            {
                chkcalbasis += " )";
                string strSpName = "[Report_PerformanceReportFO]";
                InputValue[0] = Session["LastCompany"].ToString();
                InputValue[1] = HiddenField_Segment.Value;
                InputValue[2] = ddldate.SelectedItem.Value.ToString().Trim();
                if (ddldate.SelectedItem.Value.ToString().Trim() == "0")
                {
                    InputValue[3] = "1900-01-01";
                    InputValue[4] = DtFor.Value.ToString();
                }
                else
                {
                    InputValue[3] = DtFrom.Value.ToString();
                    InputValue[4] = DtTo.Value.ToString();
                }
                InputValue[5] = ChkBfPosition.Checked.ToString().Trim();
                InputValue[6] = ChkBfPositionValue.Checked.ToString().Trim();
                InputValue[7] = DdlRptView.SelectedItem.Value.ToString().Trim();
                InputValue[8] = DdlRptType.SelectedItem.Value.ToString().Trim();
                if (ddlGroup.SelectedItem.Value.ToString() == "0")
                {
                    InputValue[9] = "BRANCH";
                    if (rdbranchAll.Checked)
                    {
                        InputValue[10] = "ALL";
                    }
                    else
                    {
                        InputValue[10] = HiddenField_Branch.Value;
                    }
                }
                else if (ddlGroup.SelectedItem.Value.ToString() == "2")
                {
                    InputValue[9] = "BRANCHGROUP";
                    if (rdbranchAll.Checked)
                    {
                        InputValue[10] = "ALL";
                    }
                    else
                    {
                        InputValue[10] = HiddenField_BranchGroup.Value;
                    }
                }
                else
                {
                    InputValue[9] = ddlgrouptype.SelectedItem.Text.ToString().Trim();
                    if (rdddlgrouptypeAll.Checked)
                    {
                        InputValue[10] = "ALL";
                    }
                    else
                    {
                        InputValue[10] = HiddenField_Group.Value;
                    }
                }
                InputValue[11] = Session["userbranchHierarchy"].ToString();
                if (rdbClientALL.Checked)
                {
                    InputValue[12] = "ALL";
                }
                else
                {
                    InputValue[12] = HiddenField_Client.Value;
                }
                if (rdbTerminalAll.Checked)
                {
                    InputValue[13] = "ALL";
                }
                else
                {
                    InputValue[13] = txtTerminal_hidden.Text.ToString().Trim();
                }
                if (rdbunderlyingall.Checked)
                {
                    InputValue[14] = "ALL";
                }
                else
                {
                    InputValue[14] = HiddenField_Product.Value;
                }
                if (rdbExpiryAll.Checked)
                {
                    InputValue[15] = "ALL";
                }
                else
                {
                    InputValue[15] = HiddenField_Expiry.Value;
                }
                InputValue[16] = chkcalbasis.ToString().Trim();
                InputValue[17] = ChkConsolidatedSpot.Checked.ToString().Trim();
                InputValue[18] = ChkIncludeCharges.Checked.ToString().Trim();
                InputValue[19] = ChkIncludeInterest.Checked.ToString().Trim();
                InputValue[20] = DdlMtmCalBasis.SelectedItem.Value.ToString().Trim();
                if (rdbnetclientboth.Checked)
                {
                    InputValue[21] = "Both";
                }
                if (rdbnetclientrecivabel.Checked)
                {
                    InputValue[21] = "Receivable";
                }
                if (rdbnetclientpayabel.Checked)
                {
                    InputValue[21] = "Payable";
                }
                InputValue[22] = chkopen.Checked.ToString().Trim();
                InputValue[23] = chkclosepricezero.Checked.ToString().Trim();
                InputValue[24] = chkopenbfpositive.Checked.ToString().Trim();
                InputValue[25] = chknetpremium.Checked.ToString().Trim();
                InputValue[26] = DdlCalculateOn.SelectedItem.Value.ToString().Trim();
                //InputValue[27] = ddloptionformail.SelectedItem.Value.ToString().Trim();
                if (ddlGeneration.SelectedItem.Value == "3")
                {
                    InputValue[27] = "Yes";
                }
                else
                {
                    InputValue[27] = "no";
                }

                ds.Reset();
                //This is For Debugging Purpose On Server
                //DebugHelper oDebugHelper = new DebugHelper();
                //oDebugHelper.GetPrepareSpToRun(strSpName, InputName, InputType, InputValue);
                //End
                ds = SQLProcedures.SelectProcedureArrDS(strSpName, InputName, InputType, InputValue);
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        ViewState["dataset"] = ds;
                        FnGeneRationCall(ds, "Sp");
                    }
                }
            }
            else
            {
                FnGeneRationCall(ds, "NoSp");
            }

        }
        //void ExportToExcel_Generic(System.Data.DataTable Dt)
        //{
        //    GenericExcelExport oGenericExcelExport = new GenericExcelExport();
        //    string strDownloadFileName = "";
        //    string[] ColumnName = { "AccountsLedger_AmountDr", "AccountsLedger_AmountCr" };
        //    string[] ColumnType = { "V", "V" };
        //    string[] ColumnSize = { "50", "50" }; 
        //    strDownloadFileName = "~/Documents/" + oDBEngine.GetDate().ToString("yyyyMMddHHmmss") + ".xlsx";
        //    oGenericExcelExport.ExportToExcel(ColumnName, ColumnType, ColumnSize, Dt, Server.MapPath(strDownloadFileName), "2007");
        //}
        void FnGeneRationCall(DataSet ds, string Status)
        {
            if (Status.ToString().Trim() == "Sp")
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (ddlGeneration.SelectedItem.Value.ToString() == "1")///Screen
                    {

                        BindOther(ds);
                    }
                    if (ddlGeneration.SelectedItem.Value.ToString() == "2")///Export
                    {
                        Export(ds);

                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "fnRecord", "fnRecord('1');", true);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "fnRecord", "fnRecord('4');", true);
            }
        }
        protected string GetRowColor(int i)
        {
            if (i++ % 2 == 0)
                return "#fff0f5";
            else
                return "White";
        }
        public static bool IsNumeric(object value)
        {
            double dbl;
            return double.TryParse(value.ToString(), System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.InvariantInfo, out dbl);
        }
        void BindOther(DataSet ds)
        {
            if (DdlRptType.SelectedItem.Value.ToString() == "1")///Report Tpe:Detail 
            {
                DataView viewData = new DataView();
                viewData = ds.Tables[0].DefaultView;
                viewData.RowFilter = " Tabid2<>'ZZZZZZZZZZZ'";
                DataTable dt = new DataTable();
                dt = viewData.ToTable();

                DataTable DistinctRecord = new DataTable();
                DataView viewRecord = new DataView(dt);
                DistinctRecord = viewRecord.ToTable(true, new string[] { "Tabid2" });

                if (DistinctRecord.Rows.Count > 0)
                {
                    cmbgroup.DataSource = DistinctRecord;
                    cmbgroup.DataValueField = "Tabid2";
                    cmbgroup.DataTextField = "Tabid2";
                    cmbgroup.DataBind();

                }
                Tabid1Bind(ds, cmbgroup.SelectedItem.Value.ToString().Trim());
            }
            if (DdlRptType.SelectedItem.Value.ToString() == "2")///Report Tpe:Summary 
            {
                DataView viewData1 = new DataView();
                viewData1 = ds.Tables[0].DefaultView;
                viewData1.RowFilter = " Tabid3<>'ZZZZZZZZZZZ'";
                DataTable dt1 = new DataTable();
                dt1 = viewData1.ToTable();

                DataTable DistinctRecord1 = new DataTable();
                DataView viewRecord1 = new DataView(dt1);
                DistinctRecord1 = viewRecord1.ToTable(true, new string[] { "Tabid3" });

                if (DistinctRecord1.Rows.Count > 0)
                {
                    cmbgroup.DataSource = DistinctRecord1;
                    cmbgroup.DataValueField = "Tabid3";
                    cmbgroup.DataTextField = "Tabid3";
                    cmbgroup.DataBind();

                }
                FnHtml(ds, cmbgroup.SelectedItem.Value.ToString().Trim(), "Summary");
            }
        }
        void Tabid1Bind(DataSet ds, string param)
        {
            DataView viewTab1 = new DataView();
            viewTab1 = ds.Tables[0].DefaultView;
            viewTab1.RowFilter = "Tabid2='" + cmbgroup.SelectedItem.Value.ToString().Trim() + "' and Tabid1 is not Null and Tabid1<>'ZZZZZZZZZZZ'";
            DataTable dtTab1 = new DataTable();
            dtTab1 = viewTab1.ToTable();

            DataView viewTab2 = new DataView(dtTab1);
            DataTable DtDistinctTab1 = viewTab2.ToTable(true, new string[] { "Tabid1" });

            if (DtDistinctTab1.Rows.Count > 0)
            {
                cmbclient.DataSource = DtDistinctTab1;
                cmbclient.DataValueField = "Tabid1";
                cmbclient.DataTextField = "Tabid1";
                cmbclient.DataBind();

            }

            LastPage = DtDistinctTab1.Rows.Count - 1;
            CurrentPage = 0;
            bind_Details();
        }
        void bind_Details()
        {
            cmbclient.SelectedIndex = CurrentPage;
            ds = (DataSet)ViewState["dataset"];
            FnHtml(ds, cmbclient.SelectedItem.Value.ToString(), "Detail");
            ShowHidePreviousNext_of_Clients();
        }
        void ShowHidePreviousNext_of_Clients()
        {
            if (LastPage == 0 || LastPage == -1)
            {
                ASPxFirst.Style["Display"] = "none";
                ASPxPrevious.Style["Display"] = "none";
                ASPxNext.Style["Display"] = "none";
                ASPxLast.Style["Display"] = "none";

            }
            else
            {
                ASPxFirst.Style["Display"] = "Display";
                ASPxPrevious.Style["Display"] = "Display";
                ASPxNext.Style["Display"] = "Display";
                ASPxLast.Style["Display"] = "Display";

            }

            if (CurrentPage == LastPage && LastPage != 0)
            {

                ASPxFirst.Enabled = true;
                ASPxPrevious.Enabled = true;

                ASPxNext.Enabled = false;
                ASPxLast.Enabled = false;

            }
            else
                if (CurrentPage == 0 && LastPage != 0)
                {
                    ASPxFirst.Enabled = false;
                    ASPxPrevious.Enabled = false;

                    ASPxNext.Enabled = true;
                    ASPxLast.Enabled = true;


                }
                else
                {
                    ASPxFirst.Enabled = true;
                    ASPxPrevious.Enabled = true;
                    ASPxNext.Enabled = true;
                    ASPxLast.Enabled = true;
                }
        }
        protected void ASPxFirst_Click(object sender, EventArgs e)
        {
            hiddencount.Value = "0";
            CurrentPage = 0;
            bind_Details();
        }
        protected void ASPxPrevious_Click(object sender, EventArgs e)
        {
            if (CurrentPage > 0)
            {
                hiddencount.Value = "0";
                CurrentPage = CurrentPage - 1;
                bind_Details();
            }
        }
        protected void ASPxNext_Click(object sender, EventArgs e)
        {
            if (CurrentPage < LastPage)
            {
                hiddencount.Value = "0";
                CurrentPage = CurrentPage + 1;
                bind_Details();
            }
        }
        protected void ASPxLast_Click(object sender, EventArgs e)
        {
            hiddencount.Value = "0";
            CurrentPage = LastPage;
            bind_Details();
        }
        protected void NavigationLinkC_Click(Object sender, CommandEventArgs e)
        {
            hiddencount.Value = "0";
            int curentIndex = cmbgroup.SelectedIndex;
            int totalNo = cmbgroup.Items.Count;
            switch (e.CommandName)
            {
                case "First":
                    pageindex = 0;
                    break;
                case "Next":
                    curentIndex = curentIndex + 1;
                    break;
                case "Prev":
                    curentIndex = curentIndex - 1;
                    break;
                case "Last":
                    pageindex = int.Parse(TotalRecord.Value);
                    break;
                default:
                    pageindex = int.Parse(e.CommandName.ToString());
                    break;
            }
            if (curentIndex >= totalNo)
            {
                curentIndex = totalNo - 1;
                ScriptManager.RegisterStartupScript(this, GetType(), "hide", "DisableC('N');", true);
            }
            else if (curentIndex <= 0)
            {
                curentIndex = 0;
                ScriptManager.RegisterStartupScript(this, GetType(), "hide", "DisableC('P');", true);
            }

            cmbgroup.SelectedIndex = curentIndex;

            ds = (DataSet)ViewState["dataset"];

            if (DdlRptType.SelectedItem.Value.ToString() == "1")///Report Tpe:Detail 
            {
                Tabid1Bind(ds, cmbgroup.SelectedItem.Value.ToString().Trim());
            }
            else
            {
                FnHtml(ds, cmbgroup.SelectedItem.Value.ToString().Trim(), "Summary");
            }
        }
        void FnHtml(DataSet ds, string Param, string RptType)
        {
            //////////For header
            String strHtmlheader = String.Empty;
            string str = null;
            if (ddlGroup.SelectedItem.Value.ToString() == "1")
            {
                str = "[" + ddlgrouptype.SelectedItem.Text.ToString().Trim() + "]";
            }

            if (ddldate.SelectedItem.Value.ToString() == "0")
            {
                str = ddlGroup.SelectedItem.Text.ToString().Trim() + " Wise " + str + "Report As On Date : " + oconverter.ArrangeDate2(DtFor.Value.ToString());
            }
            else
            {
                str = ddlGroup.SelectedItem.Text.ToString().Trim() + " Wise " + str + "Report Period :" + oconverter.ArrangeDate2(DtFrom.Value.ToString()) + " - " + oconverter.ArrangeDate2(DtTo.Value.ToString());
            }

            strHtmlheader = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";
            strHtmlheader += "<tr><td align=\"left\" colspan=" + ds.Tables[0].Columns.Count + " style=\"color:Blue;\">" + str + "</td></tr></table>";


            ////////Detail Display
            String strHtml = String.Empty;
            strHtml = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";

            DataTable dt = new DataTable();
            if (RptType.ToString().Trim() != "Summary")
            {
                DataView viewTabid1 = new DataView();
                viewTabid1 = ds.Tables[0].DefaultView;
                viewTabid1.RowFilter = " Tabid1='" + Param.ToString().Trim() + "'";
                dt = viewTabid1.ToTable();

                strHtml += "<tr style=\"background-color:lavender ;text-align:left\">";
                strHtml += "<td align=\"left\" style=\"font-size:smaller;\"  nowrap=nowrap; ><b>" + dt.Rows[0][2].ToString().Trim() + "</b></td>";
                strHtml += "</tr>";

                dt.Rows[0].Delete();
                dt.Columns.Remove("Tabid1");
                dt.Columns.Remove("Tabid2");
            }
            else
            {
                DataView viewTabid3 = new DataView();
                viewTabid3 = ds.Tables[0].DefaultView;
                viewTabid3.RowFilter = " Tabid3='" + Param.ToString().Trim() + "'";
                dt = viewTabid3.ToTable();


                dt.Columns.Remove("Tabid3");

            }


            //////////////TABLE HEADER BIND
            strHtml += "<tr style=\"background-color: #DBEEF3;\">";
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                strHtml += "<td align=\"center\" style=\"font-size:smaller;\" nowrap=nowrap;><b>" + dt.Columns[i].ColumnName + "</b></td>";

            }
            strHtml += "</tr>";

            int flag = 0;
            foreach (DataRow dr1 in dt.Rows)
            {
                flag = flag + 1;
                strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    if (dr1[j] != DBNull.Value)
                    {
                        if (dr1[j].ToString().Trim().StartsWith("Client Total :") || dr1[j].ToString().Trim().StartsWith("Total") || dr1[j].ToString().Trim().StartsWith("Group") || dr1[j].ToString().Trim().StartsWith("Scrip") || dr1[j].ToString().Trim().StartsWith("Asset"))
                        {
                            strHtml += "<td align=\"left\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + dt.Columns[j].ColumnName + "\"><b>" + dr1[j] + "</b></td>";
                        }
                        else if (dr1[j].ToString().Trim().StartsWith("Test"))
                        {
                            strHtml += "<td>&nbsp;</td>";
                        }
                        else
                        {
                            if (IsNumeric(dr1[j].ToString()) == true)
                            {
                                strHtml += "<td align=\"right\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + dt.Columns[j].ColumnName + "\">" + dr1[j] + "</td>";

                            }
                            else
                            {
                                strHtml += "<td align=\"left\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + dt.Columns[j].ColumnName + "\">" + dr1[j] + "</td>";

                            }
                        }
                    }
                    else
                    {
                        strHtml += "<td>&nbsp;</td>";
                    }
                }

                strHtml += "</tr>";
            }
            strHtml += "</table>";
            DivHeader.InnerHtml = strHtmlheader;
            Divdisplay.InnerHtml = strHtml;

            if (RptType.ToString().Trim() != "Summary")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "fnRecord", "fnRecord('5');", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "fnRecord", "fnRecord('6');", true);
            }

        }
        protected void cmbgroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            ds = (DataSet)ViewState["dataset"];

            if (DdlRptType.SelectedItem.Value.ToString() == "1")///Report Tpe:Detail 
            {
                Tabid1Bind(ds, cmbgroup.SelectedItem.Value.ToString().Trim());
            }
            else
            {
                FnHtml(ds, cmbgroup.SelectedItem.Value.ToString().Trim(), "Summary");
            }

        }
        void Export(DataSet ds)
        {
            DataTable dtExport = ds.Tables[0].Copy();
            if (DdlRptType.SelectedItem.Value.ToString() == "1")///Report Tpe:Detail 
            {
                dtExport.Columns.Remove("Tabid1");
                dtExport.Columns.Remove("Tabid2");

            }
            else
            {
                dtExport.Columns.Remove("Tabid3");
            }




            string str = null;
            if (ddlGroup.SelectedItem.Value.ToString() == "1")
            {
                str = "[" + ddlgrouptype.SelectedItem.Text.ToString().Trim() + "]";
            }

            if (ddldate.SelectedItem.Value.ToString() == "0")
            {
                str = ddlGroup.SelectedItem.Text.ToString().Trim() + " Wise " + str + "Report As On Date : " + oconverter.ArrangeDate2(DtFor.Value.ToString());
            }
            else
            {
                str = ddlGroup.SelectedItem.Text.ToString().Trim() + " Wise " + str + "Report Period :" + oconverter.ArrangeDate2(DtFrom.Value.ToString()) + " - " + oconverter.ArrangeDate2(DtTo.Value.ToString());
            }



            DataTable dtReportHeader = new DataTable();
            DataTable dtReportFooter = new DataTable();

            DataTable CompanyName = oDBEngine.GetDataTable("tbl_master_company", "cmp_Name", " cmp_internalId='" + Session["LastCompany"].ToString() + "'");
            dtReportHeader.Columns.Add(new DataColumn("Header", typeof(String))); //0
            DataRow HeaderRow = dtReportHeader.NewRow();
            HeaderRow[0] = CompanyName.Rows[0][0].ToString();
            dtReportHeader.Rows.Add(HeaderRow);
            DataRow DrRowR1 = dtReportHeader.NewRow();
            DrRowR1[0] = str.ToString().Trim();
            dtReportHeader.Rows.Add(DrRowR1);


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

            objExcel.ExportToExcelforExcel(dtExport, "Portfolio Performance Report", "Total :", dtReportHeader, dtReportFooter);

        }
        protected void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            ds = (DataSet)ViewState["dataset"];
            Export(ds);
        }
        protected void btnmail_Click(object sender, EventArgs e)
        {
            Procedure();
            ds = (DataSet)ViewState["dataset"];
            if (ds.Tables[0].Rows.Count > 0)
            {

                mail();
            }
        }
        void mail()
        {
            if (ddldate.SelectedItem.Value.ToString() == "0")
            {
                ViewState["billdate"] = oconverter.ArrangeDate2(DtFor.Value.ToString());
            }
            else
            {
                ViewState["billdate"] = oconverter.ArrangeDate2(DtFrom.Value.ToString()) + " - " + oconverter.ArrangeDate2(DtTo.Value.ToString());
            }
            if (ddloptionformail.SelectedItem.Value.ToString().Trim() == "0")
            {
                clientwisemail();
            }
            if (ddloptionformail.SelectedItem.Value.ToString().Trim() == "1")
            {
                branhgroupemail();
            }
            if (ddloptionformail.SelectedItem.Value.ToString().Trim() == "2")
            {
                optionforemailclient();
            }
        }
        void clientwisemail()
        {
            DataView viewData = new DataView();
            viewData = ds.Tables[0].DefaultView;
            viewData.RowFilter = " Tabid1<>'ZZZZZZZZZZZ' and Tabid1 is not null";
            DataTable dt = new DataTable();
            dt = viewData.ToTable();

            DataTable Distinctclient = new DataTable();
            DataView viewClient = new DataView(dt);
            Distinctclient = viewClient.ToTable(true, new string[] { "Tabid1", "Tabid2" });

            if (Distinctclient.Rows.Count > 0)
            {
                cmbclient.DataSource = Distinctclient;
                cmbclient.DataValueField = "Tabid1";
                cmbclient.DataTextField = "Tabid2";
                cmbclient.DataBind();

            }
            ViewState["mailsendresult"] = "mail";
            /////////For Client Email
            for (int k = 0; k < cmbclient.Items.Count; k++)
            {
                // FnHtml(ds, cmbclient.Items[k].Value.ToString().Trim());
                FnHtml1(cmbclient.Items[k].Value, DdlRptType.SelectedItem.Text.ToString().Trim());

                if (oDBEngine.SendReport(ViewState["mail"].ToString().Trim(), cmbclient.Items[k].Value, ViewState["billdate"].ToString().Trim(), "Portfolio Performance [" + ViewState["billdate"].ToString().Trim() + "]") == true)
                {
                    if (ViewState["mailsendresult"].ToString().Trim() == "Error")
                    {
                        ViewState["mailsendresult"] = "SomeClientError";
                    }
                    else
                    {
                        ViewState["mailsendresult"] = "Success";
                    }
                }
                else
                {

                    ViewState["mailsendresult"] = "Error";
                }
            }

            if (ViewState["mailsendresult"].ToString().Trim() == "SomeClientError")
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "JScript170", "<script language='javascript'>alert('Mail Sent Successfully !!'+'\n'+'Emails not Sent For Clients Without Email-Id ...');</script>");
            }
            if (ViewState["mailsendresult"].ToString().Trim() == "Success")
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "JScript810", "<script language='javascript'>alert('Mail Sent Successfully !!');</script>");
            }
            if (ViewState["mailsendresult"].ToString().Trim() == "Error")
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "JScript190", "<script language='javascript'>alert('Error on sending!Try again..');</script>");
            }
            Page.ClientScript.RegisterStartupScript(GetType(), "JScript32", "<script language='javascript'>Page_Load();</script>");
        }
        void branhgroupemail()
        {
            ViewState["mailsendresult"] = "mail";
            ViewState["GrpEmail"] = "mail";

            DataView viewData = new DataView();
            viewData = ds.Tables[0].DefaultView;
            viewData.RowFilter = " Tabid1<>'ZZZZZZZZZZZ' and Tabid1 is not null and Gidmain<>'123'";
            DataTable dt = new DataTable();
            dt = viewData.ToTable();

            DataTable DistinctGrp = new DataTable();
            DataView viewGrp = new DataView(dt);
            DistinctGrp = viewGrp.ToTable(true, new string[] { "Gidmain", "Tabid2" });


            if (DistinctGrp.Rows.Count > 0)
            {
                for (int j = 0; j < DistinctGrp.Rows.Count; j++)
                {
                    DataView viewgrp = new DataView();
                    viewgrp = ds.Tables[0].DefaultView;
                    viewgrp.RowFilter = "Gidmain='" + DistinctGrp.Rows[j][0].ToString().Trim() + "'";
                    DataTable dt1 = new DataTable();
                    dt1 = viewgrp.ToTable();

                    DataView viewData1 = new DataView();
                    viewData1 = dt1.DefaultView;
                    viewData1.RowFilter = " Tabid1<>'ZZZZZZZZZZZ' and Tabid1 is not null";
                    DataTable dt2 = new DataTable();
                    dt2 = viewData1.ToTable();

                    DataTable Distinctclient = new DataTable();
                    DataView viewClient = new DataView(dt2);
                    Distinctclient = viewClient.ToTable(true, new string[] { "Tabid1", "Tabid2" });

                    if (Distinctclient.Rows.Count > 0)
                    {
                        cmbclient.DataSource = Distinctclient;
                        cmbclient.DataValueField = "Tabid1";
                        cmbclient.DataTextField = "Tabid2";
                        cmbclient.DataBind();

                    }
                    /////////For Client Email Begin
                    for (int k = 0; k < cmbclient.Items.Count; k++)
                    {
                        ////FnHtml(ds, cmbclient.Items[k].Value.ToString().Trim());
                        FnHtml1(cmbclient.Items[k].Value, DdlRptType.SelectedItem.Text.ToString().Trim());

                        if (ViewState["GrpEmail"].ToString().Trim() == "mail")
                        {
                            ViewState["GrpEmail"] = ViewState["mail"].ToString().Trim();
                        }
                        else
                        {
                            ViewState["GrpEmail"] = ViewState["GrpEmail"].ToString().Trim() + ViewState["mail"].ToString().Trim();
                        }
                    }
                    /////////For Client Email End
                    /////////Group/Branch Email Send Begin
                    ////////////if (oDBEngine.SendReportBr(ViewState["GrpEmail"].ToString().Trim(), DistinctGrp.Rows[j]["GrpEmail"].ToString().Trim(), Date.ToString().Trim(), "Trade Register [" + Date.ToString().Trim() + "]", DistinctGrp.Rows[j]["Grpid"].ToString().Trim()) == true)
                    ////////////{
                    ////////////    if (ViewState["mailsendresult"].ToString().Trim() == "Error")
                    ////////////    {
                    ////////////        ViewState["mailsendresult"] = "SomeClientError";
                    ////////////    }
                    ////////////    else
                    ////////////    {
                    ////////////        ViewState["mailsendresult"] = "Success";
                    ////////////    }
                    ////////////}
                    if (ddlGroup.SelectedItem.Value == "1")
                    {
                        if (oDBEngine.SendReportBrportfoliogroup(ViewState["GrpEmail"].ToString().Trim(), DistinctGrp.Rows[j][1].ToString().Trim(), ViewState["billdate"].ToString().Trim(), "Portfolio Performance [" + ViewState["billdate"].ToString().Trim() + "]", DistinctGrp.Rows[j][0].ToString().Trim()) == true)
                        {
                            if (ViewState["mailsendresult"].ToString().Trim() == "errorsuccess")
                            {
                                ViewState["mailsendresult"] = "someclienterror";
                            }
                            else
                            {
                                ViewState["mailsendresult"] = "success";
                            }
                        }
                        else
                        {

                            ViewState["mailsendresult"] = "errorsuccess";
                        }
                        ViewState["GrpEmail"] = "mail";
                    }
                    if (ddlGroup.SelectedItem.Value == "0")
                    {
                        if (oDBEngine.SendReportBrportfolio(ViewState["GrpEmail"].ToString().Trim(), DistinctGrp.Rows[j][1].ToString().Trim(), ViewState["billdate"].ToString().Trim(), "Portfolio Performance [" + ViewState["billdate"].ToString().Trim() + "]", DistinctGrp.Rows[j][0].ToString().Trim()) == true)
                        {
                            if (ViewState["mailsendresult"].ToString().Trim() == "errorsuccess")
                            {
                                ViewState["mailsendresult"] = "someclienterror";
                            }
                            else
                            {
                                ViewState["mailsendresult"] = "success";
                            }
                        }
                        else
                        {

                            ViewState["mailsendresult"] = "errorsuccess";
                        }
                        ViewState["GrpEmail"] = "mail";
                    }
                }

                if (ViewState["mailsendresult"].ToString().Trim() == "someclienterror")
                {
                    Page.ClientScript.RegisterStartupScript(GetType(), "JScript40", "<script language='javascript'>alert('Mail Sent Successfully !!'+'\n'+'Emails not Sent For Clients Without Email-Id ...');</script>");
                }
                if (ViewState["mailsendresult"].ToString().Trim() == "success")
                {
                    Page.ClientScript.RegisterStartupScript(GetType(), "JScript50", "<script language='javascript'>alert('Mail Sent Successfully !!');</script>");
                }
                if (ViewState["mailsendresult"].ToString().Trim() == "errorsuccess")
                {
                    Page.ClientScript.RegisterStartupScript(GetType(), "JScript60", "<script language='javascript'>alert('Error on sending!Try again..');</script>");
                }
                Page.ClientScript.RegisterStartupScript(GetType(), "JScript12", "<script language='javascript'>Page_Load();</script>");
            }
        }
        void optionforemailclient()
        {
            if (HiddenField_emmail.Value.ToString().Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "NORECORD", "NORECORD(5);", true);
            }
            else
            {
                DataView viewData = new DataView();
                viewData = ds.Tables[0].DefaultView;
                viewData.RowFilter = " Tabid1<>'ZZZZZZZZZZZ' and Tabid1 is not null";
                DataTable dt = new DataTable();
                dt = viewData.ToTable();

                DataTable Distinctclient = new DataTable();
                DataView viewClient = new DataView(dt);
                Distinctclient = viewClient.ToTable(true, new string[] { "Tabid1", "Tabid2" });

                if (Distinctclient.Rows.Count > 0)
                {
                    cmbclient.DataSource = Distinctclient;
                    cmbclient.DataValueField = "Tabid1";
                    cmbclient.DataTextField = "Tabid2";
                    cmbclient.DataBind();

                }
                ViewState["mailsendresult"] = "mail";
                ViewState["Usermail"] = "UserMail";

                /////////For Client Email Begin
                for (int k = 0; k < cmbclient.Items.Count; k++)
                {
                    //FnHtml(ds, cmbrecord.Items[k].Value.ToString().Trim());
                    //FnHtml1(cmbclient.Items[k].Value, cmbclient.Items[k].Text.ToString().Trim(), DdlRptType.SelectedItem.Text.ToString().Trim());
                    FnHtml1(cmbclient.Items[k].Value, DdlRptType.SelectedItem.Text.ToString().Trim());

                    if (ViewState["Usermail"].ToString().Trim() == "UserMail")
                    {
                        ViewState["Usermail"] = ViewState["mail"].ToString().Trim();
                    }
                    else
                    {
                        ViewState["Usermail"] = ViewState["Usermail"].ToString().Trim() + ViewState["mail"].ToString().Trim();
                    }
                }
                /////////For Client Email End

                /////////User Email Send
                string[] clnt = HiddenField_emmail.Value.ToString().Split(',');
                int kk = clnt.Length;
                for (int i = 0; i < clnt.Length; i++)
                {
                    if (oDBEngine.SendReportSt(ViewState["Usermail"].ToString().Trim(), clnt[i].ToString().Trim(), ViewState["billdate"].ToString().Trim(), "Portfolio Performance [" + ViewState["billdate"].ToString().Trim() + "]") == true)
                    {
                        if (ViewState["mailsendresult"].ToString().Trim() == "errorsuccess")
                        {
                            ViewState["mailsendresult"] = "someclienterror";
                        }
                        else
                        {
                            ViewState["mailsendresult"] = "success";
                        }
                    }
                    else
                    {

                        ViewState["mailsendresult"] = "errorsuccess";
                    }
                }

                if (ViewState["mailsendresult"].ToString().Trim() == "someclienterror")
                {
                    Page.ClientScript.RegisterStartupScript(GetType(), "JScript10", "<script language='javascript'>alert('Mail Sent Successfully !!'+'\n'+'Emails not Sent For Clients Without Email-Id ...');</script>");
                }
                if (ViewState["mailsendresult"].ToString().Trim() == "success")
                {
                    Page.ClientScript.RegisterStartupScript(GetType(), "JScript20", "<script language='javascript'>alert('Mail Sent Successfully !!');</script>");
                }
                if (ViewState["mailsendresult"].ToString().Trim() == "errorsuccess")
                {
                    Page.ClientScript.RegisterStartupScript(GetType(), "JScript30", "<script language='javascript'>alert('Error on sending!Try again..');</script>");
                }
            }
            Page.ClientScript.RegisterStartupScript(GetType(), "JScript22", "<script language='javascript'>Page_Load();</script>");
            //Page.ClientScript.RegisterStartupScript(GetType(), "JScript24", "<script language='javascript'>alert('Mail Sent Successfully');</script>");
        }
        void FnHtml1(string Param, string RptType)
        {
            //////////For header
            String strHtmlheader = String.Empty;
            string str = null;
            if (ddlGroup.SelectedItem.Value.ToString() == "1")
            {
                str = "[" + ddlgrouptype.SelectedItem.Text.ToString().Trim() + "]";
            }

            if (ddldate.SelectedItem.Value.ToString() == "0")
            {
                str = ddlGroup.SelectedItem.Text.ToString().Trim() + " Wise " + str + "Report As On Date : " + oconverter.ArrangeDate2(DtFor.Value.ToString());
            }
            else
            {
                str = ddlGroup.SelectedItem.Text.ToString().Trim() + " Wise " + str + "Report Period :" + oconverter.ArrangeDate2(DtFrom.Value.ToString()) + " - " + oconverter.ArrangeDate2(DtTo.Value.ToString());
            }

            strHtmlheader = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";
            strHtmlheader += "<tr><td align=\"left\" colspan=" + ds.Tables[0].Columns.Count + " style=\"color:Blue;\">" + str + "</td></tr></table>";


            ////////Detail Display
            String strHtml = String.Empty;
            strHtml = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";

            DataTable dt = new DataTable();
            if (RptType.ToString().Trim() != "Summary")
            {
                DataView viewTabid1 = new DataView();
                viewTabid1 = ds.Tables[0].DefaultView;
                viewTabid1.RowFilter = " Tabid1='" + Param.ToString().Trim() + "'";
                dt = viewTabid1.ToTable();

                strHtml += "<tr style=\"background-color:lavender ;text-align:left\">";
                strHtml += "<td align=\"left\" style=\"font-size:smaller;\"  nowrap=nowrap; ><b>" + dt.Rows[0][2].ToString().Trim() + "</b></td>";
                strHtml += "</tr>";

                dt.Rows[0].Delete();
                dt.Columns.Remove("Tabid1");
                dt.Columns.Remove("Tabid2");
                dt.Columns.Remove("Gidmain");
            }
            else
            {
                DataView viewTabid3 = new DataView();
                viewTabid3 = ds.Tables[0].DefaultView;
                viewTabid3.RowFilter = " Tabid3='" + Param.ToString().Trim() + "'";
                dt = viewTabid3.ToTable();


                dt.Columns.Remove("Tabid3");

            }


            //////////////TABLE HEADER BIND
            strHtml += "<tr style=\"background-color: #DBEEF3;\">";
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                strHtml += "<td align=\"center\" style=\"font-size:smaller;\" nowrap=nowrap;><b>" + dt.Columns[i].ColumnName + "</b></td>";

            }
            strHtml += "</tr>";

            int flag = 0;
            foreach (DataRow dr1 in dt.Rows)
            {
                flag = flag + 1;
                strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    if (dr1[j] != DBNull.Value)
                    {
                        if (dr1[j].ToString().Trim().StartsWith("Client Total :") || dr1[j].ToString().Trim().StartsWith("Total") || dr1[j].ToString().Trim().StartsWith("Group") || dr1[j].ToString().Trim().StartsWith("Scrip") || dr1[j].ToString().Trim().StartsWith("Asset"))
                        {
                            strHtml += "<td align=\"left\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + dt.Columns[j].ColumnName + "\"><b>" + dr1[j] + "</b></td>";
                        }
                        else if (dr1[j].ToString().Trim().StartsWith("Test"))
                        {
                            strHtml += "<td>&nbsp;</td>";
                        }
                        else
                        {
                            if (IsNumeric(dr1[j].ToString()) == true)
                            {
                                strHtml += "<td align=\"right\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + dt.Columns[j].ColumnName + "\">" + dr1[j] + "</td>";

                            }
                            else
                            {
                                strHtml += "<td align=\"left\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + dt.Columns[j].ColumnName + "\">" + dr1[j] + "</td>";

                            }
                        }
                    }
                    else
                    {
                        strHtml += "<td>&nbsp;</td>";
                    }
                }

                strHtml += "</tr>";
            }
            strHtml += "</table>";
            if (ddlGeneration.SelectedItem.Value != "3")
            {
                DivHeader.InnerHtml = strHtmlheader;
                Divdisplay.InnerHtml = strHtml;
            }
            ViewState["mail"] = strHtmlheader + strHtml;

            //////if (RptType.ToString().Trim() != "Summary")
            //////{
            //////    ScriptManager.RegisterStartupScript(this, this.GetType(), "fnRecord", "fnRecord('5');", true);
            //////}
            //////else
            //////{
            //////    ScriptManager.RegisterStartupScript(this, this.GetType(), "fnRecord", "fnRecord('6');", true);
            //////}

        }
    }
}