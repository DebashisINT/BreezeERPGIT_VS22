using BusinessLogicLayer;
using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;

namespace ERP.OMS.Management.Activities.Services
{
    /// <summary>
    /// Summary description for BillingShipping
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class BillingShipping : System.Web.Services.WebService
    {

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object FetchCustomAddress(string CustomerId)
        {
            List<CustomerDetails> Details = new List<CustomerDetails>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                ProcedureExecute proc = new ProcedureExecute("Prc_ServiceBillingShipping");
                proc.AddVarcharPara("@Action", 50, "GetByCustomerId");
                proc.AddVarcharPara("@CustomerId", 50, CustomerId);
                DataTable addtable = proc.GetTable();

                Details = (from DataRow dr in addtable.Rows
                           select new CustomerDetails()
                           {

                               Id = Convert.ToInt32(dr["Id"]),
                               Type = Convert.ToString(dr["Type"]),
                               Address1 = Convert.ToString(dr["Address1"]),
                               Address2 = Convert.ToString(dr["Address2"]),
                               Address3 = Convert.ToString(dr["Address3"]),
                               CountryId = Convert.ToInt32(dr["CountryId"]),
                               CountryName = Convert.ToString(dr["CountryName"]),
                               StateId = Convert.ToInt32(dr["StateId"]),
                               StateName = Convert.ToString(dr["StateName"]),
                               StateCode = Convert.ToString(dr["StateCode"]),
                               CityId = Convert.ToInt32(dr["CityId"]),
                               CityName = Convert.ToString(dr["CityName"]),
                               AreaId = Convert.ToInt32(dr["AreaId"]),
                               AreaName = Convert.ToString(dr["AreaName"]),
                               PinId = Convert.ToInt32(dr["PinId"]),
                               PinCode = Convert.ToString(dr["PinCode"]),
                               Landmark = Convert.ToString(dr["Landmark"]),
                               Distance = Convert.ToDecimal(dr["Distance"]),
                               GSTIN = Convert.ToString(dr["GSTIN"]),
                               ContactPerson = Convert.ToString(dr["ContactPerson"]),
                               Phone = Convert.ToString(dr["Phone"]),
                               Email = Convert.ToString(dr["Email"]),
                               Isdefault = Convert.ToBoolean(dr["Isdefault"])

   }
                          ).ToList();


                return Details;
            }

  return null;

        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object FetchSegmentAddress(string CustomerId, string Entity_Segment_ID)
        {
            List<CustomerDetails> Details = new List<CustomerDetails>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                ProcedureExecute proc = new ProcedureExecute("Prc_SegmentBillingShipping");
                proc.AddVarcharPara("@Action", 50, "GetByCustomerId");
                proc.AddVarcharPara("@CustomerId", 50, CustomerId);
                proc.AddVarcharPara("@Entity_Segment_ID", 50, Entity_Segment_ID);
                DataSet addtable = proc.GetDataSet();
                DataTable dtBill = addtable.Tables[0];
               // DataTable dtSer = addtable.Tables[1];
                Details = (from DataRow dr in dtBill.Rows
                           select new CustomerDetails()
                           {

                               Id = Convert.ToInt32(dr["Id"]),
                               Type = Convert.ToString(dr["Type"]),
                               Address1 = Convert.ToString(dr["Address1"]),
                               Address2 = Convert.ToString(dr["Address2"]),
                               Address3 = Convert.ToString(dr["Address3"]),
                               CountryId = Convert.ToInt32(dr["CountryId"]),
                               CountryName = Convert.ToString(dr["CountryName"]),
                               StateId = Convert.ToInt32(dr["StateId"]),
                               StateName = Convert.ToString(dr["StateName"]),
                               StateCode = Convert.ToString(dr["StateCode"]),
                               CityId = Convert.ToInt32(dr["CityId"]),
                               CityName = Convert.ToString(dr["CityName"]),
                               AreaId = Convert.ToInt32(dr["AreaId"]),
                               AreaName = Convert.ToString(dr["AreaName"]),
                               PinId = Convert.ToInt32(dr["PinId"]),
                               PinCode = Convert.ToString(dr["PinCode"]),
                               Landmark = Convert.ToString(dr["Landmark"]),
                               Distance = Convert.ToDecimal(dr["Distance"]),
                               GSTIN = Convert.ToString(dr["GSTIN"]),
                               ContactPerson = Convert.ToString(dr["ContactPerson"]),
                               Phone = Convert.ToString(dr["Phone"]),
                               Email = Convert.ToString(dr["Email"]),
                               Isdefault = Convert.ToBoolean(dr["Isdefault"])

                           }
                          ).ToList();


                //Details = (from DataRow dr in dtSer.Rows
                //           select new CustomerDetails()
                //           {

                //               Id = Convert.ToInt32(dr["Id"]),
                //               Type = Convert.ToString(dr["Type"]),
                //               Address1 = Convert.ToString(dr["Address1"]),
                //               Address2 = Convert.ToString(dr["Address2"]),
                //               Address3 = Convert.ToString(dr["Address3"]),
                //               CountryId = Convert.ToInt32(dr["CountryId"]),
                //               CountryName = Convert.ToString(dr["CountryName"]),
                //               StateId = Convert.ToInt32(dr["StateId"]),
                //               StateName = Convert.ToString(dr["StateName"]),
                //               StateCode = Convert.ToString(dr["StateCode"]),
                //               CityId = Convert.ToInt32(dr["CityId"]),
                //               CityName = Convert.ToString(dr["CityName"]),
                //               AreaId = Convert.ToInt32(dr["AreaId"]),
                //               AreaName = Convert.ToString(dr["AreaName"]),
                //               PinId = Convert.ToInt32(dr["PinId"]),
                //               PinCode = Convert.ToString(dr["PinCode"]),
                //               Landmark = Convert.ToString(dr["Landmark"]),
                //               Distance = Convert.ToDecimal(dr["Distance"]),
                //               GSTIN = Convert.ToString(dr["GSTIN"]),
                //               ContactPerson = Convert.ToString(dr["ContactPerson"]),
                //               Phone = Convert.ToString(dr["Phone"]),
                //               Email = Convert.ToString(dr["Email"]),
                //               Isdefault = Convert.ToBoolean(dr["Isdefault"])

                //           }
                //          ).ToList();
                return Details;
            }
            return null;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object CustomAddressByPin(string PinCode)
        {
            List<CustomerDetails> addByPin = new List<CustomerDetails>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                ProcedureExecute pro = new ProcedureExecute("Prc_ServiceBillingShipping");
                pro.AddVarcharPara("@Action", 50, "CustomAddressByPin");
                pro.AddVarcharPara("@pin_code", 10, PinCode);
                DataTable addtab = pro.GetTable();

                addByPin = (from DataRow dr in addtab.Rows
                            select new CustomerDetails
                            {
                                PinCode = Convert.ToString(dr["PinCode"]),
                                PinId=Convert.ToInt32(dr["PinId"]),
                              CountryId = Convert.ToInt32(dr["CountryId"]),
                                CountryName = Convert.ToString(dr["CountryName"]),
                                StateId = Convert.ToInt32(dr["StateId"]),
                                StateName = Convert.ToString(dr["StateName"]),
                                StateCode = Convert.ToString(dr["StateCode"]),
                                CityId = Convert.ToInt32(dr["CityId"]),
                                CityName = Convert.ToString(dr["CityName"])
                            }).ToList();
                return addByPin;
            }
            return null;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]

