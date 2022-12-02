using System;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrystalDecisions.CrystalReports.Engine;
using BusinessLogicLayer;

namespace ERP.OMS.Reports
{
    public partial class Reports_Report_lead : System.Web.UI.Page, System.Web.UI.ICallbackEventHandler
    {
        ClsDropDownlistNameSpace.clsDropDownList clsdrp = new ClsDropDownlistNameSpace.clsDropDownList();
        string data = "";
        BusinessLogicLayer.Converter OConvert = new BusinessLogicLayer.Converter();
        public string pageAccess = "";
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
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
            if (HttpContext.Current.Session["userid"] == null)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }


            if (!IsPostBack)
            {
                ComboBind();
                txtFromDate.EditFormatString = OConvert.GetDateFormat("Date");
                txtToDate.EditFormatString = OConvert.GetDateFormat("Date");
                txtFromDate.Value = oDBEngine.GetDate();
                txtToDate.Value = oDBEngine.GetDate();

                txtName.Attributes.Add("onkeyup", "CallList(this,'referedby',event)");

                cmbSourceType.Attributes.Add("onchange", "SourceTypeChange(ctl00_ContentPlaceHolder3_cmbSourceType.value)");
                Session["mode"] = "off";
                //________This script is for firing javascript when page load first___//
                if (!ClientScript.IsStartupScriptRegistered("Today"))
                    ClientScript.RegisterStartupScript(typeof(Page), "Today", "<script>AtTheTimePageLoad();</script>");
                //______________________________End Script____________________________//
            }

