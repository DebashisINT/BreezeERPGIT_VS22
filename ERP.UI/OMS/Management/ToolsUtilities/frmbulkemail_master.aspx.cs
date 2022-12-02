using System;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.UI;
using BusinessLogicLayer;
using System.Globalization;
using System.Web.Services;
using System.Collections.Generic;
namespace ERP.OMS.Management.ToolsUtilities
{
    public partial class management_utilities_frmbulkemail_master : System.Web.UI.Page, System.Web.UI.ICallbackEventHandler
    {
        string data = "";
        public string pageAccess = "";
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        public EntityLayer.CommonELS.UserRightsForPage rights = new EntityLayer.CommonELS.UserRightsForPage();

        //DBEngine oDBEngine = new DBEngine(string.Empty);

        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //'http://localhost:2957/InfluxCRM/management/testProjectMainPage_employee.aspx'
                string sPath = HttpContext.Current.Request.Url.ToString();
                oDBEngine.Call_CheckPageaccessebility(sPath);
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/ToolsUtilities/frmbulkemail_master.aspx");

            // Session Handler Change by sudip on 19122016

            //if (HttpContext.Current.Session["userid"] == null)
            //{
            //    Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            //}
            ////this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");

            if (HttpContext.Current.Session["userid"] != null)
            {
                if (!IsPostBack)
                {
                    fillTimeConbo();
                    cmbBodySource.Attributes.Add("onchange", "BodySourceChange()");
                    cmbSendingOption.Attributes.Add("onchange", "SendingMailOption(cmbSendingOption.value)");
                    chkDefaultFooter.Attributes.Add("onclick", "FooterChange(chkDefaultFooter.checked)");

                    txtStartDate.Text = oDBEngine.GetDate().ToString("dd/MM/yyyy hh:mm");
                    txtEndDate.Text = oDBEngine.GetDate().AddDays(1).ToString("dd/MM/yyyy hh:mm");
                    txtStartDate.Attributes.Add("onfocus", "displayCalendar(txtStartDate ,'dd/mm/yyyy hh:ii',this,true,null,'0','350')");
                    imgStartDate.Attributes.Add("onclick", "displayCalendar(txtStartDate ,'dd/mm/yyyy hh:ii',txtStartDate,true,null,'0','350')");
                    txtEndDate.Attributes.Add("onfocus", "displayCalendar(txtEndDate,'dd/mm/yyyy hh:ii',this,true,null,'0','350')");
                    imgEnadDate.Attributes.Add("onclick", "displayCalendar(txtEndDate ,'dd/mm/yyyy hh:ii',txtEndDate,true,null,'0','350')");
                    //txtsubscriptionID.Attributes.Add("onkeyup", "ajax_showOptions(this,'SearchByUserID',event)");

                    //Subscribing Email change to multiple select combo box by sudip on 19-12-2016//
                    //txtRecipients.Attributes.Add("onkeyup", "ajax_showOptions(this,'SearchByUserID',event)");

                    //________This script is for firing javascript when page load first___//
                    if (!ClientScript.IsStartupScriptRegistered("Today"))
                        ClientScript.RegisterStartupScript(typeof(Page), "Today", "<script>AtTheTimePageLoad();</script>");
                    //______________________________End Script____________________________//
                    FillGrid();
                }

                //________This script is for firing javascript when page load first___//
                //if (!ClientScript.IsStartupScriptRegistered("Today"))
                //    ClientScript.RegisterStartupScript(typeof(Page), "Today", "<script>AtTheTimePageLoad();</script>");
                //______________________________End Script____________________________//
                //_____For performing operation without refreshing page___//

                String cbReference = Page.ClientScript.GetCallbackEventReference(this, "arg", "ReceiveServerData", "context");
                String callbackScript = "function CallServer(arg, context){ " + cbReference + ";}";
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "CallServer", callbackScript, true);

                BindReservedWord();

                //___________-end here___//
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
        }

