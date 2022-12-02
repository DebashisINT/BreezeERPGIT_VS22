using DataAccessLayer;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;

namespace ERP.Services
{
    /// <summary>
    /// Summary description for TaxService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class TaxService : System.Web.Services.WebService
    {

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetTaxDetailsForSale()
        {
            List<TaxDetailsforEntry> returnList = new List<TaxDetailsforEntry>();
            TaxDetailsforEntry returnitem = new TaxDetailsforEntry();
              
                    #region GetGstTaxSchemeByJson
                  List<TaxSchemeItemLabel> taxSchemeItemLabelList = new List<TaxSchemeItemLabel>();
             
                    ProcedureExecute proc = new ProcedureExecute("prc_GstTaxDetails");
                    proc.AddVarcharPara("@action", 50, "GetTaxData");
                   // proc.AddVarcharPara("@action", 50, "GetTaxDataException");
                    proc.AddVarcharPara("@applicableFor", 5, "S");
                    proc.AddVarcharPara("@cmp_internalid", 100, Convert.ToString(HttpContext.Current.Session["LastCompany"]));
                    DataSet DS = proc.GetDataSet();

                    //returnitem.ItemLevelTaxDetails = JsonConvert.SerializeObject(DS.Tables[0]);

                    taxSchemeItemLabelList = (from DataRow dr in DS.Tables[0].Rows
                                              select new TaxSchemeItemLabel()
                                              {
                                                  TaxRates_ID = Convert.ToInt32(dr["TaxRates_ID"]),
                                                  TaxRates_TaxCode = Convert.ToInt32(dr["TaxRates_TaxCode"]),
                                                  TaxRatesSchemeName = Convert.ToString(dr["TaxRatesSchemeName"]),
                                                  Taxes_Code = Convert.ToString(dr["Taxes_Code"]),
                                                  Taxes_ApplicableOn = Convert.ToString(dr["Taxes_ApplicableOn"]),
                                                  Taxes_ApplicableFor = Convert.ToString(dr["Taxes_ApplicableFor"]),
                                                  TaxCalculateMethods = Convert.ToString(dr["TaxCalculateMethods"]),
                                                  TaxRates_Rate = Convert.ToDouble(dr["TaxRates_Rate"])
                                              }).ToList();


                    returnitem.ItemLevelTaxDetails = taxSchemeItemLabelList;

                    #endregion

                    #region GetTaxSchemebyHSN
                    List<HSNListwithTaxes> hSNListwithTaxeslist = new List<HSNListwithTaxes>();
                    HSNListwithTaxes hSNListwithTaxes;
                    foreach (DataRow hsnrow in DS.Tables[2].Rows)
                    {
                        hSNListwithTaxes = new HSNListwithTaxes();

                        hSNListwithTaxes.HSNCODE = Convert.ToString(hsnrow["HsnCode"]);

                        DataRow[] taxes = DS.Tables[1].Select("HsnCode='" + hSNListwithTaxes.HSNCODE + "'");
                        List<Config_TaxRatesID> config_TaxRatesIDlist = new List<Config_TaxRatesID>();

                        if (taxes.Length > 0)
                        {
                            Config_TaxRatesID config_TaxRatesID;
                            foreach (DataRow taxScehemCode in taxes)
                            {
                                config_TaxRatesIDlist.Add(new Config_TaxRatesID(Convert.ToInt32(taxScehemCode["TaxRates_ID"]), Convert.ToDecimal(taxScehemCode["TaxRates_Rate"]), Convert.ToString(taxScehemCode["Taxes_ApplicableOn"]), Convert.ToString(taxScehemCode["TaxTypeCode"])));
                            }
                        }

                        hSNListwithTaxes.config_TaxRatesIDs = config_TaxRatesIDlist;

                        hSNListwithTaxeslist.Add(hSNListwithTaxes);
                    }
                 //   returnitem.HSNCodewiseTaxSchem = JsonConvert.SerializeObject(hSNListwithTaxeslist);
                    returnitem.HSNCodewiseTaxSchem = hSNListwithTaxeslist;

                    #endregion


                    #region GetBranchWiseStateByJson

                    List<BranchWiseState> ListBranchWiseState = new List<BranchWiseState>();

                    if (DS.Tables[3] != null && DS.Tables[3].Rows.Count > 0)
                    {
                        foreach (DataRow dr in DS.Tables[3].Rows)
                        {
                            BranchWiseState _ObjBranchWiseState = new BranchWiseState();
                            _ObjBranchWiseState.branch_id = Convert.ToInt32(dr["branch_id"]);
                            _ObjBranchWiseState.branch_state = Convert.ToInt32(dr["branch_state"]);
                            _ObjBranchWiseState.BranchGSTIN = Convert.ToString(dr["branch_GSTIN"]);
                            _ObjBranchWiseState.CompanyGSTIN = Convert.ToString(dr["CompGSTIN"]);
                            ListBranchWiseState.Add(_ObjBranchWiseState);
                        }
                    }

                   // returnitem.BranchWiseStateTax = JsonConvert.SerializeObject(ListBranchWiseState);
                     returnitem.BranchWiseStateTax = ListBranchWiseState;


                    #endregion
                    #region GetStateCodeWiseStateIDByJson

                    List<StateCodeWiseStateID> ListStateCodeWiseStateID = new List<StateCodeWiseStateID>();

                    if (DS.Tables[4] != null && DS.Tables[4].Rows.Count > 0)
                    {
                        foreach (DataRow dr in DS.Tables[4].Rows)
                        {
                            StateCodeWiseStateID _ObjStateCodeWiseStateID = new StateCodeWiseStateID();
                            _ObjStateCodeWiseStateID.id = Convert.ToInt32(dr["id"]);
                            _ObjStateCodeWiseStateID.StateCode = Convert.ToString(dr["StateCode"]);

                            ListStateCodeWiseStateID.Add(_ObjStateCodeWiseStateID);
                        }
                    }

                    //returnitem.StateCodeWiseStateIDTax = JsonConvert.SerializeObject(ListStateCodeWiseStateID);
                    returnitem.StateCodeWiseStateIDTax = ListStateCodeWiseStateID;


                    #endregion

                    //#region Exception

                    //try
                    //{
                    //    List<HSNListwithTaxesException> ListHSNListwithTaxesException = new List<HSNListwithTaxesException>();

                    //    if (DS.Tables[5] != null && DS.Tables[5].Rows.Count > 0)
                    //    {
                    //        ListHSNListwithTaxesException = (from DataRow dr in DS.Tables[5].Rows
                    //                                         select new HSNListwithTaxesException()
                    //                                         {
                    //                                             basedon = Convert.ToString(dr["basedon"]),
                    //                                             entity = Convert.ToString(dr["entity"]),
                    //                                             StartNumber = Convert.ToString(dr["StartNumber"]),
                    //                                             EndNumber = Convert.ToString(dr["EndNumber"]),
                    //                                             code = Convert.ToString(dr["code"]),
                    //                                             type = Convert.ToString(dr["type"]),
                    //                                             I_CGST = Convert.ToString(dr["I_CGST"]),
                    //                                             I_SGST = Convert.ToString(dr["I_SGST"]),
                    //                                             I_UTGST = Convert.ToString(dr["I_UTGST"]),
                    //                                             I_IGST = Convert.ToString(dr["I_IGST"]),

                    //                                             O_CGST = Convert.ToString(dr["O_CGST"]),
                    //                                             O_SGST = Convert.ToString(dr["O_SGST"]),
                    //                                             O_UTGST = Convert.ToString(dr["O_UTGST"]),
                    //                                             O_IGST = Convert.ToString(dr["O_IGST"]),
                    //                                             fromdate = Convert.ToString(dr["fromdate"]),
                    //                                             todate = Convert.ToString(dr["todate"])


                    //                                         }).ToList();
                    //    }

                    //    returnitem.TaxesException = ListHSNListwithTaxesException;
                    //}
                    //catch (Exception ex) {  }



                    //#endregion


            returnList.Add(returnitem);

            return returnList;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetTaxDetailsForSaleException()
        {
            List<TaxDetailsforEntry> returnList = new List<TaxDetailsforEntry>();
            TaxDetailsforEntry returnitem = new TaxDetailsforEntry();

            #region GetGstTaxSchemeByJson
            List<TaxSchemeItemLabel> taxSchemeItemLabelList = new List<TaxSchemeItemLabel>();

            ProcedureExecute proc = new ProcedureExecute("prc_GstTaxDetails");
            //proc.AddVarcharPara("@action", 50, "GetTaxData");
            proc.AddVarcharPara("@action", 50, "GetTaxDataException");
            proc.AddVarcharPara("@applicableFor", 5, "S");
            proc.AddVarcharPara("@cmp_internalid", 100, Convert.ToString(HttpContext.Current.Session["LastCompany"]));
            DataSet DS = proc.GetDataSet();

            //returnitem.ItemLevelTaxDetails = JsonConvert.SerializeObject(DS.Tables[0]);

            taxSchemeItemLabelList = (from DataRow dr in DS.Tables[0].Rows
                                      select new TaxSchemeItemLabel()
                                      {
                                          TaxRates_ID = Convert.ToInt32(dr["TaxRates_ID"]),
                                          TaxRates_TaxCode = Convert.ToInt32(dr["TaxRates_TaxCode"]),
                                          TaxRatesSchemeName = Convert.ToString(dr["TaxRatesSchemeName"]),
                                          Taxes_Code = Convert.ToString(dr["Taxes_Code"]),
                                          Taxes_ApplicableOn = Convert.ToString(dr["Taxes_ApplicableOn"]),
                                          Taxes_ApplicableFor = Convert.ToString(dr["Taxes_ApplicableFor"]),
                                          TaxCalculateMethods = Convert.ToString(dr["TaxCalculateMethods"]),
                                          TaxRates_Rate = Convert.ToDouble(dr["TaxRates_Rate"])
                                      }).ToList();


            returnitem.ItemLevelTaxDetails = taxSchemeItemLabelList;

            #endregion

            #region GetTaxSchemebyHSN
            List<HSNListwithTaxes> hSNListwithTaxeslist = new List<HSNListwithTaxes>();
            HSNListwithTaxes hSNListwithTaxes;
            foreach (DataRow hsnrow in DS.Tables[2].Rows)
            {
                hSNListwithTaxes = new HSNListwithTaxes();

                hSNListwithTaxes.HSNCODE = Convert.ToString(hsnrow["HsnCode"]);

                DataRow[] taxes = DS.Tables[1].Select("HsnCode='" + hSNListwithTaxes.HSNCODE + "'");
                List<Config_TaxRatesID> config_TaxRatesIDlist = new List<Config_TaxRatesID>();

                if (taxes.Length > 0)
                {
                    Config_TaxRatesID config_TaxRatesID;
                    foreach (DataRow taxScehemCode in taxes)
                    {
                        config_TaxRatesIDlist.Add(new Config_TaxRatesID(Convert.ToInt32(taxScehemCode["TaxRates_ID"]), Convert.ToDecimal(taxScehemCode["TaxRates_Rate"]), Convert.ToString(taxScehemCode["Taxes_ApplicableOn"]), Convert.ToString(taxScehemCode["TaxTypeCode"])));
                    }
                }

                hSNListwithTaxes.config_TaxRatesIDs = config_TaxRatesIDlist;

                hSNListwithTaxeslist.Add(hSNListwithTaxes);
            }
            //   returnitem.HSNCodewiseTaxSchem = JsonConvert.SerializeObject(hSNListwithTaxeslist);
            returnitem.HSNCodewiseTaxSchem = hSNListwithTaxeslist;

            #endregion


            #region GetBranchWiseStateByJson

            List<BranchWiseState> ListBranchWiseState = new List<BranchWiseState>();

            if (DS.Tables[3] != null && DS.Tables[3].Rows.Count > 0)
            {
                foreach (DataRow dr in DS.Tables[3].Rows)
                {
                    BranchWiseState _ObjBranchWiseState = new BranchWiseState();
                    _ObjBranchWiseState.branch_id = Convert.ToInt32(dr["branch_id"]);
                    _ObjBranchWiseState.branch_state = Convert.ToInt32(dr["branch_state"]);
                    _ObjBranchWiseState.BranchGSTIN = Convert.ToString(dr["branch_GSTIN"]);
                    _ObjBranchWiseState.CompanyGSTIN = Convert.ToString(dr["CompGSTIN"]);
                    ListBranchWiseState.Add(_ObjBranchWiseState);
                }
            }

            // returnitem.BranchWiseStateTax = JsonConvert.SerializeObject(ListBranchWiseState);
            returnitem.BranchWiseStateTax = ListBranchWiseState;


            #endregion
            #region GetStateCodeWiseStateIDByJson

            List<StateCodeWiseStateID> ListStateCodeWiseStateID = new List<StateCodeWiseStateID>();

            if (DS.Tables[4] != null && DS.Tables[4].Rows.Count > 0)
            {
                foreach (DataRow dr in DS.Tables[4].Rows)
                {
                    StateCodeWiseStateID _ObjStateCodeWiseStateID = new StateCodeWiseStateID();
                    _ObjStateCodeWiseStateID.id = Convert.ToInt32(dr["id"]);
                    _ObjStateCodeWiseStateID.StateCode = Convert.ToString(dr["StateCode"]);

                    ListStateCodeWiseStateID.Add(_ObjStateCodeWiseStateID);
                }
            }

            //returnitem.StateCodeWiseStateIDTax = JsonConvert.SerializeObject(ListStateCodeWiseStateID);
            returnitem.StateCodeWiseStateIDTax = ListStateCodeWiseStateID;


            #endregion

            #region Exception

            try
            {
                List<HSNListwithTaxesException> ListHSNListwithTaxesException = new List<HSNListwithTaxesException>();

                if (DS.Tables[5] != null && DS.Tables[5].Rows.Count > 0)
                {
                    ListHSNListwithTaxesException = (from DataRow dr in DS.Tables[5].Rows
                                                     select new HSNListwithTaxesException()
                                                     {
                                                         basedon = Convert.ToString(dr["basedon"]),
                                                         entity = Convert.ToString(dr["entity"]),
                                                         StartNumber = Convert.ToString(dr["StartNumber"]),
                                                         EndNumber = Convert.ToString(dr["EndNumber"]),
                                                         code = Convert.ToString(dr["code"]),
                                                         type = Convert.ToString(dr["type"]),
                                                         I_CGST = Convert.ToString(dr["I_CGST"]),
                                                         I_SGST = Convert.ToString(dr["I_SGST"]),
                                                         I_UTGST = Convert.ToString(dr["I_UTGST"]),
                                                         I_IGST = Convert.ToString(dr["I_IGST"]),

                                                         O_CGST = Convert.ToString(dr["O_CGST"]),
                                                         O_SGST = Convert.ToString(dr["O_SGST"]),
                                                         O_UTGST = Convert.ToString(dr["O_UTGST"]),
                                                         O_IGST = Convert.ToString(dr["O_IGST"]),
                                                         fromdate = Convert.ToString(dr["fromdate"]),
                                                         todate = Convert.ToString(dr["todate"])


                                                     }).ToList();
                }

                returnitem.TaxesException = ListHSNListwithTaxesException;
            }
            catch (Exception ex) { }



            #endregion


            returnList.Add(returnitem);

            return returnList;
        }



        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetTaxDetailsForPurchaseException()
        {
            List<TaxDetailsforEntry> returnList = new List<TaxDetailsforEntry>();
            TaxDetailsforEntry returnitem = new TaxDetailsforEntry();

            #region GetGstTaxSchemeByJson
            List<TaxSchemeItemLabel> taxSchemeItemLabelList = new List<TaxSchemeItemLabel>();

            ProcedureExecute proc = new ProcedureExecute("prc_GstTaxDetails");
            //proc.AddVarcharPara("@action", 50, "GetTaxData");
            proc.AddVarcharPara("@action", 50, "GetTaxDataException");
            proc.AddVarcharPara("@applicableFor", 5, "P");
            proc.AddVarcharPara("@cmp_internalid", 100, Convert.ToString(HttpContext.Current.Session["LastCompany"]));
            DataSet DS = proc.GetDataSet();

            //returnitem.ItemLevelTaxDetails = JsonConvert.SerializeObject(DS.Tables[0]);

            taxSchemeItemLabelList = (from DataRow dr in DS.Tables[0].Rows
                                      select new TaxSchemeItemLabel()
                                      {
                                          TaxRates_ID = Convert.ToInt32(dr["TaxRates_ID"]),
                                          TaxRates_TaxCode = Convert.ToInt32(dr["TaxRates_TaxCode"]),
                                          TaxRatesSchemeName = Convert.ToString(dr["TaxRatesSchemeName"]),
                                          Taxes_Code = Convert.ToString(dr["Taxes_Code"]),
                                          Taxes_ApplicableOn = Convert.ToString(dr["Taxes_ApplicableOn"]),
                                          Taxes_ApplicableFor = Convert.ToString(dr["Taxes_ApplicableFor"]),
                                          TaxCalculateMethods = Convert.ToString(dr["TaxCalculateMethods"]),
                                          TaxRates_Rate = Convert.ToDouble(dr["TaxRates_Rate"])
                                      }).ToList();


            returnitem.ItemLevelTaxDetails = taxSchemeItemLabelList;

            #endregion

            #region GetTaxSchemebyHSN
            List<HSNListwithTaxes> hSNListwithTaxeslist = new List<HSNListwithTaxes>();
            HSNListwithTaxes hSNListwithTaxes;
            foreach (DataRow hsnrow in DS.Tables[2].Rows)
            {
                hSNListwithTaxes = new HSNListwithTaxes();

                hSNListwithTaxes.HSNCODE = Convert.ToString(hsnrow["HsnCode"]);

                DataRow[] taxes = DS.Tables[1].Select("HsnCode='" + hSNListwithTaxes.HSNCODE + "'");
                List<Config_TaxRatesID> config_TaxRatesIDlist = new List<Config_TaxRatesID>();

                if (taxes.Length > 0)
                {
                    Config_TaxRatesID config_TaxRatesID;
                    foreach (DataRow taxScehemCode in taxes)
                    {
                        config_TaxRatesIDlist.Add(new Config_TaxRatesID(Convert.ToInt32(taxScehemCode["TaxRates_ID"]), Convert.ToDecimal(taxScehemCode["TaxRates_Rate"]), Convert.ToString(taxScehemCode["Taxes_ApplicableOn"]), Convert.ToString(taxScehemCode["TaxTypeCode"])));
                    }
                }

                hSNListwithTaxes.config_TaxRatesIDs = config_TaxRatesIDlist;

                hSNListwithTaxeslist.Add(hSNListwithTaxes);
            }
            //   returnitem.HSNCodewiseTaxSchem = JsonConvert.SerializeObject(hSNListwithTaxeslist);
            returnitem.HSNCodewiseTaxSchem = hSNListwithTaxeslist;

            #endregion


            #region GetBranchWiseStateByJson

            List<BranchWiseState> ListBranchWiseState = new List<BranchWiseState>();

            if (DS.Tables[3] != null && DS.Tables[3].Rows.Count > 0)
            {
                foreach (DataRow dr in DS.Tables[3].Rows)
                {
                    BranchWiseState _ObjBranchWiseState = new BranchWiseState();
                    _ObjBranchWiseState.branch_id = Convert.ToInt32(dr["branch_id"]);
                    _ObjBranchWiseState.branch_state = Convert.ToInt32(dr["branch_state"]);
                    _ObjBranchWiseState.BranchGSTIN = Convert.ToString(dr["branch_GSTIN"]);
                    _ObjBranchWiseState.CompanyGSTIN = Convert.ToString(dr["CompGSTIN"]);
                    ListBranchWiseState.Add(_ObjBranchWiseState);
                }
            }

            // returnitem.BranchWiseStateTax = JsonConvert.SerializeObject(ListBranchWiseState);
            returnitem.BranchWiseStateTax = ListBranchWiseState;


            #endregion
            #region GetStateCodeWiseStateIDByJson

            List<StateCodeWiseStateID> ListStateCodeWiseStateID = new List<StateCodeWiseStateID>();

            if (DS.Tables[4] != null && DS.Tables[4].Rows.Count > 0)
            {
                foreach (DataRow dr in DS.Tables[4].Rows)
                {
                    StateCodeWiseStateID _ObjStateCodeWiseStateID = new StateCodeWiseStateID();
                    _ObjStateCodeWiseStateID.id = Convert.ToInt32(dr["id"]);
                    _ObjStateCodeWiseStateID.StateCode = Convert.ToString(dr["StateCode"]);

                    ListStateCodeWiseStateID.Add(_ObjStateCodeWiseStateID);
                }
            }

            //returnitem.StateCodeWiseStateIDTax = JsonConvert.SerializeObject(ListStateCodeWiseStateID);
            returnitem.StateCodeWiseStateIDTax = ListStateCodeWiseStateID;


            #endregion

            #region Exception

            try
            {
                List<HSNListwithTaxesException> ListHSNListwithTaxesException = new List<HSNListwithTaxesException>();

                if (DS.Tables[5] != null && DS.Tables[5].Rows.Count > 0)
                {
                    ListHSNListwithTaxesException = (from DataRow dr in DS.Tables[5].Rows
                                                     select new HSNListwithTaxesException()
                                                     {
                                                         basedon = Convert.ToString(dr["basedon"]),
                                                         entity = Convert.ToString(dr["entity"]),
                                                         StartNumber = Convert.ToString(dr["StartNumber"]),
                                                         EndNumber = Convert.ToString(dr["EndNumber"]),
                                                         code = Convert.ToString(dr["code"]),
                                                         type = Convert.ToString(dr["type"]),
                                                         I_CGST = Convert.ToString(dr["I_CGST"]),
                                                         I_SGST = Convert.ToString(dr["I_SGST"]),
                                                         I_UTGST = Convert.ToString(dr["I_UTGST"]),
                                                         I_IGST = Convert.ToString(dr["I_IGST"]),

                                                         O_CGST = Convert.ToString(dr["O_CGST"]),
                                                         O_SGST = Convert.ToString(dr["O_SGST"]),
                                                         O_UTGST = Convert.ToString(dr["O_UTGST"]),
                                                         O_IGST = Convert.ToString(dr["O_IGST"]),
                                                         fromdate = Convert.ToString(dr["fromdate"]),
                                                         todate = Convert.ToString(dr["todate"])


                                                     }).ToList();
                }

                returnitem.TaxesException = ListHSNListwithTaxesException;
            }
            catch (Exception ex) { }



            #endregion


            returnList.Add(returnitem);

            return returnList;
        }
        public class HSNListwithTaxesException
        {
            public string code { get; set; }
            public string type { get; set; }
            public string fromdate { get; set; }
            public string todate { get; set; }
            public string entity { get; set; }
            public string basedon { get; set; }
            public string StartNumber { get; set; }
            public string EndNumber { get; set; }
            public string I_CGST { get; set; }
            public string I_SGST { get; set; }
            public string I_UTGST { get; set; }
            public string I_IGST { get; set; }
            public string O_CGST { get; set; }
            public string O_SGST { get; set; }
            public string O_UTGST { get; set; }
            public string O_IGST { get; set; }

        }
        public class TaxSchemeItemLabel
        {
            public int TaxRates_ID { get; set; }
            public int TaxRates_TaxCode { get; set; }
            public string TaxRatesSchemeName  { get; set; }
            public string Taxes_Code  { get; set; }
            public string Taxes_ApplicableOn { get; set; }
            public string Taxes_ApplicableFor { get; set; }
            public string TaxCalculateMethods { get; set; }
            public double TaxRates_Rate { get; set; }
            
        }

        public class TaxDetailsforEntry 
        {
            public List<TaxSchemeItemLabel> ItemLevelTaxDetails { get; set; }
            public List<HSNListwithTaxes> HSNCodewiseTaxSchem { get; set; }
            public List<BranchWiseState> BranchWiseStateTax { get; set; }
            public List<StateCodeWiseStateID> StateCodeWiseStateIDTax { get; set; }

            public List<HSNListwithTaxesException> TaxesException { get; set; }
        
        }

        public class HSNListwithTaxes
        {
            public string HSNCODE { get; set; }
            public List<Config_TaxRatesID> config_TaxRatesIDs { get; set; }

        }

        public class Config_TaxRatesID
        {
            public int TaxRates_ID { get; set; }
            public decimal Rate { get; set; }
            public string Taxes_ApplicableOn { get; set; }
            public string TaxTypeCode { get; set; }
            public Config_TaxRatesID(int id, decimal rate, string Taxes_ApplicableOn, string TaxTypeCode)
            {
                this.TaxRates_ID = id;
                this.Rate = rate;
                this.Taxes_ApplicableOn = Taxes_ApplicableOn;
                this.TaxTypeCode = TaxTypeCode;
            }

        }
        public class BranchWiseState
        {
            public int branch_id { get; set; }
            public int branch_state { get; set; }
            public string BranchGSTIN { get; set; }
            public string CompanyGSTIN { get; set; }
        }
        public class StateCodeWiseStateID
        {
            public int id { get; set; }
            public string StateCode { get; set; }
        }
    }
}
