using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;

namespace ERP.OMS.Reports
{

    public partial class Reports_frmReport_cdslHolding : System.Web.UI.Page, System.Web.UI.ICallbackEventHandler
    {
        BusinessLogicLayer.Reports objReports = new BusinessLogicLayer.Reports();
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);
        BusinessLogicLayer.Converter objConverter = new BusinessLogicLayer.Converter();
        BusinessLogicLayer.Converter OConvert = new BusinessLogicLayer.Converter();
        //ReportDocument AttendenceReportDocu = new ReportDocument();
        private static DataTable dates;
        public int chkStatus;
        PagedDataSource pds = new PagedDataSource();
        string data;
        //---------For Email
        static DataTable dtEmail = new DataTable();
        static DataTable dtMail = new DataTable();
        BusinessLogicLayer.Converter oconverter = new BusinessLogicLayer.Converter();
        ExcelFile objExcel = new ExcelFile();
        static decimal VALUE_Sum;
        static string User;

        //DBEngine oDBEngine = new DBEngine(string.Empty);
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

            //________This script is for firing javascript when page load first___//
            if (!ClientScript.IsStartupScriptRegistered("Today"))
                ClientScript.RegisterStartupScript(typeof(Page), "Today", "<script>PageLoad();</script>");
            // ______________________________End Script____________________________//

            if (!IsPostBack)
            {
                populateCmbDate();
                populateCmbTime();
                // txtName.Attributes.Add("onkeyup", "CallAjax(this,'CDSLHolding',event)");
                txtISIN.Attributes.Add("onkeyup", "CallAjax(this,'cdslholdingisin',event)");
                txtSettlement.Attributes.Add("onkeyup", "CallAjax(this,'CDSLHoldingSettlement',event)");

                t1.Visible = false;
                t11.Visible = false;
                Label5.Visible = false;

                head.Style["display"] = "none";

                //this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");
            }
            bindRepeter();
            txtDate.EditFormatString = OConvert.GetDateFormat("Date");

            rbClientUser.Attributes.Add("OnClick", "User('User')");
            rbOnlyClient.Attributes.Add("OnClick", "User('NoUser')");
            rbRspctvClient.Attributes.Add("OnClick", "User('NoUser')");

