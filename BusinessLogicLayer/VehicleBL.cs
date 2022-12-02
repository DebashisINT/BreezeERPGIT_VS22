using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAccessLayer;
using System.Data;

namespace BusinessLogicLayer
{
    public class VehicleBL
    {
        public int InsertUpdateVehicleMaster(string mod, string vehicleRegnNo, string vehicleEngineNo, string vehicleChasisNo, string vehicleType, string vehicleMake, string vehModel,
                                             int regnYear, string vehicleOwner, string engnCC, string gps, string logBook, string vehicleAllotTo, string fleetCrdApplied, string fleetCrdNo,
                                             string hppyCard, string insuranceName, string insurancePolicyNo, string insuranceValidUpto, string insuranceGivenTo,
                                             string txTokenNo, string tokenValidUptoDt, string Pollution, string pollutionValidUpto, string pollutionCaseDetail,
                                             string authLetter, string authLetterValidUpto, string blueBook, string CFcase, string CFvalid, string vehicleFuelType,
                                             string vehicleIsActive, int userID, bool IsByHand)
        {
            int i;
            int retValue = 0;

            ProcedureExecute proc = new ProcedureExecute("prc_InsertUpdateVehicleMaster");
            proc.AddVarcharPara("@Mod", 50, mod);
            proc.AddVarcharPara("@regNo", 50, vehicleRegnNo);
            proc.AddVarcharPara("@engineNo", 50, vehicleEngineNo);
            proc.AddVarcharPara("@ChassisNo", 50, vehicleChasisNo);
            proc.AddVarcharPara("@Type", 50, vehicleType);
            proc.AddVarcharPara("@maker", 500, vehicleMake);
            proc.AddVarcharPara("@model", 50, vehModel);
            if (regnYear != null)
                proc.AddIntegerPara("@yearReg", regnYear);
            else
                proc.AddIntegerNullPara("@yearReg");
            proc.AddVarcharPara("@vehOwnerType", 50, vehicleOwner);
            proc.AddVarcharPara("@engineCC", 50, engnCC);

            proc.AddVarcharPara("@isGPSInstalled", 50, gps);


            proc.AddVarcharPara("@LogBookStatus", 50, logBook);


            proc.AddVarcharPara("@AllotedTo", 200,vehicleAllotTo );

            proc.AddVarcharPara("@isFleetCardApplied", 50,fleetCrdApplied);

            proc.AddVarcharPara("@FleetCardNumber", 100, fleetCrdNo);
            proc.AddVarcharPara("@HappyCard", 100, hppyCard );
            proc.AddVarcharPara("@InsurerName", 200, insuranceName);
            proc.AddVarcharPara("@PolicyNo", 100, insurancePolicyNo);
            proc.AddVarcharPara("@PolicyValidUpto", 100, insuranceValidUpto);
            proc.AddVarcharPara("@InsuranceGivenTo", 200, insuranceGivenTo);


            proc.AddVarcharPara("@TaxTokenNo", 100, txTokenNo);
            proc.AddVarcharPara("@TaxValidUpto", 100, tokenValidUptoDt);
            proc.AddVarcharPara("@Pollution", 500, Pollution);
            proc.AddVarcharPara("@PollutionCertValidUpto", 100, pollutionValidUpto);
            proc.AddVarcharPara("@PollutionCaseDtl", 1000, pollutionCaseDetail);

            proc.AddVarcharPara("@isAuthLetter", 50, authLetter);

            proc.AddVarcharPara("@AuthLetterValidUpto", 100, authLetterValidUpto);
            proc.AddVarcharPara("@BlueBook", 400, blueBook);
            proc.AddVarcharPara("@CFDetails", 1000, CFcase);
            if(CFvalid != null)
               proc.AddVarcharPara("@CFValidUpto", 100,CFvalid);
            else
               proc.AddNullValuePara("@CFValidUpto");


            proc.AddVarcharPara("@fuelType", 50, vehicleFuelType);

            proc.AddVarcharPara("@isActive", 50, vehicleIsActive);

            proc.AddIntegerPara("@recCreateUser", userID);

            proc.AddBooleanPara("@IsByHand", IsByHand);
            proc.AddVarcharPara("@result", 50,"",QueryParameterDirection.Output);
            i = proc.RunActionQuery();
            retValue = Convert.ToInt32(proc.GetParaValue("@result")); 
            return retValue;
        }