        private void FillGrid()
        {
            //DBEngine oDBEngine = new DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);
            //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);MULTI
            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
            DataTable DTgrid = oDBEngine.GetDataTable(" tbl_master_bulkmail ", " bem_id, bem_description,bem_subject,bem_inusedate,bem_useuntil, (select 'subs-' + convert(varchar(10),count(*)) from tbl_trans_emailsubscriptionlist where esl_masterid= bem_id and esl_enddate is null) as subscription ", " bem_useuntil is null ", " CreateDate desc ");
            GridBulkTemplate.DataSource = DTgrid;
            GridBulkTemplate.DataBind();
        }

        private void fillTimeConbo()
        {
            string[,] data = new string[12, 2];
            for (int i = 1; i < 13; i++)
            {
                data[i - 1, 0] = i.ToString();
                data[i - 1, 1] = i.ToString();
            }
            //Converter oConverter = new Converter();
            BusinessLogicLayer.Converter oConverter = new BusinessLogicLayer.Converter();
            oConverter.AddDataToDropDownList(data, cmbhoure);
            string[,] data1 = new string[60, 2];
            for (int i = 0; i < 60; i++)
            {
                if (i < 10)
                {
                    data1[i, 0] = "0" + i.ToString();
                    data1[i, 1] = "0" + i.ToString();
                }
                else
                {
                    data1[i, 0] = i.ToString();
                    data1[i, 1] = i.ToString();
                }
            }
            oConverter.AddDataToDropDownList(data1, cmbminute);
        }

        #region ICallbackEventHandler Members

        string ICallbackEventHandler.GetCallbackResult()
        {
            return data;
        }

