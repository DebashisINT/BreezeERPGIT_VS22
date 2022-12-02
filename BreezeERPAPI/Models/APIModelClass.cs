using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Xml;

using System.IO;

using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
namespace BreezeERPAPI.Models
{
    #region CountryList
    public class countyryListRespose
    {
        public string ResponseCode { get; set; }
        public string Responsedetails { get; set; }
        public List<CountryList> country_list { get; set; }
    }
    public class CountryList
    {

        public int cou_id { get; set; }
        public string cou_country { get; set; }

    }



    #endregion

    #region StateList
    public class StateListResponse
    {
        public string ResponseCode { get; set; }
        public string Responsedetails { get; set; }
        public string isUpdated { get; set; }
        public List<StateList> state_list { get; set; }
    }
    public class StateList
    {
        public int state_id { get; set; }
        public string state_name { get; set; }
        public int country_id { get; set; }
    }



    #endregion

    #region CityList
    public class CityListResponse
    {
        public string ResponseCode { get; set; }
        public string Responsedetails { get; set; }
        public string isUpdated { get; set; }
        public List<CityList> city_list { get; set; }
    }
    public class CityList
    {

        public int city_id { get; set; }
        public string city_name { get; set; }
        public int state_id { get; set; }

    }



    #endregion


    #region PincodeList
    public class PincodeListResponse
    {
        public string ResponseCode { get; set; }
        public string Responsedetails { get; set; }
        public List<PincodeList> pincodelist { get; set; }
    }
    public class PincodeList
    {
        public int pincode_id { get; set; }
        public string pincode_no { get; set; }
        public decimal city_id { get; set; }
    }

    #endregion


    #region Customer Add
    public class CustomerInputParameters
    {
        public string mobile_no { get; set; }
        public string alternate_mobile_no { get; set; }
        public string email { get; set; }
        public string pan_number { get; set; }
        public string aadhar_no { get; set; }
        public string cust_name { get; set; }



        public string gender { get; set; }
        public string date_of_birth { get; set; }
        public string block_no { get; set; }
        public string street_no { get; set; }
        public string flat_no { get; set; }
        public string floor { get; set; }
        public string landmark { get; set; }
        public string country { get; set; }
        public string state { get; set; }

        public string city { get; set; }
        public string pin_code { get; set; }
        public string sales_man_id { get; set; }

        public string isSendSms { get; set; }
        public string lead_source_id { get; set; }
        public string professsion_id { get; set; }
        public string assign_to_id { get; set; }

        public string Token { get; set; }

        public string lead_type_id { get; set; }
        public string req_id_list { get; set; }


    }
    public class CustomerOutputParameters
    {
        public string ResponseCode { get; set; }
        public string Responsedetails { get; set; }
        public string customer_id { get; set; }
        public string customer_name { get; set; }



    }


    #endregion

    #region Customer Update
    public class CustomerupdateInputParameters
    {
        public string mobile_no { get; set; }
        public string alternate_mobile_no { get; set; }
        public string email { get; set; }
        public string pan_number { get; set; }
        public string aadhar_no { get; set; }
        public string cust_name { get; set; }



        public string gender { get; set; }
        public string date_of_birth { get; set; }
        public string block_no { get; set; }
        public string street_no { get; set; }
        public string flat_no { get; set; }
        public string floor { get; set; }
        public string landmark { get; set; }
        public string country { get; set; }
        public string state { get; set; }

        public string city { get; set; }
        public string pin_code { get; set; }
        public string sales_man_id { get; set; }
        public string customer_id { get; set; }


        public string isSendSms { get; set; }
        public string lead_source_id { get; set; }
        public string professsion_id { get; set; }
        public string assign_to_id { get; set; }

        public string Token { get; set; }
        public string lead_type_id { get; set; }
        public string req_id_list { get; set; }

    }

    public class CustomerUpdateOutputParameters
    {
        public string ResponseCode { get; set; }
        public string Responsedetails { get; set; }




    }

    #endregion

    #region Product List
    public class ProductListOutput
    {


        public string ResponseCode { get; set; }
        public string Responsedetails { get; set; }

        public int totalcount { get; set; }
        public List<ProductDetails> product_details { get; set; }


    }

    public class ProductDetails
    {
        public long product_id { get; set; }
        public string product_name { get; set; }

        public decimal product_price { get; set; }
        public string product_small_description { get; set; }
        public string product_full_desc { get; set; }
        public string product_service_desc { get; set; }
        public string product_brand_name { get; set; }

        public string product_category_name { get; set; }
        public int product_brand_id { get; set; }
        public int product_category_id { get; set; }

        public decimal product_min_price { get; set; }

        public decimal product_max_price { get; set; }
        public string product_image { get; set; }
        public string product_image_width { get; set; }
        public string product_image_height { get; set; }
        public string unit { get; set; }
        public string piece_unit { get; set; }
        public decimal muliplying_factor { get; set; }
        public bool isPieceVisible { get; set; }
        public bool isQuantityCalculated { get; set; }
    }

    #endregion

    #region Product Search
    public class ProductsearchInput
    {
        public string product_name { get; set; }
    }

    public class ProductsearchOutput
    {
        public string ResponseCode { get; set; }
        public string Responsedetails { get; set; }
        public int totalcount { get; set; }
        public List<ProductDetails> product_details { get; set; }
    }

    #endregion

    #region Brand List
    public class Brandlistoutput
    {
        public string ResponseCode { get; set; }
        public string Responsedetails { get; set; }
        public int totalcount { get; set; }
        public List<Brands> brand_details { get; set; }
    }

    public class Brands
    {
        public int brand_id { get; set; }
        public string brand_name { get; set; }
        public string brand_details { get; set; }

    }


    #endregion

    #region Category List
    public class Categorylistoutput
    {
        public string ResponseCode { get; set; }
        public string Responsedetails { get; set; }
        public int totalcount { get; set; }
        public List<CategoryClass> category_details { get; set; }
    }

