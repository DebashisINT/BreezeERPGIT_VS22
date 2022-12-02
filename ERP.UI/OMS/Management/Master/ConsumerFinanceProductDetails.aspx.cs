using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
//using DevExpress.Web;
////using DevExpress.Web.ASPxTabControl;
using DevExpress.Web;
using BusinessLogicLayer;

namespace ERP.OMS.Management.Master
{
    public partial class management_master_ConsumerFinanceProductDetails : ERP.OMS.ViewState_class.VSPage
    {
        //DBEngine oDBEngine = new DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);MULTI
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        ConsumerFinanceProductDeatails OConsumerFinanceProductDeatails = new ConsumerFinanceProductDeatails();
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
            SqlConsumer.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            SqlCurrency.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            SqlPName.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);

            //------- For Read Only User in SQL Datasource Connection String   Start-----------------

            if (HttpContext.Current.Session["EntryProfileType"] != null)
            {
                if (Convert.ToString(HttpContext.Current.Session["EntryProfileType"]) == "R")
                {
                    //SqlConsumerFinance.ConnectionString = ConfigurationSettings.AppSettings["DBReadOnlyConnection"];MULTI
                    //SqlproductRequired.ConnectionString = ConfigurationSettings.AppSettings["DBReadOnlyConnection"];MULTI
                    SqlConsumerFinance.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                    SqlproductRequired.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                }
                else
                {
                    //SqlConsumerFinance.ConnectionString = ConfigurationSettings.AppSettings["DBConnectionDefault"];MULTI
                    //SqlproductRequired.ConnectionString = ConfigurationSettings.AppSettings["DBConnectionDefault"];MULTI
                    SqlConsumerFinance.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                    SqlproductRequired.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                }
            }

            //------- For Read Only User in SQL Datasource Connection String   End-----------------

