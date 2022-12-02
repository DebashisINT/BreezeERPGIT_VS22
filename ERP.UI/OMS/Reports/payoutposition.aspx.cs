using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Configuration;
using BusinessLogicLayer;

namespace ERP.OMS.Reports
{
    public partial class Reports_payoutposition : System.Web.UI.Page, System.Web.UI.ICallbackEventHandler
    {
        BusinessLogicLayer.Converter oconverter = new BusinessLogicLayer.Converter();
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        DataSet ds = new DataSet();

        ExcelFile objExcel = new ExcelFile();
        DataTable dtExport = new DataTable();
        string data;
        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //'http://localhost:2957/InfluxCRM/management/testProjectMainPage_employee.aspx'
                string sPath = HttpContext.Current.Request.Url.ToString();
                oDBEngine.Call_CheckPageaccessebility(sPath);
            }
        }
        #region ProPerty
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
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            if (HttpContext.Current.Session["userid"] == null)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
            if (!IsPostBack)
            {

                Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script language='javascript'>Page_Load();</script>");
                Date();
                ChkFO();
                ChkDP();
            }
            //_____For performing operation without refreshing page___//
            String cbReference = Page.ClientScript.GetCallbackEventReference(this, "arg", "ReceiveServerData", "context");
            String callbackScript = "function CallServer(arg, context){ " + cbReference + ";}";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "CallServer", callbackScript, true);
            //___________-end here___//

        }
        void Date()
        {
            DtFor.EditFormatString = oconverter.GetDateFormat("Date");
            DtFor.Value = Convert.ToDateTime(oDBEngine.GetDate().ToShortDateString());
            SegmentnameFetch();

        }
        void SegmentnameFetch()
        {
            if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "1")
                litSegmentMain.InnerText = "NSE-CM";
            if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "4")
                litSegmentMain.InnerText = "BSE-CM";
            if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "15")
                litSegmentMain.InnerText = "CSE-CM";

        }
        #region ChkFODP

        void ChkFO()
        {
            DataTable dtFOSeg = new DataTable();
            dtFOSeg = oDBEngine.GetDataTable("tbl_master_companyExchange", "exch_internalid", "exch_compId='" + Session["LastCompany"].ToString() + "' and exch_segmentid like 'FO%'");
            if (dtFOSeg.Rows.Count > 0)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "JScript1", "<script language='javascript'>FOExists('FO');</script>");
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "JScript2", "<script language='javascript'>FOExists('NOFO');</script>");
            }
        }
        void ChkDP()
        {
            DataTable dtDPSeg = new DataTable();
            dtDPSeg = oDBEngine.GetDataTable("tbl_master_companyExchange", "exch_internalid", "exch_compId='" + Session["LastCompany"].ToString() + "' and (exch_membershiptype like 'NSDL%' or exch_membershiptype like 'CDSL%')");
            if (dtDPSeg.Rows.Count > 0)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "JScript3", "<script language='javascript'>DpExists('DP');</script>");
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "JScript4", "<script language='javascript'>DpExists('NODP');</script>");
            }
        }
        #endregion


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
                    if (idlist[0].ToString().Trim() == "Clients")
                    {
                        string[] val = cl[i].Split(';');
                        string[] AcVal = val[0].Split('-');
                        if (str == "")
                        {
                            str = "'" + AcVal[0] + "'";
                            str1 = val[1];
                        }
                        else
                        {
                            str += ",'" + AcVal[0] + "'";
                            str1 += "," + val[1];
                        }


                    }
                    else
                    {
                        string[] val = cl[i].Split(';');
                        if (str == "")
                        {
                            str = val[0];
                            str1 = val[1];
                        }
                        else
                        {
                            str += "," + val[0];
                            str1 += "," + val[1];
                        }
                    }
                }


                data = idlist[0] + '~' + str;

            }
        }

        public void BindGroup()
        {
            DataTable DtGroup = oDBEngine.GetDataTable("tbl_master_groupmaster", "distinct gpm_Type", null);
            if (DtGroup.Rows.Count > 0)
            {
                ddlGroup.DataSource = DtGroup;
                ddlGroup.DataTextField = "gpm_Type";
                ddlGroup.DataValueField = "gpm_Type";
                ddlGroup.DataBind();
                DtGroup.Dispose();

            }

        }
        protected void BtnGroup_Click(object sender, EventArgs e)
        {
            if (ddlGroupBy.SelectedItem.Value.ToString().Trim() == "Group")
            {
                if (HiddenField_Group.Value.ToString().Trim() == "")
                {
                    BindGroup();
                }
            }
        }

        protected void btnScreen_Click(object sender, EventArgs e)
        {
            DisPlay("Screen");
        }
        protected void btnExcel_Click(object sender, EventArgs e)
        {
            DisPlay("Excel");
        }
        DataSet Procedure(string DisPlayType)
        {
            string[] InputName = new string[15];
            string[] InputType = new string[15];
            string[] InputValue = new string[15];



            /////////////////Parameter Name
            InputName[0] = "Companyid";
            InputName[1] = "Segment";
            InputName[2] = "ForDate";
            InputName[3] = "Clients";
            InputName[4] = "Finyear";
            InputName[5] = "GrpType";
            InputName[6] = "GrpId";
            InputName[7] = "BranchHierchy";
            InputName[8] = "Obligation";
            InputName[9] = "ChkFo";
            InputName[10] = "ChkDP";
            InputName[11] = "ChkAppMargin";
            InputName[12] = "ChkCashMargin";
            InputName[13] = "ReportType";
            InputName[14] = "AmntGreaterThan";

            /////////////////Parameter Data Type
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
            InputType[14] = "DE";

            /////////////////Parameter Value
            InputValue[0] = Session["LastCompany"].ToString().Trim();

            if (rdbSegmentAll.Checked)
                InputValue[1] = "ALL";
            else if (rdSegmentSelected.Checked)
                InputValue[1] = HiddenField_Segment.Value.ToString().Trim();
            else
                InputValue[1] = Session["usersegid"].ToString().Trim();

            InputValue[2] = DtFor.Value.ToString().Trim();

            if (ddlGroupBy.SelectedItem.Value.ToString().Trim() == "Clients")/////group type client selection
            {
                if (RadioBtnOtherGroupByAll.Checked)
                    InputValue[3] = "ALL";
                else
                    InputValue[3] = HiddenField_Client.Value.ToString().Trim();
            }
            else
                InputValue[3] = "ALL";

            InputValue[4] = Session["LastFinYear"].ToString().Trim();

            if (ddlGroupBy.SelectedItem.Value.ToString().Trim() == "Branch")/////group type branch selection
            {
                InputValue[5] = "BRANCH";
                if (RadioBtnOtherGroupByAll.Checked)
                    InputValue[6] = "ALL";
                else
                    InputValue[6] = HiddenField_Branch.Value.ToString().Trim();
            }
            else if (ddlGroupBy.SelectedItem.Value.ToString().Trim() == "BranchGroup")/////group type branch-group selection
            {
                InputValue[5] = "BRANCHGROUP";
                if (RadioBtnOtherGroupByAll.Checked)
                    InputValue[6] = "ALL";
                else
                    InputValue[6] = HiddenField_BranchGroup.Value.ToString().Trim();


            }
            else if (ddlGroupBy.SelectedItem.Value.ToString().Trim() == "Group")/////group type group selection
            {
                InputValue[5] = ddlGroup.SelectedItem.Text.ToString().Trim();
                if (RadioBtnGroupAll.Checked)
                    InputValue[6] = "ALL";
                else
                    InputValue[6] = HiddenField_Group.Value.ToString().Trim();

            }
            else                                /////group type client selection
            {
                InputValue[5] = "BRANCH";
                InputValue[6] = "ALL";
            }

            InputValue[7] = Session["userbranchHierarchy"].ToString().Trim();
            InputValue[8] = DdlObligationType.SelectedItem.Text.ToString().Trim();
            InputValue[9] = ChkFODr.Checked.ToString().Trim();
            InputValue[10] = ChkDPDr.Checked.ToString().Trim();
            InputValue[11] = ChkAppMrgn.Checked.ToString().Trim();
            InputValue[12] = ChkCashMrgn.Checked.ToString().Trim();
            InputValue[13] = "RPT";
            InputValue[14] = txtAmntGreaterThan.Text.ToString().Trim();
            //////////////Sp Call
            ds = SQLProcedures.SelectProcedureArrDS("[Report_PayOut]", InputName, InputType, InputValue);
            ViewState["dataset"] = ds;
            return ds;
        }
        DataSet CheckCurrentActivity()
        {
            string[] InputName = new string[4];
            string[] InputType = new string[4];
            string[] InputValue = new string[4];

            InputName[0] = "Companyid";
            InputName[1] = "Segment";
            InputName[2] = "Param";
            InputName[3] = "CreateUser";

            InputType[0] = "V";
            InputType[1] = "V";
            InputType[2] = "V";
            InputType[3] = "V";

            InputValue[0] = Session["LastCompany"].ToString().Trim();

            if (rdbSegmentAll.Checked)
                InputValue[1] = "ALL~CM";
            else if (rdSegmentSelected.Checked)
                InputValue[1] = HiddenField_Segment.Value.ToString().Trim() + "~CM";
            else
                InputValue[1] = Session["usersegid"].ToString().Trim() + "~CM";


            InputValue[2] = "N~Y~Y~Y~Y~Y~Y~Y~Y~Y~Y~Y";
            InputValue[3] = Session["userid"].ToString().Trim();

            DataSet DsCurrentActivity = SQLProcedures.SelectProcedureArrDS("Report_CheckCurrentActivity", InputName, InputType, InputValue);
            return DsCurrentActivity;

        }
        DataSet CheckBill()
        {
            string[] InputName = new string[4];
            string[] InputType = new string[4];
            string[] InputValue = new string[4];

            InputName[0] = "Companyid";
            InputName[1] = "Segment";
            InputName[2] = "ForDate";
            InputName[3] = "FinYear";

            InputType[0] = "V";
            InputType[1] = "V";
            InputType[2] = "V";
            InputType[3] = "V";

            InputValue[0] = Session["LastCompany"].ToString().Trim();

            if (rdbSegmentAll.Checked)
                InputValue[1] = "ALL~CM";
            else if (rdSegmentSelected.Checked)
                InputValue[1] = HiddenField_Segment.Value.ToString().Trim() + "~CM";
            else
                InputValue[1] = Session["usersegid"].ToString().Trim() + "~CM";

            InputValue[2] = DtFor.Value.ToString().Trim();
            InputValue[3] = Session["LastFinYear"].ToString().Trim();

            DataSet DsCurrentActivity = SQLProcedures.SelectProcedureArrDS("Report_MissingBills", InputName, InputType, InputValue);
            return DsCurrentActivity;

        }
        string FnChecking()
        {
            string Cheking = "N";
            ds = CheckCurrentActivity();
            if (ds.Tables[0].Rows.Count > 0)
            {
                FnCheckCurrentActivity(ds);
                Cheking = "Y";
            }
            ds = CheckBill();
            if (ds.Tables[0].Rows.Count > 0)
            {
                FnBillsCheck(ds);
                Cheking = "Y";
            }
            return Cheking;
        }
        void DisPlay(string DisPlayType)
        {
            string Cheking = FnChecking();
            if (Cheking.ToString().Trim() == "N")
            {
                ds = Procedure(DisPlayType.ToString().Trim());
                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (DisPlayType.ToString().Trim() == "Screen")
                        GrpBind(ds);
                    if (DisPlayType.ToString().Trim() == "Excel")
                        Export(ds);
                    if (DisPlayType.ToString().Trim() == "Email")
                        Email(ds);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "fnNoRecord", "fnRecord('1');", true);
                }
            }
        }
        void FnCheckCurrentActivity(DataSet ds)
        {
            String strHtml = String.Empty;
            strHtml = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";

            strHtml += "<tr>";
            strHtml += "<td style=\"color:Red;\" colspan=" + ds.Tables[0].Columns.Count + "><b>You Cannot Processed With This Routine as The Following Routines are live at this Moment......</b></td></tr>";


            //////////////TABLE HEADER BIND
            strHtml += "<tr style=\"background-color: #DBEEF3;\">";
            for (int i = 0; i < ds.Tables[0].Columns.Count; i++)
            {
                strHtml += "<td align=\"center\" style=\"font-size:smaller;\" nowrap=nowrap;><b>" + ds.Tables[0].Columns[i].ColumnName + "</b></td>";
            }
            strHtml += "</tr>";

            int flag = 0;
            foreach (DataRow dr1 in ds.Tables[0].Rows)
            {
                flag = flag + 1;
                strHtml += "<tr style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
                for (int j = 0; j < ds.Tables[0].Columns.Count; j++)
                {
                    if (dr1[j] != DBNull.Value)
                    {
                        strHtml += "<td align=\"left\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + ds.Tables[0].Columns[j].ColumnName + "\">" + dr1[j] + "</td>";
                    }
                    else
                    {
                        strHtml += "<td>&nbsp;</td>";
                    }
                }

                strHtml += "</tr>";
            }
            strHtml += "</table>";

            Div_Activity.InnerHtml = strHtml;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "fnNoRecord", "fnRecord('9');", true);
        }
        void FnBillsCheck(DataSet ds)
        {
            String strHtml = String.Empty;
            strHtml = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";

            strHtml += "<tr>";
            strHtml += "<td style=\"color:Red;\" colspan=" + ds.Tables[0].Columns.Count + "><b>Bills for following Settlements not generated.Please Generate These Bills to continue with the current routine....</b></td></tr>";


            //////////////TABLE HEADER BIND
            strHtml += "<tr style=\"background-color: #DBEEF3;\">";
            for (int i = 0; i < ds.Tables[0].Columns.Count; i++)
            {
                strHtml += "<td align=\"center\" style=\"font-size:smaller;\" nowrap=nowrap;><b>" + ds.Tables[0].Columns[i].ColumnName + "</b></td>";
            }
            strHtml += "</tr>";

            int flag = 0;
            foreach (DataRow dr1 in ds.Tables[0].Rows)
            {
                flag = flag + 1;
                strHtml += "<tr style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
                for (int j = 0; j < ds.Tables[0].Columns.Count; j++)
                {
                    if (dr1[j] != DBNull.Value)
                    {
                        strHtml += "<td align=\"left\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + ds.Tables[0].Columns[j].ColumnName + "\">" + dr1[j] + "</td>";
                    }
                    else
                    {
                        strHtml += "<td>&nbsp;</td>";
                    }
                }

                strHtml += "</tr>";
            }


            ///////////////MissMatch Bills Begin
            if (ds.Tables[1].Rows.Count > 0)
            {
                strHtml += "<tr>";
                strHtml += "<td style=\"color:Red;\" colspan=" + ds.Tables[1].Columns.Count + "><b>Bills for following Settlements not proper..Please Generate them before Proceeding further...</b></td></tr>";


                //////////////TABLE HEADER BIND
                strHtml += "<tr style=\"background-color: #DBEEF3;\">";
                for (int i = 0; i < ds.Tables[1].Columns.Count; i++)
                {
                    strHtml += "<td align=\"center\" style=\"font-size:smaller;\" nowrap=nowrap;><b>" + ds.Tables[1].Columns[i].ColumnName + "</b></td>";
                }
                strHtml += "</tr>";

                flag = 0;
                foreach (DataRow dr1 in ds.Tables[1].Rows)
                {
                    flag = flag + 1;
                    strHtml += "<tr style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
                    for (int j = 0; j < ds.Tables[1].Columns.Count; j++)
                    {
                        if (dr1[j] != DBNull.Value)
                        {
                            strHtml += "<td align=\"left\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + ds.Tables[1].Columns[j].ColumnName + "\">" + dr1[j] + "</td>";
                        }
                        else
                        {
                            strHtml += "<td>&nbsp;</td>";
                        }
                    }

                    strHtml += "</tr>";
                }
                strHtml += "</table>";
            }
            ///////////////MissMatch Bills End

            Div_bill.InnerHtml = strHtml;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "fnNoRecord", "fnRecord('9');", true);
        }
        void GrpBind(DataSet ds)
        {
            DataView viewData = new DataView();
            viewData = ds.Tables[0].DefaultView;
            viewData.RowFilter = " GrpName<>'ZZZZZZZZZZZZZZ'";
            DataTable dt = new DataTable();
            dt = viewData.ToTable();

            DataTable DistinctRecord = new DataTable();
            DataView viewRecord = new DataView(dt);
            DistinctRecord = viewRecord.ToTable(true, new string[] { "GrpName" });

            if (DistinctRecord.Rows.Count > 0)
            {
                cmbgrp.DataSource = DistinctRecord;
                cmbgrp.DataValueField = "GrpName";
                cmbgrp.DataTextField = "GrpName";
                cmbgrp.DataBind();

            }
            LastPage = DistinctRecord.Rows.Count - 1;
            CurrentPage = 0;

            if (DistinctRecord.Rows.Count > 0)
            {
                bind_Details();
            }


        }
        void bind_Details()
        {
            cmbgrp.SelectedIndex = CurrentPage;
            ds = (DataSet)ViewState["dataset"];
            Divdisplay.InnerHtml = FnHtml(ds, cmbgrp.SelectedItem.Value.ToString());
            ShowHidePreviousNext_of_Clients();
        }
        string FnHtml(DataSet ds, string GrpName)
        {
            //////////For header
            String strHtmlheader = String.Empty;
            string str = null;
            str = "Report For " + oconverter.ArrangeDate2(DtFor.Value.ToString());

            strHtmlheader = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";
            strHtmlheader += "<tr><td align=\"left\" colspan=" + ds.Tables[0].Columns.Count + " style=\"color:Blue;\">" + str + "</td></tr></table>";


            ////////Detail Display
            String strHtml = String.Empty;
            strHtml = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";




            DataView viewGrp = new DataView();
            string ColName = null;
            viewGrp = ds.Tables[0].DefaultView;
            viewGrp.RowFilter = "GrpName='" + GrpName.ToString().Trim() + "'";
            DataTable dt = new DataTable();
            dt = viewGrp.ToTable();

            dt.Columns.Remove("ClientName");
            dt.Columns.Remove("GrpName");
            dt.Columns.Remove("StOrder");
            dt.Columns.Remove("GrpId");
            dt.Columns.Remove("GrpEmail");

            strHtml += "<tr style=\"background-color:lavender ;text-align:left\">";
            strHtml += "<td align=\"left\" style=\"font-size:smaller;\"  nowrap=nowrap; ><b>" + dt.Rows[0][0].ToString().Trim() + "</b></td>";
            strHtml += "</tr>";

            dt.Rows[0].Delete();

            //////////////TABLE HEADER BIND
            strHtml += "<tr style=\"background-color: #DBEEF3;\">";
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                if (dt.Columns[i].ColumnName == "Net Payable")
                {
                    if (DdlObligationType.SelectedIndex == 0) ColName = "Net Rec/Pay";
                    if (DdlObligationType.SelectedIndex == 1) ColName = "Net Payable";
                    if (DdlObligationType.SelectedIndex == 2) ColName = "Net Receivable";
                }
                else
                {
                    ColName = dt.Columns[i].ColumnName;
                }
                strHtml += "<td align=\"center\" style=\"font-size:smaller;\" nowrap=nowrap;><b>" + ColName + "</b></td>";
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
                        if (dr1[j].ToString().Trim().StartsWith("Total:") || dr1[j].ToString().Trim().StartsWith("Grand Total:"))
                        {
                            strHtml += "<td align=\"left\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + dt.Columns[j].ColumnName + "\"><b>" + dr1[j] + "</b></td>";
                        }
                        else if (dr1[j].ToString().Trim().StartsWith("Test") || dr1[j].ToString().Trim().StartsWith("ZZ"))
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
            if (ddlGeneration.SelectedItem.Value.ToString().Trim() == "1")
            {
                DivHeader.InnerHtml = strHtmlheader;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "fnRecord", "fnRecord('2');", true);
            }

            return strHtml;


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
        void Export(DataSet ds)
        {
            DataTable dtExport = ds.Tables[0].Copy();
            dtExport.Columns.Remove("ClientName");
            dtExport.Columns.Remove("GrpName");
            dtExport.Columns.Remove("StOrder");
            dtExport.Columns.Remove("GrpId");
            dtExport.Columns.Remove("GrpEmail");

            for (int i = 0; i < dtExport.Columns.Count; i++)
            {
                if (dtExport.Columns[i].ColumnName.Contains("</BR>"))
                {
                    dtExport.Columns[i].ColumnName = dtExport.Columns[i].ColumnName.Replace("</BR>", "/");
                }

            }


            string str = null;
            str = "Report For " + oconverter.ArrangeDate2(DtFor.Value.ToString());


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

            objExcel.ExportToExcelforExcel(dtExport, "PayOut Position", "Total:", dtReportHeader, dtReportFooter);

        }
        protected void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            ds = (DataSet)ViewState["dataset"];
            Export(ds);
        }
        protected void btnEmail_Click(object sender, EventArgs e)
        {
            DisPlay("Email");
        }
        void Email(DataSet ds)
        {
            if (ddloptionformail.SelectedItem.Value.ToString().Trim() == "1")////////Branch Group
            {
                EmailBranchGroup(ds);
            }
            if (ddloptionformail.SelectedItem.Value.ToString().Trim() == "2")///////User WIse
            {
                EmailUserWiseEmail(ds);
            }
        }
        void EmailBranchGroup(DataSet ds)
        {
            string strEmail = "GrpMail", mailsendresult = "no";

            DataView viewData = new DataView();
            viewData = ds.Tables[0].DefaultView;
            viewData.RowFilter = " GrpName<>'ZZZZZZZZZZZZZZ'";
            DataTable dt = new DataTable();
            dt = viewData.ToTable();

            DataTable DistinctRecord = new DataTable();
            DataView viewRecord = new DataView(dt);
            DistinctRecord = viewRecord.ToTable(true, new string[] { "GrpName", "GrpEmail", "GrpId" });

            if (DistinctRecord.Rows.Count > 0)
            {
                cmbgrp.DataSource = DistinctRecord;
                cmbgrp.DataValueField = "GrpName";
                cmbgrp.DataTextField = "GrpName";
                cmbgrp.DataBind();

            }
            if (ddlGroupBy.SelectedItem.Value != "Group")
            {

                for (int k = 0; k < cmbgrp.Items.Count; k++)
                {
                    strEmail = FnHtml(ds, cmbgrp.Items[k].Value.ToString().Trim());

                    if (oDBEngine.SendReportBr(strEmail.ToString().Trim(), DistinctRecord.Rows[k]["GrpEmail"].ToString().Trim(), oconverter.ArrangeDate2(DtFor.Value.ToString()), "PayOut Position For : [" + oconverter.ArrangeDate2(DtFor.Value.ToString().Trim()) + "]", DistinctRecord.Rows[k]["GrpId"].ToString().Trim()) == true)
                    {
                        if (mailsendresult.ToString().Trim() == "errorsuccess")
                            mailsendresult = "someclienterror";
                        else
                            mailsendresult = "success";
                    }
                    else
                        mailsendresult = "errorsuccess";

                }

                if (mailsendresult.ToString().Trim() == "someclienterror")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "fnRecord", "fnRecord(6);", true);
                }
                if (mailsendresult.ToString().Trim() == "success")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "fnRecord", "fnRecord(4);", true);
                }
                if (mailsendresult.ToString().Trim() == "errorsuccess")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "fnRecord", "fnRecord(5);", true);
                }
            }
            else
            {
                string groupby = ddlGroup.SelectedItem.Text.ToString().Trim();
                for (int k = 0; k < cmbgrp.Items.Count; k++)
                {
                    strEmail = FnHtml(ds, cmbgrp.Items[k].Value.ToString().Trim());
                    //string dttest = oDBEngine.GetFieldValue("select gpm_code from TBL_MASTER_GROUPMASTER where gpm_id = '"+ DistinctRecord.Rows[k]["GrpId"].ToString().Trim() +"'");
                    string[,] dttest = oDBEngine.GetFieldValue("TBL_MASTER_GROUPMASTER", "gpm_code", "gpm_id = '" + DistinctRecord.Rows[k]["GrpId"].ToString().Trim() + "'", 1);
                    if (oDBEngine.SendReportBrpayout(strEmail.ToString().Trim(), DistinctRecord.Rows[k]["GrpEmail"].ToString().Trim(), oconverter.ArrangeDate2(DtFor.Value.ToString()), "PayOut Position For : [" + oconverter.ArrangeDate2(DtFor.Value.ToString().Trim()) + "]", DistinctRecord.Rows[k]["GrpId"].ToString().Trim(), groupby, dttest[0, 0]) == true)
                    {
                        if (mailsendresult.ToString().Trim() == "errorsuccess")
                            mailsendresult = "someclienterror";
                        else
                            mailsendresult = "success";
                    }
                    else
                        mailsendresult = "errorsuccess";

                }
                if (mailsendresult.ToString().Trim() == "someclienterror")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "fnRecord", "fnRecord(6);", true);
                }
                if (mailsendresult.ToString().Trim() == "success")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "fnRecord", "fnRecord(4);", true);
                }
                if (mailsendresult.ToString().Trim() == "errorsuccess")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "fnRecord", "fnRecord(5);", true);
                }

            }
        }
        void EmailUserWiseEmail(DataSet ds)
        {
            if (HiddenField_emmail.Value.ToString().Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "RecordStatus5", "RecordStatus(7);", true);
            }
            else
            {
                string strEmail = "UserMail", MainstrEmail = "UserMail", mailsendresult = "no";


                DataView viewData = new DataView();
                viewData = ds.Tables[0].DefaultView;
                viewData.RowFilter = " GrpName<>'ZZZZZZZZZZZZZZ'";
                DataTable dt = new DataTable();
                dt = viewData.ToTable();

                DataTable DistinctRecord = new DataTable();
                DataView viewRecord = new DataView(dt);
                DistinctRecord = viewRecord.ToTable(true, new string[] { "GrpName" });

                if (DistinctRecord.Rows.Count > 0)
                {
                    cmbgrp.DataSource = DistinctRecord;
                    cmbgrp.DataValueField = "GrpName";
                    cmbgrp.DataTextField = "GrpName";
                    cmbgrp.DataBind();

                }

                for (int k = 0; k < cmbgrp.Items.Count; k++)
                {
                    strEmail = FnHtml(ds, cmbgrp.Items[k].Value.ToString().Trim());
                    if (MainstrEmail.ToString().Trim() == "UserMail")
                        MainstrEmail = strEmail.ToString().Trim();
                    else
                        MainstrEmail = MainstrEmail.ToString().Trim() + strEmail.ToString().Trim();

                }
                string[] clnt = HiddenField_emmail.Value.ToString().Split(',');
                int kk = clnt.Length;
                for (int i = 0; i < clnt.Length; i++)
                {
                    if (oDBEngine.SendReportSt(MainstrEmail.ToString().Trim(), clnt[i].ToString().Trim(), oconverter.ArrangeDate2(DtFor.Value.ToString().Trim()), "PayOut Position For : [" + oconverter.ArrangeDate2(DtFor.Value.ToString().Trim()) + "]") == true)
                    {
                        if (mailsendresult.ToString().Trim() == "errorsuccess")
                            mailsendresult = "someclienterror";
                        else
                            mailsendresult = "success";
                    }
                    else
                        mailsendresult = "errorsuccess";
                }

                if (mailsendresult.ToString().Trim() == "someclienterror")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "fnRecord", "fnRecord(6);", true);
                }
                if (mailsendresult.ToString().Trim() == "success")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "fnRecord", "fnRecord(4);", true);
                }
                if (mailsendresult.ToString().Trim() == "errorsuccess")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "fnRecord", "fnRecord(5);", true);
                }
            }
        }
    }
}