    public class CategoryClass
    {
        public int category_id { get; set; }
        public string category_name { get; set; }


    }


    #endregion

    #region Basket Add

    public class BasketInputParameters
    {

        public string product_id { get; set; }
        public string product_price { get; set; }
        public string customer_id { get; set; }
        public string quantity { get; set; }
        public string salesman_id { get; set; }

        public string Token { get; set; }
        public string discount_percent { get; set; }
        public decimal piece_quantity { get; set; }

    }
    public class BasketOutputParameters
    {
        public string ResponseCode { get; set; }
        public string Responsedetails { get; set; }




    }


    #endregion

    #region  Sale on Progress

    public class saleProgressOutputpara
    {
        public string ResponseCode { get; set; }
        public string Responsedetails { get; set; }
        public int totalcount { get; set; }
        public List<Customer> sale_on_progress_list { get; set; }

    }

    public class Customer
    {
        public string customer_id { get; set; }

        public string mobile_no { get; set; }

        public string alternate_mobile_no { get; set; }
        public string email { get; set; }

        public string pan_number { get; set; }

        public string aadhar_no { get; set; }
        public string cust_name { get; set; }

        public int gender { get; set; }

        public string date_of_birth { get; set; }

        public string block_no { get; set; }
        public string street_no { get; set; }
        public string flat_no { get; set; }
        public string floor { get; set; }
        public string landmark { get; set; }
        public int country { get; set; }
        public int state { get; set; }

        public int city { get; set; }
        public string pin_code { get; set; }

        public bool has_basket { get; set; }

        public string customer_doj { get; set; }
        public long temp_unique_id { get; set; }

        public string isSendSms { get; set; }

        public string lead_source_id { get; set; }
        public string lead_source_name { get; set; }
        public string profession_id { get; set; }
        public string profession_name { get; set; }
        public string assign_to_id { get; set; }
        public string assign_to_name { get; set; }
        public string lead_type_id { get; set; }
        public string lead_type_name { get; set; }
        public List<requirement> req_list { get; set; }
    }




    public class saleProgressSearchInputpara
    {
        public string Token { get; set; }
        public string user_id { get; set; }
        public string customer_name { get; set; }
        public string pageno { get; set; }
        public string rowcount { get; set; }


    }

    #endregion

    #region  Customer basket Show

    public class Customerbasketviewoutput
    {
        public string ResponseCode { get; set; }
        public string Responsedetails { get; set; }

        public long RequestId { get; set; }
        public List<productbasket> customer_basket_details { get; set; }


    }
    public class productbasket
    {

        public int product_id { get; set; }
        public string product_name { get; set; }
        public decimal product_price { get; set; }
        public string product_small_description { get; set; }
        public decimal product_quantity { get; set; }
        public decimal total_price { get; set; }
        public bool has_disc_applied { get; set; }
        public decimal price_after_discount { get; set; }
        public decimal discount_percent { get; set; }
        public string product_unit { get; set; }
        public string piece_unit { get; set; }
        public decimal muliplying_factor { get; set; }
        public bool isPieceVisible { get; set; }
        public decimal piece_quantity { get; set; }
        public bool isQuantityCalculated { get; set; }
    }

    #endregion

    #region Basket Delete customerwise


    public class Basketdeleteinputparameter
    {
        public string Token { get; set; }
        public string customer_id { get; set; }

        public string product_id { get; set; }
        public string sales_man_id { get; set; }

    }

    public class Basketdeleteoutputparameter
    {
        public string ResponseCode { get; set; }
        public string Responsedetails { get; set; }



    }

    #endregion

    #region Customer  Delete


    public class Customerdeleteinputparameter
    {
        public string Token { get; set; }
        public string user_id { get; set; }
        public string customer_id { get; set; }
        public string temp_unique_id { get; set; }

        public string[] temp_unique_id_list { get; set; }
    }

    public class Customerdeleteoutputparameter
    {
        public string ResponseCode { get; set; }
        public string Responsedetails { get; set; }
    }

    #endregion

    #region FinancerList
    public class FinacOutput
    {
        public string ResponseCode { get; set; }
        public string Responsedetails { get; set; }
        public List<finace> financer_details { get; set; }

    }

    public class finace
    {

        public string financer_name { get; set; }
        public int financer_id { get; set; }
    }
    #endregion

    #region Sales on progress - View customer basket- Send Discount Request

    public class SenddiscountSalesonprogressInput
    {

        public string customer_id { get; set; }
        public string user_id { get; set; }
        public string request_type { get; set; }
        public string payment_type { get; set; }

        public string exchange_amount { get; set; }
        public string financer_id { get; set; }
        public string Token { get; set; }
        public string RequestId { get; set; }
        public List<Productsrequestdetails> request_details { get; set; }

        public List<ProductsOldunitrequestdetails> oldunit_details { get; set; }
    }

    public class Productsrequestdetails
    {
        public string product_id { get; set; }
        public string discount_percentage { get; set; }
        public string product_quantity { get; set; }
        public bool Salesman_Isapplied { get; set; }
        public string discount_amount { get; set; }
        public string final_discount_price { get; set; }
        public string piece_quantity { get; set; }

    }

    public class ProductsOldunitrequestdetails
    {
        public string product_id { get; set; }
        public string quantity { get; set; }
        public string amount { get; set; }

    }


    public class SenddiscountSalesonprogressOuput
    {

        public string ResponseCode { get; set; }
        public string Responsedetails { get; set; }
        public int temp_request_id { get; set; }



    }

    #endregion

    #region  Billing Request with Address

    public class BillingDetailscustomerInput
    {
        public string temp_request_id { get; set; }
        public string delivery_option_type { get; set; }

        public string is_delivery_address_same { get; set; }
        public string block_no { get; set; }
        public string street_no { get; set; }
        public string flat_no { get; set; }
        public string floor { get; set; }
        public string landmark { get; set; }
        public string country { get; set; }

        public string state { get; set; }

        public string city { get; set; }

