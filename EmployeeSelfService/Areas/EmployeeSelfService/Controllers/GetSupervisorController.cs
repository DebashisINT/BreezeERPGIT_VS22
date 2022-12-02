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
    public class GetSupervisorController : ApiController
    {
        [HttpPost]
        public HttpResponseMessage GetSupervisorName(LeaveInfoModelInput model)
        {
            List<superVisors> _msg = new List<superVisors>();
            try
            {
                //BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine();
                //DataTable dt = objEngine.GetDataTable(@"select Leave_Id, LeaveType from ERP_Leavetype");

                DataTable dt = new DataTable();
                String con = Convert.ToString(APIConnction.ApiConnction);;
                SqlCommand sqlcmd;

                SqlConnection sqlcon = new SqlConnection(con);
                sqlcon.Open();
                sqlcmd = new SqlCommand("select cn.cnt_firstname +' '+ ISNULL(cn.cnt_lastname,'') Supervisor,cn.cnt_internalId,EML.eml_email, user_id from (select * from tbl_trans_EmployeeCTC WHERE emp_effectiveuntil iS NULL) tbl inner join tbl_master_employee e on e.emp_id =tbl.emp_reportTo inner join tbl_master_contact cn on cn.cnt_internalid=e.emp_contactId inner join tbl_master_user usr on usr.user_contactid=tbl.emp_cntId  LEFT OUTER JOIN tbl_master_email EML ON CN.cnt_internalId=EML.eml_cntId where user_id='" + model.UserId + "'", sqlcon);
                sqlcmd.CommandType = CommandType.Text;
                SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                da.Fill(dt);
                sqlcon.Close();
                if (dt != null)
                {
                    _msg = (from DataRow dr in dt.Rows
                            select new superVisors()
                            {
                                
                                userId = Convert.ToString(dr["user_id"]),
                                spName = Convert.ToString(dr["Supervisor"]),
                                spId = Convert.ToString(dr["cnt_internalid"]),
                                spEmail = Convert.ToString(dr["eml_email"]), 
                            }).ToList();
                }

            }

            catch (Exception ex)
            {

            }

            return Request.CreateResponse(HttpStatusCode.OK, _msg);
        }

        [HttpPost]
        public HttpResponseMessage isValidInOut(LeaveInfoModelInput model)
        {
            List<EMP_ATTSTATUSClass> _msg = new List<EMP_ATTSTATUSClass>();
            try
            {
                //BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine();
                //DataTable dt = objEngine.GetDataTable(@"select Leave_Id, LeaveType from ERP_Leavetype");

                DataTable dt = new DataTable();
                String con = Convert.ToString(APIConnction.ApiConnction); ;
                SqlCommand sqlcmd;

                SqlConnection sqlcon = new SqlConnection(con);
                sqlcon.Open();
                sqlcmd = new SqlCommand("select * from EMP_ATT_STATUS where CAST(ATT_DATE AS DATE)=CAST(GETDATE() AS DATE) AND EMP_CODE='" + model.UserId + "'", sqlcon);
                sqlcmd.CommandType = CommandType.Text;
                SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                da.Fill(dt);
                sqlcon.Close();
                if (dt != null)
                {
                    _msg = (from DataRow dr in dt.Rows
                            select new EMP_ATTSTATUSClass()
                            {
                                ID = Convert.ToString(dr["ID"]),
                                EMP_CODE = Convert.ToString(dr["EMP_CODE"]),
                                ATT_DATE = Convert.ToString(dr["ATT_DATE"]),
                                IS_IN = Convert.ToString(dr["IS_IN"]),
                                IS_OUT = Convert.ToString(dr["IS_OUT"]),
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
