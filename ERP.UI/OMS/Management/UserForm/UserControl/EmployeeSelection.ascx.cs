using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ERP.OMS.Management.UserForm.UserControl
{
    public partial class EmployeeSelection : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        public void SetName(string str, string val)
        {
            txtEmployeeName.Text = str;
            HdEmpId.Value = val;
        }

        public void SetClientSideEventChange(string function)
        {
            txtEmployeeName.ClientSideEvents.TextChanged = function;

        }

        public void SetClientsideEventGotFocus(string function)
        {
            txtEmployeeName.ClientSideEvents.GotFocus = function;

        }

    }
}