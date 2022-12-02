using System;
using System.Data;
using System.Web.UI;
using System.Configuration;
using BusinessLogicLayer;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using BusinessLogicLayer.IMEI;
using System.Web;
using BusinessLogicLayer.WebMacAccess;
using EntityLayer.CommonELS;

namespace ERP.OMS.Management.Master
{
    public partial class WebMacaccessAdd : ERP.OMS.ViewState_class.VSPage
    {

        string data;
        static string BranchId;

        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);MULTI
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        Webmacaccess objimei = new Webmacaccess();
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        protected void Page_PreInit(object sender, EventArgs e) // lead add
        {
            if (!IsPostBack)
            {
                string sPath = Convert.ToString(HttpContext.Current.Request.Url);
                oDBEngine.Call_CheckPageaccessebility(sPath);

                //     Bind_BranchCombo();

            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/Management/Master/WebMacAccess.aspx");
            if (!IsPostBack)
            {

                getAllBranches();
                if (Request.QueryString["Macid"] != null)
                {
                    if (Convert.ToString(Request.QueryString["Macid"]) != "add")
                    {
                        hdncommaaccept.Value = "1";
                        GetValues(Convert.ToString(Request.QueryString["Macid"]));

                    }
                    else
                    {
                        hdncommaaccept.Value = "0";
                    }
                 
                }
            }

            //String cbReference = Page.ClientScript.GetCallbackEventReference(this, "arg", "ReceiveServerData", "context");
         //   String callbackScript = "function CallServer(arg, context){ " + cbReference + ";}";
          //  Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "CallServer", callbackScript, true);

        }
        public void getAllBranches()
        {
            DataTable dtbranches = new DataTable();

            dtbranches = objimei.GetBranchdetails();

            dtbranches = objimei.GetBranchdetails();
            lstBranches.DataSource = dtbranches;
            lstBranches.DataTextField = "branch_code";
            lstBranches.DataValueField = "branch_id";
            lstBranches.DataBind();
            lstBranches.Items.Insert(0,"Select Branch");

            if (Convert.ToString(Request.QueryString["Macid"]) != "add")
            {

            }

        }

        protected void ddlbranch_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstBranches.SelectedValue != "Select Branch")
            {
                Int32 Filter = int.Parse(Convert.ToString(lstBranches.SelectedValue));
                if (Filter != 0)
                {
                    DataTable dtuser = new DataTable();

                    GetuserData(dtuser, "User", Filter);
                }
            
            }
            else
            {

                drduser.DataSource = null;
                drduser.DataBind();
            }
        }

        public void GetuserData(DataTable dtuser,string Action,int BranchId)
        {

            dtuser = objimei.GetUser(BranchId, Action);
            if (dtuser.Rows.Count > 0)
            {

                drduser.DataSource = dtuser;
                drduser.DataTextField = "NameUser";
                drduser.DataValueField = "Id";
                drduser.DataBind();

            }
        }
        protected void GetValues(string bgid)
        {
            if (Convert.ToString(Request.QueryString["Macid"]) != "add")
            {
                DataTable dtuser = new DataTable();
                dtuser = objimei.GetModifydata(Convert.ToInt32(Request.QueryString["Macid"]), "Fetchmodify");

                if(dtuser.Rows.Count>0)
                {
                    lstBranches.SelectedValue = Convert.ToString(dtuser.Rows[0]["branch"]);
                    GetuserData(dtuser, "UserModify", Convert.ToInt32(lstBranches.SelectedValue));
                    drduser.SelectedValue = Convert.ToString(dtuser.Rows[0]["UserId"]);
                    txtimei.Text = Convert.ToString(dtuser.Rows[0]["Imei_No"]);
                }
            }

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string User = "";
            IWebmacclClass model = new IWebmacclClass();
            model.Imei = txtimei.Text.Trim();
            model.User = drduser.SelectedValue;
          
            if (Session["userid"] != null)
            {
                User = Convert.ToString(HttpContext.Current.Session["userid"]).Trim();
                model.CretemodifyBy = User;
            }

            if (Convert.ToString(Request.QueryString["Macid"]) == "add")
            {
                model.Action = "Insert";
            }
            else
            {
                model.Action = "Update";
                model.ImeiId = Convert.ToInt32(Request.QueryString["Macid"]);

            }
            int ret = objimei.InsertImei(model);
            if (ret > 0)
            {

                Page.ClientScript.RegisterStartupScript(GetType(), "confmsg", "<script language='javascript'>alert('IMEI Added Successfully !')</script>");
                //    Session["KeyVal"] = null;
                txtimei.Text = "";

                Response.Redirect("WebMacaccess.aspx");



            }
            else
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "confmsg", "<script language='javascript'>alert('IMEI already exists !')</script>");
                //    Session["KeyVal"] = null;
                txtimei.Text = "";

            }




        }

 
    }
}