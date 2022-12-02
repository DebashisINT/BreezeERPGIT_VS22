using BreezeERPAPI.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BreezeERPAPI.Controllers
{
    public class AssignScheduleStatusController : Controller
    {
        [AcceptVerbs("POST")]
        public JsonResult WorkInProgressSubmit(WorkInProgressInput model)
        {
            AssignJobOutPut omodel = new AssignJobOutPut();

            DataTable dt = new DataTable();
            String con = System.Configuration.ConfigurationSettings.AppSettings["DBConnectionDefault"];
            SqlCommand sqlcmd = new SqlCommand();
            SqlConnection sqlcon = new SqlConnection(con);
            sqlcon.Open();
            sqlcmd = new SqlCommand("PRC_AssignScheduleStatusInsertUpdate", sqlcon);
            sqlcmd.Parameters.Add("@Action", "WorkInProgress");
            sqlcmd.Parameters.Add("@user_id", model.user_id);
            sqlcmd.Parameters.Add("@SCH_ID", model.job_id);
            sqlcmd.Parameters.Add("@date", model.start_date);
            sqlcmd.Parameters.Add("@time", model.start_time);
            sqlcmd.Parameters.Add("@service_due", model.service_due);
            sqlcmd.Parameters.Add("@service_completed", model.service_completed);
            sqlcmd.Parameters.Add("@next_date", model.next_date);
            sqlcmd.Parameters.Add("@next_time", model.next_time);
            sqlcmd.Parameters.Add("@remarks", model.remarks);
            sqlcmd.Parameters.Add("@date_time", model.date_time);
            sqlcmd.Parameters.Add("@latitude", model.latitude);
            sqlcmd.Parameters.Add("@longitude", model.longitude);
            sqlcmd.Parameters.Add("@address", model.address);
            sqlcmd.Parameters.Add("@fsm_id", model.fsm_id);
            sqlcmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
            da.Fill(dt);
            sqlcon.Close();
            if (dt.Rows.Count > 0)
            {
                omodel.status = "200";
                omodel.message = "Successfully submit work in progress.";
                omodel.id = dt.Rows[0]["ID"].ToString();
            }
            else
            {
                omodel.status = "205";
                omodel.message = "Failed.";
            }
            var message = Json(omodel);
            return message;
        }

        [AcceptVerbs("POST")]
        public JsonResult WorkOnHoldSubmit(WorkOnHoldInput model)
        {
            AssignJobOutPut omodel = new AssignJobOutPut();

            DataTable dt = new DataTable();
            String con = System.Configuration.ConfigurationSettings.AppSettings["DBConnectionDefault"];
            SqlCommand sqlcmd = new SqlCommand();
            SqlConnection sqlcon = new SqlConnection(con);
            sqlcon.Open();
            sqlcmd = new SqlCommand("PRC_AssignScheduleStatusInsertUpdate", sqlcon);
            sqlcmd.Parameters.Add("@Action", "WorkOnHold");
            sqlcmd.Parameters.Add("@user_id", model.user_id);
            sqlcmd.Parameters.Add("@SCH_ID", model.job_id);
            sqlcmd.Parameters.Add("@date", model.hold_date);
            sqlcmd.Parameters.Add("@time", model.hold_time);
            sqlcmd.Parameters.Add("@reason_hold", model.reason_hold);
            sqlcmd.Parameters.Add("@remarks", model.remarks);
            sqlcmd.Parameters.Add("@date_time", model.date_time);
            sqlcmd.Parameters.Add("@latitude", model.latitude);
            sqlcmd.Parameters.Add("@longitude", model.longitude);
            sqlcmd.Parameters.Add("@address", model.address);
            sqlcmd.Parameters.Add("@fsm_id", model.fsm_id);
            sqlcmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
            da.Fill(dt);
            sqlcon.Close();
            if (dt.Rows.Count > 0)
            {
                omodel.status = "200";
                omodel.message = "Successfully submit work on hold.";
                omodel.id = dt.Rows[0]["ID"].ToString();
            }
            else
            {
                omodel.status = "205";
                omodel.message = "Failed.";
            }
            var message = Json(omodel);
            return message;
        }

        [AcceptVerbs("POST")]
        public JsonResult WorkOnCompletedSubmit(WorkOnCompletedInput model)
        {
            AssignJobOutPut omodel = new AssignJobOutPut();

            DataTable dt = new DataTable();
            String con = System.Configuration.ConfigurationSettings.AppSettings["DBConnectionDefault"];
            SqlCommand sqlcmd = new SqlCommand();
            SqlConnection sqlcon = new SqlConnection(con);
            sqlcon.Open();
            sqlcmd = new SqlCommand("PRC_AssignScheduleStatusInsertUpdate", sqlcon);
            sqlcmd.Parameters.Add("@Action", "WorkOnCompleted");
            sqlcmd.Parameters.Add("@user_id", model.user_id);
            sqlcmd.Parameters.Add("@SCH_ID", model.job_id);
            sqlcmd.Parameters.Add("@date", model.finish_date);
            sqlcmd.Parameters.Add("@time", model.finish_time);
            sqlcmd.Parameters.Add("@remarks", model.remarks);
            sqlcmd.Parameters.Add("@date_time", model.date_time);
            sqlcmd.Parameters.Add("@latitude", model.latitude);
            sqlcmd.Parameters.Add("@longitude", model.longitude);
            sqlcmd.Parameters.Add("@address", model.address);
            sqlcmd.Parameters.Add("@phone_no", model.phone_no);
            sqlcmd.Parameters.Add("@fsm_id", model.fsm_id);
            sqlcmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
            da.Fill(dt);
            sqlcon.Close();
            if (dt.Rows.Count > 0)
            {
                omodel.status = "200";
                omodel.message = "Successfully submit work completed.";
                omodel.id = dt.Rows[0]["ID"].ToString();
            }
            else
            {
                omodel.status = "205";
                omodel.message = "Failed.";
            }
            var message = Json(omodel);
            return message;
        }

        [AcceptVerbs("POST")]
        public JsonResult WorkCancelledSubmit(WorkCancelledInput model)
        {
            AssignJobOutPut omodel = new AssignJobOutPut();

            DataTable dt = new DataTable();
            String con = System.Configuration.ConfigurationSettings.AppSettings["DBConnectionDefault"];
            SqlCommand sqlcmd = new SqlCommand();
            SqlConnection sqlcon = new SqlConnection(con);
            sqlcon.Open();
            sqlcmd = new SqlCommand("PRC_AssignScheduleStatusInsertUpdate", sqlcon);
            sqlcmd.Parameters.Add("@Action", "WorkCancelled");
            sqlcmd.Parameters.Add("@user_id", model.user_id);
            sqlcmd.Parameters.Add("@SCH_ID", model.job_id);
            sqlcmd.Parameters.Add("@date", model.date);
            sqlcmd.Parameters.Add("@time", model.time);
            sqlcmd.Parameters.Add("@cancel_reason", model.cancel_reason);
            sqlcmd.Parameters.Add("@remarks", model.remarks);
            sqlcmd.Parameters.Add("@date_time", model.date_time);
            sqlcmd.Parameters.Add("@latitude", model.latitude);
            sqlcmd.Parameters.Add("@longitude", model.longitude);
            sqlcmd.Parameters.Add("@address", model.address);
            sqlcmd.Parameters.Add("@CANCELLED_USER", model.user);
            sqlcmd.Parameters.Add("@CANCELLED_BY", model.cancelled_by);
            sqlcmd.Parameters.Add("@fsm_id", model.fsm_id);
            sqlcmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
            da.Fill(dt);
            sqlcon.Close();
            if (dt.Rows.Count > 0)
            {
                omodel.status = "200";
                omodel.message = "Successfully submit work cancelled.";
                omodel.id = dt.Rows[0]["ID"].ToString();
            }
            else
            {
                omodel.status = "205";
                omodel.message = "Failed.";
            }
            var message = Json(omodel);
            return message;
        }

        [AcceptVerbs("POST")]
        public JsonResult UpdateReview(ReviewInput model)
        {
            AssignJobOutPut omodel = new AssignJobOutPut();

            DataTable dt = new DataTable();
            String con = System.Configuration.ConfigurationSettings.AppSettings["DBConnectionDefault"];
            SqlCommand sqlcmd = new SqlCommand();
            SqlConnection sqlcon = new SqlConnection(con);
            sqlcon.Open();
            sqlcmd = new SqlCommand("PRC_AssignScheduleStatusInsertUpdate", sqlcon);
            sqlcmd.Parameters.Add("@Action", "UpdateReview");
            sqlcmd.Parameters.Add("@user_id", model.user_id);
            sqlcmd.Parameters.Add("@SCH_ID", model.job_id);
            sqlcmd.Parameters.Add("@review", model.review);
            sqlcmd.Parameters.Add("@rate", model.rate);
            sqlcmd.Parameters.Add("@date_time", model.date_time);
            sqlcmd.Parameters.Add("@latitude", model.latitude);
            sqlcmd.Parameters.Add("@longitude", model.longitude);
            sqlcmd.Parameters.Add("@address", model.address);
            sqlcmd.Parameters.Add("@fsm_id", model.fsm_id);
            sqlcmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
            da.Fill(dt);
            sqlcon.Close();
            if (dt.Rows.Count > 0)
            {
                omodel.status = "200";
                omodel.message = "Successfully update review.";
                omodel.id = dt.Rows[0]["ID"].ToString();
            }
            else
            {
                omodel.status = "205";
                omodel.message = "Failed.";
            }
            var message = Json(omodel);
            return message;
        }

        [AcceptVerbs("POST")]
        public JsonResult SubmitWorkUnhold(WorkUnholdInput model)
        {
            AssignJobOutPut omodel = new AssignJobOutPut();

            DataTable dt = new DataTable();
            String con = System.Configuration.ConfigurationSettings.AppSettings["DBConnectionDefault"];
            SqlCommand sqlcmd = new SqlCommand();
            SqlConnection sqlcon = new SqlConnection(con);
            sqlcon.Open();
            sqlcmd = new SqlCommand("PRC_AssignScheduleStatusInsertUpdate", sqlcon);
            sqlcmd.Parameters.Add("@Action", "WorkUnHold");
            sqlcmd.Parameters.Add("@user_id", model.user_id);
            sqlcmd.Parameters.Add("@SCH_ID", model.job_id);
            sqlcmd.Parameters.Add("@date", model.unhold_date);
            sqlcmd.Parameters.Add("@time", model.unhold_time);
            sqlcmd.Parameters.Add("@reason_hold", model.reason_unhold);
            sqlcmd.Parameters.Add("@remarks", model.remarks);
            sqlcmd.Parameters.Add("@date_time", model.date_time);
            sqlcmd.Parameters.Add("@latitude", model.latitude);
            sqlcmd.Parameters.Add("@longitude", model.longitude);
            sqlcmd.Parameters.Add("@address", model.address);
            sqlcmd.Parameters.Add("@fsm_id", model.fsm_id);
            sqlcmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
            da.Fill(dt);
            sqlcon.Close();
            if (dt.Rows.Count > 0)
            {
                omodel.status = "200";
                omodel.message = "Successfully submit work unhold.";
                omodel.id = dt.Rows[0]["ID"].ToString();
            }
            else
            {
                omodel.status = "205";
                omodel.message = "Failed.";
            }
            var message = Json(omodel);
            return message;
        }

        [AcceptVerbs("POST")]
        public JsonResult SubmitWorkReschedule(SubmitWorkRescheduleInput model)
        {
            AssignJobOutPut omodel = new AssignJobOutPut();

            DataTable dt = new DataTable();
            String con = System.Configuration.ConfigurationSettings.AppSettings["DBConnectionDefault"];
            SqlCommand sqlcmd = new SqlCommand();
            SqlConnection sqlcon = new SqlConnection(con);
            sqlcon.Open();
            sqlcmd = new SqlCommand("PRC_AssignScheduleStatusInsertUpdate", sqlcon);
            sqlcmd.Parameters.Add("@Action", "WorkReschedule");
            sqlcmd.Parameters.Add("@user_id", model.user_id);
            sqlcmd.Parameters.Add("@SCH_ID", model.job_id);
            sqlcmd.Parameters.Add("@date", model.reschedule_date);
            sqlcmd.Parameters.Add("@time", model.reschedule_time);
            sqlcmd.Parameters.Add("@cancel_reason", model.resc_reason);
            sqlcmd.Parameters.Add("@remarks", model.remarks);
            sqlcmd.Parameters.Add("@date_time", model.date_time);
            sqlcmd.Parameters.Add("@latitude", model.latitude);
            sqlcmd.Parameters.Add("@longitude", model.longitude);
            sqlcmd.Parameters.Add("@address", model.address);
            sqlcmd.Parameters.Add("@RESCHEDULE_USER", model.reschedule_by);
            sqlcmd.Parameters.Add("@RESCHEDULE_BY", model.user);
            sqlcmd.Parameters.Add("@fsm_id", model.fsm_id);
            sqlcmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
            da.Fill(dt);
            sqlcon.Close();
            if (dt.Rows.Count > 0)
            {
                omodel.status = "200";
                omodel.message = "Successfully submit work cancelled.";
                omodel.id = dt.Rows[0]["ID"].ToString();
            }
            else
            {
                omodel.status = "205";
                omodel.message = "Failed.";
            }
            var message = Json(omodel);
            return message;
        }

        public JsonResult WorkInProgressSubmitMultipart(CustomerJobStatusAttachment model)
        {
            AssignJobOutPut omodel = new AssignJobOutPut();
            string attachmentName = "";
            string Image = "";
            //List<CustomerJobStatusImagees> omedl2 = new List<CustomerJobStatusImagees>();
            DataTable Jsondt = new DataTable();
            Jsondt.Columns.Add("attachment", typeof(String));
            try
            {
                var details = JObject.Parse(model.data);
                var hhhh = Newtonsoft.Json.JsonConvert.DeserializeObject<WorkInProgressInput>(model.data);
                if (!string.IsNullOrEmpty(model.data))
                {
                    for (int i = 0; i < model.attachments.Count; i++)
                    {
                        attachmentName = model.attachments[i].FileName;
                        attachmentName = hhhh.session_token + '_' + hhhh.user_id + '_' + attachmentName;
                        string vPath = Path.Combine(Server.MapPath("~/CommonFolder/ScheduleAttachment"), attachmentName);
                        model.attachments[i].SaveAs(vPath);

                        //omedl2.Add(new CustomerJobStatusImagees()
                        //{
                        //    attachment = attachmentName,
                        //});
                        Jsondt.Rows.Add(new Object[] { attachmentName });  
                    }
                }

                //string JsonXML = XmlConversion.ConvertToXml(omedl2, 0);

                string sessionId = "";

                DataTable dt = new DataTable();
                String con = System.Configuration.ConfigurationSettings.AppSettings["DBConnectionDefault"];
                SqlCommand sqlcmd = new SqlCommand();
                SqlConnection sqlcon = new SqlConnection(con);
                sqlcon.Open();
                sqlcmd = new SqlCommand("PRC_AssignScheduleStatusInsertUpdate", sqlcon);
                sqlcmd.Parameters.Add("@Action", "WorkInProgress");
                sqlcmd.Parameters.Add("@user_id", hhhh.user_id);
                sqlcmd.Parameters.Add("@SCH_ID", hhhh.job_id);
                sqlcmd.Parameters.Add("@date", hhhh.start_date);
                sqlcmd.Parameters.Add("@time", hhhh.start_time);
                sqlcmd.Parameters.Add("@service_due", hhhh.service_due);
                sqlcmd.Parameters.Add("@service_completed", hhhh.service_completed);
                sqlcmd.Parameters.Add("@next_date", hhhh.next_date);
                sqlcmd.Parameters.Add("@next_time", hhhh.next_time);
                sqlcmd.Parameters.Add("@remarks", hhhh.remarks);
                sqlcmd.Parameters.Add("@date_time", hhhh.date_time);
                sqlcmd.Parameters.Add("@latitude", hhhh.latitude);
                sqlcmd.Parameters.Add("@longitude", hhhh.longitude);
                sqlcmd.Parameters.Add("@address", hhhh.address);
                sqlcmd.Parameters.Add("@ATTACHMENT", Jsondt);
                sqlcmd.Parameters.Add("@fsm_id", hhhh.fsm_id);
                sqlcmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                da.Fill(dt);
                sqlcon.Close();
                if (dt.Rows.Count > 0)
                {
                    omodel.status = "200";
                    omodel.message = "Successfully submit work in progress.";
                    omodel.id = dt.Rows[0]["ID"].ToString();
                }
                else
                {
                    omodel.status = "205";
                    omodel.message = "Failed.";
                }
            }
            catch (Exception msg)
            {
                omodel.status = "204" + attachmentName;
                omodel.message = msg.Message;
            }
            return Json(omodel);
        }

        public JsonResult WorkOnHoldSubmitMultipart(CustomerJobStatusAttachment model)
        {
            AssignJobOutPut omodel = new AssignJobOutPut();
            string attachmentName = "";
            string Image = "";
            //List<CustomerJobStatusImagees> omedl2 = new List<CustomerJobStatusImagees>();
            DataTable Jsondt = new DataTable();
            Jsondt.Columns.Add("attachment", typeof(String));
            try
            {
                var details = JObject.Parse(model.data);
                var hhhh = Newtonsoft.Json.JsonConvert.DeserializeObject<WorkOnHoldInput>(model.data);
                if (!string.IsNullOrEmpty(model.data))
                {
                    for (int i = 0; i < model.attachments.Count; i++)
                    {
                        attachmentName = model.attachments[i].FileName;
                        attachmentName = hhhh.session_token + '_' + hhhh.user_id + '_' + attachmentName;
                        string vPath = Path.Combine(Server.MapPath("~/CommonFolder/ScheduleAttachment"), attachmentName);
                        model.attachments[i].SaveAs(vPath);

                        //omedl2.Add(new CustomerJobStatusImagees()
                        //{
                        //    attachment = attachmentName,
                        //});
                        Jsondt.Rows.Add(new Object[] { attachmentName });  
                    }
                }
                //string JsonXML = XmlConversion.ConvertToXml(omedl2, 0);

                string sessionId = "";

                DataTable dt = new DataTable();
                String con = System.Configuration.ConfigurationSettings.AppSettings["DBConnectionDefault"];
                SqlCommand sqlcmd = new SqlCommand();
                SqlConnection sqlcon = new SqlConnection(con);
                sqlcon.Open();
                sqlcmd = new SqlCommand("PRC_AssignScheduleStatusInsertUpdate", sqlcon);
                sqlcmd.Parameters.Add("@Action", "WorkOnHold");
                sqlcmd.Parameters.Add("@user_id", hhhh.user_id);
                sqlcmd.Parameters.Add("@SCH_ID", hhhh.job_id);
                sqlcmd.Parameters.Add("@date", hhhh.hold_date);
                sqlcmd.Parameters.Add("@time", hhhh.hold_time);
                sqlcmd.Parameters.Add("@reason_hold", hhhh.reason_hold);
                sqlcmd.Parameters.Add("@remarks", hhhh.remarks);
                sqlcmd.Parameters.Add("@date_time", hhhh.date_time);
                sqlcmd.Parameters.Add("@latitude", hhhh.latitude);
                sqlcmd.Parameters.Add("@longitude", hhhh.longitude);
                sqlcmd.Parameters.Add("@address", hhhh.address);
                sqlcmd.Parameters.Add("@ATTACHMENT", Jsondt);
                sqlcmd.Parameters.Add("@fsm_id", hhhh.fsm_id);
                sqlcmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                da.Fill(dt);
                sqlcon.Close();
                if (dt.Rows.Count > 0)
                {
                    omodel.status = "200";
                    omodel.message = "Successfully submit work on hold.";
                    omodel.id = dt.Rows[0]["ID"].ToString();
                }
                else
                {
                    omodel.status = "205";
                    omodel.message = "Failed.";
                }
            }
            catch (Exception msg)
            {
                omodel.status = "204" + attachmentName;
                omodel.message = msg.Message;
            }
            return Json(omodel);
        }

        public JsonResult WorkOnCompletedSubmitMultipart(CustomerJobStatusAttachment model)
        {
            AssignJobOutPut omodel = new AssignJobOutPut();
            string attachmentName = "";
            string Image = "";
            //List<CustomerJobStatusImagees> omedl2 = new List<CustomerJobStatusImagees>();
            DataTable Jsondt = new DataTable();
            Jsondt.Columns.Add("attachment", typeof(String));
            try
            {
                var details = JObject.Parse(model.data);
                var hhhh = Newtonsoft.Json.JsonConvert.DeserializeObject<WorkOnCompletedInput>(model.data);
                if (!string.IsNullOrEmpty(model.data))
                {
                    for (int i = 0; i < model.attachments.Count; i++)
                    {
                        attachmentName = model.attachments[i].FileName;
                        attachmentName = hhhh.session_token + '_' + hhhh.user_id + '_' + attachmentName;
                        string vPath = Path.Combine(Server.MapPath("~/CommonFolder/ScheduleAttachment"), attachmentName);
                        model.attachments[i].SaveAs(vPath);

                        //omedl2.Add(new CustomerJobStatusImagees()
                        //{
                        //    attachment = attachmentName,
                        //});
                        Jsondt.Rows.Add(new Object[] { attachmentName });  
                    }
                }
                //string JsonXML = XmlConversion.ConvertToXml(omedl2, 0);

                string sessionId = "";

                DataTable dt = new DataTable();
                String con = System.Configuration.ConfigurationSettings.AppSettings["DBConnectionDefault"];
                SqlCommand sqlcmd = new SqlCommand();
                SqlConnection sqlcon = new SqlConnection(con);
                sqlcon.Open();
                sqlcmd = new SqlCommand("PRC_AssignScheduleStatusInsertUpdate", sqlcon);
                sqlcmd.Parameters.Add("@Action", "WorkOnCompleted");
                sqlcmd.Parameters.Add("@user_id", hhhh.user_id);
                sqlcmd.Parameters.Add("@SCH_ID", hhhh.job_id);
                sqlcmd.Parameters.Add("@date", hhhh.finish_date);
                sqlcmd.Parameters.Add("@time", hhhh.finish_time);
                sqlcmd.Parameters.Add("@remarks", hhhh.remarks);
                sqlcmd.Parameters.Add("@date_time", hhhh.date_time);
                sqlcmd.Parameters.Add("@latitude", hhhh.latitude);
                sqlcmd.Parameters.Add("@longitude", hhhh.longitude);
                sqlcmd.Parameters.Add("@address", hhhh.address);
                sqlcmd.Parameters.Add("@ATTACHMENT", Jsondt);
                sqlcmd.Parameters.Add("@phone_no", hhhh.phone_no);
                sqlcmd.Parameters.Add("@fsm_id", hhhh.fsm_id);
                sqlcmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                da.Fill(dt);
                sqlcon.Close();
                if (dt.Rows.Count > 0)
                {
                    omodel.status = "200";
                    omodel.message = "Successfully submit work completed.";
                    omodel.id = dt.Rows[0]["ID"].ToString();
                }
                else
                {
                    omodel.status = "205";
                    omodel.message = "Failed.";
                }
            }
            catch (Exception msg)
            {
                omodel.status = "204" + attachmentName;
                omodel.message = msg.Message;
            }
            return Json(omodel);
        }

        public JsonResult WorkCancelledSubmitMultipart(CustomerJobStatusAttachment model)
        {
            AssignJobOutPut omodel = new AssignJobOutPut();
            string attachmentName = "";
            string Image = "";
            //List<CustomerJobStatusImagees> omedl2 = new List<CustomerJobStatusImagees>();
            DataTable Jsondt = new DataTable();
            Jsondt.Columns.Add("attachment", typeof(String));
            try
            {
                var details = JObject.Parse(model.data);
                var hhhh = Newtonsoft.Json.JsonConvert.DeserializeObject<WorkCancelledInput>(model.data);
              
                if (!string.IsNullOrEmpty(model.data))
                {
                    for (int i = 0; i < model.attachments.Count; i++)
                    {
                        attachmentName = model.attachments[i].FileName;
                        attachmentName = hhhh.session_token + '_' + hhhh.user_id + '_' + attachmentName;
                        string vPath = Path.Combine(Server.MapPath("~/CommonFolder/ScheduleAttachment"), attachmentName);
                        model.attachments[i].SaveAs(vPath);

                        //omedl2.Add(new CustomerJobStatusImagees()
                        //{
                        //    attachment = attachmentName,
                        //});
                        Jsondt.Rows.Add(new Object[] { attachmentName });  
                    }
                }
                //string JsonXML = XmlConversion.ConvertToXml(omedl2, 0);

                string sessionId = "";

                DataTable dt = new DataTable();
                String con = System.Configuration.ConfigurationSettings.AppSettings["DBConnectionDefault"];
                SqlCommand sqlcmd = new SqlCommand();
                SqlConnection sqlcon = new SqlConnection(con);
                sqlcon.Open();
                sqlcmd = new SqlCommand("PRC_AssignScheduleStatusInsertUpdate", sqlcon);
                sqlcmd.Parameters.Add("@Action", "WorkCancelled");
                sqlcmd.Parameters.Add("@user_id", hhhh.user_id);
                sqlcmd.Parameters.Add("@SCH_ID", hhhh.job_id);
                sqlcmd.Parameters.Add("@date", hhhh.date);
                sqlcmd.Parameters.Add("@time", hhhh.time);
                sqlcmd.Parameters.Add("@cancel_reason", hhhh.cancel_reason);
                sqlcmd.Parameters.Add("@remarks", hhhh.remarks);
                sqlcmd.Parameters.Add("@date_time", hhhh.date_time);
                sqlcmd.Parameters.Add("@latitude", hhhh.latitude);
                sqlcmd.Parameters.Add("@longitude", hhhh.longitude);
                sqlcmd.Parameters.Add("@address", hhhh.address);
                sqlcmd.Parameters.Add("@ATTACHMENT", Jsondt);
                sqlcmd.Parameters.Add("@CANCELLED_USER", hhhh.user);
                sqlcmd.Parameters.Add("@CANCELLED_BY", hhhh.cancelled_by);
                sqlcmd.Parameters.Add("@fsm_id", hhhh.fsm_id);
                sqlcmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                da.Fill(dt);
                sqlcon.Close();
                if (dt.Rows.Count > 0)
                {
                    omodel.status = "200";
                    omodel.message = "Successfully submit work cancelled.";
                    omodel.id = dt.Rows[0]["ID"].ToString();
                }
                else
                {
                    omodel.status = "205";
                    omodel.message = "Failed.";
                }
            }
            catch (Exception msg)
            {
                omodel.status = "204" + attachmentName;
                omodel.message = msg.Message;
            }
            return Json(omodel);
        }

        public JsonResult UpdateReviewMultipart(CustomerJobStatusAttachment model)
        {
            AssignJobOutPut omodel = new AssignJobOutPut();
            string attachmentName = "";
            string Image = "";
            //List<CustomerJobStatusImagees> omedl2 = new List<CustomerJobStatusImagees>();
            DataTable Jsondt = new DataTable();
            Jsondt.Columns.Add("attachment", typeof(String));
            try
            {
                var details = JObject.Parse(model.data);
                var hhhh = Newtonsoft.Json.JsonConvert.DeserializeObject<ReviewInput>(model.data);
              
                if (!string.IsNullOrEmpty(model.data))
                {
                    for (int i = 0; i < model.attachments.Count; i++)
                    {
                        attachmentName = model.attachments[i].FileName;
                        attachmentName = hhhh.session_token + '_' + hhhh.user_id + '_' + attachmentName;
                        string vPath = Path.Combine(Server.MapPath("~/CommonFolder/ScheduleAttachment"), attachmentName);
                        model.attachments[i].SaveAs(vPath);

                        //omedl2.Add(new CustomerJobStatusImagees()
                        //{
                        //    attachment = attachmentName,
                        //});
                        Jsondt.Rows.Add(new Object[] { attachmentName });  
                    }
                }
                //string JsonXML = XmlConversion.ConvertToXml(omedl2, 0);

                string sessionId = "";

                DataTable dt = new DataTable();
                String con = System.Configuration.ConfigurationSettings.AppSettings["DBConnectionDefault"];
                SqlCommand sqlcmd = new SqlCommand();
                SqlConnection sqlcon = new SqlConnection(con);
                sqlcon.Open();
                sqlcmd = new SqlCommand("PRC_AssignScheduleStatusInsertUpdate", sqlcon);
                sqlcmd.Parameters.Add("@Action", "UpdateReview");
                sqlcmd.Parameters.Add("@user_id", hhhh.user_id);
                sqlcmd.Parameters.Add("@SCH_ID", hhhh.job_id);
                sqlcmd.Parameters.Add("@review", hhhh.review);
                sqlcmd.Parameters.Add("@rate", hhhh.rate);
                sqlcmd.Parameters.Add("@date_time", hhhh.date_time);
                sqlcmd.Parameters.Add("@latitude", hhhh.latitude);
                sqlcmd.Parameters.Add("@longitude", hhhh.longitude);
                sqlcmd.Parameters.Add("@address", hhhh.address);
                sqlcmd.Parameters.Add("@ATTACHMENT", Jsondt);
                sqlcmd.Parameters.Add("@fsm_id", hhhh.fsm_id);
                sqlcmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                da.Fill(dt);
                sqlcon.Close();
                if (dt.Rows.Count > 0)
                {
                    omodel.status = "200";
                    omodel.message = "Successfully update review.";
                    omodel.id = dt.Rows[0]["ID"].ToString();
                }
                else
                {
                    omodel.status = "205";
                    omodel.message = "Failed.";
                }
            }
            catch (Exception msg)
            {
                omodel.status = "204" + attachmentName;
                omodel.message = msg.Message;
            }
            return Json(omodel);
        }

        public JsonResult SubmitWorkUnholdMultipart(CustomerJobStatusAttachment model)
        {
            AssignJobOutPut omodel = new AssignJobOutPut();
            string attachmentName = "";
            string Image = "";
            //List<CustomerJobStatusImagees> omedl2 = new List<CustomerJobStatusImagees>();
            DataTable Jsondt = new DataTable();
            Jsondt.Columns.Add("attachment", typeof(String));
            try
            {
                var details = JObject.Parse(model.data);
                var hhhh = Newtonsoft.Json.JsonConvert.DeserializeObject<WorkUnholdInput>(model.data);
                if (!string.IsNullOrEmpty(model.data))
                {
                    for (int i = 0; i < model.attachments.Count; i++)
                    {
                        attachmentName = model.attachments[i].FileName;
                        attachmentName = hhhh.session_token + '_' + hhhh.user_id + '_' + attachmentName;
                        string vPath = Path.Combine(Server.MapPath("~/CommonFolder/ScheduleAttachment"), attachmentName);
                        model.attachments[i].SaveAs(vPath);

                        //omedl2.Add(new CustomerJobStatusImagees()
                        //{
                        //    attachment = attachmentName,
                        //});
                        Jsondt.Rows.Add(new Object[] { attachmentName });
                    }
                }
                //string JsonXML = XmlConversion.ConvertToXml(omedl2, 0);

                string sessionId = "";

                DataTable dt = new DataTable();
                String con = System.Configuration.ConfigurationSettings.AppSettings["DBConnectionDefault"];
                SqlCommand sqlcmd = new SqlCommand();
                SqlConnection sqlcon = new SqlConnection(con);
                sqlcon.Open();
                sqlcmd = new SqlCommand("PRC_AssignScheduleStatusInsertUpdate", sqlcon);
                sqlcmd.Parameters.Add("@Action", "WorkUnHold");
                //sqlcmd.Parameters.Add("@Action", "WorkUnHold");
                sqlcmd.Parameters.Add("@user_id", hhhh.user_id);
                sqlcmd.Parameters.Add("@SCH_ID", hhhh.job_id);
                sqlcmd.Parameters.Add("@date", hhhh.unhold_date);
                sqlcmd.Parameters.Add("@time", hhhh.unhold_time);
                sqlcmd.Parameters.Add("@reason_hold", hhhh.reason_unhold);
                sqlcmd.Parameters.Add("@remarks", hhhh.remarks);
                sqlcmd.Parameters.Add("@date_time", hhhh.date_time);
                sqlcmd.Parameters.Add("@latitude", hhhh.latitude);
                sqlcmd.Parameters.Add("@longitude", hhhh.longitude);
                sqlcmd.Parameters.Add("@address", hhhh.address);
                sqlcmd.Parameters.Add("@ATTACHMENT", Jsondt);
                sqlcmd.Parameters.Add("@fsm_id", hhhh.fsm_id);
                sqlcmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                da.Fill(dt);
                sqlcon.Close();
                if (dt.Rows.Count > 0)
                {
                    omodel.status = "200";
                    omodel.message = "Successfully submit work unhold.";
                    omodel.id = dt.Rows[0]["ID"].ToString();
                }
                else
                {
                    omodel.status = "205";
                    omodel.message = "Failed.";
                }
            }
            catch (Exception msg)
            {
                omodel.status = "204" + attachmentName;
                omodel.message = msg.Message;
            }
            return Json(omodel);
        }

        public JsonResult SubmitWorkRescheduleMultipart(CustomerJobStatusAttachment model)
        {
            AssignJobOutPut omodel = new AssignJobOutPut();
            string attachmentName = "";
            string Image = "";
            //List<CustomerJobStatusImagees> omedl2 = new List<CustomerJobStatusImagees>();
            DataTable Jsondt = new DataTable();
            Jsondt.Columns.Add("attachment", typeof(String));
            try
            {
                var details = JObject.Parse(model.data);
                var hhhh = Newtonsoft.Json.JsonConvert.DeserializeObject<SubmitWorkRescheduleInput>(model.data);

                if (!string.IsNullOrEmpty(model.data))
                {
                    for (int i = 0; i < model.attachments.Count; i++)
                    {
                        attachmentName = model.attachments[i].FileName;
                        attachmentName = hhhh.session_token + '_' + hhhh.user_id + '_' + attachmentName;
                        string vPath = Path.Combine(Server.MapPath("~/CommonFolder/ScheduleAttachment"), attachmentName);
                        model.attachments[i].SaveAs(vPath);

                        //omedl2.Add(new CustomerJobStatusImagees()
                        //{
                        //    attachment = attachmentName,
                        //});
                        Jsondt.Rows.Add(new Object[] { attachmentName });
                    }
                }
                //string JsonXML = XmlConversion.ConvertToXml(omedl2, 0);

                string sessionId = "";

                DataTable dt = new DataTable();
                String con = System.Configuration.ConfigurationSettings.AppSettings["DBConnectionDefault"];
                SqlCommand sqlcmd = new SqlCommand();
                SqlConnection sqlcon = new SqlConnection(con);
                sqlcon.Open();
                sqlcmd = new SqlCommand("PRC_AssignScheduleStatusInsertUpdate", sqlcon);
                sqlcmd.Parameters.Add("@Action", "WorkReschedule");
                sqlcmd.Parameters.Add("@user_id", hhhh.user_id);
                sqlcmd.Parameters.Add("@SCH_ID", hhhh.job_id);
                sqlcmd.Parameters.Add("@date", hhhh.reschedule_date);
                sqlcmd.Parameters.Add("@time", hhhh.reschedule_time);
                sqlcmd.Parameters.Add("@RESCHEDULE_reason", hhhh.resc_reason);
                sqlcmd.Parameters.Add("@remarks", hhhh.remarks);
                sqlcmd.Parameters.Add("@date_time", hhhh.date_time);
                sqlcmd.Parameters.Add("@latitude", hhhh.latitude);
                sqlcmd.Parameters.Add("@longitude", hhhh.longitude);
                sqlcmd.Parameters.Add("@address", hhhh.address);
                sqlcmd.Parameters.Add("@RESCHEDULE_USER", hhhh.reschedule_by);
                sqlcmd.Parameters.Add("@RESCHEDULE_BY", hhhh.user);
                sqlcmd.Parameters.Add("@fsm_id", hhhh.fsm_id);
                sqlcmd.Parameters.Add("@ATTACHMENT", Jsondt);
                sqlcmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                da.Fill(dt);
                sqlcon.Close();
                if (dt.Rows.Count > 0)
                {
                    omodel.status = "200";
                    omodel.message = "Successfully submit work cancelled.";
                    omodel.id = dt.Rows[0]["ID"].ToString();
                }
                else
                {
                    omodel.status = "205";
                    omodel.message = "Failed.";
                }
            }
            catch (Exception msg)
            {
                omodel.status = "204" + attachmentName;
                omodel.message = msg.Message;
            }
            return Json(omodel);
        }
    }
}