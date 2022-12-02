using DataAccessLayer;
using ERP.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ERP.OMS.Management.Master
{
    public partial class UserSyncList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void EntityServerModeDataSource_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "cnt_id";
            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            int userid = Convert.ToInt32(Session["UserID"]);
            ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
            var q = from d in dc.v_UserMasterSyncLists
                    orderby d.cnt_id descending
                    select d;
            e.QueryableSource = q;
        }

        protected void lnlDownloaderexcel_Click(object sender, EventArgs e)
        {
            String weburl = System.Configuration.ConfigurationSettings.AppSettings["FSMAPIBaseUrl"];
            string apiUrl = weburl+"ShopRegisterPortal/UserSync";
            RegisterShopOutput oview = new RegisterShopOutput();
            int userid = Convert.ToInt32(Session["UserID"]);
            EmployeeSyncInput empDtls = new EmployeeSyncInput();
            DataTable dt = new DataTable();
            List<object> QuoList = GrdEmployee.GetSelectedFieldValues("cnt_id");
            foreach (object Quo in QuoList)
            {
                ProcedureExecute proc = new ProcedureExecute("FTS_EmployeeUserSyncData");
                proc.AddPara("@ACTION", "UserDetails");
                proc.AddPara("@CNT_ID", Quo);
                dt = proc.GetTable();
                if (dt != null && dt.Rows.Count > 0)
                {


                    empDtls.Branch = Convert.ToString(dt.Rows[0]["Branch"]);
                    empDtls.cnt_UCC = Convert.ToString(dt.Rows[0]["cnt_UCC"]);
                    empDtls.Salutation = Convert.ToString(dt.Rows[0]["Salutation"]);
                    empDtls.FirstName = Convert.ToString(dt.Rows[0]["cnt_firstName"]);
                    empDtls.MiddleName = Convert.ToString(dt.Rows[0]["cnt_middleName"]);
                    empDtls.LastName = Convert.ToString(dt.Rows[0]["cnt_lastName"]);
                    empDtls.ContactType = Convert.ToString(dt.Rows[0]["cnt_contactType"]);
                    empDtls.ReferedBy = Convert.ToString(dt.Rows[0]["cnt_referedBy"]);
                    empDtls.DOB = Convert.ToString(dt.Rows[0]["cnt_dOB"]);
                    empDtls.MaritalStatus = Convert.ToString(dt.Rows[0]["cnt_maritalStatus"]);
                    empDtls.AnniversaryDate = Convert.ToString(dt.Rows[0]["cnt_anniversaryDate"]);
                    empDtls.Sex = Convert.ToString(dt.Rows[0]["cnt_sex"]);
                    empDtls.CreateDate = Convert.ToString(dt.Rows[0]["CreateDate"]);
                    empDtls.CreateUser = Convert.ToString(378);
                    empDtls.Bloodgroup = Convert.ToString(dt.Rows[0]["cnt_bloodgroup"]);
                    empDtls.SettlementMode = Convert.ToString(dt.Rows[0]["cnt_SettlementMode"]);
                    empDtls.ContractDeliveryMode = Convert.ToString(dt.Rows[0]["cnt_ContractDeliveryMode"]);
                    empDtls.DirectTMClient = Convert.ToString(dt.Rows[0]["cnt_DirectTMClient"]);
                    empDtls.RelationshipWithDirector = Convert.ToString(dt.Rows[0]["cnt_RelationshipWithDirector"]);
                    empDtls.HasOtherAccount = Convert.ToString(dt.Rows[0]["cnt_HasOtherAccount"]);
                    empDtls.Is_Active = Convert.ToString(dt.Rows[0]["Is_Active"]);
                    empDtls.cnt_IdType = Convert.ToString(dt.Rows[0]["cnt_IdType"]);
                    empDtls.AccountGroupID = Convert.ToString(dt.Rows[0]["AccountGroupID"]);
                    empDtls.DateofJoining = Convert.ToString(dt.Rows[0]["emp_dateofJoining"]);
                    empDtls.Organization = Convert.ToString(dt.Rows[0]["Organization"]);
                    empDtls.JobResponsibility = Convert.ToString(dt.Rows[0]["job_responsibility"]);
                    empDtls.Designation = Convert.ToString(dt.Rows[0]["Designation"]);
                    empDtls.emp_type = Convert.ToString(dt.Rows[0]["emp_type"]);
                    empDtls.Department = Convert.ToString(dt.Rows[0]["Department"]);
                    empDtls.ReportTo = Convert.ToString(dt.Rows[0]["ReportTo"]);
                    empDtls.Deputy = Convert.ToString(dt.Rows[0]["Deputy"]);
                    empDtls.Colleague = Convert.ToString(dt.Rows[0]["Colleague"]);

                    empDtls.workinghours = Convert.ToString(dt.Rows[0]["workinghours"]);
                    empDtls.TotalLeavePA = Convert.ToString(dt.Rows[0]["TotalLeavePA"]);
                    empDtls.LeaveSchemeAppliedFrom = Convert.ToString(dt.Rows[0]["LeaveSchemeAppliedFrom"]);
                    empDtls.username = Convert.ToString(dt.Rows[0]["user_name"]);
                    empDtls.Encryptpass = Convert.ToString(dt.Rows[0]["user_password"]);
                    empDtls.UserLoginId = Convert.ToString(dt.Rows[0]["user_loginId"]);
                    empDtls.usergroup = Convert.ToString(dt.Rows[0]["usergroup"]);
                }

                String Status = "Failed";
                String FailedReason = "";
                string data = JsonConvert.SerializeObject(empDtls);
                HttpClient httpClient = new HttpClient();
                MultipartFormDataContent form = new MultipartFormDataContent();
                form.Add(new StringContent(data), "data");
                var result = httpClient.PostAsync(apiUrl, form).Result;
                oview = JsonConvert.DeserializeObject<RegisterShopOutput>(result.Content.ReadAsStringAsync().Result);

                if (Convert.ToString(oview.status) == "200")
                {
                    Status = "Success";
                }
                else if (Convert.ToString(oview.status) == "202")
                {
                    FailedReason = "Unique Code Duplicate";
                }
                //else if (Convert.ToString(oview.status) == "203")
                //{
                //    FailedReason = "Entity Code not found";
                //}
                //else if (Convert.ToString(oview.status) == "204")
                //{
                //    FailedReason = "Owner Name Not found";
                //}
                else if (Convert.ToString(oview.status) == "205")
                {
                    FailedReason = "Failed";
                }
                //else if (Convert.ToString(oview.status) == "206")
                //{
                //    FailedReason = "Pin Code not found";
                //}
                //else if (Convert.ToString(oview.status) == "207")
                //{
                //    FailedReason = "Owner Contact not found";
                //}
                //else if (Convert.ToString(oview.status) == "208")
                //{
                //    FailedReason = "User or session token not matched";
                //}
                //else if (Convert.ToString(oview.status) == "209")
                //{
                //    FailedReason = "Duplicate shop Id or contact number";
                //}
                else if (Convert.ToString(oview.status) == "204")
                {
                    FailedReason = oview.message;
                }

                ProcedureExecute proc1 = new ProcedureExecute("FTS_EmployeeUserSyncData");
                proc1.AddPara("@ACTION", "SyncLog");
                proc1.AddPara("@InternalID", Convert.ToString(dt.Rows[0]["cnt_internalId"]));
                proc1.AddPara("@SyncBy", userid);
                proc1.AddPara("@Status", Status);
                proc1.AddPara("@FailedReason", FailedReason);
                proc1.AddPara("@CNT_ID", oview.Cnt_id);
                proc1.AddPara("@FSMUser_id", oview.User_id);
                proc1.AddPara("@User_Name", Convert.ToString(dt.Rows[0]["user_name"]));
                proc1.AddPara("@LoginID", Convert.ToString(dt.Rows[0]["user_loginId"]));
                //proc1.AddPara("@AssignedUser", Convert.ToString(dt.Rows[0]["cnt_internalId"]));
                proc1.AddPara("@BranchName", Convert.ToString(dt.Rows[0]["Branch"]));
                proc1.AddPara("@GroupName", Convert.ToString(dt.Rows[0]["usergroup"]));
                int i = proc1.RunActionQuery();
            }

            Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "AfterSync()", true);
        }

        protected void GvJvSearch_DataBinding(object sender, EventArgs e)
        {
            ProcedureExecute proc = new ProcedureExecute("FTS_EmployeeUserSyncData");
            proc.AddPara("@ACTION", "UserSyncLog");
            DataTable dt = proc.GetTable();
            GvJvSearch.DataSource = dt;
        }

        public class EmployeeSyncInput
        {
            public String Branch { get; set; }
            public String cnt_UCC { get; set; }
            public String Salutation { get; set; }
            public String FirstName { get; set; }
            public String MiddleName { get; set; }
            public String LastName { get; set; }
            //public String cnt_contactSource { get; set; }
            public String ContactType { get; set; }
            //public String cnt_legalStatus { get; set; }
            public String ReferedBy { get; set; }
            public String DOB { get; set; }
            public String MaritalStatus { get; set; }
            public String AnniversaryDate { get; set; }
            //public String cnt_education { get; set; }
            //public String cnt_profession { get; set; }
            public String Sex { get; set; }
            public String CreateDate { get; set; }
            public String CreateUser { get; set; }
            public String Bloodgroup { get; set; }
            //public String WebLogIn { get; set; }
            //public String PassWord { get; set; }
            public String SettlementMode { get; set; }
            public String ContractDeliveryMode { get; set; }
            public String DirectTMClient { get; set; }
            public String RelationshipWithDirector { get; set; }
            public String HasOtherAccount { get; set; }
            public String Is_Active { get; set; }
            public String cnt_IdType { get; set; }
            public String AccountGroupID { get; set; }
            public String DateofJoining { get; set; }
            public String Organization { get; set; }
            public String JobResponsibility { get; set; }
            public String Designation { get; set; }
            public String emp_type { get; set; }
            public String Department { get; set; }
            public String ReportTo { get; set; }
            public String Deputy { get; set; }
            public String Colleague { get; set; }
            public String workinghours { get; set; }
            public String TotalLeavePA { get; set; }
            public String LeaveSchemeAppliedFrom { get; set; }

            public String username { get; set; }
            public String Encryptpass { get; set; }
            public String UserLoginId { get; set; }
            public String usergroup { get; set; }
        }

        public class RegisterShopOutput
        {
            public String status { get; set; }
            public String message { get; set; }
            public String Cnt_id { get; set; }
            public String User_id { get; set; }
        }

        protected void GrdEmployee_HtmlDataCellPrepared(object sender, DevExpress.Web.ASPxGridViewTableDataCellEventArgs e)
        {
            if (Convert.ToString(e.CellValue) == "Sync")
            {
                e.Cell.Font.Bold = true;
                e.Cell.BackColor = Color.Green;
            }

            if (Convert.ToString(e.CellValue) == "Not Sync")
            {
                e.Cell.Font.Bold = true;
                e.Cell.BackColor = Color.Red;
            }
        }
    }
}