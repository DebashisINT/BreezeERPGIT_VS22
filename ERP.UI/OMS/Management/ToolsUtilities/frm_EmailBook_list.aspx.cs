using System;
using System.Data;
using System.Web.UI.WebControls;
using System.Configuration;
using BusinessLogicLayer;

namespace ERP.OMS.Management.ToolsUtilities
{
    public partial class management_toolsutilities_frm_EmailBook_list : System.Web.UI.Page
    {

        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);multi
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DataTable DT = oDBEngine.GetDataTable(" tbl_master_email,tbl_master_employee,tbl_master_contact   ", " distinct eml_email as Email,eml_cntId,eml_type as type," +
                        " (RTRIM(isnull(cnt_firstName,''))+' '+RTRIM(isnull(cnt_middleName,''))+' '+RTRIM(isnull(cnt_lastName,''))) as name," +
                        " RTRIM(isnull(cnt_shortname,'')) as code,(select branch_description from tbl_master_branch where branch_id=cnt_branchid) as branch," +
                        " (select cmp_name from tbl_master_company where cmp_id=(select top 1 emp_organization from tbl_trans_employeeCTC where emp_cntId=emp_contactId order by emp_effectiveDate desc )) as comp ", " emp_contactId=eml_cntId and (emp_dateofleaving is null or emp_dateofleaving>getdate() or emp_dateofleaving='1/1/1900 12:00:00 AM') and eml_type<>'' and eml_type <> 'Web Site' and cnt_internalId=emp_contactId ", " eml_cntId ");
                ViewState["table"] = DT;
                ViewState["sort"] = "";

                bindgrid();
            }
        }

        private void bindgrid()
        {
            // Replace .ToString() with Convert.ToString(..) & Change ViewState Handling By Sudip on 15122016

            DataTable dt = new DataTable();

            if (ViewState["table"] != null)
            {
                dt = ViewState["table"] as DataTable;
            }
            
            if (ViewState["sort"] != null && Convert.ToString(ViewState["sort"]) != "")
            {
                dt.DefaultView.Sort = Convert.ToString(ViewState["sort"]);
            }

            grdListData.DataSource = dt;
            grdListData.DataBind();
        }
        protected void grdListData_Sorting(object sender, GridViewSortEventArgs e)
        {
            if (ViewState["sort"].ToString() == (e.SortExpression.ToString() + " ASC"))
            {
                ViewState["sort"] = e.SortExpression + " DESC";
            }
            else // Handles cases where the previous sort expression was the current expression descending, another expression, or none at all.
            {
                ViewState["sort"] = e.SortExpression + " ASC";
            }
            bindgrid();
        }
    }
}