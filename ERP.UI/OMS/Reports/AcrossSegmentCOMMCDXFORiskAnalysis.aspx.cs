using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using BusinessLogicLayer;

namespace ERP.OMS.Reports
{
    public partial class Reports_AcrossSegmentCOMMCDXFORiskAnalysis : System.Web.UI.Page, System.Web.UI.ICallbackEventHandler
    {
        #region Local Variable
        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);MULTI
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        BusinessLogicLayer.Converter oconverter = new BusinessLogicLayer.Converter();
        string data;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (HttpContext.Current.Session["userid"] == null)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
            //this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");
            if (!IsPostBack)
            {
                dtfrom.Date = oDBEngine.GetDate();
                dtto.Date = oDBEngine.GetDate();
                Page.ClientScript.RegisterStartupScript(GetType(), "PageLoad", "<script>Page_Load();</script>");
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
            string str = "";
            string str1 = "";
            if (idlist[0] == "ComboChange")
            {
                //MainAcID = idlist[1];
            }
            else
            {
                string[] cl = idlist[1].Split(',');
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
                            str = "'" + AcVal[0] + "'";
                            str1 = "'" + AcVal[0] + "'" + ";" + val[1];
                        }
                        else
                        {
                            str += ",'" + AcVal[0] + "'";
                            str1 += "," + "'" + AcVal[0] + "'" + ";" + val[1];
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
                else if (idlist[0] == "Segment")
                {
                    data = "Segment~" + str;
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

        protected DataSet BindData()
        {
            DataSet ds = null;
            if (ddlGroup.SelectedItem.Value.ToString().Trim() == "0")/////if branch
            {
                if (rdbranchclientSelected.Checked == true && HiddenField_Branch.Value.ToString().Trim() == "")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "NORECORD1", "NORECORD(1);", true);
                }
                else
                {
                    ds = runProcedure();
                }
            }
            if (ddlGroup.SelectedItem.Value.ToString().Trim() == "1")/////if group
            {
                if (ddlgrouptype.SelectedItem.Value == "0")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "NORECORD1", "NORECORD(1);", true);
                }
                else
                {
                    if (rdddlgrouptypeSelected.Checked == true && HiddenField_Group.Value.ToString().Trim() == "")
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "NORECORD1", "NORECORD(1);", true);
                    }
                    else
                    {
                        ds = runProcedure();
                    }
                }
            }
            if (ddlGroup.SelectedItem.Value.ToString().Trim() == "2")/////if client wise
            {
                if (rdbranchclientSelected.Checked == true && HiddenField_Client.Value.ToString().Trim() == "")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "NORECORD1", "NORECORD(1);", true);
                }
                else
                {
                    ds = runProcedure();
                }
            }
            return ds;
        }

        protected DataSet runProcedure()
        {
            string[] InputName = new string[8];
            string[] InputType = new string[8];
            string[] InputValue = new string[8];

            InputName[0] = "CompanyID";
            InputName[1] = "FinYear";
            InputName[2] = "TDate";
            InputName[3] = "TDateTo";
            InputName[4] = "GrpBy";
            InputName[5] = "GrpByID";
            InputName[6] = "GrpType";
            InputName[7] = "Segments";

            InputType[0] = "V";
            InputType[1] = "V";
            InputType[2] = "D";
            InputType[3] = "D";
            InputType[4] = "V";
            InputType[5] = "V";
            InputType[6] = "V";
            InputType[7] = "V";

            InputValue[0] = Session["LastCompany"].ToString();
            InputValue[1] = Session["LastFinYear"].ToString();
            InputValue[2] = dtfrom.Date.ToString();
            InputValue[3] = dtto.Date.ToString();
            if (ddlGroup.SelectedItem.Value.ToString() == "0")/////option Branch 
            {
                InputValue[4] = "BRANCH";
                if (rdbranchclientAll.Checked)
                {
                    InputValue[5] = "ALL";
                    InputValue[6] = "";
                }
                else
                {
                    InputValue[5] = HiddenField_Branch.Value;
                    InputValue[6] = "";
                }
            }
            else if (ddlGroup.SelectedItem.Value.ToString() == "1")/////option Group
            {
                InputValue[4] = "GROUP";
                string groupType = ddlgrouptype.SelectedItem.Text.ToString().Trim();
                if (rdddlgrouptypeAll.Checked)
                {
                    InputValue[5] = "ALL";
                    InputValue[6] = groupType;
                }
                else
                {
                    InputValue[5] = HiddenField_Group.Value;
                    InputValue[6] = groupType;
                }
            }
            else if (ddlGroup.SelectedItem.Value.ToString() == "2")/////option Clients
            {
                InputValue[4] = "CLIENT";
                if (rdbranchclientAll.Checked)
                {
                    InputValue[5] = "ALL";
                    InputValue[6] = "";
                }
                else
                {
                    InputValue[5] = HiddenField_Client.Value.Replace("'", "");
                    InputValue[6] = "";
                }
            }
            else
            {
                InputValue[4] = "BRANCH";
                InputValue[5] = "ALL";
                InputValue[6] = "";
            }
            if (rdbSegmentAll.Checked == true)
                InputValue[7] = "ALL";
            else
                InputValue[7] = HiddenField_Segment.Value.Trim();

            DataSet dsData = SQLProcedures.SelectProcedureArrDS("Report_AcrossSegmentCOMMCDXFORiskAnalysis", InputName, InputType, InputValue);
            return dsData;
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            DataSet resultDS = null;
            if (dtfrom.Value == null)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Validate", "Validate(1);", true);
            }
            else if (dtto.Value == null)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Validate", "Validate(2);", true);
            }
            else
            {
                resultDS = BindData();
                ExportToExcel_Generic(resultDS, "2007");
            }
        }

        protected void ExportToExcel_Generic(DataSet excelDS, string ExcelVersion)
        {
            if (excelDS.Tables.Count > 1)
            {
                if ((excelDS.Tables[0].Rows.Count > 1) && (!excelDS.Tables[0].Rows[0]["ClientName"].ToString().Contains("Grand Total")))
                {
                    GenericExcelExport oGenericExcelExport = new GenericExcelExport();
                    string strDownloadFileName = "";
                    string exlDateTime = oDBEngine.GetDate(113).ToString();
                    string exlTime = exlDateTime.Replace(":", "");
                    exlTime = exlTime.Replace(" ", "");
                    string FileName = "RiskAnalysis_" + exlTime;
                    strDownloadFileName = "~/Documents/";
                    string strTimePeriod = null;
                    strTimePeriod = "For The Date Between " + oconverter.ArrangeDate2(dtfrom.Value.ToString()) + "  To " + oconverter.ArrangeDate2(dtto.Value.ToString());
                    string searchCriteria = null;
                    searchCriteria = "Search By ";
                    if (rdbSegmentAll.Checked == true)
                        searchCriteria += "All Segment, ";
                    else
                        searchCriteria += "Selected Segment, ";
                    if (ddlGroup.SelectedValue == "1")
                    {
                        if (rdddlgrouptypeAll.Checked == true)
                            searchCriteria += "All " + ddlGroup.SelectedItem.Text.ToString().Trim();
                        else if (rdddlgrouptypeSelected.Checked == true)
                            searchCriteria += "Selected " + ddlGroup.SelectedItem.Text.ToString().Trim();
                    }
                    else
                    {
                        if (rdbranchclientAll.Checked == true)
                            searchCriteria += "All " + ddlGroup.SelectedItem.Text.ToString().Trim();
                        else
                            searchCriteria += "Selected " + ddlGroup.SelectedItem.Text.ToString().Trim();
                    }
                    string[] strHead = new string[5];
                    strHead[0] = exlDateTime;
                    strHead[1] = searchCriteria;
                    strHead[2] = strTimePeriod;
                    strHead[3] = "Across Segment COMM/CDX/FO Risk Analysis";
                    DataTable dtcompname = oDBEngine.GetDataTable(" tbl_master_company  ", "cmp_Name", "cmp_internalid='" + HttpContext.Current.Session["LastCompany"] + "' ");
                    strHead[4] = "Company Name " + dtcompname.Rows[0]["cmp_Name"].ToString();

                    string[] ColumnType = (excelDS.Tables[1].Rows[0][0].ToString()).Split('~');
                    string[] ColumnSize = (excelDS.Tables[1].Rows[0][1].ToString()).Split('~');
                    string[] ColumnWidthSize = (excelDS.Tables[1].Rows[0][2].ToString()).Split('~');


                    //=====Static Columns=====
                    //string[] ColumnType =      { "V", "V", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N" };
                    //string[] ColumnSize =      { "140", "20", "18,2", "20,2", "20,2", "20,2", "20,2", "18,2", "18,2", "18,2", "18,2", "18,2", "20,2", "28,2" };
                    //string[] ColumnWidthSize = { "30", "8", "10", "20", "15", "15", "15", "15", "15", "15", "15", "15", "15", "15" };

                    oGenericExcelExport.ExportToExcel(ColumnType, ColumnSize, ColumnWidthSize, excelDS.Tables[0], Server.MapPath(strDownloadFileName), ExcelVersion, FileName, strHead, null);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "NORECORD1", "NORECORD(2);", true);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "NORECORD2", "NORECORD(2);", true);
            }
        }
    }
}