using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogicLayer;
namespace ERP.OMS.Reports
{
    public partial class Reports_frmReportRegSusClient : System.Web.UI.Page, System.Web.UI.ICallbackEventHandler
    {
        DBEngine oDBEngine = new DBEngine(string.Empty);
        string data;
        ExcelFile objExcel = new ExcelFile();
        Converter oconverter = new Converter();
        string nsecm = "";
        string bsecm = "";
        string nsefo = "";
        string bsefo = "";
        string nsecdx = "";
        string mcxcdx = "";
        string usecdx = "";
        string mcxcomm = "";
        string ncdexcomm = "";
        string nmcecomm = "";
        string accounts = "";
        string callback = "";
        string spot = "";
        string mcxsxcm = "";
        string mcxsxfo = "";


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                dtFrom.Date = DateTime.Today;
                dtTo.Date = DateTime.Today;
                settno();
                BindGroup();
                Page.ClientScript.RegisterStartupScript(GetType(), "PageLoad", "<script>Page_Load();</script>");
                gridClients.Visible = false;
                tr_filter.Visible = false;
                Session["callback"] = "s";


            }
            else
            {

                Session["callback"] = "";
                BindGridothers();
            }
            Page.ClientScript.RegisterStartupScript(GetType(), "jsheight", "<script>height();</script>");