        public DataTable GetsVehicleList(string vehLstOrdBy)
        {
            ProcedureExecute proc;
            DataTable rtrnvalue;
            try
            {
                using (proc = new ProcedureExecute("prc_GetAllVehicles"))
                {
                    proc.AddVarcharPara("@OrderByMod", 10 ,vehLstOrdBy);
                    rtrnvalue = proc.GetTable();
                    return rtrnvalue;
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }

            finally
            {
                proc = null;
            }
        }

        public int isRegNoAvailable(string regNumber)
        {
            int i;
            int availableRegNoCount = 0;
            ProcedureExecute proc;
            try
            {
                using (proc = new ProcedureExecute("prc_GetRegNoCount"))
                {
                    proc.AddVarcharPara("@regNo", 50 ,regNumber);
                    proc.AddVarcharPara("@result", 50,"",QueryParameterDirection.Output);
                    i = proc.RunActionQuery();
                    availableRegNoCount = Convert.ToInt32(proc.GetParaValue("@result"));                   
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return availableRegNoCount;
        }

        public int isEngNoAvailable(string engNumber)
        {
            int i;
            int availableEngNoCount = 0;
            ProcedureExecute proc;
            try
            {
                using (proc = new ProcedureExecute("prc_GetEngNoCount"))
                {
                    proc.AddVarcharPara("@engNo", 50, engNumber);
                    proc.AddVarcharPara("@result", 50, "", QueryParameterDirection.Output);
                    i = proc.RunActionQuery();
                    availableEngNoCount = Convert.ToInt32(proc.GetParaValue("@result"));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return availableEngNoCount;
        }


        public int isChassNoAvailable(string chassNumber)
        {
            int i;
            int availableChassNoCount = 0;
            ProcedureExecute proc;
            try
            {
                using (proc = new ProcedureExecute("prc_GetChassNoCount"))
                {
                    proc.AddVarcharPara("@chassNo", 50, chassNumber);
                    proc.AddVarcharPara("@result", 50, "", QueryParameterDirection.Output);
                    i = proc.RunActionQuery();
                    availableChassNoCount = Convert.ToInt32(proc.GetParaValue("@result"));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return availableChassNoCount;
        }


        public int UpdateVehicleMaster(string mod,
                                    int vehID, string vehRegnNo, string engineNo, string chasisNo, string vehType, string vehMake, string vehModel, int regnYear, string ownr, string engineCC, string isGPS,
                                    string logBook, string vehAllotTo, string fleetCardApplied, string fleetCardNo, string hppyCard, string insurName, string insurPolicyNo, string insurValidUpto, string insuranceGivenTo, string taxTokenNo, string tokenValidUpto,
                                    string Pollution, string pollutValidUpto, string pollutCaseDtl, string isAuthLettr, string authLettrValidUpto, string blueBook, string cfCaseDtl, string cfValidUpto, string vehFuelType, string vehIsActive, int userID, bool IsByHand)
        {
            int i;
            int retValue = 0;

            ProcedureExecute proc = new ProcedureExecute("prc_InsertUpdateVehicleMaster");
            proc.AddVarcharPara("@Mod", 50, mod);

            proc.AddIntegerPara("@vehicleID", vehID);
            proc.AddVarcharPara("@regNo", 50, vehRegnNo);
            proc.AddVarcharPara("@engineNo", 50, engineNo);
            proc.AddVarcharPara("@ChassisNo", 50, chasisNo);
            proc.AddVarcharPara("@Type", 50, vehType);
            proc.AddVarcharPara("@maker", 500, vehMake);
            proc.AddVarcharPara("@model", 50, vehModel);
            if (regnYear != null)
                proc.AddIntegerPara("@yearReg", regnYear);
            else
                proc.AddIntegerNullPara("@yearReg");
            proc.AddVarcharPara("@vehOwnerType", 50, ownr);
            proc.AddVarcharPara("@engineCC", 50, engineCC);
            proc.AddVarcharPara("@isGPSInstalled", 50, isGPS);

            proc.AddVarcharPara("@LogBookStatus", 50, logBook);


            proc.AddVarcharPara("@AllotedTo", 200, vehAllotTo);

            proc.AddVarcharPara("@isFleetCardApplied", 50, fleetCardApplied);

            proc.AddVarcharPara("@FleetCardNumber", 100, fleetCardNo);
            proc.AddVarcharPara("@HappyCard", 100, hppyCard);
            proc.AddVarcharPara("@InsurerName", 200, insurName);
            proc.AddVarcharPara("@PolicyNo", 100, insurPolicyNo);
            if (insurValidUpto != "")
            {
                if (Convert.ToDateTime(insurValidUpto) != Convert.ToDateTime("1/1/0001 12:00:00 AM"))
                {
                    proc.AddDateTimePara("@PolicyValidUpto", Convert.ToDateTime(insurValidUpto));
                }
            }

            proc.AddVarcharPara("@InsuranceGivenTo", 200, insuranceGivenTo);
            proc.AddVarcharPara("@TaxTokenNo", 100, taxTokenNo);
            if (tokenValidUpto != "")
            {
                if (Convert.ToDateTime(tokenValidUpto) != Convert.ToDateTime("1/1/0001 12:00:00 AM"))
                {
                    proc.AddDateTimePara("@TaxValidUpto", Convert.ToDateTime(tokenValidUpto));
                }
            }

            proc.AddVarcharPara("@Pollution", 500, Pollution);
            if (pollutValidUpto != "")
            {
                if (Convert.ToDateTime(pollutValidUpto) != Convert.ToDateTime("1/1/0001 12:00:00 AM"))
                {
                    proc.AddDateTimePara("@PollutionCertValidUpto", Convert.ToDateTime(pollutValidUpto));
                }
            }

            proc.AddVarcharPara("@PollutionCaseDtl", 1000, pollutCaseDtl);

            proc.AddVarcharPara("@isAuthLetter", 50, isAuthLettr);

            if (authLettrValidUpto != "")
            {
                if (Convert.ToDateTime(authLettrValidUpto) != Convert.ToDateTime("1/1/0001 12:00:00 AM"))
                {
                    proc.AddDateTimePara("@AuthLetterValidUpto", Convert.ToDateTime(authLettrValidUpto));
                }
            }

            proc.AddVarcharPara("@BlueBook", 400, blueBook);
            proc.AddVarcharPara("@CFDetails", 1000, cfCaseDtl);
            if (cfValidUpto != "")
            {
                if (Convert.ToDateTime(cfValidUpto) != Convert.ToDateTime("1/1/0001 12:00:00 AM"))
                {
                    proc.AddDateTimePara("@CFValidUpto", Convert.ToDateTime(cfValidUpto));
                }
            }

            proc.AddVarcharPara("@fuelType", 50, vehFuelType);
            proc.AddVarcharPara("@isActive", 50, vehIsActive);

            proc.AddIntegerPara("@recCreateUser", userID);
            proc.AddBooleanPara("@IsByHand", IsByHand);

            proc.AddVarcharPara("@result", 50, "", QueryParameterDirection.Output);
            i = proc.RunActionQuery();
            retValue = Convert.ToInt32(proc.GetParaValue("@result"));
            return retValue;
        }


    }
}
