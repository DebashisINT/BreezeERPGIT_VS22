using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using BusinessLogicLayer;
namespace ERP.OMS.Management.DailyTask
{
    public partial class management_DailyTask_export_routine : ERP.OMS.ViewState_class.VSPage
    {
        //SqlConnection con = new SqlConnection(ConfigurationSettings.AppSettings["DBConnectionDefault"]);

        SqlConnection con;
        ExportRoutines oExportRoutines = new ExportRoutines();
        BusinessLogicLayer.Converter objConverter = new BusinessLogicLayer.Converter();
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        DataSet ds = new DataSet();
        public int batchno = 0;
        public int flag = 1;

        protected void Page_PreInit(object sender, EventArgs e)
        {
            con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!ClientScript.IsStartupScriptRegistered("Today"))
                ClientScript.RegisterStartupScript(typeof(Page), "Today", "<script>PageLoad();</script>");
            ScriptManager.RegisterStartupScript(this, this.GetType(), "hide22", "height();", true);
            if (HttpContext.Current.Session["userid"] == null)
            {

               //Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
            if (!IsPostBack)
            {
                generate_batch();
            }


        }
        protected void btncreate_Click(object sender, EventArgs e)
        {
            String strTranType = "'";
            int TotalNoRecord = 0;
            if (txtbatch.Text.Trim() == String.Empty)
            {
                generate_batch();
            }
            if (gridSummary.Rows.Count > 0)
            {
                for (int i = 0; i < gridSummary.Rows.Count; i++)
                {
                    GridViewRow row = gridSummary.Rows[i];
                    CheckBox c = (CheckBox)row.FindControl("chb1");
                    if (c.Checked == true)
                    {
                        switch (gridSummary.Rows[i].Cells[2].Text)
                        {
                            case "Market(N)":
                                strTranType = strTranType + "''11'',";
                                break;
                            case "Market(E)":
                                strTranType = strTranType + "''12'',";
                                break;
                            case "On-Market":
                                strTranType = strTranType + "''13'',";
                                break;
                            case "Off-Market":
                                strTranType = strTranType + "''2'',";
                                break;
                            case "Inter-Depository":
                                strTranType = strTranType + "''3'',";
                                break;
                            case "Inter-Settlement":
                                strTranType = strTranType + "''5'',";
                                break;
                            case "Delivery Out(R)":
                                strTranType = strTranType + "''61'',";
                                break;
                            case "Delivery Out(IR)":
                                strTranType = strTranType + "''62'',";
                                break;
                            case "CM pool to pool(N)":
                                strTranType = strTranType + "''71'',";
                                break;
                            case "CM pool to pool(E)":
                                strTranType = strTranType + "''72'',";
                                break;
                            case "Demat":
                                strTranType = strTranType + "''1'',";
                                break;

                        }

                        TotalNoRecord = TotalNoRecord + Convert.ToInt32(gridSummary.Rows[i].Cells[3].Text);

                    }

                }
                strTranType = strTranType.Substring(0, strTranType.LastIndexOf(",")) + "'";
                string slipno = "'";
                string benacc = "'";
                string dpid = "'";
                String UserBranchID = oDBEngine.GetDataTable("tbl_master_user", "user_branchid", "user_id='" + Convert.ToInt32(Session["userid"].ToString()) + "'").Rows[0][0].ToString();
                int batchLengh = txtbatch.Text.Length;
                string fileExt = String.Empty;
                if (batchLengh < 3)
                {
                    if (batchLengh == 1)
                        fileExt = "00" + txtbatch.Text;
                    else if (batchLengh == 2)
                        fileExt = "0" + txtbatch.Text;

                }
                else
                    fileExt = txtbatch.Text;

                try
                {
                    //if (con.State == ConnectionState.Open)
                    //{
                    //    con.Close();
                    //}
                    //con.Open();
                    //SqlCommand com = new SqlCommand("Export_CDSL", con);
                    //com.CommandType = CommandType.StoredProcedure;
                    //SqlParameter param = com.Parameters.AddWithValue("@DPID", HttpContext.Current.Session["usersegid"].ToString());
                    //com.Parameters.AddWithValue("@TranDate", Convert.ToDateTime(txtExecDate.Value));
                    //com.Parameters.AddWithValue("@BatchNo", Convert.ToInt32(txtbatch.Text));
                    //com.Parameters.AddWithValue("@UserId", Session["userid"].ToString());
                    //com.Parameters.AddWithValue("@FinYear", Session["LastFinYear"].ToString());
                    //com.Parameters.AddWithValue("@BranchID", UserBranchID);
                    //com.Parameters.AddWithValue("@CompanyId", HttpContext.Current.Session["LastCompany"].ToString());
                    //com.Parameters.AddWithValue("@strTranType", strTranType);
                    //com.Parameters.AddWithValue("@FileExt", fileExt);
                    //com.Parameters.AddWithValue("@TotalNoRecord", TotalNoRecord);
                    //SqlDataAdapter ad = new SqlDataAdapter(com);
                    //ad.Fill(ds);
                    ds = oExportRoutines.ExportCDSL(HttpContext.Current.Session["usersegid"].ToString(), Convert.ToDateTime(txtExecDate.Value).ToString("yyyy-MM-dd"),
                        Convert.ToInt32(txtbatch.Text), Session["userid"].ToString(), Session["LastFinYear"].ToString(), UserBranchID,
                        HttpContext.Current.Session["LastCompany"].ToString(), strTranType, fileExt, TotalNoRecord);
                    for (int slip = 0; slip < ds.Tables[1].Rows.Count; slip++)
                    {
                        slipno = slipno + "','" + ds.Tables[1].Rows[slip]["Ref"].ToString();
                        benacc = benacc + "','" + ds.Tables[1].Rows[slip]["bnfcry"].ToString().Substring(8, 8);
                        dpid = dpid + "','" + ds.Tables[1].Rows[slip]["bnfcry"].ToString().Substring(0, 8);
                    }
                    slipno = slipno + "''";
                    slipno = slipno.Remove(1, 2);
                    benacc = benacc + "''";
                    benacc = benacc.Remove(1, 2);
                    dpid = dpid + "''";
                    dpid = dpid.Remove(1, 2);

                    ds.GetXml();
                    con.Close();


                    //Changes Made strdate add
                    string strDate = oDBEngine.GetDate().ToString("dd/MM/yyyy").Replace("/", "");
                    string savepath = "../ExportFiles/Dp Batch/" + 18 + ds.Tables[0].Rows[0][0].ToString().Substring(0, 6) +
                                      '.' + strDate + '.' + fileExt;

                    string filename = 18 + ds.Tables[0].Rows[0][0].ToString().Substring(0, 6) + '.' + strDate + '.' + txtbatch.Text;
                    ds.WriteXml(Server.MapPath(savepath));
                    FileInfo FICSV = new FileInfo(Server.MapPath(savepath));
                    //FileInfo FICSV = new FileInfo(Server.MapPath("../Documents/18013700.044"));
                    String fileInfo;

                    using (StreamReader rwOpenTemplate = new StreamReader(FICSV.ToString()))
                    {
                        fileInfo = rwOpenTemplate.ReadToEnd();
                        fileInfo = fileInfo.Replace("<Table>", "");
                        fileInfo = fileInfo.Replace("<Arf />", "");

                        fileInfo = fileInfo.Replace("</Table>", "");

                        fileInfo = fileInfo.Replace("<Column1>", "");

                        fileInfo = fileInfo.Replace("</Column1>", "");

                        fileInfo = fileInfo.Replace("<NewDataSet>", "");

                        fileInfo = fileInfo.Replace("</NewDataSet>", "");

                        fileInfo = fileInfo.Replace("</Table1>", "");

                        fileInfo = fileInfo.Replace("<Table1>", "");

                        fileInfo = fileInfo.Replace("<?xml version=\"1.0\" standalone=\"yes\"?>", "");

                        fileInfo = fileInfo.Replace("\r\n", "");
                        fileInfo = fileInfo.Replace(" xml:space=\"preserve\"", "");

                    }
                    string[] file = fileInfo.Split(' ');
                    fileInfo = "";
                    for (int j = 0; j < file.Length; j++)
                    {
                        if (file[j].ToString() == "")
                        {

                        }
                        else
                        {
                            if (j == 6)
                            {
                                fileInfo = fileInfo + file[j].ToString() + " ";
                            }
                            else
                            {
                                fileInfo = fileInfo + file[j].ToString();
                            }
                            //fileInfo = fileInfo + file[j].ToString();
                        }
                    }
                    int start;
                    int at;
                    int end;
                    int count;

                    end = fileInfo.Length;
                    start = 0;
                    count = 0;
                    at = 0;
                    ArrayList a = new ArrayList();
                    string i = "";
                    while ((start <= end) && (at > -1))
                    {
                        // start+count must be a position within -str-.
                        count = end - start;
                        at = fileInfo.IndexOf("<TP>", start, count);
                        if (at == -1) break;
                        start = at + 1;
                        i = i + ',' + at.ToString();
                    }
                    string[] ii = i.Split(',');

                    for (int s = ii.Length - 1; s > 0; s--)
                    {
                        fileInfo = fileInfo.Insert(Convert.ToInt32(ii[s]), "\r\n");
                    }
                    start = 0;
                    count = 0;
                    at = 0;
                    i = "";
                    while ((start <= end) && (at > -1))
                    {
                        // start+count must be a position within -str-.
                        count = end - start;
                        at = fileInfo.IndexOf("></", start, count);
                        if (at == -1) break;
                        start = at + 1;
                        i = i + ',' + at.ToString();
                    }
                    string[] iii = i.Split(',');

                    for (int ss = iii.Length - 1; ss > 0; ss--)
                    {
                        string match = "";
                        int search = 0;
                        while (match != "<")
                        {
                            match = fileInfo.Substring(Convert.ToInt32(iii[ss]) - search, 1);
                            search = search + 1;

                        }
                        if (fileInfo.Substring(Convert.ToInt32(iii[ss]) - (search - 1), (2 * (search - 1)) + 3) != "<Arf></Arf>")
                        {
                            fileInfo = fileInfo.Remove(Convert.ToInt32(iii[ss]) - (search - 1), (2 * (search - 1)) + 3);
                        }

                    }
                    fileInfo = fileInfo.Replace("\r\n", "\n");
                    fileInfo = fileInfo + "\n";
                    StreamWriter streamWriter = new StreamWriter(Server.MapPath(savepath));
                    streamWriter.Write(fileInfo);
                    streamWriter.Close();
                    ViewState["savepath"] = savepath;
                    if (con.State == ConnectionState.Open)
                    {
                        con.Close();
                    }
                    con.Open();
                    string seg = oDBEngine.GetDataTable("tbl_master_companyexchange", " exch_internalid", "exch_tmcode='" + Convert.ToInt32(Session["usersegid"].ToString()) + "'").Rows[0][0].ToString();
                    //SqlCommand com1 = new SqlCommand("sp_Insert_ExportFiles", con);
                    //com1.CommandType = CommandType.StoredProcedure;
                    //SqlParameter param1 = com1.Parameters.AddWithValue("@userid", Session["userid"].ToString());
                    //com1.Parameters.AddWithValue("@segid", seg);
                    //com1.Parameters.AddWithValue("@file_type", "Dp Batch");
                    //com1.Parameters.AddWithValue("@file_name", filename);
                    //com1.Parameters.AddWithValue("@batch_number", txtbatch.Text);
                    //com1.Parameters.AddWithValue("@file_path", savepath.Substring(3));
                    //com1.ExecuteNonQuery();
                    //con.Close();
                    string file_type = "Dp Batch";
                    oExportRoutines.InsertExportFiles(Session["userid"].ToString(), seg, file_type, filename, txtbatch.Text, savepath.Substring(3));
                    ScriptManager.RegisterStartupScript(HttpContext.Current.Handler as Page, HttpContext.Current.GetType(), "di", "alert('Generated Successfully');", true);
                    DirectoryInfo dir = new DirectoryInfo(Server.MapPath("../Documents/"));
                    FileInfo[] fileInfos = dir.GetFiles("*v");
                    for (int leng = 0; leng < fileInfos.Length; leng++)
                    {
                        File.Delete(((System.IO.FileSystemInfo)(fileInfos[leng])).FullName);
                    }
                    //if (con.State == ConnectionState.Open)
                    //{
                    //    con.Close();
                    //}
                    //con.Open();
                    //SqlCommand com11 = new SqlCommand("sp_update_slips", con);
                    //com11.CommandType = CommandType.StoredProcedure;
                    //SqlParameter param11 = com11.Parameters.AddWithValue("@slipno", slipno.ToString());
                    //com11.Parameters.AddWithValue("@benacc", benacc);
                    //com11.Parameters.AddWithValue("@dpid", dpid);
                    //com11.ExecuteNonQuery();
                    //con.Close();
                    oExportRoutines.UpdateSlips(benacc, dpid);
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(HttpContext.Current.Handler as Page, HttpContext.Current.GetType(), "di", "alert('There is no record to export');", true);

                }
                //string[] row = new string[ds.Tables[0].Rows.Count];
                //foreach (DataRow dr in ds.Tables[0].Rows)
                //{
                //    row[0] = dr.ItemArray.GetValue(0).ToString() + dr.ItemArray.GetValue(1).ToString();
                //}
                //string[] rowString = new string[ds.Tables[1].Rows.Count];
                //int i = 0;
                //foreach (DataRow dr in ds.Tables[1].Rows)
                //{
                //    rowString[i] = row[0] + dr.ItemArray.GetValue(0).ToString() + dr.ItemArray.GetValue(1).ToString() + dr.ItemArray.GetValue(2).ToString() +
                //    dr.ItemArray.GetValue(3).ToString()+dr.ItemArray.GetValue(4).ToString()+dr.ItemArray.GetValue(5).ToString()+
                //    dr.ItemArray.GetValue(6).ToString()+ dr.ItemArray.GetValue(7).ToString()
                //    +dr.ItemArray.GetValue(8).ToString()+dr.ItemArray.GetValue(9).ToString()
                //    +dr.ItemArray.GetValue(10).ToString()+dr.ItemArray.GetValue(11).ToString()
                //    +dr.ItemArray.GetValue(12).ToString()+dr.ItemArray.GetValue(13).ToString()
                //    +dr.ItemArray.GetValue(14).ToString();             
                //    i++;
                //}
                //File.WriteAllLines(@"C:\customer.txt", rowString);
                BindGrid();
            }

        }
        protected void Download_Click(object sender, EventArgs e)
        {
            fn();
        }
        protected void fn()
        {
            try
            {
                string filename = Server.MapPath(ViewState["savepath"].ToString());

                FileInfo fileInfo = new FileInfo(filename);

                //if (fileInfo.Exists)
                //{
                //    Response.Clear();
                //    Response.AddHeader("Content-Disposition", "attachment;filename=" + fileInfo.Name);
                //    Response.AddHeader("Content-Length", fileInfo.Length.ToString());
                //    Response.ContentType = "application/octet-stream";
                //    Response.Flush();
                //    Response.WriteFile(fileInfo.FullName);
                //    Response.End();
                //}
                if (fileInfo.Exists)
                {
                    Response.Clear();
                    Response.AddHeader("Content-Disposition", "attachment;filename=" + fileInfo.Name);
                    Response.AddHeader("Content-Length", fileInfo.Length.ToString());
                    //Response.ContentType = "application/octet-stream";
                    Response.ContentType = "application/unknown";
                    Response.Flush();
                    //Response.WriteFile(fileInfo.FullName);
                    Response.TransmitFile(fileInfo.FullName);
                    Response.End();
                }
            }
            catch { }

        }

