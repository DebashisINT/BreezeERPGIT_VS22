using FleetManagement.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FleetManagement.Controllers
{
    public class DeliveryListController : ApiController
    {

        [HttpPost]
        public HttpResponseMessage List(DeliveryModelclassInput model)
        {
            DeliveryModelclassOutput omodel = new DeliveryModelclassOutput();
            DeliveryModelclass omodeldata = new DeliveryModelclass();
            try
            {
                if (!ModelState.IsValid)
                {
                    omodel.status = "213";
                    omodel.message = "Some input parameters are missing.";
                    return Request.CreateResponse(HttpStatusCode.BadRequest, omodel);
                }
                else
                {
                    String tokenmatch = System.Configuration.ConfigurationSettings.AppSettings["AuthToken"];
                    System.Net.Http.Headers.HttpRequestHeaders headers = this.Request.Headers;

                    string token = string.Empty;
                    string versionname = string.Empty;
                    if (headers.Contains("version_name"))
                    {
                        versionname = headers.GetValues("version_name").First();
                    }
                    if (headers.Contains("token_Number"))
                    {
                        token = headers.GetValues("token_Number").First();
                    }


                    if (token == tokenmatch)
                    {
                        string sessionId = "";

                        DataTable dt = new DataTable();
                        DataSet ds = new DataSet();

                        List<DeliveryDetails> delivermodel = new List<DeliveryDetails>();
                        List<DeliveryDetails> delivermodel2 = new List<DeliveryDetails>();


                        List<DeliveryDetailsproduct> delivermodelproduct2 = new List<DeliveryDetailsproduct>();

                        String con = System.Configuration.ConfigurationSettings.AppSettings["DBConnectionDefault"]; 
                       // String con = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);

                        SqlCommand sqlcmd = new SqlCommand();
                        SqlConnection sqlcon = new SqlConnection(con);
                        sqlcon.Open();
                        sqlcmd = new SqlCommand("Proc_fleet_deliveryroutemanagement", sqlcon);
                        sqlcmd.Parameters.Add("@session_token", model.session_token);
                        sqlcmd.Parameters.Add("@User_id", model.user_id);
                        sqlcmd.Parameters.Add("@Date", model.date);

                        sqlcmd.CommandType = CommandType.StoredProcedure;
                        SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                        da.Fill(ds);
                        sqlcon.Close();

                        List<DeliveryDetails> datadeliverydet = new List<DeliveryDetails>();
                        if (ds.Tables[0].Rows.Count > 0)
                        {

                            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {
                                List<DeliveryDetailsproduct> delivermodelproduct = new List<DeliveryDetailsproduct>();
                                List<Exchangedetails> delivermodelproductexchange = new List<Exchangedetails>();

                                string delivery_id = "";
                                string address = "";
                                string pin_code = "";
                                string lat = "";
                                string longt = "";
                                string owner_name = "";
                                string owner_contact_no = "";
                                string owner_email = "";
                                string order_id = "";
                                string delivery_date = "";
                                string delivered_on = "";
                                string deliver_quantity = "";
                                string delivery_status_id = "";
                                string deliver_status = "";

                                string delivery_note = "";
                                string exchange_note = "";
                                string delivery_details_id = "";
                                string status = "";
                                string delivery_product_name = "";
                                string delivery_product_id = "";
                                string delivery_qty = "";
                                string customerid = "";

                                int k = i;


                                for (int j = 0; j < ds.Tables[1].Rows.Count; j++)
                                {


                                    int i1 = 0;
                                    if (Convert.ToString(ds.Tables[1].Rows[j]["Customer_Id"]) == Convert.ToString(ds.Tables[0].Rows[i]["Customer_Id"])

                                        && Convert.ToString(ds.Tables[1].Rows[j]["OrderNumber"]) == Convert.ToString(ds.Tables[0].Rows[i]["Doc_Number"])
                                        )
                                    {

                                        delivermodelproduct.Add(new DeliveryDetailsproduct()
                                            {

                                                delivery_details_id = Convert.ToString(ds.Tables[1].Rows[j]["DEliveryDetailsID"]),
                                                status = Convert.ToString(ds.Tables[1].Rows[j]["DeliveryStatus"]),
                                                delivery_product_name = Convert.ToString(ds.Tables[1].Rows[j]["ProductDesc"]),
                                                delivery_product_id = Convert.ToString(ds.Tables[1].Rows[j]["ProductID"]),
                                                delivery_qty = Convert.ToString(ds.Tables[1].Rows[j]["Prodquantity"])

                                            });

                                        //delivery_details_id = Convert.ToString(ds.Tables[1].Rows[j]["DEliveryDetailsID"]);
                                        //status = Convert.ToString(ds.Tables[1].Rows[j]["DeliveryStatus"]);
                                        //delivery_product_name = Convert.ToString(ds.Tables[1].Rows[j]["ProductDesc"]);
                                        //delivery_product_id = Convert.ToString(ds.Tables[1].Rows[j]["ProductID"]);
                                        //delivery_qty = Convert.ToString(ds.Tables[1].Rows[j]["Prodquantity"]);



                                        delivery_id = Convert.ToString(ds.Tables[1].Rows[j]["ID"]);
                                        address = Convert.ToString(ds.Tables[1].Rows[j]["Contactaddress"]);
                                        pin_code = Convert.ToString(ds.Tables[1].Rows[j]["Pincode"]);
                                        lat = Convert.ToString(ds.Tables[1].Rows[j]["latitute"]);
                                        longt = Convert.ToString(ds.Tables[1].Rows[j]["longtude"]);
                                        owner_name = Convert.ToString(ds.Tables[1].Rows[j]["Ownername"]);
                                        owner_contact_no = Convert.ToString(ds.Tables[1].Rows[j]["PhoneNumber"]);
                                        owner_email = Convert.ToString(ds.Tables[1].Rows[j]["EmailID"]);
                                        order_id = Convert.ToString(ds.Tables[1].Rows[j]["OrderNumber"]);
                                        delivery_date = Convert.ToString(ds.Tables[1].Rows[j]["Delievry_Date"]);
                                        delivered_on = Convert.ToString(ds.Tables[1].Rows[j]["delivered_on"]);
                                        deliver_quantity = Convert.ToString(ds.Tables[1].Rows[j]["deliver_quantity"]);
                                        deliver_status = Convert.ToString(ds.Tables[1].Rows[j]["deliver_status"]);
                                        delivery_status_id = Convert.ToString(ds.Tables[1].Rows[j]["delivery_status_id"]);
                                        customerid = Convert.ToString(ds.Tables[1].Rows[j]["Customer_Id"]);
                                        delivery_note = Convert.ToString(ds.Tables[1].Rows[j]["DeliveryNote"]);
                                        exchange_note = Convert.ToString(ds.Tables[1].Rows[j]["DeliveryNote"]);



                                    }




                                }

                                for (int n = 0; n < ds.Tables[2].Rows.Count; n++)
                                {

                                    if (Convert.ToString(ds.Tables[2].Rows[n]["Customer_Id"]) == Convert.ToString(ds.Tables[0].Rows[i]["Customer_Id"])
                                         && Convert.ToString(ds.Tables[2].Rows[n]["Doc_Number"]) == Convert.ToString(ds.Tables[0].Rows[i]["Doc_Number"])
                                        )
                                    {

                                        delivermodelproductexchange.Add(new Exchangedetails()
                                            {

                                                exchange_id = Convert.ToString(ds.Tables[2].Rows[n]["exchange_id"]),
                                                receive_status = Convert.ToString(ds.Tables[2].Rows[n]["receive_status"]),
                                                product_name = Convert.ToString(ds.Tables[2].Rows[n]["product_name"]),
                                                product_id = Convert.ToString(ds.Tables[2].Rows[n]["product_id"]),
                                                qty = Convert.ToString(ds.Tables[2].Rows[n]["qty"])

                                            });
                                    }

                                }
                                //delivermodelproduct.Add(new DeliveryDetailsproduct()
                                //{

                                //    delivery_details_id = delivery_details_id,
                                //    status= status,
                                //    delivery_product_name = delivery_product_name,
                                //    delivery_product_id = delivery_product_id,
                                //    delivery_qty = delivery_qty

                                //});



                                delivermodel.Add(new DeliveryDetails()
                                {

                                    delivery_id = delivery_id,
                                    address = address,
                                    pin_code = pin_code,
                                    lat = lat,
                                    longt = longt,
                                    owner_name = owner_name,
                                    owner_contact_no = owner_contact_no,
                                    owner_email = owner_email,
                                    order_id = order_id,
                                    delivery_date = delivery_date,
                                    delivered_on = delivered_on,
                                    deliver_quantity = deliver_quantity,
                                    deliver_status = deliver_status,
                                    delivery_status_id = delivery_status_id,
                                    delivery_details_list = delivermodelproduct,
                                    exchange_old_item_list = delivermodelproductexchange,
                                    delivery_note = delivery_note,
                                    exchange_note = exchange_note,
                                    customerid = customerid

                                });



                            }

                            omodeldata.delivary_list = delivermodel;

                        }

                        omodel.status = "200";
                        omodel.message = "Delivery List.";
                        omodel.data = omodeldata;

                        var message = Request.CreateResponse(HttpStatusCode.OK, omodel);
                        return message;
                    }

                    else
                    {
                        omodel.status = "205";
                        omodel.message = " Session token does not matched";
                        var message = Request.CreateResponse(HttpStatusCode.OK, omodel);
                        return message;
                    }
                }



            }
            catch (Exception ex)
            {
                omodel.status = "209";
                omodel.message = ex.Message;
                var message = Request.CreateResponse(HttpStatusCode.OK, omodel);
                return message;
            }

        }
    }
}
