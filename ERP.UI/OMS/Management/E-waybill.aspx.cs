using BusinessLogicLayer;
using DataAccessLayer;
using ERP.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web;
using System.Web.Hosting;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ERP.OMS.Management
{
    public partial class E_waybill : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                FormDate.Date = DateTime.Now;
                toDate.Date = DateTime.Now;
                ASPxDateEdit1.Date = DateTime.Now;
                ASPxDateEdit2.Date = DateTime.Now;
                ASPxDateEdit3.Date = DateTime.Now;
                ASPxDateEdit4.Date = DateTime.Now;
                ASPxDateEdit5.Date = DateTime.Now;
                ASPxDateEdit6.Date = DateTime.Now;
                ASPxDateEdit7.Date = DateTime.Now;
                ASPxDateEdit8.Date = DateTime.Now;
                ASPxDateEdit9.Date = DateTime.Now;
                ASPxDateEdit10.Date = DateTime.Now;

                DBEngine objDB = new DBEngine();
                DataTable branchtable = objDB.GetDataTable("SELECT BRANCH_ID,BRANCH_DESCRIPTION from tbl_master_branch where branch_id not in (select BRANCH_ID from dbo.fn_GetEinvoiceBranchDetails())");

                cmbBranchfilter.DataSource = branchtable;
                cmbBranchfilter.ValueField = "branch_id";
                cmbBranchfilter.TextField = "branch_description";
                cmbBranchfilter.DataBind();
                cmbBranchfilter.SelectedIndex = 0;

                brPendinSI.DataSource = branchtable;
                brPendinSI.ValueField = "branch_id";
                brPendinSI.TextField = "branch_description";
                brPendinSI.DataBind();
                brPendinSI.SelectedIndex = 0;


                brTSI.DataSource = branchtable;
                brTSI.ValueField = "branch_id";
                brTSI.TextField = "branch_description";
                brTSI.DataBind();
                brTSI.SelectedIndex = 0;


                brPendingTSI.DataSource = branchtable;
                brPendingTSI.ValueField = "branch_id";
                brPendingTSI.TextField = "branch_description";
                brPendingTSI.DataBind();
                brPendingTSI.SelectedIndex = 0;


                crBR.DataSource = branchtable;
                crBR.ValueField = "branch_id";
                crBR.TextField = "branch_description";
                crBR.DataBind();
                crBR.SelectedIndex = 0;


                brPendingCR.DataSource = branchtable;
                brPendingCR.ValueField = "branch_id";
                brPendingCR.TextField = "branch_description";
                brPendingCR.DataBind();
                brPendingCR.SelectedIndex = 0;

            }

        }

        protected void EntityServerModeDataSource_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "Invoice_Id";

            //string connectionString = ConfigurationManager.ConnectionStrings["crmConnectionString"].ConnectionString;

            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);




            string IsFilter = Convert.ToString(hfIsFilter.Value);
            string strFromDate = Convert.ToString(hfFromDate.Value);
            string strToDate = Convert.ToString(hfToDate.Value);
            string strBranchID = (Convert.ToString(hfBranchID.Value) == "") ? "0" : Convert.ToString(hfBranchID.Value);

            List<int> branchidlist;
            ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
            if (IsFilter == "Y")
            {
                if (strBranchID == "0")
                {
                    string BranchList = Convert.ToString(Session["userbranchHierarchy"]);
                    branchidlist = new List<int>(Array.ConvertAll(BranchList.Split(','), int.Parse));
                    var q = from d in dc.v_EwaybillSIs
                            where d.Invoice_DateTimeFormat >= Convert.ToDateTime(strFromDate) &&
                                  d.Invoice_DateTimeFormat <= Convert.ToDateTime(strToDate) &&
                                  d.Invoice_CompanyID == Convert.ToString(Session["LastCompany"]) &&
                                  branchidlist.Contains(Convert.ToInt32(d.Invoice_branchId)) &&                                  
                                  Convert.ToString(d.EWayBillNumber) != "" 
                            orderby d.Invoice_DateTimeFormat descending
                            select d;
                    e.QueryableSource = q;
                }
                else
                {
                    branchidlist = new List<int>(Array.ConvertAll(strBranchID.Split(','), int.Parse));
                    var q = from d in dc.v_EwaybillSIs
                            where d.Invoice_DateTimeFormat >= Convert.ToDateTime(strFromDate) &&
                                  d.Invoice_DateTimeFormat <= Convert.ToDateTime(strToDate) &&
                                  d.Invoice_CompanyID == Convert.ToString(Session["LastCompany"]) &&
                                  branchidlist.Contains(Convert.ToInt32(d.Invoice_branchId)) &&                                  
                                  Convert.ToString(d.EWayBillNumber) != "" 
                            orderby d.Invoice_DateTimeFormat descending
                            select d;
                    e.QueryableSource = q;
                }
            }
            else
            {
                var q = from d in dc.v_EwaybillSIs
                        where d.Invoice_branchId == '0' &&
                                d.Invoice_CompanyID == Convert.ToString(Session["LastCompany"])
                        orderby d.Invoice_DateTimeFormat descending
                        select d;
                e.QueryableSource = q;
            }
        }
        

        protected void LinqServerModeDataSourcePendingewaybill_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "Invoice_Id";

            //string connectionString = ConfigurationManager.ConnectionStrings["crmConnectionString"].ConnectionString;

            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);




            string IsFilter = Convert.ToString(hfIsFilter.Value);
            string strFromDate = Convert.ToString(hfFromDate.Value);
            string strToDate = Convert.ToString(hfToDate.Value);
            string strBranchID = (Convert.ToString(hfBranchID.Value) == "") ? "0" : Convert.ToString(hfBranchID.Value);

            List<int> branchidlist;
            ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
            if (IsFilter == "Y")
            {
                if (strBranchID == "0")
                {
                    string BranchList = Convert.ToString(Session["userbranchHierarchy"]);
                    branchidlist = new List<int>(Array.ConvertAll(BranchList.Split(','), int.Parse));
                    var q = from d in dc.v_EwaybillSIs
                            where d.Invoice_DateTimeFormat >= Convert.ToDateTime(strFromDate) &&
                                  d.Invoice_DateTimeFormat <= Convert.ToDateTime(strToDate) &&
                                  d.Invoice_CompanyID == Convert.ToString(Session["LastCompany"]) &&
                                  branchidlist.Contains(Convert.ToInt32(d.Invoice_branchId)) &&
                                  Convert.ToString(d.EWayBillNumber) == ""
                            orderby d.Invoice_DateTimeFormat descending
                            select d;
                    e.QueryableSource = q;
                }
                else
                {
                    branchidlist = new List<int>(Array.ConvertAll(strBranchID.Split(','), int.Parse));
                    var q = from d in dc.v_EwaybillSIs
                            where d.Invoice_DateTimeFormat >= Convert.ToDateTime(strFromDate) &&
                                  d.Invoice_DateTimeFormat <= Convert.ToDateTime(strToDate) &&
                                  d.Invoice_CompanyID == Convert.ToString(Session["LastCompany"]) &&
                                  branchidlist.Contains(Convert.ToInt32(d.Invoice_branchId)) &&
                                  Convert.ToString(d.EWayBillNumber) == ""
                            orderby d.Invoice_DateTimeFormat descending
                            select d;
                    e.QueryableSource = q;
                }
            }
            else
            {
                var q = from d in dc.v_EwaybillSIs
                        where d.Invoice_branchId == '0' &&
                                d.Invoice_CompanyID == Convert.ToString(Session["LastCompany"])
                        orderby d.Invoice_DateTimeFormat descending
                        select d;
                e.QueryableSource = q;
            }
        }

        protected void GrdQuotation_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            if(e.Parameters.Split('~')[0]=="Download")
            {
                DBEngine objDB = new DBEngine();
                authtokensOutput authObj = new authtokensOutput();
                GrdQuotation.JSProperties["cpJson"] = "DownloadEwaybill";

                string eWaybillNumber = e.Parameters.Split('~')[1];




                if (DateTime.Now > EinvoiceToken.Expiry)
                {
                    try
                    {
                        using (HttpClient client = new HttpClient())
                        {
                            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls |
                                               SecurityProtocolType.Tls11 |
                                               SecurityProtocolType.Tls12;
                            authtokensInput objI = new authtokensInput("shivkumar@peekay.co.in", "PeekaY@.!_123");
                            var json = JsonConvert.SerializeObject(objI, Formatting.Indented);
                            var stringContent = new StringContent(json);
                            var content = new StringContent(stringContent.ToString(), Encoding.UTF8, "application/json");
                            var response = client.PostAsync("https://sandbox.services.vayananet.com/theodore/apis/v1/authtokens", stringContent).Result;

                            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                var jsonString = response;
                                var data = JsonConvert.DeserializeObject<authtokensOutput>(response.Content.ReadAsStringAsync().Result);
                                authObj = response.Content.ReadAsAsync<authtokensOutput>().Result;
                                EinvoiceToken.token = authObj.data.token;
                                long unixDate = authObj.data.expiry;
                                DateTime start = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                                DateTime date = start.AddMilliseconds(unixDate).ToLocalTime();
                                EinvoiceToken.Expiry = date;
                            }
                        }
                    }
                    catch (AggregateException err)
                    {

                    }
                }

                try
                {
                    IRN objIRN = new IRN();
                    using (var client = new HttpClient())
                    {
                        client.DefaultRequestHeaders.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/pdf"));
                        client.DefaultRequestHeaders.Add("X-FLYNN-N-USER-TOKEN", EinvoiceToken.token);
                        client.DefaultRequestHeaders.Add("X-FLYNN-N-ORG-ID", "d8c333c7-fd42-40d7-b33e-2067db38c8c7");
                        client.DefaultRequestHeaders.Add("X-FLYNN-N-EWB-GSTIN", "29AEKPV7203E1Z9");
                        client.DefaultRequestHeaders.Add("X-FLYNN-N-EWB-USERNAME", "test_dlr519");
                        client.DefaultRequestHeaders.Add("X-FLYNN-N-EWB-PWD", "test_dlr519");
                        client.DefaultRequestHeaders.Add("X-FLYNN-N-EWB-GSP-CODE", "clayfin");
                        var response = client.GetAsync("https://solo.enriched-api.vayana.com/enriched/ewb/v1.0/eway-bills/download-pdf/" + eWaybillNumber).Result;
                        //var file = client.GetStreamAsync("https://live.enriched-api.vayana.com/enriched/tasks/v1.0/download/" + objIRN.data.task_id).Result;
                        //var response = await client.GetAsync(uri);

                        if (response.StatusCode == System.Net.HttpStatusCode.OK)
                        {

                            //using (var fs = new FileStream(
                            //    HostingEnvironment.MapPath(string.Format("~/Commonfolder/{0}.pdf", eWaybillNumber.ToString())),
                            //    FileMode.Create))
                            //{
                            //    response.Content.CopyToAsync(fs);
                            //}

                            System.Net.Http.HttpContent content = response.Content; // actually a System.Net.Http.StreamContent instance but you do not need to cast as the actual type does not matter in this case

                            using (var file = System.IO.File.Create(HostingEnvironment.MapPath(string.Format("~/Commonfolder/{0}.pdf", eWaybillNumber.ToString()))))
                            { // create a new file to write to
                                var contentStream = content.ReadAsStreamAsync(); // get the actual content stream
                                contentStream.Result.CopyTo(file); // copy that stream to the file stream
                            }

                        }
                        else
                        {
                            var jsonString = response.Content.ReadAsStringAsync().Result;
                        }


                    }
                }
                catch (AggregateException err)
                {


                }
            }
            
            
        }

        protected void GrdQuotationPendinSI_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            if (e.Parameters.Split('~')[0] == "GenerateEwaybill")
            {
                DBEngine objDB = new DBEngine();
                authtokensOutput authObj = new authtokensOutput();
                GrdQuotation.JSProperties["cpJson"] = "GenerateEwaybill";

                string id = e.Parameters.Split('~')[1];

                DataSet ds = new DataSet();
                ds = GetSIdetails(id);
                DataTable dtCredential = ds.Tables[5];
                string userid = Convert.ToString(dtCredential.Rows[0]["userid"]);
                string password = Convert.ToString(dtCredential.Rows[0]["password"]);
                string gstin = Convert.ToString(dtCredential.Rows[0]["gstin"]);



                eWayBill objEwaybill = eWayBill.Obj.GetData(ds, "INV");



                if (DateTime.Now > EinvoiceToken.Expiry)
                {
                    try
                    {
                        using (HttpClient client = new HttpClient())
                        {
                            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls |
                                               SecurityProtocolType.Tls11 |
                                               SecurityProtocolType.Tls12;
                            authtokensInput objI = new authtokensInput("shivkumar@peekay.co.in", "PeekaY@.!_123");
                            var json = JsonConvert.SerializeObject(objI, Formatting.Indented);
                            var stringContent = new StringContent(json);
                            var content = new StringContent(stringContent.ToString(), Encoding.UTF8, "application/json");
                            var response = client.PostAsync("https://sandbox.services.vayananet.com/theodore/apis/v1/authtokens", stringContent).Result;

                            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                var jsonString = response;
                                var data = JsonConvert.DeserializeObject<authtokensOutput>(response.Content.ReadAsStringAsync().Result);
                                authObj = response.Content.ReadAsAsync<authtokensOutput>().Result;
                                EinvoiceToken.token = authObj.data.token;
                                long unixDate = authObj.data.expiry;
                                DateTime start = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                                DateTime date = start.AddMilliseconds(unixDate).ToLocalTime();
                                EinvoiceToken.Expiry = date;
                            }
                        }
                    }
                    catch (AggregateException err)
                    {

                    }
                }

                try
                {
                    IRN objIRN = new IRN();
                    using (var client = new HttpClient())
                    {
                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls |
                        SecurityProtocolType.Tls11 |
                        SecurityProtocolType.Tls12;
                        client.DefaultRequestHeaders.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        var json = JsonConvert.SerializeObject(objEwaybill, Formatting.Indented);
                        var stringContent = new StringContent(json);
                        client.DefaultRequestHeaders.Add("X-FLYNN-N-USER-TOKEN", EinvoiceToken.token);
                        client.DefaultRequestHeaders.Add("X-FLYNN-N-ORG-ID", "d16184ae-1699-495c-b276-14c4408e76ba");
                        client.DefaultRequestHeaders.Add("X-FLYNN-N-EWB-GSTIN", gstin);
                        client.DefaultRequestHeaders.Add("X-FLYNN-N-EWB-USERNAME", userid);
                        client.DefaultRequestHeaders.Add("X-FLYNN-N-EWB-PWD", password);
                        client.DefaultRequestHeaders.Add("X-FLYNN-N-EWB-GSP-CODE", "clayfin");
                        var content = new StringContent(stringContent.ToString(), Encoding.UTF8, "application/json");
                        // var response = client.PostAsync("https://solo.enriched-api.vayana.com/basic/einv/v1.0/nic/eicore/v1.03/Invoice", stringContent).Result;
                        var response = client.PostAsync("https://solo.enriched-api.vayana.com/basic/ewb/v1.0/v1.03/gen-ewb", stringContent).Result;

                        if (response.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            var jsonString = response.Content.ReadAsStringAsync().Result;
                            objIRN = response.Content.ReadAsAsync<IRN>().Result;

                            using (var ms = new MemoryStream(Encoding.Unicode.GetBytes(objIRN.data)))
                            {
                                // Deserialization from JSON  
                                DataContractJsonSerializer deserializer = new DataContractJsonSerializer(typeof(IRNDetails));
                                IRNDetails objIRNDetails = (IRNDetails)deserializer.ReadObject(ms);

                                DBEngine objDb = new DBEngine();
                                objDb.GetDataTable("update TBL_TRANS_SALESINVOICE SET EWayBillNumber = '" + objIRNDetails.EwbNo + "',EWayBillDate='" + objIRNDetails.EwbDt + "',EwayBill_ValidTill='" + objIRNDetails.EwbValidTill + "',ISEWAYBILLCANCEL=0 where invoice_id='" + id.ToString() + "'");

                            }



                        }
                        else
                        {

                            EinvoiceError err = new EinvoiceError();
                            var jsonString = response.Content.ReadAsStringAsync().Result;
                            // var data = JsonConvert.DeserializeObject<authtokensOutput>(response.Content.ReadAsStringAsync().Result);
                            err = response.Content.ReadAsAsync<EinvoiceError>().Result;
                            objDB.GetDataTable("DELETE FROM EInvoice_ErrorLog WHERE DOC_ID='" + id.ToString() + "' and DOC_TYPE='SI' and ERROR_TYPE='EWAY_GEN'");
                            if (err.error.type != "ClientRequest")
                            {
                                foreach (errorlog item in err.error.args.irp_error.details)
                                {
                                    objDB.GetDataTable("INSERT INTO EInvoice_ErrorLog(DOC_ID,DOC_TYPE,ERROR_TYPE,ERROR_CODE,ERROR_MSG) VALUES ('" + id.ToString() + "','SI','EWAY_GEN','" + item.ErrorCode + "','" + item.ErrorMessage.Replace("'", "''") + "')");
                                }
                            }
                            else
                            {
                                ClientEinvoiceError cErr = new ClientEinvoiceError();
                                cErr = JsonConvert.DeserializeObject<ClientEinvoiceError>(response.Content.ReadAsStringAsync().Result);
                                if (cErr.error.args.errors != null)
                                {
                                    foreach (string item in cErr.error.args.errors)
                                    {
                                        objDB.GetDataTable("INSERT INTO EInvoice_ErrorLog(DOC_ID,DOC_TYPE,ERROR_TYPE,ERROR_CODE,ERROR_MSG) VALUES ('" + id.ToString() + "','SI','EWAY_GEN','" + "0" + "','" + item + "')");
                                    }
                                }
                                else
                                {
                                    objDB.GetDataTable("INSERT INTO EInvoice_ErrorLog(DOC_ID,DOC_TYPE,ERROR_TYPE,ERROR_CODE,ERROR_MSG) VALUES ('" + id.ToString() + "','SI','EWAY_GEN','" + "0" + "','Invalid request body.')");
                                }

                            }

                           


                           
                        }
                    }
                }
                catch (AggregateException err)
                {


                }
            }
            
        }

        private DataSet GetSIdetails(string id)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("prc_EwayBill");
            proc.AddVarcharPara("@Action", 100, "GetSI");
            proc.AddVarcharPara("@id", 4000, id);
            ds = proc.GetDataSet();
            return ds;
        }
        
    }
}