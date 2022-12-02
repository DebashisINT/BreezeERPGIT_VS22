using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

/// <summary>
/// Summary description for GenericMethod
/// </summary>

namespace ERP.OMS.Management
{
    public class GenericMethod
    {
        public GenericMethod()
        {

        }
        #region AjaxMethods

        #region Asset And Derivatives On Asset Selection Relavant
        // 'Param_ExchangeSegmentID' : If You Want To Find Specific ExchangeSegment Assets / Pass 0 in Other Case
        // 'SpecificExchange' : In Case Of INMX You Need To Pass a ExchangeID (User Input)/ Pass 0 in Other Case

        public String GetAssetsOrDerivative(string UnderLyingOrDerivative, int ExchangeSegmentID, int SpecificExchange, string SeriesIdentifier)
        {
            if (UnderLyingOrDerivative == "A")
            {
                return GetUnderLyingAssets(ExchangeSegmentID);
            }
            else
            {
                return GetDerivativeOnAssets(ExchangeSegmentID, SpecificExchange, SeriesIdentifier);
            }
        }
        public String GetAssetsOrDerivative(string UnderLyingOrDerivative, int ExchangeSegmentID, int SpecificExchange, string SeriesIdentifier, DateTime ExpiryOrEffectiveDate)
        {
            if (UnderLyingOrDerivative == "A")
            {
                return GetUnderLyingAssets(ExchangeSegmentID);
            }
            else
            {
                return GetDerivativeOnAssets(ExchangeSegmentID, SpecificExchange, SeriesIdentifier, ExpiryOrEffectiveDate);
            }
        }

        //Finding Assets
        public String GetUnderLyingAssets(int ExchangeSegmentID)
        {
            string ProductTypeID = String.Empty;

            if (ExchangeSegmentID == 0)
                ExchangeSegmentID = Convert.ToInt32(HttpContext.Current.Session["ExchangeSegmentID"].ToString());

            if (ExchangeSegmentID == 1)//for CM
            {
                ProductTypeID = "1";
            }
            if (ExchangeSegmentID == 2 || ExchangeSegmentID == 5)//for FO
            {
                ProductTypeID = "4,6";
            }
            if (ExchangeSegmentID == 3 || ExchangeSegmentID == 6 || ExchangeSegmentID == 8 || ExchangeSegmentID == 13)//for CDX
            {
                ProductTypeID = "11";
            }
            if (ExchangeSegmentID == 14)// For SPOT
            {
                ProductTypeID = "11";
            }
            if (ExchangeSegmentID == 7 || ExchangeSegmentID == 9 || ExchangeSegmentID == 10 || ExchangeSegmentID == 11
                || ExchangeSegmentID == 12 || ExchangeSegmentID == 17 || ExchangeSegmentID == 18)//for COMM
            {
                if (ExchangeSegmentID == 10 || ExchangeSegmentID == 18)// INMX/DGCX
                {
                    ProductTypeID = "6,9,11";
                }
                else
                {
                    ProductTypeID = "11";
                }
            }

            string strQuery_Table = "Master_Products";
            string strQuery_FieldName = @"top 10 Ltrim(Rtrim(Products_Name)) +' ['+ (Select Ltrim(Rtrim(ProductType_Name)) from Master_ProductTypes Where ProductType_ID=Products_ProductTypeID)+']' as dd,Products_ID,Products_Name";
            string strQuery_WhereClause = "Products_ProductTypeID in (" + ProductTypeID + @") And Products_Name Like 'RequestLetter%'";
            string strQuery_OrderBy = "";
            string strQuery_GroupBy = "";
            string CombinedQuery = strQuery_Table + " $" + strQuery_FieldName + " $" + strQuery_WhereClause + " $" + strQuery_GroupBy + "$" + strQuery_OrderBy;
            CombinedQuery = CombinedQuery.Replace("'", "\\'");
            return CombinedQuery;

        }

        //Finding Derivative On Assets

