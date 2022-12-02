using System;
using System.Web;
using System.Web.UI;
using System.Configuration;
using BusinessLogicLayer;
using ClsDropDownlistNameSpace;
using System.Web.UI.WebControls;

namespace ERP.OMS.Management.Master
{
    public partial class management_master_HrCost : ERP.OMS.ViewState_class.VSPage //System.Web.UI.Page
    {
        // DBEngine oDBEngine = new DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]); MULTI
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        clsDropDownList OclsDropDownList = new clsDropDownList();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindData();
            }
        }
        public void BindData()
        {
            string KeyId = "";
            if (Request.QueryString["id"] != null)
            {
                KeyId = Convert.ToString(Request.QueryString["id"]);
                Session["KeyVal"] = Convert.ToString(KeyId);
            }
            else
            {
                KeyId = Convert.ToString(Session["KeyVal"]);
            }
            string[,] Principal = oDBEngine.GetFieldValue("tbl_master_costCenter", "cost_id,cost_description", "cost_costCenterType='Department'", 2, "cost_description");
            // oDBEngine.AddDataToDropDownList(Principal, DDLCostDept);

           
            OclsDropDownList.AddDataToDropDownList(Principal, DDLCostDept);
            DDLCostDept.Items.Add(new ListItem("None", "0"));

            string[,] Head = oDBEngine.GetFieldValue("tbl_master_contact", "cnt_internalId,cnt_firstName+' '+isnull(cnt_middleName,'')+' '+isnull(cnt_lastName,'')+' ['+isnull(rtrim(cnt_shortName),'')+']' as cnt_firstName", "cnt_internalId LIKE '%em_%'", 2, "cnt_firstName");
            //  oDBEngine.AddDataToDropDownList(Head, DDLHeadDept);
            OclsDropDownList.AddDataToDropDownList(Head, DDLHeadDept);


            string[,] getValue = oDBEngine.GetFieldValue("tbl_master_costCenter", "cost_costCenterType,cost_description,cost_costCenterHead,cost_principlalDepartment,cost_operationIn,cost_email", "cost_id=" + KeyId, 6);
            if (getValue[0, 0] != "n")
            {
                if (getValue[0, 0].ToString().Trim() != "")
                    DDLType.SelectedItem.Text = getValue[0, 0];
                if (getValue[0, 1].ToString().Trim() != "")
                    TxtCenter.Text = getValue[0, 1];
                if (getValue[0, 2].ToString().Trim() != "")
                {
                    string[,] CDepart = oDBEngine.GetFieldValue("tbl_master_contact", "cnt_firstName+' '+isnull(cnt_middleName,'')+' '+isnull(cnt_lastName,'')+' ['+isnull(rtrim(cnt_shortName),'')+']' as cnt_firstName", "cnt_internalId='" + getValue[0, 2].ToString().Trim() + "'", 1);
                    if (CDepart[0, 0] != "n")
                        DDLHeadDept.SelectedItem.Text = CDepart[0, 0];
                }
                if (getValue[0, 3].ToString().Trim() != "")
                {
                    string[,] CDepart = oDBEngine.GetFieldValue("tbl_master_costCenter", "cost_description", "cost_id=" + getValue[0, 3].ToString().Trim(), 1);
                    if (CDepart[0, 0] != "n") { DDLCostDept.SelectedIndex = DDLCostDept.Items.IndexOf(DDLCostDept.Items.FindByText(CDepart[0, 0])); 
                        //DDLCostDept.SelectedItem.Text = CDepart[0, 0]; 
                    }
                    else
                    {

                        DDLCostDept.SelectedIndex = DDLCostDept.Items.IndexOf(DDLCostDept.Items.FindByText("None")); 
                    }
                        
              //  DDLCostDept.SelectedItem.Value =getValue[0, 3].ToString().Trim();
                    //string[,] Principal1 = oDBEngine.GetFieldValue("tbl_master_costCenter", "cost_description", "cost_id=" + getValue[0, 3].ToString().Trim(), 1);
                    //if (Principal1[0, 0] != "n")
                    //    DDLHeadDept.SelectedItem.Text = Principal1[0, 0];
                }

                string[] st = getValue[0, 4].ToString().Split(',');

                if (getValue[0, 5].ToString().Trim() != "")
                    TxtEmail.Text = getValue[0, 5];
                for (int i = 0; i <= st.GetUpperBound(0); i++)
                {
                    string chkVal = st[i];
                    if (chkVal == "1")
                    {
                        ChkFund.Checked = true;
                    }
                    if (chkVal == "2")
                    {
                        ChkBrok.Checked = true;
                    }
                    if (chkVal == "3")
                    {
                        ChkInsu.Checked = true;
                    }
                    if (chkVal == "4")
                    {
                        ChkDepos.Checked = true;
                    }
                }
            }
            //DBEngine objEngine3 = new DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
            string[,] cntID = oDBEngine.GetFieldValue("tbl_master_CostCenter", "Ltrim(Rtrim(cost_costCenterHead))", "Cost_ID='" + KeyId + "'", 1);
            //string[,] email = oDBEngine.GetFieldValue("tbl_master_email", "Top 1 eml_email ", "eml_cntid='" + cntID[0, 0] + "'", 1,"eml_id desc");
            //if (email[0, 0] != "n")
            //{
            //   // TxtEmail.Text = email[0, 0];
            //}
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("HRCostCenter.aspx", false);
        }
        protected void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                //DateTime Date = Convert.ToDateTime(oDBEngine.GetDate().ToShortDateString());
                DateTime Date = Convert.ToDateTime(oDBEngine.GetDate().ToShortDateString());
                 string KeyId ="";
                if (Request.QueryString["id"] != null)
                {
                    KeyId = Request.QueryString["id"].ToString();
                }
                else
                {
                    KeyId = Convert.ToString(Session["KeyVal"]);
                }

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
                oDBEngine.SetFieldValue("tbl_master_costCenter", "cost_costCenterType='" + DDLType.SelectedItem.Text + "',Cost_Email='" + TxtEmail.Text + "',cost_description='" + TxtCenter.Text + "',cost_costCenterHead='" + DDLHeadDept.SelectedItem.Value + "',cost_principlalDepartment='" + DDLCostDept.SelectedItem.Value + "',cost_operationIn='" + ChkVal + "',LastModifyDate='" + Date + "',LastModifyUser='" + Session["userid"].ToString() + "'", " cost_id='" + KeyId + "'");
               // oDBEngine.SetFieldValue("tbl_master_email", "eml_email='" + TxtEmail.Text + "',LastModifyDate='" + Date + "',LastModifyUser='" + Session["userid"].ToString() + "'", " eml_cntid='" + KeyId + "'");
                //Response.Redirect("HRCostCenter.aspx");
                ScriptManager.RegisterStartupScript(this, GetType(), "JScript", "alert('Record Updated Successfully');", true);
            }
            catch (Exception exc)
            {
                lblmessage.Text = exc.Message;
            }
        }
    }
}