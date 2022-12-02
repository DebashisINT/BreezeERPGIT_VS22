using System;
using System.Web;
using System.Web.UI;
using DevExpress.Web;
using System.Configuration;
using BusinessLogicLayer;

namespace ERP.OMS.Reports
{
    public partial class CTTRegister : System.Web.UI.Page
    {
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        BusinessLogicLayer.Converter oconverter = new BusinessLogicLayer.Converter();
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
        protected void Page_Init(object sender, EventArgs e)
        {
            CTTRegisterDataSource.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            DataSourcedetailCTTGrid.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            CTTDataSource.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
           
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (HttpContext.Current.Session["userid"] == null)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
            if (!IsPostBack)
            {

                Page.ClientScript.RegisterStartupScript(this.GetType(), "height", "<script language='javascript'>height();</script>");
                date();
                Page.ClientScript.RegisterStartupScript(this.GetType(), "height", "<script language='javascript'>height();</script>");
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "nodisplay();", true);

            }

            BindGrid();
        }
        void date()
        {
            cmbDate.EditFormatString = oconverter.GetDateFormat("Date");
            string[] idlist = oDBEngine.GetDate().GetDateTimeFormats();
            cmbDate.Value = Convert.ToDateTime(idlist[2]);

        }
        protected void CTTDetailRegisterGrid_DataSelect(object sender, EventArgs e)
        {
            Session["KeyVal"] = (sender as ASPxGridView).GetMasterRowKeyValue();
            bind();

        }
        protected void BindGrid()
        {

            bind();
            CTTRegisterGrid.DataBind();

        }
        protected void CTTRegisterGrid_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            bind();
            CTTRegisterGrid.DataBind();
            if (e.Parameters == "s")
            {
                CTTRegisterGrid.Settings.ShowFilterRow = true;
            }

