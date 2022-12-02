using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrystalDecisions.CrystalReports.Engine;

namespace ERP.OMS.Reports
{
    public partial class Reports_InterestPenaltyStatement : System.Web.UI.Page, System.Web.UI.ICallbackEventHandler
    {
        BusinessLogicLayer.FAReportsOther objFAReportsOther = new BusinessLogicLayer.FAReportsOther();
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        BusinessLogicLayer.Converter oconverter = new BusinessLogicLayer.Converter();
        string data;
        string SubLedgerType = "";
        string Branch;
        string Segment;
        string SegmentT;
        string MainAcID;
        string SubAcID;
        string SegMentName;
        string Group;
        string BranchId;
        string MainAcc;
        string Clients;


        string CompanyID = null;
        string SegmentID = null;
        string SegN = "";


        ExcelFile objExcel = new ExcelFile();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["userlastsegment"].ToString() == "9" || Session["userlastsegment"].ToString() == "10")
            {
                ddlAccountType.SelectedValue = "3";
                //  ddlAccountType.Attributes.Add("onchange", "AccountChange(this.value)");
                Page.ClientScript.RegisterStartupScript(GetType(), "JSd", "<script language='JavaScript'>AccountChange('" + ddlAccountType.SelectedItem.Value + "');</script>");

            }