        void ICallbackEventHandler.RaiseCallbackEvent(string eventArgument)
        {
            //DBEngine oDBEngine = new DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);Multi
            DBEngine oDBEngine = new DBEngine();
            string id = eventArgument.ToString();
            string[] FieldWvalue = id.Split('~');
            data = "";
            #region disclaimer
            if (FieldWvalue[0] == "disclaimer")
            {
                string[,] disclmr = oDBEngine.GetFieldValue(" tbl_trans_email_disclaimer ", " dec_disclaimer ", " dec_status = 'O' ", 1);
                if (disclmr[0, 0] != "n")
                    data = "disclaimer~" + disclmr[0, 0];
                else
                    data = "disclaimer~N";
            }
            #endregion
            #region Save

            if (FieldWvalue[0] == "Save")
            {
                //For Reference:---> 'Save~'+txtDescription.value+'~'+txtSubject.value+'~'+txtMessageHeader.value+'~'+cmbBodySource.value+'~'+txtMessageBody.value+'~'+txtMessageFooter.value+'~'+txtReplyTo.value+'~'+txtUnsubscribeEmailid.value+'~'+txtSenderName.value+'~'+txtSenderEmail.value+'~'+cmbSendingOption.value+'~'+cmbhoure.value+'~'+cmbminute.value+'~'+cmbampm.value+'~'+txtStartDate.value+'~'+txtEndDate.value+'~'+userlist;
                string fields = " bem_description,bem_subject,bem_header,bem_bodysource,bem_body,bem_footer,bem_replyto,bem_unsubscribingemailid,bem_displayname,bem_senderidtype,bem_sendingoption,bem_sendtime,bem_inusedate,CreateDate,CreateUser ";
                string values = "'" + FieldWvalue[1] + "','" + FieldWvalue[2] + "','" + FieldWvalue[3] + "','" + FieldWvalue[4] + "','" + FieldWvalue[5] + "','" + FieldWvalue[6] + "','" + FieldWvalue[7] + "','" + FieldWvalue[8] + "','" + FieldWvalue[9] + "','" + FieldWvalue[10] + "','" + FieldWvalue[11] + "','" + FieldWvalue[12] + ":" + FieldWvalue[13] + " " + FieldWvalue[14] + "','" + Convert.ToDateTime(oDBEngine.GetDate(), new CultureInfo("en-US")) + "','" + Convert.ToDateTime(oDBEngine.GetDate(), new CultureInfo("en-US")) + "'," + HttpContext.Current.Session["userid"];
                int NoOfRowsEffected = oDBEngine.InsurtFieldValue(" tbl_master_bulkmail ", fields, values);
                if (NoOfRowsEffected > 0)
                {
                    //if (FieldWvalue[17] != "0")
                    if (FieldWvalue[18] != "")
                    {
                        string[,] letestID = oDBEngine.GetFieldValue(" tbl_master_bulkmail ", " top 1 bem_id ", " bem_subject='" + FieldWvalue[2] + "' and CreateUser=" + HttpContext.Current.Session["userid"], 1, " CreateDate desc ");
                        //string[] cntIds = FieldWvalue[17].Split(',');
                        string[] cntIds = FieldWvalue[18].Split(',');

                        for (int i = 0; i < cntIds.Length; i++)
                        {
                            NoOfRowsEffected = oDBEngine.InsurtFieldValue(" tbl_trans_emailsubscriptionlist ", " esl_masterid,esl_cntid,esl_startdate,CreateDate,CreateUser ", letestID[0, 0] + ",'" + cntIds[i] + "','" + oDBEngine.GetDate() + "','" + oDBEngine.GetDate() + "'," + HttpContext.Current.Session["userid"]);
                        }
                        data = "Save~Y";
                    }
                    else
                        data = "Save~Saved Successfully without subscriptionlist!";
                }
                else
                    data = "Save~Unsuccessful Try Again!";
            }

            #endregion
            #region Update
            if (FieldWvalue[0] == "Update")
            {
                //For Reference:---> 'Save~'+txtDescription.value+'~'+txtSubject.value+'~'+txtMessageHeader.value+'~'+cmbBodySource.value+'~'+txtMessageBody.value+'~'+txtMessageFooter.value+'~'+txtReplyTo.value+'~'+txtUnsubscribeEmailid.value+'~'+txtSenderName.value+'~'+txtSenderEmail.value+'~'+cmbSendingOption.value+'~'+cmbhoure.value+'~'+cmbminute.value+'~'+cmbampm.value+'~'+txtStartDate.value+'~'+txtEndDate.value+'~'+userlist+'~'+RoWId;
                string fieldsWvalueForUpdate = " bem_description='" + FieldWvalue[1] + "',bem_subject='" + FieldWvalue[2] + "',bem_header='" + FieldWvalue[3] + "',bem_bodysource='" + FieldWvalue[4] + "',bem_body='" + FieldWvalue[5] + "',bem_footer='" + FieldWvalue[6] + "',bem_replyto='" + FieldWvalue[7] + "',bem_unsubscribingemailid='" + FieldWvalue[8] + "',bem_displayname='" + FieldWvalue[9] + "',bem_senderidtype='" + FieldWvalue[10] + "',bem_sendingoption='" + FieldWvalue[11] + "',bem_sendtime='" + FieldWvalue[12] + ":" + FieldWvalue[13] + " " + FieldWvalue[14] + "',LastModifyDate='" + oDBEngine.GetDate() + "',LastModifyUser=" + HttpContext.Current.Session["userid"];
                int NoOfRowsEffected = oDBEngine.SetFieldValue(" tbl_master_bulkmail ", fieldsWvalueForUpdate, " bem_id=" + FieldWvalue[18]);
                if (NoOfRowsEffected > 0)
                {
                    //if (FieldWvalue[17] != "0")
                    if (FieldWvalue[19] != "")
                    {
                        string[] IDlist = FieldWvalue[19].Split(',');
                        //string[] IDlist = FieldWvalue[17].Split(',');

                        string[,] actualSubslist = oDBEngine.GetFieldValue(" tbl_trans_emailsubscriptionlist ", " esl_cntid ", " esl_masterid=" + FieldWvalue[18] + " and esl_enddate is null", 1);
                        for (int i = 0; i < actualSubslist.Length; i++)
                        {
                            Boolean flag = false;
                            for (int j = 0; j < IDlist.Length; j++)
                            {
                                if (actualSubslist[i, 0] == IDlist[j])
                                {
                                    flag = true;
                                }
                            }
                            if (flag == false)
                            {
                                NoOfRowsEffected = oDBEngine.SetFieldValue(" tbl_trans_emailsubscriptionlist ", " esl_enddate='" + oDBEngine.GetDate() + "' ", " esl_masterid=" + FieldWvalue[18] + " and esl_cntid='" + actualSubslist[i, 0] + "'");
                            }
                        }

                        for (int i = 0; i < IDlist.Length; i++)
                        {
                            Boolean flag = false;
                            for (int j = 0; j < actualSubslist.Length; j++)
                            {
                                if (IDlist[i] == actualSubslist[j, 0])
                                {
                                    flag = true;
                                }
                            }
                            if (flag == false)
                            {
                                NoOfRowsEffected = oDBEngine.InsurtFieldValue(" tbl_trans_emailsubscriptionlist ", " esl_masterid,esl_cntid,esl_startdate,CreateDate,CreateUser ", FieldWvalue[18] + ",'" + IDlist[i] + "','" + oDBEngine.GetDate() + "','" + oDBEngine.GetDate() + "'," + HttpContext.Current.Session["userid"]);
                            }
                        }
                        data = "Update~Y";
                    }
                    else
                        data = "Update~Updated Successfully without subscriptionlist!";
                }
                else
                    data = "Update~Update Unsuccessful!";
            }
            #endregion
            #region subscriptionlist
            if (FieldWvalue[0] == "Subscriptionlist")
            {
                string[,] subslist = oDBEngine.GetFieldValue(" tbl_trans_emailsubscriptionlist, tbl_master_user ", " esl_cntid, user_name", " user_contactid=esl_cntid and esl_enddate is null and esl_masterid=" + FieldWvalue[1], 2);
                string list = "";
                if (subslist[0, 0] != "n")
                {
                    for (int i = 0; i < subslist.Length / 2; i++)
                    {
                        if (list != "")
                            list += "," + subslist[i, 0];
                        else
                            list = subslist[i, 0];
                    }
                    data = "Subscriptionlist~" + list;
                }
                else
                {
                    data = "Subscriptionlist~N";
                }
            }

            #endregion
            #region SaveSubscriptionlist

            if (FieldWvalue[0] == "SaveSubscriptionlist")
            {
                string[] IDlist = FieldWvalue[2].Split(',');
                int NoOfRowsEffected = 0;
                string[,] actualSubslist = oDBEngine.GetFieldValue(" tbl_trans_emailsubscriptionlist ", " esl_cntid ", " esl_masterid=" + FieldWvalue[1] + " and esl_enddate is null", 1);
                for (int i = 0; i < actualSubslist.Length; i++)
                {
                    Boolean flag = false;
                    for (int j = 0; j < IDlist.Length; j++)
                    {
                        if (actualSubslist[i, 0] == IDlist[j])
                        {
                            flag = true;
                        }
                    }
                    if (flag == false)
                    {
                        NoOfRowsEffected = oDBEngine.SetFieldValue(" tbl_trans_emailsubscriptionlist ", " esl_enddate='" + oDBEngine.GetDate() + "' ", " esl_masterid=" + FieldWvalue[1] + " and esl_cntid='" + actualSubslist[i, 0] + "'");
                    }
                }

                for (int i = 0; i < IDlist.Length; i++)
                {
                    Boolean flag = false;
                    for (int j = 0; j < actualSubslist.Length; j++)
                    {
                        if (IDlist[i] == actualSubslist[j, 0])
                        {
                            flag = true;
                        }
                    }
                    if (flag == false)
                    {
                        NoOfRowsEffected = oDBEngine.InsurtFieldValue(" tbl_trans_emailsubscriptionlist ", " esl_masterid,esl_cntid,esl_startdate,CreateDate,CreateUser ", FieldWvalue[1] + ",'" + IDlist[i] + "','" + oDBEngine.GetDate() + "','" + oDBEngine.GetDate() + "'," + HttpContext.Current.Session["userid"]);
                    }
                }
                data = "SaveSubscriptionlist~Saved Successfully!";
            }

            #endregion
            #region Delete
            if (FieldWvalue[0] == "Delete")
            {
                int NoOfRecordEffected = oDBEngine.SetFieldValue(" tbl_master_bulkmail ", " bem_useuntil ='" + oDBEngine.GetDate() + "'", " bem_id=" + FieldWvalue[1]);
                int NoOfRecordEffected_list = oDBEngine.SetFieldValue(" tbl_trans_emailsubscriptionlist ", " bem_useuntil ='" + oDBEngine.GetDate() + "'", " bem_id=" + FieldWvalue[1]);
                if (NoOfRecordEffected > 0)
                    data = "Delete~Y~Deleted Successfully!";
                else
                    data = "Delete~Delete failed!";
            }
            #endregion
            #region Edit
            if (FieldWvalue[0] == "Edit")
            {
                string[,] ItemList = oDBEngine.GetFieldValue(" tbl_master_bulkmail ", " bem_description,bem_subject,bem_header,bem_bodysource,bem_body,bem_footer,bem_replyto,bem_displayname,bem_senderidtype,bem_sendingoption,bem_sendtime ", " bem_id=" + FieldWvalue[1], 11);
                string datalist = "";
                if (ItemList[0, 0] != "n")
                {
                    datalist = ItemList[0, 0] + "\"" + ItemList[0, 1] + "\"" + ItemList[0, 2] + "\"" + ItemList[0, 3] + "\"" + ItemList[0, 4] + "\"" + ItemList[0, 5] + "\"" + ItemList[0, 6] + "\"" + ItemList[0, 7] + "\"" + ItemList[0, 8] + "\"" + ItemList[0, 9] + "\"" + ItemList[0, 10];
                }
                string[,] subslist = oDBEngine.GetFieldValue(" tbl_trans_emailsubscriptionlist, tbl_master_user ", " esl_cntid, user_name", " user_contactid=esl_cntid and esl_enddate is null and esl_masterid=" + FieldWvalue[1], 2);
                string SubsLists = "";
                if (subslist[0, 0] != "n")
                {
                    for (int i = 0; i < subslist.Length / 2; i++)
                    {
                        if (SubsLists != "")
                            SubsLists += "," + subslist[i, 0];
                        else
                            SubsLists = subslist[i, 0];
                    }
                }
                data = "Edit~" + datalist + "~" + SubsLists;
            }
            #endregion
        }