        public String GetDerivativeOnAssets(int ExchangeSegmentID, int SpecificExchange, string SeriesIdentifier)
        // 'SpecificExchange' : In Case Of INMX You Need To Pass a ExchangeID (User Input)/ Pass 0 in Other Case
        // ''SeriesIdentifier' :In Case When User Want To See Equity,Bond,Future,Option Kinda Specific Product / Pass 0 in Other Case
        {
            string ProductTypeID = String.Empty;
            string strQuery_Table = String.Empty;
            string strQuery_FieldName = String.Empty;
            string strQuery_WhereClause = String.Empty;
            string strQuery_OrderBy = "";
            string strQuery_GroupBy = "";

            if (ExchangeSegmentID == 0)
                ExchangeSegmentID = Convert.ToInt32(HttpContext.Current.Session["ExchangeSegmentID"].ToString());

            if (ExchangeSegmentID == 1 || ExchangeSegmentID == 4)//For CM
            {
                strQuery_Table = "Master_Equity";
                if (ExchangeSegmentID == 1)
                    strQuery_FieldName = @"Top 10 Ltrim(Rtrim(isnull(Equity_TickerSymbol,'')))+'[ ' + Ltrim(Rtrim(isnull(Equity_TickerCode,'')))+' ][' + Ltrim(Rtrim(isnull(Equity_Series,'')))+']' EquityDetail,Equity_SeriesID ,Equity_TickerSymbol,Equity_TickerCode";
                else
                    strQuery_FieldName = @"Top 10 Ltrim(Rtrim(isnull(Equity_TickerSymbol,'')))+'[ ' + Ltrim(Rtrim(isnull(Equity_TickerCode,'')))+' ]' EquityDetail,Equity_SeriesID,Equity_TickerSymbol,Equity_TickerCode";

                if (SeriesIdentifier.Trim() == "NA")
                    strQuery_WhereClause = @"Equity_ExchSegmentID=" + ExchangeSegmentID.ToString() + " And (Equity_TickerSymbol Like 'RequestLetter%' or Equity_TickerCode Like 'RequestLetter%')";
                else
                    strQuery_WhereClause = @"Equity_ExchSegmentID=" + ExchangeSegmentID.ToString() + "And Equity_Series='" + SeriesIdentifier + "' And (Equity_TickerSymbol Like 'RequestLetter%' or Equity_TickerCode Like 'RequestLetter%')";

            }

            if (ExchangeSegmentID == 2 || ExchangeSegmentID == 5)//for FO
            {
                strQuery_Table = "Master_Equity";
                strQuery_FieldName = @"Top 10 TickerSymbol,Equity_SeriesID from (select (case when Equity_StrikePrice=0.0 then isnull(rtrim(ltrim(Equity_TickerSymbol)),'')+' '+isnull(rtrim(ltrim(Equity_Series)),'')+' '+convert(varchar(9),Equity_EffectUntil,6) else isnull(rtrim(ltrim(Equity_TickerSymbol)),'')+' '+isnull(rtrim(ltrim(Equity_Series)),'')+' '+convert(varchar(9),Equity_EffectUntil,6)+' '+cast(cast(round(Equity_StrikePrice,2) as numeric(28,2)) as varchar) end) as TickerSymbol,Equity_SeriesID,Equity_StrikePrice,Equity_TickerSymbol,Equity_Series,Equity_EffectUntil";
                if (SeriesIdentifier.Trim() == "NA")
                    strQuery_WhereClause = @"Equity_ExchSegmentID=" + ExchangeSegmentID.ToString() + " And (Equity_SeriesID Like 'RequestLetter%' or Equity_StrikePrice Like 'RequestLetter%' Or Equity_TickerSymbol Like 'RequestLetter%' Or Equity_Series Like 'RequestLetter%' Or convert(varchar(9),Equity_EffectUntil,6) Like 'RequestLetter%')) as T1";
                else
                {
                    if (SeriesIdentifier.Trim() == "FUT" || SeriesIdentifier.Trim() == "OPT")
                        strQuery_WhereClause = @"Equity_ExchSegmentID=" + ExchangeSegmentID.ToString() + "And Left(Equity_FOIdentifier,3)='" + SeriesIdentifier + "' And (Equity_SeriesID Like 'RequestLetter%' or Equity_StrikePrice Like 'RequestLetter%' Or Equity_TickerSymbol Like 'RequestLetter%' Or Equity_Series Like 'RequestLetter%' Or convert(varchar(9),Equity_EffectUntil,6) Like 'RequestLetter%')) as T1";
                    else
                        strQuery_WhereClause = @"Equity_ExchSegmentID=" + ExchangeSegmentID.ToString() + "And Equity_FOIdentifier='" + SeriesIdentifier + "' And (Equity_SeriesID Like 'RequestLetter%' or Equity_StrikePrice Like 'RequestLetter%' Or Equity_TickerSymbol Like 'RequestLetter%' Or Equity_Series Like 'RequestLetter%' Or convert(varchar(9),Equity_EffectUntil,6) Like 'RequestLetter%')) as T1";
                }
            }
            if (ExchangeSegmentID == 3 || ExchangeSegmentID == 6 || ExchangeSegmentID == 8 || ExchangeSegmentID == 13 //For CDX
                || ExchangeSegmentID == 14 // For SPOT
                || ExchangeSegmentID == 7 || ExchangeSegmentID == 9 || ExchangeSegmentID == 10 || ExchangeSegmentID == 11
                || ExchangeSegmentID == 12 || ExchangeSegmentID == 17 || ExchangeSegmentID == 18)//for COMM
            {
                if (ExchangeSegmentID == 18)// INMX/DGCX
                {
                    string SeriesIdentifier_WhereQueryPart = String.Empty;
                    string SpecificExchange_WhereQueryPart = String.Empty;
                    if (SeriesIdentifier.Trim() != "NA")
                        SeriesIdentifier_WhereQueryPart = " And Left(Commodity_Identifier,3)='" + SeriesIdentifier + "'";
                    if (SpecificExchange != 0)
                        SpecificExchange_WhereQueryPart = " And Commodity_Exchange=" + SpecificExchange.ToString();


                    strQuery_Table = "master_commodity";
                    strQuery_FieldName = @"Top 10 (ltrim(rtrim(isnull(Commodity_TickerSymbol,'')))+'  '+ltrim(rtrim(isnull(Commodity_Identifier,'')))+'  '+ltrim(rtrim(isnull(Commodity_TickerSeries,'')))+'  '+cast(cast(isnull(Commodity_StrikePrice,0.00) as numeric(16,2)) as varchar)+'  '+convert(varchar(11),Commodity_EffectiveDate,113)) as Commodity_Product,ltrim(rtrim(Commodity_ProductSeriesID)) as Commodity_ProductSeriesID,Commodity_TickerSymbol,Commodity_Identifier,Commodity_TickerSeries,Commodity_StrikePrice,Commodity_EffectiveDate";
                    strQuery_WhereClause = "(Commodity_ExchangeSegmentID=" + ExchangeSegmentID.ToString() + SeriesIdentifier_WhereQueryPart + SpecificExchange_WhereQueryPart + ") And (Commodity_TickerSymbol Like 'RequestLetter%' Or Commodity_Identifier Like 'RequestLetter%' Or Commodity_TickerSeries Like 'RequestLetter%' Or Commodity_StrikePrice Like 'RequestLetter%' Or Commodity_EffectiveDate Like 'RequestLetter%')";

                }
                else
                {
                    strQuery_Table = "master_commodity";
                    strQuery_FieldName = @"Top 10 (ltrim(rtrim(isnull(Commodity_TickerSymbol,'')))+'  '+ltrim(rtrim(isnull(Commodity_Identifier,'')))+'  '+ltrim(rtrim(isnull(Commodity_TickerSeries,'')))+'  '+cast(cast(isnull(Commodity_StrikePrice,0.00) as numeric(16,2)) as varchar)+'  '+convert(varchar(11),Commodity_EffectiveDate,113)) as Commodity_Product,ltrim(rtrim(Commodity_ProductSeriesID)) as Commodity_ProductSeriesID,Commodity_TickerSymbol,Commodity_Identifier,Commodity_TickerSeries,Commodity_StrikePrice,convert(varchar(11),Commodity_EffectiveDate,113) Commodity_EffectiveDate";
                    if (SeriesIdentifier.Trim() == "NA")
                        strQuery_WhereClause = "(Commodity_ExchangeSegmentID=" + ExchangeSegmentID.ToString() + @") And (Commodity_TickerSymbol Like 'RequestLetter%' Or Commodity_Identifier Like 'RequestLetter%' Or Commodity_TickerSeries Like 'RequestLetter%' Or Commodity_StrikePrice Like 'RequestLetter%' Or convert(varchar(11),Commodity_EffectiveDate,113) Like 'RequestLetter%')";
                    else
                        strQuery_WhereClause = "(Commodity_ExchangeSegmentID=" + ExchangeSegmentID.ToString() + @" And Left(Commodity_Identifier,3)='" + SeriesIdentifier + "') And (Commodity_TickerSymbol Like 'RequestLetter%' Or Commodity_Identifier Like 'RequestLetter%' Or Commodity_TickerSeries Like 'RequestLetter%' Or Commodity_StrikePrice Like 'RequestLetter%' Or convert(varchar(11),Commodity_EffectiveDate,113) Like 'RequestLetter%')";

                }
            }

            string CombinedQuery = strQuery_Table + " $" + strQuery_FieldName + " $" + strQuery_WhereClause + " $" + strQuery_GroupBy + "$" + strQuery_OrderBy;
            CombinedQuery = CombinedQuery.Replace("'", "\\'");
            return CombinedQuery;
        }

