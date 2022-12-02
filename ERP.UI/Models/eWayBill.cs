using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace ERP.Models
{

    public sealed class eWayBill
    {
        private eWayBill()
        {
        }
        private static eWayBill obj = null;
        public static eWayBill Obj
        {
            get
            {
                if (obj == null)
                {
                    obj = new eWayBill();
                }
                return obj;
            }
        }


        public string supplyType { get; set; }
        public string subSupplyType { get; set; }
        public string subSupplyDesc { get; set; }
        public string docType { get; set; }
        public string docNo { get; set; }
        public string docDate { get; set; }
        public string fromGstin { get; set; }
        public string fromTrdName { get; set; }
        public string fromAddr1 { get; set; }
        public string fromAddr2 { get; set; }
        public int fromPincode { get; set; }
        public int actFromStateCode { get; set; }
        public int fromStateCode { get; set; }
        public string fromPlace { get; set; }
        public string toGstin { get; set; }
        public string toTrdName { get; set; }
        public string toAddr1 { get; set; }
        public string toAddr2 { get; set; }
        public string toPlace { get; set; }
        public int toPincode { get; set; }
        public int actToStateCode { get; set; }
        public int toStateCode { get; set; }
        public int transactionType { get; set; }
        public decimal otherValue { get; set; }
        public decimal totalValue { get; set; }
        public decimal cgstValue { get; set; }
        public decimal sgstValue { get; set; }
        public decimal igstValue { get; set; }
        public decimal cessValue { get; set; }
        public decimal cessNonAdvolValue { get; set; }
        public decimal totInvValue { get; set; }
        public string transporterId { get; set; }
        public string transporterName { get; set; }
        public string transDocNo { get; set; }
        public string transMode { get; set; }
        public string transDistance { get; set; }
        public string transDocDate { get; set; }
        public string vehicleNo { get; set; }
        public string vehicleType { get; set; }
        public List<eWayBillProduct> itemList { get; set; }


        public eWayBill GetData(DataSet ds, string doc_type)
        {
            DataTable dtHeaderFrom = ds.Tables[0];
            DataTable dtHeaderTo = ds.Tables[1];
            DataTable dtHeaderAmount = ds.Tables[2];
            DataTable dtHeader = ds.Tables[3];
            DataTable dtProducts = ds.Tables[4];

            eWayBill.Obj.actFromStateCode = Convert.ToInt32(dtHeaderFrom.Rows[0]["actFromStateCode"]);
            eWayBill.Obj.actToStateCode = Convert.ToInt32(dtHeaderTo.Rows[0]["actToStateCode"]);
            eWayBill.Obj.cessNonAdvolValue = Convert.ToDecimal(Convert.ToDecimal(dtHeaderAmount.Rows[0]["cessNonAdvolValue"]).ToString("0.00"));
            eWayBill.Obj.cessValue = Convert.ToDecimal(Convert.ToDecimal(dtHeaderAmount.Rows[0]["cessValue"]).ToString("0.00"));
            eWayBill.Obj.cgstValue = Convert.ToDecimal(Convert.ToDecimal(dtHeaderAmount.Rows[0]["cgstValue"]).ToString("0.00"));
            eWayBill.Obj.docDate = Convert.ToDateTime(dtHeader.Rows[0]["docDate"]).ToString("dd/MM/yyy");
            eWayBill.Obj.docNo = Convert.ToString(dtHeader.Rows[0]["docNo"]);
            eWayBill.Obj.fromPlace = Convert.ToString(dtHeaderFrom.Rows[0]["Loc"]);
            eWayBill.Obj.docType = Convert.ToString(doc_type);
            eWayBill.Obj.fromAddr1 = Convert.ToString(dtHeaderFrom.Rows[0]["fromAddr1"]);
            eWayBill.Obj.fromAddr2 = Convert.ToString(dtHeaderFrom.Rows[0]["fromAddr2"]);
            eWayBill.Obj.fromGstin = Convert.ToString(dtHeaderFrom.Rows[0]["fromGstin"]);
            eWayBill.Obj.fromPincode = Convert.ToInt32(dtHeaderFrom.Rows[0]["fromPincode"]);
            eWayBill.Obj.fromStateCode = Convert.ToInt32(dtHeaderFrom.Rows[0]["fromStateCode"]);
            eWayBill.Obj.fromTrdName = Convert.ToString(dtHeaderFrom.Rows[0]["fromTrdName"]);
            eWayBill.Obj.igstValue = Convert.ToDecimal(Convert.ToDecimal(dtHeaderAmount.Rows[0]["igstValue"]).ToString("0.00"));
            eWayBill.Obj.itemList = eWayBillProduct.Obj.GetProductList(dtProducts);
            eWayBill.Obj.otherValue = Convert.ToDecimal(Convert.ToDecimal(dtHeaderAmount.Rows[0]["otherValue"]).ToString("0.00"));
            eWayBill.Obj.sgstValue = Convert.ToDecimal(Convert.ToDecimal(dtHeaderAmount.Rows[0]["sgstValue"]).ToString("0.00"));
            eWayBill.Obj.subSupplyDesc = Convert.ToString(dtHeader.Rows[0]["subSupplyDesc"]);
            eWayBill.Obj.subSupplyType = Convert.ToString(dtHeader.Rows[0]["subSupplyType"]);
            eWayBill.Obj.supplyType = Convert.ToString(dtHeader.Rows[0]["supplyType"]);
            eWayBill.Obj.toAddr1 = Convert.ToString(dtHeaderTo.Rows[0]["toAddr1"]);
            eWayBill.Obj.toAddr2 = Convert.ToString(dtHeaderTo.Rows[0]["toAddr2"]);
            eWayBill.Obj.toGstin = Convert.ToString(dtHeaderTo.Rows[0]["toGstin"]);
            eWayBill.Obj.toPincode = Convert.ToInt32(dtHeaderTo.Rows[0]["toPincode"]);
            eWayBill.Obj.toPlace = Convert.ToString(dtHeaderTo.Rows[0]["toPlace"]);
            eWayBill.Obj.toStateCode = Convert.ToInt32(dtHeaderTo.Rows[0]["toStateCode"]);
            eWayBill.Obj.totalValue = Convert.ToDecimal(Convert.ToDecimal(dtHeaderAmount.Rows[0]["totalValue"]).ToString("0.00"));
            eWayBill.Obj.totInvValue = Convert.ToDecimal(Convert.ToDecimal(dtHeaderAmount.Rows[0]["totInvValue"]).ToString("0.00"));
            eWayBill.Obj.toTrdName = Convert.ToString(dtHeaderTo.Rows[0]["toTrdName"]);



            eWayBill.Obj.transactionType = Convert.ToInt32(dtHeader.Rows[0]["transactionType"]);
            // eWayBill.Obj.transDistance = Convert.ToString(dtHeader.Rows[0]["transDistance"]);
            //eWayBill.Obj.transDocDate = Convert.ToString(dtHeader.Rows[0]["transDocDate"]);
            //eWayBill.Obj.transDocNo = Convert.ToString(dtHeader.Rows[0]["transDocNo"]);
            // eWayBill.Obj.transMode = Convert.ToString(dtHeader.Rows[0]["transMode"]);
            // eWayBill.Obj.transporterId = Convert.ToString(dtHeader.Rows[0]["transporterId"]);
            //eWayBill.Obj.transporterName = Convert.ToString(dtHeader.Rows[0]["transporterName"]);
            // eWayBill.Obj.vehicleNo = Convert.ToString(dtHeader.Rows[0]["vehicleNo"]);
            //eWayBill.Obj.vehicleType = Convert.ToString(dtHeader.Rows[0]["vehicleType"]);


            if (dtHeader.Rows[0]["transDistance"] != null && Convert.ToDecimal(dtHeader.Rows[0]["transDistance"]) != 0)
                eWayBill.Obj.transDistance = Convert.ToString(dtHeader.Rows[0]["transDistance"]);    ///from table Mantis id 23408 
            else
                eWayBill.Obj.transDistance = "0";
            ///
            if (dtHeader.Rows[0]["transDocDate"] != DBNull.Value && dtHeader.Rows[0]["transDocDate"] != null)
                eWayBill.Obj.transDocDate = Convert.ToDateTime(dtHeader.Rows[0]["transDocDate"]).ToString("dd/MM/yyy"); ///from table Mantis id 23408 
            ///
            if (dtHeader.Rows[0]["transDocNo"] != DBNull.Value && dtHeader.Rows[0]["transDocNo"] != null)
                eWayBill.Obj.transDocNo = Convert.ToString(dtHeader.Rows[0]["transDocNo"]); ///from table Mantis id 23408 
            ///

            if (dtHeader.Rows[0]["transporterId"] != DBNull.Value && dtHeader.Rows[0]["transporterId"] != null)
                eWayBill.Obj.transporterId = Convert.ToString(dtHeader.Rows[0]["transporterId"]);   ///from table Mantis id 23408 
            ///

            if (dtHeader.Rows[0]["transMode"] != DBNull.Value && dtHeader.Rows[0]["transMode"] != null)
                eWayBill.Obj.transMode = Convert.ToString(dtHeader.Rows[0]["transMode"]); ///from table Mantis id 23408 
            ///

            if (dtHeader.Rows[0]["transporterName"] != DBNull.Value && dtHeader.Rows[0]["transporterName"] != null)
                eWayBill.Obj.transporterName = Convert.ToString(dtHeader.Rows[0]["transporterName"]);  ///from table Mantis id 23408 
            ///

            if (dtHeader.Rows[0]["vehicleNo"] != DBNull.Value && dtHeader.Rows[0]["vehicleNo"] != null)
                eWayBill.Obj.vehicleNo = Convert.ToString(dtHeader.Rows[0]["vehicleNo"]);    ///from table Mantis id 23408 

            if (dtHeader.Rows[0]["vehicleType"] != DBNull.Value && dtHeader.Rows[0]["vehicleType"] != null)
                eWayBill.Obj.vehicleType = Convert.ToString(dtHeader.Rows[0]["vehicleType"]);    ///from table Mantis id 23408 


            //eWayBillProduct.ObjList.Clear();
            return obj;
        }

    }
}

