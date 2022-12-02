using BusinessLogicLayer;
using DevExpress.Web;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ERP.OMS.Management.Activities
{
    public partial class ServiceAssignTechnician : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DataTable dst = new DataTable();
                DBEngine objDB = new DBEngine();
                dst = objDB.GetDataTable("select * from tbl_master_branch where branch_id in (" + Convert.ToString(Session["userbranchhierarchy"]) + ")");
                cmbBranchfilter.TextField = "branch_description";
                cmbBranchfilter.ValueField = "branch_id";
                cmbBranchfilter.DataSource = dst;
                cmbBranchfilter.DataBind();




                FormDate.Date = DateTime.Now;
                toDate.Date = DateTime.Now;

            }
        }
        protected void grid_DataBinding(object sender, EventArgs e)
        {

            string BRANCH_ID = cmbBranchfilter.Value.ToString();
            string Type = hdnType.Value.ToString();




            DataTable dt = new DataTable();
            if (Convert.ToString(Session["GridStatus"]) != "Blank")
            {
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("PRC_ServiceSchedule", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@action", "BindTechAssignList");
                cmd.Parameters.AddWithValue("@BRANCH_ID", BRANCH_ID);
                cmd.Parameters.AddWithValue("@Type", Type);
                cmd.Parameters.AddWithValue("@FromDate", FormDate.Date);
                cmd.Parameters.AddWithValue("@ToDate", toDate.Date);
                cmd.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(dt);
                cmd.Dispose();
                con.Dispose();
            }
            grid.DataSource = dt;

        }

        protected void grid_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            grid.JSProperties["cpMsg"] = null;

            string msg = "";

            if (e.Parameters.Split('~')[0] == "BindGrid")
            {
                Session["GridStatus"] = "NotBlank";
                grid.DataBind();
            }

            else if (e.Parameters.Split('~')[0] == "BlankGrid")
            {
                Session["GridStatus"] = "Blank";
                grid.DataBind();
            }

            else if (e.Parameters.Split('~')[0] == "AssignAll")
            {
                Session["GridStatus"] = "NotBlank";

                foreach (var DETAILS_ID in grid.GetSelectedFieldValues("DETAILS_ID"))
                {
                    DataTable dt = new DataTable();
                    SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                    SqlCommand cmd = new SqlCommand("PRC_ServiceSchedule", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@action", "SaveTechAssignList");
                    cmd.Parameters.AddWithValue("@DETAILS_ID", DETAILS_ID);
                    cmd.Parameters.AddWithValue("@TECH_ID", ddlTechnician.Value);
                    cmd.Parameters.AddWithValue("@SUB_TECHNICIANID", hdnSubTech.Value);
                    cmd.Parameters.AddWithValue("@USER_id", Convert.ToString(Session["userid"]));
                    cmd.CommandTimeout = 0;
                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                    cmd.Dispose();
                    con.Dispose();

                    TechAssign objTechAssign = new TechAssign();
                    objTechAssign.job_code = Convert.ToString(dt.Rows[0]["SCH_CODE"]);
                    objTechAssign.job_id = Convert.ToString(dt.Rows[0]["DETAILS_ID"]);
                    objTechAssign.sub_userid = Convert.ToString(dt.Rows[0]["FSM_SUBUSERID"]);
                    objTechAssign.create_user = Convert.ToString(dt.Rows[0]["assigned_to"]);
                    objTechAssign.jobcreate_date = Convert.ToDateTime(dt.Rows[0]["created_on"]);
                    objTechAssign.service_due_for = Convert.ToString(dt.Rows[0]["QUANTITY"]);
                    objTechAssign.service_for = Convert.ToString(dt.Rows[0]["SERVICE"]);
                    objTechAssign.service_frequency = Convert.ToString(dt.Rows[0]["Frequency"]);
                    objTechAssign.session_token = "sdgdfjhghklhjljfjhgfjhgsg";
                    objTechAssign.shop_code = Convert.ToString(dt.Rows[0]["Shop_Code"]);
                    objTechAssign.status = Convert.ToString(dt.Rows[0]["SCH_STATUS"]);
                    objTechAssign.total_service = Convert.ToString(dt.Rows[0]["QUANTITY"]);
                    objTechAssign.total_service_commited = Convert.ToString(dt.Rows[0]["QUANTITY"]);
                    objTechAssign.total_service_pending = Convert.ToString(dt.Rows[0]["QUANTITY"]);
                    objTechAssign.UOM = Convert.ToString(dt.Rows[0]["UOM"]);
                    objTechAssign.user_id = Convert.ToString(dt.Rows[0]["assigned_to"]);




                    //using (var client = new HttpClient())
                    //{
                    //    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls |
                    //    SecurityProtocolType.Tls11 |
                    //    SecurityProtocolType.Tls12;
                    //    client.DefaultRequestHeaders.Clear();
                    //    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json")); 
                    //   // client.DefaultRequestHeaders.Accept.Add("Content-Type", "application/json; charset=utf8");
                    //    var json = JsonConvert.SerializeObject(objTechAssign, Formatting.Indented);
                    //    var stringContent = new StringContent(json);
                    //    var content = new StringContent(stringContent.ToString(), Encoding.UTF8, "application/json");
                    //    // var response = client.PostAsync(IrnGenerationUrl, stringContent).Result;
                    //    var response = client.PostAsync("http://3.7.30.86:82/API/JobCustomer/AssignJob", stringContent).Result;
                    //}


                    //HttpClient client = new HttpClient();
                    //client.BaseAddress = new Uri("http://3.7.30.86:82/API/");
                    //client.DefaultRequestHeaders
                    //      .Accept
                    //      .Add(new MediaTypeWithQualityHeaderValue("application/json"));//ACCEPT header

                    //HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "JobCustomer/AssignJob");
                    //var json = JsonConvert.SerializeObject(objTechAssign, Formatting.Indented);
                    //request.Content = new StringContent(json,
                    //                                    Encoding.UTF8,
                    //                                    "application/json");//CONTENT-TYPE header

                    //client.SendAsync(request)
                    //      .ContinueWith(responseTask =>
                    //      {
                    //          var a =responseTask.Result;
                    //      });

                    string BaseUrl = ConfigurationManager.AppSettings["BaseUrlService"];

                    string data = JsonConvert.SerializeObject(objTechAssign);
                    WebClient client = new WebClient();
                    client.Headers["Content-type"] = "application/json";
                    client.Encoding = Encoding.UTF8;
                    string json = client.UploadString(BaseUrl + "/API/JobCustomer/AssignJob", data);



                }



                msg = "Technician Assignment done.";

                grid.DataBind();
            }

            else if (e.Parameters.Split('~')[0] == "UnAssignAll")
            {
                Session["GridStatus"] = "NotBlank";
                foreach (var DETAILS_ID in grid.GetSelectedFieldValues("DETAILS_ID"))
                {
                    DataTable dt = new DataTable();
                    SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                    SqlCommand cmd = new SqlCommand("PRC_ServiceSchedule", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@action", "SaveTechUnAssignList");
                    cmd.Parameters.AddWithValue("@DETAILS_ID", DETAILS_ID);
                    cmd.Parameters.AddWithValue("@USER_id", Convert.ToString(Session["userid"]));
                    cmd.CommandTimeout = 0;
                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                    cmd.Dispose();
                    con.Dispose();
                }
                grid.DataBind();
                msg = "Technician Un-Assignment done.";
            }


            grid.JSProperties["cpMsg"] = msg;
        }

        protected void ddlTechnician_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
        {
            DataTable dst = new DataTable();
            DBEngine objDB = new DBEngine();
            dst = objDB.GetDataTable("select CNT.cnt_firstName NAME,ISNULL(user_id,0) ID from tbl_master_contact CNT INNER JOIN tbl_master_contact CNT1 ON CNT.cnt_mainAccount=CNT1.cnt_internalId INNER JOIN TBL_MASTER_USER ON user_contactId=CNT1.cnt_internalId INNER JOIN Srv_master_TechnicianBranch_map ON Tech_InternalId=CNT.cnt_internalId where CNT.cnt_contactType='TM' and  branch_id=" + cmbBranchfilter.Value.ToString());
            ddlTechnician.TextField = "NAME";
            ddlTechnician.ValueField = "ID";
            ddlTechnician.DataSource = dst;
            ddlTechnician.DataBind();

        }

        protected void listBox_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
        {
            ASPxListBox s = sender as ASPxListBox;
            DataTable dst = new DataTable();
            DBEngine objDB = new DBEngine();
            dst = objDB.GetDataTable("select CNT.cnt_firstName NAME,ISNULL(user_id,0) ID from tbl_master_contact CNT INNER JOIN tbl_master_contact CNT1 ON CNT.cnt_mainAccount=CNT1.cnt_internalId INNER JOIN TBL_MASTER_USER ON user_contactId=CNT1.cnt_internalId INNER JOIN Srv_master_TechnicianBranch_map ON Tech_InternalId=CNT.cnt_internalId where CNT.cnt_contactType='TM' and  branch_id=" + cmbBranchfilter.Value.ToString());
            s.TextField = "NAME";
            s.ValueField = "ID";
            s.DataSource = dst;
            s.DataBind();
        }
    }

    public class TechAssign
    {
        public string session_token { get; set; }
        public string job_id { get; set; }
        public string shop_code { get; set; }
        public string user_id { get; set; }
        public string status { get; set; }
        public string service_for { get; set; }
        public string service_due_for { get; set; }
        public string UOM { get; set; }
        public string total_service { get; set; }
        public string service_frequency { get; set; }
        public string total_service_commited { get; set; }
        public string total_service_pending { get; set; }
        public DateTime jobcreate_date { get; set; }
        public string create_user { get; set; }
        public string sub_userid { get; set; }
        public string job_code { get; set; }

    }

    public class TechAssignUpdate
    {
        public string job_id { get; set; }
        public DateTime date { get; set; }
        public string technician_id { get; set; }
        public string subtechnician_id { get; set; }
    } 
}

