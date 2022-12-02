using BusinessLogicLayer;
using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ERP.OMS.Management.Master
{
    public partial class Team : System.Web.UI.Page
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            //SqlSchematype.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            //((GridViewDataComboBoxColumn)massBranch.Columns["pos_assignBranch"]).PropertiesComboBox.DataSource = LoadBranch();

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["LastCompany"] != null && Session["LastCompany"] != null)
                {
                    string userbranchHierachy = Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]);
                    PopulateBranchByHierchy(userbranchHierachy);
                   // cmbBranchfilter.SelectedIndex = Convert.ToInt32(Session["userbranchID"].ToString());
                }
                if (Request.QueryString["id"] != "ADD")
                {
                    PopulateTeamDetailsById(Request.QueryString["id"]);

                    Hidden_add_edit.Value = "edit";
                    Hidn_team_id.Value = Request.QueryString["id"];
                }

                else
                {
                    Hidden_add_edit.Value = "add";
                }


            }
        }

        [WebMethod]
        public static object GetAllUser(string id)
        {
            TeamDetails _TeamDetails = new TeamDetails();
            DataTable dt = new DataTable();
            // UserGroup _UserGroup = new UserGroup();
            try
            {
                ERPTeamBL objTeamlBL = new ERPTeamBL();
                dt = objTeamlBL.GetAllUser();
                _TeamDetails.msg = "true";
                if (dt != null && dt.Rows.Count > 0)
                {
                    _TeamDetails._usergroup = (from DataRow dr in dt.Rows
                                               select new UserGroup()
                                               {
                                                   id = dr["user_id"].ToString(),
                                                   name = dr["name"].ToString(),
                                               }).ToList();
                }
            }
            catch (Exception ex)
            {
                _TeamDetails.msg = ex.Message;
            }
            return _TeamDetails;
        }

        private void PopulateBranchByHierchy(string userbranchhierchy)
        {
            PosSalesInvoiceBl posSale = new PosSalesInvoiceBl();
            DataTable branchtable = posSale.getBranchListByHierchy(userbranchhierchy);
            cmbBranchfilter.DataSource = branchtable;
            cmbBranchfilter.ValueField = "branch_id";
            cmbBranchfilter.TextField = "branch_description";
            cmbBranchfilter.DataBind();
            cmbBranchfilter.SelectedIndex = 0;
        }

        [WebMethod]
        public static string save(Team_apply apply)
        {
            string output = string.Empty;

            int NoOfRowEffected = 0;

            DataTable user_dtls = new DataTable();
            try
            {
                user_dtls.Columns.Add("user_id", typeof(long));
                foreach (Users _user in apply.users_dtls)
                {
                    DataRow dr = user_dtls.NewRow();
                    dr["user_id"] = _user.value;
                    user_dtls.Rows.Add(dr);
                };

                if (HttpContext.Current.Session["userid"] != null)
                {
                    int user_id = Convert.ToInt32(HttpContext.Current.Session["userid"]);
                    ProcedureExecute proc = new ProcedureExecute("PRC_TEAMINSERT");
                    proc.AddVarcharPara("@BRANCH_ID", 25, Convert.ToString(apply.header.branchID.Trim()));
                    proc.AddVarcharPara("@TEAM_NAME", 100, Convert.ToString(apply.header.Team_name).Trim());
                    proc.AddVarcharPara("@DESCRIPTION", 500, Convert.ToString(apply.header.Description).Trim());
                    proc.AddIntegerPara("@cr_by", user_id);
                    proc.AddPara("@PARAMTABLE", user_dtls);
                    proc.AddVarcharPara("@Action", 30, Convert.ToString(apply.header.action).Trim());
                    proc.AddVarcharPara("@TEAM_ID", 25, Convert.ToString(apply.header.teamid));
                    proc.AddVarcharPara("@is_success", 500, "", QueryParameterDirection.Output);
                    NoOfRowEffected = proc.RunActionQuery();
                    output = Convert.ToString(proc.GetParaValue("@is_success"));
                    if (NoOfRowEffected > 0)
                    {
                        output = "true";
                    }
                }

            }
            catch (Exception ex)
            {
                output = ex.Message.ToString();
            }
            return output;
        }


        private void PopulateTeamDetailsById(string TeamID)
        {
            // List<CheckRights> _chkrights = new List<CheckRights>();
            string output = string.Empty;
            DataSet ds = new DataSet();
            try
            {
                if (HttpContext.Current.Session["userid"] != null)
                {
                    ProcedureExecute proc = new ProcedureExecute("PRC_TEAMINSERT");
                    proc.AddVarcharPara("@Action", 30, "select");
                    proc.AddIntegerPara("@TEAM_ID", Convert.ToInt32(TeamID.ToString().Trim()));
                    proc.AddVarcharPara("@is_success", 500, "", QueryParameterDirection.Output);
                    ds = proc.GetDataSet();
                    output = Convert.ToString(proc.GetParaValue("@is_success"));
                    if (output == "true")
                    {
                        if (ds != null && ds.Tables.Count > 0)
                        {
                            txt_Team_nm.Text = ds.Tables[0].Rows[0]["TEAM_NAME"].ToString().Trim();
                            txt_Team_nm.ClientEnabled = false;
                           // UserId.Value = ds.Tables[0].Rows[0]["group_id"].ToString().Trim();
                            txtDescription.InnerText = ds.Tables[0].Rows[0]["DESCRIPTIONS"].ToString().Trim();
                            cmbBranchfilter.Value =Convert.ToString(ds.Tables[0].Rows[0]["BRANCH_ID"].ToString().Trim());

                            if (ds.Tables[2] != null && ds.Tables[2].Rows.Count > 0)
                            {
                                list1.DataSource = ds.Tables[2];
                                list1.DataTextField = "user_name";
                                list1.DataValueField = "user_id";
                                list1.DataBind();
                            }

                            if (ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0)
                            {
                                list2.DataSource = ds.Tables[1];
                                list2.DataTextField = "user_name";
                                list2.DataValueField = "user_id";
                                list2.DataBind();
                            }
                        }
                    }
                    else
                    {

                    }
                }
            }
            catch (Exception ex)
            {
                output = ex.Message.ToString();
            }
        }
    }
}