        /////Get Derivative Which Expiry Or EffectUntil Date is Equel and Greater Than SpecificDate
        public String GetDerivativeOnAssets(int ExchangeSegmentID, int SpecificExchange, string SeriesIdentifier, DateTime ExpiryOrEffectUntilDate)
        // 'SpecificExchange' : In Case Of INMX You Need To Pass a ExchangeID (User Input)/ Pass 0 in Other Case
        // ''SeriesIdentifier' :In Case When User Want To See Equity,Bond,Future,Option Kinda Specific Product / Pass 0 in Other Case
        // ExpiryOrEffectUntilDate For Showing Derivative Equal and Greater Than This Date
        {
            string ProductTypeID = String.Empty;
            string strQuery_Table = String.Empty;
            string strQuery_FieldName = String.Empty;
            string strQuery_WhereClause = String.Empty;
            string strQuery_OrderBy = "";
            string strQuery_GroupBy = "";

            if (ExchangeSegmentID == 0)
                ExchangeSegmentID = Convert.ToInt32(HttpContext.Current.Session["ExchangeSegmentID"].ToString());

            if (ExchangeSegmentID == 1 || ExchangeSegmentID == 4)//For CM
            {
                strQuery_Table = "Master_Equity";
                if (ExchangeSegmentID == 1)
                    strQuery_FieldName = @"Top 10 Ltrim(Rtrim(isnull(Equity_TickerSymbol,'')))+'[ ' + Ltrim(Rtrim(isnull(Equity_TickerCode,'')))+' ][' + Ltrim(Rtrim(isnull(Equity_Series,'')))+']' EquityDetail,Equity_SeriesID ,Equity_TickerSymbol,Equity_TickerCode";
                else
                    strQuery_FieldName = @"Top 10 Ltrim(Rtrim(isnull(Equity_TickerSymbol,'')))+'[ ' + Ltrim(Rtrim(isnull(Equity_TickerCode,'')))+' ]' EquityDetail,Equity_TickerSymbol,Equity_TickerCode";

                if (SeriesIdentifier.Trim() == "NA")
                    strQuery_WhereClause = @"Equity_ExchSegmentID=" + ExchangeSegmentID.ToString() + " And (Equity_TickerSymbol Like 'RequestLetter%' or Equity_TickerCode Like 'RequestLetter%')";
                else
                    strQuery_WhereClause = @"Equity_ExchSegmentID=" + ExchangeSegmentID.ToString() + "And Equity_Series='" + SeriesIdentifier + "' And (Equity_TickerSymbol Like 'RequestLetter%' or Equity_TickerCode Like 'RequestLetter%')";

            }

            if (ExchangeSegmentID == 2 || ExchangeSegmentID == 5)//for FO
            {
                strQuery_Table = "Master_Equity";
                strQuery_FieldName = @"Top 10 TickerSymbol,Equity_SeriesID from (select (case when Equity_StrikePrice=0.0 then isnull(rtrim(ltrim(Equity_TickerSymbol)),'')+' '+isnull(rtrim(ltrim(Equity_Series)),'')+' '+convert(varchar(9),Equity_EffectUntil,6) else isnull(rtrim(ltrim(Equity_TickerSymbol)),'')+' '+isnull(rtrim(ltrim(Equity_Series)),'')+' '+convert(varchar(9),Equity_EffectUntil,6)+' '+cast(cast(round(Equity_StrikePrice,2) as numeric(28,2)) as varchar) end) as TickerSymbol,Equity_SeriesID,Equity_StrikePrice,Equity_TickerSymbol,Equity_Series,Equity_EffectUntil";
                if (SeriesIdentifier.Trim() == "NA")
                    strQuery_WhereClause = @"Equity_ExchSegmentID=" + ExchangeSegmentID.ToString() + "And  Equity_EffectUntil>='" + ExpiryOrEffectUntilDate.ToString("yyyy-MM-dd") + "' And (Equity_SeriesID Like 'RequestLetter%' or Equity_StrikePrice Like 'RequestLetter%' Or Equity_TickerSymbol Like 'RequestLetter%' Or Equity_Series Like 'RequestLetter%' Or convert(varchar(9),Equity_EffectUntil,6) Like 'RequestLetter%')) as T1";
                else
                {
                    if (SeriesIdentifier.Trim() == "FUT" || SeriesIdentifier.Trim() == "OPT")
                        strQuery_WhereClause = @"Equity_ExchSegmentID=" + ExchangeSegmentID.ToString() + "And Left(Equity_FOIdentifier,3)='" + SeriesIdentifier + "' And (Equity_SeriesID Like 'RequestLetter%' or Equity_StrikePrice Like 'RequestLetter%' Or Equity_TickerSymbol Like 'RequestLetter%' Or Equity_Series Like 'RequestLetter%' Or convert(varchar(9),Equity_EffectUntil,6) Like 'RequestLetter%')) as T1";
                    else
                        strQuery_WhereClause = @"Equity_ExchSegmentID=" + ExchangeSegmentID.ToString() + "And Equity_FOIdentifier='" + SeriesIdentifier + "' And (Equity_SeriesID Like 'RequestLetter%' or Equity_StrikePrice Like 'RequestLetter%' Or Equity_TickerSymbol Like 'RequestLetter%' Or Equity_Series Like 'RequestLetter%' Or convert(varchar(9),Equity_EffectUntil,6) Like 'RequestLetter%')) as T1";
                }
            }
            if (ExchangeSegmentID == 3 || ExchangeSegmentID == 6 || ExchangeSegmentID == 8 || ExchangeSegmentID == 13 //For CDX
                || ExchangeSegmentID == 14 // For SPOT
                || ExchangeSegmentID == 7 || ExchangeSegmentID == 9 || ExchangeSegmentID == 10 || ExchangeSegmentID == 11
                || ExchangeSegmentID == 12 || ExchangeSegmentID == 17 || ExchangeSegmentID == 18)//for COMM
            {
                if (ExchangeSegmentID == 18)// INMX/DGCX
                {
                    string SeriesIdentifier_WhereQueryPart = String.Empty;
                    string SpecificExchange_WhereQueryPart = String.Empty;
                    if (SeriesIdentifier.Trim() != "NA")
                        SeriesIdentifier_WhereQueryPart = " And Left(Commodity_Identifier,3)='" + SeriesIdentifier + "'";
                    if (SpecificExchange != 0)
                        SpecificExchange_WhereQueryPart = " And Commodity_Exchange=" + SpecificExchange.ToString();


                    strQuery_Table = "master_commodity";
                    strQuery_FieldName = @"Top 10 (ltrim(rtrim(isnull(Commodity_TickerSymbol,'')))+'  '+ltrim(rtrim(isnull(Commodity_Identifier,'')))+'  '+ltrim(rtrim(isnull(Commodity_TickerSeries,'')))+'  '+cast(cast(isnull(Commodity_StrikePrice,0.00) as numeric(16,2)) as varchar)+'  '+convert(varchar(11),Commodity_EffectiveDate,113)) as Commodity_Product,ltrim(rtrim(Commodity_ProductSeriesID)) as Commodity_ProductSeriesID,Commodity_TickerSymbol,Commodity_Identifier,Commodity_TickerSeries,Commodity_StrikePrice,Commodity_EffectiveDate";
                    strQuery_WhereClause = "(Commodity_ExchangeSegmentID=" + ExchangeSegmentID.ToString() + SeriesIdentifier_WhereQueryPart + SpecificExchange_WhereQueryPart + ") And Commodity_EffectiveDate>='" + ExpiryOrEffectUntilDate.ToString("yyyy-MM-dd") + "' And (Commodity_TickerSymbol Like 'RequestLetter%' Or Commodity_Identifier Like 'RequestLetter%' Or Commodity_TickerSeries Like 'RequestLetter%' Or Commodity_StrikePrice Like 'RequestLetter%' Or Commodity_EffectiveDate Like 'RequestLetter%')";

                }
                else
                {
                    strQuery_Table = "master_commodity";
                    strQuery_FieldName = @"Top 10 (ltrim(rtrim(isnull(Commodity_TickerSymbol,'')))+'  '+ltrim(rtrim(isnull(Commodity_Identifier,'')))+'  '+ltrim(rtrim(isnull(Commodity_TickerSeries,'')))+'  '+cast(cast(isnull(Commodity_StrikePrice,0.00) as numeric(16,2)) as varchar)+'  '+convert(varchar(11),Commodity_EffectiveDate,113)) as Commodity_Product,ltrim(rtrim(Commodity_ProductSeriesID)) as Commodity_ProductSeriesID,Commodity_TickerSymbol,Commodity_Identifier,Commodity_TickerSeries,Commodity_StrikePrice,convert(varchar(11),Commodity_EffectiveDate,113) Commodity_EffectiveDate";
                    if (SeriesIdentifier.Trim() == "NA")
                        strQuery_WhereClause = "(Commodity_ExchangeSegmentID=" + ExchangeSegmentID.ToString() + @") And Commodity_EffectiveDate>='" + ExpiryOrEffectUntilDate.ToString("yyyy-MM-dd") + "' And (Commodity_TickerSymbol Like 'RequestLetter%' Or Commodity_Identifier Like 'RequestLetter%' Or Commodity_TickerSeries Like 'RequestLetter%' Or Commodity_StrikePrice Like 'RequestLetter%' Or convert(varchar(11),Commodity_EffectiveDate,113) Like 'RequestLetter%')";
                    else
                        strQuery_WhereClause = "(Commodity_ExchangeSegmentID=" + ExchangeSegmentID.ToString() + @" And Left(Commodity_Identifier,3)='" + SeriesIdentifier + "') And Commodity_EffectiveDate>='" + ExpiryOrEffectUntilDate.ToString("yyyy-MM-dd") + "' And (Commodity_TickerSymbol Like 'RequestLetter%' Or Commodity_Identifier Like 'RequestLetter%' Or Commodity_TickerSeries Like 'RequestLetter%' Or Commodity_StrikePrice Like 'RequestLetter%' Or convert(varchar(11),Commodity_EffectiveDate,113) Like 'RequestLetter%')";

                }
            }

            string CombinedQuery = strQuery_Table + " $" + strQuery_FieldName + " $" + strQuery_WhereClause + " $" + strQuery_GroupBy + "$" + strQuery_OrderBy;
            CombinedQuery = CombinedQuery.Replace("'", "\\'");
            return CombinedQuery;
        }
        #endregion

