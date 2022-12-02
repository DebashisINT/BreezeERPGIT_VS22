using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ERP.OMS.Management.UserForm.UserControl
{
    public partial class CustomerSelection : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void SetName(string str, string val)
        {
            txtCustName.Text = str;
            HdCustId.Value = val;
        }
        public void SetClientSideEventChange(string function) 
        {
            txtCustName.ClientSideEvents.TextChanged = function;
        
        }

        public void SetClientsideEventGotFocus(string function)
        {
            txtCustName.ClientSideEvents.GotFocus = function;

        }
       
    }
}