        #endregion

        protected void GridBulkTemplate_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            FillGrid();
        }
        protected void ListBoxUserAll_SelectedIndexChanged(object sender, EventArgs e)
        {

        }


        [WebMethod]
        public static List<string> GetRecipients(string Type)
        {
            //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);MULTI
            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
            DataTable DT = oDBEngine.GetDataTable("tbl_master_user", "user_name,user_contactId ", null);
            List<string> obj = new List<string>();
            foreach (DataRow dr in DT.Rows)
            {
                obj.Add(Convert.ToString(dr["user_name"]) + "|" + Convert.ToString(dr["user_contactId"]));
            }

            return obj;
        }
        public void BindReservedWord()
        {
            string[] list = new String[] { "receipent" };
            //Converter oConverter = new Converter();     //____This is to call recipient variable with the predefined values.
            BusinessLogicLayer.Converter oConverter = new BusinessLogicLayer.Converter();
            string UIstring = "<table><tr><td><table>";
            string UIstring_Header = "<table><tr><td><table>";
            if (list[0] == "receipent")
            {
                UIstring += "<tr>";
                UIstring_Header += "<tr>";

                string[,] recipient = oConverter.ReservedWord_recipient();
                string mess = "window.opener.document.aspnetForm.txtMessageHeader.value";
                for (int i = 0; i < recipient.Length / 2; i++)
                {
                    //UIstring += "<td><input style='border-right: 1px groove; border-top: 1px groove; font-size: 8pt;  border-left: 1px groove; color: black; border-bottom: 1px groove; font-style: normal; font-family: Verdana; width: 125px' onclick='" + Request.QueryString["control"].Trim() + "=" + Request.QueryString["control"].Trim() + "+< \"+ this.value +\">" + ";' type='button' id='chk' name='chk' value='" + recipient[i, 0] + "'></td>";
                    UIstring += "<td><input style='border-right: 1px groove; border-top: 1px groove; font-size: 8pt;  border-left: 1px groove; color: black; border-bottom: 1px groove; font-style: normal; font-family: Verdana; width: 125px' onclick='PostReservedWord(this.value);'  type='button' id='chk' name='chk' value='" + recipient[i, 0] + "'></td>";
                }
                UIstring += "</tr>";

                for (int i = 0; i < recipient.Length / 2; i++)
                {
                    //UIstring += "<td><input style='border-right: 1px groove; border-top: 1px groove; font-size: 8pt;  border-left: 1px groove; color: black; border-bottom: 1px groove; font-style: normal; font-family: Verdana; width: 125px' onclick='" + Request.QueryString["control"].Trim() + "=" + Request.QueryString["control"].Trim() + "+< \"+ this.value +\">" + ";' type='button' id='chk' name='chk' value='" + recipient[i, 0] + "'></td>";
                    UIstring_Header += "<td><input style='border-right: 1px groove; border-top: 1px groove; font-size: 8pt;  border-left: 1px groove; color: black; border-bottom: 1px groove; font-style: normal; font-family: Verdana; width: 125px' onclick='PostReservedWordHeader(this.value);'  type='button' id='chk' name='chk' value='" + recipient[i, 0] + "'></td>";
                }
                UIstring_Header += "</tr>";
            }
            if (list.Length > 1)
            {
                if (list[1] == "sender")
                {
                    string[,] sender1 = oConverter.ReservedWord_sender();
                    UIstring += "<tr></tr><tr>";
                    for (int i = 0; i < sender1.Length / 2; i++)
                    {
                        //    UIstring += "<td><input style='border-right: 1px groove; border-top: 1px groove; font-size: 8pt;  border-left: 1px groove; color: black; border-bottom: 1px groove; font-style: normal; font-family: Verdana; width: 125px' onclick='" + Request.QueryString["control"].Trim() + "=" + Request.QueryString["control"].Trim() + "+\"< \"+this.value+\">\" ;' type='button' id='chk' name='chk' value='" + sender1[i, 0] + "'></td>";
                        UIstring += "<td><input style='border-right: 1px groove; border-top: 1px groove; font-size: 8pt;  border-left: 1px groove; color: black; border-bottom: 1px groove; font-style: normal; font-family: Verdana; width: 125px' onclick='PostReservedWord(this.value);'  type='button' id='chk' name='chk' value='" + sender1[i, 0] + "'></td>";
                    }
                    UIstring += "</tr>";
                }
            }
            //UIstring += "<tr></tr><tr><td><input style='border-right: 1px groove; border-top: 1px groove; font-size: 8pt;  border-left: 1px groove; color: black; border-bottom: 1px groove; font-style: normal; font-family: Verdana; width: 125px' onclick='window.close();' type='button' id='close' value='Close' ></td></tr>";
            UIstring += "</table></td></tr></table>";

            //UIstring_Header += "<tr></tr><tr><td><input style='border-right: 1px groove; border-top: 1px groove; font-size: 8pt;  border-left: 1px groove; color: black; border-bottom: 1px groove; font-style: normal; font-family: Verdana; width: 125px' onclick='window.close();' type='button' id='close' value='Close' ></td></tr>";
            UIstring_Header += "</table></td></tr></table>";

            //Response.Write(UIstring);
            Div.InnerHtml = UIstring;
            myDiv.InnerHtml = UIstring_Header;
        }
    }
}