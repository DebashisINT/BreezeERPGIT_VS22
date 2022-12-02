using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Configuration;
using System.Web.Services;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using BusinessLogicLayer;
using BusinessLogicLayer.EmailTemplate;
using Newtonsoft.Json;
using DataAccessLayer;
using System.Web.Script.Serialization;
using System.Data.SqlClient;

namespace ERP.OMS.Management.ToolsUtilities
{
    public partial class management_utilities_frmHeaderFooterTempl : System.Web.UI.Page, System.Web.UI.ICallbackEventHandler
    {
        //DBEngine oDBEngine = new DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);
        //DBEngine oDBEngine = new DBEngine(string.Empty);

        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);MULTI
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        public EntityLayer.CommonELS.UserRightsForPage rights = new EntityLayer.CommonELS.UserRightsForPage();
        string data = "";
        public string pageAccess = "";

        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //'http://localhost:2957/InfluxCRM/management/testProjectMainPage_employee.aspx'

                // Replace .ToString() with Convert.ToString(..) By Sudip on 16122016

                string sPath = Convert.ToString(HttpContext.Current.Request.Url);
                oDBEngine.Call_CheckPageaccessebility(sPath);

            }
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            ////if (HttpContext.Current.Session["userid"] == null)
            ////{
            ////    Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            ////}
            //////this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");

            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/ToolsUtilities/frmHeaderFooterTempl.aspx?type=email");

            if (!IsPostBack)
            {

                SetEmailSenderType();
                Session["KeyVal"] = null;
                //txt_ajax.Attributes.Add("onkeyup", "ajax_showOptions(this,'userdetails',event,'drpProducttype')");
                BindGrid();
                fillgrid();
                Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script language='javascript'>Page_Load();</script>");
                //drp_templatetype.Attributes.Add("onchange", "drpChange()");
                String cbReference = Page.ClientScript.GetCallbackEventReference(this, "arg", "ReceiveServerData", "context");
                String callbackScript = "function CallServer(arg, context){ " + cbReference + ";}";
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "CallServer", callbackScript, true);
                BindDiv();


            }
        }




        [WebMethod]
        [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
        //public static string FetchHeaderFooterTagsByStage(string Id)
        public static string FetchHeaderFooterTagsByStage()
        {
            DataTable dt1 = new DataTable();
            Emailtemplate obj = new Emailtemplate();

            List<HFtags> HFTags = new List<HFtags>();
            try
            {
                //dt1 = Emailtemplate.GetEmailTags(Id);
                dt1 = Emailtemplate.GetHeaderFooterTags();

                HFTags = DbHelpers.ToModelList<HFtags>(dt1);


                //for (int i = 0; i < dt1.Rows.Count; i++)
                //{

                //    emailTags.Add(new Emailtags()
                //    {

                //        EmailTags = Convert.ToString(dt1.Rows[i]["Brand_Name"]),
                //        Id = Convert.ToInt32(dt1.Rows[i]["Id"]),
                //        StageId = Convert.ToInt32(dt1.Rows[i]["StageId"])
                //    });
                //}

                if (HFTags != null && HFTags.Count > 0)
                {
                    var serializer = new JavaScriptSerializer(); var serializedResult = serializer.Serialize(HFTags);
                }

                return JsonConvert.SerializeObject(HFTags);

            }
            catch (Exception ex)
            {
                return "Error occured";
            }
        }




        [WebMethod]
        [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
        public static string Getupdatedata(string Id)
        {
            DataTable dt1 = new DataTable();
            Emailtemplate obj = new Emailtemplate();

            List<Emailtags> emailTags = new List<Emailtags>();
            try
            {
                dt1 = Emailtemplate.GetEmailTags(Id);

                emailTags = DbHelpers.ToModelList<Emailtags>(dt1);

                if (emailTags != null && emailTags.Count > 0)
                {
                    var serializer = new JavaScriptSerializer(); var serializedResult = serializer.Serialize(emailTags);
                }

                return JsonConvert.SerializeObject(emailTags);

            }
            catch (Exception ex)
            {
                return "Error occured";
            }
        }

        [WebMethod]
        public static int CheckRecExist(string ModuleID, string name)
        {
            DataSet dsInst = new DataSet();
            SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            SqlCommand cmd = new SqlCommand("prc_HeaderFooterTemplateAddit", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Action", "CheckExist");
            cmd.Parameters.AddWithValue("@Module_Id", ModuleID);
            cmd.Parameters.AddWithValue("@name", name);
            cmd.Parameters.Add("@ReturnValue", SqlDbType.VarChar, 50);
            cmd.Parameters["@ReturnValue"].Direction = ParameterDirection.Output;
            cmd.CommandTimeout = 0;
            SqlDataAdapter Adap = new SqlDataAdapter();
            Adap.SelectCommand = cmd;
            Adap.Fill(dsInst);
            int idFromString = Convert.ToInt32(cmd.Parameters["@ReturnValue"].Value.ToString());

            return idFromString;
        }



        public void SetEmailSenderType()
        {
            //objEngine
            DataTable DT = new DataTable();
            DT = oDBEngine.GetDataTable("tbl_ModuleList", "Module_ID,Module_Name ", null);
            ddlModuleName.DataSource = DT;
            ddlModuleName.DataMember = "Module_ID";
            ddlModuleName.DataTextField = "Module_Name";
            ddlModuleName.DataValueField = "Module_ID";
            ddlModuleName.DataBind();
        }
        public void BindGrid()
        {
            // Replace .ToString() with Convert.ToString(..) By Sudip on 16122016

            DataTable dt = new DataTable();

            #region
            try
            {
                //if (Convert.ToString(Request.QueryString["type"]).ToLower() == "msg")
                //{
                //    dt = oDBEngine.GetDataTable("tbl_master_template", "tem_id,(tem_shortmsg) as Title", " tem_type=1 and ((tem_accesslevel=1) or (createuser=" + Convert.ToString(Session["userid"]) + "))", "tem_shortmsg");
                //    //drp_templatetype.SelectedValue = "1";
                //}
                //else if (Convert.ToString(Request.QueryString["type"]).ToLower() == "email")
                //{
                //    dt = oDBEngine.GetDataTable("tbl_master_template", "tem_id,(tem_shortmsg) as Title,case when IsDefault=1 then 'ACTIVE' else 'DEACTIVE' end as IsDefault", " tem_type=2", "tem_shortmsg");
                //    //drp_templatetype.SelectedValue = "2";
                //}

                DataSet dsInst = new DataSet();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("prc_HeaderFooterTemplateAddit", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", "FetchListData");
                cmd.CommandTimeout = 0;
                SqlDataAdapter Adap = new SqlDataAdapter();
                Adap.SelectCommand = cmd;
                Adap.Fill(dsInst);

                dt = dsInst.Tables[0];
                GrdTemplate.DataSource = dt;
                GrdTemplate.DataBind();
            }
            catch (Exception ex)
            {

            }

            #endregion
        }
        void ICallbackEventHandler.RaiseCallbackEvent(string eventArgument)
        {
            // Replace .ToString() with Convert.ToString(..) By Sudip on 16122016
            string Html_Msg = string.Empty;
            data = "";
            string id = Convert.ToString(eventArgument);
            string[] idlist = id.Split('~');
            #region AddUser
            if (idlist[0] == "AddUser")
            {

                DataTable dt_temp = new DataTable();
                DataTable dt = new DataTable();
                if (Session["KeyVal"] != null)
                {
                    dt_temp = (DataTable)Session["KeyVal"];
                    dt = dt_temp.Copy();
                }
                else
                {

                    DataColumn dc = new DataColumn("UID");
                    DataColumn dc1 = new DataColumn("UNAME");
                    dt.Columns.Add(dc);
                    dt.Columns.Add(dc1);
                }

                string username = Convert.ToString(idlist[1]);
                string uid = Convert.ToString(idlist[2]);

                //string[] str = idlist[1].Trim().Split('-');
                string user_id = "";
                // string[,] userid = oDBEngine.GetFieldValue("tbl_master_user", "top 1 user_id", " user_contactId='" + str[1] + "'", 1);
                string[,] userid = oDBEngine.GetFieldValue("tbl_master_user", "top 1 user_id", " user_contactId='" + uid + "'", 1);
                if (userid[0, 0] != "n")
                {
                    user_id = userid[0, 0];

                    DataRow dr = dt.NewRow();
                    dr["UID"] = user_id;
                    //  dr["UNAME"] = str[0];
                    dr["UNAME"] = username;
                    dt.Rows.Add(dr);
                    dt_temp = dt.Copy();
                    Session["KeyVal"] = dt;
                }

            }
            #endregion
            #region Remove
            if (idlist[0] == "Remove")
            {
                string rowid = idlist[1];
                DataTable dt = (DataTable)Session["KeyVal"];
                //DataRow dr = dt.Rows.Find(rowid);
                //dt.Rows.IndexOf(dr);  
                foreach (DataRow dr in dt.Rows)
                {
                    if (Convert.ToString(dr.ItemArray[0]) == rowid)
                    {
                        dr.Delete();
                        dt.AcceptChanges();
                    }
                }
                dt.AcceptChanges();
                Session["KeyVal"] = dt;
            }
            #endregion
            #region Save

            

            if (idlist[0] == "AddSave" || idlist[0]=="EditSave" )
            {
                //int NoOfEffected = 0;
                DataTable dt = new DataTable();
                string user = "0";
                long Module_Id = Convert.ToInt64( idlist[1]);
                string strHeader = idlist[2];
                string strFooter = idlist[3];
                string strName = idlist[4];
                string strisdefault = idlist[5];
                string strCreatedUser = Convert.ToString(Session["userid"]);

                string Action = idlist[0];

                //if (idlist[5] == "")
                //{
                //    //Subhabrata:Encode the html content and insert the value to database
                //    Html_Msg = CommonBL.SafeSqlLiteral(CommonBL.CheckJavaScriptInjection(Convert.ToString(idlist[2])));

                //    NoOfEffected = oDBEngine.InsurtFieldValue("tbl_master_template", "tem_shortmsg,tem_msg,tem_recipients,tem_type,tem_accesslevel,tem_sendertype,CreateDate,CreateUser,IsDefault", "'" + idlist[1] + "','" + idlist[2] + "','" + idlist[7] + "','" + idlist[3] + "','" + idlist[4] + "','" + idlist[6] + "','" + Convert.ToString(System.DateTime.Now) + "','" + Convert.ToString(Session["userid"]) + "','" + idlist[8] + "'");
                //    //End
                //}
                //else
                //{
                //    //Modified by:Subhabrata
                //    NoOfEffected = oDBEngine.SetFieldValue("tbl_master_template", "tem_shortmsg='" + idlist[1] + "',tem_msg='" + idlist[2] + "',tem_recipients='" + idlist[7] + "',tem_type='" + idlist[3] + "',tem_accesslevel='" + idlist[4] + "',tem_sendertype='" + idlist[6] + "',LastModifyDate='" + Convert.ToString(System.DateTime.Now) + "',LastModifyUser='" + Convert.ToString(Session["userid"]) + "',IsDefault='" + idlist[8] + "'", " tem_id='" + idlist[5] + "'");
                //    //End
                //}

                DataSet dsInst = new DataSet();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("prc_HeaderFooterTemplateAddit", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", Action);
                cmd.Parameters.AddWithValue("@Module_Id", Module_Id);
                cmd.Parameters.AddWithValue("@Header", strHeader);
                cmd.Parameters.AddWithValue("@Footer", strFooter);
                cmd.Parameters.AddWithValue("@CreateUser", strCreatedUser);
                cmd.Parameters.AddWithValue("@name", strName);
                cmd.Parameters.AddWithValue("@IsDefault", strisdefault);
                cmd.Parameters.Add("@ReturnValue", SqlDbType.VarChar, 50);
                cmd.Parameters["@ReturnValue"].Direction = ParameterDirection.Output;
                cmd.CommandTimeout = 0;
                SqlDataAdapter Adap = new SqlDataAdapter();
                Adap.SelectCommand = cmd;
                Adap.Fill(dsInst);
                int idFromString = Convert.ToInt32(cmd.Parameters["@ReturnValue"].Value.ToString());


                if (idFromString == 1)
                {
                    data = "Save~Y";
                }
            }
            #endregion
            #region Update
            if (idlist[0] == "Update")
            {

                string Tmp_id = idlist[1];

                DataSet dsInst = new DataSet();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("prc_HeaderFooterTemplateAddit", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", "Load");
                cmd.Parameters.AddWithValue("@Tmp_id", Tmp_id);
                cmd.CommandTimeout = 0;
                SqlDataAdapter Adap = new SqlDataAdapter();
                Adap.SelectCommand = cmd;
                Adap.Fill(dsInst);
                DataTable dt = dsInst.Tables[0];

                string Medule_Id = Convert.ToString(dt.Rows[0]["Medule_Id"]);
                string Header = Convert.ToString(dt.Rows[0]["Header"]);
                string Footer = Convert.ToString(dt.Rows[0]["Footer"]);
                string Name = Convert.ToString(dt.Rows[0]["Name"]);
                string IsDefault = Convert.ToString(dt.Rows[0]["IsDefault"]);
                

                int rowCount = dt.Rows.Count;
                data = "Edit" + "~" + Medule_Id + "~" + Header + "~" + Footer + "~" + Tmp_id + "~" + Name + "~" + IsDefault + "~" + rowCount;
                Session["KeyVal"] = dt;


                //string RowID = idlist[1];
                //DataTable dt = oDBEngine.GetDataTable("tbl_master_template", "*", "tem_id='" + RowID + "'");
                //string description = Convert.ToString(dt.Rows[0]["tem_shortmsg"]);
                ////string msg =  Convert.ToString(dt.Rows[0]["tem_msg"]);

                ////subhabrata decode value from database and pass the value to the Server
                //string msg = System.Web.HttpUtility.HtmlDecode(dt.Rows[0]["tem_msg"].ToString());
                ////End
                //string receip = Convert.ToString(dt.Rows[0]["tem_recipients"]);
                //string tempType = Convert.ToString(dt.Rows[0]["tem_type"]);
                //string accessLevel = Convert.ToString(dt.Rows[0]["tem_accesslevel"]);
                //string senderType = Convert.ToString(dt.Rows[0]["tem_sendertype"]);
                ////Added By:Subhabrata
                //string IsDefault_chk = Convert.ToString(dt.Rows[0]["IsDefault"]);
                ////End

                ////drpSenderType.Items.FindByValue(senderType).Selected = true;
                //if (receip != null && receip != "")
                //{
                //    dt.Clear();
                //    dt = oDBEngine.GetDataTable("tbl_master_user", "user_id as UID,user_name as UNAME", " user_id IN(" + receip + ")");
                //}
                //else
                //{
                //    dt.Clear();
                //    dt = oDBEngine.GetDataTable("tbl_master_user", "user_id as UID,user_name as UNAME", " user_id IN(0)");
                //}

                //int rowCount = dt.Rows.Count;
                //data = "Edit" + "~" + description + "~" + msg + "~" + receip + "~" + tempType + "~" + accessLevel + "~" + RowID + "~" + rowCount + "~" + senderType + "~" + IsDefault_chk;
                //Session["KeyVal"] = dt;

            }
            #endregion
            #region Delete
            if (idlist[0] == "Delete")
            {
                //int NoOfRowsAffected = 0;
                //NoOfRowsAffected = oDBEngine.DeleteValue("tbl_master_template", " tem_id='" + idlist[1] + "'");
                //if (NoOfRowsAffected != 0)
                //{
                //    data = "Delete~Y";
                //}

                string Tmp_id = idlist[1];

                DataSet dsInst = new DataSet();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("prc_HeaderFooterTemplateAddit", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", "Delete");
                cmd.Parameters.AddWithValue("@Tmp_id", Tmp_id);
                cmd.Parameters.Add("@ReturnValue", SqlDbType.VarChar, 50);
                cmd.Parameters["@ReturnValue"].Direction = ParameterDirection.Output;
                cmd.CommandTimeout = 0;
                SqlDataAdapter Adap = new SqlDataAdapter();
                Adap.SelectCommand = cmd;
                Adap.Fill(dsInst);
                int idFromString = Convert.ToInt32(cmd.Parameters["@ReturnValue"].Value.ToString());


                if (idFromString == 1)
                {
                    data = "Delete~Y";
                }
            }
            #endregion
            #region Add
            if (idlist[0] == "Add")
            {
                Session["KeyVal"] = null;
                SetEmailSenderType();
            }
            #endregion
            #region IsDefault
            if (idlist[0] == "Access")
            {
                DataTable dtStat = new DataTable();
                string RowID = idlist[1];
                dtStat = oDBEngine.GetDataTable("Config_EmailAccounts ", " EmailAccounts_InUse  ", "EmailAccounts_ID ='" + Convert.ToString(RowID) + "'");
            }
            #endregion
        }
        string ICallbackEventHandler.GetCallbackResult()
        {
            return data;

        }
        //protected void grduser_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        //{
        //    fillgrid();
        //}

        #region DefaultCheck
        [WebMethod]
        public static bool AddEmailTemplateDefaultInfo(string SenderType)
        {
            bool isUpdated = false;
            Employee_BL objemployeebal = new Employee_BL();
            if (!string.IsNullOrEmpty(SenderType))
            {
                isUpdated = objemployeebal.AddEmailTemplateDefaultDetails(SenderType);
            }
            return isUpdated;
        }
        #endregion
        public void fillgrid()
        {
            try
            {
                //DataTable dt = (DataTable)Session["KeyVal"];
                //grduser.DataSource = dt;
                //grduser.DataBind();
            }
            catch
            {
            }
        }
        protected void GrdTemplate_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            BindGrid();
        }
        [WebMethod]
        public static List<string> GetRecipients(string Type)
        {
            //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);MULTI
            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
            DataTable DT = oDBEngine.GetDataTable("tbl_master_user", "user_name,user_id ", null);
            List<string> obj = new List<string>();
            foreach (DataRow dr in DT.Rows)
            {
                obj.Add(Convert.ToString(dr["user_name"]) + "|" + Convert.ToString(dr["user_id"]));
            }

            return obj;
        }
        public void BindDiv()
        {
            string[] list = new String[] { "receipent", "sender" }; ;
            //Converter oConverter = new Converter();     //____This is to call recipient variable with the predefined values.
            BusinessLogicLayer.Converter oConverter = new BusinessLogicLayer.Converter();
            string UIstring = "<div>";
            if (list[0] == "receipent")
            {
                string[,] recipient = oConverter.ReservedWord_recipient();
                string mess = "window.opener.document.aspnetForm.txt_msg.value";
                for (int i = 0; i < recipient.Length / 2; i++)
                {
                    UIstring += "<input style='border-right: 1px groove; border-top: 1px groove; font-size: 8pt;  border-left: 1px groove; color: black; border-bottom: 1px groove; font-style: normal; font-family: Verdana; width: 125px' onclick='PostReservedWord(this.value);'  type='button' id='chk' name='chk' value='" + recipient[i, 0] + "'>";
                }
            }
            if (list.Length > 1)
            {
                if (list[1] == "sender")
                {
                    string[,] sender1 = oConverter.ReservedWord_sender();
                    for (int i = 0; i < sender1.Length / 2; i++)
                    {
                        UIstring += "<input style='border-right: 1px groove; border-top: 1px groove; font-size: 8pt;  border-left: 1px groove; color: black; border-bottom: 1px groove; font-style: normal; font-family: Verdana; width: 125px' onclick='PostReservedWord(this.value);'  type='button' id='chk' name='chk' value='" + sender1[i, 0] + "'>";
                    }
                }
            }
            UIstring += "</div>";
            myDiv.InnerHtml = UIstring;
        }
    }
}