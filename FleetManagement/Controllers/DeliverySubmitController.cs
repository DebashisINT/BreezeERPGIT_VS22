using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Xml.Linq;
using System.IO;
using System.Text;
using FleetManagement.Models;

namespace FleetManagement.Controllers
{
    public class DeliverySubmitController : ApiController
    {


        [HttpPost]
        public HttpResponseMessage InsertDeliveryDetails(DEliveryInput model)
        {
            LocationupdateOutput omodel = new LocationupdateOutput();
            try
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

                    List<DeliveryDetailsInput> omedl2 = new List<DeliveryDetailsInput>();
                    List<DeliveryInputproduct> omodelproduct = new List<DeliveryInputproduct>();
                    List<DeliveryInputproduct> product = new List<DeliveryInputproduct>();
                    DeliveryInputproduct modelprod = new DeliveryInputproduct();
                    List<DeliveryDetailsInputDattabase> smodel = new List<DeliveryDetailsInputDattabase>();
                    List<DeliveryExchangeInputDattabase> smodelexchange = new List<DeliveryExchangeInputDattabase>();

                    //foreach (var s3 in omodelproduct)
                    //{
                    //    product.Add(new DeliveryInputproduct()
                    //    {
                    //        delivery_details_id = s3.delivery_details_id,
                    //        status = s3.status,
                    //        delivery_product_name = s3.delivery_product_name,
                    //        delivery_product_id = s3.delivery_product_name,
                    //        delivery_qty = s3.delivery_qty
                    //    });
                    //}

                    //foreach (var s2 in model.delivery_details)
                    //{


                    foreach (var s3 in model.delivery_details.delivery_details_list)
                    {

                        smodel.Add(new DeliveryDetailsInputDattabase()
                        {
                            delivery_id = model.delivery_details.delivery_id,
                            address = model.delivery_details.address,
                            pin_code = model.delivery_details.pin_code,
                            lat = model.delivery_details.lat,
                            longt = model.delivery_details.longt,
                            owner_name = model.delivery_details.owner_name,
                            owner_contact_no = model.delivery_details.owner_contact_no,
                            owner_email = model.delivery_details.owner_email,
                            order_id = model.delivery_details.order_id,
                            delivery_date = model.delivery_details.delivery_date,
                            delivered_on = model.delivery_details.delivered_on,
                            deliver_quantity = model.delivery_details.deliver_quantity,
                            deliver_status = model.delivery_details.deliver_status,
                            delivery_status_id = model.delivery_details.delivery_status_id,
                            delivery_note = model.delivery_details.delivery_note,
                            exchange_note = model.delivery_details.exchange_note,
                            delivery_details_id = s3.delivery_details_id,
                            status = s3.status,
                            delivery_product_name = s3.delivery_product_name,
                            delivery_product_id = s3.delivery_product_id,
                            delivery_qty = s3.delivery_qty,
                            customerid = model.delivery_details.customerid
                        });
                    }



                    foreach (var s4 in model.delivery_details.exchange_old_item_list)
                    {
                        smodelexchange.Add(new DeliveryExchangeInputDattabase()
                        {
                            delivery_id = model.delivery_details.delivery_id,
                            exchange_id = s4.exchange_id,
                            receive_status = s4.receive_status,
                            product_name = s4.product_name,
                            product_id = s4.product_id,
                            customerid = model.delivery_details.customerid,
                            exchange_note = model.delivery_details.exchange_note,
                            qty = s4.qty,
                            delivery_date = model.delivery_details.delivery_date,
                            order_id = model.delivery_details.order_id
                        });

                    }


                    //omedl2.Add(new DeliveryDetailsInput()
                    //{
                    //    delivery_id = s2.delivery_id,
                    //    address = s2.address,
                    //    pin_code = s2.pin_code,
                    //    lat = s2.lat,
                    //    longt = s2.longt,
                    //    owner_name = s2.owner_name,
                    //    owner_contact_no = s2.owner_contact_no,
                    //    owner_email = s2.owner_email,
                    //    order_id = s2.order_id,
                    //    delivery_date = s2.delivery_date,
                    //    delivered_on = s2.delivered_on,
                    //    deliver_quantity = s2.deliver_quantity,
                    //    deliver_status = s2.deliver_status,
                    //    delivery_status_id = s2.delivery_status_id,
                    //    delivery_note = s2.delivery_note,
                    //    exchange_note = s2.exchange_note,
                    //    delivery_details_list = product,

                    //});

                    //}


                    string JsonXML = XmlConversion.ConvertToXml(smodel, 0);
                     string JsonExchangeXML=null;
                    if (model.delivery_details.exchange_old_item_list.Count() == 0)
                    {
                        JsonExchangeXML = null;
                    }
                    else
                    {
                        JsonExchangeXML = XmlConversion.ConvertToXml(smodelexchange, 0);
                    }
                    DataTable dt = new DataTable();
                    String con = System.Configuration.ConfigurationSettings.AppSettings["DBConnectionDefault"];  
                 //   String con = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                    SqlCommand sqlcmd = new SqlCommand();
                    SqlConnection sqlcon = new SqlConnection(con);
                    sqlcon.Open();
                    sqlcmd = new SqlCommand("proc_Fleet_DeliverySubmit", sqlcon);
                    sqlcmd.Parameters.Add("@user_id", model.user_id);
                    sqlcmd.Parameters.Add("@JsonXML", JsonXML);
                    sqlcmd.Parameters.Add("@JsonExchangeXML", JsonExchangeXML);



                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                    da.Fill(dt);
                    sqlcon.Close();
                    if (dt.Rows.Count > 0)
                    {
                        omodel.status = "200";
                        omodel.message = "Delivery details successfully updated.";
                    }
                    else
                    {
                        omodel.status = "202";
                        omodel.message = "Already delivered";
                    }

                    var message = Request.CreateResponse(HttpStatusCode.OK, omodel);
                    return message;

                }

                else
                {
                    omodel.status = "205";
                    omodel.message = "Token Id does not matched.";
                    var message = Request.CreateResponse(HttpStatusCode.OK, omodel);
                    return message;

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

