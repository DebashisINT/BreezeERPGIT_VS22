using System;
using System.Web.UI.WebControls;

namespace ERP.OMS.Reports
{
    public partial class Reports_mtest1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void DepartmentsGridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                SqlDataSource s = (SqlDataSource)e.Row.FindControl("EmployeesDataSource");
                System.Data.DataRowView r = (System.Data.DataRowView)e.Row.DataItem;
                s.SelectParameters["DepartmentID"].DefaultValue = r["DepartmentID"].ToString();
            }
        }

    }
}