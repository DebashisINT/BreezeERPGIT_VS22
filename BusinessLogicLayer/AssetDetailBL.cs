using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks; 
using DataAccessLayer;
using System.Web;
using EntityLayer;
 

namespace BusinessLogicLayer
{
   public class AssetDetailBL
   {

       public DataTable PopulateGridForAssetDetail(string MainAccountCode, string SubAccountCode)
       {
           ProcedureExecute proc = new ProcedureExecute("prc_AssetDetail");
           proc.AddVarcharPara("@Action", 100, "PopulateGridForAssetDetail");
           if (MainAccountCode!="")
           {
               proc.AddVarcharPara("@AssetDetail_MainAccountCode", 100, MainAccountCode);
           }
           if (SubAccountCode !="")
           {
               proc.AddVarcharPara("@AssetDetail_SubAccountCode", 100, SubAccountCode);
           }
           return proc.GetTable();
       }
       public DataSet PopulateAllDropDownDataForAssetDetail()
       {
            
           ProcedureExecute proc = new ProcedureExecute("prc_AssetDetail");
           proc.AddVarcharPara("@Action", 100, "PopulateAllDropDownDataForAssetDetail");
           return proc.GetDataSet();
       }


       public int InsertAssetDetail(AssetDetailEL objAssetDetailEL)
       {
           int i;
           int rtrnvalue = 0;
  //@AssetDetail_CompanyID varchar(10)=null,
 //@AssetDetail_FinYear varchar(10)=null, 
 //@AssetDetail_MainAccountCode varchar(10)=null,
 //@AssetDetail_SubAccountCode varchar(10)=null, 
 //@AssetDetail_Category varchar(1)=null,
 //@AssetDetail_PurchaseDate datetime=null,


           ProcedureExecute proc = new ProcedureExecute("prc_AssetDetail");
           proc.AddVarcharPara("@Action", 100, "InsertAssetDetail");
           proc.AddVarcharPara("@AssetDetail_CompanyID", 10, objAssetDetailEL.AssetDetail_CompanyID);
           proc.AddVarcharPara("@AssetDetail_FinYear", 10, objAssetDetailEL.AssetDetail_FinYear);
           proc.AddVarcharPara("@AssetDetail_MainAccountCode", 10, objAssetDetailEL.AssetDetail_MainAccountCode);
           proc.AddVarcharPara("@AssetDetail_SubAccountCode", 10, objAssetDetailEL.AssetDetail_SubAccountCode);
           proc.AddVarcharPara("@AssetDetail_Category", 1, objAssetDetailEL.AssetDetail_Category); 
           //proc.AddDateTimePara("@AssetDetail_PurchaseDate", objAssetDetailEL.AssetDetail_PurchaseDate);
           if (objAssetDetailEL.AssetDetail_PurchaseDate != Convert.ToDateTime("1/1/0001 12:00:00 AM"))
           {

               proc.AddDateTimePara("@AssetDetail_PurchaseDate", objAssetDetailEL.AssetDetail_PurchaseDate);
           }


            //@AssetDetail_Vendor varchar(10)=null,
            //@AssetDetail_CostPrice money=null, 
            //@AssetDetail_BillNumber varchar(20)=null,
            //@AssetDetail_Additions money=null, 
            //@AssetDetail_Disposals money=null,
            //@AssetDetail_Depreciation money=null,
           proc.AddVarcharPara("@AssetDetail_Vendor", 10, objAssetDetailEL.AssetDetail_Vendor);
           proc.AddDecimalPara("@AssetDetail_CostPrice",2, 18, objAssetDetailEL.AssetDetail_CostPrice);
           //////////////////////
           proc.AddVarcharPara("@AssetDetail_BillNumber", 50, objAssetDetailEL.AssetDetail_BillNumber);
           ////////////////////
           proc.AddDecimalPara("@AssetDetail_Additions",2,18, objAssetDetailEL.AssetDetail_Additions); 
           proc.AddDecimalPara("@AssetDetail_Disposals",2,18, objAssetDetailEL.AssetDetail_Disposals);
           proc.AddDecimalPara("@AssetDetail_Depreciation",2,18, objAssetDetailEL.AssetDetail_Depreciation);


            //@AssetDetail_DepreciationIT money=null,
            //@AssetDetail_Location int=null,
            //@AssetDetail_User varchar(10)=null,
            //@AssetDetail_Insurer varchar(10)=null,
            //@AssetDetail_Premium money=null,
            //@AssetDetail_PolicyExpiryDate datetime=null,
           proc.AddDecimalPara("@AssetDetail_DepreciationIT",2,18, objAssetDetailEL.AssetDetail_DepreciationIT);
           proc.AddIntegerPara("@AssetDetail_Location",  objAssetDetailEL.AssetDetail_Location);
           proc.AddVarcharPara("@AssetDetail_User", 10, objAssetDetailEL.AssetDetail_User);
           proc.AddVarcharPara("@AssetDetail_Insurer", 10, objAssetDetailEL.AssetDetail_Insurer);

           //////////////////
           proc.AddDecimalPara("@AssetDetail_Premium",2,18, objAssetDetailEL.AssetDetail_Premium);
           //////////////////

           if (objAssetDetailEL.AssetDetail_PolicyExpiryDate != Convert.ToDateTime("1/1/0001 12:00:00 AM"))
           {

               proc.AddDateTimePara("@AssetDetail_PolicyExpiryDate", objAssetDetailEL.AssetDetail_PolicyExpiryDate);
           }

           //proc.AddDateTimePara("@AssetDetail_PolicyExpiryDate",  objAssetDetailEL.AssetDetail_PolicyExpiryDate); 
             //@AssetDetail_PremiumDueDate datetime=null,
             //@AssetDetail_ServiceProvider varchar(10)=null,
             //@AssetDetail_AMCExpiryDate datetime=null,
             //@AssetDetail_BroughtForward money=null,
             //@AssetDetail_CreateUser int=null,
             //@AssetDetail_CreateDate datetime=null,
             //@AssetDetail_ModifyUser int=null,
             //@AssetDetail_ModifyDateTime datetime=null 

           //proc.AddDateTimePara("@AssetDetail_PremiumDueDate",  objAssetDetailEL.AssetDetail_PremiumDueDate);
           if (objAssetDetailEL.AssetDetail_PremiumDueDate != Convert.ToDateTime("1/1/0001 12:00:00 AM"))
           {

               proc.AddDateTimePara("@AssetDetail_PremiumDueDate", objAssetDetailEL.AssetDetail_PremiumDueDate);
           }

           proc.AddVarcharPara("@AssetDetail_ServiceProvider", 10, objAssetDetailEL.AssetDetail_ServiceProvider); 
           //proc.AddDateTimePara("@AssetDetail_AMCExpiryDate", objAssetDetailEL.AssetDetail_AMCExpiryDate); 
           if (objAssetDetailEL.AssetDetail_AMCExpiryDate != Convert.ToDateTime("1/1/0001 12:00:00 AM"))
           { 
               proc.AddDateTimePara("@AssetDetail_AMCExpiryDate", objAssetDetailEL.AssetDetail_AMCExpiryDate);
           }
           proc.AddDecimalPara("@AssetDetail_BroughtForward",2,18, objAssetDetailEL.AssetDetail_BroughtForward);

           proc.AddIntegerPara("@AssetDetail_CreateUser", objAssetDetailEL.AssetDetail_CreateUser); 
           proc.AddVarcharPara("@ReturnValue", 200, "0", QueryParameterDirection.Output);
           i = proc.RunActionQuery();
           rtrnvalue = Convert.ToInt32(proc.GetParaValue("@ReturnValue"));
           return rtrnvalue;
       }


