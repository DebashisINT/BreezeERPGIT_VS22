using CRM.Models;
using CRM.Models.DataContext;
using DevExpress.Web;
using DevExpress.Web.Mvc;
using EntityLayer.CommonELS;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Services;
using UtilityLayer;

namespace CRM.Controllers.CRMS
{
    public class ServiceContactController : Controller
    {
        UserRightsForPage rights = new UserRightsForPage();
        //
        //
        // GET: /ServiceContact/
        crmLeads objdata = new crmLeads();
        public ActionResult Index()
        {

            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSessionMVC("/Index", "ServiceContact");

            ViewBag.CanAdd = rights.CanAdd;
            ViewBag.CanExport = rights.CanExport;
            ViewBag.CanView = rights.CanView;
            ViewBag.CanLiterature = rights.CanLiterature;
            ViewBag.CanCreateActivity = rights.CanSalesActivity;
            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            CRMClassDataContext dcon = new CRMClassDataContext(connectionString);
           
            TempData["DetailsID"] = null;
            TempData["ProductsDetails"] = null;
            var querycrm_cntsource =
                                        (from c in dcon.v_crmContacts
                                         select c);
            objdata.crmcontacts = querycrm_cntsource.ToList();

            var querycrm_users = from c in dcon.V_UserLIsts
                                 select c;
            objdata.Users = querycrm_users.ToList();

            var querycrm_Assign = from c in dcon.V_UserLIsts
                                  select c;
            objdata.Assigners = querycrm_Assign.ToList();


            objdata.OwnerID = Convert.ToInt64(Session["userid"]);
            objdata.Owner = Convert.ToString(System.Web.HttpContext.Current.Session["UserName"]);
            ViewBag.OwnerAssignID = Convert.ToInt64(Session["userid"]);

            //if (ActionType != "ADD")
            //{
            //   if(ID !=null)
            //   {
            //       TempData["DetailsID"] = ID;
            //       objdata.ServiceCnt_Id = Convert.ToString(TempData["DetailsID"]);
            //       TempData.Keep();

            //       if (Convert.ToInt64(objdata.ServiceCnt_Id) > 0)
            //       {
            //           DataTable Servicedata = objdata.GetDetailsEntryListByID("GetEstimateEntryDetailsData", Convert.ToInt64(objdata.ServiceCnt_Id));

            //           if (Servicedata != null && Servicedata.Rows.Count > 0)
            //           {

            //               objdata.OwnerID = Convert.ToInt64(Servicedata.Rows[0]["Service_OwnerId"]);
            //               objdata.Owner = Convert.ToString(Servicedata.Rows[0]["UserName"]);
            //               ViewBag.OwnerAssignID = Convert.ToString(Servicedata.Rows[0]["UserName"]);
            //               objdata.ServiceStatusId = Convert.ToString(Servicedata.Rows[0]["Service_statusId"]);
            //               objdata.RenewalDate = Convert.ToString(Convert.ToDateTime(Servicedata.Rows[0]["Service_RenewalDate"]));
            //               objdata.RenewalStartDate = Convert.ToString(Convert.ToDateTime(Servicedata.Rows[0]["Service_StartDate"]));
            //               objdata.RenewalEndDate = Convert.ToString(Convert.ToDateTime(Servicedata.Rows[0]["Service_EndDate"]));
            //               objdata.ServiceName = Convert.ToString(Servicedata.Rows[0]["Service_Name"]);
            //               objdata.UniqueId = Convert.ToString(Servicedata.Rows[0]["Service_UniqueId"]);
            //               objdata.Customer = Convert.ToString(Servicedata.Rows[0]["Customer"]);
            //               objdata.Customer_ID = Convert.ToString(Servicedata.Rows[0]["CustomerId"]);
            //               //objdata.company_description = Convert.ToString(Servicedata.Rows[0]["Service_Description"]);
            //               objdata.ServiceAmount = Convert.ToDecimal(Convert.ToString(Servicedata.Rows[0]["Service_amount"]));
            //               objdata.ProdServCost = Convert.ToDecimal(Convert.ToString(Servicedata.Rows[0]["Service_ProductCost"]));
            //               objdata.AdditionalCost = Convert.ToDecimal(Convert.ToString(Servicedata.Rows[0]["Service_AddlCost"]));
                          
            //           }

            //       }
            //   }

            //}

            return View(@"~/Views/CRMS/ServiceContact/Index.cshtml", objdata);
        }
        public PartialViewResult PartialServiceContactGrid(string frmdate)
        {

            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSessionMVC("/Index", "ServiceContact");

            ViewBag.CanAdd = rights.CanAdd;
            ViewBag.CanExport = rights.CanExport;
            ViewBag.CanEdit = rights.CanEdit;
            ViewBag.CanDelete = rights.CanDelete;
            ViewBag.CanView = rights.CanView;
            ViewBag.CanLiterature = rights.CanLiterature;
            ViewBag.CanCreateActivity = rights.CanSalesActivity;
            return PartialView(@"~/Views/CRMS/ServiceContact/PartialContactGrid.cshtml", GetServicecontact(frmdate));
        }

