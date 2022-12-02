using System;
using System.Web;
using System.Web.UI;
//using DevExpress.Web;
using DevExpress.Web;
using BusinessLogicLayer;
using System.Configuration;

namespace ERP.OMS.Management
{
    public partial class Reports_emp_group : System.Web.UI.Page, System.Web.UI.ICallbackEventHandler
    {

        static string Branch;
        static string Clients;
        static string InsuComp;
        static string Products;
        static string ReportTo;
        static string SaleRep;
        static string Associate;
        static string SubBroker;
        string data;
        BusinessLogicLayer.Converter oconverter = new BusinessLogicLayer.Converter();


        public string pageAccess = "";
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        BusinessLogicLayer.Converter OConvert = new BusinessLogicLayer.Converter();
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

            //------- For Read Only User in SQL Datasource Connection String   Start-----------------

            if (HttpContext.Current.Session["EntryProfileType"] != null)
            {
                if (Convert.ToString(HttpContext.Current.Session["EntryProfileType"]) == "R")
                {
                    //EmployeeDataSource.ConnectionString = ConfigurationSettings.AppSettings["DBReadOnlyConnection"];MULTI
                    EmployeeDataSource.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                }
                else
                {
                    //EmployeeDataSource.ConnectionString = ConfigurationSettings.AppSettings["DBConnectionDefault"];MULTI
                    EmployeeDataSource.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                }
            }

            //------- For Read Only User in SQL Datasource Connection String   End-----------------


            // --New Part ------------------

            if (!IsPostBack)
            {





                dtDate.EditFormatString = oconverter.GetDateFormat("Date");
                dtDate.Value = oDBEngine.GetDate().AddDays((-1 * oDBEngine.GetDate().Day) + 1);
                dtToDate.EditFormatString = oconverter.GetDateFormat("Date");
                dtToDate.Value = oDBEngine.GetDate();

                rdbbAll.Attributes.Add("OnClick", "SegAll('Branch')");
                //rdbClientA.Attributes.Add("OnClick", "SegAll('Clients')");
                rdbInsuCompA.Attributes.Add("OnClick", "SegAll('Company')");
                //rdbProductA.Attributes.Add("OnClick", "SegAll('Products')");
                rdbTelecallerA.Attributes.Add("OnClick", "SegAll('ReportTo')");
                rdbSalesRepresentativeA.Attributes.Add("OnClick", "SegAll('Employee')");
                //rdbAssociateA.Attributes.Add("OnClick", "SegAll('Associate')");
                //rdbSubBroakerA.Attributes.Add("OnClick", "SegAll('Sub Broker')");

                rdbbSelected.Attributes.Add("OnClick", "SegSelected('Branch')");
                //rdbClientS.Attributes.Add("OnClick", "SegSelected('Clients')");
                rdbInsuCompS.Attributes.Add("OnClick", "SegSelected('Company')");
                //rdbProductS.Attributes.Add("OnClick", "SegSelected('Products')");
                rdbTelecallerS.Attributes.Add("OnClick", "SegSelected('ReportTo')");
                rdbSalesRepresentativeS.Attributes.Add("OnClick", "SegSelected('Employee')");
                //rdbAssociateS.Attributes.Add("OnClick", "SegSelected('Associate')");
                //rdbSubBroakerS.Attributes.Add("OnClick", "SegSelected('Sub Broker')");

                txtsubscriptionID.Attributes.Add("onkeyup", "showOptions(this,'SearcBranchWiseEmployee',event)");
                Page.ClientScript.RegisterStartupScript(GetType(), "pageload", "<script>PageLoad();</script>");
                //_____For performing operation without refreshing page___//
                String cbReference = Page.ClientScript.GetCallbackEventReference(this, "arg", "ReceiveServerData", "context");
                String callbackScript = "function CallServer(arg, context){ " + cbReference + ";}";
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "CallServer", callbackScript, true);
                //___________-end here___//




            }
            if (HttpContext.Current.Session["userid"] == null)
            {
               //Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
            //this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");
            // GridBindEmployee();
            BindEmployee();

        }


