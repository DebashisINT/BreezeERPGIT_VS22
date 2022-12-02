using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace ERP.OMS.Management.DailyTask
{
    public partial class management_DailyTask_view_signature : ERP.OMS.ViewState_class.VSPage
    {
        


        BusinessLogicLayer.Converter objConverter = new BusinessLogicLayer.Converter();
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        static DataSet ds = new DataSet();
        #region Properties
        public string _FinYear
        {
            get { return (string)ViewState["ContactIDs"]; }
            set { ViewState["ContactIDs"] = value; }
        }
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["id"].Split('[')[1] != "CDSL]")
                {
                    //Fetch All Contacts ID For NSDL Authorized Signatory
                    DataTable DtContactIDs = oDBEngine.GetDataTable("tbl_master_document", "doc_ContactID", @"doc_contactId in (
                select NsdlSignatory_SignatoryID 
                from Master_NsdlSignatory,Master_NsdlCorporateSignatory,Master_NsdlSignatoryMap
                where NsdlSignatoryMap_BenAccountID='" + Request.QueryString["id"].Split('[')[0].Trim() + @"'
                and NsdlSignatoryMap_CorpSignatoryID=NsdlCorporateSignatory_CorpSignatoryID
                and NsdlCorporateSignatory_MemberSignatoryID=NsdlSignatory_SignatoryID)
                Union 
                Select '" + Request.QueryString["id"].Split('[')[0].Trim() + "'");
                    foreach (DataRow Dr in DtContactIDs.Rows)
                    {
                        if (ViewState["ContactIDs"] == null)
                            ViewState["ContactIDs"] = "'" + Dr[0].ToString() + "'";
                        else
                            ViewState["ContactIDs"] = ViewState["ContactIDs"].ToString() + ',' + "'" + Dr[0].ToString() + "'";
                    }
                }
                BindGrid();
                bind();
            }
        }
        public void bind()
        {
            SqlConnection con = new SqlConnection(ConfigurationSettings.AppSettings["DBConnectionDefault"]);


            ds.Clear();
            SqlCommand com = null;
            SqlDataAdapter ad = null;
            try
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }

                con.Open();
                if (Request.QueryString["id"].Split('[')[1] == "CDSL]")
                {
                    //com = new SqlCommand("select distinct ('../Documents/'+a.doc_source) as image from (select doc_source,doc_contactid from tbl_master_documentType join tbl_master_document on tbl_master_documentType.dty_id=tbl_master_document.doc_documenttypeid where dty_documenttype='Signature' and dty_applicablefor='CDSL Clients')a join trans_cdsloffline on trans_cdsloffline.CdslOffline_DPID+trans_cdsloffline.CdslOffline_BenAccountNumber=a.doc_contactid ", con);
                    com = new SqlCommand("Select distinct ('../Documents/'+tbl_master_document.doc_source) AS image from tbl_master_document INNER JOIN tbl_master_documentType ON tbl_master_document.doc_documentTypeId = tbl_master_documentType.dty_id WHERE substring(doc_contactid,9,8)='" + Request.QueryString["id"].Split('[')[0].Trim() + "'", con);

                }
                else
                {
                    com = new SqlCommand("Select distinct ('../Documents/'+tbl_master_document.doc_source) AS image from tbl_master_document INNER JOIN tbl_master_documentType ON tbl_master_document.doc_documentTypeId = tbl_master_documentType.dty_id WHERE doc_contactId in (" + ViewState["ContactIDs"].ToString() + ")", con);
                }
                com.CommandType = CommandType.Text;
                ad = new SqlDataAdapter(com);
                ad.Fill(ds);
                GridView1.DataSource = ds;
                GridView1.DataBind();
            }
            catch
            {
            }
            finally
            {
                if (com != null)
                    com.Dispose();
                if (con != null)
                    con.Dispose();
                if (ad != null)
                    ad.Dispose();

                ds.Dispose();
            }
        }

        public void BindGrid()
        {
            DataTable dt = new DataTable();
            DataTable dt1 = new DataTable();
            DataColumn col1 = new DataColumn("Id");
            DataColumn col2 = new DataColumn("Type");
            DataColumn col3 = new DataColumn("FileName");
            DataColumn col4 = new DataColumn("Src");
            DataColumn col5 = new DataColumn("FilePath");
            dt.Columns.Add(col1);
            dt.Columns.Add(col2);
            dt.Columns.Add(col3);
            dt.Columns.Add(col4);
            dt.Columns.Add(col5);
            if (Request.QueryString["id"].Split('[')[1] == "CDSL]")
            {
                dt1 = oDBEngine.GetDataTable("tbl_master_document INNER JOIN tbl_master_documentType ON tbl_master_document.doc_documentTypeId = tbl_master_documentType.dty_id", "tbl_master_document.doc_id AS Id, tbl_master_documentType.dty_documentType AS Type,tbl_master_document.doc_documentName AS FileName, tbl_master_document.doc_source AS Src,isnull(doc_buildingid,0) as doc_buildingid,doc_Floor,doc_RoomNo,doc_CellNo,doc_FileNo", "substring(doc_contactid,9,8)='" + Request.QueryString["id"].Split('[')[0].Trim() + "'");
            }
            else
            {
                dt1 = oDBEngine.GetDataTable("tbl_master_document INNER JOIN tbl_master_documentType ON tbl_master_document.doc_documentTypeId = tbl_master_documentType.dty_id", "tbl_master_document.doc_id AS Id, tbl_master_documentType.dty_documentType AS Type,tbl_master_document.doc_documentName AS FileName, tbl_master_document.doc_source AS Src,isnull(doc_buildingid,0) as doc_buildingid,doc_Floor,doc_RoomNo,doc_CellNo,doc_FileNo", "doc_contactId in (" + ViewState["ContactIDs"].ToString() + ")");

            }
            if (dt1.Rows.Count != 0)
            {
                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    if (dt1.Rows[i][4].ToString() == "0")
                    {
                        DataRow RowNew = dt.NewRow();
                        RowNew["Id"] = dt1.Rows[i][0].ToString();
                        RowNew["Type"] = dt1.Rows[i][1].ToString();
                        RowNew["FileName"] = dt1.Rows[i][2].ToString();
                        RowNew["Src"] = dt1.Rows[i][3].ToString();
                        string BName = "Building-  " + " " + "/ Floor No : " + dt1.Rows[i][5].ToString() + " " + "/ Room No-" + dt1.Rows[i][6].ToString() + " " + "/ Cabinet No-" + dt1.Rows[i][7].ToString() + " " + "/ File No-" + dt1.Rows[i][8].ToString();
                        RowNew["FilePath"] = BName;
                        dt.Rows.Add(RowNew);
                    }
                    else
                    {
                        DataRow RowNew = dt.NewRow();
                        RowNew["Id"] = dt1.Rows[i][0].ToString();
                        RowNew["Type"] = dt1.Rows[i][1].ToString();
                        RowNew["FileName"] = dt1.Rows[i][2].ToString();
                        RowNew["Src"] = dt1.Rows[i][3].ToString();
                        string BuildingName = "";
                        string[,] bname1 = oDBEngine.GetFieldValue("tbl_master_building", "bui_name", " bui_id='" + dt1.Rows[i][4].ToString() + "'", 1);
                        if (bname1[0, 0] != "n")
                        {
                            BuildingName = bname1[0, 0];
                        }
                        string BName = "Building-  " + BuildingName + " " + "/ Floor No : " + dt1.Rows[i][5].ToString() + " " + "/ Room No-" + dt1.Rows[i][6].ToString() + " " + "/ Cabinet No-" + dt1.Rows[i][7].ToString() + " " + "/ File No-" + dt1.Rows[i][8].ToString();
                        RowNew["FilePath"] = BName;
                        dt.Rows.Add(RowNew);
                    }
                }
            }
            NsdlClientDocumentGrid.DataSource = dt.DefaultView;
            NsdlClientDocumentGrid.DataBind();
        }



    }
}