        public string pin_code { get; set; }

        public string Token { get; set; }
        public string delivery_date { get; set; }

        public string gstin { get; set; }
        public string branch_id { get; set; }
        public string Customer_name { get; set; }
        public string paymentType { get; set; }

    }
    #endregion

    #region View Customer Approval

    public class CustomerlistforapprovalInput
    {

        public string user_id { get; set; }
        public string Token { get; set; }
        public string customer_name { get; set; }
        public string discount_requested_status { get; set; }
        public int pageno { get; set; }
        public int rowcount { get; set; }

    }

    public class CustomerlistforapprovalOutput
    {
        public string ResponseCode { get; set; }
        public string Responsedetails { get; set; }
        public List<Customerapprove> customer_approval_details { get; set; }
    }
    public class Customerapprove
    {
        public string customer_id { get; set; }
        public string customer_name { get; set; }
        public string customer_doj { get; set; }



        public string mobile_no { get; set; }

        public string alternate_mobile_no { get; set; }
        public string email { get; set; }

        public string pan_number { get; set; }

        public string aadhar_no { get; set; }


        public int gender { get; set; }

        public string date_of_birth { get; set; }

        public string block_no { get; set; }
        public string street_no { get; set; }
        public string flat_no { get; set; }
        public string floor { get; set; }
        public string landmark { get; set; }
        public int country { get; set; }
        public int state { get; set; }

        public int city { get; set; }
        public string pin_code { get; set; }

        public long RequestId { get; set; }

        public string DiscountApprovedStatus { get; set; }

    }
    #endregion

    #region View Discount Approval

    public class Salesmandiscountapproval
    {
        public string ResponseCode { get; set; }
        public string Responsedetails { get; set; }
        public long RequestId { get; set; }
        public decimal exchange_amount { get; set; }
        public string payment_type { get; set; }
        public List<productfordiscountsalesman> view_approval_details { get; set; }


    }

    public class productfordiscountsalesman
    {
        public int product_id { get; set; }
        public string product_name { get; set; }
        public decimal product_price { get; set; }
        public string product_small_description { get; set; }
        public decimal product_quantity { get; set; }
        public decimal total_price { get; set; }
        public decimal request_for_discount { get; set; }
        public decimal approved_discount { get; set; }
        public decimal price_after_discount { get; set; }
        public bool Salesman_Isapplied { get; set; }

    }

    #endregion

    #region View Discount Approval- Send Request
    public class ViewsendRequestCustomer
    {
        public string customer_id { get; set; }
        public string user_id { get; set; }
        public string request_type { get; set; }
        public string exchange_amount { get; set; }
        public string payment_type { get; set; }
        public string financer_id { get; set; }
        public string[] product_ids { get; set; }
        public string Token { get; set; }
        public string RequestId { get; set; }

        public List<ProductsOldunitrequestdetails> oldunit_details { get; set; }


    }




    public class SDiscountApprovalSendRequestOutput
    {
        public string ResponseCode { get; set; }
        public string Responsedetails { get; set; }
        public string temp_request_id { get; set; }
    }
    #endregion

    #region Sales Manager Discount Request View

    public class ViewDiscountRequestInput
    {
        public string user_id { get; set; }
        public string request_status { get; set; }
        public string Isappliedtop { get; set; }
        public string user_type { get; set; }
        public string Token { get; set; }
    }
    public class ViewDiscountRequestOutput
    {
        public string ResponseCode { get; set; }
        public string Responsedetails { get; set; }

        public List<DiscountrequestList> disc_request_list { get; set; }
    }

    public class DiscountrequestList
    {
        public string customer_id { get; set; }
        public string customer_name { get; set; }
        public string salesman_name { get; set; }
        public string salesman_id { get; set; }
        public string discount_requested_date { get; set; }
        public string discount_requested_status { get; set; }
        public long Requestid { get; set; }
        public bool is_approved_by_top { get; set; }

    }
    #endregion

    #region Sales Manager Discount Request(s) - details

    public class SalesmandiscountRequestProductInput
    {
        public string user_id { get; set; }
        public string customer_id { get; set; }
        public string salesman_id { get; set; }
        public string Token { get; set; }

        public string Requestid { get; set; }
    }
    public class SalesmandiscountRequestProductOutput
    {
        public string ResponseCode { get; set; }
        public string Responsedetails { get; set; }
        public long Requestid { get; set; }
        public decimal exchange_amount { get; set; }
        public List<Discountsalesman> discount_request_details { get; set; }

    }

    public class Discountsalesman
    {
        public int product_id { get; set; }
        public string product_name { get; set; }
        public decimal product_price { get; set; }

        public decimal product_discount_percent { get; set; }

        public string product_small_description { get; set; }

        public decimal product_quantity { get; set; }
        public decimal total_price { get; set; }
        public bool Salesman_Isapplied { get; set; }

        public decimal discount_amount { get; set; }

        public decimal min_discount_applied { get; set; }

    }
    #endregion

    #region Sales  Manager Discount Request(s) - Approve / Reject

    public class DiscountApprovalInput
    {
        public string customer_id { get; set; }

        public string request_type { get; set; }
        public string user_id { get; set; }
        public string Token { get; set; }
        public string sales_man_id { get; set; }
        public string Requestid { get; set; }

        public string Isappliedtop { get; set; }
        public List<Productsrequestdetails> request_details { get; set; }
    }
    #endregion

    #region Finance Request List

    public class FinanceRequestOutput
    {
        public string ResponseCode { get; set; }
        public string Responsedetails { get; set; }
        public List<FinanceRequest> finance_request_list { get; set; }
        public int total_count { get; set; }
    }

    public class FinanceRequest
    {
        public long finance_req_id { get; set; }
        public string customer_id { get; set; }
        public string customer_name { get; set; }

