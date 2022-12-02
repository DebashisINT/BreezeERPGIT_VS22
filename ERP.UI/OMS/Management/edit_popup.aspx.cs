using System;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Configuration;
using BusinessLogicLayer;

namespace ERP.OMS.Management
{
    public partial class management_edit_popup : System.Web.UI.Page
    {
        BusinessLogicLayer.Management_BL oManagement_BL = new BusinessLogicLayer.Management_BL();
        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);MULTI
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
       // SqlConnection con = new SqlConnection(ConfigurationSettings.AppSettings["DBConnectionDefault"]);
        BusinessLogicLayer.Converter Oconverter = new BusinessLogicLayer.Converter();
        public static string ServerVaraible1;
        DataTable DT = new DataTable();
        public static string tran;
        int quantity = 1;
        DataSet datatemp = new DataSet();
        public string sourceex = "";
        public string exdate = "";
        DataTable dt = new DataTable();
        //public int dateflag = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Hiddenid.Value = Request.QueryString["id"].ToString();
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
            txtdpid3.Text = Session["usersegid"].ToString().Substring(0, 3);
            txtclient.Attributes.Add("onkeyup", "CallAjaxclient(this,'Searchclientid',event)");
            if (tran == "1")
            {
                txtcmb.Attributes.Add("onkeyup", "CallAjaxcmbpid(this,'Searchcmbpid',event)");
            }
            else if (tran == "4")
            {
                txtcmb.Attributes.Add("onkeyup", "CallAjaxclient(this,'Searchcmbpidcdsl',event)");
            }

