using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;


namespace ERP.OMS.Management
{
    public partial class management_frm_EmployeeReplacement : System.Web.UI.Page, System.Web.UI.ICallbackEventHandler
    {
        BusinessLogicLayer.Management_BL oManagement_BL = new BusinessLogicLayer.Management_BL();
        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);MULTI
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        string RtnData = "";

        #region AjaxList
        void CallReplacementEmployee()
        {
            string strQuery_Table = @"(Select  ISNULL(Ltrim(Rtrim(cnt_firstName)), '') + ' ' + ISNULL(Ltrim(Rtrim(cnt_middleName)), '') + ' ' + ISNULL(Ltrim(Rtrim(cnt_lastName)), '')+ ' [' + ISNULL(Ltrim(Rtrim(cnt_shortName)), '')+']' AS Name,cnt_shortName UniqueCode,emp_id as ID from tbl_master_employee,tbl_master_contact Where emp_contactId=cnt_internalId and isnull(emp_dateofLeaving,'1900-01-01 00:00:00.000')='1900-01-01 00:00:00.000') T1";
            string strQuery_FieldName = "Top 10 Name,ID,UniqueCode";
            string strQuery_WhereClause = "Name Like '%RequestLetter%' Or UniqueCode Like '%RequestLetter%'";
            string strQuery_OrderBy = "";
            string strQuery_GroupBy = "";
            string CombinedQuery = strQuery_Table + "$" + strQuery_FieldName + "$" + strQuery_WhereClause + "$" + strQuery_OrderBy + "$" + strQuery_GroupBy;
            CombinedQuery = CombinedQuery.Replace("'", "\\'");
            txtReplacement.Attributes.Add("onkeyup", "CallAjax(this,'GenericAjaxList',event,'" + CombinedQuery + "')");
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            //_____For performing operation without refreshing page___//
            String cbReference = Page.ClientScript.GetCallbackEventReference(this, "arg", "ReceiveServerData", "context");
            String callbackScript = "function CallServer(arg, context){ " + cbReference + ";}";

            cmbDOL.Date = oDBEngine.GetDate();

            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "CallServer", callbackScript, true);
            //___________-end here___//
            if (!IsPostBack)
            {
                int day = Convert.ToDateTime(Request.QueryString["id"].ToString()).Day;
                string fields = " case when (";
                string UpdateFields = "";
                for (int i = day; i <= 31; i++)
                {
                    if (i == day)
                    {
                        fields += " case when atd_statusday" + i.ToString() + " is null then 0 else	1 end ";
                        UpdateFields = " atd_statusday" + i.ToString() + "=NULL ";
                    }
                    else
                    {
                        fields += "+ case when atd_statusday" + i.ToString() + " is null then 0 else	1 end ";
                        UpdateFields += ", atd_statusday" + i.ToString() + "=NULL ";
                    }
                }
                fields += " )>0 then 'Y' else 'N' end";
                string[,] data = oDBEngine.GetFieldValue(" tbl_trans_attendance ", fields, " atd_cntId='" + HttpContext.Current.Session["KeyVal_InternalID"].ToString() + "' and  atd_Month=" + Convert.ToDateTime(Request.QueryString["id"].ToString()).Month.ToString() + " and atd_year=" + Convert.ToDateTime(Request.QueryString["id"].ToString()).Year.ToString(), 1);
                if (data[0, 0] == "Y")
                {
                    fields = " <script language='javascript'>var where_to= confirm('Attendance of this employee is present beyond the date" + Request.QueryString["id"].ToString() + ". Press OK to continue. This will remove all the attendance beyond this date!!');";
                    fields += "if (where_to== true)" +
                             "{ updateAttendanceNull('" + UpdateFields + "'); }" +
                            "else {SetValue();closeWindow();}</script>";
                    Page.ClientScript.RegisterStartupScript(GetType(), "confirmation", fields);

                }
                string[,] emplIds = oDBEngine.GetFieldValue(" tbl_master_employee ", " Distinct emp_id ", " emp_contactId in (select C.emp_cntId from tbl_trans_employeeCTC C where (C.emp_reportTo = (SELECT EMP_ID FROM tbl_master_employee where emp_contactId='" + HttpContext.Current.Session["KeyVal_InternalID"].ToString() + "')) and (C.emp_effectiveuntil is null OR C.emp_effectiveuntil='1/1/1900 12:00:00 AM'))", 1);
                if (emplIds[0, 0] == "n")
                    Page.ClientScript.RegisterStartupScript(GetType(), "popup", "<script language='javascript'>alert('No depending employees!');closeWindow();</script>");
                else
                {
                    string empIdsG = "";
                    for (int i = 0; i < emplIds.Length; i++)
                    {
                        if (empIdsG == "")
                            empIdsG = emplIds[i, 0];
                        else
                            empIdsG += "," + emplIds[i, 0];
                    }
                    childemployees.Value = empIdsG;
                }
                cmbDOL.Value = Convert.ToDateTime(Request.QueryString["id"].ToString());
                if (Convert.ToDateTime(Request.QueryString["id"].ToString()) <= oDBEngine.GetDate())
                {
                    cmbDOL.MaxDate = oDBEngine.GetDate();
                }
                else
                {
                    cmbDOL.MaxDate = Convert.ToDateTime(Request.QueryString["id"].ToString());
                    cmbDOL.MinDate = oDBEngine.GetDate().AddDays(-1);
                }
                ///////////Call Ajax Method
                CallReplacementEmployee();

            }
        }
        protected void btnupdate_Click(object sender, EventArgs e)
        {
            if (txtReplacement_hidden.Value.Trim() != "")
            {

                oDBEngine.SetFieldValue(" tbl_trans_employeeCTC ", " emp_effectiveuntil='" + Request.QueryString["id"].ToString() + "'", " emp_cntId='" + HttpContext.Current.Session["KeyVal_InternalID"].ToString() + "' and (emp_effectiveuntil is null or emp_effectiveuntil ='1/1/1900 12:00:00 AM')");
                int NoOfRowEffected = oDBEngine.SetFieldValue(" tbl_master_employee ", " emp_dateofLeaving='" + Request.QueryString["id"].ToString() + "', emp_Replacement=" + txtReplacement_hidden.Value + ",emp_RepDate='" + cmbDOL.Value + "'", " emp_contactId='" + HttpContext.Current.Session["KeyVal_InternalID"].ToString() + "'");
                if (NoOfRowEffected > 0)
                {
                    if (Convert.ToDateTime(cmbDOL.Value.ToString()) <= oDBEngine.GetDate())
                    {
                        oManagement_BL.Employee_Replacement(
                            Convert.ToString(txtReplacement_hidden.Value),
                            Convert.ToString(childemployees.Value),
                            Convert.ToString(HttpContext.Current.Session["userid"]),
                            Convert.ToString(Convert.ToDateTime(Request.QueryString["id"].ToString()).ToString("yyyy-MM-dd")),
                          "Replacement"
                            );
                        //SqlConnection lcon = new SqlConnection(ConfigurationSettings.AppSettings["DBConnectionDefault"]);
                        //lcon.Open();

                        //SqlCommand lcmd = new SqlCommand("Employee_Replacement", lcon);
                        //lcmd.CommandType = CommandType.StoredProcedure;
                        //lcmd.Parameters.Add("@ReplacementId", SqlDbType.VarChar).Value = txtReplacement_hidden.Value;
                        //lcmd.Parameters.Add("@Users", SqlDbType.VarChar).Value = childemployees.Value;
                        //lcmd.Parameters.Add("@modifyUser", SqlDbType.Int).Value = int.Parse(HttpContext.Current.Session["userid"].ToString());
                        //lcmd.Parameters.Add("@EffectiveFrom", SqlDbType.DateTime).Value = Convert.ToDateTime(Request.QueryString["id"].ToString());
                        //lcmd.Parameters.Add("@remarks", SqlDbType.VarChar).Value = "Replacement";
                        //lcmd.ExecuteNonQuery();

                        //lcmd.Dispose();
                        //lcon.Close();
                        //lcon.Dispose();
                    }
                }
                Page.ClientScript.RegisterStartupScript(GetType(), "popup", "<script language='javascript'>parent.editwin.close();</script>");
            }
            else
                Page.ClientScript.RegisterStartupScript(GetType(), "popup", "<script language='javascript'>alert('Unable To Save.Replacement Required!!!')</script>");
        }
        string ICallbackEventHandler.GetCallbackResult()
        {
            return RtnData;

        }
        void ICallbackEventHandler.RaiseCallbackEvent(string eventArgument)
        {
            string id = eventArgument.ToString();
            string[] idlist = id.Split('~');
            #region Edit
            if (idlist[0] == "Edit")
            {
                oDBEngine.SetFieldValue(" tbl_trans_attendance ", idlist[1], " atd_cntId='" + HttpContext.Current.Session["KeyVal_InternalID"].ToString() + "' and  atd_Month=" + Convert.ToDateTime(Request.QueryString["id"].ToString()).Month.ToString() + " and atd_year=" + Convert.ToDateTime(Request.QueryString["id"].ToString()).Year.ToString());
                string FIELDS = "[atd_StatusDay1] = NULL,[atd_StatusDay2] = NULL,[atd_StatusDay3] = NULL,[atd_StatusDay4] = NULL" +
                                ",[atd_StatusDay5] = NULL,[atd_StatusDay6] = NULL,[atd_StatusDay7] = NULL,[atd_StatusDay8] = NULL" +
                                ",[atd_StatusDay9] = NULL,[atd_StatusDay10] = NULL ,[atd_StatusDay11] = NULL,[atd_StatusDay12] = NULL" +
                                ",[atd_StatusDay13] = NULL,[atd_StatusDay14] = NULL,[atd_StatusDay15] = NULL,[atd_StatusDay16] = NULL" +
                                ",[atd_StatusDay17] = NULL,[atd_StatusDay18] = NULL,[atd_StatusDay19] = NULL,[atd_StatusDay20] = NULL" +
                                ",[atd_StatusDay21] = NULL,[atd_StatusDay22] = NULL,[atd_StatusDay23] = NULL,[atd_StatusDay24] = NULL" +
                                ",[atd_StatusDay25] = NULL,[atd_StatusDay26] = NULL,[atd_StatusDay27] = NULL,[atd_StatusDay28] = NULL" +
                                ",[atd_StatusDay29] = NULL,[atd_StatusDay30] = NULL,[atd_StatusDay31] = NULL";
                oDBEngine.SetFieldValue(" tbl_trans_attendance ", FIELDS, " atd_cntId='" + HttpContext.Current.Session["KeyVal_InternalID"].ToString() + "' and  atd_Month>" + Convert.ToDateTime(Request.QueryString["id"].ToString()).Month.ToString() + " and atd_year=" + Convert.ToDateTime(Request.QueryString["id"].ToString()).Year.ToString());
                RtnData = "yes";
            }
            #endregion
        }
    }
}
