using System;
using System.Data;
using System.Web;
using System.Web.UI;
using DevExpress.Web;
using System.Configuration;
using BusinessLogicLayer;
using EntityLayer.CommonELS;

namespace ERP.OMS.Management.Master
{
    public partial class Product_Document : ERP.OMS.ViewState_class.VSPage
    {        //DBEngine oDBEngine = new DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);MULTI
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        public string pageAccess = "";
        // DBEngine oDBEngine = new DBEngine(string.Empty);
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //sas
                //'http://localhost:2957/InfluxCRM/management/testProjectMainPage_employee.aspx'
                string sPath = HttpContext.Current.Request.Url.ToString();
                oDBEngine.Call_CheckPageaccessebility(sPath);
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
              
                #region PageHeader
                if (Convert.ToString(Request.QueryString["type"]) == "Product")
                {
                    string msg = " [ Short Name:";
                    int id = Convert.ToInt32(Convert.ToString(Request.QueryString["idbldng"]));

                    DataTable dt = oDBEngine.GetDataTable(" Select sProducts_Code,sProducts_Name from Master_sProducts where sProducts_ID=" + id + " ");
                    if(dt!=null && dt.Rows.Count>0)
                    {
                        msg =msg+ Convert.ToString(dt.Rows[0]["sProducts_Code"]) +", Name:";
                        msg = msg + Convert.ToString(dt.Rows[0]["sProducts_Name"])+ " ]";
                        string msg1 = lblheader.Text;
                        lblheader.Text = msg1 + msg;
                    }
                    //lblheader
                    //HeaderText.InnerText = "Add " + Convert.ToString(Request.QueryString["type"]) + " Document";
                    //Session["PrePageRedirect"] = "/OMS/management/store/Master/sProducts.aspx";
                }
                else
                {
                    //HeaderText.InnerText = "Add Contact Document";
                    //Session["PrePageRedirect"] = "root_Companies.aspx";
                }
                #endregion
            }

            /*#region Session For QueryString
            if (!string.IsNullOrEmpty(Convert.ToString(Request.QueryString["idbldng"])) && !string.IsNullOrEmpty(Convert.ToString(Request.QueryString["type"])))
            {
                Session["PageRedirect"] = Convert.ToString("/OMS/Management/Master/Contact_Document.aspx?idbldng=" + Convert.ToString(Request.QueryString["idbldng"]) + "&type=" + Convert.ToString(Request.QueryString["type"]));
            }
            #endregion*/

            if (!string.IsNullOrEmpty(Convert.ToString(Request.QueryString["type"])))
            {
                Session["requesttype"] = Convert.ToString(Request.QueryString["type"]);
            }


            Session["KeyVal2"] = null;

