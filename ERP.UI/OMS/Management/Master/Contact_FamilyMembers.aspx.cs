using System;
using System.Web;
using System.Web.UI;
////using DevExpress.Web.ASPxTabControl;
using DevExpress.Web;
using DevExpress.Web;
using BusinessLogicLayer;
using System.Configuration;

namespace ERP.OMS.Management.Master
{
    public partial class management_master_Contact_FamilyMembers : ERP.OMS.ViewState_class.VSPage
    {
        public string pageAccess = "";
        string pageVisibility = "";
        //  DBEngine oDBEngine = new DBEngine(string.Empty);
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        //Bellow Code added by debjyoti 16-11-2016
        public static EntityLayer.CommonELS.UserRightsForPage rights;

        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //'http://localhost:2957/InfluxCRM/management/testProjectMainPage_employee.aspx'
                string sPath = Convert.ToString(HttpContext.Current.Request.Url);
                oDBEngine.Call_CheckPageaccessebility(sPath);


                if (Request.QueryString["id"] != null)
                {
                    string id = Convert.ToString(Request.QueryString["ID"]);
                    Session["KeyVal_InternalID"] = id;
                    Session["KeyVal"] = "N";
                    pageVisibility = "N";

                }
            }
        }
        protected void Page_Init(object sender, EventArgs e)
        {
            SelectRelation.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // bellow code added by debjyoti 16-11-2016
            
            if (Session["requesttype"] != null)
            {
                if (Convert.ToString(Session["requesttype"]).Trim() == "Lead")
                {
                    rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/master/frmContactMain.aspx?requesttype=Lead");
                }
            }
            //End 16-11-2016 debjyoti
            //------- For Read Only User in SQL Datasource Connection String   Start-----------------

            if (HttpContext.Current.Session["EntryProfileType"] != null)
            {
                if (Convert.ToString(HttpContext.Current.Session["EntryProfileType"]) == "R")
                {
                    //FamilyMemberData.ConnectionString = ConfigurationSettings.AppSettings["DBReadOnlyConnection"];MULTI
                    FamilyMemberData.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                }
                else
                {
                    //FamilyMemberData.ConnectionString = ConfigurationSettings.AppSettings["DBConnectionDefault"];MULTI
                    FamilyMemberData.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                }
            }

            //------- For Read Only User in SQL Datasource Connection String   End-----------------

            if (HttpContext.Current.Session["userid"] == null)
            {
               //Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
            if (!IsPostBack)
            {
                if (Session["Name"] != null)
                {
                    lblName.Text = Convert.ToString(Session["Name"]);
                }
                if (pageVisibility == "N")
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

        //Purpose: User rights in  family member list grid
        //Name : Debjyoti 16-11-2016

        protected void FamilyMemberGrid_CommandButtonInitialize(object sender, ASPxGridViewCommandButtonEventArgs e)
        {
            if (!rights.CanDelete)
            {
                if (e.ButtonType == ColumnCommandButtonType.Delete)
                {
                    e.Visible = false;
                }
            }


            if (!rights.CanEdit)
            {
                if (e.ButtonType == ColumnCommandButtonType.Edit)
                {
                    e.Visible = false;
                }
            }

        }

        protected void FamilyMemberGrid_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            FamilyMemberGrid.SettingsText.PopupEditFormCaption = "Modify Family Relationship";
        }
        //End : debjyoti 16-11-2016

    }
}
