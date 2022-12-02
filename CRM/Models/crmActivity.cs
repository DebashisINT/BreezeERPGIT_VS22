using CRM.Models.DataContext;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace CRM.Models
{
    public class crmActivity
    {
        public DataSet GetAllDropdown()
        {
            try
            {
                DataSet dsInst = new DataSet();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("prc_CRMActivity", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", "GetAllDropDown");
                cmd.CommandTimeout = 0;
                SqlDataAdapter Adap = new SqlDataAdapter();
                Adap.SelectCommand = cmd;
                Adap.Fill(dsInst);
                cmd.Dispose();
                con.Dispose();

                return dsInst;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        // Rev Mantis Issue 22801 [parameter Duration_New added]
        // Mantis Issue 24175 [new parameter ActivityTypeNew added]
        public string SaveActivity(string activity, string activity_type, string subject, string details, string assignto, string duration, string priority, DateTime? DtxtDue, DateTime? dtActivityDate, string CRMactivityid, string contacttype, DataTable dt_activityproducts, string Module_Name, TimeSpan Duration_New, string ActivityTypeNew)
        {
            try
            {
                int OutputId = 0;
                DataSet dsInst = new DataSet();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("PRC_LEAD_SALESACTIVITY", con);

                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ACTION_TYPE", "SAVE");
                cmd.Parameters.AddWithValue("@LEAD_ACTIVITYID", activity);
                cmd.Parameters.AddWithValue("@TYPEID", activity_type);
                cmd.Parameters.AddWithValue("@LEADSUBJECT", subject);
                cmd.Parameters.AddWithValue("@LEADDETAILS", details);
                cmd.Parameters.AddWithValue("@ASSIGNTO", assignto);
                // Rev Mantis Issue 22801
                //cmd.Parameters.AddWithValue("@DURATIONID", duration);
                cmd.Parameters.AddWithValue("@DURATIONID", 0);
                // End of Rev Mantis Issue 22801
                cmd.Parameters.AddWithValue("@PRIORITYID", priority);
                cmd.Parameters.AddWithValue("@DUEDATE", DtxtDue);
                cmd.Parameters.AddWithValue("@ACTIVITYDATE", dtActivityDate);
                cmd.Parameters.AddWithValue("@LEAD_ENTITY_ID", CRMactivityid);
                cmd.Parameters.AddWithValue("@MODULENAME", Module_Name);
                cmd.Parameters.AddWithValue("@CONTACTTYPE", contacttype);
                cmd.Parameters.AddWithValue("@ActivityProducts", dt_activityproducts);

                cmd.Parameters.AddWithValue("@CREATED_BY", Convert.ToInt32(System.Web.HttpContext.Current.Session["userid"]));
                // Rev Mantis Issue 22801
                cmd.Parameters.AddWithValue("@DURATION_NEW", Duration_New);
                // End of Rev Mantis Issue 22801
                // Mantis Issue 24175
                cmd.Parameters.AddWithValue("@TYPENEWID", ActivityTypeNew);
                // End of Mantis Issue 24175

                SqlParameter output = new SqlParameter("@ReturnMessage", SqlDbType.VarChar, 500);
                output.Direction = ParameterDirection.Output;

                SqlParameter outputText = new SqlParameter("@ReturnCode", SqlDbType.VarChar, 20);
                outputText.Direction = ParameterDirection.Output;


                cmd.Parameters.Add(output);
                cmd.Parameters.Add(outputText);

                cmd.CommandTimeout = 0;
                SqlDataAdapter Adap = new SqlDataAdapter();
                Adap.SelectCommand = cmd;
                Adap.Fill(dsInst);
                cmd.Dispose();
                con.Dispose();


                OutputId = Convert.ToInt32(cmd.Parameters["@ReturnCode"].Value.ToString());

                string strCPRID = Convert.ToString(cmd.Parameters["@ReturnMessage"].Value.ToString());
                return strCPRID;
            }
            catch (Exception ex)
            {
                return null;
            }
        }




        public string AssignToId { get; set; }
        public List<Contact_Type> Contact_Type { get; set; }
        public List<Activity> Activity { get; set; }
        public List<Lead_ActivityType> ActivityType { get; set; }
        public List<AssignTo> AssignTo { get; set; }
        public List<Duration> Duration { get; set; }
        public List<Prioritys> Priority { get; set; }
        // Mantis Issue 24175
        public List<Lead_ActivityTypeNew> ActivityTypeNew { get; set; }
        // End of Mantis Issue 24175


        internal DataSet EditCRMActivity(string Module_Name, string Module_id)
        {
            try
            {
                int OutputId = 0;
                DataSet dsInst = new DataSet();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("CRM_ACTIVITY", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ACTION", "EDIT");
                cmd.Parameters.AddWithValue("@Module_Name", Module_Name);
                cmd.Parameters.AddWithValue("@Module_id", Module_id);
                cmd.CommandTimeout = 0;
                SqlDataAdapter Adap = new SqlDataAdapter();
                Adap.SelectCommand = cmd;
                Adap.Fill(dsInst);
                cmd.Dispose();
                con.Dispose();
                return dsInst;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable GetShowHistoryData(string action_type, string entity_id, string Module)
        {
            try
            {
                DataTable dsInst = new DataTable();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("PRC_ACTIVITYSHOWHISTORY", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ACTION_TYPE", action_type);
                cmd.Parameters.AddWithValue("@ENTITY_ID", entity_id);
                cmd.Parameters.AddWithValue("@Module", Module);
                cmd.CommandTimeout = 0;
                SqlDataAdapter Adap = new SqlDataAdapter();
                Adap.SelectCommand = cmd;
                Adap.Fill(dsInst);
                cmd.Dispose();
                con.Dispose();

                return dsInst;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }


    public class Contact_Type
    {
        public string Code { get; set; }
        public string ContactType { get; set; }

    }

    public class Activity
    {
        public string ID { get; set; }
        public string Activity_Code { get; set; }

    }
    public class ActivityType
    {
        public string ID { get; set; }
        public string Activity_Type { get; set; }

    }
    public class AssignTo
    {
        public string ID { get; set; }
        public string Assign_To { get; set; }

    }

    public class Duration
    {
        public string ID { get; set; }
        public string Duration_Code { get; set; }

    }

    public class Prioritys
    {
        public string ID { get; set; }
        public string Priority { get; set; }

    }

    public class Lead_ActivityTypeNew
    {
        public string ID { get; set; }
        public string ActivityTypeNew { get; set; }

    }

    

    public class CRMActivityProductDetails
    {
        public string guid { get; set; }
        public int ActivityId { get; set; }
        public string Lead_Entity_id { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal Quantity { get; set; }
        public decimal Rate { get; set; }
        public string Remarks { get; set; }
    }

    public class crmPosProductModel
    {
        public string id { get; set; }
        public string Na { get; set; }
        public string Des { get; set; }
        public string HSN { get; set; }
    }







}