            //_____For performing operation without refreshing page___//
            String cbReference = Page.ClientScript.GetCallbackEventReference(this, "arg", "ReceiveServerData", "context");
            String callbackScript = "function CallServer(arg, context){ " + cbReference + ";}";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "CallServer", callbackScript, true);
            //___________-end here___//
            if (Session["mode"] == "on")
            {
                GetReport();
                //________This script is for firing javascript when page load first___//
                if (!ClientScript.IsStartupScriptRegistered("Today"))
                    ClientScript.RegisterStartupScript(typeof(Page), "Today", "<script>atpagecgange();</script>");
                //______________________________End Script____________________________//
            }

            //_________-This is to hide txtname_hidden field//
            if (!ClientScript.IsStartupScriptRegistered("Today"))
                ClientScript.RegisterStartupScript(typeof(Page), "Today", "<script>hidetextbox();</script>");
            //this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");
        }

        private void GetReport()
        {
            Session["mode"] = "on";
            //  Converter oConverter = new Converter();
            string start = txtFromDate.Value.ToString();
            string end = txtToDate.Value.ToString();
            //string start = oConverter.DateConverter(txtFromDate.Text, "mm/dd/yyyy");
            //string end = oConverter.DateConverter(txtToDate.Text, "mm/dd/yyyy");
            //DBEngine oDBEngine = new DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);
            string whereCondition = "";
            int minage = int.Parse(cmbStarYear.SelectedValue.ToString());
            string maxage = cmbEndYear.SelectedValue.ToString();
            DataTable DT = new DataTable();

            if (maxage != "All")
                whereCondition = " (DATEDIFF(YY, lead.cnt_dOB, getdate()) - CASE WHEN( (MONTH(lead.cnt_dOB)*100 + DAY(lead.cnt_dOB)) > (MONTH(getdate())*100 + DAY(getdate())))  then 1 else 0 end) < " + maxage + " and (DATEDIFF(YY, lead.cnt_dOB, getdate()) - CASE WHEN( (MONTH(lead.cnt_dOB)*100 + DAY(lead.cnt_dOB)) > (MONTH(getdate())*100 + DAY(getdate())))  then 1 else 0 end)>=" + minage;
            if (cmbProfession.SelectedValue != "All")
            {
                if (whereCondition != "")
                    whereCondition += " and " + " lead.cnt_profession=" + cmbProfession.SelectedValue;
                else
                    whereCondition = " lead.cnt_profession=" + cmbProfession.SelectedValue;
            }
            if (cmbMaritalStatus.SelectedValue != "All")
            {
                if (whereCondition != "")
                    whereCondition += " and " + " lead.cnt_maritalStatus=" + cmbMaritalStatus.SelectedValue;
                else
                    whereCondition = " lead.cnt_maritalStatus=" + cmbMaritalStatus.SelectedValue;
            }
            if (cmbLegalStatus.SelectedValue != "All")
            {
                if (whereCondition != "")
                    whereCondition += " and " + " lead.cnt_legalStatus=" + cmbLegalStatus.SelectedValue;
                else
                    whereCondition = " lead.cnt_legalStatus=" + cmbLegalStatus.SelectedValue;
            }
            if (cmbSourceType.SelectedValue != "All")
            {
                if (txtName.Text != "No Record Found" && txtName.Text != "")
                {
                    string[] ID = txtName.Text.Split('!');
                    if (ID.Length > 1)
                    {
                        if (whereCondition != "")
                            whereCondition += " and " + " lead.cnt_referedBy='" + ID[1] + "'";
                        else
                            whereCondition = " lead.cnt_referedBy ='" + ID[1] + "'";
                    }
                }
            }
            string conditionforSate = "";
            if (cmbState.SelectedValue != "All")
            {
                if (conditionforSate != "")
                    conditionforSate = " add_state =" + cmbState.SelectedValue;
                else
                    conditionforSate = " add_state =" + cmbState.SelectedValue;
                if (cmbCity.SelectedValue != "All" && cmbCity.SelectedValue != "")
                {
                    conditionforSate += " and " + " add_city =" + cmbCity.SelectedValue;
                }
                if (cmbArea.SelectedValue != "All" && cmbArea.SelectedValue != "")
                {
                    conditionforSate += " and " + " add_area =" + cmbArea.SelectedValue;
                }
            }

            //if (rdbReport.SelectedValue != "Custom")
            //{
            if (whereCondition != "")
            {
                if (conditionforSate != "")
                    DT = oDBEngine.GetDataTable(" tbl_master_lead lead,tbl_master_contact contact ", " convert(varchar(100),lead.cnt_firstName + ' '+ lead.cnt_middleName+' '+lead.cnt_lastName) as col1,(select pro_professionName from tbl_master_profession where pro_id=lead.cnt_profession) as col2,(Select mts_maritalStatus from tbl_master_maritalstatus where mts_id = lead.cnt_maritalStatus ) as col5 , convert(varchar(100),contact.cnt_firstName+' '+contact.cnt_middleName+' '+contact.cnt_lastName) as col6, DATEDIFF(YY, lead.cnt_dOB, getdate()) - CASE WHEN( (MONTH(lead.cnt_dOB)*100 + DAY(lead.cnt_dOB)) > (MONTH(getdate())*100 + DAY(getdate())))  then 1 else 0 end  as col3, (select lgl_legalStatus from tbl_master_legalStatus where lgl_id=lead.cnt_legalStatus) as col4, convert(varchar(100),lead.cnt_referedby) as grp1 ,convert(varchar(100),(Select city_name from tbl_master_city where city_id in ( select top 1 add_city from tbl_master_address where add_cntId= lead.cnt_internalId and add_addressType is not null and add_addressType != 'N/A' and add_addressType = 'Residence' and " + conditionforSate + " order by createDate desc))) as grp2 ,convert(varchar(100),(Select area_name from tbl_master_area where area_id in ( select top 1 add_area from tbl_master_address where add_cntId= lead.cnt_internalId and add_addressType is not null and add_addressType != 'N/A' and add_addressType = 'Residence' and " + conditionforSate + "  ))) as grp3 ", " contact.cnt_internalId = lead.cnt_referedBy and lead.CreateDate is not NULL and lead.CreateDate between '" + start + "' and '" + end + "' and " + whereCondition, " lead.CreateUser ");
                else
                    DT = oDBEngine.GetDataTable(" tbl_master_lead lead,tbl_master_contact contact ", " convert(varchar(100),lead.cnt_firstName + ' '+ lead.cnt_middleName+' '+lead.cnt_lastName) as col1,(select pro_professionName from tbl_master_profession where pro_id=lead.cnt_profession) as col2,(Select mts_maritalStatus from tbl_master_maritalstatus where mts_id = lead.cnt_maritalStatus ) as col5 , convert(varchar(100),contact.cnt_firstName+' '+contact.cnt_middleName+' '+contact.cnt_lastName) as col6, DATEDIFF(YY, lead.cnt_dOB, getdate()) - CASE WHEN( (MONTH(lead.cnt_dOB)*100 + DAY(lead.cnt_dOB)) > (MONTH(getdate())*100 + DAY(getdate())))  then 1 else 0 end  as col3, (select lgl_legalStatus from tbl_master_legalStatus where lgl_id=lead.cnt_legalStatus) as col4, convert(varchar(100),lead.cnt_referedby) as grp1 ,convert(varchar(100),(Select city_name from tbl_master_city where city_id in ( select top 1 add_city from tbl_master_address where add_cntId= lead.cnt_internalId and add_addressType is not null and add_addressType != 'N/A' and add_addressType = 'Residence' order by createDate desc))) as grp2 ,convert(varchar(100),(Select area_name from tbl_master_area where area_id in ( select top 1 add_area from tbl_master_address where add_cntId= lead.cnt_internalId and add_addressType is not null and add_addressType != 'N/A' and add_addressType = 'Residence'  ))) as grp3 ", " contact.cnt_internalId = lead.cnt_referedBy and lead.CreateDate is not NULL and lead.CreateDate between '" + start + "' and '" + end + "' and " + whereCondition, " lead.CreateUser ");
            }
            else
                DT = oDBEngine.GetDataTable(" tbl_master_lead lead,tbl_master_contact contact ", " convert(varchar(100),lead.cnt_firstName + ' '+ lead.cnt_middleName+' '+lead.cnt_lastName) as col1,(select pro_professionName from tbl_master_profession where pro_id=lead.cnt_profession) as col2,(Select mts_maritalStatus from tbl_master_maritalstatus where mts_id = lead.cnt_maritalStatus ) as col5 , convert(varchar(100),contact.cnt_firstName+' '+contact.cnt_middleName+' '+contact.cnt_lastName) as col6, DATEDIFF(YY, lead.cnt_dOB, getdate()) - CASE WHEN( (MONTH(lead.cnt_dOB)*100 + DAY(lead.cnt_dOB)) > (MONTH(getdate())*100 + DAY(getdate())))  then 1 else 0 end  as col3, (select lgl_legalStatus from tbl_master_legalStatus where lgl_id=lead.cnt_legalStatus) as col4, convert(varchar(100),lead.cnt_referedby) as grp1 ,convert(varchar(100),(Select city_name from tbl_master_city where city_id in ( select top 1 add_city from tbl_master_address where add_cntId= lead.cnt_internalId and add_addressType is not null and add_addressType != 'N/A' and add_addressType = 'Residence' order by createDate desc))) as grp2 ,convert(varchar(100),(Select area_name from tbl_master_area where area_id in ( select top 1 add_area from tbl_master_address where add_cntId= lead.cnt_internalId and add_addressType is not null and add_addressType != 'N/A' and add_addressType = 'Residence'  ))) as grp3 ", " contact.cnt_internalId = lead.cnt_referedBy and lead.CreateDate is not NULL and lead.CreateDate between '" + start + "' and '" + end + "'", " lead.CreateUser ");
            //}
            //else
            //{
            //    if (whereCondition != "")
            //    {
            //        if (conditionforSate != "")
            //            DT = oDBEngine.GetDataTable(" tbl_master_lead lead,tbl_master_contact contact ", " convert(varchar(100),'') as col1,convert(varchar(100),'') as col2,convert(varchar(100),'') as col5 , convert(varchar(100),contact.cnt_firstName+' '+contact.cnt_middleName+' '+contact.cnt_lastName) as col6, convert(varchar(100),'')  as col3, convert(varchar(100),'') as col4, convert(varchar(100),lead.cnt_referedby) as grp1 ,convert(varchar(100),(Select city_name from tbl_master_city where city_id in ( select top 1 add_city from tbl_master_address where add_cntId= lead.cnt_internalId and add_addressType is not null and add_addressType != 'N/A' and add_addressType = 'Residence' order by createDate desc))) as grp2 ,convert(varchar(100),(Select area_name from tbl_master_area where area_id in ( select top 1 add_area from tbl_master_address where add_cntId= lead.cnt_internalId and add_addressType is not null and add_addressType != 'N/A' and add_addressType = 'Residence'  ))) as grp3 ", " contact.cnt_internalId = lead.cnt_referedBy and lead.CreateDate is not NULL and lead.CreateDate between '" + start + "' and '" + end + "' and " + whereCondition, " lead.CreateUser ");
            //        else
            //            DT = oDBEngine.GetDataTable(" tbl_master_lead lead,tbl_master_contact contact ", " convert(varchar(100),'') as col1,convert(varchar(100),'') as col2,convert(varchar(100),'') as col5 , convert(varchar(100),contact.cnt_firstName+' '+contact.cnt_middleName+' '+contact.cnt_lastName) as col6, convert(varchar(100),'')  as col3, convert(varchar(100),'') as col4, convert(varchar(100),lead.cnt_referedby) as grp1 ,convert(varchar(100),(Select city_name from tbl_master_city where city_id in ( select top 1 add_city from tbl_master_address where add_cntId= lead.cnt_internalId and add_addressType is not null and add_addressType != 'N/A' and add_addressType = 'Residence' order by createDate desc))) as grp2 ,convert(varchar(100),(Select area_name from tbl_master_area where area_id in ( select top 1 add_area from tbl_master_address where add_cntId= lead.cnt_internalId and add_addressType is not null and add_addressType != 'N/A' and add_addressType = 'Residence'  ))) as grp3 ", " contact.cnt_internalId = lead.cnt_referedBy and lead.CreateDate is not NULL and lead.CreateDate between '" + start + "' and '" + end + "' and " + whereCondition, " lead.CreateUser ");
            //    }
            //    else
            //        DT = oDBEngine.GetDataTable(" tbl_master_lead lead,tbl_master_contact contact ", " convert(varchar(100),'') as col1,convert(varchar(100),'') as col2,convert(varchar(100),'') as col5 , convert(varchar(100),contact.cnt_firstName+' '+contact.cnt_middleName+' '+contact.cnt_lastName) as col6, convert(varchar(100),'')  as col3, convert(varchar(100),'') as col4, convert(varchar(100),lead.cnt_referedby) as grp1 ,convert(varchar(100),(Select city_name from tbl_master_city where city_id in ( select top 1 add_city from tbl_master_address where add_cntId= lead.cnt_internalId and add_addressType is not null and add_addressType != 'N/A' and add_addressType = 'Residence' order by createDate desc))) as grp2 ,convert(varchar(100),(Select area_name from tbl_master_area where area_id in ( select top 1 add_area from tbl_master_address where add_cntId= lead.cnt_internalId and add_addressType is not null and add_addressType != 'N/A' and add_addressType = 'Residence'  ))) as grp3 ", " contact.cnt_internalId = lead.cnt_referedBy and lead.CreateDate is not NULL and lead.CreateDate between '" + start + "' and '" + end + "' ", " lead.CreateUser ");
            //}
            ReportDocument LeadReportDocu = new ReportDocument();
            string path = Server.MapPath("..\\Reports\\CrystalReport6.rpt");
            LeadReportDocu.Load(path);

            //_________Seting Look____//

            FieldObject Col1;
            Col1 = (FieldObject)LeadReportDocu.ReportDefinition.ReportObjects["Col11"];
            //Col1.Left=0;//__this was for positioning
            Col1.ObjectFormat.HorizontalAlignment = CrystalDecisions.Shared.Alignment.LeftAlign;
            FieldObject Col2 = (FieldObject)LeadReportDocu.ReportDefinition.ReportObjects["Col21"];
            Col2.ObjectFormat.HorizontalAlignment = CrystalDecisions.Shared.Alignment.LeftAlign;
            FieldObject Col3 = (FieldObject)LeadReportDocu.ReportDefinition.ReportObjects["Col31"];
            Col3.ObjectFormat.HorizontalAlignment = CrystalDecisions.Shared.Alignment.LeftAlign;
            FieldObject Col4 = (FieldObject)LeadReportDocu.ReportDefinition.ReportObjects["Col41"];
            Col4.ObjectFormat.HorizontalAlignment = CrystalDecisions.Shared.Alignment.LeftAlign;
            FieldObject Col5 = (FieldObject)LeadReportDocu.ReportDefinition.ReportObjects["Col51"];
            Col5.ObjectFormat.HorizontalAlignment = CrystalDecisions.Shared.Alignment.LeftAlign;
            FieldObject Col6 = (FieldObject)LeadReportDocu.ReportDefinition.ReportObjects["Col61"];
            Col6.ObjectFormat.HorizontalAlignment = CrystalDecisions.Shared.Alignment.LeftAlign;
            FieldObject grp2 = (FieldObject)LeadReportDocu.ReportDefinition.ReportObjects["GroupNamegrp21"];
            grp2.ObjectFormat.HorizontalAlignment = CrystalDecisions.Shared.Alignment.LeftAlign;
            FieldObject grp3 = (FieldObject)LeadReportDocu.ReportDefinition.ReportObjects["GroupNamegrp31"];
            grp3.ObjectFormat.HorizontalAlignment = CrystalDecisions.Shared.Alignment.LeftAlign;
            if (rdbReport.SelectedValue == "Custom")
            {
                Section body = (Section)LeadReportDocu.ReportDefinition.Sections[5];
                body.SectionFormat.EnableSuppress = true;
            }
            else
            {
                Section body = (Section)LeadReportDocu.ReportDefinition.Sections[5];
                body.SectionFormat.EnableSuppress = false;
            }

            LeadReportDocu.SetDataSource(DT);
            //CrystalReportViewer1.ReportSource = LeadReportDocu;
            //CrystalReportViewer1.DataBind();
        }

        private void ComboBind()
        {
            // DBEngine oDBEngine = new DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);
            string[,] Data = oDBEngine.GetFieldValue("tbl_master_profession", "pro_id, pro_professionName", null, 2, "pro_professionName");
            clsdrp.AddDataToDropDownList(Data, cmbProfession, "All");
            Data = oDBEngine.GetFieldValue(" tbl_master_maritalstatus", " mts_id, mts_maritalStatus", null, 2, "mts_maritalStatus");
            clsdrp.AddDataToDropDownList(Data, cmbMaritalStatus, "All");
            Data = oDBEngine.GetFieldValue("tbl_master_legalstatus", "lgl_id, lgl_legalStatus", null, 2, "lgl_legalStatus");
            clsdrp.AddDataToDropDownList(Data, cmbLegalStatus, "All");
            Data = oDBEngine.GetFieldValue(" tbl_master_ContactSource", "cntsrc_id, cntsrc_sourcetype", null, 2, "cntsrc_sourcetype");
            clsdrp.AddDataToDropDownList(Data, cmbSourceType, "All");
            Data = oDBEngine.GetFieldValue(" tbl_master_state ", "id, state", null, 2, " state ");
            clsdrp.AddDataToDropDownList(Data, cmbState, "All");
            int j = 0;
            for (int i = 18; i < 100; i++)
            {
                cmbStarYear.Items.Add(new ListItem(i.ToString(), i.ToString()));
                j++;
            }
            cmbEndYear.Items.Add(new ListItem("All", "All"));
            for (int i = 19; i < 100; i++)
            {
                cmbEndYear.Items.Add(new ListItem(i.ToString(), i.ToString()));
                j++;
            }
        }

        string ICallbackEventHandler.GetCallbackResult()
        {
            return data;
        }

        void ICallbackEventHandler.RaiseCallbackEvent(string eventArgument)
        {
            //DBEngine oDBEngine = new DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);
            string id = eventArgument.ToString();
            string[] FieldWvalue = id.Split('~');
            data = "";
            #region City name to combo city
            if (FieldWvalue[0] == "City")
            {
                string[,] listdata = oDBEngine.GetFieldValue(" tbl_master_city ", " city_id,city_name ", " state_id =" + FieldWvalue[1], 2, " city_name  ");
                if (listdata[0, 0] != "n")
                {
                    string items = "";
                    for (int i = 0; i < listdata.Length / 2; i++)
                    {
                        if (items == "")
                            items = listdata[i, 0] + "," + listdata[i, 1];
                        else
                            items += items + "!" + listdata[i, 0] + "," + listdata[i, 1];
                    }
                    data = "City~Y~" + items;
                }
                else
                    data = "City~N";
            }
            #endregion
            #region Area name to combo area
            if (FieldWvalue[0] == "Area")
            {
                string[,] listdata = oDBEngine.GetFieldValue(" tbl_master_area ", " area_id,area_name ", " city_id =" + FieldWvalue[1], 2, " area_name  ");
                if (listdata[0, 0] != "n")
                {
                    string items = "";
                    for (int i = 0; i < listdata.Length / 2; i++)
                    {
                        if (items == "")
                            items = listdata[i, 0] + "," + listdata[i, 1];
                        else
                            items += items + "!" + listdata[i, 0] + "," + listdata[i, 1];
                    }
                    data = "Area~Y~" + items;
                }
                else
                    data = "Area~N";
            }

            #endregion



        }
        protected void btnGetReport_Click(object sender, EventArgs e)
        {
            GetReport();
        }
        protected void cmbState_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbState.SelectedValue != "All")
            {
                //DBEngine oDBEngine = new DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);
                string[,] listdata = oDBEngine.GetFieldValue(" tbl_master_city ", " city_id,city_name ", " state_id =" + cmbState.SelectedValue, 2, " city_name  ");
                if (listdata[0, 0] != "n")
                {
                    clsdrp.AddDataToDropDownList(listdata, cmbCity, "All");
                }
                else
                    cmbCity.Items.Clear();
            }
            else
                cmbCity.Items.Clear();
        }
        protected void cmbCity_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbArea.SelectedValue != "All")
            {
                // DBEngine oDBEngine = new DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);
                string[,] listdata = oDBEngine.GetFieldValue(" tbl_master_area ", " area_id,area_name ", " city_id =" + cmbCity.SelectedValue, 2, " area_name  ");
                if (listdata[0, 0] != "n")
                {
                    clsdrp.AddDataToDropDownList(listdata, cmbArea, "All");
                }
                else
                    cmbArea.Items.Clear();
            }
            else
                cmbArea.Items.Clear();
        }
    }
}