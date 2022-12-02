using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrystalDecisions.CrystalReports.Engine;
using DevExpress.Web;

namespace ERP.OMS.Reports
{
    public partial class Reports_frmReport_CDSLClientMaster : System.Web.UI.Page, System.Web.UI.ICallbackEventHandler
    {
        string ClientType = null;
        string ClientSubType = null;
        string Client = null;
        string data;
        int p;
        BusinessLogicLayer.Converter oconverter = new BusinessLogicLayer.Converter();
        BusinessLogicLayer.Reports oReports = new BusinessLogicLayer.Reports();
        ExcelFile objExcel = new ExcelFile();
        BusinessLogicLayer.DBEngine oDbEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //'http://localhost:2957/InfluxCRM/management/testProjectMainPage_employee.aspx'
                string sPath = HttpContext.Current.Request.Url.ToString();
                oDbEngine.Call_CheckPageaccessebility(sPath);
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.ClientScript.RegisterStartupScript(GetType(), "PageLoad", "<script>Page_Load();</script>");

            if (HttpContext.Current.Session["userid"] == null)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");

            }
            if (!IsPostBack)
            {


                string[] FinYear = Session["LastFinYear"].ToString().Split('-');
                dtFrom.EditFormatString = oconverter.GetDateFormat("Date");
                dtTo.EditFormatString = oconverter.GetDateFormat("Date");
                dtFrom.Value = Convert.ToDateTime(Session["FinYearStart"].ToString());
                dtTo.Value = Convert.ToDateTime(Session["FinYearEnd"].ToString());

                BindDropDownp();
            }
            else
            {
                //p = 0;
                // BindGrid();
                BindData();
            }
            Page.ClientScript.RegisterStartupScript(GetType(), "Hiight", "<script>height();</script>");
            //_____For performing operation without refreshing page___//
            String cbReference = Page.ClientScript.GetCallbackEventReference(this, "arg", "ReceiveServerData", "context");
            String callbackScript = "function CallServer(arg, context){ " + cbReference + ";}";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "CallServer", callbackScript, true);
            //this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");
            //___________-end here___//



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
                string[] val = cl[i].Split(';');
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
            if (idlist[0] == "Type")
            {
                ClientType = str;
                data = "Type~" + str;
            }
            if (idlist[0] == "SubType")
            {
                ClientSubType = str;
                data = "SubType~" + str;

            }
            if (idlist[0] == "Client")
            {
                Client = str;
                data = "Client~" + str;

            }

        }
        string ICallbackEventHandler.GetCallbackResult()
        {
            return data;
        }
        public void BindDropDownp()
        {

            DataTable DtCat = oDbEngine.GetDataTable("master_cdslclients", "distinct CdslClients_AccountCategory ", " CdslClients_AccountCategory is not null ");
            if (DtCat.Rows.Count > 0)
            {
                ddlCat.DataSource = DtCat;
                ddlCat.DataTextField = "CdslClients_AccountCategory";
                ddlCat.DataValueField = "CdslClients_AccountCategory";
                ddlCat.DataBind();
                ddlCat.Items.Insert(0, new ListItem("--ALL--", "0"));
                DtCat.Dispose();

            }
            else
            {
                ddlCat.Items.Insert(0, new ListItem("Select Beneficiary A/C Category", "0"));
            }

            DataTable DtGroup = oDbEngine.GetDataTable("tbl_master_groupmaster", "distinct gpm_Type", null);
            if (DtGroup.Rows.Count > 0)
            {
                drpGroupType.DataSource = DtGroup;
                drpGroupType.DataTextField = "gpm_Type";
                drpGroupType.DataValueField = "gpm_Type";
                drpGroupType.DataBind();
                // ddlgrouptype.Items.Insert(0, new ListItem("Select GroupType", "0"));
                DtGroup.Dispose();

            }


            //DataTable DtOcp = oDbEngine.GetDataTable("master_nsdlstaticdatacode", "nsdlstaticdata_TypeCode,nsdlstaticdata_Description ", "nsdlstaticdata_FieldName like 'Beneficiary Occupation%'");
            //if (DtOcp.Rows.Count > 0)
            //{
            //    ddlOcp.DataSource = DtOcp;
            //    ddlOcp.DataTextField = "nsdlstaticdata_Description";
            //    ddlOcp.DataValueField = "nsdlstaticdata_TypeCode";
            //    ddlOcp.DataBind();
            //    ddlOcp.Items.Insert(0, new ListItem("--ALL--", "0"));
            //    DtOcp.Dispose();

            //}
            //else
            //{
            //    ddlOcp.Items.Insert(0, new ListItem("Select Beneficiary Occupation", "0"));
            //}

        }
        protected void btnShow_Click(object sender, EventArgs e)
        {

            if (RadAll.Checked == true)
            {
                if (RadDateRangeA.Checked == true)
                {
                    lblReportHeader.Text = "CDSL Client Master For Active and Closed Account ";
                }
                else
                {
                    lblReportHeader.Text = "CDSL Client Master For Account Activation Date  From [" + oconverter.ArrangeDate2(dtFrom.Value.ToString()) + "] To   [" + oconverter.ArrangeDate2(dtTo.Value.ToString()) + "] ";
                }

            }
            else if (RadActive.Checked == true)
            {
                if (RadDateRangeA.Checked == true)
                {
                    lblReportHeader.Text = "CDSL Client Master For Active Account ";
                }
                else
                {
                    lblReportHeader.Text = "CDSL Client Master For Account Activation Date  From  [" + oconverter.ArrangeDate2(dtFrom.Value.ToString()) + "] To   [" + oconverter.ArrangeDate2(dtTo.Value.ToString()) + "] ";
                }
            }
            else if (RadClosed.Checked == true)
            {
                if (RadDateRangeA.Checked == true)
                {
                    lblReportHeader.Text = "NSDL Client Master For Closed Account  ";
                }
                else
                {
                    lblReportHeader.Text = "CDSL Client Master For Account Closure Date  From  [" + oconverter.ArrangeDate2(dtFrom.Value.ToString()) + "] To   [" + oconverter.ArrangeDate2(dtTo.Value.ToString()) + "] ";
                }
            }

            BindGrid();


        }

        protected void BindGrid()
        {
            string AccountStatus = "";
            DateTime FromDate;
            DateTime ToDate;
            string AccType = "";
            string AccSubType = "";
            string BenCategory = "";
            string BenOCP = "";
            string ShowNHolder = "";
            string ShowNPOA = "";
            string ShowAcMinor = "";
            string ShowAcNNom = "";
            string ShowAcMNom = "";
            string DateRangeSelection = "";
            string SelectedClient = "";
            string ShowGroup = "";
            if (RadClientSelected.Checked == true)
            {
                SelectedClient = HDNClient.Value;
            }

            if (RadDateRangeS.Checked == true)
            {
                DateRangeSelection = "Y";
            }

            if (RadAll.Checked == true)
            {
                AccountStatus = "A";
            }
            else if (RadActive.Checked == true)
            {
                AccountStatus = "B";
            }
            else if (RadClosed.Checked == true)
            {
                AccountStatus = "C";
            }


            if (rdbMainSelected.Checked == true)
                AccType = HDNType.Value;
            if (rdSelSegment.Checked == true)
                AccSubType = HDNSubType.Value;
            if (ddlCat.SelectedItem.Value != "0")
                BenCategory = ddlCat.SelectedItem.Value;
            if (ddlOcp.SelectedItem.Value != "0")
                BenOCP = ddlOcp.SelectedItem.Value;
            if (ChkAHolder.Checked == true)
                ShowNHolder = "Y";
            if (ChkPOA.Checked == true)
                ShowNPOA = "Y";
            if (chkMinor.Checked == true)
                ShowAcMinor = "Y";
            if (chkNom.Checked == true)
                ShowAcNNom = "Y";
            if (chkMinorNom.Checked == true)
                ShowAcMNom = "Y";

            if (chkGroup.Checked == true)
                ShowGroup = "y";
            else
                ShowGroup = "n";
            DataSet ds = new DataSet();
            ds = oReports.Fetch_CDSLClientMaster(
                SelectedClient,
                DateRangeSelection,
                AccountStatus,
                 Convert.ToString(dtFrom.Value),
                 Convert.ToString(dtTo.Value),
                AccType,
                AccSubType,
                BenCategory,
                BenOCP,
                ShowNHolder,
                ShowNPOA,
                ShowAcMinor,
                ShowAcNNom,
                ShowAcMNom,
                ShowGroup,
                Convert.ToString(drpGroupType.SelectedItem.Text)

                );
            ViewState["dataset"] = ds;


            BindData();
            //if (ds.Tables[0].Rows.Count > 0)
            //{
            //    EmployeeGrid.DataSource = ds.Tables[0];
            //    EmployeeGrid.DataBind();
            //    ScriptManager.RegisterStartupScript(this, GetType(), "JS", "ShowGrid();", true);
            //}
            //else
            //{
            //    ScriptManager.RegisterStartupScript(this, GetType(), "JS", "alert('No Record Found!.');", true);
            //}


        }
        protected void BindData()
        {
            if (ViewState["dataset"] != null)
            {
                DataSet dsBind = (DataSet)ViewState["dataset"];

                if (dsBind.Tables[0].Rows.Count > 0)
                {
                    EmployeeGrid.DataSource = dsBind.Tables[0];
                    EmployeeGrid.DataBind();
                    if (chkMinorNom.Checked == true)
                    {
                        EmployeeGrid.Columns[30].Visible = true;
                        EmployeeGrid.Columns[31].Visible = true;
                        EmployeeGrid.Columns[32].Visible = true;
                        EmployeeGrid.Columns[33].Visible = true;
                    }
                    else
                    {
                        EmployeeGrid.Columns[30].Visible = false;
                        EmployeeGrid.Columns[31].Visible = false;
                        EmployeeGrid.Columns[32].Visible = false;
                        EmployeeGrid.Columns[33].Visible = false;

                    }

                    if (chkGroup.Checked == true)
                        EmployeeGrid.Columns[5].Visible = true;
                    else
                        EmployeeGrid.Columns[5].Visible = false;

                    ScriptManager.RegisterStartupScript(this, GetType(), "JS", "ShowGrid();", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "JS", "alert('No Record Found!.');", true);
                }
            }

        }
        protected void ddlExport_SelectedIndexChanged(object sender, EventArgs e)
        {

            DataSet dtNew = (DataSet)ViewState["dataset"];
            DataTable dtEx = dtNew.Tables[0];
            dtEx.Columns[0].ColumnName = "First Holder Name";
            dtEx.Columns[1].ColumnName = "Second Holder Name";
            dtEx.Columns[2].ColumnName = "Third Holder Name";
            dtEx.Columns[3].ColumnName = "Branch";
            dtEx.Columns[4].ColumnName = "Group Details";
            dtEx.Columns[5].ColumnName = "Beneficiary A/C ID";
            dtEx.Columns[6].ColumnName = "A/C Category";
            dtEx.Columns[7].ColumnName = "BO Status";
            dtEx.Columns[8].ColumnName = "BO Sub Status";
            dtEx.Columns[9].ColumnName = "Tax Deduction Status";
            dtEx.Columns[10].ColumnName = "First Holder Mob.";
            dtEx.Columns[11].ColumnName = "First Holder Address";
            dtEx.Columns[12].ColumnName = "First Holder PH";
            dtEx.Columns[13].ColumnName = "First Holder PAN";
            dtEx.Columns[14].ColumnName = "Second Holder PAN";
            dtEx.Columns[15].ColumnName = "Third Holder PAN";
            dtEx.Columns[16].ColumnName = "First Holder Email";
            dtEx.Columns[17].ColumnName = "First Holder Husband/Gurdian Name";
            dtEx.Columns[18].ColumnName = "Trading Code";
            dtEx.Columns[19].ColumnName = "Account Status";
            dtEx.Columns[20].ColumnName = "A/C Activation Date";
            dtEx.Columns[21].ColumnName = "A/C Closure Date";
            dtEx.Columns[22].ColumnName = "Nominee Name";
            dtEx.Columns[23].ColumnName = "Nominee Address";
            dtEx.Columns[24].ColumnName = "Minor Nominee DOB";
            dtEx.Columns[25].ColumnName = "Bank A/C No";
            dtEx.Columns[26].ColumnName = "MICR";
            dtEx.Columns[27].ColumnName = "Electronic Devidend";
            dtEx.Columns[28].ColumnName = "Bank Name";
            dtEx.Columns[29].ColumnName = "Ben. Bank Code";
            dtEx.Columns[30].ColumnName = "Ben. Bank A/C No.";
            dtEx.Columns[31].ColumnName = "POA Status";
            //dtEx.Columns[30].ColumnName = "Bo ID";

            if (chkMinorNom.Checked == true)
            {
                dtEx.Columns[33].ColumnName = "POA ID";
                dtEx.Columns[34].ColumnName = "POA Name";
                dtEx.Columns[35].ColumnName = "POA Start Date";
                dtEx.Columns[36].ColumnName = "POA End Date";

            }
            else
            {
                dtEx.Columns.Remove("CdslClients_POAID");
                dtEx.Columns.Remove("CdslClients_POAName");
                dtEx.Columns.Remove("POAStartDate");
                dtEx.Columns.Remove("POAEndDate");

            }
            dtEx.Columns.Remove("CDSLCLIENTS_boid");
            if (chkGroup.Checked != true)
                dtEx.Columns.Remove("Group Details");
            DataTable CompanyName = oDbEngine.GetDataTable("tbl_master_company", "cmp_Name", " cmp_internalId='" + Session["LastCompany"].ToString() + "'");
            DataTable dtReportHeader = new DataTable();
            dtReportHeader.Columns.Add(new DataColumn("Header", typeof(String))); //0
            DataRow HeaderRow = dtReportHeader.NewRow();
            HeaderRow[0] = CompanyName.Rows[0][0].ToString();
            dtReportHeader.Rows.Add(HeaderRow);
            DataRow DrRowR1 = dtReportHeader.NewRow();
            DrRowR1[0] = lblReportHeader.Text;
            //DrRowR1[0] = lblReportHeader.Text;
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

            if (ddlExport.SelectedItem.Value == "E")
            {
                //oconverter.ExcelImport(dtBilling, "Daily Billing");
                objExcel.ExportToExcelforExcel(dtEx, "CDSL Client Master", "Total", dtReportHeader, dtReportFooter);
            }
            else if (ddlExport.SelectedItem.Value == "P")
            {
                objExcel.ExportToPDF(dtEx, "CDSL Client Master", "Total", dtReportHeader, dtReportFooter);
            }

            //Int32 Filter = int.Parse(ddlExport.SelectedItem.Value.ToString());
            //switch (Filter)
            //{
            //    case 1:
            //        exporter.WriteXlsToResponse();               
            //        break;
            //    case 2:
            //        exporter.WriteXlsToResponse();
            //        break;

            //}
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


        protected void EmployeeGrid_HtmlRowCreated(object sender, ASPxGridViewTableRowEventArgs e)
        {


            if (e.RowType == GridViewRowType.Data)
            {
                ASPxLabel textbox1 = (ASPxLabel)EmployeeGrid.FindRowCellTemplateControl(e.VisibleIndex, (GridViewDataColumn)EmployeeGrid.Columns["FirstHolderName"], "ASPxTextBox1");
                textbox1.Attributes.Add("onKeypress", "return MaskMoney(event)");
                textbox1.Focus();
                textbox1.Attributes.Add("onKeypress", "return MaskMoney(event)");
                ASPxLabel textbox2 = (ASPxLabel)EmployeeGrid.FindRowCellTemplateControl(e.VisibleIndex, (GridViewDataColumn)EmployeeGrid.Columns["BenAccountID"], "ASPxTextBox2");
                textbox2.ToolTip = "First Holder Name : " + textbox1.Text + " AccountID:" + textbox2.Text;
                textbox2.Attributes.Add("onKeypress", "return MaskMoney(event)");
                e.Row.ToolTip = textbox1.Text + "  [" + textbox2.Text + "]";

            }



        }

        protected void EmployeeGrid_PageIndexChanged(object sender, EventArgs e)
        {
            BindData();
            ScriptManager.RegisterStartupScript(this, GetType(), "JS", "ShowGrid();", true);
        }

        protected void btnPrint_Click(object sender, EventArgs e)
        {
            string AccountStatus = "";
            DateTime FromDate;
            DateTime ToDate;
            string AccType = "";
            string AccSubType = "";
            string BenCategory = "";
            string BenOCP = "";
            string ShowNHolder = "";
            string ShowNPOA = "";
            string ShowAcMinor = "";
            string ShowAcNNom = "";
            string ShowAcMNom = "";
            string DateRangeSelection = "";
            string SelectedClient = "";
            string ShowGroup = "";
            if (RadClientSelected.Checked == true)
            {
                SelectedClient = HDNClient.Value;
            }

            if (RadDateRangeS.Checked == true)
            {
                DateRangeSelection = "Y";
            }

            if (RadAll.Checked == true)
            {
                AccountStatus = "A";
            }
            else if (RadActive.Checked == true)
            {
                AccountStatus = "B";
            }
            else if (RadClosed.Checked == true)
            {
                AccountStatus = "C";
            }


            if (rdbMainSelected.Checked == true)
                AccType = HDNType.Value;
            if (rdSelSegment.Checked == true)
                AccSubType = HDNSubType.Value;
            if (ddlCat.SelectedItem.Value != "0")
                BenCategory = ddlCat.SelectedItem.Value;
            if (ddlOcp.SelectedItem.Value != "0")
                BenOCP = ddlOcp.SelectedItem.Value;
            if (ChkAHolder.Checked == true)
                ShowNHolder = "Y";
            if (ChkPOA.Checked == true)
                ShowNPOA = "Y";
            if (chkMinor.Checked == true)
                ShowAcMinor = "Y";
            if (chkNom.Checked == true)
                ShowAcNNom = "Y";
            if (chkMinorNom.Checked == true)
                ShowAcMNom = "Y";
            if (chkGroup.Checked == true)
                ShowGroup = "y";
            else
                ShowGroup = "n";

            DataSet ds = new DataSet();
            ds = oReports.Fetch_CDSLClientMaster(
                SelectedClient,
                DateRangeSelection,
                AccountStatus,
                 Convert.ToString(dtFrom.Value),
                 Convert.ToString(dtTo.Value),
                AccType,
                AccSubType,
                BenCategory,
                BenOCP,
                ShowNHolder,
                ShowNPOA,
                ShowAcMinor,
                ShowAcNNom,
                ShowAcMNom,
                ShowGroup,
                Convert.ToString(drpGroupType.SelectedItem.Text)

                );
            ViewState["dataset"] = ds;

            //   DataSet ds = new DataSet();
            DataTable dtComp = oDbEngine.GetDataTable("tbl_master_company", " cmp_name,(Select top 1 phf_countryCode+'-'+phf_areaCode+'-'+phf_phoneNumber from tbl_master_phonefax where phf_cntId=cmp_internalid) as cmpphno,(select top 1(isnull(add_address1,'')+' '+ isnull(add_address2,'')+' '+isnull(add_address3,'')+','+isnull(city_name,'')+'-'+  isnull(add_pin,'')) from tbl_master_address,tbl_master_city where add_city=city_id and add_cntID=cmp_internalid AND add_entity='Company' AND add_addressType='Office')as cmpaddress,(select top 1 eml_email from tbl_master_email   where eml_cntid=cmp_internalid) as Email  ", " cmp_internalid in('" + Session["LastCompany"].ToString() + "') ");
            // ds = (DataSet)ViewState["dataset"];
            byte[] logoinByte;

            ds.Tables[0].Columns.Add("Image", System.Type.GetType("System.Byte[]"));

            decimal mTotAmt = 0;
            if (oconverter.getLogoImage(HttpContext.Current.Server.MapPath(@"..\images\logo.bmp"), out logoinByte) != 1)
            {
                ScriptManager.RegisterStartupScript(HttpContext.Current.Handler as Page, HttpContext.Current.GetType(), "logo", "alert('Logo not Found.');", true);
            }
            else
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    ds.Tables[0].Rows[i]["Image"] = logoinByte;

                }
            }

            string Templ = "";
            ds.Tables[0].Columns.Add("Template");
            if (chkHeader.Checked == true)
            {
                if (txtHeader_hidden.Value != "")
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        //DataTable dtCnt = oDbEngine.GetDataTable("master_cdslclients ", " top 1 CdslClients_contactid", "cdslclients_BOid='" + ds.Tables[0].Rows[i]["CDSLCLIENTS_boid"].ToString() + "'");
                        Templ = oDbEngine.GenerateGenericTemplate(txtHeader_hidden.Value.ToString(), ds.Tables[0].Rows[i]["CDSLCLIENTS_boid"].ToString());
                        ds.Tables[0].Rows[i]["Template"] = Templ;
                    }

                }

            }
            else
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    ds.Tables[0].Rows[i]["Template"] = Templ;

                }
            }


            byte[] SignByte;
            ds.Tables[0].Columns.Add("Signature", System.Type.GetType("System.Byte[]"));
            ds.Tables[0].Columns.Add("SignName");
            if (ChkSignatory.Checked == true)
            {
                if (txtSignature_hidden.Value != "")
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        DataTable dtImg = oDbEngine.GetDataTable("tbl_master_document ", " top 1 * ,(select isnull(ltrim(rtrim(cnt_firstname)),'') + ' ' + isnull(ltrim(rtrim(cnt_middlename)),'') +' '+ isnull(ltrim(rtrim(cnt_lastname)),'')   from tbl_master_contact where cnt_internalid='" + txtSignature_hidden.Value + "') as SigName ", " doc_contactid='" + txtSignature_hidden.Value + "' and  doc_documentTypeId=  (select top 1 dty_id from tbl_master_documentType  where dty_documentType='Signature'  and dty_applicableFor='Employee') ");
                        DataTable dtName = oDbEngine.GetDataTable("tbl_master_contact ", "isnull(ltrim(rtrim(cnt_firstname)),'') + ' '+ isnull(ltrim(rtrim(cnt_middlename)),'') + ' ' +isnull(ltrim(rtrim(cnt_lastname)),'') as ClientName", "cnt_internalid='" + txtSignature_hidden.Value.ToString() + "'");
                        if (dtImg.Rows.Count > 0)
                        {
                            string[] source = dtImg.Rows[0]["doc_source"].ToString().Split('~');
                            string imageFilePath = @"..\Documents\" + source[1].ToString().Trim();

                            if (oconverter.getLogoImage(HttpContext.Current.Server.MapPath(imageFilePath), out SignByte) == 1)
                            {
                                ds.Tables[0].Rows[i]["Signature"] = SignByte;
                                ds.Tables[0].Rows[i]["SignName"] = dtName.Rows[0][0].ToString();

                            }

                        }

                    }

                }

            }

            DataSet dn = new DataSet();


            ReportDocument report = new ReportDocument();
            // ds.Tables[0].WriteXmlSchema("E:\\RPTXSD\\CDSLClientMaster.xsd");
            //ds.Tables[1].WriteXmlSchema("E:\\RPTXSD\\ClientMasterMainDetail.xsd");



            report.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Portrait;
            string tmpPdfPath = string.Empty;
            tmpPdfPath = HttpContext.Current.Server.MapPath("..\\Reports\\CDSLClientMaster.rpt");
            report.Load(tmpPdfPath);
            report.SetDataSource(ds.Tables[0]);
            report.VerifyDatabase();




            if (dtComp.Rows.Count > 0)
            {
                if (dtComp.Rows[0]["cmp_name"].ToString() != "")
                {
                    report.SetParameterValue("@CompanyName", (string)dtComp.Rows[0]["cmp_name"].ToString());
                }
                else
                {
                    report.SetParameterValue("@CompanyName", (string)"COMPANY NAME");
                }
                if (dtComp.Rows[0]["cmpphno"].ToString() != "")
                {
                    report.SetParameterValue("@CompanyPhone", (object)dtComp.Rows[0]["cmpphno"].ToString());
                }
                else
                {
                    report.SetParameterValue("@CompanyPhone", (object)"COMPANY Phone");
                }
                if (dtComp.Rows[0]["cmpaddress"].ToString() != "")
                {
                    report.SetParameterValue("@CompanyAddress", (object)dtComp.Rows[0]["cmpaddress"].ToString());
                }
                else
                {
                    report.SetParameterValue("@CompanyAddress", (object)"COMPANY Address");
                }
                if (dtComp.Rows[0]["Email"].ToString() != "")
                {
                    report.SetParameterValue("@Email", (object)dtComp.Rows[0]["Email"].ToString());
                }
                else
                {
                    report.SetParameterValue("@Email", (object)"COMPANY Email");
                }
                if (Session["usersegid"].ToString() != "")
                {
                    report.SetParameterValue("@ExchangeID", (object)Session["usersegid"].ToString());
                }



            }

            report.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, HttpContext.Current.Response, true, "CDSLClientsMaster");
            report.Dispose();
            GC.Collect();
        }

    }
}