using DataAccessLayer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace BusinessLogicLayer.Budget
{

    public class Customerbudget
    {
        public IEnumerable GetBudget()
        {
            List<BudgetCustomer> QuotationList = new List<BudgetCustomer>();
            DataTable Quotationdt = GetCustomerbudgetData("30231", "2016-2017").Tables[0];

            for (int i = 0; i < Quotationdt.Rows.Count; i++)
            {
                BudgetCustomer Quotations = new BudgetCustomer();

                Quotations.BudgetId = Convert.ToInt32(Quotationdt.Rows[i]["BudgetId"]);
                Quotations.CustomerId = Convert.ToInt32(Quotationdt.Rows[i]["CustomerId"]);
                Quotations.ProductId = Convert.ToInt32(Quotationdt.Rows[i]["ProductId"]);
                Quotations.Qty_CurrentFY = Convert.ToDecimal(Quotationdt.Rows[i]["Qty_CurrentFY"]);
                Quotations.Qty_PreviousFY = Convert.ToDecimal(Quotationdt.Rows[i]["Qty_PreviousFY"]);
                Quotations.Qty_Permonth = Convert.ToDecimal(Quotationdt.Rows[i]["Qty_Permonth"]);
                QuotationList.Add(Quotations);
            }

            return QuotationList;
        }

        public IEnumerable GetBudget(DataTable Quotationdt)
        {
            List<BudgetCustomer> QuotationList = new List<BudgetCustomer>();

            if (Quotationdt != null && Quotationdt.Rows.Count > 0)
            {
                for (int i = 0; i < Quotationdt.Rows.Count; i++)
                {
                    BudgetCustomer Quotations = new BudgetCustomer();
                    if (string.IsNullOrEmpty(Convert.ToString(Quotationdt.Rows[i]["BudgetId"])))
                    {
                        Quotations.BudgetId = 1;

                    }
                    else
                    {
                        Quotations.BudgetId = Convert.ToInt64(Quotationdt.Rows[i]["BudgetId"]);
                    }
                    Quotations.CustomerId = Convert.ToInt64(Quotationdt.Rows[i]["CustomerId"]);
                    Quotations.ProductId = Convert.ToInt64(Quotationdt.Rows[i]["ProductID"]);
                    Quotations.Qty_CurrentFY = Convert.ToDecimal(Quotationdt.Rows[i]["Qty_CurrentFY"]);
                    Quotations.Qty_PreviousFY = Convert.ToDecimal(Quotationdt.Rows[i]["Qty_PreviousFY"]);
                    Quotations.Qty_Permonth = Convert.ToDecimal(Quotationdt.Rows[i]["Qty_Permonth"]);
                    QuotationList.Add(Quotations);
                }
            }

            return QuotationList;
        }


        public DataSet GetCustomerbudgetData(string CustomerId, string FinYear)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("Sp_GetbudgetdataCustomerwise");
            proc.AddPara("@CustomerID", CustomerId);
            proc.AddPara("@Finyear", FinYear);
            ds = proc.GetDataSet();
            return ds;
        }

        public DataSet GetProductDetails(string CustomerId)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("Sp_GetbudgetProductData");
            proc.AddPara("@CustomerID", CustomerId);
            ds = proc.GetDataSet();
            return ds;
        }

        public DataSet GetProductDetailsbypructandproductclass(string productid, string Qty, string CustomerID)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("Sp_GetbudgetProductDataProductandProductclasswise");
            proc.AddPara("@Qty", Qty);
            proc.AddPara("@ProductID", productid);
            proc.AddPara("@CustomerID", CustomerID);
            ds = proc.GetDataSet();
            return ds;
        }



        public DataTable GetProductClassdetailsBudget(string CustomerId)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("Proc_ProductclassforBudget");
            proc.AddPara("@CustomerInternalId", CustomerId);
            ds = proc.GetTable();
            return ds;
        }


        public DataTable GetProductDetailsClasswise(string classId, string CustomerId)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("GetProductBind_ProductclassBind");

            proc.AddPara("@ClassId", classId);
            proc.AddPara("@CustomerId", CustomerId);
            ds = proc.GetTable();
            return ds;
        }



        public int InsertCustomerBudget(string Data, string CustomerID)
        {
            DataSet dsInst = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("Sp_BudgetInsertion");
            proc.AddPara("@BudgetXML", Data);
            proc.AddPara("@CustomerID", CustomerID);
            return proc.RunActionQuery();
        }
        public int InsertCustomerBudget(string Data, string CustomerID, string slsid)
        {
            DataSet dsInst = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("Sp_BudgetInsertion");
            proc.AddPara("@BudgetXML", Data);
            proc.AddPara("@CustomerID", CustomerID);
            proc.AddPara("@sls_id", slsid);
            return proc.RunActionQuery();
        }
        public static string ConvertToXml<T>(List<T> table, int metaIndex = 0)
        {
            XmlDocument ChoiceXML = new XmlDocument();
            ChoiceXML.AppendChild(ChoiceXML.CreateElement("root"));
            Type temp = typeof(T);

            foreach (var item in table)
            {
                XmlElement element = ChoiceXML.CreateElement("data");

                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    element.AppendChild(ChoiceXML.CreateElement(pro.Name)).InnerText = Convert.ToString(item.GetType().GetProperty(pro.Name).GetValue(item, null));
                }
                ChoiceXML.DocumentElement.AppendChild(element);
            }

            return ChoiceXML.InnerXml.ToString();
        }
    }





    public class BudgetCustomer
    {
        public long BudgetId { get; set; }
        public long CustomerId { get; set; }
        public long ProductId { get; set; }
        public decimal Qty_CurrentFY { get; set; }
        public decimal Qty_PreviousFY { get; set; }
        public decimal Qty_Permonth { get; set; }

        public string ProductName { get; set; }
        public string Description { get; set; }
        public string UOM { get; set; }
        public string Industry { get; set; }
        public string Productclass { get; set; }
        public int CreatedBy { get; set; }
        public string FiscalYear { get; set; }


    }




}
