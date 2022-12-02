using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using BusinessLogicLayer;

namespace ERP.OMS.Reports
{
    public partial class Reports_frmReport_DematPosition : System.Web.UI.Page, System.Web.UI.ICallbackEventHandler
    {
        BusinessLogicLayer.Converter oconverter = new BusinessLogicLayer.Converter();
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        DataSet ds = new DataSet();
        string data;
        ExcelFile objExcel = new ExcelFile();
        DataTable DT = new DataTable();

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
                Date();
                BindExchange();
                SettlementBind();
            }

            //_____For performing operation without refreshing page___//
            String cbReference = Page.ClientScript.GetCallbackEventReference(this, "arg", "ReceiveServerData", "context");
            String callbackScript = "function CallServer(arg, context){ " + cbReference + ";}";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "CallServer", callbackScript, true);
            //___________-end here___//

        }
        void Date()
        {
            DtFrom.EditFormatString = oconverter.GetDateFormat("Date");
            DtTo.EditFormatString = oconverter.GetDateFormat("Date");

            DtFrom.Value = Convert.ToDateTime(oDBEngine.GetDate().ToShortDateString());
            DtTo.Value = Convert.ToDateTime(oDBEngine.GetDate().ToShortDateString());
        }
        string ICallbackEventHandler.GetCallbackResult()
        {
            return data;
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
                if (idlist[0].ToString().Trim() == "Clients")
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

                else
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

            }
            data = idlist[0] + "~" + str;


        }
        public void BindExchange()
        {
            DdlExchange.Items.Clear();
            DataTable DtExchange = oDBEngine.GetDataTable("TBL_MASTER_COMPANYEXCHANGE", "Distinct Case When exch_exchid='EXN0000002' Then 'NSE' When exch_exchid='EXB0000001' Then 'BSE' When exch_exchid='EXC0000001' Then 'CSE' Else null End as ExchangeSegmentName,exch_internalid as Internalid", " EXCH_SEGMENTID='CM' and Exch_Compid='" + Session["LastCompany"].ToString() + "'");
            if (DtExchange.Rows.Count > 0)
            {
                DdlExchange.DataSource = DtExchange;
                DdlExchange.DataTextField = "ExchangeSegmentName";
                DdlExchange.DataValueField = "Internalid";
                DdlExchange.DataBind();
                DtExchange.Dispose();
                DdlExchange.Items.Insert(0, new ListItem("ALL", "0"));
                DdlExchange.Items.FindByValue(Session["usersegid"].ToString().Trim()).Selected = true;
            }
            else
                DdlExchange.Items.Insert(0, new ListItem("ALL", "0"));
        }
        public void SettlementBind()
        {
            DataTable DT = new DataTable();
            DT = oDBEngine.GetDataTable("Master_Settlements ", "RTRIM(settlements_Number)+RTRIM(settlements_TypeSuffix)+'  ' + REPLACE(CONVERT(VARCHAR(9), settlements_StartDateTime, 6), ' ', '-') AS [DD-Mon-YY],RTRIM(settlements_Number)+RTRIM(settlements_TypeSuffix)", "settlements_exchangesegmentid='" + HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() + "' and  Settlements_FinYear='" + HttpContext.Current.Session["LastFinYear"].ToString().Trim() + "' and RTRIM(settlements_Number)='" + Session["LastSettNo"].ToString().Substring(0, 7).ToString().Trim() + "' and RTRIM(settlements_TypeSuffix)='" + Session["LastSettNo"].ToString().Substring(7, 1).ToString().Trim() + "'");
            if (DT.Rows.Count > 0)
            {
                txtSettlements.Text = DT.Rows[0][0].ToString().Trim();
                txtSettlements_hidden.Text = DT.Rows[0][1].ToString().Trim();
            }

        }

        public void BindGroup()
        {
            DataTable DtGroup = oDBEngine.GetDataTable("tbl_master_groupmaster", "distinct gpm_Type", null);
            if (DtGroup.Rows.Count > 0)
            {
                DdlGrpType.DataSource = DtGroup;
                DdlGrpType.DataTextField = "gpm_Type";
                DdlGrpType.DataValueField = "gpm_Type";
                DdlGrpType.DataBind();
                DtGroup.Dispose();

            }

        }
        protected void BtnGroup_Click(object sender, EventArgs e)
        {
            if (DdlGroupBy.SelectedItem.Value.ToString().Trim() == "Group")
            {
                BindGroup();
            }
        }

        protected void BtnScreen_Click(object sender, EventArgs e)
        {
            SPCall();
        }
        protected void BtnExcel_Click(object sender, EventArgs e)
        {
            SPCall();
        }
        protected void BtnEmail_Click(object sender, EventArgs e)
        {
            SPCall();
        }
        DataSet Procedure()
        {
            string[] InputName = new string[17];
            string[] InputType = new string[17];
            string[] InputValue = new string[17];



            /////////////////Parameter Name
            InputName[0] = "Companyid";
            InputName[1] = "FinYear";
            InputName[2] = "Exchange";
            InputName[3] = "SearchBy";
            InputName[4] = "Settlements";
            InputName[5] = "SettNo";
            InputName[6] = "FromDate";
            InputName[7] = "ToDate";
            InputName[8] = "For";
            InputName[9] = "GrpType";
            InputName[10] = "GrpId";
            InputName[11] = "BranchHierchy";
            InputName[12] = "Asset";
            InputName[13] = "Scrip";
            InputName[14] = "TransferType";
            InputName[15] = "PositionType";
            InputName[16] = "OrderBy";


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
            InputType[14] = "V";
            InputType[15] = "V";
            InputType[16] = "V";





            /////////////////Parameter Value
            InputValue[0] = HttpContext.Current.Session["LastCompany"].ToString().Trim();
            InputValue[1] = HttpContext.Current.Session["LastFinYear"].ToString().Trim();
            InputValue[2] = DdlExchange.SelectedItem.Value.ToString().Trim() + '~' + DdlExchange.SelectedItem.Text.ToString().Trim();
            InputValue[3] = DdlSearchBy.SelectedItem.Value.ToString().Trim();
            InputValue[4] = DdlSettlements.SelectedItem.Value.ToString().Trim();
            InputValue[5] = txtSettlements_hidden.Text.ToString().Trim();
            InputValue[6] = DtFrom.Value.ToString().Trim();
            InputValue[7] = DtTo.Value.ToString().Trim();

            if (ChkForClients.Checked == true && ChkForExchange.Checked == true)
                InputValue[8] = "Both";
            if (ChkForClients.Checked == true && ChkForExchange.Checked == false)
                InputValue[8] = "Clients";
            if (ChkForClients.Checked == false && ChkForExchange.Checked == true)
                InputValue[8] = "Exchange";

            if (DdlGroupBy.SelectedItem.Value.ToString().Trim() == "Group")
                InputValue[9] = DdlGrpType.SelectedItem.Value.ToString().Trim();
            else
                InputValue[9] = DdlGroupBy.SelectedItem.Value.ToString().Trim();

            if (rdAll.Checked)
                InputValue[10] = "ALL";
            else
                InputValue[10] = HiddenField_ALL.Value.ToString().Trim();

            InputValue[11] = HttpContext.Current.Session["userbranchHierarchy"].ToString().Trim();

            if (RdAssetAll.Checked)
                InputValue[12] = "ALL";
            else
                InputValue[12] = HiddenField_Asset.Value.ToString().Trim();

            if (RdScripAll.Checked)
                InputValue[13] = "ALL";
            else
                InputValue[13] = HiddenField_Scrip.Value.ToString().Trim();

            if (RdbTransferTypeAll.Checked)
                InputValue[14] = "ALL";
            if (RdbTransferTypePending.Checked)
                InputValue[14] = "Pending";
            if (RdbTransferTypeTransferred.Checked)
                InputValue[14] = "Transferred";
            if (RdbTransferTypeFinalShortage.Checked)
                InputValue[14] = "Final Shortage";


            if (RdbPositionTypeIncoming.Checked)
                InputValue[15] = "Incoming";
            if (RdbPositionTypeOutgoing.Checked)
                InputValue[15] = "Outgoing";
            if (RdbPositionTypeBoth.Checked)
                InputValue[15] = "Both";

            InputValue[16] = DdlOrderBy.SelectedItem.Value.ToString().Trim();
            ds = BusinessLogicLayer.SQLProcedures.SelectProcedureArrDS("[Report_DeliveryPosition]", InputName, InputType, InputValue);

            return ds;
        }
        void SPCall()
        {
            ds = Procedure();
            if (ds.Tables[0].Rows.Count > 0)
            {
                string strheading = "Report Date : " + oconverter.ArrangeDate2(oDBEngine.GetDate().ToShortDateString()) + " ; ";
                /////////////////Heading Bind/////////////////////
                if (RdbTransferTypeAll.Checked)
                    strheading = strheading.ToString().Trim() + " Transfer Type:ALL";
                if (RdbTransferTypePending.Checked)
                    strheading = strheading.ToString().Trim() + " Transfer Type:Pending";
                if (RdbTransferTypeTransferred.Checked)
                    strheading = strheading.ToString().Trim() + " Transfer Type:Transferred";
                if (RdbTransferTypeFinalShortage.Checked)
                    strheading = strheading.ToString().Trim() + " Transfer Type:Final Shortage";


                if (RdbPositionTypeIncoming.Checked)
                    strheading = strheading.ToString().Trim() + " ; Poition Type :Incoming";
                if (RdbPositionTypeOutgoing.Checked)
                    strheading = strheading.ToString().Trim() + " ; Poition Type :Outgoing";
                if (RdbPositionTypeBoth.Checked)
                    strheading = strheading.ToString().Trim() + " ; Poition Type :Both";

                /////////////////////Heding End/////////////////////

                if (ddlGeneration.SelectedItem.Value.ToString().Trim() == "Screen")
                {
                    ds.Tables[0].Columns.Remove("GrpId");
                    ds.Tables[0].Columns.Remove("GrpEmailid");
                    ds.Tables[0].Columns.Remove("Clientid");
                    ViewState["DataSet"] = ds.Tables[0];
                    ViewState["strheading"] = strheading.ToString().Trim();
                    FnHtml(ds.Tables[0], strheading.ToString().Trim());

                }
                if (ddlGeneration.SelectedItem.Value.ToString().Trim() == "Export")
                {
                    ds.Tables[0].Columns.Remove("GrpId");
                    ds.Tables[0].Columns.Remove("GrpEmailid");
                    ds.Tables[0].Columns.Remove("Clientid");
                    Export(ds.Tables[0], strheading.ToString().Trim());
                }
                if (ddlGeneration.SelectedItem.Value.ToString().Trim() == "EMail")
                {
                    if (ddloptionformail.SelectedItem.Text.ToString().Trim() == "Branch/Group")
                    {
                        ds.Tables[0].Columns.Remove("Clientid");
                        FnBranchGroupEmail(ds.Tables[0], strheading.ToString().Trim());
                    }
                    if (ddloptionformail.SelectedItem.Text.ToString().Trim() == "User")
                    {
                        ds.Tables[0].Columns.Remove("Clientid");
                        FnUserEmail(ds.Tables[0], strheading.ToString().Trim());
                    }
                    if (ddloptionformail.SelectedItem.Text.ToString().Trim() == "Client")
                        Fnclientemail(ds.Tables[0], strheading.ToString().Trim());
                }
            }
            else
                ScriptManager.RegisterStartupScript(this, this.GetType(), "FnAlert", "FnAlert('No Record Found !!','1');", true);
        }
        string FnHtml(DataTable Dt, string strheading)
        {
            //////////For header
            String strHtmlheader = String.Empty;


            ////////////////Heading Bind//////////////////////
            strHtmlheader = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";
            strHtmlheader += "<tr><td align=\"left\" colspan=" + Dt.Columns.Count + " style=\"color:Blue;\">" + strheading.ToString().Trim() + "</td></tr>";
            strHtmlheader += "</tr></table>";

            ////////Detail Display
            String strHtml = String.Empty;
            strHtml = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";



            //////////////TABLE HEADER BIND
            strHtml += "<tr style=\"background-color: #DBEEF3;\">";
            for (int i = 0; i < Dt.Columns.Count; i++)
            {
                strHtml += "<td align=\"center\" style=\"font-size:smaller;\" nowrap=nowrap;><b>" + Dt.Columns[i].ColumnName + "</b></td>";
            }
            strHtml += "</tr>";

            int flag = 0;
            foreach (DataRow dr1 in Dt.Rows)
            {
                flag = flag + 1;
                strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
                for (int j = 0; j < Dt.Columns.Count; j++)
                {
                    if (dr1[j] != DBNull.Value)
                    {
                        if (IsNumeric(dr1[j].ToString()) == true)
                        {
                            strHtml += "<td align=\"right\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + Dt.Columns[j].ColumnName + "\">" + dr1[j] + "</td>";

                        }
                        else
                        {
                            strHtml += "<td align=\"left\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + Dt.Columns[j].ColumnName + "\">" + dr1[j] + "</td>";

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
            if (ddlGeneration.SelectedItem.Value.ToString().Trim() == "Screen")
            {
                DivHeader.InnerHtml = strHtmlheader;
                Divdisplay.InnerHtml = strHtml;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "FnAlert", "FnAlert('DisPlay'," + ds.Tables[0].Columns.Count + ");", true);
            }
            return strHtmlheader.ToString().Trim() + strHtml.ToString().Trim();
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
        void Export(DataTable dtExport, string strheading)
        {



            DataTable dtReportHeader = new DataTable();
            DataTable dtReportFooter = new DataTable();

            DataTable CompanyName = oDBEngine.GetDataTable("tbl_master_company", "cmp_Name", " cmp_internalId='" + Session["LastCompany"].ToString() + "'");
            dtReportHeader.Columns.Add(new DataColumn("Header", typeof(String))); //0
            DataRow HeaderRow = dtReportHeader.NewRow();
            HeaderRow[0] = strheading.ToString().Trim();
            dtReportHeader.Rows.Add(HeaderRow);


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

            objExcel.ExportToExcelforExcel(dtExport, "Delivery Position", "Total:", dtReportHeader, dtReportFooter);

        }

        void Fnclientemail(DataTable DtEmail, string strheading)
        {
            DataView Viewclient = new DataView(DtEmail);
            DataTable dtclient = Viewclient.ToTable(true, new string[] { "Clientid" });

            string MailOutPut = "Error on sending!Try again.. !!";
            for (int j = 0; j < dtclient.Rows.Count; j++)
            {
                Viewclient = DtEmail.DefaultView;
                Viewclient.RowFilter = "Clientid='" + dtclient.Rows[j][0].ToString().Trim() + "'";
                DataTable DT = new DataTable();
                DT = Viewclient.ToTable();
                DT.Columns.Remove("GrpId");
                DT.Columns.Remove("GrpEmailid");
                DT.Columns.Remove("Clientid");
                string Html = FnHtml(DT, strheading.ToString().Trim());

                // if (oDBEngine.SendReportBr(Html.ToString().Trim(), dtclient.Rows[j]["Clientid"].ToString().Trim(), oconverter.ArrangeDate2(oDBEngine.GetDate().ToShortDateString()), " Delivery Position", dtclient.Rows[j]["Clientid"].ToString().Trim()) == true)
                if (oDBEngine.SendReportSt(Html.ToString().Trim(), dtclient.Rows[j]["Clientid"].ToString().Trim(), oconverter.ArrangeDate2(oDBEngine.GetDate().ToShortDateString()), "Delivery Position") == true)
                    MailOutPut = "Mail Sent Successfully !!";
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "FnAlert", "FnAlert('" + MailOutPut.ToString().Trim() + "','1');", true);
        }
        void FnBranchGroupEmail(DataTable DtEmail, string strheading)
        {
            DataView ViewGrp = new DataView(DtEmail);
            DataTable DtGrpid = ViewGrp.ToTable(true, new string[] { "GrpId", "GrpEmailid" });

            string MailOutPut = "Error on sending!Try again.. !!";
            for (int j = 0; j < DtGrpid.Rows.Count; j++)
            {
                ViewGrp = DtEmail.DefaultView;
                ViewGrp.RowFilter = "GrpId='" + DtGrpid.Rows[j][0].ToString().Trim() + "'";
                DT = new DataTable();
                DT = ViewGrp.ToTable();
                DT.Columns.Remove("GrpId");
                DT.Columns.Remove("GrpEmailid");
                //DT.Columns.Remove("Clientid");
                string Html = FnHtml(DT, strheading.ToString().Trim());

                if (oDBEngine.SendReportBr(Html.ToString().Trim(), DtGrpid.Rows[j]["GrpEmailid"].ToString().Trim(), oconverter.ArrangeDate2(oDBEngine.GetDate().ToShortDateString()), " Delivery Position", DtGrpid.Rows[j]["GrpId"].ToString().Trim()) == true)
                    MailOutPut = "Mail Sent Successfully !!";
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "FnAlert", "FnAlert('" + MailOutPut.ToString().Trim() + "','1');", true);
        }
        void FnUserEmail(DataTable DtEmail, string strheading)
        {
            if (HiddenField_Email.Value.ToString().Trim() == "")
                ScriptManager.RegisterStartupScript(this, this.GetType(), "FnAlert", "FnAlert('E-Mail Id Could Not Be Found !!','1');", true);
            else
            {
                string MailOutPut = "Error on sending!Try again.. !!";
                DtEmail.Columns.Remove("GrpId");
                DtEmail.Columns.Remove("GrpEmailid");
                //DT.Columns.Remove("Clientid");

                string Html = FnHtml(DtEmail, strheading.ToString().Trim());
                string[] clnt = HiddenField_Email.Value.ToString().Split(',');
                int kk = clnt.Length;

                for (int i = 0; i < clnt.Length; i++)
                {
                    if (oDBEngine.SendReportSt(Html.ToString().Trim(), clnt[i].ToString().Trim(), oconverter.ArrangeDate2(oDBEngine.GetDate().ToShortDateString()), "Delivery Position") == true)
                        MailOutPut = "Mail Sent Successfully !!";
                }
                ScriptManager.RegisterStartupScript(this, this.GetType(), "FnAlert", "FnAlert('" + MailOutPut.ToString().Trim() + "','1');", true);
            }
        }
        protected void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable Dt = (DataTable)ViewState["DataSet"];
            Export(Dt, ViewState["strheading"].ToString().Trim());
        }
        protected void Button1_Click(object sender, EventArgs e)
        {
            //if (ddlGeneration.SelectedValue == "3")
            //{
            //tabRespective.Visible = true;
            //string strRptView = DLLRptView.SelectedItem.Text;
            if (ChkForExchange.Checked == true)
            {
                ddloptionformail.Items.Clear();



                ddloptionformail.Items.Add(new ListItem("Group/Branch", "3"));
                ddloptionformail.Items.Add(new ListItem("User", "1"));


            }

            else
            //else if ((DLLRptView.SelectedItem.Value == "7") || (DLLRptView.SelectedItem.Value == "8") || (DLLRptView.SelectedItem.Value == "9") || (DLLRptView.SelectedItem.Value == "10"))
            {
                if (DdlGroupBy.SelectedItem.Value == "Clients")
                {
                    ddloptionformail.Items.Clear();

                    ddloptionformail.Items.Add(new ListItem("Client", "3"));
                    ddloptionformail.Items.Add(new ListItem("Group/Branch", "2"));

                    ddloptionformail.Items.Add(new ListItem("User", "1"));
                }
                else
                {
                    ddloptionformail.Items.Add(new ListItem("Group/Branch", "2"));
                    ddloptionformail.Items.Add(new ListItem("Client", "3"));
                    ddloptionformail.Items.Add(new ListItem("User", "1"));
                }
                //cmbsearchOption.Visible = false;
            }

            //}
            // else
            //  tabRespective.Visible = false;
        }

    }
}