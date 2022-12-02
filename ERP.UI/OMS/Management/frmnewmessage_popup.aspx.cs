using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
//using DevExpress.Web;
using DevExpress.Web;
using BusinessLogicLayer;
using ClsDropDownlistNameSpace;

namespace ERP.OMS.Management
{
    public partial class management_frmnewmessage_popup : System.Web.UI.Page, System.Web.UI.ICallbackEventHandler
    {
        string data = "";
        Converter Oconverter = new Converter();
        BusinessLogicLayer.Management_BL OManagement_BL = new BusinessLogicLayer.Management_BL();
        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);MULTI
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        clsDropDownList cls = new clsDropDownList();
        protected void Page_Load(object sender, EventArgs e)
        {
            txtStart.EditFormat = EditFormat.Custom;
            txtStart.UseMaskBehavior = true;
            txtStart.EditFormatString = Oconverter.GetDateFormat();
            txtEnd.EditFormat = EditFormat.Custom;
            txtEnd.UseMaskBehavior = true;
            txtEnd.EditFormatString = Oconverter.GetDateFormat();
            txtFromDate.EditFormat = EditFormat.Custom;
            txtToDate.EditFormat = EditFormat.Custom;
            txtFromDate.EditFormatString = Oconverter.GetDateFormat();
            txtFromDate.UseMaskBehavior = true;
            txtToDate.UseMaskBehavior = true;
            txtToDate.EditFormatString = Oconverter.GetDateFormat();
            if (!IsPostBack)
            {

                txtFromDate.EditFormatString = Oconverter.GetDateFormat("Date");
                //txtFromDate.Value = oDBEngine.GetDate().AddDays((-1 * oDBEngine.GetDate().Day) + 1);
                txtFromDate.Value = oDBEngine.GetDate().AddDays((-1 * oDBEngine.GetDate().Day) + 1);
                txtToDate.EditFormatString = Oconverter.GetDateFormat("Date");
                //txtToDate.Value = oDBEngine.GetDate();
                txtToDate.Value = oDBEngine.GetDate();

                txtStart.EditFormatString = Oconverter.GetDateFormat("Date");
                //txtStart.Value = oDBEngine.GetDate().AddDays((-1 * oDBEngine.GetDate().Day) + 1);
                txtStart.Value = oDBEngine.GetDate().AddDays((-1 * oDBEngine.GetDate().Day) + 1);
                txtEnd.EditFormatString = Oconverter.GetDateFormat("Date");
                //txtEnd.Value = oDBEngine.GetDate();
                txtEnd.Value = oDBEngine.GetDate();




                ViewState["mode"] = "unRead";
                populateTemplatelist();
                //________This script is for firing javascript when page load first___//
                if (!ClientScript.IsStartupScriptRegistered("Today"))
                    ClientScript.RegisterStartupScript(typeof(Page), "Today", "<script>PageLoadFirst();</script>");
                //______________________________End Script____________________________//
                //SearchByMSGUserID
                txtUser.Attributes.Add("onkeyup", "ajax_showOptions(this,'SearchByUserID',event)");
                txtUserName.Attributes.Add("onkeyup", "ajax_showOptions(this,'SearchByUserID',event)");
                txtRecipients.Attributes.Add("onkeyup", "ajax_showOptions(this,'SearchByUserID',event)");

            }
            // cmbTemplate.Attributes.Add("onchange", "showWindow(this);");
            //_____For performing operation without refreshing page___//
            String cbReference = Page.ClientScript.GetCallbackEventReference(this, "arg", "ReceiveServerData", "context");
            String callbackScript = "function CallServer(arg, context){ " + cbReference + ";}";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "CallServer", callbackScript, true);

