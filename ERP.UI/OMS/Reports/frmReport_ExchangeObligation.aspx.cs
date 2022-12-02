using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;
using BusinessLogicLayer;

namespace ERP.OMS.Reports
{
    public partial class Reports_frmReport_ExchangeObligation : System.Web.UI.Page, System.Web.UI.ICallbackEventHandler
    {
        DailyReports dailyrep = new DailyReports();
        BusinessLogicLayer.DBEngine oDbEngine = new BusinessLogicLayer.DBEngine();
        BusinessLogicLayer.Converter objConverter = new BusinessLogicLayer.Converter();
        ClsDropDownlistNameSpace.clsDropDownList clsdrp = new ClsDropDownlistNameSpace.clsDropDownList();
        //for email
        string data;
        static string SubClients = "";
        string tblEmail;
        DataTable dtEmail;
        string lblBuyvalues = "";
        string lblsellvalues = "";
        string lbltotals = "";
        string lbltotalcriterias = "";
        string lblNDs = "";
        string lblNDcriterias = "";
        string lblNets = "";
        string lblNetcriterias = "";
        string lblBuyvalued = "";
        string lblsellvalued = "";
        string lblNetd = "";
        string lblNDd = "";
        string lbltitles = "";

        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //'http://localhost:2957/InfluxCRM/management/testProjectMainPage_employee.aspx'
                string sPath = HttpContext.Current.Request.Url.ToString();
                oDbEngine.Call_CheckPageaccessebility(sPath);
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {

            if (HttpContext.Current.Session["userid"] == null)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
            //this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");
            if (!IsPostBack)
            {
                txtfromDate.Value = Convert.ToDateTime(oDbEngine.GetDate().Month.ToString() + "-" + "01" + "-" + oDbEngine.GetDate().Year.ToString());
                txtToDate.Value = Convert.ToDateTime(oDbEngine.GetDate());
                ViewState["row"] = "";

                //________This script is for firing javascript when page load first___//
                if (!ClientScript.IsStartupScriptRegistered("Today"))
                    ClientScript.RegisterStartupScript(typeof(Page), "Today", "<script>PageLoad();</script>");
                // ______________________________End Script____________________________//


                txtset.Text = Session["LastSettNo"].ToString();

            }
            else
            {
                if (rbSettlement.SelectedItem.Value == "S")
                {

                    gridbind();
                }
                else
                {

                    date_gridbind();
                }

            }
            txtset.Attributes.Add("onkeyup", "CallList(this,'SearchSettlementWithoutbracket',event)");
            //for sending email
            txtSelectionID.Attributes.Add("onkeyup", "callAjax1(this,'GetMailId',event)");
            String cbReference = Page.ClientScript.GetCallbackEventReference(this, "arg", "ReceiveServerData", "context");
            String callbackScript = "function CallServer(arg, context){ " + cbReference + ";}";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "CallServer", callbackScript, true);
            FillCombo();


        }
        //protected void Cmbexcel_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        //{
        //    Items.Clear();
        //    Cmbexcel.JSProperties["cpexcel"] = "";
        //    string WhichCall = e.Parameter.ToString();
        //    if (WhichCall == "Excel")
        //    Cmbexcel.JSProperties["cpexcel"] = "export";
        //}
        protected void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dtexcel = (DataTable)ViewState["exportdata"];
            ExcelFile objExcel = new ExcelFile();

            string searchCriteria = null;
            Converter oconverter = new Converter();
            DataTable dtcompany = oDbEngine.GetDataTable("select ltrim(rtrim(cmp_Name)) as company from tbl_master_company where cmp_internalid ='" + Session["LastCompany"].ToString() + "'");
            searchCriteria = dtcompany.Rows[0]["company"].ToString();// +oconverter.ArrangeDate2(dtFromDate.Value.ToString()) + " Report of   " + ddlGroupBy.SelectedItem.Value + " Wise";


