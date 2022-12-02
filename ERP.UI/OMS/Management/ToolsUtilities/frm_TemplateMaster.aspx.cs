using System;
using System.Data;
using System.Web;
using System.Web.UI;
using BusinessLogicLayer;

namespace ERP.OMS.Management.ToolsUtilities
{
    public partial class management_utilities_frm_TemplateMaster : System.Web.UI.Page
    {
        //DBEngine oDBEngine = new DBEngine(string.Empty);
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        protected void Page_PreInit(object sender, EventArgs e)
        {
            Session["KeyVal_InternalID"] = "";
            Session["KeyVal"] = "n";
            if (!IsPostBack)
            {
                //'http://localhost:2957/InfluxCRM/management/testProjectMainPage_employee.aspx'
                string sPath = HttpContext.Current.Request.Url.ToString();
                oDBEngine.Call_CheckPageaccessebility(sPath);
            }
        }
        protected void Page_Init(object sender, EventArgs e)
        {
            gridStatusDataSource.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);

        }
        protected void Page_Load(object sender, EventArgs e)
        {

            btnReserve.Attributes.Add("onclick", "calldispose()");
            if (HttpContext.Current.Session["userid"] == null)
            {
                //Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }

            //this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");

            fillGrid();
        }
        protected void gridStatus_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {


            if (e.Parameters != null)
            {
                string[] data = e.Parameters.Split('~');
                if (data[0] == "Delete")
                {
                    oDBEngine.DeleteValue(" master_templateDetails", " Tmplt_ID='" + data[1] + "'");

                }
                else if (data[0].ToString() == "s")
                {
                    gridStatus.Settings.ShowFilterRow = true;

                }
                else if (data[0].ToString() == "All")
                {
                    gridStatus.FilterExpression = string.Empty;

                }

            }
            fillGrid();
            //Page.ClientScript.RegisterStartupScript(GetType(), "Jscript", "<script language='JavaScript'>height();</script>");


        }

        public void fillGrid()
        {
            gridStatusDataSource.SelectCommand = "select Tmplt_ID,case when Tmplt_UsedFor='EM' then 'Employee' when Tmplt_UsedFor='CL' then 'Customer'  when Tmplt_UsedFor='ND' then 'NSDL Clients' when Tmplt_UsedFor='CD' then 'CDSL Clients'  else 'Customer'  end as  Tmplt_UsedFor ,Tmplt_UsedSegment,Tmplt_UsedCompay,Tmplt_ShortName,Tmplt_Description,Tmplt_Content,Tmplt_CreateUser,Tmplt_CreateDate from master_templateDetails   ";
            gridStatus.DataBind();

        }
        protected void ASPxCallbackPanel1_CustomJSProperties(object sender, DevExpress.Web.CustomJSPropertiesEventArgs e)
        {
            e.Properties["cpLast"] = Session["KeyVal"].ToString();
        }
        protected void ASPxCallbackPanel1_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            DataTable DT = new DataTable();
            string[] data = e.Parameter.Split('~');
            if (data[0] == "Edit")
            {
                DT = oDBEngine.GetDataTable("master_templateDetails", " Tmplt_ID,Tmplt_UsedFor,case when Tmplt_UsedFor='EM' then 'Employee' when Tmplt_UsedFor='CL' then 'Customer'  when Tmplt_UsedFor='ND' then 'NSDL Clients' when Tmplt_UsedFor='CD' then 'CDSL Clients'  else 'Customer'  end as  Tmplt_UsedFor ,Tmplt_UsedSegment,Tmplt_UsedCompay,Tmplt_ShortName,Tmplt_Description,Tmplt_Content,Tmplt_CreateUser,Tmplt_CreateDate ", "Tmplt_ID='" + data[1] + "'");
                if (DT.Rows.Count > 0)
                {

                    cmbType.SelectedValue = DT.Rows[0]["Tmplt_UsedFor"].ToString();
                    txtShortName.Text = DT.Rows[0]["Tmplt_ShortName"].ToString();
                    txtContent.Text = DT.Rows[0]["Tmplt_Content"].ToString();
                }
            }
            else if (data[0] == "AddNew")
            {
                clearField();
                Session["KeyVal"] = "n";
            }
            else if (data[0] == "SaveNew")
            {

                int NoofRowsAffect = oDBEngine.InsurtFieldValue("master_templateDetails", " Tmplt_UsedFor,Tmplt_UsedSegment,Tmplt_UsedCompay,Tmplt_ShortName,Tmplt_Content,Tmplt_CreateDate,Tmplt_CreateUser ", " '" + cmbType.Text + "','" + HttpContext.Current.Session["userlastsegment"] + "','" + HttpContext.Current.Session["LastCompany"] + "','" + txtShortName.Text.ToString().Trim() + "','" + txtContent.Text.ToString().Trim() + "','" + DateTime.Now.ToString() + "','" + Session["userid"].ToString() + "'");
                //if (NoofRowsAffect > 0)
                //{
                //    this.Page.ClientScript.RegisterStartupScript(GetType(), "Script4", "<script>parent.editwin.close()</script>");
                //}
                //else
                //{
                //    this.Page.ClientScript.RegisterStartupScript(GetType(), "Script5", "<script>alert('Please Try Again.')</script>");

                //}

            }
            else if (data[0] == "SaveOld")
            {
                oDBEngine.SetFieldValue("master_templateDetails", "Tmplt_UsedFor='" + cmbType.SelectedItem.Value + "',Tmplt_ShortName='" + txtShortName.Text.ToString() + "',Tmplt_Content='" + txtContent.Text.ToString() + "',Tmplt_ModifyUser='" + Session["userid"].ToString() + "',Tmplt_ModifyDate='" + DateTime.Now.ToString() + "'", " Tmplt_ID='" + data[1] + "' ");


            }
        }

        private void clearField()
        {
            cmbType.SelectedIndex = 0;
            txtShortName.Text = "";
            txtContent.Text = "";
        }
    }
}