            if (!IsPostBack)
            {
                string exc = "";
                try
                {
                    exc = oDBEngine.GetDataTable("master_nsdlclients", "nsdlclients_correspondingbpid", "nsdlclients_benaccountid='" + Session["BenAccountNumber"].ToString() + "'").Rows[0][0].ToString();
                }
                catch
                {
                }

                Page.ClientScript.RegisterStartupScript(GetType(), "SighOff1", "<script>PageLoad();</script>");

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

                lblid.Text = Session["BenAccountNumber"].ToString(); lbltype.Text = trantype;
                try
                {
                    if (exc != "")
                    {
                        sourceex = oDBEngine.GetDataTable("master_nsdlbplist", "nsdlbplist_bpname as CdslExchange_Name,nsdlbplist_associatedccid as CdslExchange_ExchangeID", " NsdlBPList_BPRole=1 and nsdlbplist_bpid='" + exc + "'").Rows[0][1].ToString();
                    }
                    else
                    {
                        sourceex = oDBEngine.GetDataTable("master_nsdlbplist", "nsdlbplist_bpname as CdslExchange_Name,nsdlbplist_associatedccid as CdslExchange_ExchangeID", " NsdlBPList_BPRole=1 and nsdlbplist_bpid='" + Session["BenAccountNumber"].ToString() + "'").Rows[0][1].ToString();
                    }
                }
                catch { }
                if (sourceex == "")
                {
                    ddlmkt.Enabled = false;
                    ddlmkt.BackColor = System.Drawing.Color.LightGray;
                    TextBox3.BackColor = System.Drawing.Color.LightGray;
                    TextBox3.Enabled = false;//.style.backgroundColor='#E0E0E0'
                }
                if (Session["transactiontype"].ToString() == "5" || (Session["transactiontype"].ToString() == "10") || (Session["transactiontype"].ToString() == "11"))
                {
                    hiddenex.Value = sourceex;
                }
                bind_exchange();
                bind_market(sourceex);
            }

        }

        public void bind_exchange()
        {
            if (Session["transactiontype"].ToString() == "4")
            {
                ddlexchange.DataSource = oDBEngine.GetDataTable("master_cdslexchange", "CdslExchange_ExchangeID,CdslExchange_Name, CdslExchange_ClearingHouseID", "cdslexchange_exchangeid='" + hiddenex.Value + "'");
            }
            else if (Session["transactiontype"].ToString() == "5" || Session["transactiontype"].ToString() == "10" || Session["transactiontype"].ToString() == "11")
            {
                ddlexchange.DataSource = oDBEngine.GetDataTable("master_nsdlbplist", "nsdlbplist_bpname as CdslExchange_Name,nsdlbplist_bpid as CdslExchange_ExchangeID", " NsdlBPList_BPRole=1 and nsdlbplist_bpid='" + hiddenex.Value.Trim() + "'");
            }
            else if (Session["transactiontype"].ToString() == "1")
            {
                try
                {
                    ddlexchange.DataSource = oDBEngine.GetDataTable("master_nsdlbplist", "nsdlbplist_bpname as CdslExchange_Name,nsdlbplist_bpid as CdslExchange_ExchangeID", " NsdlBPList_BPRole=1 and nsdlbplist_bpid='" + hiddenex.Value.Trim() + "'");
                }
                catch { }
            }
            else
            {
                ddlexchange.DataSource = oDBEngine.GetDataTable("master_cdslexchange", "CdslExchange_ExchangeID,CdslExchange_Name, CdslExchange_ClearingHouseID", null);
            }
            ddlexchange.DataTextField = "CdslExchange_Name";
            ddlexchange.DataValueField = "CdslExchange_ExchangeID";
            ddlexchange.DataBind();
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
                dtMrkt = oDBEngine.GetDataTable("Master_NsdlMarketTypes", "ltrim(rtrim(NsdlMarketType_dpmcode)) as CdslMarketTypes_TypeID, ltrim(rtrim(NsdlMarketType_Description)) as CdslMarketTypes_Description ", " NsdlMarketType_ccid='" + s + "'");
                ddlmkt.DataSource = dtMrkt;
                ddlmkt.DataTextField = "CdslMarketTypes_Description";
                ddlmkt.DataValueField = "CdslMarketTypes_TypeID";
                ddlmkt.DataBind();
                ddlmkt.SelectedValue = ViewState["mkt"].ToString().Trim();


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
            ddlmktto.Items.Clear();
            DataTable dtMrkt = new DataTable();
            try
            {
                if (Session["transactiontype"].ToString() != "4")
                {
                    try
                    {
                        dtMrkt = oDBEngine.GetDataTable("Master_NsdlMarketTypes", "ltrim(rtrim(NsdlMarketType_dpmcode)) as CdslMarketTypes_TypeID, ltrim(rtrim(NsdlMarketType_Description)) as CdslMarketTypes_Description ", "NsdlMarketType_ccid='" + ViewState["ex"] + "'");
                    }
                    catch { }
                }
                else
                {
                    //dtMrkt = oDBEngine.GetDataTable("Master_CdslMarketTypes", " CdslMarketTypes_TypeID, (CdslMarketTypes_Description+'['+rtrim(ltrim(CdslMarketTypes_Type))+']') as CdslMarketTypes_Description", "CdslMarketTypes_TypeID='" + ViewState["omkt"] + "'");
                    dtMrkt = oDBEngine.GetDataTable("Master_CdslMarketTypes", " ltrim(rtrim(CdslMarketTypes_TypeID)) as CdslMarketTypes_TypeID, (CdslMarketTypes_Description+'['+rtrim(ltrim(CdslMarketTypes_Type))+']') as CdslMarketTypes_Description", "CdslMarketTypes_ExchangeID='" + ViewState["ex"] + "'");
                }

                ddlmktto.DataSource = dtMrkt;
                ddlmktto.DataTextField = "CdslMarketTypes_Description";
                ddlmktto.DataValueField = "CdslMarketTypes_TypeID";
                ddlmktto.DataBind();
                if (ViewState["omkt"].ToString().Trim() == "")
                {
                    ddlmktto.Items.Insert(0, "Select");
                }
                else
                {
                    ddlmktto.SelectedValue = ViewState["omkt"].ToString().Trim();
                }
            }


            catch
            {
                ddlmktto.Items.Clear();
                ddlmktto.DataBind();
                ddlmktto.Items.Insert(0, "Select");
                TextBox4.Text = "";
            }
        }
        protected void ddlexchange_SelectedIndexChanged(object sender, EventArgs e)
        {

            bind_marketto(ddlexchange.SelectedItem.Value.ToString());
            ScriptManager.RegisterStartupScript(this, GetType(), "LastCall", "PageLoad()", true);

        }

        protected void btnsave_Click(object sender, EventArgs e)
        {

            string isin = txtisin.Text.Split('[')[0];
            string freehol = txtholding.Text;
            string quantity = txtqty.Text;
            string dpid = "";
            string cmbpid = "";

            if (txtdpid.Text == "")
            {

            }
            else
            {
                if (tran != "4")
                {
                    dpid = txtdpid.Text.Split('[')[0];
                }
                else
                    dpid = "120" + txtdpid.Text.Substring(0, 5);
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
                exchange = ddlexchange.SelectedItem.Value.ToString();
            }
            catch { }
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
                ddlmkt.SelectedItem.Value = "";
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
            DataSet ds = new DataSet();
            DataTable Table = ds.Tables.Add();

            if (TextBox4.Text == "")
            {
                mktto = "";
            }
            else if (TextBox3.Text == "")
            {
                mkt = "";
            }
            try
            {
                if (tran == "4")
                {
                    cmbpid = txtcmb.Text.Substring(0, 16);
                    dpid = cmbpid.Substring(0, 8);
                    clientid = cmbpid.Substring(8, 8);
                    cmbpid = "";

                }
                else
                {
                    cmbpid = txtcmb.Text.Substring(0, 8);
                }
            }
            catch
            {
                cmbpid = txtcmb.Text;
            }
            if (ddlexchange.Enabled == false)
            {
                exchange = "";
            }
            string id = Request.QueryString["id"].Split(',')[0];
            if ((tran == "4") && (hiddensettleto.Value.Length != 7))
            {
                try
                {
                    hiddensettleto.Value = hiddensettleto.Value.Substring(6);
                }
                catch
                {
                }
            }
            int trantype = Convert.ToInt32(Session["transactiontype"]);
            if ((tran == "10") || (tran == "11"))
            {
                if (RadioButtonList1.SelectedValue == "906")
                {
                    trantype = 10;
                }
                else
                {
                    trantype = 11;
                }
            }
            if (tran == "1")
            {
                dpid = Hiddendpidm.Value;
            }
            try
            {
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
                 "NSDL",
                 Convert.ToString(mkt),
                 Convert.ToString(mktto),
                 Convert.ToString(lblid.Text),
                  Convert.ToString(trantype),
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
            txtisin.Text = ""; txtholding.Text = ""; txtqty.Text = ""; txtdpid.Text = ""; txtcmb.Text = ""; txtclient.Text = ""; TextBox1.Text = ""; TextBox4.Text = "";

            ScriptManager.RegisterStartupScript(this, GetType(), "LastCall", "PageLoad()", true);
            ScriptManager.RegisterStartupScript(this, GetType(), "pageclose", "pageclose()", true);




        }



        protected void btncancel_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "LastCall", "PageLoad()", true);
            ScriptManager.RegisterStartupScript(this, GetType(), "pageclose", "pageclose()", true);


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
                       Convert.ToString(TextBox3.Text)
                       );

                try
                {
                    txtholding.Text = DT.Rows[0][0].ToString().Split('~')[0];
                    if (txtholding.Text == "")
                    {
                        txtholding.Text = "0";
                    }
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
        public void bindverify()
        {
            dt = oDBEngine.GetDataTable("trans_nsdloffline", "*", "nsdloffline_id=" + Request.QueryString["id"].Split(',')[0] + "");
            Session["transactiontype"] = dt.Rows[0]["NsdlOffline_TransactionType"].ToString();
            Session["usersegid"] = dt.Rows[0]["NsdlOffline_DPID"].ToString();
            Session["BenAccountNumber"] = dt.Rows[0]["NsdlOffline_ClientID"].ToString();
            Session["dp"] = "NSDL";
            lblslip.Text = dt.Rows[0]["NsdlOffline_SlipNumber"].ToString().Trim();
            TextBox3.Text = dt.Rows[0]["NsdlOffline_SettlementNumber"].ToString();
            TextBox4.Text = dt.Rows[0]["NsdlOffline_OtherSettlementNumber"].ToString();
            txtisin.Text = dt.Rows[0]["NsdlOffline_ISIN"].ToString() + "[ " + oDBEngine.GetDataTable("master_nsdlisin", "nsdlisin_companyname", "nsdlisin_number='" + dt.Rows[0]["NsdlOffline_ISIN"].ToString() + "'").Rows[0][0].ToString().Trim() + " ]";
            txtqty.Text = dt.Rows[0]["NsdlOffline_Quantity"].ToString();

            try
            {
                if (Session["transactiontype"].ToString() == "4")
                {
                    if (TextBox4.Text.Trim() == "")
                    {
                        txtdpid.Text = dt.Rows[0]["NsdlOffline_OtherDPID"].ToString().Substring(3).Trim() + "[ " + oDBEngine.GetDataTable("master_cdslbplist", "cdslbplist_firstname", "cdslbplist_dpid like '%" + dt.Rows[0]["NsdlOffline_OtherDPID"].ToString().Substring(3).Trim() + "'").Rows[0][0].ToString().Trim() + " ]";
                        try
                        {
                            txtclient.Text = dt.Rows[0]["NsdlOffline_OtherClientID"].ToString().Trim() + "[ " + oDBEngine.GetDataTable("Master_CdslClients", "CdslClients_firstholdername", "substring(CdslClients_boid,len(CdslClients_boid)-7,8)like '%" + dt.Rows[0]["NsdlOffline_OtherClientID"].ToString().Trim() + "'").Rows[0][0].ToString().Trim() + " ]";
                        }
                        catch
                        {
                            txtclient.Text = dt.Rows[0]["NsdlOffline_OtherClientID"].ToString().Trim();
                        }
                    }
                    else
                    {
                        txtcmb.Text = dt.Rows[0]["NsdlOffline_OtherDPID"].ToString().Trim() + dt.Rows[0]["NsdlOffline_OtherClientID"].ToString().Trim() + "[ " + oDBEngine.GetDataTable("Master_CdslClients", "CdslClients_firstholdername", "substring(CdslClients_boid,len(CdslClients_boid)-7,8)like '%" + dt.Rows[0]["NsdlOffline_OtherClientID"].ToString().Trim() + "'").Rows[0][0].ToString().Trim() + " ]";
                        txtdpid.Text = "";
                        txtclient.Text = "";
                    }
                }
                else
                {
                    txtdpid.Text = dt.Rows[0]["NsdlOffline_OtherDPID"].ToString().Trim() + "[ " + oDBEngine.GetDataTable("master_nsdlbplist", "nsdlbplist_bpname", "nsdlbplist_bpid='" + dt.Rows[0]["NsdlOffline_OtherDPID"].ToString().Trim() + "'").Rows[0][0].ToString().Trim() + " ]";
                    try
                    {
                        txtclient.Text = dt.Rows[0]["NsdlOffline_OtherClientID"].ToString().Trim() + "[ " + oDBEngine.GetDataTable("Master_NsdlClients", "NsdlClients_ShortName", "NsdlClients_BenAccountID='" + dt.Rows[0]["NsdlOffline_OtherClientID"].ToString().Trim() + "'").Rows[0][0].ToString().Trim() + " ]";
                    }
                    catch
                    {
                        txtclient.Text = dt.Rows[0]["NsdlOffline_OtherClientID"].ToString().Trim();
                    }

                }
            }
            catch { }
            if ((txtdpid.Text != "") && (Session["transactiontype"].ToString() != "10" && Session["transactiontype"].ToString() != "11"))
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
            if (Session["transactiontype"].ToString() == "1")
            {
                ddlmktto.Enabled = true;
                ddlmktto.BackColor = System.Drawing.Color.White;
                TextBox4.BackColor = System.Drawing.Color.White;
                TextBox4.Enabled = true;
                ddlexchange.Enabled = true;
                ddlexchange.BackColor = System.Drawing.Color.White;
                txtcmb.BackColor = System.Drawing.Color.White;
                txtcmb.Enabled = true;
                TextBox1.BackColor = System.Drawing.Color.White;
                TextBox1.Enabled = true;
            }

            if (Session["transactiontype"].ToString() == "1")
            {
                try
                {
                    txtcmb.Text = dt.Rows[0]["NsdlOffline_OtherCMBPID"].ToString().Trim() + "[ " + oDBEngine.GetDataTable("master_nsdlbplist", "nsdlbplist_bpname", "nsdlbplist_bpid='" + dt.Rows[0]["NsdlOffline_OtherCMBPID"].ToString().Trim() + "' ").Rows[0][0].ToString().Trim() + " ]";
                }
                catch { }
            }
            Hiddendpidm.Value = txtdpid.Text.Split('[')[0].ToString().Trim();
            hiddensettlefrom.Value = dt.Rows[0]["NsdlOffline_SettlementNumber"].ToString().Trim();
            hiddensettleto.Value = dt.Rows[0]["NsdlOffline_OtherSettlementNumber"].ToString().Trim();
            hiddenex.Value = dt.Rows[0]["NsdlOffline_CCCMID"].ToString().Trim();
            Hiddenhold.Value = dt.Rows[0]["NsdlOffline_ISIN"].ToString().Trim();
            ViewState["mkt"] = dt.Rows[0]["NsdlOffline_MarketType"].ToString().Trim();
            ViewState["omkt"] = dt.Rows[0]["NsdlOffline_otherMarketType"].ToString().Trim();
            ViewState["ex"] = dt.Rows[0]["NsdlOffline_cccmid"].ToString().Trim();
            hiddendpid.Value = dt.Rows[0]["NsdlOffline_OtherDPID"].ToString().Substring(3).Trim();
            if (dt.Rows[0]["NsdlOffline_TransactionType"].ToString() == "10")
            {
                RadioButtonList1.SelectedIndex = 0;
            }
            else
            {
                RadioButtonList1.SelectedIndex = 1;
            }
            dtexec.Value = dt.Rows[0]["NsdlOffline_ExecutionDate"].ToString().Trim();
            exdate = dt.Rows[0]["NsdlOffline_ExecutionDate"].ToString().Trim();

            //dtexec.Text = dt.Rows[0]["NsdlOffline_ExecutionDate"].ToString().Trim();
            ScriptManager.RegisterStartupScript(this, GetType(), "LastCall", "PageLoad()", true);

        }
    }

}