            FillGrid(ViewState["mode"].ToString());
        }

        #region Populate template list
        public void populateTemplatelist()
        {
            //DBEngine oDBEngine = new DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);MULTI
            DBEngine oDBEngine = new DBEngine();
            string[,] list = oDBEngine.GetFieldValue(" tbl_master_template ", " tem_id,tem_shortmsg ", " (tem_type=1) and ((tem_accesslevel=1) or (createuser=" + HttpContext.Current.Session["userid"] + ")) ", 2, " tem_shortmsg DESC ");
            cls.AddDataToDropDownList(list, cmbTemplate, true);
            cmbTemplate.Items[0].Text = "New";
        }
        #endregion
        #region for servercall
        void ICallbackEventHandler.RaiseCallbackEvent(string eventArgument)
        {
            string id = eventArgument.ToString();
            string[] FieldWvalue = id.Split('~');
            string IDs = "";
            data = "";


            if (FieldWvalue[0] == "BindCombo")
            {
                if (FieldWvalue[1].ToString() != "")
                {
                    int temp_id = Convert.ToInt32(FieldWvalue[1].ToString());
                    DataTable dtRcp = oDBEngine.GetDataTable("tbl_master_template", "tem_msg,tem_recipients", " tem_id='" + temp_id + "'");
                    if (dtRcp.Rows.Count > 0)
                    {
                        string Recp = dtRcp.Rows[0]["tem_recipients"].ToString();
                        string[,] reUser = oDBEngine.GetFieldValue("tbl_master_user", "ltrim(rtrim(isnull(user_name,'')))+'['+ltrim(rtrim(isnull(user_loginid,''))) +']',user_id  ", "user_id in (" + Recp.ToString() + ")", 2);
                        string TxtUser = "";
                        for (int y = 0; y < reUser.Length / 2; y++)
                        {
                            if (y == 0)
                            {
                                TxtUser = reUser[y, 0].ToString() + "," + reUser[y, 1].ToString();
                            }
                            else
                            {

                                TxtUser = TxtUser + ";" + reUser[y, 0].ToString() + "," + reUser[y, 1].ToString();
                            }
                        }
                        data = "BindCombo~Y~" + dtRcp.Rows[0][0].ToString() + "~" + TxtUser;
                    }
                    else
                    {
                        data = "BindCombo~N~";
                    }
                }
                else
                {
                    data = "BindCombo~N~";
                }


            }

            if (FieldWvalue[0] == "read")
            {
                IDs = FieldWvalue[1];
                if (IDs != "")
                {
                    int NoOfRowsEffected = oDBEngine.SetFieldValue(" tbl_master_message ", " msg_messageread = 1 , msg_updateddate='" + oDBEngine.GetDate() + "'", " msg_id in (" + IDs + ") ");
                    if (NoOfRowsEffected > 0)
                        data = "read~Y";
                    else
                        data = "read~N";
                }
                else
                    data = "read~S";
            }
            if (FieldWvalue[0] == "reply")
            {
                IDs = FieldWvalue[1];
                string[] idlist = IDs.Split(',');
                if (idlist.Length > 1)
                    data = "reply~M";
                else
                {
                    string[,] Items = oDBEngine.GetFieldValue(" tbl_master_message ", " (SELECT     tbl_master_user.user_name   FROM   tbl_master_user  WHERE      user_id = msg_createUser) as msgcreateUser,convert(varchar(17),msg_createdate,113),msg_content,msg_createUser,(SELECT     tbl_master_user.user_contactId   FROM   tbl_master_user  WHERE      user_id = msg_createUser) as ContactId ", " msg_id=" + IDs, 5);
                    if (Items.Length > 0)
                    {
                        data = "reply~" + Items[0, 0] + '~' + Items[0, 1] + '~' + Items[0, 2] + '~' + Items[0, 3] + '~' + Items[0, 4];

                    }
                    else
                        data = "reply~N";
                }

            }
            if (FieldWvalue[0] == "create")
            {
                string[] idU = FieldWvalue[1].Split(',');
                for (int h = 0; h < idU.Length; h++)
                {
                    // int NoOfRowsEffected = oDBEngine.InsurtFieldValue(" tbl_master_message ", " msg_createuser,msg_createdate,msg_targetuser,msg_content,msg_messageread,msg_sourceid ", HttpContext.Current.Session["userid"] + ",'" + oDBEngine.GetDate() + "'," + FieldWvalue[1] + ",'" + FieldWvalue[2] + "',0,0");
                    DataSet dsCnt = new DataSet();
                    dsCnt = OManagement_BL.Fetch_MessageTemplateReservedWord(
                        Convert.ToString(Session["userid"]),
                        Convert.ToString(idU[h])
                        );

                    string TempContent = FieldWvalue[2].ToString();
                    string[] strRec = { "< Recipient Name>", "< Recipient Address>", "< Recipient Number>" };
                    string[] strRecData = { "Recipient_Name", "Recipient_Address", "Recipient_Number" };
                    for (int p = 0; p < strRec.Length; p++)
                    {
                        if (TempContent.Contains(strRec[p].ToString()) == true)
                        {
                            TempContent = TempContent.ToString().Replace(strRec[p].ToString(), dsCnt.Tables[0].Rows[0][strRecData[p].ToString()].ToString());

                        }
                    }

                    string[] strSndr = { "< Sender Name>", "< Sender Number>", "< Sender Branch>", "< Sender Email>", "< Sender Department>" };
                    string[] strSndrData = { "Sender_Name", "Sender_Number", "Sender_Branch", "Sender_Email", "Sender_Department" };
                    for (int g = 0; g < strSndr.Length; g++)
                    {
                        if (TempContent.Contains(strSndr[g].ToString()) == true)
                        {
                            TempContent = TempContent.ToString().Replace(strSndr[g].ToString(), dsCnt.Tables[1].Rows[0][strSndrData[g].ToString()].ToString());

                        }
                    }


                    int NoOfRowsEffected = oDBEngine.InsurtFieldValue(" tbl_master_message ", " msg_createuser,msg_createdate,msg_targetuser,msg_content,msg_messageread,msg_sourceid ", HttpContext.Current.Session["userid"] + ",'" + oDBEngine.GetDate() + "'," + idU[h].ToString() + ",'" + TempContent + "',0,0");
                    if (NoOfRowsEffected > 0)
                        data = "create~Y";
                    else
                        data = "create~N";
                }
                //int NoOfRowsEffected = oDBEngine.InsurtFieldValue(" tbl_master_message ", " msg_createuser,msg_createdate,msg_targetuser,msg_content,msg_messageread,msg_sourceid ", HttpContext.Current.Session["userid"] + ",'" + oDBEngine.GetDate() + "'," + FieldWvalue[1] + ",'" + FieldWvalue[2] + "',0,0");
                //if (NoOfRowsEffected > 0)
                //    data = "create~Y";
                //else
                //    data = "create~N";
            }
            if (FieldWvalue[0] == "send")   //_____this is for replying any message.
            {
                string[,] datafield = oDBEngine.GetFieldValue(" tbl_master_message ", " msg_createuser ", " msg_Id=" + FieldWvalue[1], 1);
                DataSet dsCnt = new DataSet();
                dsCnt = OManagement_BL.Fetch_MessageTemplateReservedWord(
                       Convert.ToString(Session["userid"]),
                       Convert.ToString(datafield[0, 0])
                       );

                string TempContent = FieldWvalue[2].ToString();
                string[] strRec = { "< Recipient Name>", "< Recipient Address>", "< Recipient Number>" };
                string[] strRecData = { "Recipient_Name", "Recipient_Address", "Recipient_Number" };
                for (int p = 0; p < strRec.Length; p++)
                {
                    if (TempContent.Contains(strRec[p].ToString()) == true)
                    {
                        TempContent = TempContent.ToString().Replace(strRec[p].ToString(), dsCnt.Tables[0].Rows[0][strRecData[p].ToString()].ToString());

                    }
                }

                string[] strSndr = { "< Sender Name>", "< Sender Number>", "< Sender Branch>", "< Sender Email>", "< Sender Department>" };
                string[] strSndrData = { "Sender_Name", "Sender_Number", "Sender_Branch", "Sender_Email", "Sender_Department" };
                for (int g = 0; g < strSndr.Length; g++)
                {
                    if (TempContent.Contains(strSndr[g].ToString()) == true)
                    {
                        TempContent = TempContent.ToString().Replace(strSndr[g].ToString(), dsCnt.Tables[1].Rows[0][strSndrData[g].ToString()].ToString());

                    }
                }




                int NoOfRowsEffected = oDBEngine.InsurtFieldValue(" tbl_master_message ", " msg_createuser,msg_createdate,msg_targetuser,msg_content,msg_messageread,msg_sourceid,msg_parent_msg_Id ", HttpContext.Current.Session["userid"] + ",'" + oDBEngine.GetDate() + "'," + datafield[0, 0].ToString() + ",'" + TempContent + "',0,0," + FieldWvalue[1]);
                if (NoOfRowsEffected > 0)
                {
                    NoOfRowsEffected = oDBEngine.SetFieldValue(" tbl_master_message ", " msg_messageread=1 ", " msg_Id=" + FieldWvalue[1]);
                    data = "send~Y";
                }
                else
                    data = "send~N";
                //string[,] datafield = oDBEngine.GetFieldValue(" tbl_master_message ", " msg_createuser ", " msg_Id=" + FieldWvalue[1], 1);
                //int NoOfRowsEffected = oDBEngine.InsurtFieldValue(" tbl_master_message ", " msg_createuser,msg_createdate,msg_targetuser,msg_content,msg_messageread,msg_sourceid,msg_parent_msg_Id ", HttpContext.Current.Session["userid"] + ",'" + oDBEngine.GetDate() + "'," + datafield[0, 0].ToString() + ",'" + FieldWvalue[2] + "',0,0," + FieldWvalue[1]);
                //if (NoOfRowsEffected > 0)
                //{
                //    NoOfRowsEffected = oDBEngine.SetFieldValue(" tbl_master_message ", " msg_messageread=1 ", " msg_Id=" + FieldWvalue[1]);
                //    data = "send~Y";
                //}
                //else
                //    data = "send~N";
            }
            if (FieldWvalue[0] == "delete")
            {
                string result = oDBEngine.transferToBackupMessage(FieldWvalue[1]);
                if (result == "Done")
                    data = "delete~Y";
                else
                    data = "delete~N";

            }
            if (FieldWvalue[0] == "saveTemp")
            {
                string[,] existOrNot = oDBEngine.GetFieldValue(" tbl_master_template ", " tem_msg ", " tem_shortmsg='" + FieldWvalue[1] + "'", 1);
                if (existOrNot[0, 0] == "n")
                {
                    int NoOfRowsEffected = oDBEngine.InsurtFieldValue(" tbl_master_template ", " tem_shortmsg, tem_msg, tem_recipients,tem_type,tem_sendertype,tem_accesslevel,CreateDate,CreateUser ", "'" + FieldWvalue[1] + "','" + FieldWvalue[4] + "','" + FieldWvalue[2] + "',1,0," + FieldWvalue[3] + ",'" + oDBEngine.GetDate() + "'," + HttpContext.Current.Session["userid"]);
                    if (NoOfRowsEffected > 0)
                    {
                        data = "saveTemp~Y";
                        populateTemplatelist();
                    }
                    else
                        data = "saveTemp~N";
                }
                else
                {
                    data = "saveTemp~E";
                }
            }
            if (FieldWvalue[0] == "GetHistory")
            {
                if (txtFromDate.Value != null && txtToDate.Value != null)
                    FillGrid("GetHistory");
            }
            if (FieldWvalue[0] == "Export")
            {
                exporter.WritePdfToResponse();
            }
        }
        string ICallbackEventHandler.GetCallbackResult()
        {
            return data;

        }
        #endregion
        private void FillGrid(string Type)
        {
            //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);MULTI
            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
            switch (Type)
            {
                case "unRead":
                    Session["mode"] = Type;
                    DataTable DT = oDBEngine.GetDataTable(" tbl_master_message ", " msg_id AS Mid, CASE msg_sourceid WHEN 0 THEN msg_createUser ELSE 0 END AS CreaterId, CASE msg_sourceid WHEN 0 THEN   (SELECT     tbl_master_user.user_name   FROM   tbl_master_user  WHERE      user_id = msg_createUser) ELSE 'System' END AS CreateBy, msg_targetUser AS TargetId, (SELECT tbl_master_user.user_name FROM tbl_master_user WHERE  user_id = msg_targetUser) AS Target, msg_content AS content,  CASE msg_messageread WHEN 0 THEN 'UnRead' ELSE 'Attened' END AS Status ", " msg_messageread = 0 and msg_targetUser=" + HttpContext.Current.Session["userid"] + " ORDER BY msg_createdate DESC ");
                    GridMessage.DataSource = DT;
                    GridMessage.DataBind();
                    break;
                case "inBox":
                    Session["mode"] = Type;
                    DT = oDBEngine.GetDataTable(" tbl_master_message ", " TOP 20 msg_id AS Mid, CASE msg_sourceid WHEN 0 THEN msg_createUser ELSE 0 END AS CreaterId, CASE msg_sourceid WHEN 0 THEN   (SELECT     tbl_master_user.user_name   FROM   tbl_master_user  WHERE      user_id = msg_createUser) ELSE 'System' END AS CreateBy, msg_targetUser AS TargetId, (SELECT tbl_master_user.user_name FROM tbl_master_user WHERE  user_id = msg_targetUser) AS Target, msg_content AS content,  CASE msg_messageread WHEN 0 THEN 'UnRead' ELSE 'Attened' END AS Status ", " msg_targetUser=" + HttpContext.Current.Session["userid"] + " ", " msg_createdate DESC ");
                    GridMessage.DataSource = DT;
                    GridMessage.DataBind();
                    break;
                case "filter":
                    Session["mode"] = Type;
                    Converter oConverter = new Converter();
                    string startdate = "";
                    string enddate = "";
                    if (txtStart.Value != null)
                        startdate = txtStart.Value.ToString();
                    if (txtEnd.Value != null)
                        enddate = txtEnd.Value.ToString();
                    //DT = oDBEngine.GetDataTable(" tbl_master_message ", " msg_id AS Mid, CASE msg_sourceid WHEN 0 THEN msg_createUser ELSE 0 END AS CreaterId, CASE msg_sourceid WHEN 0 THEN   (SELECT     tbl_master_user.user_name   FROM   tbl_master_user  WHERE      user_id = msg_createUser) ELSE 'System' END AS CreateBy, msg_targetUser AS TargetId, (SELECT tbl_master_user.user_name FROM tbl_master_user WHERE  user_id = msg_targetUser) AS Target, msg_content AS content,  CASE msg_messageread WHEN 0 THEN 'UnRead' ELSE 'Attened' END AS Status ", " msg_targetUser=" + HttpContext.Current.Session["userid"] + " and msg_createdate between '" + startdate.Value + "' and '" + enddate.Value + "' ", " msg_createdate DESC ");
                    DT = oDBEngine.GetDataTable(" tbl_master_message ", " msg_id AS Mid, CASE msg_sourceid WHEN 0 THEN msg_createUser ELSE 0 END AS CreaterId, CASE msg_sourceid WHEN 0 THEN   (SELECT     tbl_master_user.user_name   FROM   tbl_master_user  WHERE      user_id = msg_createUser) ELSE 'System' END AS CreateBy, msg_targetUser AS TargetId, (SELECT tbl_master_user.user_name FROM tbl_master_user WHERE  user_id = msg_targetUser) AS Target, msg_content AS content,  CASE msg_messageread WHEN 0 THEN 'UnRead' ELSE 'Attened' END AS Status ", " msg_targetUser=" + HttpContext.Current.Session["userid"] + " and msg_createdate > '" + startdate + "' and msg_createdate <='" + enddate + "' ", " msg_createdate DESC ");
                    GridMessage.DataSource = DT;
                    GridMessage.DataBind();
                    break;
                case "outbox":
                    Session["mode"] = Type;
                    DT = oDBEngine.GetDataTable(" tbl_master_message ", " msg_id AS Mid, msg_targetUser AS TargetId, (SELECT tbl_master_user.user_name FROM tbl_master_user WHERE  user_id = msg_targetUser) AS Target, msg_content AS content,convert(varchar(17),msg_updateddate,113) as ReadDate,  CASE msg_messageread WHEN 0 THEN 'UnRead' ELSE 'Attened' END AS Status ", " msg_createuser=" + HttpContext.Current.Session["userid"] + "  ", " msg_createdate DESC ");
                    gridOutBox.DataSource = DT.DefaultView;
                    gridOutBox.DataBind();
                    break;
                case "GetHistory":
                    Session["mode"] = Type;
                    string[] ids = txtUserName_hidden.Text.Split(',');
                    string startdate1 = "";
                    string enddate1 = "";
                    Converter oConverter1 = new Converter();
                    if (txtFromDate.Value != null)
                        startdate1 = txtFromDate.Value.ToString();
                    if (txtToDate.Value != null)
                        enddate1 = txtToDate.Value.ToString();
                    if (startdate1 != "" && enddate1 != "")
                    {
                        DataTable DT1 = new DataTable();
                        DataColumn col1 = new DataColumn("sender");
                        DataColumn col2 = new DataColumn("Target");
                        DataColumn col3 = new DataColumn("content");
                        DataColumn col4 = new DataColumn("CreatedDate");
                        DataColumn col5 = new DataColumn("ReadDate");
                        DataColumn col6 = new DataColumn("DeleteDate");

                        DT1.Columns.Add(col1);
                        DT1.Columns.Add(col2);
                        DT1.Columns.Add(col3);
                        DT1.Columns.Add(col4);
                        DT1.Columns.Add(col5);
                        DT1.Columns.Add(col6);

                        string[,] mesge = oDBEngine.GetFieldValue(" tbl_master_message ", " (SELECT tbl_master_user.user_name FROM tbl_master_user WHERE  user_id = msg_createuser) AS sender, (SELECT tbl_master_user.user_name FROM tbl_master_user WHERE  user_id = msg_targetUser) AS Target,msg_content as content, msg_createdate as CreatedDate,msg_updateddate as ReadDate ", " msg_createdate between '" + startdate1 + "' and '" + enddate1 + "' and ((msg_targetuser=" + HttpContext.Current.Session["userid"] + " and msg_createuser=" + ids[1] + ") or (msg_targetuser=" + ids[1] + " and msg_createuser=" + HttpContext.Current.Session["userid"] + "))", 5, " msg_createdate desc");
                        if (mesge[0, 0] != "n")
                        {
                            for (int i = 0; i < mesge.Length / 5; i++)
                            {
                                DataRow row1 = DT1.NewRow();
                                row1["sender"] = mesge[i, 0];
                                row1["Target"] = mesge[i, 1];
                                row1["content"] = mesge[i, 2];
                                row1["CreatedDate"] = mesge[i, 3];
                                row1["ReadDate"] = mesge[i, 4];
                                row1["DeleteDate"] = "";

                                DT1.Rows.Add(row1);
                            }
                        }
                        mesge = oDBEngine.GetFieldValue(" tbl_master_message_backup ", " (SELECT tbl_master_user.user_name FROM tbl_master_user WHERE  user_id = msg_createuser) AS sender, (SELECT tbl_master_user.user_name FROM tbl_master_user WHERE  user_id = msg_targetUser) AS Target,msg_content as content, msg_createdate as CreatedDate,msg_updateddate as ReadDate, msg_deleteDate ", " msg_createdate between '" + startdate1 + "' and '" + enddate1 + "' and ((msg_targetuser=" + HttpContext.Current.Session["userid"] + " and msg_createuser=" + ids[1] + ") or (msg_targetuser=" + ids[1] + " and msg_createuser=" + HttpContext.Current.Session["userid"] + "))", 6, " msg_createdate desc");
                        if (mesge[0, 0] != "n")
                        {
                            for (int i = 0; i < mesge.Length / 6; i++)
                            {
                                DataRow row1 = DT1.NewRow();
                                row1["sender"] = mesge[i, 0];
                                row1["Target"] = mesge[i, 1];
                                row1["content"] = mesge[i, 2];
                                row1["CreatedDate"] = mesge[i, 3];
                                row1["ReadDate"] = mesge[i, 4];
                                row1["DeleteDate"] = mesge[i, 5];

                                DT1.Rows.Add(row1);
                            }
                        }
                        GridHistory.DataSource = DT1.DefaultView;
                        GridHistory.DataBind();
                    }
                    break;
                default:
                    break;

            }
        }

        protected void GridMessage_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            string param = e.Parameters.ToString();
            if (param == "unRead")
            {
                FillGrid("unRead");
            }
            if (param == "inBox")
            {
                FillGrid("inBox");
            }
            if (param == "filter")
            {

                FillGrid("filter");
            }
            if (param == "read")
            {
                FillGrid(ViewState["mode"].ToString());
            }
        }
        protected void gridOutBox_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            FillGrid("outbox");
        }
        protected void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 Filter = int.Parse(cmbExport.SelectedItem.Value.ToString());
            switch (Filter)
            {
                case 1:
                    exporter.WritePdfToResponse();
                    break;
                case 2:
                    exporter.WriteXlsToResponse();
                    break;
                case 3:
                    exporter.WriteRtfToResponse();
                    break;
                case 4:
                    exporter.WriteCsvToResponse();
                    break;
                default:
                    break;
            }
        }
        protected void GridHistory_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            string param = e.Parameters.ToString();
            FillGrid(param);
        }

    }
}