       public DataTable GetAssetDetailById(int id)
       {
           ProcedureExecute proc = new ProcedureExecute("prc_AssetDetail");
           proc.AddVarcharPara("@Action", 100, "GetAssetDetailById");
           proc.AddIntegerPara("@AssetDetail_ID", id);
           
           return proc.GetTable();
       }

       public int UpdateAssetDetail(AssetDetailEL objAssetDetailEL)
       {
           int i;
           int rtrnvalue = 0;
           //@AssetDetail_CompanyID varchar(10)=null,
           //@AssetDetail_FinYear varchar(10)=null, 
           //@AssetDetail_MainAccountCode varchar(10)=null,
           //@AssetDetail_SubAccountCode varchar(10)=null, 
           //@AssetDetail_Category varchar(1)=null,
           //@AssetDetail_PurchaseDate datetime=null,


           ProcedureExecute proc = new ProcedureExecute("prc_AssetDetail");
           proc.AddVarcharPara("@Action", 100, "UpdateAssetDetail");
           proc.AddIntegerPara("@AssetDetail_ID", objAssetDetailEL.AssetDetail_ID);
           proc.AddVarcharPara("@AssetDetail_CompanyID", 10, objAssetDetailEL.AssetDetail_CompanyID);
           proc.AddVarcharPara("@AssetDetail_FinYear", 10, objAssetDetailEL.AssetDetail_FinYear);
           proc.AddVarcharPara("@AssetDetail_MainAccountCode", 10, objAssetDetailEL.AssetDetail_MainAccountCode);
           proc.AddVarcharPara("@AssetDetail_SubAccountCode", 10, objAssetDetailEL.AssetDetail_SubAccountCode);
           proc.AddVarcharPara("@AssetDetail_Category", 1, objAssetDetailEL.AssetDetail_Category);
           if (objAssetDetailEL.AssetDetail_PurchaseDate != Convert.ToDateTime("1/1/0001 12:00:00 AM"))
           {

               proc.AddDateTimePara("@AssetDetail_PurchaseDate", objAssetDetailEL.AssetDetail_PurchaseDate);
           }


           //@AssetDetail_Vendor varchar(10)=null,
           //@AssetDetail_CostPrice money=null, 
           //@AssetDetail_BillNumber varchar(20)=null,
           //@AssetDetail_Additions money=null, 
           //@AssetDetail_Disposals money=null,
           //@AssetDetail_Depreciation money=null,
           proc.AddVarcharPara("@AssetDetail_Vendor", 10, objAssetDetailEL.AssetDetail_Vendor);
           proc.AddDecimalPara("@AssetDetail_CostPrice", 2, 18, objAssetDetailEL.AssetDetail_CostPrice);
           //////////////////////
           proc.AddVarcharPara("@AssetDetail_BillNumber", 50, objAssetDetailEL.AssetDetail_BillNumber);
           ////////////////////
           proc.AddDecimalPara("@AssetDetail_Additions", 2, 18, objAssetDetailEL.AssetDetail_Additions);
           proc.AddDecimalPara("@AssetDetail_Disposals", 2, 18, objAssetDetailEL.AssetDetail_Disposals);
           proc.AddDecimalPara("@AssetDetail_Depreciation", 2, 18, objAssetDetailEL.AssetDetail_Depreciation);


           //@AssetDetail_DepreciationIT money=null,
           //@AssetDetail_Location int=null,
           //@AssetDetail_User varchar(10)=null,
           //@AssetDetail_Insurer varchar(10)=null,
           //@AssetDetail_Premium money=null,
           //@AssetDetail_PolicyExpiryDate datetime=null,
           proc.AddDecimalPara("@AssetDetail_DepreciationIT", 2, 18, objAssetDetailEL.AssetDetail_DepreciationIT);
           proc.AddIntegerPara("@AssetDetail_Location", objAssetDetailEL.AssetDetail_Location);
           proc.AddVarcharPara("@AssetDetail_User", 10, objAssetDetailEL.AssetDetail_User);
           proc.AddVarcharPara("@AssetDetail_Insurer", 10, objAssetDetailEL.AssetDetail_Insurer);

           //////////////////
           proc.AddDecimalPara("@AssetDetail_Premium", 2, 18, objAssetDetailEL.AssetDetail_Premium);
           //////////////////
           if (objAssetDetailEL.AssetDetail_PolicyExpiryDate != Convert.ToDateTime("1/1/0001 12:00:00 AM"))
           {

               proc.AddDateTimePara("@AssetDetail_PolicyExpiryDate", objAssetDetailEL.AssetDetail_PolicyExpiryDate);
           }
            
           //@AssetDetail_PremiumDueDate datetime=null,
           //@AssetDetail_ServiceProvider varchar(10)=null,
           //@AssetDetail_AMCExpiryDate datetime=null,
           //@AssetDetail_BroughtForward money=null,
           //@AssetDetail_CreateUser int=null,
           //@AssetDetail_CreateDate datetime=null,
           //@AssetDetail_ModifyUser int=null,
           //@AssetDetail_ModifyDateTime datetime=null 
           if (objAssetDetailEL.AssetDetail_PremiumDueDate != Convert.ToDateTime("1/1/0001 12:00:00 AM"))
           {

               proc.AddDateTimePara("@AssetDetail_PremiumDueDate", objAssetDetailEL.AssetDetail_PremiumDueDate);
           }

           
           proc.AddVarcharPara("@AssetDetail_ServiceProvider", 10, objAssetDetailEL.AssetDetail_ServiceProvider);
           if (objAssetDetailEL.AssetDetail_AMCExpiryDate != Convert.ToDateTime("1/1/0001 12:00:00 AM"))
           {

               proc.AddDateTimePara("@AssetDetail_AMCExpiryDate", objAssetDetailEL.AssetDetail_AMCExpiryDate);
           }
           

           proc.AddDecimalPara("@AssetDetail_BroughtForward", 2, 18, objAssetDetailEL.AssetDetail_BroughtForward);

           proc.AddIntegerPara("AssetDetail_ModifyUser", objAssetDetailEL.AssetDetail_CreateUser);
           proc.AddVarcharPara("@ReturnValue", 200, "0", QueryParameterDirection.Output);
           i = proc.RunActionQuery();
           rtrnvalue = Convert.ToInt32(proc.GetParaValue("@ReturnValue"));
           return rtrnvalue;
       }
 
       public int DeleteAssetDetail(int id)
       {
           int i;
           int rtrnvalue = 0;
           ProcedureExecute proc = new ProcedureExecute("prc_AssetDetail");
           proc.AddVarcharPara("@Action", 100, "DeleteAssetDetail");
           proc.AddIntegerPara("@AssetDetail_ID", id);
           proc.AddVarcharPara("@ReturnValue", 200, "0", QueryParameterDirection.Output);
           i = proc.RunActionQuery();
           rtrnvalue = Convert.ToInt32(proc.GetParaValue("@ReturnValue"));
           return rtrnvalue;
       }

       //BindTdsTypeForSubLedger

       public DataTable BindTdsTypeForSubLedger()
       {
           ProcedureExecute proc = new ProcedureExecute("prc_AssetDetail");
           proc.AddVarcharPara("@Action", 100, "BindTdsTypeForSubLedger"); 
           return proc.GetTable();
       }
    
   }
}
