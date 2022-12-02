using System;
using System.Data;
using System.Web.UI;
////using DevExpress.Web.ASPxClasses;
////using DevExpress.Web;
using System.Configuration;
using BusinessLogicLayer;
using System.Web;
using DevExpress.Web;
namespace ERP.OMS.Management.Activities
{

    public partial class management_Activities_sales_Sconveyence : ERP.OMS.ViewState_class.VSPage
    {

      //  BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);

        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

        protected void Page_Load(object sender, EventArgs e)
        {
            //------- For Read Only User in SQL Datasource Connection String   Start-----------------

            if (HttpContext.Current.Session["EntryProfileType"] != null)
            {
                if (Convert.ToString(HttpContext.Current.Session["EntryProfileType"]) == "R")
                {
                   // SqlConveyence.ConnectionString = ConfigurationSettings.AppSettings["DBReadOnlyConnection"];

                    SqlConveyence.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);

                }
                else
                {
                   // SqlConveyence.ConnectionString = ConfigurationSettings.AppSettings["DBConnectionDefault"];

                    SqlConveyence.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);


                }
            }

            CitySelect.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            areaSelect.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            CitySelect1.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            areaSelect1.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            SqlCurrency.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);

            //------- For Read Only User in SQL Datasource Connection String   End-----------------
            //string ss = Session["SalesID"].ToString();  
        }
        protected void GridConveyence_CellEditorInitialize(object sender, DevExpress.Web.ASPxGridViewEditorEventArgs e)
        {
            if (e.Column.FieldName == "expnd_TArea1")
            {
                if (e.KeyValue != null)
                {
                    object val = GridConveyence.GetRowValuesByKeyValue(e.KeyValue, "expnd_TCity1");
                    if (val == DBNull.Value) return;
                    int country = Convert.ToInt32(val);
                    ASPxComboBox combo = e.Editor as ASPxComboBox;
                    FillStateCombo(combo, country);
                    combo.Callback += new CallbackEventHandlerBase(cmbState_OnCallback);
                }
                else
                {

                    object val = GridConveyence.GetRowValues(0, "expnd_TCity1");
                    if (val == DBNull.Value) return;
                    if (val != null)
                    {
                        int country = Convert.ToInt32(val);
                        ASPxComboBox combo = e.Editor as ASPxComboBox;
                        FillStateCombo(combo, country);
                        combo.Callback += new CallbackEventHandlerBase(cmbState_OnCallback);
                    }
                    else
                    {

                        int country = 1;
                        ASPxComboBox combo = e.Editor as ASPxComboBox;
                        FillStateCombo(combo, country);
                        combo.Callback += new CallbackEventHandlerBase(cmbState_OnCallback);
                    }
                }
            }
            if (e.Column.FieldName == "expnd_TArea2")
            {
                if (e.KeyValue != null)
                {
                    object val = GridConveyence.GetRowValuesByKeyValue(e.KeyValue, "expnd_TCity2");
                    if (val == DBNull.Value) return;
                    int country = Convert.ToInt32(val);
                    ASPxComboBox combo = e.Editor as ASPxComboBox;
                    FillStateCombo1(combo, country);
                    combo.Callback += new CallbackEventHandlerBase(cmbState1_OnCallback);
                }
                else
                {

                    object val = GridConveyence.GetRowValues(0, "expnd_TCity2");
                    if (val == DBNull.Value) return;
                    if (val != null)
                    {
                        int country = Convert.ToInt32(val);
                        ASPxComboBox combo = e.Editor as ASPxComboBox;
                        FillStateCombo1(combo, country);
                        combo.Callback += new CallbackEventHandlerBase(cmbState1_OnCallback);
                    }
                    else
                    {

                        int country = 1;
                        ASPxComboBox combo = e.Editor as ASPxComboBox;
                        FillStateCombo1(combo, country);
                        combo.Callback += new CallbackEventHandlerBase(cmbState1_OnCallback);
                    }
                }
            }
        }
        protected void FillStateCombo(ASPxComboBox cmb, int country)
        {

            string[,] state = GetState(country);
            cmb.Items.Clear();

            for (int i = 0; i < state.GetLength(0); i++)
            {
                cmb.Items.Add(state[i, 1], state[i, 0]);
            }
        }
        string[,] GetState(int country)
        {
            areaSelect.SelectParameters[0].DefaultValue = country.ToString();
            DataView view = (DataView)areaSelect.Select(DataSourceSelectArguments.Empty);
            string[,] DATA = new string[view.Count, 2];
            for (int i = 0; i < view.Count; i++)
            {
                DATA[i, 0] = view[i][0].ToString();
                DATA[i, 1] = view[i][1].ToString();
            }
            return DATA;

        }
        protected void FillStateCombo1(ASPxComboBox cmb, int country)
        {

            string[,] state = GetState1(country);
            cmb.Items.Clear();

            for (int i = 0; i < state.GetLength(0); i++)
            {
                cmb.Items.Add(state[i, 1], state[i, 0]);
            }
        }
        string[,] GetState1(int country)
        {
            areaSelect1.SelectParameters[0].DefaultValue = country.ToString();
            DataView view = (DataView)areaSelect1.Select(DataSourceSelectArguments.Empty);
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
            FillStateCombo(source as ASPxComboBox, Convert.ToInt32(e.Parameter));
        }
        private void cmbState1_OnCallback(object source, CallbackEventArgsBase e)
        {
            FillStateCombo1(source as ASPxComboBox, Convert.ToInt32(e.Parameter));
        }
        protected void GridConveyence_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {

        }
    }
}