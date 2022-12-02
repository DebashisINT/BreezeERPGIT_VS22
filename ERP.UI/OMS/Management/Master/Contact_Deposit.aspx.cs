using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
//using DevExpress.Web;
using DevExpress.Web;
using System.Configuration;

namespace ERP.OMS.Management.Master
{
    public partial class management_master_Contact_Deposit : ERP.OMS.ViewState_class.VSPage
    {
        //DBEngine oDBEngine = new DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);MULTI
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        BusinessLogicLayer.Converter objConverter = new BusinessLogicLayer.Converter();
        public string pageAccess = "";
        string JsProp = "";
        //DBEngine oDBEngine = new DBEngine(string.Empty);
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
            SqlBpDeposit.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);

            if (HttpContext.Current.Session["userid"] == null)
            {
               //Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }

            if (!IsPostBack)
            {
                if (Session["Name"] != null)
                {
                    lblName.Text = Session["Name"].ToString();
                }
                FillGrid();
            }
        }
        public void FillGrid()
        {
            SqlBpDeposit.SelectCommand = "select id,payment_R_Number as RNumber,ReceiveDate as Receive1,DOI as DOI1,deposit_status as status,case deposit_status when '0' then 'Cleared' else 'Not Cleared' end as Status1,case ReceiveDate when '01/01/1900 10:00:00 AM' then 'N/A' else convert(varchar(12),ReceiveDate,113) end as ReceiveDate ,amount as Amount,mode as Mode,branchname as BranchName,case DOI when '01/01/1900 10:00:00 AM' then 'N/A' else convert(varchar(12),DOI,113) end as DOI,chkNo as CheckNo,bankName,case bankName when null then 'N/A' else (select bnk_bankName from tbl_master_Bank where bnk_internalid=tbl_trans_BPDeposit.bankName)  end as BkName,AcNoName from tbl_trans_BPDeposit where cnt_internalId='" + HttpContext.Current.Session["KeyVal_InternalID"].ToString() + "'";
        }

        protected void gridDeposit_HtmlEditFormCreated(object sender, DevExpress.Web.ASPxGridViewEditFormEventArgs e)
        {
            TextBox bankname = (TextBox)gridDeposit.FindEditFormTemplateControl("txtBankName");
            bankname.Attributes.Add("onkeyup", "ajax_showOptions(this,'SearchByBankName',event,'" + HttpContext.Current.Session["KeyVal_InternalID"].ToString() + "')");
            TextBox BranchName = (TextBox)gridDeposit.FindEditFormTemplateControl("txtBranchName");
            BranchName.Attributes.Add("onkeyup", "ajax_showOptions(this,'SearchByBankBranchName',event)");
        }
        protected void gridDeposit_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            string Amount = Convert.ToString(e.NewValues["Amount"]);
            string tt = Convert.ToString(e.NewValues["DOI1"]);
            string Mode = Convert.ToString(e.NewValues["Mode"]);
            string[] BankName = Convert.ToString(e.NewValues["BkName"]).Split('~');
            if (BankName[0] != "")
            {
                string BankName1 = BankName[2];
                e.NewValues["BkName"] = BankName1;
            }
            string[] BranchName = Convert.ToString(e.NewValues["BranchName"]).Split('~');
            if (BranchName[0] != "")
            {
                string BranchName1 = BranchName[1];
                e.NewValues["BranchName"] = BranchName1;
            }
            JsProp = "a";
        }
        protected void gridDeposit_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            e.NewValues["status"] = "Cleared";
            e.NewValues["Mode"] = "Cheque";
            ////ASPxPageControl pc = gridDeposit.FindEditFormTemplateControl("ASPxPageControl1") as ASPxPageControl;
            ////ASPxDateEdit InsDate = (ASPxDateEdit)ASPxPageControl1.TabPages[8].FindControl("ASPxDateEdit1");
            //ASPxDateEdit InsDate = (ASPxDateEdit)gridDeposit.FindControl("ASPxDateEdit1");
            ////ASPxPageControl1
            ////ASPxDateEdit Insdate = gridDeposit.Templates.EditForm.
            //e.NewValues["DOI1"] = oDBEngine.GetDate().Date;
            //e.NewValues["Receive1"] = oDBEngine.GetDate();
            //string Mode = e.NewValues["Mode"].ToString();
        }
        protected void gridDeposit_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            string Amount = e.NewValues["Amount"].ToString();
            string Mode = e.NewValues["Mode"].ToString();
            string[] BankName = e.NewValues["BkName"].ToString().Split('~');

            if (BankName[0] != "")
            {
                try
                {
                    string BankName1 = BankName[2];
                    e.NewValues["BkName"] = BankName1;
                }
                catch
                {
                    e.NewValues["BkName"] = e.OldValues["bankName"].ToString();
                }
            }
            string[] BranchName = e.NewValues["BranchName"].ToString().Split('~');
            if (BranchName[0] != "")
            {
                try
                {
                    string BranchName1 = BranchName[1];
                    e.NewValues["BranchName"] = BranchName1;
                }
                catch
                {
                    e.NewValues["BranchName"] = BranchName[0];
                }
            }
            JsProp = "b";
        }
        protected void gridDeposit_CustomJSProperties(object sender, ASPxGridViewClientJSPropertiesEventArgs e)
        {
            e.Properties["cpSelect"] = JsProp;
        }
        protected void gridDeposit_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            JsProp = "a";
        }
    }
}