        public int salesman_id { get; set; }
        public string salesman_name { get; set; }
        public string finance_requested_date { get; set; }
        public string finance_requested_status { get; set; }
        public string finance_approval_no { get; set; }
        public string total_amount { get; set; }
        public string loan_amount { get; set; }
        public string processing_fee { get; set; }
        public string other_charges { get; set; }
        public string finance_scheme { get; set; }
        public string dbd_no { get; set; }
        public string downpayment { get; set; }
        public string no_of_emi { get; set; }
        public string emi_amount { get; set; }

    }
    #endregion

    #region View Sales Man

    public class SalesmanagersalesmanRequestOutput
    {
        public string ResponseCode { get; set; }
        public string Responsedetails { get; set; }
        public List<SalesmanListManger> salesman_list { get; set; }
        public int total_count { get; set; }
    }

    public class SalesmanListManger
    {
        public string salesman_name { get; set; }
        public string salesman_id { get; set; }
        public string salesman_mobile_no { get; set; }
        public string salesman_branch_name { get; set; }



    }
    #endregion

    #region  Finance Request Details(Salesman and Financer)

    public class FinanceRequestDetailssalesfinanceOutput
    {
        public string ResponseCode { get; set; }
        public string Responsedetails { get; set; }
        public string exchange_amount { get; set; }
        public List<productbasketforFinanceRequest> view_finance_details { get; set; }


    }
    public class productbasketforFinanceRequest
    {
        public int product_id { get; set; }
        public string product_name { get; set; }
        public decimal product_price { get; set; }
        public string product_small_description { get; set; }
        public decimal product_quantity { get; set; }
        public decimal total_price { get; set; }
        public decimal request_for_discount { get; set; }
        public decimal approved_discount { get; set; }
        public decimal price_after_discount { get; set; }
        public bool Salesman_Isapplied { get; set; }



    }


    #endregion

    #region Finance Request Details accept By Financer

    public class FinancedetailsAcceptInput
    {
        public string finance_userid { get; set; }
        public string finance_req_id { get; set; }
        public string total_amount { get; set; }
        public string finance_approval_no { get; set; }
        public string Token { get; set; }
        public string loan_amount { get; set; }
        public string processing_fee { get; set; }
        public string other_charges { get; set; }
        public string finance_scheme { get; set; }
        public string dbd_no { get; set; }
        public string downpayment { get; set; }
        public string no_of_emi { get; set; }
        public string emi_amount { get; set; }
        public string reason { get; set; }
        public string branch_id { get; set; }
        public string Customer_name { get; set; }

    }

    #endregion

    #region View Finance Status Customer

    public class ViewFinanceStatusCustomerOutput
    {
        public string ResponseCode { get; set; }
        public string Responsedetails { get; set; }
        public List<FinanceRequestCustomer> finance_status_details { get; set; }
    }
    public class FinanceRequestCustomer
    {
        public long finance_req_id { get; set; }
        public string customer_id { get; set; }
        public string customer_name { get; set; }
        public string customer_mobile_no { get; set; }
        public string salesman_id { get; set; }
        public string finance_requested_date { get; set; }
        public string finance_requested_status { get; set; }

        public List<FinanceRequestFinance> financer_list { get; set; }
    }
    public class FinanceRequestFinance
    {
        public string financer_name { get; set; }
        public string financer_id { get; set; }
        public string rejected_reason { get; set; }
        public string finance_status { get; set; }
        public string finance_status_date { get; set; }



        public bool Is_financer_to_apply { get; set; }

    }


    #endregion

    #region  View Finance Status (Apply Again)
    public class Viewfinancestatusinput
    {
        public string user_id { get; set; }
        public string customer_id { get; set; }
        public string finance_request_id { get; set; }
        public string financer_id { get; set; }
        public string Token { get; set; }
    }



    #endregion

    #region Notification Sales man
    public class ViewNotificationsales
    {
        public string ResponseCode { get; set; }
        public string Responsedetails { get; set; }
        public List<ViewNotificationsalesdiscount> notification_details { get; set; }
    }
    public class ViewNotificationsalesdiscount
    {
        public string customer_id { get; set; }
        public string customer_name { get; set; }
        public string discount_requested_date { get; set; }
        public string discount_requested_status { get; set; }
        public string notification_read_status { get; set; }
        public long notification_id { get; set; }

        public string request_type { get; set; }
        public string mobile_no { get; set; }
    }
    #endregion


    #region Notification Sales  Manager
    public class ViewNotificationsalesmanager
    {
        public string ResponseCode { get; set; }
        public string Responsedetails { get; set; }
        public List<ViewNotificationsalesmanagerdiscount> notification_details { get; set; }
    }
    public class ViewNotificationsalesmanagerdiscount
    {
        public string customer_id { get; set; }
        public string customer_name { get; set; }
        public string discount_requested_date { get; set; }
        public string discount_requested_status { get; set; }
        public string notification_read_status { get; set; }
        public long notification_id { get; set; }

        public int salesman_id { get; set; }
        public string Salesman_name { get; set; }
    }
    #endregion


    #region Notification Financer
    public class ViewNotificationfinancerOutput
    {
        public string ResponseCode { get; set; }
        public string Responsedetails { get; set; }
        public List<ViewNotificationfinance> notification_list { get; set; }
    }
    public class ViewNotificationfinance
    {
        public string customer_id { get; set; }
        public string customer_name { get; set; }
        public long finance_req_id { get; set; }
        public int salesman_id { get; set; }
        public string salesman_name { get; set; }
        public string finance_requested_date { get; set; }
        public string finance_requested_status { get; set; }
        public string finance_approval_no { get; set; }
        public decimal total_amount { get; set; }
        public decimal loan_amount { get; set; }
        public decimal processing_fee { get; set; }
        public decimal other_charges { get; set; }
        public string finance_scheme { get; set; }
        public string dbd_no { get; set; }
        public decimal downpayment { get; set; }
        public decimal emi_amount { get; set; }
        public string notification_read_status { get; set; }
    }
    #endregion

    #region Pushnotification
    public class AndroidFCMPushNotificationStatus
    {
        public bool Successful
        {
            get;
            set;
        }

