using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;

namespace ERP.OMS.Management.DashboardSetting.service
{
    /// <summary>
    /// Summary description for DashBoard
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    [System.Web.Script.Services.ScriptService]
    public class DashBoard : System.Web.Services.WebService
    {

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetUser(string _searchkey)
        {
            List<UserGroup> _UserGroup = new List<UserGroup>();
            DataTable dt = new DataTable();
            string output = string.Empty;
            try
            {
                if (HttpContext.Current.Session["userid"] != null)
                {
                    ProcedureExecute proc = new ProcedureExecute("prc_dashboardsetting");
                    proc.AddVarcharPara("@Action", 30, "SELECTGROUP");
                    proc.AddVarcharPara("@searchKey", 50, _searchkey);
                    proc.AddVarcharPara("@is_success", 500, "", QueryParameterDirection.Output);
                    dt = proc.GetTable();
                    output = Convert.ToString(proc.GetParaValue("@is_success"));
                    if (output == "true")
                    {
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            _UserGroup = (from DataRow dr in dt.Rows
                                          select new UserGroup()
                                          {
                                              id = dr["grp_id"].ToString(),
                                              name = dr["grp_name"].ToString(),

                                          }).ToList();

                        }


                    }
                }


            }
            catch (Exception ex)
            {
                output = ex.Message.ToString();
            }

            return _UserGroup;

        }
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetAllUser(string id)
        {
            DashBoardDetails _DashBoardDetails = new DashBoardDetails();
            DataTable dt = new DataTable();
            try
            {
                if (HttpContext.Current.Session["userid"] != null)
                {
                    ProcedureExecute proc = new ProcedureExecute("prc_dashboardsetting");
                    proc.AddVarcharPara("@Action", 30, "AllUserByGroup");
                    proc.AddIntegerPara("@ID", Convert.ToInt32(id));
                    proc.AddVarcharPara("@is_success", 500, "", QueryParameterDirection.Output);
                    dt = proc.GetTable();
                    _DashBoardDetails.msg = Convert.ToString(proc.GetParaValue("@is_success"));
                    if (_DashBoardDetails.msg == "true")
                    {
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            _DashBoardDetails._usergroup = (from DataRow dr in dt.Rows
                                                            select new UserGroup()
                                                            {
                                                                id = dr["user_id"].ToString(),
                                                                name = dr["user_name"].ToString(),

                                                            }).ToList();

                        }


                    }
                }


            }
            catch (Exception ex)
            {
                _DashBoardDetails.msg = ex.Message.ToString();
            }

            return _DashBoardDetails;

        }

        [WebMethod(EnableSession = true)]
        public string save(Dashboard_apply apply)
        {
            string output = string.Empty;
            string action=string.Empty;
            int NoOfRowEffected = 0;
            int dasboard_id = 0;
            DataTable user_dtls = new DataTable();
            DataTable rights_dtls = new DataTable();
            try
            {
               
                user_dtls.Columns.Add("user_id", typeof(int));
                foreach ( Users _user in apply.users_dtls)
                {
                    DataRow dr = user_dtls.NewRow();
                    dr["user_id"] = _user.value;
                    user_dtls.Rows.Add(dr);
                };
                //user_dtls.AcceptChanges();



                
                rights_dtls.Columns.Add("column_name", typeof(string));
                rights_dtls.Columns.Add("status", typeof(bool));

                foreach (Rights _rights in apply.rights_dtls)
                {
                    DataRow dr = rights_dtls.NewRow();
                    dr["column_name"] = _rights.column_name;
                    dr["status"] = _rights.status;
                    rights_dtls.Rows.Add(dr);
                }

                if (apply.header.action=="add")
                {
                     action = "add";

                }
                else if (apply.header.action == "edit")
                {
                    action = "edit";
                    dasboard_id=Convert.ToInt32(apply.header.dashboard_id.Trim());
                }


                if (HttpContext.Current.Session["userid"] != null)
                {
                    int user_id = Convert.ToInt32(HttpContext.Current.Session["userid"]);
                    ProcedureExecute proc = new ProcedureExecute("prc_dashboardsetting");
                    proc.AddVarcharPara("@Action", 30,action);
                    proc.AddIntegerPara("@ID", dasboard_id);
                    proc.AddIntegerPara("@group_id", Convert.ToInt32(apply.header.user_grp_id.Trim()));
                    proc.AddVarcharPara("@group_name", 55, Convert.ToString(apply.header.user_group_name).Trim());
                    proc.AddIntegerPara("@cr_by", user_id);
                    proc.AddPara("@PARAMTABLE", user_dtls);
                    proc.AddPara("@PARAMTABLE1", rights_dtls);
                    proc.AddVarcharPara("@is_success", 500, "", QueryParameterDirection.Output);
                    NoOfRowEffected = proc.RunActionQuery();
                    output = Convert.ToString(proc.GetParaValue("@is_success"));
                    if (NoOfRowEffected > 0)
                    {

                        output ="true";

                    }
                }


            }
            catch (Exception ex)
            {
                output = ex.Message.ToString();
            }

            return output;

        }

    }

    public class UserGroup
    {
        public string id { get; set; }
        public string name { get; set; }

    }

    public class DashBoardDetails
    {
        public string msg { get; set; }

        public List<UserGroup> _usergroup { get; set; }
    }

    public class DashBoardHeader
    {
        public string user_group_name { get; set; }

        public string user_grp_id { get; set; }

        public string action { get; set; }
        public string dashboard_id { get; set; }
    }

    public class Users
    {
        public string value { get; set; }

        public string text { get; set; }
    }

    public class Rights
    {
        public string column_name { get; set; }

        public bool status { get; set; }
    }

    public class Dashboard_apply
    {

        public DashBoardHeader header { get; set; }
        public List<Users> users_dtls { get; set; }
        public List<Rights> rights_dtls { get; set; }
    }

}
