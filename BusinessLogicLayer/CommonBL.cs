using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAccessLayer;
using System.Data;
using System.Web;
namespace BusinessLogicLayer
{
    public class CommonBL
    {
        public bool IsReadOnlyUser(int UserId)
        {
            bool IsReadOnly = false;

            ProcedureExecute proc;
            try
            {
                using (proc = new ProcedureExecute("CheckReadOnlyUser"))
                {
                    proc.AddIntegerPara("@UserId", UserId);
                    DataTable dt = proc.GetTable();

                    if (dt != null)
                    {
                        if (dt.Rows.Count > 0)
                        {
                            if (Convert.ToString(dt.Rows[0]["user_EntryProfile"]) == "R")
                            {
                                IsReadOnly = true;
                            }
                        }
                    }
                    return IsReadOnly;
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



            return IsReadOnly;
        }


        public string GetSystemSettingsResult(string VariableName)
        {
            string VariableValue = string.Empty;
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("Proc_SystemsettingResult");
            proc.AddVarcharPara("@VariableName", 200, VariableName);
            dt = proc.GetTable();

            if (dt.Rows.Count > 0)
            {
                VariableValue = Convert.ToString(dt.Rows[0]["Variable_Value"]);
            }
            return VariableValue;
        }
        public static string SafeSqlLiteral(string inputSQL)
        {
            inputSQL = System.Security.SecurityElement.Escape(inputSQL);
            return inputSQL;

            inputSQL = inputSQL.Replace("'", "&#39");
            inputSQL = inputSQL.Replace(";", "&#59");
            inputSQL = inputSQL.Replace(")", "&#41");
            inputSQL = inputSQL.Replace("\'", "");
            inputSQL = inputSQL.Replace("alt=\"\"", "alt=\"Blank\"");
            inputSQL = inputSQL.Replace("\"", "");
            //inputSQL = inputSQL.Replace("\"", "&#92;");
            inputSQL = inputSQL.Replace("\\", "");
            return inputSQL;
        }

        public static string CheckJavaScriptInjection(string UserData)
        {
            string OutString = string.Empty;
            if (UserData.Contains("<script>") == true || UserData.Contains("%3Cscript%3E") == true || UserData.Contains("%3cscript%3e") == true || UserData.Contains("%3c/script%3e") == true || UserData.Contains("</script>") == true || UserData.Contains("type=\"text/javascript\"") == true ||
               UserData.Contains("type='text/javascript'") == true || UserData.Contains("<script type='text/javascript'>") == true ||
               UserData.Contains("<script type=\"text/javascript\">") == true || UserData.Contains("javascript:") == true || UserData.Contains("?cookie=") == true ||
               UserData.Contains("cookie=") == true || UserData.Contains("cookie") == true || UserData.Contains("?cookie") == true ||
               UserData.Contains("type = \"hidden\"") == true || UserData.Contains("type = 'hidden'") == true || UserData.Contains("<input type = 'hidden'") == true ||
               UserData.Contains("<input type = \"hidden\"") == true || UserData.Contains("<html xmlns=\"http://www.w3.org/1999/xhtml\" >") == true ||
               UserData.Contains("method = \"post\"") == true || UserData.Contains("method = 'post'") == true || UserData.Contains("<form") == true || UserData.Contains("</form>") == true ||
               UserData.Contains("action=") == true || UserData.Contains("</html>") == true || UserData.Contains("</HTML>") == true || UserData.Contains("</body>") == true ||
               UserData.Contains("</BODY>") == true || UserData.Contains("%3C/BODY%3E") == true || UserData.Contains("%3c/BODY%3e") == true || UserData.Contains("%3C/script%3E") == true
                || UserData.Contains("%3c/body%3e") == true || UserData.Contains("%3C/body%3E") == true || UserData.Contains("%3C/form%3E") == true
                || UserData.Contains("%3c/form%3e") == true || UserData.Contains("%3C/FORM%3E") == true || UserData.Contains("%3c/FORM%3e") == true
                || UserData.Contains("%3c/Form%3e") == true || UserData.Contains("%3C/Form%3E") == true
                || UserData.Contains("value=") == true || UserData.Contains("<script") == true || UserData.Contains("&lt;script&gt;") == true
                || UserData.Contains("&lt;/script&gt;") == true || UserData.Contains("&lt;SCRIPT&gt;") == true || UserData.Contains("&lt;/SCRIPT&gt;") == true
                || UserData.Contains("]]>") == true || UserData.Contains("CDATA") == true || UserData.Contains("onclick=") == true || UserData.Contains("onclick") == true || UserData.Contains("%3CSCRIPT%3E") == true || UserData.Contains("%3C/SCRIPT%3E") == true ||
                UserData.Contains("<iframe>") == true || UserData.Contains("</iframe>") == true || UserData.Contains("&lt;iframe&gt;") == true || UserData.Contains("&lt;/iframe&gt;") == true ||
                UserData.Contains("<img>") == true || UserData.Contains("</img>") == true || UserData.Contains("&lt;img&gt;") == true || UserData.Contains("&lt;/img&gt;") == true || UserData.Contains("&lt;img") == true ||
                UserData.Contains("<applet>") == true || UserData.Contains("</applet>") == true || UserData.Contains("&lt;applet&gt;") == true || UserData.Contains("&lt;/applet&gt;") == true)
            {
                OutString = HttpContext.Current.Server.HtmlEncode(UserData);
                //OutString = "Invalid data";
            }
            else
            {
                OutString = UserData;
            }
            return OutString;
        }

        #region ### Transporter Control ###
        public DataSet GetContactTypeTransporter(string contactTypePrefix)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("prc_GetContactTypeTransporter");
            proc.AddVarcharPara("@ContactTypePrefix", 5, contactTypePrefix);
            ds = proc.GetDataSet();
            return ds;
        }

        public DataSet GetVehicleControlDetails(string transporterInternalID)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("prc_GetVehicleControlDetails");
            proc.AddVarcharPara("@TransporterInternalID", 20, transporterInternalID);
            ds = proc.GetDataSet();
            return ds;
        }


