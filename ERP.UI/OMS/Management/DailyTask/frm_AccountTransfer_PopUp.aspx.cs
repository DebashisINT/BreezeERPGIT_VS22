using System;
using System.Data;
using System.Web;
using System.Web.UI;
namespace ERP.OMS.Management.DailyTask
{

    public partial class management_DailyTask_frm_AccountTransfer_PopUp : ERP.OMS.ViewState_class.VSPage
    {
        static DataTable dt1 = new DataTable();
        clsNsdlHolding objclsNsdlHolding = new clsNsdlHolding();
        clsCdslHolding objclsCdslHolding = new clsCdslHolding();
        DataView dv = new DataView();
        DataView dv1 = new DataView();
        protected void Page_Load(object sender, EventArgs e)
        {
            string dp = null;
            dp = HttpContext.Current.Session["userlastsegment"].ToString();
            if (dp.Trim() == "9" || dp.Trim() == "NSDL")
            {
                gridHolding.DataSource = objclsNsdlHolding.ShowHolding(Request.QueryString["BenAccNo"].ToString()); ;
                gridHolding.DataBind();
                gridHolding.FilterExpression = string.Empty;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "hi", "height();", true);
            }
            else
            {
                string BOID = HttpContext.Current.Session["usersegid"].ToString().Trim() + Request.QueryString["BenAccNo"].ToString().Trim();
                DataTable DT = new DataTable();
                DT = objclsCdslHolding.showCdslHolding(BOID);
                DT.Columns[0].ColumnName = "NsdlHolding_ISIN";
                DT.Columns[1].ColumnName = "CmpName";
                DT.Columns[2].ColumnName = "NsdlHolding_SettlementNumber";
                DT.Columns[3].ColumnName = "Total";
                DT.Columns[4].ColumnName = "Free";
                DT.Columns[5].ColumnName = "Pledged";
                DT.Columns[5].ColumnName = "Type";
                DT.Columns[7].ColumnName = "Remat";
                DT.Columns[8].ColumnName = "Demat";
                DT.Columns[10].ColumnName = "ISINValue";

                gridHolding.DataSource = DT;
                gridHolding.DataBind();
                gridHolding.FilterExpression = string.Empty;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "hi", "height();", true);
            }
        }

        protected void gridHolding_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {


        }

    }

}