            if (HttpContext.Current.Session["userid"] == null)
            {
                //Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
            ////this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");
        }
        protected void GridConsumerFinance_RowValidating(object sender, DevExpress.Web.Data.ASPxDataValidationEventArgs e)
        {
            try
            {
                string cf_ptype = "";
                string cf_pname = "";
                string cf_conCode = "";
                string cf_proFeature = "";
                string cf_loanCurrency = "";
                string cf_appliMinAge_Sal = "";
                string cf_appliMaxAge_Sal = "";
                string cf_appliMinAge_Self = "";
                string cf_appliMaxAge_Self = "";
                string cf_annualIncome_Sal = "0";
                string cf_annualIncome_Self = "0";
                string cf_MinloanAmount = "0";
                string cf_MaxLoanAmount = "0";
                string cf_Minloantenuare = "0";
                string cf_Maxloantenure = "0";
                string cf_serviceContinuity = "0";
                string cf_residenceStab = "0";
                string cf_loanValue = "0";
                string cf_validitySanction = "0";
                string cf_moderepayment = "";
                string cf_partrepayment = "";
                string cf_noReference = "0";
                string cf_fixedIncomeRatio = "";

                try
                {
                    cf_ptype = e.NewValues["cf_ptype"].ToString();
                }
                catch
                {
                }
                try
                {
                    cf_pname = e.NewValues["cf_pname"].ToString();
                }
                catch
                {
                }
                try
                {
                    cf_conCode = e.NewValues["cf_conCode"].ToString();
                }
                catch
                {
                }
                try
                {
                    cf_proFeature = e.NewValues["cf_proFeature"].ToString();
                }
                catch
                {
                }
                try
                {
                    cf_loanCurrency = e.NewValues["cf_loanCurrency"].ToString();
                }
                catch
                {
                }
                try
                {
                    cf_appliMinAge_Sal = e.NewValues["cf_appliMinAge_Sal"].ToString();
                }
                catch
                {
                }
                try
                {
                    cf_appliMaxAge_Sal = e.NewValues["cf_appliMaxAge_Sal"].ToString();
                }
                catch
                {
                }
                try
                {
                    cf_appliMinAge_Self = e.NewValues["cf_appliMinAge_Self"].ToString();
                }
                catch
                {
                }
                try
                {
                    cf_appliMaxAge_Self = e.NewValues["cf_appliMaxAge_Self"].ToString();
                }
                catch
                {
                }
                try
                {
                    cf_annualIncome_Sal = e.NewValues["cf_annualIncome_Sal"].ToString();
                }
                catch
                {
                }
                try
                {
                    cf_annualIncome_Self = e.NewValues["cf_annualIncome_Self"].ToString();
                }
                catch
                {
                }
                try
                {
                    cf_MinloanAmount = e.NewValues["cf_MinloanAmount"].ToString();
                }
                catch
                {
                }
                try
                {
                    cf_MaxLoanAmount = e.NewValues["cf_MaxLoanAmount"].ToString();
                }
                catch
                {
                }
                try
                {
                    cf_Minloantenuare = e.NewValues["cf_Minloantenuare"].ToString();
                }
                catch
                {
                }
                try
                {
                    cf_Maxloantenure = e.NewValues["cf_Maxloantenure"].ToString();
                }
                catch
                {
                }
                try
                {
                    cf_serviceContinuity = e.NewValues["cf_serviceContinuity"].ToString();
                }
                catch
                {
                }
                try
                {
                    cf_residenceStab = e.NewValues["cf_residenceStab"].ToString();
                }
                catch
                {
                }
                try
                {
                    cf_loanValue = e.NewValues["cf_loanValue"].ToString();
                }
                catch
                {
                }
                try
                {
                    cf_validitySanction = e.NewValues["cf_validitySanction"].ToString();
                }
                catch
                {
                }
                try
                {
                    cf_moderepayment = e.NewValues["cf_moderepayment"].ToString();
                }
                catch
                {
                }
                try
                {
                    cf_partrepayment = e.NewValues["cf_partrepayment"].ToString();
                }
                catch
                {
                }
                try
                {
                    cf_noReference = e.NewValues["cf_noReference"].ToString();
                }
                catch
                {
                }
                try
                {
                    cf_fixedIncomeRatio = e.NewValues["cf_fixedIncomeRatio"].ToString();
                }
                catch
                {
                }
                if (GridConsumerFinance.IsNewRowEditing)
                {
                    /* For Tier Structure
                    String con = ConfigurationSettings.AppSettings["DBConnectionDefault"];
                    SqlConnection lcon = new SqlConnection(con);
                    lcon.Open();
                    SqlCommand lcmdEmplInsert = new SqlCommand("ConsumerFinanceProductInsert", lcon);
                    lcmdEmplInsert.CommandType = CommandType.StoredProcedure;
                    //___________For Returning InternalID_________//
                    SqlParameter parameter = new SqlParameter("@result", SqlDbType.VarChar, 50);
                    parameter.Direction = ParameterDirection.Output;
                    ///_______________________________________________//
               

               

                    lcmdEmplInsert.Parameters.AddWithValue("@cf_ptype", cf_ptype);
                    lcmdEmplInsert.Parameters.AddWithValue("@cf_pname", cf_pname);
                    lcmdEmplInsert.Parameters.AddWithValue("@cf_conCode", cf_conCode);
                    lcmdEmplInsert.Parameters.AddWithValue("@cf_proFeature", cf_proFeature);
                    lcmdEmplInsert.Parameters.AddWithValue("@cf_loanCurrency", cf_loanCurrency);
                    lcmdEmplInsert.Parameters.AddWithValue("@cf_appliMinAge_Sal", cf_appliMinAge_Sal);
                    lcmdEmplInsert.Parameters.AddWithValue("@cf_appliMaxAge_Sal", cf_appliMaxAge_Sal);
                    lcmdEmplInsert.Parameters.AddWithValue("@cf_appliMinAge_Self", cf_appliMinAge_Self);
                    lcmdEmplInsert.Parameters.AddWithValue("@cf_appliMaxAge_Self", cf_appliMaxAge_Self);
                    lcmdEmplInsert.Parameters.AddWithValue("@cf_annualIncome_Sal", Convert.ToDecimal(cf_annualIncome_Sal));
                    lcmdEmplInsert.Parameters.Add(parameter);               
                    lcmdEmplInsert.Parameters.AddWithValue("@cf_annualIncome_Self", Convert.ToDecimal(cf_annualIncome_Self));
                    lcmdEmplInsert.Parameters.AddWithValue("@cf_MinloanAmount", Convert.ToDecimal(cf_MinloanAmount));
                    lcmdEmplInsert.Parameters.AddWithValue("@cf_MaxLoanAmount", Convert.ToDecimal(cf_MaxLoanAmount));
                    lcmdEmplInsert.Parameters.AddWithValue("@cf_Minloantenuare", Convert.ToDecimal(cf_Minloantenuare));
                    lcmdEmplInsert.Parameters.AddWithValue("@cf_Maxloantenure", Convert.ToDecimal(cf_Maxloantenure));
                    lcmdEmplInsert.Parameters.AddWithValue("@cf_serviceContinuity", Convert.ToDecimal(cf_serviceContinuity));
                    lcmdEmplInsert.Parameters.AddWithValue("@cf_residenceStab", Convert.ToDecimal(cf_residenceStab));
                    lcmdEmplInsert.Parameters.AddWithValue("@cf_loanValue", Convert.ToDecimal(cf_loanValue));
                    lcmdEmplInsert.Parameters.AddWithValue("@cf_validitySanction", Convert.ToDecimal(cf_validitySanction));
                    lcmdEmplInsert.Parameters.AddWithValue("@cf_moderepayment", cf_moderepayment);
                    lcmdEmplInsert.Parameters.AddWithValue("@cf_partrepayment", cf_partrepayment);
                    lcmdEmplInsert.Parameters.AddWithValue("@cf_noReference", Convert.ToDecimal(cf_noReference));
                    lcmdEmplInsert.Parameters.AddWithValue("@cf_fixedIncomeRatio", cf_fixedIncomeRatio);
                    lcmdEmplInsert.Parameters.AddWithValue("@CreateUser", Convert.ToDecimal(HttpContext.Current.Session["userid"].ToString()));

                    lcmdEmplInsert.ExecuteNonQuery();


                    string InternalID = parameter.Value.ToString();
                    lcon.Close();

                    */


                    string InternalID = OConsumerFinanceProductDeatails.Insert_ConsumerFinanceProduct(cf_ptype, cf_pname, cf_conCode,
                                                       cf_proFeature, cf_loanCurrency, cf_appliMinAge_Sal, cf_appliMaxAge_Sal,
                                                       cf_appliMinAge_Self, cf_appliMaxAge_Self,
                                                      (cf_annualIncome_Sal), (cf_annualIncome_Self),
                                                      (cf_MinloanAmount),
                                                      (cf_MaxLoanAmount), (cf_Minloantenuare),
                                                      (cf_Maxloantenure),
                                                      (cf_serviceContinuity), (cf_residenceStab),
                                                      (cf_loanValue),
                                                      (cf_validitySanction), cf_moderepayment, cf_partrepayment,
                                                      (cf_noReference),
                                                       cf_fixedIncomeRatio, (HttpContext.Current.Session["userid"].ToString()));

                    Session["KeyVal"] = InternalID;
                    e.RowError = "Detail has been Updated. Please Update Document";
                    return;
                }
                else
                {
                    string cf_pcode = e.Keys[0].ToString();
                    string ProductType = e.OldValues["cf_ptype"].ToString();
                    string ProductType1 = e.NewValues["cf_ptype"].ToString();
                    string ProductName = e.OldValues["cf_pname"].ToString();
                    string ProductName1 = e.NewValues["cf_pname"].ToString();
                    if (ProductType != ProductType1 || ProductName != ProductName1)
                    {
                        e.RowError = "You Can Not Edit Product Type Or Product Name";
                        return;
                    }
                    // string cf_pcode = e.OldValues["cf_pcode"].ToString();
                    oDBEngine.SetFieldValue("tbl_master_CFProducts", "[cf_conCode] = '" + cf_conCode + "', [cf_proFeature] = '" + cf_proFeature + "', [cf_loanCurrency] = '" + cf_loanCurrency + "', [cf_appliMinAge_Sal] = '" + cf_appliMinAge_Sal + "', [cf_appliMaxAge_Sal] = '" + cf_appliMaxAge_Sal + "', [cf_appliMinAge_Self] = '" + cf_appliMinAge_Self + "', [cf_appliMaxAge_Self] = '" + cf_appliMaxAge_Self + "', [cf_annualIncome_Sal] = '" + cf_annualIncome_Sal + "', [cf_annualIncome_Self] = '" + cf_annualIncome_Self + "', [cf_MinloanAmount] = '" + cf_MinloanAmount + "', [cf_MaxLoanAmount] = '" + cf_MaxLoanAmount + "', [cf_Minloantenuare] = '" + cf_Minloantenuare + "', [cf_Maxloantenure] = '" + cf_Maxloantenure + "', [cf_serviceContinuity] = '" + cf_serviceContinuity + "', [cf_residenceStab] = '" + cf_residenceStab + "', [cf_loanValue] = '" + cf_loanValue + "', [cf_validitySanction] = '" + cf_validitySanction + "', [cf_moderepayment] = '" + cf_moderepayment + "', [cf_partrepayment] = '" + cf_partrepayment + "', [cf_noReference] = '" + cf_noReference + "', [cf_fixedIncomeRatio] = '" + cf_fixedIncomeRatio + "',  [LastModifyUser] = '" + Session["userid"].ToString() + "', [LastModifyDate] = '" + oDBEngine.GetDate().ToString() + "'", " [cf_pcode] = '" + cf_pcode + "'");
                    e.RowError = "Detail has been Updated. Please Update Document";
                    return;
                }
            }
            catch
            {
            }
        }
        protected void GridConsumerFinance_HtmlRowCreated(object sender, DevExpress.Web.ASPxGridViewTableRowEventArgs e)
        {
            try
            {
                if (e.RowType == GridViewRowType.Data)
                {
                    int commandColumnIndex = -1;
                    for (int i = 0; i < GridConsumerFinance.Columns.Count; i++)
                        if (GridConsumerFinance.Columns[i] is GridViewCommandColumn)
                        {
                            commandColumnIndex = i;
                            break;
                        }
                    if (commandColumnIndex == -1)
                        return;
                    //____One colum has been hided so index of command column will be leass by 1 
                    commandColumnIndex = commandColumnIndex - 23;
                    DevExpress.Web.Rendering.GridViewTableCommandCell cell = e.Row.Cells[commandColumnIndex] as DevExpress.Web.Rendering.GridViewTableCommandCell;
                    for (int i = 0; i < cell.Controls.Count; i++)
                    {
                        DevExpress.Web.Rendering.GridCommandButtonsCell button = cell.Controls[i] as DevExpress.Web.Rendering.GridCommandButtonsCell;
                        if (button == null) return;
                        DevExpress.Web.Internal.InternalHyperLink hyperlink = button.Controls[0] as DevExpress.Web.Internal.InternalHyperLink;

                        if (hyperlink.Text == "Delete")
                        {
                            if (Session["PageAccess"].ToString() == "DelAdd" || Session["PageAccess"].ToString() == "Delete" || Session["PageAccess"].ToString() == "All")
                            {
                                hyperlink.Enabled = true;
                                continue;
                            }
                            else
                            {
                                hyperlink.Enabled = false;
                                continue;
                            }
                        }


                    }

                }
            }
            catch
            {
            }
        }
        protected void GridConsumerFinance_HtmlEditFormCreated(object sender, DevExpress.Web.ASPxGridViewEditFormEventArgs e)
        {
            try
            {
                if (!GridConsumerFinance.IsNewRowEditing)
                {
                    ASPxPageControl RT1 = GridConsumerFinance.FindEditFormTemplateControl("pageControl") as ASPxPageControl;
                    ASPxGridViewTemplateReplacement RT = RT1.FindControl("UpdateButton") as ASPxGridViewTemplateReplacement;
                    if (Session["PageAccess"].ToString().Trim() == "Add" || Session["PageAccess"].ToString().Trim() == "Modify" || Session["PageAccess"].ToString().Trim() == "All")
                        RT.Visible = true;
                    else
                        RT.Visible = false;
                }
            }
            catch
            {
            }
        }

        protected void gridProductRequired_BeforePerformDataSelect(object sender, EventArgs e)
        {
            if (!GridConsumerFinance.IsNewRowEditing)
            {
                Session["KeyVal"] = (sender as ASPxGridView).GetMasterRowKeyValue();
            }
        }
        protected void GridConsumerFinance_CancelRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            GridConsumerFinance.DataBind();
        }
    }
}