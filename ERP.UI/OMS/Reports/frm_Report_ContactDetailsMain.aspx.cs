using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using CrystalDecisions.CrystalReports.Engine;

namespace ERP.OMS.Reports
{
    public partial class Reports_frm_Report_ContactDetailsMain : System.Web.UI.Page, System.Web.UI.ICallbackEventHandler
    {
        DataTable dtClients = new DataTable();
        string data;
        BusinessLogicLayer.Converter OConvert = new BusinessLogicLayer.Converter();
        BusinessLogicLayer.Reports OReports = new BusinessLogicLayer.Reports();
        BusinessLogicLayer.Converter oconverter = new BusinessLogicLayer.Converter();
        DataSet ds = new DataSet();
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        int pageindex = 0;
        DataTable dtgrp = new DataTable();
        int PageNo = 0;
        DataTable DtMain = new DataTable();
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
            Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script language='javascript'>Page_Load();</script>");

            dtFrom.EditFormatString = OConvert.GetDateFormat("Date");
            dtTo.EditFormatString = OConvert.GetDateFormat("Date");
            if (!IsPostBack)
            {

                dtFrom.Value = Convert.ToDateTime(DateTime.Today);
                dtTo.Value = Convert.ToDateTime(DateTime.Today);


                settno();

            }
            //_____For performing operation without refreshing page___//

