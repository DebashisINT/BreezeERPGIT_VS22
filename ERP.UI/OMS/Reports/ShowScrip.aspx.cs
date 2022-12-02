using System;
using System.Data;
using System.Configuration;

namespace ERP.OMS.Reports
{
    public partial class Reports_ShowScrip : System.Web.UI.Page
    {
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        BusinessLogicLayer.Converter oconverter = new BusinessLogicLayer.Converter();
        protected void Page_Load(object sender, EventArgs e)
        {
            ShowObligation();
        }
        public void ShowObligation()
        {
            //string SegMentName = Request.QueryString["SegMentName"].ToString().Trim();
            //string TranID = Request.QueryString["TranID"].ToString().Trim();
            //string Bill = Request.QueryString["Bill"].ToString().Trim();


            string TradeDate = Request.QueryString["TradeDate"].ToString().Trim();
            string SetN = Request.QueryString["SetN"].ToString().Trim();
            string SetType = Request.QueryString["SetType"].ToString().Trim();
            string MainID = Request.QueryString["MainID"].ToString().Trim();
            string SubID = Request.QueryString["SubID"].ToString().Trim();
            string CompID = Request.QueryString["CompID"].ToString().Trim();
            string SegID = Request.QueryString["SegID"].ToString().Trim();
            string SegMentName = Request.QueryString["SegmentName"].ToString().Trim();
            string TranID = Request.QueryString["TranID"].ToString().Trim();

            DateTime TranDate = Convert.ToDateTime(Request.QueryString["TradeDate"].ToString().Trim());
            String strHtmlAllClient = String.Empty;
            if (SegMentName == "NSE - CM" || SegMentName == "NSE - CM,BSE - CM" || SegMentName == "BSE - CM")
            {
                if (TranID == "XO")
                {
                    DataTable dtProduct = oDBEngine.GetDataTable("Trans_CMPosition", "(select ltrim(rtrim(isnull(Equity_TickerSymbol,'')))+(case when isnull(convert(varchar(12),Equity_EffectUntil,113),'')='' then '' else ' ['+convert(varchar(12),Equity_EffectUntil,113)+']' end)+' ['+ltrim(rtrim(isnull(Equity_Series,(isnull(equity_tickercode,'')))))+']'+ case when isnull(cast(Equity_StrikePrice as varchar),'')='' then '' else ' ['+cast(Equity_StrikePrice as varchar)+']' end from master_Equity where Equity_SeriesID=Trans_CMPosition.CMPosition_ProductSeriesID) as CMPosition_ProductSeriesID,case when CMPosition_SqrOffQty=0 then null else CMPosition_SqrOffQty end as CMPosition_SqrOffQty,case when CMPosition_SqrOffPL=0 then null else CMPosition_SqrOffPL end as CMPosition_SqrOffPL,case when CMPosition_DeliveryBuyQty=0 then null else CMPosition_DeliveryBuyQty end as CMPosition_DeliveryBuyQty,case when CMPosition_DeliveryBuyValue=0 then null else CMPosition_DeliveryBuyValue end as CMPosition_DeliveryBuyValue,case when CMPosition_DeliverySellQty=0 then null else CMPosition_DeliverySellQty end as CMPosition_DeliverySellQty,case when CMPosition_DeliverySellValue=0 then null else CMPosition_DeliverySellValue end as CMPosition_DeliverySellValue,case when CMPosition_NetObligation=0 then null else CMPosition_NetObligation end as CMPosition_NetObligation,CMPosition_SettlementNumber+CMPosition_SettlementType as SettnumType", " CMPOSITION_FINYEAR='" + Session["LastFinYear"].ToString() + "' AND CMPOSITION_COMPANYID='" + Session["LastCompany"].ToString() + "'  AND CMPOSITION_SEGMENTID in (" + SegID + ") AND CMPOSITION_CUSTOMERID ='" + SubID + "' AND CMPOSITION_SETTLEMENTNUMBER ='" + SetN + "' AND CMPOSITION_SETTLEMENTTYPE ='" + SetType + "' ");

                    decimal TotalVal = 0;
                    strHtmlAllClient = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + ">";
                    strHtmlAllClient += "<tr style=\"background-color: #DBEEF3;\">";
                    strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Scrip</td>";
                    strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Delv.Buy Qty</td>";
                    strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Delv.Buy Value</td>";
                    strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Delv.Sell Qty</td>";
                    strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Delv.Sell Value</td>";
                    strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Sqr Qty</td>";
                    strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Sqr P/L</td>";
                    strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Total</td>";
                    strHtmlAllClient += "</tr>";
                    for (int j = 0; j < dtProduct.Rows.Count; j++)
                    {
                        strHtmlAllClient += "<tr style=\"background-color:white;text-align:center\">";
                        strHtmlAllClient += "<td align=\"left\" style=\"font-size:xx-small\">" + dtProduct.Rows[j]["CMPosition_ProductSeriesID"] + "</td>";
                        if (dtProduct.Rows[j]["CMPosition_DeliveryBuyQty"] != DBNull.Value)
                        {
                            strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(Convert.ToDecimal(dtProduct.Rows[j]["CMPosition_DeliveryBuyQty"])) + "</td>";
                        }
                        else
                        {
                            strHtmlAllClient += "<td>&nbsp;</td>";
                        }
                        if (dtProduct.Rows[j]["CMPosition_DeliveryBuyValue"] != DBNull.Value)
                        {
                            strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtProduct.Rows[j]["CMPosition_DeliveryBuyValue"])) + "</td>";
                        }
                        else
                        {
                            strHtmlAllClient += "<td>&nbsp;</td>";
                        }
                        if (dtProduct.Rows[j]["CMPosition_DeliverySellQty"] != DBNull.Value)
                        {
                            strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(Convert.ToDecimal(dtProduct.Rows[j]["CMPosition_DeliverySellQty"])) + "</td>";
                        }
                        else
                        {
                            strHtmlAllClient += "<td>&nbsp;</td>";
                        }
                        if (dtProduct.Rows[j]["CMPosition_DeliverySellValue"] != DBNull.Value)
                        {
                            strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtProduct.Rows[j]["CMPosition_DeliverySellValue"])) + "</td>";
                        }
                        else
                        {
                            strHtmlAllClient += "<td>&nbsp;</td>";
                        }
                        if (dtProduct.Rows[j]["CMPosition_SqrOffQty"] != DBNull.Value)
                        {
                            strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(Convert.ToDecimal(dtProduct.Rows[j]["CMPosition_SqrOffQty"])) + "</td>";
                        }
                        else
                        {
                            strHtmlAllClient += "<td>&nbsp;</td>";
                        }
                        if (dtProduct.Rows[j]["CMPosition_SqrOffPL"] != DBNull.Value)
                        {
                            strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtProduct.Rows[j]["CMPosition_SqrOffPL"])) + "</td>";
                        }
                        else
                        {
                            strHtmlAllClient += "<td>&nbsp;</td>";
                        }
                        if (dtProduct.Rows[j]["CMPosition_NetObligation"] != DBNull.Value)
                        {
                            strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtProduct.Rows[j]["CMPosition_NetObligation"])) + "</td></tr>";
                            TotalVal += Convert.ToDecimal(dtProduct.Rows[j]["CMPosition_NetObligation"]);
                        }
                        else
                        {
                            strHtmlAllClient += "<td>&nbsp;</td></tr>";
                        }
                    }
                    strHtmlAllClient += "<tr style=\"background-color:white;text-align:center\">";
                    strHtmlAllClient += "<td align=\"left\" style=\"font-size:xx-small\"><b>Total :</b></td>";
                    strHtmlAllClient += "<td align=\"right\" colspan=\"7\" style=\"font-size:xx-small\"><b>" + oconverter.getFormattedvaluewithoriginalsign(TotalVal) + "</b></td></tr>";
                    strHtmlAllClient += "</table>";
                }
            }
            else if (SegMentName == "NSE - FO")
            {
                if (TranID == "XO")
                {
                    DataTable dtProduct = oDBEngine.GetDataTable("trans_foposition", "(select ltrim(rtrim(isnull(Equity_TickerSymbol,'')))+(case when isnull(convert(varchar(12),Equity_EffectUntil,113),'')='' then '' else ' ['+convert(varchar(12),Equity_EffectUntil,113)+']' end)+' ['+ltrim(rtrim(isnull(Equity_Series,'')))+']'+ case when isnull(CONVERT(VARCHAR,CONVERT(MONEY,cast(Equity_StrikePrice as varchar)),1),'')='' then '' when Equity_StrikePrice=0 then '' else ' ['+CONVERT(VARCHAR,CONVERT(MONEY,cast(Equity_StrikePrice as varchar)),1)+']' end from master_Equity where Equity_SeriesID=trans_foposition.FOPosition_ProductSeriesID) as FOPosition_ProductSeriesID,case when isnull(foposition_BFPriceUnits,0)=0 then null else foposition_BFPriceUnits end as foposition_BFPriceUnits,case when isnull(foposition_openprice,0)=0 then null else foposition_openprice end as foposition_openprice,case when isnull(foposition_buyLots,0)=0 then null else foposition_buyLots end as foposition_buyPriceUnits,case when isnull(foposition_BuyValue,0)=0 then foposition_BuyValue else foposition_BuyValue end as foposition_BuyValue,case when isnull(foposition_BuyAverage,0)=0 then null else foposition_BuyAverage end as foposition_BuyAverage,case when isnull(foposition_sellLots,0)=0 then null else foposition_sellLots end as foposition_sellPriceUnits,case when isnull(foposition_SellValue,0)=0 then null else foposition_SellValue end as foposition_SellValue,case when isnull(foposition_SellAverage,0)=0 then null else foposition_SellAverage end as foposition_SellAverage,case when isnull(foposition_PostExcAsnDlvLongPriceUnits,0)=0 then foposition_PostExcAsnDlvShortPriceUnits else (-1)*foposition_PostExcAsnDlvLongPriceUnits end as CFQty,case when foposition_PostExcAsnDlvLongValue=0 then foposition_PostExcAsnDlvShortValue else foposition_PostExcAsnDlvLongValue end as CFPrice,foposition_MTMPL,(select DailyStat_SettlementPrice from Trans_dailyStatistics where DailyStat_ProductSeriesID=trans_foposition.FOPosition_ProductSeriesID and DailyStat_ExchangeSegmentID=" + Session["ExchangeSegmentID"].ToString() + " and cast(DATEADD(dd, 0, DATEDIFF(dd, 0,DailyStat_DateTime)) as datetime) =cast(DATEADD(dd, 0, DATEDIFF(dd, 0, '" + TranDate + "')) as datetime)) as SettPrice", " FOPOSITION_FINYEAR='" + Session["LastFinYear"].ToString() + "'  AND FOPOSITION_COMPANYID='" + Session["LastCompany"].ToString() + "'  AND FOPOSITION_SEGMENTID in (" + SegID + ") AND FOPOSITION_CUSTOMEREXCHANGEID='" + SubID + "'  and  FOPosition_SettlementNumber='" + SetN + "' and FOPosition_SettlementType ='" + SetType + "'  AND FOPosition_Date='" + TradeDate + "'  and  foposition_productseriesid in(select equity_seriesid from master_equity where equity_foidentifier like 'FUT%' and equity_effectuntil<>'" + TranDate + "')");
                    decimal TotalVal = 0;
                    strHtmlAllClient = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + ">";
                    strHtmlAllClient += "<tr style=\"background-color: #DBEEF3;\">";
                    strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Scrip</td>";
                    strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">B/F Qty</td>";
                    strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Price</td>";
                    strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Buy Qty</td>";
                    strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Value</td>";
                    strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Avg</td>";
                    strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Sell Qty</td>";
                    strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Value</td>";
                    strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Avg</td>";
                    strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">C/F Qty</td>";
                    strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Price</td>";
                    strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">MTM</td>";
                    strHtmlAllClient += "</tr>";
                    for (int j = 0; j < dtProduct.Rows.Count; j++)
                    {
                        strHtmlAllClient += "<tr style=\"background-color:white;text-align:center\">";
                        strHtmlAllClient += "<td align=\"left\" style=\"font-size:xx-small\">" + dtProduct.Rows[j]["FOPosition_ProductSeriesID"] + "</td>";
                        if (dtProduct.Rows[j]["foposition_BFPriceUnits"] != DBNull.Value)
                        {
                            strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(Convert.ToDecimal(dtProduct.Rows[j]["foposition_BFPriceUnits"])) + "</td>";
                        }
                        else
                        {
                            strHtmlAllClient += "<td>&nbsp;</td>";
                        }
                        if (dtProduct.Rows[j]["foposition_openprice"] != DBNull.Value)
                        {
                            strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtProduct.Rows[j]["foposition_openprice"])) + "</td>";
                        }
                        else
                        {
                            strHtmlAllClient += "<td>&nbsp;</td>";
                        }
                        if (dtProduct.Rows[j]["foposition_buyPriceUnits"] != DBNull.Value)
                        {
                            strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(Convert.ToDecimal(dtProduct.Rows[j]["foposition_buyPriceUnits"])) + "</td>";
                        }
                        else
                        {
                            strHtmlAllClient += "<td>&nbsp;</td>";
                        }
                        if (dtProduct.Rows[j]["foposition_BuyValue"] != DBNull.Value)
                        {
                            strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtProduct.Rows[j]["foposition_BuyValue"])) + "</td>";
                        }
                        else
                        {
                            strHtmlAllClient += "<td>&nbsp;</td>";
                        }
                        if (dtProduct.Rows[j]["foposition_BuyAverage"] != DBNull.Value)
                        {
                            strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtProduct.Rows[j]["foposition_BuyValue"]) / Convert.ToDecimal(dtProduct.Rows[j]["foposition_buyPriceUnits"])) + "</td>";
                        }
                        else
                        {
                            strHtmlAllClient += "<td>&nbsp;</td>";
                        }
                        if (dtProduct.Rows[j]["foposition_sellPriceUnits"] != DBNull.Value)
                        {
                            strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(Convert.ToDecimal(dtProduct.Rows[j]["foposition_sellPriceUnits"])) + "</td>";
                        }
                        else
                        {
                            strHtmlAllClient += "<td>&nbsp;</td>";
                        }
                        if (dtProduct.Rows[j]["foposition_SellValue"] != DBNull.Value)
                        {
                            strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtProduct.Rows[j]["foposition_SellValue"])) + "</td>";
                        }
                        else
                        {
                            strHtmlAllClient += "<td>&nbsp;</td>";
                        }
                        if (dtProduct.Rows[j]["foposition_SellAverage"] != DBNull.Value)
                        {
                            strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtProduct.Rows[j]["foposition_SellValue"]) / Convert.ToDecimal(dtProduct.Rows[j]["foposition_sellPriceUnits"])) + "</td>";
                        }
                        else
                        {
                            strHtmlAllClient += "<td>&nbsp;</td>";
                        }
                        if (dtProduct.Rows[j]["CFQty"] != DBNull.Value)
                        {
                            strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(Convert.ToDecimal(dtProduct.Rows[j]["CFQty"])) + "</td>";
                        }
                        else
                        {
                            strHtmlAllClient += "<td>&nbsp;</td>";
                        }
                        if (dtProduct.Rows[j]["SettPrice"] != DBNull.Value)
                        {
                            strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtProduct.Rows[j]["SettPrice"])) + "</td>";
                        }
                        else
                        {
                            strHtmlAllClient += "<td>&nbsp;</td>";
                        }
                        if (dtProduct.Rows[j]["foposition_MTMPL"] != DBNull.Value)
                        {
                            strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtProduct.Rows[j]["foposition_MTMPL"])) + "</td></tr>";
                            TotalVal += Convert.ToDecimal(dtProduct.Rows[j]["foposition_MTMPL"]);
                        }
                        else
                        {
                            strHtmlAllClient += "<td>&nbsp;</td></tr>";
                        }
                    }
                    strHtmlAllClient += "<tr style=\"background-color:white;text-align:center\">";
                    strHtmlAllClient += "<td align=\"left\" style=\"font-size:xx-small\"><b>Total :</b></td>";
                    strHtmlAllClient += "<td align=\"right\" colspan=\"11\" style=\"font-size:xx-small\"><b>" + oconverter.getFormattedvaluewithoriginalsign(TotalVal) + "</b></td></tr>";
                    strHtmlAllClient += "</table>";
                }
                if (TranID == "XP")
                {
                    DataTable dtProduct = oDBEngine.GetDataTable("trans_foposition", "(select ltrim(rtrim(isnull(Equity_TickerSymbol,'')))+(case when isnull(convert(varchar(12),Equity_EffectUntil,113),'')='' then '' else ' ['+convert(varchar(12),Equity_EffectUntil,113)+']' end)+' ['+ltrim(rtrim(isnull(Equity_Series,'')))+']'+ case when isnull(CONVERT(VARCHAR,CONVERT(MONEY,cast(Equity_StrikePrice as varchar)),1),'')='' then '' when Equity_StrikePrice=0 then '' else ' ['+CONVERT(VARCHAR,CONVERT(MONEY,cast(Equity_StrikePrice as varchar)),1)+']' end from master_Equity where Equity_SeriesID=trans_foposition.FOPosition_ProductSeriesID) as FOPosition_ProductSeriesID,case when isnull(foposition_BuyLots,0)=0 then null else foposition_BuyLots end as foposition_BuyLots,case when isnull(foposition_BuyValue,0)=0 then null else foposition_BuyValue end as foposition_BuyValue,case when isnull(foposition_BuyAverage,0)=0 then null else foposition_BuyAverage end as foposition_BuyAverage,case when isnull(foposition_SellLots,0)=0 then null else foposition_SellLots end as foposition_SellLots,case when isnull(foposition_SellValue,0)=0 then null else foposition_SellValue end as foposition_SellValue,case when isnull(foposition_SellAverage,0)=0 then null else foposition_SellAverage end as foposition_SellAverage,case when isnull(foposition_NetPremium,0)=0 then null else foposition_NetPremium end as foposition_NetPremium ", " FOPOSITION_FINYEAR='" + Session["LastFinYear"].ToString() + "'  AND FOPOSITION_COMPANYID='" + Session["LastCompany"].ToString() + "'  AND CMPOSITION_SEGMENTID in (" + SegID + ") AND FOPOSITION_CUSTOMEREXCHANGEID='" + SubID + "'  and  FOPosition_SettlementNumber='" + SetN + "' and FOPosition_SettlementType ='" + SetType + "' and AND FOPosition_Date='" + TradeDate + "' and  foposition_NetPremium is not null");
                    decimal TotalVal = 0;
                    strHtmlAllClient = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + ">";
                    strHtmlAllClient += "<tr style=\"background-color: #DBEEF3;\">";
                    strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Scrip</td>";
                    strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Buy Qty</td>";
                    strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Value</td>";
                    strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Avg</td>";
                    strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Sell Qty</td>";
                    strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Value</td>";
                    strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Avg</td>";
                    strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Net Premium</td>";
                    strHtmlAllClient += "</tr>";
                    for (int j = 0; j < dtProduct.Rows.Count; j++)
                    {
                        strHtmlAllClient += "<tr style=\"background-color:white;text-align:center\">";
                        strHtmlAllClient += "<td align=\"left\" style=\"font-size:xx-small\">" + dtProduct.Rows[j]["FOPosition_ProductSeriesID"] + "</td>";
                        if (dtProduct.Rows[j]["foposition_BuyLots"] != DBNull.Value)
                        {
                            strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(Convert.ToDecimal(dtProduct.Rows[j]["foposition_BuyLots"])) + "</td>";
                        }
                        else
                        {
                            strHtmlAllClient += "<td>&nbsp;</td>";
                        }
                        if (dtProduct.Rows[j]["foposition_BuyValue"] != DBNull.Value)
                        {
                            strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtProduct.Rows[j]["foposition_BuyValue"])) + "</td>";
                        }
                        else
                        {
                            strHtmlAllClient += "<td>&nbsp;</td>";
                        }
                        if (dtProduct.Rows[j]["foposition_BuyAverage"] != DBNull.Value)
                        {
                            strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtProduct.Rows[j]["foposition_BuyValue"]) / Convert.ToDecimal(dtProduct.Rows[j]["foposition_BuyLots"])) + "</td>";
                        }
                        else
                        {
                            strHtmlAllClient += "<td>&nbsp;</td>";
                        }
                        if (dtProduct.Rows[j]["foposition_SellLots"] != DBNull.Value)
                        {
                            strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(Convert.ToDecimal(dtProduct.Rows[j]["foposition_SellLots"])) + "</td>";
                        }
                        else
                        {
                            strHtmlAllClient += "<td>&nbsp;</td>";
                        }
                        if (dtProduct.Rows[j]["foposition_SellValue"] != DBNull.Value)
                        {
                            strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtProduct.Rows[j]["foposition_SellValue"])) + "</td>";
                        }
                        else
                        {
                            strHtmlAllClient += "<td>&nbsp;</td>";
                        }
                        if (dtProduct.Rows[j]["foposition_SellAverage"] != DBNull.Value)
                        {
                            strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtProduct.Rows[j]["foposition_SellValue"]) / Convert.ToDecimal(dtProduct.Rows[j]["foposition_SellLots"])) + "</td>";
                        }
                        else
                        {
                            strHtmlAllClient += "<td>&nbsp;</td>";
                        }
                        if (dtProduct.Rows[j]["foposition_NetPremium"] != DBNull.Value)
                        {
                            strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtProduct.Rows[j]["foposition_NetPremium"])) + "</td></tr>";
                            TotalVal += Convert.ToDecimal(dtProduct.Rows[j]["foposition_NetPremium"]);
                        }
                        else
                        {
                            strHtmlAllClient += "<td>&nbsp;</td></tr>";
                        }
                    }
                    strHtmlAllClient += "<tr style=\"background-color:white;text-align:center\">";
                    strHtmlAllClient += "<td align=\"left\" style=\"font-size:xx-small\"><b>Total :</b></td>";
                    strHtmlAllClient += "<td align=\"right\" colspan=\"11\" style=\"font-size:xx-small\"><b>" + oconverter.getFormattedvaluewithoriginalsign(TotalVal) + "</b></td></tr>";
                    strHtmlAllClient += "</table>";
                }
                if (TranID == "XZ")
                {
                    DataTable dtProduct = oDBEngine.GetDataTable("trans_foposition", "(select ltrim(rtrim(isnull(Equity_TickerSymbol,'')))+(case when isnull(convert(varchar(12),Equity_EffectUntil,113),'')='' then '' else ' ['+convert(varchar(12),Equity_EffectUntil,113)+']' end)+' ['+ltrim(rtrim(isnull(Equity_Series,'')))+']'+ case when isnull(CONVERT(VARCHAR,CONVERT(MONEY,cast(Equity_StrikePrice as varchar)),1),'')='' then '' when Equity_StrikePrice=0 then '' else ' ['+CONVERT(VARCHAR,CONVERT(MONEY,cast(Equity_StrikePrice as varchar)),1)+']' end from master_Equity where Equity_SeriesID=trans_foposition.FOPosition_ProductSeriesID) as FOPosition_ProductSeriesID,case when foposition_ExcAsnDlvLots<0 then 'Exercise' else 'Assigned' end as SettType,case when isnull(foposition_ExcAsnDlvLots,0)=0 then null else abs(foposition_ExcAsnDlvLots) end as foposition_ExcAsnDlvLots,case when isnull(foposition_SettlementPrice,0)=0 then null else abs(foposition_SettlementPrice) end as foposition_SettlementPrice,case when isnull(foposition_ExcAsnDlvMarkedValue,0)=0 then null else foposition_ExcAsnDlvMarkedValue end as foposition_ExcAsnDlvMarkedValue ", " FOPOSITION_FINYEAR='" + Session["LastFinYear"].ToString() + "'  AND FOPOSITION_COMPANYID='" + Session["LastCompany"].ToString() + "'  AND CMPOSITION_SEGMENTID in (" + SegID + ") AND FOPOSITION_CUSTOMEREXCHANGEID='" + SubID + "'  and  FOPosition_SettlementNumber='" + SetN + "' and FOPosition_SettlementType ='" + SetType + "' and AND FOPosition_Date='" + TradeDate + "' and  foposition_ExcAsnDlvMarkedValue is not null");
                    decimal TotalVal = 0;
                    strHtmlAllClient = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + ">";
                    strHtmlAllClient += "<tr style=\"background-color: #DBEEF3;\">";
                    strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Scrip</td>";
                    strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Sett.Type</td>";
                    strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Qty</td>";
                    strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Set.Price</td>";
                    strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Net Amount</td>";
                    strHtmlAllClient += "</tr>";
                    for (int j = 0; j < dtProduct.Rows.Count; j++)
                    {
                        strHtmlAllClient += "<tr style=\"background-color:white;text-align:center\">";
                        strHtmlAllClient += "<td align=\"left\" style=\"font-size:xx-small\">" + dtProduct.Rows[j]["FOPosition_ProductSeriesID"] + "</td>";
                        strHtmlAllClient += "<td align=\"left\" style=\"font-size:xx-small\">" + dtProduct.Rows[j]["SettType"] + "</td>";
                        if (dtProduct.Rows[j]["foposition_ExcAsnDlvLots"] != DBNull.Value)
                        {
                            strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(Convert.ToDecimal(dtProduct.Rows[j]["foposition_ExcAsnDlvLots"])) + "</td>";
                        }
                        else
                        {
                            strHtmlAllClient += "<td>&nbsp;</td>";
                        }
                        if (dtProduct.Rows[j]["foposition_SettlementPrice"] != DBNull.Value)
                        {
                            strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtProduct.Rows[j]["foposition_SettlementPrice"])) + "</td>";
                        }
                        else
                        {
                            strHtmlAllClient += "<td>&nbsp;</td>";
                        }
                        if (dtProduct.Rows[j]["foposition_ExcAsnDlvMarkedValue"] != DBNull.Value)
                        {
                            strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtProduct.Rows[j]["foposition_ExcAsnDlvMarkedValue"])) + "</td></tr>";
                            TotalVal += Convert.ToDecimal(dtProduct.Rows[j]["foposition_ExcAsnDlvMarkedValue"]);
                        }
                        else
                        {
                            strHtmlAllClient += "<td>&nbsp;</td></tr>";
                        }
                    }
                    strHtmlAllClient += "<tr style=\"background-color:white;text-align:center\">";
                    strHtmlAllClient += "<td align=\"left\" style=\"font-size:xx-small\"><b>Total :</b></td>";
                    strHtmlAllClient += "<td align=\"right\" colspan=\"11\" style=\"font-size:xx-small\"><b>" + oconverter.getFormattedvaluewithoriginalsign(TotalVal) + "</b></td></tr>";
                    strHtmlAllClient += "</table>";
                }
                if (TranID == "XX")
                {
                    DataTable dtProduct = oDBEngine.GetDataTable("trans_foposition", "(select ltrim(rtrim(isnull(Equity_TickerSymbol,'')))+(case when isnull(convert(varchar(12),Equity_EffectUntil,113),'')='' then '' else ' ['+convert(varchar(12),Equity_EffectUntil,113)+']' end)+' ['+ltrim(rtrim(isnull(Equity_Series,'')))+']'+ case when isnull(CONVERT(VARCHAR,CONVERT(MONEY,cast(Equity_StrikePrice as varchar)),1),'')='' then '' when Equity_StrikePrice=0 then '' else ' ['+CONVERT(VARCHAR,CONVERT(MONEY,cast(Equity_StrikePrice as varchar)),1)+']' end from master_Equity where Equity_SeriesID=trans_foposition.FOPosition_ProductSeriesID) as FOPosition_ProductSeriesID,case when isnull(foposition_BFPriceUnits,0)=0 then null else foposition_BFPriceUnits end as foposition_BFPriceUnits,case when isnull(foposition_openprice,0)=0 then null else foposition_openprice end as foposition_openprice,case when isnull(foposition_buyLots,0)=0 then null else foposition_buyLots end as foposition_buyPriceUnits,case when isnull(foposition_BuyValue,0)=0 then foposition_BuyValue else foposition_BuyValue end as foposition_BuyValue,case when isnull(foposition_BuyAverage,0)=0 then null else foposition_BuyAverage end as foposition_BuyAverage,case when isnull(foposition_sellLots,0)=0 then null else foposition_sellLots end as foposition_sellPriceUnits,case when isnull(foposition_SellValue,0)=0 then null else foposition_SellValue end as foposition_SellValue,case when isnull(foposition_SellAverage,0)=0 then null else foposition_SellAverage end as foposition_SellAverage,case when isnull(foposition_PostExcAsnDlvLongPriceUnits,0)=0 then foposition_PostExcAsnDlvShortPriceUnits else (-1)*foposition_PostExcAsnDlvLongPriceUnits end as CFQty,case when foposition_PostExcAsnDlvLongValue=0 then foposition_PostExcAsnDlvShortValue else foposition_PostExcAsnDlvLongValue end as CFPrice,foposition_FutureFinalSettlement,(select DailyStat_SettlementPrice from Trans_dailyStatistics where DailyStat_ProductSeriesID=trans_foposition.FOPosition_ProductSeriesID and DailyStat_ExchangeSegmentID=" + Session["ExchangeSegmentID"].ToString() + " and cast(DATEADD(dd, 0, DATEDIFF(dd, 0,DailyStat_DateTime)) as datetime) =cast(DATEADD(dd, 0, DATEDIFF(dd, 0, '" + TranDate + "')) as datetime)) as SettPrice", " FOPOSITION_FINYEAR='" + Session["LastFinYear"].ToString() + "'  AND FOPOSITION_COMPANYID='" + Session["LastCompany"].ToString() + "'  AND CMPOSITION_SEGMENTID in (" + SegID + ") AND FOPOSITION_CUSTOMEREXCHANGEID='" + SubID + "'  and  FOPosition_SettlementNumber='" + SetN + "' and FOPosition_SettlementType ='" + SetType + "' and AND FOPosition_Date='" + TradeDate + "' and foposition_productseriesid in(select equity_seriesid from master_equity where equity_foidentifier like 'FUT%' and equity_effectuntil='" + TranDate + "')");
                    decimal TotalVal = 0;
                    strHtmlAllClient = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + ">";
                    strHtmlAllClient += "<tr style=\"background-color: #DBEEF3;\">";
                    strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Scrip</td>";
                    strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">B/F Qty</td>";
                    strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Price</td>";
                    strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Buy Qty</td>";
                    strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Value</td>";
                    strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Avg</td>";
                    strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Sell Qty</td>";
                    strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Value</td>";
                    strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Avg</td>";
                    strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Exp Qty</td>";
                    strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Price</td>";
                    strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Future Final Sett</td>";
                    strHtmlAllClient += "</tr>";
                    for (int j = 0; j < dtProduct.Rows.Count; j++)
                    {
                        strHtmlAllClient += "<tr style=\"background-color:white;text-align:center\">";
                        strHtmlAllClient += "<td align=\"left\" style=\"font-size:xx-small\">" + dtProduct.Rows[j]["FOPosition_ProductSeriesID"] + "</td>";
                        if (dtProduct.Rows[j]["foposition_BFPriceUnits"] != DBNull.Value)
                        {
                            strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(Convert.ToDecimal(dtProduct.Rows[j]["foposition_BFPriceUnits"])) + "</td>";
                        }
                        else
                        {
                            strHtmlAllClient += "<td>&nbsp;</td>";
                        }
                        if (dtProduct.Rows[j]["foposition_openprice"] != DBNull.Value)
                        {
                            strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtProduct.Rows[j]["foposition_openprice"])) + "</td>";
                        }
                        else
                        {
                            strHtmlAllClient += "<td>&nbsp;</td>";
                        }
                        if (dtProduct.Rows[j]["foposition_buyPriceUnits"] != DBNull.Value)
                        {
                            strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(Convert.ToDecimal(dtProduct.Rows[j]["foposition_buyPriceUnits"])) + "</td>";
                        }
                        else
                        {
                            strHtmlAllClient += "<td>&nbsp;</td>";
                        }
                        if (dtProduct.Rows[j]["foposition_BuyValue"] != DBNull.Value)
                        {
                            strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtProduct.Rows[j]["foposition_BuyValue"])) + "</td>";
                        }
                        else
                        {
                            strHtmlAllClient += "<td>&nbsp;</td>";
                        }
                        if (dtProduct.Rows[j]["foposition_BuyAverage"] != DBNull.Value)
                        {
                            strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtProduct.Rows[j]["foposition_BuyValue"]) / Convert.ToDecimal(dtProduct.Rows[j]["foposition_buyPriceUnits"])) + "</td>";
                        }
                        else
                        {
                            strHtmlAllClient += "<td>&nbsp;</td>";
                        }
                        if (dtProduct.Rows[j]["foposition_sellPriceUnits"] != DBNull.Value)
                        {
                            strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(Convert.ToDecimal(dtProduct.Rows[j]["foposition_sellPriceUnits"])) + "</td>";
                        }
                        else
                        {
                            strHtmlAllClient += "<td>&nbsp;</td>";
                        }
                        if (dtProduct.Rows[j]["foposition_SellValue"] != DBNull.Value)
                        {
                            strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtProduct.Rows[j]["foposition_SellValue"])) + "</td>";
                        }
                        else
                        {
                            strHtmlAllClient += "<td>&nbsp;</td>";
                        }
                        if (dtProduct.Rows[j]["foposition_SellAverage"] != DBNull.Value)
                        {
                            strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtProduct.Rows[j]["foposition_SellValue"]) / Convert.ToDecimal(dtProduct.Rows[j]["foposition_sellPriceUnits"])) + "</td>";
                        }
                        else
                        {
                            strHtmlAllClient += "<td>&nbsp;</td>";
                        }
                        if (dtProduct.Rows[j]["CFQty"] != DBNull.Value)
                        {
                            strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(Convert.ToDecimal(dtProduct.Rows[j]["CFQty"])) + "</td>";
                        }
                        else
                        {
                            strHtmlAllClient += "<td>&nbsp;</td>";
                        }
                        if (dtProduct.Rows[j]["SettPrice"] != DBNull.Value)
                        {
                            strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtProduct.Rows[j]["SettPrice"])) + "</td>";
                        }
                        else
                        {
                            strHtmlAllClient += "<td>&nbsp;</td>";
                        }
                        if (dtProduct.Rows[j]["foposition_FutureFinalSettlement"] != DBNull.Value)
                        {
                            strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtProduct.Rows[j]["foposition_FutureFinalSettlement"])) + "</td></tr>";
                            TotalVal += Convert.ToDecimal(dtProduct.Rows[j]["foposition_FutureFinalSettlement"]);
                        }
                        else
                        {
                            strHtmlAllClient += "<td>&nbsp;</td></tr>";
                        }
                    }
                    strHtmlAllClient += "<tr style=\"background-color:white;text-align:center\">";
                    strHtmlAllClient += "<td align=\"left\" style=\"font-size:xx-small\"><b>Total :</b></td>";
                    strHtmlAllClient += "<td align=\"right\" colspan=\"11\" style=\"font-size:xx-small\"><b>" + oconverter.getFormattedvaluewithoriginalsign(TotalVal) + "</b></td></tr>";
                    strHtmlAllClient += "</table>";
                }
            }
            else if (SegMentName == "ICEX - COMM" || SegMentName == "MCX - COMM")
            {
                if (TranID == "XC")
                {
                    DataTable dtProduct = oDBEngine.GetDataTable("Trans_comPosition", "(select ltrim(rtrim(isnull(Commodity_TickerSymbol,'')))+' ['+convert(varchar(12),Commodity_ExpiryDate,113)+']' from master_commodity where commodity_ProductSeriesID=trans_composition.comPosition_ProductSeriesID) as comPosition_ProductSeriesID,case when isnull(comPosition_BFPriceUnits,0)=0 then null else comPosition_BFPriceUnits end as comPosition_BFPriceUnits,case when isnull(comPosition_openprice,0)=0 then null else comPosition_openprice end as comPosition_openprice,case when isnull(comPosition_buyPriceUnits,0)=0 then null else comPosition_buyPriceUnits end as comPosition_buyPriceUnits,case when isnull(comPosition_BuyValue,0)=0 then comPosition_BuyValue else comPosition_BuyValue end as comPosition_BuyValue,case when isnull(comPosition_BuyAverage,0)=0 then null else comPosition_BuyAverage end as comPosition_BuyAverage,case when isnull(comPosition_sellPriceUnits,0)=0 then null else comPosition_sellPriceUnits end as comPosition_sellPriceUnits,case when isnull(comPosition_SellValue,0)=0 then null else comPosition_SellValue end as comPosition_SellValue,case when isnull(comPosition_SellAverage,0)=0 then null else comPosition_SellAverage end as comPosition_SellAverage,case when isnull(comPosition_PostExcAsnDlvLongPriceUnits,0)=0 then comPosition_PostExcAsnDlvShortPriceUnits else comPosition_PostExcAsnDlvLongPriceUnits end as CFQty,case when isnull(comPosition_PostExcAsnDlvLongValue,0)=0 then case when isnull(comPosition_PostExcAsnDlvShortValue,0)/abs(isnull(comPosition_PostExcAsnDlvShortPriceUnits,1))=0 then null else isnull(comPosition_PostExcAsnDlvShortValue,0)/abs(isnull(comPosition_PostExcAsnDlvShortPriceUnits,1)) end else case when isnull(comPosition_PostExcAsnDlvLongValue,0)/abs(isnull(comPosition_PostExcAsnDlvLongPriceUnits,1))=0 then null else isnull(comPosition_PostExcAsnDlvLongValue,0)/abs(isnull(comPosition_PostExcAsnDlvLongPriceUnits,1)) end end as CFPrice,comPosition_MTMPL", " COMPOSITION_FINYEAR='" + Session["LastFinYear"].ToString() + "' AND COMPOSITION_SEGMENTID  in (" + SegID + ") AND COMPOSITION_COMPANYID='" + Session["LastCompany"].ToString() + "' AND COMPOSITION_CUSTOMEREXCHANGEID='" + SubID + "' AND COMPOSITION_SettlementNumber='" + SetN + "' AND	COMPOSITION_SettlementType='" + SetType + "' AND COMPOSITION_Date='" + TradeDate + "'  ");
                    decimal TotalVal = 0;
                    strHtmlAllClient = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + ">";
                    strHtmlAllClient += "<tr style=\"background-color: #DBEEF3;\">";
                    strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Scrip</td>";
                    strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">B/F Qty</td>";
                    strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Price</td>";
                    strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Buy Qty</td>";
                    strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Value</td>";
                    strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Avg</td>";
                    strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Sell Qty</td>";
                    strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Value</td>";
                    strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Avg</td>";
                    strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">C/F Qty</td>";
                    strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Price</td>";
                    strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">MTM</td>";
                    strHtmlAllClient += "</tr>";
                    for (int j = 0; j < dtProduct.Rows.Count; j++)
                    {
                        strHtmlAllClient += "<tr style=\"background-color:white;text-align:center\">";
                        strHtmlAllClient += "<td align=\"left\" style=\"font-size:xx-small\">" + dtProduct.Rows[j]["comPosition_ProductSeriesID"] + "</td>";
                        if (dtProduct.Rows[j]["comPosition_BFPriceUnits"] != DBNull.Value)
                        {
                            strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(Convert.ToDecimal(dtProduct.Rows[j]["comPosition_BFPriceUnits"])) + "</td>";
                        }
                        else
                        {
                            strHtmlAllClient += "<td>&nbsp;</td>";
                        }
                        if (dtProduct.Rows[j]["comPosition_openprice"] != DBNull.Value)
                        {
                            strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtProduct.Rows[j]["comPosition_openprice"])) + "</td>";
                        }
                        else
                        {
                            strHtmlAllClient += "<td>&nbsp;</td>";
                        }
                        if (dtProduct.Rows[j]["comPosition_buyPriceUnits"] != DBNull.Value)
                        {
                            strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(Convert.ToDecimal(dtProduct.Rows[j]["comPosition_buyPriceUnits"])) + "</td>";
                        }
                        else
                        {
                            strHtmlAllClient += "<td>&nbsp;</td>";
                        }
                        if (dtProduct.Rows[j]["comPosition_BuyValue"] != DBNull.Value)
                        {
                            strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtProduct.Rows[j]["comPosition_BuyValue"])) + "</td>";
                        }
                        else
                        {
                            strHtmlAllClient += "<td>&nbsp;</td>";
                        }
                        if (dtProduct.Rows[j]["comPosition_BuyAverage"] != DBNull.Value)
                        {
                            strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtProduct.Rows[j]["comPosition_BuyAverage"])) + "</td>";
                        }
                        else
                        {
                            strHtmlAllClient += "<td>&nbsp;</td>";
                        }
                        if (dtProduct.Rows[j]["comPosition_sellPriceUnits"] != DBNull.Value)
                        {
                            strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(Convert.ToDecimal(dtProduct.Rows[j]["comPosition_sellPriceUnits"])) + "</td>";
                        }
                        else
                        {
                            strHtmlAllClient += "<td>&nbsp;</td>";
                        }
                        if (dtProduct.Rows[j]["comPosition_SellValue"] != DBNull.Value)
                        {
                            strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtProduct.Rows[j]["comPosition_SellValue"])) + "</td>";
                        }
                        else
                        {
                            strHtmlAllClient += "<td>&nbsp;</td>";
                        }
                        if (dtProduct.Rows[j]["comPosition_SellAverage"] != DBNull.Value)
                        {
                            strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtProduct.Rows[j]["comPosition_SellAverage"])) + "</td>";
                        }
                        else
                        {
                            strHtmlAllClient += "<td>&nbsp;</td>";
                        }
                        if (dtProduct.Rows[j]["CFQty"] != DBNull.Value)
                        {
                            strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(Convert.ToDecimal(dtProduct.Rows[j]["CFQty"])) + "</td>";
                        }
                        else
                        {
                            strHtmlAllClient += "<td>&nbsp;</td>";
                        }
                        if (dtProduct.Rows[j]["CFPrice"] != DBNull.Value)
                        {
                            strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtProduct.Rows[j]["CFPrice"])) + "</td>";
                        }
                        else
                        {
                            strHtmlAllClient += "<td>&nbsp;</td>";
                        }
                        if (dtProduct.Rows[j]["comPosition_MTMPL"] != DBNull.Value)
                        {
                            strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtProduct.Rows[j]["comPosition_MTMPL"])) + "</td></tr>";
                            TotalVal += Convert.ToDecimal(dtProduct.Rows[j]["comPosition_MTMPL"]);
                        }
                        else
                        {
                            strHtmlAllClient += "<td>&nbsp;</td></tr>";
                        }
                    }
                    strHtmlAllClient += "<tr style=\"background-color:white;text-align:center\">";
                    strHtmlAllClient += "<td align=\"left\" style=\"font-size:xx-small\"><b>Total :</b></td>";
                    strHtmlAllClient += "<td align=\"right\" colspan=\"11\" style=\"font-size:xx-small\"><b>" + oconverter.getFormattedvaluewithoriginalsign(TotalVal) + "</b></td></tr>";
                    strHtmlAllClient += "</table>";
                }
            }
            divObligation.InnerHtml = strHtmlAllClient;
        }
    }
}