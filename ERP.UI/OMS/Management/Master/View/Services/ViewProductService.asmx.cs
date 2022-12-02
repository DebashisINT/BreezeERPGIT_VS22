using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Script.Services;
using DataAccessLayer;
using System.Data;


namespace ERP.OMS.Management.Master.View.Services
{
    /// <summary>
    /// Summary description for ViewProductService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class ViewProductService : System.Web.Services.WebService
    {

        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetProductDetails(string id)
       {
            ProductDetails return_obj = new ProductDetails();

            ProductHeader gc = new ProductHeader();
            List<Components> _Components = new List<Components>();
            List<DocumentDetailsProduct> _documentdtls = new List<DocumentDetailsProduct>();
            string output = string.Empty;


            try
            {


                if (HttpContext.Current.Session["userid"] != null)
                {
                    ProcedureExecute proc = new ProcedureExecute("prc_get_product_details");
                    proc.AddVarcharPara("@Action", 50, "get_product_details");
                    proc.AddVarcharPara("@ID", 50, id);
                    proc.AddVarcharPara("@is_success", 500, "", QueryParameterDirection.Output);
                    DataSet ds = proc.GetDataSet();
                    output = Convert.ToString(proc.GetParaValue("@is_success"));
                    if(output=="true")
                    {
                        gc.sProducts_Code = ds.Tables[0].Rows[0]["sProducts_Code"].ToString().Trim();
                        gc.sProducts_Name = ds.Tables[0].Rows[0]["sProducts_Name"].ToString().Trim();
                        gc.sProducts_Description = ds.Tables[0].Rows[0]["sProducts_Description"].ToString().Trim();
                        gc.Inventory_item = ds.Tables[0].Rows[0]["Inventory_item"].ToString().Trim();
                        gc.status = ds.Tables[0].Rows[0]["status"].ToString().Trim();
                        gc.Service_item = ds.Tables[0].Rows[0]["Service_item"].ToString().Trim();
                        gc.sProducts_Type = ds.Tables[0].Rows[0]["sProducts_Type"].ToString().Trim();
                        gc.stck_val_tech = ds.Tables[0].Rows[0]["stck_val_tech"].ToString().Trim();
                        gc.class_name = ds.Tables[0].Rows[0]["class_name"].ToString().Trim();
                        gc.sProducts_HsnCode = ds.Tables[0].Rows[0]["sProducts_HsnCode"].ToString().Trim();
                        gc.furtherence_to_buisness = Convert.ToBoolean(ds.Tables[0].Rows[0]["furtherence_to_buisness"].ToString().Trim());
                        gc.Quote_currency = ds.Tables[0].Rows[0]["Quote_currency"].ToString().Trim();
                        gc.uom_factor = ds.Tables[0].Rows[0]["uom_factor"].ToString().Trim();
                        gc.Quote_uom = ds.Tables[0].Rows[0]["Quote_uom"].ToString().Trim();
                        gc.Capital_goods = ds.Tables[0].Rows[0]["Capital_goods"].ToString().Trim();
                        gc.Sale_Uom_Factor = ds.Tables[0].Rows[0]["Sale_Uom_Factor"].ToString().Trim();
                        gc.sale_uom = ds.Tables[0].Rows[0]["sale_uom"].ToString().Trim();
                        gc.sale_price = ds.Tables[0].Rows[0]["sale_price"].ToString().Trim();
                        gc.min_sale_price = ds.Tables[0].Rows[0]["min_sale_price"].ToString().Trim();
                        gc.Purchase_uom_factor = ds.Tables[0].Rows[0]["Purchase_uom_factor"].ToString().Trim();
                        gc.purchase_uom = ds.Tables[0].Rows[0]["purchase_uom"].ToString().Trim();
                        gc.purchase_price = ds.Tables[0].Rows[0]["purchase_price"].ToString().Trim();
                        gc.mrp = ds.Tables[0].Rows[0]["mrp"].ToString().Trim();
                        gc.stock_uom = ds.Tables[0].Rows[0]["stock_uom"].ToString().Trim();
                        gc.min_levl = ds.Tables[0].Rows[0]["min_levl"].ToString().Trim();
                        gc.reordr_levl = ds.Tables[0].Rows[0]["reordr_levl"].ToString().Trim();
                        gc.negative_stock = ds.Tables[0].Rows[0]["negative_stock"].ToString().Trim();
                        gc.sInv_MainAccount_Name = ds.Tables[0].Rows[0]["sInv_MainAccount_Name"].ToString().Trim();
                        gc.sRet_MainAccount_Name = ds.Tables[0].Rows[0]["sRet_MainAccount_Name"].ToString().Trim();
                        gc.pInv_MainAccount_Name = ds.Tables[0].Rows[0]["pInv_MainAccount_Name"].ToString().Trim();
                        gc.pRet_MainAccount_Name = ds.Tables[0].Rows[0]["pRet_MainAccount_Name"].ToString().Trim();


                        gc.sProducts_barCodeType = ds.Tables[0].Rows[0]["sProducts_barCodeType"].ToString().Trim();
                        gc.sProducts_barCode = ds.Tables[0].Rows[0]["sProducts_barCode"].ToString().Trim();
                        gc.sProducts_GlobalCode = ds.Tables[0].Rows[0]["sProducts_GlobalCode"].ToString().Trim();

                        gc.tax_code_scheme_sales = ds.Tables[0].Rows[0]["tax_code_scheme_sales"].ToString().Trim();
                        gc.tax_code_scheme_purchase = ds.Tables[0].Rows[0]["tax_code_scheme_purchase"].ToString().Trim();
                        gc.service_category = ds.Tables[0].Rows[0]["service_category"].ToString().Trim();

                        gc.tdsdescription = ds.Tables[0].Rows[0]["tdsdescription"].ToString().Trim();

                        gc.Color_Name = ds.Tables[0].Rows[0]["Color_Name"].ToString().Trim();
                        gc.Size_Name = ds.Tables[0].Rows[0]["Size_Name"].ToString().Trim();
                        gc.color_applicable = Convert.ToBoolean(ds.Tables[0].Rows[0]["color_applicable"].ToString().Trim());
                        gc.size_applicable = Convert.ToBoolean(ds.Tables[0].Rows[0]["size_applicable"].ToString().Trim());
                        gc.install_req = ds.Tables[0].Rows[0]["install_req"].ToString().Trim();
                        gc.is_old_unt = ds.Tables[0].Rows[0]["is_old_unt"].ToString().Trim();
                        gc.product_quantity = ds.Tables[0].Rows[0]["product_quantity"].ToString().Trim();
                        gc.packing_quantity = ds.Tables[0].Rows[0]["packing_quantity"].ToString().Trim();
                        gc.select_uom = ds.Tables[0].Rows[0]["select_uom"].ToString().Trim();
                        gc.Brand_Name = ds.Tables[0].Rows[0]["Brand_Name"].ToString().Trim();
                        gc.Reorder_Quantity = ds.Tables[0].Rows[0]["Reorder_Quantity"].ToString().Trim();
                    
                        _Components = (from DataRow dr in ds.Tables[1].Rows
                                       select new Components()
                                  {
                                      product_code = Convert.ToString(dr["product_code"]),
                                      product_name = Convert.ToString(dr["product_name"]),
                                    



                                  }).ToList();

                        _documentdtls = (from DataRow dr in ds.Tables[2].Rows
                                         select new DocumentDetailsProduct()
                                         {
                                             doc_type = Convert.ToString(dr["doc_type"]),
                                             doc_name = Convert.ToString(dr["doc_name"]),
                                             note1 = Convert.ToString(dr["note1"]),
                                             note2 = Convert.ToString(dr["note2"]),
                                             fileno = Convert.ToString(dr["fileno"]),
                                             building = Convert.ToString(dr["building"]),
                                             upload_by = Convert.ToString(dr["upload_by"]),
                                             verified_by = Convert.ToString(dr["verified_by"]),
                                             doc_loc = Convert.ToString(dr["doc_loc"]),
                                             receive_date = Convert.ToString(dr["receive_date"]),
                                             renew_date = Convert.ToString(dr["renew_date"]),
                                             doc_source = Convert.ToString(dr["doc_source"]),
                                             doc = Convert.ToString(dr["doc"]),
                                         }).ToList();


                        return_obj._header = gc;
                        return_obj.Components_dtls =_Components;
                        return_obj.doc_details = _documentdtls;
                        return_obj.msg = "ok";
                    }
                   
                }
            }
            catch (Exception ex)
            {
                return_obj.msg = ex.Message;
            }


            return return_obj;
        }
    }
    


