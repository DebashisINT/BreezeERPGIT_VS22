using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAccessLayer;
using System.Data;


namespace BusinessLogicLayer
{
    partial class GlobalSettings
    {
        public int InsertFundProfileSettings(string FundProfile_Code, int FundProfile_SegmentID, DateTime FundProfile_DateFrom,
                                   Boolean FundProfile_AllowThirdPartyReceipts, int FundProfile_PayoutRule, int FundProfile_PayoutMode,
          Decimal FundProfile_MinimumCreditBalance, Decimal FundProfile_MaximumDebitBalance, int FundProfile_InterestChargeableAfter,
            Decimal FundProfile_RateOfInterest, Decimal FundProfile_ChequeDishonourPenalty, int FundProfile_CreateUser, int FundProfile_ID)
        {
            ProcedureExecute proc;
            try
            {


                using (proc = new ProcedureExecute("InsertFundProfileSettings"))
                {

                    proc.AddVarcharPara("@FundProfile_Code", 100, FundProfile_Code);
                    proc.AddIntegerPara("@FundProfile_SegmentID", FundProfile_SegmentID);
                    proc.AddDateTimePara("@FundProfile_DateFrom", FundProfile_DateFrom);
                    proc.AddBooleanPara("@FundProfile_AllowThirdPartyReceipts", FundProfile_AllowThirdPartyReceipts);
                    proc.AddIntegerPara("@FundProfile_PayoutRule", FundProfile_PayoutRule);
                    proc.AddIntegerPara("@FundProfile_PayoutMode", FundProfile_PayoutMode);
                    proc.AddDecimalPara("@FundProfile_MinimumCreditBalance", 10, 3, FundProfile_MinimumCreditBalance);
                    proc.AddDecimalPara("@FundProfile_MaximumDebitBalance", 10, 3, FundProfile_MaximumDebitBalance);
                    proc.AddIntegerPara("@FundProfile_InterestChargeableAfter", FundProfile_InterestChargeableAfter);
                    proc.AddDecimalPara("@FundProfile_RateOfInterest", 10, 3, FundProfile_RateOfInterest);
                    proc.AddDecimalPara("@FundProfile_ChequeDishonourPenalty", 10, 3, FundProfile_ChequeDishonourPenalty);
                    proc.AddIntegerPara("@FundProfile_CreateUser", FundProfile_CreateUser);
                    proc.AddIntegerPara("@FundProfile_ID", FundProfile_ID);

                    int i = proc.RunActionQuery();

                    return i;


                }
            }

            catch (Exception ex)
            {
                throw ex;
            }

            finally
            {
                proc = null;
            }
        }