        public IEnumerable GetServicecontact(string frmdate)
        {

            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            string Userid = Convert.ToString(Session["userid"]);

                
                if (frmdate != "Ispageload")
                {
                    CRMClassDataContext dc = new CRMClassDataContext(connectionString);
                    // Mantis Issue 24052
                    //var q = from d in dc.v_CRMServiceContacts
                    //        // where Convert.ToString(d.Assign_To) == Userid || Convert.ToString(d.Owner_Id) == Userid
                    //         orderby d.servicecontact_id  descending
                    //        select d;
                    //return q;

                    BusinessLogicLayer.CommonBL ComBL = new BusinessLogicLayer.CommonBL();
                    string ShowServiceContractDataForAllUsers = ComBL.GetSystemSettingsResult("ShowServiceContractDataForAllUsers");

                    if (ShowServiceContractDataForAllUsers.ToUpper() == "NO")
                    {
                        var q = from d in dc.v_CRMServiceContacts
                                where Convert.ToString(d.Assign_Id) == Userid || Convert.ToString(d.Owner_Id) == Userid
                                orderby d.servicecontact_id descending
                                select d;
                        return q;
                    }
                    else
                    {
                        var q = from d in dc.v_CRMServiceContacts
                               orderby d.servicecontact_id descending
                                select d;
                        return q;
                    }
                    // End of Rev Mantis issue 24052 
                }
                else
                {
                    CRMClassDataContext dc = new CRMClassDataContext(connectionString);
                    var q = from d in dc.v_CRMServiceContacts
                            where 1 == 0
                            select d;
                    return q;
                }


        }

        public ActionResult ExportCRMLeadsList(int type)
        {


            switch (type)
            {
                case 1:
                    return GridViewExtension.ExportToPdf(GetEmployeeBatchGridViewSettings(), GetServicecontact("1"));
                //break;
                case 2:
                    return GridViewExtension.ExportToXlsx(GetEmployeeBatchGridViewSettings(), GetServicecontact("1"));
                //break;
                case 3:
                    return GridViewExtension.ExportToXls(GetEmployeeBatchGridViewSettings(), GetServicecontact("1"));
                case 4:
                    return GridViewExtension.ExportToRtf(GetEmployeeBatchGridViewSettings(), GetServicecontact("1"));
                case 5:
                    return GridViewExtension.ExportToCsv(GetEmployeeBatchGridViewSettings(), GetServicecontact("1"));
                //break;

                default:
                    break;
            }

            return null;
        }

