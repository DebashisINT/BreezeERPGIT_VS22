using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogicLayer;

namespace ERP.OMS.Reports
{


    public partial class Reports_PositionFileProtectorCM : System.Web.UI.Page, System.Web.UI.ICallbackEventHandler
    {
        BusinessLogicLayer.Reports rep = new BusinessLogicLayer.Reports();
        GenericStoreProcedure objGenericStoredProcedure;
        BusinessLogicLayer.GenericMethod objGenericMethod = new BusinessLogicLayer.GenericMethod();
        BusinessLogicLayer.Converter oconverter = new BusinessLogicLayer.Converter();
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        DataSet ds = new DataSet();
        DataTable dtgroupcontactid = new DataTable();
        DataTable Distinctclient = new DataTable();
        DataTable dtsp = new DataTable();
        string path;
        string savefilepath;
        string filespath;
        string data;
        protected void Page_Load(object sender, EventArgs e)
        {

            if (HttpContext.Current.Session["userid"] == null)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
            if (!IsPostBack)
            {


                Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script language='javascript'>Page_Load();</script>");


                dtDate.EditFormatString = oconverter.GetDateFormat("Date");
                dtDate.Value = System.DateTime.Today;
            }
            //_____For performing operation without refreshing page___//
            String cbReference = Page.ClientScript.GetCallbackEventReference(this, "arg", "ReceiveServerData", "context");
            String callbackScript = "function CallServer(arg, context){ " + cbReference + ";}";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "CallServer", callbackScript, true);
            //___________-end here___//



        }
        protected void Page_PreRender(object sender, EventArgs e)
        {
            DataTable dtt1 = new DataTable();
            string s = Convert.ToString(Request.Form["__EVENTTARGET"]);
            if (IsPostBack && hdnReport.Value == "y")
            {
                dtt1 = FetchKraData();
                hdnReport.Value = "n";
            }
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

        }
        string ICallbackEventHandler.GetCallbackResult()
        {
            return data;
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
        private DataTable FetchKraData()
        {
            btnShow.Text = "Please Wait.....";
            ScriptManager.RegisterStartupScript(this, GetType(), "shhh", "ShowHideLoader('1');", true);
            ScriptManager.RegisterStartupScript(this, GetType(), "ShowHideDivs", "ShowHideDiv('a');", true);
            objGenericStoredProcedure = new GenericStoreProcedure();
            //objGenericMethod = new GenericMethod();
            string[] strSpParam = new string[4];
            // strSpParam[0] = "Mode|" + GenericStoreProcedure.ParamDBType.Int + "|20|" + Convert.ToInt32(ViewState["mode"]) + "|" + GenericStoreProcedure.ParamType.ExParam;
            DataTable dtKRADetail = new DataTable();

            string[] Ids = hdnIds.Value.Split(',');
            if (ddlGroup.SelectedValue == "0")
            {
                // Branch
                if (rdbranchAll.Checked == true)
                {
                    strSpParam[0] = "BranchIds|" + GenericStoreProcedure.ParamDBType.Varchar + "|1000|" + "0" + "|" + GenericStoreProcedure.ParamType.ExParam;
                }
                else
                {
                    strSpParam[0] = "BranchIds|" + GenericStoreProcedure.ParamDBType.Varchar + "|1000|" + hdnIds.Value + "|" + GenericStoreProcedure.ParamType.ExParam;
                }
                strSpParam[1] = "GroupIds|" + GenericStoreProcedure.ParamDBType.Varchar + "|1000|" + "0" + "|" + GenericStoreProcedure.ParamType.ExParam;
            }
            else if (ddlGroup.SelectedValue == "1")
            {
                strSpParam[0] = "BranchIds|" + GenericStoreProcedure.ParamDBType.Varchar + "|1000|" + "0" + "|" + GenericStoreProcedure.ParamType.ExParam;
                // Group
                if (rdddlgrouptypeAll.Checked == true)
                {
                    strSpParam[1] = "GroupIds|" + GenericStoreProcedure.ParamDBType.Varchar + "|1000|" + "0" + "|" + GenericStoreProcedure.ParamType.ExParam;
                }
                else
                {
                    strSpParam[1] = "GroupIds|" + GenericStoreProcedure.ParamDBType.Varchar + "|1000|" + hdnIds.Value + "|" + GenericStoreProcedure.ParamType.ExParam;
                }
            }
            if (hdnClientIds.Value != "'0'")
            {
                strSpParam[2] = "ClientIds|" + GenericStoreProcedure.ParamDBType.Varchar + "|1000|" + hdnClientIds.Value + "|" + GenericStoreProcedure.ParamType.ExParam;
            }
            else
            {
                strSpParam[2] = "ClientIds|" + GenericStoreProcedure.ParamDBType.Varchar + "|1000|" + "0" + "|" + GenericStoreProcedure.ParamType.ExParam;
            }
            strSpParam[3] = "Type|" + GenericStoreProcedure.ParamDBType.Int + "|1000|" + rlstClientType.SelectedValue + "|" + GenericStoreProcedure.ParamType.ExParam;
            dtKRADetail = objGenericStoredProcedure.Procedure_DataTable(strSpParam, "Fetch_KraDetail");
            if (dtKRADetail != null && dtKRADetail.Rows.Count > 0)
            {
                // dtKRADetail.Columns["RegistrationDate"].DataType = typeof(string);
                for (int i = 0; i < dtKRADetail.Rows.Count; i++)
                {
                    dtKRADetail.Rows[i]["Registration Date"] = GetDateFormat(Convert.ToString(dtKRADetail.Rows[i]["Registration Date"]));
                    dtKRADetail.Rows[i]["StatusDate"] = GetDateFormat(Convert.ToString(dtKRADetail.Rows[i]["StatusDate"]));
                    dtKRADetail.Rows[i]["New KYC Date"] = GetDateFormat(Convert.ToString(dtKRADetail.Rows[i]["New KYC Date"]));
                    dtKRADetail.Rows[i]["Modification KYC Date"] = GetDateFormat(Convert.ToString(dtKRADetail.Rows[i]["Modification KYC Date"]));
                }
                dtKRADetail.AcceptChanges();
                pnlData.Visible = true;
                pnlEmptyMessage.Visible = false;
                pnlExport.Visible = true;
            }
            else
            {
                pnlExport.Visible = false;
                pnlData.Visible = false;
                pnlEmptyMessage.Visible = true;
            }

            rptKraDetail.DataSource = null;
            rptKraDetail.DataSource = dtKRADetail;

            // ViewState["dtKraDetail"] = dtKRADetail;
            rptKraDetail.DataBind();
            btnShow.Text = "Show";
            ScriptManager.RegisterStartupScript(this, GetType(), "shhjjhh", "ShowHideLoader('0');", true);
            ScriptManager.RegisterStartupScript(this, GetType(), "showhideButton", "ShowHideButton('2');", true);
            return dtKRADetail;
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
        void fn_Client()
        {
            string Clients;
            if (rdbClientALL.Checked)//////////////////ALL CLIENT CHECK
            {
                Clients = null;
                if (ddlGroup.SelectedItem.Value.ToString() == "1")
                {
                    if (rdddlgrouptypeAll.Checked)
                    {
                        if (ddlgrouptype.SelectedItem.Value.ToString() != "0")
                        {
                            DataTable dtclient = new DataTable();
                            dtclient = oDBEngine.GetDataTable("tbl_master_contact", "cnt_internalID", " cnt_internalId like 'CL%' and cnt_internalid in(select grp_contactid from tbl_trans_group where  grp_groupmaster in(select gpm_id from tbl_master_groupmaster where gpm_type='" + ddlgrouptype.SelectedItem.Text.ToString() + "'))");
                            if (dtclient.Rows.Count > 0)
                            {
                                for (int i = 0; i < dtclient.Rows.Count; i++)
                                {
                                    if (Clients == null)
                                        Clients = "'" + dtclient.Rows[i][0].ToString() + "'";
                                    else
                                        Clients += "," + "'" + dtclient.Rows[i][0].ToString() + "'";
                                }

                            }

                        }
                    }
                    else
                    {
                        DataTable dtclient = new DataTable();
                        dtclient = oDBEngine.GetDataTable("tbl_master_contact", "cnt_internalID", " cnt_internalId like 'CL%' and cnt_internalid in(select grp_contactid from tbl_trans_group where grp_groupmaster in(" + HiddenField_Group.Value.ToString().Trim() + "))");
                        if (dtclient.Rows.Count > 0)
                        {
                            for (int i = 0; i < dtclient.Rows.Count; i++)
                            {
                                if (Clients == null)
                                    Clients = "'" + dtclient.Rows[i][0].ToString() + "'";
                                else
                                    Clients += "," + "'" + dtclient.Rows[i][0].ToString() + "'";
                            }

                        }
                    }
                }
                else
                {
                    if (rdbranchAll.Checked)
                    {
                        DataTable dtclient = new DataTable();
                        dtclient = oDBEngine.GetDataTable("tbl_master_contact", "cnt_internalID", " cnt_internalId like 'CL%'  and cnt_branchid in(" + Session["userbranchHierarchy"].ToString() + ")");
                        if (dtclient.Rows.Count > 0)
                        {
                            for (int i = 0; i < dtclient.Rows.Count; i++)
                            {
                                if (Clients == null)
                                    Clients = "'" + dtclient.Rows[i][0].ToString() + "'";
                                else
                                    Clients += "," + "'" + dtclient.Rows[i][0].ToString() + "'";
                            }

                        }
                    }
                    else
                    {
                        DataTable dtclient = new DataTable();
                        dtclient = oDBEngine.GetDataTable("tbl_master_contact", "cnt_internalID", " cnt_internalId like 'CL%' and cnt_branchid in(" + HiddenField_Branch.Value.ToString().Trim() + ")");
                        if (dtclient.Rows.Count > 0)
                        {
                            for (int i = 0; i < dtclient.Rows.Count; i++)
                            {
                                if (Clients == null)
                                    Clients = "'" + dtclient.Rows[i][0].ToString() + "'";
                                else
                                    Clients += "," + "'" + dtclient.Rows[i][0].ToString() + "'";
                            }

                        }
                    }
                }
                HiddenField_Client.Value = Clients;
            }

        }



        void procedure()
        {

            fn_Client();
            ds = rep.ExportPosition_CM_New(dtDate.Value.ToString(), Session["usersegid"].ToString(),
                Session["LastCompany"].ToString(), HttpContext.Current.Session["ExchangeSegmentID"].ToString(),
                HiddenField_Client.Value.ToString().Trim());
            ViewState["dataset"] = ds;

            if (ds.Tables[1].Rows.Count == 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "NORECORD3", "NORECORD(1);", true);
            }
            else
            {
                CreatetxtFile();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Display", "Display();", true);
            }

        }
        void CreatetxtFile()
        {
            ds = (DataSet)ViewState["dataset"];
            if (ds.Tables[1].Rows.Count > 0)
            {
                savefilepath = @"ExportFiles/POSITION/" + ds.Tables[0].Rows[0]["filename1"].ToString(); ///////////FILE SAVE INTO DATABASE
                filespath = Server.MapPath(@"../ExportFiles/POSITION/") + ds.Tables[0].Rows[0]["filename1"].ToString() + ".txt";///////////FILE SAVE INTO FOLDER

                if (!Directory.Exists(Server.MapPath(@"../ExportFiles")))
                    Directory.CreateDirectory(Server.MapPath(@"../ExportFiles"));

                if (!Directory.Exists(Server.MapPath(@"../ExportFiles/POSITION")))
                    Directory.CreateDirectory(Server.MapPath(@"../ExportFiles/POSITION"));

                ViewState["filespath"] = savefilepath.ToString().Trim();
                using (StreamWriter sw = new StreamWriter(filespath, false))
                {
                    int colCount = ds.Tables[1].Columns.Count;
                    foreach (DataRow dr in ds.Tables[1].Rows)
                    {

                        for (int j = 0; j < colCount; j++)
                        {

                            if (!Convert.IsDBNull(dr[j]))
                            {
                                if (j == colCount - 1)
                                {
                                    sw.Write(dr[j]);
                                }
                                else
                                {
                                    sw.Write(dr[j] + ",");
                                }

                            }
                            else
                            {
                                sw.Write(",");
                            }

                        }

                        sw.Write(sw.NewLine);


                    }
                }

                rep.sp_Insert_ExportFiles(Session["usersegid"].ToString(), "CM Position txt", ds.Tables[0].Rows[0]["filename1"].ToString(),
                    HttpContext.Current.Session["userid"].ToString(), ds.Tables[0].Rows[0]["rowno"].ToString(), savefilepath);
                filegenerate();

            }

        }
        public string GetDateFormat(object dates)
        {
            //string format = "M d h:mm yy";

            try
            {
                string format = "dd-MMM-yyyy";
                DateTime now = DateTime.Now;
                DateTime dt = Convert.ToDateTime(dates);
                string s4 = dt.ToString(format);
                return s4;
            }
            catch
            {
                return Convert.ToString(dates);
            }
        }
        protected void btnGenerate_Click(object sender, EventArgs e)
        {
            // FetchKraData();
        }
        void filegenerate()
        {
            string filename = Server.MapPath("..\\" + ViewState["filespath"].ToString()) + ".txt";
            FileInfo fileInfo = new FileInfo(filename);

            if (fileInfo.Exists)
            {

                Response.Clear();
                Response.AddHeader("Content-Disposition", "attachment;filename=" + fileInfo.Name);
                Response.AddHeader("Content-Length", fileInfo.Length.ToString());
                Response.ContentType = "application/octet-stream";
                Response.Flush();
                Response.WriteFile(fileInfo.FullName);
                Response.End();

            }

        }
        private void SummaryExcelExport()
        {
            double totalInterest = 0;
            // DataSet ds = (DataSet)ViewState["dataset"];
            ExcelFile objExcel = new ExcelFile();
            DataTable dtExport = new DataTable();
            string searchCriteria = null;
            Converter oconverter = new Converter();
            GenericMethod oGenericMethod = new GenericMethod();

            // searchCriteria = "From " + oconverter.ArrangeDate2(dtFrom.Value.ToString()) + "To " + oconverter.ArrangeDate2(dtTo.Value.ToString()) + " Register Of Security   ";
            searchCriteria = "From ";
            dtExport = FetchKraData();
            // dtExport = ds.Tables[0].Copy();
            /* if (ViewState["dtKraDetail"] != null)
             {
                 dtExport = (ViewState["dtKraDetail"] as DataTable);
             } */
            //foreach (DataRow dr in dtExport.Rows)
            //{
            //   // totalInterest += Convert.ToDouble(dr["Interest"]);
            //}

            /* dtExport.Columns.RemoveAt(0);
             dtExport.Columns.RemoveAt(2);
             dtExport.Columns.RemoveAt(5);
             dtExport.Columns.RemoveAt(13); */

            dtExport.Columns.Remove("Source");
            dtExport.Columns.Remove("Source1");
            dtExport.Columns.Remove("RegisteredAgency");
            dtExport.Columns.Remove("ClientId");
            //dtExport.Columns.RemoveAt(3);

            // dtExport.Columns.RemoveAt(9);
            //dtExport.Rows.Add("Total", null, null, null, totalInterest.ToString(), null, null, null, null).AcceptChanges();
            GenericExcelExport oGenericExcelExport = new GenericExcelExport();
            string strDownloadFileName = "";
            // string exlDateTime = System.DateTime.Now.ToShortDateString();
            string exlDateTime = oGenericMethod.GetDate(113).ToString();
            string exlTime = exlDateTime.Replace(":", "");
            exlTime = exlTime.Replace(" ", "");
            string FileName = "KRADetailsReport" + "_" + exlTime;
            strDownloadFileName = "~/Documents/";
            oGenericMethod = new GenericMethod();
            //  DataTable dtcompany = oGenericMethod.GetDataTable("select ltrim(rtrim(cmp_Name)) as company from tbl_master_company where cmp_internalid ='" + Session["LastCompany"].ToString() + "'");
            string[] strHead = new string[1];
            strHead[0] = "                     Kra Details Report";
            // strHead[1] = searchCriteria;
            // strHead[0] = " to ";
            // strHead[2] = "Kra Details Report";
            string ExcelVersion = "2007";                                                                 //Lots

            string[] ColumnType = { "V", "V", "V", "V", "V", "V", "V", "V", "V", "V", "V", "V" };
            string[] ColumnSize = { "100", "100", "90", "100", "100", "100", "100", "100", "100", "100", "100", "100" };
            string[] ColumnWidthSize = { "20", "20", "20", "15", "15", "15", "15", "10", "20", "20", "20", "20" };
            oGenericExcelExport.ExportToExcel(ColumnType, ColumnSize, ColumnWidthSize, dtExport, Server.MapPath(strDownloadFileName), ExcelVersion, FileName, strHead, null);

        }
        protected void btnExport_Click(object sender, EventArgs e)
        {
            SummaryExcelExport();
        }
        protected void btnShow_Click(object sender, EventArgs e)
        {
            // FetchKraData();
        }
        protected void rlst_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        protected void rptKraDetail_PageIndexChanged(object sender, EventArgs e)
        {

        }
        protected void rptKraDetail_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            hdnReport.Value = "y";
            rptKraDetail.PageIndex = e.NewPageIndex;
        }
    }
}