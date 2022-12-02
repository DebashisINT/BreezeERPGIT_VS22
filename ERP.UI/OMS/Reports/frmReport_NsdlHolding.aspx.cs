using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrystalDecisions.CrystalReports.Engine;
using DevExpress.Web;
using BusinessLogicLayer;

namespace ERP.OMS.Reports
{
    public partial class Reports_frmReport_NsdlHolding : System.Web.UI.Page, System.Web.UI.ICallbackEventHandler
    {
        #region Global Variable
        DailyReports dailyrep = new DailyReports();
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);
        ReportDocument AttendenceReportDocu = new ReportDocument();
        private static DataTable dates;
        public int chkStatus;
        PagedDataSource pds = new PagedDataSource();
        string selectedDay = String.Empty;
        string selectedDate = String.Empty;
        static DataTable dt = new DataTable();
        static DataTable dt1 = new DataTable();
        DataView dv = new DataView();
        DataView dv1 = new DataView();
        static string BenType;
        BusinessLogicLayer.Converter oconverter = new BusinessLogicLayer.Converter();
        string data;
        ExcelFile objExcel = new ExcelFile();
        static decimal VALUE_Sum, Free_Sum, Pledged_Sum, Remat_Sum, Demat_Sum, CurrentBalance_Sum;
        BusinessLogicLayer.Converter objConverter = new BusinessLogicLayer.Converter();
        BusinessLogicLayer.Converter OConvert = new BusinessLogicLayer.Converter();
        static string User;
        static string statementDateTime = "";
        static string holdingDateTime = "";

        string[] InputName = new string[7];
        string[] InputType = new string[7];
        string[] InputValue = new string[7];
        #endregion

        #region Page Methods
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
            //this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");
            //________This script is for firing javascript when page load first___//
            if (!ClientScript.IsStartupScriptRegistered("Today"))
                ClientScript.RegisterStartupScript(typeof(Page), "Today", "<script>PageLoad();</script>");
            // ______________________________End Script____________________________//

            if (!IsPostBack)
            {
                populateCmbDate();
                txtISIN.Attributes.Add("onkeyup", "CallAjax(this,'NSDLHoldingISIN',event)");
                txtSettlement.Attributes.Add("onkeyup", "CallAjax(this,'NSDLHoldingSettlementNumber',event)");

                t1.Visible = false;
                t11.Visible = false;

                head.Style["display"] = "none";

                if (dt.Rows.Count > 0)
                {
                    DataRow[] drDv = dt.Select(" UserID = " + Convert.ToString(HttpContext.Current.Session["userid"]));
                    foreach (DataRow d in drDv)
                        dt.Rows.Remove(d);
                }
                if (dt1.Rows.Count > 0)
                {
                    DataRow[] drDv1 = dt1.Select(" UserID1 = " + Convert.ToString(HttpContext.Current.Session["userid"]));
                    foreach (DataRow d in drDv1)
                        dt1.Rows.Remove(d);
                }
            }
            else
            {
                if (dt.Rows.Count > 0)
                    bindRepeater();
            }
            rbClientUser.Attributes.Add("OnClick", "User('User')");
            rbOnlyClient.Attributes.Add("OnClick", "User('NoUser')");
            rbRspctvClient.Attributes.Add("OnClick", "User('NoUser')");

            if (RBReportView.SelectedItem.Value.ToString() == "S")
            {
                gridHolding.Columns[0].Caption = "Client ID";
                gridHolding.Columns[1].Caption = "Client Name";
                gridHolding.Columns["Rate"].Visible = false;
                gridHolding.Columns["Rate_Date"].Visible = false;
            }
            if (RBReportView.SelectedItem.Value.ToString() == "C")
            {
                gridHolding.Columns[0].Caption = "ISIN No.";
                gridHolding.Columns[1].Caption = "CmpName";
                gridHolding.Columns["Rate"].Visible = true;
                gridHolding.Columns["Rate_Date"].Visible = true;

            }
            if (chkCalHoldingValue.Checked == true)
                gridHolding.Columns["Value"].Visible = true;
            else
                gridHolding.Columns["Value"].Visible = false;

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