            String cbReference = Page.ClientScript.GetCallbackEventReference(this, "arg", "ReceiveServerData", "context");
            String callbackScript = "function CallServer(arg, context){ " + cbReference + ";}";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "CallServer", callbackScript, true);

        }
        protected void BindGridothers()
        {





            string[] InputName = new string[10];
            string[] InputType = new string[10];
            string[] InputValue = new string[10];

            InputName[0] = "FromDate";
            InputName[1] = "ToDate";
            InputName[2] = "Selectedby";
            InputName[3] = "Branches";
            InputName[4] = "ClientAllorSpecific";
            InputName[5] = "Groups";
            InputName[6] = "ClientCategory";
            InputName[7] = "CompanyId";
            InputName[8] = "CType";
            InputName[9] = "Segments";

            // InputName[9] = "GroupType";


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

            InputValue[0] = dtFrom.Text.Split('-')[2] + "-" + dtFrom.Text.Split('-')[1] + "-" + dtFrom.Text.Split('-')[0];
            InputValue[1] = dtTo.Text.Split('-')[2] + "-" + dtTo.Text.Split('-')[1] + "-" + dtTo.Text.Split('-')[0];
            InputValue[2] = ddlGroup.SelectedValue;


            string aa = litSegment.InnerText;
            string ab = HiddenField_SegmentName.Value;
            string cc = HiddenField_ClientCategory.Value;

            if (ddlGroup.SelectedValue == "2")
            {
                string branchids = "";
                if (rdbranchAll.Checked == true)
                {
                    DataTable dtbranchid = oDBEngine.GetDataTable("trans_branchgroupmembers", "distinct BranchGroupMembers_BranchID", null);
                    if (dtbranchid != null)
                    {
                        if (dtbranchid.Rows.Count > 0)
                        {
                            for (int r = 0; r < dtbranchid.Rows.Count; r++)
                            {
                                if (branchids == "")
                                    branchids = Convert.ToString(dtbranchid.Rows[r][0]);
                                else
                                    branchids = branchids + "," + Convert.ToString(dtbranchid.Rows[r][0]);

                            }

                        }
                    }
                }
                else
                {
                    DataTable dtbranchid = oDBEngine.GetDataTable("trans_branchgroupmembers", "distinct BranchGroupMembers_BranchID", "BranchGroupMembers_BranchGroupID in(" + HiddenField_BranchGroup.Value + ")");

                    if (dtbranchid != null)
                    {
                        if (dtbranchid.Rows.Count > 0)
                        {
                            for (int r = 0; r < dtbranchid.Rows.Count; r++)
                            {
                                if (branchids == "")
                                    branchids = Convert.ToString(dtbranchid.Rows[r][0]);
                                else
                                    branchids = branchids + "," + Convert.ToString(dtbranchid.Rows[r][0]);

                            }

                        }
                    }
                }
                InputValue[3] = branchids;
            }
            else
            {
                if (rdbranchAll.Checked == true)
                    InputValue[3] = HttpContext.Current.Session["userbranchHierarchy"].ToString();
                else
                {
                    if (HiddenField_Branch.Value != "")
                        InputValue[3] = HiddenField_Branch.Value;
                    else
                        InputValue[3] = HttpContext.Current.Session["userbranchHierarchy"].ToString();

                }
            }
            if (rbClientsAll.Checked == true)
                InputValue[4] = "All";
            else
            {
                if (HiddenField_Client.Value != "")
                    InputValue[4] = HiddenField_Client.Value;
                else
                    InputValue[4] = "All";

            }


            if (rdddlgrouptypeAll.Checked == true)
            {
                DataTable dtGroup = oDBEngine.GetDataTable("tbl_master_groupmaster", "gpm_id", "rtrim(ltrim(gpm_type))='" + ddlgrouptype.SelectedValue + "'");
                string strGroupIds = "";
                if (dtGroup != null)
                {

                    if (dtGroup.Rows.Count > 0)
                    {
                        for (int r = 0; r < dtGroup.Rows.Count; r++)
                        {
                            if (strGroupIds == "")
                                strGroupIds = Convert.ToString(dtGroup.Rows[r][0]);
                            else
                                strGroupIds = strGroupIds + "," + Convert.ToString(dtGroup.Rows[r][0]);


                        }

                    }

                }
                InputValue[5] = strGroupIds;
            }
            else
            {
                InputValue[5] = HiddenField_Group.Value;
            }
            if (rbClientCategoryAll.Checked == true)
            {
                string lgstatid = "";
                DataTable dtclcat = oDBEngine.GetDataTable("tbl_master_legalStatus", "lgl_id", null);
                for (int i = 0; i < dtclcat.Rows.Count; i++)
                {
                    if (lgstatid == "")
                        lgstatid = Convert.ToString(dtclcat.Rows[i][0]);
                    else
                        lgstatid = lgstatid + "," + Convert.ToString(dtclcat.Rows[i][0]);

                }
                InputValue[6] = lgstatid;

            }
            else
                InputValue[6] = HiddenField_ClientCategory.Value;

            InputValue[7] = txtCompany_hidden.Value;
            InputValue[8] = drpType.SelectedValue;
            if (rdbSegAll.Checked == true)
                InputValue[9] = "All";
            else
            {
                string[] arrsegs = HiddenField_SegmentName.Value.Split(',');
                string strSegments = "";
                for (int i = 0; i < arrsegs.Length; i++)
                {
                    if (strSegments == "")
                        strSegments = "'" + arrsegs[i] + "'";
                    else
                        strSegments = strSegments + ",'" + arrsegs[i] + "'";
                }
                InputValue[9] = strSegments;
                litSegment.InnerText = HiddenField_SegmentName.Value;
            }


            DataTable dt = SQLProcedures.SelectProcedureArr("Report_RegSusClient", InputName, InputType, InputValue);
            if (dt.Rows.Count > 0)
            {
                ViewState["ClientData"] = dt;
                gridClients.DataSource = dt;

                gridClients.DataBind();
                gridClients.Visible = true;
                tr_filter.Visible = true;
            }
            else
            {
                gridClients.Visible = false;
                tr_filter.Visible = false;
                //Page.ClientScript.RegisterStartupScript(GetType(), "Page_Load", "<script>Page_Load();</script>");
                //Page.ClientScript.RegisterStartupScript(GetType(), "JScript34", "<script language='javascript'>alert('No Record Found !');</script>");
                Page.ClientScript.RegisterStartupScript(GetType(), "reset", "<script>reset();</script>");
            }
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                    Page.ClientScript.RegisterStartupScript(GetType(), "gridrowy", "<script>gridrowy();</script>");
                else
                    Page.ClientScript.RegisterStartupScript(GetType(), "gridrown", "<script>gridrown();</script>");

            }
            else
                Page.ClientScript.RegisterStartupScript(GetType(), "gridrown", "<script>gridrown();</script>");

        }

        protected void BindGrid()
        {

            string[] InputName = new string[10];
            string[] InputType = new string[10];
            string[] InputValue = new string[10];

            InputName[0] = "FromDate";
            InputName[1] = "ToDate";
            InputName[2] = "Selectedby";
            InputName[3] = "Branches";
            InputName[4] = "ClientAllorSpecific";
            InputName[5] = "Groups";
            InputName[6] = "ClientCategory";
            InputName[7] = "CompanyId";
            InputName[8] = "CType";
            InputName[9] = "Segments";

            // InputName[9] = "GroupType";


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

            InputValue[0] = dtFrom.Text.Split('-')[2] + "-" + dtFrom.Text.Split('-')[1] + "-" + dtFrom.Text.Split('-')[0];
            InputValue[1] = dtTo.Text.Split('-')[2] + "-" + dtTo.Text.Split('-')[1] + "-" + dtTo.Text.Split('-')[0];
            InputValue[2] = ddlGroup.SelectedValue;


            string aa = litSegment.InnerText;
            string ab = HiddenField_SegmentName.Value;
            string cc = HiddenField_ClientCategory.Value;

            if (ddlGroup.SelectedValue == "2")
            {
                string branchids = "";
                if (rdbranchAll.Checked == true)
                {
                    DataTable dtbranchid = oDBEngine.GetDataTable("trans_branchgroupmembers", "distinct BranchGroupMembers_BranchID", null);
                    if (dtbranchid != null)
                    {
                        if (dtbranchid.Rows.Count > 0)
                        {
                            for (int r = 0; r < dtbranchid.Rows.Count; r++)
                            {
                                if (branchids == "")
                                    branchids = Convert.ToString(dtbranchid.Rows[r][0]);
                                else
                                    branchids = branchids + "," + Convert.ToString(dtbranchid.Rows[r][0]);

                            }

                        }
                    }
                }
                else
                {
                    DataTable dtbranchid = oDBEngine.GetDataTable("trans_branchgroupmembers", "distinct BranchGroupMembers_BranchID", "BranchGroupMembers_BranchGroupID in(" + HiddenField_BranchGroup.Value + ")");

                    if (dtbranchid != null)
                    {
                        if (dtbranchid.Rows.Count > 0)
                        {
                            for (int r = 0; r < dtbranchid.Rows.Count; r++)
                            {
                                if (branchids == "")
                                    branchids = Convert.ToString(dtbranchid.Rows[r][0]);
                                else
                                    branchids = branchids + "," + Convert.ToString(dtbranchid.Rows[r][0]);

                            }

                        }
                    }
                }
                InputValue[3] = branchids;
            }
            else
            {
                if (rdbranchAll.Checked == true)
                    InputValue[3] = HttpContext.Current.Session["userbranchHierarchy"].ToString();
                else
                {
                    if (HiddenField_Branch.Value != "")
                        InputValue[3] = HiddenField_Branch.Value;
                    else
                        InputValue[3] = HttpContext.Current.Session["userbranchHierarchy"].ToString();

                }
            }
            if (rbClientsAll.Checked == true)
                InputValue[4] = "All";
            else
            {
                if (HiddenField_Client.Value != "")
                    InputValue[4] = HiddenField_Client.Value;
                else
                    InputValue[4] = "All";

            }


            if (rdddlgrouptypeAll.Checked == true)
            {
                DataTable dtGroup = oDBEngine.GetDataTable("tbl_master_groupmaster", "gpm_id", "rtrim(ltrim(gpm_type))='" + ddlgrouptype.SelectedValue + "'");
                string strGroupIds = "";
                if (dtGroup != null)
                {

                    if (dtGroup.Rows.Count > 0)
                    {
                        for (int r = 0; r < dtGroup.Rows.Count; r++)
                        {
                            if (strGroupIds == "")
                                strGroupIds = Convert.ToString(dtGroup.Rows[r][0]);
                            else
                                strGroupIds = strGroupIds + "," + Convert.ToString(dtGroup.Rows[r][0]);


                        }

                    }

                }
                InputValue[5] = strGroupIds;
            }
            else
            {
                InputValue[5] = HiddenField_Group.Value;
            }
            if (rbClientCategoryAll.Checked == true)
            {
                string lgstatid = "";
                DataTable dtclcat = oDBEngine.GetDataTable("tbl_master_legalStatus", "lgl_id", null);
                for (int i = 0; i < dtclcat.Rows.Count; i++)
                {
                    if (lgstatid == "")
                        lgstatid = Convert.ToString(dtclcat.Rows[i][0]);
                    else
                        lgstatid = lgstatid + "," + Convert.ToString(dtclcat.Rows[i][0]);

                }
                InputValue[6] = lgstatid;

            }
            else
                InputValue[6] = HiddenField_ClientCategory.Value;

            InputValue[7] = txtCompany_hidden.Value;
            InputValue[8] = drpType.SelectedValue;
            if (rdbSegAll.Checked == true)
                InputValue[9] = "All";
            else
            {
                string[] arrsegs = HiddenField_SegmentName.Value.Split(',');
                string strSegments = "";
                for (int i = 0; i < arrsegs.Length; i++)
                {
                    if (strSegments == "")
                        strSegments = "'" + arrsegs[i] + "'";
                    else
                        strSegments = strSegments + ",'" + arrsegs[i] + "'";
                }
                InputValue[9] = strSegments;
                litSegment.InnerText = HiddenField_SegmentName.Value;
            }


            DataTable dt = SQLProcedures.SelectProcedureArr("Report_RegSusClient", InputName, InputType, InputValue);
            ViewState["ClientData"] = dt;
            gridClients.DataSource = dt;

            gridClients.DataBind();
            gridClients.Visible = true;
            tr_filter.Visible = true;

            for (int k = 0; k < dt.Rows.Count; k++)
            {
                nsecm += dt.Rows[k]["nsereg"].ToString();
                bsecm += dt.Rows[k]["bsereg"].ToString();
                nsefo += dt.Rows[k]["nforeg"].ToString();
                nsecdx += dt.Rows[k]["cdxreg"].ToString();
                bsefo += dt.Rows[k]["bseforeg"].ToString();
                accounts += dt.Rows[k]["accountsreg"].ToString();
                mcxcdx += dt.Rows[k]["mcxcdxreg"].ToString();
                ncdexcomm += dt.Rows[k]["ncdexcommreg"].ToString();
                mcxcomm += dt.Rows[k]["mcxcommreg"].ToString();
                nmcecomm += dt.Rows[k]["nmcereg"].ToString();
                usecdx += dt.Rows[k]["usecdxreg"].ToString();
                spot += dt.Rows[k]["nselspotreg"].ToString();
                mcxsxcm += dt.Rows[k]["mcxcmreg"].ToString();
                mcxsxfo += dt.Rows[k]["mcxforeg"].ToString();
            }
            if (ddlExport.SelectedItem.Value != "E" && ddlExport.SelectedItem.Value != "P")
            {

                if (mcxsxfo == "")
                {
                    gridClients.VisibleColumns[25].Visible = false;

                }
                if (mcxsxcm == "")
                {
                    gridClients.VisibleColumns[24].Visible = false;
                }
                if (spot == "")
                {
                    gridClients.VisibleColumns[23].Visible = false;
                }
                if (accounts == "")
                {
                    gridClients.VisibleColumns[22].Visible = false;
                }
                if (usecdx == "")
                {
                    gridClients.VisibleColumns[21].Visible = false;
                }
                if (nmcecomm == "")
                {
                    gridClients.VisibleColumns[20].Visible = false;
                }
                if (ncdexcomm == "")
                {
                    gridClients.VisibleColumns[19].Visible = false;
                }
                if (mcxcomm == "")
                {
                    gridClients.VisibleColumns[18].Visible = false;
                }
                if (mcxcdx == "")
                {
                    gridClients.VisibleColumns[17].Visible = false;
                }
                if (bsefo == "")
                {
                    gridClients.VisibleColumns[16].Visible = false;
                }
                if (bsecm == "")
                {
                    gridClients.VisibleColumns[15].Visible = false;
                }
                if (nsecdx == "")
                {
                    gridClients.VisibleColumns[14].Visible = false;
                }
                if (nsefo == "")
                {
                    gridClients.VisibleColumns[13].Visible = false;
                }
                if (nsecm == "")
                {
                    gridClients.VisibleColumns[12].Visible = false;
                }


            }
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                    Page.ClientScript.RegisterStartupScript(GetType(), "gridrowy", "<script>gridrowy();</script>");
                else
                    Page.ClientScript.RegisterStartupScript(GetType(), "gridrown", "<script>gridrown();</script>");

            }
            else
                Page.ClientScript.RegisterStartupScript(GetType(), "gridrown", "<script>gridrown();</script>");

        }
        protected void ddlExport_SelectedIndexChanged1(object sender, EventArgs e)
        {


            if (ddlExport.SelectedValue == "E")
            {
                ExportToExcel_Generic((DataTable)ViewState["ClientData"], "2007");

            }
            else if (ddlExport.SelectedValue == "P")
            {

                DataTable Dt = (DataTable)ViewState["ClientData"];
                Dt.Columns.Remove("clientid");

                Dt.Columns[0].ColumnName = "Client Name";
                Dt.Columns[1].ColumnName = "contractdeliverymode";
                Dt.Columns[2].ColumnName = "UCC";
                Dt.Columns[3].ColumnName = "Branch Name";
                Dt.Columns[4].ColumnName = "Group Name";
                Dt.Columns[5].ColumnName = "Phones";
                Dt.Columns[6].ColumnName = "Pancard";
                Dt.Columns[7].ColumnName = "Bank details";
                Dt.Columns[8].ColumnName = "Dp Account";
                Dt.Columns[9].ColumnName = "Email";
                Dt.Columns[10].ColumnName = "Address";

                Dt.Columns[11].ColumnName = "NSE-CM";
                Dt.Columns[12].ColumnName = "NSE-FO";
                Dt.Columns[13].ColumnName = "NSE-CDX";
                Dt.Columns[14].ColumnName = "BSE-CM";
                /////////////////////////////////
                Dt.Columns[15].ColumnName = "BSE-FO";
                Dt.Columns[16].ColumnName = "MCXSX-CDX";
                Dt.Columns[17].ColumnName = "MCX-COMM";
                Dt.Columns[18].ColumnName = "NCDEX-COMM";
                Dt.Columns[19].ColumnName = "NMCE-COMM";
                Dt.Columns[20].ColumnName = "USE-CDX";
                Dt.Columns[21].ColumnName = "ACCOUNTS";

                Dt.Columns[22].ColumnName = "NSEL-SPOT";
                Dt.Columns[23].ColumnName = "MCXSX-CM";
                Dt.Columns[24].ColumnName = "MCXSX-FO";

                DataColumn dcolSerialNo = Dt.Columns.Add("Srl No", System.Type.GetType("System.String"));
                dcolSerialNo.SetOrdinal(0);

                for (int i = 1; i <= Dt.Rows.Count; i++)
                {
                    Dt.Rows[i - 1]["Srl No"] = i;
                }
                for (int k = 0; k < Dt.Rows.Count; k++)
                {

                    nsecm += Dt.Rows[k]["NSE-CM"].ToString();
                    bsecm += Dt.Rows[k]["BSE-CM"].ToString();
                    nsefo += Dt.Rows[k]["NSE-FO"].ToString();
                    nsecdx += Dt.Rows[k]["NSE-CDX"].ToString();
                    bsefo += Dt.Rows[k]["BSE-FO"].ToString();
                    accounts += Dt.Rows[k]["ACCOUNTS"].ToString();
                    mcxcdx += Dt.Rows[k]["MCXSX-CDX"].ToString();
                    ncdexcomm += Dt.Rows[k]["NCDEX-COMM"].ToString();
                    mcxcomm += Dt.Rows[k]["MCX-COMM"].ToString();
                    nmcecomm += Dt.Rows[k]["NMCE-COMM"].ToString();
                    usecdx += Dt.Rows[k]["USE-CDX"].ToString();
                    spot += Dt.Rows[k]["NSEL-SPOT"].ToString();
                    mcxsxcm += Dt.Rows[k]["MCXSX-CM"].ToString();
                    mcxsxfo += Dt.Rows[k]["MCXSX-FO"].ToString();
                }
                if (ddlExport.SelectedItem.Value == "P")
                {
                    if (mcxsxfo == "")
                    {
                        Dt.Columns.Remove("MCXSX-FO");
                    }
                    if (mcxsxcm == "")
                    {
                        Dt.Columns.Remove("MCXSX-CM");
                    }
                    if (spot == "")
                    {
                        Dt.Columns.Remove("NSEL-SPOT");
                    }

                    if (accounts == "")
                    {
                        Dt.Columns.Remove("ACCOUNTS");

                    }
                    if (usecdx == "")
                    {
                        Dt.Columns.Remove("USE-CDX");
                    }
                    if (nmcecomm == "")
                    {
                        Dt.Columns.Remove("NMCE-COMM");
                    }
                    if (ncdexcomm == "")
                    {
                        Dt.Columns.Remove("NCDEX-COMM");
                    }
                    if (mcxcomm == "")
                    {
                        Dt.Columns.Remove("MCX-COMM");
                    }
                    if (mcxcdx == "")
                    {
                        Dt.Columns.Remove("MCXSX-CDX");
                    }
                    if (bsefo == "")
                    {
                        Dt.Columns.Remove("BSE-FO");
                    }
                    if (bsecm == "")
                    {
                        Dt.Columns.Remove("BSE-CM");
                    }
                    if (nsecdx == "")
                    {
                        Dt.Columns.Remove("NSE-CDX");
                    }
                    if (nsefo == "")
                    {
                        Dt.Columns.Remove("NSE-FO");
                    }
                    if (nsecm == "")
                    {
                        Dt.Columns.Remove("NSE-CM");
                    }


                }
                Dt.AcceptChanges();
                DataTable CompanyName = oDBEngine.GetDataTable("tbl_master_company", "cmp_Name", " cmp_internalId='" + txtCompany_hidden.Value.ToString() + "'");
                DataTable dtReportHeader = new DataTable();
                dtReportHeader.Columns.Add(new DataColumn("Header", typeof(String))); //0
                DataRow HeaderRow = dtReportHeader.NewRow();
                HeaderRow[0] = CompanyName.Rows[0][0].ToString();
                dtReportHeader.Rows.Add(HeaderRow);
                DataRow DrRowR1 = dtReportHeader.NewRow();
                string strSegs = "";
                if (rdbSegAll.Checked == true)
                    strSegs = "All";
                else
                    strSegs = "[" + litSegment.InnerText + "]";
                DrRowR1[0] = "Clients " + drpType.SelectedItem.Text + " Between  " + oconverter.ArrangeDate2(dtFrom.Value.ToString()) + "  to " + oconverter.ArrangeDate2(dtTo.Value.ToString()) + " With Company " + txtCompany.Text + " and Segment(s) " + strSegs;
                dtReportHeader.Rows.Add(DrRowR1);
                DataRow HeaderRow1 = dtReportHeader.NewRow();
                dtReportHeader.Rows.Add(HeaderRow1);
                DataRow HeaderRow2 = dtReportHeader.NewRow();
                dtReportHeader.Rows.Add(HeaderRow2);

                DataTable dtReportFooter = new DataTable();
                dtReportFooter.Columns.Add(new DataColumn("Footer", typeof(String))); //0
                DataRow FooterRow1 = dtReportFooter.NewRow();
                dtReportFooter.Rows.Add(FooterRow1);
                DataRow FooterRow2 = dtReportFooter.NewRow();
                dtReportFooter.Rows.Add(FooterRow2);
                DataRow FooterRow = dtReportFooter.NewRow();
                FooterRow[0] = "* * *  End Of Report * * *   ";
                dtReportFooter.Rows.Add(FooterRow);

                objExcel.ExportToPDF(Dt, drpType.SelectedItem.Text + " Clients", "Total", dtReportHeader, dtReportFooter);
            }
            Page.ClientScript.RegisterStartupScript(GetType(), "filterselct", "<script>selection();</script>");

        }
        void settno()
        {
            DataTable DtSegComp = oDBEngine.GetDataTable("(select exch_compId,exch_internalId,isnull((select exh_shortName from tbl_master_Exchange where exh_cntId=tbl_master_companyExchange.exch_exchId)+'-'+exch_segmentId,exch_membershiptype) as Comp,isnull((select exh_shortName from tbl_master_Exchange where exh_cntId=tbl_master_companyExchange.exch_exchId)+' - '+exch_segmentId,exch_membershiptype) as Comp1 from tbl_master_companyExchange where exch_compid='" + Session["LastCompany"].ToString() + "') as D", "*", "Comp in(select seg_name from tbl_master_segment where seg_id=" + Session["userlastsegment"].ToString() + ")");
            if (DtSegComp.Rows.Count > 0)
            {
                HiddenField_SegmentName.Value = DtSegComp.Rows[0][3].ToString();
                HiddenField_Segment.Value = DtSegComp.Rows[0][1].ToString();

            }


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
            if (idlist[0] == "ComboChange")
            {
                //MainAcID = idlist[1];
            }
            else
            {
                string[] cl = idlist[1].Split(',');
                for (int i = 0; i < cl.Length; i++)
                {
                    if (idlist[0] != "Clients")
                    {
                        if (idlist[0] == "SettlementType")
                        {
                            string[] val = cl[i].Split(';');
                            if (str == "")
                            {
                                str = "'" + val[0] + "'";
                                str1 = "'" + val[0] + "'" + ";" + val[1];
                            }
                            else
                            {
                                str += ",'" + val[0] + "'";
                                str1 += "," + "'" + val[0] + "'" + ";" + val[1];
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
                    }
                }

                if (idlist[0] == "Branch")
                {
                    data = "Branch~" + str;
                }
                else if (idlist[0] == "Segment")
                {
                    data = "Segment~" + str1;
                }

                else if (idlist[0] == "Clients")
                {
                    data = "Clients~" + str;
                }
                else if (idlist[0] == "Group")
                {
                    data = "Group~" + str;
                }
                else if (idlist[0] == "ClientCategory")
                {
                    data = "ClientCategory~" + str;
                }
                else if (idlist[0] == "BranchGroup")
                {
                    data = "BranchGroup~" + str;
                }
            }
        }
        protected void btnshow_Click(object sender, EventArgs e)
        {
            BindGrid();
        }
        protected void gridClients_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            callback = e.Parameters.ToString();

            if (e.Parameters == "s")
            {
                gridClients.Settings.ShowFilterRow = true;
                BindGridothers();

            }
            else if (e.Parameters == "All")
            {

                gridClients.FilterExpression = string.Empty;
                BindGridothers();

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
        void ExportToExcel_Generic(DataTable Dt, string ExcelVersion)
        {
            GenericExcelExport oGenericExcelExport = new GenericExcelExport();
            string strDownloadFileName = "";
            Dt.Columns.Remove("clientid");
            Dt.Columns[0].ColumnName = "Client Name";
            Dt.Columns[1].ColumnName = "contractdeliverymode";
            Dt.Columns[2].ColumnName = "UCC";
            Dt.Columns[3].ColumnName = "Branch Name";
            Dt.Columns[4].ColumnName = "Group Name";
            Dt.Columns[5].ColumnName = "Phones";
            Dt.Columns[6].ColumnName = "Pancard";
            Dt.Columns[7].ColumnName = "Bank details";
            Dt.Columns[8].ColumnName = "Dp Account";
            Dt.Columns[9].ColumnName = "Email";
            Dt.Columns[10].ColumnName = "Address";

            Dt.Columns[11].ColumnName = "NSE-CM";
            Dt.Columns[12].ColumnName = "NSE-FO";
            Dt.Columns[13].ColumnName = "NSE-CDX";
            Dt.Columns[14].ColumnName = "BSE-CM";
            /////////////////////////////////
            Dt.Columns[15].ColumnName = "BSE-FO";
            Dt.Columns[16].ColumnName = "MCXSX-CDX";
            Dt.Columns[17].ColumnName = "MCX-COMM";
            Dt.Columns[18].ColumnName = "NCDEX-COMM";
            Dt.Columns[19].ColumnName = "NMCE-COMM";
            Dt.Columns[20].ColumnName = "USE-CDX";
            Dt.Columns[21].ColumnName = "ACCOUNTS";

            Dt.Columns[22].ColumnName = "NSEL-SPOT";
            Dt.Columns[23].ColumnName = "MCXSX-CM";
            Dt.Columns[24].ColumnName = "MCXSX-FO";

            DataColumn dcolSerialNo = Dt.Columns.Add("Srl No", System.Type.GetType("System.String"));
            dcolSerialNo.SetOrdinal(0);
            //Dt.Columns["Srl No"].AutoIncrement = true;
            //Dt.Columns["Srl No"].AutoIncrementSeed = 1;
            //Dt.Columns["Srl No"].AutoIncrementStep = 1;
            for (int i = 1; i <= Dt.Rows.Count; i++)
            {
                Dt.Rows[i - 1]["Srl No"] = i;
            }

            for (int k = 0; k < Dt.Rows.Count; k++)
            {

                nsecm += Dt.Rows[k]["NSE-CM"].ToString();
                bsecm += Dt.Rows[k]["BSE-CM"].ToString();
                nsefo += Dt.Rows[k]["NSE-FO"].ToString();
                nsecdx += Dt.Rows[k]["NSE-CDX"].ToString();
                bsefo += Dt.Rows[k]["BSE-FO"].ToString();
                accounts += Dt.Rows[k]["ACCOUNTS"].ToString();
                mcxcdx += Dt.Rows[k]["MCXSX-CDX"].ToString();
                ncdexcomm += Dt.Rows[k]["NCDEX-COMM"].ToString();
                mcxcomm += Dt.Rows[k]["MCX-COMM"].ToString();
                nmcecomm += Dt.Rows[k]["NMCE-COMM"].ToString();
                usecdx += Dt.Rows[k]["USE-CDX"].ToString();

                spot += Dt.Rows[k]["NSEL-SPOT"].ToString();
                mcxsxcm += Dt.Rows[k]["MCXSX-CM"].ToString();
                mcxsxfo += Dt.Rows[k]["MCXSX-FO"].ToString();
            }
            if (ddlExport.SelectedItem.Value == "E")
            {
                if (mcxsxfo == "")
                {
                    Dt.Columns.Remove("MCXSX-FO");
                }
                if (mcxsxcm == "")
                {
                    Dt.Columns.Remove("MCXSX-CM");
                }
                if (spot == "")
                {
                    Dt.Columns.Remove("NSEL-SPOT");
                }


                if (accounts == "")
                {
                    Dt.Columns.Remove("ACCOUNTS");

                }
                if (usecdx == "")
                {
                    Dt.Columns.Remove("USE-CDX");
                }
                if (nmcecomm == "")
                {
                    Dt.Columns.Remove("NMCE-COMM");
                }
                if (ncdexcomm == "")
                {
                    Dt.Columns.Remove("NCDEX-COMM");
                }
                if (mcxcomm == "")
                {
                    Dt.Columns.Remove("MCX-COMM");
                }
                if (mcxcdx == "")
                {
                    Dt.Columns.Remove("MCXSX-CDX");
                }
                if (bsefo == "")
                {
                    Dt.Columns.Remove("BSE-FO");
                }
                if (bsecm == "")
                {
                    Dt.Columns.Remove("BSE-CM");
                }
                if (nsecdx == "")
                {
                    Dt.Columns.Remove("NSE-CDX");
                }
                if (nsefo == "")
                {
                    Dt.Columns.Remove("NSE-FO");
                }
                if (nsecm == "")
                {
                    Dt.Columns.Remove("NSE-CM");
                }


            }
            //DataRow dr = Dt.NewRow();
            //DataRow dr1 = Dt.NewRow();
            //Dt.Rows.InsertAt(dr, 0);
            // Dt.Rows.InsertAt(dr1, 0);

            string strSegs = "";
            if (rdbSegAll.Checked == true)
                strSegs = "All";
            else
                strSegs = "[" + litSegment.InnerText + "]";

            string strReportHeader = "Clients " + drpType.SelectedItem.Text + " Between  " + oconverter.ArrangeDate2(dtFrom.Value.ToString()) + "  to " + oconverter.ArrangeDate2(dtTo.Value.ToString()) + " With Company " + txtCompany.Text + " and Segment(s) " + strSegs;


            // string strReportHeader = drpType.SelectedItem.Text + " Clients of Period : " + " [" + oconverter.ArrangeDate2(dtFrom.Value.ToString()) + "  - " + oconverter.ArrangeDate2(dtTo.Value.ToString()) + "]";
            // Dt.Rows[0][0] = strReportHeader;
            Dt.AcceptChanges();
            string[] ColumnType = { "V", "V", "V", "V", "V", "V", "V", "V", "V", "V", "V", "V", "V", "V", "V", "V", "V", "V", "V", "V", "V", "V", "V", "V", "V" };
            string[] ColumnSize = { "15", "150", "50", "100", "100", "200", "200", "200", "200", "70", "250", "100", "100", "100", "100", "100", "100", "100", "100", "100", "100", "100", "100", "100", "100" };
            string[] ColumnWidthSize = { "20", "25", "25", "15", "15", "15", "15", "30", "15", "25", "30", "10", "10", "10", "10", "10", "10", "10", "10", "10", "10", "10", "10", "10", "10" };
            //string FileName = drpType.SelectedItem.Text + "_Clients_" + oDBEngine.GetDate().ToString("yyyyMMddHHmmss");
            string exlDateTime = oDBEngine.GetDate(113).ToString();
            string exlTime = exlDateTime.Replace(":", "");
            exlTime = exlTime.Replace(" ", "");
            string FileName = drpType.SelectedItem.Text + "_Clients_" + exlTime;
            strDownloadFileName = "~/Documents/";
            //string[] strHead ={ strReportHeader };
            string[] strHead = new string[3];
            strHead[0] = exlDateTime;
            //strHead[1] = CompanyName.Rows[0][0].ToString() + " - " + HttpContext.Current.Session["Segmentname"].ToString();
            strHead[1] = strReportHeader;
            strHead[2] = "Register / Suspended Client";

            oGenericExcelExport.ExportToExcel(ColumnType, ColumnSize, ColumnWidthSize, Dt, Server.MapPath(strDownloadFileName), ExcelVersion, FileName, strHead, null);


        }
    }
}