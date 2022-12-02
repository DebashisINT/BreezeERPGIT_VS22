using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BusinessLogicLayer;
using DataAccessLayer;

namespace BusinessLogicLayer
{
    public class OtherMasters
    {
        public string InsertDPChargeMembers(string ChargeGroupMembers_SegmentID,
                                            string ChargeGroupMembers_CustomerID,
                                            string ChargeGroupMembers_CompanyID,
                                            string ChargeGroupMembers_GroupType,
                                            string ChargeGroupMembers_GroupCode,
                                            string ChargeGroupMembers_FromDate,
                                            string ChargeGroupMembers_CreateUser,
                                            string ChargeGroupMembers_ModifyUser)
        {
            ProcedureExecute proc;
            string rtrnvalue = "";
            try
            {
                using (proc = new ProcedureExecute("sp_InsertDPChargeMembers"))
                {
                    proc.AddVarcharPara("@ChargeGroupMembers_SegmentID", 100, ChargeGroupMembers_SegmentID);
                    proc.AddVarcharPara("@ChargeGroupMembers_CustomerID", 100, ChargeGroupMembers_CustomerID);
                    proc.AddBigIntegerPara("@ChargeGroupMembers_CompanyID", Convert.ToInt64(ChargeGroupMembers_CompanyID));
                    proc.AddIntegerPara("@ChargeGroupMembers_GroupType", Convert.ToInt32(ChargeGroupMembers_GroupType));
                    proc.AddVarcharPara("@ChargeGroupMembers_GroupCode", 100, ChargeGroupMembers_GroupCode);
                    proc.AddVarcharPara("@ChargeGroupMembers_FromDate", 100, ChargeGroupMembers_FromDate);
                    proc.AddIntegerPara("@ChargeGroupMembers_CreateUser", Convert.ToInt32(ChargeGroupMembers_CreateUser));
                    proc.AddIntegerPara("@ChargeGroupMembers_ModifyUser", Convert.ToInt32(ChargeGroupMembers_ModifyUser));

                    int i = proc.RunActionQuery();
                    rtrnvalue = proc.GetParaValue("@result").ToString();
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

        public string InsertDPChargeMembersElse(string ChargeGroupMembers_SegmentID,
                                          string ChargeGroupMembers_CustomerID,
                                          string ChargeGroupMembers_CompanyID,
                                          string ChargeGroupMembers_GroupType,
                                          string ChargeGroupMembers_GroupCode,
                                          string ChargeGroupMembers_FromDate,
                                          string ChargeGroupMembers_CreateUser,
                                          string ChargeGroupMembers_ModifyUser)
        {
            ProcedureExecute proc;
            string rtrnvalue = "";
            try
            {
                using (proc = new ProcedureExecute("sp_InsertChargeSchemesMembers"))
                {
                    proc.AddVarcharPara("@ChargeGroupMembers_SegmentID", 100, ChargeGroupMembers_SegmentID);
                    proc.AddVarcharPara("@ChargeGroupMembers_CustomerID", 100, ChargeGroupMembers_CustomerID);
                    proc.AddBigIntegerPara("@ChargeGroupMembers_CompanyID", Convert.ToInt64(ChargeGroupMembers_CompanyID));
                    proc.AddIntegerPara("@ChargeGroupMembers_GroupType", Convert.ToInt32(ChargeGroupMembers_GroupType));
                    proc.AddVarcharPara("@ChargeGroupMembers_GroupCode", 100, ChargeGroupMembers_GroupCode);
                    proc.AddVarcharPara("@ChargeGroupMembers_FromDate", 100, ChargeGroupMembers_FromDate);
                    proc.AddIntegerPara("@ChargeGroupMembers_CreateUser", Convert.ToInt32(ChargeGroupMembers_CreateUser));
                    proc.AddIntegerPara("@ChargeGroupMembers_ModifyUser", Convert.ToInt32(ChargeGroupMembers_ModifyUser));

                    int i = proc.RunActionQuery();
                    rtrnvalue = proc.GetParaValue("@result").ToString();
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




        public string Insert_BrokerageSlab(string BrokerageSlab_Code, string BrokerageSlab_Type, string BrokerageSlab_MinRange,
                                           string BrokerageSlab_MaxRange, string BrokerageSlab_FlatRate, string BrokerageSlab_Rate,
                                           string BrokerageSlab_MinCharge, string BrokerageSlab_CreateUser, string BrokerageSlab_CreateDateTime,
                                           string BrokerageSlab_ModifyUser)
        {
            ProcedureExecute proc;
            string rtrnvalue = "";
            Decimal rtrnvalue_maxrange;
            try
            {


                using (proc = new ProcedureExecute("BrokerageSlabInsert"))
                {

                    proc.AddVarcharPara("@BrokerageSlab_Code", 100, BrokerageSlab_Code);
                    proc.AddVarcharPara("@BrokerageSlab_Type", 100, BrokerageSlab_Type);
                    proc.AddDecimalPara("@BrokerageSlab_MinRange", 6, 18, Convert.ToDecimal(BrokerageSlab_MinRange));
                    proc.AddDecimalPara("@BrokerageSlab_MaxRange", 6, 18, Convert.ToDecimal(BrokerageSlab_MaxRange));
                    proc.AddDecimalPara("@BrokerageSlab_FlatRate", 6, 18, Convert.ToDecimal(BrokerageSlab_FlatRate));
                    proc.AddDecimalPara("@BrokerageSlab_Rate", 6, 18, Convert.ToDecimal(BrokerageSlab_Rate));
                    proc.AddDecimalPara("@BrokerageSlab_MinCharge", 6, 18, Convert.ToDecimal(BrokerageSlab_MinCharge));
                    proc.AddVarcharPara("@BrokerageSlab_CreateUser", 100, BrokerageSlab_CreateUser);
                    proc.AddVarcharPara("@BrokerageSlab_CreateDateTime", 100, BrokerageSlab_CreateDateTime);
                    proc.AddVarcharPara("@BrokerageSlab_ModifyUser", 100, BrokerageSlab_ModifyUser);

                    proc.AddVarcharPara("@ResultSlab", 20, "", QueryParameterDirection.Output);
                    proc.AddDecimalPara("@maxrange", 6, 18, 0M, QueryParameterDirection.Output);

                    int i = proc.RunActionQuery();
                    rtrnvalue = proc.GetParaValue("@ResultSlab").ToString();
                    rtrnvalue_maxrange = Convert.ToDecimal(proc.GetParaValue("@maxrange").ToString());
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




        public string Insert_Chargeheads(string OtherCharges_Code,
                                          string OtherCharges_Name,
                                          string OtherCharges_ChargeOn,
                                          string OtherCharges_Frequency,
                                          string OtherCharges_CreateUser,
                                          string OtherCharges_CreateDateTime,
                                          string OtherCharges_ModifyUser)
        {
            ProcedureExecute proc;
            string rtrnvalue = "";
            try
            {
                using (proc = new ProcedureExecute("ChargeheadsInsert"))
                {
                    proc.AddVarcharPara("@OtherCharges_Code", 100, OtherCharges_Code);
                    proc.AddVarcharPara("@OtherCharges_Name", 100, OtherCharges_Name);
                    proc.AddBigIntegerPara("@OtherCharges_ChargeOn", Convert.ToInt64(OtherCharges_ChargeOn));
                    proc.AddIntegerPara("@OtherCharges_Frequency", Convert.ToInt32(OtherCharges_Frequency));

                    proc.AddIntegerPara("@OtherCharges_CreateUser", Convert.ToInt32(OtherCharges_CreateUser));
                    proc.AddVarcharPara("@OtherCharges_CreateDateTime", 100, OtherCharges_CreateDateTime);
                    proc.AddIntegerPara("@OtherCharges_ModifyUser", Convert.ToInt32(OtherCharges_ModifyUser));

                    int i = proc.RunActionQuery();
                    rtrnvalue = proc.GetParaValue("@ResultChargeName").ToString();
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

        public string Insert_ChargeSlab(string OtherChargeSlab_Code, string OtherChargeSlab_Type, string OtherChargeSlab_MinRange,
                                                string OtherChargeSlab_MaxRange, string OtherChargeSlab_FlatRate, string OtherChargeSlab_Rate,
                                                string OtherChargeSlab_MinCharge, string OtherChargeSlab_CreateUser, string OtherChargeSlab_CreateDateTime,
                                                string OtherChargeSlab_ModifyUser)
        {
            ProcedureExecute proc;
            string rtrnvalue = "";
            Decimal rtrnvalue_maxrange;
            try
            {


                using (proc = new ProcedureExecute("ChargeSlabInsert"))
                {

                    proc.AddVarcharPara("@OtherChargeSlab_Code", 100, OtherChargeSlab_Code);
                    proc.AddVarcharPara("@OtherChargeSlab_Type", 100, OtherChargeSlab_Type);
                    proc.AddDecimalPara("@OtherChargeSlab_MinRange", 10, 3, Convert.ToDecimal(OtherChargeSlab_MinRange));
                    proc.AddDecimalPara("@OtherChargeSlab_MaxRange", 10, 3, Convert.ToDecimal(OtherChargeSlab_MaxRange));
                    proc.AddDecimalPara("@OtherChargeSlab_FlatRate", 10, 3, Convert.ToDecimal(OtherChargeSlab_FlatRate));
                    proc.AddDecimalPara("@OtherChargeSlab_Rate", 10, 3, Convert.ToDecimal(OtherChargeSlab_Rate));
                    proc.AddDecimalPara("@OtherChargeSlab_MinCharge", 10, 3, Convert.ToDecimal(OtherChargeSlab_MinCharge));
                    proc.AddVarcharPara("@OtherChargeSlab_CreateUser", 100, OtherChargeSlab_CreateUser);
                    proc.AddVarcharPara("@OtherChargeSlab_CreateDateTime", 100, OtherChargeSlab_CreateDateTime);
                    proc.AddVarcharPara("@OtherChargeSlab_ModifyUser", 100, OtherChargeSlab_ModifyUser);

                    //proc.AddVarcharPara("@ResultSlab", 20, QueryParameterDirection.Output);
                    //proc.AddDecimalPara("@maxrange", 10, 3, QueryParameterDirection.Output);

                    int i = proc.RunActionQuery();
                    rtrnvalue = proc.GetParaValue("@ResultSlab").ToString();
                    rtrnvalue_maxrange = Convert.ToDecimal(proc.GetParaValue("@maxrange").ToString());
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



    }
}
