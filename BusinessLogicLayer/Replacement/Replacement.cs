using DataAccessLayer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Replacement
{
    public class Replacement
    {

        public bool CheckUnique(string shortname, string code, string action)
        {
            ProcedureExecute proc;

            try
            {
                using (proc = new ProcedureExecute("prc_MShortNameCheckingDtl"))
                {
                    proc.AddVarcharPara("@action", 50, action);
                    proc.AddBooleanPara("@ReturnValue", false, QueryParameterDirection.Output);
                    proc.AddVarcharPara("@ShortName", 50, shortname);
                    proc.AddVarcharPara("@code", 50, code);

                    int i = proc.RunActionQuery();
                    var retData = Convert.ToBoolean(proc.GetParaValue("@ReturnValue"));
                    return retData;
                }
            }

            catch (Exception ex)
            {
                return false;
            }

            finally
            {
                proc = null;
            }

        }

        public DataTable GetNumberingSchema(string strCompanyID, string strBranchID, string strFinYear, string strType, string strIsSplit)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_SalesActivity");
            proc.AddVarcharPara("@Action", 100, "GetNumberingSchema");
            proc.AddVarcharPara("@CompanyID", 100, strCompanyID);
            proc.AddVarcharPara("@BranchID", 4000, strBranchID);
            proc.AddVarcharPara("@FinYear", 100, strFinYear);
            proc.AddVarcharPara("@Type", 100, strType);
            proc.AddVarcharPara("@IsSplit", 100, strIsSplit);
            ds = proc.GetTable();
            return ds;
        }
        public DataSet GetAllDropDownDetailForSalesInvoice(string userbranch, string CompanyID, string BranchID)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("prc_CRMSalesInvoice_Details");
            proc.AddVarcharPara("@Action", 100, "GetAllDropDownDetailForSalesQuotation");
            proc.AddVarcharPara("@BranchList", 100, userbranch);
            proc.AddVarcharPara("@CompanyID", 100, CompanyID);
            proc.AddVarcharPara("@BranchID", 100, BranchID);
            ds = proc.GetDataSet();
            return ds;
        }

        public DataTable GetComponentInvoice(string Customer, string Date, string FinYear, string BranchId)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("Proc_Replacement_InvoiceDetails");

            proc.AddPara("@CustomerId", Customer);
            proc.AddPara("@Date", Date);
            proc.AddPara("@FinYear", FinYear);
            proc.AddPara("@BranchId", BranchId);

            //proc.AddVarcharPara("@InvoiceID", 20, strInvoiceID);


            dt = proc.GetTable();
            return dt;
        }

        public DataTable GetComponentInvoiceProductList(string ComponentList, string Action)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("Proc_Replacement_InvoicewiseProducts");

            proc.AddPara("@ComponentList", ComponentList);
            proc.AddPara("@Action", Action);
            dt = proc.GetTable();
            return dt;
        }


        #region Grid Replacement List Bind

        public DataTable GetReplacementListProductList(string BranchId, string ComapnyId, string Finyear)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("Proc_Replacement_BindGrid");
            proc.AddPara("@branchId", BranchId);
            proc.AddPara("@companyId", ComapnyId);
            proc.AddPara("@finyear", Finyear);
            dt = proc.GetTable();
            return dt;
        }


        #endregion


        #region Grid Replacement Modify  and Delete

        public DataSet GetReplacementPopulateModify(string ReplacementId)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("Proc_Replacement_GridBind");
            proc.AddPara("@ReplacementId", ReplacementId);

            ds = proc.GetDataSet();
            return ds;
        }


        public int DeleteReplacement(string ReplacementId)
        {
            int i;
            int rtrnvalue = 0;
            ProcedureExecute proc = new ProcedureExecute("Proc_Replacement_Delete");

            proc.AddPara("@ReplacementId", ReplacementId);
            i = proc.RunActionQuery();

            return rtrnvalue;

        }


        #endregion

        public IEnumerable GetReplacementDetails(DataTable Quotationdt)
        {
            List<ReplacementGridData> QuotationList = new List<ReplacementGridData>();

            if (Quotationdt != null && Quotationdt.Rows.Count > 0)
            {
                for (int i = 0; i < Quotationdt.Rows.Count; i++)
                {
                    ReplacementGridData Quotations = new ReplacementGridData();

                    //    Quotations.SrlNo = Convert.ToString(Quotationdt.Rows[i]["SrlNo"]);

                    Quotations.ComponentDetailsID = Convert.ToString(Quotationdt.Rows[i]["ComponentDetailsID"]);
                    Quotations.ProductID = Convert.ToString(Quotationdt.Rows[i]["ProductID"]);
                    Quotations.ComponentNumber = Convert.ToString(Quotationdt.Rows[i]["ComponentNumber"]);
                    Quotations.Quantity = Convert.ToString(Quotationdt.Rows[i]["Quantity"]);
                    Quotations.UOM = Convert.ToString(Quotationdt.Rows[i]["UOM"]);
                    Quotations.ProductDescription = Convert.ToString(Quotationdt.Rows[i]["ProductDescription"]);
                    Quotations.Rate = Convert.ToString(Quotationdt.Rows[i]["Rate"]);

                    //   Quotations.Warehouse = "";


                    QuotationList.Add(Quotations);
                }
            }

            return QuotationList;
        }

        #region WareHouse
        public void GetProductType(string hdprod, ref string Type)
        {
            ProcedureExecute proc = new ProcedureExecute("prc_SalesOrder_Details");
            proc.AddVarcharPara("@Action", 500, "GetSchemeType");
            proc.AddVarcharPara("@ProductID", 100, Convert.ToString(hdprod));
            DataTable dt = proc.GetTable();

            if (dt != null && dt.Rows.Count > 0)
            {
                Type = Convert.ToString(dt.Rows[0]["Type"]);
            }
        }

        public void GetProductUOM(ref string Sales_UOM_Name, ref string Sales_UOM_Code, ref string Stk_UOM_Name, ref string Stk_UOM_Code, ref string Conversion_Multiplier, string ProductID)
        {
            DataTable Productdt = GetProductDetailsData(ProductID);
            if (Productdt != null && Productdt.Rows.Count > 0)
            {
                Sales_UOM_Name = Convert.ToString(Productdt.Rows[0]["Sales_UOM_Name"]);
                Sales_UOM_Code = Convert.ToString(Productdt.Rows[0]["Sales_UOM_Code"]);
                Stk_UOM_Name = Convert.ToString(Productdt.Rows[0]["Stk_UOM_Name"]);
                Stk_UOM_Code = Convert.ToString(Productdt.Rows[0]["Stk_UOM_Code"]);
                Conversion_Multiplier = Convert.ToString(Productdt.Rows[0]["Conversion_Multiplier"]);
            }
        }

        public DataTable GetProductDetailsData(string ProductID)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_SalesOrder_Details");
            proc.AddVarcharPara("@Action", 500, "ProductDetailsSearch");
            proc.AddVarcharPara("@ProductID", 500, ProductID);
            dt = proc.GetTable();
            return dt;
        }


        public DataTable GetWarehouseData(string ReplavementId, string ProductId, string Finyear, string BranchId, string CompanyId)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("Proc_replacement_GetWareHouse");
            proc.AddPara("@ReplacementId", ReplavementId);
            proc.AddPara("@ProductID", ProductId);
            proc.AddPara("@FinYear", Finyear);
            proc.AddPara("@branchId", BranchId);
            proc.AddPara("@companyId", CompanyId);
            dt = proc.GetTable();
            return dt;

        }


        #endregion

        public int InsertReplacementDetails(ReplacementGridsavedata model, string Action)
        {
            DataSet dsInst = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("Proc_ReplacementMangedata");
            proc.AddPara("@CreatedBy", model.CreatedBy);
            proc.AddPara("@FiscalYear", model.FiscalYear);
            proc.AddPara("@Branch", model.Branch);
            proc.AddPara("@Customer", model.Customer);
            proc.AddPara("@ReplacementDate", model.ReplacementDate);
            proc.AddPara("@ReplacementNumber", model.ReplacementNumber);
            proc.AddPara("@Company", model.Company);
            proc.AddPara("@lstreplacementXML", model.lstreplacementXML);
            proc.AddPara("@lstreplacementbillingXML", model.lstreplacementaddressXML);
            proc.AddPara("@ReplacementId", model.ReplacementId);
            proc.AddPara("@InvoiceId", model.InvoiceId);
            proc.AddPara("@Action", Action);
            return proc.RunActionQuery();
        }

        public DataTable GetBranch(int LoggedInBranchid, string BranchList)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("GetBranch");
            proc.AddIntegerPara("@LoggedInBranchid", LoggedInBranchid);
            proc.AddPara("@BranchList", BranchList);
            ds = proc.GetTable();
            return ds;
        }



        public static string AssignedBranch(object _Obj1)
        {
            try
            {
                dynamic _Obj = _Obj1;
                //oldunit _Obj = (oldunit)_Obj1;
                //_Obj = (oldunit)_Obj1;
                string msg = "";
                ProcedureExecute proc = new ProcedureExecute("SP_Replacemrnt_BranchAssign");

                proc.AddIntegerPara("@Replacement_Id", _Obj.Replacement_Id);
                proc.AddVarcharPara("@financial_year", 100, _Obj.financial_year);
                proc.AddVarcharPara("@assign_from_branch", 100, _Obj.assign_from_branch);
                proc.AddVarcharPara("@assignee_remark", 100, _Obj.assignee_remark);
                proc.AddIntegerPara("@assigned_by", _Obj.assigned_by);
                proc.AddVarcharPara("@assign_to_branch", 100, _Obj.assign_to_branch);
                proc.AddDateTimePara("@assigned_on", DateTime.Now);
                proc.AddIntegerPara("@current_status", 1);
                proc.AddVarcharPara("@company_Id", 100, _Obj.company_Id);

                int _Ret = proc.RunActionQuery() > 0 ? 1 : -1;

                if (_Ret == 1)
                {
                    msg = "Branch assigned successfully.";
                }
                else
                {
                    msg = "Error occured.";
                }
                return msg;
            }
            catch (Exception ex)
            {
                return "Error occured.";
            }
        }
    }


    public class ReplacementGridData
    {
        public string ComponentDetailsID { get; set; }
        public string ProductID { get; set; }
        public string ComponentNumber { get; set; }
        public string ProductsName { get; set; }
        public string ProductDescription { get; set; }
        public string Quantity { get; set; }
        public string InputQuantity { get; set; }
        public string UOM { get; set; }
        public string Rate { get; set; }
        public string Serials { get; set; }

        public long InvoicedetailId { get; set; }

        public string InvoiceId { get; set; }
    }



    public class ReplacementGridsavedata
    {
        public string ReplacementNumber { get; set; }
        public DateTime ReplacementDate { get; set; }
        public string Branch { get; set; }
        public string Customer { get; set; }
        public string FiscalYear { get; set; }
        public string CreatedBy { get; set; }
        public string lstreplacementXML { get; set; }

        public string lstreplacementaddressXML { get; set; }

        public string Company { get; set; }

        public int ReplacementId { get; set; }
        public string InvoiceId { get; set; }

    }


    public class Branchassign_replacement
    {
        public int Id { get; set; }
        public int Replacement_Id { get; set; }
        public string financial_year { get; set; }
        public string assign_from_branch { get; set; }
        public string assignee_remark { get; set; }
        public int assigned_by { get; set; }
        public string assign_to_branch { get; set; }
        public string receiver_remark { get; set; }
        public int received_by { get; set; }
        public DateTime assigned_on { get; set; }
        public DateTime received_on { get; set; }
        public int current_status { get; set; }
        public string company_Id { get; set; }
    }



    public class ReplacementBillingShippingAddress
    {
        public string AddressType { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string LandMark { get; set; }
        public string CountryID { get; set; }
        public string StateID { get; set; }
        public string CityID { get; set; }
        public string Pincode { get; set; }
        public string Area { get; set; }
        public string GSTIN { get; set; }
        public string ShipToParty { get; set; }
    }

}