        public int InsertTransporterControlDetails(long docID, string docType, string controlData, string createdBy)
        {
            string[] controlDataList = controlData.Split('|');

            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("prc_InsertTransporterControlDetails");
            proc.AddBigIntegerPara("@DocID", docID);
            proc.AddVarcharPara("@DocType", 10, docType);
            proc.AddVarcharPara("@TransID", 20, controlDataList[0]);
            proc.AddIntegerPara("@TransType", Convert.ToInt32(controlDataList[1]));
            proc.AddBooleanPara("@Registered", Convert.ToBoolean(Convert.ToInt16(controlDataList[2])));
            proc.AddVarcharPara("@GSTIN", 20, controlDataList[3]);
            proc.AddVarcharPara("@Phone", 20, (controlDataList[11] == "null") ? string.Empty : controlDataList[11]);
            proc.AddVarcharPara("@Address", 100, (controlDataList[5] == "null") ? string.Empty : controlDataList[5]);
            proc.AddBigIntegerPara("@CountryID", (controlDataList[6] == "null") ? 0 : Convert.ToInt32(controlDataList[6]));
            proc.AddBigIntegerPara("@StateID", (controlDataList[7] == "null") ? 0 : Convert.ToInt32(controlDataList[7]));
            proc.AddBigIntegerPara("@CityID", (controlDataList[8] == "null") ? 0 : Convert.ToInt32(controlDataList[8]));
            proc.AddVarcharPara("@Pincode", 100, (controlDataList[9] == "null") ? string.Empty : controlDataList[9]);
            proc.AddVarcharPara("@Area", 100, (controlDataList[10] == "null") ? string.Empty : controlDataList[10]);
            proc.AddVarcharPara("@VehicleNos", 500, (controlDataList[4] == "null") ? string.Empty : controlDataList[4]);
            proc.AddNVarcharPara("@Freight", 50, (controlDataList[12].Trim() == "") ? "0" : Convert.ToString(controlDataList[12].Trim()));
            proc.AddNVarcharPara("@Point", 50, (controlDataList[13] == "") ? "0" : Convert.ToString(controlDataList[13].Trim()));
            proc.AddNVarcharPara("@Loading", 50, (controlDataList[14].Trim() == "") ? "0" : Convert.ToString(controlDataList[14].Trim()));
            proc.AddNVarcharPara("@Unloading", 50, (controlDataList[15].Trim() == "") ? "0" : Convert.ToString(controlDataList[15].Trim()));
            proc.AddNVarcharPara("@Parking", 50, (controlDataList[16].Trim() == "") ? "0" : Convert.ToString(controlDataList[16].Trim()));
            proc.AddNVarcharPara("@Weighment", 50, (controlDataList[17].Trim() == "") ? "0" : Convert.ToString(controlDataList[17].Trim()));
            proc.AddNVarcharPara("@Lrno", 50, (controlDataList[18].Trim() == "") ? string.Empty : Convert.ToString(controlDataList[18].Trim()));
            proc.AddNVarcharPara("@VehicleOutDate", 50, (controlDataList[19].Trim() == "01/01/0100 00:00:00") ? null : Convert.ToString(controlDataList[19].Trim()));
            proc.AddNVarcharPara("@TollTax", 50, (controlDataList[20].Trim() == "") ? "0" : Convert.ToString(controlDataList[20].Trim()));
            proc.AddVarcharPara("@Trip", 255, (controlDataList[21] == "null") ? string.Empty : controlDataList[21]);
            proc.AddBigIntegerPara("@FreightArea", (controlDataList[22] == "null") ? 0 : Convert.ToInt64(controlDataList[22]));
            proc.AddNVarcharPara("@TotalCharges", 50, (controlDataList[23] == "") ? "0" : Convert.ToString(controlDataList[23].Trim()));
            proc.AddNVarcharPara("@OtherCharges", 50, (controlDataList[24] == "") ? "0" : Convert.ToString(controlDataList[24].Trim()));
            //--
            proc.AddNVarcharPara("@LRDate", 50, (controlDataList[25].Trim() == "01-01-0100") ? null : Convert.ToString(controlDataList[25].Trim()));
            //--
            proc.AddDecimalPara("@Transporter_Distance", 4, 5, Convert.ToDecimal((controlDataList[26] == "") ? "0" : Convert.ToString(controlDataList[26].Trim())));
            proc.AddNVarcharPara("@Vehicle_type", 50, (controlDataList[27] == "") ? "0" : Convert.ToString(controlDataList[27].Trim()));
            proc.AddIntegerPara("@Transporter_Mode", Convert.ToInt32((controlDataList[28] == "") ? "0" : Convert.ToString(controlDataList[28].Trim())));
            proc.AddVarcharPara("@FinalTransID", 20, controlDataList[29]);
            proc.AddBigIntegerPara("@CreatedBy", Convert.ToInt32(createdBy));
            return proc.RunActionQuery();
        }

