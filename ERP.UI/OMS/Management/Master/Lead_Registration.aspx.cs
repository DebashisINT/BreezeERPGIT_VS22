using System;
using System.Data;
using System.Web;
using System.Web.UI;
////using DevExpress.Web.ASPxClasses;
//using DevExpress.Web;
using DevExpress.Web;
using System.Configuration;
using BusinessLogicLayer;

namespace ERP.OMS.Management.Master
{
    public partial class management_Master_Lead_Registration : ERP.OMS.ViewState_class.VSPage
    {
        string DocumentID = null;
        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);MULTI
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        protected void Page_Load(object sender, EventArgs e)
        {

            //------- For Read Only User in SQL Datasource Connection String   Start-----------------
            SqlComp.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            SqlExchangeSegment.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            SqlProfessional.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            if (HttpContext.Current.Session["EntryProfileType"] != null)
            {
                if (Convert.ToString(HttpContext.Current.Session["EntryProfileType"]) == "R")
                {
                    //Sqlmembership.ConnectionString = ConfigurationSettings.AppSettings["DBReadOnlyConnection"];MULTI
                    //Sqlstatutory.ConnectionString = ConfigurationSettings.AppSettings["DBReadOnlyConnection"];MULTI
                    //SqlExchange.ConnectionString = ConfigurationSettings.AppSettings["DBReadOnlyConnection"];MULTI
                    Sqlmembership.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                    Sqlstatutory.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                    SqlExchange.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);

                }
                else
                {
                    //Sqlmembership.ConnectionString = ConfigurationSettings.AppSettings["DBConnectionDefault"];MULTI
                    //Sqlstatutory.ConnectionString = ConfigurationSettings.AppSettings["DBConnectionDefault"];MULTI
                    //SqlExchange.ConnectionString = ConfigurationSettings.AppSettings["DBConnectionDefault"];MULTI
                    Sqlmembership.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                    Sqlstatutory.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                    SqlExchange.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                }
            }

            //------- For Read Only User in SQL Datasource Connection String   End-----------------


            //if (HttpContext.Current.Session["userid"] == null)
            //{
            //   //Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            //}
            ////this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");

