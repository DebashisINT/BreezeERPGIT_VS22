using System;
using System.Data;
//////using DevExpress.Web;
using DevExpress.Web;
using System.Configuration;
using BusinessLogicLayer;
using System.Web;

namespace ERP.OMS.Management.Master
{
    public partial class management_master_Employee_Subscription : ERP.OMS.ViewState_class.VSPage //System.Web.UI.Page
    {
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        Converter ObjConvert = new Converter();
        string TopicCode = "";
        string TableName = "";
        string Notification = "";
        string DeliveryGlobal = "";
        string FrequncyGlobal = "";
        string AddUpdate = "";
        string ModeGlobal = "";
        string ChargeableGlobal = "";
        string GobalMessage = "";
        static string Description;
        string MessageVisible = "";
        string acccode = "";
        protected void Page_Load(object sender, EventArgs e)
        {

            //------- For Read Only User in SQL Datasource Connection String   Start-----------------

            if (HttpContext.Current.Session["EntryProfileType"] != null)
            {
                if (Convert.ToString(HttpContext.Current.Session["EntryProfileType"]) == "R")
                {
                    SqlSubscriptions.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                }
                else
                {
                    SqlSubscriptions.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                }
            }

            //------- For Read Only User in SQL Datasource Connection String   End-----------------

            if (IsPostBack != true)
            {
                string EmpId = Session["KeyVal_InternalID"].ToString();
            }

        }
        protected void Page_Init(object sender, EventArgs e)
        {
            sqlEmail.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            sqlPhone.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            sqlNotification.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            sqlFrequency.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            sqlMode.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
        }
        protected void dpMode_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            ASPxComboBox combo1 = (ASPxComboBox)gridSubscription.FindEditFormTemplateControl("dpDescription");
            TopicCode = combo1.Value.ToString();
            string Mode = oDBEngine.GetFieldValue("master_topics", "topics_deliverymode", "topics_code='" + TopicCode + "'", 1)[0, 0];
            ASPxComboBox combo2 = (ASPxComboBox)gridSubscription.FindEditFormTemplateControl("dpMode");
            TableName = "dbo.fnSplitReturnTable(" + "'" + Mode + "'" + ",',')";
            sqlMode.SelectCommand = "select * ,(select top 1 case Topics_DeliveryMode when 'E' then 'Email' when 'M' then 'Message' when 'B' then 'Email & SMS' when 'R' then 'Reminder' else 'SMS' end from Master_Topics  where Topics_DeliveryMode=col) as Descriptions from " + TableName;
            combo2.DataBind();
            DataTable DTMode = new DataTable();
            DTMode = oDBEngine.GetDataTable("Master_Topics", "* ,(select top 1 case Topics_DeliveryMode when 'E' then 'Email' when 'M' then 'Message' when 'B' then 'Email & SMS' when 'R' then 'Reminder' else 'SMS' end", "Topics_DeliveryMode=col) as Descriptions from " + TableName);
            if (DTMode.Rows.Count == 2)
            {
                combo2.Items.Insert(0, new ListEditItem("Both", "B"));
                combo2.SelectedIndex = 0;
            }
        }