        private GridViewSettings GetEmployeeBatchGridViewSettings()
        {
            var settings = new GridViewSettings();
            settings.Name = "gvPaging";
            // Export-specific settings
            settings.SettingsExport.ExportedRowType = GridViewExportedRowType.All;
            settings.SettingsExport.FileName = "ServiceContact";

            settings.Columns.Add(column =>
            {
                column.FieldName = "Service_UniqueId";
                column.Caption = "Unique Id";
                
            });

            settings.Columns.Add(column =>
            {
                column.FieldName = "Service_Name";
                column.Caption = "Name";
              
            });
            settings.Columns.Add(column =>
            {
                column.FieldName = "Customer";
                column.Caption = "Customer";
               
            });
          
            settings.Columns.Add(column =>
            {
                column.FieldName = "StartDate";
                column.Caption = "Start Date";
              
            });
            settings.Columns.Add(column =>
            {
                column.FieldName = "EndDate";
                column.Caption = "End Date";
              
            });
            settings.Columns.Add(column =>
            {
                column.FieldName = "Description";
                column.Caption = "Description";
              
            });
          
            settings.Columns.Add(column =>
            {
                column.FieldName = "Service_Amount";
                column.Caption = "Service Amount";
               
            });
            settings.Columns.Add(column =>
            {
                column.FieldName = "Prod_Serivce_cost";
                column.Caption = "Prod/Serivce cost";
               
            });
            settings.Columns.Add(column =>
            {
                column.FieldName = "Service_AddlCost";
                column.Caption = "Addl. cost";
           
            });
            settings.Columns.Add(column =>
            {
                column.FieldName = "Renewal_Date";
                column.Caption = "Renewal Date";
               
            });
            settings.Columns.Add(column =>
            {
                column.FieldName = "Status";
                column.Caption = "Status";
               
            });


            settings.SettingsExport.PaperKind = System.Drawing.Printing.PaperKind.A4;
            settings.SettingsExport.LeftMargin = 20;
            settings.SettingsExport.RightMargin = 20;
            settings.SettingsExport.TopMargin = 20;
            settings.SettingsExport.BottomMargin = 20;

            return settings;
        }

        public DataTable ToDataTable<T>(List<T> items)
        {

            DataTable dataTable = new DataTable(typeof(T).Name);

            //Get all the properties

            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (PropertyInfo prop in Props)
            {

                //Setting column names as Property names

                dataTable.Columns.Add(prop.Name);

            }

            foreach (T item in items)
            {

                var values = new object[Props.Length];

                for (int i = 0; i < Props.Length; i++)
                {

                    //inserting property values to datatable rows

                    values[i] = Props[i].GetValue(item, null);

                }

                dataTable.Rows.Add(values);

            }

            //put a breakpoint here and check datatable

            return dataTable;

        }

        [HttpPost]

        [ValidateInput(false)]
        public ActionResult SaveServiceContact(crmLeads obj)
        {
            crmLeads CRMLeadsobj = new crmLeads();
            string output = "";
           
            DataTable dt_PRODUCTS = (DataTable)TempData["ProductsDetails"];


            List<CRM.Models.crmLeads.ServiceContactProduct> udt = new List<CRM.Models.crmLeads.ServiceContactProduct>();

            if (dt_PRODUCTS != null && dt_PRODUCTS.Rows.Count > 0)
            {
                foreach (DataRow item in dt_PRODUCTS.Rows)
                {
                    CRM.Models.crmLeads.ServiceContactProduct obj1 = new CRM.Models.crmLeads.ServiceContactProduct();
                    obj1.ProductName = Convert.ToString(item["ProductName"]);
                    obj1.ProductId = Convert.ToString(item["ProductId"]);
                    obj1.ProductQty = Convert.ToString(item["ProductQty"]); ;

                    obj1.Price = Convert.ToString(item["Price"]);
                    obj1.Amount = Convert.ToString(item["Amount"]);
                    obj1.ProductDetailsID = Convert.ToString(item["ProductDetailsID"]);
                    obj1.WarrentyStartdate = Convert.ToString(item["WarrentyStartdate"]);
                    obj1.WarrentyEnddate = Convert.ToString(item["WarrentyEnddate"]);
                   
                    udt.Add(obj1);
                }
            }

            DataTable Bind_PRODUCTS = new DataTable();
            Bind_PRODUCTS = ToDataTable(udt);


            output = CRMLeadsobj.SaveService(obj.ServiceCnt_Id, obj.Action, (obj.OwnerID), obj.AssignedID, obj.ServiceStatusId, obj.RenewalDate, obj.ServiceName, obj.UniqueId, obj.Customer_ID,
                           obj.RenewalStartDate, obj.RenewalEndDate, obj.Servicedesc, obj.ServiceAmount, obj.ProdServCost, obj.AdditionalCost, obj.crmcontacts_id, Bind_PRODUCTS);


            //if (CRMLeadsobj.Action == "Update")
            //{
            //    if (CRMLeadsobj.Status_Id == 13) //Qualify
            //    {
            //        string outputqualified = "";
            //        CRMLeadsobj.Status = "Qualify";
            //        outputqualified = CRMLeadsobj.QualifiedLeads(obj);
            //    }
            //    else if (CRMLeadsobj.Status_Id == 9) //Cancelled/Lost
            //    {
            //        string outputcancelled = "";
            //        CRMLeadsobj.Status = "Cancelled/Lost";
            //        outputcancelled = CRMLeadsobj.LostLeads(obj);
            //    }

            //}

            //if (CRMLeadsobj.Status == "Qualify")
            //{
            //    string outputqualified = "";
            //    outputqualified = CRMLeadsobj.QualifiedLeads(newcrmLeadsobj);
            //}
            //else if (CRMLeadsobj.Status == "Cancelled/Lost")
            //{
            //    string outputcancelled = "";
            //    outputcancelled = CRMLeadsobj.LostLeads(newcrmLeadsobj);
            //}

            return Json(output, JsonRequestBehavior.AllowGet);
        }


