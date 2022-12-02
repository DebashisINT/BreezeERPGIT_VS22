using DataAccessLayer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer
{
    public class CustomerDeliveryBL
    {
        public DataSet GetAreaPinByBranch(string branchID)
        {
            DataSet dst = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("prc_CustDelivery_GetAreaPinByBranch");
            proc.AddVarcharPara("@BranchID", 500, branchID);
            dst = proc.GetDataSet();
            return dst;
        }

        public DataSet GetVehicleDetails(string vehicleNo, string branchID)
        {
            DataSet dst = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("prc_CustDelivery_GetVehicleDetails");
            proc.AddVarcharPara("@VehicleRegNo", 20, vehicleNo);
            proc.AddVarcharPara("@BranchID", 500, branchID);
            dst = proc.GetDataSet();
            return dst;
        }

        public DataSet GetDriverDetailsByVehicle(string vehicleNo, string branchID)
        {
            DataSet dst = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("prc_CustDelivery_GetDriverByVehicle");
            proc.AddVarcharPara("@VehicleRegNo", 20, vehicleNo);
            proc.AddVarcharPara("@BranchID", 20, branchID);
            dst = proc.GetDataSet();
            return dst;
        }

        public bool GetByHandFlagByVehicle(string vehicleNo, string branchID)
        {
            DataSet dst = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("prc_CustDelivery_GetByHandFlagByVehicle");
            proc.AddVarcharPara("@VehicleRegNo", 20, vehicleNo);
            proc.AddVarcharPara("@BranchID", 20, branchID);
            dst = proc.GetDataSet();
            bool returnFlag = false;
            if (dst != null && dst.Tables.Count > 0)
            {
                if (dst.Tables[0].Rows.Count > 0)
                {
                    returnFlag = Convert.ToBoolean(dst.Tables[0].Rows[0]["ByHand"]);
                }
            }
            return returnFlag;
        }

        public DataSet GetSalesChallanForCustDelivery(string branchID, string selectedDate, string area, string pincode)
        {
            DataSet dst = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("prc_CustDelivery_GetSalesChallan"); //prc_CustDelivery_GetSalesChallan_Test
            proc.AddVarcharPara("@SelectedDate", 10, selectedDate); //selectedDate
            proc.AddVarcharPara("@BranchID", 500, branchID);
            proc.AddVarcharPara("@Area", -1, (string.IsNullOrEmpty(area)) ? "0" : area);
            proc.AddVarcharPara("@Pincode", -1, (string.IsNullOrEmpty(pincode)) ? "0" : pincode);
            dst = proc.GetDataSet();
            return dst;
        }

        public ArrayList InsertUpdateCustDelivery(string challanDetailsID, string deliveryNumber, string deliveryDate, string eWayBillNo,
              string finYr, string companyID, int branchID, string reference, string vehicleNo, string driverName, string driverPhNumber,
             string routeNo, string area, string pincode, string action, int userID, long deliveryID, string byHand, string returnValue = "-1", long returnDeliveryID = 0, string returnDeliveryNo = "")
        {
            ArrayList arrList = new ArrayList();

            ProcedureExecute proc = new ProcedureExecute("prc_CustDelivery_InsertUpdateDeilveryDetails");  //prc_CustDelivery_InsertUpdateDeilveryDetails_Test
            proc.AddVarcharPara("@ChallanDetailsID", -1, challanDetailsID); //selectedDate
            proc.AddVarcharPara("@DeliveryNumber", -1, deliveryNumber);
            proc.AddVarcharPara("@DeliveryDate", 10, deliveryDate); //selectedDate
            proc.AddVarcharPara("@FinYear", 20, finYr);
            proc.AddVarcharPara("@CompanyID", 20, companyID);
            proc.AddIntegerPara("@BranchID", branchID);
            proc.AddVarcharPara("@Reference", -1, reference);
            proc.AddVarcharPara("@VehicleNo", -1, vehicleNo);
            proc.AddVarcharPara("@DriverName", -1, driverName);
            proc.AddVarcharPara("@DriverPhNo", 15, driverPhNumber);
            proc.AddVarcharPara("@RouteNo", -1, routeNo);
            proc.AddVarcharPara("@Area", -1, area);
            proc.AddVarcharPara("@Pin", -1, pincode);
            proc.AddVarcharPara("@EWayBillNo", 50, eWayBillNo);
            proc.AddVarcharPara("@Action", -1, action);
            proc.AddIntegerPara("@UserID", userID);
            proc.AddBigIntegerPara("@DeliveryID", deliveryID);
            proc.AddVarcharPara("@ByHand", 50, byHand);

            proc.AddVarcharPara("@ReturnValue", 50, returnValue, QueryParameterDirection.Output);
            proc.AddBigIntegerPara("@ReturnDeliveryID", returnDeliveryID, QueryParameterDirection.Output);
            proc.AddVarcharPara("@ReturnDeliveryNo", 50, returnDeliveryNo, QueryParameterDirection.Output);
            proc.RunActionQuery();

            arrList.Add(Convert.ToString(proc.GetParaValue("@ReturnValue")));
            arrList.Add(Convert.ToInt64(proc.GetParaValue("@ReturnDeliveryID")));
            arrList.Add(Convert.ToString(proc.GetParaValue("@ReturnDeliveryNo")));

            return arrList;
        }

        public DataSet GetCustomerDliveryDetailsList(string deliveryID, string branchID, DateTime frmDate, DateTime toDate)
        {
            DataSet dst = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("prc_CustDelivery_GetCustDeliveryDetails");  // prc_CustDelivery_GetCustDeliveryDetails_Test
            proc.AddVarcharPara("@DeliveryID", 20, deliveryID);
            proc.AddVarcharPara("@BranchID", 500, branchID);
            proc.AddDateTimePara("@FromDelivDate", frmDate);
            proc.AddDateTimePara("@ToDelivDate", toDate);

            dst = proc.GetDataSet();
            return dst;
        }
        public DataSet GetCustomerDliveryDetailsListBackup(string deliveryID, string branchID, string frmDate = "", string toDate = "")
        {
            DataSet dst = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("prc_CustDelivery_GetCustDeliveryDetails");  // prc_CustDelivery_GetCustDeliveryDetails_Test
            proc.AddVarcharPara("@DeliveryID", 20, deliveryID);
            proc.AddVarcharPara("@BranchID", 500, branchID);
            proc.AddVarcharPara("@FromDelivDate", 10, frmDate.Trim());
            proc.AddVarcharPara("@ToDelivDate", 10, toDate.Trim());

            dst = proc.GetDataSet();
            return dst;
        }


        //Kallol240517
        public string DeleteCustomerDelivery(string delivID, string custID)
        {
            int i;
            string rtrnValue = "0"; /////rtrnValue=0 --> Delete Failed  /////rtrnValue=1 --> Delete Success
            ProcedureExecute proc = new ProcedureExecute("prc_CustDelivery_delete");
            proc.AddIntegerPara("@deliveryID", Convert.ToInt32(delivID));
            proc.AddVarcharPara("@returnValue", 50, "0", QueryParameterDirection.Output);
            i = proc.RunActionQuery();

            return rtrnValue = Convert.ToString(proc.GetParaValue("@returnValue"));
        }


        public string DeleteCustomerDelivery(string delivID)
        {
            int i;
            string rtrnValue = "0"; /////rtrnValue=0 --> Delete Failed  /////rtrnValue=1 --> Delete Success
            ProcedureExecute proc = new ProcedureExecute("prc_CustDelivery_delete");
            proc.AddIntegerPara("@deliveryID", Convert.ToInt32(delivID));
            //proc.AddIntegerPara("@customerID", Convert.ToInt32(custID));
            proc.AddVarcharPara("@returnValue", 50, "0", QueryParameterDirection.Output);
            i = proc.RunActionQuery();

            return rtrnValue = Convert.ToString(proc.GetParaValue("@returnValue"));
        }

        public DataTable GetDriverPhoneNo(string driverID)
        {
            DataTable dst = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_CustDelivery_GetDriverPhoneNumber");
            proc.AddVarcharPara("@DriverID", 10, driverID); //selectedDate
            dst = proc.GetTable();
            return dst;
        }
    }
}