        protected void generate_batch()
        {
            DataTable Dt = oDBEngine.GetDataTable("Trans_CdslBatch ORDER BY CdslBatch_ID DESC ", "TOP (1) CdslBatch_Number", null);
            if (Dt.Rows.Count > 0)
                batchno = Convert.ToInt32(Dt.Rows[0][0].ToString()) + 1;
            else
                batchno = 1;
            txtbatch.Text = batchno.ToString();
        }
        protected void btnshow_Click(object sender, EventArgs e)
        {
            generate_batch();
            BindGrid();
        }
        void BindGrid()
        {
            if (txtExecDate.Text.Trim() != String.Empty)
            {
                //if (con.State == ConnectionState.Open)
                //{
                //    con.Close();
                //}
                //con.Open();
                //SqlCommand com1 = new SqlCommand("show_batch_cdsl", con);
                //com1.CommandType = CommandType.StoredProcedure;
                //SqlParameter param1 = com1.Parameters.AddWithValue("@batchdate", txtExecDate.Text.Split('-')[2] + '-' + txtExecDate.Text.Split('-')[1] + '-' + txtExecDate.Text.Split('-')[0]);
                //com1.Parameters.AddWithValue("@DpID", HttpContext.Current.Session["usersegid"].ToString());
                //DataSet ds = new DataSet();
                //SqlDataAdapter da = new SqlDataAdapter(com1);
                //DataTable TempTable = new DataTable();
                //da.Fill(ds);
                string batchdate = (txtExecDate.Text.Split('-')[2] + '-' + txtExecDate.Text.Split('-')[1] + '-' + txtExecDate.Text.Split('-')[0]);
                ds = oExportRoutines.ShowBatchCDSL(batchdate, HttpContext.Current.Session["usersegid"].ToString());
                DataTable dt2 = new DataTable();
                DataTable dt1 = ds.Tables[0];
                dt2 = dt1.Clone();
                int count = 0;
                foreach (DataRow DR in dt1.Rows)
                {
                    if (DR[1].ToString() == "Inter-Depository(Mkt.)" || DR[1].ToString() == "Inter-Depository(OffMkt.)")
                        count = count + Convert.ToInt32(DR[3].ToString());
                    else
                        dt2.ImportRow(DR);
                }
                if (count > 0)
                {
                    DataRow Nrow = dt2.NewRow();
                    Nrow[0] = ds.Tables[0].Rows[0][0].ToString();
                    Nrow[1] = "Inter-Depository";
                    Nrow[2] = ds.Tables[0].Rows[0][2].ToString();
                    Nrow[3] = count.ToString();
                    dt2.Rows.Add(Nrow);

                }
                gridSummary.DataSource = dt2;
                gridSummary.DataBind();
                if (gridSummary.Rows.Count != 0)
                {
                    btncreate.Enabled = true;
                    Download.Enabled = true;
                }
            }

        }
    }

}