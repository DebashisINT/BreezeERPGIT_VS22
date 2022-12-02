using System;
using System.Data;
using System.Web;
using System.Web.UI;
using BusinessLogicLayer;
namespace ERP.OMS.Reports
{
    public partial class Reports_GroupMemberList : System.Web.UI.Page, System.Web.UI.ICallbackEventHandler
    {
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        ClsDropDownlistNameSpace.clsDropDownList clsdrp = new ClsDropDownlistNameSpace.clsDropDownList();
        string data;
        static string SubClients = "";
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
            if (IsPostBack)
            {
                BindGrid();
            }

            if (HttpContext.Current.Session["userid"] == null)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
            //this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");
            Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script language='javascript'>Page_Load();</script>");
            //for sending email
            rbOnlyClient.Attributes.Add("OnClick", "SelectUserClient('Client')");
            rbClientUser.Attributes.Add("OnClick", "SelectUserClient('User')");
            txtSelectID.Attributes.Add("onkeyup", "callAjax1(this,'GetAllGroup',event)");
            String cbReference1 = Page.ClientScript.GetCallbackEventReference(this, "arg", "ReceiveSvrData", "context");
            String callbackScript1 = "function CallServer1(arg, context){ " + cbReference1 + ";}";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "CallServer1", callbackScript1, true);
            FillCombo();

        }
        string ICallbackEventHandler.GetCallbackResult()
        {
            return data;
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
                if (idlist[0] != "Clients")
                {
                    string[] val = cl[i].Split(';');
                    if (str == "")
                    {
                        str = val[0];
                        str1 = val[0] + ";" + val[1];
                    }
                    else
                    {
                        str += "," + val[0];
                        str1 += "," + val[0] + ";" + val[1];
                    }
                }
                else
                {

                    string[] val = cl[i].Split(';');
                    string[] AcVal = val[0].Split('-');
                    if (str == "")
                    {
                        if (idlist[0] == "EM")
                        {
                            str = AcVal[0];
                            str1 = AcVal[0] + ";" + val[1];
                        }
                        else
                        {
                            str = "'" + AcVal[0] + "'";
                            str1 = "'" + AcVal[0] + "'" + ";" + val[1];
                        }
                    }
                    else
                    {
                        if (idlist[0] == "EM")
                        {
                            str += "," + AcVal[0];
                            str1 += "," + AcVal[0] + ";" + val[1];
                        }
                        else
                        {
                            str += ",'" + AcVal[0] + "'";
                            str1 += "," + "'" + AcVal[0] + "'" + ";" + val[1];
                        }
                    }
                }
            }

            if (idlist[0] == "EM")
            {
                SubClients = str;
            }

        }
        private void FillCombo()
        {
            if (HttpContext.Current.Session["userlastsegment"].ToString() == "4")
            {
                string[,] r = new string[1, 2];
                r[0, 0] = "EM";
                r[0, 1] = "Employees";
                clsdrp.AddDataToDropDownList(r, cmbsearch);
            }
            else
            {
                string[,] r = new string[1, 2];
                r[0, 0] = "EM";
                r[0, 1] = "Employees";

                clsdrp.AddDataToDropDownList(r, cmbsearch);

            }

        }
        protected void GroupMembers_CustomJSProperties(object sender, DevExpress.Web.ASPxGridViewClientJSPropertiesEventArgs e)
        {
            e.Properties["cpHeight"] = "a";
        }
        protected void GroupMembers_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {

            GroupMembers.ClearSort();
            //BindGrid();
            if (e.Parameters == "s")
            {

                GroupMembers.Settings.ShowFilterRow = true;
            }
            else if (e.Parameters == "All")
            {

                GroupMembers.FilterExpression = string.Empty;
                BindGrid();
            }

        }
        public void BindGrid()
        {
            DataTable dt = new DataTable();
            if (HttpContext.Current.Session["userlastsegment"].ToString() == "9")
            {
                if (cmbGroup.SelectedItem.Value.ToString() == "ALL")
                {
                    if (rbOnlyClient.Checked)
                    {
                        dt = oDBEngine.GetDataTable("tbl_Trans_group as R", "grp_id,grp_contactId,(select gpm_Description  from tbl_master_groupmaster where gpm_id =R.grp_groupMaster) as GroupName,grp_groupMaster,grp_groupType,(case when  SUBSTRING( R.grp_contactId , 1 , 2 )='IN' then (select NsdlClients_BenFirstHolderName  from master_nsdlclients where nsdlClients_DPID+ cast(NsdlClients_BenAccountID as varchar(10))=R.grp_contactId)  else ''  end ) as MembersName, (select isnull(nsdlclients_benaccountid,'') from master_nsdlclients where nsdlClients_DPID+ cast(NsdlClients_BenAccountID as varchar(10))=R.grp_contactId) as Cnt_UCC, (select branch_description from tbl_master_branch where branch_id in (select Nsdlclients_branchID from master_nsdlclients where  nsdlClients_DPID+ cast(NsdlClients_BenAccountID as varchar(10))=R.grp_contactId)) as branch_desc, (select branch_code from tbl_master_branch where branch_id in (select Nsdlclients_branchID from master_nsdlclients where  nsdlClients_DPID+ cast(NsdlClients_BenAccountID as varchar(10))=R.grp_contactId)) as branch_Code  ", "  SUBSTRING( R.grp_contactId , 1 , 2 )='IN' ", " GroupName ");
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(SubClients))
                        {
                            dt = oDBEngine.GetDataTable("tbl_Trans_group as R", "grp_id,grp_contactId,(select gpm_Description  from tbl_master_groupmaster where gpm_id =R.grp_groupMaster) as GroupName,grp_groupMaster,grp_groupType,(case when  SUBSTRING( R.grp_contactId , 1 , 2 )='IN' then (select NsdlClients_BenFirstHolderName  from master_nsdlclients where nsdlClients_DPID+ cast(NsdlClients_BenAccountID as varchar(10))=R.grp_contactId)  else ''  end ) as MembersName, (select isnull(nsdlclients_benaccountid,'') from master_nsdlclients where nsdlClients_DPID+ cast(NsdlClients_BenAccountID as varchar(10))=R.grp_contactId) as Cnt_UCC, (select branch_description from tbl_master_branch where branch_id in (select Nsdlclients_branchID from master_nsdlclients where  nsdlClients_DPID+ cast(NsdlClients_BenAccountID as varchar(10))=R.grp_contactId)) as branch_desc, (select branch_code from tbl_master_branch where branch_id in (select Nsdlclients_branchID from master_nsdlclients where  nsdlClients_DPID+ cast(NsdlClients_BenAccountID as varchar(10))=R.grp_contactId)) as branch_Code  ", "  SUBSTRING( R.grp_contactId , 1 , 2 )='IN' ", " GroupName ");
                        }
                        else
                        {
                            dt = oDBEngine.GetDataTable("tbl_Trans_group as R", "grp_id,grp_contactId,(select gpm_Description  from tbl_master_groupmaster where gpm_id =R.grp_groupMaster) as GroupName,grp_groupMaster,grp_groupType,(case when  SUBSTRING( R.grp_contactId , 1 , 2 )='IN' then (select NsdlClients_BenFirstHolderName  from master_nsdlclients where nsdlClients_DPID+ cast(NsdlClients_BenAccountID as varchar(10))=R.grp_contactId)  else ''  end ) as MembersName, (select isnull(nsdlclients_benaccountid,'') from master_nsdlclients where nsdlClients_DPID+ cast(NsdlClients_BenAccountID as varchar(10))=R.grp_contactId) as Cnt_UCC, (select branch_description from tbl_master_branch where branch_id in (select Nsdlclients_branchID from master_nsdlclients where  nsdlClients_DPID+ cast(NsdlClients_BenAccountID as varchar(10))=R.grp_contactId)) as branch_desc, (select branch_code from tbl_master_branch where branch_id in (select Nsdlclients_branchID from master_nsdlclients where  nsdlClients_DPID+ cast(NsdlClients_BenAccountID as varchar(10))=R.grp_contactId)) as branch_Code  ", "  SUBSTRING( R.grp_contactId , 1 , 2 )='IN' and grp_groupMaster in (" + SubClients + ")", " GroupName ");
                        }
                    }


                }
                else
                {
                    if (rbOnlyClient.Checked)
                    {
                        dt = oDBEngine.GetDataTable("tbl_Trans_group as R", "grp_id,grp_contactId,(select gpm_Description  from tbl_master_groupmaster where gpm_id =R.grp_groupMaster) as GroupName,grp_groupMaster,grp_groupType,(case when  SUBSTRING( R.grp_contactId , 1 , 2 )='IN' then (select NsdlClients_BenFirstHolderName  from master_nsdlclients where nsdlClients_DPID+ cast(NsdlClients_BenAccountID as varchar(10))=R.grp_contactId)  else ''  end ) as MembersName, (select isnull(nsdlclients_benaccountid,'') from master_nsdlclients where nsdlClients_DPID+ cast(NsdlClients_BenAccountID as varchar(10))=R.grp_contactId) as Cnt_UCC, (select branch_description from tbl_master_branch where branch_id in (select Nsdlclients_branchID from master_nsdlclients where  nsdlClients_DPID+ cast(NsdlClients_BenAccountID as varchar(10))=R.grp_contactId)) as branch_desc, (select branch_code from tbl_master_branch where branch_id in (select Nsdlclients_branchID from master_nsdlclients where  nsdlClients_DPID+ cast(NsdlClients_BenAccountID as varchar(10))=R.grp_contactId)) as branch_Code  ", "  SUBSTRING( R.grp_contactId , 1 , 2 )='IN' and grp_groupType ='" + cmbGroup.SelectedItem.Value + "'", " GroupName ");
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(SubClients))
                        {
                            dt = oDBEngine.GetDataTable("tbl_Trans_group as R", "grp_id,grp_contactId,(select gpm_Description  from tbl_master_groupmaster where gpm_id =R.grp_groupMaster) as GroupName,grp_groupMaster,grp_groupType,(case when  SUBSTRING( R.grp_contactId , 1 , 2 )='IN' then (select NsdlClients_BenFirstHolderName  from master_nsdlclients where nsdlClients_DPID+ cast(NsdlClients_BenAccountID as varchar(10))=R.grp_contactId)  else ''  end ) as MembersName, (select isnull(nsdlclients_benaccountid,'') from master_nsdlclients where nsdlClients_DPID+ cast(NsdlClients_BenAccountID as varchar(10))=R.grp_contactId) as Cnt_UCC, (select branch_description from tbl_master_branch where branch_id in (select Nsdlclients_branchID from master_nsdlclients where  nsdlClients_DPID+ cast(NsdlClients_BenAccountID as varchar(10))=R.grp_contactId)) as branch_desc, (select branch_code from tbl_master_branch where branch_id in (select Nsdlclients_branchID from master_nsdlclients where  nsdlClients_DPID+ cast(NsdlClients_BenAccountID as varchar(10))=R.grp_contactId)) as branch_Code  ", "  SUBSTRING( R.grp_contactId , 1 , 2 )='IN' and grp_groupType ='" + cmbGroup.SelectedItem.Value + "'", " GroupName ");
                        }
                        else
                        {


                        } dt = oDBEngine.GetDataTable("tbl_Trans_group as R", "grp_id,grp_contactId,(select gpm_Description  from tbl_master_groupmaster where gpm_id =R.grp_groupMaster) as GroupName,grp_groupMaster,grp_groupType,(case when  SUBSTRING( R.grp_contactId , 1 , 2 )='IN' then (select NsdlClients_BenFirstHolderName  from master_nsdlclients where nsdlClients_DPID+ cast(NsdlClients_BenAccountID as varchar(10))=R.grp_contactId)  else ''  end ) as MembersName, (select isnull(nsdlclients_benaccountid,'') from master_nsdlclients where nsdlClients_DPID+ cast(NsdlClients_BenAccountID as varchar(10))=R.grp_contactId) as Cnt_UCC, (select branch_description from tbl_master_branch where branch_id in (select Nsdlclients_branchID from master_nsdlclients where  nsdlClients_DPID+ cast(NsdlClients_BenAccountID as varchar(10))=R.grp_contactId)) as branch_desc, (select branch_code from tbl_master_branch where branch_id in (select Nsdlclients_branchID from master_nsdlclients where  nsdlClients_DPID+ cast(NsdlClients_BenAccountID as varchar(10))=R.grp_contactId)) as branch_Code  ", "  SUBSTRING( R.grp_contactId , 1 , 2 )='IN' and grp_groupMaster in (" + SubClients + ") and grp_groupType ='" + cmbGroup.SelectedItem.Value + "'", " GroupName ");
                    }


                }

            }
            else if (HttpContext.Current.Session["userlastsegment"].ToString() == "10")
            {
                if (cmbGroup.SelectedItem.Value.ToString() == "ALL")
                {
                    if (rbOnlyClient.Checked)
                    {
                        dt = oDBEngine.GetDataTable("tbl_Trans_group as R", " grp_id,grp_contactId,(select gpm_Description  from tbl_master_groupmaster where gpm_id =R.grp_groupMaster) as GroupName,grp_groupMaster,grp_groupType, (case when  SUBSTRING( R.grp_contactId , 1 , 8 ) ='" + Session["usersegid"].ToString() + "'   then (select CdslClients_FirstHolderName from master_cdslclients where cdslclients_BOID=R.grp_contactId)  else ''  end ) as MembersName, (select isnull(cdslclients_BOID,'') from master_cdslclients where cdslclients_BOID=R.grp_contactId) as Cnt_UCC, (select branch_description from tbl_master_branch where branch_id in (select CdslClients_BranchID from master_cdslclients where  cdslclients_BOID=R.grp_contactId)) as branch_desc, (select branch_code from tbl_master_branch where branch_id in (select CdslClients_BranchID from master_cdslclients where  cdslclients_BOID=R.grp_contactId)) as branch_Code ", " SUBSTRING( R.grp_contactId , 1 , 8 )='" + Session["usersegid"].ToString() + "' ", " GroupName ");
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(SubClients))
                        {
                            dt = oDBEngine.GetDataTable("tbl_Trans_group as R", " grp_id,grp_contactId,(select gpm_Description  from tbl_master_groupmaster where gpm_id =R.grp_groupMaster) as GroupName,grp_groupMaster,grp_groupType, (case when  SUBSTRING( R.grp_contactId , 1 , 8 ) ='" + Session["usersegid"].ToString() + "'   then (select CdslClients_FirstHolderName from master_cdslclients where cdslclients_BOID=R.grp_contactId)  else ''  end ) as MembersName, (select isnull(cdslclients_BOID,'') from master_cdslclients where cdslclients_BOID=R.grp_contactId) as Cnt_UCC, (select branch_description from tbl_master_branch where branch_id in (select CdslClients_BranchID from master_cdslclients where  cdslclients_BOID=R.grp_contactId)) as branch_desc, (select branch_code from tbl_master_branch where branch_id in (select CdslClients_BranchID from master_cdslclients where  cdslclients_BOID=R.grp_contactId)) as branch_Code ", " SUBSTRING( R.grp_contactId , 1 , 8 )='" + Session["usersegid"].ToString() + "' ", " GroupName ");
                        }
                        else
                        {
                            dt = oDBEngine.GetDataTable("tbl_Trans_group as R", " grp_id,grp_contactId,(select gpm_Description  from tbl_master_groupmaster where gpm_id =R.grp_groupMaster) as GroupName,grp_groupMaster,grp_groupType, (case when  SUBSTRING( R.grp_contactId , 1 , 8 ) ='" + Session["usersegid"].ToString() + "'   then (select CdslClients_FirstHolderName from master_cdslclients where cdslclients_BOID=R.grp_contactId)  else ''  end ) as MembersName, (select isnull(cdslclients_BOID,'') from master_cdslclients where cdslclients_BOID=R.grp_contactId) as Cnt_UCC, (select branch_description from tbl_master_branch where branch_id in (select CdslClients_BranchID from master_cdslclients where  cdslclients_BOID=R.grp_contactId)) as branch_desc, (select branch_code from tbl_master_branch where branch_id in (select CdslClients_BranchID from master_cdslclients where  cdslclients_BOID=R.grp_contactId)) as branch_Code", " SUBSTRING( R.grp_contactId , 1 , 8 )='" + Session["usersegid"].ToString() + "'  and grp_groupMaster in (" + SubClients + ")", " GroupName ");
                        }
                    }


                }
                else
                {
                    if (rbOnlyClient.Checked)
                    {
                        dt = oDBEngine.GetDataTable("tbl_Trans_group as R", " grp_id,grp_contactId,(select gpm_Description  from tbl_master_groupmaster where gpm_id =R.grp_groupMaster) as GroupName,grp_groupMaster,grp_groupType, (case when  SUBSTRING( R.grp_contactId , 1 , 8 ) ='" + Session["usersegid"].ToString() + "'   then (select CdslClients_FirstHolderName from master_cdslclients where cdslclients_BOID=R.grp_contactId)  else ''  end ) as MembersName, (select isnull(cdslclients_BOID,'') from master_cdslclients where cdslclients_BOID=R.grp_contactId) as Cnt_UCC, (select branch_description from tbl_master_branch where branch_id in (select CdslClients_BranchID from master_cdslclients where  cdslclients_BOID=R.grp_contactId)) as branch_desc, (select branch_code from tbl_master_branch where branch_id in (select CdslClients_BranchID from master_cdslclients where  cdslclients_BOID=R.grp_contactId)) as branch_Code ", "SUBSTRING( R.grp_contactId , 1 , 8 ) ='" + Session["usersegid"].ToString() + "'   and grp_groupType ='" + cmbGroup.SelectedItem.Value + "'", " GroupName ");
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(SubClients))
                        {
                            dt = oDBEngine.GetDataTable("tbl_Trans_group as R", " grp_id,grp_contactId,(select gpm_Description  from tbl_master_groupmaster where gpm_id =R.grp_groupMaster) as GroupName,grp_groupMaster,grp_groupType, (case when  SUBSTRING( R.grp_contactId , 1 , 8 ) ='" + Session["usersegid"].ToString() + "'   then (select CdslClients_FirstHolderName from master_cdslclients where cdslclients_BOID=R.grp_contactId)  else ''  end ) as MembersName, (select isnull(cdslclients_BOID,'') from master_cdslclients where cdslclients_BOID=R.grp_contactId) as Cnt_UCC, (select branch_description from tbl_master_branch where branch_id in (select CdslClients_BranchID from master_cdslclients where  cdslclients_BOID=R.grp_contactId)) as branch_desc, (select branch_code from tbl_master_branch where branch_id in (select CdslClients_BranchID from master_cdslclients where  cdslclients_BOID=R.grp_contactId)) as branch_Code ", "SUBSTRING( R.grp_contactId , 1 , 8 ) ='" + Session["usersegid"].ToString() + "'   and grp_groupType ='" + cmbGroup.SelectedItem.Value + "'", " GroupName ");
                        }
                        else
                        {
                            dt = oDBEngine.GetDataTable("tbl_Trans_group as R", " grp_id,grp_contactId,(select gpm_Description  from tbl_master_groupmaster where gpm_id =R.grp_groupMaster) as GroupName,grp_groupMaster,grp_groupType, (case when  SUBSTRING( R.grp_contactId , 1 , 8 ) ='" + Session["usersegid"].ToString() + "'   then (select CdslClients_FirstHolderName from master_cdslclients where cdslclients_BOID=R.grp_contactId)  else ''  end ) as MembersName, (select isnull(cdslclients_BOID,'') from master_cdslclients where cdslclients_BOID=R.grp_contactId) as Cnt_UCC, (select branch_description from tbl_master_branch where branch_id in (select CdslClients_BranchID from master_cdslclients where  cdslclients_BOID=R.grp_contactId)) as branch_desc, (select branch_code from tbl_master_branch where branch_id in (select CdslClients_BranchID from master_cdslclients where  cdslclients_BOID=R.grp_contactId)) as branch_Code", "SUBSTRING( R.grp_contactId , 1 , 8 ) ='" + Session["usersegid"].ToString() + "'   and  grp_groupMaster in (" + SubClients + ") and grp_groupType ='" + cmbGroup.SelectedItem.Value + "'", " GroupName ");
                        }
                    }


                }

            }
            else
            {
                if (cmbGroup.SelectedItem.Value.ToString() == "ALL")
                {
                    if (rbOnlyClient.Checked)
                    {
                        dt = oDBEngine.GetDataTable("tbl_Trans_group as R", "grp_id,grp_contactId,(select gpm_Description  from tbl_master_groupmaster where gpm_id =R.grp_groupMaster) as GroupName,grp_groupMaster,grp_groupType,(case when  SUBSTRING( R.grp_contactId , 1 , 2 )='CL'  then (select isnull(RTRIM(cnt_firstName),'')+' '+isnull(RTRIM(cnt_middleName),'')+' '+isnull(RTRIM(cnt_lastName),'')+' ['+isnull(cnt_UCC,'')+']' from tbl_master_contact where cnt_internalId=R.grp_contactId) else ''  end ) as MembersName, (select isnull(cnt_UCC,'') from tbl_master_contact where cnt_internalid=R.grp_contactId) as Cnt_UCC, (select branch_description from tbl_master_branch where branch_id in (select cnt_branchid from tbl_master_contact where  cnt_internalId=R.grp_contactId)) as branch_desc, (select branch_code from tbl_master_branch where branch_id in (select cnt_branchid from tbl_master_contact where  cnt_internalId=R.grp_contactId)) as branch_Code   ", " SUBSTRING( R.grp_contactId , 1 , 2 )='CL' ", " GroupName ");
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(SubClients))
                        {
                            dt = oDBEngine.GetDataTable("tbl_Trans_group as R", "grp_id,grp_contactId,(select gpm_Description  from tbl_master_groupmaster where gpm_id =R.grp_groupMaster) as GroupName,grp_groupMaster,grp_groupType,(case when  SUBSTRING( R.grp_contactId , 1 , 2 )='CL'  then (select isnull(RTRIM(cnt_firstName),'')+' '+isnull(RTRIM(cnt_middleName),'')+' '+isnull(RTRIM(cnt_lastName),'')+' ['+isnull(cnt_UCC,'')+']' from tbl_master_contact where cnt_internalId=R.grp_contactId) else ''  end ) as MembersName, (select isnull(cnt_UCC,'') from tbl_master_contact where cnt_internalid=R.grp_contactId) as Cnt_UCC, (select branch_description from tbl_master_branch where branch_id in (select cnt_branchid from tbl_master_contact where  cnt_internalId=R.grp_contactId)) as branch_desc, (select branch_code from tbl_master_branch where branch_id in (select cnt_branchid from tbl_master_contact where  cnt_internalId=R.grp_contactId)) as branch_Code   ", " SUBSTRING( R.grp_contactId , 1 , 2 )='CL' ", " GroupName ");
                        }
                        else
                        {
                            dt = oDBEngine.GetDataTable("tbl_Trans_group as R", "grp_id,grp_contactId,(select gpm_Description  from tbl_master_groupmaster where gpm_id =R.grp_groupMaster) as GroupName,grp_groupMaster,grp_groupType,(case when  SUBSTRING( R.grp_contactId , 1 , 2 )='CL'  then (select isnull(RTRIM(cnt_firstName),'')+' '+isnull(RTRIM(cnt_middleName),'')+' '+isnull(RTRIM(cnt_lastName),'')+' ['+isnull(cnt_UCC,'')+']' from tbl_master_contact where cnt_internalId=R.grp_contactId) else ''  end ) as MembersName, (select isnull(cnt_UCC,'') from tbl_master_contact where cnt_internalid=R.grp_contactId) as Cnt_UCC, (select branch_description from tbl_master_branch where branch_id in (select cnt_branchid from tbl_master_contact where  cnt_internalId=R.grp_contactId)) as branch_desc, (select branch_code from tbl_master_branch where branch_id in (select cnt_branchid from tbl_master_contact where  cnt_internalId=R.grp_contactId)) as branch_Code   ", "SUBSTRING( R.grp_contactId , 1 , 2 )='CL' and  grp_groupMaster in (" + SubClients + ")", " GroupName ");
                        }
                    }


                }
                else
                {
                    if (rbOnlyClient.Checked)
                    {
                        dt = oDBEngine.GetDataTable("tbl_Trans_group as R", "grp_id,grp_contactId,(select gpm_Description  from tbl_master_groupmaster where gpm_id =R.grp_groupMaster) as GroupName,grp_groupMaster,grp_groupType,(case when  SUBSTRING( R.grp_contactId , 1 , 2 )='CL'  then (select isnull(RTRIM(cnt_firstName),'')+' '+isnull(RTRIM(cnt_middleName),'')+' '+isnull(RTRIM(cnt_lastName),'')+' ['+isnull(cnt_UCC,'')+']' from tbl_master_contact where cnt_internalId=R.grp_contactId) else ''  end ) as MembersName, (select isnull(cnt_UCC,'') from tbl_master_contact where cnt_internalid=R.grp_contactId) as Cnt_UCC, (select branch_description from tbl_master_branch where branch_id in (select cnt_branchid from tbl_master_contact where  cnt_internalId=R.grp_contactId)) as branch_desc, (select branch_code from tbl_master_branch where branch_id in (select cnt_branchid from tbl_master_contact where  cnt_internalId=R.grp_contactId)) as branch_Code   ", " SUBSTRING( R.grp_contactId , 1 , 2 )='CL' and grp_groupType ='" + cmbGroup.SelectedItem.Value + "'", " GroupName ");
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(SubClients))
                        {
                            dt = oDBEngine.GetDataTable("tbl_Trans_group as R", "grp_id,grp_contactId,(select gpm_Description  from tbl_master_groupmaster where gpm_id =R.grp_groupMaster) as GroupName,grp_groupMaster,grp_groupType,(case when  SUBSTRING( R.grp_contactId , 1 , 2 )='CL'  then (select isnull(RTRIM(cnt_firstName),'')+' '+isnull(RTRIM(cnt_middleName),'')+' '+isnull(RTRIM(cnt_lastName),'')+' ['+isnull(cnt_UCC,'')+']' from tbl_master_contact where cnt_internalId=R.grp_contactId) else ''  end ) as MembersName, (select isnull(cnt_UCC,'') from tbl_master_contact where cnt_internalid=R.grp_contactId) as Cnt_UCC, (select branch_description from tbl_master_branch where branch_id in (select cnt_branchid from tbl_master_contact where  cnt_internalId=R.grp_contactId)) as branch_desc, (select branch_code from tbl_master_branch where branch_id in (select cnt_branchid from tbl_master_contact where  cnt_internalId=R.grp_contactId)) as branch_Code   ", " SUBSTRING( R.grp_contactId , 1 , 2 )='CL' and grp_groupType ='" + cmbGroup.SelectedItem.Value + "'", " GroupName ");
                        }
                        else
                        {
                            dt = oDBEngine.GetDataTable("tbl_Trans_group as R", "grp_id,grp_contactId,(select gpm_Description  from tbl_master_groupmaster where gpm_id =R.grp_groupMaster) as GroupName,grp_groupMaster,grp_groupType,(case when  SUBSTRING( R.grp_contactId , 1 , 2 )='CL'  then (select isnull(RTRIM(cnt_firstName),'')+' '+isnull(RTRIM(cnt_middleName),'')+' '+isnull(RTRIM(cnt_lastName),'')+' ['+isnull(cnt_UCC,'')+']' from tbl_master_contact where cnt_internalId=R.grp_contactId) else ''  end ) as MembersName, (select isnull(cnt_UCC,'') from tbl_master_contact where cnt_internalid=R.grp_contactId) as Cnt_UCC, (select branch_description from tbl_master_branch where branch_id in (select cnt_branchid from tbl_master_contact where  cnt_internalId=R.grp_contactId)) as branch_desc, (select branch_code from tbl_master_branch where branch_id in (select cnt_branchid from tbl_master_contact where  cnt_internalId=R.grp_contactId)) as branch_Code  ", "SUBSTRING( R.grp_contactId , 1 , 2 )='CL' and  grp_groupMaster in (" + SubClients + ") and grp_groupType ='" + cmbGroup.SelectedItem.Value + "'", " GroupName ");
                        }
                    }


                }


            }


            GroupMembers.DataSource = dt;
            GroupMembers.DataBind();
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            BindGrid();
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
            }
        }
    }
}