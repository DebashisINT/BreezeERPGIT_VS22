//================================================== Revision History =============================================
//1.0    V2.0.39    Priti   19-09-2023  0026824:Tax amount mismatch happened in the Rate Difference Customer Entry module
//====================================================== Revision History =============================================
using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Newtonsoft.Json;
using System.Configuration;

namespace ERP.OMS.Tax_Details.ClassFile
{
    public class GSTtaxDetails
    {

        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        /// <summary>
        /// GetGstTaxSchemeByJson is a method, which will return all the the tax scheme with details in json format.
        /// </summary>
        /// <param name="ApplicableFor"> 
        /// <example>
        /// P for Purchase, S for Sale
        /// </example></param>
        /// <returns>json string with all tax details</returns>
        public string GetGstTaxSchemeByJson(string ApplicableFor)
        {
            ProcedureExecute proc = new ProcedureExecute("prc_GstTaxDetails");
            proc.AddVarcharPara("@action", 50, "GetProductLevelTaxDetails");
            proc.AddVarcharPara("@applicableFor", 5, ApplicableFor);
            DataTable taxDt = proc.GetTable();



            return JsonConvert.SerializeObject(taxDt);
        }
        /// <summary>
        /// Get All tax scheme which is mapped with HSN code 
        /// </summary>
        /// <param name="ApplicableFor">
        /// <example>
        /// P for Purchase, S for Sale
        /// </example>
        /// </param>
        /// <returns>
        /// json string with HSN codewise
        /// </returns>
        public string GetTaxSchemebyHSN(string ApplicableFor)
        {
            List<HSNListwithTaxes> hSNListwithTaxeslist = new List<HSNListwithTaxes>();
            ProcedureExecute proc = new ProcedureExecute("prc_GstTaxDetails");
            proc.AddVarcharPara("@action", 50, "GetHSNwisetaxCode");
            proc.AddVarcharPara("@applicableFor", 5, ApplicableFor);
            DataSet taxDataset = proc.GetDataSet();


            HSNListwithTaxes hSNListwithTaxes;
            foreach (DataRow hsnrow in taxDataset.Tables[1].Rows)
            {
                hSNListwithTaxes = new HSNListwithTaxes();

                hSNListwithTaxes.HSNCODE = Convert.ToString(hsnrow["HsnCode"]);

                DataRow[] taxes = taxDataset.Tables[0].Select("HsnCode=" + hSNListwithTaxes.HSNCODE);
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
            return JsonConvert.SerializeObject(hSNListwithTaxeslist);
        }
        /// <summary>
        /// GetBranchWiseStateByJson is a method, which will return all the State and Branch in json format.
        /// </summary>
        /// <returns>json string with all state and branch details</returns>
        public string GetBranchWiseStateByJson()
        {
            try
            {
                ProcedureExecute proc = new ProcedureExecute("prc_GstTaxDetails");
                proc.AddVarcharPara("@action", 50, "GetBranchWiseState");

                DataTable DT_BranchWiseState = proc.GetTable();
                List<BranchWiseState> ListBranchWiseState = new List<BranchWiseState>();

                if (DT_BranchWiseState != null && DT_BranchWiseState.Rows.Count > 0)
                {
                    foreach (DataRow dr in DT_BranchWiseState.Rows)
                    {
                        BranchWiseState _ObjBranchWiseState = new BranchWiseState();
                        _ObjBranchWiseState.branch_id = Convert.ToInt32(dr["branch_id"]);
                        _ObjBranchWiseState.branch_state = Convert.ToInt32(dr["branch_state"]);

                        ListBranchWiseState.Add(_ObjBranchWiseState);
                    }
                }

                return JsonConvert.SerializeObject(ListBranchWiseState);
            }
            catch (Exception ex) { return ""; }
        }
        public void GetTaxData(ref string ItemLevelTaxDetails, ref string HSNCodewisetaxSchemid, ref string BranchWiseStateTax, ref string StateCodeWiseStateID, string ApplicableFor)
        {
            #region GetGstTaxSchemeByJson
            ProcedureExecute proc = new ProcedureExecute("prc_GstTaxDetails");
            proc.AddVarcharPara("@action", 50, "GetTaxData");
            proc.AddVarcharPara("@applicableFor", 5, ApplicableFor);
            proc.AddVarcharPara("@cmp_internalid", 100, Convert.ToString(HttpContext.Current.Session["LastCompany"]));
            DataSet DS = proc.GetDataSet();
            ItemLevelTaxDetails = JsonConvert.SerializeObject(DS.Tables[0]);
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
            HSNCodewisetaxSchemid = JsonConvert.SerializeObject(hSNListwithTaxeslist);
            #endregion


            #region GetBranchWiseStateByJson
            try
            {
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

                BranchWiseStateTax = JsonConvert.SerializeObject(ListBranchWiseState);
            }
            catch (Exception ex) { BranchWiseStateTax = ""; }
            #endregion
            #region GetStateCodeWiseStateIDByJson
            try
            {
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

                StateCodeWiseStateID = JsonConvert.SerializeObject(ListStateCodeWiseStateID);
            }
            catch (Exception ex) { BranchWiseStateTax = ""; }
            #endregion
        }

        public void GetTaxDataWithException(ref string ItemLevelTaxDetails, ref string HSNCodewisetaxSchemid, ref string BranchWiseStateTax, ref string StateCodeWiseStateID,ref string taxException, string ApplicableFor)
        {
            #region GetGstTaxSchemeByJson
            ProcedureExecute proc = new ProcedureExecute("prc_GstTaxDetails");
            proc.AddVarcharPara("@action", 50, "GetTaxDataException");
            proc.AddVarcharPara("@applicableFor", 5, ApplicableFor);
            proc.AddVarcharPara("@cmp_internalid", 100, Convert.ToString(HttpContext.Current.Session["LastCompany"]));
            DataSet DS = proc.GetDataSet();
            ItemLevelTaxDetails = JsonConvert.SerializeObject(DS.Tables[0]);
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
            HSNCodewisetaxSchemid = JsonConvert.SerializeObject(hSNListwithTaxeslist);
            #endregion


            #region GetBranchWiseStateByJson
            try
            {
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

                BranchWiseStateTax = JsonConvert.SerializeObject(ListBranchWiseState);
            }
            catch (Exception ex) { BranchWiseStateTax = ""; }
            #endregion
            #region GetStateCodeWiseStateIDByJson
            try
            {
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

                StateCodeWiseStateID = JsonConvert.SerializeObject(ListStateCodeWiseStateID);
            }
            catch (Exception ex) { BranchWiseStateTax = ""; }




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

                taxException = JsonConvert.SerializeObject(ListHSNListwithTaxesException);
            }
            catch (Exception ex) { taxException = ""; }



            #endregion



        }


        #region Depends Classes
        public class HSNListwithTaxes
        {
            public string HSNCODE { get; set; }
            public List<Config_TaxRatesID> config_TaxRatesIDs { get; set; }

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
        #endregion