        public int InsertDeliveryProfileSettings(string DeliveryProfile_Code, int DeliveryProfile_SegmentID, DateTime DeliveryProfile_DateFrom,
                      int DeliveryProfile_OutgoingDeliveries, int DeliveryProfile_DesignatedBenAccount, Boolean DeliveryProfile_HoldbackEntireDeliveries,
            Decimal DeliveryProfile_MarkupOnDebitBalance, int DeliveryProfile_MaxHoldingPeriod, Boolean DeliveryProfile_AcceptThirdParty,
            Boolean DeliveryProfile_ConsiderPendingPurchases, Boolean DeliveryProfile_ConsiderMarginStocks, Boolean DeliveryProfile_CosiderHoldbackStocks,
            Boolean DeliveryProfile_ConsiderDPStocks, int DeliveryProfile_AuctionDebitRule, Decimal DeliveryProfile_MarkupForAuctionDebit,
            Decimal DeliveryProfile_ShortDeliveryPenalty, string DeliveryProfile_NoPartialHoldback, Decimal DeliveryProfile_DebitGrace,
            int DeliveryProfile_CreateUser, int DeliveryProfile_ID, Boolean DeliveryProfile_ExcludeAutoHldbk, Boolean DeliveryProfile_ConsiderCashmarginDep,
            Boolean DeliveryProfile_DeductedUnclearReceipts, Boolean DeliveryProfile_DeductedUnclearReceiptsmrgnDep, Boolean DeliveryProfile_BenifitPendingPurchase,
            Boolean DeliveryProfile_AddBackPendingSales, Boolean DeliveryProfile_GiveBenefitMrgnStocks, Boolean DeliveryProfile_CosiderAppMargn,
            Boolean DeliveryProfile_ConsiderFdrBalance, Boolean DeliveryProfile_PartialHoldback, Boolean DeliveryProfile_ConsolidateBalnAcrossSegments,
            Decimal DeliveryProfile_AspxMarkUp, Decimal DeliveryProfile_HoldBackMinAmount, Decimal DeliveryProfile_HldPettyAmount, Decimal DeliveryProfile_HldPettyRate)
        {
            ProcedureExecute proc;
            try
            {


                using (proc = new ProcedureExecute("InsertDeliveryProfileSettings"))
                {

                    proc.AddVarcharPara("@DeliveryProfile_Code", 100, DeliveryProfile_Code);
                    proc.AddIntegerPara("@DeliveryProfile_SegmentID", DeliveryProfile_SegmentID);
                    proc.AddDateTimePara("@DeliveryProfile_DateFrom", DeliveryProfile_DateFrom);
                    proc.AddIntegerPara("@DeliveryProfile_OutgoingDeliveries", DeliveryProfile_OutgoingDeliveries);
                    proc.AddIntegerPara("@DeliveryProfile_DesignatedBenAccount", DeliveryProfile_DesignatedBenAccount);
                    proc.AddBooleanPara("@DeliveryProfile_HoldbackEntireDeliveries", DeliveryProfile_HoldbackEntireDeliveries);
                    proc.AddDecimalPara("@DeliveryProfile_MarkupOnDebitBalance", 10, 3, DeliveryProfile_MarkupOnDebitBalance);
                    proc.AddIntegerPara("@DeliveryProfile_MaxHoldingPeriod", DeliveryProfile_MaxHoldingPeriod);
                    proc.AddBooleanPara("@DeliveryProfile_AcceptThirdParty", DeliveryProfile_AcceptThirdParty);
                    proc.AddBooleanPara("@DeliveryProfile_ConsiderPendingPurchases", DeliveryProfile_ConsiderPendingPurchases);
                    proc.AddBooleanPara("@DeliveryProfile_ConsiderMarginStocks", DeliveryProfile_ConsiderMarginStocks);
                    proc.AddBooleanPara("@DeliveryProfile_CosiderHoldbackStocks", DeliveryProfile_CosiderHoldbackStocks);
                    proc.AddBooleanPara("@DeliveryProfile_ConsiderDPStocks", DeliveryProfile_ConsiderDPStocks);
                    proc.AddIntegerPara("@DeliveryProfile_AuctionDebitRule", DeliveryProfile_AuctionDebitRule);
                    proc.AddDecimalPara("@DeliveryProfile_MarkupForAuctionDebit", 10, 3, DeliveryProfile_MarkupForAuctionDebit);
                    proc.AddDecimalPara("@DeliveryProfile_ShortDeliveryPenalty", 10, 3, DeliveryProfile_ShortDeliveryPenalty);
                    proc.AddVarcharPara("@DeliveryProfile_NoPartialHoldback", 100, DeliveryProfile_NoPartialHoldback);
                    proc.AddDecimalPara("@DeliveryProfile_DebitGrace", 10, 3, DeliveryProfile_DebitGrace);
                    proc.AddIntegerPara("@DeliveryProfile_CreateUser", DeliveryProfile_CreateUser);
                    proc.AddIntegerPara("@DeliveryProfile_ID", DeliveryProfile_ID);
                    proc.AddBooleanPara("@DeliveryProfile_ExcludeAutoHldbk", DeliveryProfile_ExcludeAutoHldbk);
                    proc.AddBooleanPara("@DeliveryProfile_ConsiderCashmarginDep", DeliveryProfile_ConsiderCashmarginDep);
                    proc.AddBooleanPara("@DeliveryProfile_DeductedUnclearReceipts", DeliveryProfile_DeductedUnclearReceipts);
                    proc.AddBooleanPara("@DeliveryProfile_DeductedUnclearReceiptsmrgnDep", DeliveryProfile_DeductedUnclearReceiptsmrgnDep);
                    proc.AddBooleanPara("@DeliveryProfile_BenifitPendingPurchase", DeliveryProfile_BenifitPendingPurchase);
                    proc.AddBooleanPara("@DeliveryProfile_AddBackPendingSales", DeliveryProfile_AddBackPendingSales);
                    proc.AddBooleanPara("@DeliveryProfile_GiveBenefitMrgnStocks", DeliveryProfile_GiveBenefitMrgnStocks);
                    proc.AddBooleanPara("@DeliveryProfile_CosiderAppMargn", DeliveryProfile_CosiderAppMargn);
                    proc.AddBooleanPara("@DeliveryProfile_ConsiderFdrBalance", DeliveryProfile_ConsiderFdrBalance);
                    proc.AddBooleanPara("@DeliveryProfile_PartialHoldback", DeliveryProfile_PartialHoldback);
                    proc.AddBooleanPara("@DeliveryProfile_ConsolidateBalnAcrossSegments", DeliveryProfile_ConsolidateBalnAcrossSegments);
                    proc.AddDecimalPara("@DeliveryProfile_AspxMarkUp", 10, 3, DeliveryProfile_AspxMarkUp);
                    proc.AddDecimalPara("@DeliveryProfile_HoldBackMinAmount", 10, 3, DeliveryProfile_HoldBackMinAmount);
                    proc.AddDecimalPara("@DeliveryProfile_HldPettyAmount", 10, 3, DeliveryProfile_HldPettyAmount);
                    proc.AddDecimalPara("@DeliveryProfile_HldPettyRate", 10, 3, DeliveryProfile_HldPettyRate);

                    int i = proc.RunActionQuery();

                    return i;


                }
            }

            catch (Exception ex)
            {
                throw ex;
            }

            finally
            {
                proc = null;
            }
        }





