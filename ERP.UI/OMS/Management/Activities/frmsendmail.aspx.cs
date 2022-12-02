using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogicLayer;
using ClsDropDownlistNameSpace;
namespace ERP.OMS.Management.Activities
{
    public partial class management_activities_frmsendmail : System.Web.UI.Page, System.Web.UI.ICallbackEventHandler
    {
        string data = "";
        DataTable DT = new DataTable();
        clsDropDownList clsdropdown = new clsDropDownList();
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        
        clsDropDownList clsdrp = new clsDropDownList();
        Converter objConverter = new Converter();
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
            if (HttpContext.Current.Session["userlastsegment"].ToString() == "4")
            {
                rdbContact.Visible = false;
                lblcnt.Visible = false;


            }

            if (HttpContext.Current.Session["userid"] == null)
            {
               //Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
            //this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");
            if (!IsPostBack)
            {
                Session["table"] = null;
                Session["table1"] = null;
                Session["mode"] = "";
                lblHeader.Text = "Compose Mail ";
                //________This script is for firing javascript when page load first___//
                if (!ClientScript.IsStartupScriptRegistered("Today"))
                    ClientScript.RegisterStartupScript(typeof(Page), "Today", "<script>AtTheTimePageLoad();</script>");
                //______________________________End Script____________________________//

                FillCombo();
                // fillgroupCombo();
                txtUserId.Attributes.Add("onkeyup", "callAjax(this,'GetMailId',event)");

                //drpDocumentEntity.Attributes.Add("onchange", "callDoc()");
                cmbTemplate.Attributes.Add("onchange", "callTemplateChange()");
                //txtName.Attributes.Add("onkeyup", "callAjaxDoc(this,'searchdocument',event)");
                txtGroup.Attributes.Add("onkeyup", "callAjaxGroup(this,'GetGroupName',event)");
                rdbContact.Attributes.Add("onclick", "contactChecked()");
                rdbSpecific.Attributes.Add("onclick", "specificChecked()");
                rdbGroup.Attributes.Add("onclick", "GroupChecked()");
                Session["table"] = null;
                ASPxNextDate.Value = Convert.ToDateTime(oDBEngine.GetDate().ToString());
                ASPxNextDate.EditFormatString = objConverter.GetDateFormat("DateTime");
                ClientScript.RegisterStartupScript(typeof(Page), "jscript1", "<script>PageLoad();</script>");
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
            //if(Session["ServerPath"]!=null)
            //{
            //    lblServerFile.Text = Session["ServerPath"].ToString();
            //    lblServerFile.Visible = true;
            //}
        }
        //public void Call_CheckPageName(string URL)
        //{
        //    //'http://localhost:2957/InfluxCRM/management/testProjectMainPage_employee.aspx'
        //    string sPath = HttpContext.Current.Request.Url.ToString();
        //   // Call_CheckPageName(sPath);
        //    string[] PageName = sPath.ToString().Split('/');
        //    if (PageName[4] != "SignOff.aspx")
        //    {
        //        DataTable dt = oDBEngine.GetDataTable(" tbl_trans_menu ", "mnu_id ", " mnu_menuLink like '%" + PageName[5] + "%' and mnu_segmentid='" + HttpContext.Current.Session["userlastsegment"] + "'");
        //        string menuId = dt.Rows[0]["mnu_id"].ToString();

        //    }

        //}
        private void FillCombo()
        {
            //string[,] data = oDBEngine.GetFieldValue(" tbl_master_contactType ", " cnt_prefix,cnttpy_contactType ", null, 2, " cnttpy_contactType ");
            //oDBEngine.AddDataToDropDownList(data, cmbContactType);
            if (HttpContext.Current.Session["userlastsegment"].ToString() == "4")
            {
                string[,] r = new string[1, 2];
                r[0, 0] = "EM";
                r[0, 1] = "Employees";
                clsdropdown.AddDataToDropDownList(r, cmbContactType);
            }
            else
            {
                string[,] r = new string[9, 2];
                r[0, 0] = "EM";
                r[0, 1] = "Employees";
                r[1, 0] = "CL";
                r[1, 1] = "Customers";
                r[2, 0] = "LD";
                r[2, 1] = "Lead";
                r[3, 0] = "CD";
                r[3, 1] = "CDSL Client";
                r[4, 0] = "ND";
                r[4, 1] = "NSDL Client";
                r[5, 0] = "BP";
                r[5, 1] = "Business Partne";
                r[6, 0] = "RA";
                r[6, 1] = "Relationship Partner";
                r[7, 0] = "SB";
                r[7, 1] = "Sub Broker";
                r[8, 0] = "FR";
                r[8, 1] = "Franchisees";
                //oDBEngine.AddDataToDropDownList(r, cmbContactType);
                clsdrp.AddDataToDropDownList(r, cmbContactType);
            }


            string[,] data = oDBEngine.GetFieldValue(" tbl_master_template ", " tem_msg+'^'+IsNull(cast(tem_id as nvarchar),'')+'^'+IsNull(cast(tem_sendertype as nvarchar),'') as MSG,tem_shortmsg ", " tem_type=2 ", 2, " tem_shortmsg ");
            //oDBEngine.AddDataToDropDownList(data, cmbTemplate, true);
            clsdrp.AddDataToDropDownList(data, cmbTemplate, true);
            data = oDBEngine.GetFieldValue(" tbl_master_user,tbl_master_email ", " user_name + ' <  ' + eml_email + '>' AS EmployId ", " user_contactId=eml_cntId and eml_type='Official' and user_id = " + HttpContext.Current.Session["userid"], 1);
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
            data = oDBEngine.GetFieldValue(" tbl_master_contactType ", " RTRIM(cnt_prefix),cnttpy_contactType", null, 2, " cnttpy_id ");
            //oDBEngine.AddDataToDropDownList(data, drpContactWise);
            clsdrp.AddDataToDropDownList(data, drpContactWise);

        }
        //private void fillgroupCombo()
        //{
        //    string[,] data = oDBEngine.GetFieldValue(" tbl_master_groupMaster ", "distinct gpm_Type as ID, gpm_Type as Name", null, 2, " gpm_Type ");
        //    //oDBEngine.AddDataToDropDownList(data, cmbContactType, true);
        //    oDBEngine.AddDataToDropDownList(data, drpGroupWise);
        //}
        #region for servercall
        void ICallbackEventHandler.RaiseCallbackEvent(string eventArgument)
        {
           // DBEngine oDBEngine = new DBEngine(ConfigurationSSettings.AppSettings["DBConnectionDefault"]); MULTI

            DBEngine oDBEngine = new DBEngine();

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
            //#region search
            //if (FieldWvalue[0] == "search")
            //{
            //    string[,] idstring;
            //    string id1 = "";
            //    switch (FieldWvalue[1])
            //    {
            //        case "Products  MF":
            //            idstring = oDBEngine.GetFieldValue(" tbl_master_products ", " prds_internalid", " prds_productType=12 and prds_description='" + FieldWvalue[4] + "'", 1);
            //            id1 = idstring[0, 0];
            //            break;
            //        case "Products – Insurance":
            //            idstring = oDBEngine.GetFieldValue(" tbl_master_products ", " prds_internalid", " prds_productType=13 and prds_description='" + FieldWvalue[4] + "'", 1);
            //            id1 = idstring[0, 0];
            //            break;
            //        case "Products - IPOs":
            //            idstring = oDBEngine.GetFieldValue(" tbl_master_products ", " prds_internalid", " prds_productType==15 and prds_description='" + FieldWvalue[4] + "'", 1);
            //            id1 = idstring[0, 0];
            //            break;
            //        case "Customer":
            //            idstring = oDBEngine.GetFieldValue(" tbl_master_products ", " prds_internalid", " prds_productType=1 and cnt_firstName='" + FieldWvalue[4] + "'", 1);
            //            id1 = idstring[0, 0];
            //            break;
            //        case "Lead":
            //            idstring = oDBEngine.GetFieldValue(" tbl_master_lead ", " cnt_internalid ", "cnt_firstName='" + FieldWvalue[4] + "'", 1);
            //            id1 = idstring[0, 0];
            //            break;
            //        case "Employee":
            //            idstring = oDBEngine.GetFieldValue(" tbl_master_contact ", " cnt_internalid ", " cnt_contactType=3 and cnt_firstName='" + FieldWvalue[4] + "'", 1);
            //            id1 = idstring[0, 0];
            //            break;
            //        case "Sub Brokers":
            //            idstring = oDBEngine.GetFieldValue(" tbl_master_contact ", " cnt_internalid ", " cnt_contactType=2 and cnt_firstName='" + FieldWvalue[4] + "'", 1);
            //            id1 = idstring[0, 0];
            //            break;
            //        case "Franchisees":
            //            idstring = oDBEngine.GetFieldValue(" tbl_master_contact ", " cnt_internalid ", " cnt_contactType=4 and cnt_firstName='" + FieldWvalue[4] + "'", 1);
            //            id1 = idstring[0, 0];
            //            break;
            //        case "Data Vendors":
            //            idstring = oDBEngine.GetFieldValue(" tbl_master_contact ", " cnt_internalid ", " cnt_contactType=7 and cnt_firstName='" + FieldWvalue[4] + "'", 1);
            //            id1 = idstring[0, 0];
            //            break;
            //        case "Referral Agents":
            //            idstring = oDBEngine.GetFieldValue(" tbl_master_contact ", " cnt_internalid ", " cnt_contactType=8 and cnt_firstName='" + FieldWvalue[4] + "'", 1);
            //            id1 = idstring[0, 0];
            //            break;
            //        case "Recruitment Agents":
            //            idstring = oDBEngine.GetFieldValue(" tbl_master_contact ", " cnt_internalid ", " cnt_contactType=9 and cnt_firstName='" + FieldWvalue[4] + "'", 1);
            //            id1 = idstring[0, 0];
            //            break;
            //        case "AMCs":
            //            idstring = oDBEngine.GetFieldValue(" tbl_master_AssetsManagementCompanies ", " amc_amcCode ", " amc_nameOfMutualFund='" + FieldWvalue[4] + "'", 1);
            //            id1 = idstring[0, 0];
            //            break;
            //        case "Insurance Companies":
            //            idstring = oDBEngine.GetFieldValue(" tbl_master_insurerName ", " insu_internalid ", " insu_nameOfCompany='" + FieldWvalue[4] + "'", 1);
            //            id1 = idstring[0, 0];
            //            break;
            //        case "RTAs":
            //            idstring = oDBEngine.GetFieldValue(" tbl_registrarTransferAgent ", " rta_rtaCode ", " rta_name='" + FieldWvalue[4] + "'", 1);
            //            id1 = idstring[0, 0];
            //            break;
            //        case "Branches":
            //            idstring = oDBEngine.GetFieldValue("  tbl_master_branch ", " branch_internalid ", " branch_description='" + FieldWvalue[4] + "'", 1);
            //            id1 = idstring[0, 0];
            //            break;
            //        case "Companies":
            //            idstring = oDBEngine.GetFieldValue(" tbl_master_company ", " cmp_internalId ", " cmp_Name='" + FieldWvalue[4] + "'", 1);
            //            id1 = idstring[0, 0];
            //            break;
            //        case "Building":
            //            idstring = oDBEngine.GetFieldValue(" tbl_master_building ", " bui_id ", " bui_Name='" + FieldWvalue[4] + "'", 1);
            //            id1 = idstring[0, 0];
            //            break;
            //    }
            //    if (id1 != "n")
            //    {
            //        DT = oDBEngine.GetDataTable(" tbl_master_document INNER JOIN tbl_master_documentType ON tbl_master_document.doc_documentTypeId = tbl_master_documentType.dty_id INNER JOIN tbl_master_building ON tbl_master_document.doc_buildingId = tbl_master_building.bui_id ", " tbl_master_document.doc_id AS Id, tbl_master_documentType.dty_documentType AS Type, tbl_master_document.doc_documentName AS FileName, tbl_master_document.doc_source AS Src, COALESCE ('Building- ' + tbl_master_building.bui_Name + CHAR(13) + CHAR(10), '') + COALESCE ('/ Floor No : ' + tbl_master_document.doc_Floor + CHAR(13) + CHAR(10), '')  + '/ Room No-' + tbl_master_document.doc_RoomNo + '/ Cabinet No-' + tbl_master_document.doc_CellNo + '/ File No-' + tbl_master_document.doc_FileNo AS FilePath ", " doc_contactId =" + id1 + " and doc_documentTypeId = " + FieldWvalue[2]);
            //        GridAttachment.DataSource = DT;
            //        GridAttachment.DataBind();
            //        if (DT.Rows.Count > 0)
            //            data = "search~Y";
            //        else
            //            data = "search~N";

            //    }
            //    else
            //    {
            //        DT = oDBEngine.GetDataTable(" tbl_master_document INNER JOIN tbl_master_documentType ON tbl_master_document.doc_documentTypeId = tbl_master_documentType.dty_id INNER JOIN tbl_master_building ON tbl_master_document.doc_buildingId = tbl_master_building.bui_id ", " tbl_master_document.doc_id AS Id, tbl_master_documentType.dty_documentType AS Type, tbl_master_document.doc_documentName AS FileName, tbl_master_document.doc_source AS Src, COALESCE ('Building- ' + tbl_master_building.bui_Name + CHAR(13) + CHAR(10), '') + COALESCE ('/ Floor No : ' + tbl_master_document.doc_Floor + CHAR(13) + CHAR(10), '')  + '/ Room No-' + tbl_master_document.doc_RoomNo + '/ Cabinet No-' + tbl_master_document.doc_CellNo + '/ File No-' + tbl_master_document.doc_FileNo AS FilePath ", " doc_documentName = '" + FieldWvalue[4] + "' and doc_documentTypeId=" + FieldWvalue[2]);
            //        GridAttachment.DataSource = DT;
            //        GridAttachment.DataBind();
            //        if (DT.Rows.Count > 0)
            //            data = "search~Y";
            //        else
            //            data = "search~N";
            //    }
            //}
            //#endregion

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
                    string strid = "";

                    if (Convert.ToString(FieldWvalue[1]).Contains("<") && Convert.ToString(FieldWvalue[1]).Contains(">"))
                    {
                        strid = FieldWvalue[1].Substring(start, end - start + 1);
                    }
                    else
                    {
                        strid = Convert.ToString(FieldWvalue[1]);
                    }

                    string[,] reci = oDBEngine.GetFieldValue(" tbl_master_email ", " eml_cntId  ", " eml_email='" + strid.Trim() + "' ", 1);
                    if (reci[0, 0] != "n")
                    {
                        reciepient = reci[0, 0];
                        reci = oDBEngine.GetFieldValue(" tbl_master_contact, tbl_master_salutation ", " (sal_name +'.'+ cnt_firstName +' '+cnt_middleName+' '+cnt_lastName) as name ", " cnt_internalId='" + reciepient + "' and sal_id=cnt_salutation", 1);
                        reciepient = reci[0, 0];
                    }
                    else
                    {
                        //data = "template~You Can not Use This Template Because, You don`t have E-mail ID for the respective template Type!";
                        return;
                    }
                }
                else
                    reciepient = "All";
                string[,] sender = oDBEngine.GetFieldValue(" tbl_master_email ", " eml_email  ", " eml_cntId in (select user_contactId from tbl_master_user where user_id=" + HttpContext.Current.Session["userid"] + ") and eml_type='" + emlType + "'", 1);
                if (sender[0, 0] != "n")
                {
                    senderemail = sender[0, 0];
                }
                else
                {
                    data = "template~You Can not Use This Template Because, You don`t have E-mail ID for the respective template Type!";
                    return;
                }
                string[,] senderDetails = oDBEngine.GetFieldValue(" tbl_master_contact,tbl_master_designation,tbl_master_branch,tbl_master_salutation,tbl_master_phonefax ", " (sal_name +'.'+ cnt_firstName +' '+cnt_middleName+' '+cnt_lastName) as name,deg_designation,branch_description,cnt_organization,('+'+ phf_countryCode + '-' + phf_areaCode + '-' + phf_phoneNumber+'-'+phf_extension) as phone ", " cnt_internalId IN (Select user_contactId from tbl_master_user where user_id=" + HttpContext.Current.Session["userid"] + ") and deg_id=cnt_designation and sal_id=cnt_salutation and branch_id=cnt_branchid and phf_cntId=cnt_internalId ", 5);

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
                string datediff = FieldWvalue[8].ToString();
                //if(datediff!="")
                //{
                //String[] dated = datediff.ToString().Split('-');
                //string dd = dated[0].ToString();
                //string mm = dated[1].ToString();
                //string timesp = dated[2].ToString();
                ////string dtsp = datediff[0].ToString();
                //string dttt = Convert.ToDateTime(oDBEngine.GetDate()).ToString();
                //string tms = timesp.Substring(4, 7);
                //string yys = timesp.Substring(0, 4);
                //string OrigDate=yys + "-" + mm +"-" + dd +" "+tms;
                //}
                DataTable DTFiles = (DataTable)Session["table"];
                DataTable DTSFiles = (DataTable)Session["table1"];
                string atchflile;
                if (DTFiles != null || DTSFiles != null)
                {
                    atchflile = "Y";
                    //if (DTFiles.Rows.Count > 0 || DTSFiles.Rows.Count > 0)
                    //{
                    //    atchflile = "Y";

                    //}
                    //else
                    //{
                    //    atchflile = "N";
                    //}
                }
                else
                {
                    atchflile = "N";
                }
                string contactID = HttpContext.Current.Session["usercontactID"].ToString();
                DataTable dtcmp = oDBEngine.GetDataTable(" tbl_master_company  ", "*", "cmp_id=(select emp_organization from tbl_trans_employeectc where emp_cntId='" + contactID + "' and emp_effectiveuntil is null)");
                string cmpintid = dtcmp.Rows[0]["cmp_internalid"].ToString();
                DataTable dtsg = oDBEngine.GetDataTable(" tbl_master_segment  ", "*", "seg_id='" + HttpContext.Current.Session["userlastsegment"].ToString() + "'");
                string segmentname = dtsg.Rows[0]["seg_name"].ToString();
                string sPath = HttpContext.Current.Request.Url.ToString();
                string[] PageName = sPath.ToString().Split('/');
                DataTable dt = oDBEngine.GetDataTable(" tbl_trans_menu ", "mnu_id ", " mnu_menuLink like '%" + PageName[5] + "%' and mnu_segmentid='" + HttpContext.Current.Session["userlastsegment"] + "'");
                string menuId = dt.Rows[0]["mnu_id"].ToString();
                string[,] sender = oDBEngine.GetFieldValue(" tbl_master_email ", " eml_email  ", " eml_cntId in (select user_contactId from tbl_master_user where user_id=" + HttpContext.Current.Session["userid"] + ") and eml_type='Official'", 1);
                if (sender[0, 0] != "n")
                {
                    string senderemail = sender[0, 0];
                    // string fValues = "'" + senderemail + "','" + FieldWvalue[3] + "','" + FieldWvalue[2] + "','" + atchflile + "','" + menuId + "','" + Session["userid"] + "','" + Convert.ToDateTime(DateTime.Today) + "'";
                    try
                    {
                        //oDBEngine.InsurtFieldValue("Trans_Emails", "Emails_SenderEmailID,Emails_Subject,Emails_Content,Emails_HasAttachement,Emails_CreateApplication,Emails_CreateUser,Emails_CreateDateTime", fValues);

                       // ConfigurationSSettings.AppSettings["DBConnectionDefault"];  MULTI

                        String con = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);


                        SqlConnection lcon = new SqlConnection(con);
                        lcon.Open();
                        SqlCommand lcmdEmplInsert = new SqlCommand("InsertTransEmail", lcon);
                        lcmdEmplInsert.CommandType = CommandType.StoredProcedure;
                        lcmdEmplInsert.Parameters.AddWithValue("@Emails_SenderEmailID", senderemail);
                        lcmdEmplInsert.Parameters.AddWithValue("@Emails_Subject", FieldWvalue[3]);
                        lcmdEmplInsert.Parameters.AddWithValue("@Emails_Content", FieldWvalue[2]);
                        lcmdEmplInsert.Parameters.AddWithValue("@Emails_HasAttachement", atchflile);
                        lcmdEmplInsert.Parameters.AddWithValue("@Emails_CreateApplication", menuId);
                        lcmdEmplInsert.Parameters.AddWithValue("@Emails_CreateUser", Session["userid"]);
                        lcmdEmplInsert.Parameters.AddWithValue("@Emails_Type", "R");
                        lcmdEmplInsert.Parameters.AddWithValue("@Emails_CompanyID", cmpintid);
                        lcmdEmplInsert.Parameters.AddWithValue("@Emails_Segment", segmentname);
                        SqlParameter parameter = new SqlParameter("@result", SqlDbType.BigInt);
                        parameter.Direction = ParameterDirection.Output;
                        lcmdEmplInsert.Parameters.Add(parameter);
                        lcmdEmplInsert.ExecuteNonQuery();
                        // Mantis Issue 24802
                        if (lcon.State == ConnectionState.Open)
                        {
                            lcon.Close();
                        }
                        // End of Mantis Issue 24802
                        string InternalID = parameter.Value.ToString();
                        if (DTFiles != null)
                        {
                            for (int i = 0; i < DTFiles.Rows.Count; i++)
                            {
                                string filepath = DTFiles.Rows[i]["filepathServer"].ToString().Trim();
                                string[] fpathsd = filepath.Split('\\');
                                int P = fpathsd.Length;
                                string pathNew = fpathsd[P - 3].Trim() + "\\" + fpathsd[P - 2].Trim() + "\\" + fpathsd[P - 1].Trim();
                                string fValues1 = "'" + InternalID + "','" + pathNew + "'";
                                oDBEngine.InsurtFieldValue("Trans_EmailAttachment", "EmailAttachment_MainID,EmailAttachment_Path", fValues1);
                            }
                        }

                        if (DTSFiles != null)
                        {
                            for (int i = 0; i < DTSFiles.Rows.Count; i++)
                            {
                                string filepath = DTSFiles.Rows[i]["Serverfilename"].ToString().Trim();
                                //string[] fpathsd = filepath.Split('\\');
                                //int P = fpathsd.Length;
                                //string pathNew = fpathsd[P - 3].Trim() + "\\" + fpathsd[P - 2].Trim() + "\\" + fpathsd[P - 1].Trim();
                                string fValues1 = "'" + InternalID + "','" + filepath + "'";
                                oDBEngine.InsurtFieldValue("Trans_EmailAttachment", "EmailAttachment_MainID,EmailAttachment_Path", fValues1);
                            }
                        }

                        //  ###########---recipients-----------------

                        string[] idWlist = FieldWvalue[7].Split(',');
                        string mailaddress = "";
                        string mailname = "";
                        for (int i = 0; i < idWlist.Length; i++)
                        {
                            string[] idWname = idWlist[i].Split('-');
                            if (idWname.Length == 1)
                            {
                                mailaddress = idWname[0];
                                mailname = "";
                            }
                            else
                            {
                                mailaddress = idWname[1].Substring(0, idWname[1].Length);
                                mailname = idWname[0].Trim();
                                string cnttype = idWname[2].Trim();
                                if (datediff == "")
                                {
                                    string fValues2 = "'" + InternalID + "','" + mailaddress + "','" + mailname + "','" + cnttype + "','" + Convert.ToDateTime(oDBEngine.GetDate()) + "','" + "P" + "'";
                                    oDBEngine.InsurtFieldValue("Trans_EmailRecipients", "EmailRecipients_MainID,EmailRecipients_ContactLeadID,EmailRecipients_RecipientEmailID,EmailRecipients_RecipientType,EmailRecipients_SendDateTime,EmailRecipients_Status", fValues2);
                                    cnttype = "";
                                }
                                else
                                {
                                    String[] dated = datediff.ToString().Split('-');
                                    string dd = dated[0].ToString();
                                    string mm = dated[1].ToString();
                                    string timesp = dated[2].ToString();
                                    //string dtsp = datediff[0].ToString();
                                    string dttt = Convert.ToDateTime(oDBEngine.GetDate()).ToString();
                                    string tms = timesp.Substring(4, 7);
                                    string yys = timesp.Substring(0, 4);
                                    string OrigDate = yys + "-" + mm + "-" + dd + " " + tms;
                                    //string fValues = "'" + InternalID + "','" + mailaddress + "','" + mailname + "','" + cnttype + "','" + Convert.ToDateTime(oDBEngine.GetDate()) + "','" + "P" + "','" + Convert.ToDateTime(oDBEngine.GetDate()) + "'";
                                    string fValues3 = "'" + InternalID + "','" + mailaddress + "','" + mailname + "','" + cnttype + "','" + OrigDate + "','" + "P" + "'";
                                    oDBEngine.InsurtFieldValue("Trans_EmailRecipients", "EmailRecipients_MainID,EmailRecipients_ContactLeadID,EmailRecipients_RecipientEmailID,EmailRecipients_RecipientType,EmailRecipients_SendDateTime,EmailRecipients_Status", fValues3);
                                    cnttype = "";
                                }
                                //string fValues = "'" + InternalID + "','" + mailaddress + "','" + mailname + "','" + cnttype + "','" + Convert.ToDateTime(oDBEngine.GetDate()) + "','" + "P" + "','" + Convert.ToDateTime(oDBEngine.GetDate()) + "'";
                                //string fValues3 = "'" + InternalID + "','" + mailaddress + "','" + mailname + "','" + cnttype + "','" + Convert.ToDateTime(oDBEngine.GetDate()) + "','" + "P" + "','" + datediff + "'";
                                //oDBEngine.InsurtFieldValue("Trans_EmailRecipients", "EmailRecipients_MainID,EmailRecipients_ContactLeadID,EmailRecipients_RecipientEmailID,EmailRecipients_RecipientType,EmailRecipients_SendDateTime,EmailRecipients_Status,EmailRecipients_SentDateTime", fValues3);
                                //cnttype = "";
                            }

                        }
                        Session["table"] = null;
                        Session["table1"] = null;
                        data = "sendmail~Y~Mail Posted Successfully!";
                        DataTable Empty = new DataTable();
                        GridAttachmentLocal.DataSource = Empty.DefaultView;
                        GridAttachmentLocal.DataBind();
                        GridAttachmentServer.DataSource = Empty.DefaultView;
                        GridAttachmentServer.DataBind();

                    }
                    catch (Exception e)
                    {
                        data = "sendmail~";
                    }
                }
                else
                {
                    data = "sendmail~You Can not send mail Because, You don`t have Official E-mail ID !";

                }

            }
            #endregion
            #region sendmailGroup
            if (FieldWvalue[0] == "sendmailGroup")
            {
                String datediff = FieldWvalue[5].ToString();
                DataTable DTFiles = (DataTable)Session["table"];
                DataTable DTSFiles = (DataTable)Session["table1"];
                string atchflile;
                if (DTFiles != null || DTSFiles != null)
                {
                    atchflile = "Y";
                    //if (DTFiles.Rows.Count > 0)
                    //{
                    //    atchflile = "Y";

                    //}
                    //else
                    //{
                    //    atchflile = "N";
                    //}
                }
                else
                {
                    atchflile = "N";
                }
                string contactID = HttpContext.Current.Session["usercontactID"].ToString();
                DataTable dtcmp = oDBEngine.GetDataTable(" tbl_master_company  ", "*", "cmp_id=(select emp_organization from tbl_trans_employeectc where emp_cntId='" + contactID + "' and emp_effectiveuntil is null)");
                string cmpintid = dtcmp.Rows[0]["cmp_internalid"].ToString();
                DataTable dtsg = oDBEngine.GetDataTable(" tbl_master_segment  ", "*", "seg_id='" + HttpContext.Current.Session["userlastsegment"].ToString() + "'");
                string segmentname = dtsg.Rows[0]["seg_name"].ToString();
                string sPath = HttpContext.Current.Request.Url.ToString();
                string[] PageName = sPath.ToString().Split('/');
                DataTable dt = oDBEngine.GetDataTable(" tbl_trans_menu ", "mnu_id ", " mnu_menuLink like '%" + PageName[5] + "%' and mnu_segmentid='" + HttpContext.Current.Session["userlastsegment"] + "'");
                string menuId = dt.Rows[0]["mnu_id"].ToString();
                string[,] sender = oDBEngine.GetFieldValue(" tbl_master_email ", " eml_email  ", " eml_cntId in (select user_contactId from tbl_master_user where user_id=" + HttpContext.Current.Session["userid"] + ") and eml_type='Official'", 1);
                if (sender[0, 0] != "n")
                {
                    string senderemail = sender[0, 0];
                    string[,] emails;
                    if (FieldWvalue[1] == "LD")
                    {
                        emails = oDBEngine.GetFieldValue("  tbl_master_email INNER JOIN tbl_master_lead ON tbl_master_email.eml_cntId = tbl_master_lead.cnt_internalId  ", " IsNull(RTRIM(cnt_firstName),'') + RTRIM(IsNull(cnt_lastName,'')) + '< '+ tbl_master_email.eml_email + '>' as name ,cnt_internalId as contactid ", " tbl_master_email.eml_email <> '' and eml_cntId like '" + FieldWvalue[1] + "%'", 2);
                    }
                    else
                    {
                        emails = oDBEngine.GetFieldValue("  tbl_master_email INNER JOIN tbl_master_contact ON tbl_master_email.eml_cntId = tbl_master_contact.cnt_internalId  ", " IsNull(RTRIM(cnt_firstName),'') + RTRIM(IsNull(cnt_lastName,'')) + '< '+ tbl_master_email.eml_email + '>' as name ,cnt_internalId as contactid ", " tbl_master_email.eml_email <> '' and eml_cntId like '" + FieldWvalue[1] + "%'", 2);
                    }

                    if (emails[0, 0] != "n")
                    {

                        //String con = ConfigurationSSettings.AppSettings["DBConnectionDefault"];  MULTI

                        String con = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                       
                        SqlConnection lcon = new SqlConnection(con);
                        lcon.Open();
                        SqlCommand lcmdEmplInsert = new SqlCommand("InsertTransEmail", lcon);
                        lcmdEmplInsert.CommandType = CommandType.StoredProcedure;

                        lcmdEmplInsert.Parameters.AddWithValue("@Emails_SenderEmailID", senderemail);
                        lcmdEmplInsert.Parameters.AddWithValue("@Emails_Subject", FieldWvalue[3]);
                        lcmdEmplInsert.Parameters.AddWithValue("@Emails_Content", FieldWvalue[2]);
                        lcmdEmplInsert.Parameters.AddWithValue("@Emails_HasAttachement", atchflile);
                        lcmdEmplInsert.Parameters.AddWithValue("@Emails_CreateApplication", menuId);
                        lcmdEmplInsert.Parameters.AddWithValue("@Emails_CreateUser", Session["userid"]);
                        lcmdEmplInsert.Parameters.AddWithValue("@Emails_Type", "B");
                        lcmdEmplInsert.Parameters.AddWithValue("@Emails_CompanyID", cmpintid);
                        lcmdEmplInsert.Parameters.AddWithValue("@Emails_Segment", segmentname);
                        SqlParameter parameter = new SqlParameter("@result", SqlDbType.BigInt);
                        parameter.Direction = ParameterDirection.Output;

                        lcmdEmplInsert.Parameters.Add(parameter);
                        lcmdEmplInsert.ExecuteNonQuery();
                        // Mantis Issue 24802
                        if (lcon.State == ConnectionState.Open)
                        {
                            lcon.Close();
                        }
                        // End of Mantis Issue 24802
                        string InternalID = parameter.Value.ToString();
                        if (DTFiles != null)
                        {
                            for (int i = 0; i < DTFiles.Rows.Count; i++)
                            {
                                string filepath = DTFiles.Rows[i]["filepathServer"].ToString().Trim();
                                string fValues4 = "'" + InternalID + "','" + filepath + "'";
                                oDBEngine.InsurtFieldValue("Trans_EmailAttachment", "EmailAttachment_MainID,EmailAttachment_Path", fValues4);
                            }
                        }
                        if (DTSFiles != null)
                        {
                            for (int i = 0; i < DTSFiles.Rows.Count; i++)
                            {
                                string filepath = DTSFiles.Rows[i]["Serverfilename"].ToString().Trim();
                                //string[] fpathsd = filepath.Split('\\');
                                //int P = fpathsd.Length;
                                //string pathNew = fpathsd[P - 3].Trim() + "\\" + fpathsd[P - 2].Trim() + "\\" + fpathsd[P - 1].Trim();
                                string fValues1 = "'" + InternalID + "','" + filepath + "'";
                                oDBEngine.InsurtFieldValue("Trans_EmailAttachment", "EmailAttachment_MainID,EmailAttachment_Path", fValues1);
                            }
                        }
                        string retrn = "";
                        string EmailList = "";
                        for (int i = 0; i < emails.Length / 2; i++)
                        {
                            string mailaddress = "";
                            string mailid = emails[i, 0];
                            string[] mailcnt = emails[i, 0].Split('<');
                            if (mailcnt.Length == 1)
                            {
                                mailaddress = mailcnt[0];

                            }
                            else
                            {
                                mailaddress = mailcnt[1].Substring(0, mailcnt[1].Length - 1);
                            }
                            string contid = emails[i, 1];
                            if (datediff == "")
                            {
                                string fValues5 = "'" + InternalID + "','" + contid + "','" + mailaddress + "','" + "TO" + "','" + Convert.ToDateTime(oDBEngine.GetDate()) + "','" + "P" + "','" + Convert.ToDateTime(oDBEngine.GetDate()) + "'";
                                oDBEngine.InsurtFieldValue("Trans_EmailRecipients", "EmailRecipients_MainID,EmailRecipients_ContactLeadID,EmailRecipients_RecipientEmailID,EmailRecipients_RecipientType,EmailRecipients_SendDateTime,EmailRecipients_Status,EmailRecipients_SentDateTime", fValues5);
                            }
                            else
                            {
                                String[] dated = datediff.ToString().Split('-');
                                string dd = dated[0].ToString();
                                string mm = dated[1].ToString();
                                string timesp = dated[2].ToString();
                                //string dtsp = datediff[0].ToString();
                                string dttt = Convert.ToDateTime(oDBEngine.GetDate()).ToString();
                                string tms = timesp.Substring(4, 7);
                                string yys = timesp.Substring(0, 4);
                                string OrigDate = yys + "-" + mm + "-" + dd + " " + tms;
                                //string fValues = "'" + InternalID + "','" + contid + "','" + mailaddress + "','" + "TO" + "','" + Convert.ToDateTime(oDBEngine.GetDate()) + "','" + "P" + "','" + Convert.ToDateTime(oDBEngine.GetDate()) + "'";
                                string fValues6 = "'" + InternalID + "','" + contid + "','" + mailaddress + "','" + "TO" + "','" + Convert.ToDateTime(oDBEngine.GetDate()) + "','" + "P" + "','" + OrigDate + "'";
                                oDBEngine.InsurtFieldValue("Trans_EmailRecipients", "EmailRecipients_MainID,EmailRecipients_ContactLeadID,EmailRecipients_RecipientEmailID,EmailRecipients_RecipientType,EmailRecipients_SendDateTime,EmailRecipients_Status,EmailRecipients_SentDateTime", fValues6);
                            }

                        }

                        //if (retrn == "done")
                        //{
                        Session["table"] = null;
                        Session["table1"] = null;
                        data = "sendmail~Y~Mail Posted Successfully!";
                        DataTable Empty = new DataTable();
                        GridAttachmentLocal.DataSource = Empty.DefaultView;
                        GridAttachmentLocal.DataBind();
                        GridAttachmentServer.DataSource = Empty.DefaultView;
                        GridAttachmentServer.DataBind();

                        //}
                        //else
                        //    data = "sendmail~" + retrn;
                    }
                    else
                    {
                        data = "sendmail~You Can not send mail Because, You don`t have E-mail ID for that group!";
                    }


                }
                else
                {
                    data = "sendmail~You Can not send mail Because, You don`t have Official E-mail ID !";

                }

            }
            #endregion
            #region sendGroupwise
            if (FieldWvalue[0] == "sendGroupwise")
            {
                String datediff = FieldWvalue[6].ToString();
                DataTable DTFiles = (DataTable)Session["table"];
                DataTable DTSFiles = (DataTable)Session["table1"];
                string atchflile;
                if (DTFiles != null || DTSFiles != null)
                {
                    atchflile = "Y";
                    //if (DTFiles.Rows.Count > 0)
                    //{
                    //    atchflile = "Y";

                    //}
                    //else
                    //{
                    //    atchflile = "N";
                    //}
                }
                else
                {
                    atchflile = "N";
                }
                string contactID = HttpContext.Current.Session["usercontactID"].ToString();
                DataTable dtcmp = oDBEngine.GetDataTable(" tbl_master_company  ", "*", "cmp_id=(select emp_organization from tbl_trans_employeectc where emp_cntId='" + contactID + "' and emp_effectiveuntil is null)");
                string cmpintid = dtcmp.Rows[0]["cmp_internalid"].ToString();
                DataTable dtsg = oDBEngine.GetDataTable(" tbl_master_segment  ", "*", "seg_id='" + HttpContext.Current.Session["userlastsegment"].ToString() + "'");
                string segmentname = dtsg.Rows[0]["seg_name"].ToString();
                string sPath = HttpContext.Current.Request.Url.ToString();
                string[] PageName = sPath.ToString().Split('/');
                DataTable dt = oDBEngine.GetDataTable(" tbl_trans_menu ", "mnu_id ", " mnu_menuLink like '%" + PageName[5] + "%' and mnu_segmentid='" + HttpContext.Current.Session["userlastsegment"] + "'");
                string menuId = dt.Rows[0]["mnu_id"].ToString();
                string[,] sender = oDBEngine.GetFieldValue(" tbl_master_email ", " eml_email  ", " eml_cntId in (select user_contactId from tbl_master_user where user_id=" + HttpContext.Current.Session["userid"] + ") and eml_type='Official'", 1);
                if (sender[0, 0] != "n")
                {
                    string senderemail = sender[0, 0];
                    string[,] emails = oDBEngine.GetFieldValue("tbl_master_email", "eml_email,eml_cntId ", "eml_cntId in (select grp_contactId from tbl_trans_group where grp_groupMaster='" + FieldWvalue[5] + "')", 2);

                    if (emails[0, 0] != "n")
                    {

                       // String con = ConfigurationSSettings.AppSettings["DBConnectionDefault"];  MULTI

                        String con = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);

                        SqlConnection lcon = new SqlConnection(con);
                        lcon.Open();
                        SqlCommand lcmdEmplInsert = new SqlCommand("InsertTransEmail", lcon);
                        lcmdEmplInsert.CommandType = CommandType.StoredProcedure;

                        lcmdEmplInsert.Parameters.AddWithValue("@Emails_SenderEmailID", senderemail);
                        lcmdEmplInsert.Parameters.AddWithValue("@Emails_Subject", FieldWvalue[3]);
                        lcmdEmplInsert.Parameters.AddWithValue("@Emails_Content", FieldWvalue[2]);
                        lcmdEmplInsert.Parameters.AddWithValue("@Emails_HasAttachement", atchflile);
                        lcmdEmplInsert.Parameters.AddWithValue("@Emails_CreateApplication", menuId);
                        lcmdEmplInsert.Parameters.AddWithValue("@Emails_CreateUser", Session["userid"]);
                        lcmdEmplInsert.Parameters.AddWithValue("@Emails_Type", "B");
                        lcmdEmplInsert.Parameters.AddWithValue("@Emails_CompanyID", cmpintid);
                        lcmdEmplInsert.Parameters.AddWithValue("@Emails_Segment", segmentname);
                        SqlParameter parameter = new SqlParameter("@result", SqlDbType.BigInt);
                        parameter.Direction = ParameterDirection.Output;

                        lcmdEmplInsert.Parameters.Add(parameter);
                        lcmdEmplInsert.ExecuteNonQuery();
                        // Mantis Issue 24802
                        if (lcon.State == ConnectionState.Open)
                        {
                            lcon.Close();
                        }
                        // End of Mantis Issue 24802
                        string InternalID = parameter.Value.ToString();
                        if (DTFiles != null)
                        {
                            for (int i = 0; i < DTFiles.Rows.Count; i++)
                            {

                                // Message.Attachments.Add(new Attachment(attachmentDetails.Rows[i]["filepathServer"].ToString().Trim()));
                                string filepath = DTFiles.Rows[i]["filepathServer"].ToString().Trim();
                                string fValues7 = "'" + InternalID + "','" + filepath + "'";
                                oDBEngine.InsurtFieldValue("Trans_EmailAttachment", "EmailAttachment_MainID,EmailAttachment_Path", fValues7);
                            }
                        }
                        if (DTSFiles != null)
                        {
                            for (int i = 0; i < DTSFiles.Rows.Count; i++)
                            {
                                string filepath = DTSFiles.Rows[i]["Serverfilename"].ToString().Trim();
                                //string[] fpathsd = filepath.Split('\\');
                                //int P = fpathsd.Length;
                                //string pathNew = fpathsd[P - 3].Trim() + "\\" + fpathsd[P - 2].Trim() + "\\" + fpathsd[P - 1].Trim();
                                string fValues1 = "'" + InternalID + "','" + filepath + "'";
                                oDBEngine.InsurtFieldValue("Trans_EmailAttachment", "EmailAttachment_MainID,EmailAttachment_Path", fValues1);
                            }
                        }
                        for (int i = 0; i < emails.Length / 2; i++)
                        {

                            string mailid = emails[i, 0];
                            string contid = emails[i, 1];
                            if (datediff == "")
                            {
                                string fValues8 = "'" + InternalID + "','" + contid + "','" + mailid + "','" + "TO" + "','" + Convert.ToDateTime(oDBEngine.GetDate()) + "','" + "P" + "'";
                                oDBEngine.InsurtFieldValue("Trans_EmailRecipients", "EmailRecipients_MainID,EmailRecipients_ContactLeadID,EmailRecipients_RecipientEmailID,EmailRecipients_RecipientType,EmailRecipients_SendDateTime,EmailRecipients_Status", fValues8);
                            }
                            else
                            {
                                String[] dated = datediff.ToString().Split('-');
                                string dd = dated[0].ToString();
                                string mm = dated[1].ToString();
                                string timesp = dated[2].ToString();
                                //string dtsp = datediff[0].ToString();
                                string dttt = Convert.ToDateTime(oDBEngine.GetDate()).ToString();
                                string tms = timesp.Substring(4, 7);
                                string yys = timesp.Substring(0, 4);
                                string OrigDate = yys + "-" + mm + "-" + dd + " " + tms;
                                //string fValues = "'" + InternalID + "','" + contid + "','" + mailid + "','" + "TO" + "','" + Convert.ToDateTime(oDBEngine.GetDate()) + "','" + "P" + "','" + Convert.ToDateTime(oDBEngine.GetDate()) + "'";
                                string fValues9 = "'" + InternalID + "','" + contid + "','" + mailid + "','" + "TO" + "','" + OrigDate + "','" + "P" + "'";
                                oDBEngine.InsurtFieldValue("Trans_EmailRecipients", "EmailRecipients_MainID,EmailRecipients_ContactLeadID,EmailRecipients_RecipientEmailID,EmailRecipients_RecipientType,EmailRecipients_SendDateTime,EmailRecipients_Status", fValues9);
                            }
                        }

                        //if (retrn == "done")
                        //{
                        Session["table"] = null;
                        Session["table1"] = null;
                        data = "sendmail~Y~Mail Posted Successfully!";
                        DataTable Empty = new DataTable();
                        GridAttachmentLocal.DataSource = Empty.DefaultView;
                        GridAttachmentLocal.DataBind();
                        GridAttachmentServer.DataSource = Empty.DefaultView;
                        GridAttachmentServer.DataBind();

                        //}
                        //else

                    }
                    else
                    {
                        data = "sendmail~You Can not send mail Because, You don`t have E-mail ID for that group!";
                    }


                }
                else
                {
                    data = "sendmail~You Can not send mail Because, You don`t have Official E-mail ID !";

                }

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
        //protected void GridAttachment_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        //{
        //    DBEngine oDBEngine = new DBEngine(ConfigurationSSettings.AppSettings["DBConnectionDefault"]);
        //    string datalist = e.Parameters.ToString();
        //    string[] FieldWvalue = datalist.Split('~');
        //    string[,] idstring;
        //    string id1 = "";
        //    if (FieldWvalue[0] == "search")
        //    {
        //        switch (FieldWvalue[1])
        //        {
        //            case "Products  MF":
        //                idstring = oDBEngine.GetFieldValue(" tbl_master_products ", " prds_internalid", " prds_productType=12 and prds_description='" + FieldWvalue[4] + "'", 1);
        //                id1 = idstring[0, 0];
        //                break;
        //            case "Products – Insurance":
        //                idstring = oDBEngine.GetFieldValue(" tbl_master_products ", " prds_internalid", " prds_productType=13 and prds_description='" + FieldWvalue[4] + "'", 1);
        //                id1 = idstring[0, 0];
        //                break;
        //            case "Products - IPOs":
        //                idstring = oDBEngine.GetFieldValue(" tbl_master_products ", " prds_internalid", " prds_productType==15 and prds_description='" + FieldWvalue[4] + "'", 1);
        //                id1 = idstring[0, 0];
        //                break;
        //            case "Customer":
        //                idstring = oDBEngine.GetFieldValue(" tbl_master_products ", " prds_internalid", " prds_productType=1 and cnt_firstName='" + FieldWvalue[4] + "'", 1);
        //                id1 = idstring[0, 0];
        //                break;
        //            case "Lead":
        //                idstring = oDBEngine.GetFieldValue(" tbl_master_lead ", " cnt_internalid ", "cnt_firstName='" + FieldWvalue[4] + "'", 1);
        //                id1 = idstring[0, 0];
        //                break;
        //            case "Employee":
        //                idstring = oDBEngine.GetFieldValue(" tbl_master_contact ", " cnt_internalid ", " cnt_contactType=3 and cnt_firstName='" + FieldWvalue[4] + "'", 1);
        //                id1 = idstring[0, 0];
        //                break;
        //            case "Sub Brokers":
        //                idstring = oDBEngine.GetFieldValue(" tbl_master_contact ", " cnt_internalid ", " cnt_contactType=2 and cnt_firstName='" + FieldWvalue[4] + "'", 1);
        //                id1 = idstring[0, 0];
        //                break;
        //            case "Franchisees":
        //                idstring = oDBEngine.GetFieldValue(" tbl_master_contact ", " cnt_internalid ", " cnt_contactType=4 and cnt_firstName='" + FieldWvalue[4] + "'", 1);
        //                id1 = idstring[0, 0];
        //                break;
        //            case "Data Vendors":
        //                idstring = oDBEngine.GetFieldValue(" tbl_master_contact ", " cnt_internalid ", " cnt_contactType=7 and cnt_firstName='" + FieldWvalue[4] + "'", 1);
        //                id1 = idstring[0, 0];
        //                break;
        //            case "Referral Agents":
        //                idstring = oDBEngine.GetFieldValue(" tbl_master_contact ", " cnt_internalid ", " cnt_contactType=8 and cnt_firstName='" + FieldWvalue[4] + "'", 1);
        //                id1 = idstring[0, 0];
        //                break;
        //            case "Recruitment Agents":
        //                idstring = oDBEngine.GetFieldValue(" tbl_master_contact ", " cnt_internalid ", " cnt_contactType=9 and cnt_firstName='" + FieldWvalue[4] + "'", 1);
        //                id1 = idstring[0, 0];
        //                break;
        //            case "AMCs":
        //                idstring = oDBEngine.GetFieldValue(" tbl_master_AssetsManagementCompanies ", " amc_amcCode ", " amc_nameOfMutualFund='" + FieldWvalue[4] + "'", 1);
        //                id1 = idstring[0, 0];
        //                break;
        //            case "Insurance Companies":
        //                idstring = oDBEngine.GetFieldValue(" tbl_master_insurerName ", " insu_internalid ", " insu_nameOfCompany='" + FieldWvalue[4] + "'", 1);
        //                id1 = idstring[0, 0];
        //                break;
        //            case "RTAs":
        //                idstring = oDBEngine.GetFieldValue(" tbl_registrarTransferAgent ", " rta_rtaCode ", " rta_name='" + FieldWvalue[4] + "'", 1);
        //                id1 = idstring[0, 0];
        //                break;
        //            case "Branches":
        //                idstring = oDBEngine.GetFieldValue("  tbl_master_branch ", " branch_internalid ", " branch_description='" + FieldWvalue[4] + "'", 1);
        //                id1 = idstring[0, 0];
        //                break;
        //            case "Companies":
        //                idstring = oDBEngine.GetFieldValue(" tbl_master_company ", " cmp_internalId ", " cmp_Name='" + FieldWvalue[4] + "'", 1);
        //                id1 = idstring[0, 0];
        //                break;
        //            case "Building":
        //                idstring = oDBEngine.GetFieldValue(" tbl_master_building ", " bui_id ", " bui_Name='" + FieldWvalue[4] + "'", 1);
        //                id1 = idstring[0, 0];
        //                break;
        //        }
        //        if (id1 != "n")
        //        {
        //            DT = oDBEngine.GetDataTable(" tbl_master_document INNER JOIN tbl_master_documentType ON tbl_master_document.doc_documentTypeId = tbl_master_documentType.dty_id INNER JOIN tbl_master_building ON tbl_master_document.doc_buildingId = tbl_master_building.bui_id ", " tbl_master_document.doc_id AS Id, tbl_master_documentType.dty_documentType AS Type, tbl_master_document.doc_documentName AS FileName, tbl_master_document.doc_source AS FilePath, COALESCE ('Building- ' + tbl_master_building.bui_Name + CHAR(13) + CHAR(10), '') + COALESCE ('/ Floor No : ' + tbl_master_document.doc_Floor + CHAR(13) + CHAR(10), '')  + '/ Room No-' + tbl_master_document.doc_RoomNo + '/ Cabinet No-' + tbl_master_document.doc_CellNo + '/ File No-' + tbl_master_document.doc_FileNo AS FilePath ", " doc_contactId =" + id1 + " and doc_documentTypeId = " + FieldWvalue[2]);
        //            GridAttachment.DataSource = DT;
        //            GridAttachment.DataBind();
        //            if (DT.Rows.Count > 0)
        //                data = "search~Y";
        //            else
        //                data = "search~N";

