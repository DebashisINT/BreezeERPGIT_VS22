using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DataAccessLayer;

namespace BusinessLogicLayer
{
    public class OrderApproval
    {
        public string Insert_OrderApproval(int orderDetailsId, int userId, string appPrice, decimal appQuants, string appRemarks)
        {
            ProcedureExecute proc;
            string rtrnvalue = "1";
            try
            {
                using (proc = new ProcedureExecute("SP_InsertApproval"))
                {
                    //                    @orderDetailsId int,
                    //@userId int,
                    //@appPrice varchar(max),
                    //@appQuants int,
                    //@appRemarks as varchar(max)

                    //proc.AddVarcharPara(, 100, orderDetailsId);

                    proc.AddIntegerPara("@orderDetailsId", orderDetailsId);
                    proc.AddIntegerPara("@userId", userId);

                    proc.AddDecimalPara("@appPrice",5,20, Convert.ToDecimal(appPrice));
                    proc.AddDecimalPara("@appQuants",5,20, appQuants);

                    proc.AddVarcharPara("@appRemarks", 100, appRemarks);

                    int i = proc.RunActionQuery();

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

        public string InsertRejection(int orderDetailsId, int userId, string appRemarks)
        {

            ProcedureExecute proc;
            string rtrnvalue = "1";
            try
            {
                using (proc = new ProcedureExecute("SP_InsertRejection"))
                {
                    proc.AddIntegerPara("@orderDetailsId", @orderDetailsId);
                    proc.AddIntegerPara("@userId", userId);

                    proc.AddVarcharPara("@appRemarks", 100, appRemarks);

                    int i = proc.RunActionQuery();

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


        public DataTable GetSp_GetApprovalCount(int OrderDetailsID)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("Sp_GetApprovalCount");


            proc.AddIntegerPara("@OrderDetailsID", OrderDetailsID);
           
            ds = proc.GetTable();
            return ds;
        }
    }
}
