using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Configuration;
using BusinessLogicLayer;

namespace ERP.OMS.Management.SettingsOptions
{
    public partial class management_SettingsOptions_frmtargetschemes : System.Web.UI.Page, ICallbackEventHandler
    {
        //DBEngine oDBEngine = new DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        //Converter objConverter = new Converter();
        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);MULTI
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        BusinessLogicLayer.Converter objConverter = new BusinessLogicLayer.Converter();
        string data = "";
        //Converter OConvert = new Converter();
        BusinessLogicLayer.Converter OConvert = new BusinessLogicLayer.Converter();
        public string pageAccess = "";
        // DBEngine oDBEngine = new DBEngine(string.Empty);
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
            if (!IsPostBack)
            {
                //ImgStartDate.Attributes.Add("OnClick", "displayCalendar(TxtEndDate,'mm/dd/yyyy',TxtEndDate,true,null,'0',0)");
                //TxtEndDate.Attributes.Add("onfocus", "displayCalendar(TxtEndDate ,'mm/dd/yyyy',this,true,null,'0',0)");
                TxtEndDate.EditFormatString = OConvert.GetDateFormat("Date");
                TxtEndDate.Attributes.Add("readonly", "true");
                FillGrid();
                Jscript();
                Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script language='javascript'>TdShow();</script>");
            }
            TxtEndDate.Attributes.Add("readonly", "true");
            TxtEffectiveDate.Attributes.Add("readonly", "true");
            String cbReference = Page.ClientScript.GetCallbackEventReference(this, "arg", "ReceiveServerData", "context");
            String callbackScript = "function CallServer(arg, context){ " + cbReference + ";}";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "CallServer", callbackScript, true);
        }
        public void FillGrid()
        {
            //  DataTable dt = oDBEngine.GetDataTable("tbl_targetschemes", "tgt_masterid,tgt_descripition,tgt_periodicity,convert(varchar(20),cast(tgt_effectivedate as datetime),113)  as [Effective date],convert(nvarchar,cast(tgt_enddate as datetime),106) as [End Date]", "CAST(tgt_effectivedate AS datetime) < CAST(GETDATE() AS datetime) AND CAST(tgt_enddate AS datetime) > CAST(GETDATE() AS datetime)");
          DataTable dt = oDBEngine.GetDataTable("tbl_targetschemes", "tgt_masterid,tgt_descripition,tgt_periodicity,convert(varchar(20),convert(datetime,tgt_effectivedate ,103),113)  as [Effective date],convert(nvarchar,convert(datetime,tgt_enddate ,103),106) as [End Date]", "convert(datetime, tgt_effectivedate ,103) < convert(datetime, GETDATE(),103) AND convert(datetime, tgt_enddate ,103 ) > convert(datetime, GETDATE(),103 )");
           // DataTable dt = oDBEngine.GetDataTable("tbl_targetschemes", "tgt_masterid,tgt_descripition,tgt_periodicity,convert(varchar(20),convert(datetime,tgt_effectivedate ,103),113)  as [Effective date],convert(nvarchar,convert(datetime,tgt_enddate ,103),106) as [End Date]", "");

            grd_target.DataSource = dt.DefaultView;
            grd_target.DataBind();
        }
        public void FillGridDetails(string id)
        {
            DataTable dt = oDBEngine.GetDataTable("tbl_targetschemes", "tgt_id,tgt_descripition,tgt_periodicity,convert(nvarchar,cast(tgt_effectivedate as datetime),106) as [Effective date],convert(nvarchar,cast(tgt_enddate as datetime),106) as [End Date]", "tgt_masterid=" + id + " order by tgt_effectivedate");
            grdtargetdetails.DataSource = dt.DefaultView;
            grdtargetdetails.DataBind();
        }
        public void Jscript()
        {
            txt_derivatives.Attributes.Add("onblur", "sum_value(this)");
            txt_derivatives.Attributes.Add("onkeyup", "validate_gen(this,1,'Derivatives Should Be Numeric!!',0,1)");
            txt_commodities.Attributes.Add("onblur", "sum_value(this)");
            txt_commodities.Attributes.Add("onkeyup", "validate_gen(this,1,'Commodities Should Be Numeric!!',0,1)");
            txt_equities.Attributes.Add("onblur", "sum_value(this)");
            txt_equities.Attributes.Add("onkeyup", "validate_gen(this,1,'Equities Should Be Numeric!!',0,1)");
            txt_retails.Attributes.Add("onblur", "sum_value(this)");
            txt_retails.Attributes.Add("onkeyup", "validate_gen(this,1,'Retails Should Be Numeric!!',0,1)");
            txt_hni.Attributes.Add("onblur", "sum_value(this)");
            txt_hni.Attributes.Add("onkeyup", "validate_gen(this,1,'HNI Should Be Numeric!!',0,1)");
            txt_institution.Attributes.Add("onblur", "sum_value(this)");
            txt_institution.Attributes.Add("onkeyup", "validate_gen(this,1,'Institution Should Be Numeric!!',0,1)");
            txt_coldcalls.Attributes.Add("onblur", "sum_value(this)");
            txt_coldcalls.Attributes.Add("onkeyup", "validate_gen(this,1,'ColdCalls Should Be Numeric!!',0,1)");
            txt_newcalls.Attributes.Add("onblur", "sum_value(this)");
            txt_newcalls.Attributes.Add("onkeyup", "validate_gen(this,1,'NewCalls Should Be Numeric!!',0,1)");
            txt_selfsalesvisit.Attributes.Add("onblur", "sum_value(this)");
            txt_selfsalesvisit.Attributes.Add("onkeyup", "validate_gen(this,1,'SelfGenerated salesvisit Should Be Numeric!!',0,1)");
            txt_newsalesvisit.Attributes.Add("onblur", "sum_value(this)");
            txt_newsalesvisit.Attributes.Add("onkeyup", "validate_gen(this,1,'New SalesVisit Should Be Numeric!!',0,1)");
            txt_unitmonthly.Attributes.Add("onblur", "sum_value(this)");
            txt_unitmonthly.Attributes.Add("onkeyup", "validate_gen(this,1,'Unit Should Be Numeric!!',0,1)");
            txt_acpmonthly.Attributes.Add("onblur", "sum_value(this)");
            txt_acpmonthly.Attributes.Add("onkeyup", "validate_gen(this,1,'Avg. Money  Should Be Numeric!!',0,1)");
            txt_unitquarterly.Attributes.Add("onblur", "sum_value(this)");
            txt_unitquarterly.Attributes.Add("onkeyup", "validate_gen(this,1,'Unit Should Be Numeric!!',0,1)");
            txt_acpquartly.Attributes.Add("onblur", "sum_value(this)");
            txt_acpquartly.Attributes.Add("onkeyup", "validate_gen(this,1,'Avg. Money  Should Be Numeric!!',0,1)");
            txt_unitsemiannual.Attributes.Add("onblur", "sum_value(this)");
            txt_unitsemiannual.Attributes.Add("onkeyup", "validate_gen(this,1,'Unit Should Be Numeric!!',0,1)");
            txt_acpsemiann.Attributes.Add("onblur", "sum_value(this)");
            txt_acpsemiann.Attributes.Add("onkeyup", "validate_gen(this,1,'Avg. Money  Should Be Numeric!!',0,1)");
            txt_unitannual.Attributes.Add("onblur", "sum_value(this)");
            txt_unitannual.Attributes.Add("onkeyup", "validate_gen(this,1,'Unit Should Be Numeric!!',0,1)");
            txt_acpann.Attributes.Add("onblur", "sum_value(this)");
            txt_acpann.Attributes.Add("onkeyup", "validate_gen(this,1,'Avg. Money  Should Be Numeric!!',0,1)");
            txt_unitonetime.Attributes.Add("onblur", "sum_value(this)");
            txt_unitonetime.Attributes.Add("onkeyup", "validate_gen(this,1,'Unit Should Be Numeric!!',0,1)");
            txt_acponce.Attributes.Add("onblur", "sum_value(this)");
            txt_acponce.Attributes.Add("onkeyup", "validate_gen(this,1,'Avg. Money  Should Be Numeric!!',0,1)");
            txt_unitsip.Attributes.Add("onblur", "sum_value(this)");
            txt_unitsip.Attributes.Add("onkeyup", "validate_gen(this,1,'Unit Should Be Numeric!!',0,1)");
            txt_aisip.Attributes.Add("onblur", "sum_value(this)");
            txt_aisip.Attributes.Add("onkeyup", "validate_gen(this,1,'Avg. Money  Should Be Numeric!!',0,1)");
            txt_unitfresh.Attributes.Add("onblur", "sum_value(this)");
            txt_unitfresh.Attributes.Add("onkeyup", "validate_gen(this,1,'Unit Should Be Numeric!!',0,1)");
            txt_aifresh.Attributes.Add("onblur", "sum_value(this)");
            txt_aifresh.Attributes.Add("onkeyup", "validate_gen(this,1,'Avg. Money  Should Be Numeric!!',0,1)");
            txt_unitchurned.Attributes.Add("onblur", "sum_value(this)");
            txt_unitchurned.Attributes.Add("onkeyup", "validate_gen(this,1,'Unit Should Be Numeric!!',0,1)");
            txt_aichurned.Attributes.Add("onblur", "sum_value(this)");
            txt_aichurned.Attributes.Add("onkeyup", "validate_gen(this,1,'Avg. Money  Should Be Numeric!!',0,1)");
            txt_newclients.Attributes.Add("onkeydown", "return false;");
            txt_grossearning.Attributes.Add("onkeydown", "return false;");
            txt_totalcalls.Attributes.Add("onkeydown", "return false;");
            txt_salesvisit.Attributes.Add("onkeydown", "return false;");
            txt_tcpann.Attributes.Add("onkeydown", "return false;");
            txt_tcpmonthly.Attributes.Add("onkeydown", "return false;");
            txt_tcponce.Attributes.Add("onkeydown", "return false;");
            txt_tcpquaterly.Attributes.Add("onkeydown", "return false;");
            txt_tcpsemiann.Attributes.Add("onkeydown", "return false;");
            txt_aapann.Attributes.Add("onkeydown", "return false;");
            txt_aapmonthly.Attributes.Add("onkeydown", "return false;");
            txt_aaponce.Attributes.Add("onkeydown", "return false;");
            txt_aapquaterly.Attributes.Add("onkeydown", "return false;");
            txt_aapsemiann.Attributes.Add("onkeydown", "return false;");
            txt_tapann.Attributes.Add("onkeydown", "return false;");
            txt_tapmonthly.Attributes.Add("onkeydown", "return false;");
            txt_taponce.Attributes.Add("onkeydown", "return false;");
            txt_tapsemiann.Attributes.Add("onkeydown", "return false;");
            txt_tapquarterly.Attributes.Add("onkeydown", "return false;");
            txt_tasip.Attributes.Add("onkeydown", "return false;");
            txt_tafresh.Attributes.Add("onkeydown", "return false;");
            txt_tachurned.Attributes.Add("onkeydown", "return false;");
            txt_grandta.Attributes.Add("onkeydown", "return false;");
            txt_grandtap.Attributes.Add("onkeydown", "return false;");
            txt_grandtcp.Attributes.Add("onkeydown", "return false;");
        }
        protected void grd_target_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            FillGrid();
        }
        string ICallbackEventHandler.GetCallbackResult()
        {
            return data;
        }
        void ICallbackEventHandler.RaiseCallbackEvent(string eventArgument)
        {
            string id = eventArgument.ToString();
            string[] idList = id.Split('~');
            int NoOfRecord = 0;
            if (idList[0] == "Save")
            {
                // string EndDate = objConverter.DateConverter(idList[58], "mm/dd/yyyy");
                string EndDate = idList[58].ToString();
                if (idList[60] != "")
                {
                    NoOfRecord = oDBEngine.InsurtFieldValue("tbl_targetschemes", "tgt_masterid,tgt_descripition, tgt_periodicity, tgt_newclients, tgt_brokerage_gross, tgt_brokerage_eq, tgt_brokerage_fo, tgt_brokerage_com, tgt_totalcalls, tgt_newcalls, tgt_coldcalls, tgt_salesvisit_ratio_call, tgt_sales_ratio_call, tgt_salesvisit_total, tgt_newvisit_total, tgt_selfvisit_ratio_sales, tgt_sales_ratio_sales, tgt_Retails, tgt_HNI, tgt_Institution, tgt_unitmonthly, tgt_acpmonthly, tgt_tcpmonthly, tgt_aapmonthly, tgt_tapmonthly, tgt_unitqtrly, tgt_acpqtrly, tgt_tcpqtrly, tgt_aapqtrly, tgt_tapqtrly, tgt_unitsann, tgt_acpsann, tgt_tcpsann, tgt_aapsann, tgt_tapsann, tgt_unitann, tgt_acpann, tgt_tcpann, tgt_aapann, tgt_tapann, tgt_unitonce, tgt_acponce, tgt_tcponce, tgt_aaponce, tgt_taponce, tgt_grandtcp, tgt_grandtap, tgt_unitsip, tgt_aisip, tgt_tasip, tgt_unitfresh, tgt_aifresh, tgt_tafresh, tgt_unitchurn, tgt_aichurn, tgt_tachurn,tgt_grandta, tgt_effectivedate,tgt_enddate,CreateDate,CreateUser", "'" + idList[60] + "','" + idList[1] + "','" + idList[2] + "','" + idList[3] + "','" + idList[4] + "','" + idList[5] + "','" + idList[6] + "','" + idList[7] + "','" + idList[8] + "','" + idList[9] + "','" + idList[10] + "','" + idList[11] + "','" + idList[12] + "','" + idList[13] + "','" + idList[14] + "','" + idList[15] + "','" + idList[16] + "','" + idList[17] + "','" + idList[18] + "','" + idList[19] + "','" + idList[20] + "','" + idList[21] + "','" + idList[22] + "','" + idList[23] + "','" + idList[24] + "','" + idList[25] + "','" + idList[26] + "','" + idList[27] + "','" + idList[28] + "','" + idList[29] + "','" + idList[30] + "','" + idList[31] + "','" + idList[32] + "','" + idList[33] + "','" + idList[34] + "','" + idList[35] + "','" + idList[36] + "','" + idList[37] + "','" + idList[38] + "','" + idList[39] + "','" + idList[40] + "','" + idList[41] + "','" + idList[42] + "','" + idList[43] + "','" + idList[44] + "','" + idList[45] + "','" + idList[46] + "','" + idList[47] + "','" + idList[48] + "','" + idList[49] + "','" + idList[50] + "','" + idList[51] + "','" + idList[52] + "','" + idList[53] + "','" + idList[54] + "','" + idList[55] + "','" + idList[56] + "','" + idList[57] + "','" + EndDate + "','" + oDBEngine.GetDate().ToString() + "','" + Session["userid"].ToString() + "'");
                    //NoOfRecord = oDBEngine.SetFieldValue("tbl_targetschemes", "tgt_enddate='" + idList[57] + "',LastModifyDate='" + oDBEngine.GetDate().ToString() + "',LastModifyUser='" + Session["userid"].ToString() + "'", " tgt_id='" + idList[59] + "'");
                }
                else
                {
                    if (idList[59] == "")
                    {
                        string masterID = "";
                        string[,] marsterID1 = oDBEngine.GetFieldValue("tbl_targetschemes", "MAX(tgt_id)+1", null, 1);
                        if (marsterID1[0, 0] != "n")
                        {
                            masterID = marsterID1[0, 0];
                        }
                        if (masterID == "")
                        {
                            masterID = "0";
                        }
                        NoOfRecord = oDBEngine.InsurtFieldValue("tbl_targetschemes", "tgt_masterid,tgt_descripition, tgt_periodicity, tgt_newclients, tgt_brokerage_gross, tgt_brokerage_eq, tgt_brokerage_fo, tgt_brokerage_com, tgt_totalcalls, tgt_newcalls, tgt_coldcalls, tgt_salesvisit_ratio_call, tgt_sales_ratio_call, tgt_salesvisit_total, tgt_newvisit_total, tgt_selfvisit_ratio_sales, tgt_sales_ratio_sales, tgt_Retails, tgt_HNI, tgt_Institution, tgt_unitmonthly, tgt_acpmonthly, tgt_tcpmonthly, tgt_aapmonthly, tgt_tapmonthly, tgt_unitqtrly, tgt_acpqtrly, tgt_tcpqtrly, tgt_aapqtrly, tgt_tapqtrly, tgt_unitsann, tgt_acpsann, tgt_tcpsann, tgt_aapsann, tgt_tapsann, tgt_unitann, tgt_acpann, tgt_tcpann, tgt_aapann, tgt_tapann, tgt_unitonce, tgt_acponce, tgt_tcponce, tgt_aaponce, tgt_taponce, tgt_grandtcp, tgt_grandtap, tgt_unitsip, tgt_aisip, tgt_tasip, tgt_unitfresh, tgt_aifresh, tgt_tafresh, tgt_unitchurn, tgt_aichurn, tgt_tachurn,tgt_grandta, tgt_effectivedate,tgt_enddate,CreateDate,CreateUser", "'" + masterID + "','" + idList[1] + "','" + idList[2] + "','" + idList[3] + "','" + idList[4] + "','" + idList[5] + "','" + idList[6] + "','" + idList[7] + "','" + idList[8] + "','" + idList[9] + "','" + idList[10] + "','" + idList[11] + "','" + idList[12] + "','" + idList[13] + "','" + idList[14] + "','" + idList[15] + "','" + idList[16] + "','" + idList[17] + "','" + idList[18] + "','" + idList[19] + "','" + idList[20] + "','" + idList[21] + "','" + idList[22] + "','" + idList[23] + "','" + idList[24] + "','" + idList[25] + "','" + idList[26] + "','" + idList[27] + "','" + idList[28] + "','" + idList[29] + "','" + idList[30] + "','" + idList[31] + "','" + idList[32] + "','" + idList[33] + "','" + idList[34] + "','" + idList[35] + "','" + idList[36] + "','" + idList[37] + "','" + idList[38] + "','" + idList[39] + "','" + idList[40] + "','" + idList[41] + "','" + idList[42] + "','" + idList[43] + "','" + idList[44] + "','" + idList[45] + "','" + idList[46] + "','" + idList[47] + "','" + idList[48] + "','" + idList[49] + "','" + idList[50] + "','" + idList[51] + "','" + idList[52] + "','" + idList[53] + "','" + idList[54] + "','" + idList[55] + "','" + idList[56] + "','" + idList[57] + "','" + EndDate + "','" + oDBEngine.GetDate().ToString() + "','" + Session["userid"].ToString() + "'");
                    }
                    else
                    {
                        NoOfRecord = oDBEngine.SetFieldValue("tbl_targetschemes", "tgt_descripition='" + idList[1] + "',tgt_periodicity='" + idList[2] + "',tgt_newclients=" + idList[3] + ",tgt_brokerage_gross=" + idList[4] + ",tgt_brokerage_eq=" + idList[5] + ",tgt_brokerage_fo=" + idList[6] + ",tgt_brokerage_com=" + idList[7] + ",tgt_totalcalls=" + idList[8] + ",tgt_newcalls=" + idList[9] + ",tgt_coldcalls=" + idList[10] + ",tgt_salesvisit_ratio_call=" + idList[11] + ",tgt_sales_ratio_call=" + idList[12] + ",tgt_salesvisit_total=" + idList[13] + ",tgt_newvisit_total=" + idList[14] + ",tgt_sales_ratio_sales=" + idList[16] + ",tgt_selfvisit_ratio_sales=" + idList[15] + " ,tgt_Retails=" + idList[17] + ", tgt_HNI=" + idList[18] + ", tgt_Institution=" + idList[19] + ", tgt_unitmonthly=" + idList[20] + ", tgt_acpmonthly=" + idList[21] + ", tgt_tcpmonthly=" + idList[22] + ", tgt_aapmonthly=" + idList[23] + ", tgt_tapmonthly=" + idList[24] + ", tgt_unitqtrly=" + idList[25] + ", tgt_acpqtrly=" + idList[26] + ", tgt_tcpqtrly=" + idList[27] + ", tgt_aapqtrly=" + idList[28] + ", tgt_tapqtrly=" + idList[29] + ", tgt_unitsann=" + idList[30] + ", tgt_acpsann=" + idList[31] + ", tgt_tcpsann=" + idList[32] + ", tgt_aapsann=" + idList[33] + ", tgt_tapsann=" + idList[34] + ", tgt_unitann=" + idList[35] + ", tgt_acpann=" + idList[36] + ", tgt_tcpann=" + idList[37] + ", tgt_aapann=" + idList[38] + ", tgt_tapann=" + idList[39] + ", tgt_unitonce=" + idList[40] + ", tgt_acponce=" + idList[41] + ", tgt_tcponce=" + idList[42] + ", tgt_aaponce=" + idList[43] + ", tgt_taponce=" + idList[44] + ", tgt_grandtcp=" + idList[45] + ", tgt_grandtap=" + idList[46] + ", tgt_unitsip=" + idList[47] + ", tgt_aisip=" + idList[48] + ", tgt_tasip=" + idList[49] + ", tgt_unitfresh=" + idList[50] + ", tgt_aifresh=" + idList[51] + ", tgt_tafresh=" + idList[52] + ", tgt_unitchurn=" + idList[53] + ", tgt_aichurn=" + idList[54] + ", tgt_tachurn=" + idList[55] + ", tgt_grandta=" + idList[56] + ",tgt_effectivedate='" + idList[57] + "',tgt_enddate='" + EndDate + "',LastModifyDate='" + oDBEngine.GetDate().ToString() + "',LastModifyUser='" + Session["userid"].ToString() + "'", " tgt_id='" + idList[59] + "'");
                    }
                }
                if (NoOfRecord > 0)
                {
                    data = "Save~Y";
                }
                else
                {
                    data = "Save~N";
                }
            }
            if (idList[0] == "Edit")
            {
                DataTable DtTemp = oDBEngine.GetDataTable("tbl_targetschemes", "*", " tgt_id='" + idList[1] + "'");
                if (DtTemp.Rows.Count > 0)
                {
                    txt_schemename.Text = DtTemp.Rows[0]["tgt_descripition"].ToString();
                    lst_periodicity.SelectedValue = DtTemp.Rows[0]["tgt_periodicity"].ToString();
                    txt_newclients.Text = DtTemp.Rows[0]["tgt_newclients"].ToString();
                    txt_grossearning.Text = DtTemp.Rows[0]["tgt_brokerage_gross"].ToString();
                    txt_equities.Text = DtTemp.Rows[0]["tgt_brokerage_eq"].ToString();
                    txt_derivatives.Text = DtTemp.Rows[0]["tgt_brokerage_fo"].ToString();
                    txt_commodities.Text = DtTemp.Rows[0]["tgt_brokerage_com"].ToString();
                    txt_totalcalls.Text = DtTemp.Rows[0]["tgt_totalcalls"].ToString();
                    txt_newcalls.Text = DtTemp.Rows[0]["tgt_newcalls"].ToString();
                    txt_coldcalls.Text = DtTemp.Rows[0]["tgt_coldcalls"].ToString();
                    txt_salesvisitratio.Text = DtTemp.Rows[0]["tgt_salesvisit_ratio_call"].ToString();
                    txt_salesratio.Text = DtTemp.Rows[0]["tgt_sales_ratio_call"].ToString();
                    txt_salesvisit.Text = DtTemp.Rows[0]["tgt_salesvisit_total"].ToString();
                    txt_newsalesvisit.Text = DtTemp.Rows[0]["tgt_newvisit_total"].ToString();
                    txt_selfsalesvisit.Text = DtTemp.Rows[0]["tgt_selfvisit_ratio_sales"].ToString();
                    txt_sales_salesratio.Text = DtTemp.Rows[0]["tgt_sales_ratio_sales"].ToString();
                    txt_retails.Text = DtTemp.Rows[0]["tgt_Retails"].ToString();
                    txt_hni.Text = DtTemp.Rows[0]["tgt_HNI"].ToString();
                    txt_institution.Text = DtTemp.Rows[0]["tgt_Institution"].ToString();
                    txt_unitmonthly.Text = DtTemp.Rows[0]["tgt_unitmonthly"].ToString();
                    txt_acpmonthly.Text = DtTemp.Rows[0]["tgt_acpmonthly"].ToString();
                    txt_tcpmonthly.Text = DtTemp.Rows[0]["tgt_tcpmonthly"].ToString();
                    txt_aapmonthly.Text = DtTemp.Rows[0]["tgt_aapmonthly"].ToString();
                    txt_tapmonthly.Text = DtTemp.Rows[0]["tgt_tapmonthly"].ToString();
                    txt_unitquarterly.Text = DtTemp.Rows[0]["tgt_unitqtrly"].ToString();
                    txt_acpquartly.Text = DtTemp.Rows[0]["tgt_acpqtrly"].ToString();
                    txt_tcpquaterly.Text = DtTemp.Rows[0]["tgt_tcpqtrly"].ToString();
                    txt_aapquaterly.Text = DtTemp.Rows[0]["tgt_aapqtrly"].ToString();
                    txt_tapquarterly.Text = DtTemp.Rows[0]["tgt_tapqtrly"].ToString();
                    txt_unitsemiannual.Text = DtTemp.Rows[0]["tgt_unitsann"].ToString();
                    txt_acpsemiann.Text = DtTemp.Rows[0]["tgt_acpsann"].ToString();
                    txt_tcpsemiann.Text = DtTemp.Rows[0]["tgt_tcpsann"].ToString();
                    txt_aapsemiann.Text = DtTemp.Rows[0]["tgt_aapsann"].ToString();
                    txt_tapsemiann.Text = DtTemp.Rows[0]["tgt_tapsann"].ToString();
                    txt_unitannual.Text = DtTemp.Rows[0]["tgt_unitann"].ToString();
                    txt_acpann.Text = DtTemp.Rows[0]["tgt_acpann"].ToString();
                    txt_tcpann.Text = DtTemp.Rows[0]["tgt_tcpann"].ToString();
                    txt_aapann.Text = DtTemp.Rows[0]["tgt_aapann"].ToString();
                    txt_tapann.Text = DtTemp.Rows[0]["tgt_tapann"].ToString();
                    txt_unitonetime.Text = DtTemp.Rows[0]["tgt_unitonce"].ToString();
                    txt_acponce.Text = DtTemp.Rows[0]["tgt_acponce"].ToString();
                    txt_tcponce.Text = DtTemp.Rows[0]["tgt_tcponce"].ToString();
                    txt_aaponce.Text = DtTemp.Rows[0]["tgt_aaponce"].ToString();
                    txt_taponce.Text = DtTemp.Rows[0]["tgt_taponce"].ToString();
                    txt_grandtcp.Text = DtTemp.Rows[0]["tgt_grandtcp"].ToString();
                    txt_grandtap.Text = DtTemp.Rows[0]["tgt_grandtap"].ToString();
                    txt_unitsip.Text = DtTemp.Rows[0]["tgt_unitsip"].ToString();
                    txt_aisip.Text = DtTemp.Rows[0]["tgt_aisip"].ToString();
                    txt_tasip.Text = DtTemp.Rows[0]["tgt_tasip"].ToString();
                    txt_unitfresh.Text = DtTemp.Rows[0]["tgt_unitfresh"].ToString();
                    txt_aifresh.Text = DtTemp.Rows[0]["tgt_aifresh"].ToString();
                    txt_tafresh.Text = DtTemp.Rows[0]["tgt_tafresh"].ToString();
                    txt_unitchurned.Text = DtTemp.Rows[0]["tgt_unitchurn"].ToString();
                    txt_aichurned.Text = DtTemp.Rows[0]["tgt_aichurn"].ToString();
                    txt_tachurned.Text = DtTemp.Rows[0]["tgt_tachurn"].ToString();
                    txt_grandta.Text = DtTemp.Rows[0]["tgt_grandta"].ToString();
                    TxtEffectiveDate.Text = DtTemp.Rows[0]["tgt_effectivedate"].ToString();
                    // TxtEndDate.Text = objConverter.DateConverter_d_m_y(DtTemp.Rows[0]["tgt_enddate"].ToString());
                    TxtEndDate.Value = Convert.ToDateTime(DtTemp.Rows[0]["tgt_enddate"].ToString());
                    data = "Edit" + "~" + txt_schemename.Text + "~" + lst_periodicity.SelectedValue + "~" + txt_newclients.Text + "~" + txt_grossearning.Text + "~" + txt_equities.Text + "~" + txt_derivatives.Text + "~" + txt_commodities.Text + "~" + txt_totalcalls.Text + "~" + txt_newcalls.Text + "~" + txt_coldcalls.Text + "~" + txt_salesvisitratio.Text + "~" + txt_salesratio.Text + "~" + txt_salesvisit.Text + "~" + txt_newsalesvisit.Text + "~" + txt_selfsalesvisit.Text + "~" + txt_sales_salesratio.Text + "~" + txt_retails.Text + "~" + txt_hni.Text + "~" + txt_institution.Text + "~" + txt_unitmonthly.Text + "~" + txt_acpmonthly.Text + "~" + txt_tcpmonthly.Text + "~" + txt_aapmonthly.Text + "~" + txt_tapmonthly.Text + "~" + txt_unitquarterly.Text + "~" + txt_acpquartly.Text + "~" + txt_tcpquaterly.Text + "~" + txt_aapquaterly.Text + "~" + txt_tapquarterly.Text + "~" + txt_unitsemiannual.Text + "~" + txt_acpsemiann.Text + "~" + txt_tcpsemiann.Text + "~" + txt_aapsemiann.Text + "~" + txt_tapsemiann.Text + "~" + txt_unitannual.Text + "~" + txt_acpann.Text + "~" + txt_tcpann.Text + "~" + txt_aapann.Text + "~" + txt_tapann.Text + "~" + txt_unitonetime.Text + "~" + txt_acponce.Text + "~" + txt_tcponce.Text + "~" + txt_aaponce.Text + "~" + txt_taponce.Text + "~" + txt_grandtcp.Text + "~" + txt_grandtap.Text + "~" + txt_unitsip.Text + "~" + txt_aisip.Text + "~" + txt_tasip.Text + "~" + txt_unitfresh.Text + "~" + txt_aifresh.Text + "~" + txt_tafresh.Text + "~" + txt_unitchurned.Text + "~" + txt_aichurned.Text + "~" + txt_tachurned.Text + "~" + txt_grandta.Text + "~" + TxtEffectiveDate.Text + "~" + TxtEndDate.Value.ToString() + "~" + idList[1];
                }
                else
                {
                    data = "Edit~N";
                }
            }
            if (idList[0] == "Revise")
            {
                DataTable DtTemp = oDBEngine.GetDataTable("tbl_targetschemes", "*", " tgt_id='" + idList[1] + "'");
                if (DtTemp.Rows.Count > 0)
                {
                    string enddate = DtTemp.Rows[0]["tgt_enddate"].ToString();
                    DateTime Enddate1 = Convert.ToDateTime(enddate).AddDays(-1);
                    string EndDate = objConverter.DateConverterFromMMtoDD(Enddate1.ToString(), "mm/dd/yyyy").ToString();
                    txt_schemename.Text = DtTemp.Rows[0]["tgt_descripition"].ToString();
                    lst_periodicity.SelectedValue = DtTemp.Rows[0]["tgt_periodicity"].ToString();
                    txt_newclients.Text = DtTemp.Rows[0]["tgt_newclients"].ToString();
                    txt_grossearning.Text = DtTemp.Rows[0]["tgt_brokerage_gross"].ToString();
                    txt_equities.Text = DtTemp.Rows[0]["tgt_brokerage_eq"].ToString();
                    txt_derivatives.Text = DtTemp.Rows[0]["tgt_brokerage_fo"].ToString();
                    txt_commodities.Text = DtTemp.Rows[0]["tgt_brokerage_com"].ToString();
                    txt_totalcalls.Text = DtTemp.Rows[0]["tgt_totalcalls"].ToString();
                    txt_newcalls.Text = DtTemp.Rows[0]["tgt_newcalls"].ToString();
                    txt_coldcalls.Text = DtTemp.Rows[0]["tgt_coldcalls"].ToString();
                    txt_salesvisitratio.Text = DtTemp.Rows[0]["tgt_salesvisit_ratio_call"].ToString();
                    txt_salesratio.Text = DtTemp.Rows[0]["tgt_sales_ratio_call"].ToString();
                    txt_salesvisit.Text = DtTemp.Rows[0]["tgt_salesvisit_total"].ToString();
                    txt_newsalesvisit.Text = DtTemp.Rows[0]["tgt_newvisit_total"].ToString();
                    txt_selfsalesvisit.Text = DtTemp.Rows[0]["tgt_selfvisit_ratio_sales"].ToString();
                    txt_sales_salesratio.Text = DtTemp.Rows[0]["tgt_sales_ratio_sales"].ToString();
                    txt_retails.Text = DtTemp.Rows[0]["tgt_Retails"].ToString();
                    txt_hni.Text = DtTemp.Rows[0]["tgt_HNI"].ToString();
                    txt_institution.Text = DtTemp.Rows[0]["tgt_Institution"].ToString();
                    txt_unitmonthly.Text = DtTemp.Rows[0]["tgt_unitmonthly"].ToString();
                    txt_acpmonthly.Text = DtTemp.Rows[0]["tgt_acpmonthly"].ToString();
                    txt_tcpmonthly.Text = DtTemp.Rows[0]["tgt_tcpmonthly"].ToString();
                    txt_aapmonthly.Text = DtTemp.Rows[0]["tgt_aapmonthly"].ToString();
                    txt_tapmonthly.Text = DtTemp.Rows[0]["tgt_tapmonthly"].ToString();
                    txt_unitquarterly.Text = DtTemp.Rows[0]["tgt_unitqtrly"].ToString();
                    txt_acpquartly.Text = DtTemp.Rows[0]["tgt_acpqtrly"].ToString();
                    txt_tcpquaterly.Text = DtTemp.Rows[0]["tgt_tcpqtrly"].ToString();
                    txt_aapquaterly.Text = DtTemp.Rows[0]["tgt_aapqtrly"].ToString();
                    txt_tapquarterly.Text = DtTemp.Rows[0]["tgt_tapqtrly"].ToString();
                    txt_unitsemiannual.Text = DtTemp.Rows[0]["tgt_unitsann"].ToString();
                    txt_acpsemiann.Text = DtTemp.Rows[0]["tgt_acpsann"].ToString();
                    txt_tcpsemiann.Text = DtTemp.Rows[0]["tgt_tcpsann"].ToString();
                    txt_aapsemiann.Text = DtTemp.Rows[0]["tgt_aapsann"].ToString();
                    txt_tapsemiann.Text = DtTemp.Rows[0]["tgt_tapsann"].ToString();
                    txt_unitannual.Text = DtTemp.Rows[0]["tgt_unitann"].ToString();
                    txt_acpann.Text = DtTemp.Rows[0]["tgt_acpann"].ToString();
                    txt_tcpann.Text = DtTemp.Rows[0]["tgt_tcpann"].ToString();
                    txt_aapann.Text = DtTemp.Rows[0]["tgt_aapann"].ToString();
                    txt_tapann.Text = DtTemp.Rows[0]["tgt_tapann"].ToString();
                    txt_unitonetime.Text = DtTemp.Rows[0]["tgt_unitonce"].ToString();
                    txt_acponce.Text = DtTemp.Rows[0]["tgt_acponce"].ToString();
                    txt_tcponce.Text = DtTemp.Rows[0]["tgt_tcponce"].ToString();
                    txt_aaponce.Text = DtTemp.Rows[0]["tgt_aaponce"].ToString();
                    txt_taponce.Text = DtTemp.Rows[0]["tgt_taponce"].ToString();
                    txt_grandtcp.Text = DtTemp.Rows[0]["tgt_grandtcp"].ToString();
                    txt_grandtap.Text = DtTemp.Rows[0]["tgt_grandtap"].ToString();
                    txt_unitsip.Text = DtTemp.Rows[0]["tgt_unitsip"].ToString();
                    txt_aisip.Text = DtTemp.Rows[0]["tgt_aisip"].ToString();
                    txt_tasip.Text = DtTemp.Rows[0]["tgt_tasip"].ToString();
                    txt_unitfresh.Text = DtTemp.Rows[0]["tgt_unitfresh"].ToString();
                    txt_aifresh.Text = DtTemp.Rows[0]["tgt_aifresh"].ToString();
                    txt_tafresh.Text = DtTemp.Rows[0]["tgt_tafresh"].ToString();
                    txt_unitchurned.Text = DtTemp.Rows[0]["tgt_unitchurn"].ToString();
                    txt_aichurned.Text = DtTemp.Rows[0]["tgt_aichurn"].ToString();
                    txt_tachurned.Text = DtTemp.Rows[0]["tgt_tachurn"].ToString();
                    txt_grandta.Text = DtTemp.Rows[0]["tgt_grandta"].ToString();
                    data = "Revise" + "~" + txt_schemename.Text + "~" + lst_periodicity.SelectedValue + "~" + txt_newclients.Text + "~" + txt_grossearning.Text + "~" + txt_equities.Text + "~" + txt_derivatives.Text + "~" + txt_commodities.Text + "~" + txt_totalcalls.Text + "~" + txt_newcalls.Text + "~" + txt_coldcalls.Text + "~" + txt_salesvisitratio.Text + "~" + txt_salesratio.Text + "~" + txt_salesvisit.Text + "~" + txt_newsalesvisit.Text + "~" + txt_selfsalesvisit.Text + "~" + txt_sales_salesratio.Text + "~" + txt_retails.Text + "~" + txt_hni.Text + "~" + txt_institution.Text + "~" + txt_unitmonthly.Text + "~" + txt_acpmonthly.Text + "~" + txt_tcpmonthly.Text + "~" + txt_aapmonthly.Text + "~" + txt_tapmonthly.Text + "~" + txt_unitquarterly.Text + "~" + txt_acpquartly.Text + "~" + txt_tcpquaterly.Text + "~" + txt_aapquaterly.Text + "~" + txt_tapquarterly.Text + "~" + txt_unitsemiannual.Text + "~" + txt_acpsemiann.Text + "~" + txt_tcpsemiann.Text + "~" + txt_aapsemiann.Text + "~" + txt_tapsemiann.Text + "~" + txt_unitannual.Text + "~" + txt_acpann.Text + "~" + txt_tcpann.Text + "~" + txt_aapann.Text + "~" + txt_tapann.Text + "~" + txt_unitonetime.Text + "~" + txt_acponce.Text + "~" + txt_tcponce.Text + "~" + txt_aaponce.Text + "~" + txt_taponce.Text + "~" + txt_grandtcp.Text + "~" + txt_grandtap.Text + "~" + txt_unitsip.Text + "~" + txt_aisip.Text + "~" + txt_tasip.Text + "~" + txt_unitfresh.Text + "~" + txt_aifresh.Text + "~" + txt_tafresh.Text + "~" + txt_unitchurned.Text + "~" + txt_aichurned.Text + "~" + txt_tachurned.Text + "~" + txt_grandta.Text + "~" + EndDate + "~" + idList[1];
                }
                else
                {
                    data = "Revise~N";
                }
            }

        }
        protected void grdtargetdetails_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            string param = e.Parameters.ToString();
            FillGridDetails(param);
        }

    }
}