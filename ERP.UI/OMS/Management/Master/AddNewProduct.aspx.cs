using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Configuration;
using BusinessLogicLayer;

namespace ERP.OMS.Management.Master
{
    public partial class management_master_AddNewProduct : ERP.OMS.ViewState_class.VSPage
    {
        private const string STR_Constant = "'";
        public String[] InputName = new String[20];
        public String[] InputType = new String[20];
        public String[] InputValue = new String[20];
        public string Id = string.Empty;
        public string Flag = string.Empty;
        public string ProductName = string.Empty;
        BusinessLogicLayer.Converter oconverter = new BusinessLogicLayer.Converter();
        DateTime currentdate = Convert.ToDateTime(null);
        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);MULTI
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        //public string pageAccess = "";

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
                //Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }

            ////this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");
            tblAddProductEquity.Visible = false;
            tblAddProduct.Visible = false;

        }

        public void ClearArray()
        {
            Array.Clear(InputName, 0, InputName.Length - 1);
            Array.Clear(InputType, 0, InputType.Length - 1);
            Array.Clear(InputValue, 0, InputValue.Length - 1);
        }

        protected void btnCheck_Click(object sender, EventArgs e)
        {
            if (txtISIN.Text.Length < 12)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "PageScript", "<script language='javascript'>alert('Incorrect ISIN No');</script>");
            }
            else
            {
                ClearArray();
                InputName[0] = "Module";
                InputName[1] = "ISIN";
                InputName[2] = "ExchangesegmentID";
                InputName[3] = "ProductsName";
                InputName[4] = "ProductsShortName";
                InputName[5] = "Equity_Series";
                InputName[6] = "Equity_TradingLot";
                InputName[7] = "Equity_TickerCode";
                InputName[8] = "Group";
                InputName[9] = "ISIN_FaceValue";
                InputName[10] = "ProductID";

                InputType[0] = "V";
                InputType[1] = "V";
                InputType[2] = "V";
                InputType[3] = "V";
                InputType[4] = "V";
                InputType[5] = "V";
                InputType[6] = "V";
                InputType[7] = "V";
                InputType[8] = "V";
                InputType[9] = "V";
                InputType[10] = "V";

                InputValue[0] = "CheckISIN";
                InputValue[1] = txtISIN.Text;
                InputValue[2] = HttpContext.Current.Session["ExchangeSegmentID"].ToString();
                InputValue[3] = "";
                InputValue[4] = "";
                InputValue[5] = "";
                InputValue[6] = "";
                InputValue[7] = "";
                InputValue[8] = "";
                InputValue[9] = "";
                InputValue[10] = "";
                DataTable dt1 = BusinessLogicLayer.SQLProcedures.SelectProcedureArr("AddNewProduct", InputName, InputType, InputValue);

                if (dt1.Rows.Count > 0)
                {
                    tblAddProduct.Visible = true;
                    txtProductName.Text = dt1.Rows[0][0].ToString();
                    txtProductName.Enabled = false;
                    txtProductShortName.Text = dt1.Rows[0][1].ToString();
                    txtProductShortName.Enabled = false;
                    btnContinue.Enabled = false;
                    Page.ClientScript.RegisterStartupScript(GetType(), "PageScript", "<script language='javascript'>alert('This product already exists');</script>");
                }
                else
                {
                    tblAddProduct.Visible = true;
                    Page.ClientScript.RegisterStartupScript(GetType(), "PageScript", "<script language='javascript'>alert('Insert The Following TextBoxes');</script>");
                    txtProductShortName.Text = "";
                    txtProductName.Text = "";
                    txtProductName.Enabled = true;
                    txtProductShortName.Enabled = true;
                    btnContinue.Enabled = true;
                }
            }


        }

        protected void btnContinue_Click(object sender, EventArgs e)
        {
            if (txtProductName.Text == "")
            {
                ProductName = txtProductShortName.Text;
            }
            else
            {
                ProductName = txtProductName.Text;
            }
            ClearArray();
            InputName[0] = "Module";
            InputName[1] = "ISIN";
            InputName[2] = "ExchangesegmentID";
            InputName[3] = "ProductsName";
            InputName[4] = "ProductsShortName";
            InputName[5] = "Equity_Series";
            InputName[6] = "Equity_TradingLot";
            InputName[7] = "Equity_TickerCode";
            InputName[8] = "Group";
            InputName[9] = "ISIN_FaceValue";
            InputName[10] = "ProductID";

            InputType[0] = "V";
            InputType[1] = "V";
            InputType[2] = "V";
            InputType[3] = "V";
            InputType[4] = "V";
            InputType[5] = "V";
            InputType[6] = "V";
            InputType[7] = "V";
            InputType[8] = "V";
            InputType[9] = "V";
            InputType[10] = "V";

            InputValue[0] = "InsertProduct";
            InputValue[1] = "";
            InputValue[2] = HttpContext.Current.Session["ExchangeSegmentID"].ToString();
            InputValue[3] = ProductName.ToString();
            InputValue[4] = txtProductShortName.Text;
            InputValue[5] = "";
            InputValue[6] = "";
            InputValue[7] = "";
            InputValue[8] = "";
            InputValue[9] = "";
            InputValue[10] = "";

            DataSet ds = null;
            ds = BusinessLogicLayer.SQLProcedures.SelectProcedureArrDS("[AddNewProduct]", InputName, InputType, InputValue);

            hdnProductID.Value = ds.Tables[0].Rows[0][0].ToString();
            tblAddProductEquity.Visible = true;
            tblAddProduct.Visible = true;

        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            ClearArray();
            InputName[0] = "Module";
            InputName[1] = "ISIN";
            InputName[2] = "ExchangesegmentID";
            InputName[3] = "ProductsName";
            InputName[4] = "ProductsShortName";
            InputName[5] = "Equity_Series";
            InputName[6] = "Equity_TradingLot";
            InputName[7] = "Equity_TickerCode";
            InputName[8] = "Group";
            InputName[9] = "ISIN_FaceValue";
            InputName[10] = "ProductID";

            InputType[0] = "V";
            InputType[1] = "V";
            InputType[2] = "V";
            InputType[3] = "V";
            InputType[4] = "V";
            InputType[5] = "V";
            InputType[6] = "V";
            InputType[7] = "V";
            InputType[8] = "V";
            InputType[9] = "V";
            InputType[10] = "V";

            InputValue[0] = "InsertProductEquity";
            InputValue[1] = txtISIN.Text;
            InputValue[2] = HttpContext.Current.Session["ExchangeSegmentID"].ToString();
            InputValue[3] = txtProductName.Text;
            InputValue[4] = txtProductShortName.Text;
            InputValue[5] = txtSeries.Text;
            InputValue[6] = txtTradingLot.Text;
            InputValue[7] = txtTickerCode.Text;
            InputValue[8] = txtGroup.Text;
            InputValue[9] = txtFaceValue.Text;
            InputValue[10] = hdnProductID.Value;

            DataTable dt2 = BusinessLogicLayer.SQLProcedures.SelectProcedureArr("AddNewProduct", InputName, InputType, InputValue);
            Page.ClientScript.RegisterStartupScript(GetType(), "PageScript", "<script language='javascript'>alert('This Product Created Successfully ');</script>");
            txtFaceValue.Text = "";
            txtGroup.Text = "";
            txtISIN.Text = "";
            txtProductName.Text = "";
            txtProductShortName.Text = "";
            txtSeries.Text = "";
            txtTickerCode.Text = "";
            txtTradingLot.Text = "";
            btnContinue.Enabled = true;


        }
    }
}