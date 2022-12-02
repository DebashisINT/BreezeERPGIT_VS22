using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ERP.OMS.Management.Attendance
{
    public partial class Frm_Attendance : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ProcedureExecute proc = new ProcedureExecute("Prc_AttendanceSystem");
                proc.AddVarcharPara("@Action", 100, "GetEmpNameByUserid");
                proc.AddIntegerPara("@User", Convert.ToInt32(Session["userid"]));
                DataTable dt = proc.GetTable();
                hdEmpName.Value = dt.Rows[0][0].ToString();
                EmpId.Value = dt.Rows[0][1].ToString();
            }
        }
    }
}