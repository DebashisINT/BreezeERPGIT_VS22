using System;
using System.Configuration;
using System.Data;
using System.IO;
using System.Web;
using System.Web.UI;
using BusinessLogicLayer;
namespace ERP.OMS.Management.DailyTask
{
    public partial class Management_DailyTask_frm_NSECDXTRADES : ERP.OMS.ViewState_class.VSPage
    {
        BusinessLogicLayer.Converter oconverter = new BusinessLogicLayer.Converter();
        FileInfo FIICXCSV = null;
        public string[] InputName = new string[20];
        public string[] InputType = new string[20];
        public string[] InputValue = new string[20];
        public string[] InputName1 = new string[20];
        public string[] InputType1 = new string[20];
        public string[] InputValue1 = new string[20];
        DataTable dt1 = new DataTable();
        string FilePath = "";
        string OutPut = null;
        string stringdate = "";
        private static String path, path1;
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
               //Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "JSct", "height();", true);
            this.Page.ClientScript.RegisterStartupScript(GetType(), "hide1", "<script>hide();</script>");
        }
        protected void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {

                if (McxTradeSelectFile.FileContent.Length != 0)
                {

                    BusinessLogicLayer.TransctionDescription td = new BusinessLogicLayer.TransctionDescription();

                    FilePath = Path.GetFullPath(McxTradeSelectFile.PostedFile.FileName);
                    String FileName = Path.GetFileName(FilePath);
                    //hdfname.Value = FileName;
                    String UploadPath = Server.MapPath((ConfigurationManager.AppSettings["SaveCSV"].ToString() + FileName));
                    McxTradeSelectFile.PostedFile.SaveAs(UploadPath);

                    FileInfo FICSV = new FileInfo(UploadPath);
                    path = Path.Combine(ConfigurationManager.AppSettings["SaveCSVsql"], FileName);
                    FIICXCSV = new FileInfo(UploadPath);
                    File.Copy(UploadPath, path, true);

                    ClearArray();
                  //  BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);

                    BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

                    if (cmbTrade.SelectedValue == "0")
                    {
                        InputName[0] = "Module";
                        InputName[2] = "SegmentId";
                        InputName[3] = "COMPANY_ID";
                        InputName[1] = "FilePath";
                        InputName[4] = "ModifyUser";
                        InputName[5] = "LckTradeDate";
                        InputName[6] = "ExpireDate";

                        InputType[0] = "V";
                        InputType[2] = "I";
                        InputType[3] = "V";
                        InputType[1] = "V";
                        InputType[4] = "I";
                        InputType[5] = "D";
                        InputType[6] = "D";

                        InputValue[0] = "InsertTrade";
                        InputValue[2] = HttpContext.Current.Session["usersegid"].ToString().Trim();
                        InputValue[3] = HttpContext.Current.Session["LastCompany"].ToString().Trim();
                        InputValue[1] = path.ToString().Trim();
                        InputValue[4] = HttpContext.Current.Session["userid"].ToString().Trim();
                        if (Session["LCKTRADE"] != null)
                        {
                            InputValue[5] = Convert.ToDateTime(Session["LCKTRADE"]).ToString("yyyy-MM-dd");
                        }
                        else
                        {
                            InputValue[5] = "1900-01-01";
                        }
                        InputValue[6] = Convert.ToDateTime(Session["ExpireDate"]).ToString("yyyy-MM-dd");

                        DataSet ds = null;

                        ds = BusinessLogicLayer.SQLProcedures.SelectProcedureArrDS("[Import_NSECDXTrades_FinalTxtCheck]", InputName, "LockMsg", InputType, "V", InputValue, ref OutPut);

                        if (OutPut != "")
                        {
                            importstatus.Text = OutPut.ToString();
                        }
                        else
                        {
                            if (ds.Tables[0].Rows[0]["OrderUserLastModifiedTime"].ToString() == "0")
                            {

                                ds = BusinessLogicLayer.SQLProcedures.SelectProcedureArrDS("[SP_INSUP_NSECDXTRADES]", InputName, InputType, InputValue);
                            }
                            else
                            {

                                ds = BusinessLogicLayer.SQLProcedures.SelectProcedureArrDS("[Import_NSECDXTrades_FinalTxtNew]", InputName, InputType, InputValue);

                            }
                            Session["ImportDetails"] = null;
                            Session["ImportDetails"] = ds;
                            ShowSummary(ds);
                        }
                    }
                    else if (cmbTrade.SelectedValue == "1")
                    {
                        InputName[1] = "SegmentId";
                        InputName[2] = "COMPANY_ID";
                        InputName[0] = "FilePath";
                        InputName[3] = "ModifyUser";
                        InputName[4] = "LckTradeDate";
                        InputName[5] = "ExpireDate";

                        InputType[1] = "I";
                        InputType[2] = "V";
                        InputType[0] = "V";
                        InputType[3] = "I";
                        InputType[4] = "D";
                        InputType[5] = "D";

                        InputValue[1] = HttpContext.Current.Session["usersegid"].ToString().Trim();
                        InputValue[2] = HttpContext.Current.Session["LastCompany"].ToString().Trim();
                        InputValue[0] = path.ToString().Trim();
                        InputValue[3] = HttpContext.Current.Session["userid"].ToString().Trim();
                        if (Session["LCKTRADE"] != null)
                        {
                            InputValue[4] = Convert.ToDateTime(Session["LCKTRADE"]).ToString("yyyy-MM-dd");
                        }
                        else
                        {
                            InputValue[4] = "1900-01-01";
                        }
                        InputValue[5] = Convert.ToDateTime(Session["ExpireDate"]).ToString("yyyy-MM-dd");

                        DataSet ds = null;

                        ds = BusinessLogicLayer.SQLProcedures.SelectProcedureArrDS("[SP_INSUP_NSECDXTRADES1]", InputName, "LockMsg", InputType, "V", InputValue, ref OutPut);

                        if (OutPut != "")
                        {
                            importstatus.Text = OutPut.ToString();
                        }
                        else
                        {
                            Session["ImportDetails"] = null;
                            Session["ImportDetails"] = ds;
                            ShowSummary(ds);
                        }
                    }
                    else if (cmbTrade.SelectedValue == "7")
                    {
                        InputName[1] = "SegmentId";
                        InputName[2] = "COMPANY_ID";
                        InputName[0] = "FilePath";
                        InputName[3] = "ModifyUser";
                        InputName[4] = "LckTradeDate";
                        InputName[5] = "ExpireDate";

                        InputType[1] = "I";
                        InputType[2] = "V";
                        InputType[0] = "V";
                        InputType[3] = "I";
                        InputType[4] = "D";
                        InputType[5] = "D";

                        InputValue[1] = HttpContext.Current.Session["usersegid"].ToString().Trim();
                        InputValue[2] = HttpContext.Current.Session["LastCompany"].ToString().Trim();
                        InputValue[0] = path.ToString().Trim();
                        InputValue[3] = HttpContext.Current.Session["userid"].ToString().Trim();
                        if (Session["LCKTRADE"] != null)
                        {
                            InputValue[4] = Convert.ToDateTime(Session["LCKTRADE"]).ToString("yyyy-MM-dd");
                        }
                        else
                        {
                            InputValue[4] = "1900-01-01";
                        }
                        InputValue[5] = Convert.ToDateTime(Session["ExpireDate"]).ToString("yyyy-MM-dd");

                        DataSet ds = null;

                        ds = BusinessLogicLayer.SQLProcedures.SelectProcedureArrDS("[Import_NSECDX_OdinTXT]", InputName, "LockMsg", InputType, "V", InputValue, ref OutPut);

                        if (OutPut != "")
                        {
                            importstatus.Text = OutPut.ToString();
                        }
                        else
                        {
                            Session["ImportDetails"] = null;
                            Session["ImportDetails"] = ds;
                            ShowSummary(ds);
                        }
                    }
                    else if (cmbTrade.SelectedValue == "9")
                    {
                        InputName[1] = "SegmentId";
                        InputName[2] = "COMPANY_ID";
                        InputName[0] = "FilePath";
                        InputName[3] = "ModifyUser";
                        InputName[4] = "LckTradeDate";
                        InputName[5] = "ExpireDate";

                        InputType[1] = "I";
                        InputType[2] = "V";
                        InputType[0] = "V";
                        InputType[3] = "I";
                        InputType[4] = "D";
                        InputType[5] = "D";

                        InputValue[1] = HttpContext.Current.Session["usersegid"].ToString().Trim();
                        InputValue[2] = HttpContext.Current.Session["LastCompany"].ToString().Trim();
                        InputValue[0] = path.ToString().Trim();
                        InputValue[3] = HttpContext.Current.Session["userid"].ToString().Trim();
                        if (Session["LCKTRADE"] != null)
                        {
                            InputValue[4] = Convert.ToDateTime(Session["LCKTRADE"]).ToString("yyyy-MM-dd");
                        }
                        else
                        {
                            InputValue[4] = "1900-01-01";
                        }
                        InputValue[5] = Convert.ToDateTime(Session["ExpireDate"]).ToString("yyyy-MM-dd");

                        DataSet ds = null;

                        ds = BusinessLogicLayer.SQLProcedures.SelectProcedureArrDS("[Import_NSECDX_PS03Trades]", InputName, "LockMsg", InputType, "V", InputValue, ref OutPut);

                        if (OutPut != "")
                        {
                            importstatus.Text = OutPut.ToString();
                        }
                        else
                        {
                            Session["ImportDetails"] = null;
                            Session["ImportDetails"] = ds;
                            ShowSummary(ds);
                        }
                    }
                    else if (cmbTrade.SelectedValue == "8")
                    {
                        InputName[1] = "SegmentId";
                        InputName[2] = "COMPANY_ID";
                        InputName[0] = "FilePath";
                        InputName[3] = "ModifyUser";
                        InputName[4] = "LckTradeDate";
                        InputName[5] = "ExpireDate";

                        InputType[1] = "I";
                        InputType[2] = "V";
                        InputType[0] = "V";
                        InputType[3] = "I";
                        InputType[4] = "D";
                        InputType[5] = "D";

                        InputValue[1] = HttpContext.Current.Session["usersegid"].ToString().Trim();
                        InputValue[2] = HttpContext.Current.Session["LastCompany"].ToString().Trim();
                        InputValue[0] = path.ToString().Trim();
                        InputValue[3] = HttpContext.Current.Session["userid"].ToString().Trim();
                        if (Session["LCKTRADE"] != null)
                        {
                            InputValue[4] = Convert.ToDateTime(Session["LCKTRADE"]).ToString("yyyy-MM-dd");
                        }
                        else
                        {
                            InputValue[4] = "1900-01-01";
                        }
                        InputValue[5] = Convert.ToDateTime(Session["ExpireDate"]).ToString("yyyy-MM-dd");

                        DataSet ds = null;

                        ds = BusinessLogicLayer.SQLProcedures.SelectProcedureArrDS("[Import_NSECDXTrades_Flash]", InputName, "LockMsg", InputType, "V", InputValue, ref OutPut);

                        if (OutPut != "")
                        {
                            importstatus.Text = OutPut.ToString();
                        }
                        else
                        {
                            Session["ImportDetails"] = null;
                            Session["ImportDetails"] = ds;
                            ShowSummary(ds);
                        }
                    }
                    else if (cmbTrade.SelectedValue == "6")
                    {
                        InputName[1] = "SegmentId";
                        InputName[2] = "COMPANY_ID";
                        InputName[0] = "FilePath";
                        InputName[3] = "ModifyUser";
                        InputName[4] = "LckTradeDate";
                        InputName[5] = "ExpireDate";

                        InputType[1] = "I";
                        InputType[2] = "V";
                        InputType[0] = "V";
                        InputType[3] = "I";
                        InputType[4] = "D";
                        InputType[5] = "D";

                        InputValue[1] = HttpContext.Current.Session["usersegid"].ToString().Trim();
                        InputValue[2] = HttpContext.Current.Session["LastCompany"].ToString().Trim();
                        InputValue[0] = path.ToString().Trim();
                        InputValue[3] = HttpContext.Current.Session["userid"].ToString().Trim();
                        if (Session["LCKTRADE"] != null)
                        {
                            InputValue[4] = Convert.ToDateTime(Session["LCKTRADE"]).ToString("yyyy-MM-dd");
                        }
                        else
                        {
                            InputValue[4] = "1900-01-01";
                        }
                        InputValue[5] = Convert.ToDateTime(Session["ExpireDate"]).ToString("yyyy-MM-dd");

                        DataSet ds = null;

                        ds = BusinessLogicLayer.SQLProcedures.SelectProcedureArrDS("[Import_NSECDXTrades_Greek]", InputName, "LockMsg", InputType, "V", InputValue, ref OutPut);

                        if (OutPut != "")
                        {
                            importstatus.Text = OutPut.ToString();
                        }
                        else
                        {
                            Session["ImportDetails"] = null;
                            Session["ImportDetails"] = ds;
                            ShowSummary(ds);
                        }
                    }
                    else if (cmbTrade.SelectedValue == "2")
                    {
                        InputName[0] = "FilePath";
                        InputName[1] = "SegmentId";
                        InputName[2] = "COMPANY_ID";
                        InputName[3] = "ModifyUser";
                        InputName[4] = "LckTradeDate";
                        InputName[5] = "ExpireDate";

                        InputType[0] = "V";
                        InputType[1] = "I";
                        InputType[2] = "V";
                        InputType[3] = "I";
                        InputType[4] = "D";
                        InputType[5] = "D";

                        InputValue[0] = path.ToString().Trim();
                        InputValue[1] = HttpContext.Current.Session["usersegid"].ToString().Trim();
                        InputValue[2] = HttpContext.Current.Session["LastCompany"].ToString().Trim();
                        InputValue[3] = HttpContext.Current.Session["userid"].ToString().Trim();
                        if (Session["LCKTRADE"] != null)
                        {
                            InputValue[4] = Convert.ToDateTime(Session["LCKTRADE"]).ToString("yyyy-MM-dd");
                        }
                        else
                        {
                            InputValue[4] = "1900-01-01";
                        }
                        InputValue[5] = Convert.ToDateTime(Session["ExpireDate"]).ToString("yyyy-MM-dd");

                        DataSet ds = null;
                        ds = BusinessLogicLayer.SQLProcedures.SelectProcedureArrDS("[Import_NSECDXTradeCSV]", InputName, "LockMsg", InputType, "V", InputValue, ref OutPut);

                        if (OutPut != "")
                        {
                            importstatus.Text = OutPut.ToString();
                        }
                        else
                        {
                            Session["ImportDetails"] = null;
                            Session["ImportDetails"] = ds;
                            ShowSummary(ds);
                        }
                    }
                    else if (cmbTrade.SelectedValue == "3")
                    {
                        InputName[0] = "FilePath";
                        InputName[1] = "SegmentId";
                        InputName[2] = "COMPANY_ID";
                        InputName[3] = "ModifyUser";
                        InputName[4] = "LckTradeDate";
                        InputName[5] = "ExpireDate";

                        InputType[0] = "V";
                        InputType[1] = "I";
                        InputType[2] = "V";
                        InputType[3] = "I";
                        InputType[4] = "D";
                        InputType[5] = "D";

                        InputValue[0] = path.ToString().Trim();
                        InputValue[1] = HttpContext.Current.Session["usersegid"].ToString().Trim();
                        InputValue[2] = HttpContext.Current.Session["LastCompany"].ToString().Trim();
                        InputValue[3] = HttpContext.Current.Session["userid"].ToString().Trim();
                        if (Session["LCKTRADE"] != null)
                        {
                            InputValue[4] = Convert.ToDateTime(Session["LCKTRADE"]).ToString("yyyy-MM-dd");
                        }
                        else
                        {
                            InputValue[4] = "1900-01-01";
                        }
                        InputValue[5] = Convert.ToDateTime(Session["ExpireDate"]).ToString("yyyy-MM-dd");

                        DataSet ds = null;
                        ds = BusinessLogicLayer.SQLProcedures.SelectProcedureArrDS("[Import_NSECDXTRADES_TradeOP]", InputName, "LockMsg", InputType, "V", InputValue, ref OutPut);

                        if (OutPut != "")
                        {
                            importstatus.Text = OutPut.ToString();
                        }
                        else
                        {
                            Session["ImportDetails"] = null;
                            Session["ImportDetails"] = ds;
                            ShowSummary(ds);
                        }
                    }

                    else if (cmbTrade.SelectedValue == "4")
                    {
                        InputName[0] = "FilePath";
                        InputName[1] = "SegmentId";
                        InputName[2] = "COMPANY_ID";
                        InputName[3] = "ModifyUser";
                        InputName[4] = "LckTradeDate";
                        InputName[5] = "ExpireDate";

                        InputType[0] = "V";
                        InputType[1] = "I";
                        InputType[2] = "V";
                        InputType[3] = "I";
                        InputType[4] = "D";
                        InputType[5] = "D";

                        InputValue[0] = path.ToString().Trim();
                        InputValue[1] = HttpContext.Current.Session["usersegid"].ToString().Trim();
                        InputValue[2] = HttpContext.Current.Session["LastCompany"].ToString().Trim();
                        InputValue[3] = HttpContext.Current.Session["userid"].ToString().Trim();
                        if (Session["LCKTRADE"] != null)
                        {
                            InputValue[4] = Convert.ToDateTime(Session["LCKTRADE"]).ToString("yyyy-MM-dd");
                        }
                        else
                        {
                            InputValue[4] = "1900-01-01";
                        }
                        InputValue[5] = Convert.ToDateTime(Session["ExpireDate"]).ToString("yyyy-MM-dd");

                        DataSet ds = null;
                        ds = BusinessLogicLayer.SQLProcedures.SelectProcedureArrDS("[Import_NSECDXTRADES_Now]", InputName, "LockMsg", InputType, "V", InputValue, ref OutPut);

                        if (OutPut != "")
                        {
                            importstatus.Text = OutPut.ToString();
                        }
                        else
                        {
                            Session["ImportDetails"] = null;
                            Session["ImportDetails"] = ds;
                            ShowSummary(ds);
                        }
                    }
                    else if (cmbTrade.SelectedValue == "5")
                    {
                        InputName[0] = "FilePath";
                        InputName[1] = "SegmentId";
                        InputName[2] = "COMPANY_ID";
                        InputName[3] = "ModifyUser";
                        InputName[4] = "LckTradeDate";
                        InputName[5] = "ExpireDate";

                        InputType[0] = "V";
                        InputType[1] = "I";
                        InputType[2] = "V";
                        InputType[3] = "I";
                        InputType[4] = "D";
                        InputType[5] = "D";

                        InputValue[0] = path.ToString().Trim();
                        InputValue[1] = HttpContext.Current.Session["usersegid"].ToString().Trim();
                        InputValue[2] = HttpContext.Current.Session["LastCompany"].ToString().Trim();
                        InputValue[3] = HttpContext.Current.Session["userid"].ToString().Trim();
                        if (Session["LCKTRADE"] != null)
                        {
                            InputValue[4] = Convert.ToDateTime(Session["LCKTRADE"]).ToString("yyyy-MM-dd");
                        }
                        else
                        {
                            InputValue[4] = "1900-01-01";
                        }
                        InputValue[5] = Convert.ToDateTime(Session["ExpireDate"]).ToString("yyyy-MM-dd");

                        DataSet ds = null;
                        ds = BusinessLogicLayer.SQLProcedures.SelectProcedureArrDS("[Import_NSECDXTRADES_FinalTxt_DTB]", InputName, "LockMsg", InputType, "V", InputValue, ref OutPut);

                        if (OutPut != "")
                        {
                            importstatus.Text = OutPut.ToString();
                        }
                        else
                        {
                            Session["ImportDetails"] = null;
                            Session["ImportDetails"] = ds;
                            ShowSummary(ds);
                        }
                    }
                    Page.ClientScript.RegisterStartupScript(GetType(), "PageScript", "<script language='javascript'>alert('Import Successfully!');</script>");

                    if (File.Exists(UploadPath))
                    {
                        File.Delete(UploadPath);
                    }
                    if (File.Exists(path))
                    {
                        File.Delete(path);
                    }

                }
                else
                    Page.ClientScript.RegisterStartupScript(GetType(), "PageScript", "<script language='javascript'>alert('Selected File Cannot Be Blank!');</script>");

            }