            if (!IsPostBack)
            {
                //DataTable dtSeg = oDBEngine.GetDataTable("tbl_master_segment", "seg_name", " seg_id=" + Session["userlastsegment"].ToString() + "");
                //if (dtSeg.Rows.Count > 0)
                //    litSegment.InnerHtml = dtSeg.Rows[0][0].ToString();
                dtFrom.EditFormatString = oconverter.GetDateFormat("Date");
                dtTo.EditFormatString = oconverter.GetDateFormat("Date");
                //dtFrom.Value = Convert.ToDateTime(DateTime.Today);                
                string[] FinalCialYear = Session["LastFinYear"].ToString().Split('-');
                dtFrom.Value = Convert.ToDateTime(oDBEngine.GetDate().Month + "/01/" + oDBEngine.GetDate().Year);
                
                ////// Old Code ///////
                //dtTo.Value = Convert.ToDateTime("03/31/" + FinalCialYear[1].ToString());
                //////////////////////

                ///// New Code /////////
                string vTmpDtTo = FinalCialYear[1].ToString() + "-03-31";
                dtTo.Value = DateTime.ParseExact(vTmpDtTo, "yyyy-MM-dd", null);
                ///////////////////////

                Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script language='javascript'>Page_Load();</script>");


                DataTable DtSegComp = new DataTable();
                SegmentID = null;
                CompanyID = null;
                string SegMentName = null;




                DataTable dtSeg = oDBEngine.GetDataTable("tbl_master_segment", "seg_name", " seg_id=" + Session["userlastsegment"].ToString() + "");
                //if (dtSeg.Rows[0][0].ToString().EndsWith("CM"))
                //    DtSegComp = oDBEngine.GetDataTable("(select exch_compId,exch_internalId,isnull((select exh_shortName from tbl_master_Exchange where exh_cntId=tbl_master_companyExchange.exch_exchId)+'-'+exch_segmentId,exch_membershiptype) as Comp from tbl_master_companyExchange where exch_compid in (select ls_lastCompany from tbl_trans_Lastsegment where ls_lastSegment in(" + Session["userallsegmentnotonlyLast"].ToString() + "))) as D", "*", "Comp in(select seg_name from tbl_master_segment where seg_id in(" + Session["userallsegmentnotonlyLast"].ToString() + "))  and Comp like '%CM' and exch_compID='" + Session["LastCompany"].ToString() + "'");
                //else
                //    DtSegComp = oDBEngine.GetDataTable("(select exch_compId,exch_internalId,isnull((select exh_shortName from tbl_master_Exchange where exh_cntId=tbl_master_companyExchange.exch_exchId)+'-'+exch_segmentId,exch_membershiptype) as Comp from tbl_master_companyExchange where exch_compid in (select ls_lastCompany from tbl_trans_Lastsegment where ls_lastSegment=" + Session["userlastsegment"].ToString() + ")) as D", "*", "Comp in(select seg_name from tbl_master_segment where seg_id=" + Session["userlastsegment"].ToString() + ")");
                if (dtSeg.Rows[0][0].ToString().EndsWith("CM"))
                    DtSegComp = oDBEngine.GetDataTable("(select top 1 exch_compId,exch_internalId ,isnull((select exh_shortName from tbl_master_Exchange where exh_cntId=tbl_master_companyExchange.exch_exchId)+'-'+exch_segmentId,exch_membershiptype) as Seg , isnull((select exh_shortName from tbl_master_Exchange where exh_cntId=tbl_master_companyExchange.exch_exchId)+' - '+exch_segmentId,exch_membershiptype) as Comp from tbl_master_companyExchange where exch_compid in (select  ls_lastCompany from tbl_trans_Lastsegment where ls_lastSegment in(" + Session["userallsegmentnotonlyLast"].ToString() + "))) as D", " exch_compId,exch_internalId ,Comp ", "Seg in(select seg_name from tbl_master_segment where seg_id in(" + Session["userallsegmentnotonlyLast"].ToString() + "))  and Comp like '%CM' and exch_compID='" + Session["LastCompany"].ToString() + "'");
                else
                    DtSegComp = oDBEngine.GetDataTable("(select top 1 exch_compId,exch_internalId,isnull((select exh_shortName from tbl_master_Exchange where exh_cntId=tbl_master_companyExchange.exch_exchId)+'-'+exch_segmentId,exch_membershiptype) as Seg,isnull((select exh_shortName from tbl_master_Exchange where exh_cntId=tbl_master_companyExchange.exch_exchId)+' - '+exch_segmentId,exch_membershiptype) as Comp from tbl_master_companyExchange where exch_compid in (select  ls_lastCompany from tbl_trans_Lastsegment where ls_lastSegment=" + Session["userlastsegment"].ToString() + ")) as D", " exch_compId,exch_internalId ,Comp ", "Seg in(select seg_name from tbl_master_segment where seg_id=" + Session["userlastsegment"].ToString() + ")");
                if (DtSegComp.Rows.Count > 0)
                {
                    CompanyID = DtSegComp.Rows[0][0].ToString();
                    for (int i = 0; i < DtSegComp.Rows.Count; i++)
                    {
                        if (SegmentID == null)
                        {
                            SegmentID = DtSegComp.Rows[i][1].ToString();
                            SegMentName = DtSegComp.Rows[i][2].ToString();
                            SegN = "'" + DtSegComp.Rows[i][2].ToString() + "'";
                        }
                        else
                        {
                            SegmentID = SegmentID + "," + DtSegComp.Rows[i][1].ToString();
                            SegMentName = SegMentName + "," + DtSegComp.Rows[i][2].ToString();
                            SegN = SegN + ",'" + DtSegComp.Rows[i][2].ToString() + "'";
                        }
                    }
                    //ViewState["SegMentName"] = SegMentName;
                    //Session["CompanyID"] = CompanyID;
                    ViewState["SegmentID"] = SegmentID;
                    // HdnSegment.Value = SegmentID;

                    litSegment.InnerText = SegN;
                    //     litSegment.InnerText = SegMentName;


                }


            }
            //_____For performing operation without refreshing page___//
            String cbReference = Page.ClientScript.GetCallbackEventReference(this, "arg", "ReceiveServerData", "context");
            String callbackScript = "function CallServer(arg, context){ " + cbReference + ";}";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "CallServer", callbackScript, true);
            //___________-end here___//
        }
        protected void btnhide_Click(object sender, EventArgs e)
        {
            if (ddlGroup.SelectedItem.Value.ToString() == "1")
            {
                BindGroup();
            }
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

        #region ICallbackEventHandler Members

        public string GetCallbackResult()
        {
            return data;
        }

        public void RaiseCallbackEvent(string eventArgument)
        {
            string id = eventArgument.ToString();
            string[] idlist = id.Split('~');
            string str = "";
            string str1 = "";
            if (idlist[0] == "ComboChange")
            {
                MainAcID = idlist[1];
            }
            else
            {
                string[] cl = idlist[1].Split(',');
                for (int i = 0; i < cl.Length; i++)
                {
                    if (idlist[0] != "Ac Name")
                    {
                        SubLedgerType = "";
                        string[] val = cl[i].Split(';');
                        if (str == "")
                        {
                            str = "'" + val[0] + "'";
                            str1 = val[0] + ";" + val[1];
                        }
                        else
                        {
                            str += ",'" + val[0] + "'";
                            str1 += "," + val[0] + ";" + val[1];
                        }
                    }
                    else
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
                        SubLedgerType = AcVal[1];
                    }
                }
                if (idlist[0] == "Branch")
                {
                    Branch = str;
                    data = "Branch~" + str;
                }
                else if (idlist[0] == "Segment")
                {
                    SegmentT = str;
                    data = "Segment~" + str1;
                }
                else if (idlist[0] == "Ac Name")
                {
                    MainAcID = str;
                    data = "Ac Name~" + str + "~" + SubLedgerType;
                    // FillDropDown();
                }
                else if (idlist[0] == "Sub Ac")
                {
                    SubAcID = str;
                    data = "Sub Ac~" + str;
                }
                else if (idlist[0] == "Clients")
                {
                    SubAcID = str;
                    data = "Clients~" + str;
                }
                else if (idlist[0] == "Group")
                {
                    SubAcID = str;
                    data = "Group~" + str;
                }
            }
        }

        #endregion
        void fn_Client()
        {
            BranchId = HdnBranch.Value;
            Group = HdnGroup.Value;
            if (ddlAccountType.SelectedItem.Value == "0")
                MainAcc = "'SYSTM00001'";
            else if (ddlAccountType.SelectedItem.Value == "1")
                MainAcc = "'SYSTM00002'";
            else if (ddlAccountType.SelectedItem.Value == "2")
                MainAcc = "'SYSTM00001','SYSTM00002'";
            else
                MainAcc = HdnMainAc.Value;
            if (BranchId != "" && BranchId != null)
                ViewState["branchID"] = BranchId;
            else
                ViewState["branchID"] = Session["userbranchHierarchy"].ToString();

            if (rdSubAcAll.Checked)//////////////////ALL CLIENT CHECK
            {
                if (ddlGroup.SelectedItem.Value.ToString() == "1")
                {
                    if (rdddlgrouptypeAll.Checked)
                    {
                        if (ddlgrouptype.SelectedItem.Value.ToString() != "0")
                        {
                            DataTable dtclient = new DataTable();
                            dtclient = oDBEngine.GetDataTable("tbl_master_contact", "cnt_internalID", " cnt_internalid in(select grp_contactid from tbl_trans_group where  grp_groupmaster in(select gpm_id from tbl_master_groupmaster where gpm_type='" + ddlgrouptype.SelectedItem.Text.ToString() + "'))");
                            if (dtclient.Rows.Count > 0)
                            {
                                for (int i = 0; i < dtclient.Rows.Count; i++)
                                {
                                    if (Clients == null)
                                        Clients = "'" + dtclient.Rows[i][0].ToString() + "'";
                                    else
                                        Clients += "," + "'" + dtclient.Rows[i][0].ToString() + "'";
                                }

                            }

                        }
                    }
                    else
                    {
                        DataTable dtclient = new DataTable();
                        dtclient = oDBEngine.GetDataTable("tbl_master_contact", "cnt_internalID", " cnt_internalid in(select grp_contactid from tbl_trans_group where grp_groupmaster in(" + Group + "))");
                        if (dtclient.Rows.Count > 0)
                        {
                            for (int i = 0; i < dtclient.Rows.Count; i++)
                            {
                                if (Clients == null)
                                    Clients = "'" + dtclient.Rows[i][0].ToString() + "'";
                                else
                                    Clients += "," + "'" + dtclient.Rows[i][0].ToString() + "'";
                            }

                        }
                    }
                }
                else
                {
                    if (rdbranchAll.Checked)
                    {
                        DataTable dtclient = new DataTable();
                        dtclient = oDBEngine.GetDataTable("tbl_master_contact", "cnt_internalID", " cnt_branchid in(" + Session["userbranchHierarchy"].ToString() + ")");
                        if (dtclient.Rows.Count > 0)
                        {
                            for (int i = 0; i < dtclient.Rows.Count; i++)
                            {
                                if (Clients == null)
                                    Clients = "'" + dtclient.Rows[i][0].ToString() + "'";
                                else
                                    Clients += "," + "'" + dtclient.Rows[i][0].ToString() + "'";
                            }

                        }
                    }
                    else
                    {
                        DataTable dtclient = new DataTable();
                        dtclient = oDBEngine.GetDataTable("tbl_master_contact", "cnt_internalID", " cnt_branchid in(" + BranchId + ")");
                        if (dtclient.Rows.Count > 0)
                        {
                            for (int i = 0; i < dtclient.Rows.Count; i++)
                            {
                                if (Clients == null)
                                    Clients = "'" + dtclient.Rows[i][0].ToString() + "'";
                                else
                                    Clients += "," + "'" + dtclient.Rows[i][0].ToString() + "'";
                            }

                        }
                    }
                }
            }
            if (rdSubAcSelected.Checked == true)
                Clients = HdnSubAc.Value;

            ViewState["Clients"] = Clients;
        }
        void fn_ClientCDSL()
        {
            ViewState["Clients"] = null;
            BranchId = HdnBranch.Value;
            Group = HdnGroup.Value;
            MainAcc = HdnMainAc.Value;
            SubLedgerType = HdnSubLedgerType.Value;
            if (BranchId != "" && BranchId != null)
                ViewState["branchID"] = BranchId;
            else
                ViewState["branchID"] = Session["userbranchHierarchy"].ToString();
            string NSDlCdsl = null;
            if (rdSubAcAll.Checked)//////////////////ALL CLIENT CHECK
            {
                if (ddlGroup.SelectedItem.Value.ToString() == "1")
                {
                    if (rdddlgrouptypeAll.Checked)
                    {
                        if (ddlgrouptype.SelectedItem.Value.ToString() != "0")
                        {
                            DataTable dtclient = new DataTable();
                            dtclient = oDBEngine.GetDataTable("tbl_trans_group", "substring(ltrim(rtrim(grp_contactid)),9,8)", "  grp_contactid in(select grp_contactid from tbl_trans_group where  grp_groupmaster in(select gpm_id from tbl_master_groupmaster where gpm_type='" + ddlgrouptype.SelectedItem.Text.ToString() + "'))");
                            if (dtclient.Rows.Count > 0)
                            {
                                for (int i = 0; i < dtclient.Rows.Count; i++)
                                {
                                    if (Clients == null)
                                        Clients = "'" + dtclient.Rows[i][0].ToString() + "'";
                                    else
                                        Clients += "," + "'" + dtclient.Rows[i][0].ToString() + "'";
                                }

                            }

                        }
                    }
                    else
                    {
                        DataTable dtclient = new DataTable();
                        dtclient = oDBEngine.GetDataTable("tbl_trans_group", "substring(ltrim(rtrim(grp_contactid)),9,8)", " grp_contactid in(select grp_contactid from tbl_trans_group where grp_groupmaster in(" + Group + "))");
                        if (dtclient.Rows.Count > 0)
                        {
                            for (int i = 0; i < dtclient.Rows.Count; i++)
                            {
                                if (Clients == null)
                                    Clients = "'" + dtclient.Rows[i][0].ToString() + "'";
                                else
                                    Clients += "," + "'" + dtclient.Rows[i][0].ToString() + "'";
                            }

                        }
                    }
                }
                else
                {
                    if (rdbranchAll.Checked)
                    {
                        DataTable dtclient = new DataTable();
                        if (SubLedgerType == "CDSL Clients")
                            dtclient = oDBEngine.GetDataTable("master_CdslClients", "CdslClients_BenAccountNumber", null);
                        else if (SubLedgerType == "NSDL Clients")
                            dtclient = oDBEngine.GetDataTable("master_NsdlClients", "NsdlClients_BenAccountID", null);
                        if (dtclient.Rows.Count > 0)
                        {
                            for (int i = 0; i < dtclient.Rows.Count; i++)
                            {
                                if (Clients == null)
                                    Clients = "'" + dtclient.Rows[i][0].ToString() + "'";
                                else
                                    Clients += "," + "'" + dtclient.Rows[i][0].ToString() + "'";
                            }

                        }
                    }
                    else
                    {
                        DataTable dtclient = new DataTable();
                        if (SubLedgerType == "CDSL Clients")
                            dtclient = oDBEngine.GetDataTable("master_CdslClients", "CdslClients_BenAccountNumber", null);
                        else if (SubLedgerType == "NSDL Clients")
                            dtclient = oDBEngine.GetDataTable("master_NsdlClients", "NsdlClients_BenAccountID", null);
                        if (dtclient.Rows.Count > 0)
                        {
                            for (int i = 0; i < dtclient.Rows.Count; i++)
                            {
                                if (Clients == null)
                                    Clients = "'" + dtclient.Rows[i][0].ToString() + "'";
                                else
                                    Clients += "," + "'" + dtclient.Rows[i][0].ToString() + "'";
                            }

                        }
                    }
                }
            }
            if (rdSubAcSelected.Checked == true)
                Clients = HdnSubAc.Value;
            ViewState["Clients"] = Clients;
        }
        protected void btnReport_Click(object sender, EventArgs e)
        {
            DataSet dsCrystal = new DataSet();
            ViewState["Clients"] = null;
            ViewState["branchID"] = null;
            SubLedgerType = HdnSubLedgerType.Value;
            SegmentT = HdnSegment.Value;
            SubAcID = HdnSubAc.Value;
            ViewState["SubAcID"] = SubAcID;
            MainAcID = HdnMainAc.Value;
            string GpType = null;
            string GpId = null;
            string AddType = null;
            string Debit = "0";
            string Credit = "0";
            string GracePeriod = "0";
            if (SubLedgerType == "CDSL Clients" || SubLedgerType == "NSDL Clients")
            {
                fn_ClientCDSL();
                AddType = "Y";
            }
            else if (SubLedgerType == "")
            {
                fn_Client();
                AddType = "Y";
            }
            else
            {
                fn_Custom();
                AddType = "N";
            }
            if (rdbSegAll.Checked == true)
            {
                DataTable dtSegment = oDBEngine.GetDataTable("tbl_master_companyExchange", "exch_internalId", "exch_compId='" + Session["LastCompany"].ToString() + "'");
                if (dtSegment.Rows.Count > 0)
                {
                    for (int i = 0; i < dtSegment.Rows.Count; i++)
                    {
                        if (Segment == null)
                            Segment = dtSegment.Rows[i][0].ToString();
                        else
                            Segment += "," + dtSegment.Rows[i][0].ToString();
                    }
                }
            }
            else
            {
                Segment = Session["usersegid"].ToString();
            }
            if (ViewState["branchID"] == null)
            {
                if (rdbranchAll.Checked == true)
                {
                    Branch = Session["userbranchHierarchy"].ToString();
                }
            }
            else
                Branch = ViewState["branchID"].ToString();


            GpType = ddlGroup.SelectedItem.Value.ToString();
            if (GpType == "1")
            {
                DataTable DTGpType = oDBEngine.GetDataTable("tbl_master_groupmaster", "gpm_id", " gpm_type='" + ddlgrouptype.SelectedItem.Text.Trim() + "'");
                if (DTGpType.Rows.Count > 0)
                {
                    for (int i = 0; i < DTGpType.Rows.Count; i++)
                    {
                        if (GpId == null)
                            GpId = "'" + DTGpType.Rows[i][0].ToString() + "'";
                        else
                            GpId += "," + "'" + DTGpType.Rows[i][0].ToString() + "'";
                    }
                }
            }
            else
                GpId = "0";
            if (txtDebits.Text != "")
                Debit = txtDebits.Text;
            if (txtCredits.Text != "")
                Credit = txtCredits.Text;
            if (txtgracePeriod.Text != "")
                GracePeriod = txtgracePeriod.Text;

            string SingleDouble = null;
            if (radSingle.Checked == true)
                SingleDouble = "S";
            else
                SingleDouble = "B";


            dsCrystal = objFAReportsOther.InterestPenaltyStatement(
                Convert.ToDateTime(dtFrom.Value).ToString("yyyy-MM-dd"),
                Convert.ToDateTime(dtTo.Value).ToString("yyyy-MM-dd"),
                     Convert.ToString(Segment),
                    Convert.ToString(MainAcID),
                    Convert.ToString(ViewState["Clients"]),
                    Convert.ToString(ddlGenerateFor.SelectedItem.Value),
                  Convert.ToDecimal(Debit).ToString(),
                   Convert.ToDecimal(Credit).ToString(),
                    Convert.ToString(GracePeriod),
                    Convert.ToString(ddlStatementType.SelectedItem.Value),
                    Convert.ToString(Session["LastCompany"]),
                    Convert.ToString(Branch),
                    Convert.ToString(HttpContext.Current.Session["LastFinYear"]),
                    Convert.ToString(GpType),
                     Convert.ToString(GpId),
                         Convert.ToString(SingleDouble),
                    Convert.ToString(SubLedgerType),
                     Convert.ToString(AddType)
                     );

            byte[] logoinByte;
            dsCrystal.Tables[0].Columns.Add("Image", System.Type.GetType("System.Byte[]"));
            if (oconverter.getLogoImage(HttpContext.Current.Server.MapPath(@"..\images\logo.jpg"), out logoinByte) != 1)
            {
                ScriptManager.RegisterStartupScript(HttpContext.Current.Handler as Page, HttpContext.Current.GetType(), "logo", "alert('Logo not Found.');", true);

            }
            else
            {
                for (int i = 0; i < dsCrystal.Tables[0].Rows.Count; i++)
                {
                    dsCrystal.Tables[0].Rows[i]["Image"] = logoinByte;
                }
                if (ddlStatementType.SelectedItem.Value == "1")
                {
                    //dsCrystal.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "//Reports//InterestPenaltyStatement.xsd");
                    string[] connPath = ConfigurationManager.AppSettings["DBConnectionDefault"].Split(';');
                    ReportDocument reportObj = new ReportDocument();
                    string ReportPath = Server.MapPath("..\\Reports\\InterestPenaltyStatement.rpt");
                    reportObj.Load(ReportPath);
                    reportObj.SetDatabaseLogon(connPath[2].Substring(connPath[2].IndexOf("=")).Trim(), connPath[3].Substring(connPath[3].IndexOf("=")).Trim(), connPath[0].Substring(connPath[0].IndexOf("=")).Trim(), connPath[1].Substring(connPath[1].IndexOf("=")).Trim(), false);
                    reportObj.SetDataSource(dsCrystal);
                    //reportObj.Subreports["logo"].SetDataSource(dsCrystal.Tables[0]);
                    reportObj.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, HttpContext.Current.Response, true, "AccountsLedger");
                    reportObj.Dispose();
                }
                else
                {
                    //dsCrystal.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "//Reports//InterestPenaltyStatementSummary.xsd");
                    string[] connPath = ConfigurationManager.AppSettings["DBConnectionDefault"].Split(';');
                    ReportDocument reportObj = new ReportDocument();
                    string ReportPath = Server.MapPath("..\\Reports\\InterestPenaltyStatementSummary.rpt");
                    reportObj.Load(ReportPath);
                    reportObj.SetDatabaseLogon(connPath[2].Substring(connPath[2].IndexOf("=")).Trim(), connPath[3].Substring(connPath[3].IndexOf("=")).Trim(), connPath[0].Substring(connPath[0].IndexOf("=")).Trim(), connPath[1].Substring(connPath[1].IndexOf("=")).Trim(), false);
                    reportObj.SetDataSource(dsCrystal);
                    //reportObj.Subreports["logo"].SetDataSource(dsCrystal.Tables[0]);
                    reportObj.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, HttpContext.Current.Response, true, "AccountsLedger");
                    reportObj.Dispose();
                }
                GC.Collect();
            }
            //using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]))
            //{
            //    SqlCommand cmd = new SqlCommand();
            //    cmd.Connection = con;
            //    cmd.CommandText = "InterestPenaltyStatement";
            //    cmd.CommandType = CommandType.StoredProcedure;
            //    cmd.Parameters.AddWithValue("@Fromdate", Convert.ToDateTime(Convert.ToDateTime(dtFrom.Value).ToShortDateString()));
            //    cmd.Parameters.AddWithValue("@ToDate", Convert.ToDateTime(Convert.ToDateTime(dtTo.Value).ToShortDateString()));
            //    cmd.Parameters.AddWithValue("@Segment", Segment);
            //    cmd.Parameters.AddWithValue("@MainAccount", MainAcID);
            //    cmd.Parameters.AddWithValue("@SubAccount", ViewState["Clients"].ToString());
            //    cmd.Parameters.AddWithValue("@GenerateFor", ddlGenerateFor.SelectedItem.Value.ToString());
            //    cmd.Parameters.AddWithValue("@Debit", Convert.ToDecimal(Debit));
            //    cmd.Parameters.AddWithValue("@Credit", Convert.ToDecimal(Credit));
            //    cmd.Parameters.AddWithValue("@GracePeriod", Convert.ToInt32(GracePeriod));
            //    cmd.Parameters.AddWithValue("@StatementType", ddlStatementType.SelectedItem.Value);
            //    cmd.Parameters.AddWithValue("@CompID", Session["LastCompany"].ToString());
            //    cmd.Parameters.AddWithValue("@branchID", Branch);
            //    cmd.Parameters.AddWithValue("@FinYear", HttpContext.Current.Session["LastFinYear"]);
            //    cmd.Parameters.AddWithValue("@GrpBranchType", GpType);
            //    cmd.Parameters.AddWithValue("@GroupID", GpId);
            //    cmd.Parameters.AddWithValue("@SingleDouble", SingleDouble);
            //    cmd.Parameters.AddWithValue("@SubLedgerType", SubLedgerType);
            //    cmd.Parameters.AddWithValue("@AddType", AddType);
            //    cmd.CommandTimeout = 0;
            //    SqlDataAdapter Adap = new SqlDataAdapter();
            //    Adap.SelectCommand = cmd;
            //    Adap.Fill(dsCrystal);
            //    cmd.Dispose();
            //    con.Dispose();
            //    GC.Collect();
            //    byte[] logoinByte;
            //    dsCrystal.Tables[0].Columns.Add("Image", System.Type.GetType("System.Byte[]"));
            //    if (oconverter.getLogoImage(HttpContext.Current.Server.MapPath(@"..\images\logo.jpg"), out logoinByte) != 1)
            //    {
            //        ScriptManager.RegisterStartupScript(HttpContext.Current.Handler as Page, HttpContext.Current.GetType(), "logo", "alert('Logo not Found.');", true);

            //    }
            //    else
            //    {
            //        for (int i = 0; i < dsCrystal.Tables[0].Rows.Count; i++)
            //        {
            //            dsCrystal.Tables[0].Rows[i]["Image"] = logoinByte;
            //        }
            //        if (ddlStatementType.SelectedItem.Value == "1")
            //        {
            //            dsCrystal.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "//Reports//InterestPenaltyStatement.xsd");
            //            string[] connPath = ConfigurationManager.AppSettings["DBConnectionDefault"].Split(';');
            //            ReportDocument reportObj = new ReportDocument();
            //            string ReportPath = Server.MapPath("..\\Reports\\InterestPenaltyStatement.rpt");
            //            reportObj.Load(ReportPath);
            //            reportObj.SetDatabaseLogon(connPath[2].Substring(connPath[2].IndexOf("=")).Trim(), connPath[3].Substring(connPath[3].IndexOf("=")).Trim(), connPath[0].Substring(connPath[0].IndexOf("=")).Trim(), connPath[1].Substring(connPath[1].IndexOf("=")).Trim(), false);
            //            reportObj.SetDataSource(dsCrystal);
            //            reportObj.Subreports["logo"].SetDataSource(dsCrystal.Tables[0]);
            //            reportObj.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, HttpContext.Current.Response, true, "AccountsLedger");
            //            reportObj.Dispose();
            //        }
            //        else
            //        {
            //            dsCrystal.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "//Reports//InterestPenaltyStatementSummary.xsd");
            //            string[] connPath = ConfigurationManager.AppSettings["DBConnectionDefault"].Split(';');
            //            ReportDocument reportObj = new ReportDocument();
            //            string ReportPath = Server.MapPath("..\\Reports\\InterestPenaltyStatementSummary.rpt");
            //            reportObj.Load(ReportPath);
            //            reportObj.SetDatabaseLogon(connPath[2].Substring(connPath[2].IndexOf("=")).Trim(), connPath[3].Substring(connPath[3].IndexOf("=")).Trim(), connPath[0].Substring(connPath[0].IndexOf("=")).Trim(), connPath[1].Substring(connPath[1].IndexOf("=")).Trim(), false);
            //            reportObj.SetDataSource(dsCrystal);
            //            reportObj.Subreports["logo"].SetDataSource(dsCrystal.Tables[0]);
            //            reportObj.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, HttpContext.Current.Response, true, "AccountsLedger");
            //            reportObj.Dispose();
            //        }
            //        GC.Collect();
            //    }
            //}
        }
        public void fn_Custom()
        {
            MainAcID = HdnMainAc.Value;
            if (rdSubAcAll.Checked)//////////////////ALL CLIENT CHECK
            {
                DataTable dtclient = new DataTable();
                dtclient = oDBEngine.GetDataTable("Trans_AccountsLedger", "AccountsLedger_SubAccountID", " AccountsLedger_MainAccountID=" + MainAcID + " and AccountsLedger_TransactionDate between '" + Convert.ToDateTime(dtFrom.Value).ToShortDateString() + "' and '" + Convert.ToDateTime(dtTo.Value).ToShortDateString() + "'");
                if (dtclient.Rows.Count > 0)
                {
                    for (int i = 0; i < dtclient.Rows.Count; i++)
                    {
                        if (Clients == null)
                            Clients = "'" + dtclient.Rows[i][0].ToString() + "'";
                        else
                            Clients += "," + "'" + dtclient.Rows[i][0].ToString() + "'";
                    }

                }
            }
            if (rdSubAcSelected.Checked == true)
                Clients = HdnSubAc.Value;
            ViewState["Clients"] = Clients;
        }


        protected void btnExport_Click(object sender, EventArgs e)
        {
            DataSet dsCrystal = new DataSet();
            ViewState["Clients"] = null;
            ViewState["branchID"] = null;
            SubLedgerType = HdnSubLedgerType.Value;
            SegmentT = HdnSegment.Value;
            SubAcID = HdnSubAc.Value;
            ViewState["SubAcID"] = SubAcID;
            MainAcID = HdnMainAc.Value;
            string GpType = null;
            string GpId = null;
            string AddType = null;
            string Debit = "0";
            string Credit = "0";
            string GracePeriod = "0";
            if (SubLedgerType == "CDSL Clients" || SubLedgerType == "NSDL Clients")
            {
                fn_ClientCDSL();
                AddType = "Y";
            }
            else if (SubLedgerType == "")
            {
                fn_Client();
                AddType = "Y";
            }
            else
            {
                fn_Custom();
                AddType = "N";
            }
            if (rdbSegAll.Checked == true)
            {
                DataTable dtSegment = oDBEngine.GetDataTable("tbl_master_companyExchange", "exch_internalId", "exch_compId='" + Session["LastCompany"].ToString() + "'");
                if (dtSegment.Rows.Count > 0)
                {
                    for (int i = 0; i < dtSegment.Rows.Count; i++)
                    {
                        if (Segment == null)
                            Segment = dtSegment.Rows[i][0].ToString();
                        else
                            Segment += "," + dtSegment.Rows[i][0].ToString();
                    }
                }
            }
            else
            {
                Segment = Session["usersegid"].ToString();
            }
            if (ViewState["branchID"] == null)
            {
                if (rdbranchAll.Checked == true)
                {
                    Branch = Session["userbranchHierarchy"].ToString();
                }
            }
            else
                Branch = ViewState["branchID"].ToString();


            GpType = ddlGroup.SelectedItem.Value.ToString();
            if (GpType == "1")
            {
                DataTable DTGpType = oDBEngine.GetDataTable("tbl_master_groupmaster", "gpm_id", " gpm_type='" + ddlgrouptype.SelectedItem.Text.Trim() + "'");
                if (DTGpType.Rows.Count > 0)
                {
                    for (int i = 0; i < DTGpType.Rows.Count; i++)
                    {
                        if (GpId == null)
                            GpId = "'" + DTGpType.Rows[i][0].ToString() + "'";
                        else
                            GpId += "," + "'" + DTGpType.Rows[i][0].ToString() + "'";
                    }
                }
            }
            else
                GpId = "0";
            if (txtDebits.Text != "")
                Debit = txtDebits.Text;
            if (txtCredits.Text != "")
                Credit = txtCredits.Text;
            if (txtgracePeriod.Text != "")
                GracePeriod = txtgracePeriod.Text;

            string SingleDouble = null;
            if (radSingle.Checked == true)
                SingleDouble = "S";
            else
                SingleDouble = "B";
            //using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]))
            //{
            //    SqlCommand cmd = new SqlCommand();
            //    cmd.Connection = con;
            //    cmd.CommandText = "InterestPenaltyStatement";
            //    cmd.CommandType = CommandType.StoredProcedure;
            //    cmd.Parameters.AddWithValue("@Fromdate", Convert.ToDateTime(Convert.ToDateTime(dtFrom.Value).ToShortDateString()));
            //    cmd.Parameters.AddWithValue("@ToDate", Convert.ToDateTime(Convert.ToDateTime(dtTo.Value).ToShortDateString()));
            //    cmd.Parameters.AddWithValue("@Segment", Segment);
            //    cmd.Parameters.AddWithValue("@MainAccount", MainAcID);
            //    cmd.Parameters.AddWithValue("@SubAccount", ViewState["Clients"].ToString());
            //    cmd.Parameters.AddWithValue("@GenerateFor", ddlGenerateFor.SelectedItem.Value.ToString());
            //    cmd.Parameters.AddWithValue("@Debit", Convert.ToDecimal(Debit));
            //    cmd.Parameters.AddWithValue("@Credit", Convert.ToDecimal(Credit));
            //    cmd.Parameters.AddWithValue("@GracePeriod", Convert.ToInt32(GracePeriod));
            //    cmd.Parameters.AddWithValue("@StatementType", ddlStatementType.SelectedItem.Value);
            //    cmd.Parameters.AddWithValue("@CompID", Session["LastCompany"].ToString());
            //    cmd.Parameters.AddWithValue("@branchID", Branch);
            //    cmd.Parameters.AddWithValue("@FinYear", HttpContext.Current.Session["LastFinYear"]);
            //    cmd.Parameters.AddWithValue("@GrpBranchType", GpType);
            //    cmd.Parameters.AddWithValue("@GroupID", GpId);
            //    cmd.Parameters.AddWithValue("@SingleDouble", SingleDouble);
            //    cmd.Parameters.AddWithValue("@SubLedgerType", SubLedgerType);
            //    cmd.Parameters.AddWithValue("@AddType", AddType);
            //    cmd.CommandTimeout = 0;
            //    SqlDataAdapter Adap = new SqlDataAdapter();
            //    Adap.SelectCommand = cmd;
            //    Adap.Fill(dsCrystal);
            //    ViewState["DsExport"] = dsCrystal;
            //    cmd.Dispose();
            //    con.Dispose();
            //    GC.Collect();

            //}



            dsCrystal = objFAReportsOther.InterestPenaltyStatement(
              Convert.ToDateTime(dtFrom.Value).ToString("yyyy-MM-dd"),
              Convert.ToDateTime(dtTo.Value).ToString("yyyy-MM-dd"),
                   Convert.ToString(Segment),
                  Convert.ToString(MainAcID),
                  Convert.ToString(ViewState["Clients"]),
                  Convert.ToString(ddlGenerateFor.SelectedItem.Value),
                  Convert.ToDecimal(Debit).ToString(),
                  Convert.ToDecimal(Credit).ToString(),
                  Convert.ToString(GracePeriod),
                  Convert.ToString(ddlStatementType.SelectedItem.Value),
                  Convert.ToString(Session["LastCompany"]),
                  Convert.ToString(Branch),
                  Convert.ToString(HttpContext.Current.Session["LastFinYear"]),
                  Convert.ToString(GpType),
                   Convert.ToString(GpId),
                       Convert.ToString(SingleDouble),
                  Convert.ToString(SubLedgerType),
                   Convert.ToString(AddType)
                   );

            ViewState["DsExport"] = dsCrystal;
            if (ddlStatementType.SelectedItem.Value == "0")
            {
                DataTable dtEx = new DataTable();
                DataTable DtMain = new DataTable();
                int CountB = 0;
                DtMain = dsCrystal.Tables[0];
                dtEx.Columns.Add("MainAccName");
                dtEx.Columns.Add("SubAccName");
                dtEx.Columns.Add("UCC");
                dtEx.Columns.Add("Closing");
                dtEx.Columns.Add("InterestRate");
                dtEx.Columns.Add("Interest");
                for (int i = 0; i < DtMain.Rows.Count; i++)
                {
                    if (DtMain.Rows.Count == 1)
                    {
                        DataRow row14 = dtEx.NewRow();
                        row14[0] = DtMain.Rows[i]["MainAccName"].ToString();
                        row14[1] = DtMain.Rows[i]["SubAccName"].ToString(); ;
                        row14[2] = DtMain.Rows[i]["UCC"].ToString(); ;
                        row14[3] = DtMain.Rows[i]["Closing"].ToString(); ;
                        row14[4] = DtMain.Rows[i]["InterestRate"].ToString(); ;
                        row14[5] = DtMain.Rows[i]["AmtTotal"].ToString(); ;
                        dtEx.Rows.Add(row14);

                        DataRow row15 = dtEx.NewRow();
                        row15[0] = "Branch/Group Total";
                        row15[1] = "";
                        row15[2] = "";
                        row15[3] = "";
                        row15[4] = "";
                        row15[5] = DtMain.Rows[i]["BrGrpTotal"].ToString(); ;
                        dtEx.Rows.Add(row15);

                        DataRow row16 = dtEx.NewRow();
                        row16[0] = "";
                        row16[1] = "Grand Total";
                        row16[2] = "";
                        row16[3] = "";
                        row16[4] = "";
                        row16[5] = DtMain.Rows[i]["GrandTotal"].ToString(); ;
                        dtEx.Rows.Add(row16);

                    }
                    else if (DtMain.Rows.Count > i + 1)
                    {
                        if (DtMain.Rows[i]["Branch"].ToString().Trim() == DtMain.Rows[i + 1]["Branch"].ToString().Trim())
                        {
                            if (CountB == 0)
                            {
                                CountB = CountB + 1;
                                DataRow row2 = dtEx.NewRow();
                                row2[0] = DtMain.Rows[i]["Branch"].ToString();
                                row2[1] = "";
                                row2[2] = "";
                                row2[3] = "";
                                row2[4] = "";
                                row2[5] = "";
                                dtEx.Rows.Add(row2);
                            }

                            DataRow row4 = dtEx.NewRow();
                            row4[0] = DtMain.Rows[i]["MainAccName"].ToString();
                            row4[1] = DtMain.Rows[i]["SubAccName"].ToString(); ;
                            row4[2] = DtMain.Rows[i]["UCC"].ToString(); ;
                            row4[3] = DtMain.Rows[i]["Closing"].ToString(); ;
                            row4[4] = DtMain.Rows[i]["InterestRate"].ToString(); ;
                            row4[5] = DtMain.Rows[i]["AmtTotal"].ToString(); ;
                            dtEx.Rows.Add(row4);


                        }
                        else
                        {
                            if (CountB != 0)
                            {
                                if (DtMain.Rows[i]["Branch"].ToString().Trim() == DtMain.Rows[i - 1]["Branch"].ToString().Trim())
                                {
                                    CountB = CountB;
                                }
                                else
                                {
                                    CountB = 0;

                                }
                            }

                            if (CountB == 0)
                            {
                                CountB = CountB + 1;
                                DataRow row5 = dtEx.NewRow();
                                row5[0] = DtMain.Rows[i]["Branch"].ToString();
                                row5[1] = "";
                                row5[2] = "";
                                row5[3] = "";
                                row5[4] = "";
                                row5[5] = "";
                                dtEx.Rows.Add(row5);

                            }

                            DataRow row7 = dtEx.NewRow();
                            row7[0] = DtMain.Rows[i]["MainAccName"].ToString();
                            row7[1] = DtMain.Rows[i]["SubAccName"].ToString(); ;
                            row7[2] = DtMain.Rows[i]["UCC"].ToString(); ;
                            row7[3] = DtMain.Rows[i]["Closing"].ToString(); ;
                            row7[4] = DtMain.Rows[i]["InterestRate"].ToString(); ;
                            row7[5] = DtMain.Rows[i]["AmtTotal"].ToString(); ;
                            dtEx.Rows.Add(row7);

                            DataRow row8 = dtEx.NewRow();
                            row8[0] = "Branch/Group Total";
                            row8[1] = "";
                            row8[2] = "";
                            row8[3] = "";
                            row8[4] = "";
                            row8[5] = DtMain.Rows[i]["BrGrpTotal"].ToString(); ;
                            dtEx.Rows.Add(row8);

                        }
                    }
                    else if (DtMain.Rows.Count == i + 1)
                    {

                        if (DtMain.Rows[i]["Branch"].ToString().Trim() == DtMain.Rows[i - 1]["Branch"].ToString().Trim())
                        {

                            DataRow row4 = dtEx.NewRow();
                            row4[0] = DtMain.Rows[i]["MainAccName"].ToString();
                            row4[1] = DtMain.Rows[i]["SubAccName"].ToString(); ;
                            row4[2] = DtMain.Rows[i]["UCC"].ToString(); ;
                            row4[3] = DtMain.Rows[i]["Closing"].ToString(); ;
                            row4[4] = DtMain.Rows[i]["InterestRate"].ToString(); ;
                            row4[5] = DtMain.Rows[i]["AmtTotal"].ToString(); ;
                            dtEx.Rows.Add(row4);

                            DataRow row10 = dtEx.NewRow();
                            row10[0] = "Branch/Group Total";
                            row10[1] = "";
                            row10[2] = "";
                            row10[3] = "";
                            row10[4] = "";
                            row10[5] = DtMain.Rows[i]["BrGrpTotal"].ToString(); ;
                            dtEx.Rows.Add(row10);

                            DataRow row11 = dtEx.NewRow();
                            row11[0] = "";
                            row11[1] = "Grand Total";
                            row11[2] = "";
                            row11[3] = "";
                            row11[4] = "";
                            row11[5] = DtMain.Rows[i]["GrandTotal"].ToString(); ;
                            dtEx.Rows.Add(row11);



                        }
                        else
                        {
                            if (DtMain.Rows[i]["Branch"].ToString().Trim() == DtMain.Rows[i - 1]["Branch"].ToString().Trim())
                            {
                                CountB = CountB;
                            }
                            else
                            {
                                CountB = 0;

                            }
                            if (CountB == 0)
                            {
                                CountB = CountB + 1;
                                DataRow row5 = dtEx.NewRow();
                                row5[0] = DtMain.Rows[i]["Branch"].ToString();
                                row5[1] = "";
                                row5[2] = "";
                                row5[3] = "";
                                row5[4] = "";
                                row5[5] = "";
                                dtEx.Rows.Add(row5);

                            }

                            DataRow row7 = dtEx.NewRow();
                            row7[0] = DtMain.Rows[i]["MainAccName"].ToString();
                            row7[1] = DtMain.Rows[i]["SubAccName"].ToString(); ;
                            row7[2] = DtMain.Rows[i]["UCC"].ToString(); ;
                            row7[3] = DtMain.Rows[i]["Closing"].ToString(); ;
                            row7[4] = DtMain.Rows[i]["InterestRate"].ToString(); ;
                            row7[5] = DtMain.Rows[i]["AmtTotal"].ToString(); ;
                            dtEx.Rows.Add(row7);

                            DataRow row8 = dtEx.NewRow();
                            row8[0] = "Branch/Group Total";
                            row8[1] = "";
                            row8[2] = "";
                            row8[3] = "";
                            row8[4] = "";
                            row8[5] = DtMain.Rows[i]["BrGrpTotal"].ToString(); ;
                            dtEx.Rows.Add(row8);

                            DataRow row9 = dtEx.NewRow();
                            row9[0] = "";
                            row9[1] = "Grand Total";
                            row9[2] = "";
                            row9[3] = "";
                            row9[4] = "";
                            row9[5] = DtMain.Rows[i]["GrandTotal"].ToString(); ;
                            dtEx.Rows.Add(row9);

                        }

                    }

                }

                // DataTable CompanyName = oDBEngine.GetDataTable("tbl_master_company", "cmp_Name", " cmp_internalId='" + Session["LastCompany"].ToString() + "'");
                DataTable dtReportHeader = new DataTable();
                if (DtMain.Rows.Count > 0)
                {
                    dtReportHeader.Columns.Add(new DataColumn("Header", typeof(String))); //0
                    DataRow HeaderRow = dtReportHeader.NewRow();
                    HeaderRow[0] = DtMain.Rows[0]["CompName"].ToString();
                    dtReportHeader.Rows.Add(HeaderRow);

                    DataRow DrRowR1 = dtReportHeader.NewRow();
                    DrRowR1[0] = DtMain.Rows[0]["CompAddress"].ToString() + "  " + DtMain.Rows[0]["CompAddress1"].ToString() + "  " + DtMain.Rows[0]["CompAddress2"].ToString();
                    dtReportHeader.Rows.Add(DrRowR1);
                    DataRow DrRowR4 = dtReportHeader.NewRow();
                    DrRowR4[0] = " Phone Number: " + DtMain.Rows[0]["CompPhNumber"].ToString() + "         Fax: " + DtMain.Rows[0]["CompFaxNumber"].ToString();
                    dtReportHeader.Rows.Add(DrRowR4);
                    DataRow DrRowR7 = dtReportHeader.NewRow();
                    DrRowR7[0] = " Email: " + DtMain.Rows[0]["CompEmail"].ToString() + "          PAN: " + DtMain.Rows[0]["CompPanNo"].ToString();
                    dtReportHeader.Rows.Add(DrRowR7);
                    DataRow DrRowR2 = dtReportHeader.NewRow();
                    DrRowR2[0] = " Interest Penalty Statement ";
                    dtReportHeader.Rows.Add(DrRowR2);
                    DataRow DrRowR6 = dtReportHeader.NewRow();
                    DrRowR6[0] = " Period: " + DtMain.Rows[0]["FromDate"].ToString() + "           To  " + DtMain.Rows[0]["ToDate"].ToString();
                    dtReportHeader.Rows.Add(DrRowR6);
                }
                DataTable dtReportFooter = new DataTable();
                dtReportFooter.Columns.Add(new DataColumn("Footer", typeof(String))); //0
                DataRow FooterRow = dtReportFooter.NewRow();
                FooterRow[0] = "* * *  End Of Report * * *   ";
                dtReportFooter.Rows.Add(FooterRow);
                if ((dtEx != null && dtEx.Rows.Count > 0) && (dtReportHeader != null && dtReportHeader.Rows.Count > 0) && (dtReportFooter != null && dtReportFooter.Rows.Count > 0))
                    objExcel.ExportToExcelforExcel(dtEx, " Accounts Ledger ", "Branch/Group Total", dtReportHeader, dtReportFooter);

            }



        }








    }



}