        public ActionResult GetServiceProductEntryList()
        {
            CRM.Models.crmLeads.ServiceContactProduct Estimateproductdataobj = new CRM.Models.crmLeads.ServiceContactProduct();
            List<CRM.Models.crmLeads.ServiceContactProduct> Estimateproductdata = new List<CRM.Models.crmLeads.ServiceContactProduct>();
            Int64 DetailsID = 0;

            try
            {
                DataTable dft=new DataTable();
                dft=(DataTable)TempData["ProductsDetails"];
                if ( TempData["DetailsID"] !=null)
                {
                    DetailsID = Convert.ToInt64(TempData["DetailsID"]);
                    TempData.Keep();
                }
                DataTable dt = new DataTable();
                if (DetailsID > 0 && (dft == null || dft.Rows.Count<0))
                {
                    DataTable objData = objdata.GetServiceContactProductEntryListByID("GetServiceCNTProductDetails", DetailsID);
                    if (objData != null && objData.Rows.Count > 0)
                    {
                        dt = objData;
                      
                        DataTable dtable = new DataTable();

                        dtable.Clear();
                        dtable.Columns.Add("HIddenID", typeof(System.Guid));
                        dtable.Columns.Add("ProductName", typeof(System.String));
                        dtable.Columns.Add("ProductId", typeof(System.String));

                        dtable.Columns.Add("ProductQty", typeof(System.String));

                        dtable.Columns.Add("Price", typeof(System.String));
                        dtable.Columns.Add("Amount", typeof(System.String));
                        dtable.Columns.Add("ProductDetailsID", typeof(System.String));
                        dtable.Columns.Add("WarrentyStartdate", typeof(System.String));
                        dtable.Columns.Add("WarrentyEnddate", typeof(System.String));
                        String Gid = "";
                        foreach (DataRow row in dt.Rows)
                        {
                            Gid = Guid.NewGuid().ToString();
                            Estimateproductdataobj = new CRM.Models.crmLeads.ServiceContactProduct();

                            Estimateproductdataobj.ProductName = Convert.ToString(row["ProductName"]);
                            Estimateproductdataobj.ProductId = Convert.ToString(row["ProductId"]);

                            Estimateproductdataobj.ProductQty = Convert.ToString(row["ProductQty"]);

                            Estimateproductdataobj.Price = Convert.ToString(row["Price"]);
                            Estimateproductdataobj.Amount = Convert.ToString(row["Amount"]);

                            Estimateproductdataobj.ProductDetailsID = Convert.ToString(row["ProductDetailsID"]);
                            Estimateproductdataobj.WarrentyStartdate = Convert.ToString(row["WarrentyStartdate"]);
                            Estimateproductdataobj.WarrentyEnddate = Convert.ToString(row["WarrentyEnddate"]);
                            Estimateproductdataobj.HIddenID = Gid;

                            Estimateproductdata.Add(Estimateproductdataobj);


                            object[] trow = { Gid, Convert.ToString(row["ProductName"]),Convert.ToString(row["ProductID"]),
                                                Convert.ToString(row["ProductQty"]),Convert.ToString(row["Price"]),Convert.ToString(row["Amount"]),
                                    Convert.ToString(row["ProductDetailsID"]),Convert.ToString(row["WarrentyStartdate"]),Convert.ToString(row["WarrentyEnddate"]) };
                            dtable.Rows.Add(trow);
                        }
                        //TempData["ProdAddlDesc"] = prodAddlDesc;
                        //TempData.Keep();

                        dt = dtable;

                       
                    }
                }
                else
                {
                    dt = (DataTable)TempData["ProductsDetails"];
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            Estimateproductdataobj = new CRM.Models.crmLeads.ServiceContactProduct();

                            Estimateproductdataobj.ProductName = Convert.ToString(row["ProductName"]);
                            Estimateproductdataobj.ProductId = Convert.ToString(row["ProductId"]);

                            Estimateproductdataobj.ProductQty = Convert.ToString(row["ProductQty"]);

                            Estimateproductdataobj.Price = Convert.ToString(row["Price"]);
                            Estimateproductdataobj.Amount = Convert.ToString(row["Amount"]);


                            Estimateproductdataobj.ProductDetailsID = Convert.ToString(row["ProductDetailsID"]);
                            Estimateproductdataobj.WarrentyStartdate = Convert.ToString(row["WarrentyStartdate"]);
                            Estimateproductdataobj.WarrentyEnddate = Convert.ToString(row["WarrentyEnddate"]);
                            Estimateproductdataobj.HIddenID = Convert.ToString(row["HIddenID"]);
                            Estimateproductdata.Add(Estimateproductdataobj);
                        }
                       
                    }
                }
                TempData["ProductsDetails"] = dt;
                TempData.Keep();

            }
            catch { }
            // return PartialView("~/Views/PMS/Estimate/_BOMProductEntryGrid.cshtml", Estimateproductdata);
            return PartialView("~/Views/CRMS/ServiceContact/ServiceContactProductGrid.cshtml", Estimateproductdata);
        }

        public ActionResult EditServiceContact(crmLeads newcrmLeadsobj)
        {
            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            CRMClassDataContext dcon = new CRMClassDataContext(connectionString);
            //DataTable Servicedata = objdata.GetDetailsEntryListByID("GetEstimateEntryDetailsData", Convert.ToInt64(objdata.ServiceCnt_Id))
            crmLeads CRMLeadsobj = new crmLeads();
            DataSet output = CRMLeadsobj.GetDetailsEntryListByID("GetServiceCNTDetails", Convert.ToInt64(newcrmLeadsobj.ServiceCnt_Id));
            TempData["DetailsID"] = Convert.ToInt64(newcrmLeadsobj.ServiceCnt_Id);
            TempData.Keep();
            CustomerDetails clsObj = new CustomerDetails();
            CRMLeadsobj = APIHelperMethods.ToModel<crmLeads>(output.Tables[0]);
            //clsObj = APIHelperMethods.ToModel<CustomerDetails>(output.Tables[1]);
            CRMLeadsobj.cntids = output.Tables[1].AsEnumerable()
                           .Select(r => r.Field<string>("Contact_id"))
                           .ToList(); ;
           // CRMLeadsobj.custdetails = clsObj;



            //var querycrm_StatusDetails = from c in dcon.v_StatusDetails
            //                             where c.Status_Code != "Close" 
            //                             select c;
            //CRMLeadsobj.Status_Details = querycrm_StatusDetails.ToList();
            //var querycrm_StatusDetails = from c in dcon.v_StatusDetails
            //                             where c.Status_Code == "New" || c.Status_Code == "Cancelled/Lost" || c.Status_Code == "Qualify"
            //                             select c;
            //CRMLeadsobj.Status_Details = querycrm_StatusDetails.ToList();




            return Json(CRMLeadsobj);
        }

        public ActionResult DeleteServiceContact(Int64 Id)
        {
            crmLeads CRMLeadsobj = new crmLeads();
            string outPut = CRMLeadsobj.DeleteService(Id);
            return Json("Data Delete Successfully.");
        }

        [WebMethod]
        public JsonResult DeleteData(string HiddenID)
        {
            DataTable dt = (DataTable)TempData["ProductsDetails"];
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow item in dt.Rows)
                {
                    if (HiddenID.ToString() == item["HIddenID"].ToString())
                    {
                        dt.Rows.Remove(item);
                        break;
                    }
                }
            }

            //if (dt != null && dt.Rows.Count > 0)
            //{
            //    int conut = 1;
            //    foreach (DataRow dr in dt.Rows)
            //    {
            //        dr["SlNO"] = conut;
            //        conut++;
            //    }
            //}

            TempData["ProductsDetails"] = dt;
            TempData.Keep();
            return Json("Data Remove Successfully.");
        }

        public ActionResult AddModeServiceContact()
        {
            TempData["ProductsDetails"] = null;
            TempData["DetailsID"] = null;
            return null;
        }

        public ActionResult PopulateSummaryContactGrid(crmLeads obj)
        {
            //if (TempData["CONTACT_IDs"] != null)
            //{
            //    ViewBag.CONTACT_IDs = TempData["CONTACT_IDs"];
            //}
            return PartialView(@"~/Views/CRMS/ServiceContact/PartialSummaryContLookup.cshtml", GetContactData(obj.Customer_ID));
        }
        public IEnumerable GetContactData(string CustId)
        {
            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            CRMClassDataContext dc = new CRMClassDataContext(connectionString);
            var q = from d in dc.v_crmContacts
                    where Convert.ToString(d.Customer_ID) == CustId
                    orderby d.FirstName
                    select d;
            return q;
        }

        [WebMethod]
        public JsonResult AddProduct(CRM.Models.crmLeads.ServiceContactProduct prod)
        {
            DataTable dt = (DataTable)TempData["ProductsDetails"];
            DataTable dt2 = new DataTable();

            if (dt == null)
            {
                DataTable dtable = new DataTable();

                dtable.Clear();
                dtable.Columns.Add("HIddenID", typeof(System.Guid));
                dtable.Columns.Add("ProductName", typeof(System.String));
                dtable.Columns.Add("ProductId", typeof(System.String));
                dtable.Columns.Add("ProductQty", typeof(System.String));
                dtable.Columns.Add("Price", typeof(System.String));
                dtable.Columns.Add("Amount", typeof(System.String));
                dtable.Columns.Add("ProductDetailsID", typeof(System.String));
                dtable.Columns.Add("WarrentyStartdate", typeof(System.String));
                dtable.Columns.Add("WarrentyEnddate", typeof(System.String));
                object[] trow = { Guid.NewGuid(),prod.ProductName,prod.ProductId,prod.ProductQty,prod.Price,prod.Amount,prod.ProductDetailsID,prod.WarrentyStartdate,
                                prod.WarrentyEnddate};
                dtable.Rows.Add(trow);
                TempData["ProductsDetails"] = dtable;
                TempData.Keep();
            }
            else
            {
                if (string.IsNullOrEmpty(prod.HIddenID))
                {
                    object[] trow = { Guid.NewGuid(), prod.ProductName,prod.ProductId,prod.ProductQty,prod.Price,prod.Amount,prod.ProductDetailsID,prod.WarrentyStartdate,
                                prod.WarrentyEnddate};// Add new parameter Here
                    dt.Rows.Add(trow);
                    TempData["ProductsDetails"] = dt;
                    TempData.Keep();
                }
                else
                {
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow item in dt.Rows)
                        {
                            if (prod.HIddenID.ToString() == item["HIddenID"].ToString())
                            {   item["ProductName"] = prod.ProductName;
                                item["ProductId"] = prod.ProductId;
                               
                                item["ProductQty"] = prod.ProductQty;
                               
                                item["Price"] = prod.Price;
                                item["Amount"] = prod.Amount;
                                
                                item["ProductDetailsID"] = prod.ProductDetailsID;
                                item["WarrentyStartdate"] = prod.WarrentyStartdate;
                                item["WarrentyEnddate"] = prod.WarrentyEnddate;
                            }
                        }
                    }
                }
                TempData["ProductsDetails"] = dt;
                TempData.Keep();
            }


            return Json("");
        }
    }
}