        protected void dpFrequency_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            ASPxComboBox combo1 = (ASPxComboBox)gridSubscription.FindEditFormTemplateControl("dpDescription");
            TopicCode = combo1.Value.ToString();
            string Frequency = oDBEngine.GetFieldValue("master_topics", "topics_deliveryfrequency", "topics_code='" + TopicCode + "'", 1)[0, 0];
            ASPxComboBox combo3 = (ASPxComboBox)gridSubscription.FindEditFormTemplateControl("dpFrequency");
            TableName = "dbo.fnSplitReturnTable(" + "'" + Frequency + "'" + ",',')";
            sqlFrequency.SelectCommand = "select * ,(select top 1 case topics_deliveryfrequency when 'E' then 'Upon Event' when 'D' then 'Daily' when 'F' then 'Fort Nightly' when 'M' then 'Monthly'  when 'Q' then 'Quaterly' when 'W' then 'Weekly' else 'Yearly' end from Master_Topics  where topics_deliveryfrequency=col) as Frequency from " + TableName;
            combo3.DataBind();
            ASPxTextBox txt = (ASPxTextBox)gridSubscription.FindEditFormTemplateControl("txtAccID");
            acccode = oDBEngine.GetFieldValue("master_topics", "topics_accesscode", " topics_code='" + TopicCode + "'", 1)[0, 0];

        }
        protected void dpNotification_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            ASPxComboBox combo1 = (ASPxComboBox)gridSubscription.FindEditFormTemplateControl("dpDescription");
            TopicCode = combo1.Value.ToString();
            string NotificationMethod = oDBEngine.GetFieldValue("master_topics", "topics_notificationmethod", "topics_code='" + TopicCode + "'", 1)[0, 0];
            ASPxComboBox combo4 = (ASPxComboBox)gridSubscription.FindEditFormTemplateControl("dpNotification");
            TableName = "dbo.fnSplitReturnTable(" + "'" + NotificationMethod + "'" + ",',')";
            sqlNotification.SelectCommand = "select * ,(select top 1 case topics_notificationmethod when 'A' then 'Auto'  else 'On Request' end from Master_Topics  where topics_notificationmethod=col) as Notification from " + TableName;
            combo4.DataBind();
            DataTable DTNotification = new DataTable();
            DTNotification = oDBEngine.GetDataTable("Master_Topics", "* ,(select top 1 case topics_notificationmethod when 'A' then 'Auto'  else 'On Request' end", "topics_notificationmethod=col) as Notification from " + TableName);
            if (DTNotification.Rows.Count == 2)
            {
                combo4.Items.Insert(0, new ListEditItem("Both", "B"));
                combo4.SelectedIndex = 0;

            }
        }
        protected void gridSubscription_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            ASPxComboBox cmbDescription = (ASPxComboBox)gridSubscription.FindEditFormTemplateControl("dpDescription");
            ASPxComboBox cmbNotification = (ASPxComboBox)gridSubscription.FindEditFormTemplateControl("dpNotification");
            ASPxComboBox cmbMode = (ASPxComboBox)gridSubscription.FindEditFormTemplateControl("dpMode");
            ASPxComboBox cmbFrequency = (ASPxComboBox)gridSubscription.FindEditFormTemplateControl("dpFrequency");
            ASPxComboBox cmbPhone = (ASPxComboBox)gridSubscription.FindEditFormTemplateControl("dpPhoneNo");
            ASPxComboBox cmbEmail = (ASPxComboBox)gridSubscription.FindEditFormTemplateControl("dpEmail");
            ASPxDateEdit DStartDate = (ASPxDateEdit)gridSubscription.FindEditFormTemplateControl("ASPxStartDate");
            ASPxDateEdit DEndDate = (ASPxDateEdit)gridSubscription.FindEditFormTemplateControl("ASPxDateTo");
            ASPxComboBox cmbChargable = (ASPxComboBox)gridSubscription.FindEditFormTemplateControl("dpchargeable");
            ASPxTextBox txtFreeMessageControl = (ASPxTextBox)gridSubscription.FindEditFormTemplateControl("txtFreeMessage");


            if (cmbDescription.Value.ToString() == "Select")
            {
                GobalMessage = "Select";
            }
            if (DStartDate.Value == null)
            {
                GobalMessage = "Blank";
            }
            else if (GobalMessage != "Select" || GobalMessage != "Blank")
            {
                string DataCheckBeforeInsert = oDBEngine.GetFieldValue("Trans_TopicSubscriptions", "TopicSubscriptions_ContactID", "TopicSubscriptions_ContactID='" + Convert.ToString(Session["KeyVal_InternalID"]) + "' and topicsubscriptions_topiccode='" + Convert.ToString(cmbDescription.Value) + "' and topicsubscriptions_deliverymode='" + Convert.ToString(cmbMode.Value) + "'", 1)[0, 0];
                if (DataCheckBeforeInsert != "n")
                {
                    GobalMessage = "Duplicate";
                }
                else
                {
                    if (Convert.ToString(cmbMode.Value) == "E")
                    {
                        oDBEngine.InsurtFieldValue("Trans_TopicSubscriptions", "TopicSubscriptions_TopicCode,TopicSubscriptions_ContactID,TopicSubscriptions_NotificationMethod,TopicSubscriptions_DeliveryMode,TopicSubscriptions_DeliveryFrequency,TopicSubscriptions_EmailID,TopicSubscriptions_StartDate,TopicSubscriptions_EndDate,TopicSubscriptions_Chargeable,TopicSubscriptions_FreeMessages,TopicSubscriptions_CreateUser,TopicSubscriptions_CreateDateTime", "'" + Convert.ToString(cmbDescription.Value) + "','" + Convert.ToString(Session["KeyVal_InternalID"]) + "','" + Convert.ToString(cmbNotification.Value) + "','" + Convert.ToString(cmbMode.Value) + "','" + Convert.ToString(cmbFrequency.Value) + "','" + Convert.ToString(cmbEmail.Value) + "','" + DStartDate.Value + "','" + DEndDate.Value + "','" + cmbChargable.Value + "','" + Convert.ToString(txtFreeMessageControl.Text) + "','" + Convert.ToString(Session["userid"]) + "','" + Convert.ToDateTime(Convert.ToString(oDBEngine.GetDate())) + "'");

                    }
                    else if (Convert.ToString(cmbMode.Value) == "S")
                    {
                        oDBEngine.InsurtFieldValue("Trans_TopicSubscriptions", "TopicSubscriptions_TopicCode,TopicSubscriptions_ContactID,TopicSubscriptions_NotificationMethod,TopicSubscriptions_DeliveryMode,TopicSubscriptions_DeliveryFrequency,TopicSubscriptions_PhoneID,TopicSubscriptions_StartDate,TopicSubscriptions_EndDate,TopicSubscriptions_Chargeable,TopicSubscriptions_FreeMessages,TopicSubscriptions_CreateUser,TopicSubscriptions_CreateDateTime", "'" + Convert.ToString(cmbDescription.Value) + "','" + Convert.ToString(Session["KeyVal_InternalID"]) + "','" + Convert.ToString(cmbNotification.Value) + "','" + Convert.ToString(cmbMode.Value) + "','" + Convert.ToString(cmbFrequency.Value) + "','" + Convert.ToString(cmbPhone.Value) + "','" + DStartDate.Value + "','" + DEndDate.Value + "','" + cmbChargable.Value + "','" + Convert.ToString(txtFreeMessageControl.Text)+ "','" + Convert.ToString(Session["userid"]) + "','" + Convert.ToDateTime(Convert.ToString(oDBEngine.GetDate())) + "'");

                    }
                    else
                    {
                        oDBEngine.InsurtFieldValue("Trans_TopicSubscriptions", "TopicSubscriptions_TopicCode,TopicSubscriptions_ContactID,TopicSubscriptions_NotificationMethod,TopicSubscriptions_DeliveryMode,TopicSubscriptions_DeliveryFrequency,TopicSubscriptions_PhoneID,TopicSubscriptions_EmailID,TopicSubscriptions_StartDate,TopicSubscriptions_EndDate,TopicSubscriptions_Chargeable,TopicSubscriptions_FreeMessages,TopicSubscriptions_CreateUser,TopicSubscriptions_CreateDateTime", "'" + Convert.ToString(cmbDescription.Value) + "','" + Convert.ToString(Session["KeyVal_InternalID"]) + "','" + Convert.ToString(cmbNotification.Value) + "','" + Convert.ToString(cmbMode.Value) + "','" + Convert.ToString(cmbFrequency.Value) + "','" + Convert.ToString(cmbPhone.Value) + "','" + Convert.ToString(cmbEmail.Value) + "','" + DStartDate.Value + "','" + DEndDate.Value + "','" + cmbChargable.Value + "','" + Convert.ToString(txtFreeMessageControl.Text) + "','" + Convert.ToString(Session["userid"]) + "','" + Convert.ToDateTime(Convert.ToString(oDBEngine.GetDate())) + "'");
                    }
                    GobalMessage = "";
                }
            }
            //e.Cancel = true;
        }



        protected void dpPhoneNo_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            ASPxComboBox cmbMode = (ASPxComboBox)gridSubscription.FindEditFormTemplateControl("dpMode");
            ASPxComboBox cmbPhone = (ASPxComboBox)gridSubscription.FindEditFormTemplateControl("dpPhoneNo");
            ASPxComboBox cmbEmail = (ASPxComboBox)gridSubscription.FindEditFormTemplateControl("dpEmail");
            sqlPhone.SelectCommand = "select phf_id as col, phf_phonenumber as phone from tbl_master_phonefax where phf_type='Mobile' and phf_cntid='" + Session["KeyVal_InternalID"] + "'";
            cmbPhone.DataBind();

        }
        protected void dpEmail_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            ASPxComboBox cmbEmail = (ASPxComboBox)gridSubscription.FindEditFormTemplateControl("dpEmail");
            sqlEmail.SelectCommand = "select eml_id as col,eml_email as email from tbl_master_email where eml_cntid='" + Session["KeyVal_InternalID"] + "' and eml_email is not null ";
            cmbEmail.DataBind();
        }


        protected void gridSubscription_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            string id = e.Keys[0].ToString();
            ASPxComboBox cmbDescription = (ASPxComboBox)gridSubscription.FindEditFormTemplateControl("dpDescription");
            ASPxComboBox cmbNotification = (ASPxComboBox)gridSubscription.FindEditFormTemplateControl("dpNotification");
            ASPxComboBox cmbMode = (ASPxComboBox)gridSubscription.FindEditFormTemplateControl("dpMode");
            ASPxComboBox cmbFrequency = (ASPxComboBox)gridSubscription.FindEditFormTemplateControl("dpFrequency");
            ASPxComboBox cmbPhone = (ASPxComboBox)gridSubscription.FindEditFormTemplateControl("dpPhoneNo");
            ASPxComboBox cmbEmail = (ASPxComboBox)gridSubscription.FindEditFormTemplateControl("dpEmail");
            ASPxDateEdit DStartDate = (ASPxDateEdit)gridSubscription.FindEditFormTemplateControl("ASPxStartDate");
            ASPxDateEdit DEndDate = (ASPxDateEdit)gridSubscription.FindEditFormTemplateControl("ASPxDateTo");
            ASPxComboBox cmbChargable = (ASPxComboBox)gridSubscription.FindEditFormTemplateControl("dpchargeable");
            ASPxTextBox txtFreeMessageControl = (ASPxTextBox)gridSubscription.FindEditFormTemplateControl("txtFreeMessage");
            TopicCode = cmbDescription.Value.ToString();
            string NotificationMethod = oDBEngine.GetFieldValue("master_topics", "topics_notificationmethod", "topics_code='" + TopicCode + "'", 1)[0, 0];
            TableName = "dbo.fnSplitReturnTable(" + "'" + NotificationMethod + "'" + ",',')";
            sqlNotification.SelectCommand = "select * ,(select top 1 case topics_notificationmethod when 'A' then 'Auto'  else 'On Request' end from Master_Topics  where topics_notificationmethod=col) as Notification from " + TableName;
            cmbNotification.DataBind();
            //BIND FREQUENCY
            string Frequency = oDBEngine.GetFieldValue("master_topics", "topics_deliveryfrequency", "topics_code='" + TopicCode + "'", 1)[0, 0];
            TableName = "dbo.fnSplitReturnTable(" + "'" + Frequency + "'" + ",',')";
            sqlFrequency.SelectCommand = "select * ,(select top 1 case topics_deliveryfrequency when 'E' then 'Upon Event' when 'D' then 'Daily' when 'F' then 'Fort Nightly' when 'M' then 'Monthly'  when 'Q' then 'Quaterly' when 'W' then 'Weekly' else 'Yearly' end from Master_Topics  where topics_deliveryfrequency=col) as Frequency from " + TableName;
            cmbFrequency.DataBind();
            //BIND DELIVERY MODE
            string Mode = oDBEngine.GetFieldValue("master_topics", "topics_deliverymode", "topics_code='" + TopicCode + "'", 1)[0, 0];
            TableName = "dbo.fnSplitReturnTable(" + "'" + Mode + "'" + ",',')";
            sqlMode.SelectCommand = "select * ,(select top 1 case Topics_DeliveryMode when 'E' then 'Email' when 'M' then 'Message' when 'B' then 'Email & SMS' when 'R' then 'Reminder' else 'SMS' end from Master_Topics  where Topics_DeliveryMode=col) as Descriptions from " + TableName;
            cmbMode.DataBind();
            //BIND PHONE NUMBER
            sqlPhone.SelectCommand = "select phf_id as col, phf_phonenumber as phone from tbl_master_phonefax where phf_type='Mobile' and phf_cntid='" + Session["KeyVal_InternalID"] + "'";
            cmbPhone.DataBind();
            //BIND EMAIL ID
            sqlEmail.SelectCommand = "select eml_id as col,eml_email as email from tbl_master_email where eml_cntid='" + Session["KeyVal_InternalID"] + "' and eml_email is not null ";
            cmbEmail.DataBind();

            if (cmbMode.Value.ToString() == "E")
            {
                oDBEngine.SetFieldValue("Trans_TopicSubscriptions", "TopicSubscriptions_NotificationMethod='" + cmbNotification.Value + "',TopicSubscriptions_DeliveryMode='" + cmbMode.Value + "',TopicSubscriptions_DeliveryFrequency='" + cmbFrequency.Value + "',TopicSubscriptions_EmailID='" + cmbEmail.Value + "',TopicSubscriptions_PhoneID=null,TopicSubscriptions_StartDate='" + DStartDate.Value + "',TopicSubscriptions_EndDate='" + DEndDate.Value + "',TopicSubscriptions_Chargeable='" + cmbChargable.Value + "',TopicSubscriptions_FreeMessages='" + txtFreeMessageControl.Text.ToString() + "',TopicSubscriptions_ModifyUser='" + Session["userid"].ToString() + "',TopicSubscriptions_ModifyDateTime='" + Convert.ToDateTime(oDBEngine.GetDate().ToString()) + "'", "TopicSubscriptions_ContactID='" + Session["KeyVal_InternalID"].ToString() + "' and TopicSubscriptions_TopicCode='" + cmbDescription.Value.ToString() + "' and topicsubscriptions_id='" + id.ToString().Trim() + "' ");
            }
            else if (cmbMode.Value.ToString() == "S")
            {
                oDBEngine.SetFieldValue("Trans_TopicSubscriptions", "TopicSubscriptions_NotificationMethod='" + cmbNotification.Value + "',TopicSubscriptions_DeliveryMode='" + cmbMode.Value + "',TopicSubscriptions_DeliveryFrequency='" + cmbFrequency.Value + "',TopicSubscriptions_PhoneID='" + cmbPhone.Value + "',topicsubscriptions_emailid=null,TopicSubscriptions_StartDate='" + DStartDate.Value + "',TopicSubscriptions_EndDate='" + DEndDate.Value + "',TopicSubscriptions_Chargeable='" + cmbChargable.Value + "',TopicSubscriptions_FreeMessages='" + txtFreeMessageControl.Text.ToString() + "',TopicSubscriptions_ModifyUser='" + Session["userid"].ToString() + "',TopicSubscriptions_ModifyDateTime='" + Convert.ToDateTime(oDBEngine.GetDate().ToString()) + "'", "TopicSubscriptions_ContactID='" + Session["KeyVal_InternalID"].ToString() + "' and TopicSubscriptions_TopicCode='" + cmbDescription.Value.ToString() + "' and topicsubscriptions_id='" + id.ToString().Trim() + "'");
            }
            else
            {
                string a = cmbNotification.Value.ToString();
                string b = cmbNotification.Text.ToString();
                string c = cmbMode.Value.ToString();
                if (b == "Both")
                {
                    cmbNotification.Value = "B";
                }
                if (c == "Both")
                {
                    cmbMode.Value = "B";
                }
                oDBEngine.SetFieldValue("Trans_TopicSubscriptions", "TopicSubscriptions_NotificationMethod='" + cmbNotification.Value + "',TopicSubscriptions_DeliveryMode='" + cmbMode.Value + "',TopicSubscriptions_DeliveryFrequency='" + cmbFrequency.Value + "',TopicSubscriptions_PhoneID='" + cmbPhone.Value + "',TopicSubscriptions_EmailID='" + cmbEmail.Value + "',TopicSubscriptions_StartDate='" + DStartDate.Value + "',TopicSubscriptions_EndDate='" + DEndDate.Value + "',TopicSubscriptions_Chargeable='" + cmbChargable.Value + "',TopicSubscriptions_FreeMessages='" + txtFreeMessageControl.Text.ToString() + "',TopicSubscriptions_ModifyUser='" + Session["userid"].ToString() + "',TopicSubscriptions_ModifyDateTime='" + Convert.ToDateTime(oDBEngine.GetDate().ToString()) + "'", "TopicSubscriptions_ContactID='" + Session["KeyVal_InternalID"].ToString() + "' and TopicSubscriptions_TopicCode='" + cmbDescription.Value.ToString() + "' and topicsubscriptions_id='" + id.ToString().Trim() + "'");

            }
            //e.Cancel = true;
        }
        protected void gridSubscription_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            GobalMessage = "";
            ASPxComboBox cmbDescription = (ASPxComboBox)gridSubscription.FindEditFormTemplateControl("dpDescription");
            ASPxComboBox cmbNotification = (ASPxComboBox)gridSubscription.FindEditFormTemplateControl("dpNotification");
            ASPxComboBox cmbMode = (ASPxComboBox)gridSubscription.FindEditFormTemplateControl("dpMode");
            ASPxComboBox cmbFrequency = (ASPxComboBox)gridSubscription.FindEditFormTemplateControl("dpFrequency");
            ASPxComboBox cmbPhone = (ASPxComboBox)gridSubscription.FindEditFormTemplateControl("dpPhoneNo");
            ASPxComboBox cmbEmail = (ASPxComboBox)gridSubscription.FindEditFormTemplateControl("dpEmail");
            ASPxDateEdit DStartDate = (ASPxDateEdit)gridSubscription.FindEditFormTemplateControl("ASPxStartDate");
            ASPxDateEdit DEndDate = (ASPxDateEdit)gridSubscription.FindEditFormTemplateControl("ASPxDateTo");
            ASPxComboBox cmbChargable = (ASPxComboBox)gridSubscription.FindEditFormTemplateControl("dpchargeable");
            ASPxTextBox txtFreeMessageControl = (ASPxTextBox)gridSubscription.FindEditFormTemplateControl("txtFreeMessage");
            ASPxTextBox txt = (ASPxTextBox)gridSubscription.FindEditFormTemplateControl("txtAccID");
            DataTable DT = new DataTable();
            TopicCode = e.EditingKeyValue.ToString();
            DT = oDBEngine.GetDataTable("Trans_TopicSubscriptions", "TopicSubscriptions_TopicCode,TopicSubscriptions_ContactID,TopicSubscriptions_NotificationMethod,TopicSubscriptions_DeliveryMode,TopicSubscriptions_DeliveryFrequency,TopicSubscriptions_PhoneID,TopicSubscriptions_EmailID,TopicSubscriptions_StartDate,TopicSubscriptions_EndDate,TopicSubscriptions_Chargeable,TopicSubscriptions_FreeMessages", "TopicSubscriptions_ContactID='" + Session["KeyVal_InternalID"].ToString() + "' and TopicSubscriptions_ID='" + TopicCode.ToString() + "'");
            cmbDescription.Value = DT.Rows[0]["TopicSubscriptions_TopicCode"].ToString();
            string Description = "";
            Description = oDBEngine.GetFieldValue("master_topics", "topics_description", "topics_code='" + DT.Rows[0]["TopicSubscriptions_TopicCode"].ToString() + "'", 1)[0, 0];
            cmbDescription.Text = Description.ToString();
            cmbMode.Value = DT.Rows[0]["TopicSubscriptions_DeliveryMode"].ToString();
            ModeGlobal = DT.Rows[0]["TopicSubscriptions_DeliveryMode"].ToString();
            cmbFrequency.Value = DT.Rows[0]["TopicSubscriptions_DeliveryFrequency"].ToString();
            cmbPhone.Value = DT.Rows[0]["TopicSubscriptions_PhoneID"].ToString();
            string PhoneNumber = "";
            if (cmbPhone.Value != null)
            {
                PhoneNumber = oDBEngine.GetFieldValue("tbl_master_phonefax", "phf_phonenumber", "phf_id='" + DT.Rows[0]["TopicSubscriptions_PhoneID"].ToString() + "'", 1)[0, 0];
                cmbPhone.Text = PhoneNumber.ToString();
            }

            cmbEmail.Value = DT.Rows[0]["TopicSubscriptions_EmailID"].ToString();
            string MailId = "";
            if (cmbEmail.Value != null)
            {
                MailId = oDBEngine.GetFieldValue("tbl_master_email", "eml_email", "eml_id='" + DT.Rows[0]["TopicSubscriptions_EmailID"].ToString() + "'", 1)[0, 0];
                cmbEmail.Text = MailId.ToString();
            }

            DStartDate.Value = Convert.ToDateTime(DT.Rows[0]["TopicSubscriptions_StartDate"].ToString());
            if (DT.Rows[0]["TopicSubscriptions_EndDate"].ToString() != "1/1/1900 12:00:00 AM")
            {
                DEndDate.Value = Convert.ToDateTime(DT.Rows[0]["TopicSubscriptions_EndDate"].ToString());
            }
            cmbChargable.Value = DT.Rows[0]["TopicSubscriptions_Chargeable"].ToString();
            ChargeableGlobal = DT.Rows[0]["TopicSubscriptions_Chargeable"].ToString();
            txtFreeMessageControl.Text = DT.Rows[0]["TopicSubscriptions_FreeMessages"].ToString();
            if (DT.Rows[0]["TopicSubscriptions_Chargeable"].ToString() == "N")
            {
                MessageVisible = "Visible";
            }
            else if (DT.Rows[0]["TopicSubscriptions_Chargeable"].ToString() == "Y")
            {
                MessageVisible = "Y";
            }
            //Bind Notification
            TopicCode = cmbDescription.Value.ToString();
            string NotificationMethod = oDBEngine.GetFieldValue("master_topics", "topics_notificationmethod", "topics_code='" + TopicCode + "'", 1)[0, 0];
            TableName = "dbo.fnSplitReturnTable(" + "'" + NotificationMethod + "'" + ",',')";
            sqlNotification.SelectCommand = "select * ,(select top 1 case topics_notificationmethod when 'A' then 'Auto'  else 'On Request' end from Master_Topics  where topics_notificationmethod=col) as Notification from " + TableName;
            cmbNotification.DataBind();
            cmbNotification.Value = DT.Rows[0]["TopicSubscriptions_NotificationMethod"].ToString();
            DataTable DTNotification = new DataTable();
            DTNotification = oDBEngine.GetDataTable("Master_Topics", "* ,(select top 1 case topics_notificationmethod when 'A' then 'Auto'  else 'On Request' end", "topics_notificationmethod=col) as Notification from " + TableName);
            if (DTNotification.Rows.Count == 2)
            {
                cmbNotification.Items.Insert(0, new ListEditItem("Both", "B"));
                //cmbNotification.SelectedIndex = 0;

            }
            //BIND FREQUENCY
            string Frequency = oDBEngine.GetFieldValue("master_topics", "topics_deliveryfrequency", "topics_code='" + TopicCode + "'", 1)[0, 0];
            TableName = "dbo.fnSplitReturnTable(" + "'" + Frequency + "'" + ",',')";
            sqlFrequency.SelectCommand = "select * ,(select top 1 case topics_deliveryfrequency when 'E' then 'Upon Event' when 'D' then 'Daily' when 'F' then 'Fort Nightly' when 'M' then 'Monthly'  when 'Q' then 'Quaterly' when 'W' then 'Weekly' else 'Yearly' end from Master_Topics  where topics_deliveryfrequency=col) as Frequency from " + TableName;
            cmbFrequency.DataBind();
            //BIND DELIVERY MODE
            string Mode = oDBEngine.GetFieldValue("master_topics", "topics_deliverymode", "topics_code='" + TopicCode + "'", 1)[0, 0];
            TableName = "dbo.fnSplitReturnTable(" + "'" + Mode + "'" + ",',')";
            sqlMode.SelectCommand = "select * ,(select top 1 case Topics_DeliveryMode when 'E' then 'Email' when 'M' then 'Message' when 'B' then 'Email & SMS' when 'R' then 'Reminder' else 'SMS' end from Master_Topics  where Topics_DeliveryMode=col) as Descriptions from " + TableName;
            cmbMode.DataBind();
            DataTable DTMode = new DataTable();
            DTMode = oDBEngine.GetDataTable("Master_Topics", "* ,(select top 1 case Topics_DeliveryMode when 'E' then 'Email' when 'M' then 'Message' when 'B' then 'Email & SMS' when 'R' then 'Reminder' else 'SMS' end", "Topics_DeliveryMode=col) as Descriptions from " + TableName);
            if (DTMode.Rows.Count == 2)
            {
                cmbMode.Items.Insert(0, new ListEditItem("Both", "B"));
                //cmbMode.SelectedIndex = 0;
            }
            //BIND PHONE NUMBER
            sqlPhone.SelectCommand = "select phf_id as col, phf_phonenumber as phone from tbl_master_phonefax where phf_type='Mobile' and phf_cntid='" + Session["KeyVal_InternalID"] + "'";
            cmbPhone.DataBind();
            //BIND EMAIL ID
            sqlEmail.SelectCommand = "select eml_id as col,eml_email as email from tbl_master_email where eml_cntid='" + Session["KeyVal_InternalID"] + "' and eml_email is not null ";
            cmbEmail.DataBind();
            string acccode = oDBEngine.GetFieldValue("master_topics", "topics_accesscode", " topics_code='" + TopicCode + "'", 1)[0, 0];
            txt.Text = acccode.ToString();
            AddUpdate = "UPDATE";
        }

        protected void gridSubscription_CustomJSProperties1(object sender, DevExpress.Web.ASPxGridViewClientJSPropertiesEventArgs e)
        {
            if (AddUpdate != "" && ModeGlobal != "")
            {
                if (GobalMessage == "")
                {
                    GobalMessage = "a";
                }
                e.Properties["cpValue"] = AddUpdate + "~" + ModeGlobal + "~" + ChargeableGlobal + "~" + GobalMessage;
            }
            else if (GobalMessage == "Duplicate" || GobalMessage == "Blank" || GobalMessage == "Select")
            {
                e.Properties["cpValue"] = "INSERT ~ b ~ c" + "~" + GobalMessage;
            }
            else
            {
                e.Properties["cpValue"] = "a ~ b ~ c ~ d";
            }
            if (MessageVisible == "N")
            {
                e.Properties["cpValue"] = "visible";
            }
        }


        protected void dpMode_CustomJSProperties(object sender, DevExpress.Web.CustomJSPropertiesEventArgs e)
        {
            if (AddUpdate != "UPDATE")
            {
                AddUpdate = "INSERT";
            }
            e.Properties["cpValueCombo"] = AddUpdate + "~" + ModeGlobal;
        }
        protected void dpchargeable_CustomJSProperties(object sender, DevExpress.Web.CustomJSPropertiesEventArgs e)
        {
            e.Properties["cpa"] = MessageVisible;
        }
        protected void gridSubscription_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            TopicCode = e.Keys[0].ToString();
            DataTable DT = new DataTable();
            DT = oDBEngine.GetDataTable("Trans_TopicSubscriptions", "TopicSubscriptions_TopicCode,TopicSubscriptions_ContactID,TopicSubscriptions_NotificationMethod,TopicSubscriptions_DeliveryMode,TopicSubscriptions_DeliveryFrequency,TopicSubscriptions_PhoneID,TopicSubscriptions_EmailID,TopicSubscriptions_StartDate,TopicSubscriptions_EndDate,TopicSubscriptions_Chargeable,TopicSubscriptions_FreeMessages,TopicSubscriptions_CreateUser,TopicSubscriptions_CreateDateTime", "TopicSubscriptions_ContactID='" + Session["KeyVal_InternalID"].ToString() + "' and TopicSubscriptions_ID='" + TopicCode.ToString() + "'");
            oDBEngine.InsurtFieldValue("Trans_TopicSubscriptions_log", "TopicSubscriptions_TopicCode,TopicSubscriptions_ContactID,TopicSubscriptions_NotificationMethod,TopicSubscriptions_DeliveryMode,TopicSubscriptions_DeliveryFrequency,TopicSubscriptions_PhoneID,TopicSubscriptions_EmailID,TopicSubscriptions_StartDate,TopicSubscriptions_EndDate,TopicSubscriptions_Chargeable,TopicSubscriptions_FreeMessages,TopicSubscriptions_CreateUser,TopicSubscriptions_CreateDateTime,status,delete_user,delete_time", "'" + DT.Rows[0]["TopicSubscriptions_TopicCode"].ToString() + "','" + DT.Rows[0]["TopicSubscriptions_ContactId"] + "','" + DT.Rows[0]["TopicSubscriptions_NotificationMethod"] + "','" + DT.Rows[0]["TopicSubscriptions_DeliveryMode"] + "','" + DT.Rows[0]["TopicSubscriptions_DeliveryFrequency"] + "','" + DT.Rows[0]["TopicSubscriptions_PhoneID"] + "','" + DT.Rows[0]["TopicSubscriptions_EmailID"] + "','" + DT.Rows[0]["TopicSubscriptions_StartDate"] + "','" + DT.Rows[0]["TopicSubscriptions_EndDate"] + "','" + DT.Rows[0]["TopicSubscriptions_Chargeable"] + "','" + DT.Rows[0]["TopicSubscriptions_FreeMessages"] + "','" + DT.Rows[0]["TopicSubscriptions_CreateUser"] + "','" + DT.Rows[0]["TopicSubscriptions_CreateDateTime"] + "','D','" + Session["userid"].ToString() + "','" + Convert.ToDateTime(oDBEngine.GetDate().ToString()) + "'");
            oDBEngine.DeleteValue("Trans_TopicSubscriptions", " topicsubscriptions_id='" + TopicCode.ToString() + "'");
            e.Cancel = true;
        }


        protected void gridSubscription_HtmlEditFormCreated(object sender, DevExpress.Web.ASPxGridViewEditFormEventArgs e)
        {
            if (Description == null)
            {
                ASPxComboBox cmbDescription = (ASPxComboBox)gridSubscription.FindEditFormTemplateControl("dpDescription");
                cmbDescription.Items.Insert(0, new ListEditItem("Select", "S"));
                cmbDescription.SelectedIndex = 0;
                Description = "a";
                ASPxComboBox cmbChargable = (ASPxComboBox)gridSubscription.FindEditFormTemplateControl("dpchargeable");
                if (cmbChargable.Value == "N")
                {
                    //ASPxTextBox txt = (ASPxTextBox)gridSubscription.FindEditFormTemplateControl("txtFreeMessage");
                    //txt.ClientVisible = false;
                    MessageVisible = "N";
                    //Label lbl = (Label)gridSubscription.FindEditFormTemplateControl("Label10");
                    //lbl.cliVisible = false;
                }

            }
            //cmbChargable.Value = "S";
        }
        protected void gridSubscription_Init(object sender, EventArgs e)
        {

        }
        protected void gridSubscription_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            Description = null;
        }
        protected void gridSubscription_RowValidating(object sender, DevExpress.Web.Data.ASPxDataValidationEventArgs e)
        {
            ASPxComboBox cmbDescription = (ASPxComboBox)gridSubscription.FindEditFormTemplateControl("dpDescription");
            ASPxComboBox cmbNotification = (ASPxComboBox)gridSubscription.FindEditFormTemplateControl("dpNotification");
            ASPxComboBox cmbMode = (ASPxComboBox)gridSubscription.FindEditFormTemplateControl("dpMode");
            ASPxComboBox cmbFrequency = (ASPxComboBox)gridSubscription.FindEditFormTemplateControl("dpFrequency");
            ASPxComboBox cmbPhone = (ASPxComboBox)gridSubscription.FindEditFormTemplateControl("dpPhoneNo");
            ASPxComboBox cmbEmail = (ASPxComboBox)gridSubscription.FindEditFormTemplateControl("dpEmail");
            ASPxDateEdit DStartDate = (ASPxDateEdit)gridSubscription.FindEditFormTemplateControl("ASPxStartDate");
            ASPxDateEdit DEndDate = (ASPxDateEdit)gridSubscription.FindEditFormTemplateControl("ASPxDateTo");
            ASPxComboBox cmbChargable = (ASPxComboBox)gridSubscription.FindEditFormTemplateControl("dpchargeable");
            ASPxTextBox txtFreeMessageControl = (ASPxTextBox)gridSubscription.FindEditFormTemplateControl("txtFreeMessage");
            ASPxTextBox txt = (ASPxTextBox)gridSubscription.FindEditFormTemplateControl("txtAccID");

            if (cmbDescription.Value.ToString() == "Select")
            {
                e.RowError = "Please select a Topic";
            }
            if (gridSubscription.IsNewRowEditing == true)
            {
                string DataCheckBeforeInsert = oDBEngine.GetFieldValue("Trans_TopicSubscriptions", "TopicSubscriptions_ContactID", "TopicSubscriptions_ContactID='" + Session["KeyVal_InternalID"].ToString() + "' and topicsubscriptions_topiccode='" + cmbDescription.Value.ToString() + "'", 1)[0, 0];
                if (DataCheckBeforeInsert != "n")
                {
                    GobalMessage = "Duplicate";
                    e.RowError = "Duplicate Record Found";

                }
            }
            if (DStartDate.Value == null)
            {
                e.RowError = "Start date Can not blank";
            }
            //Bind Notification
            TopicCode = cmbDescription.Value.ToString();
            string NotificationMethod = oDBEngine.GetFieldValue("master_topics", "topics_notificationmethod", "topics_code='" + TopicCode + "'", 1)[0, 0];
            TableName = "dbo.fnSplitReturnTable(" + "'" + NotificationMethod + "'" + ",',')";
            sqlNotification.SelectCommand = "select * ,(select top 1 case topics_notificationmethod when 'A' then 'Auto'  else 'On Request' end from Master_Topics  where topics_notificationmethod=col) as Notification from " + TableName;
            cmbNotification.DataBind();
            DataTable DTNotification = new DataTable();//DataTable is used for checking wheather there is a two value
            DTNotification = oDBEngine.GetDataTable("Master_Topics", "* ,(select top 1 case topics_notificationmethod when 'A' then 'Auto'  else 'On Request' end", "topics_notificationmethod=col) as Notification from " + TableName);
            if (DTNotification.Rows.Count == 2)
            {
                cmbNotification.Items.Insert(0, new ListEditItem("Both", "B"));
                //cmbNotification.SelectedIndex = 0;

            }

            //BIND FREQUENCY
            string Frequency = oDBEngine.GetFieldValue("master_topics", "topics_deliveryfrequency", "topics_code='" + TopicCode + "'", 1)[0, 0];
            TableName = "dbo.fnSplitReturnTable(" + "'" + Frequency + "'" + ",',')";
            sqlFrequency.SelectCommand = "select * ,(select top 1 case topics_deliveryfrequency when 'E' then 'Upon Event' when 'D' then 'Daily' when 'F' then 'Fort Nightly' when 'M' then 'Monthly'  when 'Q' then 'Quaterly' when 'W' then 'Weekly' else 'Yearly' end from Master_Topics  where topics_deliveryfrequency=col) as Frequency from " + TableName;
            cmbFrequency.DataBind();
            //BIND DELIVERY MODE
            string Mode = oDBEngine.GetFieldValue("master_topics", "topics_deliverymode", "topics_code='" + TopicCode + "'", 1)[0, 0];
            TableName = "dbo.fnSplitReturnTable(" + "'" + Mode + "'" + ",',')";
            sqlMode.SelectCommand = "select * ,(select top 1 case Topics_DeliveryMode when 'E' then 'Email' when 'M' then 'Message' when 'B' then 'Email & SMS' when 'R' then 'Reminder' else 'SMS' end from Master_Topics  where Topics_DeliveryMode=col) as Descriptions from " + TableName;
            cmbMode.DataBind();
            DataTable DTMode = new DataTable();
            DTMode = oDBEngine.GetDataTable("Master_Topics", "* ,(select top 1 case Topics_DeliveryMode when 'E' then 'Email' when 'M' then 'Message' when 'B' then 'Email & SMS' when 'R' then 'Reminder' else 'SMS' end", "Topics_DeliveryMode=col) as Descriptions from " + TableName);
            if (DTMode.Rows.Count == 2)
            {
                cmbMode.Items.Insert(0, new ListEditItem("Both", "B"));
                //cmbMode.SelectedIndex = 0;
            }
            //BIND PHONE NUMBER
            sqlPhone.SelectCommand = "select phf_id as col, phf_phonenumber as phone from tbl_master_phonefax where phf_type='Mobile' and phf_cntid='" + Session["KeyVal_InternalID"] + "'";
            cmbPhone.DataBind();
            //BIND EMAIL ID
            sqlEmail.SelectCommand = "select eml_id as col,eml_email as email from tbl_master_email where eml_cntid='" + Session["KeyVal_InternalID"] + "' and eml_email is not null ";
            cmbEmail.DataBind();
            string acccode = oDBEngine.GetFieldValue("master_topics", "topics_accesscode", " topics_code='" + TopicCode + "'", 1)[0, 0];
            txt.Text = acccode.ToString();

            //Clear Notification
            string NotificationMethod1 = oDBEngine.GetFieldValue("master_topics", "topics_notificationmethod", "topics_code='" + TopicCode + "'", 1)[0, 0];
            TableName = "dbo.fnSplitReturnTable(" + "'" + NotificationMethod1 + "'" + ",',')";
            sqlNotification.SelectCommand = "select * ,(select top 1 case topics_notificationmethod when 'A' then 'Auto'  else 'On Request' end from Master_Topics  where topics_notificationmethod=col ) as Notification from " + TableName + "where 1=2";
            cmbNotification.DataBind();
            //

            //CLEAR FREQUENCY
            string Frequency1 = oDBEngine.GetFieldValue("master_topics", "topics_deliveryfrequency", "topics_code='" + TopicCode + "'", 1)[0, 0];
            TableName = "dbo.fnSplitReturnTable(" + "'" + Frequency1 + "'" + ",',')";
            sqlFrequency.SelectCommand = "select * ,(select top 1 case topics_deliveryfrequency when 'E' then 'Upon Event' when 'D' then 'Daily' when 'F' then 'Fort Nightly' when 'M' then 'Monthly'  when 'Q' then 'Quaterly' when 'W' then 'Weekly' else 'Yearly' end from Master_Topics  where topics_deliveryfrequency=col) as Frequency from " + TableName + "where 1=2";
            cmbFrequency.DataBind();
            //CLEAR DELIVERY MODE
            string Mode1 = oDBEngine.GetFieldValue("master_topics", "topics_deliverymode", "topics_code='" + TopicCode + "'", 1)[0, 0];
            TableName = "dbo.fnSplitReturnTable(" + "'" + Mode1 + "'" + ",',')";
            sqlMode.SelectCommand = "select * ,(select top 1 case Topics_DeliveryMode when 'E' then 'Email' when 'M' then 'Message' when 'B' then 'Email & SMS' when 'R' then 'Reminder' else 'SMS' end from Master_Topics  where Topics_DeliveryMode=col) as Descriptions from " + TableName + "where 1=2";
            cmbMode.DataBind();
            DataTable DTMode1 = new DataTable();
            DTMode1 = oDBEngine.GetDataTable("Master_Topics", "* ,(select top 1 case Topics_DeliveryMode when 'E' then 'Email' when 'M' then 'Message' when 'B' then 'Email & SMS' when 'R' then 'Reminder' else 'SMS' end", "Topics_DeliveryMode=col) as Descriptions from " + TableName);
            if (DTMode1.Rows.Count == 2)
            {
                cmbMode.Items.Insert(0, new ListEditItem("Both", "B"));
                //cmbMode.SelectedIndex = 0;
            }
            //CLEAR PHONE NUMBER
            sqlPhone.SelectCommand = "select phf_id as col, phf_phonenumber as phone from tbl_master_phonefax where phf_type='Mobile' and phf_cntid='" + Session["KeyVal_InternalID"] + "' and 1=2";
            cmbPhone.DataBind();
            //CLEAR EMAIL ID
            sqlEmail.SelectCommand = "select eml_id as col,eml_email as email from tbl_master_email where eml_cntid='" + Session["KeyVal_InternalID"] + "' and eml_email is not null and 1=2";
            cmbEmail.DataBind();
            return;
        }
        protected void dpFrequency_CustomJSProperties(object sender, DevExpress.Web.CustomJSPropertiesEventArgs e)
        {
            e.Properties["cpValueCombo1"] = acccode;
        }
        protected void gridSubscription_CancelRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            Description = null;
        }
    }
}
