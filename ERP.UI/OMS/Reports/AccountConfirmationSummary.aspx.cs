using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrystalDecisions.CrystalReports.Engine;


namespace ERP.OMS.Reports
{
    public partial class Reports_AccountConfirmationSummary : System.Web.UI.Page, System.Web.UI.ICallbackEventHandler
    {
        BusinessLogicLayer.Converter oconverter = new BusinessLogicLayer.Converter();
        BusinessLogicLayer.FAReportsOther oFAReportsOther = new BusinessLogicLayer.FAReportsOther();
        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);MULTI
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        DataSet ds = new DataSet();
        string data;

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
                Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script language='javascript'>Page_Load();</script>");
                Date();
                Segment();
            }
            //_____For performing operation without refreshing page___//
            String cbReference = Page.ClientScript.GetCallbackEventReference(this, "arg", "ReceiveServerData", "context");
            String callbackScript = "function CallServer(arg, context){ " + cbReference + ";}";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "CallServer", callbackScript, true);
            //___________-end here___//
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
                if (idlist[0].ToString().Trim() == "Clients")
                {
                    string[] val = cl[i].Split(';');
                    string[] AcVal = val[0].Split('-');
                    if (str == "")
                    {
                        str = "'" + AcVal[0] + "'";
                        str1 = "'" + AcVal[0] + "'" + ";" + val[1];
                    }
                    else
                    {
                        str += ",'" + AcVal[0] + "'";
                        str1 += "," + "'" + AcVal[0] + "'" + ";" + val[1];
                    }
                }
                else
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
            }

            if (idlist[0] == "Clients")
            {
                data = "Clients~" + str;
            }
            else if (idlist[0] == "Group")
            {
                data = "Group~" + str;
            }
            else if (idlist[0] == "Branch")
            {
                data = "Branch~" + str;
            }
            else if (idlist[0] == "BranchGroup")
            {
                data = "BranchGroup~" + str;
            }
            else if (idlist[0] == "Segment")
            {
                data = "Segment~" + str;
            }
        }

        private void Date()
        {
            DtFrom.EditFormatString = oconverter.GetDateFormat("Date");
            DtFrom.Value = Convert.ToDateTime(oDBEngine.GetDate().ToShortDateString());
            DtTo.EditFormatString = oconverter.GetDateFormat("Date");
            DtTo.Value = Convert.ToDateTime(oDBEngine.GetDate().ToShortDateString());
            DtPrintDate.EditFormatString = oconverter.GetDateFormat("Date");
            DtPrintDate.Value = Convert.ToDateTime(oDBEngine.GetDate().ToShortDateString());
        }

        private void Segment()
        {
            HiddenField_Segment.Value = Session["usersegid"].ToString();

            if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "1")
                litSegmentMain.InnerText = "NSE-CM";
            if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "2")
                litSegmentMain.InnerText = "NSE-FO";
            if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "3")
                litSegmentMain.InnerText = "NSE-CDX";
            if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "4")
                litSegmentMain.InnerText = "BSE-CM";
            if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "5")
                litSegmentMain.InnerText = "BSE-FO";
            if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "6")
                litSegmentMain.InnerText = "BSE-CDX";
            if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "7")
                litSegmentMain.InnerText = "MCX-COMM";
            if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "8")
                litSegmentMain.InnerText = "MCXSX-CDX";
            if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "9")
                litSegmentMain.InnerText = "NCDEX-COMM";
            if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "10")
                litSegmentMain.InnerText = "DGCX-COMM";
            if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "11")
                litSegmentMain.InnerText = "NMCE-COMM";
            if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "12")
                litSegmentMain.InnerText = "ICEX-COMM";
            if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "13")
                litSegmentMain.InnerText = "USE-CDX";
            if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "14")
                litSegmentMain.InnerText = "NSEL-SPOT";
            if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "15")
                litSegmentMain.InnerText = "CSE-CM";
            if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "17")
                litSegmentMain.InnerText = "ACE-COMM";
            if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "18")
                litSegmentMain.InnerText = "INMX-COMM";
            if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "19")
                litSegmentMain.InnerText = "MCXSX-CM";
            if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "20")
                litSegmentMain.InnerText = "MCXSX-FO";
            if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "21")
                litSegmentMain.InnerText = "BFX-COMM";
            if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "22")
                litSegmentMain.InnerText = "INSX-COMM";
            if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "23")
                litSegmentMain.InnerText = "INFX-COMM";
        }

        protected void btnhide_Click(object sender, EventArgs e)
        {
            if (ddlGroup.SelectedItem.Value.ToString() == "3")
            {
                if (HiddenField_Group.Value.ToString().Trim() == "")
                {
                    BindGroup();
                }
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

        private void Procedure()
        {
            string ClientId = string.Empty;
            string GrpType = string.Empty;
            string GrpId = string.Empty;
            string Segment = string.Empty;
            string HeaderId = string.Empty;
            string FooterId = string.Empty;
            byte[] Signature;

            if (RdbClientAll.Checked)
            {
                ClientId = "ALL";
            }
            else if (RdbClientPOA.Checked)
            {
                ClientId = "POA";
            }
            else if (RdbClientAllBtSelected.Checked)
            {
                ClientId = "'~'" + HiddenField_Client.Value.ToString().Trim();
            }
            else
            {
                ClientId = HiddenField_Client.Value.ToString().Trim();
            }

            if (ddlGroup.SelectedItem.Value.ToString() == "2")
            {
                GrpType = "BRANCH";
                if (RdbBranchAll.Checked)
                {
                    GrpId = "ALL";
                }
                else
                {
                    GrpId = HiddenField_Branch.Value;
                }
            }
            else if (ddlGroup.SelectedItem.Value.ToString() == "4")
            {
                GrpType = "BRANCHGROUP";
                if (RdbBranchAll.Checked)
                {
                    GrpId = "ALL";
                }
                else
                {
                    GrpId = HiddenField_BranchGroup.Value;
                }
            }
            else if (ddlGroup.SelectedItem.Value.ToString() == "3")
            {
                GrpType = ddlgrouptype.SelectedItem.Text.ToString().Trim();
                if (RdbGroupAll.Checked)
                {
                    GrpId = "ALL";
                }
                else
                {
                    GrpId = HiddenField_Group.Value;
                }
            }
            else if (ddlGroup.SelectedItem.Value.ToString() == "1")
            {
                GrpType = "BRANCH";
                GrpId = "ALL";
            }
            if (rdbSegmentAll.Checked)
            {
                Segment = "ALL";
            }
            else
            {
                Segment = HiddenField_Segment.Value;
            }

            if (txtHeader.Text.ToString().Trim() == "")
            {
                HeaderId = "NA";
            }
            else
            {
                HeaderId = txtHeader_hidden.Value.ToString().Trim();
            }
            if (txtFooter.Text.ToString().Trim() == "")
            {
                FooterId = "NA";
            }
            else
            {
                FooterId = txtFooter_hidden.Value.ToString().Trim();
            }
            string SignatureChk = "This is an electronicaly generated statement that requires no Signature. *";
            byte[] SignatureinByte;
            /////////Signature
            if (txtSignature.Text.ToString().Trim() != "")
            {
                if (oconverter.getSignatureImage(txtSignature_hidden.Value.ToString().Trim(), out SignatureinByte, "NSE") == 1)
                {
                    SignatureChk = "Y";
                    Signature = SignatureinByte;
                }
                else
                {
                    Signature = null;
                }
            }
            else
            {
                Signature = null;
            }

            ds = oFAReportsOther.Report_AccountConfirmationSummary(
                Convert.ToString(DtFrom.Value),
                Convert.ToString(DtTo.Value),
               Convert.ToString(ClientId),
               Convert.ToString(Session["userbranchHierarchy"]),
               Convert.ToString(GrpType),
              Convert.ToString(GrpId),
             Convert.ToString(Segment),
              Convert.ToString(Session["LastCompany"]),
             Convert.ToString(Session["LastFinYear"]),
              Convert.ToString(HeaderId),
             Convert.ToString(FooterId),
             Signature
                );

            FnSPCall(ds, SignatureChk.ToString().Trim());
            //using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]))
            //{
            //    SqlCommand cmd = new SqlCommand();
            //    cmd.Connection = con;
            //    cmd.CommandText = "[Report_AccountConfirmationSummary]";
            //    cmd.CommandType = CommandType.StoredProcedure;

            //    cmd.Parameters.AddWithValue("@FromDate", DtFrom.Value);
            //    cmd.Parameters.AddWithValue("@ToDate", DtTo.Value);

            //    if (RdbClientAll.Checked)
            //    {
            //        cmd.Parameters.AddWithValue("@ClientId", "ALL");
            //    }
            //    else if (RdbClientPOA.Checked)
            //    {
            //        cmd.Parameters.AddWithValue("@ClientId", "POA");
            //    }
            //    else if (RdbClientAllBtSelected.Checked)
            //    {
            //        cmd.Parameters.AddWithValue("@ClientId", "'~'" + HiddenField_Client.Value.ToString().Trim());
            //    }
            //    else
            //    {
            //        cmd.Parameters.AddWithValue("@ClientId", HiddenField_Client.Value.ToString().Trim());
            //    }
            //    cmd.Parameters.AddWithValue("@BranchHierchy", Session["userbranchHierarchy"].ToString());
            //    if (ddlGroup.SelectedItem.Value.ToString() == "2")
            //    {
            //        cmd.Parameters.AddWithValue("@GrpType", "BRANCH");
            //        if (RdbBranchAll.Checked)
            //        {
            //            cmd.Parameters.AddWithValue("@GrpId", "ALL");
            //        }
            //        else
            //        {
            //            cmd.Parameters.AddWithValue("@GrpId", HiddenField_Branch.Value);
            //        }
            //    }
            //    else if (ddlGroup.SelectedItem.Value.ToString() == "4")
            //    {
            //        cmd.Parameters.AddWithValue("@GrpType", "BRANCHGROUP");
            //        if (RdbBranchAll.Checked)
            //        {
            //            cmd.Parameters.AddWithValue("@GrpId", "ALL");
            //        }
            //        else
            //        {
            //            cmd.Parameters.AddWithValue("@GrpId", HiddenField_BranchGroup.Value);
            //        }
            //    }
            //    else if (ddlGroup.SelectedItem.Value.ToString() == "3")
            //    {
            //        cmd.Parameters.AddWithValue("@GrpType", ddlgrouptype.SelectedItem.Text.ToString().Trim());
            //        if (RdbGroupAll.Checked)
            //        {
            //            cmd.Parameters.AddWithValue("@GrpId", "ALL");
            //        }
            //        else
            //        {
            //            cmd.Parameters.AddWithValue("@GrpId", HiddenField_Group.Value);
            //        }
            //    }
            //    else if (ddlGroup.SelectedItem.Value.ToString() == "1")
            //    {
            //        cmd.Parameters.AddWithValue("@GrpType", "BRANCH");
            //        cmd.Parameters.AddWithValue("@GrpId", "ALL");
            //    }
            //    if (rdbSegmentAll.Checked)
            //    {
            //        cmd.Parameters.AddWithValue("@Segment", "ALL");
            //    }
            //    else
            //    {
            //        cmd.Parameters.AddWithValue("@Segment", HiddenField_Segment.Value);
            //    }
            //    cmd.Parameters.AddWithValue("@CompanyID", Session["LastCompany"].ToString().Trim());
            //    cmd.Parameters.AddWithValue("@Finyear", Session["LastFinYear"].ToString().Trim());
            //    if (txtHeader.Text.ToString().Trim() == "")
            //    {
            //        cmd.Parameters.AddWithValue("@HeaderId", "NA");
            //    }
            //    else
            //    {
            //        cmd.Parameters.AddWithValue("@HeaderId", txtHeader_hidden.Value.ToString().Trim());
            //    }
            //    if (txtFooter.Text.ToString().Trim() == "")
            //    {
            //        cmd.Parameters.AddWithValue("@FooterId", "NA");
            //    }
            //    else
            //    {
            //        cmd.Parameters.AddWithValue("@FooterId", txtFooter_hidden.Value.ToString().Trim());
            //    }
            //    string SignatureChk = "This is an electronicaly generated statement that requires no Signature. *";
            //    byte[] SignatureinByte;
            //    /////////Signature
            //    if (txtSignature.Text.ToString().Trim() != "")
            //    {
            //        if (oconverter.getSignatureImage(txtSignature_hidden.Value.ToString().Trim(), out SignatureinByte, "NSE") == 1)
            //        {
            //            SignatureChk = "Y";
            //            cmd.Parameters.AddWithValue("@Signature", SignatureinByte);
            //        }
            //        else
            //        {
            //            cmd.Parameters.AddWithValue("@Signature", SqlByte.Null);
            //        }
            //    }
            //    else
            //    {
            //        cmd.Parameters.AddWithValue("@Signature", SqlBytes.Null);
            //    }

            //    SqlDataAdapter da = new SqlDataAdapter();
            //    da.SelectCommand = cmd;
            //    cmd.CommandTimeout = 0;
            //    ds.Reset();
            //    ds.Clear();
            //    da.Fill(ds);
            //    da.Dispose();
            //    FnSPCall(ds, SignatureChk.ToString().Trim());
            //}
        }

        protected void BtnPrint_Click(object sender, EventArgs e)
        {
            Procedure();
        }

        private void FnSPCall(DataSet ds, string SignatureChk)
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                Print(ds, SignatureChk.ToString().Trim());
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "fnNoRecord", "fnNoRecord('No Record Found !!');", true);
            }
        }

        private void Print(DataSet ds, string SignatureChk)
        {
            byte[] logoinByte;

            ds.Tables[1].Columns.Add("Image", System.Type.GetType("System.Byte[]"));
            DataTable dtcmp = oDBEngine.GetDataTable("tbl_master_company", "cmp_id", "cmp_internalid='" + Session["LastCompany"].ToString() + "'");
            string filePath = "";
            filePath = @"..\images\logo_" + dtcmp.Rows[0][0].ToString() + ".bmp";
            if (ChkCompanyLogo.Checked == true)
            {
                if (oconverter.getLogoImage(HttpContext.Current.Server.MapPath(filePath), out logoinByte) != 1)
                {
                    ScriptManager.RegisterStartupScript(HttpContext.Current.Handler as Page, HttpContext.Current.GetType(), "logo", "alert('Logo not Found.');", true);
                }
                else
                {
                    for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                    {
                        ds.Tables[1].Rows[i]["Image"] = logoinByte;
                    }
                }
            }

            ReportDocument report = new ReportDocument();
            //ds.Tables[0].WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\AccountConfirmationSummary.xsd");
            //ds.Tables[1].WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\AccountConfirmationSummaryCompanylogo.xsd");
            //report.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Landscape;

            string tmpPdfPath = string.Empty;
            tmpPdfPath = HttpContext.Current.Server.MapPath("..\\Reports\\AccountConfirmationSummary.rpt");
            report.Load(tmpPdfPath);
            report.SetDataSource(ds.Tables[0]);
            report.Subreports["CompanyDetails"].SetDataSource(ds.Tables[1]);
            report.VerifyDatabase();
            string param = "";
            if (ds.Tables[1].Rows[0]["SebiNo"].ToString().StartsWith("--") == true)
                param = "SEBI Reg No. - " + ds.Tables[1].Rows[0]["SebiNo"].ToString().Split('-')[2];
            if (ds.Tables[1].Rows[0]["SebiNo"].ToString().StartsWith("_") == true)
                param = "FMC Reg No. - " + ds.Tables[1].Rows[0]["SebiNo"].ToString().Split('_')[1];
            if (ds.Tables[1].Rows[0]["SebiNo"].ToString().StartsWith(":") == true)
                param = "";
            report.SetParameterValue("@parameter", (object)param.ToString().Trim());
            report.SetParameterValue("@Header", (object)ds.Tables[2].Rows[0]["Header"].ToString().Trim());
            report.SetParameterValue("@Footer", (object)ds.Tables[3].Rows[0]["Footer"].ToString().Trim());
            report.SetParameterValue("@PrintDate", (object)"Statement Date :" + oconverter.ArrangeDate2(DtPrintDate.Value.ToString()));

            if (ChkBothSidePrint.Checked)
            {
                report.SetParameterValue("@BothSidePrint", (object)"CHK");
            }
            else
            {
                report.SetParameterValue("@BothSidePrint", (object)"UNCHK");
            }
            report.SetParameterValue("@SignatureChk", (object)SignatureChk.ToString().Trim());

            report.SetParameterValue("@CompanyName", (object)ds.Tables[1].Rows[0]["CmpName"].ToString().Trim());
            report.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, HttpContext.Current.Response, true, "Accounts Confirmation Summary Statement With Collaterals");
            report.Dispose();
            GC.Collect();
        }
    }
}