        public int InsertSalesChallanDetails(long docID, string docType, string controlData, string createdBy)
        {
            string[] controlDataList = controlData.Split('|');

            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("prc_InsertTransporterChallanDetails");
            proc.AddBigIntegerPara("@DocID", docID);
            proc.AddVarcharPara("@DocType", 10, docType);
            proc.AddVarcharPara("@TransID", 20, controlDataList[0]);
            proc.AddIntegerPara("@TransType", Convert.ToInt32(controlDataList[1]));
            proc.AddBooleanPara("@Registered", Convert.ToBoolean(Convert.ToInt16(controlDataList[2])));
            proc.AddVarcharPara("@GSTIN", 20, controlDataList[3]);
            proc.AddVarcharPara("@Phone", 20, (controlDataList[11] == "null") ? string.Empty : controlDataList[11]);
            proc.AddVarcharPara("@Address", 100, (controlDataList[5] == "null") ? string.Empty : controlDataList[5]);
            proc.AddBigIntegerPara("@CountryID", (controlDataList[6] == "null") ? 0 : Convert.ToInt32(controlDataList[6]));
            proc.AddBigIntegerPara("@StateID", (controlDataList[7] == "null") ? 0 : Convert.ToInt32(controlDataList[7]));
            proc.AddBigIntegerPara("@CityID", (controlDataList[8] == "null") ? 0 : Convert.ToInt32(controlDataList[8]));
            proc.AddVarcharPara("@Pincode", 100, (controlDataList[9] == "null") ? string.Empty : controlDataList[9]);
            proc.AddVarcharPara("@Area", 100, (controlDataList[10] == "null") ? string.Empty : controlDataList[10]);
            proc.AddVarcharPara("@VehicleNos", 500, (controlDataList[4] == "null") ? string.Empty : controlDataList[4]);
            proc.AddNVarcharPara("@Freight", 50, (controlDataList[12].Trim() == "") ? "0" : Convert.ToString(controlDataList[12].Trim()));
            proc.AddNVarcharPara("@Point", 50, (controlDataList[13] == "") ? "0" : Convert.ToString(controlDataList[13].Trim()));
            proc.AddNVarcharPara("@Loading", 50, (controlDataList[14].Trim() == "") ? "0" : Convert.ToString(controlDataList[14].Trim()));
            proc.AddNVarcharPara("@Unloading", 50, (controlDataList[15].Trim() == "") ? "0" : Convert.ToString(controlDataList[15].Trim()));
            proc.AddNVarcharPara("@Parking", 50, (controlDataList[16].Trim() == "") ? "0" : Convert.ToString(controlDataList[16].Trim()));
            proc.AddNVarcharPara("@Weighment", 50, (controlDataList[17].Trim() == "") ? "0" : Convert.ToString(controlDataList[17].Trim()));
            proc.AddNVarcharPara("@Lrno", 50, (controlDataList[18].Trim() == "") ? string.Empty : Convert.ToString(controlDataList[18].Trim()));
            proc.AddNVarcharPara("@VehicleOutDate", 50, (controlDataList[19].Trim() == "01/01/0100 00:00:00") ? null : Convert.ToString(controlDataList[19].Trim()));
            proc.AddNVarcharPara("@TollTax", 50, (controlDataList[20].Trim() == "") ? "0" : Convert.ToString(controlDataList[20].Trim()));
            proc.AddVarcharPara("@Trip", 255, (controlDataList[21] == "null") ? string.Empty : controlDataList[21]);
            proc.AddBigIntegerPara("@FreightArea", (controlDataList[22] == "null") ? 0 : Convert.ToInt64(controlDataList[22]));
            proc.AddNVarcharPara("@TotalCharges", 50, (controlDataList[23] == "") ? "0" : Convert.ToString(controlDataList[23].Trim()));
            proc.AddNVarcharPara("@OtherCharges", 50, (controlDataList[24] == "") ? "0" : Convert.ToString(controlDataList[24].Trim()));
            proc.AddBigIntegerPara("@CreatedBy", Convert.ToInt32(createdBy));
            proc.AddDecimalPara("@Transporter_Distance", 4, 5, Convert.ToDecimal((controlDataList[26] == "") ? "0" : Convert.ToString(controlDataList[26].Trim())));
            proc.AddNVarcharPara("@Vehicle_type", 50, (controlDataList[27] == "") ? "0" : Convert.ToString(controlDataList[27].Trim()));
            proc.AddIntegerPara("@Transporter_Mode", Convert.ToInt32((controlDataList[28] == "") ? "0" : Convert.ToString(controlDataList[28].Trim())));
            proc.AddVarcharPara("@FinalTransID", 20, controlDataList[29]);
            return proc.RunActionQuery();
        }