        public string Response
        {
            get;
            set;
        }
        public Exception Error
        {
            get;
            set;
        }
    }
    #endregion

    #region Login Logout
    public class UserLoginInputParameters
    {
        public string user_name { get; set; }
        public string password { get; set; }

        public string device_id { get; set; }

        public string device_type { get; set; }
        public string Token { get; set; }

        public string Imei_no { get; set; }
    }

    public class UserLoginOutputParameters
    {
        public string ResponseCode { get; set; }
        public string Responsedetails { get; set; }

        public string userId { get; set; }
        public string user_type { get; set; }
        public int? user_country_id { get; set; }
        public int? user_state_id { get; set; }

        public int? user_city_id { get; set; }

        public string notification_count { get; set; }

        public int User_login_Id { get; set; }

        public string session_token { get; set; }
        public string Logintype { get; set; }
        public string full_name { get; set; }
        public int user_branchId { get; set; }

        public string sms_text { get; set; }
        public bool isOneFieldOptional { get; set; }
        public string view_customer_list_text { get; set; }

        public bool will_show_team { get; set; }

    }


    #endregion



    #region IMEI
    public class IMEIClass
    {
        public string ResponseCode { get; set; }
        public string Responsedetails { get; set; }

        public string isPresent { get; set; }
        public string userids { get; set; }
    }
    #endregion


    #region Company Logo
    public class CompanyLogoclass
    {
        public string ResponseCode { get; set; }
        public string Responsedetails { get; set; }
        public string logo_url { get; set; }
    }
    #endregion
    public class ErrorModel
    {
        public string ResponseCode { get; set; }
        public string Responsedetails { get; set; }
    }

    public class UserLogOutputParameters
    {
        public string ResponseCode { get; set; }
        public string Responsedetails { get; set; }



    }
    public class Commonclass
    {
        public string ResponseCode { get; set; }
        public string Responsedetails { get; set; }
    }



    #region New List For 3 New Feild in TAB
    public class LeadSourceList
    {
        public string ResponseCode { get; set; }
        public string Responsedetails { get; set; }

        public List<LeadSource> lead_source_list { get; set; }
    }

    public class LeadSource
    {
        public string lead_id { get; set; }
        public string lead_value { get; set; }
    }


    public class ProffessionList
    {
        public string ResponseCode { get; set; }
        public string Responsedetails { get; set; }

        public List<Proffession> professsion_list { get; set; }
    }

    public class Proffession
    {
        public string prof_id { get; set; }
        public string prof_value { get; set; }
    }


    public class AssignToList
    {
        public string ResponseCode { get; set; }
        public string Responsedetails { get; set; }

        public List<Assigns> assigned_to_list { get; set; }
    }

    public class Assigns
    {
        public string assigned_to_id { get; set; }
        public string assigned_to_value { get; set; }
    }





    #endregion

    #region Start Activity From TAB

    public class CustomerList
    {
        public string ResponseCode { get; set; }
        public string Responsedetails { get; set; }
        public string totalcount { get; set; }

        public List<AllCustomer> customer_list { get; set; }
    }



    public class LeadTypeList
    {
        public string ResponseCode { get; set; }
        public string Responsedetails { get; set; }

        public List<LeadType> lead_type_list { get; set; }
    }


    public class ProductAndGroupList
    {
        public string ResponseCode { get; set; }
        public string Responsedetails { get; set; }
        public string total_count { get; set; }

        public List<ProductGroupList> prod_grp_list { get; set; }


    }

    public class ProductGroupList
    {
        public string id { get; set; }
        public string item { get; set; }
        public string type { get; set; }
        public string hierarchy { get; set; }
        public string price { get; set; }
        public string small_description { get; set; }
        public string img { get; set; }
        public string image_width { get; set; }
        public string image_height { get; set; }
        public string unit { get; set; }
        public string piece_unit { get; set; }
        public string muliplying_factor { get; set; }
        public bool isPieceVisible { get; set; }
        public bool isQuantityCalculated { get; set; }
    }












    public class LeadType
    {
        public string id { get; set; }
        public string type { get; set; }

    }

    public class requirementList
    {
        public string ResponseCode { get; set; }
        public string Responsedetails { get; set; }
        public List<requirement> req_list { get; set; }
    }

    public class requirement
    {
        public string id { get; set; }
        public string req { get; set; }

    }



    public class AllCustomer
    {
        public string id { get; set; }
        public string name { get; set; }

        public string address { get; set; }
        public string phone_no { get; set; }



    }





    public class ActivityList
    {
        public string ResponseCode { get; set; }
        public string Responsedetails { get; set; }
        public List<Activity> activity_list { get; set; }
    }

    public class Activity
    {
        public string id { get; set; }
        public string date { get; set; }

        public string remarks { get; set; }
        public string next_contact_date { get; set; }



    }





    #endregion





    #region HelperMethod
    public class APIHelperMethods
    {

