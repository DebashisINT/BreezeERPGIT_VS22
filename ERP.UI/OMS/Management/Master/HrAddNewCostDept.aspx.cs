using System;
using System.Web;
using System.Web.UI;
using BusinessLogicLayer;
using ClsDropDownlistNameSpace;
using System.Web.UI.WebControls;

namespace ERP.OMS.Management.Master
{
    public partial class management_master_HrAddNewCostDept : ERP.OMS.ViewState_class.VSPage //System.Web.UI.Page
    {
        //DBEngine oDBEngine = null;
        //GenericMethod oGenericMethod = null;
        BusinessLogicLayer.DBEngine oDBEngine = null;
        BusinessLogicLayer.GenericMethod oGenericMethod = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (HttpContext.Current.Session["userid"] == null)
                {
                    //Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
                }
                //this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");
           
                BindData();
            }
        }
        public void BindData()
        {
            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(null);
            BusinessLogicLayer.GenericMethod oGenericMethod = new BusinessLogicLayer.GenericMethod();

            clsDropDownList OclsDropDownList = new clsDropDownList();


            string[,] Principal = oDBEngine.GetFieldValue("tbl_master_costCenter", "cost_id,cost_description", "cost_costCenterType='Department'", 2, "cost_description");

           
            //oDBEngine.AddDataToDropDownList(Principal, DDLCostDept);
           
            OclsDropDownList.AddDataToDropDownList(Principal, DDLCostDept);
            DDLCostDept.Items.Add(new ListItem("None", "0"));
            DDLCostDept.SelectedValue = "0";

            
            string[,] Head = oDBEngine.GetFieldValue("tbl_master_contact", "cnt_internalId,cnt_firstName+' '+isnull(cnt_middleName,'')+' '+isnull(cnt_lastName,'')+' ['+isnull(rtrim(cnt_shortName),'')+']' as cnt_firstName", "cnt_internalId LIKE '%em_%'", 2, "cnt_firstName");
            //  oDBEngine.AddDataToDropDownList(Head, DDLHeadDept);
      OclsDropDownList.AddDataToDropDownList(Head, DDLHeadDept);

            string CombinedQuery = oGenericMethod.GetAllContact("EM");
            if (CombinedQuery != String.Empty)
                txtDptHead.Attributes.Add("onkeyup", "CallAjax(this,'GenericAjaxList',event,'" + CombinedQuery + "')");

        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("HRCostCenter.aspx", false);
        }
       
        protected void BtnSave_Click(object sender, EventArgs e)
        {
            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(null);
            int IsCostCenterInsert = 0;
            int IsEmailInsert = 0;
            bool BothSucces = false;
            try
            {
                DateTime Date = Convert.ToDateTime(Session["ServerDate"].ToString());
                string ChkVal = "";
                if (ChkFund.Checked == true)
                {
                    ChkVal += "1";
                }
                if (ChkBrok.Checked == true)
                {
                    if (ChkVal != "")
                        ChkVal += ",2";
                    else
                        ChkVal += "2";
                }
                if (ChkInsu.Checked == true)
                {
                    if (ChkVal != "")
                        ChkVal += ",3";
                    else
                        ChkVal += "3";
                }
                if (ChkDepos.Checked == true)
                {
                    if (ChkVal != "")
                        ChkVal += ",4";
                    else
                        ChkVal += "4";
                }
                //string DptHeadHidden = Convert.ToString(txtDptHead_hidden.Value);
                //string DptHead = string.Empty;
                //if (!string.IsNullOrEmpty(DptHeadHidden) && DptHeadHidden.Split('~').Length > 3)
                //{
                //    DptHead = txtDptHead_hidden.Value.Split('~')[4];
                //}
                //else
                //{

                //    DptHead = "";
                
                //}
                IsCostCenterInsert = oDBEngine.InsurtFieldValue("tbl_master_costCenter", @"cost_costCenterType,cost_description,cost_costCenterHead,cost_principlalDepartment,
            cost_operationIn,LastModifyDate,LastModifyUser,Cost_Email",
                "'" + DDLType.SelectedItem.Text + "','" + TxtCenter.Text + "','" + DDLHeadDept.SelectedItem.Value + "','" + DDLCostDept.SelectedItem.Value +
                "','" + ChkVal + "','" + Date + "','" + Session["userid"].ToString() + "','" + TxtEmail.Text + "'");
                if (IsCostCenterInsert > 0)
                    BothSucces = true;
                else
                    BothSucces = false;
                IsEmailInsert = oDBEngine.InsurtFieldValue("tbl_master_email", @"eml_email,LastModifyDate,LastModifyUser,eml_cntID,eml_entity",
                    "'" + TxtEmail.Text + "','" + Date + "','" + Session["userid"].ToString() + "','" + DDLCostDept.SelectedItem.Value + "','" + DDLType.SelectedItem.Text + "'");
                if (IsEmailInsert > 0)
                    if (BothSucces)
                        BothSucces = true;
                    else
                        BothSucces = false;


                if (BothSucces)
                    //this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>Message(1);</script>");
                    ScriptManager.RegisterStartupScript(this, GetType(), "JScript", "alert('Saved successfully.');window.location ='HRCostCenter.aspx';", true);
                else
                    this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>Message(0);</script>");
                //Response.Redirect("HRCostCenter.aspx");

            }
            catch (Exception exc)
            {
                lblmessage.Text = exc.Message;
            }
        }
    }
}