        void ICallbackEventHandler.RaiseCallbackEvent(string eventArgument)
        {
            string id = eventArgument.ToString();
            string str = "";
            string str1 = "";
            string[] idlist = id.Split('~');
            if (idlist.Length > 1)
            {

                string[] SelectedValue = idlist[1].Split(',');

                for (int i = 0; i < SelectedValue.Length; i++)
                {
                    string[] val = SelectedValue[i].Split(';');
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
            }
            if (idlist[0] == "Branch")
            {
                Branch = str;
                data = "Branch~" + str1;
            }
            //else if (idlist[0] == "Clients")
            //{
            //    Clients = str;
            //    data = "Clients~" + str1;
            //}
            else if (idlist[0] == "Company")
            {
                InsuComp = str;
                data = "InsuComp~" + str1;
            }
            //else if (idlist[0] == "Products")
            //{
            //    Products = str;
            //    data = "Products~" + str1;
            //}
            else if (idlist[0] == "ReportTo")
            {
                ReportTo = str;
                data = "ReportTo~" + str1;
            }
            else if (idlist[0] == "Employee")
            {
                SaleRep = str;
                data = "SaleRep~" + str1;
            }
            //else if (idlist[0] == "Associate")
            //{
            //    Associate = str;
            //    data = "Associate~" + str1;
            //}
            //else if (idlist[0] == "Sub Broker")
            //{
            //    SubBroker = str;
            //    data = "SubBroker~" + str1;
            //}

        }
        string ICallbackEventHandler.GetCallbackResult()
        {
            return data;
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
        protected void EmployeeGrid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            if (e.Parameters == "s")
                EmployeeGrid.Settings.ShowFilterRow = true;
            else if (e.Parameters == "All")
            {
                EmployeeGrid.FilterExpression = string.Empty;
            }
            else
            {
                EmployeeGrid.ClearSort();
            }
        }
        //protected void btnShow_Click(object sender, EventArgs e)
        //{
        //    GridBindEmployee();
        //}
        protected void Button1_Click(object sender, EventArgs e)
        {
            BindEmployee();

        }

        public void GridBindEmployee()
        {


            String LIS = HttpContext.Current.Session["userbranchHierarchy"].ToString();
            EmployeeDataSource.SelectCommand = " select top 10 ISNULL(tbl_master_contact.cnt_firstName, '') + ' ' + ISNULL(tbl_master_contact.cnt_middleName, '') + ' ' + ISNULL(tbl_master_contact.cnt_lastName, '') AS Name," +
                                                " tbl_master_contact.cnt_internalId AS Id,tbl_master_branch.branch_description AS BranchName,(ISNULL(Cont.cnt_firstName, '') + ' ' + ISNULL(Cont.cnt_middleName, '') + ' ' + ISNULL(Cont.cnt_lastName, '')+ ' [' + ISNULL(Cont.cnt_shortName, '')+']') as ReportTo,tbl_master_employee.emp_dateofjoining as DOJ, " +
                                                " (Select (case when (Select top 1  '(M)'+ phf_phonenumber    from tbl_master_phonefax where phf_cntid=tbl_master_contact.cnt_internalId and phf_type = 'Mobile' ) is NULL then '' else ((Select top 1  '(M)'+ phf_phonenumber    from tbl_master_phonefax where phf_cntid=tbl_master_contact.cnt_internalId and phf_type = 'Mobile' ))  end) )as phone," +
                                                " (select tbl_master_company.cmp_name from tbl_master_company where tbl_master_company.cmp_id=ctc.emp_organization ) as CompName," +
                                                " tbl_master_user.user_name as user_name," +
                                                " (select cost_description from tbl_master_costCenter where cost_id =ctc.emp_Department ) as cost_description ," +
                                                " (select deg_designation from tbl_master_designation where deg_id =ctc.emp_designation ) as deg_designation," +
                                                " tbl_master_contact.cnt_id,tbl_master_contact.cnt_shortName, " +
                                                " (select top 1 crg_number from tbl_master_contactregistration where crg_type='Pancard' and crg_cntId=tbl_master_contact.cnt_InternalId) as PanCard " +
                                                " from tbl_master_contact, tbl_master_branch, tbl_master_employee,tbl_master_user,tbl_master_salutation,tbl_trans_employeeCTC ctc ,tbl_master_contact Cont,   tbl_master_employee emp" +
                                                " where tbl_master_contact.cnt_internalId=ctc.emp_cntId and (ctc.emp_effectiveuntil is null OR ctc.emp_effectiveuntil>getdate() OR ctc.emp_effectiveuntil='1/1/1900 12:00:00 AM')  and ctc.emp_Organization in(select cmp_id from tbl_master_company where cmp_internalid in (" + HttpContext.Current.Session["userCompanyHierarchy"] + ")) and " +
                                                " tbl_master_contact.cnt_branchid  in (" + HttpContext.Current.Session["userbranchHierarchy"] + ") and tbl_master_contact.cnt_contactType='" + HttpContext.Current.Session["userContactType"] + "' and tbl_master_user.user_id = tbl_master_contact.createUser and tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id and tbl_master_contact.cnt_internalId = tbl_master_employee.emp_contactId  and tbl_master_salutation.sal_id = tbl_master_contact.cnt_salutation and (tbl_master_employee.emp_dateofLeaving is null or tbl_master_employee.emp_dateofLeaving='1/1/1900 12:00:00 AM') and ctc.emp_reportTo=emp.emp_id and emp.emp_contactID=cont.cnt_internalID   order by tbl_master_contact.createdate  desc";
            EmployeeGrid.DataBind();
            //this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");
            Page.ClientScript.RegisterStartupScript(GetType(), "PageLD1", "<script>Pageload();</script>");


        }
        public void BindEmployee()
        {

            int i = 0;
            string startdate = dtDate.Date.Month.ToString() + "/" + dtDate.Date.Day.ToString() + "/" + dtDate.Date.Year.ToString();
            string Enddate = dtToDate.Date.Month.ToString() + "/" + dtToDate.Date.Day.ToString() + "/" + dtToDate.Date.Year.ToString();
            // string wherecondition = " trn_company='" + HttpContext.Current.Session["LastCompany"].ToString() + "' and trn_FinancialYear='" + HttpContext.Current.Session["LastFinYear"].ToString() + "' and (CAST(trn_TransDate AS datetime) >= CONVERT(varchar,'" + startdate + "', 101)) and (CAST(trn_TransDate AS datetime) <= CONVERT(varchar,'" + Enddate + "', 101)) ";

            //  string whereCondGI = " GeneralInsurance_CompanyID='" + HttpContext.Current.Session["LastCompany"].ToString() + "' and GeneralInsurance_FinYear='" + HttpContext.Current.Session["LastFinYear"].ToString() + "' and cast(GeneralInsurance_TransactionDate as datetime) between '" + dtDate.Value + "' and '" + dtToDate.Value + "'";

            //string whereCondGI = " GeneralInsurance_CompanyID='" + HttpContext.Current.Session["LastCompany"].ToString() + "' and GeneralInsurance_FinYear='" + HttpContext.Current.Session["LastFinYear"].ToString() + "'  and   (CAST(GeneralInsurance_TransactionDate AS datetime) >= CONVERT(varchar,'" + startdate + "', 101)) and (CAST(GeneralInsurance_TransactionDate AS datetime) <= CONVERT(varchar,'" + Enddate + "', 101)) ";

            string wherecondition = "";
            if (rdbbAll.Checked != true)
            {
                if (Branch != "")
                {

                    if (i == 0)
                    {
                        i = i + 1;
                        wherecondition += " where t.branch_id  in  (" + Branch + ")  ";
                    }
                    else
                    {
                        wherecondition += " and t.branch_id  in  (" + Branch + ")  ";
                    }

                }
            }

            //if (rdbClientA.Checked != true)
            //{
            //    if (Clients != "")
            //    {
            //        wherecondition += " and  trn_ContactId in (" + Clients + ")";
            //        whereCondGI += " and GeneralInsurance_ContactID in (" + Clients + ")";
            //    }
            //}
            if (rdbInsuCompA.Checked != true)
            {
                if (InsuComp != "")
                {
                    if (i == 0)
                    {
                        i = i + 1;
                        wherecondition += " where   t.emp_organization  in(select cmp_id from tbl_master_company where cmp_internalid in (" + InsuComp + ")) ";
                    }
                    else
                    {
                        wherecondition += "  and t.emp_organization  in(select cmp_id from tbl_master_company where cmp_internalid in (" + InsuComp + ")) ";
                    }

                }
            }

            //if (rdbProductA.Checked != true)
            //{
            //    if (Products != "")
            //    {
            //        wherecondition += " and  trn_Scheme in (" + Products + ")";
            //        whereCondGI += " and  GeneralInsurance_ProductID in(" + Products + ")";
            //    }
            //}
            if (rdbTelecallerA.Checked != true)
            {
                if (ReportTo != "")
                {

                    if (i == 0)
                    {
                        i = i + 1;
                        wherecondition += " where   t.emp_Reportto  in (" + ReportTo + ") ";
                    }
                    else
                    {
                        wherecondition += "  and t.emp_Reportto  in (" + ReportTo + ") ";
                    }
                }
            }


            if (rdbSalesRepresentativeA.Checked != true)
            {

                if (!String.IsNullOrEmpty(SaleRep)) 
                {
                    //   wherecondition += " and  trn_Fos in (" + SaleRep + ")";
                    if (i == 0)
                    {
                        i = i + 1;
                        wherecondition += " where  t.ID  in (" + SaleRep + ")  ";
                    }
                    else
                    {
                        wherecondition += "and t.ID  in (" + SaleRep + ")  ";
                    }
                    // whereCondGI += " and  GeneralInsurance_SalesRep in(" + SaleRep + ")";
                }
            }

            //if (rdbAssociateA.Checked != true)
            //{
            //    if (Associate != "")
            //    {
            //        wherecondition += " and  trn_Referal in (" + Associate + ")";
            //        whereCondGI += " and  GeneralInsurance_Associates in(" + Associate + ")";
            //    }
            //}
            //if (rdbSubBroakerA.Checked != true)
            //{
            //    if (SubBroker != "")
            //    {
            //        wherecondition += " and  trn_SubBroker in (" + SubBroker + ")";
            //        whereCondGI += " and  GeneralInsurance_BrokerFranchise in(" + SubBroker + ")";
            //    }
            //}
            //if (drpPolicyStatus.SelectedValue != "A")
            //{
            //    wherecondition += " and  trn_Status in (" + drpPolicyStatus.SelectedValue + ")";
            //    whereCondGI += " and  GeneralInsurance_PolicyStatus in(" + drpPolicyStatus.SelectedValue + ")";
            //}



            String LIS = HttpContext.Current.Session["userbranchHierarchy"].ToString();

            EmployeeDataSource.SelectCommand = " select * from ( select ctc.emp_organization,ctc.emp_Reportto,tbl_master_branch.branch_id,tbl_master_contact.createdate, ISNULL(tbl_master_contact.cnt_firstName, '') + ' ' + ISNULL(tbl_master_contact.cnt_middleName, '') + ' ' + ISNULL(tbl_master_contact.cnt_lastName, '') AS Name," +
                                                 " tbl_master_contact.cnt_internalId AS Id,tbl_master_branch.branch_description AS BranchName,(ISNULL(Cont.cnt_firstName, '') + ' ' + ISNULL(Cont.cnt_middleName, '') + ' ' + ISNULL(Cont.cnt_lastName, '')+ ' [' + ISNULL(Cont.cnt_shortName, '')+']') as ReportTo,tbl_master_employee.emp_dateofjoining as DOJ, " +
                                                 " (Select (case when (Select top 1  '(M)'+ phf_phonenumber    from tbl_master_phonefax where phf_cntid=tbl_master_contact.cnt_internalId and phf_type = 'Mobile' ) is NULL then '' else ((Select top 1  '(M)'+ phf_phonenumber    from tbl_master_phonefax where phf_cntid=tbl_master_contact.cnt_internalId and phf_type = 'Mobile' ))  end) )as phone," +
                                                 " (select tbl_master_company.cmp_name from tbl_master_company where tbl_master_company.cmp_id=ctc.emp_organization ) as CompName," +
                                                 " tbl_master_user.user_name as user_name," +
                                                 " (select cost_description from tbl_master_costCenter where cost_id =ctc.emp_Department ) as cost_description ," +
                                                 " (select deg_designation from tbl_master_designation where deg_id =ctc.emp_designation ) as deg_designation," +
                                                 " tbl_master_contact.cnt_id,tbl_master_contact.cnt_shortName, " +
                                                 " (select top 1 crg_number from tbl_master_contactregistration where crg_type='Pancard' and crg_cntId=tbl_master_contact.cnt_InternalId) as PanCard " +
                                                 " from tbl_master_contact, tbl_master_branch, tbl_master_employee,tbl_master_user,tbl_master_salutation,tbl_trans_employeeCTC ctc ,tbl_master_contact Cont,   tbl_master_employee emp" +
                                                 " where tbl_master_contact.cnt_internalId=ctc.emp_cntId and (ctc.emp_effectiveuntil is null OR ctc.emp_effectiveuntil>getdate() OR ctc.emp_effectiveuntil='1/1/1900 12:00:00 AM')  and ctc.emp_Organization in(select cmp_id from tbl_master_company where cmp_internalid in (" + HttpContext.Current.Session["userCompanyHierarchy"] + ")) and " +
                                                 " tbl_master_contact.cnt_branchid  in (" + HttpContext.Current.Session["userbranchHierarchy"] + ") and tbl_master_contact.cnt_contactType='" + HttpContext.Current.Session["userContactType"] + "' and tbl_master_user.user_id = tbl_master_contact.createUser and tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id and tbl_master_contact.cnt_internalId = tbl_master_employee.emp_contactId  and tbl_master_salutation.sal_id = tbl_master_contact.cnt_salutation and (tbl_master_employee.emp_dateofLeaving is null or tbl_master_employee.emp_dateofLeaving='1/1/1900 12:00:00 AM') and ctc.emp_reportTo=emp.emp_id and emp.emp_contactID=cont.cnt_internalID  ) as t " + wherecondition + " order by t.createdate  desc";


            //EmployeeDataSource.SelectCommand = "select * from ( select  ISNULL(tbl_master_contact.cnt_firstName, '') + ' ' + ISNULL(tbl_master_contact.cnt_middleName, '') + ' ' + ISNULL(tbl_master_contact.cnt_lastName, '') AS Name," +
            //                                    " tbl_master_contact.cnt_internalId AS Id,tbl_master_branch.branch_description AS BranchName,(ISNULL(Cont.cnt_firstName, '') + ' ' + ISNULL(Cont.cnt_middleName, '') + ' ' + ISNULL(Cont.cnt_lastName, '')+ ' [' + ISNULL(Cont.cnt_shortName, '')+']') as ReportTo,tbl_master_employee.emp_dateofjoining as DOJ, " +
            //                                    " (Select (case when (Select top 1  '(M)'+ phf_phonenumber    from tbl_master_phonefax where phf_cntid=tbl_master_contact.cnt_internalId and phf_type = 'Mobile' ) is NULL then '' else ((Select top 1  '(M)'+ phf_phonenumber    from tbl_master_phonefax where phf_cntid=tbl_master_contact.cnt_internalId and phf_type = 'Mobile' ))  end) )as phone," +
            //                                    " (select tbl_master_company.cmp_name from tbl_master_company where tbl_master_company.cmp_id=ctc.emp_organization ) as CompName," +
            //                                    " tbl_master_user.user_name as user_name," +
            //                                    " (select cost_description from tbl_master_costCenter where cost_id =ctc.emp_Department ) as cost_description ," +
            //                                    " (select deg_designation from tbl_master_designation where deg_id =ctc.emp_designation ) as deg_designation," +
            //                                    " tbl_master_contact.cnt_id,tbl_master_contact.cnt_shortName, " +
            //                                    " (select top 1 crg_number from tbl_master_contactregistration where crg_type='Pancard' and crg_cntId=tbl_master_contact.cnt_InternalId) as PanCard " +
            //                                    " from tbl_master_contact, tbl_master_branch, tbl_master_employee,tbl_master_user,tbl_master_salutation,tbl_trans_employeeCTC ctc ,tbl_master_contact Cont,   tbl_master_employee emp" +
            //                                    " where tbl_master_contact.cnt_internalId=ctc.emp_cntId and (ctc.emp_effectiveuntil is null OR ctc.emp_effectiveuntil>getdate() OR ctc.emp_effectiveuntil='1/1/1900 12:00:00 AM')" +
            //                                    "  and tbl_master_contact.cnt_contactType='" + HttpContext.Current.Session["userContactType"] + "' and tbl_master_user.user_id = tbl_master_contact.createUser and tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id  and tbl_master_salutation.sal_id = tbl_master_contact.cnt_salutation and (tbl_master_employee.emp_dateofLeaving is null or tbl_master_employee.emp_dateofLeaving='1/1/1900 12:00:00 AM')  and emp.emp_contactID=cont.cnt_internalID and ctc.emp_reportTo =emp.emp_id ) as t " + wherecondition  + "";
            EmployeeGrid.DataBind();
            //this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");
            Page.ClientScript.RegisterStartupScript(GetType(), "PageLD1", "<script>Pageload();</script>");


        }
    }
}