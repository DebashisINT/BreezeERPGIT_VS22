/**********************************************************************************************************************
 * Rev 1.0      Sanchita    V2.0.38     26-05-2023      Party Invoice No & Party Invoice Date fields required in the 
 *                                                      Consolidated Vendor Opening Module. Refer: 25889
 * **********************************************************************************************************************/
using DataAccessLayer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace OpeningBusinessLogic.Vendorconsolidate
{

    public class VendorConsolidate
    {

        ProcedureExecute proc;


        public DataTable GetBranch(int LoggedInBranchid, string BranchList)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("GetBranch");
            proc.AddIntegerPara("@LoggedInBranchid", LoggedInBranchid);
            proc.AddPara("@BranchList", BranchList);
            ds = proc.GetTable();
            return ds;
        }

        public DataTable PopulateContactPersonOfCustomer(string InternalId)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("Proc_Opening_VendorConsolidate");
            proc.AddVarcharPara("@Action", 100, "PopulateContactPersonOfCustomer");
            proc.AddVarcharPara("@InternalId", 100, InternalId);
            ds = proc.GetTable();
            return ds;
        }

        public int InsertReplacementDetails(string modelXML, string Action, string Amount, string type, string CusID, int ModId, Int64 ProjID, string RetAmount, string Gl, DateTime? DueDate)
        {
            int i;
            int rtrnvalue = 0;
            DataSet dsInst = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("Proc_Opening_VendorConsolidate");
            proc.AddPara("@lstConsolidateXML", modelXML);        
            proc.AddPara("@Action", Action);
            proc.AddPara("@OSAmount", Amount);
            proc.AddPara("@type", type);
            proc.AddPara("@CustomerID", CusID);
            proc.AddPara("@Mod", ModId);
            proc.AddPara("@Project_Id", ProjID);

            proc.AddPara("@RetAmount", RetAmount);
            proc.AddPara("@Gl", Gl);
            proc.AddPara("@DueDate", DueDate);


            return proc.RunActionQuery();
            i = proc.RunActionQuery();
            rtrnvalue = Convert.ToInt32(proc.GetParaValue("@ReturnValue"));
            return rtrnvalue;
        }

        public int IDeleteReplacementDetails(int Mod, string Action)
        {
            DataSet dsInst = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("Proc_Opening_VendorConsolidate");
            proc.AddPara("@Mod", Mod);
            proc.AddPara("@Action", Action);
            return proc.RunActionQuery();
        }
        public DataTable GetCustomersalesFinancialyear(string FinYearcode)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("Opening_CustomerSales_GetDateFinbancialYear");
            proc.AddPara("@FinYearcode", FinYearcode);
            proc.AddPara("@Mode", 1);
            ds = proc.GetTable();
            return ds;
        }

        public DataTable GetCustomersalesFinancialyearCode(DateTime FinYearDate)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("Opening_CustomerSales_GetDateFinbancialYear");
            proc.AddPara("@FinDatecode", FinYearDate);
            proc.AddPara("@Mode", 2);
            ds = proc.GetTable();
            return ds;
        }

        public DataTable GetCustomesconsolidate(string Action, int branch)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("Proc_Opening_VendorConsolidate");
            proc.AddPara("@Action", Action);
            proc.AddPara("@branch", @branch);
            dt = proc.GetTable();
            return dt;
        }

        public DataTable GetCustomesconsolidate(string Action,string CustomerId,int branchid)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("Proc_Opening_VendorConsolidate");
            proc.AddPara("@Action", Action);
            proc.AddPara("@InternalId", CustomerId);
            proc.AddPara("@branch", branchid);
            dt = proc.GetTable();
            return dt;
        }

        public DataTable GetVendorpopulateBranch(int branchid)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("Proc_Opening_VendorConsolidate");
            proc.AddPara("@Action", "Customerbind");
  
            proc.AddPara("@branch", branchid);
            dt = proc.GetTable();
            return dt;
        }

        public DataTable GetDuplicateDoc(string Action,string Doc, string Mod)
        {
               DataTable dt = new DataTable();
            if(string.IsNullOrEmpty(Mod))
            {

                Mod = null;
            }
            ProcedureExecute proc = new ProcedureExecute("Proc_Opening_VendorConsolidate");
            proc.AddPara("@Action", Action);
            proc.AddPara("@Mod", Mod);
            proc.AddPara("@DocNo", Doc);
            dt = proc.GetTable();
            return dt;
        }

        public DataTable GetCustomesconsolidateTagged(string Action, string Customer)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("Proc_Opening_VendorConsolidate");
            proc.AddPara("@Action", Action);
            proc.AddPara("@InternalId", Customer);
            dt = proc.GetTable();
            return dt;
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

    public class ConsolidatedVendorClass
    {
        public int Slno { get; set; }
        public int ModId { get; set; }
        public string Branch { get; set; }
        public string BranchId { get; set; }
        public string Customer { get; set; }
        public string CustomerId { get; set; }
        public string Type { get; set; }
        public string TypeId { get; set; }
        public string DocNumber { get; set; }
        public string Date { get; set; }
        public DateTime? Date_db { get; set; }
        public string FullBill { get; set; }
        public string DueDate { get; set; }
        public DateTime? DueDate_db { get; set; }
        public string RefDate { get; set; }
        public DateTime? RefDate_db { get; set; }
        public string DocAmount { get; set; }
        public bool DeclarationForm { get; set; }

        public string DSAmount { get; set; }

        public bool FormType { get; set; }
        public string AgentName { get; set; }
        public string AgentId { get; set; }

        public string Commpercntag { get; set; }
        public string CommmAmt { get; set; }
        public string FinYear { get; set; }
        public string User { get; set; }

        public string Company { get; set; }

        // Rev 1.0
        public string PartyInvNo { get; set; }
        public DateTime? PartyInvDate { get; set; }
        // End of Rev 1.0
    }


}
