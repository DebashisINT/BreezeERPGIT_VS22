using System;
using System.Data;
using System.Web.UI;
using System.Configuration;
using BusinessLogicLayer;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using BusinessLogicLayer.IMEI;
using System.Web;
using BusinessLogicLayer.SalesmanAddress;
using System.Net;
using System.IO;
using System.Text;

namespace ERP.OMS.Management.Master
{
    public partial class Salesman_Address : ERP.OMS.ViewState_class.VSPage
    {

        string data;
        static string BranchId;

        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        SalesmanAddress objimei = new SalesmanAddress();

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

            if (!IsPostBack)
            {

                getAllBranches();
                getAllStates();
                if (Request.QueryString["Addid"] != null)
                {
                    if (Convert.ToString(Request.QueryString["Addid"]) != "add")
                    {
                        GetValues(Convert.ToString(Request.QueryString["Addid"]));
                        hdncommaaccept.Value = "1";
                    }
                    else
                    {

                        hdncommaaccept.Value = "0";
                    }
                }
            }

        }
        public void getAllBranches()
        {
            DataTable dtbranches = new DataTable();

            dtbranches = objimei.GetBranchdetails();

            lstBranches.DataSource = dtbranches;
            lstBranches.DataTextField = "branch_code";
            lstBranches.DataValueField = "branch_id";
            lstBranches.DataBind();
            lstBranches.Items.Insert(0, "Select Branch");

            if (Convert.ToString(Request.QueryString["Addid"]) != "add")
            {

            }


        }

        public void getAllStates()
        {
            DataTable dtbranches = new DataTable();

            dtbranches = objimei.GetStatesdetails();

            ddlstate.DataSource = dtbranches;
            ddlstate.DataTextField = "Statename";
            ddlstate.DataValueField = "id";
            ddlstate.DataBind();
            //ddlstate.Items.Insert(0, "Select States");

        }



        protected void ddlbranch_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstBranches.SelectedValue != "Select Branch")
            {
                Int32 Filter = int.Parse(Convert.ToString(lstBranches.SelectedValue));
                if (Filter != 0)
                {
                    DataTable dtuser = new DataTable();

                    GetuserData(dtuser, "User", Filter, "0");
                }

            }
            else
            {

                drduser.DataSource = null;
                drduser.DataBind();
            }
        }

        public void GetuserData(DataTable dtuser, string Action, int BranchId, string PId)
        {

            dtuser = objimei.GetUser(BranchId, Action, PId);
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
            if (Convert.ToString(Request.QueryString["Addid"]) != "add")
            {
                DataTable dtuser = new DataTable();
                dtuser = objimei.GetModifydata(Convert.ToInt32(Request.QueryString["Addid"]), "Fetchmodify");

                if (dtuser.Rows.Count > 0)
                {
                    lstBranches.SelectedValue = Convert.ToString(dtuser.Rows[0]["branch"]);
                    GetuserData(dtuser, "UserModify", Convert.ToInt32(lstBranches.SelectedValue), Convert.ToString(Request.QueryString["Addid"]));
                    drduser.SelectedValue = Convert.ToString(dtuser.Rows[0]["UserId"]);
                    txtaddress.Text = Convert.ToString(dtuser.Rows[0]["Address"]);
                    txtlat.Text = Convert.ToString(dtuser.Rows[0]["Latitude"]);
                    txtlong.Text = Convert.ToString(dtuser.Rows[0]["longitude"]);
                    ddlstate.SelectedValue = Convert.ToString(dtuser.Rows[0]["stateid"]);
                }
            }

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string User = "";
            UserAddressClass model = new UserAddressClass();
            model.Address = txtaddress.Text.Trim();
            model.User = drduser.SelectedValue;
            model.latitude = txtlat.Text.Trim();
            model.longitude = txtlong.Text.Trim();
            model.stateId = ddlstate.SelectedValue;
            if (Session["userid"] != null)
            {
                User = Convert.ToString(HttpContext.Current.Session["userid"]).Trim();
                model.CretemodifyBy = User;
            }

            if (Convert.ToString(Request.QueryString["Addid"]) == "add")
            {
                model.Action = "Insert";
            }
            else
            {
                model.Action = "Update";
                model.UserAddressId = Convert.ToInt32(Request.QueryString["Addid"]);

            }
            int ret = objimei.InsertSalesManAddress(model);
            if (ret > 0)
            {

                Page.ClientScript.RegisterStartupScript(GetType(), "confmsg", "<script language='javascript'>alert('IMEI Added Successfully !')</script>");
                //    Session["KeyVal"] = null;
                txtaddress.Text = "";
                txtlat.Text = "";
                txtlong.Text = "";
                Response.Redirect("Salesman-AddressList.aspx");

            }
            else
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "confmsg", "<script language='javascript'>alert('User Address already exists !')</script>");
                //    Session["KeyVal"] = null;
                txtaddress.Text = "";
                txtlat.Text = "";
                txtlong.Text = "";

            }




        }

        protected void txtaddress_TextxChanged(object sender, EventArgs e)
        {
            DataTable dtlatlong = Getlatlong(txtaddress.Text);
            if(dtlatlong.Rows.Count>0)
            {

                txtlat.Text = Convert.ToString(dtlatlong.Rows[0]["Latitude"]);

                txtlong.Text = Convert.ToString(dtlatlong.Rows[0]["Longitude"]);
            }
        }
        public DataTable Getlatlong(string address)
        {
            string url = "http://maps.google.com/maps/api/geocode/xml?address=" + address + "&sensor=false";
            WebRequest request = WebRequest.Create(url);
            DataTable dtCoordinates = new DataTable();

            using (WebResponse response = (HttpWebResponse)request.GetResponse())
            {
                using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    DataSet dsResult = new DataSet();
                    dsResult.ReadXml(reader);
                    dtCoordinates.Columns.AddRange(new DataColumn[4] { new DataColumn("Id", typeof(int)),
                    new DataColumn("Address", typeof(string)),
                    new DataColumn("Latitude",typeof(string)),
                    new DataColumn("Longitude",typeof(string)) });
                    if (dsResult.Tables["result"] !=null)
                    {
                        foreach (DataRow row in dsResult.Tables["result"].Rows)
                        {
                            string geometry_id = dsResult.Tables["geometry"].Select("result_id = " + row["result_id"].ToString())[0]["geometry_id"].ToString();
                            DataRow location = dsResult.Tables["location"].Select("geometry_id = " + geometry_id)[0];
                            dtCoordinates.Rows.Add(row["result_id"], row["formatted_address"], location["lat"], location["lng"]);
                        }
                    }
                }
                return dtCoordinates;
            }
        }
    }
}