        public int InsertRiskProfileSettings(string RiskProfile_Code, int RiskProfile_SegmentID, DateTime RiskProfile_DateFrom,
                    Decimal RiskProfile_PendingSalesHaircut, Decimal RiskProfile_PendingPurchasesMarkup, Decimal RiskProfile_DayEndExposureLimit,
            Decimal RiskProfile_PeakExposureLimit, Decimal RiskProfile_ObligationLimit, Boolean RiskProfile_ConsiderHoldbackStocks,
            Boolean RiskProfile_ConsiderHoldingPOAMargin, int RiskProfile_ApplicableHaircutRates, Decimal RiskProfile_FlatHaircut,
            Decimal RiskProfile_VARMarkup, Decimal RiskProfile_HoldbackHaircut, Decimal RiskProfile_UnApprovedHaircut, Decimal RiskProfile_FDRHaircut,
            Boolean RiskProfile_ConsiderClearBalance, Decimal RiskProfile_MinimumCashDeposit, Decimal RiskProfile_MinimumCashComponent,
            Decimal RiskProfile_MinimumRequiredBalanceForUpload, Decimal RiskProfile_ExchangeMarginMarkup, Decimal RiskProfile_ExposureMarginRate,
            Decimal RiskProfile_PermissibleMarginShortage, Decimal RiskProfile_MarginShortagePenalty, Decimal RiskProfile_LimitViolationPenalty,
            Decimal RiskProfile_InterestOnMarginShortage, Decimal RiskProfile_DefaultMinimumUpload,
            Decimal RiskProfile_TopUpUploadAmount, Decimal RiskProfile_TopUpUploadCriterion, Decimal RiskProfile_MaxCollateralValue,
            Decimal RiskProfile_MaxUploadValue, Decimal RiskProfile_RunningDebitBalanceAmount, Decimal RiskProfile_RunningDebitBalanceFor,
            string RiskProfile_UploadFormula, string RiskProfile_FOMarginFormula, int RiskProfile_CreateUser, int RiskProfile_ID)
        {
            ProcedureExecute proc;
            try
            {


                using (proc = new ProcedureExecute("InsertRiskProfileSettings"))
                {

                    proc.AddVarcharPara("@RiskProfile_Code", 100, RiskProfile_Code);
                    proc.AddIntegerPara("@RiskProfile_SegmentID", RiskProfile_SegmentID);
                    proc.AddDateTimePara("@RiskProfile_DateFrom", RiskProfile_DateFrom);
                    proc.AddDecimalPara("@RiskProfile_PendingSalesHaircut", 10, 3, RiskProfile_PendingSalesHaircut);
                    proc.AddDecimalPara("@RiskProfile_PendingPurchasesMarkup", 10, 3, RiskProfile_PendingPurchasesMarkup);
                    proc.AddDecimalPara("@RiskProfile_DayEndExposureLimit", 10, 3, RiskProfile_DayEndExposureLimit);
                    proc.AddDecimalPara("@RiskProfile_PeakExposureLimit", 10, 3, RiskProfile_PeakExposureLimit);
                    proc.AddDecimalPara("@RiskProfile_ObligationLimit", 10, 3, RiskProfile_ObligationLimit);
                    proc.AddBooleanPara("@RiskProfile_ConsiderHoldbackStocks", RiskProfile_ConsiderHoldbackStocks);
                    proc.AddBooleanPara("@RiskProfile_ConsiderHoldingPOAMargin", RiskProfile_ConsiderHoldingPOAMargin);
                    proc.AddIntegerPara("@RiskProfile_ApplicableHaircutRates", RiskProfile_ApplicableHaircutRates);
                    proc.AddDecimalPara("@RiskProfile_FlatHaircut", 10, 3, RiskProfile_FlatHaircut);
                    proc.AddDecimalPara("@RiskProfile_VARMarkup", 10, 3, RiskProfile_VARMarkup);
                    proc.AddDecimalPara("@RiskProfile_HoldbackHaircut", 10, 3, RiskProfile_HoldbackHaircut);
                    proc.AddDecimalPara("@RiskProfile_UnApprovedHaircut", 10, 3, RiskProfile_UnApprovedHaircut);
                    proc.AddDecimalPara("@RiskProfile_FDRHaircut", 10, 3, RiskProfile_FDRHaircut);
                    proc.AddBooleanPara("@RiskProfile_ConsiderClearBalance", RiskProfile_ConsiderClearBalance);
                    proc.AddDecimalPara("@RiskProfile_MinimumCashDeposit", 10, 3, RiskProfile_MinimumCashDeposit);
                    proc.AddDecimalPara("@RiskProfile_MinimumCashComponent", 10, 3, RiskProfile_MinimumCashComponent);
                    proc.AddDecimalPara("@RiskProfile_MinimumRequiredBalanceForUpload", 10, 3, RiskProfile_MinimumRequiredBalanceForUpload);
                    proc.AddDecimalPara("@RiskProfile_ExchangeMarginMarkup", 10, 3, RiskProfile_ExchangeMarginMarkup);
                    proc.AddDecimalPara("@RiskProfile_ExposureMarginRate", 10, 3, RiskProfile_ExposureMarginRate);
                    proc.AddDecimalPara("@RiskProfile_PermissibleMarginShortage", 10, 3, RiskProfile_PermissibleMarginShortage);
                    proc.AddDecimalPara("@RiskProfile_MarginShortagePenalty", 10, 3, RiskProfile_MarginShortagePenalty);
                    proc.AddDecimalPara("@RiskProfile_LimitViolationPenalty", 10, 3, RiskProfile_LimitViolationPenalty);
                    proc.AddDecimalPara("@RiskProfile_InterestOnMarginShortage", 10, 3, RiskProfile_InterestOnMarginShortage);
                    proc.AddDecimalPara("@RiskProfile_DefaultMinimumUpload", 10, 3, RiskProfile_DefaultMinimumUpload);
                    proc.AddDecimalPara("@RiskProfile_TopUpUploadAmount", 10, 3, RiskProfile_TopUpUploadAmount);
                    proc.AddDecimalPara("@RiskProfile_TopUpUploadCriterion", 10, 3, RiskProfile_TopUpUploadCriterion);
                    proc.AddDecimalPara("@RiskProfile_MaxCollateralValue", 10, 3, RiskProfile_MaxCollateralValue);
                    proc.AddDecimalPara("@RiskProfile_MaxUploadValue", 10, 3, RiskProfile_MaxUploadValue);
                    proc.AddDecimalPara("@RiskProfile_RunningDebitBalanceAmount", 10, 3, RiskProfile_RunningDebitBalanceAmount);
                    proc.AddDecimalPara("@RiskProfile_RunningDebitBalanceFor", 10, 3, RiskProfile_RunningDebitBalanceFor);
                    proc.AddVarcharPara("@RiskProfile_UploadFormula", 100, RiskProfile_UploadFormula);
                    proc.AddVarcharPara("@RiskProfile_FOMarginFormula", 100, RiskProfile_FOMarginFormula);
                    proc.AddIntegerPara("@RiskProfile_CreateUser", RiskProfile_CreateUser);
                    proc.AddIntegerPara("@RiskProfile_ID", RiskProfile_ID);

                    int i = proc.RunActionQuery();

                    return i;


                }
            }

            catch (Exception ex)
            {
                throw ex;
            }

            finally
            {
                proc = null;
            }
        }


    }
}