        public object AreaByCity(string SearchKey, int CityId)
        {
            List<AreaDetails> areaList = new List<AreaDetails>();
            if (HttpContext.Current.Session["userid"] != null)
            {

                //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]); MULTI
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
                
                ProcedureExecute pro = new ProcedureExecute("Prc_ServiceBillingShipping");
                pro.AddVarcharPara("@Action", 50, "AreaByCityId");
                pro.AddIntegerPara("@CityId", CityId);
                pro.AddVarcharPara("@filter", 100, Convert.ToString(SearchKey));
                DataTable dt = pro.GetTable();




                areaList = (from DataRow dr in dt.Rows
                            select new AreaDetails
                            {
                                AreaId = Convert.ToInt32(dr["AreaId"]),
                                AreaName = Convert.ToString(dr["AreaName"])
                            }).ToList();

            }
            return areaList;
        }


        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]

        public int  GetRequiredShipToParty()
        {
            int RequiredSIPPartyResult = 0;
             CommonBL ComBL = new CommonBL();
             string RequiredSIPParty = ComBL.GetSystemSettingsResult("BillingShipping_ShipToParty");

             if (!String.IsNullOrEmpty(RequiredSIPParty))
             {
                 if (RequiredSIPParty == "Yes")
                 {

                     RequiredSIPPartyResult = 1;
                 }
                 else if (RequiredSIPParty.ToUpper().Trim() == "NO")
                 {
                     RequiredSIPPartyResult = 0;

                 }
             }

             return RequiredSIPPartyResult;
            
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]

        public int GetPlaceOfSupplyShipParty()
        {
            int PlaceOfSupplyShipParty = 0;
            CommonBL ComBL = new CommonBL();
            string RequiredSIPParty = ComBL.GetSystemSettingsResult("PlaceOfSupplyShipParty");

            if (!String.IsNullOrEmpty(RequiredSIPParty))
            {
                if (RequiredSIPParty == "Yes")
                {

                    PlaceOfSupplyShipParty = 1;
                }
                else if (RequiredSIPParty.ToUpper().Trim() == "NO")
                {
                    PlaceOfSupplyShipParty = 0;

                }
            }

            return PlaceOfSupplyShipParty;

        }

        //[WebMethod(EnableSession = true)]
        //[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        //public  int GetprojectTerms(DateTime DefLiaPer, string DefLiaPerRemarks, string Liqdamageper, DateTime LiqDamageApplicableDt, string PaymentTerms, string OrderType, string NatureofWork, string BGGroup, string BGType, string Percentage, string Value, string Status, DateTime ValidityFromDate, DateTime ValidityToDate)
        //{

        //    ProjectTermsBL ProjectTermsBL = new ProjectTermsBL();
        //    int Returnval = ProjectTermsBL.ProjectTermsCoditionsSave(DefLiaPer, DefLiaPerRemarks, Liqdamageper, LiqDamageApplicableDt, PaymentTerms, OrderType, NatureofWork, BGGroup, BGType, Percentage, Value, Status, ValidityFromDate, ValidityToDate);

        //    return Returnval;
        //}




    }
    public class CustomerDetails
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public int CountryId { get; set; }
        public string CountryName { get; set; }
        public int StateId { get; set; }
        public string StateName { get; set; }
        public string StateCode { get; set; }
        public int CityId { get; set; }
        public string CityName { get; set; }
        public int AreaId { get; set; }
        public string AreaName { get; set; }
        public int PinId { get; set; }
        public string PinCode { get; set; }

        public decimal Distance { get; set; }

        public string GSTIN { get; set; }
        public string Landmark { get; set; }

        public bool Isdefault { get; set; }
        public string ContactPerson { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }

    }

    public class AreaDetails
    {
        public int AreaId { get; set; }
        public string AreaName { get; set; }


    }
  
}
