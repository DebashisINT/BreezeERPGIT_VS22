using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
//using DevExpress.Web.ASPxClasses;
//using DevExpress.Web.ASPxEditors;
//using DevExpress.Web.ASPxGridView;
//using DevExpress.Web.ASPxTabControl;
using DevExpress.Web;
using System.Configuration;
using ClsDropDownlistNameSpace;
using EntityLayer.CommonELS;
using System.Web.Services;
using System.Collections.Generic;
using BusinessLogicLayer;
using System.Data.SqlClient;
using System.Runtime.Serialization;
using Newtonsoft.Json.Linq;
using System.Globalization;

namespace ERP.OMS.Management.Master
{
    public partial class frm_currency_master : ERP.OMS.ViewState_class.VSPage //System.Web.UI.Page
    {
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]); MULTI
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        BusinessLogicLayer.Contact oContactGeneralBL = new BusinessLogicLayer.Contact();
        BusinessLogicLayer.Converter oConverter = new BusinessLogicLayer.Converter();
        BusinessLogicLayer.GenericStoreProcedure oGenericStoreProcedure;
        clsDropDownList clsdropdown = new clsDropDownList();
        CurrencyMasterBL cmbl = new CurrencyMasterBL();
        public Boolean edit=false;
        string[,] BaseCurrData;
        public Boolean fstSave=false;
        public Boolean checkexistance = false;

        protected void Page_PreInit(object sender, EventArgs e)
        {
            // Code  Added and Commented By Priti on 21122016 to add Convert.ToString instead of ToString()
            if (!IsPostBack)
            {
                //'http://localhost:2957/InfluxCRM/management/testProjectMainPage_employee.aspx'
                string sPath = Convert.ToString(HttpContext.Current.Request.Url);
                oDBEngine.Call_CheckPageaccessebility(sPath);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                // rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/Master/BranchAddEdit.aspx");

                if (!IsPostBack)
                {
                    if(Convert.ToString(Request.QueryString["id"]) != "ADD")
                    {
                        edit = true;
                        string Keyval = Convert.ToString(Request.QueryString["id"]);

                        BaseCurrData = oDBEngine.GetFieldValue("tbl_Master_CurrencyRateDateWise", "cmp_internalid,(select Currency_AlphaCode from Master_Currency where Currency_ID=BaseCurrency_ID) as BaseCurrency_Name,ConversionCurrency_ID,SalesRate,PurchaseRate,CONVERT(CHAR(23),CONVERT(DATETIME,ConversionDate,101),121) as ConversionDate,BaseCurrency_ID", "CRID=" + Keyval, 7);
                        //BaseCurrData = oDBEngine.GetFieldValue("tbl_Master_CurrencyRateDateWise", "cmp_internalid,(select Currency_AlphaCode from Master_Currency where Currency_ID=BaseCurrency_ID) as BaseCurrency_Name,ConversionCurrency_ID,SalesRate,PurchaseRate,CONVERT(VARCHAR(10), ConversionDate, 126) as ConversionDate,BaseCurrency_ID", "CRID=" + Keyval, 7);
                        HttpContext.Current.Session["KeyVal_InternalID"] = BaseCurrData[0, 0];

                    }
                    loadCompany();
                    //loadBaseCurrency();
                    ShowForm();
                    Session["ContactType"] = "Financer";
                    IsUdfpresent.Value = Convert.ToString(getUdfCount());
                    LoadCurrencyInHiddenField();
                }
               
            }
            catch { }
        }
        //protected void loadBaseCurrency()
        //{
        //    string[,] parent = oDBEngine.GetFieldValue("Master_Currency", "Currency_ID,Currency_AlphaCode", "Currency_AlphaCode like '%%'", 2);
        //    if (Convert.ToString(Request.QueryString["id"]) == "ADD")
        //    {
        //        if (parent[0, 0] != "n")
        //        {
        //            try
        //            {
        //                clsdropdown.AddDataToDropDownList(parent, DrpCurrency, true);
        //                //DrpCompany.SelectedValue = ContactData[0, 3];
        //                DrpCurrency.Items.Insert(0, new ListItem("None", "0"));
        //            }
        //            catch { }
        //        }
        //        else
        //        {
        //            DrpCurrency.Items.Insert(0, new ListItem("None", "0"));
        //        }
        //    }
        //    else
        //    {
        //        clsdropdown.AddDataToDropDownList(parent, DrpCurrency, true);
        //        DrpCurrency.SelectedValue = Convert.ToString(BaseCurrData[0, 1]);
        //    }
        //}
        protected void loadCompany()
        {
            //string[,] parent = oDBEngine.GetFieldValue("tbl_master_company", "cmp_internalId,cmp_Name", "CreateUser='" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + "' or LastModifyUser='" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + "'", 2);
            string[,] parent = oDBEngine.GetFieldValue("tbl_master_company h", "h.cmp_internalId,h.cmp_Name", "h.cmp_internalid in (select UserCompany_CompanyID   from  Master_UserCompany where  UserCompany_UserID='" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + "')", 2);

            if (Convert.ToString(Request.QueryString["id"]) == "ADD")
            {
                if (parent[0, 0] != "n")
                {
                    try
                    {
                        clsdropdown.AddDataToDropDownList(parent, DrpCompany,false);
                        //DrpCompany.SelectedValue = BaseCurrData[0, 0];
                        DrpCompany.Items.Insert(0, new ListItem("None", "0"));
                    }
                    catch {
                    }
                }
                else
                {
                    DrpCompany.Items.Insert(0, new ListItem("None", "0"));
                }
            }
            else
            {
                //string[,] ContactData;
                //ContactData = oDBEngine.GetFieldValue("tbl_master_company", "cmp_internalId", "cmp_id=" + Convert.ToString(Request.QueryString["cmp_id"]), 1);
                clsdropdown.AddDataToDropDownList(parent, DrpCompany, true);
                DrpCompany.SelectedValue = BaseCurrData[0, 0];
                txtcurrency.Text = BaseCurrData[0, 1];
                currid_hidden.Value = BaseCurrData[0, 6];
            }
        }

        protected void ShowForm()
        {
            if (Convert.ToString(Request.QueryString["id"]) == "ADD")
            {
                

            }
            else
            {
                DrpCompany.Enabled = false;
                //DrpCurrency.Enabled = false;
               
            }
        }
   

        protected void SetfieldData(string[,] ContactData)
        {
            if (ContactData[0, 0] != "n")
            {
                ////txtFinancerId.Text = ContactData[0, 0];
                ////txtFinancerName.Text = ContactData[0, 1];
                ////cmbBranch.SelectedValue = ContactData[0, 2];
                ////chkActive.Checked = Convert.ToBoolean(ContactData[0, 3]);
            }

        }
        protected void ExecutiveCallbackPanel_Callback(object sender, CallbackEventArgsBase e)
        {
            string data = Convert.ToString(e.Parameter);
            saveUpdateData(data);
        }

        protected void saveUpdateData(string mode)
        {


        }


        [WebMethod]
        public static bool CheckUniqueName(string ShortName)
        {
            string ShortCode = "0";

            if (HttpContext.Current.Session["KeyVal_InternalID"] != null)
            {
                ShortCode = Convert.ToString(HttpContext.Current.Session["KeyVal_InternalID"]);
            }


            MShortNameCheckingBL objMShortNameCheckingBL = new MShortNameCheckingBL();
            DataTable dt = new DataTable();
            Boolean status = false;
            BusinessLogicLayer.GenericMethod oGeneric = new BusinessLogicLayer.GenericMethod();
            if (ShortCode != "" && Convert.ToString(ShortName).Trim() != "")
            {
                status = objMShortNameCheckingBL.CheckUnique(ShortName, ShortCode, "Financer");
            }
            return status;
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            Response.Redirect("CurrencyMaster.aspx");
        }

        protected int getUdfCount()
        {
            DataTable udfCount = oDBEngine.GetDataTable("select 1 from tbl_master_remarksCategory rc where cat_applicablefor='FI'   and ( exists (select * from tbl_master_udfGroup where id=rc.cat_group_id and grp_isVisible=1) or rc.cat_group_id=0)");
            return udfCount.Rows.Count;
        }
        [WebMethod]
        public static List<ListItem> Getcurrency()
        {
            //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]); MULTI
            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
            string query = "select Currency_id,Currency_AlphaCode as Currency_name from Master_Currency";
            List<ListItem> currency = new List<ListItem>();
            SqlDataReader lsdr;
            lsdr = oDBEngine.GetReader(query);
            while (lsdr.Read())
            {
                currency.Add(new ListItem
                {
                    Value = lsdr["Currency_id"].ToString(),
                    Text = lsdr["Currency_name"].ToString()
                });
            }
            return currency;

        }


        public void LoadCurrencyInHiddenField()
        {
            string currency = "";
            DataTable dt=new DataTable();
            if (txtcurrency.Text!="")
            {
                dt = oDBEngine.GetDataTable("select Currency_id,Currency_AlphaCode as Currency_name from Master_Currency where Currency_AlphaCode not in('" + txtcurrency.Text.Trim()+ "')");
            }
            else{
                 dt = oDBEngine.GetDataTable("select Currency_id,Currency_AlphaCode as Currency_name from Master_Currency");
            }
          
            foreach (DataRow dr in dt.Rows)
            {
                if (currency.Trim() == "")
                {
                    currency = " <option value=" + Convert.ToString(dr["Currency_id"]) + ">" + Convert.ToString(dr["Currency_name"]).Trim() + "</option>  ";
                }
                else
                {
                    currency += "   <option value=" + Convert.ToString(dr["Currency_id"]) + ">" + Convert.ToString(dr["Currency_name"]).Trim() + "</option>  ";
                }
            }

            if (Convert.ToString(Request.QueryString["id"]) != "ADD")
            {

                //var curr = currency;
                //curr = curr.Replace("value='" + BaseCurrData[0, 1] + "'", "value='" + BaseCurrData[0, 1] + "' selected='selected'");
                //CurrencyHD.Value = curr;
                 CurrencyHD.Value = currency;
                 //BaseCurrData[0, 5].ToString("yyyy/MM/dd hh:mmtt");
                 //DateTime datea = DateTime.ParseExact(BaseCurrData[0, 5], "yyyy/MM/dd", CultureInfo.InvariantCulture);
                 executiveName_hidden.Value = BaseCurrData[0, 2] + "~" + BaseCurrData[0, 5] + "~" + BaseCurrData[0, 3] + "~" + BaseCurrData[0, 4];
            }
            else
            {
                CurrencyHD.Value = currency;
            }
            
         
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
           
            ////BaseCurrData = oDBEngine.GetFieldValue("tbl_Master_CurrencyRateDateWise", "cmp_internalid,BaseCurrency_ID,ConversionCurrency_ID,SalesRate,PurchaseRate,ConversionDate", " cmp_internalid='" + DrpCompany.SelectedValue + "'and BaseCurrency_ID='" + DrpCurrency.SelectedValue + "'", 6);
            //string add_currency = Convert.ToString(executiveName_hidden.Value);
            //if (Convert.ToString(Request.QueryString["id"]) == "ADD")
            //{
            //    string[,] bid_id = oDBEngine.GetFieldValue(" tbl_Master_BaseCurrency", "BID", " cmp_internalid='" + DrpCompany.SelectedValue + "' and BaseCurrency_ID='" + DrpCurrency.SelectedValue + "'", 1);
            //    if (Convert.ToString(bid_id[0, 0]) != "n")
            //    {
            //        int Id = cmbl.InsertCurrencyMaster(DrpCompany.SelectedItem.Value, Convert.ToInt32(DrpCurrency.SelectedItem.Value), Convert.ToInt32(HttpContext.Current.Session["userid"]), "Dup");
            //        fstSave = true;
            //    }
            //    else
            //    {
            //        int Id = cmbl.InsertCurrencyMaster(DrpCompany.SelectedItem.Value, Convert.ToInt32(DrpCurrency.SelectedItem.Value), Convert.ToInt32(HttpContext.Current.Session["userid"]), "ADD");
            //        fstSave = true;
            //    }
               
            // }
            //else
            //{

            //}

        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
           
            //string[,] bid_id = oDBEngine.GetFieldValue(" tbl_Master_BaseCurrency", "BID", " cmp_internalid='" + DrpCompany.SelectedValue + "'and BaseCurrency_ID='" + DrpCurrency.SelectedValue + "'", 1);
            //if (Convert.ToString(bid_id[0, 0]) != "n")
            //{
            //    fstSave = true;
            //}
            //else
            //{
            //    fstSave = false;
            //}
            //if(fstSave == true)
            //{
            //    string add_currency = Convert.ToString(executiveName_hidden.Value);
            //    int Id = cmbl.InsertCurrencyRateDateWise(DrpCompany.SelectedItem.Value, Convert.ToInt32(DrpCurrency.SelectedItem.Value), Convert.ToInt32(HttpContext.Current.Session["userid"]), "", executiveName_hidden.Value);
            //    if (Convert.ToString(Request.QueryString["id"]) != "ADD")
            //    {
            //        string[,] cnt_id = oDBEngine.GetFieldValue(" tbl_Master_CurrencyRateDateWise", " CRID", " CRID='" + Id + "'", 1);
            //        if (Convert.ToString(cnt_id[0, 0]) != "n")
            //        {
            //            Response.Redirect("frm_currency_master.aspx?id=" + Convert.ToString(cnt_id[0, 0]), false);
            //            //Request.QueryString["id"] = Convert.ToString(cnt_id[0, 0]);
            //        }
            //    }
            //}
            //else
            //{
            //    ClientScript.RegisterClientScriptBlock(GetType(), "js1", "<script> alert('Save Base Currency First!....');</script>", false);
            //}
            int id_q = 0;
            if (Convert.ToString(Request.QueryString["id"]) != "ADD")
            {
                id_q = Convert.ToInt32(Request.QueryString["id"]);
            }
            else
            {
                id_q = 0;
            }
            string add_currency = Convert.ToString(executiveName_hidden.Value);
            int Id = cmbl.InsertCurrencyRateDateWise(DrpCompany.SelectedItem.Value, Convert.ToInt32(currid_hidden.Value), Convert.ToInt32(HttpContext.Current.Session["userid"]), "", executiveName_hidden.Value, id_q);
            if (Convert.ToString(Request.QueryString["id"]) != "ADD")
            {
                string[,] cnt_id = oDBEngine.GetFieldValue(" tbl_Master_CurrencyRateDateWise", " CRID", " CRID='" + Id + "'", 1);
                if (Convert.ToString(cnt_id[0, 0]) != "n")
                {
                    //string popUpscript1 = "";
                    //popUpscript1 = "alert('Successfully Saved'); window.parent.popup.Hide();";
                    //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, popUpscript1, true);

                    //Response.Redirect("frm_currency_master.aspx?id=" + Convert.ToString(cnt_id[0, 0]), false);

                    string popUpscript1 = "";
                    popUpscript1 = "Submited('" + Convert.ToString(cnt_id[0, 0]) + "')";
                    ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, popUpscript1, true);

               
                    
                }
            }

        }

        protected void DrpCompany_SelectedIndexChanged(object sender, EventArgs e)
        {
           
        }

        protected void DrpCompany_SelectedIndexChanged1(object sender, EventArgs e)
        { 
            //string[,] chkcurr = oDBEngine.GetFieldValue("tbl_Master_BaseCurrency", "BaseCurrency_ID", "cmp_internalid='" + DrpCompany.SelectedValue + "'", 1);
            string[,] chkcurr = oDBEngine.GetFieldValue("Master_Currency", "Currency_AlphaCode,Currency_ID", "Currency_ID=(select cmp_currencyid from tbl_master_company where cmp_internalid='" + DrpCompany.SelectedValue + "')", 2);
            if (chkcurr[0, 0] != "n")
            {
                txtcurrency.Text = chkcurr[0, 0];
                currid_hidden.Value = chkcurr[0, 1];
                LoadCurrencyInHiddenField();
            }
            else
            {
            //    //DrpCurrency.Items.Insert(0, new ListItem("None", "0"));
            //    DrpCurrency.SelectedValue = "0";
            }
            if(DrpCompany.SelectedItem.ToString()=="None")
            {
                txtcurrency.Text = "";
            }
        }

    }

}