        public int InsertDeliveryReceiptTransporterControlDetails(long docID, string docType, string controlData, string createdBy)
        {
            string[] controlDataList = controlData.Split('|');

            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("prc_InsertDeliveryReceiptTransporterControlDetails");
            proc.AddBigIntegerPara("@DocID", docID);
            proc.AddVarcharPara("@DocType", 10, docType);
            proc.AddVarcharPara("@TransID", 20, controlDataList[0]);
            proc.AddIntegerPara("@TransType", Convert.ToInt32(controlDataList[1]));
            proc.AddBooleanPara("@Registered", Convert.ToBoolean(Convert.ToInt16(controlDataList[2])));
            proc.AddVarcharPara("@GSTIN", 20, controlDataList[3]);
            proc.AddVarcharPara("@Phone", 20, (controlDataList[11] == "null") ? string.Empty : controlDataList[11]);
            proc.AddVarcharPara("@Address", 100, (controlDataList[5] == "null") ? string.Empty : controlDataList[5]);
            proc.AddBigIntegerPara("@CountryID", (controlDataList[6] == "null") ? 0 : Convert.ToInt32(controlDataList[6]));
            proc.AddBigIntegerPara("@StateID", (controlDataList[7] == "null") ? 0 : Convert.ToInt32(controlDataList[7]));
            proc.AddBigIntegerPara("@CityID", (controlDataList[8] == "null") ? 0 : Convert.ToInt32(controlDataList[8]));
            proc.AddVarcharPara("@Pincode", 100, (controlDataList[9] == "null") ? string.Empty : controlDataList[9]);
            proc.AddVarcharPara("@Area", 100, (controlDataList[10] == "null") ? string.Empty : controlDataList[10]);
            proc.AddVarcharPara("@VehicleNos", 500, (controlDataList[4] == "null") ? string.Empty : controlDataList[4]);
            proc.AddNVarcharPara("@Freight", 50, (controlDataList[12].Trim() == "") ? "0" : Convert.ToString(controlDataList[12].Trim()));
            proc.AddNVarcharPara("@Point", 50, (controlDataList[13] == "") ? "0" : Convert.ToString(controlDataList[13].Trim()));
            proc.AddNVarcharPara("@Loading", 50, (controlDataList[14].Trim() == "") ? "0" : Convert.ToString(controlDataList[14].Trim()));
            proc.AddNVarcharPara("@Unloading", 50, (controlDataList[15].Trim() == "") ? "0" : Convert.ToString(controlDataList[15].Trim()));
            proc.AddNVarcharPara("@Parking", 50, (controlDataList[16].Trim() == "") ? "0" : Convert.ToString(controlDataList[16].Trim()));
            proc.AddNVarcharPara("@Weighment", 50, (controlDataList[17].Trim() == "") ? "0" : Convert.ToString(controlDataList[17].Trim()));
            proc.AddNVarcharPara("@Lrno", 50, (controlDataList[18].Trim() == "") ? string.Empty : Convert.ToString(controlDataList[18].Trim()));
            proc.AddNVarcharPara("@VehicleOutDate", 50, (controlDataList[19].Trim() == "01/01/0100 00:00:00") ? null : Convert.ToString(controlDataList[19].Trim()));
            proc.AddNVarcharPara("@TollTax", 50, (controlDataList[20].Trim() == "") ? "0" : Convert.ToString(controlDataList[20].Trim()));
            proc.AddVarcharPara("@Trip", 255, (controlDataList[21] == "null") ? string.Empty : controlDataList[21]);
            proc.AddBigIntegerPara("@FreightArea", (controlDataList[22] == "null") ? 0 : Convert.ToInt64(controlDataList[22]));
            proc.AddNVarcharPara("@TotalCharges", 50, (controlDataList[23] == "") ? "0" : Convert.ToString(controlDataList[23].Trim()));
            //--
            proc.AddNVarcharPara("@LRDate", 50, (controlDataList[26].Trim() == "01-01-0100") ? null : Convert.ToString(controlDataList[26].Trim()));
            //--
            proc.AddBigIntegerPara("@CreatedBy", Convert.ToInt32(createdBy));
            proc.AddVarcharPara("@FinalTransID", 20, controlDataList[24]);
            proc.AddVarcharPara("@OtherCharges", 20, controlDataList[25]);
            return proc.RunActionQuery();
        }

