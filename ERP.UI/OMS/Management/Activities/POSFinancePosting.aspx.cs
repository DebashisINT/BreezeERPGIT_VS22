using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;
using BusinessLogicLayer;
using ClsDropDownlistNameSpace;
using EntityLayer.CommonELS;
using System.Web.Services;
using System.Collections.Generic;
using System.Collections;
using DevExpress.Web.Data;
using DataAccessLayer;
using System.ComponentModel;
using System.Linq;
using EntityLayer;
using System.Collections.Specialized;
using System.Text.RegularExpressions;
using System.Globalization;
namespace ERP.OMS.Management.Activities
{
    public partial class POSFinancePosting : System.Web.UI.Page
    {

        SalesReturnBL objSalesReturnBL = new SalesReturnBL();
        protected void Page_Load(object sender, EventArgs e)
        {

            if(!Page.IsPostBack)
            {
                trmsgid.Visible = false;

             //  btnSubmit.Enabled= false;
            }

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            int id = 0;

            string SalesReturnNumber = txtReturnNo.Text.Trim();
            id = objSalesReturnBL.AdjustFinanceBill(SalesReturnNumber);
            if(id==1)
            {
                trmsgid.Visible = true;
                lblMessage.Text = "Already posted.";
            }
            else if(id == 2)
            {
                trmsgid.Visible = true;
                lblMessage.Text = "Posted successfully.";
            
            }
            if (id == -1)
            {
                trmsgid.Visible = true;
                lblMessage.Text = "Document Number not exist.";
            }

        }

     
      
    }
}