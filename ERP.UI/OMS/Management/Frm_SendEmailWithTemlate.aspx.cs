using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrystalDecisions.CrystalReports.Engine;
//using DevExpress.Web;
using DevExpress.Web;
using FreeTextBoxControls;
using BusinessLogicLayer;

namespace ERP.OMS.Management
{
    public partial class management_Frm_SendEmailWithTemlate : System.Web.UI.Page, System.Web.UI.ICallbackEventHandler
    {
        Management_BL oManagement_BL = new Management_BL();
        string data;
        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);MULTI
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

        protected void page_PreInit(object sender, EventArgs e)
        {





        }

        protected void Page_Load(object sender, EventArgs e)
        {

            FreeTextBox FreeTextBox1 = new FreeTextBox();
            FreeTextBox1.ID = "FreeTextBox1";
            FreeTextBox1.Height = 450;
            FreeTextBox1.Width = 840;
            //    PlaceHolder update = (PlaceHolder)UpdatePanel1.FindControl("FreeTextBoxPlaceHolder");

            FreeTextBoxPlaceHolder.Controls.Add(FreeTextBox1);
            FreeTextBox1.EnableHtmlMode = false;



            if (!IsPostBack)
            {


                Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script language='JavaScript'>Page_Load();</script>");
                if (HdnTemplate.Value.ToString() == "")
                {
                    drpBranch.Items.Clear();
                    DataTable DTTemp = oDBEngine.GetDataTable("master_templateDetails", "tmplt_shortname,tmplt_id", "tmplt_usedfor='CL'");
                    drpBranch.DataSource = DTTemp;
                    drpBranch.ValueField = "tmplt_id";
                    drpBranch.TextField = "tmplt_shortname";
                    drpBranch.DataBind();
                    drpBranch.Items.Insert(0, new ListEditItem("New", "N"));
                    drpBranch.SelectedIndex = 0;
                }
            }



            //_____For performing operation without refreshing page___//
            String cbReference = Page.ClientScript.GetCallbackEventReference(this, "arg", "ReceiveServerData", "context");
            String callbackScript = "function CallServer(arg, context){ " + cbReference + ";}";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "CallServer", callbackScript, true);
            //this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");


        }
        void ICallbackEventHandler.RaiseCallbackEvent(string eventArgument)
        {
            string id = eventArgument.ToString();
            string[] idlist = id.Split('~');
            string[] cl = idlist[1].Split(',');
            string str = "";
            string str1 = "";
            string str2 = "";
            for (int i = 0; i < cl.Length; i++)
            {
                string[] val = cl[i].Split(';');
                if (str == "")
                {
                    if (idlist[0] == "EM")
                    {
                        str = val[0];
                        str1 = val[0] + ";" + val[1];
                    }
                    else
                    {
                        str = "'" + val[0] + "'";
                        str1 = val[0] + ";" + val[1];
                    }
                }
                else
                {
                    if (idlist[0] == "EM")
                    {
                        str += "," + val[0];
                    }
                    else
                    {
                        str += ",'" + val[0] + "'";
                        str1 += "," + val[0] + ";" + val[1];
                    }
                }
            }
            if (idlist[0] == "Clients")
            {
                data = "Clients~" + str;
            }
            else if (idlist[0] == "Branch")
            {
                data = "Branch~" + str;
            }
            else if (idlist[0] == "Group")
            {
                data = "Group~" + str;
            }
            else if (idlist[0] == "EM")
            {
                data = "Employee~" + str;
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
        protected void drpBranch_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            drpBranch.Items.Clear();
            string SubAccountRefID = e.Parameter;
            DataTable DTTemp = oDBEngine.GetDataTable("master_templateDetails", "tmplt_shortname,tmplt_id", "tmplt_usedfor='" + SubAccountRefID + "'");
            drpBranch.DataSource = DTTemp;
            drpBranch.ValueField = "tmplt_id";
            drpBranch.TextField = "tmplt_shortname";
            drpBranch.DataBind();
            drpBranch.Items.Insert(0, new ListEditItem("New", "N"));
            drpBranch.SelectedIndex = 0;
        }

        protected void btnContent_Click(object sender, EventArgs e)
        {

            FreeTextBox textEditor = FreeTextBoxPlaceHolder.FindControl("FreeTextBox1") as FreeTextBox;
            textEditor.Height = 340;
            textEditor.Width = 840;
            if (HdnTemplate.Value.ToString() != "N")
            {
                DataTable dT = oDBEngine.GetDataTable("master_templateDetails", "tmplt_Content", "tmplt_id='" + HdnTemplate.Value + "'");
                if (dT.Rows.Count > 0)
                {
                    textEditor.Text = dT.Rows[0]["tmplt_Content"].ToString();


                }
                else
                {
                    textEditor.Text = "";
                }
            }
            else
            {
                textEditor.Text = "";
            }
        }

        protected void btnSend_Click(object sender, EventArgs e)
        {

            string Subject = TxtSubject.Text;
            string SenderType = cmbType.SelectedItem.Value;
            string TemplateID = "";
            string Content = "";
            FreeTextBox textEditor = FreeTextBoxPlaceHolder.FindControl("FreeTextBox1") as FreeTextBox;
            Content = textEditor.Text;
            string Brnch = "";
            if (rdbranchAll.Checked == true)
            {
                Brnch = Session["userbranchHierarchy"].ToString();
            }
            else
            {
                Brnch = HdnBranch.Value;
            }

            #region GetContact
            DataTable DT = new DataTable();

            if (rdbClientsAll.Checked == true)
            {
                if (ddlGroup.SelectedItem.Value == "0")
                {

                    if (cmbType.SelectedItem.Value == "CD")
                    {
                        DT = oDBEngine.GetDataTable("MASTER_CDSLCLIENTS,TBL_MASTER_EMAIL ", " distinct   LTRIM(RTRIM(ISNULL(CDSLCLIENTS_FIRSTHOLDERNAME,'')))+' ['+ LTRIM(RTRIM(ISNULL(RIGHT(CDSLCLIENTS_BOID,8),''))) + '] ' as ClientName , ltrim(rtrim(isnull(eml_email,''))) as EmailID ,CDSLCLIENTS_BOID as ID ", " MASTER_CDSLCLIENTS.CDSLCLIENTS_BOID=tbl_master_email.eml_cntid and eml_email !='' and  eml_type='Official' and  cdslclients_branchid in(" + Brnch + ") ");
                    }
                    else if (cmbType.SelectedItem.Value == "ND")
                    {
                        DT = oDBEngine.GetDataTable(" MASTER_NSDLCLIENTS,TBL_MASTER_EMAIL ", " distinct  LTRIM(RTRIM(ISNULL(NSDLCLIENTS_BENFIRSTHOLDERNAME,''))) + ' ['+LTRIM(RTRIM(ISNULL(NSDLCLIENTS_BENACCOUNTID,''))) +']' as  ClientName, ltrim(rtrim(isnull(eml_email,''))) as EmailID ,NSDLCLIENTS_DPID +''+ CAST(NSDLCLIENTS_BENACCOUNTID AS  VARCHAR)  as ID", " NSDLCLIENTS_DPID +''+ CAST(NSDLCLIENTS_BENACCOUNTID AS  VARCHAR)=eml_cntid and eml_email !='' and  eml_type='Official'  and nsdlclients_branchid in (" + Brnch + ")  ");
                    }
                    else if (cmbType.SelectedItem.Value == "EM")
                    {
                        DT = oDBEngine.GetDataTable(" TBL_MASTER_CONTACT,tbl_master_email", " distinct LTRIM(RTRIM(ISNULL(CNT_FIRSTNAME,'')))+' '+ LTRIM(RTRIM(ISNULL(CNT_MIDDLENAME,''))) +' ' + LTRIM(RTRIM(ISNULL(CNT_LASTNAME,''))) + ' ['+ LTRIM(RTRIM(ISNULL(CNT_UCC,CNT_SHORTNAME)))+']' as ClientName,ltrim(rtrim(isnull(eml_email,''))) as EmailID ,CNT_INTERNALID  as ID ", " TBL_MASTER_CONTACT.CNT_INTERNALID=tbl_master_email.eml_cntid and eml_email !='' and eml_type='Official' and cnt_internalid like 'EM%' and cnt_branchid in (" + Brnch + ") ");
                    }
                    else
                    {
                        DT = oDBEngine.GetDataTable(" TBL_MASTER_CONTACT,tbl_master_email", " distinct   LTRIM(RTRIM(ISNULL(CNT_FIRSTNAME,'')))+' '+ LTRIM(RTRIM(ISNULL(CNT_MIDDLENAME,''))) +' ' + LTRIM(RTRIM(ISNULL(CNT_LASTNAME,''))) + ' ['+ LTRIM(RTRIM(ISNULL(CNT_UCC,CNT_SHORTNAME)))+']' as ClientName,ltrim(rtrim(isnull(eml_email,''))) as EmailID  ,CNT_INTERNALID   as ID", " TBL_MASTER_CONTACT.CNT_INTERNALID=tbl_master_email.eml_cntid and eml_email !='' and eml_type='Official' and cnt_internalid like 'CL%'  and cnt_branchid in (" + Brnch + ") ");

                    }

                }
                else
                {
                    if (cmbType.SelectedItem.Value == "CD")
                    {
                        if (rdddlgrouptypeAll.Checked == true)
                        {
                            DT = oDBEngine.GetDataTable("MASTER_CDSLCLIENTS,TBL_MASTER_EMAIL ", " distinct LTRIM(RTRIM(ISNULL(CDSLCLIENTS_FIRSTHOLDERNAME,'')))+' ['+ LTRIM(RTRIM(ISNULL(RIGHT(CDSLCLIENTS_BOID,8),''))) + ']'   as ClientName ,ltrim(rtrim(isnull(eml_email,''))) as EmailID,CDSLCLIENTS_BOID as ID ", " MASTER_CDSLCLIENTS.CDSLCLIENTS_BOID=tbl_master_email.eml_cntid and eml_email !='' and  eml_type='Official'  ");

                        }
                        else
                        {
                            DT = oDBEngine.GetDataTable("MASTER_CDSLCLIENTS,TBL_MASTER_EMAIL ", " distinct LTRIM(RTRIM(ISNULL(CDSLCLIENTS_FIRSTHOLDERNAME,'')))+' ['+ LTRIM(RTRIM(ISNULL(RIGHT(CDSLCLIENTS_BOID,8),''))) + ']' as ClientName, ltrim(rtrim(isnull(eml_email,''))) as EmailID,CDSLCLIENTS_BOID  as ID ", " MASTER_CDSLCLIENTS.CDSLCLIENTS_BOID=tbl_master_email.eml_cntid and eml_email !='' and  eml_type='Official' and   CDSLCLIENTS_BOID in (select grp_contactid from	tbl_trans_group where grp_groupmaster in(" + HdnGroup.Value + ")) ");
                        }
                    }
                    else if (cmbType.SelectedItem.Value == "ND")
                    {
                        if (rdddlgrouptypeAll.Checked == true)
                        {
                            DT = oDBEngine.GetDataTable(" MASTER_NSDLCLIENTS,TBL_MASTER_EMAIL ", " distinct LTRIM(RTRIM(ISNULL(NSDLCLIENTS_BENFIRSTHOLDERNAME,''))) + ' ['+LTRIM(RTRIM(ISNULL(NSDLCLIENTS_BENACCOUNTID,''))) +']' as ClientName,ltrim(rtrim(isnull(eml_email,''))) as EmailID ,NSDLCLIENTS_DPID +''+ CAST(NSDLCLIENTS_BENACCOUNTID AS  VARCHAR)  as ID ", " NSDLCLIENTS_DPID +''+ CAST(NSDLCLIENTS_BENACCOUNTID AS  VARCHAR)=eml_cntid and eml_email !='' and  eml_type='Official'  ");
                        }
                        else
                        {
                            DT = oDBEngine.GetDataTable(" MASTER_NSDLCLIENTS,TBL_MASTER_EMAIL ", " distinct LTRIM(RTRIM(ISNULL(NSDLCLIENTS_BENFIRSTHOLDERNAME,''))) + ' ['+LTRIM(RTRIM(ISNULL(NSDLCLIENTS_BENACCOUNTID,''))) +']' as ClientName ,ltrim(rtrim(isnull(eml_email,''))) as EmailID ,NSDLCLIENTS_DPID +''+ CAST(NSDLCLIENTS_BENACCOUNTID AS  VARCHAR) as ID ", " NSDLCLIENTS_DPID +''+ CAST(NSDLCLIENTS_BENACCOUNTID AS  VARCHAR)=eml_cntid and eml_email !='' and  eml_type='Official'  and NSDLCLIENTS_DPID +''+ CAST(NSDLCLIENTS_BENACCOUNTID AS  VARCHAR) in (select grp_contactid from	tbl_trans_group where grp_groupmaster in(" + HdnGroup.Value + "))  ");
                        }
                    }
                    else if (cmbType.SelectedItem.Value == "EM")
                    {
                        if (rdddlgrouptypeAll.Checked == true)
                        {
                            DT = oDBEngine.GetDataTable(" TBL_MASTER_CONTACT,tbl_master_email", " distinct  LTRIM(RTRIM(ISNULL(CNT_FIRSTNAME,'')))+' '+ LTRIM(RTRIM(ISNULL(CNT_MIDDLENAME,''))) +' ' + LTRIM(RTRIM(ISNULL(CNT_LASTNAME,''))) + ' ['+ LTRIM(RTRIM(ISNULL(CNT_UCC,CNT_SHORTNAME)))+']' as ClientName, ltrim(rtrim(isnull(eml_email,''))) as EmailID ,CNT_INTERNALID  as ID", " TBL_MASTER_CONTACT.CNT_INTERNALID=tbl_master_email.eml_cntid and eml_email !='' and eml_type='Official' and cnt_internalid like 'EM%'  ");
                        }
                        else
                        {
                            DT = oDBEngine.GetDataTable(" TBL_MASTER_CONTACT,tbl_master_email", " distinct  LTRIM(RTRIM(ISNULL(CNT_FIRSTNAME,'')))+' '+ LTRIM(RTRIM(ISNULL(CNT_MIDDLENAME,''))) +' ' + LTRIM(RTRIM(ISNULL(CNT_LASTNAME,''))) + ' ['+ LTRIM(RTRIM(ISNULL(CNT_UCC,CNT_SHORTNAME)))+']' as ClientName, ltrim(rtrim(isnull(eml_email,''))) as EmailID  ,CNT_INTERNALID  as ID", " TBL_MASTER_CONTACT.CNT_INTERNALID=tbl_master_email.eml_cntid and eml_email !='' and eml_type='Official' and cnt_internalid like 'EM%'  and cnt_internalid in(select grp_contactid from	tbl_trans_group where grp_groupmaster in(" + HdnGroup.Value + ")) ");
                        }
                    }
                    else
                    {
                        if (rdddlgrouptypeAll.Checked == true)
                        {
                            DT = oDBEngine.GetDataTable(" TBL_MASTER_CONTACT,tbl_master_email", " TOP 10  LTRIM(RTRIM(ISNULL(CNT_FIRSTNAME,'')))+' '+ LTRIM(RTRIM(ISNULL(CNT_MIDDLENAME,''))) +' ' + LTRIM(RTRIM(ISNULL(CNT_LASTNAME,''))) + ' ['+ LTRIM(RTRIM(ISNULL(CNT_UCC,CNT_SHORTNAME)))+']' as ClientName,ltrim(rtrim(isnull(eml_email,''))) as EmailID ,CNT_INTERNALID as ID  ", " TBL_MASTER_CONTACT.CNT_INTERNALID=tbl_master_email.eml_cntid and eml_email !='' and eml_type='Official' and cnt_internalid like 'CL%'  ");
                        }
                        else
                        {
                            DT = oDBEngine.GetDataTable(" TBL_MASTER_CONTACT,tbl_master_email", " TOP 10  LTRIM(RTRIM(ISNULL(CNT_FIRSTNAME,'')))+' '+ LTRIM(RTRIM(ISNULL(CNT_MIDDLENAME,''))) +' ' + LTRIM(RTRIM(ISNULL(CNT_LASTNAME,''))) + ' ['+ LTRIM(RTRIM(ISNULL(CNT_UCC,CNT_SHORTNAME)))+']' as ClientName,ltrim(rtrim(isnull(eml_email,''))) as EmailID ,CNT_INTERNALID  as ID  ", " TBL_MASTER_CONTACT.CNT_INTERNALID=tbl_master_email.eml_cntid and eml_email !='' and eml_type='Official' and cnt_internalid like 'CL%'  and cnt_internalid in(select grp_contactid from	tbl_trans_group where grp_groupmaster in(" + HdnGroup.Value + "))");
                        }
                    }
                }
            }
            else
            {
                if (cmbType.SelectedItem.Value == "CD")
                {
                    DT = oDBEngine.GetDataTable("MASTER_CDSLCLIENTS,TBL_MASTER_EMAIL ", " distinct   LTRIM(RTRIM(ISNULL(CDSLCLIENTS_FIRSTHOLDERNAME,'')))+' ['+ LTRIM(RTRIM(ISNULL(RIGHT(CDSLCLIENTS_BOID,8),''))) + '] ' as ClientName , ltrim(rtrim(isnull(eml_email,''))) as EmailID ,CDSLCLIENTS_BOID as ID ", " MASTER_CDSLCLIENTS.CDSLCLIENTS_BOID=tbl_master_email.eml_cntid and eml_email !='' and  eml_type='Official' and  CDSLCLIENTS_BOID in(" + HdnClient.Value + ") ");
                }
                else if (cmbType.SelectedItem.Value == "ND")
                {
                    DT = oDBEngine.GetDataTable(" MASTER_NSDLCLIENTS,TBL_MASTER_EMAIL ", " distinct  LTRIM(RTRIM(ISNULL(NSDLCLIENTS_BENFIRSTHOLDERNAME,''))) + ' ['+LTRIM(RTRIM(ISNULL(NSDLCLIENTS_BENACCOUNTID,''))) +']' as  ClientName, ltrim(rtrim(isnull(eml_email,''))) as EmailID ,NSDLCLIENTS_DPID +''+ CAST(NSDLCLIENTS_BENACCOUNTID AS  VARCHAR)  as ID", " NSDLCLIENTS_DPID +''+ CAST(NSDLCLIENTS_BENACCOUNTID AS  VARCHAR)=eml_cntid and eml_email !='' and  eml_type='Official'  and NSDLCLIENTS_DPID +''+ CAST(NSDLCLIENTS_BENACCOUNTID AS  VARCHAR) in (" + HdnClient.Value + ")  ");
                }
                else if (cmbType.SelectedItem.Value == "EM")
                {
                    DT = oDBEngine.GetDataTable(" TBL_MASTER_CONTACT,tbl_master_email", " distinct LTRIM(RTRIM(ISNULL(CNT_FIRSTNAME,'')))+' '+ LTRIM(RTRIM(ISNULL(CNT_MIDDLENAME,''))) +' ' + LTRIM(RTRIM(ISNULL(CNT_LASTNAME,''))) + ' ['+ LTRIM(RTRIM(ISNULL(CNT_UCC,CNT_SHORTNAME)))+']' as ClientName,ltrim(rtrim(isnull(eml_email,''))) as EmailID ,CNT_INTERNALID  as ID ", " TBL_MASTER_CONTACT.CNT_INTERNALID=tbl_master_email.eml_cntid and eml_email !='' and eml_type='Official' and cnt_internalid like 'EM%' and CNT_INTERNALID in (" + HdnClient.Value + ") ");
                }
                else
                {
                    DT = oDBEngine.GetDataTable(" TBL_MASTER_CONTACT,tbl_master_email", " distinct   LTRIM(RTRIM(ISNULL(CNT_FIRSTNAME,'')))+' '+ LTRIM(RTRIM(ISNULL(CNT_MIDDLENAME,''))) +' ' + LTRIM(RTRIM(ISNULL(CNT_LASTNAME,''))) + ' ['+ LTRIM(RTRIM(ISNULL(CNT_UCC,CNT_SHORTNAME)))+']' as ClientName,ltrim(rtrim(isnull(eml_email,''))) as EmailID  ,CNT_INTERNALID   as ID", " TBL_MASTER_CONTACT.CNT_INTERNALID=tbl_master_email.eml_cntid and eml_email !='' and eml_type='Official' and cnt_internalid like 'CL%'  and CNT_INTERNALID in (" + HdnClient.Value + ") ");

                }

            }
            #endregion

            DataTable dtCFG = oDBEngine.GetDataTable("CONFIG_EMAILACCOUNTS ", " TOP 1 EMAILACCOUNTS_COMPANYID,EMAILACCOUNTS_EMAILID  ", " EMAILACCOUNTS_SEGMENTID=1 AND EMAILACCOUNTS_INUSE='Y'");
            if (DT.Rows.Count > 0)
            {
                for (int i = 0; i < DT.Rows.Count; i++)
                {
                    DataSet dsCnt = new DataSet();
                    DataTable dtCnt = new DataTable();

                    #region GetTemlate
                    //using (SqlConnection con = new SqlConnection(ConfigurationSettings.AppSettings["DBConnectionDefault"]))
                    //{
                    //    using (SqlDataAdapter da = new SqlDataAdapter("[Fetch_EmailTemplateReservedWord]", con))
                    //    {
                    //        da.SelectCommand.Parameters.AddWithValue("@UsedFor", cmbType.SelectedItem.Value.ToString());
                    //        da.SelectCommand.Parameters.AddWithValue("@ContactID", DT.Rows[i]["ID"].ToString());
                    //        da.SelectCommand.Parameters.AddWithValue("@CompanyID", HttpContext.Current.Session["LastCompany"].ToString());
                    //        da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    //        if (con.State == ConnectionState.Closed)
                    //            con.Open();
                    //        dsCnt.Reset();
                    //        da.Fill(dsCnt);
                    //        dtCnt = dsCnt.Tables[0];

                    //        con.Close();
                    //        con.Dispose();
                    //    }
                    //}
                    dsCnt = oManagement_BL.Fetch_EmailTemplateReservedWord(Convert.ToString(cmbType.SelectedItem.Value), Convert.ToString(DT.Rows[i]["ID"]),
                        Convert.ToString(HttpContext.Current.Session["LastCompany"]));
                    dtCnt = dsCnt.Tables[0];
                    string TempContent = string.Empty;
                    TempContent = textEditor.Text;



                    if (cmbType.SelectedItem.Value.ToString() == "CL" || cmbType.SelectedItem.Value.ToString() == "EM")
                    {


                        // DataTable dtCnt = GetDataTable("tbl_master_contact ", " isnull(ltrim(rtrim(cnt_firstname)),'') as FirstName,isnull(ltrim(rtrim(cnt_middlename)),'') as MiddleName,isnull(ltrim(rtrim(cnt_lastname)),'') as LastName,isnull(ltrim(rtrim(cnt_ucc)),'') as [ClientCode],(select top 1 isnull(ltrim(rtrim(add_address1)),'') from tbl_master_address where add_cntId=cnt_internalID) as Addres1,(select top 1 isnull(ltrim(rtrim(add_address2)),'') from tbl_master_address where add_cntId=cnt_internalID) as Addres2,(select top 1 isnull(ltrim(rtrim(add_address3)),'') from tbl_master_address where add_cntId=cnt_internalID) as Addres3,(select top 1 isnull(ltrim(rtrim(city_name)),'') from tbl_master_City  where city_id in (select top 1 add_City from tbl_master_address where add_cntId=cnt_internalID)) as City,(select  top 1 isnull(ltrim(rtrim(state)),'') from tbl_master_state  where id in (select top 1 add_state from tbl_master_address where add_cntId=cnt_internalID)) as State,(select top 1 isnull(ltrim(rtrim(cou_country)),'') from tbl_master_country  where cou_id in (select top 1 add_country from tbl_master_address where add_cntId=cnt_internalID)) as Country,(select  top 1 isnull(ltrim(rtrim(add_pin)),'') from tbl_master_address where add_cntId=cnt_internalID) as Pin,(select top 1  isnull(ltrim(rtrim(phf_countryCode)),'') from tbl_master_phonefax  where phf_cntId=cnt_internalID) as [ISDCode],(select top 1  isnull(ltrim(rtrim(phf_areaCode)),'') from tbl_master_phonefax  where phf_cntId=cnt_internalID) as [STDCode],(select top 1  isnull(ltrim(rtrim(phf_phoneNumber)),'') from tbl_master_phonefax  where phf_cntId=cnt_internalID and phf_type<>'Mobile') as [TelephoneNumber],(select top 1  isnull(ltrim(rtrim(phf_phoneNumber)),'') from tbl_master_phonefax  where phf_cntId=cnt_internalID and phf_type='Mobile') as [MobNumber],REPLACE(CONVERT(VARCHAR(11), cnt_dOB, 106), ' ', '-')  as DateOfBirth,(select top 1 REPLACE(CONVERT(VARCHAR(11), crg_regisDate, 106), ' ', '-') from tbl_master_contactExchange  where crg_cntID=cnt_internalID) as ClientAgrementDate,(select top 1 crg_Number from tbl_master_contactRegistration where crg_cntid=cnt_internalID and crg_type='Pancard') as PANNumber,convert(varchar,getdate(),106) as CurrentDate ", " cnt_internalid ='" + contactID + "'");
                        TempContent = TempContent.ToString().Replace("#FirstName#", dtCnt.Rows[0]["FirstName"].ToString());
                        TempContent = TempContent.ToString().Replace("#MiddleName#", dtCnt.Rows[0]["MiddleName"].ToString());
                        TempContent = TempContent.ToString().Replace("#LastName#", dtCnt.Rows[0]["LastName"].ToString());
                        TempContent = TempContent.ToString().Replace("#ClientID#", dtCnt.Rows[0]["ClientCode"].ToString());
                        TempContent = TempContent.ToString().Replace("#Addres1#", dtCnt.Rows[0]["Addres1"].ToString());
                        TempContent = TempContent.ToString().Replace("#Addres2#", dtCnt.Rows[0]["Addres2"].ToString());
                        TempContent = TempContent.ToString().Replace("#Addres3#", dtCnt.Rows[0]["Addres3"].ToString());
                        TempContent = TempContent.ToString().Replace("#City#", dtCnt.Rows[0]["City"].ToString());
                        TempContent = TempContent.ToString().Replace("#State#", dtCnt.Rows[0]["State"].ToString());
                        TempContent = TempContent.ToString().Replace("#Country#", dtCnt.Rows[0]["Country"].ToString());
                        TempContent = TempContent.ToString().Replace("#Pin#", dtCnt.Rows[0]["Pin"].ToString());
                        TempContent = TempContent.ToString().Replace("#ISDCode#", dtCnt.Rows[0]["ISDCode"].ToString());
                        TempContent = TempContent.ToString().Replace("#STDCode#", dtCnt.Rows[0]["STDCode"].ToString());
                        TempContent = TempContent.ToString().Replace("#TelephoneNumber#", dtCnt.Rows[0]["TelephoneNumber"].ToString());
                        TempContent = TempContent.ToString().Replace("#MobNumber#", dtCnt.Rows[0]["MobNumber"].ToString());
                        TempContent = TempContent.ToString().Replace("#DateOfBirth#", dtCnt.Rows[0]["DateOfBirth"].ToString());
                        TempContent = TempContent.ToString().Replace("#ClientAgrementDate#", dtCnt.Rows[0]["ClientAgrementDate"].ToString());
                        TempContent = TempContent.ToString().Replace("#PANNumber#", dtCnt.Rows[0]["PANNumber"].ToString());
                        TempContent = TempContent.ToString().Replace("#CurrentDate#", dtCnt.Rows[0]["CurrentDate"].ToString());
                    }
                    else if (cmbType.SelectedItem.Value.ToString() == "CD")
                    {
                        TempContent = TempContent.ToString().Replace("#ClientName#", dtCnt.Rows[0]["FirstName"].ToString());
                        TempContent = TempContent.ToString().Replace("#ClientID#", dtCnt.Rows[0]["ClientCode"].ToString());
                        TempContent = TempContent.ToString().Replace("#Addres1#", dtCnt.Rows[0]["Addres1"].ToString());
                        TempContent = TempContent.ToString().Replace("#Addres2#", dtCnt.Rows[0]["Addres2"].ToString());
                        TempContent = TempContent.ToString().Replace("#Addres3#", dtCnt.Rows[0]["Addres3"].ToString());
                        TempContent = TempContent.ToString().Replace("#City#", dtCnt.Rows[0]["City"].ToString());
                        TempContent = TempContent.ToString().Replace("#State#", dtCnt.Rows[0]["State"].ToString());
                        TempContent = TempContent.ToString().Replace("#Pin#", dtCnt.Rows[0]["Pin"].ToString());
                        TempContent = TempContent.ToString().Replace("#TelephoneNumber#", dtCnt.Rows[0]["TelephoneNumber"].ToString());
                        TempContent = TempContent.ToString().Replace("#PANNumber#", dtCnt.Rows[0]["PANNumber"].ToString());
                        TempContent = TempContent.ToString().Replace("#CurrentDate#", dtCnt.Rows[0]["CurrentDate"].ToString());
                        TempContent = TempContent.ToString().Replace("#TradingCode#", dtCnt.Rows[0]["TradingUCC"].ToString());

                    }
                    else if (cmbType.SelectedItem.Value.ToString() == "ND")
                    {
                        TempContent = TempContent.ToString().Replace("#ClientName#", dtCnt.Rows[0]["FirstName"].ToString());
                        TempContent = TempContent.ToString().Replace("#ClientID#", dtCnt.Rows[0]["ClientCode"].ToString());
                        TempContent = TempContent.ToString().Replace("#Addres1#", dtCnt.Rows[0]["Addres1"].ToString());
                        TempContent = TempContent.ToString().Replace("#Addres2#", dtCnt.Rows[0]["Addres2"].ToString());
                        TempContent = TempContent.ToString().Replace("#Addres3#", dtCnt.Rows[0]["Addres3"].ToString());
                        TempContent = TempContent.ToString().Replace("#City#", dtCnt.Rows[0]["City"].ToString());
                        TempContent = TempContent.ToString().Replace("#Pin#", dtCnt.Rows[0]["Pin"].ToString());
                        TempContent = TempContent.ToString().Replace("#TelephoneNumber#", dtCnt.Rows[0]["TelephoneNumber"].ToString());
                        TempContent = TempContent.ToString().Replace("#PANNumber#", dtCnt.Rows[0]["PANNumber"].ToString());
                        TempContent = TempContent.ToString().Replace("#CurrentDate#", dtCnt.Rows[0]["CurrentDate"].ToString());
                        TempContent = TempContent.ToString().Replace("#TradingCode#", dtCnt.Rows[0]["TradingUCC"].ToString());
                    }
                    #endregion
                    string InternalID = "";
                    //using (SqlConnection lcon = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]))
                    //{
                    //    // SqlConnection lcon = new SqlConnection(con);
                    //    lcon.Open();
                    //    SqlCommand lcmdEmplInsert = new SqlCommand("InsertTransEmail", lcon);
                    //    lcmdEmplInsert.CommandType = CommandType.StoredProcedure;
                    //    lcmdEmplInsert.Parameters.AddWithValue("@Emails_SenderEmailID", dtCFG.Rows[0][1].ToString());
                    //    lcmdEmplInsert.Parameters.AddWithValue("@Emails_Subject", TxtSubject.Text.ToString());
                    //    lcmdEmplInsert.Parameters.AddWithValue("@Emails_Content", TempContent.ToString());
                    //    lcmdEmplInsert.Parameters.AddWithValue("@Emails_HasAttachement", "N");
                    //    lcmdEmplInsert.Parameters.AddWithValue("@Emails_CreateApplication", "1");
                    //    lcmdEmplInsert.Parameters.AddWithValue("@Emails_CreateUser", Session["userid"].ToString());
                    //    lcmdEmplInsert.Parameters.AddWithValue("@Emails_Type", "N");
                    //    lcmdEmplInsert.Parameters.AddWithValue("@Emails_CompanyID", dtCFG.Rows[0][0].ToString());
                    //    lcmdEmplInsert.Parameters.AddWithValue("@Emails_Segment", "CRM");
                    //    SqlParameter parameter = new SqlParameter("@result", SqlDbType.BigInt);
                    //    parameter.Direction = ParameterDirection.Output;
                    //    lcmdEmplInsert.Parameters.Add(parameter);
                    //    lcmdEmplInsert.ExecuteNonQuery();
                    //    InternalID = parameter.Value.ToString();
                    long result = 0;
                    oManagement_BL.InsertTransEmail(Convert.ToString(dtCFG.Rows[0][1]), Convert.ToString(TxtSubject.Text), Convert.ToString(TempContent), "N", 1,
                        Convert.ToInt32(Session["userid"]), "N", Convert.ToString(dtCFG.Rows[0][0]), "CRM", out result);
                    InternalID = Convert.ToString(result.ToString());
                    //    lcmdEmplInsert.Parameters.AddWithValue("@Emails_Segment", "CRM");

                    //  ###########---recipients-----------------                   

                    string fValues3 = "'" + InternalID + "','" + DT.Rows[i]["ID"].ToString() + "','" + DT.Rows[i]["EmailID"].ToString() + "','TO','" + oDBEngine.GetDate() + "','" + "P" + "'";
                    oDBEngine.InsurtFieldValue("Trans_EmailRecipients", "EmailRecipients_MainID,EmailRecipients_ContactLeadID,EmailRecipients_RecipientEmailID,EmailRecipients_RecipientType,EmailRecipients_SendDateTime,EmailRecipients_Status", fValues3);
                    //lcon.Close();
                    //lcon.Dispose();
                    //}
                    if (InternalID != "")
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "AlertF", "alert('Email Sent Seccessfully!...')", true);

                    }


                }

            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "AlertF", "alert('Customer/Email Id Not Found!...')", true);
            }



        }
        protected void btnPrint_Click(object sender, EventArgs e)
        {

            string Subject = TxtSubject.Text;
            string SenderType = cmbType.SelectedItem.Value;
            string TemplateID = "";
            string Content = "";
            FreeTextBox textEditor = FreeTextBoxPlaceHolder.FindControl("FreeTextBox1") as FreeTextBox;
            Content = textEditor.Text;
            string Brnch = "";
            if (rdbranchAll.Checked == true)
            {
                Brnch = Session["userbranchHierarchy"].ToString();
            }
            else
            {
                Brnch = HdnBranch.Value;
            }

            #region GetContact
            DataTable DT = new DataTable();

            if (rdbClientsAll.Checked == true)
            {
                if (ddlGroup.SelectedItem.Value == "0")
                {

                    if (cmbType.SelectedItem.Value == "CD")
                    {
                        DT = oDBEngine.GetDataTable("MASTER_CDSLCLIENTS ", " distinct   LTRIM(RTRIM(ISNULL(CDSLCLIENTS_FIRSTHOLDERNAME,'')))+' ['+ LTRIM(RTRIM(ISNULL(RIGHT(CDSLCLIENTS_BOID,8),''))) + '] ' as ClientName , CDSLCLIENTS_BOID as ID ", "  cdslclients_branchid in(" + Brnch + ") ");
                    }
                    else if (cmbType.SelectedItem.Value == "ND")
                    {
                        DT = oDBEngine.GetDataTable(" MASTER_NSDLCLIENTS ", " distinct  LTRIM(RTRIM(ISNULL(NSDLCLIENTS_BENFIRSTHOLDERNAME,''))) + ' ['+LTRIM(RTRIM(ISNULL(NSDLCLIENTS_BENACCOUNTID,''))) +']' as  ClientName, NSDLCLIENTS_DPID +''+ CAST(NSDLCLIENTS_BENACCOUNTID AS  VARCHAR)  as ID", " nsdlclients_branchid in (" + Brnch + ")  ");
                    }
                    else if (cmbType.SelectedItem.Value == "EM")
                    {
                        DT = oDBEngine.GetDataTable(" TBL_MASTER_CONTACT ", " distinct LTRIM(RTRIM(ISNULL(CNT_FIRSTNAME,'')))+' '+ LTRIM(RTRIM(ISNULL(CNT_MIDDLENAME,''))) +' ' + LTRIM(RTRIM(ISNULL(CNT_LASTNAME,''))) + ' ['+ LTRIM(RTRIM(ISNULL(CNT_UCC,CNT_SHORTNAME)))+']' as ClientName  ,CNT_INTERNALID  as ID ", "  cnt_internalid like 'EM%' and cnt_branchid in (" + Brnch + ") ");
                    }
                    else
                    {
                        DT = oDBEngine.GetDataTable(" TBL_MASTER_CONTACT  ", " distinct   LTRIM(RTRIM(ISNULL(CNT_FIRSTNAME,'')))+' '+ LTRIM(RTRIM(ISNULL(CNT_MIDDLENAME,''))) +' ' + LTRIM(RTRIM(ISNULL(CNT_LASTNAME,''))) + ' ['+ LTRIM(RTRIM(ISNULL(CNT_UCC,CNT_SHORTNAME)))+']' as ClientName  ,CNT_INTERNALID   as ID", "  cnt_internalid like 'CL%'  and cnt_branchid in (" + Brnch + ") ");

                    }

                }
                else
                {
                    if (cmbType.SelectedItem.Value == "CD")
                    {
                        if (rdddlgrouptypeAll.Checked == true)
                        {
                            DT = oDBEngine.GetDataTable("MASTER_CDSLCLIENTS  ", " distinct LTRIM(RTRIM(ISNULL(CDSLCLIENTS_FIRSTHOLDERNAME,'')))+' ['+ LTRIM(RTRIM(ISNULL(RIGHT(CDSLCLIENTS_BOID,8),''))) + ']'   as ClientName ,CDSLCLIENTS_BOID as ID ", null);

                        }
                        else
                        {
                            DT = oDBEngine.GetDataTable("MASTER_CDSLCLIENTS ", " distinct LTRIM(RTRIM(ISNULL(CDSLCLIENTS_FIRSTHOLDERNAME,'')))+' ['+ LTRIM(RTRIM(ISNULL(RIGHT(CDSLCLIENTS_BOID,8),''))) + ']' as ClientName ,CDSLCLIENTS_BOID  as ID ", "    CDSLCLIENTS_BOID in (select grp_contactid from	tbl_trans_group where grp_groupmaster in(" + HdnGroup.Value + ")) ");
                        }
                    }
                    else if (cmbType.SelectedItem.Value == "ND")
                    {
                        if (rdddlgrouptypeAll.Checked == true)
                        {
                            DT = oDBEngine.GetDataTable(" MASTER_NSDLCLIENTS ", " distinct LTRIM(RTRIM(ISNULL(NSDLCLIENTS_BENFIRSTHOLDERNAME,''))) + ' ['+LTRIM(RTRIM(ISNULL(NSDLCLIENTS_BENACCOUNTID,''))) +']' as ClientName ,NSDLCLIENTS_DPID +''+ CAST(NSDLCLIENTS_BENACCOUNTID AS  VARCHAR)  as ID ", null);
                        }
                        else
                        {
                            DT = oDBEngine.GetDataTable(" MASTER_NSDLCLIENTS  ", " distinct LTRIM(RTRIM(ISNULL(NSDLCLIENTS_BENFIRSTHOLDERNAME,''))) + ' ['+LTRIM(RTRIM(ISNULL(NSDLCLIENTS_BENACCOUNTID,''))) +']' as ClientName ,NSDLCLIENTS_DPID +''+ CAST(NSDLCLIENTS_BENACCOUNTID AS  VARCHAR) as ID ", "  NSDLCLIENTS_DPID +''+ CAST(NSDLCLIENTS_BENACCOUNTID AS  VARCHAR) in (select grp_contactid from	tbl_trans_group where grp_groupmaster in(" + HdnGroup.Value + "))  ");
                        }
                    }
                    else if (cmbType.SelectedItem.Value == "EM")
                    {
                        if (rdddlgrouptypeAll.Checked == true)
                        {
                            DT = oDBEngine.GetDataTable(" TBL_MASTER_CONTACT ", " distinct  LTRIM(RTRIM(ISNULL(CNT_FIRSTNAME,'')))+' '+ LTRIM(RTRIM(ISNULL(CNT_MIDDLENAME,''))) +' ' + LTRIM(RTRIM(ISNULL(CNT_LASTNAME,''))) + ' ['+ LTRIM(RTRIM(ISNULL(CNT_UCC,CNT_SHORTNAME)))+']' as ClientName ,CNT_INTERNALID  as ID", " cnt_internalid like 'EM%'  ");
                        }
                        else
                        {
                            DT = oDBEngine.GetDataTable(" TBL_MASTER_CONTACT ", " distinct  LTRIM(RTRIM(ISNULL(CNT_FIRSTNAME,'')))+' '+ LTRIM(RTRIM(ISNULL(CNT_MIDDLENAME,''))) +' ' + LTRIM(RTRIM(ISNULL(CNT_LASTNAME,''))) + ' ['+ LTRIM(RTRIM(ISNULL(CNT_UCC,CNT_SHORTNAME)))+']' as ClientName ,CNT_INTERNALID  as ID", " cnt_internalid like 'EM%'  and cnt_internalid in(select grp_contactid from	tbl_trans_group where grp_groupmaster in(" + HdnGroup.Value + ")) ");
                        }
                    }
                    else
                    {
                        if (rdddlgrouptypeAll.Checked == true)
                        {
                            DT = oDBEngine.GetDataTable(" TBL_MASTER_CONTACT ", " TOP 10  LTRIM(RTRIM(ISNULL(CNT_FIRSTNAME,'')))+' '+ LTRIM(RTRIM(ISNULL(CNT_MIDDLENAME,''))) +' ' + LTRIM(RTRIM(ISNULL(CNT_LASTNAME,''))) + ' ['+ LTRIM(RTRIM(ISNULL(CNT_UCC,CNT_SHORTNAME)))+']' as ClientName,CNT_INTERNALID as ID  ", "  cnt_internalid like 'CL%'  ");
                        }
                        else
                        {
                            DT = oDBEngine.GetDataTable(" TBL_MASTER_CONTACT ", " TOP 10  LTRIM(RTRIM(ISNULL(CNT_FIRSTNAME,'')))+' '+ LTRIM(RTRIM(ISNULL(CNT_MIDDLENAME,''))) +' ' + LTRIM(RTRIM(ISNULL(CNT_LASTNAME,''))) + ' ['+ LTRIM(RTRIM(ISNULL(CNT_UCC,CNT_SHORTNAME)))+']' as ClientName,CNT_INTERNALID  as ID  ", "  cnt_internalid like 'CL%'  and cnt_internalid in(select grp_contactid from	tbl_trans_group where grp_groupmaster in(" + HdnGroup.Value + "))");
                        }
                    }
                }
            }
            else
            {
                if (cmbType.SelectedItem.Value == "CD")
                {
                    DT = oDBEngine.GetDataTable("MASTER_CDSLCLIENTS  ", " distinct   LTRIM(RTRIM(ISNULL(CDSLCLIENTS_FIRSTHOLDERNAME,'')))+' ['+ LTRIM(RTRIM(ISNULL(RIGHT(CDSLCLIENTS_BOID,8),''))) + '] ' as ClientName ,CDSLCLIENTS_BOID as ID ", "   CDSLCLIENTS_BOID in(" + HdnClient.Value + ") ");
                }
                else if (cmbType.SelectedItem.Value == "ND")
                {
                    DT = oDBEngine.GetDataTable(" MASTER_NSDLCLIENTS  ", " distinct  LTRIM(RTRIM(ISNULL(NSDLCLIENTS_BENFIRSTHOLDERNAME,''))) + ' ['+LTRIM(RTRIM(ISNULL(NSDLCLIENTS_BENACCOUNTID,''))) +']' as  ClientName ,NSDLCLIENTS_DPID +''+ CAST(NSDLCLIENTS_BENACCOUNTID AS  VARCHAR)  as ID", "  NSDLCLIENTS_DPID +''+ CAST(NSDLCLIENTS_BENACCOUNTID AS  VARCHAR) in (" + HdnClient.Value + ")  ");
                }
                else if (cmbType.SelectedItem.Value == "EM")
                {
                    DT = oDBEngine.GetDataTable(" TBL_MASTER_CONTACT  ", " distinct LTRIM(RTRIM(ISNULL(CNT_FIRSTNAME,'')))+' '+ LTRIM(RTRIM(ISNULL(CNT_MIDDLENAME,''))) +' ' + LTRIM(RTRIM(ISNULL(CNT_LASTNAME,''))) + ' ['+ LTRIM(RTRIM(ISNULL(CNT_UCC,CNT_SHORTNAME)))+']' as ClientName  ,CNT_INTERNALID  as ID ", "  cnt_internalid like 'EM%' and CNT_INTERNALID in (" + HdnClient.Value + ") ");
                }
                else
                {
                    DT = oDBEngine.GetDataTable(" TBL_MASTER_CONTACT ", " distinct   LTRIM(RTRIM(ISNULL(CNT_FIRSTNAME,'')))+' '+ LTRIM(RTRIM(ISNULL(CNT_MIDDLENAME,''))) +' ' + LTRIM(RTRIM(ISNULL(CNT_LASTNAME,''))) + ' ['+ LTRIM(RTRIM(ISNULL(CNT_UCC,CNT_SHORTNAME)))+']' as ClientName ,CNT_INTERNALID   as ID", " cnt_internalid like 'CL%'  and CNT_INTERNALID in (" + HdnClient.Value + ") ");

                }

            }
            #endregion

            DataTable dtCFG = oDBEngine.GetDataTable("CONFIG_EMAILACCOUNTS ", " TOP 1 EMAILACCOUNTS_COMPANYID,EMAILACCOUNTS_EMAILID  ", " EMAILACCOUNTS_SEGMENTID=1 AND EMAILACCOUNTS_INUSE='Y'");
            DataTable dtReport = new DataTable();
            dtReport.Columns.Add(new DataColumn("ClientID", typeof(String)));
            dtReport.Columns.Add(new DataColumn("TemplateText", typeof(String)));


            if (DT.Rows.Count > 0)
            {
                for (int i = 0; i < DT.Rows.Count; i++)
                {
                    DataSet dsCnt = new DataSet();
                    DataTable dtCnt = new DataTable();

                    #region GetTemlate
                    //using (SqlConnection con = new SqlConnection(ConfigurationSettings.AppSettings["DBConnectionDefault"]))
                    //{
                    //    using (SqlDataAdapter da = new SqlDataAdapter("[Fetch_EmailTemplateReservedWord]", con))
                    //    {
                    //        da.SelectCommand.Parameters.AddWithValue("@UsedFor", cmbType.SelectedItem.Value.ToString());
                    //        da.SelectCommand.Parameters.AddWithValue("@ContactID", DT.Rows[i]["ID"].ToString());
                    //        da.SelectCommand.Parameters.AddWithValue("@CompanyID", HttpContext.Current.Session["LastCompany"].ToString());
                    //        da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    //        if (con.State == ConnectionState.Closed)
                    //            con.Open();
                    //        dsCnt.Reset();
                    //        da.Fill(dsCnt);
                    //        dtCnt = dsCnt.Tables[0];

                    //        con.Close();
                    //        con.Dispose();
                    //    }
                    //}
                    dsCnt = oManagement_BL.Fetch_EmailTemplateReservedWord(Convert.ToString(cmbType.SelectedItem.Value), Convert.ToString(DT.Rows[i]["ID"]),
                        Convert.ToString(HttpContext.Current.Session["LastCompany"]));
                    dtCnt = dsCnt.Tables[0];



                    string TempContent = string.Empty;
                    TempContent = textEditor.Text;
                    if (cmbType.SelectedItem.Value.ToString() == "CL" || cmbType.SelectedItem.Value.ToString() == "EM")
                    {
                        // DataTable dtCnt = GetDataTable("tbl_master_contact ", " isnull(ltrim(rtrim(cnt_firstname)),'') as FirstName,isnull(ltrim(rtrim(cnt_middlename)),'') as MiddleName,isnull(ltrim(rtrim(cnt_lastname)),'') as LastName,isnull(ltrim(rtrim(cnt_ucc)),'') as [ClientCode],(select top 1 isnull(ltrim(rtrim(add_address1)),'') from tbl_master_address where add_cntId=cnt_internalID) as Addres1,(select top 1 isnull(ltrim(rtrim(add_address2)),'') from tbl_master_address where add_cntId=cnt_internalID) as Addres2,(select top 1 isnull(ltrim(rtrim(add_address3)),'') from tbl_master_address where add_cntId=cnt_internalID) as Addres3,(select top 1 isnull(ltrim(rtrim(city_name)),'') from tbl_master_City  where city_id in (select top 1 add_City from tbl_master_address where add_cntId=cnt_internalID)) as City,(select  top 1 isnull(ltrim(rtrim(state)),'') from tbl_master_state  where id in (select top 1 add_state from tbl_master_address where add_cntId=cnt_internalID)) as State,(select top 1 isnull(ltrim(rtrim(cou_country)),'') from tbl_master_country  where cou_id in (select top 1 add_country from tbl_master_address where add_cntId=cnt_internalID)) as Country,(select  top 1 isnull(ltrim(rtrim(add_pin)),'') from tbl_master_address where add_cntId=cnt_internalID) as Pin,(select top 1  isnull(ltrim(rtrim(phf_countryCode)),'') from tbl_master_phonefax  where phf_cntId=cnt_internalID) as [ISDCode],(select top 1  isnull(ltrim(rtrim(phf_areaCode)),'') from tbl_master_phonefax  where phf_cntId=cnt_internalID) as [STDCode],(select top 1  isnull(ltrim(rtrim(phf_phoneNumber)),'') from tbl_master_phonefax  where phf_cntId=cnt_internalID and phf_type<>'Mobile') as [TelephoneNumber],(select top 1  isnull(ltrim(rtrim(phf_phoneNumber)),'') from tbl_master_phonefax  where phf_cntId=cnt_internalID and phf_type='Mobile') as [MobNumber],REPLACE(CONVERT(VARCHAR(11), cnt_dOB, 106), ' ', '-')  as DateOfBirth,(select top 1 REPLACE(CONVERT(VARCHAR(11), crg_regisDate, 106), ' ', '-') from tbl_master_contactExchange  where crg_cntID=cnt_internalID) as ClientAgrementDate,(select top 1 crg_Number from tbl_master_contactRegistration where crg_cntid=cnt_internalID and crg_type='Pancard') as PANNumber,convert(varchar,getdate(),106) as CurrentDate ", " cnt_internalid ='" + contactID + "'");
                        TempContent = TempContent.ToString().Replace("#FirstName#", dtCnt.Rows[0]["FirstName"].ToString());
                        TempContent = TempContent.ToString().Replace("#MiddleName#", dtCnt.Rows[0]["MiddleName"].ToString());
                        TempContent = TempContent.ToString().Replace("#LastName#", dtCnt.Rows[0]["LastName"].ToString());
                        TempContent = TempContent.ToString().Replace("#ClientID#", dtCnt.Rows[0]["ClientCode"].ToString());
                        TempContent = TempContent.ToString().Replace("#Addres1#", dtCnt.Rows[0]["Addres1"].ToString());
                        TempContent = TempContent.ToString().Replace("#Addres2#", dtCnt.Rows[0]["Addres2"].ToString());
                        TempContent = TempContent.ToString().Replace("#Addres3#", dtCnt.Rows[0]["Addres3"].ToString());
                        TempContent = TempContent.ToString().Replace("#City#", dtCnt.Rows[0]["City"].ToString());
                        TempContent = TempContent.ToString().Replace("#State#", dtCnt.Rows[0]["State"].ToString());
                        TempContent = TempContent.ToString().Replace("#Country#", dtCnt.Rows[0]["Country"].ToString());
                        TempContent = TempContent.ToString().Replace("#Pin#", dtCnt.Rows[0]["Pin"].ToString());
                        TempContent = TempContent.ToString().Replace("#ISDCode#", dtCnt.Rows[0]["ISDCode"].ToString());
                        TempContent = TempContent.ToString().Replace("#STDCode#", dtCnt.Rows[0]["STDCode"].ToString());
                        TempContent = TempContent.ToString().Replace("#TelephoneNumber#", dtCnt.Rows[0]["TelephoneNumber"].ToString());
                        TempContent = TempContent.ToString().Replace("#MobNumber#", dtCnt.Rows[0]["MobNumber"].ToString());
                        TempContent = TempContent.ToString().Replace("#DateOfBirth#", dtCnt.Rows[0]["DateOfBirth"].ToString());
                        TempContent = TempContent.ToString().Replace("#ClientAgrementDate#", dtCnt.Rows[0]["ClientAgrementDate"].ToString());
                        TempContent = TempContent.ToString().Replace("#PANNumber#", dtCnt.Rows[0]["PANNumber"].ToString());
                        TempContent = TempContent.ToString().Replace("#CurrentDate#", dtCnt.Rows[0]["CurrentDate"].ToString());
                    }
                    else if (cmbType.SelectedItem.Value.ToString() == "CD")
                    {
                        TempContent = TempContent.ToString().Replace("#ClientName#", dtCnt.Rows[0]["FirstName"].ToString());
                        TempContent = TempContent.ToString().Replace("#ClientID#", dtCnt.Rows[0]["ClientCode"].ToString());
                        TempContent = TempContent.ToString().Replace("#Addres1#", dtCnt.Rows[0]["Addres1"].ToString());
                        TempContent = TempContent.ToString().Replace("#Addres2#", dtCnt.Rows[0]["Addres2"].ToString());
                        TempContent = TempContent.ToString().Replace("#Addres3#", dtCnt.Rows[0]["Addres3"].ToString());
                        TempContent = TempContent.ToString().Replace("#City#", dtCnt.Rows[0]["City"].ToString());
                        TempContent = TempContent.ToString().Replace("#State#", dtCnt.Rows[0]["State"].ToString());
                        TempContent = TempContent.ToString().Replace("#Pin#", dtCnt.Rows[0]["Pin"].ToString());
                        TempContent = TempContent.ToString().Replace("#TelephoneNumber#", dtCnt.Rows[0]["TelephoneNumber"].ToString());
                        TempContent = TempContent.ToString().Replace("#PANNumber#", dtCnt.Rows[0]["PANNumber"].ToString());
                        TempContent = TempContent.ToString().Replace("#CurrentDate#", dtCnt.Rows[0]["CurrentDate"].ToString());
                        TempContent = TempContent.ToString().Replace("#TradingCode#", dtCnt.Rows[0]["TradingUCC"].ToString());

                    }
                    else if (cmbType.SelectedItem.Value.ToString() == "ND")
                    {
                        TempContent = TempContent.ToString().Replace("#ClientName#", dtCnt.Rows[0]["FirstName"].ToString());
                        TempContent = TempContent.ToString().Replace("#ClientID#", dtCnt.Rows[0]["ClientCode"].ToString());
                        TempContent = TempContent.ToString().Replace("#Addres1#", dtCnt.Rows[0]["Addres1"].ToString());
                        TempContent = TempContent.ToString().Replace("#Addres2#", dtCnt.Rows[0]["Addres2"].ToString());
                        TempContent = TempContent.ToString().Replace("#Addres3#", dtCnt.Rows[0]["Addres3"].ToString());
                        TempContent = TempContent.ToString().Replace("#City#", dtCnt.Rows[0]["City"].ToString());
                        TempContent = TempContent.ToString().Replace("#Pin#", dtCnt.Rows[0]["Pin"].ToString());
                        TempContent = TempContent.ToString().Replace("#TelephoneNumber#", dtCnt.Rows[0]["TelephoneNumber"].ToString());
                        TempContent = TempContent.ToString().Replace("#PANNumber#", dtCnt.Rows[0]["PANNumber"].ToString());
                        TempContent = TempContent.ToString().Replace("#CurrentDate#", dtCnt.Rows[0]["CurrentDate"].ToString());
                        TempContent = TempContent.ToString().Replace("#TradingCode#", dtCnt.Rows[0]["TradingUCC"].ToString());
                    }
                    #endregion

                    DataRow dr = dtReport.NewRow();
                    dr[0] = DT.Rows[i]["ID"].ToString();
                    dr[1] = TempContent.ToString();
                    dtReport.Rows.Add(dr);

                }
                dtReport.AcceptChanges();

                DataSet ds = new DataSet();
                ds.Tables.Add(dtReport);
                ReportDocument report = new ReportDocument();
                //ds.Tables[0].WriteXmlSchema("E:\\RPTXSD\\TemplatePrint.xsd");
                string tmpPdfPath = string.Empty;
                tmpPdfPath = HttpContext.Current.Server.MapPath("..\\management\\TemplatePrint.rpt");
                report.Load(tmpPdfPath);
                report.SetDataSource(ds.Tables[0]);
                report.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, HttpContext.Current.Response, true, "Template");
                report.Dispose();
                GC.Collect();



                //dtReport.AcceptChanges();
                //ReportDocument report = new ReportDocument();
                //dtReport.WriteXmlSchema("E:\\RPTXSD\\TemplatePrint.xsd");
                //report.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Portrait;
                //string tmpPdfPath = string.Empty;
                //tmpPdfPath = HttpContext.Current.Server.MapPath("..\\management\\TemplatePrint.rpt");           
                //report.Load(tmpPdfPath);
                //report.SetDataSource(DS.Tables[0]);
                //report.VerifyDatabase();
                //report.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, HttpContext.Current.Response, true, "Template");
                //report.Dispose();
                //GC.Collect();

            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "AlertF", "alert('Customer/Email Id Not Found!...')", true);
            }
        }


        protected string GetMandatory(string Template)
        {
            string TempContent = Template;
            if (TempContent.IndexOf("(#FirstName#)") > -1)
            {

            }
            if (TempContent.IndexOf("(#MiddleName#)") > -1)
            {

            }

            if (TempContent.IndexOf("(#LastName#)") > -1)
            {

            }
            if (TempContent.IndexOf("(#ClientID#)") > -1)
            {


            }
            if (TempContent.IndexOf("(#Addres1#)") > -1)
            {

            }
            if (TempContent.IndexOf("(#Addres2#)") > -1)
            {


            }
            if (TempContent.IndexOf("(#Addres3#)") > -1)
            {

            }
            if (TempContent.IndexOf("(#City#)") > -1)
            {

            }
            if (TempContent.IndexOf("(#State#)") > -1)
            {

            }
            if (TempContent.IndexOf("(#Country#)") > -1)
            {

            }
            if (TempContent.IndexOf("(#Pin#)") > -1)
            {

            }
            if (TempContent.IndexOf("(#ISDCode#)") > -1)
            {

            }
            if (TempContent.IndexOf("(#STDCode#)") > -1)
            {

            }
            if (TempContent.IndexOf("(#TelephoneNumber#)") > -1)
            {

            }
            if (TempContent.IndexOf("(#MobNumber#)") > -1)
            {

            }
            if (TempContent.IndexOf("(#DateOfBirth#)") > -1)
            {


            }
            if (TempContent.IndexOf("(#ClientAgrementDate#)") > -1)
            {

            }
            if (TempContent.IndexOf("(#PANNumber#)") > -1)
            {

            }
            if (TempContent.IndexOf("(#CurrentDate#)") > -1)
            {

            }
            if (TempContent.IndexOf("(#TradingCode#)") > -1)
            {

            }
            return TempContent;

        }

    }
}