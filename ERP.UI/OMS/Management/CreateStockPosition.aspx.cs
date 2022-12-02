using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using DevExpress.Web;
//////using DevExpress.Web;
//////using DevExpress.Web.ASPxClasses;
//using DevExpress.Web;
////using DevExpress.Web.ASPxPopupControl;
using System.Data.SqlClient;
using BusinessLogicLayer;


namespace ERP.OMS.Management
{
    public partial class management_CreateStockPosition : System.Web.UI.Page
    {
        #region Global Variable
        //string strCon = ConfigurationManager.AppSettings["DBConnectionDefault"].ToString();MULTI
        string strCon = null;
        DBEngine oDBEngine = new DBEngine();
        DataTable DT = new DataTable();
        string CurrentDateTime = String.Empty;
        #endregion

        #region PageClase
        void GenerateDeleteCreateStockPosition()
        {
            string[] sqlParameterName = new string[8];
            string[] sqlParameterType = new string[8];
            string[] sqlParameterValue = new string[8];

            sqlParameterName[0] = "CompanyID";
            sqlParameterValue[0] = Session["LastCompany"].ToString();
            sqlParameterType[0] = "V";

            sqlParameterName[1] = "SegmentID";
            sqlParameterValue[1] = Session["UserSegID"].ToString();
            sqlParameterType[1] = "I";

            sqlParameterName[2] = "DateFrom";
            sqlParameterValue[2] = DtFrom.Date.ToString("yyyy-MM-dd");
            sqlParameterType[2] = "D";

            sqlParameterName[3] = "DateTo";
            sqlParameterValue[3] = DtTo.Date.ToString("yyyy-MM-dd");
            sqlParameterType[3] = "D";

            sqlParameterName[4] = "FinYear";
            sqlParameterValue[4] = Session["LastFinYear"].ToString();
            sqlParameterType[4] = "V";

            sqlParameterName[5] = "User";
            sqlParameterValue[5] = Session["UserID"].ToString();
            sqlParameterType[5] = "I";

            sqlParameterName[6] = "GenrationType";
            sqlParameterValue[6] = ForGenOrDel.SelectedItem.Value.ToString();
            sqlParameterType[6] = "C";

            DataSet Ds = SQLProcedures.SelectProcedureArrDS("Process_StockPosition", sqlParameterName, sqlParameterType, sqlParameterValue);

            if (Ds != null)
                if (Ds.Tables.Count > 0)
                    if (Ds.Tables[0].Rows.Count > 0)
                    {
                        lblMsg.Text = Convert.ToString(Ds.Tables[0].Rows[0]["Retval"]);
                        //GvDateDtl.JSProperties["cpShowUpdateDeleteStatus"] = Ds.Tables[0].Rows[0][0].ToString();
                    }
            Ds.Dispose();
        }
        #endregion
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

            //if (HttpContext.Current.Session["userid"] == null)
            //{
            //   //Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            //}
            ////this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");

            //////Initialize Global variable
            oDBEngine = new DBEngine(strCon);
            CurrentDateTime = Convert.ToDateTime(oDBEngine.GetDate()).ToString("yyyy-MM-dd");
            oDBEngine = null;
            if (!IsPostBack)
            {
                //Set DataBase Date in Date Control
                DtFrom.Date = Convert.ToDateTime(CurrentDateTime);
                DtTo.Date = Convert.ToDateTime(CurrentDateTime);
            }


        }
        protected void GvDateDtl_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            string[] strSplit = e.Parameters.Split('~');
            string WhichCall = strSplit[0];
            if (WhichCall == "Show")
            { }
            //  GenerateDeleteCreateStockPosition();
        }

        protected void CancelBtn_Click(object sender, EventArgs e)
        {
            DtFrom.Date = Convert.ToDateTime(CurrentDateTime);
            DtTo.Date = Convert.ToDateTime(CurrentDateTime);
        }

        protected void BtnGnDl_Click(object sender, EventArgs e)
        {
            GenerateDeleteCreateStockPosition();
        }
    }
}