    public class ProductHeader
    {
        //public int InvoiceId { get; set; }
        public string sProducts_Code { get; set; }
        public string sProducts_Name { get; set; }
        public string sProducts_Description { get; set; }
        public string Inventory_item { get; set; }
        public string Service_item { get; set; }
        public string sProducts_Type { get; set; }
        public string stck_val_tech { get; set; }
        public string class_name { get; set; }
        public string status { get; set; }
        public string sProducts_HsnCode { get; set; }
        public string hsn_desc { get; set; }
        public bool furtherence_to_buisness { get; set; }
        public string Quote_currency { get; set; }
        public string uom_factor { get; set; }
        public string Quote_uom { get; set; }
        public string Capital_goods { get; set; }
        public string Sale_Uom_Factor { get; set; }
        public string sale_uom { get; set; }
        public string sale_price { get; set; }
        public string min_sale_price { get; set; }
        public string Purchase_uom_factor { get; set; }
        public string purchase_uom { get; set; }
        public string purchase_price { get; set; }
        public string mrp { get; set; }
        public string stock_uom { get; set; }
        public string min_levl { get; set; }
        public string reordr_levl { get; set; }
        public string negative_stock { get; set; }
       
        public string sInv_MainAccount_Name { get; set; }
        public string sRet_MainAccount_Name { get; set; }
        public string pInv_MainAccount_Name { get; set; }
        public string pRet_MainAccount_Name { get; set; }