        protected void gridHolding_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            if (e.Parameters == "s")
                gridHolding.Settings.ShowFilterRow = true;
            if (e.Parameters == "All")
            {
                gridHolding.FilterExpression = string.Empty;
            }
        }

        protected void btnShow_Click(object sender, EventArgs e)
        {
            CurrentPage = 0;
            if (dt.Rows.Count > 0)
            {
                DataRow[] drDv = dt.Select(" UserID = " + Convert.ToString(HttpContext.Current.Session["userid"]));
                foreach (DataRow d in drDv)
                    dt.Rows.Remove(d);
            }
            if (dt1.Rows.Count > 0)
            {
                DataRow[] drDv1 = dt1.Select(" UserID1 = " + Convert.ToString(HttpContext.Current.Session["userid"]));
                foreach (DataRow d in drDv1)
                    dt1.Rows.Remove(d);
            }
            bindGrid();
        }
        #endregion

        #region Populate Client Group
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
        #endregion

        #region User Methods
        protected void populateCmbDate()
        {
            dates = oDBEngine.GetDataTable(" Trans_NsdlHolding ", " distinct CAST(DAY(NsdlHolding_HoldingDateTime) AS VARCHAR(2)) as selDay,CAST(DAY(NsdlHolding_HoldingDateTime) AS VARCHAR(2)) + ' ' + DATENAME(MM, NsdlHolding_HoldingDateTime) + ' ' + CAST(YEAR(NsdlHolding_HoldingDateTime) AS VARCHAR(4))as date, NsdlHolding_HoldingDateTime ", null, "NsdlHolding_HoldingDateTime desc");
            if (dates.Rows.Count > 0)
            {
                tblNoDate.Visible = false;
                selectedDay = Convert.ToString(dates.Rows[0][0]);
                selectedDate = Convert.ToString(dates.Rows[0][1]);

                if (Convert.ToInt32(selectedDay) < 10)
                    txtDate.Text = "0" + selectedDate;
                else
                    txtDate.Text = selectedDate;
            }
            else
            {
                tblNoDate.Visible = true;
            }
        }

        void bindGrid()
        {
            if (dt.Rows.Count > 0)
            {
                dv = dt.DefaultView;
                dv.RowFilter = " UserID = " + Convert.ToString(HttpContext.Current.Session["userid"]);
            }
            if (dv.Count == 0)
            {
                string where = " NsdlClients_BenAccountID = NsdlHolding_BenAccountNumber and LTRIM(RTRIM(NsdlClients_DPID)) = LTRIM(RTRIM(NsdlHolding_DPID)) ";
                where = where + " and NsdlHolding_HoldingDateTime='" + txtDate.Value + "'";

                if (rbUser.SelectedItem.Value.ToString() == "S")
                {
                    where = where + " and NsdlClients_BenAccountID  in (" + hidClients.Value + ")";
                }

                if (ASPxComboBox1.Value.ToString() != "All")
                {
                    if (ASPxComboBox1.Value.ToString() == "other")
                        where = where + " and NsdlClients_BenType NOT IN (01,05,06,04) ";
                    else
                        where = where + " and NsdlClients_BenType='" + ASPxComboBox1.Value + "' ";
                }

                if (txtISIN_hidden.Text.Trim() != "")
                {
                    where = where + " and Trans_NsdlHolding.NsdlHolding_ISIN='" + txtISIN_hidden.Text + "'";
                }

                if (txtSettlement_hidden.Text.Trim() != "")
                {
                    where = where + " and NsdlHolding_SettlementNumber='" + txtSettlement_hidden.Text + "' ";
                }
                string orderBy = " ";
                ////-----Client Wise Or Share wise------------------------------------
                if (RBReportView.SelectedItem.Value.ToString() == "C")
                {
                    orderBy = " NsdlClients_BenAccountID ";
                    if (ddlGroup.SelectedItem.Value.ToString() == "0")
                    {
                        if (rdbranchAll.Checked)
                        {
                            where = where + " and NsdlClients_branchid in (" + HttpContext.Current.Session["userbranchHierarchy"].ToString() + ")";
                            dt = oDBEngine.GetDataTable(" Trans_NsdlHolding,Master_NsdlClients ", "  distinct( NsdlClients_BenAccountID),NsdlClients_BenFirstHolderName,case when len(ltrim(rtrim(NsdlClients_BenSecondHolderName)))=0 then 'N/A' else ltrim(rtrim(NsdlClients_BenSecondHolderName)) end as NsdlClients_BenSecondHolderName,case when len(ltrim(rtrim(NsdlClients_BenThirdHolderName)))=0 then 'N/A' else ltrim(rtrim(NsdlClients_BenThirdHolderName)) end as NsdlClients_BenThirdHolderName,NsdlClients_BenType, " + Convert.ToString(HttpContext.Current.Session["userid"]) + " as UserID ", where, orderBy);
                        }
                        else
                        {
                            where = where + " and NsdlClients_branchid in (" + hidBranch.Value + ")";
                            orderBy = " Master_NsdlClients.NsdlClients_BranchID  ";
                            dt = oDBEngine.GetDataTable(" Trans_NsdlHolding,Master_NsdlClients ", "  distinct( NsdlClients_BenAccountID),NsdlClients_BenFirstHolderName,case when len(ltrim(rtrim(NsdlClients_BenSecondHolderName)))=0 then 'N/A' else ltrim(rtrim(NsdlClients_BenSecondHolderName)) end as NsdlClients_BenSecondHolderName,case when len(ltrim(rtrim(NsdlClients_BenThirdHolderName)))=0 then 'N/A' else ltrim(rtrim(NsdlClients_BenThirdHolderName)) end as NsdlClients_BenThirdHolderName,NsdlClients_BenType,Master_NsdlClients.NsdlClients_BranchID as grp_groupMaster, " + Convert.ToString(HttpContext.Current.Session["userid"]) + " as UserID ", where, orderBy);
                        }
                    }
                    else
                    {
                        if (rdddlgrouptypeAll.Checked)
                        {
                            if (ddlgrouptype.SelectedItem.Value.ToString() != "0")
                            {
                                where = where + "and NsdlClients_DPID +Cast(NsdlClients_BenAccountID as Varchar(10))=grp_contactId and grp_groupType='" + ddlgrouptype.SelectedItem.Text.ToString().Trim() + "' and grp_contactId like 'IN%'";
                                dt = oDBEngine.GetDataTable(" Trans_NsdlHolding,Master_NsdlClients,tbl_trans_group ", "  distinct( NsdlClients_BenAccountID),NsdlClients_BenFirstHolderName,case when len(ltrim(rtrim(NsdlClients_BenSecondHolderName)))=0 then 'N/A' else ltrim(rtrim(NsdlClients_BenSecondHolderName)) end as NsdlClients_BenSecondHolderName,case when len(ltrim(rtrim(NsdlClients_BenThirdHolderName)))=0 then 'N/A' else ltrim(rtrim(NsdlClients_BenThirdHolderName)) end as NsdlClients_BenThirdHolderName,NsdlClients_BenType, " + Convert.ToString(HttpContext.Current.Session["userid"]) + " as UserID ", where, orderBy);
                            }
                            else
                            {
                                dt = oDBEngine.GetDataTable(" Trans_NsdlHolding,Master_NsdlClients", "  distinct( NsdlClients_BenAccountID),NsdlClients_BenFirstHolderName,case when len(ltrim(rtrim(NsdlClients_BenSecondHolderName)))=0 then 'N/A' else ltrim(rtrim(NsdlClients_BenSecondHolderName)) end as NsdlClients_BenSecondHolderName,case when len(ltrim(rtrim(NsdlClients_BenThirdHolderName)))=0 then 'N/A' else ltrim(rtrim(NsdlClients_BenThirdHolderName)) end as NsdlClients_BenThirdHolderName,NsdlClients_BenType, " + Convert.ToString(HttpContext.Current.Session["userid"]) + " as UserID ", where, orderBy);
                            }
                        }
                        else
                        {
                            where = where + "and NsdlClients_DPID +Cast(NsdlClients_BenAccountID as Varchar(10))=grp_contactId and grp_groupMaster in (" + hidGroup.Value + ") and grp_contactId like 'IN%'";
                            orderBy = " tbl_trans_group.grp_groupMaster  ";
                            dt = oDBEngine.GetDataTable(" Trans_NsdlHolding,Master_NsdlClients,tbl_trans_group ", "  distinct( NsdlClients_BenAccountID),NsdlClients_BenFirstHolderName,case when len(ltrim(rtrim(NsdlClients_BenSecondHolderName)))=0 then 'N/A' else ltrim(rtrim(NsdlClients_BenSecondHolderName)) end as NsdlClients_BenSecondHolderName,case when len(ltrim(rtrim(NsdlClients_BenThirdHolderName)))=0 then 'N/A' else ltrim(rtrim(NsdlClients_BenThirdHolderName)) end as NsdlClients_BenThirdHolderName,NsdlClients_BenType,tbl_trans_group.grp_groupMaster," + Convert.ToString(HttpContext.Current.Session["userid"]) + " as UserID ", where, orderBy);
                        }
                    }
                }
                else    //=====Share wise Data fetch=================
                {
                    string shareOrderBy = "ISIN_Name";
                    string shareWhere = " and NSDLISIN_Number=NsdlHolding_ISIN";
                    if (ddlGroup.SelectedItem.Value.ToString() == "0")
                    {
                        if (rdbranchAll.Checked)
                        {
                            shareWhere = where + shareWhere + " and NsdlClients_branchid in (" + HttpContext.Current.Session["userbranchHierarchy"].ToString() + ")";
                        }
                        else
                        {
                            shareWhere = where + shareWhere + " and NsdlClients_branchid in (" + hidBranch.Value + ")";
                        }
                    }
                    else
                    {
                        if (rdddlgrouptypeAll.Checked)
                        {
                            shareWhere = where + shareWhere + " and NsdlClients_DPID +Cast(NsdlClients_BenAccountID as Varchar(10))=grp_contactId and grp_groupType='" + ddlgrouptype.SelectedItem.Text.ToString().Trim() + "' and grp_contactId like 'IN%'";
                        }
                        else
                        {
                            shareWhere = where + shareWhere + " and NsdlClients_DPID +Cast(NsdlClients_BenAccountID as Varchar(10))=grp_contactId and grp_groupMaster in (" + hidGroup.Value + ") and grp_contactId like 'IN%'";
                        }
                    }
                    dt = oDBEngine.GetDataTable(" Trans_NsdlHolding,Master_NsdlClients,Master_NSDLISIN ", "  Distinct NsdlHolding_ISIN as ISIN_ID,ltrim(rtrim(Isnull(NSDLISIN_CompanyName,''))) ISIN_Name,0 NsdlClients_BenType, " + Convert.ToString(HttpContext.Current.Session["userid"]) + " as UserID", shareWhere, shareOrderBy);

                    //===For RETRIEVE BENAccountID for SP=====================
                    DataTable DTShareBenID = new DataTable();
                    orderBy = " NsdlClients_BenAccountID ";
                    if (ddlGroup.SelectedItem.Value.ToString() == "0")
                    {
                        if (rdbranchAll.Checked)
                        {
                            where = where + " and NsdlClients_branchid in (" + HttpContext.Current.Session["userbranchHierarchy"].ToString() + ")";
                            DTShareBenID = oDBEngine.GetDataTable(" Trans_NsdlHolding,Master_NsdlClients ", "  distinct( NsdlClients_BenAccountID),NsdlClients_BenFirstHolderName,case when len(ltrim(rtrim(NsdlClients_BenSecondHolderName)))=0 then 'N/A' else ltrim(rtrim(NsdlClients_BenSecondHolderName)) end as NsdlClients_BenSecondHolderName,case when len(ltrim(rtrim(NsdlClients_BenThirdHolderName)))=0 then 'N/A' else ltrim(rtrim(NsdlClients_BenThirdHolderName)) end as NsdlClients_BenThirdHolderName,0 NsdlClients_BenType, " + Convert.ToString(HttpContext.Current.Session["userid"]) + " as UserID ", where, orderBy);
                        }
                        else
                        {
                            where = where + " and NsdlClients_branchid in (" + hidBranch.Value + ")";
                            orderBy = " Master_NsdlClients.NsdlClients_BranchID  ";
                            DTShareBenID = oDBEngine.GetDataTable(" Trans_NsdlHolding,Master_NsdlClients ", "  distinct( NsdlClients_BenAccountID),NsdlClients_BenFirstHolderName,case when len(ltrim(rtrim(NsdlClients_BenSecondHolderName)))=0 then 'N/A' else ltrim(rtrim(NsdlClients_BenSecondHolderName)) end as NsdlClients_BenSecondHolderName,case when len(ltrim(rtrim(NsdlClients_BenThirdHolderName)))=0 then 'N/A' else ltrim(rtrim(NsdlClients_BenThirdHolderName)) end as NsdlClients_BenThirdHolderName,0 NsdlClients_BenType,Master_NsdlClients.NsdlClients_BranchID as grp_groupMaster, " + Convert.ToString(HttpContext.Current.Session["userid"]) + " as UserID ", where, orderBy);
                        }
                    }
                    else
                    {
                        if (rdddlgrouptypeAll.Checked)
                        {
                            if (ddlgrouptype.SelectedItem.Value.ToString() != "0")
                            {
                                where = where + "and NsdlClients_DPID +Cast(NsdlClients_BenAccountID as Varchar(10))=grp_contactId and grp_groupType='" + ddlgrouptype.SelectedItem.Text.ToString().Trim() + "' and grp_contactId like 'IN%'";
                                DTShareBenID = oDBEngine.GetDataTable(" Trans_NsdlHolding,Master_NsdlClients,tbl_trans_group ", "  distinct( NsdlClients_BenAccountID),NsdlClients_BenFirstHolderName,case when len(ltrim(rtrim(NsdlClients_BenSecondHolderName)))=0 then 'N/A' else ltrim(rtrim(NsdlClients_BenSecondHolderName)) end as NsdlClients_BenSecondHolderName,case when len(ltrim(rtrim(NsdlClients_BenThirdHolderName)))=0 then 'N/A' else ltrim(rtrim(NsdlClients_BenThirdHolderName)) end as NsdlClients_BenThirdHolderName,0 NsdlClients_BenType, " + Convert.ToString(HttpContext.Current.Session["userid"]) + " as UserID ", where, orderBy);
                            }
                            else
                            {
                                DTShareBenID = oDBEngine.GetDataTable(" Trans_NsdlHolding,Master_NsdlClients", "  distinct( NsdlClients_BenAccountID),NsdlClients_BenFirstHolderName,case when len(ltrim(rtrim(NsdlClients_BenSecondHolderName)))=0 then 'N/A' else ltrim(rtrim(NsdlClients_BenSecondHolderName)) end as NsdlClients_BenSecondHolderName,case when len(ltrim(rtrim(NsdlClients_BenThirdHolderName)))=0 then 'N/A' else ltrim(rtrim(NsdlClients_BenThirdHolderName)) end as NsdlClients_BenThirdHolderName,0 NsdlClients_BenType, " + Convert.ToString(HttpContext.Current.Session["userid"]) + " as UserID ", where, orderBy);
                            }
                        }
                        else
                        {
                            where = where + "and NsdlClients_DPID +Cast(NsdlClients_BenAccountID as Varchar(10))=grp_contactId and grp_groupMaster in (" + hidGroup.Value + ") and grp_contactId like 'IN%'";
                            orderBy = " tbl_trans_group.grp_groupMaster  ";
                            DTShareBenID = oDBEngine.GetDataTable(" Trans_NsdlHolding,Master_NsdlClients,tbl_trans_group ", "  distinct( NsdlClients_BenAccountID),NsdlClients_BenFirstHolderName,case when len(ltrim(rtrim(NsdlClients_BenSecondHolderName)))=0 then 'N/A' else ltrim(rtrim(NsdlClients_BenSecondHolderName)) end as NsdlClients_BenSecondHolderName,case when len(ltrim(rtrim(NsdlClients_BenThirdHolderName)))=0 then 'N/A' else ltrim(rtrim(NsdlClients_BenThirdHolderName)) end as NsdlClients_BenThirdHolderName,0 NsdlClients_BenType,tbl_trans_group.grp_groupMaster," + Convert.ToString(HttpContext.Current.Session["userid"]) + " as UserID ", where, orderBy);
                        }
                    }
                    string strSelectedBenAccNumber = "";
                    if (DTShareBenID.Rows.Count > 0)
                    {
                        for (int i = 0; i < DTShareBenID.Rows.Count; i++)
                        {
                            if (i == 0)
                                strSelectedBenAccNumber = DTShareBenID.Rows[i][0].ToString();
                            else
                                strSelectedBenAccNumber = strSelectedBenAccNumber + "," + DTShareBenID.Rows[i][0].ToString();
                        }
                        hidClients.Value = strSelectedBenAccNumber;
                    }
                }//----End share Wise
            }

            dv = dt.DefaultView;
            dv.RowFilter = " UserID = " + Convert.ToString(HttpContext.Current.Session["userid"]);
            pds.DataSource = dv;
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

            Label1.Text = "Total " + pds.DataSourceCount.ToString() + " Record Found.";
            Label11.Text = "Total " + pds.DataSourceCount.ToString() + " Record Found.";

            if (pds.DataSourceCount < 1)
            {
                Label5.Text = "No Record Found.";
                Label5.Visible = true;
                lblBoID.Text = "";
                lblBoName.Text = "";
                lblSecondHolder.Text = "";
                lblThirdHolder.Text = "";

                head.Style["display"] = "none";
            }
            else
            {
                Label5.Visible = false;
                if (RBReportView.SelectedItem.Value.ToString() == "C")
                {
                    lblBoID.Text = "Beneficiary AccountID: " + dt.DefaultView.Table.Rows[pds.CurrentPageIndex][0].ToString();
                    lblBoName.Text = "Beneficiary Name: " + dt.DefaultView.Table.Rows[pds.CurrentPageIndex][1].ToString();
                    lblSecondHolder.Text = "Second Holder: " + dt.DefaultView.Table.Rows[pds.CurrentPageIndex][2].ToString();
                    lblThirdHolder.Text = "Third Holder: " + dt.DefaultView.Table.Rows[pds.CurrentPageIndex][3].ToString();
                }
                if (RBReportView.SelectedItem.Value.ToString() == "S")
                {
                    lblBoID.Text = "ISIN: " + dt.DefaultView.Table.Rows[pds.CurrentPageIndex][0].ToString();
                    lblBoName.Text = "ISIN Name: " + dt.DefaultView.Table.Rows[pds.CurrentPageIndex][1].ToString();
                    lblSecondHolder.Text = "";
                    lblThirdHolder.Text = " ";
                }

                BenType = dt.DefaultView.Table.Rows[pds.CurrentPageIndex]["NsdlClients_BenType"].ToString();
                head.Style["display"] = "display";
            }

            bindRepeater();

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

        protected void bindRepeater()
        {
            try
            {
                if (dt.Rows.Count > 0)
                {
                    dv = dt.DefaultView;
                    dv.RowFilter = " UserID = " + Convert.ToString(HttpContext.Current.Session["userid"]);
                }
                if (dt1.Rows.Count > 0)
                {
                    dv1 = dt1.DefaultView;
                    dv1.RowFilter = " UserID1 = " + Convert.ToString(HttpContext.Current.Session["userid"]);
                }
                if (dv.Count > 0 && ((dv1 == null) || (dv1.Count == 0)))
                {
                    string where_NsdlHolding_HoldingDateTime;
                    string id = "";

                    string strISIN, strSettlementNumber;
                    if (RBReportView.SelectedItem.Value.ToString() == "C")
                    {
                        id = lblBoID.Text.ToString();
                        id = id.Replace("Beneficiary AccountID: ", "");
                    }
                    if (RBReportView.SelectedItem.Value.ToString() == "S")
                    {
                        //===Get BenAccountNumber list from Branch/Group/Client wise
                        if (hidClients.Value != null)
                            id = hidClients.Value.ToString();
                    }

                    where_NsdlHolding_HoldingDateTime = txtDate.Value.ToString();

                    if (txtISIN_hidden.Text.Trim() != "")
                    {
                        strISIN = txtISIN_hidden.Text;
                    }
                    else
                        strISIN = "na";

                    if (txtSettlement_hidden.Text.Trim() != "")
                    {
                        strSettlementNumber = txtSettlement_hidden.Text;
                    }
                    else
                        strSettlementNumber = "na";

                    //=========For Client Wise/Share Wise=====================
                    string reportVW = "";
                    if (RBReportView.SelectedItem.Value.ToString() == "C")
                        reportVW = "~C";

                    string Selected_ISINID = "";
                    if (RBReportView.SelectedItem.Value.ToString() == "S")
                    {
                        reportVW = "~S";
                        Selected_ISINID = lblBoID.Text.ToString();
                        Selected_ISINID = Selected_ISINID.Replace("ISIN: ", "");
                    }

                    ClearArray();

                    InputName[0] = "BenAccId";
                    InputName[1] = "where_NsdlHolding_HoldingDateTime";
                    InputName[2] = "BenType";
                    InputName[3] = "isin";
                    InputName[4] = "Settlement_Id";
                    InputName[5] = "branhid";
                    InputName[6] = "user";

                    InputType[0] = "V";
                    InputType[1] = "V";
                    InputType[2] = "V";
                    InputType[3] = "V";
                    InputType[4] = "V";
                    InputType[5] = "V";
                    InputType[6] = "V";

                    InputValue[0] = id;
                    InputValue[1] = where_NsdlHolding_HoldingDateTime.Substring(0, where_NsdlHolding_HoldingDateTime.Length - 11);
                    InputValue[2] = BenType;
                    InputValue[3] = strISIN;
                    InputValue[4] = strSettlementNumber;
                    InputValue[5] = HttpContext.Current.Session["userbranchHierarchy"].ToString();
                    InputValue[6] = Convert.ToString(HttpContext.Current.Session["userid"]) + reportVW + "~" + Selected_ISINID;

                    dt1 = SQLProcedures.SelectProcedureArr("sp_ShowNsdlHolding", InputName, InputType, InputValue);
                }
                if ((ASPxComboBox1.Value.ToString() == "06") || (ASPxComboBox1.Value.ToString() == "All"))
                {
                    gridHolding.Columns[2].Visible = true;
                    gridHolding.Columns[3].Visible = true;
                }
                else
                {
                    gridHolding.Columns[2].Visible = false;
                    gridHolding.Columns[3].Visible = false;
                }

                if (RBReportView.SelectedItem.Value.ToString() == "S")
                {
                    lblSecondHolder.Text = "Rate: " + dt1.Rows[0]["Rate"].ToString();
                    lblThirdHolder.Text = "Rate Date: " + dt1.Rows[0]["Rate_Date"].ToString();
                }

                gridHolding.FilterEnabled = false;

                dv1 = dt1.DefaultView;
                dv1.RowFilter = " UserID1 = " + Convert.ToString(HttpContext.Current.Session["userid"]);
                gridHolding.DataSource = dv1;

                gridHolding.DataBind();
                gridHolding.FilterExpression = string.Empty;
            }
            catch
            {
            }
        }

        protected void ClearArray()
        {
            Array.Clear(InputName, 0, InputName.Length - 1);
            Array.Clear(InputType, 0, InputType.Length - 1);
            Array.Clear(InputValue, 0, InputValue.Length - 1);
        }
        #endregion

        #region Screen
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

        protected void btnFirst(object sender, EventArgs e)
        {
            CurrentPage = 0;
            if (dt1.Rows.Count > 0)
                dt1.Clear();

            bindGrid();
            gridHolding.PageIndex = 0;
        }
        protected void btnPrevious(object sender, EventArgs e)
        {
            CurrentPage = CurrentPage - 1;
            if (dt1.Rows.Count > 0)
                dt1.Clear();

            bindGrid();
            gridHolding.PageIndex = 0;
        }
        protected void btnNext(object sender, EventArgs e)
        {
            CurrentPage = CurrentPage + 1;
            dv1 = dt1.DefaultView;
            dv1.RowFilter = " UserID1 = " + Convert.ToString(HttpContext.Current.Session["userid"]);

            if (dv1.Count > 0)
            {
                DataRow[] drDv1 = dt1.Select(" UserID1 = " + Convert.ToString(HttpContext.Current.Session["userid"]));
                foreach (DataRow d in drDv1)
                    dt1.Rows.Remove(d);
            }
            bindGrid();
            gridHolding.PageIndex = 0;
        }
        protected void btnLast(object sender, EventArgs e)
        {
            CurrentPage = LastPage;
            if (dt1.Rows.Count > 0)
                dt1.Clear();
            bindGrid();
            gridHolding.PageIndex = 0;
        }
        #endregion

        #region Export
        protected void ddlExport_SelectedIndexChanged1(object sender, EventArgs e)
        {
            export();
        }

        void export()
        {
            DataTable dtEx = new DataTable();
            DataTable dtTemp = new DataTable();
            if (RBReportView.SelectedItem.Value.ToString() == "C")
                dtEx.Columns.Add("ISIN No.");
            else
                dtEx.Columns.Add("Client ID");
            if (RBReportView.SelectedItem.Value.ToString() == "C")
                dtEx.Columns.Add("ISIN Name");
            else
                dtEx.Columns.Add("Client Name");
            dtEx.Columns.Add("Settlement ID");
            dtEx.Columns.Add("Type");
            dtEx.Columns.Add("Free");
            dtEx.Columns.Add("Pending Demat");
            dtEx.Columns.Add("Pending Remat");
            dtEx.Columns.Add("Pledged");
            dtEx.Columns.Add("Current Balance");
            dtEx.Columns.Add("Rate");
            if (RBReportView.SelectedItem.Value.ToString() == "C")
                dtEx.Columns.Add("Rate Date");
            dtEx.Columns.Add("Value");

            DataTable dtFilter = dv.ToTable();
            for (int i = 0; i < dtFilter.Rows.Count; i++)
            {
                DataRow[] drs = dt1.Select(" UserID1 = " + Convert.ToString(HttpContext.Current.Session["userid"]));
                foreach (DataRow d in drs)
                    dt1.Rows.Remove(d);

                CurrentPage = i;
                bindGrid();

                DataRow row3 = dtEx.NewRow();
                if (RBReportView.SelectedItem.Value.ToString() == "C")
                {
                    row3[0] = "Beneficiary AccountID: " + dtFilter.Rows[i][0].ToString() + "   ||  " + "Beneficiary Name:  " + dtFilter.Rows[i][1].ToString() + "  ||  " + "Second Holder:  " + dtFilter.Rows[i][2].ToString() + "  ||  " + "Third Holder:  " + dtFilter.Rows[i][3].ToString();
                }
                if (RBReportView.SelectedItem.Value.ToString() == "S")
                {
                    row3[0] = "ISIN: " + dtFilter.Rows[i][0].ToString() + "   ||  " + "ISIN Name:  " + dtFilter.Rows[i][1].ToString();
                }

                DataTable dt1Filter = dv1.ToTable();
                if (RBReportView.SelectedItem.Value.ToString() == "S")
                {
                    row3[0] = row3[0] + "  ||  " + "Rate:  " + dt1Filter.Rows[0]["Rate"].ToString() + "  ||  " + "Rate Date:  " + dt1Filter.Rows[0]["Rate_Date"].ToString();
                }
                row3[1] = "Test";
                dtEx.Rows.Add(row3);

                //======Other Fields Sum Calculation If Shared Wise====Free_Sum,Pledged_Sum,Remat_Sum,Demat_Sum,CurrentBalance_Sum
                if (RBReportView.SelectedItem.Value.ToString() == "S")
                {
                    ViewState["Free_Sum"] = null;
                    ViewState["Pledged_Sum"] = null;
                    ViewState["Remat_Sum"] = null;
                    ViewState["Demat_Sum"] = null;
                    ViewState["CurrentBalance_Sum"] = null;
                    Free_Sum = 0;
                    Pledged_Sum = 0;
                    Remat_Sum = 0;
                    Demat_Sum = 0;
                    CurrentBalance_Sum = 0;
                    for (int k = 0; k < dt1Filter.Rows.Count; k++)
                    {
                        Free_Sum += Convert.ToDecimal(dt1Filter.Rows[k]["Free"].ToString());
                        Pledged_Sum += Convert.ToDecimal(dt1Filter.Rows[k]["Pledged"].ToString());
                        Remat_Sum += Convert.ToDecimal(dt1Filter.Rows[k]["Remat"].ToString());
                        Demat_Sum += Convert.ToDecimal(dt1Filter.Rows[k]["Demat"].ToString());
                        CurrentBalance_Sum += Convert.ToDecimal(dt1Filter.Rows[k]["Total"].ToString());

                        ViewState["Free_Sum"] = Free_Sum;
                        ViewState["Pledged_Sum"] = Pledged_Sum;
                        ViewState["Remat_Sum"] = Remat_Sum;
                        ViewState["Demat_Sum"] = Demat_Sum;
                        ViewState["CurrentBalance_Sum"] = CurrentBalance_Sum;
                    }
                }
                //------If Calculated Holding Value Checked To Show
                if (chkCalHoldingValue.Checked == true)
                {
                    ViewState["VALUE_Sum"] = null;
                    VALUE_Sum = 0;
                    for (int k = 0; k < dt1Filter.Rows.Count; k++)
                    {
                        if (dt1Filter.Rows[k]["ISINVAlue"].ToString().Trim() != "" || dt1Filter.Rows[k]["ISINVAlue"].ToString().Trim().Length > 0)
                        {
                            VALUE_Sum += Convert.ToDecimal(dt1Filter.Rows[k]["ISINVAlue"].ToString());
                            ViewState["ISINVAlue"] = VALUE_Sum;
                        }
                    }
                }

                int colCount = 0;
                if (RBReportView.SelectedItem.Value.ToString() == "S")
                {
                    //dt1Filter.Columns.Remove("Rate_Date");
                    //dt1Filter.AcceptChanges();
                    if (chkCalHoldingValue.Checked == true)
                        colCount = dt1Filter.Columns.Count - 2;
                    else
                        colCount = dt1Filter.Columns.Count - 3;
                }
                if (RBReportView.SelectedItem.Value.ToString() == "C")
                {
                    if (chkCalHoldingValue.Checked == true)
                        colCount = dt1Filter.Columns.Count - 1;
                    else
                        colCount = dt1Filter.Columns.Count - 2;

                }
                foreach (DataRow dr1 in dt1Filter.Rows)
                {
                    DataRow row2 = dtEx.NewRow();
                    for (int j = 0; j < colCount; j++)
                    {
                        row2[j] = dr1[j];
                    }
                    dtEx.Rows.Add(row2);
                }

                if (RBReportView.SelectedItem.Value.ToString() == "S")
                {
                    DataRow row9 = dtEx.NewRow();
                    row9[0] = "Sum(CurrentBalance):" + ViewState["CurrentBalance_Sum"].ToString() + " | Sum(Free):" + ViewState["Free_Sum"].ToString() + " | Sum(Pledged):" + ViewState["Pledged_Sum"].ToString() + " | Sum(Remat):" + ViewState["Remat_Sum"].ToString() + " | Sum(Demat):" + ViewState["Demat_Sum"].ToString() + " | ";
                    if (chkCalHoldingValue.Checked == true)
                        row9[0] = row9[0] + "Total Value of Holding:" + objConverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(ViewState["ISINVAlue"].ToString()));
                    row9[1] = "Test";
                    dtEx.Rows.Add(row9);
                }
                if (RBReportView.SelectedItem.Value.ToString() == "C")
                {
                    if (chkCalHoldingValue.Checked == true)
                    {
                        DataRow row9 = dtEx.NewRow();
                        row9[0] = "Total Value of Holding:" + objConverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(ViewState["ISINVAlue"].ToString()));
                        row9[1] = "";
                        dtEx.Rows.Add(row9);
                    }
                }

                if (ddlExport.SelectedItem.Value == "PM")
                {
                    DataRow row10 = dtEx.NewRow();
                    row10[0] = "";
                    row10[1] = "Break";
                    dtEx.Rows.Add(row10);
                }
            }
            CurrentPage = 0;
            bindGrid();

            DataTable CompanyName = oDBEngine.GetDataTable("tbl_master_company", "cmp_Name", " cmp_internalId='" + Session["LastCompany"].ToString() + "'");
            DataTable dtReportHeader = new DataTable();
            dtReportHeader.Columns.Add(new DataColumn("Header", typeof(String))); //0
            DataRow HeaderRow = dtReportHeader.NewRow();
            HeaderRow[0] = CompanyName.Rows[0][0].ToString() + "[" + HttpContext.Current.Session["usersegid"] + "]";
            dtReportHeader.Rows.Add(HeaderRow);
            DataRow DrRowR1 = dtReportHeader.NewRow();

            statementDateTime = oDBEngine.GetDataTable("SELECT Top 1 convert(varchar, cast(NsdlHolding_StatementDateTime as datetime), 106)+SUBSTRING( convert(varchar, cast(NsdlHolding_StatementDateTime as datetime), 100),13,Len(convert(varchar, cast(NsdlHolding_StatementDateTime as datetime), 100))) from Trans_NsdlHolding where NsdlHolding_HoldingDateTime='" + txtDate.Value.ToString() + "'").Rows[0][0].ToString();

            DrRowR1[0] = "NSDL Holding Report For " + oconverter.ArrangeDate2(txtDate.Value.ToString()) + " (As At " + statementDateTime + ")";
            dtReportHeader.Rows.Add(DrRowR1);

            DataTable dtReportFooter = new DataTable();
            dtReportFooter.Columns.Add(new DataColumn("Footer", typeof(String))); //0
            DataRow FooterRow = dtReportFooter.NewRow();
            FooterRow[0] = "* * *  End Of Report * * *   ";
            dtReportFooter.Rows.Add(FooterRow);

            if (chkCalHoldingValue.Checked == false)
            {
                dtEx.Columns.Remove("Value");
            }
            dtEx.AcceptChanges();
            if (hiddenIsinMail.Value == "T")
            {
                PDFHoldingDetail(dtEx, "ISINWiseMailToUser");
            }
            else
            {
                if (ddlExport.SelectedItem.Value == "E")
                {
                    objExcel.ExportToExcelforExcel(dtEx, "NSDL Holding Report", "Branch/Group Total", dtReportHeader, dtReportFooter);
                }
                else if (ddlExport.SelectedItem.Value == "P")
                {
                    PDFHoldingDetail(dtEx, "PDF");
                }
                else if (ddlExport.SelectedItem.Value == "PM")
                {
                    objExcel.ExportToPDFPageBreak(dtEx, "NSDL Holding Report", "Branch/Group Total", dtReportHeader, dtReportFooter);
                }
            }
        }
        #endregion

        #region Email
        protected void btnMailSend_Click(object sender, EventArgs e)
        {
            if (RBReportView.SelectedItem.Value.ToString() == "C")
            {
                string disptbl = "";
                string emlTbl = "";
                string mailid = string.Empty;
                string dpid = string.Empty;
                string emlcnt = string.Empty;
                string emailbdy = "";
                string branchContact = "";
                string billdate = oconverter.ArrangeDate2(txtDate.Value.ToString());

                statementDateTime = oDBEngine.GetDataTable("SELECT Top 1 convert(varchar, cast(NsdlHolding_StatementDateTime as datetime), 106)+SUBSTRING( convert(varchar, cast(NsdlHolding_StatementDateTime as datetime), 100),13,Len(convert(varchar, cast(NsdlHolding_StatementDateTime as datetime), 100))) from Trans_NsdlHolding where NsdlHolding_HoldingDateTime='" + txtDate.Value.ToString() + "'").Rows[0][0].ToString();
                string Subject = "NSDL Holding Report for" + billdate + " (As At " + statementDateTime + ")";

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt1.Clear();
                    CurrentPage = i;
                    bindGrid();
                    disptbl = "<table width=\"1050px\" style=\"font-size:8pt;\" cellspacing=\"0\" cellpadding=\"0\" border=\"1\">";
                    disptbl += "<tr><td colspan=\"10\"><table width=\"1050px\" style=\"font-size:10pt;\" cellspacing=\"0\" cellpadding=\"0\"><tr><td align=\"left\"><b>Beneficiary AccountID:" + dt.Rows[i][0].ToString() + "</b></td><td align=\"left\"> <b>Beneficiary Name:" + dt.Rows[i][1].ToString() + "</b></td><td align=\"left\"><b>Second Holder:" + dt.Rows[i][2].ToString() + "</b></td><td align=\"left\"><b>Third Holder:" + dt.Rows[i][3].ToString() + "</b></td></tr></table></td></tr>";
                    disptbl += "<tr style=\"background-color#DBEEF3;\"><td align=\"center\"><b>ISIN No.</b></td><td align=\"center\"><b>ISIN Name</b></td><td align=\"center\"><b>SettlementID</b></td><td align=\"center\"><b>Current </br> Balance</b></td><td align=\"center\"><b>Free</b></td><td align=\"center\"><b>Pledged</b></td><td align=\"center\"><b>Pending </br> Remat</b></td><td align=\"center\"><b>Pending </br> Demat</b></td><td align=\"center\"><b>Rate</b></td><td align=\"center\"><b>Value</b></td></tr>";

                    ViewState["ISINVAlue"] = null;
                    VALUE_Sum = 0;
                    for (int j = 0; j < dt1.Rows.Count; j++)
                    {
                        disptbl += "<tr><td>" + dt1.Rows[j]["NsdlHolding_ISIN"] + "</td><td>" + dt1.Rows[j]["CmpName"] + "</td><td>&nbsp;" + dt1.Rows[j]["NsdlHolding_SettlementNumber"] + "</td><td align=\"right\">&nbsp;" + dt1.Rows[j]["Total"] + "</td><td align=\"right\">&nbsp;" + dt1.Rows[j]["Free"] + "</td><td align=\"right\">&nbsp;" + dt1.Rows[j]["Pledged"] + "</td><td align=\"right\">&nbsp;" + dt1.Rows[j]["Remat"] + "</td><td align=\"right\">&nbsp;" + dt1.Rows[j]["Demat"] + "</td><td align=\"right\">&nbsp;" + dt1.Rows[j]["Rate"] + "</td><td align=\"right\">&nbsp;" + dt1.Rows[j]["ISINValue"] + "</td></tr>";
                        if (dt1.Rows[j]["ISINVAlue"].ToString().Trim() != "" || dt1.Rows[j]["ISINVAlue"].ToString().Trim().Length > 0)
                        {
                            VALUE_Sum += Convert.ToDecimal(dt1.Rows[j]["ISINVAlue"].ToString());
                            ViewState["ISINVAlue"] = VALUE_Sum;
                        }
                    }
                    disptbl += "<tr><td>Total Holding Value: </td><td>&nbsp;</td><td>&nbsp;</td><td align=\"right\">&nbsp;</td><td align=\"right\">&nbsp;</td><td align=\"right\">&nbsp;</td><td align=\"right\">&nbsp;</td><td align=\"right\">&nbsp;</td><td align=\"right\">&nbsp;</td><td align=\"right\">&nbsp;" + objConverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(ViewState["ISINVAlue"].ToString())) + "</td></tr>";
                    disptbl += "</table>";
                    if (rbRspctvClient.Checked)
                    {
                        DataTable dt123 = oDBEngine.GetDataTable("SELECT Top 1 '['+ltrim(rtrim(isnull(NsdlHolding_DPID,'')))+']' , convert(varchar, cast(NsdlHolding_StatementDateTime as datetime), 106)+SUBSTRING( convert(varchar, cast(NsdlHolding_StatementDateTime as datetime), 100),13,Len(convert(varchar, cast(NsdlHolding_StatementDateTime as datetime), 100))) from Trans_NsdlHolding where NsdlHolding_HoldingDateTime='" + txtDate.Value.ToString() + "'");
                        string emailDpID = dt123.Rows[0][0].ToString();
                        statementDateTime = dt123.Rows[0][1].ToString();

                        Subject = "NSDL Holding Report for" + billdate + "[" + dt.Rows[i]["NsdlClients_BenAccountId"].ToString().Trim() + "] " + emailDpID + " (As At " + statementDateTime + ")";
                        DataTable dp = oDBEngine.GetDataTable(" master_nsdlclients  ", "  top 1  *  ", "NsdlClients_BenAccountID='" + dt.Rows[i]["NsdlClients_BenAccountId"] + "'");
                        dpid = dp.Rows[0]["NsdlClients_DpID"].ToString().Trim();
                        emlcnt = dp.Rows[0]["NsdlClients_DpID"].ToString().Trim() + "" + dt.Rows[i]["NsdlClients_BenAccountId"].ToString().Trim();
                        DataTable dtCnt = oDBEngine.GetDataTable(" tbl_master_email  ", "    *  ", "eml_cntId='" + emlcnt + "'");
                        emailbdy = disptbl;
                        branchContact = HttpContext.Current.Session["usersegid"].ToString().Trim() + "" + dt.Rows[i]["NsdlClients_BenAccountId"].ToString().Trim();

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
                                    if (dt.Rows.Count <= 1)
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
                        disptbl = "";
                    }
                    else if (rbOnlyClient.Checked)
                    {
                        if (rdddlgrouptypeSelected.Checked)
                        {
                            if (i < dt.Rows.Count - 1)
                            {
                                if (dt.Rows[i]["grp_groupMaster"].ToString() == dt.Rows[i + 1]["grp_groupMaster"].ToString())
                                {
                                    emlTbl += "<table width=\"1050px\" style=\"font-size:8pt;\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\"><tr><td> " + disptbl + "</td></tr></table>";
                                    disptbl = "";
                                }
                                else
                                {
                                    emlTbl += "<table width=\"1050px\" style=\"font-size:8pt;\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\"><tr><td> " + disptbl + "</td></tr></table>";
                                    DataTable dp = oDBEngine.GetDataTable(" master_nsdlclients  ", "  top 1  *  ", "NsdlClients_BenAccountID='" + dt.Rows[i]["NsdlClients_BenAccountId"] + "'");
                                    dpid = dp.Rows[0]["NsdlClients_DpID"].ToString().Trim();
                                    emlcnt = dp.Rows[0]["NsdlClients_DpID"].ToString().Trim() + "" + dt.Rows[i]["NsdlClients_BenAccountId"].ToString().Trim();
                                    DataTable dtCnt = oDBEngine.GetDataTable(" tbl_master_groupmaster  ", "    *  ", "gpm_id='" + dt.Rows[i]["grp_groupMaster"].ToString() + "'");
                                    emailbdy = emlTbl;
                                    branchContact = dt.Rows[i]["grp_groupMaster"].ToString();
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
                                                if (dt.Rows.Count <= 1)
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
                                }
                            }
                            else if (i == dt.Rows.Count - 1)
                            {
                                if (dt.Rows[i]["grp_groupMaster"].ToString() == dt.Rows[i - 1]["grp_groupMaster"].ToString())
                                {
                                    emlTbl += "<table width=\"1050px\" style=\"font-size:8pt;\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\"><tr><td> " + disptbl + "</td></tr></table>";
                                    disptbl = "";
                                    DataTable dp = oDBEngine.GetDataTable(" master_nsdlclients  ", "  top 1  *  ", "NsdlClients_BenAccountID='" + dt.Rows[i]["NsdlClients_BenAccountId"] + "'");
                                    dpid = dp.Rows[0]["NsdlClients_DpID"].ToString().Trim();
                                    emlcnt = dp.Rows[0]["NsdlClients_DpID"].ToString().Trim() + "" + dt.Rows[i]["NsdlClients_BenAccountId"].ToString().Trim();
                                    DataTable dtCnt = oDBEngine.GetDataTable(" tbl_master_groupmaster  ", "    *  ", "gpm_id='" + dt.Rows[i]["grp_groupMaster"].ToString() + "'");
                                    emailbdy = emlTbl;
                                    branchContact = dt.Rows[i]["grp_groupMaster"].ToString();
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
                                                this.Page.ClientScript.RegisterStartupScript(GetType(), "jscript78", "<script>ForFilterOff();</script>");
                                                this.Page.ClientScript.RegisterStartupScript(GetType(), "jscript6", "<script>MailsendT();</script>");
                                            }
                                            else
                                            {
                                                if (dt.Rows.Count <= 1)
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
                                    DataTable dp = oDBEngine.GetDataTable(" master_nsdlclients  ", "  top 1  *  ", "NsdlClients_BenAccountID='" + dt.Rows[i]["NsdlClients_BenAccountId"] + "'");
                                    dpid = dp.Rows[0]["NsdlClients_DpID"].ToString().Trim();
                                    emlcnt = dp.Rows[0]["NsdlClients_DpID"].ToString().Trim() + "" + dt.Rows[i]["NsdlClients_BenAccountId"].ToString().Trim();
                                    DataTable dtCnt = oDBEngine.GetDataTable(" tbl_master_groupmaster  ", "    *  ", "gpm_id='" + dt.Rows[i]["grp_groupMaster"].ToString() + "'");
                                    emailbdy = emlTbl;
                                    branchContact = dt.Rows[i]["grp_groupMaster"].ToString();
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
                                                this.Page.ClientScript.RegisterStartupScript(GetType(), "jscript78", "<script>ForFilterOff();</script>");
                                                this.Page.ClientScript.RegisterStartupScript(GetType(), "jscript6", "<script>MailsendT();</script>");
                                            }
                                            else
                                            {
                                                if (dt.Rows.Count <= 1)
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
                                    disptbl = "";
                                    emlTbl = "";
                                }
                            }
                        }
                        else if (rdbranchSelected.Checked)
                        {
                            if (i < dt.Rows.Count - 1)
                            {
                                if (dt.Rows[i]["grp_groupMaster"].ToString() == dt.Rows[i + 1]["grp_groupMaster"].ToString())
                                {
                                    emlTbl += "<table width=\"1050px\" style=\"font-size:8pt;\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\"><tr><td> " + disptbl + "</td></tr></table>";
                                    disptbl = "";
                                }
                                else
                                {
                                    emlTbl += "<table width=\"1050px\" style=\"font-size:8pt;\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\"><tr><td> " + disptbl + "</td></tr></table>";
                                    DataTable dp = oDBEngine.GetDataTable(" master_nsdlclients  ", "  top 1  *  ", "NsdlClients_BenAccountID='" + dt.Rows[i]["NsdlClients_BenAccountId"] + "'");
                                    dpid = dp.Rows[0]["NsdlClients_DpID"].ToString().Trim();
                                    emlcnt = dp.Rows[0]["NsdlClients_DpID"].ToString().Trim() + "" + dt.Rows[i]["NsdlClients_BenAccountId"].ToString().Trim();
                                    DataTable dtCnt = oDBEngine.GetDataTable(" TBL_MASTER_BRANCH  ", "  top 1 *  ", "branch_id='" + dt.Rows[i]["grp_groupMaster"].ToString() + "'");
                                    emailbdy = emlTbl;
                                    branchContact = dtCnt.Rows[0]["branch_head"].ToString();
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
                                                if (dt.Rows.Count <= 1)
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
                                }
                            }
                            else if (i == dt.Rows.Count - 1)
                            {
                                if (dt.Rows[i]["grp_groupMaster"].ToString() == dt.Rows[i - 1]["grp_groupMaster"].ToString())
                                {
                                    emlTbl += "<table width=\"1050px\" style=\"font-size:8pt;\" cellspacing=\"0\" cellpadding=\"0\" border=\"1\"><tr><td> " + disptbl + "</td></tr></table>";
                                    DataTable dp = oDBEngine.GetDataTable(" master_nsdlclients  ", "  top 1  *  ", "NsdlClients_BenAccountID='" + dt.Rows[i]["NsdlClients_BenAccountId"] + "'");
                                    dpid = dp.Rows[0]["NsdlClients_DpID"].ToString().Trim();
                                    emlcnt = dp.Rows[0]["NsdlClients_DpID"].ToString().Trim() + "" + dt.Rows[i]["NsdlClients_BenAccountId"].ToString().Trim();
                                    DataTable dtCnt = oDBEngine.GetDataTable(" TBL_MASTER_BRANCH  ", "  top 1 *  ", "branch_id='" + dt.Rows[i]["grp_groupMaster"].ToString() + "'");
                                    emailbdy = emlTbl;
                                    branchContact = dtCnt.Rows[0]["branch_head"].ToString();
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
                                                if (dt.Rows.Count <= 1)
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
                                    disptbl = "";
                                    emlTbl = "";
                                }
                                else
                                {
                                    emlTbl += "<table width=\"1050px\" style=\"font-size:8pt;\" cellspacing=\"0\" cellpadding=\"0\" border=\"1\"><tr><td> " + disptbl + "</td></tr></table>";
                                    DataTable dp = oDBEngine.GetDataTable(" master_nsdlclients  ", "  top 1  *  ", "NsdlClients_BenAccountID='" + dt.Rows[i]["NsdlClients_BenAccountId"] + "'");
                                    dpid = dp.Rows[0]["NsdlClients_DpID"].ToString().Trim();
                                    emlcnt = dp.Rows[0]["NsdlClients_DpID"].ToString().Trim() + "" + dt.Rows[i]["NsdlClients_BenAccountId"].ToString().Trim();
                                    DataTable dtCnt = oDBEngine.GetDataTable(" TBL_MASTER_BRANCH  ", "  top 1 *  ", "branch_id='" + dt.Rows[i]["grp_groupMaster"].ToString() + "'");
                                    emailbdy = emlTbl;
                                    branchContact = dtCnt.Rows[0]["branch_head"].ToString();
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
                                                if (dt.Rows.Count <= 1)
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
                                    disptbl = "";
                                    emlTbl = "";
                                }
                            }
                        }
                    }
                    else if (rbClientUser.Checked)
                    {
                        if (i == dt.Rows.Count - 1)
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
            else
            {
                hiddenIsinMail.Value = "T";
                export();
            }
        }
        #endregion

        #region PDF
        protected void PDFHoldingDetail(DataTable dtClientHoldingDetail, string CallFor)
        {
            string[] connPath = ConfigurationManager.AppSettings["DBConnectionDefault"].Split(';');
            string path = "";
            if (RBReportView.SelectedItem.Value.ToString() == "C")
                path = HttpContext.Current.Server.MapPath("..\\Reports\\NSDL_ClientHoldingDetail.rpt");
            else
                path = HttpContext.Current.Server.MapPath("..\\Reports\\NSDL_ClientHoldingShareDetail.rpt");

            if (dtClientHoldingDetail.Rows.Count > 0)
            {
                #region PDF Content Detail
                if (chkCalHoldingValue.Checked == false)// === Blank Insert for getting Value Field Empty in RPT Files
                    dtClientHoldingDetail.Columns.Add("Value");

                statementDateTime = oDBEngine.GetDataTable("SELECT Top 1 convert(varchar, cast(NsdlHolding_StatementDateTime as datetime), 106)+SUBSTRING( convert(varchar, cast(NsdlHolding_StatementDateTime as datetime), 100),13,Len(convert(varchar, cast(NsdlHolding_StatementDateTime as datetime), 100))) from Trans_NsdlHolding where NsdlHolding_HoldingDateTime='" + txtDate.Value.ToString() + "'").Rows[0][0].ToString();
                DataTable dtCompany = oDBEngine.GetDataTable(@"Declare @cmpname varchar(500),@cmpaddress varchar(max),@cmpPhone varchar(500),@cmpStaxRegNo varchar(50),@cmpPan varchar(20),@cmpHoldingStatementAsAt varchar(200) 
                                                            Select @cmpname=isnull(cmp_Name,'') +' [ ' +isnull(exch_TMCode,'')+' ]',
	                                                               @cmpaddress=isnull(add_address1,'')+' '+isnull(add_address2,'')+' '+isnull(add_address3,'')+', '+isnull(city_name,'')+'-'+ isnull(add_pin,''),
	                                                               @cmpStaxRegNo=isnull(cmp_serviceTaxNo,''),
                                                                   @cmpPan=isnull(cmp_panNo,''),
                                                                   @cmpHoldingStatementAsAt='NSDL Holding Report For " + oconverter.ArrangeDate2(txtDate.Value.ToString()) + " (As At " + statementDateTime + ")',@cmpPhone=isnull((Select phf_countryCode+'-'+phf_areaCode+'-'+phf_phoneNumber from tbl_master_phonefax where phf_cntId='" + Session["LastCompany"].ToString() + "' and phf_type='Office'),'') From  tbl_master_address,tbl_master_company,tbl_master_companyExchange,tbl_master_city Where exch_compId=cmp_internalid and cmp_internalid=add_cntId and add_city=city_id and exch_compId='" + Session["LastCompany"].ToString() + "' and exch_membershipType='NSDL' Select @cmpname cmp_Name,@cmpaddress cmp_Address,@cmpPan cmp_Pan,@cmpPhone cmp_Phone,@cmpStaxRegNo cmp_ServTaxRegNo,@cmpHoldingStatementAsAt as AsAtHoldingDate");
                DataTable dtClient = dv.ToTable();

                dtClientHoldingDetail.Columns.Add("BenAccountNumber");
                if (RBReportView.SelectedItem.Value.ToString() == "S")
                    dtClientHoldingDetail.Columns.Add("Rate_Date");

                string strBenAccNumber = null;
                string strRate = null;
                string strRateDate = null;
                string totalValue = "";
                for (int i = 0; i < dtClientHoldingDetail.Rows.Count; i++)
                {
                    if (RBReportView.SelectedItem.Value.ToString() == "C")
                    {
                        if (dtClientHoldingDetail.Rows[i]["ISIN No."].ToString().Contains("Beneficiary AccountID:"))
                        {
                            strBenAccNumber = dtClientHoldingDetail.Rows[i]["ISIN No."].ToString().Replace("Beneficiary AccountID:", "");
                            strBenAccNumber = strBenAccNumber.Substring(0, strBenAccNumber.IndexOf("|", 0)).Trim();
                        }
                    }
                    else    //ISIN Wise========================
                    {
                        if (dtClientHoldingDetail.Rows[i]["Client ID"].ToString().Contains("ISIN:"))
                        {
                            strBenAccNumber = dtClientHoldingDetail.Rows[i]["Client ID"].ToString().Replace("ISIN:", "");
                            strBenAccNumber = strBenAccNumber.Substring(0, strBenAccNumber.IndexOf("|", 0)).Trim();
                        }
                    }
                    dtClientHoldingDetail.Rows[i]["BenAccountNumber"] = strBenAccNumber;

                    //===========Total Calculation================
                    if (RBReportView.SelectedItem.Value.ToString() == "C")
                    {
                        if (dtClientHoldingDetail.Rows[i]["ISIN No."].ToString().Contains("Total Value of Holding:"))
                        {
                            if (chkCalHoldingValue.Checked == true)
                            {
                                totalValue = (dtClientHoldingDetail.Rows[i]["ISIN No."].ToString()).Substring((dtClientHoldingDetail.Rows[i]["ISIN No."].ToString()).IndexOf(":") + 1).Trim();
                                dtClientHoldingDetail.Rows[i]["ISIN No."] = "";
                                dtClientHoldingDetail.Rows[i]["ISIN Name"] = "";
                                dtClientHoldingDetail.Rows[i]["Rate"] = "Total Value : ";
                                dtClientHoldingDetail.Rows[i]["Value"] = totalValue;
                            }
                            else
                                dtClientHoldingDetail.Rows[i]["Value"] = "";
                        }
                        dtClientHoldingDetail.AcceptChanges();
                    }
                    else  //ISIN Wise========================
                    {
                        if (dtClientHoldingDetail.Rows[i]["Client ID"].ToString().Contains("Rate: "))
                        {
                            string strRateDetail = dtClientHoldingDetail.Rows[i]["Client ID"].ToString().Substring(dtClientHoldingDetail.Rows[i]["Client ID"].ToString().IndexOf("Rate", 0)).Trim();
                            strRate = strRateDetail.Substring(0, strRateDetail.IndexOf("|", 0)).Trim();
                            strRate = strRate.Replace("Rate: ", "");
                            strRateDate = strRateDetail.Substring(strRateDetail.IndexOf("Date", 0)).Trim();
                            strRateDate = strRateDate.Substring(0).Trim();
                            strRateDate = strRateDate.Replace("Date: ", "");
                        }
                        dtClientHoldingDetail.Rows[i]["Rate"] = strRate;
                        dtClientHoldingDetail.Rows[i]["Rate_Date"] = strRateDate;

                        //-----------Start Total value Showing---------------------------
                        if ((dtClientHoldingDetail.Rows[i]["Client ID"].ToString().Contains("Total Value of Holding:")) || (dtClientHoldingDetail.Rows[i]["Client ID"].ToString().Contains("Sum(CurrentBalance):")))
                        {
                            string totalCurrentBalance = "";
                            string totalFree = "";
                            string totalPledged = "";
                            string totalRemat = "";
                            string totalDemat = "";

                            string[] strTotalValues = dtClientHoldingDetail.Rows[i][0].ToString().Split('|');
                            totalCurrentBalance = strTotalValues[0].ToString();
                            totalCurrentBalance = totalCurrentBalance.Replace("Sum(CurrentBalance):", "");
                            totalFree = strTotalValues[1].ToString();
                            totalFree = totalFree.Replace("Sum(Free):", "");
                            totalPledged = strTotalValues[2].ToString();
                            totalPledged = totalPledged.Replace("Sum(Pledged):", "");
                            totalRemat = strTotalValues[3].ToString();
                            totalRemat = totalRemat.Replace("Sum(Remat):", "");
                            totalDemat = strTotalValues[4].ToString();
                            totalDemat = totalDemat.Replace("Sum(Demat):", "");

                            dtClientHoldingDetail.Rows[i]["Client ID"] = "";
                            dtClientHoldingDetail.Rows[i]["Client Name"] = "Total Value : ";
                            dtClientHoldingDetail.Rows[i]["Free"] = totalFree;
                            dtClientHoldingDetail.Rows[i]["Pending Demat"] = totalDemat;
                            dtClientHoldingDetail.Rows[i]["Pending Remat"] = totalRemat;
                            dtClientHoldingDetail.Rows[i]["Pledged"] = totalPledged;
                            dtClientHoldingDetail.Rows[i]["Current Balance"] = totalCurrentBalance;

                            if (chkCalHoldingValue.Checked == true)
                            {
                                totalValue = strTotalValues[5].ToString();
                                totalValue = totalValue.Replace("Total Value of Holding:", "");
                                //dtClientHoldingDetail.Rows[i]["Rate"] = "Total Value : ";
                                dtClientHoldingDetail.Rows[i]["Value"] = totalValue;
                            }
                            else
                            {
                                dtClientHoldingDetail.Rows[i]["Value"] = "";
                            }
                            dtClientHoldingDetail.AcceptChanges();
                        }
                    }
                    if (RBReportView.SelectedItem.Value.ToString() == "C")
                    {
                        if ((dtClientHoldingDetail.Rows[i]["ISIN No."].ToString().Contains("Beneficiary AccountID:")) || (dtClientHoldingDetail.Rows[i]["ISIN No."].ToString().Contains("ISIN:")))
                        {
                            dtClientHoldingDetail.Rows[i]["ISIN No."] = "";
                            dtClientHoldingDetail.Rows[i]["ISIN Name"] = "";
                            dtClientHoldingDetail.AcceptChanges();
                        }
                    }
                    else
                    {
                        if ((dtClientHoldingDetail.Rows[i]["Client ID"].ToString().Contains("Beneficiary AccountID:")) || (dtClientHoldingDetail.Rows[i]["Client ID"].ToString().Contains("ISIN:")))
                        {
                            dtClientHoldingDetail.Rows[i]["Client ID"] = "";
                            dtClientHoldingDetail.Rows[i]["Client Name"] = "";
                            dtClientHoldingDetail.AcceptChanges();
                        }
                    }
                }
                dtClientHoldingDetail.AcceptChanges();

                if (RBReportView.SelectedItem.Value.ToString() == "C")
                {
                    if (dtClientHoldingDetail.Rows.Count > 0)
                    {
                        dtClientHoldingDetail.Columns.Add("BenTypeSubtype");
                        dtClientHoldingDetail.Columns.Add("BenAccCategory");
                        dtClientHoldingDetail.Columns.Add("AccountStatus");
                        dtClientHoldingDetail.Columns.Add("ActivationDate");
                        dtClientHoldingDetail.Columns.Add("FHName");
                        dtClientHoldingDetail.Columns.Add("PAN");
                        dtClientHoldingDetail.Columns.Add("SHName");
                        dtClientHoldingDetail.Columns.Add("FHAddress");
                        dtClientHoldingDetail.Columns.Add("SHAddress");
                        dtClientHoldingDetail.Columns.Add("PinCode");
                        for (int i = 0; i < dtClientHoldingDetail.Rows.Count; i++)
                        {
                            DataTable dtClientDetail = oDBEngine.GetDataTable(@"Select Top 1 case when dbo.fn_BeneficiaryType(NsdlClients_BenType,NsdlClients_BenSubType)='CM' then 'Clearing Member'
				                                                                        else dbo.fn_BeneficiaryType(NsdlClients_BenType,NsdlClients_BenSubType) end 
		                                                                        + case when dbo.fn_BeneficiarySubType(NsdlClients_BenType,NsdlClients_BenSubType) is null then ''
				                                                                        else ' [ '+ dbo.fn_BeneficiarySubType(NsdlClients_BenType,NsdlClients_BenSubType)+' ] '
		                                                                        end as BenTypeSubtype,
		                                                                        BenAccCategory=dbo.fn_BenAccCategory(NsdlClients_BenAccountCategory),
		                                                                        dbo.fn_BenStatus(NsdlClients_BeneficiaryStatus)as AccountStatus,
                                                                                CONVERT(varchar,NsdlClients_ActivationDateTime,106) as ActivationDate,
		                                                                        ltrim(rtrim(NsdlClients_BenFirstHolderName)) as FHName,
		                                                                        ltrim(rtrim(NsdlClients_FirstHolderPAN)) as PAN,

		                                                                        ltrim(rtrim(NsdlClients_BenSecondHolderName)) +
		                                                                        case when len(ltrim(rtrim(NsdlClients_BenThirdHolderName))) = 0
		                                                                        then ''
		                                                                        else ', ' + ltrim(rtrim(NsdlClients_BenThirdHolderName))
		                                                                        end as SHName,  

		                                                                        ltrim(rtrim(NsdlClients_FirstHolderAdd1)) +
		                                                                        case when len(ltrim(rtrim(NsdlClients_FirstHolderAdd2))) > 0
		                                                                        then ', ' + ltrim(rtrim(NsdlClients_FirstHolderAdd2)) 
		                                                                        else '' end as FHAddress,

		                                                                        case when len(ltrim(rtrim(NsdlClients_FirstHolderAdd3))) > 0
		                                                                        then ltrim(rtrim(NsdlClients_FirstHolderAdd3))
		                                                                        else '' end +
		                                                                        case when len(ltrim(rtrim(NsdlClients_FristHolderAdd4))) > 0
		                                                                        then 
			                                                                        case when len(ltrim(rtrim(NsdlClients_FirstHolderAdd3))) > 0
			                                                                        then ', ' + ltrim(rtrim(NsdlClients_FristHolderAdd4))
			                                                                        else ltrim(rtrim(NsdlClients_FristHolderAdd4)) end
		                                                                        else '' end as SHAddress,

		                                                                        case when len(ltrim(rtrim(NsdlClients_FirstHolderPinCode))) > 0
		                                                                        then 'Pin: ' + ltrim(rtrim(NsdlClients_FirstHolderPinCode))
		                                                                        else '' end +
		                                                                        case when len(ltrim(rtrim(NsdlClients_FirstHolderMobile))) > 0
		                                                                        then ', Mobile: ' + ltrim(rtrim(NsdlClients_FirstHolderMobile))
		                                                                        when len(ltrim(rtrim(NsdlClients_FirstHolderPhone))) > 0
		                                                                        then ', Phone: ' + ltrim(rtrim(NsdlClients_FirstHolderPhone))
		                                                                        else '' end as PinCode	
                                                                        From Master_NSDLClients
                                                                        Where NsdlClients_BenAccountID='" + dtClientHoldingDetail.Rows[i]["BenAccountNumber"] + "'");
                            dtClientHoldingDetail.Rows[i]["BenTypeSubtype"] = dtClientDetail.Rows[0]["BenTypeSubtype"];
                            dtClientHoldingDetail.Rows[i]["BenAccCategory"] = dtClientDetail.Rows[0]["BenAccCategory"];
                            dtClientHoldingDetail.Rows[i]["AccountStatus"] = dtClientDetail.Rows[0]["AccountStatus"];
                            dtClientHoldingDetail.Rows[i]["ActivationDate"] = dtClientDetail.Rows[0]["ActivationDate"];
                            dtClientHoldingDetail.Rows[i]["FHName"] = dtClientDetail.Rows[0]["FHName"];
                            dtClientHoldingDetail.Rows[i]["PAN"] = dtClientDetail.Rows[0]["PAN"];
                            dtClientHoldingDetail.Rows[i]["SHName"] = dtClientDetail.Rows[0]["SHName"];
                            dtClientHoldingDetail.Rows[i]["FHAddress"] = dtClientDetail.Rows[0]["FHAddress"];
                            dtClientHoldingDetail.Rows[i]["SHAddress"] = dtClientDetail.Rows[0]["SHAddress"];
                            dtClientHoldingDetail.Rows[i]["PinCode"] = dtClientDetail.Rows[0]["PinCode"];
                        }
                        dtClientHoldingDetail.AcceptChanges();
                    }
                }
                if (RBReportView.SelectedItem.Value.ToString() == "S")
                {
                    if (dtClientHoldingDetail.Rows.Count > 0)
                    {
                        dtClientHoldingDetail.Columns.Add("ISIN_Name");
                        for (int i = 0; i < dtClientHoldingDetail.Rows.Count; i++)
                        {
                            DataTable dtClientDetail = oDBEngine.GetDataTable(@"Select ltrim(rtrim(NSDLISIN_CompanyName)) ISIN_Name 
                                                                            From Master_NSDLISIN
                                                                            Where NSDLISIN_Number='" + dtClientHoldingDetail.Rows[i]["BenAccountNumber"] + "'");
                            dtClientHoldingDetail.Rows[i]["ISIN_Name"] = dtClientDetail.Rows[0]["ISIN_Name"];
                        }
                    }
                }

                //====For Logo Add==============
                byte[] logoinByte;
                DataTable dtLogo = new DataTable();
                dtLogo.Columns.Add("Image", System.Type.GetType("System.Byte[]"));

                if (oconverter.getLogoImage(HttpContext.Current.Server.MapPath(@"..\images\logo.jpg"), out logoinByte) != 1)
                {
                    ScriptManager.RegisterStartupScript(HttpContext.Current.Handler as Page, HttpContext.Current.GetType(), "logo", "alert('Logo not Found.');", true);
                }
                else
                {
                    dtLogo.Rows.Add(logoinByte);
                }
                dtLogo.AcceptChanges();

                DataSet dsHoldingDetail = new DataSet();
                dsHoldingDetail.Tables.Add(dtCompany);
                dsHoldingDetail.Tables.Add(dtClientHoldingDetail);
                dsHoldingDetail.Tables.Add(dtLogo);
                dsHoldingDetail.AcceptChanges();
                //======Generate XSD===========================            
                //dsHoldingDetail.Tables[0].WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\NSDL_HoldingCompany.xsd");
                //dsHoldingDetail.Tables[1].WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\NSDL_ClientHoldingDetail.xsd");
                //dsHoldingDetail.Tables[1].WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\NSDL_ClientHoldingShareDetail.xsd");
                //dsHoldingDetail.Tables[2].WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\NSDL_HoldingCompanyLogo.xsd");
                string IsValueChecked = "";
                if (chkCalHoldingValue.Checked == true)
                    IsValueChecked = "T";
                else
                    IsValueChecked = "F";

                ReportDocument oReportDocument = new ReportDocument();
                oReportDocument.Load(path);
                oReportDocument.SetDataSource(dsHoldingDetail.Tables[1]);
                oReportDocument.Subreports["NSDL_CompanyHolding"].SetDataSource(dsHoldingDetail.Tables[0]);
                oReportDocument.Subreports["NSDL_HoldingLogo"].SetDataSource(dsHoldingDetail.Tables[2]);
                oReportDocument.SetDatabaseLogon(connPath[2].Substring(connPath[2].IndexOf("=")).Trim(), connPath[3].Substring(connPath[3].IndexOf("=")).Trim(), connPath[0].Substring(connPath[0].IndexOf("=")).Trim(), connPath[1].Substring(connPath[1].IndexOf("=")).Trim(), false);

                #endregion

                if (RBReportView.SelectedItem.Value.ToString() == "S")
                {
                    if (hiddenIsinMail.Value == "T")
                    {
                        string pdfDateTime = oDBEngine.GetDate(113).ToString();
                        string pdfTime = pdfDateTime.Replace(":", "");
                        pdfTime = pdfTime.Replace(" ", "");
                        string DocumentName = "NSDLHolding_" + pdfTime + ".PDF";
                        string VirtualPath;
                        string strPdfMailPath = oconverter.DirectoryPath(out VirtualPath);
                        string DocumentPath = strPdfMailPath + "\\" + DocumentName;


                        oReportDocument.VerifyDatabase();
                        oReportDocument.SetParameterValue("@IsValueChecked", IsValueChecked);
                        oReportDocument.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, DocumentPath);

                        int EmailCreateAppMenuId = 0;
                        DataTable dtmenu = oDBEngine.GetDataTable("tbl_trans_menu", "mnu_id", "mnu_menuName='Statement Of Holding' and mnu_segmentID=9");
                        if (dtmenu.Rows.Count > 0)
                        {
                            EmailCreateAppMenuId = Convert.ToInt32(dtmenu.Rows[0][0].ToString());
                        }
                        //==========Sp to save Mail Content=================
                        string[] strUserIDs = User.ToString().Split(',');
                        int Status = 0;
                        for (int i = 0; i < strUserIDs.Length; i++)
                        {
                            DataTable dtEmail = oDBEngine.GetDataTable(" tbl_master_email ", "top 1 *", "eml_cntId='" + strUserIDs[i] + "' and eml_type='Official'");
                            string RecipientEmailID = dtEmail.Rows[0]["eml_email"].ToString();
                            if (RecipientEmailID != "")
                            {
                                Status = MailSend_ForISIN(strUserIDs[i].ToString(), RecipientEmailID, EmailCreateAppMenuId, DocumentName, VirtualPath);
                            }
                        }
                        hiddenIsinMail.Value = "F";
                        this.Page.ClientScript.RegisterStartupScript(GetType(), "jscript123456", "<script>alert('Mail sent Successfully!!!');</script>");
                    }
                    else
                    {
                        oReportDocument.SetParameterValue("@IsValueChecked", IsValueChecked);
                        oReportDocument.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, HttpContext.Current.Response, true, "NSDL Holding Report");
                    }
                }
                else
                {
                    oReportDocument.SetParameterValue("@IsValueChecked", IsValueChecked);
                    oReportDocument.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, HttpContext.Current.Response, true, "NSDL Holding Report");
                }
            }
        }

        public int MailSend_ForISIN(string RecipientContactID, string RecipientEmailID, int EmailCreateAppMenuId,
                                     string DocumentName, string DocumentPath)
        {
            int i = dailyrep.MailSend_ForISIN(Session["LastFinYear"].ToString(), Session["LastCompany"].ToString(), Session["usersegid"].ToString().Trim(),
                Convert.ToDateTime(txtDate.Text).ToString("dd-MMM-yyyy"), RecipientContactID, DocumentName, DocumentPath,
                Session["userid"].ToString(), RecipientEmailID, EmailCreateAppMenuId);


            return i;
            //using (SqlConnection connn = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]))
            //{
            //    SqlCommand cmd = new SqlCommand("Insert_UnSignedAttachDocuments", connn);
            //    cmd.CommandType = CommandType.StoredProcedure;

            //    cmd.Parameters.AddWithValue("@FinancialYear", Session["LastFinYear"].ToString());
            //    cmd.Parameters.AddWithValue("@CompanyID", Session["LastCompany"].ToString());
            //    cmd.Parameters.AddWithValue("@Segment_OR_DPID", Session["usersegid"].ToString().Trim());
            //    cmd.Parameters.AddWithValue("@Segment_Name", "NSDL");
            //    cmd.Parameters.AddWithValue("@ContractDate", Convert.ToDateTime(txtDate.Text).ToString("dd-MMM-yyyy"));
            //    cmd.Parameters.AddWithValue("@BranchID", "1");//==for digital Signed
            //    cmd.Parameters.AddWithValue("@ContactID_OR_BenAccNumber", RecipientContactID);
            //    cmd.Parameters.AddWithValue("@DocumentType", "21");
            //    cmd.Parameters.AddWithValue("@DocumentName", DocumentName);
            //    cmd.Parameters.AddWithValue("@DocumentPath", DocumentPath + "/" + DocumentName);
            //    cmd.Parameters.AddWithValue("@user", Session["userid"].ToString());
            //    cmd.Parameters.AddWithValue("@RecipientEmailID", RecipientEmailID);
            //    cmd.Parameters.AddWithValue("@EmailCreateAppMenuId", EmailCreateAppMenuId);

            //    cmd.CommandTimeout = 0;

            //    if (connn.State == ConnectionState.Open)
            //    {
            //        connn.Close();
            //    }
            //    connn.Open();
            //    int i = cmd.ExecuteNonQuery();
            //    connn.Close();

            //    return i;
            //}
        }
        #endregion

        protected void gridHolding_SummaryDisplayText(object sender, ASPxGridViewSummaryDisplayTextEventArgs e)
        {
            if (RBReportView.SelectedItem.Value == "C")
            {
                if (e.Item.FieldName == "Total")
                {
                    e.Text = "";
                }
                if (e.Item.FieldName == "Free")
                {
                    e.Text = "";
                }
                if (e.Item.FieldName == "Pledge")
                {
                    e.Text = "";
                }
                if (e.Item.FieldName == "Remat")
                {
                    e.Text = "";
                }
                if (e.Item.FieldName == "Demat")
                {
                    e.Text = "";
                }
            }
        }
    }
}