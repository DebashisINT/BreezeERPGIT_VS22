using BusinessLogicLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ERP.OMS.Management.Activities
{
    public partial class ServiceAssignBranch : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["EntityID"] != null)
                {
                    hdnCustomerId.Value = Request.QueryString["EntityID"];
                }

                if (Request.QueryString["ID"] != null)
                {
                    hdnServiceContractId.Value = Request.QueryString["ID"];

                }
                if (Request.QueryString["Details_Id"] != null)
                {
                    hdnServiceContractDetailsId.Value = Request.QueryString["Details_Id"];

                }
                if (Request.QueryString["Schedule_id"] != null)
                {
                    hdnScheduleid.Value = Request.QueryString["Schedule_id"];

                }

                DataTable dst = new DataTable();
                DBEngine objDB = new DBEngine();
                dst = objDB.GetDataTable("select * from tbl_master_branch");


                ddlBranch.TextField = "branch_description";
                ddlBranch.ValueField = "branch_id";
                ddlBranch.DataSource = dst;
                ddlBranch.DataBind();

            }
        }

        protected void grid_DataBinding(object sender, EventArgs e)
        {
            
            string customer = hdnCustomerId.Value;
            string segment1 = hdnSegment1.Value;
            string segment2 = hdnSegment2.Value;
            string segment3 = hdnSegment3.Value;
            string segment4 = hdnSegment4.Value;
            string segment5 = hdnSegment5.Value;
            string Schedule_id = hdnScheduleid.Value;
            string Type = ddlStatus.Value.ToString();


            

            DataTable dt = new DataTable();
            if (Convert.ToString(Session["GridStatus"]) != "Blank")
            {
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("PRC_ServiceSchedule", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@action", "BindAssignList");
                cmd.Parameters.AddWithValue("@Customer_id", customer);
                //cmd.Parameters.AddWithValue("@ORDER_ID", order_id);
                //cmd.Parameters.AddWithValue("@ORDERDETAILS_ID", orderdetails_id.Split('~')[0]);
                //cmd.Parameters.AddWithValue("@ORDER_NUMBER", order_no);
                //cmd.Parameters.AddWithValue("@Frequency", Frequency);
                cmd.Parameters.AddWithValue("@Segment_Id1", segment1);
                cmd.Parameters.AddWithValue("@Segment_Id2", segment2);
                cmd.Parameters.AddWithValue("@Segment_Id3", segment3);
                cmd.Parameters.AddWithValue("@Segment_Id4", segment4);
                cmd.Parameters.AddWithValue("@Segment_Id5", segment5);
                cmd.Parameters.AddWithValue("@SCHEDULE_ID", Schedule_id);
                cmd.Parameters.AddWithValue("@Type", Type);

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
                
            if(e.Parameters.Split('~')[0]=="BindGrid")
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
                    cmd.Parameters.AddWithValue("@action", "SaveAssignList");
                    cmd.Parameters.AddWithValue("@DETAILS_ID", DETAILS_ID);
                    cmd.Parameters.AddWithValue("@BRANCH_ID", ddlBranch.Value);
                    cmd.Parameters.AddWithValue("@USER_id", Convert.ToString(Session["userid"]));
                    cmd.CommandTimeout = 0;
                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                    cmd.Dispose();
                    con.Dispose();
                }



                msg = "Branch Assignment done.";

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
                    cmd.Parameters.AddWithValue("@action", "SaveUnAssignList");
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
                msg = "Branch Un-Assignment done.";
            }


            grid.JSProperties["cpMsg"] = msg;
        }
    }
}