using System;
using System.Configuration;
using System.Data;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogicLayer;
using ClsDropDownlistNameSpace;

namespace ERP.OMS.Management
{
    public partial class management_frmSendMailForPhoneCall : System.Web.UI.Page, System.Web.UI.ICallbackEventHandler
    {
        clsDropDownList cls = new clsDropDownList();
        string data = "";
        DataTable DT = new DataTable();
        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);MULTI
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        BusinessLogicLayer.Converter objconverter = new BusinessLogicLayer.Converter();
        DataSet ds = new DataSet();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["phonecallid"] != null)
                {
                    string PhoneId = Session["phonecallid"].ToString();

                    ds = oDBEngine.PopulateData("tbl_master_calldispositions.call_dispositions, tbl_trans_phonecall.phc_nextCall, tbl_trans_phonecall.phc_callDate,phc_leadcotactId,phc_activityId", "tbl_trans_phonecall INNER JOIN tbl_master_calldispositions ON tbl_trans_phonecall.phc_callDispose = tbl_master_calldispositions.call_id", " tbl_trans_phonecall.phc_id = '" + PhoneId + "'");


                    if (ds.Tables[0].Rows.Count != 0)
                    {
                        string LeadID = ds.Tables[0].Rows[0]["phc_leadcotactId"].ToString();
                        string[,] email_id = oDBEngine.GetFieldValue("tbl_master_email", "eml_email", "eml_cntId='" + LeadID + "'", 1);
                        for (int k = 0; k < email_id.Length; k++)
                        {
                            if (email_id[k, 0] != null)
                            {
                                if (k == 0)
                                {
                                    txtUsermailIDs.Text = email_id[k, 0].ToString();
                                }
                                else
                                {
                                    txtUsermailIDs.Text = txtUsermailIDs.Text + "," + email_id[k, 0].ToString();
                                }
                            }
                        }

                    }
                }



                lblHeader.Text = "Compose Mail ";
                //________This script is for firing javascript when page load first___//
                if (!ClientScript.IsStartupScriptRegistered("Today"))
                    ClientScript.RegisterStartupScript(typeof(Page), "Today", "<script>AtTheTimePageLoad();</script>");
                //______________________________End Script____________________________//

                FillCombo();
                txtUserId.Attributes.Add("onkeyup", "callAjax(this,'GetMailId',event)");
                drpDocumentEntity.Attributes.Add("onchange", "callDoc()");
                cmbTemplate.Attributes.Add("onchange", "callTemplateChange()");
                txtName.Attributes.Add("onkeyup", "callAjaxDoc(this,'searchdocument',event)");
                Page.ClientScript.RegisterStartupScript(GetType(), "Calling", "<script language='JavaScript'>visibilityonoff()</script>");
                Session["table"] = null;
            }

            //_____For performing operation without refreshing page___//
            String cbReference = Page.ClientScript.GetCallbackEventReference(this, "arg", "ReceiveServerData", "context");
            String callbackScript = "function CallServer(arg, context){ " + cbReference + ";}";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "CallServer", callbackScript, true);
            //___________-end here___//
            if (Session["mode"].ToString() == "LocalCall")
            {
                //________This script is for firing javascript when page postback___//
                if (!ClientScript.IsStartupScriptRegistered("Today"))
                    ClientScript.RegisterStartupScript(typeof(Page), "Today", "<script>ServerLocalCall('LocalCall');</script>");
                //______________________________End Script____________________________//
            }
            if (Session["mode"].ToString() == "ServerCall")
            {
                //________This script is for firing javascript when page postback___//
                if (!ClientScript.IsStartupScriptRegistered("Today"))
                    ClientScript.RegisterStartupScript(typeof(Page), "Today", "<script>ServerLocalCall('ServerCall');</script>");
                //______________________________End Script____________________________//

            }
        }

        private void FillCombo()
        {
            //DBEngine oDBEngine = new DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);MULTI
            DBEngine oDBEngine = new DBEngine();
            string[,] data = oDBEngine.GetFieldValue(" tbl_master_contactType ", " cnt_prefix,cnttpy_contactType ", null, 2, " cnttpy_contactType ");
            //oDBEngine.AddDataToDropDownList(data, cmbContactType, true);
            cls.AddDataToDropDownList(data, cmbContactType);
            data = oDBEngine.GetFieldValue(" tbl_master_template ", " tem_msg+'^'+IsNull(cast(tem_id as nvarchar),'')+'^'+IsNull(cast(tem_sendertype as nvarchar),'') as MSG,tem_shortmsg ", " tem_type=2 ", 2, " tem_shortmsg ");
            cls.AddDataToDropDownList(data, cmbTemplate, true);
            /*data = oDBEngine.GetFieldValue(" tbl_master_user INNER JOIN  tbl_master_branch ON tbl_master_user.user_branchId = tbl_master_branch.branch_id INNER JOIN  tbl_trans_employeeCTC ON " +
                                         "tbl_master_user.user_contactId = tbl_trans_employeeCTC.emp_cntId INNER JOIN " +
                                        "tbl_master_costCenter ON tbl_trans_employeeCTC.emp_Department = tbl_master_costCenter.cost_id INNER JOIN    " +
                                        "tbl_master_email tbl_master_email_2 ON CAST(tbl_master_costCenter.cost_id AS nvarchar) = tbl_master_email_2.eml_cntId INNER JOIN  " +
                                        "tbl_master_email tbl_master_email_3 ON tbl_master_user.user_contactId = tbl_master_email_3.eml_cntId " +
                                        "INNER JOIN  tbl_master_email tbl_master_email_1 ON tbl_master_branch.branch_internalId = tbl_master_email_1.eml_cntId ",
                                        " tbl_master_user.user_name + ' <  ' + tbl_master_email_3.eml_email + '>' AS EmployId, " +
                                        "tbl_master_costCenter.cost_description + ' < ' + tbl_master_email_2.eml_email + '>' AS DepartmentId, " +
                                        "tbl_master_branch.branch_description + ' <  ' + tbl_master_email_1.eml_email + '>' AS BranchId ",
                                        " user_id = " + HttpContext.Current.Session["userid"] + " and tbl_master_email_3.eml_type='Official' ", 3); */
            //data = oDBEngine.GetFieldValue(" tbl_master_user,tbl_master_email ", " user_name + ' <  ' + eml_email + '>' AS EmployId ", " user_contactId=eml_cntId and eml_type='Official' and user_id = " + HttpContext.Current.Session["userid"], 1);
            data = oDBEngine.GetFieldValue("(select eml_email from tbl_master_email where eml_cntid =(select distinct b.cmp_internalid from tbl_trans_employeeCTC as a inner join tbl_master_company as b on a.emp_organization=b.cmp_id where emp_cntid='" + Session["UserContactid"].ToString() + "')and eml_type='Web Site'union select distinct eml_email from tbl_master_email where eml_cntId='" + Session["UserContactid"].ToString() + "' and eml_type='Official' union select distinct eml_email from tbl_master_email where eml_cntId='EMS0000004' and eml_type='Personal' union select distinct a.branch_cpemail from tbl_master_branch as a inner join tbl_trans_employeeCTC as b on a.branch_id=b.emp_branch where b.emp_cntid='" + Session["UserContactid"].ToString() + "') as c", "eml_email", null, 1);
            if (data[0, 0] != "n")
            {
                cmbFrom.Items.Clear();
                for (int i = 0; i < data.Length; i++)
                {
                    ListItem list = new ListItem();
                    list.Text = data[i, 0];
                    list.Value = data[i, 0];
                    //list.Value = i.ToString();
                    cmbFrom.Items.Add(list);
                }
            }


        }
        #region for servercall
        void ICallbackEventHandler.RaiseCallbackEvent(string eventArgument)
        {
            //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);MULTI
            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
            string id = eventArgument.ToString();
            string[] FieldWvalue = id.Split('~');
            string IDs = "";
            data = "";
            #region Combo
            if (FieldWvalue[0] == "Combo")
            {
                IDs = FieldWvalue[1];
                string listitems = "";
                string[,] dpVal = oDBEngine.GetFieldValue(" tbl_master_documentType ", " dty_id as Id, dty_documentType as Name ", " dty_applicableFor='" + IDs + "' ", 2, " dty_documentType ");
                for (int i = 0; i < dpVal.Length / 2; i++)
                {
                    if (listitems != "")
                        listitems += ";" + dpVal[i, 0] + "," + dpVal[i, 1];
                    else
                        listitems = dpVal[i, 0] + "," + dpVal[i, 1];
                }
                data = "Combo~" + listitems;
            }
            #endregion
            #region search
            if (FieldWvalue[0] == "search")
            {
                string[,] idstring;
                string id1 = "";
                switch (FieldWvalue[1])
                {
                    case "Products  MF":
                        idstring = oDBEngine.GetFieldValue(" tbl_master_products ", " prds_internalid", " prds_productType=12 and prds_description='" + FieldWvalue[4] + "'", 1);
                        id1 = idstring[0, 0];
                        break;
                    case "Products – Insurance":
                        idstring = oDBEngine.GetFieldValue(" tbl_master_products ", " prds_internalid", " prds_productType=13 and prds_description='" + FieldWvalue[4] + "'", 1);
                        id1 = idstring[0, 0];
                        break;
                    case "Products - IPOs":
                        idstring = oDBEngine.GetFieldValue(" tbl_master_products ", " prds_internalid", " prds_productType==15 and prds_description='" + FieldWvalue[4] + "'", 1);
                        id1 = idstring[0, 0];
                        break;
                    case "Customer":
                        idstring = oDBEngine.GetFieldValue(" tbl_master_products ", " prds_internalid", " prds_productType=1 and cnt_firstName='" + FieldWvalue[4] + "'", 1);
                        id1 = idstring[0, 0];
                        break;
                    case "Lead":
                        idstring = oDBEngine.GetFieldValue(" tbl_master_lead ", " cnt_internalid ", "cnt_firstName='" + FieldWvalue[4] + "'", 1);
                        id1 = idstring[0, 0];
                        break;
                    case "Employee":
                        idstring = oDBEngine.GetFieldValue(" tbl_master_contact ", " cnt_internalid ", " cnt_contactType=3 and cnt_firstName='" + FieldWvalue[4] + "'", 1);
                        id1 = idstring[0, 0];
                        break;
                    case "Sub Brokers":
                        idstring = oDBEngine.GetFieldValue(" tbl_master_contact ", " cnt_internalid ", " cnt_contactType=2 and cnt_firstName='" + FieldWvalue[4] + "'", 1);
                        id1 = idstring[0, 0];
                        break;
                    case "Franchisees":
                        idstring = oDBEngine.GetFieldValue(" tbl_master_contact ", " cnt_internalid ", " cnt_contactType=4 and cnt_firstName='" + FieldWvalue[4] + "'", 1);
                        id1 = idstring[0, 0];
                        break;
                    case "Data Vendors":
                        idstring = oDBEngine.GetFieldValue(" tbl_master_contact ", " cnt_internalid ", " cnt_contactType=7 and cnt_firstName='" + FieldWvalue[4] + "'", 1);
                        id1 = idstring[0, 0];
                        break;
                    case "Referral Agents":
                        idstring = oDBEngine.GetFieldValue(" tbl_master_contact ", " cnt_internalid ", " cnt_contactType=8 and cnt_firstName='" + FieldWvalue[4] + "'", 1);
                        id1 = idstring[0, 0];
                        break;
                    case "Recruitment Agents":
                        idstring = oDBEngine.GetFieldValue(" tbl_master_contact ", " cnt_internalid ", " cnt_contactType=9 and cnt_firstName='" + FieldWvalue[4] + "'", 1);
                        id1 = idstring[0, 0];
                        break;
                    case "AMCs":
                        idstring = oDBEngine.GetFieldValue(" tbl_master_AssetsManagementCompanies ", " amc_amcCode ", " amc_nameOfMutualFund='" + FieldWvalue[4] + "'", 1);
                        id1 = idstring[0, 0];
                        break;
                    case "Insurance Companies":
                        idstring = oDBEngine.GetFieldValue(" tbl_master_insurerName ", " insu_internalid ", " insu_nameOfCompany='" + FieldWvalue[4] + "'", 1);
                        id1 = idstring[0, 0];
                        break;
                    case "RTAs":
                        idstring = oDBEngine.GetFieldValue(" tbl_registrarTransferAgent ", " rta_rtaCode ", " rta_name='" + FieldWvalue[4] + "'", 1);
                        id1 = idstring[0, 0];
                        break;
                    case "Branches":
                        idstring = oDBEngine.GetFieldValue("  tbl_master_branch ", " branch_internalid ", " branch_description='" + FieldWvalue[4] + "'", 1);
                        id1 = idstring[0, 0];
                        break;
                    case "Companies":
                        idstring = oDBEngine.GetFieldValue(" tbl_master_company ", " cmp_internalId ", " cmp_Name='" + FieldWvalue[4] + "'", 1);
                        id1 = idstring[0, 0];
                        break;
                    case "Building":
                        idstring = oDBEngine.GetFieldValue(" tbl_master_building ", " bui_id ", " bui_Name='" + FieldWvalue[4] + "'", 1);
                        id1 = idstring[0, 0];
                        break;
                }
                if (id1 != "n")
                {
                    DT = oDBEngine.GetDataTable(" tbl_master_document INNER JOIN tbl_master_documentType ON tbl_master_document.doc_documentTypeId = tbl_master_documentType.dty_id INNER JOIN tbl_master_building ON tbl_master_document.doc_buildingId = tbl_master_building.bui_id ", " tbl_master_document.doc_id AS Id, tbl_master_documentType.dty_documentType AS Type, tbl_master_document.doc_documentName AS FileName, tbl_master_document.doc_source AS Src, COALESCE ('Building- ' + tbl_master_building.bui_Name + CHAR(13) + CHAR(10), '') + COALESCE ('/ Floor No : ' + tbl_master_document.doc_Floor + CHAR(13) + CHAR(10), '')  + '/ Room No-' + tbl_master_document.doc_RoomNo + '/ Cabinet No-' + tbl_master_document.doc_CellNo + '/ File No-' + tbl_master_document.doc_FileNo AS FilePath ", " doc_contactId =" + id1 + " and doc_documentTypeId = " + FieldWvalue[2]);
                    GridAttachment.DataSource = DT;
                    GridAttachment.DataBind();
                    if (DT.Rows.Count > 0)
                        data = "search~Y";
                    else
                        data = "search~N";

                }
                else
                {
                    DT = oDBEngine.GetDataTable(" tbl_master_document INNER JOIN tbl_master_documentType ON tbl_master_document.doc_documentTypeId = tbl_master_documentType.dty_id INNER JOIN tbl_master_building ON tbl_master_document.doc_buildingId = tbl_master_building.bui_id ", " tbl_master_document.doc_id AS Id, tbl_master_documentType.dty_documentType AS Type, tbl_master_document.doc_documentName AS FileName, tbl_master_document.doc_source AS Src, COALESCE ('Building- ' + tbl_master_building.bui_Name + CHAR(13) + CHAR(10), '') + COALESCE ('/ Floor No : ' + tbl_master_document.doc_Floor + CHAR(13) + CHAR(10), '')  + '/ Room No-' + tbl_master_document.doc_RoomNo + '/ Cabinet No-' + tbl_master_document.doc_CellNo + '/ File No-' + tbl_master_document.doc_FileNo AS FilePath ", " doc_documentName = '" + FieldWvalue[4] + "' and doc_documentTypeId=" + FieldWvalue[2]);
                    GridAttachment.DataSource = DT;
                    GridAttachment.DataBind();
                    if (DT.Rows.Count > 0)
                        data = "search~Y";
                    else
                        data = "search~N";
                }
            }
            #endregion

            #region template

            if (FieldWvalue[0] == "template")
            {
                string reciepient = "";
                string senderemail = "";
                string emlType = "";
                string[] emailtype = FieldWvalue[2].Split('^');
                if (emailtype[2] == "0")
                    emlType = "Official";
                if (emailtype[2] == "1")
                    emlType = "Department";
                if (emailtype[2] == "2")
                    emlType = "Breanch";
                if (FieldWvalue[1] != "all")
                {
                    int start = FieldWvalue[1].IndexOf('<') + 1;
                    int end = FieldWvalue[1].IndexOf('>') - 1;
                    //int start = FieldWvalue[2].IndexOf('<') + 1;
                    //int end = FieldWvalue[2].IndexOf('>') - 1;
                    string strid = FieldWvalue[1].ToString();

                    string[,] reci = oDBEngine.GetFieldValue(" tbl_master_email ", " eml_cntId  ", " eml_email='" + strid.Trim() + "' ", 1);
                    if (reci[0, 0] != "n")
                    {
                        reciepient = reci[0, 0];
                        string TypeIdentifier = reciepient.Substring(0, 2);
                        if (TypeIdentifier == "LD")
                        {
                            reci = oDBEngine.GetFieldValue(" tbl_master_lead left outer join tbl_master_salutation on sal_id=cnt_salutation ", "cnt_internalId, ( cnt_firstName +' '+cnt_middleName+' '+cnt_lastName) as name ", " cnt_internalId='" + reciepient + "'", 2);
                            string sal_name = oDBEngine.GetFieldValue(" tbl_master_lead left outer join tbl_master_salutation on sal_id=cnt_salutation ", "sal_name", " cnt_internalId='" + reciepient + "'", 1)[0, 0];
                            if (sal_name != "")
                            {
                                string reciepient_name = sal_name + '.' + reci[0, 1];
                            }
                            else
                            {
                                string reciepient_name = reci[0, 1];
                            }
                        }
                        else
                        {
                            reci = oDBEngine.GetFieldValue(" tbl_master_contact left outer join tbl_master_salutation on sal_id=cnt_salutation ", " cnt_internalId( cnt_firstName +' '+cnt_middleName+' '+cnt_lastName) as name ", " cnt_internalId='" + reciepient + "'", 2);
                            string sal_name = oDBEngine.GetFieldValue(" tbl_master_contact left outer join tbl_master_salutation on sal_id=cnt_salutation ", "sal_name", " cnt_internalId='" + reciepient + "'", 1)[0, 0];
                            if (sal_name != "")
                            {
                                string reciepient_name = sal_name + '.' + reci[0, 1];
                            }
                            else
                            {
                                string reciepient_name = reci[0, 1];
                            }

                        }

                    }
                    else
                    {
                        //data = "template~You Can not Use This Template Because, You don`t have E-mail ID for the respective template Type!";
                        return;
                    }
                }
                else
                    reciepient = "All";
                string[,] sender = oDBEngine.GetFieldValue(" tbl_master_email ", " eml_email  ", " eml_cntId  ='" + Session["UserContactid"].ToString() + "'", 1);
                // and eml_type='" + emlType + "'", 1);
                if (sender[0, 0] != "n")
                {
                    senderemail = sender[0, 0];
                }
                else
                {
                    data = "template~You Can not Use This Template Because, You don`t have E-mail ID for the respective template Type!";
                    return;
                }
                string[,] senderDetails = oDBEngine.GetFieldValue(" tbl_master_contact left outer join tbl_master_designation on deg_id=cnt_designation left outer join tbl_master_branch on branch_id=cnt_branchid left outer join tbl_master_salutation on sal_id=cnt_salutation left outer join tbl_master_phonefax on phf_cntId=cnt_internalId  ", " (sal_name +'.'+ cnt_firstName +' '+cnt_middleName+' '+cnt_lastName) as name,deg_designation,branch_description,cnt_organization,('+'+ phf_countryCode + '-' + phf_areaCode + '-' + phf_phoneNumber+'-'+phf_extension) as phone ", " cnt_internalId IN (Select user_contactId from tbl_master_user where user_id=" + HttpContext.Current.Session["userid"] + ") ", 5);

                Boolean flag = true;
                string firstpart = "";
                string secondpart = "";
                //string Finalmessage = "";
                string msg = emailtype[0];
                for (int i = 0; i < msg.Length; i++)
                {
                    if (flag == true)
                    {
                        if (msg[i] == '<')
                        {
                            flag = false;

                        }
                        else
                        {
                            firstpart += msg[i];
                        }
                    }
                    else
                    {
                        if (msg[i] == '>')
                        {
                            flag = true;
                            if (secondpart.Trim() == "Recipient Name")
                            {
                                firstpart += reciepient.Trim();

                            }
                            else if (secondpart.Trim() == "Sender Name")
                            {
                                firstpart += " " + senderDetails[0, 0] + "\n" + senderDetails[0, 1] + "\n" + senderDetails[0, 2] + "\n" + senderDetails[0, 3] + "\n";

                            }
                            else if (secondpart.Trim() == "Sender Number")
                            {
                                firstpart += " " + senderDetails[0, 4];

                            }
                            else
                            {
                                firstpart += "<" + secondpart + ">";
                            }
                            secondpart = "";
                        }
                        else
                        {
                            secondpart += msg[i];
                        }
                    }
                }
                //Finalmessage = firstpart;
                data = "template~Y~" + firstpart;
            }

            #endregion
            #region sendmail
            if (FieldWvalue[0] == "sendmail")
            {
                string sendermail = cmbFrom.Text.ToString();
                string AddressTo = txtUsermailIDs.Text;
                string subject = FieldWvalue[3].ToString();
                string Body = FieldWvalue[2].ToString();
                string template = FieldWvalue[6].ToString();
                //string attached = "D:/date.txt";
                string retrn = "";// objconverter.SendEmailWithAttachment(sendermail, AddressTo, subject, Body, (DataTable)Session["table"], template, FieldWvalue[4], FieldWvalue[5]);
                //string retrn = oDBEngine.SendMailByComposer(FieldWvalue[1], sendermail, (DataTable)Session["table"], FieldWvalue[3], FieldWvalue[2], FieldWvalue[4], FieldWvalue[5], FieldWvalue[6]);
                if (retrn == "Done")
                {
                    Session["table"] = null;
                    data = "sendmail~Y~Mail Delivered Successfully!";
                    DataTable Empty = new DataTable();
                    GridAttachmentLocal.DataSource = Empty.DefaultView;
                    GridAttachmentLocal.DataBind();

                }
                else
                    data = "sendmail~" + retrn;
                //}
                //else
                //{
                //    data = "sendmail~You Can not send mail Because, You don`t have Official E-mail ID !";

                //}

            }
            #endregion
            #region AttachLocal

            if (FieldWvalue[0] == "AttachLocal")
            {
                string path = Server.MapPath("../Documents") + "\\" + HttpContext.Current.Session["userid"].ToString();
                if (System.IO.Directory.Exists(path))
                {
                    uploadDocument(FieldWvalue[1]);
                }
                else
                {
                    System.IO.Directory.CreateDirectory(path);
                    uploadDocument(FieldWvalue[1]);
                }
            }

            #endregion
            #region LocalCall
            if (FieldWvalue[0] == "LocalCall")
            {
                Session["mode"] = "LocalCall";
            }
            #endregion
            #region ServerCall
            if (FieldWvalue[0] == "ServerCall")
            {
                Session["mode"] = "ServerCall";
            }
            #endregion

        }


        string ICallbackEventHandler.GetCallbackResult()
        {
            return data;

        }
        #endregion
        protected void GridAttachment_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);MULTI
            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
            string datalist = e.Parameters.ToString();
            string[] FieldWvalue = datalist.Split('~');
            string[,] idstring;
            string id1 = "";
            if (FieldWvalue[0] == "search")
            {
                switch (FieldWvalue[1])
                {
                    case "Products  MF":
                        idstring = oDBEngine.GetFieldValue(" tbl_master_products ", " prds_internalid", " prds_productType=12 and prds_description='" + FieldWvalue[4] + "'", 1);
                        id1 = idstring[0, 0];
                        break;
                    case "Products – Insurance":
                        idstring = oDBEngine.GetFieldValue(" tbl_master_products ", " prds_internalid", " prds_productType=13 and prds_description='" + FieldWvalue[4] + "'", 1);
                        id1 = idstring[0, 0];
                        break;
                    case "Products - IPOs":
                        idstring = oDBEngine.GetFieldValue(" tbl_master_products ", " prds_internalid", " prds_productType==15 and prds_description='" + FieldWvalue[4] + "'", 1);
                        id1 = idstring[0, 0];
                        break;
                    case "Customer":
                        idstring = oDBEngine.GetFieldValue(" tbl_master_products ", " prds_internalid", " prds_productType=1 and cnt_firstName='" + FieldWvalue[4] + "'", 1);
                        id1 = idstring[0, 0];
                        break;
                    case "Lead":
                        idstring = oDBEngine.GetFieldValue(" tbl_master_lead ", " cnt_internalid ", "cnt_firstName='" + FieldWvalue[4] + "'", 1);
                        id1 = idstring[0, 0];
                        break;
                    case "Employee":
                        idstring = oDBEngine.GetFieldValue(" tbl_master_contact ", " cnt_internalid ", " cnt_contactType=3 and cnt_firstName='" + FieldWvalue[4] + "'", 1);
                        id1 = idstring[0, 0];
                        break;
                    case "Sub Brokers":
                        idstring = oDBEngine.GetFieldValue(" tbl_master_contact ", " cnt_internalid ", " cnt_contactType=2 and cnt_firstName='" + FieldWvalue[4] + "'", 1);
                        id1 = idstring[0, 0];
                        break;
                    case "Franchisees":
                        idstring = oDBEngine.GetFieldValue(" tbl_master_contact ", " cnt_internalid ", " cnt_contactType=4 and cnt_firstName='" + FieldWvalue[4] + "'", 1);
                        id1 = idstring[0, 0];
                        break;
                    case "Data Vendors":
                        idstring = oDBEngine.GetFieldValue(" tbl_master_contact ", " cnt_internalid ", " cnt_contactType=7 and cnt_firstName='" + FieldWvalue[4] + "'", 1);
                        id1 = idstring[0, 0];
                        break;
                    case "Referral Agents":
                        idstring = oDBEngine.GetFieldValue(" tbl_master_contact ", " cnt_internalid ", " cnt_contactType=8 and cnt_firstName='" + FieldWvalue[4] + "'", 1);
                        id1 = idstring[0, 0];
                        break;
                    case "Recruitment Agents":
                        idstring = oDBEngine.GetFieldValue(" tbl_master_contact ", " cnt_internalid ", " cnt_contactType=9 and cnt_firstName='" + FieldWvalue[4] + "'", 1);
                        id1 = idstring[0, 0];
                        break;
                    case "AMCs":
                        idstring = oDBEngine.GetFieldValue(" tbl_master_AssetsManagementCompanies ", " amc_amcCode ", " amc_nameOfMutualFund='" + FieldWvalue[4] + "'", 1);
                        id1 = idstring[0, 0];
                        break;
                    case "Insurance Companies":
                        idstring = oDBEngine.GetFieldValue(" tbl_master_insurerName ", " insu_internalid ", " insu_nameOfCompany='" + FieldWvalue[4] + "'", 1);
                        id1 = idstring[0, 0];
                        break;
                    case "RTAs":
                        idstring = oDBEngine.GetFieldValue(" tbl_registrarTransferAgent ", " rta_rtaCode ", " rta_name='" + FieldWvalue[4] + "'", 1);
                        id1 = idstring[0, 0];
                        break;
                    case "Branches":
                        idstring = oDBEngine.GetFieldValue("  tbl_master_branch ", " branch_internalid ", " branch_description='" + FieldWvalue[4] + "'", 1);
                        id1 = idstring[0, 0];
                        break;
                    case "Companies":
                        idstring = oDBEngine.GetFieldValue(" tbl_master_company ", " cmp_internalId ", " cmp_Name='" + FieldWvalue[4] + "'", 1);
                        id1 = idstring[0, 0];
                        break;
                    case "Building":
                        idstring = oDBEngine.GetFieldValue(" tbl_master_building ", " bui_id ", " bui_Name='" + FieldWvalue[4] + "'", 1);
                        id1 = idstring[0, 0];
                        break;
                }
                if (id1 != "n")
                {
                    DT = oDBEngine.GetDataTable(" tbl_master_document INNER JOIN tbl_master_documentType ON tbl_master_document.doc_documentTypeId = tbl_master_documentType.dty_id INNER JOIN tbl_master_building ON tbl_master_document.doc_buildingId = tbl_master_building.bui_id ", " tbl_master_document.doc_id AS Id, tbl_master_documentType.dty_documentType AS Type, tbl_master_document.doc_documentName AS FileName, tbl_master_document.doc_source AS FilePath, COALESCE ('Building- ' + tbl_master_building.bui_Name + CHAR(13) + CHAR(10), '') + COALESCE ('/ Floor No : ' + tbl_master_document.doc_Floor + CHAR(13) + CHAR(10), '')  + '/ Room No-' + tbl_master_document.doc_RoomNo + '/ Cabinet No-' + tbl_master_document.doc_CellNo + '/ File No-' + tbl_master_document.doc_FileNo AS FilePath ", " doc_contactId =" + id1 + " and doc_documentTypeId = " + FieldWvalue[2]);
                    GridAttachment.DataSource = DT;
                    GridAttachment.DataBind();
                    if (DT.Rows.Count > 0)
                        data = "search~Y";
                    else
                        data = "search~N";

                }
                else
                {
                    DT = oDBEngine.GetDataTable(" tbl_master_document INNER JOIN tbl_master_documentType ON tbl_master_document.doc_documentTypeId = tbl_master_documentType.dty_id INNER JOIN tbl_master_building ON tbl_master_document.doc_buildingId = tbl_master_building.bui_id ", " tbl_master_document.doc_id AS Id, tbl_master_documentType.dty_documentType AS Type, tbl_master_document.doc_documentName AS FileName, tbl_master_document.doc_source AS FilePath, COALESCE ('Building- ' + tbl_master_building.bui_Name + CHAR(13) + CHAR(10), '') + COALESCE ('/ Floor No : ' + tbl_master_document.doc_Floor + CHAR(13) + CHAR(10), '')  + '/ Room No-' + tbl_master_document.doc_RoomNo + '/ Cabinet No-' + tbl_master_document.doc_CellNo + '/ File No-' + tbl_master_document.doc_FileNo AS FilePath ", " doc_documentName = '" + FieldWvalue[4] + "' and doc_documentTypeId=" + FieldWvalue[2]);
                    GridAttachment.DataSource = DT;
                    GridAttachment.DataBind();
                    if (DT.Rows.Count > 0)
                        data = "search~Y";
                    else
                        data = "search~N";
                }
                Session["table"] = DT;
            }
            if (FieldWvalue[0] == "cancel")
            {
                GridAttachment.CancelEdit();
            }
        }
        protected void GridAttachmentLocal_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            string datalist = e.Parameters.ToString();
            string[] FieldWvalue = datalist.Split('~');

            if (FieldWvalue[0] == "remvloc")        //___Remove Upload
            {
                string[] FileLocationIDs = FieldWvalue[1].ToString().Split(',');
                DataTable DTLocalFiles = (DataTable)Session["table"];
                for (int i = 0; i < DTLocalFiles.Rows.Count; i++)
                {
                    for (int j = 0; j < FileLocationIDs.Length; j++)
                    {
                        if (DTLocalFiles.Rows[i]["filepathServer"].ToString().Trim() == FileLocationIDs[j].ToString().Trim())
                        {
                            File.Delete(FileLocationIDs[j].ToString().Trim());
                            DTLocalFiles.Rows[i].Delete();
                        }
                    }
                }
                GridAttachmentLocal.DataSource = DTLocalFiles.DefaultView;
                GridAttachmentLocal.DataBind();
            }
            if (FieldWvalue[0] == "Canloc")     //___Cancel Upload
            {
                DataTable DTLocalFiles = (DataTable)Session["table"];
                for (int i = 0; i < DTLocalFiles.Rows.Count; i++)
                {
                    File.Delete(DTLocalFiles.Rows[i]["filepathServer"].ToString().Trim());
                }
                DTLocalFiles.Rows.Clear();
                GridAttachmentLocal.DataSource = DTLocalFiles.DefaultView;
                GridAttachmentLocal.DataBind();
            }
        }
        private string uploadDocument(string path)
        {
            string FLocation1 = "";
            string FName = Path.GetFileName(path);
            if (FName != "")
            {
                string FLocation = Server.MapPath("../Documents/E-mail_Attacthments/") + Session.SessionID + "_" + FName;
                FLocation1 = FLocation;
                FileUpload1.PostedFile.SaveAs(FLocation);
            }
            return FLocation1;
        }
        protected void Button1_Click(object sender, EventArgs e)
        {
            Session["mode"] = "LocalCall";
            string paths = "";
            //string path = Server.MapPath("../Documents") + "\\" + HttpContext.Current.Session["userid"].ToString();
            string path = Server.MapPath("../Documents") + "\\E-mail_Attacthments";
            if (System.IO.Directory.Exists(path))
            {
                paths = uploadDocument(FileUpload1.PostedFile.FileName);
            }
            else
            {
                System.IO.Directory.CreateDirectory(path);
                paths = uploadDocument(FileUpload1.PostedFile.FileName);
            }
            fillGridLocal(paths);
        }

        private void fillGridLocal(string path)
        {
            if (Session["table"] != null)
            {
                //DataTable DT = new DataTable();
                DT = (DataTable)Session["table"];
                int lenghth = DT.Rows.Count;
                DataRow DR = DT.NewRow();
                DR["fileid"] = lenghth + 1;
                DR["filename"] = FileUpload1.FileName;
                DR["filepath"] = FileUpload1.PostedFile.FileName;
                DR["filepathServer"] = path;

                DT.Rows.Add(DR);
            }
            else
            {
                ///DataTable DT = new DataTable();
                DataColumn DC1 = new DataColumn("fileid");
                DataColumn DC2 = new DataColumn("filename");
                DataColumn DC3 = new DataColumn("filepath");
                DataColumn DC4 = new DataColumn("filepathServer");
                DT.Columns.Add(DC1);
                DT.Columns.Add(DC2);
                DT.Columns.Add(DC3);
                DT.Columns.Add(DC4);

                DataRow DR = DT.NewRow();
                DR["fileid"] = 1;
                DR["filename"] = FileUpload1.FileName;
                DR["filepath"] = FileUpload1.PostedFile.FileName;
                DR["filepathServer"] = path;

                DT.Rows.Add(DR);
            }
            Session["table"] = DT;

            GridAttachmentLocal.DataSource = DT;
            GridAttachmentLocal.DataBind();
        }

        //protected void Button2_Click(object sender, EventArgs e)
        //{
        //    objconverter.SendMail("indira.bhatta@gmail.com", "influxcrm@gmail.com", "test", "<table><tr><td>test</td></tr></table>");
        //}
        protected void Button2_Click(object sender, EventArgs e)
        {
            objconverter.SendMail("noreply.gmail.com", "indira@influxerp.com", "Hello", "Hi");
        }
    }
}