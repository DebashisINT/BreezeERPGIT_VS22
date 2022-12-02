using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using BusinessLogicLayer;
using ClsDropDownlistNameSpace;


namespace ERP.OMS.Management
{
    public partial class management_Contact_CommissionProfile : System.Web.UI.Page
    {
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        BusinessLogicLayer.Converter OConvert = new BusinessLogicLayer.Converter();
        BusinessLogicLayer.Converter oconverter = new BusinessLogicLayer.Converter();
        clsDropDownList clsDropDownList = new clsDropDownList();
        Management_BL Management_BL = new Management_BL();
        protected void Page_Load(object sender, EventArgs e)
        {

            txtFromDate.EditFormatString = oconverter.GetDateFormat("Date");
            txtFromDate.Value = oDBEngine.GetDate();
            string[,] Data = oDBEngine.GetFieldValue("tbl_master_Company", "cmp_id, cmp_name", null, 2, "cmp_name");
            clsDropDownList.AddDataToDropDownList(Data, cmbOrganization);

            Data = oDBEngine.GetFieldValue("tbl_master_groupMaster", "gpm_id,  gpm_Description+ '['+gpm_code+']'", null, 2, "gpm_Description");
            clsDropDownList.AddDataToDropDownList(Data, drpGroupCode);

            string st = Session["KeyVal_InternalID"].ToString();
            if (Request.QueryString["id1"] != null)
            {
                string DocementTypeId = Request.QueryString["id1"].ToString();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            //String con = ConfigurationSettings.AppSettings["DBConnectionDefault"];MULTI
            String con = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            //SqlConnection lcon = new SqlConnection(con);
            //lcon.Open();
            //SqlCommand lcmdEmplInsert = new SqlCommand("[insertCommission]", lcon);
            //lcmdEmplInsert.CommandType = CommandType.StoredProcedure;
            //lcmdEmplInsert.Parameters.AddWithValue("@CommissionProfile_CntID", Session["KeyVal_InternalID"].ToString());
            //lcmdEmplInsert.Parameters.AddWithValue("@CommissionProfile_CompanyID", Session["LastCompany"].ToString());
            //lcmdEmplInsert.Parameters.AddWithValue("@CommissionProfile_Type", 1);
            //lcmdEmplInsert.Parameters.AddWithValue("@CommissionProfile_GroupCode", drpGroupCode.SelectedItem.Value);
            //lcmdEmplInsert.Parameters.AddWithValue("@CommissionProfile_FromDate", txtFromDate.Value);
            //lcmdEmplInsert.Parameters.AddWithValue("@CommissionProfile_CreateUser", Session["userid"]);
            //lcmdEmplInsert.Parameters.AddWithValue("@CommissionProfile_CreateDateTime", oDBEngine.GetDate());
            //lcmdEmplInsert.ExecuteNonQuery();
            Management_BL.management_insertCommission(Session["KeyVal_InternalID"].ToString(), Session["LastCompany"].ToString(),
                drpGroupCode.SelectedItem.Value.ToString(), Convert.ToDateTime(txtFromDate.Value), Convert.ToInt32(Session["userid"].ToString()),
                Convert.ToDateTime(oDBEngine.GetDate()));

            string popupScript = "";
            string query = Request.QueryString["id"].ToString();
            //if (Request.QueryString["mode"].ToString() != "")
            //{
            //    query = query + "&mode=" + Request.QueryString["mode"].ToString();
            //}
            if (Session["KeyVal2"] != null)
            {
                popupScript = "<script language='javascript'>" + "alert('Successfully Added');window.parent.Getvalue();window.parent.popup.Hide();</script>";
                ClientScript.RegisterStartupScript(GetType(), "PopupScript", popupScript);
            }
            else
            {
                popupScript = "<script language='javascript'>" + "alert('Successfully Added');window.parent.location.href='" + query + "';window.parent.popup.Hide();</script>";
                ClientScript.RegisterStartupScript(GetType(), "PopupScript", popupScript);
            }

        }
    }
}