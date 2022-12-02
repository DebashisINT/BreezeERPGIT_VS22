using BusinessLogicLayer;
using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;

namespace ERP.OMS.Management.Activities.Services
{
    /// <summary>
    /// Summary description for ProjectTermsService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
     [System.Web.Script.Services.ScriptService]
    public class ProjectTermsService : System.Web.Services.WebService
    {



        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public String AddData(string Terms_BankGuaranteeSL, string BGGroup, string BGType, string Percentage, String Value, string Status, string ValidityFromDate, string ValidityToDate)
        {

            DataTable dt = (DataTable)HttpContext.Current.Session["BGDetails"];

            if (dt == null)
            {
                DataTable dtable = new DataTable();

                dtable.Clear();
                dtable.Columns.Add("Terms_BankGuaranteeSL", typeof(System.Guid));
                dtable.Columns.Add("BGGroup", typeof(System.String));
                dtable.Columns.Add("BGType", typeof(System.String));
                dtable.Columns.Add("Percentage", typeof(System.String));
                dtable.Columns.Add("Value", typeof(System.String));
                dtable.Columns.Add("Status", typeof(System.String));
                dtable.Columns.Add("ValidityFromDate", typeof(System.String));
                dtable.Columns.Add("ValidityToDate", typeof(System.String));


                object[] trow = { Guid.NewGuid(), BGGroup, BGType, Percentage, Value, Status, ValidityFromDate, ValidityToDate };// Add new parameter Here
                dtable.Rows.Add(trow);
                HttpContext.Current.Session["BGDetails"] = dtable;
            }
            else
            {
                if (string.IsNullOrEmpty(Terms_BankGuaranteeSL))
                {
                    object[] trow = { Guid.NewGuid(), BGGroup, BGType, Percentage, Value, Status, ValidityFromDate, ValidityToDate };// Add new parameter Here
                    dt.Rows.Add(trow);
                }
                else
                {
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow item in dt.Rows)
                        {
                            if (Terms_BankGuaranteeSL.ToString() == item["Terms_BankGuaranteeSL"].ToString())
                            {


                                item["Terms_BankGuaranteeSL"] = Terms_BankGuaranteeSL;
                                item["BGGroup"] = BGGroup;
                                item["BGType"] = BGType;
                                item["Percentage"] = Percentage;
                                item["Value"] = Value;
                                item["Status"] = Status;
                                item["ValidityFromDate"] = ValidityFromDate;
                                item["ValidityToDate"] = ValidityToDate;

                            }
                        }
                    }
                    
                }

                HttpContext.Current.Session["BGDetails"] = dt;
                
            }
            return "Bank guarantee is Added Successfully.";
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public  BankGuaranteeDetails EditData(String Terms_BankGuaranteeSL)
        {
            DataTable dt = (DataTable)HttpContext.Current.Session["BGDetails"];
            BankGuaranteeDetails ret = new BankGuaranteeDetails();
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow item in dt.Rows)
                {
                    if (Terms_BankGuaranteeSL.ToString() == item["Terms_BankGuaranteeSL"].ToString())
                    {
                        ret.Terms_BankGuaranteeSL = item["Terms_BankGuaranteeSL"].ToString();
                        ret.BGGroup = item["BGGroup"].ToString();
                        ret.BGType = item["BGType"].ToString();
                        ret.Percentage = item["Percentage"].ToString();
                        ret.Value = item["Value"].ToString();
                        ret.Status = item["Status"].ToString();
                        ret.ValidityFromDate = Convert.ToDateTime(item["ValidityFromDate"].ToString());
                        ret.ValidityToDate = Convert.ToDateTime(item["ValidityToDate"].ToString());
                       
                        // dt.Rows.Remove(item);
                        break;
                    }
                }
            }
            return ret;// "Holiday Remove Sucessfylly";
        }


        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public  String DeleteData(string Terms_BankGuaranteeSL)
        {
            DataTable dt = (DataTable)HttpContext.Current.Session["BGDetails"];

            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow item in dt.Rows)
                {
                    if (Terms_BankGuaranteeSL.ToString() == item["Terms_BankGuaranteeSL"].ToString())
                    {
                        dt.Rows.Remove(item);
                        break;
                    }
                }
            }
            return "Bank guarantee  Remove Successfully.";
        }


        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public int GetprojectTerms(DateTime DefLiaPer, string DefLiaPerRemarks, string Liqdamageper, DateTime LiqDamageApplicableDt, string PaymentTerms, string OrderType, string NatureofWork)
        {
            DataTable dt = (DataTable)HttpContext.Current.Session["BGDetails"];
           // ProjectTermsBL ProjectTermsBL = new ProjectTermsBL();
          //  int Returnval = ProjectTermsBL.ProjectTermsCoditionsSave(DefLiaPer, DefLiaPerRemarks, Liqdamageper, LiqDamageApplicableDt, PaymentTerms, OrderType, NatureofWork,dt);

        //      public int ProjectTermsCoditionsSave(DateTime DefLiaPer, string DefLiaPerRemarks, string Liqdamageper, DateTime LiqDamageApplicableDt, string PaymentTerms, string OrderType, string NatureofWork,datatab)
        //{
            
            int i;
            int rtrnvalue = 0;
            ProcedureExecute proc = new ProcedureExecute("Prc_Project_TermsConditions");
            proc.AddVarcharPara("@Action", 500, "Add");
            //proc.AddVarcharPara("@Doc_id", Vendorid);
            proc.AddDateTimePara("@Terms_DefectLibilityPeriodDate", DefLiaPer);
            proc.AddVarcharPara("@Terms_DefectLibilityPeriodRemarks", 500, DefLiaPerRemarks);
            proc.AddVarcharPara("@Terms_LiqDamage", 100, Liqdamageper);
            proc.AddDateTimePara("@Terms_LiqDamageAppDate", LiqDamageApplicableDt);
            proc.AddVarcharPara("@Terms_Payment", 100, PaymentTerms);
            proc.AddVarcharPara("@Terms_OrderType", 100, OrderType);
            proc.AddVarcharPara("@Terms_NatureWork", 100, NatureofWork);
           
            proc.AddIntegerPara("@Terms_CreatedBy", Convert.ToInt32(Session["userid"]));
            proc.AddPara("@BGTABLE", dt);
            proc.AddVarcharPara("@ReturnValue", 50, "", QueryParameterDirection.Output);
            i = proc.RunActionQuery();
            rtrnvalue = Convert.ToInt32(proc.GetParaValue("@ReturnValue"));
            return rtrnvalue;
           }

    }


    public class BankGuaranteeDetails
    {
        //public BankGuaranteeDetails();
        public string BGGroup { get; set; }
        public string BGType { get; set; }
        public string Percentage { get; set; }
        public string Value { get; set; }
        public string Status { get; set; }

        public DateTime ValidityFromDate { get; set; }
        public string Terms_BankGuaranteeSL { get; set; }
  
        public DateTime ValidityToDate { get; set; }
    }

}
