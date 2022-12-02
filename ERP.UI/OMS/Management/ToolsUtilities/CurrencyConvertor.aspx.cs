using System;
using System.Data;
using System.Web;
using System.Web.UI;
//////using DevExpress.Web.ASPxClasses;
using DevExpress.Web;
using System.Configuration;

namespace ERP.OMS.Management.ToolsUtilities
{
    public partial class management_ToolsUtilities_CurrencyConverter : System.Web.UI.Page
    {
        #region Global Variable
        //string strCon = ConfigurationManager.AppSettings["DBConnectionDefault"].ToString();MULTI
        string strCon = null;
        //DBEngine oDBEngine = null;
        DataTable DT = new DataTable();
        BusinessLogicLayer.DBEngine oDBEngine = null;
        string CurrentDateTime = String.Empty;
        AspxHelper oAspxHelper = new AspxHelper();
        string feeBasisValue;
        #endregion

        #region PageClase
        void CurrencyConversion()
        {
            string[] sqlParameterName = new string[6];
            string[] sqlParameterType = new string[6];
            string[] sqlParameterValue = new string[6];

            sqlParameterName[0] = "FromCurrency";
            sqlParameterValue[0] = Combo_frmCrncy.Value.ToString();
            sqlParameterType[0] = "I";

            sqlParameterName[1] = "ToCurrency";
            sqlParameterValue[1] = Combo_toCrncy.Value.ToString();
            sqlParameterType[1] = "I";

            sqlParameterName[2] = "FxDate";
            sqlParameterValue[2] = Date.Date.ToString();
            sqlParameterType[2] = "D";

            sqlParameterName[3] = "Amount";
            sqlParameterValue[3] = TxtAmnt.Text.ToString();
            sqlParameterType[3] = "DE";

            sqlParameterName[4] = "FeeBasis";
            sqlParameterValue[4] = feeBasisValue;
            sqlParameterType[4] = "I";

            sqlParameterName[5] = "FeeRate";
            sqlParameterValue[5] = TxtConvrsnChrg.Text.ToString();
            sqlParameterType[5] = "DE";

            //DataSet Ds = SQLProcedures.SelectProcedureArrDS("conversion_of_currency", sqlParameterName, sqlParameterType, sqlParameterValue);
            DataSet Ds = BusinessLogicLayer.SQLProcedures.SelectProcedureArrDS("conversion_of_currency", sqlParameterName, sqlParameterType, sqlParameterValue);

            if (Ds.Tables.Count > 0)
            {
                if (Ds.Tables[0].Rows.Count > 0)
                {
                    if (Ds.Tables[0].Rows[0][0].ToString().Contains("Fx Rates"))
                    {
                        CbpConvert.JSProperties["cpNtAvl"] = Ds.Tables[0].Rows[0][0].ToString();
                    }
                    else
                    {

                        CbpConvert.JSProperties["cpUnit"] = Ds.Tables[0].Rows[0][0].ToString();
                        CbpConvert.JSProperties["cpUnit2"] = Ds.Tables[0].Rows[0][1].ToString();
                        CbpConvert.JSProperties["cpCnvamnt"] = String.Format("{0:###,###,###,###,###.#0}", Ds.Tables[0].Rows[0][2]);
                        CbpConvert.JSProperties["cpCnCharge"] = Ds.Tables[0].Rows[0][3].ToString();
                        CbpConvert.JSProperties["cpFromCrncySymbol"] = Ds.Tables[0].Rows[0][4].ToString();
                        CbpConvert.JSProperties["cpToCrncySymbol"] = Ds.Tables[0].Rows[0][5].ToString();


                    }
                }


            }

            Ds.Dispose();

        }
        #endregion



        void Bind_Combo_frmCrncy()
        {
            string strObj = "Indian Rupee [INR]";
            string strQuery = "select Currency_Name+' ['+ltrim(rtrim([Currency_AlphaCode]))+']'as CurrencyNames,Currency_ID from Master_Currency";
            DataSet Ds = oAspxHelper.Bind_Combo(strQuery);
            oAspxHelper.Bind_Combo(Combo_frmCrncy, Ds, "CurrencyNames", "Currency_ID", (object)strObj);
        }
        void Bind_Combo_toCrncy()
        {
            string strObj = "US Dollar [USD]";
            string strQuery = "select Currency_Name+' ['+ltrim(rtrim([Currency_AlphaCode]))+']'as CurrencyNames,Currency_ID from Master_Currency";
            DataSet Ds = oAspxHelper.Bind_Combo(strQuery);
            oAspxHelper.Bind_Combo(Combo_toCrncy, Ds, "CurrencyNames", "Currency_ID", (object)strObj);
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            if (HttpContext.Current.Session["userid"] == null)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
            //this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");
            //////Initialize Global variable
            //oDBEngine = new DBEngine(strCon);
            oDBEngine = new BusinessLogicLayer.DBEngine(strCon);
            CurrentDateTime = Convert.ToDateTime(oDBEngine.GetDate()).ToString("yyyy-MM-dd");
            oDBEngine = null;
            if (!IsPostBack)
            {
                //Set DataBase Date in Date Control
                Date.Date = Convert.ToDateTime(CurrentDateTime);
                Bind_Combo_frmCrncy();
                Bind_Combo_toCrncy();

            }
        }
        protected void CbpConvert_Callback(object source, CallbackEventArgsBase e)
        {
            CbpConvert.JSProperties["cpNtAvl"] = null;
            CbpConvert.JSProperties["cpUnit"] = null;
            CbpConvert.JSProperties["cpUnit2"] = null;
            CbpConvert.JSProperties["cpCnvamnt"] = null;
            CbpConvert.JSProperties["cpCnCharge"] = null;
            CbpConvert.JSProperties["cpUnitCharge"] = null;
            CbpConvert.JSProperties["cpFromCrncySymbol"] = null;
            CbpConvert.JSProperties["cpToCrncySymbol"] = null;

            string[] strSplit = e.Parameter.Split('~');
            string WhichCall = strSplit[0];
            if (WhichCall == "Convert")
            {
                string SelectValue = strSplit[1];
                if (SelectValue == "Fc")
                {
                    feeBasisValue = "1";
                    CurrencyConversion();
                    CbpConvert.JSProperties["cpUnitCharge"] = "F";
                }
                else
                {
                    feeBasisValue = "2";
                    CurrencyConversion();
                    CbpConvert.JSProperties["cpUnitCharge"] = "T";
                }
            }
        }
    }
}