public sealed class eWayBillProduct
{
    private eWayBillProduct()
    {
    }
    private static eWayBillProduct obj = null;
    public static eWayBillProduct Obj
    {
        get
        {
            if (obj == null)
            {
                obj = new eWayBillProduct();
            }
            return obj;
        }
    }

    private static List<eWayBillProduct> objList = null;
    public static List<eWayBillProduct> ObjList
    {
        get
        {
            if (objList == null)
            {
                objList = new List<eWayBillProduct>();
            }
            return objList;
        }
    }

    public string productName { get; set; }
    public string productDesc { get; set; }
    public int hsnCode { get; set; }
    public decimal quantity { get; set; }
    public string qtyUnit { get; set; }
    public decimal cgstRate { get; set; }
    public decimal sgstRate { get; set; }
    public decimal igstRate { get; set; }
    public decimal cessRate { get; set; }
    public decimal cessNonadvol { get; set; }
    public decimal taxableAmount { get; set; }


    public List<eWayBillProduct> GetProductList(DataTable dtProduct)
    {
        foreach (DataRow drProduct in dtProduct.Rows)
        {
            eWayBillProduct.Obj.cessNonadvol = Convert.ToDecimal(Convert.ToDecimal(drProduct["cessNonadvol"]).ToString("0.00"));
            eWayBillProduct.Obj.cessRate = Convert.ToDecimal(Convert.ToDecimal(drProduct["cessRate"]).ToString("0.00"));
            eWayBillProduct.Obj.cgstRate = Convert.ToDecimal(Convert.ToDecimal(drProduct["cgstRate"]).ToString("0.00"));
            eWayBillProduct.Obj.hsnCode = Convert.ToInt32(drProduct["hsnCode"]);
            eWayBillProduct.Obj.igstRate = Convert.ToDecimal(Convert.ToDecimal(drProduct["igstRate"]).ToString("0.00"));
            eWayBillProduct.Obj.productName = Convert.ToString(drProduct["productName"]);
            eWayBillProduct.Obj.productDesc = Convert.ToString(drProduct["productDesc"]);
            eWayBillProduct.Obj.qtyUnit = Convert.ToString(drProduct["qtyUnit"]);
            eWayBillProduct.Obj.quantity = Convert.ToDecimal(Convert.ToDecimal(drProduct["quantity"]).ToString("0.000"));
            eWayBillProduct.Obj.sgstRate = Convert.ToDecimal(Convert.ToDecimal(drProduct["sgstRate"]).ToString("0.00"));
            eWayBillProduct.Obj.taxableAmount = Convert.ToDecimal(Convert.ToDecimal(drProduct["taxableAmount"]).ToString("0.00"));
            eWayBillProduct.ObjList.Add(eWayBillProduct.Obj);
        }


        return eWayBillProduct.ObjList;
    }

}