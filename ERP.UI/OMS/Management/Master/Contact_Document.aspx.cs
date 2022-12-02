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
    public partial class management_Master_Contact_Document : ERP.OMS.ViewState_class.VSPage
    {
        //DBEngine oDBEngine = new DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]); MULTI
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();//For Connection string purpose

        public string pageAccess = "";
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //sas
                //'http://localhost:2957/InfluxCRM/management/testProjectMainPage_employee.aspx'
                string sPath = Convert.ToString(HttpContext.Current.Request.Url);
                oDBEngine.Call_CheckPageaccessebility(sPath);
              
                if (Session["querystring"] != null)
                {
                    Session["redirct"] = "frm_subledger.aspx" + Convert.ToString(Session["querystring"]);
                }
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
                if (Convert.ToString(Session["requesttype"]) == "Lead")
                {
                    rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/Master/frmContactMain.aspx?requesttype=Lead");
                }
                else if (Session["requesttype"] == "Branches")
                {
                    rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/Master/branch.aspx");
                }
                else if (Session["requesttype"] == "Building/Warehouses")
                {
                    rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/Master/RootBuilding.aspx");
                }
                else if (Session["requesttype"] == "Relationship Partners")
                {
                    rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/master/frmContactMain.aspx?requesttype=referalagent");
                }
                else if (Convert.ToString(Session["requesttype"]).Trim() == "Customer/Client")
                {
                    //rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/master/frmContactMain.aspx?requesttype=customer");
                    rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/master/CustomerMasterList.aspx");
                }
                else if (Convert.ToString(Session["requesttype"]).Trim() == "Salesman/Agents")
                {
                    rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/master/frmContactMain.aspx?requesttype=agent");
                }
                else if (Convert.ToString(Session["requesttype"]).Trim() == "Franchisee")
                {
                    rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/master/frmContactMain.aspx?requesttype=franchisee");
                }
                else if (Convert.ToString(Session["requesttype"]).Trim() == "Partner")
                {
                    rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/master/frmContactMain.aspx?requesttype=partner");
                }
                else if (Convert.ToString(Session["requesttype"]).Trim() == "Relationship Partners")
                {
                    rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/master/frmContactMain.aspx?requesttype=referalagent");
                }
                else if (Convert.ToString(Session["requesttype"]).Trim() == "Consultant")
                {
                    rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/master/frmContactMain.aspx?requesttype=consultant");
                }               
                else if (Convert.ToString(Session["requesttype"]).Trim() == "Relationship Manager")
                {
                    rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/master/frmContactMain.aspx?requesttype=agent");
                }

                else if (Convert.ToString(Session["requesttype"]).Trim() == "Account Heads")
                {
                    rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/Master/AccountGroup.aspx");
                }



                else if (Convert.ToString(Session["requesttype"]).Trim() == "Sub Ledger")
                {
                    rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/Master/AccountGroup.aspx");
                }
                else if (Convert.ToString(Session["requesttype"]).Trim() == "Transporter")
                {
                    //rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/master/frmContactMain.aspx?requesttype=Transporter");
                    rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/master/TransporterMasterList.aspx?requesttype=Transporter");
                }
            
           
            if (Session["requesttype"] != null)
            {
                lblHeadTitle.Text = Convert.ToString(Session["requesttype"]) + " Document";
            }
            if (Convert.ToString(Session["requesttype"]) == "Lead")
            {
                TabPage pageFM = ASPxPageControl1.TabPages.FindByName("FamilyMembers");
                pageFM.Visible = true;
            }
           

            Session["KeyVal2"] = null;

         

            if (Session["Name"] != null)
            {
                lblName.Text = Session["Name"].ToString();
            }
            else if (Session["CompanyName"] != null)
            {
                lblName.Text = "Company Name :" + "  " + Session["CompanyName"].ToString();
            }
            BindGrid();

            if (Request.QueryString["Page"] != null)
            {
                IsredirectedBranch.Value = "1";
            }
            else
            {
                IsredirectedBranch.Value = "0";
            }


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
                
                if (reqsttype == "Companies" || reqsttype == "Branches" || reqsttype == "Exchanges" || reqsttype == "Building/Warehouses" || reqsttype == "Account Heads" || reqsttype == "Sub Ledger")
                {
                    TabPage page = ASPxPageControl1.TabPages.FindByName("CorresPondence");
                    page.Visible = false;   
                    if (reqsttype == "Branches")
                    {
                        page.Visible = true;
                    }
                    
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
                   
                    if (reqsttype == "Companies" || reqsttype == "Branches")          
                    {
                        page = ASPxPageControl1.TabPages.FindByName("Remarks");
                        page.Visible = false;
                    }
                   
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
                if (reqsttype == "Account Heads") 
                {
                    TabPage page = ASPxPageControl1.TabPages.FindByName("General");                   
                    page.Visible = false;
                }
                if (reqsttype == "Transporter")
                {
                    TabPage page = ASPxPageControl1.TabPages.FindByName("Group Member");                   
                    page.Visible = false;
                }
                if (reqsttype == "Sub Ledger")
                {
                    TabPage page = ASPxPageControl1.TabPages.FindByName("General");                 
                    page.Visible = false;
                }
                if (reqsttype == "Product")
                {
                    TabPage page = ASPxPageControl1.TabPages.FindByName("General");
                    page.Visible = false;
                    page = ASPxPageControl1.TabPages.FindByName("CorresPondence");
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
                   
                    
                }
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
            
            if (Request.QueryString["idbldng"] != null)
            {
                if (Convert.ToString(Request.QueryString["idbldng"]).Contains("^"))
                {
                    string[] two = Request.QueryString["idbldng"].Split('^');
                    Session["KeyVal_InternalID"] = Convert.ToString(two[1]).Trim();
                }
                else
                {
                    Session["KeyVal_InternalID"] = Convert.ToString(Request.QueryString["idbldng"]);
                }

                dt1 = oDBEngine.GetDataTable("tbl_master_document INNER JOIN tbl_master_documentType ON tbl_master_document.doc_documentTypeId = tbl_master_documentType.dty_id", "tbl_master_document.doc_id AS Id, tbl_master_documentType.dty_documentType AS Type,tbl_master_document.doc_documentName AS FileName, tbl_master_document.doc_source AS Src,doc_buildingid,doc_Floor,doc_RoomNo,doc_CellNo,doc_FileNo,convert(varchar,doc_receivedate,106)as ReceiveDate,convert(varchar,doc_renewdate,106)as RenewDate,(case when doc_verifydatetime is not null then ((select user_loginid from tbl_master_user where user_id=doc_verifyuser)  + '['+ convert(varchar,doc_verifydatetime,106)+']') else 'Not Verified' end) as vrfy,doc_Note1,doc_Note2,(select user_loginid from tbl_master_user where user_id=tbl_master_document.Createuser) as createuser, tbl_master_document.doc_documentTypeId as doc ", "doc_contactId='" + Session["KeyVal_InternalID"] + "'");
            }
            else
            {
                if (Request.QueryString["Page"] != null)
                {
                    if(Convert.ToString(Request.QueryString["Page"])=="branch")
                    {
                        dt1 = oDBEngine.GetDataTable("tbl_master_document INNER JOIN tbl_master_documentType ON tbl_master_document.doc_documentTypeId = tbl_master_documentType.dty_id", "tbl_master_document.doc_id AS Id, tbl_master_documentType.dty_documentType AS Type,tbl_master_document.doc_documentName AS FileName, tbl_master_document.doc_source AS Src,doc_buildingid,doc_Floor,doc_RoomNo,doc_CellNo,doc_FileNo,convert(varchar,doc_receivedate,106)as ReceiveDate,convert(varchar,doc_renewdate,106)as RenewDate,(case when doc_verifydatetime is not null then ((select user_loginid from tbl_master_user where user_id=doc_verifyuser)  + '['+ convert(varchar,doc_verifydatetime,106)+']') else 'Not Verified' end) as vrfy,doc_Note1,doc_Note2,(select user_loginid from tbl_master_user where user_id=tbl_master_document.Createuser) as createuser, tbl_master_document.doc_documentTypeId as doc ", "doc_contactId='" + Session["branch_InternalId"] + "'");
                          
                    }
                  
                }
                else
                {

                    dt1 = oDBEngine.GetDataTable("tbl_master_document INNER JOIN tbl_master_documentType ON tbl_master_document.doc_documentTypeId = tbl_master_documentType.dty_id", "tbl_master_document.doc_id AS Id, tbl_master_documentType.dty_documentType AS Type,tbl_master_document.doc_documentName AS FileName, tbl_master_document.doc_source AS Src,doc_buildingid,doc_Floor,doc_RoomNo,doc_CellNo,doc_FileNo,convert(varchar,doc_receivedate,106)as ReceiveDate,convert(varchar,doc_renewdate,106)as RenewDate,(case when doc_verifydatetime is not null then ((select user_loginid from tbl_master_user where user_id=doc_verifyuser)  + '['+ convert(varchar,doc_verifydatetime,106)+']') else 'Not Verified' end) as vrfy,doc_Note1,doc_Note2,(select user_loginid from tbl_master_user where user_id=tbl_master_document.Createuser) as createuser, tbl_master_document.doc_documentTypeId as doc ", "doc_contactId='" + Session["KeyVal_InternalID"] + "'");
                }
            }
         
           

            if (dt1.Rows.Count != 0)
            {
                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    if (Convert.ToString(dt1.Rows[i][4]) == "0")
                    {
                        DataRow RowNew = dt.NewRow();
                        RowNew["Id"] = Convert.ToString(dt1.Rows[i][0]);
                        RowNew["Type"] = Convert.ToString(dt1.Rows[i][1]);
                        RowNew["FileName"] = Convert.ToString(dt1.Rows[i][2]);
                        RowNew["Src"] = Convert.ToString(dt1.Rows[i][3]);
                        if (dt1.Rows[i][9] != null)
                            RowNew["ReceiveDate"] = Convert.ToString(dt1.Rows[i][9]);
                        else
                            RowNew["ReceiveDate"] = "";
                        if (dt1.Rows[i][10] != null)
                            RowNew["RenewDate"] = Convert.ToString(dt1.Rows[i][10]);
                        else
                            RowNew["RenewDate"] = "";

                        string BName = "Floor No : " + Convert.ToString(dt1.Rows[i][5]) + " " + "/ Room No-" + Convert.ToString(dt1.Rows[i][6]) + " " + "/ Cabinet No-" + Convert.ToString(dt1.Rows[i][7]) + " ";
                        RowNew["FilePath"] = BName;
                        RowNew["vrfy"] = Convert.ToString(dt1.Rows[i]["vrfy"]);
                        RowNew["Fileno"] = Convert.ToString(dt1.Rows[i][8]);
                        RowNew["bldng"] = "";
                        RowNew["Note1"] = Convert.ToString(dt1.Rows[i]["doc_Note1"]);
                        RowNew["Note2"] = Convert.ToString(dt1.Rows[i]["doc_Note2"]);
                        RowNew["createuser"] = Convert.ToString(dt1.Rows[i]["createuser"]);
                        RowNew["doc"] = Convert.ToString(dt1.Rows[i]["doc"]);
                        dt.Rows.Add(RowNew);

                    }
                    else
                    {
                        DataRow RowNew = dt.NewRow();
                        RowNew["Id"] = Convert.ToString(dt1.Rows[i][0]);
                        RowNew["Type"] = Convert.ToString(dt1.Rows[i][1]);
                        RowNew["FileName"] = Convert.ToString(dt1.Rows[i][2]);
                        RowNew["Src"] = Convert.ToString(dt1.Rows[i][3]);

                        if (dt1.Rows[i][9] != null)
                            RowNew["ReceiveDate"] = Convert.ToString(dt1.Rows[i][9]);
                        else
                            RowNew["ReceiveDate"] = "";
                        if (dt1.Rows[i][10] != null)
                            RowNew["RenewDate"] = Convert.ToString(dt1.Rows[i][10]);
                        else
                            RowNew["RenewDate"] = "";

                        string BuildingName = "";
                        string[,] bname1 = oDBEngine.GetFieldValue("tbl_master_building", "bui_name", " bui_id='" + Convert.ToString(dt1.Rows[i][4]) + "'", 1);
                        if (bname1[0, 0] != "n")
                        {
                            BuildingName = bname1[0, 0];
                        }


                        RowNew["vrfy"] = Convert.ToString(dt1.Rows[i]["vrfy"]);
                        RowNew["bldng"] = BuildingName;
                        string BName = "Floor No : " + Convert.ToString(dt1.Rows[i][5]) + " " + "/ Room No-" + Convert.ToString(dt1.Rows[i][6]) + " " + "/ Cabinet No-" + Convert.ToString(dt1.Rows[i][7]) + " ";
                        RowNew["FilePath"] = BName;
                        RowNew["Fileno"] = Convert.ToString(dt1.Rows[i][8]);
                        RowNew["Note1"] = Convert.ToString(dt1.Rows[i]["doc_Note1"]);
                        RowNew["Note2"] = Convert.ToString(dt1.Rows[i]["doc_Note2"]);
                        RowNew["createuser"] = Convert.ToString(dt1.Rows[i]["createuser"]);
                        RowNew["doc"] = Convert.ToString(dt1.Rows[i]["doc"]);
                        dt.Rows.Add(RowNew);
                    }
                }
            }
            EmployeeDocumentGrid.DataSource = dt.DefaultView;
            EmployeeDocumentGrid.DataBind();
        }

        protected void EmployeeDocumentGrid_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {

            string[] CallVal = Convert.ToString(e.Parameters).Split('~');
            if (Convert.ToString(CallVal[0]) == "Delete")
            {
                oDBEngine.DeleteValue("tbl_master_document", " doc_id='" + Convert.ToString(CallVal[1]) + "'");
                BindGrid();

                
            }


        }
  
        protected void EmployeeDocumentGrid_HtmlRowCreated(object sender, ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType != DevExpress.Web.GridViewRowType.Data) return;
            int rowindex = e.VisibleIndex;
            string Verify = Convert.ToString(EmployeeDocumentGrid.GetRowValues(rowindex, "vrfy"));
            string ContactID = Convert.ToString(e.GetValue("Src"));
            string Rowid = Convert.ToString(e.GetValue("Id"));
            if (Verify != "Not Verified")
            {
                DataTable dt = oDBEngine.GetDataTable("select doc_verifyremarks from tbl_master_document where doc_id=" + Rowid + "");
                string tooltip = Convert.ToString(dt.Rows[0][0]);
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
                e.Row.Cells[8].ToolTip = Convert.ToString(tooltip);
            }
            if (Verify == "Not Verified")
            {
                DataTable dt = oDBEngine.GetDataTable("select doc_verifyremarks from tbl_master_document where doc_id=" + Rowid + "");
                string tooltip = Convert.ToString(dt.Rows[0][0]);
                

                e.Row.Cells[8].Style.Add("cursor", "pointer");
                e.Row.Cells[8].ToolTip = "Click here to Verify !";
                e.Row.Cells[8].Attributes.Add("onclick", "javascript:Changestatus('" + Rowid + "');");
                e.Row.Cells[8].Style.Add("color", "Red");
            }
           
        }

        protected void EmployeeDocumentGrid_RowCommand(object sender, ASPxGridViewRowCommandEventArgs e)
        {

        }

      

       
    }
}