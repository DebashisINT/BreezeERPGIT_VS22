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
    public class GETHOLIDAYSController : ApiController
    {

        [HttpPost]
        public HttpResponseMessage GetHOLIDAYSLIST() 
        {
            List<HolidayList> _msg = new List<HolidayList>();
            try
            {
                //BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine();
                //DataTable dt = objEngine.GetDataTable(@"select Leave_Id, LeaveType from ERP_Leavetype");

                DataTable dt = new DataTable();
                String con = Convert.ToString(APIConnction.ApiConnction);
                SqlCommand sqlcmd = new SqlCommand();

                SqlConnection sqlcon = new SqlConnection(con);
                sqlcon.Open();
                sqlcmd = new SqlCommand("Proc_GETUPCOMINGHOLIDAYS", sqlcon);
                sqlcmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                da.Fill(dt);
                sqlcon.Close();
                if (dt != null)
                {
                    _msg = (from DataRow dr in dt.Rows
                            select new HolidayList()
                            {

                                HOLIDAYID = Convert.ToString(dr["HOLIDAYID"]),
                                HOLIDAY_NAME = Convert.ToString(dr["HOLIDAY_NAME"]),
                                STARTDAY = Convert.ToString(dr["STARTDAY"]),
                                ENDDAY = Convert.ToString(dr["ENDDAY"]),
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
