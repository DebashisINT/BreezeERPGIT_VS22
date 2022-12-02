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
    public class CRMCasesController : Controller
    {
        //
        // GET: /CRMCases/
        UserRightsForPage rights;
         crmCases cases=new crmCases();
        public CRMCasesController()
        {
            rights = new UserRightsForPage();
        }
        public ActionResult Index()
        {
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSessionMVC("/Index", "CRMCases");
            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            CRMClassDataContext dcon = new CRMClassDataContext(connectionString);
            crmCases crmCasesobj = new crmCases();

            TempData["ProductsDetails"] = null;
            TempData["DetailsID"] = null;

            ViewBag.CanAdd = rights.CanAdd;
            ViewBag.CanExport =  rights.CanExport;
            ViewBag.CanEdit = rights.CanEdit;
            ViewBag.CanDelete = rights.CanDelete;
            ViewBag.CanView =rights.CanView;
            ViewBag.CanLiterature =rights.CanLiterature;
            ViewBag.CanCreateActivity =  rights.CanSalesActivity;

            var querycrm_users = (from c in dcon.V_UserLIsts
                                  select c);

            //var querycrm_ParentCases = (from c in dcon.V_CaseLists
            //                      select c);
          
            

            



           // crmCasesobj.V_ORIGINS = querycrm_origin.ToList();

            crmCasesobj.Users = querycrm_users.ToList();
            //crmCasesobj.CasesList = querycrm_ParentCases.ToList();
            
            crmCasesobj.OWNER_ID = Convert.ToInt64(Session["userid"]);
            crmCasesobj.ASSIGNED_ID = Convert.ToInt64(Session["userid"]);
            ViewBag.OwnerAssignID = Convert.ToInt64(Session["userid"]);
            return View(@"~/Views/CRMS/Cases/Cases.cshtml", crmCasesobj);
        }

        public ActionResult AddModeServiceContact()
        {
            TempData["ProductsDetails"] = null;
            TempData["DetailsID"] = null;
            return null;
        }
        public ActionResult DeleteServiceContact(Int64 Id)
        {
            crmCases CRMLeadsobj = new crmCases();
            string outPut = CRMLeadsobj.DeleteService(Id);
            return Json("Data Delete Successfully.");
        }
        public ActionResult SaveCase(crmCases crmcaseobj)
        {
            crmCases cntObj = new crmCases();

            DataTable dt_PRODUCTS = (DataTable)TempData["ProductsDetails"];


            List<CRM.Models.crmCases.ServiceContactProduct> udt = new List<CRM.Models.crmCases.ServiceContactProduct>();

            if (dt_PRODUCTS != null && dt_PRODUCTS.Rows.Count > 0)
            {
                foreach (DataRow item in dt_PRODUCTS.Rows)
                {
                    CRM.Models.crmCases.ServiceContactProduct obj1 = new CRM.Models.crmCases.ServiceContactProduct();
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


            string Output = cntObj.SaveCase(crmcaseobj.CASE_ID, crmcaseobj.Action, crmcaseobj.OWNER_ID, crmcaseobj.ASSIGNED_ID, crmcaseobj.STATUS_ID, crmcaseobj.STATUS_DATE, crmcaseobj.EST_CLOSE_DATE,
                crmcaseobj.TITLE, crmcaseobj.CODE, crmcaseobj.SUBJECTS, crmcaseobj.Customer_ID, crmcaseobj.ORIGIN_ID, crmcaseobj.crmcontacts_id, crmcaseobj.crmParentCase_id,
                crmcaseobj.RESPONSED_BY, crmcaseobj.ResponseSent, crmcaseobj.RESOLVED_BY, crmcaseobj.Escalated_DATE, crmcaseobj.EsCalatedTo, crmcaseobj.ADDRESS1,crmcaseobj.ADDRESS2,
                crmcaseobj.ADDRESS3, crmcaseobj.PIN_ID, crmcaseobj.CITY_ID, crmcaseobj.STATE_ID, crmcaseobj.COUNTRY_ID, Bind_PRODUCTS);

            return Json(Output, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PopulateSummaryContactGrid(crmCases obj)
        {

            return PartialView(@"~/Views/CRMS/Cases/PartialContactLookup.cshtml", GetContactData(obj.Customer_ID));
        }
        public ActionResult ExportCRMContactList(int type)
        {


            switch (type)
            {
                case 1:
                    return GridViewExtension.ExportToPdf(GetEmployeeBatchGridViewSettings(), GetSalesSummary("1"));
                //break;
                case 2:
                    return GridViewExtension.ExportToXlsx(GetEmployeeBatchGridViewSettings(), GetSalesSummary("1"));
                //break;
                case 3:
                    return GridViewExtension.ExportToXls(GetEmployeeBatchGridViewSettings(), GetSalesSummary("1"));
                case 4:
                    return GridViewExtension.ExportToRtf(GetEmployeeBatchGridViewSettings(), GetSalesSummary("1"));
                case 5:
                    return GridViewExtension.ExportToCsv(GetEmployeeBatchGridViewSettings(), GetSalesSummary("1"));
                //break;

                default:
                    break;
            }

            return null;
        }


        private GridViewSettings GetEmployeeBatchGridViewSettings()
        {
            var settings = new GridViewSettings();
            settings.Name = "gridcrmCases";
            // Export-specific settings
            settings.SettingsExport.ExportedRowType = GridViewExportedRowType.All;
            settings.SettingsExport.FileName = "Cases";


            settings.Columns.Add(x =>
            {
                x.Caption = "Unique ID";
                x.FieldName = "CODE";
                
            });

            settings.Columns.Add(x =>
            {
                x.Caption = "Title";
                x.FieldName = "TITLE";
              
            });
            settings.Columns.Add(x =>
            {
                x.Caption = "Subject";
                x.FieldName = "SUBJECTS";
              
            });

            settings.Columns.Add(x =>
            {
                x.Caption = "Customer";
                x.FieldName = "CUST_NAME";
             

            });
            settings.Columns.Add(x =>
            {
                x.Caption = "Origin";
                x.FieldName = "ORIGIN_NAME";
        

            });




            settings.Columns.Add(x =>
            {
                x.Caption = "Responsed By";
                x.FieldName = "RESP_BY";
             

            });


            settings.Columns.Add(x =>
            {
                x.Caption = "Resolved By";
                x.FieldName = "RESV_BY";
               

            });



            settings.Columns.Add(x =>
            {
                x.Caption = "Address 1";
                x.FieldName = "ADDRESS1";
             

            });



            settings.Columns.Add(x =>
            {
                x.Caption = "Address 2";
                x.FieldName = "ADDRESS2";
              

            });



            settings.Columns.Add(x =>
            {
                x.Caption = "Address 3";
                x.FieldName = "ADDRESS3";
              

            });
            settings.Columns.Add(x =>
            {
                x.Caption = "Landmark";
                x.FieldName = "LANDMARK";
         

            });


            settings.Columns.Add(x =>
            {
                x.Caption = "Postal/Zip Code";
                x.FieldName = "PIN_CODE";
                

            });


            settings.Columns.Add(x =>
            {
                x.Caption = "City";
                x.FieldName = "CITY";
             

            });



            settings.Columns.Add(x =>
            {
                x.Caption = "State";
                x.FieldName = "STATE";
          
             });





            settings.Columns.Add(x =>
            {
                x.Caption = "Country";
                x.FieldName = "COUNTRY";
            

            });




            settings.Columns.Add(x =>
            {
                x.Caption = "Status";
                x.FieldName = "STATUS_ID";
           

            });



            settings.Columns.Add(x =>
            {
                x.Caption = "Status Date";
                x.FieldName = "STATUS_DATE";
            

            });

            settings.Columns.Add(x =>
            {
                x.Caption = "Est. Close Date";
                x.FieldName = "EST_CLOSE_DATE";
     

            });


            settings.Columns.Add(x =>
            {
                x.Caption = "Parent Case";
                x.FieldName = "PARENT_ID";
                

            });




            settings.Columns.Add(x =>
            {
                x.Caption = "Owner";
                x.FieldName = "OWNER_NAME";
           


            });


            settings.Columns.Add(x =>
            {
                x.Caption = "Assign User";
                x.FieldName = "ASSIGNED_TO";
           

            });

            settings.Columns.Add(x =>
            {
                x.Caption = "Escalated Date";
                x.FieldName = "Escalated_DATE";
              

            });

            settings.Columns.Add(x =>
            {
                x.Caption = "First Response Sent";
                x.FieldName = "First_Response_Sent";
               

            });


            settings.Columns.Add(x =>
            {
                x.Caption = "Created By";
                x.FieldName = "CREATED_BY";
        

            });

            settings.Columns.Add(x =>
            {
                x.Caption = "Created On";
                x.FieldName = "CREATED_ON";
           

            });


            settings.Columns.Add(x =>
            {
                x.Caption = "Modified By";
                x.FieldName = "MODIFIED_BY";
     

            });
            settings.Columns.Add(x =>
            {
                x.Caption = "Modified On";
                x.FieldName = "MODIFIED_ON";
             

            });


            settings.SettingsExport.PaperKind = System.Drawing.Printing.PaperKind.A4;
            settings.SettingsExport.LeftMargin = 20;
            settings.SettingsExport.RightMargin = 20;
            settings.SettingsExport.TopMargin = 20;
            settings.SettingsExport.BottomMargin = 20;

            return settings;
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

        public ActionResult PopulateSummaryParentCaseGrid(crmCases obj)
        {
            Int64 CaseValue = 0;
            if (Convert.ToString(obj.CASE_ID) == "0" || Convert.ToString(obj.CASE_ID) == "")
            {
                CaseValue = 0;
            }
            else
            {
                CaseValue = Convert.ToInt64(obj.CASE_ID);
            }
            return PartialView(@"~/Views/CRMS/Cases/PartialParentCaseLookup.cshtml", GetParentCase(obj.Customer_ID, CaseValue));
        }

        public IEnumerable GetParentCase(string CustId, Int64 CASE_ID)
        {
            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            CRMClassDataContext dc = new CRMClassDataContext(connectionString);
            if (CASE_ID != 0)
            {
                var q = from d in dc.V_CaseLists
                        where Convert.ToString(d.CUSTOMER_ID) == CustId &&  d.CASE_ID !=Convert.ToInt64(CASE_ID)
                        orderby d.CASE_ID descending
                        select d;
                return q;
            }
            else
            {
                var q = from d in dc.V_CaseLists
                        where Convert.ToString(d.CUSTOMER_ID) == CustId
                        orderby d.CASE_ID descending
                        select d;
                return q;
            }
        }
        public ActionResult SetAddressDetailsBasedInCustomer(crmCases objAddr)
        {

            crmCases CRMLeadsobj = new crmCases();
            DataTable output = CRMLeadsobj.GetCustomerAddress("GetCustomerAddress", Convert.ToString(objAddr.Customer_ID));


            CRMLeadsobj = APIHelperMethods.ToModel<crmCases>(output);
          





            return Json(CRMLeadsobj);
        }


        public ActionResult EditServiceContact(crmCases newcrmLeadsobj)
        {
            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            CRMClassDataContext dcon = new CRMClassDataContext(connectionString);
            //DataTable Servicedata = objdata.GetDetailsEntryListByID("GetEstimateEntryDetailsData", Convert.ToInt64(objdata.ServiceCnt_Id))
            crmCases CRMLeadsobj = new crmCases();
            DataSet output = CRMLeadsobj.GetDetailsEntryListByID("GetServiceCNTDetails", Convert.ToInt64(newcrmLeadsobj.CASE_ID));
            TempData["DetailsID"] = Convert.ToInt64(newcrmLeadsobj.CASE_ID);
            TempData.Keep();
            CustomerDetails clsObj = new CustomerDetails();
            CRMLeadsobj = APIHelperMethods.ToModel<crmCases>(output.Tables[0]);
            //clsObj = APIHelperMethods.ToModel<CustomerDetails>(output.Tables[1]);
            CRMLeadsobj.cntids = output.Tables[1].AsEnumerable()
                           .Select(r => r.Field<string>("Contact_id"))
                           .ToList();
            CRMLeadsobj.ParentCaseids = output.Tables[2].AsEnumerable()
                           .Select(r => r.Field<string>("PARENT_ID"))
                           .ToList();





            return Json(CRMLeadsobj);
        }

        public ActionResult PopulateGrid(string frmdate)
        {
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSessionMVC("/Index", "CRMCases");
            ViewBag.CanAdd = rights.CanAdd;
            ViewBag.CanExport =  rights.CanExport;
            ViewBag.CanEdit = rights.CanEdit;
            ViewBag.CanDelete =  rights.CanDelete;
            ViewBag.CanView =  rights.CanView;
            ViewBag.CanLiterature = rights.CanLiterature;
            ViewBag.CanCreateActivity = rights.CanSalesActivity;
            return PartialView(@"~/Views/CRMS/Cases/_PartialListing.cshtml", GetSalesSummary(frmdate));
        }
        public IEnumerable GetSalesSummary(string frmdate)
        {

            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            string Userid = Convert.ToString(Session["userid"]);

            if (frmdate != "Ispageload")
            {
                CRMClassDataContext dc = new CRMClassDataContext(connectionString);
                // Mantis Issue 24054
                //var q = from d in dc.V_CRMCASEs
                //        //where Convert.ToString(d.ASSIGNED_TO) == Userid || Convert.ToString(d.OWNER_NAME) == Userid
                //        orderby d.CASE_ID descending
                //        select d;
                //return q;

                BusinessLogicLayer.CommonBL ComBL = new BusinessLogicLayer.CommonBL();
                string ShowCasesDataForAllUsers = ComBL.GetSystemSettingsResult("ShowCasesDataForAllUsers");

                if (ShowCasesDataForAllUsers.ToUpper() == "NO")
                {
                    var q = from d in dc.V_CRMCASEs
                            where Convert.ToString(d.ASSIGNED_ID) == Userid || Convert.ToString(d.OWNER_ID) == Userid
                            orderby d.CASE_ID descending
                            select d;
                    return q;
                }
                else
                {
                    var q = from d in dc.V_CRMCASEs
                            orderby d.CASE_ID descending
                            select d;
                    return q;
                }
                // End of Rev Mantis issue 24054  
            }
            else
            {
                CRMClassDataContext dc = new CRMClassDataContext(connectionString);
                var q = from d in dc.V_CRMCASEs
                        where 1 == 0
                        select d;
                return q;
            }



        }

        public ActionResult EditCase(crmCases newCRMcasesobj)
        {
            crmCases crmCasesobj = new crmCases();
            DataSet output = crmCasesobj.EditCase(newCRMcasesobj);
            newCRMcasesobj = APIHelperMethods.ToModel<crmCases>(output.Tables[0]);
            return Json(newCRMcasesobj);
        }

        public ActionResult DeleteCase(crmCases newCRMCasesobj)
        {
            crmCases crmContactobj = new crmCases();
            string output = crmContactobj.DeleteCase(newCRMCasesobj);
            return Json(output);
        }

        public ActionResult GetServiceProductEntryList()
        {
            CRM.Models.crmCases.ServiceContactProduct Estimateproductdataobj = new CRM.Models.crmCases.ServiceContactProduct();
            List<CRM.Models.crmCases.ServiceContactProduct> Estimateproductdata = new List<CRM.Models.crmCases.ServiceContactProduct>();
            Int64 DetailsID = 0;
           
            try
            {
                DataTable dft = new DataTable();
                dft = (DataTable)TempData["ProductsDetails"];
                if (TempData["DetailsID"] != null)
                {
                    DetailsID = Convert.ToInt64(TempData["DetailsID"]);
                    TempData.Keep();
                }
                DataTable dt = new DataTable();
                if (DetailsID > 0 && (dft == null || dft.Rows.Count < 0))
                {
                    DataTable objData = new DataTable();
                    objData = cases.GetServiceContactProductEntryListByID("GetServiceCNTProductDetails", DetailsID);
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
                            Estimateproductdataobj = new CRM.Models.crmCases.ServiceContactProduct();

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
                            Estimateproductdataobj = new CRM.Models.crmCases.ServiceContactProduct();

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
            return PartialView("~/Views/CRMS/Cases/PartialProductDetailsGrid.cshtml", Estimateproductdata);
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


            TempData["ProductsDetails"] = dt;
            TempData.Keep();
            return Json("Data Remove Successfully.");
        }

        [WebMethod]
        public JsonResult AddProduct(CRM.Models.crmCases.ServiceContactProduct prod)
        {
            DataTable dt = (DataTable)TempData["ProductsDetails"];
            DataTable dt2 = new DataTable();

            if (dt == null || dt.Rows.Count == 0)
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
                            {
                                item["ProductName"] = prod.ProductName;
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

