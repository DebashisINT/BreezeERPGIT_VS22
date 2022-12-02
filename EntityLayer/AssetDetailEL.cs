﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer
{
   public class AssetDetailEL
    {  
       public int AssetDetail_ID { get; set; } 
       public string AssetDetail_CompanyID { get; set; }
       public string AssetDetail_FinYear { get; set; }
       public string AssetDetail_MainAccountCode { get; set; }
       public string AssetDetail_SubAccountCode { get; set; }
       public string AssetDetail_Category { get; set; }
       public DateTime AssetDetail_PurchaseDate { get; set; }
       public string AssetDetail_Vendor { get; set; }
       public decimal AssetDetail_CostPrice { get; set; }
       public string AssetDetail_BillNumber { get; set; }
       public decimal AssetDetail_Additions { get; set; }
       public decimal AssetDetail_Disposals { get; set; }
       public decimal AssetDetail_Depreciation { get; set; }
       public decimal AssetDetail_DepreciationIT { get; set; }
       public Int32 AssetDetail_Location { get; set; }
       public string AssetDetail_User { get; set; }
       public string AssetDetail_Insurer { get; set; }
       public decimal AssetDetail_Premium { get; set; }
       public DateTime AssetDetail_PolicyExpiryDate { get; set; }
       public DateTime AssetDetail_PremiumDueDate { get; set; }
       public string AssetDetail_ServiceProvider { get; set; }
       public DateTime AssetDetail_AMCExpiryDate { get; set; }
       public decimal AssetDetail_BroughtForward { get; set; }
       public Int32 AssetDetail_CreateUser { get; set; }
       public DateTime AssetDetail_CreateDate { get; set; }
       public Int32 AssetDetail_ModifyUser { get; set; }
       public DateTime AssetDetail_ModifyDateTime { get; set; }
       


           
    }
}
