using System;
using System.Data;
using System.Web.UI;
using System.Configuration;
using BusinessLogicLayer;
namespace ERP.OMS.Reports
{
    public partial class Reports_frm_LeadDetailsReport : System.Web.UI.Page
    {
        ExcelFile objExcel = new ExcelFile();
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        BusinessLogicLayer.Converter ObjConvert = new BusinessLogicLayer.Converter();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ASPxFromDate.EditFormatString = ObjConvert.GetDateFormat("Date");
                ASPxFromDate.Value = Convert.ToDateTime(oDBEngine.GetDate().ToString());
                ASPxToDate.EditFormatString = ObjConvert.GetDateFormat("Date");
                ASPxToDate.Value = Convert.ToDateTime(oDBEngine.GetDate().ToString());
            }
            Page.ClientScript.RegisterStartupScript(GetType(), "heighty", "<script language='JavaScript'>height();</script>");

        }
        protected void btnShow_Click(object sender, EventArgs e)
        {
            int flag = 0;
            string Format = "";
            DataTable Dt = oDBEngine.GetDataTable("tbl_master_lead as a,tbl_master_user as b,tbl_master_contact as c", " distinct a.cnt_internalid as LeadId,isnull(a.cnt_firstname,'')+' '+isnull(a.cnt_middlename,'')+' '+isnull(a.cnt_lastname,'') as LeadName,isnull((select top 1 isnull(phf_countrycode,'')+''+isnull(phf_areacode,'')+''+isnull(phf_phonenumber,'')as LeadPhoneNo from tbl_master_phonefax where phf_cntid=a.cnt_internalid and phf_type='Office' and (phf_phonenumber is not null or phf_phonenumber<>'') ),'')+' '+isnull((select top 1 isnull(phf_countrycode,'')+''+isnull(phf_areacode,'')+''+isnull(phf_phonenumber,'')as LeadPhoneNo from tbl_master_phonefax where phf_cntid=a.cnt_internalid and phf_type='Residence' and (phf_phonenumber is not null or phf_phonenumber<>'' )),'')+' '+isnull((select top 1 isnull(phf_phonenumber,'')as LeadPhoneNo from tbl_master_phonefax where phf_cntid=a.cnt_internalid and phf_type='Mobile' and (phf_phonenumber is not null or phf_phonenumber<>'' )),'')as LeadPhone,(select top 1 isnull(add_address1,'')+' '+isnull(add_address2,'')+' '+isnull(add_address3,'')+' '+(select city_name from tbl_master_city where city_id=tbl_master_address.add_city)+' '+(select state from tbl_master_state where id=tbl_master_address.add_state)+' '+(select cou_country from tbl_master_country where cou_id=tbl_master_address.add_country)+' Pin:'+add_pin from tbl_master_address where add_cntid=a.cnt_internalid)as LeadAddress,(select isnull(tbl_master_contact.cnt_firstname,'')+' '+isnull(tbl_master_contact.cnt_middlename,'')+' '+isnull(tbl_master_contact.cnt_lastname,'')+'('+cnt_shortname+')' from tbl_master_contact where cnt_internalid=b.user_contactid)as EmpName,(select branch_description from tbl_master_branch where branch_id=c.cnt_branchid)as Branch,convert(varchar(11),a.createdate,113)+ ' '+ LTRIM(SUBSTRING(CONVERT(VARCHAR(20), a.createdate, 22), 10, 5))+ RIGHT(CONVERT(VARCHAR(20),a.createdate, 22), 3) as CreateDate,b.user_contactid as EmpID,a.createuser,case a.cnt_Lead_Stage when 1 then 'Due' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status", "a.createuser=b.user_id and c.cnt_internalid=b.user_contactid and a.createdate>='" + ASPxFromDate.Value.ToString() + "' and a.createdate<='" + ASPxToDate.Value.ToString() + "'", "a.createuser");
            if (Dt.Rows.Count > 0)
            {
                MainContainer.Visible = true;
                string DiffUser = "";
                Format = "<table cellspacing=\"1\" cellpadding=\"1\"  class=\"TableMain100\" style=\"border:slid 1px blue\">";
                Format += "<script language=\"javascript\" type=\"text/javascript\">";
                Format += "function height(){if(document.body.scrollHeight>=350){window.frameElement.height = document.body.scrollHeight;}else {window.frameElement.height = '350px';} window.frameElement.width = document.body.scrollWidth;}";
                Format += "</script>";
                string DtTotal = "";
                DtTotal = oDBEngine.GetFieldValue("tbl_master_lead", "count(*)", "createdate>='" + ASPxFromDate.Value.ToString() + "' and createdate<='" + ASPxToDate.Value.ToString() + "' and createuser='" + Dt.Rows[0]["createuser"] + "'", 1)[0, 0];
                string DtDue = "";
                DtDue = oDBEngine.GetFieldValue("tbl_master_lead", "count(*)", "createdate>='" + ASPxFromDate.Value.ToString() + "' and createdate<='" + ASPxToDate.Value.ToString() + "' and createuser='" + Dt.Rows[0]["createuser"] + "' and cnt_Lead_Stage=1", 1)[0, 0];
                if (DtDue == "n")
                {
                    DtDue = "0";
                }
                string DtOpp = "";
                DtOpp = oDBEngine.GetFieldValue("tbl_master_lead", "count(*)", "createdate>='" + ASPxFromDate.Value.ToString() + "' and createdate<='" + ASPxToDate.Value.ToString() + "' and createuser='" + Dt.Rows[0]["createuser"] + "' and (cnt_lead_stage='2' or cnt_lead_stage='3' or cnt_lead_stage='4')", 1)[0, 0];
                if (DtOpp == "n")
                {
                    DtOpp = "0";
                }
                string dtLost = "";
                dtLost = oDBEngine.GetFieldValue("tbl_master_lead", "count(*)", "createdate>='" + ASPxFromDate.Value.ToString() + "' and createdate<='" + ASPxToDate.Value.ToString() + "' and createuser='" + Dt.Rows[0]["createuser"] + "' and cnt_Lead_Stage=5", 1)[0, 0];
                if (dtLost == "n")
                {
                    dtLost = "0";
                }

                Format += "<tr style=background:white;font-weight: bold><td style=font-weight:bold;color:blue>Employee Name: </td><td style=\"color:blue;font-weight:bold\">" + Dt.Rows[0]["EmpName"].ToString() + " </td><td style=font-weight:bold;color:blue> Branch: </td><td  style=\"color:blue;font-weight: bold\"> " + Dt.Rows[0]["Branch"].ToString() + "</td><td  style=\"color:blue;font-weight:bold\">Total:" + DtTotal.ToString() + "(Due:" + DtDue.ToString() + " Converted:" + DtOpp.ToString() + " Lost:" + dtLost + ")</td></tr>";
                Format += "<tr  class=EHEADER ><td style=font-weight:bold align=center>Lead Name</td><td style=font-weight:bold align=center>Address</td><td style=font-weight:bold align=center>Phone Number</td><td style=font-weight:bold;width:15% align=center>Lead Date</td><td style=font-weight:bold align=center>Status</td></tr>";
                for (int i = 0; i < Dt.Rows.Count; i++)
                {
                    flag = flag + 1;
                    DiffUser = Dt.Rows[i]["EmpName"].ToString();
                    Format += "<tr style=\"background-color: " + GetRowColor(flag) + " ;text-align:left\"><td style=\"padding-left: 5px\">" + Dt.Rows[i]["LeadName"].ToString() + "</td><td style=\"padding-left: 5px\">" + Dt.Rows[i]["LeadAddress"].ToString() + "</td><td style=\"padding-left: 5px\">" + Dt.Rows[i]["LeadPhone"].ToString() + "</td><td style=\"padding-left: 5px\">" + Dt.Rows[i]["CreateDate"].ToString() + "</td><td style=\"padding-left: 5px\">" + Dt.Rows[i]["Status"].ToString() + "</td></tr>";
                    int k = i + 1;
                    if (k != Dt.Rows.Count)
                    {
                        if (DiffUser != Dt.Rows[k]["EmpName"].ToString())
                        {
                            DtTotal = oDBEngine.GetFieldValue("tbl_master_lead", "count(*)", "createdate>='" + ASPxFromDate.Value.ToString() + "' and createdate<='" + ASPxToDate.Value.ToString() + "' and createuser='" + Dt.Rows[k]["createuser"] + "'", 1)[0, 0];
                            DtDue = oDBEngine.GetFieldValue("tbl_master_lead", "count(*)", "createdate>='" + ASPxFromDate.Value.ToString() + "' and createdate<='" + ASPxToDate.Value.ToString() + "' and createuser='" + Dt.Rows[k]["createuser"] + "' and cnt_Lead_Stage=1", 1)[0, 0];
                            if (DtDue == "n")
                            {
                                DtDue = "0";
                            }
                            DtOpp = oDBEngine.GetFieldValue("tbl_master_lead", "count(*)", "createdate>='" + ASPxFromDate.Value.ToString() + "' and createdate<='" + ASPxToDate.Value.ToString() + "' and createuser='" + Dt.Rows[k]["createuser"] + "' and (cnt_lead_stage='2' or cnt_lead_stage='3' or cnt_lead_stage='4')", 1)[0, 0];
                            if (DtOpp == "n")
                            {
                                DtOpp = "0";
                            }
                            dtLost = oDBEngine.GetFieldValue("tbl_master_lead", "count(*)", "createdate>='" + ASPxFromDate.Value.ToString() + "' and createdate<='" + ASPxToDate.Value.ToString() + "' and createuser='" + Dt.Rows[0]["createuser"] + "' and cnt_Lead_Stage=5", 1)[0, 0];
                            if (dtLost == "n")
                            {
                                dtLost = "0";
                            }
                            Format += "<tr style=background:white><td style=color:blue;font-weight:bold>Employee Name:</td><td style=\"color:blue;font-weight:bold\">" + Dt.Rows[k]["EmpName"].ToString() + " </td><td style=color:blue;font-weight:bold> Branch: </td><td style=\"color:blue;font-weight:bold\"> " + Dt.Rows[k]["Branch"].ToString() + "</td><td style=\"color:blue;font-weight:bold\">Total :" + DtTotal.ToString() + "(Due:" + DtDue.ToString() + " Converted:" + DtOpp.ToString() + ")</td></tr>";
                            Format += "<tr class=EHEADER ><td style=font-weight:bold>Lead Name</td><td style=font-weight:bold>Address</td><td style=font-weight:bold>Phone Number</td><td style=font-weight:bold;width:15% wrap=true>Lead Date</td><td style=font-weight:bold>Status</td></tr>";
                        }
                    }

                }
                Format += "<tr><td><script>height();</script></td></tr>";
                Format += "</table>";

                MainContainer.InnerHtml = Format;
                Page.ClientScript.RegisterStartupScript(GetType(), "heightc", "<script language='JavaScript'>height();</script>");
            }
            else
            {
                MainContainer.Visible = false;
                Page.ClientScript.RegisterStartupScript(GetType(), "Record", "<script language='JavaScript'>alert('No Record Found');</script>");
            }
        }
        protected string GetRowColor(int i)
        {
            //if (i++ % 2 == 0)
            //    return "#fff0f5";
            //else
            //    return "lavender";
            if (i++ % 2 == 0)
                return "#fff0f5";
            else
                return "White";
        }
        protected void ddlExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable DtForExportData = new DataTable();
            DataTable ForCreateUser = oDBEngine.GetDataTable("tbl_master_lead as a,tbl_master_user as b,tbl_master_contact as c", "distinct a.createuser", "a.createuser=b.user_id and c.cnt_internalid=b.user_contactid and a.createdate>='" + ASPxFromDate.Value.ToString() + "' and a.createdate<='" + ASPxToDate.Value.ToString() + "'", "a.createuser");
            for (int i = 0; i < ForCreateUser.Rows.Count; i++)
            {
                DataTable Dt = oDBEngine.GetDataTable("tbl_master_lead as a,tbl_master_user as b,tbl_master_contact as c", " distinct isnull(a.cnt_firstname,'')+' '+isnull(a.cnt_middlename,'')+' '+isnull(a.cnt_lastname,'') as LeadName,(select top 1 isnull(add_address1,'')+' '+isnull(add_address2,'')+' '+isnull(add_address3,'')+' '+(select city_name from tbl_master_city where city_id=tbl_master_address.add_city)+' '+(select state from tbl_master_state where id=tbl_master_address.add_state)+' '+(select cou_country from tbl_master_country where cou_id=tbl_master_address.add_country)+' Pin:'+add_pin from tbl_master_address where add_cntid=a.cnt_internalid)as LeadAddress,isnull((select top 1 isnull(phf_countrycode,'')+''+isnull(phf_areacode,'')+''+isnull(phf_phonenumber,'')as LeadPhoneNo from tbl_master_phonefax where phf_cntid=a.cnt_internalid and phf_type='Office' and (phf_phonenumber is not null or phf_phonenumber<>'') ),'')+' '+isnull((select top 1 isnull(phf_countrycode,'')+''+isnull(phf_areacode,'')+''+isnull(phf_phonenumber,'')as LeadPhoneNo from tbl_master_phonefax where phf_cntid=a.cnt_internalid and phf_type='Residence' and (phf_phonenumber is not null or phf_phonenumber<>'' )),'')+' '+isnull((select top 1 isnull(phf_phonenumber,'')as LeadPhoneNo from tbl_master_phonefax where phf_cntid=a.cnt_internalid and phf_type='Mobile' and (phf_phonenumber is not null or phf_phonenumber<>'' )),'')as LeadPhone,convert(varchar(11),a.createdate,113)+ ' '+ LTRIM(SUBSTRING(CONVERT(VARCHAR(20), a.createdate, 22), 10, 5))+ RIGHT(CONVERT(VARCHAR(20),a.createdate, 22), 3) as CreateDate,case a.cnt_Lead_Stage when 1 then 'Due' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status", "a.createuser=b.user_id and c.cnt_internalid=b.user_contactid and a.createdate>='" + ASPxFromDate.Value.ToString() + "' and a.createdate<='" + ASPxToDate.Value.ToString() + "' and a.createuser='" + ForCreateUser.Rows[i]["createuser"].ToString() + "'");
                DataTable Dtemp = oDBEngine.GetDataTable("tbl_master_lead as a,tbl_master_user as b,tbl_master_contact as c", " distinct (select isnull(tbl_master_contact.cnt_firstname,'')+' '+isnull(tbl_master_contact.cnt_middlename,'')+' '+isnull(tbl_master_contact.cnt_lastname,'')+'('+cnt_shortname+')' from tbl_master_contact where cnt_internalid=b.user_contactid)as EmpName,b.user_contactid as EmpID,(select branch_description from tbl_master_branch where branch_id=c.cnt_branchid)as Branch", "a.createuser=b.user_id and c.cnt_internalid=b.user_contactid and a.createdate>='" + ASPxFromDate.Value.ToString() + "' and a.createdate<='" + ASPxToDate.Value.ToString() + "' and a.createuser='" + ForCreateUser.Rows[i]["createuser"].ToString() + "'");


                if (i == 0)
                {
                    DtForExportData = Dt.Clone();
                    DtForExportData.Clear();
                }

                if (Dt.Rows.Count > 0)
                {
                    DataRow DrGroup = DtForExportData.NewRow();
                    DrGroup[0] = "Employee Name:    " + Dtemp.Rows[0]["EmpName"].ToString() + "      Branch:    " + Dtemp.Rows[0]["Branch"].ToString();
                    DrGroup[1] = "Test";
                    DtForExportData.Rows.Add(DrGroup);
                    for (int j = 0; j < Dt.Rows.Count; j++)
                    {
                        DataRow dr = DtForExportData.NewRow();
                        //dr["LeadId"] = Dt.Rows[j]["LeadId"];
                        dr["LeadName"] = Dt.Rows[j]["LeadName"];
                        dr["LeadPhone"] = Dt.Rows[j]["LeadPhone"];
                        dr["LeadAddress"] = Dt.Rows[j]["LeadAddress"];
                        //dr["EmpName"] = Dt.Rows[j]["EmpName"];
                        //dr["Branch"] = Dt.Rows[j]["Branch"];
                        dr["CreateDate"] = Dt.Rows[j]["CreateDate"];
                        //dr["EmpId"] = Dt.Rows[j]["EmpId"];
                        //dr["CreateUser"] = Dt.Rows[j]["CreateUser"];
                        dr["Status"] = Dt.Rows[j]["Status"];
                        DtForExportData.Rows.Add(dr);
                        //Add Row For The DataTable (FOr export)
                    }


                }
            }

            //DataTable CompanyName = oDBEngine.GetDataTable("tbl_master_company", "cmp_Name", " cmp_internalId='" + Session["LastCompany"].ToString() + "'");
            DataTable dtReportHeader = new DataTable();
            dtReportHeader.Columns.Add(new DataColumn("Header", typeof(String))); //0
            DataRow HeaderRow = dtReportHeader.NewRow();
            //HeaderRow[0] = CompanyName.Rows[0][0].ToString();
            //dtReportHeader.Rows.Add(HeaderRow);
            DataRow DrRowR1 = dtReportHeader.NewRow();
            DrRowR1[0] = "Lead Generation Report [From:  " + ObjConvert.ArrangeDate2(ASPxFromDate.Value.ToString()) + "  To:  " + ObjConvert.ArrangeDate2(ASPxToDate.Value.ToString()) + " ]";
            dtReportHeader.Rows.Add(DrRowR1);
            DataRow HeaderRow1 = dtReportHeader.NewRow();
            dtReportHeader.Rows.Add(HeaderRow1);
            DataRow HeaderRow2 = dtReportHeader.NewRow();
            dtReportHeader.Rows.Add(HeaderRow2);
            DataTable dtReportFooter = new DataTable();
            dtReportFooter.Columns.Add(new DataColumn("Footer", typeof(String))); //0
            DataRow FooterRow1 = dtReportFooter.NewRow();
            dtReportFooter.Rows.Add(FooterRow1);
            DataRow FooterRow2 = dtReportFooter.NewRow();
            dtReportFooter.Rows.Add(FooterRow2);
            DataRow FooterRow = dtReportFooter.NewRow();
            //FooterRow[0] = "* * *  End Of Report * * *         [" + oconverter.ArrangeDate2(oDBEngine.GetDate().ToString(), "Test") + "]";
            FooterRow[0] = "* * *  End Of Report * * *   ";
            dtReportFooter.Rows.Add(FooterRow);
            //DataRow EmpDetails = dtReportHeader.NewRow();
            //EmpDetails[0] = DtForExportData.Rows[1]["EmpName"].ToString();
            //EmpDetails[1] = "Test";
            //DtForExportData.Rows.Add(EmpDetails);

            if (ddlExport.SelectedItem.Value == "E")
            {
                //oconverter.ExcelImport(dtBilling, "Daily Billing");
                objExcel.ExportToExcelforExcel(DtForExportData, "LeadDetails", "Total", dtReportHeader, dtReportFooter);

            }
            else if (ddlExport.SelectedItem.Value == "P")
            {
                objExcel.ExportToPDF(DtForExportData, "LeadDetails", "Total", dtReportHeader, dtReportFooter);
            }
        }
    }
}