using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogicLayer;

namespace ERP.OMS.Reports
{
    public partial class Reports_CFStatement : System.Web.UI.Page, System.Web.UI.ICallbackEventHandler
    {
        BusinessLogicLayer.Converter oconverter = new BusinessLogicLayer.Converter();
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        DataSet ds = new DataSet();
        ExcelFile objExcel = new ExcelFile();
        DataTable dtExport = new DataTable();
        string data;

        BusinessLogicLayer.Reports ObjReport = new BusinessLogicLayer.Reports();


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

                Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script language='javascript'>Page_Load('" + HttpContext.Current.Session["ExchangeSegmentID"].ToString() + "');</script>");
                date();

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
            else if (idlist[0] == "Product")
            {
                data = "Product~" + str;
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

        }
        void date()
        {
            DtFrom.EditFormatString = oconverter.GetDateFormat("Date");
            DtFrom.Value = Convert.ToDateTime(oDBEngine.GetDate().ToShortDateString());
            DtTo.EditFormatString = oconverter.GetDateFormat("Date");
            DtTo.Value = Convert.ToDateTime(oDBEngine.GetDate().ToShortDateString());

        }
        protected void btnhide_Click(object sender, EventArgs e)
        {
            if (ddlGroup.SelectedItem.Value.ToString() == "1")
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
        void procedure()
        {

            /* For Tier Structure ----------------
        
            using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]))
            {

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                if (DdlRptType.SelectedItem.Value.ToString().Trim() == "1")//////CF STATEMENT
                {
                    cmd.CommandText = "[CFChargeStatement]";
                }
                if (DdlRptType.SelectedItem.Value.ToString().Trim() == "2")//////MTM STATEMENT
                {
                    cmd.CommandText = "[MTMChargeStatement]";
                }
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@fromdate", DtFrom.Value);
                cmd.Parameters.AddWithValue("@todate", DtTo.Value);
                if (rdbClientALL.Checked)
                {
                    cmd.Parameters.AddWithValue("@ClientId", "ALL");
                }
                else
                {
                    cmd.Parameters.AddWithValue("@ClientId", HiddenField_Client.Value);
                }
                if (rdbAssetAll.Checked)
                {
                    cmd.Parameters.AddWithValue("@Scrip", "ALL");
                }
                else
                {
                    cmd.Parameters.AddWithValue("@Scrip", HiddenField_Asset.Value);
                }
                cmd.Parameters.AddWithValue("@Segment", Convert.ToInt32(Session["usersegid"].ToString()));
                cmd.Parameters.AddWithValue("@companyid", Session["LastCompany"].ToString());
                if (ddlGroup.SelectedItem.Value.ToString() == "0")
                {
                    cmd.Parameters.AddWithValue("@GrpType", "BRANCH");
                    if (rdbranchAll.Checked)
                    {
                        cmd.Parameters.AddWithValue("@GrpId", "ALL");
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@GrpId", HiddenField_Branch.Value);
                    }
                }
                else if (ddlGroup.SelectedItem.Value.ToString() == "2")
                {
                    cmd.Parameters.AddWithValue("@GrpType", "BRANCHGROUP");
                    if (rdbranchAll.Checked)
                    {
                        cmd.Parameters.AddWithValue("@GrpId", "ALL");
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@GrpId", HiddenField_BranchGroup.Value);
                    }
                }
                else
                {
                    cmd.Parameters.AddWithValue("@GrpType", ddlgrouptype.SelectedItem.Text.ToString().Trim());
                    if (rdddlgrouptypeAll.Checked)
                    {
                        cmd.Parameters.AddWithValue("@GrpId", "ALL");
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@GrpId", HiddenField_Group.Value);
                    }
                }
                cmd.Parameters.AddWithValue("@BranchHierchy", Session["userbranchHierarchy"].ToString());
                if (DdlRptType.SelectedItem.Value.ToString().Trim() == "1")//////CF STATEMENT
                {
                    cmd.Parameters.AddWithValue("@RptView", DLLCFRptView.SelectedItem.Value.ToString().Trim());
                }
                if (DdlRptType.SelectedItem.Value.ToString().Trim() == "2")//////MTM STATEMENT
                {
                    cmd.Parameters.AddWithValue("@RptView", DLLMTMRptView.SelectedItem.Value.ToString().Trim());
                }

         
           
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                cmd.CommandTimeout = 0;
                ds.Reset();
                ds.Clear();
                da.Fill(ds);
                da.Dispose();
                ViewState["dataset"] = ds;

            }


            */


            //-------------------------------------------------------------------------------------------------------------

            string vSPName = "";


            if (DdlRptType.SelectedItem.Value.ToString().Trim() == "1")//////CF STATEMENT
            {
                vSPName = "[CFChargeStatement]";
            }
            if (DdlRptType.SelectedItem.Value.ToString().Trim() == "2")//////MTM STATEMENT
            {
                vSPName = "[MTMChargeStatement]";
            }

            string ClientId, Scrip, GrpType, GrpId;
            string RptView = "";

            if (rdbClientALL.Checked)
            {
                ClientId = "ALL";
            }
            else
            {
                ClientId = HiddenField_Client.Value;
            }
            if (rdbAssetAll.Checked)
            {
                Scrip = "ALL";
            }
            else
            {
                Scrip = HiddenField_Asset.Value;
            }

            if (ddlGroup.SelectedItem.Value.ToString() == "0")
            {
                GrpType = "BRANCH";
                if (rdbranchAll.Checked)
                {
                    GrpId = "ALL";
                }
                else
                {
                    GrpId = HiddenField_Branch.Value;
                }
            }
            else if (ddlGroup.SelectedItem.Value.ToString() == "2")
            {
                GrpType = "BRANCHGROUP";
                if (rdbranchAll.Checked)
                {
                    GrpId = "ALL";
                }
                else
                {
                    GrpId = HiddenField_BranchGroup.Value;
                }
            }
            else
            {
                GrpType = ddlgrouptype.SelectedItem.Text.ToString().Trim();
                if (rdddlgrouptypeAll.Checked)
                {
                    GrpId = "ALL";
                }
                else
                {
                    GrpId = HiddenField_Group.Value;
                }
            }

            if (DdlRptType.SelectedItem.Value.ToString().Trim() == "1")//////CF STATEMENT
            {
                RptView = DLLCFRptView.SelectedItem.Value.ToString().Trim();
            }
            if (DdlRptType.SelectedItem.Value.ToString().Trim() == "2")//////MTM STATEMENT
            {
                RptView = DLLMTMRptView.SelectedItem.Value.ToString().Trim();
            }



            ds = ObjReport.CFOrMTMChargeStatement(DtFrom.Value.ToString(), DtTo.Value.ToString(), ClientId, Scrip, Session["usersegid"].ToString(),
                Session["LastCompany"].ToString(), GrpType, GrpId, Session["userbranchHierarchy"].ToString(), RptView, vSPName);

            ViewState["dataset"] = ds;

            //--------------------------------------------------------------------------------------------------------------


        }
        protected void btnExcel_Click(object sender, EventArgs e)
        {
            procedure();
            ds = (DataSet)ViewState["dataset"];
            if (ds.Tables[0].Rows.Count > 0)
            {
                export(ds);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "fnNoRecord", "fnNoRecord('1');", true);
            }

        }
        void export(DataSet ds)
        {
            DataTable dtExport = ds.Tables[0].Copy();

            string str = null;
            if (ddlGroup.SelectedItem.Value.ToString() == "1")
            {
                str = "[" + ddlgrouptype.SelectedItem.Text.ToString().Trim() + "]";
            }
            str = ddlGroup.SelectedItem.Text.ToString().Trim() + " Wise " + str + "Report Period " + oconverter.ArrangeDate2(DtFrom.Value.ToString()) + " - " + oconverter.ArrangeDate2(DtTo.Value.ToString());
            if (DdlRptType.SelectedItem.Value.ToString().Trim() == "1")//////CF STATEMENT
            {
                str = str + " Report View : " + DLLCFRptView.SelectedItem.Text.ToString().Trim();
            }
            if (DdlRptType.SelectedItem.Value.ToString().Trim() == "2")//////MTM STATEMENT
            {
                str = str + " Report View : " + DLLMTMRptView.SelectedItem.Text.ToString().Trim();
            }




            DataTable dtReportHeader = new DataTable();
            DataTable dtReportFooter = new DataTable();

            DataTable CompanyName = oDBEngine.GetDataTable("tbl_master_company", "cmp_Name", " cmp_internalId='" + Session["LastCompany"].ToString() + "'");
            dtReportHeader.Columns.Add(new DataColumn("Header", typeof(String))); //0
            DataRow HeaderRow = dtReportHeader.NewRow();
            HeaderRow[0] = CompanyName.Rows[0][0].ToString();
            dtReportHeader.Rows.Add(HeaderRow);
            DataRow DrRowR1 = dtReportHeader.NewRow();
            DrRowR1[0] = str.ToString().Trim();
            dtReportHeader.Rows.Add(DrRowR1);


            DataRow HeaderRow1 = dtReportHeader.NewRow();
            dtReportHeader.Rows.Add(HeaderRow1);
            DataRow HeaderRow2 = dtReportHeader.NewRow();
            dtReportHeader.Rows.Add(HeaderRow2);

            dtReportFooter.Columns.Add(new DataColumn("Footer", typeof(String))); //0
            DataRow FooterRow1 = dtReportFooter.NewRow();
            dtReportFooter.Rows.Add(FooterRow1);
            DataRow FooterRow2 = dtReportFooter.NewRow();
            dtReportFooter.Rows.Add(FooterRow2);
            DataRow FooterRow = dtReportFooter.NewRow();
            FooterRow[0] = "* * *  End Of Report * * *   ";
            dtReportFooter.Rows.Add(FooterRow);

            objExcel.ExportToExcelforExcel(dtExport, "CF Charge Statement", "Client Total", dtReportHeader, dtReportFooter);

        }
    }
}
