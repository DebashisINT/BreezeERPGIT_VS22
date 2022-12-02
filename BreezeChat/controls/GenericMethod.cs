using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace BreezeChat.controls
{
    public class GenericMethod
    {
        string strAppConnection = String.Empty;

        #region Properties
        public string _FinYear
        {
            get { return strAppConnection; }
            set { strAppConnection = value; }
        }
        #endregion
        SqlConnection oSqlConnection = new SqlConnection();
        GenericStoreProcedure oGenericStoreProcedure;
        GenericMethod oGenericMethod;
        //ZipFile zip;
        public enum ZipFileType { Folder, File };
        public enum WhichCall { DistroyUnWantedSession_AllExceptPage, DistroyUnWantedSession_Page, DistroyUnWantedSession_All };
        public enum ClientIDType { InternalID, UCC, TCode };
        public enum DateConvertFrom
        {
            UTCToDateTime, UTCToOnlyDate, UTCToOnlyTime, UTCToDateTimeHour, UTCToDateTimeHourSecond, UTCToDateTimeHourSecondMiliSecond
        }
        public GenericMethod()
        {
            //strAppConnection = ConfigurationSettings.AppSettings["DBConnectionDefault"]; MULTI

            //strAppConnection = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);

            // added for read only user
            //if (Convert.ToString(HttpContext.Current.Session["EntryProfileType1"]) != null)
            //{
            //    if (Convert.ToString(HttpContext.Current.Session["EntryProfileType1"]) == "R")
            //    {
            //        strAppConnection = ConfigurationSettings.AppSettings["DBReadOnlyConnection"];
            //    }              
            //}


        }




        public GenericMethod(string strDataSource, string strInitialCatalog, string strUID, string strPwd, string strPooling, string strPoolSize)
        {
            strAppConnection = "Data Source=" + strDataSource + ";Initial Catalog=" + strInitialCatalog + ";User ID=" + strUID + "; Password=" + strPwd + ";pooling='" + strPooling + "';Max Pool Size=" + strPoolSize + "";
            // added for read only user
            //if (HttpContext.Current.Session["EntryProfileType"] != null)
            //{
            //    if (Convert.ToString(HttpContext.Current.Session["EntryProfileType"]) == "R")
            //    {
            //        strAppConnection = ConfigurationSettings.AppSettings["DBReadOnlyConnection"];
            //    }
            //}

        }
        #region AjaxMethods

        #region Asset And Derivatives On Asset Selection Relavant
        // 'Param_ExchangeSegmentID' : If You Want To Find Specific ExchangeSegment Assets / Pass 0 in Other Case
        // 'SpecificExchange' : In Case Of INMX You Need To Pass a ExchangeID (User Input)/ Pass 0 in Other Case
        // 'SeriesIdentifier' : Pass "NA" When Want To Show All Type Of Identifier;
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

        //Finding DerivativeOnAsset Indicator Belongs Product Table
        //Description About ProductName and Producttype_Name
        public String GetUnderLyingAssets(int ExchangeSegmentID)
        {
            string ProductTypeID = String.Empty;

            if (ExchangeSegmentID == 0)
                ExchangeSegmentID = Convert.ToInt32(HttpContext.Current.Session["ExchangeSegmentID"].ToString());

            if (ExchangeSegmentID == 1 || ExchangeSegmentID == 4 || ExchangeSegmentID == 15 || ExchangeSegmentID == 19)//for CM
            {
                ProductTypeID = "1";
            }
            if (ExchangeSegmentID == 2 || ExchangeSegmentID == 5 || ExchangeSegmentID == 20)//for FO
            {
                ProductTypeID = "1,5";
            }
            if (ExchangeSegmentID == 3 || ExchangeSegmentID == 6 || ExchangeSegmentID == 8 || ExchangeSegmentID == 13)//for CDX
            {
                ProductTypeID = "8,21";
            }
            if (ExchangeSegmentID == 14)// For SPOT
            {
                ProductTypeID = "10";
            }
            if (ExchangeSegmentID == 7 || ExchangeSegmentID == 9 || ExchangeSegmentID == 10 || ExchangeSegmentID == 11
                || ExchangeSegmentID == 12 || ExchangeSegmentID == 17 || ExchangeSegmentID == 18 || ExchangeSegmentID == 21 || ExchangeSegmentID == 22 || ExchangeSegmentID == 23 || ExchangeSegmentID == 24 || ExchangeSegmentID == 25 || ExchangeSegmentID == 26 || ExchangeSegmentID == 27)//for COMM
            {
                if (ExchangeSegmentID == 10 || ExchangeSegmentID == 18 || ExchangeSegmentID == 21 || ExchangeSegmentID == 22 || ExchangeSegmentID == 23 || ExchangeSegmentID == 25 || ExchangeSegmentID == 26 || ExchangeSegmentID == 27)// INMX/DGCX//INSX/INFX/INBX/INAX/INEX
                {
                    ProductTypeID = "5,8,10,21";
                }
                else
                {
                    ProductTypeID = "10";
                }
            }

            string strQuery_Table = "Master_Products";
            string strQuery_FieldName = @"top 10 Ltrim(Rtrim(Products_Name)) +' ['+ (Select Ltrim(Rtrim(ProductType_Name)) from Master_ProductTypes Where ProductType_ID=Products_ProductTypeID)+']' as TextField,Cast(Products_ID as Varchar(20)) ValueField,Products_Name";
            string strQuery_WhereClause = "Products_ProductTypeID in (" + ProductTypeID + @") And Products_Name Like 'RequestLetter%'";
            string strQuery_OrderBy = null;
            string strQuery_GroupBy = null;
            return ReturnCombinedQuery(strQuery_Table, strQuery_FieldName, strQuery_WhereClause, strQuery_GroupBy, strQuery_OrderBy, "Asset");

        }

        //Finding Asset Or Derivative Indicator Belongs Product Table
        //Description About ProductName and ShortName
        public String GetUnderLyingAssets(string AssetOrDerivativeIndicator, int ExchangeSegmentID)
        {
            string ProductTypeID = String.Empty;

            if (AssetOrDerivativeIndicator == "A")
            {
                if (ExchangeSegmentID == 0)
                    ExchangeSegmentID = Convert.ToInt32(HttpContext.Current.Session["ExchangeSegmentID"].ToString());

                if (ExchangeSegmentID == 1 || ExchangeSegmentID == 4 || ExchangeSegmentID == 15 || ExchangeSegmentID == 19)//for CM
                {
                    ProductTypeID = "1";
                }
                if (ExchangeSegmentID == 2 || ExchangeSegmentID == 5 || ExchangeSegmentID == 20)//for FO
                {
                    ProductTypeID = "5";
                }
                if (ExchangeSegmentID == 3 || ExchangeSegmentID == 6 || ExchangeSegmentID == 8 || ExchangeSegmentID == 13)//for CDX
                {
                    ProductTypeID = "8,21";
                }
                if (ExchangeSegmentID == 14)// For SPOT
                {
                    ProductTypeID = "10";
                }
                if (ExchangeSegmentID == 7 || ExchangeSegmentID == 9 || ExchangeSegmentID == 10 || ExchangeSegmentID == 11
                    || ExchangeSegmentID == 12 || ExchangeSegmentID == 17 || ExchangeSegmentID == 18 || ExchangeSegmentID == 21 || ExchangeSegmentID == 22 || ExchangeSegmentID == 23 || ExchangeSegmentID == 24 || ExchangeSegmentID == 25 || ExchangeSegmentID == 26 || ExchangeSegmentID == 27)//for COMM
                {
                    if (ExchangeSegmentID == 10 || ExchangeSegmentID == 18 || ExchangeSegmentID == 21 || ExchangeSegmentID == 22 || ExchangeSegmentID == 23 || ExchangeSegmentID == 25 || ExchangeSegmentID == 26 || ExchangeSegmentID == 27)// INMX/DGCX/INSX/INFX/INBX/INAX/INEX
                    {
                        ProductTypeID = "5,8,10,21";
                    }
                    else
                    {
                        ProductTypeID = "10";
                    }
                }
            }
            else if (AssetOrDerivativeIndicator == "D")
            {
                if (ExchangeSegmentID == 0)
                    ExchangeSegmentID = Convert.ToInt32(HttpContext.Current.Session["ExchangeSegmentID"].ToString());

                if (ExchangeSegmentID == 1 || ExchangeSegmentID == 4 || ExchangeSegmentID == 15 || ExchangeSegmentID == 19)//for CM
                {
                    ProductTypeID = "1";
                }
                if (ExchangeSegmentID == 2 || ExchangeSegmentID == 5 || ExchangeSegmentID == 20)//for FO
                {
                    ProductTypeID = "4,6";
                }
                if (ExchangeSegmentID == 3 || ExchangeSegmentID == 6 || ExchangeSegmentID == 8 || ExchangeSegmentID == 13)//for CDX
                {
                    ProductTypeID = "9";
                }
                if (ExchangeSegmentID == 14)// For SPOT
                {
                    ProductTypeID = "11";
                }
                if (ExchangeSegmentID == 7 || ExchangeSegmentID == 9 || ExchangeSegmentID == 10 || ExchangeSegmentID == 11
                    || ExchangeSegmentID == 12 || ExchangeSegmentID == 17 || ExchangeSegmentID == 18 || ExchangeSegmentID == 21 || ExchangeSegmentID == 22 || ExchangeSegmentID == 23 || ExchangeSegmentID == 24 || ExchangeSegmentID == 25 || ExchangeSegmentID == 26 || ExchangeSegmentID == 27)//for COMM
                {
                    if (ExchangeSegmentID == 10 || ExchangeSegmentID == 18 || ExchangeSegmentID == 21 || ExchangeSegmentID == 22 || ExchangeSegmentID == 23 || ExchangeSegmentID == 25 || ExchangeSegmentID == 26 || ExchangeSegmentID == 27)// INMX/DGCX/INSX/INFX/INBX/INAX/INEX
                    {
                        ProductTypeID = "6,9,11";
                    }
                    else
                    {
                        ProductTypeID = "11";
                    }
                }
            }
            else if (AssetOrDerivativeIndicator == "AD")
            {
                if (ExchangeSegmentID == 0)
                    ExchangeSegmentID = Convert.ToInt32(HttpContext.Current.Session["ExchangeSegmentID"].ToString());

                if (ExchangeSegmentID == 1 || ExchangeSegmentID == 4 || ExchangeSegmentID == 15 || ExchangeSegmentID == 19)//for CM
                {
                    ProductTypeID = "1";
                }
                if (ExchangeSegmentID == 2 || ExchangeSegmentID == 5 || ExchangeSegmentID == 20)//for FO
                {
                    ProductTypeID = "1,4,5,6";
                }
                if (ExchangeSegmentID == 3 || ExchangeSegmentID == 6 || ExchangeSegmentID == 8 || ExchangeSegmentID == 13)//for CDX
                {
                    ProductTypeID = "8,9,21,13";
                }
                if (ExchangeSegmentID == 14)// For SPOT
                {
                    ProductTypeID = "10,11";
                }
                if (ExchangeSegmentID == 7 || ExchangeSegmentID == 9 || ExchangeSegmentID == 10 || ExchangeSegmentID == 11
                    || ExchangeSegmentID == 12 || ExchangeSegmentID == 17 || ExchangeSegmentID == 18 || ExchangeSegmentID == 21 || ExchangeSegmentID == 22 || ExchangeSegmentID == 23 || ExchangeSegmentID == 24 || ExchangeSegmentID == 25 || ExchangeSegmentID == 26 || ExchangeSegmentID == 27)//for COMM
                {
                    if (ExchangeSegmentID == 10 || ExchangeSegmentID == 18 || ExchangeSegmentID == 21 || ExchangeSegmentID == 22 || ExchangeSegmentID == 23 || ExchangeSegmentID == 25 || ExchangeSegmentID == 26 || ExchangeSegmentID == 27)// INMX/DGCX/INSX/INFX/INBX/INAX/INEX
                    {
                        ProductTypeID = "5,6,8,9,10,11,21,13";
                    }
                    else
                    {
                        ProductTypeID = "10,11";
                    }
                }
            }

            string strQuery_Table = "Master_Products";
            string strQuery_FieldName = @"top 10 Ltrim(Rtrim(Products_Name)) +' ['+ Ltrim(Rtrim(Products_ShortName)) +']' as TextFeild,Cast(Products_ID as Varchar(10)) ValueField,Products_Name";
            string strQuery_WhereClause = "Products_ProductTypeID in (" + ProductTypeID + @") And (Products_Name Like '%RequestLetter%' Or Products_ShortName Like '%RequestLetter%')";
            string strQuery_OrderBy = null;
            string strQuery_GroupBy = null;

            return ReturnCombinedQuery(strQuery_Table, strQuery_FieldName, strQuery_WhereClause, strQuery_GroupBy, strQuery_OrderBy, "Asset");
        }

        public String GetDerivativeOnAssets(int ExchangeSegmentID, int SpecificExchange, string SeriesIdentifier)
        // 'SpecificExchange' : In Case Of INMX You Need To Pass a ExchangeID (User Input)/ Pass 0 in Other Case
        // ''SeriesIdentifier' :In Case When User Want To See Equity,Bond,Future,Option Kinda Specific Product / Pass 0 in Other Case
        {
            string ProductTypeID = String.Empty;
            string strQuery_Table = String.Empty;
            string strQuery_FieldName = String.Empty;
            string strQuery_WhereClause = String.Empty;
            string strQuery_OrderBy = null;
            string strQuery_GroupBy = null;

            if (ExchangeSegmentID == 0)
                ExchangeSegmentID = Convert.ToInt32(HttpContext.Current.Session["ExchangeSegmentID"].ToString());

            if (ExchangeSegmentID == 1 || ExchangeSegmentID == 4 || ExchangeSegmentID == 19)//For CM
            {
                strQuery_Table = "Master_Equity";
                if (ExchangeSegmentID == 1 || ExchangeSegmentID == 19)
                    strQuery_FieldName = @"Top 10 Ltrim(Rtrim(isnull(Equity_TickerSymbol,'')))+' [' + Ltrim(Rtrim(isnull(Equity_Series,'')))+']' EquityDetail,Equity_SeriesID ,Equity_TickerSymbol,Equity_TickerCode";
                else
                    strQuery_FieldName = @"Top 10 Ltrim(Rtrim(isnull(Equity_TickerSymbol,'')))+'[ ' + Ltrim(Rtrim(isnull(Equity_TickerCode,'')))+' ]' EquityDetail,Equity_SeriesID,Equity_TickerSymbol,Equity_TickerCode";

                if (SeriesIdentifier.Trim() == "NA")
                    strQuery_WhereClause = @"Equity_ExchSegmentID=" + ExchangeSegmentID.ToString() + " And (Equity_TickerSymbol Like 'RequestLetter%' or Equity_TickerCode Like 'RequestLetter%')";
                else
                    strQuery_WhereClause = @"Equity_ExchSegmentID=" + ExchangeSegmentID.ToString() + "And Equity_Series='" + SeriesIdentifier + "' And (Equity_TickerSymbol Like 'RequestLetter%' or Equity_TickerCode Like 'RequestLetter%')";

            }

            if (ExchangeSegmentID == 2 || ExchangeSegmentID == 5 || ExchangeSegmentID == 20)//for FO
            {
                strQuery_Table = "Master_Equity";
                strQuery_FieldName = @"Top 10 * from (select (case when Equity_StrikePrice=0.0 then isnull(rtrim(ltrim(Equity_TickerSymbol)),'')+' '+isnull(rtrim(ltrim(Equity_Series)),'')+' '+convert(varchar(9),Equity_EffectUntil,6) else isnull(rtrim(ltrim(Equity_TickerSymbol)),'')+' '+isnull(rtrim(ltrim(Equity_Series)),'')+' '+convert(varchar(9),Equity_EffectUntil,6)+' '+cast(cast(round(Equity_StrikePrice,2) as numeric(28,2)) as varchar) end) as TickerSymbol,Equity_SeriesID,Equity_StrikePrice,Equity_TickerSymbol,Equity_Series,Equity_EffectUntil";
                if (SeriesIdentifier.Trim() == "NA")
                    strQuery_WhereClause = @"Equity_ExchSegmentID=" + ExchangeSegmentID.ToString() + ") as T1 Where (Equity_SeriesID Like 'RequestLetter%' or Equity_StrikePrice Like 'RequestLetter%' Or Equity_TickerSymbol Like 'RequestLetter%' Or Equity_Series Like 'RequestLetter%' Or convert(varchar(9),Equity_EffectUntil,6) Like 'RequestLetter%' Or TickerSymbol Like 'RequestLetter%')";
                else
                {
                    if (SeriesIdentifier.Trim() == "FUT" || SeriesIdentifier.Trim() == "OPT")
                        strQuery_WhereClause = @"Equity_ExchSegmentID=" + ExchangeSegmentID.ToString() + "And Left(Equity_FOIdentifier,3)='" + SeriesIdentifier + "') as T1 Where (Equity_SeriesID Like 'RequestLetter%' or Equity_StrikePrice Like 'RequestLetter%' Or Equity_TickerSymbol Like 'RequestLetter%' Or Equity_Series Like 'RequestLetter%' Or convert(varchar(9),Equity_EffectUntil,6) Like 'RequestLetter%' Or TickerSymbol Like 'RequestLetter%')";
                    else
                        strQuery_WhereClause = @"Equity_ExchSegmentID=" + ExchangeSegmentID.ToString() + "And Left(Equity_FOIdentifier,3)='" + SeriesIdentifier + "') as T1 Where (Equity_SeriesID Like 'RequestLetter%' or Equity_StrikePrice Like 'RequestLetter%' Or Equity_TickerSymbol Like 'RequestLetter%' Or Equity_Series Like 'RequestLetter%' Or convert(varchar(9),Equity_EffectUntil,6) Like 'RequestLetter% Or TickerSymbol Like 'RequestLetter%')";
                }
            }
            if (ExchangeSegmentID == 3 || ExchangeSegmentID == 6 || ExchangeSegmentID == 8 || ExchangeSegmentID == 13 //For CDX
                || ExchangeSegmentID == 14 // For SPOT
                || ExchangeSegmentID == 7 || ExchangeSegmentID == 9 || ExchangeSegmentID == 10 || ExchangeSegmentID == 11
                || ExchangeSegmentID == 12 || ExchangeSegmentID == 17 || ExchangeSegmentID == 18 || ExchangeSegmentID == 21 || ExchangeSegmentID == 22 || ExchangeSegmentID == 23 || ExchangeSegmentID == 24 || ExchangeSegmentID == 25 || ExchangeSegmentID == 26 || ExchangeSegmentID == 27)//for COMM
            {
                if (ExchangeSegmentID == 18 || ExchangeSegmentID == 21 || ExchangeSegmentID == 22 || ExchangeSegmentID == 23 || ExchangeSegmentID == 25 || ExchangeSegmentID == 26 || ExchangeSegmentID == 27)// INMX/DGCX/INSX/INFX/INBX/INAX/INEX
                {
                    string SeriesIdentifier_WhereQueryPart = String.Empty;
                    string SpecificExchange_WhereQueryPart = String.Empty;
                    if (SeriesIdentifier.Trim() != "NA")
                        SeriesIdentifier_WhereQueryPart = " And Left(Commodity_Identifier,3)='" + SeriesIdentifier + "'";
                    if (SpecificExchange != 0)
                        SpecificExchange_WhereQueryPart = " And Commodity_Exchange=" + SpecificExchange.ToString();


                    strQuery_Table = "master_commodity";
                    strQuery_FieldName = @"Top 10 * From (Select (ltrim(rtrim(isnull(Commodity_TickerSymbol,'')))+' '+ ltrim(rtrim(isnull(Commodity_TickerSeries,'')))+' '+cast(cast(isnull(Commodity_StrikePrice,0.00) as numeric(16,2)) as varchar)+' '+ convert(varchar(11),Commodity_ExpiryDate,113)) as TextField,ltrim(rtrim(Commodity_ProductSeriesID)) ValueField,Commodity_TickerSymbol,Commodity_Identifier,Commodity_TickerSeries,Commodity_StrikePrice,convert(varchar(11),Commodity_ExpiryDate,113) Commodity_ExpiryDate";
                    strQuery_WhereClause = "(Commodity_ExchangeSegmentID=" + ExchangeSegmentID.ToString() + SeriesIdentifier_WhereQueryPart + SpecificExchange_WhereQueryPart + ")) as T1 Where (TextField Like 'RequestLetter%' Or Commodity_TickerSymbol Like 'RequestLetter%' Or Commodity_Identifier Like 'RequestLetter%' Or Commodity_TickerSeries Like 'RequestLetter%' Or Commodity_StrikePrice Like 'RequestLetter%' Or Commodity_ExpiryDate Like 'RequestLetter%')";

                }
                else
                {
                    strQuery_Table = "master_commodity";
                    strQuery_FieldName = @"Top 10 * From (Select (ltrim(rtrim(isnull(Commodity_TickerSymbol,'')))+' '+ ltrim(rtrim(isnull(Commodity_TickerSeries,'')))+' '+cast(cast(isnull(Commodity_StrikePrice,0.00) as numeric(16,2)) as varchar)+' '+ convert(varchar(11),Commodity_ExpiryDate,113)) as TextField,ltrim(rtrim(Commodity_ProductSeriesID)) ValueField, Commodity_TickerSymbol,Commodity_Identifier,Commodity_TickerSeries,Commodity_StrikePrice, convert(varchar(11),Commodity_ExpiryDate,113) Commodity_ExpiryDate";
                    if (SeriesIdentifier.Trim() == "NA")
                        strQuery_WhereClause = "(Commodity_ExchangeSegmentID=" + ExchangeSegmentID.ToString() + @")) as T1 Where (TextField Like 'RequestLetter%' Or Commodity_TickerSymbol Like 'RequestLetter%' Or Commodity_Identifier Like 'RequestLetter%' Or Commodity_TickerSeries Like 'RequestLetter%' Or Commodity_StrikePrice Like 'RequestLetter%' Or convert(varchar(11),Commodity_ExpiryDate,113) Like 'RequestLetter%')";
                    else
                        strQuery_WhereClause = "(Commodity_ExchangeSegmentID=" + ExchangeSegmentID.ToString() + @" And Left(Commodity_Identifier,3)='" + SeriesIdentifier + "')) as T1 Where (TextField Like 'RequestLetter%' Or Commodity_TickerSymbol Like 'RequestLetter%' Or Commodity_Identifier Like 'RequestLetter%' Or Commodity_TickerSeries Like 'RequestLetter%' Or Commodity_StrikePrice Like 'RequestLetter%' Or convert(varchar(11),Commodity_ExpiryDate,113) Like 'RequestLetter%')";

                }
            }

            return ReturnCombinedQuery(strQuery_Table, strQuery_FieldName, strQuery_WhereClause, strQuery_GroupBy, strQuery_OrderBy, "Derivative");
        }
        /////Get Derivative Which Expiry Or EffectUntil Date is Equel and Greater Than SpecificDate
        public String GetDerivativeOnAssets(int ExchangeSegmentID, int SpecificExchange, string SeriesIdentifier, DateTime ExpiryOrEffectUntilDate)
        // 'SpecificExchange' : In Case Of INMX You Need To Pass a ExchangeID (User Input)/ Pass 0 in Other Case
        // ''SeriesIdentifier' :In Case When User Want To See Equity,Bond,Future,Option Kinda Specific Product / Pass "NA" in Other Case
        // ExpiryOrEffectUntilDate For Showing Derivative Equal and Greater Than This Date
        {
            string ProductTypeID = String.Empty;
            string strQuery_Table = String.Empty;
            string strQuery_FieldName = String.Empty;
            string strQuery_WhereClause = String.Empty;
            string strQuery_OrderBy = null;
            string strQuery_GroupBy = null;

            if (ExchangeSegmentID == 0)
                ExchangeSegmentID = Convert.ToInt32(HttpContext.Current.Session["ExchangeSegmentID"].ToString());

            if (ExchangeSegmentID == 1 || ExchangeSegmentID == 4 || ExchangeSegmentID == 19)//For CM
            {
                strQuery_Table = "Master_Equity";
                if (ExchangeSegmentID == 1 || ExchangeSegmentID == 19)
                    strQuery_FieldName = @"Top 10 Ltrim(Rtrim(isnull(Equity_TickerSymbol,'')))+'[ ' + Ltrim(Rtrim(isnull(Equity_Series,'')))+']' EquityDetail,Equity_SeriesID ,Equity_TickerSymbol,Equity_TickerCode";
                else
                    strQuery_FieldName = @"Top 10 Ltrim(Rtrim(isnull(Equity_TickerSymbol,'')))+'[ ' + Ltrim(Rtrim(isnull(Equity_TickerCode,'')))+' ]' EquityDetail,Equity_SeriesID,Equity_TickerSymbol,Equity_TickerCode";

                if (SeriesIdentifier.Trim() == "NA")
                    strQuery_WhereClause = @"Equity_ExchSegmentID=" + ExchangeSegmentID.ToString() + " And (Equity_TickerSymbol Like 'RequestLetter%' or Equity_TickerCode Like 'RequestLetter%')";
                else
                    strQuery_WhereClause = @"Equity_ExchSegmentID=" + ExchangeSegmentID.ToString() + "And Equity_Series='" + SeriesIdentifier + "' And (Equity_TickerSymbol Like 'RequestLetter%' or Equity_TickerCode Like 'RequestLetter%')";

            }

            if (ExchangeSegmentID == 2 || ExchangeSegmentID == 5 || ExchangeSegmentID == 20)//for FO
            {
                strQuery_Table = "Master_Equity";
                strQuery_FieldName = @"Top 10 TickerSymbol,Equity_SeriesID from (select (case when Equity_StrikePrice=0.0 then isnull(rtrim(ltrim(Equity_TickerSymbol)),'')+' '+isnull(rtrim(ltrim(Equity_Series)),'')+' '+convert(varchar(9),Equity_EffectUntil,6) else isnull(rtrim(ltrim(Equity_TickerSymbol)),'')+' '+isnull(rtrim(ltrim(Equity_Series)),'')+' '+convert(varchar(9),Equity_EffectUntil,6)+' '+cast(cast(round(Equity_StrikePrice,2) as numeric(28,2)) as varchar) end) as TickerSymbol,Equity_SeriesID,Equity_StrikePrice,Equity_TickerSymbol,Equity_Series,Equity_EffectUntil";
                if (SeriesIdentifier.Trim() == "NA")
                    strQuery_WhereClause = @"Equity_ExchSegmentID=" + ExchangeSegmentID.ToString() + "And  Equity_EffectUntil>='" + ExpiryOrEffectUntilDate.ToString("yyyy-MM-dd") + "') as T1 Where  (Equity_SeriesID Like 'RequestLetter%' or Equity_StrikePrice Like 'RequestLetter%' Or Equity_TickerSymbol Like 'RequestLetter%' Or Equity_Series Like 'RequestLetter%' Or convert(varchar(9),Equity_EffectUntil,6) Like 'RequestLetter%' Or TickerSymbol Like 'RequestLetter%')";
                else
                {
                    if (SeriesIdentifier.Trim() == "FUT" || SeriesIdentifier.Trim() == "OPT")
                        strQuery_WhereClause = @"Equity_ExchSegmentID=" + ExchangeSegmentID.ToString() + "And  Equity_EffectUntil>='" + ExpiryOrEffectUntilDate.ToString("yyyy-MM-dd") + "' And Left(Equity_FOIdentifier,3)='" + SeriesIdentifier + "') as T1 Where (Equity_SeriesID Like 'RequestLetter%' or Equity_StrikePrice Like 'RequestLetter%' Or Equity_TickerSymbol Like 'RequestLetter%' Or Equity_Series Like 'RequestLetter%' Or convert(varchar(9),Equity_EffectUntil,6) Like 'RequestLetter%')";
                    else
                        strQuery_WhereClause = @"Equity_ExchSegmentID=" + ExchangeSegmentID.ToString() + "And  Equity_EffectUntil>='" + ExpiryOrEffectUntilDate.ToString("yyyy-MM-dd") + "' And Equity_FOIdentifier='" + SeriesIdentifier + "') as T1 Where  (Equity_SeriesID Like 'RequestLetter%' or Equity_StrikePrice Like 'RequestLetter%' Or Equity_TickerSymbol Like 'RequestLetter%' Or Equity_Series Like 'RequestLetter%' Or convert(varchar(9),Equity_EffectUntil,6) Like 'RequestLetter%' Or TickerSymbol Like 'RequestLetter%')";
                }
            }
            if (ExchangeSegmentID == 3 || ExchangeSegmentID == 6 || ExchangeSegmentID == 8 || ExchangeSegmentID == 13 //For CDX
                || ExchangeSegmentID == 14 // For SPOT
                || ExchangeSegmentID == 7 || ExchangeSegmentID == 9 || ExchangeSegmentID == 10 || ExchangeSegmentID == 11
                || ExchangeSegmentID == 12 || ExchangeSegmentID == 17 || ExchangeSegmentID == 18 || ExchangeSegmentID == 21 || ExchangeSegmentID == 22 || ExchangeSegmentID == 23 || ExchangeSegmentID == 24 || ExchangeSegmentID == 25 || ExchangeSegmentID == 26 || ExchangeSegmentID == 27)//for COMM
            {
                if (ExchangeSegmentID == 18 || ExchangeSegmentID == 21 || ExchangeSegmentID == 22 || ExchangeSegmentID == 23 || ExchangeSegmentID == 25 || ExchangeSegmentID == 26 || ExchangeSegmentID == 27)// INMX/DGCX/INSX/INFX/INBX/INAX/INEX
                {
                    string SeriesIdentifier_WhereQueryPart = String.Empty;
                    string SpecificExchange_WhereQueryPart = String.Empty;
                    if (SeriesIdentifier.Trim() != "NA")
                        SeriesIdentifier_WhereQueryPart = " And Left(Commodity_Identifier,3)='" + SeriesIdentifier + "'";
                    if (SpecificExchange != 0)
                        SpecificExchange_WhereQueryPart = " And Commodity_Exchange=" + SpecificExchange.ToString();


                    strQuery_Table = "master_commodity";
                    strQuery_FieldName = @"Top 10 * From (Select (ltrim(rtrim(isnull(Commodity_TickerSymbol,'')))+' '+ltrim(rtrim(isnull(Commodity_TickerSeries,'')))+' '+cast(cast(isnull(Commodity_StrikePrice,0.00) as numeric(16,2)) as varchar)+' '+convert(varchar(11),Commodity_ExpiryDate,113)) as Commodity_Product,ltrim(rtrim(Commodity_ProductSeriesID)) as Commodity_ProductSeriesID,Commodity_TickerSymbol,Commodity_Identifier,Commodity_TickerSeries,Commodity_StrikePrice,convert(varchar(11),Commodity_ExpiryDate,113) Commodity_ExpiryDate";
                    strQuery_WhereClause = "(Commodity_ExchangeSegmentID=" + ExchangeSegmentID.ToString() + SeriesIdentifier_WhereQueryPart + SpecificExchange_WhereQueryPart + ") And Commodity_ExpiryDate>='" + ExpiryOrEffectUntilDate.ToString("yyyy-MM-dd") + @"') as T1 Where (Commodity_TickerSymbol Like 'RequestLetter%' Or Commodity_Identifier Like 'RequestLetter%' Or Commodity_TickerSeries Like 'RequestLetter%' Or Commodity_StrikePrice Like 'RequestLetter%' Or Commodity_ExpiryDate Like 'RequestLetter%' Or Commodity_Product Like 'RequestLetter%')";

                }
                else
                {
                    strQuery_Table = "master_commodity";
                    strQuery_FieldName = @"Top 10 * From (Select (ltrim(rtrim(isnull(Commodity_TickerSymbol,'')))+' '+ltrim(rtrim(isnull(Commodity_TickerSeries,'')))+' '+cast(cast(isnull(Commodity_StrikePrice,0.00) as numeric(16,2)) as varchar)+' '+convert(varchar(11),Commodity_ExpiryDate,113)) as Commodity_Product,ltrim(rtrim(Commodity_ProductSeriesID)) as Commodity_ProductSeriesID,Commodity_TickerSymbol,Commodity_Identifier,Commodity_TickerSeries,Commodity_StrikePrice,convert(varchar(11),Commodity_ExpiryDate,113) Commodity_ExpiryDate";
                    if (SeriesIdentifier.Trim() == "NA")
                        strQuery_WhereClause = "(Commodity_ExchangeSegmentID=" + ExchangeSegmentID.ToString() + @") And Commodity_ExpiryDate>='" + ExpiryOrEffectUntilDate.ToString("yyyy-MM-dd") + "') as T1 Where  (Commodity_TickerSymbol Like 'RequestLetter%' Or Commodity_Identifier Like 'RequestLetter%' Or Commodity_TickerSeries Like 'RequestLetter%' Or Commodity_StrikePrice Like 'RequestLetter%' Or convert(varchar(11),Commodity_ExpiryDate,113) Like 'RequestLetter%' Or Commodity_Product Like 'RequestLetter%')";
                    else
                        strQuery_WhereClause = "(Commodity_ExchangeSegmentID=" + ExchangeSegmentID.ToString() + @" And Left(Commodity_Identifier,3)='" + SeriesIdentifier + "') And Commodity_ExpiryDate>='" + ExpiryOrEffectUntilDate.ToString("yyyy-MM-dd") + "') as T1 Where (Commodity_TickerSymbol Like 'RequestLetter%' Or Commodity_Identifier Like 'RequestLetter%' Or Commodity_TickerSeries Like 'RequestLetter%' Or Commodity_StrikePrice Like 'RequestLetter%' Or convert(varchar(11),Commodity_ExpiryDate,113) Like 'RequestLetter%' Or Commodity_Product Like 'RequestLetter%')";

                }
            }

            return ReturnCombinedQuery(strQuery_Table, strQuery_FieldName, strQuery_WhereClause, strQuery_GroupBy, strQuery_OrderBy, "Asset");
        }
        public string GetRollingAssets()
        // Get All Products From RollingContracts
        {
            string strQuery_Table = " (select * From Master_Products Where Products_ID in (select RollingContracts_ProductID From Master_RollingContracts )) RollingAsset";
            string strQuery_FieldName = @" Top 10 ltrim(rtrim(Products_Name)) +' ['+ltrim(rtrim(isnull(Products_ShortName,'')))+']' as TextField,cast(Products_ID as varchar(20)) ValueField,Products_Name";
            string strQuery_WhereClause = " Products_Name Like 'RequestLetter%'";
            string strQuery_OrderBy = " TextField ";
            string strQuery_GroupBy = null;
            return ReturnCombinedQuery(strQuery_Table, strQuery_FieldName, strQuery_WhereClause, strQuery_GroupBy, strQuery_OrderBy, "RollingAsset");
        }

        public string GetRollingAssets(string SpecificSegment)
        // Get Specific Segment Wise Products From RollingContracts
        {
            string strQuery_Table = " (select * From Master_Products Where Products_ID in (select RollingContracts_ProductID From Master_RollingContracts where RollingContracts_ExchangeSegment=" + Convert.ToInt32(SpecificSegment) + ")) RollingAsset";
            string strQuery_FieldName = @" Top 10 ltrim(rtrim(Products_Name)) +' ['+ltrim(rtrim(isnull(Products_ShortName,'')))+']' as TextField,cast(Products_ID as varchar(20)) ValueField,Products_Name";
            string strQuery_WhereClause = " Products_Name Like 'RequestLetter%'";
            string strQuery_OrderBy = " TextField ";
            string strQuery_GroupBy = null;
            return ReturnCombinedQuery(strQuery_Table, strQuery_FieldName, strQuery_WhereClause, strQuery_GroupBy, strQuery_OrderBy, "RollingAsset");
        }
        #endregion

        #region Client/Employee/Broker Selection Relavant

        string strQuery_Table = String.Empty;
        string strQuery_FieldName = String.Empty;
        string strQuery_WhereClause = String.Empty;
        string strQuery_OrderBy = null;
        string strQuery_GroupBy = null;
        //ClientWise
        /// Prefix Like 'CL' For Client 'EM For Employee and So on .....
        /// 

        public void ReadOnlyConnection()
        {
            if (Convert.ToString(HttpContext.Current.Session["EntryProfileType"]) != null)
            {
                if (Convert.ToString(HttpContext.Current.Session["EntryProfileType"]) == "R")
                {
                    //strAppConnection = ConfigurationSettings.AppSettings["DBReadOnlyConnection"]; MULTI
                    // strAppConnection = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);

                }
            }
        }
        public string GetAllContact(string Prefix)
        {
            string strQuery_Table = "(select isnull(Ltrim(Rtrim(cnt_firstName)),'')+' '+isnull(Ltrim(Rtrim(cnt_middleName)),'') +' '+isnull(Ltrim(Rtrim(cnt_lastName)),'')+'['+Ltrim(Rtrim(cnt_UCC))+']' TextField,isnull(Ltrim(Rtrim(cnt_firstName)),'')+'~'+isnull(Ltrim(Rtrim(cnt_middleName)),'')+'~'+isnull(Ltrim(Rtrim(cnt_lastName)),'')+'~'+Ltrim(Rtrim(cnt_UCC))+'~'+LTRIM(Rtrim(cnt_internalId)) ValueField,cnt_id ID,cnt_firstName,cnt_middleName,cnt_lastName,cnt_UCC from tbl_master_contact Where Left(cnt_internalId,2)='" + Prefix + "') AllClient";
            string strQuery_FieldName = "Top 10  *";
            string strQuery_WhereClause = "cnt_firstName like '%RequestLetter%' Or cnt_middleName like '%RequestLetter%' Or cnt_lastName like '%RequestLetter%' or cnt_UCC like '%RequestLetter%' Or TextField like '%RequestLetter%'";
            string strQuery_OrderBy = "TextField";
            string strQuery_GroupBy = null;
            return ReturnCombinedQuery(strQuery_Table, strQuery_FieldName, strQuery_WhereClause, strQuery_GroupBy, strQuery_OrderBy, "Client");
        }

        //SegmentFilter
        ////Client Of Specific Company and All Segment
        public string GetClient_SegmentFilter(string CompanyID)
        {
            string strQuery_Table = @"(Select isnull(Ltrim(Rtrim(cnt_firstName)),'')+' '+isnull(Ltrim(Rtrim(cnt_middleName)),'')+' '+isnull(Ltrim(Rtrim(cnt_lastName)),'')+'['+Ltrim(Rtrim(cnt_UCC))+']'+'['+Ltrim(Rtrim(crg_tcode))+']' TextField,isnull(Ltrim(Rtrim(cnt_firstName)),'')+'~'+isnull(Ltrim(Rtrim(cnt_middleName)),'')+'~'+isnull(Ltrim(Rtrim(cnt_lastName)),'')+'~'+Ltrim(Rtrim(cnt_UCC))+','+Ltrim(Rtrim(crg_tcode))+'~'+LTRIM(Rtrim(cnt_internalId))+'~'+Cast(cnt_id as Varchar(50))+'~'+crg_exchange ValueField,isnull(Ltrim(Rtrim(cnt_firstName)),'') FirstName,isnull(Ltrim(Rtrim(cnt_middleName)),'') MiddleName,isnull(Ltrim(Rtrim(cnt_lastName)),'') LastName,Ltrim(Rtrim(cnt_UCC)) UCC,Ltrim(Rtrim(crg_tcode)) TCode,LTRIM(Rtrim(cnt_internalId)) InternalID,Cast(cnt_id as Varchar(50)) Cnt_ID,crg_exchange from tbl_master_contact,tbl_master_contactExchange Where cnt_internalId=crg_cntID And Left(cnt_internalId,2)='CL' And crg_company='" + CompanyID + "') SpecificCompNAllSegment";
            string strQuery_FieldName = "Top 10  *";
            string strQuery_WhereClause = @"FirstName like '%RequestLetter%' Or MiddleName like '%RequestLetter%' Or LastName like '%RequestLetter%' or UCC like '%RequestLetter%' or TCode like '%RequestLetter%' Or TextField like '%RequestLetter%'";
            string strQuery_OrderBy = "TextField";
            string strQuery_GroupBy = null;
            return ReturnCombinedQuery(strQuery_Table, strQuery_FieldName, strQuery_WhereClause, strQuery_GroupBy, strQuery_OrderBy, "Client");
        }
        ////Client Of Specific Company and Specific Segment
        public string GetClient_SegmentFilter(string CompanyID, string SpecificSegment)
        {
            string strQuery_Table = @"(Select isnull(Ltrim(Rtrim(cnt_firstName)),'')+' '+isnull(Ltrim(Rtrim(cnt_middleName)),'')+' '+isnull(Ltrim(Rtrim(cnt_lastName)),'')+'['+Ltrim(Rtrim(cnt_UCC))+']'+'['+Ltrim(Rtrim(crg_tcode))+']' TextField,isnull(Ltrim(Rtrim(cnt_firstName)),'')+'~'+isnull(Ltrim(Rtrim(cnt_middleName)),'')+'~'+isnull(Ltrim(Rtrim(cnt_lastName)),'')+'~'+Ltrim(Rtrim(cnt_UCC))+','+Ltrim(Rtrim(crg_tcode))+'~'+LTRIM(Rtrim(cnt_internalId))+'~'+Cast(cnt_id as Varchar(50))+'~'+crg_exchange ValueField,isnull(Ltrim(Rtrim(cnt_firstName)),'') FirstName,isnull(Ltrim(Rtrim(cnt_middleName)),'') MiddleName,isnull(Ltrim(Rtrim(cnt_lastName)),'') LastName,Ltrim(Rtrim(cnt_UCC)) UCC,Ltrim(Rtrim(crg_tcode)) TCode,LTRIM(Rtrim(cnt_internalId)) InternalID,Cast(cnt_id as Varchar(50)) Cnt_ID,crg_exchange from tbl_master_contact,tbl_master_contactExchange Where cnt_internalId=crg_cntID And Left(cnt_internalId,2)='CL' And crg_company='" + CompanyID + "' And crg_exchange in (" + SpecificSegment + ")) SpecificCompSpecificSeg";
            string strQuery_FieldName = "Top 10  *";
            string strQuery_WhereClause = @"FirstName like '%RequestLetter%' Or MiddleName like '%RequestLetter%' Or LastName like '%RequestLetter%' or UCC like '%RequestLetter%' or TCode like '%RequestLetter%' Or TextField like '%RequestLetter%'";
            string strQuery_OrderBy = "TextField";
            string strQuery_GroupBy = null;
            return ReturnCombinedQuery(strQuery_Table, strQuery_FieldName, strQuery_WhereClause, strQuery_GroupBy, strQuery_OrderBy, "Client");
        }


        ////Client Of Specific Company and Specific Segment and of Secific ClientType
        public string GetClient_SegmentFilter(string CompanyID, string SpecificSegment, string ClientType)
        {
            string strQuery_Table = @"(Select isnull(Ltrim(Rtrim(cnt_firstName)),'')+' '+isnull(Ltrim(Rtrim(cnt_middleName)),'')+' '+isnull(Ltrim(Rtrim(cnt_lastName)),'')+'['+Ltrim(Rtrim(cnt_UCC))+']'+'['+Ltrim(Rtrim(crg_tcode))+']' TextField,isnull(Ltrim(Rtrim(cnt_firstName)),'')+'~'+isnull(Ltrim(Rtrim(cnt_middleName)),'')+'~'+isnull(Ltrim(Rtrim(cnt_lastName)),'')+'~'+Ltrim(Rtrim(cnt_UCC))+','+Ltrim(Rtrim(crg_tcode))+'~'+LTRIM(Rtrim(cnt_internalId))+'~'+Cast(cnt_id as Varchar(50))+'~'+crg_exchange ValueField,isnull(Ltrim(Rtrim(cnt_firstName)),'') FirstName,isnull(Ltrim(Rtrim(cnt_middleName)),'') MiddleName,isnull(Ltrim(Rtrim(cnt_lastName)),'') LastName,Ltrim(Rtrim(cnt_UCC)) UCC,Ltrim(Rtrim(crg_tcode)) TCode,LTRIM(Rtrim(cnt_internalId)) InternalID,Cast(cnt_id as Varchar(50)) Cnt_ID,crg_exchange from tbl_master_contact,tbl_master_contactExchange Where cnt_internalId=crg_cntID And Left(cnt_internalId,2)='CL' And crg_company='" + CompanyID + "'And cnt_clienttype='" + ClientType + "' And crg_exchange in (" + SpecificSegment + ")) SpecificCompSpecificSeg";
            string strQuery_FieldName = "Top 10  *";
            string strQuery_WhereClause = @"FirstName like '%RequestLetter%' Or MiddleName like '%RequestLetter%' Or LastName like '%RequestLetter%' or UCC like '%RequestLetter%' or TCode like '%RequestLetter%' Or TextField like '%RequestLetter%'";
            string strQuery_OrderBy = "TextField";
            string strQuery_GroupBy = null;
            return ReturnCombinedQuery(strQuery_Table, strQuery_FieldName, strQuery_WhereClause, strQuery_GroupBy, strQuery_OrderBy, "Client");
        }


        /////Note : SpecificSegment string Will Be Like 'NSE - CM','BSE - CM'
        //BranchFilter
        ////Client Of Specific Company and All Branch
        public string GetClient_BranchFilter(string CompanyID)
        {
            string strQuery_Table = @"(Select isnull(Ltrim(Rtrim(cnt_firstName)),'')+' '+isnull(Ltrim(Rtrim(cnt_middleName)),'') +' '+isnull(Ltrim(Rtrim(cnt_lastName)),'')+'['+Ltrim(Rtrim(cnt_UCC))+']'+'['+Ltrim(Rtrim(crg_tcode))+']' TextField,isnull(Ltrim(Rtrim(cnt_firstName)),'')+'~'+isnull(Ltrim(Rtrim(cnt_middleName)),'')+'~'+isnull(Ltrim(Rtrim(cnt_lastName)),'')+'~'+Ltrim(Rtrim(cnt_UCC))+'~'+Ltrim(Rtrim(crg_tcode))+'~'+LTRIM(Rtrim(cnt_internalId))+'~'+Cast(cnt_id as Varchar(50))+'~'+crg_exchange ValueField,isnull(Ltrim(Rtrim(cnt_firstName)),'')FirstName,isnull(Ltrim(Rtrim(cnt_middleName)),'') MiddleName,isnull(Ltrim(Rtrim(cnt_lastName)),'') LastName,Ltrim(Rtrim(cnt_UCC)) UCC,Ltrim(Rtrim(crg_tcode)) TCode,LTRIM(Rtrim(cnt_internalId)) InternalID,cnt_id ID,crg_exchange from tbl_master_contact,tbl_master_contactExchange Where cnt_internalId=crg_cntID And Left(cnt_internalId,2)='CL' And crg_company='" + CompanyID + "') AllSegmentAndSpeficBranch";
            string strQuery_FieldName = "Top 10  *";
            string strQuery_WhereClause = @"FirstName like '%RequestLetter%' Or MiddleName like '%RequestLetter%' Or LastName like '%RequestLetter%' or UCC like '%RequestLetter%' or TCode like '%RequestLetter%' Or TextField like '%RequestLetter%'";
            string strQuery_OrderBy = "TextField";
            string strQuery_GroupBy = null;
            return ReturnCombinedQuery(strQuery_Table, strQuery_FieldName, strQuery_WhereClause, strQuery_GroupBy, strQuery_OrderBy, "Client");
        }
        ////Client Of Specific Company and Specific Branch
        public string GetClient_BranchFilter(string CompanyID, string SpecificBranch)
        {
            string strQuery_Table = @"(Select isnull(Ltrim(Rtrim(cnt_firstName)),'')+' '+isnull(Ltrim(Rtrim(cnt_middleName)),'') +' '+isnull(Ltrim(Rtrim(cnt_lastName)),'')+'['+Ltrim(Rtrim(cnt_UCC))+']'+'['+Ltrim(Rtrim(crg_tcode))+']' TextField,isnull(Ltrim(Rtrim(cnt_firstName)),'')+'~'+isnull(Ltrim(Rtrim(cnt_middleName)),'')+'~'+isnull(Ltrim(Rtrim(cnt_lastName)),'')+'~'+Ltrim(Rtrim(cnt_UCC))+'~'+Ltrim(Rtrim(crg_tcode))+'~'+LTRIM(Rtrim(cnt_internalId))+'~'+Cast(cnt_id as Varchar(50))+'~'+crg_exchange ValueField,isnull(Ltrim(Rtrim(cnt_firstName)),'')FirstName,isnull(Ltrim(Rtrim(cnt_middleName)),'') MiddleName,isnull(Ltrim(Rtrim(cnt_lastName)),'') LastName,Ltrim(Rtrim(cnt_UCC)) UCC,Ltrim(Rtrim(crg_tcode)) TCode,LTRIM(Rtrim(cnt_internalId)) InternalID,cnt_id ID,crg_exchange from tbl_master_contact,tbl_master_contactExchange Where cnt_internalId=crg_cntID And Left(cnt_internalId,2)='CL' And crg_company='" + CompanyID + "' and cnt_branchid in (" + SpecificBranch + ")) AllSegmentAndSpeficBranch";
            string strQuery_FieldName = "Top 10  *";
            string strQuery_WhereClause = @"FirstName like '%RequestLetter%' Or MiddleName like '%RequestLetter%' Or LastName like '%RequestLetter%' or UCC like '%RequestLetter%' or TCode like '%RequestLetter%' Or TextField like '%RequestLetter%'";
            string strQuery_OrderBy = "TextField";
            string strQuery_GroupBy = null;
            return ReturnCombinedQuery(strQuery_Table, strQuery_FieldName, strQuery_WhereClause, strQuery_GroupBy, strQuery_OrderBy, "Client");
        }
        ////Client Of Specific Company and Specific Segment and Specific Branch
        public string GetClient_BranchFilter(string CompanyID, string SpecificSegment, string SpecificBranch)
        {
            string strQuery_Table = @"(Select isnull(Ltrim(Rtrim(cnt_firstName)),'')+' '+isnull(Ltrim(Rtrim(cnt_middleName)),'') +' '+isnull(Ltrim(Rtrim(cnt_lastName)),'')+'['+Ltrim(Rtrim(cnt_UCC))+']'+'['+Ltrim(Rtrim(crg_tcode))+']' TextField,isnull(Ltrim(Rtrim(cnt_firstName)),'')+'~'+isnull(Ltrim(Rtrim(cnt_middleName)),'')+'~'+isnull(Ltrim(Rtrim(cnt_lastName)),'')+'~'+Ltrim(Rtrim(cnt_UCC))+'~'+Ltrim(Rtrim(crg_tcode))+'~'+LTRIM(Rtrim(cnt_internalId))+'~'+Cast(cnt_id as Varchar(50))+'~'+crg_exchange ValueField,isnull(Ltrim(Rtrim(cnt_firstName)),'')FirstName,isnull(Ltrim(Rtrim(cnt_middleName)),'') MiddleName,isnull(Ltrim(Rtrim(cnt_lastName)),'') LastName,Ltrim(Rtrim(cnt_UCC)) UCC,Ltrim(Rtrim(crg_tcode)) TCode,LTRIM(Rtrim(cnt_internalId)) InternalID,cnt_id ID,crg_exchange from tbl_master_contact,tbl_master_contactExchange Where cnt_internalId=crg_cntID And Left(cnt_internalId,2)='CL' And crg_company='" + CompanyID + "' and crg_exchange in (" + SpecificSegment + ")  and cnt_branchid in (" + SpecificBranch + ")) AllSegmentAndSpeficBranch";
            string strQuery_FieldName = "Top 10  *";
            string strQuery_WhereClause = @"FirstName like '%RequestLetter%' Or MiddleName like '%RequestLetter%' Or LastName like '%RequestLetter%' or UCC like '%RequestLetter%' or TCode like '%RequestLetter%' Or TextField like '%RequestLetter%'";
            string strQuery_OrderBy = "TextField";
            string strQuery_GroupBy = null;
            return ReturnCombinedQuery(strQuery_Table, strQuery_FieldName, strQuery_WhereClause, strQuery_GroupBy, strQuery_OrderBy, "Client");
        }

        //GroupFilter

        //// All Client Of Specific Company and Specific Segment and ALL Group
        public string GetClient_GroupFilter(string CompanyID, string SpecificSegment, string GroupType)
        {
            string strQuery_Table = @"(Select  ClientFullName TextField,FirstName+'~'+MiddleName+'~'+LastName+'~'+UCC+'~'+TCode+'~'+InternalID+'~'+Cast(cnt_ID as Varchar(50))+'~'+crg_exchange+'~'+GrpDetail+'~'+gpm_code+'~'+grp_groupType+'~'+Cast(gpm_id as Varchar(50)) ValueField,FirstName,MiddleName,LastName,UCC,TCode,InternalID,cnt_ID,crg_exchange,GrpDetail,gpm_code,grp_groupType,gpm_id From (Select isnull(Ltrim(Rtrim(cnt_firstName)),'')+' '+isnull(Ltrim(Rtrim(cnt_middleName)),'')+' '+isnull(Ltrim(Rtrim(cnt_lastName)),'')+'['+Ltrim(Rtrim(cnt_UCC))+']'+'['+Ltrim(Rtrim(crg_tcode))+']' ClientFullName,isnull(Ltrim(Rtrim(cnt_firstName)),'')FirstName,isnull(Ltrim(Rtrim(cnt_middleName)),'') MiddleName,isnull(Ltrim(Rtrim(cnt_lastName)),'') LastName,Ltrim(Rtrim(cnt_UCC)) UCC,Ltrim(Rtrim(crg_tcode)) TCode,LTRIM(Rtrim(cnt_internalId)) InternalID,cnt_id,crg_exchange from tbl_master_contact,tbl_master_contactExchange Where cnt_internalId=crg_cntID And Left(cnt_internalId,2)='CL' And crg_company='" + CompanyID + "' and crg_exchange in (" + SpecificSegment + @") Clients Inner Join (Select isnull(Ltrim(Rtrim(gpm_Description)),'')+'['+isnull(Ltrim(Rtrim(gpm_code)),'NoCode')+']' GrpDetail,grp_contactId,gpm_code,grp_groupType,gpm_id from tbl_master_groupMaster,tbl_trans_group Where gpm_id=grp_groupMaster and grp_groupType='" + GroupType + @"') [Group] on InternalID=grp_contactId) as T";
            string strQuery_FieldName = "Top 10  *";
            string strQuery_WhereClause = @"FirstName like '%RequestLetter%' Or MiddleName like '%RequestLetter%' Or LastName like '%RequestLetter%' or UCC like '%RequestLetter%' or TCode like '%RequestLetter%' Or TextField like '%RequestLetter%'";
            string strQuery_OrderBy = "TextField";
            string strQuery_GroupBy = null;
            return ReturnCombinedQuery(strQuery_Table, strQuery_FieldName, strQuery_WhereClause, strQuery_GroupBy, strQuery_OrderBy, "Client");
        }
        //--All Client Of Specific Company and Specific Segment and Specific Group

        public string GetClient_GroupFilter(string CompanyID, string SpecificSegment, string GroupType, string SpecificGroup)
        {
            string strQuery_Table = @"(Select  ClientFullName TextField,FirstName+'~'+MiddleName+'~'+LastName+'~'+UCC+'~'+TCode+'~'+InternalID+'~'+Cast(cnt_ID as Varchar(50))+'~'+crg_exchange+'~'+GrpDetail+'~'+gpm_code+'~'+grp_groupType+'~'+Cast(gpm_id as Varchar(50)) ValueField,FirstName,MiddleName,LastName,UCC,TCode,InternalID,cnt_ID,crg_exchange,GrpDetail,gpm_code,grp_groupType,gpm_id From (Select isnull(Ltrim(Rtrim(cnt_firstName)),'')+' '+isnull(Ltrim(Rtrim(cnt_middleName)),'') +' '+isnull(Ltrim(Rtrim(cnt_lastName)),'')+'['+Ltrim(Rtrim(cnt_UCC))+']'+'['+Ltrim(Rtrim(crg_tcode))+']' ClientFullName,isnull(Ltrim(Rtrim(cnt_firstName)),'')FirstName,isnull(Ltrim(Rtrim(cnt_middleName)),'') MiddleName,isnull(Ltrim(Rtrim(cnt_lastName)),'') LastName,Ltrim(Rtrim(cnt_UCC)) UCC,Ltrim(Rtrim(crg_tcode)) TCode,LTRIM(Rtrim(cnt_internalId)) InternalID,cnt_id,crg_exchange from tbl_master_contact,tbl_master_contactExchange Where cnt_internalId=crg_cntID And Left(cnt_internalId,2)='CL' And crg_company='" + CompanyID + "' and crg_exchange in (" + SpecificSegment + @") Clients Inner Join (Select isnull(Ltrim(Rtrim(gpm_Description)),'')+'['+isnull(Ltrim(Rtrim(gpm_code)),'NoCode')+']' GrpDetail,grp_contactId,gpm_code,grp_groupType,gpm_id from tbl_master_groupMaster,tbl_trans_group Where gpm_id=grp_groupMaster and grp_groupType='" + GroupType + @"'and grp_groupMaster in (" + SpecificGroup + @")) [Group] on InternalID=grp_contactId) as T";
            string strQuery_FieldName = "Top 10  *";
            string strQuery_WhereClause = @"FirstName like '%RequestLetter%' Or MiddleName like '%RequestLetter%' Or LastName like '%RequestLetter%' or UCC like '%RequestLetter%' or TCode like '%RequestLetter%' Or TextField like '%RequestLetter%'";
            string strQuery_OrderBy = "TextField";
            string strQuery_GroupBy = null;
            return ReturnCombinedQuery(strQuery_Table, strQuery_FieldName, strQuery_WhereClause, strQuery_GroupBy, strQuery_OrderBy, "Client");
        }

        public string ReturnCombinedQuery(string strQuery_Table, string strQuery_FieldName, string strQuery_WhereClause, string strQuery_GroupBy, string strQuery_OrderBy, string WhichCall)
        {
            string CombinedQuery = strQuery_Table + " $" + strQuery_FieldName + " $" + strQuery_WhereClause + " $" + strQuery_GroupBy + "$" + strQuery_OrderBy + "$" + WhichCall;
            //This is Replaced To Set WhichCall into ValueField
            CombinedQuery = CombinedQuery.Replace("ValueField", "+'~" + WhichCall + "' ValueField");
            CombinedQuery = CombinedQuery.Replace("'", "\\'");
            return CombinedQuery;
        }
        public string GetClient_SegmentFilterBetweenRegDate(string CompanyID, string SpecificSegment, string fromdate, string todate)
        {

            string strQuery_Table = @"(Select isnull(Ltrim(Rtrim(cnt_firstName)),'')+' '+isnull(Ltrim(Rtrim(cnt_middleName)),'')+' '+isnull(Ltrim(Rtrim(cnt_lastName)),'')+'['+Ltrim(Rtrim(cnt_UCC))+']'+'['+Ltrim(Rtrim(crg_tcode))+']' TextField,isnull(Ltrim(Rtrim(cnt_firstName)),'')+'~'+isnull(Ltrim(Rtrim(cnt_middleName)),'')+'~'+isnull(Ltrim(Rtrim(cnt_lastName)),'')+'~'+Ltrim(Rtrim(cnt_UCC))+','+Ltrim(Rtrim(crg_tcode))+'~'+LTRIM(Rtrim(cnt_internalId))+'~'+Cast(cnt_id as Varchar(50))+'~'+crg_exchange ValueField,isnull(Ltrim(Rtrim(cnt_firstName)),'') FirstName,isnull(Ltrim(Rtrim(cnt_middleName)),'') MiddleName,isnull(Ltrim(Rtrim(cnt_lastName)),'') LastName,Ltrim(Rtrim(cnt_UCC)) UCC,Ltrim(Rtrim(crg_tcode)) TCode,LTRIM(Rtrim(cnt_internalId)) InternalID,Cast(cnt_id as Varchar(50)) Cnt_ID,crg_exchange from tbl_master_contact,tbl_master_contactExchange Where cnt_internalId=crg_cntID And Left(cnt_internalId,2)='CL' And crg_company='" + CompanyID + "' And crg_exchange in (" + SpecificSegment + ") and crg_regisdate between '" + fromdate + "' and '" + todate + "') SpecificCompSpecificSeg";
            string strQuery_FieldName = "Top 10  *";
            string strQuery_WhereClause = @"FirstName like '%RequestLetter%' Or MiddleName like '%RequestLetter%' Or LastName like '%RequestLetter%' or UCC like '%RequestLetter%' or TCode like '%RequestLetter%' Or TextField like '%RequestLetter%'";
            string strQuery_OrderBy = "TextField";
            string strQuery_GroupBy = null;
            return ReturnCombinedQuery(strQuery_Table, strQuery_FieldName, strQuery_WhereClause, strQuery_GroupBy, strQuery_OrderBy, "Client");
        }

        #endregion

        #region Pertaining To Client
        public string GetClient_DPDetail(string ClientID)
        {
            string strQuery_Table = "(Select (select rtrim(replace(dp_dpname,char(160),'')) from tbl_master_depositoryparticipants where substring(dp_dpID,1,8)=dpd_dpCode)+' ['+ rtrim(dpd_ClientID)+']' AS TextFeild,cast(dpd_id as varchar)+'~'+dpd_dpCode+'~'+dpd_ClientID+'~'+isnull(dpd_cmbpid,'N/A') ValueField,rtrim(dpd_accountType) as dpd_accountType from tbl_master_contactdpdetails WHERE  dpd_cntId='" + ClientID + "') dpd";
            string strQuery_FieldName = @"top 10 *";
            string strQuery_WhereClause = "(TextFeild Like '%RequestLetter%' Or dpd_accountType Like '%RequestLetter%') ";
            string strQuery_OrderBy = "dpd_accountType";
            string strQuery_GroupBy = null;
            return ReturnCombinedQuery(strQuery_Table, strQuery_FieldName, strQuery_WhereClause, strQuery_GroupBy, strQuery_OrderBy, "ClientDpDetail");
        }
        #endregion

        #region Branch Relavant
        public string GetAllBranch()
        {
            string strQuery_Table = @"Tbl_Master_Branch";
            string strQuery_FieldName = "Ltrim(Rtrim(isnull(Branch_Description,'')))+'['+Ltrim(Rtrim(isnull(branch_Code,'')))+']' TextField,Cast(Branch_ID as Varchar)+'~'+Branch_internalID+'~'+Ltrim(Rtrim(isnull(Branch_Address1,'')))+' '+Ltrim(Rtrim(isnull(Branch_Address2,'')))+' '+Ltrim(Rtrim(isnull(Branch_Address3,'')))+' '+ISNULL((Select Ltrim(Rtrim(isnull(cou_country,''))) From tbl_Master_Country Where Cou_ID=branch_Country),'')+' '+Ltrim(Rtrim(isnull(Branch_Pin,''))) ValueField";
            string strQuery_WhereClause = @"Branch_Description Like '%RequestLetter%' Or Branch_Code Like '%RequestLetter%'";
            string strQuery_OrderBy = "TextField";
            string strQuery_GroupBy = null;
            return ReturnCombinedQuery(strQuery_Table, strQuery_FieldName, strQuery_WhereClause, strQuery_GroupBy, strQuery_OrderBy, "Branch");
        }
        public string GetALLBranch(string Branchhierarchy)
        {
            string strQuery_Table = @"Tbl_Master_Branch";
            string strQuery_FieldName = "Ltrim(Rtrim(isnull(Branch_Description,'')))+'['+Ltrim(Rtrim(isnull(branch_Code,'')))+']' TextField,Cast(Branch_ID as Varchar)+'~'+Branch_internalID+'~'+Ltrim(Rtrim(isnull(Branch_Address1,'')))+' '+Ltrim(Rtrim(isnull(Branch_Address2,'')))+' '+Ltrim(Rtrim(isnull(Branch_Address3,'')))+' '+ISNULL((Select Ltrim(Rtrim(isnull(cou_country,''))) From tbl_Master_Country Where Cou_ID=branch_Country),'')+' '+Ltrim(Rtrim(isnull(Branch_Pin,''))) ValueField";
            string strQuery_WhereClause = @"Branch_ID in (" + Branchhierarchy + ") And (Branch_Description Like '%RequestLetter%' Or Branch_Code Like '%RequestLetter%')";
            string strQuery_OrderBy = "TextField";
            string strQuery_GroupBy = null;
            return ReturnCombinedQuery(strQuery_Table, strQuery_FieldName, strQuery_WhereClause, strQuery_GroupBy, strQuery_OrderBy, "BranchHierarchy");
        }
        public string GetAllBranch(int oTop)
        {
            string strQuery_Table = @"Tbl_Master_Branch";
            string strQuery_FieldName = "Top " + oTop + " Ltrim(Rtrim(isnull(Branch_Description,'')))+'['+Ltrim(Rtrim(isnull(branch_Code,'')))+']' TextField,Cast(Branch_ID as Varchar)+'~'+Branch_internalID+'~'+Ltrim(Rtrim(isnull(Branch_Address1,'')))+' '+Ltrim(Rtrim(isnull(Branch_Address2,'')))+' '+Ltrim(Rtrim(isnull(Branch_Address3,'')))+' '+ISNULL((Select Ltrim(Rtrim(isnull(cou_country,''))) From tbl_Master_Country Where Cou_ID=branch_Country),'')+' '+Ltrim(Rtrim(isnull(Branch_Pin,''))) ValueField";
            string strQuery_WhereClause = @"Branch_Description Like '%RequestLetter%' Or Branch_Code Like '%RequestLetter%'";
            string strQuery_OrderBy = "TextField";
            string strQuery_GroupBy = null;
            return ReturnCombinedQuery(strQuery_Table, strQuery_FieldName, strQuery_WhereClause, strQuery_GroupBy, strQuery_OrderBy, "Branch");
        }

        public string GetParentBranch()
        {
            string strQuery_Table = @"Tbl_Master_Branch";
            string strQuery_FieldName = "Ltrim(Rtrim(isnull(Branch_Description,'')))+'['+Ltrim(Rtrim(isnull(branch_Code,'')))+']' TextField,Branch_ID";
            string strQuery_WhereClause = @"Branch_ParentID=0";
            string strQuery_OrderBy = null;
            string[,] ParentBranchInfo = GetFieldValue(strQuery_Table, strQuery_FieldName, strQuery_WhereClause, 2, strQuery_OrderBy);
            return ParentBranchInfo[0, 1];
        }
        #endregion

        #region Group Relavant
        public string GetAllGroups(string SubType)
        {
            string strQuery_Table = @"Tbl_Master_GroupMaster";
            string strQuery_FieldName = "Ltrim(Rtrim(isnull(Gpm_Description,'')))+'['+Ltrim(Rtrim(isnull(Gpm_Code,'')))+']' TextField,Ltrim(Rtrim(isnull(Gpm_id,'')))+'~'+Ltrim(Rtrim(isnull(gpm_Code,'')))+'~'+Ltrim(Rtrim(isnull(gpm_Membertype,'')))+'~'+Ltrim(Rtrim(isnull(gpm_emailID,'')))+'~'+Ltrim(Rtrim(isnull(gpm_ccemailID,''))) ValueField";
            string strQuery_WhereClause = @"gpm_type='" + SubType + "' and (Gpm_Description Like '%RequestLetter%' Or  Gpm_Code Like '%RequestLetter%')";
            string strQuery_OrderBy = "TextField";
            string strQuery_GroupBy = null;
            return ReturnCombinedQuery(strQuery_Table, strQuery_FieldName, strQuery_WhereClause, strQuery_GroupBy, strQuery_OrderBy, "Group");
        }
        public string GetAllGroups(string SubType, int oTop)
        {
            string strQuery_Table = @"Tbl_Master_GroupMaster";
            string strQuery_FieldName = "Top " + oTop + " Ltrim(Rtrim(isnull(Gpm_Description,'')))+'['+Ltrim(Rtrim(isnull(Gpm_Code,'')))+']' TextField,Ltrim(Rtrim(isnull(Gpm_id,'')))+'~'+Ltrim(Rtrim(isnull(gpm_Code,'')))+'~'+Ltrim(Rtrim(isnull(gpm_Membertype,'')))+'~'+Ltrim(Rtrim(isnull(gpm_emailID,'')))+'~'+Ltrim(Rtrim(isnull(gpm_ccemailID,''))) ValueField";
            string strQuery_WhereClause = @"gpm_type='" + SubType + "' and (Gpm_Description Like '%RequestLetter%' Or  Gpm_Code Like '%RequestLetter%')";
            string strQuery_OrderBy = "TextField";
            string strQuery_GroupBy = null;
            return ReturnCombinedQuery(strQuery_Table, strQuery_FieldName, strQuery_WhereClause, strQuery_GroupBy, strQuery_OrderBy, "Group");
        }
        #endregion

        #region Exchange Relavant

        #region GetExchangeDetail

        ////All Exchange Detail
        public string GetExchangesDetail()
        {
            string strQuery_Table = "(Select Ltrim(Rtrim(Exchange_Name))+' ['+Ltrim(Rtrim(Exchange_ShortName))+']' as FullName,Cast(Exchange_ID as Varchar(10))+'~'+Exchange_Name+'~'+Exchange_ShortName+'~'+Cast(Exchange_CountryID as Varchar(10))+'~'+Exchange_IsCommodity+'~'+Cast(Exchange_CurrencyID as Varchar(10)) ValueField,Exchange_Name,Exchange_ShortName,Exchange_CountryID,Exchange_IsCommodity,Exchange_CurrencyID from Master_Exchange) Exchange";
            string strQuery_FieldName = "Top 10 *";
            string strQuery_WhereClause = "Exchange_Name like '%RequestLetter%' Or Exchange_ShortName like '%RequestLetter%' Or FullName like '%RequestLetter%'";
            string strQuery_OrderBy = "Exchange_Name";
            string strQuery_GroupBy = null;
            return ReturnCombinedQuery(strQuery_Table, strQuery_FieldName, strQuery_WhereClause, strQuery_GroupBy, strQuery_OrderBy, "Exchange");
        }

        ////AllOrSpecific Value (A,S) Respectively
        ////If A Then Company will be blank
        ////Registered Exchange Detail CompanyWise
        public string GetExchangesDetail(string AllOrSpecific, string CompanyID)
        {
            string strQuery_WhereClause = String.Empty;
            string strQuery_Table = "(Select  Distinct  FullName TextField,Cast(Exchange_ID as Varchar(10))+'~'+Exchange_Name+'~'+Exchange_ShortName+'~'+Cast(Exchange_CountryID as Varchar(10))+'~'+Exchange_IsCommodity+'~'+Cast(Exchange_CurrencyID as Varchar(10)) ValueField,Exchange_Name,Exchange_ShortName,Exchange_CountryID,Exchange_IsCommodity,Exchange_CurrencyID,FullName from (Select Exchange_ID,Exchange_Name,Exchange_ShortName,Exchange_CountryID,Exchange_IsCommodity,Exchange_CurrencyID ,Ltrim(Rtrim(Exchange_Name))+' ['+Ltrim(Rtrim(Exchange_ShortName))+']' as FullName from Master_Exchange) T1 Inner Join (Select Exh_ShortName from Tbl_Master_Exchange,Tbl_Master_CompanyExchange Where Exh_CntId=Exch_ExchID and Exch_CompID='COR0000002') T2 On Ltrim(Rtrim(Exchange_ShortName))=Ltrim(Rtrim(exh_shortName)) and Ltrim(Rtrim(Exchange_ShortName)) not Like '%Account%') Exchange";
            string strQuery_FieldName = "Top 10 *";
            if (AllOrSpecific == "S")
                strQuery_WhereClause = "exch_compId='" + CompanyID + "' And (Exchange_Name like '%RequestLetter%' Or Exchange_ShortName like '%RequestLetter%' Or FullName like '%RequestLetter%')";
            else
                strQuery_WhereClause = "(Exchange_Name like '%RequestLetter%' Or Exchange_ShortName like '%RequestLetter%' Or FullName like '%RequestLetter%')";
            string strQuery_OrderBy = "Exchange_Name";
            string strQuery_GroupBy = null;
            return ReturnCombinedQuery(strQuery_Table, strQuery_FieldName, strQuery_WhereClause, strQuery_GroupBy, strQuery_OrderBy, "Exchange");

        }

        public string GetExchangeName()
        {
            return GetDataTable(@"(Select Exch_CompID,Exch_InternalID,Exh_ShortName,Exch_SegmentID from Tbl_Master_Exchange,
            Tbl_Master_CompanyExchange Where Exh_CntId=Exch_ExchID) as T1,Master_Exchange", @"
            isnull(Exh_ShortName,'') ExchangeName", "Exchange_ShortName=Exh_ShortName and exch_internalId='" +
            HttpContext.Current.Session["UserSegID"].ToString() + "'", "Exchange_ID").Rows[0][0].ToString();
        }
        #endregion

        #endregion

        #region Currency Relavant
        public string GetCurrencyAQuery()
        {
            string strQuery_Table = "(Select Ltrim(Rtrim(isnull(Currency_Name,'')))+'['+Ltrim(Rtrim(isnull(Currency_AlphaCode,'')))+']' TextField,Cast(Currency_ID as Varchar(10))+'~'+Ltrim(Rtrim(isnull(Currency_Name,'')))+'~'+Ltrim(Rtrim(isnull(Currency_Symbol,'')))+'~'+Ltrim(Rtrim(isnull(Currency_AlphaCode,'')))+'~'+Ltrim(Rtrim(isnull(Currency_InternationalCode,'')))+'~'+Ltrim(Rtrim(isnull(Currency_DecimalCharacter,'')))+'~'+Cast(Currency_DecimalPlaces as Varchar(10))+'~'+Ltrim(Rtrim(isnull(Currency_DecimalPortionName,'')))+'~'+Ltrim(Rtrim(isnull(Currency_DecimalPortionSymbol,'')))+'~'+Ltrim(Rtrim(isnull(Currency_AmountDisplayBasis,''))) ValueField,Currency_Name,Currency_Symbol,Currency_AlphaCode,Currency_InternationalCode,Currency_DecimalCharacter,Currency_DecimalPlaces,Currency_DecimalPortionName,Currency_DecimalPortionSymbol,Currency_AmountDisplayBasis from Master_Currency) Currency";
            string strQuery_FieldName = "Top 10 *";
            string strQuery_WhereClause = "Currency_Name Like '%RequestLetter%' Or  Currency_AlphaCode Like '%RequestLetter%' ";
            string strQuery_OrderBy = "Currency_Name";
            string strQuery_GroupBy = null;
            return ReturnCombinedQuery(strQuery_Table, strQuery_FieldName, strQuery_WhereClause, strQuery_GroupBy, strQuery_OrderBy, "Currency");
        }
        public string GetCurrencyDetail()
        {
            string strQuery_Table = "(Select Ltrim(Rtrim(isnull(Currency_Name,'')))+'['+Ltrim(Rtrim(isnull(Currency_AlphaCode,'')))+']' TextField,Cast(Currency_ID as Varchar(10))+'~'+Ltrim(Rtrim(isnull(Currency_Name,'')))+'~'+Ltrim(Rtrim(isnull(Currency_Symbol,'')))+'~'+Ltrim(Rtrim(isnull(Currency_AlphaCode,'')))+'~'+Ltrim(Rtrim(isnull(Currency_InternationalCode,'')))+'~'+Ltrim(Rtrim(isnull(Currency_DecimalCharacter,'')))+'~'+Cast(Currency_DecimalPlaces as Varchar(10))+'~'+Ltrim(Rtrim(isnull(Currency_DecimalPortionName,'')))+'~'+Ltrim(Rtrim(isnull(Currency_DecimalPortionSymbol,'')))+'~'+Ltrim(Rtrim(isnull(Currency_AmountDisplayBasis,''))) ValueField,Currency_Name,Currency_Symbol,Currency_AlphaCode,Currency_InternationalCode,Currency_DecimalCharacter,Currency_DecimalPlaces,Currency_DecimalPortionName,Currency_DecimalPortionSymbol,Currency_AmountDisplayBasis from Master_Currency) Currency";
            string strQuery_FieldName = "Top 10 *";
            string strQuery_WhereClause = "Currency_Name Like '%RequestLetter%' Or  Currency_AlphaCode Like '%RequestLetter%' ";
            string strQuery_OrderBy = "Currency_Name";
            string strQuery_GroupBy = null;
            return ReturnCombinedQuery(strQuery_Table, strQuery_FieldName, strQuery_WhereClause, strQuery_GroupBy, strQuery_OrderBy, "Currency");
        }
        #endregion
        #region DP
        #region DpAccount Relavant
        ///Note 1 : if AllOrSpecificAccountType and AllOrSpecificSegmentID is ALL ("A") Not Specific "S" Pass String.Empty in 
        ///SpecificDpAccountType and SpecificSegmentID;
        ///Note 2 : SpecificDpAccountType {'[POOL]','[MRGIN]',............}
        ///Note 3 : SpecificSegmentID {1,2,......}

        public string GetDpAccountsAQuery(string AllOrSpecificAccountType, string AllOrSpecificSegmentID, string SpecificDpAccountType, string SpecificSegmentID)
        {
            if (AllOrSpecificAccountType == "A" && AllOrSpecificSegmentID == "A")
            {
                strQuery_Table = "Master_DPAccounts";
                strQuery_FieldName = "Top 10 Ltrim(Rtrim(isnull(DPAccounts_ShortName,''))) TextField,Cast(DPAccounts_ID as Varchar(10))+'~'+Ltrim(Rtrim(isnull(DPAccounts_AccountName,'')))+'~'+Ltrim(Rtrim(isnull(DPAccounts_AccountType,'')))+'~'+Ltrim(Rtrim(Cast(isnull(DPAccounts_ExchangeSegmentID,'') as Varchar(10))))+'~'+Ltrim(Rtrim(isnull(DPAccounts_DPID,'')))+'~'+Ltrim(Rtrim(isnull(DPAccounts_ClientID,'')))+'~'+Ltrim(Rtrim(isnull(DPAccounts_CMBPID,'')))+'~'+Ltrim(Rtrim(isnull(DPAccounts_CompanyID,''))) ValueField";
                strQuery_WhereClause = "(DPAccounts_ShortName Like '%RequestLetter%' Or DPAccounts_AccountName Like '%RequestLetter%' Or DPAccounts_AccountType Like '%RequestLetter%' Or DPAccounts_DPID Like '%RequestLetter%' Or DPAccounts_ClientID Like '%RequestLetter%') And DPAccounts_CompanyID='" + HttpContext.Current.Session["LastCompany"].ToString() + "'";
                strQuery_OrderBy = "DPAccounts_AccountType";
                strQuery_GroupBy = "";
            }
            else if (AllOrSpecificAccountType == "S" && AllOrSpecificSegmentID == "A")
            {
                strQuery_Table = "Master_DPAccounts";
                strQuery_FieldName = "Top 10 Ltrim(Rtrim(isnull(DPAccounts_ShortName,''))) TextField,Cast(DPAccounts_ID as Varchar(10))+'~'+Ltrim(Rtrim(isnull(DPAccounts_AccountName,'')))+'~'+Ltrim(Rtrim(isnull(DPAccounts_AccountType,'')))+'~'+Ltrim(Rtrim(Cast(isnull(DPAccounts_ExchangeSegmentID,'') as Varchar(10))))+'~'+Ltrim(Rtrim(isnull(DPAccounts_DPID,'')))+'~'+Ltrim(Rtrim(isnull(DPAccounts_ClientID,'')))+'~'+Ltrim(Rtrim(isnull(DPAccounts_CMBPID,'')))+'~'+Ltrim(Rtrim(isnull(DPAccounts_CompanyID,''))) ValueField";
                strQuery_WhereClause = "DPAccounts_AccountType in (" + SpecificDpAccountType + ") And (DPAccounts_ShortName Like '%RequestLetter%' Or DPAccounts_AccountName Like '%RequestLetter%' Or DPAccounts_AccountType Like '%RequestLetter%' Or DPAccounts_DPID Like '%RequestLetter%' Or DPAccounts_ClientID Like '%RequestLetter%') And DPAccounts_CompanyID='" + HttpContext.Current.Session["LastCompany"].ToString() + "'";
                strQuery_OrderBy = "DPAccounts_AccountType";
                strQuery_GroupBy = "";
            }
            else if (AllOrSpecificAccountType == "A" && AllOrSpecificSegmentID == "S")
            {
                strQuery_Table = "Master_DPAccounts";
                strQuery_FieldName = "Top 10 Ltrim(Rtrim(isnull(DPAccounts_ShortName,''))) TextField,Cast(DPAccounts_ID as Varchar(10))+'~'+Ltrim(Rtrim(isnull(DPAccounts_AccountName,'')))+'~'+Ltrim(Rtrim(isnull(DPAccounts_AccountType,'')))+'~'+Ltrim(Rtrim(Cast(isnull(DPAccounts_ExchangeSegmentID,'') as Varchar(10))))+'~'+Ltrim(Rtrim(isnull(DPAccounts_DPID,'')))+'~'+Ltrim(Rtrim(isnull(DPAccounts_ClientID,'')))+'~'+Ltrim(Rtrim(isnull(DPAccounts_CMBPID,'')))+'~'+Ltrim(Rtrim(isnull(DPAccounts_CompanyID,''))) ValueField";
                strQuery_WhereClause = "DPAccounts_ExchangeSegmentID in (" + SpecificSegmentID + ") And(DPAccounts_ShortName Like '%RequestLetter%' Or DPAccounts_AccountName Like '%RequestLetter%' Or DPAccounts_AccountType Like '%RequestLetter%' Or DPAccounts_DPID Like '%RequestLetter%' Or DPAccounts_ClientID Like '%RequestLetter%') And DPAccounts_CompanyID='" + HttpContext.Current.Session["LastCompany"].ToString() + "'";
                strQuery_OrderBy = "DPAccounts_AccountType";
                strQuery_GroupBy = "";
            }
            else if (AllOrSpecificAccountType == "S" && AllOrSpecificSegmentID == "S")
            {
                strQuery_Table = "Master_DPAccounts";
                strQuery_FieldName = "Top 10 Ltrim(Rtrim(isnull(DPAccounts_ShortName,''))) TextField,Cast(DPAccounts_ID as Varchar(10))+'~'+Ltrim(Rtrim(isnull(DPAccounts_AccountName,'')))+'~'+Ltrim(Rtrim(isnull(DPAccounts_AccountType,'')))+'~'+Ltrim(Rtrim(Cast(isnull(DPAccounts_ExchangeSegmentID,'') as Varchar(10))))+'~'+Ltrim(Rtrim(isnull(DPAccounts_DPID,'')))+'~'+Ltrim(Rtrim(isnull(DPAccounts_ClientID,'')))+'~'+Ltrim(Rtrim(isnull(DPAccounts_CMBPID,'')))+'~'+Ltrim(Rtrim(isnull(DPAccounts_CompanyID,''))) ValueField";
                strQuery_WhereClause = "DPAccounts_AccountType in (" + SpecificDpAccountType + ") And DPAccounts_ExchangeSegmentID in (" + SpecificSegmentID + ") And(DPAccounts_ShortName Like '%RequestLetter%' Or DPAccounts_AccountName Like '%RequestLetter%' Or DPAccounts_AccountType Like '%RequestLetter%' Or DPAccounts_DPID Like '%RequestLetter%' Or DPAccounts_ClientID Like '%RequestLetter%') And DPAccounts_CompanyID='" + HttpContext.Current.Session["LastCompany"].ToString() + "'";
                strQuery_OrderBy = "DPAccounts_AccountType";
                strQuery_GroupBy = "";
            }
            return ReturnCombinedQuery(strQuery_Table, strQuery_FieldName, strQuery_WhereClause, strQuery_GroupBy, strQuery_OrderBy, "DpAccounts");
        }
        #endregion
        #region CDSL
        public string GetGeographicalData()
        {
            string strQuery_Table = @"Master_Geographical";
            string strQuery_FieldName = "Top 10 Geographical_Description TextField,Cast(Ltrim(Rtrim(Geographical_ID)) as Varchar(10))+'~'+Ltrim(Rtrim(IsNull(Geographical_Description,''))) ValueField";
            string strQuery_WhereClause = @"Geographical_Description Like '%RequestLetter%'";
            string strQuery_OrderBy = "TextField";
            string strQuery_GroupBy = null;
            return ReturnCombinedQuery(strQuery_Table, strQuery_FieldName, strQuery_WhereClause, strQuery_GroupBy, strQuery_OrderBy, "GEOGRAPHICAL");
        }

        public string GetAnnualIncomeData()
        {
            string strQuery_Table = @"Master_AnnualIncome";
            string strQuery_FieldName = "Top 10 AnnualIncome_Description TextField,Cast(Ltrim(Rtrim(AnnualIncome_ID)) as Varchar(10))+'~'+Ltrim(Rtrim(IsNull(AnnualIncome_Description,''))) ValueField";
            string strQuery_WhereClause = @"AnnualIncome_Description Like '%RequestLetter%'";
            string strQuery_OrderBy = "TextField";
            string strQuery_GroupBy = null;
            return ReturnCombinedQuery(strQuery_Table, strQuery_FieldName, strQuery_WhereClause, strQuery_GroupBy, strQuery_OrderBy, "ANNUALINCOME");
        }

        public string GetNationalityData()
        {
            string strQuery_Table = @"Master_Nationality";
            string strQuery_FieldName = "Top 10 Nationality_Description TextField,Cast(Ltrim(Rtrim(Nationality_ID)) as Varchar(10))+'~'+Ltrim(Rtrim(IsNull(Nationality_Description,''))) ValueField";
            string strQuery_WhereClause = @"Nationality_Description Like '%RequestLetter%'";
            string strQuery_OrderBy = "TextField";
            string strQuery_GroupBy = null;
            return ReturnCombinedQuery(strQuery_Table, strQuery_FieldName, strQuery_WhereClause, strQuery_GroupBy, strQuery_OrderBy, "NATIONALITY");
        }

        public string GetEducationData()
        {
            string strQuery_Table = @"tbl_master_education";
            string strQuery_FieldName = "Top 10 edu_education TextField,Cast(Ltrim(Rtrim(edu_id)) as Varchar(10))+'~'+Ltrim(Rtrim(IsNull(edu_education,''))) ValueField";
            string strQuery_WhereClause = @"edu_education Like '%RequestLetter%'";
            string strQuery_OrderBy = "TextField";
            string strQuery_GroupBy = null;
            return ReturnCombinedQuery(strQuery_Table, strQuery_FieldName, strQuery_WhereClause, strQuery_GroupBy, strQuery_OrderBy, "EDUCATION");
        }

        public string GetLegalStatusData()
        {
            string strQuery_Table = @"tbl_master_legalStatus";
            string strQuery_FieldName = "Top 10 lgl_legalStatus TextField,Cast(Ltrim(Rtrim(lgl_id)) as Varchar(10))+'~'+Ltrim(Rtrim(IsNull(lgl_legalStatus,''))) ValueField";
            string strQuery_WhereClause = @"lgl_legalStatus Like '%RequestLetter%'";
            string strQuery_OrderBy = "TextField";
            string strQuery_GroupBy = null;
            return ReturnCombinedQuery(strQuery_Table, strQuery_FieldName, strQuery_WhereClause, strQuery_GroupBy, strQuery_OrderBy, "LEGALSTATUS");
        }

        public string GetLanguageData()
        {
            string strQuery_Table = @"Master_Language";
            string strQuery_FieldName = "Top 10 Language_Name TextField,Cast(Ltrim(Rtrim(Language_ID)) as Varchar(10))+'~'+Ltrim(Rtrim(IsNull(Language_Name,''))) ValueField";
            string strQuery_WhereClause = @"Language_Name Like '%RequestLetter%'";
            string strQuery_OrderBy = "TextField";
            string strQuery_GroupBy = null;
            return ReturnCombinedQuery(strQuery_Table, strQuery_FieldName, strQuery_WhereClause, strQuery_GroupBy, strQuery_OrderBy, "LANGUAGE");
        }

        public string GetCdslPanExemptionData()
        {
            string strQuery_Table = @"Master_PANExemptCategory";
            string strQuery_FieldName = "Top 10 PanExemptCategory_Description TextField,Cast(Ltrim(Rtrim(PanExemptCategory_ID)) as Varchar(10))+'~'+Ltrim(Rtrim(IsNull(PanExemptCategory_Description,'')))+'~'+Cast(Ltrim(Rtrim(IsNull(PanExemptCategory_CdslID,'0'))) as Varchar(10)) ValueField";
            string strQuery_WhereClause = @"PanExemptCategory_Description Like '%RequestLetter%'";
            string strQuery_OrderBy = "TextField";
            string strQuery_GroupBy = null;
            return ReturnCombinedQuery(strQuery_Table, strQuery_FieldName, strQuery_WhereClause, strQuery_GroupBy, strQuery_OrderBy, "CDSLPANEXEMPTION");
        }

        public string GetCdslOccupationData()
        {
            string strQuery_Table = @"tbl_master_profession";
            string strQuery_FieldName = "Top 10 pro_professionName TextField,Cast(Ltrim(Rtrim(pro_id)) as Varchar(10))+'~'+Ltrim(Rtrim(IsNull(pro_professionName,'')))+'~'+Cast(Ltrim(Rtrim(IsNull(PRO_CdslID,'0'))) as Varchar(10)) ValueField";
            string strQuery_WhereClause = @"pro_professionName Like '%RequestLetter%'";
            string strQuery_OrderBy = "TextField";
            string strQuery_GroupBy = null;
            return ReturnCombinedQuery(strQuery_Table, strQuery_FieldName, strQuery_WhereClause, strQuery_GroupBy, strQuery_OrderBy, "CDSLOCCUPATION");
        }

        public string GetCurrencyData()
        {
            string strQuery_Table = @"Master_Currency";
            string strQuery_FieldName = "Top 10 Currency_Name TextField,Cast(Ltrim(Rtrim(Currency_ID)) as Varchar(10))+'~'+Ltrim(Rtrim(IsNull(Currency_Name,'')))+'~'+Cast(Ltrim(Rtrim(IsNull(Currency_CDSLID,'0'))) as Varchar(10)) ValueField";
            string strQuery_WhereClause = @"Currency_Name Like '%RequestLetter%'";
            string strQuery_OrderBy = "TextField";
            string strQuery_GroupBy = null;
            return ReturnCombinedQuery(strQuery_Table, strQuery_FieldName, strQuery_WhereClause, strQuery_GroupBy, strQuery_OrderBy, "CURRENCY");
        }

        public string GetCDSL_MappingStaticData(string FieldName)
        {
            string strQuery_Table = @"(select CdslStaticData_Description,CdslStaticData_TypeCode,CdslStaticData_ID From Master_CdslStaticDataCode Where CdslStaticData_FieldName='" + FieldName + "') CdslData";
            string strQuery_FieldName = "Top 10 CdslStaticData_Description TextField,Cast(Ltrim(Rtrim(CdslStaticData_ID)) as Varchar(10))+'~'+Ltrim(Rtrim(IsNull(CdslStaticData_TypeCode,'')))+'~'+Ltrim(Rtrim(IsNull(CdslStaticData_Description,''))) ValueField";
            string strQuery_WhereClause = @"CdslStaticData_Description Like '%RequestLetter%'";
            string strQuery_OrderBy = "TextField";
            string strQuery_GroupBy = null;
            return ReturnCombinedQuery(strQuery_Table, strQuery_FieldName, strQuery_WhereClause, strQuery_GroupBy, strQuery_OrderBy, "CDSLMAPSTATIC");
        }
        #endregion
        #region NSDL

        #endregion
        #region NSDLCDSL
        //WhichDP (NSDL,CDSL)
        public string GetCompanyDpDetail(string WhichDP)
        {
            string strQuery_Table = @"Master_NsdlBPList,tbl_master_companyExchange";
            string strQuery_FieldName = "Top 10 Ltrim(Rtrim(isnull(NsdlBPList_BPName,'')))+'['+Ltrim(Rtrim(isnull(NsdlBPList_BPID,'')))+']' TextField,Ltrim(Rtrim(isnull(NsdlBPList_ID,'')))+'~'+Ltrim(Rtrim(isnull(NsdlBPList_BPID,'')))+'~'+Ltrim(Rtrim(isnull(NsdlBPList_BPRole,'')))  +'~'+Ltrim(Rtrim(isnull(NsdlBPList_BPCategory,''))) +'~'+Ltrim(Rtrim(isnull(exch_compId,''))) +'~'+Ltrim(Rtrim(isnull(exch_membershipType,''))) +'~'+Ltrim(Rtrim(isnull(exch_sebiNo,''))) +'~'+Ltrim(Rtrim(isnull(exch_sebiexpDate,''))) ValueField";
            string strQuery_WhereClause = @"NsdlBPList_BPID=exch_TMCode and NsdlBPList_BPRole in (1,6) and exch_membershipType='" + WhichDP + "' and (NsdlBPList_BPName Like '%RequestLetter%' Or NsdlBPList_BPID Like '%RequestLetter%')";
            string strQuery_OrderBy = "TextField";
            string strQuery_GroupBy = null;
            return ReturnCombinedQuery(strQuery_Table, strQuery_FieldName, strQuery_WhereClause, strQuery_GroupBy, strQuery_OrderBy, "CompanyDP");
        }

        public DataTable GetClient_NSDLCDSL(string DTOrAQuery, ref string AQuery, int NoOfTopRows, string CustomizeSelect, string CustomizeWhere, int Version, string WhichDP)
        {
            if (Version == 1)
            {
                string WhichClient = String.Empty;
                if (WhichDP == "NSDL")
                {
                    strQuery_Table = "(Select TextField,ValueField+'~'+isnull(GroupName,'')+'~'+isnull(Cast(GPMID as Varchar(20)),'')+'~'+isnull(Cast(GRPID as Varchar(20)),'')+'~'+isnull(GDescription,'') ValueField,T1.ContactID,BranchID,GPMID,GRPID,GType,GroupName,GDescription from (Select LTRIM(Rtrim(isnull(NsdlClients_BenFirstHolderName,'')))+' ['+Ltrim(Rtrim(isnull(NsdlClients_BenAccountID,'')))+'] ['+Ltrim(Rtrim(isnull(Cnt_UCC,'')))+'] ['+Ltrim(Rtrim(isnull(branch_description,'')))+']' TextField,Ltrim(Rtrim(isnull(NsdlClients_ContactID,'')))+ '~'+ Ltrim(Rtrim(isnull(NsdlClients_BenAccountID,'')))+'~'+Cast(NsdlClients_ID as Varchar(20))+'~'+Ltrim(Rtrim(isnull(cnt_UCC,'')))+'~'+Ltrim(Rtrim(isnull(branch_description,'')))+'~'+Ltrim(Rtrim(isnull(branch_id,''))) ValueField,NsdlClients_ContactID ContactID,NsdlClients_ID NsdlClientID,cnt_UCC UCC,Branch_ID BranchID,branch_description BranchDescription from Master_NsdlClients Left Outer Join tbl_master_contact On cnt_internalId=NsdlClients_ContactID Left Outer Join tbl_master_Branch On branch_id=cnt_branchid and cnt_branchid=NsdlClients_BranchID) T1 Left Outer Join (Select Ltrim(Rtrim(gpm_Description))+'['+gpm_Code+']' GroupName,gpm_ID GPMID,grp_ID GRPID,LTRIM(RTRIM(gpm_Description)) GDescription,Ltrim(Rtrim(gpm_Type)) GType,LTRIM(RTRIM(gpm_Code)) GCode,Ltrim(Rtrim(gpm_MemberType)) MemberType, grp_contactId ContactID from tbl_master_groupMaster,tbl_trans_group Where gpm_id=grp_groupMaster) T2 On  T1.ContactID=T2.ContactID) NsdlClientDetail";
                    strQuery_FieldName = "Top " + NoOfTopRows.ToString() + " " + CustomizeSelect;
                    strQuery_OrderBy = "TextField";
                    strQuery_GroupBy = null;
                    if (DTOrAQuery == "D")
                    {
                        strQuery_WhereClause = CustomizeWhere;
                        return GetDataTable(strQuery_Table, strQuery_FieldName, strQuery_WhereClause, strQuery_OrderBy);
                    }
                    else
                    {
                        strQuery_WhereClause = @"TextField Like '%RequestLetter%' " + CustomizeWhere;
                        AQuery = ReturnCombinedQuery(strQuery_Table, strQuery_FieldName, strQuery_WhereClause, strQuery_GroupBy, strQuery_OrderBy, "NSDLCLIENT");
                    }
                }
                else
                {
                    strQuery_Table = "(Select TextField,ValueField+'~'+isnull(GroupName,'')+'~'+isnull(Cast(GPMID as Varchar(20)),'')+'~'+isnull(Cast(GRPID as Varchar(20)),'')+'~'+isnull(GDescription,'') ValueField,T1.ContactID,BranchID,GPMID,GRPID,GType,GroupName,GDescription from (Select LTRIM(Rtrim(isnull(CdslClients_FirstHolderName,'')))+' ['+Ltrim(Rtrim(isnull(CdslClients_BOID,'')))+'] ['+Ltrim(Rtrim(isnull(Cnt_UCC,'')))+'] ['+Ltrim(Rtrim(isnull(branch_description,'')))+']' TextField,Ltrim(Rtrim(isnull(CdslClients_ContactID,'')))+ '~'+ Ltrim(Rtrim(isnull(CdslClients_BOID,'')))+'~'+Cast(CdslClients_ID as Varchar(20))+'~'+Ltrim(Rtrim(isnull(cnt_UCC,'')))+'~'+Ltrim(Rtrim(isnull(branch_description,'')))+'~'+Ltrim(Rtrim(isnull(branch_id,''))) ValueField,CdslClients_ContactID ContactID,CdslClients_ID CdslClientID,cnt_UCC UCC,Branch_ID BranchID,branch_description BranchDescription from Master_CdslClients Left Outer Join tbl_master_contact On cnt_internalId=CdslClients_ContactID Left Outer Join tbl_master_Branch On branch_id=cnt_branchid and cnt_branchid=CdslClients_BranchID) T1 Left Outer Join (Select Ltrim(Rtrim(gpm_Description))+'['+gpm_Code+']' GroupName,gpm_ID GPMID,grp_ID GRPID, LTRIM(RTRIM(gpm_Description)) GDescription,Ltrim(Rtrim(gpm_Type)) GType,LTRIM(RTRIM(gpm_Code)) GCode,Ltrim(Rtrim(gpm_MemberType)) MemberType, grp_contactId ContactID from tbl_master_groupMaster,tbl_trans_group Where gpm_id=grp_groupMaster) T2 On  T1.ContactID=T2.ContactID) CDSLClientDetail";
                    strQuery_FieldName = "Top " + NoOfTopRows.ToString() + " TextField,ValueField";
                    strQuery_WhereClause = @"TextField Like '%RequestLetter%'";
                    strQuery_OrderBy = "TextField";
                    strQuery_GroupBy = null;
                    if (DTOrAQuery == "D")
                    {
                        return GetDataTable(strQuery_Table, strQuery_FieldName, strQuery_WhereClause, strQuery_OrderBy);
                    }
                    else
                    {
                        AQuery = ReturnCombinedQuery(strQuery_Table, strQuery_FieldName, strQuery_WhereClause, strQuery_GroupBy, strQuery_OrderBy, "CDSLCLIENT");
                    }

                }
            }
            return null;
        }


        #endregion
        #endregion
        #region Settlement Relavant
        ///Note : It Will Show All Or Specific Settlement 
        ///if AllOrSpecific "S"  a Date Settlement Will Be Greater and Equal to That Date
        ///if AllOrSpecific "A" Pass Min Date DateTime.MinValue
        public string GetSettlementsAQuery(string AllOrSpecific, DateTime SpecificStartFrom)
        {
            if (AllOrSpecific == "A")
                strQuery_Table = @"(Select Ltrim(Rtrim(Isnull(Settlements_Number,'')))+Ltrim(Rtrim(Isnull(Settlements_TypeSuffix,''))) TextField,Ltrim(Rtrim(Isnull(Settlements_Number,'')))+'~'+Ltrim(Rtrim(Isnull(Settlements_TypeSuffix,'')))+'~'+Ltrim(Rtrim(Isnull(Settlements_Type,'')))+'~'+Replace(CONVERT(Varchar,Settlements_StartDateTime,111),'/','-')+'~'+Replace(CONVERT(Varchar,Settlements_EndDateTime,111),'/','-')+'~'+Replace(CONVERT(Varchar,Settlements_FundsPayin,111),'/','-')+'~'+Replace(CONVERT(Varchar,Settlements_FundsPayout,111),'/','-') ValueField,Settlements_Number,Settlements_TypeSuffix,Settlements_StartDateTime,Settlements_EndDateTime,Settlements_FundsPayin,Settlements_FundsPayout from Master_Settlements Where Settlements_FinYear='" + HttpContext.Current.Session["LastFinYear"] + "' and Settlements_ExchangeSegmentID=1 Union Select Ltrim(Rtrim(Isnull(Settlements_Number,'')))+Ltrim(Rtrim(Isnull(Settlements_TypeSuffix,''))) TextField,Ltrim(Rtrim(Isnull(Settlements_Number,'')))+'~'+Ltrim(Rtrim(Isnull(Settlements_TypeSuffix,'')))+'~'+Ltrim(Rtrim(Isnull(Settlements_Type,'')))+'~'+Replace(CONVERT(Varchar,Settlements_StartDateTime,111),'/','-')+'~'+Replace(CONVERT(Varchar,Settlements_StartDateTime,111),'/','-') ValueField,Settlements_Number,Settlements_TypeSuffix,Settlements_StartDateTime,Settlements_EndDateTime,Settlements_FundsPayin,Settlements_FundsPayout from Master_Settlements Where Settlements_FundsPayout>(Select FinYear_StartDate from Master_FinYear Where FinYear_Code='" + HttpContext.Current.Session["LastFinYear"].ToString() + "') and Settlements_ExchangeSegmentID=1 and Settlements_FinYear='" + GetPreviousFinYear() + "') Settlements";
            else
                strQuery_Table = @"(Select Ltrim(Rtrim(Isnull(Settlements_Number,'')))+Ltrim(Rtrim(Isnull(Settlements_TypeSuffix,''))) TextField,Ltrim(Rtrim(Isnull(Settlements_Number,'')))+'~'+Ltrim(Rtrim(Isnull(Settlements_TypeSuffix,'')))+'~'+Ltrim(Rtrim(Isnull(Settlements_Type,'')))+'~'+Replace(CONVERT(Varchar,Settlements_StartDateTime,111),'/','-')+'~'+Replace(CONVERT(Varchar,Settlements_EndDateTime,111),'/','-')+'~'+Replace(CONVERT(Varchar,Settlements_FundsPayin,111),'/','-')+'~'+Replace(CONVERT(Varchar,Settlements_FundsPayout,111),'/','-') ValueField,Settlements_Number,Settlements_TypeSuffix,Settlements_StartDateTime,Settlements_EndDateTime,Settlements_FundsPayin,Settlements_FundsPayout from Master_Settlements Where Settlements_FinYear='" + HttpContext.Current.Session["LastFinYear"] + "' and Settlements_StartDateTime>'" + SpecificStartFrom.ToString("yyyy-MM-dd") + "' and Settlements_ExchangeSegmentID=1 Union Select Ltrim(Rtrim(Isnull(Settlements_Number,'')))+Ltrim(Rtrim(Isnull(Settlements_TypeSuffix,''))) TextField,Ltrim(Rtrim(Isnull(Settlements_Number,'')))+'~'+Ltrim(Rtrim(Isnull(Settlements_TypeSuffix,'')))+'~'+Ltrim(Rtrim(Isnull(Settlements_Type,'')))+'~'+Replace(CONVERT(Varchar,Settlements_StartDateTime,111),'/','-')+'~'+Replace(CONVERT(Varchar,Settlements_StartDateTime,111),'/','-') ValueField,Settlements_Number,Settlements_TypeSuffix,Settlements_StartDateTime,Settlements_EndDateTime,Settlements_FundsPayin,Settlements_FundsPayout from Master_Settlements Where Settlements_FundsPayout>(Select FinYear_StartDate from Master_FinYear Where FinYear_Code='" + HttpContext.Current.Session["LastFinYear"] + "') and Settlements_ExchangeSegmentID=1 and Settlements_FinYear='" + GetPreviousFinYear() + "' and Settlements_StartDateTime>'" + HttpContext.Current.Session["StartdateFundsPayindate"].ToString() + "') Settlements";
            strQuery_FieldName = " Top 10 * ";
            strQuery_OrderBy = "Settlements_StartDateTime";
            strQuery_GroupBy = "";
            strQuery_WhereClause = @"Settlements_Number like '%RequestLetter%' Or Settlements_TypeSuffix like  '%RequestLetter%' Or Convert(Varchar,Settlements_StartDateTime,110) like  '%RequestLetter%' Or Convert(Varchar,Settlements_EndDateTime,110) like  '%RequestLetter%' Or Convert(Varchar,Settlements_FundsPayin,110) like  '%RequestLetter%' Or Convert(Varchar,Settlements_FundsPayout,110) like  '%RequestLetter%'";
            return ReturnCombinedQuery(strQuery_Table, strQuery_FieldName, strQuery_WhereClause, strQuery_GroupBy, strQuery_OrderBy, "Settlements");
        }
        #endregion

        #region ISIN Relavant

        #endregion

        #region Cash/Bank A/C Relavant
        //AllOrSpecific : A/S
        //SpecificSegment : If Specific Then Pass Specific SegmentID(Session["UserSegID"]) To Find Out Bank Or Cash Account Of Same
        //CashOrBankOrCashBank : Pass Cash For Only Cash A/c,Pass Bank For Only Bank A/c,Pass CashBank For Both Cash/Bank A/c
        public string GetCashBankAccAQuery(string AllOrSpecific, string SpecificSegment, string CashOrBankOrCashBank)
        {
            string strSegmentAndBankCashType = String.Empty;
            if (AllOrSpecific == "S")
                strSegmentAndBankCashType = " and (MainAccount_ExchangeSegment=" + SpecificSegment + " or MainAccount_ExchangeSegment=0)";
            if (CashOrBankOrCashBank == "CashBank")
                strSegmentAndBankCashType = strSegmentAndBankCashType + " and (MainAccount_BankCashType='Cash' Or MainAccount_BankCashType='Bank')";
            else
                strSegmentAndBankCashType = strSegmentAndBankCashType + " and (MainAccount_BankCashType='" + CashOrBankOrCashBank + "')";

            string strQuery_Table = "(Select MainAccount_AccountCode+'-'+MainAccount_Name+' [ '+MainAccount_BankAcNumber+' ]'+' ~ '+MainAccount_BankCashType as TextField,MainAccount_AccountCode+'~'+MainAccount_BankCashType ValueField,MainAccount_Name,MainAccount_BankAcNumber,MainAccount_AccountCode from Master_MainAccount where (MainAccount_BankCompany='" + HttpContext.Current.Session["LastCompany"] + "' Or IsNull(MainAccount_BankCompany,'')='')" + strSegmentAndBankCashType + ") as t1";
            string strQuery_FieldName = " Top 10 * ";
            string strQuery_WhereClause = "MainAccount_AccountCode like ('%RequestLetter%') or MainAccount_Name like ('%RequestLetter%') or MainAccount_BankAcNumber like ('%RequestLetter%')";
            string strQuery_OrderBy = "MainAccount_AccountCode";
            string strQuery_GroupBy = null;
            return ReturnCombinedQuery(strQuery_Table, strQuery_FieldName, strQuery_WhereClause, strQuery_GroupBy, strQuery_OrderBy, "CashBank");
        }
        #endregion


        #region Pertaining To User
        public string GetUser()
        {
            string strQuery_Table = "tbl_master_user";
            string strQuery_FieldName = @"top 10 LTRIM(Rtrim(user_loginId))+' ['+Cast(USER_ID as Varchar)+']' UserName,USER_ID ";
            string strQuery_WhereClause = "(LTRIM(Rtrim(user_loginId))+' ['+Cast(USER_ID as Varchar)+']' Like '%RequestLetter%' Or user_name Like '%RequestLetter%') ";
            string strQuery_OrderBy = null;
            string strQuery_GroupBy = null;
            return ReturnCombinedQuery(strQuery_Table, strQuery_FieldName, strQuery_WhereClause, strQuery_GroupBy, strQuery_OrderBy, "ClientDpDetail");
        }
        #endregion
        #endregion

        #region Domain Related
        #region Company Detail
        /// <summary>
        /// All Info About Company like Companies Name,
        /// Companies Exchanges,Company Exchanges ShortName
        /// Companies Segmensts, Companies Segments ShortName
        /// 20120622
        /// </summary>
        /// <returns></returns>

        /////Get Detail Of All Company's All ExchangeSegment
        public DataTable GetCompanyDetail()
        {
            DataTable DtCompanyDetail = new DataTable();
            DtCompanyDetail = GetDataTable("Tbl_Master_Exchange,Tbl_Master_CompanyExchange", @"Ltrim(Rtrim(Exch_CompID)) as CompanyID,
                            (Select Ltrim(RTrim(Cmp_Name)) from Tbl_Master_Company Where Cmp_InternalID=Exch_CompID) as Company,
                            (Select ExchangeSegment_ID From Master_ExchangeSegments Where ExchangeSegment_ExchangeID=Exchange_ID 
                            and ExchangeSegment_Code=Exch_SegmentID) as [Session_ExchangeSegmentID],
                            Exch_InternalID as [Session_UserSegID],Exh_ShortName,Exch_SegmentID from
                            (Select Exch_CompID,Exch_InternalID,Exh_ShortName,Exch_SegmentID", @"Exh_CntId=Exch_ExchID) as T1,Master_Exchange
                            Where Exchange_ShortName=Exh_ShortName");
            return DtCompanyDetail;
        }

        ////Get Detail Of All Company's Specific Segment
        public DataTable GetCompanyDetail(string ExchangeSegmentID, Boolean IsTopOne)
        {
            DataTable DtCompanyDetail = new DataTable();
            if (!IsTopOne)
            {
                DtCompanyDetail = GetDataTable(@"(Select Ltrim(Rtrim(Exch_CompID)) as CompanyID,
                            (Select Ltrim(RTrim(Cmp_Name)) from Tbl_Master_Company Where Cmp_InternalID=Exch_CompID) as Company,
                            (Select ExchangeSegment_ID From Master_ExchangeSegments Where ExchangeSegment_ExchangeID=Exchange_ID 
                            and ExchangeSegment_Code=Exch_SegmentID) as [Session_ExchangeSegmentID],
                            Exch_InternalID as [Session_UserSegID],Exh_ShortName,Exch_SegmentID from
                            (Select Exch_CompID,Exch_InternalID,Exh_ShortName,Exch_SegmentID from Tbl_Master_Exchange,Tbl_Master_CompanyExchange WHERE Exh_CntId=Exch_ExchID) as T1,Master_Exchange
                            Where Exchange_ShortName=Exh_ShortName) T1", "*", @"Session_ExchangeSegmentID=" + ExchangeSegmentID.ToString());
            }
            else
            {
                DtCompanyDetail = GetDataTable(@"(Select Ltrim(Rtrim(Exch_CompID)) as CompanyID,
                            (Select Ltrim(RTrim(Cmp_Name)) from Tbl_Master_Company Where Cmp_InternalID=Exch_CompID) as Company,
                            (Select ExchangeSegment_ID From Master_ExchangeSegments Where ExchangeSegment_ExchangeID=Exchange_ID 
                            and ExchangeSegment_Code=Exch_SegmentID) as [Session_ExchangeSegmentID],
                            Exch_InternalID as [Session_UserSegID],Exh_ShortName,Exch_SegmentID from
                            (Select Exch_CompID,Exch_InternalID,Exh_ShortName,Exch_SegmentID from Tbl_Master_Exchange,Tbl_Master_CompanyExchange WHERE Exh_CntId=Exch_ExchID) as T1,Master_Exchange
                            Where Exchange_ShortName=Exh_ShortName) T1", "Top 1 *", @"Session_ExchangeSegmentID=" + ExchangeSegmentID);
            }
            return DtCompanyDetail;
        }

        //////Get Detail Of Specific Company All ExchangeSegment
        public DataTable GetCompanyDetail(string CompanyID)
        {
            DataTable DtCompanyDetail = new DataTable();
            DtCompanyDetail = GetDataTable(@"(Select Ltrim(Rtrim(Exch_CompID)) as CompanyID,
                            (Select Ltrim(RTrim(Cmp_Name)) from Tbl_Master_Company Where Cmp_InternalID=Exch_CompID) as Company,
                            (Select ExchangeSegment_ID From Master_ExchangeSegments Where ExchangeSegment_ExchangeID=Exchange_ID and ExchangeSegment_Code=Exch_SegmentID) as [Session_ExchangeSegmentID],
                            Exch_InternalID as [Session_UserSegID],Exh_ShortName,Exch_SegmentID from
                            (Select Exch_CompID,Exch_InternalID,Exh_ShortName,Exch_SegmentID from Tbl_Master_Exchange,Tbl_Master_CompanyExchange 
                            Where Exh_CntId=Exch_ExchID) as T1,Master_Exchange
                            Where Exchange_ShortName=Exh_ShortName) as T1", "*", @"CompanyID='" + CompanyID.Trim() + "'");
            return DtCompanyDetail;
        }

        //////Get Detail Of Specific Company Specific UserSegID Like (CM or F0 Etc..)
        public DataTable GetCompanyDetail(string CompanyID, string UserSegID)
        {
            DataTable DtCompanyDetail = new DataTable();
            if (UserSegID.Length == 8 && (UserSegID.StartsWith("IN") || UserSegID.StartsWith("12")))
            {
                DtCompanyDetail = GetDataTable("tbl_master_companyExchange", @"Ltrim(Rtrim(Exch_CompID)) as CompanyID,
            (Select Ltrim(RTrim(Cmp_Name)) from Tbl_Master_Company Where Cmp_InternalID=Exch_CompID) as Company,
            null as [Session_ExchangeSegmentID],
            exch_TmCode as [Session_UserSegID], exch_membershipType as Exh_ShortName,
            (Select Seg_ID from tbl_master_Segment Where Seg_Name=Ltrim(Rtrim(exch_membershipType)))as Exch_SegmentID,
            Exch_InternalID InternalID",
              "exch_compId='" + CompanyID + "' and exch_TMCode in (" + UserSegID + ")");
            }
            else
            {
                DtCompanyDetail = GetDataTable(@"(Select Ltrim(Rtrim(Exch_CompID)) as CompanyID,
            (Select Ltrim(RTrim(Cmp_Name)) from Tbl_Master_Company Where Cmp_InternalID=Exch_CompID) as Company,
            (Select ExchangeSegment_ID From Master_ExchangeSegments Where ExchangeSegment_ExchangeID=Exchange_ID and ExchangeSegment_Code=Exch_SegmentID) as [Session_ExchangeSegmentID],
            Exch_InternalID as [Session_UserSegID],Exh_ShortName,Exch_SegmentID from
            (Select Exch_CompID,Exch_InternalID,Exh_ShortName,Exch_SegmentID from Tbl_Master_Exchange,Tbl_Master_CompanyExchange
            Where Exh_CntId=Exch_ExchID) as T1,Master_Exchange
            Where Exchange_ShortName=Exh_ShortName) as T1", "*", @"CompanyID='" + CompanyID.Trim() + @"'
            and Session_UserSegID in (" + UserSegID + ")");
            }
            return DtCompanyDetail;
        }


        #endregion

        //ALL
        #region GetExchangeDetail
        public DataTable GetExchanges()
        {
            return GetDataTable("Master_Exchange", @"Exchange_ID,Exchange_Name,Exchange_ShortName,Exchange_CountryID,Exchange_IsCommodity,Exchange_CurrencyID,Ltrim(Rtrim(Exchange_Name))+' ['+Ltrim(Rtrim(Exchange_ShortName))+']' as FullName", null);
        }
        ///Exchange With there Segments
        ///AllOrSpecific : Pass A-All ,S-Specific, and SpecificSegmentCode in ('CM','FO',......)
        ///if All Segment Then No Need To Pass Segment
        public DataTable GetExchanges(string AllOrSpecific, string SpecificSegmentCode)
        {
            if (AllOrSpecific == "A")
            {
                ///Exchange With there Segments
                return GetDataTable("Master_Exchange,Master_ExchangeSegments", @"Exchange_ID,Ltrim(Rtrim(Exchange_Name))+' ['+Ltrim(Rtrim(Exchange_ShortName))+']' FullName,
                Exchange_Name,Exchange_ShortName,Exchange_CountryID,Exchange_IsCommodity,Exchange_CurrencyID,ExchangeSegment_ID,
                ExchangeSegment_SegmentType,ExchangeSegment_TradeCurrencyID,ExchangeSegment_Code", "Exchange_ID = ExchangeSegment_ExchangeID");
            }
            else
            {
                ///Exchange With there Specific Segments
                if (SpecificSegmentCode != String.Empty)
                {
                    return GetDataTable("Master_Exchange,Master_ExchangeSegments", @"Exchange_ID,Ltrim(Rtrim(Exchange_Name))+' ['+Ltrim(Rtrim(Exchange_ShortName))+']' FullName,
                    Exchange_Name,Exchange_ShortName,Exchange_CountryID,Exchange_IsCommodity,Exchange_CurrencyID,ExchangeSegment_ID,
                    ExchangeSegment_SegmentType,ExchangeSegment_TradeCurrencyID,ExchangeSegment_Code", @"Exchange_ID = ExchangeSegment_ExchangeID
                    and ExchangeSegment_Code in (" + SpecificSegmentCode + ")");
                }
            }
            return null;
        }





        //Now Exchange That A Company Has
        public DataTable GetExchanges(string CompanyID)
        {
            return GetDataTable(@"(Select Exchange_ID,Exchange_Name,Exchange_ShortName,Exchange_CountryID,
                                                Exchange_IsCommodity,Exchange_CurrencyID,Ltrim(Rtrim(Exchange_Name))+
                                                ' ['+Ltrim(Rtrim(Exchange_ShortName))+']' as FullName from Master_Exchange) T1
                                                Inner Join (Select Exh_ShortName from Tbl_Master_Exchange,Tbl_Master_CompanyExchange 
                                                Where Exh_CntId=Exch_ExchID and Exch_CompID='" + CompanyID + @"') T2
                                                On Ltrim(Rtrim(Exchange_ShortName))=Ltrim(Rtrim(exh_shortName))
                                                and Ltrim(Rtrim(Exchange_ShortName)) not Like '%Account%'",
                                        @"Distinct Exchange_ID,Exchange_Name,Exchange_ShortName,Exchange_CountryID,
                                                Exchange_IsCommodity,Exchange_CurrencyID,FullName", null);
        }

        #endregion

        #region GetSegmentDetail
        //All Segments
        public DataTable GetSegments()
        {
            return GetDataTable(@"(Select  Distinct Exchange_ID,exch_segmentId SegmentName,exch_internalId SegmentID,Company From
                                                    (Select Exchange_ID,Exchange_Name,Exchange_ShortName,Exchange_CountryID,Exchange_IsCommodity,
                                                    Exchange_CurrencyID,Ltrim(Rtrim(Exchange_Name))+' ['+Ltrim(Rtrim(Exchange_ShortName))+']' as FullName 
                                                    from Master_Exchange) T1
                                                    Inner Join 
                                                    (Select Exh_ShortName,exch_segmentId,exh_id,exh_name,exch_internalId,Exch_CompID Company
                                                    from Tbl_Master_Exchange,Tbl_Master_CompanyExchange 
                                                    Where Exh_CntId=Exch_ExchID) T2
                                                    On Ltrim(Rtrim(Exchange_ShortName))=Ltrim(Rtrim(exh_shortName))) T3
                                                    Inner Join
                                                    (Select * from Master_ExchangeSegments) T4
                                                    On Exchange_ID=ExchangeSegment_ExchangeID and 
                                                    ExchangeSegment_Code=SegmentName",
                                            @"T3.*,ExchangeSegment_SegmentType SegmentFullName,ExchangeSegment_ClearingHouseID ClearingHouseID,
                                                    ExchangeSegment_TradeCurrencyID TradeCurrencyID", null);
        }
        //All Segment and Specific Company
        public DataTable GetSegments(string CompanyID)
        {
            return GetDataTable(@"(Select  Distinct Exchange_ID,exch_segmentId SegmentName,exch_internalId SegmentID,Company From
                                                        (Select Exchange_ID,Exchange_Name,Exchange_ShortName,Exchange_CountryID,Exchange_IsCommodity,
                                                        Exchange_CurrencyID,Ltrim(Rtrim(Exchange_Name))+' ['+Ltrim(Rtrim(Exchange_ShortName))+']' as FullName 
                                                        from Master_Exchange) T1
                                                        Inner Join 
                                                        (Select Exh_ShortName,exch_segmentId,exh_id,exh_name,exch_internalId,Exch_CompID Company
                                                        from Tbl_Master_Exchange,Tbl_Master_CompanyExchange 
                                                        Where Exh_CntId=Exch_ExchID and Exch_CompID='" + CompanyID + @"') T2
                                                        On Ltrim(Rtrim(Exchange_ShortName))=Ltrim(Rtrim(exh_shortName))) T3
                                                        Inner Join
                                                        (Select * from Master_ExchangeSegments) T4
                                                        On Exchange_ID=ExchangeSegment_ExchangeID and 
                                                        ExchangeSegment_Code=SegmentName",
                                                @"T3.*,ExchangeSegment_SegmentType SegmentFullName,ExchangeSegment_ClearingHouseID ClearingHouseID,
                                                        ExchangeSegment_TradeCurrencyID TradeCurrencyID", null);
        }
        //All Segment and Specific Company and Specific Exchange
        public DataTable GetSegments(string CompanyID, string ExchangeID)
        {
            return GetDataTable(@"(Select  Distinct Exchange_ID,exch_segmentId SegmentName,exch_internalId SegmentID,Company From
                                                        (Select Exchange_ID,Exchange_Name,Exchange_ShortName,Exchange_CountryID,Exchange_IsCommodity,
                                                        Exchange_CurrencyID,Ltrim(Rtrim(Exchange_Name))+' ['+Ltrim(Rtrim(Exchange_ShortName))+']' as FullName 
                                                        from Master_Exchange Where Exchange_ID='" + ExchangeID + @"') T1
                                                        Inner Join 
                                                        (Select Exh_ShortName,exch_segmentId,exh_id,exh_name,exch_internalId,Exch_CompID Company
                                                        from Tbl_Master_Exchange,Tbl_Master_CompanyExchange 
                                                        Where Exh_CntId=Exch_ExchID and Exch_CompID='" + CompanyID + @"') T2
                                                        On Ltrim(Rtrim(Exchange_ShortName))=Ltrim(Rtrim(exh_shortName))) T3
                                                        Inner Join
                                                        (Select * from Master_ExchangeSegments) T4
                                                        On Exchange_ID=ExchangeSegment_ExchangeID and 
                                                        ExchangeSegment_Code=SegmentName",
                                            @"T3.*,ExchangeSegment_SegmentType SegmentFullName,ExchangeSegment_ClearingHouseID ClearingHouseID,
                                                        ExchangeSegment_TradeCurrencyID TradeCurrencyID", null);
        }
        public string GetSegmentName()
        {
            return GetDataTable(@"(Select Exch_CompID,Exch_InternalID,Exh_ShortName,Exch_SegmentID from Tbl_Master_Exchange,
            Tbl_Master_CompanyExchange Where Exh_CntId=Exch_ExchID) as T1,Master_Exchange", @"
            isnull(Exch_SegmentID ,'') SegmentName", "Exchange_ShortName=Exh_ShortName and exch_internalId='" +
            HttpContext.Current.Session["UserSegID"].ToString() + "'", "Exchange_ID").Rows[0][0].ToString();
        }
        #endregion

        #region GetCurrrency
        public DataTable GetCurrency()
        {
            return GetDataTable("Master_Currency", @"Currency_ID,Currency_Name,Currency_Symbol,Currency_AlphaCode,Currency_InternationalCode,
            Currency_DecimalCharacter,Currency_DecimalPlaces,Currency_DecimalPortionName,Currency_DecimalPortionSymbol,Currency_AmountDisplayBasis", "");
        }
        #endregion

        #region GetClient/Employee/Broker Detail Relavant
        public DataTable GetDpDetail(string ClientID, ClientIDType ClientIDType)
        {
            if (ClientIDType.ToString() == "InternalID")
                return GetDataTable("tbl_master_contactdpdetails", @"cast(dpd_id as varchar)+'~'+dpd_dpCode+'~'+dpd_ClientID+'~'+
            isnull(dpd_cmbpid,'N/A') AS id,(select rtrim(replace(dp_dpname,char(160),'')) from tbl_master_depositoryparticipants 
            where substring(dp_dpID,1,8)=dpd_dpCode)+' ['+ rtrim(dpd_ClientID)+']' AS Name,rtrim(dpd_accountType) as dpd_accountType",
            " dpd_cntId='" + ClientID + "'", " dpd_accountType ");

            return null;
        }
        #endregion

        #region GetExchangeSegmentDetail
        public DataTable GetExchangeSegmentDetail()
        {
            return GetDataTable(@"(Select ExchangeSegment_ID From Master_ExchangeSegments Where ExchangeSegment_ExchangeID=Exchange_ID and ExchangeSegment_Code=Exch_SegmentID) as [Session_ExchangeSegmentID],
            Exch_InternalID as [Session_UserSegID],Exh_ShortName,Exch_SegmentID,
            isnull(Exh_ShortName,'')+
            Case When isnull(Exch_SegmentID ,'')!='' Then ' - ' Else '' End
            +isnull(Exch_SegmentID ,'') SegmentName", "Exchange_ShortName=Exh_ShortName", "Exchange_ID");
        }
        public DataTable GetExchangeSegmentDetail(string SpecificExchangeSegment)
        {
            return GetDataTable(@"(Select ExchangeSegment_ID From Master_ExchangeSegments Where ExchangeSegment_ExchangeID=Exchange_ID and ExchangeSegment_Code=Exch_SegmentID) as [Session_ExchangeSegmentID],
            Exch_InternalID as [Session_UserSegID],Exh_ShortName,Exch_SegmentID,
            isnull(Exh_ShortName,'')+
            Case When isnull(Exch_SegmentID ,'')!='' Then ' - ' Else '' End
            +isnull(Exch_SegmentID ,'') SegmentName",
            "Exchange_ShortName=Exh_ShortName and exch_internalId in (" + SpecificExchangeSegment + ")", "Exchange_ID");
        }
        public string GetExchangeSegmentName()
        {
            return GetDataTable(@"(Select Exch_CompID,Exch_InternalID,Exh_ShortName,Exch_SegmentID from Tbl_Master_Exchange,
            Tbl_Master_CompanyExchange Where Exh_CntId=Exch_ExchID) as T1,Master_Exchange", @"
            isnull(Exh_ShortName,'')+Case When isnull(Exch_SegmentID ,'')!='' Then ' - ' Else '' End
            +isnull(Exch_SegmentID ,'') SegmentName", "Exchange_ShortName=Exh_ShortName and exch_internalId='" +
            HttpContext.Current.Session["UserSegID"].ToString() + "'", "Exchange_ID").Rows[0][0].ToString();
        }
        public DataTable GetExchangeSegmentName(string UserSegIDs)
        {
            return GetDataTable(@"(Select Exch_CompID,Exch_InternalID,Exh_ShortName,Exch_SegmentID from Tbl_Master_Exchange,
            Tbl_Master_CompanyExchange Where Exh_CntId=Exch_ExchID) as T1,Master_Exchange", @"
            isnull(Exh_ShortName,'')+Case When isnull(Exch_SegmentID ,'')!='' Then ' - ' Else '' End
            +isnull(Exch_SegmentID ,'') SegmentName", "Exchange_ShortName=Exh_ShortName and exch_internalId in (" +
            UserSegIDs + ")", "Exchange_ID");
        }

        //ABOUT : GetExchangeSegment_FullDetail
        //DTOrAQuery    : DataTable Or Ajax Query
        //NoofTopRow    : No of Top Row Only in Case Of Ajax Query
        //Version       : We Can Have More Part Of It So Usefull For More Extention Now Veraion 1 Available  
        //CustomizeWhere: You Can Your Own Make Where Clause After Seeing Query Result By Sending Null Value Of Where;
        //                Example --> CompanyID='COR0000002' and SegID=24
        //CustomizeSelection: ExchangeSegmentID,SegmentID
        //CustomizeOrderBy:CompanyID,SegmentID
        //CustomizeGroupBy:ExchangeSegmentID
        //For Version 1 : You Can Get 
        //--CompanyID : International Commodity & Currency Arbitrage [COR0000002]	
        //--ExchangeSegmentID:7
        //--UserSegID:4	
        //--Exh_ShorName:BSE	
        //--SegmentID:CM
        public DataTable GetExchangeSegment_FullDetail(string DTOrAQuery, ref string AQuery, int NoOfTopRows, string CustomizeWhere, string CustomizeSelection, string CustomizeOrderBy, string CustomizeGroupBy, string[] AjaxLikeFields, int Version)
        {
            if (Version == 1)
            {
                strQuery_Table = @"(Select (Select Ltrim(RTrim(Cmp_Name))+' ['+Cmp_InternalID+']' from Tbl_Master_Company Where Cmp_InternalID=Exch_CompID) as Company,Exch_CompID,(Select ExchangeSegment_ID From Master_ExchangeSegments Where ExchangeSegment_ExchangeID=Exchange_ID and ExchangeSegment_Code=Exch_SegmentID) as [ExchangeSegmentID],Exch_InternalID as [UserSegID],Exh_ShortName,Exch_SegmentID from (Select Exch_CompID,Exch_InternalID,Exh_ShortName,Exch_SegmentID from Tbl_Master_Exchange,Tbl_Master_CompanyExchange Where Exh_CntId=Exch_ExchID) as T1,Master_Exchange Where Exchange_ShortName=Exh_ShortName) ExchangeFullDetail";
                strQuery_FieldName = "Top " + NoOfTopRows.ToString() + CustomizeSelection;
                strQuery_OrderBy = CustomizeOrderBy;
                strQuery_GroupBy = CustomizeGroupBy;
                strQuery_WhereClause = null;
                if (DTOrAQuery == "D")
                {
                    //Remove Top 10 Not Required For DataTable
                    strQuery_FieldName = strQuery_FieldName.Replace("Top " + NoOfTopRows.ToString(), "");
                    strQuery_WhereClause = CustomizeWhere;
                    return GetDataTable(strQuery_Table, strQuery_FieldName, strQuery_WhereClause, strQuery_OrderBy);
                }
                else
                {
                    foreach (string strALF in AjaxLikeFields)
                    {
                        if (strQuery_WhereClause == null)
                            strQuery_WhereClause = strALF + "Like '%RequestLetter%'";
                        else
                            strQuery_WhereClause = strQuery_WhereClause + " And " + strALF + "Like '%RequestLetter%'";
                    }
                    strQuery_WhereClause = strQuery_WhereClause + " And (" + CustomizeWhere + ")";
                    AQuery = ReturnCombinedQuery(strQuery_Table, strQuery_FieldName, strQuery_WhereClause, strQuery_GroupBy, strQuery_OrderBy, "ClientFullDetail");
                }
            }
            return null;
        }
        #endregion

        #region DpAccounts


        public DataTable GetDpAccountsInfo(string AccountID)
        {
            string strQuery_Table = "Master_DPAccounts";
            string strQuery_FieldName = @" DPAccounts_ShortName TextField,Cast(DPAccounts_ID as Varchar(10))DPAccounts_ID,Ltrim(Rtrim(isnull(DPAccounts_AccountName,'')))DPAccounts_AccountName,
                                    Ltrim(Rtrim(isnull(DPAccounts_AccountType,'')))DPAccounts_AccountType,Ltrim(Rtrim(Cast(isnull(DPAccounts_ExchangeSegmentID,'') as Varchar(10))))DPAccounts_ExchangeSegmentID,
                                    Ltrim(Rtrim(isnull(DPAccounts_DPID,'')))DPAccounts_DPID,Ltrim(Rtrim(isnull(DPAccounts_ClientID,''))) DPAccounts_ClientID,Ltrim(Rtrim(isnull(DPAccounts_CMBPID,''))) DPAccounts_CMBPID,
                                    Ltrim(Rtrim(isnull(DPAccounts_CompanyID,'')))  DPAccounts_CompanyID";
            string strQuery_OrderBy = "DPAccounts_AccountType";
            string strQuery_GroupBy = null;
            string strQuery_WhereClause = "DPAccounts_ID in (" + AccountID + ")";
            return GetDataTable(strQuery_Table, strQuery_FieldName, strQuery_WhereClause, strQuery_OrderBy);
        }


        ///Note 1 : if AllOrSpecificAccountType and AllOrSpecificSegmentID is ALL ("A") Not Specific "S" Pass String.Empty in 
        ///SpecificDpAccountType and SpecificSegmentID;
        ///Note 2 : SpecificDpAccountType {'[POOL]','[MRGIN]',.......s.....}
        ///Note 3 : SpecificSegmentID {Differnt For Different Company,24,29,......}
        ///
        public DataTable GetDpAccounts(string AllOrSpecificAccountType, string AllOrSpecificSegmentID, string SpecificDpAccountType, string SpecificSegmentID)
        {
            if (AllOrSpecificAccountType == "A" && AllOrSpecificSegmentID == "A")
            {
                return GetDataTable("Master_DPAccounts", @"DPAccounts_ShortName TextField,Cast(DPAccounts_ID as Varchar(10)),Ltrim(Rtrim(isnull(DPAccounts_AccountName,''))),
                    Ltrim(Rtrim(isnull(DPAccounts_AccountType,''))),Ltrim(Rtrim(Cast(isnull(DPAccounts_ExchangeSegmentID,'') as Varchar(10)))),
                    Ltrim(Rtrim(isnull(DPAccounts_DPID,''))),Ltrim(Rtrim(isnull(DPAccounts_ClientID,''))),Ltrim(Rtrim(isnull(DPAccounts_CMBPID,''))),
                    Ltrim(Rtrim(isnull(DPAccounts_CompanyID,''))) ValueField", "DPAccounts_CompanyID='" + HttpContext.Current.Session["LastCompany"].ToString() + "'",
                    "DPAccounts_AccountType");
            }
            else if (AllOrSpecificAccountType == "S" && AllOrSpecificSegmentID == "A")
            {
                return GetDataTable("Master_DPAccounts", @"DPAccounts_ShortName TextField,Cast(DPAccounts_ID as Varchar(10)),Ltrim(Rtrim(isnull(DPAccounts_AccountName,''))),
                    Ltrim(Rtrim(isnull(DPAccounts_AccountType,''))),Ltrim(Rtrim(Cast(isnull(DPAccounts_ExchangeSegmentID,'') as Varchar(10)))),
                    Ltrim(Rtrim(isnull(DPAccounts_DPID,''))),Ltrim(Rtrim(isnull(DPAccounts_ClientID,''))),Ltrim(Rtrim(isnull(DPAccounts_CMBPID,''))),
                    Ltrim(Rtrim(isnull(DPAccounts_CompanyID,''))) ValueField",
                    "DPAccounts_AccountType in (" + SpecificDpAccountType + ") And DPAccounts_CompanyID='" + HttpContext.Current.Session["LastCompany"].ToString() + "'",
                    "DPAccounts_AccountType");
            }
            else if (AllOrSpecificAccountType == "A" && AllOrSpecificSegmentID == "S")
            {
                return GetDataTable("Master_DPAccounts", @"DPAccounts_ShortName TextField,Cast(DPAccounts_ID as Varchar(10)),Ltrim(Rtrim(isnull(DPAccounts_AccountName,''))),
                    Ltrim(Rtrim(isnull(DPAccounts_AccountType,''))),Ltrim(Rtrim(Cast(isnull(DPAccounts_ExchangeSegmentID,'') as Varchar(10)))),
                    Ltrim(Rtrim(isnull(DPAccounts_DPID,''))),Ltrim(Rtrim(isnull(DPAccounts_ClientID,''))),Ltrim(Rtrim(isnull(DPAccounts_CMBPID,''))),
                    Ltrim(Rtrim(isnull(DPAccounts_CompanyID,''))) ValueField",
                    "DPAccounts_ExchangeSegmentID in (" + SpecificSegmentID + ",0) And DPAccounts_CompanyID='" + HttpContext.Current.Session["LastCompany"].ToString() + "'",
                    "DPAccounts_AccountType");
            }
            else if (AllOrSpecificAccountType == "S" && AllOrSpecificSegmentID == "S")
            {
                return GetDataTable("Master_DPAccounts", @"DPAccounts_ShortName TextField,Cast(DPAccounts_ID as Varchar(10)),Ltrim(Rtrim(isnull(DPAccounts_AccountName,''))),
                    Ltrim(Rtrim(isnull(DPAccounts_AccountType,''))),Ltrim(Rtrim(Cast(isnull(DPAccounts_ExchangeSegmentID,'') as Varchar(10)))),
                    Ltrim(Rtrim(isnull(DPAccounts_DPID,''))),Ltrim(Rtrim(isnull(DPAccounts_ClientID,''))),Ltrim(Rtrim(isnull(DPAccounts_CMBPID,''))),
                    Ltrim(Rtrim(isnull(DPAccounts_CompanyID,''))) ValueField",
                    "DPAccounts_AccountType in (" + SpecificDpAccountType + ") And DPAccounts_ExchangeSegmentID in (" + SpecificSegmentID + ",0) And DPAccounts_CompanyID='" + HttpContext.Current.Session["LastCompany"].ToString() + "'",
                    "DPAccounts_AccountType");
            }
            return null;
        }
        #endregion

        ///Note : It Will Show All Or Specific Settlement 
        ///if AllOrSpecific "S"  a Date Settlement Will Be Greater and Equal to That Date
        ///if AllOrSpecific "A" Pass Min Date DateTime.MinValue
        #region Settlements(Except NSDL/CDSL)
        public DataTable GetSettlements(string AllOrSpecific, DateTime SettlementStartFrom)
        {
            if (AllOrSpecific == "A")
            {
                return GetDataTable("", "", "");
            }
            return null;
        }
        #endregion

        #region FinYear
        public string GetPreviousFinYear()
        {
            return GetDataTable("Master_FinYear", "FinYear_Code", @"FinYear_EndDate=Cast('" + HttpContext.Current.Session["FinYearStart"].ToString() + "' as DateTime)-1").Rows[0][0].ToString();
        }
        #endregion


        #endregion

        #region Call Scalar and Table Value Function
        public DataSet CallGeneric_TableValueFunction_DataSet(string FunctionName, string Parameters)
        {
            //Call Like ParamName|DataType(enum)|Size|ParamType(ExParam,XParam) enum
            string[] strSpParam = new string[2];
            strSpParam[0] = "FunctionName|" + GenericStoreProcedure.ParamDBType.Varchar + "|250|" + FunctionName + "|" + GenericStoreProcedure.ParamType.ExParam;
            strSpParam[1] = "Parameters|" + GenericStoreProcedure.ParamDBType.Varchar + "|1000|" + Parameters + "|" + GenericStoreProcedure.ParamType.ExParam;

            oGenericStoreProcedure = new GenericStoreProcedure();
            return oGenericStoreProcedure.Procedure_DataSet(strSpParam, "GenericTableValueFunction");

        }
        public DataTable CallGeneric_TableValueFunction_DataTable(string FunctionName, string Parameters)
        {
            DataSet DsReturn = new DataSet();
            //Call Like ParamName|DataType(enum)|Size|ParamType(ExParam,XParam) enum
            string[] strSpParam = new string[2];
            strSpParam[0] = "CallFor|" + GenericStoreProcedure.ParamDBType.Varchar + "|250|" + FunctionName + "|" + GenericStoreProcedure.ParamType.ExParam;
            strSpParam[1] = "Parameters|" + GenericStoreProcedure.ParamDBType.Varchar + "|1000|" + Parameters + "|" + GenericStoreProcedure.ParamType.ExParam;

            oGenericStoreProcedure = new GenericStoreProcedure();
            DsReturn = oGenericStoreProcedure.Procedure_DataSet(strSpParam, "GenericTableValueFunction");
            if (DsReturn.Tables.Count > 0)
                if (DsReturn.Tables[0].Rows.Count > 0)
                    return DsReturn.Tables[0];

            return null;
        }
        public int CallGeneric_ScalerFunction_Int(string FunctionName, string Parameters)
        {

            //Call Like ParamName|DataType(enum)|Size|ParamType(ExParam,XParam) enum
            string[] strSpParam = new string[2];
            strSpParam[0] = "FunctionName|" + GenericStoreProcedure.ParamDBType.Varchar + "|250|" + FunctionName + "|" + GenericStoreProcedure.ParamType.ExParam;
            strSpParam[1] = "Parameters|" + GenericStoreProcedure.ParamDBType.Varchar + "|1000|" + Parameters + "|" + GenericStoreProcedure.ParamType.ExParam;

            oGenericStoreProcedure = new GenericStoreProcedure();
            return oGenericStoreProcedure.Procedure_Int(strSpParam, "GenericFunction");

        }
        public string CallGeneric_ScalerFunction_String(string FunctionName, string Parameters)
        {

            //Call Like ParamName|DataType(enum)|Size|ParamType(ExParam,XParam) enum
            string[] strSpParam = new string[2];
            strSpParam[0] = "FunctionName|" + GenericStoreProcedure.ParamDBType.Varchar + "|250|" + FunctionName + "|" + GenericStoreProcedure.ParamType.ExParam;
            strSpParam[1] = "Parameters|" + GenericStoreProcedure.ParamDBType.Varchar + "|1000|" + Parameters + "|" + GenericStoreProcedure.ParamType.ExParam;

            oGenericStoreProcedure = new GenericStoreProcedure();
            return oGenericStoreProcedure.Procedure_String(strSpParam, "GenericFunction");

        }
        public DataTable CallGeneric_StoreProcedure(string WhichCall, string Parameters)
        {
            //Call Like ParamName|DataType(enum)|Size|ParamType(ExParam,XParam) enum
            string[] strSpParam = new string[2];
            strSpParam[0] = "WhichCall|" + GenericStoreProcedure.ParamDBType.Varchar + "|250|" + WhichCall + "|" + GenericStoreProcedure.ParamType.ExParam;
            strSpParam[1] = "Parameters|" + GenericStoreProcedure.ParamDBType.Varchar + "|-1|" + Parameters + "|" + GenericStoreProcedure.ParamType.ExParam;

            oGenericStoreProcedure = new GenericStoreProcedure();
            return oGenericStoreProcedure.Procedure_DataTable(strSpParam, "GenericStoreProcedure");
        }

        #endregion

        #region UTILITY

        //This Method Remove UnWanted Sessions
        public void PageInitializer(WhichCall pWhichCall, string[] Parameters)
        {
            if (Parameters != null)
            {
                if (pWhichCall.ToString() == "DistroyUnWantedSession_AllExceptPage")
                {
                    string[] SessionArrayToDistroy ={
                //Mangement->CashBankEntry.cs
                "Mode","CashBankVoucherFile_XMLPATH","TotalBankBalance","ChoosenCurrency",
                //Mangement->Employee.cs
               "PageSize", "FromDOJ", "ToDoj", "PageNumAfterNav", "SerachString", "SearchBy", "FindOption", 
                "ShowFilter_SearchString",
                //Management->Employee_EmployeeDetailsView.aspx  
                "PageNum",
                //Reports->QuarterlySettlementOfFunds.aspx  
                "dp"
                };

                    bool isStrExists = false;

                    foreach (string strCompare1 in SessionArrayToDistroy)
                    {
                        foreach (string strCompare2 in Parameters)
                        {
                            if (strCompare1 == strCompare2)
                                isStrExists = true;
                        }
                        if (!isStrExists)
                            HttpContext.Current.Session.Remove(strCompare1);
                    }
                }
                if (pWhichCall.ToString() == "DistroyUnWantedSession_Page")
                {
                    foreach (string strSession in Parameters)
                    {
                        HttpContext.Current.Session.Remove(strSession);
                    }
                }
            }
            if (pWhichCall.ToString() == "DistroyUnWantedSession_ALL")
            {
                HttpContext.Current.Session.Clear();
                HttpContext.Current.Session.Abandon();
            }

        }

        //This Method is for To Find Out If Product Expired
        public string IsProductExpired(DateTime ExpiryCompareDateTime)
        {
            DateTime ExpiryDate = Convert.ToDateTime(HttpContext.Current.Session["ExpireDate"] != null ? HttpContext.Current.Session["ExpireDate"].ToString() : "2000-01-01");
            if (ExpiryCompareDateTime > ExpiryDate)
                return "true~Licency Expired.Please Renew Your Licence for No Further Interruption!!!.Sorry For InConvenience.";

            return "false~";
        }

        //Write Flat File From Table
        public bool WriteFile(DataTable Dt, string ExportedFilePath, Boolean IsDownload)
        {
            //Check For Existence If Exist then Delete
            DeleteExistingFile(ExportedFilePath);
            FileInfo FI = new FileInfo(ExportedFilePath);
            string FileFullPath = FI.FullName;
            string FileName = Path.GetFileName(FileFullPath);
            try
            {
                using (StreamWriter sw = new StreamWriter(HttpContext.Current.Server.MapPath(ExportedFilePath), true))
                {

                    int colCount = Dt.Columns.Count;

                    foreach (DataRow dr in Dt.Rows)
                    {
                        for (int j = 0; j < colCount; j++)
                        {

                            if (!Convert.IsDBNull(dr[j]))
                                sw.Write(dr[j]);


                        }

                        sw.Write(sw.NewLine);
                    }


                }
                if (IsDownload)
                    DownloadFile(ExportedFilePath, FileName);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        //Write Flat File From String
        public bool WriteFile(string FileContent, string ExportedFilePath, Boolean IsDownload)
        {
            //Check For Existence If Exist then Delete
            DeleteExistingFile(ExportedFilePath);

            FileInfo FI = new FileInfo(ExportedFilePath);
            string FileFullPath = FI.FullName;
            string FileName = Path.GetFileName(FileFullPath);
            try
            {
                using (StreamWriter sw = new StreamWriter(HttpContext.Current.Server.MapPath(ExportedFilePath), true))
                {
                    sw.Write(FileContent);
                    sw.Write(sw.NewLine);
                }
                if (IsDownload)
                {
                    GC.GetTotalMemory(false);
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    GC.Collect();
                    GC.GetTotalMemory(true);
                    DownloadFile(ExportedFilePath, FileName);
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool AppendFile(DataTable Dt, string ExportedFilePath, Boolean IsDownload)
        {
            FileInfo FI = new FileInfo(ExportedFilePath);
            string FileFullPath = FI.FullName;
            string FileName = Path.GetFileName(FileFullPath);
            try
            {
                using (StreamWriter sw = File.AppendText(HttpContext.Current.Server.MapPath(ExportedFilePath)))
                {

                    int colCount = Dt.Columns.Count;

                    foreach (DataRow dr in Dt.Rows)
                    {
                        for (int j = 0; j < colCount; j++)
                        {

                            if (!Convert.IsDBNull(dr[j]))
                                sw.Write(dr[j]);


                        }

                        sw.Write(sw.NewLine);
                    }


                }
                if (IsDownload)
                    DownloadFile(ExportedFilePath, FileName);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        //Append Flat File From String
        //DownLoad File Please Give File Name With Its Extention Like FileName.txt,FileName.docx etc
        public bool AppendFile(string FileContent, string ExportedFilePath, Boolean IsDownload)
        {
            FileInfo FI = new FileInfo(ExportedFilePath);
            string FileFullPath = FI.FullName;
            string FileName = Path.GetFileName(FileFullPath);
            try
            {
                using (StreamWriter sw = File.AppendText(HttpContext.Current.Server.MapPath(ExportedFilePath)))
                {
                    sw.Write(FileContent);
                    sw.Write(sw.NewLine);
                }
                if (IsDownload)
                {
                    GC.GetTotalMemory(false);
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    GC.Collect();
                    GC.GetTotalMemory(true);
                    DownloadFile(ExportedFilePath, FileName);
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public void DownloadFile(string ExportedFilePath, string FileName)
        {
            try
            {
                FileInfo fileInfo = new FileInfo(HttpContext.Current.Server.MapPath(ExportedFilePath));
                if (fileInfo.Exists)
                {
                    HttpContext.Current.Response.Clear();
                    HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=" + FileName);
                    HttpContext.Current.Response.AddHeader("Content-Length", fileInfo.Length.ToString());
                    HttpContext.Current.Response.ContentType = "application/unknown";
                    HttpContext.Current.Response.Flush();
                    HttpContext.Current.Response.WriteFile(fileInfo.FullName);
                    HttpContext.Current.Response.End();
                }
            }
            catch { }
        }
        public void DeleteExistingFile(string ExportedFilePath)
        {
            string FilePath = HttpContext.Current.Server.MapPath(ExportedFilePath);
            if (File.Exists(FilePath))
                File.Delete(FilePath);
        }
        public bool IsRowExists(string BetweenSelectFrom, string BetweenFromWhere, string AfterWhere, string AfterOrderBy, string AfterGroupBy)
        {
            DataTable DtIsRowExist = new DataTable();
            DtIsRowExist = GetDataTable(BetweenFromWhere, BetweenSelectFrom, AfterWhere, AfterGroupBy, AfterOrderBy);
            if (DtIsRowExist.Rows.Count > 0)
                return true;
            return false;
        }

        DataSet ExcelToDataSet(string FilePath, string NameofSheet)
        {
            string strSQL;
            string strConn;
            strConn = "Provider=Microsoft.Jet.OLEDB.4.0;" +
            "Data Source='" + FilePath + "';" +
            "Extended Properties=Excel 8.0;";

            strSQL = "SELECT * FROM [" + NameofSheet + "$]";
            DataSet dsExcel = new DataSet();
            OleDbDataAdapter daExcel = new OleDbDataAdapter(strSQL, strConn);
            daExcel.Fill(dsExcel);
            return dsExcel;
        }

        public string ConvertSpecialCharStringForDB(string stringitem)
        {
            string rtn = "";
            for (int i = 0; i < stringitem.Length; i++)
            {
                if (stringitem[i].ToString() == "'")
                {
                    if (i != 0 && i != stringitem.Length - 1)
                    {
                        if (stringitem[i + 1].ToString() == "," || stringitem[i - 1].ToString() == "," || stringitem[i + 1].ToString() == "=" || stringitem[i - 1].ToString() == "=" || stringitem[i + 1].ToString() == "'" || stringitem[i - 1].ToString() == "'" || stringitem[i + 1].ToString() == " " || stringitem[i - 1].ToString() == " ")
                        {
                            rtn += stringitem[i];
                        }
                        else
                            rtn += stringitem[i] + "'";
                    }
                    else
                        rtn += stringitem[i];
                }
                else
                    rtn += stringitem[i];
            }
            return rtn;
        }

        public bool IsDirectoryExist(string DirectoryPath)
        {
            bool result = false;
            DirectoryInfo directoryInfo = new DirectoryInfo(HttpContext.Current.Server.MapPath(DirectoryPath));
            if (directoryInfo.Exists)
                result = true;
            //directoryInfo.Create();
            return result;
        }
        public bool IsDirectoryExist(string DirectoryPath, Boolean IsDirectoryCreate)
        {
            bool result = false;
            DirectoryInfo directoryInfo = new DirectoryInfo(HttpContext.Current.Server.MapPath(DirectoryPath));
            if (!directoryInfo.Exists)
            {
                if (IsDirectoryCreate == true)
                    directoryInfo.Create();
            }
            if (directoryInfo.Exists)
                result = true;
            return result;
        }
        //public bool IsDirectoryEmpty(string DirectoryPath)
        //{
        //    string[] Files = Directory.GetFiles(DirectoryPath);
        //    if (Files.Length > 0) return false; else return true; return false;

        //}
        //public string ConvertToZip(
        //string ExportedPath,//Full File Path {(FolderPath+FilePath)/FolderPath }
        //string FileName, //The Name of The File With Extension/Without Extension Which Will Be Exported
        //string ZipName, //The Name Of the Zip file/Folder To be made
        //ZipFileType IsDirectoryOrFile, //File or Folder
        //bool DelActualFile
        //)
        //{
        //    zip = new ZipFile();
        //    string inputPath = null;
        //    string inputName = null;
        //    string inputExt = null;
        //    string compressPath = null;

        //    inputName = Path.GetFileName(FileName);
        //    inputPath = Path.GetDirectoryName(ExportedPath);
        //    inputExt = Path.GetExtension(FileName).ToString();

        //    if (inputExt != ".zip" && inputExt != ".rar")
        //    {
        //        //******Uploaded Normal  Save in Server Path With Formatted Compress *******
        //        compressPath = inputPath + "\\" + ZipName + ".zip";
        //        //using (Zip zip = new Zip())
        //        //{
        //        //// Sample===zip.Add("D:\\Reports\\2008-Regional-Sales-Report.pdf", "s");
        //        if (IsDirectoryOrFile.ToString() == "File")
        //            zip.AddFile(ExportedPath, ZipName);
        //        else
        //            zip.AddDirectory(ExportedPath, ZipName);
        //        zip.Save(compressPath);
        //        //}
        //        //if (DelActualFile)
        //        //{
        //        //    if (IsDirectoryOrFile.ToString() == "File")
        //        //        File.Delete(ExportedPath);
        //        //    else
        //        //    {
        //        //        Array.ForEach(Directory.GetFiles(ExportedPath), delegate(string path)
        //        //        {File.Delete(path);});
        //        //    }
        //        //    if(IsDirectoryEmpty(ExportedPath))
        //        //        Directory.Delete(ExportedPath);
        //        //}
        //    }
        //    return (compressPath); //Returning the Path Of the  which has been compressed
        //}

        #endregion

        ///Some Modification The Way To Fetch The Method
        ///I InCorporate Both, Query Or DataTable, To Fetch As Required
        #region AjaxMethods_DomanRelated

        #region ISIN
        ///DTOrAQuery D : DataTable || A : AQuery
        ///NoOfTopRows For AQuery
        public DataTable GetIsInNumber(string DTOrAQuery, ref string AQuery, int ProductID, int ProductSeriesID, int NoOfTopRows)
        {
            string strQuery_Table = "Master_ISIN";
            string strQuery_FieldName = "Top " + NoOfTopRows.ToString() + " ISIN_Number TextField,Cast(Ltrim(Rtrim(IsNull(ISIN_ID,'0'))) as Varchar(100))+'~'+Ltrim(Rtrim(IsNull(ISIN_Number,'0')))+'~'+Cast(Ltrim(Rtrim(IsNull(Isin_ProductID,'0'))) as Varchar(10))+'~'+Cast(Ltrim(Rtrim(IsNull(ISIN_ProductSeriesID,'0'))) as Varchar(10))+'~'+Cast(Ltrim(Rtrim(IsNull(ISIN_FaceValue,'0'))) as Varchar(10))+'~'+Cast(Ltrim(Rtrim(IsNull(ISIN_CurrencyID,'0'))) as Varchar(10))+'~'+Cast(Ltrim(Rtrim(IsNull(ISIN_IsActive,'0'))) as Varchar(10)) ValueField";
            string strQuery_OrderBy = "ISIN_Number";
            string strQuery_GroupBy = null;
            string strQuery_WhereClause = null;
            if (DTOrAQuery == "D")
            {
                //Remove Top 10 Not Required For DataTable
                strQuery_FieldName = strQuery_FieldName.Replace("Top " + NoOfTopRows.ToString(), "");
                strQuery_WhereClause = "ISIN_Number is not null";
                if (ProductID != 0)
                    strQuery_WhereClause = strQuery_WhereClause + " and IsIn_ProductID=" + ProductID.ToString().Trim();
                if (ProductSeriesID != 0)
                    strQuery_WhereClause = strQuery_WhereClause + " and IsIn_ProductSeriesID=" + ProductID.ToString().Trim();
                return GetDataTable(strQuery_Table, strQuery_FieldName, strQuery_WhereClause, strQuery_OrderBy);
            }
            else
            {
                strQuery_WhereClause = "ISIN_Number Like '%RequestLetter%'";
                if (ProductID != 0)
                    strQuery_WhereClause = strQuery_WhereClause + " and IsIn_ProductID=" + ProductID.ToString().Trim();
                if (ProductSeriesID != 0)
                    strQuery_WhereClause = strQuery_WhereClause + " and IsIn_ProductSeriesID=" + ProductID.ToString().Trim();
                AQuery = ReturnCombinedQuery(strQuery_Table, strQuery_FieldName, strQuery_WhereClause, strQuery_GroupBy, strQuery_OrderBy, "ISIN");
            }
            return null;
        }

        //IsActiveOrNotFalg : To Find ISIN Those Whether Is Active Or Not Active (Value "N","Y")
        public DataTable GetIsInNumber(string DTOrAQuery, ref string AQuery, int ProductID, int ProductSeriesID, int NoOfTopRows, char IsActiveOrNotFlag)
        {
            string strQuery_Table = "Master_ISIN";
            string strQuery_FieldName = "Top " + NoOfTopRows.ToString() + " ISIN_Number TextField,Cast(Ltrim(Rtrim(IsNull(ISIN_ID,'0'))) as Varchar(100))+'~'+Ltrim(Rtrim(IsNull(ISIN_Number,'0')))+'~'+Cast(Ltrim(Rtrim(IsNull(Isin_ProductID,'0'))) as Varchar(10))+'~'+Cast(Ltrim(Rtrim(IsNull(ISIN_ProductSeriesID,'0'))) as Varchar(10))+'~'+Cast(Ltrim(Rtrim(IsNull(ISIN_FaceValue,'0'))) as Varchar(10))+'~'+Cast(Ltrim(Rtrim(IsNull(ISIN_CurrencyID,'0'))) as Varchar(10))+'~'+Cast(Ltrim(Rtrim(IsNull(ISIN_IsActive,'0'))) as Varchar(10)) ValueField";
            string strQuery_OrderBy = "ISIN_Number";
            string strQuery_GroupBy = null;
            string strQuery_WhereClause = null;
            if (DTOrAQuery == "D")
            {
                //Remove Top 10 Not Required For DataTable
                strQuery_FieldName = strQuery_FieldName.Replace("Top " + NoOfTopRows.ToString(), "");
                strQuery_WhereClause = "ISIN_Number is not null and ISIN_IsActive='" + IsActiveOrNotFlag + "'";
                if (ProductID != 0)
                    strQuery_WhereClause = strQuery_WhereClause + " and IsIn_ProductID=" + ProductID.ToString().Trim();
                if (ProductSeriesID != 0)
                    strQuery_WhereClause = strQuery_WhereClause + " and IsIn_ProductSeriesID=" + ProductID.ToString().Trim();
                return GetDataTable(strQuery_Table, strQuery_FieldName, strQuery_WhereClause, strQuery_OrderBy);
            }
            else
            {
                strQuery_WhereClause = "ISIN_IsActive='" + IsActiveOrNotFlag + "' and ISIN_Number Like '%RequestLetter%'";
                if (ProductID != 0)
                    strQuery_WhereClause = strQuery_WhereClause + " and IsIn_ProductID=" + ProductID.ToString().Trim();
                if (ProductSeriesID != 0)
                    strQuery_WhereClause = strQuery_WhereClause + " and IsIn_ProductSeriesID=" + ProductID.ToString().Trim();
                AQuery = ReturnCombinedQuery(strQuery_Table, strQuery_FieldName, strQuery_WhereClause, strQuery_GroupBy, strQuery_OrderBy, "ISIN");
            }
            return null;
        }

        //DTOrAQuery    : DataTable Or Ajax Query
        //NoofTopRow    : No of Top Row Only in Case Of Ajax Query
        //Version       : We Can Have More Part Of It So Usefull For More Extention Now Veraion 1 Available  
        //CustomizeWhere: You Can Your Own Make Where Clause After Seeing Query Result By Sending Null Value Of Where;
        //                Example --> CompanyID='COR0000002' and SegID=24
        //For Version 1 : You Can Get Company,SegID,SegName,ExchangeSegmentID,CntInternalID,TCode,RegDate,STTPattern,AccountType,DpID,ClientID,
        //POAFlag,POAName
        public DataTable GetClient_FullDetail(string DTOrAQuery, ref string AQuery, int NoOfTopRows, string CustomizeWhere, string CustomizeSelection, int Version)
        {
            if (Version == 1)
            {
                strQuery_Table = @"(Select (Select LTRIM(Rtrim(isnull(cnt_firstName,'')))+LTRIM(Rtrim(isnull(cnt_middleName,'')))+LTRIM(Rtrim(isnull(cnt_lastName,'')))+'['+LTRIM(Rtrim(Cnt_UCC))+']['+LTRIM(Rtrim(TCode))+']' from tbl_master_contact Where cnt_internalId=CntInternalID) TextField,isnull(Company,'')+'~'+Cast(isnull(SegID,0) as Varchar(5))+'~'+isnull(SegName,'')+'~'+Cast(isnull(ExchangeSegmentID,'') as Varchar(5))+'~'+CntInternalID+'~'+TCode+'~'+Convert(Varchar,RegDate,106)+'~'+STTPattern+'~'+AccountType+'~'+isnull(DpID,'')+'~'+isnull(ClientID,'')+'~'+Cast(isnull(POAFlag,'0') as Varchar(1))+'~'+isnull(POAName,'')+isnull(GroupName,'')+'~'+isnull(Cast(GPMID as Varchar(20)),'')+'~'+isnull(Cast(GRPID as Varchar(20)),'')+'~'+isnull(GDescription,'')+'~'+isnull(GType,'')+'~'+isnull(GCode,'')+'~'+isnull(MemberType,'') ValueField,(Select Ltrim(Rtrim(branch_description))+'['+Ltrim(Rtrim(branch_code))+']' from tbl_master_branch Where branch_id=BranchID)BranchDescription,Company,SegID,SegName,ExchangeSegmentID,CntInternalID,BranchID,TCode,RegDate,STTPattern,AccountType,DpID,ClientID,POAFlag,POAName,GroupName,GPMID,GRPID,GDescription,GType,GCode,MemberType from (Select Company,SegID,SegName,ExchangeSegmentID,CntInternalID,TCode,RegDate,STTPattern,dpd_accountType AccountType,dpd_dpCode DpID,dpd_ClientId ClientID,dpd_POA POAFlag,Ltrim(Rtrim(dpd_POAName)) POAName,BranchID from (Select SegID,Company,SegName,ExchangeSegmentID,crg_CntID CntInternalID,(Select Cnt_BranchID From tbl_master_contact Where cnt_internalId=crg_CntID) BranchID,crg_tcode TCode,crg_regisDate RegDate,crg_STTPattern STTPattern  from (Select SegID,Exch_CompID as Company,(Select ExchangeSegment_ID From Master_ExchangeSegments Where ExchangeSegment_ExchangeID=Exchange_ID and ExchangeSegment_Code=Exch_SegmentID) as ExchangeSegmentID,Ltrim(Rtrim(Exh_ShortName))+' - '+Ltrim(Rtrim(Exch_SegmentID)) SegName from (Select Exch_CompID,Exch_InternalID SegID,Exh_ShortName,Exch_SegmentID from Tbl_Master_Exchange,Tbl_Master_CompanyExchange Where Exh_CntId=Exch_ExchID) Exchange,Master_Exchange Where Exchange_ShortName=Exh_ShortName) Exh_Exch,tbl_master_contactExchange Where Company=crg_company and crg_exchange=SegName) Exh_Exch_Crg Left Outer Join (Select * from tbl_master_contactDPDetails) Dpd On dpd_cntId=CntInternalID)T3 Left Outer Join  (Select Ltrim(Rtrim(gpm_Description))+'['+gpm_Code+']' GroupName,gpm_ID GPMID,grp_ID GRPID,LTRIM(RTRIM(gpm_Description)) GDescription,Ltrim(Rtrim(gpm_Type)) GType,LTRIM(RTRIM(gpm_Code)) GCode,Ltrim(Rtrim(gpm_MemberType)) MemberType, grp_contactId ContactID from tbl_master_groupMaster,tbl_trans_group Where gpm_id=grp_groupMaster) T4 On ContactID=CntInternalID)FullDetail";
                strQuery_FieldName = "Top " + NoOfTopRows.ToString() + CustomizeSelection;
                strQuery_OrderBy = "TextField";
                strQuery_GroupBy = null;
                strQuery_WhereClause = null;
                if (DTOrAQuery == "D")
                {
                    //Remove Top 10 Not Required For DataTable
                    strQuery_FieldName = strQuery_FieldName.Replace("Top " + NoOfTopRows.ToString(), "");
                    strQuery_WhereClause = CustomizeWhere;
                    return GetDataTable(strQuery_Table, strQuery_FieldName, strQuery_WhereClause, strQuery_OrderBy);
                }
                else
                {
                    strQuery_WhereClause = "TextField Like '%RequestLetter%' And (" + CustomizeWhere + ")";
                    AQuery = ReturnCombinedQuery(strQuery_Table, strQuery_FieldName, strQuery_WhereClause, strQuery_GroupBy, strQuery_OrderBy, "ClientFullDetail");
                }
            }
            return null;
        }


        #endregion

        #region Products Equity Commodity
        //ExchangeSegmentID: Specific ExchangeID Or Pass 0 For Current Segment
        //Only For Assets
        string GetProductType(int ExchangeSegmentID)
        {
            string ProductTypeID = null;
            if (ExchangeSegmentID == 0)
                ExchangeSegmentID = Convert.ToInt32(HttpContext.Current.Session["ExchangeSegmentID"].ToString());

            if (ExchangeSegmentID == 1 || ExchangeSegmentID == 4 || ExchangeSegmentID == 15 || ExchangeSegmentID == 19)//for CM
            {
                ProductTypeID = "1";
            }
            if (ExchangeSegmentID == 2 || ExchangeSegmentID == 5 || ExchangeSegmentID == 20)//for FO
            {
                ProductTypeID = "5";//Only Currency is Asset Here
            }
            if (ExchangeSegmentID == 3 || ExchangeSegmentID == 6 || ExchangeSegmentID == 8 || ExchangeSegmentID == 13)//for CDX
            {
                ProductTypeID = "8,21";
            }
            if (ExchangeSegmentID == 14)// For SPOT
            {
                ProductTypeID = "10";
            }
            if (ExchangeSegmentID == 7 || ExchangeSegmentID == 9 || ExchangeSegmentID == 10 || ExchangeSegmentID == 11
                || ExchangeSegmentID == 12 || ExchangeSegmentID == 17 || ExchangeSegmentID == 18 || ExchangeSegmentID == 21 || ExchangeSegmentID == 22 || ExchangeSegmentID == 23 || ExchangeSegmentID == 24 || ExchangeSegmentID == 25 || ExchangeSegmentID == 26 || ExchangeSegmentID == 27)//for COMM
            {
                if (ExchangeSegmentID == 10 || ExchangeSegmentID == 18 || ExchangeSegmentID == 21 || ExchangeSegmentID == 22 || ExchangeSegmentID == 23 || ExchangeSegmentID == 25 || ExchangeSegmentID == 26 || ExchangeSegmentID == 27)// INMX/DGCX/INSX/INFX/INBX/INAX/INEX
                {
                    ProductTypeID = "5,8,10,21";
                }
                else
                {
                    ProductTypeID = "10";
                }
            }
            return ProductTypeID;
        }
        //ExchangeSegmentID: Specific ExchangeID Or Pass 0 For Current Segment
        //AssetOrDerivativeIndicator : A (Asset)/ D(Derivative)/ AD(AssetDerivative)
        string GetProductType(int ExchangeSegmentID, string AssetOrDerivativeIndicator)
        {
            string ProductTypeID = String.Empty;
            if (AssetOrDerivativeIndicator == "A")
            {
                if (ExchangeSegmentID == 0)
                    ExchangeSegmentID = Convert.ToInt32(HttpContext.Current.Session["ExchangeSegmentID"].ToString());

                if (ExchangeSegmentID == 1 || ExchangeSegmentID == 4 || ExchangeSegmentID == 15 || ExchangeSegmentID == 19)//for CM
                {
                    ProductTypeID = "1";
                }
                if (ExchangeSegmentID == 2 || ExchangeSegmentID == 5 || ExchangeSegmentID == 20)//for FO
                {
                    ProductTypeID = "5";
                }
                if (ExchangeSegmentID == 3 || ExchangeSegmentID == 6 || ExchangeSegmentID == 8 || ExchangeSegmentID == 13)//for CDX
                {
                    ProductTypeID = "8,21";
                }
                if (ExchangeSegmentID == 14)// For SPOT
                {
                    ProductTypeID = "10";
                }
                if (ExchangeSegmentID == 7 || ExchangeSegmentID == 9 || ExchangeSegmentID == 10 || ExchangeSegmentID == 11
                    || ExchangeSegmentID == 12 || ExchangeSegmentID == 17 || ExchangeSegmentID == 18 || ExchangeSegmentID == 21 || ExchangeSegmentID == 22 || ExchangeSegmentID == 23 || ExchangeSegmentID == 24 || ExchangeSegmentID == 25 || ExchangeSegmentID == 26 || ExchangeSegmentID == 27)//for COMM
                {
                    if (ExchangeSegmentID == 10 || ExchangeSegmentID == 18 || ExchangeSegmentID == 21 || ExchangeSegmentID == 22 || ExchangeSegmentID == 23 || ExchangeSegmentID == 25 || ExchangeSegmentID == 26 || ExchangeSegmentID == 27)// INMX/DGCX/INSX/INFX/INBX/INAX/INEX
                    {
                        ProductTypeID = "5,8,10,21";
                    }
                    else
                    {
                        ProductTypeID = "10";
                    }
                }
            }
            else if (AssetOrDerivativeIndicator == "D")
            {
                if (ExchangeSegmentID == 0)
                    ExchangeSegmentID = Convert.ToInt32(HttpContext.Current.Session["ExchangeSegmentID"].ToString());

                if (ExchangeSegmentID == 1 || ExchangeSegmentID == 4 || ExchangeSegmentID == 15 || ExchangeSegmentID == 19)//for CM
                {
                    ProductTypeID = "1";
                }
                if (ExchangeSegmentID == 2 || ExchangeSegmentID == 5 || ExchangeSegmentID == 20)//for FO
                {
                    ProductTypeID = "4,6";
                }
                if (ExchangeSegmentID == 3 || ExchangeSegmentID == 6 || ExchangeSegmentID == 8 || ExchangeSegmentID == 13)//for CDX
                {
                    ProductTypeID = "9";
                }
                if (ExchangeSegmentID == 14)// For SPOT
                {
                    ProductTypeID = "11";
                }
                if (ExchangeSegmentID == 7 || ExchangeSegmentID == 9 || ExchangeSegmentID == 10 || ExchangeSegmentID == 11
                    || ExchangeSegmentID == 12 || ExchangeSegmentID == 17 || ExchangeSegmentID == 18 || ExchangeSegmentID == 21 || ExchangeSegmentID == 22 || ExchangeSegmentID == 23 || ExchangeSegmentID == 24 || ExchangeSegmentID == 25 || ExchangeSegmentID == 26 || ExchangeSegmentID == 27)//for COMM
                {
                    if (ExchangeSegmentID == 10 || ExchangeSegmentID == 18 || ExchangeSegmentID == 21 || ExchangeSegmentID == 22 || ExchangeSegmentID == 23 || ExchangeSegmentID == 25 || ExchangeSegmentID == 26 || ExchangeSegmentID == 27)// INMX/DGCX/INSX/INFX/INBX/INAX/INEX
                    {
                        ProductTypeID = "6,9,11";
                    }
                    else
                    {
                        ProductTypeID = "11";
                    }
                }
            }
            else if (AssetOrDerivativeIndicator == "AD")
            {
                if (ExchangeSegmentID == 0)
                    ExchangeSegmentID = Convert.ToInt32(HttpContext.Current.Session["ExchangeSegmentID"].ToString());

                if (ExchangeSegmentID == 1 || ExchangeSegmentID == 4 || ExchangeSegmentID == 15 || ExchangeSegmentID == 19)//for CM
                {
                    ProductTypeID = "1";
                }
                if (ExchangeSegmentID == 2 || ExchangeSegmentID == 5 || ExchangeSegmentID == 20)//for FO
                {
                    ProductTypeID = "1,4,5,6";
                }
                if (ExchangeSegmentID == 3 || ExchangeSegmentID == 6 || ExchangeSegmentID == 8 || ExchangeSegmentID == 13)//for CDX
                {
                    ProductTypeID = "8,9,21,13";
                }
                if (ExchangeSegmentID == 14)// For SPOT
                {
                    ProductTypeID = "10,11";
                }
                if (ExchangeSegmentID == 7 || ExchangeSegmentID == 9 || ExchangeSegmentID == 10 || ExchangeSegmentID == 11
                    || ExchangeSegmentID == 12 || ExchangeSegmentID == 17 || ExchangeSegmentID == 18 || ExchangeSegmentID == 21 || ExchangeSegmentID == 22 || ExchangeSegmentID == 23 || ExchangeSegmentID == 24 || ExchangeSegmentID == 25 || ExchangeSegmentID == 26 || ExchangeSegmentID == 27)//for COMM
                {
                    if (ExchangeSegmentID == 10 || ExchangeSegmentID == 18 || ExchangeSegmentID == 21 || ExchangeSegmentID == 22 || ExchangeSegmentID == 23 || ExchangeSegmentID == 25 || ExchangeSegmentID == 26 || ExchangeSegmentID == 27)// INMX/DGCX/INSX/INFX/INBX/INAX/INEX
                    {
                        ProductTypeID = "5,6,8,9,10,11,21,13";
                    }
                    else
                    {
                        ProductTypeID = "10,11";
                    }
                }
            }

            return ProductTypeID;
        }

        ///Note For All Product Or ProductSeriesID : Pass 0
        ///For Current ExchangeID : Pass 0
        public DataTable GetProducts_Equity_InnerJoinByProductID(string DTOrAQuery, ref string AQuery, int ProductID, int ProductSeriesID, int ProductType, int ExchangeSegmentID)
        {
            string ProductTypeFetch = GetProductType(ExchangeSegmentID);
            string strQuery_Table = "Master_Products,Master_Equity";
            string strQuery_FieldName = @"Top 10 Ltrim(Rtrim(ISNULL(Products_Name,'')))+' [' +Ltrim(Rtrim(ISNULL(Equity_TickerSymbol,'')))+'] ['+ Ltrim(Rtrim(ISNULL(Equity_TickerCode,'')))+']' TextField,Cast(Products_ID as Varchar(10))+'~'+Cast(Equity_SeriesID as Varchar(10)) +'~'+Ltrim(Rtrim(isnull(Equity_Series,''))) ValueField,Products_Name,Equity_TickerSymbol,Equity_TickerCode,Equity_Series";
            string strQuery_OrderBy = "Products_Name,Equity_TickerSymbol,Equity_TickerCode";
            string strQuery_GroupBy = null;
            string strQuery_WhereClause = null;
            if (DTOrAQuery == "D")
            {
                //Remove Top 10 Not Required For DataTable
                strQuery_FieldName = strQuery_FieldName.Replace("Top 10", "");

                strQuery_WhereClause = "Equity_ProductID=Products_ID";
                if (ProductID != 0)
                    strQuery_WhereClause = strQuery_WhereClause + " And Equity_ProductID=" + ProductID.ToString().Trim();

                if (ProductSeriesID != 0)
                    strQuery_WhereClause = strQuery_WhereClause + " And Equity_ProductSeriesID=" + ProductSeriesID.ToString().Trim();

                if (ProductType != 0)
                    strQuery_WhereClause = strQuery_WhereClause + " And Products_ProductTypeID in (" + ProductType.ToString().Trim() + ")";
                else
                    strQuery_WhereClause = strQuery_WhereClause + " And Products_ProductTypeID in(" + ProductTypeFetch.Trim() + ")";

                return GetDataTable(strQuery_Table, strQuery_FieldName, strQuery_WhereClause, strQuery_OrderBy);
            }
            else
            {
                strQuery_WhereClause = "(Ltrim(Rtrim(ISNULL(Products_Name,''))) Like '%RequestLetter%' Or Ltrim(Rtrim(ISNULL(Equity_TickerSymbol,''))) Like '%RequestLetter%' Or Ltrim(Rtrim(ISNULL(Equity_TickerCode,''))) Like '%RequestLetter%') and Equity_ProductID=Products_ID";
                if (ProductID != 0)
                    strQuery_WhereClause = strQuery_WhereClause + " And Equity_ProductID=" + ProductID.ToString().Trim();
                if (ProductSeriesID != 0)
                    strQuery_WhereClause = strQuery_WhereClause + " And Equity_ProductSeriesID=" + ProductID.ToString().Trim();
                if (ProductType != 0)
                    strQuery_WhereClause = strQuery_WhereClause + " And Products_ProductTypeID in (" + ProductType.ToString().Trim() + ")";
                else
                    strQuery_WhereClause = strQuery_WhereClause + " And Products_ProductTypeID in (" + ProductTypeFetch.Trim() + ")";

                AQuery = ReturnCombinedQuery(strQuery_Table, strQuery_FieldName, strQuery_WhereClause, strQuery_GroupBy, strQuery_OrderBy, "ProductInnerEquity");
            }
            return null;
        }
        public DataTable GetProducts_Equity_InnerJoinByProductID(string DTOrAQuery, ref string AQuery, int ProductID, int ProductSeriesID, int ProductType, int ExchangeSegmentID, string AssetOrDerivativeOrAssetDerivative)
        {
            string ProductTypeFetch = GetProductType(ExchangeSegmentID, AssetOrDerivativeOrAssetDerivative);
            string strQuery_Table = "Master_Products,Master_Equity";
            string strQuery_FieldName = @"Top 10 Ltrim(Rtrim(ISNULL(Products_Name,'')))+' [' +Ltrim(Rtrim(ISNULL(Equity_TickerSymbol,'')))+'] ['+ Ltrim(Rtrim(ISNULL(Equity_TickerCode,'')))+']' TextField,Cast(Products_ID as Varchar(10))+'~'+Cast(Equity_SeriesID as Varchar(10)) +'~'+Ltrim(Rtrim(isnull(Equity_Series,''))) ValueField,Products_Name,Equity_TickerSymbol,Equity_TickerCode,Equity_Series";
            string strQuery_OrderBy = "Products_Name,Equity_TickerSymbol,Equity_TickerCode";
            string strQuery_GroupBy = null;
            string strQuery_WhereClause = null;
            if (DTOrAQuery == "D")
            {
                //Remove Top 10 Not Required For DataTable
                strQuery_FieldName = strQuery_FieldName.Replace("Top 10", "");

                strQuery_WhereClause = "Equity_ProductID=Products_ID";
                if (ProductID != 0)
                    strQuery_WhereClause = strQuery_WhereClause + " And Equity_ProductID=" + ProductID.ToString().Trim();

                if (ProductSeriesID != 0)
                    strQuery_WhereClause = strQuery_WhereClause + " And Equity_ProductSeriesID=" + ProductSeriesID.ToString().Trim();

                if (ProductType != 0)
                    strQuery_WhereClause = strQuery_WhereClause + " And Products_ProductTypeID in (" + ProductType.ToString().Trim() + ")";
                else
                    strQuery_WhereClause = strQuery_WhereClause + " And Products_ProductTypeID in(" + ProductTypeFetch.Trim() + ")";

                return GetDataTable(strQuery_Table, strQuery_FieldName, strQuery_WhereClause, strQuery_OrderBy);
            }
            else
            {
                strQuery_WhereClause = "(Ltrim(Rtrim(ISNULL(Products_Name,''))) Like '%RequestLetter%' Or Ltrim(Rtrim(ISNULL(Equity_TickerSymbol,''))) Like '%RequestLetter%' Or Ltrim(Rtrim(ISNULL(Equity_TickerCode,''))) Like '%RequestLetter%') and Equity_ProductID=Products_ID";
                if (ProductID != 0)
                    strQuery_WhereClause = strQuery_WhereClause + " And Equity_ProductID=" + ProductID.ToString().Trim();
                if (ProductSeriesID != 0)
                    strQuery_WhereClause = strQuery_WhereClause + " And Equity_ProductSeriesID=" + ProductID.ToString().Trim();
                if (ProductType != 0)
                    strQuery_WhereClause = strQuery_WhereClause + " And Products_ProductTypeID in (" + ProductType.ToString().Trim() + ")";
                else
                    strQuery_WhereClause = strQuery_WhereClause + " And Products_ProductTypeID in (" + ProductTypeFetch.Trim() + ")";

                AQuery = ReturnCombinedQuery(strQuery_Table, strQuery_FieldName, strQuery_WhereClause, strQuery_GroupBy, strQuery_OrderBy, "ProductInnerEquity");
            }
            return null;
        }
        #endregion

        #region Company
        //if CompanyID = "NA" Then All Or Pass CompID Like 'COR000002' For Specific Company
        public DataTable GetCompany(string DTOrAQuery, ref string AQuery, int NoOfTopRows)
        {
            string strQuery_Table = "tbl_master_Company";
            string strQuery_FieldName = "Top " + NoOfTopRows.ToString() + " Ltrim(Rtrim(Cmp_Name)) TextField,Cast(Cmp_ID as Varchar(100))+'~'+LTRIM(Rtrim(isnull(Cmp_internalID,'')))+'~'+LTRIM(Rtrim(isnull(cmp_parentid,'')))+'~'+LTRIM(Rtrim(isnull(cmp_currencyid,'')))+'~'+LTRIM(Rtrim(isnull(Com_Add,''))) ValueField,cmp_id,cmp_Name,cmp_internalid,cmp_parentid,cmp_currencyid";
            string strQuery_OrderBy = "Cmp_Name";
            string strQuery_GroupBy = null;
            string strQuery_WhereClause = null;
            if (DTOrAQuery == "D")
            {
                //Remove Top 10 Not Required For DataTable
                strQuery_FieldName = strQuery_FieldName.Replace("Top " + NoOfTopRows.ToString(), "");
                strQuery_WhereClause = null;
                return GetDataTable(strQuery_Table, strQuery_FieldName, strQuery_WhereClause, strQuery_OrderBy);
            }
            else
            {
                strQuery_WhereClause = "cmp_Name Like '%RequestLetter%'";
                AQuery = ReturnCombinedQuery(strQuery_Table, strQuery_FieldName, strQuery_WhereClause, strQuery_GroupBy, strQuery_OrderBy, "Company");
            }
            return null;
        }
        #endregion

        #region Employee
        //if EmployeeType = "NA" Then All Or Pass EmpTypeID Like '1' For Specific Employee Type
        public DataTable GetEmployeeType(string DTOrAQuery, ref string AQuery, int NoOfTopRows)
        {
            string strQuery_Table = "tbl_master_employeeType";
            string strQuery_FieldName = "Top " + NoOfTopRows.ToString() + " Ltrim(Rtrim(emptpy_type))+' ['+ LTRIM(Rtrim(isnull(emptpy_code,'')))+']' TextField,Cast(emptpy_id as Varchar(100))+'~'+LTRIM(Rtrim(isnull(emptpy_code,''))) ValueField,emptpy_id,emptpy_type,emptpy_code";
            string strQuery_OrderBy = "emptpy_type,emptpy_code";
            string strQuery_GroupBy = null;
            string strQuery_WhereClause = null;
            if (DTOrAQuery == "D")
            {
                //Remove Top 10 Not Required For DataTable
                strQuery_FieldName = strQuery_FieldName.Replace("Top " + NoOfTopRows.ToString(), "");
                strQuery_WhereClause = null;
                return GetDataTable(strQuery_Table, strQuery_FieldName, strQuery_WhereClause, strQuery_OrderBy);
            }
            else
            {
                strQuery_WhereClause = "emptpy_code Like '%RequestLetter%' or emptpy_type Like '%RequestLetter%'";
                AQuery = ReturnCombinedQuery(strQuery_Table, strQuery_FieldName, strQuery_WhereClause, strQuery_GroupBy, strQuery_OrderBy, "EmployeeType");
            }
            return null;
        }
        public DataTable GetUserForSendMail(string DTOrAQuery, ref string AQuery, int NoOfTopRows)
        {
            string strQuery_Table = "tbl_master_email,tbl_master_contact";
            string strQuery_FieldName = "Top " + NoOfTopRows.ToString() + " isnull(Ltrim(rtrim(cnt_firstName)),'')+' '+isnull(Ltrim(rtrim(cnt_middlename)),'')+' '+isnull(Ltrim(rtrim(cnt_lastName)),'') +'['+Ltrim(Rtrim(cnt_UCC))+'] '+ '<'+ eml_email + '>' TextField,isnull(Ltrim(rtrim(cnt_firstName)),'')+'~'+isnull(Ltrim(rtrim(cnt_middlename)),'')+'~'+isnull(Ltrim(rtrim(cnt_lastName)),'')+'~'+Ltrim(Rtrim(cnt_UCC))+'~'+isnull(Ltrim(rtrim(eml_email)),'')+'~'+cnt_internalId ValueField,cnt_firstName, cnt_middlename,cnt_lastName,cnt_UCC,eml_email,cnt_internalId";
            string strQuery_OrderBy = "TextField";
            string strQuery_GroupBy = null;
            string strQuery_WhereClause = "eml_cntId = cnt_internalId And (eml_email <> '') AND Left(cnt_internalId,2) ='EM'";
            if (DTOrAQuery == "D")
            {
                //Remove Top 10 Not Required For DataTable
                strQuery_FieldName = strQuery_FieldName.Replace("Top " + NoOfTopRows.ToString(), "");
                return GetDataTable(strQuery_Table, strQuery_FieldName, strQuery_WhereClause, strQuery_OrderBy);
            }
            else
            {
                strQuery_WhereClause += " And (cnt_firstName Like '%RequestLetter%' or cnt_ucc Like '%RequestLetter%' or eml_email  Like '%RequestLetter%')";
                AQuery = ReturnCombinedQuery(strQuery_Table, strQuery_FieldName, strQuery_WhereClause, strQuery_GroupBy, strQuery_OrderBy, "UserByEmail");
            }
            return null;
        }
        public DataTable GetDigitalSignature(string DTOrAQuery, ref string AQuery, int AuthorizeUser, int NoOfTopRows)
        {
            string strQuery_Table = "Master_DigitalSignature,tbl_master_contact";
            string strQuery_FieldName = "Top " + NoOfTopRows.ToString() + " (cnt_firstName +' '+cnt_middleName+' '+cnt_lastName +'['+cnt_shortName+']') as TextField,Cast(DigitalSignature_ID as Varchar) ValueField";
            string strQuery_OrderBy = "TextField";
            string strQuery_GroupBy = null;
            string strQuery_WhereClause = null;
            if (DTOrAQuery == "D")
            {
                strQuery_WhereClause = @"cnt_internalid=DigitalSignature_ContactID and '" + AuthorizeUser + @"' 
            in ( select * from dbo.fnSplitReturnTable(DigitalSignature_AuthorizedUsers,','))  and 
            DigitalSignature_ValidUntil>=cast(convert(varchar(9),GetDate(),06) as DateTime)";
                //Remove Top 10 Not Required For DataTable
                strQuery_FieldName = strQuery_FieldName.Replace("Top " + NoOfTopRows.ToString(), "");
                return GetDataTable(strQuery_Table, strQuery_FieldName, strQuery_WhereClause, strQuery_OrderBy);
            }
            else
            {
                strQuery_WhereClause = @"cnt_firstName Like 'RequestLetter%' and cnt_internalid=DigitalSignature_ContactID and '" + AuthorizeUser + @"'in ( select * from dbo.fnSplitReturnTable(DigitalSignature_AuthorizedUsers,','))  and DigitalSignature_ValidUntil>=cast(convert(varchar(9),GetDate(),06) as DateTime)";
                AQuery = ReturnCombinedQuery(strQuery_Table, strQuery_FieldName, strQuery_WhereClause, strQuery_GroupBy, strQuery_OrderBy, "DigitalSignature");
            }
            return null;
        }
        public string GetDigitalSignature()
        {

            string strQuery_Table = @"Master_DigitalSignature,tbl_master_contact";
            string strQuery_FieldName = "Top 10 Ltrim(Rtrim(IsNull(cnt_firstName,''))) +' '+Ltrim(Rtrim(IsNull(cnt_middleName,'')))+' '+Ltrim(Rtrim(IsNull(cnt_lastName,''))) +'['+Ltrim(Rtrim(IsNull(cnt_shortName,'')))+']' TextField,Cast(Ltrim(Rtrim(DigitalSignature_ID)) as Varchar(10))+'~'+Ltrim(Rtrim(IsNull(cnt_firstName,''))) +' '+Ltrim(Rtrim(IsNull(cnt_middleName,'')))+' '+Ltrim(Rtrim(IsNull(cnt_lastName,''))) +'['+Ltrim(Rtrim(IsNull(cnt_shortName,'')))+']'+'~'+isnull(digitalsignature_Type,'N')+'~'+isnull(digitalSignature_Name,'NA')+'~DigiSign'+ ValueField";
            string strQuery_WhereClause = @"cnt_firstName Like '%RequestLetter%' and cnt_internalid=DigitalSignature_ContactID and '" + HttpContext.Current.Session["UserID"] + @"' in ( select * from dbo.fnSplitReturnTable(DigitalSignature_AuthorizedUsers,','))  and DigitalSignature_ValidUntil>=cast(convert(varchar(9),getdate(),06) as datetime)";
            string strQuery_OrderBy = "TextField";
            string strQuery_GroupBy = null;
            return ReturnCombinedQuery(strQuery_Table, strQuery_FieldName, strQuery_WhereClause, strQuery_GroupBy, strQuery_OrderBy, "EDUCATION");
        }

        public string GetEmployeeWithSignature()
        {
            //strQuery_Table = "tbl_master_contact contact,tbl_master_employee e,tbl_master_document";
            //strQuery_FieldName = " top 10 (contact.cnt_firstName +' '+contact.cnt_middleName+' '+contact.cnt_lastName +'['+contact.cnt_shortName+']') as Name ,e.emp_contactid";
            //strQuery_WhereClause = " contact.cnt_firstName Like (\'%RequestLetter%') and e.emp_contactId=contact.cnt_internalid and doc_contactId=e.emp_contactid and doc_documentTypeId=(select top 1 dty_id from tbl_master_documentType where dty_documentType='Signature' and dty_applicableFor='Employee') ";

            string strQuery_Table = @"tbl_master_contact contact,tbl_master_employee e,tbl_master_document";
            string strQuery_FieldName = " Top 10 Ltrim(Rtrim(IsNull(contact.cnt_firstName,''))) +' '+Ltrim(Rtrim(IsNull(contact.cnt_middleName,'')))+' '+Ltrim(Rtrim(IsNull(contact.cnt_lastName,''))) +'['+Ltrim(Rtrim(IsNull(contact.cnt_shortName,'')))+']' TextField,Cast(Ltrim(Rtrim(e.emp_contactid)) as Varchar(10))+'~'+Ltrim(Rtrim(IsNull(contact.cnt_firstName,''))) +' '+Ltrim(Rtrim(IsNull(contact.cnt_middleName,'')))+' '+Ltrim(Rtrim(IsNull(contact.cnt_lastName,''))) +'['+Ltrim(Rtrim(IsNull(contact.cnt_shortName,'')))+']' ValueField";
            string strQuery_WhereClause = @" contact.cnt_firstName Like '%RequestLetter%'  and e.emp_contactId=contact.cnt_internalid and doc_contactId=e.emp_contactid and doc_documentTypeId=(select top 1 dty_id from tbl_master_documentType where dty_documentType='Signature' and dty_applicableFor='Employee')";
            string strQuery_OrderBy = "TextField";
            string strQuery_GroupBy = null;
            return ReturnCombinedQuery(strQuery_Table, strQuery_FieldName, strQuery_WhereClause, strQuery_GroupBy, strQuery_OrderBy, "EDUCATION");
        }

        #endregion

        #region ClientCategory
        public DataTable GetAllClientCategory(string DTOrAQuery, ref string AQuery, int NoOfTopRows, string Prefix, string status, string type)
        {
            string contactCategory = string.Empty;
            if (Prefix == "CL") contactCategory = "ClientCat";
            else if (Prefix == "BO") contactCategory = "BrokerCat";
            else if (Prefix == "EM") contactCategory = "EmployeeCat";
            else if (Prefix == "RT")
            {
                contactCategory = "ReportToCat";
                Prefix = "EM";
            }
            string strQuery_Table = " tbl_master_legalStatus,tbl_master_contact ";
            string strQuery_FieldName = " distinct Top " + NoOfTopRows.ToString() + " isnull(Ltrim(Rtrim(lgl_legalStatus)),'')+'['+Ltrim(Rtrim(lgl_type))+']' TextField,isnull(Ltrim(Rtrim(lgl_id)),'')+'~'+isnull(Ltrim(Rtrim(lgl_legalStatus)),'')+'~'+isnull(Ltrim(Rtrim(lgl_type)),'') ValueField,lgl_id ID,lgl_legalStatus,lgl_type ";
            string strQuery_OrderBy = " TextField ";
            string strQuery_GroupBy = null;
            string strQuery_WhereClause = " lgl_id in (Select cnt_legalStatus From tbl_master_contact Where cnt_internalId like '" + Prefix + "%') ";
            if (status != null)
                strQuery_WhereClause += " and lgl_legalStatus='" + status + "' ";
            if (type != null)
                strQuery_WhereClause += " and lgl_type='" + type + "'";
            if (DTOrAQuery == "D")
            {
                //Remove Top 10 Not Required For DataTable
                strQuery_FieldName = strQuery_FieldName.Replace(" distinct Top " + NoOfTopRows.ToString() + " ", " distinct ");
                return GetDataTable(strQuery_Table, strQuery_FieldName, strQuery_WhereClause, strQuery_OrderBy);
            }
            else
            {
                strQuery_WhereClause += " and lgl_legalStatus like '%RequestLetter%' ";
                AQuery = ReturnCombinedQuery(strQuery_Table, strQuery_FieldName, strQuery_WhereClause, strQuery_GroupBy, strQuery_OrderBy, contactCategory);
            }
            return null;
        }
        #endregion

        #region Client/Employee/Broker/ReportTo
        public DataTable GetClientInfo(string InternalID, int NoOfTopRows)
        {
            string strQuery_Table = "(Select cnt_InternalID,cnt_branchid,cnt_UCC,cnt_firstName,cnt_middleName,cnt_lastName,cnt_maritalStatus from tbl_master_contact Where cnt_internalId='" + InternalID + "') as T1 Left Outer Join tbl_master_contactExchange on cnt_internalId=crg_cntID Left Outer Join tbl_master_contactDPDetails on crg_cntID=dpd_cntId";
            string strQuery_FieldName = " Top " + NoOfTopRows.ToString() + " T1.*,crg_company,crg_exchange,crg_tcode,crg_STTPattern,dpd_accountType,dpd_dpCode,dpd_ClientId,dpd_CMBPID,dpd_AccountName,dpd_ID";
            string strQuery_OrderBy = null;
            string strQuery_GroupBy = null;
            string strQuery_WhereClause = null;
            return GetDataTable(strQuery_Table, strQuery_FieldName, strQuery_WhereClause, strQuery_OrderBy);
        }


        //DTOrAQuery --->'D','A'
        public DataTable GetContact(string DTOrAQuery, ref string AQuery, int NoOfTopRows, string Prefix)
        {
            string contactCategory = string.Empty;
            if (Prefix == "CL") contactCategory = "Client";
            else if (Prefix == "BO") contactCategory = "Broker";
            else if (Prefix == "EM") contactCategory = "Employee";
            else if (Prefix == "RT")
            {
                contactCategory = "ReportTo";
                Prefix = "EM";
            }
            string strQuery_Table = " (select isnull(Ltrim(Rtrim(cnt_firstName)),'')+' '+isnull(Ltrim(Rtrim(cnt_middleName)),'') +' '+isnull(Ltrim(Rtrim(cnt_lastName)),'')+'['+Ltrim(Rtrim(cnt_UCC))+']' TextField,isnull(Ltrim(Rtrim(cnt_firstName)),'')+'~'+isnull(Ltrim(Rtrim(cnt_middleName)),'')+'~'+isnull(Ltrim(Rtrim(cnt_lastName)),'')+'~'+Ltrim(Rtrim(cnt_UCC))+'~'+LTRIM(Rtrim(cnt_internalId)) ValueField,cnt_id ID,cnt_firstName,cnt_middleName,cnt_lastName,cnt_UCC from tbl_master_contact Where Left(cnt_internalId,2)='" + Prefix + "') AllClient ";
            string strQuery_FieldName = " * From (Select Top " + NoOfTopRows.ToString() + "  * ";
            string strQuery_OrderBy = " TextField ";
            string strQuery_GroupBy = null;
            string strQuery_WhereClause = null;
            if (DTOrAQuery == "D")
            {
                //Remove Top 10 Not Required For DataTable
                strQuery_FieldName = strQuery_FieldName.Replace(" * From (Select Top " + NoOfTopRows.ToString() + "  * ", " * From (Select * ");
                strQuery_WhereClause = null;
                return GetDataTable(strQuery_Table, strQuery_FieldName, strQuery_WhereClause, strQuery_OrderBy);
            }
            else
            {
                strQuery_WhereClause = " cnt_firstName like '%RequestLetter%' Or cnt_middleName like '%RequestLetter%' Or cnt_lastName like '%RequestLetter%' or cnt_UCC like '%RequestLetter%' Or TextField like '%RequestLetter%') AllClientForOrderBy ";
                AQuery = ReturnCombinedQuery(strQuery_Table, strQuery_FieldName, strQuery_WhereClause, strQuery_GroupBy, strQuery_OrderBy, contactCategory);
            }
            return null;
        }

        public DataTable GetContact(string DTOrAQuery, ref string AQuery, int NoOfTopRows, string Prefix, string IgnoreInternalID)
        {
            string contactCategory = string.Empty;
            if (Prefix == "CL") contactCategory = "Client";
            else if (Prefix == "BO") contactCategory = "Broker";
            else if (Prefix == "EM") contactCategory = "Employee";
            else if (Prefix == "RT")
            {
                contactCategory = "ReportTo";
                Prefix = "EM";
            }
            string strQuery_Table = " (select isnull(Ltrim(Rtrim(cnt_firstName)),'')+' '+isnull(Ltrim(Rtrim(cnt_middleName)),'') +' '+isnull(Ltrim(Rtrim(cnt_lastName)),'')+'['+Ltrim(Rtrim(cnt_UCC))+']' TextField,isnull(Ltrim(Rtrim(cnt_firstName)),'')+'~'+isnull(Ltrim(Rtrim(cnt_middleName)),'')+'~'+isnull(Ltrim(Rtrim(cnt_lastName)),'')+'~'+Ltrim(Rtrim(cnt_UCC))+'~'+LTRIM(Rtrim(cnt_internalId)) ValueField,cnt_id ID,cnt_firstName,cnt_middleName,cnt_lastName,cnt_UCC from tbl_master_contact Where Left(cnt_internalId,2)='" + Prefix + "') AllClient ";
            string strQuery_FieldName = " * From (Select Top " + NoOfTopRows.ToString() + "  * ";
            string strQuery_OrderBy = " TextField ";
            string strQuery_GroupBy = null;
            string strQuery_WhereClause = null;
            if (DTOrAQuery == "D")
            {
                //Remove Top 10 Not Required For DataTable
                strQuery_FieldName = strQuery_FieldName.Replace(" * From (Select Top " + NoOfTopRows.ToString() + "  * ", " * From (Select * ");
                strQuery_WhereClause = "Cnt_InternalID not in (" + IgnoreInternalID + ")";
                return GetDataTable(strQuery_Table, strQuery_FieldName, strQuery_WhereClause, strQuery_OrderBy);
            }
            else
            {
                strQuery_WhereClause = " cnt_firstName like '%RequestLetter%' Or cnt_middleName like '%RequestLetter%' Or cnt_lastName like '%RequestLetter%' or cnt_UCC like '%RequestLetter%' Or TextField like '%RequestLetter%') and Cnt_InternalID not in (" + IgnoreInternalID + ") AllClientForOrderBy ";
                AQuery = ReturnCombinedQuery(strQuery_Table, strQuery_FieldName, strQuery_WhereClause, strQuery_GroupBy, strQuery_OrderBy, contactCategory);
            }
            return null;
        }



        #endregion

        #region Segment
        //////Get Detail Of Specific Company Specific UserSegName Like (CM or F0 Etc..)
        public DataTable GetSegments(string DTOrAQuery, ref string AQuery, int NoOfTopRows, string CompanyID, string UserSegName)
        {
            string strQuery_Table = "";
            string strQuery_FieldName = "";
            string strQuery_OrderBy = " TextField";
            string strQuery_GroupBy = null;
            string strQuery_WhereClause = null;

            strQuery_FieldName = "Top " + NoOfTopRows.ToString() + " ExchSegmentName TextField,cast(Session_UserSegID as varchar)+'~'+LTRIM(RTRIM(exh_shortName))+'~'+LTRIM(rtrim(exch_segmentId))+'~'+LTRIM(rtrim(Session_ExchangeSegmentID))+'~'+LTRIM(rtrim(Company)) ValueField,ExchSegmentName,Session_UserSegID,exh_shortName,exch_segmentId,Session_ExchangeSegmentID,Company";

            strQuery_Table = " (Select (Select Ltrim(RTrim(Cmp_Name))+' ['+Cmp_InternalID+']' as Company from Tbl_Master_Company Where Cmp_InternalID=Exch_CompID) as Company,(Select ExchangeSegment_ID From Master_ExchangeSegments Where ExchangeSegment_ExchangeID=Exchange_ID and ExchangeSegment_Code=Exch_SegmentID) as [Session_ExchangeSegmentID],Exch_InternalID as [Session_UserSegID],Exh_ShortName,Exch_SegmentID,LTRIM(RTRIM(Exh_ShortName))+' - '+ LTRIM(RTRIM(Exch_SegmentID)) as ExchSegmentName from (Select Exch_CompID,Exch_InternalID,Exh_ShortName,Exch_SegmentID from Tbl_Master_Exchange,Tbl_Master_CompanyExchange Where Exh_CntId=Exch_ExchID and Exh_ShortName!='Accounts') as T1,Master_Exchange Where Exchange_ShortName=Exh_ShortName";
            if (UserSegName != null) strQuery_Table = strQuery_Table + " and Exch_SegmentID in (select col from fnSplitReturnTable('" + UserSegName + "',','))";
            if (CompanyID != null) strQuery_Table = strQuery_Table + " and Exch_CompID='" + CompanyID + "'";
            strQuery_Table = strQuery_Table + " )R1";
            if (DTOrAQuery == "D")
            {
                //Remove Top 10 Not Required For DataTable
                strQuery_FieldName = strQuery_FieldName.Replace("Top " + NoOfTopRows.ToString(), "");
                strQuery_WhereClause = null;
                return GetDataTable(strQuery_Table, strQuery_FieldName, strQuery_WhereClause, strQuery_OrderBy);
            }
            else
            {
                strQuery_WhereClause = @" exh_shortName like '%RequestLetter%' OR exch_segmentId like '%RequestLetter%' OR ExchSegmentName like '%RequestLetter%'";
                AQuery = ReturnCombinedQuery(strQuery_Table, strQuery_FieldName, strQuery_WhereClause, strQuery_GroupBy, strQuery_OrderBy, "CompanySegment");
            }
            return null;
        }
        #endregion

        #region JournalVoucher
        public DataTable GetSystemJvs(string DTOrAQuery, ref string AQuery, int NoOfTopRows)
        {
            string strQuery_Table = "master_vouchertype";
            string strQuery_FieldName = "top 10 VoucherType_Name+' [ '+rtrim(ltrim(VoucherType_Code))+' ]' as VouchaerName,VoucherType_Code as voucherCode";
            string strQuery_WhereClause = " (VoucherType_Name like ('%RequestLetter%') or VoucherType_Code like ('%RequestLetter%')) ";
            string strQuery_OrderBy = "VoucherType_Name,VoucherType_Code";
            string strQuery_GroupBy = null;
            if (DTOrAQuery == "D")
            {
                //Remove Top 10 Not Required For DataTable
                strQuery_FieldName = strQuery_FieldName.Replace("Top " + NoOfTopRows.ToString(), "");
                strQuery_WhereClause = null;
                return GetDataTable(strQuery_Table, strQuery_FieldName, strQuery_WhereClause, strQuery_OrderBy);
            }
            else
            {
                AQuery = ReturnCombinedQuery(strQuery_Table, strQuery_FieldName, strQuery_WhereClause, strQuery_GroupBy, strQuery_OrderBy, "CompanySegment");
            }
            return null;
        }
        #endregion

        #region ExchangeSegment
        public DataTable GetExchangeSegmentFromMenuSegment(string CompanyID, int MenuID)
        {
            string strQuery_Table = @"(Select Exch_CompID,Exch_InternalID,Exh_ShortName,Exch_SegmentID from Tbl_Master_Exchange,Tbl_Master_CompanyExchange 
        Where Exh_CntId=Exch_ExchID and Exh_shortName+'-'+Exch_segmentId=(Select Seg_Name from tbl_master_segment Where seg_id=" + MenuID + @")
        and Exch_CompID='" + CompanyID + "') as T1,Master_Exchange";
            string strQuery_FieldName = @"(Select ExchangeSegment_ID From Master_ExchangeSegments Where ExchangeSegment_ExchangeID=Exchange_ID 
        and ExchangeSegment_Code=Exch_SegmentID) as [Session_ExchangeSegmentID]";
            string strQuery_WhereClause = "Exchange_ShortName=Exh_ShortName";
            string strQuery_OrderBy = null;
            string strQuery_GroupBy = null;

            return GetDataTable(strQuery_Table, strQuery_FieldName, strQuery_WhereClause, strQuery_OrderBy);
        }
        #endregion

        #region UserLastSegment
        public DataTable GetUserLastSegmentDetail()
        {
            string strQuery_Table = @"(Select ls_lastdpcoid,ls_lastCompany,ls_lastFinYear,ls_lastSettlementNo,ls_lastSettlementType,
        Session_ExchangeSegmentID,CmpLedgerView from 
        (Select ls_lastdpcoid,ls_lastCompany,ls_lastFinYear,ls_lastSettlementNo,ls_lastSettlementType,ls_lastSegment,
        (Select seg_name from tbl_master_segment Where seg_id='" + HttpContext.Current.Session["UserLastSegment"] + @"') SegName
        from tbl_trans_LastSegment 
        WHERE  ls_lastSegment='" + HttpContext.Current.Session["UserLastSegment"] + @"' 
        and ls_userId='" + HttpContext.Current.Session["UserID"] + @"') T1
        Left outer Join 
        (Select Exch_CompID,
        (Select Ltrim(RTrim(Cmp_Name))+' ['+Cmp_InternalID+']' from Tbl_Master_Company Where Cmp_InternalID=Exch_CompID) as Company,
        (Select isnull(cmp_LedgerView,'C') from Tbl_Master_Company Where Cmp_InternalID=Exch_CompID) as CmpLedgerView,
        (Select ExchangeSegment_ID From Master_ExchangeSegments Where ExchangeSegment_ExchangeID=Exchange_ID and ExchangeSegment_Code=Exch_SegmentID) as [Session_ExchangeSegmentID],
        Exch_InternalID as [Session_UserSegID],Exh_ShortName,Exch_SegmentID from
        (Select Exch_CompID,Exch_InternalID,Exh_ShortName,Exch_SegmentID from Tbl_Master_Exchange,Tbl_Master_CompanyExchange 
        Where Exh_CntId=Exch_ExchID) as T1,Master_Exchange
        Where Exchange_ShortName=Exh_ShortName) T2
        On Exh_shortName+'-'+Exch_segmentId=SegName
        and exch_compId=ls_lastCompany) T3
        Left Outer Join
        (select cast(Settlements_StartDateTime as varchar)+','+cast(Settlements_FundsPayin as varchar)+','+
        cast(Settlements_EndDateTime as varchar) StartdateFundsPayindate,settlements_Number,settlements_TypeSuffix,Settlements_ExchangeSegmentID
        from Master_Settlements) T4
        On Ltrim(RTRIM(settlements_Number))=ls_lastSettlementNo
        and ltrim(RTRIM(settlements_TypeSuffix))=ls_lastSettlementType
        and Settlements_ExchangeSegmentID=Session_ExchangeSegmentID";
            string strQuery_FieldName = @"ls_lastdpcoid,ls_lastCompany,ls_lastFinYear,ls_lastSettlementNo,ls_lastSettlementType,StartdateFundsPayindate,CmpLedgerView";
            string strQuery_WhereClause = null;
            string strQuery_OrderBy = null;
            string strQuery_GroupBy = null;

            return GetDataTable(strQuery_Table, strQuery_FieldName, strQuery_WhereClause, strQuery_OrderBy);
        }
        #endregion



        #endregion

        #region DBEngine Methods

        public void GetConnection()
        {
            ReadOnlyConnection();
            if (oSqlConnection.State.Equals(ConnectionState.Open))
            {
            }
            else
            {
                oSqlConnection.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]); ;
                oSqlConnection.Open();
            }
        }
        public void CloseConnection()
        {
            if (oSqlConnection.State.Equals(ConnectionState.Open))
            {
                oSqlConnection.Close();
            }
        }
        public string[,] GetFieldValue(
                String cTableName,      // TableName from which the field value is to be fetched
                String cFieldName,      // The name of the field whose value needs to be returned
                String cWhereClause,    // Optional : WHERE condition [if any]
                int NoField)            // Number of field value to selection
        {

            // Return Value
            string[,] vRetVal;
            String lcSql;
            // Count how many rows of data is there?
            // according to that we can define array lenth!
            //lcSql = "Select Count(*) from tbl_master_segment"; //+ cTableName;
            lcSql = "Select Count(*) from " + cTableName;
            if (cWhereClause != null)
            {
                lcSql += " WHERE " + cWhereClause;
            }
            SqlDataReader lsdr;
            lsdr = GetReader(lcSql);
            int CountNoRow = 0;
            while (lsdr.Read())
            {
                CountNoRow = (int)lsdr[0];
            }
            lsdr.Close();

            if (CountNoRow > 0)
            {
                //Now define length of an array
                vRetVal = new string[CountNoRow, NoField];

                // Now we construct a SQL command that will fetch the field from the table
                lcSql = "Select " + cFieldName + " from " + cTableName;
                if (cWhereClause != null)
                {
                    lcSql += " WHERE " + " " + cWhereClause;
                }

                //SqlDataReader lsdr;
                lsdr = GetReader(lcSql);

                if (lsdr.HasRows)
                {
                    //vRetVal = "inside datareader if data is present";
                    int i = 0;
                    while (lsdr.Read())
                    {
                        for (int j = 0; j < NoField; j++)
                        {
                            vRetVal[i, j] = lsdr[j].ToString();
                            //vRetValInt.Add(lsdr[i].ToString());
                        }
                        i = i + 1;
                    }
                }
                else
                {
                    string[,] messg = new string[1, 1];
                    messg[0, 0] = "n";
                    // We close the DataReader
                    lsdr.Close();

                    return messg;

                }
            }
            else
            {
                string[,] messg = new string[1, 1];
                messg[0, 0] = "n";
                // We close the DataReader
                lsdr.Close();

                return messg;
            }

            // We close the DataReader
            lsdr.Close();

            return vRetVal;

        }

        public string[,] GetFieldValue(
                String cTableName,      // TableName from which the field value is to be fetched
                String cFieldName,      // The name of the field whose value needs to be returned
                String cWhereClause,    // Optional : WHERE condition [if any]
                int NoField,             // Number of field value to selection
                string cOrderbyName)    // Order by field
        {

            // Return Value
            string[,] vRetVal;
            String lcSql;
            // Count how many rows of data is there?
            // according to that we can define array lenth!
            //lcSql = "Select Count(*) from tbl_master_segment"; //+ cTableName;
            lcSql = "Select Count(*) from " + cTableName;
            if (cWhereClause != null)
            {
                lcSql += " WHERE " + cWhereClause;
            }
            SqlDataReader lsdr;
            lsdr = GetReader(lcSql);
            int CountNoRow = 0;
            while (lsdr.Read())
            {
                CountNoRow = (int)lsdr[0];
            }
            lsdr.Close();

            //Now define length of an array
            vRetVal = new string[CountNoRow, NoField];

            // Now we construct a SQL command that will fetch the field from the table
            lcSql = "Select " + cFieldName + " from " + cTableName;
            if (cWhereClause != null)
            {
                lcSql += " WHERE " + cWhereClause;
            }
            if (cOrderbyName != null)
            {
                lcSql += " Order By " + cOrderbyName;
            }
            //SqlDataReader lsdr;
            lsdr = GetReader(lcSql);

            if (lsdr.HasRows)
            {
                //vRetVal = "inside datareader if data is present";
                int i = 0;
                while (lsdr.Read())
                {
                    for (int j = 0; j < NoField; j++)
                    {
                        vRetVal[i, j] = lsdr[j].ToString();
                        //vRetValInt.Add(lsdr[i].ToString());
                    }
                    i = i + 1;
                }
            }
            else
            {
                string[,] messg = new string[1, 1];
                messg[0, 0] = "n";
                lsdr.Close();

                return messg;
                /* vRetValInt.Add("n");
                vRetVal.Add(vRetValInt);*/
            }

            // We close the DataReader
            lsdr.Close();

            return vRetVal;
            //return lcmd;
        }


        public DataTable GetDataTable(
                            String cTableName,     // TableName from which the field value is to be fetched
                            String cFieldName,     // The name if the field whose value needs to be returned
                            String cWhereClause)   // WHERE condition [if any]
        {
            // Now we construct a SQL command that will fetch fields from the Suplied table

            String lcSql;
            lcSql = "Select " + cFieldName + " from " + cTableName;
            if (cWhereClause != null)
            {
                lcSql += " WHERE " + cWhereClause;
            }

            GetConnection();
            SqlDataAdapter lda = new SqlDataAdapter(lcSql, oSqlConnection);
            // createing an object of datatable        
            DataTable GetTable = new DataTable();
            lda.SelectCommand.CommandTimeout = 0;
            lda.Fill(GetTable);
            oSqlConnection.Close();
            return GetTable;

        }

        public DataTable GetDataTable(
                           String query)    // TableName from which the field value is to be fetched
        // The name if the field whose value needs to be returned
        // WHERE condition [if any]
        {
            // Now we construct a SQL command that will fetch fields from the Suplied table

            String lcSql;
            lcSql = query;

            GetConnection();
            SqlDataAdapter lda = new SqlDataAdapter(lcSql, oSqlConnection);
            // createing an object of datatable        
            DataTable getquery = new DataTable();
            lda.SelectCommand.CommandTimeout = 0;
            lda.Fill(getquery);
            oSqlConnection.Close();
            return getquery;

        }

        public DataTable GetDataTable(
                            String cTableName,     // TableName from which the field value is to be fetched
                            String cFieldName,     // The name if the field whose value needs to be returned
                            String cWhereClause,   // WHERE condition [if any]
                            string cOrderBy)       // Order by condition
        {
            // Now we construct a SQL command that will fetch fields from the Suplied table

            String lcSql;
            lcSql = "Select " + cFieldName + " from " + cTableName;
            if (cWhereClause != null)
            {
                lcSql += " WHERE " + cWhereClause;
            }
            if (cOrderBy != null)
            {
                lcSql += " Order By " + cOrderBy;
            }

            GetConnection();
            SqlDataAdapter lda = new SqlDataAdapter(lcSql, oSqlConnection);
            // createing an object of datatable
            DataTable GetTable = new DataTable();
            lda.SelectCommand.CommandTimeout = 0;
            lda.Fill(GetTable);
            oSqlConnection.Close();
            return GetTable;

        }
        public DataTable GetDataTableGroup(
                            String cTableName,     // TableName from which the field value is to be fetched
                            String cFieldName,     // The name if the field whose value needs to be returned
                            String cWhereClause,   // WHERE condition [if any]
                            string groupBy)       // Group by condition
        {
            // Now we construct a SQL command that will fetch fields from the Suplied table

            String lcSql;
            lcSql = "Select " + cFieldName + " from " + cTableName;
            if (cWhereClause != null)
            {
                lcSql += " WHERE " + cWhereClause;
            }
            if (groupBy != null)
            {
                lcSql += " group By " + groupBy;
            }

            GetConnection();
            SqlDataAdapter lda = new SqlDataAdapter(lcSql, oSqlConnection);
            // createing an object of datatable
            DataTable GetTable = new DataTable();
            lda.SelectCommand.CommandTimeout = 0;
            lda.Fill(GetTable);
            oSqlConnection.Close();
            return GetTable;

        }
        public DataTable GetDataTable(
                            String cTableName,     // TableName from which the field value is to be fetched
                            String cFieldName,     // The name if the field whose value needs to be returned
                            String cWhereClause,   // WHERE condition [if any]
                            string groupBy,         // Gropu by condition
                            string cOrderBy)        // Order by condition
        {
            // Now we construct a SQL command that will fetch fields from the Suplied table

            String lcSql;
            lcSql = "Select " + cFieldName + " from " + cTableName;
            if (cWhereClause != null)
            {
                lcSql += " WHERE " + cWhereClause;
            }
            if (groupBy != null)
            {
                lcSql += " group By " + groupBy;
            }
            if (cOrderBy != null)
            {
                lcSql += " Order By " + cOrderBy;
            }

            GetConnection();
            SqlDataAdapter lda = new SqlDataAdapter(lcSql, oSqlConnection);
            // createing an object of datatable
            DataTable GetTable = new DataTable();
            lda.SelectCommand.CommandTimeout = 0;
            lda.Fill(GetTable);
            oSqlConnection.Close();
            return GetTable;

        }
        public SqlDataReader GetReader(String cSql)
        {
            GetConnection();
            SqlDataReader lsdr;
            SqlCommand lcmd = new SqlCommand(cSql, oSqlConnection);
            lsdr = lcmd.ExecuteReader();
            return lsdr;
        }
        //======GetDate()===============================
        public DateTime GetDate()
        {
            DateTime Date = new DateTime();
            Date = Convert.ToDateTime("1900-01-01");
            string Query = "Select Getdate()";
            SqlDataReader Dr = GetReader(Query);
            try
            {
                while (Dr.Read())
                    if (Dr[0] != null)
                        Date = Convert.ToDateTime(Dr[0].ToString());
                Dr.Close();
            }
            catch { Dr.Close(); }
            finally { Dr.Close(); }
            return Date;
        }

        public DateTime GetDate(string sdate)
        {
            if (sdate != "")
            {
                DateTime Date = new DateTime();
                Date = Convert.ToDateTime(sdate);
                //string Query = "Select Getdate()";
                //SqlDataReader Dr = GetReader(Query);
                //try
                //{
                //    while (Dr.Read())
                //        if (Dr[0] != null)
                //            Date = Convert.ToDateTime(Dr[0].ToString());
                //    Dr.Close();
                //}
                //catch { Dr.Close(); }
                //finally { Dr.Close(); }
                return Date;
            }
            else
            {
                return DateTime.Now;
            }
        }
        public string GetDate(int StringFormatNumericValue)
        {
            string Date;
            Date = String.Empty;
            string Query = "Select Convert(Varchar,Getdate()," + StringFormatNumericValue.ToString().Trim() + ")";
            SqlDataReader Dr = GetReader(Query);
            try
            {
                while (Dr.Read())
                    if (Dr[0] != null)
                        Date = Dr[0].ToString();
                Dr.Close();
            }
            catch { Dr.Close(); }
            finally { Dr.Close(); }
            return Date;
        }
        //Convert UTC DateTime To DataBase DateTime Format
        public string GetDate(Enum DateConvertFrom, string DateString)
        {
            string Date;
            Date = String.Empty;
            string Query = String.Empty;
            if (DateString.Trim() != String.Empty && DateString != null)
            {
                if (DateConvertFrom.ToString() == "UTCToDateTime")
                {
                    Query = @"SELECT 
            CAST(SUBSTRING('" + DateString + "', 5, CHARINDEX('UTC','" + DateString + "',1)-6) + SUBSTRING('" + DateString + "',CHARINDEX('UTC','" + DateString + "',1)+8,8000) AS DATETIME)";
                }
                else if (DateConvertFrom.ToString() == "UTCToOnlyDate")
                {
                    Query = @"Select (DATEADD(dd, 0, DATEDIFF(dd, 0, CAST(SUBSTRING('" + DateString + "', 5, CHARINDEX('UTC','" + DateString + "',1)-6) + SUBSTRING('" + DateString + "',CHARINDEX('UTC','" + DateString + "',1)+8,8000) AS DATETIME))))";
                }
                else if (DateConvertFrom.ToString() == "UTCToOnlyTime")
                {
                    Query = @"SELECT 
            Convert(Varchar(8),CAST(SUBSTRING('" + DateString + "', 5, CHARINDEX('UTC','" + DateString + "',1)-6) + SUBSTRING('" + DateString + "',CHARINDEX('UTC','" + DateString + "',1)+8,8000) AS DATETIME),108) [DateTime]";
                }
                SqlDataReader Dr = GetReader(Query);
                try
                {
                    while (Dr.Read())
                        if (Dr[0] != null)
                            Date = Dr[0].ToString();
                    Dr.Close();
                }
                catch { Dr.Close(); }
                finally { Dr.Close(); }
            }
            return Date;
        }
        //====================================


        //__This function help to enter ',= thease charectore in database as a string content
        // Example usage:
        //
        public string SepCharectores(string stringitem)
        {
            string rtn = "";
            for (int i = 0; i < stringitem.Length; i++)
            {
                if (stringitem[i].ToString() == "'")
                {
                    if (i != 0 && i != stringitem.Length - 1)
                    {
                        if (stringitem[i + 1].ToString() == "," || stringitem[i - 1].ToString() == "," || stringitem[i + 1].ToString() == "=" || stringitem[i - 1].ToString() == "=" || stringitem[i + 1].ToString() == "'" || stringitem[i - 1].ToString() == "'" || stringitem[i + 1].ToString() == " " || stringitem[i - 1].ToString() == " ")
                        {
                            rtn += stringitem[i];
                        }
                        else
                            rtn += stringitem[i] + "'";
                    }
                    else
                        rtn += stringitem[i];
                }
                else
                    rtn += stringitem[i];
            }
            return rtn;
        }
        // this will insert Any Data to any table
        //
        public Int32 Insert_Table(
                    String cTableName,      // TableName to which the field value is to be Insert
                    String cFieldName,      // Name of the field and values whose data needs to be Insert
                    String cFieldValue     // Value of fields 
                    )
        {
            cFieldValue = SepCharectores(cFieldValue);
            String lSsql = "INSERT INTO " + cTableName + "(" + cFieldName + ")" + "values(" + cFieldValue + ")";

            GetConnection();
            SqlCommand lcmd = new SqlCommand(lSsql, oSqlConnection);
            Int32 rowsEffected = 0;
            try
            {
                rowsEffected = lcmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                oSqlConnection.Close();
                return rowsEffected;


            }
            oSqlConnection.Close();
            return rowsEffected;
        }

        public Int32 Delete_Table(
               String cTableName,      // TableName to which the field value is to be updated            
               String cWhereClause    // WHERE condition 
               )
        {
            String lSsql = "Delete from " + cTableName + " Where " + cWhereClause;

            GetConnection();
            SqlCommand lcmd = new SqlCommand(lSsql, oSqlConnection);
            Int32 rowscount = lcmd.ExecuteNonQuery();
            oSqlConnection.Close();
            return rowscount;
        }

        // This table update ANY table field When called!
        // Example Usage :
        // 
        public Int32 Update_Table(
                String cTableName,      // TableName to which the field value is to be updated
                String cFieldName,      // Name of the field and values whose data needs to be upadted
                String cWhereClause    // WHERE condition 
                )
        {
            cFieldName = SepCharectores(cFieldName);
            if (cWhereClause != null)
            {
                String lSsql = "Update " + cTableName + " SET " + cFieldName + " Where " + cWhereClause;

                GetConnection();
                SqlCommand lcmd = new SqlCommand(lSsql, oSqlConnection);
                Int32 rowsEffected = 0;
                try
                {
                    rowsEffected = lcmd.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    oSqlConnection.Close();
                    return rowsEffected;
                }
                oSqlConnection.Close();
                return rowsEffected;
            }
            return 0;
        }

        // --------------------------------------------------------------------------//
        // This method returns the value of ANY field from ANY table in the database.
        // Example Usage :
        // GetFieldValue("tbl_master_bank", "bnk_bankName", "bnk_id=7075")
        // NOT in Use Right now, Because of ONE DIAMENTINAL ARRAY!
        // --------------------------------------------------------------------------//
        public string[] GetFieldValue1(
                String cTableName,      // TableName from which the field value is to be fetched
                String cFieldName,     // The name if the field whose value needs to be returned
                String cWhereClause,    // Optional : WHERE condition [if any]
                int NoField)   // Number of field value to selection
        {

            // Return Value
            string[] vRetVal = new string[NoField];
            /*ArrayList vRetVal = new ArrayList();
            ArrayList vRetValInt = new ArrayList();*/

            // Now we construct a SQL command that will fetch the field from the table

            String lcSql;
            lcSql = "Select " + cFieldName + " from " + cTableName;
            if (cWhereClause != null)
            {
                lcSql += " WHERE " + cWhereClause;
            }

            SqlDataReader lsdr;
            lsdr = GetReader(lcSql);

            if (lsdr.HasRows)
            {
                //vRetVal = "inside datareader if data is present";
                //int j = 0;
                while (lsdr.Read())
                {
                    for (int i = 0; i < NoField; i++)
                    {
                        vRetVal[i] = lsdr[i].ToString();
                        //vRetValInt.Add(lsdr[i].ToString());
                    }
                    //j = j + 1;
                    //vRetVal.Add(vRetValInt);
                }
            }
            else
            {
                vRetVal[0] = "n";
                /*vRetValInt.Add("n");
                vRetVal.Add(vRetValInt);*/
            }

            // We close the DataReader
            lsdr.Close();

            return vRetVal;
            //return lcmd;
        }
        #endregion

        #region KRA
        public string GetAddressProofData(string KycRaIdMap)
        {
            string strQuery_Table = @"Master_AddressProof";

            string strQuery_FieldName = null;
            if (KycRaIdMap != null)
                strQuery_FieldName = "Top 10 AddressProof_Description TextField,Cast(Ltrim(Rtrim(AddressProof_ID)) as Varchar(10))+'~'+Ltrim(Rtrim(IsNull(AddressProof_Description,'')))+'~'+Cast(Ltrim(Rtrim(IsNull(" + KycRaIdMap + ",'0'))) as Varchar(10)) ValueField";
            else
                strQuery_FieldName = "Top 10 AddressProof_Description TextField,Cast(Ltrim(Rtrim(AddressProof_ID)) as Varchar(10))+'~'+Ltrim(Rtrim(IsNull(AddressProof_Description,''))) ValueField";

            string strQuery_WhereClause = @"AddressProof_Description Like '%RequestLetter%'";
            string strQuery_OrderBy = "TextField";
            string strQuery_GroupBy = null;
            return ReturnCombinedQuery(strQuery_Table, strQuery_FieldName, strQuery_WhereClause, strQuery_GroupBy, strQuery_OrderBy, "ADDRESSPROOF");
        }

        public string GetIdentityProofData(string KycRaIdMap)
        {
            string strQuery_Table = @"Master_IdentityProof";

            string strQuery_FieldName = null;
            if (KycRaIdMap != null)
                strQuery_FieldName = "Top 10 IdentityProof_Description TextField,Cast(Ltrim(Rtrim(IdentityProof_ID)) as Varchar(10))+'~'+Ltrim(Rtrim(IsNull(IdentityProof_Description,'')))+'~'+Cast(Ltrim(Rtrim(IsNull(" + KycRaIdMap + ",'0'))) as Varchar(10)) ValueField";
            else
                strQuery_FieldName = "Top 10 IdentityProof_Description TextField,Cast(Ltrim(Rtrim(IdentityProof_ID)) as Varchar(10))+'~'+Ltrim(Rtrim(IsNull(IdentityProof_Description,''))) ValueField";

            string strQuery_WhereClause = @"IdentityProof_Description Like '%RequestLetter%'";
            string strQuery_OrderBy = "TextField";
            string strQuery_GroupBy = null;
            return ReturnCombinedQuery(strQuery_Table, strQuery_FieldName, strQuery_WhereClause, strQuery_GroupBy, strQuery_OrderBy, "IDENTITYPROOF");
        }

        public string GetPanExemptCategoryData(string KycRaIdMap)
        {
            string strQuery_Table = @"Master_PanExemptCategory";

            string strQuery_FieldName = null;
            if (KycRaIdMap != null)
                strQuery_FieldName = "Top 10 PanExemptCategory_Description TextField,Cast(Ltrim(Rtrim(PanExemptCategory_ID)) as Varchar(10))+'~'+Ltrim(Rtrim(IsNull(PanExemptCategory_Description,'')))+'~'+Cast(Ltrim(Rtrim(IsNull(" + KycRaIdMap + ",'0'))) as Varchar(10)) ValueField";
            else
                strQuery_FieldName = "Top 10 PanExemptCategory_Description TextField,Cast(Ltrim(Rtrim(PanExemptCategory_ID)) as Varchar(10))+'~'+Ltrim(Rtrim(IsNull(PanExemptCategory_Description,''))) ValueField";

            string strQuery_WhereClause = @"PanExemptCategory_Description Like '%RequestLetter%'";
            string strQuery_OrderBy = "TextField";
            string strQuery_GroupBy = null;
            return ReturnCombinedQuery(strQuery_Table, strQuery_FieldName, strQuery_WhereClause, strQuery_GroupBy, strQuery_OrderBy, "PANEXEMPTCATEGORY");
        }

        public string GetIndividualStatusData(string KycRaIdMap)
        {
            string strQuery_Table = @"Master_IndividualStatus";

            string strQuery_FieldName = null;
            if (KycRaIdMap != null)
                strQuery_FieldName = "Top 10 IndividualStatus_Description TextField,Cast(Ltrim(Rtrim(IndividualStatus_ID)) as Varchar(10))+'~'+Ltrim(Rtrim(IsNull(IndividualStatus_Description,'')))+'~'+Cast(Ltrim(Rtrim(IsNull(" + KycRaIdMap + ",'0'))) as Varchar(10)) ValueField";
            else
                strQuery_FieldName = "Top 10 IndividualStatus_Description TextField,Cast(Ltrim(Rtrim(IndividualStatus_ID)) as Varchar(10))+'~'+Ltrim(Rtrim(IsNull(IndividualStatus_Description,''))) ValueField";

            string strQuery_WhereClause = @"IndividualStatus_Description Like '%RequestLetter%'";
            string strQuery_OrderBy = "TextField";
            string strQuery_GroupBy = null;
            return ReturnCombinedQuery(strQuery_Table, strQuery_FieldName, strQuery_WhereClause, strQuery_GroupBy, strQuery_OrderBy, "INDIVIDUALSTATUS");
        }

        public string GetNRIStatusProofData(string KycRaIdMap)
        {
            string strQuery_Table = @"Master_NRIStatusProof";

            string strQuery_FieldName = null;
            if (KycRaIdMap != null)
                strQuery_FieldName = "Top 10 NRIStatusProof_Description TextField,Cast(Ltrim(Rtrim(NRIStatusProof_ID)) as Varchar(10))+'~'+Ltrim(Rtrim(IsNull(NRIStatusProof_Description,'')))+'~'+Cast(Ltrim(Rtrim(IsNull(" + KycRaIdMap + ",'0'))) as Varchar(10)) ValueField";
            else
                strQuery_FieldName = "Top 10 NRIStatusProof_Description TextField,Cast(Ltrim(Rtrim(NRIStatusProof_ID)) as Varchar(10))+'~'+Ltrim(Rtrim(IsNull(NRIStatusProof_Description,''))) ValueField";

            string strQuery_WhereClause = @"NRIStatusProof_Description Like '%RequestLetter%'";
            string strQuery_OrderBy = "TextField";
            string strQuery_GroupBy = null;
            return ReturnCombinedQuery(strQuery_Table, strQuery_FieldName, strQuery_WhereClause, strQuery_GroupBy, strQuery_OrderBy, "NRISTATUSPROOF");
        }

        public string GetNonIndividualStatusData(string KycRaIdMap)
        {
            string strQuery_Table = @"Master_NonIndividualStatus";

            string strQuery_FieldName = null;
            if (KycRaIdMap != null)
                strQuery_FieldName = "Top 10 NonIndividualStatus_Description TextField,Cast(Ltrim(Rtrim(NonIndividualStatus_ID)) as Varchar(10))+'~'+Ltrim(Rtrim(IsNull(NonIndividualStatus_Description,'')))+'~'+Cast(Ltrim(Rtrim(IsNull(" + KycRaIdMap + ",'0'))) as Varchar(10)) ValueField";
            else
                strQuery_FieldName = "Top 10 NonIndividualStatus_Description TextField,Cast(Ltrim(Rtrim(NonIndividualStatus_ID)) as Varchar(10))+'~'+Ltrim(Rtrim(IsNull(NonIndividualStatus_Description,''))) ValueField";

            string strQuery_WhereClause = @"NonIndividualStatus_Description Like '%RequestLetter%'";
            string strQuery_OrderBy = "TextField";
            string strQuery_GroupBy = null;
            return ReturnCombinedQuery(strQuery_Table, strQuery_FieldName, strQuery_WhereClause, strQuery_GroupBy, strQuery_OrderBy, "NONINDIVIDUALSTATUS");
        }

        public string GetOccupationData(string KycRaIdMap)
        {
            string strQuery_Table = @"tbl_master_profession";

            string strQuery_FieldName = null;
            if (KycRaIdMap != null)
                strQuery_FieldName = "Top 10 pro_professionName TextField,Cast(Ltrim(Rtrim(pro_id)) as Varchar(10))+'~'+Ltrim(Rtrim(IsNull(pro_professionName,'')))+'~'+Cast(Ltrim(Rtrim(IsNull(" + KycRaIdMap + ",'0'))) as Varchar(10)) ValueField";
            else
                strQuery_FieldName = "Top 10 pro_professionName TextField,Cast(Ltrim(Rtrim(pro_id)) as Varchar(10))+'~'+Ltrim(Rtrim(IsNull(pro_professionName,''))) ValueField";

            string strQuery_WhereClause = @"pro_professionName Like '%RequestLetter%'";
            string strQuery_OrderBy = "TextField";
            string strQuery_GroupBy = null;
            return ReturnCombinedQuery(strQuery_Table, strQuery_FieldName, strQuery_WhereClause, strQuery_GroupBy, strQuery_OrderBy, "OCCUPATION");
        }

        public string GetPoliticalConnectionData(string KycRaIdMap)
        {
            string strQuery_Table = @"Master_AddressProof";

            string strQuery_FieldName = null;
            if (KycRaIdMap != null)
                strQuery_FieldName = "Top 10 PoliticalConnection_Description TextField,Cast(Ltrim(Rtrim(PoliticalConnection_ID)) as Varchar(10))+'~'+Ltrim(Rtrim(IsNull(PoliticalConnection_Description,'')))+'~'+Cast(Ltrim(Rtrim(IsNull(" + KycRaIdMap + ",'0'))) as Varchar(10)) ValueField";
            else
                strQuery_FieldName = "Top 10 PoliticalConnection_Description TextField,Cast(Ltrim(Rtrim(PoliticalConnection_ID)) as Varchar(10))+'~'+Ltrim(Rtrim(IsNull(PoliticalConnection_Description,''))) ValueField";

            string strQuery_WhereClause = @"PoliticalConnection_Description Like '%RequestLetter%'";
            string strQuery_OrderBy = "TextField";
            string strQuery_GroupBy = null;
            return ReturnCombinedQuery(strQuery_Table, strQuery_FieldName, strQuery_WhereClause, strQuery_GroupBy, strQuery_OrderBy, "POLITICALCONNECTION");
        }

        public string GetMaritalStatusData(string KycRaIdMap)
        {
            string strQuery_Table = @"tbl_master_maritalstatus";

            string strQuery_FieldName = null;
            if (KycRaIdMap != null)
                strQuery_FieldName = "Top 10 AddressProof_Description TextField,Cast(Ltrim(Rtrim(mts_id)) as Varchar(10))+'~'+Ltrim(Rtrim(IsNull(mts_maritalStatus,'')))+'~'+Cast(Ltrim(Rtrim(IsNull(" + KycRaIdMap + ",'0'))) as Varchar(10)) ValueField";
            else
                strQuery_FieldName = "Top 10 AddressProof_Description TextField,Cast(Ltrim(Rtrim(mts_id)) as Varchar(10))+'~'+Ltrim(Rtrim(IsNull(mts_maritalStatus,''))) ValueField";

            string strQuery_WhereClause = @"mts_maritalStatus Like '%RequestLetter%'";
            string strQuery_OrderBy = "TextField";
            string strQuery_GroupBy = null;
            return ReturnCombinedQuery(strQuery_Table, strQuery_FieldName, strQuery_WhereClause, strQuery_GroupBy, strQuery_OrderBy, "MARITALSTATUS");
        }

        public string GetStateData(string KycRaIdMap)
        {
            string strQuery_Table = @"tbl_master_state";

            string strQuery_FieldName = null;
            if (KycRaIdMap != null)
                strQuery_FieldName = "Top 10 state TextField,Cast(Ltrim(Rtrim(id)) as Varchar(10))+'~'+Ltrim(Rtrim(IsNull(state,'')))+'~'+Cast(Ltrim(Rtrim(IsNull(" + KycRaIdMap + ",'0'))) as Varchar(10)) ValueField";
            else
                strQuery_FieldName = "Top 10 state TextField,Cast(Ltrim(Rtrim(id)) as Varchar(10))+'~'+Ltrim(Rtrim(IsNull(state,''))) ValueField";

            string strQuery_WhereClause = @"state Like '%RequestLetter%'";
            string strQuery_OrderBy = "TextField";
            string strQuery_GroupBy = null;
            return ReturnCombinedQuery(strQuery_Table, strQuery_FieldName, strQuery_WhereClause, strQuery_GroupBy, strQuery_OrderBy, "STATE");
        }

        public string GetCountryData(string KycRaIdMap)
        {
            string strQuery_Table = @"tbl_master_country";

            string strQuery_FieldName = null;
            if (KycRaIdMap != null)
                strQuery_FieldName = "Top 10 cou_country TextField,Cast(Ltrim(Rtrim(cou_id)) as Varchar(10))+'~'+Ltrim(Rtrim(IsNull(cou_country,'')))+'~'+Cast(Ltrim(Rtrim(IsNull(" + KycRaIdMap + ",'0'))) as Varchar(10)) ValueField";
            else
                strQuery_FieldName = "Top 10 cou_country TextField,Cast(Ltrim(Rtrim(cou_id)) as Varchar(10))+'~'+Ltrim(Rtrim(IsNull(cou_country,''))) ValueField";

            string strQuery_WhereClause = @"cou_country Like '%RequestLetter%'";
            string strQuery_OrderBy = "TextField";
            string strQuery_GroupBy = null;
            return ReturnCombinedQuery(strQuery_Table, strQuery_FieldName, strQuery_WhereClause, strQuery_GroupBy, strQuery_OrderBy, "COUNTRY");
        }

        public string GetKRA_MappingStaticData(string KraMachCode, int KRAId)
        {
            string strQuery_Table = " (select KRAStaticData_ID,KRAStaticData_Value,KRAStaticData_Code,KRAStaticData_SubCode From Config_KRAStaticData Where KRAStaticData_Type='" + KraMachCode + "' and KRAStaticData_KRAId=" + KRAId + ") KRAData";
            string strQuery_FieldName = @" Top 10 KRAStaticData_Value TextField,Cast(Ltrim(Rtrim(KRAStaticData_ID)) as Varchar(10))+'~'+Ltrim(Rtrim(IsNull(KRAStaticData_Value,'')))+'~'+Ltrim(Rtrim(IsNull(KRAStaticData_Code,'')))+'~'+Ltrim(Rtrim(IsNull(KRAStaticData_SubCode,''))) ValueField";
            string strQuery_WhereClause = @" KRAStaticData_Value Like '%RequestLetter%'";
            string strQuery_OrderBy = "TextField";
            string strQuery_GroupBy = null;

            return ReturnCombinedQuery(strQuery_Table, strQuery_FieldName, strQuery_WhereClause, strQuery_GroupBy, strQuery_OrderBy, "KRAMAPSTATIC");
        }

        public string GetContactTypeData(string KycRaIdMap)
        {
            string strQuery_Table = @"tbl_master_contacttype";

            string strQuery_FieldName = null;
            if (KycRaIdMap != null)
                strQuery_FieldName = "Top 10 cnttpy_contactType TextField,Cast(Ltrim(Rtrim(cnttpy_id)) as Varchar(10))+'~'+Ltrim(Rtrim(IsNull(cnttpy_contactType,'')))+'~'+Cast(Ltrim(Rtrim(IsNull(" + KycRaIdMap + ",'0'))) as Varchar(10)) ValueField";
            else
                strQuery_FieldName = "Top 10 cnttpy_contactType TextField,Cast(Ltrim(Rtrim(cnttpy_id)) as Varchar(10))+'~'+Ltrim(Rtrim(IsNull(cnttpy_contactType,''))) ValueField";

            string strQuery_WhereClause = @"cnttpy_contactType Like '%RequestLetter%'";
            string strQuery_OrderBy = "TextField";
            string strQuery_GroupBy = null;
            return ReturnCombinedQuery(strQuery_Table, strQuery_FieldName, strQuery_WhereClause, strQuery_GroupBy, strQuery_OrderBy, "CONTACTTYPE");
        }

        public string GetKRANationalityData(string KycRaIdMap)
        {
            string strQuery_Table = @"Master_Nationality";
            string strQuery_FieldName = null;
            if (KycRaIdMap != null)
                strQuery_FieldName = "Top 10 Nationality_Description TextField,Cast(Ltrim(Rtrim(Nationality_ID)) as Varchar(10))+'~'+Ltrim(Rtrim(IsNull(Nationality_Description,'')))+'~'+Cast(Ltrim(Rtrim(IsNull(" + KycRaIdMap + ",'0'))) as Varchar(10)) ValueField";
            else
                strQuery_FieldName = "Top 10 Nationality_Description TextField,Cast(Ltrim(Rtrim(Nationality_ID)) as Varchar(10))+'~'+Ltrim(Rtrim(IsNull(Nationality_Description,''))) ValueField";

            string strQuery_WhereClause = @"Nationality_Description Like '%RequestLetter%'";
            string strQuery_OrderBy = "TextField";
            string strQuery_GroupBy = null;
            return ReturnCombinedQuery(strQuery_Table, strQuery_FieldName, strQuery_WhereClause, strQuery_GroupBy, strQuery_OrderBy, "KRANATIONALITY");
        }

        public string GetKRAAnnualIncomeData(string KycRaIdMap)
        {
            string strQuery_Table = @"Master_AnnualIncome";
            string strQuery_FieldName = null;
            if (KycRaIdMap != null)
                strQuery_FieldName = "Top 10 AnnualIncome_Description TextField,Cast(Ltrim(Rtrim(AnnualIncome_ID)) as Varchar(10))+'~'+Ltrim(Rtrim(IsNull(AnnualIncome_Description,'')))+'~'+Cast(Ltrim(Rtrim(IsNull(" + KycRaIdMap + ",'0'))) as Varchar(10)) ValueField";
            else
                strQuery_FieldName = "Top 10 AnnualIncome_Description TextField,Cast(Ltrim(Rtrim(AnnualIncome_ID)) as Varchar(10))+'~'+Ltrim(Rtrim(IsNull(AnnualIncome_Description,''))) ValueField";

            string strQuery_WhereClause = @"AnnualIncome_Description Like '%RequestLetter%'";
            string strQuery_OrderBy = "TextField";
            string strQuery_GroupBy = null;
            return ReturnCombinedQuery(strQuery_Table, strQuery_FieldName, strQuery_WhereClause, strQuery_GroupBy, strQuery_OrderBy, "KRAANNUALINCOME");
        }

        #endregion

        #region ServerDebugging

        //To execute following code we have to follow the below steps
        //step 1-string[,] strExecProc = new string[20, 2];String array(2D) declaration according to number of parameters
        //step 2-lcmdEmplInsert.Parameters.AddWithValue("@cmp_Name", txtCompname.Text);
        //step 3-strExecProc[0, 0] = "cmp_Name"; strExecProc[0, 1] = "'"+txtCompname.Text+"'"; this section will be add after every parameter passing(step 2) for sp execution
        //step 4-this section will create string and wll create a txt file as executable mode we can execute this txt contain as sql query.
        //oGenericMethod = new GenericMethod();
        //string strDateTime = oGenericMethod.GetDate().ToString("yyyyMMddHHmmss");
        //string FilePath = "../ExportFiles/ServerDebugging/Insert_RootCompany" + strDateTime + ".txt";
        //oGenericMethod.WriteFile(OldSpExecuteWriter(strExecProc, "CompanyInsert"), FilePath, false);

        public string OldSpExecuteWriter(string[,] Param, string SpName)
        {
            string strExecProc = string.Empty;
            strExecProc = "Exec " + SpName;
            for (int i = 0; i <= Param.GetUpperBound(0); i++)
            {
                if (Param[i, 1] != null && Param[i, 0] != null)
                    strExecProc = strExecProc + "@" + Param[i, 0].Trim() + "=" + Param[i, 1] + ",";
            }
            return strExecProc.Substring(0, strExecProc.Length - 1);
        }
        public string OldSpExecuteWriter(string SpName, string[] ParamName, string[] value)
        {
            string strExecProc = string.Empty;
            strExecProc = "Exec " + SpName;
            for (int i = 0; i < value.Length; i++)
            {
                if (ParamName[i] != null && value[i] != null)
                    strExecProc = strExecProc + " " + "@" + ParamName[i].Trim() + "=" + value[i] + ",";
            }
            return strExecProc.Substring(0, strExecProc.Length - 1);

        }
        #endregion

        #region FullDetail
        //[DTOrAQuery] Value(D Or A) D-->DataTable And A-->AjaxQuery
        //[AQuery]--Will Pass String Variable With Ref KeyWord Like {strubg CombinedQuery  And Pass It Like (ref CombinedQuery)
        //Will Contain Query For Ajax Query(It a Reference Variable So No Need To Return
        //[NoOfTopRows] If Greater Than 0 Then Work 
        //[AjaxLikeFields] is an Array Varible in Which You Can Pass Like {"CompnayName","UCC"}
        //[WhichTableQuery] is StaticQuery Allready Define You Donot Need To Pass it..


        DataTable FullDetail_CommonSection(string DTOrAQuery, ref string AQuery, int NoOfTopRows, string CustomizeWhere, string CustomizeSelection, string CustomizeOrderBy, string CustomizeGroupBy, string[] AjaxLikeFields, string WhichSectionCall, string WhichTableQuery)
        {
            strQuery_Table = WhichTableQuery;
            if (NoOfTopRows > 0)
                strQuery_FieldName = "Top " + NoOfTopRows.ToString() + CustomizeSelection;
            else
                strQuery_FieldName = CustomizeSelection;
            strQuery_OrderBy = CustomizeOrderBy;
            strQuery_GroupBy = CustomizeGroupBy;
            strQuery_WhereClause = null;
            if (DTOrAQuery == "D")
            {
                strQuery_WhereClause = CustomizeWhere;
                return GetDataTable(strQuery_Table, strQuery_FieldName, strQuery_WhereClause, strQuery_OrderBy);
            }
            else
            {
                foreach (string strALF in AjaxLikeFields)
                {
                    if (strQuery_WhereClause == null)
                        strQuery_WhereClause = strALF + "Like '%RequestLetter%'";
                    else
                        strQuery_WhereClause = strQuery_WhereClause + " And " + strALF + "Like '%RequestLetter%'";
                }
                strQuery_WhereClause = strQuery_WhereClause + " And (" + CustomizeWhere + ")";
                AQuery = ReturnCombinedQuery(strQuery_Table, strQuery_FieldName, strQuery_WhereClause, strQuery_GroupBy, strQuery_OrderBy, WhichSectionCall);
            }
            return null;
        }
        public DataTable FullDetail_Company(string DTOrAQuery, ref string AQuery, int NoOfTopRows, string CustomizeWhere, string CustomizeSelection, string CustomizeOrderBy, string CustomizeGroupBy, string[] AjaxLikeFields, int Version)
        {
            string WhichSectionCall = String.Empty;
            if (Version == 1)//ALL Company
                strQuery_Table = @"(Select  Ltrim(Rtrim(Cmp_Name)) TextField,Cast(Cmp_ID as Varchar(100))+'~'+LTRIM(Rtrim(isnull(Cmp_internalID,'')))+'~'+LTRIM(Rtrim(isnull(cmp_parentid,'')))+'~'+LTRIM(Rtrim(isnull(cmp_currencyid,'')))+'~'+LTRIM(Rtrim(isnull(Com_Add,''))) ValueField,cmp_id,cmp_Name,cmp_internalid,cmp_parentid,cmp_currencyid from tbl_master_Company) Company";
            else if (Version == 2)//ALL Company And Exchanges
                strQuery_Table = @"(Select Ltrim(Rtrim(Company)) TextField,Ltrim(Rtrim(CompanyID))+'~'+Ltrim(Rtrim(isnull(Cast(Session_ExchangeSegmentID as Varchar),'')))+'~'+Ltrim(Rtrim(Session_UserSegID))+'~'+Ltrim(Rtrim(exh_shortName))+'~'+Ltrim(Rtrim(isnull(exch_segmentId,''))) ValueField,CompanyID,Session_ExchangeSegmentID,Session_UserSegID,exh_shortName,exch_segmentId from (Select Ltrim(Rtrim(Exch_CompID)) as CompanyID,(Select Ltrim(RTrim(Cmp_Name)) from Tbl_Master_Company Where Cmp_InternalID=Exch_CompID) as Company,(Select ExchangeSegment_ID From Master_ExchangeSegments Where ExchangeSegment_ExchangeID=Exchange_ID and ExchangeSegment_Code=Exch_SegmentID) as [Session_ExchangeSegmentID],Exch_InternalID as [Session_UserSegID],Exh_ShortName,Exch_SegmentID from (Select Exch_CompID,Exch_InternalID,Exh_ShortName,Exch_SegmentID from Tbl_Master_Exchange,Tbl_Master_CompanyExchange WHERE Exh_CntId=Exch_ExchID) as T1,Master_Exchange Where Exchange_ShortName=Exh_ShortName) Exch_Exh) CompanyExchange";
            WhichSectionCall = "FullDetail_Company";
            return FullDetail_CommonSection(DTOrAQuery, ref AQuery, NoOfTopRows, CustomizeWhere, CustomizeSelection, CustomizeOrderBy, CustomizeGroupBy, AjaxLikeFields, WhichSectionCall, strQuery_Table);
        }

        #endregion

        #region Encrypt/Decript
        //======================================Expiry Module==========================
        //public string EncryptDecript(int Version, string WhichCall, string AnyValue)
        //{
        //    if (WhichCall == "SetExpiryDate")
        //    {
        //        if (Version == 1)
        //        {
        //            //Fetch Expiry From Encrypted File
        //            string CurrentSegmentName = GetFieldValue1("tbl_master_segment", "Seg_Name", "Seg_id=" + HttpContext.Current.Session["userlastsegment"], 1)[0];
        //            string CurrentCompanyName = GetCompanyDetail(HttpContext.Current.Session["LastCompany"].ToString()).Rows[0][1].ToString();
        //            string ExpiryValue = null;

        //            //====Start Decoding=======================================
        //            StreamReader decodeFile = new StreamReader(AnyValue);
        //            string StrDecodeXmlContent = decodeFile.ReadToEnd();
        //            decodeFile.Close();
        //            //===Content ReFormatting===========
        //            StrDecodeXmlContent = Encoding.Unicode.GetString(Convert.FromBase64String(StrDecodeXmlContent));
        //            decodeFile = null;

        //            GenericXML oGenericXML = new GenericXML();
        //            ExpiryValue = oGenericXML.Decrypt_License(GenericXML.XWhichMethod.RecordExist, StrDecodeXmlContent, CurrentCompanyName, CurrentSegmentName);
        //            //Set Expire Date 
        //            if (!ExpiryValue.Contains("Segment") && !ExpiryValue.Contains("Company"))
        //                HttpContext.Current.Session["ExpireDate"] = Convert.ToDateTime(ExpiryValue);
        //            else
        //                HttpContext.Current.Session["ExpireDate"] = Convert.ToDateTime("1900-01-01");
        //            //End Set Expire Date 
        //        }
        //        //====End Decoding=======================================  
        //    }
        //    return null;
        //    //======================================End Expiry Module==========================
        //}

        public string EncryptDecript(int Version, string WhichCall, string AnyValue)
        {
            string CallBy = WhichCall.Split('~')[0].ToString();
            string StrDecodeXmlContent = "";

            if (CallBy == "SetExpiryDate")
            {
                //====Start Decoding=======================================                
                if (Version == 1)
                {
                    //Fetch Expiry From Encrypted File
                    string ExpiryValue = null;
                    string CurrentSegmentName = GetFieldValue1("tbl_master_segment", "Seg_Name", "Seg_id=" + HttpContext.Current.Session["userlastsegment"], 1)[0];
                    string CurrentCompanyName = null;

                    if (CurrentSegmentName == "HR")
                        HttpContext.Current.Session["ExpireDate"] = Convert.ToDateTime("2300-12-31");
                    else if (CurrentSegmentName == "CRM")
                        HttpContext.Current.Session["ExpireDate"] = Convert.ToDateTime("2300-12-31");
                    else if (CurrentSegmentName == "Root")
                        HttpContext.Current.Session["ExpireDate"] = Convert.ToDateTime("2300-12-31");
                    else
                    {
                        if (HttpContext.Current.Session["LastCompany"].ToString() != "")
                            CurrentCompanyName = GetCompanyDetail(HttpContext.Current.Session["LastCompany"].ToString()).Rows[0][1].ToString();
                        else
                        {
                            string contactID = HttpContext.Current.Session["usercontactID"].ToString();
                            DataTable dtcmp = GetDataTable(" tbl_master_company  ", "*", "cmp_id=(select emp_organization from tbl_trans_employeectc where emp_cntId='" + contactID + "' and emp_effectiveuntil is null)");
                            CurrentCompanyName = dtcmp.Rows[0]["cmp_Name"].ToString();
                            dtcmp = null;
                        }

                        StreamReader decodeFile = new StreamReader(AnyValue);
                        StrDecodeXmlContent = decodeFile.ReadToEnd();
                        decodeFile.Close();
                        //===Content ReFormatting===========
                        StrDecodeXmlContent = Encoding.Unicode.GetString(Convert.FromBase64String(StrDecodeXmlContent));
                        decodeFile = null;

                        GenericXML oGenericXML = new GenericXML();
                        string[] EDParam = { "//cName[@Value='" + CurrentCompanyName + "']/@Value", "//Segments[@Value='" + CurrentSegmentName + "']/@Value", "//cName[@Value='" + CurrentCompanyName + "']/Segments[@Value='" + CurrentSegmentName + "']/Expiry" };
                        string[] EDValue = { CurrentCompanyName, CurrentSegmentName, "" };
                        ExpiryValue = oGenericXML.Decrypt_License(GenericXML.XWhichMethod.RecordExist, StrDecodeXmlContent, EDParam, EDValue);
                        //Start Set Expire Date 
                        if (!ExpiryValue.Contains("Segment") && !ExpiryValue.Contains("Company") && !ExpiryValue.Contains("Invalid"))
                            HttpContext.Current.Session["ExpireDate"] = Convert.ToDateTime(ExpiryValue);
                        else
                            HttpContext.Current.Session["ExpireDate"] = Convert.ToDateTime("1900-01-01");
                    }
                    //End Set Expire Date 
                }
                //====End Decoding=======================================  
            }
            if (CallBy == "GetAnyNodeValue")
            {
                if (Version == 1)
                {
                    string Result = "";
                    //=== start Retrieve Parameters Values==============
                    string[] EDParam = new string[] { null };
                    if (WhichCall.Split('~')[1].Contains(","))
                        EDParam = WhichCall.Split('~')[1].ToString().Split(',');
                    else
                        EDParam[0] = WhichCall.Split('~')[1].ToString();
                    //EDParam ={ "//Name" };//{ "//ServerName" };//{ "//GlobalExpiry" };
                    //EDParam ={ "//cName[@Value='NAKAMICHI SECURITIES LIMITED']/@Value",//Segments[@Value='BSE-CM']/@Value", "//Expiry" };

                    string[] EDValue = new string[] { null };
                    if (WhichCall.Split('~')[2].Contains(","))
                        EDValue = WhichCall.Split('~')[2].ToString().Split(',');
                    else
                        EDValue[0] = WhichCall.Split('~')[2].ToString();
                    //=== End Retrieve Parameters Values==============

                    //==Get Content from Path
                    StreamReader decodeFile = new StreamReader(AnyValue);
                    StrDecodeXmlContent = decodeFile.ReadToEnd();
                    decodeFile.Close();
                    //===Content ReFormatting and get Expiry Date===========
                    try
                    {
                        StrDecodeXmlContent = Encoding.Unicode.GetString(Convert.FromBase64String(StrDecodeXmlContent));
                        decodeFile = null;

                        GenericXML oGenericXML = new GenericXML();
                        Result = oGenericXML.Decrypt_License(GenericXML.XWhichMethod.RecordExist, StrDecodeXmlContent, EDParam, EDValue);
                        if (Result.Contains("\r"))
                            Result = Result.Replace("\r", "");
                        if (Result.Contains("\n"))
                            Result = Result.Replace("\n", "");
                        if (Result.Contains("\t"))
                            Result = Result.Replace("\t", "");
                    }
                    catch (Exception ex)
                    {
                        Result = "License File Corrupted!!! Please Contact Your Administrator.";
                    }
                    return Result;
                }
            }
            return null;
        }
        //======================================End Expiry Module==========================
        #endregion


        //public void PageInitializer(global::GenericMethod.WhichCall globalGenericMethodWhichCall, string[] PageSession)
        //{
        //    throw new NotImplementedException();
        //}
    }
}