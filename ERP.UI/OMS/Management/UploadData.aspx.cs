using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web;
using System.Web.UI;
using BusinessLogicLayer;

namespace ERP.OMS.Management
{
    public partial class management_UploadData : System.Web.UI.Page
    {
        //string str = ConfigurationManager.AppSettings["DBConnectionDefault"].ToString();MULTI
        string str = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
        static string databasestr;
        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);MULTI
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        DataSet dsData;
        string delIDs = "";
        string delColName = "";
        Management_BL mng_bl = new Management_BL();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string[] str1 = str.Split(';');
                databasestr = str1[1].Remove(0, str1[1].IndexOf('=') + 1);

            }

        }
        protected void Button2_Click(object sender, EventArgs e)
        {
            string mainAcc = oDBEngine.transfertDataToXmlforMainAcc();
            string subAcc = oDBEngine.transfertDataToXmlforSubAcc();

        }
        protected void Button3_Click(object sender, EventArgs e)
        {
            String FilePath = Path.GetFullPath(FileUpload1.PostedFile.FileName);
            String FileName = Path.GetFileName(FileUpload1.PostedFile.FileName);
            String fileInfo;
            string path1;
            path1 = Path.Combine(ConfigurationManager.AppSettings["SaveCSV"], FileName);


            FileUpload1.SaveAs(MapPath(path1));
            FileInfo FICSV = new FileInfo(MapPath(path1));
            string strExt = System.IO.Path.GetExtension(FileUpload1.PostedFile.FileName);
            if (strExt.Trim() == ".xml")
            {
                DataSet reportData = new DataSet();
                reportData.ReadXml(MapPath(path1));
                //using (SqlConnection lConn = new SqlConnection(ConfigurationSettings.AppSettings["DBConnectionDefault"]))MULTI
                using (SqlConnection lConn = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"])))
                {
                    SqlCommand cmd = null;
                    SqlCommand cmd1 = null;
                    try
                    {
                        SqlBulkCopy sbc = new SqlBulkCopy(lConn);
                        sbc.DestinationTableName = "Master_MainAccount_Temp";
                        if (lConn.State != ConnectionState.Open)
                            lConn.Open();
                        DataTable dt = reportData.Tables[0];
                        sbc.WriteToServer(dt);

                        cmd = new SqlCommand("insert into Master_MainAccount(MainAccount_AccountType,MainAccount_BankCashType,MainAccount_BankAccountType,MainAccount_ExchangeSegment,MainAccount_AccountCode,MainAccount_AccountGroup,MainAccount_Name,MainAccount_BankAcNumber,MainAccount_SubLedgerType,MainAccount_IsTDS,MainAccount_TDSRate,MainAccount_IsFBT,MainAccount_FBTRate,MainAccount_RateOfInterest,CreateDate,CreateUser,MainAccount_ID) select MainAccount_AccountType,MainAccount_BankCashType,MainAccount_BankAccountType,MainAccount_ExchangeSegment,MainAccount_AccountCode,MainAccount_AccountGroup,MainAccount_Name,MainAccount_BankAcNumber,MainAccount_SubLedgerType,cast(MainAccount_IsTDS as bit),cast(MainAccount_TDSRate as decimal(6,2)),cast(MainAccount_IsFBT as bit),cast(MainAccount_FBTRate as decimal(6,2)),cast(MainAccount_RateOfInterest as decimal(6,2)),CreateDate,cast(CreateUser as numeric(18,0)),MainAccount_ReferenceID from Master_MainAccount_Temp where Master_MainAccount_Temp.MainAccount_AccountCode not in (select a.MainAccount_AccountCode from Master_MainAccount a)", lConn);
                        cmd1 = new SqlCommand("truncate table Master_MainAccount_Temp", lConn);

                        cmd.ExecuteNonQuery();
                        cmd1.ExecuteNonQuery();
                    }
                    catch { }
                    finally
                    {
                        if (cmd != null)
                            cmd.Dispose();
                        if (cmd1 != null)
                            cmd1.Dispose();
                        if (lConn.State == ConnectionState.Open)
                            lConn.Close();

                    }

                }
                File.Delete(MapPath(path1));
            }
        }
        protected void Button1_Click(object sender, EventArgs e)
        {
            String FilePath = Path.GetFullPath(FileUpload2.PostedFile.FileName);
            String FileName = Path.GetFileName(FileUpload2.PostedFile.FileName);
            String fileInfo;
            string path1;
            path1 = Path.Combine(ConfigurationManager.AppSettings["SaveCSV"], FileName);


            FileUpload2.SaveAs(MapPath(path1));
            FileInfo FICSV = new FileInfo(MapPath(path1));
            string strExt = System.IO.Path.GetExtension(FileUpload2.PostedFile.FileName);
            if (strExt.Trim() == ".xml")
            {
                DataSet reportData = new DataSet();
                reportData.ReadXml(MapPath(path1));
                //using (SqlConnection lConn = new SqlConnection(ConfigurationSettings.AppSettings["DBConnectionDefault"]))MULTI
                using (SqlConnection lConn = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"])))
                {
                    SqlCommand cmd = null;
                    SqlCommand cmd1 = null;
                    try
                    {
                        SqlBulkCopy sbc = new SqlBulkCopy(lConn);
                        sbc.DestinationTableName = "master_subaccount_temp";
                        if (lConn.State != ConnectionState.Open)
                            lConn.Open();
                        DataTable dt = reportData.Tables[0];
                        sbc.WriteToServer(dt);

                        cmd = new SqlCommand("insert into Master_SubAccount(SubAccount_MainAcReferenceID,SubAccount_Code,SubAccount_Name,SubAccount_IsTDS,SubAccount_TDSRate,SubAccount_IsFBT,SubAccount_FBTRate,SubAccount_RateOfInterest,SubAccount_ContactID,CreateDate,CreateUser) select SubAccount_MainAcReferenceID ,SubAccount_Code,SubAccount_Name,cast(SubAccount_IsTDS as bit),cast(SubAccount_TDSRate as decimal(6,2)),cast(SubAccount_IsFBT as bit),cast(SubAccount_FBTRate as decimal(6,2)),cast(SubAccount_RateOfInterest as decimal(6,2)),SubAccount_ContactID,CreateDate,cast(CreateUser as numeric(18,0)) from master_subaccount_temp where cast(master_subaccount_temp.createdate as datetime)>(select max(cast(createdate as datetime)) from Master_SubAccount)", lConn);
                        cmd1 = new SqlCommand("truncate table master_subaccount_temp", lConn);

                        cmd.ExecuteNonQuery();
                        cmd1.ExecuteNonQuery();
                    }
                    catch { }
                    finally
                    {
                        if (cmd != null)
                            cmd.Dispose();
                        if (cmd1 != null)
                            cmd1.Dispose();
                        if (lConn.State == ConnectionState.Open)
                            lConn.Close();
                    }

                }
                DataTable dtSub = oDBEngine.GetDataTable("master_mainaccount", "mainaccount_id,mainaccount_referenceid", " mainaccount_id in (select subaccount_MainAcReferenceID from master_subaccount)");
                if (dtSub.Rows.Count > 0)
                {
                    for (int i = 0; i < dtSub.Rows.Count; i++)
                    {
                        int NoofAffected = oDBEngine.SetFieldValue("master_subaccount", "SubAccount_MainAcReferenceID='" + dtSub.Rows[i][1].ToString() + "'", "SubAccount_MainAcReferenceID='" + dtSub.Rows[i][0].ToString() + "'");
                    }
                }
                File.Delete(MapPath(path1));
            }
        }

        protected void Create_IncrementalXML(object sender, EventArgs e)
        {
            //using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]))MULTI
            using (SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"])))
            {
                if (con.State.Equals(ConnectionState.Closed))
                    con.Open();
                try
                {
                    using (SqlDataAdapter da = new SqlDataAdapter("sp_Data_Del_Ins_Up", con))
                    {
                        DataSet Ds = new DataSet();

                        //da.SelectCommand.CommandTimeout = 0;
                        //da.SelectCommand.CommandType = CommandType.StoredProcedure;
                        //da.SelectCommand.Parameters.AddWithValue("@sourceDB", databasestr);
                        //da.Fill(Ds);
                        Ds = mng_bl.sp_Data_Del_Ins_Up(databasestr);
                        string xmlPath = Server.MapPath("Data.xml");
                        if (Ds.Tables[0].Rows.Count > 0)
                            Ds.Tables[0].TableName = "Insert_tbl_trans_menu";

                        if (Ds.Tables[1].Rows.Count > 0)
                            Ds.Tables[1].TableName = "Insert_Master_UOM";

                        if (Ds.Tables[2].Rows.Count > 0)
                            Ds.Tables[2].TableName = "Insert_Master_MainAccount";

                        if (Ds.Tables[3].Rows.Count > 0)
                            Ds.Tables[3].TableName = "Insert_Master_SubAccount";

                        if (Ds.Tables[4].Rows.Count > 0)
                            Ds.Tables[4].TableName = "Insert_Master_topics";
                        for (int i = 0; i < Ds.Tables[4].Rows.Count; i++)
                        {
                            Ds.Tables[4].Rows[i]["Topics_AccessCode"] = Ds.Tables[4].Rows[i]["Topics_AccessCode"].ToString().Replace("'", "''");
                        }


                        Ds.WriteXml(xmlPath);
                        ScriptManager.RegisterStartupScript(HttpContext.Current.Handler as Page, HttpContext.Current.GetType(), "di", "alert('Uploaded Sucessfully.');", true);
                    }

                }
                catch
                {
                    ScriptManager.RegisterStartupScript(HttpContext.Current.Handler as Page, HttpContext.Current.GetType(), "di", "alert('Can not be Uploaded.');", true);

                }

            }


        }

        //protected void Read_IncrementalXML(object sender, EventArgs e)
        //{
        //    string FileName = "Data.xml";
        //    string FilePath = Path.Combine(ConfigurationManager.AppSettings["DataUpgradePath"], FileName);

        //    FetchData(FilePath);
        //}
        //private XmlReader GetXMLContent(String ContentURL)
        //{
        //    //try
        //    //{
        //        //create an HTTP request.
        //        HttpWebRequest wr = (HttpWebRequest)(WebRequest.Create(ContentURL));
        //        wr.Timeout = 10000; // 10 seconds
        //        //get the response object.
        //        WebResponse resp = wr.GetResponse();
        //        Stream stream = resp.GetResponseStream();
        //        //load XML document
        //        XmlTextReader reader = new XmlTextReader(stream);

        //        reader.XmlResolver = null;
        //        return reader;
        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //    //return some error code.
        //    //    string errStr1 = ex.Message;
        //    //    Page.ClientScript.RegisterStartupScript(typeof(Page), "ErrMessage1", "<script language='javascript'>alert('Hi');alert(<%=errStr1%>);</script>");
        //    //    return;
        //    //}
        //}

        //private void FetchData(String inURL)
        //{
        //    try
        //    {
        //        dsData = new DataSet();
        //        dsData.ReadXml(GetXMLContent(inURL));

        //        SqlConnection lConn = new SqlConnection(ConfigurationSettings.AppSettings["DBConnectionDefault"]);
        //        if (lConn.State == ConnectionState.Open)
        //        {
        //            lConn.Close();
        //        }
        //        lConn.Open();
        //        SqlCommand cmd = new SqlCommand("sp_Data_Del_Ins_Up_xml", lConn);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.AddWithValue("@targetDB", databasestr);
        //        cmd.Parameters.AddWithValue("@doc", dsData.GetXml());
        //        cmd.ExecuteNonQuery();
        //        lConn.Close();

        //        //GridView1.DataSource = dsData;
        //        //GridView1.DataBind();


        //        //for (int i = 0; i < dsData.Tables[0].Rows.Count; i++)
        //        //{
        //        //    if (i == 0)
        //        //        delIDs = Convert.ToString(dsData.Tables[0].Rows[i][0]);
        //        //    else
        //        //        delIDs = delIDs + ", " + Convert.ToString(dsData.Tables[0].Rows[i][0]);
        //        //}

        //        //for (int i = 0; i < dsData.Tables[0].Columns.Count; i++)
        //        //    delColName = dsData.Tables[0].Columns[i].ColumnName;

        //        //DeleteData(delIDs, delColName);

        //    }
        //    catch (Exception ex)
        //    {
        //        //return some error code.
        //        string errStr2 = ex.Message;
        //        Page.ClientScript.RegisterStartupScript(typeof(Page), "ErrMessage2", "<script language='javascript'>alert('Hello');alert(<%=errStr2%>);</script>");

        //    }
        //}

        //protected void DeleteData(string delIDs, string delColName)
        //{
        //    string sqlDel = @" delete from " + txtTarget.Text.Trim() +
        //                    " where " + delColName + " in (" + delIDs + ")";

        //}

        //protected void Test(object sender, EventArgs e)
        //{
        //    using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]))
        //    {
        //        if (con.State.Equals(ConnectionState.Closed))
        //            con.Open();

        //        SqlDataAdapter da = new SqlDataAdapter("sp_Test", con);

        //        da.SelectCommand.CommandTimeout = 0;
        //        da.SelectCommand.CommandType = CommandType.StoredProcedure;

        //        DataSet ds = new DataSet();
        //        da.Fill(ds);

        //        string count1 = Convert.ToString(ds.Tables.Count);

        //        ds.Tables[0].TableName = "Employee";
        //        ds.Tables[1].TableName = "Department";

        //        ds.WriteXml(@"D:\Test.xml");

        //        DataSet ds1 = new DataSet();
        //        ds1.ReadXml(@"D:\Test.xml");

        //        GridView1.DataSource = ds1.Tables["Employee"];
        //        GridView1.DataBind();

        //        GridView2.DataSource = ds1.Tables["Department"];
        //        GridView2.DataBind();
        //    }
        //}
    }
}