            GenericExcelExport oGenericExcelExport = new GenericExcelExport();
            string strDownloadFileName = "";
            string exlDateTime = oDbEngine.GetDate(113).ToString();
            string exlTime = exlDateTime.Replace(":", "");
            exlTime = exlTime.Replace(" ", "");
            string FileName = "Exchange Obligation_" + exlTime;
            strDownloadFileName = "~/Documents/";

            string[] strHead = new string[3];
            strHead[0] = exlDateTime;
            strHead[1] = searchCriteria;
            strHead[2] = "Exchange Obligation For " + Session["SegmentName"].ToString().Trim() + " Of " + dtcompany.Rows[0]["company"];
            string ExcelVersion = "2007";
            if (rbSettlement.SelectedItem.Value == "S")
            {
                //Lots
                string[] ColumnType = { "V", "V", "I", "N", "I", "N", "N", "N" };
                string[] ColumnSize = { "30", "30", "10", "28,2", "10", "28,2", "28,2", "28,2" };
                string[] ColumnWidthSize = { "30", "30", "30", "30", "30", "30", "30", "30" };
                oGenericExcelExport.ExportToExcel(ColumnType, ColumnSize, ColumnWidthSize, dtexcel, Server.MapPath(strDownloadFileName), ExcelVersion, FileName, strHead, null);
            }
            else
            {
                //Lots
                string[] ColumnType = { "V", "N", "N", "N", "N", "V" };
                string[] ColumnSize = { "30", "28,2", "28,2", "28,2", "28,2", "15" };
                string[] ColumnWidthSize = { "30", "30", "30", "30", "30", "30" };
                oGenericExcelExport.ExportToExcel(ColumnType, ColumnSize, ColumnWidthSize, dtexcel, Server.MapPath(strDownloadFileName), ExcelVersion, FileName, strHead, null);
            }


        }
        protected void Settelment_Grid_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {

            if (e.Parameters == "s")
            {
                Settelment_Grid.Settings.ShowFilterRow = true;
            }

            if (e.Parameters == "All")
            {

                Settelment_Grid.FilterExpression = string.Empty;
            }
            if (e.Parameters == "grid")
            {

                gridbind();

            }
            if (e.Parameters == "empty")
            {
                ViewState["row"] = "";



                DataTable ds = getemptyDatatable();
                Settelment_Grid.DataSource = ds;
                Settelment_Grid.DataBind();

            }

        }
        private DataTable getemptyDatatable()
        {
            DataTable dtb = new DataTable();
            dtb.Columns.Add("productname");
            dtb.Columns.Add("series");
            dtb.Columns.Add("totalbuy");
            dtb.Columns.Add("buyvalue");
            dtb.Columns.Add("sell");
            dtb.Columns.Add("sellvalue");
            dtb.Columns.Add("netvalue");
            dtb.Columns.Add("obligance");
            return dtb;
        }

        decimal total = 0;
        protected void Settelment_Grid_HtmlRowCreated(object sender, DevExpress.Web.ASPxGridViewTableRowEventArgs e)
        {

            if (e.RowType == GridViewRowType.Data)
            {
                if (e.GetValue("totalbuy") != DBNull.Value)
                    e.Row.Cells[2].Text = e.GetValue("totalbuy").ToString();
                if (e.GetValue("buyvalue") != DBNull.Value)
                    e.Row.Cells[3].Text = objConverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(e.GetValue("buyvalue")));
                if (e.GetValue("sell") != DBNull.Value)
                    e.Row.Cells[4].Text = e.GetValue("sell").ToString();
                if (e.GetValue("sellvalue") != DBNull.Value)
                    e.Row.Cells[5].Text = objConverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(e.GetValue("sellvalue")));
                if (e.GetValue("netvalue") != DBNull.Value)
                    e.Row.Cells[6].Text = e.GetValue("netvalue").ToString();
                if (e.GetValue("obligance") != DBNull.Value)
                    e.Row.Cells[7].Text = objConverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(e.GetValue("obligance")));
            }


            if (e.RowType == GridViewRowType.Footer)
            {
                if (ViewState["row"].ToString() == "1")
                {
                    Label lbltotal = Settelment_Grid.FindFooterRowTemplateControl("lbltoltal") as Label;
                    Label lbltotalcriteria = Settelment_Grid.FindFooterRowTemplateControl("lbltotalcriteria") as Label;
                    Label lblND = Settelment_Grid.FindFooterRowTemplateControl("lblND") as Label;
                    Label lblNDcriteria = Settelment_Grid.FindFooterRowTemplateControl("lblNdCriteria") as Label;
                    Label lblNet = Settelment_Grid.FindFooterRowTemplateControl("lblObligance") as Label;
                    Label lblNetcriteria = Settelment_Grid.FindFooterRowTemplateControl("lbltotalnet") as Label;
                    Label lblBuyvalue = Settelment_Grid.FindFooterRowTemplateControl("lblbuyvalue") as Label;
                    Label lblsellvalue = Settelment_Grid.FindFooterRowTemplateControl("lblsellvalue") as Label;

                    try
                    {
                        string InternalID = dailyrep.exchange_obligationtotal(Convert.ToInt32(Session["usersegid"].ToString()), txtset.Text.ToString(),
                             ASPxComboBox1.SelectedItem.Text, Session["LastCompany"].ToString());

                        string[] idlist = InternalID.Split('~');


                        lbltotal.Text = "Total :" + objConverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(idlist[0]));

                        lbltotalcriteria.Text = "(" + idlist[1] + ")";
                        //-------For Email 
                        lbltotals = lbltotal.Text;
                        lbltotalcriterias = lbltotalcriteria.Text;
                        if (ASPxComboBox1.SelectedItem.Text == "Only Nd")
                        {
                            lblND.Text = "";
                            lblNDcriteria.Text = "";
                            lblNet.Text = "";
                            lblNetcriteria.Text = "";
                            //-------For Email 
                            lblNDs = "";
                            lblNDcriterias = "";
                            lblNets = "";
                            lblNetcriterias = "";
                        }
                        else
                        {
                            lblND.Text = "ND Obligation :" + objConverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(idlist[2]));
                            lblNDcriteria.Text = "(" + idlist[3] + ")";
                            lblNet.Text = "Net Obligation :" + objConverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(idlist[4]));
                            lblNetcriteria.Text = "(" + idlist[5] + ")";
                            //-------For Email 
                            lblNDs = lblND.Text;
                            lblNDcriterias = lblNDcriteria.Text;
                            lblNets = lblNet.Text;
                            lblNetcriterias = lblNetcriteria.Text;
                        }



                        lblBuyvalue.Text = objConverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(idlist[6]));
                        lblsellvalue.Text = objConverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(idlist[7]));
                        //-------For Email 
                        lblBuyvalues = lblBuyvalue.Text;
                        lblsellvalues = lblsellvalue.Text;
                    }
                    catch
                    {
                    }

                }

            }
        }
        protected void Settelment_Grid_DataBound(object sender, EventArgs e)
        {
            if (ViewState["row"].ToString() == "1")
            {
                Label lbltitle = Settelment_Grid.FindTitleTemplateControl("lblobligationtitle") as Label;
                lbltitle.Text = "Exchange Obligation For " + txtset.Text.ToString() + " [ " + ASPxComboBox1.SelectedItem.Text + " ]";
                lbltitles = lbltitle.Text;
            }
        }
        //protected void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    Int32 Filter = int.Parse(cmbExport.SelectedItem.Value.ToString());
        //    switch (Filter)
        //    {
        //        case 1:
        //            exporter.WritePdfToResponse();
        //            break;
        //        case 2:
        //            exporter.WriteXlsToResponse();
        //            break;
        //        case 3:
        //            exporter.WriteRtfToResponse();
        //            break;
        //        case 4:
        //            exporter.WriteCsvToResponse();
        //            break;
        //    }
        //}

        void gridbind()
        {

            DataTable dt = new DataTable();
            dt = dailyrep.exchange_obligation(Convert.ToInt32(Session["usersegid"].ToString()), txtset.Text.ToString(),
                             ASPxComboBox1.SelectedItem.Text, Session["LastCompany"].ToString());
            dtEmail = dt;

            Settelment_Grid.DataSource = dt;
            Settelment_Grid.DataBind();

            if (dt.Rows.Count == 0)
            {
                ViewState["row"] = "";
            }
            else
            {
                ViewState["row"] = "1";
                ViewState["exportdata"] = "";
                ViewState["exportdata"] = dt;

            }
        }
        void date_gridbind()
        {
            DataTable dt = new DataTable();
            dt = dailyrep.exchange_dateobligation(Convert.ToInt32(Session["usersegid"].ToString()), txtfromDate.Value.ToString(), txtToDate.Value.ToString(),
              ASPxComboBox1.SelectedItem.Text, HttpContext.Current.Session["ExchangeSegmentID"].ToString(), Session["LastCompany"].ToString());
            dtEmail = dt;

            Obligation_date.DataSource = dt;
            Obligation_date.DataBind();

            if (dt.Rows.Count == 0)
            {
                ViewState["row"] = "";
            }
            else
            {
                ViewState["exportdata"] = "";
                ViewState["exportdata"] = dt;
                ViewState["row"] = "1";

            }
        }

        protected void Obligation_date_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            if (e.Parameters == "s")

                Obligation_date.Settings.ShowFilterRow = true;

            if (e.Parameters == "All")
            {
                Obligation_date.FilterExpression = string.Empty;
            }

            if (e.Parameters == "dategrid")
            {
                date_gridbind();
            }

            if (e.Parameters == "empty")
            {
                ViewState["row"] = "";

                DataTable ds = getemptyDatatableDate();
                Obligation_date.DataSource = ds;
                Obligation_date.DataBind();
            }
        }

        private DataTable getemptyDatatableDate()
        {
            DataTable dtb = new DataTable();
            dtb.Columns.Add("settelmentno");
            dtb.Columns.Add("tradedate");
            dtb.Columns.Add("buyvalue");
            dtb.Columns.Add("sellvalue");
            dtb.Columns.Add("NdObligance");
            dtb.Columns.Add("obligance");

            return dtb;
        }

        protected void Obligation_date_DataBound(object sender, EventArgs e)
        {

            if (ViewState["row"].ToString() == "1")
            {
                Label lbltitle = Obligation_date.FindTitleTemplateControl("lbldateobligationtitle") as Label;
                lbltitle.Text = "Exchange Obligation For " + " ( " + txtfromDate.Text.ToString() + " ) " + "To" + " ( " + txtToDate.Text.ToString() + ")" + " [" + ASPxComboBox1.SelectedItem.Text + " ]";
                //lbltitle.Text = "aadsas";
                lbltitles = lbltitle.Text;
            }
        }
        protected void Obligation_date_HtmlRowCreated(object sender, ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType == GridViewRowType.Data)
            {

                if (e.GetValue("buyvalue") != DBNull.Value)
                    e.Row.Cells[2].Text = objConverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(e.GetValue("buyvalue")));
                if (e.GetValue("sellvalue") != DBNull.Value)
                    e.Row.Cells[3].Text = objConverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(e.GetValue("sellvalue")));
                if (e.GetValue("NdObligance") != DBNull.Value)
                    e.Row.Cells[4].Text = objConverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(e.GetValue("NdObligance")));
                if (e.GetValue("obligance") != DBNull.Value)
                    e.Row.Cells[5].Text = objConverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(e.GetValue("obligance")));

            }


            if (e.RowType == GridViewRowType.Footer)
            {
                if (ViewState["row"].ToString() == "1")
                {

                    Label lblND = Obligation_date.FindFooterRowTemplateControl("lblNDvaluedate") as Label;
                    Label lblNet = Obligation_date.FindFooterRowTemplateControl("lblnetvaluedate") as Label;
                    Label lblBuyvalue = Obligation_date.FindFooterRowTemplateControl("lblbuyvaluedate") as Label;
                    Label lblsellvalue = Obligation_date.FindFooterRowTemplateControl("lblsellvaluedate") as Label;

                    try
                    {

                        string InternalID = dailyrep.exchange_dateobligationtotal(Convert.ToInt32(Session["usersegid"].ToString()), txtfromDate.Value.ToString(),
                             txtToDate.Value.ToString(), ASPxComboBox1.SelectedItem.Text, HttpContext.Current.Session["ExchangeSegmentID"].ToString(), Session["LastCompany"].ToString());
                        //lcon.Close();

                        string[] idlist = InternalID.Split('~');

                        lblBuyvalue.Text = objConverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(idlist[0]));
                        lblsellvalue.Text = objConverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(idlist[1]));
                        lblNet.Text = objConverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(idlist[2]));
                        lblND.Text = objConverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(idlist[3]));
                        //---for Email
                        lblBuyvalued = lblBuyvalue.Text;
                        lblsellvalued = lblsellvalue.Text;
                        lblNetd = lblNet.Text;
                        lblNDd = lblND.Text;

                    }
                    catch
                    {
                    }
                }
            }
        }
        protected void Settelment_Grid_CustomJSProperties(object sender, ASPxGridViewClientJSPropertiesEventArgs e)
        {
            e.Properties["cpHeight"] = "a";
            //  e.Properties["cpEND"] = "2";
        }
        protected void Obligation_date_CustomJSProperties(object sender, ASPxGridViewClientJSPropertiesEventArgs e)
        {
            e.Properties["cp1Height"] = "a";
            // e.Properties["cpEND"] = "2";
        }


        //--------------------Region for sending email---------------

        private void FillCombo()
        {
            if (HttpContext.Current.Session["userlastsegment"].ToString() == "4")
            {
                string[,] r = new string[1, 2];
                r[0, 0] = "EM";
                r[0, 1] = "Employees";
                clsdrp.AddDataToDropDownList(r, cmbsearchOption);
            }
            else
            {
                string[,] r = new string[1, 2];
                r[0, 0] = "EM";
                r[0, 1] = "Employees";
                //r[1, 0] = "CL";
                //r[1, 1] = "Customers";
                //r[2, 0] = "LD";
                //r[2, 1] = "Lead";
                //r[3, 0] = "CD";
                //r[3, 1] = "CDSL Client";
                //r[4, 0] = "ND";
                //r[4, 1] = "NSDL Client";
                //r[5, 0] = "BP";
                //r[5, 1] = "Business Partne";
                //r[6, 0] = "RA";
                //r[6, 1] = "Relationship Partner";
                //r[7, 0] = "SB";
                //r[7, 1] = "Sub Broker";
                //r[8, 0] = "FR";
                //r[8, 1] = "Franchisees";
                clsdrp.AddDataToDropDownList(r, cmbsearchOption);

            }

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
                    string[] val = cl[i].Split(';');
                    string[] AcVal = val[0].Split('-');
                    if (str == "")
                    {
                        str = AcVal[0];
                        str1 = "'" + AcVal[0] + "'" + ";" + val[1];
                    }
                    else
                    {
                        str += "," + AcVal[0];
                        str1 += "," + "'" + AcVal[0] + "'" + ";" + val[1];
                    }

                    SubClients = str;
                    data = str1;
                }
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "jscript3", "MailsendT();", true);
            }
        }


        string ICallbackEventHandler.GetCallbackResult()
        {


            return SubClients;
        }


        protected void Button1_Click(object sender, EventArgs e)
        {


            // this.Page.ClientScript.RegisterStartupScript(GetType(), "heightklk", "<script>bindrid();</script>");

            if (SubClients != "")
            {
                string[] clnt = SubClients.ToString().Split(',');
                int k = clnt.Length;
                for (int i = 0; i < clnt.Length; i++)
                {
                    string disptbl = "";
                    if (rbSettlement.SelectedItem.Value == "S")
                    {
                        gridbind();
                        DataTable dtTbl = dtEmail;
                        disptbl = "<table width=\"750px\" style=\"font-size:8pt;\" cellspacing=\"0\" cellpadding=\"0\" border=\"1\"><tr style=\"background-color:#BB694D;color:White;\"><td align=\"center\">Product</td><td align=\"center\">Series</td><td align=\"center\">Total Buy</td><td align=\"center\">Value</td><td align=\"center\">Total Sell</td><td align=\"center\">Value</td><td align=\"center\">Net Quantity</td><td align=\"center\">Obligation</td></tr>";

                        for (int j = 0; j < dtTbl.Rows.Count; j++)
                        {
                            string byvals = "";
                            string sellvalus = "";
                            string obligances = "";
                            //------------------
                            if (dtTbl.Rows[j]["buyvalue"] != DBNull.Value)
                            {
                                byvals = objConverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtTbl.Rows[j]["buyvalue"]));
                            }
                            else
                            {
                                byvals = "";
                            }
                            //------------------
                            if (dtTbl.Rows[j]["sellvalue"] != DBNull.Value)
                            {
                                sellvalus = objConverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtTbl.Rows[j]["sellvalue"]));
                            }
                            else
                            {
                                sellvalus = "";
                            }
                            //------------------
                            if (dtTbl.Rows[j]["obligance"] != DBNull.Value)
                            {
                                obligances = objConverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtTbl.Rows[j]["obligance"]));
                            }
                            else
                            {
                                obligances = "";
                            }
                            //------------------
                            disptbl += "<tr><td>&nbsp;" + dtTbl.Rows[j]["productname"] + "</td><td>&nbsp;" + dtTbl.Rows[j]["series"] + "</td><td align=\"right\">&nbsp;" + dtTbl.Rows[j]["totalbuy"] + "</td><td align=\"right\">&nbsp;" + byvals + "</td><td align=\"right\">&nbsp;" + dtTbl.Rows[j]["sell"] + "</td><td align=\"right\">&nbsp;" + sellvalus + "</td><td align=\"right\">&nbsp;" + dtTbl.Rows[j]["netvalue"] + "</td><td align=\"right\">&nbsp;" + obligances + "</td></tr>";

                        }

                        disptbl += "<tr style=\"background-color: #FFD4AA; color: Black;\"><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>" + lblBuyvalues + "</td><td>&nbsp;</td><td>" + lblsellvalues + "</td><td>&nbsp;</td><td>&nbsp;</td></tr>";
                        disptbl += "<tr style=\"background-color: #FFFFFF; color: Black;\"><td colspan=\"7\" align=\"right\">" + lbltotals + "</td><td align=\"right\">&nbsp;" + lbltotalcriterias + "</td></tr>";
                        disptbl += "<tr style=\"background-color: #FFFFFF; color: Black;\"><td colspan=\"7\" align=\"right\">" + lblNDs + "</td><td align=\"right\">&nbsp;" + lblNDcriterias + "</td></tr>";
                        disptbl += "<tr style=\"background-color: #FFFFFF; color: Black;\"><td colspan=\"7\" align=\"right\">" + lblNets + "</td><td align=\"right\">&nbsp;" + lblNetcriterias + "</td></tr>";
                        //  disptbl += "<tr style=\"background-color: #FFD4AA; color: Black;\"><td>Total</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td align=\"right\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(ViewState["BFVALUE_Sum"].ToString())) + "</td><td>&nbsp;</td><td align=\"right\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(ViewState["BUYVALUE_Sum"].ToString())) + "</td><td>&nbsp;</td><td align=\"right\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(ViewState["SELLVALUE_Sum"].ToString())) + "</td><td>&nbsp;</td><td>&nbsp;</td><td align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ViewState["CFVALUE_Sum"].ToString())) + "</td><td align=\"right\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(ViewState["PRM_Sum"].ToString())) + "</td><td align=\"right\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(ViewState["MTM_Sum"].ToString())) + "</td><td align=\"right\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(ViewState["FINSETT_Sum"].ToString())) + "</td><td align=\"right\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(ViewState["ExcAsn_Sum"].ToString())) + "</td><td align=\"right\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(ViewState["NETOBLIGATION_Sum"].ToString())) + "</td></tr>";
                        disptbl += "</table>";
                        this.Page.ClientScript.RegisterStartupScript(GetType(), "heightklk", "<script>ShowISettelmentFilterForm('S');</script>");
                        this.Page.ClientScript.RegisterStartupScript(GetType(), "jscript1", "<script>bindrid();</script>");
                        this.Page.ClientScript.RegisterStartupScript(GetType(), "jscript17", "<script>SndComplete();</script>");

                    }
                    else
                    {
                        date_gridbind(); ;
                        DataTable dtTbl = dtEmail;
                        disptbl = "<table width=\"750px\" style=\"font-size:8pt;\" cellspacing=\"0\" cellpadding=\"0\" border=\"1\"><tr style=\"background-color:#BB694D;color:White;\"><td align=\"center\">Settelment No</td><td align=\"center\">Trade Date</td><td align=\"center\">Buy Value</td><td align=\"center\">Sell Value</td><td align=\"center\">Nd Obligation</td><td align=\"center\">Obligation</td></tr>";

                        for (int j = 0; j < dtTbl.Rows.Count; j++)
                        {
                            string byval = "";
                            string sellvalu = "";
                            string obligance = "";
                            //------------------
                            if (dtTbl.Rows[j]["buyvalue"] != DBNull.Value)
                            {
                                byval = objConverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtTbl.Rows[j]["buyvalue"]));
                            }
                            else
                            {
                                byval = "";
                            }
                            //------------------
                            if (dtTbl.Rows[j]["sellvalue"] != DBNull.Value)
                            {
                                sellvalu = objConverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtTbl.Rows[j]["sellvalue"]));
                            }
                            else
                            {
                                sellvalu = "";
                            }
                            //------------------
                            if (dtTbl.Rows[j]["obligance"] != DBNull.Value)
                            {
                                obligance = objConverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtTbl.Rows[j]["obligance"]));
                            }
                            else
                            {
                                obligance = "";
                            }
                            //------------------

                            disptbl += "<tr><td >&nbsp;" + dtTbl.Rows[j]["settelmentno"] + "</td><td>&nbsp;" + dtTbl.Rows[j]["tradedate"] + "</td><td align=\"right\">&nbsp;" + byval + "</td><td align=\"right\">&nbsp;" + sellvalu + "</td><td align=\"right\">&nbsp;" + dtTbl.Rows[j]["NdObligance"] + "</td><td align=\"right\">&nbsp;" + obligance + "</td></tr>";

                        }
                        disptbl += "<tr style=\"background-color: #FFD4AA; color: Black;\"><td>&nbsp;</td><td>&nbsp;</td><td align=\"right\">&nbsp;" + lblBuyvalued + "</td><td align=\"right\">&nbsp;" + lblsellvalued + "</td><td align=\"right\">&nbsp;" + lblNDd + "</td><td align=\"right\">&nbsp;" + lblNetd + "</td></tr>";
                        disptbl += "</table>";


                        this.Page.ClientScript.RegisterStartupScript(GetType(), "heightklk", "<script>ShowISettelmentFilterForm('A');</script>");
                        this.Page.ClientScript.RegisterStartupScript(GetType(), "jscript1", "<script>bindrid();</script>");
                        this.Page.ClientScript.RegisterStartupScript(GetType(), "jscript13", "<script>SndComplete();</script>");

                    }
                    string emailbdy = disptbl;
                    string contactid = clnt[i].ToString();
                    // string billdate = oconverter.ArrangeDate2(dtdate.Value.ToString());
                    string billdate = DateTime.Today.ToString();
                    string Subject = lbltitles;
                    if (oDbEngine.SendReportSt(emailbdy, contactid, billdate, Subject) == true)
                    {

                        this.Page.ClientScript.RegisterStartupScript(GetType(), "jscript78", "<script>MailsendT();</script>");
                    }
                    else
                    {
                        this.Page.ClientScript.RegisterStartupScript(GetType(), "jscript45", "<script>MailsendF();</script>");
                    }
                }

            }
            else
            {

                this.Page.ClientScript.RegisterStartupScript(GetType(), "jscript70", "<script>MailsendFT();</script>");
            }

            this.Page.ClientScript.RegisterStartupScript(GetType(), "jscript58", "<script>height();</script>");
        }

    }
}