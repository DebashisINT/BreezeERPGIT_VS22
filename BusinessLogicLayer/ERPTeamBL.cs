using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BusinessLogicLayer
{
    public class ERPTeamBL
    {
        public DataTable GetAllUser()
        {
            DataTable dt = new DataTable();
            try
            {
                if (HttpContext.Current.Session["userid"] != null)
                {
                    ProcedureExecute proc = new ProcedureExecute("Proc_PMS_ALLUSER");
                    dt = proc.GetTable();
                }
            }
            catch (Exception ex)
            {
                dt = null;
            }
            return dt;
        }

    }

    public class TeamDetails
    {
        public string msg { get; set; }

        public List<UserGroup> _usergroup { get; set; }
    }

    public class UserGroup
    {
        public string id { get; set; }
        public string name { get; set; }

    }

    public class TeamHeader
    {
        public string Team_name { get; set; }

        public string branchID { get; set; }

        public string Description { get; set; }
        public string action { get; set; }
        public string teamid { get; set; }
    }

    public class Users
    {
        public string value { get; set; }

        public string text { get; set; }
    }

    public class Team_apply
    {

        public TeamHeader header { get; set; }
        public List<Users> users_dtls { get; set; }
    }
}
