using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer
{
    public class SendSmsBL
    {
        public string getMsgbody(int moduleID)
        {
            string message = "";
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("Proc_SmsTexts");
            proc.AddIntegerPara("@SmsModuleID", moduleID);
            dt = proc.GetTable();
            if (dt != null && dt.Rows.Count > 0)
            {
                message = dt.Rows[0]["TEXTSMS"].ToString();
            }
            return message;
        }

        public string sendSMS(String mobile, String message)
        {
            String SmsSentmsg = "";
            try
            {
                DataTable dt = new DataTable();
                ProcedureExecute proc = new ProcedureExecute("Proc_ApismsConfiguration");
                dt = proc.GetTable();
                if (dt != null && dt.Rows.Count > 0)
                {
                    string username = dt.Rows[0]["UserName"].ToString();
                    string password = dt.Rows[0]["Password"].ToString();
                    string Provider = dt.Rows[0]["Provider"].ToString();
                    string senderId = dt.Rows[0]["SenderId"].ToString();
                    string type = "";
                    if (!string.IsNullOrEmpty(dt.Rows[0]["Type"].ToString()))
                    {
                        type = dt.Rows[0]["Type"].ToString();
                    }
                    else
                    {
                        type = dt.Rows[0]["Route"].ToString();
                    }
                    SmsSentmsg = SmsSent(username, password, Provider, senderId, mobile, message, type);
                }
                return SmsSentmsg;
            }
            catch
            {
                return null;
            }
        }

        public string SmsSent(string username, string password, string Provider, string senderId, string mobile, string message, string type)
        {
            //  http://5.189.187.82/sendsms/sendsms.php?username=QHEkaruna&password=baj8piv3&type=Text&sender=KARUNA&mobile=9831892083&message=HELO
            string response = "";
           // string url = Provider + "?username=" + username + "&password=" + password + "&type=" + type + "&sender=" + senderId + "&mobile=" + mobile + "&message=" + message;
            //For Amit 
            //http://sms.netsanchar.com/http-api.php?username=amitmart&password=9874594211&senderid=AMTMRT&route=11&number=9563218466&message=hello%20there%20Testing
            string url = Provider + "?username=" + username + "&password=" + password + "&senderid=" + senderId + "&route=" + type + "&number=" + mobile + "&message=" + message;
            if (mobile.Trim() != "")
            {
                try
                {
                    var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                    var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                    response = httpResponse.StatusCode.ToString();
                }
                catch
                {
                    return "0";
                }
            }
            return response;
        }

        public int SmsStatusSave(string TextSent, String FromModule, String deliveryStatus, String SentNo)
        {
            ProcedureExecute proc;
            try
            {
                using (proc = new ProcedureExecute("Proc_InsertSensSmsStatus"))
                {
                    proc.AddVarcharPara("@SentNo", 50, SentNo);
                    proc.AddVarcharPara("@TextSent", 500, TextSent);
                    proc.AddVarcharPara("@FromModule", 100, FromModule);
                    proc.AddVarcharPara("@deliveryStatus", 100, deliveryStatus);
                    int i = proc.RunActionQuery();
                    return i;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                proc = null;
            }
        }

        public DataTable GetSMSSettings(String Variable_Name)
        {
            ProcedureExecute proc;
            try
            {
                using (proc = new ProcedureExecute("Prc_GetConfigDetails"))
                {
                    proc.AddVarcharPara("@Variable_Name", 500, Variable_Name);
                    return proc.GetTable();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                proc = null;
            }

        }
    }
}
