using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ERP.OMS.Reports
{
    public partial class Reports_contractregisteriframe : System.Web.UI.Page, System.Web.UI.ICallbackEventHandler
    {
        int pageindex = 0;
        int pagecount = 0;
        int pageRow = 0;
        int pageSize;
        int rowcount = 0;
        BusinessLogicLayer.Converter oconverter = new BusinessLogicLayer.Converter();
        BusinessLogicLayer.Reports oReports = new BusinessLogicLayer.Reports();
        BusinessLogicLayer.DBEngine oDbEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        static DataSet ds = new DataSet();
        ExcelFile objExcel = new ExcelFile();

        static string Group;
        static string Clients;
        static string Branch;
        string data;
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //'http://localhost:2957/InfluxCRM/management/testProjectMainPage_employee.aspx'
                string sPath = HttpContext.Current.Request.Url.ToString();
                oDBEngine.Call_CheckPageaccessebility(sPath);
            }
        }
        public string sortExpression
        {
            get
            {
                if (this.ViewState["sortExpression"] == null)
                    return "ContractNotes_TradeDate";

                else
                    return Convert.ToString(this.ViewState["sortExpression"]);



            }
            set
            {

                this.ViewState["sortExpression"] = value;


            }
        }

        public string direction
        {
            get
            {
                if (this.ViewState["direction"] == null)
                    return " ASC";

                else
                    return Convert.ToString(this.ViewState["direction"]);

            }
            set
            {
                this.ViewState["direction"] = value;

            }

        }


        protected void Page_Load(object sender, EventArgs e)
        {


            Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script language='javascript'>Page_Load();</script>");

            String cbReference = Page.ClientScript.GetCallbackEventReference(this, "arg", "ReceiveServerData", "context");
            String callbackScript = "function CallServer(arg, context){ " + cbReference + ";}";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "CallServer", callbackScript, true);
            if (HttpContext.Current.Session["userid"] == null)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
            //this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");
            // ______________________________End Script____________________________//
            if (!IsPostBack)
            {

                string SettNumber = Session["LastSettNo"].ToString();
                string setlmntno = SettNumber.Substring(0, 7);
                string setlmnttype = SettNumber.Substring(7, 1);

                Clients = null;
                Branch = null;
                Group = null;
                date();


            }

        }
        void date()
        {
            dtFrom.EditFormatString = oconverter.GetDateFormat("Date");
            dtTo.EditFormatString = oconverter.GetDateFormat("Date");
            string[] idlist;
            if (HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "1")
            {
                string[,] DtStartEnddate = oDbEngine.GetFieldValue("Master_Settlements", "cast(Settlements_StartDateTime as varchar)", "settlements_Number='" + Session["LastSettNo"].ToString().Substring(0, 7) + "' and settlements_TypeSuffix='" + Session["LastSettNo"].ToString().Substring(7, 1) + "' and Settlements_ExchangeSegmentID='" + HttpContext.Current.Session["ExchangeSegmentID"] + "'", 1);
                if (DtStartEnddate[0, 0] != "n")
                {
                    idlist = DtStartEnddate[0, 0].ToString().Split(','); // fetch startdate and FundsPayin and End Date from Master_Settlements
                    dtFrom.Value = Convert.ToDateTime(idlist[0]);
                    dtTo.Value = Convert.ToDateTime(idlist[0]);
                }
            }
            else
            {
                idlist = oDBEngine.GetDate().GetDateTimeFormats();
                dtFrom.Value = Convert.ToDateTime(idlist[2]);
                dtTo.Value = Convert.ToDateTime(idlist[2]);
            }
        }

        public void BindGroup()
        {
            ddlgrouptype.Items.Clear();
            DataTable DtGroup = oDbEngine.GetDataTable("tbl_master_groupmaster", "distinct gpm_Type", null);
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
        void fn_Client()
        {
            if (rdbClientALL.Checked)//////////////////ALL CLIENT CHECK
            {
                Clients = null;
                if (ddlGroup.SelectedItem.Value.ToString() == "1")
                {
                    if (rdddlgrouptypeAll.Checked)
                    {
                        if (ddlgrouptype.SelectedItem.Value.ToString() != "0")
                        {
                            DataTable dtclient = new DataTable();
                            dtclient = oDbEngine.GetDataTable("tbl_master_contact", "cnt_internalID", " cnt_internalId like 'CL%' and cnt_internalid in(select grp_contactid from tbl_trans_group where  grp_groupmaster in(select gpm_id from tbl_master_groupmaster where gpm_type='" + ddlgrouptype.SelectedItem.Text.ToString() + "'))");
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
                        dtclient = oDbEngine.GetDataTable("tbl_master_contact", "cnt_internalID", " cnt_internalId like 'CL%' and cnt_internalid in(select grp_contactid from tbl_trans_group where grp_groupmaster in(" + Group + "))");
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
                        dtclient = oDbEngine.GetDataTable("tbl_master_contact", "cnt_internalID", " cnt_internalId like 'CL%'  and cnt_branchid in(" + Session["userbranchHierarchy"].ToString() + ")");
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
                        dtclient = oDbEngine.GetDataTable("tbl_master_contact", "cnt_internalID", " cnt_internalId like 'CL%' and cnt_branchid in(" + Branch + ")");
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
            else if (radPOAClient.Checked)//////////////////////ALL POA CLIENT CHECK
            {
                Clients = null;
                if (ddlGroup.SelectedItem.Value.ToString() == "1")
                {
                    if (rdddlgrouptypeAll.Checked)
                    {
                        if (ddlgrouptype.SelectedItem.Value.ToString() != "0")
                        {
                            DataTable dtclient = new DataTable();
                            dtclient = oDbEngine.GetDataTable("tbl_master_contact", "cnt_internalID", " cnt_internalId like 'CL%' and cnt_internalid in (select dpd_cntID from tbl_master_contactdpdetails where dpd_POA=1) and cnt_internalid in(select grp_contactid from tbl_trans_group where  grp_groupmaster in(select gpm_id from tbl_master_groupmaster where gpm_type='" + ddlgrouptype.SelectedItem.Text.ToString() + "'))");
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
                        dtclient = oDbEngine.GetDataTable("tbl_master_contact", "cnt_internalID", " cnt_internalId like 'CL%' and cnt_internalid in (select dpd_cntID from tbl_master_contactdpdetails where dpd_POA=1) and cnt_internalid in(select grp_contactid from tbl_trans_group where grp_groupmaster in(" + Group + "))");
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
                        dtclient = oDbEngine.GetDataTable("tbl_master_contact", "cnt_internalID", " cnt_internalId like 'CL%'  and cnt_internalid in (select dpd_cntID from tbl_master_contactdpdetails where dpd_POA=1) and cnt_branchid in(" + Session["userbranchHierarchy"].ToString() + ")");
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
                        dtclient = oDbEngine.GetDataTable("tbl_master_contact", "cnt_internalID", " cnt_internalId like 'CL%' and cnt_internalid in (select dpd_cntID from tbl_master_contactdpdetails where dpd_POA=1) and cnt_branchid in(" + Branch + ")");
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
        }
        protected void btn_show_Click(object sender, EventArgs e)
        {

            fn_Client();
            if (Clients == null)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "NoRecord()", true);
            }
            else
            {
                FillGrid();
            }
        }
        public void FillGrid()
        {

            ViewState["del"] = null;
            ViewState["sqr"] = null;
            ViewState["totalto"] = null;
            ViewState["totalbrkg"] = null;
            ViewState["trancharge"] = null;
            ViewState["stamp"] = null;
            ViewState["Total"] = null;
            ViewState["netamount"] = null;
            ViewState["Sebifee"] = null;
            ViewState["Deliverycharge"] = null;
            ViewState["Othercharge"] = null;
            ViewState["RoundAmount"] = null;
            ViewState["STTAmount"] = null;


            String conn = ConfigurationSettings.AppSettings["DBConnectionDefault"];

            ds = oReports.Sp_ContractRegister(
                Convert.ToString(dtFrom.Value),
                 Convert.ToString(dtTo.Value),
                 Convert.ToString(Clients),
                 Convert.ToString(Session["usersegid"]),
                 Convert.ToString(Session["LastCompany"])
                );

            if (ds.Tables[0].Rows.Count > 0)
            {


                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    if (ds.Tables[0].Rows[i]["ContractNotes_DelFutTO"] != DBNull.Value)
                        ds.Tables[0].Rows[i]["ContractNotes_DelFutTO"] = oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ds.Tables[0].Rows[i]["ContractNotes_DelFutTO"].ToString()));

                    if (ds.Tables[0].Rows[i]["ContractNotes_SqrOptPrmTO"] != DBNull.Value)
                        ds.Tables[0].Rows[i]["ContractNotes_SqrOptPrmTO"] = oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ds.Tables[0].Rows[i]["ContractNotes_SqrOptPrmTO"].ToString()));

                    if (ds.Tables[0].Rows[i]["ContractNotes_TotalTO"] != DBNull.Value)
                        ds.Tables[0].Rows[i]["ContractNotes_TotalTO"] = oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ds.Tables[0].Rows[i]["ContractNotes_TotalTO"].ToString()));

                    if (ds.Tables[0].Rows[i]["ContractNotes_TotalBrokerage"] != DBNull.Value)
                        ds.Tables[0].Rows[i]["ContractNotes_TotalBrokerage"] = oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ds.Tables[0].Rows[i]["ContractNotes_TotalBrokerage"].ToString()));

                    if (ds.Tables[0].Rows[i]["ContractNotes_TransactionCharges"] != DBNull.Value)
                        ds.Tables[0].Rows[i]["ContractNotes_TransactionCharges"] = oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ds.Tables[0].Rows[i]["ContractNotes_TransactionCharges"].ToString()));

                    if (ds.Tables[0].Rows[i]["ContractNotes_StampDuty"] != DBNull.Value)
                        ds.Tables[0].Rows[i]["ContractNotes_StampDuty"] = oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ds.Tables[0].Rows[i]["ContractNotes_StampDuty"].ToString()));

                    if (ds.Tables[0].Rows[i]["TotalTax"] != DBNull.Value)
                        ds.Tables[0].Rows[i]["TotalTax"] = oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ds.Tables[0].Rows[i]["TotalTax"].ToString()));

                    if (ds.Tables[0].Rows[i]["ContractNotes_NetAmount"] != DBNull.Value)
                        ds.Tables[0].Rows[i]["ContractNotes_NetAmount"] = oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ds.Tables[0].Rows[i]["ContractNotes_NetAmount"].ToString()));

                    if (ds.Tables[0].Rows[i]["ContractNotes_STTAmount"] != DBNull.Value)
                        ds.Tables[0].Rows[i]["ContractNotes_STTAmount"] = oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ds.Tables[0].Rows[i]["ContractNotes_STTAmount"].ToString()));

                }

                pageSize = 15;
                pagecount = ds.Tables[0].Rows.Count / pageSize + 1;
                TotalPages.Value = pagecount.ToString();
                grdContractRegister.PageIndex = pageindex;
                CurrentPage.Value = pageindex.ToString();
                if (pageindex <= 0)
                {
                    pageindex = 0;


                    ScriptManager.RegisterStartupScript(this, this.GetType(), "hide1", "Disable('P');", true);
                    // Page.ClientScript.RegisterStartupScript(GetType(), "hide", "<script language='javascript'>Disable('P');</script>");
                }
                if (pageindex >= int.Parse(TotalPages.Value.ToString()))
                {
                    pageindex = int.Parse(TotalPages.Value.ToString()) - 1;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "hide1", "Disable('N');", true);
                    // Page.ClientScript.RegisterStartupScript(GetType(), "hide", "<script language='javascript'>Disable('N');</script>");
                }
                if (pageindex >= (int.Parse(TotalPages.Value.ToString()) - 1))
                {
                    pageindex = int.Parse(TotalPages.Value.ToString()) - 1;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "hide1", "Disable('N');", true);
                    // Page.ClientScript.RegisterStartupScript(GetType(), "hide", "<script language='javascript'>Disable('N');</script>");
                }


                ViewState["del"] = Convert.ToDecimal(ds.Tables[1].Rows[0]["del"]);

                ViewState["sqr"] = Convert.ToDecimal(ds.Tables[1].Rows[0]["sqr"]);

                ViewState["totalto"] = Convert.ToDecimal(ds.Tables[1].Rows[0]["totalto"]);

                ViewState["totalbrkg"] = Convert.ToDecimal(ds.Tables[1].Rows[0]["totalbrkg"]);

                ViewState["trancharge"] = Convert.ToDecimal(ds.Tables[1].Rows[0]["trancharge"]);

                ViewState["stamp"] = Convert.ToDecimal(ds.Tables[1].Rows[0]["stamp"]);

                ViewState["Total"] = Convert.ToDecimal(ds.Tables[1].Rows[0]["Total"]);

                ViewState["netamount"] = Convert.ToDecimal(ds.Tables[1].Rows[0]["netamount"]);

                ViewState["STTAmount"] = Convert.ToDecimal(ds.Tables[1].Rows[0]["STTAmount"]);

                ViewState["Sebifee"] = Convert.ToDecimal(ds.Tables[1].Rows[0]["SEBIFee"]);
                ViewState["Deliverycharge"] = Convert.ToDecimal(ds.Tables[1].Rows[0]["DeliveryCharges"]);
                ViewState["Othercharge"] = Convert.ToDecimal(ds.Tables[1].Rows[0]["OtherCharges"]);
                ViewState["RoundAmount"] = Convert.ToDecimal(ds.Tables[1].Rows[0]["RoundAmount"]);

                /////////////////////////////DISPLAY DATE/////////////////////////////////////

                string SpanText = oconverter.ArrangeDate2(dtFrom.Value.ToString()) + "  - " + oconverter.ArrangeDate2(dtTo.Value.ToString());
                ScriptManager.RegisterStartupScript(this, this.GetType(), "JSct", "displaydate('" + SpanText + "')", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "allselected", "ALLSELECTED()();", true);

                ///////////////////////////////END//////////////////////////////////////////



                DataView dv2 = new DataView(ds.Tables[0]);

                dv2.Sort = sortExpression + direction;
                grdContractRegister.DataSource = dv2;
                grdContractRegister.DataBind();


                // ScriptManager.RegisterStartupScript(this, this.GetType(), "JSgriddiv", "Setdivwd();", true);
                divgrid.Attributes.Add("style", "width: " + hidScreenWd.Value + "px; overflow:scroll");


            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "hi", "NoRecord()", true);
            }


        }
        protected void NavigationLink_Click(Object sender, CommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "First":
                    pageindex = 0;
                    pageRow = 0;
                    break;
                case "Next":
                    pageindex = int.Parse(CurrentPage.Value) + 1;
                    pageRow = pageindex * 15;
                    break;
                case "Prev":
                    pageindex = int.Parse(CurrentPage.Value) - 1;
                    pageRow = pageindex * 15;
                    break;
                case "Last":
                    pageindex = int.Parse(TotalPages.Value) - 1;
                    pageRow = pageindex * 15;
                    break;
                default:
                    pageindex = int.Parse(e.CommandName.ToString());
                    break;
            }

            FillGrid();
        }
        protected void grdContractRegister_RowCreated(object sender, GridViewRowEventArgs e)
        {
            //string SettNumber = Session["LastSettNo"].ToString();
            string SettNumber = Convert.ToString(ds.Tables[0].Rows[0]["SETTNO"]);
            string setlmntno = SettNumber.Substring(0, 7);
            string setlmnttype = SettNumber.Substring(7, 1);

            string rowID = String.Empty;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                rowcount = Convert.ToInt32(e.Row.RowIndex) + pageRow;
                rowID = "row" + Convert.ToString(rowcount);
                e.Row.Attributes.Add("id", rowID);
                string contractno = Convert.ToString(ds.Tables[0].Rows[rowcount]["cntno"]);
                string customerid = Convert.ToString(ds.Tables[0].Rows[rowcount]["contractnotes_customerID"]);
                e.Row.Attributes.Add("onclick", "ChangeRowColor(" + "'" + rowID + "','" + ds.Tables[0].Rows.Count + "'" + ")");
                //string strJS = "'" + Clients.Replace("'", "") + "','ALL','" + setlmntno + "','" + setlmnttype + "','" + Session["usersegid"] + "','" + dtFrom.Value + "','" + dtTo.Value + "','" + contractno + "','outputtype'";
                string strJS = "'" + customerid + "','ALL','" + setlmntno + "','" + setlmnttype + "','" + Session["usersegid"] + "','" + dtFrom.Value + "','" + dtTo.Value + "','" + contractno + "','outputtype'";

                e.Row.Attributes.Add("onclick", "javascript:TradeRegisterIframe(" + strJS + ")");
                e.Row.Style.Add("cursor", "hand");
                e.Row.ToolTip = "Click to Show Trade Register!";

            }

        }
        protected void grdContractRegister_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.Footer)
            {
                Label lbldel_sum = (Label)e.Row.FindControl("lbldel_sum");
                Label lblsqr_sum = (Label)e.Row.FindControl("lblsqr_sum");
                Label lbltotalto_sum = (Label)e.Row.FindControl("lbltotalto_sum");
                Label lbltotalbrkg_sum = (Label)e.Row.FindControl("lbltotalbrkg_sum");
                Label lbltrancharge_sum = (Label)e.Row.FindControl("lbltrancharge_sum");
                Label lblstamp_sum = (Label)e.Row.FindControl("lblstamp_sum");
                Label lblTotal_sum = (Label)e.Row.FindControl("lblTotal_sum");
                Label lblnetamount_sum = (Label)e.Row.FindControl("lblnetamount_sum");
                Label lblStt_sum = (Label)e.Row.FindControl("lblStt_sum");
                Label lblCntNo = (Label)e.Row.FindControl("lblCntNo");


                lbldel_sum.Text = oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ViewState["del"].ToString()));
                lblsqr_sum.Text = oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(ViewState["sqr"].ToString()));
                lbltotalto_sum.Text = oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ViewState["totalto"].ToString()));
                lbltotalbrkg_sum.Text = oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(ViewState["totalbrkg"].ToString()));
                lbltrancharge_sum.Text = oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ViewState["trancharge"].ToString()));
                lblstamp_sum.Text = oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(ViewState["stamp"].ToString()));
                lblStt_sum.Text = oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(ViewState["STTAmount"].ToString()));
                lblTotal_sum.Text = oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ViewState["Total"].ToString()));
                lblnetamount_sum.Text = oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(ViewState["netamount"].ToString()));
                ((Label)e.Row.FindControl("lblSebifee_sum")).Text = oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(ViewState["Sebifee"].ToString()));
                ((Label)e.Row.FindControl("lblDeliveryCharge_sum")).Text = oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(ViewState["Deliverycharge"].ToString()));
                ((Label)e.Row.FindControl("lblOtherCharge_sum")).Text = oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(ViewState["Othercharge"].ToString()));
                ((Label)e.Row.FindControl("lblRoundamount_sum")).Text = oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(ViewState["RoundAmount"].ToString()));



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
                        if (idlist[0] == "Branch")
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


                if (idlist[0] == "Clients")
                {
                    Clients = str;
                    data = "Clients~" + str1;
                }
                else if (idlist[0] == "Group")
                {
                    Group = str;
                    data = "Group~" + str;
                }
                else if (idlist[0] == "Branch")
                {
                    Branch = str;
                    data = "Branch~" + str;
                }

            }
        }
        public SortDirection GridViewSortDirection
        {

            get
            {

                if (ViewState["sortDirection"] == null)

                    ViewState["sortDirection"] = SortDirection.Ascending;

                return (SortDirection)ViewState["sortDirection"];

            }

            set { ViewState["sortDirection"] = value; }

        }
        private void SortGridView(string sortExpression, string direction)
        {


            DataView dv = new DataView(ds.Tables[0]);
            dv.Sort = sortExpression + direction;
            grdContractRegister.DataSource = dv;
            grdContractRegister.DataBind();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "hide", "height();", true);
        }


        protected void grdContractRegister_Sorting(object sender, GridViewSortEventArgs e)
        {
            sortExpression = e.SortExpression;
            if (GridViewSortDirection == SortDirection.Ascending)
            {
                direction = " DESC";
                GridViewSortDirection = SortDirection.Descending;
                SortGridView(sortExpression, direction);
            }
            else
            {
                direction = " ASC";
                GridViewSortDirection = SortDirection.Ascending;
                SortGridView(sortExpression, direction);
            }
        }


        protected void ddlExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dtExport = new DataTable();

            dtExport.Columns.Add("Date", Type.GetType("System.String"));
            dtExport.Columns.Add("Sett No.", Type.GetType("System.String"));
            dtExport.Columns.Add("CntNo.", Type.GetType("System.String"));
            dtExport.Columns.Add("Name", Type.GetType("System.String"));
            dtExport.Columns.Add("UCC", Type.GetType("System.String"));
            dtExport.Columns.Add("Branch", Type.GetType("System.String"));
            dtExport.Columns.Add("Branch Code", Type.GetType("System.String"));
            dtExport.Columns.Add("Del To", Type.GetType("System.String"));
            dtExport.Columns.Add("Sqr To", Type.GetType("System.String"));
            dtExport.Columns.Add("Total To", Type.GetType("System.String"));
            dtExport.Columns.Add("Total Brkg", Type.GetType("System.String"));
            dtExport.Columns.Add("TranCharge", Type.GetType("System.String"));
            dtExport.Columns.Add("StampDuty", Type.GetType("System.String"));
            dtExport.Columns.Add("STT Tax", Type.GetType("System.String"));
            dtExport.Columns.Add("Sebi Fee", Type.GetType("System.String"));
            dtExport.Columns.Add("Delivery Charge", Type.GetType("System.String"));
            dtExport.Columns.Add("Other Charge", Type.GetType("System.String"));
            dtExport.Columns.Add("Srv Tax & Cess", Type.GetType("System.String"));
            dtExport.Columns.Add("Net Amount", Type.GetType("System.String"));
            dtExport.Columns.Add("Round Amount", Type.GetType("System.String"));
            dtExport.Columns.Add("Remarks", Type.GetType("System.String"));
            dtExport.Columns.Add("Cancel Reason", Type.GetType("System.String"));

            DataTable dt = new DataTable();
            dt = ds.Tables[0].Copy();

            foreach (DataRow dr in dt.Rows)
            {
                DataRow row = dtExport.NewRow();
                if (dr["ContractNotes_TradeDate"] != DBNull.Value)
                {
                    row["Date"] = Convert.ToDateTime(dr["ContractNotes_TradeDate"]).ToShortDateString();
                }
                if (dr["SETTNO"] != DBNull.Value)
                {
                    row["Sett No."] = dr["SETTNO"];
                }
                if (dr["ContractNotes_Number"] != DBNull.Value)
                {
                    row["CntNo."] = dr["ContractNotes_Number"];
                }
                if (dr["Name"] != DBNull.Value)
                {
                    row["Name"] = dr["Name"];
                }
                if (dr["UCC"] != DBNull.Value)
                {
                    row["UCC"] = dr["UCC"];
                }
                if (dr["Branch"] != DBNull.Value)
                {
                    row["Branch"] = dr["Branch"];
                }
                if (dr["branch_code"] != DBNull.Value)
                {
                    row["Branch Code"] = dr["branch_code"];
                }
                if (dr["ContractNotes_DelFutTO"] != DBNull.Value)
                {
                    row["Del To"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dr["ContractNotes_DelFutTO"]));
                }
                if (dr["ContractNotes_DelFutTO"] != DBNull.Value)
                {
                    row["Del To"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dr["ContractNotes_DelFutTO"]));
                }
                if (dr["ContractNotes_SqrOptPrmTO"] != DBNull.Value)
                {
                    row["Sqr To"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dr["ContractNotes_SqrOptPrmTO"]));
                }
                if (dr["ContractNotes_SqrOptPrmTO"] != DBNull.Value)
                {
                    row["Sqr To"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dr["ContractNotes_SqrOptPrmTO"]));
                }
                if (dr["ContractNotes_TotalTO"] != DBNull.Value)
                {
                    row["Total To"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dr["ContractNotes_TotalTO"]));
                }
                if (dr["ContractNotes_TotalBrokerage"] != DBNull.Value)
                {
                    row["Total Brkg"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dr["ContractNotes_TotalBrokerage"]));
                }
                if (dr["ContractNotes_TransactionCharges"] != DBNull.Value)
                {
                    row["TranCharge"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dr["ContractNotes_TransactionCharges"]));
                }
                if (dr["ContractNotes_StampDuty"] != DBNull.Value)
                {
                    row["StampDuty"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dr["ContractNotes_StampDuty"]));
                }
                if (dr["ContractNotes_STTAmount"] != DBNull.Value)
                {
                    row["STT Tax"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dr["ContractNotes_STTAmount"]));
                }
                if (dr["TotalTax"] != DBNull.Value)
                {
                    row["Srv Tax & Cess"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dr["TotalTax"]));
                }
                if (dr["ContractNotes_NetAmount"] != DBNull.Value)
                {
                    row["Net Amount"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dr["ContractNotes_NetAmount"]));
                }

                if (dr["ContractNotes_SEBIFee"] != DBNull.Value)
                {
                    row["Sebi Fee"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dr["ContractNotes_SEBIFee"]));
                }
                if (dr["ContractNotes_DeliveryCharges"] != DBNull.Value)
                {
                    row["Delivery Charge"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dr["ContractNotes_DeliveryCharges"]));
                }
                if (dr["ContractNotes_OtherCharges"] != DBNull.Value)
                {
                    row["Other Charge"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dr["ContractNotes_OtherCharges"]));
                }
                if (dr["ContractNotes_Remarks"] != DBNull.Value)
                {
                    row["Remarks"] = dr["ContractNotes_Remarks"];
                }
                if (dr["ContractNotes_CancellationReason"] != DBNull.Value)
                {
                    row["Cancel Reason"] = dr["ContractNotes_CancellationReason"];
                }
                if (dr["ContractNotes_RoundAmount"] != DBNull.Value)
                {
                    row["Round Amount"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dr["ContractNotes_RoundAmount"]));
                }


                dtExport.Rows.Add(row);
            }

            DataRow row1 = dtExport.NewRow();
            row1["Date"] = "Total";
            row1["Del To"] = oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ViewState["del"].ToString()));
            row1["Sqr To"] = oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ViewState["sqr"].ToString()));
            row1["Total To"] = oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ViewState["totalto"].ToString()));
            row1["Total Brkg"] = oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ViewState["totalbrkg"].ToString()));
            row1["TranCharge"] = oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ViewState["trancharge"].ToString()));
            row1["StampDuty"] = oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ViewState["stamp"].ToString()));
            row1["STT Tax"] = oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ViewState["STTAmount"].ToString()));
            row1["Srv Tax & Cess"] = oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ViewState["Total"].ToString()));
            row1["Net Amount"] = oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ViewState["netamount"].ToString()));
            row1["Sebi Fee"] = oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ViewState["Sebifee"].ToString()));
            row1["Delivery Charge"] = oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ViewState["Deliverycharge"].ToString()));
            row1["Other Charge"] = oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ViewState["Othercharge"].ToString()));
            row1["Round Amount"] = oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ViewState["RoundAmount"].ToString()));
            dtExport.Rows.Add(row1);

            DataTable CompanyName = oDbEngine.GetDataTable("tbl_master_company", "cmp_Name", " cmp_internalId='" + Session["LastCompany"].ToString() + "'");
            DataTable dtReportHeader = new DataTable();
            dtReportHeader.Columns.Add(new DataColumn("Header", typeof(String))); //0
            DataRow HeaderRow = dtReportHeader.NewRow();
            HeaderRow[0] = CompanyName.Rows[0][0].ToString();
            dtReportHeader.Rows.Add(HeaderRow);
            DataRow DrRowR1 = dtReportHeader.NewRow();
            DrRowR1[0] = "Contract Register [" + ds.Tables[2].Rows[0]["segmentname"].ToString() + "]" + "Period :" + oconverter.ArrangeDate2(dtFrom.Value.ToString()) + '-' + oconverter.ArrangeDate2(dtTo.Value.ToString());

            dtReportHeader.Rows.Add(DrRowR1);
            DataRow DrRowR2 = dtReportHeader.NewRow();
            dtReportHeader.Rows.Add(DrRowR2);
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


            if (ddlExport.SelectedItem.Value == "E")
            {
                objExcel.ExportToExcelforExcel(dtExport, "Contract Register", "Total", dtReportHeader, dtReportFooter);
            }
            else if (ddlExport.SelectedItem.Value == "P")
            {
                objExcel.ExportToPDF(dtExport, "Contract Register", "Total", dtReportHeader, dtReportFooter);
            }
        }

        protected void btnhide_Click(object sender, EventArgs e)
        {
            if (ddlGroup.SelectedItem.Value.ToString() == "1")
            {
                if (Group == null)
                {
                    BindGroup();
                }
            }
        }
    }
}