            if (e.Parameters == "All")
            {
                CTTRegisterGrid.FilterExpression = string.Empty;
            }
        }


        protected void CTTRegisterGrid_CustomJSProperties(object sender, ASPxGridViewClientJSPropertiesEventArgs e)
        {
            e.Properties["cpHeight"] = "a";
        }

        protected void CTTRegisterGrid_SummaryDisplayText(object sender, ASPxGridViewSummaryDisplayTextEventArgs e)
        {
            e.Text = "Total" + "   " + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(e.Value));
        }
        void bind()
        {
            if (cmbtype.SelectedItem.Text.ToString() == "Exchange")
            {
                CTTRegisterDataSource.SelectCommand = "select name,CTTaxSummary_ID,CTTaxSummary_CustomerUcc,cast(CTTaxSummary_TotalCTT as decimal(18,2))as CTTaxSummary_TotalCTT, cast(CTTaxSummary_RoundOffAmount as decimal(18,2))as CTTaxSummary_RoundOffAmount ,cast(CTTaxSummary_NetCTT as decimal(18,2))as CTTaxSummary_NetCTT from Trans_CTTaxSummary as a left outer join(select ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '')   + ' ' + ISNULL(cnt_lastName, '') as name,cnt_InternalID from tbl_master_contact)as d on d.cnt_InternalID=a.CTTaxSummary_CustomerID where CTTaxSummary_CTTDate='" + cmbDate.Value + "' and CTTaxSummary_TotalCTT!='0.0000' and CTTaxSummary_SegmentID='" + HttpContext.Current.Session["usersegid"].ToString().Trim() + "' and CTTaxSummary_CompanyID='" + Session["LastCompany"].ToString() + "' and CTTaxSummary_Type='Exch' order by name";
                DataSourcedetailCTTGrid.SelectCommand = "SELECT CTTax_SecuritySymbol,CTTax_SecuritySeries,convert(varchar(9),CTTax_ExpiryDate,6) as CTTax_ExpiryDate," +
                            "(case when CTTax_StrikePrice=0.0000 then '' else convert( varchar,cast(CTTax_StrikePrice as money),1)end )as CTTax_StrikePrice," +
                            "(case when CTTax_FutureSaleValue=0.0000 then '' else convert(varchar,cast(CTTax_FutureSaleValue as money),1)end)as CTTax_FutureSaleValue," +
                            "(case when CTTax_FutureCTT=0.0000 then '' else convert(varchar,cast(CTTax_FutureCTT as money),1)end)as CTTax_FutureCTT," +
                            "(case when CTTax_OptionSaleValue=0.0000 then '' else convert(varchar,cast(CTTax_OptionSaleValue as money),1)end )as CTTax_OptionSaleValue," +
                            "(case when CTTax_OptionCTT=0.0000 then '' else convert(varchar,cast(CTTax_OptionCTT  as money),1)end )as CTTax_OptionCTT," +
                            "(case when CTTax_OptionExerciseValue=0.0000 then '' else convert(varchar,cast(CTTax_OptionExerciseValue as money),1) end)as CTTax_OptionExerciseValue," +
                            "(case when CTTax_OptionExerciseCTT=0.0000 then '' else convert(varchar,cast(CTTax_OptionExerciseCTT as money),1)end )as CTTax_OptionExerciseCTT," +
                           "(case when CTTax_TotalCTT=0.0000 then '' else convert(varchar,cast(CTTax_TotalCTT as money ),1)end )as CTTax_TotalCTT" +
                             " FROM [Trans_CTTax] Where CTTax_CustomerID =(select CTTaxSummary_CustomerID from Trans_CTTaxSummary where CTTaxSummary_ID='" + Session["KeyVal"] + "' and  CTTaxSummary_Type='Exch')" +
                             "and CTTax_CTTDate='" + cmbDate.Value + "' and CTTax_CompanyID='" + Session["LastCompany"].ToString() + "' and CTTax_TotalCTT!='0.0000' and CTTax_SegmentID='" + HttpContext.Current.Session["usersegid"].ToString().Trim() + "' and CTTax_Type='Exch'";


            }
            else if (cmbtype.SelectedItem.Text.ToString() == "Provisional")
            {
                CTTRegisterDataSource.SelectCommand = "select name,CTTaxSummary_ID,CTTaxSummary_CustomerUcc,cast(CTTaxSummary_TotalCTT as decimal(18,2))as CTTaxSummary_TotalCTT, cast(CTTaxSummary_RoundOffAmount as decimal(18,2))as CTTaxSummary_RoundOffAmount ,cast(CTTaxSummary_NetCTT as decimal(18,2))as CTTaxSummary_NetCTT from Trans_CTTaxSummary as a left outer join(select ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '')   + ' ' + ISNULL(cnt_lastName, '') as name,cnt_InternalID from tbl_master_contact)as d on d.cnt_InternalID=a.CTTaxSummary_CustomerID where CTTaxSummary_CTTDate='" + cmbDate.Value + "' and CTTaxSummary_TotalCTT!='0.0000' and CTTaxSummary_SegmentID='" + HttpContext.Current.Session["usersegid"].ToString().Trim() + "' and CTTaxSummary_CompanyID='" + Session["LastCompany"].ToString() + "' and CTTaxSummary_Type='Prov' order by name";
                DataSourcedetailCTTGrid.SelectCommand = "SELECT CTTax_SecuritySymbol,CTTax_SecuritySeries,convert(varchar(9),CTTax_ExpiryDate,6) as CTTax_ExpiryDate," +
                            "(case when CTTax_StrikePrice=0.0000 then '' else convert( varchar,cast(CTTax_StrikePrice as money),1)end )as CTTax_StrikePrice," +
                            "(case when CTTax_FutureSaleValue=0.0000 then '' else convert(varchar,cast(CTTax_FutureSaleValue as money),1)end)as CTTax_FutureSaleValue," +
                            "(case when CTTax_FutureCTT=0.0000 then '' else convert(varchar,cast(CTTax_FutureCTT as money),1)end)as CTTax_FutureCTT," +
                            "(case when CTTax_OptionSaleValue=0.0000 then '' else convert(varchar,cast(CTTax_OptionSaleValue as money),1)end )as CTTax_OptionSaleValue," +
                            "(case when CTTax_OptionCTT=0.0000 then '' else convert(varchar,cast(CTTax_OptionCTT  as money),1)end )as CTTax_OptionCTT," +
                            "(case when CTTax_OptionExerciseValue=0.0000 then '' else convert(varchar,cast(CTTax_OptionExerciseValue as money),1) end)as CTTax_OptionExerciseValue," +
                            "(case when CTTax_OptionExerciseCTT=0.0000 then '' else convert(varchar,cast(CTTax_OptionExerciseCTT as money),1)end )as CTTax_OptionExerciseCTT," +
                           "(case when CTTax_TotalCTT=0.0000 then '' else convert(varchar,cast(CTTax_TotalCTT as money ),1)end )as CTTax_TotalCTT" +
                             " FROM [Trans_CTTax] Where CTTax_CustomerID =(select CTTaxSummary_CustomerID from Trans_CTTaxSummary where CTTaxSummary_ID='" + Session["KeyVal"] + "' and  CTTaxSummary_Type='Prov')" +
                             "and CTTax_CTTDate='" + cmbDate.Value + "' and CTTax_CompanyID='" + Session["LastCompany"].ToString() + "' and CTTax_TotalCTT!='0.0000' and CTTax_SegmentID='" + HttpContext.Current.Session["usersegid"].ToString().Trim() + "' and CTTax_Type='Prov'";


            }
            else
            {
                if (Chkdiff.Checked == true)
                {
                    CTTDataSource.SelectCommand = "select distinct (ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '')   + ' ' + ISNULL(cnt_lastName, '')) as Name,CTTaxSummary_CustomerUcc as UCC,exchtotalCTT as ExchCTT,provtotalCTT as ProvCTT, " +
                " Diffr,provclient from" +
                "(select  (isnull(exchtotalCTT,0.0)-isnull(provtotalCTT,0.0)) as Diffr,case when exchtotalCTT is null then 0.0 else exchtotalCTT end  as exchtotalCTT," +
                "case when provtotalCTT  is null then 0.0 else provtotalCTT end  as provtotalCTT,case when provclient is null then  exchclient else provclient end as provclient from(select CTTaxSummary_CustomerID as provclient,sum(isnull(CTTaxSummary_NetCTT,0.0)) as provtotalCTT from Trans_CTTaxSummary,tbl_master_contact where CTTaxSummary_CustomerID=cnt_InternalID" +
                " and CTTaxSummary_CustomerID=cnt_internalid and CTTaxSummary_Type='Prov' and CTTaxSummary_CTTDate='" + cmbDate.Value + "' and CTTaxSummary_TotalCTT!='0.0000' and CTTaxSummary_SegmentID='" + HttpContext.Current.Session["usersegid"].ToString().Trim() + "' group by CTTaxSummary_CustomerID,CTTaxSummary_CntrNo) as tb" +
                " full outer join (select CTTaxSummary_CustomerID as exchclient,sum(isnull(CTTaxSummary_NetCTT,0.0)) as exchtotalCTT from Trans_CTTaxSummary,tbl_master_contact where CTTaxSummary_CustomerID=cnt_internalid " +
                " and CTTaxSummary_Type='Exch' and CTTaxSummary_CTTDate='" + cmbDate.Value + "' and CTTaxSummary_TotalCTT!='0.0000' and CTTaxSummary_SegmentID='" + HttpContext.Current.Session["usersegid"].ToString().Trim() + "' group by CTTaxSummary_CustomerID,CTTaxSummary_CntrNo) as tb1 on(provclient=exchclient)) as tab," +
                " Trans_CTTaxSummary,tbl_master_contact where CTTaxSummary_CustomerID=provclient and CTTaxSummary_CustomerID=cnt_internalid and CTTaxSummary_CTTDate='" + cmbDate.Value + "'" +
                " and CTTaxSummary_TotalCTT!='0.0000'and CTTaxSummary_SegmentID='" + HttpContext.Current.Session["usersegid"].ToString().Trim() + "' and CTTaxSummary_CompanyID='" + Session["LastCompany"].ToString() + "' and Diffr<>0.0";
                }
                else
                {
                    CTTDataSource.SelectCommand = "select distinct (ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '')   + ' ' + ISNULL(cnt_lastName, '')) as Name,CTTaxSummary_CustomerUcc as UCC,exchtotalCTT as ExchCTT,provtotalCTT as ProvCTT, " +
                " Diffr,provclient from" +
                "(select (isnull(exchtotalCTT,0.0)-isnull(provtotalCTT,0.0)) as Diffr,case when exchtotalCTT is null then 0.0 else exchtotalCTT end  as exchtotalCTT," +
                "case when provtotalCTT  is null then 0.0 else provtotalCTT end  as provtotalCTT,case when provclient is null then  exchclient else provclient end as provclient from(select CTTaxSummary_CustomerID as provclient,sum(isnull(CTTaxSummary_NetCTT,0.0)) as provtotalCTT from Trans_CTTaxSummary,tbl_master_contact where CTTaxSummary_CustomerID=cnt_InternalID" +
                " and CTTaxSummary_CustomerID=cnt_internalid and CTTaxSummary_Type='Prov' and CTTaxSummary_CTTDate='" + cmbDate.Value + "' and CTTaxSummary_TotalCTT!='0.0000' and CTTaxSummary_SegmentID='" + HttpContext.Current.Session["usersegid"].ToString().Trim() + "' group by CTTaxSummary_CustomerID,CTTaxSummary_CntrNo) as tb" +
                " full outer join (select CTTaxSummary_CustomerID as exchclient,sum(isnull(CTTaxSummary_NetCTT,0.0)) as exchtotalCTT from Trans_CTTaxSummary,tbl_master_contact where CTTaxSummary_CustomerID=cnt_internalid " +
                " and CTTaxSummary_Type='Exch' and CTTaxSummary_CTTDate='" + cmbDate.Value + "' and CTTaxSummary_TotalCTT!='0.0000' and CTTaxSummary_SegmentID='" + HttpContext.Current.Session["usersegid"].ToString().Trim() + "' group by CTTaxSummary_CustomerID,CTTaxSummary_CntrNo) as tb1 on(provclient=exchclient)) as tab," +
                " Trans_CTTaxSummary,tbl_master_contact where CTTaxSummary_CustomerID=provclient  and CTTaxSummary_CustomerID=cnt_internalid and CTTaxSummary_CTTDate='" + cmbDate.Value + "'" +
                " and CTTaxSummary_TotalCTT!='0.0000'and CTTaxSummary_SegmentID='" + HttpContext.Current.Session["usersegid"].ToString().Trim() + "' and CTTaxSummary_CompanyID='" + Session["LastCompany"].ToString() + "'";

                }

            }
        }
        protected void CTTRegister_SummaryDisplayText(object sender, ASPxGridViewSummaryDisplayTextEventArgs e)
        {
            e.Text = "  " + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(e.Value));
        }
        protected void CTTRegister_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            bind();


            if (e.Parameters == "s")
            {
                grdCTTRegister.Settings.ShowFilterRow = true;
            }

            if (e.Parameters == "All")
            {
                grdCTTRegister.FilterExpression = string.Empty;
            }
            grdCTTRegister.DataBind();
        }
        protected void CTTRegister_CustomJSProperties(object sender, ASPxGridViewClientJSPropertiesEventArgs e)
        {
            e.Properties["cpHeight1"] = "b";
        }
    }
}