        public string sProducts_barCodeType { get; set; }

        public string sProducts_barCode { get; set; }
        public string sProducts_GlobalCode { get; set; }

        public string tax_code_scheme_sales { get; set; }
        public string tax_code_scheme_purchase { get; set; }
        public string service_category { get; set; }

        public string tdsdescription { get; set; }

        public string Color_Name { get; set; }
        public string Size_Name { get; set; }
        public bool color_applicable { get; set; }
        public bool size_applicable { get; set; }
        public string Brand_Name { get; set; }
        public string install_req { get; set; }
        public string is_old_unt { get; set; }
        public string product_quantity { get; set; }
        public string packing_quantity { get; set; }
        public string select_uom { get; set; }
        public string Reorder_Quantity { get; set; }


    }

    public class Components
    {
        public string product_code { get; set; }
        public string product_name { get; set; }

    }

    public class DocumentDetailsProduct
    {
        public string doc_type { get; set; }
        public string doc_name { get; set; }
        public string note1 { get; set; }
        public string note2 { get; set; }
        public string fileno { get; set; }
        public string building { get; set; }
        public string doc_loc { get; set; }
        public string upload_by { get; set; }
        public string verified_by { get; set; }
        public string receive_date { get; set; }
        public string renew_date { get; set; }
        public string doc_source { get; set; }
        public string doc { get; set; }
    }

    public class ProductDetails
    {
        public string msg { get; set; }
        public ProductHeader _header { get; set; }
        public List<Components> Components_dtls { get; set; }
        public List<DocumentDetailsProduct> doc_details { get; set; }

    }

    
}