            catch (Exception ex)
            {

            }
            finally
            {

            }
        }

        void ShowSummary(DataSet ds)
        {
            if (ds.Tables[0].Rows.Count > 0)
            {

                String strHtmlAllClient1 = String.Empty;
                int colcount = ds.Tables[0].Rows.Count;
                strHtmlAllClient1 = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";
                strHtmlAllClient1 += "<tr style=\"background-color: #DBEEF3;\">";
                strHtmlAllClient1 += "<td align=\"center\" colspan='" + colcount + "' style=\"width:400px; \"style=\"Color:blue;\">Following Clients were not Identified.</td>";
                strHtmlAllClient1 += "</tr>";
                strHtmlAllClient1 += "<tr style=\"background-color: lavender ;text-align:left\">";
                int j = 0;
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    j = j + 1 - 1;
                    if (j >= 10)
                    {
                        strHtmlAllClient1 += "</tr>";
                        strHtmlAllClient1 += "<tr style=\"background-color: lavender ;text-align:left\">";
                        j = 0;
                    }
                    strHtmlAllClient1 += "<td>" + ds.Tables[0].Rows[i]["ComExchangeTrades_CustomerUcc"] + "</td>";
                    j++;

                }
                strHtmlAllClient1 += "</tr></table>";
                divCust.InnerHtml = strHtmlAllClient1;
                this.Page.ClientScript.RegisterStartupScript(GetType(), "showTr_Cust", "<script>showtr_Cust();</script>");
            }
            String strHtmlAllClient2 = String.Empty;
            int colcount2 = ds.Tables[1].Rows.Count;
            strHtmlAllClient2 = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";
            strHtmlAllClient2 += "<tr style=\"background-color: white;\">";
            strHtmlAllClient2 += "<td align=\"center\" colspan=3 style=\"width:400px; \"style=\"Color:blue;\">Import Summary:.</td>";
            strHtmlAllClient2 += "<td colspan=4><a href=javascript:void(0); style=\"Color:red;\" onClick=javascript:OnLinkButtonClick()>Click Here To Go To Import Details</a></td>";
            strHtmlAllClient2 += "</tr>";
            strHtmlAllClient2 += "<tr style=\"background-color: #DBEEF3;\">";
            strHtmlAllClient2 += "<td align=\"center\"  style=\"width:400px; \"style=\"Color:blue;\">Sett.No.</td>";
            strHtmlAllClient2 += "<td align=\"center\"  style=\"width:400px; \"style=\"Color:blue;\">Sett.Type</td>";
            strHtmlAllClient2 += "<td align=\"center\"  style=\"width:400px; \"style=\"Color:blue;\">Records</td>";
            strHtmlAllClient2 += "<td align=\"center\"  style=\"width:400px; \"style=\"Color:blue;\">Buy Obligation</td>";
            strHtmlAllClient2 += "<td align=\"center\"  style=\"width:400px; \"style=\"Color:blue;\">Sell Obligation</td>";
            strHtmlAllClient2 += "<td align=\"center\"  style=\"width:400px; \"style=\"Color:blue;\">Net Obligation</td>";
            strHtmlAllClient2 += "</tr>";

            for (int i1 = 0; i1 < ds.Tables[1].Rows.Count; i1++)
            {
                strHtmlAllClient2 += "<tr style=\"background-color: lavender ;text-align:left\">";
                strHtmlAllClient2 += "<td align=\"left\">" + ds.Tables[1].Rows[i1]["Sett.No"] + "</td>";
                strHtmlAllClient2 += "<td align=\"left\">" + ds.Tables[1].Rows[i1]["Sett.Type"] + "</td>";
                strHtmlAllClient2 += "<td align=\"right\">" + ds.Tables[1].Rows[i1]["Records"] + "</td>";
                strHtmlAllClient2 += "<td align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ds.Tables[1].Rows[i1]["Buy Obligation"])) + "</td>";
                strHtmlAllClient2 += "<td align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ds.Tables[1].Rows[i1]["Sell Obligation"])) + "</td>";
                strHtmlAllClient2 += "<td align=\"right\" nowrap>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ds.Tables[1].Rows[i1]["Net Obligation"])) + "</td>";
                strHtmlAllClient2 += "</tr>";
            }
            strHtmlAllClient2 += "</table>";
            divStatus.InnerHtml = strHtmlAllClient2;

            if (ds.Tables[3].Rows.Count > 0)
            {
                String strHtmlAllClient4 = String.Empty;
                String strterminalID = String.Empty;
                foreach (DataRow dr in ds.Tables[3].Rows)
                {
                    strterminalID += "'" + dr["TerminalID"].ToString() + "'" + ",";
                }
                strterminalID = strterminalID.Substring(0, strterminalID.Length - 1);
                hdnTerminalID.Value = strterminalID;

                int colcount = ds.Tables[3].Rows.Count;
                strHtmlAllClient4 = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";
                strHtmlAllClient4 += "<tr style=\"background-color: #DBEEF3;\">";
                strHtmlAllClient4 += "<td align=\"center\" colspan='" + colcount + "' style=\"width:400px; \"style=\"Color:blue;\">Following Terminal IDs Are Not Present In Table.</td>";
                strHtmlAllClient4 += "</tr>";
                strHtmlAllClient4 += "<tr style=\"background-color: lavender ;text-align:left\">";
                int j = 0;
                for (int i = 0; i < ds.Tables[3].Rows.Count; i++)
                {
                    j = j + 1 - 1;
                    if (j >= 10)
                    {
                        strHtmlAllClient4 += "</tr>";
                        strHtmlAllClient4 += "<tr style=\"background-color: lavender ;text-align:left\">";
                        j = 0;
                    }
                    strHtmlAllClient4 += "<td>" + ds.Tables[3].Rows[i]["TERMINALID"] + "</td>";
                    j++;

                }
                strHtmlAllClient4 += "</tr>";
                strHtmlAllClient4 += "<tr style=\"background-color: white;\">";
                strHtmlAllClient4 += "<td align=\"center\" colspan=" + colcount + " style=\"Color:blue;\">Want To Remove Above Terminal IDs AND Re-import the The File ?</td>";
                strHtmlAllClient4 += "</tr></table>";
                divterminalID.InnerHtml = strHtmlAllClient4;

                this.Page.ClientScript.RegisterStartupScript(GetType(), "heightO", "<script>show();</script>");
            }

            if (ds.Tables[4].Rows.Count > 0)
            {
                hdnDate.Value = Convert.ToDateTime(ds.Tables[4].Rows[0]["TradeDate"]).ToString("yyyy-MM-dd");
            }

            stringdate = ds.Tables[4].Rows[0][0].ToString();
            InsertProduct(hdnDate.Value);


        }
        public void ClearArray()
        {
            Array.Clear(InputName, 0, InputName.Length - 1);
            Array.Clear(InputType, 0, InputType.Length - 1);
            Array.Clear(InputValue, 0, InputValue.Length - 1);
        }
        public void InsertProduct(string stringdate)
        {

            ClearArray();
            InputName[0] = "Module";
            InputName[1] = "Segment";
            InputName[2] = "tradedate";
            InputType[0] = "V";
            InputType[1] = "I";
            InputType[2] = "V";
            InputValue[0] = "SelectCURNullPId";
            InputValue[1] = HttpContext.Current.Session["usersegid"].ToString().Trim();
            InputValue[2] = stringdate.ToString().Trim();
            dt1 = BusinessLogicLayer.SQLProcedures.SelectProcedureArr("SP_SelectTradeProduct", InputName, InputType, InputValue);
            if (dt1.Rows.Count > 0)
            {
                for (int i = 0; i < dt1.Rows.Count; i++)
                {

                    ClearArray();
                    InputName[0] = "Module";
                    InputName[1] = "SecurityType";
                    InputName[2] = "Symbol";
                    InputName[3] = "StrikePrice";
                    InputName[4] = "SecuritySeries";
                    InputName[5] = "Expiry";
                    InputName[6] = "TradeDate";
                    InputName[7] = "Segment";

                    InputType[0] = "V";
                    InputType[1] = "C";
                    InputType[2] = "C";
                    InputType[3] = "DE";
                    InputType[4] = "C";
                    InputType[5] = "D";
                    InputType[6] = "D";
                    InputType[7] = "V";

                    InputValue[0] = "InsUpdateTradeNSECDX";
                    if (DBNull.Value != dt1.Rows[i]["ComExchangeTrades_SecurityType"] && dt1.Rows[i]["ComExchangeTrades_SecurityType"].ToString() != "")
                        InputValue[1] = dt1.Rows[i]["ComExchangeTrades_SecurityType"].ToString();
                    if (DBNull.Value != dt1.Rows[i]["ComExchangeTrades_SecuritySymbol"] && dt1.Rows[i]["ComExchangeTrades_SecuritySymbol"].ToString() != "")
                        InputValue[2] = dt1.Rows[i]["ComExchangeTrades_SecuritySymbol"].ToString();
                    if (DBNull.Value != dt1.Rows[i]["ComExchangeTrades_SecurityStrikePrice"] && dt1.Rows[i]["ComExchangeTrades_SecurityStrikePrice"].ToString() != "")
                    {
                        InputValue[3] = dt1.Rows[i]["ComExchangeTrades_SecurityStrikePrice"].ToString();
                    }
                    else
                    {
                        InputValue[3] = "0.00";
                    }
                    if (DBNull.Value != dt1.Rows[i]["ComExchangeTrades_SecuritySeries"] && dt1.Rows[i]["ComExchangeTrades_SecuritySeries"].ToString() != "")
                        InputValue[4] = dt1.Rows[i]["ComExchangeTrades_SecuritySeries"].ToString();
                    if (DBNull.Value != dt1.Rows[i]["ComExchangeTrades_SecurityExpiry"] && dt1.Rows[i]["ComExchangeTrades_SecurityExpiry"].ToString() != "")
                        InputValue[5] = dt1.Rows[i]["ComExchangeTrades_SecurityExpiry"].ToString();
                    if (DBNull.Value != dt1.Rows[i]["ComExchangeTrades_Tradedate"] && dt1.Rows[i]["ComExchangeTrades_Tradedate"].ToString() != "")
                        InputValue[6] = dt1.Rows[i]["ComExchangeTrades_Tradedate"].ToString();
                    if (DBNull.Value != dt1.Rows[i]["Comexchangetrades_segment"] && dt1.Rows[i]["Comexchangetrades_segment"].ToString() != "")
                        InputValue[7] = dt1.Rows[i]["Comexchangetrades_segment"].ToString();
                    SQLProcedures.SelectProcedureArr("[ImportTradeNSECDX]", InputName, InputType, InputValue);

                }

            }

            else
            {

            }

        }
        protected void cmbTrade_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbTrade.SelectedItem.Value == "0")
            {
                Label1.Text = "EG: X_DDMMYYYY_MemberCode.txt";
            }
            else if (cmbTrade.SelectedItem.Value == "1")
            {
                Label1.Text = "EG:colo_cdsDD-MM-YYYY.txt";
            }
            else if (cmbTrade.SelectedItem.Value == "2")
            {
                Label1.Text = "EG:X_TR01_<MEMBER CODE>_DDMMYYYY.CSV";
            }
            else if (cmbTrade.SelectedItem.Value == "3")
            {
                Label1.Text = "EG:TradeCD.txt";
            }
            else if (cmbTrade.SelectedItem.Value == "4")
            {
                Label1.Text = "";
            }
            if (cmbTrade.SelectedItem.Value == "5")
            {
                Label1.Text = "EG: Y_DDMMYYYY_MemberCode.txt";
            }

            if (cmbTrade.SelectedItem.Value == "6")
            {
                Label1.Text = "Greek Txt Files";
            }
            if (cmbTrade.SelectedItem.Value == "7")
            {
                Label1.Text = "Odin Txt Files";
            }
            if (cmbTrade.SelectedItem.Value == "8")
            {
                Label1.Text = "Flash CSV Files";
            }
        }
        protected void btnYes_Click(object sender, EventArgs e)
        {

            oDBEngine.DeleteValue("trans_ComExchangeTrades", "ComExchangeTrades_Tradedate='" + Convert.ToDateTime(hdnDate.Value).ToString("yyyy-MM-dd") + "' AND ComExchangeTrades_CompanyID='" + HttpContext.Current.Session["LastCompany"].ToString().Trim() + "' AND ComExchangeTrades_Segment='" + HttpContext.Current.Session["usersegid"].ToString().Trim() + "'AND ComExchangeTrades_TerminalID IN(" + hdnTerminalID.Value + ")");

        }
        protected void btnNo_Click(object sender, EventArgs e)
        {

        }
        protected void btnCust_Click(object sender, EventArgs e)
        {
            if (txtClient.Text != "")
            {
                oDBEngine.SetFieldValue("trans_ComExchangeTrades", "ComExchangeTrades_CustomerID='" + txtClient_hidden.Text + "'", "ComExchangeTrades_Tradedate='" + Convert.ToDateTime(hdnDate.Value).ToString("yyyy-MM-dd") + "' AND ComExchangeTrades_CompanyID='" + HttpContext.Current.Session["LastCompany"].ToString().Trim() + "' AND ComExchangeTrades_Segment='" + HttpContext.Current.Session["usersegid"].ToString().Trim() + "' and ComExchangeTrades_CustomerID is null");
                oDBEngine.SetFieldValue("trans_ComExchangeTrades", "ComExchangeTrades_BranchID=Cnt_BranchID from Tbl_master_Contact", "ComExchangeTrades_Tradedate='" + Convert.ToDateTime(hdnDate.Value).ToString("yyyy-MM-dd") + "' AND ComExchangeTrades_CompanyID='" + HttpContext.Current.Session["LastCompany"].ToString().Trim() + "' AND ComExchangeTrades_Segment='" + HttpContext.Current.Session["usersegid"].ToString().Trim() + "' and ComExchangeTrades_BranchID is null and ComExchangeTrades_CustomerID=cnt_InternalID and ComExchangeTrades_CustomerID='" + txtClient_hidden.Text + "'");
            }
        }
        protected void btnRemove_Click(object sender, EventArgs e)
        {
            oDBEngine.DeleteValue("trans_ComExchangeTrades", "ComExchangeTrades_Tradedate='" + Convert.ToDateTime(hdnDate.Value).ToString("yyyy-MM-dd") + "' AND ComExchangeTrades_CompanyID='" + HttpContext.Current.Session["LastCompany"].ToString().Trim() + "' AND ComExchangeTrades_Segment='" + HttpContext.Current.Session["usersegid"].ToString().Trim() + "'AND ComExchangeTrades_CustomerID is null");
            Page.ClientScript.RegisterStartupScript(GetType(), "PageScript", "<script language='javascript'>alert('Removed Successfully!');</script>");
        }
    }

}