            String cbReference1 = Page.ClientScript.GetCallbackEventReference(this, "arg", "ReceiveSvrData", "context");
            String callbackScript1 = "function CallServer1(arg, context){ " + cbReference1 + ";}";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "CallServer1", callbackScript1, true);
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
                if (idlist[0] != "Clients")
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
                else
                {

                    string[] val = cl[i].Split(';');
                    string[] AcVal = val[0].Split('-');
                    if (str == "")
                    {

                        if (idlist[0] == "User")
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

                        if (idlist[0] == "User")
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
            else if (idlist[0] == "Group")
            {
                data = "Group~" + str;
            }
            else if (idlist[0] == "Branch")
            {
                data = "Branch~" + str;
            }
            else if (idlist[0] == "User")
            {
                User = str;
                data = "User~" + str;
            }



        }
        protected void populateCmbDate()
        {
            dates = oDBEngine.GetDataTable(" Trans_CdslHolding ", " distinct CdslHolding_HoldingDateTime as date, CdslHolding_HoldingDateTime ", null, "CdslHolding_HoldingDateTime desc");
            if (dates.Rows.Count > 0)
            {
                txtDate.Value = Convert.ToDateTime(dates.Rows[0][0].ToString());
            }
        }

        protected void populateCmbTime()
        {
            cmbTime.Items.Clear();
            //cmbTime.DataSource = oDBEngine.GetDataTable(" Trans_CdslHolding ", "distinct substring(convert(varchar(20), CdslHolding_HoldingDateTime, 9), 13, 5)+ ' ' + substring(convert(varchar(30), CdslHolding_HoldingDateTime, 9), 25, 2) as time,CdslHolding_HoldingDateTime", " CAST(DAY(CdslHolding_HoldingDateTime) AS VARCHAR(2)) + ' ' + DATENAME(MM, CdslHolding_HoldingDateTime) + ' ' + CAST(YEAR(CdslHolding_HoldingDateTime) AS VARCHAR(4)) = '" + dt + "' ", " CdslHolding_HoldingDateTime desc");
            //cmbTime.DataSource = oDBEngine.GetDataTable(" Trans_CdslHolding ", "distinct substring(convert(varchar(20), CdslHolding_HoldingDateTime, 9), 13, 5)+ ' ' + substring(convert(varchar(30), CdslHolding_HoldingDateTime, 9), 25, 2) as time,CdslHolding_HoldingDateTime", " CAST(DAY(CdslHolding_HoldingDateTime) AS VARCHAR(2)) + ' ' + DATENAME(MM, CdslHolding_HoldingDateTime) + ' ' + CAST(YEAR(CdslHolding_HoldingDateTime) AS VARCHAR(4)) = '" + txtDate.Value.ToString() + "' ", " CdslHolding_HoldingDateTime desc");
            cmbTime.DataSource = oDBEngine.GetDataTable(" Trans_CdslHolding ", "distinct substring(convert(varchar(20), CdslHolding_HoldingDateTime, 9), 13, 5)+ ' ' + substring(convert(varchar(30), CdslHolding_HoldingDateTime, 9), 25, 2) as time,CdslHolding_HoldingDateTime", " convert(varchar(12),CdslHolding_HoldingDateTime,113) = convert(varchar(12),cast('" + txtDate.Value.ToString() + "' as datetime),113) ", " CdslHolding_HoldingDateTime desc");
            cmbTime.ValueField = "CdslHolding_HoldingDateTime";
            cmbTime.TextField = "time";
            cmbTime.DataBind();
            cmbTime.SelectedIndex = 0;
        }
        protected void btnShow_Click(object sender, EventArgs e)
        {
            CurrentPage = 0;
            ViewState["dtable"] = null;
            FetchBoid();
            bindGrid();
        }

        void FetchBoid()
        {
            string orderBy = "";
            DataTable dtable = new DataTable();
            string[] dat = cmbTime.Value.ToString().Split(' ');
            /*
            string where = " not exists(Select 1 " +
                          " from Trans_CdslTransaction " +
                          " where CdslHolding_HoldingDateTime<CdslTransaction_StatementDateTime and " +
                          " isnull(CdslTransaction_SettlementID,1)=isnull(CdslHolding_SettlementID,1) " +
                          "	and CdslTransaction_ISIN=cdslholding_ISIN " +
                          " and CdslTransaction_BenAccountNumber=CdslHolding_BenAccountNumber" +
                          " and CdslTransaction_StatementDateTime between  '" + dat[0] + " 00:00:00 AM'  and '" + cmbTime.Value + "')" +
                          " and Master_CdslClients.CdslClients_BOID=LTRIM(RTRIM(CdslHolding_DPID))+ LTRIM(RTRIM(CdslHolding_BenAccountNumber)) ";
            */

            string where = " CdslClients_BOID=LTRIM(RTRIM(CdslHolding_DPID))+ LTRIM(RTRIM(CdslHolding_BenAccountNumber)) ";

            where = where + "and CdslHolding_HoldingDateTime between '" + dat[0] + " 00:00:00 AM'  and '" + cmbTime.Value + "'";
            //where =where+ " and CdslClients_BOStatus='"++"'";

            //if (txtName_hidden.Text.Trim() != "")
            //{

            //   // where = where + " and CdslClients_BOID='" + txtName_hidden.Text + "'";

            //    where = where + " and CdslClients_BOID  in (" + Clients + ")";
            //  //  Clients

            //}

            if (rbUser.SelectedItem.Value.ToString() == "S")
            {
                where = where + " and CdslClients_BOID  in (" + hidClients.Value + ")";
            }
            if (txtISIN_hidden.Text.Trim() != "")
            {
                where = where + " and CdslHolding_ISIN='" + txtISIN_hidden.Text + "'";
            }
            if (txtSettlement_hidden.Text.Trim() != "")
            {
                where = where + " and CdslHolding_SettlementID='" + txtSettlement_hidden.Text + "'";
            }
            if (ASPxComboBox1.Value != "All")
            {
                where = where + " and CdslClients_BOStatus='" + ASPxComboBox1.Value + "' ";
            }
            //where = where + " group by CdslClients_BOID ,CdslClients_FirstHolderName,isnull(CdslClients_SecondHolderName,'N/A')  ,isnull(CdslClients_ThirdHolderName,'N/A')  ";
            //txtName_hidden.Text = "";
            txtName.Text = "";
            rbUser.SelectedIndex = 0;


            txtISIN.Text = "";
            rbISIN.SelectedIndex = 0;

            txtSettlement.Text = "";
            rbSettlement.SelectedIndex = 0;
            where = where + "and cdslClients_BranchID in (" + HttpContext.Current.Session["userbranchHierarchy"].ToString() + ")";

            if (ddlGroup.SelectedItem.Value.ToString() == "0")
            {

                if (rdbranchAll.Checked)
                {

                    dtable = oDBEngine.GetDataTable(" Trans_CdslHolding,Master_CdslClients ", "  distinct( CdslClients_BOID),CdslClients_FirstHolderName,isnull(CdslClients_SecondHolderName,'N/A') as CdslClients_SecondHolderName,isnull(CdslClients_ThirdHolderName,'N/A') as CdslClients_ThirdHolderName", where, null);

                }
                else
                {
                    where = where + "and cdslClients_BranchID in (" + hidBranch.Value + ")";
                    orderBy = " Master_CdslClients.cdslClients_BranchID  ";
                    dtable = oDBEngine.GetDataTable(" Trans_CdslHolding,Master_CdslClients ", "  distinct( CdslClients_BOID),CdslClients_FirstHolderName,isnull(CdslClients_SecondHolderName,'N/A') as CdslClients_SecondHolderName,isnull(CdslClients_ThirdHolderName,'N/A') as CdslClients_ThirdHolderName,cdslClients_BranchID as grp_groupMaster", where, orderBy);
                    //orderBy = " Master_NsdlClients.NsdlClients_BranchID  ";
                    //  dt = oDBEngine.GetDataTable(" Trans_NsdlHolding,Master_NsdlClients ", "  distinct( NsdlClients_BenAccountID),NsdlClients_BenFirstHolderName,case when len(ltrim(rtrim(NsdlClients_BenSecondHolderName)))=0 then 'N/A' else ltrim(rtrim(NsdlClients_BenSecondHolderName)) end as NsdlClients_BenSecondHolderName,case when len(ltrim(rtrim(NsdlClients_BenThirdHolderName)))=0 then 'N/A' else ltrim(rtrim(NsdlClients_BenThirdHolderName)) end as NsdlClients_BenThirdHolderName,NsdlClients_BenType,Master_NsdlClients.NsdlClients_BranchID as grp_groupMaster ", where, orderBy);

                }
            }
            else
            {

                if (rdddlgrouptypeAll.Checked)
                {
                    if (ddlgrouptype.SelectedItem.Value.ToString() != "0")
                    {
                        where = where + "and cdslClients_BOID=grp_contactId and grp_groupType='" + ddlgrouptype.SelectedItem.Text.ToString().Trim() + "' and grp_contactId like '1%'";
                        orderBy = " tbl_trans_group.grp_groupMaster  ";
                        dtable = oDBEngine.GetDataTable(" Trans_CdslHolding,Master_CdslClients ,tbl_trans_group ", "  distinct( CdslClients_BOID),CdslClients_FirstHolderName,isnull(CdslClients_SecondHolderName,'N/A') as CdslClients_SecondHolderName,isnull(CdslClients_ThirdHolderName,'N/A') as CdslClients_ThirdHolderName,grp_groupMaster ", where, orderBy);

                        //where = where + "and NsdlClients_DPID +Cast(NsdlClients_BenAccountID as Varchar(10))=grp_contactId and grp_groupType='" + ddlgrouptype.SelectedItem.Text.ToString().Trim() + "' and grp_contactId like 'IN%'";

                        //dt = oDBEngine.GetDataTable(" Trans_NsdlHolding,Master_NsdlClients,tbl_trans_group ", "  distinct( NsdlClients_BenAccountID),NsdlClients_BenFirstHolderName,case when len(ltrim(rtrim(NsdlClients_BenSecondHolderName)))=0 then 'N/A' else ltrim(rtrim(NsdlClients_BenSecondHolderName)) end as NsdlClients_BenSecondHolderName,case when len(ltrim(rtrim(NsdlClients_BenThirdHolderName)))=0 then 'N/A' else ltrim(rtrim(NsdlClients_BenThirdHolderName)) end as NsdlClients_BenThirdHolderName,NsdlClients_BenType", where, orderBy);

                    }
                    else
                    {
                        dtable = oDBEngine.GetDataTable(" Trans_CdslHolding,Master_CdslClients ,tbl_trans_group ", "  distinct( CdslClients_BOID),CdslClients_FirstHolderName,isnull(CdslClients_SecondHolderName,'N/A') as CdslClients_SecondHolderName,isnull(CdslClients_ThirdHolderName,'N/A') as CdslClients_ThirdHolderName", where, null);
                        //dt = oDBEngine.GetDataTable(" Trans_NsdlHolding,Master_NsdlClients", "  distinct( NsdlClients_BenAccountID),NsdlClients_BenFirstHolderName,case when len(ltrim(rtrim(NsdlClients_BenSecondHolderName)))=0 then 'N/A' else ltrim(rtrim(NsdlClients_BenSecondHolderName)) end as NsdlClients_BenSecondHolderName,case when len(ltrim(rtrim(NsdlClients_BenThirdHolderName)))=0 then 'N/A' else ltrim(rtrim(NsdlClients_BenThirdHolderName)) end as NsdlClients_BenThirdHolderName,NsdlClients_BenType", where, orderBy);

                    }

                }
                else
                {

                    where = where + "and cdslClients_BOID=grp_contactId and grp_groupMaster in (" + hidGroup.Value + ") and grp_contactId like '1%'";
                    orderBy = " tbl_trans_group.grp_groupMaster  ";
                    dtable = oDBEngine.GetDataTable(" Trans_CdslHolding,Master_CdslClients ,tbl_trans_group", "  distinct( CdslClients_BOID),CdslClients_FirstHolderName,isnull(CdslClients_SecondHolderName,'N/A') as CdslClients_SecondHolderName,isnull(CdslClients_ThirdHolderName,'N/A') as CdslClients_ThirdHolderName,grp_groupMaster", where, orderBy);
                }


            }

            //  DataTable dtable = oDBEngine.GetDataTable(" Trans_CdslHolding,Master_CdslClients ", "  distinct( CdslClients_BOID),CdslClients_FirstHolderName,isnull(CdslClients_SecondHolderName,'N/A') as CdslClients_SecondHolderName,isnull(CdslClients_ThirdHolderName,'N/A') as CdslClients_ThirdHolderName", where, null);
            dtEmail = dtable;
            ViewState["dtable"] = dtable;

        }

        void bindGrid()
        {
            DataTable dt = (DataTable)ViewState["dtable"];

            pds.DataSource = dt.DefaultView;
            pds.AllowPaging = true;
            pds.PageSize = 1;
            pds.CurrentPageIndex = CurrentPage;
            ASPxFirst.Enabled = !pds.IsFirstPage;
            ASPxNext.Enabled = !pds.IsLastPage;
            ASPxPrevious.Enabled = !pds.IsFirstPage;
            ASPxLast.Enabled = !pds.IsLastPage;
            ASPxFirst1.Enabled = !pds.IsFirstPage;
            ASPxNext1.Enabled = !pds.IsLastPage;
            ASPxPrevious1.Enabled = !pds.IsFirstPage;
            ASPxLast1.Enabled = !pds.IsLastPage;

            LastPage = pds.PageCount - 1;

            //lnkbtnNext.Enabled = !pds.IsLastPage;
            //lnkbtnPrevious.Enabled = !pds.IsFirstPage;




            // DataList1.DataSource = pds;
            // DataList1.DataBind();
            //doPaging();
            Label1.Text = "Total " + pds.DataSourceCount.ToString() + " Record Found.";
            Label11.Text = "Total " + pds.DataSourceCount.ToString() + " Record Found.";
            if (pds.DataSourceCount < 1)
            {
                Label5.Text = "NO Record Found.";
                Label5.Visible = true;
                lblBoID.Text = "";
                bindRepeter();

            }
            else
            {
                lblBoID.Text = dt.DefaultView.Table.Rows[pds.CurrentPageIndex][0].ToString();
                lblBoName.Text = dt.DefaultView.Table.Rows[pds.CurrentPageIndex][1].ToString();
                lblSecondHolder.Text = dt.DefaultView.Table.Rows[pds.CurrentPageIndex][2].ToString();
                lblThirdHolder.Text = dt.DefaultView.Table.Rows[pds.CurrentPageIndex][3].ToString();
                head.Style["display"] = "display";
                bindRepeter();
                Label5.Visible = false;
            }

            if (pds.PageCount > 1)
            {

                t1.Visible = true;
                t11.Visible = true;
            }
            else
            {
                t1.Visible = false;
                t11.Visible = false;


            }

            ScriptManager.RegisterStartupScript(this, this.GetType(), "hi", "height();", true);

        }

        void bindRepeter()
        {
            try
            {
                string startTime = string.Empty;
                string endTime = string.Empty;
                string isin = string.Empty;
                string settlementId = string.Empty;
                string boid = string.Empty;

                string select, where, id, sql;
                id = lblBoID.Text;
                string[] date = cmbTime.Value.ToString().Split(' ');
                startTime = date[0] + " 00:00:00 AM";
                endTime = Convert.ToString(cmbTime.Value);
                if (txtISIN_hidden.Text.Trim() != "")
                {
                    isin = txtISIN_hidden.Text.ToString();
                }
                else
                {
                    isin = "NA";
                }

                if (txtSettlement_hidden.Text.Trim() != "")
                {
                    settlementId = "'" + txtSettlement_hidden.Text + "'";
                }
                else
                {
                    settlementId = "NA";
                }

                if (lblBoID.Text.Length == 16)
                {
                    boid = "'" + lblBoID.Text.Substring(8) + "'";
                }
                else
                {
                    boid = "noid";
                }
                DataTable dt1 = new DataTable();

                dt1 = objReports.cdslHoldingReport1(startTime, endTime, isin, settlementId, boid);


                for (int k = 0; k < dt1.Rows.Count; k++)
                {
                    if (dt1.Rows[k]["CdslHolding_CurrentBalance"] != DBNull.Value)
                        dt1.Rows[k]["CdslHolding_CurrentBalance"] = objConverter.getFormattedvalueWithCheckingDecimalPlaceOriginalSign(Convert.ToDecimal(dt1.Rows[k]["CdslHolding_CurrentBalance"].ToString()));

                    if (dt1.Rows[k]["CdslHolding_FreeBalance"] != DBNull.Value)
                        dt1.Rows[k]["CdslHolding_FreeBalance"] = objConverter.getFormattedvalueWithCheckingDecimalPlaceOriginalSign(Convert.ToDecimal(dt1.Rows[k]["CdslHolding_FreeBalance"].ToString()));


                    if (dt1.Rows[k]["CdslHolding_PledgeBalance"] != DBNull.Value)
                        dt1.Rows[k]["CdslHolding_PledgeBalance"] = objConverter.getFormattedvalueWithCheckingDecimalPlaceOriginalSign(Convert.ToDecimal(dt1.Rows[k]["CdslHolding_PledgeBalance"].ToString()));


                    if (dt1.Rows[k]["CdslHolding_EarmarkedBalance"] != DBNull.Value)
                        dt1.Rows[k]["CdslHolding_EarmarkedBalance"] = objConverter.getFormattedvalueWithCheckingDecimalPlaceOriginalSign(Convert.ToDecimal(dt1.Rows[k]["CdslHolding_EarmarkedBalance"].ToString()));


                    if (dt1.Rows[k]["CdslHolding_PendingRematBalance"] != DBNull.Value)
                        dt1.Rows[k]["CdslHolding_PendingRematBalance"] = objConverter.getFormattedvalueWithCheckingDecimalPlaceOriginalSign(Convert.ToDecimal(dt1.Rows[k]["CdslHolding_PendingRematBalance"].ToString()));


                    if (dt1.Rows[k]["CdslHolding_PendingDematBalance"] != DBNull.Value)
                        dt1.Rows[k]["CdslHolding_PendingDematBalance"] = objConverter.getFormattedvalueWithCheckingDecimalPlaceOriginalSign(Convert.ToDecimal(dt1.Rows[k]["CdslHolding_PendingDematBalance"].ToString()));

                    if (dt1.Rows[k]["Rate"] != DBNull.Value)
                        dt1.Rows[k]["Rate"] = objConverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dt1.Rows[k]["Rate"].ToString()));

                    if (dt1.Rows[k]["ISINVAlue"] != DBNull.Value)
                        dt1.Rows[k]["ISINVAlue"] = objConverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dt1.Rows[k]["ISINVAlue"].ToString()));
                    //VALUE_Sum += Convert.ToDecimal(dt1.Rows[k]["ISINVAlue"].ToString());
                    //ViewState["ISINVAlue"] = VALUE_Sum;
                }
                ////oDBEngine1.GetConnection();
                //SqlDataAdapter lda = new SqlDataAdapter(sql, con);
                //// createing an object of datatable
                //DataTable dt1 = new DataTable();
                //lda.Fill(dt1);
                //oSqlConnection.Close();

                // DataView dv = dt1.DefaultView;

                if ((ASPxComboBox1.Value.ToString() == "Clearing Member") || (ASPxComboBox1.Value.ToString() == "All"))
                {
                    gridHolding.Columns[2].Visible = true;
                }
                else
                {
                    gridHolding.Columns[2].Visible = false;
                }

                // gridHolding.Columns[9].Assign(
                // gridHolding.FilterEnabled = false;

                //gridHolding.SettingsPager.PageSize = dt1.Rows.Count;

                gridHolding.DataSource = dt1;
                gridHolding.DataBind();
                dtMail = dt1;
                //con.Dispose();
                //lda.Dispose();
                //

                gridHolding.FilterExpression = string.Empty;

            }
            catch (Exception ex)
            {
            }

        }

        protected void Button1_Click(object sender, EventArgs e)
        {

            string disptbl = "";
            string emlTbl = "";
            string mailid = string.Empty;
            string dpid = string.Empty;
            string emlcnt = string.Empty;
            string emailbdy = "";
            string branchContact = "";
            string billdate = oconverter.ArrangeDate2(txtDate.Value.ToString());
            string Subject = "CDSL Holding Report for" + billdate;


            //ViewState["dtable"]
            for (int i = 0; i < dtEmail.Rows.Count; i++)
            {
                dtMail.Clear();
                CurrentPage = i;
                bindGrid();
                ViewState["ISINVAlue"] = null;
                VALUE_Sum = 0;

                disptbl = "<table width=\"1050px\" style=\"font-size:8pt;\" cellspacing=\"0\" cellpadding=\"0\" border=\"1\">";
                disptbl += "<tr><td colspan=\"11\"><table width=\"1050px\" style=\"font-size:10pt;\" cellspacing=\"0\" cellpadding=\"0\"><tr><td align=\"left\"><b>BO ID:" + dtEmail.Rows[i][0].ToString() + "</b></td><td align=\"left\"><b> BO Name:" + dtEmail.Rows[i][1].ToString() + "</b></td><td align=\"left\"><b>Second Holder:" + dtEmail.Rows[i][2].ToString() + "</b></td><td align=\"left\"><b>Third Holder:" + dtEmail.Rows[i][3].ToString() + "</b></td></tr></table></td></tr>";
                disptbl += "<tr style=\"background-color:#DBEEF3;\"><td align=\"center\"><b>ISIN No.</b></td><td align=\"center\"><b>Instrument </br> Name (Short)</b></td><td align=\"center\"><b>SettlementID</b></td><td align=\"center\"><b>Current </br> Balance</b></td><td align=\"center\"><b>Free</b></td><td align=\"center\"><b>Pledged</b></td><td align=\"center\"><b>Blocked </br> [Earmark]</td><td align=\"center\"><b>Pending </br>Remat</b></td><td align=\"center\"><b>Pending </br>Demat</b></td><td align=\"center\"><b>Rate</b></td><td align=\"center\"><b>Value</b></td></tr>";
                for (int j = 0; j < dtMail.Rows.Count; j++)
                {
                    disptbl += "<tr><td>" + dtMail.Rows[j]["CdslHolding_ISIN"] + "</td><td>" + dtMail.Rows[j]["CDSLISIN_ShortName"] + "</td><td>&nbsp;" + dtMail.Rows[j]["CdslHolding_SettlementID"] + "</td><td align=\"right\">&nbsp;" + dtMail.Rows[j]["CdslHolding_CurrentBalance"] + "</td><td align=\"right\">&nbsp;" + dtMail.Rows[j]["CdslHolding_FreeBalance"] + "</td><td align=\"right\">&nbsp;" + dtMail.Rows[j]["CdslHolding_PledgeBalance"] + "</td><td align=\"right\">&nbsp;" + dtMail.Rows[j]["CdslHolding_EarmarkedBalance"] + "</td><td align=\"right\">&nbsp;" + dtMail.Rows[j]["CdslHolding_PendingRematBalance"] + "</td><td align=\"right\">&nbsp;" + dtMail.Rows[j]["CdslHolding_PendingDematBalance"] + "</td><td align=\"right\">&nbsp;" + dtMail.Rows[j]["Rate"] + "</td><td align=\"right\">&nbsp;" + dtMail.Rows[j]["ISINVAlue"] + "</td></tr>";

                    if (dtMail.Rows[j]["ISINVAlue"].ToString().Trim() != "" || dtMail.Rows[j]["ISINVAlue"].ToString().Trim().Length > 0)
                    {
                        VALUE_Sum += Convert.ToDecimal(dtMail.Rows[j]["ISINVAlue"].ToString());
                        ViewState["ISINVAlue"] = VALUE_Sum;

                    }


                }
                if (ViewState["ISINVAlue"] != null)
                {
                    if (ViewState["ISINVAlue"].ToString().ToString().Length > 0)
                    {
                        disptbl += "<tr style=\"background-color:#DBEEF3;\"><td><b>Total Holding Value : </b></td><td>&nbsp;</td><td>&nbsp;</td><td align=\"right\">&nbsp;</td><td align=\"right\">&nbsp;</td><td align=\"right\">&nbsp;</td><td align=\"right\">&nbsp;</td><td align=\"right\">&nbsp;</td><td align=\"right\">&nbsp;</td><td align=\"right\">&nbsp;</td><td align=\"right\">&nbsp;<b>" + objConverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(ViewState["ISINVAlue"].ToString())) + "</b></td></tr><tr><td colspan=11>&nbsp;</td></tr>";
                    }
                }
                disptbl += "</table>";

                if (rbRspctvClient.Checked)
                {
                    Subject = "CDSL Holding Report for" + billdate + "[" + dtEmail.Rows[i]["CdslClients_BOID"].ToString().Trim() + "]";
                    DataTable dtCnt = oDBEngine.GetDataTable(" tbl_master_email  ", "  * ", "eml_cntId='" + dtEmail.Rows[i]["CdslClients_BOID"] + "'");

                    //if (dtCnt.Rows.Count > 0)
                    //{
                    //    mailid = dtCnt.Rows[0]["eml_email"].ToString().Trim();
                    //}
                    emailbdy = disptbl;
                    branchContact = dtEmail.Rows[i]["CdslClients_BOID"].ToString().Trim();

                    for (int k = 0; k < dtCnt.Rows.Count; k++)
                    {
                        mailid = dtCnt.Rows[k]["eml_email"].ToString().Trim();
                        if (mailid.Length > 0)
                        {

                            if (oDBEngine.SendReportBr(emailbdy, mailid, billdate, Subject, branchContact) == true)
                            {
                                this.Page.ClientScript.RegisterStartupScript(GetType(), "jscript78", "<script>ForFilterOff();</script>");
                                this.Page.ClientScript.RegisterStartupScript(GetType(), "jscript6", "<script>MailsendT();</script>");

                            }
                            else
                            {
                                if (dtEmail.Rows.Count <= 1)
                                {
                                    this.Page.ClientScript.RegisterStartupScript(GetType(), "jscript75", "<script>ForFilterOff();</script>");
                                    this.Page.ClientScript.RegisterStartupScript(GetType(), "jscript9", "<script>MailsendF();</script>");
                                }

                            }
                        }
                        else
                        {
                            this.Page.ClientScript.RegisterStartupScript(GetType(), "jscript15", "<script>ForFilterOff();</script>");
                            this.Page.ClientScript.RegisterStartupScript(GetType(), "jscript94", "<script>alert('Mailid Not found..');</script>");
                        }
                    }
                    disptbl = "";

                }
                else if (rbOnlyClient.Checked)
                {

                    if (rdddlgrouptypeSelected.Checked)
                    {

                        if (i < dtEmail.Rows.Count - 1)
                        {
                            if (dtEmail.Rows[i]["grp_groupMaster"].ToString() == dtEmail.Rows[i + 1]["grp_groupMaster"].ToString())
                            {
                                emlTbl += "<table width=\"1050px\" style=\"font-size:8pt;\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\"><tr><td> " + disptbl + "</td></tr></table>";
                                disptbl = "";
                            }
                            else
                            {
                                emlTbl += "<table width=\"1050px\" style=\"font-size:8pt;\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\"><tr><td> " + disptbl + "</td></tr></table>";
                                // DataTable dp = oDBEngine.GetDataTable(" master_nsdlclients  ", "  top 1  *  ", "NsdlClients_BenAccountID='" + dt.Rows[i]["NsdlClients_BenAccountId"] + "'");
                                //  dpid = dp.Rows[0]["NsdlClients_DpID"].ToString().Trim();
                                //  emlcnt = dp.Rows[0]["NsdlClients_DpID"].ToString().Trim() + "" + dt.Rows[i]["NsdlClients_BenAccountId"].ToString().Trim();
                                DataTable dtCnt = oDBEngine.GetDataTable(" tbl_master_groupmaster  ", "    *  ", "gpm_id='" + dtEmail.Rows[i]["grp_groupMaster"].ToString() + "'");
                                emailbdy = emlTbl;
                                branchContact = dtEmail.Rows[i]["grp_groupMaster"].ToString();
                                emlTbl = "";
                                disptbl = "";
                                if (dtCnt.Rows.Count > 0)
                                {
                                    mailid = dtCnt.Rows[0]["gpm_emailID"].ToString().Trim();
                                    if (mailid.Length > 0)
                                    {

                                        if (oDBEngine.SendReportBr(emailbdy, mailid, billdate, Subject, branchContact) == true)
                                        {

                                            string ccemailid = dtCnt.Rows[0]["gpm_ccemailID"].ToString().Trim();
                                            if (ccemailid.Length > 0)
                                            {
                                                oDBEngine.SendReportBr(emailbdy, ccemailid, billdate, Subject, branchContact);
                                            }
                                            emlTbl = "";
                                            disptbl = "";
                                            this.Page.ClientScript.RegisterStartupScript(GetType(), "jscript78", "<script>ForFilterOff();</script>");
                                            this.Page.ClientScript.RegisterStartupScript(GetType(), "jscript6", "<script>MailsendT();</script>");

                                        }
                                        else
                                        {
                                            if (dtEmail.Rows.Count <= 1)
                                            {
                                                this.Page.ClientScript.RegisterStartupScript(GetType(), "jscript75", "<script>ForFilterOff();</script>");
                                                this.Page.ClientScript.RegisterStartupScript(GetType(), "jscript9", "<script>MailsendF();</script>");
                                            }

                                        }
                                    }
                                    else
                                    {
                                        this.Page.ClientScript.RegisterStartupScript(GetType(), "jscript85", "<script>ForFilterOff();</script>");
                                        this.Page.ClientScript.RegisterStartupScript(GetType(), "jscript74", "<script>alert('Recipients Mail id not found...');</script>");

                                    }
                                }

                                emlTbl = "";
                                disptbl = "";

                            }

                        }
                        else if (i == dtEmail.Rows.Count - 1)
                        {
                            if (dtEmail.Rows[i]["grp_groupMaster"].ToString() == dtEmail.Rows[i - 1]["grp_groupMaster"].ToString())
                            {
                                emlTbl += "<table width=\"1050px\" style=\"font-size:8pt;\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\"><tr><td> " + disptbl + "</td></tr></table>";
                                // DataTable dp = oDBEngine.GetDataTable(" master_nsdlclients  ", "  top 1  *  ", "NsdlClients_BenAccountID='" + dt.Rows[i]["NsdlClients_BenAccountId"] + "'");
                                //  dpid = dp.Rows[0]["NsdlClients_DpID"].ToString().Trim();
                                //  emlcnt = dp.Rows[0]["NsdlClients_DpID"].ToString().Trim() + "" + dt.Rows[i]["NsdlClients_BenAccountId"].ToString().Trim();
                                DataTable dtCnt = oDBEngine.GetDataTable(" tbl_master_groupmaster  ", "    *  ", "gpm_id='" + dtEmail.Rows[i]["grp_groupMaster"].ToString() + "'");
                                emailbdy = emlTbl;
                                branchContact = dtEmail.Rows[i]["grp_groupMaster"].ToString();
                                emlTbl = "";
                                disptbl = "";
                                if (dtCnt.Rows.Count > 0)
                                {
                                    mailid = dtCnt.Rows[0]["gpm_emailID"].ToString().Trim();
                                    if (mailid.Length > 0)
                                    {


                                        if (oDBEngine.SendReportBr(emailbdy, mailid, billdate, Subject, branchContact) == true)
                                        {

                                            string ccemailid = dtCnt.Rows[0]["gpm_ccemailID"].ToString().Trim();
                                            if (ccemailid.Length > 0)
                                            {
                                                oDBEngine.SendReportBr(emailbdy, ccemailid, billdate, Subject, branchContact);
                                            }
                                            emlTbl = "";
                                            this.Page.ClientScript.RegisterStartupScript(GetType(), "jscript78", "<script>ForFilterOff();</script>");
                                            this.Page.ClientScript.RegisterStartupScript(GetType(), "jscript6", "<script>MailsendT();</script>");

                                        }
                                        else
                                        {
                                            if (dtEmail.Rows.Count <= 1)
                                            {
                                                this.Page.ClientScript.RegisterStartupScript(GetType(), "jscript75", "<script>ForFilterOff();</script>");
                                                this.Page.ClientScript.RegisterStartupScript(GetType(), "jscript9", "<script>MailsendF();</script>");
                                            }

                                        }
                                    }
                                    else
                                    {
                                        this.Page.ClientScript.RegisterStartupScript(GetType(), "jscript85", "<script>ForFilterOff();</script>");
                                        this.Page.ClientScript.RegisterStartupScript(GetType(), "jscript74", "<script>alert('Recipients Mail id not found...');</script>");

                                    }
                                }

                                emlTbl = "";
                                disptbl = "";
                            }
                            else
                            {
                                emlTbl = "<table width=\"1050px\" style=\"font-size:8pt;\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\"><tr><td> " + disptbl + "</td></tr></table>";
                                // DataTable dp = oDBEngine.GetDataTable(" master_nsdlclients  ", "  top 1  *  ", "NsdlClients_BenAccountID='" + dt.Rows[i]["NsdlClients_BenAccountId"] + "'");
                                //  dpid = dp.Rows[0]["NsdlClients_DpID"].ToString().Trim();
                                //  emlcnt = dp.Rows[0]["NsdlClients_DpID"].ToString().Trim() + "" + dt.Rows[i]["NsdlClients_BenAccountId"].ToString().Trim();

                                DataTable dtCnt = oDBEngine.GetDataTable(" tbl_master_groupmaster  ", "    *  ", "gpm_id='" + dtEmail.Rows[i]["grp_groupMaster"].ToString() + "'");
                                emailbdy = emlTbl;
                                branchContact = dtEmail.Rows[i]["grp_groupMaster"].ToString().Trim();
                                emlTbl = "";
                                disptbl = "";
                                if (dtCnt.Rows.Count > 0)
                                {
                                    mailid = dtCnt.Rows[0]["gpm_emailID"].ToString().Trim();
                                    if (mailid.Length > 0)
                                    {
                                        this.Page.ClientScript.RegisterStartupScript(GetType(), "jscript78", "<script>ForFilterOff();</script>");
                                        this.Page.ClientScript.RegisterStartupScript(GetType(), "jscript6", "<script>MailsendT();</script>");

                                        if (oDBEngine.SendReportBr(emailbdy, mailid, billdate, Subject, branchContact) == true)
                                        {

                                            string ccemailid = dtCnt.Rows[0]["gpm_ccemailID"].ToString().Trim();
                                            if (ccemailid.Length > 0)
                                            {
                                                oDBEngine.SendReportBr(emailbdy, ccemailid, billdate, Subject, branchContact);
                                            }
                                            emlTbl = "";
                                            this.Page.ClientScript.RegisterStartupScript(GetType(), "jscript78", "<script>ForFilterOff();</script>");
                                            this.Page.ClientScript.RegisterStartupScript(GetType(), "jscript6", "<script>MailsendT();</script>");

                                        }
                                        else
                                        {
                                            if (dtEmail.Rows.Count <= 1)
                                            {
                                                this.Page.ClientScript.RegisterStartupScript(GetType(), "jscript75", "<script>ForFilterOff();</script>");
                                                this.Page.ClientScript.RegisterStartupScript(GetType(), "jscript9", "<script>MailsendF();</script>");
                                            }

                                        }
                                    }
                                    else
                                    {
                                        this.Page.ClientScript.RegisterStartupScript(GetType(), "jscript85", "<script>ForFilterOff();</script>");
                                        this.Page.ClientScript.RegisterStartupScript(GetType(), "jscript74", "<script>alert('Recipients Mail id not found...');</script>");

                                    }
                                }

                                emlTbl = "";
                                disptbl = "";

                            }

                        }
                    }
                    else if (rdbranchSelected.Checked)
                    {
                        if (i < dtEmail.Rows.Count - 1)
                        {
                            if (dtEmail.Rows[i]["grp_groupMaster"].ToString() == dtEmail.Rows[i + 1]["grp_groupMaster"].ToString())
                            {
                                emlTbl += "<table width=\"1050px\" style=\"font-size:8pt;\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\"><tr><td> " + disptbl + "</td></tr></table>";
                                disptbl = "";
                            }
                            else
                            {
                                emlTbl += "<table width=\"1050px\" style=\"font-size:8pt;\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\"><tr><td> " + disptbl + "</td></tr></table>";
                                // DataTable dp = oDBEngine.GetDataTable(" master_nsdlclients  ", "  top 1  *  ", "NsdlClients_BenAccountID='" + dt.Rows[i]["NsdlClients_BenAccountId"] + "'");
                                //  dpid = dp.Rows[0]["NsdlClients_DpID"].ToString().Trim();
                                //  emlcnt = dp.Rows[0]["NsdlClients_DpID"].ToString().Trim() + "" + dt.Rows[i]["NsdlClients_BenAccountId"].ToString().Trim();
                                DataTable dtCnt = oDBEngine.GetDataTable(" TBL_MASTER_BRANCH  ", "  top 1 *  ", "BRANCH_CODE='" + dtEmail.Rows[i]["grp_groupMaster"].ToString() + "'");
                                emailbdy = emlTbl;
                                branchContact = dtCnt.Rows[0]["branch_head"].ToString().Trim();
                                emlTbl = "";
                                disptbl = "";
                                if (dtCnt.Rows.Count > 0)
                                {
                                    mailid = dtCnt.Rows[0]["branch_cpEmail"].ToString().Trim();
                                    if (mailid.Length > 0)
                                    {

                                        if (oDBEngine.SendReportBr(emailbdy, mailid, billdate, Subject, branchContact) == true)
                                        {


                                            this.Page.ClientScript.RegisterStartupScript(GetType(), "jscript78", "<script>ForFilterOff();</script>");
                                            this.Page.ClientScript.RegisterStartupScript(GetType(), "jscript6", "<script>MailsendT();</script>");

                                        }
                                        else
                                        {
                                            if (dtEmail.Rows.Count <= 1)
                                            {
                                                this.Page.ClientScript.RegisterStartupScript(GetType(), "jscript75", "<script>ForFilterOff();</script>");
                                                this.Page.ClientScript.RegisterStartupScript(GetType(), "jscript9", "<script>MailsendF();</script>");
                                            }

                                        }
                                    }
                                    else
                                    {
                                        this.Page.ClientScript.RegisterStartupScript(GetType(), "jscript85", "<script>ForFilterOff();</script>");
                                        this.Page.ClientScript.RegisterStartupScript(GetType(), "jscript74", "<script>alert('Recipients Mail id not found...');</script>");

                                    }
                                }

                                emlTbl = "";
                                disptbl = "";

                            }

                        }
                        else if (i == dtEmail.Rows.Count - 1)
                        {
                            if (dtEmail.Rows[i]["grp_groupMaster"].ToString() == dtEmail.Rows[i - 1]["grp_groupMaster"].ToString())
                            {
                                emlTbl += "<table width=\"1050px\" style=\"font-size:8pt;\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\"><tr><td> " + disptbl + "</td></tr></table>";
                                // DataTable dp = oDBEngine.GetDataTable(" master_nsdlclients  ", "  top 1  *  ", "NsdlClients_BenAccountID='" + dt.Rows[i]["NsdlClients_BenAccountId"] + "'");
                                //  dpid = dp.Rows[0]["NsdlClients_DpID"].ToString().Trim();
                                //  emlcnt = dp.Rows[0]["NsdlClients_DpID"].ToString().Trim() + "" + dt.Rows[i]["NsdlClients_BenAccountId"].ToString().Trim();
                                DataTable dtCnt = oDBEngine.GetDataTable(" TBL_MASTER_BRANCH  ", "  top 1 *  ", "branch_id='" + dtEmail.Rows[i]["grp_groupMaster"].ToString() + "'");
                                emailbdy = emlTbl;
                                branchContact = dtCnt.Rows[0]["branch_head"].ToString().Trim();
                                emlTbl = "";
                                disptbl = "";
                                if (dtCnt.Rows.Count > 0)
                                {
                                    mailid = dtCnt.Rows[0]["branch_cpEmail"].ToString().Trim();
                                    if (mailid.Length > 0)
                                    {


                                        if (oDBEngine.SendReportBr(emailbdy, mailid, billdate, Subject, branchContact) == true)
                                        {


                                            this.Page.ClientScript.RegisterStartupScript(GetType(), "jscript78", "<script>ForFilterOff();</script>");
                                            this.Page.ClientScript.RegisterStartupScript(GetType(), "jscript6", "<script>MailsendT();</script>");

                                        }
                                        else
                                        {
                                            if (dtEmail.Rows.Count <= 1)
                                            {
                                                this.Page.ClientScript.RegisterStartupScript(GetType(), "jscript75", "<script>ForFilterOff();</script>");
                                                this.Page.ClientScript.RegisterStartupScript(GetType(), "jscript9", "<script>MailsendF();</script>");
                                            }

                                        }
                                    }
                                    else
                                    {
                                        this.Page.ClientScript.RegisterStartupScript(GetType(), "jscript85", "<script>ForFilterOff();</script>");
                                        this.Page.ClientScript.RegisterStartupScript(GetType(), "jscript74", "<script>alert('Recipients Mail id not found...');</script>");

                                    }
                                }

                                emlTbl = "";
                                disptbl = "";
                            }
                            else
                            {
                                emlTbl += "<table width=\"1050px\" style=\"font-size:8pt;\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\"><tr><td> " + disptbl + "</td></tr></table>";
                                // DataTable dp = oDBEngine.GetDataTable(" master_nsdlclients  ", "  top 1  *  ", "NsdlClients_BenAccountID='" + dt.Rows[i]["NsdlClients_BenAccountId"] + "'");
                                //  dpid = dp.Rows[0]["NsdlClients_DpID"].ToString().Trim();
                                //  emlcnt = dp.Rows[0]["NsdlClients_DpID"].ToString().Trim() + "" + dt.Rows[i]["NsdlClients_BenAccountId"].ToString().Trim();
                                DataTable dtCnt = oDBEngine.GetDataTable(" TBL_MASTER_BRANCH  ", "  top 1 *  ", "branch_id='" + dtEmail.Rows[i]["grp_groupMaster"].ToString() + "'");
                                emailbdy = emlTbl;
                                branchContact = dtCnt.Rows[0]["branch_head"].ToString().Trim();
                                emlTbl = "";
                                disptbl = "";
                                if (dtCnt.Rows.Count > 0)
                                {
                                    mailid = dtCnt.Rows[0]["branch_cpEmail"].ToString().Trim();
                                    if (mailid.Length > 0)
                                    {
                                        if (oDBEngine.SendReportBr(emailbdy, mailid, billdate, Subject, branchContact) == true)
                                        {

                                            this.Page.ClientScript.RegisterStartupScript(GetType(), "jscript78", "<script>ForFilterOff();</script>");
                                            this.Page.ClientScript.RegisterStartupScript(GetType(), "jscript6", "<script>MailsendT();</script>");

                                        }
                                        else
                                        {
                                            if (dtEmail.Rows.Count <= 1)
                                            {
                                                this.Page.ClientScript.RegisterStartupScript(GetType(), "jscript75", "<script>ForFilterOff();</script>");
                                                this.Page.ClientScript.RegisterStartupScript(GetType(), "jscript9", "<script>MailsendF();</script>");
                                            }

                                        }
                                    }
                                    else
                                    {
                                        this.Page.ClientScript.RegisterStartupScript(GetType(), "jscript85", "<script>ForFilterOff();</script>");
                                        this.Page.ClientScript.RegisterStartupScript(GetType(), "jscript74", "<script>alert('Recipients Mail id not found...');</script>");

                                    }
                                }

                                emlTbl = "";
                                disptbl = "";

                            }

                        }

                    }




                }
                else if (rbClientUser.Checked)
                {

                    if (i == dtEmail.Rows.Count - 1)
                    {
                        emlTbl += disptbl;
                        disptbl = "";
                        if (User != "")
                        {
                            string[] clnt = User.ToString().Split(',');
                            for (int m = 0; m < clnt.Length; m++)
                            {
                                emailbdy = emlTbl;
                                string contactid = clnt[m].ToString();

                                emlTbl = "";
                                disptbl = "";
                                if (oDBEngine.SendReportSt(emailbdy, contactid, billdate, Subject) == true)
                                {

                                    this.Page.ClientScript.RegisterStartupScript(GetType(), "jscript78", "<script>ForFilterOff();</script>");
                                    this.Page.ClientScript.RegisterStartupScript(GetType(), "jscript6", "<script>MailsendT();</script>");

                                }
                                else
                                {
                                    this.Page.ClientScript.RegisterStartupScript(GetType(), "jscript78", "<script>ForFilterOff();</script>");
                                    this.Page.ClientScript.RegisterStartupScript(GetType(), "jscript6", "<script>MailsendF();</script>");

                                }
                            }
                        }
                    }
                    else
                    {
                        emlTbl += disptbl;
                        disptbl = "";

                    }

                }

            }

            CurrentPage = 0;
            bindGrid();






        }

        void export()
        {
            DataTable dtEx = new DataTable();
            DataTable dtTemp = new DataTable();
            dtEx.Columns.Add("ISIN No.");
            dtEx.Columns.Add("Instrument Name (Short)");
            dtEx.Columns.Add("Settlement ID");
            dtEx.Columns.Add("Current Balance");
            dtEx.Columns.Add("Free");
            dtEx.Columns.Add("Pledged");
            dtEx.Columns.Add("Blocked[Earmark]");
            dtEx.Columns.Add("Pending Remat");
            dtEx.Columns.Add("Pending Demat");
            dtEx.Columns.Add("Rate");
            dtEx.Columns.Add("Value");

            for (int i = 0; i < dtEmail.Rows.Count; i++)
            {
                dtMail.Clear();
                CurrentPage = i;
                bindGrid();
                DataRow row3 = dtEx.NewRow();
                row3[0] = "Beneficiary AccountID: " + dtEmail.Rows[i][0].ToString() + "   ||  " + "Beneficiary Name:  " + dtEmail.Rows[i][1].ToString() + "  ||  " + "Second Holder:  " + dtEmail.Rows[i][2].ToString() + "  ||  " + "Third Holder:  " + dtEmail.Rows[i][3].ToString();
                row3[1] = "Test";
                dtEx.Rows.Add(row3);
                ViewState["ISINVAlue"] = null;
                VALUE_Sum = 0;
                for (int k = 0; k < dtMail.Rows.Count; k++)
                {
                    if (dtMail.Rows[k]["ISINVAlue"].ToString().Trim() != "" || dtMail.Rows[k]["ISINVAlue"].ToString().Trim().Length > 0)
                    {
                        VALUE_Sum += Convert.ToDecimal(dtMail.Rows[k]["ISINVAlue"].ToString());
                        ViewState["ISINVAlue"] = VALUE_Sum;
                    }
                }
                int colCount = dtMail.Columns.Count;
                foreach (DataRow dr1 in dtMail.Rows)
                {
                    DataRow row2 = dtEx.NewRow();
                    for (int j = 0; j < colCount; j++)
                    {
                        row2[j] = dr1[j];
                    }
                    dtEx.Rows.Add(row2);
                }

                //if (ViewState["ISINVAlue"] != DBNull.Value)
                //{
                if (ViewState["ISINVAlue"] != null)
                {
                    DataRow row9 = dtEx.NewRow();
                    row9[0] = "Total Value of Holding:  " + objConverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(ViewState["ISINVAlue"].ToString()));
                    row9[1] = "Test";
                    dtEx.Rows.Add(row9);
                }
                if (ddlExport.SelectedItem.Value == "PM")
                {
                    DataRow row10 = dtEx.NewRow();
                    row10[0] = "";
                    row10[1] = "Break";
                    dtEx.Rows.Add(row10);
                }
                //}
            }
            //dt1.Clear();
            CurrentPage = 0;
            bindGrid();

            DataTable CompanyName = oDBEngine.GetDataTable("tbl_master_company", "cmp_Name", " cmp_internalId='" + Session["LastCompany"].ToString() + "'");
            DataTable dtReportHeader = new DataTable();
            dtReportHeader.Columns.Add(new DataColumn("Header", typeof(String))); //0
            DataRow HeaderRow = dtReportHeader.NewRow();
            HeaderRow[0] = CompanyName.Rows[0][0].ToString() + "[" + HttpContext.Current.Session["usersegid"] + "]";
            dtReportHeader.Rows.Add(HeaderRow);
            DataRow DrRowR1 = dtReportHeader.NewRow();
            DrRowR1[0] = "CDSL Holding Report For " + oconverter.ArrangeDate2(txtDate.Value.ToString());
            dtReportHeader.Rows.Add(DrRowR1);

            DataTable dtReportFooter = new DataTable();
            dtReportFooter.Columns.Add(new DataColumn("Footer", typeof(String))); //0
            DataRow FooterRow = dtReportFooter.NewRow();
            FooterRow[0] = "* * *  End Of Report * * *   ";
            dtReportFooter.Rows.Add(FooterRow);



            if (ddlExport.SelectedItem.Value == "E")
            {
                objExcel.ExportToExcelforExcel(dtEx, "CDSL Holding Report", "Branch/Group Total", dtReportHeader, dtReportFooter);

            }
            else if (ddlExport.SelectedItem.Value == "P")
            {
                objExcel.ExportToPDF(dtEx, "CDSL Holding Report", "Branch/Group Total", dtReportHeader, dtReportFooter);
            }
            else if (ddlExport.SelectedItem.Value == "PM")
            {
                //DataTable testDT = oDBEngine.GetDataTable("Select Top 4 user_loginId,user_contactId,user_EntryProfile,user_EntryProfile From tbl_master_user Union All Select '','Break','','' Union All Select Top 2 user_loginId,user_contactId,user_EntryProfile,user_EntryProfile From tbl_master_user Union All Select '','Break','','' Union All Select Top 3 user_loginId,user_contactId,user_EntryProfile,user_EntryProfile From tbl_master_user");
                //DataTable testDTHeader = oDBEngine.GetDataTable("Select Top 1 user_name From tbl_master_user");
                //DataTable testDTFooter = oDBEngine.GetDataTable("Select Top 1 user_loginId From tbl_master_user");

                //objExcel.ExportToPDFPageBreak(testDT, "CDSL Holding Report", "Branch/Group Total", testDTHeader, testDTFooter);
                objExcel.ExportToPDFPageBreak(dtEx, "CDSL Holding Report", "Branch/Group Total", dtReportHeader, dtReportFooter);
            }
        }

        protected void cmbTime_Callback(object source, CallbackEventArgsBase e)
        {
            chkStatus = 0;
            if (checkDate() == 1)
            {
                populateCmbTime();
            }
            else
            {
                if (dates.Rows.Count > 0)
                {
                    txtDate.Text = dates.Rows[0][0].ToString();
                    cmbTime.Items.Clear();
                    cmbTime.Items.Add(new ListEditItem("No Data Found", "6/15/2100 0:00:00 AM"));
                    chkStatus = 1;
                }

            }
        }

        private int checkDate()
        {
            int flag = 0;
            try
            {
                string txdt = txtDate.Value.ToString();
                string[] txdtsp = txdt.Split(' ');
                if (txdtsp[0].Substring(0, 1) == "0")
                    txdt = txdt.Substring(1);

                txdt = txdtsp[0];
                for (int i = 0; i < dates.Rows.Count; i++)
                {
                    // txtDate.EditFormatString = OConvert.GetDateFormat("Date");
                    // if (txdt == OConvert.DateConverter(dates.Rows[i]["date"].ToString(), "dd/MM/yyyy"))
                    string[] datet = dates.Rows[i]["date"].ToString().Split(' ');
                    if (txdt == datet[0].ToString())
                        flag = 1;

                }
            }
            catch
            {
            }

            return flag;



        }

        protected void SortInstrumentClick(object sender, EventArgs e)
        {

            bindRepeter();

        }

        protected void SortISINClick(object sender, EventArgs e)
        {

            bindRepeter();

        }

        protected void btnFirst(object sender, EventArgs e)
        {
            CurrentPage = 0;
            bindGrid();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "hi", "height();", true);
        }
        protected void btnPrevious(object sender, EventArgs e)
        {
            CurrentPage = CurrentPage - 1;
            bindGrid();
        }
        protected void btnNext(object sender, EventArgs e)
        {
            CurrentPage = CurrentPage + 1;
            bindGrid();

        }
        protected void btnLast(object sender, EventArgs e)
        {
            CurrentPage = LastPage;
            bindGrid();
        }

        protected void gridHolding_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            if (e.Parameters == "s")
                gridHolding.Settings.ShowFilterRow = true;
            if (e.Parameters == "All")
            {
                gridHolding.FilterExpression = string.Empty;
            }

        }

        protected void gridHolding_CustomJSProperties(object sender, ASPxGridViewClientJSPropertiesEventArgs e)
        {
            e.Properties["cpEND"] = "2";
        }

        protected void ddlExport_SelectedIndexChanged1(object sender, EventArgs e)
        {
            export();
        }

        //--Section or Group select--------------
        protected void btnhide_Click(object sender, EventArgs e)
        {
            if (ddlGroup.SelectedItem.Value.ToString() == "1")
            {
                if (hidGroup.Value == "")
                {
                    BindGroup();
                }
            }
        }

        public void BindGroup()
        {
            ddlgrouptype.Items.Clear();
            DataTable DtGroup = oDBEngine.GetDataTable("tbl_trans_group", "distinct grp_groupType", null);
            if (DtGroup.Rows.Count > 0)
            {
                ddlgrouptype.DataSource = DtGroup;
                ddlgrouptype.DataTextField = "grp_groupType";
                ddlgrouptype.DataValueField = "grp_groupType";
                ddlgrouptype.DataBind();
                ddlgrouptype.Items.Insert(0, new ListItem("Select GroupType", "0"));
                DtGroup.Dispose();

            }
            else
            {
                ddlgrouptype.Items.Insert(0, new ListItem("Select GroupType", "0"));
            }

        }

    }
}