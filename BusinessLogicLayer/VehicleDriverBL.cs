using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer
{
    public class VehicleDriverBL
    {
        public DataSet GetVehicleByActiveStatus(string IsActive)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("prc_GetVehicleRegNo");
            proc.AddVarcharPara("@IsActive", 100, IsActive);
            ds = proc.GetDataSet();
            return ds;
        }
        public Dictionary<string, object> PopulateVehicleDetails(string vehicle_Regno)
        {
            Dictionary<string, object> _Obj = new Dictionary<string, object>();

            DataSet DS_VehicalDetails = GetVehicleDetails(vehicle_Regno);
            if (DS_VehicalDetails != null && DS_VehicalDetails.Tables[0].Rows.Count > 0)
            {
                _Obj.Add("vehicle_regNo", Convert.ToString(DS_VehicalDetails.Tables[0].Rows[0]["vehicle_regNo"]).Trim());
                _Obj.Add("vehicle_engineNo", Convert.ToString(DS_VehicalDetails.Tables[0].Rows[0]["vehicle_engineNo"]).Trim());
                _Obj.Add("vehicle_Type", Convert.ToString(DS_VehicalDetails.Tables[0].Rows[0]["vehicle_Type"]).Trim());
                _Obj.Add("vehicle_maker", Convert.ToString(DS_VehicalDetails.Tables[0].Rows[0]["vehicle_maker"]).Trim());
                _Obj.Add("vehicle_model", Convert.ToString(DS_VehicalDetails.Tables[0].Rows[0]["vehicle_model"]).Trim());
                _Obj.Add("vehicle_yearReg", Convert.ToString(DS_VehicalDetails.Tables[0].Rows[0]["vehicle_yearReg"]).Trim());
                _Obj.Add("vehicle_fuelType", Convert.ToString(DS_VehicalDetails.Tables[0].Rows[0]["vehicle_fuelType"]).Trim());
                _Obj.Add("vehicle_Pollution", Convert.ToString(DS_VehicalDetails.Tables[0].Rows[0]["vehicle_Pollution"]).Trim());
                _Obj.Add("vehicle_isGPSInstalled", Convert.ToString(DS_VehicalDetails.Tables[0].Rows[0]["vehicle_isGPSInstalled"]).Trim());
                _Obj.Add("vehicle_BlueBook", Convert.ToString(DS_VehicalDetails.Tables[0].Rows[0]["vehicle_BlueBook"]).Trim());
                _Obj.Add("vehicle_engineCC", Convert.ToString(DS_VehicalDetails.Tables[0].Rows[0]["vehicle_engineCC"]).Trim());
                _Obj.Add("vehicle_AllotedTo", Convert.ToString(DS_VehicalDetails.Tables[0].Rows[0]["vehicle_AllotedTo"]).Trim());
                _Obj.Add("vehicle_FleetCardNumber", Convert.ToString(DS_VehicalDetails.Tables[0].Rows[0]["vehicle_FleetCardNumber"]).Trim());
                _Obj.Add("vehicle_HappyCard", Convert.ToString(DS_VehicalDetails.Tables[0].Rows[0]["vehicle_HappyCard"]).Trim());
                _Obj.Add("vehicle_InsurerName", Convert.ToString(DS_VehicalDetails.Tables[0].Rows[0]["vehicle_InsurerName"]).Trim());
                _Obj.Add("vehicle_PolicyNo", Convert.ToString(DS_VehicalDetails.Tables[0].Rows[0]["vehicle_PolicyNo"]).Trim());
                _Obj.Add("vehicle_PolicyValidUpto", Convert.ToString(Convert.ToDateTime(DS_VehicalDetails.Tables[0].Rows[0]["vehicle_PolicyValidUpto"]).ToString("dd/MM/yyyy")).Trim());
                _Obj.Add("vehicle_InsuranceGivenTo", Convert.ToString(DS_VehicalDetails.Tables[0].Rows[0]["vehicle_InsuranceGivenTo"]).Trim());
                _Obj.Add("vehicle_TaxTokenNo", Convert.ToString(DS_VehicalDetails.Tables[0].Rows[0]["vehicle_TaxTokenNo"]).Trim());
                _Obj.Add("vehicle_TaxValidUpto", Convert.ToString(Convert.ToDateTime(DS_VehicalDetails.Tables[0].Rows[0]["vehicle_TaxValidUpto"]).ToString("dd/MM/yyyy")).Trim());
                _Obj.Add("vehicle_PollutionCaseDtl", Convert.ToString(DS_VehicalDetails.Tables[0].Rows[0]["vehicle_PollutionCaseDtl"]).Trim());
                _Obj.Add("vehicle_PollutionCertValidUpto", Convert.ToString(DS_VehicalDetails.Tables[0].Rows[0]["vehicle_PollutionCertValidUpto"]).Trim());
                _Obj.Add("vehicle_AuthLetterValidUpto", Convert.ToString(Convert.ToDateTime(DS_VehicalDetails.Tables[0].Rows[0]["vehicle_AuthLetterValidUpto"]).ToString("dd/MM/yyyy")).Trim());
                _Obj.Add("vehicle_CFDetails", Convert.ToString(DS_VehicalDetails.Tables[0].Rows[0]["vehicle_CFDetails"]).Trim());
                _Obj.Add("vehicle_CFValidUpto", Convert.ToString(Convert.ToDateTime(DS_VehicalDetails.Tables[0].Rows[0]["vehicle_CFValidUpto"]).ToString("dd/MM/yyyy")).Trim());
                _Obj.Add("vehicle_vehOwnerType", Convert.ToString(DS_VehicalDetails.Tables[0].Rows[0]["vehicle_vehOwnerType"]).Trim());
                _Obj.Add("vehicle_ChassisNo", Convert.ToString(DS_VehicalDetails.Tables[0].Rows[0]["vehicle_ChassisNo"]).Trim());
                _Obj.Add("vehicle_LogBookStatus", Convert.ToString(DS_VehicalDetails.Tables[0].Rows[0]["vehicle_LogBookStatus"]).Trim());
                _Obj.Add("vehicle_isFleetCardApplied", Convert.ToString(DS_VehicalDetails.Tables[0].Rows[0]["vehicle_isFleetCardApplied"]).Trim());
                _Obj.Add("vehicle_isAuthLetter", Convert.ToString(DS_VehicalDetails.Tables[0].Rows[0]["vehicle_isAuthLetter"]).Trim());
                _Obj.Add("vehicle_isActive", Convert.ToString(DS_VehicalDetails.Tables[0].Rows[0]["vehicle_isActive"]).Trim());

                _Obj.Add("VehiclesDriver", GetVehiclesDriver(Convert.ToString(DS_VehicalDetails.Tables[0].Rows[0]["vehicle_regNo"]).Trim()));
                _Obj.Add("AllVehiclesDriver", GetAllVehiclesDriver());
            }
            return _Obj;
        }
        public List<DriverContact> GetVehiclesDriver(string vehicle_regNo)
        {
            List<DriverContact> _ObjDriverContact = new List<DriverContact>();
            string cTableName = "tbl_master_contact";
            string cFieldName = "cnt_internalId, cnt_shortName";
            string cWhereClause = @"tbl_master_contact.cnt_internalId IN(SELECT tbl_trans_VehiclesDriver.DriversInternalID FROM 
                                  tbl_trans_VehiclesDriver WHERE LTRIM(RTRIM(tbl_trans_VehiclesDriver.VehiclesRegNo)) = LTRIM(RTRIM('" + vehicle_regNo + "')))";

          //  BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]); MULTI
              BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine();


            DataTable DT = objEngine.GetDataTable(cTableName, cFieldName, cWhereClause);
            _ObjDriverContact.Clear();

            if (DT != null && DT.Rows.Count > 0)
            {
                foreach (DataRow dr in DT.Rows)
                {
                    _ObjDriverContact.Add(new DriverContact()
                    {
                        DriversInternalID = Convert.ToString(dr["cnt_internalId"]).Trim(),
                        fullname = Convert.ToString(dr["cnt_shortName"]).Trim()
                    });
                }
            }
            return _ObjDriverContact;
        }
        public List<DriverContact> GetAllVehiclesDriver()
        {
            List<DriverContact> _ObjDriverContact = new List<DriverContact>();
            string cTableName = "tbl_master_contact";
            string cFieldName = "cnt_internalId, cnt_shortName";
            string cWhereClause = null;

           // BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]); MULTI
            BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine();

            DataTable DT = objEngine.GetDataTable(cTableName, cFieldName, cWhereClause);
            _ObjDriverContact.Clear();

            if (DT != null && DT.Rows.Count > 0)
            {
                foreach (DataRow dr in DT.Rows)
                {
                    bool containsItem = _ObjDriverContact.Any(x => x.DriversInternalID == Convert.ToString(dr["cnt_internalId"]).Trim());
                    if (!containsItem)
                    {
                        _ObjDriverContact.Add(new DriverContact()
                        {
                            DriversInternalID = Convert.ToString(dr["cnt_internalId"]).Trim(),
                            fullname = Convert.ToString(dr["cnt_shortName"]).Trim()
                        });
                    }
                }
            }
            return _ObjDriverContact;
        }
        public DataSet GetVehicleDetails(string vehicle_regNo)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("prc_GetVehicleDetails");
            proc.AddVarcharPara("@vehicle_regNo", 20, vehicle_regNo);
            ds = proc.GetDataSet();
            return ds;
        }
        public int SaveVehicleDriverData(DocwiseVehicledriverModel obj)
        {
            ProcedureExecute proc = new ProcedureExecute("SP_DocwiseVehicledriverCRUD");
            proc.AddVarcharPara("@VehicleRegNo", 100, obj.VehicleRegNo);
            proc.AddVarcharPara("@DocId", 100, obj.DocId);
            proc.AddVarcharPara("@DocType", 100, obj.DocType);
            proc.AddVarcharPara("@DriversID", 100, obj.DriversID);
            proc.AddIntegerPara("@CreatedBy", obj.CreatedBy);
            int Ret = proc.RunActionQuery();

            return Ret;
        }
    }
    public class DriverContact
    {
        public string DriversInternalID { get; set; }
        public string fullname { get; set; }
    }
    public class DocwiseVehicledriverModel
    {
        public string VehicleRegNo { get; set; }
        public string DocId { get; set; }
        public string DocType { get; set; }
        public string DriversID { get; set; }
        public int CreatedBy { get; set; }
    }
}