        //        }
        //        else
        //        {
        //            DT = oDBEngine.GetDataTable(" tbl_master_document INNER JOIN tbl_master_documentType ON tbl_master_document.doc_documentTypeId = tbl_master_documentType.dty_id INNER JOIN tbl_master_building ON tbl_master_document.doc_buildingId = tbl_master_building.bui_id ", " tbl_master_document.doc_id AS Id, tbl_master_documentType.dty_documentType AS Type, tbl_master_document.doc_documentName AS FileName, tbl_master_document.doc_source AS FilePath, COALESCE ('Building- ' + tbl_master_building.bui_Name + CHAR(13) + CHAR(10), '') + COALESCE ('/ Floor No : ' + tbl_master_document.doc_Floor + CHAR(13) + CHAR(10), '')  + '/ Room No-' + tbl_master_document.doc_RoomNo + '/ Cabinet No-' + tbl_master_document.doc_CellNo + '/ File No-' + tbl_master_document.doc_FileNo AS FilePath ", " doc_documentName = '" + FieldWvalue[4] + "' and doc_documentTypeId=" + FieldWvalue[2]);
        //            GridAttachment.DataSource = DT;
        //            GridAttachment.DataBind();
        //            if (DT.Rows.Count > 0)
        //                data = "search~Y";
        //            else
        //                data = "search~N";
        //        }
        //        Session["table"] = DT;
        //    }
        //    if (FieldWvalue[0] == "cancel")
        //    {
        //        GridAttachment.CancelEdit();
        //    }
        //}
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
                    Session["table"] = null;
                }
                DTLocalFiles.Rows.Clear();
                GridAttachmentLocal.DataSource = DTLocalFiles.DefaultView;
                GridAttachmentLocal.DataBind();
                Session["table"] = null;
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
            if (Session["table1"] == null)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "mailsend", "<script>VisibleServer()</script>");
            }
            Page.ClientScript.RegisterStartupScript(GetType(), "jscript1", "<script>SetSendDateTime()</script>");
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
            Page.ClientScript.RegisterStartupScript(GetType(), "mailsend", "<script>AttachmentCall()</script>");
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





        protected void GridAttachmentServer_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            string datapathlist = e.Parameters.ToString();
            string[] FieldWvalue = datapathlist.Split('~');
            if (FieldWvalue[0] == "remvServer")
            {
                string[] FileLocationIDs = FieldWvalue[1].ToString().Split(',');
                DataTable DTLocalFiles = (DataTable)Session["table1"];
                for (int i = 0; i < DTLocalFiles.Rows.Count; i++)
                {
                    for (int j = 0; j < FileLocationIDs.Length; j++)
                    {
                        if (DTLocalFiles.Rows[i]["Serverfilename"].ToString().Trim() == FileLocationIDs[j].ToString().Trim())
                        {
                            //File.Delete(FileLocationIDs[j].ToString().Trim());
                            DTLocalFiles.Rows[i].Delete();
                        }
                    }
                }
                GridAttachmentServer.DataSource = DTLocalFiles.DefaultView;
                GridAttachmentServer.DataBind();
            }
            else if (FieldWvalue[0] == "canServer")
            {
                DataTable DTLocalFiles = (DataTable)Session["table1"];
                DTLocalFiles.Rows.Clear();
                GridAttachmentServer.DataSource = DTLocalFiles.DefaultView;
                GridAttachmentServer.DataBind();
                Session["table1"] = null;
            }
            else if (FieldWvalue[0] == "AttachServer")
            {
                string pathlist = FieldWvalue[1].ToString();
                FillGridServer(pathlist);
            }
        }
        private void FillGridServer(string pathlist)
        {
            if (Session["table1"] != null)
            {
                string[] FilePath = pathlist.ToString().Split(',');
                int j = FilePath.Length;
                DT = (DataTable)Session["table1"];
                int lenghth = DT.Rows.Count;
                for (int i = 0; i < j; i++)
                {
                    DataRow DR = DT.NewRow();
                    DR["Serverfileid"] = lenghth + 1;
                    DR["Serverfilename"] = FilePath[i].ToString();
                    DT.Rows.Add(DR);
                }
                GridAttachmentServer.DataSource = DT;
                GridAttachmentServer.DataBind();

            }
            else
            {
                string[] FilePath = pathlist.ToString().Split(',');
                int j = FilePath.Length;
                DataColumn DC1 = new DataColumn("Serverfileid");
                DataColumn DC2 = new DataColumn("Serverfilename");
                DT.Columns.Add(DC1);
                DT.Columns.Add(DC2);
                for (int i = 0; i < j; i++)
                {
                    DataRow DR = DT.NewRow();
                    DR["Serverfileid"] = i + 1;
                    DR["Serverfilename"] = FilePath[i].ToString();
                    DT.Rows.Add(DR);
                }
                Session["table1"] = DT;

                GridAttachmentServer.DataSource = DT;
                GridAttachmentServer.DataBind();
            }
        }
        protected void btnCancelall_Click(object sender, EventArgs e)
        {
            Session["table1"] = null;
            Session["table"] = null;
            Response.Redirect("frmsendmail.aspx");
        }
    }
}