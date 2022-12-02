using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAccessLayer;
using System.Data;

namespace BusinessLogicLayer
{
    public partial class GlobalSettings
    {

        public int BrokerageMainGrid_RowDeleting(string BrokerageMain_ID, string Company, string Client,
                                      string Segment)
        {
            ProcedureExecute proc;
            try
            {


                using (proc = new ProcedureExecute("BrokerageMain_Delete"))
                {

                    proc.AddVarcharPara("@BrokerageMain_ID", 100, BrokerageMain_ID);
                    proc.AddVarcharPara("@Company", 100, Company);
                    proc.AddVarcharPara("@Client", 100, Client);
                    proc.AddVarcharPara("@Segment", 100, Segment);

                    int i = proc.RunActionQuery();

                    return i;


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


        public DataTable fetchDigitalSignHolders()
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("fetchDigitalSignHolders");
            ds = proc.GetDataSet();
            return ds.Tables[0];
        }


        public int insertDigitalSignature(string DigitalSignature_ContactID, string DigitalSignature_ValidFrom, string DigitalSignature_ValidUntil,
                                     string DigitalSignature_AuthorizedUsers, string DigitalSignature_CreateUser, string DigitalSignature_Password,
            string DigitalSignature_Type, string DigitalSignature_Name)
        {
            ProcedureExecute proc;
            try
            {


                using (proc = new ProcedureExecute("insertDigitalSignature"))
                {

                    proc.AddVarcharPara("@DigitalSignature_ContactID", 100, DigitalSignature_ContactID);
                    proc.AddVarcharPara("@DigitalSignature_ValidFrom", 100, DigitalSignature_ValidFrom);
                    proc.AddVarcharPara("@DigitalSignature_ValidUntil", 100, DigitalSignature_ValidUntil);
                    proc.AddVarcharPara("@DigitalSignature_AuthorizedUsers", 100, DigitalSignature_AuthorizedUsers);

                    proc.AddVarcharPara("@DigitalSignature_CreateUser", 100, DigitalSignature_CreateUser);
                    proc.AddVarcharPara("@DigitalSignature_Password", 100, DigitalSignature_Password);
                    proc.AddVarcharPara("@DigitalSignature_Type", 100, DigitalSignature_Type);
                    proc.AddVarcharPara("@DigitalSignature_Name", 100, DigitalSignature_Name);

                    int i = proc.RunActionQuery();

                    return i;


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


        public int EmployeeDeleteBySelctName(string cnt_internalId, string userID)
        {
            ProcedureExecute proc;
            try
            {


                using (proc = new ProcedureExecute("EmployeeDeleteBySelctName"))
                {

                    proc.AddVarcharPara("@cnt_internalId", 100, cnt_internalId);
                    proc.AddVarcharPara("@userID", 100, userID);

                    int i = proc.RunActionQuery();

                    return i;


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

        public DataSet CustomMarginRatesCOMM(string date, string MasterSegment)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("CustomMarginRatesCOMM");
            proc.AddVarcharPara("@date", 100, date);
            proc.AddVarcharPara("@MasterSegment", 100, MasterSegment);

            ds = proc.GetDataSet();
            return ds;
        }

        public DataSet Insert_CustomMarginRates(string date, string MasterSegment, string ProductSeriesID, string InitialMargin, string ExposureMargin, string ModifyUser)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("Insert_CustomMarginRates");
            proc.AddVarcharPara("@date", 100, date);
            proc.AddVarcharPara("@MasterSegment", 100, MasterSegment);
            proc.AddVarcharPara("@ProductSeriesID", 100, ProductSeriesID);
            proc.AddVarcharPara("@InitialMargin", 100, InitialMargin);
            proc.AddVarcharPara("@ExposureMargin", 100, ExposureMargin);
            proc.AddVarcharPara("@ModifyUser", 100, ModifyUser);
            ds = proc.GetDataSet();
            return ds;
        }

        public int DpChargeMain_Delete(string DPChargeMain_ID)
        {
            ProcedureExecute proc;
            try
            {


                using (proc = new ProcedureExecute("DpChargeMain_Delete"))
                {

                    proc.AddVarcharPara("@DPChargeMain_ID", 100, DPChargeMain_ID); 

                    int i = proc.RunActionQuery();

                    return i;


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

        public int TransactionCharge_Delete(string TranCharge_ID, string TranCharge_CompanyID, string TranCharge_ExchangeSegmentID, string TranCharge_ChargeGroupID, string TranCharge_ProductSeriesID)
        {
            ProcedureExecute proc;
            try
            {


                using (proc = new ProcedureExecute("TransactionCharge_Delete"))
                {
                    proc.AddVarcharPara("@TranCharge_ID", 100, TranCharge_ID);
                    proc.AddVarcharPara("@TranCharge_CompanyID", 100, TranCharge_CompanyID);
                    proc.AddIntegerPara("@TranCharge_ExchangeSegmentID", Convert.ToInt32(TranCharge_ExchangeSegmentID));
                    proc.AddVarcharPara("@TranCharge_ChargeGroupID", 100, TranCharge_ChargeGroupID);
                    proc.AddIntegerPara("@TranCharge_ProductSeriesID",  Convert.ToInt32(TranCharge_ProductSeriesID));
                    int i = proc.RunActionQuery();
                    return i;
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

        public DataTable DigitalSignatureAuthUser(string id)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("DigitalSignatureAuthUser");
            proc.AddVarcharPara("@id", 50, id);
            ds = proc.GetDataSet();
            return ds.Tables[0];
        }

        public int DigitalSignAuthUserDel(string id, string removeID)
        {
            ProcedureExecute proc;
            try
            {
                using (proc = new ProcedureExecute("DigitalSignAuthUserDel"))
                {
                    proc.AddVarcharPara("@id", 50, id);
                    proc.AddVarcharPara("@removeID", 50, removeID);
                    int i = proc.RunActionQuery();
                    return i;
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

        public int DigitalSignAuthUserAdd(string id, string addID)
        {
            ProcedureExecute proc;
            try
            {
                using (proc = new ProcedureExecute("DigitalSignAuthUserAdd"))
                {
                    proc.AddVarcharPara("@id", 50, id);
                    proc.AddVarcharPara("@addID", 50, addID);
                    int i = proc.RunActionQuery();
                    return i;
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
