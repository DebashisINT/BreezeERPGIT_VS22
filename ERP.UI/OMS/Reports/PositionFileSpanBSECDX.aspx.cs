using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
//using BusinessLogicLayer;

namespace ERP.OMS.Reports
{
    public partial class Reports_PositionFileSpanBSECDX : System.Web.UI.Page, System.Web.UI.ICallbackEventHandler
    {
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
        BusinessLogicLayer.Reports Reports = new BusinessLogicLayer.Reports();

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
            using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]))
            {

                //SqlCommand cmd = new SqlCommand();
                //cmd.Connection = con;
                //cmd.CommandText = "[ExportPosition_BSECDX]";
                //cmd.CommandType = CommandType.StoredProcedure;

                //cmd.Parameters.AddWithValue("@date", dtDate.Value);
                //cmd.Parameters.AddWithValue("@segment", Convert.ToInt32(Session["usersegid"].ToString()));
                //cmd.Parameters.AddWithValue("@companyid", Session["LastCompany"].ToString());
                //cmd.Parameters.AddWithValue("@MasterSegment", HttpContext.Current.Session["ExchangeSegmentID"].ToString());
                //cmd.Parameters.AddWithValue("@ClientsID", HiddenField_Client.Value.ToString().Trim());

                //SqlDataAdapter da = new SqlDataAdapter();
                //da.SelectCommand = cmd;
                //cmd.CommandTimeout = 0;
                //ds.Reset();
                //da.Fill(ds);
                //da.Dispose();
                //ViewState["dataset"] = ds;

                ds.Reset();
                ds = Reports.ExportPosition_BSECDX(Convert.ToString(dtDate.Value), Session["usersegid"].ToString(), Session["LastCompany"].ToString(),
                    HttpContext.Current.Session["ExchangeSegmentID"].ToString(), HiddenField_Client.Value.ToString().Trim());

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
                //String con = ConfigurationSettings.AppSettings["DBConnectionDefault"];
                //SqlConnection lcon = new SqlConnection(con);
                //lcon.Open();
                //SqlCommand com = new SqlCommand("[sp_Insert_ExportFiles]", lcon);
                //com.CommandType = CommandType.StoredProcedure;

                //com.Parameters.AddWithValue("@segid", Convert.ToInt32(Session["usersegid"].ToString()));
                //com.Parameters.AddWithValue("@file_type", "FO Position txt");
                //com.Parameters.AddWithValue("@file_name", ds.Tables[0].Rows[0]["filename1"].ToString());
                //com.Parameters.AddWithValue("@userid", HttpContext.Current.Session["userid"]);
                //com.Parameters.AddWithValue("@batch_number", ds.Tables[0].Rows[0]["rowno"].ToString());
                //com.Parameters.AddWithValue("@file_path", savefilepath);
                //com.CommandTimeout = 0;
                //com.ExecuteNonQuery();
                //lcon.Close();
                Reports.sp_Insert_ExportFiles(Session["usersegid"].ToString(), ds.Tables[0].Rows[0]["filename1"].ToString(),
                   Convert.ToString(HttpContext.Current.Session["userid"]), ds.Tables[0].Rows[0]["rowno"].ToString(), savefilepath);

                filegenerate();

            }

        }

        protected void btnGenerate_Click(object sender, EventArgs e)
        {
            procedure();
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
    }
}