        public DataTable SetTaxTableDataWithProductSerial(DataTable productDetails, string SerialColumnName, string ProductColumnName, string AmountColumnName, string chargesColumns, DataTable taxTable, string ApplicableFor, string TransactionDate, string BranchId, string ShippingState, string inclusiveOrExclusive)
        {
            string fromState = "", shippingStateCode = "", TaxType = "", roundOfPlus = "", roundofMinus = "";
            DataTable fetchedData = oDBEngine.GetDataTable("select StateCode  from tbl_master_branch br inner join tbl_master_state st on br.branch_state=st.id where branch_id=" + BranchId);
            if (fetchedData != null)
                fromState = Convert.ToString(fetchedData.Rows[0][0]).Trim();

            fetchedData = oDBEngine.GetDataTable("select StateCode from tbl_master_state where id=" + ShippingState);
            if (fetchedData != null && fetchedData.Rows.Count > 0)
                shippingStateCode = Convert.ToString(fetchedData.Rows[0][0]).Trim();

            fetchedData = oDBEngine.GetDataTable("select TaxRates_ID from Config_TaxRates config inner join Master_Taxes masterTax on config.TaxRates_TaxCode = masterTax.Taxes_ID  where  TaxRates_RoundingOff='R+' and Taxes_ApplicableFor in ('B','" + ApplicableFor + "')");
            if (fetchedData.Rows.Count > 0)
                roundOfPlus = Convert.ToString(fetchedData.Rows[0][0]).Trim();

            fetchedData = oDBEngine.GetDataTable("select TaxRates_ID from Config_TaxRates config inner join Master_Taxes masterTax on config.TaxRates_TaxCode = masterTax.Taxes_ID  where  TaxRates_RoundingOff='R-' and Taxes_ApplicableFor in ('B','" + ApplicableFor + "')");
            if (fetchedData.Rows.Count > 0)
                roundofMinus = Convert.ToString(fetchedData.Rows[0][0]).Trim();




            #region setTaxType
            if (fromState == shippingStateCode)
            {
                TaxType = "SGST";
                if (shippingStateCode == "4" || shippingStateCode == "26" || shippingStateCode == "25" || shippingStateCode == "35" || shippingStateCode == "31" || shippingStateCode == "34")
                {
                    TaxType = "UTGST";
                }
            }
            else
            {
                TaxType = "IGST";
            }

            #endregion






            foreach (DataRow productRow in productDetails.Rows)
            {
                if (taxTable.Select("SlNo='" + productRow[SerialColumnName]+"'").Length == 0)
                {

                    DataTable taxDetail = new DataTable();
                    ProcedureExecute proc = new ProcedureExecute("prc_GstTaxDetails");
                    proc.AddVarcharPara("@Action", 500, "LoadOtherTaxDetails");
                    proc.AddVarcharPara("@ProductID", 10, Convert.ToString(productRow[ProductColumnName]));
                    proc.AddVarcharPara("@applicableFor", 5, ApplicableFor);
                    proc.AddVarcharPara("@TransDate", 10, TransactionDate);
                    taxDetail = proc.GetTable();


                    #region DeleteExtra GstCode

                    if (TaxType == "UTGST")
                    {
                        foreach (DataRow dr in taxDetail.Rows)
                        {
                            if (Convert.ToString(dr["TaxTypeCode"]).Trim() == "IGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "SGST")
                            {
                                dr.Delete();
                            }
                        }
                    }
                    else if (TaxType == "IGST")
                    {
                        foreach (DataRow dr in taxDetail.Rows)
                        {
                            if (Convert.ToString(dr["TaxTypeCode"]).Trim() == "CGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "SGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "UTGST")
                            {
                                dr.Delete();
                            }
                        }
                    }
                    else if (TaxType == "SGST")
                    {
                        foreach (DataRow dr in taxDetail.Rows)
                        {
                            if (Convert.ToString(dr["TaxTypeCode"]).Trim() == "IGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "UTGST")
                            {
                                dr.Delete();
                            }
                        }
                    }
                    taxDetail.AcceptChanges();
                    #endregion

                    //#region CreateTempTaxTable
                    //DataTable TaxRecord = new DataTable();

                    //TaxRecord.Columns.Add("SlNo", typeof(System.Int32));
                    //TaxRecord.Columns.Add("TaxCode", typeof(System.String));
                    //TaxRecord.Columns.Add("AltTaxCode", typeof(System.String));
                    //TaxRecord.Columns.Add("Percentage", typeof(System.Decimal));
                    //TaxRecord.Columns.Add("Amount", typeof(System.Decimal)); 

                    //#endregion






                    //decimal totalParcentage = 0;
                    //foreach (DataRow dr in taxDetail.Rows)
                    //{
                    //    if (Convert.ToString(dr["TaxTypeCode"]).Trim() == "CGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "SGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "IGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "UTGST")
                    //    {
                    //        totalParcentage += Convert.ToDecimal(dr["TaxRates_Rate"]);
                    //    }
                    //}


                    if (Convert.ToDecimal(productRow[chargesColumns]) != 0)
                    {
                        foreach (DataRow taxexistingRow in taxDetail.Rows)
                        {
                            if (Convert.ToString(taxexistingRow["TaxTypeCode"]).Trim() != "O")
                            {
                                DataRow txRecordRow = taxTable.NewRow();
                                txRecordRow["SlNo"] = productRow[SerialColumnName];
                                txRecordRow["TaxCode"] = taxexistingRow["Taxes_ID"];
                                txRecordRow["AltTaxCode"] = "0";
                                txRecordRow["Percentage"] = taxexistingRow["TaxRates_Rate"];
                                txRecordRow["Amount"] = Math.Round(Convert.ToDecimal(productRow[AmountColumnName]) * (Convert.ToDecimal(taxexistingRow["TaxRates_Rate"]) / 100), 2);
                                taxTable.Rows.Add(txRecordRow);

                            }
                        }
                    }
                }


            }



            #region setRound of for GST
            //DataTable GSTTABLE = oDBEngine.GetDataTable("select TaxRates_ID,TaxTypeCode from Config_TaxRates config inner join Master_Taxes mast on config.TaxRates_TaxCode =mast.Taxes_ID where TaxTypeCode in ('IGST','CGST','SGST','UTGST')");

            //foreach (DataRow gstrow in taxTable.Rows)
            //{
            //    if (GSTTABLE.Select("TaxRates_ID=" + gstrow["TaxCode"]).Length > 0)
            //    {
            //        gstrow["Amount"] = Math.Round(Convert.ToDecimal(gstrow["Amount"]));
            //    }
            //}

            #endregion





            foreach (DataRow productRow in productDetails.Rows)
            {
                #region SetRounded of ledger


                if (taxTable.Select("SlNo='" + productRow[SerialColumnName]+"'").Length > 0)
                {
                    DataTable filteredTable = taxTable.Select("SlNo='" + productRow[SerialColumnName]+"'").CopyToDataTable();
                    decimal totalTaxAmount = Convert.ToDecimal(filteredTable.Compute("SUM(Amount)", string.Empty));

                    if (totalTaxAmount != Convert.ToDecimal(productRow[chargesColumns]))
                    {
                        decimal roundOf = Convert.ToDecimal(productRow[chargesColumns]) - totalTaxAmount;
                        if (roundOf > 0)
                        {
                            if (roundOfPlus.Trim() != "")
                            {
                                DataRow txRecordRow = taxTable.NewRow();
                                txRecordRow["SlNo"] = productRow[SerialColumnName];
                                txRecordRow["TaxCode"] = roundOfPlus;
                                txRecordRow["AltTaxCode"] = "0";
                                txRecordRow["Percentage"] = "0";
                                txRecordRow["Amount"] = roundOf;
                                taxTable.Rows.Add(txRecordRow);
                            }
                        }
                        else if (roundOf < 0)
                        {
                            if (roundofMinus.Trim() != "")
                            {
                                DataRow txRecordRow = taxTable.NewRow();
                                txRecordRow["SlNo"] = productRow[SerialColumnName];
                                txRecordRow["TaxCode"] = roundofMinus;
                                txRecordRow["AltTaxCode"] = "0";
                                txRecordRow["Percentage"] = "0";
                                txRecordRow["Amount"] = roundOf;
                                taxTable.Rows.Add(txRecordRow);
                            }
                        }
                    }
                }



                #endregion
            }

            return taxTable;
        }

        public DataTable SetTaxTableDataWithProductSerialWithException(DataTable productDetails, string SerialColumnName, string ProductColumnName, string AmountColumnName, string chargesColumns, DataTable taxTable, string ApplicableFor, string TransactionDate, string BranchId, string ShippingState, string inclusiveOrExclusive, object customerID, String quantitycolumn, string modulename)
        {
            string fromState = "", shippingStateCode = "", TaxType = "", roundOfPlus = "", roundofMinus = "";
            DataTable fetchedData = oDBEngine.GetDataTable("select StateCode  from tbl_master_branch br inner join tbl_master_state st on br.branch_state=st.id where branch_id=" + BranchId);
            if (fetchedData != null)
                fromState = Convert.ToString(fetchedData.Rows[0][0]).Trim();

            fetchedData = oDBEngine.GetDataTable("select StateCode from tbl_master_state where id=" + ShippingState);
            if (fetchedData != null && fetchedData.Rows.Count > 0)
                shippingStateCode = Convert.ToString(fetchedData.Rows[0][0]).Trim();

            fetchedData = oDBEngine.GetDataTable("select TaxRates_ID from Config_TaxRates config inner join Master_Taxes masterTax on config.TaxRates_TaxCode = masterTax.Taxes_ID  where  TaxRates_RoundingOff='R+' and Taxes_ApplicableFor in ('B','" + ApplicableFor + "')");
            if (fetchedData.Rows.Count > 0)
                roundOfPlus = Convert.ToString(fetchedData.Rows[0][0]).Trim();

            fetchedData = oDBEngine.GetDataTable("select TaxRates_ID from Config_TaxRates config inner join Master_Taxes masterTax on config.TaxRates_TaxCode = masterTax.Taxes_ID  where  TaxRates_RoundingOff='R-' and Taxes_ApplicableFor in ('B','" + ApplicableFor + "')");
            if (fetchedData.Rows.Count > 0)
                roundofMinus = Convert.ToString(fetchedData.Rows[0][0]).Trim();




            #region setTaxType
            if (fromState == shippingStateCode)
            {
                TaxType = "SGST";
                if (shippingStateCode == "4" || shippingStateCode == "26" || shippingStateCode == "25" || shippingStateCode == "35" || shippingStateCode == "31" || shippingStateCode == "34")
                {
                    TaxType = "UTGST";
                }
            }
            else
            {
                TaxType = "IGST";
            }

            #endregion






            foreach (DataRow productRow in productDetails.Rows)
            {
                if (taxTable.Select("SlNo='" + productRow[SerialColumnName] + "'").Length == 0)
                {

                    DataTable taxDetail = new DataTable();
                    //ProcedureExecute proc = new ProcedureExecute("prc_GstTaxDetails");
                    //proc.AddVarcharPara("@Action", 500, "LoadOtherTaxDetails");
                    //proc.AddVarcharPara("@ProductID", 10, Convert.ToString(productRow[ProductColumnName]));
                    //proc.AddVarcharPara("@applicableFor", 5, ApplicableFor);
                    //proc.AddVarcharPara("@TransDate", 10, TransactionDate);
                    //taxDetail = proc.GetTable();

                    string HSNExempted = "", HSNNonGst = "", HSNNilRated = "", SERVICEExempted = "", SERVICENonGst = "", SERVICENilRated="";

                    DataTable Exempted_Data = oDBEngine.GetDataTable("select Isnull(tbl_HSN_Master.Exempted,'0')HSNExempted,Isnull(tbl_HSN_Master.NonGst,'0')HSNNonGst,Isnull(tbl_HSN_Master.NilRated,'0')HSNNilRated,Isnull(TBL_MASTER_SERVICE_TAX.Exempted,'0')SERVICEExempted,Isnull(TBL_MASTER_SERVICE_TAX.NonGst,'0')SERVICENonGst,Isnull(TBL_MASTER_SERVICE_TAX.NilRated,'0')SERVICENilRated	 from  Master_sProducts PM left outer join tbl_HSN_Master on Code=PM.sProducts_HsnCode left outer join TBL_MASTER_SERVICE_TAX on TAX_ID=PM.sProducts_serviceTax	where sProducts_ID='" + Convert.ToString(productRow[ProductColumnName]) + "'");
                    if (Exempted_Data.Rows.Count > 0)
                    {
                        HSNExempted = Convert.ToString(Exempted_Data.Rows[0]["HSNExempted"]).Trim();
                        HSNNonGst = Convert.ToString(Exempted_Data.Rows[0]["HSNNonGst"]).Trim();

                        HSNNilRated = Convert.ToString(Exempted_Data.Rows[0]["HSNNilRated"]).Trim();
                        SERVICEExempted = Convert.ToString(Exempted_Data.Rows[0]["SERVICEExempted"]).Trim();

                        SERVICENonGst = Convert.ToString(Exempted_Data.Rows[0]["SERVICENonGst"]).Trim();
                        SERVICENilRated = Convert.ToString(Exempted_Data.Rows[0]["SERVICENilRated"]).Trim();
                    }
                       

                    ProcedureExecute proc = new ProcedureExecute("prc_TaxExceptionFind");
                    proc.AddVarcharPara("@Action", 500, modulename);
                    proc.AddVarcharPara("@ProductID", 10, Convert.ToString(productRow[ProductColumnName]));
                    proc.AddPara("@ENTITY_ID", customerID);
                    proc.AddVarcharPara("@Date", 10, TransactionDate);
                    proc.AddVarcharPara("@Amount", 100, Convert.ToString(productRow[AmountColumnName]));
                    proc.AddVarcharPara("@Qty", 100, Convert.ToString(productRow[quantitycolumn]));
                    taxDetail = proc.GetTable();


                    #region DeleteExtra GstCode

                    if (TaxType == "UTGST")
                    {
                        foreach (DataRow dr in taxDetail.Rows)
                        {
                            if (Convert.ToString(dr["TaxTypeCode"]).Trim() == "IGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "SGST")
                            {
                                dr.Delete();
                            }
                        }
                    }
                    else if (TaxType == "IGST")
                    {
                        foreach (DataRow dr in taxDetail.Rows)
                        {
                            if (Convert.ToString(dr["TaxTypeCode"]).Trim() == "CGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "SGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "UTGST")
                            {
                                dr.Delete();
                            }
                        }
                    }
                    else if (TaxType == "SGST")
                    {
                        foreach (DataRow dr in taxDetail.Rows)
                        {
                            if (Convert.ToString(dr["TaxTypeCode"]).Trim() == "IGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "UTGST")
                            {
                                dr.Delete();
                            }
                        }
                    }
                    taxDetail.AcceptChanges();
                    #endregion

                    

                    if (Convert.ToDecimal(productRow[chargesColumns]) != 0)
                    {
                        foreach (DataRow taxexistingRow in taxDetail.Rows)
                        {
                            if (Convert.ToString(taxexistingRow["TaxTypeCode"]).Trim() != "O")
                            {
                                DataRow txRecordRow = taxTable.NewRow();
                                txRecordRow["SlNo"] = productRow[SerialColumnName];
                                txRecordRow["TaxCode"] = taxexistingRow["Taxes_ID"];
                                txRecordRow["AltTaxCode"] = "0";
                                txRecordRow["Percentage"] = taxexistingRow["TaxRates_Rate"];
                                txRecordRow["Amount"] = Math.Round(Convert.ToDecimal(productRow[AmountColumnName]) * (Convert.ToDecimal(taxexistingRow["TaxRates_Rate"]) / 100), 2, MidpointRounding.AwayFromZero);
                                taxTable.Rows.Add(txRecordRow);

                            }
                        }
                    }
                    else if (HSNExempted == "True" || SERVICEExempted == "True")
                    {
                        foreach (DataRow taxexistingRow in taxDetail.Rows)
                        {
                            if (Convert.ToString(taxexistingRow["TaxTypeCode"]).Trim() != "O")
                            {
                                DataRow txRecordRow = taxTable.NewRow();
                                txRecordRow["SlNo"] = productRow[SerialColumnName];
                                txRecordRow["TaxCode"] = taxexistingRow["Taxes_ID"];
                                txRecordRow["AltTaxCode"] = "0";
                                txRecordRow["Percentage"] = taxexistingRow["TaxRates_Rate"];
                                txRecordRow["Amount"] = Math.Round(Convert.ToDecimal(productRow[AmountColumnName]) * (Convert.ToDecimal(taxexistingRow["TaxRates_Rate"]) / 100), 2, MidpointRounding.AwayFromZero);
                                taxTable.Rows.Add(txRecordRow);

                            }
                        }
                    }
                    else if (HSNNonGst == "True" || SERVICENonGst == "True")
                    {
                        foreach (DataRow taxexistingRow in taxDetail.Rows)
                        {
                            if (Convert.ToString(taxexistingRow["TaxTypeCode"]).Trim() != "O")
                            {
                                DataRow txRecordRow = taxTable.NewRow();
                                txRecordRow["SlNo"] = productRow[SerialColumnName];
                                txRecordRow["TaxCode"] = taxexistingRow["Taxes_ID"];
                                txRecordRow["AltTaxCode"] = "0";
                                txRecordRow["Percentage"] = taxexistingRow["TaxRates_Rate"];
                                txRecordRow["Amount"] = Math.Round(Convert.ToDecimal(productRow[AmountColumnName]) * (Convert.ToDecimal(taxexistingRow["TaxRates_Rate"]) / 100), 2, MidpointRounding.AwayFromZero);
                                taxTable.Rows.Add(txRecordRow);

                            }
                        }
                    }
                    else if (HSNNilRated == "True" || SERVICENilRated == "True")
                    {
                        foreach (DataRow taxexistingRow in taxDetail.Rows)
                        {
                            if (Convert.ToString(taxexistingRow["TaxTypeCode"]).Trim() != "O")
                            {
                                DataRow txRecordRow = taxTable.NewRow();
                                txRecordRow["SlNo"] = productRow[SerialColumnName];
                                txRecordRow["TaxCode"] = taxexistingRow["Taxes_ID"];
                                txRecordRow["AltTaxCode"] = "0";
                                txRecordRow["Percentage"] = taxexistingRow["TaxRates_Rate"];
                                txRecordRow["Amount"] = Math.Round(Convert.ToDecimal(productRow[AmountColumnName]) * (Convert.ToDecimal(taxexistingRow["TaxRates_Rate"]) / 100), 2, MidpointRounding.AwayFromZero);
                                taxTable.Rows.Add(txRecordRow);

                            }
                        }
                    }
                }


            }



            #region setRound of for GST
            //DataTable GSTTABLE = oDBEngine.GetDataTable("select TaxRates_ID,TaxTypeCode from Config_TaxRates config inner join Master_Taxes mast on config.TaxRates_TaxCode =mast.Taxes_ID where TaxTypeCode in ('IGST','CGST','SGST','UTGST')");

            //foreach (DataRow gstrow in taxTable.Rows)
            //{
            //    if (GSTTABLE.Select("TaxRates_ID=" + gstrow["TaxCode"]).Length > 0)
            //    {
            //        gstrow["Amount"] = Math.Round(Convert.ToDecimal(gstrow["Amount"]));
            //    }
            //}

            #endregion





            foreach (DataRow productRow in productDetails.Rows)
            {
                #region SetRounded of ledger


                if (taxTable.Select("SlNo='" + productRow[SerialColumnName] + "'").Length > 0)
                {
                    DataTable filteredTable = taxTable.Select("SlNo='" + productRow[SerialColumnName] + "'").CopyToDataTable();
                    decimal totalTaxAmount = Convert.ToDecimal(filteredTable.Compute("SUM(Amount)", string.Empty));

                    if (totalTaxAmount != Convert.ToDecimal(productRow[chargesColumns]))
                    {
                        decimal roundOf = Convert.ToDecimal(productRow[chargesColumns]) - totalTaxAmount;
                        if (roundOf > 0)
                        {
                            if (roundOfPlus.Trim() != "")
                            {
                                DataRow txRecordRow = taxTable.NewRow();
                                txRecordRow["SlNo"] = productRow[SerialColumnName];
                                txRecordRow["TaxCode"] = roundOfPlus;
                                txRecordRow["AltTaxCode"] = "0";
                                txRecordRow["Percentage"] = "0";
                                txRecordRow["Amount"] = roundOf;
                                taxTable.Rows.Add(txRecordRow);
                            }
                        }
                        else if (roundOf < 0)
                        {
                            if (roundofMinus.Trim() != "")
                            {
                                DataRow txRecordRow = taxTable.NewRow();
                                txRecordRow["SlNo"] = productRow[SerialColumnName];
                                txRecordRow["TaxCode"] = roundofMinus;
                                txRecordRow["AltTaxCode"] = "0";
                                txRecordRow["Percentage"] = "0";
                                txRecordRow["Amount"] = roundOf;
                                taxTable.Rows.Add(txRecordRow);
                            }
                        }
                    }
                }



                #endregion
            }

            return taxTable;
        }

        public DataTable SetTaxTableDataWithProductSerialRoundOff(ref DataTable productDetails, string SerialColumnName, string ProductColumnName, string AmountColumnName, string chargesColumns, DataTable taxTable, string ApplicableFor, string TransactionDate, string BranchId, string ShippingState, string inclusiveOrExclusive)
        {
            string fromState = "", shippingStateCode = "", TaxType = "", roundOfPlus = "", roundofMinus = "";
            DataTable fetchedData = oDBEngine.GetDataTable("select StateCode  from tbl_master_branch br inner join tbl_master_state st on br.branch_state=st.id where branch_id=" + BranchId);
            if (fetchedData != null)
                fromState = Convert.ToString(fetchedData.Rows[0][0]).Trim();

            fetchedData = oDBEngine.GetDataTable("select StateCode from tbl_master_state where id=" + ShippingState);
            if (fetchedData != null)
                shippingStateCode = Convert.ToString(fetchedData.Rows[0][0]).Trim();

            fetchedData = oDBEngine.GetDataTable("select TaxRates_ID from Config_TaxRates config inner join Master_Taxes masterTax on config.TaxRates_TaxCode = masterTax.Taxes_ID  where  TaxRates_RoundingOff='R+' and Taxes_ApplicableFor in ('B','" + ApplicableFor + "')");
            if (fetchedData.Rows.Count > 0)
                roundOfPlus = Convert.ToString(fetchedData.Rows[0][0]).Trim();

            fetchedData = oDBEngine.GetDataTable("select TaxRates_ID from Config_TaxRates config inner join Master_Taxes masterTax on config.TaxRates_TaxCode = masterTax.Taxes_ID  where  TaxRates_RoundingOff='R-' and Taxes_ApplicableFor in ('B','" + ApplicableFor + "')");
            if (fetchedData.Rows.Count > 0)
                roundofMinus = Convert.ToString(fetchedData.Rows[0][0]).Trim();




            #region setTaxType
            if (fromState == shippingStateCode)
            {
                TaxType = "SGST";
                //Remove shippingStateCode == "35" as now Delhi is not under UTGST
                if (shippingStateCode == "4" || shippingStateCode == "26" || shippingStateCode == "25" || shippingStateCode == "35" || shippingStateCode == "31" || shippingStateCode == "34")
                {
                    TaxType = "UTGST";
                }
            }
            else
            {
                TaxType = "IGST";
            }

            #endregion






            foreach (DataRow productRow in productDetails.Rows)
            {
                if (taxTable.Select("SlNo='" + productRow[SerialColumnName]+"'").Length == 0)
                {

                    DataTable taxDetail = new DataTable();
                    ProcedureExecute proc = new ProcedureExecute("prc_GstTaxDetails");
                    proc.AddVarcharPara("@Action", 500, "LoadOtherTaxDetails");
                    proc.AddVarcharPara("@ProductID", 10, Convert.ToString(productRow[ProductColumnName]));
                    proc.AddVarcharPara("@applicableFor", 5, ApplicableFor);
                    proc.AddVarcharPara("@TransDate", 10, TransactionDate);
                    taxDetail = proc.GetTable();


                    #region DeleteExtra GstCode

                    if (TaxType == "UTGST")
                    {
                        foreach (DataRow dr in taxDetail.Rows)
                        {
                            if (Convert.ToString(dr["TaxTypeCode"]).Trim() == "IGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "SGST")
                            {
                                dr.Delete();
                            }
                        }
                    }
                    else if (TaxType == "IGST")
                    {
                        foreach (DataRow dr in taxDetail.Rows)
                        {
                            if (Convert.ToString(dr["TaxTypeCode"]).Trim() == "CGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "SGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "UTGST")
                            {
                                dr.Delete();
                            }
                        }
                    }
                    else if (TaxType == "SGST")
                    {
                        foreach (DataRow dr in taxDetail.Rows)
                        {
                            if (Convert.ToString(dr["TaxTypeCode"]).Trim() == "IGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "UTGST")
                            {
                                dr.Delete();
                            }
                        }
                    }
                    taxDetail.AcceptChanges();
                    #endregion

                    //#region CreateTempTaxTable
                    //DataTable TaxRecord = new DataTable();
                    //TaxRecord.Columns.Add("SlNo", typeof(System.Int32));
                    //TaxRecord.Columns.Add("TaxCode", typeof(System.String));
                    //TaxRecord.Columns.Add("AltTaxCode", typeof(System.String));
                    //TaxRecord.Columns.Add("Percentage", typeof(System.Decimal));
                    //TaxRecord.Columns.Add("Amount", typeof(System.Decimal));
                    //#endregion
                    //decimal totalParcentage = 0;
                    //foreach (DataRow dr in taxDetail.Rows)
                    //{
                    //    if (Convert.ToString(dr["TaxTypeCode"]).Trim() == "CGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "SGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "IGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "UTGST")
                    //    {
                    //        totalParcentage += Convert.ToDecimal(dr["TaxRates_Rate"]);
                    //    }
                    //}


                    if (Convert.ToDecimal(productRow[chargesColumns]) != 0)
                    {
                        foreach (DataRow taxexistingRow in taxDetail.Rows)
                        {
                            if (Convert.ToString(taxexistingRow["TaxTypeCode"]).Trim() != "O")
                            {
                                DataRow txRecordRow = taxTable.NewRow();
                                txRecordRow["SlNo"] = productRow[SerialColumnName];
                                txRecordRow["TaxCode"] = taxexistingRow["Taxes_ID"];
                                txRecordRow["AltTaxCode"] = "0";
                                txRecordRow["Percentage"] = taxexistingRow["TaxRates_Rate"];
                                //Rev 1.0
                                txRecordRow["Amount"] = Math.Round(Convert.ToDouble(Convert.ToDecimal(productRow[AmountColumnName]) * (Convert.ToDecimal(taxexistingRow["TaxRates_Rate"]) / 100)), 2);
                                //txRecordRow["Amount"] = RoundUp(Convert.ToDouble(Convert.ToDecimal(productRow[AmountColumnName]) * (Convert.ToDecimal(taxexistingRow["TaxRates_Rate"]) / 100)), 2);
                                //Rev 1.0 End
                                taxTable.Rows.Add(txRecordRow);

                            }
                        }
                    }
                }
            }
            #region setRound of for GST
            //DataTable GSTTABLE = oDBEngine.GetDataTable("select TaxRates_ID,TaxTypeCode from Config_TaxRates config inner join Master_Taxes mast on config.TaxRates_TaxCode =mast.Taxes_ID where TaxTypeCode in ('IGST','CGST','SGST','UTGST')");

            //foreach (DataRow gstrow in taxTable.Rows)
            //{
            //    if (GSTTABLE.Select("TaxRates_ID=" + gstrow["TaxCode"]).Length > 0)
            //    {
            //        gstrow["Amount"] = Math.Round(Convert.ToDecimal(gstrow["Amount"]));
            //    }
            //}

            #endregion
            foreach (DataRow productRow in productDetails.Rows)
            {
                #region SetRounded of ledger
                if (taxTable.Select("SlNo='" + productRow[SerialColumnName] + "'").Length > 0  )
                {
                    DataTable filteredTable = taxTable.Select("SlNo='" + productRow[SerialColumnName] + "'").CopyToDataTable();
                    // decimal totalTaxAmount = Convert.ToDecimal(filteredTable.Compute("SUM(Amount)", string.Empty));
                    
                    decimal totalTaxAmount = 0;
                    GetTotalAmount(filteredTable, ref totalTaxAmount);

                    if (totalTaxAmount != Convert.ToDecimal(productRow[chargesColumns]))
                    {
                        decimal roundOf = Convert.ToDecimal(productRow[chargesColumns]) - totalTaxAmount;
                        if (inclusiveOrExclusive == "I")
                        {
                            productRow[chargesColumns] = Convert.ToDecimal(productRow[chargesColumns]) - roundOf;
                            productRow[AmountColumnName] = Convert.ToDecimal(productRow[AmountColumnName]) + roundOf;
                        }
                        else {
                            productRow[chargesColumns] = totalTaxAmount;
                        }
                    }
                }
                #endregion
            }

            productDetails.AcceptChanges();

            return taxTable;
        }

        public DataTable SetTaxTableDataWithProductSerialForOldUnit(ref DataTable productDetails, string SerialColumnName, string ProductColumnName, string AmountColumnName, string chargesColumns, DataTable taxTable, string ApplicableFor, string TransactionDate, string BranchId, string ShippingState, string inclusiveOrExclusive)
        {
            string fromState = "", shippingStateCode = "", TaxType = "", roundOfPlus = "", roundofMinus = "";
            DataTable fetchedData = oDBEngine.GetDataTable("select StateCode  from tbl_master_branch br inner join tbl_master_state st on br.branch_state=st.id where branch_id=" + BranchId);
            if (fetchedData != null)
                fromState = Convert.ToString(fetchedData.Rows[0][0]).Trim();

            fetchedData = oDBEngine.GetDataTable("select StateCode from tbl_master_state where id=" + ShippingState);
            if (fetchedData != null)
                shippingStateCode = Convert.ToString(fetchedData.Rows[0][0]).Trim();

            fetchedData = oDBEngine.GetDataTable("select TaxRates_ID from Config_TaxRates config inner join Master_Taxes masterTax on config.TaxRates_TaxCode = masterTax.Taxes_ID  where  TaxRates_RoundingOff='R+' and Taxes_ApplicableFor in ('B','" + ApplicableFor + "')");
            if (fetchedData.Rows.Count > 0)
                roundOfPlus = Convert.ToString(fetchedData.Rows[0][0]).Trim();

            fetchedData = oDBEngine.GetDataTable("select TaxRates_ID from Config_TaxRates config inner join Master_Taxes masterTax on config.TaxRates_TaxCode = masterTax.Taxes_ID  where  TaxRates_RoundingOff='R-' and Taxes_ApplicableFor in ('B','" + ApplicableFor + "')");
            if (fetchedData.Rows.Count > 0)
                roundofMinus = Convert.ToString(fetchedData.Rows[0][0]).Trim();




            #region setTaxType
            if (fromState == shippingStateCode)
            {
                TaxType = "SGST";
                //Remove shippingStateCode == "35" as now Delhi is not under UTGST
                if (shippingStateCode == "4" || shippingStateCode == "26" || shippingStateCode == "25" || shippingStateCode == "35" || shippingStateCode == "31" || shippingStateCode == "34")
                {
                    TaxType = "UTGST";
                }
            }
            else
            {
                TaxType = "IGST";
            }

            #endregion






            foreach (DataRow productRow in productDetails.Rows)
            {
                if (taxTable.Select("SlNo='" + productRow[SerialColumnName] + "'").Length == 0)
                {

                    DataTable taxDetail = new DataTable();
                    ProcedureExecute proc = new ProcedureExecute("prc_GstTaxDetails");
                    proc.AddVarcharPara("@Action", 500, "LoadOtherTaxDetails");
                    proc.AddVarcharPara("@ProductID", 10, Convert.ToString(productRow[ProductColumnName]));
                    proc.AddVarcharPara("@applicableFor", 5, ApplicableFor);
                    proc.AddVarcharPara("@TransDate", 10, TransactionDate);
                    taxDetail = proc.GetTable();


                    #region DeleteExtra GstCode

                    if (TaxType == "UTGST")
                    {
                        foreach (DataRow dr in taxDetail.Rows)
                        {
                            if (Convert.ToString(dr["TaxTypeCode"]).Trim() == "IGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "SGST")
                            {
                                dr.Delete();
                            }
                        }
                    }
                    else if (TaxType == "IGST")
                    {
                        foreach (DataRow dr in taxDetail.Rows)
                        {
                            if (Convert.ToString(dr["TaxTypeCode"]).Trim() == "CGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "SGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "UTGST")
                            {
                                dr.Delete();
                            }
                        }
                    }
                    else if (TaxType == "SGST")
                    {
                        foreach (DataRow dr in taxDetail.Rows)
                        {
                            if (Convert.ToString(dr["TaxTypeCode"]).Trim() == "IGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "UTGST")
                            {
                                dr.Delete();
                            }
                        }
                    }
                    taxDetail.AcceptChanges();
                    #endregion




                    if (Convert.ToDecimal(productRow[chargesColumns]) != 0)
                    {
                        foreach (DataRow taxexistingRow in taxDetail.Rows)
                        {
                            if (Convert.ToString(taxexistingRow["TaxTypeCode"]).Trim() != "O")
                            {
                                DataRow txRecordRow = taxTable.NewRow();
                                txRecordRow["SlNo"] = productRow[SerialColumnName];
                                txRecordRow["TaxCode"] = taxexistingRow["Taxes_ID"];
                                txRecordRow["AltTaxCode"] = "0";
                                txRecordRow["Percentage"] = taxexistingRow["TaxRates_Rate"];
                                txRecordRow["Amount"] = Math.Round((Convert.ToDecimal(productRow[AmountColumnName]) - (Convert.ToDecimal(productRow["ProdAvgvalue"]) * Convert.ToDecimal(productRow["Quantity"]))) * (Convert.ToDecimal(taxexistingRow["TaxRates_Rate"]) / 100), 2);
                                taxTable.Rows.Add(txRecordRow);

                            }
                        }
                    }
                }


            }




            foreach (DataRow productRow in productDetails.Rows)
            {
                #region SetRounded of ledger


                if (taxTable.Select("SlNo='" + productRow[SerialColumnName] + "'").Length > 0)
                {
                    DataTable filteredTable = taxTable.Select("SlNo='" + productRow[SerialColumnName] + "'").CopyToDataTable();
                    decimal totalTaxAmount = Convert.ToDecimal(filteredTable.Compute("SUM(Amount)", string.Empty));

                    if (totalTaxAmount != Convert.ToDecimal(productRow[chargesColumns]))
                    {
                        decimal roundOf = Convert.ToDecimal(productRow[chargesColumns]) - totalTaxAmount;

                        productRow[chargesColumns] = Convert.ToDecimal(productRow[chargesColumns]) - roundOf;
                        if (inclusiveOrExclusive == "I")
                            productRow[AmountColumnName] = Convert.ToDecimal(productRow[AmountColumnName]) + roundOf;
                        else if (inclusiveOrExclusive == "E")
                        {
                            productRow["TotalAmount"] = Convert.ToDecimal(productRow["TotalAmount"]) - roundOf;
                        }
                    }
                }



                #endregion
            }

            productDetails.AcceptChanges();

            return taxTable;
        }

        public DataTable SetTaxAmountWithGSTonDetailsTable(DataTable ProductDetail, string ProductIdColumnName, string taxChargesColumnName, string AmountColName, string NetAmountColName, string TransactionDate, string Applicablefor, string branchId, string shippingCode, string inclusiveOrExclusive)
        {
            string fromState = "", shippingStateCode = "", TaxType = "", roundOfPlus = "", roundofMinus = "";
            DataTable fetchedData = oDBEngine.GetDataTable("select StateCode  from tbl_master_branch br inner join tbl_master_state st on br.branch_state=st.id where branch_id=" + branchId);
            if (fetchedData != null)
                fromState = Convert.ToString(fetchedData.Rows[0][0]).Trim();

            fetchedData = oDBEngine.GetDataTable("select StateCode from tbl_master_state where id=" + shippingCode);
            if (fetchedData != null && fetchedData.Rows.Count>0)
                shippingStateCode = Convert.ToString(fetchedData.Rows[0][0]).Trim();

            fetchedData = oDBEngine.GetDataTable("select TaxRates_ID from Config_TaxRates config inner join Master_Taxes masterTax on config.TaxRates_TaxCode = masterTax.Taxes_ID  where  TaxRates_RoundingOff='R+' and Taxes_ApplicableFor in ('B','" + Applicablefor + "')");
            if (fetchedData.Rows.Count > 0)
                roundOfPlus = Convert.ToString(fetchedData.Rows[0][0]).Trim();

            fetchedData = oDBEngine.GetDataTable("select TaxRates_ID from Config_TaxRates config inner join Master_Taxes masterTax on config.TaxRates_TaxCode = masterTax.Taxes_ID  where  TaxRates_RoundingOff='R-' and Taxes_ApplicableFor in ('B','" + Applicablefor + "')");
            if (fetchedData.Rows.Count > 0)
                roundofMinus = Convert.ToString(fetchedData.Rows[0][0]).Trim();

            #region setTaxType
            if (fromState == shippingStateCode)
            {
                TaxType = "SGST";
                if (shippingStateCode == "4" || shippingStateCode == "26" || shippingStateCode == "25" || shippingStateCode == "35" || shippingStateCode == "31" || shippingStateCode == "34")
                {
                    TaxType = "UTGST";
                }
            }
            else
            {
                TaxType = "IGST";
            }

            #endregion


            foreach (DataRow productRow in ProductDetail.Rows)
            {

                DataTable taxDetail = new DataTable();
                ProcedureExecute proc = new ProcedureExecute("prc_GstTaxDetails");
                proc.AddVarcharPara("@Action", 500, "LoadOtherTaxDetails");
                proc.AddVarcharPara("@ProductID", 10, Convert.ToString(productRow[ProductIdColumnName]));
                proc.AddVarcharPara("@applicableFor", 5, Applicablefor);
                proc.AddVarcharPara("@TransDate", 10, TransactionDate);
                taxDetail = proc.GetTable();



                #region DeleteExtra GstCode

                if (TaxType == "UTGST")
                {
                    foreach (DataRow dr in taxDetail.Rows)
                    {
                        if (Convert.ToString(dr["TaxTypeCode"]).Trim() == "IGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "SGST")
                        {
                            dr.Delete();
                        }
                    }
                }
                else if (TaxType == "IGST")
                {
                    foreach (DataRow dr in taxDetail.Rows)
                    {
                        if (Convert.ToString(dr["TaxTypeCode"]).Trim() == "CGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "SGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "UTGST")
                        {
                            dr.Delete();
                        }
                    }
                }
                else if (TaxType == "SGST")
                {
                    foreach (DataRow dr in taxDetail.Rows)
                    {
                        if (Convert.ToString(dr["TaxTypeCode"]).Trim() == "IGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "UTGST")
                        {
                            dr.Delete();
                        }
                    }
                }
                taxDetail.AcceptChanges();
                #endregion


                decimal ChargesAmount = 0, totalCharges = 0;
                foreach (DataRow taxexistingRow in taxDetail.Rows)
                {
                    if (Convert.ToString(taxexistingRow["TaxTypeCode"]).Trim() != "O")
                    {
                        totalCharges = totalCharges + Convert.ToDecimal(taxexistingRow["TaxRates_Rate"]);
                    }
                }

                if (inclusiveOrExclusive == "I")
                {
                    totalCharges = 1 + (totalCharges / 100);

                    ChargesAmount = Math.Round(Convert.ToDecimal(productRow[AmountColName]) - (Convert.ToDecimal(productRow[AmountColName]) / totalCharges));
                    productRow[AmountColName] = Convert.ToDecimal(productRow[AmountColName]) - ChargesAmount;
                    productRow[taxChargesColumnName] = ChargesAmount + ".00";

                }
                else if (inclusiveOrExclusive == "E")
                {
                    ChargesAmount = Math.Round(Convert.ToDecimal(productRow[AmountColName]) * (totalCharges / 100));
                    productRow[NetAmountColName] = Convert.ToDecimal(productRow[AmountColName]) - ChargesAmount;
                    productRow[taxChargesColumnName] = ChargesAmount + ".00";
                }

            }

            ProductDetail.AcceptChanges();
            return ProductDetail;
        }

        public DataTable SetTaxAmountWithGSTonDetailsTableOrderBasket(DataTable ProductDetail, string ProductIdColumnName, string taxChargesColumnName, string AmountColName, string NetAmountColName, string TransactionDate, string Applicablefor, string branchId, string shippingCode, string inclusiveOrExclusive)
        {
            string fromState = "", shippingStateCode = "", TaxType = "", roundOfPlus = "", roundofMinus = "";
            DataTable fetchedData = oDBEngine.GetDataTable("select StateCode  from tbl_master_branch br inner join tbl_master_state st on br.branch_state=st.id where branch_id=" + branchId);
            if (fetchedData != null)
                fromState = Convert.ToString(fetchedData.Rows[0][0]).Trim();

            fetchedData = oDBEngine.GetDataTable("select StateCode from tbl_master_state where id=" + shippingCode);
            if (fetchedData != null)
                shippingStateCode = Convert.ToString(fetchedData.Rows[0][0]).Trim();

            fetchedData = oDBEngine.GetDataTable("select TaxRates_ID from Config_TaxRates config inner join Master_Taxes masterTax on config.TaxRates_TaxCode = masterTax.Taxes_ID  where  TaxRates_RoundingOff='R+' and Taxes_ApplicableFor in ('B','" + Applicablefor + "')");
            if (fetchedData.Rows.Count > 0)
                roundOfPlus = Convert.ToString(fetchedData.Rows[0][0]).Trim();

            fetchedData = oDBEngine.GetDataTable("select TaxRates_ID from Config_TaxRates config inner join Master_Taxes masterTax on config.TaxRates_TaxCode = masterTax.Taxes_ID  where  TaxRates_RoundingOff='R-' and Taxes_ApplicableFor in ('B','" + Applicablefor + "')");
            if (fetchedData.Rows.Count > 0)
                roundofMinus = Convert.ToString(fetchedData.Rows[0][0]).Trim();

            #region setTaxType
            if (fromState == shippingStateCode)
            {
                TaxType = "SGST";
                if (shippingStateCode == "4" || shippingStateCode == "26" || shippingStateCode == "25" || shippingStateCode == "35" || shippingStateCode == "31" || shippingStateCode == "34")
                {
                    TaxType = "UTGST";
                }
            }
            else
            {
                TaxType = "IGST";
            }

            #endregion


            foreach (DataRow productRow in ProductDetail.Rows)
            {

                DataTable taxDetail = new DataTable();
                ProcedureExecute proc = new ProcedureExecute("prc_GstTaxDetails");
                proc.AddVarcharPara("@Action", 500, "LoadOtherTaxDetails");
                proc.AddVarcharPara("@ProductID", 10, Convert.ToString(productRow[ProductIdColumnName]));
                proc.AddVarcharPara("@applicableFor", 5, Applicablefor);
                proc.AddVarcharPara("@TransDate", 10, TransactionDate);
                taxDetail = proc.GetTable();



                #region DeleteExtra GstCode

                if (TaxType == "UTGST")
                {
                    foreach (DataRow dr in taxDetail.Rows)
                    {
                        if (Convert.ToString(dr["TaxTypeCode"]).Trim() == "IGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "SGST")
                        {
                            dr.Delete();
                        }
                    }
                }
                else if (TaxType == "IGST")
                {
                    foreach (DataRow dr in taxDetail.Rows)
                    {
                        if (Convert.ToString(dr["TaxTypeCode"]).Trim() == "CGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "SGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "UTGST")
                        {
                            dr.Delete();
                        }
                    }
                }
                else if (TaxType == "SGST")
                {
                    foreach (DataRow dr in taxDetail.Rows)
                    {
                        if (Convert.ToString(dr["TaxTypeCode"]).Trim() == "IGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "UTGST")
                        {
                            dr.Delete();
                        }
                    }
                }
                taxDetail.AcceptChanges();
                #endregion


                decimal ChargesAmount = 0, totalCharges = 0;
                foreach (DataRow taxexistingRow in taxDetail.Rows)
                {
                    if (Convert.ToString(taxexistingRow["TaxTypeCode"]).Trim() != "O")
                    {
                        totalCharges = totalCharges + Convert.ToDecimal(taxexistingRow["TaxRates_Rate"]);
                    }
                }

                if (inclusiveOrExclusive == "I")
                {
                    totalCharges = 1 + (totalCharges / 100);

                    ChargesAmount = Math.Round(Convert.ToDecimal(productRow[AmountColName]) - (Convert.ToDecimal(productRow[AmountColName]) / totalCharges), 2);
                    productRow[AmountColName] = Convert.ToDecimal(productRow[AmountColName]) - ChargesAmount;
                    productRow[taxChargesColumnName] = ChargesAmount ;

                }
                else if (inclusiveOrExclusive == "E")
                {
                    ChargesAmount = Math.Round(Convert.ToDecimal(productRow[AmountColName]) * (totalCharges / 100),2);
                    productRow[NetAmountColName] = Convert.ToDecimal(productRow[AmountColName]) - ChargesAmount;
                    productRow[taxChargesColumnName] = ChargesAmount ;
                }

            }

            ProductDetail.AcceptChanges();
            return ProductDetail;
        }

        public DataTable GetReverseTaxTable(DataTable productDetails, string SerialColumnName, string ProductColumnName, string AmountColumnName, string chargesColumns, DataTable taxTable, string ApplicableFor, string TransactionDate, string BranchId, string ShippingState, string inclusiveOrExclusive, string vendorId)
        {
            string VendorState = "", TaxType = "", roundOfPlus = "", roundofMinus = "";
            //DataTable fetchedData = oDBEngine.GetDataTable("select StateCode  from tbl_master_branch br inner join tbl_master_state st on br.branch_state=st.id where branch_id=" + BranchId);
            //if (fetchedData != null)
            //    fromState = Convert.ToString(fetchedData.Rows[0][0]).Trim();

            //fetchedData = oDBEngine.GetDataTable("select StateCode from tbl_master_state where id=" + ShippingState);
            //if (fetchedData != null)
            //    shippingStateCode = Convert.ToString(fetchedData.Rows[0][0]).Trim();

            DataTable fetchedData = oDBEngine.GetDataTable("select TaxRates_ID from Config_TaxRates config inner join Master_Taxes masterTax on config.TaxRates_TaxCode = masterTax.Taxes_ID  where  TaxRates_RoundingOff='R+' and Taxes_ApplicableFor in ('B','" + ApplicableFor + "')");
            if (fetchedData.Rows.Count > 0)
                roundOfPlus = Convert.ToString(fetchedData.Rows[0][0]).Trim();

            fetchedData = oDBEngine.GetDataTable("select TaxRates_ID from Config_TaxRates config inner join Master_Taxes masterTax on config.TaxRates_TaxCode = masterTax.Taxes_ID  where  TaxRates_RoundingOff='R-' and Taxes_ApplicableFor in ('B','" + ApplicableFor + "')");
            if (fetchedData.Rows.Count > 0)
                roundofMinus = Convert.ToString(fetchedData.Rows[0][0]).Trim();

            fetchedData = oDBEngine.GetDataTable("select StateCode from tbl_master_address addrs inner join tbl_master_state st on addrs.add_state = st.id where add_addressType='Shipping' and Isdefault=1 and add_cntId='" + vendorId + "'");
            if (fetchedData.Rows.Count > 0)
                VendorState = Convert.ToString(fetchedData.Rows[0][0]).Trim();

            string CompInternalId = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
            string[] compGstin = oDBEngine.GetFieldValue1("tbl_master_company", "cmp_gstin", "cmp_internalid='" + CompInternalId + "'", 1);


            string BrancgStateCode = "", BranchGSTIN = "";
            DataTable BranchTable = oDBEngine.GetDataTable("select StateCode,branch_GSTIN   from tbl_master_branch branch inner join tbl_master_state st on branch.branch_state=st.id where branch_id=" + BranchId);
            if (BranchTable != null)
            {
                BrancgStateCode = Convert.ToString(BranchTable.Rows[0][0]);
                BranchGSTIN = Convert.ToString(BranchTable.Rows[0][1]);
                if (BranchGSTIN.Trim() != "")
                {
                    BrancgStateCode = BranchGSTIN.Substring(0, 2);
                }
            }


            if (BranchGSTIN.Trim() == "")
            {
                BrancgStateCode = compGstin[0].Substring(0, 2);
            }

            #region setTaxType
            if (BrancgStateCode == VendorState)
            {
                TaxType = "SGST";
                if (VendorState == "4" || VendorState == "26" || VendorState == "25" || VendorState == "35" || VendorState == "31" || VendorState == "34")
                {
                    TaxType = "UTGST";
                }
            }
            else
            {
                TaxType = "IGST";
            }

            #endregion




            if (!(BrancgStateCode.Trim() == "" || VendorState.Trim() == ""))
            {
                foreach (DataRow productRow in productDetails.Rows)
                {
                    if (taxTable.Select("SlNo='" + productRow[SerialColumnName] + "'").Length == 0)
                    {

                        DataTable taxDetail = new DataTable();
                        ProcedureExecute proc = new ProcedureExecute("prc_GstTaxDetails");
                        proc.AddVarcharPara("@Action", 500, "LoadOtherTaxDetails");
                        proc.AddVarcharPara("@ProductID", 10, Convert.ToString(productRow[ProductColumnName]));
                        proc.AddVarcharPara("@applicableFor", 5, ApplicableFor);
                        proc.AddVarcharPara("@TransDate", 10, TransactionDate);
                        taxDetail = proc.GetTable();


                        #region DeleteExtra GstCode

                        if (TaxType == "UTGST")
                        {
                            foreach (DataRow dr in taxDetail.Rows)
                            {
                                if (Convert.ToString(dr["TaxTypeCode"]).Trim() == "IGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "SGST")
                                {
                                    dr.Delete();
                                }
                            }
                        }
                        else if (TaxType == "IGST")
                        {
                            foreach (DataRow dr in taxDetail.Rows)
                            {
                                if (Convert.ToString(dr["TaxTypeCode"]).Trim() == "CGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "SGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "UTGST")
                                {
                                    dr.Delete();
                                }
                            }
                        }
                        else if (TaxType == "SGST")
                        {
                            foreach (DataRow dr in taxDetail.Rows)
                            {
                                if (Convert.ToString(dr["TaxTypeCode"]).Trim() == "IGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "UTGST")
                                {
                                    dr.Delete();
                                }
                            }
                        }
                        taxDetail.AcceptChanges();
                        #endregion



                        foreach (DataRow taxexistingRow in taxDetail.Rows)
                        {
                            if (Convert.ToString(taxexistingRow["TaxTypeCode"]).Trim() != "O")
                            {
                                DataRow txRecordRow = taxTable.NewRow();
                                txRecordRow["SlNo"] = productRow[SerialColumnName];
                                txRecordRow["TaxCode"] = taxexistingRow["Taxes_ID"];
                                txRecordRow["AltTaxCode"] = "0";
                                txRecordRow["Percentage"] = taxexistingRow["TaxRates_Rate"];
                                txRecordRow["Amount"] = Convert.ToDecimal(productRow[AmountColumnName]) * (Convert.ToDecimal(taxexistingRow["TaxRates_Rate"]) / 100);
                                taxTable.Rows.Add(txRecordRow);

                            }
                        }

                    }


                }
            }






            return taxTable;
        }


        public DataTable GetGSTTaxDataForCustomerRecPay(DataTable detailTable, DataTable taxTable, string ProdList, string RecPay, string ApplicableFor, string TransactionDate, string BranchId, string ShippingState)
        {
            #region Customer Receipt zone

            if (RecPay == "R")
            {
                DataRow[] AdvanceRow = detailTable.Select("Type='Advance'");
                if (AdvanceRow.Length > 0)
                {
                    string[] prodarray = ProdList.Split(',');

                    if (prodarray.Length > 0)
                    {
                        decimal recpayAmount = Convert.ToDecimal(AdvanceRow[0]["Receipt"]);

                        string fromState = "", branchGSTIN = "", shippingStateCode = "", TaxType = "", roundOfPlus = "", roundofMinus = "";
                        string CompInternalId = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
                        string[] compGstin = oDBEngine.GetFieldValue1("tbl_master_company", "cmp_gstin", "cmp_internalid='" + CompInternalId + "'", 1);


                        DataTable fetchedData = oDBEngine.GetDataTable("select StateCode,branch_GSTIN  from tbl_master_branch br inner join tbl_master_state st on br.branch_state=st.id where branch_id=" + BranchId);
                        if (fetchedData != null)
                        {
                            fromState = Convert.ToString(fetchedData.Rows[0][0]).Trim();
                            branchGSTIN = Convert.ToString(fetchedData.Rows[0][1]).Trim();
                            if (branchGSTIN != "")
                                fromState = branchGSTIN.Substring(0, 2);
                        }

                        fetchedData = oDBEngine.GetDataTable("select StateCode from tbl_master_state where id=" + ShippingState);
                        if (fetchedData != null)
                            shippingStateCode = Convert.ToString(fetchedData.Rows[0][0]).Trim();

                        fetchedData = oDBEngine.GetDataTable("select TaxRates_ID from Config_TaxRates config inner join Master_Taxes masterTax on config.TaxRates_TaxCode = masterTax.Taxes_ID  where  TaxRates_RoundingOff='R+' and Taxes_ApplicableFor in ('B','" + ApplicableFor + "')");
                        if (fetchedData.Rows.Count > 0)
                            roundOfPlus = Convert.ToString(fetchedData.Rows[0][0]).Trim();

                        fetchedData = oDBEngine.GetDataTable("select TaxRates_ID from Config_TaxRates config inner join Master_Taxes masterTax on config.TaxRates_TaxCode = masterTax.Taxes_ID  where  TaxRates_RoundingOff='R-' and Taxes_ApplicableFor in ('B','" + ApplicableFor + "')");
                        if (fetchedData.Rows.Count > 0)
                            roundofMinus = Convert.ToString(fetchedData.Rows[0][0]).Trim();


                        #region setTaxType
                        if (fromState == shippingStateCode)
                        {
                            TaxType = "SGST";
                            //Remove shippingStateCode == "35" as now Delhi is not under UTGST
                            if (shippingStateCode == "4" || shippingStateCode == "26" || shippingStateCode == "25" || shippingStateCode == "35" || shippingStateCode == "31" || shippingStateCode == "34")
                            {
                                TaxType = "UTGST";
                            }
                        }
                        else
                        {
                            TaxType = "IGST";
                        }

                        #endregion


                        DataTable taxDetail = new DataTable();
                        ProcedureExecute proc = new ProcedureExecute("prc_GstTaxDetails");
                        proc.AddVarcharPara("@Action", 500, "LoadOtherTaxDetails");
                        proc.AddVarcharPara("@ProductID", 10, Convert.ToString(prodarray[0]));
                        proc.AddVarcharPara("@applicableFor", 5, ApplicableFor);
                        proc.AddVarcharPara("@TransDate", 10, TransactionDate);
                        taxDetail = proc.GetTable();


                        #region DeleteExtra GstCode

                        if (TaxType == "UTGST")
                        {
                            foreach (DataRow dr in taxDetail.Rows)
                            {
                                if (Convert.ToString(dr["TaxTypeCode"]).Trim() == "IGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "SGST")
                                {
                                    dr.Delete();
                                }
                            }
                        }
                        else if (TaxType == "IGST")
                        {
                            foreach (DataRow dr in taxDetail.Rows)
                            {
                                if (Convert.ToString(dr["TaxTypeCode"]).Trim() == "CGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "SGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "UTGST")
                                {
                                    dr.Delete();
                                }
                            }
                        }
                        else if (TaxType == "SGST")
                        {
                            foreach (DataRow dr in taxDetail.Rows)
                            {
                                if (Convert.ToString(dr["TaxTypeCode"]).Trim() == "IGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "UTGST")
                                {
                                    dr.Delete();
                                }
                            }
                        }
                        if (compGstin[0].Trim() == "" && branchGSTIN == "")
                        {
                            foreach (DataRow dr in taxDetail.Rows)
                            {
                                if (Convert.ToString(dr["TaxTypeCode"]).Trim() == "CGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "SGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "UTGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "IGST")
                                {
                                    dr.Delete();
                                }
                            }
                            taxDetail.AcceptChanges();
                        }

                        taxDetail.AcceptChanges();
                        #endregion

                        decimal TotalPercentage = 0, InclusiveAmt = 0, TaxAmount = 0, RoundOfAmt = 0;
                        foreach (DataRow taxRow in taxDetail.Rows)
                        {
                            if (Convert.ToString(taxRow["TaxTypeCode"]).Trim() == "IGST" || Convert.ToString(taxRow["TaxTypeCode"]).Trim() == "SGST" || Convert.ToString(taxRow["TaxTypeCode"]).Trim() == "CGST" || Convert.ToString(taxRow["TaxTypeCode"]).Trim() == "UTGST")
                            {
                                TotalPercentage = TotalPercentage + Convert.ToDecimal(taxRow["TaxRates_Rate"]);
                            }
                        }
                        decimal backcalculatePercent = 1 + (TotalPercentage / 100);

                        InclusiveAmt = recpayAmount / backcalculatePercent;

                        foreach (DataRow taxexistingRow in taxDetail.Rows)
                        {
                            TaxAmount = 0;

                            if (Convert.ToString(taxexistingRow["TaxTypeCode"]).Trim() != "O")
                            {
                                DataRow txRecordRow = taxTable.NewRow();
                                txRecordRow["SlNo"] = "0";
                                txRecordRow["TaxCode"] = taxexistingRow["Taxes_ID"];
                                txRecordRow["AltTaxCode"] = "0";
                                txRecordRow["Percentage"] = taxexistingRow["TaxRates_Rate"];

                                TaxAmount = InclusiveAmt * (Convert.ToDecimal(taxexistingRow["TaxRates_Rate"]) / 100);
                                // RoundOfAmt = RoundOfAmt + (TaxAmount - Math.Round(TaxAmount, 2));

                                //txRecordRow["Amount"] = String.Format("{0:0.##}", TaxAmount);
                                // txRecordRow["Amount"] = Math.Round(TaxAmount, 2);
                                txRecordRow["Amount"] = TaxAmount;
                                taxTable.Rows.Add(txRecordRow);

                            }
                        }



                        //if (Math.Round(RoundOfAmt, 2) > 0)
                        //{
                        //    if (roundOfPlus.Trim() != "")
                        //    {
                        //        DataRow txRecordRow = taxTable.NewRow();
                        //        txRecordRow["SlNo"] = 0;
                        //        txRecordRow["TaxCode"] = roundOfPlus;
                        //        txRecordRow["AltTaxCode"] = "0";
                        //        txRecordRow["Percentage"] = "0";
                        //        txRecordRow["Amount"] = Math.Round(RoundOfAmt, 2);
                        //        taxTable.Rows.Add(txRecordRow);
                        //    }
                        //}
                        //else if (Math.Round(RoundOfAmt, 2) < 0)
                        //{
                        //    if (roundofMinus.Trim() != "")
                        //    {
                        //        DataRow txRecordRow = taxTable.NewRow();
                        //        txRecordRow["SlNo"] = 0;
                        //        txRecordRow["TaxCode"] = roundofMinus;
                        //        txRecordRow["AltTaxCode"] = "0";
                        //        txRecordRow["Percentage"] = "0";
                        //        txRecordRow["Amount"] = Math.Round(RoundOfAmt, 2);
                        //        taxTable.Rows.Add(txRecordRow);
                        //    }
                        //}


                    }

                }
            }
            #endregion
            #region Customer Payment zone

            if (RecPay == "P")
            {
                string advanceNo = "", roundOfPlus = "", roundofMinus = "";

                DataTable fetchedData = oDBEngine.GetDataTable("select TaxRates_ID from Config_TaxRates config inner join Master_Taxes masterTax on config.TaxRates_TaxCode = masterTax.Taxes_ID  where  TaxRates_RoundingOff='R+' and Taxes_ApplicableFor in ('B','" + ApplicableFor + "')");
                if (fetchedData.Rows.Count > 0)
                    roundOfPlus = Convert.ToString(fetchedData.Rows[0][0]).Trim();

                fetchedData = oDBEngine.GetDataTable("select TaxRates_ID from Config_TaxRates config inner join Master_Taxes masterTax on config.TaxRates_TaxCode = masterTax.Taxes_ID  where  TaxRates_RoundingOff='R-' and Taxes_ApplicableFor in ('B','" + ApplicableFor + "')");
                if (fetchedData.Rows.Count > 0)
                    roundofMinus = Convert.ToString(fetchedData.Rows[0][0]).Trim();


                DataRow[] AdvanceRow = detailTable.Select("Type='AdvancePayment' and IsOpening='ADVRec'");
                if (AdvanceRow.Length > 0)
                {
                    decimal paymentAmount = 0, taxebleAmt = 0, TaxAmount = 0, RoundOfAmt = 0, TotaltaxAmount = 0;

                    foreach (DataRow paymentRow in AdvanceRow)
                    {

                        advanceNo = Convert.ToString(paymentRow["DocumentID"]);
                        paymentAmount = Convert.ToDecimal(paymentRow["Payment"]);

                        if (advanceNo.Trim() != "")
                        {
                            DataTable CustomerPayment = new DataTable();
                            ProcedureExecute proc = new ProcedureExecute("prc_GstTaxDetails");
                            proc.AddVarcharPara("@Action", 500, "GetCustomerReceiptDetail");
                            proc.AddIntegerPara("@docId", Convert.ToInt32(advanceNo));
                            CustomerPayment = proc.GetTable();

                            decimal totalPercentage = 0;
                            foreach (DataRow existingGStRow in CustomerPayment.Rows)
                            {
                                totalPercentage = totalPercentage + Convert.ToDecimal(existingGStRow["CRPTax_Percentage"]);
                            }

                            decimal backCalculate = 1 + (totalPercentage / 100);

                            taxebleAmt = Math.Round((paymentAmount / backCalculate), 2);

                            TaxAmount = 0;
                            RoundOfAmt = 0;
                            foreach (DataRow taxexistingRow in CustomerPayment.Rows)
                            {
                                DataRow txRecordRow = taxTable.NewRow();
                                txRecordRow["SlNo"] = advanceNo;
                                txRecordRow["TaxCode"] = taxexistingRow["TaxRates_ID"];
                                txRecordRow["AltTaxCode"] = "0";
                                txRecordRow["Percentage"] = taxexistingRow["CRPTax_Percentage"];

                                TaxAmount = taxebleAmt * (Convert.ToDecimal(taxexistingRow["CRPTax_Percentage"]) / 100);

                                //RoundOfAmt = RoundOfAmt + (TaxAmount - Math.Round(TaxAmount, 2));

                                txRecordRow["Amount"] = TaxAmount;
                                //txRecordRow["Amount"] = Math.Round(TaxAmount, 2);//priti

                                taxTable.Rows.Add(txRecordRow);
                            }


                            //if (Math.Round(RoundOfAmt, 2) > 0)
                            //{
                            //    if (roundOfPlus.Trim() != "")
                            //    {
                            //        DataRow txRecordRow = taxTable.NewRow();
                            //        txRecordRow["SlNo"] = advanceNo;
                            //        txRecordRow["TaxCode"] = roundOfPlus;
                            //        txRecordRow["AltTaxCode"] = "0";
                            //        txRecordRow["Percentage"] = "0";
                            //        txRecordRow["Amount"] = Math.Round(RoundOfAmt, 2);
                            //        taxTable.Rows.Add(txRecordRow);
                            //    }
                            //}
                            //else if (Math.Round(RoundOfAmt, 2) < 0)
                            //{
                            //    if (roundofMinus.Trim() != "")
                            //    {
                            //        DataRow txRecordRow = taxTable.NewRow();
                            //        txRecordRow["SlNo"] = advanceNo;
                            //        txRecordRow["TaxCode"] = roundofMinus;
                            //        txRecordRow["AltTaxCode"] = "0";
                            //        txRecordRow["Percentage"] = "0";
                            //        txRecordRow["Amount"] = Math.Round(RoundOfAmt, 2);
                            //        taxTable.Rows.Add(txRecordRow);
                            //    }
                            //}


                        }

                    }
                }
            }
            #endregion

            return taxTable;
        }
        public DataTable GetGSTTaxDataForVendorRecPay(DataTable detailTable, DataTable taxTable, string ProdList, string RecPay, string ApplicableFor, string TransactionDate, string BranchId, string VendorInternalId)
        {
            #region Vendor Payment zone

            if (RecPay == "P")
            {
                DataRow[] AdvanceRow = detailTable.Select("Type='Advance'");
                if (AdvanceRow.Length > 0)
                {
                    string[] prodarray = ProdList.Split(',');

                    if (prodarray.Length > 0)
                    {
                        decimal recpayAmount = Convert.ToDecimal(AdvanceRow[0]["payment"]);

                        string fromState = "", TaxType = "", roundOfPlus = "", roundofMinus = "";

                        //DataTable fetchedData = oDBEngine.GetDataTable("select StateCode  from tbl_master_branch br inner join tbl_master_state st on br.branch_state=st.id where branch_id=" + BranchId);
                        //if (fetchedData != null)
                        //{
                        //    fromState = Convert.ToString(fetchedData.Rows[0][0]).Trim();
                        //}

                        //Get Company Gstin 09032017
                        string CompInternalId = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
                        string[] compGstin = oDBEngine.GetFieldValue1("tbl_master_company", "cmp_gstin", "cmp_internalid='" + CompInternalId + "'", 1);


                        //Get BranchStateCode
                        string BranchStateCode = "", BranchGSTIN = "";
                        DataTable BranchTable = oDBEngine.GetDataTable("select StateCode,branch_GSTIN   from tbl_master_branch branch inner join tbl_master_state st on branch.branch_state=st.id where branch_id=" + Convert.ToString(BranchId));
                        if (BranchTable != null)
                        {
                            fromState = Convert.ToString(BranchTable.Rows[0][0]);
                            BranchGSTIN = Convert.ToString(BranchTable.Rows[0][1]);
                            if (BranchGSTIN.Trim() != "")
                            {
                                fromState = BranchGSTIN.Substring(0, 2);
                            }
                        }



                        if (BranchGSTIN.Trim() == "")
                        {
                            fromState = compGstin[0].Substring(0, 2);
                        }






                        DataTable fetchedData = oDBEngine.GetDataTable("select TaxRates_ID from Config_TaxRates config inner join Master_Taxes masterTax on config.TaxRates_TaxCode = masterTax.Taxes_ID  where  TaxRates_RoundingOff='R+' and Taxes_ApplicableFor in ('B','" + ApplicableFor + "')");
                        if (fetchedData.Rows.Count > 0)
                            roundOfPlus = Convert.ToString(fetchedData.Rows[0][0]).Trim();

                        fetchedData = oDBEngine.GetDataTable("select TaxRates_ID from Config_TaxRates config inner join Master_Taxes masterTax on config.TaxRates_TaxCode = masterTax.Taxes_ID  where  TaxRates_RoundingOff='R-' and Taxes_ApplicableFor in ('B','" + ApplicableFor + "')");
                        if (fetchedData.Rows.Count > 0)
                            roundofMinus = Convert.ToString(fetchedData.Rows[0][0]).Trim();

                        string VendorState = "";


                        ProcedureExecute GetVendorGstin = new ProcedureExecute("prc_GstTaxDetails");
                        GetVendorGstin.AddVarcharPara("@Action", 500, "GetVendorGSTINByBranch");
                        GetVendorGstin.AddVarcharPara("@branchId", 10, Convert.ToString(BranchId));
                        GetVendorGstin.AddVarcharPara("@entityId", 10, Convert.ToString(VendorInternalId));
                        DataTable VendorGstin = GetVendorGstin.GetTable();

                        if (VendorGstin.Rows.Count > 0)
                        {
                            if (Convert.ToString(VendorGstin.Rows[0][0]).Trim() != "")
                            {
                                VendorState = Convert.ToString(VendorGstin.Rows[0][0]).Substring(0, 2);
                            }

                        }


                        //
                        if (VendorState.Trim() != "")
                        {

                            #region setTaxType
                            if (fromState == VendorState)
                            {
                                TaxType = "SGST";
                                //Remove shippingStateCode == "7" as now Delhi is not under UTGST
                                if (VendorState == "4" || VendorState == "26" || VendorState == "25" || VendorState == "35" || VendorState == "31" || VendorState == "34")
                                {
                                    TaxType = "UTGST";
                                }
                            }
                            else
                            {
                                TaxType = "IGST";
                            }

                            #endregion


                            DataTable taxDetail = new DataTable();
                            ProcedureExecute proc = new ProcedureExecute("prc_GstTaxDetails");
                            proc.AddVarcharPara("@Action", 500, "LoadOtherTaxDetails");
                            proc.AddVarcharPara("@ProductID", 10, Convert.ToString(prodarray[0]));
                            proc.AddVarcharPara("@applicableFor", 5, ApplicableFor);
                            proc.AddVarcharPara("@TransDate", 10, TransactionDate);
                            taxDetail = proc.GetTable();


                            #region DeleteExtra GstCode

                            if (TaxType == "UTGST")
                            {
                                foreach (DataRow dr in taxDetail.Rows)
                                {
                                    if (Convert.ToString(dr["TaxTypeCode"]).Trim() == "IGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "SGST")
                                    {
                                        dr.Delete();
                                    }
                                }
                            }
                            else if (TaxType == "IGST")
                            {
                                foreach (DataRow dr in taxDetail.Rows)
                                {
                                    if (Convert.ToString(dr["TaxTypeCode"]).Trim() == "CGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "SGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "UTGST")
                                    {
                                        dr.Delete();
                                    }
                                }
                            }
                            else if (TaxType == "SGST")
                            {
                                foreach (DataRow dr in taxDetail.Rows)
                                {
                                    if (Convert.ToString(dr["TaxTypeCode"]).Trim() == "IGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "UTGST")
                                    {
                                        dr.Delete();
                                    }
                                }
                            }

                            taxDetail.AcceptChanges();
                            #endregion

                            decimal TotalPercentage = 0, InclusiveAmt = 0, TaxAmount = 0, RoundOfAmt = 0;
                            foreach (DataRow taxRow in taxDetail.Rows)
                            {
                                if (Convert.ToString(taxRow["TaxTypeCode"]).Trim() == "IGST" || Convert.ToString(taxRow["TaxTypeCode"]).Trim() == "SGST" || Convert.ToString(taxRow["TaxTypeCode"]).Trim() == "CGST" || Convert.ToString(taxRow["TaxTypeCode"]).Trim() == "UTGST")
                                {
                                    TotalPercentage = TotalPercentage + Convert.ToDecimal(taxRow["TaxRates_Rate"]);
                                }
                            }
                            decimal backcalculatePercent = 1 + (TotalPercentage / 100);

                            InclusiveAmt = recpayAmount / backcalculatePercent;

                            foreach (DataRow taxexistingRow in taxDetail.Rows)
                            {
                                TaxAmount = 0;

                                if (Convert.ToString(taxexistingRow["TaxTypeCode"]).Trim() != "O")
                                {
                                    DataRow txRecordRow = taxTable.NewRow();
                                    txRecordRow["SlNo"] = "0";
                                    txRecordRow["TaxCode"] = taxexistingRow["Taxes_ID"];
                                    txRecordRow["AltTaxCode"] = "0";
                                    txRecordRow["Percentage"] = taxexistingRow["TaxRates_Rate"];

                                    TaxAmount = InclusiveAmt * (Convert.ToDecimal(taxexistingRow["TaxRates_Rate"]) / 100);
                                    //RoundOfAmt = RoundOfAmt + (TaxAmount - Math.Round(TaxAmount, 2));

                                    txRecordRow["Amount"] = Math.Round(TaxAmount, 2);

                                    taxTable.Rows.Add(txRecordRow);

                                }
                            }



                            //if (Math.Round(RoundOfAmt, 2) > 0)
                            //{
                            //    if (roundOfPlus.Trim() != "")
                            //    {
                            //        DataRow txRecordRow = taxTable.NewRow();
                            //        txRecordRow["SlNo"] = 0;
                            //        txRecordRow["TaxCode"] = roundOfPlus;
                            //        txRecordRow["AltTaxCode"] = "0";
                            //        txRecordRow["Percentage"] = "0";
                            //        txRecordRow["Amount"] = Math.Round(RoundOfAmt, 2);
                            //        taxTable.Rows.Add(txRecordRow);
                            //    }
                            //}
                            //else if (Math.Round(RoundOfAmt, 2) < 0)
                            //{
                            //    if (roundofMinus.Trim() != "")
                            //    {
                            //        DataRow txRecordRow = taxTable.NewRow();
                            //        txRecordRow["SlNo"] = 0;
                            //        txRecordRow["TaxCode"] = roundofMinus;
                            //        txRecordRow["AltTaxCode"] = "0";
                            //        txRecordRow["Percentage"] = "0";
                            //        txRecordRow["Amount"] = Math.Round(RoundOfAmt, 2);
                            //        taxTable.Rows.Add(txRecordRow);
                            //    }
                            //}

                        }
                    }

                }
            }
            #endregion
            #region Vendor Receipt zone

            if (RecPay == "R")
            {
                string advanceNo = "", roundOfPlus = "", roundofMinus = "";

                DataTable fetchedData = oDBEngine.GetDataTable("select TaxRates_ID from Config_TaxRates config inner join Master_Taxes masterTax on config.TaxRates_TaxCode = masterTax.Taxes_ID  where  TaxRates_RoundingOff='R+' and Taxes_ApplicableFor in ('B','" + ApplicableFor + "')");
                if (fetchedData.Rows.Count > 0)
                    roundOfPlus = Convert.ToString(fetchedData.Rows[0][0]).Trim();

                fetchedData = oDBEngine.GetDataTable("select TaxRates_ID from Config_TaxRates config inner join Master_Taxes masterTax on config.TaxRates_TaxCode = masterTax.Taxes_ID  where  TaxRates_RoundingOff='R-' and Taxes_ApplicableFor in ('B','" + ApplicableFor + "')");
                if (fetchedData.Rows.Count > 0)
                    roundofMinus = Convert.ToString(fetchedData.Rows[0][0]).Trim();


                DataRow[] AdvanceRow = detailTable.Select("Type='AdvanceReceipt' and IsOpening='AVR'");
                if (AdvanceRow.Length > 0)
                {
                    decimal paymentAmount = 0, taxebleAmt = 0, TaxAmount = 0, RoundOfAmt = 0, TotaltaxAmount = 0;

                    foreach (DataRow paymentRow in AdvanceRow)
                    {

                        advanceNo = Convert.ToString(paymentRow["DocumentID"]);
                        paymentAmount = Convert.ToDecimal(paymentRow["Receipt"]);

                        if (advanceNo.Trim() != "")
                        {
                            DataTable CustomerPayment = new DataTable();
                            ProcedureExecute proc = new ProcedureExecute("prc_GstTaxDetails");
                            proc.AddVarcharPara("@Action", 500, "GetVendorReceiptDetail");
                            proc.AddIntegerPara("@docId", Convert.ToInt32(advanceNo));
                            CustomerPayment = proc.GetTable();

                            decimal totalPercentage = 0;
                            foreach (DataRow existingGStRow in CustomerPayment.Rows)
                            {
                                totalPercentage = totalPercentage + Convert.ToDecimal(existingGStRow["VRPTax_Percentage"]);
                            }

                            decimal backCalculate = 1 + (totalPercentage / 100);

                            taxebleAmt = Math.Round((paymentAmount / backCalculate));

                            TaxAmount = 0;
                            RoundOfAmt = 0;
                            foreach (DataRow taxexistingRow in CustomerPayment.Rows)
                            {
                                DataRow txRecordRow = taxTable.NewRow();
                                txRecordRow["SlNo"] = advanceNo;
                                txRecordRow["TaxCode"] = taxexistingRow["TaxRates_ID"];
                                txRecordRow["AltTaxCode"] = "0";
                                txRecordRow["Percentage"] = taxexistingRow["VRPTax_Percentage"];

                                TaxAmount = taxebleAmt * (Convert.ToDecimal(taxexistingRow["VRPTax_Percentage"]) / 100);

                                //RoundOfAmt = RoundOfAmt + (TaxAmount - Math.Round(TaxAmount, 2));

                                txRecordRow["Amount"] = Math.Round(TaxAmount, 2);

                                taxTable.Rows.Add(txRecordRow);
                            }


                            //if (Math.Round(RoundOfAmt, 2) > 0)
                            //{
                            //    if (roundOfPlus.Trim() != "")
                            //    {
                            //        DataRow txRecordRow = taxTable.NewRow();
                            //        txRecordRow["SlNo"] = advanceNo;
                            //        txRecordRow["TaxCode"] = roundOfPlus;
                            //        txRecordRow["AltTaxCode"] = "0";
                            //        txRecordRow["Percentage"] = "0";
                            //        txRecordRow["Amount"] = Math.Round(RoundOfAmt, 2);
                            //        taxTable.Rows.Add(txRecordRow);
                            //    }
                            //}
                            //else if (Math.Round(RoundOfAmt, 2) < 0)
                            //{
                            //    if (roundofMinus.Trim() != "")
                            //    {
                            //        DataRow txRecordRow = taxTable.NewRow();
                            //        txRecordRow["SlNo"] = advanceNo;
                            //        txRecordRow["TaxCode"] = roundofMinus;
                            //        txRecordRow["AltTaxCode"] = "0";
                            //        txRecordRow["Percentage"] = "0";
                            //        txRecordRow["Amount"] = Math.Round(RoundOfAmt, 2);
                            //        taxTable.Rows.Add(txRecordRow);
                            //    }
                            //}


                        }

                    }
                }
            }
            #endregion
            return taxTable;
        }


        public DataTable SetTaxTableDataWithProductSerialForPurchase(DataTable productDetails, string SerialColumnName, string ProductColumnName, string AmountColumnName, string chargesColumns, DataTable taxTable, string ApplicableFor, string TransactionDate, string BranchId, string ShippingState, string inclusiveOrExclusive, string VendorInternalID)
        {
            string fromState = "", shippingStateCode = "", TaxType = "", roundOfPlus = "", roundofMinus = "";

            ProcedureExecute procstateTable = new ProcedureExecute("Prc_taxForpurchase");
            procstateTable.AddVarcharPara("@action", 500, "GetGSTINByBranch");
            procstateTable.AddIntegerPara("@BranchId", Convert.ToInt32(BranchId));
            procstateTable.AddVarcharPara("@companyintId", 50, Convert.ToString(HttpContext.Current.Session["LastCompany"]));
            procstateTable.AddVarcharPara("@vendInternalId", 20, VendorInternalID);
            DataSet taxForpurchase = procstateTable.GetDataSet();



            //  DataTable fetchedData = oDBEngine.GetDataTable("select StateCode  from tbl_master_branch br inner join tbl_master_state st on br.branch_state=st.id where branch_id=" + BranchId);
            if (taxForpurchase != null)
            {
                fromState = Convert.ToString(taxForpurchase.Tables[0].Rows[0][0]).Trim();
                if (fromState.Trim() != "")
                {
                    fromState = fromState.Substring(0, 2);
                }

                shippingStateCode = Convert.ToString(taxForpurchase.Tables[1].Rows[0][0]).Trim();
                if (shippingStateCode.Trim() != "")
                {
                    shippingStateCode = shippingStateCode.Substring(0, 2);
                }
            }
            //fetchedData = oDBEngine.GetDataTable("select StateCode from tbl_master_state where id=" + ShippingState);
            //if (fetchedData != null)


            DataTable fetchedData = oDBEngine.GetDataTable("select TaxRates_ID from Config_TaxRates config inner join Master_Taxes masterTax on config.TaxRates_TaxCode = masterTax.Taxes_ID  where  TaxRates_RoundingOff='R+' and Taxes_ApplicableFor in ('B','" + ApplicableFor + "')");
            if (fetchedData.Rows.Count > 0)
                roundOfPlus = Convert.ToString(fetchedData.Rows[0][0]).Trim();

            fetchedData = oDBEngine.GetDataTable("select TaxRates_ID from Config_TaxRates config inner join Master_Taxes masterTax on config.TaxRates_TaxCode = masterTax.Taxes_ID  where  TaxRates_RoundingOff='R-' and Taxes_ApplicableFor in ('B','" + ApplicableFor + "')");
            if (fetchedData.Rows.Count > 0)
                roundofMinus = Convert.ToString(fetchedData.Rows[0][0]).Trim();




            #region setTaxType
            if (fromState == shippingStateCode)
            {
                TaxType = "SGST";
                if (shippingStateCode == "4" || shippingStateCode == "26" || shippingStateCode == "25" || shippingStateCode == "35" || shippingStateCode == "31" || shippingStateCode == "34")
                {
                    TaxType = "UTGST";
                }
            }
            else
            {
                TaxType = "IGST";
            }

            #endregion






            foreach (DataRow productRow in productDetails.Rows)
            {
                if (taxTable.Select("SlNo='" + productRow[SerialColumnName] + "'").Length == 0)
                {

                    DataTable taxDetail = new DataTable();
                    ProcedureExecute proc = new ProcedureExecute("prc_GstTaxDetails");
                    proc.AddVarcharPara("@Action", 500, "LoadOtherTaxDetails");
                    proc.AddVarcharPara("@ProductID", 10, Convert.ToString(productRow[ProductColumnName]));
                    proc.AddVarcharPara("@applicableFor", 5, ApplicableFor);
                    proc.AddVarcharPara("@TransDate", 10, TransactionDate);
                    taxDetail = proc.GetTable();


                    #region DeleteExtra GstCode

                    if (TaxType == "UTGST")
                    {
                        foreach (DataRow dr in taxDetail.Rows)
                        {
                            if (Convert.ToString(dr["TaxTypeCode"]).Trim() == "IGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "SGST")
                            {
                                dr.Delete();
                            }
                        }
                    }
                    else if (TaxType == "IGST")
                    {
                        foreach (DataRow dr in taxDetail.Rows)
                        {
                            if (Convert.ToString(dr["TaxTypeCode"]).Trim() == "CGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "SGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "UTGST")
                            {
                                dr.Delete();
                            }
                        }
                    }
                    else if (TaxType == "SGST")
                    {
                        foreach (DataRow dr in taxDetail.Rows)
                        {
                            if (Convert.ToString(dr["TaxTypeCode"]).Trim() == "IGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "UTGST")
                            {
                                dr.Delete();
                            }
                        }
                    }
                    taxDetail.AcceptChanges();
                    #endregion

                    //#region CreateTempTaxTable
                    //DataTable TaxRecord = new DataTable();

                    //TaxRecord.Columns.Add("SlNo", typeof(System.Int32));
                    //TaxRecord.Columns.Add("TaxCode", typeof(System.String));
                    //TaxRecord.Columns.Add("AltTaxCode", typeof(System.String));
                    //TaxRecord.Columns.Add("Percentage", typeof(System.Decimal));
                    //TaxRecord.Columns.Add("Amount", typeof(System.Decimal)); 

                    //#endregion






                    //decimal totalParcentage = 0;
                    //foreach (DataRow dr in taxDetail.Rows)
                    //{
                    //    if (Convert.ToString(dr["TaxTypeCode"]).Trim() == "CGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "SGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "IGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "UTGST")
                    //    {
                    //        totalParcentage += Convert.ToDecimal(dr["TaxRates_Rate"]);
                    //    }
                    //}


                    if (Convert.ToDecimal(productRow[chargesColumns]) != 0)
                    {
                        foreach (DataRow taxexistingRow in taxDetail.Rows)
                        {
                            if (Convert.ToString(taxexistingRow["TaxTypeCode"]).Trim() != "O")
                            {
                                DataRow txRecordRow = taxTable.NewRow();
                                txRecordRow["SlNo"] = productRow[SerialColumnName];
                                txRecordRow["TaxCode"] = taxexistingRow["Taxes_ID"];
                                txRecordRow["AltTaxCode"] = "0";
                                txRecordRow["Percentage"] = taxexistingRow["TaxRates_Rate"];
                                txRecordRow["Amount"] = Convert.ToDecimal(productRow[AmountColumnName]) * (Convert.ToDecimal(taxexistingRow["TaxRates_Rate"]) / 100);
                                taxTable.Rows.Add(txRecordRow);

                            }
                        }
                    }
                }


            }



            #region setRound of for GST
            DataTable GSTTABLE = oDBEngine.GetDataTable("select TaxRates_ID,TaxTypeCode from Config_TaxRates config inner join Master_Taxes mast on config.TaxRates_TaxCode =mast.Taxes_ID where TaxTypeCode in ('IGST','CGST','SGST','UTGST')");

            foreach (DataRow gstrow in taxTable.Rows)
            {
                if (GSTTABLE.Select("TaxRates_ID=" + gstrow["TaxCode"]).Length > 0)
                {
                    gstrow["Amount"] = Math.Round(Convert.ToDecimal(gstrow["Amount"]));
                }
            }

            #endregion





            foreach (DataRow productRow in productDetails.Rows)
            {
                #region SetRounded of ledger


                if (taxTable.Select("SlNo='" + productRow[SerialColumnName] + "'").Length > 0)
                {
                    DataTable filteredTable = taxTable.Select("SlNo='" + productRow[SerialColumnName] + "'").CopyToDataTable();
                    decimal totalTaxAmount = Convert.ToDecimal(filteredTable.Compute("SUM(Amount)", string.Empty));

                    if (totalTaxAmount != Convert.ToDecimal(productRow[chargesColumns]))
                    {
                        decimal roundOf = Convert.ToDecimal(productRow[chargesColumns]) - totalTaxAmount;
                        if (roundOf > 0)
                        {
                            if (roundOfPlus.Trim() != "")
                            {
                                DataRow txRecordRow = taxTable.NewRow();
                                txRecordRow["SlNo"] = productRow[SerialColumnName];
                                txRecordRow["TaxCode"] = roundOfPlus;
                                txRecordRow["AltTaxCode"] = "0";
                                txRecordRow["Percentage"] = "0";
                                txRecordRow["Amount"] = roundOf;
                                taxTable.Rows.Add(txRecordRow);
                            }
                        }
                        else if (roundOf < 0)
                        {
                            if (roundofMinus.Trim() != "")
                            {
                                DataRow txRecordRow = taxTable.NewRow();
                                txRecordRow["SlNo"] = productRow[SerialColumnName];
                                txRecordRow["TaxCode"] = roundofMinus;
                                txRecordRow["AltTaxCode"] = "0";
                                txRecordRow["Percentage"] = "0";
                                txRecordRow["Amount"] = roundOf;
                                taxTable.Rows.Add(txRecordRow);
                            }
                        }
                    }
                }



                #endregion
            }

            return taxTable;
        }

        public DataTable SetTaxTableDataWithProductSerialForPurchaseRoundOff(ref DataTable productDetails, string SerialColumnName, string ProductColumnName, string AmountColumnName, string chargesColumns, string NetAmountCol, DataTable taxTable, string ApplicableFor, string TransactionDate, string BranchId, string ShippingState, string inclusiveOrExclusive, string VendorInternalID)
        {
            string fromState = "", shippingStateCode = "", TaxType = "", roundOfPlus = "", roundofMinus = "";

            ProcedureExecute procstateTable = new ProcedureExecute("Prc_taxForpurchase");
            procstateTable.AddVarcharPara("@action", 500, "GetGSTINByBranch");
            procstateTable.AddIntegerPara("@BranchId", Convert.ToInt32(BranchId));
            procstateTable.AddVarcharPara("@companyintId", 50, Convert.ToString(HttpContext.Current.Session["LastCompany"]));
            procstateTable.AddVarcharPara("@vendInternalId", 20, VendorInternalID);
            DataSet taxForpurchase = procstateTable.GetDataSet();



            //  DataTable fetchedData = oDBEngine.GetDataTable("select StateCode  from tbl_master_branch br inner join tbl_master_state st on br.branch_state=st.id where branch_id=" + BranchId);
            if (taxForpurchase != null)
            {
                fromState = Convert.ToString(taxForpurchase.Tables[0].Rows[0][0]).Trim();
                if (fromState.Trim() != "")
                {
                    fromState = fromState.Substring(0, 2);
                }

                shippingStateCode = Convert.ToString(taxForpurchase.Tables[1].Rows[0][0]).Trim();
                if (shippingStateCode.Trim() != "")
                {
                    shippingStateCode = shippingStateCode.Substring(0, 2);
                }
            }
            //fetchedData = oDBEngine.GetDataTable("select StateCode from tbl_master_state where id=" + ShippingState);
            //if (fetchedData != null)


            DataTable fetchedData = oDBEngine.GetDataTable("select TaxRates_ID from Config_TaxRates config inner join Master_Taxes masterTax on config.TaxRates_TaxCode = masterTax.Taxes_ID  where  TaxRates_RoundingOff='R+' and Taxes_ApplicableFor in ('B','" + ApplicableFor + "')");
            if (fetchedData.Rows.Count > 0)
                roundOfPlus = Convert.ToString(fetchedData.Rows[0][0]).Trim();

            fetchedData = oDBEngine.GetDataTable("select TaxRates_ID from Config_TaxRates config inner join Master_Taxes masterTax on config.TaxRates_TaxCode = masterTax.Taxes_ID  where  TaxRates_RoundingOff='R-' and Taxes_ApplicableFor in ('B','" + ApplicableFor + "')");
            if (fetchedData.Rows.Count > 0)
                roundofMinus = Convert.ToString(fetchedData.Rows[0][0]).Trim();




            #region setTaxType
            if (fromState == shippingStateCode)
            {
                TaxType = "SGST";
                if (shippingStateCode == "4" || shippingStateCode == "26" || shippingStateCode == "25" || shippingStateCode == "35" || shippingStateCode == "31" || shippingStateCode == "34")
                {
                    TaxType = "UTGST";
                }
            }
            else
            {
                TaxType = "IGST";
            }

            #endregion






            foreach (DataRow productRow in productDetails.Rows)
            {
                if (taxTable.Select("SlNo='" + productRow[SerialColumnName] + "'").Length == 0)
                {

                    DataTable taxDetail = new DataTable();
                    ProcedureExecute proc = new ProcedureExecute("prc_GstTaxDetails");
                    proc.AddVarcharPara("@Action", 500, "LoadOtherTaxDetails");
                    proc.AddVarcharPara("@ProductID", 10, Convert.ToString(productRow[ProductColumnName]));
                    proc.AddVarcharPara("@applicableFor", 5, ApplicableFor);
                    proc.AddVarcharPara("@TransDate", 10, TransactionDate);
                    taxDetail = proc.GetTable();


                    #region DeleteExtra GstCode

                    if (TaxType == "UTGST")
                    {
                        foreach (DataRow dr in taxDetail.Rows)
                        {
                            if (Convert.ToString(dr["TaxTypeCode"]).Trim() == "IGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "SGST")
                            {
                                dr.Delete();
                            }
                        }
                    }
                    else if (TaxType == "IGST")
                    {
                        foreach (DataRow dr in taxDetail.Rows)
                        {
                            if (Convert.ToString(dr["TaxTypeCode"]).Trim() == "CGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "SGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "UTGST")
                            {
                                dr.Delete();
                            }
                        }
                    }
                    else if (TaxType == "SGST")
                    {
                        foreach (DataRow dr in taxDetail.Rows)
                        {
                            if (Convert.ToString(dr["TaxTypeCode"]).Trim() == "IGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "UTGST")
                            {
                                dr.Delete();
                            }
                        }
                    }
                    taxDetail.AcceptChanges();
                    #endregion

                    //#region CreateTempTaxTable
                    //DataTable TaxRecord = new DataTable();

                    //TaxRecord.Columns.Add("SlNo", typeof(System.Int32));
                    //TaxRecord.Columns.Add("TaxCode", typeof(System.String));
                    //TaxRecord.Columns.Add("AltTaxCode", typeof(System.String));
                    //TaxRecord.Columns.Add("Percentage", typeof(System.Decimal));
                    //TaxRecord.Columns.Add("Amount", typeof(System.Decimal)); 

                    //#endregion






                    //decimal totalParcentage = 0;
                    //foreach (DataRow dr in taxDetail.Rows)
                    //{
                    //    if (Convert.ToString(dr["TaxTypeCode"]).Trim() == "CGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "SGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "IGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "UTGST")
                    //    {
                    //        totalParcentage += Convert.ToDecimal(dr["TaxRates_Rate"]);
                    //    }
                    //}


                    if (Convert.ToDecimal(productRow[chargesColumns]) != 0)
                    {
                        foreach (DataRow taxexistingRow in taxDetail.Rows)
                        {
                            if (Convert.ToString(taxexistingRow["TaxTypeCode"]).Trim() != "O")
                            {
                                DataRow txRecordRow = taxTable.NewRow();
                                txRecordRow["SlNo"] = productRow[SerialColumnName];
                                txRecordRow["TaxCode"] = taxexistingRow["Taxes_ID"];
                                txRecordRow["AltTaxCode"] = "0";
                                txRecordRow["Percentage"] = taxexistingRow["TaxRates_Rate"];
                                txRecordRow["Amount"] = Math.Round(Convert.ToDecimal(productRow[AmountColumnName]) * (Convert.ToDecimal(taxexistingRow["TaxRates_Rate"]) / 100), 2);
                                taxTable.Rows.Add(txRecordRow);

                            }
                        }
                    }
                }


            }



            #region setRound of for GST
            //DataTable GSTTABLE = oDBEngine.GetDataTable("select TaxRates_ID,TaxTypeCode from Config_TaxRates config inner join Master_Taxes mast on config.TaxRates_TaxCode =mast.Taxes_ID where TaxTypeCode in ('IGST','CGST','SGST','UTGST')");

            //foreach (DataRow gstrow in taxTable.Rows)
            //{
            //    if (GSTTABLE.Select("TaxRates_ID=" + gstrow["TaxCode"]).Length > 0)
            //    {
            //        gstrow["Amount"] = Math.Round(Convert.ToDecimal(gstrow["Amount"]));
            //    }
            //}

            #endregion





            foreach (DataRow productRow in productDetails.Rows)
            {
                #region SetRounded of ledger


                if (taxTable.Select("SlNo='" + productRow[SerialColumnName] + "'").Length > 0)
                {
                    DataTable filteredTable = taxTable.Select("SlNo='" + productRow[SerialColumnName] + "'").CopyToDataTable();
                    decimal totalTaxAmount = 0;//Convert.ToDecimal(filteredTable.Compute("SUM(Amount)", string.Empty));

                    foreach (DataRow taxCalRow in filteredTable.Rows)
                    {
                        if (Convert.ToDecimal(taxCalRow["Amount"]) > 0)
                        {
                            DataRow returnRow = oDBEngine.GetDataTable("select TaxCalculateMethods  from Config_TaxRates config inner join Master_Taxes tax on config.TaxRates_TaxCode  = tax.Taxes_ID where TaxRates_ID='" + Convert.ToString(taxCalRow["TaxCode"]) + "'").Rows[0];

                            if (Convert.ToString(returnRow["TaxCalculateMethods"]) == "A")
                                totalTaxAmount = totalTaxAmount + Convert.ToDecimal(taxCalRow["Amount"]);
                            else
                                totalTaxAmount = totalTaxAmount - Convert.ToDecimal(taxCalRow["Amount"]);
                        }
                    }


                    if (totalTaxAmount != Convert.ToDecimal(productRow[chargesColumns]))
                    {
                        decimal roundOf = Convert.ToDecimal(productRow[chargesColumns]) - totalTaxAmount;
                        productRow[chargesColumns] = Convert.ToDecimal(productRow[chargesColumns]) - roundOf;
                        if (inclusiveOrExclusive == "I")
                            productRow[AmountColumnName] = Convert.ToDecimal(productRow[AmountColumnName]) + roundOf;
                        else if (inclusiveOrExclusive == "E")
                        {
                            productRow[NetAmountCol] = Convert.ToDecimal(productRow[NetAmountCol]) - roundOf;
                        }

                    }
                }



                #endregion
            }

            return taxTable;
        }

        public DataTable SetTaxTableDataWithProductSerialForPurchaseRoundOffWithException(ref DataTable productDetails, string SerialColumnName, string ProductColumnName, string AmountColumnName, string chargesColumns, string NetAmountCol, DataTable taxTable, string ApplicableFor, string TransactionDate, string BranchId, string ShippingState, string inclusiveOrExclusive, string VendorInternalID, String quantitycolumn, string modulename)
        {
            string fromState = "", shippingStateCode = "", TaxType = "", roundOfPlus = "", roundofMinus = "";

            ProcedureExecute procstateTable = new ProcedureExecute("Prc_taxForpurchase");
            procstateTable.AddVarcharPara("@action", 500, "GetGSTINByBranch");
            procstateTable.AddIntegerPara("@BranchId", Convert.ToInt32(BranchId));
            procstateTable.AddVarcharPara("@companyintId", 50, Convert.ToString(HttpContext.Current.Session["LastCompany"]));
            procstateTable.AddVarcharPara("@vendInternalId", 20, VendorInternalID);
            DataSet taxForpurchase = procstateTable.GetDataSet();


          


            //  DataTable fetchedData = oDBEngine.GetDataTable("select StateCode  from tbl_master_branch br inner join tbl_master_state st on br.branch_state=st.id where branch_id=" + BranchId);
            if (taxForpurchase != null)
            {
                fromState = Convert.ToString(taxForpurchase.Tables[0].Rows[0][0]).Trim();
                if (fromState.Trim() != "")
                {
                    fromState = fromState.Substring(0, 2);
                }

                shippingStateCode = Convert.ToString(taxForpurchase.Tables[1].Rows[0][0]).Trim();
                if (shippingStateCode.Trim() != "")
                {
                    shippingStateCode = shippingStateCode.Substring(0, 2);
                }
            }
            //fetchedData = oDBEngine.GetDataTable("select StateCode from tbl_master_state where id=" + ShippingState);
            //if (fetchedData != null)


            DataTable fetchedData = oDBEngine.GetDataTable("select TaxRates_ID from Config_TaxRates config inner join Master_Taxes masterTax on config.TaxRates_TaxCode = masterTax.Taxes_ID  where  TaxRates_RoundingOff='R+' and Taxes_ApplicableFor in ('B','" + ApplicableFor + "')");
            if (fetchedData.Rows.Count > 0)
                roundOfPlus = Convert.ToString(fetchedData.Rows[0][0]).Trim();

            fetchedData = oDBEngine.GetDataTable("select TaxRates_ID from Config_TaxRates config inner join Master_Taxes masterTax on config.TaxRates_TaxCode = masterTax.Taxes_ID  where  TaxRates_RoundingOff='R-' and Taxes_ApplicableFor in ('B','" + ApplicableFor + "')");
            if (fetchedData.Rows.Count > 0)
                roundofMinus = Convert.ToString(fetchedData.Rows[0][0]).Trim();




            #region setTaxType
            if (fromState == shippingStateCode)
            {
                TaxType = "SGST";
                if (shippingStateCode == "4" || shippingStateCode == "26" || shippingStateCode == "25" || shippingStateCode == "35" || shippingStateCode == "31" || shippingStateCode == "34")
                {
                    TaxType = "UTGST";
                }
            }
            else
            {
                TaxType = "IGST";
            }

            #endregion






            foreach (DataRow productRow in productDetails.Rows)
            {
                if (taxTable.Select("SlNo='" + productRow[SerialColumnName] + "'").Length == 0)
                {

                    DataTable taxDetail = new DataTable();
                    //ProcedureExecute proc = new ProcedureExecute("prc_GstTaxDetails");
                    //proc.AddVarcharPara("@Action", 500, "LoadOtherTaxDetails");
                    //proc.AddVarcharPara("@ProductID", 10, Convert.ToString(productRow[ProductColumnName]));
                    //proc.AddVarcharPara("@applicableFor", 5, ApplicableFor);
                    //proc.AddVarcharPara("@TransDate", 10, TransactionDate);
                    //taxDetail = proc.GetTable();

                    ProcedureExecute proc = new ProcedureExecute("prc_TaxExceptionFind");
                    proc.AddVarcharPara("@Action", 500, modulename);
                    proc.AddVarcharPara("@ProductID", 10, Convert.ToString(productRow[ProductColumnName]));
                    proc.AddPara("@ENTITY_ID", VendorInternalID);
                    proc.AddVarcharPara("@Date", 10, TransactionDate);
                    proc.AddVarcharPara("@Amount", 100, Convert.ToString(productRow[AmountColumnName]));
                    proc.AddVarcharPara("@Qty", 100, Convert.ToString(productRow[quantitycolumn]));
                    taxDetail = proc.GetTable();




                    #region DeleteExtra GstCode

                    if (TaxType == "UTGST")
                    {
                        foreach (DataRow dr in taxDetail.Rows)
                        {
                            if (Convert.ToString(dr["TaxTypeCode"]).Trim() == "IGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "SGST")
                            {
                                dr.Delete();
                            }
                        }
                    }
                    else if (TaxType == "IGST")
                    {
                        foreach (DataRow dr in taxDetail.Rows)
                        {
                            if (Convert.ToString(dr["TaxTypeCode"]).Trim() == "CGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "SGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "UTGST")
                            {
                                dr.Delete();
                            }
                        }
                    }
                    else if (TaxType == "SGST")
                    {
                        foreach (DataRow dr in taxDetail.Rows)
                        {
                            if (Convert.ToString(dr["TaxTypeCode"]).Trim() == "IGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "UTGST")
                            {
                                dr.Delete();
                            }
                        }
                    }
                    taxDetail.AcceptChanges();
                    #endregion

                    //#region CreateTempTaxTable
                    //DataTable TaxRecord = new DataTable();

                    //TaxRecord.Columns.Add("SlNo", typeof(System.Int32));
                    //TaxRecord.Columns.Add("TaxCode", typeof(System.String));
                    //TaxRecord.Columns.Add("AltTaxCode", typeof(System.String));
                    //TaxRecord.Columns.Add("Percentage", typeof(System.Decimal));
                    //TaxRecord.Columns.Add("Amount", typeof(System.Decimal)); 

                    //#endregion






                    //decimal totalParcentage = 0;
                    //foreach (DataRow dr in taxDetail.Rows)
                    //{
                    //    if (Convert.ToString(dr["TaxTypeCode"]).Trim() == "CGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "SGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "IGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "UTGST")
                    //    {
                    //        totalParcentage += Convert.ToDecimal(dr["TaxRates_Rate"]);
                    //    }
                    //}


                    if (Convert.ToDecimal(productRow[chargesColumns]) != 0)
                    {
                        foreach (DataRow taxexistingRow in taxDetail.Rows)
                        {
                            if (Convert.ToString(taxexistingRow["TaxTypeCode"]).Trim() != "O")
                            {
                                DataRow txRecordRow = taxTable.NewRow();
                                txRecordRow["SlNo"] = productRow[SerialColumnName];
                                txRecordRow["TaxCode"] = taxexistingRow["Taxes_ID"];
                                txRecordRow["AltTaxCode"] = "0";
                                txRecordRow["Percentage"] = taxexistingRow["TaxRates_Rate"];
                                txRecordRow["Amount"] = Math.Round(Convert.ToDecimal(productRow[AmountColumnName]) * (Convert.ToDecimal(taxexistingRow["TaxRates_Rate"]) / 100), 2);
                                taxTable.Rows.Add(txRecordRow);

                            }
                        }
                    }
                }


            }



            #region setRound of for GST
            //DataTable GSTTABLE = oDBEngine.GetDataTable("select TaxRates_ID,TaxTypeCode from Config_TaxRates config inner join Master_Taxes mast on config.TaxRates_TaxCode =mast.Taxes_ID where TaxTypeCode in ('IGST','CGST','SGST','UTGST')");

            //foreach (DataRow gstrow in taxTable.Rows)
            //{
            //    if (GSTTABLE.Select("TaxRates_ID=" + gstrow["TaxCode"]).Length > 0)
            //    {
            //        gstrow["Amount"] = Math.Round(Convert.ToDecimal(gstrow["Amount"]));
            //    }
            //}

            #endregion





            foreach (DataRow productRow in productDetails.Rows)
            {
                #region SetRounded of ledger


                if (taxTable.Select("SlNo='" + productRow[SerialColumnName] + "'").Length > 0)
                {
                    DataTable filteredTable = taxTable.Select("SlNo='" + productRow[SerialColumnName] + "'").CopyToDataTable();
                    decimal totalTaxAmount = 0;//Convert.ToDecimal(filteredTable.Compute("SUM(Amount)", string.Empty));

                    foreach (DataRow taxCalRow in filteredTable.Rows)
                    {
                        if (Convert.ToDecimal(taxCalRow["Amount"]) > 0)
                        {
                            DataRow returnRow = oDBEngine.GetDataTable("select TaxCalculateMethods  from Config_TaxRates config inner join Master_Taxes tax on config.TaxRates_TaxCode  = tax.Taxes_ID where TaxRates_ID='" + Convert.ToString(taxCalRow["TaxCode"]) + "'").Rows[0];

                            if (Convert.ToString(returnRow["TaxCalculateMethods"]) == "A")
                                totalTaxAmount = totalTaxAmount + Convert.ToDecimal(taxCalRow["Amount"]);
                            else
                                totalTaxAmount = totalTaxAmount - Convert.ToDecimal(taxCalRow["Amount"]);
                        }
                    }


                    if (totalTaxAmount != Convert.ToDecimal(productRow[chargesColumns]))
                    {
                        decimal roundOf = Convert.ToDecimal(productRow[chargesColumns]) - totalTaxAmount;
                        productRow[chargesColumns] = Convert.ToDecimal(productRow[chargesColumns]) - roundOf;
                        if (inclusiveOrExclusive == "I")
                            productRow[AmountColumnName] = Convert.ToDecimal(productRow[AmountColumnName]) + roundOf;
                        else if (inclusiveOrExclusive == "E")
                        {
                            productRow[NetAmountCol] = Convert.ToDecimal(productRow[NetAmountCol]) - roundOf;
                        }

                    }
                }



                #endregion
            }

            return taxTable;
        }


        public DataTable SetTaxTableDataMainAccount(ref DataTable productDetails, string SerialColumnName, string HSNColumnName, string AmountColumnName, string chargesColumns, string NetAmountCol, DataTable taxTable, string ApplicableFor, string TransactionDate, string BranchId, string ShippingState, string inclusiveOrExclusive, string VendorInternalID)
        {
            string fromState = "", shippingStateCode = "", TaxType = "", roundOfPlus = "", roundofMinus = "";

            ProcedureExecute procstateTable = new ProcedureExecute("Prc_taxForpurchase");
            procstateTable.AddVarcharPara("@action", 500, "GetGSTINByBranch");
            procstateTable.AddIntegerPara("@BranchId", Convert.ToInt32(BranchId));
            procstateTable.AddVarcharPara("@companyintId", 50, Convert.ToString(HttpContext.Current.Session["LastCompany"]));
            procstateTable.AddVarcharPara("@vendInternalId", 20, VendorInternalID);
            DataSet taxForpurchase = procstateTable.GetDataSet();



            //  DataTable fetchedData = oDBEngine.GetDataTable("select StateCode  from tbl_master_branch br inner join tbl_master_state st on br.branch_state=st.id where branch_id=" + BranchId);
            if (taxForpurchase != null)
            {
                fromState = Convert.ToString(taxForpurchase.Tables[0].Rows[0][0]).Trim();
                if (fromState.Trim() != "")
                {
                    fromState = fromState.Substring(0, 2);
                }

                shippingStateCode = Convert.ToString(taxForpurchase.Tables[1].Rows[0][0]).Trim();
                if (shippingStateCode.Trim() != "")
                {
                    shippingStateCode = shippingStateCode.Substring(0, 2);
                }
            }
            //fetchedData = oDBEngine.GetDataTable("select StateCode from tbl_master_state where id=" + ShippingState);
            //if (fetchedData != null)


            DataTable fetchedData = oDBEngine.GetDataTable("select TaxRates_ID from Config_TaxRates config inner join Master_Taxes masterTax on config.TaxRates_TaxCode = masterTax.Taxes_ID  where  TaxRates_RoundingOff='R+' and Taxes_ApplicableFor in ('B','" + ApplicableFor + "')");
            if (fetchedData.Rows.Count > 0)
                roundOfPlus = Convert.ToString(fetchedData.Rows[0][0]).Trim();

            fetchedData = oDBEngine.GetDataTable("select TaxRates_ID from Config_TaxRates config inner join Master_Taxes masterTax on config.TaxRates_TaxCode = masterTax.Taxes_ID  where  TaxRates_RoundingOff='R-' and Taxes_ApplicableFor in ('B','" + ApplicableFor + "')");
            if (fetchedData.Rows.Count > 0)
                roundofMinus = Convert.ToString(fetchedData.Rows[0][0]).Trim();




            #region setTaxType
            if (fromState == shippingStateCode)
            {
                TaxType = "SGST";
                if (shippingStateCode == "4" || shippingStateCode == "26" || shippingStateCode == "25" || shippingStateCode == "35" || shippingStateCode == "31" || shippingStateCode == "34")
                {
                    TaxType = "UTGST";
                }
            }
            else
            {
                TaxType = "IGST";
            }

            #endregion






            foreach (DataRow productRow in productDetails.Rows)
            {
                if (taxTable.Select("SlNo='" + productRow[SerialColumnName] + "'").Length == 0)
                {

                    DataTable taxDetail = new DataTable();
                    ProcedureExecute proc = new ProcedureExecute("prc_GstTaxDetails");
                    proc.AddVarcharPara("@Action", 500, "LoadMainAccountTaxDetails");
                    proc.AddVarcharPara("@ProductID", 10, Convert.ToString(productRow[HSNColumnName]));
                    proc.AddVarcharPara("@applicableFor", 5, ApplicableFor);
                    proc.AddVarcharPara("@TransDate", 10, TransactionDate);
                    taxDetail = proc.GetTable();


                    #region DeleteExtra GstCode

                    if (TaxType == "UTGST")
                    {
                        foreach (DataRow dr in taxDetail.Rows)
                        {
                            if (Convert.ToString(dr["TaxTypeCode"]).Trim() == "IGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "SGST")
                            {
                                dr.Delete();
                            }
                        }
                    }
                    else if (TaxType == "IGST")
                    {
                        foreach (DataRow dr in taxDetail.Rows)
                        {
                            if (Convert.ToString(dr["TaxTypeCode"]).Trim() == "CGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "SGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "UTGST")
                            {
                                dr.Delete();
                            }
                        }
                    }
                    else if (TaxType == "SGST")
                    {
                        foreach (DataRow dr in taxDetail.Rows)
                        {
                            if (Convert.ToString(dr["TaxTypeCode"]).Trim() == "IGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "UTGST")
                            {
                                dr.Delete();
                            }
                        }
                    }
                    taxDetail.AcceptChanges();
                    #endregion

                    //#region CreateTempTaxTable
                    //DataTable TaxRecord = new DataTable();

                    //TaxRecord.Columns.Add("SlNo", typeof(System.Int32));
                    //TaxRecord.Columns.Add("TaxCode", typeof(System.String));
                    //TaxRecord.Columns.Add("AltTaxCode", typeof(System.String));
                    //TaxRecord.Columns.Add("Percentage", typeof(System.Decimal));
                    //TaxRecord.Columns.Add("Amount", typeof(System.Decimal)); 

                    //#endregion






                    //decimal totalParcentage = 0;
                    //foreach (DataRow dr in taxDetail.Rows)
                    //{
                    //    if (Convert.ToString(dr["TaxTypeCode"]).Trim() == "CGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "SGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "IGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "UTGST")
                    //    {
                    //        totalParcentage += Convert.ToDecimal(dr["TaxRates_Rate"]);
                    //    }
                    //}


                    if (Convert.ToDecimal(productRow[chargesColumns]) != 0)
                    {
                        foreach (DataRow taxexistingRow in taxDetail.Rows)
                        {
                            if (Convert.ToString(taxexistingRow["TaxTypeCode"]).Trim() != "O")
                            {
                                DataRow txRecordRow = taxTable.NewRow();
                                txRecordRow["SlNo"] = productRow[SerialColumnName];
                                txRecordRow["TaxCode"] = taxexistingRow["Taxes_ID"];
                                txRecordRow["AltTaxCode"] = "0";
                                txRecordRow["Percentage"] = taxexistingRow["TaxRates_Rate"];
                                txRecordRow["Amount"] = Math.Round(Convert.ToDecimal(productRow[AmountColumnName]) * (Convert.ToDecimal(taxexistingRow["TaxRates_Rate"]) / 100), 2, MidpointRounding.AwayFromZero);
                                taxTable.Rows.Add(txRecordRow);

                            }
                        }
                    }
                }


            }



            #region setRound of for GST
            //DataTable GSTTABLE = oDBEngine.GetDataTable("select TaxRates_ID,TaxTypeCode from Config_TaxRates config inner join Master_Taxes mast on config.TaxRates_TaxCode =mast.Taxes_ID where TaxTypeCode in ('IGST','CGST','SGST','UTGST')");

            //foreach (DataRow gstrow in taxTable.Rows)
            //{
            //    if (GSTTABLE.Select("TaxRates_ID=" + gstrow["TaxCode"]).Length > 0)
            //    {
            //        gstrow["Amount"] = Math.Round(Convert.ToDecimal(gstrow["Amount"]));
            //    }
            //}

            #endregion





            foreach (DataRow productRow in productDetails.Rows)
            {
                #region SetRounded of ledger


                if (taxTable.Select("SlNo='" + productRow[SerialColumnName] + "'").Length > 0)
                {
                    DataTable filteredTable = taxTable.Select("SlNo='" + productRow[SerialColumnName] + "'").CopyToDataTable();
                    decimal totalTaxAmount = 0;//Convert.ToDecimal(filteredTable.Compute("SUM(Amount)", string.Empty));

                    foreach (DataRow taxCalRow in filteredTable.Rows)
                    {
                        if (Convert.ToDecimal(taxCalRow["Amount"]) > 0)
                        {
                            DataRow returnRow = oDBEngine.GetDataTable("select TaxCalculateMethods  from Config_TaxRates config inner join Master_Taxes tax on config.TaxRates_TaxCode  = tax.Taxes_ID where TaxRates_ID='" + Convert.ToString(taxCalRow["TaxCode"]) + "'").Rows[0];

                            if (Convert.ToString(returnRow["TaxCalculateMethods"]) == "A")
                                totalTaxAmount = totalTaxAmount + Convert.ToDecimal(taxCalRow["Amount"]);
                            else
                                totalTaxAmount = totalTaxAmount - Convert.ToDecimal(taxCalRow["Amount"]);
                        }
                    }


                    if (totalTaxAmount != Convert.ToDecimal(productRow[chargesColumns]))
                    {
                        decimal roundOf = Convert.ToDecimal(productRow[chargesColumns]) - totalTaxAmount;
                        productRow[chargesColumns] = Convert.ToDecimal(productRow[chargesColumns]) - roundOf;
                        if (inclusiveOrExclusive == "I")
                            productRow[AmountColumnName] = Convert.ToDecimal(productRow[AmountColumnName]) + roundOf;
                        else if (inclusiveOrExclusive == "E")
                        {
                            productRow[NetAmountCol] = Convert.ToDecimal(productRow[NetAmountCol]) - roundOf;
                        }

                    }
                }



                #endregion
            }

            return taxTable;
        }

        private void GetTotalAmount(DataTable filteredTable, ref decimal totalTaxAmount)
        {
            foreach (DataRow taxCalRow in filteredTable.Rows)
            {
                if (Convert.ToDecimal(taxCalRow["Amount"]) > 0)
                {
                    DataRow returnRow = oDBEngine.GetDataTable("select TaxCalculateMethods  from Config_TaxRates config inner join Master_Taxes tax on config.TaxRates_TaxCode  = tax.Taxes_ID where TaxRates_ID='" + Convert.ToString(taxCalRow["TaxCode"]) + "'").Rows[0];

                    if (Convert.ToString(returnRow["TaxCalculateMethods"]) == "A")
                        totalTaxAmount = totalTaxAmount + Convert.ToDecimal(taxCalRow["Amount"]);
                    else
                        totalTaxAmount = totalTaxAmount - Convert.ToDecimal(taxCalRow["Amount"]);
                }
            }
        }

        public double RoundDown(double i, double decimalPlaces)
        {
            double power = Math.Pow(10, decimalPlaces);
            return Math.Floor(i * power) / power;
        }
        public double RoundUp(double i, double decimalPlaces)
        {
            double Amount = 0;

            string input_decimal_number = Convert.ToString(i);
            string decimal_places = "";

            var regex = new System.Text.RegularExpressions.Regex("(?<=[\\.])[0-9]+");
            if (regex.IsMatch(input_decimal_number)) decimal_places = regex.Match(input_decimal_number).Value;

            if (decimal_places.Length > 2)
            {
                string last_decimal_places = decimal_places.Substring(decimal_places.Length - 1);
                if (Convert.ToInt32(last_decimal_places) >= 5)
                {
                    double power = Math.Pow(10, decimalPlaces);
                    Amount = Math.Ceiling(i * power) / power;
                }
                else
                {
                    Amount = GetRoundOfValue(Convert.ToDecimal(i));
                }
            }
            else
            {
                Amount = GetRoundOfValue(Convert.ToDecimal(i));
            }

            return Amount;
        }
        public double GetRoundOfValue(decimal Value)
        {
            double RoundOfValue = 0;

            if (Value > 0)
            {
                decimal _Value = Convert.ToDecimal(Value);
                _Value = Math.Round(Value, 2);

                RoundOfValue = Convert.ToDouble(_Value);
            }

            return RoundOfValue;
        }
    }
}