            if (Request.QueryString["formtype"] != null)
            {
                DocumentID = Session["InternalId"].ToString();
                // DisabledTabPage();
            }
            else
                try
                {
                    DocumentID = HttpContext.Current.Session["KeyVal_InternalID"].ToString();
                }
                catch (Exception ex)
                { }
            Session["KeyVal_InternalID"] = DocumentID;
        }
        protected void gridRegisStatutory_RowValidating(object sender, DevExpress.Web.Data.ASPxDataValidationEventArgs e)
        {
            string companyID = Session["KeyVal_InternalID"].ToString();
            string exchangeId = "";
            string eid = "";
            try
            {
                exchangeId = e.NewValues["crg_type"].ToString();
            }
            catch
            {
            }
            if (Session["check"] != null)
            {
                Session["check"] = null;
                string[,] id = oDBEngine.GetFieldValue("tbl_master_contactRegistration", "crg_cntId", "crg_cntId='" + companyID + "' and crg_type='" + exchangeId + "' ", 1);

                if (id[0, 0] != "n")
                {
                    eid = id[0, 0];
                }
                if (eid == "")
                {
                }
                else
                {
                    e.RowError = "This Type Already Exists";
                    return;
                }

            }
            else
            {
                string depositoryId1 = "";
                try
                {
                    depositoryId1 = e.OldValues["crg_type"].ToString();
                }
                catch
                {

                }
                if (exchangeId == depositoryId1)
                {
                }
                else
                {
                    string[,] id = oDBEngine.GetFieldValue("tbl_master_contactRegistration", "crg_cntId", "crg_cntId='" + companyID + "' and crg_type='" + exchangeId + "' ", 1);
                    if (id[0, 0] != "n")
                    {
                        eid = id[0, 0];
                    }
                    if (eid == "")
                    {
                    }
                    else
                    {
                        e.RowError = "This Type Already Exists";
                        return;
                    }
                }
            }
        }
        protected void gridRegisStatutory_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            Session["check"] = "a";
        }
        protected void gridExchange_CellEditorInitialize(object sender, DevExpress.Web.ASPxGridViewEditorEventArgs e)
        {
            if (e.Column.FieldName == "crg_exchange1")
            {
                if (e.KeyValue != null)
                {
                    object val = gridExchange.GetRowValuesByKeyValue(e.KeyValue, "crg_company1");
                    if (val == DBNull.Value) return;
                    string country = (string)val;
                    ASPxComboBox combo = e.Editor as ASPxComboBox;
                    FillStateCombo(combo, country);

                    combo.Callback += new CallbackEventHandlerBase(cmbState_OnCallback);
                }
                else
                {

                    object val = gridExchange.GetRowValues(0, "crg_company1");
                    if (val != null)
                    {

                        string country = (string)val;
                        ASPxComboBox combo = e.Editor as ASPxComboBox;
                        FillStateCombo(combo, country);

                        combo.Callback += new CallbackEventHandlerBase(cmbState_OnCallback);
                    }
                    else
                    {

                        string country = "1";
                        ASPxComboBox combo = e.Editor as ASPxComboBox;
                        FillStateCombo(combo, country);

                        combo.Callback += new CallbackEventHandlerBase(cmbState_OnCallback);
                    }
                }
            }
        }
        protected void FillStateCombo(ASPxComboBox cmb, string country)
        {

            string[,] state = GetState(country);
            cmb.Items.Clear();

            for (int i = 0; i < state.GetLength(0); i++)
            {
                cmb.Items.Add(state[i, 1], state[i, 0]);
            }
        }
        string[,] GetState(string country)
        {


            SqlExchangeSegment.SelectParameters[0].DefaultValue = country.ToString();
            DataView view = (DataView)SqlExchangeSegment.Select(DataSourceSelectArguments.Empty);
            string[,] DATA = new string[view.Count, 2];
            for (int i = 0; i < view.Count; i++)
            {
                DATA[i, 0] = view[i][0].ToString();
                DATA[i, 1] = view[i][1].ToString();
            }
            return DATA;


        }
        private void cmbState_OnCallback(object source, CallbackEventArgsBase e)
        {
            FillStateCombo(source as ASPxComboBox, Convert.ToString(e.Parameter));
        }
        protected void gridExchange_RowValidating(object sender, DevExpress.Web.Data.ASPxDataValidationEventArgs e)
        {
            string companyID = Session["KeyVal_InternalID"].ToString();
            string Ucc = "";
            string eid = "";
            string eid1 = "";
            string company = "";
            string exchange = "";
            try
            {
                Ucc = e.NewValues["crg_tcode"].ToString();
                company = e.NewValues["crg_company1"].ToString();
                exchange = e.NewValues["crg_exchange1"].ToString();
            }
            catch
            {

            }
            if (Ucc == "")
            {
                return;
            }
            if (Session["check"] != null)
            {
                Session["check"] = null;
                string[,] id = oDBEngine.GetFieldValue("tbl_master_contactExchange", "crg_internalId,crg_cntId", "crg_company='" + company + "' and crg_exchange='" + exchange + "' and crg_tcode='" + Ucc + "'", 2);
                string[,] exch = oDBEngine.GetFieldValue("tbl_master_contactExchange", "crg_exchange", " crg_company='" + company + "' and crg_exchange='" + exchange + "' and crg_cntID='" + companyID + "'", 1);
                if (exch[0, 0] != "n")
                {
                    string[] sname = exch[0, 0].Split('-');
                    string[,] shortName = oDBEngine.GetFieldValue("tbl_master_exchange", "exh_shortName", " exh_shortName='" + sname[0].Trim() + "'", 1);
                    if (shortName[0, 0] != "n")
                    {
                        e.RowError = "This Combination Already Exists";
                        return;
                    }
                }
                if (id[0, 0] != "n")
                {
                    eid = id[0, 1];
                    if (eid != "")
                    {
                        e.RowError = "This UCC Already Exists";
                        return;
                    }
                }

            }
            else
            {
                string UCC1 = "";
                string company1 = "";
                string exchange1 = "";
                try
                {
                    UCC1 = e.OldValues["crg_tcode"].ToString();
                    company1 = e.OldValues["crg_company1"].ToString();
                    exchange1 = e.OldValues["crg_exchange1"].ToString();
                }
                catch
                {

                }
                if (Ucc == UCC1)
                {
                }
                else
                {
                    string keyVal = e.Keys[0].ToString();
                    string[,] id = oDBEngine.GetFieldValue("tbl_master_contactExchange", "crg_internalId,crg_cntId", "crg_company='" + company + "' and crg_exchange='" + exchange + "' and crg_tcode='" + Ucc + "'", 2);
                    string[,] exch = oDBEngine.GetFieldValue("tbl_master_contactExchange", "crg_exchange,crg_internalId", " crg_company='" + company + "' and crg_exchange='" + exchange + "' and crg_cntID='" + companyID + "'", 2);
                    if (exch[0, 0] != "n")
                    {
                        if (keyVal != exch[0, 1].ToString())
                        {
                            string[] sname = exch[0, 0].Split('-');
                            string[,] shortName = oDBEngine.GetFieldValue("tbl_master_exchange", "exh_shortName", " exh_shortName='" + sname[0].Trim() + "'", 1);
                            if (shortName[0, 0] != "n")
                            {
                                e.RowError = "This Combination Already Exists";
                                return;
                            }
                        }
                    }
                    if (id[0, 0] != "n")
                    {
                        eid = id[0, 1];
                        if (eid != "")
                        {
                            e.RowError = "This UCC Already Exists";
                            return;
                        }
                    }
                }
            }
        }
        protected void gridExchange_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            Session["check"] = "a";
            string[,] id = oDBEngine.GetFieldValue("tbl_master_lead", "cnt_UCC", "cnt_internalId='" + Session["KeyVal_InternalID"].ToString() + "'", 1);
            if (id[0, 0] != "n")
            {
                e.NewValues["crg_tcode"] = id[0, 0];
            }
        }
    }
}