        #region Client Selection Relavant

        string strQuery_Table = String.Empty;
        string strQuery_FieldName = String.Empty;
        string strQuery_WhereClause = String.Empty;
        string strQuery_OrderBy = String.Empty;
        string strQuery_GroupBy = String.Empty;
        //ClientWise
        /// Prefix Like 'CL' For Client 'EM For Employee and So on .....
        string GetAllContact(string Prefix)
        {
            string strQuery_Table = @"(select isnull(Ltrim(Rtrim(cnt_firstName)),'')+' '+isnull(Ltrim(Rtrim(cnt_middleName)),'')
                                        +' '+isnull(Ltrim(Rtrim(cnt_lastName)),'')+'['+Ltrim(Rtrim(cnt_UCC))+']' TextField,
                                        isnull(Ltrim(Rtrim(cnt_firstName)),'')+'~'+isnull(Ltrim(Rtrim(cnt_middleName)),'')+'~'+
                                        isnull(Ltrim(Rtrim(cnt_lastName)),'')+'~'+Ltrim(Rtrim(cnt_UCC))+'~'+LTRIM(Rtrim(cnt_internalId)) ValueField,
                                        cnt_id ID,cnt_firstName,cnt_middleName,cnt_lastName,cnt_UCC
                                        from tbl_master_contact Where Left(cnt_internalId,2)='" + Prefix + "') AllClient";
            string strQuery_FieldName = "Top 10  *";
            string strQuery_WhereClause = @"cnt_firstName like '%RequestLetter%' Or cnt_middleName like '%RequestLetter%' Or cnt_lastName like '%RequestLetter%'
                                            or cnt_UCC like '%RequestLetter%' Or TextField like '%RequestLetter%'";
            string strQuery_OrderBy = "TextField";
            string strQuery_GroupBy = "";
            return ReturnCombinedQuery(strQuery_Table, strQuery_FieldName, strQuery_WhereClause, strQuery_GroupBy, strQuery_OrderBy);
        }