            String cbReference = Page.ClientScript.GetCallbackEventReference(this, "arg", "ReceiveServerData", "context");
            String callbackScript = "function CallServer(arg, context){ " + cbReference + ";}";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "CallServer", callbackScript, true);
            //___________-end here___//
            //Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script language='javascript'>divscroll('" + HttpContext.Current.Session["ExchangeSegmentID"] + "');</script>");


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
                    data = "Segment~" + str;
                }

                else if (idlist[0] == "Clients")
                {
                    data = "Clients~" + str;
                }
                else if (idlist[0] == "Group")
                {
                    data = "Group~" + str;
                }
            }
        }
        protected void btnhide_Click(object sender, EventArgs e)
        {
            if (ddlGroup.SelectedItem.Value.ToString() == "1")
            {
                if (HiddenField_Group.Value.ToString().Trim() == "")
                {
                    BindGroup();
                }
            }
        }

        void settno()
        {
            DataTable DtSegComp = oDBEngine.GetDataTable("(select exch_compId,exch_internalId,isnull((select exh_shortName from tbl_master_Exchange where exh_cntId=tbl_master_companyExchange.exch_exchId)+'-'+exch_segmentId,exch_membershiptype) as Comp from tbl_master_companyExchange where exch_compid='" + Session["LastCompany"].ToString() + "') as D", "*", "Comp in(select seg_name from tbl_master_segment where seg_id=" + Session["userlastsegment"].ToString() + ")");
            if (DtSegComp.Rows.Count > 0)
            {
                litSegment.InnerText = DtSegComp.Rows[0][2].ToString(); ///Segment disply within braket
                HiddenField_Segment.Value = DtSegComp.Rows[0][1].ToString();

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
                //ddlgrouptype.Items.Insert(0, new ListItem("Select GroupType", "0"));
                DtGroup.Dispose();

            }
        }

        public void CreateDataTable()
        {
            string segment = string.Empty;
            string IsBranchGroup = string.Empty;
            string BranchGroupValue = string.Empty;
            string RPTTYPE = string.Empty;
            string CLIENTS = string.Empty;
            if (rdbSegAll.Checked)
            {
                segment = Convert.ToString(Session["usersegid"]);
            }
            else
            {
                segment = Convert.ToString(HiddenField_Segment.Value);

            }


            if (ddlGroup.SelectedItem.Value.ToString() == "0")
            {
                IsBranchGroup = "Branch";
                if (rdbranchAll.Checked)
                {
                    //Query1 = "select cnt_internalid from tbl_master_contact  where cnt_internalid like 'CL%'";
                    BranchGroupValue = "BranchALL";
                }
                else
                {
                    BranchGroupValue = Convert.ToString(HiddenField_Branch.Value);
                    //Query1 = "select cnt_internalid from tbl_master_contact  where cnt_internalid like 'CL%' and cnt_branchid in (" + HiddenField_Branch.Value.ToString().Trim() + ")";
                }
            }
            else
            {
                IsBranchGroup = "Group";

                if (rdddlgrouptypeAll.Checked)
                {

                    //  Query1 = "select cnt_internalid from tbl_master_contact  where cnt_internalid like 'CL%'";
                    BranchGroupValue = "GroupAll";
                }
                else
                {

                    // Query1 = "select grp_contactid from tbl_trans_group  where grp_contactid like 'CL%' and grp_groupmaster in (" + HiddenField_Group.Value.ToString().Trim() + ")";
                    BranchGroupValue = Convert.ToString(HiddenField_Group.Value);
                }
            }


            if (ddllist.SelectedItem.Value.ToString() == "0")
            {
                RPTTYPE = "SHOW";
            }
            else
            {
                RPTTYPE = "PRINT";
            }
            if (rdbClientALL.Checked)
            {
                CLIENTS = "ALL";
            }
            else
            {
                CLIENTS = Convert.ToString(HiddenField_Client.Value);
            }
            ds = OReports.Fetch_ClientMaster(
                    Convert.ToString(Session["LastCompany"]),
                    segment,
                   IsBranchGroup,
                   BranchGroupValue,
                    Convert.ToString(dtFrom.Value),
                    Convert.ToString(dtTo.Value),
                   RPTTYPE,
                   CLIENTS

                );

            ViewState["dataset"] = ds;
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ddllist.SelectedItem.Value.ToString() == "0")
                {
                    fillHTML();
                }

                //if (ddllist.SelectedItem.Value.ToString() == "0")
                //{
                //    fillHTML();
                //}
                //else
                //{
                //    print();
                //}

            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "NORECORD", "NORECORD();", true);
            }





        }


        protected void btnshow_Click(object sender, EventArgs e)
        {


            CreateDataTable();
            this.Page.ClientScript.RegisterStartupScript(GetType(), "height123", "<script>height();</script>");



        }



        public void fillHTML()
        {
            ds = (DataSet)ViewState["dataset"];
            String strHtml = String.Empty;

            strHtml = "<table width=\"2000px\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";
            strHtml += "<tr style=\"background-color: #DBEEF3;\">";
            strHtml += "<td align=\"center\" ><b>Name</b></td>";
            strHtml += "<td align=\"center\" ><b>Branch</b></td>";
            strHtml += "<td align=\"center\" ><b>Category</b></td>";
            strHtml += "<td align=\"center\" ><b>Email</b></td>";
            strHtml += "<td align=\"center\" ><b>Telephone</b></td>";
            strHtml += "<td align=\"center\"  ><b>Mobile</b></td>";
            strHtml += "<td align=\"center\" ><b>Address</b></td>";
            strHtml += "<td align=\"center\"><b>PAN</b></td>";
            strHtml += "<td align=\"center\" ><b>VoterID</b></td>";
            strHtml += "<td align=\"center\" ><b>Ration</b></td>";
            strHtml += "<td align=\"center\" ><b>Passport</b></td>";
            strHtml += "<td align=\"center\" width=\"20%\"><b>Bank Details</b></td>";
            strHtml += "<td align=\"center\" width=\"20%\"><b>DP Details</b></td>";
            strHtml += "<td align=\"center\" ><b>Group</b></td>";
            strHtml += "<td align=\"center\" ><b>Subbroker</b></td>";
            strHtml += "<td align=\"center\"><b>Relationship Partner</b></td>";
            strHtml += "</tr>";



            int flag = 0;

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                flag = flag + 1;
                strHtml += "<tr valign=top id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
                //strHtml += "<td align=\"left\" >&nbsp;" + ds.Tables[0].Rows[i][].ToString() + "</td>";
                //  strHtml += "<td align=\"left\" style=\"font-size:xx-small;\" >&nbsp;" + ds.Tables[0].Rows[i][1].ToString() + "</td>";
                strHtml += "<td align=\"left\" style=\"font-size:xx-small;\" >&nbsp;" + ds.Tables[0].Rows[i][2].ToString() + "</td>";
                strHtml += "<td align=\"left\" style=\"font-size:xx-small;\" >&nbsp;" + ds.Tables[0].Rows[i][3].ToString() + "</td>";
                strHtml += "<td align=\"left\" style=\"font-size:xx-small;\" >&nbsp;" + ds.Tables[0].Rows[i][4].ToString() + "</td>";
                strHtml += "<td align=\"left\" style=\"font-size:xx-small;\" >&nbsp;" + ds.Tables[0].Rows[i][5].ToString() + "</td>";
                strHtml += "<td align=\"left\" style=\"font-size:xx-small;\" >&nbsp;" + ds.Tables[0].Rows[i][6].ToString() + "</td>";
                strHtml += "<td align=\"left\" style=\"font-size:xx-small;\" >&nbsp;" + ds.Tables[0].Rows[i][7].ToString() + "</td>";
                strHtml += "<td align=\"left\" style=\"font-size:xx-small;\" >&nbsp;" + ds.Tables[0].Rows[i][8].ToString() + "</td>";
                strHtml += "<td align=\"left\" style=\"font-size:xx-small;\" >&nbsp;" + ds.Tables[0].Rows[i][9].ToString() + "</td>";
                strHtml += "<td align=\"left\" style=\"font-size:xx-small;\" >&nbsp;" + ds.Tables[0].Rows[i][10].ToString() + "</td>";
                strHtml += "<td align=\"left\" style=\"font-size:xx-small;\" >&nbsp;" + ds.Tables[0].Rows[i][11].ToString() + "</td>";
                strHtml += "<td align=\"left\" style=\"font-size:xx-small;\" >&nbsp;" + ds.Tables[0].Rows[i][12].ToString() + "</td>";
                strHtml += "<td align=\"left\" style=\"font-size:xx-small;\" >&nbsp;" + ds.Tables[0].Rows[i][13].ToString() + "</td>";
                strHtml += "<td align=\"left\" style=\"font-size:xx-small;\" >&nbsp;" + ds.Tables[0].Rows[i][14].ToString() + "</td>";
                strHtml += "<td align=\"left\" style=\"font-size:xx-small;\" >&nbsp;" + ds.Tables[0].Rows[i][15].ToString() + "</td>";
                strHtml += "<td align=\"left\" style=\"font-size:xx-small;\" >&nbsp;" + ds.Tables[0].Rows[i][16].ToString() + "</td>";
                strHtml += "<td align=\"left\" style=\"font-size:xx-small;\" >&nbsp;" + ds.Tables[0].Rows[i][17].ToString() + "</td>";
                strHtml += "</tr>";

                /////////BANK DETAILS
                DataView view1 = new DataView();
                view1 = ds.Tables[1].DefaultView;
                view1.RowFilter = "ChargeGroupMembers_CustomerID='" + ds.Tables[0].Rows[i]["ClientID"].ToString().Trim() + "'";

                DataTable dt2 = new DataTable();
                dt2 = view1.ToTable();
                strHtml += "<tr>  <td colspan=\"16\" align=\"left\">";
                strHtml += "<table width=\"600px\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";
                strHtml += "<tr valign=top id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
                strHtml += "<td align=\"center\" ><b>Segment</b></td>";
                strHtml += "<td align=\"center\" ><b>TCODE</b></td>";
                strHtml += "<td align=\"center\" ><b>Registration Date</b></td>";
                strHtml += "<td align=\"center\" ><b>Brokerage</b></td>";
                strHtml += "<td align=\"center\"  ><b>Stump Duty</b></td>";
                strHtml += "<td align=\"center\" ><b>Demat Charges</b></td>";
                strHtml += "<td align=\"center\"><b>SEBI Fee</b></td>";
                strHtml += "<td align=\"center\" ><b>STT</b></td>";
                strHtml += "<td align=\"center\" ><b>Transaction Charges</b></td>";
                strHtml += "<td align=\"center\" ><b>Service Tax</b></td>";
                strHtml += "</tr>";

                for (int k = 0; k < dt2.Rows.Count; k++)
                {
                    strHtml += "<tr>";

                    strHtml += "<td align=\"left\" style=\"font-size:xx-small;\" >" + dt2.Rows[k]["crg_exchange"].ToString() + "</td>";
                    strHtml += "<td align=\"left\" style=\"font-size:xx-small;\" >" + dt2.Rows[k]["crg_tcode"].ToString() + "</td>";

                    strHtml += "<td align=\"left\" style=\"font-size:xx-small;\" >" + dt2.Rows[k]["crg_regisDate1"].ToString() + "</td>";
                    strHtml += "<td align=\"left\" style=\"font-size:xx-small;\" >" + dt2.Rows[k]["Brok"].ToString() + "</td>";
                    strHtml += "<td align=\"left\" style=\"font-size:xx-small;\" >" + dt2.Rows[k]["StampDuty"].ToString() + "</td>";
                    strHtml += "<td align=\"left\" style=\"font-size:xx-small;\" >" + dt2.Rows[k]["DematCharges"].ToString() + "</td>";
                    strHtml += "<td align=\"left\" style=\"font-size:xx-small;\" >" + dt2.Rows[k]["SEBIFee"].ToString() + "</td>";
                    strHtml += "<td align=\"left\" style=\"font-size:xx-small;\" >" + dt2.Rows[k]["STT"].ToString() + "</td>";
                    strHtml += "<td align=\"left\" style=\"font-size:xx-small;\" >" + dt2.Rows[k]["TransactionCharge"].ToString() + "</td>";
                    strHtml += "<td align=\"left\" style=\"font-size:xx-small;\" >" + dt2.Rows[k]["ServiceTax"].ToString() + "</td>";


                    strHtml += "</tr>";

                }

                strHtml += "</table></td></tr>";

            }
            strHtml += "</table>";


            display.InnerHtml = strHtml;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Display", "Display();", true);

        }

        protected string GetRowColor(int i)
        {
            if (i++ % 2 == 0)
                return "#fff0f5";
            else
                return "White";
        }





        void print()
        {
            ds = (DataSet)ViewState["dataset"];
            byte[] logoinByte;
            ds.Tables[0].Columns.Add("Image", System.Type.GetType("System.Byte[]"));
            if (oconverter.getLogoImage(HttpContext.Current.Server.MapPath(@"..\images\logo.bmp"), out logoinByte) != 1)
            {
                ScriptManager.RegisterStartupScript(HttpContext.Current.Handler as Page, HttpContext.Current.GetType(), "logo", "alert('Logo not Found.');", true);
            }
            else
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    ds.Tables[0].Rows[i]["Image"] = logoinByte;

                }
            }
            ReportDocument report = new ReportDocument();
            //ds.Tables[0].WriteXmlSchema("E:\\RPTXSD\\ClientMasterMain.xsd");
            //ds.Tables[1].WriteXmlSchema("E:\\RPTXSD\\ClientMasterMainDetail.xsd");

            report.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Landscape;
            string tmpPdfPath = string.Empty;
            tmpPdfPath = HttpContext.Current.Server.MapPath("..\\Reports\\ClientMasterMain.rpt");
            report.Load(tmpPdfPath);
            report.SetDataSource(ds.Tables[0]);
            report.Subreports["ClientMasterDetails"].SetDataSource(ds.Tables[1]);
            //report.Subreports["dpdetails"].SetDataSource(ds.Tables[2]);
            report.VerifyDatabase();
            report.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.Excel, HttpContext.Current.Response, true, "Client Master Main");

            report.Dispose();
            GC.Collect();
        }

        protected void btnprint_Click(object sender, EventArgs e)
        {

            CreateDataTable();
            print();
            this.Page.ClientScript.RegisterStartupScript(GetType(), "height123", "<script>height();</script>");
        }




    }
}