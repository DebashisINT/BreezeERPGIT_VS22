using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogicLayer;

namespace ERP.OMS.Management.ToolsUtilities
{
    public partial class management_utilities_frm_SendParameterForSMS : System.Web.UI.Page, System.Web.UI.ICallbackEventHandler
    {

        string data;
        string BranchId = null;
        string Clients;
        string Group = null;
        static string CompanyID = null;
        BusinessLogicLayer.Converter c = new BusinessLogicLayer.Converter();
        GetAutomaticMail mail = new GetAutomaticMail();
        Utilities obj = new Utilities();
        //DBEngine oDBEngine = new DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);
        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);MULTI
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        BusinessLogicLayer.Converter ObjConvert = new BusinessLogicLayer.Converter();
        protected void Page_Init(object sender, EventArgs e)
        {
            sqlMode.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            sqlDescription.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
        }
        protected void Page_Load(object sender, EventArgs e)
        {


            Page.ClientScript.RegisterStartupScript(GetType(), "CallingHeight", "<script language='JavaScript'>height();</script>");
            if (!IsPostBack)
            {
                if (Session["userlastsegment"].ToString() == "9" || Session["userlastsegment"].ToString() == "10")
                {
                    sqlDescription.SelectCommand = "select topics_accesscode,topics_description from master_topics where (topics_applicableentity='D')";
                }
                else if (Session["userlastsegment"].ToString() == "4")
                {
                    sqlDescription.SelectCommand = "select topics_accesscode,topics_description from master_topics where ( topics_applicableentity='M')";

                }
                else
                {
                    sqlDescription.SelectCommand = "select topics_accesscode,topics_description from master_topics where ( topics_applicableentity='C')";
                }
                dpDescription.DataBind();
                Page.ClientScript.RegisterStartupScript(GetType(), "PageLoad", "<script language='JavaScript'>Page_Load();</script>");
            }

            //_____For performing operation without refreshing page___//
            String cbReference = Page.ClientScript.GetCallbackEventReference(this, "arg", "ReceiveServerData", "context");
            String callbackScript = "function CallServer(arg, context){ " + cbReference + ";}";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "CallServer", callbackScript, true);
            //this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");
            //___________-end here___//
        }
        void ICallbackEventHandler.RaiseCallbackEvent(string eventArgument)
        {
            string id = eventArgument.ToString();
            string[] idlist = id.Split('~');
            string[] cl = idlist[1].Split(',');
            string str = "";
            string str1 = "";
            for (int i = 0; i < cl.Length; i++)
            {
                string[] val = cl[i].Split(';');
                if (str == "")
                {
                    str = "'" + val[0] + "'";
                    str1 = val[0] + ";" + val[1];
                }
                else
                {
                    str += ",'" + val[0] + "'";
                    str1 += "," + val[0] + ";" + val[1];
                }
            }

            if (idlist[0] == "Clients")
            {
                Clients = str;
                data = "Clients~" + str;
                ViewState["Clients"] = Clients;
            }

            else if (idlist[0] == "Group")
            {
                Group = str;
                data = "Group~" + str;
            }
            else if (idlist[0] == "Branch")
            {
                BranchId = str;
                data = "Branch~" + str;
            }

        }
        string ICallbackEventHandler.GetCallbackResult()
        {
            return data;
        }
        protected void btnhide_Click(object sender, EventArgs e)
        {
            if (ddlGroup.SelectedItem.Value.ToString() == "1")
            {
                BindGroup();
            }
        }
        protected void btnEmail_Click(object sender, EventArgs e)
        {
            CmbNotType.Items.Clear();
            string Mode = oDBEngine.GetFieldValue("master_topics", "topics_deliverymode", "topics_Accesscode='" + dpDescription.SelectedItem.Value + "'", 1)[0, 0];
            string TableName = "dbo.fnSplitReturnTable(" + "'" + Mode + "'" + ",',')";
            DataTable DTMode = new DataTable();
            DTMode = oDBEngine.GetDataTable("Master_Topics", "* ,(select top 1 case Topics_DeliveryMode when 'E' then 'Email' when 'M' then 'Message' when 'B' then 'Email & SMS' when 'R' then 'Reminder' else 'SMS' end", "Topics_DeliveryMode=col) as Descriptions from " + TableName);
            for (int i = 0; i < DTMode.Rows.Count; i++)
            {
                CmbNotType.Items.Insert(0, new ListItem(DTMode.Rows[i][1].ToString(), DTMode.Rows[i][0].ToString()));

            }
        }
        public void BindGroup()
        {
            ddlgrouptype.Items.Clear();
            DataTable DtGroup = oDBEngine.GetDataTable("tbl_master_groupmaster", "distinct gpm_Type", null);
            if (DtGroup.Rows.Count > 0)
            {
                ddlgrouptype.DataSource = DtGroup;
                ddlgrouptype.DataTextField = "gpm_Type";
                ddlgrouptype.DataValueField = "gpm_Type";
                ddlgrouptype.DataBind();
                ddlgrouptype.Items.Insert(0, new ListItem("Select GroupType", "0"));
                DtGroup.Dispose();

            }
            else
            {
                ddlgrouptype.Items.Insert(0, new ListItem("Select GroupType", "0"));
            }

        }

        protected void BtnSubmit_Click(object sender, EventArgs e)
        {

            DataTable DT = new DataTable();
            string FirstVariable;
            string SecondVariable;
            string ThirdVariable = "";
            string ForthVariable = "";
            string GeneratedQueryString = "";
            string CompleteQueryString = "";
            string Branch = Session["userbranchHierarchy"].ToString();
            string Group = "Group";
            string Client = "";
            string GroupType = "N";
            string NotType = CmbNotType.SelectedItem.Value;

            FirstVariable = lblVariblehid1.Value.ToString();
            SecondVariable = lblVariablehid2.Value.ToString();
            ThirdVariable = lblDatehid1.Value.ToString();
            ForthVariable = lbldatehid2.Value.ToString();

            if (ddlGroup.SelectedItem.Value == "0")
            {
                if (rdbranchAll.Checked == true)
                {
                    Branch = Session["userbranchHierarchy"].ToString();
                }
                else
                {
                    if (HdnBranchId.Value.ToString() != "")
                    {
                        Branch = HdnBranchId.Value;
                    }
                    else
                    {
                        Branch = Session["userbranchHierarchy"].ToString();
                    }
                }
            }
            else
            {
                if (rdddlgrouptypeAll.Checked == true)
                {
                    if (ddlgrouptype.SelectedItem.Value != "0")
                    {
                        GroupType = "Y";
                        Group = ddlgrouptype.SelectedItem.Value;
                    }
                    else
                    {
                        Group = "Group";
                    }
                }
                else
                {
                    if (HdnGroup.Value.ToString() != "")
                    {
                        Group = HdnGroup.Value;
                    }
                    else
                    {
                        Group = "Group";
                    }
                }

            }

            if (rdbALLSCL.Checked == true)
            {
                Client = "S";
            }
            else if (radAllCL.Checked == true)
            {
                Client = "A";
            }
            else if (rdbSCL.Checked == true)
            {
                if (HdnClients.Value.ToString() != "")
                {
                    Client = HdnClients.Value;
                }
                else
                {
                    Client = "S";
                }
            }


            DataSet ds = new DataSet();
            //using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]))MULTI
            using (SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"])))
            {
                //using (SqlDataAdapter da = new SqlDataAdapter("Fetch_CustomerForEmailSMS", con))
                //{
                //    da.SelectCommand.Parameters.AddWithValue("@AccessCode", dpDescription.SelectedItem.Value);
                //    da.SelectCommand.Parameters.AddWithValue("@Branch", Branch);
                //    da.SelectCommand.Parameters.AddWithValue("@Group", Group);
                //    da.SelectCommand.Parameters.AddWithValue("@Client", Client);
                //    da.SelectCommand.Parameters.AddWithValue("@SendType", NotType);
                //    da.SelectCommand.Parameters.AddWithValue("@GroupType", GroupType);
                //    da.SelectCommand.Parameters.AddWithValue("@Segment", Session["userlastsegment"].ToString());
                //    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                //    da.SelectCommand.CommandTimeout = 0;

                //    if (con.State == ConnectionState.Closed)
                //        con.Open();
                //    ds.Reset();
                //    da.Fill(ds);
                //    ViewState["dataset"] = ds;
                //    ViewState["topiccode"] = ds.Tables[1].Rows[0][0].ToString();

                //}
                ds = obj.Fetch_CustomerForEmailSMS(dpDescription.SelectedItem.Value.ToString(), Branch, Group, Client, NotType, GroupType, Session["userlastsegment"].ToString());
                ViewState["dataset"] = ds;
                ViewState["topiccode"] = ds.Tables[1].Rows[0][0].ToString();
            }
            DataTable DTC = ds.Tables[0];
            if (CmbNotType.SelectedItem.Value == "E")
            {
                if (DTC.Rows.Count > 0)
                {
                    GeneratedQueryString = txtSubject.Text.ToString();
                    if (txtVariable1.Text != "")
                    {
                        GeneratedQueryString = GeneratedQueryString + " " + txtVariable1.Text.Trim().ToString();
                    }
                    if (txtVariable2.Text != "")
                    {
                        GeneratedQueryString = GeneratedQueryString + " " + txtVariable2.Text.Trim().ToString();
                    }
                    if (AspxDate1.Value != null)
                    {
                        GeneratedQueryString = GeneratedQueryString + " " + AspxDate1.Value.ToString();
                    }
                    if (AspxDate2.Value != null)
                    {
                        GeneratedQueryString = GeneratedQueryString + " " + AspxDate2.Value.ToString();
                    }
                    for (int i = 0; i < DTC.Rows.Count; i++)
                    {
                        string senderid = DTC.Rows[i][0].ToString();
                        string subject = GeneratedQueryString;
                        string ContactID = DTC.Rows[i][1].ToString();

                        DataSet DSR = new DataSet();
                        //using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]))
                        //{
                        //    using (SqlDataAdapter da = new SqlDataAdapter("FETCH_NOTIFICATION_REQUESTIDENTITY", con))
                        //    {
                        //        da.SelectCommand.Parameters.AddWithValue("@PHONE_EMAIL", senderid);
                        //        da.SelectCommand.Parameters.AddWithValue("@SUBJECT", subject);
                        //        da.SelectCommand.CommandType = CommandType.StoredProcedure;
                        //        da.SelectCommand.CommandTimeout = 0;
                        //        if (con.State == ConnectionState.Closed)
                        //            con.Open();
                        //        DSR.Reset();
                        //        da.Fill(DSR);
                        //        ViewState["RequestID"] = DSR.Tables[0].Rows[0][0].ToString();

                        //    }
                        //}
                        DSR = obj.FETCH_NOTIFICATION_REQUESTIDENTITY(senderid, subject);
                        ViewState["RequestID"] = DSR.Tables[0].Rows[0][0].ToString();
                        string Err = MailSend(senderid, subject, ContactID);
                        //  Server.Execute("../management/Sendemails_frm.aspx?senderid=" + DT.Rows[i][0].ToString() + "&subject= " + GeneratedQueryString);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Err", "alert('" + Err + "')", true);
                    }
                }

            }
            else
            {

                //DT = oDBEngine.GetDataTable("Trans_TopicSubscriptions as a,master_topics as b,tbl_master_phonefax as c", "distinct a.topicsubscriptions_phoneid,c.phf_phonenumber", "a.topicsubscriptions_topiccode=b.topics_code and a.topicsubscriptions_phoneid is not null and a.topicsubscriptions_phoneid=c.phf_id and b.topics_accesscode='" + txtAccID.Text.ToString().Trim() + "'");
                //if (DT.Rows.Count > 0)
                //{
                //    GeneratedQueryString = "KC " + txtSubject.Text.ToString();
                //    if (txtVariable1.Text != "")
                //    {
                //        GeneratedQueryString = GeneratedQueryString + " " + txtVariable1.Text.Trim().ToString();
                //    }
                //    if (txtVariable2.Text != "")
                //    {
                //        GeneratedQueryString = GeneratedQueryString + " " + txtVariable2.Text.Trim().ToString();
                //    }
                //    if (AspxDate1.Value != null)
                //    {
                //        GeneratedQueryString = GeneratedQueryString + " " + AspxDate1.Value.ToString();
                //    }
                //    if (AspxDate2.Value != null)
                //    {
                //        GeneratedQueryString = GeneratedQueryString + " " + AspxDate2.Value.ToString();
                //    }
                //    for (int i = 0; i < DT.Rows.Count; i++)
                //    {

                //        Server.Execute("../management/sms_report.aspx?number=" + DT.Rows[i]["phf_phonenumber"].ToString() + "&subject= " + GeneratedQueryString);

                //    }

                //}
            }
        }



        protected string MailSend(string senderid, string subject, string ContactID)
        {
            string StrOp = "";
            try
            {

                DataTable dt2 = new DataTable();
                dt2.Columns.Add("filepathServer", System.Type.GetType("System.String"));
                string sBody = getemail(senderid, subject, ContactID);
                string str2 = subject;
                string str1 = senderid;
                if (sBody != null && sBody != "a" && sBody != "")
                {

                    if (HttpContext.Current.Session["mail"] != null)
                    {
                        try
                        {
                            HttpContext.Current.Session["mailpath"] = HttpContext.Current.Session["mailpath"].ToString().Remove(HttpContext.Current.Session["mailpath"].ToString().Length - 1, 1);


                            string[] mailpath = HttpContext.Current.Session["mailpath"].ToString().Split('~');

                            for (int g = 0; g < mailpath.Length; g++)
                            {
                                dt2.Rows.Add(mailpath[g]);

                            }
                            oDBEngine.SendattachmentMail(str1, ConfigurationSettings.AppSettings["CredentialUserName"].ToString(), dt2, str2, "", "", "", "");

                            for (int i = 0; i < dt2.Rows.Count; i++)
                            {

                                if (File.Exists(dt2.Rows[i][0].ToString()))
                                {
                                    File.Delete(dt2.Rows[i][0].ToString());
                                }
                            }
                            mail.insert_notification_log(ViewState["topiccode"].ToString(), "R", ViewState["RequestID"].ToString(), ContactID, senderid, "S", ViewState["RequestID"].ToString(), "s");

                            //Response.Write("Send Successfully");
                            StrOp = "Send Successfully";
                        }
                        catch
                        {
                            mail.insert_notification_log(ViewState["topiccode"].ToString(), "R", ViewState["RequestID"].ToString(), ContactID, senderid, "F", ViewState["RequestID"].ToString(), "B");//B-blank
                            // Response.Write("Do not have any value in the segment");
                            StrOp = "Do not have any value in the segment";
                        }
                    }
                    else
                    {
                        c.SendMailHtmlBody(ConfigurationSettings.AppSettings["CredentialUserName"].ToString(), senderid, subject, sBody);
                        mail.insert_notification_log(ViewState["topiccode"].ToString(), "R", ViewState["RequestID"].ToString(), ContactID, senderid, "S", ViewState["RequestID"].ToString(), "s");//s-success
                        StrOp = "Send Successfully";
                        // Response.Write("Send Successfully");
                    }

                }
                else
                {
                    mail.insert_notification_log(ViewState["topiccode"].ToString(), "R", ViewState["RequestID"].ToString(), ContactID, senderid, "F", ViewState["RequestID"].ToString(), "N");//N-no subscription
                    StrOp = "Do not have any value in the segment ";
                    // Response.Write("Do not have any value in the segment ");
                }

            }
            catch
            {

                DataTable dt2 = new DataTable();
                dt2.Columns.Add("filepathServer", System.Type.GetType("System.String"));
                HttpContext.Current.Session["mailpath"] = HttpContext.Current.Session["mailpath"].ToString().Remove(HttpContext.Current.Session["mailpath"].ToString().Length - 1, 1);
                string[] mailpath = HttpContext.Current.Session["mailpath"].ToString().Split('~');

                for (int g = 0; g < mailpath.Length; g++)
                {
                    dt2.Rows.Add(mailpath[g]);

                }
                mail.insert_notification_log(ViewState["topiccode"].ToString(), "R", ViewState["RequestID"].ToString(), ContactID, senderid, "F", ViewState["RequestID"].ToString(), "N");
                StrOp = "   Can not send";
                if (HttpContext.Current.Session["mail"] != null)
                {
                    for (int i = 0; i < dt2.Rows.Count; i++)
                    {

                        if (File.Exists(dt2.Rows[i][0].ToString()))
                        {
                            File.Delete(dt2.Rows[i][0].ToString());
                        }
                    }
                }
            }
            return StrOp;

        }

        private string getemail(string senderid, string subject, string ContactID)
        {
            try
            {
                //  DataSet dt = (DataSet)ViewState["dataset"];
                string contactid = "";
                contactid = ContactID.Trim();
                string s = "";
                switch (ViewState["topiccode"].ToString())
                {

                    case "CLNT000004":
                        try
                        {

                            s = mail.cdslHoldingFrmEmail(contactid, getwords(subject));
                        }
                        catch
                        {
                            s = null;
                        }
                        break;
                    case "CLNT000002":
                        try
                        {

                            s = mail.NSEFrmEmail(contactid, getwords(subject));

                        }
                        catch
                        {
                            s = null;
                        }
                        break;
                    case "CLNT000008":
                        try
                        {

                            s = mail.ledgerFrmEmail(contactid, getwords(subject));

                        }
                        catch
                        {
                            s = null;
                        }
                        break;
                    case "CLNT000019":
                        try
                        {

                            s = mail.cdslbillFrmEmail(contactid, getwords(subject));

                        }
                        catch
                        {
                            s = null;
                        }
                        break;
                    case "CLNT000005":
                        try
                        {

                            s = mail.cdsltransactionFrmEmail(contactid, getwords(subject.ToUpper()));

                        }
                        catch
                        {
                            s = null;
                        }
                        break;
                    //case "CLNT000001":
                    //    try
                    //    {

                    //        s = mail.ContractNoteEmail(contactid, getwords(Request.QueryString["subject"].ToString()));

                    //    }
                    //    catch
                    //    {
                    //        s = null;
                    //    }
                    //    break;

                }

                return s;
            }
            catch
            {

                Response.Write("Can not send subject is invalid");
                return "a";
            }

        }

        public string[] getwords(string sub)
        {
            string str = sub;
            str = str.Trim();
            string[] str1 = str.Split(' ');
            string j = "";
            for (int i = 0; i < str1.Length; i++)
            {
                if (str1[i] != "")
                {
                    j = j + " " + str1[i];
                }
                else
                {
                    j = j;
                }


            }
            j = j.Trim();
            str1 = j.Split(' ');

            return str1;
        }

    }

}