using System;
using System.Web;
using DevExpress.Web;
using System.Configuration;
using BusinessLogicLayer;
using EntityLayer.CommonELS;

namespace ERP.OMS.Management.Master
{
    public partial class Contact_Person : System.Web.UI.Page
    {
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();

        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {               
                string sPath = Convert.ToString(HttpContext.Current.Request.Url);              
                oDBEngine.Call_CheckPageaccessebility(sPath);
            }
        }
        protected void Page_Init(object sender, EventArgs e)
        {
            SqlDataSource1.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            SqlDesignation.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            SqlFamRelationShip.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
           
            if (Session["requesttype"] != null)
            {
                if (Convert.ToString(Session["requesttype"]).Trim() == "Lead")
                {
                    rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/master/frmContactMain.aspx?requesttype=Lead");
                }
                else if (Convert.ToString(Session["requesttype"]).Trim() == "Customer/Client")
                {                  
                    rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/master/CustomerMasterList.aspx");
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
                else if (Convert.ToString(Session["requesttype"]).Trim() == "Share Holder")
                {
                    rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/master/frmContactMain.aspx?requesttype=shareholder");
                }
                else if (Convert.ToString(Session["requesttype"]).Trim() == "Debtor")
                {
                    rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/master/frmContactMain.aspx?requesttype=debtor");
                }
                else if (Convert.ToString(Session["requesttype"]).Trim() == "Creditors")
                {
                    rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/master/frmContactMain.aspx?requesttype=creditor");
                }
                else if (Convert.ToString(Session["requesttype"]).Trim() == "Relationship Manager")
                {
                    rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/master/frmContactMain.aspx?requesttype=agent");
                }
                else if (Convert.ToString(Session["requesttype"]).Trim() == " Salesman/Agents")
                {
                    rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/master/frmContactMain.aspx?requesttype=agent");
                }
                else if (Convert.ToString(Session["requesttype"]).Trim() == "Transporter")
                {                    
                    rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/master/TransporterMasterList.aspx?requesttype=Transporter");
                }
                else if (Convert.ToString(Session["requesttype"]).Trim() == "Branches")
                {
                    rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/Master/branch.aspx");
                }
                else
                {
                    rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/master/Contact_Correspondence.aspx");
                }
            }       

            if (Session["requesttype"] != null)
            {
                lblHeadTitle.Text = Convert.ToString(Session["requesttype"]) + " Contact Person List";
            }          
            string cnttype = "";
            if (Session["ContactType"] != null)
            {
                cnttype = Convert.ToString(Session["ContactType"]);
            }
            string intid = "";
            if (Request.QueryString["Page"] != null)
            {
                string page = Convert.ToString(Request.QueryString["Page"]);
                if (page == "branch")
                {
                    if (Session["branch_InternalId"] != null)
                    {
                        intid = Convert.ToString(Session["branch_InternalId"]);
                    }
                }
            }
            else
            {
                if (Session["KeyVal_InternalID"] != null)
                {
                    intid = Convert.ToString(Session["KeyVal_InternalID"]);
                }
            }
            if (HttpContext.Current.Session["userid"] == null)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
            if (!IsPostBack)
            { 
                if (Session["Name"] != null)
                {
                    lblName.Text = Convert.ToString(Session["Name"]);
                }             
            }
            if (Convert.ToString(Session["requesttype"]) == "Lead")
            {
                TabPage pageFM = ASPxPageControl1.TabPages.FindByName("FamilyMembers");
                pageFM.Visible = true;
            }
            if (cnttype == "Branches")
            {
                TabPage page = ASPxPageControl1.TabPages.FindByName("Correspondence");
                page.Visible = true;
            }

            SalesVisibleContact();

         

            if (cnttype == "OtherEntity")
            {
                TabPage page = ASPxPageControl1.TabPages.FindByName("ContactPreson");
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
                page = ASPxPageControl1.TabPages.FindByName("Correspondence");
                page.Visible = false;
            }
            if (cnttype == "Transporter")
            {
                TabPage page = ASPxPageControl1.TabPages.FindByName("Group Member");
                page.Visible = false;
            }         
        }
        public void SalesVisibleContact()
        {         
            if (Session["ContactType"] == null)
            {
                Session["ContactType"] = "Lead";
            }
            try
            {
                if (Request.QueryString["formtype"] != null)
                {
                    string ID = Convert.ToString(Session["InternalId"]);
                    Session["KeyVal_InternalID_New"] = Convert.ToString(ID);
                  
                    
                    TabPage page = ASPxPageControl1.TabPages.FindByName("Documents");
                    page.Visible = false;
                    page = ASPxPageControl1.TabPages.FindByName("General");
                    page.Visible = false;
                    page = ASPxPageControl1.TabPages.FindByName("Correspondence");
                    page.Visible = false;
                    page = ASPxPageControl1.TabPages.FindByName("Bank Details");
                    page.Visible = false;
                    page = ASPxPageControl1.TabPages.FindByName("DP Details");
                    page.Visible = false;
                    page = ASPxPageControl1.TabPages.FindByName("Documents");
                    page.Visible = false;
                    page = ASPxPageControl1.TabPages.FindByName("Family Members");
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
                  
                }
                else
                {


                    TabPage page1 = ASPxPageControl1.TabPages.FindByName("ContactPreson");
                    page1.Visible = true;
                  
                    if (Session["KeyVal_InternalID"] != null)
                    {
                        string ID = Convert.ToString(Session["KeyVal_InternalID"]);
                        Session["KeyVal_InternalID_New"] = Convert.ToString(ID);                      
                    }
                    else
                    {
                        if (Request.QueryString["requesttypeP"] != null)
                        {
                            string ID = Convert.ToString(Session["LeadId"]);
                            Session["KeyVal_InternalID_New"] = Convert.ToString(ID);
                           
                            TabPage page = ASPxPageControl1.TabPages.FindByName("Documents");
                            page.Visible = true;
                            page = ASPxPageControl1.TabPages.FindByName("General");
                            page.Visible = true;
                            page = ASPxPageControl1.TabPages.FindByName("Correspondence");
                            page.Visible = true;
                            page = ASPxPageControl1.TabPages.FindByName("Bank Details");
                            page.Visible = true;
                            page = ASPxPageControl1.TabPages.FindByName("DP Details");
                            page.Visible = true;
                            page = ASPxPageControl1.TabPages.FindByName("Documents");
                            page.Visible = true;
                            page = ASPxPageControl1.TabPages.FindByName("Family Members");
                            page.Visible = true;
                            page = ASPxPageControl1.TabPages.FindByName("Registration");
                            page.Visible = true;
                            page = ASPxPageControl1.TabPages.FindByName("Group Member");
                            page.Visible = true;
                            page = ASPxPageControl1.TabPages.FindByName("Deposit");
                            page.Visible = true;
                            page = ASPxPageControl1.TabPages.FindByName("Remarks");
                            page.Visible = true;
                            page = ASPxPageControl1.TabPages.FindByName("Education");
                            page.Visible = true;
                            page = ASPxPageControl1.TabPages.FindByName("Trad. Prof.");
                            page.Visible = true;
                            page = ASPxPageControl1.TabPages.FindByName("Other");
                            page.Visible = true;

                        }
                    }
                }
            }
            catch
            {
            }           
        }
        protected void GridContactPerson_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            if (e.DataColumn.FieldName == "status")
            {
                if (e.CellValue.Equals("Suspended"))
                    e.Cell.BackColor = System.Drawing.Color.LightGray;
            }
        }
        protected void GridContactPerson_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
        {
            if (GridContactPerson.IsNewRowEditing)
            {
                if (e.Column.FieldName == "cp_status")
                {
                    ASPxComboBox cmb = e.Editor as ASPxComboBox;
                    cmb.SelectedIndex = 0;  //or another code that allows to set selected index/value according to your requirements
                }
            }
        }

        protected void drdExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 Filter = int.Parse(Convert.ToString(drdExport.SelectedItem.Value));
            if (Filter != 0)
            {
                if (Session["exportval"] == null)
                {
                    Session["exportval"] = Filter;
                    bindexport(Filter);
                }
                else if (Convert.ToInt32(Session["exportval"]) != Filter)
                {
                    Session["exportval"] = Filter;
                    bindexport(Filter);
                }
            }
        }
        public void bindexport(int Filter)
        {
            string filename = "Recruitment Agents Details (Contact Person)";
            exporter.FileName = filename;

            exporter.PageHeader.Left = "Recruitment Agents Details (Contact Person)";
            exporter.PageFooter.Center = "[Page # of Pages #]";
            exporter.PageFooter.Right = "[Date Printed]";

            switch (Filter)
            {
                case 1:
                    exporter.WritePdfToResponse();
                    break;
                case 2:
                    exporter.WriteXlsToResponse();
                    break;
                case 3:
                    exporter.WriteRtfToResponse();
                    break;
                case 4:
                    exporter.WriteCsvToResponse();
                    break;
            }
        }
        protected void ASPxPageControl1_ActiveTabChanged(object source, TabControlEventArgs e)
        {
        }
    }
}