        public DataSet GetTransporterControlDetails(string docID, string docType)
        {
            try
            {
                DataSet ds = new DataSet();
                ProcedureExecute proc = new ProcedureExecute("prc_GetTransporterControlDetails");
                proc.AddBigIntegerPara("@DocID", Convert.ToInt64(docID));
                proc.AddVarcharPara("@DocType", 20, docType);
                ds = proc.GetDataSet();
                return ds;
            }
            catch (Exception ex) { return null; }
            
        }

        public DataTable GetSendMailPermissionDetails(String Action, Int64 id)
        {
            DataTable ds = new DataTable();
            try
            {
                ProcedureExecute proc = new ProcedureExecute("prc_GetSendMailPermissionDetails");
                proc.AddVarcharPara("@ACTION",200, Action);
                proc.AddBigIntegerPara("@ID", Convert.ToInt64(id));
                ds = proc.GetTable();
            }
            catch (Exception ex) { }
            return ds;
        }


        #endregion

        #region By Sam for Customer For Ship to Pary on 08122017 For Ship to Pary in Transit Purchase Invoice

        public DataTable PopulateShipToCustomerList(string filter, int startindex, int EndIndex)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_PurchaseSideBillingShippingDtl");
            proc.AddVarcharPara("@Action", 500, "PopulateShipToCustomerList");
            proc.AddVarcharPara("@filter", 20, filter);
            proc.AddIntegerPara("@startindex", startindex);
            proc.AddIntegerPara("@EndIndex", EndIndex);
            //proc.AddVarcharPara("@CustInternalID", 20, custInternalID);
            ds = proc.GetTable();
            return ds;
        }