        public static T ToModel<T>(DataTable dt)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();

            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataColumn column in dt.Columns)
                {
                    foreach (PropertyInfo pro in temp.GetProperties())
                    {
                        if (pro.Name == column.ColumnName && dt.Rows[0][column.ColumnName] != DBNull.Value)
                        {
                            try
                            {
                                pro.SetValue(obj, dt.Rows[0][column.ColumnName], null);
                            }
                            catch
                            {

                            }
                        }
                    }
                }
            }

            return obj;
        }

        public static List<T> ToModelList<T>(DataTable dt)
        {
            Type temp = typeof(T);

            List<T> objList = new List<T>();

            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    T obj = Activator.CreateInstance<T>();

                    foreach (DataColumn column in row.Table.Columns)
                    {
                        foreach (PropertyInfo pro in temp.GetProperties())
                        {
                            if (pro.Name == column.ColumnName && row[column.ColumnName] != DBNull.Value)
                            {
                                try
                                {
                                    pro.SetValue(obj, row[column.ColumnName], null);
                                }
                                catch
                                {

                                }
                            }
                        }
                    }

                    objList.Add(obj);
                }
            }

            return objList;
        }


        public static string ConvertToXml<T>(List<T> table, int metaIndex = 0)
        {
            XmlDocument ChoiceXML = new XmlDocument();
            ChoiceXML.AppendChild(ChoiceXML.CreateElement("root"));
            Type temp = typeof(T);

            foreach (var item in table)
            {
                XmlElement element = ChoiceXML.CreateElement("data");

                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    element.AppendChild(ChoiceXML.CreateElement(pro.Name)).InnerText = Convert.ToString(item.GetType().GetProperty(pro.Name).GetValue(item, null));
                }
                ChoiceXML.DocumentElement.AppendChild(element);
            }

            return ChoiceXML.InnerXml.ToString();
        }

    }
    #endregion


    #region Encryption
    public class Encryption
    {
        #region Properties

        private string Password = "3269875";
        private string Salt = "05983654";
        private string HashAlgorithm = "SHA1";
        private int PasswordIterations = 2;
        private string InitialVector = "OFRna73m*aze01xY";
        private int KeySize = 256;

        public string password
        {
            get { return Password; }
        }

        public string salt
        {
            get { return Salt; }
        }

        public string hashAlgo
        {
            get { return HashAlgorithm; }
        }

        public int passwordterations
        {
            get { return PasswordIterations; }
        }

        public string initialvector
        {
            get { return InitialVector; }
        }

        public int keysize
        {
            get { return KeySize; }
        }

        #endregion Properties

        #region Encrypt region

        public string Encrypt(string PlainText)
        {
            if (string.IsNullOrEmpty(PlainText))
                return "";
            byte[] InitialVectorBytes = Encoding.ASCII.GetBytes(initialvector);
            byte[] SaltValueBytes = Encoding.ASCII.GetBytes(salt);
            byte[] PlainTextBytes = Encoding.UTF8.GetBytes(PlainText);
            PasswordDeriveBytes DerivedPassword = new PasswordDeriveBytes(password, SaltValueBytes, hashAlgo, passwordterations);
            byte[] KeyBytes = DerivedPassword.GetBytes(KeySize / 8);
            RijndaelManaged SymmetricKey = new RijndaelManaged();
            SymmetricKey.Mode = CipherMode.CBC;
            byte[] CipherTextBytes = null;
            using (ICryptoTransform Encryptor = SymmetricKey.CreateEncryptor(KeyBytes, InitialVectorBytes))
            {
                using (MemoryStream MemStream = new MemoryStream())
                {
                    using (CryptoStream CryptoStream = new CryptoStream(MemStream, Encryptor, CryptoStreamMode.Write))
                    {
                        CryptoStream.Write(PlainTextBytes, 0, PlainTextBytes.Length);
                        CryptoStream.FlushFinalBlock();
                        CipherTextBytes = MemStream.ToArray();
                        MemStream.Close();
                        CryptoStream.Close();
                    }
                }
            }
            SymmetricKey.Clear();
            return Convert.ToBase64String(CipherTextBytes);
        }

        #endregion Encrypt region

        #region Decrypt Region

        public string Decrypt(string CipherText)
        {
            try
            {
                if (string.IsNullOrEmpty(CipherText))
                    return "";
                byte[] InitialVectorBytes = Encoding.ASCII.GetBytes(initialvector);
                byte[] SaltValueBytes = Encoding.ASCII.GetBytes(salt);
                byte[] CipherTextBytes = Convert.FromBase64String(CipherText);
                PasswordDeriveBytes DerivedPassword = new PasswordDeriveBytes(password, SaltValueBytes, hashAlgo, passwordterations);
                byte[] KeyBytes = DerivedPassword.GetBytes(KeySize / 8);
                RijndaelManaged SymmetricKey = new RijndaelManaged();
                SymmetricKey.Mode = CipherMode.CBC;
                byte[] PlainTextBytes = new byte[CipherTextBytes.Length];
                int ByteCount = 0;
                using (ICryptoTransform Decryptor = SymmetricKey.CreateDecryptor(KeyBytes, InitialVectorBytes))
                {
                    using (MemoryStream MemStream = new MemoryStream(CipherTextBytes))
                    {
                        using (CryptoStream CryptoStream = new CryptoStream(MemStream, Decryptor, CryptoStreamMode.Read))
                        {
                            ByteCount = CryptoStream.Read(PlainTextBytes, 0, PlainTextBytes.Length);
                            MemStream.Close();
                            CryptoStream.Close();
                        }
                    }
                }
                SymmetricKey.Clear();
                return Encoding.UTF8.GetString(PlainTextBytes, 0, ByteCount);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
    }

    #endregion

    #region My Sales
    public class MysalesReportOutput
    {
        public string ResponseCode { get; set; }
        public string Responsedetails { get; set; }
        public Int32 total_sales { get; set; }
        public List<MysalesReportCustomerOutput> customer_approval_details { get; set; }
    }
    public class MysalesReportCustomerOutput
    {
        public string customer_id { get; set; }

        public string mobile_no { get; set; }

        public string alternate_mobile_no { get; set; }
        public string email { get; set; }

        public string pan_number { get; set; }

        public string aadhar_no { get; set; }
        public string cust_name { get; set; }

        public int gender { get; set; }

        public string customer_doj { get; set; }


        public int country { get; set; }
        public int state { get; set; }

        public int city { get; set; }
        public string pin_code { get; set; }


        public long RequestId { get; set; }
    }
    #endregion


    #region Change Password


    public class ChangePasswordInput
    {
        public string User_id { get; set; }
        public string Old_password { get; set; }
        public string New_password { get; set; }
        public string Token { get; set; }
    }


    #endregion

    #region Product Old Unit List
    public class ProductListOldunitOutput
    {


        public string ResponseCode { get; set; }
        public string Responsedetails { get; set; }

        public int totalcount { get; set; }
        public List<ProductDetailsOld> productold_details { get; set; }


    }

    public class ProductDetailsOld
    {
        public long product_id { get; set; }
        public string product_name { get; set; }

        public decimal product_min_price { get; set; }

        public decimal product_max_price { get; set; }



    }

    #endregion

    #region Order Details
    public class orderDetailsInput
    {
        public long user_id { get; set; }
        public string session_token { get; set; }
        public string from_date { get; set; }
        public string to_date { get; set; }
        public string Token { get; set; }
    }

    public class OrderDetailsProduct
    {
        public string ResponseCode { get; set; }
        public string Responsedetails { get; set; }
        public List<oredrdetailsMain> order_list { get; set; }
    }

    public class oredrdetailsMain
    {
        public string order_id { get; set; }
        public string order_no { get; set; }
        public string invoice_no { get; set; }
        public string order_date { get; set; }
        public string invoice_date { get; set; }
        public decimal item_no { get; set; }
        public decimal amount { get; set; }
        public string order_owner_name { get; set; }
        public string order_address { get; set; }
        public string order_owner_phn_no { get; set; }
        public decimal invoice_amount { get; set; }
        public List<OrderDetailsProductList> product_list { get; set; }
    }

    public class OrderDetailsProductList
    {
        public long product_id { get; set; }
        public string product_name { get; set; }
        public decimal qty { get; set; }
        public string unit { get; set; }
        public decimal price { get; set; }
    }
    #endregion

    #region Amit new API

    #region Registration
    public class CustomerRegistrationInput
    {
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public String UniqueID { get; set; }
        public String Salutation { get; set; }
        public String Token { get; set; }
    }

    public class CustomerRegistrationOutput
    {
        public String Lead_InternalID { get; set; }
        public String ResponseCode { get; set; }
        public String Responsedetails { get; set; }
    }
    #endregion

    #region Leads User Details

    public class LeadUserDetails
    {
        public String CONTACT_NAME { get; set; }
        public String Lead_InternalID { get; set; }
        public String Profession { get; set; }
        public String Organization { get; set; }
        public String job_responsibility { get; set; }
        public String Designation { get; set; }
        public String Industry { get; set; }
        public String Source { get; set; }
        public String REFER_BY { get; set; }
        public String Rating { get; set; }
        public String MaritalStatus { get; set; }
        public String GENDER { get; set; }
        public String ContactStatus { get; set; }
        public DateTime? DOB { get; set; }
        public DateTime? AnniversaryDate { get; set; }
        public String Education { get; set; }
        public String BloodGroup { get; set; }
        public String Enteredby { get; set; }
        public DateTime EnteredON { get; set; }
        public String ContactPerson { get; set; }
        public String Address1 { get; set; }
        public String Address2 { get; set; }
        public String Address3 { get; set; }
        public String Address4 { get; set; }
        public String Phone { get; set; }
        public String Country { get; set; }
        public String State { get; set; }
        public String City { get; set; }
        public String Pin { get; set; }
        public String EmailID { get; set; }
    }
    #endregion

    #region Product Catalogue

    public class productDetails
    {
        public String ResponseCode { get; set; }
        public String Responsedetails { get; set; }
        public List<productCatalogue> ProductList { get; set; }
    }

    public class productCatalogue
    {
        public String sProducts_ID { get; set; }
        public String sProducts_Code { get; set; }
        public String sProducts_Name { get; set; }
        public String sProducts_Description { get; set; }
        public String ProductColor { get; set; }
        public String ProductSize { get; set; }
        public String Brand { get; set; }
        public String InstallationRequired { get; set; }
        public String ProductSeries { get; set; }
        public String Surface { get; set; }
        public String LeadTime { get; set; }
        public String WEIGHT { get; set; }
        public String SUBCATEGORY { get; set; }
        public String LENGTH { get; set; }
        public String WIDTH { get; set; }
        public String THICKNESS { get; set; }
        public String UOM { get; set; }
        public String CoverageArea { get; set; }
        public String VOLUME { get; set; }

        public String ProductNature_Name { get; set; }
        public String ProductApplication_Name { get; set; }
        public String APPLICATION_NAME { get; set; }
        public String CATEGORY_NAME { get; set; }
        public String MOV_NAME { get; set; }
        public String ProductPedestalNo { get; set; }
        public String ProductCatNo { get; set; }
        public String ProductWarranty { get; set; }
        public List<ComponentsList> Component { get; set; }

    }

    public class ComponentsList
    {
        public String ComponentName { get; set; }
        public String Componentid { get; set; }
        public String Product_id { get; set; }
    }
    #endregion

    #region Cart Product

    public class CartDetailsInput
    {
        public String product_id { get; set; }
        public decimal product_price { get; set; }
        public String customer_id { get; set; }
        public decimal quantity { get; set; }
        public String salesman_id { get; set; }
        public decimal discount_percent { get; set; }
        public decimal piece_quantity { get; set; }
        public String Token { get; set; }
    }

    public class CartDetailsOutPut
    {
        public String CART_ID { get; set; }
        public String ResponseCode { get; set; }
        public String Responsedetails { get; set; }
    }

    public class CartDetailsBatchInput
    {
        public String Token { get; set; }
        public List<CartDetailsBatch> CartDetailsList { get; set; }
    }

    public class CartDetailsBatch
    {
        public String product_id { get; set; }
        public decimal product_price { get; set; }
        public String customer_id { get; set; }
        public decimal quantity { get; set; }
        public String salesman_id { get; set; }
        public decimal discount_percent { get; set; }
        public decimal piece_quantity { get; set; }
    }
    #endregion

    #region Basket Add Order

    public class BasketMain
    {
        public String CustomerId { get; set; }
        public String Token { get; set; }
        public List<BasketDetails> BasketDetailsList { get; set; }
    }

    public class BasketDetails
    {
        public String product_id { get; set; }
        public String ProductName { get; set; }
        public Decimal ProductPrice { get; set; }
        public Decimal ProductQuantity { get; set; }
        public Decimal TotalProductPrice { get; set; }
        public Decimal Finaldiscountedamount { get; set; }
        public Decimal piece_quantity { get; set; }
    }

    public class BasketDetailsOutPut
    {
        public String Order_Awaiting_id { get; set; }
        public String ResponseCode { get; set; }
        public String Responsedetails { get; set; }
    }

    public class CustomerOrderDetaisInput
    {
        public String CustomerId { get; set; }
        public String Token { get; set; }
    }

    public class CustomerOrderDetaisOutPut
    {
        public String ResponseCode { get; set; }
        public String Responsedetails { get; set; }
        public List<OrderDetailsLists> OrderDetailsList { get; set; }
    }

    public class OrderDetailsLists
    {
        public String Order_Number { get; set; }
        public String Order_Date { get; set; }
        public String Customer_Id { get; set; }
        public String CUSTOMER_NAME { get; set; }
        public List<OrderDetailsProductList> ProductList { get; set; }
    }

    public class CustomerOrderCancelInput
    {
        public String OrderNumber { get; set; }
        public String Remarks { get; set; }
        public String Token { get; set; }
    }

    public class OrderDeliveryDetails
    {
        public String Challan_Number { get; set; }
        public DateTime Challan_Date { get; set; }
        public long Challan_Id { get; set; }
        public String Vehicle_No { get; set; }
    }

    public class employeeAddInput
    {
        public String EmployeeCode { get; set; }
        public String userId { get; set; }
        public String ContactType { get; set; }
        public String Salutation { get; set; }
        public String FirstName { get; set; }
        public String MiddileName { get; set; }
        public String LastName { get; set; }
        public String Dob { get; set; }
        public String Gender { get; set; }
        public String doj { get; set; }
        public String Grade { get; set; }
        public String BloodGroup { get; set; }
        public String MaritalStatus { get; set; }
        public String Organization { get; set; }
        public String JobResposibility { get; set; }
        public String Branch { get; set; }
        public String Designation { get; set; }
        public String EmployeeType { get; set; }
        public String ReportTo { get; set; }
        public String AddressTypeResidence { get; set; }
        public String Address1_Res { get; set; }
        public String Address2_Res { get; set; }
        public String Address3_Res { get; set; }
        public String Country_Res { get; set; }
        public String State_res { get; set; }
        public String City_District_Res { get; set; }
        public String Pin_Zip_Res { get; set; }
        public String Phone_type_res { get; set; }
        public String Number_Res { get; set; }
        public String AddressType_off { get; set; }
        public String Address1_off { get; set; }
        public String Address2_off { get; set; }
        public String Address3_off { get; set; }
        public String Country_off { get; set; }
        public String State_off { get; set; }
        public String City_District_Off { get; set; }
        public String Pin_Zip_Off { get; set; }
        public String Phone_type_off { get; set; }
        public String Number_Off { get; set; }
        public String Email_Type { get; set; }
        public String Email_Id { get; set; }
        public String Relationship_1 { get; set; }
        public String Name_1 { get; set; }
        public String RelationShip_2 { get; set; }
        public String Name_2 { get; set; }
        public String Current_Ctc { get; set; }
        public String Pan { get; set; }
        public String Aadhar { get; set; }
        public String Passport { get; set; }
        public String ValidUpTo { get; set; }
        public String Epic { get; set; }
        public String BankName { get; set; }
        public String Account_No { get; set; }
        public String AccountType { get; set; }
        public String Pf_Applicable { get; set; }
        public String Pf_No { get; set; }
        public String Uan { get; set; }
        public String Esi_Applicable { get; set; }
        public String Esi_No { get; set; }
        public String Token { get; set; }
    }

    public class EmployeeAddOutPut
    {
        public String Success { get; set; }
        public String HasLog { get; set; }
        public String Responsedetails { get; set; }
        public String EmployeeCode { get; set; }
    }
    #endregion

    #region Employee Drop down Value
    public class EmployeeDetailsMaster
    {
        public String Responsedetails { get; set; }
        public String ResponseCode { get; set; }
        public List<Branch> BranchList { get; set; }
        public List<Designation> DesignationList { get; set; }
        public List<EmployeeType> EmployeeTypeList { get; set; }
        public List<Organization> OrganizationList { get; set; }
        public List<JobResponsibility> JobResponsibilityList { get; set; }
        public List<WorkingHour> WorkingHourList { get; set; }
        public List<LeavePolicy> LeavePolicyList { get; set; }
        public List<Department> DepartmentList { get; set; }
        public List<salutation> salutationList { get; set; }
    }

    public class salutation
    {
        public String sal_id { get; set; }
        public String sal_name { get; set; }
    }

    public class Branch
    {
        public String branch_id { get; set; }
        public String branch_description { get; set; }
    }

    public class Designation
    {
        public String deg_id { get; set; }
        public String deg_designation { get; set; }
    }
    public class EmployeeType
    {
        public String emptpy_id { get; set; }
        public String emptpy_type { get; set; }
    }
    public class Organization
    {
        public String OrganizationID { get; set; }
        public String OrganizationName { get; set; }
    }
    public class JobResponsibility
    {
        public String job_id { get; set; }
        public String job_responsibility { get; set; }
    }
    public class WorkingHour
    {
        public String Id { get; set; }
        public String Name { get; set; }
    }
    public class LeavePolicy
    {
        public String LeavePolicyID { get; set; }
        public String LeavePolicyName { get; set; }
    }
    public class Department
    {
        public String DepartmentID { get; set; }
        public String DepartmentName { get; set; }
    }
    #endregion

    #region user wise Hierarchy
    public class UserHierarchy
    {
        public String id { get; set; }
        public String name { get; set; }
    }

    public class UserHierarchyOut
    {
        public String ResponseCode { get; set; }
        public String Responsedetails { get; set; }
        public String total_count { get; set; }
        public List<UserHierarchy> team_list { get; set; }
    }
    #endregion
    #endregion
}