        //SegmentFilter
        ////Client Of Specific Company and All Segment
        string GetClient_SegmentFilter(string CompanyID)
        {
            string strQuery_Table = @"(Select isnull(Ltrim(Rtrim(cnt_firstName)),'')+' '+isnull(Ltrim(Rtrim(cnt_middleName)),'')
                                        +' '+isnull(Ltrim(Rtrim(cnt_lastName)),'')+'['+Ltrim(Rtrim(cnt_UCC))+']'+'['+Ltrim(Rtrim(crg_tcode))+']' TextField,
                                        isnull(Ltrim(Rtrim(cnt_firstName)),'')+'~'+isnull(Ltrim(Rtrim(cnt_middleName)),'')+'~'+
                                        isnull(Ltrim(Rtrim(cnt_lastName)),'')+'~'+Ltrim(Rtrim(cnt_UCC))+','+Ltrim(Rtrim(crg_tcode))+'~'+
                                        LTRIM(Rtrim(cnt_internalId))+'~'+Cast(cnt_id as Varchar(50))+'~'+crg_exchange ValueField,
                                        isnull(Ltrim(Rtrim(cnt_firstName)),'') FirstName,isnull(Ltrim(Rtrim(cnt_middleName)),'') MiddleName,
                                        isnull(Ltrim(Rtrim(cnt_lastName)),'') LastName,Ltrim(Rtrim(cnt_UCC)) UCC,Ltrim(Rtrim(crg_tcode)) TCode,
                                        LTRIM(Rtrim(cnt_internalId)) InternalID,Cast(cnt_id as Varchar(50)) Cnt_ID,crg_exchange
                                        from tbl_master_contact,tbl_master_contactExchange
                                        Where cnt_internalId=crg_cntID And Left(cnt_internalId,2)='CL' And
                                        crg_company='" + CompanyID + "') SpecificCompNAllSegment";
            string strQuery_FieldName = "Top 10  *";
            string strQuery_WhereClause = @"FirstName like '%RequestLetter%' Or MiddleName like '%RequestLetter%' Or LastName like '%RequestLetter%'
                                            or UCC like '%RequestLetter%' or TCode like '%RequestLetter%' Or TextField like '%RequestLetter%'";
            string strQuery_OrderBy = "TextField";
            string strQuery_GroupBy = "";
            return ReturnCombinedQuery(strQuery_Table, strQuery_FieldName, strQuery_WhereClause, strQuery_GroupBy, strQuery_OrderBy);
        }
        ////Client Of Specific Company and Specific Segment
        string GetClient_SegmentFilter(string CompanyID, string SpecificSegment)
        {
            string strQuery_Table = @"(Select isnull(Ltrim(Rtrim(cnt_firstName)),'')+' '+isnull(Ltrim(Rtrim(cnt_middleName)),'')
                                        +' '+isnull(Ltrim(Rtrim(cnt_lastName)),'')+'['+Ltrim(Rtrim(cnt_UCC))+']'+'['+Ltrim(Rtrim(crg_tcode))+']' TextField,
                                        isnull(Ltrim(Rtrim(cnt_firstName)),'')+'~'+isnull(Ltrim(Rtrim(cnt_middleName)),'')+'~'+
                                        isnull(Ltrim(Rtrim(cnt_lastName)),'')+'~'+Ltrim(Rtrim(cnt_UCC))+','+Ltrim(Rtrim(crg_tcode))+'~'+
                                        LTRIM(Rtrim(cnt_internalId))+'~'+Cast(cnt_id as Varchar(50))+'~'+crg_exchange ValueField,
                                        isnull(Ltrim(Rtrim(cnt_firstName)),'') FirstName,isnull(Ltrim(Rtrim(cnt_middleName)),'') MiddleName,
                                        isnull(Ltrim(Rtrim(cnt_lastName)),'') LastName,Ltrim(Rtrim(cnt_UCC)) UCC,Ltrim(Rtrim(crg_tcode)) TCode,
                                        LTRIM(Rtrim(cnt_internalId)) InternalID,Cast(cnt_id as Varchar(50)) Cnt_ID,crg_exchange
                                        from tbl_master_contact,tbl_master_contactExchange
                                        Where cnt_internalId=crg_cntID And Left(cnt_internalId,2)='CL' And
                                        crg_company='" + CompanyID + "' And crg_exchange in (" + SpecificSegment + ")) SpecificCompSpecificSeg";
            string strQuery_FieldName = "Top 10  *";
            string strQuery_WhereClause = @"FirstName like '%RequestLetter%' Or MiddleName like '%RequestLetter%' Or LastName like '%RequestLetter%'
                                            or UCC like '%RequestLetter%' or TCode like '%RequestLetter%' Or TextField like '%RequestLetter%'";
            string strQuery_OrderBy = "TextField";
            string strQuery_GroupBy = "";
            return ReturnCombinedQuery(strQuery_Table, strQuery_FieldName, strQuery_WhereClause, strQuery_GroupBy, strQuery_OrderBy);
        }


        //BranchFilter

        ////Client Of Specific Company and All Branch
        string GetClient_BranchFilter(string CompanyID)
        {
            string strQuery_Table = @"(Select isnull(Ltrim(Rtrim(cnt_firstName)),'')+' '+isnull(Ltrim(Rtrim(cnt_middleName)),'')
                                        +' '+isnull(Ltrim(Rtrim(cnt_lastName)),'')+'['+Ltrim(Rtrim(cnt_UCC))+']'+'['+Ltrim(Rtrim(crg_tcode))+']' TextField,
                                        isnull(Ltrim(Rtrim(cnt_firstName)),'')+'~'+isnull(Ltrim(Rtrim(cnt_middleName)),'')+'~'+
                                        isnull(Ltrim(Rtrim(cnt_lastName)),'')+'~'+Ltrim(Rtrim(cnt_UCC))+'~'+Ltrim(Rtrim(crg_tcode))+'~'+
                                        LTRIM(Rtrim(cnt_internalId))+'~'+Cast(cnt_id as Varchar(50))+'~'+crg_exchange ValueField,
                                        isnull(Ltrim(Rtrim(cnt_firstName)),'')FirstName,isnull(Ltrim(Rtrim(cnt_middleName)),'') MiddleName,
                                        isnull(Ltrim(Rtrim(cnt_lastName)),'') LastName,Ltrim(Rtrim(cnt_UCC)) UCC,Ltrim(Rtrim(crg_tcode)) TCode,
                                        LTRIM(Rtrim(cnt_internalId)) InternalID,cnt_id ID,crg_exchange from tbl_master_contact,tbl_master_contactExchange
                                        Where cnt_internalId=crg_cntID And Left(cnt_internalId,2)='CL' And
                                        crg_company='" + CompanyID + "') AllSegmentAndSpeficBranch";
            string strQuery_FieldName = "Top 10  *";
            string strQuery_WhereClause = @"FirstName like '%RequestLetter%' Or MiddleName like '%RequestLetter%' Or LastName like '%RequestLetter%'
                                                or UCC like '%RequestLetter%' or TCode like '%RequestLetter%' Or TextField like '%RequestLetter%'";
            string strQuery_OrderBy = "TextField";
            string strQuery_GroupBy = "";
            return ReturnCombinedQuery(strQuery_Table, strQuery_FieldName, strQuery_WhereClause, strQuery_GroupBy, strQuery_OrderBy);
        }
        ////Client Of Specific Company and Specific Branch
        string GetClient_BranchFilter(string CompanyID, string SpecificBranch)
        {
            string strQuery_Table = @"(Select isnull(Ltrim(Rtrim(cnt_firstName)),'')+' '+isnull(Ltrim(Rtrim(cnt_middleName)),'')
                                        +' '+isnull(Ltrim(Rtrim(cnt_lastName)),'')+'['+Ltrim(Rtrim(cnt_UCC))+']'+'['+Ltrim(Rtrim(crg_tcode))+']' TextField,
                                        isnull(Ltrim(Rtrim(cnt_firstName)),'')+'~'+isnull(Ltrim(Rtrim(cnt_middleName)),'')+'~'+
                                        isnull(Ltrim(Rtrim(cnt_lastName)),'')+'~'+Ltrim(Rtrim(cnt_UCC))+'~'+Ltrim(Rtrim(crg_tcode))+'~'+
                                        LTRIM(Rtrim(cnt_internalId))+'~'+Cast(cnt_id as Varchar(50))+'~'+crg_exchange ValueField,
                                        isnull(Ltrim(Rtrim(cnt_firstName)),'')FirstName,isnull(Ltrim(Rtrim(cnt_middleName)),'') MiddleName,
                                        isnull(Ltrim(Rtrim(cnt_lastName)),'') LastName,Ltrim(Rtrim(cnt_UCC)) UCC,Ltrim(Rtrim(crg_tcode)) TCode,
                                        LTRIM(Rtrim(cnt_internalId)) InternalID,cnt_id ID,crg_exchange from tbl_master_contact,tbl_master_contactExchange
                                        Where cnt_internalId=crg_cntID And Left(cnt_internalId,2)='CL' And
                                        crg_company='" + CompanyID + "' and cnt_branchid in (" + SpecificBranch + ")) AllSegmentAndSpeficBranch";
            string strQuery_FieldName = "Top 10  *";
            string strQuery_WhereClause = @"FirstName like '%RequestLetter%' Or MiddleName like '%RequestLetter%' Or LastName like '%RequestLetter%'
                                                or UCC like '%RequestLetter%' or TCode like '%RequestLetter%' Or TextField like '%RequestLetter%'";
            string strQuery_OrderBy = "TextField";
            string strQuery_GroupBy = "";
            return ReturnCombinedQuery(strQuery_Table, strQuery_FieldName, strQuery_WhereClause, strQuery_GroupBy, strQuery_OrderBy);
        }

        ////Client Of Specific Company and Specific Segment and Specific Branch
        string GetClient_BranchFilter(string CompanyID, string SpecificSegment, string SpecificBranch)
        {
            string strQuery_Table = @"(Select isnull(Ltrim(Rtrim(cnt_firstName)),'')+' '+isnull(Ltrim(Rtrim(cnt_middleName)),'')
                                        +' '+isnull(Ltrim(Rtrim(cnt_lastName)),'')+'['+Ltrim(Rtrim(cnt_UCC))+']'+'['+Ltrim(Rtrim(crg_tcode))+']' TextField,
                                        isnull(Ltrim(Rtrim(cnt_firstName)),'')+'~'+isnull(Ltrim(Rtrim(cnt_middleName)),'')+'~'+
                                        isnull(Ltrim(Rtrim(cnt_lastName)),'')+'~'+Ltrim(Rtrim(cnt_UCC))+'~'+Ltrim(Rtrim(crg_tcode))+'~'+
                                        LTRIM(Rtrim(cnt_internalId))+'~'+Cast(cnt_id as Varchar(50))+'~'+crg_exchange ValueField,
                                        isnull(Ltrim(Rtrim(cnt_firstName)),'')FirstName,isnull(Ltrim(Rtrim(cnt_middleName)),'') MiddleName,
                                        isnull(Ltrim(Rtrim(cnt_lastName)),'') LastName,Ltrim(Rtrim(cnt_UCC)) UCC,Ltrim(Rtrim(crg_tcode)) TCode,
                                        LTRIM(Rtrim(cnt_internalId)) InternalID,cnt_id ID,crg_exchange from tbl_master_contact,tbl_master_contactExchange
                                        Where cnt_internalId=crg_cntID And Left(cnt_internalId,2)='CL' And
                                        crg_company='" + CompanyID + "' and crg_exchange in (" + SpecificSegment + ")  and cnt_branchid in (" + SpecificBranch + ")) AllSegmentAndSpeficBranch";
            string strQuery_FieldName = "Top 10  *";
            string strQuery_WhereClause = @"FirstName like '%RequestLetter%' Or MiddleName like '%RequestLetter%' Or LastName like '%RequestLetter%'
                                                or UCC like '%RequestLetter%' or TCode like '%RequestLetter%' Or TextField like '%RequestLetter%'";
            string strQuery_OrderBy = "TextField";
            string strQuery_GroupBy = "";
            return ReturnCombinedQuery(strQuery_Table, strQuery_FieldName, strQuery_WhereClause, strQuery_GroupBy, strQuery_OrderBy);
        }

        //GroupFilter

        //// All Client Of Specific Company and Specific Segment and ALL Group
        string GetClient_GroupFilter(string CompanyID, string SpecificSegment, string GroupType)
        {
            string strQuery_Table = @"(Select  ClientFullName TextField,FirstName+'~'+MiddleName+'~'+LastName+'~'+UCC+'~'+TCode+'~'+InternalID+'~'+
                                        Cast(cnt_ID as Varchar(50))+'~'+crg_exchange+'~'+GrpDetail+'~'+gpm_code+'~'+grp_groupType+'~'+
                                        Cast(gpm_id as Varchar(50)) ValueField,
                                        FirstName,MiddleName,LastName,UCC,TCode,InternalID,cnt_ID,crg_exchange,GrpDetail,gpm_code,grp_groupType,gpm_id
                                        From
                                        (Select 
                                        isnull(Ltrim(Rtrim(cnt_firstName)),'')+' '+isnull(Ltrim(Rtrim(cnt_middleName)),'')
                                        +' '+isnull(Ltrim(Rtrim(cnt_lastName)),'')+'['+Ltrim(Rtrim(cnt_UCC))+']'+'['+Ltrim(Rtrim(crg_tcode))+']' ClientFullName,
                                        isnull(Ltrim(Rtrim(cnt_firstName)),'')FirstName,isnull(Ltrim(Rtrim(cnt_middleName)),'') MiddleName,
                                        isnull(Ltrim(Rtrim(cnt_lastName)),'') LastName,Ltrim(Rtrim(cnt_UCC)) UCC,Ltrim(Rtrim(crg_tcode)) TCode,
                                        LTRIM(Rtrim(cnt_internalId)) InternalID,cnt_id,crg_exchange from tbl_master_contact,tbl_master_contactExchange
                                        Where cnt_internalId=crg_cntID And Left(cnt_internalId,2)='CL' And
                                        crg_company='" + CompanyID + "' and crg_exchange in (" + SpecificSegment + @") Clients
                                        Inner Join 
                                        (Select isnull(Ltrim(Rtrim(gpm_Description)),'')+'['+isnull(Ltrim(Rtrim(gpm_code)),'NoCode')+']' GrpDetail,
                                        grp_contactId,gpm_code,grp_groupType,gpm_id from tbl_master_groupMaster,tbl_trans_group
                                        Where gpm_id=grp_groupMaster and 
                                        grp_groupType='" + GroupType + @"') [Group]
                                        on InternalID=grp_contactId) as T";
            string strQuery_FieldName = "Top 10  *";
            string strQuery_WhereClause = @"FirstName like '%RequestLetter%' Or MiddleName like '%RequestLetter%' Or LastName like '%RequestLetter%'
                                                or UCC like '%RequestLetter%' or TCode like '%RequestLetter%' Or TextField like '%RequestLetter%'";
            string strQuery_OrderBy = "TextField";
            string strQuery_GroupBy = "";
            return ReturnCombinedQuery(strQuery_Table, strQuery_FieldName, strQuery_WhereClause, strQuery_GroupBy, strQuery_OrderBy);
        }
        //--All Client Of Specific Company and Specific Segment and Specific Group

        string GetClient_GroupFilter(string CompanyID, string SpecificSegment, string GroupType, string SpecificGroup)
        {
            string strQuery_Table = @"(Select  ClientFullName TextField,FirstName+'~'+MiddleName+'~'+LastName+'~'+UCC+'~'+TCode+'~'+InternalID+'~'+
                                        Cast(cnt_ID as Varchar(50))+'~'+crg_exchange+'~'+GrpDetail+'~'+gpm_code+'~'+grp_groupType+'~'+
                                        Cast(gpm_id as Varchar(50)) ValueField,
                                        FirstName,MiddleName,LastName,UCC,TCode,InternalID,cnt_ID,crg_exchange,GrpDetail,gpm_code,grp_groupType,gpm_id
                                        From
                                        (Select 
                                        isnull(Ltrim(Rtrim(cnt_firstName)),'')+' '+isnull(Ltrim(Rtrim(cnt_middleName)),'')
                                        +' '+isnull(Ltrim(Rtrim(cnt_lastName)),'')+'['+Ltrim(Rtrim(cnt_UCC))+']'+'['+Ltrim(Rtrim(crg_tcode))+']' ClientFullName,
                                        isnull(Ltrim(Rtrim(cnt_firstName)),'')FirstName,isnull(Ltrim(Rtrim(cnt_middleName)),'') MiddleName,
                                        isnull(Ltrim(Rtrim(cnt_lastName)),'') LastName,Ltrim(Rtrim(cnt_UCC)) UCC,Ltrim(Rtrim(crg_tcode)) TCode,
                                        LTRIM(Rtrim(cnt_internalId)) InternalID,cnt_id,crg_exchange from tbl_master_contact,tbl_master_contactExchange
                                        Where cnt_internalId=crg_cntID And Left(cnt_internalId,2)='CL' And
                                        crg_company='" + CompanyID + "' and crg_exchange in (" + SpecificSegment + @") Clients
                                        Inner Join 
                                        (Select isnull(Ltrim(Rtrim(gpm_Description)),'')+'['+isnull(Ltrim(Rtrim(gpm_code)),'NoCode')+']' GrpDetail,
                                        grp_contactId,gpm_code,grp_groupType,gpm_id from tbl_master_groupMaster,tbl_trans_group
                                        Where gpm_id=grp_groupMaster and 
                                        grp_groupType='" + GroupType + @"'
                                        and grp_groupMaster in (" + SpecificGroup + @")) [Group]
                                        on InternalID=grp_contactId) as T";
            string strQuery_FieldName = "Top 10  *";
            string strQuery_WhereClause = @"FirstName like '%RequestLetter%' Or MiddleName like '%RequestLetter%' Or LastName like '%RequestLetter%'
                                                or UCC like '%RequestLetter%' or TCode like '%RequestLetter%' Or TextField like '%RequestLetter%'";
            string strQuery_OrderBy = "TextField";
            string strQuery_GroupBy = "";
            return ReturnCombinedQuery(strQuery_Table, strQuery_FieldName, strQuery_WhereClause, strQuery_GroupBy, strQuery_OrderBy);
        }

        string ReturnCombinedQuery(string strQuery_Table, string strQuery_FieldName, string strQuery_WhereClause, string strQuery_GroupBy, string strQuery_OrderBy)
        {
            string CombinedQuery = strQuery_Table + " $" + strQuery_FieldName + " $" + strQuery_WhereClause + " $" + strQuery_GroupBy + "$" + strQuery_OrderBy;
            CombinedQuery = CombinedQuery.Replace("'", "\\'");
            return CombinedQuery;
        }
        #endregion



        #endregion
    }
}