        public DataTable PopulateShipToCustomerByID(string customerId)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_PurchaseSideBillingShippingDtl");
            proc.AddVarcharPara("@Action", 500, "PopulateShipToCustomerByID");
            proc.AddVarcharPara("@CustomerID", 20, customerId); 
            //proc.AddVarcharPara("@CustInternalID", 20, custInternalID);
            ds = proc.GetTable();
            return ds;
        }
        #endregion By Sam for Customer For Ship to Pary on 08122017 in Transit Purchase Invoice Section End 

        #region ### Billing/Shipping Control ###
        public DataSet GetBillingShippingAllCustomer(string custInternalID = null)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("prc_BillingShipping_GetAllCustomer");
            proc.AddVarcharPara("@CustInternalID", 20, custInternalID);
            ds = proc.GetDataSet();
            return ds;
        }

        public DataSet GetBillingShippingAllCountry(string countryID = null)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("prc_BillingShipping_GetAllCountry");
            proc.AddVarcharPara("@CountryID", 20, countryID);
            ds = proc.GetDataSet();
            return ds;
        }

        public DataSet GetBillingShippingAllPinCode(string pinID = null)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("prc_BillingShipping_GetAllPinZip");
            proc.AddNVarcharPara("@PinID", 20, pinID);
            ds = proc.GetDataSet();
            return ds;
        }

        public DataSet GetBillingShippingAddressDetails(string docID, string docType)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("prc_GetBillingShippingAddressDetails");
            proc.AddVarcharPara("@DocID", 20, docID);
            proc.AddVarcharPara("@DocType", 50, docType);
            ds = proc.GetDataSet();
            return ds;
        }
        public DataSet GetBillingShippingCustomerAddressDetails(string customerID, string branchID, string docType)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("prc_GetBillingShippingCustomerAddressDetails");
            proc.AddVarcharPara("@CustomerID", 20, customerID);
            proc.AddVarcharPara("@AddressID", 20, "0"); // 0 refers Blank Address ID
            proc.AddVarcharPara("@BranchID", 20, branchID);
            proc.AddVarcharPara("@DocType", 20, docType);
            ds = proc.GetDataSet();
            return ds;
        }
        public DataSet GetBillingShippingCustomerAddressDetailsByAddressID(string addressID, string docType, bool shippingCustomer = false, string branchID = "")
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("prc_GetBillingShippingCustomerAddressDetails");
            proc.AddVarcharPara("@CustomerID", 20, "0"); // 0 refers Blank Customer ID
            proc.AddVarcharPara("@AddressID", 20, addressID);
            proc.AddVarcharPara("@DocType", 20, (shippingCustomer == true) ? "SO" : docType);  //// shippingCustomer is true means fetch customer address
            proc.AddVarcharPara("@BranchID", 20, branchID);
            ds = proc.GetDataSet();
            return ds;
        }

        public DataSet GetAllBillingShippingCustomerAddressDetails(string customerID, string branchID, string docType)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("prc_GetBillingShippingCustomerAllAddressDetails");
            proc.AddVarcharPara("@CustomerID", 20, customerID);
            proc.AddVarcharPara("@BranchID", 20, branchID);
            proc.AddVarcharPara("@DocType", 20, docType);
            ds = proc.GetDataSet();
            return ds;
        }

        public DataSet GetGSTINBillingShipping(string customerID, string stateID, string branchID, string docType)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("prc_GetGSTINforBillingShipping");
            proc.AddVarcharPara("@CustomerID", 20, customerID);
            proc.AddVarcharPara("@StateID", 20, stateID);
            proc.AddVarcharPara("@BranchID", 20, branchID);
            proc.AddVarcharPara("@DocType", 20, docType);
            ds = proc.GetDataSet();
            return ds;
        }

        public DataTable GetCustomerNameByCustID(string custID, string docID = "", string docType = "")
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_BillingShipping_CustomerDetailsByCustomerID");
            proc.AddVarcharPara("@InternalID", 20, custID);
            proc.AddVarcharPara("@DocID", 20, docID);
            proc.AddVarcharPara("@DocType", 20, docType);
            dt = proc.GetTable();
            return dt;
        }

        #endregion

        #region ### Common Methods for Custom User Control ###
        public DataSet GetStateByCountryForBillingShipping(string countryID)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("prc_GetStateByCountryForBillingShipping");
            proc.AddVarcharPara("@CountryID", 20, countryID);
            ds = proc.GetDataSet();
            return ds;
        }
        public DataSet GetStateByCountry(string countryID)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("prc_GetStateByCountry");
            proc.AddVarcharPara("@CountryID", 20, countryID);
            ds = proc.GetDataSet();
            return ds;
        }

        public DataSet GetCityByState(string stateID)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("prc_GetCityByState");
            proc.AddVarcharPara("@StateID", 20, stateID);
            ds = proc.GetDataSet();
            return ds;
        }

        public DataSet GetPinByCity(string cityID)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("prc_GetPinByCity");
            proc.AddVarcharPara("@cityID", 20, cityID);
            ds = proc.GetDataSet();
            return ds;
        }

        public DataSet GetAreaByCity(string cityID)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("prc_GetAreaByCity");
            proc.AddVarcharPara("@cityID", 20, cityID);
            ds = proc.GetDataSet();
            return ds;
        }

        public string GetVendorGSTIN(string vendorID, string branchID)
        {
            string VendorGSTIN = string.Empty;
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_GetVendorGSTIN");
            proc.AddVarcharPara("@VendorID", 20, vendorID);
            proc.AddIntegerPara("@BranchID", Convert.ToInt32(branchID));
            dt = proc.GetTable();

            if (dt.Rows.Count > 0)
            {
                VendorGSTIN = Convert.ToString(dt.Rows[0]["CNT_GSTIN"]);
            }

            return VendorGSTIN;
        }
        #endregion



        #region ### Financialyear wise Date ###
        public DataTable GetDateFinancila(string Finyear)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("Getdatefromfinyear");
            proc.AddPara("@Finyear", Finyear);
            ds = proc.GetTable();
            return ds;
        }

        #endregion


        #region ### head Office Branch Bind###
        //Rev Debashis && Hierarchy wise Head Branch Bind
        //public DataTable GetBranchheadoffice(string Action)
        //{
        //    DataTable ds = new DataTable();
        //    ProcedureExecute proc = new ProcedureExecute("Get_AllbranchHO");
        //    proc.AddPara("@Ation", Action);
        //    ds = proc.GetTable();
        //    return ds;
        //}
        public DataTable GetBranchheadoffice(string CHILDBRANCH,string Action)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("Get_AllbranchHO");
            proc.AddPara("@CHILDBRANCH", CHILDBRANCH);
            proc.AddPara("@Ation", Action);
            ds = proc.GetTable();
            return ds;
        }

        public DataTable GetHierarchyWiseBranchheadoffice(string CHILDBRANCH)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("PRC_HIERARCHYWISEHEADBRANCHBIND");
            proc.AddPara("@CHILDBRANCH", CHILDBRANCH);
            ds = proc.GetTable();
            return ds;
        }
        //End of Rev Debashis
        public DataTable GetLayout(string LAYOUT_FOR)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("Get_Layout");
            proc.AddPara("@LAYOUT_FOR", LAYOUT_FOR);
            ds = proc.GetTable();
            return ds;
        }
        #endregion

        #region ### Ledger Bind Report ###
        public DataTable GetLedgerBind(string branch)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("Proc_GetLedgerPosting");
            proc.AddPara("@BranchId", branch);
            ds = proc.GetTable();
            return ds;
        }

        #endregion
    }
}
