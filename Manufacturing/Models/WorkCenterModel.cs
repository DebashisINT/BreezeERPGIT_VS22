using DataAccessLayer;
using Manufacturing.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Manufacturing.Models
{
    public class WorkCenterModel
    {
        string ConnectionString = String.Empty;
        public WorkCenterModel()
        {
            ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
        }

        public DataTable WorkCenterInsertUpdate(WorkCenterViewModel obj)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("usp_WorkCenterInsertUpdate");
            proc.AddVarcharPara("@ACTION", 100, "INSERTUPDATEWORKCENTER");
            proc.AddPara("@WorkCenterID", obj.WorkCenterID);
            proc.AddPara("@WorkCenterCode", obj.WorkCenterCode);
            proc.AddPara("@WorkCenterDescription", obj.WorkCenterDescription);
            proc.AddPara("@Remarks", obj.Remarks);
            proc.AddPara("@Address1", obj.WorkCenterAddress1);
            proc.AddPara("@Address2", obj.WorkCenterAddress2);
            proc.AddPara("@Address3", obj.WorkCenterAddress3);
            proc.AddPara("@Landmark", obj.WorkCenterLandmark);
            proc.AddPara("@CountryID", obj.WorkCenterCountry);
            proc.AddPara("@StateID", obj.WorkCenterState);
            proc.AddPara("@CityID", obj.WorkCenterCity);
            proc.AddPara("@PinID", obj.WorkCenterPin);
            proc.AddPara("@BranchID", obj.WorkCenterBranch);
            proc.AddPara("@UserID", obj.UserID);
            ds = proc.GetTable();
            return ds;
        }

        public DataTable GetWorkCenterData(string PinCode = null)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("usp_WorkCenterDataGet");
            proc.AddVarcharPara("@ACTION", 100, "GETADDRESSBYPIN");
            proc.AddPara("@pin_code", PinCode);
            ds = proc.GetTable();
            return ds;
        }
    }
}