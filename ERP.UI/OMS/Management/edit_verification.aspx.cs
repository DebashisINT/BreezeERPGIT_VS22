using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using BusinessLogicLayer;


namespace ERP.OMS.Management
{
    public partial class management_edit_verification : System.Web.UI.Page
    {
        BusinessLogicLayer.Management_BL oManagement_BL = new BusinessLogicLayer.Management_BL();
        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);MULTI
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        //SqlConnection con = new SqlConnection(ConfigurationSettings.AppSettings["DBConnectionDefault"]);MULTI
        SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
        BusinessLogicLayer.Converter Oconverter = new BusinessLogicLayer.Converter();
        public static string ServerVaraible1;
        DataTable DT = new DataTable();
        public static string tran;
        int quantity = 1;
        DataSet datatemp = new DataSet();
        public string sourceex = "";
        DataTable dt = new DataTable();
        public string exdate = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                bindverify();
            }
            tran = Session["transactiontype"].ToString();
            if (HttpContext.Current.Session["userid"] == null)
            {
               //Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
            //________This script is for firing javascript when page load first___//
            //if (!ClientScript.IsStartupScriptRegistered("Today"))
            //    ClientScript.RegisterStartupScript(typeof(Page), "Today", "<script>PageLoad();</script>");
            // ______________________________End Script____________________________//


            //this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");
            ServerVaraible1 = Session["usersegid"].ToString().Substring(0, 3);
            txtisin.Attributes.Add("onkeyup", "CallAjax(this,'Searchslip',event)");

            TextBox3.Attributes.Add("onkeyup", "CallAjax(this,'Searchsettlementfrom',event)");
            TextBox4.Attributes.Add("onkeyup", "CallAjaxto(this,'Searchsettlementto',event)");
            txtdpid.Attributes.Add("onkeyup", "CallAjax(this,'Searchdpid',event)");
            //txtdpid3.Text = Session["usersegid"].ToString().Substring(0, 3);
            txtclient.Attributes.Add("onkeyup", "CallAjaxclient(this,'Searchclientid',event)");
            if (tran == "4")
            {
                txtcmb.Attributes.Add("onkeyup", "CallAjaxcmbpid(this,'Searchcmbpid',event)");
            }
            else if (tran == "1")
            {
                txtcmb.Attributes.Add("onkeyup", "CallAjaxclient(this,'Searchcmbpidcdsl',event)");
            }

            if (!IsPostBack)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "SighOff1", "<script>PageLoad();</script>");
                bind_exchange();
                string trantype = "";
                if (tran == "1")
                {
                    trantype = "Market";
                }
                else if (tran == "2")
                {
                    trantype = "Off-Market";
                }
                else if (tran == "3")
                {
                    trantype = "Early-Payin";
                }
                else if (tran == "4")
                {
                    trantype = "Inter-Depository";
                }
                else if (tran == "5")
                {
                    trantype = "Inter-Settlement";
                }
                else if (tran == "6")
                {
                    trantype = "On-Market";
                }
                //lblname.Text = Request.QueryString["id"].ToString().Split('[')[0];
                lblid.Text = Session["BenAccountNumber"].ToString(); //lblslip.Text = Session["slipno"].ToString().Split('~')[0]; lbltype.Text = trantype;
                sourceex = oDBEngine.GetDataTable("Master_CdslClients", " CdslClients_exchangeid", "CdslClients_benaccountnumber='" + Session["BenAccountNumber"].ToString().Trim() + "'").Rows[0][0].ToString();
                if (sourceex == "  ")
                {
                    ddlmkt.Enabled = false;
                    ddlmkt.BackColor = System.Drawing.Color.LightGray;
                    TextBox3.BackColor = System.Drawing.Color.LightGray;
                    TextBox3.Enabled = false;//.style.backgroundColor='#E0E0E0'
                }
                bind_market(sourceex);
            }

        }

        public void bind_exchange()
        {
            if (Session["transactiontype"].ToString() == "1" || Session["transactiontype"].ToString() == "2" || Session["transactiontype"].ToString() == "4")
            {
                if (hiddenex.Value == "IN001002")
                {
                    hiddenex.Value = "12";
                }
                if (hiddenex.Value == "IN001019")
                {
                    hiddenex.Value = "11";
                }

                ddlexchange.DataSource = oDBEngine.GetDataTable("master_cdslexchange", "CdslExchange_ExchangeID,CdslExchange_Name, CdslExchange_ClearingHouseID", "cdslexchange_exchangeid='" + hiddenex.Value + "'");
            }
            else if (Session["transactiontype"].ToString() == "5")
            {
                ddlexchange.DataSource = oDBEngine.GetDataTable("master_cdslexchange", "CdslExchange_ExchangeID,CdslExchange_Name, CdslExchange_ClearingHouseID", "cdslexchange_exchangeid='" + selex() + "'");
            }
            //else if (Session["transactiontype"].ToString() == "4")
            //{
            //    try
            //    {
            //        //ddlexchange.DataSource = oDBEngine.GetDataTable("master_nsdlbplist", "nsdlbplist_bpname as CdslExchange_Name,nsdlbplist_bpid as CdslExchange_ExchangeID", " NsdlBPList_BPRole=1 and   NsdlBPList_bpid in (SELECT DISTINCT NsdlCalendar_CCID from Master_NsdlCalendar )and nsdlbplist_bpid='" + hiddenex.Value.Trim() + "'");
            //        DataTable a = new DataTable();
            //        a = oDBEngine.GetDataTable("master_nsdlbplist", "case when nsdlbplist_bpid ='IN001002' then 'NSE' when nsdlbplist_bpid ='IN001019' then 'BSE' else nsdlbplist_bpname end as CdslExchange_Name,nsdlbplist_bpid as CdslExchange_ExchangeID", " NsdlBPList_BPRole=1 and nsdlbplist_bpid='" + hiddenex.Value.Trim() + "'");
            //        if (a.Rows.Count == 0)
            //        {
            //            ddlexchange.DataSource = oDBEngine.GetDataTable("master_nsdlbplist", "nsdlbplist_bpname as CdslExchange_Name,nsdlbplist_bpid as CdslExchange_ExchangeID ", "NsdlBPList_BPRole=1 and nsdlbplist_bpid=(Select (cast(nsdlbplist_associatedccid as varchar(20))) as nsdlbplist_bpid from master_nsdlbplist WHERE nsdlbplist_bprole= 1 and (nsdlbplist_bpid like '" + txtcmb.Text.Substring(0, 8) + "' or nsdlbplist_bpname like '" + txtcmb.Text.Substring(0, 8) + "') )");
            //        }
            //        else
            //        {
            //            ddlexchange.DataSource = a;
            //        }

            //    }
            //    catch { }
            //}
            else
            {
                ddlexchange.DataSource = oDBEngine.GetDataTable("master_cdslexchange", "CdslExchange_ExchangeID,CdslExchange_Name, CdslExchange_ClearingHouseID", null);
            }
            //ViewState[" CdslExchange_ClearingHouseID"] = oDBEngine.GetDataTable("master_cdslexchange", "CdslExchange_ExchangeID,CdslExchange_Name, CdslExchange_ClearingHouseID", null).Rows[0]["CdslExchange_ClearingHouseID"];
            ddlexchange.DataTextField = "CdslExchange_Name";
            ddlexchange.DataValueField = "CdslExchange_ExchangeID";
            //ddlexchange.SelectedIndex = 0;
            ddlexchange.DataBind();
            //ddlexchange.Items.Insert(0, "Select");
            //bind_market(ddlexchange.SelectedItem.Value.ToString());
            try
            {
                bind_marketto(ddlexchange.SelectedItem.Value.ToString());
            }
            catch { }
        }
        public void bind_market(string s)
        {
            DataTable dtMrkt = new DataTable();

            try
            {
                //DataSet datatemp = new DataSet();
                //datatemp.ReadXml(Server.MapPath("../Documents/" + lblslip.Text.Trim() + "v"));

                dtMrkt = oDBEngine.GetDataTable("Master_CdslMarketTypes", " rtrim(ltrim(CdslMarketTypes_TypeID)) as CdslMarketTypes_TypeID, (CdslMarketTypes_Description+'['+rtrim(ltrim(CdslMarketTypes_Type))+']') as CdslMarketTypes_Description", "CdslMarketTypes_ExchangeID='" + s + "'");
                ddlmkt.DataSource = dtMrkt;
                ddlmkt.DataTextField = "CdslMarketTypes_Description";
                ddlmkt.DataValueField = "CdslMarketTypes_TypeID";
                ddlmkt.DataBind();
                //ddlmkt.Items.FindByText(datatemp.Tables[0].Rows[0]["mkt"].ToString().Trim()).Selected=true;
                ddlmkt.SelectedValue = hiddensettlefrom.Value.Substring(5, 1).Trim();

            }
            catch
            {
                ddlmkt.Items.Clear();
                ddlmkt.DataBind();
                ddlmkt.Items.Insert(0, "Select");
                TextBox3.Text = "";
            }


        }
        public void bind_marketto(string s)
        {
            DataTable dtMrkt = new DataTable();
            DataSet datatemp = new DataSet();
            //datatemp.ReadXml(Server.MapPath("../Documents/" + lblslip.Text.Trim() + "v"));
            try
            {
                //if (Session["transactiontype"].ToString() == "4")
                //{
                //    try
                //    {
                //        dtMrkt = oDBEngine.GetDataTable("Master_NsdlMarketTypes", "NsdlMarketType_dpmcode as CdslMarketTypes_TypeID, NsdlMarketType_Description as CdslMarketTypes_Description ", "NsdlMarketType_CCID='" + s + "'");
                //    }
                //    catch { }
                //}
                //else
                //{
                dtMrkt = oDBEngine.GetDataTable("Master_CdslMarketTypes", " CdslMarketTypes_TypeID, (CdslMarketTypes_Description+'['+rtrim(ltrim(CdslMarketTypes_Type))+']') as CdslMarketTypes_Description", "CdslMarketTypes_ExchangeID='" + s + "'");
                //}

                ddlmktto.DataSource = dtMrkt;
                ddlmktto.DataTextField = "CdslMarketTypes_Description";
                ddlmktto.DataValueField = "CdslMarketTypes_TypeID";
                //ddlmkt.SelectedIndex = 0;
                ddlmktto.DataBind();
                ddlmktto.SelectedValue = hiddensettleto.Value.Substring(5, 1).Trim();

                //ddlmktto.Items.Insert(0, "Select");
            }


            catch
            {

            }
        }
        protected void ddlexchange_SelectedIndexChanged(object sender, EventArgs e)
        {

            bind_marketto(ddlexchange.SelectedItem.Value.ToString());
            ScriptManager.RegisterStartupScript(this, GetType(), "LastCall", "PageLoad()", true);

        }
        protected void Button1_Click(object sender, EventArgs e)
        {
            Hiddendateex.Value = "1";
            if (Hiddenhold.Value != " ")
            {
                DT = oManagement_BL.sp_edit_Searchslip(
                         Convert.ToString(Hiddenhold.Value),
                         Convert.ToString(Session["BenAccountNumber"]),
                         Convert.ToString(Session["usersegid"]),
                         Convert.ToString(Session["dp"]),
                         Convert.ToString(hiddensettlefrom.Value)
                         );

                try
                {
                    txtholding.Text = DT.Rows[0][0].ToString().Split('~')[0];
                }
                catch
                {
                    txtholding.Text = "0";
                }

            }
            else
            {
                bind_exchange();
            }
            ScriptManager.RegisterStartupScript(this, GetType(), "LastCall", "PageLoad()", true);

        }
        public string selex()
        {
            string Exchange = oDBEngine.GetDataTable("Master_CdslClients", "*", "substring(CdslClients_boid,9,8)='" + Session["BenAccountNumber"].ToString().Trim() + "'").Rows[0]["CdslClients_ExchangeID"].ToString();
            return Exchange;

        }
        protected void btnsave_Click(object sender, EventArgs e)
        {
            string CdslExchange_ClearingHouseID = "";
            try
            {
                CdslExchange_ClearingHouseID = oDBEngine.GetDataTable("master_cdslexchange", "CdslExchange_ExchangeID,CdslExchange_Name, CdslExchange_ClearingHouseID", "CdslExchange_Name='" + ddlexchange.SelectedItem.Text.ToString() + "'").Rows[0]["CdslExchange_ClearingHouseID"].ToString();
            }
            catch
            {

            }

            string isin = txtisin.Text.Split('[')[0];
            string freehol = txtholding.Text;
            string quantity = txtqty.Text;
            string dpid = "";
            string cmbpid = "";
            if (txtcmb.Text == "")
            {
                cmbpid = "";
            }
            else
            {
                if (tran == "1")
                {
                    cmbpid = txtcmb.Text.Substring(0, 16);
                }
                else if (tran == "4")
                {
                    cmbpid = txtcmb.Text.Substring(0, 8);
                }
            }
            if (txtdpid3.Text == "" && txtdpid.Text == "")
            {

            }
            else
            {
                if (tran != "4")
                {
                    dpid = txtdpid.Text.Split('[')[0];
                }
                else
                    dpid = txtdpid.Text.Split('[')[0];
            }
            string clientid = "";
            if (txtclient.Text == "")
            {

            }
            else if (txtclient.Text.Contains("["))
            {
                clientid = txtclient.Text.Split('[')[0];
            }
            else
            {
                clientid = txtclient.Text;
            }
            string exchange = "";
            try
            {
                int q = Convert.ToInt32((ddlexchange.SelectedItem.Value.ToString()));
                exchange = ddlexchange.SelectedItem.Value.ToString();
            }
            catch
            {

            }
            if (exchange == "Select")
            {
                exchange = "";
            }
            string mkt = "";
            try
            {
                mkt = ddlmkt.SelectedItem.Value.ToString();
            }
            catch { }
            if (mkt == "Select")
            {
                mkt = "";
            }
            string mktto = "";
            try
            {
                mktto = ddlmktto.SelectedItem.Value.ToString();
            }
            catch { }
            if (mktto == "Select")
            {
                mktto = "";
            }
            if (hiddensettleto.Value == "")
            {
                mktto = "";
            }

            else if (hiddensettlefrom.Value == "")
            {
                mkt = "";
            }
            if (txtclient.Enabled == false)
            {
                clientid = "";
            }
            string id = Request.QueryString["id"];

            try
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }


                con.Open();
                string dpid3 = "";
                try
                {
                    dpid3 = oDBEngine.GetDataTable("master_cdslbplist", "cdslbplist_dptype", "cdslbplist_dpid='" + txtdpid.Text.Split('[')[0].Remove(txtdpid.Text.Split('[')[0].Length - 1, 1) + "'").Rows[0][0].ToString().Trim();
                }
                catch
                {
                }
                if (dpid3 == "3")
                {
                    txtdpid3.Text = "130";
                }
                else if (dpid3 == "2")
                {
                    txtdpid3.Text = "120";
                }
                else if (dpid3 == "6")
                {
                    txtdpid3.Text = "160";
                }
                else if (dpid3 == "1")
                {
                    txtdpid3.Text = "110";

                }
                else if (dpid3 == "5")
                {
                    txtdpid3.Text = "150";
                }
                if (tran == "2")
                {
                    dpid = txtdpid3.Text.Trim() + dpid.Trim();
                }

                oManagement_BL.sp_edit_verification_log(
               id,
                isin,
                 Convert.ToDecimal(txtholding.Text),
                 Convert.ToDecimal(quantity),
                dpid,
                clientid,
                cmbpid,
                exchange,
                 Convert.ToString(hiddensettleto.Value),
                 Convert.ToString(hiddensettlefrom.Value),
                 "CDSL",
                "",
                 "",
                 Convert.ToString(lblid.Text),
                 "",
                 dtexec.Text.Split('-')[2] + "-" + dtexec.Text.Split('-')[1] + "-" + dtexec.Text.Split('-')[0]
                 );

                bindverify();
                ScriptManager.RegisterStartupScript(HttpContext.Current.Handler as Page, HttpContext.Current.GetType(), "di", "alert('Updated successfully');", true);
                Session.Remove("transactiontype");
                Session.Remove("dp");
                Session.Remove("BenAccountNumber");
                ScriptManager.RegisterStartupScript(this, GetType(), "pageclose", "pageclose()", true);

            }
            catch
            {

            }

            txtisin.Text = ""; txtholding.Text = ""; txtqty.Text = ""; txtdpid.Text = ""; txtcmb.Text = ""; txtclient.Text = ""; TextBox1.Text = ""; TextBox3.Text = ""; TextBox4.Text = "";
            ddlmkt.Items.Clear();
            bind_market(sourceex);
            ScriptManager.RegisterStartupScript(this, GetType(), "LastCall", "PageLoad()", true);
            ScriptManager.RegisterStartupScript(this, GetType(), "pageclose", "pageclose()", true);
            bindverify();



        }
        protected void btncancel_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "LastCall", "PageLoad()", true);
            ScriptManager.RegisterStartupScript(this, GetType(), "pageclose", "pageclose()", true);


        }

        public void bindverify()
        {
            dt = oDBEngine.GetDataTable("trans_cdsloffline", "*", "cdsloffline_id=" + Request.QueryString["id"] + "");
            Session["transactiontype"] = dt.Rows[0]["cdsloffline_transactiontype"].ToString();
            Session["usersegid"] = dt.Rows[0]["cdsloffline_dpid"].ToString();
            Session["BenAccountNumber"] = dt.Rows[0]["cdsloffline_benaccountnumber"].ToString();
            Session["dp"] = "CDSL";
            lblslip.Text = dt.Rows[0]["cdsloffline_slipnumber"].ToString().Trim();
            //if (Session["transactiontype"].ToString() != "4" )
            //{
            try
            {
                if (Session["transactiontype"].ToString() != "4")
                {
                    TextBox4.Text = dt.Rows[0]["cdsloffline_countersettlementid"].ToString().Substring(6);
                }
                else
                {
                    TextBox4.Text = dt.Rows[0]["CdslOffline_NSDLSettlementID"].ToString().Substring(6);
                }
            }
            catch
            {
                TextBox4.Text = "";
            }
            try
            {
                TextBox3.Text = dt.Rows[0]["cdsloffline_settlementid"].ToString().Substring(6);
            }
            catch
            {
                TextBox3.Text = "";
            }

            txtisin.Text = dt.Rows[0]["cdsloffline_isin"].ToString() + "[ " + oDBEngine.GetDataTable("master_cdslisin", "cdslisin_shortname", "cdslisin_number='" + dt.Rows[0]["cdsloffline_isin"].ToString() + "'").Rows[0][0].ToString().Trim() + " ]";
            txtqty.Text = dt.Rows[0]["cdsloffline_quantity"].ToString();
            if (Session["transactiontype"].ToString() != "4")
            {
                txtdpid.Text = dt.Rows[0]["cdsloffline_countercdslid"].ToString().Substring(3, 5).Trim() + "[ " + oDBEngine.GetDataTable("master_cdslbplist", "cdslbplist_firstname", "cdslbplist_dpid like '%" + dt.Rows[0]["cdsloffline_countercdslid"].ToString().Substring(3, 5).Trim() + "'").Rows[0][0].ToString().Trim() + " ]";
                txtdpid3.Text = dt.Rows[0]["cdsloffline_countercdslid"].ToString().Substring(0, 3).Trim();
                hiddendpid.Value = dt.Rows[0]["cdsloffline_countercdslid"].ToString().Substring(3, 5).Trim();
                try
                {
                    txtclient.Text = dt.Rows[0]["cdsloffline_countercdslid"].ToString().Substring(8).Trim() + "[ " + oDBEngine.GetDataTable("Master_CdslClients", "CdslClients_firstholdername", "substring(CdslClients_boid,len(CdslClients_boid)-7,8)like '%" + dt.Rows[0]["cdsloffline_countercdslid"].ToString().Substring(8).Trim() + "'").Rows[0][0].ToString().Trim() + " ]";
                }
                catch
                {
                    txtclient.Text = dt.Rows[0]["cdsloffline_countercdslid"].ToString().Substring(8).Trim();
                }
            }
            else
            {
                try
                {
                    try
                    {
                        txtdpid.Text = dt.Rows[0]["cdsloffline_nsdldpid"].ToString().Trim() + "[ " + oDBEngine.GetDataTable("master_nsdlbplist", "nsdlbplist_bpname", "nsdlbplist_bpid='" + dt.Rows[0]["cdsloffline_nsdldpid"].ToString().Trim() + "'").Rows[0][0].ToString().Trim() + " ]";
                    }
                    catch
                    {
                        txtdpid.Text = dt.Rows[0]["cdsloffline_nsdldpid"].ToString().Trim();
                    }
                    hiddendpid.Value = dt.Rows[0]["cdsloffline_nsdldpid"].ToString().Trim();
                    try
                    {
                        txtclient.Text = dt.Rows[0]["cdsloffline_nsdlclientid"].ToString().Trim() + "[ " + oDBEngine.GetDataTable("Master_NsdlClients", "NsdlClients_ShortName", "NsdlClients_BenAccountID='" + dt.Rows[0]["cdsloffline_nsdlclientid"].ToString().Trim() + "'").Rows[0][0].ToString().Trim() + " ]";
                    }
                    catch
                    {
                        txtclient.Text = dt.Rows[0]["cdsloffline_nsdlclientid"].ToString().Trim();
                    }
                }
                catch { }
                if (txtdpid.Text != "")
                {
                    ddlmktto.Enabled = false;
                    ddlmktto.BackColor = System.Drawing.Color.LightGray;
                    TextBox4.BackColor = System.Drawing.Color.LightGray;
                    TextBox4.Enabled = false;
                    ddlexchange.Enabled = false;
                    ddlexchange.BackColor = System.Drawing.Color.LightGray;
                    txtcmb.BackColor = System.Drawing.Color.LightGray;
                    txtcmb.Enabled = false;
                    TextBox1.BackColor = System.Drawing.Color.LightGray;
                    TextBox1.Enabled = false;
                }
            }
            if (Session["transactiontype"].ToString() == "1")
            {
                try
                {
                    txtcmb.Text = dt.Rows[0]["cdsloffline_countercdslid"].ToString().Trim() + "[ " + oDBEngine.GetDataTable("Master_CdslClearingMember", "CdslClearingMember_name1", "CdslClearingMember_principalaccount='" + dt.Rows[0]["cdsloffline_countercdslid"].ToString().Trim() + "' or CdslClearingMember_unifiedaccount='" + dt.Rows[0]["cdsloffline_countercdslid"].ToString().Trim() + "'").Rows[0][0].ToString() + " ]";
                }
                catch { }
            }
            else if (Session["transactiontype"].ToString() == "4")
            {
                try
                {
                    txtcmb.Text = dt.Rows[0]["cdsloffline_nsdlcmbpid"].ToString().Trim() + "[ " + oDBEngine.GetDataTable("master_nsdlbplist", "nsdlbplist_bpname", "nsdlbplist_bpid='" + dt.Rows[0]["cdsloffline_nsdlcmbpid"].ToString().Trim() + "' ").Rows[0][0].ToString().Trim() + " ]";
                }
                catch { }
            }
            hiddensettlefrom.Value = dt.Rows[0]["CdslOffline_SettlementID"].ToString().Trim();
            hiddensettleto.Value = dt.Rows[0]["CdslOffline_CounterSettlementID"].ToString().Trim();
            hiddenex.Value = dt.Rows[0]["cdsloffline_exchangeid"].ToString().Trim();
            Hiddenhold.Value = dt.Rows[0]["cdsloffline_isin"].ToString().Trim();
            dtexec.Value = dt.Rows[0]["CdslOffline_ExecutionDate"].ToString().Trim();
            exdate = dt.Rows[0]["CdslOffline_ExecutionDate"].ToString().Trim();
            ScriptManager.RegisterStartupScript(this, GetType(), "LastCall", "PageLoad()", true);
        }
    }
}