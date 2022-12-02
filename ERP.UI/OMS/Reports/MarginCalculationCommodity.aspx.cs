using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace ERP.OMS.Reports
{

    public partial class MarginCalculationCommodity : System.Web.UI.Page, System.Web.UI.ICallbackEventHandler
    {
        BusinessLogicLayer.Converter oconverter = new BusinessLogicLayer.Converter();
        BusinessLogicLayer.DailyTaskOther oDailyTaskOther = new BusinessLogicLayer.DailyTaskOther();
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        DataSet ds = new DataSet();
        DataTable dtgroupcontactid = new DataTable();
        DataTable Distinctclient = new DataTable();
        DataTable dtsp = new DataTable();

        string data;
        protected void Page_Load(object sender, EventArgs e)
        {

            if (HttpContext.Current.Session["userid"] == null)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
            if (!IsPostBack)
            {


                Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script language='javascript'>Page_Load();</script>");


                dtDate.EditFormatString = oconverter.GetDateFormat("Date");
                dtDate.Value = System.DateTime.Today;
                maxdate();
            }
            //_____For performing operation without refreshing page___//
            String cbReference = Page.ClientScript.GetCallbackEventReference(this, "arg", "ReceiveServerData", "context");
            String callbackScript = "function CallServer(arg, context){ " + cbReference + ";}";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "CallServer", callbackScript, true);
            //___________-end here___//



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

        }
        string ICallbackEventHandler.GetCallbackResult()
        {
            return data;
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
            maxdate();
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
        void fn_Client()
        {
            string Clients;
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
                            dtclient = oDBEngine.GetDataTable("tbl_master_contact", "cnt_internalID", " cnt_internalId like 'CL%' and cnt_internalid in(select grp_contactid from tbl_trans_group where  grp_groupmaster in(select gpm_id from tbl_master_groupmaster where gpm_type='" + ddlgrouptype.SelectedItem.Text.ToString() + "'))");
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
                        dtclient = oDBEngine.GetDataTable("tbl_master_contact", "cnt_internalID", " cnt_internalId like 'CL%' and cnt_internalid in(select grp_contactid from tbl_trans_group where grp_groupmaster in(" + HiddenField_Group.Value.ToString().Trim() + "))");
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
                        dtclient = oDBEngine.GetDataTable("tbl_master_contact", "cnt_internalID", " cnt_internalId like 'CL%'  and cnt_branchid in(" + Session["userbranchHierarchy"].ToString() + ")");
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
                        dtclient = oDBEngine.GetDataTable("tbl_master_contact", "cnt_internalID", " cnt_internalId like 'CL%' and cnt_branchid in(" + HiddenField_Branch.Value.ToString().Trim() + ")");
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
                HiddenField_Client.Value = Clients;
            }

        }


        void procedure()
        {

            fn_Client();



            string ClientsID = string.Empty;
            if (rdbClientALL.Checked == true)
            {
                ClientsID = "All";
            }
            else
            {
                ClientsID = HiddenField_Client.Value.ToString().Trim();
            }
            ds = oDailyTaskOther.MarginCalculationCommodity(
                Convert.ToString(dtDate.Value),
                 Convert.ToString(Session["usersegid"]),
                 Convert.ToString(Session["LastCompany"]),
                 Convert.ToString(HttpContext.Current.Session["ExchangeSegmentID"]),
                 Convert.ToString(ClientsID),
                 Convert.ToString(HttpContext.Current.Session["userid"]),
                  Convert.ToString(HttpContext.Current.Session["LastFinYear"])
               );
            ViewState["dataset"] = ds;
            if (ds.Tables[0].Rows.Count == 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "NORECORD3", "NORECORD(1);", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Display", "Display(1);", true);


            }

        }


        protected void btnGenerate_Click(object sender, EventArgs e)
        {
            procedure();
        }
        void maxdate()
        {
            string ExchSeg = HttpContext.Current.Session["ExchangeSegmentID"].ToString();

            DataTable Dtclosingdate = oDBEngine.GetDataTable("Trans_ComDailyStat", "convert(varchar(11),max(ComDailyStat_DateTime),106)", "ComDailyStat_DateTime<='" + dtDate.Value + "' and ComDailyStat_ExchangeSegmentID=" + ExchSeg + "");
            DataTable Dtvardate = oDBEngine.GetDataTable("Trans_ComDailyMarginRates", "convert(varchar(11),max(ComDailyMarginRates_Date),106)", "ComDailyMarginRates_Date<='" + dtDate.Value + "' and ComDailyMarginRates_ExchangesegmentID=" + ExchSeg + "");

            String strhtml = String.Empty;

            strhtml = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";
            strhtml += "<tr style=\"background-color: #fff0f5;\"><td colspan=2>Last Date For </td></tr>";
            strhtml += "<tr style=\"background-color: White;\">";
            if (Dtclosingdate.Rows.Count > 0)
            {
                strhtml += "<td>Closing Rate :</td><td>" + Dtclosingdate.Rows[0][0].ToString() + "</td>";
            }
            strhtml += "</tr>";
            strhtml += "<tr style=\"background-color: White;\">";
            if (Dtvardate.Rows.Count > 0)
            {
                strhtml += "<td>Margin Rate :</td><td>" + Dtvardate.Rows[0][0].ToString() + "</td>";
            }
            strhtml += "</tr></table>";
            display.InnerHtml = strhtml;
        }

    }
}