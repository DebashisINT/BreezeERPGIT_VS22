using EmployeeSelfService.Areas.EmployeeSelfService.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace EmployeeSelfService.Areas.EmployeeSelfService.Controllers
{
    public class GetLeaveTypeController : ApiController
    {
        [HttpPost]
        public HttpResponseMessage GetLeaveType(leaveTypeModel model)
        {
            List<leaveType> _msg = new List<leaveType>();
            try
            {
                //BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine();
                //DataTable dt = objEngine.GetDataTable(@"select Leave_Id, LeaveType from ERP_Leavetype");

                DataTable dt = new DataTable();
                String con = Convert.ToString(APIConnction.ApiConnction);
                SqlCommand sqlcmd = new SqlCommand();

                SqlConnection sqlcon = new SqlConnection(con);
                sqlcon.Open();
                sqlcmd = new SqlCommand("PRC_GETLEAVETYPE", sqlcon);
                sqlcmd.Parameters.Add("@Action", "GET_LEAVETYPES");
                sqlcmd.Parameters.Add("@EMP_CODE", model.EMPCODE);
                sqlcmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                da.Fill(dt);
                sqlcon.Close();
                if (dt != null)
                {
                    _msg = (from DataRow dr in dt.Rows
                            select new leaveType()
                            {
                                LeaveID = Convert.ToString(dr["LeaveID"]),
                                LeaveName = Convert.ToString(dr["LeaveName"])
                            }).ToList();
                }

            }

            catch (Exception ex)
            {

            }

            return Request.CreateResponse(HttpStatusCode.OK, _msg);
        }
    }
}