            if (HttpContext.Current.Session["userid"] == null)
            {
                //Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }

            if (Session["Name"] != null)
            {
                lblName.Text = Session["Name"].ToString();
            }
            else if (Session["CompanyName"] != null)
            {
                lblName.Text = "Company Name :" + "  " + Session["CompanyName"].ToString();
            }
            BindGrid();
            if (Session["ContactType"] != null || Session["requesttype"] != null)
            {
                string cnttype = Convert.ToString(Session["ContactType"]);
                string reqsttype = Convert.ToString(Session["requesttype"]);
                if (cnttype == "OtherEntity")
                {
                    TabPage page = ASPxPageControl1.TabPages.FindByName("CorresPondence");
                    page.Visible = true;
                    page = ASPxPageControl1.TabPages.FindByName("Bank Details");
                    page.Visible = false;
                    page = ASPxPageControl1.TabPages.FindByName("DP Details");
                    page.Visible = false;
                    page = ASPxPageControl1.TabPages.FindByName("Documents");
                    page.Visible = true;
                    page = ASPxPageControl1.TabPages.FindByName("FamilyMembers");
                    page.Visible = false;
                    page = ASPxPageControl1.TabPages.FindByName("Registration");
                    page.Visible = false;
                    page = ASPxPageControl1.TabPages.FindByName("Group Member");
                    page.Visible = false;
                    page = ASPxPageControl1.TabPages.FindByName("Deposit");
                    page.Visible = false;
                    page = ASPxPageControl1.TabPages.FindByName("Remarks");
                    page.Visible = true;
                    page = ASPxPageControl1.TabPages.FindByName("Education");
                    page.Visible = false;
                    page = ASPxPageControl1.TabPages.FindByName("Trad. Prof.");
                    page.Visible = false;
                    page = ASPxPageControl1.TabPages.FindByName("Other");
                    page.Visible = false;
                    page = ASPxPageControl1.TabPages.FindByName("Subscription");
                    page.Visible = false;
                }
                //if (cnttype == "Companies" || reqsttype == "Companies")
                if (reqsttype == "Companies" || reqsttype == "Branches" || reqsttype == "Exchanges" || reqsttype == "Buildzsing" || reqsttype == "Account Heads")// || reqsttype == "Companies"
                {
                    TabPage page = ASPxPageControl1.TabPages.FindByName("CorresPondence");
                    page.Visible = true;
                    page = ASPxPageControl1.TabPages.FindByName("Bank Details");
                    page.Visible = false;
                    page = ASPxPageControl1.TabPages.FindByName("DP Details");
                    page.Visible = false;
                    page = ASPxPageControl1.TabPages.FindByName("Documents");
                    page.Visible = true;
                    page = ASPxPageControl1.TabPages.FindByName("FamilyMembers");
                    page.Visible = false;
                    page = ASPxPageControl1.TabPages.FindByName("Registration");
                    page.Visible = false;
                    page = ASPxPageControl1.TabPages.FindByName("Group Member");
                    page.Visible = false;
                    page = ASPxPageControl1.TabPages.FindByName("Deposit");
                    page.Visible = false;
                    page = ASPxPageControl1.TabPages.FindByName("Remarks");
                    page.Visible = false;
                    page = ASPxPageControl1.TabPages.FindByName("Education");
                    page.Visible = false;
                    page = ASPxPageControl1.TabPages.FindByName("Trad. Prof.");
                    page.Visible = false;
                    page = ASPxPageControl1.TabPages.FindByName("Other");
                    page.Visible = false;
                    page = ASPxPageControl1.TabPages.FindByName("Subscription");
                    page.Visible = false;
                    page = ASPxPageControl1.TabPages.FindByName("General");
                    page.Visible = true;
                }
                if (reqsttype == "Product")
                {
                    TabPage page = ASPxPageControl1.TabPages.FindByName("CorresPondence");
                    page.Visible = false;
                    page = ASPxPageControl1.TabPages.FindByName("Bank Details");
                    page.Visible = false;
                    page = ASPxPageControl1.TabPages.FindByName("DP Details");
                    page.Visible = false;
                    page = ASPxPageControl1.TabPages.FindByName("Documents");
                    page.Visible = true;
                    page = ASPxPageControl1.TabPages.FindByName("FamilyMembers");
                    page.Visible = false;
                    page = ASPxPageControl1.TabPages.FindByName("Registration");
                    page.Visible = false;
                    page = ASPxPageControl1.TabPages.FindByName("Group Member");
                    page.Visible = false;
                    page = ASPxPageControl1.TabPages.FindByName("Deposit");
                    page.Visible = false;
                    page = ASPxPageControl1.TabPages.FindByName("Remarks");
                    page.Visible = false;
                    page = ASPxPageControl1.TabPages.FindByName("Education");
                    page.Visible = false;
                    page = ASPxPageControl1.TabPages.FindByName("Trad. Prof.");
                    page.Visible = false;
                    page = ASPxPageControl1.TabPages.FindByName("Other");
                    page.Visible = false;
                    page = ASPxPageControl1.TabPages.FindByName("Subscription");
                    page.Visible = false;
                    page = ASPxPageControl1.TabPages.FindByName("General");
                    page.Visible = false;
                }
            }
        }
        public void BindGrid()
        {
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/store/Master/sProducts.aspx");
            string bldng = "";
            string verify = "";
            DataTable dt = new DataTable();
            DataTable dt1 = new DataTable();
            DataColumn col1 = new DataColumn("Id");
            DataColumn col2 = new DataColumn("Type");
            DataColumn col3 = new DataColumn("FileName");
            DataColumn col4 = new DataColumn("Src");
            DataColumn col5 = new DataColumn("FilePath");
            DataColumn col6 = new DataColumn("ReceiveDate");
            DataColumn col7 = new DataColumn("RenewDate");



            DataColumn col8 = new DataColumn("Bldng");
            DataColumn col9 = new DataColumn("Fileno");
            DataColumn col10 = new DataColumn("vrfy");
            DataColumn col11 = new DataColumn("Note1");
            DataColumn col12 = new DataColumn("Note2");
            DataColumn col13 = new DataColumn("createuser");
            DataColumn col14 = new DataColumn("doc");
            dt.Columns.Add(col1);
            dt.Columns.Add(col2);
            dt.Columns.Add(col3);
            dt.Columns.Add(col4);
            dt.Columns.Add(col5);
            dt.Columns.Add(col6);
            dt.Columns.Add(col7);
            dt.Columns.Add(col8);
            dt.Columns.Add(col9);
            dt.Columns.Add(col10);
            dt.Columns.Add(col11);
            dt.Columns.Add(col12);
            dt.Columns.Add(col13);
            dt.Columns.Add(col14);
            //if (Session["KeyVal_InternalID"] == "")
            //{
            if (Request.QueryString["idbldng"] != null)
            {
                if (Request.QueryString["idbldng"].ToString().Contains("^"))
                {
                    string[] two = Request.QueryString["idbldng"].Split('^');
                    Session["KeyVal_InternalID"] = two[1].ToString().Trim();
                }
                else
                {
                    Session["KeyVal_InternalID"] = Request.QueryString["idbldng"].ToString();
                }

                dt1 = oDBEngine.GetDataTable("tbl_master_document INNER JOIN tbl_master_documentType ON tbl_master_document.doc_documentTypeId = tbl_master_documentType.dty_id", "tbl_master_document.doc_id AS Id, tbl_master_documentType.dty_documentType AS Type,tbl_master_document.doc_documentName AS FileName, tbl_master_document.doc_source AS Src,doc_buildingid,doc_Floor,doc_RoomNo,doc_CellNo,doc_FileNo,convert(varchar,doc_receivedate,106)as ReceiveDate,convert(varchar,doc_renewdate,106)as RenewDate,(case when doc_verifydatetime is not null then ((select user_loginid from tbl_master_user where user_id=doc_verifyuser)  + '['+ convert(varchar,doc_verifydatetime,106)+']') else 'Not Verified' end) as vrfy,doc_Note1,doc_Note2,(select user_loginid from tbl_master_user where user_id=tbl_master_document.Createuser) as createuser, tbl_master_document.doc_documentTypeId as doc ", "doc_contactId='" + Session["KeyVal_InternalID"] + "'");
            }
            else
            {
                dt1 = oDBEngine.GetDataTable("tbl_master_document INNER JOIN tbl_master_documentType ON tbl_master_document.doc_documentTypeId = tbl_master_documentType.dty_id", "tbl_master_document.doc_id AS Id, tbl_master_documentType.dty_documentType AS Type,tbl_master_document.doc_documentName AS FileName, tbl_master_document.doc_source AS Src,doc_buildingid,doc_Floor,doc_RoomNo,doc_CellNo,doc_FileNo,convert(varchar,doc_receivedate,106)as ReceiveDate,convert(varchar,doc_renewdate,106)as RenewDate,(case when doc_verifydatetime is not null then ((select user_loginid from tbl_master_user where user_id=doc_verifyuser)  + '['+ convert(varchar,doc_verifydatetime,106)+']') else 'Not Verified' end) as vrfy,doc_Note1,doc_Note2,(select user_loginid from tbl_master_user where user_id=tbl_master_document.Createuser) as createuser, tbl_master_document.doc_documentTypeId as doc ", "doc_contactId='" + Session["KeyVal_InternalID"] + "'");
            }
            //}
            //else
            //{
            //    dt1 = oDBEngine.GetDataTable("tbl_master_document INNER JOIN tbl_master_documentType ON tbl_master_document.doc_documentTypeId = tbl_master_documentType.dty_id", "tbl_master_document.doc_id AS Id, tbl_master_documentType.dty_documentType AS Type,tbl_master_document.doc_documentName AS FileName, tbl_master_document.doc_source AS Src,doc_buildingid,doc_Floor,doc_RoomNo,doc_CellNo,doc_FileNo,convert(varchar,doc_receivedate,106)as ReceiveDate,convert(varchar,doc_renewdate,106)as RenewDate,(case when doc_verifydatetime is not null then ((select user_loginid from tbl_master_user where user_id=doc_verifyuser)  + '['+ convert(varchar,doc_verifydatetime,106)+']') else 'Not Verified' end) as vrfy,doc_Note1,doc_Note2,(select user_loginid from tbl_master_user where user_id=tbl_master_document.Createuser) as createuser ", "doc_contactId='" + Session["KeyVal_InternalID"] + "'");
            //}

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
                        if (dt1.Rows[i][9] != null)
                            RowNew["ReceiveDate"] = dt1.Rows[i][9].ToString();
                        else
                            RowNew["ReceiveDate"] = "";
                        if (dt1.Rows[i][10] != null)
                            RowNew["RenewDate"] = dt1.Rows[i][10].ToString();
                        else
                            RowNew["RenewDate"] = "";

                        string BName = "Floor No : " + dt1.Rows[i][5].ToString() + " " + "/ Room No-" + dt1.Rows[i][6].ToString() + " " + "/ Cabinet No-" + dt1.Rows[i][7].ToString() + " ";
                        RowNew["FilePath"] = BName;
                        RowNew["vrfy"] = dt1.Rows[i]["vrfy"].ToString();
                        RowNew["Fileno"] = dt1.Rows[i][8].ToString();
                        RowNew["bldng"] = "";
                        RowNew["Note1"] = dt1.Rows[i]["doc_Note1"].ToString();
                        RowNew["Note2"] = dt1.Rows[i]["doc_Note2"].ToString();
                        RowNew["createuser"] = dt1.Rows[i]["createuser"].ToString();
                        RowNew["doc"] = dt1.Rows[i]["doc"].ToString();
                        dt.Rows.Add(RowNew);

                    }
                    else
                    {
                        DataRow RowNew = dt.NewRow();
                        RowNew["Id"] = dt1.Rows[i][0].ToString();
                        RowNew["Type"] = dt1.Rows[i][1].ToString();
                        RowNew["FileName"] = dt1.Rows[i][2].ToString();
                        RowNew["Src"] = dt1.Rows[i][3].ToString();

                        if (dt1.Rows[i][9] != null)
                            RowNew["ReceiveDate"] = dt1.Rows[i][9].ToString();
                        else
                            RowNew["ReceiveDate"] = "";
                        if (dt1.Rows[i][10] != null)
                            RowNew["RenewDate"] = dt1.Rows[i][10].ToString();
                        else
                            RowNew["RenewDate"] = "";

                        string BuildingName = "";
                        string[,] bname1 = oDBEngine.GetFieldValue("tbl_master_building", "bui_name", " bui_id='" + dt1.Rows[i][4].ToString() + "'", 1);
                        if (bname1[0, 0] != "n")
                        {
                            BuildingName = bname1[0, 0];
                        }


                        RowNew["vrfy"] = dt1.Rows[i]["vrfy"].ToString();
                        RowNew["bldng"] = BuildingName;
                        string BName = "Floor No : " + dt1.Rows[i][5].ToString() + " " + "/ Room No-" + dt1.Rows[i][6].ToString() + " " + "/ Cabinet No-" + dt1.Rows[i][7].ToString() + " ";
                        RowNew["FilePath"] = BName;
                        RowNew["Fileno"] = dt1.Rows[i][8].ToString();
                        RowNew["Note1"] = dt1.Rows[i]["doc_Note1"].ToString();
                        RowNew["Note2"] = dt1.Rows[i]["doc_Note2"].ToString();
                        RowNew["createuser"] = dt1.Rows[i]["createuser"].ToString();
                        RowNew["doc"] = dt1.Rows[i]["doc"].ToString();
                        dt.Rows.Add(RowNew);
                    }
                }
            }
            EmployeeDocumentGrid.DataSource = dt.DefaultView;
            EmployeeDocumentGrid.DataBind();
        }
        protected void EmployeeDocumentGrid_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {

            string[] CallVal = e.Parameters.ToString().Split('~');
            if (CallVal[0].ToString() == "Delete")
            {
                oDBEngine.DeleteValue("tbl_master_document", " doc_id='" + CallVal[1].ToString() + "'");
                BindGrid();

                //oDBEngine.DeleteValue("Config_EmailAccounts ", "EmailAccounts_ID ='" + CallVal[1].ToString() + "'");
                //this.Page.ClientScript.RegisterStartupScript(GetType(), "script4", "<script>height();</script>");
                //fillGrid();

            }


        }
        protected void EmployeeDocumentGrid_HtmlRowCreated(object sender, ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType != DevExpress.Web.GridViewRowType.Data) return;
            int rowindex = e.VisibleIndex;
            string Verify = EmployeeDocumentGrid.GetRowValues(rowindex, "vrfy").ToString();
            string ContactID = e.GetValue("Src").ToString();
            string Rowid = e.GetValue("Id").ToString();
            if (Verify != "Not Verified")
            {
                DataTable dt = oDBEngine.GetDataTable("select doc_verifyremarks from tbl_master_document where doc_id=" + Rowid + "");
                string tooltip = dt.Rows[0][0].ToString();
                e.Row.Cells[0].Style.Add("cursor", "hand");
                e.Row.Cells[0].ToolTip = "View Document!";
                e.Row.Cells[0].Attributes.Add("onclick", "javascript:OnDocumentView('" + ContactID + "');");

                e.Row.Cells[1].Style.Add("cursor", "hand");
                e.Row.Cells[1].ToolTip = "View Document!";
                e.Row.Cells[1].Attributes.Add("onclick", "javascript:OnDocumentView('" + ContactID + "');");

                e.Row.Cells[2].Style.Add("cursor", "hand");
                e.Row.Cells[2].ToolTip = "View Document!";
                e.Row.Cells[2].Attributes.Add("onclick", "javascript:OnDocumentView('" + ContactID + "');");

                e.Row.Cells[3].Style.Add("cursor", "hand");
                e.Row.Cells[3].ToolTip = "View Document!";
                e.Row.Cells[3].Attributes.Add("onclick", "javascript:OnDocumentView('" + ContactID + "');");

                e.Row.Cells[4].Style.Add("cursor", "hand");
                e.Row.Cells[4].ToolTip = "View Document!";
                e.Row.Cells[4].Attributes.Add("onclick", "javascript:OnDocumentView('" + ContactID + "');");

                e.Row.Cells[5].Style.Add("cursor", "hand");
                e.Row.Cells[5].ToolTip = "View Document!";
                e.Row.Cells[5].Attributes.Add("onclick", "javascript:OnDocumentView('" + ContactID + "');");

                e.Row.Cells[6].Style.Add("cursor", "hand");
                e.Row.Cells[6].ToolTip = "View Document!";
                e.Row.Cells[6].Attributes.Add("onclick", "javascript:OnDocumentView('" + ContactID + "');");

                e.Row.Cells[7].Style.Add("cursor", "hand");
                e.Row.Cells[7].ToolTip = "View Document!";
                e.Row.Cells[7].Attributes.Add("onclick", "javascript:OnDocumentView('" + ContactID + "');");


                e.Row.Cells[8].Style.Add("cursor", "hand");
                e.Row.Cells[8].ToolTip = tooltip.ToString();
            }
            if (Verify == "Not Verified")
            {
                DataTable dt = oDBEngine.GetDataTable("select doc_verifyremarks from tbl_master_document where doc_id=" + Rowid + "");
                string tooltip = dt.Rows[0][0].ToString();
                //e.Row.Cells[0].Style.Add("cursor", "hand");
                //e.Row.Cells[0].ToolTip = "View Document!";
                //e.Row.Cells[0].Attributes.Add("onclick", "javascript:OnDocumentView('" + ContactID + "');");

                //e.Row.Cells[1].Style.Add("cursor", "hand");
                //e.Row.Cells[1].ToolTip = "View Document!";
                //e.Row.Cells[1].Attributes.Add("onclick", "javascript:OnDocumentView('" + ContactID + "');");

                //e.Row.Cells[2].Style.Add("cursor", "hand");
                //e.Row.Cells[2].ToolTip = "View Document!";
                //e.Row.Cells[2].Attributes.Add("onclick", "javascript:OnDocumentView('" + ContactID + "');");

                //e.Row.Cells[3].Style.Add("cursor", "hand");
                //e.Row.Cells[3].ToolTip = "View Document!";
                //e.Row.Cells[3].Attributes.Add("onclick", "javascript:OnDocumentView('" + ContactID + "');");

                //e.Row.Cells[4].Style.Add("cursor", "hand");
                //e.Row.Cells[4].ToolTip = "View Document!";
                //e.Row.Cells[4].Attributes.Add("onclick", "javascript:OnDocumentView('" + ContactID + "');");

                //e.Row.Cells[5].Style.Add("cursor", "hand");
                //e.Row.Cells[5].ToolTip = "View Document!";
                //e.Row.Cells[5].Attributes.Add("onclick", "javascript:OnDocumentView('" + ContactID + "');");

                //e.Row.Cells[6].Style.Add("cursor", "hand");
                //e.Row.Cells[6].ToolTip = "View Document!";
                //e.Row.Cells[6].Attributes.Add("onclick", "javascript:OnDocumentView('" + ContactID + "');");

                //e.Row.Cells[7].Style.Add("cursor", "hand");
                //e.Row.Cells[7].ToolTip = "View Document!";
                //e.Row.Cells[7].Attributes.Add("onclick", "javascript:OnDocumentView('" + ContactID + "');");

                e.Row.Cells[8].Style.Add("cursor", "Pointer");
                e.Row.Cells[8].ToolTip = "Click here to Verify !";
                e.Row.Cells[8].Attributes.Add("onclick", "javascript:Changestatus('" + Rowid + "');");
                e.Row.Cells[8].Style.Add("color", "Red");
            }
            //if (Verify.Contains("["))
            //{
            //    DataTable dt = oDBEngine.GetDataTable("select doc_verifyremarks from tbl_master_document where doc_id=" + Rowid + "");
            //    string tooltip = dt.Rows[0][0].ToString();

            //    e.Row.Cells[7].Style.Add("cursor", "hand");
            //    e.Row.Cells[7].ToolTip = tooltip.ToString();
            //    //e.Row.Cells[7].Attributes.Add("onclick", "javascript:Changestatus('" + Rowid + "');");
            //}
        }
        protected void EmployeeDocumentGrid_RowCommand(object sender, ASPxGridViewRowCommandEventArgs e)
        {

        }
    }
}